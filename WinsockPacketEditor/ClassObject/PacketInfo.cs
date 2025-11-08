using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class PacketInfo : NotifyProperty
    {
        #region//时间戳

        DateTime _PacketTime;

        public DateTime PacketTime
        {
            get => _PacketTime;
            set
            {
                if (_PacketTime == value) return;
                _PacketTime = value;
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

        #region//封包类别

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

        #region//源地址

        string _PacketFrom;

        public string PacketFrom
        {
            get => _PacketFrom;
            set
            {
                if (_PacketFrom == value) return;
                _PacketFrom = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//源所属地

        string _FromLocation;

        public string FromLocation
        {
            get => _FromLocation;
            set
            {
                if (_FromLocation == value) return;
                _FromLocation = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//目的地址

        string _PacketTo;

        public string PacketTo
        {
            get => _PacketTo;
            set
            {
                if (_PacketTo == value) return;
                _PacketTo = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//目的所属地

        string _ToLocation;

        public string ToLocation
        {
            get => _ToLocation;
            set
            {
                if (_ToLocation == value) return;
                _ToLocation = value;
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

        #region//封包内容

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

        #region//PacketInfo

        public PacketInfo()
        { 
            //
        }

        public PacketInfo(
            DateTime PacketTime, 
            int PacketSocket, 
            Operate.PacketConfig.Packet.PacketType PacketType, 
            string PacketFrom,
            string FromLocation,
            string PacketTo,
            string ToLocation,
            byte[] RawBuffer, 
            byte[] PacketBuffer, 
            string PacketData,
            int PacketLen, 
            Operate.FilterConfig.Filter.FilterAction FilterAction)
        {  
            this._PacketTime = PacketTime;            
            this._PacketSocket = PacketSocket;          
            this._PacketType = PacketType;
            this._PacketFrom = PacketFrom;
            this._FromLocation = FromLocation;
            this._PacketTo = PacketTo;
            this._ToLocation = ToLocation;
            this._RawBuffer = RawBuffer;
            this._PacketBuffer = PacketBuffer;
            this._PacketData = PacketData;
            this._PacketLen = PacketLen;
            this._FilterAction = FilterAction;
        }

        #endregion        
    }
}
