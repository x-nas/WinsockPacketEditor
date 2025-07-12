using AntdUI;
using System;
using System.ComponentModel;
using WPE.Lib.ClassObject;

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

        long _ExecutionCount;

        public long ExecutionCount
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

        protected BindingList<InstructionInfo> _RInstruction;

        public BindingList<InstructionInfo> RInstruction
        {
            get { return _RInstruction; }
            set 
            {
                _RInstruction = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//RobotInfo

        public RobotInfo(bool IsEnable, Guid RID, string RName, BindingList<InstructionInfo> RInstructions)
        {
            this._IsEnable = IsEnable;
            this._RID = RID;
            this._RName = RName;
            this._RInstruction = RInstructions;
            this._ExecutionCount = 0;
        }        

        #endregion
    }
}
