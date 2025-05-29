using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;

namespace WPELibrary.Lib
{
    public class Socket_ProxyTCP : IDisposable
    {
        private volatile bool _isDisposed;
        private readonly object _closeLock = new object();

        public Socket_Cache.SocketProxy.ProxyType ProxyType { get; set; }
        public Socket_Cache.SocketProxy.ProxyStep ProxyStep { get; set; }
        public Socket_Cache.SocketProxy.CommandType CommandType { get; set; }
        public Socket_Cache.SocketProxy.DomainType DomainType { get; set; }
        public Socket_Cache.SocketProxy.AddressType AddressType { get; set; }

        public ClientConnection Client { get; }
        public ServerConnection Server { get; }

        #region//Socket_ProxyTCP

        public Socket_ProxyTCP(Socket clientSocket, int bufferSize)
        {
            Client = new ClientConnection(clientSocket, bufferSize);
            Server = new ServerConnection(bufferSize);
            ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Handshake;
        }

        #endregion

        #region //TCP 客户端

        public class ClientConnection : IDisposable
        {
            private volatile bool _isDisposed;

            public Socket Socket { get; private set; }
            public string Address { get; set; }
            public byte[] Buffer { get; private set; }
            public byte[] Data { get; set; }

            public ClientConnection(Socket socket, int bufferSize)
            {
                Socket = socket ?? throw new ArgumentNullException(nameof(socket));
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
                            Socket_Operation.DoLog_Proxy(nameof(ClientConnection.Close), ex.Message);
                        }
                    }
                }
            }

            public void Dispose() => Close();
        }

        #endregion

        #region //TCP 服务端

        public class ServerConnection : IDisposable
        {
            private volatile bool _isDisposed;

            public Socket Socket { get; set; }
            public string Address { get; set; }
            public byte[] Buffer { get; private set; }
            public IPEndPoint EndPoint { get; set; }

            public ServerConnection(int bufferSize)
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
                            Socket_Operation.DoLog_Proxy(nameof(ServerConnection.Close), ex.Message);
                        }
                    }
                }
            }

            public void Dispose() => Close();
        }

        #endregion

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
                    Server?.Close();
                    Client?.Close();
                }
            }
        }

        ~Socket_ProxyTCP()
        {
            Dispose(false);
        }

        #endregion
    }
}