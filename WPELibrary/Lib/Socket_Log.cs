using System;

namespace WPELibrary.Lib
{
    public class Socket_Log
    {
        #region//时间
        protected string time;
        public string Time
        {
            get { return time; }
            set { time = value; }
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
        public Socket_Log(string content)
        {
            this.time = DateTime.Now.ToString("HH:mm:ss:ffff");
            this.content = content;           
        }
        #endregion        
    }
}
