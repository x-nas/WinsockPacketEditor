using System;

namespace WPELibrary.Lib
{
    public class Socket_Filter_Info
    {        
        #region//序号
        protected int fnum;
        public int FNum
        {
            get { return fnum; }
            set { fnum = value; }
        }
        #endregion

        #region//是否启用
        protected bool ischeck;
        public bool IsCheck
        {
            get { return ischeck; }
            set { ischeck = value; }
        }
        #endregion

        #region//滤镜名称
        protected string fname;
        public string FName
        {
            get { return fname; }
            set { fname = value; }
        }
        #endregion

        #region//搜索内容
        protected string fsearch;
        public string FSearch
        {
            get { return fsearch; }
            set { fsearch = value; }
        }
        #endregion

        #region//修改内容
        protected string fmodify;
        public string FModify
        {
            get { return fmodify; }
            set { fmodify = value; }
        }
        #endregion

        #region//Socket_Filter
        public Socket_Filter_Info(int FNum,bool IsCheck, string FName, string FSearch, string FModify) 
        {
            this.fnum = FNum;
            this.ischeck = IsCheck;
            this.fname = FName;
            this.fsearch = FSearch;
            this.fmodify = FModify;
        }
        #endregion
    }
}
