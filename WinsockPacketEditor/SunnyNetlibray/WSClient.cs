using SunnyNetlibray.Event;
using SunnyNetlibray.Internal;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System;

namespace SunnyNetlibray
{
    /// <summary>
    /// WebSocket 客户端类，参考易语言用法。
    /// </summary>
    public class WSClient
    {
        public class Resut
        {
            /// <summary>
            /// 消息类型，请使用以下常量之一，来判断：
            /// <list type="bullet">
            ///   <item><see cref="Const.WsMessageType.Text"/> 文本消息</item>
            ///   <item><see cref="Const.WsMessageType.Binary"/> 二进制消息</item>
            ///   <item><see cref="Const.WsMessageType.Ping"/> Ping 消息</item>
            ///   <item><see cref="Const.WsMessageType.Pong"/> Pong 消息</item>
            ///   <item><see cref="Const.WsMessageType.Close"/> 关闭消息</item>
            ///   <item><see cref="Const.WsMessageType.Invalid"/> 无效的消息</item>
            /// </list>
            /// </summary>
            public int MessageType = 0;
            /// <summary>
            /// 本次事件的消息内容
            /// </summary>
            public byte[] Bytes;

            public Resut(int MessageType, byte[] data)
            {
                this.MessageType = MessageType;
                this.Bytes = data;
            }
        }
        private long context = 0; // 客户端标识 
        private WsDefaultCallback funcCall = null;
        private WebsocketClient _callback = null;
        private WsDefaultHeartbeatCallback funcCall2 = null;
        private WebsocketClient _HeartbeatCallback = null;

        /// <summary>
        /// 初始化 WSClient 实例并创建 WebSocket。
        /// </summary>
        public WSClient()
        {
            context = Bridge.CreateWebsocket();
        }

        /// <summary>
        /// 释放资源，自动调用。
        /// </summary>
        ~WSClient()
        {
            Bridge.RemoveWebsocket(context);
        }

        /// <summary>
        /// 重新创建 WebSocket 客户端。
        /// </summary>
        public void Recreate()
        {
            Bridge.WebsocketClose(context);
            context = Bridge.CreateWebsocket();
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
        /// 设置心跳函数
        /// <param name="callback">心跳的 回调函数</param>
        /// <param name="HeartbeatTime">心跳的函数 触发的 间隔时间</param>
        /// </summary>
        public void Heartbeat(WebsocketClient callback, int HeartbeatTime = 3000)
        {
            _HeartbeatCallback = callback;
            if (callback == null)
            {
                Bridge.WebsocketHeartbeat(context, 0, new IntPtr(0));
                return;
            }
            funcCall2 = new WsDefaultHeartbeatCallback(DefaultWsHeartbeatCallback);
            Bridge.WebsocketHeartbeat(context, HeartbeatTime, Marshal.GetFunctionPointerForDelegate(funcCall2));
        }

        /// <summary>
        /// 连接 WebSocket 客户端。
        /// </summary>
        /// <param name="url">WebSocket 服务器地址</param>
        /// <param name="callback">回调函数，处理 WebSocket 事件,如果为NULL 表示同步模式</param>
        /// <param name="headers">请求头（可选）</param> 
        /// <param name="RouterIP">出口IP[请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择]</param> 
        /// <param name="connectionTimeout">连接超时时间（毫秒，默认 30000）</param>
        /// <param name="proxy">代理地址，仅支持 Socket5 和 HTTP</param>
        /// <param name="certManager">证书管理器（可选）</param>
        /// <returns>连接是否成功</returns>
        public bool Connect(string url, WebsocketClient callback, string headers = "", int connectionTimeout = 30000, string proxy = "", CertManager certManager = null, string RouterIP = "")
        {
            _callback = callback;
            funcCall = new WsDefaultCallback(DefaultWsCallback);
            bool isSynchronous = false;
            if (callback == null)
            {
                isSynchronous = true;
            }
            return Bridge.WebsocketDial(context, url, headers, Marshal.GetFunctionPointerForDelegate(funcCall), isSynchronous, proxy, certManager, connectionTimeout, RouterIP);
        }

        /// <summary>
        /// 连接 WebSocket 客户端。同步模式
        /// </summary>
        /// <param name="url">WebSocket 服务器地址</param>
        /// <param name="headers">请求头（可选）</param> 
        /// <param name="RouterIP">出口IP[请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择]</param> 
        /// <param name="connectionTimeout">连接超时时间（毫秒，默认 30000）</param>
        /// <param name="proxy">代理地址，仅支持 Socket5 和 HTTP</param>
        /// <param name="certManager">证书管理器（可选）</param>
        /// <returns>连接是否成功</returns>
        public bool Connect(string url, string headers = "", int connectionTimeout = 30000, string proxy = "", CertManager certManager = null, string RouterIP = "")
        {
            return Connect(url, null, headers, connectionTimeout, proxy, certManager, RouterIP);
        }
        /// <summary>
        /// 连接 WebSocket 客户端。同步模式
        /// </summary>
        /// <param name="url">WebSocket 服务器地址</param>
        /// <param name="headers">请求头（可选）</param> 
        /// <param name="RouterIP">出口IP[请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择]</param> 
        /// <param name="connectionTimeout">连接超时时间（毫秒，默认 30000）</param>
        /// <param name="proxy">代理地址，仅支持 Socket5 和 HTTP</param>
        /// <returns>连接是否成功</returns>
        public bool Connect(string url, string headers = "", int connectionTimeout = 30000, string proxy = "", string RouterIP = "")
        {
            return Connect(url, headers, connectionTimeout, "", RouterIP);
        }
        /// <summary>
        /// 连接 WebSocket 客户端。同步模式
        /// </summary>
        /// <param name="url">WebSocket 服务器地址</param>
        /// <param name="headers">请求头（可选）</param> 
        /// <param name="RouterIP">出口IP[请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择]</param>  
        /// <returns>连接是否成功</returns>
        public bool Connect(string url, string headers = "", string RouterIP = "")
        {
            return Connect(url, headers, 30000, "", RouterIP);
        }

        /// <summary>
        /// 连接 WebSocket 客户端。同步模式
        /// </summary>
        /// <param name="url">WebSocket 服务器地址</param>
        /// <param name="RouterIP">出口IP[请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择]</param>  
        /// <returns>连接是否成功</returns>
        public bool Connect(string url, string RouterIP = "")
        {
            return Connect(url, "", RouterIP);
        }
        /// <summary>
        /// 断开 WebSocket 连接。
        /// </summary>
        public void Close()
        {
            Bridge.WebsocketClose(context);
        }

        /// <summary>
        /// 获取最近的错误信息。
        /// </summary>
        /// <returns>错误信息</returns>
        public string GetErrorMessage()
        {
            return Bridge.WebsocketGetErr(context);
        }

        /// <summary>
        /// 向 WebSocket 发送数据。
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="messageType">消息类型，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="Const.WsMessageType.Text"/> 文本消息</item>
        ///   <item><see cref="Const.WsMessageType.Binary"/> 二进制消息</item>
        ///   <item><see cref="Const.WsMessageType.Ping"/> Ping 消息</item>
        ///   <item><see cref="Const.WsMessageType.Pong"/> Pong 消息</item>
        ///   <item><see cref="Const.WsMessageType.Close"/> 关闭消息</item>
        ///   <item><see cref="Const.WsMessageType.Invalid"/> 无效的消息</item>
        /// </list></param>
        /// <returns>发送是否成功</returns>
        public bool Send(byte[] data, int messageType = Const.WsMessageType.Text)
        {
            return Bridge.WebsocketReadWrite(context, data, messageType);
        }

        /// <summary>
        /// 向 WebSocket 发送数据。
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="messageType">消息类型，请使用以下常量之一：
        /// <list type="bullet">
        ///   <item><see cref="Const.WsMessageType.Text"/> 文本消息</item>
        ///   <item><see cref="Const.WsMessageType.Binary"/> 二进制消息</item>
        ///   <item><see cref="Const.WsMessageType.Ping"/> Ping 消息</item>
        ///   <item><see cref="Const.WsMessageType.Pong"/> Pong 消息</item>
        ///   <item><see cref="Const.WsMessageType.Close"/> 关闭消息</item>
        ///   <item><see cref="Const.WsMessageType.Invalid"/> 无效的消息</item>
        /// </list></param>
        /// <returns>发送是否成功</returns>
        public bool Send(string data, int messageType = Const.WsMessageType.Text, string Encoding = "UTF-8")
        {
            return Send(Tool.StrToBytes(data, Encoding), messageType);
        }
        /// <summary>
        /// 在同步模式下接收数据，异步模式下无效。
        /// </summary> 
        /// <param name="timeout">超时时间（毫秒，默认 3000）</param>
        /// <returns>接收到的数据</returns>
        public Resut ReceiveData(int timeout = 3000)
        {
            int wsType = Const.WsMessageType.Invalid;
            byte[] receivedData = Bridge.WebsocketClientReceive(out wsType, context, timeout);
            return new Resut(wsType, receivedData);
        }

        // WebSocket 默认回调委托
        public delegate void WsDefaultCallback(IntPtr context, IntPtr messageType, IntPtr dataPointer, IntPtr pointerLength, IntPtr dataType);

        /// <summary>
        /// WebSocket 客户端回调处理。
        /// </summary>
        /// <param name="context">客户端上下文</param>
        /// <param name="messageType">消息类型（1=接收消息，2=接收时连接被断开，3=发送时连接被断开）</param>
        /// <param name="dataPointer">消息数据指针</param>
        /// <param name="pointerLength">数据指针长度</param>
        /// <param name="dataType">数据类型（在消息类型为 1 时有效）</param>
        private void DefaultWsCallback(IntPtr context, IntPtr messageType, IntPtr dataPointer, IntPtr pointerLength, IntPtr dataType)
        {
            if (_callback != null)
            {
                _callback.OnCallback(new WebsocketClientEvent(context, messageType, Tool.PtrToBytes(dataPointer, pointerLength.ToInt64()), dataType));
            }
        }
        // WebSocket 默认回调委托
        public delegate void WsDefaultHeartbeatCallback(IntPtr context);

        /// <summary>
        /// WebSocket 客户端回调处理。
        /// </summary>
        /// <param name="context">客户端上下文</param>
        /// <param name="messageType">消息类型（1=接收消息，2=接收时连接被断开，3=发送时连接被断开）</param>
        /// <param name="dataPointer">消息数据指针</param>
        /// <param name="pointerLength">数据指针长度</param>
        /// <param name="dataType">数据类型（在消息类型为 1 时有效）</param>
        private void DefaultWsHeartbeatCallback(IntPtr context)
        {
            if (_callback != null)
            {
                _callback.OnHeartbeatCallback(context.ToInt64());
            }
        }
    }
}