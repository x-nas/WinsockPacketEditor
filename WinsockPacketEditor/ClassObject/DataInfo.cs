using AntdUI;

namespace WinsockPacketEditor
{
    public class DataInfo : NotifyProperty
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

        #region//DataInfo

        public DataInfo(bool IsCheck, byte[] PacketBuffer, int PacketLen)
        {
            this._IsCheck = IsCheck;
            this._PacketBuffer = PacketBuffer;
            this._PacketLen = PacketLen;
        }

        #endregion        
    }
}
