using SunnyNetlibray.Internal;
using SunnyNetlibray.Tools;
using System;

namespace SunnyNetlibray.Event
{
    public class TCPEvent
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
        /// TCP 事件类型常量：即将开始连接。
        /// </summary>
        public const int EventType_TCP_About = 4;

        /// <summary>
        /// TCP 事件类型常量：连接成功。
        /// </summary>
        public const int EventType_TCP_OK = 0;

        /// <summary>
        /// TCP 事件类型常量：客户端发送数据。
        /// </summary>
        public const int EventType_TCP_Send = 1;

        /// <summary>
        /// TCP 事件类型常量：客户端收到数据。
        /// </summary>
        public const int EventType_TCP_Receive = 2;

        /// <summary>
        /// TCP 事件类型常量：连接关闭或连接失败。
        /// </summary>
        public const int EventType_TCP_Close = 3;

        private long __SunnyNetContext;
        private long __TheologyID;
        private long __MessageId;
        private long __EventType;
        private string __LocalAddr;
        private string __RemoteAddr;
        private EventValue __MessageData;
        private long __pid;

        public TCPEvent(IntPtr SunnyNetContext, string LocalAddr, string RemoteAddr, IntPtr EventType, IntPtr MessageId, byte[] data, IntPtr TheologyID, IntPtr pid)
        {
            __SunnyNetContext = SunnyNetContext.ToInt64();
            __LocalAddr = LocalAddr;
            __RemoteAddr = RemoteAddr;
            __EventType = EventType.ToInt64();
            __MessageId = MessageId.ToInt64();
            __TheologyID = TheologyID.ToInt64();
            __MessageData = new EventValue(data);
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
        /// <para>设置出口IP</para>
        /// <para>你也可以在中间件中设置全局出口IP</para>
        /// </summary>
        /// <param name="ip">请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择</param>
        public void SetOutRouterIP(String ip)
        {
            Bridge.RequestSetOutRouterIP(__MessageId, ip);
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
        ///   <item><see cref="EventType_TCP_About"/> 即将连接</item>
        ///   <item><see cref="EventType_TCP_OK"/> 连接成功</item>
        ///   <item><see cref="EventType_TCP_Send"/> 发送数据</item>
        ///   <item><see cref="EventType_TCP_Receive"/> 接收数据</item>
        ///   <item><see cref="EventType_TCP_Close"/> 连接关闭</item>
        /// </list>
        /// </remarks>
        /// <returns>TCP 当前 事件类型。</returns>
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
            return __MessageData;
        }

        /// <summary>
        /// 修改当前事件的消息内容。
        /// </summary>
        /// <param name="data">新的消息内容，作为字节数组。</param>
        /// <returns>如果成功修改则返回 true；否则返回 false。</returns>
        public bool Body(byte[] data)
        {
            __MessageData = new EventValue(data);
            return Bridge.SetTcpBody(__MessageId, (int)__EventType, __MessageData.Bytes);
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
        /// 设置代理，例如，以下示例格式：
        /// <list type="bullet">
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </summary>
        /// <param name="ProxyUrl">代理 URL，指定要使用的代理地址。
        /// 例如，以下示例格式：
        /// <list type="bullet">
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </param>
        /// <param name="outTime">代理超时(毫秒)，默认30秒</param>
        /// <returns>如果成功设置代理，返回 true；否则返回 false。</returns>
        public bool SetProxy(string ProxyUrl, int outTime = 30000)
        {
            if (__EventType != EventType_TCP_About)
            {
                return false;
            }
            return Bridge.SetTcpAgent(__MessageId, ProxyUrl, outTime);
        }

        /// <summary>
        /// 重定向到另一个地址，仅限 TCP 回调，即将连接时使用。
        /// </summary>
        /// <param name="newAddress">请提供带端口的 IP 地址，例如: <c>8.8.8.8:443</c></param>
        /// <returns>如果事件类型不是连接即将开始，则返回 false；否则返回 true。</returns>
        public bool Redirect(string newAddress)
        {
            if (__EventType != EventType_TCP_About)
            {
                return false;
            }
            return Bridge.SetTcpConnectionIP(__MessageId, newAddress);
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

        /// <summary>
        /// 关闭当前 TCP 会话。
        /// </summary>
        /// <returns>如果成功则返回 true；否则返回 false。</returns>
        public bool Close()
        {
            return TCPTools.Close(__TheologyID);
        }
    }
}