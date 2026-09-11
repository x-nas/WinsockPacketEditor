using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WinsockPacketEditor
{
    [RoutePrefix("SocketInfo")]

    public class SocketInfo_Controller : ApiController
    {
        #region//获取封包信息

        [HttpGet]
        [Route("GetSocketInfo")]

        /// <summary>
        /// 注入模式的统计。
        ///
        /// ⚠️ <b>返回具名对象，不要改回位置数组。</b>原来回的是 string[15]，
        /// 前端只能按下标绑（d[13]）—— 往中间插一个字段，后面每一格都会静默错位，
        /// 页面上是一排对不上的数字而不报任何错。加字段直接加属性即可。
        /// </summary>
        public IHttpActionResult GetSocketInfo()
        {
            return Ok(new
            {
                Process = Operate.PacketConfig.Packet.InjectProcess,
                WorkMode = Operate.SystemConfig.GetWorkModeName(),
                Total = Operate.PacketConfig.Packet.TotalPackets,
                FilterExecute = Operate.FilterConfig.Filter.FilterExecute_CNT,
                Queue = Operate.PacketConfig.Queue.cqPacketInfo.Count,
                FilterHit = Operate.PacketConfig.Packet.FilterPacket_CNT,
                Send = Operate.PacketConfig.Packet.Send_CNT,
                Recv = Operate.PacketConfig.Packet.Recv_CNT,
                SendTo = Operate.PacketConfig.Packet.SendTo_CNT,
                RecvFrom = Operate.PacketConfig.Packet.RecvFrom_CNT,
                WSASend = Operate.PacketConfig.Packet.WSASend_CNT,
                WSARecv = Operate.PacketConfig.Packet.WSARecv_CNT,
                WSASendTo = Operate.PacketConfig.Packet.WSASendTo_CNT,
                WSARecvFrom = Operate.PacketConfig.Packet.WSARecvFrom_CNT,
                //已经是拼好给人看的串（"发送 : x  接收 : y"），前端原样显示
                Speed = Operate.PacketConfig.Packet.SpeedInfo,
            });
        }

        #endregion

        /// <summary>
        /// 取列表<b>末尾</b>的 take 条（也就是最近的那些）。take &lt;= 0 返回整份。
        ///
        /// 【为什么要有它】这几个接口原来一律返回整份列表（日志默认上限 5000 条）。
        /// 一条 LogInfo 序列化出来约 152 字节，5000 条就是 742KB；
        /// 管理台 3 秒一拍，挂一小时约 870MB，而页面只画最近 300 条 ——
        /// 传过去的九成多当场丢掉。手机用流量看日志时这一笔很贵。
        ///
        /// ⚠️ 默认值刻意是 0（整份），<b>不改变任何已有调用方的行为</b>；
        /// 省流量要调用方自己传 ?take=300。
        ///
        /// ⚠️ 取的是<b>末尾</b>不是开头：这几份列表都是追加序，末尾才是最新的。
        /// </summary>
        internal static IEnumerable<T> Tail<T>(IList<T> list, int take)
        {
            if (list == null) { return new List<T>(); }
            if (take <= 0 || take >= list.Count) { return list; }

            //ToList 是必要的：直接返回 Skip 的惰性序列，序列化时列表可能已经被
            //搬运拍改过了，会抛「集合已修改」
            return list.Skip(list.Count - take).ToList();
        }

        #region//获取系统日志

        [HttpGet]
        [Route("GetSocketLogList")]

        public IEnumerable<LogInfo> GetSocketLogList(int take = 0)
        {
            return Tail(Operate.LogConfig.List.lstLogInfo, take);
        }

        #endregion

        #region//获取代理日志

        [HttpGet]
        [Route("GetProxyLogList")]

        public IEnumerable<ProxyLogInfo> GetProxyLogList(int take = 0)
        {
            return Tail(Operate.LogConfig.List.lstProxyLogInfo, take);
        }

        #endregion
    }
}
