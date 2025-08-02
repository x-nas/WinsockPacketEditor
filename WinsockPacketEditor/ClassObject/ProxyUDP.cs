using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace WinsockPacketEditor
{
    public class ProxyUDP
    {
        public UdpClient ClientUDP { get; private set; }
        public IPEndPoint ClientEndPoint { get; set; }
        public DateTime LastActivityTime { get; private set; }
        public bool IsActive { get; private set; }

        #region//ProxyUDP

        public ProxyUDP(IPEndPoint udpClientEndpoint)
        {
            this.ClientUDP = new UdpClient(udpClientEndpoint);
            this.LastActivityTime = DateTime.Now;
            this.IsActive = true;
        }

        #endregion

        #region//更新活动时间

        public void UpdateActivity()
        {
            this.LastActivityTime = DateTime.Now;
        }

        #endregion

        #region//关闭UDP连接

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

        #endregion
    }
}