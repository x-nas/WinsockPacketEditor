using System;
using System.Data;

namespace WPELibrary.Lib
{
    public class Socket_RobotInfo
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

        protected Guid rid;

        public Guid RID
        {
            get { return rid; }
            set { rid = value; }
        }

        #endregion

        #region//机器人名称

        protected string rname;

        public string RName
        {
            get { return rname; }
            set { rname = value; }
        }

        #endregion

        #region//指令集        

        protected DataTable rinstruction;

        public DataTable RInstruction
        {
            get { return rinstruction; }
            set { rinstruction = value; }
        }        

        #endregion

        #region//Socket_RobotInfo

        public Socket_RobotInfo(bool IsEnable, Guid RID, string RName, DataTable RInstructions)
        {
            this.isenable = IsEnable;
            this.rid = RID;
            this.rname = RName;
            this.rinstruction = RInstructions;
        }

        #endregion
    }
}
