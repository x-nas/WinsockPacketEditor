using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System.Net;

namespace WinsockPacketEditor
{
    public class SocksProxyReceiveFilterFactory : IReceiveFilterFactory<BinaryRequestInfo>
    {
        public IReceiveFilter<BinaryRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            return new SocksSwitchReceiveFilter((ProxySession)appSession);
        }
    }
}
