using AntdUI;
using System;

namespace WPE.Lib
{
    public class LogInfo : NotifyProperty
    {
        #region//时间戳

        DateTime _LogTime;

        public DateTime LogTime
        {
            get => _LogTime;
            set
            {
                if (_LogTime == value) return;
                _LogTime = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//模块

        string _FuncName;

        public string FuncName
        {
            get => _FuncName;
            set
            {
                if (_FuncName == value) return;
                _FuncName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//日志内容

        string _LogContent;

        public string LogContent
        {
            get => _LogContent;
            set
            {
                if (_LogContent == value) return;
                _LogContent = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//LogInfo

        public LogInfo(string funcname, string logcontent)
        {
            this._LogTime = DateTime.Now;
            this._FuncName = funcname;
            this._LogContent = logcontent;
        }

        #endregion        
    }
}
