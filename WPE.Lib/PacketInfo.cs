using AntdUI;
using System;

namespace WPE.Lib
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

        #region//封包图标

        AntdUI.CellImage _PacketImg;

        public AntdUI.CellImage PacketImg
        {
            get => _PacketImg;
            set
            {
                _PacketImg = value;
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

        #region//PacketInfo

        public PacketInfo()
        { 
            //
        }

        public PacketInfo(DateTime pTime, int pSocket, Operate.PacketConfig.Packet.PacketType pType, string pFrom, string pTo, byte[] pRawBuffer, byte[] pBuffer, int pLen, Operate.FilterConfig.Filter.FilterAction pAction)
        {  
            this._PacketTime = pTime;            
            this._PacketSocket = pSocket;          
            this._PacketType = pType;
            this._PacketFrom = pFrom;
            this._PacketTo = pTo;
            this._RawBuffer = pRawBuffer;
            this._PacketBuffer = pBuffer;
            this._PacketLen = pLen;
            this._FilterAction = pAction;
            this._PacketImg = new AntdUI.CellImage(Operate.PacketConfig.Packet.GetImg_ByPacketType(pType));
        }

        #endregion        
    }
}
