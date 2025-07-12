using AntdUI;

namespace WPE.Lib.ClassObject
{
    public class InstructionInfo : NotifyProperty
    {
        #region//指令类型

        Operate.RobotConfig.Robot.InstructionType _InstType;

        public Operate.RobotConfig.Robot.InstructionType InstType
        {
            get => _InstType;
            set
            {
                if (_InstType == value) return;
                _InstType = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//指令内容

        string _InstContent;

        public string InstContent
        {
            get => _InstContent;
            set
            {
                if (_InstContent == value) return;
                _InstContent = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//InstructionInfo

        public InstructionInfo(Operate.RobotConfig.Robot.InstructionType instructionType, string InstContent)
        { 
            this._InstType = instructionType;
            this._InstContent = InstContent;
        }

        #endregion
    }
}
