using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace WinsockPacketEditor
{
    public class ProxySession : AppSession<ProxySession, BinaryRequestInfo>
    {
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
        public Socket TargetSocket = null;
        private readonly SocketAsyncEventArgsPool _socketEventArgsPool = new SocketAsyncEventArgsPool(5000);

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        internal protected new void SetNextReceiveFilter(IReceiveFilter<BinaryRequestInfo> receiveFilter)
        {
            base.SetNextReceiveFilter(receiveFilter);
        }

        #endregion

        #region//SocketAsyncEventArgs 专用池

        public class SocketAsyncEventArgsPool
        {
            private readonly ConcurrentBag<SocketAsyncEventArgs> _pool;
            private readonly int _maxSize;

            public SocketAsyncEventArgsPool(int maxSize = 100)
            {
                _maxSize = maxSize;
                _pool = new ConcurrentBag<SocketAsyncEventArgs>();
            }

            public SocketAsyncEventArgs Get()
            {
                if (_pool.TryTake(out SocketAsyncEventArgs item))
                {
                    return item;
                }
                return new SocketAsyncEventArgs();
            }

            public void Return(SocketAsyncEventArgs item)
            {
                if (item == null) return;

                // 清理状态
                item.SetBuffer(null, 0, 0);
                item.RemoteEndPoint = null;
                item.UserToken = null;
                item.SocketError = SocketError.Success;

                if (_pool.Count < _maxSize)
                {
                    _pool.Add(item);
                }
                else
                {
                    // 池已满，释放资源
                    item.Dispose();
                }
            }

            public int Count => _pool.Count;
        }

        private void ReturnSocketEventArgs(SocketAsyncEventArgs e)
        {
            try
            {
                e.Completed -= UdpReceiveCompleted;

                if (e.Buffer != null)
                {
                    ArrayPool<byte>.Shared.Return(e.Buffer);
                    e.SetBuffer(null, 0, 0);
                }

                e.UserToken = null;
                e.RemoteEndPoint = null;
                _socketEventArgsPool.Return(e);
            }
            catch
            {
                e.Dispose();
            }
        }

        #endregion

        #region//发送 Command 响应数据

        public void SendCommandResponse(ProtocolType ProtocolType, Operate.ProxyConfig.Proxy.CommandResponse CommandResponse, int UDPPort = 0)
        {
            try
            {
                ReadOnlySpan<byte> bServerIP = null;
                ReadOnlySpan<byte> bServerPort = null;

                switch (ProtocolType)
                {
                    case ProtocolType.Tcp:

                        bServerIP = Operate.ProxyConfig.Proxy.ProxyTCP_IP.GetAddressBytes();
                        bServerPort = BitConverter.GetBytes(Operate.ProxyConfig.Proxy.ProxyPort);

                        break;

                    case ProtocolType.Udp:

                        bServerIP = Operate.ProxyConfig.Proxy.ProxyUDP_IP.GetAddressBytes();
                        bServerPort = BitConverter.GetBytes(UDPPort);

                        break;
                }

                Span<byte> response = stackalloc byte[10];
                response[0] = (byte)Operate.ProxyConfig.Proxy.ProxyType.Socket5;
                response[1] = (byte)CommandResponse;
                response[2] = 0x00;
                response[3] = (byte)Operate.ProxyConfig.Proxy.AddressType.IPv4;
                bServerIP.CopyTo(response.Slice(4, 4));
                response[8] = bServerPort[1];
                response[9] = bServerPort[0];

                this.TrySend(response.ToArray(), 0, response.Length);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
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
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.Close(CloseReason.SocketError);

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private readonly object m_SocketLock = new object();
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

                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Success);
                this.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
                this.StartReceivingFromTarget();
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception ex)
            {
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.Close(CloseReason.SocketError);

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Fault);
                    return;
                }

                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Success);
                this.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
                this.StartReceivingFromTarget();
            }
            catch (Exception ex)
            {
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.Close(CloseReason.SocketError);

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//处理 TCP 请求数据

        public void ForwardData(Span<byte> bData)
        {
            try
            {
                if (this.CommandType == Operate.ProxyConfig.Proxy.CommandType.Connect)
                {
                    switch (this.DomainType)
                    {
                        case Operate.ProxyConfig.Proxy.DomainType.Http:

                            if (Operate.ProxyConfig.Mapping.Enable_MapLocal || Operate.ProxyConfig.Mapping.Enable_MapRemote)
                            {
                                string request = Encoding.ASCII.GetString(bData.ToArray());

                                if (request.StartsWith("GET") || request.StartsWith("POST") || request.StartsWith("HEAD") || request.StartsWith("PUT"))
                                {
                                    var headers = Operate.ProxyConfig.Proxy.ParseHttpHeaders(request);
                                    if (headers.TryGetValue("Host", out string hostHeader))
                                    {
                                        string requestPath = request.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                                        string cleanPath = requestPath.Split('?')[0];

                                        #region//本地代理映射

                                        if (Operate.ProxyConfig.Mapping.Enable_MapLocal)
                                        {
                                            var localRule = Operate.ProxyConfig.Mapping.GetMapLocal(
                                                Operate.ProxyConfig.Proxy.MapProtocol.Http,
                                                hostHeader.Split(':')[0],
                                                80,
                                                cleanPath);

                                            if (localRule != null)
                                            {
                                                this.MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType.TCP_Req, bData.ToArray(), false);

                                                if (File.Exists(localRule.LocalPath))
                                                {
                                                    byte[] fileBytes = File.ReadAllBytes(localRule.LocalPath);
                                                    string contentType = Operate.ProxyConfig.Proxy.GetContentType(Path.GetExtension(localRule.LocalPath));

                                                    string response =
                                                        $"HTTP/1.1 200 OK\r\n" +
                                                        $"Content-Type: {contentType}\r\n" +
                                                        $"Content-Length: {fileBytes.Length}\r\n" +
                                                        "Connection: close\r\n\r\n";

                                                    byte[] headerBytes = Encoding.UTF8.GetBytes(response);

                                                    this.TrySend(headerBytes, 0, headerBytes.Length);
                                                    this.MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType.TCP_Resp, headerBytes, false);

                                                    this.TrySend(fileBytes, 0, fileBytes.Length);
                                                    this.MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType.TCP_Resp, fileBytes, false);

                                                    return;
                                                }
                                                else
                                                {
                                                    byte[] b404 = Operate.ProxyConfig.Proxy.Get404Response();
                                                    this.TrySend(b404, 0, b404.Length);
                                                    this.MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType.TCP_Resp, b404, false);

                                                    return;
                                                }
                                            }
                                        }

                                        #endregion

                                        #region//远程代理映射

                                        if (Operate.ProxyConfig.Mapping.Enable_MapRemote)
                                        {
                                            string TargetIP = hostHeader.Split(':')[0];
                                            int TargetPort = 80;

                                            var remoteRule = Operate.ProxyConfig.Mapping.GetMapRemote(
                                                Operate.ProxyConfig.Proxy.MapProtocol.Http,
                                                TargetIP,
                                                TargetPort,
                                                cleanPath);

                                            if (remoteRule != null)
                                            {
                                                this.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(TargetIP, TargetPort);
                                                this.MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType.TCP_Req, bData.ToArray(), true);

                                                byte[] modifiedRequestBytes = Operate.ProxyConfig.Mapping.ModifyRequestHostAndPath(
                                                    request,
                                                    headers,
                                                    remoteRule.HostTo,
                                                    remoteRule.PortTo,
                                                    remoteRule.PathTo);

                                                if (modifiedRequestBytes != null)
                                                {
                                                    this.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(remoteRule.HostTo, remoteRule.PortTo);
                                                    this.TargetSocket.Send(modifiedRequestBytes);
                                                    this.MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType.TCP_Req, modifiedRequestBytes, true);
                                                }

                                                return;
                                            }
                                        }

                                        #endregion
                                    }
                                }
                            }

                            break;

                        case Operate.ProxyConfig.Proxy.DomainType.Https:
                        case Operate.ProxyConfig.Proxy.DomainType.Socket:
                        case Operate.ProxyConfig.Proxy.DomainType.External:

                            break;
                    }

                    if (Operate.ProxyConfig.Proxy.HookTCP_Req)
                    {
                        this.DoFilter_TCP(bData, Operate.PacketConfig.Packet.PacketType.TCP_Req);
                    }
                    else
                    {
                        this.TargetSocket.Send(bData.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                this.Close(CloseReason.SocketError);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            this.DoFilter_TCP(bData.AsSpan(), Operate.PacketConfig.Packet.PacketType.TCP_Resp);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//执行 UDP 中继

        public void UDPRelay(string SessionID)
        {
            try
            {
                ProxyUDP pu = Operate.ProxyConfig.Proxy.CreateNewUDP(SessionID);

                if (pu == null)
                {
                    return;
                }

                int localPort = ((IPEndPoint)pu.ClientSocket.LocalEndPoint).Port;
                this.SendCommandResponse(ProtocolType.Udp, Operate.ProxyConfig.Proxy.CommandResponse.Success, localPort);

                this.StartUdpReceive(pu);
            }
            catch (SocketException)
            {
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Fault);
            }
        }        

        #endregion

        #region//处理 UDP 请求数据

        private void ProcessUdpRequest(ProxyUDP pu, IPEndPoint epRemote, Span<byte> bData)
        {
            Operate.ProxyConfig.Proxy.AddressType addressType = (Operate.ProxyConfig.Proxy.AddressType)bData[3];

            if (addressType == Operate.ProxyConfig.Proxy.AddressType.IPv4 ||
                addressType == Operate.ProxyConfig.Proxy.AddressType.IPv6 ||
                addressType == Operate.ProxyConfig.Proxy.AddressType.Domain)
            {
                pu.ClientEndPoint = epRemote;

                ReadOnlySpan<byte> bADDRESS = bData.Slice(4, bData.Length - 4);
                IPEndPoint targetEndPoint = Operate.ProxyConfig.Proxy.GetIPEndPoint_ByAddressType(addressType, bADDRESS, out string AddressString);
                if (targetEndPoint != null)
                {
                    Span<byte> bRequestData = Operate.ProxyConfig.Proxy.GetUDPData_ByAddressType(addressType, bData);
                    if (!bRequestData.IsEmpty)
                    {
                        Operate.ProxyConfig.Proxy.UDP_Req_CNT++;
                        Interlocked.Add(ref Operate.ProxyConfig.Proxy.Total_Request, bRequestData.Length);
                        Interlocked.Add(ref Operate.ProxyConfig.Proxy.ProxySpeed_Uplink, bRequestData.Length);

                        if (Operate.ProxyConfig.Proxy.HookUDP_Req)
                        {
                            this.DoFilter_UDP(pu, targetEndPoint, bRequestData, Operate.PacketConfig.Packet.PacketType.UDP_Req);
                        }
                        else
                        {
                            this.SendUdpData(pu.ClientSocket, bRequestData, targetEndPoint);
                        }

                        pu.UpdateActivity();
                    }
                }
            }
        }

        #endregion

        #region//处理 UDP 响应数据

        private void ProcessUdpResponse(ProxyUDP pu, IPEndPoint epRemote, Span<byte> bData)
        {
            if (pu.ClientEndPoint == null)
            {
                return;
            }

            ReadOnlySpan<byte> bIP = pu.ClientEndPoint.Address.GetAddressBytes();
            ushort port = ((ushort)pu.ClientEndPoint.Port);
            ReadOnlySpan<byte> bPort = stackalloc byte[2] { (byte)(port >> 8), (byte)port };

            byte[] responseBuffer = ArrayPool<byte>.Shared.Rent(4 + bIP.Length + bPort.Length + bData.Length);
            Span<byte> bResponseData = responseBuffer.AsSpan(0, 4 + bIP.Length + bPort.Length + bData.Length);

            bResponseData[0] = 0x00;
            bResponseData[1] = 0x00;
            bResponseData[2] = 0x00;
            bResponseData[3] = (byte)Operate.ProxyConfig.Proxy.AddressType.IPv4;
            bIP.CopyTo(bResponseData.Slice(4, bIP.Length));
            bPort.CopyTo(bResponseData.Slice(4 + bIP.Length, bPort.Length));
            bData.CopyTo(bResponseData.Slice(4 + bIP.Length + bPort.Length, bData.Length));

            if (!bResponseData.IsEmpty)
            {
                Operate.ProxyConfig.Proxy.UDP_Resp_CNT++;
                Interlocked.Add(ref Operate.ProxyConfig.Proxy.Total_Response, bResponseData.Length);
                Interlocked.Add(ref Operate.ProxyConfig.Proxy.ProxySpeed_Downlink, bResponseData.Length);

                if (Operate.ProxyConfig.Proxy.HookUDP_Resp)
                {
                    this.DoFilter_UDP(pu, epRemote, bResponseData, Operate.PacketConfig.Packet.PacketType.UDP_Resp);
                }
                else
                {
                    this.SendUdpData(pu.ClientSocket, bResponseData, pu.ClientEndPoint);
                }

                pu.UpdateActivity();
            }

            ArrayPool<byte>.Shared.Return(responseBuffer);
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
                SocketAsyncEventArgs socketEventArgs = _socketEventArgsPool.Get();
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void UdpReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessUdpReceive(e);
        }

        private void ProcessUdpReceive(SocketAsyncEventArgs e)
        {
            ProxyUDP pu = e.UserToken as ProxyUDP;

            if (pu == null || !pu.IsActive || pu.ClientSocket == null)
            {
                ReturnSocketEventArgs(e);
                return;
            }

            try
            {
                if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
                {
                    ReturnSocketEventArgs(e);
                    return;
                }

                IPEndPoint epRemote = e.RemoteEndPoint as IPEndPoint;
                if (epRemote == null || epRemote.Address.Equals(IPAddress.Any) || epRemote.Port == 0)
                {
                    ReturnSocketEventArgs(e);
                    return;
                }

                Span<byte> bData = e.Buffer.AsSpan(e.Offset, e.BytesTransferred);

                if (bData[0] == 0 && bData[1] == 0 && bData[2] == 0)
                {
                    ProcessUdpRequest(pu, epRemote, bData);
                }
                else
                {
                    ProcessUdpResponse(pu, epRemote, bData);
                }

                e.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                if (pu.IsActive && pu.ClientSocket != null && !pu.ClientSocket.ReceiveFromAsync(e))
                {
                    ProcessUdpReceive(e);
                }
            }
            catch (SocketException ex) when (Operate.PacketConfig.Packet.IsExpectedSocketError(ex.ErrorCode))
            {
                ReturnSocketEventArgs(e);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                ReturnSocketEventArgs(e);
                if (pu.IsActive)
                {
                    this.StartUdpReceive(pu);
                }
            }
        }

        #endregion

        #region//缓存映射数据

        private void MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType ptType, byte[] bData, bool MapRemote)
        {
            try
            {
                string ClientAddr = $"{this.ClientIP}:{this.ClientPort}";
                string ServerAddr = string.Empty;

                if (MapRemote)
                {
                    ServerAddr = $"{this.ServerIP}:{this.ServerPort}";
                }
                else
                {
                    ServerAddr = $"{this.ClientIP}:{this.ClientPort}";
                }

                _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                    DateTime.Now,
                    Operate.FilterConfig.Filter.FilterAction.None,
                    bData.Length,
                    this.SocketSession.Client.Handle.ToInt32(),
                    ptType,
                    ClientAddr,
                    ServerAddr,
                    this.ServerAddress,
                    this.DomainType,
                    bData,
                    bData);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//执行滤镜 - 代理模式

        public void DoFilter_TCP(Span<byte> bData, Operate.PacketConfig.Packet.PacketType ptType)
        {
            try
            {
                Socket TargetSocket = null;

                switch (ptType)
                {
                    case Operate.PacketConfig.Packet.PacketType.TCP_Req:
                        TargetSocket = this.TargetSocket;
                        break;

                    case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                        TargetSocket = this.SocketSession.Client;
                        break;
                }

                if (TargetSocket == null || !TargetSocket.Connected)
                {
                    return;
                }

                IPEndPoint epRemote = TargetSocket.RemoteEndPoint as IPEndPoint;
                int SocketID = TargetSocket.Handle.ToInt32();

                byte[] bRawBuffer = bData.ToArray();
                byte[] bNewBuffer = null;

                Operate.FilterConfig.Filter.FilterAction FilterAction =
                    Operate.FilterConfig.List.DoFilterList(
                        SocketID,
                        bData,
                        out bNewBuffer,
                        ptType,
                        new Operate.PacketConfig.Packet.SockAddr());

                if (FilterAction != Operate.FilterConfig.Filter.FilterAction.Intercept)
                {
                    switch (ptType)
                    {
                        case Operate.PacketConfig.Packet.PacketType.TCP_Req:
                            this.TargetSocket.Send(bNewBuffer);
                            break;

                        case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                            this.TrySend(bNewBuffer, 0, bNewBuffer.Length);
                            break;
                    }
                }

                _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                    DateTime.Now,
                    FilterAction,
                    bNewBuffer.Length,
                    SocketID,
                    ptType,
                    $"{this.ClientIP}:{this.ClientPort}",
                    $"{this.ServerIP}:{this.ServerPort}",
                    this.ServerAddress,
                    this.DomainType,
                    bRawBuffer,
                    bNewBuffer);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public void DoFilter_UDP(ProxyUDP pu, IPEndPoint epRemote, Span<byte> bData, Operate.PacketConfig.Packet.PacketType ptType)
        {
            try
            {
                IPEndPoint epSend = null;
                switch (ptType)
                {
                    case Operate.PacketConfig.Packet.PacketType.UDP_Req:
                        epSend = epRemote;
                        break;

                    case Operate.PacketConfig.Packet.PacketType.UDP_Resp:
                        epSend = pu.ClientEndPoint;
                        break;
                }

                if (epSend == null || pu?.ClientSocket == null)
                {
                    return;
                }

                int iSocket = pu.ClientSocket.Handle.ToInt32();

                Int32 res = 0;
                byte[] bRawBuffer = bData.ToArray();
                byte[] bNewBuffer = null;

                Operate.FilterConfig.Filter.FilterAction FilterAction =
                    Operate.FilterConfig.List.DoFilterList(
                        iSocket,
                        bData,
                        out bNewBuffer,
                        ptType,
                        new Operate.PacketConfig.Packet.SockAddr());

                if (FilterAction != Operate.FilterConfig.Filter.FilterAction.Intercept)
                {
                    res = this.SendUdpData(pu.ClientSocket, bNewBuffer, epSend);
                }

                string ClientAddr = $"{pu.ClientEndPoint.Address.ToString()}:{pu.ClientEndPoint.Port.ToString()}";
                string ServerAddr = $"{epRemote.Address.ToString()}:{epRemote.Port.ToString()}";

                _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                    DateTime.Now,
                    FilterAction,
                    res,
                    iSocket,
                    ptType,
                    ClientAddr,
                    ServerAddr,
                    ServerAddr,
                    Operate.ProxyConfig.Proxy.DomainType.External,
                    bRawBuffer,
                    bNewBuffer);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//记录无法处理的代理数据

        protected override void HandleUnknownRequest(BinaryRequestInfo requestInfo)
        {
            try
            {
                byte[] bData = requestInfo.Body;

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, "无法处理的代理数据：" + Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, bData));
                Close(CloseReason.ProtocolError);
            }
            catch (Exception ex)
            {
                Close(CloseReason.SocketError);
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
