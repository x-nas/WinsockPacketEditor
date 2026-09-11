// 进程设置 · 第二批修复的验收跑测（编译进 bin/Debug 运行）
//   ④ 驱动类型 + 按名称拦截的名单 落库 / 读回 / 老库补列 / 备份 XML 往返
//   ⑤ GetMustTCP 的 userinfo 转义
//   ⑥ StopProxy 之后再 StartProxy，进程名单仍在（摘掉的是驱动上的，不是内存里的）
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Xml.Linq;
using WinsockPacketEditor;

static class T
{
    static int pass = 0, fail = 0;
    static void Check(string name, bool ok, string detail = "")
    {
        if (ok) { pass++; Console.WriteLine("  PASS  " + name + (detail.Length > 0 ? "   [" + detail + "]" : "")); }
        else { fail++; Console.WriteLine("  FAIL  " + name + (detail.Length > 0 ? "   [" + detail + "]" : "")); }
    }

    static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        string dir = Path.Combine(Path.GetTempPath(), "wpe-ps2-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);

        try
        {
            Persist(dir);
            Migrate(dir);
            Xml();
            Escape();
        }
        catch (Exception ex)
        {
            Console.WriteLine("!! " + ex);
            fail++;
        }

        try { Directory.Delete(dir, true); } catch { }
        Console.WriteLine();
        Console.WriteLine("pass " + pass + " / fail " + fail);
        return fail == 0 ? 0 : 1;
    }

    static void Fill()
    {
        Operate.ProxyConfig.Proxy.DriverType = 2;
        Operate.ProxyConfig.Proxy.lstSelectProcessName.Clear();
        Operate.ProxyConfig.Proxy.lstSelectProcessName.Add(new ProcessInfo(null, "game", 0, "game.exe", @"D:\Games\game.exe"));
        Operate.ProxyConfig.Proxy.lstSelectProcessName.Add(new ProcessInfo(null, "Launcher", 0, "Launcher.exe", ""));
        Operate.ProxyConfig.Proxy.lstSelectProcessName.Add(new ProcessInfo(null, "GAME", 0, "GAME.EXE", ""));   //同名（不分大小写）读回时只留一条
        Operate.ProxyConfig.Proxy.MustTCP_AppointPort = true;
        Operate.ProxyConfig.Proxy.MustTCP_AppointPortContent = "80, 443";
    }

    static void Reset()
    {
        Operate.ProxyConfig.Proxy.DriverType = 1;
        Operate.ProxyConfig.Proxy.lstSelectProcessName.Clear();
        Operate.ProxyConfig.Proxy.MustTCP_AppointPort = false;
        Operate.ProxyConfig.Proxy.MustTCP_AppointPortContent = "";
    }

    static string Names()
    {
        var l = new List<string>();
        foreach (ProcessInfo p in Operate.ProxyConfig.Proxy.lstSelectProcessName) { l.Add(p.ModuleName + "@" + p.ProcessPath); }
        return string.Join(" ", l);
    }

    // ───────────── ④ 落库往返 ─────────────
    static void Persist(string dir)
    {
        Console.WriteLine("④ 落库 / 读回（新库）");
        Operate.DataBase.dbPath = Path.Combine(dir, "new");
        Operate.DataBase.InitDB();

        Fill();
        Operate.SystemConfig.SaveProxyMode_ToDB();
        Reset();
        Check("Reset 之后内存是空的", Operate.ProxyConfig.Proxy.DriverType == 1 && Operate.ProxyConfig.Proxy.lstSelectProcessName.Count == 0);

        Operate.SystemConfig.LoadProxyMode_FromDB();
        Check("DriverType 读回 2", Operate.ProxyConfig.Proxy.DriverType == 2, "" + Operate.ProxyConfig.Proxy.DriverType);
        Check("名单读回 2 条（GAME.EXE 与 game.exe 合成一条）", Operate.ProxyConfig.Proxy.lstSelectProcessName.Count == 2, Names());
        Check("路径跟着回来了", Names().Contains(@"game.exe@D:\Games\game.exe"), Names());
        Check("端口列表读回后集合也重建了（属性 setter）", Operate.ProxyConfig.Proxy.MustTCP_AppointPort && Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(443) && !Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(8080));

        // 空名单也能存回去、读回来是空
        Operate.ProxyConfig.Proxy.lstSelectProcessName.Clear();
        Operate.SystemConfig.SaveProxyMode_ToDB();
        Fill();
        Operate.SystemConfig.LoadProxyMode_FromDB();
        Check("存了空名单 → 读回是空（不是留着上一份）", Operate.ProxyConfig.Proxy.lstSelectProcessName.Count == 0, Names());
    }

    // ───────────── ④ 老库补列 ─────────────
    static void Migrate(string dir)
    {
        Console.WriteLine("④ 老库（没有这两列）→ EnsureColumn 补列");
        string old = Path.Combine(dir, "old");
        Directory.CreateDirectory(old);
        string file = Path.Combine(old, Operate.DataBase.dbName);

        // 照老版本的样子建一张只有前面那些列的 ProxyMode（拿几列意思一下，足以让 CREATE TABLE IF NOT EXISTS 跳过）
        using (var c = new SQLiteConnection("Data Source=" + file + ";Version=3;"))
        {
            c.Open();
            using (var cmd = new SQLiteCommand("CREATE TABLE ProxyMode (ProxyIP_Auto BOOLEAN DEFAULT 1, Enable_SOCKS5 BOOLEAN DEFAULT 1, MustTCP BOOLEAN DEFAULT 1, FireWall_AutoClear_Expiry BOOLEAN DEFAULT 0);", c)) { cmd.ExecuteNonQuery(); }
        }

        Operate.DataBase.dbPath = old;
        Operate.DataBase.InitDB();

        var cols = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        using (var c = new SQLiteConnection("Data Source=" + file + ";Version=3;"))
        {
            c.Open();
            using (var cmd = new SQLiteCommand("PRAGMA table_info(ProxyMode);", c))
            using (var r = cmd.ExecuteReader()) { while (r.Read()) { cols.Add(Convert.ToString(r["name"])); } }
        }
        Check("InitDB 之后老库多了 DriverType 列", cols.Contains("DriverType"));
        Check("InitDB 之后老库多了 SelectProcessNames 列", cols.Contains("SelectProcessNames"));
        Check("老库里原有的列还在", cols.Contains("FireWall_AutoClear_Expiry") && cols.Contains("MustTCP"));
    }

    // ───────────── ④ 备份 XML ─────────────
    static void Xml()
    {
        Console.WriteLine("④ 备份 XML 往返");
        Fill();
        XElement xe = Operate.SystemConfig.GetProxyMode_XML();
        Check("导出的节里有 DriverType 与 SelectProcessNames", xe != null && xe.Element("DriverType") != null && xe.Element("SelectProcessNames") != null);

        Reset();
        Operate.SystemConfig.SetProxyMode_FromXML(xe);
        Check("导入后 DriverType = 2、名单 2 条", Operate.ProxyConfig.Proxy.DriverType == 2 && Operate.ProxyConfig.Proxy.lstSelectProcessName.Count == 2, Names());

        // 旧备份（没有这两个元素）导入不动它们
        Fill();
        var old = new XElement("ProxyMode", new XElement("MustTCP", "true"));
        Operate.SystemConfig.SetProxyMode_FromXML(old);
        Check("旧备份没有这两节 → 内存里的保持不动", Operate.ProxyConfig.Proxy.DriverType == 2 && Operate.ProxyConfig.Proxy.lstSelectProcessName.Count == 3, Names());

        // 备份里的 DriverType 越界 → 不采用
        Operate.ProxyConfig.Proxy.DriverType = 0;
        Operate.SystemConfig.SetProxyMode_FromXML(new XElement("ProxyMode", new XElement("DriverType", "9")));
        Check("DriverType 越界（9）不采用", Operate.ProxyConfig.Proxy.DriverType == 0);
    }

    // ───────────── ⑤ userinfo 转义 ─────────────
    static void Escape()
    {
        Console.WriteLine("⑤ GetMustTCP 转义");
        Operate.ProxyConfig.Proxy.MustTCP_Auth = true;
        Operate.ProxyConfig.Proxy.MustTCP_IP = "127.0.0.1";
        Operate.ProxyConfig.Proxy.MustTCP_Port = 1080;
        Operate.ProxyConfig.Proxy.MustTCP_UserName = "u@x";
        Operate.ProxyConfig.Proxy.MustTCP_PassWord = "p:w@rd";
        string s = Operate.SystemConfig.GetMustTCP();
        Check("@ 与 : 被转义（旧代码：密码里一个 @ 就把地址切错）", s == "socket5://u%40x:p%3Aw%40rd@127.0.0.1:1080", s);
        Uri u = new Uri(s.Replace("socket5://", "http://"));
        Check("按 URL 解回来是原样的账号密码", Uri.UnescapeDataString(u.UserInfo.Split(':')[0]) == "u@x" && Uri.UnescapeDataString(u.UserInfo.Split(':')[1]) == "p:w@rd" && u.Port == 1080, u.UserInfo);

        Operate.ProxyConfig.Proxy.MustTCP_Auth = false;
        Check("不认证时没有 userinfo", Operate.SystemConfig.GetMustTCP() == "socket5://127.0.0.1:1080");
    }
}
