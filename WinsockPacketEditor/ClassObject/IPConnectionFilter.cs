using SuperSocket.SocketBase;
using System;
using System.Net;

namespace WinsockPacketEditor
{
    public class IPConnectionFilter : IConnectionFilter
    {
        public string Name { get; private set; }

        public bool Initialize(string name, IAppServer appServer)
        {
            try
            {
                this.Name = name;

                // 从配置中读取黑名单
                var blackListRanges = "192.168.137.37";
                if (!string.IsNullOrEmpty(blackListRanges))
                {
                    Operate.ProxyConfig.Proxy.lstBlackList = Operate.ProxyConfig.Proxy.ParseIpRanges(blackListRanges);
                }

                // 从配置中读取白名单
                var whiteListRanges = "192.168.137.37";
                if (!string.IsNullOrEmpty(whiteListRanges))
                {
                    Operate.ProxyConfig.Proxy.lstWhiteList = Operate.ProxyConfig.Proxy.ParseIpRanges(whiteListRanges);
                }

                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Initialize), ex.Message);
            }

            return false;            
        }

        public bool AllowConnect(IPEndPoint remoteAddress)
        {
            bool bAllow = true;

            try
            {
                var ip = remoteAddress.Address.ToString();
                var ipValue = Operate.ProxyConfig.Proxy.ConvertIpToLong(ip);

                if (Operate.ProxyConfig.Proxy.WhiteListMode)
                {
                    bAllow = Operate.ProxyConfig.Proxy.IsIpInRanges(ipValue, Operate.ProxyConfig.Proxy.lstWhiteList);
                }
                else
                {
                    bAllow = !Operate.ProxyConfig.Proxy.IsIpInRanges(ipValue, Operate.ProxyConfig.Proxy.lstBlackList);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(AllowConnect), ex.Message);
            }            

            return bAllow;
        }
    }
}
