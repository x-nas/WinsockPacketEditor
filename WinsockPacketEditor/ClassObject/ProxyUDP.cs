using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace WinsockPacketEditor
{
    public class ProxyUDPManager
    {
        private static readonly ConcurrentDictionary<IPAddress, ProxyUDP> _udpClients =
            new ConcurrentDictionary<IPAddress, ProxyUDP>();
        private static readonly TimeSpan _inactiveTimeout = TimeSpan.FromMinutes(5);

        public static ProxyUDP GetOrCreateUdpClient(IPEndPoint clientEndPoint, IPAddress proxyUdpIp)
        {
            CleanupInactiveClients();

            return _udpClients.GetOrAdd(clientEndPoint.Address, ip =>
            {
                var pu = new ProxyUDP(new IPEndPoint(proxyUdpIp, 0), clientEndPoint.Address);
                Operate.ProxyConfig.Proxy.StartUdpReceive(pu);
                return pu;
            });
        }

        private static void CleanupInactiveClients()
        {
            foreach (var kvp in _udpClients)
            {
                if (DateTime.Now - kvp.Value.LastActivityTime > _inactiveTimeout)
                {
                    if (_udpClients.TryRemove(kvp.Key, out var oldClient))
                    {
                        oldClient.Close();
                    }
                }
            }
        }

        public static bool TryRemoveClient(IPAddress clientIp)
        {
            if (_udpClients.TryRemove(clientIp, out var client))
            {
                client.Close();
                return true;
            }
            return false;
        }
    }

    public class ProxyUDP
    {
        public UdpClient ClientUDP { get; private set; }
        public IPEndPoint ClientUDP_EndPoint { get; set; }
        public DateTime LastActivityTime { get; private set; }
        public bool IsActive { get; private set; }
        public IPAddress ClientIP { get; private set; }

        public ProxyUDP(IPEndPoint udpClientEndpoint, IPAddress clientIp)
        {
            this.ClientUDP = new UdpClient(udpClientEndpoint);
            this.ClientIP = clientIp;
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