using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using WinsockPacketEditor;
using FA = WinsockPacketEditor.Operate.FilterConfig.Filter.FilterAction;
using PT = WinsockPacketEditor.Operate.PacketConfig.Packet.PacketType;

namespace WPEHookTest
{
    /// <summary>
    /// 钩子体 / 滤镜引擎的缺陷回归（2026-09-15 那份注入模式分析报告的 A、B 两批）。
    ///
    /// 用法：<c>WPEHookTest.exe --hookfix [--only A1a,A3] [--out 报告.md]</c>
    ///
    /// 【手法与 Program.cs 相同】钩子装在本进程里，<b>装钩子的线程（主线程）不被钩</b>，
    /// 所以服务端一律在主线程上跑，客户端在工作线程上跑（被钩到）。
    ///
    /// 【每一条都必须在修之前的代码上失败】否则就是没验到东西。
    /// A3（滤镜自己执行自己）在旧代码上是栈溢出、整个进程直接没了 —— 单独用 --only 跑。
    /// </summary>
    internal static class HookFix
    {
        private const string LOOPBACK = "127.0.0.1";

        private static readonly StringBuilder rep = new StringBuilder();

        #region//入口

        public static int Run(string[] args)
        {
            string outPath = GetArg(args, "--out", null);
            string only = GetArg(args, "--only", null);
            HashSet<string> pick = only == null ? null
                : new HashSet<string>(only.Split(',').Select(s => s.Trim()), StringComparer.OrdinalIgnoreCase);

            Init();

            rep.AppendLine("# 钩子体 / 滤镜引擎缺陷回归");
            rep.AppendLine();
            rep.AppendLine("进程位数: " + (IntPtr.Size == 8 ? "x64" : "x86"));
            rep.AppendLine();

            //【顺序】A17 自己装卸钩子；之后装一次钩子跑全部走套接字的；最后跑不需要钩子的
            //（A13 会发布滤镜快照、改变 FilterEngine 的数据源，必须放最后）
            var hookless = new List<Tuple<string, string, Func<bool>>>
            {
                T("A2", "随机字节在多线程下不卡死", CaseRandomConcurrent),
                T("A3", "滤镜执行自己 / 互相执行不栈溢出", CaseFilterRecursion),
                T("A14", "递进计数与执行次数在并发命中下不丢", CaseProgressionRace),
                T("A15", "高级滤镜首个条件带通配也能匹配", CaseAdvancedWildcardFirst),
                T("B8", "按套接字过滤时不再每包做系统调用（同结果）", CasePortFilterStillWorks),
                T("A13", "换滤镜快照不把目标里的连续递进计数清零", CaseSnapshotKeepsProgression),
                T("Bparse", "预解析保持通配、排除、修改顺序及编辑后失效语义", CaseParsedRules),
            };

            var hooked = new List<Tuple<string, string, Func<bool>>>
            {
                T("A5", "诊断出口抛异常不丢发送、不重复发送、不放行拦截包", CaseFailingHost),
                T("A8e", "TCP 变长规则遇到受控短写仍返回原包前缀长度", CaseChangedPartialSend),
                T("Aasync", "重叠发送保留真实完成通知和调用方缓冲区", CaseOverlappedPassthrough),
                T("A1a", "拦截 recv 不再返回 0（阻塞套接字取下一段）", CaseInterceptRecvBlocking),
                T("A1b", "拦截 recv（非阻塞）返回 WSAEWOULDBLOCK 而不是 0", CaseInterceptRecvNonBlocking),
                T("A1c", "拦截 WSARecv 不再返回 0 字节", CaseInterceptWsaRecv),
                T("A1d", "拦截 recvfrom 丢掉这个报文、取下一个", CaseInterceptRecvFrom),
                T("A8a", "TCP 变短改包回退原包，保留非阻塞语义", CaseSendChangeShorter),
                T("A8b", "TCP 变长改包回退原包，保留非阻塞语义", CaseSendChangeLonger),
                T("A8c", "WSASend TCP 变长改包回退原包", CaseWsaSendChangeLonger),
                T("A8d", "recv 换包变长：只受缓冲区容量限制", CaseRecvChangeLonger),
                T("A9", "WSASend UDP 多缓冲区换包：不携带旧尾部", CaseWsaSendMultiChange),
                T("A10", "WSARecv 多缓冲区换包变短：不抛异常、内容正确", CaseWsaRecvMultiShorter),
                T("A11", "0 字节 WSASend 照常调原函数", CaseWsaSendZero),
                T("A12a", "send 的替换不改写程序自己的缓冲区", CaseSendKeepsCallerBuffer),
                T("A12b", "WSASend 的替换不改写程序自己的缓冲区", CaseWsaSendKeepsCallerBuffer),
                T("A4", "滤镜触发的发送进执行器表、同一条不叠加", CaseFilterTriggeredSend),
                T("A16", "SendPacket 在部分发送时补发完整", CaseSendPacketFull),
                T("B6a", "sendto 目标地址为 NULL（已 connect 的 UDP）照常发出", CaseSendToNullAddr),
                T("B6b", "WSARecvFrom 来源地址为 NULL 照常过滤", CaseWsaRecvFromNullAddr),
                T("B7", "失败的 recv 保留 WSAEWOULDBLOCK 错误码", CaseLastErrorPreserved),
                T("B7x", "日志出口覆盖线程错误码后仍返回原生错误", CaseClobberedError),
                T("Breuse", "未改包共用原始数组且钩子时间为 UTC", CaseBufferReuse),
            };

            int pass = 0, fail = 0, skip = 0;

            Action<Tuple<string, string, Func<bool>>> exec = t =>
            {
                if (pick != null && !pick.Contains(t.Item1)) { skip++; return; }
                ResetState();
                Console.Error.WriteLine("  [case] " + t.Item1 + " " + t.Item2);
                rep.AppendLine("## " + t.Item1 + " " + t.Item2);
                rep.AppendLine();
                bool ok;
                try { ok = t.Item3(); }
                catch (Exception ex) { rep.AppendLine("- 异常: " + ex.GetType().Name + ": " + ex.Message); ok = false; }
                rep.AppendLine();
                rep.AppendLine(ok ? "→ ok" : "→ FAIL");
                rep.AppendLine();
                if (ok) { pass++; } else { fail++; }
            };

            if (pick == null || pick.Contains("A17")) { exec(T("A17", "一个钩子装不上，其余照装", CaseStartHookResilient)); } else { skip++; }

            bool anyHooked = pick == null || hooked.Any(t => pick.Contains(t.Item1));
            WinSockHook ws = null;
            if (anyHooked)
            {
                ws = new WinSockHook();
                ws.StartHook();
            }

            foreach (var t in hooked) { exec(t); }

            if (ws != null) { ws.StopHook(); }

            foreach (var t in hookless) { exec(t); }

            rep.AppendLine("合计: 通过 " + pass + "，失败 " + fail + (skip > 0 ? "，未选 " + skip : ""));
            rep.AppendLine(fail == 0 ? "结论: PASS" : "结论: FAIL");

            string text = rep.ToString();
            Console.WriteLine(text);
            if (!string.IsNullOrEmpty(outPath)) { File.WriteAllText(outPath, text, new UTF8Encoding(false)); }

            //旧代码上可能有停不下来的执行器 / 卡死的线程，别让它们拖住退出
            Environment.Exit(fail == 0 ? 0 : 1);
            return 0;
        }

        private static Tuple<string, string, Func<bool>> T(string id, string title, Func<bool> body)
        {
            return Tuple.Create(id, title, body);
        }

        private static void Init()
        {
            IntPtr wsaData = Marshal.AllocHGlobal(408);
            try { Native.WSAStartup(0x0202, wsaData); }
            finally { Marshal.FreeHGlobal(wsaData); }

            Native.LoadLibrary("ws2_32.dll");
            Native.LoadLibrary("wsock32.dll");
            Native.LoadLibrary("mswsock.dll");

            string dir = Path.Combine(Path.GetTempPath(), "WPEHookTest");
            Directory.CreateDirectory(dir);
            Operate.DataBase.dbPath = dir;
            Operate.DataBase.InitDB();
            Operate.SystemConfig.LoadSystemConfig_FromDB();

            Operate.SystemConfig.SpeedMode = false;
            Operate.PacketConfig.Packet.Support_WS1 = true;
            Operate.PacketConfig.Packet.Support_WS2 = true;
            Operate.PacketConfig.Packet.Support_MsWS = true;
            Operate.FilterConfig.Filter.FilterExecute = Operate.FilterConfig.Filter.Execute.Sequence;
        }

        private static void ResetState()
        {
            Operate.FilterConfig.List.lstFilterInfo.Clear();
            Operate.SendConfig.List.lstSendInfo.Clear();
            Operate.SystemConfig.SpeedMode = false;
            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out _)) { }
        }

        #endregion

        #region//A17 一个钩子装不上，其余照装

        private static bool CaseStartHookResilient()
        {
            string saved = WSock32.ModuleName;
            WSock32.ModuleName = "WPE_NO_SUCH_MODULE.dll";
            var ws = new WinSockHook();

            try
            {
                ws.StartHook();
                WSock32.ModuleName = saved;

                using (var pair = TcpPair.Open())
                {
                    RunClient(() => SendBytes(pair.Client, Encoding.ASCII.GetBytes("A17-PING"), PT.WS2_Send));
                    byte[] got = pair.ServerReadFor(500);
                    rep.AppendLine("- 对端收到: " + Ascii(got));
                }

                Thread.Sleep(1500);
                var captured = Drain();
                int ws2 = captured.Count(p => p.PacketType == PT.WS2_Send);
                rep.AppendLine("- WS1 模块名故意写错之后，WS2_Send 抓到 " + ws2 + " 条（必须 > 0）");
                return ws2 > 0;
            }
            finally
            {
                WSock32.ModuleName = saved;
                ws.StopHook();
            }
        }

        #endregion

        #region//A1 拦截接收

        private static Operate.FilterConfig.Filter.FilterFunction RecvFunctions()
        {
            return new Operate.FilterConfig.Filter.FilterFunction { Recv = true, RecvFrom = true, WSARecv = true, WSARecvFrom = true };
        }

        private static Operate.FilterConfig.Filter.FilterFunction SendFunctions()
        {
            return new Operate.FilterConfig.Filter.FilterFunction { Send = true, SendTo = true, WSASend = true, WSASendTo = true };
        }

        private const string SEARCH_DROP = "0|44,1|52,2|4F,3|50"; //"DROP"

        private static bool CaseInterceptRecvBlocking()
        {
            AddFilter("拦截DROP", FA.Intercept, SEARCH_DROP, "", RecvFunctions());

            using (var pair = TcpPair.Open())
            {
                int r = 0; string got = null;
                var client = StartClient(() =>
                {
                    byte[] buf = new byte[512];
                    r = RecvBytes(pair.Client, buf, PT.WS2_Recv);
                    got = r > 0 ? Encoding.ASCII.GetString(buf, 0, r) : null;
                });

                pair.ServerSend("DROP1234");
                Thread.Sleep(300);
                pair.ServerSend("KEEP5678");

                bool joined = client.Join(5000);
                rep.AppendLine("- 服务端先发 DROP1234（被拦截），300ms 后发 KEEP5678");
                rep.AppendLine("- 客户端 recv 返回 " + r + "，内容 " + (got ?? "(无)") + (joined ? "" : "（超时）"));
                return joined && r == 8 && got == "KEEP5678";
            }
        }

        private static bool CaseInterceptRecvNonBlocking()
        {
            AddFilter("拦截DROP", FA.Intercept, SEARCH_DROP, "", RecvFunctions());

            using (var pair = TcpPair.Open())
            {
                pair.ServerSend("DROP1234");
                Thread.Sleep(300);

                int r = 0, err = 0;
                RunClient(() =>
                {
                    SetNonBlocking(pair.Client, true);
                    byte[] buf = new byte[512];
                    r = RecvBytes(pair.Client, buf, PT.WS2_Recv);
                    err = Marshal.GetLastWin32Error();
                });

                rep.AppendLine("- 非阻塞套接字，唯一一段 DROP1234 被拦截");
                rep.AppendLine("- recv 返回 " + r + "，错误码 " + err + "（必须是 -1 / 10035）");
                return r == -1 && err == 10035;
            }
        }

        private static bool CaseInterceptWsaRecv()
        {
            AddFilter("拦截DROP", FA.Intercept, SEARCH_DROP, "", RecvFunctions());

            using (var pair = TcpPair.Open())
            {
                SocketError err = SocketError.SocketError; int got = -1; string text = null;
                var client = StartClient(() =>
                {
                    byte[] buf = new byte[512];
                    got = WsaRecv(pair.Client, new[] { buf }, out err);
                    text = got > 0 ? Encoding.ASCII.GetString(buf, 0, got) : null;
                });

                pair.ServerSend("DROP1234");
                Thread.Sleep(300);
                pair.ServerSend("KEEP5678");

                bool joined = client.Join(5000);
                rep.AppendLine("- WSARecv 结果 " + err + "，字节数 " + got + "，内容 " + (text ?? "(无)") + (joined ? "" : "（超时）"));
                return joined && err == SocketError.Success && got == 8 && text == "KEEP5678";
            }
        }

        private static bool CaseInterceptRecvFrom()
        {
            AddFilter("拦截DROP", FA.Intercept, SEARCH_DROP, "", RecvFunctions());

            using (var pair = UdpPair.Open())
            {
                int r = 0; string text = null;
                var client = StartClient(() =>
                {
                    byte[] buf = new byte[512];
                    r = RecvFromBytes(pair.Client, buf, PT.WS2_RecvFrom);
                    text = r > 0 ? Encoding.ASCII.GetString(buf, 0, r) : null;
                });

                Thread.Sleep(200);
                pair.ServerSendToClient("DROP1234");
                Thread.Sleep(200);
                pair.ServerSendToClient("KEEP5678");

                bool joined = client.Join(5000);
                rep.AppendLine("- recvfrom 返回 " + r + "，内容 " + (text ?? "(无)") + (joined ? "" : "（超时）"));
                return joined && r == 8 && text == "KEEP5678";
            }
        }

        #endregion

        #region//A8 换包改变长度

        private const string SEARCH_LONG = "0|4C,1|4F,2|4E,3|47"; //"LONG"

        private static string ModifyOf(string s)
        {
            return string.Join(",", Encoding.ASCII.GetBytes(s).Select((b, i) => i + "|" + b.ToString("X2")));
        }

        private static bool CaseSendChangeShorter()
        {
            AddFilter("换短", FA.Change, SEARCH_LONG, ModifyOf("ABC"), SendFunctions());

            using (var pair = TcpPair.Open())
            {
                SetNonBlocking(pair.Client, true);
                int r = 0;
                RunClient(() => r = SendBytes(pair.Client, Encoding.ASCII.GetBytes("LONGPAYLOAD1"), PT.WS2_Send));
                byte[] got = pair.ServerReadFor(600);

                rep.AppendLine("- 程序发 12 字节，滤镜换成 ABC");
                rep.AppendLine("- send 返回 " + r + "（必须 12），对端收到 " + Ascii(got) + "（必须原包 LONGPAYLOAD1）");
                return r == 12 && Ascii(got) == "LONGPAYLOAD1";
            }
        }

        private static unsafe bool CaseChangedPartialSend()
        {
            AddFilter("变长回退", FA.Change, "0|4C", ModifyOf("SHORT"), SendFunctions());
            using (var pair = TcpPair.Open())
            {
                SetNonBlocking(pair.Client, true);
                byte[] payload = Encoding.ASCII.GetBytes("LONGPAYLOAD1");
                int nativeLength = -1;
                Func<IntPtr, int, IntPtr, SocketError> shortWrite = (buffers, count, sent) =>
                {
                    var buffer = Marshal.PtrToStructure<Operate.PacketConfig.Packet.WSABUF>(buffers);
                    nativeLength = buffer.len;
                    // Deterministically model a transport consuming only a prefix;
                    // those exact three bytes still go over the real loopback socket.
                    int written = WS2_32.send((int)pair.Client, buffer.buf, Math.Min(3, buffer.len), SocketFlags.None);
                    Marshal.WriteInt32(sent, written);
                    return written < 0 ? SocketError.SocketError : SocketError.Success;
                };
                var method = typeof(WinSockHook).GetMethod("SendSynchronousBuffers",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                int sentCount = -1;
                SocketError result;
                fixed (byte* data = payload)
                {
                    var buffer = new Operate.PacketConfig.Packet.WSABUF { len = payload.Length, buf = (IntPtr)data };
                    result = (SocketError)method.Invoke(null, new object[] { (int)pair.Client, (IntPtr)(&buffer), 1,
                        (IntPtr)(&sentCount), PT.WSASend, new Operate.PacketConfig.Packet.SockAddr(), shortWrite });
                }
                byte[] got = pair.ServerReadFor(300);
                rep.AppendLine("- 受控原生短写：原包 12 字节，传给传输层 " + nativeLength + "，报告 " + sentCount + "，实际收到 " + Ascii(got));
                return result == SocketError.Success && nativeLength == payload.Length && sentCount == 3 && Ascii(got) == "LON";
            }
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateEvent(IntPtr security, bool manualReset, bool initialState, IntPtr name);
        [DllImport("kernel32.dll")]
        private static extern uint WaitForSingleObject(IntPtr handle, uint milliseconds);
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);
        [DllImport("ws2_32.dll", SetLastError = true, EntryPoint = "WSAGetOverlappedResult")]
        private static extern bool GetOverlappedResult(int socket, IntPtr overlapped, out int transferred, bool wait, out SocketFlags flags);

        private static unsafe bool CaseOverlappedPassthrough()
        {
            AddFilter("异步不伪造完成", FA.Intercept, "0|48", "", SendFunctions());
            using (var pair = TcpPair.Open())
            {
                byte[] bytes = Encoding.ASCII.GetBytes("HELLO-ASYNC");
                bool completed = false;
                int transferred = -1;
                RunClient(() =>
                {
                    IntPtr signal = CreateEvent(IntPtr.Zero, true, false, IntPtr.Zero);
                    IntPtr overlap = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Operate.PacketConfig.Packet.OVERLAPPED)));
                    try
                    {
                        Marshal.StructureToPtr(new Operate.PacketConfig.Packet.OVERLAPPED { EventHandle = signal }, overlap, false);
                        fixed (byte* data = bytes)
                        {
                            var buffer = new Operate.PacketConfig.Packet.WSABUF { len = bytes.Length, buf = (IntPtr)data };
                            SocketError result = WS2_32.WSASend((int)pair.Client, (IntPtr)(&buffer), 1, IntPtr.Zero,
                                SocketFlags.None, overlap, IntPtr.Zero);
                            int error = Marshal.GetLastWin32Error();
                            if (result == SocketError.Success || error == (int)SocketError.IOPending)
                            {
                                if (WaitForSingleObject(signal, 3000) != 0) { Native.closesocket(pair.Client); WaitForSingleObject(signal, 3000); }
                                SocketFlags flags;
                                completed = GetOverlappedResult((int)pair.Client, overlap, out transferred, false, out flags);
                            }
                        }
                    }
                    finally { Marshal.FreeHGlobal(overlap); CloseHandle(signal); }
                });
                byte[] received = pair.ServerReadFor(300);
                rep.AppendLine("- 完成通知 " + completed + "，完成字节 " + transferred + "，对端 " + Ascii(received));
                return completed && transferred == bytes.Length && received.SequenceEqual(bytes) && Ascii(bytes) == "HELLO-ASYNC";
            }
        }

        private static bool CaseSendChangeLonger()
        {
            AddFilter("换长", FA.Change, SEARCH_LONG, ModifyOf("ABCDEFGHIJKLMNOPQRST"), SendFunctions());

            using (var pair = TcpPair.Open())
            {
                SetNonBlocking(pair.Client, true);
                int r = 0;
                RunClient(() => r = SendBytes(pair.Client, Encoding.ASCII.GetBytes("LONGPAYLOAD1"), PT.WS2_Send));
                byte[] got = pair.ServerReadFor(600);

                rep.AppendLine("- send 返回 " + r + "（必须 12），对端收到 " + Ascii(got) + "（必须原包 LONGPAYLOAD1）");
                return r == 12 && Ascii(got) == "LONGPAYLOAD1";
            }
        }

        private static bool CaseWsaSendChangeLonger()
        {
            AddFilter("换长", FA.Change, SEARCH_LONG, ModifyOf("ABCDEFGHIJKLMNOPQRST"), SendFunctions());

            using (var pair = TcpPair.Open())
            {
                SetNonBlocking(pair.Client, true);
                SocketError err = SocketError.SocketError; int sent = -1;
                RunClient(() => sent = WsaSend(pair.Client, new[] { Encoding.ASCII.GetBytes("LONGPAYLOAD1") }, out err));
                byte[] got = pair.ServerReadFor(600);

                rep.AppendLine("- WSASend " + err + "，报已发 " + sent + "（必须 12），对端收到 " + Ascii(got));
                return err == SocketError.Success && sent == 12 && Ascii(got) == "LONGPAYLOAD1";
            }
        }

        private static bool CaseRecvChangeLonger()
        {
            AddFilter("收包换长", FA.Change, SEARCH_LONG, ModifyOf("ABCDEFGHIJKLMNOPQRST"), RecvFunctions());

            using (var pair = TcpPair.Open())
            {
                pair.ServerSend("LONG5");
                Thread.Sleep(200);

                int r = 0; string text = null;
                RunClient(() =>
                {
                    byte[] buf = new byte[512];
                    r = RecvBytes(pair.Client, buf, PT.WS2_Recv);
                    text = r > 0 ? Encoding.ASCII.GetString(buf, 0, r) : null;
                });

                rep.AppendLine("- 收到 5 字节，滤镜换成 20 字节，缓冲区 512");
                rep.AppendLine("- recv 返回 " + r + "，内容 " + (text ?? "(无)"));
                return r == 20 && text == "ABCDEFGHIJKLMNOPQRST";
            }
        }

        #endregion

        #region//A9 / A10 / A11 多缓冲区与 0 字节

        private static bool CaseWsaSendMultiChange()
        {
            AddFilter("多缓冲换包", FA.Change, "0|41", ModifyOf("XYZXYZXYZXYZXYZ"), SendFunctions());

            using (var pair = UdpPair.Open())
            {
                var destination = Native.MakeAddr(LOOPBACK, pair.ServerPort);
                Native.connect(pair.Client, ref destination, 16);
                SocketError err = SocketError.SocketError; int sent = -1;
                RunClient(() => sent = WsaSend(pair.Client, new[]
                {
                    Encoding.ASCII.GetBytes("AAAAAAAAAA"),
                    Encoding.ASCII.GetBytes("BBBBBBBBBB"),
                    Encoding.ASCII.GetBytes("CCCCCCCCCC"),
                }, out err));
                string got = pair.ServerRecvFor(600);

                rep.AppendLine("- 3 块各 10 字节，滤镜换成 15 字节");
                rep.AppendLine("- WSASend " + err + "，报已发 " + sent + "（必须 30），对端收到 " + got + "（必须 15 字节）");
                return err == SocketError.Success && sent == 30 && got == "XYZXYZXYZXYZXYZ";
            }
        }

        private static bool CaseWsaRecvMultiShorter()
        {
            AddFilter("多缓冲收包换短", FA.Change, "0|57", ModifyOf("abcde"), RecvFunctions());

            using (var pair = TcpPair.Open())
            {
                pair.ServerSend("WXYZ12345678");
                Thread.Sleep(200);

                SocketError err = SocketError.SocketError; int got = -1;
                byte[] b0 = new byte[8], b1 = new byte[8];
                RunClient(() => got = WsaRecv(pair.Client, new[] { b0, b1 }, out err));

                string text = got > 0 ? Encoding.ASCII.GetString(b0.Concat(b1).Take(got).ToArray()) : "(无)";
                rep.AppendLine("- 两块各 8 字节，收到 12 字节，滤镜换成 abcde");
                rep.AppendLine("- WSARecv " + err + "，字节数 " + got + "，内容 " + text);
                return err == SocketError.Success && got == 5 && text == "abcde";
            }
        }

        private static bool CaseWsaSendZero()
        {
            using (var pair = TcpPair.Open())
            {
                SocketError err = SocketError.SocketError; int sent = -1;
                RunClient(() => sent = WsaSend(pair.Client, new[] { new byte[0] }, out err));
                rep.AppendLine("- 0 字节 WSASend 结果 " + err + "，已发 " + sent);
                return err == SocketError.Success && sent == 0;
            }
        }

        #endregion

        #region//A12 替换不改写程序自己的发送缓冲区

        private const string SEARCH_HELLO = "0|48,1|45,2|4C,3|4C,4|4F";
        private const string MODIFY_XXXXX = "0|58,1|58,2|58,3|58,4|58";

        private static bool CaseSendKeepsCallerBuffer()
        {
            AddFilter("替换", FA.Replace, SEARCH_HELLO, MODIFY_XXXXX, SendFunctions());

            using (var pair = TcpPair.Open())
            {
                byte[] mine = Encoding.ASCII.GetBytes("HELLOWORLD");
                int r = 0;
                RunClient(() => r = SendBytes(pair.Client, mine, PT.WS2_Send));
                byte[] got = pair.ServerReadFor(600);

                rep.AppendLine("- 对端收到 " + Ascii(got) + "（必须 XXXXXWORLD），程序自己的缓冲区现在是 " + Ascii(mine) + "（必须仍是 HELLOWORLD）");
                return r == 10 && Ascii(got) == "XXXXXWORLD" && Ascii(mine) == "HELLOWORLD";
            }
        }

        private static bool CaseWsaSendKeepsCallerBuffer()
        {
            AddFilter("替换", FA.Replace, SEARCH_HELLO, MODIFY_XXXXX, SendFunctions());

            using (var pair = TcpPair.Open())
            {
                byte[] mine = Encoding.ASCII.GetBytes("HELLOWORLD");
                SocketError err = SocketError.SocketError; int sent = -1;
                RunClient(() => sent = WsaSend(pair.Client, new[] { mine }, out err));
                byte[] got = pair.ServerReadFor(600);

                rep.AppendLine("- WSASend " + err + "，对端收到 " + Ascii(got) + "，程序缓冲区 " + Ascii(mine));
                return err == SocketError.Success && sent == 10 && Ascii(got) == "XXXXXWORLD" && Ascii(mine) == "HELLOWORLD";
            }
        }

        #endregion

        #region//A4 滤镜触发的发送

        private static bool CaseFilterTriggeredSend()
        {
            using (var sink = TcpPair.Open())
            using (var trig = TcpPair.Open())
            {
                var packets = new BindingList<PacketInfo>();
                packets.Add(new PacketInfo
                {
                    PacketSocket = (int)sink.Client,
                    PacketType = PT.WS2_Send,
                    PacketFrom = string.Empty,
                    PacketTo = string.Empty,
                    PacketBuffer = Encoding.ASCII.GetBytes("EXEC"),
                });

                var si = new SendInfo(true, Guid.NewGuid(), "滤镜触发", false, 100000, 20, packets, "");
                Operate.SendConfig.List.lstSendInfo.Add(si);

                var fi = AddFilter("触发发送", FA.NoModify_Display, "0|54,1|52,2|49,3|47", "", SendFunctions()); //"TRIG"
                fi.IsExecute = true;
                fi.FEType = Operate.FilterConfig.Filter.FilterExecuteType.Send;
                fi.Execute_GUID = si.SID;

                RunClient(() =>
                {
                    for (int i = 0; i < 20; i++) { SendBytes(trig.Client, Encoding.ASCII.GetBytes("TRIG"), PT.WS2_Send); }
                });
                trig.ServerReadFor(300);

                sink.ServerReadFor(800);                     //预热
                int running = sink.ServerReadFor(1000).Length; //一秒内的量

                Operate.SendConfig.List.StopSendList();
                Thread.Sleep(300);
                sink.ServerReadFor(200);
                int after = sink.ServerReadFor(700).Length;

                rep.AppendLine("- 20 个包命中「执行发送」（发送集间隔 20ms、循环 10 万次）");
                rep.AppendLine("- 运行中 1 秒对端收到 " + running + " 字节（一个执行器约 200，必须 < 600）");
                rep.AppendLine("- 停止发送列表之后 700ms 内收到 " + after + " 字节（必须 0）");
                return running > 0 && running < 600 && after == 0;
            }
        }

        #endregion

        #region//A16 SendPacket 部分发送时补发

        private static bool CaseSendPacketFull()
        {
            const int total = 8 * 1024 * 1024;
            IntPtr listen = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
            var addr = Native.MakeAddr(LOOPBACK, 0);
            Native.bind(listen, ref addr, 16);
            SetSockOptInt(listen, SO_RCVBUF, 4096);
            Native.listen(listen, 1);
            int len = 16;
            var bound = new Operate.PacketConfig.Packet.SockAddr();
            Native.getsockname(listen, ref bound, ref len);

            long received = 0;
            var reader = new Thread(() =>
            {
                IntPtr conn = Native.accept(listen, IntPtr.Zero, IntPtr.Zero);
                Thread.Sleep(800);
                SetSockOptInt(conn, SO_RCVTIMEO, 1500);
                byte[] buf = new byte[65536];
                while (true)
                {
                    int r = RawRecv(conn, buf);
                    if (r <= 0) { break; }
                    received += r;
                }
                Native.closesocket(conn);
            }) { IsBackground = true };
            reader.Start();

            //主线程 = 不被钩的线程，直接调 SendPacket 本身
            IntPtr c = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
            var to = Native.MakeAddr(LOOPBACK, Native.ntohs(bound.sin_port));
            Native.connect(c, ref to, 16);
            SetSockOptInt(c, SO_SNDBUF, 4096);
            SetNonBlocking(c, true);

            byte[] payload = new byte[total];
            bool ok = Operate.PacketConfig.Packet.SendPacket((int)c, PT.WS2_Send, "", "", payload);
            Native.closesocket(c);

            reader.Join(20000);
            Native.closesocket(listen);

            rep.AppendLine("- 非阻塞套接字、发送缓冲 4 KB、对端 800ms 后才开始读，SendPacket 发 8 MB");
            rep.AppendLine("- SendPacket 返回 " + ok + "，对端共收到 " + received + " 字节（必须 " + total + "）");
            return ok && received == total;
        }

        #endregion

        #region//B6 / B7 空地址与错误码

        private static bool CaseSendToNullAddr()
        {
            AddFilter("看一眼", FA.NoModify_Display, "0|55", "", SendFunctions());

            using (var pair = UdpPair.Open())
            {
                int r = 0; SocketError wsaErr = SocketError.SocketError; int wsaSent = -1;
                RunClient(() =>
                {
                    ConnectUdp(pair.Client, pair.ServerPort);
                    byte[] b = Encoding.ASCII.GetBytes("U-NULLTO");
                    r = SendToNull(pair.Client, b);
                    wsaSent = WsaSendToNull(pair.Client, Encoding.ASCII.GetBytes("U-WSANULL"), out wsaErr);
                });

                string a = pair.ServerRecvFor(500), b2 = pair.ServerRecvFor(500);
                rep.AppendLine("- 已 connect 的 UDP：sendto(to=NULL) 返回 " + r + "，WSASendTo(lpTo=NULL) " + wsaErr + " / " + wsaSent);
                rep.AppendLine("- 对端收到: " + (a ?? "(无)") + " | " + (b2 ?? "(无)"));
                return r == 8 && wsaErr == SocketError.Success && wsaSent == 9 && a == "U-NULLTO" && b2 == "U-WSANULL";
            }
        }

        private static bool CaseWsaRecvFromNullAddr()
        {
            AddFilter("收包替换", FA.Replace, SEARCH_HELLO, MODIFY_XXXXX, RecvFunctions());

            using (var pair = UdpPair.Open())
            {
                pair.ServerSendToClient("HELLOWORLD");
                Thread.Sleep(200);

                SocketError err = SocketError.SocketError; int got = -1; string text = null;
                RunClient(() =>
                {
                    byte[] buf = new byte[512];
                    got = WsaRecvFromNull(pair.Client, buf, out err);
                    text = got > 0 ? Encoding.ASCII.GetString(buf, 0, got) : null;
                });

                rep.AppendLine("- WSARecvFrom(lpFrom=NULL) " + err + "，字节数 " + got + "，内容 " + (text ?? "(无)") + "（必须 XXXXXWORLD）");
                return err == SocketError.Success && got == 10 && text == "XXXXXWORLD";
            }
        }

        private static bool CaseLastErrorPreserved()
        {
            //没有任何滤镜：纯粹看钩子体跑完之后错误码还在不在
            AddFilter("不相干", FA.NoModify_Display, "0|FF,1|FE,2|FD", "", RecvFunctions());

            using (var pair = TcpPair.Open())
            {
                int r = 0, err = 0, wsaErr = 0; SocketError wr = SocketError.Success;
                RunClient(() =>
                {
                    SetNonBlocking(pair.Client, true);
                    byte[] buf = new byte[64];
                    r = RecvBytes(pair.Client, buf, PT.WS2_Recv);
                    err = Marshal.GetLastWin32Error();
                    int ignore; wr = WsaRecvRaw(pair.Client, buf, out ignore);
                    wsaErr = Marshal.GetLastWin32Error();
                });

                rep.AppendLine("- 非阻塞、没有数据：recv 返回 " + r + " 错误码 " + err + "；WSARecv " + wr + " 错误码 " + wsaErr + "（都必须 10035）");
                return r == -1 && err == 10035 && wsaErr == 10035;
            }
        }

        #endregion

        #region//不需要钩子的几条

        private sealed class FailingHost : IHookHost
        {
            public bool FailFilter;
            public void OnPacket(int socket, byte[] raw, byte[] modified, int res, PT type, FA action,
                Operate.PacketConfig.Packet.SockAddr address, DateTime time) { throw new InvalidOperationException("test packet sink"); }
            public void OnLog(string name, string content) { throw new InvalidOperationException("test log sink"); }
            public void OnFilterLog(string name, FA action, int count, PT type, int length)
            { if (FailFilter) { throw new InvalidOperationException("test filter sink"); } }
            public void OnStore(Guid id, byte[] bytes) { }
            public SelectedPacket GetSelectedPacket() { return null; }
        }

        [DllImport("kernel32.dll")]
        private static extern void SetLastError(uint error);

        private sealed class InspectHost : IHookHost
        {
            public int Packets;
            public bool Shared = true, Utc = true;
            public void OnPacket(int socket, byte[] raw, byte[] modified, int res, PT type, FA action,
                Operate.PacketConfig.Packet.SockAddr address, DateTime time)
            { Packets++; Shared &= ReferenceEquals(raw, modified); Utc &= time.Kind == DateTimeKind.Utc; SetLastError(1234); }
            public void OnLog(string name, string content) { SetLastError(4321); }
            public void OnFilterLog(string name, FA action, int count, PT type, int length) { }
            public void OnStore(Guid id, byte[] bytes) { }
            public SelectedPacket GetSelectedPacket() { return null; }
        }

        private static bool CaseClobberedError()
        {
            var saved = HookHost.Current;
            var inspect = new InspectHost();
            HookHost.Attach(inspect);
            int result = 0, error = 0;
            try
            {
                RunClient(() => { result = SendBytes(new IntPtr(-1), new byte[] { 1 }, PT.WS2_Send); error = Marshal.GetLastWin32Error(); });
                rep.AppendLine("- 无效套接字 send 返回 " + result + "，错误 " + error + "，诊断回调 " + inspect.Packets);
                return result == -1 && error == (int)SocketError.NotSocket && inspect.Packets > 0;
            }
            finally { HookHost.Attach(saved); }
        }

        private static bool CaseBufferReuse()
        {
            var saved = HookHost.Current;
            var inspect = new InspectHost();
            HookHost.Attach(inspect);
            try
            {
                using (var pair = TcpPair.Open())
                {
                    RunClient(() => SendBytes(pair.Client, Encoding.ASCII.GetBytes("UNCHANGED"), PT.WS2_Send));
                    byte[] got = pair.ServerReadFor(200);
                    rep.AppendLine("- 相同数组 " + inspect.Shared + "，UTC " + inspect.Utc + "，收到 " + Ascii(got));
                    return inspect.Packets > 0 && inspect.Shared && inspect.Utc && Ascii(got) == "UNCHANGED";
                }
            }
            finally { HookHost.Attach(saved); }
        }

        private static bool CaseParsedRules()
        {
            var rule = AddFilter("解析语义", FA.Replace, "0|A*,1|*B", "2|10,2|20", SendFunctions());
            byte[] data = { 0xA1, 0x2B, 0x00 };
            bool before = Operate.FilterConfig.Filter.CheckFilter_IsMatch_Normal(rule, data);
            bool replaced = Operate.FilterConfig.Filter.Replace_Normal(rule, data) && data[2] == 0x20;
            rule.FSearch = "0|B*";
            bool invalidated = !Operate.FilterConfig.Filter.CheckFilter_IsMatch_Normal(rule, data);
            rule.ExcludePosition = "0";
            bool excluded = Operate.FilterConfig.Filter.CheckFilter_IsMatch_Normal(rule, data);
            rule.ExcludePosition = "";
            rule.FSearch = "0|ZZ";
            bool invalid = !Operate.FilterConfig.Filter.CheckFilter_IsMatch_Normal(rule, data);
            rule.FSearch = "2|A*,4|*B";
            var matches = Operate.FilterConfig.Filter.CheckFilter_IsMatch_Advanced(rule, new byte[] { 0, 0xA1, 0, 0x2B });
            bool advanced = matches.Count == 1 && matches[0] == 1;
            bool ranges = Operate.FilterConfig.Filter.CheckPacket_IsMatch_AppointLength(12, "4;10-20") &&
                !Operate.FilterConfig.Filter.CheckPacket_IsMatch_AppointLength(25, "4;10-20") &&
                Operate.FilterConfig.Filter.CheckPacket_IsMatch_AppointSocket(17, "5;17;bad");
            rep.AppendLine("- 通配 " + before + "，修改顺序 " + replaced + "，编辑失效 " + invalidated +
                "，排除 " + excluded + "，非法规则 " + invalid + "，高级相对位置 " + advanced + "，范围 " + ranges);
            return before && replaced && invalidated && excluded && invalid && advanced && ranges;
        }

        private static bool CaseFailingHost()
        {
            var saved = HookHost.Current;
            bool ok = true;
            try
            {
                foreach (bool wsa in new[] { false, true })
                foreach (int phase in new[] { 0, 1, 2 })
                {
                    Operate.FilterConfig.List.lstFilterInfo.Clear();
                    AddFilter("故障注入", phase == 2 ? FA.Intercept : FA.NoModify_Display, "0|48", "", SendFunctions());
                    HookHost.Attach(new FailingHost { FailFilter = phase == 0 });
                    using (var pair = TcpPair.Open())
                    {
                        byte[] payload = Encoding.ASCII.GetBytes("HELLO-ONCE");
                        int sent = -1;
                        RunClient(() =>
                        {
                            if (wsa) { SocketError error; sent = WsaSend(pair.Client, new[] { payload }, out error); }
                            else { sent = SendBytes(pair.Client, payload, PT.WS2_Send); }
                        });
                        byte[] received = pair.ServerReadFor(300);
                        bool passed = sent == payload.Length &&
                            (phase == 2 ? received.Length == 0 : received.SequenceEqual(payload));
                        rep.AppendLine("- " + (wsa ? "WSASend" : "send") + " 故障阶段 " + phase +
                            "：返回 " + sent + "，对端 " + received.Length + " 字节，" + passed);
                        ok &= passed;
                    }
                }
            }
            finally { HookHost.Attach(saved); }
            return ok;
        }

        private static bool CaseRandomConcurrent()
        {
            const int threads = 8, per = 3000000;
            int done = 0;
            var ts = Enumerable.Range(0, threads).Select(_ => new Thread(() =>
            {
                for (int i = 0; i < per; i++) { Operate.SystemConfig.GetRandomHexByte(0); }
                Interlocked.Increment(ref done);
            }) { IsBackground = true }).ToList();

            ts.ForEach(t => t.Start());
            var sw = System.Diagnostics.Stopwatch.StartNew();
            foreach (var t in ts) { t.Join(Math.Max(1, 60000 - (int)sw.ElapsedMilliseconds)); }

            var distinct = new HashSet<byte>();
            if (done == threads)
            {
                for (int i = 0; i < 2000; i++) { distinct.Add(Operate.SystemConfig.GetRandomHexByte(1)); }
            }

            rep.AppendLine("- " + threads + " 线程各调 " + per + " 次 GetRandomHexByte(0)：跑完 " + done + " 条线程，用时 " + sw.ElapsedMilliseconds + "ms");
            rep.AppendLine("- 之后 2000 次取值的不同值个数 " + distinct.Count + "（Random 没坏的话接近 255）");
            return done == threads && distinct.Count > 200;
        }

        private static bool CaseFilterRecursion()
        {
            var self = AddFilter("自己执行自己", FA.NoModify_Display, "0|41", "", SendFunctions());
            self.IsExecute = true;
            self.FEType = Operate.FilterConfig.Filter.FilterExecuteType.Filter;
            self.Execute_GUID = self.FID;

            byte[] buf = { 0x41 };
            byte[] nb;
            FA a1 = Operate.FilterConfig.List.DoFilterList(0, buf, out nb, PT.WS2_Send, new Operate.PacketConfig.Packet.SockAddr());
            rep.AppendLine("- A 执行 A：返回 " + a1);

            Operate.FilterConfig.List.lstFilterInfo.Clear();
            var fa = AddFilter("A", FA.NoModify_Display, "0|41", "", SendFunctions());
            var fb = AddFilter("B", FA.NoModify_Display, "0|41", "", SendFunctions());
            fa.IsExecute = true; fa.FEType = Operate.FilterConfig.Filter.FilterExecuteType.Filter; fa.Execute_GUID = fb.FID;
            fb.IsExecute = true; fb.FEType = Operate.FilterConfig.Filter.FilterExecuteType.Filter; fb.Execute_GUID = fa.FID;

            FA a2 = Operate.FilterConfig.List.DoFilterList(0, buf, out nb, PT.WS2_Send, new Operate.PacketConfig.Packet.SockAddr());
            rep.AppendLine("- A → B → A：返回 " + a2 + "（能走到这一行就说明没有栈溢出）");
            return a1 == FA.NoModify_Display && a2 == FA.NoModify_Display;
        }

        private static bool CaseProgressionRace()
        {
            Operate.SystemConfig.SpeedMode = true; //不记滤镜日志，只看计数
            var fi = AddFilter("递进", FA.Replace, "1|FF", "", SendFunctions());
            fi.ProgressionPosition = "0";
            fi.ProgressionStep = 1;
            fi.IsProgressionContinuous = true;

            const int threads = 4, per = 20000;
            var ts = Enumerable.Range(0, threads).Select(_ => new Thread(() =>
            {
                for (int i = 0; i < per; i++)
                {
                    byte[] b = { 0x00, 0xFF };
                    byte[] nb;
                    Operate.FilterConfig.List.DoFilterList(0, b, out nb, PT.WS2_Send, new Operate.PacketConfig.Packet.SockAddr());
                }
            }) { IsBackground = true }).ToList();
            ts.ForEach(t => t.Start());
            ts.ForEach(t => t.Join(60000));

            long expect = threads * per;
            rep.AppendLine("- " + threads + " 线程各 " + per + " 次命中同一条连续递进滤镜");
            rep.AppendLine("- 执行次数 " + fi.ExecutionCount + "、递进计数 " + fi.ProgressionCount + "（都必须 " + expect + "）");
            return fi.ExecutionCount == expect && fi.ProgressionCount == expect;
        }

        private static bool CaseAdvancedWildcardFirst()
        {
            var fi = AddFilter("高级通配", FA.NoModify_Display, "0|*1,1|42", "", SendFunctions());
            fi.FMode = Operate.FilterConfig.Filter.FilterMode.Advanced;

            var hit = Operate.FilterConfig.Filter.CheckFilter_IsMatch_Advanced(fi, new byte[] { 0x99, 0x31, 0x42 });
            var miss = Operate.FilterConfig.Filter.CheckFilter_IsMatch_Advanced(fi, new byte[] { 0x99, 0x32, 0x43 });
            rep.AppendLine("- 条件 `0|*1,1|42`：99 31 42 命中位置 [" + string.Join(",", hit) + "]（必须 [1]），99 32 43 命中 " + miss.Count + " 处（必须 0）");
            return hit.Count == 1 && hit[0] == 1 && miss.Count == 0;
        }

        private static bool CasePortFilterStillWorks()
        {
            //B 批把「指定端口」改成直接读 sockaddr / 按套接字缓存，结果必须与原来一致
            IntPtr listen = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
            var addr = Native.MakeAddr(LOOPBACK, 0);
            Native.bind(listen, ref addr, 16);
            Native.listen(listen, 1);
            int len = 16;
            var bound = new Operate.PacketConfig.Packet.SockAddr();
            Native.getsockname(listen, ref bound, ref len);
            ushort port = Native.ntohs(bound.sin_port);

            IntPtr c = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
            var to = Native.MakeAddr(LOOPBACK, port);
            Native.connect(c, ref to, 16);
            IntPtr conn = Native.accept(listen, IntPtr.Zero, IntPtr.Zero);

            var fi = AddFilter("端口", FA.NoModify_Display, "0|41", "", SendFunctions());
            fi.AppointPort = true;
            fi.PortContent = port.ToString();

            byte[] nb;
            FA hit = Operate.FilterConfig.List.DoFilterList((int)c, new byte[] { 0x41 }, out nb, PT.WS2_Send, new Operate.PacketConfig.Packet.SockAddr());
            fi.PortContent = (port == 1 ? 2 : 1).ToString();
            FA miss = Operate.FilterConfig.List.DoFilterList((int)c, new byte[] { 0x41 }, out nb, PT.WS2_Send, new Operate.PacketConfig.Packet.SockAddr());
            fi.PortContent = "1-65535";
            FA range = Operate.FilterConfig.List.DoFilterList((int)c, new byte[] { 0x41 }, out nb, PT.WS2_Send, new Operate.PacketConfig.Packet.SockAddr());

            var udpTo = Native.MakeAddr(LOOPBACK, 5353);
            fi.PortContent = "5353";
            FA udp = Operate.FilterConfig.List.DoFilterList((int)c, new byte[] { 0x41 }, out nb, PT.WS2_SendTo, udpTo);

            Native.closesocket(conn); Native.closesocket(c); Native.closesocket(listen);

            rep.AppendLine("- 指定对端端口 " + port + " → " + hit + "；指定别的端口 → " + miss + "；1-65535 → " + range + "；sendto 目标 5353 → " + udp);
            return hit == FA.NoModify_Display && miss == FA.None && range == FA.NoModify_Display && udp == FA.NoModify_Display;
        }

        private static bool CaseSnapshotKeepsProgression()
        {
            var fi = AddFilter("连续递进", FA.Replace, "1|FF", "", SendFunctions());
            fi.ProgressionPosition = "0";
            fi.ProgressionStep = 1;
            fi.IsProgressionContinuous = true;

            //外壳那份永远是 0 —— 这就是推下去的载荷
            byte[] payload = WinsockPacketEditor.Ipc.ConfigSnapshot.EncodeFilters();

            //目标里已经递进了 7 次
            fi.ProgressionCount = 7;
            WinsockPacketEditor.Ipc.ConfigSnapshot.ApplyFilters(payload);
            int kept = Operate.FilterConfig.List.lstFilterInfo[0].ProgressionCount;

            //改了递进位置：应当从头算
            Operate.FilterConfig.List.lstFilterInfo[0].ProgressionCount = 7;
            fi = Operate.FilterConfig.List.lstFilterInfo[0];
            fi.ProgressionPosition = "1";
            byte[] payload2 = WinsockPacketEditor.Ipc.ConfigSnapshot.EncodeFilters(); //编码时位置已变、计数 7
            Operate.FilterConfig.List.lstFilterInfo[0].ProgressionPosition = "0";     //目标里还是旧位置
            WinsockPacketEditor.Ipc.ConfigSnapshot.ApplyFilters(ZeroCountIn(payload2));
            int reset = Operate.FilterConfig.List.lstFilterInfo[0].ProgressionCount;

            rep.AppendLine("- 目标里递进到 7，外壳推来同一条滤镜（计数 0）→ 应用后 " + kept + "（必须 7）");
            rep.AppendLine("- 外壳改了递进位置再推 → 应用后 " + reset + "（必须 0）");
            return kept == 7 && reset == 0;
        }

        /// <summary>把编码好的快照里那条滤镜的 ProgressionCount 改回 0（模拟外壳那份计数）。</summary>
        private static byte[] ZeroCountIn(byte[] payload)
        {
            Operate.FilterConfig.List.lstFilterInfo[0].ProgressionCount = 0;
            Operate.FilterConfig.List.lstFilterInfo[0].ProgressionPosition = "1";
            byte[] p = WinsockPacketEditor.Ipc.ConfigSnapshot.EncodeFilters();
            Operate.FilterConfig.List.lstFilterInfo[0].ProgressionCount = 7;
            Operate.FilterConfig.List.lstFilterInfo[0].ProgressionPosition = "0";
            return p;
        }

        #endregion

        #region//滤镜

        private static FilterInfo AddFilter(string name, FA action, string search, string modify,
            Operate.FilterConfig.Filter.FilterFunction ff)
        {
            Operate.FilterConfig.Filter.AddFilter(
                true, Guid.NewGuid(), name,
                false, "", false, "", false, "", false, "",
                Operate.FilterConfig.Filter.FilterMode.Normal,
                action,
                false, Operate.FilterConfig.Filter.FilterExecuteType.None, Guid.Empty,
                ff,
                Operate.FilterConfig.Filter.FilterStartFrom.Head,
                false, false, 1, false, 0, "", 0, "", "",
                search, modify);

            var list = Operate.FilterConfig.List.lstFilterInfo;
            return list[list.Count - 1];
        }

        private static List<PacketInfo> Drain()
        {
            var list = new List<PacketInfo>();
            while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out PacketInfo pi)) { list.Add(pi); }
            return list;
        }

        #endregion

        #region//客户端线程

        private static Thread StartClient(Action body)
        {
            var t = new Thread(() =>
            {
                try { body(); }
                catch (Exception ex) { Console.Error.WriteLine("    客户端异常: " + ex.Message); }
            }) { IsBackground = true };
            t.Start();
            return t;
        }

        private static void RunClient(Action body)
        {
            if (!StartClient(body).Join(15000)) { rep.AppendLine("- 客户端 15 秒没返回"); }
        }

        #endregion

        #region//被钩的收发（走主工程的导入）

        private static unsafe int SendBytes(IntPtr s, byte[] b, PT type)
        {
            fixed (byte* p = b)
            {
                return type == PT.WS1_Send
                    ? WSock32.send((int)s, (IntPtr)p, b.Length, SocketFlags.None)
                    : WS2_32.send((int)s, (IntPtr)p, b.Length, SocketFlags.None);
            }
        }

        private static unsafe int RecvBytes(IntPtr s, byte[] buf, PT type)
        {
            fixed (byte* p = buf)
            {
                return type == PT.WS1_Recv
                    ? WSock32.recv((int)s, (IntPtr)p, buf.Length, SocketFlags.None)
                    : WS2_32.recv((int)s, (IntPtr)p, buf.Length, SocketFlags.None);
            }
        }

        private static unsafe int RecvFromBytes(IntPtr s, byte[] buf, PT type)
        {
            var from = new Operate.PacketConfig.Packet.SockAddr();
            IntPtr pLen = Marshal.AllocHGlobal(4);
            try
            {
                Marshal.WriteInt32(pLen, 16);
                fixed (byte* p = buf) { return WS2_32.recvfrom((int)s, (IntPtr)p, buf.Length, SocketFlags.None, ref from, pLen); }
            }
            finally { Marshal.FreeHGlobal(pLen); }
        }

        /// <summary>WSASend，每个数组一块 WSABUF。返回报告的已发字节。</summary>
        private static int WsaSend(IntPtr s, byte[][] bufs, out SocketError err)
        {
            var handles = bufs.Select(b => GCHandle.Alloc(b, GCHandleType.Pinned)).ToArray();
            int sz = Marshal.SizeOf(typeof(Operate.PacketConfig.Packet.WSABUF));
            IntPtr arr = Marshal.AllocHGlobal(sz * bufs.Length);
            IntPtr pSent = Marshal.AllocHGlobal(4);
            try
            {
                for (int i = 0; i < bufs.Length; i++)
                {
                    Marshal.StructureToPtr(new Operate.PacketConfig.Packet.WSABUF { len = bufs[i].Length, buf = handles[i].AddrOfPinnedObject() }, arr + sz * i, false);
                }
                Marshal.WriteInt32(pSent, -1);
                err = WS2_32.WSASend((int)s, arr, bufs.Length, pSent, SocketFlags.None, IntPtr.Zero, IntPtr.Zero);
                return Marshal.ReadInt32(pSent);
            }
            finally
            {
                Marshal.FreeHGlobal(arr); Marshal.FreeHGlobal(pSent);
                foreach (var h in handles) { h.Free(); }
            }
        }

        private static int WsaRecv(IntPtr s, byte[][] bufs, out SocketError err)
        {
            var handles = bufs.Select(b => GCHandle.Alloc(b, GCHandleType.Pinned)).ToArray();
            int sz = Marshal.SizeOf(typeof(Operate.PacketConfig.Packet.WSABUF));
            IntPtr arr = Marshal.AllocHGlobal(sz * bufs.Length);
            IntPtr pGot = Marshal.AllocHGlobal(4);
            try
            {
                for (int i = 0; i < bufs.Length; i++)
                {
                    Marshal.StructureToPtr(new Operate.PacketConfig.Packet.WSABUF { len = bufs[i].Length, buf = handles[i].AddrOfPinnedObject() }, arr + sz * i, false);
                }
                Marshal.WriteInt32(pGot, -1);
                SocketFlags flags = SocketFlags.None;
                err = WS2_32.WSARecv((int)s, arr, bufs.Length, pGot, ref flags, IntPtr.Zero, IntPtr.Zero);
                return Marshal.ReadInt32(pGot);
            }
            finally
            {
                Marshal.FreeHGlobal(arr); Marshal.FreeHGlobal(pGot);
                foreach (var h in handles) { h.Free(); }
            }
        }

        private static SocketError WsaRecvRaw(IntPtr s, byte[] buf, out int got)
        {
            SocketError err;
            got = WsaRecv(s, new[] { buf }, out err);
            return err;
        }

        [DllImport("ws2_32.dll", SetLastError = true, EntryPoint = "sendto")]
        private static extern int sendto_null(int s, IntPtr buf, int len, SocketFlags flags, IntPtr to, int tolen);

        [DllImport("ws2_32.dll", SetLastError = true, EntryPoint = "WSASendTo")]
        private static extern SocketError WSASendTo_null(int s, IntPtr lpBuffers, int count, IntPtr lpSent, SocketFlags flags,
            IntPtr lpTo, int tolen, IntPtr lpOverlapped, IntPtr lpRoutine);

        [DllImport("ws2_32.dll", SetLastError = true, EntryPoint = "WSARecvFrom")]
        private static extern SocketError WSARecvFrom_null(int s, IntPtr lpBuffers, int count, IntPtr lpGot, ref SocketFlags flags,
            IntPtr lpFrom, IntPtr lpFromLen, IntPtr lpOverlapped, IntPtr lpRoutine);

        private static unsafe int SendToNull(IntPtr s, byte[] b)
        {
            fixed (byte* p = b) { return sendto_null((int)s, (IntPtr)p, b.Length, SocketFlags.None, IntPtr.Zero, 0); }
        }

        private static unsafe int WsaSendToNull(IntPtr s, byte[] b, out SocketError err)
        {
            IntPtr arr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Operate.PacketConfig.Packet.WSABUF)));
            IntPtr pSent = Marshal.AllocHGlobal(4);
            try
            {
                fixed (byte* p = b)
                {
                    Marshal.StructureToPtr(new Operate.PacketConfig.Packet.WSABUF { len = b.Length, buf = (IntPtr)p }, arr, false);
                    Marshal.WriteInt32(pSent, -1);
                    err = WSASendTo_null((int)s, arr, 1, pSent, SocketFlags.None, IntPtr.Zero, 0, IntPtr.Zero, IntPtr.Zero);
                    return Marshal.ReadInt32(pSent);
                }
            }
            finally { Marshal.FreeHGlobal(arr); Marshal.FreeHGlobal(pSent); }
        }

        private static unsafe int WsaRecvFromNull(IntPtr s, byte[] buf, out SocketError err)
        {
            IntPtr arr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Operate.PacketConfig.Packet.WSABUF)));
            IntPtr pGot = Marshal.AllocHGlobal(4);
            try
            {
                fixed (byte* p = buf)
                {
                    Marshal.StructureToPtr(new Operate.PacketConfig.Packet.WSABUF { len = buf.Length, buf = (IntPtr)p }, arr, false);
                    Marshal.WriteInt32(pGot, -1);
                    SocketFlags flags = SocketFlags.None;
                    err = WSARecvFrom_null((int)s, arr, 1, pGot, ref flags, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                    return Marshal.ReadInt32(pGot);
                }
            }
            finally { Marshal.FreeHGlobal(arr); Marshal.FreeHGlobal(pGot); }
        }

        private static void ConnectUdp(IntPtr s, ushort port)
        {
            var to = Native.MakeAddr(LOOPBACK, port);
            Native.connect(s, ref to, 16);
        }

        #endregion

        #region//不被钩的原始调用（服务端 / 设置）

        private const int SOL_SOCKET = 0xFFFF;
        private const int SO_SNDBUF = 0x1001;
        private const int SO_RCVBUF = 0x1002;
        private const int SO_RCVTIMEO = 0x1006;

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int setsockopt(IntPtr s, int level, int optname, ref int optval, int optlen);

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int ioctlsocket(IntPtr s, int cmd, ref uint argp);

        private static void SetSockOptInt(IntPtr s, int opt, int v)
        {
            if (setsockopt(s, SOL_SOCKET, opt, ref v, 4) != 0) { throw new SocketException(Marshal.GetLastWin32Error()); }
        }

        private static void SetNonBlocking(IntPtr s, bool on)
        {
            uint v = on ? 1u : 0u;
            if (ioctlsocket(s, unchecked((int)0x8004667E), ref v) != 0) { throw new SocketException(Marshal.GetLastWin32Error()); }
        }

        /// <summary>
        /// 服务端这一侧的收发只在<b>主线程</b>上调（不被钩）。
        /// ⚠️ 它们走的是主工程的导入 —— 不能在工作线程上用，否则自己也会被滤镜命中。
        /// </summary>
        private static unsafe int RawRecv(IntPtr s, byte[] buf)
        {
            fixed (byte* p = buf) { return WS2_32.recv((int)s, (IntPtr)p, buf.Length, SocketFlags.None); }
        }

        private static unsafe int RawSend(IntPtr s, byte[] b)
        {
            fixed (byte* p = b) { return WS2_32.send((int)s, (IntPtr)p, b.Length, SocketFlags.None); }
        }

        private sealed class TcpPair : IDisposable
        {
            public IntPtr Client, Server, Listen;

            public static TcpPair Open(bool smallWindow = false)
            {
                var p = new TcpPair();
                p.Listen = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
                if (smallWindow) { SetSockOptInt(p.Listen, SO_RCVBUF, 4096); }
                var addr = Native.MakeAddr(LOOPBACK, 0);
                Native.bind(p.Listen, ref addr, 16);
                Native.listen(p.Listen, 1);
                int len = 16;
                var bound = new Operate.PacketConfig.Packet.SockAddr();
                Native.getsockname(p.Listen, ref bound, ref len);

                //connect 不经 WinSock 的收发钩子，放主线程上做就行
                p.Client = Native.socket(Native.AF_INET, Native.SOCK_STREAM, Native.IPPROTO_TCP);
                var to = Native.MakeAddr(LOOPBACK, Native.ntohs(bound.sin_port));
                Native.connect(p.Client, ref to, 16);
                p.Server = Native.accept(p.Listen, IntPtr.Zero, IntPtr.Zero);
                return p;
            }

            public void ServerSend(string s) { RawSend(Server, Encoding.ASCII.GetBytes(s)); }

            /// <summary>在 ms 毫秒内能读到多少读多少。</summary>
            public byte[] ServerReadFor(int ms)
            {
                var all = new List<byte>();
                var sw = System.Diagnostics.Stopwatch.StartNew();
                byte[] buf = new byte[65536];
                while (true)
                {
                    int left = ms - (int)sw.ElapsedMilliseconds;
                    if (left <= 0) { break; }
                    SetSockOptInt(Server, SO_RCVTIMEO, left);
                    int r = RawRecv(Server, buf);
                    if (r <= 0) { break; }
                    all.AddRange(buf.Take(r));
                }
                return all.ToArray();
            }

            public void Dispose()
            {
                Native.closesocket(Client);
                Native.closesocket(Server);
                Native.closesocket(Listen);
            }
        }

        private sealed class UdpPair : IDisposable
        {
            public IntPtr Client, Server;
            public ushort ServerPort, ClientPort;

            public static UdpPair Open()
            {
                var p = new UdpPair();
                p.Server = Native.socket(Native.AF_INET, Native.SOCK_DGRAM, Native.IPPROTO_UDP);
                p.Client = Native.socket(Native.AF_INET, Native.SOCK_DGRAM, Native.IPPROTO_UDP);
                var a = Native.MakeAddr(LOOPBACK, 0);
                Native.bind(p.Server, ref a, 16);
                var b = Native.MakeAddr(LOOPBACK, 0);
                Native.bind(p.Client, ref b, 16);
                p.ServerPort = PortOf(p.Server);
                p.ClientPort = PortOf(p.Client);
                return p;
            }

            private static ushort PortOf(IntPtr s)
            {
                int len = 16;
                var bound = new Operate.PacketConfig.Packet.SockAddr();
                Native.getsockname(s, ref bound, ref len);
                return Native.ntohs(bound.sin_port);
            }

            public unsafe void ServerSendToClient(string s)
            {
                byte[] b = Encoding.ASCII.GetBytes(s);
                var to = Native.MakeAddr(LOOPBACK, ClientPort);
                fixed (byte* p = b) { WS2_32.sendto((int)Server, (IntPtr)p, b.Length, SocketFlags.None, ref to, 16); }
            }

            public unsafe string ServerRecvFor(int ms)
            {
                SetSockOptInt(Server, SO_RCVTIMEO, ms);
                byte[] buf = new byte[1024];
                var from = new Operate.PacketConfig.Packet.SockAddr();
                IntPtr pLen = Marshal.AllocHGlobal(4);
                try
                {
                    Marshal.WriteInt32(pLen, 16);
                    int r;
                    fixed (byte* p = buf) { r = WS2_32.recvfrom((int)Server, (IntPtr)p, buf.Length, SocketFlags.None, ref from, pLen); }
                    return r > 0 ? Encoding.ASCII.GetString(buf, 0, r) : null;
                }
                finally { Marshal.FreeHGlobal(pLen); }
            }

            public void Dispose()
            {
                Native.closesocket(Client);
                Native.closesocket(Server);
            }
        }

        #endregion

        #region//杂项

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
