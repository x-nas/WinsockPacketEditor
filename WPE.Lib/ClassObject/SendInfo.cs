using AntdUI;
using System;
using System.ComponentModel;

namespace WPE.Lib
{
    public class SendInfo : NotifyProperty
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

        #region//序号

        Guid _SID;

        public Guid SID
        {
            get => _SID;
            set
            {
                if (_SID == value) return;
                _SID = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//发送名称

        string _SName;

        public string SName
        {
            get => _SName;
            set
            {
                if (_SName == value) return;
                _SName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//发送已执行次数

        int _ExecutionCount;

        public int ExecutionCount
        {
            get => _ExecutionCount;
            set
            {
                if (_ExecutionCount == value) return;
                _ExecutionCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//使用系统套接字

        bool _SSystemSocket;

        public bool SSystemSocket
        {
            get => _SSystemSocket;
            set
            {
                if (_SSystemSocket == value) return;
                _SSystemSocket = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//循环次数

        int _SLoopCNT;

        public int SLoopCNT
        {
            get => _SLoopCNT;
            set
            {
                if (_SLoopCNT == value) return;
                _SLoopCNT = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//循环间隔

        int _SLoopINT;

        public int SLoopINT
        {
            get => _SLoopINT;
            set
            {
                if (_SLoopINT == value) return;
                _SLoopINT = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//发送集

        BindingList<PacketInfo> _SCollection;

        public BindingList<PacketInfo> SCollection
        {
            get => _SCollection;
            set
            {
                if (_SCollection == value) return;
                _SCollection = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//备注

        string _SNotes;

        public string SNotes
        {
            get => _SNotes;
            set
            {
                if (_SNotes == value) return;
                _SNotes = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//列表操作

        AntdUI.CellLink[] _CellLinks;
        public AntdUI.CellLink[] CellLinks
        {
            get => _CellLinks;
            set
            {
                _CellLinks = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//SendInfo

        public SendInfo(
            bool IsEnable, 
            Guid SID, 
            string SName, 
            bool SSystemSocket, 
            int SLoopCNT, 
            int SLoopINT,
            BindingList<PacketInfo> SCollection, 
            string SNotes)
        {
            this._IsEnable = IsEnable;
            this._SID = SID;
            this._SName = SName;
            this._SSystemSocket = SSystemSocket;         
            this._SLoopCNT = SLoopCNT;
            this._SLoopINT = SLoopINT;
            this._SCollection = SCollection;
            this._SNotes = SNotes;
            this._ExecutionCount = 0;

            CellLinks = new AntdUI.CellLink[]
            {
                new AntdUI.CellButton("bEdit", AntdUI.Localization.Get("System.Button.Edit", "编辑"), AntdUI.TTypeMini.Primary),
                new AntdUI.CellButton("bDelete", AntdUI.Localization.Get("System.Button.Delete", "删除"), AntdUI.TTypeMini.Error)
            };
        }        

        #endregion
    }
}
