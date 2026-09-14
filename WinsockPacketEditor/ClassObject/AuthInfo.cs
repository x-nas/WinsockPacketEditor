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

        #region//设备与客户端（2026-09-14）

        string _DeviceId = string.Empty;

        /// <summary>WPC 报上来的设备指纹；普通 SOCKS5 客户端为空。</summary>
        public string DeviceId
        {
            get => _DeviceId;
            set
            {
                if (_DeviceId == value) return;
                _DeviceId = value;
                OnPropertyChanged();
            }
        }

        string _Client = string.Empty;

        /// <summary>"WPC 1.0" 或 "SOCKS5"。</summary>
        public string Client
        {
            get => _Client;
            set
            {
                if (_Client == value) return;
                _Client = value;
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

        #region//流量统计

        long _TrafficStatistics;

        public long TrafficStatistics
        {
            get => _TrafficStatistics;
            set
            {
                if (_TrafficStatistics == value) return;
                _TrafficStatistics = value;
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

        public AuthInfo(Guid AID, string AuthIP, string IPLocation, bool AuthResult, DateTime AuthTime)
        {
            this._AID = AID;
            this._AuthIP = AuthIP;
            this._IPLocation = IPLocation;
            this._LinksNumber = 0;
            this._DevicesNumber = 0;
            this._TrafficStatistics = 0;
            this._AuthResult = AuthResult;
            this._AuthTime = AuthTime;
        }

        #endregion        
    }
}
