using System.Collections.Generic;
using System.Web.Http;

namespace WPELibrary.Lib.WebAPI
{
    [RoutePrefix("api/SocketInfo")]

    public class SocketInfo_Controller : ApiController
    {
        #region//获取封包信息

        [HttpGet]
        [Route("GetSocketInfo")]

        public IHttpActionResult GetSocketInfo()
        {
            string SocketProcess = Socket_Cache.SocketPacket.InjectProcess;
            string SocketWorkMode = Socket_Operation.GetWorkModeName(Socket_Cache.SocketPacket.SpeedMode);
            string SocketTotal = Socket_Cache.SocketPacket.TotalPackets.ToString();
            string SocketFilter = Socket_Cache.Filter.FilterExecute_CNT.ToString();
            string SocketCache = Socket_Cache.SocketQueue.qSocket_PacketInfo.Count.ToString();
            string SocketLeach = Socket_Cache.SocketQueue.FilterSocketList_CNT.ToString();
            string SocketSend = Socket_Cache.SocketQueue.Send_CNT.ToString();
            string SocketRecv = Socket_Cache.SocketQueue.Recv_CNT.ToString();
            string SocketSendTo = Socket_Cache.SocketQueue.SendTo_CNT.ToString();
            string SocketRecvFrom = Socket_Cache.SocketQueue.RecvFrom_CNT.ToString();
            string SocketWSASend = Socket_Cache.SocketQueue.WSASend_CNT.ToString();
            string SocketWSARecv = Socket_Cache.SocketQueue.WSARecv_CNT.ToString();
            string SocketWSASendTo = Socket_Cache.SocketQueue.WSASendTo_CNT.ToString();
            string SocketWSARecvFrom = Socket_Cache.SocketQueue.WSARecvFrom_CNT.ToString();
            string SocketBytes = Socket_Cache.SocketPacket.SocketBytesInfo;

            string[] SocketInfo = new string[15];
            SocketInfo[0] = SocketProcess;
            SocketInfo[1] = SocketWorkMode;
            SocketInfo[2] = SocketTotal;
            SocketInfo[3] = SocketFilter;
            SocketInfo[4] = SocketCache;
            SocketInfo[5] = SocketLeach;
            SocketInfo[6] = SocketSend;
            SocketInfo[7] = SocketRecv;
            SocketInfo[8] = SocketSendTo;
            SocketInfo[9] = SocketRecvFrom;
            SocketInfo[10] = SocketWSASend;
            SocketInfo[11] = SocketWSARecv;
            SocketInfo[12] = SocketWSASendTo;
            SocketInfo[13] = SocketWSARecvFrom;
            SocketInfo[14] = SocketBytes;            

            return Ok(SocketInfo);
        }

        #endregion

        #region//获取封包日志

        [HttpGet]
        [Route("GetSocketLogList")]

        public IEnumerable<Socket_LogInfo> GetSocketLogList()
        {
            return Socket_Cache.LogList.lstSocketLog;
        }

        #endregion
    }
}
