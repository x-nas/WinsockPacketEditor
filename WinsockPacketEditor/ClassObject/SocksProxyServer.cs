
namespace WinsockPacketEditor
{
    public class SocksProxyServer : ProxyAppServer
    {
        public SocksProxyServer() : base(new SocksProxyReceiveFilterFactory())
        {
            //
        }
    }
}
