using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class AutoStoresInfo : NotifyProperty
    {
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

        #region//指定包头

        string _PacketHead;

        public string PacketHead
        {
            get => _PacketHead;
            set
            {
                if (_PacketHead == value) return;
                _PacketHead = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//仓库序号

        Guid _WID;

        public Guid WID
        {
            get => _WID;
            set
            {
                if (_WID == value) return;
                _WID = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//AutoStoresInfo

        public AutoStoresInfo(bool IsEnable, string PacketHead, Guid WID)
        { 
            this._IsEnable = IsEnable;
            this._PacketHead = PacketHead;
            this._WID = WID;
        }

        #endregion
    }
}
