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

        #region//BlackListInfo

        public BlackListInfo(string IPAddress, string IPLocation, DateTime ExpiryTime)
        {
            this.IPAddress = IPAddress;
            this.IPLocation = IPLocation;
            this.ExpiryTime = ExpiryTime;
        }

        #endregion
    }
}
