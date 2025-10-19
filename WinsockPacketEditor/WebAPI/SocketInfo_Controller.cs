using System.Collections.Generic;
using System.Web.Http;

namespace WinsockPacketEditor
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
            string SocketWorkMode = Operate.SystemConfig.GetWorkModeName();
            string SocketTotal = Operate.PacketConfig.Packet.TotalPackets.ToString();
            string SocketFilter = Operate.FilterConfig.Filter.FilterExecute_CNT.ToString();
            string SocketCache = Operate.PacketConfig.Queue.cqPacketInfo.Count.ToString();
            string SocketLeach = Operate.PacketConfig.Packet.FilterPacket_CNT.ToString();
            string SocketSend = Operate.PacketConfig.Packet.Send_CNT.ToString();
            string SocketRecv = Operate.PacketConfig.Packet.Recv_CNT.ToString();
            string SocketSendTo = Operate.PacketConfig.Packet.SendTo_CNT.ToString();
            string SocketRecvFrom = Operate.PacketConfig.Packet.RecvFrom_CNT.ToString();
            string SocketWSASend = Operate.PacketConfig.Packet.WSASend_CNT.ToString();
            string SocketWSARecv = Operate.PacketConfig.Packet.WSARecv_CNT.ToString();
            string SocketWSASendTo = Operate.PacketConfig.Packet.WSASendTo_CNT.ToString();
            string SocketWSARecvFrom = Operate.PacketConfig.Packet.WSARecvFrom_CNT.ToString();
            string SocketBytes = Operate.PacketConfig.Packet.SpeedInfo;

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

        #region//获取系统日志

        [HttpGet]
        [Route("GetSocketLogList")]

        public IEnumerable<LogInfo> GetSocketLogList()
        {
            return Operate.LogConfig.List.lstLogInfo;
        }

        #endregion

        #region//获取代理日志

        [HttpGet]
        [Route("GetProxyLogList")]

        public IEnumerable<ProxyLogInfo> GetProxyLogList()
        {
            return Operate.LogConfig.List.lstProxyLogInfo;
        }

        #endregion
    }
}
