using SuperSocket.SocketBase.Protocol;
using System;
using System.Reflection;

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

                    string sLog = string.Format(AntdUI.Localization.Get("SOCKS.Unsupported", "不支持的 SOCKS 协议版本: {0}"), version);
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);

                    return null;
                }
            }
            catch (Exception ex)
            {
                left = 0;
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
