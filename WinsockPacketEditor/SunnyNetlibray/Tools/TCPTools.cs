using SunnyNetlibray.Internal;

namespace SunnyNetlibray.Tools
{
    public class TCPTools
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
        /// 发送消息到指定的目标。
        /// </summary>
        /// <param name="SendTarget">发送目标，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="SendToClient"/> 发送到客户端</item>
        ///   <item><see cref="SendToServer"/> 发送到服务器</item>
        /// </list>
        /// </param>
        /// <param name="theology">唯一标识符。</param>
        /// <param name="message">要发送的消息内容，作为字节数组。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public static bool SendMessage(int SendTarget, long theology, byte[] message)
        {
            if (SendTarget == SendToClient)
            {
                return Bridge.TcpSendMsgClient(theology, message);
            }
            return Bridge.TcpSendMsg(theology, message);
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
        /// <param name="theology">唯一标识符。</param>
        /// <param name="message">要发送的消息内容，作为字符串。</param>
        /// <param name="Encoding">消息编码格式，默认为 "UTF-8"。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public static bool SendMessage(int SendTarget, long theology, string message, string Encoding = "UTF-8")
        {
            return SendMessage(SendTarget, theology, Tool.StrToBytes(message, Encoding));
        }

        /// <summary>
        /// 关闭指定的 TCP 连接。
        /// </summary>
        /// <param name="theology">消息的唯一标识符，通常用于标识要关闭的连接。</param>
        /// <returns>如果连接成功关闭，则返回 true；否则返回 false。</returns>
        public static bool Close(long theology)
        {
            return Bridge.TcpCloseClient(theology);
        }
    }
}