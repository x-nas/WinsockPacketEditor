
using SunnyNetlibray.Event;

namespace SunnyNetlibray.Internal
{
    /// <summary>
    /// SunnyNet 回调事件接口 你只需要实现这些接口就行
    /// </summary>
    /// <param name="Conn">HTTP 事件对象。</param>
    public interface SunnyNet
    {
        /// <summary>
        /// HTTP 请求的回调方法。
        /// </summary>
        /// <param name="Conn">HTTP 事件对象。</param>
        void OnHttpCallback(HTTPEvent Conn);

        /// <summary>
        /// WebSocket 请求的回调方法。
        /// </summary>
        /// <param name="Conn">WebSocket 事件对象。</param>
        void OnWebSocketCallback(WebSocketEvent Conn);

        /// <summary>
        /// TCP 请求的回调方法。
        /// </summary>
        /// <param name="Conn">TCP 事件对象。</param>
        void OnTcpCallback(TCPEvent Conn);

        /// <summary>
        /// UDP 请求的回调方法。
        /// </summary>
        /// <param name="Conn">UDP 事件对象。</param>
        void OnUdpCallback(UDPEvent Conn);

        /// <summary>
        /// 脚本日志输出回调函数。
        /// </summary>
        /// <param name="logInfo">脚本日志输出信息。</param>
        void OnScriptLogCallback(long SunnyNetContext, EventValue logInfo);

        /// <summary>
        /// 脚本代码保存回调函数。
        /// </summary>
        /// <param name="scriptCode">保存的脚本代码。</param>
        void OnScriptCodeSaveCallback(long SunnyNetContext, EventValue scriptCode);
    }

    /// <summary>
    /// 表示一个字节数组的事件值类。
    /// </summary>
    public class EventValue
    {
        /// <summary>
        /// 存储字节数组的字段。
        /// </summary>
        public byte[] Bytes;

        /// <summary>
        /// 初始化 <see cref="EventValue"/> 类的新实例，使用指定的字节数组。
        /// </summary>
        /// <param name="data">要存储的字节数组。</param>
        public EventValue(byte[] data)
        {
            Bytes = data; // 将传入的字节数组赋值给字段 Bytes
        }

        /// <summary>
        /// 获取字节数组的长度。
        /// </summary>
        public int Length => Bytes.Length; // 返回字节数组的长度

        /// <summary>
        /// 获取或设置指定索引处的字节值。
        /// </summary>
        /// <param name="index">字节数组中的索引。</param>
        /// <returns>该索引处的字节值。</returns>
        public byte this[int index]
        {
            get => Bytes[index]; // 获取指定索引的字节值
            set => Bytes[index] = value; // 设置指定索引的字节值
        }

        /// <summary>
        /// 将字节数组转换为字符串，使用指定的编码。
        /// </summary>
        /// <param name="Encoding">要使用的字符串编码，默认为 "UTF-8"。</param>
        /// <returns>转换后的字符串。</returns>
        public string String(string Encoding = "UTF-8")
        {
            return Tool.BytesToStr(Bytes, Encoding); // 调用工具方法将字节数组转换为字符串
        }

        /// <summary>
        /// 返回当前 <see cref="EventValue"/> 实例的字符串表示形式。
        /// </summary>
        /// <returns>当前实例的字符串表示。</returns>
        override
        public string ToString()
        {
            return String(); // 返回调用 String 方法的结果
        }
    }
}
