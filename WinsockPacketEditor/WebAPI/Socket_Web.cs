using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 远程管理台（OWIN 自托管，HttpListener）。管道自上而下：
    ///
    ///   ① 安全响应头            nosniff / 禁止被嵌进 iframe / 不带 Referer
    ///   ② 跨站请求拦截          会改数据的请求，Origin（或 Referer）不是本站就 403
    ///   ③ 认证                  HTTP Basic；/ProxyCap/* 免认证（给 WPC 客户端）；连续失败按 IP 退避
    ///   ④ /account              CCProxy 兼容接口
    ///   ⑤ Web API               五个控制器
    ///   ⑥ 页面入口改写          / → /index.html，/ProxyAccount → /ProxyAccount.html，/SystemLog → /SystemLog.html
    ///   ⑦ 静态文件              Web\ 目录，统一的缓存头与 charset
    ///
    /// ⚠️ 这里<b>不再 try/catch 吞异常</b>：原来管道搭到一半抛了（比如绑定重定向缺了，UseWebApi 那一步就抛）
    /// 会被吞掉，服务照样「启动成功」，只是所有接口都 404、日志里什么都没有。现在抛给 WebApp.Start，
    /// 由 StartRemoteMGT 报出来。
    /// </summary>
    public class Socket_Web
    {
        public void Configuration(IAppBuilder app)
        {
            #region//① 安全响应头

            app.Use(async (context, next) =>
            {
                IHeaderDictionary h = context.Response.Headers;
                h["X-Content-Type-Options"] = "nosniff";      //按 Content-Type 走，不猜
                h["X-Frame-Options"] = "DENY";                //管理台不该被别的页面嵌进 iframe（点击劫持）
                h["Referrer-Policy"] = "no-referrer";         //从管理台点出去的链接不带管理台地址
                await next();
            });

            #endregion

            #region//② 跨站请求拦截

            /*
                浏览器会替<b>已经登录过</b>管理台的人自动带上 Basic 凭据 —— 别的网站里的一个自动提交的表单，
                就能以管理员身份往这里 POST。/account（表单）与 DeleteProxyAccount（"=GUID" 表单体）都是
                浏览器不做预检的「简单请求」，实测跨站增删账号都能成（2026-09-11）。

                判据：会改数据的方法（除 GET / HEAD / OPTIONS 外）如果带了 Origin（没有就看 Referer），
                它的「主机:端口」必须与请求的 Host 相同。两个都没带的放行 —— CCProxy 客户端、WPC 这类
                非浏览器客户端不发这两个头；而浏览器发跨站 POST 一定带 Origin。
            */
            app.Use(async (context, next) =>
            {
                if (IsCrossSiteWrite(context.Request))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }

                await next();
            });

            #endregion

            #region//③ 认证

            app.Use(async (context, next) =>
            {
                //无需认证的路径（WPC 客户端取节点 / 公告）
                if (context.Request.Path.Value != null && context.Request.Path.Value.StartsWith("/ProxyCap/", StringComparison.OrdinalIgnoreCase))
                {
                    await next();
                    return;
                }

                string ip = context.Request.RemoteIpAddress ?? string.Empty;

                int wait = LoginGuard.SecondsLocked(ip);
                if (wait > 0)
                {
                    context.Response.StatusCode = 429;
                    context.Response.Headers["Retry-After"] = wait.ToString();
                    return;
                }

                string header = context.Request.Headers["Authorization"];

                if (!string.IsNullOrEmpty(header))
                {
                    string username, password;

                    if (TryParseBasic(header, out username, out password) && Operate.ProxyConfig.Account.IsValidAdmin(username, password))
                    {
                        LoginGuard.Succeeded(ip);
                        context.Request.User = new GenericPrincipal(new GenericIdentity(username), null);
                        await next();
                        return;
                    }

                    //带了凭据却不对（包括格式不对）才算一次失败；没带的只是浏览器的第一次试探
                    LoginGuard.Failed(ip);
                }

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //charset="UTF-8"：告诉浏览器用 UTF-8 编码凭据（RFC 7617），中文用户名 / 密码才对得上
                context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"WPE x64\", charset=\"UTF-8\"";
            });

            #endregion

            #region//④ /account（CCProxy 兼容接口）

            app.Use(async (context, next) =>
            {
                if (context.Request.Path != new PathString("/account"))
                {
                    await next();
                    return;
                }

                if (context.Request.Method == "GET")
                {
                    string sReturn = CCProxy_Controller.QueryUserAll();

                    if (!string.IsNullOrEmpty(sReturn))
                    {
                        context.Response.ContentType = "text/html; charset=utf-8";
                        await context.Response.WriteAsync(sReturn);
                    }
                }
                else if (context.Request.Method == "POST")
                {
                    await HandleAccountPost(context);
                }
            });

            #endregion

            #region//⑤ Web API

            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            //只出 JSON。XML 格式化器留着只是多一种要操心的输出（请求头写 Accept: application/xml 就换格式）
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            app.UseWebApi(config);

            #endregion

            #region//⑥ 页面入口改写

            /*
                原来这三个入口各有一段手写中间件（复制粘贴了三份）自己读文件往外写：
                没有 Cache-Control、没有 ETag（绕开了下面静态文件那套缓存规则，也不回 304），
                Content-Type 不带 charset，HEAD 也写 body。另外还有一段 UseDefaultFiles 是死代码 ——
                没配 FileSystem（默认找的是程序目录而不是 Web\），而且排在 UseStaticFiles 后面。

                现在只改写路径，交给静态文件中间件去发，三个入口与 .html 直链的响应完全一样。
                ⚠️ 文件名（ProxyAccount / SystemLog）不能改：这两个无扩展名的地址是对外的。
            */
            app.Use(async (context, next) =>
            {
                string to;
                if (Pages.TryGetValue(context.Request.Path.Value ?? string.Empty, out to))
                {
                    context.Request.Path = new PathString(to);
                }

                await next();
            });

            #endregion

            #region//⑦ 静态文件

            app.UseStaticFiles(new StaticFileOptions
            {
                FileSystem = new PhysicalFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web")),
                ServeUnknownFileTypes = true,

                /*
                    ⚠️ <b>必须显式给 Cache-Control，一个都不能少。</b>

                    不给的话浏览器走「启发式缓存」：拿 Last-Modified 距今时间的约 10% 当新鲜期，
                    在那段时间里<b>连条件请求都不发</b> —— 一个上个月改的文件能被缓存好几天。
                    而这几个文件名里<b>没有版本号也没有内容哈希</b>（是手写的静态页，不过 Vite），
                    于是「程序升级了、管理台还是旧界面」，且看不出任何异常。
                    真撞到过：账号页的「启用」列改成只读之后，用户那边还是能点。

                    no-cache 不是「不缓存」：它照样存，只是<b>每次都回来用 ETag 校验一次</b>，
                    没变就是一个 304（几十字节），变了立刻拿到新的。
                    字体是例外 —— 那三个 woff2 的内容永不变，值得长缓存。

                    文本类顺带补上 charset=utf-8（Web\ 下的文件都是 UTF-8），不必靠页面里的 meta 猜。
                */
                OnPrepareResponse = ctx =>
                {
                    string path = ctx.OwinContext.Request.Path.Value ?? string.Empty;

                    ctx.OwinContext.Response.Headers["Cache-Control"] =
                        path.EndsWith(".woff2", StringComparison.OrdinalIgnoreCase)
                            ? "public, max-age=31536000, immutable"
                            : "no-cache";

                    string ct = ctx.OwinContext.Response.ContentType ?? string.Empty;
                    if ((ct.StartsWith("text/", StringComparison.OrdinalIgnoreCase) || ct.IndexOf("javascript", StringComparison.OrdinalIgnoreCase) >= 0)
                        && ct.IndexOf("charset", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        ctx.OwinContext.Response.ContentType = ct + "; charset=utf-8";
                    }
                }
            });

            #endregion
        }

        #region//页面入口

        private static readonly Dictionary<string, string> Pages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "/", "/index.html" },
            { "/ProxyAccount", "/ProxyAccount.html" },
            { "/SystemLog", "/SystemLog.html" },
        };

        #endregion

        #region//跨站判定

        internal static bool IsCrossSiteWrite(IOwinRequest Request)
        {
            string method = Request.Method ?? string.Empty;
            if (method == "GET" || method == "HEAD" || method == "OPTIONS") { return false; }

            string source = Request.Headers["Origin"];
            if (string.IsNullOrEmpty(source)) { source = Request.Headers["Referer"]; }

            //两个都没带：不是浏览器发的（CCProxy 客户端 / WPC），放行
            if (string.IsNullOrEmpty(source)) { return false; }

            //沙箱 iframe、file:// 页面发来的 Origin 是字面量 "null"
            Uri uri;
            if (!Uri.TryCreate(source, UriKind.Absolute, out uri)) { return true; }

            //Uri.Authority 会省掉默认端口（http 的 80 / https 的 443），与浏览器写 Host 头的规矩一致
            return !string.Equals(uri.Authority, Request.Headers["Host"] ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region//HTTP Basic 解析

        /// <summary>
        /// 解析 Authorization: Basic ...。任何格式问题都返回 false（→ 401），<b>不抛</b>。
        ///
        /// 原来的写法四处有问题（2026-09-11 实测）：
        ///   · 按冒号 Split 后取 [1] —— 密码里带冒号就被截断，正确的密码也登不上；RFC 规定只在<b>第一个</b>冒号处分。
        ///   · 只有 "Basic"、base64 不合法、没有冒号 —— 直接抛异常返回 500，而且不用登录就能触发。
        ///   · 按 ISO-8859-1 解码 —— 浏览器是按 UTF-8 编码的，中文用户名 / 密码永远对不上。
        ///     现在先按 UTF-8 严格解，解不了（老客户端发的是 Latin-1）再退回 ISO-8859-1。
        ///   · 方案名区分大小写 —— RFC 规定不区分。
        /// </summary>
        internal static bool TryParseBasic(string Header, out string UserName, out string Password)
        {
            UserName = null;
            Password = null;

            string h = (Header ?? string.Empty).Trim();
            int space = h.IndexOf(' ');
            if (space <= 0 || !h.Substring(0, space).Equals("Basic", StringComparison.OrdinalIgnoreCase)) { return false; }

            byte[] raw;
            try { raw = Convert.FromBase64String(h.Substring(space + 1).Trim()); }
            catch (FormatException) { return false; }

            string text;
            try { text = new UTF8Encoding(false, true).GetString(raw); }
            catch (ArgumentException) { text = Encoding.GetEncoding("iso-8859-1").GetString(raw); }

            int colon = text.IndexOf(':');
            if (colon < 0) { return false; }

            UserName = text.Substring(0, colon);
            Password = text.Substring(colon + 1);
            return true;
        }

        #endregion

        #region//登录失败退避

        /// <summary>
        /// 按来源 IP 记连续登录失败：前 5 次不限，之后每次失败锁 30 秒、60 秒、120 秒……封顶 15 分钟；
        /// 登录成功清零，15 分钟没再失败也清零。锁着的时候直接回 429 + Retry-After，<b>不再比对密码</b>。
        ///
        /// 【为什么要有】/ProxyCap/* 是给 WPC 客户端从公网取节点的，管理台和它在同一个端口上 ——
        /// 也就是说管理台大概率暴露在公网，而认证是明文 HTTP Basic、原来失败次数不限，可以一直猜。
        /// </summary>
        internal static class LoginGuard
        {
            private const int FreeFails = 5;
            private const int BaseLockSeconds = 30;
            private const int MaxLockSeconds = 15 * 60;
            private static readonly TimeSpan Forget = TimeSpan.FromMinutes(15);

            private sealed class Entry
            {
                public int Fails;
                public DateTime LastFail;
                public DateTime LockUntil;
            }

            private static readonly ConcurrentDictionary<string, Entry> entries = new ConcurrentDictionary<string, Entry>();

            /// <summary>这个 IP 还要锁多少秒；0 = 没锁。</summary>
            public static int SecondsLocked(string Ip)
            {
                Entry e;
                if (!entries.TryGetValue(Ip, out e)) { return 0; }

                lock (e)
                {
                    double s = (e.LockUntil - DateTime.UtcNow).TotalSeconds;
                    return s > 0 ? (int)Math.Ceiling(s) : 0;
                }
            }

            public static void Succeeded(string Ip)
            {
                Entry e;
                entries.TryRemove(Ip, out e);
            }

            public static void Failed(string Ip)
            {
                DateTime now = DateTime.UtcNow;
                Entry e = entries.GetOrAdd(Ip, _ => new Entry());
                int lockSeconds = 0, fails;

                lock (e)
                {
                    if (now - e.LastFail > Forget) { e.Fails = 0; }

                    e.Fails++;
                    e.LastFail = now;
                    fails = e.Fails;

                    if (e.Fails > FreeFails)
                    {
                        int shift = Math.Min(e.Fails - FreeFails - 1, 10);
                        lockSeconds = Math.Min(BaseLockSeconds << shift, MaxLockSeconds);
                        e.LockUntil = now.AddSeconds(lockSeconds);
                    }
                }

                if (lockSeconds > 0)
                {
                    Operate.DoLog(nameof(LoginGuard), string.Format(
                        UI.T("MGT.AuthLocked", "远程管理：{0} 连续 {1} 次登录失败，暂停接受它的请求 {2} 秒"), Ip, fails, lockSeconds));
                }

                //别让一个被扫的端口把字典撑大：条目多了就把早就过期的扫掉
                if (entries.Count > 4096)
                {
                    foreach (KeyValuePair<string, Entry> kv in entries.ToArray())
                    {
                        if (now - kv.Value.LastFail > Forget && kv.Value.LockUntil < now) { entries.TryRemove(kv.Key, out _); }
                    }
                }
            }

            /// <summary>跑测用：清掉全部记录。</summary>
            internal static void Reset()
            {
                entries.Clear();
            }
        }

        #endregion

        #region//CCProxy 的 POST

        private static async Task HandleAccountPost(IOwinContext context)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";

            IFormCollection body = await context.Request.ReadFormAsync();

            bool IsAdd = body["add"] != null && Operate.SystemConfig.StringToBool(body["add"]);
            bool IsDel = body["delete"] != null && Operate.SystemConfig.StringToBool(body["delete"]);
            bool IsEdit = body["edit"] != null && Operate.SystemConfig.StringToBool(body["edit"]);

            //只写表单里带了的字段：新增时其余取默认值，修改时其余保持账号原样
            Action<AccountInfo> apply = a =>
            {
                if (body["enable"] != null) { a.IsEnable = Operate.SystemConfig.StringToBool(body["enable"]); }
                if (body["username"] != null) { a.UserName = body["username"]; }
                if (body["password"] != null) { a.Password = body["password"]; }
                if (body["autodisable"] != null) { a.IsExpiry = Operate.SystemConfig.StringToBool(body["autodisable"]); }

                if (body["disabledate"] != null && body["disabletime"] != null)
                {
                    a.ExpiryTime = Operate.SystemConfig.StringToDateTime(body["disabledate"], body["disabletime"]);
                }
            };

            AccountInfo pai = new AccountInfo();
            apply(pai);

            if (IsAdd && CCProxy_Controller.AddUser(pai))
            {
                await context.Response.WriteAsync("1");
            }

            if (IsDel && body["userid"] != null && CCProxy_Controller.DelUser(body["userid"]))
            {
                await context.Response.WriteAsync("1");
            }

            if (IsEdit && CCProxy_Controller.UserUpdate(body["username"], apply))
            {
                await context.Response.WriteAsync("1");
            }
        }

        #endregion
    }
}
