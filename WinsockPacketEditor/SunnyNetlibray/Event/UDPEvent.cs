using SunnyNetlibray.Internal;
using SunnyNetlibray.Tools;
using System;

namespace SunnyNetlibray.Event
{
    public class UDPEvent
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
        /// UDP 事件类型常量：连接关闭。
        /// </summary>
        public const int EventType_UDP_Closed = 1;

        /// <summary>
        /// UDP 事件类型常量：客户端发送数据。
        /// </summary>
        public const int EventType_UDP_Send = 2;

        /// <summary>
        /// UDP 事件类型常量：客户端收到数据。
        /// </summary>
        public const int EventType_UDP_Receive = 3;

        private long __SunnyNetContext;
        private long __TheologyID;
        private long __MessageId;
        private long __EventType;
        private string __LocalAddr;
        private string __RemoteAddr;
        private long __pid;

        public UDPEvent(IntPtr SunnyNetContext, string LocalAddr, string RemoteAddr, IntPtr EventType, IntPtr MessageId, IntPtr TheologyID, IntPtr pid)
        {
            __SunnyNetContext = SunnyNetContext.ToInt64();
            __LocalAddr = LocalAddr;
            __RemoteAddr = RemoteAddr;
            __EventType = EventType.ToInt64();
            __MessageId = MessageId.ToInt64();
            __TheologyID = TheologyID.ToInt64();
            __pid = pid.ToInt64();
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
        /// 返回唯一 ID。
        /// </summary>
        /// <returns>事件的唯一标识符。</returns>
        public long TheologyID()
        {
            return __TheologyID;
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
        ///   <item><see cref="EventType_UDP_Send"/> 发送数据</item>
        ///   <item><see cref="EventType_UDP_Receive"/> 接收数据</item>
        ///   <item><see cref="EventType_UDP_Closed"/> 连接关闭</item>
        /// </list>
        /// </remarks>
        /// <returns>UDP 当前 事件类型。</returns>
        public int Type()
        {
            return (int)__EventType;
        }

        /// <summary>
        /// 获取本地地址。
        /// </summary>
        /// <returns>本地地址字符串。</returns>
        public string LocalAddr()
        {
            return __LocalAddr;
        }

        /// <summary>
        /// 获取远程地址。
        /// </summary>
        /// <returns>远程地址字符串。</returns>
        public string RemoteAddr()
        {
            return __RemoteAddr;
        }

        /// <summary>
        /// 获取事件数据内容。
        /// </summary>
        /// <returns>返回字节数组。</returns>
        public EventValue Body()
        {
            return new EventValue(Bridge.GetUdpData(__MessageId));
        }

        /// <summary>
        /// 修改当前事件的消息内容。
        /// </summary>
        /// <param name="data">新的消息内容，作为字节数组。</param>
        /// <returns>如果成功修改则返回 true；否则返回 false。</returns>
        public bool Body(byte[] data)
        {
            return Bridge.SetUdpData(__MessageId, data ?? new byte[0]);
        }
        /// <summary>
        /// 修改当前事件的消息内容。
        /// </summary>
        /// <param name="data">新的消息内容，作为字符串。</param>
        /// <returns>如果成功修改则返回 true；否则返回 false。</returns>
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
        /// <param name="message">要发送的消息内容，作为字节数组。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool SendMessage(int SendTarget, byte[] message)
        {
            return TCPTools.SendMessage(SendTarget, __TheologyID, message);
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
        /// <param name="message">要发送的消息内容，作为字符串。</param>
        /// <param name="Encoding">消息编码格式，默认为 "UTF-8"。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public bool SendMessage(int SendTarget, string message, string Encoding = "UTF-8")
        {
            return TCPTools.SendMessage(SendTarget, __TheologyID, message, Encoding);
        }
    }
}