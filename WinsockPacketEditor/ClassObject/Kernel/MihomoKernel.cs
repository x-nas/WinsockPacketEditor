using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private const int ReadyTimeoutMs = 45000;

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
            int waited = 0;
            while (waited < ReadyTimeoutMs)
            {
                if (ready) { return true; }
                if (!IsRunning)
                {
                    error = "内核启动失败：" + (string.IsNullOrEmpty(lastLine) ? "进程已退出" : lastLine);
                    SafeKill();
                    return false;
                }
                Thread.Sleep(200);
                waited += 200;
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
