using AntdUI;
using System;

namespace WPE.Lib
{
    public class AccountInfo : NotifyProperty
    {
        #region//是否选中

        bool _IsCheck = false;

        public bool IsCheck
        {
            get => _IsCheck;
            set
            {
                if (_IsCheck == value) return;
                _IsCheck = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//序号

        Guid _AID;

        public Guid AID
        {
            get => _AID;
            set
            {
                if (_AID == value) return;
                _AID = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//是否启用

        bool _IsEnable;

        public bool IsEnable
        {
            get => _IsEnable;
            set
            {
                if (_IsEnable == value) return;
                _IsEnable = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//账号

        string _UserName;

        public string UserName
        {
            get => _UserName;
            set
            {
                if (_UserName == value) return;
                _UserName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//密码

        string _Password;

        public string Password
        {
            get => _Password;
            set
            {
                if (_Password == value) return;
                _Password = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//登录时间

        DateTime _LoginTime;

        public DateTime LoginTime
        {
            get => _LoginTime;
            set
            {
                if (_LoginTime == value) return;
                _LoginTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//登录IP

        string _LoginIP;

        public string LoginIP
        {
            get => _LoginIP;
            set
            {
                if (_LoginIP == value) return;
                _LoginIP = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//IP所属地

        string _IPLocation;

        public string IPLocation
        {
            get => _IPLocation;
            set
            {
                if (_IPLocation == value) return;
                _IPLocation = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//是否限制链接数

        bool _IsLimitLinks;

        public bool IsLimitLinks
        {
            get => _IsLimitLinks;
            set
            {
                if (_IsLimitLinks == value) return;
                _IsLimitLinks = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//最大链接数

        int _LimitLinks;

        public int LimitLinks
        {
            get => _LimitLinks;
            set
            {
                if (_LimitLinks == value) return;
                _LimitLinks = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//是否限制设备数

        bool _IsLimitDevices;

        public bool IsLimitDevices
        {
            get => _IsLimitDevices;
            set
            {
                if (_IsLimitDevices == value) return;
                _IsLimitDevices = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//最大设备数

        int _LimitDevices;

        public int LimitDevices
        {
            get => _LimitDevices;
            set
            {
                if (_LimitDevices == value) return;
                _LimitDevices = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//是否过期

        bool _IsExpiry;

        public bool IsExpiry
        {
            get => _IsExpiry;
            set
            {
                if (_IsExpiry == value) return;
                _IsExpiry = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//过期时间

        DateTime _ExpiryTime;

        public DateTime ExpiryTime
        {
            get => _ExpiryTime;
            set
            {
                if (_ExpiryTime == value) return;
                _ExpiryTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//开通时间

        DateTime _CreateTime;

        public DateTime CreateTime
        {
            get => _CreateTime;
            set
            {
                if (_CreateTime == value) return;
                _CreateTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//是否在线

        bool _IsOnLine;

        public bool IsOnLine
        {
            get => _IsOnLine;
            set
            {
                if (_IsOnLine == value) return;
                _IsOnLine = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//AccountInfo

        public AccountInfo()
        {
            //
        }

        public AccountInfo(
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
            this._AID = AID;
            this._IsEnable = IsEnable;
            this._UserName = UserName;
            this._Password = PassWord;
            this._LoginTime = LoginTime;
            this._LoginIP = LoginIP;
            this._IPLocation = IPLocation;
            this._IsLimitLinks = IsLimitLinks;
            this._LimitLinks = LimitLinks;
            this._IsLimitDevices = IsLimitDevices;
            this._LimitDevices = LimitDevices;
            this._IsExpiry = IsExpiry;
            this._ExpiryTime = ExpiryTime;
            this._CreateTime = CreateTime;
            this._IsOnLine = false;
        }

        #endregion
    }
}
