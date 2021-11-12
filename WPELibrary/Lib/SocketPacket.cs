using System.Runtime.InteropServices;

namespace WPELibrary.Lib
{
    public class SocketPacket
    {
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

        //类别
        private string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        //套接字
        private int socket;
        public int Socket
        {
            get { return socket; }
            set { socket = value; }
        }

        //长度
        private int length;
        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        //字节
        private byte[] buffer;
        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        //地址栈
        private sockaddr addr;
        public sockaddr Addr
        {
            get { return addr; }
            set { addr = value; }
        }

        public SocketPacket(string type, int socket, int length, byte[] buffer, sockaddr addr)
        {
            this.type = type;
            this.socket = socket;          
            this.length = length;           
            this.buffer = buffer;
            this.addr = addr;
        }
    }
}
