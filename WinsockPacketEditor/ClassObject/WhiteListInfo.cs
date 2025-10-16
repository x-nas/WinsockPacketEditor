using AntdUI;

namespace WinsockPacketEditor
{
    public class WhiteListInfo : NotifyProperty
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

        #region//WhiteListInfo

        public WhiteListInfo(string IPAddress, string IPLocation)
        {
            this.IPAddress = IPAddress;
            this.IPLocation = IPLocation;
        }

        #endregion
    }
}
