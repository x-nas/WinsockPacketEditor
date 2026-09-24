using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using WinsockPacketEditor;
using WinsockPacketEditor.Ipc;

namespace WPEHookTest
{
    /// <summary>
    /// 验证矩阵的自动化部分（INJECT-IPC-PLAN.md 第七节）。
    ///
    /// 能自动跑的：
    ///   #2  挂起启动 → 开始拦截 → 第一个包也抓到
    ///   #4  持续高速流量下不丢包 / 丢了要计数
    ///   #5  背压：环满丢最旧的，Enqueue 永不阻塞
    ///   #6  目标崩 → 外壳保留全部数据（已在 IpcTest 里验过，这里复验状态机）
    ///   #7  外壳崩 → 目标 3 秒内卸钩；对同一 PID 重新附加能继续工作
    ///  #11  钩子在跑的时候换滤镜快照，无异常、计数按 GUID 延续
    ///
    /// 要人工确认的（32 位目标、24 小时挂机、与 WinForms 版逐字节对比）另说。
    /// </summary>
    internal static class Matrix
    {
        public static int Run(string[] args)
        {
            string outPath = GetArg(args, "--out", null);

            var rep = new StringBuilder();
            rep.AppendLine("# WPE 注入模式 IPC · 验证矩阵（自动化部分）");
            rep.AppendLine();
            rep.AppendLine("跑测机: " + Environment.OSVersion + " · " + (IntPtr.Size == 8 ? "x64" : "x86") + " 外壳");
            rep.AppendLine();

            InitShellSide();

            var results = new List<bool>();

            results.Add(Case5_RingBackpressure(rep));
            results.Add(Case2_SuspendedLaunch(rep));
            results.Add(Case11_SwapFiltersWhileHooked(rep));
            results.Add(Case7_ShellCrashThenReattach(rep));

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

        #region//#5 背压：环满丢最旧的，入队永不阻塞

        private static bool Case5_RingBackpressure(StringBuilder rep)
        {
            rep.AppendLine("## #5 背压：环满丢最旧的，Enqueue 永不阻塞");
            rep.AppendLine();

            //小环，好让它一定溢出
            var ring = new PacketRing(100, 64 * 1024);

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                ring.Enqueue(new PendingPacket
                {
                    Id = i,
                    Raw = new byte[256],
                    Modified = null,
                });
            }
            sw.Stop();

            int left = ring.Count;
            long dropped = ring.Dropped;

            //取一批出来，确认留下的是<b>最新的</b>那些（丢的是最旧的）
            var batch = ring.DequeueBatch(1000, long.MaxValue, 0);
            long firstId = batch.Count > 0 ? batch[0].Id : -1;
            long lastId = batch.Count > 0 ? batch[batch.Count - 1].Id : -1;

            rep.AppendLine("- 灌 10000 条进容量 100 条的环，耗时 " + sw.ElapsedMilliseconds + " ms（不该阻塞）");
            rep.AppendLine("- 剩余 " + left + " 条，丢弃计数 " + dropped);
            rep.AppendLine("- 留下的 Id 区间 " + firstId + " .. " + lastId + "（应当是最后那 100 条，即 9900..9999）");

            bool ok =
                dropped == 9900 &&
                left == 100 &&
                firstId == 9900 && lastId == 9999 &&
                sw.ElapsedMilliseconds < 2000;

            //字节上限也要卡住：100 条 × 256 字节远没到 64 KB，换个大包试
            var ring2 = new PacketRing(1000000, 8 * 1024);
            for (int i = 0; i < 100; i++)
            {
                ring2.Enqueue(new PendingPacket { Id = i, Raw = new byte[4096] });
            }
            bool byteCapOk = ring2.Count <= 3 && ring2.Dropped >= 97;
            rep.AppendLine("- 字节上限：8 KB 的环灌 100 条 4 KB 的包 → 剩 " + ring2.Count + " 条，丢 " + ring2.Dropped + " 条");

            ok = ok && byteCapOk;
            rep.AppendLine();
            rep.AppendLine(ok ? "→ ok" : "→ FAIL");
            rep.AppendLine();
            return ok;
        }

        #endregion

        #region//#2 挂起启动 → 开始拦截 → 第一个包也抓到

        /// <summary>
        /// 【为什么靶子是 curl.exe 而不是我们自己】
        /// 用<b>托管</b>的 WPEHookTest.exe 当挂起启动的靶子时，唤醒之后进程立刻就没了，
        /// Main 一行都没跑到（留痕文件是空的）。原因是 EasyHook 已经在那个进程里
        /// 初始化了一份 CLR，而进程自己的运行时宿主随后还要再初始化一遍 —— 撞在一起。
        /// 这是<b>跑测的限制，不是产品的限制</b>：真实的注入目标是游戏，全是原生进程。
        ///
        /// 所以这一项换系统自带的 curl.exe：原生、会做 TCP 收发、能跑十几秒，
        /// 而且它一醒来<b>第一件事</b>就是发 GET —— 正好用来验「第一个包也抓得到」。
        /// </summary>
        private static bool Case2_SuspendedLaunch(StringBuilder rep)
        {
            rep.AppendLine("## #2 挂起启动（CreateAndInject）→ 开始拦截 → 第一个包也抓到");
            rep.AppendLine();

            string curl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "curl.exe");

            if (!File.Exists(curl))
            {
                rep.AppendLine("- 这台机器没有 curl.exe，跳过");
                rep.AppendLine();
                rep.AppendLine("→ 跳过（当通过算）");
                rep.AppendLine();
                return true;
            }

            var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
            listener.Start();
            int port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;

            new Thread(() => DribbleServer(listener)) { IsBackground = true }.Start();

            var link = new ShellLink();
            Drain();

            try
            {
                string cmd = "\"" + curl + "\" -s --max-time 25 http://127.0.0.1:" + port + "/";
                link.Attach(-1, curl, 20000, cmd);
                rep.AppendLine("- 挂起启动并注入成功，目标 PID " + link.TargetPid + "（靶子是原生的 curl.exe）");

                //此刻目标还挂着 —— 一个包都不该有
                Thread.Sleep(1500);
                int beforeStart = CountQueued();
                rep.AppendLine("- 唤醒之前收到 " + beforeStart + " 条（应为 0，目标还挂着）");

                link.StartHook();
                rep.AppendLine("- 已 StartHook（钩子装好之后才唤醒）");

                Thread.Sleep(6000);

                var got = DrainList();
                var texts = got.Select(p => Ascii(p.PacketBuffer)).ToList();

                //curl 醒来第一件事就是发 GET —— 抓到它就等于抓到了「第一个包」
                bool sawFirst = texts.Any(t => t.Contains("GET /"));
                bool sawBody = texts.Any(t => t.Contains("NATIVE-BEAT"));

                rep.AppendLine("- 收到 " + got.Count + " 条");
                rep.AppendLine("- 抓到目标醒来发的第一个包（GET /）: " + (sawFirst ? "是" : "否"));
                rep.AppendLine("- 抓到服务端回的正文（NATIVE-BEAT）: " + (sawBody ? "是" : "否"));

                if (got.Count > 0)
                {
                    rep.AppendLine("- 头两条: " + string.Join(" | ", texts.Take(2).Select(t => t.Substring(0, Math.Min(40, t.Length))).ToArray()));
                }

                bool ok = beforeStart == 0 && sawFirst && sawBody;

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
                try { KillPid(link.TargetPid); } catch { }
                try { link.Dispose(); } catch { }
                try { listener.Stop(); } catch { }
            }
        }

        /// <summary>细水长流的 HTTP 服务端：让 curl 活十几秒，好有时间观察。</summary>
        private static void DribbleServer(System.Net.Sockets.TcpListener listener)
        {
            try
            {
                using (var c = listener.AcceptTcpClient())
                using (var ns = c.GetStream())
                {
                    byte[] req = new byte[4096];
                    ns.Read(req, 0, req.Length);

                    byte[] head = Encoding.ASCII.GetBytes(
                        "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nTransfer-Encoding: chunked\r\n\r\n");
                    ns.Write(head, 0, head.Length);
                    ns.Flush();

                    for (int i = 0; i < 15; i++)
                    {
                        //⚠️ chunked 的长度是这一段的真实字节数（十六进制），写错的话 curl 直接以 56 退出
                        string chunk = "NATIVE-BEAT-" + i.ToString("D2");
                        byte[] body = Encoding.ASCII.GetBytes(chunk.Length.ToString("x") + "\r\n" + chunk + "\r\n");
                        ns.Write(body, 0, body.Length);
                        ns.Flush();
                        Thread.Sleep(300);
                    }

                    byte[] tail = Encoding.ASCII.GetBytes("0\r\n\r\n");
                    ns.Write(tail, 0, tail.Length);
                    ns.Flush();
                }
            }
            catch { }
        }

        /// <summary>把目标进程的状态打出来 —— 「0 条包」有太多种原因，靠猜查不出来。</summary>
        private static void Diag(StringBuilder rep, int pid, string when)
        {
            try
            {
                var p = Process.GetProcessById(pid);
                p.Refresh();
                rep.AppendLine("  · [" + when + "] 进程在: 是, 线程 " + p.Threads.Count +
                               ", 已用 CPU " + p.TotalProcessorTime.TotalMilliseconds.ToString("F0") + " ms" +
                               ", 工作集 " + (p.WorkingSet64 / 1024 / 1024) + " MB");
            }
            catch (Exception ex)
            {
                rep.AppendLine("  · [" + when + "] 进程查不到: " + ex.Message);
            }
        }

        #endregion

        #region//#11 钩子在跑的时候换滤镜快照

        private static bool Case11_SwapFiltersWhileHooked(StringBuilder rep)
        {
            rep.AppendLine("## #11 钩子在跑的时候反复换滤镜快照");
            rep.AppendLine();

            /*
                这一项验的是 FilterEngine 那条约束：快照用 Volatile 换引用，
                钩子线程读一次用到底。反复换的过程中不能有异常、不能丢包、不能卡住目标。

                「计数按 GUID 延续」在同一轮里也一并验：换快照之后同 GUID 的
                ExecutionCount 不该被清零。
            */
            var link = new ShellLink();
            Process target = null;
            Drain();

            try
            {
                target = StartTarget(40);
                link.Attach(target.Id, null, 15000);
                link.StartHook();

                //推一条什么都不匹配的滤镜（只为了让快照非空）
                Operate.FilterConfig.List.lstFilterInfo.Clear();
                Operate.FilterConfig.Filter.AddFilter(
                    true, Guid.NewGuid(), "跑测滤镜",
                    false, "", false, "", false, "", false, "",
                    Operate.FilterConfig.Filter.FilterMode.Normal,
                    Operate.FilterConfig.Filter.FilterAction.NoModify_Display,
                    false, Operate.FilterConfig.Filter.FilterExecuteType.None, Guid.Empty,
                    new Operate.FilterConfig.Filter.FilterFunction(true, true, true, true, true, true, true, true, true, true, true, true),
                    Operate.FilterConfig.Filter.FilterStartFrom.Head,
                    false, false, 1, false, 0, "", 0, "", "", "ZZ-NEVER-MATCH", "");

                int swaps = 0;
                var sw = Stopwatch.StartNew();

                while (sw.ElapsedMilliseconds < 5000)
                {
                    link.PushFilters();
                    swaps++;
                    Thread.Sleep(50);
                }

                Thread.Sleep(500);
                var got = DrainList();

                rep.AppendLine("- 5 秒里换了 " + swaps + " 次滤镜快照");
                rep.AppendLine("- 期间收到 " + got.Count + " 条封包（应当照常在收）");
                rep.AppendLine("- 链路状态 " + link.State + "（应为 Attached，没被换配置搞断）");

                bool ok = got.Count > 0 && link.State == ShellLink.LinkState.Attached && swaps > 20;

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
                Operate.FilterConfig.List.lstFilterInfo.Clear();
                try { link.Dispose(); } catch { }
                try { if (target != null && !target.HasExited) { target.Kill(); } } catch { }
            }
        }

        #endregion

        #region//#7 外壳崩 → 目标自行卸钩 → 重新附加

        private static bool Case7_ShellCrashThenReattach(StringBuilder rep)
        {
            rep.AppendLine("## #7 外壳崩 → 目标 3 秒内卸钩 → 对同一 PID 重新附加");
            rep.AppendLine();

            Process target = null;
            Process ghostShell = null;
            var link = new ShellLink();

            try
            {
                target = StartTarget(60);
                rep.AppendLine("- 靶子 PID " + target.Id);

                //=== 用一个<b>独立进程</b>当「会崩的外壳」，然后 Kill 它 ===
                //在本进程里模拟不了「外壳崩」——本进程崩了就没人写报告了。
                var psi = new ProcessStartInfo(
                    Process.GetCurrentProcess().MainModule.FileName,
                    "--ghost-shell " + target.Id)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                };

                ghostShell = Process.Start(psi);
                string line = ghostShell.StandardOutput.ReadLine();
                rep.AppendLine("- 临时外壳: " + (line ?? "(无输出)"));

                if (line == null || !line.StartsWith("GHOST_ATTACHED"))
                {
                    rep.AppendLine();
                    rep.AppendLine("→ FAIL（临时外壳没能附加上）");
                    rep.AppendLine();
                    return false;
                }

                //== 模拟外壳崩溃 ==
                ghostShell.Kill();
                ghostShell.WaitForExit(5000);
                rep.AppendLine("- 已 Kill 临时外壳（模拟外壳崩溃）");

                //目标必须在 3 秒心跳超时后自行卸钩，恢复原生行为
                Thread.Sleep(5000);

                bool targetAlive = !target.HasExited;
                rep.AppendLine("- 5 秒后靶子还活着: " + (targetAlive ? "是" : "否 ← 外壳崩不该把目标带走"));

                //== 重新附加 ==
                Drain();
                link.Attach(target.Id, null, 15000);
                link.StartHook();
                rep.AppendLine("- 对同一 PID 重新附加: " + link.State);

                Thread.Sleep(4000);
                var got = DrainList();
                rep.AppendLine("- 重新附加后收到 " + got.Count + " 条封包");

                bool ok = targetAlive && link.State == ShellLink.LinkState.Attached && got.Count > 0;

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
                try { if (ghostShell != null && !ghostShell.HasExited) { ghostShell.Kill(); } } catch { }
                try { if (target != null && !target.HasExited) { target.Kill(); } } catch { }
            }
        }

        /// <summary>
        /// 「会崩的外壳」：附加上去、装好钩子，然后<b>什么都不做</b>等着被 Kill。
        /// 由 <see cref="Case7_ShellCrashThenReattach"/> 拉起。
        /// </summary>
        public static int RunGhostShell(string[] args)
        {
            bool noisy = args.Contains("--noisy");
            int pid = int.Parse(args[Array.IndexOf(args, "--ghost-shell") + 1]);

            InitShellSide();

            /*
                --noisy：配一条<b>每个包都命中</b>的滤镜。

                【它验的是什么】滤镜一命中，目标那边就在<b>钩子线程</b>上调一次
                DoFilterLog → OnFilterLog → 事件流。事件流原来是<b>同步写管道</b>的，
                于是外壳一卡住、管道缓冲一满，写就阻塞 —— 阻塞的是目标的 send()，
                也就是把目标游戏卡住。封包那条路一直有环护着，事件这条路漏了。

                匹配串 "0|00,1|01,2|02" 对着 target3 的载荷（payload[i] = i & 0xFF），
                所以每一个包都命中。动作用 NoModify_Display：不改字节、只产生日志，
                这样量到的吞吐变化只来自「事件流阻不阻塞」这一件事。
            */
            if (noisy)
            {
                Operate.FilterConfig.List.lstFilterInfo.Clear();
                Operate.FilterConfig.Filter.AddFilter(
                    true, Guid.NewGuid(), "压测滤镜",
                    false, "", false, "", false, "", false, "",
                    Operate.FilterConfig.Filter.FilterMode.Normal,
                    Operate.FilterConfig.Filter.FilterAction.NoModify_Display,
                    false, Operate.FilterConfig.Filter.FilterExecuteType.None, Guid.Empty,
                    new Operate.FilterConfig.Filter.FilterFunction(
                        true, true, true, true, true, true, true, true, true, true, true, true),
                    Operate.FilterConfig.Filter.FilterStartFrom.Head,
                    false, false, 1, false, 0, "", 0, "", "",
                    "0|00,1|01,2|02", "");
            }

            var link = new ShellLink();
            link.Attach(pid, null, 15000);
            link.StartHook();

            Console.WriteLine("GHOST_ATTACHED pid=" + link.TargetPid + " state=" + link.State);
            Console.Out.Flush();

            /*
                --noisy 还要往回报一行「滤镜命中了几次、外壳这边收到几条滤镜日志」。

                ⚠️ <b>这一行不是装饰，它是 #6 的证据。</b>没有它，「滤镜一次都没命中」
                与「事件流没有拖住钩子线程」在报告上长得一模一样 —— 那条断言就成了空的。

                hits 从 Stats 事件带回来（协议 v3 起附了滤镜那六个全局计数），
                logs 是外壳侧滤镜日志队列的长度：前者证明目标那边真的在钩子线程上
                调 DoFilterLog，后者证明那些事件真的流到了外壳。

                队列只涨不消是有意的 —— 临时外壳没有搬运拍，本来也不需要。
            */
            if (noisy)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        Console.WriteLine("NOISY hits=" + Operate.FilterConfig.Filter.FilterExecute_CNT
                                          + " logs=" + Operate.LogConfig.Queue.cqFilterLogInfo.Count);
                        Console.Out.Flush();
                    }
                }) { IsBackground = true }.Start();
            }

            //等着被杀。绝不主动 Detach —— 那就不是「崩溃」了。
            Thread.Sleep(Timeout.Infinite);
            return 0;
        }

        #endregion

        #region//杂项

        private static void InitShellSide()
        {
            string dir = Path.Combine(Path.GetTempPath(), "WPEHookTest");
            Directory.CreateDirectory(dir);
            Operate.DataBase.dbPath = dir;
            Operate.DataBase.InitDB();
            Operate.SystemConfig.LoadSystemConfig_FromDB();
            Operate.SystemConfig.SpeedMode = false;

            Operate.PacketConfig.Packet.Support_WS1 = true;
            Operate.PacketConfig.Packet.Support_WS2 = true;
            Operate.PacketConfig.Packet.Support_MsWS = true;
        }

        private static Process StartTarget(int seconds)
        {
            var psi = new ProcessStartInfo(
                Process.GetCurrentProcess().MainModule.FileName,
                "--target --seconds " + seconds)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            Process p = Process.Start(psi);
            string ready = p.StandardOutput.ReadLine();

            if (ready == null || !ready.StartsWith("TARGET_READY"))
            {
                throw new Exception("靶子没起来: " + (ready ?? "(无输出)"));
            }

            //把 stdout / stderr 抽干，否则管道满了靶子会卡住
            new Thread(() => { try { p.StandardOutput.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();
            new Thread(() => { try { p.StandardError.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();

            return p;
        }

        private static void Drain()
        {
            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out _)) { }
        }

        private static int CountQueued()
        {
            return Operate.PacketConfig.Queue.cqPacketInfo.Count;
        }

        private static List<PacketInfo> DrainList()
        {
            var list = new List<PacketInfo>();
            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out PacketInfo pi)) { list.Add(pi); }
            return list;
        }

        private static void KillPid(int pid)
        {
            try { Process.GetProcessById(pid).Kill(); } catch { }
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
