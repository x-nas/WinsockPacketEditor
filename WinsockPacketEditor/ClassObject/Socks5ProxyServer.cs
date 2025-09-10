using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace WinsockPacketEditor
{
    public class Socks5ProxyServer : AppServer<Socks5ProxySession, BinaryRequestInfo>
    {
        public Socks5ProxyServer() : base(new DefaultReceiveFilterFactory<Socks5ProxyReceiveFilter, BinaryRequestInfo>())
        {
            //
        }
    }
}
