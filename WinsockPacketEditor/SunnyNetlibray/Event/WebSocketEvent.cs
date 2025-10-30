using SunnyNetlibray.Internal;
using SunnyNetlibray.Tools;
using System;

namespace SunnyNetlibray.Event
{
    public class WebSocketEvent
    {
        /// <summary>
        /// 消息发送目标常量：发送到客户端。
        /// </summary>
        public const int SendToClient = 1;

        /// <summary>
        /// 消息发送目标常量：发送到服务器。
        /// </summary>
        public const int SendToServer = 2;

        /// <summary>
        /// WebSocket 事件类型常量：连接成功。
        /// </summary>
        public const int EventType_Websocket_OK = 1;

        /// <summary>
        /// WebSocket 事件类型常量：客户端发送数据。
        /// </summary>
        public const int EventType_Websocket_Send = 2;

        /// <summary>
        /// WebSocket 事件类型常量：客户端收到数据。
        /// </summary>
        public const int EventType_Websocket_Receive = 3;

        /// <summary>
        /// WebSocket 事件类型常量：断开连接。
        /// </summary>
        public const int EventType_Websocket_Close = 4;

        private long __SunnyNetContext;
        private long __TheologyID;
        private long __MessageId;
        private long __EventType;
        private string __Method;
        private string __Url;
        private long __pid;
        private int __WsMsgType;
        private Request __Request;

        public WebSocketEvent(IntPtr SunnyNetContext, IntPtr TheologyID, IntPtr MessageId, IntPtr EventType, string Method, string Url, IntPtr pid, IntPtr WsMsgType)
        {
            __SunnyNetContext = SunnyNetContext.ToInt64();
            __TheologyID = TheologyID.ToInt64();
            __MessageId = MessageId.ToInt64();
            __EventType = EventType.ToInt64();
            __Method = Method;
            __Url = Url;
            __pid = pid.ToInt64();
            __WsMsgType = (int)WsMsgType.ToInt64();
            __Request = new Request(__MessageId);
        }

        /// <summary>
        /// 获取请求的 URL。
        /// </summary>
        /// <returns>请求的 URL。</returns>
        public string URL()
        {
            return __Url;
        }

        /// <summary>
        /// 获取请求方法。
        /// </summary>
        /// <returns>HTTP 方法。</returns>
        public string Method()
        {
            return __Method;
        }

        /// <summary>
        /// 返回唯一 ID。
        /// </summary>
        /// <returns>事件的唯一标识符。</returns>
        public long TheologyID()
        {
            return __TheologyID;
        }

        /// <summary>
        /// 获取 SunnyNet 上下文。
        /// </summary>
        /// <returns>上下文的长整型值。</returns>
        public long Context()
        {
            return __SunnyNetContext;
        }

        /// <summary>
        /// 如果开启了身份验证模式，将返回客户端使用的 S5 账号。
        /// </summary>
        /// <returns>客户端的 S5 账号。</returns>
        public string GetUser()
        {
            return Bridge.SunnyNetGetSocket5User(__TheologyID);
        }

        /// <summary>
        /// 返回事件由哪个进程发起。如果返回 0，表示远程设备通过代理连接。
        /// </summary>
        /// <returns>进程 ID。</returns>
        public int PID()
        {
            return (int)__pid;
        }

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <remarks>
        /// 请使用以下常量来判断事件类型：
        /// <list type="bullet">
        ///   <item><see cref="EventType_Websocket_OK"/> 连接成功</item>
        ///   <item><see cref="EventType_Websocket_Send"/> 发送数据</item>
        ///   <item><see cref="EventType_Websocket_Receive"/> 接收数据</item>
        ///   <item><see cref="EventType_Websocket_Close"/> 连接关闭</item>
        /// </list>
        /// </remarks>
        /// <returns>WebSocket 当前事件类型。</returns>
        public int Type()
        {
            return (int)__EventType;
        }

        /// <summary>
        /// 获取当前 WebSocket 消息的类型。
        /// <param>消息类型，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="Const.WsMessageType.Text"/> 文本消息</item>
        ///   <item><see cref="Const.WsMessageType.Binary"/> 二进制消息</item>
        ///   <item><see cref="Const.WsMessageType.Ping"/> Ping 消息</item>
        ///   <item><see cref="Const.WsMessageType.Pong"/> Pong 消息</item>
        ///   <item><see cref="Const.WsMessageType.Close"/> 关闭消息</item>
        ///   <item><see cref="Const.WsMessageType.Invalid"/> 无效的消息</item>
        /// </list></param>
        /// </summary>
        /// <returns>当前事件消息的类型。</returns>
        public long MessageType()
        {
            return __WsMsgType;
        }

        /// <summary>
        /// 获取 WebSocket 连接时的 Headers 信息。
        /// </summary>
        /// <returns>Headers 信息字符串。</returns>
        public string Headers()
        {
            return __Request.GetAllHeader();
        }
        /// <summary>
        /// 获取指定协议头。
        /// 如果有多个同名协议头，将返回第一个。
        /// </summary>
        /// <param name="key">协议头名称。</param>
        /// <returns>返回指定名称的协议头值。</returns>
        public string Header(string key)
        {
            return __Request.GetHeader(key);
        }
        /// <summary>
        /// 获取全部 Cookies。
        /// </summary>
        /// <returns>返回所有 提交的 Cookies  字符串。</returns>
        public string Cookies()
        {
            return __Request.GetCookies();
        }
        /// <summary>
        /// 获取指定 Cookie。
        /// </summary>
        /// <param name="key">Cookie 名。</param>
        /// <returns>返回指定 Cookie 的字符串。</returns>
        public string Cookie(string key)
        {
            return __Request.GetCookie(key);
        }

        /// <summary>
        /// 获取指定 Cookie，不包含键名。
        /// </summary>
        /// <param name="key">Cookie 名。</param>
        /// <returns>返回指定 Cookie 的值。</returns>
        public string Cookie_value(string key)
        {
            return __Request.GetCookie_value(key);
        }
        /// <summary>
        /// 获取事件数据内容。
        /// </summary>
        /// <returns>返回字节数组。</returns>
        public EventValue Body()
        {
            return new EventValue(Bridge.GetWebsocketBody(__MessageId));
        }

        /// <summary>
        /// 修改事件数据内容。
        /// </summary>
        /// <param name="data">新的事件数据，作为字节数组。</param>
        /// <returns>成功返回 true；否则返回 false。</returns>
        public bool Body(byte[] data)
        {
            return Bridge.SetWebsocketBody(__MessageId, data ?? new byte[0]);
        }

        /// <summary>
        /// 修改事件数据内容。
        /// </summary>
        /// <param name="data">新的事件数据，作为字符串。</param>
        /// <param name="Encoding">消息编码格式，默认为 "UTF-8"。</param>
        /// <returns>成功返回 true；否则返回 false。</returns>
        public bool Body(string data, string Encoding = "UTF-8")
        {
            return Body(Tool.StrToBytes(data, Encoding));
        }

        /// <summary>
        /// 发送消息到指定的目标。
        /// </summary>
        /// <param name="SendTarget">发送目标，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="SendToClient"/> 发送到客户端</item>
        ///   <item><see cref="SendToServer"/> 发送到服务器</item>
        /// </list>
        /// </param>
        /// <param name="theology">消息的唯一标识符，通常用于标识目标连接。</param>
        /// <param name="wsMessageType">消息类型，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="Const.WsMessageType.Text"/> 文本消息</item>
        ///   <item><see cref="Const.WsMessageType.Binary"/> 二进制消息</item>
        ///   <item><see cref="Const.WsMessageType.Ping"/> Ping 消息</item>
        ///   <item><see cref="Const.WsMessageType.Pong"/> Pong 消息</item>
        ///   <item><see cref="Const.WsMessageType.Close"/> 关闭消息</item>
        ///   <item><see cref="Const.WsMessageType.Invalid"/> 无效的消息</item>
        /// </list>
        /// </param>
        /// <param name="message">要发送的消息内容，作为字节数组。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool SendMessage(int SendTarget, long wsMessageType, byte[] message)
        {
            return WebSocketTools.SendMessage(SendTarget, __TheologyID, wsMessageType, message);
        }

        /// <summary>
        /// 发送消息到指定的目标（字节数组）。
        /// </summary>
        /// <param name="SendTarget">发送目标，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="SendToClient"/> 发送到客户端</item>
        ///   <item><see cref="SendToServer"/> 发送到服务器</item>
        /// </list>
        /// </param>
        /// <param name="message">要发送的消息内容，作为字节数组。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool SendMessage(int SendTarget, byte[] message)
        {
            return SendMessage(SendTarget, __WsMsgType, message);
        }

        /// <summary>
        /// 发送消息到指定的目标（字符串格式）。
        /// </summary>
        /// <param name="SendTarget">发送目标，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="SendToClient"/> 发送到客户端</item>
        ///   <item><see cref="SendToServer"/> 发送到服务器</item>
        /// </list>
        /// </param>
        /// <param name="wsMessageType">消息类型，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="Const.WsMessageType.Text"/> 文本消息</item>
        ///   <item><see cref="Const.WsMessageType.Binary"/> 二进制消息</item>
        ///   <item><see cref="Const.WsMessageType.Ping"/> Ping 消息</item>
        ///   <item><see cref="Const.WsMessageType.Pong"/> Pong 消息</item>
        ///   <item><see cref="Const.WsMessageType.Close"/> 关闭消息</item>
        ///   <item><see cref="Const.WsMessageType.Invalid"/> 无效的消息</item>
        /// </list>
        /// </param>
        /// <param name="message">要发送的消息内容，作为字符串。</param>
        /// <param name="Encoding">消息编码格式，默认为 "UTF-8"。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool SendMessage(int SendTarget, long wsMessageType, string message, string Encoding = "UTF-8")
        {
            return SendMessage(SendTarget, wsMessageType, Tool.StrToBytes(message, Encoding));
        }

        /// <summary>
        /// 发送消息到指定的目标（字符串格式）。
        /// </summary>
        /// <param name="SendTarget">发送目标，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="SendToClient"/> 发送到客户端</item>
        ///   <item><see cref="SendToServer"/> 发送到服务器</item>
        /// </list>
        /// </param>
        /// <param name="message">要发送的消息内容，作为字符串。</param>
        /// <param name="Encoding">消息编码格式，默认为 "UTF-8"。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool SendMessage(int SendTarget, string message, string Encoding = "UTF-8")
        {
            return SendMessage(SendTarget, Tool.StrToBytes(message, Encoding));
        }

        /// <summary>
        /// 关闭当前 WebSocket 连接。
        /// </summary>
        /// <returns>如果连接成功关闭，则返回 true；否则返回 false。</returns>
        public bool Close()
        {
            return WebSocketTools.Close(__TheologyID);
        }
    }
}