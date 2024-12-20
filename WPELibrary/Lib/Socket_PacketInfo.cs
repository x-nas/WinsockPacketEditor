using System;

namespace WPELibrary.Lib
{
    public class Socket_PacketInfo
    {
        #region//时间戳

        protected DateTime packettime;

        public DateTime PacketTime
        {
            get { return packettime; }
            set { packettime = value; }
        }

        #endregion

        #region//套接字

        protected int packetsocket;

        public int PacketSocket
        {
            get { return packetsocket; }
            set { packetsocket = value; }
        }

        #endregion        

        #region//封包类别

        protected Socket_Cache.SocketPacket.PacketType packettype;

        public Socket_Cache.SocketPacket.PacketType PacketType
        {
            get { return packettype; }
            set { packettype = value; }
        }

        #endregion

        #region//源地址

        protected string packetfrom;

        public string PacketFrom
        {
            get { return packetfrom; }
            set { packetfrom = value; }
        }

        #endregion

        #region//目的地址

        protected string packetto;

        public string PacketTo
        {
            get { return packetto; }
            set { packetto = value; }
        }

        #endregion

        #region//原始封包数据（字节）

        protected byte[] rawbuffer;

        public byte[] RawBuffer
        {
            get { return rawbuffer; }
            set { rawbuffer = value; }
        }

        #endregion

        #region//封包数据（字节）

        protected byte[] packetbuffer;

        public byte[] PacketBuffer
        {
            get { return packetbuffer; }
            set { packetbuffer = value; }
        }

        #endregion

        #region//封包内容（十六进制）

        protected string packetdata;

        public string PacketData
        {
            get { return packetdata; }
            set { packetdata = value; }
        }

        #endregion

        #region//封包长度

        protected int packetlen;

        public int PacketLen
        {
            get { return packetlen; }
            set { packetlen = value; }
        }

        #endregion

        #region//过滤动作

        protected Socket_Cache.Filter.FilterAction filteraction;

        public Socket_Cache.Filter.FilterAction FilterAction
        {
            get { return filteraction; }
            set { filteraction = value; }
        }

        #endregion

        #region//Socket_PacketInfo        

        public Socket_PacketInfo(DateTime pTime, int pSocket, Socket_Cache.SocketPacket.PacketType pType, string pFrom, string pTo, byte[] pRawBuffer, byte[] pBuffer, int pLen, Socket_Cache.Filter.FilterAction pAction)
        {  
            this.packettime = pTime;            
            this.packetsocket = pSocket;          
            this.packettype = pType;
            this.packetfrom = pFrom;
            this.packetto = pTo;
            this.rawbuffer = pRawBuffer;
            this.packetbuffer = pBuffer;
            this.packetlen = pLen;
            this.filteraction = pAction;
        }

        #endregion        
    }
}
