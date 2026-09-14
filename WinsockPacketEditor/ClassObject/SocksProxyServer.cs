using SuperSocket.SocketBase.Protocol;

namespace WinsockPacketEditor
{
    public class SocksProxyServer : ProxyAppServer
    {
        public SocksProxyServer() : base(new SocksProxyReceiveFilterFactory())
        {
            /*
                过滤器切出来的每一帧都从这里进会话。
                这是 SuperSocket 认可的收帧口：AppServerBase.ExecuteCommand 调这个处理器之前会
                把 session.LastActiveTime 更新成当前时间，空闲清理因此能看到「客户端还在发」。
                处理器本身不能阻塞（跑在接收的 IOCP 线程上）—— 会话那边只是入队 / 内联转发。
            */
            this.NewRequestReceived += OnFrame;
        }

        private static void OnFrame(ProxySession session, BinaryRequestInfo frame)
        {
            session.OnFrame(frame);
        }
    }
}
