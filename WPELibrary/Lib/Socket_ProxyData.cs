
namespace WPELibrary.Lib
{
    public class Socket_ProxyData
    {
        #region//域名

        protected string domain;

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        #endregion        

        #region//域名类别

        protected Socket_Cache.SocketProxy.DomainType domaintype;

        public Socket_Cache.SocketProxy.DomainType DomainType
        {
            get { return domaintype; }
            set { domaintype = value; }
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

        #region//数据类别

        protected Socket_Cache.SocketProxy.DataType datatype;

        public Socket_Cache.SocketProxy.DataType DataType
        {
            get { return datatype; }
            set { datatype = value; }
        }

        #endregion        

        #region//Socket_ProxyData

        public Socket_ProxyData(string Domain, Socket_Cache.SocketProxy.DomainType DomainType, byte[] Buffer, Socket_Cache.SocketProxy.DataType DataType)
        {          
            this.domain = Domain;
            this.domaintype = DomainType;
            this.buffer = Buffer;
            this.datatype = DataType;         
        }

        #endregion
    }
}
