using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Net.Sockets;
using System.Reflection;

namespace WinsockPacketEditor
{
    public class ProxySession : AppSession<ProxySession, BinaryRequestInfo>
    {
        public byte[] bBuffer = null;
        public string ClientIP = string.Empty;
        public int ClientPort = 0;
        public string ServerIP = string.Empty;
        public int ServerPort = 0;
        public Guid AID = Guid.Empty;        
        public string ServerAddress = string.Empty;
        public string ClientAddress = string.Empty;
        public Operate.ProxyConfig.Proxy.ProxyStep ProxyStep;
        public Operate.ProxyConfig.Proxy.CommandType CommandType;
        public Operate.ProxyConfig.Proxy.AddressType AddressType;
        public Operate.ProxyConfig.Proxy.DomainType DomainType;
        public Socket TargetSocket = null;

        public Operate.ProxyConfig.Proxy.ProxyType ProxyType { get; internal set; }

        public new ProxyAppServer AppServer
        {
            get
            {
                return (ProxyAppServer)base.AppServer;
            }
        }

        #region//初始化

        protected override void OnSessionStarted()
        {
            try
            {
                base.OnSessionStarted();

                this.ClientIP = this.RemoteEndPoint.Address.ToString();
                this.ClientPort = this.RemoteEndPoint.Port;

                this.bBuffer = AppServer.RequestProxyBuffer();
                this.TargetSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        internal protected new void SetNextReceiveFilter(IReceiveFilter<BinaryRequestInfo> receiveFilter)
        {
            base.SetNextReceiveFilter(receiveFilter);
        }

        #endregion

        #region//记录无法处理的代理数据

        protected override void HandleUnknownRequest(BinaryRequestInfo requestInfo)
        {
            try
            {
                byte[] bData = requestInfo.Body;

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, "无法处理的代理数据：" + Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, bData));
                Close(CloseReason.ProtocolError);
            }
            catch (Exception ex)
            {
                Close(CloseReason.SocketError);
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//客户端断开链接

        protected override void OnSessionClosed(CloseReason reason)
        {
            if (this.bBuffer != null)
            { 
                AppServer.PushProxyBuffer(this.bBuffer);
            }

            if (this.TargetSocket != null)
            {
                this.TargetSocket.Close();            
            }
        }

        #endregion
    }
}
