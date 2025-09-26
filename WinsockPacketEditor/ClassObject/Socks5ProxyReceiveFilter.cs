using SuperSocket.SocketBase.Protocol;
using System;
using System.Threading.Tasks;

namespace WinsockPacketEditor
{
    public class Socks5ProxyReceiveFilter : IReceiveFilter<BinaryRequestInfo>
    {        
        private ProxySession m_Session;

        public int LeftBufferSize { get; set; }

        public FilterState State { get; set; }

        public IReceiveFilter<BinaryRequestInfo> NextReceiveFilter { get; set; }

        #region//初始化

        public Socks5ProxyReceiveFilter(ProxySession session)
        {
            this.m_Session = session;            
            this.m_Session.ProxyType = Operate.ProxyConfig.Proxy.ProxyType.Socket5;
            this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.Handshake;
        }

        public void Reset()
        {
            State = FilterState.Normal;
            LeftBufferSize = 0;
            NextReceiveFilter = null;
        }

        #endregion

        #region//处理 Socks5 代理步骤（异步）

        public BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            if (length <= 0)
            {
                return null;
            }                

            byte[] body = new byte[length];
            Buffer.BlockCopy(readBuffer, offset, body, 0, length);

            _ = HandleSocks5Request(body);

            return null;
        }

        private async Task HandleSocks5Request(byte[] bData)
        {
            try
            {
                switch (this.m_Session.ProxyStep)
                {
                    case Operate.ProxyConfig.Proxy.ProxyStep.Handshake:
                        await Operate.ProxyConfig.Proxy.Handshake(this.m_Session, bData);
                        break;

                    case Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName:
                        await Operate.ProxyConfig.Proxy.AuthUserName(this.m_Session, bData);
                        break;

                    case Operate.ProxyConfig.Proxy.ProxyStep.Command:
                        await Operate.ProxyConfig.Proxy.Command(this.m_Session, bData);
                        break;

                    case Operate.ProxyConfig.Proxy.ProxyStep.ForwardData:
                        Operate.ProxyConfig.Proxy.ForwardData(this.m_Session, bData);
                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(HandleSocks5Request), ex.Message);
            }
        }

        #endregion
    }
}
