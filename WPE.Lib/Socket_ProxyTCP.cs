﻿using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;

namespace WPE.Lib
{
    public class Socket_ProxyTCP : IDisposable
    {
        private volatile bool _isDisposed;
        private readonly object _closeLock = new object();

        public Operate.ProxyConfig.SocketProxy.ProxyType ProxyType { get; set; }
        public Operate.ProxyConfig.SocketProxy.ProxyStep ProxyStep { get; set; }
        public Operate.ProxyConfig.SocketProxy.CommandType CommandType { get; set; }
        public Operate.ProxyConfig.SocketProxy.DomainType DomainType { get; set; }
        public Operate.ProxyConfig.SocketProxy.AddressType AddressType { get; set; }
        public Guid AID { get; set; }
        public ClientConnection Client { get; }
        public ServerConnection Server { get; }

        #region//Socket_ProxyTCP

        public Socket_ProxyTCP(Socket clientSocket, int bufferSize)
        {
            Client = new ClientConnection(clientSocket, bufferSize);
            Server = new ServerConnection(bufferSize);
            ProxyStep = Operate.ProxyConfig.SocketProxy.ProxyStep.Handshake;
        }

        #endregion

        #region //TCP 客户端

        public class ClientConnection : IDisposable
        {
            private volatile bool _isDisposed;

            public Socket Socket { get; private set; }
            public IPEndPoint EndPoint { get; set; }
            public string Address { get; set; }
            public byte[] Buffer { get; private set; }
            public byte[] Data { get; set; }

            public ClientConnection(Socket socket, int bufferSize)
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
                            Operate.DoLog(nameof(ClientConnection.Close), ex.Message);
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
                            Operate.DoLog(nameof(ServerConnection.Close), ex.Message);
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