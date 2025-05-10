using System;

namespace WPELibrary.Lib
{
    public class Proxy_AuthInfo
    {
        public string IPAddress { get; set; }

        public bool AuthResult { get; set; }

        public DateTime AuthTime { get; set; }

        public Proxy_AuthInfo(string ipAddress, bool authResult, DateTime authTime)
        {
            IPAddress = ipAddress;
            AuthResult = authResult;
            AuthTime = authTime;
        }
    }
}
