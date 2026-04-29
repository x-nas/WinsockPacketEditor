using System.Web.Http;

namespace WinsockPacketEditor
{
    [RoutePrefix("ProxyCap")]

    public class ProxyCap_Controller : ApiController
    {
        #region//获取服务器列表

        [HttpGet]
        [Route("GetServerList")]

        public IHttpActionResult GetServerList()
        {
            return Ok(Operate.WPCConfig.ServerList.GetEnabledServers());
        }

        #endregion        

        #region//获取公告列表

        [HttpGet]
        [Route("GetNoticeList")]
        public IHttpActionResult GetNoticeList()
        {
            return Ok(Operate.WPCConfig.NoticeList.lstNoticeInfo);
        }

        #endregion
    }
}
