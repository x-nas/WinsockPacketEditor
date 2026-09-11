using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WinsockPacketEditor
{
    [RoutePrefix("ProxyInfo")]

    public class ProxyInfo_Controller : ApiController
    {
        #region//获取代理数据

        [HttpGet]
        [Route("GetProxyInfo")]

        /// <summary>
        /// 代理模式的统计。⚠️ 具名对象，理由同 SocketInfo_Controller.GetSocketInfo。
        /// </summary>
        public IHttpActionResult GetProxyInfo()
        {
            long tcp = Operate.ProxyConfig.Proxy.TCP_Req_CNT + Operate.ProxyConfig.Proxy.TCP_Resp_CNT;
            long udp = Operate.ProxyConfig.Proxy.UDP_Req_CNT + Operate.ProxyConfig.Proxy.UDP_Resp_CNT;

            return Ok(new
            {
                WorkMode = Operate.SystemConfig.GetWorkModeName(),
                Total = tcp + udp,
                TCP = tcp,
                UDP = udp,
                Queue = Operate.ProxyConfig.Queue.qProxyInfo.Count,
                Links = Operate.ProxyConfig.Proxy.ProxyServer?.GetAllSessions().Count() ?? 0,
                //下面三个已经是拼好给人看的串，前端原样显示
                OnLine = Operate.ProxyConfig.Proxy.ProxyOnLineInfo,
                Speed = Operate.ProxyConfig.Proxy.ProxySpeedInfo,
                Bytes = Operate.ProxyConfig.Proxy.ProxyBytesInfo,
            });
        }

        #endregion

        #region//获取认证日志

        [HttpGet]
        [Route("GetProxyAuthList")]

        public IEnumerable<AuthInfo> GetProxyAuthList(int take = 0)
        {
            return SocketInfo_Controller.Tail(Operate.ProxyConfig.Account.lstAuthInfo, take);
        }

        #endregion

        #region//获取代理日志

        /// <summary>
        /// ⚠️ <b>名不副实，保留只为兼容。</b>路由叫 GetProxyLogList，返回的却是
        /// lstLogInfo（<b>系统</b>日志）—— 与 SocketInfo/GetSocketLogList 是同一份数据。
        /// 真正的代理日志（lstProxyLogInfo）在 SocketInfo/GetProxyLogList。
        ///
        /// 旧版管理台的「代理日志」页签调的正是这一个，所以那个页签一直显示的是系统日志；
        /// 2026-09-09 重做的页面已改接 SocketInfo 那个。这里没有删，是怕有别的调用方。
        /// </summary>
        [HttpGet]
        [Route("GetProxyLogList")]

        public IEnumerable<LogInfo> GetProxyLogList(int take = 0)
        {
            return SocketInfo_Controller.Tail(Operate.LogConfig.List.lstLogInfo, take);
        }

        #endregion
    }
}
