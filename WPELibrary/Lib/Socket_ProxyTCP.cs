using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace WPELibrary.Lib
{
    public class Socket_ProxyTCP
    {
        public Socket_Cache.SocketProxy.ProxyType ProxyType { get; set; }
        public Socket_Cache.SocketProxy.ProxyStep ProxyStep { get; set; }
        public Socket_Cache.SocketProxy.CommandType CommandType { get; set; }
        public Socket_Cache.SocketProxy.DomainType DomainType { get; set; }
        public Socket_Cache.SocketProxy.AddressType AddressType { get; set; }

        #region//TCP 客户端

        public Socket ClientSocket { get; set; }
        public string ClientAddress { get; set; }
        public byte[] ClientBuffer { get; set; }
        public byte[] ClientData { get; set; }

        public void CloseClientSocket()
        {
            try
            {
                if (this.ClientSocket != null)
                {
                    this.ClientSocket.Shutdown(SocketShutdown.Both);
                    this.ClientSocket.Close();
                    this.ClientSocket = null;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//TCP 服务端

        public Socket ServerSocket { get; set; }        
        public string ServerAddress { get; set; }
        public byte[] ServerBuffer { get; set; }
        public IPEndPoint ServerEndPoint { get; set; }

        public void CloseServerSocket()
        {
            try
            {
                if (this.ServerSocket != null)
                {
                    this.ServerSocket.Shutdown(SocketShutdown.Both);
                    this.ServerSocket.Close();
                    this.ServerSocket = null;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        public Socket_ProxyTCP(Socket clientSocket, int BufferSize)
        {
            this.ClientSocket = clientSocket;

            this.ClientBuffer = new byte[BufferSize];
            this.ServerBuffer = new byte[BufferSize];

            this.ClientData = Array.Empty<byte>();

            this.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Handshake;
        }
    }
}
