using System;

namespace WPELibrary.Lib
{
    public class Socket_FilterInfo
    {
        #region//是否启用

        protected bool isenable;

        public bool IsEnable
        {
            get { return isenable; }
            set { isenable = value; }
        }

        #endregion

        #region//序号

        protected Guid fid;

        public Guid FID
        {
            get { return fid; }
            set { fid = value; }
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

        #region//指定包头

        protected bool appointheader;

        public bool AppointHeader
        {
            get { return appointheader; }
            set { appointheader = value; }
        }

        protected string headercontent;

        public string HeaderContent
        {
            get { return headercontent; }
            set { headercontent = value; }
        }

        #endregion        

        #region//指定套接字

        protected bool appointsocket;

        public bool AppointSocket
        {
            get { return appointsocket; }
            set { appointsocket = value; }
        }

        protected decimal socketcontent;

        public decimal SocketContent
        {
            get { return socketcontent; }
            set { socketcontent = value; }
        }

        #endregion

        #region//指定长度

        protected bool appointlength;

        public bool AppointLength
        {
            get { return appointlength; }
            set { appointlength = value; }
        }

        protected string lengthcontent;

        public string LengthContent
        {
            get { return lengthcontent; }
            set { lengthcontent = value; }
        }

        #endregion

        #region//指定端口

        protected bool appointport;

        public bool AppointPort
        {
            get { return appointport; }
            set { appointport = value; }
        }

        protected decimal portcontent;

        public decimal PortContent
        {
            get { return portcontent; }
            set { portcontent = value; }
        }

        #endregion

        #region//模式

        protected Socket_Cache.Filter.FilterMode fmode;

        public Socket_Cache.Filter.FilterMode FMode
        {
            get { return fmode; }
            set { fmode = value; }
        }

        #endregion        

        #region//动作

        protected Socket_Cache.Filter.FilterAction faction;

        public Socket_Cache.Filter.FilterAction FAction
        {
            get { return faction; }
            set { faction = value; }
        }

        #endregion        

        #region//是否执行

        protected bool isexecute;

        public bool IsExecute
        {
            get { return isexecute; }
            set { isexecute = value; }
        }

        #endregion

        #region//执行类型

        protected Socket_Cache.Filter.FilterExecuteType fetype;

        public Socket_Cache.Filter.FilterExecuteType FEType
        {
            get { return fetype; }
            set { fetype = value; }
        }

        #endregion        

        #region//发送序号

        protected Guid sid;

        public Guid SID
        {
            get { return sid; }
            set { sid = value; }
        }

        #endregion        

        #region//机器人序号

        protected Guid rid;

        public Guid RID
        {
            get { return rid; }
            set { rid = value; }
        }

        #endregion        

        #region//作用类别

        protected Socket_Cache.Filter.FilterFunction ffunction;

        public Socket_Cache.Filter.FilterFunction FFunction
        {
            get { return ffunction; }
            set { ffunction = value; }
        }

        #endregion        

        #region//修改起始于

        protected Socket_Cache.Filter.FilterStartFrom fstartfrom;

        public Socket_Cache.Filter.FilterStartFrom FStartFrom
        {
            get { return fstartfrom; }
            set { fstartfrom = value; }
        }

        #endregion

        #region//是否递进完成

        protected bool isprogressiondone;

        public bool IsProgressionDone
        {
            get { return isprogressiondone; }
            set { isprogressiondone = value; }
        }

        #endregion

        #region//递进步长

        protected decimal progressionstep;

        public decimal ProgressionStep
        {
            get { return progressionstep; }
            set { progressionstep = value; }
        }

        #endregion

        #region//递进位置

        protected string progressionposition;

        public string ProgressionPosition
        {
            get { return progressionposition; }
            set { progressionposition = value; }
        }

        #endregion

        #region//递进已执行次数

        protected int progressioncount;

        public int ProgressionCount
        {
            get { return progressioncount; }
            set { progressioncount = value; }
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

        #region//Socket_FilterInfo

        public Socket_FilterInfo(
            bool IsEnable, 
            Guid FID, 
            string FName, 
            bool AppointHeader, 
            string HeaderContent, 
            bool AppointSocket, 
            decimal SocketContent, 
            bool AppointLength, 
            string LengthContent,
            bool AppointPort,
            decimal PortContent,
            Socket_Cache.Filter.FilterMode FMode, 
            Socket_Cache.Filter.FilterAction FAction,
            bool IsExecute,
            Socket_Cache.Filter.FilterExecuteType FEType,
            Guid SID,
            Guid RID,
            Socket_Cache.Filter.FilterFunction FFunction, 
            Socket_Cache.Filter.FilterStartFrom FStartFrom,
            bool IsProgressionDone,
            decimal ProgressionStep,
            string ProgressionPosition,
            int ProgressionCount,
            string FSearch, 
            string FModify) 
        {
            this.isenable = IsEnable;
            this.fid = FID;            
            this.fname = FName;
            this.appointheader = AppointHeader;
            this.headercontent = HeaderContent;
            this.appointsocket = AppointSocket;
            this.socketcontent = SocketContent;
            this.appointlength = AppointLength;
            this.lengthcontent = LengthContent;
            this.appointport = AppointPort;
            this.portcontent = PortContent;
            this.fmode = FMode;
            this.faction = FAction;
            this.isexecute = IsExecute;
            this.fetype = FEType;
            this.sid = SID;
            this.rid = RID;
            this.ffunction = FFunction;
            this.fstartfrom = FStartFrom;
            this.isprogressiondone = IsProgressionDone;
            this.progressionstep = ProgressionStep;
            this.progressionposition = ProgressionPosition;
            this.progressioncount = ProgressionCount;
            this.fsearch = FSearch;          
            this.fmodify = FModify;         
        }

        #endregion
    }
}
