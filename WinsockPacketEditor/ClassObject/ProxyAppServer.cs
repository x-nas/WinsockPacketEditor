using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Buffers;
using System.Reflection;

namespace WinsockPacketEditor
{
    public abstract class ProxyAppServer : AppServer<ProxySession, BinaryRequestInfo>
    {
        private int m_ProxyReceiveBufferSize;
        private ArrayPool<byte> m_BufferPool;

        public ProxyAppServer(IReceiveFilterFactory<BinaryRequestInfo> receiveFilterFactory) : base(receiveFilterFactory)
        {
            //
        }

        #region//初始化缓存池

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            m_ProxyReceiveBufferSize = config.ReceiveBufferSize;
            m_BufferPool = ArrayPool<byte>.Shared;
            return true;
        }

        #endregion

        #region//租用缓存池

        internal ArraySegment<byte> RequestProxyBuffer()
        {
            try
            {
                var buffer = m_BufferPool.Rent(m_ProxyReceiveBufferSize);
                return new ArraySegment<byte>(buffer, 0, m_ProxyReceiveBufferSize);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return new ArraySegment<byte>(new byte[0]);
            }
        }

        #endregion

        #region//归还缓存池

        internal void PushProxyBuffer(ArraySegment<byte> buffer)
        {
            if (buffer.Array != null && buffer.Array.Length >= m_ProxyReceiveBufferSize)
            {
                m_BufferPool.Return(buffer.Array);
            }
        }

        #endregion
    }
}
