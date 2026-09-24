using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using WinsockPacketEditor;

namespace WPEHookTest
{
    /// <summary>
    /// 跑测自己要用的 P/Invoke。
    ///
    /// 【为什么不直接用主工程的 WS2_32 / WSock32】那两个类里的 DllImport 是<b>被钩住的目标</b>，
    /// 调它们当然也会命中钩子 —— 这正是我们要的。但 WSASendTo / WSARecvFrom 的
    /// 参数形状（ref SockAddr）在跑测里不好摆，另外 accept/bind/listen 主工程没有导入。
    /// 所以这里只补主工程没有的那几个，收发一律走主工程的导入，保证走的是同一条路。
    /// </summary>
    internal static class Native
    {
        public const int AF_INET = 2;
        public const int SOCK_STREAM = 1;
        public const int SOCK_DGRAM = 2;
        public const int IPPROTO_TCP = 6;
        public const int IPPROTO_UDP = 17;

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern IntPtr socket(int af, int type, int protocol);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern int bind(IntPtr s, ref Operate.PacketConfig.Packet.SockAddr name, int namelen);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern int listen(IntPtr s, int backlog);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern IntPtr accept(IntPtr s, IntPtr addr, IntPtr addrlen);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern int connect(IntPtr s, ref Operate.PacketConfig.Packet.SockAddr name, int namelen);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern int closesocket(IntPtr s);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern int getsockname(IntPtr s, ref Operate.PacketConfig.Packet.SockAddr name, ref int namelen);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern uint inet_addr(string cp);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern ushort htons(ushort hostshort);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern ushort ntohs(ushort netshort);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern int WSAStartup(ushort versionRequested, IntPtr wsaData);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// WSASendTo / WSARecvFrom 的导入。主工程那两个的签名是给 detour 用的，
        /// 参数里带 ref SockAddr，跑测直接调不方便，这里另导一份指针形态的。
        /// </summary>
        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern SocketError WSASendTo(IntPtr s, IntPtr lpBuffers, int dwBufferCount,
            out int lpNumberOfBytesSent, SocketFlags dwFlags, IntPtr lpTo, int iToLen,
            IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        [DllImport("ws2_32.dll", SetLastError = true)]
        public static extern SocketError WSARecvFrom(IntPtr s, IntPtr lpBuffers, int dwBufferCount,
            out int lpNumberOfBytesRecvd, ref SocketFlags lpFlags, IntPtr lpFrom, IntPtr lpFromLen,
            IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        /// <summary>拼一个 IPv4 的 sockaddr_in（端口按网络序）。</summary>
        public static Operate.PacketConfig.Packet.SockAddr MakeAddr(string ip, ushort port)
        {
            return new Operate.PacketConfig.Packet.SockAddr
            {
                sin_family = AF_INET,
                sin_port = htons(port),
                sin_addr = inet_addr(ip),
            };
        }
    }
}
