using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Http;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 远程管理台的「代理账号」接口。
    ///
    /// 【2026-09-11 改的三件事】
    ///   · 读写一律切到界面线程（<see cref="WebUi.OnUi{T}"/>）：账号列表由界面线程维护、
    ///     FeedPump 在界面线程上遍历它推给前端，HTTP 线程直接改会把那边的遍历撞坏，直接读会撞上「集合已修改」。
    ///   · 更新之后 <see cref="Operate.ProxyConfig.Account.PushAccountRow"/>：就地改属性不触发 ListChanged，
    ///     原来在网页上改完，程序里的账号表停在旧值。增删会触发 ListChanged，FeedPump 自己会推。
    ///   · 出参是 <see cref="WebAccount"/> 拷贝，不再是活的 AccountInfo（见那个类的注释）。
    /// </summary>
    [RoutePrefix("ProxyAccount")]

    public class ProxyAccount_Controller : ApiController
    {
        /// <summary>
        /// 账号的一份拷贝，字段名与 AccountInfo 相同（管理台照旧读 a.UserName / a.IsEnable …）。
        ///
        /// ⚠️ 与 AccountInfo 的区别只有两处：
        ///   · <b>列表接口不带登录记录（AIPInfo）</b>。管理台从来不读它，而它是 SOCKS5 会话线程在锁里改的嵌套列表 ——
        ///     账号多、登录频繁时它是整份响应里最大的一块，也是序列化时最容易撞上并发修改的一块。
        ///     单个账号的接口（GetProxyAccountByID）仍然带，在锁里复制。
        ///   · 没有 IsCheck（那是 WinForms 表格的勾选状态，早就没人用）。
        /// </summary>
        public sealed class WebAccount
        {
            public Guid AID;
            public bool IsEnable;
            public string UserName;
            public string Password;          //仍是存库的那种编码串（兼容老调用方）；管理台要明文走 GetPlainPassword
            public bool IsLimitLinks;
            public int LimitLinks;
            public bool IsLimitDevices;
            public int LimitDevices;
            public bool IsExpiry;
            public DateTime ExpiryTime;
            public DateTime CreateTime;
            public bool IsOnLine;
            public AccountIPInfo[] AIPInfo;  //只有 GetProxyAccountByID 填

            public static WebAccount From(AccountInfo a, bool WithLogins)
            {
                return new WebAccount
                {
                    AID = a.AID,
                    IsEnable = a.IsEnable,
                    UserName = a.UserName,
                    Password = a.Password,
                    IsLimitLinks = a.IsLimitLinks,
                    LimitLinks = a.LimitLinks,
                    IsLimitDevices = a.IsLimitDevices,
                    LimitDevices = a.LimitDevices,
                    IsExpiry = a.IsExpiry,
                    ExpiryTime = a.ExpiryTime,
                    CreateTime = a.CreateTime,
                    IsOnLine = a.IsOnLine,
                    AIPInfo = WithLogins ? Operate.ProxyConfig.Account.CopyLogins(a) : null,
                };
            }
        }

        #region//获取代理账号列表

        [HttpGet]
        [Route("GetProxyAccountList")]

        public IEnumerable<WebAccount> GetProxyAccountList()
        {
            return WebUi.OnUi(() => Operate.ProxyConfig.Account.lstAccountInfo
                .Where(a => a != null)
                .Select(a => WebAccount.From(a, false))
                .ToList());
        }

        #endregion

        #region//获取代理账号

        [HttpGet]
        [Route("GetProxyAccountByID")]

        public WebAccount GetProxyAccountByID(Guid AID)
        {
            return WebUi.OnUi(() =>
            {
                AccountInfo a = Operate.ProxyConfig.Account.GetProxyAccount_ByAccountID(AID);
                return a == null ? null : WebAccount.From(a, true);
            });
        }

        #endregion

        #region//取明文密码

        /// <summary>
        /// 编辑账号时回填密码用：按账号 Id 取，<b>URL 里只有 Id</b>。
        ///
        /// 原来管理台调的是 GetPassWordDecrypt?PassWord=&lt;编码串&gt; —— 而那个「加密」只是逐字符替换，
        /// 编码串约等于明文，放进查询串就留在了浏览器历史里。
        /// </summary>
        [HttpGet]
        [Route("GetPlainPassword")]

        public IHttpActionResult GetPlainPassword(Guid AID)
        {
            string pw = WebUi.OnUi(() =>
            {
                AccountInfo a = Operate.ProxyConfig.Account.GetProxyAccount_ByAccountID(AID);
                return a == null ? null : Operate.SystemConfig.PassWord_Decrypt(a.Password);
            });

            if (pw == null) { return NotFound(); }
            return Ok(pw);
        }

        /// <summary>
        /// ⚠️ <b>保留只为兼容</b>：管理台已改用 <see cref="GetPlainPassword"/>。
        /// 这个接口把编码串放在查询串里，而且等于一个「任意串解码器」，新代码别再用。
        /// </summary>
        [HttpGet]
        [Route("GetPassWordDecrypt")]

        public string GetPassWordDecrypt(string PassWord)
        {
            return Operate.SystemConfig.PassWord_Decrypt(PassWord);
        }

        #endregion

        #region//新增代理账号

        [HttpPost]
        [Route("AddProxyAccount")]

        public IHttpActionResult AddProxyAccount([FromBody] AccountInfo pai)
        {
            if (pai == null || string.IsNullOrEmpty(pai.UserName)) { return BadRequest(UI.T("AddAccount.Error", "添加账号失败")); }

            try
            {
                string error = WebUi.OnUi(() =>
                {
                    if (Operate.ProxyConfig.Account.CheckProxyAccount_Exist(pai.UserName))
                    {
                        return UI.T("UserName.Exists", "该用户名已存在");
                    }

                    bool ok = Operate.ProxyConfig.Account.AddProxyAccount(
                        true,
                        Guid.NewGuid(),
                        pai.IsEnable,
                        pai.UserName,
                        Operate.SystemConfig.PassWord_Encrypt(pai.Password),
                        new BindingList<AccountIPInfo>(),
                        pai.IsLimitLinks,
                        pai.LimitLinks,
                        pai.IsLimitDevices,
                        pai.LimitDevices,
                        pai.IsExpiry,
                        pai.ExpiryTime,
                        DateTime.Now);

                    return ok ? null : UI.T("AddAccount.Error", "添加账号失败");
                });

                if (error == null) { return Ok(UI.T("AddAccount.Success", "添加账号成功")); }
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(AddProxyAccount), ex);
                return BadRequest(UI.T("AddAccount.Error", "添加账号失败"));
            }
        }

        #endregion

        #region//删除代理账号

        [HttpPost]
        [Route("DeleteProxyAccount")]

        public IHttpActionResult DeleteProxyAccount([FromBody] Guid AID)
        {
            bool bOK = WebUi.OnUi(() => Operate.ProxyConfig.Account.DeleteProxyAccount_ByAccountID(AID));

            if (bOK)
            {
                return Ok(UI.T("DeleteAccount.Success", "删除账号成功"));
            }
            else
            {
                return BadRequest(UI.T("DeleteAccount.Error", "删除账号失败"));
            }
        }

        #endregion

        #region//更新代理账号

        [HttpPost]
        [Route("UpdateProxyAccount")]

        public IHttpActionResult UpdateProxyAccount([FromBody] AccountInfo pai)
        {
            if (pai == null) { return BadRequest(UI.T("UpdateAccount.Error", "更新账号失败")); }

            //空密码 = 不改密码（UpdateProxyAccount_ByAccountID 的约定）；PassWord_Encrypt 对空串返回空串
            string password = Operate.SystemConfig.PassWord_Encrypt(pai.Password);

            bool bOK = WebUi.OnUi(() =>
            {
                bool ok = Operate.ProxyConfig.Account.UpdateProxyAccount_ByAccountID(
                    pai.AID,
                    pai.IsEnable,
                    password,
                    pai.IsLimitLinks,
                    pai.LimitLinks,
                    pai.IsLimitDevices,
                    pai.LimitDevices,
                    pai.IsExpiry,
                    pai.ExpiryTime);

                //就地改属性不触发 ListChanged —— 不推的话程序里的账号表停在旧值
                if (ok) { Operate.ProxyConfig.Account.PushAccountRow(Operate.ProxyConfig.Account.GetProxyAccount_ByAccountID(pai.AID)); }

                return ok;
            });

            if (bOK)
            {
                return Ok(UI.T("UpdateAccount.Success", "更新账号成功"));
            }
            else
            {
                return BadRequest(UI.T("UpdateAccount.Error", "更新账号失败"));
            }
        }

        #endregion
    }
}
