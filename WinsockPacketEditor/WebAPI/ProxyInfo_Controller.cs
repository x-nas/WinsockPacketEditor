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

        public IHttpActionResult GetProxyInfo()
        {
            string WorkMode = Operate.SystemConfig.GetWorkModeName(Operate.ProxyConfig.Proxy.SpeedMode);
            long ProxyTCP = Operate.ProxyConfig.Proxy.TCP_Req_CNT + Operate.ProxyConfig.Proxy.TCP_Resp_CNT;
            long ProxyUDP = Operate.ProxyConfig.Proxy.UDP_Req_CNT + Operate.ProxyConfig.Proxy.UDP_Resp_CNT;
            string ProxyCache = Operate.ProxyConfig.Queue.qProxyInfo.Count.ToString();
            long ProxyTotal = ProxyTCP + ProxyUDP;
            string ProxyOnLine = Operate.ProxyConfig.Proxy.ProxyOnLineInfo;
            string ProxyLinks = Operate.ProxyConfig.Proxy.ProxyServer?.GetAllSessions().Count().ToString() ?? "0";
            string ProxySpeed = Operate.ProxyConfig.Proxy.ProxySpeedInfo;
            string ProxyBytes = Operate.ProxyConfig.Proxy.ProxyBytesInfo;

            string[] ProxyInfo = new string[9];
            ProxyInfo[0] = WorkMode;
            ProxyInfo[1] = ProxyTotal.ToString();
            ProxyInfo[2] = ProxyTCP.ToString();
            ProxyInfo[3] = ProxyUDP.ToString();
            ProxyInfo[4] = ProxyCache;
            ProxyInfo[5] = ProxyOnLine;
            ProxyInfo[6] = ProxyLinks;
            ProxyInfo[7] = ProxySpeed;
            ProxyInfo[8] = ProxyBytes;

            return Ok(ProxyInfo);
        }

        #endregion

        #region//获取认证日志

        [HttpGet]
        [Route("GetProxyAuthList")]

        public IEnumerable<AuthInfo> GetProxyAuthList()
        {
            return Operate.ProxyConfig.Account.cdAuthInfo.Values;
        }

        #endregion

        #region//获取代理日志

        [HttpGet]
        [Route("GetProxyLogList")]

        public IEnumerable<LogInfo> GetProxyLogList()
        {
            return Operate.LogConfig.List.lstLogInfo;
        }

        #endregion
    }
}
