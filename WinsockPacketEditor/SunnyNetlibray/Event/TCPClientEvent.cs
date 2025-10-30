using System;
using SunnyNetlibray.Internal;
using SunnyNetlibray.Tools;
using System.Xml.Linq;

namespace SunnyNetlibray.Event
{
    public class TCPClientEvent
    {


        /// <summary>
        /// TCP 客户端 事件类型常量：客户端收到数据。
        /// </summary>
        public const int EventType_Receive = 1;

        /// <summary>
        /// TCP 客户端 事件类型常量：断开连接。
        /// </summary>
        public const int EventType_Close = 2;

        private long __ClientContext;
        private long __EventType;
        private EventValue __data;

        /// <summary>
        /// Ws客户端回调事件
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="消息类型">1=接收消息 2=接收时连接被断开 3=发送时连接被断开</param>
        /// <param name="数据指针">消息类型=2、3时 这里是错误信息</param>
        /// <param name="指针长度"></param>  

        public TCPClientEvent(IntPtr ClientContext, IntPtr EventType, byte[] data)
        {
            __ClientContext = ClientContext.ToInt64();
            __EventType = EventType.ToInt64();
            __data = new EventValue(data);
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
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool Send(byte[] data)
        {
            return Bridge.SocketClientWrite(__ClientContext, 60000, data ?? new byte[0]) > 0;
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
            Bridge.SocketClientClose(__ClientContext);
        }
    }
}