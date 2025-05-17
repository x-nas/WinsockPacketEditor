using System.Web.Http;

namespace WPELibrary.Lib.WebAPI
{
    [RoutePrefix("api/SystemInfo")]

    public class SystemInfo_Controller : ApiController
    {
        #region//获取服务器CPU和内存使用率

        [HttpGet]
        [Route("GetCPUAndMemory")]

        public IHttpActionResult GetCPUAndMemory()
        {
            return Ok(Socket_Operation.GetCPUAndMemory());
        }

        #endregion

        #region//获取服务器启动时间

        [HttpGet]
        [Route("GetStartTime")]

        public IHttpActionResult GetStartTime()
        {  
            return Ok(Socket_Cache.System.StartTime);
        }

        #endregion

        #region//获取系统运行模式名称

        [HttpGet]
        [Route("GetSelectMode")]

        public IHttpActionResult GetSelectMode()
        {
            string SelectMode = Socket_Operation.GetSystemModeName();
            return Ok(SelectMode);
        }

        #endregion
    }
}
