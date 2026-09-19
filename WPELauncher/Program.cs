using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WPELauncher
{
    internal static class Program
    {
        // 固定 MCP 入口。payload 的 app 目录带版本 + 哈希，不能让用户把它写进
        // VS Code 配置；只为 WPE x64 同步，避免影响共用本启动器的其他产品。
        private const string WpeMcpStableDir = @"C:\WPE64DB\McpServer";

        /// <summary>
        /// 读载荷说明 → 目录不完整就解压 / 修复（带进度窗）→ 启动程序 → 清理不在用的旧版本。
        /// </summary>
        [STAThread]
        private static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string title = Strings.FallbackTitle;
            string root = null;

            try
            {
                PayloadInfo info = Payload.ReadInfo();

                if (info == null)
                {
                    Error(title, Strings.NoPayload);
                    return 2;
                }

                title = info.Title;
                root = Payload.DefaultRoot(info.Name);
                string dir = Path.Combine(root, info.DirName);

                //同一个包被连点两下：第二个等第一个解压完，不要两个一起往同一处写
                using (Mutex mutex = new Mutex(false, @"Local\" + info.Name + "Launcher-" + info.Hash.Substring(0, 16)))
                {
                    bool owned;

                    try
                    {
                        owned = mutex.WaitOne(TimeSpan.FromMinutes(10));
                    }
                    catch (AbandonedMutexException)
                    {
                        owned = true;
                    }

                    if (!owned)
                    {
                        Error(title, Strings.Busy);
                        return 3;
                    }

                    try
                    {
                        if (!Payload.IsIntact(dir, info))
                        {
                            string status = Payload.HasMarker(dir, info) ? Strings.Repairing : Strings.Preparing;
                            string capturedRoot = root;
                            Exception err = SplashForm.Run(info.Title, info.Version, status, p => Payload.Extract(capturedRoot, info, p));

                            if (err != null)
                            {
                                Error(title, Strings.Failed + err.Message + "\r\n\r\n" + Strings.AvHint + "\r\n" + root);
                                return 4;
                            }
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }

                if (string.Equals(info.Name, "WPE64", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        SyncDirectory(Path.Combine(dir, "McpServer"), WpeMcpStableDir);
                    }
                    catch (IOException)
                    {
                        LauncherDialog.ShowMcpServerInUse(title);
                        return 6;
                    }
                    catch (Exception ex)
                    {
                        Error(title, "MCP Server 同步失败。\r\n\r\n" + ex.Message);
                        return 6;
                    }
                }

                if (!Launch(title, Path.Combine(dir, info.Exe), dir, args))
                {
                    return 5;
                }

                try
                {
                    Payload.CleanupOld(root, info.DirName);
                }
                catch (Exception)
                {
                    //清理失败不影响使用，下次启动再试
                }

                return 0;
            }
            catch (Exception ex)
            {
                string where = root == null ? string.Empty : "\r\n\r\n" + Strings.AvHint + "\r\n" + root;
                Error(title, Strings.Failed + ex.Message + where);
                return 1;
            }
        }

        /// <summary>
        /// <summary>将当前 payload 的 MCP sidecar 同步到 VS Code 配置使用的固定目录。</summary>
        private static void SyncDirectory(string source, string destination)
        {
            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException("打包内容中缺少 McpServer 目录：" + source);
            }

            Directory.CreateDirectory(destination);

            foreach (string file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
            {
                string relative = file.Substring(source.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string target = Path.Combine(destination, relative);
                string parent = Path.GetDirectoryName(target);
                if (!Directory.Exists(parent)) { Directory.CreateDirectory(parent); }
                File.Copy(file, target, true);
            }
        }

        private static bool Launch(string title, string exe, string dir, string[] args)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(exe)
                {
                    UseShellExecute = true,
                    WorkingDirectory = dir,
                    Arguments = JoinArgs(args),
                };

                using (Process.Start(psi))
                {
                }

                return true;
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                //用户在 UAC 上点了「否」（启动器本身已提权时不会走到这里，留作兜底）
                return false;
            }
            catch (Exception ex)
            {
                Error(title, Strings.LaunchFailed + ex.Message + "\r\n" + exe);
                return false;
            }
        }

        /// <summary>按 CommandLineToArgvW 的规则把参数重新拼回去，原样转交给程序。</summary>
        private static string JoinArgs(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string a in args)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }

                if (a.Length > 0 && a.IndexOfAny(new[] { ' ', '\t', '"' }) < 0)
                {
                    sb.Append(a);
                    continue;
                }

                sb.Append('"');
                int slashes = 0;

                foreach (char c in a)
                {
                    if (c == '\\')
                    {
                        slashes++;
                        continue;
                    }

                    if (c == '"')
                    {
                        sb.Append('\\', slashes * 2 + 1);
                    }
                    else
                    {
                        sb.Append('\\', slashes);
                    }

                    slashes = 0;
                    sb.Append(c);
                }

                sb.Append('\\', slashes * 2);
                sb.Append('"');
            }

            return sb.ToString();
        }

        private static void Error(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
