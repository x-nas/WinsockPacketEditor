using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace WinsockPacketEditor
{
    public class ProxyTCP : IDisposable
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

        #region//ProxyTCP

        public ProxyTCP(Socket clientSocket, int bufferSize)
        {
            try
            {
                TCP_Client = new TCPClient(clientSocket, bufferSize);
                TCP_Server = new TCPServer(bufferSize);                

                ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.Handshake;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//TCPClient

        public class TCPClient : IDisposable
        {
            private volatile bool _isDisposed;

            public Socket Socket { get; private set; }
            public IPEndPoint EndPoint { get; set; }
            public string Address { get; set; }
            public byte[] Buffer { get; private set; }
            public byte[] Data { get; set; }
            public SocketAsyncEventArgs ReceiveArgs { get; set; }

            public TCPClient(Socket socket, int bufferSize)
            {
                try
                {
                    Socket = socket;
                    EndPoint = socket?.RemoteEndPoint as IPEndPoint;
                    Buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
                    Data = Array.Empty<byte>();
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }                
            }

            public void Close()
            {
                try
                {
                    if (_isDisposed) return;

                    lock (this)
                    {
                        if (_isDisposed) return;
                        _isDisposed = true;

                        if (ReceiveArgs != null)
                        {
                            ReceiveArgs.Completed -= Operate.ProxyConfig.Proxy.ClientReceiveCompleted;
                            ReceiveArgs?.Dispose();
                            ReceiveArgs = null;
                        }

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
                            Operate.SystemConfig.ReturnBuffer(buffer);
                        }
                        catch (SocketException ex) when (Operate.PacketConfig.Packet.IsExpectedSocketError(ex.ErrorCode))
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
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }                
            }

            public void Dispose() => Close();
        }

        #endregion

        #region//TCPServer

        public class TCPServer : IDisposable
        {
            private volatile bool _isDisposed;

            public Socket Socket { get; set; }        
            public string Address { get; set; }
            public byte[] Buffer { get; private set; }
            public IPEndPoint EndPoint { get; set; }
            public SocketAsyncEventArgs ReceiveArgs { get; set; }

            public TCPServer(int bufferSize)
            {
                try
                {
                    Buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public void Close()
            {
                try
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

                                if (ReceiveArgs != null)
                                {
                                    ReceiveArgs.Completed -= Operate.ProxyConfig.Proxy.ServerReceiveCompleted;
                                    ReceiveArgs?.Dispose();
                                    ReceiveArgs = null;
                                }

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
                            Operate.SystemConfig.ReturnBuffer(buffer);
                        }
                        catch (SocketException ex) when (Operate.PacketConfig.Packet.IsExpectedSocketError(ex.ErrorCode))
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
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public void Dispose() => Close();
        }

        #endregion        

        #region //IDisposable

        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        protected virtual void Dispose(bool disposing)
        {
            try
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
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        ~ProxyTCP()
        {
            Dispose(false);
        }

        #endregion
    }
}