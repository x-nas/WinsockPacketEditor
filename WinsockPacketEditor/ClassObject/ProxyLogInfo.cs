using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class ProxyLogInfo : NotifyProperty
    {
        #region//时间戳

        DateTime _LogTime;

        public DateTime LogTime
        {
            get => _LogTime;
            set
            {
                if (_LogTime == value) return;
                _LogTime = value;
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

        #region//日志内容

        string _LogContent;

        public string LogContent
        {
            get => _LogContent;
            set
            {
                if (_LogContent == value) return;
                _LogContent = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//ProxyLogInfo

        public ProxyLogInfo(string UserName, string LoginIP, string LogContent)
        {
            this._LogTime = DateTime.Now;
            this._UserName = UserName;
            this._LoginIP = LoginIP;
            this._LogContent = LogContent;
        }

        #endregion
    }
}
