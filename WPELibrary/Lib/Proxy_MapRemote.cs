
namespace WPELibrary.Lib
{
    public class Proxy_MapRemote
    {
        //是否启用
        public bool IsEnable { get; set; }

        #region//请求地址

        //协议类型
        public Socket_Cache.SocketProxy.MapProtocol ProtocolType_From { get; set; }

        //主机地址
        public string Host_From { get; set; }

        //主机端口
        public int Port_From { get; set; }

        //路径地址
        public string Path_From { get; set; }

        #endregion

        #region//映射地址

        //协议类型
        public Socket_Cache.SocketProxy.MapProtocol ProtocolType_To { get; set; }

        //主机地址
        public string Host_To { get; set; }

        //主机端口
        public int Port_To { get; set; }

        //路径地址
        public string Path_To { get; set; }

        #endregion

        #region//Proxy_MapRemote

        public Proxy_MapRemote(
            bool IsEnable, 
            Socket_Cache.SocketProxy.MapProtocol ProtocolType_From, 
            string Host_From, 
            int Port_From, 
            string Path_From,
            Socket_Cache.SocketProxy.MapProtocol ProtocolType_To,
            string Host_To,
            int Port_To,
            string Path_To) 
        {
            this.IsEnable = IsEnable;
            this.ProtocolType_From = ProtocolType_From;
            this.Host_From = Host_From;
            this.Port_From = Port_From;
            this.Path_From = Path_From;
            this.ProtocolType_To = ProtocolType_To;
            this.Host_To = Host_To;
            this.Port_To = Port_To;
            this.Path_To = Path_To;           
        }

        #endregion
    }
}
