using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WinsockPacketEditor
{
    /// <summary>
    /// CCProxy 兼容接口（/account，给第三方 CCProxy 客户端解析的，不是给人看的页面）。
    /// 由 Socket_Web 的 /account 中间件调用。
    ///
    /// 【2026-09-11】读写都切到界面线程（见 WebUi.cs）；编辑之后推一次那一行给程序界面；
    /// 模板里的用户名 / 密码做 HTML 转义，占位符一次替换（原来的连环 Replace 在用户名里含 $password 时会串）。
    /// </summary>
    public static class CCProxy_Controller
    {
        #region//获取全部代理账号

        private static readonly Regex Placeholder = new Regex(
            @"\$(username|password|checkenable|checkusepassword|checkautodisable|disabledate|disabletime|connection|bandwidth)\b",
            RegexOptions.Compiled);

        public static string QueryUserAll()
        {
            string sReturn = string.Empty;

            try
            {
                string html = Operate.ProxyConfig.Account.CCProxy_HTML ?? string.Empty;
                int bodyIndex = html.IndexOf("<!-- body -->");
                int tailIndex = html.IndexOf("<!-- tail -->");

                if (bodyIndex > 0 && tailIndex > bodyIndex)
                {
                    string template = html.Substring(bodyIndex + "<!-- body -->".Length, tailIndex - bodyIndex - "<!-- body -->".Length);

                    //在界面线程上把要用的字段取出来（账号列表是界面线程维护的），拼 HTML 回到 HTTP 线程上做
                    List<Dictionary<string, string>> users = WebUi.OnUi(() => Operate.ProxyConfig.Account.lstAccountInfo
                        .Where(a => a != null)
                        .Select(a => new Dictionary<string, string>
                        {
                            //值都在 value="..." 属性里 —— 用户名、密码要转义，否则一个引号就能把页面结构打乱（在浏览器里打开就是存储型 XSS）
                            { "username", WebUtility.HtmlEncode(a.UserName ?? string.Empty) },
                            { "password", WebUtility.HtmlEncode(Operate.SystemConfig.PassWord_Decrypt(a.Password)) },
                            { "checkenable", a.IsEnable ? "checked" : "" },
                            { "checkusepassword", "checked" },
                            { "checkautodisable", a.IsExpiry ? "checked" : "" },
                            { "disabledate", a.ExpiryTime.ToString("yyyy-MM-dd") },
                            { "disabletime", a.ExpiryTime.ToString("HH:mm:ss") },
                            { "connection", "-1" },
                            { "bandwidth", "-1/-1" },
                        })
                        .ToList());

                    StringBuilder sb = new StringBuilder();
                    foreach (Dictionary<string, string> u in users)
                    {
                        sb.Append(Placeholder.Replace(template, m => u[m.Groups[1].Value]));
                    }

                    sReturn = html.Substring(0, bodyIndex) + sb.ToString() + html.Substring(tailIndex);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(QueryUserAll), ex);
            }

            return sReturn;
        }

        #endregion

        #region//新增代理账号

        public static bool AddUser(AccountInfo pai)
        {
            try
            {
                if (pai == null || string.IsNullOrEmpty(pai.UserName) || string.IsNullOrEmpty(pai.Password))
                {
                    return false;
                }

                string password = Operate.SystemConfig.PassWord_Encrypt(pai.Password);

                return WebUi.OnUi(() =>
                {
                    if (Operate.ProxyConfig.Account.CheckProxyAccount_Exist(pai.UserName))
                    {
                        return false;
                    }

                    //CCProxy 那边没有「设备数」这个概念，新建的账号默认限 1 台设备（沿用原来的约定）
                    return Operate.ProxyConfig.Account.AddProxyAccount(
                        true,
                        Guid.NewGuid(),
                        pai.IsEnable,
                        pai.UserName,
                        password,
                        new BindingList<AccountIPInfo>(),
                        pai.IsLimitLinks,
                        pai.LimitLinks,
                        true,
                        1,
                        pai.IsExpiry,
                        pai.ExpiryTime,
                        DateTime.Now);
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(AddUser), ex);
                return false;
            }
        }

        #endregion

        #region//修改代理账号

        /// <summary>
        /// 改账号。<paramref name="Apply"/> 只写表单里<b>真的带了</b>的字段 ——
        /// 从现有账号出发再覆盖，没带的字段（连接数限制、到期时间、启用）保持原样，而不是被默认值洗掉。
        /// </summary>
        public static bool UserUpdate(string UserName, Action<AccountInfo> Apply)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName) || Apply == null)
                {
                    return false;
                }

                return WebUi.OnUi(() =>
                {
                    AccountInfo old = Operate.ProxyConfig.Account.lstAccountInfo.FirstOrDefault(a => a != null && a.UserName == UserName);
                    if (old == null)
                    {
                        return false;
                    }

                    AccountInfo pai = new AccountInfo
                    {
                        UserName = UserName,
                        IsEnable = old.IsEnable,
                        Password = string.Empty,
                        IsLimitLinks = old.IsLimitLinks,
                        LimitLinks = old.LimitLinks,
                        IsExpiry = old.IsExpiry,
                        ExpiryTime = old.ExpiryTime,
                    };
                    Apply(pai);

                    //空密码 = 不改密码
                    string password = string.IsNullOrEmpty(pai.Password) ? pai.Password : Operate.SystemConfig.PassWord_Encrypt(pai.Password);

                    bool ok = Operate.ProxyConfig.Account.UpdateProxyAccount_ByCCProxy(
                        pai.UserName,
                        pai.IsEnable,
                        password,
                        pai.IsLimitLinks,
                        pai.LimitLinks,
                        pai.IsExpiry,
                        pai.ExpiryTime);

                    //就地改属性不触发 ListChanged —— 不推的话程序里的账号表停在旧值
                    if (ok)
                    {
                        Operate.ProxyConfig.Account.PushAccountRow(
                            Operate.ProxyConfig.Account.lstAccountInfo.FirstOrDefault(a => a != null && a.UserName == pai.UserName));
                    }

                    return ok;
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(UserUpdate), ex);
                return false;
            }
        }

        #endregion

        #region//删除代理账号

        public static bool DelUser(string UserName)
        {
            try
            {
                return WebUi.OnUi(() => Operate.ProxyConfig.Account.DeleteProxyAccount_ByUserName(UserName));
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(DelUser), ex);
                return false;
            }
        }

        #endregion
    }
}
