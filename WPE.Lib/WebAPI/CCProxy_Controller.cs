using System;
using System.Reflection;
using System.Text;

namespace WPE.Lib.WebAPI
{
    public static class CCProxy_Controller
    {
        #region//获取全部代理账号

        public static string QueryUserAll()
        {
            string sReturn = string.Empty;

            try
            {
                int bodyIndex = Operate.ProxyConfig.ProxyAccount.CCProxy_HTML.IndexOf("<!-- body -->");
                int tailIndex = Operate.ProxyConfig.ProxyAccount.CCProxy_HTML.IndexOf("<!-- tail -->");

                if (bodyIndex > 0 && tailIndex > bodyIndex)
                {
                    string template = Operate.ProxyConfig.ProxyAccount.CCProxy_HTML.Substring(bodyIndex + "<!-- body -->".Length, tailIndex - bodyIndex - "<!-- body -->".Length);

                    var sb = new StringBuilder();
                    foreach (Proxy_AccountInfo pai in Operate.ProxyConfig.ProxyAccount.lstProxyAccount)
                    {
                        string userTemplate = template
                            .Replace("$username", pai.UserName)
                            .Replace("$password", Socket_Operation.PassWord_Decrypt(pai.PassWord))
                            .Replace("$checkenable", pai.IsEnable ? "checked" : "")
                            .Replace("$checkusepassword", true ? "checked" : "")
                            .Replace("$checkautodisable", pai.IsExpiry ? "checked" : "")
                            .Replace("$disabledate", pai.ExpiryTime.ToString("yyyy-MM-dd"))
                            .Replace("$disabletime", pai.ExpiryTime.ToString("HH:mm:ss"))
                            .Replace("$connection", "-1")
                            .Replace("$bandwidth", "-1/-1");

                        sb.Append(userTemplate);
                    }

                    sReturn = Operate.ProxyConfig.ProxyAccount.CCProxy_HTML.Substring(0, bodyIndex) + sb.ToString() + Operate.ProxyConfig.ProxyAccount.CCProxy_HTML.Substring(tailIndex);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//新增代理账号

        public static bool AddUser(Proxy_AccountInfo pai)
        {
            try
            {
                if (string.IsNullOrEmpty(pai.UserName) || string.IsNullOrEmpty(pai.PassWord))
                {
                    return false;
                }

                if (Operate.ProxyConfig.ProxyAccount.CheckProxyAccount_Exist(pai.UserName))
                {
                    return false;
                }

                pai.LoginTime = DateTime.MinValue;

                if (pai.ExpiryTime == null)
                {
                    pai.ExpiryTime = DateTime.Now;
                }

                pai.PassWord = Socket_Operation.PassWord_Encrypt(pai.PassWord);
                pai.IsLimitDevices = true;
                pai.LimitDevices = 1;

                return Operate.ProxyConfig.ProxyAccount.AddProxyAccount(
                    Guid.NewGuid(), 
                    pai.IsEnable, 
                    pai.UserName, 
                    pai.PassWord, 
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
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }
        }

        #endregion

        #region//修改代理账号

        public static bool UserUpdate(Proxy_AccountInfo pai)
        {
            try
            {
                if (string.IsNullOrEmpty(pai.UserName))
                {
                    return false;
                }

                if (!Operate.ProxyConfig.ProxyAccount.CheckProxyAccount_Exist(pai.UserName))
                {
                    return false;
                }

                if (pai.ExpiryTime == null)
                {
                    pai.ExpiryTime = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(pai.PassWord))
                {
                    pai.PassWord = Socket_Operation.PassWord_Encrypt(pai.PassWord);
                }

                pai.IsLimitDevices = true;

                return Operate.ProxyConfig.ProxyAccount.UpdateProxyAccount_ByCCProxy(
                    pai.UserName, 
                    pai.IsEnable, 
                    pai.PassWord, 
                    pai.IsLimitLinks,
                    pai.LimitLinks,                   
                    pai.IsExpiry, 
                    pai.ExpiryTime);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }            
        }

        #endregion

        #region//删除代理账号

        public static bool DelUser(string UserName)
        {
            return Operate.ProxyConfig.ProxyAccount.DeleteProxyAccount_ByUserName(UserName);
        }

        #endregion
    }
}
