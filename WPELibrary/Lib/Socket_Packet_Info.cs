
namespace WPELibrary.Lib
{
    public class Socket_Packet_Info : Socket_Packet
    {
        #region//序号
        protected int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        #endregion

        #region//源地址
        protected string from;
        public string From
        {
            get { return from; }
            set { from = value; }
        }
        #endregion

        #region//目的地址
        protected string to;
        public string To
        {
            get { return to; }
            set { to = value; }
        }
        #endregion

        #region//类别（中文）
        protected string type_cn;
        public string Type_CN
        {
            get { return type_cn; }
            set { type_cn = value; }
        }
        #endregion

        #region//封包内容（十六进制）
        protected string data;
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
        #endregion

        #region//Socket_Class_Info
        public Socket_Packet_Info(int index, SocketType type, int socket, string from, string to, int reslen, string data, byte[] buffer)
        {
            this.index = index;
            this.type = type;
            this.type_cn = Socket_Operation.GetSocketType_CN(type);
            this.socket = socket;
            this.from = from;
            this.to = to;
            this.reslen = reslen;
            this.data = data;
            this.buffer = buffer;
        }
        #endregion
    }
}
