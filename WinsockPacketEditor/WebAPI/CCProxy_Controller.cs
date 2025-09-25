using System;
using System.Text;
using System.ComponentModel;

namespace WinsockPacketEditor
{
    public static class CCProxy_Controller
    {
        #region//获取全部代理账号

        public static string QueryUserAll()
        {
            string sReturn = string.Empty;

            try
            {
                int bodyIndex = Operate.ProxyConfig.Account.CCProxy_HTML.IndexOf("<!-- body -->");
                int tailIndex = Operate.ProxyConfig.Account.CCProxy_HTML.IndexOf("<!-- tail -->");

                if (bodyIndex > 0 && tailIndex > bodyIndex)
                {
                    string template = Operate.ProxyConfig.Account.CCProxy_HTML.Substring(bodyIndex + "<!-- body -->".Length, tailIndex - bodyIndex - "<!-- body -->".Length);

                    var sb = new StringBuilder();
                    foreach (AccountInfo pai in Operate.ProxyConfig.Account.lstAccountInfo)
                    {
                        string userTemplate = template
                            .Replace("$username", pai.UserName)
                            .Replace("$password", Operate.SystemConfig.PassWord_Decrypt(pai.Password))
                            .Replace("$checkenable", pai.IsEnable ? "checked" : "")
                            .Replace("$checkusepassword", true ? "checked" : "")
                            .Replace("$checkautodisable", pai.IsExpiry ? "checked" : "")
                            .Replace("$disabledate", pai.ExpiryTime.ToString("yyyy-MM-dd"))
                            .Replace("$disabletime", pai.ExpiryTime.ToString("HH:mm:ss"))
                            .Replace("$connection", "-1")
                            .Replace("$bandwidth", "-1/-1");

                        sb.Append(userTemplate);
                    }

                    sReturn = Operate.ProxyConfig.Account.CCProxy_HTML.Substring(0, bodyIndex) + sb.ToString() + Operate.ProxyConfig.Account.CCProxy_HTML.Substring(tailIndex);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(QueryUserAll), ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//新增代理账号

        public static bool AddUser(AccountInfo pai)
        {
            try
            {
                if (string.IsNullOrEmpty(pai.UserName) || string.IsNullOrEmpty(pai.Password))
                {
                    return false;
                }

                if (Operate.ProxyConfig.Account.CheckProxyAccount_Exist(pai.UserName))
                {
                    return false;
                }

                if (pai.ExpiryTime == null)
                {
                    pai.ExpiryTime = DateTime.Now;
                }

                pai.Password = Operate.SystemConfig.PassWord_Encrypt(pai.Password);
                pai.IsLimitDevices = true;
                pai.LimitDevices = 1;

                return Operate.ProxyConfig.Account.AddProxyAccount(
                    Guid.NewGuid(), 
                    pai.IsEnable, 
                    pai.UserName, 
                    pai.Password, 
                    new BindingList<AccountIPInfo>(),
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
                Operate.DoLog(nameof(AddUser), ex.Message);
                return false;
            }
        }

        #endregion

        #region//修改代理账号

        public static bool UserUpdate(AccountInfo pai)
        {
            try
            {
                if (string.IsNullOrEmpty(pai.UserName))
                {
                    return false;
                }

                if (!Operate.ProxyConfig.Account.CheckProxyAccount_Exist(pai.UserName))
                {
                    return false;
                }

                if (pai.ExpiryTime == null)
                {
                    pai.ExpiryTime = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(pai.Password))
                {
                    pai.Password = Operate.SystemConfig.PassWord_Encrypt(pai.Password);
                }

                pai.IsLimitDevices = true;

                return Operate.ProxyConfig.Account.UpdateProxyAccount_ByCCProxy(
                    pai.UserName, 
                    pai.IsEnable, 
                    pai.Password, 
                    pai.IsLimitLinks,
                    pai.LimitLinks,                   
                    pai.IsExpiry, 
                    pai.ExpiryTime);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(UserUpdate), ex.Message);
                return false;
            }            
        }

        #endregion

        #region//删除代理账号

        public static bool DelUser(string UserName)
        {
            return Operate.ProxyConfig.Account.DeleteProxyAccount_ByUserName(UserName);
        }

        #endregion
    }
}
