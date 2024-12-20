
using System.Net;

namespace WPELibrary.Lib
{
    public class Socket_ProxyData
    {
        #region//IP地址

        protected IPAddress ipaddress;

        public IPAddress IPAddress
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }

        #endregion        

        #region//域名

        protected string domain;

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        #endregion        

        #region//端口号

        protected ushort port;

        public ushort Port
        {
            get { return port; }
            set { port = value; }
        }

        #endregion        

        #region//数据类别

        protected Socket_Cache.SocketProxy.DataType datatype;

        public Socket_Cache.SocketProxy.DataType DataType
        {
            get { return datatype; }
            set { datatype = value; }
        }

        #endregion        

        #region//数据

        protected byte[] buffer;

        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        #endregion

        #region//Socket_ProxyData

        public Socket_ProxyData(IPAddress IPAddress, string Domain, ushort Port, Socket_Cache.SocketProxy.DataType DataType)
        {
            this.ipaddress = IPAddress;
            this.domain = Domain;
            this.port = Port;
            this.datatype = DataType;         
        }

        #endregion
    }
}
