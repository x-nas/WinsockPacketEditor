using System;
using System.Runtime.InteropServices;
using EasyHook;

namespace WPELibrary.Lib
{
    public class WinSockHook
    {
        private LocalHook lhSend, lhSendTo, lhRecv, lhRecvFrom;        

        #region//ws2_32.dll Send Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int send(int socket, IntPtr buffer, int length, int flags);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int SendHook(int socket, IntPtr buffer, int length, int flags);
        private int Send_Hook(int socket, IntPtr buffer, int length, int flags)
        {
            int res = 0;
            Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

            if (Socket_Cache.Interecept_Send)
            {
                Socket_Cache.SocketQueue.Interecept_CNT++;
                stSocketType = Socket_Packet.SocketType.Send_Interecept;

                Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
            }
            else
            {
                Filter_List.DoFilter(buffer, length);

                res = send(socket, buffer, length, flags);

                if (res > 0)
                {
                    if (Socket_Cache.Display_Send)
                    {
                        Socket_Cache.SocketQueue.Send_CNT++;
                        stSocketType = Socket_Packet.SocketType.Send;
                        Socket_Cache.SocketQueue.SocketToQueue(socket, buffer, length, stSocketType, new Socket_Packet.sockaddr(), res);

                        Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
                    }
                }                
            }            

            return res;
        }

        #endregion                

        #region//ws2_32.dll SendTo Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int sendto(int socket, IntPtr buffer, int length, int flags, ref Socket_Packet.sockaddr To, ref int toLenth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int SendToHook(int socket, IntPtr buffer, int length, int flags, ref Socket_Packet.sockaddr To, ref int toLenth);
        private int SendTo_Hook(int socket, IntPtr buffer, int length, int flags, ref Socket_Packet.sockaddr To, ref int toLenth)
        {
            int res = 0;
            Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

            if (Socket_Cache.Interecept_SendTo)
            {
                Socket_Cache.SocketQueue.Interecept_CNT++;
                stSocketType = Socket_Packet.SocketType.SendTo_Interecept;

                Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
            }
            else
            {
                Filter_List.DoFilter(buffer, length);

                res = sendto(socket, buffer, length, flags, ref To, ref toLenth);

                if (res > 0)
                {
                    if (Socket_Cache.Display_SendTo)
                    {
                        Socket_Cache.SocketQueue.Send_CNT++;
                        stSocketType = Socket_Packet.SocketType.SendTo;
                        Socket_Cache.SocketQueue.SocketToQueue(socket, buffer, length, stSocketType, To, res);

                        Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
                    }
                }
            }

            return res;
        }

        #endregion                        

        #region//ws2_32.dll Recv Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int recv(int socket, IntPtr buffer, int length, int flags);        

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int RecvHook(int socket, IntPtr buffer, int length, int flags);
        private int Recv_Hook(int socket, IntPtr buffer, int length, int flags)
        {
            int res = 0;
            Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

            res = recv(socket, buffer, length, flags);

            if (res > 0)
            {
                if (Socket_Cache.Interecept_Recv)
                {
                    byte[] bBuff_NULL = new byte[res];
                    Socket_Operation.SetByteToIntPtr(bBuff_NULL, buffer, res);

                    Socket_Cache.SocketQueue.Interecept_CNT++;
                    stSocketType = Socket_Packet.SocketType.Recv_Interecept;
                }
                else
                {
                    Filter_List.DoFilter(buffer, length);

                    if (Socket_Cache.Display_Recv)
                    {
                        Socket_Cache.SocketQueue.Recv_CNT++;
                        stSocketType = Socket_Packet.SocketType.Recv;
                        Socket_Cache.SocketQueue.SocketToQueue(socket, buffer, length, stSocketType, new Socket_Packet.sockaddr(), res);
                    }
                }

                Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
            }

            return res;
        }

        #endregion        

        #region//ws2_32.dll RecvFrom Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int recvfrom(int socket, IntPtr buffer, int length, int flags, ref Socket_Packet.sockaddr from, ref int fromLen);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int RecvFromHook(int socket, IntPtr buffer, int length, int flags, ref Socket_Packet.sockaddr from, ref int fromLen);
        private int RecvFrom_Hook(int socket, IntPtr buffer, int length, int flags, ref Socket_Packet.sockaddr from, ref int fromLen)
        {
            int res = 0;
            Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

            res = recvfrom(socket, buffer, length, flags, ref from, ref fromLen);

            if (res > 0)
            {
                if (Socket_Cache.Interecept_RecvFrom)
                {
                    byte[] bBuff_NULL = new byte[res];
                    Socket_Operation.SetByteToIntPtr(bBuff_NULL, buffer, res);

                    Socket_Cache.SocketQueue.Interecept_CNT++;
                    stSocketType = Socket_Packet.SocketType.RecvFrom_Interecept;
                }
                else
                {
                    Filter_List.DoFilter(buffer, length);

                    if (Socket_Cache.Display_RecvFrom)
                    {
                        Socket_Cache.SocketQueue.Recv_CNT++;
                        stSocketType = Socket_Packet.SocketType.RecvFrom;
                        Socket_Cache.SocketQueue.SocketToQueue(socket, buffer, length, stSocketType, from, res);
                    }
                }

                Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
            }
            
            return res;
        }

        #endregion

        #region//开始拦截
        public void StartHook()
        {
            lhRecv = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "recv"), new RecvHook(Recv_Hook), this);
            lhRecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            lhRecvFrom = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "recvfrom"), new RecvFromHook(RecvFrom_Hook), this);
            lhRecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            lhSend = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "send"), new SendHook(Send_Hook), this);
            lhSend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            lhSendTo = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "sendto"), new SendToHook(SendTo_Hook), this);
            lhSendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            Socket_Operation.DoLog("开始拦截！");
        }
        #endregion

        #region//停止拦截
        public void StopHook()
        {
            lhRecv.Dispose();
            lhSend.Dispose();
            lhRecvFrom.Dispose();
            lhSendTo.Dispose();

            Socket_Operation.DoLog("结束拦截！");
        }
        #endregion        

        #region//发送封包
        public static bool SendPacket(int socket, IntPtr buffer, int length)
        {
            bool bReturn = false;

            int res = send(socket, buffer, length, 0);

            if (res > 0)
            {
                bReturn = true;
                Socket_Operation.DoLog("发送封包成功！");
            }
            else
            {
                bReturn = false;
                Socket_Operation.DoLog("发送封包失败！");
            }

            return bReturn;            
        }
        #endregion
    }
}
