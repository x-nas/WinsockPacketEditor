using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;

namespace WPE.Lib
{
    public class ProxyExecute : IDisposable
    {
        private volatile bool _isDisposed;
        private readonly object _closeLock = new object();

        public Operate.ProxyConfig.Proxy.ProxyType ProxyType { get; set; }
        public Operate.ProxyConfig.Proxy.ProxyStep ProxyStep { get; set; }
        public Operate.ProxyConfig.Proxy.CommandType CommandType { get; set; }
        public Operate.ProxyConfig.Proxy.DomainType DomainType { get; set; }
        public Operate.ProxyConfig.Proxy.AddressType AddressType { get; set; }
        public Guid AID { get; set; }
        public TCPClient TCP_Client { get; }
        public TCPServer TCP_Server { get; }
        public UDPRelay UDP_Relay { get; }

        #region//ProxyExecute

        public ProxyExecute(Socket clientSocket, int bufferSize)
        {
            TCP_Client = new TCPClient(clientSocket, bufferSize);
            TCP_Server = new TCPServer(bufferSize);
            UDP_Relay = new UDPRelay(new IPEndPoint(IPAddress.Any, 0));

            ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.Handshake;
        }

        #endregion

        public class TCPClient : IDisposable
        {
            private volatile bool _isDisposed;

            public Socket Socket { get; private set; }
            public IPEndPoint EndPoint { get; set; }
            public string Address { get; set; }
            public byte[] Buffer { get; private set; }
            public byte[] Data { get; set; }

            public TCPClient(Socket socket, int bufferSize)
            {
                Socket = socket;
                EndPoint = socket?.RemoteEndPoint as IPEndPoint;
                Buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
                Data = Array.Empty<byte>();
            }

            public void Close()
            {
                if (_isDisposed) return;

                lock (this)
                {
                    if (_isDisposed) return;
                    _isDisposed = true;

                    try
                    {
                        if (Socket != null)
                        {
                            var socket = Socket;
                            Socket = null;

                            try
                            {
                                if (socket.Connected)
                                {
                                    socket.Shutdown(SocketShutdown.Both);
                                }
                            }
                            finally
                            {
                                socket.Close();
                                socket.Dispose();
                            }
                        }

                        var buffer = Buffer;
                        Buffer = null;
                        Socket_Operation.ReturnBuffer(buffer);
                    }
                    catch (SocketException ex) when (Socket_Operation.IsExpectedSocketError(ex.ErrorCode))
                    {
                        // 忽略预期错误
                    }
                    catch (Exception ex)
                    {
                        if (!_isDisposed)
                        {
                            Operate.DoLog(nameof(TCPClient.Close), ex.Message);
                        }
                    }
                }
            }

            public void Dispose() => Close();
        }

        public class TCPServer : IDisposable
        {
            private volatile bool _isDisposed;

            public Socket Socket { get; set; }        
            public string Address { get; set; }
            public byte[] Buffer { get; private set; }
            public IPEndPoint EndPoint { get; set; }

            public TCPServer(int bufferSize)
            {
                Buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            }

            public void Close()
            {
                if (_isDisposed) return;

                lock (this)
                {
                    if (_isDisposed) return;
                    _isDisposed = true;

                    try
                    {
                        if (Socket != null)
                        {
                            var socket = Socket;
                            Socket = null;

                            try
                            {
                                if (socket.Connected)
                                {
                                    socket.Shutdown(SocketShutdown.Both);
                                }
                            }
                            finally
                            {
                                socket.Close();
                                socket.Dispose();
                            }
                        }

                        var buffer = Buffer;
                        Buffer = null;
                        Socket_Operation.ReturnBuffer(buffer);
                    }
                    catch (SocketException ex) when (Socket_Operation.IsExpectedSocketError(ex.ErrorCode))
                    {
                        // 忽略预期错误
                    }
                    catch (Exception ex)
                    {
                        if (!_isDisposed)
                        {
                            Operate.DoLog(nameof(TCPServer.Close), ex.Message);
                        }
                    }
                }
            }

            public void Dispose() => Close();
        }

        public class UDPRelay
        {
            public UdpClient ClientUDP { get; set; }
            public IPEndPoint ClientUDP_EndPoint { get; set; }
            public DateTime ClientUDP_Time { get; set; }
            public bool IsActive { get; private set; }

            public UDPRelay(IPEndPoint UDPClient)
            {
                this.ClientUDP = new UdpClient(UDPClient);
                this.ClientUDP_Time = DateTime.Now;
                this.IsActive = true;
            }

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
        }

        #region //IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            lock (_closeLock)
            {
                if (_isDisposed) return;
                _isDisposed = true;

                if (disposing)
                {
                    TCP_Server?.Close();
                    TCP_Client?.Close();
                }
            }
        }

        ~ProxyExecute()
        {
            Dispose(false);
        }

        #endregion
    }
}