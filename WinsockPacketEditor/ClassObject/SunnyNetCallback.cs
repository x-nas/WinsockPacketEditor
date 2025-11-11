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
                string ConnPort = "80";
                Operate.ProxyConfig.Proxy.DomainType dtType = Operate.ProxyConfig.Proxy.DomainType.HTTP;

                if (Conn.URL().ToUpper().StartsWith("HTTPS"))
                {
                    ConnPort = "443";
                    dtType = Operate.ProxyConfig.Proxy.DomainType.HTTPS;
                }

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
                Operate.DoLog(nameof(OnHttpCallback), ex.Message);
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
                    case TCPEvent.EventType_TCP_About:

                        if (Operate.ProxyConfig.Proxy.MustTCP && Operate.ProxyConfig.Proxy.IsLoadDriver)
                        {
                            string ConnPort = Conn.RemoteAddr().Split(':')[1];

                            if (Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(ConnPort.ToString()))
                            {
                                Conn.SetProxy(Operate.SystemConfig.GetMustTCP(), 5000);
                                return;
                            }
                        }

                        break;

                    case TCPEvent.EventType_TCP_OK:
                        break;

                    case TCPEvent.EventType_TCP_Send:

                        if (Operate.ProxyConfig.Proxy.MustTCP && Operate.ProxyConfig.Proxy.IsLoadDriver)
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

                        if (Operate.ProxyConfig.Proxy.MustTCP && Operate.ProxyConfig.Proxy.IsLoadDriver)
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
                Operate.DoLog(nameof(OnTcpCallback), ex.Message);
            }            
        }

        #endregion

        #region//OnUdpCallback        

        public void OnUdpCallback(UDPEvent Conn)
        {
            long TheologyID = Conn.TheologyID();

            try
            {
                string ClientIP = Operate.SystemConfig.GetUDPIPString(Conn.LocalAddr());
                string ServerIP = Operate.SystemConfig.GetUDPIPString(Conn.RemoteAddr());

                switch (Conn.Type())
                {
                    case UDPEvent.EventType_UDP_Send:

                        if (Operate.ProxyConfig.Proxy.MustTCP && Operate.ProxyConfig.Proxy.IsLoadDriver)
                        {
                            string ConnPort = Conn.RemoteAddr().Split(':')[1];

                            if (Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(ConnPort.ToString()))
                            {
                                byte[] bSendData = Conn.Body().Bytes;
                                Conn.Body(null);

                                Operate.ProxyConfig.Proxy.SetUDPProxy(Conn, bSendData);
                                return;
                            }
                        }

                        break;

                    case UDPEvent.EventType_UDP_Receive:
                        break;

                    case UDPEvent.EventType_UDP_Closed:
                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnUdpCallback), ex.Message);
            }
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
                Operate.DoLog(nameof(OnWebSocketCallback), ex.Message);
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
