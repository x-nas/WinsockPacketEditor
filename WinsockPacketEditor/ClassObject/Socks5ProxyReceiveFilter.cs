using SuperSocket.SocketBase.Protocol;

namespace WinsockPacketEditor
{
    public class Socks5ProxyReceiveFilter : IReceiveFilter<BinaryRequestInfo>
    {
        public FilterState State { get; set; }
        public int LeftBufferSize { get; set; }

        public BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;

            if (length <= 0)
                return null;

            // 创建一个新的字节数组来存储数据
            byte[] body = new byte[length];
            System.Buffer.BlockCopy(readBuffer, offset, body, 0, length);

            return new BinaryRequestInfo("SOCKS5", body);
        }        

        public IReceiveFilter<BinaryRequestInfo> NextReceiveFilter { get; set; }

        public void Reset()
        {
            // 重置过滤器状态
            State = FilterState.Normal;
            LeftBufferSize = 0;
            NextReceiveFilter = null;
        }
    }
}
