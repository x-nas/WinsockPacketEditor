using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using WinsockPacketEditor;
using WinsockPacketEditor.Ipc;

namespace WPEHookTest
{
    /// <summary>
    /// 「挂起启动」这条路，换一个<b>原生</b>目标来验。
    ///
    /// 【为什么要单独验】用托管的 WPEHookTest.exe 当靶子时，挂起启动之后一唤醒
    /// 进程就没了（Main 一行都没跑到，留痕文件是空的）。怀疑是<b>托管目标特有的</b>：
    /// EasyHook 已经在这个进程里初始化了一份 CLR，而进程自己的运行时宿主随后又要初始化一遍。
    /// 真实场景里目标是游戏，全是原生进程。
    ///
    /// 这里拿系统自带的 curl.exe 当靶子（原生、会做 TCP 收发、能跑一段时间），
    /// 外壳自己起一个会「细水长流」的 HTTP 服务端喂它。
    /// </summary>
    internal static class ProbeNative
    {
        public static int Run(string[] args)
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

            string curl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "curl.exe");

            if (!File.Exists(curl))
            {
                Console.WriteLine("这台机器没有 " + curl + "，换个原生靶子再验");
                return 2;
            }

            //=== 细水长流的 HTTP 服务端：一次 accept，分 20 段慢慢吐 ===
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            Console.WriteLine("服务端端口 " + port);

            new Thread(() =>
            {
                try
                {
                    using (TcpClient c = listener.AcceptTcpClient())
                    using (NetworkStream ns = c.GetStream())
                    {
                        byte[] req = new byte[4096];
                        ns.Read(req, 0, req.Length);

                        byte[] head = Encoding.ASCII.GetBytes(
                            "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nTransfer-Encoding: chunked\r\n\r\n");
                        ns.Write(head, 0, head.Length);
                        ns.Flush();

                        for (int i = 0; i < 20; i++)
                        {
                            //⚠️ chunked 的长度必须是这一段的真实字节数（十六进制）。
                            //第一版写死成 10（=16）而正文只有 14 字节，curl 直接以 56 退出，
                            //看着像 WPE 把连接搞坏了 —— 其实是跑测自己的 bug。
                            string chunk = "NATIVE-BEAT-" + i.ToString("D2");
                            byte[] body = Encoding.ASCII.GetBytes(
                                chunk.Length.ToString("x") + "\r\n" + chunk + "\r\n");
                            ns.Write(body, 0, body.Length);
                            ns.Flush();
                            Thread.Sleep(300);
                        }

                        byte[] tail = Encoding.ASCII.GetBytes("0\r\n\r\n");
                        ns.Write(tail, 0, tail.Length);
                        ns.Flush();
                    }
                }
                catch (Exception ex) { Console.WriteLine("服务端: " + ex.Message); }
            })
            { IsBackground = true }.Start();

            var link = new ShellLink();
            link.StateChanged += s => Console.WriteLine("  状态 → " + s);

            try
            {
                //⚠️ 命令行以 exe 路径开头，见 ShellLink.Attach 的注释
                string cmd = "\"" + curl + "\" -s --max-time 25 http://127.0.0.1:" + port + "/";
                Console.WriteLine("cmd = " + cmd);

                link.Attach(-1, curl, 20000, cmd);
                Console.WriteLine("挂起启动 + 注入成功，PID = " + link.TargetPid);
                KeepHandle(link.TargetPid);

                Dump(link.TargetPid, "刚注入完（应当挂着）");
                Thread.Sleep(1500);
                Dump(link.TargetPid, "1.5 秒后（还应挂着）");

                Console.WriteLine("下发 StartHook（应答之后唤醒）…");
                link.StartHook();

                for (int i = 1; i <= 8; i++)
                {
                    Thread.Sleep(1000);
                    Dump(link.TargetPid, i + " 秒后");
                    ReportExitCode();
                    Console.WriteLine("    队列里 " + Operate.PacketConfig.Queue.cqPacketInfo.Count + " 条");
                }

                int n = 0;
                bool sawGet = false, sawBeat = false;

                while (Operate.PacketConfig.Queue.cqPacketInfo.TryDequeue(out PacketInfo pi))
                {
                    n++;
                    string s = Ascii(pi.PacketBuffer);
                    if (s.Contains("GET /")) { sawGet = true; }
                    if (s.Contains("NATIVE-BEAT")) { sawBeat = true; }
                    if (n <= 5) { Console.WriteLine("    " + pi.PacketType + " [" + pi.PacketLen + "] " + s.Substring(0, Math.Min(60, s.Length))); }
                }

                Console.WriteLine();
                Console.WriteLine("共 " + n + " 条；抓到 curl 发出的 GET: " + sawGet + "；抓到服务端回的 BEAT: " + sawBeat);

                bool ok = n > 0 && sawGet;
                Console.WriteLine(ok ? "结论: PASS" : "结论: FAIL");
                return ok ? 0 : 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("出错: " + ex);
                return 1;
            }
            finally
            {
                try { link.Dispose(); } catch { }
                try { Process.GetProcessById(link.TargetPid).Kill(); } catch { }
                try { listener.Stop(); } catch { }
            }
        }

        //=== 拿退出码：Process.GetProcessById 拿到的对象读 ExitCode 会抛
        //   「进程不是由此对象启动的」，只能自己开句柄 ===
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(int access, bool inherit, int pid);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetExitCodeProcess(IntPtr h, out uint code);

        private static IntPtr _hTarget = IntPtr.Zero;

        private static void KeepHandle(int pid)
        {
            //SYNCHRONIZE | PROCESS_QUERY_LIMITED_INFORMATION
            _hTarget = OpenProcess(0x00100000 | 0x1000, false, pid);
            Console.WriteLine("  句柄 = " + _hTarget);
        }

        private static void ReportExitCode()
        {
            if (_hTarget == IntPtr.Zero) { return; }

            uint code;
            if (GetExitCodeProcess(_hTarget, out code))
            {
                //259 = STILL_ACTIVE
                Console.WriteLine("  目标退出码 = " + code + " (0x" + code.ToString("X8") + ")" +
                                  (code == 259 ? " ← 还活着" : ""));
            }
            else
            {
                Console.WriteLine("  取退出码失败 " + Marshal.GetLastWin32Error());
            }
        }

        private static void Dump(int pid, string when)
        {
            try
            {
                var p = Process.GetProcessById(pid);
                p.Refresh();
                Console.WriteLine("  [" + when + "] 在跑, 线程 " + p.Threads.Count +
                                  ", CPU " + p.TotalProcessorTime.TotalMilliseconds.ToString("F0") + " ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine("  [" + when + "] 没了: " + ex.Message);
            }
        }

        private static string Ascii(byte[] b)
        {
            if (b == null) { return "(null)"; }
            var sb = new StringBuilder();
            foreach (byte x in b) { sb.Append(x >= 0x20 && x < 0x7F ? (char)x : '.'); }
            return sb.ToString();
        }
    }
}
