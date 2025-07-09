using AntdUI;
using System;
using System.Data;
using System.Drawing;

namespace WPE.Lib
{
    public class RobotInfo : NotifyProperty
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

        Guid _RID;

        public Guid RID
        {
            get => _RID;
            set
            {
                if (_RID == value) return;
                _RID = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//机器人名称

        string _RName;

        public string RName
        {
            get => _RName;
            set
            {
                if (_RName == value) return;
                _RName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//已执行次数

        CellText _ExecutionCount = new CellText("0")
        {
            Fore = Color.Blue,
        };

        public CellText ExecutionCount
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

        #region//指令集        

        protected DataTable _RInstruction;

        public DataTable RInstruction
        {
            get { return _RInstruction; }
            set 
            {
                _RInstruction = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//列表操作

        AntdUI.CellLink[] _CellLinks = new AntdUI.CellLink[]
        {
            new AntdUI.CellButton("bEdit", AntdUI.Localization.Get("System.Button.Edit", "编辑"), AntdUI.TTypeMini.Primary),
            new AntdUI.CellButton("bDelete", AntdUI.Localization.Get("System.Button.Delete", "删除"), AntdUI.TTypeMini.Error)
        };

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

        #region//RobotInfo

        public RobotInfo(bool IsEnable, Guid RID, string RName, DataTable RInstructions)
        {
            this._IsEnable = IsEnable;
            this._RID = RID;
            this._RName = RName;
            this._RInstruction = RInstructions;
        }

        public void AddExecutionCount()
        {
            if (int.TryParse(this._ExecutionCount.Text.Trim(), out int iCNT))
            {
                iCNT++;
                this._ExecutionCount.Text = iCNT.ToString();
            }
        }

        #endregion
    }
}
