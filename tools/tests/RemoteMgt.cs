// 远程管理（OWIN 自托管 + Web API + 静态页）的回归跑测 —— 真机冒烟清单里「远程管理起不起得来」那一条的自动化版本，
// 外加 2026-09-11 那一轮修复的每一条（认证解析 / 跨站拦截 / 并发 / 推送 / 页面入口 / 启动失败提示 / 登录退避 / 转义）。
//
// 编译进产品输出目录运行（依赖按目录解析），并且<b>必须带上产品的 exe.config</b>：
//   System.Web.Http.Owin 是按 Microsoft.Owin 4.2.2.0 编译的，而输出里是 4.2.3.0，
//   靠 AutoGenerateBindingRedirects 生成的那条重定向才接得上。
//
//   set VS=<vswhere -latest -property installationPath>
//   cd WinsockPacketEditor\bin\Release
//   "%VS%\MSBuild\Current\Bin\Roslyn\csc.exe" -nologo -out:RemoteMgt.exe -r:WinsockPacketEditor.exe -r:Microsoft.Owin.dll ^
//       -r:System.Net.Http.dll -r:System.Windows.Forms.dll ..\..\..\tools\tests\RemoteMgt.cs
//   copy /y WinsockPacketEditor.exe.config RemoteMgt.exe.config
//   RemoteMgt.exe            （跑完删掉 RemoteMgt.exe / .exe.config，别让它们进发布目录）
//
// 数据库指向临时目录（不碰真库）；用 localhost 而不是 127.0.0.1：HttpListener 注册 localhost 前缀不需要 URL ACL。
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WinsockPacketEditor;

static class T
{
    static int pass = 0, fail = 0, port;
    static void Check(string name, bool ok, string detail = "")
    {
        if (ok) { pass++; Console.WriteLine("  PASS  " + name + (detail.Length > 0 ? "   [" + detail + "]" : "")); }
        else { fail++; Console.WriteLine("  FAIL  " + name + (detail.Length > 0 ? "   [" + detail + "]" : "")); }
    }

    static int FreePort()
    {
        TcpListener l = new TcpListener(IPAddress.Loopback, 0);
        l.Start();
        int p = ((IPEndPoint)l.LocalEndpoint).Port;
        l.Stop();
        return p;
    }

    static string B64(string s) { return Convert.ToBase64String(Encoding.UTF8.GetBytes(s)); }

    //原样发请求（要控制每一个头，也要看服务器对畸形请求怎么反应）
    static string Raw(string head, string body = null)
    {
        using (TcpClient c = new TcpClient("localhost", port))
        using (NetworkStream s = c.GetStream())
        {
            s.ReadTimeout = 10000;
            string req = head.Replace("{PORT}", port.ToString()) + "\r\nHost: localhost:" + port + "\r\nConnection: close\r\n"
                + (body != null ? "Content-Length: " + Encoding.UTF8.GetByteCount(body) + "\r\n" : "") + "\r\n" + (body ?? "");
            byte[] b = Encoding.UTF8.GetBytes(req);
            s.Write(b, 0, b.Length);
            return new StreamReader(s, Encoding.UTF8).ReadToEnd();
        }
    }
    static int Code(string r) { return int.Parse(r.Substring(9, 3)); }
    static string Hdr(string r, string name)
    {
        foreach (string l in r.Split(new[] { "\r\n" }, StringSplitOptions.None)) { if (l.Length == 0) break; if (l.StartsWith(name + ":", StringComparison.OrdinalIgnoreCase)) return l.Substring(name.Length + 1).Trim(); }
        return "";
    }
    static string Body(string r) { int i = r.IndexOf("\r\n\r\n"); return i < 0 ? "" : r.Substring(i + 4); }

    //记下推给界面的东西（验「网页上改了账号，程序界面要跟着刷新」）
    sealed class FakeFeed : IUiFeed
    {
        public readonly List<string> Updates = new List<string>();
        public bool NeedsRows { get { return true; } }
        public void Append(FeedList List, object[] Rows) { }
        public void Replace(FeedList List, object[] Rows) { }
        public void Update(FeedList List, object Row) { lock (Updates) { Updates.Add(List + ":" + ((AccountRow)Row).UserName); } }
        public void Remove(FeedList List, string Id) { }
        public void Clear(FeedList List) { }
        public void Trim(FeedList List, int Keep) { }
    }

    [STAThread]
    static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;   //ProxyConfig.Proxy 的静态构造按工作目录找 qqwry.dat

        string tmp = Path.Combine(Path.GetTempPath(), "wpe-remotemgt-" + Guid.NewGuid().ToString("N").Substring(0, 8));
        Directory.CreateDirectory(tmp);
        Operate.DataBase.dbPath = tmp;
        Operate.DataBase.InitDB();

        //一个真的「界面线程」：WinForms 消息循环 + 程序里那句 InvokeAction（与 ShellForm 同一个写法）
        Control ui = null;
        ManualResetEvent uiReady = new ManualResetEvent(false);
        Thread uiThread = new Thread(() => { ui = new Control(); ui.CreateControl(); IntPtr h = ui.Handle; uiReady.Set(); Application.Run(); });
        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.IsBackground = true;
        uiThread.Start();
        uiReady.WaitOne();
        Operate.SystemConfig.InvokeAction = a => { if (ui.InvokeRequired) { ui.Invoke(a); } else { a(); } };

        FakeFeed feed = new FakeFeed();
        UI.AttachFeed(feed);

        try
        {
            port = FreePort();

            Operate.SystemConfig.IsRemote = true;
            Operate.SystemConfig.Remote_IP = "localhost";
            Operate.SystemConfig.Remote_Port = (ushort)port;
            Operate.SystemConfig.Remote_UserName = "admin";
            Operate.SystemConfig.Remote_PassWord = "p:ss";            //带冒号 —— 原来这样的密码永远登不上

            string startErr = Operate.SystemConfig.StartRemoteMGT();
            Check("① StartRemoteMGT 起得来（返回空串）", Operate.SystemConfig.WebServer != null && startErr == string.Empty, "http://localhost:" + port);

            string auth = "Authorization: Basic " + B64("admin:p:ss");

            //② Web API（System.Web.Http.Owin → Microsoft.Owin，走绑定重定向）；/ProxyCap/* 免认证
            string r = Raw("GET /ProxyCap/GetServerList HTTP/1.1");
            Check("② Web API：GET /ProxyCap/GetServerList → 200 + JSON 数组", Code(r) == 200 && Body(r).Contains("["), Code(r).ToString());

            //③ 页面入口：三个无扩展名地址与 .html 直链的响应一样（缓存头 / ETag / charset）
            foreach (string p in new[] { "/", "/index.html", "/ProxyAccount", "/SystemLog" })
            {
                r = Raw("GET " + p + " HTTP/1.1\r\n" + auth);
                string ct = Hdr(r, "Content-Type"), cc = Hdr(r, "Cache-Control");
                Check("③ " + p + " → 200、no-cache、有 ETag、charset=utf-8", Code(r) == 200 && cc == "no-cache" && Hdr(r, "ETag").Length > 0 && ct.Contains("charset=utf-8"),
                    Code(r) + " · " + ct + " · " + cc);
            }
            r = Raw("HEAD / HTTP/1.1\r\n" + auth);
            Check("③ HEAD / 不带 body", Code(r) == 200 && Body(r).Length == 0);

            string font = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web", "fonts"), "*.woff2").Select(Path.GetFileName).FirstOrDefault();
            if (font != null)
            {
                r = Raw("GET /fonts/" + font + " HTTP/1.1\r\n" + auth);
                Check("④ 字体长缓存 → immutable", Code(r) == 200 && Hdr(r, "Cache-Control").Contains("immutable"), Hdr(r, "Cache-Control"));
            }

            //⑤ 认证解析
            r = Raw("GET /SystemInfo/GetStartTime HTTP/1.1");
            Check("⑤ 不带认证 → 401，质询里声明 charset=\"UTF-8\"", Code(r) == 401 && Hdr(r, "WWW-Authenticate").Contains("charset=\"UTF-8\""), Hdr(r, "WWW-Authenticate"));
            Check("⑤ 密码里带冒号能登上", Code(Raw("GET /SystemInfo/GetStartTime HTTP/1.1\r\n" + auth)) == 200);
            Check("⑤ 小写 basic 能登上", Code(Raw("GET /SystemInfo/GetStartTime HTTP/1.1\r\nAuthorization: basic " + B64("admin:p:ss"))) == 200);
            foreach (var bad in new[] { Tuple.Create("只有 Basic", "Authorization: Basic"), Tuple.Create("非法 base64", "Authorization: Basic !!!notbase64"), Tuple.Create("没有冒号", "Authorization: Basic " + B64("admin")) })
            {
                int c = Code(Raw("GET /SystemInfo/GetStartTime HTTP/1.1\r\n" + bad.Item2));
                Check("⑤ " + bad.Item1 + " → 401（原来 500）", c == 401, c.ToString());
            }
            Operate.SystemConfig.Remote_UserName = "管理员"; Operate.SystemConfig.Remote_PassWord = "密码";
            Check("⑤ 中文用户名 / 密码（按 UTF-8 发）能登上", Code(Raw("GET /SystemInfo/GetStartTime HTTP/1.1\r\nAuthorization: Basic " + B64("管理员:密码"))) == 200);
            Operate.SystemConfig.Remote_UserName = "admin"; Operate.SystemConfig.Remote_PassWord = "p:ss";
            ResetGuard();

            //⑥ 跨站请求
            string form = "add=1&username=csrf_evil&password=x&enable=1";
            r = Raw("POST /account HTTP/1.1\r\n" + auth + "\r\nOrigin: http://evil.example\r\nContent-Type: application/x-www-form-urlencoded", form);
            Check("⑥ 跨站 POST /account → 403，账号没建", Code(r) == 403 && !Operate.ProxyConfig.Account.CheckProxyAccount_Exist("csrf_evil"), Code(r).ToString());
            r = Raw("POST /account HTTP/1.1\r\n" + auth + "\r\nReferer: http://evil.example/x.html\r\nContent-Type: application/x-www-form-urlencoded", form);
            Check("⑥ 只带 Referer 的跨站 POST 也 403", Code(r) == 403, Code(r).ToString());
            r = Raw("POST /account HTTP/1.1\r\n" + auth + "\r\nContent-Type: application/x-www-form-urlencoded", "add=1&username=ccp_user&password=x&enable=1");
            Check("⑥ 不带 Origin 的 POST（CCProxy 客户端）照常 → 200 + \"1\"", Code(r) == 200 && Body(r).Contains("1") && Operate.ProxyConfig.Account.CheckProxyAccount_Exist("ccp_user"));
            string json = "{\"UserName\":\"web_user\",\"Password\":\"pw\",\"IsEnable\":true,\"LimitLinks\":1,\"LimitDevices\":1,\"ExpiryTime\":\"2030-01-01T00:00:00\"}";
            r = Raw("POST /ProxyAccount/AddProxyAccount HTTP/1.1\r\n" + auth + "\r\nOrigin: http://localhost:" + port + "\r\nContent-Type: application/json", json);
            Check("⑥ 同源 POST（管理台自己）照常 → 200", Code(r) == 200 && Operate.ProxyConfig.Account.CheckProxyAccount_Exist("web_user"), Code(r).ToString());

            //⑦ 更新账号要推给程序界面；密码不进 URL
            AccountInfo wu = Operate.ProxyConfig.Account.lstAccountInfo.First(a => a.UserName == "web_user");
            string upd = "{\"AID\":\"" + wu.AID + "\",\"Password\":\"pw2\",\"IsEnable\":false,\"LimitLinks\":3,\"IsLimitLinks\":true,\"LimitDevices\":1,\"ExpiryTime\":\"2030-01-01T00:00:00\"}";
            r = Raw("POST /ProxyAccount/UpdateProxyAccount HTTP/1.1\r\n" + auth + "\r\nOrigin: http://localhost:" + port + "\r\nContent-Type: application/json", upd);
            bool pushed; lock (feed.Updates) { pushed = feed.Updates.Contains("Account:web_user"); }
            Check("⑦ 网页上更新账号 → 推了这一行给程序界面", Code(r) == 200 && pushed && !wu.IsEnable && wu.LimitLinks == 3, string.Join(",", feed.Updates));
            r = Raw("POST /account HTTP/1.1\r\n" + auth + "\r\nContent-Type: application/x-www-form-urlencoded", "edit=1&username=ccp_user&enable=0");
            lock (feed.Updates) { pushed = feed.Updates.Contains("Account:ccp_user"); }
            Check("⑦ CCProxy 编辑账号 → 也推了", Code(r) == 200 && pushed);
            r = Raw("GET /ProxyAccount/GetPlainPassword?AID=" + wu.AID + " HTTP/1.1\r\n" + auth);
            Check("⑦ GetPlainPassword 按 Id 取明文", Code(r) == 200 && Body(r).Contains("\"pw2\""), Body(r).Trim());
            r = Raw("POST /ProxyAccount/UpdateProxyAccount HTTP/1.1\r\n" + auth + "\r\nContent-Type: application/json", "");
            Check("⑦ 空请求体 → 400（原来 500）", Code(r) == 400, Code(r).ToString());
            r = Raw("GET /ProxyAccount/GetProxyAccountList HTTP/1.1\r\n" + auth);
            Check("⑦ 账号列表不带登录记录、字段名照旧", Code(r) == 200 && Body(r).Contains("\"UserName\":\"web_user\"") && Body(r).Contains("\"AIPInfo\":null"));

            //⑧ 并发：「界面线程」上一刻不停地改认证列表与账号列表，HTTP 线程同时读
            using (HttpClient http = new HttpClient { BaseAddress = new Uri("http://localhost:" + port + "/") })
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", B64("admin:p:ss"));
                bool stop = false;
                Thread writer = new Thread(() =>
                {
                    int n = 0;
                    while (!Volatile.Read(ref stop))
                    {
                        Operate.SystemConfig.InvokeAction(() =>
                        {
                            BindingList<AuthInfo> auths = Operate.ProxyConfig.Account.lstAuthInfo;
                            if (auths.Count > 200) { auths.Clear(); }         //RefreshAuthList 每秒就是这么清空再重填的
                            for (int i = 0; i < 20; i++) { auths.Add(new AuthInfo(Guid.NewGuid(), "10.0.0." + (i + 1), "局域网", true, DateTime.Now)); }
                            BindingList<AccountInfo> accs = Operate.ProxyConfig.Account.lstAccountInfo;
                            accs.Add(new AccountInfo { AID = Guid.NewGuid(), UserName = "tmp" + (n++), AIPInfo = new BindingList<AccountIPInfo>() });
                            if (accs.Count > 60) { accs.RemoveAt(accs.Count - 1); }
                        });
                    }
                });
                writer.Start();

                int ok = 0, err = 0; string sample = "";
                DateTime until = DateTime.Now.AddSeconds(4);
                while (DateTime.Now < until)
                {
                    foreach (string u in new[] { "ProxyInfo/GetProxyAuthList?take=300", "ProxyAccount/GetProxyAccountList", "SocketInfo/GetSocketLogList?take=300" })
                    {
                        HttpResponseMessage m = http.GetAsync(u).Result;
                        if (m.IsSuccessStatusCode) { ok++; } else { err++; if (sample.Length == 0) { sample = (int)m.StatusCode + " " + m.Content.ReadAsStringAsync().Result; } }
                    }
                }
                Volatile.Write(ref stop, true);
                writer.Join();
                Check("⑧ 边改边读 → 0 次失败（原来绝大多数 500「集合已修改」）", err == 0 && ok > 50, "成功 " + ok + " / 失败 " + err + (sample.Length > 0 ? " · " + sample.Substring(0, Math.Min(120, sample.Length)) : ""));
            }

            //⑨ CCProxy 模板里的用户名要转义
            Operate.SystemConfig.InvokeAction(() => Operate.ProxyConfig.Account.lstAccountInfo.Add(new AccountInfo { AID = Guid.NewGuid(), UserName = "x\"><script>alert(1)</script>$password", Password = "", AIPInfo = new BindingList<AccountIPInfo>() }));
            r = Raw("GET /account HTTP/1.1\r\n" + auth);
            string page = Body(r);
            Check("⑨ /account 里的用户名做了 HTML 转义，也不会被当成占位符再替换一次", Code(r) == 200 && !page.Contains("<script>alert(1)") && page.Contains("&lt;script&gt;alert(1)&lt;/script&gt;$password"));

            //⑩ 安全响应头 / 只出 JSON
            r = Raw("GET /SystemInfo/GetStartTime HTTP/1.1\r\n" + auth + "\r\nAccept: application/xml");
            Check("⑩ 安全头齐全、Accept: xml 也回 JSON", Hdr(r, "X-Content-Type-Options") == "nosniff" && Hdr(r, "X-Frame-Options") == "DENY" && Hdr(r, "Content-Type").StartsWith("application/json"),
                Hdr(r, "Content-Type"));

            //⑪ 运行时实际加载的是 4.2.3.0 —— 证明重定向生效
            string owin = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name == "Microsoft.Owin").Select(a => a.GetName().Version.ToString()).FirstOrDefault() ?? "<没加载>";
            Check("⑪ 加载的 Microsoft.Owin 版本 = 4.2.3.0", owin == "4.2.3.0", owin);

            //⑫ 登录失败退避：前 5 次照常 401，第 6 次起锁住，锁着时正确的密码也回 429
            ResetGuard();
            int[] codes = Enumerable.Range(0, 6).Select(_ => Code(Raw("GET /SystemInfo/GetStartTime HTTP/1.1\r\nAuthorization: Basic " + B64("admin:wrong")))).ToArray();
            r = Raw("GET /SystemInfo/GetStartTime HTTP/1.1\r\n" + auth);
            Check("⑫ 连续 6 次失败后锁住：正确密码也 429 + Retry-After", codes.All(c => c == 401) && Code(r) == 429 && Hdr(r, "Retry-After").Length > 0, string.Join(",", codes) + " → " + Code(r) + " Retry-After=" + Hdr(r, "Retry-After"));
            r = Raw("GET /ProxyCap/GetServerList HTTP/1.1");
            Check("⑫ 锁着时 /ProxyCap/*（WPC 客户端）不受影响", Code(r) == 200);
            ResetGuard();
            Check("⑫ 解锁后正确密码照常", Code(Raw("GET /SystemInfo/GetStartTime HTTP/1.1\r\n" + auth)) == 200);

            //⑬ 停服务
            Operate.SystemConfig.StopRemoteMGT(true);
            bool closed;
            try { using (TcpClient c = new TcpClient()) { c.Connect("localhost", port); closed = false; } }
            catch (SocketException) { closed = true; }
            Check("⑬ StopRemoteMGT 之后端口关掉、WebServer 置空", closed && Operate.SystemConfig.WebServer == null);

            //⑭ 启动失败要说实话
            TcpListener holder = new TcpListener(IPAddress.Any, 0);
            holder.Start();
            int busy = ((IPEndPoint)holder.LocalEndpoint).Port;
            Operate.SystemConfig.Remote_Port = (ushort)busy;
            string e1 = Operate.SystemConfig.StartRemoteMGT();
            holder.Stop();
            Check("⑭ 端口被占用 → 返回的原因里有端口号，不再说「请用管理员权限」", Operate.SystemConfig.WebServer == null && e1.Contains(busy.ToString()) && !e1.Contains("管理员"), e1);

            Operate.SystemConfig.Remote_IP = "10.254.254.254";
            string e2 = Operate.SystemConfig.StartRemoteMGT();
            RemoteSettingRow rs = Operate.SystemConfig.GetRemoteSetting();
            Check("⑭ 地址不在本机 → 说清楚是地址的问题", Operate.SystemConfig.WebServer == null && e2.Contains("10.254.254.254"), e2);
            Check("⑭ 设置页照实显示保存的地址并标 IPMissing", rs.IP == "10.254.254.254" && rs.IPMissing && rs.IPs[0] == "10.254.254.254", rs.IP + " / " + string.Join(",", rs.IPs));
        }
        catch (Exception ex)
        {
            Console.WriteLine("!! " + ex);
            fail++;
        }
        finally
        {
            try { Operate.SystemConfig.StopRemoteMGT(true); } catch { }
            try { Directory.Delete(tmp, true); } catch { }
        }

        Console.WriteLine();
        Console.WriteLine("  " + pass + " PASS / " + fail + " FAIL");
        return fail == 0 ? 0 : 1;
    }

    static void ResetGuard()
    {
        typeof(Socket_Web).GetNestedType("LoginGuard", BindingFlags.NonPublic).GetMethod("Reset", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
    }
}
