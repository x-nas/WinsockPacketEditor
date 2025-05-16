using System.Collections.Generic;
using System.Web.Http;

namespace WPELibrary.Lib.WebAPI
{
    [RoutePrefix("api/ProxyInfo")]

    public class ProxyInfo_Controller : ApiController
    {
        #region//获取代理数据

        [HttpGet]
        [Route("GetProxyInfo")]

        public IHttpActionResult GetProxyInfo()
        {
            string WorkMode = Socket_Operation.GetWorkModeName(Socket_Cache.SocketProxy.SpeedMode);
            ulong ProxyTCP = Socket_Cache.SocketProxy.ProxyTCP_CNT;
            ulong ProxyUDP = Socket_Cache.SocketProxy.ProxyUDP_CNT;
            string ProxyCache = Socket_Cache.SocketProxyQueue.qSocket_ProxyData.Count.ToString();
            ulong ProxyTotal = ProxyTCP + ProxyUDP;
            string ProxyOnLine = Socket_Cache.SocketProxy.ProxyOnLineInfo;
            string ProxyLinks = Socket_Cache.SocketProxyList.lstProxyTCP.Count.ToString();
            string ProxySpeed = Socket_Cache.SocketProxy.ProxySpeedInfo;
            string ProxyBytes = Socket_Cache.SocketProxy.ProxyBytesInfo;

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
            return Socket_Cache.SocketProxy.lstProxyAuth;
        }

        #endregion

        #region//获取代理日志

        [HttpGet]
        [Route("GetProxyLogList")]

        public IEnumerable<Socket_LogInfo> GetProxyLogList()
        {
            return Socket_Cache.LogList.lstProxyLog;
        }

        #endregion
    }
}
