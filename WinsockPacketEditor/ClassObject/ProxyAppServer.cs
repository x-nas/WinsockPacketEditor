using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace WinsockPacketEditor
{
    public abstract class ProxyAppServer : AppServer<ProxySession, BinaryRequestInfo>
    {
        public ProxyAppServer(IReceiveFilterFactory<BinaryRequestInfo> receiveFilterFactory) : base(receiveFilterFactory)
        {
            //
        }

        #region//初始化缓存池大小

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            Operate.ProxyConfig.Proxy.ProxyReceiveBufferSize = config.ReceiveBufferSize;
            return true;
        }

        #endregion
    }
}
