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

        #region//运行期标识（不持久化）

        /// <summary>
        /// 运行期唯一标识，<b>只在本次运行内有效、不落库</b>。
        ///
        /// 这个模型没有自然主键：表里连 GUID 列都没有，PacketHead 是事实上的唯一键
        /// （<c>InsertTable_AutoStores</c> 按它查重）。而桥那边一律按 Id 收发，
        /// 拿 PacketHead 当 Id 的话，编辑时改了包头就对不上行了 —— 所以补一个。
        /// </summary>
        public Guid AID { get; } = Guid.NewGuid();

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
