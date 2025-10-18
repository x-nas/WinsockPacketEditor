using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class BlackListInfo : NotifyProperty
    {
        #region//IP地址

        string _IPAddress;

        public string IPAddress
        {
            get => _IPAddress;
            set
            {
                if (_IPAddress == value) return;
                _IPAddress = value;
                OnPropertyChanged();
            }
        }

        long _StartIP;

        public long StartIP
        {
            get => _StartIP;
            set
            {
                if (_StartIP == value) return;
                _StartIP = value;
                OnPropertyChanged();
            }
        }

        long _EndIP;

        public long EndIP
        {
            get => _EndIP;
            set
            {
                if (_EndIP == value) return;
                _EndIP = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//所属地

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

        #region//生效次数

        long _EffectCount;

        public long EffectCount
        {
            get => _EffectCount;
            set
            {
                if (_EffectCount == value) return;
                _EffectCount = value;
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

        #region//创建时间

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

        #region//BlackListInfo

        public BlackListInfo(
            string IPAddress, 
            string IPLocation, 
            bool IsExpiry, 
            DateTime ExpiryTime,
            DateTime CreateTime)
        {
            this._IPAddress = IPAddress;
            this._IPLocation = IPLocation;
            this._EffectCount = 0;
            this._IsExpiry = IsExpiry;
            this._ExpiryTime = ExpiryTime;
            this._CreateTime = CreateTime;

            var ParseResult = Operate.ProxyConfig.Proxy.ParseIpRange(IPAddress);
            this._StartIP = ParseResult.StartIP;
            this._EndIP = ParseResult.EndIP;
        }

        #endregion

        #region//ContainsIp

        public bool ContainsIp(long ipValue)
        {
            return this._StartIP != -1 && this._EndIP != -1 && ipValue >= this._StartIP && ipValue <= this._EndIP;
        }

        #endregion        
    }
}
