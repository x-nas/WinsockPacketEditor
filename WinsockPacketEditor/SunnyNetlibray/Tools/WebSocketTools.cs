using SunnyNetlibray.Internal;

namespace SunnyNetlibray.Tools
{
    public class WebSocketTools
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
        public static bool SendMessage(int SendTarget, long theology, long wsMessageType, byte[] message)
        {
            if (SendTarget == SendToClient)
            {
                return Bridge.SendWebsocketClientBody(theology, wsMessageType, message);
            }
            return Bridge.SendWebsocketBody(theology, wsMessageType, message);
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
        /// <param name="message">要发送的消息内容，作为字符串。</param>
        /// <param name="Encoding">消息编码格式，默认为 "UTF-8"。</param>
        /// <returns>如果消息成功发送，则返回 true；否则返回 false。</returns>
        public static bool SendMessage(int SendTarget, long theology, long wsMessageType, string message, string Encoding = "UTF-8")
        {
            return SendMessage(SendTarget, theology, wsMessageType, Tool.StrToBytes(message, Encoding));
        }

        /// <summary>
        /// 关闭指定的 WebSocket 连接。
        /// </summary>
        /// <param name="theology">消息的唯一标识符，通常用于标识要关闭的连接。</param>
        /// <returns>如果连接成功关闭，则返回 true；否则返回 false。</returns>
        public static bool Close(long theology)
        {
            return Bridge.CloseWebsocket(theology);
        }
    }
}