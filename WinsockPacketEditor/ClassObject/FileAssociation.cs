using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 让资源管理器给 WPE 自己导出的 13 种数据文件（.sb / .fp / .sp …）显示 WPE 的数据文件图标。
    ///
    /// 【为什么只能靠注册表】Windows 显示文件图标只看文件关联（后缀 → ProgID → DefaultIcon），
    /// 没注册的后缀一律画成空白文档。这里只注册<b>图标与类型名</b>，<b>不注册打开方式</b> ——
    /// 机器上可能同时解压着好几个版本的 WPE，双击该用哪一个说不准（2026-09-11 定的）。
    ///
    /// 【写在哪】只写当前用户的 <c>HKCU\Software\Classes</c>，不碰 HKLM：
    /// 这是个解压即用的程序，不该动全机器的设置，也不需要为此另要权限。
    ///
    /// 【三条规矩】
    ///   · <b>不抢别人的后缀</b>：这几个后缀都很短，.pas（Pascal 源码）、.rp（Axure 原型）、.wl（Wolfram）、
    ///     .sb（Scratch 1.x）、.pml（Process Monitor 日志）、.fp（FileMaker）都有人在用。
    ///     后缀没有归属、或者已经归 WPE 才写；已经有主的一律不动（包括 Windows 为「打开方式」自动建的 *_auto_file）。
    ///   · <b>图标文件放在固定位置</b>（%LOCALAPPDATA%\WPE64\Icons）：程序每个版本解压到不同目录，
    ///     指向程序目录的话删掉旧版本文件夹图标就又变回空白。文件名带内容哈希 ——
    ///     资源管理器的图标缓存按「路径 + 序号」记，图标换了而路径不变的话要重启资源管理器才看得到新的。
    ///   · <b>值没变就不写</b>：每次启动都跑一遍，绝大多数时候只是读几个键。
    ///
    /// 【用户可以清除】「软件设置」里有「清除文件关联」：删掉这里写的全部内容，并在
    /// <c>HKCU\Software\WPE64</c> 记一个 <c>FileAssociation = 0</c> —— 否则下次启动又自动注册回来了。
    /// 这个标记本身是清除之后唯一留下的东西；点「重新关联」把它改回 1。
    /// </summary>
    public static class FileAssociation
    {
        #region//13 种文件

        public sealed class FileType
        {
            public readonly string Ext;
            public readonly string ProgId;
            public readonly Func<string> Name;   //类型名（资源管理器「类型」列），按当前界面语言取

            public FileType(string Ext, string ProgId, Func<string> Name)
            {
                this.Ext = Ext;
                this.ProgId = ProgId;
                this.Name = Name;
            }
        }

        /// <summary>
        /// WPE 自己的导出格式。<b>别的软件的格式不在这里</b>：.xls / .ini（CCProxy）/ .txt /
        /// .cer / .crt / .pem / .0（证书）/ .chlsx（Charles）/ .filt（旧版 WPE，只导入）。
        /// 类型名的键写字面量（C# 文案表的瘦身是按字面量判断键有没有人用的）。
        /// </summary>
        public static readonly FileType[] Types =
        {
            new FileType(".sb",  "WPE64.Backup",         () => UI.T("BackupFile", "备份文件")),
            new FileType(".fp",  "WPE64.FilterList",     () => UI.T("FilterListFile", "滤镜列表文件")),
            new FileType(".sp",  "WPE64.SendList",       () => UI.T("SendListFile", "发送列表文件")),
            new FileType(".sc",  "WPE64.SendCollection", () => UI.T("SendList.SendCollectionFile", "发送集文件")),
            new FileType(".rp",  "WPE64.RobotList",      () => UI.T("RobotListFile", "机器人列表文件")),
            new FileType(".whp", "WPE64.WareHouseList",  () => UI.T("WareHouseList.File", "仓库列表文件")),
            new FileType(".whs", "WPE64.Stores",         () => UI.T("StoresFile", "仓储数据文件")),
            new FileType(".pas", "WPE64.AutoStores",     () => UI.T("AutoStores.File", "自动入库文件")),
            new FileType(".pa",  "WPE64.ProxyAccount",   () => UI.T("ProxyAccountListFile", "代理账号列表文件")),
            new FileType(".wl",  "WPE64.WhiteList",      () => UI.T("FireWallSetting.WhiteListFile", "白名单文件")),
            new FileType(".bl",  "WPE64.BlackList",      () => UI.T("FireWallSetting.BlackListFile", "黑名单文件")),
            new FileType(".pml", "WPE64.MapLocal",       () => UI.T("MapLocalFile", "本地映射文件")),
            new FileType(".pmr", "WPE64.MapRemote",      () => UI.T("MapRemoteFile", "远程映射文件")),
        };

        private const string IconName = "wpe-data.ico";
        private const string MarkerKey = @"Software\WPE64";
        private const string MarkerValue = "FileAssociation";

        #endregion

        #region//对外：启动时同步 / 设置里的开关 / 状态

        /// <summary>设置页用的状态。</summary>
        public sealed class Status
        {
            public bool Enabled;
            public string[] Claimed = new string[0];      //归 WPE 的后缀（显示 WPE 图标）
            public string[] Foreign = new string[0];      //已被别的程序占用、没动的后缀
            public string[] Owners = new string[0];       //与 Foreign 一一对应：占用它的 ProgID
            public bool IconMissing;                      //程序目录里没有 wpe-data.ico（开发机没构建全时会这样）
        }

        private static readonly object gate = new object();

        /// <summary>
        /// 启动时与切换界面语言后各调一次（后台线程）。用户清除过就什么都不做 —— 也不替他再清一遍，
        /// 清除只在他点按钮的那一刻做。
        /// </summary>
        public static void Sync()
        {
            try
            {
                if (!IsEnabled()) { return; }

                lock (gate)
                {
                    ApplyResult r = ApplyReal();
                    if (r.Changed)
                    {
                        Operate.DoLog(nameof(FileAssociation), string.Format(
                            UI.T("FileAssoc.Registered", "已为 {0} 种 WPE 数据文件注册图标（{1}）"), r.Claimed.Count, string.Join(" ", r.Claimed)));
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Sync), ex);
            }
        }

        /// <summary>设置里的按钮：on = 重新关联，off = 清除。返回操作之后的状态。</summary>
        public static Status SetEnabled(bool on)
        {
            lock (gate)
            {
                using (RegistryKey mk = Registry.CurrentUser.CreateSubKey(MarkerKey))
                {
                    mk.SetValue(MarkerValue, on ? 1 : 0, RegistryValueKind.DWord);
                }

                if (on)
                {
                    ApplyResult r = ApplyReal();
                    Operate.DoLog(nameof(FileAssociation), string.Format(
                        UI.T("FileAssoc.Registered", "已为 {0} 种 WPE 数据文件注册图标（{1}）"), r.Claimed.Count, string.Join(" ", r.Claimed)));
                }
                else
                {
                    int n;
                    using (RegistryKey classes = Registry.CurrentUser.CreateSubKey(@"Software\Classes"))
                    using (RegistryKey fileExts = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts", true))
                    {
                        n = Remove(classes, fileExts);
                    }

                    try { if (Directory.Exists(IconDir)) { Directory.Delete(IconDir, true); } }
                    catch (Exception ex) { Operate.DoLog(nameof(SetEnabled) + ".IconDir", ex); }

                    NotifyShell();
                    Operate.DoLog(nameof(FileAssociation), string.Format(UI.T("FileAssoc.Cleared", "已清除 WPE 数据文件的图标关联（{0} 种）"), n));
                }

                return GetStatusCore();
            }
        }

        public static Status GetStatus()
        {
            lock (gate) { return GetStatusCore(); }
        }

        /// <summary>设置页上画一枚图标用：32px 那一层，PNG 的 base64。取不到返回 null。</summary>
        public static string PreviewPng()
        {
            try
            {
                string src = SourceIcon;
                if (!File.Exists(src)) { return null; }

                using (System.Drawing.Icon ic = new System.Drawing.Icon(src, 32, 32))
                using (System.Drawing.Bitmap b = ic.ToBitmap())
                using (MemoryStream ms = new MemoryStream())
                {
                    b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(PreviewPng), ex);
                return null;
            }
        }

        private static bool IsEnabled()
        {
            using (RegistryKey mk = Registry.CurrentUser.OpenSubKey(MarkerKey))
            {
                object v = mk == null ? null : mk.GetValue(MarkerValue);
                return !(v is int) || (int)v != 0;
            }
        }

        private static Status GetStatusCore()
        {
            Status s = new Status { Enabled = IsEnabled(), IconMissing = !File.Exists(SourceIcon) };
            List<string> claimed = new List<string>(), foreign = new List<string>(), owners = new List<string>();

            foreach (FileType t in Types)
            {
                string owner = MergedOwner(t.Ext);
                if (string.Equals(owner, t.ProgId, StringComparison.OrdinalIgnoreCase)) { claimed.Add(t.Ext); }
                else if (!string.IsNullOrEmpty(owner)) { foreign.Add(t.Ext); owners.Add(owner); }
            }

            s.Claimed = claimed.ToArray();
            s.Foreign = foreign.ToArray();
            s.Owners = owners.ToArray();
            return s;
        }

        #endregion

        #region//真实环境：路径与根键

        private static string SourceIcon
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IconName); }
        }

        private static string IconDir
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WPE64", "Icons"); }
        }

        /// <summary>HKCR（HKCU 叠在 HKLM 上的合并视图）里这个后缀的默认 ProgID —— 判断「有没有主」看的是它。</summary>
        private static string MergedOwner(string Ext)
        {
            using (RegistryKey k = Registry.ClassesRoot.OpenSubKey(Ext))
            {
                return k == null ? null : k.GetValue(string.Empty) as string;
            }
        }

        private static ApplyResult ApplyReal()
        {
            string icon = StageIcon(SourceIcon, IconDir);
            if (icon == null) { return new ApplyResult(); }

            ApplyResult r;
            using (RegistryKey classes = Registry.CurrentUser.CreateSubKey(@"Software\Classes"))
            {
                r = Apply(classes, MergedOwner, icon, t => "WPE x64 " + t.Name());
            }

            if (r.Changed) { NotifyShell(); }
            return r;
        }

        /// <summary>
        /// 把程序目录里的 wpe-data.ico 拷到固定位置，文件名带内容哈希；返回拷过去的路径，源文件不在返回 null。
        /// 同一份内容已经在那儿就不拷；别的哈希的旧图标顺手删掉（删不掉无所谓，下次再删）。
        /// </summary>
        internal static string StageIcon(string Source, string Dir)
        {
            if (!File.Exists(Source))
            {
                Operate.DoLog(nameof(StageIcon), string.Format(UI.T("FileAssoc.NoIcon", "找不到数据文件图标：{0}"), Source));
                return null;
            }

            byte[] bytes = File.ReadAllBytes(Source);
            string hash;
            using (SHA1 sha = SHA1.Create())
            {
                hash = BitConverter.ToString(sha.ComputeHash(bytes), 0, 4).Replace("-", string.Empty).ToLowerInvariant();
            }

            Directory.CreateDirectory(Dir);
            string target = Path.Combine(Dir, "wpe-data." + hash + ".ico");

            if (!File.Exists(target) || new FileInfo(target).Length != bytes.Length)
            {
                //先写临时文件再改名：两个 WPE 同时启动时不会读到半截文件
                int pid;
                using (Process me = Process.GetCurrentProcess()) { pid = me.Id; }
                string tmp = target + "." + pid + ".tmp";
                File.WriteAllBytes(tmp, bytes);
                try
                {
                    if (File.Exists(target)) { File.Delete(target); }
                    File.Move(tmp, target);
                }
                catch (IOException)
                {
                    //另一个进程抢先写好了同一份（内容相同，文件名里就是它的哈希）
                    try { File.Delete(tmp); } catch { }
                }
            }

            foreach (string old in Directory.GetFiles(Dir, "wpe-data.*.ico"))
            {
                if (!string.Equals(old, target, StringComparison.OrdinalIgnoreCase))
                {
                    try { File.Delete(old); } catch { }
                }
            }

            return target;
        }

        #endregion

        #region//核心：写 / 删（根键由调用方给 —— 跑测用私有的 app hive，不碰真注册表）

        internal sealed class ApplyResult
        {
            public bool Changed;
            public readonly List<string> Claimed = new List<string>();
            public readonly List<string> Foreign = new List<string>();
        }

        /// <param name="Classes">相当于 HKCU\Software\Classes 的可写根</param>
        /// <param name="OwnerOf">后缀在合并视图里的默认 ProgID（没有返回 null / 空串）</param>
        /// <param name="IconPath">DefaultIcon 指向的图标文件</param>
        /// <param name="TypeName">类型名</param>
        internal static ApplyResult Apply(RegistryKey Classes, Func<string, string> OwnerOf, string IconPath, Func<FileType, string> TypeName)
        {
            ApplyResult r = new ApplyResult();

            foreach (FileType t in Types)
            {
                string owner = OwnerOf(t.Ext);

                //已经有主、而且不是 WPE —— 不动，连 ProgID 也不建
                if (!string.IsNullOrEmpty(owner) && !string.Equals(owner, t.ProgId, StringComparison.OrdinalIgnoreCase))
                {
                    r.Foreign.Add(t.Ext);
                    continue;
                }

                using (RegistryKey pk = Classes.CreateSubKey(t.ProgId))
                {
                    r.Changed |= SetIfDifferent(pk, string.Empty, TypeName(t));

                    using (RegistryKey ik = pk.CreateSubKey("DefaultIcon"))
                    {
                        r.Changed |= SetIfDifferent(ik, string.Empty, IconPath);
                    }
                }

                using (RegistryKey ek = Classes.CreateSubKey(t.Ext))
                {
                    r.Changed |= SetIfDifferent(ek, string.Empty, t.ProgId);
                }

                r.Claimed.Add(t.Ext);
            }

            return r;
        }

        /// <summary>
        /// 删掉 Apply 写过的全部内容。后缀的默认值<b>只在它指向 WPE 时</b>才删 —— 别人的不动；
        /// 删完后缀键空了（没有值也没有子键）才连键一起删。返回清掉了几种。
        /// </summary>
        /// <param name="FileExts">资源管理器自己记的「打开方式」候选（HKCU\...\Explorer\FileExts），可以为 null</param>
        internal static int Remove(RegistryKey Classes, RegistryKey FileExts)
        {
            int n = 0;

            foreach (FileType t in Types)
            {
                bool had = false;

                using (RegistryKey ek = Classes.OpenSubKey(t.Ext, true))
                {
                    if (ek != null && string.Equals(ek.GetValue(string.Empty) as string, t.ProgId, StringComparison.OrdinalIgnoreCase))
                    {
                        ek.DeleteValue(string.Empty, false);
                        had = true;
                    }
                }

                using (RegistryKey ek = Classes.OpenSubKey(t.Ext))
                {
                    if (ek != null && ek.ValueCount == 0 && ek.SubKeyCount == 0) { ek.Close(); Classes.DeleteSubKey(t.Ext, false); }
                }

                bool progId;
                using (RegistryKey pk = Classes.OpenSubKey(t.ProgId)) { progId = pk != null; }
                if (progId)
                {
                    Classes.DeleteSubKeyTree(t.ProgId, false);
                    had = true;
                }

                //资源管理器看到过我们的 ProgID 会把它记进这里；ProgID 没了它也只是一条死引用，顺手清掉
                if (FileExts != null)
                {
                    try
                    {
                        using (RegistryKey ok = FileExts.OpenSubKey(t.Ext + @"\OpenWithProgids", true))
                        {
                            if (ok != null && ok.GetValueNames().Contains(t.ProgId, StringComparer.OrdinalIgnoreCase)) { ok.DeleteValue(t.ProgId, false); }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(Remove) + ".FileExts", ex);
                    }
                }

                if (had) { n++; }
            }

            return n;
        }

        private static bool SetIfDifferent(RegistryKey Key, string Name, string Value)
        {
            if (string.Equals(Key.GetValue(Name) as string, Value, StringComparison.Ordinal)) { return false; }
            Key.SetValue(Name, Value, RegistryValueKind.String);
            return true;
        }

        #endregion

        #region//通知资源管理器

        [DllImport("shell32.dll")]
        private static extern void SHChangeNotify(int wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private const int SHCNE_ASSOCCHANGED = 0x08000000;
        private const uint SHCNF_IDLIST = 0x0000;

        /// <summary>关联变了要喊一声，资源管理器才会当场重画图标，不然要等重启它。</summary>
        private static void NotifyShell()
        {
            try { SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero); }
            catch (Exception ex) { Operate.DoLog(nameof(NotifyShell), ex); }
        }

        #endregion
    }
}
