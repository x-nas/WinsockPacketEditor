using System.Web.Http;

namespace WinsockPacketEditor
{
    [RoutePrefix("SystemInfo")]

    public class SystemInfo_Controller : ApiController
    {
        #region//获取服务器CPU和内存使用率

        [HttpGet]
        [Route("GetCPUAndMemory")]

        public IHttpActionResult GetCPUAndMemory()
        {
            return Ok(Operate.SystemConfig.GetCPUAndMemory());
        }

        #endregion

        #region//获取服务器启动时间

        [HttpGet]
        [Route("GetStartTime")]

        public IHttpActionResult GetStartTime()
        {  
            return Ok(Operate.SystemConfig.StartTime);
        }

        #endregion

        #region//获取系统运行模式名称

        [HttpGet]
        [Route("GetSelectMode")]

        public IHttpActionResult GetSelectMode()
        {
            string SelectMode = Operate.SystemConfig.GetSystemModeName();
            return Ok(SelectMode);
        }

        #endregion
    }
}
