using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WPETarget32
{
    /// <summary>
    /// 32 位靶子：连上给定端口，反复发同一句话，直到到时或被杀。
    ///
    /// 与 64 位那个靶子的脚本一致（同样的 HELLO-WORLD-nnnn），
    /// 好让两种位数的报告能直接对比。
    /// </summary>
    internal static class Program
    {
        private static int Main(string[] args)
        {
            int port = int.Parse(Arg(args, "--port", "0"));
            int seconds = int.Parse(Arg(args, "--seconds", "30"));

            /*
                ⚠️ 三个 DLL 要先加载。
                EasyHook 的 GetProcAddress 要求模块已经在本进程里，
                而 .NET 做 socket 会带起 ws2_32 与 mswsock、<b>不会</b>带起 wsock32 ——
                不补这一句 WS1 那四个钩子就装不上，而 StartHook 把异常吞进日志了。
            */
            LoadLibrary("ws2_32.dll");
            LoadLibrary("wsock32.dll");
            LoadLibrary("mswsock.dll");

            IntPtr wsaData = Marshal.AllocHGlobal(408);
            try { WSAStartup(0x0202, wsaData); }
            finally { Marshal.FreeHGlobal(wsaData); }

            IntPtr sock = socket(2, 1, 6);   //AF_INET, SOCK_STREAM, IPPROTO_TCP

            var to = new SockAddr
            {
                sin_family = 2,
                sin_port = htons((ushort)port),
                sin_addr = inet_addr("127.0.0.1"),
            };

            if (connect(sock, ref to, 16) != 0)
            {
                Console.Error.WriteLine("连不上 " + port + " err=" + Marshal.GetLastWin32Error());
                return 2;
            }

            Console.WriteLine("TARGET32_READY pid=" + System.Diagnostics.Process.GetCurrentProcess().Id +
                              " is64=" + (IntPtr.Size == 8));
            Console.Out.Flush();

            int deadline = unchecked(Environment.TickCount + seconds * 1000);
            int round = 0;

            while (unchecked(deadline - Environment.TickCount) > 0)
            {
                round++;
                Say(sock, "HELLO-WORLD-" + round.ToString("D4"));
                Thread.Sleep(80);
            }

            closesocket(sock);
            return 0;
        }

        private static unsafe void Say(IntPtr s, string payload)
        {
            byte[] b = Encoding.ASCII.GetBytes(payload);
            fixed (byte* p = b) { send((int)s, (IntPtr)p, b.Length, 0); }
        }

        private static string Arg(string[] args, string name, string def)
        {
            int i = Array.IndexOf(args, name);
            return (i >= 0 && i + 1 < args.Length) ? args[i + 1] : def;
        }

        #region//裸 P/Invoke（不引主工程，见 csproj 的说明）

        [StructLayout(LayoutKind.Sequential)]
        private struct SockAddr
        {
            public short sin_family;
            public ushort sin_port;
            public uint sin_addr;
            private long Zero;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string name);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int WSAStartup(ushort ver, IntPtr data);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern IntPtr socket(int af, int type, int protocol);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int connect(IntPtr s, ref SockAddr name, int len);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int closesocket(IntPtr s);

        //走 ws2_32 的 send —— 与 64 位靶子同一个入口（WS2_Send）
        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int send(int s, IntPtr buf, int len, int flags);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern uint inet_addr(string cp);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern ushort htons(ushort v);

        #endregion
    }
}
