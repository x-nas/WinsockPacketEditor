using SunnyNetlibray.Event;
using System;

namespace WinsockPacketEditor
{
    public class SunnyNetCallback : SunnyNetlibray.Internal.SunnyNet
    {
        #region//OnHttpCallback

        public void OnHttpCallback(HTTPEvent Conn)
        {
            try
            {
                Operate.ProxyConfig.Proxy.DomainType dtType = Operate.ProxyConfig.Proxy.DomainType.HTTP;
                bool bHttps = Conn.URL().ToUpper().StartsWith("HTTPS");

                if (bHttps)
                {
                    dtType = Operate.ProxyConfig.Proxy.DomainType.HTTPS;
                }

                //「指定端口」按 URL 上的真实端口判：原先按 scheme 写死 80 / 443，8080 上的 HTTP 会被当成 80
                int ConnPort = Operate.ProxyConfig.Proxy.PortOfUrl(Conn.URL(), bHttps);

                switch (Conn.Type())
                {
                    case HTTPEvent.EventType_HTTP_Request:

                        if (Operate.ProxyConfig.Proxy.MustTCP && Operate.ProxyConfig.Proxy.IsLoadDriver)
                        {
                            if (Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(ConnPort))
                            {
                                Conn.Request().SetProxy(Operate.SystemConfig.GetMustTCP(), 5000);
                                return;
                            }
                        }

                        Operate.PacketConfig.Packet.PacketType ptRequest = Operate.PacketConfig.Packet.PacketType.HTTP_Req;
                        if (dtType == Operate.ProxyConfig.Proxy.DomainType.HTTPS)
                        {
                            ptRequest = Operate.PacketConfig.Packet.PacketType.HTTPS_Req;
                        }

                        string sRequest = string.Format("{0} {1} {2}\r\n{3}", Conn.Method(), Conn.URL(), Conn.Request().GetProto(), Conn.Request().GetAllHeader());
                        if (Conn.Request().BodyLen() > 0)
                        {
                            sRequest += Conn.Request().Body().String();
                        }
                        sRequest = sRequest.Trim();

                        byte[] bRequest = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sRequest);

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            bRequest.Length,
                            0,
                            0,
                            ptRequest,
                            0,
                            Conn.ClientIP(),
                            Conn.Response().ServerAddress(),
                            Conn.URL(),
                            dtType,
                            bRequest,
                            bRequest,
                            sRequest);

                        break;

                    case HTTPEvent.EventType_HTTP_Response:

                        if (Operate.ProxyConfig.Proxy.MustTCP && Operate.ProxyConfig.Proxy.IsLoadDriver)
                        {
                            if (Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(ConnPort))
                            {
                                return;
                            }
                        }

                        Operate.PacketConfig.Packet.PacketType ptResponse = Operate.PacketConfig.Packet.PacketType.HTTP_Resp;                        
                        if (dtType == Operate.ProxyConfig.Proxy.DomainType.HTTPS)
                        {
                            ptResponse = Operate.PacketConfig.Packet.PacketType.HTTPS_Resp;
                        }

                        string sResponse = string.Format("{0} {1}\r\n{2}", Conn.Response().GetProto(), Conn.Response().StatusText(), Conn.Response().GetAllHeader());
                        if (Conn.Response().BodyLen() > 0)
                        {
                            sResponse += "\r\n" + Conn.Response().BodyAuto().String();
                        }
                        sResponse = sResponse.Trim();

                        byte[] bResponse = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sResponse);

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            bResponse.Length,
                            0,
                            0,
                            ptResponse,
                            0,
                            Conn.ClientIP(),
                            Conn.Response().ServerAddress(),
                            Conn.URL(),
                            dtType,
                            bResponse,
                            bResponse,
                            sResponse);

                        break;

                    case HTTPEvent.EventType_HTTP_Error:

                        string sError = Conn.Error();
                        if (sError.StartsWith("[SunnyNet]"))
                        {
                            sError = sError.Remove(0, 10);
                        }

                        //Operate.DoLog(nameof(OnHttpCallback), sError);

                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnHttpCallback), ex);
            }            
        }

        #endregion

        #region//OnTcpCallback

        public void OnTcpCallback(TCPEvent Conn)
        {
            try
            {
                Operate.ProxyConfig.Proxy.DomainType dtType = Operate.ProxyConfig.Proxy.DomainType.Socket;

                switch (Conn.Type())
                {
                    /*
                        三个分支的判据必须是同一个：「这条连接转不转代理」= MustTCP && 驱动已装 && 端口在名单里。
                        原先 Send / Receive 只看前两项、不看端口 —— 勾了「指定端口」之后，
                        名单之外的 TCP 连接在 About 里没转代理，到 Send / Receive 又被无条件 return，
                        既不转、也不进列表，静默消失。
                    */
                    case TCPEvent.EventType_TCP_About:

                        if (MustProxy(Conn.RemoteAddr()))
                        {
                            Conn.SetProxy(Operate.SystemConfig.GetMustTCP(), 5000);
                            return;
                        }

                        break;

                    case TCPEvent.EventType_TCP_OK:
                        break;

                    case TCPEvent.EventType_TCP_Send:

                        //转了代理的连接，字节会从 SOCKS5 那一路进列表（并过滤镜），这里不再记一遍
                        if (MustProxy(Conn.RemoteAddr()))
                        {
                            return;
                        }

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Conn.TheologyID(),
                            Operate.PacketConfig.Packet.PacketType.TCP_Req,
                            0,
                            Conn.LocalAddr(),
                            Conn.RemoteAddr(),
                            Conn.RemoteAddr(),
                            dtType,
                            Conn.Body().Bytes,
                            Conn.Body().Bytes,
                            null);

                        break;

                    case TCPEvent.EventType_TCP_Receive:

                        if (MustProxy(Conn.RemoteAddr()))
                        {
                            return;
                        }

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Conn.TheologyID(),
                            Operate.PacketConfig.Packet.PacketType.TCP_Resp,
                            0,
                            Conn.LocalAddr(),
                            Conn.RemoteAddr(),
                            Conn.RemoteAddr(),
                            dtType,
                            Conn.Body().Bytes,
                            Conn.Body().Bytes,
                            null);

                        break;

                    case TCPEvent.EventType_TCP_Close:
                        break;
                }                                                        
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnTcpCallback), ex);
            }            
        }

        #endregion

        #region//OnUdpCallback        

        public void OnUdpCallback(UDPEvent Conn)
        {
            long TheologyID = Conn.TheologyID();

            try
            {
                Operate.ProxyConfig.Proxy.DomainType dtType = Operate.ProxyConfig.Proxy.DomainType.Socket;

                string ClientIP = Operate.SystemConfig.GetUDPIPString(Conn.LocalAddr());
                string ServerIP = Operate.SystemConfig.GetUDPIPString(Conn.RemoteAddr());

                switch (Conn.Type())
                {
                    case UDPEvent.EventType_UDP_Send:

                        if (MustProxy(Conn.RemoteAddr()))
                        {
                            //Body 置空让 SunnyNet 不再直发，数据交给中继（MustTcpUdpRelay）经 SOCKS5 送出去
                            byte[] bSendData = Conn.Body().Bytes;
                            Conn.Body(null);

                            Operate.ProxyConfig.Proxy.SetUDPProxy(Conn, bSendData);
                            return;
                        }

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Conn.TheologyID(),
                            Operate.PacketConfig.Packet.PacketType.UDP_Req,
                            0,
                            ClientIP,
                            ServerIP,
                            ServerIP,
                            dtType,
                            Conn.Body().Bytes,
                            Conn.Body().Bytes,
                            null);

                        break;

                    case UDPEvent.EventType_UDP_Receive:

                        //转了代理的套接字，它收到的只可能是中继交回来的应答（请求根本没直发），SOCKS5 那一路已经记过了
                        if (MustProxy(Conn.RemoteAddr()))
                        {
                            return;
                        }

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Conn.TheologyID(),
                            Operate.PacketConfig.Packet.PacketType.UDP_Resp,
                            0,
                            ClientIP,
                            ServerIP,
                            ServerIP,
                            dtType,
                            Conn.Body().Bytes,
                            Conn.Body().Bytes,
                            null);

                        break;

                    case UDPEvent.EventType_UDP_Closed:

                        //目标进程关了这个套接字 → 它那条 SOCKS5 UDP 关联也收掉（没这一句只能等 3 分钟静置回收）
                        Operate.ProxyConfig.Proxy.CloseUDPProxy(TheologyID);
                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnUdpCallback), ex);
            }
        }

        #endregion

        #region//这条连接转不转代理

        /// <summary>
        /// 「强制转代理」对这条连接生不生效：开关开着、驱动已装、远端端口在「指定端口」名单里（没勾名单则一律算在）。
        /// TCP 的三个分支与 UDP 的两个分支都用它，判据只此一份。
        /// 端口从 "ip:port" / "[v6]:port" 里取 —— 原先 Split(':')[1] 在 IPv6 上取到的是地址的一段。
        /// </summary>
        private static bool MustProxy(string RemoteAddr)
        {
            if (!Operate.ProxyConfig.Proxy.MustTCP || !Operate.ProxyConfig.Proxy.IsLoadDriver)
            {
                return false;
            }

            return Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(Operate.ProxyConfig.Proxy.PortOfAddress(RemoteAddr));
        }

        #endregion

        #region//OnWebSocketCallback

        public void OnWebSocketCallback(WebSocketEvent Conn)
        {
            try
            {
                Operate.ProxyConfig.Proxy.DomainType dtType = Operate.ProxyConfig.Proxy.DomainType.WebSocket;

                switch (Conn.Type())
                {
                    case WebSocketEvent.EventType_Websocket_OK:
                        break;

                    case WebSocketEvent.EventType_Websocket_Send:

                        string sRequest = string.Format("{0} {1}", Conn.Method(), Conn.URL());
                        if (Conn.Body().Length > 0)
                        {
                            sRequest += Conn.Body().String();
                        }
                        sRequest = sRequest.Trim();

                        byte[] bRequest = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sRequest);

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Conn.TheologyID(),
                            Operate.PacketConfig.Packet.PacketType.WebSocket_Req,
                            Conn.MessageType(),
                            string.Empty,
                            string.Empty,
                            Conn.URL(),
                            dtType,
                            bRequest,
                            bRequest,
                            sRequest);

                        break;

                    case WebSocketEvent.EventType_Websocket_Receive:

                        string sResponse = string.Format("{0} {1}", Conn.Method(), Conn.URL());
                        if (Conn.Body().Length > 0)
                        {
                            sResponse += Conn.Body().String();
                        }
                        sResponse = sResponse.Trim();

                        byte[] bResponse = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sResponse);

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Body().Length,
                            0,
                            Conn.TheologyID(),
                            Operate.PacketConfig.Packet.PacketType.WebSocket_Resp,
                            Conn.MessageType(),
                            string.Empty,
                            string.Empty,
                            Conn.URL(),
                            dtType,
                            bResponse,
                            bResponse,
                            sResponse);

                        break;

                    case WebSocketEvent.EventType_Websocket_Close:
                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnWebSocketCallback), ex);
            }            
        }

        #endregion

        #region//OnScriptCodeSaveCallback

        public void OnScriptCodeSaveCallback(long SunnyNetContext, SunnyNetlibray.Internal.EventValue scriptCode)
        {
            //不做处理
        }

        #endregion

        #region//OnScriptLogCallback

        public void OnScriptLogCallback(long SunnyNetContext, SunnyNetlibray.Internal.EventValue logInfo)
        {
            //不做处理
        }

        #endregion
    }
}
