using System.Collections.Generic;
using System.Web.Http;

namespace WPE.Lib.WebAPI
{
    [RoutePrefix("SocketInfo")]

    public class SocketInfo_Controller : ApiController
    {
        #region//获取封包信息

        [HttpGet]
        [Route("GetSocketInfo")]

        public IHttpActionResult GetSocketInfo()
        {
            string SocketProcess = Operate.PacketConfig.Packet.InjectProcess;
            string SocketWorkMode = Socket_Operation.GetWorkModeName(Operate.PacketConfig.Packet.SpeedMode);
            string SocketTotal = Operate.PacketConfig.Packet.TotalPackets.ToString();
            string SocketFilter = Operate.FilterConfig.Filter.FilterExecute_CNT.ToString();
            string SocketCache = Operate.PacketConfig.Queue.cqPacketInfo.Count.ToString();
            string SocketLeach = Operate.PacketConfig.Queue.FilterSocketList_CNT.ToString();
            string SocketSend = Operate.PacketConfig.Queue.Send_CNT.ToString();
            string SocketRecv = Operate.PacketConfig.Queue.Recv_CNT.ToString();
            string SocketSendTo = Operate.PacketConfig.Queue.SendTo_CNT.ToString();
            string SocketRecvFrom = Operate.PacketConfig.Queue.RecvFrom_CNT.ToString();
            string SocketWSASend = Operate.PacketConfig.Queue.WSASend_CNT.ToString();
            string SocketWSARecv = Operate.PacketConfig.Queue.WSARecv_CNT.ToString();
            string SocketWSASendTo = Operate.PacketConfig.Queue.WSASendTo_CNT.ToString();
            string SocketWSARecvFrom = Operate.PacketConfig.Queue.WSARecvFrom_CNT.ToString();
            string SocketBytes = Operate.PacketConfig.Packet.SocketBytesInfo;

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

        public IEnumerable<LogInfo> GetSocketLogList()
        {
            return Operate.LogConfig.lstLogInfo;
        }

        #endregion
    }
}
