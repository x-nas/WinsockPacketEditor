using SuperSocket.SocketBase;
using System;
using System.Net;

namespace WinsockPacketEditor
{
    public class IPConnectionFilter : IConnectionFilter
    {
        public string Name { get; private set; }

        #region//初始化

        public bool Initialize(string name, IAppServer appServer)
        {
            try
            {
                this.Name = name;
                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Initialize), ex.Message);
            }

            return false;            
        }

        #endregion

        #region//防火墙过滤

        public bool AllowConnect(IPEndPoint remoteAddress)
        {
            bool bAllow = true;

            try
            {
                if (Operate.ProxyConfig.Proxy.EnableFireWall)
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
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(AllowConnect), ex.Message);
            }            

            return bAllow;
        }

        #endregion
    }
}
