using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WinsockPacketEditor
{
    public class ProxySession : AppSession<ProxySession, BinaryRequestInfo>
    {
        public Socket TargetSocket = null;
        public byte[] bBuffer = null;
        public string ClientIP = string.Empty;
        public int ClientPort = 0;
        public string ServerIP = string.Empty;
        public int ServerPort = 0;
        public Guid AID = Guid.Empty;

        /*
            设备身份（2026-09-14）。
            DeviceKey：认证成功后占的设备槽的键 —— WPC 连接是设备指纹，普通连接是 "ip:" + 源 IP；
                       null 表示还没认证。OnSessionClosed 按它释放。
            DeviceId / WpcVersion / WpcOs：只有 WPC 连接有（令牌认证的数据连接、或控制连接本身）。
            WpcLastFrame：控制连接最后一次收到帧的时间，WPCConfig.Device.SweepControlSessions 按它判活。
            IsWpcControl / WpcToken：这条连接是 WPC 的控制连接（私有方法 0x80），令牌由 WPCConfig.Device 签发。
        */
        public string DeviceKey = null;
        public string DeviceId = null;
        public string WpcVersion = null;
        public string WpcOs = null;
        public DateTime WpcLastFrame = DateTime.MinValue;
        public bool IsWpcControl = false;
        public string WpcToken = null;

        public string ServerAddress = string.Empty;
        public string ClientAddress = string.Empty;
        public Operate.ProxyConfig.Proxy.ProxyStep ProxyStep;
        public Operate.ProxyConfig.Proxy.CommandType CommandType;
        public Operate.ProxyConfig.Proxy.AddressType AddressType;
        public Operate.ProxyConfig.Proxy.DomainType DomainType;

        /*
            HTTP 结构化嗅探（2026-09-23）：只在 DomainType == HTTP（端口 80/8080）的会话上启用，按方向各一个。
            拼出来的完整请求/响应按 HTTP_Req / HTTP_Resp 入列表，替代逐段 TCP 条目（只影响展示，不改线上字节）。
        */
        internal HttpSniffer HttpReqSniffer;
        internal HttpSniffer HttpRespSniffer;

        /// <summary>取（或建）本会话某个方向的 HTTP 嗅探器；非 HTTP 会话返回 null。</summary>
        internal HttpSniffer Sniffer(bool request)
        {
            if (DomainType != Operate.ProxyConfig.Proxy.DomainType.HTTP) { return null; }

            if (request)
            {
                if (HttpReqSniffer == null) { HttpReqSniffer = new HttpSniffer(true); }
                return HttpReqSniffer;
            }

            if (HttpRespSniffer == null) { HttpRespSniffer = new HttpSniffer(false); }
            return HttpRespSniffer;
        }

        public Operate.ProxyConfig.Proxy.ProxyType ProxyType { get; internal set; }

        public new ProxyAppServer AppServer
        {
            get
            {
                return (ProxyAppServer)base.AppServer;
            }
        }

        #region//初始化

        protected override void OnSessionStarted()
        {
            try
            {
                base.OnSessionStarted();

                this.ClientIP = this.RemoteEndPoint.Address.ToString();
                this.ClientPort = this.RemoteEndPoint.Port;

                /*
                    关掉 Nagle。SuperSocket 1.6 自己不设 NoDelay，而这条连接上回给客户端的
                    是网游的小包：「头 + 体」两次写会撞上对端的延迟 ACK，Windows 上最坏 200 ms。
                    目标那头的套接字在 EnsureTargetSocket 里同样关。
                */
                Socket client = this.SocketSession?.Client;
                if (client != null) { client.NoDelay = true; }

                /*
                    目标侧的套接字与缓冲<b>不在这里建</b>，等 CONNECT 真的要连目标时再建
                    （EnsureTargetSocket）。理由：
                      · 认证失败 / 扫描 / UDP 关联 / WPC 控制连接都用不着它们，
                        以前每条连接一进门就租 64 KB + 一个套接字，5000 次拨号就是 320 MB；
                      · 地址族要等目标地址解析出来才知道，以前写死 InterNetwork，IPv6 目标直接连不上。
                */
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnSessionStarted), ex);
            }
        }

        internal protected new void SetNextReceiveFilter(IReceiveFilter<BinaryRequestInfo> receiveFilter)
        {
            base.SetNextReceiveFilter(receiveFilter);
        }

        /// <summary>
        /// 按目标地址族建目标套接字并租接收缓冲；已经有了就什么都不做。
        /// 只在会话自己的异步流程里调（ConnectToTarget / ConnectToEXTProxyServer），没有并发。
        /// </summary>
        public void EnsureTargetSocket(AddressFamily Family)
        {
            if (this.TargetSocket == null)
            {
                this.TargetSocket = new Socket(Family, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
            }

            if (this.bBuffer == null || this.bBuffer.Length == 0)
            {
                this.bBuffer = Operate.ProxyConfig.Proxy.RequestProxyBuffer(Operate.ProxyConfig.Proxy.ProxyReceiveBufferSize);
            }
        }

        /// <summary>
        /// 数据面往客户端发。<b>不能用裸 TrySend</b>：它在发送队列满（SendingQueueSize=100）时
        /// 返回 false 并丢掉这一段，TCP 流就错位了 —— 客户端下行慢于目标上行（弱网、下载补丁）时
        /// 必然出现。这里退到阻塞的 Send()（受 SendTimeOut 约束），发不出去就关会话，宁可断也不错位。
        /// 握手 / 认证的两字节应答仍用 TrySend，那时队列不可能满。
        /// </summary>
        public void SendToClient(byte[] Data, int Offset, int Length)
        {
            if (Data == null || Length <= 0) { return; }

            if (this.TrySend(Data, Offset, Length)) { return; }

            try
            {
                this.Send(Data, Offset, Length);
            }
            catch (TimeoutException)
            {
                Operate.DoLog(nameof(SendToClient), string.Format("发送队列持续满，客户端接收过慢，关闭会话 [ {0}:{1} ]", this.ClientIP, this.ClientPort));
                this.Close(CloseReason.TimeOut);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(SendToClient), ex);
                this.Close(CloseReason.SocketError);
            }
        }

        #endregion

        #region//帧队列：过滤器切出来的帧按顺序处理

        /*
            过滤器（Socks5ProxyReceiveFilter）每切出一帧就经 SocksProxyServer 送到 OnFrame，跑在 SuperSocket 的接收线程上，
            同一条会话不会并发进来（SuperSocket 一次只挂一个接收）。

            握手 / 认证 / 命令三步的处理是异步的（DNS、连目标），不能在接收线程上等，
            所以入队后由<b>一个</b>排空任务顺序处理；转发帧在没有排空任务在跑、队列也空时<b>内联</b>处理 ——
            那条路是同步的（目标侧 Socket.Send），阻塞接收线程正是天然的背压：目标收不动时客户端这边也就收不动了。
            命令还在连目标时客户端就发过来的数据（紧接 CONNECT 的那种）会排在命令帧后面，连上再按顺序转出去。
        */
        private readonly Queue<BinaryRequestInfo> frames = new Queue<BinaryRequestInfo>();
        private readonly object frameGate = new object();
        private bool draining = false;

        /// <summary>转发阶段拆包用的残片（Enable_UnPack 时 ProcessForwardData 往里存半个包）。</summary>
        public byte[] ForwardBuffer = Array.Empty<byte>();

        internal void OnFrame(BinaryRequestInfo frame)
        {
            if (frame == null) { return; }

            bool inline;
            lock (this.frameGate)
            {
                inline = !this.draining && this.frames.Count == 0 && frame.Key == Socks5ProxyReceiveFilter.KeyForward;
                if (!inline)
                {
                    this.frames.Enqueue(frame);
                    if (!this.draining)
                    {
                        this.draining = true;
                        _ = this.DrainFramesAsync();
                    }
                }
            }

            if (inline)
            {
                this.ForwardFrame(frame.Body);
            }
        }

        private async Task DrainFramesAsync()
        {
            while (true)
            {
                BinaryRequestInfo frame;
                lock (this.frameGate)
                {
                    if (this.frames.Count == 0 || !this.Connected)
                    {
                        this.frames.Clear();
                        this.draining = false;
                        return;
                    }
                    frame = this.frames.Dequeue();
                }

                try
                {
                    await this.HandleFrame(frame);
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DrainFramesAsync), ex);
                }
            }
        }

        private async Task HandleFrame(BinaryRequestInfo frame)
        {
            switch (frame.Key)
            {
                case Socks5ProxyReceiveFilter.KeyHandshake:
                    await Operate.ProxyConfig.Proxy.Handshake(this, frame.Body);
                    break;

                case Socks5ProxyReceiveFilter.KeyAuth:
                    await Operate.ProxyConfig.Proxy.AuthUserName(this, frame.Body);
                    break;

                case Socks5ProxyReceiveFilter.KeyCommand:
                    await Operate.ProxyConfig.Proxy.Command(this, frame.Body);
                    break;

                case Socks5ProxyReceiveFilter.KeyForward:
                    this.ForwardFrame(frame.Body);
                    break;

                case Socks5ProxyReceiveFilter.KeyControl:
                    await Operate.WPCConfig.Device.HandleControlFrame(this, frame.Body);
                    break;

                default:
                    this.HandleUnknownRequest(frame);
                    break;
            }
        }

        private void ForwardFrame(byte[] body)
        {
            //命令那一步失败（连不上目标）时会话已经在关了，后面排着的数据帧直接丢
            if (this.ProxyStep != Operate.ProxyConfig.Proxy.ProxyStep.ForwardData || !this.Connected) { return; }
            Operate.ProxyConfig.Proxy.ProcessForwardData(this, body, ref this.ForwardBuffer);
        }

        #endregion

        #region//连接远程服务器（异步）

        public async Task ConnectToTarget(string TargetIP, int TargetPort)
        {
            try
            {
                //地址族跟着目标走：IPv6 目标要 InterNetworkV6 的套接字（以前写死 IPv4，v6 目标必 Unreachable）
                AddressFamily family = AddressFamily.InterNetwork;
                if (IPAddress.TryParse(TargetIP, out IPAddress parsed) && parsed.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    family = AddressFamily.InterNetworkV6;
                }

                this.EnsureTargetSocket(family);

                Socket targetSocket = this.TargetSocket;
                if (targetSocket == null)
                {
                    Operate.ProxyConfig.Proxy.SendCommandResponse(this, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                    this.Close(CloseReason.SocketError);
                    return;
                }

                await targetSocket.ConnectAsync(TargetIP, TargetPort);

                this.ServerIP = TargetIP;
                this.ServerPort = TargetPort;

                Operate.ProxyConfig.Proxy.SendCommandResponse(this, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Success);
                this.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
                this.StartReceivingFromTarget();
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception ex)
            {
                Operate.ProxyConfig.Proxy.SendCommandResponse(this, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.Close(CloseReason.SocketError);
                Operate.DoLog(nameof(ConnectToTarget), ex);
            }
        }        

        #endregion

        #region//连接外部代理服务器（异步）

        public async Task ConnectToEXTProxyServer(byte[] bData)
        {
            try
            {
                this.ServerIP = Operate.ProxyConfig.Proxy.ExternalProxy_IP;
                this.ServerPort = Operate.ProxyConfig.Proxy.ExternalProxy_Port;

                //外部代理地址是 IPv6 字面量才用 v6 套接字；域名由 EstablishSocksProxyServer 解析，它优先取 IPv4
                AddressFamily family = AddressFamily.InterNetwork;
                if (IPAddress.TryParse(this.ServerIP, out IPAddress parsed) && parsed.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    family = AddressFamily.InterNetworkV6;
                }
                this.EnsureTargetSocket(family);

                var Establish = await Operate.ProxyConfig.Proxy.EstablishSocksProxyServer(
                    this.TargetSocket,
                    Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth,
                    Operate.ProxyConfig.Proxy.ExternalProxy_IP,
                    Operate.ProxyConfig.Proxy.ExternalProxy_Port,
                    Operate.ProxyConfig.Proxy.ExternalProxy_UserName,
                    Operate.ProxyConfig.Proxy.ExternalProxy_PassWord,
                    bData);

                if (!Establish.Success || Establish.Response[1] != 0x00)
                {
                    this.Close(CloseReason.ServerClosing);
                    Operate.ProxyConfig.Proxy.SendCommandResponse(this, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Fault);
                    return;
                }                

                Operate.ProxyConfig.Proxy.SendCommandResponse(this, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Success);
                this.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
                this.StartReceivingFromTarget();
            }
            catch (Exception ex)
            {
                Operate.ProxyConfig.Proxy.SendCommandResponse(this, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.Close(CloseReason.SocketError);
                Operate.DoLog(nameof(ConnectToEXTProxyServer), ex);
            }
        }        

        #endregion

        #region//处理 TCP 响应数据

        private void StartReceivingFromTarget()
        {
            try
            {
                Socket targetSocket = this.TargetSocket;
                if (targetSocket == null || !targetSocket.Connected)
                {
                    return;
                }

                targetSocket.BeginReceive(this.bBuffer, 0, this.bBuffer.Length, SocketFlags.None, OnTargetDataReceived, null);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.NotConnected || ex.SocketErrorCode == SocketError.ConnectionReset)
            {
                this.Close(CloseReason.SocketError);
            }
            catch (Exception ex)
            {
                this.Close(CloseReason.SocketError);
                Operate.DoLog(nameof(StartReceivingFromTarget), ex);
            }
        }

        private void OnTargetDataReceived(IAsyncResult ar)
        {
            try
            {
                Socket targetSocket = this.TargetSocket;
                if (targetSocket == null || !targetSocket.Connected)
                {
                    return;
                }

                int bytesRead = targetSocket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    byte[] bData = this.bBuffer.AsSpan(0, bytesRead).ToArray();

                    if (this.CommandType != Operate.ProxyConfig.Proxy.CommandType.Connect)
                    {
                        return;
                    }

                    if (!Operate.ProxyConfig.Proxy.HookTCP_Resp)
                    {
                        this.SendToClient(bData, 0, bData.Length);
                        this.StartReceivingFromTarget();
                        return;
                    }

                    if (Operate.ProxyConfig.Proxy.Enable_UnPack)
                    {
                        byte[][] packets = Operate.ProxyConfig.Proxy.ProcessResponseData(bData);
                        foreach (byte[] packet in packets)
                        {
                            if (packet.Length > 0)
                            {
                                Operate.FilterConfig.Filter.DoFilter_SOCKS_TCP(this, packet.AsSpan(), Operate.PacketConfig.Packet.PacketType.TCP_Resp);
                                Operate.ProxyConfig.Account.AddTraffic(this.AID, this.ClientIP, packet.Length);
                            }
                        }
                    }
                    else
                    {
                        Operate.FilterConfig.Filter.DoFilter_SOCKS_TCP(this, bData.AsSpan(), Operate.PacketConfig.Packet.PacketType.TCP_Resp);
                        Operate.ProxyConfig.Account.AddTraffic(this.AID, this.ClientIP, bData.Length);
                    }

                    this.StartReceivingFromTarget();
                }
                else
                {
                    this.Close(CloseReason.ServerClosing);
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.NotConnected || ex.SocketErrorCode == SocketError.ConnectionReset)
            {
                this.Close(CloseReason.SocketError);
            }
            catch (Exception ex)
            {
                this.Close(CloseReason.SocketError);
                Operate.DoLog(nameof(OnTargetDataReceived), ex);
            }
        }

        #endregion

        #region//发送和接收 UDP 数据

        public int SendUdpData(Socket clientSocket, ReadOnlySpan<byte> bData, IPEndPoint ep)
        {
            int iReturn = 0;

            try
            {
                if (clientSocket != null && !bData.IsEmpty && ep != null)
                {
                    byte[] sendBuffer = ArrayPool<byte>.Shared.Rent(bData.Length);
                    bData.CopyTo(sendBuffer);

                    iReturn = clientSocket.SendTo(sendBuffer, 0, bData.Length, SocketFlags.None, ep);

                    ArrayPool<byte>.Shared.Return(sendBuffer);
                }
            }
            catch
            {
                //忽略错误
            }

            return iReturn;
        }

        public void StartUdpReceive(ProxyUDP pu)
        {
            if (pu == null || pu.ClientSocket == null || !pu.ClientSocket.IsBound || !pu.IsActive)
            {
                return;
            }

            try
            {
                SocketAsyncEventArgs socketEventArgs = Operate.ProxyConfig.Proxy.SocketAsyncEventArgsPoolManager.Get("UDPRelay");
                socketEventArgs.UserToken = pu;

                byte[] buffer = ArrayPool<byte>.Shared.Rent(65535);
                socketEventArgs.SetBuffer(buffer, 0, buffer.Length);
                socketEventArgs.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                socketEventArgs.Completed += UdpReceiveCompleted;

                if (!pu.ClientSocket.ReceiveFromAsync(socketEventArgs))
                {
                    ProcessUdpReceive(socketEventArgs);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StartUdpReceive), ex);
            }
        }

        public void UdpReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessUdpReceive(e);
        }

        private void ProcessUdpReceive(SocketAsyncEventArgs e)
        {
            ProxyUDP pu = e.UserToken as ProxyUDP;

            if (pu == null || !pu.IsActive || pu.ClientSocket == null)
            {
                this.ReturnUdpSocketEventArgs(e);
                return;
            }

            try
            {
                if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
                {
                    this.ReturnUdpSocketEventArgs(e);
                    return;
                }

                IPEndPoint epRemote = e.RemoteEndPoint as IPEndPoint;
                if (epRemote == null || epRemote.Address.Equals(IPAddress.Any) || epRemote.Port == 0)
                {
                    this.ReturnUdpSocketEventArgs(e);
                    return;
                }

                Span<byte> bData = e.Buffer.AsSpan(e.Offset, e.BytesTransferred);

                /*
                    分辨「客户端发来的请求」与「目标服务器发回的应答」。
                    以前只看前三个字节是不是 00 00 00：目标的负载恰好以三个 0 开头时（游戏协议里不少见）会被当成请求再转出去。
                    现在认过客户端之后按来源比：端点对得上就是客户端；同一个 IP 换了端口（手机网络 NAT 重新映射）
                    且带着 SOCKS5 UDP 头的也算客户端。还没认过客户端时（第一包）照旧看包头（2026-09-14）。
                */
                bool zeroHeader = bData.Length >= 3 && bData[0] == 0 && bData[1] == 0 && bData[2] == 0;
                IPEndPoint known = pu.ClientEndPoint;
                bool fromClient = known == null ? zeroHeader : (epRemote.Equals(known) || (zeroHeader && epRemote.Address.Equals(known.Address)));

                if (fromClient)
                {
                    Operate.ProxyConfig.Proxy.ProcessUdpRequest(this, pu, epRemote, bData);
                }
                else
                {
                    Operate.ProxyConfig.Proxy.ProcessUdpResponse(this, pu, epRemote, bData);
                }

                e.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                if (pu.IsActive && pu.ClientSocket != null && !pu.ClientSocket.ReceiveFromAsync(e))
                {
                    ProcessUdpReceive(e);
                }
            }
            catch (SocketException ex) when (Operate.PacketConfig.Packet.IsExpectedSocketError(ex.ErrorCode))
            {
                this.ReturnUdpSocketEventArgs(e);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ProcessUdpReceive), ex);

                this.ReturnUdpSocketEventArgs(e);

                if (pu.IsActive)
                {
                    this.StartUdpReceive(pu);
                }
            }
        }

        private void ReturnUdpSocketEventArgs(SocketAsyncEventArgs e, string poolName = "UDPRelay")
        {
            if (e == null) return;

            try
            {
                e.Completed -= UdpReceiveCompleted;
                Operate.ProxyConfig.Proxy.SocketAsyncEventArgsPoolManager.Return(e, poolName);
            }
            catch
            {
                e.Dispose();
            }
        }

        #endregion

        #region//记录无法处理的代理数据

        protected override void HandleUnknownRequest(BinaryRequestInfo requestInfo)
        {
            try
            {
                byte[] bData = requestInfo.Body;

                Operate.DoLog(nameof(HandleUnknownRequest), "无法处理的代理数据：" + Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, bData));
                Close(CloseReason.ProtocolError);
            }
            catch (Exception ex)
            {
                Close(CloseReason.SocketError);
                Operate.DoLog(nameof(HandleUnknownRequest), ex);
            }
        }

        #endregion

        #region//客户端断开链接

        protected override void OnSessionClosed(CloseReason reason)
        {
            /*
                释放设备槽：控制连接走注销（令牌作废 + 释放它占的那份），普通 / 令牌数据连接按 DeviceKey 减一份。
                两条路互斥 —— 控制连接的 DeviceKey 是注册时占的那份，由 Unregister 释放，不能再减一次。
            */
            try
            {
                if (this.IsWpcControl)
                {
                    Operate.WPCConfig.Device.Unregister(this);
                }
                else if (this.AID != Guid.Empty && this.DeviceKey != null)
                {
                    Operate.ProxyConfig.Account.Devices.Remove(this.AID, this.DeviceKey);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnSessionClosed), ex);
            }

            /*
                先关套接字再还缓冲：目标侧可能还挂着一次 BeginReceive，它引用的就是 bBuffer。
                关了套接字那次接收才会以 ObjectDisposed 结束，之后还回池里才不会被别的会话租走时还在被引用。
            */
            Socket target = this.TargetSocket;
            this.TargetSocket = null;
            if (target != null)
            {
                try { target.Close(); } catch { }
            }

            byte[] buffer = this.bBuffer;
            this.bBuffer = null;
            if (buffer != null)
            {
                Operate.ProxyConfig.Proxy.PushProxyBuffer(buffer);
            }
        }

        #endregion        
    }
}
