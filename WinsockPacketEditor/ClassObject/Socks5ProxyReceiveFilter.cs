using SuperSocket.SocketBase.Protocol;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WinsockPacketEditor
{
    public class Socks5ProxyReceiveFilter : IReceiveFilter<BinaryRequestInfo>
    {        
        private ProxySession m_Session;

        public int LeftBufferSize { get; set; }

        public FilterState State { get; set; }

        public IReceiveFilter<BinaryRequestInfo> NextReceiveFilter { get; set; }

        #region//初始化

        public Socks5ProxyReceiveFilter(ProxySession session)
        {
            this.m_Session = session;            
            this.m_Session.ProxyType = Operate.ProxyConfig.Proxy.ProxyType.Socket5;
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
            
            if (this.HandleSocks5Request(body.AsSpan()))
            {
                return null;
            }
            else
            {
                return new BinaryRequestInfo("SOCKS5", body);
            }
        }

        private bool HandleSocks5Request(Span<byte> bDataSpan)
        {
            try
            {
                switch (this.m_Session.ProxyStep)
                {
                    case Operate.ProxyConfig.Proxy.ProxyStep.Handshake:
                        this.Handshake(bDataSpan);
                        break;

                    case Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName:
                        this.AuthUserName(bDataSpan);
                        break;

                    case Operate.ProxyConfig.Proxy.ProxyStep.Command:
                        this.Command(bDataSpan);
                        break;

                    case Operate.ProxyConfig.Proxy.ProxyStep.ForwardData:
                        this.m_Session.ForwardData(bDataSpan);
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(HandleSocks5Request), ex.Message);
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
                    string sLog = string.Format(AntdUI.Localization.Get("SOCKS.Unsupported", "不支持的 SOCKS 协议版本: {0} [ {1} ]"), ptType, this.m_Session.ClientIP);
                    Operate.DoLog(nameof(Handshake), sLog);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Handshake), ex.Message);
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
                Operate.DoLog(nameof(AuthUserName), ex.Message);
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
                    this.m_Session.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Fault);
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
                                this.m_Session.ConnectToEXTProxyServer(Operate.ProxyConfig.Proxy.ExternalProxy_IP, Operate.ProxyConfig.Proxy.ExternalProxy_Port, bData.ToArray());

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
                                                this.m_Session.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Success);
                                                this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
                                                return;
                                            }
                                            else
                                            {
                                                this.m_Session.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
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
                                            this.m_Session.ConnectToTarget(remoteRule.HostTo, remoteRule.PortTo);
                                            return;
                                        }
                                    }

                                    #endregion
                                }

                                this.m_Session.ConnectToTarget(TargetIP, TargetPort);

                                break;

                            case Operate.ProxyConfig.Proxy.DomainType.Https:
                            case Operate.ProxyConfig.Proxy.DomainType.Socket:

                                this.m_Session.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(TargetAddress, TargetPort);
                                this.m_Session.ConnectToTarget(TargetIP, TargetPort);

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

                        this.m_Session.UDPRelay(this.m_Session.SessionID);

                        #endregion

                        break;

                    default:

                        #region//不支持的命令

                        this.m_Session.SendCommandResponse(ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unsupport);

                        string sLog = string.Format(AntdUI.Localization.Get("Command.Unsupported", "{0} - 不支持的命令: {1}"), this.m_Session.ClientAddress, this.m_Session.CommandType);
                        Operate.DoLog(nameof(Command), sLog);

                        #endregion

                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Command), ex.Message);
            }
        }

        #endregion        
    }
}
