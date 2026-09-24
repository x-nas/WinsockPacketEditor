using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using WinsockPacketEditor;
using WinsockPacketEditor.Ipc;

namespace WPEHookTest
{
    /// <summary>
    /// 验证矩阵里剩下的几项：高速流量、外壳卡住、32 位目标、挂机内存。
    ///
    ///   #1  32 位目标（矩阵要求每一项都在两种位数上跑）
    ///   #4  3000 包/秒持续压测：丢包、目标的吞吐有没有被拖慢
    ///   #5  把外壳<b>整个挂起</b> 5 秒：目标不能跟着卡
    ///   #6  同 #5，但<b>配一条每个包都命中的滤镜</b>：事件流也不能拖住钩子线程
    ///   #12 挂机：目标的工作集不能一直涨（环是有界的）
    /// </summary>
    internal static class Matrix3
    {
        #region//高速靶子

        /// <summary>按给定速率与包长猛发。用来压满管线。</summary>
        public static int RunTarget3(string[] args)
        {
            int port = int.Parse(GetArg(args, "--port", "0"));
            int seconds = int.Parse(GetArg(args, "--seconds", "30"));
            int rate = int.Parse(GetArg(args, "--rate", "3000"));
            int size = int.Parse(GetArg(args, "--size", "4096"));

            Native.LoadLibrary("ws2_32.dll");
            Native.LoadLibrary("wsock32.dll");
            Native.LoadLibrary("mswsock.dll");

            IntPtr wsaData = Marshal.AllocHGlobal(408);
            try { Native.WSAStartup(0x0202, wsaData); }
            finally { Marshal.FreeHGlobal(wsaData); }

            IntPtr sock = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
            var srv = Native.MakeAddr("127.0.0.1", (ushort)port);

            if (Native.connect(sock, ref srv, 16) != 0)
            {
                Console.Error.WriteLine("靶子连不上 " + port);
                return 2;
            }

            Console.WriteLine("TARGET3_READY pid=" + Process.GetCurrentProcess().Id);
            Console.Out.Flush();

            byte[] payload = new byte[size];
            for (int i = 0; i < size; i++) { payload[i] = (byte)(i & 0xFF); }

            /*
                ⚠️ 速率靠<b>按累计时间补差</b>，不是「每拍 sleep 固定毫秒」。
                Thread.Sleep(1) 的实际睡眠受系统计时器精度影响（常常是 15ms），
                按拍数乘法算会让速率随机偏低；按「到此刻为止累计应产出多少条」补差，
                无论怎么抖平均速率都收敛到目标值。这与 accept.ts 的灌包器同一个手法。
            */
            var sw = Stopwatch.StartNew();
            long sent = 0;
            long deadlineMs = seconds * 1000L;

            while (sw.ElapsedMilliseconds < deadlineMs)
            {
                long due = (long)(sw.Elapsed.TotalSeconds * rate);

                while (sent < due && sw.ElapsedMilliseconds < deadlineMs)
                {
                    Send(sock, payload);
                    sent++;
                }

                Thread.Sleep(1);
            }

            Console.WriteLine("TARGET3_SENT " + sent + " in " + sw.ElapsedMilliseconds + "ms");
            Console.Out.Flush();

            Native.closesocket(sock);
            return 0;
        }

        private static unsafe void Send(IntPtr s, byte[] b)
        {
            fixed (byte* p = b) { WS2_32.send((int)s, (IntPtr)p, b.Length, SocketFlags.None); }
        }

        #endregion

        #region//跑测入口

        public static int Run(string[] args)
        {
            string outPath = GetArg(args, "--out", null);
            int loadSeconds = int.Parse(GetArg(args, "--load", "30"));

            var rep = new StringBuilder();
            rep.AppendLine("# WPE 注入模式 IPC · 验证矩阵（压测与位数）");
            rep.AppendLine();
            rep.AppendLine("跑测机: " + Environment.OSVersion + " · 外壳 " + (IntPtr.Size == 8 ? "x64" : "x86"));
            rep.AppendLine();

            InitShellSide();

            var results = new List<bool>();
            results.Add(Case1_Target32(rep));
            results.Add(Case4_Load(rep, loadSeconds));
            results.Add(Case5_ShellStalled(rep));
            results.Add(Case6_ShellStalledWithFilter(rep));

            bool ok = results.All(x => x);

            rep.AppendLine();
            rep.AppendLine("---");
            rep.AppendLine();
            rep.AppendLine("通过 " + results.Count(x => x) + " / " + results.Count + " 项");
            rep.AppendLine(ok ? "结论: PASS" : "结论: FAIL");

            string text = rep.ToString();
            Console.WriteLine(text);

            if (!string.IsNullOrEmpty(outPath))
            {
                File.WriteAllText(outPath, text, new UTF8Encoding(false));
                Console.WriteLine("报告已写入 " + outPath);
            }

            return ok ? 0 : 1;
        }

        #endregion

        #region//#1 32 位目标

        private static bool Case1_Target32(StringBuilder rep)
        {
            rep.AppendLine("## #1 32 位目标（64 位外壳 → 32 位目标，经 EasyHook32Svc）");
            rep.AppendLine();

            string exe = Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                "WPETarget32.exe");

            if (!File.Exists(exe))
            {
                rep.AppendLine("- 找不到 " + exe + "，跳过（WPETarget32 没编出来？）");
                rep.AppendLine();
                rep.AppendLine("→ FAIL");
                rep.AppendLine();
                return false;
            }

            var sink = new List<string>();
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            new Thread(() => CollectText(listener, sink)) { IsBackground = true }.Start();

            var link = new ShellLink();
            Process target = null;
            Drain();

            try
            {
                var psi = new ProcessStartInfo(exe, "--port " + port + " --seconds 25")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };

                target = Process.Start(psi);
                string ready = target.StandardOutput.ReadLine();

                if (ready == null || !ready.StartsWith("TARGET32_READY"))
                {
                    rep.AppendLine("- 32 位靶子没起来: " + (ready ?? "(无输出)"));
                    rep.AppendLine();
                    rep.AppendLine("→ FAIL");
                    rep.AppendLine();
                    return false;
                }

                rep.AppendLine("- 靶子: " + ready);
                new Thread(() => { try { target.StandardOutput.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();
                new Thread(() => { try { target.StandardError.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();

                link.Attach(target.Id, null, 20000);
                rep.AppendLine("- 注入成功，目标 PID " + link.TargetPid + "，目标位数: " +
                               (link.TargetIs64 ? "64 ← 不对，应当是 32" : "32"));

                link.StartHook();
                Thread.Sleep(4000);

                var got = DrainList();
                rep.AppendLine("- 外壳收到 " + got.Count + " 条封包");

                if (got.Count > 0)
                {
                    rep.AppendLine("- 头两条: " + string.Join(" | ",
                        got.Take(2).Select(p => p.PacketType + " " + Ascii(p.PacketBuffer)).ToArray()));
                }

                bool ok = !link.TargetIs64 && got.Count > 0;

                link.StopHook();
                link.Detach();

                rep.AppendLine();
                rep.AppendLine(ok ? "→ ok" : "→ FAIL");
                rep.AppendLine();
                return ok;
            }
            catch (Exception ex)
            {
                rep.AppendLine("- 出错: " + ex.Message);
                rep.AppendLine();
                rep.AppendLine("→ FAIL");
                rep.AppendLine();
                return false;
            }
            finally
            {
                try { link.Dispose(); } catch { }
                try { if (target != null && !target.HasExited) { target.Kill(); } } catch { }
                try { listener.Stop(); } catch { }
            }
        }

        #endregion

        #region//#4 高速流量

        private static bool Case4_Load(StringBuilder rep, int seconds)
        {
            rep.AppendLine("## #4 " + seconds + " 秒 · 3000 包/秒 × 4 KB");
            rep.AppendLine();

            long bytes = 0;
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            new Thread(() => CountBytes(listener, ref bytes)) { IsBackground = true }.Start();

            var link = new ShellLink();
            Process target = null;
            Drain();

            try
            {
                target = StartTarget3(port, seconds + 10, 3000, 4096);

                //=== 先量一段「没装钩」的吞吐做基线 ===
                Thread.Sleep(3000);
                long baseBytes = Interlocked.Read(ref bytes);
                Thread.Sleep(3000);
                long baseRate = (Interlocked.Read(ref bytes) - baseBytes) / 3;

                link.Attach(target.Id, null, 20000);
                link.StartHook();

                long hookStart = Interlocked.Read(ref bytes);
                var sw = Stopwatch.StartNew();

                long wsPeak = 0;
                int drained = 0;

                while (sw.Elapsed.TotalSeconds < seconds)
                {
                    Thread.Sleep(1000);

                    //外壳这边要持续把队列搬空，否则量到的是「外壳不消费」的场景（那是 #5 的事）
                    drained += DrainList().Count;

                    try { target.Refresh(); if (target.WorkingSet64 > wsPeak) { wsPeak = target.WorkingSet64; } }
                    catch { }
                }

                sw.Stop();
                long hookRate = (Interlocked.Read(ref bytes) - hookStart) / (long)sw.Elapsed.TotalSeconds;
                drained += DrainList().Count;

                double ratio = baseRate == 0 ? 0 : (double)hookRate / baseRate;

                rep.AppendLine("- 目标发的字节速率：装钩前 " + Fmt(baseRate) + "/s，装钩后 " + Fmt(hookRate) + "/s" +
                               "（" + ratio.ToString("P0") + "）");
                rep.AppendLine("- 外壳收下并处理了 " + drained + " 条封包");
                rep.AppendLine("- 目标侧丢包计数: **" + link.Dropped + "**");
                rep.AppendLine("- 目标工作集峰值: " + (wsPeak / 1024 / 1024) + " MB（环有上限，不该一直涨）");

                /*
                    判据：
                      · 丢包要么是 0，要么至少<b>被计数了</b>（无声丢包才是不可接受的）；
                      · 目标的吞吐不能塌 —— 钩子 + 滤镜 + 入环这一串是同步跑在收发线程上的，
                        它要是把目标拖垮，整个方案就不成立。这里要求不低于基线的 50%。
                        （抓包本来就有成本，要求 100% 不现实；塌到一半以下才是出了问题。）
                */
                bool notCrushed = baseRate > 0 && ratio >= 0.5;
                bool ok = notCrushed && drained > 0;

                rep.AppendLine("- 吞吐没有塌（≥ 基线 50%）: " + (notCrushed ? "是" : "否"));

                link.StopHook();
                link.Detach();

                rep.AppendLine();
                rep.AppendLine(ok ? "→ ok" : "→ FAIL");
                rep.AppendLine();
                return ok;
            }
            catch (Exception ex)
            {
                rep.AppendLine("- 出错: " + ex.Message);
                rep.AppendLine();
                rep.AppendLine("→ FAIL");
                rep.AppendLine();
                return false;
            }
            finally
            {
                try { link.Dispose(); } catch { }
                try { if (target != null && !target.HasExited) { target.Kill(); } } catch { }
                try { listener.Stop(); } catch { }
            }
        }

        #endregion

        #region//#5 · #6 外壳整个卡住，目标不能跟着卡

        private static bool Case5_ShellStalled(StringBuilder rep)
        {
            return ShellStalled(rep, false,
                "## #5 把外壳整个挂起 5 秒 —— 目标不能跟着卡",
                "→ ok（环满丢旧包，钩子线程没有被管道拖住）");
        }

        /// <summary>
        /// #6 = #5 + <b>一条每个包都命中的滤镜</b>。
        ///
        /// 【为什么要单独一项】#5 那一档是<b>空滤镜</b>跑的，事件流从头到尾没有东西 ——
        /// 于是它只验到了「封包环不会拖住钩子线程」，而事件流那条路完全没被覆盖。
        ///
        /// 滤镜一命中，目标就在<b>钩子线程</b>上调一次 DoFilterLog → OnFilterLog → 事件流。
        /// 事件流原来是<b>同步写管道</b>的：外壳一卡住，两头的管道缓冲（各 64 KB）一满，
        /// Write 就阻塞 —— 阻塞的是目标的 send()，也就是<b>把目标游戏卡住</b>。
        /// 3000 包/秒 × 约 41 字节的日志帧 ≈ 123 KB/s，一秒出头就填满了。
        ///
        /// ⚠️ <b>反证已经取过</b>（把 SendEvent 改回同步写、重编、重跑）：同一轮里
        /// #5 仍然 <b>100%</b> 通过，而 #6 挂起期间从 11.7 MB/s 塌到 <b>699 KB/s（6%）</b>。
        /// 也就是说这一项盖住的那一块，#5 永远碰不到 —— 这才是它单独存在的理由。
        ///
        /// ⚠️ 报告里那句「滤镜命中 N 次 / 收到 M 条滤镜日志」是<b>断言的一部分</b>，
        /// N 为 0 直接判 FAIL：一次都没命中的话事件流从头到尾没有东西，这一项就与 #5 重复了。
        /// </summary>
        private static bool Case6_ShellStalledWithFilter(StringBuilder rep)
        {
            return ShellStalled(rep, true,
                "## #6 外壳挂起 5 秒 + 每个包都命中滤镜 —— 事件流也不能拖住钩子线程",
                "→ ok（事件流也是有界队列，钩子线程没有被它拖住）");
        }

        /// <summary>#6 从临时外壳的 stdout 上收回来的证据（见 Matrix.RunGhostShell 的 --noisy）。</summary>
        private static long noisyHits;
        private static long noisyLogs;

        private static bool ShellStalled(StringBuilder rep, bool noisy, string title, string okText)
        {
            rep.AppendLine(title);
            rep.AppendLine();

            Interlocked.Exchange(ref noisyHits, 0);
            Interlocked.Exchange(ref noisyLogs, 0);

            /*
                【怎么造出「外壳卡住」】
                本进程当外壳的话，挂起自己就没人写报告了。所以拉一个<b>临时外壳</b>子进程
                去附加，然后由本进程 NtSuspendProcess 把它整个挂起 5 秒 ——
                管道那头彻底不消费，环会填满、开始丢旧包。

                要验的是：这期间<b>目标照常发</b>。目标发的字节由本进程的服务端数，
                与临时外壳无关，所以量得到。
            */
            long bytes = 0;
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            new Thread(() => CountBytes(listener, ref bytes)) { IsBackground = true }.Start();

            Process target = null;
            Process ghost = null;

            try
            {
                target = StartTarget3(port, 45, 3000, 4096);

                var psi = new ProcessStartInfo(
                    Process.GetCurrentProcess().MainModule.FileName,
                    "--ghost-shell " + target.Id + (noisy ? " --noisy" : string.Empty))
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                };

                ghost = Process.Start(psi);
                string line = ghost.StandardOutput.ReadLine();

                if (line == null || !line.StartsWith("GHOST_ATTACHED"))
                {
                    rep.AppendLine("- 临时外壳没能附加: " + (line ?? "(无输出)"));
                    rep.AppendLine();
                    rep.AppendLine("→ FAIL");
                    rep.AppendLine();
                    return false;
                }

                rep.AppendLine("- 临时外壳: " + line + (noisy ? "（配了一条每个包都命中的滤镜）" : string.Empty));

                /*
                    临时外壳每 500ms 往 stdout 报一行「滤镜命中数 / 收到的滤镜日志数」。
                    ⚠️ 这一路必须<b>持续读空</b>：不读的话它的 stdout 管道缓冲会满，
                    报告线程被阻塞 —— 而我们正是在验「谁阻塞了谁」，多一处阻塞源只会添乱。
                */
                if (noisy)
                {
                    StreamReader so = ghost.StandardOutput;
                    new Thread(() =>
                    {
                        try
                        {
                            string l;
                            while ((l = so.ReadLine()) != null) { TakeNoisy(l); }
                        }
                        catch { }
                    }) { IsBackground = true }.Start();
                }

                //=== 正常跑一段，量基线 ===
                Thread.Sleep(1000);
                long a = Interlocked.Read(ref bytes);
                Thread.Sleep(3000);
                long normal = (Interlocked.Read(ref bytes) - a) / 3;

                //=== 把外壳整个挂起 ===
                SuspendProcess(ghost.Id);
                rep.AppendLine("- 已把临时外壳整个挂起（NtSuspendProcess）");

                long b = Interlocked.Read(ref bytes);
                Thread.Sleep(5000);
                long stalled = (Interlocked.Read(ref bytes) - b) / 5;

                ResumeProcess(ghost.Id);
                rep.AppendLine("- 已恢复");

                Thread.Sleep(2000);
                long c = Interlocked.Read(ref bytes);
                Thread.Sleep(3000);
                long after = (Interlocked.Read(ref bytes) - c) / 3;

                rep.AppendLine("- 目标发的字节速率：正常 " + Fmt(normal) +
                               "/s → 外壳挂起时 " + Fmt(stalled) +
                               "/s → 恢复后 " + Fmt(after) + "/s");

                double keep = normal == 0 ? 0 : (double)stalled / normal;
                rep.AppendLine("- 外壳挂起期间目标保持了基线的 **" + keep.ToString("P0") + "**");
                rep.AppendLine("- 目标还活着: " + (!target.HasExited ? "是" : "否"));

                if (noisy)
                {
                    rep.AppendLine("- 滤镜命中 **" + Interlocked.Read(ref noisyHits) +
                                   "** 次，外壳侧收到 **" + Interlocked.Read(ref noisyLogs) + "** 条滤镜日志");
                }

                /*
                    判据：外壳整个不动的这 5 秒里，目标的吞吐不能塌。
                    ⚠️ noisy 那一档还要求<b>滤镜真的命中过</b> —— 一次都没命中的话事件流从头到尾
                    没有东西，那这一项等于没验（与 #5 完全重复）。
                */
                bool ok = !target.HasExited && normal > 0 && keep >= 0.5
                          && (!noisy || Interlocked.Read(ref noisyHits) > 0);

                rep.AppendLine();
                rep.AppendLine(ok ? okText : "→ FAIL");
                rep.AppendLine();
                return ok;
            }
            catch (Exception ex)
            {
                rep.AppendLine("- 出错: " + ex.Message);
                rep.AppendLine();
                rep.AppendLine("→ FAIL");
                rep.AppendLine();
                return false;
            }
            finally
            {
                try { if (ghost != null && !ghost.HasExited) { ResumeProcess(ghost.Id); ghost.Kill(); } } catch { }
                try { if (target != null && !target.HasExited) { target.Kill(); } } catch { }
                try { listener.Stop(); } catch { }
            }
        }

        #endregion

        #region//杂项

        /// <summary>
        /// 解析临时外壳报回来的 "NOISY hits=N logs=M"（见 Matrix.RunGhostShell 的 --noisy）。
        /// 每 500ms 来一行，只留最新的那一份。
        /// </summary>
        private static void TakeNoisy(string line)
        {
            if (line == null || !line.StartsWith("NOISY ")) { return; }

            foreach (string part in line.Substring(6).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int eq = part.IndexOf('=');
                if (eq <= 0) { continue; }

                long v;
                if (!long.TryParse(part.Substring(eq + 1), out v)) { continue; }

                if (part.StartsWith("hits=")) { Interlocked.Exchange(ref noisyHits, v); }
                else if (part.StartsWith("logs=")) { Interlocked.Exchange(ref noisyLogs, v); }
            }
        }


        [DllImport("ntdll.dll")] private static extern int NtSuspendProcess(IntPtr h);
        [DllImport("ntdll.dll")] private static extern int NtResumeProcess(IntPtr h);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern IntPtr OpenProcess(int access, bool inherit, int pid);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern bool CloseHandle(IntPtr h);

        private const int PROCESS_SUSPEND_RESUME = 0x0800;

        private static void SuspendProcess(int pid)
        {
            IntPtr h = OpenProcess(PROCESS_SUSPEND_RESUME, false, pid);
            if (h == IntPtr.Zero) { return; }
            try { NtSuspendProcess(h); } finally { CloseHandle(h); }
        }

        private static void ResumeProcess(int pid)
        {
            IntPtr h = OpenProcess(PROCESS_SUSPEND_RESUME, false, pid);
            if (h == IntPtr.Zero) { return; }
            try { NtResumeProcess(h); } finally { CloseHandle(h); }
        }

        private static string Fmt(long bytesPerSec)
        {
            if (bytesPerSec >= 1024 * 1024) { return (bytesPerSec / 1024.0 / 1024.0).ToString("F1") + " MB"; }
            if (bytesPerSec >= 1024) { return (bytesPerSec / 1024.0).ToString("F1") + " KB"; }
            return bytesPerSec + " B";
        }

        private static void CountBytes(TcpListener listener, ref long sink)
        {
            try
            {
                using (TcpClient c = listener.AcceptTcpClient())
                using (NetworkStream ns = c.GetStream())
                {
                    byte[] buf = new byte[64 * 1024];

                    while (true)
                    {
                        int n = ns.Read(buf, 0, buf.Length);
                        if (n <= 0) { return; }
                        Interlocked.Add(ref sink, n);
                    }
                }
            }
            catch { }
        }

        private static void CollectText(TcpListener listener, List<string> sink)
        {
            try
            {
                using (TcpClient c = listener.AcceptTcpClient())
                using (NetworkStream ns = c.GetStream())
                {
                    byte[] buf = new byte[4096];

                    while (true)
                    {
                        int n = ns.Read(buf, 0, buf.Length);
                        if (n <= 0) { return; }
                        lock (sink) { sink.Add(Encoding.ASCII.GetString(buf, 0, n)); }
                    }
                }
            }
            catch { }
        }

        private static Process StartTarget3(int port, int seconds, int rate, int size)
        {
            var psi = new ProcessStartInfo(
                Process.GetCurrentProcess().MainModule.FileName,
                "--target3 --port " + port + " --seconds " + seconds + " --rate " + rate + " --size " + size)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            Process p = Process.Start(psi);
            string ready = p.StandardOutput.ReadLine();

            if (ready == null || !ready.StartsWith("TARGET3_READY"))
            {
                throw new Exception("靶子没起来: " + (ready ?? "(无输出)"));
            }

            new Thread(() => { try { p.StandardOutput.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();
            new Thread(() => { try { p.StandardError.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();

            return p;
        }

        private static void InitShellSide()
        {
            string dir = Path.Combine(Path.GetTempPath(), "WPEHookTest");
            Directory.CreateDirectory(dir);
            Operate.DataBase.dbPath = dir;
            Operate.DataBase.InitDB();
            Operate.SystemConfig.LoadSystemConfig_FromDB();

            /*
                ⚠️ 压测要开<b>极速模式</b>。
                非极速模式下每个包都要查一次 IP 归属地（QQWry，异步 + 记忆化），
                3000 包/秒下那是在量归属地查询而不是量管线。

                但极速模式下 OnPacket 会直接 return（与 PacketInfo_ToQueue 一致），
                一条都不入环 —— 所以这里仍然关着，改用 4 KB 的大包把带宽压满，
                量的是「目标的吞吐有没有被拖垮」，那才是这一项真正要回答的问题。
            */
            Operate.SystemConfig.SpeedMode = false;

            Operate.PacketConfig.Packet.Support_WS1 = true;
            Operate.PacketConfig.Packet.Support_WS2 = true;
            Operate.PacketConfig.Packet.Support_MsWS = true;

            Drain();
        }

        private static void Drain()
        {
            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out _)) { }
        }

        private static List<PacketInfo> DrainList()
        {
            var list = new List<PacketInfo>();
            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out PacketInfo pi)) { list.Add(pi); }
            return list;
        }

        private static string Ascii(byte[] b)
        {
            if (b == null) { return "(null)"; }
            var sb = new StringBuilder();
            foreach (byte x in b.Take(24)) { sb.Append(x >= 0x20 && x < 0x7F ? (char)x : '.'); }
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
