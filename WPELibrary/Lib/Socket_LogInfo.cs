using System;

namespace WPELibrary.Lib
{
    public class Socket_LogInfo
    {
        #region//时间戳

        protected string logtime;

        public string LogTime
        {
            get { return logtime; }
            set { logtime = value; }
        }

        #endregion

        #region//模块名称

        protected string funcname;

        public string FuncName
        {
            get { return funcname; }
            set { funcname = value; }
        }

        #endregion

        #region//日志内容

        protected string logcontent;

        public string LogContent
        {
            get { return logcontent; }
            set { logcontent = value; }
        }

        #endregion

        #region//Socket_LogInfo

        public Socket_LogInfo(string funcname, string logcontent)
        {
            this.logtime = DateTime.Now.ToString("HH:mm:ss:fff");
            this.funcname = funcname;
            this.logcontent = logcontent;           
        }

        #endregion        
    }
}
