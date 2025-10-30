using SunnyNetlibray.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunnyNetlibray.Internal
{
    public interface RedisClient
    {
        /// <summary>
        /// Redis客户端回调事件
        /// </summary> 
        void OnCallback(long Context, string message);
    }
}
