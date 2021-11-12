using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EasyHook;

namespace WPELibrary.Lib
{
    public class WinSockHook
    { 
        public Queue<SocketPacket> _SocketQueue = new Queue<SocketPacket>();        

        public bool Interecept_Recv;
        public bool Interecept_RecvFrom;
        public bool Interecept_Send;
        public bool Interecept_SendTo;

        public bool Display_Recv;
        public bool Display_RecvFrom;
        public bool Display_Send;
        public bool Display_SendTo;

        public bool Reset_CNT;

        public int Interecept_CNT = 0;
        public int Recv_CNT = 0;
        public int Send_CNT = 0;

        private LocalHook lhSend = null;
        private LocalHook lhSendTo = null;
        private LocalHook lhRecv = null;
        private LocalHook lhRecvFrom = null;

        #region//ws2_32.dll Recv Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int recv(int socket, IntPtr buffer, int length, int flags);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int RecvHook(int s, IntPtr buf, int length, int flags);
        private int Recv_Hook(int s, IntPtr buf, int length, int flags)
        {
            int res = 0;

            if (Interecept_Recv)
            {
                if (length > 0)
                {
                    Interecept_CNT++;
                }

                res = length;
            }
            else
            {
                res = recv(s, buf, length, flags);
            }

            if (res > 0)
            {
                if (Display_Recv)
                {
                    Recv_CNT++;
                    this.SocketEnqueue(s, buf, res, "R", new SocketPacket.sockaddr());
                }
            }

            return res;
        }

        #endregion

        #region//ws2_32.dll RecvFrom Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int recvfrom(int socket, IntPtr buffer, int length, int flags, ref SocketPacket.sockaddr from, ref int fromLen);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int RecvFromHook(int socket, IntPtr buffer, int length, int flags, ref SocketPacket.sockaddr from, ref int fromLen);
        private int RecvFrom_Hook(int socket, IntPtr buffer, int length, int flags, ref SocketPacket.sockaddr from, ref int fromLen)
        {
            int res = 0;

            if (Interecept_RecvFrom)
            {
                if (length > 0)
                {
                    Interecept_CNT++;
                }

                res = length;
            }
            else
            {
                res = recvfrom(socket, buffer, length, flags, ref from, ref fromLen);
            }

            if (res > 0)
            {
                if (Display_RecvFrom)
                {
                    Recv_CNT++;
                    this.SocketEnqueue(socket, buffer, res, "RF", from);
                }
            }

            return res;
        }

        #endregion

        #region//ws2_32.dll Send Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int send(int socket, IntPtr buffer, int length, int flags);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int SendHook(int s, IntPtr buf, int length, int flags);
        private int Send_Hook(int s, IntPtr buf, int length, int flags)
        {
            int res = 0;

            if (Interecept_Send)
            {
                if (length > 0)
                {
                    Interecept_CNT++;
                }

                res = length;
            }
            else
            {
                res = send(s, buf, length, flags);
            }

            if (res > 0)
            {
                if (Display_Send)
                {
                    Send_CNT++;
                    this.SocketEnqueue(s, buf, res, "S", new SocketPacket.sockaddr());
                }
            }

            return res;
        }

        #endregion        

        #region//ws2_32.dll SendTo Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int sendto(int socket, IntPtr buffer, int length, int flags, ref SocketPacket.sockaddr To, ref int toLenth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int SendToHook(int socket, IntPtr buffer, int length, int flags, ref SocketPacket.sockaddr To, ref int toLenth);
        private int SendTo_Hook(int socket, IntPtr buffer, int length, int flags, ref SocketPacket.sockaddr To, ref int toLenth)
        {
            int res = 0;

            if (Interecept_SendTo)
            {
                if (length > 0)
                {
                    Interecept_CNT++;
                }

                res = length;
            }
            else
            {
                res = sendto(socket, buffer, length, flags, ref To, ref toLenth);
            }

            if (res > 0)
            {
                if (Display_SendTo)
                {
                    Send_CNT++;
                    this.SocketEnqueue(socket, buffer, res, "ST", To);
                }
            }

            return res;
        }

        #endregion                        

        //开始拦截
        public void StartHook()
        {
            ResetCNT();

            lhRecv = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "recv"), new RecvHook(Recv_Hook), this);
            lhRecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            lhRecvFrom = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "recvfrom"), new RecvFromHook(RecvFrom_Hook), this);
            lhRecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            lhSend = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "send"), new SendHook(Send_Hook), this);
            lhSend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            lhSendTo = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "sendto"), new SendToHook(SendTo_Hook), this);
            lhSendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
        }

        //停止拦截
        public void StopHook()
        {
            lhRecv.Dispose();
            lhSend.Dispose();
            lhRecvFrom.Dispose();
            lhSendTo.Dispose();
        }

        //重置封包计数
        private void ResetCNT()
        {
            if (Reset_CNT)
            {
                Interecept_CNT = 0;
                Recv_CNT = 0;
                Send_CNT = 0;

                this._SocketQueue.Clear();
            }
        }

        //封包入队列
        private void SocketEnqueue(int iSocket, IntPtr ipBuff, int iLen, string sType, SocketPacket.sockaddr sAddr)
        {
            byte[] bBuffer = new byte[iLen];
            Marshal.Copy(ipBuff, bBuffer, 0, iLen);            

            SocketPacket sp = new SocketPacket(sType, iSocket, iLen, bBuffer, sAddr);
            _SocketQueue.Enqueue(sp);            
        }

        //发送封包
        public bool SendPacket(int socket, IntPtr buffer, int length)
        {
            bool bReturn = false;

            int res = send(socket, buffer, length, 0);

            if (res > 0)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }

            return bReturn;            
        }
    }
}
