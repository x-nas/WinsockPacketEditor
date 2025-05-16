using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;

namespace WPELibrary.Lib.WebAPI
{
    [RoutePrefix("api/ProxyAccount")]

    public class ProxyAccount_Controller : ApiController
    {
        #region//获取代理账号列表

        [HttpGet]
        [Route("GetProxyAccountList")]

        public IEnumerable<Proxy_AccountInfo> GetProxyAccountList()
        {
            return Socket_Cache.ProxyAccount.lstProxyAccount;
        }

        #endregion

        #region//获取代理账号

        [HttpGet]
        [Route("GetProxyAccountByID")]

        public Proxy_AccountInfo GetProxyAccountByID(Guid AID)
        {
            return Socket_Cache.ProxyAccount.GetProxyAccount_ByAccountID(AID);
        }

        #endregion

        #region//获取解密后的密码

        [HttpGet]
        [Route("GetPassWordDecrypt")]

        public string GetPassWordDecrypt(string PassWord)
        {
            return Socket_Operation.PassWord_Decrypt(PassWord);
        }

        #endregion

        #region//新增代理账号

        [HttpPost]
        [Route("AddProxyAccount")]

        public IHttpActionResult AddProxyAccount([FromBody] Proxy_AccountInfo pai)
        {
            try
            {
                if (Socket_Cache.ProxyAccount.CheckProxyAccount_Exist(pai.UserName))
                {
                    return BadRequest(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_177));
                }

                if (pai.ExpiryTime == null)
                {
                    pai.ExpiryTime = DateTime.Now;
                }

                pai.PassWord = Socket_Operation.PassWord_Encrypt(pai.PassWord);
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
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return BadRequest(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_181));
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

        #region//更新代理账号

        [HttpPost]
        [Route("UpdateProxyAccount")]

        public IHttpActionResult UpdateProxyAccount([FromBody] Proxy_AccountInfo pai)
        {
            if (pai.ExpiryTime == null)
            {
                pai.ExpiryTime = DateTime.Now;
            }

            pai.PassWord = Socket_Operation.PassWord_Encrypt(pai.PassWord);
            bool bOK = Socket_Cache.ProxyAccount.UpdateProxyAccount_ByAccountID(pai.AID, pai.IsEnable, pai.PassWord, pai.IsExpiry, pai.ExpiryTime);

            if (bOK)
            {
                return Ok(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_194));
            }
            else
            {
                return BadRequest(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_195));
            }            
        }

        #endregion
    }
}
