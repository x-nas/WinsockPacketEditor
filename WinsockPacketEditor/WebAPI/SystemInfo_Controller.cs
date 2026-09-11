using System.Web.Http;

namespace WinsockPacketEditor
{
    [RoutePrefix("SystemInfo")]

    public class SystemInfo_Controller : ApiController
    {
        #region//获取服务器CPU和内存使用率

        [HttpGet]
        [Route("GetCPUAndMemory")]

        /// <summary>
        /// CPU / 内存占用。⚠️ 具名对象，理由同 SocketInfo_Controller.GetSocketInfo。
        /// Operate.GetCPUAndMemory() 出的仍是 string[2]（WinForms 那边在用），这里只换个壳。
        /// </summary>
        public IHttpActionResult GetCPUAndMemory()
        {
            string[] v = Operate.SystemConfig.GetCPUAndMemory();

            return Ok(new
            {
                //形如 "12.34%" / "61.4%"，带百分号
                Cpu = v != null && v.Length > 0 ? v[0] : null,
                Memory = v != null && v.Length > 1 ? v[1] : null,
            });
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
