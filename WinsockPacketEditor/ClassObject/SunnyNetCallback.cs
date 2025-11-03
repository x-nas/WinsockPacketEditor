using SunnyNetlibray.Event;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WinsockPacketEditor
{
    public class SunnyNetCallback : SunnyNetlibray.Internal.SunnyNet
    {
        public void OnHttpCallback(HTTPEvent Conn)
        {
            

            switch (Conn.Type())
            {
                case HTTPEvent.EventType_HTTP_Request:

                    Operate.ProxyConfig.Proxy.DomainType dtRequest = Operate.ProxyConfig.Proxy.DomainType.HTTP;
                    Operate.PacketConfig.Packet.PacketType ptRequest = Operate.PacketConfig.Packet.PacketType.HTTP_Req;
                    if (Conn.URL().StartsWith("https://"))
                    {
                        dtRequest = Operate.ProxyConfig.Proxy.DomainType.HTTPS;
                        ptRequest = Operate.PacketConfig.Packet.PacketType.HTTPS_Req;
                    }

                    Operate.DoLog(nameof(OnHttpCallback), Conn.Request().Body().String());

                    _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Request().BodyLen(),
                            0,
                            ptRequest,
                            Conn.ClientIP(),
                            Conn.Response().ServerAddress(),
                            Conn.URL(),
                            dtRequest,
                            Conn.Request().Body().Bytes,
                            Conn.Request().Body().Bytes);

                    break;
                case HTTPEvent.EventType_HTTP_Response:

                    Operate.ProxyConfig.Proxy.DomainType dtResponse = Operate.ProxyConfig.Proxy.DomainType.HTTP;
                    Operate.PacketConfig.Packet.PacketType ptResponse = Operate.PacketConfig.Packet.PacketType.HTTP_Resp;
                    if (Conn.URL().StartsWith("https://"))
                    {
                        dtResponse = Operate.ProxyConfig.Proxy.DomainType.HTTPS;
                        ptResponse = Operate.PacketConfig.Packet.PacketType.HTTPS_Resp;
                    }

                    Operate.DoLog(nameof(OnHttpCallback), Conn.Response().Body().String());

                    _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Response().Body().Length,
                            0,
                            ptResponse,
                            Conn.ClientIP(),
                            Conn.Response().ServerAddress(),
                            Conn.URL(),
                            dtResponse,
                            Conn.Response().Body().Bytes,
                            Conn.Response().Body().Bytes);

                    break;
                case HTTPEvent.EventType_HTTP_Error:
                    Debug.WriteLine("请求错误:" + Conn.URL() + " ->> " + Conn.Error());
                    break;
            }
        }

        public void OnTcpCallback(TCPEvent Conn)
        {
            //你可以记录保存 Conn.TheologyID() 唯一ID,使用以下函数,在回调函数以外的任意位置发送数据
            //SunnyNet.Tools.TCPTools.SendMessage()
            //SunnyNet.Tools.TCPTools.Close()

            switch (Conn.Type())
            {
                case TCPEvent.EventType_TCP_About:
                    Debug.WriteLine("TCP 即将连接:" + Conn.LocalAddr() + " -> " + Conn.RemoteAddr());
                    break;
                case TCPEvent.EventType_TCP_OK:
                    Debug.WriteLine("TCP 连接成功:" + Conn.LocalAddr() + " -> " + Conn.RemoteAddr());
                    break;
                case TCPEvent.EventType_TCP_Send:
                    Debug.WriteLine("TCP 发送消息:" + Conn.LocalAddr() + " -> " + Conn.RemoteAddr() + ",发送:" + Conn.Body().Length + " / byte");

                    _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Operate.PacketConfig.Packet.PacketType.TCP_Req,
                            Conn.LocalAddr(),
                            Conn.RemoteAddr(),
                            Conn.RemoteAddr(),
                            Operate.ProxyConfig.Proxy.DomainType.Socket,
                            Conn.Body().Bytes,
                            Conn.Body().Bytes);

                    break;
                case TCPEvent.EventType_TCP_Receive:
                    Debug.WriteLine("TCP 收到数据:" + Conn.LocalAddr() + " -> " + Conn.RemoteAddr() + ",接收:" + Conn.Body().Length + " / byte");

                    _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Operate.PacketConfig.Packet.PacketType.TCP_Resp,
                            Conn.LocalAddr(),
                            Conn.RemoteAddr(),
                            Conn.RemoteAddr(),
                            Operate.ProxyConfig.Proxy.DomainType.Socket,
                            Conn.Body().Bytes,
                            Conn.Body().Bytes);

                    break;
                case TCPEvent.EventType_TCP_Close:
                    Debug.WriteLine("TCP 连接关闭:" + Conn.LocalAddr() + " -> " + Conn.RemoteAddr());
                    break;
            }
        }

        public void OnUdpCallback(UDPEvent Conn)
        {
            //你可以记录保存 Conn.TheologyID() 唯一ID,使用以下函数,在回调函数以外的任意位置发送数据
            //SunnyNet.Tools.UDPTools.SendMessage() 

            string ClientIP = GetUDPIPString(Conn.LocalAddr());
            string ServerIP = GetUDPIPString(Conn.RemoteAddr());

            switch (Conn.Type())
            {
                case UDPEvent.EventType_UDP_Send:
                    Debug.WriteLine("UDP 发送消息:" + Conn.LocalAddr() + " -> " + Conn.RemoteAddr() + ",发送:" + Conn.Body().Length + " / byte");

                    _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Operate.PacketConfig.Packet.PacketType.UDP_Req,
                            ClientIP,
                            ServerIP,
                            ServerIP,
                            Operate.ProxyConfig.Proxy.DomainType.Socket,
                            Conn.Body().Bytes,
                            Conn.Body().Bytes);

                    break;
                case UDPEvent.EventType_UDP_Receive:
                    Debug.WriteLine("UDP 收到数据:" + Conn.LocalAddr() + " -> " + Conn.RemoteAddr() + ",接收:" + Conn.Body().Length + " / byte");

                    _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Operate.PacketConfig.Packet.PacketType.UDP_Resp,
                            ClientIP,
                            ServerIP,
                            ServerIP,
                            Operate.ProxyConfig.Proxy.DomainType.Socket,
                            Conn.Body().Bytes,
                            Conn.Body().Bytes);

                    break;
                case UDPEvent.EventType_UDP_Closed:
                    Debug.WriteLine("UDP 连接关闭:" + Conn.LocalAddr() + " -> " + Conn.RemoteAddr());
                    break;
            }
        }

        public void OnWebSocketCallback(WebSocketEvent Conn)
        {
            //你可以记录保存 Conn.TheologyID() 唯一ID,使用以下函数,在回调函数以外的任意位置发送数据、关闭会话
            //SunnyNet.Tools.WebSocketTools.SendMessage()
            //SunnyNet.Tools.WebSocketTools.Close()
            switch (Conn.Type())
            {
                case WebSocketEvent.EventType_Websocket_OK:
                    Debug.WriteLine("WebSocket 连接成功:" + Conn.URL());
                    break;
                case WebSocketEvent.EventType_Websocket_Send:
                    Debug.WriteLine("WebSocket 发送消息:" + Conn.URL() + " -> " + ",发送:" + Conn.Body().Length + " / byte  ->wsMeassageType:" + Conn.MessageType());
                    break;
                case WebSocketEvent.EventType_Websocket_Receive:
                    Debug.WriteLine("WebSocket 收到数据:" + Conn.URL() + " -> " + ",接收:" + Conn.Body().Length + " / byte ->wsMeassageType:" + Conn.MessageType());
                    break;
                case WebSocketEvent.EventType_Websocket_Close:
                    Debug.WriteLine("WebSocket 连接关闭:" + Conn.URL());
                    break;
            }
        }

        public void OnScriptCodeSaveCallback(long SunnyNetContext, SunnyNetlibray.Internal.EventValue scriptCode)
        {
            Debug.WriteLine(scriptCode.String() + "\r\n脚本编辑按下了保存按钮！");
        }

        public void OnScriptLogCallback(long SunnyNetContext, SunnyNetlibray.Internal.EventValue logInfo)
        {
            Debug.WriteLine(" 脚本日志：" + logInfo.String());
            Debug.WriteLine(" 脚本日志1：" + logInfo.Length);
        }

        public string GetUDPIPString(string UDPString)
        {
            return Regex.Replace(UDPString, @"[\[\]]", "");
        }
    }
}
