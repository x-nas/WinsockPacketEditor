using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;

namespace WinsockPacketEditor
{
    [RoutePrefix("ProxyAccount")]

    public class ProxyAccount_Controller : ApiController
    {
        #region//获取代理账号列表

        [HttpGet]
        [Route("GetProxyAccountList")]

        public IEnumerable<AccountInfo> GetProxyAccountList()
        {
            return Operate.ProxyConfig.Account.lstAccountInfo;
        }

        #endregion

        #region//获取代理账号

        [HttpGet]
        [Route("GetProxyAccountByID")]

        public AccountInfo GetProxyAccountByID(Guid AID)
        {
            return Operate.ProxyConfig.Account.GetProxyAccount_ByAccountID(AID);
        }

        #endregion

        #region//获取解密后的密码

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
            try
            {
                if (Operate.ProxyConfig.Account.CheckProxyAccount_Exist(pai.UserName))
                {
                    return BadRequest(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_177));
                }

                pai.LoginTime = DateTime.MinValue;

                if (pai.ExpiryTime == null)
                {
                    pai.ExpiryTime = DateTime.Now;
                }

                pai.Password = Operate.SystemConfig.PassWord_Encrypt(pai.Password);
                bool bOK = Operate.ProxyConfig.Account.AddProxyAccount(
                    Guid.NewGuid(), 
                    pai.IsEnable, 
                    pai.UserName, 
                    pai.Password, 
                    pai.LoginTime, 
                    string.Empty, 
                    string.Empty, 
                    pai.IsLimitLinks,
                    pai.LimitLinks,
                    pai.IsLimitDevices,
                    pai.LimitDevices,
                    pai.IsExpiry, 
                    pai.ExpiryTime, 
                    DateTime.Now);

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return BadRequest(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_181));
        }

        #endregion

        #region//删除代理账号

        [HttpPost]
        [Route("DeleteProxyAccount")]

        public IHttpActionResult DeleteProxyAccount([FromBody] Guid AID)
        {
            bool bOK = Operate.ProxyConfig.Account.DeleteProxyAccount_ByAccountID(AID);

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

        public IHttpActionResult UpdateProxyAccount([FromBody] AccountInfo pai)
        {
            if (pai.ExpiryTime == null)
            {
                pai.ExpiryTime = DateTime.Now;
            }

            pai.Password = Operate.SystemConfig.PassWord_Encrypt(pai.Password);

            bool bOK = Operate.ProxyConfig.Account.UpdateProxyAccount_ByAccountID(
                pai.AID, 
                pai.IsEnable, 
                pai.Password, 
                pai.IsLimitLinks,
                pai.LimitLinks,
                pai.IsLimitDevices,
                pai.LimitDevices,
                pai.IsExpiry, 
                pai.ExpiryTime);

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
