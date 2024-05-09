using System;

namespace WPELibrary.Lib
{
    public class Socket_Log_Info
    {
        #region//时间
        protected string time;
        public string Time
        {
            get { return time; }
            set { time = value; }
        }
        #endregion

        #region//模块
        protected string funcname;
        public string FuncName
        {
            get { return funcname; }
            set { funcname = value; }
        }
        #endregion

        #region//内容
        protected string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        #endregion

        #region//Socket_Log
        public Socket_Log_Info(string funcname, string content)
        {
            this.time = DateTime.Now.ToString("T");
            this.funcname = funcname;
            this.content = content;           
        }
        #endregion        
    }
}
