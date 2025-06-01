using System;

namespace WPELibrary.Lib
{
    public class Proxy_AuthInfo
    {
        #region//IP地址

        public string IPAddress { get; set; }

        #endregion

        #region//链接数

        public int LinksNumber { get; set; }

        #endregion

        #region//用户名

        public string UserName { get; set; }

        #endregion

        #region//是否验证成功

        public bool AuthResult { get; set; }

        #endregion

        #region//验证时间

        public DateTime AuthTime { get; set; }

        #endregion

        #region//Proxy_AuthInfo

        public Proxy_AuthInfo(string ipAddress, string userName, bool authResult, DateTime authTime)
        {
            this.IPAddress = ipAddress;
            this.LinksNumber = 0;
            this.UserName = userName;
            this.AuthResult = authResult;
            this.AuthTime = authTime;
        }

        #endregion
    }
}
