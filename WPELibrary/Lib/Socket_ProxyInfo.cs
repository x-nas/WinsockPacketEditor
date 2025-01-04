using System;
using System.Net;
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

        public IPEndPoint TargetEndPoint { get; set; }

        public string TargetAddress { get; set; }

        public byte[] TargetBuffer { get; set; }

        public byte[] TargetData { get; set; }

        public UdpClient ClientUDP { get; set; }

        public IPEndPoint ClientUDP_EndPoint { get; set; }

        public DateTime ClientUDP_Time { get; set; }

        public void CloseTCPClient()
        {
            if (this.ClientSocket != null)
            {
                this.ClientSocket.Close();
                this.ClientSocket = null;
                this.ClientBuffer = null;
                this.ClientData = null;
            }            
        }

        public void CloseTCPTarget()
        {
            if (this.TargetSocket != null)
            {
                this.TargetSocket.Close();
                this.TargetSocket = null;
                this.TargetBuffer = null;
                this.TargetData = null;
            }
        }

        public void CloseUDPClient()
        {
            if (this.ClientUDP != null)
            {
                this.ClientUDP.Close();
                this.ClientUDP = null;
            }
        }
    }
}
