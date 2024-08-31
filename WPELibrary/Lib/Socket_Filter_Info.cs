using System;

namespace WPELibrary.Lib
{
    public class Socket_Filter_Info
    {
        #region//定义结构

        public enum FilterDGVType
        { 
            Search = 1,
            Modify = 2,
            All = 3,
        }

        public enum FilterMode
        {
            Normal = 1,
            Advanced = 2,
        }

        public enum StartFrom
        { 
            Head = 1,
            Position = 2,
        }      

        #endregion

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

        #region//模式选项
        protected FilterMode fmode;
        public FilterMode FMode
        {
            get { return fmode; }
            set { fmode = value; }
        }
        #endregion        

        #region//修改起始于选项
        protected StartFrom fstartfrom;
        public StartFrom FStartFrom
        {
            get { return fstartfrom; }
            set { fstartfrom = value; }
        }
        #endregion

        #region//修改次数
        protected int fmodifycnt;
        public int FModifyCNT
        {
            get { return fmodifycnt; }
            set { fmodifycnt = value; }
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

        #region//搜索长度
        protected int fsearchlen;
        public int FSearchLen
        {
            get { return fsearchlen; }
            set { fsearchlen = value; }
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

        #region//修改长度
        protected int fmodifylen;
        public int FModifyLen
        {
            get { return fmodifylen; }
            set { fmodifylen = value; }
        }
        #endregion

        #region//Socket_Filter

        public Socket_Filter_Info(int FNum,bool IsCheck, string FName, FilterMode FMode, StartFrom FStartFrom, int FModifyCNT, string FSearch, int FSearchLen, string FModify, int FModifyLen) 
        {
            this.fnum = FNum;
            this.ischeck = IsCheck;
            this.fname = FName;
            this.fmode = FMode;          
            this.fstartfrom = FStartFrom;
            this.fmodifycnt = FModifyCNT;
            this.fsearch = FSearch;
            this.fsearchlen = FSearchLen;
            this.fmodify = FModify;
            this.fmodifylen = FModifyLen;
        }

        #endregion
    }
}
