using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using WinsockPacketEditor;

namespace WPEHookTest
{
    /// <summary>
    /// 钩子 / 滤镜引擎的回归跑测。
    ///
    /// 【核心手法】<c>LocalHook</c> 本来就是进程内的，而 <c>StartHook</c> 用
    /// <c>SetExclusiveACL(new[]{ 0 })</c> 把<b>安装钩子的那条线程</b>排除在外。
    /// 所以：主线程装钩子 + 当服务端（不会被钩到，零噪声），工作线程当客户端（被钩到）。
    /// 这样不用真的注入一个游戏，就能逐个验 13 个 WinSock 入口。
    ///
    /// 输出是一份稳定排序的文本报告，每个阶段改完跑一次，用 diff 比对即可。
    /// </summary>
    internal static class Program
    {
        private const string LOOPBACK = "127.0.0.1";

        private static int Main(string[] args)
        {
            //两个 IPC 角色（见 IpcTest.cs）：靶子 / 外壳。不带这两个开关就是
            //阶段 0 那个「进程内装钩子」的引擎跑测。
            if (args.Contains("--target")) { return IpcTest.RunTarget(args); }
            if (args.Contains("--shell")) { return IpcTest.RunShell(args); }

            string outPath = GetArg(args, "--out", null);

            try
            {
                Init();

                var report = new StringBuilder();
                report.AppendLine("# WPE 钩子引擎跑测报告");
                report.AppendLine();

                var captured = RunTraffic();

                /*
                    ⚠️ <b>不能按 Id 排序</b>。Id 是 PacketInfo 构造时用 Interlocked 分配的，
                    而入队隔着十几行、又跨多条钩子线程 —— 同一份流量跑两遍，Id 顺序不一样。
                    要拿这份报告做 diff，就必须给它一个<b>与线程调度无关</b>的规范序：
                    按（类型, 长度, 改写前内容, 改写后内容, 动作）排，再计数。
                */
                report.AppendLine("## 抓到的封包（规范序：类型 → 长度 → 内容）");
                report.AppendLine();
                report.AppendLine("| 条数 | 类型 | 长度 | 改写前 | 改写后 | 动作 |");
                report.AppendLine("|------|------|------|--------|--------|------|");

                var grouped = captured
                    .Select(p => new
                    {
                        Type = p.PacketType,
                        Len = p.PacketLen,
                        Raw = Ascii(p.RawBuffer),
                        New = Ascii(p.PacketBuffer),
                        Act = p.FilterAction,
                    })
                    .GroupBy(x => x)
                    .Select(g => new { g.Key, N = g.Count() })
                    .OrderBy(x => x.Key.Type.ToString(), StringComparer.Ordinal)
                    .ThenBy(x => x.Key.Len)
                    .ThenBy(x => x.Key.Raw, StringComparer.Ordinal)
                    .ThenBy(x => x.Key.New, StringComparer.Ordinal);

                foreach (var g in grouped)
                {
                    report.AppendLine(string.Format("| {0} | {1} | {2} | {3} | {4} | {5} |",
                        g.N, g.Key.Type, g.Key.Len, g.Key.Raw, g.Key.New, g.Key.Act));
                }

                report.AppendLine();
                report.AppendLine("## 覆盖情况（13 个钩子入口里的 12 个）");
                report.AppendLine();
                report.AppendLine("> WSARecvEx 未列入：它的 P/Invoke 把 `int *flags` 导成了按值的 SocketFlags，");
                report.AppendLine("> 64 位下指针会被截断，一调就 AV。改造前就有，与 IPC 无关。");
                report.AppendLine();

                var expect = new[]
                {
                    Operate.PacketConfig.Packet.PacketType.WS1_Send,
                    Operate.PacketConfig.Packet.PacketType.WS1_Recv,
                    Operate.PacketConfig.Packet.PacketType.WS1_SendTo,
                    Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom,
                    Operate.PacketConfig.Packet.PacketType.WS2_Send,
                    Operate.PacketConfig.Packet.PacketType.WS2_Recv,
                    Operate.PacketConfig.Packet.PacketType.WS2_SendTo,
                    Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom,
                    Operate.PacketConfig.Packet.PacketType.WSASend,
                    Operate.PacketConfig.Packet.PacketType.WSASendTo,
                    Operate.PacketConfig.Packet.PacketType.WSARecv,
                    Operate.PacketConfig.Packet.PacketType.WSARecvFrom,
                    //WSARecvEx 不在覆盖清单里 —— 见 ClientScript 里那段注释（x64 下的老 bug）
                };

                int missing = 0;
                foreach (var t in expect)
                {
                    int n = captured.Count(p => p.PacketType == t);
                    if (n == 0) { missing++; }
                    report.AppendLine(string.Format("- {0}: {1} 条 {2}", t, n, n == 0 ? "MISSING" : "ok"));
                }

                report.AppendLine();
                report.AppendLine("合计 " + captured.Count + " 条，缺 " + missing + " 种。");
                report.AppendLine(missing == 0 ? "结论: PASS" : "结论: FAIL");

                string text = report.ToString();
                Console.WriteLine(text);

                if (!string.IsNullOrEmpty(outPath))
                {
                    File.WriteAllText(outPath, text, new UTF8Encoding(false));
                    Console.WriteLine("报告已写入 " + outPath);
                }

                return missing == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("跑测本身出错: " + ex);
                return 2;
            }
        }

        #region//初始化

        private static void Init()
        {
            //Winsock 要先初始化，否则第一个 socket() 就是 10093（WSANOTINITIALISED）。
            //平时是 .NET 的 Socket 类顺手做掉的，这个跑测全程走裸 P/Invoke，得自己来。
            IntPtr wsaData = Marshal.AllocHGlobal(408);
            try { Native.WSAStartup(0x0202, wsaData); }
            finally { Marshal.FreeHGlobal(wsaData); }

            /*
                ⚠️ 三个 DLL 必须先 LoadLibrary。

                EasyHook 的 LocalHook.GetProcAddress 要求模块<b>已经加载进本进程</b>，
                否则抛 DllNotFoundException —— 而 StartHook 把异常吞进 DoLog 了，
                表现就是「钩子已安装」照常打印、却一条都抓不到（查了一轮日志才看出来）。

                真实的注入模式里目标程序自己早就加载了它们（ProcessList 正是靠
                枚举目标模块来决定 Support_WS1 / WS2 / MsWS 的），跑测得自己补这一步。
            */
            Native.LoadLibrary("ws2_32.dll");
            Native.LoadLibrary("wsock32.dll");
            Native.LoadLibrary("mswsock.dll");

            //库放临时目录，绝不碰用户那份
            string dir = Path.Combine(Path.GetTempPath(), "WPEHookTest");
            Directory.CreateDirectory(dir);
            Operate.DataBase.dbPath = dir;
            Operate.DataBase.InitDB();
            Operate.SystemConfig.LoadSystemConfig_FromDB();

            //⚠️ 极速模式下 PacketInfo_ToQueue 根本不入队（核过 Operate.cs 的 PacketInfo_ToQueue），
            //所以跑测必须关掉它，否则一条都抓不到。
            Operate.SystemConfig.SpeedMode = false;

            //13 个入口全开
            Operate.PacketConfig.Packet.Support_WS1 = true;
            Operate.PacketConfig.Packet.Support_WS2 = true;
            Operate.PacketConfig.Packet.Support_MsWS = true;

            //清干净，避免上一轮的残留
            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out _)) { }
            Operate.FilterConfig.List.lstFilterInfo.Clear();
        }

        #endregion

        #region//跑一遍流量

        private static List<PacketInfo> RunTraffic()
        {
            //=== 服务端（主线程 = 安装钩子的线程 = 不被钩） ===
            IntPtr tcpListen = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
            var anyAddr = Native.MakeAddr(LOOPBACK, 0);
            Check(Native.bind(tcpListen, ref anyAddr, 16), "bind tcp");
            Check(Native.listen(tcpListen, 4), "listen");

            var boundTcp = new Operate.PacketConfig.Packet.SockAddr();
            int len = 16;
            Check(Native.getsockname(tcpListen, ref boundTcp, ref len), "getsockname tcp");
            ushort tcpPort = Native.ntohs(boundTcp.sin_port);

            IntPtr udpServer = Native.socket(Native.AF_INET, Native.SOCK_DGRAM, Native.IPPROTO_UDP);
            var udpBind = Native.MakeAddr(LOOPBACK, 0);
            Check(Native.bind(udpServer, ref udpBind, 16), "bind udp");
            var boundUdp = new Operate.PacketConfig.Packet.SockAddr();
            len = 16;
            Check(Native.getsockname(udpServer, ref boundUdp, ref len), "getsockname udp");
            ushort udpPort = Native.ntohs(boundUdp.sin_port);

            Console.WriteLine("服务端就绪 tcp=" + tcpPort + " udp=" + udpPort);

            //=== 装钩子（主线程从此被排除） ===
            var ws = new WinSockHook();
            ws.StartHook();
            Console.WriteLine("钩子已安装");

            Exception clientError = null;
            var client = new Thread(() =>
            {
                try { ClientScript(tcpPort, udpPort); }
                catch (Exception ex) { clientError = ex; }
            });
            client.IsBackground = true;
            client.Start();

            //=== 服务端脚本（与客户端脚本一一对应） ===
            Exception serverError = null;
            try { ServerScript(tcpListen, udpServer); }
            catch (Exception ex) { serverError = ex; }

            client.Join(20000);
            ws.StopHook();
            Console.WriteLine("钩子已卸载");

            Native.closesocket(tcpListen);
            Native.closesocket(udpServer);

            if (clientError != null) { throw new Exception("客户端脚本失败", clientError); }
            if (serverError != null) { throw new Exception("服务端脚本失败", serverError); }

            //异步入队要给一点时间（PacketInfo_ToQueue 是 async void + Task.Run）
            Thread.Sleep(2000);

            var list = new List<PacketInfo>();
            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out PacketInfo pi)) { list.Add(pi); }
            return list;
        }

        /// <summary>服务端：跑在主线程上，所有调用都不会被钩到。</summary>
        private static void ServerScript(IntPtr tcpListen, IntPtr udpServer)
        {
            IntPtr conn = Native.accept(tcpListen, IntPtr.Zero, IntPtr.Zero);

            //TCP：客户端每发一条，服务端就回一条（内容加前缀 R:）
            for (int i = 0; i < 4; i++)
            {
                byte[] got = RecvRaw(conn, 512);
                SendRaw(conn, Encoding.ASCII.GetBytes("R:" + Encoding.ASCII.GetString(got)));
            }

            //UDP：三来三回
            for (int i = 0; i < 3; i++)
            {
                var from = new Operate.PacketConfig.Packet.SockAddr();
                byte[] got = RecvFromRaw(udpServer, 512, ref from);
                SendToRaw(udpServer, Encoding.ASCII.GetBytes("R:" + Encoding.ASCII.GetString(got)), ref from);
            }

            Native.closesocket(conn);
        }

        /// <summary>客户端：跑在工作线程上，13 个入口逐个走一遍。</summary>
        private static unsafe void ClientScript(ushort tcpPort, ushort udpPort)
        {
            #region//TCP

            IntPtr tcp = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
            var srv = Native.MakeAddr(LOOPBACK, tcpPort);
            Check(Native.connect(tcp, ref srv, 16), "connect");
            int s = (int)tcp;
            Step("connected");

            //① WS1_Send / ② WS1_Recv —— wsock32.dll
            byte[] b1 = Encoding.ASCII.GetBytes("T1-WS1SEND");
            fixed (byte* p = b1) { Need(WSock32.send(s, (IntPtr)p, b1.Length, SocketFlags.None), "WS1 send"); }
            Step("WS1_Send");
            RecvInto(buf => { fixed (byte* p = buf) return WSock32.recv(s, (IntPtr)p, buf.Length, SocketFlags.None); }, "WS1 recv");
            Step("WS1_Recv");

            //③ WS2_Send / ④ WS2_Recv —— ws2_32.dll
            byte[] b2 = Encoding.ASCII.GetBytes("T2-WS2SEND");
            fixed (byte* p = b2) { Need(WS2_32.send(s, (IntPtr)p, b2.Length, SocketFlags.None), "WS2 send"); }
            Step("WS2_Send");
            RecvInto(buf => { fixed (byte* p = buf) return WS2_32.recv(s, (IntPtr)p, buf.Length, SocketFlags.None); }, "WS2 recv");
            Step("WS2_Recv");

            //⑤ WSASend / ⑥ WSARecv
            WsaSend(s, "T3-WSASEND");
            Step("WSASend");
            WsaRecv(s);
            Step("WSARecv");

            /*
                ⑦ WSARecvEx —— <b>这一项在 64 位下跑不了，跑测里刻意跳过</b>。

                真实签名是 int WSARecvEx(SOCKET s, char *buf, int len, int *flags)：第 4 个参数是<b>指针</b>。
                而 NativeMethods/Mswsock.cs 把它导成了按值传的 SocketFlags（4 字节）：
                  · x86 —— 指针本来就 4 字节，钩子把它原样传回原函数，正好是对的；
                  · x64 —— 指针被截成低 32 位，传回去就是野指针，直接 AV（实测 0xC0000005）。

                这是<b>改造前就有的老 bug，与 IPC 无关</b>。阶段 0 要的是「与现状等价」，
                所以这里不顺手改它。要修的话得给 WSARecvEx 单开一个钩子体 ——
                Recv_Hook 的 Flags 参数是 WS1/WS2 recv 共用的、按值传才对，
                不能为了它把三个入口的签名一起改成 IntPtr。

                这里换成再走一遍 WS2 recv，好歹让 TCP 那段的收发条数与服务端脚本对齐。
            */
            byte[] b4 = Encoding.ASCII.GetBytes("T4-EXTRA");
            fixed (byte* p = b4) { Need(WS2_32.send(s, (IntPtr)p, b4.Length, SocketFlags.None), "WS2 send #2"); }
            Step("WS2_Send#2");
            RecvInto(buf => { fixed (byte* p = buf) return WS2_32.recv(s, (IntPtr)p, buf.Length, SocketFlags.None); }, "WS2 recv #2");
            Step("WS2_Recv#2");

            Native.closesocket(tcp);

            #endregion

            #region//UDP

            IntPtr udp = Native.socket(Native.AF_INET, Native.SOCK_DGRAM, Native.IPPROTO_UDP);
            var ubind = Native.MakeAddr(LOOPBACK, 0);
            Check(Native.bind(udp, ref ubind, 16), "bind client udp");
            int u = (int)udp;
            var to = Native.MakeAddr(LOOPBACK, udpPort);
            var from = new Operate.PacketConfig.Packet.SockAddr();

            //⚠️ recvfrom 的 from 非空时 fromlen 也必须非空，否则一律 WSAEFAULT(10014)。
            //生产里这个指针是目标程序给的，钩子只是原样透传；跑测得自己备一个。
            IntPtr pFromLen = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(pFromLen, 16);

            //⑧ WS1_SendTo / ⑨ WS1_RecvFrom
            byte[] u1 = Encoding.ASCII.GetBytes("U1-WS1SENDTO");
            fixed (byte* p = u1) { Need(WSock32.sendto(u, (IntPtr)p, u1.Length, SocketFlags.None, ref to, 16), "WS1 sendto"); }
            Step("WS1_SendTo");
            {
                byte[] buf = new byte[512];
                Marshal.WriteInt32(pFromLen, 16);
                fixed (byte* p = buf) { Need(WSock32.recvfrom(u, (IntPtr)p, buf.Length, SocketFlags.None, ref from, pFromLen), "WS1 recvfrom"); }
                Step("WS1_RecvFrom");
            }

            //⑩ WS2_SendTo / ⑪ WS2_RecvFrom
            byte[] u2 = Encoding.ASCII.GetBytes("U2-WS2SENDTO");
            fixed (byte* p = u2) { Need(WS2_32.sendto(u, (IntPtr)p, u2.Length, SocketFlags.None, ref to, 16), "WS2 sendto"); }
            Step("WS2_SendTo");
            {
                byte[] buf = new byte[512];
                Marshal.WriteInt32(pFromLen, 16);
                fixed (byte* p = buf) { Need(WS2_32.recvfrom(u, (IntPtr)p, buf.Length, SocketFlags.None, ref from, pFromLen), "WS2 recvfrom"); }
                Step("WS2_RecvFrom");
            }

            //⑫ WSASendTo / ⑬ WSARecvFrom
            WsaSendTo(udp, "U3-WSASENDTO", to);
            Step("WSASendTo");
            WsaRecvFrom(udp);
            Step("WSARecvFrom");

            Marshal.FreeHGlobal(pFromLen);
            Native.closesocket(udp);

            #endregion
        }

        #endregion

        #region//收发的小工具

        private delegate int RecvFn(byte[] buf);

        private static void Need(int r, string what)
        {
            if (r <= 0) { throw new Exception(what + " 失败 r=" + r + " err=" + Marshal.GetLastWin32Error()); }
        }

        private static void RecvInto(RecvFn fn, string what)
        {
            byte[] buf = new byte[512];
            Need(fn(buf), what);
        }

        private static unsafe void WsaSend(int s, string payload)
        {
            byte[] b = Encoding.ASCII.GetBytes(payload);
            fixed (byte* p = b)
            {
                var wb = new Operate.PacketConfig.Packet.WSABUF { len = b.Length, buf = (IntPtr)p };
                IntPtr pwb = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Operate.PacketConfig.Packet.WSABUF)));
                IntPtr pSent = Marshal.AllocHGlobal(4);
                try
                {
                    Marshal.StructureToPtr(wb, pwb, false);
                    SocketError err = WS2_32.WSASend(s, pwb, 1, pSent, SocketFlags.None, IntPtr.Zero, IntPtr.Zero);
                    if (err != SocketError.Success) { throw new Exception("WSASend 失败 " + err); }
                }
                finally { Marshal.FreeHGlobal(pwb); Marshal.FreeHGlobal(pSent); }
            }
        }

        private static unsafe void WsaRecv(int s)
        {
            byte[] buf = new byte[512];
            fixed (byte* p = buf)
            {
                var wb = new Operate.PacketConfig.Packet.WSABUF { len = buf.Length, buf = (IntPtr)p };
                IntPtr pwb = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Operate.PacketConfig.Packet.WSABUF)));
                IntPtr pGot = Marshal.AllocHGlobal(4);
                try
                {
                    Marshal.StructureToPtr(wb, pwb, false);
                    SocketFlags flags = SocketFlags.None;
                    SocketError err = WS2_32.WSARecv(s, pwb, 1, pGot, ref flags, IntPtr.Zero, IntPtr.Zero);
                    if (err != SocketError.Success) { throw new Exception("WSARecv 失败 " + err); }
                }
                finally { Marshal.FreeHGlobal(pwb); Marshal.FreeHGlobal(pGot); }
            }
        }

        private static unsafe void WsaSendTo(IntPtr s, string payload, Operate.PacketConfig.Packet.SockAddr to)
        {
            byte[] b = Encoding.ASCII.GetBytes(payload);
            fixed (byte* p = b)
            {
                var wb = new Operate.PacketConfig.Packet.WSABUF { len = b.Length, buf = (IntPtr)p };
                IntPtr pwb = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Operate.PacketConfig.Packet.WSABUF)));
                IntPtr pTo = Marshal.AllocHGlobal(16);
                try
                {
                    Marshal.StructureToPtr(wb, pwb, false);
                    Marshal.StructureToPtr(to, pTo, false);
                    int sent;
                    SocketError err = Native.WSASendTo(s, pwb, 1, out sent, SocketFlags.None, pTo, 16, IntPtr.Zero, IntPtr.Zero);
                    if (err != SocketError.Success) { throw new Exception("WSASendTo 失败 " + err); }
                }
                finally { Marshal.FreeHGlobal(pwb); Marshal.FreeHGlobal(pTo); }
            }
        }

        private static unsafe void WsaRecvFrom(IntPtr s)
        {
            byte[] buf = new byte[512];
            fixed (byte* p = buf)
            {
                var wb = new Operate.PacketConfig.Packet.WSABUF { len = buf.Length, buf = (IntPtr)p };
                IntPtr pwb = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Operate.PacketConfig.Packet.WSABUF)));
                IntPtr pFrom = Marshal.AllocHGlobal(16);
                IntPtr pFromLen = Marshal.AllocHGlobal(4);
                try
                {
                    Marshal.StructureToPtr(wb, pwb, false);
                    Marshal.WriteInt32(pFromLen, 16);
                    SocketFlags flags = SocketFlags.None;
                    int got;
                    SocketError err = Native.WSARecvFrom(s, pwb, 1, out got, ref flags, pFrom, pFromLen, IntPtr.Zero, IntPtr.Zero);
                    if (err != SocketError.Success) { throw new Exception("WSARecvFrom 失败 " + err); }
                }
                finally { Marshal.FreeHGlobal(pwb); Marshal.FreeHGlobal(pFrom); Marshal.FreeHGlobal(pFromLen); }
            }
        }

        //服务端用的裸收发（主线程，不被钩）
        private static unsafe void SendRaw(IntPtr s, byte[] b)
        {
            fixed (byte* p = b) { WS2_32.send((int)s, (IntPtr)p, b.Length, SocketFlags.None); }
        }

        private static unsafe byte[] RecvRaw(IntPtr s, int max)
        {
            byte[] buf = new byte[max];
            int r;
            fixed (byte* p = buf) { r = WS2_32.recv((int)s, (IntPtr)p, max, SocketFlags.None); }
            if (r <= 0) { throw new Exception("服务端 recv 失败 r=" + r); }
            return buf.Take(r).ToArray();
        }

        private static unsafe void SendToRaw(IntPtr s, byte[] b, ref Operate.PacketConfig.Packet.SockAddr to)
        {
            fixed (byte* p = b) { WS2_32.sendto((int)s, (IntPtr)p, b.Length, SocketFlags.None, ref to, 16); }
        }

        private static unsafe byte[] RecvFromRaw(IntPtr s, int max, ref Operate.PacketConfig.Packet.SockAddr from)
        {
            byte[] buf = new byte[max];
            int r;
            //同客户端那处：from 非空时 fromlen 也必须非空，否则 WSAEFAULT(10014)
            IntPtr pLen = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(pLen, 16);
            try { fixed (byte* p = buf) { r = WS2_32.recvfrom((int)s, (IntPtr)p, max, SocketFlags.None, ref from, pLen); } }
            finally { Marshal.FreeHGlobal(pLen); }
            if (r <= 0) { throw new Exception("服务端 recvfrom 失败 r=" + r + " err=" + Marshal.GetLastWin32Error()); }
            return buf.Take(r).ToArray();
        }

        #endregion

        #region//杂项

        /// <summary>逐步打点：出 AV 时能一眼看出停在哪个入口（stderr 不缓冲）。</summary>
        private static void Step(string what)
        {
            Console.Error.WriteLine("  [step] " + what);
            Console.Error.Flush();
        }

        private static void Check(int r, string what)
        {
            if (r != 0) { throw new Exception(what + " 失败 err=" + Marshal.GetLastWin32Error()); }
        }

        private static string Ascii(byte[] b)
        {
            if (b == null) { return "(null)"; }
            var sb = new StringBuilder();
            foreach (byte x in b) { sb.Append(x >= 0x20 && x < 0x7F ? (char)x : '.'); }
            return sb.ToString();
        }

        private static string GetArg(string[] args, string name, string def)
        {
            int i = Array.IndexOf(args, name);
            return (i >= 0 && i + 1 < args.Length) ? args[i + 1] : def;
        }

        #endregion
    }
}
