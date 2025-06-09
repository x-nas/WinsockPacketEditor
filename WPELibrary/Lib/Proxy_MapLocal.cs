
namespace WPELibrary.Lib
{
    public class Proxy_MapLocal
    {
        //是否启用
        public bool IsEnable { get; set; }

        //协议类型
        public Socket_Cache.SocketProxy.MapProtocol ProtocolType { get; set; }

        //主机地址
        public string Host { get; set; }

        //主机端口
        public int Port { get; set; }

        //远程地址
        public string RemotePath { get; set; }

        //本地文件
        public string LocalPath { get; set; }        

        public Proxy_MapLocal(bool IsEnable, Socket_Cache.SocketProxy.MapProtocol ProtocolType, string Host, int Port, string RemotePath, string LocalPath) 
        {
            this.IsEnable = IsEnable;
            this.ProtocolType = ProtocolType;
            this.Host = Host;
            this.Port = Port;
            this.RemotePath = RemotePath;
            this.LocalPath = LocalPath;   
        }
    }
}
