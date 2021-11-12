using System.Runtime.InteropServices;
using System;

namespace WPELibrary.Lib
{
    public class SocketOperation
    {
        public bool Filter_Size = false;
        public bool Filter_Socket = false;
        public bool Filter_IP = false;
        public bool Filter_Packet = false;

        public string Filter_Size_From = "";
        public string Filter_Size_To = "";
        public string Filter_Socket_txt = "";
        public string Filter_IP_txt = "";
        public string Filter_Packet_txt = "";

        #region//ws2_32.dll API        

        [DllImport("ws2_32.dll")]
        private static extern int getsockname(int s, ref SocketPacket.sockaddr Address, ref int namelen);

        [DllImport("ws2_32.dll")]
        private static extern int getpeername(int s, ref SocketPacket.sockaddr Address, ref int namelen);

        [DllImport("ws2_32.dll")]
        private static extern System.IntPtr inet_ntoa(SocketPacket.in_addr a);

        [DllImport("ws2_32.dll")]
        private static extern ushort ntohs(ushort netshort);

        #endregion

        #region//进制转换        

        public string Byte_To_Hex(byte[] buffer)
        {
            string sReturn = "";

            foreach (byte n in buffer)
            {
                sReturn += n.ToString("X2") + " ";
            }

            sReturn.Trim();

            return sReturn;
        }

        public byte[] Hex_To_Byte(string hexString)
        {
            hexString = hexString.Replace(" ", "");

            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }

            byte[] returnBytes = new byte[hexString.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = System.Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

        public string Byte_To_Bin(byte[] buffer)
        {
            string sReturn = "";

            foreach (byte n in buffer)
            {
                string strTemp = System.Convert.ToString(n, 2);
                strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));

                sReturn += strTemp + " ";
            }

            sReturn.Trim();

            return sReturn;
        }

        public string Byte_To_UTF8(byte[] buffer)
        {
            string sReturn = "";

            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("utf-8");
            sReturn = chs.GetString(buffer);

            return sReturn;
        }

        public string Byte_To_Unicode(byte[] buffer)
        {
            string sReturn = "";

            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("utf-16");
            sReturn = chs.GetString(buffer);

            return sReturn;
        }

        public string Byte_To_GB2312(byte[] buffer)
        {
            string sReturn = "";

            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");
            sReturn = chs.GetString(buffer);

            return sReturn;
        }

        public string Byte_To_Default(byte[] buffer)
        {
            string sReturn = "";

            System.Text.Encoding chs = System.Text.Encoding.Default;
            sReturn = chs.GetString(buffer);

            return sReturn;
        }

        public string Byte_To_Dec(byte[] buffer)
        {
            string sReturn = "";

            foreach (byte n in buffer)
            {
                sReturn += n.ToString("D") + " ";
            }

            return sReturn;
        }

        #endregion

        //获取套接字IP地址和端口
        public string GetSocketIP(SocketPacket.in_addr in_Addr, ushort NetPort)
        {
            string sReturn = "", sIP = "", sPort = "";

            sIP = GetAddr_IP(in_Addr);
            sPort = GetAddr_Port(NetPort);

            sReturn = sIP + ":" + sPort;

            return sReturn;
        }

        public string GetSocketIP(int iSocket, string sIP_Type)
        {
            string sReturn = "", sIP = "", sPort = "";

            SocketPacket.sockaddr saSocket = new SocketPacket.sockaddr();
            int iLen = Marshal.SizeOf(saSocket);

            if (sIP_Type.Equals("F"))
            {
                getsockname(iSocket, ref saSocket, ref iLen);
            }
            else if (sIP_Type.Equals("T"))
            {
                getpeername(iSocket, ref saSocket, ref iLen);
            }
            else
            {
                //
            }

            sIP = GetAddr_IP(saSocket.sin_addr);
            sPort = GetAddr_Port(saSocket.sin_port);

            sReturn = sIP + ":" + sPort;

            return sReturn;
        }

        //转换IP地址
        private string GetAddr_IP(SocketPacket.in_addr in_Addr)
        {
            string sReturn = "";

            sReturn = Marshal.PtrToStringAnsi(inet_ntoa(in_Addr));

            return sReturn;
        }

        //转换端口号
        private string GetAddr_Port(ushort NetPort)
        {
            string sReturn = "";

            sReturn = ntohs(NetPort).ToString();

            return sReturn;
        }

        //获取指定位置的16进制数据值
        public string GetValueByIndex_HEX(string sHex, int iIndex)
        {
            string sReturn = "";

            try
            {
                string[] slHex = sHex.Split(' ');

                if (slHex.Length > iIndex)
                {
                    sReturn = slHex[iIndex];
                }
            }
            catch
            { 
                //
            }            

            return sReturn;
        }

        //获取指定步长的16进制数据值
        public string GetValueByLen_HEX(string sHex, int iLen)
        {
            string sReturn = "";

            try
            {
                int iValue_Dec = Convert.ToInt32(sHex, 16);
                iValue_Dec += iLen;

                sReturn = iValue_Dec.ToString("X2");
            }
            catch
            { 
                //
            }

            return sReturn;
        }

        //替换封包指定位置的16进制数据值
        public string ReplaceValueByIndexAndLen_HEX(string sHex, int iIndex, int iLen)
        {
            string sReturn = "";

            try
            {
                string sOldValue = GetValueByIndex_HEX(sHex, iIndex);
                string sNewValue = GetValueByLen_HEX(sOldValue, iLen);

                int iStartIndex = iIndex * 3;

                sHex = sHex.Remove(iStartIndex, 2);
                sHex = sHex.Insert(iStartIndex, sNewValue);

                sReturn = sHex;
            }
            catch
            { 
                //
            }

            return sReturn;
        }

        #region//过滤器

        public bool Filter_Default(SocketPacket s)
        {
            bool bReturn = true;

            //默认过滤 0.0.0.0:0 的封包

            int iSocket = s.Socket;
            string sIP_From = GetSocketIP(iSocket, "F");
            string sIP_To = GetSocketIP(iSocket, "T");

            if (sIP_From.Equals("0.0.0.0:0") || sIP_To.Equals("0.0.0.0:0"))
            {
                return false;
            }

            return bReturn;
        }

        public bool Filter(SocketPacket s)
        {
            bool bReturn = true;

            int iSocket = s.Socket;
            byte[] bBuffer = s.Buffer;
            int iLength = s.Length;

            string sIP_From = GetSocketIP(iSocket, "F");
            string sIP_To = GetSocketIP(iSocket, "T");

            //过滤封包大小
            bool bFilter_Size = DoFilter_Size(iLength);
            if (bFilter_Size)
            {
                return false;
            }

            //过滤套接字
            bool bFilter_Socket = DoFilter_Socket(iSocket);
            if (bFilter_Socket)
            {
                return false;
            }

            //过滤IP地址
            bool bFilter_IP = DoFilter_IP(sIP_From, sIP_To);
            if (bFilter_IP)
            {
                return false;
            }

            //过滤封包内容
            string sPacket = Byte_To_Hex(bBuffer);
            bool bFilter_Packet = DoFilter_Packet(sPacket);
            if (bFilter_Packet)
            {
                return false;
            }

            return bReturn;
        }

        //过滤封包大小
        private bool DoFilter_Size(int iLength)
        {
            bool bReturn = false;

            if (Filter_Size)
            {
                try
                {
                    int iFrom = int.Parse(Filter_Size_From);
                    int iTo = int.Parse(Filter_Size_To);

                    if (iLength >= iFrom && iLength <= iTo)
                    {
                        bReturn = false;
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
                catch
                {
                    //
                }
            }

            return bReturn;
        }

        //过滤套接字
        private bool DoFilter_Socket(int iSocket)
        {
            bool bReturn = false;

            if (Filter_Socket)
            {
                try
                {
                    string[] sSocketArr = Filter_Socket_txt.Split(';');

                    foreach (string sSocket in sSocketArr)
                    {
                        int iSocketCheck = int.Parse(sSocket);

                        if (iSocket == iSocketCheck)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    //
                }
            }

            return bReturn;
        }

        //过滤套接字
        private bool DoFilter_IP(string sIP_From, string sIP_To)
        {
            bool bReturn = false;

            if (Filter_IP)
            {
                try
                {
                    string[] sIPArr = Filter_IP_txt.Split(';');

                    foreach (string sIP in sIPArr)
                    {
                        if (sIP_From.IndexOf(sIP) >= 0 || sIP_To.IndexOf(sIP) >= 0)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    //
                }
            }

            return bReturn;
        }

        //过滤封包内容
        private bool DoFilter_Packet(string sPacket)
        {
            bool bReturn = false;

            if (Filter_Packet)
            {
                try
                {
                    string[] sPacketArr = Filter_Packet_txt.Split(';');

                    foreach (string sPacketCheck in sPacketArr)
                    {
                        if (sPacket.IndexOf(sPacketCheck) >= 0)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    //
                }
            }

            return bReturn;
        }

        #endregion
    }
}
