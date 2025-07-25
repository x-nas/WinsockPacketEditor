using AntdUI;

namespace WinsockPacketEditor
{
    public class MapRemote : NotifyProperty
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

        #region//请求地址

        #region//协议类型

        Operate.ProxyConfig.Proxy.MapProtocol _ProtocolTypeFrom;

        public Operate.ProxyConfig.Proxy.MapProtocol ProtocolTypeFrom
        {
            get => _ProtocolTypeFrom;
            set
            {
                if (_ProtocolTypeFrom == value) return;
                _ProtocolTypeFrom = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//主机地址

        string _HostFrom;

        public string HostFrom
        {
            get => _HostFrom;
            set
            {
                if (_HostFrom == value) return;
                _HostFrom = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//主机端口

        int _PortFrom;

        public int PortFrom
        {
            get => _PortFrom;
            set
            {
                if (_PortFrom == value) return;
                _PortFrom = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//路径地址

        string _PathFrom;

        public string PathFrom
        {
            get => _PathFrom;
            set
            {
                if (_PathFrom == value) return;
                _PathFrom = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region//映射地址

        #region//协议类型

        Operate.ProxyConfig.Proxy.MapProtocol _ProtocolTypeTo;

        public Operate.ProxyConfig.Proxy.MapProtocol ProtocolTypeTo
        {
            get => _ProtocolTypeTo;
            set
            {
                if (_ProtocolTypeTo == value) return;
                _ProtocolTypeTo = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//主机地址

        string _HostTo;

        public string HostTo
        {
            get => _HostTo;
            set
            {
                if (_HostTo == value) return;
                _HostTo = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//主机端口

        int _PortTo;

        public int PortTo
        {
            get => _PortTo;
            set
            {
                if (_PortTo == value) return;
                _PortTo = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//路径地址

        string _PathTo;

        public string PathTo
        {
            get => _PathTo;
            set
            {
                if (_PathTo == value) return;
                _PathTo = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region//MapRemote

        public MapRemote(
            bool IsEnable, 
            Operate.ProxyConfig.Proxy.MapProtocol ProtocolTypeFrom, 
            string HostFrom, 
            int PortFrom, 
            string PathFrom,
            Operate.ProxyConfig.Proxy.MapProtocol ProtocolTypeTo,
            string HostTo,
            int PortTo,
            string PathTo) 
        {
            this._IsEnable = IsEnable;
            this._ProtocolTypeFrom = ProtocolTypeFrom;
            this._HostFrom = HostFrom;
            this._PortFrom = PortFrom;
            this._PathFrom = PathFrom;
            this._ProtocolTypeTo = ProtocolTypeTo;
            this._HostTo = HostTo;
            this._PortTo = PortTo;
            this._PathTo = PathTo;           
        }

        #endregion
    }
}
