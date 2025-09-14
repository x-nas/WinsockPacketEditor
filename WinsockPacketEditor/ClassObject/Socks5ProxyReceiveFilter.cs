using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace WinsockPacketEditor
{
    public class Socks5ProxyReceiveFilter : IReceiveFilter<BinaryRequestInfo>
    {
        private byte[] bBuffer = new byte[8192];
        private ProxySession m_Session;

        public int LeftBufferSize { get; set; }

        public FilterState State { get; set; }

        public IReceiveFilter<BinaryRequestInfo> NextReceiveFilter { get; set; }

        #region//初始化

        public Socks5ProxyReceiveFilter(ProxySession session)
        {
            this.m_Session = session;
            this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.Handshake;
        }

        public void Reset()
        {
            State = FilterState.Normal;
            LeftBufferSize = 0;
            NextReceiveFilter = null;
        }

        #endregion

        #region//处理 Socks5 代理步骤

        public BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            if (length <= 0)
            {
                return null;
            }                

            byte[] body = new byte[length];
            Buffer.BlockCopy(readBuffer, offset, body, 0, length);

            BinaryRequestInfo bRequest = new BinaryRequestInfo("SOCKS5", body);
            if (this.HandleSocks5Request(bRequest))
            {
                return null;
            }
            else
            {
                return bRequest;
            }
        }

        private bool HandleSocks5Request(BinaryRequestInfo requestInfo)
        {
            try
            {
                byte[] bData = requestInfo.Body;

                switch (this.m_Session.ProxyStep)
                {
                    case Operate.ProxyConfig.Proxy.ProxyStep.Handshake:
                        this.Handshake(bData.AsSpan());
                        break;

                    case Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName:
                        this.AuthUserName(bData);
                        break;

                    case Operate.ProxyConfig.Proxy.ProxyStep.Command:
                        this.Command(bData);
                        break;

                    case Operate.ProxyConfig.Proxy.ProxyStep.ForwardData:
                        this.ForwardData(bData);
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//握手过程                

        private void Handshake(ReadOnlySpan<byte> bData)
        {
            try
            {
                Operate.ProxyConfig.Proxy.ProxyType ptType = (Operate.ProxyConfig.Proxy.ProxyType)bData[0];

                if (ptType == Operate.ProxyConfig.Proxy.ProxyType.Socket5)
                {
                    bool bSupportAuthType = false;

                    Operate.ProxyConfig.Proxy.AuthType atServer = new Operate.ProxyConfig.Proxy.AuthType();
                    if (Operate.ProxyConfig.Proxy.Enable_Auth)
                    {
                        atServer = Operate.ProxyConfig.Proxy.AuthType.UserName;
                    }
                    else
                    {
                        atServer = Operate.ProxyConfig.Proxy.AuthType.None;
                    }

                    int iMETHODS_COUNT = bData[1];
                    ReadOnlySpan<byte> bMETHODS = bData.Slice(2, iMETHODS_COUNT);
                    foreach (byte method in bMETHODS)
                    {
                        Operate.ProxyConfig.Proxy.AuthType atClient = (Operate.ProxyConfig.Proxy.AuthType)method;

                        if (atServer == atClient)
                        {
                            bSupportAuthType = true;
                            break;
                        }
                    }

                    if (bSupportAuthType)
                    {
                        byte[] bAuth = new byte[2];
                        bAuth[0] = (byte)Operate.ProxyConfig.Proxy.ProxyType.Socket5;
                        bAuth[1] = (byte)atServer;
                        this.m_Session.TrySend(bAuth, 0, bAuth.Length);

                        if (atServer == Operate.ProxyConfig.Proxy.AuthType.UserName)
                        {
                            this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName;

                            if (bData.Length > iMETHODS_COUNT + 2)
                            {
                                ReadOnlySpan<byte> bAuthDate = bData.Slice(iMETHODS_COUNT + 2);

                                bool bIsMatch = Operate.ProxyConfig.Proxy.CheckDataIsMatchProxyStep(bAuthDate, Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName);
                                if (bIsMatch)
                                {
                                    this.AuthUserName(bAuthDate);
                                }
                            }
                        }
                        else
                        {
                            this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.Command;
                        }
                    }
                }
                else
                {
                    string sLog = string.Format(AntdUI.Localization.Get("SOCKS.Unsupported", "不支持的 SOCKS 协议版本: {0}"), ptType);
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//验证账号密码

        private void AuthUserName(ReadOnlySpan<byte> bData)
        {
            try
            {
                byte VERSION = bData[0];

                if (VERSION == 0x01)
                {
                    int USERNAME_LENGTH = bData[1];
                    ReadOnlySpan<byte> USERNAME = bData.Slice(2, USERNAME_LENGTH);

                    int PASSWORD_LENGTH = bData[2 + USERNAME_LENGTH];
                    ReadOnlySpan<byte> PASSWORD = bData.Slice(3 + USERNAME_LENGTH, PASSWORD_LENGTH);

                    string sUserName = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, USERNAME);
                    string sPassWord = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, PASSWORD);

                    Span<byte> bAuth = stackalloc byte[2];
                    bAuth[0] = 0x01;

                    // 第一步：先验证账号密码
                    bool bAuthOK = Operate.ProxyConfig.Account.CheckUserNameAndPassWord(sUserName, sPassWord, out Guid AccountID);

                    if (!bAuthOK)
                    {
                        // 账号密码验证失败直接返回
                        bAuth[1] = (byte)0x01;
                        this.m_Session.TrySend(bAuth.ToArray(), 0, bAuth.Length);
                        return;
                    }

                    // 第二步：验证通过后检查连接数限制
                    bool isOverLinks = Operate.ProxyConfig.Account.CheckLimitLinks(AccountID, this.m_Session.ClientIP);
                    if (isOverLinks)
                    {
                        bAuth[1] = (byte)0x01;
                        this.m_Session.TrySend(bAuth.ToArray(), 0, bAuth.Length);
                        return;
                    }

                    // 第三步：检查设备数限制
                    bool isOverDevices = Operate.ProxyConfig.Account.CheckLimitDevices(AccountID, this.m_Session.ClientIP);
                    if (isOverDevices)
                    {
                        bAuth[1] = (byte)0x01;
                        this.m_Session.TrySend(bAuth.ToArray(), 0, bAuth.Length);
                        return;
                    }

                    // 最终判断是否允许登录
                    bool isAllowed = bAuthOK && !isOverLinks && !isOverDevices;
                    bAuth[1] = isAllowed ? (byte)0x00 : (byte)0x01;

                    if (isAllowed)
                    {
                        Operate.ProxyConfig.Account.SetOnline_ByAccountID(AccountID, true);
                        Operate.ProxyConfig.Account.IPInfo_ToAccount(AccountID, this.m_Session.ClientIP);
                        Operate.ProxyConfig.Account.AuthInfo_ToList(AccountID, this.m_Session.ClientIP, true);

                        this.m_Session.AID = AccountID;
                        this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.Command;
                    }

                    this.m_Session.TrySend(bAuth.ToArray(), 0, bAuth.Length);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//执行命令

        private void Command(ReadOnlySpan<byte> bData)
        {
            try
            {
                Operate.ProxyConfig.Proxy.ProxyType ptType = (Operate.ProxyConfig.Proxy.ProxyType)bData[0];
                if (ptType != Operate.ProxyConfig.Proxy.ProxyType.Socket5)
                {
                    return;
                }

                this.m_Session.CommandType = (Operate.ProxyConfig.Proxy.CommandType)bData[1];
                this.m_Session.AddressType = (Operate.ProxyConfig.Proxy.AddressType)bData[3];

                ReadOnlySpan<byte> bADDRESS = bData.Slice(4, bData.Length - 4);

                IPEndPoint epServer = Operate.ProxyConfig.Proxy.GetIPEndPoint_ByAddressType(this.m_Session.AddressType, bADDRESS, out string TargetAddress);
                if (epServer == null)
                {
                    this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Fault);
                    return;
                }

                string TargetIP = epServer.Address.ToString();
                int TargetPort = epServer.Port;

                this.m_Session.DomainType = Operate.ProxyConfig.Proxy.GetDomainType_ByPort(TargetPort);
                this.m_Session.ClientAddress = Operate.ProxyConfig.Proxy.GetClientAddress(TargetAddress, TargetPort, this.m_Session.ClientPort);

                switch (this.m_Session.CommandType)
                {
                    case Operate.ProxyConfig.Proxy.CommandType.Connect:

                        #region//代理 TCP

                        switch (this.m_Session.DomainType)
                        {
                            case Operate.ProxyConfig.Proxy.DomainType.External:

                                this.m_Session.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(Operate.ProxyConfig.Proxy.ExternalProxy_IP, Operate.ProxyConfig.Proxy.ExternalProxy_Port);
                                this.ConnectToEXTProxyServer(Operate.ProxyConfig.Proxy.ExternalProxy_IP, Operate.ProxyConfig.Proxy.ExternalProxy_Port, bData.ToArray());

                                break;

                            case Operate.ProxyConfig.Proxy.DomainType.Http:

                                this.m_Session.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(TargetAddress, TargetPort);

                                if (Operate.ProxyConfig.Mapping.Enable_MapLocal || Operate.ProxyConfig.Mapping.Enable_MapRemote)
                                {
                                    #region//本地代理映射

                                    if (Operate.ProxyConfig.Mapping.Enable_MapLocal)
                                    {
                                        var localRule = Operate.ProxyConfig.Mapping.GetMapLocal(
                                            Operate.ProxyConfig.Proxy.MapProtocol.Http,
                                            TargetAddress,
                                            TargetPort,
                                            string.Empty);

                                        if (localRule != null)
                                        {
                                            this.m_Session.ServerIP = TargetAddress;
                                            this.m_Session.ServerPort = TargetPort;

                                            if (File.Exists(localRule.LocalPath))
                                            {
                                                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Success);
                                                this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
                                                return;
                                            }
                                            else
                                            {
                                                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                                                return;
                                            }
                                        }
                                    }

                                    #endregion

                                    #region//远程代理映射

                                    if (Operate.ProxyConfig.Mapping.Enable_MapRemote)
                                    {
                                        var remoteRule = Operate.ProxyConfig.Mapping.GetMapRemote(
                                            Operate.ProxyConfig.Proxy.MapProtocol.Http,
                                            TargetAddress,
                                            TargetPort,
                                            string.Empty);

                                        if (remoteRule != null)
                                        {
                                            this.ConnectToTarget(remoteRule.HostTo, remoteRule.PortTo);
                                            return;
                                        }
                                    }

                                    #endregion
                                }

                                this.ConnectToTarget(TargetIP, TargetPort);

                                break;

                            case Operate.ProxyConfig.Proxy.DomainType.Https:
                            case Operate.ProxyConfig.Proxy.DomainType.Socket:

                                this.m_Session.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(TargetAddress, TargetPort);
                                this.ConnectToTarget(TargetIP, TargetPort);

                                break;
                        }

                        if (!Operate.ProxyConfig.Proxy.SpeedMode)
                        {
                            string ProxyIP = (this.m_Session.SocketSession.Client.LocalEndPoint as IPEndPoint).Address.ToString();
                            Operate.DoProxyLog(this.m_Session.AID, this.m_Session.ClientIP, this.m_Session.ServerAddress, ProxyIP);
                        }

                        #endregion

                        break;

                    case Operate.ProxyConfig.Proxy.CommandType.UDP:

                        #region//UDP 中继

                        this.UDPRelay(this.m_Session.SessionID);

                        #endregion

                        break;

                    default:

                        #region//不支持的命令

                        this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unsupport);

                        string sLog = string.Format(AntdUI.Localization.Get("Command.Unsupported", "{0} - 不支持的命令: {1}"), this.m_Session.ClientAddress, this.m_Session.CommandType);
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);

                        #endregion

                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//发送 Command 响应数据

        private void SendCommandResponse(ProtocolType ProtocolType, Operate.ProxyConfig.Proxy.CommandResponse CommandResponse, int UDPPort = 0)
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

                this.m_Session.TrySend(response.ToArray(), 0, response.Length);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//连接远程服务器

        private void ConnectToTarget(string TargetIP, int TargetPort)
        {
            try
            {
                this.m_Session.TargetSocket.BeginConnect(TargetIP, TargetPort, new AsyncCallback(OnTargetConnected), null);
            }
            catch (Exception ex)
            {
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.m_Session.Close(CloseReason.SocketError);

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void OnTargetConnected(IAsyncResult ar)
        {
            try
            {
                this.m_Session.TargetSocket.EndConnect(ar);

                this.m_Session.ServerIP = (this.m_Session.TargetSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                this.m_Session.ServerPort = (this.m_Session.TargetSocket.RemoteEndPoint as IPEndPoint).Port;

                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Success);
                this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
                this.StartReceivingFromTarget();
            }
            catch (Exception ex)
            {
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.m_Session.Close(CloseReason.SocketError);

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//连接外部代理服务器

        private void ConnectToEXTProxyServer(string TargetIP, int TargetPort, byte[] bData)
        {
            try
            {
                this.m_Session.TargetSocket.BeginConnect(TargetIP, TargetPort, new AsyncCallback(OnEXTProxyServerConnected), bData);
            }
            catch (Exception ex)
            {
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.m_Session.Close(CloseReason.SocketError);

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void OnEXTProxyServerConnected(IAsyncResult ar)
        {
            byte[] bData = (byte[])ar.AsyncState;

            try
            {
                this.m_Session.TargetSocket.EndConnect(ar);

                this.m_Session.ServerIP = (this.m_Session.TargetSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                this.m_Session.ServerPort = (this.m_Session.TargetSocket.RemoteEndPoint as IPEndPoint).Port;

                byte[] handshakeRequest = null;
                if (Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth)
                {
                    handshakeRequest = new byte[] { 0x05, 0x02, 0x00, 0x02 };
                }
                else
                {
                    handshakeRequest = new byte[] { 0x05, 0x01, 0x00 };
                }
                this.m_Session.TargetSocket.Send(handshakeRequest);

                byte[] handshakeResponse = new byte[2];
                this.m_Session.TargetSocket.Receive(handshakeResponse);

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
                        this.m_Session.TargetSocket.Send(AuthRequest);

                        byte[] AuthResponse = new byte[2];
                        this.m_Session.TargetSocket.Receive(AuthResponse);

                        if (AuthResponse[1] != 0x00)
                        {
                            return;
                        }

                        break;

                    default:
                        return;
                }

                this.m_Session.TargetSocket.Send(bData);

                byte[] connectResponse = new byte[10];
                this.m_Session.TargetSocket.Receive(connectResponse);

                if (connectResponse[1] != 0x00)
                {
                    this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Fault);
                    return;
                }

                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Success);
                this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
                this.StartReceivingFromTarget();
            }
            catch (Exception ex)
            {
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                this.m_Session.Close(CloseReason.SocketError);

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//处理 TCP 请求数据

        private void ForwardData(Span<byte> bData)
        {
            try
            {
                if (this.m_Session.CommandType == Operate.ProxyConfig.Proxy.CommandType.Connect)
                {
                    switch (this.m_Session.DomainType)
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

                                                    this.m_Session.TrySend(headerBytes, 0, headerBytes.Length);
                                                    this.MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType.TCP_Resp, headerBytes, false);

                                                    this.m_Session.TrySend(fileBytes, 0, fileBytes.Length);
                                                    this.MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType.TCP_Resp, fileBytes, false);

                                                    return;
                                                }
                                                else
                                                {
                                                    byte[] b404 = Operate.ProxyConfig.Proxy.Get404Response();
                                                    this.m_Session.TrySend(b404, 0, b404.Length);
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
                                                this.m_Session.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(TargetIP, TargetPort);
                                                this.MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType.TCP_Req, bData.ToArray(), true);

                                                byte[] modifiedRequestBytes = Operate.ProxyConfig.Mapping.ModifyRequestHostAndPath(
                                                    request,
                                                    headers,
                                                    remoteRule.HostTo,
                                                    remoteRule.PortTo,
                                                    remoteRule.PathTo);

                                                if (modifiedRequestBytes != null)
                                                {
                                                    this.m_Session.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(remoteRule.HostTo, remoteRule.PortTo);
                                                    this.m_Session.TargetSocket.Send(modifiedRequestBytes);
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
                        this.m_Session.TargetSocket.Send(bData.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                this.m_Session.Close(CloseReason.SocketError);
            }
        }

        #endregion

        #region//处理 TCP 响应数据

        private void StartReceivingFromTarget()
        {
            try
            {
                Socket targetSocket = this.m_Session.TargetSocket;
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
                this.m_Session.Close(CloseReason.SocketError);
            }
            catch (Exception ex)
            {
                this.m_Session.Close(CloseReason.SocketError);
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void OnTargetDataReceived(IAsyncResult ar)
        {
            try
            {
                Socket targetSocket = this.m_Session.TargetSocket;
                if (targetSocket == null || !targetSocket.Connected)
                {
                    return;
                }

                int bytesRead = targetSocket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    byte[] bData = this.bBuffer.AsSpan(0, bytesRead).ToArray();

                    if (this.m_Session.CommandType == Operate.ProxyConfig.Proxy.CommandType.Connect)
                    {
                        if (Operate.ProxyConfig.Proxy.HookTCP_Resp)
                        {
                            this.DoFilter_TCP(bData.AsSpan(), Operate.PacketConfig.Packet.PacketType.TCP_Resp);
                        }
                        else
                        {
                            this.m_Session.TrySend(bData, 0, bData.Length);
                        }
                    }

                    this.StartReceivingFromTarget();
                }
                else
                {
                    this.m_Session.Close(CloseReason.ServerClosing);
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.NotConnected || ex.SocketErrorCode == SocketError.ConnectionReset)
            {
                this.m_Session.Close(CloseReason.SocketError);
            }
            catch (Exception ex)
            {
                this.m_Session.Close(CloseReason.SocketError);
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//执行 UDP 中继

        private void UDPRelay(string SessionID)
        {
            try
            {
                ProxyUDP pu = Operate.ProxyConfig.Proxy.CreateNewUDP(SessionID);

                if (pu == null)
                {
                    return;
                }

                this.SendCommandResponse(ProtocolType.Udp, Operate.ProxyConfig.Proxy.CommandResponse.Success, ((IPEndPoint)pu.ClientUDP.Client.LocalEndPoint).Port);

                this.StartUdpReceive(pu);
            }
            catch (SocketException)
            {
                this.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Fault);
            }
        }

        #endregion

        #region//处理 UDP 中继数据

        public void StartUdpReceive(ProxyUDP pu)
        {
            try
            {
                if (pu.ClientUDP != null)
                {
                    pu.ClientUDP.BeginReceive(new AsyncCallback(UdpReceiveCallback), pu);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void UdpReceiveCallback(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is ProxyUDP pu))
            {
                return;
            }

            if (pu.ClientUDP == null)
            {
                return;
            }

            try
            {
                IPEndPoint epRemote = new IPEndPoint(IPAddress.Any, 0);

                byte[] bReceivedData = this.ReceiveUDPData(pu.ClientUDP, ar, ref epRemote);
                if (bReceivedData.Length == 0 || epRemote.Address.Equals(IPAddress.Any) || epRemote.Port == 0)
                {
                    return;
                }

                Span<byte> bData = bReceivedData.AsSpan();
                if (bData[0] == 0 && bData[1] == 0 && bData[2] == 0)
                {
                    #region//处理 UDP 请求数据

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
                                    this.SendUDPData(pu.ClientUDP, bRequestData, targetEndPoint);
                                }

                                pu.UpdateActivity();
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region//处理 UDP 响应数据

                    if (pu.ClientEndPoint == null)
                    {
                        return;
                    }

                    ReadOnlySpan<byte> bIP = pu.ClientEndPoint.Address.GetAddressBytes();
                    ushort port = ((ushort)pu.ClientEndPoint.Port);
                    ReadOnlySpan<byte> bPort = new byte[2] { (byte)(port >> 8), (byte)port };

                    Span<byte> bResponseData = stackalloc byte[4 + bIP.Length + bPort.Length + bData.Length];
                    bResponseData[0] = 0x00;
                    bResponseData[1] = 0x00;
                    bResponseData[2] = 0x00;
                    bResponseData[3] = (byte)Operate.ProxyConfig.Proxy.AddressType.IPv4;
                    bIP.CopyTo(bResponseData.Slice(4, bIP.Length));
                    bPort.CopyTo(bResponseData.Slice(8, bPort.Length));
                    bData.CopyTo(bResponseData.Slice(10, bData.Length));

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
                            this.SendUDPData(pu.ClientUDP, bResponseData, pu.ClientEndPoint);
                        }

                        pu.UpdateActivity();
                    }

                    #endregion
                }

                this.StartUdpReceive(pu);
            }
            catch (SocketException ex) when (Operate.PacketConfig.Packet.IsExpectedSocketError(ex.ErrorCode))
            {
                //
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                this.StartUdpReceive(pu);
            }
        }

        #endregion

        #region//发送和接收 UDP 数据        

        public int SendUDPData(UdpClient ClientUDP, ReadOnlySpan<byte> bData, IPEndPoint ep)
        {
            int iReturn = 0;

            try
            {
                if (ClientUDP != null && !bData.IsEmpty)
                {
                    iReturn = ClientUDP.Send(bData.ToArray(), bData.Length, ep);
                }
            }
            catch
            {
                //
            }

            return iReturn;
        }

        public byte[] ReceiveUDPData(UdpClient ClientUDP, IAsyncResult ar, ref IPEndPoint ep)
        {
            try
            {
                if (ClientUDP != null && ClientUDP.Client != null)
                {
                    return ClientUDP.EndReceive(ar, ref ep);
                }
            }
            catch
            {
                return Array.Empty<byte>();
            }

            return Array.Empty<byte>();
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
                        TargetSocket = this.m_Session.TargetSocket;
                        break;

                    case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                        TargetSocket = this.m_Session.SocketSession.Client;
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
                            this.m_Session.TargetSocket.Send(bNewBuffer);
                            break;

                        case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                            this.m_Session.TrySend(bNewBuffer, 0, bNewBuffer.Length);
                            break;
                    }
                }

                _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                    DateTime.Now,
                    FilterAction,
                    bNewBuffer.Length,
                    SocketID,
                    ptType,
                    $"{this.m_Session.ClientIP}:{this.m_Session.ClientPort}",
                    $"{this.m_Session.ServerIP}:{this.m_Session.ServerPort}",
                    this.m_Session.ServerAddress,
                    this.m_Session.DomainType,
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

                if (epSend == null || pu?.ClientUDP?.Client == null)
                {
                    return;
                }

                int iSocket = pu.ClientUDP.Client.Handle.ToInt32();

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
                    res = this.SendUDPData(pu.ClientUDP, bNewBuffer, epSend);
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

        #region//缓存映射数据

        private void MappingData_ToQueue(Operate.PacketConfig.Packet.PacketType ptType, byte[] bData, bool MapRemote)
        {
            try
            {
                string ClientAddr = $"{this.m_Session.ClientIP}:{this.m_Session.ClientPort}";
                string ServerAddr = string.Empty;

                if (MapRemote)
                {
                    ServerAddr = $"{this.m_Session.ServerIP}:{this.m_Session.ServerPort}";
                }
                else
                {
                    ServerAddr = $"{this.m_Session.ClientIP}:{this.m_Session.ClientPort}";
                }

                _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                    DateTime.Now,
                    Operate.FilterConfig.Filter.FilterAction.None,
                    bData.Length,
                    this.m_Session.SocketSession.Client.Handle.ToInt32(),
                    ptType,
                    ClientAddr,
                    ServerAddr,
                    this.m_Session.ServerAddress,
                    this.m_Session.DomainType,
                    bData,
                    bData);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
