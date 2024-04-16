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
        private static string sLanguage = "";
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
                byte[] bBuffer = new byte[iResLen];
                Marshal.Copy(ipBuff, bBuffer, 0, iResLen);

                Socket_Packet sp = new Socket_Packet(iSocket, ipBuff, iLen, sType, sAddr, bBuffer, iResLen);

                Thread tSocket_Queue = new Thread(new ParameterizedThreadStart(SocketToQueue_Thread));
                tSocket_Queue.Start(sp);
            }

            private static void SocketToQueue_Thread(object ob)
            {
                Socket_Packet sp = (Socket_Packet)ob;

                lock (qSocket_Packet)
                {
                    qSocket_Packet.Enqueue(sp);
                }                
            }

            #endregion

            #region//清除队列数据
            public static void ResetQueue()
            {
                Interecept_CNT = 0;
                Recv_CNT = 0;
                Send_CNT = 0;
                qSocket_Packet.Clear();
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

                            Socket_Packet_Info si = new Socket_Packet_Info(iIndex, sType, iSocket, sIP_From, sIP_To, iResLen, sData, bBuffer);

                            RecSocketPacket?.Invoke(si);
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

            #region//添加到发送列表
            public static void SendList_Add_BySocketListIndex(int iSLIndex)
            {
                SendList_Add(
                    SocketList.lstRecPacket[iSLIndex].Index,
                    "",
                    SocketList.lstRecPacket[iSLIndex].Socket,
                    SocketList.lstRecPacket[iSLIndex].To,
                    SocketList.lstRecPacket[iSLIndex].ResLen,
                    SocketList.lstRecPacket[iSLIndex].Data,
                    SocketList.lstRecPacket[iSLIndex].Buffer
                    );
            }

            public static void SendList_Add(int iIndex, string sNote, int iSocket, string sIPTo, int iResLen, string sData, byte[] bBuffer)
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

        #region//日志队列
        public static class LogQueue
        {
            public static Queue<Socket_Log> qSocket_Log = new Queue<Socket_Log>();

            #region//日志入队列（多线程）
            public static void LogToQueue(string sLogContent)
            {
                Socket_Log sl = new Socket_Log(sLogContent);

                Thread tLog_Queue = new Thread(new ParameterizedThreadStart(LogToQueue_Thread));
                tLog_Queue.Start(sl);
            }

            private static void LogToQueue_Thread(object ob)
            {
                Socket_Log sl = (Socket_Log)ob;

                lock (qSocket_Log)
                {
                    qSocket_Log.Enqueue(sl);
                }               
            }

            #endregion

            #region//清除队列数据
            public static void ResetQueue()
            {
                qSocket_Log.Clear();
            }
            #endregion
        }
        #endregion

        #region//日志入列表
        public static class LogList
        {
            public static BindingList<Socket_Log> lstRecLog = new BindingList<Socket_Log>();

            public delegate void SocketLogReceived(Socket_Log sl);
            public static event SocketLogReceived RecSocketLog;

            #region//日志入列表
            public static void LogToList()
            {
                try
                {
                    if (LogQueue.qSocket_Log.Count > 0)
                    {
                        Socket_Log sl = LogQueue.qSocket_Log.Dequeue();
                        RecSocketLog?.Invoke(sl);
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
    }
}
