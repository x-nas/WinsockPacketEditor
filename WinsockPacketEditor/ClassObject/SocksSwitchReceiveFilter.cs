using SuperSocket.SocketBase.Protocol;
using System;

namespace WinsockPacketEditor
{
    public class SocksSwitchReceiveFilter : IReceiveFilter<BinaryRequestInfo>
    {
        private ProxySession m_Session;

        public SocksSwitchReceiveFilter(ProxySession session)
        {
            this.m_Session = session;
        }

        public BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int left)
        {
            try
            {
                var session = this.m_Session;
                left = length;

                var version = readBuffer[offset];

                if (version == 0x05)
                {
                    session.SetNextReceiveFilter(new Socks5ProxyReceiveFilter(session));
                }
                else
                {
                    left = 0;
                    State = FilterState.Error;

                    if (Operate.ProxyConfig.Proxy.EnableFireWall && Operate.ProxyConfig.Proxy.FireWall_AutoBlock_UnSupport)
                    {
                        Operate.ProxyConfig.Proxy.AddToBlackList(
                            session.ClientIP, 
                            true, 
                            DateTime.Now.AddMinutes(Operate.ProxyConfig.Proxy.FireWall_AutoBlock_Minutes), 
                            DateTime.Now);
                    }

                    string sLog = string.Format(AntdUI.Localization.Get("SOCKS.Unsupport", "不支持的 Socks 协议: {0} [ {1} ]"), version, session.ClientIP);
                    Operate.DoLog(nameof(Filter), sLog);

                    return null;
                }
            }
            catch (Exception ex)
            {
                left = 0;
                Operate.DoLog(nameof(Filter), ex.Message);
            }            

            return null;
        }

        public int LeftBufferSize
        {
            get { return 0; }
        }

        public IReceiveFilter<BinaryRequestInfo> NextReceiveFilter
        {
            get { return null; }
        }


        public void Reset()
        {
            throw new NotImplementedException();
        }

        public FilterState State { get; private set; }
    }
}
