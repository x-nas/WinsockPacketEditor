
namespace WPELibrary.Lib
{    
    public class SocketInfo
    {
        //序号
        private int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        //类别
        private string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        //套接字
        private int socket;
        public int Socket
        {
            get { return socket; }
            set { socket = value; }
        }

        //源地址
        private string from;
        public string From
        {
            get { return from; }
            set { from = value; }
        }

        //目的地址
        private string to;
        public string To
        {
            get { return to; }
            set { to = value; }
        }

        //长度
        private int length;
        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        //数据
        private string data;
        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        //字节
        private byte[] buffer;
        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }        

        public SocketInfo(int index, string type, int socket, string from, string to, int length, string data, byte[] buffer)
        {
            this.index = index;
            this.type = type;
            this.socket = socket;
            this.from = from;
            this.to = to;
            this.length = length;
            this.data = data;
            this.buffer = buffer;          
        }
    }
}
