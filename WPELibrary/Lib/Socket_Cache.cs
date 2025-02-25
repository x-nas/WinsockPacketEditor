using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;
using Be.Windows.Forms;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WPELibrary.Lib
{
    public static class Socket_Cache
    {
        public static string WPE = "Winsock Packet Editor x64";
        public static IntPtr MainHandle = IntPtr.Zero;
        public static int SystemSocket = 0;

        #region//结构定义

        public enum SystemMode
        { 
            Process = 0,
            Proxy = 1,
        }

        public enum PWType
        {
            FilterList_Import = 0,
            FilterList_Export = 1,
            RobotList_Import = 2,
            RobotList_Export = 3,
            SendList_Import = 4,
            SendList_Export = 5,
            SendCollection_Import = 6,
            SendCollection_Export = 7,
        }

        public enum ListAction
        {
            Top = 0,
            Up = 1,
            Down = 2,
            Bottom = 3,
            Copy = 4,
            Export = 5,
            Delete = 6,
            CleanUp = 7,
            Import = 8,
        }

        public enum LogType
        {
            Socket,
            Proxy,
        }

        #endregion                

        #region//代理

        public static class SocketProxy
        {
            public static ulong ProxyTotal_CNT, ProxyTCP_CNT, ProxyUDP_CNT;
            public static IPAddress ProxyTCP_IP = IPAddress.Any;
            public static IPAddress ProxyUDP_IP = IPAddress.Any;
            public static bool IsListening = false;
            public static bool ProxyIP_Auto;
            public static bool Enable_SOCKS5, Enable_Auth;
            public static bool Enable_EXTHttp, Enable_EXTHttps;
            public static string EXTHttpIP, EXTHttpsIP;
            public static ushort EXTHttpPort, EXTHttpsPort;
            public static string AppointHttpPort, AppointHttpsPort;
            public static string Auth_UserName, Auth_PassWord;
            public static ushort ProxyPort;
            public static int UDPCloseTime = 60;
            public static long Total_Request = 0;
            public static long Total_Response = 0;

            #region//定义结构

            public enum ProxyType
            {
                None = 0,
                Http = 1,
                Socket5 = 5,
                Socket6 = 6,
            }

            public enum ProxyStep : byte
            {
                Handshake = 0,
                AuthUserName = 1,
                Command = 2,
                ForwardData = 3,
            }

            public enum AuthType : byte
            {
                None = 0,
                GSSAPI = 1,
                UserName = 2,
            }

            public enum AddressType : byte
            {
                IPV4 = 1,
                Domain = 3,
                IPV6 = 4,
            }

            public enum DomainType : byte
            {
                Socket = 0,
                Http = 1,
                Https = 2,
            }

            public enum CommandType : byte
            {
                Connect = 1,
                Bind = 2,
                UDP = 3,
            }

            public enum CommandResponse : byte
            {
                Success = 0,
                Fault = 1,
                Unsupport =7,
            }

            public enum DataType : byte
            {
                Request = 0,
                Response = 1,
            }

            #endregion

            #region//接收客户端请求

            public static void HandleClient(Socket clientSocket)
            {
                try
                {
                    Socket_ProxyInfo spi = new Socket_ProxyInfo
                    {
                        ClientSocket = clientSocket,
                        ClientData = new byte[0],
                        ClientBuffer = new byte[clientSocket.ReceiveBufferSize],
                        TargetBuffer = new byte[clientSocket.ReceiveBufferSize],
                        ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Handshake
                    };

                    spi.ClientSocket.BeginReceive(spi.ClientBuffer, 0, spi.ClientBuffer.Length, 0, new AsyncCallback(ReadCallback), spi);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void ReadCallback(IAsyncResult ar)
            {
                Socket_ProxyInfo spi = (Socket_ProxyInfo)ar.AsyncState;

                try
                {
                    if (Socket_Cache.SocketProxy.IsListening)
                    {
                        int bytesRead = Socket_Operation.ReceiveTCPData(spi.ClientSocket, ar);

                        if (bytesRead > 0)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                if (spi.ClientData.Length > 0)
                                {
                                    ms.Write(spi.ClientData, 0, spi.ClientData.Length);
                                }
                                ms.Write(spi.ClientBuffer, 0, bytesRead);
                                spi.ClientData = ms.ToArray();
                            }

                            byte[] bData = new byte[spi.ClientData.Length];
                            Buffer.BlockCopy(spi.ClientData, 0, bData, 0, spi.ClientData.Length);

                            bool bIsMatch = Socket_Operation.CheckDataIsMatchProxyStep(bData, spi.ProxyStep);

                            if (bIsMatch)
                            {
                                switch (spi.ProxyStep)
                                {
                                    case Socket_Cache.SocketProxy.ProxyStep.Handshake:
                                        Socket_Cache.SocketProxy.Handshake(spi, bData);
                                        break;

                                    case Socket_Cache.SocketProxy.ProxyStep.AuthUserName:
                                        Socket_Cache.SocketProxy.AuthUserName(spi, bData);
                                        break;

                                    case Socket_Cache.SocketProxy.ProxyStep.Command:
                                        Socket_Cache.SocketProxy.Command(spi, bData);
                                        break;

                                    case Socket_Cache.SocketProxy.ProxyStep.ForwardData:
                                        Socket_Cache.SocketProxy.ForwardData(spi, bData);
                                        break;
                                }

                                spi.ClientData = new byte[0];
                            }

                            spi.ClientSocket.BeginReceive(spi.ClientBuffer, 0, spi.ClientBuffer.Length, 0, new AsyncCallback(ReadCallback), spi);
                        }
                        else
                        {
                            spi.CloseTCPClient();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.ClientSocket.RemoteEndPoint + " - " + ex.Message);
                    spi.CloseTCPClient();                                        
                }
            }

            #endregion

            #region//握手过程

            private static void Handshake(Socket_ProxyInfo spi, byte[] bData)
            {
                try
                {
                    spi.ProxyType = (Socket_Cache.SocketProxy.ProxyType)bData[0];

                    if (spi.ProxyType == Socket_Cache.SocketProxy.ProxyType.Socket5)
                    {
                        bool bSupportAuthType = false;
                        Socket_Cache.SocketProxy.AuthType atServer = new Socket_Cache.SocketProxy.AuthType();
                        Socket_Cache.SocketProxy.AuthType atClient = new Socket_Cache.SocketProxy.AuthType();

                        if (Socket_Cache.SocketProxy.Enable_Auth)
                        {
                            atServer = Socket_Cache.SocketProxy.AuthType.UserName;
                        }
                        else
                        {
                            atServer = Socket_Cache.SocketProxy.AuthType.None;
                        }

                        int iMETHODS_COUNT = bData[1];
                        byte[] bMETHODS = new byte[iMETHODS_COUNT];

                        for (int i = 0; i < iMETHODS_COUNT; i++)
                        {
                            bMETHODS[i] = bData[2 + i];
                            atClient = (Socket_Cache.SocketProxy.AuthType)bMETHODS[i];

                            if (atServer == atClient)
                            {
                                bSupportAuthType = true;
                            }
                        }

                        if (bSupportAuthType)
                        {
                            byte[] bAuth = new byte[] { ((byte)Socket_Cache.SocketProxy.ProxyType.Socket5), ((byte)atServer) };
                            Socket_Operation.SendTCPData(spi.ClientSocket, bAuth);

                            if (atServer == Socket_Cache.SocketProxy.AuthType.UserName)
                            {
                                spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.AuthUserName;

                                if (bData.Length > iMETHODS_COUNT + 2)
                                {
                                    byte[] bAuthDate = new byte[bData.Length - (iMETHODS_COUNT + 2)];
                                    Buffer.BlockCopy(bData, iMETHODS_COUNT + 2, bAuthDate, 0, bAuthDate.Length);

                                    bool bIsMatch = Socket_Operation.CheckDataIsMatchProxyStep(bAuthDate, Socket_Cache.SocketProxy.ProxyStep.AuthUserName);
                                    if (bIsMatch)
                                    {
                                        Socket_Cache.SocketProxy.AuthUserName(spi, bAuthDate);
                                    }
                                }
                            }
                            else
                            {
                                spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Command;
                            }
                        }
                        else
                        {
                            string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_146), spi.ClientSocket.RemoteEndPoint, atServer);
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                        }
                    }
                    else
                    {
                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_145), spi.ProxyType);
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//验证账号密码

            private static void AuthUserName(Socket_ProxyInfo spi, byte[] bData)
            {
                try
                {
                    byte VERSION = bData[0];

                    if (VERSION == 0x01)
                    {
                        int USERNAME_LENGTH = bData[1];
                        byte[] USERNAME = new byte[USERNAME_LENGTH];
                        Buffer.BlockCopy(bData, 2, USERNAME, 0, USERNAME_LENGTH);

                        int PASSWORD_LENGTH = bData[2 + USERNAME_LENGTH];
                        byte[] PASSWORD = new byte[PASSWORD_LENGTH];
                        Buffer.BlockCopy(bData, 3 + USERNAME_LENGTH, PASSWORD, 0, PASSWORD_LENGTH);

                        string sUserName = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, USERNAME);
                        string sPassWord = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, PASSWORD);
                        string sAuth_UserName = Socket_Cache.SocketProxy.Auth_UserName;
                        string sAuth_PassWord = Socket_Cache.SocketProxy.Auth_PassWord;

                        bool bAuthOK = true;
                        if (!string.IsNullOrEmpty(sAuth_UserName) && !string.IsNullOrEmpty(sAuth_PassWord))
                        {
                            if (!sAuth_UserName.Equals(sUserName) || !sAuth_PassWord.Equals(sPassWord))
                            {
                                bAuthOK = false;
                            }
                        }

                        byte[] bAuth;
                        if (bAuthOK)
                        {
                            bAuth = new byte[] { 0x01, 0x00 };
                        }
                        else
                        {
                            bAuth = new byte[] { 0x01, 0x01 };

                            string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_147), spi.ClientSocket.RemoteEndPoint);
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                        }

                        Socket_Operation.SendTCPData(spi.ClientSocket, bAuth);
                        spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Command;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//执行命令

            private static void Command(Socket_ProxyInfo spi, byte[] bData)
            {
                try
                {
                    spi.ProxyType = (Socket_Cache.SocketProxy.ProxyType)bData[0];
                    spi.CommandType = (Socket_Cache.SocketProxy.CommandType)bData[1];
                    spi.AddressType = (Socket_Cache.SocketProxy.AddressType)bData[3];

                    if (spi.ProxyType == Socket_Cache.SocketProxy.ProxyType.Socket5)
                    {
                        byte[] bADDRESS = new byte[bData.Length - 4];
                        Buffer.BlockCopy(bData, 4, bADDRESS, 0, bADDRESS.Length);

                        IPEndPoint targetEP = Socket_Operation.GetIPEndPoint_ByAddressType(spi.AddressType, bADDRESS);
                        string sIPString = Socket_Operation.GetIP_ByAddressType(spi.AddressType, bADDRESS);
                        ushort uPort = ((ushort)targetEP.Port);

                        spi.TargetEndPoint = targetEP;
                        spi.DomainType = Socket_Operation.GetDomainType_ByPort(uPort);
                        spi.ClientAddress = Socket_Operation.GetClientAddress(sIPString, uPort, spi);
                        spi.TargetAddress = Socket_Operation.GetTargetAddress(sIPString, uPort, spi);

                        try
                        {
                            byte[] bServerTCP_IP = Socket_Cache.SocketProxy.ProxyTCP_IP.GetAddressBytes();
                            byte[] bServerTCP_Port = BitConverter.GetBytes(Socket_Cache.SocketProxy.ProxyPort);                            

                            switch (spi.CommandType)
                            {
                                case Socket_Cache.SocketProxy.CommandType.Connect:

                                    #region//代理 TCP

                                    spi.TargetSocket = new Socket(targetEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                                    switch (spi.DomainType)
                                    {
                                        case Socket_Cache.SocketProxy.DomainType.Http:

                                            #region//HTTP 协议

                                            try
                                            {
                                                if (Socket_Cache.SocketProxy.Enable_EXTHttp)
                                                {
                                                    IPEndPoint HttpProxyEP = new IPEndPoint(IPAddress.Parse(Socket_Cache.SocketProxy.EXTHttpIP), Socket_Cache.SocketProxy.EXTHttpPort);
                                                    spi.TargetSocket.Connect(HttpProxyEP);

                                                    if (Socket_Operation.CheckHttpProxyState(spi, sIPString, uPort))
                                                    {
                                                        spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);
                                                        spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                        Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));                                                        
                                                    }
                                                    else
                                                    {
                                                        Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                                    }
                                                }
                                                else
                                                {  
                                                    spi.TargetSocket.Connect(targetEP);
                                                    spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);
                                                    spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                    Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));
                                                    Socket_Cache.SocketProxyQueue.ProxyInfoToQueue(spi);
                                                }                                                
                                            }
                                            catch (SocketException)
                                            {
                                                Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                            }

                                            #endregion

                                            break;

                                        case Socket_Cache.SocketProxy.DomainType.Https:

                                            #region//HTTPS 协议

                                            try
                                            {
                                                if (Socket_Cache.SocketProxy.Enable_EXTHttps)
                                                {
                                                    IPEndPoint HttpsProxyEP = new IPEndPoint(IPAddress.Parse(Socket_Cache.SocketProxy.EXTHttpsIP), Socket_Cache.SocketProxy.EXTHttpsPort);
                                                    spi.TargetSocket.Connect(HttpsProxyEP);

                                                    if (Socket_Operation.CheckHttpProxyState(spi, sIPString, uPort))
                                                    {
                                                        spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);
                                                        spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                        Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));                                                        
                                                    }
                                                    else
                                                    {
                                                        Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                                    }
                                                }
                                                else
                                                {
                                                    spi.TargetSocket.Connect(targetEP);
                                                    spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);
                                                    spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                    Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));
                                                    Socket_Cache.SocketProxyQueue.ProxyInfoToQueue(spi);
                                                }
                                            }
                                            catch (SocketException)
                                            {
                                                Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                            }

                                            #endregion

                                            break;

                                        case Socket_Cache.SocketProxy.DomainType.Socket:

                                            #region//Socket 协议

                                            try
                                            {
                                                spi.TargetSocket.Connect(targetEP);
                                                spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);
                                                spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));

                                                Socket_Cache.SocketProxyQueue.ProxyInfoToQueue(spi);
                                            }
                                            catch (SocketException)
                                            {
                                                Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                            }

                                            #endregion

                                            break;
                                    }

                                    #endregion

                                    break;

                                case Socket_Cache.SocketProxy.CommandType.UDP:

                                    #region//UDP 中继                                    

                                    try
                                    {
                                        IPEndPoint epUDPClient = new IPEndPoint(IPAddress.Any, 0);
                                        spi.ClientUDP = new UdpClient(epUDPClient);
                                        spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                        spi.ClientUDP.BeginReceive(new AsyncCallback(AcceptUdpCallback), spi);

                                        byte[] bServerUDP_IP = Socket_Cache.SocketProxy.ProxyUDP_IP.GetAddressBytes();
                                        byte[] bServerUDP_Port = BitConverter.GetBytes(((IPEndPoint)spi.ClientUDP.Client.LocalEndPoint).Port);                                        

                                        Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerUDP_IP, bServerUDP_Port));

                                        Socket_Cache.SocketProxyQueue.ProxyInfoToQueue(spi);
                                    }
                                    catch (SocketException)
                                    {
                                        Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                    }

                                    #endregion

                                    break;

                                default:

                                    #region//不支持的命令

                                    Socket_Operation.SendTCPData(spi.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Unsupport, bServerTCP_IP, bServerTCP_Port));

                                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_152), spi.ClientSocket.RemoteEndPoint, spi.CommandType);
                                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);

                                    #endregion

                                    break;
                            }
                        }
                        catch (SocketException ex)
                        {
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.TargetAddress + " - " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//请求数据（TCP）       

            private static void ForwardData(Socket_ProxyInfo spi, byte[] bData)
            {
                try
                {
                    if (spi.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                    {
                        switch (spi.DomainType)
                        {
                            case Socket_Cache.SocketProxy.DomainType.Http:

                                #region//HTTP

                                Socket_Operation.SendTCPData(spi.TargetSocket, bData);

                                if (!Socket_Cache.SocketProxy.Enable_EXTHttp)
                                {
                                    Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, bData, Socket_Cache.SocketProxy.DataType.Request);
                                }                                

                                #endregion

                                break;

                            case Socket_Cache.SocketProxy.DomainType.Https:

                                #region//HTTPS

                                Socket_Operation.SendTCPData(spi.TargetSocket, bData);

                                if (!Socket_Cache.SocketProxy.Enable_EXTHttps)
                                {
                                    Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, bData, Socket_Cache.SocketProxy.DataType.Request);
                                }                                

                                #endregion

                                break;

                            case Socket_Cache.SocketProxy.DomainType.Socket:

                                #region//Socket

                                Socket_Operation.SendTCPData(spi.TargetSocket, bData);
                                Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, bData, Socket_Cache.SocketProxy.DataType.Request);

                                #endregion

                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.ClientAddress + " - " + ex.Message);
                }
            }

            #endregion

            #region//响应数据（TCP）

            private static void ResponseCallback(IAsyncResult ar)
            {
                Socket_ProxyInfo spi = (Socket_ProxyInfo)ar.AsyncState;

                try
                {
                    int bytesRead = spi.TargetSocket.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        spi.TargetData = new byte[bytesRead];
                        Buffer.BlockCopy(spi.TargetBuffer, 0, spi.TargetData, 0, bytesRead);

                        if (spi.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                        {
                            switch (spi.DomainType)
                            { 
                                case Socket_Cache.SocketProxy.DomainType.Http:

                                    Socket_Operation.SendTCPData(spi.ClientSocket, spi.TargetData);

                                    if (!Socket_Cache.SocketProxy.Enable_EXTHttp)
                                    {
                                        Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, spi.TargetData, Socket_Cache.SocketProxy.DataType.Response);
                                    }                                        

                                    break;

                                case Socket_Cache.SocketProxy.DomainType.Https:

                                    Socket_Operation.SendTCPData(spi.ClientSocket, spi.TargetData);

                                    if (!Socket_Cache.SocketProxy.Enable_EXTHttps)
                                    {
                                        Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, spi.TargetData, Socket_Cache.SocketProxy.DataType.Response);
                                    }                                    

                                    break;

                                case Socket_Cache.SocketProxy.DomainType.Socket:

                                    Socket_Operation.SendTCPData(spi.ClientSocket, spi.TargetData);
                                    Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, spi.TargetData, Socket_Cache.SocketProxy.DataType.Response);

                                    break;
                            }                            
                        }
                        
                        spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);
                    }
                    else
                    {
                        spi.CloseTCPTarget();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.TargetAddress + " - " + ex.Message);
                    spi.CloseTCPTarget();
                }
            }

            #endregion            

            #region//数据转发（UDP）

            private static void AcceptUdpCallback(IAsyncResult ar)
            {
                Socket_ProxyInfo spi = (Socket_ProxyInfo)ar.AsyncState;

                try
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] bData = Socket_Operation.ReceiveUDPData(spi.ClientUDP, ar, ref remoteEndPoint);

                    if (bData != null && !remoteEndPoint.Address.Equals(IPAddress.Any) && remoteEndPoint.Port != 0)
                    {
                        if (bData.Length > 0)
                        {
                            if (bData[0].Equals(0) && bData[1].Equals(0) && bData[2].Equals(0))
                            {
                                Socket_Cache.SocketProxy.AddressType addressType = (Socket_Cache.SocketProxy.AddressType)bData[3];

                                if (addressType == Socket_Cache.SocketProxy.AddressType.IPV4 || addressType == Socket_Cache.SocketProxy.AddressType.IPV6 || addressType == Socket_Cache.SocketProxy.AddressType.Domain)
                                {
                                    spi.ClientUDP_EndPoint = remoteEndPoint;

                                    byte[] bADDRESS = new byte[bData.Length - 4];
                                    Buffer.BlockCopy(bData, 4, bADDRESS, 0, bADDRESS.Length);
                                    IPEndPoint targetEndPoint = Socket_Operation.GetIPEndPoint_ByAddressType(addressType, bADDRESS);

                                    byte[] bUDP_Data = Socket_Operation.GetUDPData_ByAddressType(addressType, bData);

                                    if (bUDP_Data.Length > 0)
                                    {
                                        spi.ClientUDP_Time = DateTime.Now;
                                        Socket_Cache.SocketProxy.Total_Request += bUDP_Data.Length;
                                        Socket_Operation.SendUDPData(spi.ClientUDP, bUDP_Data, targetEndPoint);
                                    }
                                }
                            }
                            else
                            {
                                byte[] bIP = spi.ClientUDP_EndPoint.Address.GetAddressBytes();
                                byte[] bPort = BitConverter.GetBytes(spi.ClientUDP_EndPoint.Port);
                                byte[] bResponseData = new byte[] { 0x00, 0x00, 0x00, (byte)Socket_Cache.SocketProxy.AddressType.IPV4, bIP[0], bIP[1], bIP[2], bIP[3], bPort[1], bPort[0] };
                                bResponseData = bResponseData.Concat(bData).ToArray();

                                if (bResponseData.Length > 0)
                                {
                                    spi.ClientUDP_Time = DateTime.Now;
                                    Socket_Cache.SocketProxy.Total_Response += bResponseData.Length;
                                    Socket_Operation.SendUDPData(spi.ClientUDP, bResponseData, spi.ClientUDP_EndPoint);
                                }
                            }
                        }

                        Socket_Cache.SocketProxy.ProxyUDP_CNT++;
                        spi.ClientUDP.BeginReceive(new AsyncCallback(AcceptUdpCallback), spi);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//获取客户端的名称

            public static string GetClientName(Socket_ProxyInfo spi)
            {
                string sReturn = string.Empty;

                try
                {
                    if (spi != null && !string.IsNullOrEmpty(spi.ClientAddress))
                    {
                        if (spi.ClientSocket != null)
                        {
                            if (spi.ClientSocket.RemoteEndPoint != null)
                            {
                                if (((IPEndPoint)spi.ClientSocket.RemoteEndPoint).Address != null)
                                {
                                    sReturn = ((IPEndPoint)spi.ClientSocket.RemoteEndPoint).Address.ToString();
                                }                                
                            }
                        }                     
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return sReturn;
            }

            #endregion
        }

        #endregion               

        #region//代理队列

        public static class SocketProxyQueue
        {
            public static ConcurrentQueue<Socket_ProxyInfo> qSocket_ProxyInfo = new ConcurrentQueue<Socket_ProxyInfo>();
            public static ConcurrentQueue<Socket_ProxyData> qSocket_ProxyData = new ConcurrentQueue<Socket_ProxyData>();

            #region//代理入队列

            public static void ProxyInfoToQueue(Socket_ProxyInfo spi)
            {
                try
                {
                    qSocket_ProxyInfo.Enqueue(spi);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//代理数据入队列

            public static void ProxyDataToQueue(Socket_ProxyInfo spi, byte[] bData, Socket_Cache.SocketProxy.DataType DataType)
            {
                try
                {
                    Socket_ProxyData spd = new Socket_ProxyData(spi.TargetAddress, spi.DomainType, bData, DataType);
                    qSocket_ProxyData.Enqueue(spd);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//清除队列数据

            public static void ResetProxyInfoQueue()
            {
                try
                {
                    while (!qSocket_ProxyInfo.IsEmpty)
                    {
                        qSocket_ProxyInfo.TryDequeue(out Socket_ProxyInfo spi);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void ResetProxyDataQueue()
            {
                try
                {
                    while (!qSocket_ProxyData.IsEmpty)
                    {
                        qSocket_ProxyData.TryDequeue(out Socket_ProxyData spd);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion

        #region//代理列表

        public static class SocketProxyList
        {
            public static bool NoRecord, DelClosed;

            public static BindingList<Socket_ProxyInfo> lstProxyInfo = new BindingList<Socket_ProxyInfo>();
            public delegate void ProxyInfoReceived(Socket_ProxyInfo spi);
            public static event ProxyInfoReceived RecProxyInfo;

            public static BindingList<Socket_ProxyData> lstProxyData = new BindingList<Socket_ProxyData>();
            public delegate void ProxyDataReceived(Socket_ProxyData spd);
            public static event ProxyDataReceived RecProxyData;

            #region//代理入列表

            public static async Task ProxyInfoToList()
            {
                try
                {
                    await Task.Run(() =>
                    {
                        if (Socket_Cache.SocketProxyQueue.qSocket_ProxyInfo.TryDequeue(out Socket_ProxyInfo spi))
                        {
                            lstProxyInfo.Add(spi);
                            RecProxyInfo?.Invoke(spi);
                        }
                    });                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//代理数据入列表

            public static async Task ProxyDataToList()
            {
                try
                {
                    await Task.Run(() =>
                    {
                        if (Socket_Cache.SocketProxyQueue.qSocket_ProxyData.TryDequeue(out Socket_ProxyData spd))
                        {
                            switch (spd.DataType)
                            {
                                case SocketProxy.DataType.Request:
                                    Socket_Cache.SocketProxy.Total_Request += spd.Buffer.Length;
                                    break;

                                case SocketProxy.DataType.Response:
                                    Socket_Cache.SocketProxy.Total_Response += spd.Buffer.Length;
                                    break;
                            }

                            Socket_Cache.SocketProxy.ProxyTCP_CNT++;
                            RecProxyData?.Invoke(spd);
                        }
                    });                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//清除列表数据

            public static void ResetProxyInfoList()
            {
                try
                {
                    lstProxyInfo.Clear();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void ResetProxyDataList()
            {
                try
                {
                    lstProxyData.Clear();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion

        #region//封包

        public static class SocketPacket
        {
            public static int PacketData_MaxLen = 60;
            public static long TotalPackets = 0;
            public static long Total_SendBytes = 0;
            public static long Total_RecvBytes = 0;
            public static bool SpeedMode;
            public static byte[] bByteBuff = new byte[0];
            public static bool Support_WS1, Support_WS2, Support_MsWS;
            public static bool HookWS1_Send, HookWS1_SendTo, HookWS1_Recv, HookWS1_RecvFrom;
            public static bool HookWS2_Send, HookWS2_SendTo, HookWS2_Recv, HookWS2_RecvFrom;
            public static bool HookWSA_Send, HookWSA_SendTo, HookWSA_Recv, HookWSA_RecvFrom;
            public static bool CheckNotShow, CheckSize, CheckSocket, CheckIP, CheckPort, CheckHead, CheckData;
            public static string CheckSocket_Value, CheckLength_Value, CheckIP_Value, CheckPort_Value, CheckHead_Value, CheckData_Value;

            #region//结构定义

            [StructLayout(LayoutKind.Sequential)]

            public struct SockAddr
            {
                public short sin_family;
                public ushort sin_port;
                public uint sin_addr;
                private Int64 Zero;

                public void MarshalFromNative(IntPtr native)
                {
                    Marshal.PtrToStructure(native, this);

                    sin_port = (ushort)(((sin_port & 0xFF) << 8) | ((sin_port >> 8) & 0xFF));
                }

                public void MarshalToNative(IntPtr native)
                {
                    sin_port = (ushort)(((sin_port & 0xFF) << 8) | ((sin_port >> 8) & 0xFF));

                    Marshal.StructureToPtr(this, native, true);
                }
            }

            [StructLayout(LayoutKind.Sequential)]

            public struct WSABUF
            {
                public int len;
                public IntPtr buf;
            }

            [StructLayout(LayoutKind.Sequential)]

            public struct OVERLAPPED
            {
                public UIntPtr InternalLow;
                public UIntPtr InternalHigh;
                public int OffsetLow;
                public int OffsetHigh;
                public IntPtr EventHandle;
            }

            public enum PacketType
            {
                WS1_Send = 0,
                WS2_Send = 1,
                WS1_SendTo = 2,
                WS2_SendTo = 3,
                WS1_Recv = 4,
                WS2_Recv = 5,
                WS1_RecvFrom = 6,
                WS2_RecvFrom = 7,
                WSASend = 8,
                WSASendTo = 9,
                WSARecv = 10,
                WSARecvEx = 11,
                WSARecvFrom = 12,
            }

            public enum IPType
            {
                From = 0,
                To = 1,
            }

            public enum EncodingFormat
            {
                Default = 0,
                Char = 1,
                Byte = 2,
                Bytes = 3,
                Short = 4,
                UShort = 5,
                Int32 = 6,
                UInt32 = 7,
                Int64 = 8,
                UInt64 = 9,
                Float = 10,
                Double = 11,
                Bin = 12,
                GBK = 13,
                Unicode = 14,
                ASCII = 15,
                Hex = 16,
                UTF7 = 17,
                UTF8 = 18,
                UTF16 = 19,
                UTF32 = 20,
                Base64 = 21,
            }

            #endregion

            #region//获取封包类型

            public static Socket_Cache.SocketPacket.PacketType GetPacketType_ByString(string PacketType)
            {
                Socket_Cache.SocketPacket.PacketType ptReturn = new Socket_Cache.SocketPacket.PacketType();

                try
                {
                    ptReturn = (Socket_Cache.SocketPacket.PacketType)Enum.Parse(typeof(Socket_Cache.SocketPacket.PacketType), PacketType);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return ptReturn;
            }

            #endregion            

            #region//获取封包类型对应的名称

            public static string GetName_ByPacketType(Socket_Cache.SocketPacket.PacketType socketType)
            {
                string sReturn = string.Empty;

                try
                {
                    switch (socketType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_54);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_156);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_55);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_157);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_56);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_158);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_57);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_159);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_58);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_59);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_59);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_60);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_61);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return sReturn;
            }

            #endregion            

            #region//获取封包类型对应的图标

            public static Image GetImg_ByPacketType(Socket_Cache.SocketPacket.PacketType socketType)
            {
                Image imgReturn = null;

                try
                {
                    switch (socketType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                            imgReturn = Properties.Resources.sent;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                            imgReturn = Properties.Resources.sent;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                            imgReturn = Properties.Resources.received;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                            imgReturn = Properties.Resources.received;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                            imgReturn = Properties.Resources.sent;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                            imgReturn = Properties.Resources.sent;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                            imgReturn = Properties.Resources.received;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                            imgReturn = Properties.Resources.received;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                            imgReturn = Properties.Resources.sent;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                            imgReturn = Properties.Resources.received;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                            imgReturn = Properties.Resources.received;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                            imgReturn = Properties.Resources.sent;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            imgReturn = Properties.Resources.received;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return imgReturn;
            }

            #endregion
        }

        #endregion

        #region//封包队列

        public static class SocketQueue
        {            
            public static int Send_CNT = 0;
            public static int SendTo_CNT = 0;
            public static int Recv_CNT = 0;
            public static int RecvFrom_CNT = 0;
            public static int WSASend_CNT = 0;
            public static int WSASendTo_CNT = 0;
            public static int WSARecv_CNT = 0;
            public static int WSARecvFrom_CNT = 0;
            public static int FilterSocketList_CNT = 0;                       

            public static ConcurrentQueue<Socket_PacketInfo> qSocket_PacketInfo = new ConcurrentQueue<Socket_PacketInfo>();

            #region//封包入队列            

            public static void SocketPacket_ToQueue(
                int iSocket,
                byte[] bRawBuff,
                byte[] bBuffByte, 
                Socket_Cache.SocketPacket.PacketType ptPacketType, 
                Socket_Cache.SocketPacket.SockAddr sAddr,
                Socket_Cache.Filter.FilterAction pAction)
            {
                try
                {
                    string sPacketIP = Socket_Operation.GetIPString_BySocketAddr(iSocket, sAddr, ptPacketType);                    

                    if (!string.IsNullOrEmpty(sPacketIP) && sPacketIP.IndexOf("|") > 0)
                    {
                        string sIPFrom = sPacketIP.Split('|')[0];
                        string sIPTo = sPacketIP.Split('|')[1];
                        DateTime dtTime = DateTime.Now;

                        Socket_PacketInfo spi = new Socket_PacketInfo(dtTime, iSocket, ptPacketType, sIPFrom, sIPTo, bRawBuff, bBuffByte, bBuffByte.Length, pAction);
                        qSocket_PacketInfo.Enqueue(spi);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }                
            }

            #endregion

            #region//清除队列数据

            public static void ResetSocketQueue()
            {
                try
                {
                    while (!qSocket_PacketInfo.IsEmpty)
                    {
                        qSocket_PacketInfo.TryDequeue(out Socket_PacketInfo spi);
                    }                      
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion

        #region//封包列表

        public static class SocketList
        {
            public static bool DoSearch;
            public static bool AutoRoll;
            public static bool AutoClear;
            public static decimal AutoClear_Value;
            public static int Select_Index = -1, Search_Index = -1;
            public static FindOptions FindOptions = new FindOptions();
            public static BindingList<Socket_PacketInfo> lstRecPacket = new BindingList<Socket_PacketInfo>();
            public delegate void SocketPacketReceived(Socket_PacketInfo si);
            public static event SocketPacketReceived RecSocketPacket;

            #region//封包入列表

            public static async Task SocketToList(int iMax_DataLen)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        if (SocketQueue.qSocket_PacketInfo.TryDequeue(out Socket_PacketInfo spi))
                        {
                            bool bIsShow = Socket_Operation.IsShowSocketPacket_ByFilter(spi);
                            if (bIsShow)
                            {
                                int iPacketLen = spi.PacketLen;
                                byte[] bBuffer = spi.PacketBuffer;

                                spi.PacketData = Socket_Operation.GetPacketData_Hex(bBuffer, iMax_DataLen);

                                Socket_Cache.SocketPacket.PacketType ptType = spi.PacketType;

                                switch (ptType)
                                {
                                    case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                                        SocketQueue.Send_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                                        SocketQueue.Send_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                                        SocketQueue.SendTo_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                                        SocketQueue.SendTo_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                                        SocketQueue.Recv_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                                        SocketQueue.Recv_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                                        SocketQueue.RecvFrom_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                                        SocketQueue.RecvFrom_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WSASend:
                                        SocketQueue.WSASend_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                                        SocketQueue.WSASendTo_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WSARecv:
                                        SocketQueue.WSARecv_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                                        SocketQueue.WSARecv_CNT++;
                                        break;

                                    case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                                        SocketQueue.WSARecvFrom_CNT++;
                                        break;
                                }

                                RecSocketPacket?.Invoke(spi);
                            }
                            else
                            {
                                SocketQueue.FilterSocketList_CNT++;
                            }
                        }
                    });                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//搜索封包列表

            public static async Task<int> FindSocketList(Socket_Cache.SocketPacket.EncodingFormat efFormat, int FromIndex, string SearchData, bool MatchCase)
            {
                int iResult = -1;

                try
                {
                    await Task.Run(() =>
                    {
                        if (!string.IsNullOrEmpty(SearchData))
                        {
                            int iListCNT = Socket_Cache.SocketList.lstRecPacket.Count;

                            if (iListCNT > 0 && FromIndex < iListCNT)
                            {
                                string sSearch = "";

                                for (int i = FromIndex; i < iListCNT; i++)
                                {
                                    byte[] bSearch = Socket_Cache.SocketList.lstRecPacket[i].PacketBuffer;
                                    sSearch = Socket_Operation.BytesToString(efFormat, bSearch);

                                    if (!MatchCase)
                                    {
                                        sSearch = sSearch.ToLower();
                                        SearchData = SearchData.ToLower();
                                    }

                                    if (sSearch.IndexOf(SearchData) >= 0)
                                    {
                                        iResult = i;
                                        break;
                                    }
                                }
                            }
                        }
                    });                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iResult;
            }

            #endregion

            #region//发送封包列表中的封包

            public static void SendSocketList_ByIndex(int Index)
            {
                try
                {
                    if (Index > -1 && Index < Socket_Cache.SocketList.lstRecPacket.Count)
                    {
                        int Socket = Socket_Cache.SocketList.lstRecPacket[Index].PacketSocket;
                        Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketList.lstRecPacket[Index].PacketType;
                        string From = Socket_Cache.SocketList.lstRecPacket[Index].PacketFrom;
                        string To = Socket_Cache.SocketList.lstRecPacket[Index].PacketTo;
                        byte[] bBuffer = Socket_Cache.SocketList.lstRecPacket[Index].PacketBuffer;

                        Socket_Operation.SendPacket(Socket, ptType, From, To, bBuffer);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//保存封包列表为Excel（对话框）

            public static async void SaveSocketList_Dialog()
            {
                try
                {
                    if (Socket_Cache.SocketList.lstRecPacket.Count > 0)
                    {
                        int SaveCount = Socket_Cache.SocketList.lstRecPacket.Count;
                        SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                        sfdSaveToExcel.Filter = "Execl files (*.xls)|*.xls";
                        sfdSaveToExcel.FilterIndex = 0;
                        sfdSaveToExcel.RestoreDirectory = true;
                        sfdSaveToExcel.CreatePrompt = true;

                        sfdSaveToExcel.Title = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_76);

                        if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = sfdSaveToExcel.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_151), SaveCount);
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);

                                await SaveSocketListToExcel(FilePath, SaveCount);

                                sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_150), FilePath);
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                            }
                        }
                    }               
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static async Task<int> SaveSocketListToExcel(string FilePath, int SaveCount)
            {
                int iSuccess = 0;

                try
                {
                    await Task.Run(() =>
                    {
                        Stream myStream = File.OpenWrite(FilePath);
                        StreamWriter sw = new StreamWriter(myStream, Encoding.Default);

                        string sColTitle = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_77);
                        sw.WriteLine(sColTitle);

                        if (SaveCount > Socket_Cache.SocketList.lstRecPacket.Count)
                        {
                            SaveCount = Socket_Cache.SocketList.lstRecPacket.Count;
                        }

                        for (int i = 0; i < SaveCount; i++)
                        {
                            try
                            {
                                string sColValue = "";

                                string sTime = Socket_Cache.SocketList.lstRecPacket[i].PacketTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
                                string sType = Socket_Cache.SocketList.lstRecPacket[i].PacketType.ToString();
                                string sSocket = Socket_Cache.SocketList.lstRecPacket[i].PacketSocket.ToString();
                                string sFrom = Socket_Cache.SocketList.lstRecPacket[i].PacketFrom;
                                string sTo = Socket_Cache.SocketList.lstRecPacket[i].PacketTo;
                                string sLen = Socket_Cache.SocketList.lstRecPacket[i].PacketLen.ToString();
                                byte[] bBuff = Socket_Cache.SocketList.lstRecPacket[i].PacketBuffer;
                                string sData = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuff);

                                sColValue += sTime + "\t" + sType + "\t" + sSocket + "\t" + sFrom + "\t" + sTo + "\t" + sLen + "\t" + sData + "\t";
                                sw.WriteLine(sColValue);

                                iSuccess++;
                            }
                            catch (Exception ex)
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                            }
                        }

                        sw.Close();
                        myStream.Close();
                    });                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iSuccess;
            }

            #endregion            
        }

        #endregion

        #region//日志队列

        public static class LogQueue
        {
            public static ConcurrentQueue<Socket_LogInfo> qSocket_Log = new ConcurrentQueue<Socket_LogInfo>();
            public static ConcurrentQueue<Socket_LogInfo> qProxy_Log = new ConcurrentQueue<Socket_LogInfo>();

            #region//日志入队列

            public static void LogToQueue(Socket_Cache.LogType logType, string sFuncName, string sLogContent)
            {
                try
                {
                    Socket_LogInfo sli = new Socket_LogInfo(sFuncName, sLogContent);

                    switch (logType)
                    {
                        case LogType.Socket:
                            qSocket_Log.Enqueue(sli);
                            break;

                        case LogType.Proxy:
                            qProxy_Log.Enqueue(sli);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }            

            #endregion

            #region//清除队列数据

            public static void ResetLogQueue(Socket_Cache.LogType logType)
            {
                try
                {
                    switch (logType)
                    {
                        case LogType.Socket:

                            while (!qSocket_Log.IsEmpty)
                            {
                                qSocket_Log.TryDequeue(out Socket_LogInfo sli);
                            }

                            break;

                        case LogType.Proxy:

                            while (!qProxy_Log.IsEmpty)
                            {
                                qProxy_Log.TryDequeue(out Socket_LogInfo sli);
                            }

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion

        #region//日志列表

        public static class LogList
        {
            public static bool AutoRoll, Proxy_AutoRoll, AutoClear, Proxy_AutoClear;
            public static decimal AutoClear_Value, Proxy_AutoClear_Value;
            public static BindingList<Socket_LogInfo> lstSocketLog = new BindingList<Socket_LogInfo>();
            public static BindingList<Socket_LogInfo> lstProxyLog = new BindingList<Socket_LogInfo>();

            public delegate void SocketLogReceived(Socket_LogInfo sl);
            public delegate void ProxyLogReceived(Socket_LogInfo sli);

            public static event SocketLogReceived RecSocketLog;
            public static event ProxyLogReceived RecProxyLog;

            #region//日志入列表

            public static async Task LogToList(Socket_Cache.LogType logType)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        switch (logType)
                        {
                            case LogType.Socket:

                                if (LogQueue.qSocket_Log.TryDequeue(out Socket_LogInfo sliSocket))
                                {
                                    RecSocketLog?.Invoke(sliSocket);
                                }

                                break;

                            case LogType.Proxy:

                                if (LogQueue.qProxy_Log.TryDequeue(out Socket_LogInfo sliProxy))
                                {
                                    RecProxyLog?.Invoke(sliProxy);
                                }

                                break;
                        }
                    });                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//清除列表数据

            public static void ResetLogList(Socket_Cache.LogType logType)
            {
                try
                {
                    switch (logType)
                    {
                        case LogType.Socket:
                            lstSocketLog.Clear();
                            break;

                        case LogType.Proxy:
                            lstProxyLog.Clear();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//保存日志列表为Excel

            public static int SaveLogListToExcel()
            {
                int iSuccess = 0;

                try
                {
                    SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                    sfdSaveToExcel.Filter = "Execl files (*.xls)|*.xls";
                    sfdSaveToExcel.FilterIndex = 0;
                    sfdSaveToExcel.RestoreDirectory = true;
                    sfdSaveToExcel.CreatePrompt = true;

                    sfdSaveToExcel.Title = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_76);

                    if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                    {
                        Stream myStream = sfdSaveToExcel.OpenFile();
                        StreamWriter sw = new StreamWriter(myStream, Encoding.GetEncoding(-0));

                        string sColTitle = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_78);
                        sw.WriteLine(sColTitle);

                        foreach (Socket_LogInfo sl in Socket_Cache.LogList.lstSocketLog)
                        {
                            try
                            {
                                string sColValue = "";

                                string sTime = sl.LogTime;
                                string sFuncName = sl.FuncName;
                                string sContent = sl.LogContent;

                                sColValue += sTime + "\t" + sFuncName + "\t" + sContent + "\t";
                                sw.WriteLine(sColValue);

                                iSuccess++;
                            }
                            catch (Exception ex)
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                            }
                        }

                        sw.Close();
                        myStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iSuccess;
            }

            #endregion
        }

        #endregion

        #region//滤镜

        public static class Filter
        {
            public static long FilterExecute_CNT = 0;            
            public static int FilterSize_MaxLen = 500;

            #region//定义结构        

            public enum FilterMode
            {
                Normal,
                Advanced,
            }

            public enum FilterAction
            {  
                Replace,
                Intercept,
                NoModify_Display,
                NoModify_NoDisplay,              
                None,
            }
            
            public enum FilterStartFrom
            {
                Head,
                Position,
            }            

            public struct FilterFunction
            {
                public bool Send;
                public bool SendTo;
                public bool Recv;
                public bool RecvFrom;
                public bool WSASend;
                public bool WSASendTo;
                public bool WSARecv;
                public bool WSARecvFrom;

                public FilterFunction(bool bSend, bool bSendTo, bool bRecv, bool bRecvFrom, bool bWSASend, bool bWSASendTo, bool bWSARecv, bool bWSARecvFrom)
                {
                    Send = bSend;
                    SendTo = bSendTo;
                    Recv = bRecv;
                    RecvFrom = bRecvFrom;
                    WSASend = bWSASend;
                    WSASendTo = bWSASendTo;
                    WSARecv = bWSARecv;
                    WSARecvFrom = bWSARecvFrom;
                }
            }

            #endregion

            #region//新增滤镜            

            public static void AddFilter_New()
            {
                try
                {
                    Guid FID = Guid.NewGuid();
                    int FNum = Socket_Cache.FilterList.lstFilter.Count + 1;
                    string FName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_50), FNum.ToString());

                    Socket_Cache.Filter.FilterMode FilterMode = Socket_Cache.Filter.FilterMode.Normal;
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.Filter.FilterAction.Replace;
                    Guid RID = Guid.Empty;
                    Socket_Cache.Filter.FilterFunction FilterFunction = new Socket_Cache.Filter.FilterFunction(true, true, true, true, false, false, false, false);
                    Socket_Cache.Filter.FilterStartFrom FilterStartFrom = Socket_Cache.Filter.FilterStartFrom.Head;

                    Socket_Cache.Filter.AddFilter(false, FID, FName, false, string.Empty, false, 0, false, 0, FilterMode, FilterAction, false, RID, FilterFunction, FilterStartFrom, false, 1, string.Empty, 0, string.Empty, string.Empty);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void AddFilter_BySocketListIndex(int iSLIndex, byte[] bBuffer)
            {
                try
                {
                    if (SocketList.lstRecPacket.Count > 0 && iSLIndex >= 0)
                    {
                        Guid FID = Guid.NewGuid();
                        int iIndex = iSLIndex + 1;
                        string sFName = Process.GetCurrentProcess().ProcessName.Trim() + " [" + iIndex.ToString() + "]";
                        Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketList.lstRecPacket[iSLIndex].PacketType;

                        if (bBuffer == null || bBuffer.Length == 0)
                        {
                            bBuffer = Socket_Cache.SocketList.lstRecPacket[iSLIndex].PacketBuffer;
                        }

                        Socket_Cache.Filter.FilterMode FilterMode = Socket_Cache.Filter.FilterMode.Normal;
                        Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.Filter.FilterAction.Replace;
                        Guid RID = Guid.Empty;
                        Socket_Cache.Filter.FilterFunction FilterFunction = Socket_Cache.Filter.GetFilterFunction_ByPacketType(ptType);
                        Socket_Cache.Filter.FilterStartFrom FilterStartFrom = Socket_Cache.Filter.FilterStartFrom.Head;

                        string sFSearch = Socket_Cache.Filter.GetFilterString_ByBytes(bBuffer);

                        Socket_Cache.Filter.AddFilter(false, FID, sFName, false, string.Empty, false, 0, false, 0, FilterMode, FilterAction, false, RID, FilterFunction, FilterStartFrom, false, 1, string.Empty, 0, sFSearch, string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void AddFilter(
                bool IsEnable,
                Guid FID,
                string FName,
                bool bAppointHeader,
                string HeaderContent,
                bool bAppointSocket,
                decimal SocketContent,
                bool bAppointLength,
                decimal LengthContent,
                Socket_Cache.Filter.FilterMode FilterMode,
                Socket_Cache.Filter.FilterAction FilterAction,
                bool IsExecute,
                Guid RID,
                Socket_Cache.Filter.FilterFunction FilterFunction,
                Socket_Cache.Filter.FilterStartFrom FilterStartFrom,
                bool IsProgressionDone,
                decimal ProgressionStep,
                string ProgressionPosition,
                int ProgressionCount,
                string FSearch,
                string FModify)
            {
                try
                {
                    if (FID != null && !string.IsNullOrEmpty(FName))
                    {
                        Socket_FilterInfo sfi = new Socket_FilterInfo(
                        IsEnable,
                        FID,
                        FName,
                        bAppointHeader,
                        HeaderContent,
                        bAppointSocket,
                        SocketContent,
                        bAppointLength,
                        LengthContent,
                        FilterMode,
                        FilterAction,
                        IsExecute,
                        RID,
                        FilterFunction,
                        FilterStartFrom,
                        IsProgressionDone,
                        ProgressionStep,
                        ProgressionPosition,
                        ProgressionCount,
                        FSearch,
                        FModify);

                        Socket_Cache.FilterList.FilterToList(sfi);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }            

            #endregion

            #region//更新滤镜

            public static void UpdateFilter_ByFilterIndex(
                int iFIndex,
                string FName,
                bool AppointHeader,
                string HeaderContent,
                bool AppointSocket,
                decimal SocketContent,
                bool AppointLength,
                decimal LengthContent,
                Socket_Cache.Filter.FilterMode FilterMode,
                Socket_Cache.Filter.FilterAction FilterAction,
                bool IsExecute,
                Guid RID,
                Socket_Cache.Filter.FilterFunction FilterFunction,
                Socket_Cache.Filter.FilterStartFrom FilterStartFrom,
                decimal ProgressionStep,
                string ProgressionPosition,
                string FSearch,
                string FModify)
            {
                try
                {
                    if (iFIndex > -1)
                    {
                        Socket_Cache.FilterList.lstFilter[iFIndex].FName = FName;
                        Socket_Cache.FilterList.lstFilter[iFIndex].AppointHeader = AppointHeader;
                        Socket_Cache.FilterList.lstFilter[iFIndex].HeaderContent = HeaderContent;
                        Socket_Cache.FilterList.lstFilter[iFIndex].AppointSocket = AppointSocket;
                        Socket_Cache.FilterList.lstFilter[iFIndex].SocketContent = SocketContent;
                        Socket_Cache.FilterList.lstFilter[iFIndex].AppointLength = AppointLength;
                        Socket_Cache.FilterList.lstFilter[iFIndex].LengthContent = LengthContent;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FMode = FilterMode;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FAction = FilterAction;
                        Socket_Cache.FilterList.lstFilter[iFIndex].IsExecute = IsExecute;
                        Socket_Cache.FilterList.lstFilter[iFIndex].RID = RID;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FFunction = FilterFunction;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FStartFrom = FilterStartFrom;
                        Socket_Cache.FilterList.lstFilter[iFIndex].ProgressionStep = ProgressionStep;
                        Socket_Cache.FilterList.lstFilter[iFIndex].ProgressionPosition = ProgressionPosition;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FSearch = FSearch;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FModify = FModify;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//删除滤镜

            public static void DeleteFilter_ByFilterIndex_Dialog(int FIndex)
            {
                try
                {
                    if (FIndex > -1)
                    {
                        DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                        if (dr.Equals(DialogResult.OK))
                        {
                            Socket_Cache.Filter.DeleteFilter_ByFilterIndex(FIndex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void DeleteFilter_ByFilterIndex(int FIndex)
            {
                try
                {
                    if (FIndex > -1)
                    {
                        Socket_Cache.FilterList.lstFilter.RemoveAt(FIndex);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//复制滤镜

            public static void CopyFilter_ByFilterIndex(int iFIndex)
            {
                try
                {
                    bool IsEnable = false;
                    Guid FID = Guid.NewGuid();
                    string FName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_62), Socket_Cache.FilterList.lstFilter[iFIndex].FName);
                    bool bAppointHeader = Socket_Cache.FilterList.lstFilter[iFIndex].AppointHeader;
                    string HeaderContent = Socket_Cache.FilterList.lstFilter[iFIndex].HeaderContent;
                    bool bAppointSocket = Socket_Cache.FilterList.lstFilter[iFIndex].AppointSocket;
                    decimal SocketContent = Socket_Cache.FilterList.lstFilter[iFIndex].SocketContent;
                    bool bAppointLength = Socket_Cache.FilterList.lstFilter[iFIndex].AppointLength;
                    decimal LengthContent = Socket_Cache.FilterList.lstFilter[iFIndex].LengthContent;
                    Socket_Cache.Filter.FilterMode FMode = Socket_Cache.FilterList.lstFilter[iFIndex].FMode;
                    Socket_Cache.Filter.FilterAction FAction = Socket_Cache.FilterList.lstFilter[iFIndex].FAction;
                    bool IsExecute = Socket_Cache.FilterList.lstFilter[iFIndex].IsExecute;
                    Guid RID = Socket_Cache.FilterList.lstFilter[iFIndex].RID;
                    Socket_Cache.Filter.FilterFunction FFunction = Socket_Cache.FilterList.lstFilter[iFIndex].FFunction;
                    Socket_Cache.Filter.FilterStartFrom FStartFrom = Socket_Cache.FilterList.lstFilter[iFIndex].FStartFrom;
                    bool IsProgressionDone = false;
                    decimal ProgressionStep = Socket_Cache.FilterList.lstFilter[iFIndex].ProgressionStep;
                    string ProgressionPosition = Socket_Cache.FilterList.lstFilter[iFIndex].ProgressionPosition;
                    int ProgressionCount = 0;
                    string FSearch = Socket_Cache.FilterList.lstFilter[iFIndex].FSearch;
                    string FModify = Socket_Cache.FilterList.lstFilter[iFIndex].FModify;

                    Socket_Cache.Filter.AddFilter(
                        IsEnable,
                        FID,
                        FName,
                        bAppointHeader,
                        HeaderContent,
                        bAppointSocket,
                        SocketContent,
                        bAppointLength,
                        LengthContent,
                        FMode,
                        FAction,
                        IsExecute,
                        RID,
                        FFunction,
                        FStartFrom,
                        IsProgressionDone,
                        ProgressionStep,
                        ProgressionPosition,
                        ProgressionCount,
                        FSearch,
                        FModify);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//获取滤镜选项

            public static Socket_Cache.Filter.FilterMode GetFilterMode_ByString(string FilterMode)
            {
                Socket_Cache.Filter.FilterMode FMode = new Socket_Cache.Filter.FilterMode();

                try
                {
                    FMode = (Socket_Cache.Filter.FilterMode)Enum.Parse(typeof(Socket_Cache.Filter.FilterMode), FilterMode);
                }
                catch (Exception ex)
                {
                    FMode = Socket_Cache.Filter.FilterMode.Normal;
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return FMode;
            }

            public static Socket_Cache.Filter.FilterAction GetFilterAction_ByString(string FilterAction)
            {
                Socket_Cache.Filter.FilterAction FAction = new Socket_Cache.Filter.FilterAction();

                try
                {
                    FAction = (Socket_Cache.Filter.FilterAction)Enum.Parse(typeof(Socket_Cache.Filter.FilterAction), FilterAction);
                }
                catch (Exception ex)
                {
                    FAction = Socket_Cache.Filter.FilterAction.Replace;
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return FAction;
            }

            public static Socket_Cache.Filter.FilterFunction GetFilterFunction_ByString(string FilterFunction)
            {
                Socket_Cache.Filter.FilterFunction FFunction = new Socket_Cache.Filter.FilterFunction();

                try
                {
                    string[] slFilterFunction = FilterFunction.Split(':');

                    FFunction.Send = Convert.ToBoolean(int.Parse(slFilterFunction[0]));
                    FFunction.SendTo = Convert.ToBoolean(int.Parse(slFilterFunction[1]));
                    FFunction.Recv = Convert.ToBoolean(int.Parse(slFilterFunction[2]));
                    FFunction.RecvFrom = Convert.ToBoolean(int.Parse(slFilterFunction[3]));
                    FFunction.WSASend = Convert.ToBoolean(int.Parse(slFilterFunction[4]));
                    FFunction.WSASendTo = Convert.ToBoolean(int.Parse(slFilterFunction[5]));
                    FFunction.WSARecv = Convert.ToBoolean(int.Parse(slFilterFunction[6]));
                    FFunction.WSARecvFrom = Convert.ToBoolean(int.Parse(slFilterFunction[7]));
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return FFunction;
            }

            public static Socket_Cache.Filter.FilterStartFrom GetFilterStartFrom_ByString(string sFStartFrom)
            {
                Socket_Cache.Filter.FilterStartFrom FStartFrom = new Socket_Cache.Filter.FilterStartFrom();

                try
                {
                    FStartFrom = (Socket_Cache.Filter.FilterStartFrom)Enum.Parse(typeof(Socket_Cache.Filter.FilterStartFrom), sFStartFrom);
                }
                catch (Exception ex)
                {
                    FStartFrom = Socket_Cache.Filter.FilterStartFrom.Head;
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return FStartFrom;
            }

            #endregion

            #region//获取滤镜字符串

            public static string GetFilterString_ByBytes(byte[] bBuffer)
            {
                string sReturn = string.Empty;

                try
                {
                    for (int i = 0; i < bBuffer.Length; i++)
                    {
                        string sHex = bBuffer[i].ToString("X2");
                        sReturn += i.ToString() + "|" + sHex + ",";
                    }

                    sReturn = sReturn.Trim(',');
                }
                catch (Exception ex)
                {
                    sReturn = "";
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//获取滤镜动作对应的名称

            public static string GetName_ByFilterAction(Socket_Cache.Filter.FilterAction filterAction)
            {
                string sReturn = string.Empty;

                try
                {
                    switch (filterAction)
                    {
                        case Socket_Cache.Filter.FilterAction.Replace:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_65);
                            break;

                        case Socket_Cache.Filter.FilterAction.Intercept:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_66);
                            break;

                        case Socket_Cache.Filter.FilterAction.NoModify_Display:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_67);
                            break;

                        case Socket_Cache.Filter.FilterAction.NoModify_NoDisplay:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_68);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//获取滤镜作用类别字符串

            public static string GetFilterFunctionString(Socket_Cache.Filter.FilterFunction FilterFunction)
            {
                string sReturn = string.Empty;

                try
                {
                    sReturn += Convert.ToInt32(FilterFunction.Send) + ":";
                    sReturn += Convert.ToInt32(FilterFunction.SendTo) + ":";
                    sReturn += Convert.ToInt32(FilterFunction.Recv) + ":";
                    sReturn += Convert.ToInt32(FilterFunction.RecvFrom) + ":";
                    sReturn += Convert.ToInt32(FilterFunction.WSASend) + ":";
                    sReturn += Convert.ToInt32(FilterFunction.WSASendTo) + ":";
                    sReturn += Convert.ToInt32(FilterFunction.WSARecv) + ":";
                    sReturn += Convert.ToInt32(FilterFunction.WSARecvFrom);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//获取封包类别对应的滤镜作用类别

            public static Socket_Cache.Filter.FilterFunction GetFilterFunction_ByPacketType(Socket_Cache.SocketPacket.PacketType ptType)
            {
                Socket_Cache.Filter.FilterFunction ffReturn = new Socket_Cache.Filter.FilterFunction();

                try
                {
                    switch (ptType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                            ffReturn.Send = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                            ffReturn.Send = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                            ffReturn.SendTo = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                            ffReturn.SendTo = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                            ffReturn.Recv = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                            ffReturn.Recv = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                            ffReturn.RecvFrom = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                            ffReturn.RecvFrom = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                            ffReturn.WSASend = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                            ffReturn.WSASendTo = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                            ffReturn.WSARecv = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                            ffReturn.WSARecv = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            ffReturn.WSARecvFrom = true;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return ffReturn;
            }

            #endregion            

            #region//设置滤镜是否启用

            public static void SetIsCheck_ByFilterIndex(int FIndex, bool bCheck)
            {
                try
                {
                    if (FIndex > -1)
                    {
                        Socket_Cache.FilterList.lstFilter[FIndex].IsEnable = bCheck;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//检查滤镜是否生效

            public static bool CheckFilter_IsEffective(Int32 iSocket, byte[] bBuffer, Socket_Cache.SocketPacket.PacketType ptType, Socket_FilterInfo sfi)
            {
                bool bResult = true;

                try
                {
                    if (sfi.IsEnable)
                    {
                        if (Socket_Cache.Filter.CheckFilterFunction_ByPacketType(ptType, sfi.FFunction))
                        {
                            if (sfi.AppointSocket)
                            {
                                if (!Socket_Cache.Filter.CheckPacket_IsMatch_AppointSocket(iSocket, sfi.SocketContent))
                                {
                                    return false;
                                }
                            }

                            if (sfi.AppointLength)
                            {
                                if (!Socket_Cache.Filter.CheckPacket_IsMatch_AppointLength(bBuffer.Length, sfi.LengthContent))
                                {
                                    return false;
                                }
                            }

                            if (sfi.AppointHeader)
                            {                                
                                if (!Socket_Cache.Filter.CheckPacket_IsMatch_AppointHeader(bBuffer, sfi.HeaderContent))
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return false;
                }

                return bResult;
            }

            #endregion

            #region//检查滤镜作用类别

            public static bool CheckFilterFunction_ByPacketType(Socket_Cache.SocketPacket.PacketType ptType, Socket_Cache.Filter.FilterFunction ffFunction)
            {
                bool bReturn = false;

                try
                {
                    switch (ptType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                            bReturn = ffFunction.Send;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                            bReturn = ffFunction.Send;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                            bReturn = ffFunction.SendTo;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                            bReturn = ffFunction.SendTo;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                            bReturn = ffFunction.Recv;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                            bReturn = ffFunction.Recv;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                            bReturn = ffFunction.RecvFrom;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                            bReturn = ffFunction.RecvFrom;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                            bReturn = ffFunction.WSASend;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                            bReturn = ffFunction.WSASendTo;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                            bReturn = ffFunction.WSARecv;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                            bReturn = ffFunction.WSARecv;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            bReturn = ffFunction.WSARecvFrom;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
            }

            #endregion

            #region//检查是否匹配指定套接字

            public static bool CheckPacket_IsMatch_AppointSocket(Int32 iSocket, decimal dSocketContent)
            {
                bool bResult = false;

                try
                {
                    if (iSocket == dSocketContent)
                    {
                        bResult = true;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                }

                return bResult;
            }

            #endregion

            #region//检查是否匹配指定长度

            public static bool CheckPacket_IsMatch_AppointLength(int iLen, decimal dLengthContent)
            {
                bool bResult = false;

                try
                {
                    if (iLen == dLengthContent)
                    {
                        bResult = true;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                }

                return bResult;
            }

            #endregion

            #region//检查是否匹配指定包头

            public static bool CheckPacket_IsMatch_AppointHeader(byte[] bBuffer, string sHeaderContent)
            {
                bool bResult = false;

                try
                {
                    if (!string.IsNullOrEmpty(sHeaderContent))
                    {
                        byte[] bHeaderContent = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sHeaderContent);
                        int iHeaderContent_Len = bHeaderContent.Length;

                        if (iHeaderContent_Len > 0 && iHeaderContent_Len <= bBuffer.Length)
                        {
                            byte[] bPacketHeader = new byte[iHeaderContent_Len];
                            Buffer.BlockCopy(bBuffer, 0, bPacketHeader, 0, iHeaderContent_Len);                            
                            string sPacketHeader = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bPacketHeader);

                            if (sHeaderContent.Equals(sPacketHeader))
                            {
                                bResult = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                }

                return bResult;
            }

            #endregion

            #region//检查滤镜是否匹配成功（普通滤镜）

            public static bool CheckFilter_IsMatch_Normal(Socket_FilterInfo sfi, byte[] bBuffer)
            {
                bool bResult = true;

                try
                {
                    if (!string.IsNullOrEmpty(sfi.FSearch))
                    {
                        string[] slSearch = sfi.FSearch.Split(',');

                        foreach (string sSearch in slSearch)
                        {
                            if (!string.IsNullOrEmpty(sSearch) && sSearch.IndexOf("|") > 0)
                            {
                                if (int.TryParse(sSearch.Split('|')[0], out int iIndex))
                                {
                                    string sValue = sSearch.Split('|')[1];

                                    if (iIndex > -1 && iIndex < bBuffer.Length)
                                    {
                                        string sBuffValue = bBuffer[iIndex].ToString("X2");

                                        if (!sValue.Equals(sBuffValue))
                                        {
                                            bResult = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        bResult = false;
                                        break;
                                    }
                                }                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                    bResult = false;
                }

                return bResult;
            }

            #endregion

            #region//检查滤镜是否匹配成功（高级滤镜）

            public static List<int> CheckFilter_IsMatch_Adcanced(Socket_FilterInfo sfi, byte[] bBuffer)
            {
                List<int> lReturn = new List<int>();

                try
                {
                    if (!string.IsNullOrEmpty(sfi.FSearch))
                    {
                        Dictionary<int, int> dSearchIndex = new Dictionary<int, int>();
                        Dictionary<int, byte> dSearchValue = new Dictionary<int, byte>();

                        string[] slSearch = sfi.FSearch.Split(',');

                        for (int i = 0; i < slSearch.Length; i++)
                        {
                            if (int.TryParse(slSearch[i].Split('|')[0], out int iIndex))
                            {
                                string sValue = slSearch[i].Split('|')[1];
                                byte bValue = Convert.ToByte(sValue, 16);

                                dSearchIndex.Add(i, iIndex);
                                dSearchValue.Add(i, bValue);
                            }                            
                        }

                        int iMatchIndex = -1;
                        int iBuffIndex = -1;

                        byte bFirst_SearchValue = dSearchValue[0];

                        for (int i = 0; i < bBuffer.Length; i++)
                        {
                            if (bBuffer[i] == bFirst_SearchValue)
                            {
                                iMatchIndex = i;

                                for (int j = 1; j < slSearch.Length; j++)
                                {
                                    int iIndex = dSearchIndex[j];
                                    byte bValue = dSearchValue[j];

                                    iBuffIndex = i + iIndex;

                                    if (iBuffIndex >= 0 && iBuffIndex < bBuffer.Length)
                                    {
                                        if (bBuffer[iBuffIndex] != bValue)
                                        {
                                            iMatchIndex = -1;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        iMatchIndex = -1;
                                        break;
                                    }
                                }

                                if (iMatchIndex > -1)
                                {
                                    lReturn.Add(iMatchIndex);

                                    if (iBuffIndex > i)
                                    {
                                        i = iBuffIndex;
                                    }

                                    if (sfi.FStartFrom == Socket_Cache.Filter.FilterStartFrom.Head)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                }

                return lReturn;
            }

            #endregion            

            #region//执行滤镜（普通滤镜）

            public static bool DoFilter_Normal(Socket_FilterInfo sfi, byte[] bBuffer)
            {
                bool bReturn = true;

                try
                {
                    if (string.IsNullOrEmpty(sfi.FSearch))
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(sfi.FModify) && string.IsNullOrEmpty(sfi.ProgressionPosition))
                    {
                        return false;
                    }

                    if (!string.IsNullOrEmpty(sfi.FModify))
                    {
                        string[] slModify = sfi.FModify.Split(',');

                        foreach (string sModify in slModify)
                        {
                            if (!string.IsNullOrEmpty(sModify) && sModify.IndexOf("|") > 0)
                            {
                                if (int.TryParse(sModify.Split('|')[0], out int iIndex))
                                {
                                    string sValue = sModify.Split('|')[1];

                                    if (iIndex > -1 && iIndex < bBuffer.Length)
                                    {
                                        byte bValue = Convert.ToByte(sValue, 16);
                                        bBuffer[iIndex] = bValue;
                                    }
                                }                                
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sfi.ProgressionPosition))
                    {
                        int iStep = ((int)sfi.ProgressionStep);
                        string[] slProgression = sfi.ProgressionPosition.Split(',');

                        foreach (string sProgression in slProgression)
                        {
                            if (!string.IsNullOrEmpty(sProgression))
                            {
                                if (int.TryParse(sProgression, out int iIndex))
                                {
                                    if (iIndex >= 0 && iIndex < bBuffer.Length)
                                    {
                                        byte bValue = bBuffer[iIndex];
                                        bValue = Socket_Operation.GetStepByte(bValue, iStep * (sfi.ProgressionCount + 1));
                                        bBuffer[iIndex] = bValue;                                        

                                        sfi.IsProgressionDone = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sfi.FName + " - " + ex.Message);
                    bReturn = false;
                }

                return bReturn;
            }

            #endregion

            #region//执行滤镜（高级滤镜）

            public static bool DoFilter_Advanced(Socket_FilterInfo sfi, int iMatch, byte[] bBuffer)
            {
                bool bReturn = true;

                try
                {
                    if (string.IsNullOrEmpty(sfi.FSearch))
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(sfi.FModify) && string.IsNullOrEmpty(sfi.ProgressionPosition))
                    {
                        return false;
                    }

                    Socket_Cache.Filter.FilterStartFrom FStartFrom = sfi.FStartFrom;

                    if (!string.IsNullOrEmpty(sfi.FModify))
                    {
                        string[] slModify = sfi.FModify.Split(',');

                        foreach (string sModify in slModify)
                        {
                            if (!string.IsNullOrEmpty(sModify) && sModify.IndexOf("|") > 0)
                            {
                                if (int.TryParse(sModify.Split('|')[0], out int iIndex))
                                {
                                    string sValue = sModify.Split('|')[1];

                                    if (FStartFrom == Socket_Cache.Filter.FilterStartFrom.Position)
                                    {
                                        iIndex += iMatch;
                                    }

                                    if (iIndex > -1 && iIndex < bBuffer.Length)
                                    {
                                        byte bValue = Convert.ToByte(sValue, 16);
                                        bBuffer[iIndex] = bValue;                                        
                                    }
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sfi.ProgressionPosition))
                    {
                        int iStep = ((int)sfi.ProgressionStep);
                        string[] slProgression = sfi.ProgressionPosition.Split(',');

                        foreach (string sProgression in slProgression)
                        {
                            if (!string.IsNullOrEmpty(sProgression))
                            {
                                if (int.TryParse(sProgression, out int iIndex))
                                {
                                    if (FStartFrom == Socket_Cache.Filter.FilterStartFrom.Position)
                                    {
                                        iIndex += iMatch;
                                    }

                                    if (iIndex > -1 && iIndex < bBuffer.Length)
                                    {
                                        byte bValue = bBuffer[iIndex];
                                        bValue = Socket_Operation.GetStepByte(bValue, iStep * (sfi.ProgressionCount + 1));
                                        bBuffer[iIndex] = bValue;                                        

                                        sfi.IsProgressionDone = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sfi.FName + " - " + ex.Message);
                    bReturn = false;
                }

                return bReturn;
            }

            #endregion
        }

        #endregion

        #region//滤镜列表

        public static class FilterList
        {
            public enum Execute
            {
                Priority,
                Sequence,
            }

            public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\FilterList.fp";
            public static string AESKey = string.Empty;
            public static Execute FilterList_Execute;            

            public static BindingList<Socket_FilterInfo> lstFilter = new BindingList<Socket_FilterInfo>();
            public delegate void SocketFilterReceived(Socket_FilterInfo sfi);
            public static event SocketFilterReceived RecSocketFilter;

            #region//滤镜入列表

            public static void FilterToList(Socket_FilterInfo sfi)
            {
                try
                {
                    RecSocketFilter?.Invoke(sfi);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//初始化所有滤镜的递进次数

            public static void InitFilterList_ProgressionCount()
            {
                try
                {
                    foreach (Socket_FilterInfo sfi in lstFilter)
                    {
                        sfi.ProgressionCount = 0;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//清空滤镜列表（对话框）

            public static void CleanUpFilterList_Dialog()
            {
                try
                {
                    DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_38));

                    if (dr.Equals(DialogResult.OK))
                    {
                        Socket_Cache.FilterList.FilterListClear();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void FilterListClear()
            {
                try
                {
                    lstFilter.Clear();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//获取滤镜列表执行模式

            public static Socket_Cache.FilterList.Execute GetFilterListExecute_ByString(string sFLExecute)
            {
                Socket_Cache.FilterList.Execute FLExecute = new Socket_Cache.FilterList.Execute();

                try
                {
                    FLExecute = (Socket_Cache.FilterList.Execute)Enum.Parse(typeof(Socket_Cache.FilterList.Execute), sFLExecute);
                }
                catch (Exception ex)
                {
                    FLExecute = Socket_Cache.FilterList.Execute.Priority;
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return FLExecute;
            }

            #endregion          

            #region//滤镜列表的列表操作

            public static int UpdateFilterList_ByListAction(Socket_Cache.ListAction listAction, int iFIndex)
            {
                int iReturn = -1;

                try
                {
                    int iFilterListCount = Socket_Cache.FilterList.lstFilter.Count;
                    Socket_FilterInfo sfi = Socket_Cache.FilterList.lstFilter[iFIndex];

                    switch (listAction)
                    {
                        case Socket_Cache.ListAction.Top:
                            if (iFIndex > 0)
                            {
                                Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                                Socket_Cache.FilterList.lstFilter.Insert(0, sfi);
                                iReturn = 0;
                            }
                            break;

                        case Socket_Cache.ListAction.Up:
                            if (iFIndex > 0)
                            {
                                Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                                Socket_Cache.FilterList.lstFilter.Insert(iFIndex - 1, sfi);
                                iReturn = iFIndex - 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Down:
                            if (iFIndex < iFilterListCount - 1)
                            {
                                Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                                Socket_Cache.FilterList.lstFilter.Insert(iFIndex + 1, sfi);
                                iReturn = iFIndex + 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Bottom:
                            if (iFIndex < iFilterListCount - 1)
                            {
                                Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                                Socket_Cache.FilterList.lstFilter.Add(sfi);
                                iReturn = Socket_Cache.FilterList.lstFilter.Count - 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Copy:
                            Socket_Cache.Filter.CopyFilter_ByFilterIndex(iFIndex);
                            iReturn = Socket_Cache.FilterList.lstFilter.Count - 1;
                            break;

                        case Socket_Cache.ListAction.Export:
                            string sFName = Socket_Cache.FilterList.lstFilter[iFIndex].FName;
                            Socket_Cache.FilterList.SaveFilterList_Dialog(sFName, iFIndex);
                            iReturn = iFIndex;
                            break;

                        case Socket_Cache.ListAction.Delete:
                            Socket_Cache.Filter.DeleteFilter_ByFilterIndex_Dialog(iFIndex);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
            }

            #endregion                        

            #region//执行滤镜列表

            public static Socket_Cache.Filter.FilterAction DoFilterList(Socket_Cache.SocketPacket.PacketType ptType, Int32 iSocket, byte[] bBuffer)
            {
                bool bBreak = false;
                string sFName = string.Empty;
                Socket_Cache.Filter.FilterAction faReturn = Filter.FilterAction.None;                

                try
                {
                    foreach (Socket_FilterInfo sfi in Socket_Cache.FilterList.lstFilter)
                    {
                        sFName = sfi.FName;
                        bool bIsEffective = Socket_Cache.Filter.CheckFilter_IsEffective(iSocket, bBuffer, ptType, sfi);

                        if (bIsEffective)
                        {
                            int iMatchCNT = 0;
                            bool bDoFilter = true;

                            if (sfi.FMode == Filter.FilterMode.Normal)
                            {
                                if (Socket_Cache.Filter.CheckFilter_IsMatch_Normal(sfi, bBuffer))
                                {
                                    faReturn = sfi.FAction;

                                    switch (sfi.FAction)
                                    {
                                        case Filter.FilterAction.Replace:

                                            sfi.IsProgressionDone = false;

                                            bDoFilter = Socket_Cache.Filter.DoFilter_Normal(sfi, bBuffer);

                                            if (sfi.IsProgressionDone)
                                            {
                                                sfi.ProgressionCount++;
                                            }

                                            if (bDoFilter)
                                            {
                                                if (Socket_Cache.FilterList.FilterList_Execute == Execute.Priority)
                                                {
                                                    bBreak = true;
                                                }
                                            }

                                            break;

                                        case Filter.FilterAction.Intercept:                                                                                        
                                            bBreak = true;
                                            break;

                                        case Filter.FilterAction.NoModify_Display:
                                            bBreak = true;
                                            break;

                                        case Filter.FilterAction.NoModify_NoDisplay:
                                            bBreak = true;
                                            break;                                        
                                    }
                                }
                                else
                                {
                                    bDoFilter = false;
                                }
                            }
                            else if (sfi.FMode == Filter.FilterMode.Advanced)
                            {
                                List<int> MatchIndex = Socket_Cache.Filter.CheckFilter_IsMatch_Adcanced(sfi, bBuffer);

                                if (MatchIndex.Count > 0)
                                {
                                    faReturn = sfi.FAction;

                                    switch (sfi.FAction)
                                    {
                                        case Filter.FilterAction.Replace:

                                            sfi.IsProgressionDone = false;

                                            foreach (int iIndex in MatchIndex)
                                            {
                                                Socket_Cache.Filter.DoFilter_Advanced(sfi, iIndex, bBuffer);
                                            }

                                            if (sfi.IsProgressionDone)
                                            {
                                                sfi.ProgressionCount++;
                                            }

                                            if (sfi.FStartFrom == Filter.FilterStartFrom.Position)
                                            {
                                                iMatchCNT = MatchIndex.Count;
                                            }

                                            if (Socket_Cache.FilterList.FilterList_Execute == Execute.Priority)
                                            {
                                                bBreak = true;
                                            }

                                            break;

                                        case Filter.FilterAction.Intercept:
                                            bBreak = true;
                                            break;

                                        case Filter.FilterAction.NoModify_Display:
                                            bBreak = true;
                                            break;

                                        case Filter.FilterAction.NoModify_NoDisplay:
                                            bBreak = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    bDoFilter = false;
                                }
                            }                            

                            if (bDoFilter)
                            {
                                if (sfi.IsExecute)
                                {
                                    Socket_Cache.Robot.DoRobot(sfi.RID);
                                }

                                Socket_Cache.Filter.FilterExecute_CNT++;

                                if (!Socket_Cache.SocketPacket.SpeedMode)
                                {
                                    string sLog = string.Empty;

                                    if (iMatchCNT > 0)
                                    {
                                        sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_69), Socket_Cache.Filter.GetName_ByFilterAction(sfi.FAction), sFName, Socket_Cache.SocketPacket.GetName_ByPacketType(ptType), bBuffer.Length, iMatchCNT);
                                    }
                                    else
                                    {
                                        sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_51), Socket_Cache.Filter.GetName_ByFilterAction(sfi.FAction), sFName, Socket_Cache.SocketPacket.GetName_ByPacketType(ptType), bBuffer.Length);
                                    }

                                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                                }
                            }
                        }

                        if (bBreak)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_52), sFName, bBuffer.Length, ex.Message);
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                }

                return faReturn;
            }

            public static Socket_Cache.Filter.FilterAction DoWSAFilterList(Socket_Cache.SocketPacket.PacketType ptType, Int32 iSocket, Socket_Cache.SocketPacket.WSABUF lpBuffers, int dwBufferCount, int BytesCNT)
            {
                Socket_Cache.Filter.FilterAction faReturn = Socket_Cache.Filter.FilterAction.None;

                try
                {
                    int BytesLeft = BytesCNT;

                    for (int i = 0; i < dwBufferCount; i++)
                    {
                        if (BytesLeft > 0)
                        {
                            int iBuffLen = 0;

                            if (lpBuffers.len >= BytesLeft)
                            {
                                iBuffLen = BytesLeft;
                            }
                            else
                            {
                                iBuffLen = lpBuffers.len;
                            }

                            BytesLeft -= iBuffLen;

                            byte[] bBuffer = Socket_Operation.GetBytesFromIntPtr(lpBuffers.buf, iBuffLen);                          
                            Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, iSocket, bBuffer);
                            Marshal.Copy(bBuffer, 0, lpBuffers.buf, iBuffLen);

                            faReturn = FilterAction;

                            if (FilterAction != Filter.FilterAction.Replace && FilterAction != Filter.FilterAction.None)
                            {
                                break;
                            }                         
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return faReturn;
            }

            #endregion

            #region//保存滤镜列表（对话框）

            public static void SaveFilterList_Dialog(string FileName, int FilterIndex)
            {
                try
                {
                    if (Socket_Cache.FilterList.lstFilter.Count > 0)
                    {
                        SaveFileDialog sfdSaveFile = new SaveFileDialog();

                        sfdSaveFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_75) + "（*.fp）|*.fp";

                        if (!string.IsNullOrEmpty(FileName))
                        {
                            sfdSaveFile.FileName = FileName;
                        }

                        sfdSaveFile.RestoreDirectory = true;

                        if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                        {
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.FilterList_Export);                            
                            pwForm.ShowDialog();

                            string FilePath = sfdSaveFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                SaveFilterList(FilePath, FilterIndex, true);

                                string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_153), FilePath);
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                            }                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void SaveFilterList(string FilePath, int FilterIndex, bool DoEncrypt)
            {
                try
                {
                    SaveFilterList_ToXDocument(FilePath, FilterIndex);

                    if (DoEncrypt)
                    {
                        string sPassword = Socket_Cache.FilterList.AESKey;

                        if (!string.IsNullOrEmpty(sPassword))
                        {
                            Socket_Operation.EncryptXMLFile(FilePath, sPassword);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void SaveFilterList_ToXDocument(string FilePath, int FilterIndex)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeRoot = new XElement("FilterList");
                    xdoc.Add(xeRoot);

                    if (Socket_Cache.FilterList.lstFilter.Count > 0)
                    {
                        int Start = 0;
                        int End = Socket_Cache.FilterList.lstFilter.Count;

                        if (FilterIndex > -1 && FilterIndex < End)
                        {
                            Start = FilterIndex;
                            End = FilterIndex + 1;
                        }

                        for (int i = Start; i < End; i++)
                        {
                            string sIsEnable = Socket_Cache.FilterList.lstFilter[i].IsEnable.ToString();
                            string sFID = Socket_Cache.FilterList.lstFilter[i].FID.ToString().ToUpper();
                            string sFName = Socket_Cache.FilterList.lstFilter[i].FName;
                            string sFAppointHeader = Socket_Cache.FilterList.lstFilter[i].AppointHeader.ToString();
                            string sFHeaderContent = Socket_Cache.FilterList.lstFilter[i].HeaderContent;
                            string sFAppointSocket = Socket_Cache.FilterList.lstFilter[i].AppointSocket.ToString();
                            string sFSocketContent = Socket_Cache.FilterList.lstFilter[i].SocketContent.ToString();
                            string sFAppointLength = Socket_Cache.FilterList.lstFilter[i].AppointLength.ToString();
                            string sFLengthContent = Socket_Cache.FilterList.lstFilter[i].LengthContent.ToString();
                            string sFMode = ((int)Socket_Cache.FilterList.lstFilter[i].FMode).ToString();
                            string sFAction = ((int)Socket_Cache.FilterList.lstFilter[i].FAction).ToString();
                            string sIsExecute = Socket_Cache.FilterList.lstFilter[i].IsExecute.ToString();
                            string sRID = Socket_Cache.FilterList.lstFilter[i].RID.ToString().ToUpper();
                            string sFFunction = Socket_Cache.Filter.GetFilterFunctionString(Socket_Cache.FilterList.lstFilter[i].FFunction);
                            string sFStartFrom = ((int)Socket_Cache.FilterList.lstFilter[i].FStartFrom).ToString();
                            string sFProgressionStep = Socket_Cache.FilterList.lstFilter[i].ProgressionStep.ToString();
                            string sFProgressionPosition = Socket_Cache.FilterList.lstFilter[i].ProgressionPosition;
                            string sFSearch = Socket_Cache.FilterList.lstFilter[i].FSearch;
                            string sFModify = Socket_Cache.FilterList.lstFilter[i].FModify;

                            XElement xeFilter =
                                new XElement("Filter",
                                new XElement("IsEnable", sIsEnable),
                                new XElement("ID", sFID),
                                new XElement("Name", sFName),
                                new XElement("AppointHeader", sFAppointHeader),
                                new XElement("HeaderContent", sFHeaderContent),
                                new XElement("AppointSocket", sFAppointSocket),
                                new XElement("SocketContent", sFSocketContent),
                                new XElement("AppointLength", sFAppointLength),
                                new XElement("LengthContent", sFLengthContent),
                                new XElement("Mode", sFMode),
                                new XElement("Action", sFAction),
                                new XElement("IsExecute", sIsExecute),
                                new XElement("RobotID", sRID),
                                new XElement("Function", sFFunction),
                                new XElement("StartFrom", sFStartFrom),
                                new XElement("ProgressionStep", sFProgressionStep),
                                new XElement("ProgressionPosition", sFProgressionPosition),
                                new XElement("Search", sFSearch),
                                new XElement("Modify", sFModify)
                                );

                            xeRoot.Add(xeFilter);
                        }
                    }

                    xdoc.Save(FilePath);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//加载滤镜列表（对话框）

            public static void LoadFilterList_Dialog()
            {
                try
                {
                    OpenFileDialog ofdLoadFile = new OpenFileDialog();

                    ofdLoadFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_75) + "（*.fp）|*.fp";
                    ofdLoadFile.RestoreDirectory = true;

                    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = ofdLoadFile.FileName;

                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            LoadFilterList(FilePath, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void LoadFilterList(string FilePath, bool LoadFromUser)
            {
                try
                {
                    if (File.Exists(FilePath))
                    {
                        XDocument xdoc = new XDocument();

                        bool bEncrypt = Socket_Operation.IsEncryptXMLFile(FilePath);

                        if (bEncrypt)
                        {
                            if (LoadFromUser)
                            {
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.FilterList_Import);
                                pwForm.ShowDialog();
                            }

                            xdoc = Socket_Operation.DecryptXMLFile(FilePath, Socket_Cache.FilterList.AESKey);
                        }
                        else
                        {
                            xdoc = XDocument.Load(FilePath);
                        }

                        if (xdoc == null)
                        {
                            string sError = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_92);

                            if (LoadFromUser)
                            {
                                Socket_Operation.ShowMessageBox(sError);
                            }
                            else
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                            }
                        }
                        else
                        {
                            LoadFilterList_FromXDocument(xdoc);

                            if (bEncrypt)
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_81));
                            }
                            else
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_80));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void LoadFilterList_FromXDocument(XDocument xdoc)
            {
                try
                {
                    foreach (XElement xeFilter in xdoc.Root.Elements())
                    {
                        bool bIsEnable = false;
                        if (xeFilter.Element("IsEnable") != null)
                        {
                            bIsEnable = bool.Parse(xeFilter.Element("IsEnable").Value);
                        }

                        Guid gFID = Guid.Empty;
                        if (xeFilter.Element("ID") != null)
                        {
                            gFID = Guid.Parse(xeFilter.Element("ID").Value);
                        }
                        else
                        {
                            gFID = Guid.NewGuid();
                        }

                        string sFName = string.Empty;
                        if (xeFilter.Element("Name") != null)
                        {
                            sFName = xeFilter.Element("Name").Value;
                        }

                        bool bAppointHeader = false;
                        if (xeFilter.Element("AppointHeader") != null)
                        {
                            bAppointHeader = bool.Parse(xeFilter.Element("AppointHeader").Value);
                        }

                        string sFHeaderContent = string.Empty;
                        if (xeFilter.Element("HeaderContent") != null)
                        {
                            sFHeaderContent = xeFilter.Element("HeaderContent").Value;
                        }

                        bool bAppointSocket = false;
                        if (xeFilter.Element("AppointSocket") != null)
                        {
                            bAppointSocket = bool.Parse(xeFilter.Element("AppointSocket").Value);
                        }

                        decimal dFSocketContent = 1;
                        if (xeFilter.Element("SocketContent") != null)
                        {
                            dFSocketContent = decimal.Parse(xeFilter.Element("SocketContent").Value);
                        }

                        bool bAppointLength = false;
                        if (xeFilter.Element("AppointLength") != null)
                        {
                            bAppointLength = bool.Parse(xeFilter.Element("AppointLength").Value);
                        }

                        decimal dFLengthContent = 1;
                        if (xeFilter.Element("LengthContent") != null)
                        {
                            dFLengthContent = decimal.Parse(xeFilter.Element("LengthContent").Value);
                        }

                        Socket_Cache.Filter.FilterMode FilterMode = Socket_Cache.Filter.FilterMode.Normal;
                        if (xeFilter.Element("Mode") != null)
                        {
                            FilterMode = Socket_Cache.Filter.GetFilterMode_ByString(xeFilter.Element("Mode").Value);
                        }

                        Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.Filter.FilterAction.NoModify_Display;
                        if (xeFilter.Element("Action") != null)
                        {
                            FilterAction = Socket_Cache.Filter.GetFilterAction_ByString(xeFilter.Element("Action").Value);
                        }

                        bool bIsExecute = false;
                        if (xeFilter.Element("IsExecute") != null)
                        {
                            bIsExecute = bool.Parse(xeFilter.Element("IsExecute").Value);
                        }

                        Guid gRID = Guid.Empty;
                        if (xeFilter.Element("RobotID") != null)
                        {
                            gRID = Guid.Parse(xeFilter.Element("RobotID").Value);
                        }
                        else
                        {
                            gRID = Guid.Empty;
                        }

                        Socket_Cache.Filter.FilterFunction FilterFunction = new Socket_Cache.Filter.FilterFunction();
                        if (xeFilter.Element("Function") != null)
                        {
                            FilterFunction = Socket_Cache.Filter.GetFilterFunction_ByString(xeFilter.Element("Function").Value);
                        }

                        Socket_Cache.Filter.FilterStartFrom FilterStartFrom = Socket_Cache.Filter.FilterStartFrom.Head;
                        if (xeFilter.Element("StartFrom") != null)
                        {
                            FilterStartFrom = Socket_Cache.Filter.GetFilterStartFrom_ByString(xeFilter.Element("StartFrom").Value);
                        }

                        bool IsProgressionDone = false;

                        decimal dFProgressionStep = 1;
                        if (xeFilter.Element("ProgressionStep") != null)
                        {
                            dFProgressionStep = decimal.Parse(xeFilter.Element("ProgressionStep").Value);
                        }

                        string sFProgressionPosition = string.Empty;
                        if (xeFilter.Element("ProgressionPosition") != null)
                        {
                            sFProgressionPosition = xeFilter.Element("ProgressionPosition").Value;
                        }

                        int iProgressionCount = 0;

                        string sFSearch = string.Empty;
                        if (xeFilter.Element("Search") != null)
                        {
                            sFSearch = xeFilter.Element("Search").Value;
                        }

                        string sFModify = string.Empty;
                        if (xeFilter.Element("Modify") != null)
                        {
                            sFModify = xeFilter.Element("Modify").Value;
                        }

                        Socket_Cache.Filter.AddFilter(
                            bIsEnable, 
                            gFID, 
                            sFName, 
                            bAppointHeader, 
                            sFHeaderContent, 
                            bAppointSocket, 
                            dFSocketContent, 
                            bAppointLength, 
                            dFLengthContent, 
                            FilterMode, 
                            FilterAction,
                            bIsExecute,
                            gRID, 
                            FilterFunction, 
                            FilterStartFrom, 
                            IsProgressionDone, 
                            dFProgressionStep, 
                            sFProgressionPosition, 
                            iProgressionCount, 
                            sFSearch, 
                            sFModify);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion

        #region//机器人

        public static class Robot
        {
            #region//结构定义

            public enum KeyBoardType
            {
                Press = 0,
                Down = 1,
                Up = 2,
                Combine = 3,
                Text = 4,
            }

            public enum MouseType
            {
                LeftClick = 0,
                RightClick = 1,
                LeftDBClick = 2,
                RightDBClick = 3,
                LeftDown = 4,
                LeftUp = 5,
                RightDown = 6,
                RightUp = 7,
                WheelUp = 8,
                WheelDown = 9,
                MoveTo = 10,
                MoveBy = 11,
            }

            public enum InstructionType
            {
                SendSendList = 0,                
                Delay = 1,
                LoopStart = 2,
                LoopEnd = 3,
                KeyBoard = 4,
                Mouse = 5,
                SendSocketList = 6,
            }

            #endregion

            #region//初始化指令集

            public static DataTable InitInstructions()
            {
                DataTable dtInstructions = new DataTable();

                try
                {
                    dtInstructions.Columns.Add("Type", typeof(Socket_Cache.Robot.InstructionType));
                    dtInstructions.Columns.Add("Content", typeof(string));
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtInstructions;
            }

            #endregion

            #region//新增机器人

            public static void AddRobot_New()
            {
                try
                {
                    bool IsEnable = false;
                    Guid RID = Guid.NewGuid();
                    int RNum = Socket_Cache.RobotList.lstRobot.Count + 1;
                    string RName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_27), RNum.ToString());

                    AddRobot(IsEnable, RID, RName, Socket_Cache.Robot.InitInstructions());
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void AddRobot(bool IsEnable, Guid RID, string RName, DataTable RInstructions)
            {
                try
                {
                    if (RID != Guid.Empty && !string.IsNullOrEmpty(RName))
                    {
                        Socket_RobotInfo sri = new Socket_RobotInfo(IsEnable, RID, RName, RInstructions);
                        Socket_Cache.RobotList.RobotToList(sri);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }            

            #endregion

            #region//更新机器人

            public static void UpdateRobot_ByRobotIndex(int RIndex, string RName, DataTable RInstruction)
            {
                try
                {
                    if (RIndex > -1)
                    {
                        Socket_Cache.RobotList.lstRobot[RIndex].RName = RName;
                        Socket_Cache.RobotList.lstRobot[RIndex].RInstruction = RInstruction.Copy();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//复制机器人

            public static int CopyRobot_ByRobotIndex(int RIndex)
            {
                int iReturn = -1;

                try
                {
                    bool IsEnable = false;
                    Guid RID_New = Guid.NewGuid();
                    string RName_Copy = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_62), Socket_Cache.RobotList.lstRobot[RIndex].RName);
                    DataTable RInstruction_Copy = Socket_Cache.RobotList.lstRobot[RIndex].RInstruction.Copy();

                    Socket_Cache.Robot.AddRobot(IsEnable, RID_New, RName_Copy, RInstruction_Copy);
                    iReturn = Socket_Cache.RobotList.lstRobot.Count - 1;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
            }

            #endregion

            #region//删除机器人

            public static void DeleteRobot_ByRobotIndex_Dialog(int RIndex)
            {
                try
                {
                    if (RIndex > -1)
                    {
                        DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                        if (dr.Equals(DialogResult.OK))
                        {
                            Socket_Cache.Robot.DeleteRobot_ByRobotIndex(RIndex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void DeleteRobot_ByRobotIndex(int RIndex)
            {
                try
                {
                    if (RIndex > -1)
                    {
                        Socket_Cache.RobotList.lstRobot.RemoveAt(RIndex);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//设置机器人是否启用

            public static void SetIsCheck_ByRobotIndex(int RIndex, bool bCheck)
            {
                try
                {
                    if (RIndex > -1)
                    {
                        Socket_Cache.RobotList.lstRobot[RIndex].IsEnable = bCheck;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//获取指令类型的名称

            public static string GetName_ByInstructionType(Socket_Cache.Robot.InstructionType instructionType)
            {
                string sReturn = string.Empty;

                try
                {
                    switch (instructionType)
                    {
                        case Socket_Cache.Robot.InstructionType.SendSendList:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_94);
                            break;

                        case Socket_Cache.Robot.InstructionType.SendSocketList:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_94);
                            break;

                        case Socket_Cache.Robot.InstructionType.Delay:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_95);
                            break;

                        case Socket_Cache.Robot.InstructionType.LoopStart:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_96);
                            break;

                        case Socket_Cache.Robot.InstructionType.LoopEnd:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_97);
                            break;

                        case Socket_Cache.Robot.InstructionType.KeyBoard:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_105);
                            break;

                        case Socket_Cache.Robot.InstructionType.Mouse:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_107);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//获取指令类型的颜色

            public static Color GetColor_ByInstructionType(Socket_Cache.Robot.InstructionType instructionType)
            {
                Color cReturn = Color.White;

                try
                {
                    switch (instructionType)
                    {
                        case Socket_Cache.Robot.InstructionType.SendSendList:
                            cReturn = Color.YellowGreen;
                            break;

                        case Socket_Cache.Robot.InstructionType.SendSocketList:
                            cReturn = Color.YellowGreen;
                            break;

                        case Socket_Cache.Robot.InstructionType.Delay:
                            cReturn = Color.Khaki;
                            break;

                        case Socket_Cache.Robot.InstructionType.LoopStart:
                            cReturn = Color.Orchid;
                            break;

                        case Socket_Cache.Robot.InstructionType.LoopEnd:
                            cReturn = Color.Orchid;
                            break;

                        case Socket_Cache.Robot.InstructionType.KeyBoard:
                            cReturn = Color.LightSeaGreen;
                            break;

                        case Socket_Cache.Robot.InstructionType.Mouse:
                            cReturn = Color.LightSkyBlue;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return cReturn;
            }

            #endregion

            #region//获取指令内容的字符串

            public static string GetContentString_ByInstructionType(Socket_Cache.Robot.InstructionType instructionType, string sContent)
            {
                string sReturn = string.Empty;

                try
                {
                    switch (instructionType)
                    {
                        case Socket_Cache.Robot.InstructionType.SendSendList:

                            if (!string.IsNullOrEmpty(sContent))
                            {
                                Guid SID = Guid.Parse(sContent);
                                string SName = Socket_Cache.Send.GetSendName_ByGuid(SID);

                                sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_113), SName);                                              
                            }

                            break;

                        case Socket_Cache.Robot.InstructionType.SendSocketList:

                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_161);

                            break;

                        case Socket_Cache.Robot.InstructionType.Delay:

                            if (!string.IsNullOrEmpty(sContent))
                            {
                                sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_115), sContent);
                            }
                            
                            break;

                        case Socket_Cache.Robot.InstructionType.LoopStart:

                            if (!string.IsNullOrEmpty(sContent))
                            {
                                sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_116), sContent);
                            }
                            
                            break;

                        case Socket_Cache.Robot.InstructionType.LoopEnd:
                            
                            break;

                        case Socket_Cache.Robot.InstructionType.KeyBoard:

                            if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                            {
                                Socket_Cache.Robot.KeyBoardType kbType = Socket_Cache.Robot.GetKeyBoardType_ByString(sContent.Split('|')[0].ToString());
                                string KeyCode = sContent.Split('|')[1];

                                switch (kbType)
                                {
                                    case Socket_Cache.Robot.KeyBoardType.Press:
                                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_106), KeyCode);
                                        break;

                                    case Socket_Cache.Robot.KeyBoardType.Down:
                                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_124), KeyCode);
                                        break;

                                    case Socket_Cache.Robot.KeyBoardType.Up:
                                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_125), KeyCode);
                                        break;

                                    case Socket_Cache.Robot.KeyBoardType.Combine:
                                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_130), KeyCode);
                                        break;

                                    case Socket_Cache.Robot.KeyBoardType.Text:
                                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_131), KeyCode);
                                        break;
                                }                                
                            }

                            break;

                        case Socket_Cache.Robot.InstructionType.Mouse:

                            if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                            {
                                Socket_Cache.Robot.MouseType mType = Socket_Cache.Robot.GetMouseType_ByString(sContent.Split('|')[0].ToString());
                                string MouseCode = sContent.Split('|')[1];

                                switch (mType)
                                {
                                    case Socket_Cache.Robot.MouseType.LeftClick:
                                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_117);
                                        break;

                                    case Socket_Cache.Robot.MouseType.RightClick:
                                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_118);
                                        break;

                                    case Socket_Cache.Robot.MouseType.LeftDBClick:
                                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_119);
                                        break;

                                    case Socket_Cache.Robot.MouseType.RightDBClick:
                                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_120);
                                        break;

                                    case Socket_Cache.Robot.MouseType.LeftDown:
                                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_126);
                                        break;

                                    case Socket_Cache.Robot.MouseType.LeftUp:
                                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_127);
                                        break;

                                    case Socket_Cache.Robot.MouseType.RightDown:
                                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_128);
                                        break;

                                    case Socket_Cache.Robot.MouseType.RightUp:
                                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_129);
                                        break;

                                    case Socket_Cache.Robot.MouseType.WheelUp:
                                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_121), MouseCode);
                                        break;

                                    case Socket_Cache.Robot.MouseType.WheelDown:
                                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_122), MouseCode);
                                        break;

                                    case Socket_Cache.Robot.MouseType.MoveTo:
                                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_108), MouseCode);
                                        break;

                                    case Socket_Cache.Robot.MouseType.MoveBy:
                                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_132), MouseCode);
                                        break;
                                }                                
                            }

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//获取指令类型

            public static Socket_Cache.Robot.InstructionType GetInstructionType_ByString(string InstructionType)
            {
                Socket_Cache.Robot.InstructionType instructionType = new InstructionType();

                try
                {
                    instructionType = (Socket_Cache.Robot.InstructionType)Enum.Parse(typeof(Socket_Cache.Robot.InstructionType), InstructionType);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return instructionType;
            }

            #endregion            

            #region//获取键盘按键类型

            public static Socket_Cache.Robot.KeyBoardType GetKeyBoardType_ByString(string KeyBoardType)
            {
                Socket_Cache.Robot.KeyBoardType kbType = new Socket_Cache.Robot.KeyBoardType();

                try
                {
                    kbType = (Socket_Cache.Robot.KeyBoardType)Enum.Parse(typeof(Socket_Cache.Robot.KeyBoardType), KeyBoardType);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return kbType;
            }

            #endregion            

            #region//获取鼠标按键类型

            public static Socket_Cache.Robot.MouseType GetMouseType_ByString(string MouseType)
            {
                Socket_Cache.Robot.MouseType mType = new Socket_Cache.Robot.MouseType();

                try
                {
                    mType = (Socket_Cache.Robot.MouseType)Enum.Parse(typeof(Socket_Cache.Robot.MouseType), MouseType);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return mType;
            }

            #endregion                        

            #region//检查指令集

            public static int CheckRobotInstruction(DataTable dtRInstruction, bool bFromSystem)
            {
                int iReturn = -1;

                try
                {
                    if (dtRInstruction != null && dtRInstruction.Rows.Count > 0)
                    {
                        List<int> listSendSendList = new List<int>();                 
                        List<int> listLoopStart = new List<int>();
                        List<int> listLoopEnd = new List<int>();

                        for (int i = 0; i < dtRInstruction.Rows.Count; i++)
                        {
                            Socket_Cache.Robot.InstructionType instructionType = (Socket_Cache.Robot.InstructionType)dtRInstruction.Rows[i]["Type"];

                            switch (instructionType)
                            {
                                case Socket_Cache.Robot.InstructionType.SendSendList:
                                    listSendSendList.Add(i);
                                    break;                      

                                case Socket_Cache.Robot.InstructionType.LoopStart:
                                    listLoopStart.Add(i);
                                    break;

                                case Socket_Cache.Robot.InstructionType.LoopEnd:
                                    listLoopEnd.Add(i);
                                    break;
                            }                      
                        }

                        #region//检测发送指令

                        foreach (int iSendIndex in listSendSendList)
                        { 
                            string sSendContent = dtRInstruction.Rows[iSendIndex]["Content"].ToString();

                            if (!string.IsNullOrEmpty(sSendContent))
                            {
                                Guid SID = Guid.Parse(sSendContent);
                                string SName = Socket_Cache.Send.GetSendName_ByGuid(SID);

                                if (string.IsNullOrEmpty(SName))
                                {
                                    string sError = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_99), iSendIndex + 1, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_103));

                                    if (!bFromSystem)
                                    {
                                        Socket_Operation.ShowMessageBox(sError);
                                    }

                                    return iSendIndex;
                                }
                            }
                        }                      

                        #endregion

                        #region//检测循环指令

                        if (listLoopStart.Count != listLoopEnd.Count)
                        {
                            int iErrorIndex = 0;
                            if (listLoopStart.Count > 0)
                            {
                                iErrorIndex = listLoopStart[0];
                            }
                            else if (listLoopEnd.Count > 0)
                            {
                                iErrorIndex = listLoopEnd[0];
                            }

                            string sError = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_99), iErrorIndex + 1, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_104));

                            if (!bFromSystem)
                            {
                                Socket_Operation.ShowMessageBox(sError);
                            }                            
                            
                            return iErrorIndex;
                        }

                        for (int i = 0; i < listLoopStart.Count; i++) 
                        {
                            int iLoopStartIndex = listLoopStart[i];
                            int iLoopEndIndex = listLoopEnd[i];

                            if (iLoopStartIndex >= iLoopEndIndex)
                            {
                                string sError = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_99), iLoopEndIndex + 1, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_104));

                                if (!bFromSystem)
                                {
                                    Socket_Operation.ShowMessageBox(sError);
                                }                                

                                return iLoopEndIndex;
                            }
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
            }

            #endregion

            #region//执行机器人

            private static void DoRobot_ByIndex(int RobotListIndex)
            {
                try
                {
                    if (RobotListIndex > -1 && RobotListIndex < Socket_Cache.RobotList.lstRobot.Count)
                    {
                        Guid RID = Socket_Cache.RobotList.lstRobot[RobotListIndex].RID;
                        Socket_Cache.Robot.DoRobot(RID);
                    }                   
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static Socket_Robot DoRobot(Guid RID)
            {
                Socket_Robot srReturn = null;

                try
                {
                    if (RID != Guid.Empty)
                    {
                        Socket_RobotInfo sri = Socket_Cache.RobotList.lstRobot.Where(item => item.RID == RID).FirstOrDefault();

                        if (sri != null) 
                        {
                            if (sri.RInstruction.Rows.Count > 0)
                            {
                                srReturn = new Socket_Robot();
                                srReturn.StartRobot(sri.RName, sri.RInstruction);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return srReturn;
            }

            public static void DoRobot_ByHotKey(int HOTKEY_ID)
            {
                switch (HOTKEY_ID)
                {
                    case 9001:
                        Socket_Cache.Robot.DoRobot_ByIndex(0);
                        break;

                    case 9002:
                        Socket_Cache.Robot.DoRobot_ByIndex(1);
                        break;

                    case 9003:
                        Socket_Cache.Robot.DoRobot_ByIndex(2);
                        break;

                    case 9004:
                        Socket_Cache.Robot.DoRobot_ByIndex(3);
                        break;

                    case 9005:
                        Socket_Cache.Robot.DoRobot_ByIndex(4);
                        break;

                    case 9006:
                        Socket_Cache.Robot.DoRobot_ByIndex(5);
                        break;

                    case 9007:
                        Socket_Cache.Robot.DoRobot_ByIndex(6);
                        break;

                    case 9008:
                        Socket_Cache.Robot.DoRobot_ByIndex(7);
                        break;

                    case 9009:
                        Socket_Cache.Robot.DoRobot_ByIndex(8);
                        break;

                    case 9010:
                        Socket_Cache.Robot.DoRobot_ByIndex(9);
                        break;

                    case 9011:
                        Socket_Cache.Robot.DoRobot_ByIndex(10);
                        break;

                    case 9012:
                        Socket_Cache.Robot.DoRobot_ByIndex(11);
                        break;
                }
            }

            #endregion                        
        }

        #endregion

        #region//机器人列表

        public static class RobotList
        {
            public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\RobotList.rp";
            public static string AESKey = string.Empty;

            public static BindingList<Socket_RobotInfo> lstRobot = new BindingList<Socket_RobotInfo>();
            public delegate void SocketRobotReceived(Socket_RobotInfo sri);
            public static event SocketRobotReceived RecSocketRobot;

            #region//机器人入列表

            public static void RobotToList(Socket_RobotInfo sri)
            {
                try
                {
                    RecSocketRobot?.Invoke(sri);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//清空机器人列表（对话框）

            public static void CleanUpRobotList_Dialog()
            {
                try
                {
                    DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_38));

                    if (dr.Equals(DialogResult.OK))
                    {
                        Socket_Cache.RobotList.RobotListClear();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void RobotListClear()
            {
                try
                {
                    lstRobot.Clear();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion                        

            #region//机器人列表的列表操作

            public static int UpdateRobotList_ByListAction(Socket_Cache.ListAction listAction, int iRIndex)
            {
                int iReturn = -1;

                try
                {
                    int iRobotListCount = Socket_Cache.RobotList.lstRobot.Count;
                    Socket_RobotInfo sri = Socket_Cache.RobotList.lstRobot[iRIndex];

                    switch (listAction)
                    {
                        case Socket_Cache.ListAction.Top:
                            if (iRIndex > 0)
                            {
                                Socket_Cache.RobotList.lstRobot.RemoveAt(iRIndex);
                                Socket_Cache.RobotList.lstRobot.Insert(0, sri);
                                iReturn = 0;
                            }
                            break;

                        case Socket_Cache.ListAction.Up:
                            if (iRIndex > 0)
                            {
                                Socket_Cache.RobotList.lstRobot.RemoveAt(iRIndex);
                                Socket_Cache.RobotList.lstRobot.Insert(iRIndex - 1, sri);
                                iReturn = iRIndex - 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Down:
                            if (iRIndex < iRobotListCount - 1)
                            {
                                Socket_Cache.RobotList.lstRobot.RemoveAt(iRIndex);
                                Socket_Cache.RobotList.lstRobot.Insert(iRIndex + 1, sri);
                                iReturn = iRIndex + 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Bottom:
                            if (iRIndex < iRobotListCount - 1)
                            {
                                Socket_Cache.RobotList.lstRobot.RemoveAt(iRIndex);
                                Socket_Cache.RobotList.lstRobot.Add(sri);
                                iReturn = Socket_Cache.RobotList.lstRobot.Count - 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Copy:
                            Socket_Cache.Robot.CopyRobot_ByRobotIndex(iRIndex);
                            iReturn = Socket_Cache.RobotList.lstRobot.Count - 1;
                            break;

                        case Socket_Cache.ListAction.Export:
                            string sRName = Socket_Cache.RobotList.lstRobot[iRIndex].RName;
                            Socket_Cache.RobotList.SaveRobotList_Dialog(sRName, iRIndex);
                            iReturn = iRIndex;
                            break;

                        case Socket_Cache.ListAction.Delete:
                            Socket_Cache.Robot.DeleteRobot_ByRobotIndex_Dialog(iRIndex);                            
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
            }

            #endregion                        

            #region//加载机器人列表（对话框）

            public static void LoadRobotList_Dialog()
            {
                try
                {
                    OpenFileDialog ofdLoadFile = new OpenFileDialog();

                    ofdLoadFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_74) + "（*.rp）|*.rp";
                    ofdLoadFile.RestoreDirectory = true;

                    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = ofdLoadFile.FileName;

                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            LoadRobotList(FilePath, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void LoadRobotList(string FilePath, bool LoadFromUser)
            {
                try
                {
                    if (File.Exists(FilePath))
                    {
                        XDocument xdoc = new XDocument();

                        bool bEncrypt = Socket_Operation.IsEncryptXMLFile(FilePath);

                        if (bEncrypt)
                        {
                            if (LoadFromUser)
                            {
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.RobotList_Import);
                                pwForm.ShowDialog();
                            }

                            xdoc = Socket_Operation.DecryptXMLFile(FilePath, Socket_Cache.RobotList.AESKey);
                        }
                        else
                        {
                            xdoc = XDocument.Load(FilePath);
                        }

                        if (xdoc == null)
                        {
                            string sError = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_92);

                            if (LoadFromUser)
                            {
                                Socket_Operation.ShowMessageBox(sError);
                            }
                            else
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                            }
                        }
                        else
                        {
                            LoadRobotList_FromXDocument(xdoc);

                            if (bEncrypt)
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72));
                            }
                            else
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void LoadRobotList_FromXDocument(XDocument xdoc)
            {
                try
                {
                    foreach (XElement xeRobot in xdoc.Root.Elements())
                    {
                        bool IsEnable = false;
                        if (xeRobot.Element("IsEnable") != null)
                        {
                            IsEnable = bool.Parse(xeRobot.Element("IsEnable").Value);
                        }

                        Guid RID = Guid.Empty;
                        if (xeRobot.Element("ID") != null)
                        {
                            RID = Guid.Parse(xeRobot.Element("ID").Value);
                        }
                        else
                        {
                            RID = Guid.NewGuid();
                        }

                        string RName = string.Empty;
                        if (xeRobot.Element("Name") != null)
                        {
                            RName = xeRobot.Element("Name").Value;
                        }

                        DataTable RInstruction = Socket_Cache.Robot.InitInstructions();
                        if (xeRobot.Element("Instructions") != null)
                        {
                            foreach (XElement xeInstruction in xeRobot.Element("Instructions").Elements())
                            {
                                string sType = xeInstruction.Attribute("Type").Value;
                                string sContent = xeInstruction.Value;

                                DataRow dr = RInstruction.NewRow();
                                dr[0] = Socket_Cache.Robot.GetInstructionType_ByString(sType);
                                dr[1] = sContent;

                                RInstruction.Rows.Add(dr);
                            }
                        }

                        Socket_Cache.Robot.AddRobot(IsEnable, RID, RName, RInstruction);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//保存机器人列表（对话框）

            public static void SaveRobotList_Dialog(string FileName, int RobotIndex)
            {
                try
                {
                    if (Socket_Cache.RobotList.lstRobot.Count > 0)
                    {
                        SaveFileDialog sfdSaveFile = new SaveFileDialog();

                        sfdSaveFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_74) + "（*.rp）|*.rp";

                        if (!string.IsNullOrEmpty(FileName))
                        {
                            sfdSaveFile.FileName = FileName;
                        }

                        sfdSaveFile.RestoreDirectory = true;

                        if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                        {
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.RobotList_Export);
                            pwForm.ShowDialog();

                            string FilePath = sfdSaveFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                SaveRobotList(FilePath, RobotIndex, true);

                                string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_154), FilePath);
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void SaveRobotList(string FilePath, int RobotIndex, bool DoEncrypt)
            {
                try
                {
                    SaveRobotList_ToXDocument(FilePath, RobotIndex);

                    if (DoEncrypt)
                    {
                        string sPassword = Socket_Cache.RobotList.AESKey;

                        if (!string.IsNullOrEmpty(sPassword))
                        {                            
                            Socket_Operation.EncryptXMLFile(FilePath, sPassword);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void SaveRobotList_ToXDocument(string FilePath, int RobotIndex)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeRoot = new XElement("RobotList");
                    xdoc.Add(xeRoot);

                    if (Socket_Cache.RobotList.lstRobot.Count > 0)
                    {
                        int Start = 0;
                        int End = Socket_Cache.RobotList.lstRobot.Count;

                        if (RobotIndex > -1 && RobotIndex < End)
                        {
                            Start = RobotIndex;
                            End = RobotIndex + 1;
                        }

                        for (int i = Start; i < End; i++)
                        {
                            string IsEnable = Socket_Cache.RobotList.lstRobot[i].IsEnable.ToString();
                            string sRID = Socket_Cache.RobotList.lstRobot[i].RID.ToString().ToUpper();
                            string sRName = Socket_Cache.RobotList.lstRobot[i].RName;
                            DataTable dtRInstruction = Socket_Cache.RobotList.lstRobot[i].RInstruction;

                            XElement xeRobot =
                                new XElement("Robot",
                                new XElement("IsEnable", IsEnable),
                                new XElement("ID", sRID),
                                new XElement("Name", sRName)
                                );

                            if (dtRInstruction.Rows.Count > 0)
                            {
                                XElement xeInstruction = new XElement("Instructions");

                                foreach (DataRow row in dtRInstruction.Rows)
                                {
                                    XElement xeInst = new XElement("Inst", new XAttribute("Type", row[0].ToString()), row[1].ToString());
                                    xeInstruction.Add(xeInst);
                                }

                                xeRobot.Add(xeInstruction);
                            }

                            xeRoot.Add(xeRobot);
                        }
                    }

                    xdoc.Save(FilePath);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion

        #region//发送

        public static class Send
        {
            public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\SendCollection.sc";
            public static string AESKey = string.Empty;

            #region//初始化发送集

            public static DataTable InitSendCollection()
            {
                DataTable dtSendCollection = new DataTable();

                try
                {
                    dtSendCollection.Columns.Add("Socket", typeof(int));
                    dtSendCollection.Columns.Add("Type", typeof(Socket_Cache.SocketPacket.PacketType));
                    dtSendCollection.Columns.Add("IPTo", typeof(string));
                    dtSendCollection.Columns.Add("Length", typeof(int));
                    dtSendCollection.Columns.Add("Buffer", typeof(byte[]));
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtSendCollection;
            }

            #endregion

            #region//获取发送集

            public static DataTable GetSendCollection_ByGuid(Guid SID)
            {
                DataTable dtReturn = null;

                try
                {
                    if (SID != null && SID != Guid.Empty)
                    {
                        foreach (Socket_SendInfo ssi in Socket_Cache.SendList.lstSend)
                        {
                            if (ssi.SID == SID)
                            {
                                return ssi.SCollection;                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            #endregion

            #region//新增发送集

            public static void AddSendCollection_ByIndex(Guid SID, int Index)
            {
                try
                {
                    if (SID != null && SID != Guid.Empty)
                    {
                        if (Index > -1 && Index < Socket_Cache.SocketList.lstRecPacket.Count)
                        {
                            foreach (Socket_SendInfo ssi in Socket_Cache.SendList.lstSend)
                            {
                                if (ssi.SID == SID)
                                {
                                    DataTable SCollection = ssi.SCollection;                                 
                                    int Socket = Socket_Cache.SocketList.lstRecPacket[Index].PacketSocket;
                                    Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketList.lstRecPacket[Index].PacketType;
                                    string IPTo = Socket_Cache.SocketList.lstRecPacket[Index].PacketTo;
                                    byte[] Buffer = Socket_Cache.SocketList.lstRecPacket[Index].PacketBuffer;

                                    Socket_Cache.Send.AddSendCollection(SCollection, Socket, ptType, IPTo, Buffer);
                                }
                            }
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void AddSendCollection(DataTable SCollection, int Socket, Socket_Cache.SocketPacket.PacketType ptType, string IPTo, byte[] Buffer)
            {
                try
                {
                    DataRow dr = SCollection.NewRow();
                    dr["Socket"] = Socket;
                    dr["Type"] = ptType;
                    dr["IPTo"] = IPTo;
                    dr["Length"] = Buffer.Length;
                    dr["Buffer"] = Buffer;
                    SCollection.Rows.Add(dr);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//发送集的列表操作

            public static int UpdateSendCollection_ByListAction(DataTable dtSendCollection, Socket_Cache.ListAction listAction, int iSIndex)
            {
                int iReturn = -1;

                try
                {
                    int iSendCollectionCount = dtSendCollection.Rows.Count;

                    DataRow dr = dtSendCollection.NewRow();
                    if (iSIndex > -1 && iSIndex < dtSendCollection.Rows.Count)
                    {
                        dr.ItemArray = dtSendCollection.Rows[iSIndex].ItemArray;
                    }

                    switch (listAction)
                    {
                        case Socket_Cache.ListAction.Top:
                            if (iSIndex > 0 && iSIndex < iSendCollectionCount)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                                dtSendCollection.Rows.InsertAt(dr, 0);
                                iReturn = 0;
                            }
                            break;

                        case Socket_Cache.ListAction.Up:
                            if (iSIndex > 0 && iSIndex < iSendCollectionCount)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                                dtSendCollection.Rows.InsertAt(dr, iSIndex - 1);
                                iReturn = iSIndex - 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Down:
                            if (iSIndex > -1 && iSIndex < iSendCollectionCount - 1)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                                dtSendCollection.Rows.InsertAt(dr, iSIndex + 1);
                                iReturn = iSIndex + 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Bottom:
                            if (iSIndex > -1 && iSIndex < iSendCollectionCount - 1)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                                dtSendCollection.Rows.Add(dr);
                                iReturn = dtSendCollection.Rows.Count - 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Delete:
                            if (iSIndex > -1 && iSIndex < iSendCollectionCount)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                            }                            
                            break;

                        case Socket_Cache.ListAction.Export:
                            if (iSendCollectionCount > 0)
                            {
                                Socket_Cache.Send.SaveSendCollection_Dialog(string.Empty, dtSendCollection);
                            }                            
                            break;

                        case Socket_Cache.ListAction.Import:
                            Socket_Cache.Send.LoadSendCollection_Dialog(dtSendCollection);
                            break;

                        case Socket_Cache.ListAction.CleanUp:
                            if (iSendCollectionCount > 0)
                            {
                                DialogResult dia = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_38));
                                if (dia.Equals(DialogResult.OK))
                                {
                                    dtSendCollection.Rows.Clear();
                                }                                
                            }                            
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
            }

            #endregion

            #region//获取发送名称

            public static string GetSendName_ByGuid(Guid SID)
            {
                string sReturn = string.Empty;

                try
                {
                    if (SID != null && SID != Guid.Empty)
                    {
                        foreach (Socket_SendInfo ssi in Socket_Cache.SendList.lstSend)
                        {
                            if (ssi.SID == SID)
                            {
                                return ssi.SName;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//新增发送

            public static void AddSend_New()
            {
                try
                {
                    bool IsEnable = false;
                    Guid SID = Guid.NewGuid();
                    int SNum = Socket_Cache.SendList.lstSend.Count + 1;
                    string SName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_162), SNum.ToString());
                    bool SSystemSocket = false;              
                    int SLoopCNT = 1;
                    int SLoopINT = 1000;
                    string SNotes = string.Empty;

                    Socket_Cache.Send.AddSend(IsEnable, SID, SName, SSystemSocket, SLoopCNT, SLoopINT, Socket_Cache.Send.InitSendCollection(), SNotes);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void AddSend(bool IsEnable, Guid SID, string SName, bool SSystemSocket, int SLoopCNT, int SLoopINT, DataTable SCollection, string SNotes)
            {
                try
                {
                    if (SID != Guid.Empty && !string.IsNullOrEmpty(SName))
                    {
                        Socket_SendInfo ssi = new Socket_SendInfo(IsEnable, SID, SName, SSystemSocket, SLoopCNT, SLoopINT, SCollection, SNotes);
                        Socket_Cache.SendList.SendToList(ssi);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//更新发送

            public static void UpdateSend_BySendIndex(int SIndex, string SName, bool SSystemSocket, int SLoopCNT, int SLoopINT, DataTable SCollection, string SNotes)
            {
                try
                {
                    if (SIndex > -1)
                    {
                        Socket_Cache.SendList.lstSend[SIndex].SName = SName;
                        Socket_Cache.SendList.lstSend[SIndex].SSystemSocket = SSystemSocket;                     
                        Socket_Cache.SendList.lstSend[SIndex].SLoopCNT = SLoopCNT;
                        Socket_Cache.SendList.lstSend[SIndex].SLoopINT = SLoopINT;
                        Socket_Cache.SendList.lstSend[SIndex].SCollection = SCollection.Copy();
                        Socket_Cache.SendList.lstSend[SIndex].SNotes = SNotes;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//复制发送

            public static int CopySend_BySendIndex(int SIndex)
            {
                int iReturn = -1;

                try
                {
                    bool IsEnable_Copy = false;
                    Guid SID_New = Guid.NewGuid();
                    string SName_Copy = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_62), Socket_Cache.SendList.lstSend[SIndex].SName);
                    bool SSystemSocket_Copy = Socket_Cache.SendList.lstSend[SIndex].SSystemSocket;                
                    int SLoopCNT_Copy = Socket_Cache.SendList.lstSend[SIndex].SLoopCNT;
                    int SLoopINT_Copy = Socket_Cache.SendList.lstSend[SIndex].SLoopINT;
                    DataTable SCollection_Copy = Socket_Cache.SendList.lstSend[SIndex].SCollection.Copy();
                    string SNotes_Copy = Socket_Cache.SendList.lstSend[SIndex].SNotes;

                    Socket_Cache.Send.AddSend(IsEnable_Copy, SID_New, SName_Copy, SSystemSocket_Copy, SLoopCNT_Copy, SLoopINT_Copy, SCollection_Copy, SNotes_Copy);
                    iReturn = Socket_Cache.SendList.lstSend.Count - 1;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
            }

            #endregion

            #region//删除发送

            public static void DeleteSend_BySendIndex_Dialog(int SIndex)
            {
                try
                {
                    if (SIndex > -1)
                    {
                        DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                        if (dr.Equals(DialogResult.OK))
                        {
                            Socket_Cache.Send.DeleteSend_BySendIndex(SIndex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void DeleteSend_BySendIndex(int SIndex)
            {
                try
                {
                    if (SIndex > -1)
                    {
                        Socket_Cache.SendList.lstSend.RemoveAt(SIndex);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//执行发送

            public static void DoSend_ByIndex(int SendListIndex)
            {
                try
                {
                    if (SendListIndex > -1 && SendListIndex < Socket_Cache.SendList.lstSend.Count)
                    {
                        Guid SID = Socket_Cache.SendList.lstSend[SendListIndex].SID;
                        Socket_Cache.Send.DoSend(SID);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static Socket_Send DoSend(Guid SID)
            {
                Socket_Send ssReturn = null;

                try
                {
                    if (SID != null && SID != Guid.Empty)
                    {
                        Socket_SendInfo ssi = Socket_Cache.SendList.lstSend.Where(item => item.SID == SID).FirstOrDefault();

                        if (ssi != null)
                        {
                            if (ssi.SCollection.Rows.Count > 0)
                            {
                                ssReturn = new Socket_Send();
                                ssReturn.StartSend(ssi.SName, ssi.SSystemSocket, ssi.SLoopCNT, ssi.SLoopINT, ssi.SCollection);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return ssReturn;
            }

            public static void DoSend_ByHotKey(int HOTKEY_ID)
            {
                switch (HOTKEY_ID)
                {
                    case 9001:
                        Socket_Cache.Send.DoSend_ByIndex(0);
                        break;

                    case 9002:
                        Socket_Cache.Send.DoSend_ByIndex(1);
                        break;

                    case 9003:
                        Socket_Cache.Send.DoSend_ByIndex(2);
                        break;

                    case 9004:
                        Socket_Cache.Send.DoSend_ByIndex(3);
                        break;

                    case 9005:
                        Socket_Cache.Send.DoSend_ByIndex(4);
                        break;

                    case 9006:
                        Socket_Cache.Send.DoSend_ByIndex(5);
                        break;

                    case 9007:
                        Socket_Cache.Send.DoSend_ByIndex(6);
                        break;

                    case 9008:
                        Socket_Cache.Send.DoSend_ByIndex(7);
                        break;

                    case 9009:
                        Socket_Cache.Send.DoSend_ByIndex(8);
                        break;

                    case 9010:
                        Socket_Cache.Send.DoSend_ByIndex(9);
                        break;

                    case 9011:
                        Socket_Cache.Send.DoSend_ByIndex(10);
                        break;

                    case 9012:
                        Socket_Cache.Send.DoSend_ByIndex(11);
                        break;
                }
            }

            #endregion                        

            #region//设置发送是否启用

            public static void SetIsCheck_BySendIndex(int SIndex, bool bCheck)
            {
                try
                {
                    if (SIndex > -1)
                    {
                        Socket_Cache.SendList.lstSend[SIndex].IsEnable = bCheck;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//保存发送集（对话框）

            public static void SaveSendCollection_Dialog(string FileName, DataTable SendCollection)
            {
                try
                {
                    if (SendCollection.Rows.Count > 0)
                    {
                        SaveFileDialog sfdSaveFile = new SaveFileDialog();
                        sfdSaveFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_114) + "（*.sc）|*.sc";

                        if (!string.IsNullOrEmpty(FileName))
                        {
                            sfdSaveFile.FileName = FileName;
                        }

                        sfdSaveFile.RestoreDirectory = true;

                        if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                        {
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.SendList_Export);
                            pwForm.ShowDialog();

                            string FilePath = sfdSaveFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                SaveSendCollection(FilePath, SendCollection, true);

                                string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_166), FilePath);
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void SaveSendCollection(string FilePath, DataTable SendCollection, bool DoEncrypt)
            {
                try
                {
                    SaveSendCollection_ToXDocument(FilePath, SendCollection);

                    if (DoEncrypt)
                    {
                        string sPassword = Socket_Cache.Send.AESKey;

                        if (!string.IsNullOrEmpty(sPassword))
                        {
                            Socket_Operation.EncryptXMLFile(FilePath, sPassword);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void SaveSendCollection_ToXDocument(string FilePath, DataTable SendCollection)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeRoot = new XElement("SendCollection");
                    xdoc.Add(xeRoot);

                    if (SendCollection.Rows.Count > 0)
                    {
                        foreach (DataRow row in SendCollection.Rows)
                        {
                            string sSocket = row["Socket"].ToString();
                            string sType = row["Type"].ToString();
                            string sIPTo = row["IPTo"].ToString();
                            string sBuffer = Socket_Operation.BytesToString(SocketPacket.EncodingFormat.Hex, (byte[])row["Buffer"]);

                            XElement xeColl =
                                new XElement("Collection",
                                new XElement("Socket", sSocket),
                                new XElement("Type", sType),
                                new XElement("IPTo", sIPTo),
                                new XElement("Buffer", sBuffer)
                                );

                            xeRoot.Add(xeColl);
                        }                     
                    }                

                    xdoc.Save(FilePath);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//加载发送集（对话框）

            public static void LoadSendCollection_Dialog(DataTable SendCollection)
            {
                try
                {
                    OpenFileDialog ofdLoadFile = new OpenFileDialog();
                    ofdLoadFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_114) + "（*.sc）|*.sc";
                    ofdLoadFile.RestoreDirectory = true;

                    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = ofdLoadFile.FileName;

                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            LoadSendCollection(FilePath, SendCollection, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void LoadSendCollection(string FilePath, DataTable SendCollection, bool LoadFromUser)
            {
                try
                {
                    if (File.Exists(FilePath))
                    {
                        XDocument xdoc = new XDocument();
                        bool bEncrypt = Socket_Operation.IsEncryptXMLFile(FilePath);

                        if (bEncrypt)
                        {
                            if (LoadFromUser)
                            {
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.SendCollection_Import);
                                pwForm.ShowDialog();
                            }

                            xdoc = Socket_Operation.DecryptXMLFile(FilePath, Socket_Cache.Send.AESKey);
                        }
                        else
                        {
                            xdoc = XDocument.Load(FilePath);
                        }

                        if (xdoc == null)
                        {
                            string sError = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_92);

                            if (LoadFromUser)
                            {
                                Socket_Operation.ShowMessageBox(sError);
                            }
                            else
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                            }
                        }
                        else
                        {
                            LoadSendCollection_FromXDocument(xdoc, SendCollection);

                            if (bEncrypt)
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_168));
                            }
                            else
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_167));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void LoadSendCollection_FromXDocument(XDocument xdoc, DataTable SendCollection)
            {
                try
                {
                    XElement xeRoot = xdoc.Root;

                    switch (xeRoot.Name.LocalName)
                    {
                        case "SendList":

                            #region//SendList File

                            foreach (XElement xeSend in xeRoot.Elements())
                            {
                                int iSocket = 0;
                                if (xeSend.Element("Socket") != null)
                                {
                                    iSocket = int.Parse(xeSend.Element("Socket").Value);
                                }

                                Socket_Cache.SocketPacket.PacketType ptType = new Socket_Cache.SocketPacket.PacketType();
                                if (xeSend.Element("Type") != null)
                                {
                                    ptType = Socket_Cache.SocketPacket.GetPacketType_ByString(xeSend.Element("Type").Value);
                                }

                                string sIPTo = string.Empty;
                                if (xeSend.Element("ToAddress") != null)
                                {
                                    sIPTo = xeSend.Element("ToAddress").Value;
                                }

                                byte[] bBuffer = null;
                                if (xeSend.Element("Data") != null)
                                {
                                    bBuffer = Socket_Operation.StringToBytes(SocketPacket.EncodingFormat.Hex, xeSend.Element("Data").Value);
                                }

                                Socket_Cache.Send.AddSendCollection(SendCollection, iSocket, ptType, sIPTo, bBuffer);
                            }

                            #endregion

                            break;

                        case "SendCollection":

                            #region//SendCollection File

                            foreach (XElement xeCollection in xeRoot.Elements())
                            {
                                int iSocket = 0;
                                if (xeCollection.Element("Socket") != null)
                                {
                                    iSocket = int.Parse(xeCollection.Element("Socket").Value);
                                }

                                Socket_Cache.SocketPacket.PacketType ptType = new Socket_Cache.SocketPacket.PacketType();
                                if (xeCollection.Element("Type") != null)
                                {
                                    ptType = Socket_Cache.SocketPacket.GetPacketType_ByString(xeCollection.Element("Type").Value);
                                }

                                string sIPTo = string.Empty;
                                if (xeCollection.Element("IPTo") != null)
                                {
                                    sIPTo = xeCollection.Element("IPTo").Value;
                                }

                                byte[] bBuffer = null;
                                if (xeCollection.Element("Buffer") != null)
                                {
                                    bBuffer = Socket_Operation.StringToBytes(SocketPacket.EncodingFormat.Hex, xeCollection.Element("Buffer").Value);
                                }

                                Socket_Cache.Send.AddSendCollection(SendCollection, iSocket, ptType, sIPTo, bBuffer);
                            }

                            #endregion

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion

        #region//发送列表

        public static class SendList
        {  
            public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\SendList.sp";
            public static string AESKey = string.Empty;

            public static BindingList<Socket_SendInfo> lstSend = new BindingList<Socket_SendInfo>();
            public delegate void SocketSendReceived(Socket_SendInfo ssi);
            public static event SocketSendReceived RecSocketSend;

            #region//发送列表索引项

            public class SendListItem
            {                
                public string SName { get; set; }

                public Guid SID { get; set; }

                public override string ToString()
                {
                    return SName;
                }
            }

            #endregion

            #region//发送入列表

            public static void SendToList(Socket_SendInfo ssi)
            {
                try
                {
                    RecSocketSend?.Invoke(ssi);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//发送列表的列表操作

            public static int UpdateSendList_ByListAction(Socket_Cache.ListAction listAction, int iSIndex)
            {
                int iReturn = -1;

                try
                {
                    int iSendListCount = Socket_Cache.SendList.lstSend.Count;
                    Socket_SendInfo ssi = Socket_Cache.SendList.lstSend[iSIndex];

                    switch (listAction)
                    {
                        case Socket_Cache.ListAction.Top:
                            if (iSIndex > 0)
                            {
                                Socket_Cache.SendList.lstSend.RemoveAt(iSIndex);
                                Socket_Cache.SendList.lstSend.Insert(0, ssi);
                                iReturn = 0;
                            }
                            break;

                        case Socket_Cache.ListAction.Up:
                            if (iSIndex > 0)
                            {
                                Socket_Cache.SendList.lstSend.RemoveAt(iSIndex);
                                Socket_Cache.SendList.lstSend.Insert(iSIndex - 1, ssi);
                                iReturn = iSIndex - 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Down:
                            if (iSIndex < iSendListCount - 1)
                            {
                                Socket_Cache.SendList.lstSend.RemoveAt(iSIndex);
                                Socket_Cache.SendList.lstSend.Insert(iSIndex + 1, ssi);
                                iReturn = iSIndex + 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Bottom:
                            if (iSIndex < iSendListCount - 1)
                            {
                                Socket_Cache.SendList.lstSend.RemoveAt(iSIndex);
                                Socket_Cache.SendList.lstSend.Add(ssi);
                                iReturn = Socket_Cache.SendList.lstSend.Count - 1;
                            }
                            break;

                        case Socket_Cache.ListAction.Copy:
                            Socket_Cache.Send.CopySend_BySendIndex(iSIndex);
                            iReturn = Socket_Cache.SendList.lstSend.Count - 1;
                            break;

                        case Socket_Cache.ListAction.Export:
                            string sSName = Socket_Cache.SendList.lstSend[iSIndex].SName;
                            Socket_Cache.SendList.SaveSendList_Dialog(sSName, iSIndex);
                            iReturn = iSIndex;
                            break;

                        case Socket_Cache.ListAction.Delete:
                            Socket_Cache.Send.DeleteSend_BySendIndex_Dialog(iSIndex);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
            }

            #endregion                        

            #region//清空发送列表（对话框）

            public static void CleanUpSendList_Dialog()
            {
                try
                {
                    DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_38));

                    if (dr.Equals(DialogResult.OK))
                    {
                        Socket_Cache.SendList.SendListClear();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void SendListClear()
            {
                try
                {
                    lstSend.Clear();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//保存发送列表（对话框）

            public static void SaveSendList_Dialog(string FileName, int SendIndex)
            {
                try
                {
                    if (Socket_Cache.SendList.lstSend.Count > 0)
                    {
                        SaveFileDialog sfdSaveFile = new SaveFileDialog();

                        sfdSaveFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_70) + "（*.sp）|*.sp";

                        if (!string.IsNullOrEmpty(FileName))
                        {
                            sfdSaveFile.FileName = FileName;
                        }

                        sfdSaveFile.RestoreDirectory = true;

                        if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                        {
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.SendList_Export);
                            pwForm.ShowDialog();

                            string FilePath = sfdSaveFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                SaveSendList(FilePath, SendIndex, true);

                                string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_160), FilePath);
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void SaveSendList(string FilePath, int SendPacketIndex, bool DoEncrypt)
            {
                try
                {
                    SaveSendList_ToXDocument(FilePath, SendPacketIndex);

                    if (DoEncrypt)
                    {
                        string sPassword = Socket_Cache.SendList.AESKey;

                        if (!string.IsNullOrEmpty(sPassword))
                        {
                            Socket_Operation.EncryptXMLFile(FilePath, sPassword);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void SaveSendList_ToXDocument(string FilePath, int SendIndex)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeRoot = new XElement("SendList");
                    xdoc.Add(xeRoot);

                    if (Socket_Cache.SendList.lstSend.Count > 0)
                    {
                        int Start = 0;
                        int End = Socket_Cache.SendList.lstSend.Count;

                        if (SendIndex > -1 && SendIndex < End)
                        {
                            Start = SendIndex;
                            End = SendIndex + 1;
                        }

                        for (int i = Start; i < End; i++)
                        {
                            string IsEnable = Socket_Cache.SendList.lstSend[i].IsEnable.ToString();
                            string SID = Socket_Cache.SendList.lstSend[i].SID.ToString().ToUpper();
                            string SName = Socket_Cache.SendList.lstSend[i].SName;
                            string SSystemSocket = Socket_Cache.SendList.lstSend[i].SSystemSocket.ToString();                       
                            string LoopCNT = Socket_Cache.SendList.lstSend[i].SLoopCNT.ToString();
                            string LoopINT = Socket_Cache.SendList.lstSend[i].SLoopINT.ToString();
                            DataTable SCollection = Socket_Cache.SendList.lstSend[i].SCollection;
                            string SNotes = Socket_Cache.SendList.lstSend[i].SNotes;

                            XElement xeSend =
                                new XElement("Send",
                                new XElement("IsEnable", IsEnable),
                                new XElement("ID", SID),
                                new XElement("Name", SName),
                                new XElement("SystemSocket", SSystemSocket),                            
                                new XElement("LoopCNT", LoopCNT),
                                new XElement("LoopINT", LoopINT),
                                new XElement("Notes", SNotes)
                                );

                            if (SCollection.Rows.Count > 0)
                            {
                                XElement xeCollection = new XElement("SendCollection");

                                foreach (DataRow row in SCollection.Rows)
                                {
                                    string sSocket = row["Socket"].ToString();
                                    string sType = row["Type"].ToString();
                                    string sIPTo = row["IPTo"].ToString();
                                    string sBuffer = Socket_Operation.BytesToString(SocketPacket.EncodingFormat.Hex, (byte[])row["Buffer"]);

                                    XElement xeColl =
                                        new XElement("Collection",
                                        new XElement("Socket", sSocket),
                                        new XElement("Type", sType),
                                        new XElement("IPTo", sIPTo),
                                        new XElement("Buffer", sBuffer)
                                        );

                                    xeCollection.Add(xeColl);
                                }

                                xeSend.Add(xeCollection);
                            }

                            xeRoot.Add(xeSend);
                        }
                    }

                    xdoc.Save(FilePath);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//加载发送列表（对话框）

            public static void LoadSendList_Dialog()
            {
                try
                {
                    OpenFileDialog ofdLoadFile = new OpenFileDialog();

                    ofdLoadFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_70) + "（*.sp）|*.sp";
                    ofdLoadFile.RestoreDirectory = true;

                    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = ofdLoadFile.FileName;

                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            LoadSendList(FilePath, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void LoadSendList(string FilePath, bool LoadFromUser)
            {
                try
                {
                    if (File.Exists(FilePath))
                    {
                        XDocument xdoc = new XDocument();

                        bool bEncrypt = Socket_Operation.IsEncryptXMLFile(FilePath);

                        if (bEncrypt)
                        {
                            if (LoadFromUser)
                            {
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.SendList_Import);
                                pwForm.ShowDialog();
                            }

                            xdoc = Socket_Operation.DecryptXMLFile(FilePath, Socket_Cache.SendList.AESKey);
                        }
                        else
                        {
                            xdoc = XDocument.Load(FilePath);
                        }

                        if (xdoc == null)
                        {
                            string sError = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_92);

                            if (LoadFromUser)
                            {
                                Socket_Operation.ShowMessageBox(sError);
                            }
                            else
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                            }
                        }
                        else
                        {
                            LoadSendList_FromXDocument(xdoc);

                            if (bEncrypt)
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_101));
                            }
                            else
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_100));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void LoadSendList_FromXDocument(XDocument xdoc)
            {
                try
                {
                    foreach (XElement xeSend in xdoc.Root.Elements())
                    {
                        bool IsEnable = false;
                        if (xeSend.Element("IsEnable") != null)
                        {
                            IsEnable = bool.Parse(xeSend.Element("IsEnable").Value);
                        }

                        Guid SID = Guid.Empty;
                        if (xeSend.Element("ID") != null)
                        {
                            SID = Guid.Parse(xeSend.Element("ID").Value);
                        }
                        else
                        {
                            SID = Guid.NewGuid();
                        }

                        string SName = string.Empty;
                        if (xeSend.Element("Name") != null)
                        {
                            SName = xeSend.Element("Name").Value;
                        }

                        bool SSystemSocket = false;
                        if (xeSend.Element("SystemSocket") != null)
                        {
                            SSystemSocket = bool.Parse(xeSend.Element("SystemSocket").Value);
                        }

                        int SLoopCNT = 1;
                        if (xeSend.Element("LoopCNT") != null)
                        {
                            SLoopCNT = int.Parse(xeSend.Element("LoopCNT").Value);
                        }

                        int SLoopINT = 1000;
                        if (xeSend.Element("LoopINT") != null)
                        {
                            SLoopINT = int.Parse(xeSend.Element("LoopINT").Value);
                        }

                        string SNotes = string.Empty;
                        if (xeSend.Element("Notes") != null)
                        {
                            SNotes = xeSend.Element("Notes").Value;
                        }

                        DataTable SCollection = Socket_Cache.Send.InitSendCollection();

                        if (xeSend.Element("SendCollection") != null)
                        {
                            foreach (XElement xeCollection in xeSend.Element("SendCollection").Elements())
                            {
                                int iSocket = 0;
                                if (xeCollection.Element("Socket") != null)
                                {
                                    iSocket = int.Parse(xeCollection.Element("Socket").Value);
                                }

                                Socket_Cache.SocketPacket.PacketType ptType = new Socket_Cache.SocketPacket.PacketType();
                                if (xeCollection.Element("Type") != null)
                                {
                                    ptType = Socket_Cache.SocketPacket.GetPacketType_ByString(xeCollection.Element("Type").Value);
                                }

                                string sIPTo = string.Empty;
                                if (xeCollection.Element("IPTo") != null)
                                {
                                    sIPTo = xeCollection.Element("IPTo").Value;
                                }

                                byte[] bBuffer = null;
                                if (xeCollection.Element("Buffer") != null)
                                {
                                    bBuffer = Socket_Operation.StringToBytes(SocketPacket.EncodingFormat.Hex, xeCollection.Element("Buffer").Value);
                                }

                                Socket_Cache.Send.AddSendCollection(SCollection, iSocket, ptType, sIPTo, bBuffer);                                
                            }
                        }

                        Socket_Cache.Send.AddSend(IsEnable, SID, SName, SSystemSocket, SLoopCNT, SLoopINT, SCollection, SNotes);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion
    }
}
