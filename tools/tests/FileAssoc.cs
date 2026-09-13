// 文件图标关联（ClassObject/FileAssociation.cs）的跑测 —— <b>不碰真注册表</b>
//
// 注册表写在一个私有的 app hive 里（RegLoadAppKey：拿一个临时文件当注册表挂进来，
// 不挂到 HKCU / HKLM 下面，别的进程看不见，句柄关掉就卸下），图标拷进临时目录。
// 所以能放心在开发机上跑；真注册表那一路（HKCU\Software\Classes + 通知资源管理器）靠真机看。
//
//   cd WinsockPacketEditor\bin\Debug
//   "%VS%\MSBuild\Current\Bin\Roslyn\csc.exe" -nologo -out:FileAssocTest.exe -r:WinsockPacketEditor.exe ..\..\..\tools\tests\FileAssoc.cs
//   FileAssocTest.exe          （跑完删掉 FileAssocTest.exe）
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using WinsockPacketEditor;

static class T
{
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    static extern int RegLoadAppKey(string lpFile, out IntPtr phkResult, int samDesired, int dwOptions, int Reserved);

    static int pass = 0, fail = 0;
    static void Check(string name, bool ok, string detail = "")
    {
        if (ok) { pass++; Console.WriteLine("  PASS  " + name + (detail.Length > 0 ? "   [" + detail + "]" : "")); }
        else { fail++; Console.WriteLine("  FAIL  " + name + (detail.Length > 0 ? "   [" + detail + "]" : "")); }
    }

    static readonly Type FA = typeof(FileAssociation);
    static object Invoke(string name, params object[] a)
    {
        return FA.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, a);
    }
    static T2 Field<T2>(object o, string name)
    {
        return (T2)o.GetType().GetField(name).GetValue(o);
    }

    static string Def(RegistryKey root, string path)
    {
        using (RegistryKey k = root.OpenSubKey(path)) { return k == null ? null : k.GetValue(string.Empty) as string; }
    }
    static bool Exists(RegistryKey root, string path)
    {
        using (RegistryKey k = root.OpenSubKey(path)) { return k != null; }
    }

    static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

        string tmp = Path.Combine(Path.GetTempPath(), "wpe-fa-test-" + Guid.NewGuid().ToString("N").Substring(0, 8));
        Directory.CreateDirectory(tmp);
        RegistryKey hive = null;

        try
        {
            IntPtr h;
            int rc = RegLoadAppKey(Path.Combine(tmp, "test.hive"), out h, 0xF003F, 0, 0);
            if (rc != 0) { Console.WriteLine("!! RegLoadAppKey 失败 " + rc); return 1; }
            hive = RegistryKey.FromHandle(new SafeRegistryHandle(h, true));

            RegistryKey classes = hive.CreateSubKey(@"Software\Classes");
            RegistryKey fileExts = hive.CreateSubKey(@"FileExts");

            //「别人的」：.rp 归 Axure（HKCU 里就有）、.sc 是 Windows 为「打开方式」自动建的、.pml 只挂了系统的搜索过滤器（没有默认值）
            using (RegistryKey k = classes.CreateSubKey(".rp")) { k.SetValue(string.Empty, "Axure.rp"); }
            using (RegistryKey k = classes.CreateSubKey(".pml\\PersistentHandler")) { k.SetValue(string.Empty, "{5e941d80-bf96-11cd-b579-08002b30bfeb}"); }
            Dictionary<string, string> machine = new Dictionary<string, string> { { ".sc", "sc_auto_file" } };   //模拟 HKLM 那一半
            Func<string, string> ownerOf = ext =>
            {
                string v = Def(classes, ext);
                if (!string.IsNullOrEmpty(v)) { return v; }
                string m; return machine.TryGetValue(ext, out m) ? m : null;
            };

            //explorer 自己记的「打开方式」候选：一条我们的，一条别人的
            using (RegistryKey k = fileExts.CreateSubKey(@".fp\OpenWithProgids")) { k.SetValue("WPE64.FilterList", new byte[0], RegistryValueKind.None); k.SetValue("Other.fp", new byte[0], RegistryValueKind.None); }

            //① 图标暂存
            string srcIco = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wpe-data.ico");
            string iconDir = Path.Combine(tmp, "Icons");
            string staged = (string)Invoke("StageIcon", srcIco, iconDir);
            Check("① 图标拷到固定目录，文件名带内容哈希", staged != null && File.Exists(staged)
                && Path.GetFileName(staged).StartsWith("wpe-data.") && File.ReadAllBytes(staged).SequenceEqual(File.ReadAllBytes(srcIco)), Path.GetFileName(staged ?? "<null>"));
            DateTime t1 = File.GetLastWriteTimeUtc(staged);
            File.WriteAllText(Path.Combine(iconDir, "wpe-data.deadbeef.ico"), "old");
            System.Threading.Thread.Sleep(20);
            string staged2 = (string)Invoke("StageIcon", srcIco, iconDir);
            Check("② 内容没变就不重写；别的哈希的旧图标被清掉", staged2 == staged && File.GetLastWriteTimeUtc(staged2) == t1
                && !File.Exists(Path.Combine(iconDir, "wpe-data.deadbeef.ico")));
            Check("③ 源文件不在 → 返回 null（不注册）", Invoke("StageIcon", Path.Combine(tmp, "nope.ico"), iconDir) == null);

            //④ 第一次注册
            Func<FileAssociation.FileType, string> name = ft => "WPE x64 " + ft.Ext.ToUpperInvariant();
            object r1 = Invoke("Apply", classes, ownerOf, staged, name);
            List<string> claimed = Field<List<string>>(r1, "Claimed"), foreign = Field<List<string>>(r1, "Foreign");
            Check("④ 空闲的 11 种都认领、.rp / .sc 两种有主的跳过", Field<bool>(r1, "Changed") && claimed.Count == 11
                && foreign.OrderBy(x => x).SequenceEqual(new[] { ".rp", ".sc" }), "认领 " + claimed.Count + " · 跳过 " + string.Join(" ", foreign));

            bool allOk = FileAssociation.Types.Where(ft => claimed.Contains(ft.Ext)).All(ft =>
                Def(classes, ft.Ext) == ft.ProgId && Def(classes, ft.ProgId) == "WPE x64 " + ft.Ext.ToUpperInvariant()
                && Def(classes, ft.ProgId + "\\DefaultIcon") == staged && !Exists(classes, ft.ProgId + "\\shell"));
            Check("⑤ 后缀 → ProgID → 类型名 + DefaultIcon 都写对了，且没有注册打开方式（无 shell 子键）", allOk);
            Check("⑥ 有主的原样：.rp 仍归 Axure、没建 WPE64.RobotList；.sc 没建 HKCU 键",
                Def(classes, ".rp") == "Axure.rp" && !Exists(classes, "WPE64.RobotList") && !Exists(classes, ".sc") && !Exists(classes, "WPE64.SendCollection"));
            Check("⑦ .pml 只挂了系统过滤器（没默认值）→ 认领，过滤器子键还在",
                Def(classes, ".pml") == "WPE64.MapLocal" && Def(classes, ".pml\\PersistentHandler") != null);

            //⑧ 再跑一遍：什么都不改
            object r2 = Invoke("Apply", classes, ownerOf, staged, name);
            Check("⑧ 第二次注册：值都没变 → Changed = false", !Field<bool>(r2, "Changed") && Field<List<string>>(r2, "Claimed").Count == 11);

            //⑨ 换语言：类型名跟着变
            object r3 = Invoke("Apply", classes, ownerOf, staged, (Func<FileAssociation.FileType, string>)(ft => "WPE x64 " + ft.Ext));
            Check("⑨ 类型名变了 → Changed = true，只改了名字", Field<bool>(r3, "Changed") && Def(classes, "WPE64.Backup") == "WPE x64 .sb"
                && Def(classes, "WPE64.Backup\\DefaultIcon") == staged);

            //⑩ 清除
            int n = (int)Invoke("Remove", classes, fileExts);
            bool gone = FileAssociation.Types.All(ft => !Exists(classes, ft.ProgId));
            bool extsGone = FileAssociation.Types.Where(ft => ft.Ext != ".rp" && ft.Ext != ".pml" && ft.Ext != ".sc").All(ft => !Exists(classes, ft.Ext));
            Check("⑩ 清除：11 种全清、ProgID 一个不剩、空了的后缀键连键删掉", n == 11 && gone && extsGone, "清掉 " + n + " 种");
            Check("⑪ 清除不碰别人的：.rp 仍归 Axure；.pml 去掉默认值、过滤器子键保留",
                Def(classes, ".rp") == "Axure.rp" && Exists(classes, ".pml") && string.IsNullOrEmpty(Def(classes, ".pml")) && Def(classes, ".pml\\PersistentHandler") != null);
            string[] ow;
            using (RegistryKey k = fileExts.OpenSubKey(@".fp\OpenWithProgids")) { ow = k.GetValueNames(); }
            Check("⑫ 资源管理器记的「打开方式」候选：删了我们那条，别人的留着", !ow.Contains("WPE64.FilterList") && ow.Contains("Other.fp"), string.Join(",", ow));

            //⑬ 状态：真注册表只读一遍，看不写也能跑（只读 HKCR，不改任何东西）
            FileAssociation.Status st = FileAssociation.GetStatus();
            Check("⑬ GetStatus 在真环境下只读得起来", st != null && !st.IconMissing, "enabled=" + st.Enabled + " claimed=" + st.Claimed.Length + " foreign=" + string.Join(" ", st.Foreign));

            classes.Close(); fileExts.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("!! " + ex);
            fail++;
        }
        finally
        {
            if (hive != null) { hive.Close(); }
            try { Directory.Delete(tmp, true); } catch { }
        }

        Console.WriteLine();
        Console.WriteLine("  " + pass + " PASS / " + fail + " FAIL");
        return fail == 0 ? 0 : 1;
    }
}
