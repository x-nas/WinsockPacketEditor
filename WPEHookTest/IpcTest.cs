using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using WinsockPacketEditor;
using WinsockPacketEditor.Ipc;

namespace WPEHookTest
{
    /// <summary>
    /// IPC 的端到端跑测（验证矩阵第 1、2、5、6、7 项的自动化部分）。
    ///
    /// 一个 exe 两个角色：
    ///   <c>--target</c> 当靶子 —— 自己跟自己收发，等着被注入；
    ///   <c>--shell</c>  当外壳 —— 拉起靶子、注入无头核心、收封包、出报告。
    ///
    /// 【为什么必须是两个进程】阶段 0 那个跑测是进程内装钩子，验的是引擎；
    /// 这一个验的是<b>跨进程</b>那条链路：注入 → 管道 → 环 → 写线程 → 帧 →
    /// 外壳还原成 PacketInfo → cqPacketInfo。进程内测不出来。
    /// </summary>
    internal static class IpcTest
    {
        private const string LOOPBACK = "127.0.0.1";

        #region//靶子

        /// <summary>
        /// 靶子：起一个自收自发的回环，每 50ms 来一轮，直到被杀或到时。
        /// 它自己<b>不装任何钩子</b> —— 钩子由注入进来的无头核心装。
        /// </summary>
        public static int RunTarget(string[] args)
        {
            int seconds = int.Parse(GetArg(args, "--seconds", "30"));

            /*
                ⚠️ 必须显式加载这三个 DLL。
                EasyHook 的 GetProcAddress 要求模块<b>已经加载进本进程</b>；
                .NET 做 socket 会带起 ws2_32 与 mswsock，但<b>不会带起 wsock32</b>，
                于是 WS1 那四个钩子装不上 —— 而 StartHook 把异常吞进日志了，
                表现是「装上了却抓不到 WS1 的包」。

                真实的游戏里 wsock32 通常是它自己加载的（ProcessList 正是靠枚举
                目标模块来决定 Support_WS1/WS2/MsWS）。这里是人造靶子，得自己补。
            */
            Native.LoadLibrary("ws2_32.dll");
            Native.LoadLibrary("wsock32.dll");
            Native.LoadLibrary("mswsock.dll");

            IntPtr wsaData = Marshal.AllocHGlobal(408);
            try { Native.WSAStartup(0x0202, wsaData); }
            finally { Marshal.FreeHGlobal(wsaData); }

            //回环：服务端一条线程，客户端主线程
            IntPtr listen = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
            var bindAddr = Native.MakeAddr(LOOPBACK, 0);
            if (Native.bind(listen, ref bindAddr, 16) != 0) { Console.Error.WriteLine("靶子 bind 失败"); return 2; }
            Native.listen(listen, 4);

            var bound = new Operate.PacketConfig.Packet.SockAddr();
            int len = 16;
            Native.getsockname(listen, ref bound, ref len);
            ushort port = Native.ntohs(bound.sin_port);

            Console.WriteLine("TARGET_READY pid=" + Process.GetCurrentProcess().Id + " port=" + port);
            Console.Out.Flush();

            var server = new Thread(() => EchoServer(listen));
            server.IsBackground = true;
            server.Start();

            //客户端：连上去，反复发同一句话
            IntPtr sock = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
            var srv = Native.MakeAddr(LOOPBACK, port);
            if (Native.connect(sock, ref srv, 16) != 0) { Console.Error.WriteLine("靶子 connect 失败"); return 2; }

            int deadline = unchecked(Environment.TickCount + seconds * 1000);
            int round = 0;

            while (unchecked(deadline - Environment.TickCount) > 0)
            {
                round++;
                Beat(sock, "TARGET-BEAT-" + round.ToString("D5"));
                Thread.Sleep(50);
            }

            Native.closesocket(sock);
            Native.closesocket(listen);
            return 0;
        }

        private static unsafe void Beat(IntPtr s, string payload)
        {
            byte[] b = Encoding.ASCII.GetBytes(payload);
            fixed (byte* p = b) { WS2_32.send((int)s, (IntPtr)p, b.Length, SocketFlags.None); }

            byte[] buf = new byte[256];
            fixed (byte* p = buf) { WS2_32.recv((int)s, (IntPtr)p, buf.Length, SocketFlags.None); }
        }

        private static unsafe void EchoServer(IntPtr listen)
        {
            IntPtr conn = Native.accept(listen, IntPtr.Zero, IntPtr.Zero);

            while (true)
            {
                byte[] buf = new byte[512];
                int r;
                fixed (byte* p = buf) { r = WS2_32.recv((int)conn, (IntPtr)p, buf.Length, SocketFlags.None); }
                if (r <= 0) { return; }

                byte[] echo = Encoding.ASCII.GetBytes("R:" + Encoding.ASCII.GetString(buf, 0, r));
                fixed (byte* p = echo) { WS2_32.send((int)conn, (IntPtr)p, echo.Length, SocketFlags.None); }
            }
        }

        #endregion

        #region//外壳

        public static int RunShell(string[] args)
        {
            string outPath = GetArg(args, "--out", null);
            int collectSeconds = int.Parse(GetArg(args, "--collect", "6"));

            var report = new StringBuilder();
            report.AppendLine("# WPE 注入模式 IPC 端到端跑测");
            report.AppendLine();

            //外壳侧也要一份配置（GetIPLocation 要 qqwry，CountPacketInfo 要计数器）
            string dir = Path.Combine(Path.GetTempPath(), "WPEHookTest");
            Directory.CreateDirectory(dir);
            Operate.DataBase.dbPath = dir;
            Operate.DataBase.InitDB();
            Operate.SystemConfig.LoadSystemConfig_FromDB();
            Operate.SystemConfig.SpeedMode = false;

            Operate.PacketConfig.Packet.Support_WS1 = true;
            Operate.PacketConfig.Packet.Support_WS2 = true;
            Operate.PacketConfig.Packet.Support_MsWS = true;

            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out _)) { }

            //=== 拉起靶子 ===
            var psi = new ProcessStartInfo(
                Process.GetCurrentProcess().MainModule.FileName,
                "--target --seconds " + (collectSeconds + 20))
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            Process target = Process.Start(psi);
            string ready = target.StandardOutput.ReadLine();

            if (ready == null || !ready.StartsWith("TARGET_READY"))
            {
                report.AppendLine("靶子没起来: " + (ready ?? "(无输出)"));
                return Finish(report, outPath, false);
            }

            report.AppendLine("靶子已就绪: " + ready);
            Console.WriteLine("靶子已就绪: " + ready);

            //靶子的 stdout / stderr 要一直抽干，否则管道满了它会卡住
            new Thread(() => { try { target.StandardOutput.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();
            new Thread(() => { try { target.StandardError.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();

            var link = new ShellLink();
            var stateLog = new List<string>();
            link.StateChanged += s => { lock (stateLog) { stateLog.Add(s.ToString()); } Console.WriteLine("链路状态: " + s); };

            try
            {
                //=== 注入 + 握手 + 推快照 ===
                var sw = Stopwatch.StartNew();
                link.Attach(target.Id, null, 15000);
                sw.Stop();

                report.AppendLine();
                report.AppendLine("- 注入到握手完成: " + sw.ElapsedMilliseconds + " ms");
                report.AppendLine("- 目标 PID: " + link.TargetPid + "（拉起的是 " + target.Id + "）");
                report.AppendLine("- 目标位数: " + (link.TargetIs64 ? "64" : "32"));
                report.AppendLine("- 链路状态: " + link.State);

                //=== 开始拦截 ===
                link.StartHook();
                Console.WriteLine("已下发 StartHook，收 " + collectSeconds + " 秒");
                Thread.Sleep(collectSeconds * 1000);

                var captured = new List<PacketInfo>();
                while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out PacketInfo pi)) { captured.Add(pi); }

                report.AppendLine("- 丢包计数: " + link.Dropped);
                report.AppendLine("- 目标侧钩子: " + (link.HookInstalled ? "已装" : "未装"));
                report.AppendLine();

                report.AppendLine("## 收到的封包（" + captured.Count + " 条，取前 8 条看内容）");
                report.AppendLine();
                report.AppendLine("| # | 类型 | 长度 | 本机 | 远端 | 内容 |");
                report.AppendLine("|---|------|------|------|------|------|");

                foreach (var pi in captured.Take(8))
                {
                    report.AppendLine(string.Format("| {0} | {1} | {2} | {3} | {4} | {5} |",
                        pi.Id, pi.PacketType, pi.PacketLen, pi.PacketFrom, pi.PacketTo, Ascii(pi.PacketBuffer)));
                }

                report.AppendLine();
                report.AppendLine("## 按类型统计");
                report.AppendLine();
                foreach (var g in captured.GroupBy(p => p.PacketType).OrderBy(g => g.Key.ToString(), StringComparer.Ordinal))
                {
                    report.AppendLine("- " + g.Key + ": " + g.Count() + " 条");
                }

                //=== 停止拦截 + 断开 ===
                link.StopHook();
                report.AppendLine();
                report.AppendLine("- StopHook 之后链路状态: " + link.State);

                //=== 目标崩溃时数据要保留（验证矩阵第 6 项）===
                int before = captured.Count;
                target.Kill();
                target.WaitForExit(5000);
                Thread.Sleep(1500);

                report.AppendLine("- 杀掉靶子之后链路状态: " + link.State + "（期望 Disconnected）");
                report.AppendLine("- 已抓的 " + before + " 条数据仍在外壳手里（本来就没清过）");

                bool ok =
                    captured.Count > 0 &&
                    captured.Any(p => Ascii(p.PacketBuffer).Contains("TARGET-BEAT")) &&
                    link.State == ShellLink.LinkState.Disconnected;

                report.AppendLine();
                report.AppendLine("判定：");
                report.AppendLine("  · 收到封包 > 0 ............ " + (captured.Count > 0 ? "ok" : "FAIL"));
                report.AppendLine("  · 内容与靶子发的对得上 .... " + (captured.Any(p => Ascii(p.PacketBuffer).Contains("TARGET-BEAT")) ? "ok" : "FAIL"));
                report.AppendLine("  · 靶子没了 → Disconnected . " + (link.State == ShellLink.LinkState.Disconnected ? "ok" : "FAIL"));

                return Finish(report, outPath, ok);
            }
            catch (Exception ex)
            {
                report.AppendLine();
                report.AppendLine("出错: " + ex);
                try { target.Kill(); } catch { }
                return Finish(report, outPath, false);
            }
            finally
            {
                try { link.Dispose(); } catch { }
                try { if (!target.HasExited) { target.Kill(); } } catch { }
            }
        }

        #endregion

        #region//杂项

        private static int Finish(StringBuilder report, string outPath, bool ok)
        {
            report.AppendLine();
            report.AppendLine(ok ? "结论: PASS" : "结论: FAIL");

            string text = report.ToString();
            Console.WriteLine(text);

            if (!string.IsNullOrEmpty(outPath))
            {
                File.WriteAllText(outPath, text, new UTF8Encoding(false));
                Console.WriteLine("报告已写入 " + outPath);
            }

            return ok ? 0 : 1;
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
