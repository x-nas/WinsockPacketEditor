using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class MapLocal : NotifyProperty
    {
        /// <summary>运行期 Id，构造时分配、<b>不落库</b>。表里没有主键，外壳按它收发（与 AutoStoresInfo.AID 同一个理由）。</summary>
        public Guid MID { get; } = Guid.NewGuid();

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

        #region//协议类型

        Operate.ProxyConfig.Proxy.MapProtocol _ProtocolType;

        public Operate.ProxyConfig.Proxy.MapProtocol ProtocolType
        {
            get => _ProtocolType;
            set
            {
                if (_ProtocolType == value) return;
                _ProtocolType = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//主机地址

        string _Host;

        public string Host
        {
            get => _Host;
            set
            {
                if (_Host == value) return;
                _Host = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//主机端口

        int _Port;

        public int Port
        {
            get => _Port;
            set
            {
                if (_Port == value) return;
                _Port = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//远程地址

        string _RemotePath;

        public string RemotePath
        {
            get => _RemotePath;
            set
            {
                if (_RemotePath == value) return;
                _RemotePath = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//本地文件

        string _LocalPath;

        public string LocalPath
        {
            get => _LocalPath;
            set
            {
                if (_LocalPath == value) return;
                _LocalPath = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//MapLocal

        public MapLocal(bool IsEnable, Operate.ProxyConfig.Proxy.MapProtocol ProtocolType, string Host, int Port, string RemotePath, string LocalPath) 
        {
            this._IsEnable = IsEnable;
            this._ProtocolType = ProtocolType;
            this._Host = Host;
            this._Port = Port;
            this._RemotePath = RemotePath;
            this._LocalPath = LocalPath;   
        }

        #endregion
    }
}
