using AntdUI;
using System;
using System.Net;

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

        #region//协议类型

        System.Net.Sockets.ProtocolType _ProtocolType;

        public System.Net.Sockets.ProtocolType ProtocolType
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

        #region//客户端IP地址

        IPEndPoint _ClientIP;

        public IPEndPoint ClientIP
        {
            get => _ClientIP;
            set
            {
                if (_ClientIP == value) return;
                _ClientIP = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//服务端IP地址

        IPEndPoint _ServerIP;

        public IPEndPoint ServerIP
        {
            get => _ServerIP;
            set
            {
                if (_ServerIP == value) return;
                _ServerIP = value;
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

        #region//ProxyInfo

        public ProxyInfo(
            DateTime ProxyTime,
            int PacketSocket,
            System.Net.Sockets.ProtocolType ProtocolType,
            Operate.ProxyConfig.Proxy.DataType DataType,        
            IPEndPoint ClientIP,        
            IPEndPoint ServerIP,
            string ServerDomain, 
            Operate.ProxyConfig.Proxy.DomainType DomainType,
            byte[] pBuffer,
            int pLen)
        {          
            this._ProxyTime = ProxyTime;
            this._PacketSocket = PacketSocket;
            this._ProtocolType = ProtocolType;
            this._DataType = DataType;
            this._ClientIP = ClientIP;
            this._ServerIP = ServerIP;
            this._ServerDomain = ServerDomain;
            this._DomainType = DomainType;
            this._PacketBuffer = pBuffer;
            this._PacketLen = pLen;
        }

        #endregion
    }
}
