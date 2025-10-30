using SunnyNetlibray.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunnyNetlibray.Internal
{
    public interface WebsocketClient
    {
        /// <summary>
        /// Ws客户端回调事件
        /// </summary> 
        void OnCallback(WebsocketClientEvent Conn);
        /// <summary>
        /// Ws客户端心跳回调事件，需手动调用设置函数后生效
        /// </summary> 
        void OnHeartbeatCallback(long Context);
    }
}
