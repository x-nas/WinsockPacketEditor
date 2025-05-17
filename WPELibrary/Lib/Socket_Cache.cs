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
using System.Globalization;
using System.Data.SQLite;

namespace WPELibrary.Lib
{
    public static class Socket_Cache
    {
        #region//系统

        public static class System
        {
            public static string DefaultLanguage = "zh-CN";
            public static string LastInjection = string.Empty;
            public static string WPE64_URL = "https://www.wpe64.com";
            public static string WPE64_IP = "http://101.132.222.195";
            public static string WPE64_DLL = "WPELibrary.dll";
            public static string WPE = "Winsock Packet Editor x64";
            public static Socket_Cache.System.SystemMode StartMode = SystemMode.None;
            public static DateTime StartTime = DateTime.Now;
            public static IntPtr MainHandle = IntPtr.Zero;
            public static int SystemSocket = 0;
            public static bool ShowDebug = false;
            public static bool IsRemote = false;
            public static string Remote_URL, Remote_UserName, Remote_PassWord;
            public static ushort Remote_Port = 89;
            public static IDisposable WebServer;
            public static PerformanceCounter cpuCounter;

            public static Action<Action> InvokeAction { get; set; }

            #region//结构定义

            public enum SystemMode
            {
                None = 0,
                Process = 1,
                Proxy = 2,
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
                ProxyAccount_Import = 8,
                ProxyAccount_Export = 9,
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

            #region//保存系统配置到数据库

            public static void SaveSystemConfig_ToDB()
            {
                try
                {
                    Socket_Cache.DataBase.DeleteTable_SystemConfig();
                    Socket_Cache.DataBase.InsertTable_SystemConfig();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//从数据库加载系统配置

            public static void LoadSystemConfig_FromDB()
            {
                try
                {
                    DataTable SystemConfig = Socket_Cache.DataBase.SelectTable_SystemConfig();

                    if (SystemConfig.Rows.Count > 0)
                    {
                        Socket_Cache.System.DefaultLanguage = SystemConfig.Rows[0]["SystemConfig_DefaultLanguage"].ToString();
                        Socket_Cache.System.LastInjection = SystemConfig.Rows[0]["SystemConfig_LastInjection"].ToString();
                        Socket_Cache.System.StartMode = Socket_Cache.System.GetSystemMode_ByString(SystemConfig.Rows[0]["SystemConfig_StartMode"].ToString());
                        Socket_Cache.System.IsRemote = Convert.ToBoolean(SystemConfig.Rows[0]["SystemConfig_Remote_IsEnable"]);
                        Socket_Cache.System.Remote_UserName = SystemConfig.Rows[0]["SystemConfig_Remote_UserName"].ToString();
                        Socket_Cache.System.Remote_PassWord = SystemConfig.Rows[0]["SystemConfig_Remote_PassWord"].ToString();
                        Socket_Cache.System.Remote_Port = ushort.Parse(SystemConfig.Rows[0]["SystemConfig_Remote_Port"].ToString());
                        Socket_Cache.System.Remote_URL = SystemConfig.Rows[0]["SystemConfig_Remote_URL"].ToString();
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//保存注入进程名称到数据库

            public static void SaveSystemConfig_LastInjection_ToDB()
            {
                try
                {
                    Socket_Cache.DataBase.UpdateTable_SystemConfig_LastInjection();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//保存运行配置到数据库

            public static void SaveRunConfig_ToDB(Socket_Cache.System.SystemMode FromMode)
            {
                try
                {
                    if (Socket_Cache.System.StartMode.Equals(FromMode))
                    {
                        Socket_Cache.DataBase.DeleteTable_RunConfig();
                        Socket_Cache.DataBase.InsertTable_RunConfig();
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//从数据库加载运行配置

            public static void LoadRunConfig_FromDB()
            {
                try
                {
                    DataTable RunConfig = Socket_Cache.DataBase.SelectTable_RunConfig();

                    if (RunConfig.Rows.Count > 0)
                    {
                        Socket_Cache.SocketProxy.ProxyIP_Auto = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_ProxyIP_Auto"]);
                        Socket_Cache.SocketProxy.Enable_SOCKS5 = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_EnableSOCKS5"]);
                        Socket_Cache.SocketProxy.ProxyPort = ushort.Parse(RunConfig.Rows[0]["ProxyConfig_ProxyPort"].ToString());
                        Socket_Cache.SocketProxy.Enable_Auth = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_EnableAuth"]);
                        Socket_Cache.SocketProxy.NoRecord = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_ProxyList_NoRecord"]);
                        Socket_Cache.SocketProxy.DelClosed = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_ClientList_DelClosed"]);
                        Socket_Cache.LogList.Proxy_AutoRoll = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_LogList_AutoRoll"]);
                        Socket_Cache.LogList.Proxy_AutoClear = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_LogList_AutoClear"]);
                        Socket_Cache.LogList.Proxy_AutoClear_Value = Convert.ToInt32(RunConfig.Rows[0]["ProxyConfig_LogList_AutoClear_Value"]);
                        Socket_Cache.SocketProxy.Enable_EXTHttp = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_EXTProxy_EnableHttp"]);
                        Socket_Cache.SocketProxy.EXTHttpIP = RunConfig.Rows[0]["ProxyConfig_EXTProxy_HttpIP"].ToString();
                        Socket_Cache.SocketProxy.EXTHttpPort = ushort.Parse(RunConfig.Rows[0]["ProxyConfig_EXTProxy_HttpPort"].ToString());
                        Socket_Cache.SocketProxy.AppointHttpPort = RunConfig.Rows[0]["ProxyConfig_EXTProxy_AppointHttpPort"].ToString();
                        Socket_Cache.SocketProxy.Enable_EXTHttps = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_EXTProxy_EnableHttps"]);
                        Socket_Cache.SocketProxy.EXTHttpsIP = RunConfig.Rows[0]["ProxyConfig_EXTProxy_HttpsIP"].ToString();
                        Socket_Cache.SocketProxy.EXTHttpsPort = ushort.Parse(RunConfig.Rows[0]["ProxyConfig_EXTProxy_HttpsPort"].ToString());
                        Socket_Cache.SocketProxy.AppointHttpsPort = RunConfig.Rows[0]["ProxyConfig_EXTProxy_AppointHttpsPort"].ToString();
                        Socket_Cache.SocketProxy.SpeedMode = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_SpeedMode"]);

                        Socket_Cache.SocketPacket.CheckNotShow = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_CheckNotShow"]);
                        Socket_Cache.SocketPacket.CheckSocket = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_CheckSocket"]);
                        Socket_Cache.SocketPacket.CheckSocket_Value = RunConfig.Rows[0]["InjectionConfig_CheckSocket_Value"].ToString();
                        Socket_Cache.SocketPacket.CheckIP = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_CheckIP"]);
                        Socket_Cache.SocketPacket.CheckIP_Value = RunConfig.Rows[0]["InjectionConfig_CheckIP_Value"].ToString();
                        Socket_Cache.SocketPacket.CheckPort = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_CheckPort"]);
                        Socket_Cache.SocketPacket.CheckPort_Value = RunConfig.Rows[0]["InjectionConfig_CheckPort_Value"].ToString();
                        Socket_Cache.SocketPacket.CheckHead = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_CheckHead"]);
                        Socket_Cache.SocketPacket.CheckHead_Value = RunConfig.Rows[0]["InjectionConfig_CheckHead_Value"].ToString();
                        Socket_Cache.SocketPacket.CheckData = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_CheckData"]);
                        Socket_Cache.SocketPacket.CheckData_Value = RunConfig.Rows[0]["InjectionConfig_CheckData_Value"].ToString();
                        Socket_Cache.SocketPacket.CheckSize = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_CheckSize"]);
                        Socket_Cache.SocketPacket.CheckLength_Value = RunConfig.Rows[0]["InjectionConfig_CheckLength_Value"].ToString();
                        Socket_Cache.SocketPacket.HookWS1_Send = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWS1_Send"]);
                        Socket_Cache.SocketPacket.HookWS1_SendTo = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWS1_SendTo"]);
                        Socket_Cache.SocketPacket.HookWS1_Recv = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWS1_Recv"]);
                        Socket_Cache.SocketPacket.HookWS1_RecvFrom = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWS1_RecvFrom"]);
                        Socket_Cache.SocketPacket.HookWS2_Send = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWS2_Send"]);
                        Socket_Cache.SocketPacket.HookWS2_SendTo = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWS2_SendTo"]);
                        Socket_Cache.SocketPacket.HookWS2_Recv = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWS2_Recv"]);
                        Socket_Cache.SocketPacket.HookWS2_RecvFrom = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWS2_RecvFrom"]);
                        Socket_Cache.SocketPacket.HookWSA_Send = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWSA_Send"]);
                        Socket_Cache.SocketPacket.HookWSA_SendTo = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWSA_SendTo"]);
                        Socket_Cache.SocketPacket.HookWSA_Recv = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWSA_Recv"]);
                        Socket_Cache.SocketPacket.HookWSA_RecvFrom = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_HookWSA_RecvFrom"]);
                        Socket_Cache.SocketList.AutoRoll = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_SocketList_AutoRoll"]);
                        Socket_Cache.SocketList.AutoClear = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_SocketList_AutoClear"]);
                        Socket_Cache.SocketList.AutoClear_Value = Convert.ToInt32(RunConfig.Rows[0]["InjectionConfig_SocketList_AutoClear_Value"]);
                        Socket_Cache.LogList.Socket_AutoRoll = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_LogList_AutoRoll"]);
                        Socket_Cache.LogList.Socket_AutoClear = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_LogList_AutoClear"]);
                        Socket_Cache.LogList.Socket_AutoClear_Value = Convert.ToInt32(RunConfig.Rows[0]["InjectionConfig_LogList_AutoClear_Value"]);
                        Socket_Cache.SocketPacket.SpeedMode = Convert.ToBoolean(RunConfig.Rows[0]["InjectionConfig_SpeedMode"]);
                        Socket_Cache.Filter.FilterExecute = Socket_Cache.FilterList.GetFilterListExecute_ByString(RunConfig.Rows[0]["InjectionConfig_FilterExecute"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//获取启动模式

            public static Socket_Cache.System.SystemMode GetSystemMode_ByString(string smMode)
            {
                Socket_Cache.System.SystemMode systemMode = Socket_Cache.System.SystemMode.None;

                try
                {
                    systemMode = (Socket_Cache.System.SystemMode)Enum.Parse(typeof(Socket_Cache.System.SystemMode), smMode);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return systemMode;
            }

            #endregion          

            #region//保存系统列表到数据库

            public static void SaveSystemList_ToDB()
            {
                Socket_Cache.FilterList.SaveFilterList_ToDB();
                Socket_Cache.SendList.SaveSendList_ToDB();
                Socket_Cache.RobotList.SaveRobotList_ToDB();
            }

            #endregion

            #region//从数据库加载系统列表

            public static void LoadSystemList_FromDB()
            {
                Task.Run(() =>
                {
                    Socket_Cache.FilterList.LoadFilterList_FromDB();
                    Socket_Cache.SendList.LoadSendList_FromDB();
                    Socket_Cache.RobotList.LoadRobotList_FromDB();
                });
            }

            #endregion
        }

        #endregion

        #region//代理

        public static class SocketProxy
        {  
            public static ulong ProxyTotal_CNT, ProxyTCP_CNT, ProxyUDP_CNT;
            public static int ProxySpeed_Uplink, ProxySpeed_Downlink;
            public static IPAddress ProxyTCP_IP = IPAddress.Any;
            public static IPAddress ProxyUDP_IP = IPAddress.Any;
            public static bool NoRecord = true, DelClosed = true;
            public static bool SpeedMode = false;
            public static bool IsListening = false;            
            public static bool ProxyIP_Auto = true;
            public static bool Enable_SOCKS5 = true, Enable_Auth = true;
            public static bool Enable_EXTHttp = false, Enable_EXTHttps = false;
            public static string EXTHttpIP = "127.0.0.1", EXTHttpsIP = "127.0.0.1";
            public static ushort EXTHttpPort = 8889, EXTHttpsPort = 8889;
            public static string AppointHttpPort = "80,8080", AppointHttpsPort = "443,8443";            
            public static ushort ProxyPort = 1080;
            public static int UDPCloseTime = 60;
            public static long Total_Request = 0;
            public static long Total_Response = 0;
            public static int MaxChartPoint = 100;
            public const long MaxNetworkSpeed = 100000;
            public static string ProxyOnLineInfo = string.Empty;
            public static string ProxyBytesInfo = string.Empty;
            public static string ProxySpeedInfo = string.Empty;            

            public static BindingList<Proxy_AuthInfo> lstProxyAuth = new BindingList<Proxy_AuthInfo>();
            public delegate void ProxyAuthReceived(Proxy_AuthInfo pai);         
            public static event ProxyAuthReceived RecProxyAuth;

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

            public enum ProxySpeedType
            {
                Uplink = 0,
                Downlink = 1,
            }

            public struct IPAddressAndPort
            {
                public IPAddress IPAddress { get; set; }

                public ushort Port { get; set; }
            }

            #endregion

            #region//接收客户端请求

            public static void HandleClient(Socket clientSocket)
            {
                try
                {
                    Socket_ProxyTCP spc = new Socket_ProxyTCP(clientSocket, clientSocket.ReceiveBufferSize);
                    spc.ClientSocket.BeginReceive(spc.ClientBuffer, 0, spc.ClientBuffer.Length, 0, new AsyncCallback(ReadCallback), spc);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void ReadCallback(IAsyncResult ar)
            {
                Socket_ProxyTCP spc = (Socket_ProxyTCP)ar.AsyncState;

                try
                {
                    if (Socket_Cache.SocketProxy.IsListening)
                    {
                        int bytesRead = Socket_Operation.ReceiveTCPData(spc.ClientSocket, ar);

                        if (bytesRead > 0)
                        {
                            ReadOnlySpan<byte> proxyDataSpan = spc.ClientData.AsSpan();
                            ReadOnlySpan<byte> proxyBufferSpan = spc.ClientBuffer.AsSpan(0, bytesRead);
                            Span<byte> combinedData = new Span<byte>(new byte[spc.ClientData.Length + bytesRead]);

                            if (spc.ClientData.Length > 0)
                            {
                                proxyDataSpan.CopyTo(combinedData);
                            }
                            proxyBufferSpan.CopyTo(combinedData.Slice(spc.ClientData.Length));

                            bool bIsMatch = Socket_Operation.CheckDataIsMatchProxyStep(combinedData, spc.ProxyStep);
                            if (bIsMatch)
                            {
                                switch (spc.ProxyStep)
                                {
                                    case Socket_Cache.SocketProxy.ProxyStep.Handshake:
                                        Socket_Cache.SocketProxy.Handshake(spc, combinedData);
                                        break;

                                    case Socket_Cache.SocketProxy.ProxyStep.AuthUserName:
                                        Socket_Cache.SocketProxy.AuthUserName(spc, combinedData);
                                        break;

                                    case Socket_Cache.SocketProxy.ProxyStep.Command:
                                        Socket_Cache.SocketProxy.Command(spc, combinedData);
                                        break;

                                    case Socket_Cache.SocketProxy.ProxyStep.ForwardData:
                                        Socket_Cache.SocketProxy.ForwardData(spc, combinedData);
                                        break;
                                }

                                spc.ClientData = Array.Empty<byte>();
                            }
                            else
                            {
                                spc.ClientData = combinedData.ToArray();
                            }

                            spc.ClientSocket.BeginReceive(spc.ClientBuffer, 0, spc.ClientBuffer.Length, 0, new AsyncCallback(ReadCallback), spc);
                        }
                        else
                        {
                            spc.CloseTCPClient();
                        }
                    }
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode != 10053 && ex.ErrorCode != 10054)
                    {
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spc.ClientSocket.RemoteEndPoint + " - " + ex.Message);
                    }
                    
                    spc.CloseTCPClient();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spc.ClientSocket.RemoteEndPoint + " - " + ex.Message);
                    spc.CloseTCPClient();
                }
            }

            #endregion

            #region//握手过程

            private static void Handshake(Socket_ProxyTCP spc, ReadOnlySpan<byte> bData)
            {
                try
                {
                    spc.ProxyType = (Socket_Cache.SocketProxy.ProxyType)bData[0];

                    if (spc.ProxyType == Socket_Cache.SocketProxy.ProxyType.Socket5)
                    {
                        bool bSupportAuthType = false;

                        Socket_Cache.SocketProxy.AuthType atServer = new Socket_Cache.SocketProxy.AuthType();
                        if (Socket_Cache.SocketProxy.Enable_Auth)
                        {
                            atServer = Socket_Cache.SocketProxy.AuthType.UserName;
                        }
                        else
                        {
                            atServer = Socket_Cache.SocketProxy.AuthType.None;
                        }

                        int iMETHODS_COUNT = bData[1];
                        ReadOnlySpan<byte> bMETHODS = bData.Slice(2, iMETHODS_COUNT);
                        foreach (byte method in bMETHODS)
                        {
                            Socket_Cache.SocketProxy.AuthType atClient = (Socket_Cache.SocketProxy.AuthType)method;

                            if (atServer == atClient)
                            {
                                bSupportAuthType = true;
                                break;
                            }
                        }

                        if (bSupportAuthType)
                        {
                            Span<byte> bAuth = stackalloc byte[2];
                            bAuth[0] = (byte)Socket_Cache.SocketProxy.ProxyType.Socket5;
                            bAuth[1] = (byte)atServer;
                            Socket_Operation.SendTCPData(spc.ClientSocket, bAuth);

                            if (atServer == Socket_Cache.SocketProxy.AuthType.UserName)
                            {
                                spc.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.AuthUserName;

                                if (bData.Length > iMETHODS_COUNT + 2)
                                {
                                    ReadOnlySpan<byte> bAuthDate = bData.Slice(iMETHODS_COUNT + 2);

                                    bool bIsMatch = Socket_Operation.CheckDataIsMatchProxyStep(bAuthDate, Socket_Cache.SocketProxy.ProxyStep.AuthUserName);
                                    if (bIsMatch)
                                    {
                                        Socket_Cache.SocketProxy.AuthUserName(spc, bAuthDate);
                                    }
                                }
                            }
                            else
                            {
                                spc.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Command;
                            }
                        }
                        else
                        {
                            IPEndPoint epClient = (IPEndPoint)spc.ClientSocket.RemoteEndPoint;
                            Socket_Cache.SocketProxy.AuthResult_ToList(epClient.Address.ToString(), string.Empty, false);
                        }
                    }
                    else
                    {
                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_145), spc.ProxyType);
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

            private static void AuthUserName(Socket_ProxyTCP spc, ReadOnlySpan<byte> bData)
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

                        string sUserName = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, USERNAME);
                        string sPassWord = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, PASSWORD);

                        bool bAuthOK = Socket_Cache.ProxyAccount.CheckUserNameAndPassWord(sUserName, sPassWord, out Guid AccountID);

                        IPEndPoint epClient = (IPEndPoint)spc.ClientSocket.RemoteEndPoint;
                        Socket_Cache.SocketProxy.AuthResult_ToList(epClient.Address.ToString(), sUserName, bAuthOK);

                        Span<byte> bAuth = stackalloc byte[2];
                        if (bAuthOK)
                        {
                            bAuth[0] = 0x01;
                            bAuth[1] = 0x00;

                            Socket_Cache.ProxyAccount.SetProxyAccount_Online(AccountID, true);
                            Socket_Cache.SocketProxy.RecordLoginIP(AccountID, epClient.Address.ToString());
                        }
                        else
                        {
                            bAuth[0] = 0x01;
                            bAuth[1] = 0x01;
                        }

                        Socket_Operation.SendTCPData(spc.ClientSocket, bAuth);
                        spc.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Command;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//执行命令

            private static void Command(Socket_ProxyTCP spc, ReadOnlySpan<byte> bData)
            {
                try
                {
                    spc.ProxyType = (Socket_Cache.SocketProxy.ProxyType)bData[0];
                    spc.CommandType = (Socket_Cache.SocketProxy.CommandType)bData[1];
                    spc.AddressType = (Socket_Cache.SocketProxy.AddressType)bData[3];

                    if (spc.ProxyType == Socket_Cache.SocketProxy.ProxyType.Socket5)
                    {
                        try
                        {
                            ReadOnlySpan<byte> bADDRESS = bData.Slice(4, bData.Length - 4);
                            ReadOnlySpan<byte> bServerTCP_IP = Socket_Cache.SocketProxy.ProxyTCP_IP.GetAddressBytes();
                            ReadOnlySpan<byte> bServerTCP_Port = BitConverter.GetBytes(Socket_Cache.SocketProxy.ProxyPort);
                            
                            string sIPString = Socket_Operation.GetIP_ByAddressType(spc.AddressType, bADDRESS);
                            IPEndPoint epServer = Socket_Operation.GetIPEndPoint_ByAddressType(spc.AddressType, bADDRESS);
                            ushort uPort = ((ushort)epServer.Port);                            

                            spc.DomainType = Socket_Operation.GetDomainType_ByPort(uPort);
                            spc.ServerSocket = new Socket(epServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                            spc.ServerAddress = Socket_Operation.GetServerAddress(sIPString, uPort, spc);                            
                            spc.ClientAddress = Socket_Operation.GetClientAddress(sIPString, uPort, spc);
                            spc.ServerEndPoint = epServer;

                            switch (spc.CommandType)
                            {
                                case Socket_Cache.SocketProxy.CommandType.Connect:

                                    #region//代理 TCP

                                    switch (spc.DomainType)
                                    {
                                        case Socket_Cache.SocketProxy.DomainType.Http:

                                            try
                                            {
                                                if (Socket_Cache.SocketProxy.Enable_EXTHttp)
                                                {
                                                    IPEndPoint HttpProxyEP = new IPEndPoint(IPAddress.Parse(Socket_Cache.SocketProxy.EXTHttpIP), Socket_Cache.SocketProxy.EXTHttpPort);
                                                    spc.ServerSocket.Connect(HttpProxyEP);

                                                    if (Socket_Operation.CheckHttpProxyState(spc, sIPString, uPort))
                                                    {
                                                        spc.ServerSocket.BeginReceive(spc.ServerBuffer, 0, spc.ServerBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spc);
                                                        spc.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                        Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));                                                        
                                                    }
                                                    else
                                                    {
                                                        Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                                    }
                                                }
                                                else
                                                {
                                                    spc.ServerSocket.Connect(spc.ServerEndPoint);
                                                    spc.ServerSocket.BeginReceive(spc.ServerBuffer, 0, spc.ServerBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spc);
                                                    spc.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                    Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));

                                                    Socket_Cache.SocketProxyQueue.ProxyTCP_ToQueue(spc);
                                                }                                                
                                            }
                                            catch (SocketException)
                                            {
                                                Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                            }

                                            break;

                                        case Socket_Cache.SocketProxy.DomainType.Https:

                                            try
                                            {
                                                if (Socket_Cache.SocketProxy.Enable_EXTHttps)
                                                {
                                                    IPEndPoint HttpsProxyEP = new IPEndPoint(IPAddress.Parse(Socket_Cache.SocketProxy.EXTHttpsIP), Socket_Cache.SocketProxy.EXTHttpsPort);
                                                    spc.ServerSocket.Connect(HttpsProxyEP);

                                                    if (Socket_Operation.CheckHttpProxyState(spc, sIPString, uPort))
                                                    {
                                                        spc.ServerSocket.BeginReceive(spc.ServerBuffer, 0, spc.ServerBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spc);
                                                        spc.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                        Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));                                                        
                                                    }
                                                    else
                                                    {
                                                        Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                                    }
                                                }
                                                else
                                                {
                                                    spc.ServerSocket.Connect(spc.ServerEndPoint);
                                                    spc.ServerSocket.BeginReceive(spc.ServerBuffer, 0, spc.ServerBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spc);
                                                    spc.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                    Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));
                                                    Socket_Cache.SocketProxyQueue.ProxyTCP_ToQueue(spc);
                                                }
                                            }
                                            catch (SocketException)
                                            {
                                                Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                            }

                                            break;

                                        case Socket_Cache.SocketProxy.DomainType.Socket:

                                            try
                                            {
                                                spc.ServerSocket.Connect(spc.ServerEndPoint);
                                                spc.ServerSocket.BeginReceive(spc.ServerBuffer, 0, spc.ServerBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spc);
                                                spc.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));

                                                Socket_Cache.SocketProxyQueue.ProxyTCP_ToQueue(spc);
                                            }
                                            catch (SocketException)
                                            {
                                                Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                            }

                                            break;
                                    }

                                    #endregion

                                    break;

                                case Socket_Cache.SocketProxy.CommandType.UDP:

                                    #region//UDP 中继                                    

                                    try
                                    {
                                        Socket_ProxyUDP spu = new Socket_ProxyUDP(new IPEndPoint(IPAddress.Any, 0));
                                        spu.ClientUDP.BeginReceive(new AsyncCallback(AcceptUdpCallback), spu);
                                        Socket_Cache.SocketProxyQueue.ProxyUDP_ToQueue(spu);

                                        ReadOnlySpan<byte> bServerUDP_IP = Socket_Cache.SocketProxy.ProxyUDP_IP.GetAddressBytes();
                                        ReadOnlySpan<byte> bServerUDP_Port = BitConverter.GetBytes(((IPEndPoint)spu.ClientUDP.Client.LocalEndPoint).Port);                                        

                                        Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerUDP_IP, bServerUDP_Port));                                        
                                    }
                                    catch (SocketException)
                                    {
                                        Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                    }

                                    #endregion

                                    break;

                                default:

                                    #region//不支持的命令

                                    Socket_Operation.SendTCPData(spc.ClientSocket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Unsupport, bServerTCP_IP, bServerTCP_Port));

                                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_152), spc.ClientSocket.RemoteEndPoint, spc.CommandType);
                                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);

                                    #endregion

                                    break;
                            }
                        }
                        catch (SocketException ex)
                        {
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spc.ServerAddress + " - " + ex.Message);
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

            private static void ForwardData(Socket_ProxyTCP spc, ReadOnlySpan<byte> bData)
            {
                try
                {
                    if (spc.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                    {
                        Socket_Operation.SendTCPData(spc.ServerSocket, bData);

                        bool enableProxyQueue = false;
                        switch (spc.DomainType)
                        {
                            case Socket_Cache.SocketProxy.DomainType.Http:

                                enableProxyQueue = !Socket_Cache.SocketProxy.Enable_EXTHttp;

                                break;

                            case Socket_Cache.SocketProxy.DomainType.Https:

                                enableProxyQueue = !Socket_Cache.SocketProxy.Enable_EXTHttps;

                                break;

                            case Socket_Cache.SocketProxy.DomainType.Socket:

                                enableProxyQueue = true;

                                break;
                        }

                        if (enableProxyQueue)
                        {
                            Socket_Cache.SocketProxyQueue.ProxyData_ToQueue(spc, bData, Socket_Cache.SocketProxy.DataType.Request);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spc.ClientAddress + " - " + ex.Message);
                }
            }

            #endregion

            #region//响应数据（TCP）

            private static void ResponseCallback(IAsyncResult ar)
            {
                Socket_ProxyTCP spc = (Socket_ProxyTCP)ar.AsyncState;

                try
                {
                    int bytesRead = spc.ServerSocket.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        ReadOnlySpan<byte> receivedData = spc.ServerBuffer.AsSpan(0, bytesRead);

                        if (spc.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                        {
                            Socket_Operation.SendTCPData(spc.ClientSocket, receivedData);

                            bool enableProxyQueue = false;
                            switch (spc.DomainType)
                            {
                                case Socket_Cache.SocketProxy.DomainType.Http:
                                    enableProxyQueue = !Socket_Cache.SocketProxy.Enable_EXTHttp;
                                    break;

                                case Socket_Cache.SocketProxy.DomainType.Https:
                                    enableProxyQueue = !Socket_Cache.SocketProxy.Enable_EXTHttps;
                                    break;

                                case Socket_Cache.SocketProxy.DomainType.Socket:
                                    enableProxyQueue = true;
                                    break;
                            }

                            if (enableProxyQueue)
                            {
                                Socket_Cache.SocketProxyQueue.ProxyData_ToQueue(spc, receivedData, Socket_Cache.SocketProxy.DataType.Response);
                            }
                        }

                        spc.ServerSocket.BeginReceive(spc.ServerBuffer, 0, spc.ServerBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spc);
                    }
                    else
                    {
                        spc.CloseTCPServer();
                    }
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode != 10053 && ex.ErrorCode != 10054)
                    {
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spc.ServerAddress + " - " + ex.Message);
                    }                    

                    spc.CloseTCPServer();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spc.ServerAddress + " - " + ex.Message);
                    spc.CloseTCPServer();
                }
            }

            #endregion

            #region//UDP 中继

            private static void AcceptUdpCallback(IAsyncResult ar)
            {
                Socket_ProxyUDP spu = (Socket_ProxyUDP)ar.AsyncState;

                try
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    ReadOnlySpan<byte> bData = Socket_Operation.ReceiveUDPData(spu.ClientUDP, ar, ref remoteEndPoint);

                    if (!bData.IsEmpty && !remoteEndPoint.Address.Equals(IPAddress.Any) && remoteEndPoint.Port != 0)
                    {
                        if (bData[0] == 0 && bData[1] == 0 && bData[2] == 0)
                        {
                            Socket_Cache.SocketProxy.AddressType addressType = (Socket_Cache.SocketProxy.AddressType)bData[3];

                            if (addressType == Socket_Cache.SocketProxy.AddressType.IPV4 ||
                                addressType == Socket_Cache.SocketProxy.AddressType.IPV6 ||
                                addressType == Socket_Cache.SocketProxy.AddressType.Domain)
                            {
                                spu.ClientUDP_EndPoint = remoteEndPoint;

                                ReadOnlySpan<byte> bADDRESS = bData.Slice(4, bData.Length - 4);
                                IPEndPoint targetEndPoint = Socket_Operation.GetIPEndPoint_ByAddressType(addressType, bADDRESS);

                                ReadOnlySpan<byte> bUDP_Data = Socket_Operation.GetUDPData_ByAddressType(addressType, bData);
                                if (!bUDP_Data.IsEmpty)
                                {
                                    spu.ClientUDP_Time = DateTime.Now;
                                    Socket_Cache.SocketProxy.Total_Request += bUDP_Data.Length;
                                    Socket_Operation.CountProxySpeed(Socket_Cache.SocketProxy.ProxySpeedType.Uplink, bUDP_Data.Length);
                                    Socket_Operation.SendUDPData(spu.ClientUDP, bUDP_Data, targetEndPoint);
                                }
                            }
                        }
                        else
                        {
                            ReadOnlySpan<byte> bIP = spu.ClientUDP_EndPoint.Address.GetAddressBytes();
                            ushort port = ((ushort)spu.ClientUDP_EndPoint.Port);
                            ReadOnlySpan<byte> bPort = new byte[2] { (byte)(port >> 8), (byte)port };

                            Span<byte> bResponseData = stackalloc byte[4 + bIP.Length + bPort.Length + bData.Length];
                            bResponseData[0] = 0x00;
                            bResponseData[1] = 0x00;
                            bResponseData[2] = 0x00;
                            bResponseData[3] = (byte)Socket_Cache.SocketProxy.AddressType.IPV4;
                            bIP.CopyTo(bResponseData.Slice(4, bIP.Length));
                            bPort.CopyTo(bResponseData.Slice(8, bPort.Length));
                            bData.CopyTo(bResponseData.Slice(10, bData.Length));

                            if (!bResponseData.IsEmpty)
                            {
                                spu.ClientUDP_Time = DateTime.Now;
                                Socket_Cache.SocketProxy.Total_Response += bResponseData.Length;
                                Socket_Operation.CountProxySpeed(Socket_Cache.SocketProxy.ProxySpeedType.Downlink, bResponseData.Length);
                                Socket_Operation.SendUDPData(spu.ClientUDP, bResponseData, spu.ClientUDP_EndPoint);
                            }
                        }

                        Socket_Cache.SocketProxy.ProxyUDP_CNT++;
                        spu.ClientUDP.BeginReceive(new AsyncCallback(AcceptUdpCallback), spu);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//记录代理认证结果            

            public static void AuthResult_ToList(string IPAddress, string UserName, bool AuthResult)
            {
                try
                {
                    Proxy_AuthInfo pai = new Proxy_AuthInfo(IPAddress, UserName, AuthResult, DateTime.Now);
                    RecProxyAuth?.Invoke(pai);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//记录登录端IP地址

            public static void RecordLoginIP(Guid AccountID, string IPAddress)
            {
                try
                {
                    if (AccountID != Guid.Empty && !string.IsNullOrEmpty(IPAddress))
                    {
                        Proxy_AccountInfo paiItem = Socket_Cache.ProxyAccount.lstProxyAccount.FirstOrDefault(item => item.AID == AccountID);

                        if (paiItem != null)
                        {
                            paiItem.LoginIP = IPAddress;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//获取客户端的名称

            public static string GetClientName(Socket_ProxyTCP spc)
            {
                string sReturn = string.Empty;

                try
                {
                    if (spc != null && spc.ClientSocket != null)
                    {
                        if (!spc.ClientSocket.IsBound || !spc.ClientSocket.Connected)
                        {
                            return sReturn;
                        }

                        if (spc.ClientSocket.RemoteEndPoint is IPEndPoint endpoint)
                        {
                            return endpoint.Address.ToString();
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
            public static ConcurrentQueue<Socket_ProxyTCP> qSocket_ProxyTCP = new ConcurrentQueue<Socket_ProxyTCP>();
            public static ConcurrentQueue<Socket_ProxyUDP> qSocket_ProxyUDP = new ConcurrentQueue<Socket_ProxyUDP>();
            public static ConcurrentQueue<Socket_ProxyData> qSocket_ProxyData = new ConcurrentQueue<Socket_ProxyData>();

            #region//TCP代理入队列

            public static void ProxyTCP_ToQueue(Socket_ProxyTCP spc)
            {
                if (!Socket_Cache.SocketProxy.SpeedMode)
                {
                    qSocket_ProxyTCP.Enqueue(spc);
                }
            }

            #endregion

            #region//UDP代理入队列

            public static void ProxyUDP_ToQueue(Socket_ProxyUDP spu)
            {
                if (!Socket_Cache.SocketProxy.SpeedMode)
                {
                    qSocket_ProxyUDP.Enqueue(spu);
                }
            }

            #endregion

            #region//代理数据入队列

            public static void ProxyData_ToQueue(Socket_ProxyTCP spc, ReadOnlySpan<byte> bData, Socket_Cache.SocketProxy.DataType DataType)
            {
                try
                {
                    switch (DataType)
                    {
                        case SocketProxy.DataType.Request:
                            Socket_Cache.SocketProxy.Total_Request += bData.Length;
                            Socket_Operation.CountProxySpeed(Socket_Cache.SocketProxy.ProxySpeedType.Uplink, bData.Length);
                            break;

                        case SocketProxy.DataType.Response:
                            Socket_Cache.SocketProxy.Total_Response += bData.Length;
                            Socket_Operation.CountProxySpeed(Socket_Cache.SocketProxy.ProxySpeedType.Downlink, bData.Length);
                            break;
                    }

                    Socket_Cache.SocketProxy.ProxyTCP_CNT++;                    

                    if (!Socket_Cache.SocketProxy.SpeedMode)
                    {
                        Socket_ProxyData spd = new Socket_ProxyData(spc.ServerAddress, spc.DomainType, bData.ToArray(), DataType);
                        qSocket_ProxyData.Enqueue(spd);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//清除队列数据

            public static void ResetProxy_TCPQueue()
            {
                try
                {
                    while (!qSocket_ProxyTCP.IsEmpty)
                    {
                        qSocket_ProxyTCP.TryDequeue(out Socket_ProxyTCP spc);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void ResetProxy_UDPQueue()
            {
                try
                {
                    while (!qSocket_ProxyUDP.IsEmpty)
                    {
                        qSocket_ProxyUDP.TryDequeue(out Socket_ProxyUDP spu);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void ResetProxy_DataQueue()
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
            public static BindingList<Socket_ProxyTCP> lstProxyTCP = new BindingList<Socket_ProxyTCP>();
            public delegate void ProxyTCPReceived(Socket_ProxyTCP spc);
            public static event ProxyTCPReceived RecProxyTCP;

            public static BindingList<Socket_ProxyUDP> lstProxyUDP = new BindingList<Socket_ProxyUDP>();
            public delegate void ProxyUDPReceived(Socket_ProxyUDP spu);
            public static event ProxyUDPReceived RecProxyUDP;

            public static BindingList<Socket_ProxyData> lstProxyData = new BindingList<Socket_ProxyData>();
            public delegate void ProxyDataReceived(Socket_ProxyData spd);
            public static event ProxyDataReceived RecProxyData;

            #region//TCP代理入列表

            public static void ProxyTCP_ToList()
            {
                if (Socket_Cache.SocketProxyQueue.qSocket_ProxyTCP.TryDequeue(out Socket_ProxyTCP spc))
                {
                    lstProxyTCP.Add(spc);
                    RecProxyTCP?.Invoke(spc);
                }
            }

            #endregion            

            #region//UDP代理入列表

            public static void ProxyUDP_ToList()
            {
                if (Socket_Cache.SocketProxyQueue.qSocket_ProxyUDP.TryDequeue(out Socket_ProxyUDP spu))
                {
                    lstProxyUDP.Add(spu);
                    RecProxyUDP?.Invoke(spu);
                }
            }

            #endregion            

            #region//代理数据入列表

            public static void ProxyData_ToList()
            {
                if (Socket_Cache.SocketProxyQueue.qSocket_ProxyData.TryDequeue(out Socket_ProxyData spd))
                {
                    RecProxyData?.Invoke(spd);
                }
            }

            #endregion            

            #region//清除列表数据

            public static void ResetProxy_TCPList()
            {
                lstProxyTCP.Clear();
            }

            public static void ResetProxy_UDPList()
            {
                lstProxyUDP.Clear();
            }

            public static void ResetProxy_DataList()
            {
                lstProxyData.Clear();
            }

            #endregion
        }

        #endregion

        #region//代理账号

        public static class ProxyAccount
        {
            public static bool IsShow = false;
            public static string AESKey = string.Empty;
            public static BindingList<Proxy_AccountInfo> lstProxyAccount = new BindingList<Proxy_AccountInfo>();

            #region//验证远程管理的账号密码

            public static bool IsValidAdmin(string username, string password)
            {
                bool bReturn = false;

                try
                {
                    if (Socket_Cache.System.Remote_UserName.Equals(username) && Socket_Cache.System.Remote_PassWord.Equals(password))
                    {
                        bReturn = true;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
            }

            #endregion

            #region//检测代理账号是否已存在

            public static bool CheckProxyAccount_Exist(string UserName)
            {
                try
                {
                    foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                    {
                        if (pai.UserName.Equals(UserName))
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return false;
            }

            #endregion

            #region//检测用户名和密码是否正确（区分大小写）

            public static bool CheckUserNameAndPassWord(string UserName, string PassWord, out Guid AccountID)
            {
                AccountID = Guid.Empty;                                

                try
                {
                    string pwEncrypt = Socket_Operation.PassWord_Encrypt(PassWord);

                    foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                    {
                        if (pai.IsEnable && pai.UserName.Equals(UserName) && pai.PassWord.Equals(pwEncrypt))
                        {
                            if (pai.IsExpiry)
                            {
                                if (pai.ExpiryTime > DateTime.Now)
                                {
                                    AccountID = pai.AID;
                                    return true;
                                }
                            }
                            else
                            {
                                AccountID = pai.AID;
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return false;
            }

            #endregion

            #region//获取认证结果对应的图标

            public static Image GetImg_ByAuthResult(bool AuthResult)
            {
                try
                {
                    if (AuthResult)
                    {
                        return Properties.Resources.pass;
                    }
                    else
                    {
                        return Properties.Resources.fail;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return null;
                }
            }

            #endregion            

            #region//设置代理账号的在线情况

            public static void SetProxyAccount_Online(Guid AccountID, bool IsOnline)
            {
                try
                {
                    foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                    {
                        if (pai.AID.Equals(AccountID))
                        {
                            pai.IsOnLine = IsOnline;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//新增代理账号

            public static bool AddProxyAccount(Guid AID, bool IsEnable, string UserName, string PassWord, string LoginIP, bool IsExpiry, DateTime ExpiryTime, DateTime CreateTime)
            {
                try
                {
                    if (AID != Guid.Empty && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PassWord))
                    {
                        if (!Socket_Cache.ProxyAccount.CheckProxyAccount_Exist(UserName))
                        {
                            Proxy_AccountInfo pai = new Proxy_AccountInfo(AID, IsEnable, UserName, PassWord, LoginIP, IsExpiry, ExpiryTime, CreateTime);
                            Socket_Cache.ProxyAccount.ProxyAccountToList(pai);

                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return false;
            }

            #endregion

            #region//更新代理账号            

            public static bool UpdateProxyAccount_ByAccountID(Guid AID, bool IsEnable, string PassWord, bool IsExpiry, DateTime ExpiryTime)
            {
                try
                {
                    if (AID != null)
                    {
                        var pai = Socket_Cache.ProxyAccount.lstProxyAccount.FirstOrDefault(account => account.AID == AID);

                        if (pai != null)
                        {
                            pai.IsEnable = IsEnable;
                            pai.PassWord = PassWord;
                            pai.IsExpiry = IsExpiry;
                            pai.ExpiryTime = ExpiryTime;

                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return false;
            }

            #endregion

            #region//删除代理账号

            public static void DeleteProxyAccount_Dialog(Guid[] glAID)
            {
                try
                {
                    if (glAID.Length > 0)
                    {
                        DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                        if (dr.Equals(DialogResult.OK))
                        {
                            foreach (Guid AID in glAID)
                            {
                                if (AID != null)
                                {
                                    Socket_Cache.ProxyAccount.DeleteProxyAccount_ByAccountID(AID);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }            

            public static bool DeleteProxyAccount_ByAccountID(Guid AID)
            {
                try
                {
                    if (AID != null)
                    {
                        var pai = Socket_Cache.ProxyAccount.lstProxyAccount.FirstOrDefault(account => account.AID == AID);

                        if (pai != null)
                        {
                            if (Socket_Cache.System.InvokeAction != null)
                            {
                                Socket_Cache.System.InvokeAction(() =>
                                {
                                    Socket_Cache.ProxyAccount.lstProxyAccount.Remove(pai);
                                });
                            }
                            
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return false;
            }

            #endregion

            #region//查找代理账号

            public static Proxy_AccountInfo GetProxyAccount_ByAccountID(Guid AID)
            {
                try
                {
                    if (AID != null)
                    {
                        Proxy_AccountInfo pai = Socket_Cache.ProxyAccount.lstProxyAccount.FirstOrDefault(account => account.AID == AID);

                        if (pai != null)
                        {
                            return pai;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            public static BindingList<Proxy_AccountInfo> GetProxyAccount_ByUserName(string UserName)
            {
                try
                {
                    if (!string.IsNullOrEmpty(UserName))
                    {
                        BindingList<Proxy_AccountInfo> pai = new BindingList<Proxy_AccountInfo>
                            (lstProxyAccount.Where(account => account.UserName.Contains(UserName)).ToList());

                        return pai;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            public static BindingList<Proxy_AccountInfo> GetProxyAccount_ByIsEnable(bool IsEnable)
            {
                try
                {
                    BindingList<Proxy_AccountInfo> pai = new BindingList<Proxy_AccountInfo>
                        (lstProxyAccount.Where(account => account.IsEnable == IsEnable).ToList());

                    return pai;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            public static BindingList<Proxy_AccountInfo> GetProxyAccount_ByIsOnLine(bool IsOnLine)
            {
                try
                {
                    BindingList<Proxy_AccountInfo> pai = new BindingList<Proxy_AccountInfo>
                        (lstProxyAccount.Where(account => account.IsOnLine == IsOnLine).ToList());

                    return pai;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            public static BindingList<Proxy_AccountInfo> GetProxyAccount_ByIsExpiry(bool IsExpiry)
            {
                try
                {
                    BindingList<Proxy_AccountInfo> pai = new BindingList<Proxy_AccountInfo>
                        (lstProxyAccount.Where(account => account.IsExpiry == IsExpiry).ToList());

                    return pai;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            #endregion

            #region//代理账号入列表

            public static void ProxyAccountToList(Proxy_AccountInfo pai)
            {
                try
                {
                    if (Socket_Cache.System.InvokeAction != null)
                    {
                        Socket_Cache.System.InvokeAction(() =>
                        {
                            Socket_Cache.ProxyAccount.lstProxyAccount.Add(pai);
                        });
                    }                       
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//重置代理账号的在线状态

            public static void ResetProxyAccount_Online()
            {
                try
                {
                    foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                    {
                        pai.IsOnLine = false;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//保存代理账号列表到数据库

            public static void SaveProxyAccountList_ToDB(Socket_Cache.System.SystemMode FromMode)
            {
                try
                {
                    if (Socket_Cache.System.StartMode == FromMode)
                    {
                        Socket_Cache.DataBase.DeleteTable_ProxyAccount();

                        foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                        {
                            Socket_Cache.DataBase.InsertTable_ProxyAccount(pai);
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//从数据库加载代理账号列表

            public static void LoadProxyAccountList_FromDB()
            {
                try
                {
                    DataTable dtProxyAccount = Socket_Cache.DataBase.SelectTable_ProxyAccount();

                    foreach (DataRow dataRow in dtProxyAccount.Rows)
                    {
                        Guid AID = Guid.Parse(dataRow["GUID"].ToString());
                        bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);
                        string UserName = dataRow["UserName"].ToString();
                        string PassWord = dataRow["PassWord"].ToString();
                        string LoginIP = dataRow["LoginIP"].ToString();
                        bool IsExpiry = Convert.ToBoolean(dataRow["IsExpiry"]);
                        DateTime ExpiryTime = Convert.ToDateTime(dataRow["ExpiryTime"]);
                        DateTime CreateTime = Convert.ToDateTime(dataRow["CreateTime"]);                        

                        Socket_Cache.ProxyAccount.AddProxyAccount(AID, IsEnable, UserName, PassWord, LoginIP, IsExpiry, ExpiryTime, CreateTime);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//保存代理账号列表到文件（对话框）

            public static void SaveProxyAccountList_Dialog(string FileName, Guid[] glAID)
            {
                try
                {
                    if (Socket_Cache.ProxyAccount.lstProxyAccount.Count > 0)
                    {
                        SaveFileDialog sfdSaveFile = new SaveFileDialog();
                        sfdSaveFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_189) + "（*.pa）|*.pa";

                        if (!string.IsNullOrEmpty(FileName))
                        {
                            sfdSaveFile.FileName = FileName;
                        }

                        sfdSaveFile.RestoreDirectory = true;

                        if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                        {
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.ProxyAccount_Export);
                            pwForm.ShowDialog();

                            string FilePath = sfdSaveFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                SaveProxyAccountList(FilePath, glAID, true);

                                string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_190), FilePath);
                                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void SaveProxyAccountList(string FilePath, Guid[] glAID, bool DoEncrypt)
            {
                try
                {
                    SaveProxyAccountList_ToXDocument(FilePath, glAID);

                    if (DoEncrypt)
                    {
                        string sPassword = Socket_Cache.ProxyAccount.AESKey;

                        if (!string.IsNullOrEmpty(sPassword))
                        {
                            Socket_Operation.EncryptXMLFile(FilePath, sPassword);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void SaveProxyAccountList_ToXDocument(string FilePath, Guid[] glAID)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeProxyAccountList = new XElement("ProxyAccountList");
                    xdoc.Add(xeProxyAccountList);

                    if (Socket_Cache.ProxyAccount.lstProxyAccount.Count > 0)
                    {
                        foreach (Guid AID in glAID)
                        { 
                            Proxy_AccountInfo pai = Socket_Cache.ProxyAccount.GetProxyAccount_ByAccountID(AID);

                            if (pai != null)
                            {
                                XElement xeProxyAccount =
                                    new XElement("ProxyAccount",
                                    new XElement("IsEnable", pai.IsEnable.ToString()),
                                    new XElement("AID", pai.AID.ToString().ToUpper()),
                                    new XElement("UserName", pai.UserName),
                                    new XElement("PassWord", pai.PassWord),
                                    new XElement("LoginIP", pai.LoginIP),
                                    new XElement("IsOnLine", pai.IsOnLine.ToString()),
                                    new XElement("IsExpiry", pai.IsExpiry),
                                    new XElement("ExpiryTime", pai.ExpiryTime.ToString("yyyy/MM/dd HH:mm:ss")),
                                    new XElement("CreateTime", pai.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                    );

                                xeProxyAccountList.Add(xeProxyAccount);
                            }
                        }
                    }

                    xdoc.Save(FilePath);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//从文件加载代理账号列表（对话框）

            public static void LoadProxyAccountList_Dialog()
            {
                try
                {
                    OpenFileDialog ofdLoadFile = new OpenFileDialog();

                    ofdLoadFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_189) + " (*.pa)|*.pa|INI Files (*.ini)|*.ini";
                    ofdLoadFile.RestoreDirectory = true;

                    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = ofdLoadFile.FileName;

                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            LoadProxyAccountList(FilePath, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void LoadProxyAccountList(string FilePath, bool LoadFromUser)
            {
                try
                {
                    if (File.Exists(FilePath))
                    {
                        string fileExtension = Path.GetExtension(FilePath);

                        if (!string.IsNullOrEmpty(fileExtension))
                        {                            
                            if (fileExtension.Equals(".ini"))
                            {
                                LoadProxyAccountList_FromInIFile(FilePath);
                            }
                            else
                            {
                                #region//LoadProxyAccountList_FromXDocument

                                XDocument xdoc = new XDocument();

                                bool bEncrypt = Socket_Operation.IsEncryptXMLFile(FilePath);

                                if (bEncrypt)
                                {
                                    if (LoadFromUser)
                                    {
                                        Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.ProxyAccount_Import);
                                        pwForm.ShowDialog();
                                    }

                                    xdoc = Socket_Operation.DecryptXMLFile(FilePath, Socket_Cache.ProxyAccount.AESKey);
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
                                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sError);
                                    }
                                }
                                else
                                {
                                    LoadProxyAccountList_FromXDocument(xdoc);

                                    if (bEncrypt)
                                    {
                                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_192));
                                    }
                                    else
                                    {
                                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_191));
                                    }
                                }

                                #endregion
                            }
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void LoadProxyAccountList_FromXDocument(XDocument xdoc)
            {
                try
                {
                    foreach (XElement xeProxyAccount in xdoc.Root.Elements())
                    {
                        bool IsEnable = false;
                        if (xeProxyAccount.Element("IsEnable") != null)
                        {
                            IsEnable = bool.Parse(xeProxyAccount.Element("IsEnable").Value);
                        }

                        Guid AID = Guid.NewGuid();

                        string UserName = string.Empty;
                        if (xeProxyAccount.Element("UserName") != null)
                        {
                            UserName = xeProxyAccount.Element("UserName").Value;
                        }

                        string PassWord = string.Empty;
                        if (xeProxyAccount.Element("PassWord") != null)
                        {
                            PassWord = xeProxyAccount.Element("PassWord").Value;
                        }

                        string LoginIP = string.Empty;
                        if (xeProxyAccount.Element("LoginIP") != null)
                        {
                            LoginIP = xeProxyAccount.Element("LoginIP").Value;
                        }

                        bool IsOnLine = false;
                        if (xeProxyAccount.Element("IsOnLine") != null)
                        {
                            IsOnLine = bool.Parse(xeProxyAccount.Element("IsOnLine").Value);
                        }

                        bool IsExpiry = false;
                        if (xeProxyAccount.Element("IsExpiry") != null)
                        {
                            IsExpiry = bool.Parse(xeProxyAccount.Element("IsExpiry").Value);
                        }

                        DateTime ExpiryTime = DateTime.Now;
                        if (xeProxyAccount.Element("ExpiryTime") != null)
                        {
                            ExpiryTime = DateTime.Parse(xeProxyAccount.Element("ExpiryTime").Value);
                        }

                        DateTime CreateTime = DateTime.Now;
                        if (xeProxyAccount.Element("CreateTime") != null)
                        {
                            CreateTime = DateTime.Parse(xeProxyAccount.Element("CreateTime").Value);
                        }

                        bool bOK = Socket_Cache.ProxyAccount.AddProxyAccount(AID, IsEnable, UserName, PassWord, LoginIP, IsExpiry, ExpiryTime, CreateTime);

                        if (!bOK)
                        {
                            string FailLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_193), UserName);
                            Socket_Operation.DoLog_Proxy("Import Proxy Account", FailLog);
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void LoadProxyAccountList_FromInIFile(string filePath)
            {
                try
                {
                    string[] lines = File.ReadAllLines(filePath);

                    Proxy_AccountInfo pai = null;
                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim();
                        if (trimmedLine.StartsWith("[User"))
                        {
                            if (pai != null)
                            {
                                Socket_Cache.ProxyAccount.AddProxyAccount_FromIniFile(pai);
                            }

                            pai = new Proxy_AccountInfo();
                        }
                        else if (trimmedLine.Contains("="))
                        {
                            string[] parts = trimmedLine.Split(new char[] { '=' }, 2);
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();

                            switch (key)
                            {
                                case "Enable":
                                    pai.IsEnable = Convert.ToBoolean(int.Parse(value));
                                    break;

                                case "UserName":
                                    pai.UserName = value;
                                    break;

                                case "Password":
                                    pai.PassWord = value;
                                    break;

                                case "AutoDisable":
                                    pai.IsExpiry = Convert.ToBoolean(int.Parse(value));
                                    break;

                                case "DisableDateTime":
                                    pai.ExpiryTime = DateTime.Parse(value);
                                    break;
                            }
                        }
                    }

                    if (pai != null)
                    {
                        Socket_Cache.ProxyAccount.AddProxyAccount_FromIniFile(pai);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }                
            }

            private static void AddProxyAccount_FromIniFile(Proxy_AccountInfo pai)
            {
                try
                {
                    if (pai != null)
                    {
                        if (pai.AID == null || pai.AID == Guid.Empty)
                        {
                            pai.AID = Guid.NewGuid();
                        }

                        if (pai.ExpiryTime == DateTime.MinValue)
                        {
                            pai.ExpiryTime = DateTime.Now;
                        }                        

                        if (pai.CreateTime == DateTime.MinValue)
                        {
                            pai.CreateTime = DateTime.Now;
                        }

                        if (string.IsNullOrEmpty(pai.LoginIP))
                        {
                            pai.LoginIP = string.Empty;
                        }

                        bool bOK = Socket_Cache.ProxyAccount.AddProxyAccount(pai.AID, pai.IsEnable, pai.UserName, pai.PassWord, pai.LoginIP, pai.IsExpiry, pai.ExpiryTime, pai.CreateTime);

                        if (!bOK)
                        {
                            string FailLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_193), pai.UserName);
                            Socket_Operation.DoLog_Proxy("Import Proxy Account", FailLog);
                        }
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

        #region//封包

        public static class SocketPacket
        {
            public static int PacketData_MaxLen = 60;
            public static long TotalPackets = 0;
            public static long Total_SendBytes = 0;
            public static long Total_RecvBytes = 0;
            public static bool SpeedMode;            
            public static byte[] bByteBuff = new byte[0];
            public static string InjectProcess = string.Empty;
            public static string SocketBytesInfo = string.Empty;
            public static bool Support_WS1, Support_WS2, Support_MsWS;
            public static bool HookWS1_Send = true, HookWS1_SendTo = true, HookWS1_Recv = true, HookWS1_RecvFrom = true;
            public static bool HookWS2_Send = true, HookWS2_SendTo = true, HookWS2_Recv = true, HookWS2_RecvFrom = true;
            public static bool HookWSA_Send = true, HookWSA_SendTo = true, HookWSA_Recv = true, HookWSA_RecvFrom = true;
            public static bool CheckNotShow = true, CheckSize, CheckSocket, CheckIP, CheckPort, CheckHead, CheckData;
            public static string CheckSocket_Value, CheckLength_Value, CheckIP_Value, CheckPort_Value, CheckHead_Value, CheckData_Value;
            private static readonly Image SentImage = Properties.Resources.sent;
            private static readonly Image ReceivedImage = Properties.Resources.received;
            public static readonly Font FontUnderline = new Font(RichTextBox.DefaultFont, FontStyle.Underline);
            public static readonly Font FontStrikeout = new Font(RichTextBox.DefaultFont, FontStyle.Strikeout);

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

            private static class PacketTypeNames
            {
                public static readonly string WS1_Send = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_54);
                public static readonly string WS2_Send = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_156);
                public static readonly string WS1_Recv = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_55);
                public static readonly string WS2_Recv = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_157);
                public static readonly string WS1_SendTo = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_56);
                public static readonly string WS2_SendTo = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_158);
                public static readonly string WS1_RecvFrom = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_57);
                public static readonly string WS2_RecvFrom = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_159);
                public static readonly string WSASend = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_58);
                public static readonly string WSARecv = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_59);
                public static readonly string WSARecvEx = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_59);
                public static readonly string WSASendTo = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_60);
                public static readonly string WSARecvFrom = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_61);
            }

            public static string GetName_ByPacketType(Socket_Cache.SocketPacket.PacketType socketType)
            {
                try
                {
                    switch (socketType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                            return PacketTypeNames.WS1_Send;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                            return PacketTypeNames.WS2_Send;

                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                            return PacketTypeNames.WS1_Recv;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                            return PacketTypeNames.WS2_Recv;

                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                            return PacketTypeNames.WS1_SendTo;

                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                            return PacketTypeNames.WS2_SendTo;

                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                            return PacketTypeNames.WS1_RecvFrom;

                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                            return PacketTypeNames.WS2_RecvFrom;

                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                            return PacketTypeNames.WSASend;

                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                            return PacketTypeNames.WSARecv;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                            return PacketTypeNames.WSARecvEx;

                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                            return PacketTypeNames.WSASendTo;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            return PacketTypeNames.WSARecvFrom;

                        default:
                            return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return string.Empty;
                }
            }

            #endregion            

            #region//获取封包类型对应的图标

            public static Image GetImg_ByPacketType(Socket_Cache.SocketPacket.PacketType socketType)
            {
                try
                {
                    switch (socketType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                            return Socket_Cache.SocketPacket.SentImage;

                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            return Socket_Cache.SocketPacket.ReceivedImage;

                        default:
                            return null;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return null;
                }
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
                    Socket_Operation.CountSocketInfo(ptPacketType, bBuffByte.Length);

                    if (!Socket_Cache.SocketPacket.SpeedMode)
                    {
                        string sPacketIP = Socket_Operation.GetIPString_BySocketAddr(iSocket, sAddr, ptPacketType);

                        if (!string.IsNullOrEmpty(sPacketIP) && sPacketIP.Contains("|"))
                        {
                            string[] ipParts = sPacketIP.Split('|');
                            string sIPFrom = ipParts[0];
                            string sIPTo = ipParts[1];
                            DateTime dtTime = DateTime.Now;

                            Socket_PacketInfo spc = new Socket_PacketInfo(dtTime, iSocket, ptPacketType, sIPFrom, sIPTo, bRawBuff, bBuffByte, bBuffByte.Length, pAction);
                            qSocket_PacketInfo.Enqueue(spc);
                        }
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
                        qSocket_PacketInfo.TryDequeue(out Socket_PacketInfo spc);
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
            public static bool AutoRoll = false;
            public static bool AutoClear = true;
            public static decimal AutoClear_Value = 5000;
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
                        if (SocketQueue.qSocket_PacketInfo.TryDequeue(out Socket_PacketInfo spc))
                        {
                            bool bIsShow = Socket_Operation.IsShowSocketPacket_ByFilter(spc);
                            if (bIsShow)
                            {
                                Span<byte> bufferSpan = spc.PacketBuffer.AsSpan();
                                spc.PacketData = Socket_Operation.GetPacketData_Hex(bufferSpan, iMax_DataLen);
                                RecSocketPacket?.Invoke(spc);
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

            public static int SearchForSocketList(int fromIndex, ReadOnlySpan<byte> searchData)
            {
                int iResult = -1;

                try
                {
                    if (searchData.Length == 0 || fromIndex < 0)
                    {
                        return -1;
                    }

                    int listCount = Socket_Cache.SocketList.lstRecPacket.Count;
                    if (listCount == 0 || fromIndex >= listCount)
                    {
                        return -1;
                    }

                    for (int i = fromIndex; i < listCount; i++)
                    {
                        byte[] packetBuffer = Socket_Cache.SocketList.lstRecPacket[i].PacketBuffer;
                        if (packetBuffer != null && packetBuffer.Length >= searchData.Length)
                        {
                            ReadOnlySpan<byte> packetSpan = packetBuffer.AsSpan();
                            if (packetSpan.IndexOf(searchData) != -1)
                            {
                                return i;
                            }
                        }
                    }
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

            public static void LogToQueue(Socket_Cache.System.LogType logType, string sFuncName, string sLogContent)
            {
                try
                {
                    Socket_LogInfo sli = new Socket_LogInfo(sFuncName, sLogContent);

                    switch (logType)
                    {
                        case Socket_Cache.System.LogType.Socket:
                            qSocket_Log.Enqueue(sli);
                            break;

                        case Socket_Cache.System.LogType.Proxy:
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

            public static void ResetLogQueue(Socket_Cache.System.LogType logType)
            {
                try
                {
                    switch (logType)
                    {
                        case Socket_Cache.System.LogType.Socket:

                            while (!qSocket_Log.IsEmpty)
                            {
                                qSocket_Log.TryDequeue(out Socket_LogInfo sli);
                            }

                            break;

                        case Socket_Cache.System.LogType.Proxy:

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
            public static bool Socket_AutoRoll = false, Proxy_AutoRoll = false, Socket_AutoClear = true, Proxy_AutoClear = true;
            public static decimal Socket_AutoClear_Value = 5000, Proxy_AutoClear_Value = 5000;
            public static BindingList<Socket_LogInfo> lstSocketLog = new BindingList<Socket_LogInfo>();
            public static BindingList<Socket_LogInfo> lstProxyLog = new BindingList<Socket_LogInfo>();

            public delegate void SocketLogReceived(Socket_LogInfo sl);
            public delegate void ProxyLogReceived(Socket_LogInfo sli);

            public static event SocketLogReceived RecSocketLog;
            public static event ProxyLogReceived RecProxyLog;

            #region//日志入列表

            public static void LogToList(Socket_Cache.System.LogType logType)
            {
                switch (logType)
                {
                    case Socket_Cache.System.LogType.Socket:

                        if (LogQueue.qSocket_Log.TryDequeue(out Socket_LogInfo sliSocket))
                        {
                            RecSocketLog?.Invoke(sliSocket);
                        }

                        break;

                    case Socket_Cache.System.LogType.Proxy:

                        if (LogQueue.qProxy_Log.TryDequeue(out Socket_LogInfo sliProxy))
                        {
                            RecProxyLog?.Invoke(sliProxy);
                        }

                        break;
                }
            }

            #endregion

            #region//清除列表数据

            public static void ResetLogList(Socket_Cache.System.LogType logType)
            {
                switch (logType)
                {
                    case Socket_Cache.System.LogType.Socket:
                        lstSocketLog.Clear();
                        break;

                    case Socket_Cache.System.LogType.Proxy:
                        lstProxyLog.Clear();
                        break;
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
            public static Socket_Cache.Filter.Execute FilterExecute = Socket_Cache.Filter.Execute.Sequence;
            public static readonly Color FilterActionForeColor_Replace = Color.Black;
            public static readonly Color FilterActionBackColor_Replace = Color.Goldenrod;
            public static readonly Color FilterActionForeColor_Intercept = Color.White;
            public static readonly Color FilterActionBackColor_Intercept = Color.DarkRed;
            public static readonly Color FilterActionForeColor_Change = Color.Black;
            public static readonly Color FilterActionBackColor_Change = Color.DodgerBlue;
            public static readonly Color FilterActionForeColor_Other = Color.LimeGreen;
            public static readonly Color FilterActionBackColor_Other = Color.FromArgb(30, 30, 30);

            #region//定义结构

            public enum Execute
            {
                Priority,
                Sequence,
            }

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
                Change,
            }

            public enum FilterExecuteType
            {
                Send,
                Robot,
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
                    Socket_Cache.Filter.FilterExecuteType FilterExecuteType = new Socket_Cache.Filter.FilterExecuteType();
                    Guid SID = Guid.Empty;
                    Guid RID = Guid.Empty;
                    Socket_Cache.Filter.FilterFunction FilterFunction = new Socket_Cache.Filter.FilterFunction(true, true, true, true, false, false, false, false);
                    Socket_Cache.Filter.FilterStartFrom FilterStartFrom = Socket_Cache.Filter.FilterStartFrom.Head;

                    Socket_Cache.Filter.AddFilter(false, FID, FName, false, string.Empty, false, 0, false, string.Empty, false, 0, FilterMode, FilterAction, false, FilterExecuteType, SID, RID, FilterFunction, FilterStartFrom, false, 1, string.Empty, 0, string.Empty, string.Empty);
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
                        Socket_Cache.Filter.FilterExecuteType FilterExecuteType = new Socket_Cache.Filter.FilterExecuteType();
                        Guid SID = Guid.Empty;
                        Guid RID = Guid.Empty;
                        Socket_Cache.Filter.FilterFunction FilterFunction = Socket_Cache.Filter.GetFilterFunction_ByPacketType(ptType);
                        Socket_Cache.Filter.FilterStartFrom FilterStartFrom = Socket_Cache.Filter.FilterStartFrom.Head;

                        string sFSearch = Socket_Cache.Filter.GetFilterString_ByBytes(bBuffer);

                        Socket_Cache.Filter.AddFilter(false, FID, sFName, false, string.Empty, false, 0, false, string.Empty, false, 0, FilterMode, FilterAction, false, FilterExecuteType, SID, RID, FilterFunction, FilterStartFrom, false, 1, string.Empty, 0, sFSearch, string.Empty);
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
                string LengthContent,
                bool bAppointPort,
                decimal PortContent,
                Socket_Cache.Filter.FilterMode FilterMode,
                Socket_Cache.Filter.FilterAction FilterAction,
                bool IsExecute,
                Socket_Cache.Filter.FilterExecuteType FEType,
                Guid SID,
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
                        bAppointPort,
                        PortContent,
                        FilterMode,
                        FilterAction,
                        IsExecute,
                        FEType,
                        SID,
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
                string LengthContent,
                bool AppointPort,
                decimal PortContent,
                Socket_Cache.Filter.FilterMode FilterMode,
                Socket_Cache.Filter.FilterAction FilterAction,
                bool IsExecute,
                Socket_Cache.Filter.FilterExecuteType FEType,
                Guid SID,
                Guid RID,
                Socket_Cache.Filter.FilterFunction FilterFunction,
                Socket_Cache.Filter.FilterStartFrom FilterStartFrom,
                decimal ProgressionStep,
                string ProgressionPosition,
                int ProgressionCount,
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
                        Socket_Cache.FilterList.lstFilter[iFIndex].AppointPort = AppointPort;
                        Socket_Cache.FilterList.lstFilter[iFIndex].PortContent = PortContent;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FMode = FilterMode;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FAction = FilterAction;
                        Socket_Cache.FilterList.lstFilter[iFIndex].IsExecute = IsExecute;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FEType = FEType;
                        Socket_Cache.FilterList.lstFilter[iFIndex].SID = SID;
                        Socket_Cache.FilterList.lstFilter[iFIndex].RID = RID;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FFunction = FilterFunction;
                        Socket_Cache.FilterList.lstFilter[iFIndex].FStartFrom = FilterStartFrom;
                        Socket_Cache.FilterList.lstFilter[iFIndex].ProgressionStep = ProgressionStep;
                        Socket_Cache.FilterList.lstFilter[iFIndex].ProgressionPosition = ProgressionPosition;
                        Socket_Cache.FilterList.lstFilter[iFIndex].ProgressionCount = ProgressionCount;
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
                    string LengthContent = Socket_Cache.FilterList.lstFilter[iFIndex].LengthContent;
                    bool bAppointPort = Socket_Cache.FilterList.lstFilter[iFIndex].AppointPort;
                    decimal PortContent = Socket_Cache.FilterList.lstFilter[iFIndex].PortContent;
                    Socket_Cache.Filter.FilterMode FMode = Socket_Cache.FilterList.lstFilter[iFIndex].FMode;
                    Socket_Cache.Filter.FilterAction FAction = Socket_Cache.FilterList.lstFilter[iFIndex].FAction;
                    bool IsExecute = Socket_Cache.FilterList.lstFilter[iFIndex].IsExecute;
                    Socket_Cache.Filter.FilterExecuteType FEType = Socket_Cache.FilterList.lstFilter[iFIndex].FEType;
                    Guid SID = Socket_Cache.FilterList.lstFilter[iFIndex].SID;
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
                        bAppointPort,
                        PortContent,
                        FMode,
                        FAction,
                        IsExecute,
                        FEType,
                        SID,
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

            public static Socket_Cache.Filter.FilterExecuteType GetFilterExecuteType_ByString(string FilterExecuteType)
            {
                Socket_Cache.Filter.FilterExecuteType FEType = new Socket_Cache.Filter.FilterExecuteType();

                try
                {
                    FEType = (Socket_Cache.Filter.FilterExecuteType)Enum.Parse(typeof(Socket_Cache.Filter.FilterExecuteType), FilterExecuteType);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return FEType;
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

                        case Socket_Cache.Filter.FilterAction.Change:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_173);
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

            public static bool CheckFilter_IsEffective(Int32 iSocket, Span<byte> bufferSpan, Socket_Cache.SocketPacket.PacketType ptType, Socket_Cache.SocketPacket.SockAddr sAddr, Socket_FilterInfo sfi)
            {
                bool bResult = true;

                try
                {
                    if (!sfi.IsEnable)
                    {
                        return false;
                    }

                    if (!Socket_Cache.Filter.CheckFilterFunction_ByPacketType(ptType, sfi.FFunction))
                    {
                        return false;
                    }

                    if (sfi.AppointSocket && !Socket_Cache.Filter.CheckPacket_IsMatch_AppointSocket(iSocket, sfi.SocketContent))
                    {
                        return false;
                    }

                    if (sfi.AppointLength && !Socket_Cache.Filter.CheckPacket_IsMatch_AppointLength(bufferSpan.Length, sfi.LengthContent))
                    {
                        return false;
                    }

                    if (sfi.AppointPort && !Socket_Cache.Filter.CheckPacket_IsMatch_AppointPort(iSocket, ptType, sAddr, sfi.PortContent))
                    {
                        return false;
                    }

                    if (sfi.AppointHeader && !Socket_Cache.Filter.CheckPacket_IsMatch_AppointHeader(bufferSpan, sfi.HeaderContent))
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

            public static bool CheckPacket_IsMatch_AppointLength(int Len, string LengthContent)
            {
                bool bResult = false;

                try
                {
                    if (!string.IsNullOrEmpty(LengthContent))
                    {
                        if (LengthContent.Contains("-"))
                        {
                            string[] sLengthContent = LengthContent.Split('-');
                            if (int.TryParse(sLengthContent[0], out int iLenFrom))
                            {
                                if (int.TryParse(sLengthContent[1], out int iLenTo))
                                {
                                    if (Len >= iLenFrom && Len <= iLenTo)
                                    {
                                        bResult = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (int.TryParse(LengthContent, out int iLength))
                            {
                                if (Len == iLength)
                                {
                                    bResult = true;
                                }
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

            #region//检查是否匹配指定端口

            public static bool CheckPacket_IsMatch_AppointPort(Int32 iSocket, Socket_Cache.SocketPacket.PacketType ptType, Socket_Cache.SocketPacket.SockAddr sAddr, decimal dPortContent)
            {
                bool bResult = false;

                try
                {
                    string sPort = string.Empty;
                    string sPacketIP = Socket_Operation.GetIPString_BySocketAddr(iSocket, sAddr, ptType);

                    if (!string.IsNullOrEmpty(sPacketIP) && sPacketIP.IndexOf("|") > 0)
                    {
                        string sIPFrom = sPacketIP.Split('|')[0];
                        string sIPTo = sPacketIP.Split('|')[1];
                        string sPortFrom = string.Empty;
                        string sPortTo = string.Empty;

                        if (!string.IsNullOrEmpty(sIPFrom) && sIPFrom.IndexOf(":") > 0)
                        {
                            sPortFrom = sIPFrom.Split(':')[1];

                            if (sPortFrom.Equals(dPortContent.ToString()))
                            {
                                bResult = true;
                            }
                        }

                        if (!string.IsNullOrEmpty(sIPTo) && sIPTo.IndexOf(":") > 0)
                        {
                            sPortTo = sIPTo.Split(':')[1];

                            if (sPortTo.Equals(dPortContent.ToString()))
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

            #region//检查是否匹配指定包头

            public static bool CheckPacket_IsMatch_AppointHeader(Span<byte> bufferSpan, string sHeaderContent)
            {
                if (string.IsNullOrEmpty(sHeaderContent))
                {
                    return false;
                }

                try
                {
                    byte[] bHeaderContent = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sHeaderContent);
                    int iHeaderContent_Len = bHeaderContent.Length;

                    if (iHeaderContent_Len > 0 && iHeaderContent_Len <= bufferSpan.Length)
                    {
                        Span<byte> headerSpan = new Span<byte>(bHeaderContent);

                        for (int i = 0; i < iHeaderContent_Len; i++)
                        {
                            if (bufferSpan[i] != headerSpan[i])
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                }

                return false;
            }

            #endregion

            #region//检查滤镜是否匹配成功（普通滤镜）

            public static bool CheckFilter_IsMatch_Normal(Socket_FilterInfo sfi, Span<byte> bufferSpan)
            {
                if (string.IsNullOrEmpty(sfi.FSearch))
                {
                    return false;
                }

                try
                {
                    string[] slSearch = sfi.FSearch.Split(',');

                    foreach (string sSearch in slSearch)
                    {
                        if (!string.IsNullOrEmpty(sSearch) && sSearch.IndexOf("|") > 0)
                        {
                            string[] searchParts = sSearch.Split('|');

                            if (int.TryParse(searchParts[0], out int iIndex) && iIndex >= 0 && iIndex < bufferSpan.Length)
                            {
                                string sValue = searchParts[1].ToUpper();
                                byte bufferByte = bufferSpan[iIndex];
                                string sBuffValue = bufferByte.ToString("X2");

                                if (!sValue.Equals(sBuffValue))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                    return false;
                }

                return true;
            }

            #endregion

            #region//检查滤镜是否匹配成功（高级滤镜）

            public static List<int> CheckFilter_IsMatch_Adcanced(Socket_FilterInfo sfi, Span<byte> bufferSpan)
            {
                List<int> lReturn = new List<int>();

                if (string.IsNullOrEmpty(sfi.FSearch))
                {
                    return lReturn;
                }

                try
                {
                    string[] slSearch = sfi.FSearch.Split(',');

                    int[] searchIndices = new int[slSearch.Length];
                    byte[] searchValues = new byte[slSearch.Length];

                    for (int i = 0; i < slSearch.Length; i++)
                    {
                        string[] searchParts = slSearch[i].Split('|');

                        if (int.TryParse(searchParts[0], out int iIndex) && byte.TryParse(searchParts[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte bValue))
                        {
                            searchIndices[i] = iIndex;
                            searchValues[i] = bValue;
                        }
                    }

                    int iMatchIndex = -1;
                    int iBuffIndex = -1;

                    byte bFirst_SearchValue = searchValues[0];

                    for (int i = 0; i < bufferSpan.Length; i++)
                    {
                        if (bufferSpan[i] == bFirst_SearchValue)
                        {
                            iMatchIndex = i;

                            for (int j = 1; j < slSearch.Length; j++)
                            {
                                int iIndex = searchIndices[j];
                                byte bValue = searchValues[j];

                                iBuffIndex = i + iIndex;

                                if (iBuffIndex >= 0 && iBuffIndex < bufferSpan.Length)
                                {
                                    if (bufferSpan[iBuffIndex] != bValue)
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
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                }

                return lReturn;
            }

            #endregion            

            #region//执行替换（普通滤镜）

            public static bool Replace_Normal(Socket_FilterInfo sfi, Span<byte> bufferSpan)
            {
                if (string.IsNullOrEmpty(sfi.FSearch))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(sfi.FModify) && string.IsNullOrEmpty(sfi.ProgressionPosition))
                {
                    return false;
                }

                try
                {
                    if (!string.IsNullOrEmpty(sfi.FModify))
                    {
                        string[] slModify = sfi.FModify.Split(',');

                        foreach (string sModify in slModify)
                        {
                            if (!string.IsNullOrEmpty(sModify) && sModify.IndexOf("|") > 0)
                            {
                                string[] modifyParts = sModify.Split('|');

                                if (int.TryParse(sModify.Split('|')[0], out int iIndex) && iIndex >= 0 && iIndex < bufferSpan.Length)
                                {
                                    if (byte.TryParse(modifyParts[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte bValue))
                                    {
                                        bufferSpan[iIndex] = bValue;
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
                            if (!string.IsNullOrEmpty(sProgression) && int.TryParse(sProgression, out int iIndex) && iIndex >= 0 && iIndex < bufferSpan.Length)
                            {
                                byte bValue = bufferSpan[iIndex];
                                bValue = Socket_Operation.GetStepByte(bValue, iStep * (sfi.ProgressionCount + 1));
                                bufferSpan[iIndex] = bValue;

                                sfi.IsProgressionDone = true;                            
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sfi.FName + " - " + ex.Message);
                    return false;
                }

                return true;
            }

            #endregion            

            #region//执行替换（高级滤镜）

            public static bool Replace_Advanced(Socket_FilterInfo sfi, int iMatch, Span<byte> bufferSpan)
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

                try
                {
                    if (!string.IsNullOrEmpty(sfi.FModify))
                    {
                        string[] slModify = sfi.FModify.Split(',');

                        foreach (string sModify in slModify)
                        {
                            if (!string.IsNullOrEmpty(sModify) && sModify.IndexOf("|") > 0)
                            {
                                string[] modifyParts = sModify.Split('|');

                                if (int.TryParse(modifyParts[0], out int iIndex))
                                {
                                    if (FStartFrom == Socket_Cache.Filter.FilterStartFrom.Position)
                                    {
                                        iIndex += iMatch;
                                    }

                                    if (iIndex >=0 && iIndex < bufferSpan.Length)
                                    {
                                        if (byte.TryParse(modifyParts[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte bValue))
                                        {
                                            bufferSpan[iIndex] = bValue;
                                        }                             
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
                            if (!string.IsNullOrEmpty(sProgression) && int.TryParse(sProgression, out int iIndex))
                            {
                                if (FStartFrom == Socket_Cache.Filter.FilterStartFrom.Position)
                                {
                                    iIndex += iMatch;
                                }

                                if (iIndex > -1 && iIndex < bufferSpan.Length)
                                {
                                    byte bValue = bufferSpan[iIndex];
                                    bValue = Socket_Operation.GetStepByte(bValue, iStep * (sfi.ProgressionCount + 1));
                                    bufferSpan[iIndex] = bValue;

                                    sfi.IsProgressionDone = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sfi.FName + " - " + ex.Message);
                    return false;
                }

                return true;
            }

            #endregion

            #region//执行换包

            public static byte[] ChangePacket_Filter(Socket_FilterInfo sfi)
            {
                if (string.IsNullOrEmpty(sfi.FModify))
                {
                    return Array.Empty<byte>();
                }

                try
                {
                    string[] slModify = sfi.FModify.Split(',');

                    int maxIndex = 0;

                    foreach (string sModify in slModify)
                    {
                        if (!string.IsNullOrEmpty(sModify) && sModify.IndexOf("|") > 0)
                        {
                            string[] modifyParts = sModify.Split('|');
                            if (int.TryParse(modifyParts[0], out int iIndex))
                            {
                                maxIndex = Math.Max(maxIndex, iIndex);
                            }
                        }
                    }

                    Span<byte> newBufferSpan = new Span<byte>(new byte[maxIndex + 1]);

                    foreach (string sModify in slModify)
                    {
                        if (!string.IsNullOrEmpty(sModify) && sModify.IndexOf("|") > 0)
                        {
                            string[] modifyParts = sModify.Split('|');
                            if (int.TryParse(modifyParts[0], out int iIndex) && byte.TryParse(modifyParts[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte bValue))
                            {
                                if (iIndex >= 0 && iIndex < newBufferSpan.Length)
                                {
                                    newBufferSpan[iIndex] = bValue;
                                }
                            }
                        }
                    }

                    return newBufferSpan.ToArray();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sfi.FName + " - " + ex.Message);
                    return Array.Empty<byte>();
                }
            }

            #endregion
        }

        #endregion

        #region//滤镜列表

        public static class FilterList
        {  
            public static string AESKey = string.Empty;
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

            public static Socket_Cache.Filter.Execute GetFilterListExecute_ByString(string sFLExecute)
            {
                Socket_Cache.Filter.Execute FLExecute = new Socket_Cache.Filter.Execute();

                try
                {
                    FLExecute = (Socket_Cache.Filter.Execute)Enum.Parse(typeof(Socket_Cache.Filter.Execute), sFLExecute);
                }
                catch (Exception ex)
                {
                    FLExecute = Socket_Cache.Filter.Execute.Priority;
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return FLExecute;
            }

            #endregion          

            #region//滤镜列表的列表操作

            public static int UpdateFilterList_ByListAction(Socket_Cache.System.ListAction listAction, int iFIndex)
            {
                int iReturn = -1;

                try
                {
                    int iFilterListCount = Socket_Cache.FilterList.lstFilter.Count;
                    Socket_FilterInfo sfi = Socket_Cache.FilterList.lstFilter[iFIndex];

                    switch (listAction)
                    {
                        case Socket_Cache.System.ListAction.Top:
                            if (iFIndex > 0)
                            {
                                Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                                Socket_Cache.FilterList.lstFilter.Insert(0, sfi);
                                iReturn = 0;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Up:
                            if (iFIndex > 0)
                            {
                                Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                                Socket_Cache.FilterList.lstFilter.Insert(iFIndex - 1, sfi);
                                iReturn = iFIndex - 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Down:
                            if (iFIndex < iFilterListCount - 1)
                            {
                                Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                                Socket_Cache.FilterList.lstFilter.Insert(iFIndex + 1, sfi);
                                iReturn = iFIndex + 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Bottom:
                            if (iFIndex < iFilterListCount - 1)
                            {
                                Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                                Socket_Cache.FilterList.lstFilter.Add(sfi);
                                iReturn = Socket_Cache.FilterList.lstFilter.Count - 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Copy:
                            Socket_Cache.Filter.CopyFilter_ByFilterIndex(iFIndex);
                            iReturn = Socket_Cache.FilterList.lstFilter.Count - 1;
                            break;

                        case Socket_Cache.System.ListAction.Export:
                            string sFName = Socket_Cache.FilterList.lstFilter[iFIndex].FName;
                            Socket_Cache.FilterList.SaveFilterList_Dialog(sFName, iFIndex);
                            iReturn = iFIndex;
                            break;

                        case Socket_Cache.System.ListAction.Delete:
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

            public static Socket_Cache.Filter.FilterAction DoFilterList(Int32 iSocket, Span<byte> bufferSpan, out byte[] bNewBuffer, Socket_Cache.SocketPacket.PacketType ptType, Socket_Cache.SocketPacket.SockAddr sAddr)
            {  
                bool bBreak = false;
                bNewBuffer = Array.Empty<byte>();
                Socket_Cache.Filter.FilterAction faReturn = Filter.FilterAction.None;            

                try
                {
                    foreach (Socket_FilterInfo sfi in Socket_Cache.FilterList.lstFilter)
                    {
                        if (Socket_Cache.Filter.CheckFilter_IsEffective(iSocket, bufferSpan, ptType, sAddr, sfi))
                        {
                            bool bDoFilter = false;
                            bool isMatch = false;
                            List<int> MatchIndex = new List<int>();
                            
                            if (sfi.FMode == Filter.FilterMode.Normal)
                            {
                                isMatch = Socket_Cache.Filter.CheckFilter_IsMatch_Normal(sfi, bufferSpan);
                            }
                            else if (sfi.FMode == Filter.FilterMode.Advanced)
                            {
                                MatchIndex = Socket_Cache.Filter.CheckFilter_IsMatch_Adcanced(sfi, bufferSpan);
                                isMatch = MatchIndex.Count > 0;
                            }
                            
                            if (isMatch)
                            {
                                faReturn = sfi.FAction;

                                switch (sfi.FAction)
                                {
                                    case Filter.FilterAction.Replace:

                                        sfi.IsProgressionDone = false;

                                        if (sfi.FMode == Filter.FilterMode.Normal)
                                        {
                                            bDoFilter = Socket_Cache.Filter.Replace_Normal(sfi, bufferSpan);
                                        }
                                        else if (sfi.FMode == Filter.FilterMode.Advanced)
                                        {
                                            foreach (int iIndex in MatchIndex)
                                            {
                                                Socket_Cache.Filter.Replace_Advanced(sfi, iIndex, bufferSpan);
                                            }

                                            bDoFilter = true;
                                        }

                                        if (sfi.IsProgressionDone)
                                        {
                                            sfi.ProgressionCount++;
                                        }

                                        if (bDoFilter)
                                        {
                                            bNewBuffer = bufferSpan.ToArray();

                                            if (Socket_Cache.Filter.FilterExecute == Socket_Cache.Filter.Execute.Priority)
                                            {
                                                bBreak = true;
                                            }
                                        }

                                        break;

                                    case Filter.FilterAction.Change:

                                        bNewBuffer = Socket_Cache.Filter.ChangePacket_Filter(sfi);

                                        if (bNewBuffer.Length > 0)
                                        {
                                            bDoFilter = true;

                                            if (Socket_Cache.Filter.FilterExecute == Socket_Cache.Filter.Execute.Priority)
                                            {
                                                bBreak = true;
                                            }
                                        }
                                        else
                                        {
                                            bNewBuffer = bufferSpan.ToArray();
                                        }

                                        break;

                                    case Filter.FilterAction.Intercept:
                                        bDoFilter = true;
                                        bBreak = true;
                                        break;

                                    case Filter.FilterAction.NoModify_Display:
                                        bDoFilter = true;
                                        bBreak = true;
                                        break;

                                    case Filter.FilterAction.NoModify_NoDisplay:
                                        bDoFilter = true;
                                        bBreak = true;
                                        break;
                                }
                            }
                            else
                            {
                                bDoFilter = false;
                            }

                            if (bDoFilter)
                            {
                                Socket_Cache.Filter.FilterExecute_CNT++;

                                if (sfi.IsExecute)
                                {
                                    switch (sfi.FEType)
                                    {
                                        case Socket_Cache.Filter.FilterExecuteType.Send:
                                            Socket_Cache.Send.DoSend(sfi.SID);
                                            break;

                                        case Socket_Cache.Filter.FilterExecuteType.Robot:
                                            Socket_Cache.Robot.DoRobot(sfi.RID);
                                            break;
                                    }
                                }

                                if (!Socket_Cache.SocketPacket.SpeedMode)
                                {
                                    string sFilterLog = string.Empty;

                                    if (MatchIndex.Count > 0)
                                    {
                                        sFilterLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_69), Socket_Cache.Filter.GetName_ByFilterAction(sfi.FAction), sfi.FName, Socket_Cache.SocketPacket.GetName_ByPacketType(ptType), bufferSpan.Length, MatchIndex.Count);
                                    }
                                    else
                                    {
                                        sFilterLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_51), Socket_Cache.Filter.GetName_ByFilterAction(sfi.FAction), sfi.FName, Socket_Cache.SocketPacket.GetName_ByPacketType(ptType), bufferSpan.Length);
                                    }

                                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sFilterLog);
                                }
                            }
                        }

                        if (bBreak)
                        {  
                            return faReturn;
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

            #region//保存滤镜列表到数据库

            public static void SaveFilterList_ToDB()
            {
                try
                {
                    Socket_Cache.DataBase.DeleteTable_Filter();

                    foreach (Socket_FilterInfo sfi in Socket_Cache.FilterList.lstFilter)
                    {
                        Socket_Cache.DataBase.InsertTable_Filter(sfi);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//从数据库加载滤镜列表

            public static void LoadFilterList_FromDB()
            {
                try
                {
                    DataTable dtFilter = Socket_Cache.DataBase.SelectTable_Filter();

                    foreach (DataRow dataRow in dtFilter.Rows) 
                    {
                        bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);
                        Guid FID = Guid.Parse(dataRow["GUID"].ToString());
                        string FName = dataRow["Name"].ToString();
                        bool AppointHeader = Convert.ToBoolean(dataRow["AppointHeader"]);
                        string FHeaderContent = dataRow["HeaderContent"].ToString();
                        bool AppointSocket = Convert.ToBoolean(dataRow["AppointSocket"]);
                        decimal FSocketContent = Convert.ToDecimal(dataRow["SocketContent"]);
                        bool AppointLength = Convert.ToBoolean(dataRow["AppointLength"]);
                        string FLengthContent = dataRow["LengthContent"].ToString();
                        bool AppointPort = Convert.ToBoolean(dataRow["AppointPort"]);
                        decimal FPortContent = Convert.ToDecimal(dataRow["PortContent"]);
                        Socket_Cache.Filter.FilterMode FilterMode = Socket_Cache.Filter.GetFilterMode_ByString(dataRow["Mode"].ToString());
                        Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.Filter.GetFilterAction_ByString(dataRow["Action"].ToString());
                        bool IsExecute = Convert.ToBoolean(dataRow["IsExecute"]);
                        Socket_Cache.Filter.FilterExecuteType FilterExecuteType = Socket_Cache.Filter.GetFilterExecuteType_ByString(dataRow["ExecuteType"].ToString());
                        Guid SID = Guid.Parse(dataRow["Send_GUID"].ToString());
                        Guid RID = Guid.Parse(dataRow["Robot_GUID"].ToString());
                        Socket_Cache.Filter.FilterFunction FilterFunction = Socket_Cache.Filter.GetFilterFunction_ByString(dataRow["Function"].ToString());
                        Socket_Cache.Filter.FilterStartFrom FilterStartFrom = Socket_Cache.Filter.GetFilterStartFrom_ByString(dataRow["StartFrom"].ToString());
                        bool IsProgressionDone = false;
                        decimal FProgressionStep = Convert.ToDecimal(dataRow["ProgressionStep"]);
                        string FProgressionPosition = dataRow["ProgressionPosition"].ToString();
                        int ProgressionCount = 0;
                        string FSearch = dataRow["Search"].ToString();
                        string FModify = dataRow["Modify"].ToString();

                        Socket_Cache.Filter.AddFilter(
                            IsEnable,
                            FID,
                            FName,
                            AppointHeader,
                            FHeaderContent,
                            AppointSocket,
                            FSocketContent,
                            AppointLength,
                            FLengthContent,
                            AppointPort,
                            FPortContent,
                            FilterMode,
                            FilterAction,
                            IsExecute,
                            FilterExecuteType,
                            SID,
                            RID,
                            FilterFunction,
                            FilterStartFrom,
                            IsProgressionDone,
                            FProgressionStep,
                            FProgressionPosition,
                            ProgressionCount,
                            FSearch,
                            FModify);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//保存滤镜列表到文件（对话框）

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
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.FilterList_Export);                            
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

            private static void SaveFilterList(string FilePath, int FilterIndex, bool DoEncrypt)
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
                            string sFAppointPort = Socket_Cache.FilterList.lstFilter[i].AppointPort.ToString();
                            string sFPortContent = Socket_Cache.FilterList.lstFilter[i].PortContent.ToString();
                            string sFMode = ((int)Socket_Cache.FilterList.lstFilter[i].FMode).ToString();
                            string sFAction = ((int)Socket_Cache.FilterList.lstFilter[i].FAction).ToString();
                            string sIsExecute = Socket_Cache.FilterList.lstFilter[i].IsExecute.ToString();
                            string sFEType = ((int)Socket_Cache.FilterList.lstFilter[i].FEType).ToString();
                            string sSID = Socket_Cache.FilterList.lstFilter[i].SID.ToString().ToUpper();
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
                                new XElement("AppointPort", sFAppointPort),
                                new XElement("PortContent", sFPortContent),
                                new XElement("Mode", sFMode),
                                new XElement("Action", sFAction),
                                new XElement("IsExecute", sIsExecute),
                                new XElement("ExecuteType", sFEType),
                                new XElement("SendID", sSID),
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

            #region//从文件加载滤镜列表（对话框）

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

            private static void LoadFilterList(string FilePath, bool LoadFromUser)
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
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.FilterList_Import);
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

                        Guid gFID = Guid.NewGuid();

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

                        string sFLengthContent = string.Empty;
                        if (xeFilter.Element("LengthContent") != null)
                        {
                            sFLengthContent = xeFilter.Element("LengthContent").Value;
                        }

                        bool bAppointPort = false;
                        if (xeFilter.Element("AppointPort") != null)
                        {
                            bAppointPort = bool.Parse(xeFilter.Element("AppointPort").Value);
                        }

                        decimal dFPortContent = 1;
                        if (xeFilter.Element("PortContent") != null)
                        {
                            dFPortContent = decimal.Parse(xeFilter.Element("PortContent").Value);
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

                        Socket_Cache.Filter.FilterExecuteType FilterExecuteType = new Socket_Cache.Filter.FilterExecuteType();
                        if (xeFilter.Element("ExecuteType") != null)
                        {
                            FilterExecuteType = Socket_Cache.Filter.GetFilterExecuteType_ByString(xeFilter.Element("ExecuteType").Value);
                        }

                        Guid gSID = Guid.Empty;
                        if (xeFilter.Element("SendID") != null)
                        {
                            gSID = Guid.Parse(xeFilter.Element("SendID").Value);
                        }
                        else
                        {
                            gSID = Guid.Empty;
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
                            sFLengthContent,
                            bAppointPort,
                            dFPortContent,
                            FilterMode, 
                            FilterAction,
                            bIsExecute,
                            FilterExecuteType,
                            gSID,
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

            public static int UpdateRobotList_ByListAction(Socket_Cache.System.ListAction listAction, int iRIndex)
            {
                int iReturn = -1;

                try
                {
                    int iRobotListCount = Socket_Cache.RobotList.lstRobot.Count;
                    Socket_RobotInfo sri = Socket_Cache.RobotList.lstRobot[iRIndex];

                    switch (listAction)
                    {
                        case Socket_Cache.System.ListAction.Top:
                            if (iRIndex > 0)
                            {
                                Socket_Cache.RobotList.lstRobot.RemoveAt(iRIndex);
                                Socket_Cache.RobotList.lstRobot.Insert(0, sri);
                                iReturn = 0;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Up:
                            if (iRIndex > 0)
                            {
                                Socket_Cache.RobotList.lstRobot.RemoveAt(iRIndex);
                                Socket_Cache.RobotList.lstRobot.Insert(iRIndex - 1, sri);
                                iReturn = iRIndex - 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Down:
                            if (iRIndex < iRobotListCount - 1)
                            {
                                Socket_Cache.RobotList.lstRobot.RemoveAt(iRIndex);
                                Socket_Cache.RobotList.lstRobot.Insert(iRIndex + 1, sri);
                                iReturn = iRIndex + 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Bottom:
                            if (iRIndex < iRobotListCount - 1)
                            {
                                Socket_Cache.RobotList.lstRobot.RemoveAt(iRIndex);
                                Socket_Cache.RobotList.lstRobot.Add(sri);
                                iReturn = Socket_Cache.RobotList.lstRobot.Count - 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Copy:
                            Socket_Cache.Robot.CopyRobot_ByRobotIndex(iRIndex);
                            iReturn = Socket_Cache.RobotList.lstRobot.Count - 1;
                            break;

                        case Socket_Cache.System.ListAction.Export:
                            string sRName = Socket_Cache.RobotList.lstRobot[iRIndex].RName;
                            Socket_Cache.RobotList.SaveRobotList_Dialog(sRName, iRIndex);
                            iReturn = iRIndex;
                            break;

                        case Socket_Cache.System.ListAction.Delete:
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

            #region//保存机器人列表到数据库

            public static void SaveRobotList_ToDB()
            {
                try
                {
                    Socket_Cache.DataBase.DeleteTable_Robot();

                    foreach (Socket_RobotInfo sri in Socket_Cache.RobotList.lstRobot)
                    {
                        Socket_Cache.DataBase.InsertTable_Robot(sri);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//从数据库加载机器人列表

            public static void LoadRobotList_FromDB()
            {
                try
                {
                    DataTable dtRobot = Socket_Cache.DataBase.SelectTable_Robot();

                    foreach (DataRow dataRow in dtRobot.Rows)
                    {
                        Guid RID = Guid.Parse(dataRow["GUID"].ToString());
                        bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);
                        string RName = dataRow["Name"].ToString();


                        DataTable RInstruction = Socket_Cache.Robot.InitInstructions();
                        DataTable dtInstruction = Socket_Cache.DataBase.SelectTable_RobotInstruction(RID);

                        foreach (DataRow row in dtInstruction.Rows)
                        {
                            DataRow dr = RInstruction.NewRow();
                            dr[0] = Socket_Cache.Robot.GetInstructionType_ByString(row["Type"].ToString());
                            dr[1] = row["Content"].ToString();
                            RInstruction.Rows.Add(dr);
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

            #region//从文件加载机器人列表（对话框）

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

            private static void LoadRobotList(string FilePath, bool LoadFromUser)
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
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.RobotList_Import);
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

                        Guid RID = Guid.NewGuid();                        

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

            #region//保存机器人列表到文件（对话框）

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
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.RobotList_Export);
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

            public static int UpdateSendCollection_ByListAction(DataTable dtSendCollection, Socket_Cache.System.ListAction listAction, int iSIndex)
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
                        case Socket_Cache.System.ListAction.Top:
                            if (iSIndex > 0 && iSIndex < iSendCollectionCount)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                                dtSendCollection.Rows.InsertAt(dr, 0);
                                iReturn = 0;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Up:
                            if (iSIndex > 0 && iSIndex < iSendCollectionCount)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                                dtSendCollection.Rows.InsertAt(dr, iSIndex - 1);
                                iReturn = iSIndex - 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Down:
                            if (iSIndex > -1 && iSIndex < iSendCollectionCount - 1)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                                dtSendCollection.Rows.InsertAt(dr, iSIndex + 1);
                                iReturn = iSIndex + 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Bottom:
                            if (iSIndex > -1 && iSIndex < iSendCollectionCount - 1)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                                dtSendCollection.Rows.Add(dr);
                                iReturn = dtSendCollection.Rows.Count - 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Delete:
                            if (iSIndex > -1 && iSIndex < iSendCollectionCount)
                            {
                                dtSendCollection.Rows.RemoveAt(iSIndex);
                            }                            
                            break;

                        case Socket_Cache.System.ListAction.Export:
                            if (iSendCollectionCount > 0)
                            {
                                Socket_Cache.Send.SaveSendCollection_Dialog(string.Empty, dtSendCollection);
                            }                            
                            break;

                        case Socket_Cache.System.ListAction.Import:
                            Socket_Cache.Send.LoadSendCollection_Dialog(dtSendCollection);
                            break;

                        case Socket_Cache.System.ListAction.CleanUp:
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
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.SendList_Export);
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
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.SendCollection_Import);
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

            public static int UpdateSendList_ByListAction(Socket_Cache.System.ListAction listAction, int iSIndex)
            {
                int iReturn = -1;

                try
                {
                    int iSendListCount = Socket_Cache.SendList.lstSend.Count;
                    Socket_SendInfo ssi = Socket_Cache.SendList.lstSend[iSIndex];

                    switch (listAction)
                    {
                        case Socket_Cache.System.ListAction.Top:
                            if (iSIndex > 0)
                            {
                                Socket_Cache.SendList.lstSend.RemoveAt(iSIndex);
                                Socket_Cache.SendList.lstSend.Insert(0, ssi);
                                iReturn = 0;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Up:
                            if (iSIndex > 0)
                            {
                                Socket_Cache.SendList.lstSend.RemoveAt(iSIndex);
                                Socket_Cache.SendList.lstSend.Insert(iSIndex - 1, ssi);
                                iReturn = iSIndex - 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Down:
                            if (iSIndex < iSendListCount - 1)
                            {
                                Socket_Cache.SendList.lstSend.RemoveAt(iSIndex);
                                Socket_Cache.SendList.lstSend.Insert(iSIndex + 1, ssi);
                                iReturn = iSIndex + 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Bottom:
                            if (iSIndex < iSendListCount - 1)
                            {
                                Socket_Cache.SendList.lstSend.RemoveAt(iSIndex);
                                Socket_Cache.SendList.lstSend.Add(ssi);
                                iReturn = Socket_Cache.SendList.lstSend.Count - 1;
                            }
                            break;

                        case Socket_Cache.System.ListAction.Copy:
                            Socket_Cache.Send.CopySend_BySendIndex(iSIndex);
                            iReturn = Socket_Cache.SendList.lstSend.Count - 1;
                            break;

                        case Socket_Cache.System.ListAction.Export:
                            string sSName = Socket_Cache.SendList.lstSend[iSIndex].SName;
                            Socket_Cache.SendList.SaveSendList_Dialog(sSName, iSIndex);
                            iReturn = iSIndex;
                            break;

                        case Socket_Cache.System.ListAction.Delete:
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

            #region//保存发送列表到数据库

            public static void SaveSendList_ToDB()
            {
                try
                {
                    Socket_Cache.DataBase.DeleteTable_Send();

                    foreach (Socket_SendInfo ssi in Socket_Cache.SendList.lstSend)
                    {
                        Socket_Cache.DataBase.InsertTable_Send(ssi);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//从数据库加载滤镜列表

            public static void LoadSendList_FromDB()
            {
                try
                {
                    DataTable dtSend = Socket_Cache.DataBase.SelectTable_Send();

                    foreach (DataRow dataRow in dtSend.Rows)
                    {
                        Guid SID = Guid.Parse(dataRow["GUID"].ToString());
                        bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);
                        string SName = dataRow["Name"].ToString();
                        bool SSystemSocket = Convert.ToBoolean(dataRow["SystemSocket"]);
                        int SLoopCNT = Convert.ToInt32(dataRow["LoopCNT"]);
                        int SLoopINT = Convert.ToInt32(dataRow["LoopINT"]);
                        string SNotes = dataRow["Notes"].ToString();                        

                        DataTable SCollection = Socket_Cache.Send.InitSendCollection();
                        DataTable dtCollection = Socket_Cache.DataBase.SelectTable_SendCollection(SID);

                        foreach (DataRow row in dtCollection.Rows)
                        {
                            int Socket = Convert.ToInt32(row["Socket"]);
                            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.GetPacketType_ByString(row["Type"].ToString());
                            string IPTo = row["IPTo"].ToString();
                            byte[] Buffer = (byte[])row["Buffer"];
                            Socket_Cache.Send.AddSendCollection(SCollection, Socket, ptType, IPTo, Buffer);
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

            #region//保存发送列表到文件（对话框）

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
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.SendList_Export);
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

            private static void SaveSendList(string FilePath, int SendPacketIndex, bool DoEncrypt)
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

            #region//从文件加载发送列表（对话框）

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

            private static void LoadSendList(string FilePath, bool LoadFromUser)
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
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.SendList_Import);
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

                        Guid SID = Guid.NewGuid();

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

        #region//数据库

        public static class DataBase
        {
            private static string dbPath = @"C:\WPE64Cache";
            private static string dbName = Socket_Operation.AssemblyVersion + ".db";
            private static string conStr = string.Format("Data Source={0}\\{1};Version=3;", dbPath, dbName);

            #region//初始化

            public static void InitDB()
            {
                Socket_Cache.DataBase.InitdbPath();

                Socket_Cache.DataBase.CreateTable_SystemConfig();
                Socket_Cache.DataBase.CreateTable_RunConfig();
                Socket_Cache.DataBase.CreateTable_Filter();
                Socket_Cache.DataBase.CreateTable_Send();
                Socket_Cache.DataBase.CreateTable_Robot();
                Socket_Cache.DataBase.CreateTable_ProxyAccount();
            }

            private static void InitdbPath()
            {
                try
                {
                    if (!Directory.Exists(dbPath))
                    {
                        Directory.CreateDirectory(dbPath);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }                                
            }

            #endregion

            #region//系统配置表

            private static bool CreateTable_SystemConfig()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS SystemConfig (";
                        sql += "SystemConfig_DefaultLanguage TEXT,";//系统设置 - 默认语言
                        sql += "SystemConfig_LastInjection TEXT,";//系统设置 - 上次注入进程名称
                        sql += "SystemConfig_StartMode INTEGER DEFAULT 0,";//系统设置 - 启动模式
                        sql += "SystemConfig_Remote_IsEnable BOOLEAN DEFAULT 0,";//系统设置 - 启用远程管理
                        sql += "SystemConfig_Remote_UserName TEXT,";//系统设置 - 远程管理账号
                        sql += "SystemConfig_Remote_PassWord TEXT,";//系统设置 - 远程管理密码
                        sql += "SystemConfig_Remote_Port INTEGER,";//系统设置 - 远程管理端口                    
                        sql += "SystemConfig_Remote_URL TEXT";//系统设置 - 远程管理网址
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_SystemConfig()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM SystemConfig;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_SystemConfig()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM SystemConfig;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void InsertTable_SystemConfig()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "INSERT INTO SystemConfig (";
                        sql += "SystemConfig_DefaultLanguage,";
                        sql += "SystemConfig_LastInjection,";
                        sql += "SystemConfig_StartMode,";
                        sql += "SystemConfig_Remote_IsEnable,";
                        sql += "SystemConfig_Remote_UserName,";
                        sql += "SystemConfig_Remote_PassWord,";
                        sql += "SystemConfig_Remote_Port,";
                        sql += "SystemConfig_Remote_URL";
                        sql += ") VALUES (";
                        sql += "@SystemConfig_DefaultLanguage,";
                        sql += "@SystemConfig_LastInjection,";
                        sql += "@SystemConfig_StartMode,";
                        sql += "@SystemConfig_Remote_IsEnable,";
                        sql += "@SystemConfig_Remote_UserName,";
                        sql += "@SystemConfig_Remote_PassWord,";
                        sql += "@SystemConfig_Remote_Port,";
                        sql += "@SystemConfig_Remote_URL";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@SystemConfig_DefaultLanguage", Socket_Cache.System.DefaultLanguage);
                            cmd.Parameters.AddWithValue("@SystemConfig_LastInjection", Socket_Cache.System.LastInjection);
                            cmd.Parameters.AddWithValue("@SystemConfig_StartMode", Socket_Cache.System.StartMode);
                            cmd.Parameters.AddWithValue("@SystemConfig_Remote_IsEnable", Socket_Cache.System.IsRemote);
                            cmd.Parameters.AddWithValue("@SystemConfig_Remote_UserName", Socket_Cache.System.Remote_UserName);
                            cmd.Parameters.AddWithValue("@SystemConfig_Remote_PassWord", Socket_Cache.System.Remote_PassWord);
                            cmd.Parameters.AddWithValue("@SystemConfig_Remote_Port", Socket_Cache.System.Remote_Port);
                            cmd.Parameters.AddWithValue("@SystemConfig_Remote_URL", Socket_Cache.System.Remote_URL);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void UpdateTable_SystemConfig_LastInjection()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "UPDATE SystemConfig SET SystemConfig_LastInjection = @LastInjection;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@LastInjection", Socket_Cache.System.LastInjection);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }            

            #endregion

            #region//运行配置表

            private static bool CreateTable_RunConfig()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS RunConfig (";
                        
                        sql += "ProxyConfig_ProxyIP_Auto BOOLEAN DEFAULT 1,";//代理模式 - 自动检测IP
                        sql += "ProxyConfig_EnableSOCKS5 BOOLEAN DEFAULT 1,";//代理模式 - 启用SOCKS5代理
                        sql += "ProxyConfig_ProxyPort INTEGER DEFAULT 1080,";//代理模式 - 代理端口
                        sql += "ProxyConfig_EnableAuth BOOLEAN DEFAULT 1,";//代理模式 - 启用代理认证
                        sql += "ProxyConfig_ProxyList_NoRecord BOOLEAN DEFAULT 1,";//代理模式 - 不记录数据
                        sql += "ProxyConfig_ClientList_DelClosed BOOLEAN DEFAULT 1,";//代理模式 - 清理关闭的链接
                        sql += "ProxyConfig_LogList_AutoRoll BOOLEAN DEFAULT 0,";//代理模式 - 日志列表自动滚动
                        sql += "ProxyConfig_LogList_AutoClear BOOLEAN DEFAULT 1,";//代理模式 - 日志列表自动清理
                        sql += "ProxyConfig_LogList_AutoClear_Value INTEGER DEFAULT 5000,";//代理模式 - 日志列表自动清理数值
                        sql += "ProxyConfig_EXTProxy_EnableHttp BOOLEAN DEFAULT 0,";//代理模式 - 启用外部HTTP代理
                        sql += "ProxyConfig_EXTProxy_HttpIP TEXT,";//代理模式 - 外部HTTP代理IP
                        sql += "ProxyConfig_EXTProxy_HttpPort INTEGER DEFAULT 8889,";//代理模式 - 外部HTTP代理端口
                        sql += "ProxyConfig_EXTProxy_AppointHttpPort TEXT,";//代理模式 - 外部HTTP代理指定端口
                        sql += "ProxyConfig_EXTProxy_EnableHttps BOOLEAN DEFAULT 0,";//代理模式 - 启用外部HTTP(S)代理
                        sql += "ProxyConfig_EXTProxy_HttpsIP TEXT,";//代理模式 - 外部HTTP(S)代理IP
                        sql += "ProxyConfig_EXTProxy_HttpsPort INTEGER DEFAULT 8889,";//代理模式 - 外部HTTP(S)代理端口
                        sql += "ProxyConfig_EXTProxy_AppointHttpsPort TEXT,";//代理模式 - 外部HTTP(S)代理指定端口
                        sql += "ProxyConfig_SpeedMode BOOLEAN DEFAULT 0,";//代理模式 - 极速模式

                        sql += "InjectionConfig_CheckNotShow BOOLEAN DEFAULT 1,";//注入模式 - 过滤设置不显示
                        sql += "InjectionConfig_CheckSocket BOOLEAN DEFAULT 0,";//注入模式 - 过滤套接字
                        sql += "InjectionConfig_CheckSocket_Value TEXT,";//注入模式 - 过滤套接字内容
                        sql += "InjectionConfig_CheckIP BOOLEAN DEFAULT 0,";//注入模式 - 过滤IP
                        sql += "InjectionConfig_CheckIP_Value TEXT,";//注入模式 - 过滤IP内容
                        sql += "InjectionConfig_CheckPort BOOLEAN DEFAULT 0,";//注入模式 - 过滤端口
                        sql += "InjectionConfig_CheckPort_Value TEXT,";//注入模式 - 过滤端口内容
                        sql += "InjectionConfig_CheckHead BOOLEAN DEFAULT 0,";//注入模式 - 过滤包头
                        sql += "InjectionConfig_CheckHead_Value TEXT,";//注入模式 - 过滤包头内容
                        sql += "InjectionConfig_CheckData BOOLEAN DEFAULT 0,";//注入模式 - 过滤数据
                        sql += "InjectionConfig_CheckData_Value TEXT,";//注入模式 - 过滤数据内容
                        sql += "InjectionConfig_CheckSize BOOLEAN DEFAULT 0,";//注入模式 - 过滤长度
                        sql += "InjectionConfig_CheckLength_Value TEXT,";//注入模式 - 过滤长度内容
                        sql += "InjectionConfig_HookWS1_Send BOOLEAN DEFAULT 1,";//注入模式 - 发送1.1
                        sql += "InjectionConfig_HookWS1_SendTo BOOLEAN DEFAULT 1,";//注入模式 - 发送到1.1
                        sql += "InjectionConfig_HookWS1_Recv BOOLEAN DEFAULT 1,";//注入模式 - 接收1.1
                        sql += "InjectionConfig_HookWS1_RecvFrom BOOLEAN DEFAULT 1,";//注入模式 - 接收自1.1
                        sql += "InjectionConfig_HookWS2_Send BOOLEAN DEFAULT 1,";//注入模式 - 发送
                        sql += "InjectionConfig_HookWS2_SendTo BOOLEAN DEFAULT 1,";//注入模式 - 发送到
                        sql += "InjectionConfig_HookWS2_Recv BOOLEAN DEFAULT 1,";//注入模式 - 接收
                        sql += "InjectionConfig_HookWS2_RecvFrom BOOLEAN DEFAULT 1,";//注入模式 - 接收自
                        sql += "InjectionConfig_HookWSA_Send BOOLEAN DEFAULT 1,";//注入模式 - WSA 发送
                        sql += "InjectionConfig_HookWSA_SendTo BOOLEAN DEFAULT 1,";//注入模式 - WSA 发送到
                        sql += "InjectionConfig_HookWSA_Recv BOOLEAN DEFAULT 1,";//注入模式 - WSA 接收
                        sql += "InjectionConfig_HookWSA_RecvFrom BOOLEAN DEFAULT 1,";//注入模式 - WSA 接收自
                        sql += "InjectionConfig_SocketList_AutoRoll BOOLEAN DEFAULT 0,";//注入模式 - 封包列表自动滚动
                        sql += "InjectionConfig_SocketList_AutoClear BOOLEAN DEFAULT 1,";//注入模式 - 封包列表自动清理
                        sql += "InjectionConfig_SocketList_AutoClear_Value INTEGER DEFAULT 5000,";//注入模式 - 封包列表自动清理数值
                        sql += "InjectionConfig_LogList_AutoRoll BOOLEAN DEFAULT 0,";//注入模式 - 日志列表自动滚动
                        sql += "InjectionConfig_LogList_AutoClear BOOLEAN DEFAULT 1,";//注入模式 - 日志列表自动清理
                        sql += "InjectionConfig_LogList_AutoClear_Value INTEGER DEFAULT 5000,";//注入模式 - 日志列表自动清理数值
                        sql += "InjectionConfig_SpeedMode BOOLEAN DEFAULT 0,";//注入模式 - 极速模式
                        sql += "InjectionConfig_FilterExecute INTEGER DEFAULT 1";//注入模式 - 滤镜执行模式
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_RunConfig()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM RunConfig;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_RunConfig()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM RunConfig;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void InsertTable_RunConfig()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "INSERT INTO RunConfig (";                       
                        sql += "ProxyConfig_ProxyIP_Auto,";
                        sql += "ProxyConfig_EnableSOCKS5,";
                        sql += "ProxyConfig_ProxyPort,";
                        sql += "ProxyConfig_EnableAuth,";
                        sql += "ProxyConfig_ProxyList_NoRecord,";
                        sql += "ProxyConfig_ClientList_DelClosed,";
                        sql += "ProxyConfig_LogList_AutoRoll,";
                        sql += "ProxyConfig_LogList_AutoClear,";
                        sql += "ProxyConfig_LogList_AutoClear_Value,";
                        sql += "ProxyConfig_EXTProxy_EnableHttp,";
                        sql += "ProxyConfig_EXTProxy_HttpIP,";
                        sql += "ProxyConfig_EXTProxy_HttpPort,";
                        sql += "ProxyConfig_EXTProxy_AppointHttpPort,";
                        sql += "ProxyConfig_EXTProxy_EnableHttps,";
                        sql += "ProxyConfig_EXTProxy_HttpsIP,";
                        sql += "ProxyConfig_EXTProxy_HttpsPort,";
                        sql += "ProxyConfig_EXTProxy_AppointHttpsPort,";
                        sql += "ProxyConfig_SpeedMode,";
                        sql += "InjectionConfig_CheckNotShow,";
                        sql += "InjectionConfig_CheckSocket,";
                        sql += "InjectionConfig_CheckSocket_Value,";
                        sql += "InjectionConfig_CheckIP,";
                        sql += "InjectionConfig_CheckIP_Value,";
                        sql += "InjectionConfig_CheckPort,";
                        sql += "InjectionConfig_CheckPort_Value,";
                        sql += "InjectionConfig_CheckHead,";
                        sql += "InjectionConfig_CheckHead_Value,";
                        sql += "InjectionConfig_CheckData,";
                        sql += "InjectionConfig_CheckData_Value,";
                        sql += "InjectionConfig_CheckSize,";
                        sql += "InjectionConfig_CheckLength_Value,";
                        sql += "InjectionConfig_HookWS1_Send,";
                        sql += "InjectionConfig_HookWS1_SendTo,";
                        sql += "InjectionConfig_HookWS1_Recv,";
                        sql += "InjectionConfig_HookWS1_RecvFrom,";
                        sql += "InjectionConfig_HookWS2_Send,";
                        sql += "InjectionConfig_HookWS2_SendTo,";
                        sql += "InjectionConfig_HookWS2_Recv,";
                        sql += "InjectionConfig_HookWS2_RecvFrom,";
                        sql += "InjectionConfig_HookWSA_Send,";
                        sql += "InjectionConfig_HookWSA_SendTo,";
                        sql += "InjectionConfig_HookWSA_Recv,";
                        sql += "InjectionConfig_HookWSA_RecvFrom,";
                        sql += "InjectionConfig_SocketList_AutoRoll,";
                        sql += "InjectionConfig_SocketList_AutoClear,";
                        sql += "InjectionConfig_SocketList_AutoClear_Value,";
                        sql += "InjectionConfig_LogList_AutoRoll,";
                        sql += "InjectionConfig_LogList_AutoClear,";
                        sql += "InjectionConfig_LogList_AutoClear_Value,";
                        sql += "InjectionConfig_SpeedMode,";
                        sql += "InjectionConfig_FilterExecute";
                        sql += ") VALUES (";                  
                        sql += "@ProxyConfig_ProxyIP_Auto,";
                        sql += "@ProxyConfig_EnableSOCKS5,";
                        sql += "@ProxyConfig_ProxyPort,";
                        sql += "@ProxyConfig_EnableAuth,";
                        sql += "@ProxyConfig_ProxyList_NoRecord,";
                        sql += "@ProxyConfig_ClientList_DelClosed,";
                        sql += "@ProxyConfig_LogList_AutoRoll,";
                        sql += "@ProxyConfig_LogList_AutoClear,";
                        sql += "@ProxyConfig_LogList_AutoClear_Value,";
                        sql += "@ProxyConfig_EXTProxy_EnableHttp,";
                        sql += "@ProxyConfig_EXTProxy_HttpIP,";
                        sql += "@ProxyConfig_EXTProxy_HttpPort,";
                        sql += "@ProxyConfig_EXTProxy_AppointHttpPort,";
                        sql += "@ProxyConfig_EXTProxy_EnableHttps,";
                        sql += "@ProxyConfig_EXTProxy_HttpsIP,";
                        sql += "@ProxyConfig_EXTProxy_HttpsPort,";
                        sql += "@ProxyConfig_EXTProxy_AppointHttpsPort,";
                        sql += "@ProxyConfig_SpeedMode,";
                        sql += "@InjectionConfig_CheckNotShow,";
                        sql += "@InjectionConfig_CheckSocket,";
                        sql += "@InjectionConfig_CheckSocket_Value,";
                        sql += "@InjectionConfig_CheckIP,";
                        sql += "@InjectionConfig_CheckIP_Value,";
                        sql += "@InjectionConfig_CheckPort,";
                        sql += "@InjectionConfig_CheckPort_Value,";
                        sql += "@InjectionConfig_CheckHead,";
                        sql += "@InjectionConfig_CheckHead_Value,";
                        sql += "@InjectionConfig_CheckData,";
                        sql += "@InjectionConfig_CheckData_Value,";
                        sql += "@InjectionConfig_CheckSize,";
                        sql += "@InjectionConfig_CheckLength_Value,";
                        sql += "@InjectionConfig_HookWS1_Send,";
                        sql += "@InjectionConfig_HookWS1_SendTo,";
                        sql += "@InjectionConfig_HookWS1_Recv,";
                        sql += "@InjectionConfig_HookWS1_RecvFrom,";
                        sql += "@InjectionConfig_HookWS2_Send,";
                        sql += "@InjectionConfig_HookWS2_SendTo,";
                        sql += "@InjectionConfig_HookWS2_Recv,";
                        sql += "@InjectionConfig_HookWS2_RecvFrom,";
                        sql += "@InjectionConfig_HookWSA_Send,";
                        sql += "@InjectionConfig_HookWSA_SendTo,";
                        sql += "@InjectionConfig_HookWSA_Recv,";
                        sql += "@InjectionConfig_HookWSA_RecvFrom,";
                        sql += "@InjectionConfig_SocketList_AutoRoll,";
                        sql += "@InjectionConfig_SocketList_AutoClear,";
                        sql += "@InjectionConfig_SocketList_AutoClear_Value,";
                        sql += "@InjectionConfig_LogList_AutoRoll,";
                        sql += "@InjectionConfig_LogList_AutoClear,";
                        sql += "@InjectionConfig_LogList_AutoClear_Value,";
                        sql += "@InjectionConfig_SpeedMode,";
                        sql += "@InjectionConfig_FilterExecute";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {                           
                            cmd.Parameters.AddWithValue("@ProxyConfig_ProxyIP_Auto", Socket_Cache.SocketProxy.ProxyIP_Auto);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EnableSOCKS5", Socket_Cache.SocketProxy.Enable_SOCKS5);
                            cmd.Parameters.AddWithValue("@ProxyConfig_ProxyPort", Socket_Cache.SocketProxy.ProxyPort);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EnableAuth", Socket_Cache.SocketProxy.Enable_Auth);
                            cmd.Parameters.AddWithValue("@ProxyConfig_ProxyList_NoRecord", Socket_Cache.SocketProxy.NoRecord);
                            cmd.Parameters.AddWithValue("@ProxyConfig_ClientList_DelClosed", Socket_Cache.SocketProxy.DelClosed);
                            cmd.Parameters.AddWithValue("@ProxyConfig_LogList_AutoRoll", Socket_Cache.LogList.Proxy_AutoRoll);
                            cmd.Parameters.AddWithValue("@ProxyConfig_LogList_AutoClear", Socket_Cache.LogList.Proxy_AutoClear);
                            cmd.Parameters.AddWithValue("@ProxyConfig_LogList_AutoClear_Value", Socket_Cache.LogList.Proxy_AutoClear_Value);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EXTProxy_EnableHttp", Socket_Cache.SocketProxy.Enable_EXTHttp);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EXTProxy_HttpIP", Socket_Cache.SocketProxy.EXTHttpIP);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EXTProxy_HttpPort", Socket_Cache.SocketProxy.EXTHttpPort);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EXTProxy_AppointHttpPort", Socket_Cache.SocketProxy.AppointHttpPort);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EXTProxy_EnableHttps", Socket_Cache.SocketProxy.Enable_EXTHttps);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EXTProxy_HttpsIP", Socket_Cache.SocketProxy.EXTHttpsIP);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EXTProxy_HttpsPort", Socket_Cache.SocketProxy.EXTHttpsPort);
                            cmd.Parameters.AddWithValue("@ProxyConfig_EXTProxy_AppointHttpsPort", Socket_Cache.SocketProxy.AppointHttpsPort);
                            cmd.Parameters.AddWithValue("@ProxyConfig_SpeedMode", Socket_Cache.SocketProxy.SpeedMode);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckNotShow", Socket_Cache.SocketPacket.CheckNotShow);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckSocket", Socket_Cache.SocketPacket.CheckSocket);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckSocket_Value", Socket_Cache.SocketPacket.CheckSocket_Value);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckIP", Socket_Cache.SocketPacket.CheckIP);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckIP_Value", Socket_Cache.SocketPacket.CheckIP_Value);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckPort", Socket_Cache.SocketPacket.CheckPort);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckPort_Value", Socket_Cache.SocketPacket.CheckPort_Value);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckHead", Socket_Cache.SocketPacket.CheckHead);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckHead_Value", Socket_Cache.SocketPacket.CheckHead_Value);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckData", Socket_Cache.SocketPacket.CheckData);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckData_Value", Socket_Cache.SocketPacket.CheckData_Value);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckSize", Socket_Cache.SocketPacket.CheckSize);
                            cmd.Parameters.AddWithValue("@InjectionConfig_CheckLength_Value", Socket_Cache.SocketPacket.CheckLength_Value);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWS1_Send", Socket_Cache.SocketPacket.HookWS1_Send);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWS1_SendTo", Socket_Cache.SocketPacket.HookWS1_SendTo);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWS1_Recv", Socket_Cache.SocketPacket.HookWS1_Recv);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWS1_RecvFrom", Socket_Cache.SocketPacket.HookWS1_RecvFrom);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWS2_Send", Socket_Cache.SocketPacket.HookWS2_Send);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWS2_SendTo", Socket_Cache.SocketPacket.HookWS2_SendTo);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWS2_Recv", Socket_Cache.SocketPacket.HookWS2_Recv);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWS2_RecvFrom", Socket_Cache.SocketPacket.HookWS2_RecvFrom);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWSA_Send", Socket_Cache.SocketPacket.HookWSA_Send);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWSA_SendTo", Socket_Cache.SocketPacket.HookWSA_SendTo);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWSA_Recv", Socket_Cache.SocketPacket.HookWSA_Recv);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HookWSA_RecvFrom", Socket_Cache.SocketPacket.HookWSA_RecvFrom);
                            cmd.Parameters.AddWithValue("@InjectionConfig_SocketList_AutoRoll", Socket_Cache.SocketList.AutoRoll);
                            cmd.Parameters.AddWithValue("@InjectionConfig_SocketList_AutoClear", Socket_Cache.SocketList.AutoClear);
                            cmd.Parameters.AddWithValue("@InjectionConfig_SocketList_AutoClear_Value", Socket_Cache.SocketList.AutoClear_Value);
                            cmd.Parameters.AddWithValue("@InjectionConfig_LogList_AutoRoll", Socket_Cache.LogList.Socket_AutoRoll);
                            cmd.Parameters.AddWithValue("@InjectionConfig_LogList_AutoClear", Socket_Cache.LogList.Socket_AutoClear);
                            cmd.Parameters.AddWithValue("@InjectionConfig_LogList_AutoClear_Value", Socket_Cache.LogList.Socket_AutoClear_Value);
                            cmd.Parameters.AddWithValue("@InjectionConfig_SpeedMode", Socket_Cache.SocketPacket.SpeedMode);
                            cmd.Parameters.AddWithValue("@InjectionConfig_FilterExecute", Socket_Cache.Filter.FilterExecute);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//滤镜列表

            private static bool CreateTable_Filter()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {                        
                        string sql = "CREATE TABLE IF NOT EXISTS Filter (";
                        sql += "GUID TEXT NOT NULL PRIMARY KEY,";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";                        
                        sql += "Name TEXT NOT NULL,";
                        sql += "AppointHeader BOOLEAN DEFAULT 0,";
                        sql += "HeaderContent TEXT,";
                        sql += "AppointSocket BOOLEAN DEFAULT 0,";
                        sql += "SocketContent INTEGER DEFAULT 0,";
                        sql += "AppointLength BOOLEAN DEFAULT 0,";
                        sql += "LengthContent TEXT,";
                        sql += "AppointPort BOOLEAN DEFAULT 0,";
                        sql += "PortContent INTEGER DEFAULT 0,";
                        sql += "Mode INTEGER NOT NULL DEFAULT 0,";
                        sql += "Action INTEGER NOT NULL DEFAULT 0,";
                        sql += "IsExecute BOOLEAN DEFAULT 0,";
                        sql += "ExecuteType INTEGER DEFAULT 0,";
                        sql += "Send_GUID TEXT NOT NULL,";
                        sql += "Robot_GUID TEXT NOT NULL,";
                        sql += "Function TEXT NOT NULL,";
                        sql += "StartFrom INTEGER DEFAULT 0,";
                        sql += "ProgressionStep INTEGER DEFAULT 1,";
                        sql += "ProgressionPosition TEXT,";
                        sql += "Search TEXT,";
                        sql += "Modify TEXT";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_Filter()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM Filter;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }                      
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_Filter()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM Filter;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void InsertTable_Filter(Socket_FilterInfo sfi)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "INSERT INTO Filter (";
                        sql += "GUID,";
                        sql += "IsEnable,";                        
                        sql += "Name,";
                        sql += "AppointHeader,";
                        sql += "HeaderContent,";
                        sql += "AppointSocket,";
                        sql += "SocketContent,";
                        sql += "AppointLength,";
                        sql += "LengthContent,";
                        sql += "AppointPort,";
                        sql += "PortContent,";
                        sql += "Mode,";
                        sql += "Action,";
                        sql += "IsExecute,";
                        sql += "ExecuteType,";
                        sql += "Send_GUID,";
                        sql += "Robot_GUID,";
                        sql += "Function,";
                        sql += "StartFrom,";
                        sql += "ProgressionStep,";
                        sql += "ProgressionPosition,";
                        sql += "Search,";
                        sql += "Modify";
                        sql += ") VALUES (";
                        sql += "@GUID,";
                        sql += "@IsEnable,";                        
                        sql += "@Name,";
                        sql += "@AppointHeader,";
                        sql += "@HeaderContent,";
                        sql += "@AppointSocket,";
                        sql += "@SocketContent,";
                        sql += "@AppointLength,";
                        sql += "@LengthContent,";
                        sql += "@AppointPort,";
                        sql += "@PortContent,";
                        sql += "@Mode,";
                        sql += "@Action,";
                        sql += "@IsExecute,";
                        sql += "@ExecuteType,";
                        sql += "@Send_GUID,";
                        sql += "@Robot_GUID,";
                        sql += "@Function,";
                        sql += "@StartFrom,";
                        sql += "@ProgressionStep,";
                        sql += "@ProgressionPosition,";
                        sql += "@Search,";
                        sql += "@Modify";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", sfi.FID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@IsEnable", sfi.IsEnable);
                            cmd.Parameters.AddWithValue("@Name", sfi.FName);
                            cmd.Parameters.AddWithValue("@AppointHeader", sfi.AppointHeader);
                            cmd.Parameters.AddWithValue("@HeaderContent", sfi.HeaderContent);
                            cmd.Parameters.AddWithValue("@AppointSocket", sfi.AppointSocket);
                            cmd.Parameters.AddWithValue("@SocketContent", sfi.SocketContent);
                            cmd.Parameters.AddWithValue("@AppointLength", sfi.AppointLength);
                            cmd.Parameters.AddWithValue("@LengthContent", sfi.LengthContent);
                            cmd.Parameters.AddWithValue("@AppointPort", sfi.AppointPort);
                            cmd.Parameters.AddWithValue("@PortContent", sfi.PortContent);
                            cmd.Parameters.AddWithValue("@Mode", sfi.FMode);
                            cmd.Parameters.AddWithValue("@Action", sfi.FAction);
                            cmd.Parameters.AddWithValue("@IsExecute", sfi.IsExecute);
                            cmd.Parameters.AddWithValue("@ExecuteType", sfi.FEType);
                            cmd.Parameters.AddWithValue("@Send_GUID", sfi.SID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@Robot_GUID", sfi.RID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@Function", Socket_Cache.Filter.GetFilterFunctionString(sfi.FFunction));
                            cmd.Parameters.AddWithValue("@StartFrom", sfi.FStartFrom);
                            cmd.Parameters.AddWithValue("@ProgressionStep", sfi.ProgressionStep);
                            cmd.Parameters.AddWithValue("@ProgressionPosition", sfi.ProgressionPosition);
                            cmd.Parameters.AddWithValue("@Search", sfi.FSearch);
                            cmd.Parameters.AddWithValue("@Modify", sfi.FModify);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//发送列表

            private static bool CreateTable_Send()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS Send (";
                        sql += "GUID TEXT NOT NULL PRIMARY KEY,";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";                        
                        sql += "Name TEXT NOT NULL,";
                        sql += "SystemSocket BOOLEAN DEFAULT 0,";
                        sql += "LoopCNT INTEGER NOT NULL DEFAULT 1,";
                        sql += "LoopINT INTEGER NOT NULL DEFAULT 1000,";
                        sql += "Notes TEXT";
                        sql += ");";

                        sql += "CREATE TABLE IF NOT EXISTS SendCollection (";
                        sql += "GUID TEXT NOT NULL,";
                        sql += "Socket INTEGER NOT NULL,";
                        sql += "Type INTEGER NOT NULL,";
                        sql += "IPTo TEXT NOT NULL,";
                        sql += "Buffer BLOB,";
                        sql += "FOREIGN KEY (GUID) REFERENCES Send(GUID)";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_Send()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM Send;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static DataTable SelectTable_SendCollection(Guid guid)
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT Socket, Type, IPTo, Buffer FROM SendCollection WHERE GUID = @GUID;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", guid.ToString().ToUpper());

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                            adapter.Fill(dtReturn);
                        }  
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_Send()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM SendCollection;";
                        sql += "DELETE FROM Send;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void InsertTable_Send(Socket_SendInfo ssi)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        conn.Open();

                        string sql = "INSERT INTO Send (";
                        sql += "GUID,";
                        sql += "IsEnable,";
                        sql += "Name,";
                        sql += "SystemSocket,";
                        sql += "LoopCNT,";
                        sql += "LoopINT,";
                        sql += "Notes";
                        sql += ") VALUES (";
                        sql += "@GUID,";
                        sql += "@IsEnable,";
                        sql += "@Name,";
                        sql += "@SystemSocket,";
                        sql += "@LoopCNT,";
                        sql += "@LoopINT,";
                        sql += "@Notes";
                        sql += ");";                        

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", ssi.SID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@IsEnable", ssi.IsEnable);
                            cmd.Parameters.AddWithValue("@Name", ssi.SName);
                            cmd.Parameters.AddWithValue("@SystemSocket", ssi.SSystemSocket);
                            cmd.Parameters.AddWithValue("@LoopCNT", ssi.SLoopCNT);
                            cmd.Parameters.AddWithValue("@LoopINT", ssi.SLoopINT);
                            cmd.Parameters.AddWithValue("@Notes", ssi.SNotes);
                            cmd.ExecuteNonQuery();
                        }

                        foreach (DataRow row in ssi.SCollection.Rows)
                        {
                            sql = "INSERT INTO SendCollection (";
                            sql += "GUID,";
                            sql += "Socket,";
                            sql += "Type,";
                            sql += "IPTo,";
                            sql += "Buffer";
                            sql += ") VALUES (";
                            sql += "@GUID,";
                            sql += "@Socket,";
                            sql += "@Type,";
                            sql += "@IPTo,";
                            sql += "@Buffer";
                            sql += ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@GUID", ssi.SID.ToString().ToUpper());
                                cmd.Parameters.AddWithValue("@Socket", Convert.ToInt32(row["Socket"]));
                                cmd.Parameters.AddWithValue("@Type", Convert.ToInt32(row["Type"]));
                                cmd.Parameters.AddWithValue("@IPTo", row["IPTo"].ToString());
                                cmd.Parameters.AddWithValue("@Buffer", (byte[])row["Buffer"]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//机器人列表

            private static bool CreateTable_Robot()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS Robot (";
                        sql += "GUID TEXT NOT NULL PRIMARY KEY,";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";
                        sql += "Name TEXT NOT NULL";
                        sql += ");";

                        sql += "CREATE TABLE IF NOT EXISTS RobotInstruction (";
                        sql += "GUID TEXT NOT NULL,";
                        sql += "Type INTEGER NOT NULL,";
                        sql += "Content TEXT,";
                        sql += "FOREIGN KEY (GUID) REFERENCES Robot(GUID)";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_Robot()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM Robot;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static DataTable SelectTable_RobotInstruction(Guid guid)
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT Type, Content FROM RobotInstruction WHERE GUID = @GUID;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", guid.ToString().ToUpper());

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_Robot()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM RobotInstruction;";
                        sql += "DELETE FROM Robot;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void InsertTable_Robot(Socket_RobotInfo sri)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        conn.Open();

                        string sql = "INSERT INTO Robot (";
                        sql += "GUID,";
                        sql += "IsEnable,";
                        sql += "Name";
                        sql += ") VALUES (";
                        sql += "@GUID,";
                        sql += "@IsEnable,";
                        sql += "@Name";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", sri.RID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@IsEnable", sri.IsEnable);
                            cmd.Parameters.AddWithValue("@Name", sri.RName);                            
                            cmd.ExecuteNonQuery();
                        }

                        foreach (DataRow row in sri.RInstruction.Rows)
                        {
                            sql = "INSERT INTO RobotInstruction (";
                            sql += "GUID,";
                            sql += "Type,";
                            sql += "Content";
                            sql += ") VALUES (";
                            sql += "@GUID,";
                            sql += "@Type,";
                            sql += "@Content";
                            sql += ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@GUID", sri.RID.ToString().ToUpper());
                                cmd.Parameters.AddWithValue("@Type", Convert.ToInt32(row["Type"]));
                                cmd.Parameters.AddWithValue("@Content", row["Content"].ToString());                                
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//代理账号

            private static bool CreateTable_ProxyAccount()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS ProxyAccount (";
                        sql += "GUID TEXT NOT NULL PRIMARY KEY,";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";
                        sql += "UserName TEXT NOT NULL UNIQUE,";
                        sql += "PassWord TEXT NOT NULL,";
                        sql += "LoginIP TEXT,";
                        sql += "IsExpiry BOOLEAN DEFAULT 0,";
                        sql += "ExpiryTime TIMESTAMP,";                        
                        sql += "CreateTime TIMESTAMP";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_ProxyAccount()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM ProxyAccount;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_ProxyAccount()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM ProxyAccount;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void InsertTable_ProxyAccount(Proxy_AccountInfo pai)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "INSERT INTO ProxyAccount (";
                        sql += "GUID,";
                        sql += "IsEnable,";
                        sql += "UserName,";
                        sql += "PassWord,";
                        sql += "LoginIP,";
                        sql += "IsExpiry,";
                        sql += "ExpiryTime,";
                        sql += "CreateTime";
                        sql += ") VALUES (";
                        sql += "@GUID,";
                        sql += "@IsEnable,";
                        sql += "@UserName,";
                        sql += "@PassWord,";
                        sql += "@LoginIP,";
                        sql += "@IsExpiry,";
                        sql += "@ExpiryTime,";
                        sql += "@CreateTime";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", pai.AID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@IsEnable", pai.IsEnable);
                            cmd.Parameters.AddWithValue("@UserName", pai.UserName);
                            cmd.Parameters.AddWithValue("@PassWord", pai.PassWord);
                            cmd.Parameters.AddWithValue("@LoginIP", pai.LoginIP);
                            cmd.Parameters.AddWithValue("@IsExpiry", pai.IsExpiry);
                            cmd.Parameters.AddWithValue("@ExpiryTime", pai.ExpiryTime);
                            cmd.Parameters.AddWithValue("@CreateTime", pai.CreateTime);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
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
    }
}
