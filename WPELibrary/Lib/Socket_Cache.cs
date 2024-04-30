using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;

namespace WPELibrary.Lib
{
    public static class Socket_Cache
    {        
        public static bool Interecept_Recv, Interecept_RecvFrom, Interecept_Send, Interecept_SendTo;
        public static bool Display_Recv, Display_RecvFrom, Display_Send, Display_SendTo;

        #region//封包队列
        public static class SocketQueue
        {
            public static int Interecept_CNT = 0;
            public static int Recv_CNT = 0;
            public static int Send_CNT = 0;

            public static Queue<Socket_Packet> qSocket_Packet = new Queue<Socket_Packet>();

            #region//封包入队列（多线程）
            public static void SocketToQueue(int iSocket, IntPtr ipBuff, int iLen, Socket_Packet.SocketType sType, Socket_Packet.sockaddr sAddr, int iResLen)
            {
                try
                {
                    byte[] bBuffer = new byte[iResLen];
                    Marshal.Copy(ipBuff, bBuffer, 0, iResLen);

                    Socket_Packet sp = new Socket_Packet(iSocket, ipBuff, iLen, sType, sAddr, bBuffer, iResLen);

                    Thread tSocket_Queue = new Thread(new ParameterizedThreadStart(SocketToQueue_Thread));
                    tSocket_Queue.Start(sp);
                }
                catch (Exception ex) 
                {
                    Socket_Operation.DoLog(ex.Message);
                }
            }

            private static void SocketToQueue_Thread(object ob)
            {
                try
                {
                    Socket_Packet sp = (Socket_Packet)ob;

                    lock (qSocket_Packet)
                    {
                        qSocket_Packet.Enqueue(sp);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(ex.Message);
                }              
            }

            #endregion

            #region//清除队列数据
            public static void ResetSocketQueue()
            {
                try
                {
                    Interecept_CNT = 0;
                    Recv_CNT = 0;
                    Send_CNT = 0;

                    qSocket_Packet.Clear();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(ex.Message);
                }
            }
            #endregion
        }
        #endregion

        #region//封包列表
        public static class SocketList
        {
            public static BindingList<Socket_Packet_Info> lstRecPacket = new BindingList<Socket_Packet_Info>();

            public delegate void SocketPacketReceived(Socket_Packet_Info si);
            public static event SocketPacketReceived RecSocketPacket;

            #region//封包入列表
            public static void SocketToList(int iMax_DataLen)
            {
                try
                {
                    if (SocketQueue.qSocket_Packet.Count > 0)
                    {
                        Socket_Packet sa = SocketQueue.qSocket_Packet.Dequeue();

                        bool bCheck = Socket_Operation.ISShow_SocketInfo(sa);

                        if (bCheck)
                        {
                            int iIndex = lstRecPacket.Count + 1;
                            Socket_Packet.SocketType sType = sa.Type;
                            int iSocket = sa.Socket;
                            int iResLen = sa.ResLen;
                            byte[] bBuffer = sa.Buffer;

                            string sData = "";

                            if (iResLen > iMax_DataLen)
                            {
                                byte[] bTemp = new byte[iMax_DataLen];

                                for (int j = 0; j < iMax_DataLen; j++)
                                {
                                    bTemp[j] = bBuffer[j];
                                }

                                sData = Socket_Operation.Byte_To_Hex(bTemp) + " ...";
                            }
                            else
                            {
                                sData = Socket_Operation.Byte_To_Hex(bBuffer);
                            }

                            Socket_Packet.sockaddr sAddr = sa.Addr;

                            string sIP_From = "", sIP_To = "";

                            switch (sType)
                            {
                                case Socket_Packet.SocketType.Recv:

                                    sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.To);
                                    sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);

                                    break;
                                case Socket_Packet.SocketType.WSARecv:

                                    sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.To);
                                    sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);

                                    break;
                                case Socket_Packet.SocketType.Send:

                                    sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                    sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.To);

                                    break;
                                case Socket_Packet.SocketType.WSASend:

                                    sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                    sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.To);

                                    break;
                                case Socket_Packet.SocketType.SendTo:

                                    sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                    sIP_To = Socket_Operation.GetSocketIP(sAddr.sin_addr, sAddr.sin_port);

                                    break;
                                case Socket_Packet.SocketType.RecvFrom:

                                    sIP_From = Socket_Operation.GetSocketIP(sAddr.sin_addr, sAddr.sin_port);
                                    sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);

                                    break;
                            }                         

                            Socket_Packet_Info spi = new Socket_Packet_Info(iIndex, sType, iSocket, sIP_From, sIP_To, iResLen, sData, bBuffer);
                            RecSocketPacket?.Invoke(spi);
                        }
                        else
                        {
                            Socket_Operation.CheckCNT++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(ex.Message);
                }
            }
            #endregion
        }
        #endregion

        #region//日志队列
        public static class LogQueue
        {
            public static Queue<Socket_Log_Info> qSocket_Log = new Queue<Socket_Log_Info>();

            #region//日志入队列（多线程）
            public static void LogToQueue(string sLogContent)
            {
                try
                {
                    Socket_Log_Info sli = new Socket_Log_Info(sLogContent);

                    Thread tLog_Queue = new Thread(new ParameterizedThreadStart(LogToQueue_Thread));
                    tLog_Queue.Start(sli);
                }
                catch (Exception ex) 
                {
                    Socket_Operation.DoLog(ex.Message);
                }
            }

            private static void LogToQueue_Thread(object ob)
            {
                try
                {
                    Socket_Log_Info sli = (Socket_Log_Info)ob;

                    lock (qSocket_Log)
                    {
                        qSocket_Log.Enqueue(sli);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(ex.Message);
                }             
            }

            #endregion

            #region//清除队列数据
            public static void ResetLogQueue()
            {
                try
                {
                    qSocket_Log.Clear();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(ex.Message);
                }
            }
            #endregion
        }
        #endregion

        #region//日志列表
        public static class LogList
        {
            public static BindingList<Socket_Log_Info> lstRecLog = new BindingList<Socket_Log_Info>();

            public delegate void SocketLogReceived(Socket_Log_Info sl);
            public static event SocketLogReceived RecSocketLog;

            #region//日志入列表
            public static void LogToList()
            {
                try
                {
                    if (LogQueue.qSocket_Log.Count > 0)
                    {
                        Socket_Log_Info sli = LogQueue.qSocket_Log.Dequeue();
                        RecSocketLog?.Invoke(sli);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(ex.Message);
                }
            }
            #endregion
        }
        #endregion

        #region//滤镜列表
        public static class SocketFilterList
        {
            public static int SearchRowIndex = 0;
            public static int ModifyRowIndex = 1;
            public static int FilterLen_MAX = 500;
            public static BindingList<Socket_Filter_Info> lstFilter = new BindingList<Socket_Filter_Info>();

            #region//初始化滤镜列表
            public static void InitFilterList(int iFilterMaxNum)
            {
                try
                {
                    lstFilter.Clear();

                    for (int i = 0; i < iFilterMaxNum; i++)
                    {
                        int iFNum = i + 1;
                        string sName = MultiLanguage.GetDefaultLanguage("滤镜", "Filter") + " " + iFNum.ToString();                     

                        FilterToList(iFNum, false, sName, "", "");                        
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(ex.Message);
                }
            }
            #endregion

            #region//滤镜入列表
            public static void FilterToList(int iFNum, bool bCheck, string sName, string sSearch, string sModify)
            {
                Socket_Filter_Info sf = new Socket_Filter_Info(iFNum, bCheck, sName, sSearch, sModify);

                lstFilter.Add(sf);
            }
            #endregion

            #region//清空滤镜列表
            public static void FilterListClear()
            {
                lstFilter.Clear();
            }
            #endregion

            #region//设置滤镜是否启用
            public static void SetIsCheck_ByFilterNum(int FNum, bool bCheck)
            {
                if (FNum > 0)
                {
                    int iFIndex = GetFilterIndex_ByFilterNum(FNum);

                    lstFilter[iFIndex].IsCheck = bCheck;
                }
            }
            #endregion

            #region//获取滤镜是否启用
            public static bool GetIsCheck_ByFilterNum(int FNum)
            {
                bool bReturn = false;

                if (FNum > 0)
                {
                    int iFIndex = GetFilterIndex_ByFilterNum(FNum);

                    bReturn = lstFilter[iFIndex].IsCheck;
                }

                return bReturn;                
            }
            #endregion

            #region//滤镜列表操作（新增，修改，删除）
            public static void AddFilter_New()
            {
                int FNum = GetFilterNum_New();
                string FName = MultiLanguage.GetDefaultLanguage("滤镜", "Filter") + " " + FNum.ToString();

                AddFilter_New(FNum, false, FName, "", "");
            }

            public static void AddFilter_New(int FNum, bool bCheck, string FName, string FSearch, string FModify)
            {
                Socket_Filter_Info sc = new Socket_Filter_Info(FNum, bCheck,FName, FSearch, FModify);
                lstFilter.Add(sc);
            }

            private static int GetFilterNum_New()
            {
                int iReturn = 0;

                for (int i = 0; i < lstFilter.Count; i++)
                {
                    int iFNum = lstFilter[i].FNum;

                    if (iFNum > iReturn)
                    {
                        iReturn = iFNum;
                    }
                }

                return iReturn + 1;
            }

            public static void DeleteFilter_ByFilterNum(int FNum)
            {
                if (FNum > 0)
                {
                    int iFIndex = GetFilterIndex_ByFilterNum(FNum);

                    if (iFIndex > -1)
                    {
                        lstFilter.RemoveAt(iFIndex);
                    }
                }
            }

            public static void UpdateFilter_ByFilterNum(int FNum, string FName, string FSearch, string FModify)
            {
                if (FNum > 0 && !string.IsNullOrEmpty(FName))
                {
                    int iFIndex = GetFilterIndex_ByFilterNum(FNum);

                    if (iFIndex > -1)
                    {
                        lstFilter[iFIndex].FName = FName;
                        lstFilter[iFIndex].FSearch = FSearch;
                        lstFilter[iFIndex].FModify = FModify;
                    }
                }
            }

            public static int GetFilterIndex_ByFilterNum(int FNum)
            {
                int iReturn = -1;

                for (int i = 0; i < lstFilter.Count; i++)
                {
                    int iFNum = lstFilter[i].FNum;

                    if (iFNum == FNum)
                    {
                        iReturn = i;
                        break;
                    }
                }

                return iReturn;
            }

            public static int GetFilterNum_ByFilterIndex(int FIndex)
            {
                int iReturn = 0;

                int iFNum = lstFilter[FIndex].FNum;

                if (iFNum > 0)
                {
                    iReturn = iFNum;
                }

                return iReturn;
            }
            #endregion

            #region//执行滤镜
            public static void DoFilter(IntPtr ipBuff, int iLen)
            {
                try
                {
                    byte[] bBuff = Socket_Operation.GetByteFromIntPtr(ipBuff, iLen);

                    foreach (Socket_Filter_Info sfi in lstFilter)
                    {
                        bool bCheck = sfi.IsCheck;

                        if (bCheck) 
                        {
                            int iFNum = sfi.FNum;
                            string sFName = sfi.FName;
                            string sFSearch = sfi.FSearch;
                            string sModify = sfi.FModify;

                            if (CheckFilterSearch_ByBuff(sFSearch, bBuff))
                            {
                                if (!string.IsNullOrEmpty(sModify))
                                {
                                    string[] ssModify = sModify.Split(',');

                                    foreach (string sTemp in ssModify)
                                    {
                                        string[] sModifyValue = sTemp.Split('|');
                                        int iIndex = int.Parse(sModifyValue[0].ToString().Trim());
                                        string sValue = sModifyValue[1].ToString().Trim();                                        

                                        bBuff[iIndex] = Socket_Operation.Hex_To_Byte(sValue)[0];
                                    }

                                    bool bSetOK = Socket_Operation.SetByteToIntPtr(bBuff, ipBuff, iLen);

                                    if (bSetOK)
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
                    Socket_Operation.DoLog(ex.Message);
                }
            }
            #endregion

            #region//检查是否匹配滤镜
            private static bool CheckFilterSearch_ByBuff(string FSearch, byte[] bBuff)
            {
                bool bResult = true;                

                if (!string.IsNullOrEmpty(FSearch))
                {
                    string[] ssSearch = FSearch.Split(',');

                    foreach (string sSearch in ssSearch)
                    {
                        string[] sSearchValue = sSearch.Split('|');
                        int iIndex = int.Parse(sSearchValue[0].ToString().Trim());
                        string sValue = sSearchValue[1].ToString().Trim();

                        string sBufferValue = bBuff[iIndex].ToString("X2");

                        if (!sValue.Equals(sBufferValue))
                        {                            
                            bResult = false;
                            break;
                        }
                    }
                }
                else
                {                    
                    bResult = false;
                }

                return bResult;
            }
            #endregion
        }
        #endregion

        #region//封包发送列表
        public static class SocketSendList
        {
            public static int Loop_CNT = 0;//循环次数
            public static int Loop_Int = 0;//循环间隔
            public static int Loop_Send_CNT = 0;//已循环次数
            public static int SendList_Success_CNT = 0;//发送成功
            public static int SendList_Fail_CNT = 0;//发送失败
            public static int UseSocket = 0;//使用此套接字
            public static bool bShow_SendListForm = true;

            public static DataTable dtSocketSendList = new DataTable();

            #region//初始化发送列表
            public static void InitSendList()
            {
                dtSocketSendList.Columns.Add("ID", typeof(int));
                dtSocketSendList.Columns.Add("Remark", typeof(string));
                dtSocketSendList.Columns.Add("Socket", typeof(int));
                dtSocketSendList.Columns.Add("ToAddress", typeof(string));
                dtSocketSendList.Columns.Add("Len", typeof(int));
                dtSocketSendList.Columns.Add("Data", typeof(string));
                dtSocketSendList.Columns.Add("Bytes", typeof(byte[]));
            }
            #endregion            

            #region//发送列表操作（新增，删除）
            public static void AddSendList_BySocketListIndex(int iSLIndex)
            {
                AddSendList_New(
                    SocketList.lstRecPacket[iSLIndex].Index,
                    "",
                    SocketList.lstRecPacket[iSLIndex].Socket,
                    SocketList.lstRecPacket[iSLIndex].To,
                    SocketList.lstRecPacket[iSLIndex].ResLen,
                    SocketList.lstRecPacket[iSLIndex].Data,
                    SocketList.lstRecPacket[iSLIndex].Buffer
                    );
            }

            public static void AddSendList_New(int iIndex, string sNote, int iSocket, string sIPTo, int iResLen, string sData, byte[] bBuffer)
            {
                DataRow dr = dtSocketSendList.NewRow();

                dr["ID"] = iIndex;
                dr["Remark"] = sNote;
                dr["Socket"] = iSocket;
                dr["ToAddress"] = sIPTo;
                dr["Len"] = iResLen;
                dr["Data"] = sData;
                dr["Bytes"] = bBuffer;
                dtSocketSendList.Rows.Add(dr);
            }

            public static void DeleteSendList_ByIndex(int SIndex)
            {
                dtSocketSendList.Rows[SIndex].Delete();
            }
            #endregion

            #region//清空发送列表
            public static void SendListClear()
            {
                dtSocketSendList.Rows.Clear();
            }
            #endregion

            #region//发送列表
            public static bool SendPacketList_ByIndex(int iSocket, int iIndex)
            {
                bool bResult = false;

                try
                {
                    int iLen = (int)dtSocketSendList.Rows[iIndex]["Len"];
                    byte[] bBuffer = (byte[])dtSocketSendList.Rows[iIndex]["Bytes"];

                    if (bBuffer.Length > 0)
                    {
                        IntPtr ipSend = Marshal.AllocHGlobal(bBuffer.Length);
                        Marshal.Copy(bBuffer, 0, ipSend, bBuffer.Length);

                        bool bReturn = WinSockHook.SendPacket(iSocket, ipSend, bBuffer.Length);

                        if (bReturn)
                        {
                            SendList_Success_CNT++;
                        }
                        else
                        {
                            SendList_Fail_CNT++;
                        }

                        Thread.Sleep(Loop_Int);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(ex.Message);
                }

                return bResult;
            }
            #endregion
        }
        #endregion
    }
}
