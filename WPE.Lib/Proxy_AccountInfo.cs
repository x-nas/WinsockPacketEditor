﻿using System;

namespace WPE.Lib
{
    public class Proxy_AccountInfo
    {
        #region//序号

        protected Guid aid;

        public Guid AID
        {
            get { return aid; }
            set { aid = value; }
        }

        #endregion        

        #region//是否启用

        protected bool isenable;

        public bool IsEnable
        {
            get { return isenable; }
            set { isenable = value; }
        }

        #endregion

        #region//账号

        protected string username;

        public string UserName
        {
            get { return username; }
            set { username = value; }
        }

        #endregion

        #region//密码

        protected string password;

        public string PassWord
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region//登录时间

        protected DateTime logintime;

        public DateTime LoginTime
        {
            get { return logintime; }
            set { logintime = value; }
        }

        #endregion

        #region//登录IP

        protected string loginip;

        public string LoginIP
        {
            get { return loginip; }
            set { loginip = value; }
        }

        #endregion

        #region//IP所属地

        protected string iplocation;

        public string IPLocation
        {
            get { return iplocation; }
            set { iplocation = value; }
        }

        #endregion

        #region//是否限制链接数

        protected bool islimitlinks;

        public bool IsLimitLinks
        {
            get { return islimitlinks; }
            set { islimitlinks = value; }
        }

        #endregion

        #region//最大链接数  

        protected int limitlinks;

        public int LimitLinks
        {
            get { return limitlinks; }
            set { limitlinks = value; }
        }

        #endregion

        #region//是否限制设备数

        protected bool islimitdevices;

        public bool IsLimitDevices
        {
            get { return islimitdevices; }
            set { islimitdevices = value; }
        }

        #endregion

        #region//最大设备数

        protected int limitdevices;

        public int LimitDevices
        {
            get { return limitdevices; }
            set { limitdevices = value; }
        }

        #endregion

        #region//是否过期

        protected bool isexpiry;

        public bool IsExpiry
        {
            get { return isexpiry; }
            set { isexpiry = value; }
        }

        #endregion

        #region//过期时间

        protected DateTime expirytime;

        public DateTime ExpiryTime
        {
            get { return expirytime; }
            set { expirytime = value; }
        }

        #endregion

        #region//开通时间

        protected DateTime createtime;

        public DateTime CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }

        #endregion

        #region//是否在线

        protected bool isonline;

        public bool IsOnLine
        {
            get { return isonline; }
            set { isonline = value; }
        }

        #endregion

        #region//Proxy_AccountInfo

        public Proxy_AccountInfo()
        {
            //
        }

        public Proxy_AccountInfo(
            Guid AID, 
            bool IsEnable, 
            string UserName, 
            string PassWord, 
            DateTime LoginTime, 
            string LoginIP, 
            string IPLocation, 
            bool IsLimitLinks, 
            int LimitLinks,
            bool IsLimitDevices,
            int LimitDevices,
            bool IsExpiry, 
            DateTime ExpiryTime, 
            DateTime CreateTime) 
        {
            this.aid = AID;
            this.isenable = IsEnable;
            this.username = UserName;
            this.password = PassWord;
            this.logintime = LoginTime;
            this.loginip = LoginIP;
            this.iplocation = IPLocation;
            this.islimitlinks = IsLimitLinks;
            this.limitlinks = LimitLinks;
            this.islimitdevices = IsLimitDevices;
            this.limitdevices = LimitDevices;
            this.isexpiry = IsExpiry;
            this.expirytime = ExpiryTime;
            this.createtime = CreateTime;
            this.isonline = false;
        }

        #endregion
    }
}
