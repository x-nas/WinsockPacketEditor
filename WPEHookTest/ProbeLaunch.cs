using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WinsockPacketEditor;
using WinsockPacketEditor.Ipc;

namespace WPEHookTest
{
    /// <summary>
    /// 「挂起启动」这条路的单项探针。
    ///
    /// 验证矩阵第 2 项一直是 0 条包，而 0 条包有太多种原因
    /// （命令行没传对 / 进程没醒 / 醒了又退了 / 钩子没装上……）。
    /// 这个模式把每一步的现场都打出来，一次看清楚。
    /// </summary>
    internal static class ProbeLaunch
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

            string exe = Process.GetCurrentProcess().MainModule.FileName;
            string cmd = "\"" + exe + "\" --target --seconds 40";

            Console.WriteLine("exe = " + exe);
            Console.WriteLine("cmd = " + cmd);

            var link = new ShellLink();
            link.StateChanged += s => Console.WriteLine("  状态 → " + s);

            Process target = null;

            try
            {
                link.Attach(-1, exe, 20000, cmd);
                Console.WriteLine("注入成功，PID = " + link.TargetPid);

                target = Process.GetProcessById(link.TargetPid);
                Dump(target, "刚注入完");

                Thread.Sleep(1500);
                Dump(target, "1.5 秒后（应当还挂着：CPU 基本不涨）");

                Console.WriteLine("下发 StartHook …");
                link.StartHook();
                Console.WriteLine("StartHook 应答回来了");

                for (int i = 1; i <= 6; i++)
                {
                    Thread.Sleep(1000);
                    Dump(target, i + " 秒后");
                    Console.WriteLine("    队列里 " + Operate.PacketConfig.Queue.cqPacketInfo.Count + " 条");
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("出错: " + ex);
                return 1;
            }
            finally
            {
                try { if (target != null && !target.HasExited) { target.Kill(); } } catch { }
                try { link.Dispose(); } catch { }
            }
        }

        private static void Dump(Process p, string when)
        {
            try
            {
                p.Refresh();

                if (p.HasExited)
                {
                    Console.WriteLine("  [" + when + "] 已退出，退出码 " + p.ExitCode +
                                      "（0x" + p.ExitCode.ToString("X8") + "）");
                    return;
                }

                Console.WriteLine("  [" + when + "] 在跑, 线程 " + p.Threads.Count +
                                  ", CPU " + p.TotalProcessorTime.TotalMilliseconds.ToString("F0") + " ms" +
                                  ", 工作集 " + (p.WorkingSet64 / 1024 / 1024) + " MB");
            }
            catch (Exception ex)
            {
                Console.WriteLine("  [" + when + "] 查不到: " + ex.Message);
            }
        }
    }
}
