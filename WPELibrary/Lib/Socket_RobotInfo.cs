using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPELibrary.Lib
{
    public class Socket_RobotInfo
    {
        #region//序号

        protected int rnum;

        public int RNum
        {
            get { return rnum; }
            set { rnum = value; }
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

        public Socket_RobotInfo(            
            int RNum,
            string RName,
            DataTable RInstructions
            )
        {
            this.rinstruction = RInstructions;
            this.rnum = RNum;
            this.rname = RName;           
        }

        #endregion
    }
}
