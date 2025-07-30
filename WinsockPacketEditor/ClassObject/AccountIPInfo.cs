using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class AccountIPInfo : NotifyProperty
    {
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

        #region//AccountIPInfo

        public AccountIPInfo(DateTime LoginTime, string LoginIP, string IPLocation)
        {
            this._LoginTime = LoginTime;
            this._LoginIP = LoginIP;
            this._IPLocation = IPLocation;
        }

        #endregion
    }
}
