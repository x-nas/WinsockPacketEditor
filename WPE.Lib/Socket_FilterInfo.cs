using System;

namespace WPE.Lib
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

        #region//滤镜已执行次数

        protected int executioncount;

        public int ExecutionCount
        {
            get { return executioncount; }
            set { executioncount = value; }
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

        protected Operate.FilterConfig.Filter.FilterMode fmode;

        public Operate.FilterConfig.Filter.FilterMode FMode
        {
            get { return fmode; }
            set { fmode = value; }
        }

        #endregion        

        #region//动作

        protected Operate.FilterConfig.Filter.FilterAction faction;

        public Operate.FilterConfig.Filter.FilterAction FAction
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

        protected Operate.FilterConfig.Filter.FilterExecuteType fetype;

        public Operate.FilterConfig.Filter.FilterExecuteType FEType
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

        protected Operate.FilterConfig.Filter.FilterFunction ffunction;

        public Operate.FilterConfig.Filter.FilterFunction FFunction
        {
            get { return ffunction; }
            set { ffunction = value; }
        }

        #endregion        

        #region//修改起始于

        protected Operate.FilterConfig.Filter.FilterStartFrom fstartfrom;

        public Operate.FilterConfig.Filter.FilterStartFrom FStartFrom
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

        #region//是否连续递进

        protected bool isprogressioncontinuous;

        public bool IsProgressionContinuous
        {
            get { return isprogressioncontinuous; }
            set { isprogressioncontinuous = value; }
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

        #region//是否进位递进

        protected bool isprogressioncarry;

        public bool IsProgressionCarry
        {
            get { return isprogressioncarry; }
            set { isprogressioncarry = value; }
        }

        #endregion

        #region//进位位数

        protected decimal progressioncarrynumber;

        public decimal ProgressionCarryNumber
        {
            get { return progressioncarrynumber; }
            set { progressioncarrynumber = value; }
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
            Operate.FilterConfig.Filter.FilterMode FMode, 
            Operate.FilterConfig.Filter.FilterAction FAction,
            bool IsExecute,
            Operate.FilterConfig.Filter.FilterExecuteType FEType,
            Guid SID,
            Guid RID,
            Operate.FilterConfig.Filter.FilterFunction FFunction, 
            Operate.FilterConfig.Filter.FilterStartFrom FStartFrom,
            bool IsProgressionDone,
            bool IsProgressionContinuous,
            decimal ProgressionStep,
            bool IsProgressionCarry,
            decimal ProgressionCarryNumber,
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
            this.isprogressioncontinuous = IsProgressionContinuous;
            this.progressionstep = ProgressionStep;
            this.isprogressioncarry = IsProgressionCarry;
            this.progressioncarrynumber = ProgressionCarryNumber;
            this.progressionposition = ProgressionPosition;
            this.progressioncount = ProgressionCount;
            this.fsearch = FSearch;          
            this.fmodify = FModify;
            
            this.executioncount = 0;
        }

        #endregion
    }
}
