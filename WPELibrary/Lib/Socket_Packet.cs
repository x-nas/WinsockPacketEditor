using System;
using System.Runtime.InteropServices;

namespace WPELibrary.Lib
{
    public class Socket_Packet
    {
        #region//结构定义
        public struct in_addr
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] sin_addr;
        }

        public struct sockaddr
        {
            public short sin_family;
            public ushort sin_port;
            public in_addr sin_addr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] sin_zero;
        }

        public unsafe struct WSABUF
        {
            public Int32 len;
            public IntPtr buf;
        }

        public enum SocketType
        {
            Send = 1,
            WSASend = 2,
            SendTo = 3,
            Recv = 4,
            WSARecv = 5,
            RecvFrom = 6,
            Send_Interecept = 7,
            WSASend_Interecept = 8,
            SendTo_Interecept = 9,
            Recv_Interecept = 10,
            WSARecv_Interecept = 11,
            RecvFrom_Interecept = 12,
        }

        public enum IPType
        {
            From = 1,
            To = 2,
        }
        #endregion

        #region//套接字
        protected int socket;
        public int Socket
        {
            get { return socket; }
            set { socket = value; }
        }
        #endregion

        #region//缓冲区指针
        protected IntPtr ipt;
        public IntPtr Ipt
        {
            get { return ipt; }
            set { ipt = value; }
        }
        #endregion

        #region//缓冲区数据长度（字节）
        protected int len;
        public int Len
        {
            get { return len; }
            set { len = value; }
        }
        #endregion

        #region//类别
        protected SocketType type;
        public SocketType Type
        {
            get { return type; }
            set { type = value; }
        }
        #endregion

        #region//地址栈
        protected sockaddr addr;
        public sockaddr Addr
        {
            get { return addr; }
            set { addr = value; }
        }
        #endregion

        #region//封包内容（字节）
        protected byte[] buffer;
        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }
        #endregion

        #region//返回结果
        protected int reslen;
        public int ResLen
        {
            get { return reslen; }
            set { reslen = value; }
        }
        #endregion

        #region//Socket_Class
        public Socket_Packet()
        {
            //
        }

        public Socket_Packet(int socket, IntPtr ipt, int len, SocketType type, sockaddr addr, byte[] buffer, int reslen)
        {
            this.socket = socket;
            this.ipt = ipt;
            this.len = len;
            this.type = type;
            this.addr = addr;
            this.buffer = buffer;
            this.reslen = reslen;
        }
        #endregion        
    }
}
