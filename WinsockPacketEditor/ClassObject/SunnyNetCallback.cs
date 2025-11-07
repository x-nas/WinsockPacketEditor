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

                if (Conn.URL().ToUpper().StartsWith("HTTPS"))
                {                    
                    dtType = Operate.ProxyConfig.Proxy.DomainType.HTTPS;
                }

                switch (Conn.Type())
                {
                    case HTTPEvent.EventType_HTTP_Request:

                        string ConnPort = "80";
                        Operate.PacketConfig.Packet.PacketType ptRequest = Operate.PacketConfig.Packet.PacketType.HTTP_Req;
                        
                        if (dtType == Operate.ProxyConfig.Proxy.DomainType.HTTPS)
                        {
                            ConnPort = "443";
                            ptRequest = Operate.PacketConfig.Packet.PacketType.HTTPS_Req;
                        }

                        if (Conn.GetUser().Equals("驱动程序"))
                        {
                            if (Operate.ProxyConfig.Proxy.MustTCP && Operate.ProxyConfig.Proxy.IsLoadDriver)
                            {
                                if (Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(ConnPort))
                                {
                                    Conn.Request().SetProxy(Operate.SystemConfig.GetMustTCP(), 5000);
                                    return;
                                }
                            }
                        }

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Request().BodyLen(),
                            0,
                            ptRequest,
                            Conn.ClientIP(),
                            Conn.Response().ServerAddress(),
                            Conn.URL(),
                            dtType,
                            Conn.Request().Body().Bytes,
                            Conn.Request().Body().Bytes);

                        break;

                    case HTTPEvent.EventType_HTTP_Response:

                        Operate.PacketConfig.Packet.PacketType ptResponse = Operate.PacketConfig.Packet.PacketType.HTTP_Resp;
                        
                        if (dtType == Operate.ProxyConfig.Proxy.DomainType.HTTPS)
                        {
                            ptResponse = Operate.PacketConfig.Packet.PacketType.HTTPS_Resp;
                        }

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            Conn.Response().Body().Length,
                            0,
                            ptResponse,
                            Conn.ClientIP(),
                            Conn.Response().ServerAddress(),
                            Conn.URL(),
                            dtType,
                            Conn.Response().Body().Bytes,
                            Conn.Response().Body().Bytes);

                        break;

                    case HTTPEvent.EventType_HTTP_Error:

                        string sError = Conn.Error();
                        if (sError.StartsWith("[SunnyNet]"))
                        {
                            sError = sError.Remove(0, 10);
                        }

                        Operate.DoLog(nameof(OnHttpCallback), sError);

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
                if (Conn.GetUser().Equals("驱动程序"))
                {
                    if (Operate.ProxyConfig.Proxy.MustTCP && Operate.ProxyConfig.Proxy.IsLoadDriver)
                    {
                        if (Conn.Type() == TCPEvent.EventType_TCP_About)
                        {
                            string ConnPort = Conn.RemoteAddr().Split(':')[1];

                            if (Operate.ProxyConfig.Proxy.IsMustTCP_ByPort(ConnPort.ToString()))
                            {
                                Conn.SetProxy(Operate.SystemConfig.GetMustTCP(), 5000);
                                return;
                            }
                        }
                    }
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
            try
            {
                string ClientIP = Operate.SystemConfig.GetUDPIPString(Conn.LocalAddr());
                string ServerIP = Operate.SystemConfig.GetUDPIPString(Conn.RemoteAddr());

                switch (Conn.Type())
                {
                    case UDPEvent.EventType_UDP_Send:

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
            //不做处理
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
