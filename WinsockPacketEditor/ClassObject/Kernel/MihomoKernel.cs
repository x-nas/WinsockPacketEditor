using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 内置 mihomo 内核的生命周期：生成 config.yaml → 启动 wpe-mihomo.exe → 转发日志 → 监控退出 → 停进程。
    ///
    /// 【为什么要等 TUN 就绪】POC 实测：上一次内核被强杀后，TUN 网卡卸载有延迟，
    /// 下一次创建会先失败一次（"Cannot create a file when that file already exists"）再重试成功，
    /// 全程可能 ~16s。所以 Start 必须等到日志出现 "Tun adapter listening" 才算起来，
    /// 不能只看 external-controller 能连上。
    /// </summary>
    public static class MihomoKernel
    {
        private const string ExeName = "wpe-mihomo.exe";
        private const string ProcessName = "wpe-mihomo";
        private const int ReadyTimeoutMs = 60000;

        /// <summary>mihomo（sing-tun）在 Windows 上固定的 TUN 适配器名。</summary>
        private const string TunName = "Meta";

        /// <summary>内核连接自身 SOCKS5 时用的固定用户名（口令每次启动随机）。</summary>
        public const string KernelUser = "__wpe_kernel__";

        private static readonly object gate = new object();

        private static Process proc;
        private static string exePath;
        private static string workDir;
        private static string configPath;
        private static string secret;
        private static int ctlPort;

        /// <summary>
        /// 内核专用的认证口令。AuthUserName 里用 TryAuthKernel 校验它，
        /// 这样内核不必占用用户账号、设备槽与连接数。
        /// </summary>
        public static bool TryAuthKernel(string userName, string passWord)
        {
            string s = secret;
            return !string.IsNullOrEmpty(s)
                && string.Equals(userName, KernelUser, StringComparison.Ordinal)
                && string.Equals(passWord, s, StringComparison.Ordinal);
        }

        private static volatile bool ready;
        private static volatile bool stopRequested;
        private static volatile string lastLine = string.Empty;
        private static string version = string.Empty;

        /// <summary>内核版本串（启动时用 -v 读一次），未启动时为空。</summary>
        public static string Version { get { return version; } }

        /// <summary>内核进程是否在跑。</summary>
        public static bool IsRunning
        {
            get
            {
                Process p = proc;
                return p != null && !p.HasExited;
            }
        }

        /// <summary>TUN 是否已就绪（日志出现过 Tun adapter listening）。</summary>
        public static bool IsReady { get { return ready; } }

        /// <summary>最近一条 mihomo 日志（warning/error），用于报错。</summary>
        public static string LastLine { get { return lastLine; } }

        public static string WorkDir { get { return workDir; } }

        public static string ConfigPath { get { return configPath; } }

        /// <summary>
        /// 生成配置并启动内核，等到 TUN 就绪再返回。失败返回 false，error 是要显示的文案。
        /// selected 为空 = 不拦截任何进程（仅断环规则）。
        /// </summary>
        public static bool Start(
            int socks5Port,
            string server,
            bool authEnabled,
            string tunStack,
            string dnsMode,
            IList<ProcessInfo> selected,
            out string error)
        {
            error = string.Empty;

            lock (gate)
            {
                if (proc != null && !proc.HasExited)
                {
                    error = "内核已在运行";
                    return false;
                }

                try
                {
                    exePath = Path.Combine(AppContext.BaseDirectory, ExeName);
                    if (!File.Exists(exePath))
                    {
                        error = "找不到内核程序 " + ExeName;
                        return false;
                    }

                    workDir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "WPE64", "mihomo");
                    Directory.CreateDirectory(workDir);
                    configPath = Path.Combine(workDir, "config.yaml");

                    secret = RandomHex(16);
                    ctlPort = FindFreePort();
                    version = ReadVersion(exePath);

                    string tplErr;
                    string tpl = MihomoConfig.ReadTemplate(out tplErr);
                    if (tpl == null) { error = tplErr; return false; }

                    List<string> names = new List<string>();
                    if (selected != null)
                    {
                        foreach (ProcessInfo pi in selected)
                        {
                            if (pi != null && !string.IsNullOrEmpty(pi.ModuleName)) { names.Add(pi.ModuleName); }
                        }
                    }

                    // 内核用自己的随机口令（不占用户账号 / 设备 / 连接数）；认证关着时不写凭据
                    string userName = authEnabled ? KernelUser : string.Empty;
                    string passWord = authEnabled ? secret : string.Empty;

                    string cfg = MihomoConfig.Build(
                        tpl, server, socks5Port, userName, passWord, tunStack, dnsMode, ctlPort, secret, names, out error);
                    if (cfg == null) { return false; }

                    WriteAllTextAtomic(configPath, cfg);

                    ready = false;
                    stopRequested = false;
                    lastLine = string.Empty;

                    //没有可用的 Meta 网卡时，清掉上次强杀留下的半死 WINTUN 设备（否则创建会撞车、要等重试）
                    if (!TunInterfaceUp()) { RemoveStaleTunDevices(); }

                    Process p = new Process();
                    p.StartInfo.FileName = exePath;
                    p.StartInfo.Arguments = "-d \"" + workDir + "\"";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                    p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                    p.EnableRaisingEvents = true;
                    p.OutputDataReceived += OnOutput;
                    p.ErrorDataReceived += OnError;
                    p.Exited += OnExited;

                    p.Start();
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    proc = p;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(MihomoKernel) + ".Start", ex);
                    error = ex.Message;
                    SafeKill();
                    return false;
                }
            }

            // 等 TUN 就绪（不持锁，避免阻塞 IsRunning / Stop）
            //
            // 判据：Meta 网卡已 Up（主）+ 日志出现 Tun adapter listening（兜底）。
            // ⚠️ 不能只看日志 —— 模板的 log-level 是 warning，而那条是 info，会被压掉，
            // 于是「内核其实已经工作」也会被当成启动失败（2026-09-23 修）。
            int waited = 0;
            while (waited < ReadyTimeoutMs)
            {
                if (ready || TunInterfaceUp())
                {
                    /*
                        ⚠️ <b>命中网卡也要把 ready 立起来。</b>

                        就绪有两份判据：日志里那句 info「Tun adapter listening」与「Meta 网卡已 Up」。
                        但 ready 只有前者会置位 —— 而模板的 log-level 是 warning，那句 info 根本不打，
                        于是内核明明已经接管流量，IsReady 仍是 false：
                        界面（进程设置的状态标）显示「未就绪」、RunBar 的 TUN 灯不亮。
                        2026-09-23 用户报「启用进程拦截后面出现黄色的『未就绪』」就是这条。
                    */
                    ready = true;
                    return true;
                }
                if (!IsRunning)
                {
                    error = "内核启动失败：" + (string.IsNullOrEmpty(lastLine) ? "进程已退出" : lastLine);
                    SafeKill();
                    return false;
                }
                Thread.Sleep(250);
                waited += 250;
            }

            error = "内核启动超时（等 TUN 就绪超过 " + (ReadyTimeoutMs / 1000) + " 秒）";
            SafeKill();
            return false;
        }

        /// <summary>停内核：杀进程 + 兜底清掉所有同名进程。</summary>
        public static void Stop()
        {
            Process p;
            lock (gate)
            {
                stopRequested = true;
                p = proc;
                proc = null;
            }

            if (p != null)
            {
                try
                {
                    if (!p.HasExited)
                    {
                        p.Kill();
                        p.WaitForExit(5000);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(MihomoKernel) + ".Stop", ex);
                }
                finally
                {
                    try { p.Dispose(); } catch { }
                }
            }

            // 兜底：上一次异常退出可能留下同名进程
            try
            {
                foreach (Process q in Process.GetProcessesByName(ProcessName))
                {
                    try { q.Kill(); } catch { }
                    try { q.Dispose(); } catch { }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(MihomoKernel) + ".Stop.Stray", ex);
            }

            //等 Meta 网卡消失（最多 5 秒）—— 强杀来不及自己摘，留着会让下次启动撞车
            for (int i = 0; i < 20 && TunInterfaceUp(); i++) { Thread.Sleep(250); }

            ready = false;
        }

        private static void OnOutput(object sender, DataReceivedEventArgs e)
        {
            string line = e.Data;
            if (string.IsNullOrEmpty(line)) { return; }

            if (line.IndexOf("Tun adapter listening", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ready = true;
            }

            if (line.IndexOf("level=warning", StringComparison.OrdinalIgnoreCase) >= 0
                || line.IndexOf("level=error", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                lastLine = line;
                Operate.DoLog("Mihomo", line);
            }
        }

        private static void OnError(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) { return; }
            lastLine = e.Data;
            Operate.DoLog("Mihomo", e.Data);
        }

        private static void OnExited(object sender, EventArgs e)
        {
            if (stopRequested) { return; }

            ready = false;
            string msg = string.IsNullOrEmpty(lastLine) ? "内核意外退出，代理已断开" : lastLine;
            Operate.DoLog("Mihomo", msg);
            try { UI.Toast(UiIcon.Error, msg); } catch { }
        }

        /// <summary>Meta（mihomo 的 TUN 网卡）是否已经在用。</summary>
        private static bool TunInterfaceUp()
        {
            try
            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus != OperationalStatus.Up) { continue; }
                    if (string.Equals(ni.Name, TunName, StringComparison.OrdinalIgnoreCase)) { return true; }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(MihomoKernel) + ".TunInterfaceUp", ex);
            }
            return false;
        }

        /// <summary>
        /// 移除 SWD\WINTUN 下残留的设备 —— 上次强杀 mihomo 会留下「设备还在、打不开」的状态，
        /// 会让下一次创建撞车（create adapter: Cannot create a file when that file already exists）。
        /// 只在没有可用的 Meta 网卡时调用；best-effort，失败只记日志。
        /// </summary>
        private static void RemoveStaleTunDevices()
        {
            const int DIGCF_ALLCLASSES = 0x4;

            /*
                设备实例不在场。这里刻意<b>没带</b> DIGCF_PRESENT —— 要把「上次强杀留下的、还在占名字的设备」
                一并列出来删掉，而那些设备的状态未必是 present。代价是列表里会混进真正的幽灵设备，
                SetupDiRemoveDevice 对它们返回的就是这个错误码：那不是失败，是「它已经不在了」，跳过即可
                （2026-09-23 日志里那条「移除 WINTUN 残留设备失败（Win32 错误 -536870389）」就是它）。
            */
            const int ERROR_NO_SUCH_DEVINST = unchecked((int)0xE000020B);

            IntPtr set = IntPtr.Zero;

            try
            {
                set = SetupDiGetClassDevs(IntPtr.Zero, "SWD\\WINTUN", IntPtr.Zero, DIGCF_ALLCLASSES);
                if (set == IntPtr.Zero || set == new IntPtr(-1)) { return; }

                /*
                    删成功会让后面的下标整体前移，所以删掉一个就回到下标 0 重来；
                    删不掉的（幽灵设备）跳过它继续看下一个。
                    ⚠️ 原来这里是 break —— 一个幽灵挡在第 0 位，后面真正要删的设备就一个都清不掉，
                    而「上次强杀留下的设备占着名字」正是这个函数存在的理由。
                */
                int index = 0;
                for (int guard = 0; guard < 64; guard++)
                {
                    SP_DEVINFO_DATA data = new SP_DEVINFO_DATA();
                    data.cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));

                    if (!SetupDiEnumDeviceInfo(set, index, ref data)) { break; }

                    if (SetupDiRemoveDevice(set, ref data)) { index = 0; continue; }

                    int err = Marshal.GetLastWin32Error();
                    if (err != ERROR_NO_SUCH_DEVINST)
                    {
                        Operate.DoLog(nameof(MihomoKernel) + ".CleanTun", "移除 WINTUN 残留设备失败（Win32 错误 " + err + "）");
                    }

                    index++;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(MihomoKernel) + ".CleanTun", ex);
            }
            finally
            {
                if (set != IntPtr.Zero && set != new IntPtr(-1))
                {
                    try { SetupDiDestroyDeviceInfoList(set); } catch { }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public int DevInst;
            public IntPtr Reserved;
        }

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs(IntPtr ClassGuid, string Enumerator, IntPtr hwndParent, int Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, int MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiRemoveDevice(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        private static void SafeKill()
        {
            Process p;
            lock (gate) { p = proc; proc = null; }
            if (p == null) { return; }
            try { if (!p.HasExited) { p.Kill(); p.WaitForExit(3000); } } catch { }
            try { p.Dispose(); } catch { }
            ready = false;
        }

        /// <summary>用 -v 读一次内核版本（很快，失败返回空串）。</summary>
        private static string ReadVersion(string exe)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(exe, "-v");
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.StandardOutputEncoding = Encoding.UTF8;
                using (Process p = Process.Start(psi))
                {
                    if (p == null) { return string.Empty; }
                    string line = p.StandardOutput.ReadLine();
                    p.WaitForExit(3000);
                    return line == null ? string.Empty : line.Trim();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void WriteAllTextAtomic(string path, string text)
        {
            string tmp = path + ".tmp";
            File.WriteAllText(tmp, text, new UTF8Encoding(false));
            if (File.Exists(path)) { File.Delete(path); }
            File.Move(tmp, path);
        }

        private static string RandomHex(int bytes)
        {
            byte[] b = new byte[bytes];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(b);
            }
            StringBuilder sb = new StringBuilder(bytes * 2);
            foreach (byte x in b) { sb.Append(x.ToString("x2")); }
            return sb.ToString();
        }

        private static int FindFreePort()
        {
            System.Net.Sockets.TcpListener l = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
            l.Start();
            int port = ((System.Net.IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}
