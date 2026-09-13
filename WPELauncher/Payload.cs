using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;

namespace WPELauncher
{
    /// <summary>嵌在本 exe 里的那份程序目录的说明（payload.txt）。</summary>
    internal sealed class PayloadInfo
    {
        public string Version = "";
        public string Hash = "";
        public string Exe = "WinsockPacketEditor.exe";

        /// <summary>解压根目录名：%LOCALAPPDATA%\&lt;Name&gt;\app。WPE x64 是 WPE64，WPE Proxy Cap 是 WPEProxyCap。</summary>
        public string Name = "WPE64";

        /// <summary>进度窗与错误框的标题。</summary>
        public string Title = "WPE x64";
        public long Bytes;
        public int Files;

        /// <summary>
        /// 解压目录名：版本 + 载荷哈希前 12 位。
        /// 带哈希是为了「同一个版本号重新打了一次包」时换一个目录，而不是把旧文件当成新包的继续用。
        /// </summary>
        public string DirName
        {
            get { return Version + "-" + Hash.Substring(0, Math.Min(12, Hash.Length)); }
        }
    }

    /// <summary>
    /// 载荷的读取、核对、解压、修复与旧版本清理。<b>不碰界面</b>（进度走 IProgress），
    /// 所以 tools\tests\Launcher.ps1 能反射进来直接跑。
    /// </summary>
    internal static class Payload
    {
        public const string ZipResource = "WPE64.payload.zip";
        public const string InfoResource = "WPE64.payload.txt";

        /// <summary>解压完整之后才写的标记，内容是载荷哈希。没有它的目录一律当成残留。</summary>
        public const string ReadyMarker = ".wpe64-ready";

        private const string TempPrefix = ".tmp-";

        #region//位置与说明

        /// <summary>%LOCALAPPDATA%\&lt;name&gt;\app —— 按用户、不需要写 Program Files。</summary>
        public static string DefaultRoot(string name)
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), name, "app");
        }

        /// <summary>读 payload.txt；本 exe 里没有载荷时返回 null。</summary>
        public static PayloadInfo ReadInfo()
        {
            using (Stream s = typeof(Payload).Assembly.GetManifestResourceStream(InfoResource))
            {
                if (s == null)
                {
                    return null;
                }

                PayloadInfo info = new PayloadInfo();

                using (StreamReader r = new StreamReader(s, Encoding.UTF8))
                {
                    string line;

                    while ((line = r.ReadLine()) != null)
                    {
                        int eq = line.IndexOf('=');

                        if (eq <= 0)
                        {
                            continue;
                        }

                        string k = line.Substring(0, eq).Trim();
                        string v = line.Substring(eq + 1).Trim();

                        switch (k)
                        {
                            case "Version": info.Version = v; break;
                            case "Hash": info.Hash = v.ToLowerInvariant(); break;
                            case "Exe": info.Exe = v; break;
                            case "Name": info.Name = v; break;
                            case "Title": info.Title = v; break;
                            case "Bytes": long.TryParse(v, out info.Bytes); break;
                            case "Files": int.TryParse(v, out info.Files); break;
                        }
                    }
                }

                if (info.Version.Length == 0 || info.Hash.Length < 12)
                {
                    throw new InvalidDataException("payload.txt 缺少版本或哈希");
                }

                //Name 与 Exe 都要落成路径的一段，不许带目录分隔符或 ..
                if (!IsPlainName(info.Name) || !IsPlainName(info.Exe) || info.Title.Length == 0)
                {
                    throw new InvalidDataException("payload.txt 的 Name / Exe / Title 不合法");
                }

                return info;
            }
        }

        private static ZipArchive OpenZip()
        {
            //资源流是映射在 exe 映像上的 UnmanagedMemoryStream：可随机访问，不会把几十 MB 读进托管堆
            Stream s = typeof(Payload).Assembly.GetManifestResourceStream(ZipResource);

            if (s == null)
            {
                throw new InvalidDataException("启动器里没有 payload.zip");
            }

            return new ZipArchive(s, ZipArchiveMode.Read, false);
        }

        #endregion

        #region//核对

        /// <summary>目录完整可用：有就绪标记、哈希对得上、清单里每个文件都在且大小相同。</summary>
        public static bool IsIntact(string dir, PayloadInfo info)
        {
            if (!HasMarker(dir, info))
            {
                return false;
            }

            return FindDamaged(dir).Count == 0;
        }

        public static bool HasMarker(string dir, PayloadInfo info)
        {
            string marker = Path.Combine(dir, ReadyMarker);

            try
            {
                return File.Exists(marker)
                    && string.Equals(File.ReadAllText(marker).Trim(), info.Hash, StringComparison.OrdinalIgnoreCase);
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        /// <summary>
        /// 缺失或大小不对的条目（zip 里的名字）。
        /// 只比大小不算哈希：每次启动要过一遍 500 多个文件，读内容就慢了；
        /// 常见的损坏（被杀软删掉、解到一半断电）大小一定对不上。
        /// </summary>
        public static HashSet<string> FindDamaged(string dir)
        {
            HashSet<string> bad = new HashSet<string>(StringComparer.Ordinal);
            string root = RootedDir(dir);

            using (ZipArchive zip = OpenZip())
            {
                foreach (ZipArchiveEntry e in zip.Entries)
                {
                    if (IsDirectoryEntry(e))
                    {
                        continue;
                    }

                    FileInfo fi = new FileInfo(SafePath(root, e.FullName));

                    if (!fi.Exists || fi.Length != e.Length)
                    {
                        bad.Add(e.FullName);
                    }
                }
            }

            return bad;
        }

        #endregion

        #region//解压与修复

        /// <summary>
        /// 保证 <paramref name="root"/> 下有一份完整的程序目录，返回它的路径。
        /// 已有就绪标记 → 只补坏掉的文件；否则解压到临时目录，写完标记再整体改名过去。
        /// </summary>
        public static string Extract(string root, PayloadInfo info, IProgress<int> progress)
        {
            Directory.CreateDirectory(root);
            string dir = Path.Combine(root, info.DirName);

            if (HasMarker(dir, info))
            {
                Repair(dir, progress);
                return dir;
            }

            EnsureSpace(root, info.Bytes);

            string tmp = Path.Combine(root, TempPrefix + Guid.NewGuid().ToString("N"));

            try
            {
                ExtractEntries(tmp, null, progress);
                File.WriteAllText(Path.Combine(tmp, ReadyMarker), info.Hash);

                //没有标记的同名目录：上次解到一半、或被删掉了标记。先清掉再换上新的
                if (Directory.Exists(dir))
                {
                    DeleteDirectory(dir);
                }

                MoveWithRetry(tmp, dir);
            }
            catch
            {
                TryDeleteDirectory(tmp);
                throw;
            }

            return dir;
        }

        private static void Repair(string dir, IProgress<int> progress)
        {
            HashSet<string> bad = FindDamaged(dir);

            if (bad.Count == 0)
            {
                return;
            }

            try
            {
                ExtractEntries(dir, bad, progress);
            }
            catch (IOException ex)
            {
                //最常见：这一版的 WPE 还开着，文件被占用
                throw new IOException(Strings.RepairLocked + " " + ex.Message, ex);
            }
        }

        private static void ExtractEntries(string target, HashSet<string> only, IProgress<int> progress)
        {
            string root = RootedDir(target);
            Directory.CreateDirectory(root);

            using (ZipArchive zip = OpenZip())
            {
                long total = 0;

                foreach (ZipArchiveEntry e in zip.Entries)
                {
                    if (only == null || only.Contains(e.FullName))
                    {
                        total += e.Length;
                    }
                }

                long done = 0;
                int lastPercent = -1;
                byte[] buffer = new byte[81920];

                foreach (ZipArchiveEntry e in zip.Entries)
                {
                    if (only != null && !only.Contains(e.FullName))
                    {
                        continue;
                    }

                    string path = SafePath(root, e.FullName);

                    if (IsDirectoryEntry(e))
                    {
                        Directory.CreateDirectory(path);
                        continue;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                    using (Stream src = e.Open())
                    using (FileStream dst = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length))
                    {
                        int n;

                        while ((n = src.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            dst.Write(buffer, 0, n);
                            done += n;

                            int percent = total > 0 ? (int)(done * 100 / total) : 100;

                            if (progress != null && percent != lastPercent)
                            {
                                lastPercent = percent;
                                progress.Report(percent);
                            }
                        }
                    }

                    try
                    {
                        File.SetLastWriteTime(path, e.LastWriteTime.DateTime);
                    }
                    catch (ArgumentException)
                    {
                        //zip 里的时间超出文件系统范围时不影响使用
                    }
                }

                if (progress != null)
                {
                    progress.Report(100);
                }
            }
        }

        private static void EnsureSpace(string root, long bytes)
        {
            try
            {
                string drive = Path.GetPathRoot(Path.GetFullPath(root));
                long free = new DriveInfo(drive).AvailableFreeSpace;
                long need = bytes + 64L * 1024 * 1024;

                if (free < need)
                {
                    throw new IOException(string.Format(Strings.NoSpace, drive, need / 1048576, free / 1048576));
                }
            }
            catch (ArgumentException)
            {
                //网络路径等拿不到 DriveInfo：不预检，真写满了由写文件那一步报错
            }
        }

        #endregion

        #region//旧版本清理

        /// <summary>
        /// 删掉 <paramref name="root"/> 下除 <paramref name="keepName"/> 之外、没有在运行的版本目录，
        /// 以及放了超过一小时的临时目录。任何一个删不掉都跳过，不影响启动。
        /// </summary>
        public static int CleanupOld(string root, string keepName)
        {
            if (!Directory.Exists(root))
            {
                return 0;
            }

            int removed = 0;

            foreach (string d in Directory.GetDirectories(root))
            {
                string name = Path.GetFileName(d);

                if (string.Equals(name, keepName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (name.StartsWith(TempPrefix, StringComparison.Ordinal))
                {
                    //别动别的启动器正在解压的那一份
                    if (Directory.GetLastWriteTimeUtc(d) < DateTime.UtcNow.AddHours(-1) && TryDeleteDirectory(d))
                    {
                        removed++;
                    }

                    continue;
                }

                if (!IsInUse(d) && TryDeleteDirectory(d))
                {
                    removed++;
                }
            }

            return removed;
        }

        /// <summary>这个版本目录里有没有进程在跑（WPE 本体、EasyHook 的 32/64 位服务等）。</summary>
        public static bool IsInUse(string dir)
        {
            string root = RootedDir(dir);
            string[] exes;

            try
            {
                exes = Directory.GetFiles(root, "*.exe", SearchOption.TopDirectoryOnly);
            }
            catch (IOException)
            {
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return true;
            }

            foreach (string exe in exes)
            {
                foreach (Process p in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(exe)))
                {
                    try
                    {
                        if (p.MainModule.FileName.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        //读不到模块（权限 / 进程刚退出）：按占用处理，宁可留着也不误删
                        return true;
                    }
                    finally
                    {
                        p.Dispose();
                    }
                }

                //兜底：运行中的 exe 映像不能以独占方式打开
                try
                {
                    using (new FileStream(exe, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                    }
                }
                catch (IOException)
                {
                    return true;
                }
                catch (UnauthorizedAccessException)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region//文件系统小工具

        private static bool IsPlainName(string s)
        {
            return !string.IsNullOrEmpty(s)
                && s != "." && s != ".."
                && s.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }

        private static string RootedDir(string dir)
        {
            return Path.GetFullPath(dir).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        }

        private static bool IsDirectoryEntry(ZipArchiveEntry e)
        {
            return e.FullName.EndsWith("/", StringComparison.Ordinal) || e.FullName.EndsWith("\\", StringComparison.Ordinal);
        }

        /// <summary>zip 条目 → 目标路径；拒绝跑出目标目录的条目（../ 之类）。</summary>
        private static string SafePath(string root, string entryName)
        {
            string path = Path.GetFullPath(Path.Combine(root, entryName.Replace('/', Path.DirectorySeparatorChar)));

            if (!path.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException("载荷里有越出目标目录的条目：" + entryName);
            }

            return path;
        }

        /// <summary>先删就绪标记再删内容：删到一半失败时，这个目录下次会被当成残留重新解压，而不是当成完整的去用。</summary>
        private static void DeleteDirectory(string dir)
        {
            string marker = Path.Combine(dir, ReadyMarker);

            if (File.Exists(marker))
            {
                File.Delete(marker);
            }

            Directory.Delete(dir, true);
        }

        private static bool TryDeleteDirectory(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    DeleteDirectory(dir);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>刚写完的 DLL 常被杀软扫描占着，改名要重试几次。</summary>
        private static void MoveWithRetry(string from, string to)
        {
            for (int i = 0; ; i++)
            {
                try
                {
                    Directory.Move(from, to);
                    return;
                }
                catch (IOException) when (i < 19)
                {
                    Thread.Sleep(250);
                }
                catch (UnauthorizedAccessException) when (i < 19)
                {
                    Thread.Sleep(250);
                }
            }
        }

        #endregion
    }
}
