using System.Net.Sockets;

namespace WPELibrary.Lib
{
    public class Socket_ProxyInfo
    {
        public Socket_Cache.SocketProxy.ProxyType ProxyType { get; set; }

        public Socket_Cache.SocketProxy.ProxyStep ProxyStep { get; set; }

        public Socket_Cache.SocketProxy.CommandType CommandType { get; set; }

        public Socket_Cache.SocketProxy.AddressType AddressType { get; set; }      
        
        public Socket_Cache.SocketProxy.DomainType DomainType { get; set; }

        public Socket ClientSocket { get; set; }

        public string ClientAddress { get; set; }

        public byte[] ClientBuffer { get; set; }

        public byte[] ClientData { get; set; }

        public Socket TargetSocket { get; set; }

        public string TargetAddress { get; set; }

        public byte[] TargetBuffer { get; set; }

        public byte[] TargetData { get; set; }
    }
}
