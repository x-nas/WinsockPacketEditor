using Be.Windows.Forms;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

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
            public static string AESKey = string.Empty;

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
                SystemBackUp_Import = 10,
                SystemBackUp_Export = 11,
                MapLocal_Import = 12,
                MapLocal_Export = 13,
                MapRemote_Import = 14,
                MapRemote_Export = 15,
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

            public static XElement GetSystemConfig_XML()
            {
                try
                {
                    XElement xeSystemConfig = 
                        new XElement("SystemConfig",                 
                        new XElement("DefaultLanguage", Socket_Cache.System.DefaultLanguage),
                        new XElement("LastInjection", Socket_Cache.System.LastInjection),
                        new XElement("StartMode", Socket_Cache.System.StartMode),
                        new XElement("Remote_IsEnable", Socket_Cache.System.IsRemote),
                        new XElement("Remote_UserName", Socket_Cache.System.Remote_UserName),
                        new XElement("Remote_PassWord", Socket_Cache.System.Remote_PassWord),
                        new XElement("Remote_Port", Socket_Cache.System.Remote_Port),
                        new XElement("Remote_URL", Socket_Cache.System.Remote_URL)  
                        );
                    
                    return xeSystemConfig;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
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

            public static void SetSystemConfig_FromXML(XElement xeSystemConfig)
            {
                try
                {
                    XElement DefaultLanguage = xeSystemConfig.Element("DefaultLanguage");
                    if (DefaultLanguage != null)
                    {
                        Socket_Cache.System.DefaultLanguage = DefaultLanguage.Value;
                    }

                    XElement LastInjection = xeSystemConfig.Element("LastInjection");
                    if (LastInjection != null)
                    {
                        Socket_Cache.System.LastInjection = LastInjection.Value;
                    }

                    XElement StartMode = xeSystemConfig.Element("StartMode");
                    if (StartMode != null)
                    {
                        Socket_Cache.System.StartMode = Socket_Cache.System.GetSystemMode_ByString(StartMode.Value);
                    }

                    XElement Remote_IsEnable = xeSystemConfig.Element("Remote_IsEnable");
                    if (Remote_IsEnable != null)
                    {
                        Socket_Cache.System.IsRemote = Convert.ToBoolean(Remote_IsEnable.Value);
                    }

                    XElement Remote_UserName = xeSystemConfig.Element("Remote_UserName");
                    if (Remote_UserName != null)
                    {
                        Socket_Cache.System.Remote_UserName = Remote_UserName.Value;
                    }

                    XElement Remote_PassWord = xeSystemConfig.Element("Remote_PassWord");
                    if (Remote_PassWord != null)
                    {
                        Socket_Cache.System.Remote_PassWord = Remote_PassWord.Value;
                    }

                    XElement Remote_Port = xeSystemConfig.Element("Remote_Port");
                    if (Remote_Port != null)
                    {
                        Socket_Cache.System.Remote_Port = ushort.Parse(Remote_Port.Value);
                    }

                    XElement Remote_URL = xeSystemConfig.Element("Remote_URL");
                    if (Remote_URL != null)
                    {
                        Socket_Cache.System.Remote_URL = Remote_URL.Value;
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

            public static XElement GetProxyConfig_XML()
            {
                try
                {
                    XElement xeProxyConfig =
                        new XElement("ProxyConfig",
                        new XElement("ProxyIP_Auto", Socket_Cache.SocketProxy.ProxyIP_Auto),
                        new XElement("Enable_SOCKS5", Socket_Cache.SocketProxy.Enable_SOCKS5),
                        new XElement("ProxyPort", Socket_Cache.SocketProxy.ProxyPort),
                        new XElement("Enable_Auth", Socket_Cache.SocketProxy.Enable_Auth),
                        new XElement("NoRecord", Socket_Cache.SocketProxy.NoRecord),
                        new XElement("DelClosed", Socket_Cache.SocketProxy.DelClosed),
                        new XElement("LogList_AutoRoll", Socket_Cache.LogList.Proxy_AutoRoll),
                        new XElement("LogList_AutoClear", Socket_Cache.LogList.Proxy_AutoClear),
                        new XElement("LogList_AutoClear_Value", Socket_Cache.LogList.Proxy_AutoClear_Value),
                        new XElement("Enable_MapLocal", Socket_Cache.ProxyMapping.Enable_MapLocal),
                        new XElement("Enable_ExternalProxy", Socket_Cache.SocketProxy.Enable_ExternalProxy),
                        new XElement("ExternalProxy_IP", Socket_Cache.SocketProxy.ExternalProxy_IP),
                        new XElement("ExternalProxy_Port", Socket_Cache.SocketProxy.ExternalProxy_Port),
                        new XElement("Enable_ExternalProxy_AppointPort", Socket_Cache.SocketProxy.Enable_ExternalProxy_AppointPort),
                        new XElement("ExternalProxy_AppointPort", Socket_Cache.SocketProxy.ExternalProxy_AppointPort),
                        new XElement("Enable_ExternalProxy_Auth", Socket_Cache.SocketProxy.Enable_ExternalProxy_Auth),
                        new XElement("ExternalProxy_UserName", Socket_Cache.SocketProxy.ExternalProxy_UserName),
                        new XElement("ExternalProxy_PassWord", Socket_Cache.SocketProxy.ExternalProxy_PassWord),
                        new XElement("SpeedMode", Socket_Cache.SocketProxy.SpeedMode)
                        );

                    return xeProxyConfig;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            public static XElement GetInjectionConfig_XML()
            {
                try
                {
                    XElement xeInjectionConfig =
                        new XElement("InjectionConfig",
                        new XElement("CheckNotShow", Socket_Cache.SocketPacket.CheckNotShow),
                        new XElement("CheckSocket", Socket_Cache.SocketPacket.CheckSocket),
                        new XElement("CheckSocket_Value", Socket_Cache.SocketPacket.CheckSocket_Value),
                        new XElement("CheckIP", Socket_Cache.SocketPacket.CheckIP),
                        new XElement("CheckIP_Value", Socket_Cache.SocketPacket.CheckIP_Value),
                        new XElement("CheckPort", Socket_Cache.SocketPacket.CheckPort),
                        new XElement("CheckPort_Value", Socket_Cache.SocketPacket.CheckPort_Value),
                        new XElement("CheckHead", Socket_Cache.SocketPacket.CheckHead),
                        new XElement("CheckHead_Value", Socket_Cache.SocketPacket.CheckHead_Value),
                        new XElement("CheckData", Socket_Cache.SocketPacket.CheckData),
                        new XElement("CheckData_Value", Socket_Cache.SocketPacket.CheckData_Value),
                        new XElement("CheckSize", Socket_Cache.SocketPacket.CheckSize),
                        new XElement("CheckLength_Value", Socket_Cache.SocketPacket.CheckLength_Value),
                        new XElement("HookWS1_Send", Socket_Cache.SocketPacket.HookWS1_Send),
                        new XElement("HookWS1_SendTo", Socket_Cache.SocketPacket.HookWS1_SendTo),
                        new XElement("HookWS1_Recv", Socket_Cache.SocketPacket.HookWS1_Recv),
                        new XElement("HookWS1_RecvFrom", Socket_Cache.SocketPacket.HookWS1_RecvFrom),
                        new XElement("HookWS2_Send", Socket_Cache.SocketPacket.HookWS2_Send),
                        new XElement("HookWS2_SendTo", Socket_Cache.SocketPacket.HookWS2_SendTo),
                        new XElement("HookWS2_Recv", Socket_Cache.SocketPacket.HookWS2_Recv),
                        new XElement("HookWS2_RecvFrom", Socket_Cache.SocketPacket.HookWS2_RecvFrom),
                        new XElement("HookWSA_Send", Socket_Cache.SocketPacket.HookWSA_Send),
                        new XElement("HookWSA_SendTo", Socket_Cache.SocketPacket.HookWSA_SendTo),
                        new XElement("HookWSA_Recv", Socket_Cache.SocketPacket.HookWSA_Recv),
                        new XElement("HookWSA_RecvFrom", Socket_Cache.SocketPacket.HookWSA_RecvFrom),
                        new XElement("HotKey1", Socket_Cache.SocketPacket.HotKey1),
                        new XElement("HotKey2", Socket_Cache.SocketPacket.HotKey2),
                        new XElement("HotKey3", Socket_Cache.SocketPacket.HotKey3),
                        new XElement("HotKey4", Socket_Cache.SocketPacket.HotKey4),
                        new XElement("HotKey5", Socket_Cache.SocketPacket.HotKey5),
                        new XElement("HotKey6", Socket_Cache.SocketPacket.HotKey6),
                        new XElement("HotKey7", Socket_Cache.SocketPacket.HotKey7),
                        new XElement("HotKey8", Socket_Cache.SocketPacket.HotKey8),
                        new XElement("HotKey9", Socket_Cache.SocketPacket.HotKey9),
                        new XElement("HotKey10", Socket_Cache.SocketPacket.HotKey10),
                        new XElement("HotKey11", Socket_Cache.SocketPacket.HotKey11),
                        new XElement("HotKey12", Socket_Cache.SocketPacket.HotKey12),
                        new XElement("SocketList_AutoRoll", Socket_Cache.SocketList.AutoRoll),
                        new XElement("SocketList_AutoClear", Socket_Cache.SocketList.AutoClear),
                        new XElement("SocketList_AutoClear_Value", Socket_Cache.SocketList.AutoClear_Value),
                        new XElement("LogList_AutoRoll", Socket_Cache.LogList.Socket_AutoRoll),
                        new XElement("LogList_AutoClear", Socket_Cache.LogList.Socket_AutoClear),
                        new XElement("LogList_AutoClear_Value", Socket_Cache.LogList.Socket_AutoClear_Value),
                        new XElement("SpeedMode", Socket_Cache.SocketPacket.SpeedMode),
                        new XElement("FilterExecute", Socket_Cache.Filter.FilterExecute)
                        );

                    return xeInjectionConfig;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
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
                        Socket_Cache.ProxyMapping.Enable_MapLocal = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_Enable_MapLocal"]);
                        Socket_Cache.SocketProxy.Enable_ExternalProxy = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_Enable_ExternalProxy"]);
                        Socket_Cache.SocketProxy.ExternalProxy_IP = RunConfig.Rows[0]["ProxyConfig_ExternalProxy_IP"].ToString();
                        Socket_Cache.SocketProxy.ExternalProxy_Port = ushort.Parse(RunConfig.Rows[0]["ProxyConfig_ExternalProxy_Port"].ToString());                                                
                        Socket_Cache.SocketProxy.Enable_ExternalProxy_AppointPort = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_Enable_ExternalProxy_AppointPort"]);
                        Socket_Cache.SocketProxy.ExternalProxy_AppointPort = RunConfig.Rows[0]["ProxyConfig_ExternalProxy_AppointPort"].ToString();
                        Socket_Cache.SocketProxy.Enable_ExternalProxy_Auth = Convert.ToBoolean(RunConfig.Rows[0]["ProxyConfig_Enable_ExternalProxy_Auth"]);
                        Socket_Cache.SocketProxy.ExternalProxy_UserName = RunConfig.Rows[0]["ProxyConfig_ExternalProxy_UserName"].ToString();
                        Socket_Cache.SocketProxy.ExternalProxy_PassWord = RunConfig.Rows[0]["ProxyConfig_ExternalProxy_PassWord"].ToString();
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
                        Socket_Cache.SocketPacket.HotKey1 = RunConfig.Rows[0]["InjectionConfig_HotKey1"].ToString();
                        Socket_Cache.SocketPacket.HotKey2 = RunConfig.Rows[0]["InjectionConfig_HotKey2"].ToString();
                        Socket_Cache.SocketPacket.HotKey3 = RunConfig.Rows[0]["InjectionConfig_HotKey3"].ToString();
                        Socket_Cache.SocketPacket.HotKey4 = RunConfig.Rows[0]["InjectionConfig_HotKey4"].ToString();
                        Socket_Cache.SocketPacket.HotKey5 = RunConfig.Rows[0]["InjectionConfig_HotKey5"].ToString();
                        Socket_Cache.SocketPacket.HotKey6 = RunConfig.Rows[0]["InjectionConfig_HotKey6"].ToString();
                        Socket_Cache.SocketPacket.HotKey7 = RunConfig.Rows[0]["InjectionConfig_HotKey7"].ToString();
                        Socket_Cache.SocketPacket.HotKey8 = RunConfig.Rows[0]["InjectionConfig_HotKey8"].ToString();
                        Socket_Cache.SocketPacket.HotKey9 = RunConfig.Rows[0]["InjectionConfig_HotKey9"].ToString();
                        Socket_Cache.SocketPacket.HotKey10 = RunConfig.Rows[0]["InjectionConfig_HotKey10"].ToString();
                        Socket_Cache.SocketPacket.HotKey11 = RunConfig.Rows[0]["InjectionConfig_HotKey11"].ToString();
                        Socket_Cache.SocketPacket.HotKey12 = RunConfig.Rows[0]["InjectionConfig_HotKey12"].ToString();
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

            public static void SetProxyConfig_FromXML(XElement xeProxyConfig)
            {
                try
                {
                    XElement ProxyIP_Auto = xeProxyConfig.Element("ProxyIP_Auto");
                    if (ProxyIP_Auto != null)
                    {
                        Socket_Cache.SocketProxy.ProxyIP_Auto = Convert.ToBoolean(ProxyIP_Auto.Value);
                    }

                    XElement Enable_SOCKS5 = xeProxyConfig.Element("Enable_SOCKS5");
                    if (Enable_SOCKS5 != null)
                    {
                        Socket_Cache.SocketProxy.Enable_SOCKS5 = Convert.ToBoolean(Enable_SOCKS5.Value);
                    }

                    XElement ProxyPort = xeProxyConfig.Element("ProxyPort");
                    if (ProxyPort != null)
                    {
                        Socket_Cache.SocketProxy.ProxyPort = ushort.Parse(ProxyPort.Value);
                    }

                    XElement Enable_Auth = xeProxyConfig.Element("Enable_Auth");
                    if (Enable_Auth != null)
                    {
                        Socket_Cache.SocketProxy.Enable_Auth = Convert.ToBoolean(Enable_Auth.Value);
                    }

                    XElement NoRecord = xeProxyConfig.Element("NoRecord");
                    if (NoRecord != null)
                    {
                        Socket_Cache.SocketProxy.NoRecord = Convert.ToBoolean(NoRecord.Value);
                    }

                    XElement DelClosed = xeProxyConfig.Element("DelClosed");
                    if (DelClosed != null)
                    {
                        Socket_Cache.SocketProxy.DelClosed = Convert.ToBoolean(DelClosed.Value);
                    }

                    XElement LogList_AutoRoll = xeProxyConfig.Element("LogList_AutoRoll");
                    if (LogList_AutoRoll != null)
                    {
                        Socket_Cache.LogList.Proxy_AutoRoll = Convert.ToBoolean(LogList_AutoRoll.Value);
                    }

                    XElement LogList_AutoClear = xeProxyConfig.Element("LogList_AutoClear");
                    if (LogList_AutoClear != null)
                    {
                        Socket_Cache.LogList.Proxy_AutoClear = Convert.ToBoolean(LogList_AutoClear.Value);
                    }

                    XElement LogList_AutoClear_Value = xeProxyConfig.Element("LogList_AutoClear_Value");
                    if (LogList_AutoClear_Value != null)
                    {
                        Socket_Cache.LogList.Proxy_AutoClear_Value = int.Parse(LogList_AutoClear_Value.Value);
                    }

                    XElement Enable_MapLocal = xeProxyConfig.Element("Enable_MapLocal");
                    if (Enable_MapLocal != null)
                    {
                        Socket_Cache.ProxyMapping.Enable_MapLocal = Convert.ToBoolean(Enable_MapLocal.Value);
                    }

                    XElement Enable_ExternalProxy = xeProxyConfig.Element("Enable_ExternalProxy");
                    if (Enable_ExternalProxy != null)
                    {
                        Socket_Cache.SocketProxy.Enable_ExternalProxy = Convert.ToBoolean(Enable_ExternalProxy.Value);
                    }

                    XElement ExternalProxy_IP = xeProxyConfig.Element("ExternalProxy_IP");
                    if (ExternalProxy_IP != null)
                    {
                        Socket_Cache.SocketProxy.ExternalProxy_IP = ExternalProxy_IP.Value;
                    }

                    XElement ExternalProxy_Port = xeProxyConfig.Element("ExternalProxy_Port");
                    if (ExternalProxy_Port != null)
                    {
                        Socket_Cache.SocketProxy.ExternalProxy_Port = ushort.Parse(ExternalProxy_Port.Value);
                    }

                    XElement Enable_ExternalProxy_AppointPort = xeProxyConfig.Element("Enable_ExternalProxy_AppointPort");
                    if (Enable_ExternalProxy_AppointPort != null)
                    {
                        Socket_Cache.SocketProxy.Enable_ExternalProxy_AppointPort = Convert.ToBoolean(Enable_ExternalProxy_AppointPort.Value);
                    }

                    XElement ExternalProxy_AppointPort = xeProxyConfig.Element("ExternalProxy_AppointPort");
                    if (ExternalProxy_AppointPort != null)
                    {
                        Socket_Cache.SocketProxy.ExternalProxy_AppointPort = ExternalProxy_AppointPort.Value;
                    }

                    XElement Enable_ExternalProxy_Auth = xeProxyConfig.Element("Enable_ExternalProxy_Auth");
                    if (Enable_ExternalProxy_Auth != null)
                    {
                        Socket_Cache.SocketProxy.Enable_ExternalProxy_Auth = Convert.ToBoolean(Enable_ExternalProxy_Auth.Value);
                    }

                    XElement ExternalProxy_UserName = xeProxyConfig.Element("ExternalProxy_UserName");
                    if (ExternalProxy_UserName != null)
                    {
                        Socket_Cache.SocketProxy.ExternalProxy_UserName = ExternalProxy_UserName.Value;
                    }

                    XElement ExternalProxy_PassWord = xeProxyConfig.Element("ExternalProxy_PassWord");
                    if (ExternalProxy_PassWord != null)
                    {
                        Socket_Cache.SocketProxy.ExternalProxy_PassWord = ExternalProxy_PassWord.Value;
                    }

                    XElement SpeedMode = xeProxyConfig.Element("SpeedMode");
                    if (SpeedMode != null)
                    {
                        Socket_Cache.SocketProxy.SpeedMode = Convert.ToBoolean(SpeedMode.Value);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void SetInjectionConfig_FromXML(XElement xeInjectionConfig)
            {
                try
                {
                    XElement CheckNotShow = xeInjectionConfig.Element("CheckNotShow");
                    if (CheckNotShow != null)
                    {
                        Socket_Cache.SocketPacket.CheckNotShow = Convert.ToBoolean(CheckNotShow.Value);
                    }

                    XElement CheckSocket = xeInjectionConfig.Element("CheckSocket");
                    if (CheckSocket != null)
                    {
                        Socket_Cache.SocketPacket.CheckSocket = Convert.ToBoolean(CheckSocket.Value);
                    }

                    XElement CheckSocket_Value = xeInjectionConfig.Element("CheckSocket_Value");
                    if (CheckSocket_Value != null)
                    {
                        Socket_Cache.SocketPacket.CheckSocket_Value = CheckSocket_Value.Value;
                    }

                    XElement CheckIP = xeInjectionConfig.Element("CheckIP");
                    if (CheckIP != null)
                    {
                        Socket_Cache.SocketPacket.CheckIP = Convert.ToBoolean(CheckIP.Value);
                    }

                    XElement CheckIP_Value = xeInjectionConfig.Element("CheckIP_Value");
                    if (CheckIP_Value != null)
                    {
                        Socket_Cache.SocketPacket.CheckIP_Value = CheckIP_Value.Value;
                    }

                    XElement CheckPort = xeInjectionConfig.Element("CheckPort");
                    if (CheckPort != null)
                    {
                        Socket_Cache.SocketPacket.CheckPort = Convert.ToBoolean(CheckPort.Value);
                    }

                    XElement CheckPort_Value = xeInjectionConfig.Element("CheckPort_Value");
                    if (CheckPort_Value != null)
                    {
                        Socket_Cache.SocketPacket.CheckPort_Value = CheckPort_Value.Value;
                    }

                    XElement CheckHead = xeInjectionConfig.Element("CheckHead");
                    if (CheckHead != null)
                    {
                        Socket_Cache.SocketPacket.CheckHead = Convert.ToBoolean(CheckHead.Value);
                    }

                    XElement CheckHead_Value = xeInjectionConfig.Element("CheckHead_Value");
                    if (CheckHead_Value != null)
                    {
                        Socket_Cache.SocketPacket.CheckHead_Value = CheckHead_Value.Value;
                    }

                    XElement CheckData = xeInjectionConfig.Element("CheckData");
                    if (CheckData != null)
                    {
                        Socket_Cache.SocketPacket.CheckData = Convert.ToBoolean(CheckData.Value);
                    }

                    XElement CheckData_Value = xeInjectionConfig.Element("CheckData_Value");
                    if (CheckData_Value != null)
                    {
                        Socket_Cache.SocketPacket.CheckData_Value = CheckData_Value.Value;
                    }

                    XElement CheckSize = xeInjectionConfig.Element("CheckSize");
                    if (CheckSize != null)
                    {
                        Socket_Cache.SocketPacket.CheckSize = Convert.ToBoolean(CheckSize.Value);
                    }

                    XElement CheckLength_Value = xeInjectionConfig.Element("CheckLength_Value");
                    if (CheckLength_Value != null)
                    {
                        Socket_Cache.SocketPacket.CheckLength_Value = CheckLength_Value.Value;
                    }

                    XElement HookWS1_Send = xeInjectionConfig.Element("HookWS1_Send");
                    if (HookWS1_Send != null)
                    {
                        Socket_Cache.SocketPacket.HookWS1_Send = Convert.ToBoolean(HookWS1_Send.Value);
                    }

                    XElement HookWS1_SendTo = xeInjectionConfig.Element("HookWS1_SendTo");
                    if (HookWS1_SendTo != null)
                    {
                        Socket_Cache.SocketPacket.HookWS1_SendTo = Convert.ToBoolean(HookWS1_SendTo.Value);
                    }

                    XElement HookWS1_Recv = xeInjectionConfig.Element("HookWS1_Recv");
                    if (HookWS1_Recv != null)
                    {
                        Socket_Cache.SocketPacket.HookWS1_Recv = Convert.ToBoolean(HookWS1_Recv.Value);
                    }

                    XElement HookWS1_RecvFrom = xeInjectionConfig.Element("HookWS1_RecvFrom");
                    if (HookWS1_RecvFrom != null)
                    {
                        Socket_Cache.SocketPacket.HookWS1_RecvFrom = Convert.ToBoolean(HookWS1_RecvFrom.Value);
                    }

                    XElement HookWS2_Send = xeInjectionConfig.Element("HookWS2_Send");
                    if (HookWS2_Send != null)
                    {
                        Socket_Cache.SocketPacket.HookWS2_Send = Convert.ToBoolean(HookWS2_Send.Value);
                    }

                    XElement HookWS2_SendTo = xeInjectionConfig.Element("HookWS2_SendTo");
                    if (HookWS2_SendTo != null)
                    {
                        Socket_Cache.SocketPacket.HookWS2_SendTo = Convert.ToBoolean(HookWS2_SendTo.Value);
                    }

                    XElement HookWS2_Recv = xeInjectionConfig.Element("HookWS2_Recv");
                    if (HookWS2_Recv != null)
                    {
                        Socket_Cache.SocketPacket.HookWS2_Recv = Convert.ToBoolean(HookWS2_Recv.Value);
                    }

                    XElement HookWS2_RecvFrom = xeInjectionConfig.Element("HookWS2_RecvFrom");
                    if (HookWS2_RecvFrom != null)
                    {
                        Socket_Cache.SocketPacket.HookWS2_RecvFrom = Convert.ToBoolean(HookWS2_RecvFrom.Value);
                    }

                    XElement HookWSA_Send = xeInjectionConfig.Element("HookWSA_Send");
                    if (HookWSA_Send != null)
                    {
                        Socket_Cache.SocketPacket.HookWSA_Send = Convert.ToBoolean(HookWSA_Send.Value);
                    }

                    XElement HookWSA_SendTo = xeInjectionConfig.Element("HookWSA_SendTo");
                    if (HookWSA_SendTo != null)
                    {
                        Socket_Cache.SocketPacket.HookWSA_SendTo = Convert.ToBoolean(HookWSA_SendTo.Value);
                    }

                    XElement HookWSA_Recv = xeInjectionConfig.Element("HookWSA_Recv");
                    if (HookWSA_Recv != null)
                    {
                        Socket_Cache.SocketPacket.HookWSA_Recv = Convert.ToBoolean(HookWSA_Recv.Value);
                    }

                    XElement HookWSA_RecvFrom = xeInjectionConfig.Element("HookWSA_RecvFrom");
                    if (HookWSA_RecvFrom != null)
                    {
                        Socket_Cache.SocketPacket.HookWSA_RecvFrom = Convert.ToBoolean(HookWSA_RecvFrom.Value);
                    }

                    XElement HotKey1 = xeInjectionConfig.Element("HotKey1");
                    if (HotKey1 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey1 = HotKey1.Value;
                    }

                    XElement HotKey2 = xeInjectionConfig.Element("HotKey2");
                    if (HotKey2 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey2 = HotKey2.Value;
                    }

                    XElement HotKey3 = xeInjectionConfig.Element("HotKey3");
                    if (HotKey3 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey3 = HotKey3.Value;
                    }

                    XElement HotKey4 = xeInjectionConfig.Element("HotKey4");
                    if (HotKey4 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey4 = HotKey4.Value;
                    }

                    XElement HotKey5 = xeInjectionConfig.Element("HotKey5");
                    if (HotKey5 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey5 = HotKey5.Value;
                    }

                    XElement HotKey6 = xeInjectionConfig.Element("HotKey6");
                    if (HotKey6 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey6 = HotKey6.Value;
                    }

                    XElement HotKey7 = xeInjectionConfig.Element("HotKey7");
                    if (HotKey7 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey7 = HotKey7.Value;
                    }

                    XElement HotKey8 = xeInjectionConfig.Element("HotKey8");
                    if (HotKey8 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey8 = HotKey8.Value;
                    }

                    XElement HotKey9 = xeInjectionConfig.Element("HotKey9");
                    if (HotKey9 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey9 = HotKey9.Value;
                    }

                    XElement HotKey10 = xeInjectionConfig.Element("HotKey10");
                    if (HotKey10 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey10 = HotKey10.Value;
                    }

                    XElement HotKey11 = xeInjectionConfig.Element("HotKey11");
                    if (HotKey11 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey11 = HotKey11.Value;
                    }

                    XElement HotKey12 = xeInjectionConfig.Element("HotKey12");
                    if (HotKey12 != null)
                    {
                        Socket_Cache.SocketPacket.HotKey12 = HotKey12.Value;
                    }

                    XElement SocketList_AutoRoll = xeInjectionConfig.Element("SocketList_AutoRoll");
                    if (SocketList_AutoRoll != null)
                    {
                        Socket_Cache.SocketList.AutoRoll = Convert.ToBoolean(SocketList_AutoRoll.Value);
                    }

                    XElement SocketList_AutoClear = xeInjectionConfig.Element("SocketList_AutoClear");
                    if (SocketList_AutoClear != null)
                    {
                        Socket_Cache.SocketList.AutoClear = Convert.ToBoolean(SocketList_AutoClear.Value);
                    }

                    XElement SocketList_AutoClear_Value = xeInjectionConfig.Element("SocketList_AutoClear_Value");
                    if (SocketList_AutoClear_Value != null)
                    {
                        Socket_Cache.SocketList.AutoClear_Value = int.Parse(SocketList_AutoClear_Value.Value);
                    }

                    XElement LogList_AutoRoll = xeInjectionConfig.Element("LogList_AutoRoll");
                    if (LogList_AutoRoll != null)
                    {
                        Socket_Cache.LogList.Socket_AutoRoll = Convert.ToBoolean(LogList_AutoRoll.Value);
                    }

                    XElement LogList_AutoClear = xeInjectionConfig.Element("LogList_AutoClear");
                    if (LogList_AutoClear != null)
                    {
                        Socket_Cache.LogList.Socket_AutoClear = Convert.ToBoolean(LogList_AutoClear.Value);
                    }

                    XElement LogList_AutoClear_Value = xeInjectionConfig.Element("LogList_AutoClear_Value");
                    if (LogList_AutoClear_Value != null)
                    {
                        Socket_Cache.LogList.Socket_AutoClear_Value = int.Parse(LogList_AutoClear_Value.Value);
                    }

                    XElement SpeedMode = xeInjectionConfig.Element("SpeedMode");
                    if (SpeedMode != null)
                    {
                        Socket_Cache.SocketPacket.SpeedMode = Convert.ToBoolean(SpeedMode.Value);
                    }

                    XElement FilterExecute = xeInjectionConfig.Element("FilterExecute");
                    if (FilterExecute != null)
                    {
                        Socket_Cache.Filter.FilterExecute = Socket_Cache.FilterList.GetFilterListExecute_ByString(FilterExecute.Value);
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

            #region//导出系统备份到文件（对话框）

            public static void ExportSystemBackUp_Dialog(
                string FileName, 
                bool SystemConfig,
                bool ProxySet, 
                bool ProxyAccount, 
                bool ProxyMapping,
                bool InjectionSet, 
                bool FilterList, 
                bool SendList, 
                bool RobotList)
            {
                try
                {
                    SaveFileDialog sfdSaveFile = new SaveFileDialog();
                    sfdSaveFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_213) + "（*.sb）|*.sb";

                    if (!string.IsNullOrEmpty(FileName))
                    {
                        sfdSaveFile.FileName = FileName;
                    }
                    sfdSaveFile.RestoreDirectory = true;

                    if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                    {
                        Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.SystemBackUp_Export);
                        pwForm.ShowDialog();

                        string FilePath = sfdSaveFile.FileName;
                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            Socket_Cache.System.ExportSystemBackUp(
                                FilePath,
                                SystemConfig,
                                ProxySet,
                                ProxyAccount,
                                ProxyMapping,
                                InjectionSet,
                                FilterList,
                                SendList,
                                RobotList,
                                true);

                            string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_214), FilePath);
                            Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void ExportSystemBackUp(
                string FilePath,
                bool SystemConfig,
                bool ProxySet,
                bool ProxyAccount,
                bool ProxyMapping,
                bool InjectionSet,
                bool FilterList,
                bool SendList,
                bool RobotList,
                bool DoEncrypt)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeBackUp = new XElement("WPE64_BackUp");

                    //系统设置
                    if (SystemConfig)
                    {
                        XElement xeSystemConfig = Socket_Cache.System.GetSystemConfig_XML();
                        if (xeSystemConfig != null)
                        {
                            xeBackUp.Add(xeSystemConfig);
                        }
                    }

                    //代理设置
                    if (ProxySet)
                    {
                        XElement xeProxyConfig = Socket_Cache.System.GetProxyConfig_XML();
                        if (xeProxyConfig != null)
                        {
                            xeBackUp.Add(xeProxyConfig);
                        }
                    }

                    //代理账号
                    if (ProxyAccount)
                    {
                        if (Socket_Cache.ProxyAccount.lstProxyAccount.Count > 0)
                        {
                            List<Guid> gExport = new List<Guid>();
                            foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                            {
                                gExport.Add(pai.AID);
                            }

                            XElement xeProxyAccount = Socket_Cache.ProxyAccount.GetProxyAccountList_XML(gExport);
                            if (xeProxyAccount != null)
                            {
                                xeBackUp.Add(xeProxyAccount);
                            }
                        }                        
                    }

                    //代理映射
                    if (ProxyMapping)
                    {
                        //本地映射
                        if (Socket_Cache.ProxyMapping.lstMapLocal.Count > 0)
                        {
                            XElement xeMapLocal = Socket_Cache.ProxyMapping.GetMapLocal_XML(Socket_Cache.ProxyMapping.lstMapLocal.ToList());
                            if (xeMapLocal != null)
                            {
                                xeBackUp.Add(xeMapLocal);
                            }
                        }
                    }

                    //注入设置
                    if (InjectionSet)
                    {
                        XElement xeInjectionConfig = Socket_Cache.System.GetInjectionConfig_XML();
                        if (xeInjectionConfig != null)
                        {
                            xeBackUp.Add(xeInjectionConfig);
                        }
                    }

                    //滤镜列表
                    if (FilterList)
                    {
                        if (Socket_Cache.FilterList.lstFilter.Count > 0)
                        {
                            XElement xeFilterList = Socket_Cache.FilterList.GetFilterList_XML(Socket_Cache.FilterList.lstFilter.ToList());
                            if (xeFilterList != null)
                            {
                                xeBackUp.Add(xeFilterList);
                            }
                        }                        
                    }

                    //发送列表
                    if (SendList)
                    {
                        if (Socket_Cache.SendList.lstSend.Count > 0)
                        {
                            XElement xeSendList = Socket_Cache.SendList.GetSendList_XML(Socket_Cache.SendList.lstSend.ToList());
                            if (xeSendList != null)
                            {
                                xeBackUp.Add(xeSendList);
                            }
                        }                        
                    }

                    //机器人列表
                    if (RobotList)
                    {
                        if (Socket_Cache.RobotList.lstRobot.Count > 0)
                        {
                            XElement xeRobotList = Socket_Cache.RobotList.GetRobotList_XML(Socket_Cache.RobotList.lstRobot.ToList());
                            if (xeRobotList != null)
                            {
                                xeBackUp.Add(xeRobotList);
                            }
                        }
                    }

                    xdoc.Add(xeBackUp);
                    xdoc.Save(FilePath);

                    if (DoEncrypt)
                    {
                        string sPassword = Socket_Cache.System.AESKey;

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

            #endregion

            #region//从文件导入系统备份（对话框）

            public static void ImportSystemBackUp_Dialog()
            {
                try
                {
                    OpenFileDialog ofdLoadFile = new OpenFileDialog();

                    ofdLoadFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_213) + "（*.sb）|*.sb";
                    ofdLoadFile.RestoreDirectory = true;

                    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = ofdLoadFile.FileName;

                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            ImportSystemBackUp(FilePath, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void ImportSystemBackUp(string FilePath, bool LoadFromUser)
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
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.SystemBackUp_Import);
                                pwForm.ShowDialog();
                            }

                            xdoc = Socket_Operation.DecryptXMLFile(FilePath, Socket_Cache.System.AESKey);
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
                            ImportSystemBackUp_FromXDocument(xdoc);

                            if (bEncrypt)
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_216));
                            }
                            else
                            {
                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_215));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void ImportSystemBackUp_FromXDocument(XDocument xdoc)
            {
                #region//有效性检测

                string RootName = xdoc.Root.Name.LocalName;
                if (!RootName.Equals("WPE64_BackUp"))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_217));
                    return;
                }

                #endregion

                #region//系统设置

                try
                {                    
                    XElement xeSystemConfig = xdoc.Root.Element("SystemConfig");
                    if (xeSystemConfig != null)
                    {
                        SetSystemConfig_FromXML(xeSystemConfig);
                        SaveSystemConfig_ToDB();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog("Import SystemConfig", ex.Message);
                }

                #endregion

                #region//代理设置

                try
                {                    
                    XElement xeProxyConfig = xdoc.Root.Element("ProxyConfig");
                    if (xeProxyConfig != null)
                    {
                        SetProxyConfig_FromXML(xeProxyConfig);
                        Socket_Cache.DataBase.DeleteTable_RunConfig();
                        Socket_Cache.DataBase.InsertTable_RunConfig();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog("Import ProxyConfig", ex.Message);
                }

                #endregion

                #region//注入设置

                try
                {                    
                    XElement xeInjectionConfig = xdoc.Root.Element("InjectionConfig");
                    if (xeInjectionConfig != null)
                    {
                        SetInjectionConfig_FromXML(xeInjectionConfig);
                        Socket_Cache.DataBase.DeleteTable_RunConfig();
                        Socket_Cache.DataBase.InsertTable_RunConfig();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog("Import InjectionConfig", ex.Message);
                }

                #endregion

                #region//代理账号

                try
                {                    
                    XElement xeProxyAccountList = xdoc.Root.Element("ProxyAccountList");                    
                    if (xeProxyAccountList != null)
                    {
                        XDocument ProxyAccountList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        ProxyAccountList.Add(xeProxyAccountList);

                        Socket_Cache.ProxyAccount.LoadProxyAccountList_FromXDocument(ProxyAccountList);
                        Socket_Cache.DataBase.DeleteTable_ProxyAccount();
                        Socket_Cache.DataBase.InsertTable_ProxyAccount();
                        Socket_Cache.DataBase.DeleteTable_ProxyAccount_LoginInfo();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog("Import ProxyAccountList", ex.Message);
                }

                #endregion

                #region//代理映射

                try
                {
                    //本地代理映射
                    XElement xeMapLocal = xdoc.Root.Element("MapLocal");
                    if (xeMapLocal != null)
                    {
                        XDocument MapLocal = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        MapLocal.Add(xeMapLocal);

                        Socket_Cache.ProxyMapping.LoadMapLocal_FromXDocument(MapLocal);
                        Socket_Cache.DataBase.DeleteTable_ProxyMapLocal();
                        Socket_Cache.DataBase.InsertTable_ProxyMapLocal();                        
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog("Import ProxyAccountList", ex.Message);
                }

                #endregion

                #region//滤镜列表

                try
                {                    
                    XElement xeFilterList = xdoc.Root.Element("FilterList");
                    if (xeFilterList != null)
                    {
                        XDocument FilterList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        FilterList.Add(xeFilterList);

                        Socket_Cache.FilterList.LoadFilterList_FromXDocument(FilterList);
                        Socket_Cache.FilterList.SaveFilterList_ToDB();
                        Socket_Cache.FilterList.FilterListClear();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog("Import FilterList", ex.Message);
                }

                #endregion

                #region//发送列表

                try
                {                    
                    XElement xeSendList = xdoc.Root.Element("SendList");
                    if (xeSendList != null)
                    {
                        XDocument SendList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        SendList.Add(xeSendList);

                        Socket_Cache.SendList.LoadSendList_FromXDocument(SendList);
                        Socket_Cache.SendList.SaveSendList_ToDB();
                        Socket_Cache.SendList.SendListClear();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog("Import SendList", ex.Message);
                }

                #endregion

                #region//机器人列表

                try
                {                    
                    XElement xeRobotList = xdoc.Root.Element("RobotList");
                    if (xeRobotList != null)
                    {
                        XDocument RobotList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        RobotList.Add(xeRobotList);

                        Socket_Cache.RobotList.LoadRobotList_FromXDocument(RobotList);
                        Socket_Cache.RobotList.SaveRobotList_ToDB();
                        Socket_Cache.RobotList.RobotListClear();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog("Import RobotList", ex.Message);
                }

                #endregion
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
            public static bool Enable_ExternalProxy = false, Enable_ExternalProxy_AppointPort = false, Enable_ExternalProxy_Auth = false;
            public static string ExternalProxy_IP = "127.0.0.1";
            public static ushort ExternalProxy_Port = 8889;
            public static string ExternalProxy_AppointPort = "80,8080,443,8443", ExternalProxy_UserName, ExternalProxy_PassWord;            
            public static ushort ProxyPort = 1080;
            public static int UDPCloseTime = 60;
            public static long Total_Request = 0;
            public static long Total_Response = 0;
            public static int MaxChartPoint = 100;
            public const long MaxNetworkSpeed = 100000;
            public static string ProxyOnLineInfo = string.Empty;
            public static string ProxyBytesInfo = string.Empty;
            public static string ProxySpeedInfo = string.Empty;

            public static readonly ConcurrentDictionary<string, IPAddress> DnsCache = new ConcurrentDictionary<string, IPAddress>(StringComparer.OrdinalIgnoreCase);
            public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);            

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
                Invalid = 0,
                IPv4 = 1,
                Domain = 3,
                IPv6 = 4,
            }

            public enum DomainType : byte
            {
                Socket = 0,
                Http = 1,
                Https = 2,
                External = 3,
            }

            public enum MapProtocol : byte
            {
                Http = 0,
                Https = 1,
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

            #endregion

            #region//接收客户端请求

            public static void HandleClient(Socket clientSocket)
            {
                try
                {
                    Socket_ProxyTCP spt = new Socket_ProxyTCP(clientSocket, clientSocket.ReceiveBufferSize);
                    Socket_Cache.SocketProxy.StartReceive(spt);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void StartReceive(Socket_ProxyTCP spt)
            {
                try
                {
                    var args = new SocketAsyncEventArgs();
                    args.SetBuffer(spt.Client.Buffer, 0, spt.Client.Buffer.Length);
                    args.UserToken = spt;
                    args.Completed += ReceiveCompleted;

                    if (!spt.Client.Socket.ReceiveAsync(args))
                    {
                        Socket_Cache.SocketProxy.ReceiveCompleted(spt.Client.Socket, args);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                    spt.Client.Close();
                }
            }

            private static void ReceiveCompleted(object sender, SocketAsyncEventArgs args)
            {
                Socket_ProxyTCP spt = (Socket_ProxyTCP)args.UserToken;

                try
                {
                    if (args.SocketError != SocketError.Success || args.BytesTransferred <= 0)
                    {
                        spt.Client.Close();
                        return;
                    }

                    if (Socket_Cache.SocketProxy.IsListening)
                    {
                        int bytesRead = args.BytesTransferred;
                        ReadOnlySpan<byte> proxyBufferSpan = spt.Client.Buffer.AsSpan(0, bytesRead);
                        Span<byte> combinedData = new byte[spt.Client.Data.Length + bytesRead].AsSpan();

                        if (spt.Client.Data.Length > 0)
                        {
                            spt.Client.Data.AsSpan().CopyTo(combinedData);
                        }
                        proxyBufferSpan.CopyTo(combinedData.Slice(spt.Client.Data.Length));

                        bool bIsMatch = Socket_Operation.CheckDataIsMatchProxyStep(combinedData, spt.ProxyStep);
                        if (bIsMatch)
                        {
                            switch (spt.ProxyStep)
                            {
                                case Socket_Cache.SocketProxy.ProxyStep.Handshake:
                                    Socket_Cache.SocketProxy.Handshake(spt, combinedData);
                                    break;

                                case Socket_Cache.SocketProxy.ProxyStep.AuthUserName:
                                    Socket_Cache.SocketProxy.AuthUserName(spt, combinedData);
                                    break;

                                case Socket_Cache.SocketProxy.ProxyStep.Command:
                                    Socket_Cache.SocketProxy.Command(spt, combinedData);
                                    break;

                                case Socket_Cache.SocketProxy.ProxyStep.ForwardData:
                                    Socket_Cache.SocketProxy.ForwardData(spt, combinedData);
                                    break;
                            }

                            spt.Client.Data = Array.Empty<byte>();
                        }
                        else
                        {
                            spt.Client.Data = combinedData.ToArray();
                        }

                        Socket_Cache.SocketProxy.StartReceive(spt);
                    }
                }
                catch (SocketException ex) when (Socket_Operation.IsExpectedSocketError(ex.ErrorCode))
                {
                    spt.Client.Close();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                    spt.Client.Close();
                }
                finally
                {
                    args.Dispose();
                }
            }            

            #endregion

            #region//握手过程

            private static void Handshake(Socket_ProxyTCP spt, ReadOnlySpan<byte> bData)
            {
                try
                {
                    spt.ProxyType = (Socket_Cache.SocketProxy.ProxyType)bData[0];

                    if (spt.ProxyType == Socket_Cache.SocketProxy.ProxyType.Socket5)
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
                            Socket_Operation.SendTCPData(spt.Client.Socket, bAuth);

                            if (atServer == Socket_Cache.SocketProxy.AuthType.UserName)
                            {
                                spt.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.AuthUserName;

                                if (bData.Length > iMETHODS_COUNT + 2)
                                {
                                    ReadOnlySpan<byte> bAuthDate = bData.Slice(iMETHODS_COUNT + 2);

                                    bool bIsMatch = Socket_Operation.CheckDataIsMatchProxyStep(bAuthDate, Socket_Cache.SocketProxy.ProxyStep.AuthUserName);
                                    if (bIsMatch)
                                    {
                                        Socket_Cache.SocketProxy.AuthUserName(spt, bAuthDate);
                                    }
                                }
                            }
                            else
                            {
                                spt.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Command;
                            }
                        }                        
                    }
                    else
                    {
                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_145), spt.ProxyType);
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

            private static void AuthUserName(Socket_ProxyTCP spt, ReadOnlySpan<byte> bData)
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
                        string ClientIP = spt.Client.EndPoint.Address.ToString();

                        bool bAuthOK = Socket_Cache.ProxyAccount.CheckUserNameAndPassWord(sUserName, sPassWord, out Guid AccountID);

                        Span<byte> bAuth = stackalloc byte[2];
                        bAuth[0] = 0x01;

                        bool isAllowed = 
                            bAuthOK && 
                            !Socket_Cache.ProxyAccount.CheckLimitLinks_ByAccountID(AccountID) &&
                            !Socket_Cache.ProxyAccount.CheckLimitDevices_ByAccountID(AccountID, ClientIP);

                        bAuth[1] = isAllowed ? (byte)0x00 : (byte)0x01;
                        if (isAllowed)
                        {
                            Socket_Cache.ProxyAccount.SetOnline_ByAccountID(AccountID, true);
                            Socket_Cache.ProxyAccount.RecordLoginIP_ByAccountID(AccountID, ClientIP);
                            Socket_Cache.SocketProxy.AuthResult_ToList(AccountID, ClientIP, bAuthOK);

                            spt.AID = AccountID;
                            spt.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Command;
                        }

                        Socket_Operation.SendTCPData(spt.Client.Socket, bAuth);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//执行命令

            private static void Command(Socket_ProxyTCP spt, ReadOnlySpan<byte> bData)
            {
                try
                {
                    spt.ProxyType = (Socket_Cache.SocketProxy.ProxyType)bData[0];
                    spt.CommandType = (Socket_Cache.SocketProxy.CommandType)bData[1];
                    spt.AddressType = (Socket_Cache.SocketProxy.AddressType)bData[3];

                    if (spt.ProxyType == Socket_Cache.SocketProxy.ProxyType.Socket5)
                    {
                        try
                        {
                            ReadOnlySpan<byte> bADDRESS = bData.Slice(4, bData.Length - 4);
                            ReadOnlySpan<byte> bServerTCP_IP = Socket_Cache.SocketProxy.ProxyTCP_IP.GetAddressBytes();
                            ReadOnlySpan<byte> bServerTCP_Port = BitConverter.GetBytes(Socket_Cache.SocketProxy.ProxyPort);
                                                        
                            IPEndPoint epServer = Socket_Operation.GetIPEndPoint_ByAddressType(spt.AddressType, bADDRESS, out string AddressString);
                            spt.Server.Socket = new Socket(epServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                            spt.Server.EndPoint = epServer;
                            ushort uPort = ((ushort)epServer.Port);

                            spt.DomainType = Socket_Operation.GetDomainType_ByPort(uPort);
                            spt.Server.Address = Socket_Operation.GetServerAddress(spt.DomainType, AddressString, uPort);
                            spt.Client.Address = Socket_Operation.GetClientAddress(spt.Client.Socket, AddressString, uPort);                            

                            switch (spt.CommandType)
                            {
                                case Socket_Cache.SocketProxy.CommandType.Connect:

                                    #region//代理 TCP

                                    switch (spt.DomainType)
                                    {
                                        case Socket_Cache.SocketProxy.DomainType.External:

                                            try
                                            {
                                                IPEndPoint ExternalProxyEP = Socket_Operation.GetIPEndPoint_ByAddressString(Socket_Cache.SocketProxy.ExternalProxy_IP, Socket_Cache.SocketProxy.ExternalProxy_Port);
                                                if (ExternalProxyEP == null)
                                                {
                                                    return;
                                                }

                                                var connectResult = spt.Server.Socket.BeginConnect(ExternalProxyEP, null, null);
                                                if (!connectResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5)))
                                                {
                                                    return;
                                                }
                                                spt.Server.Socket.EndConnect(connectResult);

                                                byte[] handshakeRequest = null;
                                                if (Socket_Cache.SocketProxy.Enable_ExternalProxy_Auth)
                                                {
                                                    handshakeRequest = new byte[] { 0x05, 0x02, 0x00, 0x02 };
                                                }
                                                else
                                                {
                                                    handshakeRequest = new byte[] { 0x05, 0x01, 0x00 };
                                                }
                                                spt.Server.Socket.Send(handshakeRequest);

                                                byte[] handshakeResponse = new byte[2];
                                                spt.Server.Socket.Receive(handshakeResponse);

                                                if (handshakeResponse[0] != 0x05)
                                                {
                                                    return;
                                                }

                                                switch (handshakeResponse[1])
                                                {
                                                    case 0x00:
                                                        break;

                                                    case 0x02:

                                                        if (!Socket_Cache.SocketProxy.Enable_ExternalProxy_Auth)
                                                        {
                                                            return;
                                                        }

                                                        byte[] AuthRequest = Socket_Operation.CreateSOCKS5AuthPacket(Socket_Cache.SocketProxy.ExternalProxy_UserName, Socket_Cache.SocketProxy.ExternalProxy_PassWord);
                                                        if (AuthRequest == null)
                                                        {
                                                            return;
                                                        }
                                                        spt.Server.Socket.Send(AuthRequest);

                                                        byte[] AuthResponse = new byte[2];
                                                        spt.Server.Socket.Receive(AuthResponse);

                                                        if (AuthResponse[1] != 0x00)
                                                        {
                                                            return;
                                                        }

                                                        break;

                                                    default:
                                                        return;
                                                }

                                                spt.Server.Socket.Send(bData.ToArray());

                                                byte[] connectResponse = new byte[10];
                                                spt.Server.Socket.Receive(connectResponse);

                                                if (connectResponse[1] != 0x00)
                                                {
                                                    Socket_Operation.SendTCPData(spt.Client.Socket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                                    return;
                                                }

                                                Socket_Cache.SocketProxy.StartServerReceive(spt);
                                                spt.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                Socket_Operation.SendTCPData(spt.Client.Socket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));

                                                Socket_Cache.SocketProxyQueue.ProxyTCP_ToQueue(spt);
                                            }
                                            catch (SocketException)
                                            {
                                                Socket_Operation.SendTCPData(spt.Client.Socket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                            }                                            

                                            break;

                                        case Socket_Cache.SocketProxy.DomainType.Http:
                                        case Socket_Cache.SocketProxy.DomainType.Https:
                                        case Socket_Cache.SocketProxy.DomainType.Socket:

                                            try
                                            {
                                                spt.Server.Socket.Connect(spt.Server.EndPoint);
                                                Socket_Cache.SocketProxy.StartServerReceive(spt);
                                                spt.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                                                Socket_Operation.SendTCPData(spt.Client.Socket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));

                                                Socket_Cache.SocketProxyQueue.ProxyTCP_ToQueue(spt);
                                            }
                                            catch (SocketException)
                                            {
                                                Socket_Operation.SendTCPData(spt.Client.Socket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
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
                                        Socket_Cache.SocketProxy.StartUdpReceive(spu);
                                        Socket_Cache.SocketProxyQueue.ProxyUDP_ToQueue(spu);

                                        ReadOnlySpan<byte> bServerUDP_IP = Socket_Cache.SocketProxy.ProxyUDP_IP.GetAddressBytes();
                                        ReadOnlySpan<byte> bServerUDP_Port = BitConverter.GetBytes(((IPEndPoint)spu.ClientUDP.Client.LocalEndPoint).Port);                                        

                                        Socket_Operation.SendTCPData(spt.Client.Socket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Success, bServerUDP_IP, bServerUDP_Port));                                        
                                    }
                                    catch (SocketException)
                                    {
                                        Socket_Operation.SendTCPData(spt.Client.Socket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                    }

                                    #endregion

                                    break;

                                default:

                                    #region//不支持的命令

                                    Socket_Operation.SendTCPData(spt.Client.Socket, Socket_Operation.GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse.Unsupport, bServerTCP_IP, bServerTCP_Port));

                                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_152), spt.Client.Socket.RemoteEndPoint, spt.CommandType);
                                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);

                                    #endregion

                                    break;
                            }
                        }
                        catch (SocketException ex)
                        {
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spt.Server.Address + " - " + ex.Message);
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

            private static void ForwardData(Socket_ProxyTCP spt, ReadOnlySpan<byte> bData)
            {
                try
                {
                    if (spt.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                    {
                        bool enableProxyQueue = false;
                        switch (spt.DomainType)
                        {
                            case Socket_Cache.SocketProxy.DomainType.Http:

                                if (Socket_Cache.ProxyMapping.Enable_MapLocal)
                                {
                                    #region//Http代理映射

                                    string request = Encoding.ASCII.GetString(bData.ToArray());
                                    if (request.StartsWith("GET") || request.StartsWith("POST") || request.StartsWith("HEAD") || request.StartsWith("PUT"))
                                    {
                                        var headers = Socket_Operation.ParseHttpHeaders(request);
                                        if (headers.TryGetValue("Host", out string hostHeader))
                                        {
                                            string requestPath = request.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                                            string cleanPath = requestPath.Split('?')[0];

                                            var rule = Socket_Cache.ProxyMapping.GetMapLocal(Socket_Cache.SocketProxy.MapProtocol.Http, hostHeader.Split(':')[0], spt.Server.EndPoint.Port, cleanPath);
                                            if (rule != null)
                                            {
                                                if (File.Exists(rule.LocalPath))
                                                {
                                                    byte[] fileBytes = File.ReadAllBytes(rule.LocalPath);
                                                    string contentType = Socket_Operation.GetContentType(Path.GetExtension(rule.LocalPath));

                                                    string response =
                                                        $"HTTP/1.1 200 OK\r\n" +
                                                        $"Content-Type: {contentType}\r\n" +
                                                        $"Content-Length: {fileBytes.Length}\r\n" +
                                                        "Connection: close\r\n\r\n";

                                                    byte[] headerBytes = Encoding.UTF8.GetBytes(response);
                                                    Socket_Operation.SendTCPData(spt.Client.Socket, headerBytes);
                                                    Socket_Operation.SendTCPData(spt.Client.Socket, fileBytes);                                                    
                                                }
                                                else
                                                {
                                                    Socket_Operation.Send404Response(spt.Client.Socket);
                                                }                                                    
                                            }
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    Socket_Operation.SendTCPData(spt.Server.Socket, bData);
                                }

                                enableProxyQueue = true;
                                break;

                            case Socket_Cache.SocketProxy.DomainType.Https:
                                
                                Socket_Operation.SendTCPData(spt.Server.Socket, bData);
                                enableProxyQueue = true;

                                break;

                            case Socket_Cache.SocketProxy.DomainType.Socket:
                                
                                Socket_Operation.SendTCPData(spt.Server.Socket, bData);
                                enableProxyQueue = true;

                                break;

                            case Socket_Cache.SocketProxy.DomainType.External:
                                
                                Socket_Operation.SendTCPData(spt.Server.Socket, bData);
                                enableProxyQueue = true;

                                break;
                        }

                        if (enableProxyQueue)
                        {
                            Socket_Cache.SocketProxyQueue.ProxyData_ToQueue(spt, bData, Socket_Cache.SocketProxy.DataType.Request);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spt.Client.Address + " - " + ex.Message);
                }
            }

            #endregion

            #region//响应数据（TCP）

            private static void StartServerReceive(Socket_ProxyTCP spt)
            {
                try
                {
                    var args = new SocketAsyncEventArgs();
                    args.SetBuffer(spt.Server.Buffer, 0, spt.Server.Buffer.Length);
                    args.UserToken = spt;
                    args.Completed += Socket_Cache.SocketProxy.ServerReceiveCompleted;

                    if (!spt.Server.Socket.ReceiveAsync(args))
                    {
                        Socket_Cache.SocketProxy.ServerReceiveCompleted(spt.Server.Socket, args);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void ServerReceiveCompleted(object sender, SocketAsyncEventArgs args)
            {
                Socket_ProxyTCP spt = (Socket_ProxyTCP)args.UserToken;

                try
                {
                    if (args.SocketError != SocketError.Success || args.BytesTransferred <= 0)
                    {
                        spt.Server.Close();
                        return;
                    }

                    int bytesRead = args.BytesTransferred;
                    ReadOnlySpan<byte> receivedData = spt.Server.Buffer.AsSpan(0, bytesRead);

                    if (spt.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                    {
                        Socket_Operation.SendTCPData(spt.Client.Socket, receivedData);

                        bool enableProxyQueue = false;
                        switch (spt.DomainType)
                        {
                            case Socket_Cache.SocketProxy.DomainType.Http:
                                enableProxyQueue = true;
                                break;

                            case Socket_Cache.SocketProxy.DomainType.Https:
                                enableProxyQueue = true;
                                break;

                            case Socket_Cache.SocketProxy.DomainType.Socket:
                                enableProxyQueue = true;
                                break;

                            case Socket_Cache.SocketProxy.DomainType.External:
                                enableProxyQueue = true;
                                break;
                        }

                        if (enableProxyQueue)
                        {
                            Socket_Cache.SocketProxyQueue.ProxyData_ToQueue(spt, receivedData, Socket_Cache.SocketProxy.DataType.Response);
                        }
                    }

                    Socket_Cache.SocketProxy.StartServerReceive(spt);
                }
                catch (SocketException ex) when (Socket_Operation.IsExpectedSocketError(ex.ErrorCode))
                {
                    spt.Server.Close();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spt.Server.Address + " - " + ex.Message);
                    spt.Server.Close();
                }
                finally
                {
                    args.Dispose();
                }
            }

            #endregion

            #region//UDP 中继

            private static void StartUdpReceive(Socket_ProxyUDP spu)
            {
                try
                {
                    if (spu.ClientUDP != null)
                    {
                        spu.ClientUDP.BeginReceive(new AsyncCallback(UdpReceiveCallback), spu);
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void UdpReceiveCallback(IAsyncResult ar)
            {
                Socket_ProxyUDP spu = (Socket_ProxyUDP)ar.AsyncState;

                try
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receivedData = Socket_Operation.ReceiveUDPData(spu.ClientUDP, ar, ref remoteEndPoint);
                    ReadOnlySpan<byte> bData = receivedData.AsSpan();

                    if (!bData.IsEmpty && !remoteEndPoint.Address.Equals(IPAddress.Any) && remoteEndPoint.Port != 0)
                    {
                        if (bData[0] == 0 && bData[1] == 0 && bData[2] == 0)
                        {
                            Socket_Cache.SocketProxy.AddressType addressType = (Socket_Cache.SocketProxy.AddressType)bData[3];

                            if (addressType == Socket_Cache.SocketProxy.AddressType.IPv4 ||
                                addressType == Socket_Cache.SocketProxy.AddressType.IPv6 ||
                                addressType == Socket_Cache.SocketProxy.AddressType.Domain)
                            {
                                spu.ClientUDP_EndPoint = remoteEndPoint;

                                ReadOnlySpan<byte> bADDRESS = bData.Slice(4, bData.Length - 4);
                                IPEndPoint targetEndPoint = Socket_Operation.GetIPEndPoint_ByAddressType(addressType, bADDRESS, out string AddressString);

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
                            bResponseData[3] = (byte)Socket_Cache.SocketProxy.AddressType.IPv4;
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
                        Socket_Cache.SocketProxy.StartUdpReceive(spu);
                    }
                }                
                catch (SocketException ex) when (Socket_Operation.IsExpectedSocketError(ex.ErrorCode))
                {
                    //
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                    Socket_Cache.SocketProxy.StartUdpReceive(spu);
                }
            }

            #endregion            

            #region//代理认证入列表            

            public static void AuthResult_ToList(Guid AID, string IPAddress, bool AuthResult)
            {
                try
                {
                    if (AID != null && AID != Guid.Empty)
                    {
                        Proxy_AuthInfo pai = new Proxy_AuthInfo(AID, IPAddress, AuthResult, DateTime.Now);
                        RecProxyAuth?.Invoke(pai);
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion                        

            #region//查找代理认证

            public static Proxy_AuthInfo GetProxyAuthInfo_ByAccountID(Guid AID)
            {
                try
                {
                    if (AID != null)
                    {
                        Proxy_AuthInfo pai = Socket_Cache.SocketProxy.lstProxyAuth.FirstOrDefault(Auth => Auth.AID == AID);

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

            public static Proxy_AuthInfo GetProxyAuthInfo_ByIPAddress(string IPAddress)
            {
                try
                {
                    if (string.IsNullOrEmpty(IPAddress))
                    {
                        return null;
                    }

                    return Socket_Cache.SocketProxy.lstProxyAuth.FirstOrDefault(p => p.IPAddress.Equals(IPAddress, StringComparison.OrdinalIgnoreCase));
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            public static Proxy_AuthInfo GetProxyAuthInfo_ByAIDandIP(Guid AID, string IPAddress)
            {
                try
                {
                    if (string.IsNullOrEmpty(IPAddress))
                    {
                        return null;
                    }

                    return Socket_Cache.SocketProxy.lstProxyAuth.FirstOrDefault(p => p.AID == AID && p.IPAddress.Equals(IPAddress, StringComparison.OrdinalIgnoreCase));
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            #endregion

            #region//删除代理认证

            public static void DeleteProxyAuthInfo_ByIPAndAID(string IPAddress, Guid AID)
            {
                try
                {
                    if (!string.IsNullOrEmpty(IPAddress) && AID != null)
                    {
                        Proxy_AuthInfo pai = Socket_Cache.SocketProxy.lstProxyAuth.FirstOrDefault(Auth => Auth.IPAddress.Equals(IPAddress, StringComparison.OrdinalIgnoreCase) && Auth.AID == AID);

                        if (pai != null)
                        {
                            Socket_Cache.SocketProxy.lstProxyAuth.Remove(pai);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//获取客户端的IP地址

            public static string GetClientIPAddress(Socket_ProxyTCP spt)
            {
                try
                {
                    if (spt != null && spt.Client.EndPoint != null)
                    {
                        return spt.Client.EndPoint.Address.ToString();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return string.Empty;
            }

            #endregion            

            #region//更新 UDP 状态（异步）

            public static async Task UpdateProxyUDP()
            {
                await Task.Run(() =>
                {
                    try
                    {
                        DateTime dtNow = DateTime.Now;

                        for (int i = Socket_Cache.SocketProxyList.lstProxyUDP.Count - 1; i >= 0; i--)
                        {
                            var spu = Socket_Cache.SocketProxyList.lstProxyUDP[i];
                            if (spu.ClientUDP != null && spu.ClientUDP_Time != null)
                            {
                                TimeSpan timeSpan = dtNow - spu.ClientUDP_Time;
                                if (timeSpan.TotalSeconds > Socket_Cache.SocketProxy.UDPCloseTime)
                                {
                                    spu.CloseUDPClient();
                                    Socket_Cache.SocketProxyList.ClearUDP(spu);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                });                
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
                qSocket_ProxyTCP.Enqueue(spc);
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
                        Socket_ProxyData spd = new Socket_ProxyData(spc.Server.Address, spc.DomainType, bData.ToArray(), DataType);
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

            #region//查找TCP代理列表

            public static Socket_ProxyTCP GetProxyTCP_ByAccountID(Guid AID)
            {
                try
                {
                    if (AID != null)
                    {
                        Socket_ProxyTCP spt = Socket_Cache.SocketProxyList.lstProxyTCP.FirstOrDefault(ProxyTCP => ProxyTCP.AID == AID);

                        if (spt != null)
                        {
                            return spt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            #endregion            

            #region//清除 TCP 列表中的指定数据

            public static void ClearTCP(Socket_ProxyTCP spt)
            {
                Socket_Cache.SocketProxyList.lstProxyTCP.Remove(spt);
            }

            #endregion

            #region//清除 UDP 列表中的指定数据

            public static void ClearUDP(Socket_ProxyUDP spu)
            {
                Socket_Cache.SocketProxyList.lstProxyUDP.Remove(spu);
            }

            #endregion

            #region//清空整个列表

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
            public static int OnLineTimeOut = 60;
            public static string AESKey = string.Empty;
            public static string CCProxy_HTML = string.Empty;
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

            #region//检测是否已超过限制链接数

            public static bool CheckLimitLinks_ByAccountID(Guid AID)
            {
                try
                {
                    if (AID != null && AID != Guid.Empty)
                    {
                        Proxy_AccountInfo paiAccount = Socket_Cache.ProxyAccount.GetProxyAccount_ByAccountID(AID);                        
                        Proxy_AuthInfo paiAuth = Socket_Cache.SocketProxy.GetProxyAuthInfo_ByAccountID(AID);

                        if (paiAccount != null && paiAuth != null)
                        {
                            if (paiAccount.IsLimitLinks)
                            {
                                int LimitLinks = paiAccount.LimitLinks;
                                int LinksNumber = paiAuth.LinksNumber;

                                if (LinksNumber >= LimitLinks)
                                {
                                    return true;
                                }
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

            #region//检测是否已超过限制设备数

            public static bool CheckLimitDevices_ByAccountID(Guid AID, string ClientIP)
            {
                try
                {
                    if (AID != null && AID != Guid.Empty)
                    {
                        Proxy_AccountInfo paiAccount = Socket_Cache.ProxyAccount.GetProxyAccount_ByAccountID(AID);
                        if (paiAccount != null)
                        {
                            if (paiAccount.IsLimitDevices)
                            {
                                int DevicesNumber = Socket_Cache.ProxyAccount.GetDevicesNumber_ByAccountID(AID);

                                if (DevicesNumber < paiAccount.LimitDevices)
                                {
                                    return false;
                                }
                                else if (DevicesNumber == paiAccount.LimitDevices)
                                {
                                    Proxy_AuthInfo pai = Socket_Cache.SocketProxy.GetProxyAuthInfo_ByAIDandIP(AID, ClientIP);

                                    if (pai != null)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                                else
                                {
                                    return true;
                                }
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

            public static void SetOnline_ByAccountID(Guid AccountID, bool IsOnline)
            {
                try
                {
                    foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                    {
                        if (pai.AID.Equals(AccountID))
                        {                            
                            pai.IsOnLine = IsOnline;

                            if (IsOnline)
                            { 
                                pai.LoginTime = DateTime.Now;
                            }

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

            #region//获取代理账号的链接数

            public static int GetLinksNumber_ByAccountID(Guid AID, string ClientIP, TreeNodeCollection nodes)
            {
                int iReturn = 0;

                try
                {
                    string ClientUserName = Socket_Cache.ProxyAccount.GetUserName_ByAccountID(AID);
                    string RootName = Socket_Operation.GetClientListName(ClientIP, ClientUserName);

                    TreeNode RootNode = Socket_Operation.FindNodeSync(nodes, RootName);
                    if (RootNode != null)
                    {
                        iReturn = RootNode.Nodes.Count;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;                
            }

            #endregion

            #region//获取代理账号登录的设备数

            public static int GetDevicesNumber_ByAccountID(Guid AID)
            {
                return Socket_Cache.SocketProxy.lstProxyAuth.Count(p => p.AID == AID);
            }

            #endregion

            #region//更新所有代理账号的在线状态（异步）

            public static async Task UpdateOnlineStatus()
            {
                await Task.Run(() =>
                {
                    try
                    {
                        DateTime dtNow = DateTime.Now;

                        foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                        {
                            if (pai.IsOnLine)
                            {
                                if (pai.LoginTime != null)
                                {
                                    TimeSpan timeDiff = dtNow - pai.LoginTime;

                                    if (timeDiff.TotalMinutes > Socket_Cache.ProxyAccount.OnLineTimeOut)
                                    {
                                        pai.IsOnLine = false;
                                    }
                                }
                                else
                                {
                                    pai.IsOnLine = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                });                
            }

            #endregion

            #region//记录代理账号的IP地址（异步）

            public static async void RecordLoginIP_ByAccountID(Guid AccountID, string IPAddress)
            {
                try
                {
                    if (AccountID != Guid.Empty && !string.IsNullOrEmpty(IPAddress))
                    {
                        Proxy_AccountInfo paiItem = Socket_Cache.ProxyAccount.lstProxyAccount.FirstOrDefault(item => item.AID == AccountID);

                        if (paiItem != null)
                        {
                            if (paiItem.LoginIP != IPAddress)
                            {
                                paiItem.LoginIP = IPAddress;
                                paiItem.IPLocation = await Socket_Operation.GetIPLocation(IPAddress);

                                Socket_Cache.ProxyAccount.SaveProxyAccount_LoginInfo_ToDB(paiItem);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(paiItem.IPLocation))
                                {
                                    paiItem.IPLocation = await Socket_Operation.GetIPLocation(IPAddress);
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

            #endregion

            #region//新增代理账号

            public static bool AddProxyAccount(
                Guid AID, 
                bool IsEnable, 
                string UserName, 
                string PassWord, 
                DateTime LoginTime, 
                string LoginIP, 
                string IPLocation, 
                bool IsLimitLinks,
                int LimitLinks,
                bool IsLimitDevices,
                int LimitDevices,
                bool IsExpiry, 
                DateTime ExpiryTime, 
                DateTime CreateTime)
            {
                try
                {
                    if (AID != Guid.Empty && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PassWord))
                    {
                        if (!Socket_Cache.ProxyAccount.CheckProxyAccount_Exist(UserName))
                        {
                            Proxy_AccountInfo pai = new Proxy_AccountInfo(
                                AID, 
                                IsEnable, 
                                UserName, 
                                PassWord, 
                                LoginTime, 
                                LoginIP, 
                                IPLocation, 
                                IsLimitLinks, 
                                LimitLinks, 
                                IsLimitDevices,
                                LimitDevices,
                                IsExpiry, 
                                ExpiryTime, 
                                CreateTime);

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

            public static bool UpdateProxyAccount_ByAccountID(
                Guid AID, 
                bool IsEnable, 
                string PassWord,
                bool IsLimitLinks,
                int LimitLinks,
                bool IsLimitDevices,
                int LimitDevices,
                bool IsExpiry, 
                DateTime ExpiryTime)
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
                            pai.IsLimitLinks = IsLimitLinks;
                            pai.LimitLinks = LimitLinks;
                            pai.IsExpiry = IsExpiry;
                            pai.IsLimitDevices = IsLimitDevices;
                            pai.LimitDevices = LimitDevices;
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

            public static bool UpdateProxyAccount_ByUserName(
                string UserName, 
                bool IsEnable, 
                string PassWord,
                bool IsLimitLinks,
                int LimitLinks,
                bool IsLimitDevices,
                int LimitDevices,
                bool IsExpiry, 
                DateTime ExpiryTime)
            {
                try
                {
                    if (!string.IsNullOrEmpty(UserName))
                    {
                        var pai = Socket_Cache.ProxyAccount.lstProxyAccount.FirstOrDefault(account => account.UserName == UserName);

                        if (pai != null)
                        {
                            Socket_Cache.ProxyAccount.UpdateProxyAccount_ByAccountID(pai.AID, IsEnable, PassWord, IsLimitLinks, LimitLinks, IsLimitDevices, LimitDevices, IsExpiry, ExpiryTime);
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

            public static void DeleteProxyAccount_Dialog(List<Guid> gList)
            {
                try
                {
                    if (gList.Count > 0)
                    {
                        DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                        if (dr.Equals(DialogResult.OK))
                        {
                            foreach (Guid gAID in gList)
                            {
                                Socket_Cache.ProxyAccount.DeleteProxyAccount_ByAccountID(gAID);
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

            public static bool DeleteProxyAccount_ByUserName(string UserName)
            {
                try
                {
                    if (!string.IsNullOrEmpty(UserName))
                    {
                        var pai = Socket_Cache.ProxyAccount.lstProxyAccount.FirstOrDefault(account => account.UserName == UserName);

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

            public static string GetUserName_ByAccountID(Guid AID)
            {
                try
                {
                    if (AID != null)
                    {
                        Proxy_AccountInfo pai = Socket_Cache.ProxyAccount.lstProxyAccount.FirstOrDefault(account => account.AID == AID);

                        if (pai != null)
                        {
                            return pai.UserName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return string.Empty;
            }

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
                    else
                    { 
                        return Socket_Cache.ProxyAccount.lstProxyAccount;
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

            public static BindingList<Proxy_AccountInfo> GetProxyAccount_ByIsLimitLinks(bool IsLimitLinks)
            {
                try
                {
                    BindingList<Proxy_AccountInfo> pai = new BindingList<Proxy_AccountInfo>
                        (lstProxyAccount.Where(account => account.IsLimitLinks == IsLimitLinks).ToList());

                    return pai;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            public static BindingList<Proxy_AccountInfo> GetProxyAccount_ByIsLimitDevices(bool IsLimitDevices)
            {
                try
                {
                    BindingList<Proxy_AccountInfo> pai = new BindingList<Proxy_AccountInfo>
                        (lstProxyAccount.Where(account => account.IsLimitDevices == IsLimitDevices).ToList());

                    return pai;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            public static BindingList<Proxy_AccountInfo> GetProxyAccount_ByExpireTime(DateTime dtFrom, DateTime dtTo)
            {
                try
                {
                    BindingList<Proxy_AccountInfo> pai = new BindingList<Proxy_AccountInfo>
                        (lstProxyAccount.Where(account => account.ExpiryTime >= dtFrom && account.ExpiryTime <= dtTo).ToList());

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
                    else
                    {
                        Socket_Cache.ProxyAccount.lstProxyAccount.Add(pai);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//代理账号加时

            public static void ProxyAccountAddTime_Dialog(List<Guid> gList, int Hours)
            {
                try
                {
                    if (gList.Count > 0)
                    {
                        DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_18));

                        if (dr.Equals(DialogResult.OK))
                        {
                            foreach (Guid gAID in gList)
                            {
                                Proxy_AccountInfo pai = Socket_Cache.ProxyAccount.GetProxyAccount_ByAccountID(gAID);

                                if (pai != null)
                                {
                                    if (pai.ExpiryTime >= DateTime.Now)
                                    {
                                        pai.ExpiryTime = pai.ExpiryTime.AddHours(Hours);
                                    }
                                    else
                                    {
                                        pai.ExpiryTime = DateTime.Now.AddHours(Hours);
                                    }
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

            #endregion            

            #region//保存登录信息到数据库

            public static void SaveProxyAccount_LoginInfo_ToDB(Proxy_AccountInfo pai)
            {
                try
                {
                    Socket_Cache.DataBase.InsertOrUpdateTable_ProxyAccount_LoginInfo(pai);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//从数据库加载登录信息

            public static DataTable LoadProxyAccount_LoginInfo_FromDB(Guid AID)
            {
                DataTable dtReturn = null;

                try
                {
                    if (AID != Guid.Empty)
                    {
                        dtReturn = Socket_Cache.DataBase.SelectTable_ProxyAccount_LoginInfo(AID);
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            #endregion

            #region//保存代理账号列表到数据库

            public static void SaveProxyAccountList_ToDB(Socket_Cache.System.SystemMode FromMode)
            {
                try
                {
                    if (Socket_Cache.System.StartMode == FromMode)
                    {
                        try
                        {
                            Socket_Cache.DataBase.DeleteTable_ProxyAccount();
                            Socket_Cache.DataBase.InsertTable_ProxyAccount();
                            Socket_Cache.DataBase.DeleteTable_ProxyAccount_LoginInfo();
                        }
                        catch (Exception ex)
                        {
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }            

            #endregion

            #region//从数据库加载代理账号列表（异步）

            public static async void LoadProxyAccountList_FromDB()
            {
                await Task.Run(() =>
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
                            DateTime LoginTime = Convert.ToDateTime(dataRow["LoginTime"]);
                            string LoginIP = dataRow["LoginIP"].ToString();
                            string IPLocation = dataRow["IPLocation"].ToString();
                            bool IsLimitLinks = Convert.ToBoolean(dataRow["IsLimitLinks"]);
                            int LimitLinks = int.Parse(dataRow["LimitLinks"].ToString());
                            bool IsLimitDevices = Convert.ToBoolean(dataRow["IsLimitDevices"]);
                            int LimitDevices = int.Parse(dataRow["LimitDevices"].ToString());
                            bool IsExpiry = Convert.ToBoolean(dataRow["IsExpiry"]);
                            DateTime ExpiryTime = Convert.ToDateTime(dataRow["ExpiryTime"]);
                            DateTime CreateTime = Convert.ToDateTime(dataRow["CreateTime"]);

                            Socket_Cache.ProxyAccount.AddProxyAccount(
                                AID, 
                                IsEnable, 
                                UserName, 
                                PassWord, 
                                LoginTime, 
                                LoginIP, 
                                IPLocation, 
                                IsLimitLinks, 
                                LimitLinks, 
                                IsLimitDevices,
                                LimitDevices,
                                IsExpiry, 
                                ExpiryTime, 
                                CreateTime);
                        }
                    }
                    catch (Exception ex)
                    {
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                });
            }

            #endregion

            #region//保存代理账号列表到文件（对话框）

            public static void SaveProxyAccountList_Dialog(string FileName, List<Guid> gList)
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
                                SaveProxyAccountList(FilePath, gList, true);

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

            private static void SaveProxyAccountList(string FilePath, List<Guid> gList, bool DoEncrypt)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeProxyAccountList = Socket_Cache.ProxyAccount.GetProxyAccountList_XML(gList);
                    if (xeProxyAccountList == null)
                    {
                        return;
                    }

                    xdoc.Add(xeProxyAccountList);
                    xdoc.Save(FilePath);

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

            public static XElement GetProxyAccountList_XML(List<Guid> gList)
            {
                try
                {
                    XElement xeProxyAccountList = new XElement("ProxyAccountList");                    

                    foreach (Guid gAID in gList)
                    {
                        Proxy_AccountInfo pai = Socket_Cache.ProxyAccount.GetProxyAccount_ByAccountID(gAID);

                        if (pai != null)
                        {
                            XElement xeProxyAccount =
                                new XElement("ProxyAccount",
                                new XElement("IsEnable", pai.IsEnable.ToString()),
                                new XElement("AID", pai.AID.ToString().ToUpper()),
                                new XElement("UserName", pai.UserName),
                                new XElement("PassWord", pai.PassWord),
                                new XElement("LoginIP", pai.LoginIP),
                                new XElement("IPLocation", pai.IPLocation),
                                new XElement("IsOnLine", pai.IsOnLine.ToString()),
                                new XElement("IsLimitLinks", pai.IsLimitLinks),
                                new XElement("LimitLinks", pai.LimitLinks),
                                new XElement("IsLimitDevices", pai.IsLimitDevices),
                                new XElement("LimitDevices", pai.LimitDevices),
                                new XElement("IsExpiry", pai.IsExpiry),
                                new XElement("ExpiryTime", pai.ExpiryTime.ToString("yyyy/MM/dd HH:mm:ss")),
                                new XElement("CreateTime", pai.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                );

                            xeProxyAccountList.Add(xeProxyAccount);
                        }                        
                    }

                    return xeProxyAccountList;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
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

            public static void LoadProxyAccountList_FromXDocument(XDocument xdoc)
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

                        DateTime LoginTime = DateTime.MinValue;
                        if (xeProxyAccount.Element("LoginTime") != null)
                        {
                            LoginTime = DateTime.Parse(xeProxyAccount.Element("LoginTime").Value);
                        }

                        string LoginIP = string.Empty;
                        if (xeProxyAccount.Element("LoginIP") != null)
                        {
                            LoginIP = xeProxyAccount.Element("LoginIP").Value;
                        }

                        string IPLocation = string.Empty;
                        if (xeProxyAccount.Element("IPLocation") != null)
                        {
                            IPLocation = xeProxyAccount.Element("IPLocation").Value;
                        }

                        bool IsOnLine = false;
                        if (xeProxyAccount.Element("IsOnLine") != null)
                        {
                            IsOnLine = bool.Parse(xeProxyAccount.Element("IsOnLine").Value);
                        }

                        bool IsLimitLinks = false;
                        if (xeProxyAccount.Element("IsLimitLinks") != null)
                        {
                            IsLimitLinks = bool.Parse(xeProxyAccount.Element("IsLimitLinks").Value);
                        }

                        int LimitLinks = 1;
                        if (xeProxyAccount.Element("LimitLinks") != null)
                        {
                            LimitLinks = int.Parse(xeProxyAccount.Element("LimitLinks").Value);
                        }

                        bool IsLimitDevices = true;
                        if (xeProxyAccount.Element("IsLimitDevices") != null)
                        {
                            IsLimitDevices = bool.Parse(xeProxyAccount.Element("IsLimitDevices").Value);
                        }

                        int LimitDevices = 1;
                        if (xeProxyAccount.Element("LimitDevices") != null)
                        {
                            LimitDevices = int.Parse(xeProxyAccount.Element("LimitDevices").Value);
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

                        bool bOK = Socket_Cache.ProxyAccount.AddProxyAccount(
                            AID, 
                            IsEnable, 
                            UserName, 
                            PassWord, 
                            LoginTime, 
                            LoginIP, 
                            IPLocation, 
                            IsLimitLinks, 
                            LimitLinks, 
                            IsLimitDevices,
                            LimitDevices,
                            IsExpiry, 
                            ExpiryTime, 
                            CreateTime);

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

                                case "MaxConn":
                                    if (value.Equals("-1"))
                                    {
                                        pai.IsLimitLinks = false;
                                        pai.LimitLinks = 1;
                                    }
                                    else
                                    { 
                                        pai.IsLimitLinks = true;
                                        pai.LimitLinks = int.Parse(value);
                                    }
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

                        pai.LoginTime = DateTime.MinValue;
                        pai.IsLimitDevices = true;
                        pai.LimitDevices = 1;

                        bool bOK = Socket_Cache.ProxyAccount.AddProxyAccount(
                            pai.AID, 
                            pai.IsEnable, 
                            pai.UserName, 
                            pai.PassWord, 
                            pai.LoginTime, 
                            pai.LoginIP, 
                            pai.IPLocation, 
                            pai.IsLimitLinks,
                            pai.LimitLinks,
                            pai.IsLimitDevices,
                            pai.LimitDevices,
                            pai.IsExpiry, 
                            pai.ExpiryTime, 
                            pai.CreateTime);

                        if (!bOK)
                        {
                            string FailLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_193), pai.UserName);
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, FailLog);
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

        #region//代理映射

        public static class ProxyMapping
        {
            public static string AESKey = string.Empty;
            public static bool IsShow = false;
            public static bool Enable_MapLocal = false;
            public static List<Proxy_MapLocal> lstMapLocal = new List<Proxy_MapLocal>();

            #region//获取 MapProtocol 类型

            public static Socket_Cache.SocketProxy.MapProtocol GetMapProtocol_ByString(string MapProtocol)
            {
                Socket_Cache.SocketProxy.MapProtocol MProtocol = SocketProxy.MapProtocol.Http;

                try
                {
                    MProtocol = (Socket_Cache.SocketProxy.MapProtocol)Enum.Parse(typeof(Socket_Cache.SocketProxy.MapProtocol), MapProtocol);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return MProtocol;
            }

            #endregion

            #region//新增本地代理映射    

            public static void AddMapLocal(bool IsEnable, Socket_Cache.SocketProxy.MapProtocol ProtocolType, string Host, int Port, string RemotePath, string LocalPath)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Host) && Port > 0)
                    {
                        Proxy_MapLocal pml = new Proxy_MapLocal(IsEnable, ProtocolType, Host, Port, RemotePath, LocalPath);
                        Socket_Cache.ProxyMapping.lstMapLocal.Add(pml);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//删除本地代理映射

            public static void DelMapLocal(Proxy_MapLocal pml)
            {
                try
                {
                    if (pml != null)
                    { 
                        Socket_Cache.ProxyMapping.lstMapLocal.Remove(pml);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//清空本地代理映射（对话框）

            public static void CleanUpMapLocal_Dialog()
            {
                try
                {
                    DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_38));

                    if (dr.Equals(DialogResult.OK))
                    {
                        Socket_Cache.ProxyMapping.MapLocalClear();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void MapLocalClear()
            {
                try
                {
                    lstMapLocal.Clear();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//更新本地代理映射

            public static void UpdateMapLocal(Proxy_MapLocal pml, Socket_Cache.SocketProxy.MapProtocol ProtocolType, string Host, int Port, string RemotePath, string LocalPath)
            {
                try
                {
                    if (pml != null && !string.IsNullOrEmpty(Host) && Port > 0)
                    {
                        pml.ProtocolType = ProtocolType;
                        pml.Host = Host; 
                        pml.Port = Port; 
                        pml.RemotePath = RemotePath;
                        pml.LocalPath = LocalPath;                        
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//查找本地代理映射

            public static Proxy_MapLocal GetMapLocal(Socket_Cache.SocketProxy.MapProtocol ProtocolType, string host, int port, string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return Socket_Cache.ProxyMapping.lstMapLocal.FirstOrDefault(rule =>
                    rule.IsEnable == true &&
                    rule.ProtocolType == ProtocolType &&
                    rule.Host.Equals(host, StringComparison.OrdinalIgnoreCase) &&
                    rule.Port == port);
                }
                else
                {
                    return Socket_Cache.ProxyMapping.lstMapLocal.FirstOrDefault(rule =>
                    rule.IsEnable == true &&
                    rule.ProtocolType == ProtocolType &&
                    rule.Host.Equals(host, StringComparison.OrdinalIgnoreCase) &&
                    rule.Port == port &&
                    path.StartsWith(rule.RemotePath, StringComparison.OrdinalIgnoreCase));
                }
                    
            }

            #endregion

            #region//本地代理映射的列表操作

            public static void UpdateMapLocal_ByListAction(Socket_Cache.System.ListAction listAction, Proxy_MapLocal pml)
            {
                try
                {
                    int iIndex = 0;

                    switch (listAction)
                    {
                        case Socket_Cache.System.ListAction.Top:

                            Socket_Cache.ProxyMapping.lstMapLocal.Remove(pml);
                            Socket_Cache.ProxyMapping.lstMapLocal.Insert(0, pml);

                            break;

                        case Socket_Cache.System.ListAction.Up:

                            iIndex = Socket_Cache.ProxyMapping.lstMapLocal.IndexOf(pml);
                            if (iIndex > 0)
                            {
                                Socket_Cache.ProxyMapping.lstMapLocal.Remove(pml);
                                Socket_Cache.ProxyMapping.lstMapLocal.Insert(iIndex - 1, pml);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Down:

                            iIndex = Socket_Cache.ProxyMapping.lstMapLocal.IndexOf(pml);
                            if (iIndex > -1 && iIndex < Socket_Cache.ProxyMapping.lstMapLocal.Count - 1)
                            {
                                Socket_Cache.ProxyMapping.lstMapLocal.Remove(pml);
                                Socket_Cache.ProxyMapping.lstMapLocal.Insert(iIndex + 1, pml);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Bottom:

                            Socket_Cache.ProxyMapping.lstMapLocal.Remove(pml);
                            Socket_Cache.ProxyMapping.lstMapLocal.Add(pml);

                            break;

                        case Socket_Cache.System.ListAction.Import:

                            Socket_Cache.ProxyMapping.LoadMapLocal_Dialog();

                            break;

                        case Socket_Cache.System.ListAction.Export:

                            Socket_Cache.ProxyMapping.SaveMapLocal_Dialog(string.Empty, Socket_Cache.ProxyMapping.lstMapLocal);

                            break;

                        case Socket_Cache.System.ListAction.CleanUp:

                            Socket_Cache.ProxyMapping.CleanUpMapLocal_Dialog();

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion                        

            #region//保存本地代理映射到数据库

            public static void SaveProxyMapLocal_ToDB(Socket_Cache.System.SystemMode FromMode)
            {
                try
                {
                    if (Socket_Cache.System.StartMode == FromMode)
                    {
                        try
                        {
                            Socket_Cache.DataBase.DeleteTable_ProxyMapLocal();
                            Socket_Cache.DataBase.InsertTable_ProxyMapLocal();                            
                        }
                        catch (Exception ex)
                        {
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//从数据库加载本地代理映射（异步）

            public static async void LoadProxyMapLocal_FromDB()
            {
                await Task.Run(() =>
                {
                    try
                    {
                        DataTable dtProxyMapLocal = Socket_Cache.DataBase.SelectTable_ProxyMapLocal();

                        foreach (DataRow dataRow in dtProxyMapLocal.Rows)
                        {
                            bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);
                            Socket_Cache.SocketProxy.MapProtocol ProtocolType = Socket_Cache.ProxyMapping.GetMapProtocol_ByString(dataRow["ProtocolType"].ToString());
                            string Host = dataRow["Host"].ToString();
                            int Port = int.Parse(dataRow["Port"].ToString());
                            string RemotePath = dataRow["RemotePath"].ToString();
                            string LocalPath = dataRow["LocalPath"].ToString();

                            Socket_Cache.ProxyMapping.AddMapLocal(IsEnable, ProtocolType, Host, Port, RemotePath, LocalPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                });
            }

            #endregion

            #region//保存本地映射到文件（对话框）

            public static void SaveMapLocal_Dialog(string FileName, List<Proxy_MapLocal> pmlList)
            {
                try
                {
                    if (Socket_Cache.ProxyMapping.lstMapLocal.Count > 0)
                    {
                        SaveFileDialog sfdSaveFile = new SaveFileDialog();

                        sfdSaveFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_220) + "（*.pml）|*.pml";

                        if (!string.IsNullOrEmpty(FileName))
                        {
                            sfdSaveFile.FileName = FileName;
                        }

                        sfdSaveFile.RestoreDirectory = true;

                        if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                        {
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.MapLocal_Export);
                            pwForm.ShowDialog();

                            string FilePath = sfdSaveFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                SaveMapLocal(FilePath, pmlList, true);

                                string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_221), FilePath);
                                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void SaveMapLocal(string FilePath, List<Proxy_MapLocal> pmlList, bool DoEncrypt)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeMapLocal = Socket_Cache.ProxyMapping.GetMapLocal_XML(pmlList);
                    if (xeMapLocal == null)
                    {
                        return;
                    }

                    xdoc.Add(xeMapLocal);
                    xdoc.Save(FilePath);

                    if (DoEncrypt)
                    {
                        string sPassword = Socket_Cache.ProxyMapping.AESKey;

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

            public static XElement GetMapLocal_XML(List<Proxy_MapLocal> pmlList)
            {
                try
                {
                    XElement xeMapLocal = new XElement("MapLocal");

                    foreach (Proxy_MapLocal pml in pmlList)
                    {
                        string sIsEnable = pml.IsEnable.ToString();
                        string sProtocolType = pml.ProtocolType.ToString();
                        string sHost = pml.Host.ToString();
                        string sPort = pml.Port.ToString();
                        string sRemotePath = pml.RemotePath.ToString();
                        string sLocalPath = pml.LocalPath.ToString();

                        XElement xeLocal =
                            new XElement("Local",
                            new XElement("IsEnable", pml.IsEnable.ToString()),
                            new XElement("ProtocolType", pml.ProtocolType.ToString()),
                            new XElement("Host", pml.Host),
                            new XElement("Port", pml.Port.ToString()),
                            new XElement("RemotePath", pml.RemotePath),
                            new XElement("LocalPath", pml.LocalPath)
                            );

                        xeMapLocal.Add(xeLocal);
                    }

                    return xeMapLocal;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            #endregion

            #region//从文件加载本地映射（对话框）

            public static void LoadMapLocal_Dialog()
            {
                try
                {
                    OpenFileDialog ofdLoadFile = new OpenFileDialog();

                    ofdLoadFile.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_220) + "（*.pml）|*.pml";
                    ofdLoadFile.RestoreDirectory = true;

                    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = ofdLoadFile.FileName;

                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            LoadMapLocal(FilePath, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static void LoadMapLocal(string FilePath, bool LoadFromUser)
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
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.System.PWType.MapLocal_Import);
                                pwForm.ShowDialog();
                            }

                            xdoc = Socket_Operation.DecryptXMLFile(FilePath, Socket_Cache.ProxyMapping.AESKey);
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
                            LoadMapLocal_FromXDocument(xdoc);

                            if (bEncrypt)
                            {
                                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_223));
                            }
                            else
                            {
                                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_222));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void LoadMapLocal_FromXDocument(XDocument xdoc)
            {
                try
                {
                    foreach (XElement xeMapLocal in xdoc.Root.Elements())
                    {
                        bool IsEnable = false;
                        if (xeMapLocal.Element("IsEnable") != null)
                        {
                            IsEnable = bool.Parse(xeMapLocal.Element("IsEnable").Value);
                        }

                        Socket_Cache.SocketProxy.MapProtocol ProtocolType = SocketProxy.MapProtocol.Http;
                        if (xeMapLocal.Element("ProtocolType") != null)
                        {
                            ProtocolType = Socket_Cache.ProxyMapping.GetMapProtocol_ByString(xeMapLocal.Element("ProtocolType").Value);
                        }

                        string Host = string.Empty;
                        if (xeMapLocal.Element("Host") != null)
                        {
                            Host = xeMapLocal.Element("Host").Value;
                        }

                        int Port = 80;
                        if (xeMapLocal.Element("Port") != null)
                        {
                            Port = int.Parse(xeMapLocal.Element("Port").Value);
                        }

                        string RemotePath = string.Empty;
                        if (xeMapLocal.Element("RemotePath") != null)
                        {
                            RemotePath = xeMapLocal.Element("RemotePath").Value;
                        }

                        string LocalPath = string.Empty;
                        if (xeMapLocal.Element("LocalPath") != null)
                        {
                            LocalPath = xeMapLocal.Element("LocalPath").Value;
                        }

                        Socket_Cache.ProxyMapping.AddMapLocal(IsEnable, ProtocolType, Host, Port, RemotePath, LocalPath);
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
            public static string HotKey1 = "Ctrl + Alt + F1";
            public static string HotKey2 = "Ctrl + Alt + F2";
            public static string HotKey3 = "Ctrl + Alt + F3";
            public static string HotKey4 = "Ctrl + Alt + F4";
            public static string HotKey5 = "Ctrl + Alt + F5";
            public static string HotKey6 = "Ctrl + Alt + F6";
            public static string HotKey7 = "Ctrl + Alt + F7";
            public static string HotKey8 = "Ctrl + Alt + F8";
            public static string HotKey9 = "Ctrl + Alt + F9";
            public static string HotKey10 = "Ctrl + Alt + F10";
            public static string HotKey11 = "Ctrl + Alt + F11";
            public static string HotKey12 = "Ctrl + Alt + F12";
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
                Socket_Cache.Filter.FilterAction pAction,
                DateTime PacketTime)
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

                            Socket_PacketInfo spi = new Socket_PacketInfo(PacketTime, iSocket, ptPacketType, sIPFrom, sIPTo, bRawBuff, bBuffByte, bBuffByte.Length, pAction);
                            qSocket_PacketInfo.Enqueue(spi);
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
            public static int Search_Index = -1;
            public static FindOptions FindOptions = new FindOptions();
            public static Socket_PacketInfo spiSelect;
            public static BindingList<Socket_PacketInfo> lstRecPacket = new BindingList<Socket_PacketInfo>();
       
            #region//封包入列表

            public static async Task SocketToList()
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
                                Span<byte> bufferSpan = spi.PacketBuffer.AsSpan();
                                spi.PacketData = Socket_Operation.GetPacketData_Hex(bufferSpan, Socket_Cache.SocketPacket.PacketData_MaxLen);

                                if (Socket_Cache.System.InvokeAction != null)
                                {
                                    Socket_Cache.System.InvokeAction(() =>
                                    {
                                        Socket_Cache.SocketList.lstRecPacket.Add(spi);
                                    });
                                }
                                else
                                {
                                    Socket_Cache.SocketList.lstRecPacket.Add(spi);
                                }
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

            #region//封包列表统计

            public static DataTable StatisticalSocketList_ByPacketLen()
            { 
                DataTable dtReturn = new DataTable();
                dtReturn.Columns.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_146), typeof(int));
                dtReturn.Columns.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_196), typeof(int));

                try
                {
                    Dictionary<int, int> packetLenCount = new Dictionary<int, int>();

                    foreach (Socket_PacketInfo packetInfo in lstRecPacket)
                    {
                        int packetLen = packetInfo.PacketLen;

                        if (packetLenCount.ContainsKey(packetLen))
                        {
                            packetLenCount[packetLen]++;
                        }
                        else
                        {
                            packetLenCount.Add(packetLen, 1);
                        }
                    }

                    Dictionary<int, int> sortedByKeyAsc = Socket_Operation.SortDictionaryByKey(packetLenCount, ascending: true);

                    foreach (KeyValuePair<int, int> kvp in sortedByKeyAsc)
                    {
                        DataRow row = dtReturn.NewRow();
                        row[0] = kvp.Key;
                        row[1] = kvp.Value;
                        dtReturn.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static DataTable StatisticalSocketList_ByPacketSocket()
            {
                DataTable dtReturn = new DataTable();
                dtReturn.Columns.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_147), typeof(int));
                dtReturn.Columns.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_196), typeof(int));

                try
                {
                    Dictionary<int, int> packetLenCount = new Dictionary<int, int>();

                    foreach (Socket_PacketInfo packetInfo in lstRecPacket)
                    {
                        int packetLen = packetInfo.PacketSocket;

                        if (packetLenCount.ContainsKey(packetLen))
                        {
                            packetLenCount[packetLen]++;
                        }
                        else
                        {
                            packetLenCount.Add(packetLen, 1);
                        }
                    }

                    Dictionary<int, int> sortedByKeyAsc = Socket_Operation.SortDictionaryByKey(packetLenCount, ascending: true);

                    foreach (KeyValuePair<int, int> kvp in sortedByKeyAsc)
                    {
                        DataRow row = dtReturn.NewRow();
                        row[0] = kvp.Key;
                        row[1] = kvp.Value;
                        dtReturn.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static DataTable StatisticalFilterList_ByExecutionCount()
            {
                DataTable dtReturn = new DataTable();
                dtReturn.Columns.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_197), typeof(string));
                dtReturn.Columns.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_196), typeof(int));

                try
                {
                    foreach (Socket_FilterInfo sfi in Socket_Cache.FilterList.lstFilter)
                    {
                        if (sfi.ExecutionCount > 0)
                        {
                            DataRow row = dtReturn.NewRow();
                            row[0] = sfi.FName;
                            row[1] = sfi.ExecutionCount;
                            dtReturn.Rows.Add(row);
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

            #region//发送封包列表中当前选中的封包

            public static void SendSocketList_BySelect()
            {
                try
                {
                    if (Socket_Cache.SocketList.spiSelect != null)
                    {
                        int Socket = Socket_Cache.SocketList.spiSelect.PacketSocket;
                        Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketList.spiSelect.PacketType;
                        string From = Socket_Cache.SocketList.spiSelect.PacketFrom;
                        string To = Socket_Cache.SocketList.spiSelect.PacketTo;
                        byte[] bBuffer = Socket_Cache.SocketList.spiSelect.PacketBuffer;

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

            #region//日志入列表

            public static void LogToList(Socket_Cache.System.LogType logType)
            {
                switch (logType)
                {
                    case Socket_Cache.System.LogType.Socket:

                        if (LogQueue.qSocket_Log.TryDequeue(out Socket_LogInfo sliSocket))
                        {
                            if (Socket_Cache.System.InvokeAction != null)
                            {
                                Socket_Cache.System.InvokeAction(() =>
                                {
                                    Socket_Cache.LogList.lstSocketLog.Add(sliSocket);
                                });
                            }
                            else
                            {
                                Socket_Cache.LogList.lstSocketLog.Add(sliSocket);
                            }
                        }

                        break;

                    case Socket_Cache.System.LogType.Proxy:

                        if (LogQueue.qProxy_Log.TryDequeue(out Socket_LogInfo sliProxy))
                        {
                            if (Socket_Cache.System.InvokeAction != null)
                            {
                                Socket_Cache.System.InvokeAction(() =>
                                {
                                    Socket_Cache.LogList.lstProxyLog.Add(sliProxy);
                                });
                            }
                            else
                            {
                                Socket_Cache.LogList.lstProxyLog.Add(sliProxy);
                            }
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

            private struct SearchCondition
            {
                public int RelativePosition { get; set; }
                public byte Value { get; set; }
            }

            private struct Modification
            {
                public int Index { get; set; }
                public byte Value { get; set; }
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

                    Socket_Cache.Filter.AddFilter(false, FID, FName, false, string.Empty, false, 0, false, string.Empty, false, 0, FilterMode, FilterAction, false, FilterExecuteType, SID, RID, FilterFunction, FilterStartFrom, false, false, 1, false, 1, string.Empty, 0, string.Empty, string.Empty);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void AddFilter_ByPacketInfo(Socket_PacketInfo spi, byte[] bBuffer)
            {
                try
                {
                    if (spi != null)
                    {
                        if (bBuffer == null || bBuffer.Length == 0)
                        {
                            bBuffer = spi.PacketBuffer;
                        }

                        Guid FID = Guid.NewGuid();                     
                        string sFName = Process.GetCurrentProcess().ProcessName.Trim() + " [" + bBuffer.Length + "]";
                        Socket_Cache.SocketPacket.PacketType ptType = spi.PacketType;
                        Socket_Cache.Filter.FilterMode FilterMode = Socket_Cache.Filter.FilterMode.Normal;
                        Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.Filter.FilterAction.Replace;
                        Socket_Cache.Filter.FilterExecuteType FilterExecuteType = new Socket_Cache.Filter.FilterExecuteType();
                        Guid SID = Guid.Empty;
                        Guid RID = Guid.Empty;
                        Socket_Cache.Filter.FilterFunction FilterFunction = Socket_Cache.Filter.GetFilterFunction_ByPacketType(ptType);
                        Socket_Cache.Filter.FilterStartFrom FilterStartFrom = Socket_Cache.Filter.FilterStartFrom.Head;
                        string sFSearch = Socket_Cache.Filter.GetFilterString_ByBytes(bBuffer);

                        Socket_Cache.Filter.AddFilter(false, FID, sFName, false, string.Empty, false, 0, false, string.Empty, false, 0, FilterMode, FilterAction, false, FilterExecuteType, SID, RID, FilterFunction, FilterStartFrom, false, false, 1, false, 1, string.Empty, 0, sFSearch, string.Empty);
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
                bool IsProgressionContinuous,
                decimal ProgressionStep,
                bool IsProgressionCarry,
                decimal ProgressionCarryNumber,
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
                        IsProgressionContinuous,
                        ProgressionStep,
                        IsProgressionCarry,
                        ProgressionCarryNumber,
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

            public static void UpdateFilter(
                Socket_FilterInfo sfi,
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
                bool IsProgressionContinuous,
                decimal ProgressionStep,
                bool IsProgressionCarry,
                decimal ProgressionCarryNumber,
                string ProgressionPosition,
                int ProgressionCount,
                string FSearch,
                string FModify)
            {
                try
                {
                    if (sfi != null)
                    {
                        sfi.FName = FName;
                        sfi.AppointHeader = AppointHeader;
                        sfi.HeaderContent = HeaderContent;
                        sfi.AppointSocket = AppointSocket;
                        sfi.SocketContent = SocketContent;
                        sfi.AppointLength = AppointLength;
                        sfi.LengthContent = LengthContent;
                        sfi.AppointPort = AppointPort;
                        sfi.PortContent = PortContent;
                        sfi.FMode = FilterMode;
                        sfi.FAction = FilterAction;
                        sfi.IsExecute = IsExecute;
                        sfi.FEType = FEType;
                        sfi.SID = SID;
                        sfi.RID = RID;
                        sfi.FFunction = FilterFunction;
                        sfi.FStartFrom = FilterStartFrom;
                        sfi.IsProgressionContinuous = IsProgressionContinuous;
                        sfi.ProgressionStep = ProgressionStep;
                        sfi.IsProgressionCarry = IsProgressionCarry;
                        sfi.ProgressionCarryNumber = ProgressionCarryNumber;
                        sfi.ProgressionPosition = ProgressionPosition;
                        sfi.ProgressionCount = ProgressionCount;
                        sfi.FSearch = FSearch;
                        sfi.FModify = FModify;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//删除滤镜

            public static void DeleteFilter_Dialog(List<Socket_FilterInfo> sfiList)
            {
                try
                {
                    if (sfiList.Count > 0)
                    {
                        DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                        if (dr.Equals(DialogResult.OK))
                        {
                            foreach (Socket_FilterInfo sfi in sfiList)
                            {
                                Socket_Cache.FilterList.lstFilter.Remove(sfi);
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

            #region//复制滤镜

            public static void CopyFilter(Socket_FilterInfo sfi)
            {
                try
                {
                    bool IsEnable = false;
                    Guid FID = Guid.NewGuid();
                    string FName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_62), sfi.FName);
                    bool bAppointHeader = sfi.AppointHeader;
                    string HeaderContent = sfi.HeaderContent;
                    bool bAppointSocket = sfi.AppointSocket;
                    decimal SocketContent = sfi.SocketContent;
                    bool bAppointLength = sfi.AppointLength;
                    string LengthContent = sfi.LengthContent;
                    bool bAppointPort = sfi.AppointPort;
                    decimal PortContent = sfi.PortContent;
                    Socket_Cache.Filter.FilterMode FMode = sfi.FMode;
                    Socket_Cache.Filter.FilterAction FAction = sfi.FAction;
                    bool IsExecute = sfi.IsExecute;
                    Socket_Cache.Filter.FilterExecuteType FEType = sfi.FEType;
                    Guid SID = sfi.SID;
                    Guid RID = sfi.RID;
                    Socket_Cache.Filter.FilterFunction FFunction = sfi.FFunction;
                    Socket_Cache.Filter.FilterStartFrom FStartFrom = sfi.FStartFrom;
                    bool IsProgressionDone = false;
                    bool IsProgressionContinuous = sfi.IsProgressionContinuous;
                    decimal ProgressionStep = sfi.ProgressionStep;
                    bool IsProgressionCarry = sfi.IsProgressionCarry;
                    decimal ProgressionCarryNumber = sfi.ProgressionCarryNumber;
                    string ProgressionPosition = sfi.ProgressionPosition;
                    int ProgressionCount = 0;
                    string FSearch = sfi.FSearch;
                    string FModify = sfi.FModify;

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
                        IsProgressionContinuous,
                        ProgressionStep,
                        IsProgressionCarry,
                        ProgressionCarryNumber,
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
                try
                {
                    switch (filterAction)
                    {
                        case Socket_Cache.Filter.FilterAction.Replace:
                            return MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_65);

                        case Socket_Cache.Filter.FilterAction.Intercept:
                            return MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_66);

                        case Socket_Cache.Filter.FilterAction.Change:
                            return MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_173);

                        case Socket_Cache.Filter.FilterAction.NoModify_Display:
                            return MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_67);

                        case Socket_Cache.Filter.FilterAction.NoModify_NoDisplay:
                            return MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_68);

                        default:
                            return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(nameof(GetName_ByFilterAction), ex.Message);
                    return string.Empty;
                }
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

            #region//检查滤镜是否生效

            public static bool CheckFilter_IsEffective(
                Int32 iSocket, 
                Span<byte> bufferSpan,
                Socket_Cache.SocketPacket.PacketType ptType,
                Socket_Cache.SocketPacket.SockAddr sAddr,
                Socket_FilterInfo sfi)
            {
                if (!sfi.IsEnable)
                    return false;

                if (!Socket_Cache.Filter.CheckFilterFunction_ByPacketType(ptType, sfi.FFunction))
                    return false;

                if (sfi.AppointSocket && !Socket_Cache.Filter.CheckPacket_IsMatch_AppointSocket(iSocket, ((int)sfi.SocketContent)))
                    return false;

                if (sfi.AppointPort && !Socket_Cache.Filter.CheckPacket_IsMatch_AppointPort(iSocket, ptType, sAddr, ((int)sfi.PortContent)))
                    return false;

                if (sfi.AppointLength && !Socket_Cache.Filter.CheckPacket_IsMatch_AppointLength(bufferSpan.Length, sfi.LengthContent))
                    return false;

                if (sfi.AppointHeader && !Socket_Cache.Filter.CheckPacket_IsMatch_AppointHeader(bufferSpan, sfi.HeaderContent))
                    return false;

                return true;
            }

            #endregion

            #region//检查滤镜作用类别

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe bool CheckFilterFunction_ByPacketType(Socket_Cache.SocketPacket.PacketType ptType, in FilterFunction ffFunction)
            {
                fixed (bool* pFlags = &ffFunction.Send)
                {
                    byte* indexMap = stackalloc byte[13]
                    {
                        0,  // WS1_Send -> Send (offset 0)
                        0,  // WS2_Send -> Send
                        1,  // WS1_SendTo -> SendTo
                        1,  // WS2_SendTo -> SendTo
                        2,  // WS1_Recv -> Recv
                        2,  // WS2_Recv -> Recv
                        3,  // WS1_RecvFrom -> RecvFrom
                        3,  // WS2_RecvFrom -> RecvFrom
                        4,  // WSASend -> WSASend
                        5,  // WSASendTo -> WSASendTo
                        6,  // WSARecv -> WSARecv
                        6,  // WSARecvEx -> WSARecv
                        7   // WSARecvFrom -> WSARecvFrom
                    };

                    int index = (int)ptType;
                    if (index >= 0 && index < 13)
                    {
                        return pFlags[indexMap[index]];
                    }
                }

                return false;
            }            

            #endregion

            #region//检查是否匹配指定套接字

            public static bool CheckPacket_IsMatch_AppointSocket(Int32 iSocket, int socketContent)
            {
                return iSocket == socketContent;
            }

            #endregion

            #region//检查是否匹配指定长度

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool CheckPacket_IsMatch_AppointLength(int len, string lengthContent)
            {
                if (string.IsNullOrEmpty(lengthContent))
                    return false;

                try
                {
                    int dashIndex = lengthContent.IndexOf('-');

                    if (dashIndex >= 0)
                    {
                        string fromStr = lengthContent.Substring(0, dashIndex);
                        string toStr = lengthContent.Substring(dashIndex + 1);

                        return int.TryParse(fromStr, out int lenFrom) &&
                               int.TryParse(toStr, out int lenTo) &&
                               len >= lenFrom &&
                               len <= lenTo;
                    }
                    
                    return int.TryParse(lengthContent, out int exactLen) &&
                           len == exactLen;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return false;
                }
            }

            #endregion

            #region//检查是否匹配指定端口

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool CheckPacket_IsMatch_AppointPort(
                int iSocket,
                Socket_Cache.SocketPacket.PacketType ptType,
                Socket_Cache.SocketPacket.SockAddr sAddr,
                int portContent)
            {
                try
                {
                    string packetIP = Socket_Operation.GetIPString_BySocketAddr(iSocket, sAddr, ptType);
                    if (string.IsNullOrEmpty(packetIP))
                        return false;

                    ReadOnlySpan<char> ipSpan = packetIP.AsSpan();
                    int pipeIndex = ipSpan.IndexOf('|');

                    if (pipeIndex <= 0)
                        return false;

                    if (CheckPortInPart(ipSpan.Slice(0, pipeIndex), portContent))
                        return true;

                    return CheckPortInPart(ipSpan.Slice(pipeIndex + 1), portContent);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool CheckPortInPart(ReadOnlySpan<char> ipPortPart, int port)
            {
                int colonIndex = ipPortPart.IndexOf(':');
                if (colonIndex <= 0)
                    return false;

                ReadOnlySpan<char> portSpan = ipPortPart.Slice(colonIndex + 1);
                unsafe
                {
                    fixed (char* ptr = &portSpan.GetPinnableReference())
                    {
                        return int.TryParse(new string(ptr, 0, portSpan.Length), out int actualPort)
                            && actualPort == port;
                    }
                }
            }

            #endregion

            #region//检查是否匹配指定包头

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool CheckPacket_IsMatch_AppointHeader(ReadOnlySpan<byte> bufferSpan, string headerContent)
            {
                if (string.IsNullOrEmpty(headerContent))
                    return false;

                try
                {
                    byte[] headerBytes = Socket_Operation.StringToBytes(
                        Socket_Cache.SocketPacket.EncodingFormat.Hex,
                        headerContent);

                    if (headerBytes.Length > 0 && headerBytes.Length <= bufferSpan.Length)
                    {
                        return bufferSpan.Slice(0, headerBytes.Length).SequenceEqual(headerBytes);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return false;
            }

            #endregion

            #region//检查滤镜是否匹配成功（普通滤镜）

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool CheckFilter_IsMatch_Normal(Socket_FilterInfo sfi, ReadOnlySpan<byte> bufferSpan)
            {
                if (string.IsNullOrEmpty(sfi.FSearch))
                    return false;

                try
                {
                    string[] searchParts = sfi.FSearch.Split(',');
                    foreach (string part in searchParts)
                    {
                        if (!string.IsNullOrEmpty(part) && part.IndexOf('|') > 0)
                        {
                            string[] pair = part.Split('|');
                            if (pair.Length != 2)
                                return false;

                            if (!TryParseNonNegativeInt(pair[0], out int index) ||
                                index >= bufferSpan.Length)
                            {
                                return false;
                            }

                            if (pair[1].Length != 2 ||
                                !HexCharsToByte(pair[1], out byte expected) ||
                                bufferSpan[index] != expected)
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return false;
                }

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool TryParseNonNegativeInt(string s, out int result)
            {
                return int.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out result) &&
                       result >= 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool HexCharsToByte(string s, out byte result)
            {
                result = 0;
                if (s.Length != 2) return false;

                int high = CharToNibble(s[0]);
                int low = CharToNibble(s[1]);
                if (high == -1 || low == -1)
                    return false;

                result = (byte)((high << 4) | low);
                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int CharToNibble(char c)
            {
                if (c >= '0' && c <= '9') return c - '0';
                if (c >= 'A' && c <= 'F') return 10 + (c - 'A');
                if (c >= 'a' && c <= 'f') return 10 + (c - 'a');
                return -1;
            }

            #endregion

            #region//检查滤镜是否匹配成功（高级滤镜）

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static List<int> CheckFilter_IsMatch_Advanced(Socket_FilterInfo sfi, ReadOnlySpan<byte> bufferSpan)
            {
                var result = new List<int>();
                if (string.IsNullOrEmpty(sfi.FSearch))
                    return result;

                try
                {
                    var searchConditions = Socket_Cache.Filter.ParseSearchConditions(sfi.FSearch);
                    if (searchConditions.Count == 0)
                        return result;

                    var firstCondition = searchConditions[0];
                    byte firstValue = firstCondition.Value;
                    int relativePosition = firstCondition.RelativePosition;

                    for (int i = 0; i < bufferSpan.Length; i++)
                    {
                        if (bufferSpan[i] == firstValue)
                        {
                            bool isMatch = true;
                            int lastCheckedIndex = i;

                            for (int j = 1; j < searchConditions.Count; j++)
                            {
                                var condition = searchConditions[j];
                                int checkIndex = i + condition.RelativePosition - relativePosition;

                                if (checkIndex < 0 || checkIndex >= bufferSpan.Length ||
                                    bufferSpan[checkIndex] != condition.Value)
                                {
                                    isMatch = false;
                                    break;
                                }
                                lastCheckedIndex = Math.Max(lastCheckedIndex, checkIndex);
                            }

                            if (isMatch)
                            {
                                result.Add(i);

                                if (sfi.FStartFrom == Socket_Cache.Filter.FilterStartFrom.Head)
                                {
                                    break;
                                }

                                i = lastCheckedIndex;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static List<SearchCondition> ParseSearchConditions(string searchPattern)
            {
                var conditions = new List<SearchCondition>();
                string[] parts = searchPattern.Split(',');

                foreach (string part in parts)
                {
                    if (string.IsNullOrEmpty(part))
                        continue;

                    string[] pair = part.Split('|');
                    if (pair.Length != 2)
                        continue;

                    if (int.TryParse(pair[0], out int position) &&
                        byte.TryParse(pair[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                    {
                        conditions.Add(new Socket_Cache.Filter.SearchCondition
                        {
                            RelativePosition = position,
                            Value = value
                        });
                    }
                }

                return conditions;
            }

            #endregion

            #region//执行替换（普通滤镜）

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Replace_Normal(Socket_FilterInfo sfi, Span<byte> bufferSpan)
            {
                if (string.IsNullOrEmpty(sfi.FSearch))
                    return false;

                bool hasModifications = !string.IsNullOrEmpty(sfi.FModify);
                bool hasProgressions = !string.IsNullOrEmpty(sfi.ProgressionPosition);

                if (!hasModifications && !hasProgressions)
                    return false;

                try
                {
                    bool result = false;

                    if (hasModifications)
                    {
                        result |= Socket_Cache.Filter.ProcessModifications(sfi, bufferSpan);
                    }

                    if (hasProgressions)
                    {
                        result |= Socket_Cache.Filter.ProcessProgressions(sfi, bufferSpan);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool ProcessModifications(Socket_FilterInfo sfi, Span<byte> bufferSpan)
            {
                bool modified = false;
                string[] modifications = sfi.FModify.Split(',');

                foreach (string modification in modifications)
                {
                    if (string.IsNullOrEmpty(modification))
                        continue;

                    string[] parts = modification.Split('|');
                    if (parts.Length != 2)
                        continue;

                    if (int.TryParse(parts[0], out int index) &&
                        index >= 0 &&
                        index < bufferSpan.Length &&
                        byte.TryParse(parts[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                    {
                        bufferSpan[index] = value;
                        modified = true;
                    }
                }

                return modified;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool ProcessProgressions(Socket_FilterInfo sfi, Span<byte> bufferSpan)
            {
                bool modified = false;
                int carryCount = 0;
                int step = (int)sfi.ProgressionStep;
                string[] positions = sfi.ProgressionPosition.Split(',');

                foreach (string position in positions)
                {
                    if (string.IsNullOrEmpty(position) ||
                        !int.TryParse(position, out int index) ||
                        index < 0 ||
                        index >= bufferSpan.Length)
                    {
                        continue;
                    }

                    byte currentValue = bufferSpan[index];
                    byte newValue = Socket_Operation.GetStepByte(currentValue, step * (sfi.ProgressionCount + 1), out carryCount);
                    bufferSpan[index] = newValue;
                    modified = true;
                    sfi.IsProgressionDone = true;

                    if (sfi.IsProgressionCarry && carryCount > 0)
                    {
                        for (int i = 0; i < sfi.ProgressionCarryNumber; i++)
                        {
                            int prevIndex = index - (i + 1);
                            if (prevIndex < 0)
                                break;

                            byte prevValue = bufferSpan[prevIndex];
                            prevValue = Socket_Operation.GetStepByte(prevValue, carryCount, out carryCount);
                            bufferSpan[prevIndex] = prevValue;
                            modified = true;

                            if (carryCount == 0)
                                break;
                        }
                    }
                }

                return modified;
            }

            #endregion            

            #region//执行替换（高级滤镜）

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Replace_Advanced(Socket_FilterInfo sfi, int matchIndex, Span<byte> bufferSpan)
            {
                if (string.IsNullOrEmpty(sfi.FSearch))
                    return false;

                bool hasModifications = !string.IsNullOrEmpty(sfi.FModify);
                bool hasProgressions = !string.IsNullOrEmpty(sfi.ProgressionPosition);

                if (!hasModifications && !hasProgressions)
                    return false;

                try
                {
                    bool result = false;
                    var startFrom = sfi.FStartFrom;

                    if (hasModifications)
                    {
                        result |= ProcessAdvancedModifications(sfi, matchIndex, bufferSpan, startFrom);
                    }

                    if (hasProgressions)
                    {
                        result |= ProcessAdvancedProgressions(sfi, matchIndex, bufferSpan, startFrom);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool ProcessAdvancedModifications(
                Socket_FilterInfo sfi, 
                int matchIndex, 
                Span<byte> bufferSpan,
                Socket_Cache.Filter.FilterStartFrom startFrom)
            {
                bool modified = false;
                string[] modifications = sfi.FModify.Split(',');

                foreach (string modification in modifications)
                {
                    if (string.IsNullOrEmpty(modification))
                        continue;

                    string[] parts = modification.Split('|');
                    if (parts.Length != 2)
                        continue;

                    if (!int.TryParse(parts[0], out int index))
                        continue;

                    if (startFrom == Socket_Cache.Filter.FilterStartFrom.Position)
                    {
                        index += matchIndex;
                    }

                    if (index < 0 || index >= bufferSpan.Length)
                        continue;

                    if (byte.TryParse(parts[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                    {
                        bufferSpan[index] = value;
                        modified = true;
                    }
                }

                return modified;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool ProcessAdvancedProgressions(
                Socket_FilterInfo sfi, 
                int matchIndex, 
                Span<byte> bufferSpan,
                Socket_Cache.Filter.FilterStartFrom startFrom)
            {
                bool modified = false;
                int carryCount = 0;
                int step = (int)sfi.ProgressionStep;
                string[] positions = sfi.ProgressionPosition.Split(',');

                foreach (string position in positions)
                {
                    if (string.IsNullOrEmpty(position) || !int.TryParse(position, out int index))
                        continue;

                    if (startFrom == Socket_Cache.Filter.FilterStartFrom.Position)
                    {
                        index += matchIndex;
                    }

                    if (index < 0 || index >= bufferSpan.Length)
                        continue;

                    byte currentValue = bufferSpan[index];
                    byte newValue = Socket_Operation.GetStepByte(currentValue, step * (sfi.ProgressionCount + 1), out carryCount);
                    bufferSpan[index] = newValue;
                    modified = true;
                    sfi.IsProgressionDone = true;
                    
                    if (sfi.IsProgressionCarry && carryCount > 0)
                    {
                        HandleCarryOver(sfi, bufferSpan, index, ref carryCount);
                    }
                }

                return modified;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void HandleCarryOver(Socket_FilterInfo sfi, Span<byte> bufferSpan, int index, ref int carryCount)
            {
                for (int i = 0; i < sfi.ProgressionCarryNumber && carryCount > 0; i++)
                {
                    int prevIndex = index - (i + 1);
                    if (prevIndex < 0)
                        break;

                    byte prevValue = bufferSpan[prevIndex];
                    prevValue = Socket_Operation.GetStepByte(prevValue, carryCount, out carryCount);
                    bufferSpan[prevIndex] = prevValue;
                }
            }

            #endregion

            #region//执行换包

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte[] ChangePacket_Filter(Socket_FilterInfo sfi)
            {
                if (string.IsNullOrEmpty(sfi.FModify))
                    return Array.Empty<byte>();

                try
                {
                    var modifications = Socket_Cache.Filter.ParseModifications(sfi.FModify);
                    if (modifications.Count == 0)
                        return Array.Empty<byte>();

                    byte[] newBuffer = new byte[modifications.Max(m => m.Index) + 1];
                    Socket_Cache.Filter.ApplyModifications(newBuffer, modifications);

                    return newBuffer;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    return Array.Empty<byte>();
                }
            }            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static List<Socket_Cache.Filter.Modification> ParseModifications(string modifyString)
            {
                var modifications = new List<Socket_Cache.Filter.Modification>();
                string[] parts = modifyString.Split(',');

                foreach (string part in parts)
                {
                    if (string.IsNullOrEmpty(part))
                        continue;

                    string[] pair = part.Split('|');
                    if (pair.Length != 2)
                        continue;

                    if (int.TryParse(pair[0], out int index) &&
                        byte.TryParse(pair[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                    {
                        modifications.Add(new Socket_Cache.Filter.Modification { Index = index, Value = value });
                    }
                }

                return modifications;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void ApplyModifications(byte[] buffer, List<Socket_Cache.Filter.Modification> modifications)
            {
                foreach (var mod in modifications)
                {
                    if (mod.Index >= 0 && mod.Index < buffer.Length)
                    {
                        buffer[mod.Index] = mod.Value;
                    }
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

            #region//滤镜入列表

            public static void FilterToList(Socket_FilterInfo sfi)
            {
                try
                {
                    if (Socket_Cache.System.InvokeAction != null)
                    {
                        Socket_Cache.System.InvokeAction(() =>
                        {
                            Socket_Cache.FilterList.lstFilter.Add(sfi);
                        });
                    }
                    else
                    {
                        Socket_Cache.FilterList.lstFilter.Add(sfi);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }                
            }

            #endregion

            #region//初始化滤镜列表的计数

            public static void InitFilterList_Count()
            {
                try
                {
                    foreach (Socket_FilterInfo sfi in lstFilter)
                    {
                        sfi.ExecutionCount = 0;
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

            public static void UpdateFilterList_ByListAction(Socket_Cache.System.ListAction listAction, List<Socket_FilterInfo> sfiList)
            {
                try
                {
                    switch (listAction)
                    {
                        case Socket_Cache.System.ListAction.Top:

                            sfiList.Reverse();

                            foreach (Socket_FilterInfo sfi in sfiList)
                            {
                                Socket_Cache.FilterList.lstFilter.Remove(sfi);
                                Socket_Cache.FilterList.lstFilter.Insert(0, sfi);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Up:

                            foreach (Socket_FilterInfo sfi in sfiList)
                            {
                                int iIndex = Socket_Cache.FilterList.lstFilter.IndexOf(sfi);

                                if (iIndex > 0)
                                {
                                    Socket_Cache.FilterList.lstFilter.Remove(sfi);
                                    Socket_Cache.FilterList.lstFilter.Insert(iIndex - 1, sfi);
                                }
                            }

                            break;

                        case Socket_Cache.System.ListAction.Down:

                            sfiList.Reverse();

                            foreach (Socket_FilterInfo sfi in sfiList)
                            {
                                int iIndex = Socket_Cache.FilterList.lstFilter.IndexOf(sfi);

                                if (iIndex > -1 && iIndex < Socket_Cache.FilterList.lstFilter.Count - 1)
                                {
                                    Socket_Cache.FilterList.lstFilter.Remove(sfi);
                                    Socket_Cache.FilterList.lstFilter.Insert(iIndex + 1, sfi);
                                }
                            }

                            break;

                        case Socket_Cache.System.ListAction.Bottom:

                            foreach (Socket_FilterInfo sfi in sfiList)
                            {
                                Socket_Cache.FilterList.lstFilter.Remove(sfi);
                                Socket_Cache.FilterList.lstFilter.Add(sfi);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Copy:

                            foreach (Socket_FilterInfo sfi in sfiList)
                            {
                                Socket_Cache.Filter.CopyFilter(sfi);
                            }                            

                            break;

                        case Socket_Cache.System.ListAction.Export:

                            string sFName = sfiList[0].FName;
                            Socket_Cache.FilterList.SaveFilterList_Dialog(sFName, sfiList);

                            break;

                        case Socket_Cache.System.ListAction.Delete:

                            Socket_Cache.Filter.DeleteFilter_Dialog(sfiList);

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion                        

            #region//执行滤镜列表

            public static Socket_Cache.Filter.FilterAction DoFilterList(Int32 iSocket, Span<byte> bufferSpan, out byte[] bNewBuffer, Socket_Cache.SocketPacket.PacketType ptType, Socket_Cache.SocketPacket.SockAddr sAddr)
            {
                Socket_Cache.Filter.FilterAction faReturn = Filter.FilterAction.None;
                bool bBreak = false;
                bNewBuffer = null;

                try
                {
                    var filters = Socket_Cache.FilterList.lstFilter;
                    for (int i = 0; i < filters.Count; i++)
                    {
                        var sfi = filters[i];
                        if (!Socket_Cache.Filter.CheckFilter_IsEffective(iSocket, bufferSpan, ptType, sAddr, sfi))
                        {
                            continue;
                        }                            

                        bool bDoFilter = false;
                        bool isMatch = false;
                        List<int> MatchIndex = null;

                        if (sfi.FMode == Filter.FilterMode.Normal)
                        {
                            isMatch = Socket_Cache.Filter.CheckFilter_IsMatch_Normal(sfi, bufferSpan);
                        }
                        else if (sfi.FMode == Filter.FilterMode.Advanced)
                        {
                            MatchIndex = Socket_Cache.Filter.CheckFilter_IsMatch_Advanced(sfi, bufferSpan);
                            isMatch = MatchIndex != null && MatchIndex.Count > 0;
                        }

                        if (!isMatch)
                        {
                            continue;
                        }

                        faReturn = sfi.FAction;
                        byte[] tempBuffer = null;

                        switch (sfi.FAction)
                        {
                            case Filter.FilterAction.Replace:

                                sfi.IsProgressionDone = false;

                                if (sfi.FMode == Filter.FilterMode.Normal)
                                {
                                    bDoFilter = Socket_Cache.Filter.Replace_Normal(sfi, bufferSpan);
                                    if (bDoFilter)
                                    {
                                        tempBuffer = bufferSpan.ToArray();
                                    }
                                }
                                else if (sfi.FMode == Filter.FilterMode.Advanced && MatchIndex != null)
                                {
                                    foreach (int iIndex in MatchIndex)
                                    {
                                        Socket_Cache.Filter.Replace_Advanced(sfi, iIndex, bufferSpan);
                                    }

                                    bDoFilter = true;
                                    tempBuffer = bufferSpan.ToArray();
                                }

                                if (sfi.IsProgressionDone && sfi.IsProgressionContinuous)
                                {
                                    sfi.ProgressionCount++;
                                }

                                break;

                            case Filter.FilterAction.Change:

                                tempBuffer = Socket_Cache.Filter.ChangePacket_Filter(sfi);
                                bDoFilter = tempBuffer != null && tempBuffer.Length > 0;

                                break;

                            case Filter.FilterAction.Intercept:
                            case Filter.FilterAction.NoModify_Display:
                            case Filter.FilterAction.NoModify_NoDisplay:

                                bDoFilter = true;
                                bBreak = true;

                                break;
                        }

                        if (bDoFilter)
                        {
                            sfi.ExecutionCount++;
                            Interlocked.Increment(ref Socket_Cache.Filter.FilterExecute_CNT);

                            if (tempBuffer != null)
                            {
                                bNewBuffer = tempBuffer;
                            }

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
                                string sFilterLog = MatchIndex != null && MatchIndex.Count > 0
                                    ? string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_69),
                                        Socket_Cache.Filter.GetName_ByFilterAction(sfi.FAction),
                                        sfi.FName,
                                        Socket_Cache.SocketPacket.GetName_ByPacketType(ptType),
                                        bufferSpan.Length,
                                        MatchIndex.Count)
                                    : string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_51),
                                        Socket_Cache.Filter.GetName_ByFilterAction(sfi.FAction),
                                        sfi.FName,
                                        Socket_Cache.SocketPacket.GetName_ByPacketType(ptType),
                                        bufferSpan.Length);

                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sFilterLog);
                            }

                            if (Socket_Cache.Filter.FilterExecute == Socket_Cache.Filter.Execute.Priority)
                            {
                                bBreak = true;
                            }
                        }

                        if (bBreak)
                        {
                            if (bNewBuffer == null)
                            {
                                bNewBuffer = bufferSpan.ToArray();
                            }

                            return faReturn;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                if (bNewBuffer == null)
                {
                    bNewBuffer = bufferSpan.ToArray();
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
                        bool IsProgressionContinuous = Convert.ToBoolean(dataRow["IsProgressionContinuous"]);
                        decimal FProgressionStep = Convert.ToDecimal(dataRow["ProgressionStep"]);
                        bool IsProgressionCarry = Convert.ToBoolean(dataRow["IsProgressionCarry"]);
                        decimal ProgressionCarryNumber = Convert.ToDecimal(dataRow["ProgressionCarryNumber"]);
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
                            IsProgressionContinuous,
                            FProgressionStep,
                            IsProgressionCarry,
                            ProgressionCarryNumber,
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

            public static void SaveFilterList_Dialog(string FileName, List<Socket_FilterInfo> sfiList)
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
                                SaveFilterList(FilePath, sfiList, true);

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

            private static void SaveFilterList(string FilePath, List<Socket_FilterInfo> sfiList, bool DoEncrypt)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeRoot = Socket_Cache.FilterList.GetFilterList_XML(sfiList);
                    if (xeRoot == null)
                    {
                        return;
                    }

                    xdoc.Add(xeRoot);
                    xdoc.Save(FilePath);

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

            public static XElement GetFilterList_XML(List<Socket_FilterInfo> sfiList)
            {
                try
                {
                    XElement xeRoot = new XElement("FilterList");                 

                    foreach (Socket_FilterInfo sfi in sfiList)
                    {
                        string sIsEnable = sfi.IsEnable.ToString();
                        string sFID = sfi.FID.ToString().ToUpper();
                        string sFName = sfi.FName;
                        string sFAppointHeader = sfi.AppointHeader.ToString();
                        string sFHeaderContent = sfi.HeaderContent;
                        string sFAppointSocket = sfi.AppointSocket.ToString();
                        string sFSocketContent = sfi.SocketContent.ToString();
                        string sFAppointLength = sfi.AppointLength.ToString();
                        string sFLengthContent = sfi.LengthContent.ToString();
                        string sFAppointPort = sfi.AppointPort.ToString();
                        string sFPortContent = sfi.PortContent.ToString();
                        string sFMode = ((int)sfi.FMode).ToString();
                        string sFAction = ((int)sfi.FAction).ToString();
                        string sIsExecute = sfi.IsExecute.ToString();
                        string sFEType = ((int)sfi.FEType).ToString();
                        string sSID = sfi.SID.ToString().ToUpper();
                        string sRID = sfi.RID.ToString().ToUpper();
                        string sFFunction = Socket_Cache.Filter.GetFilterFunctionString(sfi.FFunction);
                        string sFStartFrom = ((int)sfi.FStartFrom).ToString();
                        string sIsProgressionContinuous = sfi.IsProgressionContinuous.ToString();
                        string sFProgressionStep = sfi.ProgressionStep.ToString();
                        string sIsProgressionCarry = sfi.IsProgressionCarry.ToString();
                        string sFProgressionCarryNumber = sfi.ProgressionCarryNumber.ToString();
                        string sFProgressionPosition = sfi.ProgressionPosition;
                        string sFSearch = sfi.FSearch;
                        string sFModify = sfi.FModify;

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
                            new XElement("IsProgressionContinuous", sIsProgressionContinuous),
                            new XElement("ProgressionStep", sFProgressionStep),
                            new XElement("IsProgressionCarry", sIsProgressionCarry),
                            new XElement("ProgressionCarryNumber", sFProgressionCarryNumber),
                            new XElement("ProgressionPosition", sFProgressionPosition),
                            new XElement("Search", sFSearch),
                            new XElement("Modify", sFModify)
                            );

                        xeRoot.Add(xeFilter);
                    }

                    return xeRoot;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
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

            public static void LoadFilterList_FromXDocument(XDocument xdoc)
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

                        bool bIsProgressionContinuous = false;
                        if (xeFilter.Element("IsProgressionContinuous") != null)
                        {
                            bIsProgressionContinuous = bool.Parse(xeFilter.Element("IsProgressionContinuous").Value);
                        }

                        decimal dFProgressionStep = 1;
                        if (xeFilter.Element("ProgressionStep") != null)
                        {
                            dFProgressionStep = decimal.Parse(xeFilter.Element("ProgressionStep").Value);
                        }

                        bool bIsProgressionCarry = false;
                        if (xeFilter.Element("IsProgressionCarry") != null)
                        {
                            bIsProgressionCarry = bool.Parse(xeFilter.Element("IsProgressionCarry").Value);
                        }

                        decimal dFProgressionCarryNumber = 1;
                        if (xeFilter.Element("ProgressionCarryNumber") != null)
                        {
                            dFProgressionCarryNumber = decimal.Parse(xeFilter.Element("ProgressionCarryNumber").Value);
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
                            bIsProgressionContinuous,
                            dFProgressionStep,
                            bIsProgressionCarry,
                            dFProgressionCarryNumber,
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

            public static void UpdateRobot(Socket_RobotInfo sri, string RName, DataTable RInstruction)
            {
                try
                {
                    if (sri != null)
                    {
                        sri.RName = RName;
                        sri.RInstruction = RInstruction.Copy();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//复制机器人

            public static void CopyRobot(Socket_RobotInfo sri)
            {
                try
                {
                    bool IsEnable = false;
                    Guid RID_New = Guid.NewGuid();
                    string RName_Copy = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_62), sri.RName);
                    DataTable RInstruction_Copy = sri.RInstruction.Copy();

                    Socket_Cache.Robot.AddRobot(IsEnable, RID_New, RName_Copy, RInstruction_Copy);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//删除机器人

            public static void DeleteRobot_Dialog(List<Socket_RobotInfo> sriList)
            {
                try
                {
                    if (sriList.Count > 0)
                    {
                        DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                        if (dr.Equals(DialogResult.OK))
                        {
                            foreach (Socket_RobotInfo sri in sriList)
                            {
                                Socket_Cache.RobotList.lstRobot.Remove(sri);                                
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

            #region//机器人入列表

            public static void RobotToList(Socket_RobotInfo sri)
            {
                try
                {
                    if (Socket_Cache.System.InvokeAction != null)
                    {
                        Socket_Cache.System.InvokeAction(() =>
                        {
                            Socket_Cache.RobotList.lstRobot.Add(sri);
                        });
                    }
                    else
                    {
                        Socket_Cache.RobotList.lstRobot.Add(sri);
                    }
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

            public static void UpdateRobotList_ByListAction(Socket_Cache.System.ListAction listAction, List<Socket_RobotInfo> sriList)
            {
                try
                {
                    switch (listAction)
                    {
                        case Socket_Cache.System.ListAction.Top:

                            sriList.Reverse();

                            foreach (Socket_RobotInfo sri in sriList)
                            {
                                Socket_Cache.RobotList.lstRobot.Remove(sri);
                                Socket_Cache.RobotList.lstRobot.Insert(0, sri);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Up:

                            foreach (Socket_RobotInfo sri in sriList)
                            {
                                int iIndex = Socket_Cache.RobotList.lstRobot.IndexOf(sri);

                                if (iIndex > 0)
                                {
                                    Socket_Cache.RobotList.lstRobot.Remove(sri);
                                    Socket_Cache.RobotList.lstRobot.Insert(iIndex - 1, sri);
                                }
                            }

                            break;

                        case Socket_Cache.System.ListAction.Down:

                            sriList.Reverse();

                            foreach (Socket_RobotInfo sri in sriList)
                            {
                                int iIndex = Socket_Cache.RobotList.lstRobot.IndexOf(sri);

                                if (iIndex > -1 && iIndex < Socket_Cache.RobotList.lstRobot.Count - 1)
                                {
                                    Socket_Cache.RobotList.lstRobot.Remove(sri);
                                    Socket_Cache.RobotList.lstRobot.Insert(iIndex + 1, sri);
                                }
                            }

                            break;

                        case Socket_Cache.System.ListAction.Bottom:

                            foreach (Socket_RobotInfo sri in sriList)
                            {
                                Socket_Cache.RobotList.lstRobot.Remove(sri);
                                Socket_Cache.RobotList.lstRobot.Add(sri);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Copy:

                            foreach (Socket_RobotInfo sri in sriList)
                            {
                                Socket_Cache.Robot.CopyRobot(sri);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Export:

                            string sRName = sriList[0].RName;
                            Socket_Cache.RobotList.SaveRobotList_Dialog(sRName, sriList);

                            break;

                        case Socket_Cache.System.ListAction.Delete:

                            Socket_Cache.Robot.DeleteRobot_Dialog(sriList);
                            
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
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

            #region//保存机器人列表到文件（对话框）

            public static void SaveRobotList_Dialog(string FileName, List<Socket_RobotInfo> sriList)
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
                                SaveRobotList(FilePath, sriList, true);

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

            public static void SaveRobotList(string FilePath, List<Socket_RobotInfo> sriList, bool DoEncrypt)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeRoot = Socket_Cache.RobotList.GetRobotList_XML(sriList);
                    if (xeRoot == null)
                    {
                        return;
                    }

                    xdoc.Add(xeRoot);
                    xdoc.Save(FilePath);

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

            public static XElement GetRobotList_XML(List<Socket_RobotInfo> sriList)
            {
                try
                {
                    XElement xeRoot = new XElement("RobotList");                    

                    foreach (Socket_RobotInfo sri in sriList)
                    {
                        string IsEnable = sri.IsEnable.ToString();
                        string sRID = sri.RID.ToString().ToUpper();
                        string sRName = sri.RName;
                        DataTable dtRInstruction = sri.RInstruction;

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

                    return xeRoot;                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
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

            public static void LoadRobotList_FromXDocument(XDocument xdoc)
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
        }

        #endregion

        #region//发送

        public static class Send
        {
            public static string AESKey = string.Empty;

            #region//获取发送集

            public static BindingList<Socket_PacketInfo> GetSendCollection_ByGuid(Guid SID)
            {
                BindingList<Socket_PacketInfo> sscReturn = null;

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

                return sscReturn;
            }

            #endregion

            #region//新增发送集

            public static void AddSendCollection_ByPacketInfo(Guid SID, List<Socket_PacketInfo> spiList)
            {
                try
                {
                    if (SID != null && SID != Guid.Empty && spiList.Count > 0)
                    {
                        foreach (Socket_SendInfo ssi in Socket_Cache.SendList.lstSend)
                        {
                            if (ssi.SID == SID)
                            {
                                foreach (Socket_PacketInfo spi in spiList)
                                {
                                    Socket_Cache.Send.AddSendCollection(ssi.SCollection, spi.PacketSocket, spi.PacketType, spi.PacketFrom, spi.PacketTo, spi.PacketBuffer);
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

            public static void AddSendCollection(BindingList<Socket_PacketInfo> SCollection, int Socket, Socket_Cache.SocketPacket.PacketType ptType, string PacketFrom, string PacketTo, byte[] PacketBuffer)
            {
                try
                {
                    Socket_PacketInfo spi = new Socket_PacketInfo();
                    spi.PacketSocket = Socket;
                    spi.PacketType = ptType;
                    spi.PacketFrom = PacketFrom;
                    spi.PacketTo = PacketTo;
                    spi.PacketBuffer = PacketBuffer;
                    SCollection.Add(spi);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//发送集的列表操作

            public static void UpdateSendCollection_ByListAction(BindingList<Socket_PacketInfo> SendCollection, Socket_Cache.System.ListAction listAction, List<Socket_PacketInfo> sscList)
            {
                try
                {
                    switch (listAction)
                    {
                        case Socket_Cache.System.ListAction.Top:

                            sscList.Reverse();

                            foreach (Socket_PacketInfo spi in sscList)
                            {
                                SendCollection.Remove(spi);
                                SendCollection.Insert(0, spi);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Up:

                            foreach (Socket_PacketInfo spi in sscList)
                            {
                                int iIndex = SendCollection.IndexOf(spi);

                                if (iIndex > 0)
                                {
                                    SendCollection.Remove(spi);
                                    SendCollection.Insert(iIndex - 1, spi);
                                }
                            }

                            break;

                        case Socket_Cache.System.ListAction.Down:

                            sscList.Reverse();

                            foreach (Socket_PacketInfo spi in sscList)
                            {
                                int iIndex = SendCollection.IndexOf(spi);

                                if (iIndex > -1 && iIndex < SendCollection.Count - 1)
                                {
                                    SendCollection.Remove(spi);
                                    SendCollection.Insert(iIndex + 1, spi);
                                }
                            }

                            break;

                        case Socket_Cache.System.ListAction.Bottom:

                            foreach (Socket_PacketInfo spi in sscList)
                            {
                                SendCollection.Remove(spi);
                                SendCollection.Add(spi);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Delete:

                            foreach (Socket_PacketInfo spi in sscList)
                            {
                                SendCollection.Remove(spi);                            
                            }
                            
                            break;

                        case Socket_Cache.System.ListAction.Export:

                            Socket_Cache.Send.SaveSendCollection_Dialog(string.Empty, SendCollection);
            
                            break;

                        case Socket_Cache.System.ListAction.Import:

                            Socket_Cache.Send.LoadSendCollection_Dialog(SendCollection);

                            break;

                        case Socket_Cache.System.ListAction.CleanUp:

                            if (SendCollection.Count > 0)
                            {
                                DialogResult dia = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_38));
                                if (dia.Equals(DialogResult.OK))
                                {
                                    SendCollection.Clear();
                                }                                
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
                    BindingList<Socket_PacketInfo> SCollection = new BindingList<Socket_PacketInfo>();

                    Socket_Cache.Send.AddSend(IsEnable, SID, SName, SSystemSocket, SLoopCNT, SLoopINT, SCollection, SNotes);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void AddSend(bool IsEnable, Guid SID, string SName, bool SSystemSocket, int SLoopCNT, int SLoopINT, BindingList<Socket_PacketInfo> SCollection, string SNotes)
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

            public static void UpdateSend(Socket_SendInfo ssi, string SName, bool SSystemSocket, int SLoopCNT, int SLoopINT, BindingList<Socket_PacketInfo> SCollection, string SNotes)
            {
                try
                {
                    if (ssi != null)
                    {
                        ssi.SName = SName;
                        ssi.SSystemSocket = SSystemSocket;
                        ssi.SLoopCNT = SLoopCNT;
                        ssi.SLoopINT = SLoopINT;
                        ssi.SCollection = new BindingList<Socket_PacketInfo>(SCollection.ToList());
                        ssi.SNotes = SNotes;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//复制发送

            public static void CopySend(Socket_SendInfo ssi)
            {
                try
                {
                    bool IsEnable_Copy = false;
                    Guid SID_New = Guid.NewGuid();
                    string SName_Copy = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_62), ssi.SName);
                    bool SSystemSocket_Copy = ssi.SSystemSocket;                
                    int SLoopCNT_Copy = ssi.SLoopCNT;
                    int SLoopINT_Copy = ssi.SLoopINT;
                    BindingList<Socket_PacketInfo> SCollection_Copy = new BindingList<Socket_PacketInfo>(ssi.SCollection.ToList());
                    string SNotes_Copy = ssi.SNotes;

                    Socket_Cache.Send.AddSend(IsEnable_Copy, SID_New, SName_Copy, SSystemSocket_Copy, SLoopCNT_Copy, SLoopINT_Copy, SCollection_Copy, SNotes_Copy);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//删除发送

            public static void DeleteSend_Dialog(List<Socket_SendInfo> ssiList)
            {
                try
                {
                    if (ssiList.Count > 0)
                    {
                        DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                        if (dr.Equals(DialogResult.OK))
                        {
                            foreach (Socket_SendInfo ssi in ssiList)
                            {
                                Socket_Cache.SendList.lstSend.Remove(ssi);
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
                            if (ssi.SCollection.Count > 0)
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

            public static void SaveSendCollection_Dialog(string FileName, BindingList<Socket_PacketInfo> SendCollection)
            {
                try
                {
                    if (SendCollection.Count > 0)
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

            public static void SaveSendCollection(string FilePath, BindingList<Socket_PacketInfo> SendCollection, bool DoEncrypt)
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

            private static void SaveSendCollection_ToXDocument(string FilePath, BindingList<Socket_PacketInfo> SendCollection)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeRoot = new XElement("SendCollection");
                    xdoc.Add(xeRoot);

                    foreach (Socket_PacketInfo spi in SendCollection)
                    {
                        string sBuffer = Socket_Operation.BytesToString(SocketPacket.EncodingFormat.Hex, spi.PacketBuffer);

                        XElement xeColl =
                            new XElement("Collection",
                            new XElement("Socket", spi.PacketSocket),
                            new XElement("Type", spi.PacketType),
                            new XElement("IPFrom", spi.PacketFrom),
                            new XElement("IPTo", spi.PacketTo),
                            new XElement("Buffer", sBuffer)
                            );

                        xeRoot.Add(xeColl);
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

            public static void LoadSendCollection_Dialog(BindingList<Socket_PacketInfo> SendCollection)
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

            public static void LoadSendCollection(string FilePath, BindingList<Socket_PacketInfo> SendCollection, bool LoadFromUser)
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

            private static void LoadSendCollection_FromXDocument(XDocument xdoc, BindingList<Socket_PacketInfo> SendCollection)
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

                                string sIPFrom = string.Empty;
                                if (xeSend.Element("IPFrom") != null)
                                {
                                    sIPFrom = xeSend.Element("IPFrom").Value;
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

                                Socket_Cache.Send.AddSendCollection(SendCollection, iSocket, ptType, sIPFrom, sIPTo, bBuffer);
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

                                string sIPFrom = string.Empty;
                                if (xeCollection.Element("IPFrom") != null)
                                {
                                    sIPFrom = xeCollection.Element("IPFrom").Value;
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

                                Socket_Cache.Send.AddSendCollection(SendCollection, iSocket, ptType, sIPFrom, sIPTo, bBuffer);
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
                    if (Socket_Cache.System.InvokeAction != null)
                    {
                        Socket_Cache.System.InvokeAction(() =>
                        {
                            Socket_Cache.SendList.lstSend.Add(ssi);
                        });
                    }
                    else
                    {
                        Socket_Cache.SendList.lstSend.Add(ssi);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//发送列表的列表操作

            public static void UpdateSendList_ByListAction(Socket_Cache.System.ListAction listAction, List<Socket_SendInfo> ssiList)
            {
                try
                {
                    switch (listAction)
                    {
                        case Socket_Cache.System.ListAction.Top:

                            ssiList.Reverse();

                            foreach (Socket_SendInfo ssi in ssiList)
                            {
                                Socket_Cache.SendList.lstSend.Remove(ssi);
                                Socket_Cache.SendList.lstSend.Insert(0, ssi);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Up:

                            foreach (Socket_SendInfo ssi in ssiList)
                            {
                                int iIndex = Socket_Cache.SendList.lstSend.IndexOf(ssi);

                                if (iIndex > 0)
                                {
                                    Socket_Cache.SendList.lstSend.Remove(ssi);
                                    Socket_Cache.SendList.lstSend.Insert(iIndex - 1, ssi);
                                }
                            }

                            break;

                        case Socket_Cache.System.ListAction.Down:

                            ssiList.Reverse();

                            foreach (Socket_SendInfo ssi in ssiList)
                            {
                                int iIndex = Socket_Cache.SendList.lstSend.IndexOf(ssi);

                                if (iIndex > -1 && iIndex < Socket_Cache.SendList.lstSend.Count - 1)
                                {
                                    Socket_Cache.SendList.lstSend.Remove(ssi);
                                    Socket_Cache.SendList.lstSend.Insert(iIndex + 1, ssi);
                                }
                            }

                            break;

                        case Socket_Cache.System.ListAction.Bottom:

                            foreach (Socket_SendInfo ssi in ssiList)
                            {
                                Socket_Cache.SendList.lstSend.Remove(ssi);
                                Socket_Cache.SendList.lstSend.Add(ssi);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Copy:

                            foreach (Socket_SendInfo ssi in ssiList)
                            {
                                Socket_Cache.Send.CopySend(ssi);
                            }

                            break;

                        case Socket_Cache.System.ListAction.Export:

                            string sSName = ssiList[0].SName;
                            Socket_Cache.SendList.SaveSendList_Dialog(sSName, ssiList);

                            break;

                        case Socket_Cache.System.ListAction.Delete:

                            Socket_Cache.Send.DeleteSend_Dialog(ssiList);

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
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

            public static void SendListClear()
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

            #region//从数据库加载发送列表

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
                        BindingList<Socket_PacketInfo> SCollection = new BindingList<Socket_PacketInfo>();

                        DataTable dtSCollection = Socket_Cache.DataBase.SelectTable_SendCollection(SID);
                        foreach (DataRow row in dtSCollection.Rows)
                        {
                            int Socket = Convert.ToInt32(row["Socket"]);
                            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.GetPacketType_ByString(row["Type"].ToString());
                            string IPFrom = row["IPFrom"].ToString();
                            string IPTo = row["IPTo"].ToString();
                            byte[] Buffer = (byte[])row["Buffer"];

                            Socket_Cache.Send.AddSendCollection(SCollection, Socket, ptType, IPFrom, IPTo, Buffer);
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

            public static void SaveSendList_Dialog(string FileName, List<Socket_SendInfo> ssiList)
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
                                SaveSendList(FilePath, ssiList, true);

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

            private static void SaveSendList(string FilePath, List<Socket_SendInfo> ssiList, bool DoEncrypt)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeRoot = Socket_Cache.SendList.GetSendList_XML(ssiList);
                    if (xeRoot == null) 
                    {
                        return;
                    }

                    xdoc.Add(xeRoot);
                    xdoc.Save(FilePath);

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

            public static XElement GetSendList_XML(List<Socket_SendInfo> ssiList)
            {
                try
                {
                    XElement xeRoot = new XElement("SendList");               

                    foreach (Socket_SendInfo ssi in ssiList)
                    {
                        XElement xeSend =
                            new XElement("Send",
                            new XElement("IsEnable", ssi.IsEnable.ToString()),
                            new XElement("ID", ssi.SID.ToString().ToUpper()),
                            new XElement("Name", ssi.SName),
                            new XElement("SystemSocket", ssi.SSystemSocket.ToString()),
                            new XElement("LoopCNT", ssi.SLoopCNT.ToString()),
                            new XElement("LoopINT", ssi.SLoopINT.ToString()),
                            new XElement("Notes", ssi.SNotes)
                            );

                        if (ssi.SCollection.Count > 0)
                        {
                            XElement xeCollection = new XElement("SendCollection");

                            foreach (Socket_PacketInfo spi in ssi.SCollection)
                            {
                                string sBuffer = Socket_Operation.BytesToString(SocketPacket.EncodingFormat.Hex, spi.PacketBuffer);

                                XElement xeColl =
                                    new XElement("Collection",
                                    new XElement("Socket", spi.PacketSocket),
                                    new XElement("Type", spi.PacketType),
                                    new XElement("IPTo", spi.PacketTo),
                                    new XElement("Buffer", sBuffer)
                                    );

                                xeCollection.Add(xeColl);
                            }

                            xeSend.Add(xeCollection);
                        }

                        xeRoot.Add(xeSend);
                    }

                    return xeRoot;
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
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

            public static void LoadSendList_FromXDocument(XDocument xdoc)
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

                        BindingList<Socket_PacketInfo> SCollection = new BindingList<Socket_PacketInfo>();

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

                                string sIPFrom = string.Empty;
                                if (xeCollection.Element("IPFrom") != null)
                                {
                                    sIPFrom = xeCollection.Element("IPFrom").Value;
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

                                Socket_Cache.Send.AddSendCollection(SCollection, iSocket, ptType, sIPFrom, sIPTo, bBuffer);                                
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
                Socket_Cache.DataBase.CreateTable_ProxyMapLocal();
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
                        sql += "ProxyConfig_Enable_MapLocal BOOLEAN DEFAULT 0,";//代理模式 - 启用本地代理映射
                        sql += "ProxyConfig_Enable_ExternalProxy BOOLEAN DEFAULT 0,";//代理模式 - 启用外部代理
                        sql += "ProxyConfig_ExternalProxy_IP TEXT,";//代理模式 - 外部代理IP
                        sql += "ProxyConfig_ExternalProxy_Port INTEGER DEFAULT 8889,";//代理模式 - 外部代理端口               
                        sql += "ProxyConfig_Enable_ExternalProxy_AppointPort BOOLEAN DEFAULT 0,";//代理模式 - 启用指定代理端口
                        sql += "ProxyConfig_ExternalProxy_AppointPort TEXT,";//代理模式 - 指定代理端口
                        sql += "ProxyConfig_Enable_ExternalProxy_Auth BOOLEAN DEFAULT 0,";//代理模式 - 启用外部代理认证
                        sql += "ProxyConfig_ExternalProxy_UserName TEXT,";//代理模式 - 外部代理用户名
                        sql += "ProxyConfig_ExternalProxy_PassWord TEXT,";//代理模式 - 外部代理密码
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
                        sql += "InjectionConfig_HotKey1 TEXT,";//注入模式 - 快捷键1
                        sql += "InjectionConfig_HotKey2 TEXT,";//注入模式 - 快捷键2
                        sql += "InjectionConfig_HotKey3 TEXT,";//注入模式 - 快捷键3
                        sql += "InjectionConfig_HotKey4 TEXT,";//注入模式 - 快捷键4
                        sql += "InjectionConfig_HotKey5 TEXT,";//注入模式 - 快捷键5
                        sql += "InjectionConfig_HotKey6 TEXT,";//注入模式 - 快捷键6
                        sql += "InjectionConfig_HotKey7 TEXT,";//注入模式 - 快捷键7
                        sql += "InjectionConfig_HotKey8 TEXT,";//注入模式 - 快捷键8
                        sql += "InjectionConfig_HotKey9 TEXT,";//注入模式 - 快捷键9
                        sql += "InjectionConfig_HotKey10 TEXT,";//注入模式 - 快捷键10
                        sql += "InjectionConfig_HotKey11 TEXT,";//注入模式 - 快捷键11
                        sql += "InjectionConfig_HotKey12 TEXT,";//注入模式 - 快捷键12
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
                        sql += "ProxyConfig_Enable_MapLocal,";
                        sql += "ProxyConfig_Enable_ExternalProxy,";
                        sql += "ProxyConfig_ExternalProxy_IP,";
                        sql += "ProxyConfig_ExternalProxy_Port,";                    
                        sql += "ProxyConfig_Enable_ExternalProxy_AppointPort,";
                        sql += "ProxyConfig_ExternalProxy_AppointPort,";
                        sql += "ProxyConfig_Enable_ExternalProxy_Auth,";
                        sql += "ProxyConfig_ExternalProxy_UserName,";
                        sql += "ProxyConfig_ExternalProxy_PassWord,";
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
                        sql += "InjectionConfig_HotKey1,";
                        sql += "InjectionConfig_HotKey2,";
                        sql += "InjectionConfig_HotKey3,";
                        sql += "InjectionConfig_HotKey4,";
                        sql += "InjectionConfig_HotKey5,";
                        sql += "InjectionConfig_HotKey6,";
                        sql += "InjectionConfig_HotKey7,";
                        sql += "InjectionConfig_HotKey8,";
                        sql += "InjectionConfig_HotKey9,";
                        sql += "InjectionConfig_HotKey10,";
                        sql += "InjectionConfig_HotKey11,";
                        sql += "InjectionConfig_HotKey12,";
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
                        sql += "@ProxyConfig_Enable_MapLocal,";
                        sql += "@ProxyConfig_Enable_ExternalProxy,";
                        sql += "@ProxyConfig_ExternalProxy_IP,";
                        sql += "@ProxyConfig_ExternalProxy_Port,";                    
                        sql += "@ProxyConfig_Enable_ExternalProxy_AppointPort,";
                        sql += "@ProxyConfig_ExternalProxy_AppointPort,";
                        sql += "@ProxyConfig_Enable_ExternalProxy_Auth,";
                        sql += "@ProxyConfig_ExternalProxy_UserName,";
                        sql += "@ProxyConfig_ExternalProxy_PassWord,";
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
                        sql += "@InjectionConfig_HotKey1,";
                        sql += "@InjectionConfig_HotKey2,";
                        sql += "@InjectionConfig_HotKey3,";
                        sql += "@InjectionConfig_HotKey4,";
                        sql += "@InjectionConfig_HotKey5,";
                        sql += "@InjectionConfig_HotKey6,";
                        sql += "@InjectionConfig_HotKey7,";
                        sql += "@InjectionConfig_HotKey8,";
                        sql += "@InjectionConfig_HotKey9,";
                        sql += "@InjectionConfig_HotKey10,";
                        sql += "@InjectionConfig_HotKey11,";
                        sql += "@InjectionConfig_HotKey12,";
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
                            cmd.Parameters.AddWithValue("@ProxyConfig_Enable_MapLocal", Socket_Cache.ProxyMapping.Enable_MapLocal);
                            cmd.Parameters.AddWithValue("@ProxyConfig_Enable_ExternalProxy", Socket_Cache.SocketProxy.Enable_ExternalProxy);
                            cmd.Parameters.AddWithValue("@ProxyConfig_ExternalProxy_IP", Socket_Cache.SocketProxy.ExternalProxy_IP);
                            cmd.Parameters.AddWithValue("@ProxyConfig_ExternalProxy_Port", Socket_Cache.SocketProxy.ExternalProxy_Port);                          
                            cmd.Parameters.AddWithValue("@ProxyConfig_Enable_ExternalProxy_AppointPort", Socket_Cache.SocketProxy.Enable_ExternalProxy_AppointPort);
                            cmd.Parameters.AddWithValue("@ProxyConfig_ExternalProxy_AppointPort", Socket_Cache.SocketProxy.ExternalProxy_AppointPort);
                            cmd.Parameters.AddWithValue("@ProxyConfig_Enable_ExternalProxy_Auth", Socket_Cache.SocketProxy.Enable_ExternalProxy_Auth);
                            cmd.Parameters.AddWithValue("@ProxyConfig_ExternalProxy_UserName", Socket_Cache.SocketProxy.ExternalProxy_UserName);
                            cmd.Parameters.AddWithValue("@ProxyConfig_ExternalProxy_PassWord", Socket_Cache.SocketProxy.ExternalProxy_PassWord);
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
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey1", Socket_Cache.SocketPacket.HotKey1);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey2", Socket_Cache.SocketPacket.HotKey2);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey3", Socket_Cache.SocketPacket.HotKey3);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey4", Socket_Cache.SocketPacket.HotKey4);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey5", Socket_Cache.SocketPacket.HotKey5);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey6", Socket_Cache.SocketPacket.HotKey6);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey7", Socket_Cache.SocketPacket.HotKey7);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey8", Socket_Cache.SocketPacket.HotKey8);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey9", Socket_Cache.SocketPacket.HotKey9);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey10", Socket_Cache.SocketPacket.HotKey10);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey11", Socket_Cache.SocketPacket.HotKey11);
                            cmd.Parameters.AddWithValue("@InjectionConfig_HotKey12", Socket_Cache.SocketPacket.HotKey12);
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
                        sql += "IsProgressionContinuous BOOLEAN DEFAULT 0,";
                        sql += "ProgressionStep INTEGER DEFAULT 1,";
                        sql += "IsProgressionCarry BOOLEAN DEFAULT 0,";
                        sql += "ProgressionCarryNumber INTEGER DEFAULT 1,";
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
                        sql += "IsProgressionContinuous,";
                        sql += "ProgressionStep,";
                        sql += "IsProgressionCarry,";
                        sql += "ProgressionCarryNumber,";
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
                        sql += "@IsProgressionContinuous,";
                        sql += "@ProgressionStep,";
                        sql += "@IsProgressionCarry,";
                        sql += "@ProgressionCarryNumber,";
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
                            cmd.Parameters.AddWithValue("@IsProgressionContinuous", sfi.IsProgressionContinuous);
                            cmd.Parameters.AddWithValue("@ProgressionStep", sfi.ProgressionStep);
                            cmd.Parameters.AddWithValue("@IsProgressionCarry", sfi.IsProgressionCarry);
                            cmd.Parameters.AddWithValue("@ProgressionCarryNumber", sfi.ProgressionCarryNumber);
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
                        sql += "IPFrom TEXT NOT NULL,";
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
                        string sql = "SELECT * FROM SendCollection WHERE GUID = @GUID;";

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

                        foreach (Socket_PacketInfo spi in ssi.SCollection)
                        {
                            sql = "INSERT INTO SendCollection (";
                            sql += "GUID,";
                            sql += "Socket,";
                            sql += "Type,";
                            sql += "IPFrom,";
                            sql += "IPTo,";
                            sql += "Buffer";
                            sql += ") VALUES (";
                            sql += "@GUID,";
                            sql += "@Socket,";
                            sql += "@Type,";
                            sql += "@IPFrom,";
                            sql += "@IPTo,";
                            sql += "@Buffer";
                            sql += ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@GUID", ssi.SID.ToString().ToUpper());
                                cmd.Parameters.AddWithValue("@Socket", spi.PacketSocket);
                                cmd.Parameters.AddWithValue("@Type", spi.PacketType);
                                cmd.Parameters.AddWithValue("@IPFrom", spi.PacketFrom);
                                cmd.Parameters.AddWithValue("@IPTo", spi.PacketTo);
                                cmd.Parameters.AddWithValue("@Buffer", spi.PacketBuffer);
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
                        sql += "LoginTime TIMESTAMP,";
                        sql += "LoginIP TEXT,";
                        sql += "IPLocation TEXT,";
                        sql += "IsLimitLinks BOOLEAN DEFAULT 0,";
                        sql += "LimitLinks INTEGER DEFAULT 1,";
                        sql += "IsLimitDevices BOOLEAN DEFAULT 0,";
                        sql += "LimitDevices INTEGER DEFAULT 1,";
                        sql += "IsExpiry BOOLEAN DEFAULT 0,";
                        sql += "ExpiryTime TIMESTAMP,";                        
                        sql += "CreateTime TIMESTAMP";
                        sql += ");";

                        sql += "CREATE TABLE IF NOT EXISTS ProxyAccount_LoginInfo (";
                        sql += "GUID TEXT NOT NULL,";
                        sql += "LoginTime TIMESTAMP,";
                        sql += "LoginIP TEXT,";
                        sql += "IPLocation TEXT,";
                        sql += "FOREIGN KEY (GUID) REFERENCES ProxyAccount(GUID),";
                        sql += "UNIQUE (GUID, LoginIP)";
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

            public static DataTable SelectTable_ProxyAccount_LoginInfo(Guid guid)
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM ProxyAccount_LoginInfo WHERE GUID = @GUID;";

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

            public static void DeleteTable_ProxyAccount_LoginInfo()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM ProxyAccount_LoginInfo WHERE GUID NOT IN (SELECT GUID FROM ProxyAccount);";

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

            public static void UpdateTable_ProxyAccount_LoginInfo(Guid guid, string LoginIP)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "UPDATE ProxyAccount_LoginInfo SET LoginTime = @LoginTime WHERE GUID = @guid AND LoginIP = @LoginIP;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@LoginTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@guid", guid);
                            cmd.Parameters.AddWithValue("@LoginIP", LoginIP);

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

            public static void InsertTable_ProxyAccount()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(Socket_Cache.DataBase.conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sql = "INSERT INTO ProxyAccount (" +
                                         "GUID, IsEnable, UserName, PassWord, LoginTime, LoginIP, IPLocation, IsLimitLinks, LimitLinks, IsLimitDevices, LimitDevices, IsExpiry, ExpiryTime, CreateTime" +
                                         ") VALUES (" +
                                         "@GUID, @IsEnable, @UserName, @PassWord, @LoginTime, @LoginIP, @IPLocation, @IsLimitLinks, @LimitLinks, @IsLimitDevices, @LimitDevices, @IsExpiry, @ExpiryTime, @CreateTime" +
                                         ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.Add(new SQLiteParameter("@GUID", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@IsEnable", DbType.Boolean));
                                cmd.Parameters.Add(new SQLiteParameter("@UserName", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@PassWord", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@LoginTime", DbType.DateTime));
                                cmd.Parameters.Add(new SQLiteParameter("@LoginIP", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@IPLocation", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@IsLimitLinks", DbType.Boolean));
                                cmd.Parameters.Add(new SQLiteParameter("@LimitLinks", DbType.Int32));
                                cmd.Parameters.Add(new SQLiteParameter("@IsLimitDevices", DbType.Boolean));
                                cmd.Parameters.Add(new SQLiteParameter("@LimitDevices", DbType.Int32));
                                cmd.Parameters.Add(new SQLiteParameter("@IsExpiry", DbType.Boolean));
                                cmd.Parameters.Add(new SQLiteParameter("@ExpiryTime", DbType.DateTime));
                                cmd.Parameters.Add(new SQLiteParameter("@CreateTime", DbType.DateTime));

                                foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                                {
                                    cmd.Parameters["@GUID"].Value = pai.AID.ToString().ToUpper();
                                    cmd.Parameters["@IsEnable"].Value = pai.IsEnable;
                                    cmd.Parameters["@UserName"].Value = pai.UserName;
                                    cmd.Parameters["@PassWord"].Value = pai.PassWord;
                                    cmd.Parameters["@LoginTime"].Value = pai.LoginTime;
                                    cmd.Parameters["@LoginIP"].Value = pai.LoginIP;
                                    cmd.Parameters["@IPLocation"].Value = pai.IPLocation;
                                    cmd.Parameters["@IsLimitLinks"].Value = pai.IsLimitLinks;
                                    cmd.Parameters["@LimitLinks"].Value = pai.LimitLinks;
                                    cmd.Parameters["@IsLimitDevices"].Value = pai.IsLimitDevices;
                                    cmd.Parameters["@LimitDevices"].Value = pai.LimitDevices;
                                    cmd.Parameters["@IsExpiry"].Value = pai.IsExpiry;
                                    cmd.Parameters["@ExpiryTime"].Value = pai.ExpiryTime;
                                    cmd.Parameters["@CreateTime"].Value = pai.CreateTime;

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void InsertOrUpdateTable_ProxyAccount_LoginInfo(Proxy_AccountInfo pai)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sql = "INSERT INTO ProxyAccount_LoginInfo (GUID, LoginTime, LoginIP, IPLocation)";
                            sql += "VALUES (@GUID, @LoginTime, @LoginIP, @IPLocation)";
                            sql += "ON CONFLICT(GUID, LoginIP)";
                            sql += "DO UPDATE SET LoginTime = @LoginTime;";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@GUID", pai.AID.ToString().ToUpper());
                                cmd.Parameters.AddWithValue("@LoginTime", pai.LoginTime);
                                cmd.Parameters.AddWithValue("@LoginIP", pai.LoginIP);
                                cmd.Parameters.AddWithValue("@IPLocation", pai.IPLocation);

                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//本地代理映射

            private static bool CreateTable_ProxyMapLocal()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS ProxyMapLocal (";                   
                        sql += "IsEnable BOOLEAN DEFAULT 0,";
                        sql += "ProtocolType TEXT NOT NULL,";
                        sql += "Host TEXT NOT NULL,";
                        sql += "Port INTEGER DEFAULT 80,";
                        sql += "RemotePath TEXT,";
                        sql += "LocalPath TEXT NOT NULL";
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

            public static DataTable SelectTable_ProxyMapLocal()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM ProxyMapLocal;";

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

            public static void DeleteTable_ProxyMapLocal()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM ProxyMapLocal;";

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

            public static void InsertTable_ProxyMapLocal()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(Socket_Cache.DataBase.conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sql = "INSERT INTO ProxyMapLocal (" +
                                        "IsEnable, ProtocolType, Host, Port, RemotePath, LocalPath" +
                                        ") VALUES (" +
                                        "@IsEnable, @ProtocolType, @Host, @Port, @RemotePath, @LocalPath" +
                                        ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.Add(new SQLiteParameter("@IsEnable", DbType.Boolean));
                                cmd.Parameters.Add(new SQLiteParameter("@ProtocolType", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@Host", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@Port", DbType.Int32));
                                cmd.Parameters.Add(new SQLiteParameter("@RemotePath", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@LocalPath", DbType.String));

                                foreach (Proxy_MapLocal pml in Socket_Cache.ProxyMapping.lstMapLocal)
                                {
                                    cmd.Parameters["@IsEnable"].Value = pml.IsEnable;
                                    cmd.Parameters["@ProtocolType"].Value = pml.ProtocolType;
                                    cmd.Parameters["@Host"].Value = pml.Host;
                                    cmd.Parameters["@Port"].Value = pml.Port;
                                    cmd.Parameters["@RemotePath"].Value = pml.RemotePath ?? (object)DBNull.Value;
                                    cmd.Parameters["@LocalPath"].Value = pml.LocalPath;

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
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
