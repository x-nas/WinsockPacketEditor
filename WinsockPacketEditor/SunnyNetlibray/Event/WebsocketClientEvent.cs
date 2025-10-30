using SunnyNetlibray.Internal;
using SunnyNetlibray.Tools;
using System.Xml.Linq;
using System;

namespace SunnyNetlibray.Event
{
    public class WebsocketClientEvent
    {
        /// <summary>
        /// WebSocket 客户端 事件类型常量：客户端发送数据。
        /// </summary>
        public const int EventType_Send = 1;

        /// <summary>
        /// WebSocket 客户端 事件类型常量：客户端收到数据。
        /// </summary>
        public const int EventType_Receive = 2;

        /// <summary>
        /// WebSocket 客户端 事件类型常量：断开连接。
        /// </summary>
        public const int EventType_Close = 34;

        private long __ClientContext;
        private long __EventType;
        private EventValue __data;
        private int __WsMsgType;

        /// <summary>
        /// Ws客户端回调事件
        /// </summary>
        /// <param name="ClientContext"></param>
        /// <param name="EventType">1=接收消息 2=接收时连接被断开 3=发送时连接被断开</param>
        /// <param name="data">消息信息</param>
        /// <param name="WsMsgType">Const.WSClient_ (当消息类型=1时有效)</param> 

        public WebsocketClientEvent(IntPtr ClientContext, IntPtr EventType, byte[] data, IntPtr WsMsgType)
        {
            __ClientContext = ClientContext.ToInt64();
            __EventType = EventType.ToInt64();
            __data = new EventValue(data);
            __WsMsgType = (int)WsMsgType.ToInt64();
        }


        /// <summary>
        /// 获取 SunnyNet 上下文。
        /// </summary>
        /// <returns>上下文的长整型值。</returns>
        public long Context()
        {
            return __ClientContext;
        }


        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <remarks>
        /// 请使用以下常量来判断事件类型：
        /// <list type="bullet">
        ///   <item><see cref="EventType_Send"/> 发送数据</item>
        ///   <item><see cref="EventType_Receive"/> 接收数据</item>
        ///   <item><see cref="EventType_Close"/> 连接关闭</item>
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
        /// 获取事件数据内容。
        /// </summary>
        /// <returns>返回字节数组。</returns>
        public EventValue Body()
        {
            return __data;
        }

        /// <summary>
        /// 发送数据（字节数组）。
        /// <param>使用当前事件 的：
        /// <list type="bullet">
        ///   <item><see cref="MessageType"/></item>
        ///   <item>作为要发送数据的 MessageType</item>
        /// </list>
        /// </param>
        /// </summary>
        /// <param name="data">要发送的消息内容，作为字节数组。</param>
        /// <param name="MessageType">要发送的消息的类型，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="Const.WsMessageType.Text"/> 文本消息</item>
        ///   <item><see cref="Const.WsMessageType.Binary"/> 二进制消息</item>
        ///   <item><see cref="Const.WsMessageType.Ping"/> Ping 消息</item>
        ///   <item><see cref="Const.WsMessageType.Pong"/> Pong 消息</item>
        ///   <item><see cref="Const.WsMessageType.Close"/> 关闭消息</item>
        ///   <item><see cref="Const.WsMessageType.Invalid"/> 无效的消息</item>
        /// </list>
        /// </param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool Send(int MessageType, byte[] data)
        {
            return Bridge.WebsocketReadWrite(__ClientContext, data ?? new byte[0], MessageType);
        }

        /// <summary>
        /// 发送数据（字节数组）。
        /// <param>使用当前事件 的：
        /// <list type="bullet">
        ///   <item><see cref="MessageType"/></item>
        ///   <item>作为要发送数据的 MessageType</item>
        /// </list>
        /// </param>
        /// </summary>
        /// <param name="data">要发送的消息内容，作为字节数组。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool Send(byte[] data)
        {
            return Send(__WsMsgType, data ?? new byte[0]);
        }

        /// <summary>
        /// 发送数据。 
        /// </summary>
        /// <param name="MessageType">要发送的消息的类型，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="Const.WsMessageType.Text"/> 文本消息</item>
        ///   <item><see cref="Const.WsMessageType.Binary"/> 二进制消息</item>
        ///   <item><see cref="Const.WsMessageType.Ping"/> Ping 消息</item>
        ///   <item><see cref="Const.WsMessageType.Pong"/> Pong 消息</item>
        ///   <item><see cref="Const.WsMessageType.Close"/> 关闭消息</item>
        ///   <item><see cref="Const.WsMessageType.Invalid"/> 无效的消息</item>
        /// </list></param>
        /// <param name="data">新的事件数据，作为字符串。</param>
        /// <param name="Encoding">消息编码格式，默认为 "UTF-8"。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool Send(int MessageType, string data, string Encoding = "UTF-8")
        {
            return Send(MessageType, Tool.StrToBytes(data, Encoding));
        }

        /// <summary>
        /// 发送数据 。
        /// <param>使用当前事件 的：
        /// <list type="bullet">
        ///   <item><see cref="MessageType"/></item>
        ///   <item>作为要发送数据的 MessageType</item>
        /// </list>
        /// </param>
        /// </summary>
        /// <param name="data">新的事件数据，作为字符串。</param>
        /// <param name="Encoding">消息编码格式，默认为 "UTF-8"。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool Send(string data, string Encoding = "UTF-8")
        {
            return Send(Tool.StrToBytes(data, Encoding));
        }
        /// <summary>
        /// 断开连接
        /// </summary> 
        public void Close()
        {
            Bridge.WebsocketClose(__ClientContext);
        }
    }
}