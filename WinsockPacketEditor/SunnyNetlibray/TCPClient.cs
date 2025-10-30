using SunnyNetlibray.Event;
using SunnyNetlibray.Internal;
using System.Runtime.InteropServices;
using System;

using SunnyNetTCPClient = SunnyNetlibray.Internal.TCPClient;

namespace SunnyNetlibray
{
    /// <summary>
    /// TCP 客户端类，参考易语言用法。
    /// </summary>
    public class TCPClient
    {
        private long context = 0; // 客户端标识 
        private TcpClientDefaultCallback funcCall = null;
        private SunnyNetTCPClient _callback = null;

        /// <summary>
        /// 初始化 TCPClient 实例并创建 TCP Socket 客户端。
        /// </summary>
        public TCPClient()
        {
            context = Bridge.CreateSocketClient();
        }

        /// <summary>
        /// 释放资源，自动调用。
        /// </summary>
        ~TCPClient()
        {
            Bridge.RemoveSocketClient(context);
        }

        /// <summary>
        /// 重新创建 TCP Socket 客户端。
        /// </summary>
        public void Recreate()
        {
            Bridge.RemoveSocketClient(context);
            context = Bridge.CreateSocketClient();
        }

        /// <summary>
        /// 获取当前客户端的唯一标识。
        /// </summary>
        /// <returns>客户端标识</returns>
        public long GetClientId()
        {
            return context;
        }

        /// <summary>
        /// 连接 TCP 客户端。
        /// </summary>
        /// <param name="ip">要连接的 IP 地址</param>
        /// <param name="certManager">证书管理器（可选）如果不为 null 表示 TlsClient</param>
        /// <param name="callback">回调函数，处理 TCP 事件 如果为null,表示同步模式</param>
        /// <param name="RouterIP">出口IP[请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择]</param> 
        /// <param name="isTlsClient">是否为 TLS 客户端（默认 false）</param> 
        /// <param name="ProxyUrl">代理 URL，指定要使用的代理地址。
        /// 例如，以下示例格式：
        ///  <list type="bullet"> 
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </param>
        /// <returns>连接是否成功</returns>
        public bool Connect(string ip, SunnyNetTCPClient callback, CertManager certManager = null, string ProxyUrl = "", int timeOut = 30000, string RouterIP = "")
        {
            _callback = callback;
            funcCall = new TcpClientDefaultCallback(DefaultTcpClientCallback);
            bool isSynchronous = false;
            if (callback == null)
            {
                isSynchronous = true;
            }
            bool isTlsClient = true;
            if (certManager == null)
            {
                isTlsClient = true;
            }
            return Bridge.SocketClientDial(context, ip, Marshal.GetFunctionPointerForDelegate(funcCall), isTlsClient, isSynchronous, ProxyUrl, certManager, timeOut, RouterIP);
        }

        /// <summary>
        /// 连接 TCP 客户端。同步模式
        /// </summary>
        /// <param name="ip">要连接的 IP 地址</param>
        /// <param name="timeOut">连接超时</param> 
        /// <param name="RouterIP">出口IP[请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择]</param> 
        /// <param name="certManager">证书管理器 如果不为 null 表示 TlsClient </param> 
        /// <param name="ProxyUrl">代理 URL，指定要使用的代理地址。
        /// 例如，以下示例格式：
        ///  <list type="bullet"> 
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </param>
        /// <returns>连接是否成功</returns>
        public bool Connect(string ip, CertManager certManager = null, string ProxyUrl = "", int timeOut = 30000, string RouterIP = "")
        {
            return Connect(ip, null, certManager, ProxyUrl, timeOut, RouterIP);
        }

        /// <summary>
        /// 连接 TCP 客户端。同步模式
        /// </summary>
        /// <param name="ip">要连接的 IP 地址</param> 
        /// <param name="timeOut">连接超时</param> 
        /// <param name="RouterIP">出口IP[请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择]</param> 
        /// <param name="ProxyUrl">代理 URL，指定要使用的代理地址。
        /// 例如，以下示例格式：
        ///  <list type="bullet"> 
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </param>
        /// <returns>连接是否成功</returns>
        public bool Connect(string ip, string ProxyUrl, int timeOut = 30000, string RouterIP = "")
        {
            return Connect(ip, null, ProxyUrl, timeOut, RouterIP);
        }


        /// <summary>
        /// 连接 TCP 客户端。同步模式
        /// </summary>
        /// <param name="ip">要连接的 IP 地址</param>  
        /// <param name="RouterIP">出口IP[请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择]</param> 
        /// <param name="ProxyUrl">代理 URL，指定要使用的代理地址。
        /// 例如，以下示例格式：
        ///  <list type="bullet"> 
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </param>
        /// <returns>连接是否成功</returns>
        public bool Connect(string ip, string ProxyUrl, string RouterIP = "")
        {
            return Connect(ip, null, ProxyUrl, 30000, RouterIP);
        }


        /// <summary>
        /// 断开 TCP 客户端连接。
        /// </summary>
        public void Close()
        {
            Bridge.SocketClientClose(context);
        }

        /// <summary>
        /// 获取最近的错误信息。
        /// </summary>
        /// <returns>错误信息</returns>
        public string GetErrorMessage()
        {
            return Bridge.SocketClientGetErr(context);
        }

        /// <summary>
        /// 向 TCP 客户端发送数据，返回发送成功的字节数。
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="timeout">写入超时时间（毫秒，默认 30000）</param>
        /// <returns>发送成功的字节数</returns>
        public int Send(byte[] data, int timeout = 30000)
        {
            return Bridge.SocketClientWrite(context, timeout, data);
        }
        /// <summary>
        /// 向 TCP 客户端发送数据，返回发送成功的字节数。
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="Encoding">编码格式，默认为 UTF-8。</param>
        /// <param name="timeout">写入超时时间（毫秒，默认 30000）</param>
        /// <returns>发送成功的字节数</returns>
        public int Send(string data, string Encoding = "UTF-8", int timeout = 30000)
        {
            return Send(Tool.StrToBytes(data, Encoding), timeout);
        }


        /// <summary>
        /// 接收 TCP 客户端的数据。同步模式下有效，异步模式下无效。
        /// </summary>
        /// <param name="timeout">接收超时时间（毫秒，默认 100）</param>
        /// <returns>接收到的数据</returns>
        public EventValue ReceiveData(int timeout = 100)
        {
            return new EventValue(Bridge.SocketClientReceive(context, timeout));
        }

        /// <summary>
        /// 设置缓冲区大小，请在连接之前调用。
        /// </summary>
        /// <param name="bufferSize">缓冲区大小（默认 4096）</param>
        /// <returns>设置是否成功</returns>
        public bool SetBufferSize(int bufferSize = 4096)
        {
            return Bridge.SocketClientSetBufferSize(context, bufferSize);
        }

        // TCP 客户端默认回调委托
        public delegate void TcpClientDefaultCallback(IntPtr context, IntPtr messageType, IntPtr dataPointer, IntPtr pointerLength);

        /// <summary>
        /// TCP 客户端回调处理。
        /// </summary>
        /// <param name="context">客户端上下文</param>
        /// <param name="messageType">消息类型（1=接收消息，2=连接断开）</param>
        /// <param name="dataPointer">数据指针（消息类型为 2、3 时为错误信息）</param>
        /// <param name="pointerLength">数据指针长度</param>
        private void DefaultTcpClientCallback(IntPtr context, IntPtr messageType, IntPtr dataPointer, IntPtr pointerLength)
        {
            if (_callback != null)
            {
                _callback.OnCallback(new TCPClientEvent(context, messageType, Tool.PtrToBytes(dataPointer, pointerLength.ToInt64())));
            }
        }
    }
}