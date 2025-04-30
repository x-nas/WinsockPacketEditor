using System;
using System.Net.Sockets;
using System.Net;
using System.Reflection;

namespace WPELibrary.Lib
{
    public class Socket_ProxyUDP
    {
        public UdpClient ClientUDP { get; set; }
        public IPEndPoint ClientUDP_EndPoint { get; set; }
        public DateTime ClientUDP_Time { get; set; }

        public void CloseUDPClient()
        {
            try
            {
                if (this.ClientUDP != null)
                {
                    this.ClientUDP.Close();
                    this.ClientUDP = null;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public Socket_ProxyUDP(IPEndPoint UDPClient)
        {
            this.ClientUDP = new UdpClient(UDPClient);
        }        
    }
}
