using System;

namespace WPELibrary.Lib
{
    public class Proxy_AuthInfo
    {
        #region//代理账号序号

        public Guid AID { get; set; }

        #endregion

        #region//IP地址

        public string IPAddress { get; set; }

        #endregion

        #region//链接数

        public int LinksNumber { get; set; }

        #endregion        

        #region//设备数

        public int DevicesNumber { get; set; }

        #endregion        

        #region//是否验证成功

        public bool AuthResult { get; set; }

        #endregion

        #region//验证时间

        public DateTime AuthTime { get; set; }

        #endregion

        #region//Proxy_AuthInfo

        public Proxy_AuthInfo(Guid AID, string ipAddress, bool authResult, DateTime authTime)
        {
            this.AID = AID;
            this.IPAddress = ipAddress;
            this.LinksNumber = 0;
            this.DevicesNumber = 0;
            this.AuthResult = authResult;
            this.AuthTime = authTime;
        }

        #endregion
    }
}
