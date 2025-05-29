using System;
using System.Net.Sockets;
using System.Net;

namespace WPELibrary.Lib
{
    public class Socket_ProxyUDP
    {
        public UdpClient ClientUDP { get; set; }
        public IPEndPoint ClientUDP_EndPoint { get; set; }
        public DateTime ClientUDP_Time { get; set; }
        public bool IsActive { get; private set; }        

        public Socket_ProxyUDP(IPEndPoint UDPClient)
        {
            this.ClientUDP = new UdpClient(UDPClient);
            this.ClientUDP_Time = DateTime.Now;
            this.IsActive = true;
        }

        #region//关闭 UDP 客户端

        public void CloseUDPClient()
        {
            if (!IsActive) return;

            IsActive = false;

            try
            {
                ClientUDP?.Close();
            }
            finally
            {
                ClientUDP = null;
            }
        }

        #endregion
    }
}
