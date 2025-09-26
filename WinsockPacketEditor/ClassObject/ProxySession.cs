using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;

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
        public string ServerAddress = string.Empty;
        public string ClientAddress = string.Empty;
        public Operate.ProxyConfig.Proxy.ProxyStep ProxyStep;
        public Operate.ProxyConfig.Proxy.CommandType CommandType;
        public Operate.ProxyConfig.Proxy.AddressType AddressType;
        public Operate.ProxyConfig.Proxy.DomainType DomainType;        

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

                this.bBuffer = AppServer.RequestProxyBuffer();
                this.TargetSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnSessionStarted), ex.Message);
            }
        }

        internal protected new void SetNextReceiveFilter(IReceiveFilter<BinaryRequestInfo> receiveFilter)
        {
            base.SetNextReceiveFilter(receiveFilter);
        }

        #endregion        

        #region//连接远程服务器

        public void ConnectToTarget(string TargetIP, int TargetPort)
        {
            try
            {
                this.TargetSocket.BeginConnect(TargetIP, TargetPort, new AsyncCallback(OnTargetConnected), null);
            }
            catch (Exception ex)
            {
                Operate.ProxyConfig.Proxy.SendCommandResponse(this, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.Close(CloseReason.SocketError);

                Operate.DoLog(nameof(ConnectToTarget), ex.Message);
            }
        }
        
        private void OnTargetConnected(IAsyncResult ar)
        {
            try
            {
                Socket targetSocket = this.TargetSocket;
                if (targetSocket == null || !targetSocket.Connected)
                {
                    return;
                }

                targetSocket.EndConnect(ar);

                this.ServerIP = (targetSocket.RemoteEndPoint as IPEndPoint)?.Address.ToString();
                this.ServerPort = (targetSocket.RemoteEndPoint as IPEndPoint)?.Port ?? 0;

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
                Operate.DoLog(nameof(OnTargetConnected), ex.Message);
            }
        }

        #endregion

        #region//连接外部代理服务器

        public void ConnectToEXTProxyServer(string TargetIP, int TargetPort, byte[] bData)
        {
            try
            {
                this.TargetSocket.BeginConnect(TargetIP, TargetPort, new AsyncCallback(OnEXTProxyServerConnected), bData);
            }
            catch (Exception ex)
            {
                Operate.ProxyConfig.Proxy.SendCommandResponse(this, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.Close(CloseReason.SocketError);

                Operate.DoLog(nameof(ConnectToEXTProxyServer), ex.Message);
            }
        }

        private void OnEXTProxyServerConnected(IAsyncResult ar)
        {
            byte[] bData = (byte[])ar.AsyncState;

            try
            {
                this.TargetSocket.EndConnect(ar);

                this.ServerIP = (this.TargetSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                this.ServerPort = (this.TargetSocket.RemoteEndPoint as IPEndPoint).Port;

                byte[] handshakeRequest = null;
                if (Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth)
                {
                    handshakeRequest = new byte[] { 0x05, 0x02, 0x00, 0x02 };
                }
                else
                {
                    handshakeRequest = new byte[] { 0x05, 0x01, 0x00 };
                }
                this.TargetSocket.Send(handshakeRequest);

                byte[] handshakeResponse = new byte[2];
                this.TargetSocket.Receive(handshakeResponse);

                if (handshakeResponse[0] != 0x05)
                {
                    return;
                }

                switch (handshakeResponse[1])
                {
                    case 0x00:
                        break;

                    case 0x02:

                        if (!Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth)
                        {
                            return;
                        }

                        byte[] AuthRequest = Operate.ProxyConfig.Proxy.CreateSOCKS5AuthPacket(Operate.ProxyConfig.Proxy.ExternalProxy_UserName, Operate.ProxyConfig.Proxy.ExternalProxy_PassWord);
                        if (AuthRequest == null)
                        {
                            return;
                        }
                        this.TargetSocket.Send(AuthRequest);

                        byte[] AuthResponse = new byte[2];
                        this.TargetSocket.Receive(AuthResponse);

                        if (AuthResponse[1] != 0x00)
                        {
                            return;
                        }

                        break;

                    default:
                        return;
                }

                this.TargetSocket.Send(bData);

                byte[] connectResponse = new byte[10];
                this.TargetSocket.Receive(connectResponse);

                if (connectResponse[1] != 0x00)
                {
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

                Operate.DoLog(nameof(OnEXTProxyServerConnected), ex.Message);
            }
        }

        #endregion

        #region//发送和接收 TCP 数据

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
                Operate.DoLog(nameof(StartReceivingFromTarget), ex.Message);
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

                    if (this.CommandType == Operate.ProxyConfig.Proxy.CommandType.Connect)
                    {
                        if (Operate.ProxyConfig.Proxy.HookTCP_Resp)
                        {
                            Operate.FilterConfig.Filter.DoFilter_TCP(this, bData.AsSpan(), Operate.PacketConfig.Packet.PacketType.TCP_Resp);
                        }
                        else
                        {
                            this.TrySend(bData, 0, bData.Length);
                        }
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
                Operate.DoLog(nameof(OnTargetDataReceived), ex.Message);
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
                Operate.DoLog(nameof(StartUdpReceive), ex.Message);
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

                if (bData[0] == 0 && bData[1] == 0 && bData[2] == 0)
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
                Operate.DoLog(nameof(ProcessUdpReceive), ex.Message);

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
                Operate.DoLog(nameof(HandleUnknownRequest), ex.Message);
            }
        }

        #endregion

        #region//客户端断开链接

        protected override void OnSessionClosed(CloseReason reason)
        {
            if (this.bBuffer != null)
            { 
                AppServer.PushProxyBuffer(this.bBuffer);
            }

            if (this.TargetSocket != null)
            {
                this.TargetSocket.Close();
            }
        }

        #endregion
    }
}
