using AntdUI;

namespace WinsockPacketEditor
{
    public class DataInfo : NotifyProperty
    {
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

        #region//DataInfo

        public DataInfo(byte[] PacketBuffer)
        {
            this._PacketBuffer = PacketBuffer;
        }

        #endregion        
    }
}
