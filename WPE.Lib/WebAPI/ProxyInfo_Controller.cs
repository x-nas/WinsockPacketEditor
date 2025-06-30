using System.Collections.Generic;
using System.Web.Http;

namespace WPE.Lib.WebAPI
{
    [RoutePrefix("ProxyInfo")]

    public class ProxyInfo_Controller : ApiController
    {
        #region//获取代理数据

        [HttpGet]
        [Route("GetProxyInfo")]

        public IHttpActionResult GetProxyInfo()
        {
            string WorkMode = Socket_Operation.GetWorkModeName(Operate.ProxyConfig.SocketProxy.SpeedMode);
            ulong ProxyTCP = Operate.ProxyConfig.SocketProxy.ProxyTCP_CNT;
            ulong ProxyUDP = Operate.ProxyConfig.SocketProxy.ProxyUDP_CNT;
            string ProxyCache = Operate.ProxyConfig.SocketProxyQueue.qSocket_ProxyData.Count.ToString();
            ulong ProxyTotal = ProxyTCP + ProxyUDP;
            string ProxyOnLine = Operate.ProxyConfig.SocketProxy.ProxyOnLineInfo;
            string ProxyLinks = Operate.ProxyConfig.SocketProxyList.lstProxyTCP.Count.ToString();
            string ProxySpeed = Operate.ProxyConfig.SocketProxy.ProxySpeedInfo;
            string ProxyBytes = Operate.ProxyConfig.SocketProxy.ProxyBytesInfo;

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

        public IEnumerable<Proxy_AuthInfo> GetProxyAuthList()
        {
            return Operate.ProxyConfig.ProxyAccount.lstProxyAuth;
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
