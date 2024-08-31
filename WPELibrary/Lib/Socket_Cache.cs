using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;
using System.Reflection;

namespace WPELibrary.Lib
{
    public static class Socket_Cache
    {
        public static byte[] bByteBuff = new byte[0];
        public static bool Hook_Send, Hook_SendTo, Hook_Recv, Hook_RecvFrom, Hook_WSASend, Hook_WSASendTo, Hook_WSARecv, Hook_WSARecvFrom;
        public static bool Check_Size, Check_Socket, Check_IP, Check_Packet;
        public static string txtCheck_Socket, txtCheck_IP, txtCheck_Packet;
        public static decimal txtCheck_Size_From, txtCheck_Size_To;

        #region//封包队列
        public static class SocketQueue
        {  
            public static int Filter_CNT = 0;
            public static int Recv_CNT = 0;
            public static int Send_CNT = 0;

            public static Queue<Socket_Packet> qSocket_Packet = new Queue<Socket_Packet>();

            #region//封包入队列

            public static void SocketPacketToQueue(int iSocket, byte[] bBuffByte, Socket_Packet.SocketType sType, Socket_Packet.sockaddr sAddr)
            {
                try
                {
                    Socket_Packet sp = new Socket_Packet(iSocket, IntPtr.Zero, bBuffByte.Length, sType, sAddr, bBuffByte, bBuffByte.Length);

                    if (Socket_Operation.ISShowSocketPacket_ByFilter(sp))
                    {
                        lock (qSocket_Packet)
                        {
                            qSocket_Packet.Enqueue(sp);
                        }
                    }
                    else
                    {
                        Filter_CNT++;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void SocketPacketToQueue(int iSocket, IntPtr ipBuff, int iLen, Socket_Packet.SocketType sType, Socket_Packet.sockaddr sAddr, int iResLen)
            {
                try
                {
                    byte[] bBuffer = new byte[iResLen];
                    Marshal.Copy(ipBuff, bBuffer, 0, iResLen);

                    Socket_Packet sp = new Socket_Packet(iSocket, ipBuff, iLen, sType, sAddr, bBuffer, iResLen);

                    if (Socket_Operation.ISShowSocketPacket_ByFilter(sp))
                    {
                        lock (qSocket_Packet)
                        {
                            qSocket_Packet.Enqueue(sp);
                        }
                    }
                    else
                    {
                        Filter_CNT++;
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
                    Filter_CNT = 0;
                    Recv_CNT = 0;
                    Send_CNT = 0;

                    qSocket_Packet.Clear();
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

                            sData = Socket_Operation.ByteToString("HEX", bTemp) + " ...";
                        }
                        else
                        {
                            sData = Socket_Operation.ByteToString("HEX", bBuffer);
                        }

                        Socket_Packet.sockaddr sAddr = sa.Addr;

                        string sIP_From = "", sIP_To = "";

                        switch (sType)
                        {  
                            case Socket_Packet.SocketType.Send:
                                SocketQueue.Send_CNT++;
                                sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.To);
                                break;
                            case Socket_Packet.SocketType.SendTo:
                                SocketQueue.Send_CNT++;
                                sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                sIP_To = Socket_Operation.GetSocketIP(sAddr.sin_addr, sAddr.sin_port);
                                break;
                            case Socket_Packet.SocketType.Recv:
                                SocketQueue.Recv_CNT++;
                                sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.To);
                                sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                break;
                            case Socket_Packet.SocketType.RecvFrom:
                                SocketQueue.Recv_CNT++;
                                sIP_From = Socket_Operation.GetSocketIP(sAddr.sin_addr, sAddr.sin_port);
                                sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                break;
                            case Socket_Packet.SocketType.WSASend:
                                SocketQueue.Send_CNT++;
                                sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.To);
                                break;
                            case Socket_Packet.SocketType.WSASendTo:
                                SocketQueue.Send_CNT++;
                                sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                sIP_To = Socket_Operation.GetSocketIP(sAddr.sin_addr, sAddr.sin_port);
                                break;
                            case Socket_Packet.SocketType.WSARecv:
                                SocketQueue.Recv_CNT++;
                                sIP_From = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.To);
                                sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                break;
                            case Socket_Packet.SocketType.WSARecvFrom:
                                SocketQueue.Recv_CNT++;
                                sIP_From = Socket_Operation.GetSocketIP(sAddr.sin_addr, sAddr.sin_port);
                                sIP_To = Socket_Operation.GetSocketIP(iSocket, Socket_Packet.IPType.From);
                                break;
                        }                     

                        Socket_Packet_Info spi = new Socket_Packet_Info(iIndex, sType, iSocket, sIP_From, sIP_To, iResLen, sData, bBuffer);
                        RecSocketPacket?.Invoke(spi);
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
            public static Queue<Socket_Log_Info> qSocket_Log = new Queue<Socket_Log_Info>();

            #region//日志入队列

            public static void LogToQueue(string sFuncName, string sLogContent)
            {
                try
                {
                    Socket_Log_Info sli = new Socket_Log_Info(sFuncName, sLogContent);

                    lock (qSocket_Log)
                    {
                        qSocket_Log.Enqueue(sli);
                    }                 
                }
                catch (Exception ex) 
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }
            #endregion
        }
        #endregion

        #region//滤镜列表
        public static class SocketFilterList
        {
            public static int FilterNormal_SearchRowIndex = 0, FilterNormal_ModifyRowIndex = 1;
            public static int FilterAdvanced_SearchRowIndex = 0, FilterAdvanced_ModifyRowIndex = 0;
            public static int FilterSearchLen_New = 100, FilterModifyLen_New = 100;
            public static BindingList<Socket_Filter_Info> lstFilter = new BindingList<Socket_Filter_Info>();

            #region//初始化滤镜列表
            public static void InitFilterList(int iFilterMaxNum)
            {
                try
                {
                    lstFilter.Clear();

                    for (int i = 0; i < iFilterMaxNum; i++)
                    {                        
                        AddFilter_New();
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }
            #endregion

            #region//清空滤镜列表

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

            #region//设置滤镜是否启用

            public static void SetIsCheck_ByFilterNum(int FNum, bool bCheck)
            {
                try
                {
                    if (FNum > 0)
                    {
                        int iFIndex = GetFilterIndex_ByFilterNum(FNum);

                        lstFilter[iFIndex].IsCheck = bCheck;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion

            #region//获取滤镜是否启用

            public static bool GetIsCheck_ByFilterNum(int FNum)
            {
                bool bReturn = false;

                try
                {
                    if (FNum > 0)
                    {
                        int iFIndex = GetFilterIndex_ByFilterNum(FNum);

                        bReturn = lstFilter[iFIndex].IsCheck;
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;                
            }

            #endregion

            #region//返回滤镜长度

            public static int GetFilterLen_ByFilterNum(int FNum)
            {
                int iReturn = 0;

                try
                {
                    if (FNum > 0)
                    {
                        int iFIndex = GetFilterIndex_ByFilterNum(FNum);

                        string sFSearch = lstFilter[iFIndex].FSearch;
                        string sFModify = lstFilter[iFIndex].FModify;


                        int iFSearch = 0, iFModify = 0;

                        if (string.IsNullOrEmpty(sFSearch) && string.IsNullOrEmpty(sFModify))
                        {
                            iReturn = FilterSearchLen_New;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(sFSearch))
                            {
                                string[] slFSearch = sFSearch.Split(',');
                                string[] slTemp = slFSearch[slFSearch.Length - 1].ToString().Split('-');
                                iFSearch = int.Parse(slTemp[0].ToString());
                            }

                            if (!string.IsNullOrEmpty(sFModify))
                            {
                                string[] slFModify = sFModify.Split(',');
                                string[] slTemp = slFModify[slFModify.Length - 1].ToString().Split('-');
                                iFModify = int.Parse(slTemp[0].ToString());
                            }

                            if (iFSearch >= iFModify)
                            {
                                iReturn = iFSearch + 1;
                            }
                            else
                            {
                                iReturn = iFModify + 1;
                            }
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

            #region//滤镜列表操作（新增，修改，删除）

            //新增滤镜
            public static void AddFilter_New()
            {
                try
                {
                    AddFilter_New("", Socket_Filter_Info.FilterMode.Normal, Socket_Filter_Info.StartFrom.Head, 1, "", FilterSearchLen_New, "", FilterModifyLen_New, false);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }
            
            public static void AddFilter_New(string FName, Socket_Filter_Info.FilterMode FMode, Socket_Filter_Info.StartFrom FStartFrom, int FModifyCNT, string FSearch, int FSearchLen, string FModify, int FModifyLen, bool bCheck)
            {
                try
                {
                    int FNum = GetFilterNum_New();

                    if (string.IsNullOrEmpty(FName))
                    {
                        FName = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_50) + " " + FNum.ToString();
                    }

                    if (string.IsNullOrEmpty(FSearch))
                    {
                        FSearchLen = FilterSearchLen_New;
                    }

                    if (string.IsNullOrEmpty(FModify))
                    {
                        FModifyLen = FilterModifyLen_New;
                    }

                    Socket_Filter_Info sc = new Socket_Filter_Info(FNum, bCheck, FName, FMode, FStartFrom, FModifyCNT, FSearch, FSearchLen, FModify, FModifyLen);

                    lstFilter.Add(sc);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }
            
            private static int GetFilterNum_New()
            {
                int iReturn = 0;

                try
                {
                    for (int i = 0; i < lstFilter.Count; i++)
                    {
                        int iFNum = lstFilter[i].FNum;

                        if (iFNum > iReturn)
                        {
                            iReturn = iFNum;
                        }
                    }

                    iReturn = iReturn + 1;
                }
                catch (Exception ex)
                {
                    iReturn = 0;

                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }                

                return iReturn;
            }

            //删除滤镜
            public static void DeleteFilter_ByFilterNum(int FNum)
            {
                try
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
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            //修改滤镜
            public static void UpdateFilter_ByFilterNum(int FNum, string FName, Socket_Filter_Info.FilterMode FMode, Socket_Filter_Info.StartFrom FStartFrom, string FSearch, int FSearchLen, string FModify, int FModifyLen)
            {
                try
                {
                    if (FNum > 0 && !string.IsNullOrEmpty(FName))
                    {
                        int iFIndex = GetFilterIndex_ByFilterNum(FNum);

                        if (iFIndex > -1)
                        {
                            lstFilter[iFIndex].FName = FName;
                            lstFilter[iFIndex].FMode = FMode;                            
                            lstFilter[iFIndex].FStartFrom = FStartFrom;
                            lstFilter[iFIndex].FSearch = FSearch;
                            lstFilter[iFIndex].FSearchLen = FSearchLen;
                            lstFilter[iFIndex].FModify = FModify;
                            lstFilter[iFIndex].FModifyLen = FModifyLen;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            //获取滤镜序号
            public static int GetFilterIndex_ByFilterNum(int FNum)
            {
                int iReturn = -1;

                try
                {
                    for (int i = 0; i < lstFilter.Count; i++)
                    {
                        int iFNum = lstFilter[i].FNum;

                        if (iFNum == FNum)
                        {
                            iReturn = i;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    iReturn = -1;

                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
            }

            //获取滤镜编号
            public static int GetFilterNum_ByFilterIndex(int FIndex)
            {
                int iReturn = -1;

                try
                {
                    int iFNum = lstFilter[FIndex].FNum;

                    if (iFNum > 0)
                    {
                        iReturn = iFNum;
                    }
                }
                catch (Exception ex)
                {
                    iReturn = -1;

                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return iReturn;
            }

            #endregion

            #region//执行滤镜
            public static void DoFilter(IntPtr ipBuff, int iLen)
            {
                string sFName = "";

                try
                {
                    byte[] bBuff = Socket_Operation.GetByteFromIntPtr(ipBuff, iLen);

                    if (bBuff.Length > 0)
                    {
                        foreach (Socket_Filter_Info sfi in lstFilter)
                        {
                            bool bChecked = sfi.IsCheck;

                            if (bChecked)
                            {
                                sFName = sfi.FName;
                                Socket_Filter_Info.FilterMode Fmode = sfi.FMode;

                                switch (Fmode)
                                {
                                    case Socket_Filter_Info.FilterMode.Normal:

                                        bool bMatch = CheckFilterIsMatch_Normal(sfi, bBuff);                                        

                                        if (bMatch)
                                        {
                                            bool bOK = DoFilter_Normal(sfi, ipBuff, iLen);

                                            if (bOK)
                                            {
                                                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_51) + sFName);

                                                break;
                                            }
                                        }

                                        break;

                                    case Socket_Filter_Info.FilterMode.Advanced:

                                        DataTable dtReturn = CheckFilterIsMatch_Adcanced(sfi, bBuff);

                                        if (dtReturn.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtReturn.Rows.Count; i++) 
                                            {
                                                int iMatch = int.Parse(dtReturn.Rows[i][0].ToString());

                                                bool bOK = DoFilter_Advanced(sfi, iMatch, ipBuff, iLen);

                                                if (bOK)
                                                {
                                                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_51) + sFName);
                                                }
                                            }

                                            break;
                                        }                                     

                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_52) + sFName + " | " + ex.Message);
                }
            }

            private static bool DoFilter_Normal(Socket_Filter_Info sfi, IntPtr ipBuff, int iLen)
            { 
                bool bReturn = false;

                try
                {                    
                    string sModify = sfi.FModify;
                    string[] ssModify = sModify.Split(',');

                    byte[] bBuff = Socket_Operation.GetByteFromIntPtr(ipBuff, iLen);

                    foreach (string sTemp in ssModify)
                    {
                        string[] sModifyValue = sTemp.Split('-');
                        int iIndex = int.Parse(sModifyValue[0].ToString().Trim());
                        string sValue = sModifyValue[1].ToString().Trim();

                        if (iIndex >= 0 && iIndex < bBuff.Length)
                        {
                            bBuff[iIndex] = Socket_Operation.Hex_To_Byte(sValue)[0];
                        }
                    }

                    bReturn = Socket_Operation.SetByteToIntPtr(bBuff, ipBuff, iLen);                  
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    bReturn = false;
                }

                return bReturn;
            }

            private static bool DoFilter_Advanced(Socket_Filter_Info sfi, int iMatch, IntPtr ipBuff, int iLen)
            {
                bool bReturn = false;

                try
                {
                    byte[] bBuff = Socket_Operation.GetByteFromIntPtr(ipBuff, iLen);

                    int iModifyLen = sfi.FModifyLen;
                    string sModify = sfi.FModify;
                    string[] ssModify = sModify.Split(',');

                    Socket_Filter_Info.StartFrom FStartFrom = sfi.FStartFrom;

                    foreach (string sTemp in ssModify)
                    {
                        int iModifyIndex = -1;
                        string[] sModifyValue = sTemp.Split('-');
                        int iIndex = int.Parse(sModifyValue[0].ToString().Trim());
                        string sValue = sModifyValue[1].ToString().Trim();

                        switch (FStartFrom)
                        {
                            case Socket_Filter_Info.StartFrom.Head:

                                iModifyIndex = iIndex;

                                break;

                            case Socket_Filter_Info.StartFrom.Position:

                                iModifyIndex = iMatch + (iIndex - iModifyLen);

                                break;
                        }

                        if (iModifyIndex >= 0 && iModifyIndex < bBuff.Length)
                        {
                            bBuff[iModifyIndex] = Socket_Operation.Hex_To_Byte(sValue)[0];
                        }                        
                    }                    

                    bReturn = Socket_Operation.SetByteToIntPtr(bBuff, ipBuff, iLen);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bReturn;
            }

            #endregion

            #region//检查是否匹配滤镜

            private static bool CheckFilterIsMatch_Normal(Socket_Filter_Info sfi, byte[] bBuff)
            {
                bool bResult = true;

                try
                {                    
                    string sFSearch = sfi.FSearch;
                    string sModify = sfi.FModify;

                    if (!string.IsNullOrEmpty(sFSearch) && !string.IsNullOrEmpty(sModify))
                    {
                        string[] ssSearch = sFSearch.Split(',');

                        foreach (string sSearch in ssSearch)
                        {
                            string[] sSearchValue = sSearch.Split('-');
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
                }
                catch (Exception ex)
                {                    
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                    bResult = false;
                }

                return bResult;
            }

            private static DataTable CheckFilterIsMatch_Adcanced(Socket_Filter_Info sfi, byte[] bBuff)
            {
                DataTable dtReturn = new DataTable();
                dtReturn.Columns.Add("MatchIndex", typeof(int));

                try
                {
                    int iStartPosition = -1;
                    int iInterval = 0;
                    int iFModifyCNT = sfi.FModifyCNT;
                    string sFSearch = sfi.FSearch;
                    string sModify = sfi.FModify;

                    if (!string.IsNullOrEmpty(sFSearch) && !string.IsNullOrEmpty(sModify))
                    {
                        string[] ssSearch = sFSearch.Split(',');

                        for (int i = 0; i < bBuff.Length; i++)
                        {
                            int iEndPosition = -1;
                            for (int j = 0; j < ssSearch.Length; j++)
                            {
                                string[] ssSearchString = ssSearch[j].Split('-');
                                int iSearchString_Index = int.Parse(ssSearchString[0].ToString().Trim());
                                string sSearchString_Value = ssSearchString[1].ToString().Trim();

                                if (j > 0)
                                {
                                    string[] ssSearchString_Head = ssSearch[0].Split('-');
                                    int iSearchString_Prev_Index = int.Parse(ssSearchString_Head[0].ToString().Trim());
                                    string sSearchString_Prev_Value = ssSearchString_Head[1].ToString().Trim();

                                    iInterval = iSearchString_Index - iSearchString_Prev_Index;
                                }
                                else
                                {
                                    iInterval = 0;
                                }

                                iEndPosition = i + iInterval;

                                if (iEndPosition >= 0 && iEndPosition < bBuff.Length)
                                {
                                    string sBufferValue = bBuff[iEndPosition].ToString("X2");

                                    if (sBufferValue.Equals(sSearchString_Value))
                                    {
                                        iStartPosition = i;
                                    }
                                    else
                                    {
                                        iStartPosition = -1;
                                        break;
                                    }
                                }
                                else
                                {
                                    iStartPosition = -1;
                                    break;
                                }
                            }                            

                            if (iStartPosition > -1)
                            {
                                if (iFModifyCNT > 0)
                                {
                                    DataRow dr = dtReturn.NewRow();
                                    dr[0] = iStartPosition;
                                    dtReturn.Rows.Add(dr);

                                    iFModifyCNT = iFModifyCNT - 1;

                                    i = iEndPosition;
                                }
                                else
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

                return dtReturn;
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
                dtSocketSendList.Columns.Clear();

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
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return bResult;
            }
            #endregion
        }
        #endregion
    }
}
