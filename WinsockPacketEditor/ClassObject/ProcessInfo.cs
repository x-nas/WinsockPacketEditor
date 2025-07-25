using AntdUI;
using System.Drawing;

namespace WinsockPacketEditor
{
    public class ProcessInfo : NotifyProperty
    {
        #region//进程图标

        Image _ICO;

        public Image ICO
        {
            get => _ICO;
            set
            {
                _ICO = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//进程名称

        string _ProcessName;

        public string ProcessName
        {
            get => _ProcessName;
            set
            {
                if (_ProcessName == value) return;
                _ProcessName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//进程ID

        int _ProcessID;

        public int ProcessID
        {
            get => _ProcessID;
            set
            {
                if (_ProcessID == value) return;
                _ProcessID = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//进程路径

        string _ProcessPath;

        public string ProcessPath
        {
            get => _ProcessPath;
            set
            {
                if (_ProcessPath == value) return;
                _ProcessPath = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//ProcessInfo

        public ProcessInfo(Image ico, string processName, int processID, string processPath)
        {
            this._ICO = ico;
            this._ProcessName = processName;
            this._ProcessID = processID;
            this._ProcessPath = processPath;
        }

        #endregion
    }
}
