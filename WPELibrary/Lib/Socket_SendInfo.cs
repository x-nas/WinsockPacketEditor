using System;
using System.Data;

namespace WPELibrary.Lib
{
    public class Socket_SendInfo
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

        protected Guid sid;

        public Guid SID
        {
            get { return sid; }
            set { sid = value; }
        }

        #endregion        

        #region//名称

        protected string sname;

        public string SName
        {
            get { return sname; }
            set { sname = value; }
        }

        #endregion

        #region//系统套接字

        protected bool ssystemsocket;

        public bool SSystemSocket
        {
            get { return ssystemsocket; }
            set { ssystemsocket = value; }
        }

        #endregion        

        #region//循环次数

        protected int sloopcnt;

        public int SLoopCNT
        {
            get { return sloopcnt; }
            set { sloopcnt = value; }
        }

        #endregion

        #region//循环间隔

        protected int sloopint;

        public int SLoopINT
        {
            get { return sloopint; }
            set { sloopint = value; }
        }

        #endregion

        #region//发送集合

        protected DataTable scollection;

        public DataTable SCollection
        {
            get { return scollection; }
            set { scollection = value; }
        }

        #endregion

        #region//备注

        protected string snotes;

        public string SNotes
        {
            get { return snotes; }
            set { snotes = value; }
        }

        #endregion

        #region//Socket_SendInfo

        public Socket_SendInfo(
            bool IsEnable, 
            Guid SID, 
            string SName, 
            bool SSystemSocket, 
            int SLoopCNT, 
            int SLoopINT, 
            DataTable SCollection, 
            string SNotes)
        {
            this.isenable = IsEnable;
            this.sid = SID;
            this.sname = SName;
            this.ssystemsocket = SSystemSocket;         
            this.sloopcnt = SLoopCNT;
            this.sloopint = SLoopINT;
            this.scollection = SCollection;
            this.snotes = SNotes;
        }

        #endregion
    }
}
