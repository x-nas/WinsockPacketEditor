using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;

namespace WPELibrary.Lib
{
    [RoutePrefix("api")]

    public class Socket_WebController : ApiController
    {
        #region//获取代理账号列表

        [HttpGet]
        [Route("GetProxyAccountList")]

        public IEnumerable<Proxy_AccountInfo> GetProxyAccountList()
        {
            return Socket_Cache.ProxyAccount.lstProxyAccount;
        }

        #endregion

        #region//新增代理账号

        [HttpPost]
        [Route("AddProxyAccount")]

        public IHttpActionResult AddProxyAccount([FromBody] Proxy_AccountInfo pai)
        {  
            if (Socket_Cache.ProxyAccount.CheckProxyAccount_Exist(pai.UserName))
            {
                return BadRequest(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_177));
            }

            if (pai.ExpiryTime == null)
            {
                pai.ExpiryTime = DateTime.Now;
            }

            bool bOK = Socket_Cache.ProxyAccount.AddProxyAccount(Guid.NewGuid(), pai.IsEnable, pai.UserName, pai.PassWord, string.Empty, pai.IsExpiry, pai.ExpiryTime, DateTime.Now);

            if (bOK)
            {
                return Ok(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_183));
            }
            else
            {
                return BadRequest(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_181));
            }
        }

        #endregion

        #region//删除代理账号

        [HttpPost]
        [Route("DeleteProxyAccount")]

        public IHttpActionResult DeleteProxyAccount([FromBody] Guid AID)
        {
            bool bOK = Socket_Cache.ProxyAccount.DeleteProxyAccount_ByAccountID(AID);

            if (bOK)
            {
                return Ok(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_184));
            }
            else
            {
                return BadRequest(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_182));
            }
        }

        #endregion
    }
}
