using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WinsockPacketEditor;
using WinsockPacketEditor.Ipc;

namespace WPEHookTest
{
    /// <summary>
    /// 量一次「无头核心到底往目标进程里塞了多少东西」。
    ///
    /// 【为什么要量】抽 WPECore（方案的阶段 3）唯一的收益就是缩小这个足迹 ——
    /// 风险清单里的 R2：AntdUI / EF6 / SQLite / OWIN / Newtonsoft / SunnyNet / InputSimulator
    /// 全进目标，目标若自带同名程序集（Newtonsoft 最常见）就是版本绑定冲突。
    ///
    /// 而那一步要拆一个三万行的类、解开四个模型与 AntdUI 的绑定，是三阶段里
    /// 工程量最大、回归风险最高的一步。<b>先量清楚再决定值不值</b>，别凭猜动手。
    /// </summary>
    internal static class Footprint
    {
        /// <summary>WPE 自己带的那些依赖 —— 它们出现在目标里就是 R2 说的检测面与冲突面。</summary>
        private static readonly string[] Watch =
        {
            "AntdUI", "Newtonsoft.Json", "EntityFramework", "System.Data.SQLite",
            "Microsoft.Owin", "Owin", "System.Web.Http", "SuperSocket",
            "QQWry", "WindowsInput", "DiffPlex", "Be.Windows.Forms.HexBox",
            "System.Windows.Forms", "System.Drawing",
        };

        private static readonly string[] WatchNative =
        {
            "SQLite.Interop.dll", "System.Data.SQLite.dll",
        };

        public static int Run(string[] args)
        {
            string outPath = GetArg(args, "--out", null);

            var rep = new StringBuilder();
            rep.AppendLine("# 无头核心在目标进程里的足迹");
            rep.AppendLine();
            rep.AppendLine("量的是「注入之后目标里多了什么」。这决定阶段 3（抽 WPECore）值不值得做 ——");
            rep.AppendLine("那一步唯一的收益就是缩小这个足迹（风险清单 R2）。");
            rep.AppendLine();

            InitShellSide();

            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            new Thread(() => Sink(listener)) { IsBackground = true }.Start();

            var link = new ShellLink();
            Process target = null;

            try
            {
                target = StartTarget(port, 40);

                //=== 注入之前先记一份基线 ===
                target.Refresh();
                var before = NativeModules(target);
                rep.AppendLine("- 注入<b>前</b>目标的原生模块数: " + before.Count);

                link.Attach(target.Id, null, 15000);
                link.StartHook();
                Thread.Sleep(2500);

                List<string[]> asms;
                List<string> mods;
                link.GetFootprint(out asms, out mods);

                rep.AppendLine("- 注入<b>后</b>目标的原生模块数: " + mods.Count +
                               "（多了 " + (mods.Count - before.Count) + " 个）");
                rep.AppendLine("- 目标里的托管程序集数: " + asms.Count);
                rep.AppendLine();

                rep.AppendLine("## 托管程序集（目标进程内）");
                rep.AppendLine();
                rep.AppendLine("| 程序集 | 是不是 WPE 带进去的 |");
                rep.AppendLine("|--------|--------------------|");

                var loadedWatch = new List<string>();

                foreach (var a in asms.OrderBy(x => x[0], StringComparer.OrdinalIgnoreCase))
                {
                    bool watched = Watch.Any(w => a[0].StartsWith(w, StringComparison.OrdinalIgnoreCase));
                    if (watched) { loadedWatch.Add(a[0]); }

                    rep.AppendLine("| " + a[0] + " | " + (watched ? "**是**" : "") + " |");
                }

                rep.AppendLine();
                rep.AppendLine("## 关注清单的落实情况");
                rep.AppendLine();
                rep.AppendLine("这些是 R2 点名的东西。**没出现 = 那一条风险本来就不存在**。");
                rep.AppendLine();

                foreach (string w in Watch)
                {
                    bool hit = asms.Any(a => a[0].StartsWith(w, StringComparison.OrdinalIgnoreCase));
                    rep.AppendLine("- " + w.PadRight(28) + (hit ? "**已加载**" : "未加载"));
                }

                rep.AppendLine();
                foreach (string w in WatchNative)
                {
                    bool hit = mods.Any(m => string.Equals(m, w, StringComparison.OrdinalIgnoreCase));
                    rep.AppendLine("- （原生）" + w.PadRight(24) + (hit ? "**已加载**" : "未加载"));
                }

                rep.AppendLine();
                rep.AppendLine("## 结论");
                rep.AppendLine();
                rep.AppendLine("目标里真正被 WPE 带进去的托管程序集共 **" + loadedWatch.Count + "** 个：");
                rep.AppendLine(loadedWatch.Count == 0 ? "（无）" : "`" + string.Join("`, `", loadedWatch.ToArray()) + "`");

                link.Detach();

                string text = rep.ToString();
                Console.WriteLine(text);

                if (!string.IsNullOrEmpty(outPath))
                {
                    File.WriteAllText(outPath, text, new UTF8Encoding(false));
                    Console.WriteLine("报告已写入 " + outPath);
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
                try { link.Dispose(); } catch { }
                try { if (target != null && !target.HasExited) { target.Kill(); } } catch { }
                try { listener.Stop(); } catch { }
            }
        }

        #region//杂项

        private static List<string> NativeModules(Process p)
        {
            var list = new List<string>();
            try { foreach (ProcessModule m in p.Modules) { list.Add(m.ModuleName); } }
            catch { }
            return list;
        }

        private static void Sink(TcpListener listener)
        {
            try
            {
                using (TcpClient c = listener.AcceptTcpClient())
                using (NetworkStream ns = c.GetStream())
                {
                    byte[] buf = new byte[4096];
                    while (ns.Read(buf, 0, buf.Length) > 0) { }
                }
            }
            catch { }
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
        }

        private static Process StartTarget(int port, int seconds)
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

        private static string GetArg(string[] args, string name, string def)
        {
            int i = Array.IndexOf(args, name);
            return (i >= 0 && i + 1 < args.Length) ? args[i + 1] : def;
        }

        #endregion
    }
}
