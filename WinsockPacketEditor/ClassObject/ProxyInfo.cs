using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class ProxyInfo : NotifyProperty
    {
        #region//时间戳

        DateTime _ProxyTime;

        public DateTime ProxyTime
        {
            get => _ProxyTime;
            set
            {
                if (_ProxyTime == value) return;
                _ProxyTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//套接字

        int _PacketSocket;

        public int PacketSocket
        {
            get => _PacketSocket;
            set
            {
                if (_PacketSocket == value) return;
                _PacketSocket = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//封包类型

        Operate.PacketConfig.Packet.PacketType _PacketType;

        public Operate.PacketConfig.Packet.PacketType PacketType
        {
            get => _PacketType;
            set
            {
                if (_PacketType == value) return;
                _PacketType = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//数据类型

        Operate.ProxyConfig.Proxy.DataType _DataType;

        public Operate.ProxyConfig.Proxy.DataType DataType
        {
            get => _DataType;
            set
            {
                if (_DataType == value) return;
                _DataType = value;
                OnPropertyChanged();
            }
        }

        #endregion                

        #region//客户端地址

        string _ClientAddr;

        public string ClientAddr
        {
            get => _ClientAddr;
            set
            {
                if (_ClientAddr == value) return;
                _ClientAddr = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//服务端地址

        string _ServerAddr;

        public string ServerAddr
        {
            get => _ServerAddr;
            set
            {
                if (_ServerAddr == value) return;
                _ServerAddr = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//服务端域名

        string _ServerDomain;

        public string ServerDomain
        {
            get => _ServerDomain;
            set
            {
                if (_ServerDomain == value) return;
                _ServerDomain = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//域名类别

        Operate.ProxyConfig.Proxy.DomainType _DomainType;

        public Operate.ProxyConfig.Proxy.DomainType DomainType
        {
            get => _DomainType;
            set
            {
                if (_DomainType == value) return;
                _DomainType = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//原始封包数据（字节）

        byte[] _RawBuffer;

        public byte[] RawBuffer
        {
            get => _RawBuffer;
            set
            {
                if (_RawBuffer == value) return;
                _RawBuffer = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//封包数据（字节）

        byte[] _PacketBuffer;

        public byte[] PacketBuffer
        {
            get => _PacketBuffer;
            set
            {
                if (_PacketBuffer == value) return;
                _PacketBuffer = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//封包内容（十六进制）

        string _PacketData;

        public string PacketData
        {
            get => _PacketData;
            set
            {
                if (_PacketData == value) return;
                _PacketData = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//封包长度

        int _PacketLen;

        public int PacketLen
        {
            get => _PacketLen;
            set
            {
                if (_PacketLen == value) return;
                _PacketLen = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//过滤动作

        Operate.FilterConfig.Filter.FilterAction _FilterAction;

        public Operate.FilterConfig.Filter.FilterAction FilterAction
        {
            get => _FilterAction;
            set
            {
                if (_FilterAction == value) return;
                _FilterAction = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//ProxyInfo

        public ProxyInfo()
        {
            //
        }

        public ProxyInfo(
            DateTime ProxyTime,
            int PacketSocket,
            Operate.PacketConfig.Packet.PacketType PacketType,
            Operate.ProxyConfig.Proxy.DataType DataType,        
            string ClientAddr,
            string ServerAddr,
            string ServerDomain, 
            Operate.ProxyConfig.Proxy.DomainType DomainType,
            byte[] pRawBuffer,
            byte[] pBuffer,
            int pLen,
            Operate.FilterConfig.Filter.FilterAction pAction)
        {          
            this._ProxyTime = ProxyTime;
            this._PacketSocket = PacketSocket;
            this._PacketType = PacketType;
            this._DataType = DataType;
            this._ClientAddr = ClientAddr;
            this._ServerAddr = ServerAddr;
            this._ServerDomain = ServerDomain;
            this._DomainType = DomainType;
            this._RawBuffer = pRawBuffer;
            this._PacketBuffer = pBuffer;
            this._PacketLen = pLen;
            this._FilterAction = pAction;
        }

        #endregion
    }
}
