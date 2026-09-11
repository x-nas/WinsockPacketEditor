// 进程设置 · 第一批修复的验收跑测（编译进 bin/Debug 运行，依赖按目录解析）
//   ① MustTcpUdpRelay 端到端：对着 WPE 自己的 SOCKS5（UDP ASSOCIATE + 中继）打 UDP 回显
//   ② 端口集合：Trim / 中文逗号 / IPv6 端口 / URL 端口
//   ③ ValidateProcessSetting：八种该拦的情形 + 两种该放的情形，且失败时不碰驱动
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        try
        {
            Ports();
            Validate();
            Relay().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine("!! " + ex);
            fail++;
        }
        finally
        {
            try { Operate.ProxyConfig.Proxy.StopProxy(); } catch { }
        }

        Console.WriteLine();
        Console.WriteLine("pass " + pass + " / fail " + fail);
        return fail == 0 ? 0 : 1;
    }

    // ───────────── ② 端口集合 ─────────────
    static void Ports()
    {
        Console.WriteLine("② 端口集合");
        var P = typeof(Operate.ProxyConfig.Proxy);

        Operate.ProxyConfig.Proxy.MustTCP_AppointPort = true;
        Operate.ProxyConfig.Proxy.MustTCP_AppointPortContent = "80, 443 ，7777;abc,0,70000";
        Check("\" 443\" 命中（旧代码 Split 不 Trim，永远匹配不上）", Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(443));
        Check("字符串重载 \" 443 \" 命中", Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(" 443 "));
        Check("中文逗号后的 7777 命中", Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(7777));
        Check("8080 不在名单里", !Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(8080));
        Check("越界的 0 / 70000 被丢掉", !Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(0) && !Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(70000));

        List<string> bad;
        string norm = Operate.ProxyConfig.Proxy.NormalizeAppointPorts("443 ,80，7777;abc,0", out bad);
        Check("规范化成 \"80,443,7777\"", norm == "80,443,7777", norm);
        Check("认不出来的片段是 abc 与 0", bad.Count == 2 && bad[0] == "abc" && bad[1] == "0", string.Join("|", bad));

        Operate.ProxyConfig.Proxy.MustTCP_AppointPort = false;
        Check("没勾指定端口时任何端口都转", Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(1) && Operate.ProxyConfig.Proxy.IsMustTCP_ByPort("x"));

        Check("PortOfAddress ip:port", Operate.ProxyConfig.Proxy.PortOfAddress("1.2.3.4:80") == 80);
        Check("PortOfAddress [v6]:port（旧代码 Split(':')[1] 取到的是地址段）", Operate.ProxyConfig.Proxy.PortOfAddress("[::1]:443") == 443);
        Check("PortOfAddress host:port", Operate.ProxyConfig.Proxy.PortOfAddress("example.com:9000") == 9000);
        Check("PortOfAddress 坏输入 → -1", Operate.ProxyConfig.Proxy.PortOfAddress("nonsense") == -1 && Operate.ProxyConfig.Proxy.PortOfAddress(null) == -1);
        Check("PortOfUrl http://h:8080/x → 8080（旧代码按 scheme 写死 80）", Operate.ProxyConfig.Proxy.PortOfUrl("http://h:8080/x", false) == 8080);
        Check("PortOfUrl https://h/ → 443", Operate.ProxyConfig.Proxy.PortOfUrl("https://h/", true) == 443);
        Check("PortOfUrl 坏 URL → 按 scheme", Operate.ProxyConfig.Proxy.PortOfUrl("not a url", true) == 443);
    }

    // ───────────── ③ 保存前校验 ─────────────
    static void Validate()
    {
        Console.WriteLine("③ ValidateProcessSetting");
        var X = Operate.ProxyConfig.Proxy.IsLoadDriver;
        Operate.ProxyConfig.Proxy.Enable_HTTP = true;
        Operate.ProxyConfig.Proxy.Enable_SOCKS5 = true;
        Operate.ProxyConfig.Proxy.Enable_Auth = true;
        Operate.ProxyConfig.Proxy.SOCKS5_Port = 1080;
        Operate.ProxyConfig.Proxy.HTTP_Port = 1081;
        string ports;

        // 默认配置：EnableAuth=1、MustTCP_Auth=0、127.0.0.1:1080 —— 旧代码放行，进程当场断网
        string r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "127.0.0.1", 1080, false, "", false, "", "", out ports);
        Check("默认配置被拦：本机 SOCKS5 开着认证而没勾「需要认证」", r.Contains("身份认证"), r);

        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "127.0.0.1", 1080, false, "", true, "u", "p", out ports);
        Check("勾了认证并填了账号 → 放行", r == "", r);

        Operate.ProxyConfig.Proxy.Enable_Auth = false;
        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "127.0.0.1", 1080, false, "", false, "", "", out ports);
        Check("本机 SOCKS5 没开认证 → 不勾也放行", r == "", r);

        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "localhost", 1080, false, "", false, "", "", out ports);
        Check("localhost 当本机放行", r == "", r);

        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "127.0.0.1", 1081, false, "", false, "", "", out ports);
        Check("转到 HTTP 代理自己的端口 → 拦（成环）", r.Contains("成环"), r);

        Operate.ProxyConfig.Proxy.Enable_SOCKS5 = false;
        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "127.0.0.1", 1080, false, "", false, "", "", out ports);
        Check("本机 SOCKS5 没启用 → 拦", r.Contains("没有启用"), r);
        Operate.ProxyConfig.Proxy.Enable_SOCKS5 = true;

        Operate.ProxyConfig.Proxy.Enable_HTTP = false;
        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(false, "", 0, false, "", false, "", "", out ports);
        Check("HTTP 代理没开 → 连关着强制转代理也拦", r.Contains("HTTP"), r);
        Operate.ProxyConfig.Proxy.Enable_HTTP = true;

        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "10.0.0.5", 1080, true, "80,abc, 443", false, "", "", out ports);
        Check("端口列表里的 abc 被点名", r.Contains("abc"), r);

        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "10.0.0.5", 1080, true, " , ", false, "", "", out ports);
        Check("勾了指定端口却是空的 → 拦", r.Contains("至少一个"), r);

        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "10.0.0.5", 1080, true, "443 , 80，8080", false, "", "", out ports);
        Check("外部地址 + 合法端口 → 放行，且端口被规范化", r == "" && ports == "80,443,8080", r + " / " + ports);

        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "10.0.0.5", 1080, false, "", true, "u", "", out ports);
        Check("勾了认证密码为空 → 拦", r.Contains("账号和密码"), r);

        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(true, "999.1.1.1", 1080, false, "", false, "", "", out ports);
        Check("地址不合法 → 拦", r.Contains("地址错误"), r);

        r = Operate.ProxyConfig.Proxy.ValidateProcessSetting(false, "", 0, false, "junk", false, "", "", out ports);
        Check("强制转代理关着：地址 / 端口 / 认证一概不查", r == "", r);

        // 失败的保存不能碰驱动
        Operate.ProxyConfig.Proxy.Enable_HTTP = false;
        string s = Operate.ProxyConfig.Proxy.SaveProcessSetting(1, true, "127.0.0.1", 1080, false, "", false, "", "", new List<int>()).GetAwaiter().GetResult();
        Check("SaveProcessSetting 校验失败 → 返回错误且 IsLoadDriver 没动", s.Length > 0 && Operate.ProxyConfig.Proxy.IsLoadDriver == X, s);
        Operate.ProxyConfig.Proxy.Enable_HTTP = true;

        Check("自身进程不进列表", Array.TrueForAll(Operate.ProxyConfig.Proxy.GetProcessRows(), x => x.ProcessID != Operate.ProxyConfig.Proxy.SelfProcessId));
    }

    // ───────────── ① UDP 中继 ─────────────
    static async Task Relay()
    {
        Console.WriteLine("① MustTcpUdpRelay 端到端（WPE 自己的 SOCKS5 → UDP 回显）");

        // 起 WPE 自己的 SOCKS5：不认证、不起 HTTP（SunnyNet 那半边与本测试无关）
        Operate.ProxyConfig.Proxy.Enable_Auth = false;
        Operate.ProxyConfig.Proxy.Enable_HTTP = false;
        Operate.ProxyConfig.Proxy.Enable_SOCKS5 = true;
        Operate.ProxyConfig.Proxy.EnableFireWall = false;
        Operate.ProxyConfig.Proxy.ProxyIP_Auto = true;
        Operate.ProxyConfig.Proxy.SOCKS5_Port = 18080;
        Operate.ProxyConfig.Proxy.HookUDP_Req = true;
        Operate.ProxyConfig.Proxy.HookUDP_Resp = true;

        bool started = Operate.ProxyConfig.Proxy.StartProxy();
        Check("SOCKS5 起来了（18080）", started && Operate.ProxyConfig.Proxy.IsRunning);
        if (!started) { return; }

        // UDP 回显服务器（目标）：把收到的原样发回，前面加 "echo:"
        var echo = new UdpClient(new IPEndPoint(IPAddress.Loopback, 0));
        var echoEp = (IPEndPoint)echo.Client.LocalEndPoint;
        int echoed = 0;
        _ = Task.Run(async () =>
        {
            try
            {
                while (true)
                {
                    var r = await echo.ReceiveAsync();
                    Interlocked.Increment(ref echoed);
                    byte[] reply = Encoding.ASCII.GetBytes("echo:" + Encoding.ASCII.GetString(r.Buffer));
                    await echo.SendAsync(reply, reply.Length, r.RemoteEndPoint);
                }
            }
            catch { }
        });

        // 中继客户端：应答收进一个队列（生产上这里是 SunnyNet 的 UDPTools.SendMessage）
        var got = new ConcurrentQueue<Tuple<long, string>>();
        var logs = new ConcurrentQueue<string>();
        var relay = new MustTcpUdpRelay(
            () => new MustTcpUdpRelay.Target { Auth = false, IP = "127.0.0.1", Port = 18080 },
            (t, d) => got.Enqueue(Tuple.Create(t, Encoding.ASCII.GetString(d))),
            (w, t) => logs.Enqueue(w + ": " + t));

        // 同一个会话（TheologyID 7）连发 3 个，并发发起 —— 第一批同时到达也只能建一条关联
        var sends = new List<Task<bool>>();
        for (int i = 0; i < 3; i++) { sends.Add(relay.SendAsync(7, echoEp, Encoding.ASCII.GetBytes("m" + i))); }
        bool[] ok = await Task.WhenAll(sends);
        Check("3 个数据报都发出去了", Array.TrueForAll(ok, x => x), string.Join(",", ok));

        await WaitUntil(() => got.Count >= 3, 3000);
        Check("3 个应答都回到了会话 7（旧代码：UdpClient 发完就 Dispose，一个都回不来）", got.Count == 3, "got " + got.Count + " echoed " + echoed);
        var all = new List<Tuple<long, string>>(got);
        Check("应答内容对得上且都交给了会话 7", all.TrueForAll(x => x.Item1 == 7 && x.Item2.StartsWith("echo:m")), string.Join(" ", all.ConvertAll(x => x.Item1 + "/" + x.Item2)));
        Check("只建了 1 条关联", relay.Count == 1, "count " + relay.Count);
        var st = relay.StatsOf(7);
        Check("关联计数 发 3 / 收 3", st.Sent == 3 && st.Received == 3, st.Sent + "/" + st.Received);

        // 第二个会话 → 第二条关联；第一条不受影响
        await relay.SendAsync(8, echoEp, Encoding.ASCII.GetBytes("k"));
        await WaitUntil(() => got.Count >= 4, 2000);
        Check("会话 8 有自己的关联，应答交给 8", relay.Count == 2 && got.Count == 4 && new List<Tuple<long, string>>(got).Exists(x => x.Item1 == 8 && x.Item2 == "echo:k"), "count " + relay.Count + " got " + got.Count);

        // 关一条
        relay.Close(7);
        await Task.Delay(100);
        Check("Close(7) 之后只剩 1 条", relay.Count == 1, "count " + relay.Count);

        // 关掉的会话再发 → 重建一条（不是永久死掉）
        bool again = await relay.SendAsync(7, echoEp, Encoding.ASCII.GetBytes("z"));
        await WaitUntil(() => got.Count >= 5, 2000);
        Check("关掉之后再发会重建关联并照常收到应答", again && relay.Count == 2 && got.Count == 5, "count " + relay.Count + " got " + got.Count);

        // 静置回收
        relay.IdleTimeout = TimeSpan.FromMilliseconds(50);
        await Task.Delay(120);
        int swept = relay.SweepIdle();
        await Task.Delay(50);
        Check("SweepIdle 把静置的两条都收了", swept == 2 && relay.Count == 0, "swept " + swept + " count " + relay.Count);

        // 服务端断开时这边跟着收掉：停掉 SOCKS5，关联应该自己消失
        relay.IdleTimeout = TimeSpan.FromMinutes(3);
        await relay.SendAsync(9, echoEp, Encoding.ASCII.GetBytes("q"));
        await WaitUntil(() => got.Count >= 6, 2000);
        Check("停服务前会话 9 正常", relay.Count == 1 && got.Count == 6, "count " + relay.Count);
        Operate.ProxyConfig.Proxy.StopProxy();
        await WaitUntil(() => relay.Count == 0, 3000);
        Check("SOCKS5 停了 → 控制连接断开 → 关联自己收掉", relay.Count == 0, "count " + relay.Count);

        // 服务不在时发送要失败而不是挂住
        bool dead = await relay.SendAsync(10, echoEp, Encoding.ASCII.GetBytes("x"));
        Check("服务不在：SendAsync 返回 false、不抛", !dead && relay.Count == 0);

        echo.Close();
        Console.WriteLine("  日志 " + logs.Count + " 条" + (logs.Count > 0 ? "：" + string.Join(" | ", logs) : ""));
    }

    static async Task WaitUntil(Func<bool> f, int ms)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (!f() && sw.ElapsedMilliseconds < ms) { await Task.Delay(20); }
    }
}
