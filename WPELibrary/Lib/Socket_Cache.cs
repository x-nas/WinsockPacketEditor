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

namespace WPELibrary.Lib
{
    public static class Socket_Cache
    {
        #region//结构定义

        public enum PWType
        {
            Import,
            Export,
        }

        public enum ListAction
        {
            Top,
            Up,
            Down,
            Bottom,
            Copy,
            Export,
            Delete,
            CleanUp,
        }

        public enum LogType
        {
            Socket,
            Proxy,
        }

        #endregion

        public static IntPtr MainHandle = IntPtr.Zero;
        public static long TotalPackets = 0;
        public static long Total_SendBytes = 0;
        public static long Total_RecvBytes = 0;        
        public static bool SpeedMode;      
        public static byte[] bByteBuff = new byte[0];
        public static bool Support_WS1, Support_WS2;
        public static bool HookSend, HookSendTo, HookRecv, HookRecvFrom, HookWSASend, HookWSASendTo, HookWSARecv, HookWSARecvFrom;
        public static bool CheckNotShow, CheckSize, CheckSocket, CheckIP, CheckPort, CheckHead, CheckData;
        public static string CheckSocket_Value, CheckLength_Value, CheckIP_Value, CheckPort_Value, CheckHead_Value, CheckData_Value;

        #region//Socket 代理

        public static class SocketProxy
        {
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
                PassWord = 2,
            }

            public enum AddressType : byte
            {
                IPV4 = 1,
                Domain = 3,
                IPV6 = 4,
            }

            public enum CommandType : byte
            {
                Connect = 1,
                Bind = 2,
                UDP = 3,
            }

            public enum DataType : byte
            {
                Request = 0,
                Response = 1,
            }

            #endregion
            
            public static bool bIsListening = false;
            public static ushort Port = 8889;
        }

        #endregion               

        #region//代理队列

        public static class SocketProxyQueue
        {
            public static ConcurrentQueue<Socket_ProxyData> qSocket_ProxyData = new ConcurrentQueue<Socket_ProxyData>();

            #region//代理数据入队列

            public static void ProxyDataToQueue(Socket_ProxyInfo spi, Socket_Cache.SocketProxy.DataType DataType)
            {
                try
                {
                    Socket_ProxyData spd = new Socket_ProxyData(spi.IPAddress, spi.Domain, spi.Port, DataType);

                    switch (DataType)
                    { 
                        case SocketProxy.DataType.Request:
                            spd.Buffer = spi.ClientData;
                            break;

                        case SocketProxy.DataType.Response:
                            spd.Buffer = spi.TargetData;
                            break;
                    }

                    qSocket_ProxyData.Enqueue(spd);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//清除队列数据

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
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion

        #region//代理列表

        public static class SocketProxyList
        {
            public static BindingList<Socket_ProxyInfo> lstSocketProxy = new BindingList<Socket_ProxyInfo>();
            public delegate void SocketProxyReceived(Socket_ProxyInfo spi);
            public static event SocketProxyReceived RecSocketProxy;

            public static BindingList<Socket_ProxyData> lstProxyData = new BindingList<Socket_ProxyData>();
            public delegate void ProxyDataReceived(Socket_ProxyData spd);
            public static event ProxyDataReceived RecProxyData;

            #region//代理入列表

            public static void SocketProxyToList(Socket_ProxyInfo spi)
            {
                try
                {
                    lstSocketProxy.Add(spi);
                    RecSocketProxy?.Invoke(spi);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//代理数据入列表

            public static void ProxyDataToList()
            {
                try
                {
                    if (Socket_Cache.SocketProxyQueue.qSocket_ProxyData.TryDequeue(out Socket_ProxyData spd))
                    {
                        RecProxyData?.Invoke(spd);
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//清除代理数据列表

            public static void ResetProxyDataList()
            {
                try
                {
                    lstProxyData.Clear();
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

            #region//结构定义

            [StructLayout(LayoutKind.Explicit)]

            public struct in_addr
            {
                [FieldOffset(0)]
                public S_un _S_un;

                [StructLayout(LayoutKind.Explicit)]

                public struct S_un
                {
                    [FieldOffset(0)]
                    public S_un_b S_un_b;

                    [FieldOffset(0)]
                    public S_un_w S_un_w;

                    [FieldOffset(0)]
                    public uint S_addr;
                }

                [StructLayout(LayoutKind.Sequential)]

                public struct S_un_b
                {
                    public byte s_b1;
                    public byte s_b2;
                    public byte s_b3;
                    public byte s_b4;
                }

                [StructLayout(LayoutKind.Sequential)]

                public struct S_un_w
                {
                    public ushort s_w1;
                    public ushort s_w2;
                }
            }            

            public struct sockaddr
            {
                public short sin_family;
                public ushort sin_port;
                public in_addr sin_addr;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
                public byte[] sin_zero;
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
                public IntPtr InternalLow;
                public IntPtr InternalHigh;
                public int OffsetLow;
                public int OffsetHigh;
                public IntPtr EventHandle;
            }

            public enum PacketType
            {
                Send,
                SendTo,
                Recv,
                RecvFrom,                
                WSASend,
                WSASendTo,
                WSARecv,
                WSARecvFrom,
            }

            public enum IPType
            {
                From,
                To,
            }

            public enum EncodingFormat
            {
                Default,
                Char,
                Byte,
                Bytes,
                Short,
                UShort,
                Int32,
                UInt32,
                Int64,
                UInt64,
                Float,
                Double,
                Bin,
                GBK,
                Unicode,
                ASCII,
                Hex,
                UTF7,
                UTF8,
                UTF16,
                UTF32,
                Base64,
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
                        case Socket_Cache.SocketPacket.PacketType.Send:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_54);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.Recv:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_55);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.SendTo:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_56);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.RecvFrom:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_57);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                            sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_58);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
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
                        case Socket_Cache.SocketPacket.PacketType.Send:
                            imgReturn = Properties.Resources.sent;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.Recv:
                            imgReturn = Properties.Resources.received;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.SendTo:
                            imgReturn = Properties.Resources.sent;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.RecvFrom:
                            imgReturn = Properties.Resources.received;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                            imgReturn = Properties.Resources.sent;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
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
                Socket_Cache.SocketPacket.sockaddr sAddr,
                Socket_Cache.Filter.FilterAction pAction)
            {
                try
                {
                    if (!Socket_Cache.SpeedMode)
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

            public static FindOptions FindOptions = new FindOptions();

            public static BindingList<Socket_PacketInfo> lstRecPacket = new BindingList<Socket_PacketInfo>();

            public delegate void SocketPacketReceived(Socket_PacketInfo si);
            public static event SocketPacketReceived RecSocketPacket;

            #region//封包入列表

            public static void SocketToList(int iMax_DataLen)
            {
                try
                {
                    if (SocketQueue.qSocket_PacketInfo.TryDequeue(out Socket_PacketInfo spi))
                    {
                        if (Socket_Operation.ISShowSocketPacket_ByFilter(spi))
                        {                            
                            int iPacketLen = spi.PacketLen;
                            byte[] bBuffer = spi.PacketBuffer;
                            
                            spi.PacketData = Socket_Operation.GetPacketData_Hex(bBuffer, iMax_DataLen);

                            Socket_Cache.SocketPacket.PacketType ptType = spi.PacketType;

                            switch (ptType)
                            {
                                case Socket_Cache.SocketPacket.PacketType.Send:                                    
                                    SocketQueue.Send_CNT++;
                                    break;

                                case Socket_Cache.SocketPacket.PacketType.SendTo:                                    
                                    SocketQueue.SendTo_CNT++;
                                    break;

                                case Socket_Cache.SocketPacket.PacketType.Recv:                                    
                                    SocketQueue.Recv_CNT++;
                                    break;

                                case Socket_Cache.SocketPacket.PacketType.RecvFrom:                                    
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
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//搜索封包列表

            public static int FindSocketList(Socket_Cache.SocketPacket.EncodingFormat efFormat, int FromIndex, string SearchData, bool MatchCase)
            {
                int iResult = -1;

                try
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
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iResult;
            }

            #endregion

            #region//保存封包列表为Excel

            public static void SaveSocketListToExcel()
            {
                int iSuccess = 0;

                try
                {
                    if (Socket_Cache.SocketList.lstRecPacket.Count > 0)
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

                            string sColTitle = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_77);
                            sw.WriteLine(sColTitle);

                            foreach (Socket_PacketInfo spi in Socket_Cache.SocketList.lstRecPacket)
                            {
                                try
                                {
                                    string sColValue = "";

                                    string sTime = spi.PacketTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff");                                   
                                    string sType = spi.PacketType.ToString();
                                    string sSocket = spi.PacketSocket.ToString();
                                    string sFrom = spi.PacketFrom;
                                    string sTo = spi.PacketTo;
                                    string sLen = spi.PacketLen.ToString();
                                    byte[] bBuff = spi.PacketBuffer;
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
                        }
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
            public static bool AutoRoll;
            public static bool AutoClear;
            public static decimal AutoClear_Value;
            public static BindingList<Socket_LogInfo> lstSocketLog = new BindingList<Socket_LogInfo>();
            public static BindingList<Socket_LogInfo> lstProxyLog = new BindingList<Socket_LogInfo>();

            public delegate void SocketLogReceived(Socket_LogInfo sl);
            public delegate void ProxyLogReceived(Socket_LogInfo sli);

            public static event SocketLogReceived RecSocketLog;
            public static event ProxyLogReceived RecProxyLog;

            #region//日志入列表

            public static void LogToList(Socket_Cache.LogType logType)
            {
                try
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

            public static void SaveLogListToExcel()
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

                        Socket_Cache.FilterList.lstFilter.Add(sfi);
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
                Socket_Cache.Filter.FilterMode FMode;

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
                Socket_Cache.Filter.FilterAction FAction;

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
                Socket_Cache.Filter.FilterStartFrom FStartFrom;

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
                string sReturn = "";

                try
                {
                    for (int i = 0; i < bBuffer.Length; i++)
                    {
                        string sHex = bBuffer[i].ToString("X2");
                        sReturn += i.ToString() + "-" + sHex + ",";
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
                        case Socket_Cache.SocketPacket.PacketType.Send:
                            ffReturn.Send = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.SendTo:
                            ffReturn.SendTo = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.Recv:
                            ffReturn.Recv = true;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.RecvFrom:
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

            #region//获取滤镜是否启用

            public static bool GetIsCheck_ByFilterIndex(int iFIndex)
            {
                bool bReturn = false;

                try
                {
                    if (iFIndex > -1)
                    {
                        bReturn = Socket_Cache.FilterList.lstFilter[iFIndex].IsEnable;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
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
                        if (CheckFilterFunction_ByPacketType(ptType, sfi.FFunction))
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
                        case Socket_Cache.SocketPacket.PacketType.Send:
                            bReturn = ffFunction.Send;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.SendTo:
                            bReturn = ffFunction.SendTo;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.Recv:
                            bReturn = ffFunction.Recv;
                            break;

                        case Socket_Cache.SocketPacket.PacketType.RecvFrom:
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
                            if (!string.IsNullOrEmpty(sSearch) && sSearch.IndexOf("-") > 0)
                            {
                                int iIndex = int.Parse(sSearch.Split('-')[0]);
                                string sValue = sSearch.Split('-')[1];

                                if (iIndex >= 0 && iIndex < bBuffer.Length)
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
                            int iIndex = int.Parse(slSearch[i].Split('-')[0]);
                            string sValue = slSearch[i].Split('-')[1];
                            byte bValue = Convert.ToByte(sValue, 16);

                            dSearchIndex.Add(i, iIndex);
                            dSearchValue.Add(i, bValue);
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
                            if (!string.IsNullOrEmpty(sModify) && sModify.IndexOf("-") > 0)
                            {
                                int iIndex = int.Parse(sModify.Split('-')[0]);
                                string sValue = sModify.Split('-')[1];

                                if (iIndex >= 0 && iIndex < bBuffer.Length)
                                {
                                    byte bValue = Convert.ToByte(sValue, 16);
                                    bBuffer[iIndex] = bValue;                                    
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
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            if (!string.IsNullOrEmpty(sModify) && sModify.IndexOf("-") > 0)
                            {
                                if (int.TryParse(sModify.Split('-')[0], out int iIndex))
                                {
                                    string sValue = sModify.Split('-')[1];

                                    if (FStartFrom == Socket_Cache.Filter.FilterStartFrom.Position)
                                    {
                                        iIndex = iMatch + (iIndex - (Socket_Cache.Filter.FilterSize_MaxLen / 2));
                                    }

                                    if (iIndex >= 0 && iIndex < bBuffer.Length)
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
                                        iIndex = iMatch + (iIndex - (Socket_Cache.Filter.FilterSize_MaxLen / 2));
                                    }

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
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Cache.FilterList.Execute FLExecute;

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

                        if (Socket_Cache.Filter.CheckFilter_IsEffective(iSocket, bBuffer, ptType, sfi))
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

                                if (!Socket_Cache.SpeedMode)
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

            public static Socket_Cache.Filter.FilterAction DoWSAFilterList(Socket_Cache.SocketPacket.PacketType ptType, Int32 iSocket, IntPtr lpBuffers, int dwBufferCount, int BytesCNT)
            {
                Socket_Cache.Filter.FilterAction faReturn = Socket_Cache.Filter.FilterAction.None;

                try
                {
                    int BytesLeft = BytesCNT;

                    for (int i = 0; i < dwBufferCount; i++)
                    {
                        if (BytesLeft > 0)
                        {
                            IntPtr lpNewBuffer = IntPtr.Add(lpBuffers, Marshal.SizeOf(typeof(Socket_Cache.SocketPacket.WSABUF)) * i);
                            Socket_Cache.SocketPacket.WSABUF wsBuffer = Marshal.PtrToStructure<Socket_Cache.SocketPacket.WSABUF>(lpNewBuffer);

                            int iBuffLen = 0;

                            if (wsBuffer.len >= BytesLeft)
                            {
                                iBuffLen = BytesLeft;
                            }
                            else
                            {
                                iBuffLen = wsBuffer.len;
                            }

                            BytesLeft -= iBuffLen;

                            byte[] bBuffer = new byte[iBuffLen];
                            Marshal.Copy(wsBuffer.buf, bBuffer, 0, iBuffLen);
                            Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, iSocket, bBuffer);
                            Marshal.Copy(bBuffer, 0, wsBuffer.buf, iBuffLen);

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
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.Export);
                            pwForm.ShowDialog();

                            string FilePath = sfdSaveFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                SaveFilterList(FilePath, FilterIndex, true);
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
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.Import);
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
                Press,
                Down,
                Up,
                Combine,
                Text,
            }

            public enum MouseType
            {
                LeftClick,
                RightClick,
                LeftDBClick,
                RightDBClick,
                LeftDown,
                LeftUp,
                RightDown,
                RightUp,
                WheelUp,
                WheelDown,
                MoveTo,
                MoveBy,
            }

            public enum InstructionType
            {
                Send,
                Delay,
                LoopStart,
                LoopEnd,
                KeyBoard,
                Mouse,
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
                    Guid RID = Guid.NewGuid();
                    int RNum = Socket_Cache.RobotList.lstRobot.Count + 1;
                    string RName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_27), RNum.ToString());

                    AddRobot(RID, RName, Socket_Cache.Robot.InitInstructions());
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void AddRobot(Guid RID, string RName, DataTable RInstructions)
            {
                try
                {
                    if (RID != Guid.Empty && !string.IsNullOrEmpty(RName))
                    {
                        Socket_RobotInfo sri = new Socket_RobotInfo(RID, RName, RInstructions);
                        Socket_Cache.RobotList.lstRobot.Add(sri);
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
                    Guid RID_New = Guid.NewGuid();
                    string RName_Copy = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_62), Socket_Cache.RobotList.lstRobot[RIndex].RName);
                    DataTable RInstruction_Copy = Socket_Cache.RobotList.lstRobot[RIndex].RInstruction.Copy();

                    Socket_Cache.Robot.AddRobot(RID_New, RName_Copy, RInstruction_Copy);
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

            #region//获取指令类型的名称

            public static string GetName_ByInstructionType(Socket_Cache.Robot.InstructionType instructionType)
            {
                string sReturn = string.Empty;

                try
                {
                    switch (instructionType)
                    {
                        case Socket_Cache.Robot.InstructionType.Send:
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
                        case Socket_Cache.Robot.InstructionType.Send:
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
                        case Socket_Cache.Robot.InstructionType.Send:

                            if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                            {
                                string SendPacket_Index = sContent.Split('|')[0];                             
                                string SendPacket_Socket = sContent.Split('|')[1];                            

                                sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_113), SendPacket_Index);

                                if (!string.IsNullOrEmpty(SendPacket_Socket) && !SendPacket_Socket.Equals("0"))
                                {
                                    sReturn += string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_114), SendPacket_Socket);
                                }                           
                            }

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
                Socket_Cache.Robot.KeyBoardType kbType = new KeyBoardType();

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

            #region//指令集的列表操作

            public static int UpdateInstruction_ByListAction(Socket_Cache.ListAction listAction, int RobotIndex, int InstructionIndex)
            {
                int iReturn = -1;

                try
                {
                    if (RobotIndex > -1 && RobotIndex < Socket_Cache.RobotList.lstRobot.Count)
                    {
                        DataTable dtRInstruction = Socket_Cache.RobotList.lstRobot[RobotIndex].RInstruction;
                        int iInstructionCount = dtRInstruction.Rows.Count;

                        DataRow dr = dtRInstruction.NewRow();
                        dr.ItemArray = dtRInstruction.Rows[InstructionIndex].ItemArray;

                        switch (listAction)
                        {
                            case Socket_Cache.ListAction.Top:

                                if (InstructionIndex > 0)
                                {
                                    dtRInstruction.Rows.RemoveAt(InstructionIndex);
                                    dtRInstruction.Rows.InsertAt(dr, 0);
                                    iReturn = 0;
                                }

                                break;

                            case Socket_Cache.ListAction.Up:

                                if (InstructionIndex > 0)
                                {  
                                    dtRInstruction.Rows.RemoveAt(InstructionIndex);
                                    dtRInstruction.Rows.InsertAt(dr, InstructionIndex - 1);
                                    iReturn = InstructionIndex - 1;
                                }

                                break;

                            case Socket_Cache.ListAction.Down:

                                if (InstructionIndex < iInstructionCount - 1)
                                {
                                    dtRInstruction.Rows.RemoveAt(InstructionIndex);
                                    dtRInstruction.Rows.InsertAt(dr, InstructionIndex + 1);
                                    iReturn = InstructionIndex + 1;
                                }

                                break;

                            case Socket_Cache.ListAction.Bottom:

                                if (InstructionIndex < iInstructionCount - 1)
                                {
                                    dtRInstruction.Rows.RemoveAt(InstructionIndex);
                                    dtRInstruction.Rows.Add(dr);
                                    iReturn = dtRInstruction.Rows.Count - 1;
                                }

                                break;

                            case Socket_Cache.ListAction.Delete:

                                dtRInstruction.Rows.RemoveAt(InstructionIndex);
                                iReturn = dtRInstruction.Rows.Count - 1;

                                break;
                        }
                    }                        
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
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
                        List<int> listSend = new List<int>();                 
                        List<int> listLoopStart = new List<int>();
                        List<int> listLoopEnd = new List<int>();

                        for (int i = 0; i < dtRInstruction.Rows.Count; i++)
                        {
                            Socket_Cache.Robot.InstructionType instructionType = (Socket_Cache.Robot.InstructionType)dtRInstruction.Rows[i]["Type"];

                            switch (instructionType)
                            {
                                case Socket_Cache.Robot.InstructionType.Send:
                                    listSend.Add(i);
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

                        foreach (int iSendIndex in listSend)
                        { 
                            string sSendContent = dtRInstruction.Rows[iSendIndex]["Content"].ToString();

                            if (!string.IsNullOrEmpty(sSendContent) && sSendContent.IndexOf("|") > 0)
                            {
                                int iSendPacketID = int.Parse(sSendContent.Split('|')[0].ToString());

                                if (iSendPacketID < 1 || iSendPacketID > Socket_Cache.SendList.dtSendList.Rows.Count)
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

            public static void DoRobot_ByIndex(int RobotListIndex)
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

            public static void DoRobot(Guid RID)
            {
                try
                {
                    if (RID != Guid.Empty)
                    {
                        Socket_RobotInfo sri = Socket_Cache.RobotList.lstRobot.Where(item => item.RID == RID).FirstOrDefault();

                        if (sri != null) 
                        {
                            if (sri.RInstruction.Rows.Count > 0)
                            {
                                Socket_Robot sr = new Socket_Robot();                                
                                sr.StartRobot(sri.RName, sri.RInstruction);                             
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
        }

        #endregion

        #region//机器人列表

        public static class RobotList
        {
            public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\RobotList.rp";
            public static string AESKey = string.Empty;
            public static BindingList<Socket_RobotInfo> lstRobot = new BindingList<Socket_RobotInfo>();

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
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.Import);
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

                        Socket_Cache.Robot.AddRobot(RID, RName, RInstruction);
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
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.Export);
                            pwForm.ShowDialog();

                            string FilePath = sfdSaveFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                SaveRobotList(FilePath, RobotIndex, true);
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
                            string sRID = Socket_Cache.RobotList.lstRobot[i].RID.ToString().ToUpper();
                            string sRName = Socket_Cache.RobotList.lstRobot[i].RName;
                            DataTable dtRInstruction = Socket_Cache.RobotList.lstRobot[i].RInstruction;

                            XElement xeRobot =
                                new XElement("Robot",
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

        #region//发送列表

        public static class SendList
        {
            public static int Loop_CNT = 0;
            public static int Loop_Int = 0;
            public static int Loop_Send_CNT = 0;
            public static int SendList_Success_CNT = 0;
            public static int SendList_Fail_CNT = 0;
            public static int UseSocket = 0;
            public static bool bShow_SendListForm = true;
            public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\SendList.sp";
            public static string AESKey = string.Empty;

            public static DataTable dtSendList = new DataTable();

            #region//初始化发送列表

            private static void InitSendList()
            {
                try
                {
                    if (dtSendList.Columns.Count == 0)
                    {
                        dtSendList.Columns.Add("Remark", typeof(string));
                        dtSendList.Columns.Add("Socket", typeof(int));
                        dtSendList.Columns.Add("ToAddress", typeof(string));
                        dtSendList.Columns.Add("Len", typeof(int));
                        dtSendList.Columns.Add("Data", typeof(string));
                        dtSendList.Columns.Add("Bytes", typeof(byte[]));
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion            

            #region//发送

            public static bool DoSendList_ByIndex(int iSocket, int iIndex)
            {
                bool bResult = false;

                try
                {
                    if (iIndex > -1 && iIndex < dtSendList.Rows.Count)
                    {
                        byte[] bBuffer = (byte[])dtSendList.Rows[iIndex]["Bytes"];

                        if (bBuffer.Length > 0)
                        {
                            IntPtr ipSend = Marshal.AllocHGlobal(bBuffer.Length);
                            Marshal.Copy(bBuffer, 0, ipSend, bBuffer.Length);

                            if (iSocket == 0)
                            {
                                iSocket = (int)dtSendList.Rows[iIndex]["Socket"];
                            }

                            bResult = WinSockHook.SendPacket(iSocket, ipSend, bBuffer.Length);                          
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bResult;
            }

            #endregion

            #region//添加

            public static void AddToSendList_BytIndex(int iSIndex)
            {
                try
                {
                    if (iSIndex > -1 && iSIndex < SocketList.lstRecPacket.Count)
                    {
                        string sRemark = string.Empty;
                        int iSocket = SocketList.lstRecPacket[iSIndex].PacketSocket;
                        string sToAddress = SocketList.lstRecPacket[iSIndex].PacketTo;
                        byte[] bBuffer = SocketList.lstRecPacket[iSIndex].PacketBuffer;
                        string sData = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);

                        AddToSendList(sRemark, iSocket, sToAddress, sData,bBuffer);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void AddToSendList(string sRemark, int iSocket, string sToAddress, string sData, byte[] bBuffer)
            {
                try
                {
                    Socket_Cache.SendList.InitSendList();

                    DataRow dr = dtSendList.NewRow();
             
                    dr["Remark"] = sRemark;
                    dr["Socket"] = iSocket;
                    dr["ToAddress"] = sToAddress;
                    dr["Len"] = bBuffer.Length;
                    dr["Data"] = sData;
                    dr["Bytes"] = bBuffer;

                    dtSendList.Rows.Add(dr);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//删除

            public static void DeleteFromSendList_ByIndex(int iSIndex)
            {
                try
                {
                    if (iSIndex > -1 && iSIndex < dtSendList.Rows.Count)
                    {
                        dtSendList.Rows[iSIndex].Delete();
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

            private static void SendListClear()
            {
                dtSendList.Rows.Clear();
            }

            #endregion

            #region//保存发送列表（对话框）

            public static void SaveSendList_Dialog(string FileName, int SendPacketIndex)
            {
                try
                {
                    if (Socket_Cache.SendList.dtSendList.Rows.Count > 0)
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
                            Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.Export);
                            pwForm.ShowDialog();

                            string FilePath = sfdSaveFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                SaveSendList(FilePath, SendPacketIndex, true);
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

            private static void SaveSendList_ToXDocument(string FilePath, int SendPacketIndex)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeRoot = new XElement("SendList");
                    xdoc.Add(xeRoot);

                    if (Socket_Cache.SendList.dtSendList.Rows.Count > 0)
                    {
                        int Start = 0;
                        int End = Socket_Cache.SendList.dtSendList.Rows.Count;

                        if (SendPacketIndex > -1 && SendPacketIndex < End)
                        {
                            Start = SendPacketIndex;
                            End = SendPacketIndex + 1;
                        }

                        for (int i = Start; i < End; i++)
                        {  
                            string sRemark = Socket_Cache.SendList.dtSendList.Rows[i]["Remark"].ToString().Trim();
                            string sSocket = Socket_Cache.SendList.dtSendList.Rows[i]["Socket"].ToString().Trim();
                            string sToAddress = Socket_Cache.SendList.dtSendList.Rows[i]["ToAddress"].ToString().Trim();                            
                            byte[] bBuffer = (byte[])Socket_Cache.SendList.dtSendList.Rows[i]["Bytes"];
                            string sData = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);

                            XElement xeRobot =
                                new XElement("Send",
                                new XElement("Remark", sRemark),
                                new XElement("Socket", sSocket),
                                new XElement("ToAddress", sToAddress),                                
                                new XElement("Data", sData)
                                );                          

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
                                Socket_PasswordFrom pwForm = new Socket_PasswordFrom(Socket_Cache.PWType.Import);
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
                        string sSRemark = string.Empty;
                        if (xeSend.Element("Remark") != null)
                        {
                            sSRemark = xeSend.Element("Remark").Value;
                        }

                        int iSSocket = -1;
                        if (xeSend.Element("Socket") != null)
                        {
                            iSSocket = int.Parse(xeSend.Element("Socket").Value);
                        }

                        string sSToAddress = string.Empty;
                        if (xeSend.Element("ToAddress") != null)
                        {
                            sSToAddress = xeSend.Element("ToAddress").Value;
                        }                     

                        string sSData = string.Empty;
                        if (xeSend.Element("Data") != null)
                        {
                            sSData = xeSend.Element("Data").Value;
                        }

                        byte[] bBuffer = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sSData);
                        Socket_Cache.SendList.AddToSendList(sSRemark, iSSocket, sSToAddress, sSData, bBuffer);
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
