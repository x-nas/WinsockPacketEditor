using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace WinsockPacketEditor
{
    public class ProxyUDP
    {
        public UdpClient ClientUDP { get; private set; }
        public IPEndPoint ClientUDP_EndPoint { get; set; }
        public DateTime LastActivityTime { get; private set; }
        public bool IsActive { get; private set; }

        public ProxyUDP(IPEndPoint udpClientEndpoint)
        {
            this.ClientUDP = new UdpClient(udpClientEndpoint);
            this.LastActivityTime = DateTime.Now;
            this.IsActive = true;
        }

        public void UpdateActivity()
        {
            this.LastActivityTime = DateTime.Now;
        }

        public void Close()
        {
            try
            {
                if (!IsActive) return;

                IsActive = false;

                try
                {
                    ClientUDP?.Close();
                    ClientUDP?.Dispose();
                }
                finally
                {
                    ClientUDP = null;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}