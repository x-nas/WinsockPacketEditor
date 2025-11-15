using AntdUI;
using System;
using System.ComponentModel;

namespace WinsockPacketEditor
{
    public class WareHouseInfo : NotifyProperty
    {
        #region//序号

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

        #region//仓库名称

        string _WName;

        public string WName
        {
            get => _WName;
            set
            {
                if (_WName == value) return;
                _WName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//仓储数据

        BindingList<DataInfo> _Stores = new BindingList<DataInfo>();

        public BindingList<DataInfo> Stores
        {
            get => _Stores;
            set
            {
                if (_Stores == value) return;
                _Stores = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//WareHouseInfo

        public WareHouseInfo(Guid WID, string WName, BindingList<DataInfo> Stores)
        {
            this._WID = WID;
            this._WName = WName;
            this._Stores = Stores;
        }

        #endregion
    }
}
