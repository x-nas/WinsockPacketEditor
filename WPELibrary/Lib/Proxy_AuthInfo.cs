using System;

namespace WPELibrary.Lib
{
    public class Proxy_AuthInfo
    {
        //IP地址
        public string IPAddress { get; set; }

        //用户名
        public string UserName { get; set; }

        //是否验证成功
        public bool AuthResult { get; set; }

        //验证时间
        public DateTime AuthTime { get; set; }

        public Proxy_AuthInfo(string ipAddress, string userName, bool authResult, DateTime authTime)
        {
            this.IPAddress = ipAddress;
            this.UserName = userName;
            this.AuthResult = authResult;
            this.AuthTime = authTime;
        }
    }
}
