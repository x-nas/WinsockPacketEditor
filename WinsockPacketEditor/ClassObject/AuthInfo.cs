using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class AuthInfo : NotifyProperty
    {
        #region//代理账号序号

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

        #region//认证IP

        string _AuthIP;

        public string AuthIP
        {
            get => _AuthIP;
            set
            {
                if (_AuthIP == value) return;
                _AuthIP = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//链接数

        int _LinksNumber;

        public int LinksNumber
        {
            get => _LinksNumber;
            set
            {
                if (_LinksNumber == value) return;
                _LinksNumber = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//设备数

        int _DevicesNumber;

        public int DevicesNumber
        {
            get => _DevicesNumber;
            set
            {
                if (_DevicesNumber == value) return;
                _DevicesNumber = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//是否认证成功

        bool _AuthResult;

        public bool AuthResult
        {
            get => _AuthResult;
            set
            {
                if (_AuthResult == value) return;
                _AuthResult = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//认证时间

        DateTime _AuthTime;

        public DateTime AuthTime
        {
            get => _AuthTime;
            set
            {
                if (_AuthTime == value) return;
                _AuthTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//AuthInfo

        public AuthInfo(Guid AID, string AuthIP, bool AuthResult, DateTime AuthTime)
        {
            this._AID = AID;
            this._AuthIP = AuthIP;
            this._LinksNumber = 0;
            this._DevicesNumber = 0;
            this._AuthResult = AuthResult;
            this._AuthTime = AuthTime;
        }

        #endregion
    }
}
