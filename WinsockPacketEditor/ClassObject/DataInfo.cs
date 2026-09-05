using System;

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

        #region//运行期标识（不持久化）

        /// <summary>
        /// 运行期唯一标识，<b>只在本次运行内有效、不落库</b>（WareHouseData 表里没有这一列）。
        /// 仓库编辑器按它选行、取字节、做列表操作 —— 同一份字节可以在仓库里出现多次（右键「复制」），
        /// 按内容或下标都对不上行，所以要一个自己的 Id。与 AutoStoresInfo.AID 同一个理由。
        /// </summary>
        public Guid DID { get; } = Guid.NewGuid();

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
