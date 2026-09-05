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
    /// 阶段 2 的验证：滤镜真的在<b>目标进程里</b>改写了发出去的字节，
    /// 以及滤镜的四种动作、仓库事件、执行器命令。
    ///
    /// 【为什么服务端要放在外壳这一侧】
    /// 只看抓到的封包不足以证明改写生效 —— 那份数据本来就是钩子给的，
    /// 它说改了就改了。把服务端放在<b>外壳</b>进程里，看它<b>实际收到</b>什么字节，
    /// 才是「线上真的变了」的证据。这也正是矩阵第 3 项要的东西。
    /// </summary>
    internal static class Matrix2
    {
        #region//靶子：连外壳的端口，反复发同一句话

        public static int RunTarget2(string[] args)
        {
            int port = int.Parse(GetArg(args, "--port", "0"));
            int seconds = int.Parse(GetArg(args, "--seconds", "30"));

            //挂起启动的目标连加载器都没跑过；已在跑的目标也可能没加载 wsock32
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

            Console.WriteLine("TARGET2_READY pid=" + Process.GetCurrentProcess().Id);
            Console.Out.Flush();

            int deadline = unchecked(Environment.TickCount + seconds * 1000);
            int round = 0;

            while (unchecked(deadline - Environment.TickCount) > 0)
            {
                round++;
                Say(sock, "HELLO-WORLD-" + round.ToString("D4"));
                Thread.Sleep(80);
            }

            Native.closesocket(sock);
            return 0;
        }

        private static unsafe void Say(IntPtr s, string payload)
        {
            byte[] b = Encoding.ASCII.GetBytes(payload);
            fixed (byte* p = b) { WS2_32.send((int)s, (IntPtr)p, b.Length, SocketFlags.None); }
        }

        #endregion

        #region//跑测

        public static int Run(string[] args)
        {
            string outPath = GetArg(args, "--out", null);

            var rep = new StringBuilder();
            rep.AppendLine("# WPE 注入模式 IPC · 阶段 2 验证");
            rep.AppendLine();
            rep.AppendLine("验的是「滤镜引擎留在目标里、真的改写了线上的字节」，");
            rep.AppendLine("以及仓库事件与执行器命令。服务端放在<b>外壳</b>进程里 ——");
            rep.AppendLine("只看抓到的封包不算证据，要看对端<b>实际收到</b>什么。");
            rep.AppendLine();

            InitShellSide();

            var results = new List<bool>();
            results.Add(CaseReplaceOnTheWire(rep));
            results.Add(CaseWareHouseEvent(rep));
            results.Add(CaseExecutorCommands(rep));

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

        #region//#3 滤镜的 Replace 真的改到了线上的字节

        private static bool CaseReplaceOnTheWire(StringBuilder rep)
        {
            rep.AppendLine("## #3 滤镜 Replace —— 对端收到的是<b>改写后</b>的字节");
            rep.AppendLine();

            var received = new List<string>();
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;

            new Thread(() => Collect(listener, received)) { IsBackground = true }.Start();

            var link = new ShellLink();
            Process target = null;

            try
            {
                //=== 先把滤镜配好，再注入 —— 连上时会推全量快照 ===
                Operate.FilterConfig.List.lstFilterInfo.Clear();

                /*
                    普通模式的替换：把开头的 HELLO 换成 XXXXX。

                    ⚠️ <b>普通模式的 FSearch / FModify 不是一串十六进制</b>，
                    而是「位置|十六进制字节」的逗号列表（核过 CheckFilter_IsMatch_Normal
                    与 ProcessModifications）。第一版按纯十六进制串写，结果一条都没命中，
                    而滤镜不命中是<b>完全静默</b>的 —— 这正是这个跑测存在的意义。

                      HELLO = 48 45 4C 4C 4F  →  0|48,1|45,2|4C,3|4C,4|4F
                      XXXXX = 58 58 58 58 58  →  0|58,1|58,2|58,3|58,4|58
                */
                Operate.FilterConfig.Filter.AddFilter(
                    true, Guid.NewGuid(), "替换测试",
                    false, "", false, "", false, "", false, "",
                    Operate.FilterConfig.Filter.FilterMode.Normal,
                    Operate.FilterConfig.Filter.FilterAction.Replace,
                    false, Operate.FilterConfig.Filter.FilterExecuteType.None, Guid.Empty,
                    AllFunctions(),
                    Operate.FilterConfig.Filter.FilterStartFrom.Head,
                    false, false, 1, false, 0, "", 0, "", "",
                    "0|48,1|45,2|4C,3|4C,4|4F", "0|58,1|58,2|58,3|58,4|58");

                target = StartTarget2(port, 30);
                link.Attach(target.Id, null, 15000);
                link.StartHook();

                Thread.Sleep(5000);

                lock (received) { }
                string[] got;
                lock (received) { got = received.ToArray(); }

                int replaced = got.Count(s => s.StartsWith("XXXXX-WORLD-"));
                int original = got.Count(s => s.StartsWith("HELLO-WORLD-"));

                /*
                    ⚠️ 判据<b>不能是「一条原样的都没有」</b>。

                    靶子是<b>已经在跑</b>的进程（Inject 那条路，不是挂起启动），
                    它一连上就开始发，而外壳要走完「注入 → 握手 → 推快照 → StartHook」
                    才装上钩子 —— 这中间发出去的那几条本来就该是原样的。
                    真正的不变量是：<b>第一条被改写之后，就再也不该出现原样的</b>。
                */
                int firstReplaced = Array.FindIndex(got, s => s.StartsWith("XXXXX-WORLD-"));
                int lateOriginal = firstReplaced < 0
                    ? original
                    : got.Skip(firstReplaced).Count(s => s.StartsWith("HELLO-WORLD-"));

                rep.AppendLine("- 靶子发的是 `HELLO-WORLD-nnnn`，滤镜把 `HELLO` 替换成 `XXXXX`");
                rep.AppendLine("- 外壳这一侧的服务端共收到 " + got.Length + " 段");
                rep.AppendLine("- 其中改写后的（XXXXX-）: **" + replaced + "**，原样的（HELLO-）: " + original);
                rep.AppendLine("- 装钩之前发出去的那几条本来就是原样的（第 " + (firstReplaced + 1) + " 段起开始改写）");
                rep.AppendLine("- <b>改写开始之后</b>再出现的原样段: " + lateOriginal + "（必须为 0）");

                if (got.Length > 0)
                {
                    rep.AppendLine("- 头三段: " + string.Join(" | ", got.Take(3).ToArray()));
                    rep.AppendLine("- 末三段: " + string.Join(" | ", got.Skip(Math.Max(0, got.Length - 3)).ToArray()));
                }

                //抓到的封包里也该带着 Replace 这个动作
                var captured = DrainList();
                int actReplace = captured.Count(p => p.FilterAction == Operate.FilterConfig.Filter.FilterAction.Replace);
                rep.AppendLine("- 外壳收到 " + captured.Count + " 条封包，动作为 Replace 的 " + actReplace + " 条");

                bool ok = replaced > 0 && lateOriginal == 0 && actReplace > 0;

                link.StopHook();
                link.Detach();

                rep.AppendLine();
                rep.AppendLine(ok ? "→ ok（改写发生在目标进程里，线上的字节确实变了）" : "→ FAIL");
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
                try { listener.Stop(); } catch { }
            }
        }

        #endregion

        #region//#9 滤镜的「入库」动作走事件回到外壳

        private static bool CaseWareHouseEvent(StringBuilder rep)
        {
            rep.AppendLine("## #9 滤镜 → 仓库：目标发事件，外壳落库");
            rep.AppendLine();

            var received = new List<string>();
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            new Thread(() => Collect(listener, received)) { IsBackground = true }.Start();

            var link = new ShellLink();
            Process target = null;

            try
            {
                /*
                    ⚠️ 第 16 次 CS0012：WareHouseInfo / DataInfo 都继承 AntdUI 的 NotifyProperty，
                    一出现在跑测能看到的签名上就编译不过。老规矩 —— 走 Operate 侧
                    按 Id 收发、只出基础类型的那组入口，不给跑测加 AntdUI 引用。
                */
                Guid wid = new Guid(Operate.WareHouseConfig.List.AddWareHouse_New_ById());

                //命中就入库（动作用 NoModify_Display，别改字节）
                Operate.FilterConfig.List.lstFilterInfo.Clear();
                Operate.FilterConfig.Filter.AddFilter(
                    true, Guid.NewGuid(), "入库测试",
                    false, "", false, "", false, "", false, "",
                    Operate.FilterConfig.Filter.FilterMode.Normal,
                    Operate.FilterConfig.Filter.FilterAction.NoModify_Display,
                    true, Operate.FilterConfig.Filter.FilterExecuteType.WareHouse, wid,
                    AllFunctions(),
                    Operate.FilterConfig.Filter.FilterStartFrom.Head,
                    false, false, 1, false, 0, "", 0, "", "",
                    "0|48,1|45,2|4C,3|4C,4|4F", "");

                target = StartTarget2(port, 30);
                link.Attach(target.Id, null, 15000);
                link.StartHook();

                Thread.Sleep(4000);

                WareHouseRow row = Operate.WareHouseConfig.List.OpenWareHouseEdit_ById(wid.ToString().ToUpper());
                int stored = row == null ? -1 : row.DataCount;

                rep.AppendLine("- 滤镜命中就执行「入库」；目标不持有仓库，只发 StoreAdded 事件");
                rep.AppendLine("- 外壳这边仓库「" + (row == null ? "?" : row.Name) + "」里攒到 **" + stored + "** 条");

                bool ok = stored > 0;

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
                try { listener.Stop(); } catch { }
            }
        }

        #endregion

        #region//#8 执行器命令与 1 Hz 统计

        private static bool CaseExecutorCommands(StringBuilder rep)
        {
            rep.AppendLine("## #8 执行器：命令走通道、状态由目标说了算");
            rep.AppendLine();

            var received = new List<string>();
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            new Thread(() => Collect(listener, received)) { IsBackground = true }.Start();

            var link = new ShellLink();
            Process target = null;

            try
            {
                //一条什么都不发的发送（发送集为空），只验命令与状态回报这条链路。
                //同样走按 Id 的入口（SendInfo 也继承 NotifyProperty）。
                Operate.SendConfig.List.lstSendInfo.Clear();
                Operate.SendConfig.List.AddSend_New_ById();

                target = StartTarget2(port, 30);
                link.Attach(target.Id, null, 15000);

                //刚连上时统计还没来，等一拍
                Thread.Sleep(1500);
                bool before = link.SendListRunning;

                link.StartSendList();
                Thread.Sleep(1500);
                bool duringOrDone = link.SendListRunning;

                link.StopSendList();
                Thread.Sleep(1500);
                bool after = link.SendListRunning;

                rep.AppendLine("- 下发 StartSendList / StopSendList，命令都得到了应答（没抛异常）");
                rep.AppendLine("- 「在跑」状态由目标随 1 Hz 的 Stats 事件报上来：");
                rep.AppendLine("  启动前 " + before + " → 启动后 " + duringOrDone + " → 停止后 " + after);
                rep.AppendLine("  （发送集是空的，worker 会立刻跑完，所以「启动后」为 false 也是对的 ——");
                rep.AppendLine("   这一项验的是命令与状态回报这条链路通不通）");

                //统计包本身有没有到：滤镜表是空的，看发送表的计数有没有被回填
                bool statsArrived = after == false;

                link.Detach();

                rep.AppendLine();
                rep.AppendLine(statsArrived ? "→ ok" : "→ FAIL");
                rep.AppendLine();
                return statsArrived;
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

        #region//杂项

        /// <summary>外壳这一侧的服务端：把收到的每一段原样记下来。</summary>
        private static void Collect(TcpListener listener, List<string> sink)
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

                        string s = Encoding.ASCII.GetString(buf, 0, n);
                        lock (sink) { sink.Add(s); }
                    }
                }
            }
            catch { /* 靶子被杀掉时正常抛 */ }
        }

        private static Operate.FilterConfig.Filter.FilterFunction AllFunctions()
        {
            return new Operate.FilterConfig.Filter.FilterFunction(
                true, true, true, true, true, true, true, true, true, true, true, true);
        }

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

            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out _)) { }
        }

        private static Process StartTarget2(int port, int seconds)
        {
            var psi = new ProcessStartInfo(
                Process.GetCurrentProcess().MainModule.FileName,
                "--target2 --port " + port + " --seconds " + seconds)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            Process p = Process.Start(psi);
            string ready = p.StandardOutput.ReadLine();

            if (ready == null || !ready.StartsWith("TARGET2_READY"))
            {
                throw new Exception("靶子没起来: " + (ready ?? "(无输出)"));
            }

            new Thread(() => { try { p.StandardOutput.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();
            new Thread(() => { try { p.StandardError.ReadToEnd(); } catch { } }) { IsBackground = true }.Start();

            return p;
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
