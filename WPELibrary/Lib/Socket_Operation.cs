using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WPELibrary.Lib
{   
    public static class Socket_Operation
    {        
        public static bool bDoLog = true;
        public static DataTable dtSearchFrom = new DataTable();
        public static DataTable dtPacketFormat = new DataTable();

        #region//数据格式转换

        #region//初始化封包数据格式表        

        public static void InitPacketFormat()
        {
            try
            {
                if (dtPacketFormat.Columns.Count == 0)
                {
                    dtPacketFormat.Columns.Add("Key", typeof(string));
                    dtPacketFormat.Columns.Add("Value", typeof(string));
                }

                if (dtPacketFormat.Rows.Count == 0)
                {
                    DataRow drUTF7 = dtPacketFormat.NewRow();
                    drUTF7[0] = "UTF-7";
                    drUTF7[1] = "UTF-7";
                    dtPacketFormat.Rows.Add(drUTF7);

                    DataRow drASCII = dtPacketFormat.NewRow();
                    drASCII[0] = "ASCII";
                    drASCII[1] = "ASCII";
                    dtPacketFormat.Rows.Add(drASCII);                  

                    DataRow drUnicode = dtPacketFormat.NewRow();
                    drUnicode[0] = "UNICODE";
                    drUnicode[1] = "UNICODE";
                    dtPacketFormat.Rows.Add(drUnicode);                    

                    DataRow drUTF8 = dtPacketFormat.NewRow();
                    drUTF8[0] = "UTF-8";
                    drUTF8[1] = "UTF-8";
                    dtPacketFormat.Rows.Add(drUTF8);

                    DataRow drUTF16LE = dtPacketFormat.NewRow();
                    drUTF16LE[0] = "UTF-16-LE";
                    drUTF16LE[1] = "UTF-16（LE）";
                    dtPacketFormat.Rows.Add(drUTF16LE);

                    DataRow drUTF16BE = dtPacketFormat.NewRow();
                    drUTF16BE[0] = "UTF-16-BE";
                    drUTF16BE[1] = "UTF-16（BE）";
                    dtPacketFormat.Rows.Add(drUTF16BE);

                    DataRow drUTF32LE = dtPacketFormat.NewRow();
                    drUTF32LE[0] = "UTF-32-LE";
                    drUTF32LE[1] = "UTF-32（LE）";
                    dtPacketFormat.Rows.Add(drUTF32LE);

                    DataRow drUTF32BE = dtPacketFormat.NewRow();
                    drUTF32BE[0] = "UTF-32-BE";
                    drUTF32BE[1] = "UTF-32（BE）";
                    dtPacketFormat.Rows.Add(drUTF32BE);

                    DataRow drBIN = dtPacketFormat.NewRow();
                    drBIN[0] = "BIN";
                    drBIN[1] = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_86);
                    dtPacketFormat.Rows.Add(drBIN);

                    DataRow drDEC = dtPacketFormat.NewRow();
                    drDEC[0] = "DEC";
                    drDEC[1] = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_87);
                    dtPacketFormat.Rows.Add(drDEC);

                    DataRow drHEX = dtPacketFormat.NewRow();
                    drHEX[0] = "HEX";
                    drHEX[1] = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_85);
                    dtPacketFormat.Rows.Add(drHEX);
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }          
        }

        #endregion

        #region//Byte[]转字符串

        public static string BytesToString(Socket_Cache.SocketPacket.EncodingFormat efFormat, byte[] buffer)
        {
            string sReturn = string.Empty;

            try
            {                
                switch (efFormat)
                {
                    case Socket_Cache.SocketPacket.EncodingFormat.Char:
                        char c = Convert.ToChar(buffer[0]);
                        if ((int)c > 31)
                        {
                            sReturn = c.ToString();
                        }
                        else
                        {
                            sReturn = ".";
                        }
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Byte:
                        sReturn = buffer[0].ToString();
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Short:
                        if (buffer.Length >= 2)
                        {
                            sReturn = BitConverter.ToInt16(buffer, 0).ToString();
                        }
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.UShort:
                        if (buffer.Length >= 2)
                        {
                            sReturn = BitConverter.ToUInt16(buffer, 0).ToString();
                        }
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Int32:
                        if (buffer.Length >= 4)
                        {
                            sReturn = BitConverter.ToInt32(buffer, 0).ToString();
                        }
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.UInt32:
                        if (buffer.Length >= 4)
                        {
                            sReturn = BitConverter.ToUInt32(buffer, 0).ToString();
                        }
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Int64:
                        if (buffer.Length >= 8)
                        {
                            sReturn = BitConverter.ToInt64(buffer, 0).ToString();
                        }
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.UInt64:
                        if (buffer.Length >= 8)
                        {
                            sReturn = BitConverter.ToUInt64(buffer, 0).ToString();
                        }
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Float:
                        if (buffer.Length >= 4)
                        {
                            sReturn = BitConverter.ToSingle(buffer, 0).ToString();
                        }
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Double:
                        if (buffer.Length >= 8)
                        {
                            sReturn = BitConverter.ToDouble(buffer, 0).ToString();
                        }
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Bin:
                        foreach (byte b in buffer)
                        {
                            string strTemp = Convert.ToString(b, 2);
                            strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));
                            sReturn += strTemp + " ";
                        }
                        sReturn = sReturn.Trim();
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Hex:
                        foreach (byte b in buffer)
                        {
                            sReturn += b.ToString("X2") + " ";
                        }
                        sReturn = sReturn.Trim();
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.UTF7:
                        sReturn = Encoding.UTF7.GetString(buffer);
                        break;                 

                    default:
                        sReturn = Encoding.Default.GetString(buffer);
                        break;
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//十六进制字符串转byte[]

        public static byte[] Hex_To_Byte(string hexString)
        {
            try
            {
                hexString = hexString.Replace(" ", "");

                if ((hexString.Length % 2) != 0)
                {
                    hexString += " ";
                }

                byte[] returnBytes = new byte[hexString.Length / 2];

                for (int i = 0; i < returnBytes.Length; i++)
                {
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return returnBytes;
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);

                return null;
            }
        }

        #endregion

        #endregion

        #region//初始化进程支持的Winsock版本

        public static void InitProcessWinSockSupport()
        {
            try
            {
                Socket_Cache.Support_WS1 = false;
                Socket_Cache.Support_WS2 = false;

                ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;

                foreach (ProcessModule module in modules)
                {
                    string sModuleName = module.ModuleName;

                    if (sModuleName.Equals(WSock32.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Socket_Cache.Support_WS1 = true;                        
                    }

                    if (sModuleName.Equals(WS2_32.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Socket_Cache.Support_WS2 = true;                        
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//从内存复制指定长度的字节数组

        public static byte[] GetBytes_FromIntPtr(IntPtr ipBuff, int iLen)
        {
            byte[] bBuffer = new byte[iLen];

            try
            {
                Marshal.Copy(ipBuff, bBuffer, 0, iLen);
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bBuffer;
        }

        #endregion

        #region//设置指针地址位置的字节数据

        public static bool SetByteToIntPtr(byte[] bBuffer, IntPtr ipBuff, int iLen)
        {
            bool bResult = false;

            try
            {
                Marshal.Copy(bBuffer, 0, ipBuff, iLen);
                bResult = true;
            }
            catch
            {
                bResult = false;
            }

            return bResult;
        }

        #endregion

        #region//获取sockaddr对应的IP地址和端口

        public static string GetIPString_BySocketAddr(int pSocket, Socket_Cache.SocketPacket.sockaddr pAddr, Socket_Cache.SocketPacket.PacketType pType)
        {
            string sReturn = "";

            try
            {  
                string sIP_From = string.Empty;
                string sIP_To = string.Empty;

                sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);

                if (pType == Socket_Cache.SocketPacket.PacketType.Send || pType == Socket_Cache.SocketPacket.PacketType.Recv || pType == Socket_Cache.SocketPacket.PacketType.WSASend || pType == Socket_Cache.SocketPacket.PacketType.WSARecv)
                {
                    sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.To);
                }
                else if (pType == Socket_Cache.SocketPacket.PacketType.SendTo || pType == Socket_Cache.SocketPacket.PacketType.RecvFrom || pType == Socket_Cache.SocketPacket.PacketType.WSASendTo || pType == Socket_Cache.SocketPacket.PacketType.WSARecvFrom)
                {
                    sIP_To = Socket_Operation.GetIP_ByAddr(pAddr);
                }

                if (!string.IsNullOrEmpty(sIP_From) && !string.IsNullOrEmpty(sIP_To))
                {
                    sReturn = sIP_From + "|" + sIP_To;
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        public static string GetIP_ByAddr(Socket_Cache.SocketPacket.sockaddr saAddr)
        {
            string sReturn = "";

            try
            {
                string sIP = Marshal.PtrToStringAnsi(WS2_32.inet_ntoa(saAddr.sin_addr));
                string sPort = WS2_32.ntohs(saAddr.sin_port).ToString();

                sReturn = sIP + ":" + sPort;
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        public static string GetIP_BySocket(int iSocket, Socket_Cache.SocketPacket.IPType IPType)
        {
            string sReturn = "";

            try
            {
                Socket_Cache.SocketPacket.sockaddr saAddr = new Socket_Cache.SocketPacket.sockaddr();
                int iAddrLen = Marshal.SizeOf(saAddr);

                switch (IPType)
                {
                    case Socket_Cache.SocketPacket.IPType.From:

                        WS2_32.getsockname(iSocket, ref saAddr, ref iAddrLen);

                        break;

                    case Socket_Cache.SocketPacket.IPType.To:

                        WS2_32.getpeername(iSocket, ref saAddr, ref iAddrLen);

                        break;                    
                }

                sReturn = GetIP_ByAddr(saAddr);
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取IP地址和端口对应的sockaddr

        public static Socket_Cache.SocketPacket.sockaddr GetSocketAddr_ByIPString(string IPString)
        {
            Socket_Cache.SocketPacket.sockaddr saReturn = new Socket_Cache.SocketPacket.sockaddr();

            try
            {
                if (!string.IsNullOrEmpty(IPString) && IPString.IndexOf(":") > 0)
                {                    
                    string sIP = IPString.Split(':')[0];
                    string sPort = IPString.Split(':')[1];

                    saReturn.sin_family = ((short)AddressFamily.InterNetwork);
                    saReturn.sin_port = WS2_32.htons(ushort.Parse(sPort));

                    Socket_Cache.SocketPacket.in_addr ia = new Socket_Cache.SocketPacket.in_addr();
                    IPAddress ipAddress = IPAddress.Parse(sIP);
                    ia._S_un.S_addr = ((uint)ipAddress.GetHashCode());

                    saReturn.sin_addr = ia;
                    saReturn.sin_zero = new byte[8];
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return saReturn;
        }

        #endregion

        #region//获取封包数据字符串（十六进制）

        public static string GetPacketData_Hex(byte[] bBuff, int Max_DataLen)
        {
            string sReturn = "";

            try
            {
                int iPacketLen = bBuff.Length;

                if (iPacketLen > Max_DataLen)
                {
                    byte[] bTemp = new byte[Max_DataLen];

                    for (int j = 0; j < Max_DataLen; j++)
                    {
                        bTemp[j] = bBuff[j];
                    }

                    sReturn = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bTemp) + " ...";
                }
                else
                {
                    sReturn = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuff);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取WSABUF数组的字节数组        

        public static byte[] GetByteFromWSABUF(IntPtr lpBuffers, Int32 dwBufferCount, int BytesCNT)
        {
            byte[] bReturn = new byte[0];

            try
            {
                int BytesLeft = BytesCNT;

                for (int i = 0; i < dwBufferCount; i++)
                {
                    if (BytesLeft > 0)
                    {
                        IntPtr lpNewBuffer = IntPtr.Add(lpBuffers, Marshal.SizeOf(typeof(Socket_Cache.SocketPacket.WSABUF)) * i);
                        Socket_Cache.SocketPacket.WSABUF wsBuffer = Marshal.PtrToStructure<Socket_Cache.SocketPacket.WSABUF>(lpNewBuffer);

                        int iBuffLen = 0;

                        if (wsBuffer.len >= BytesLeft)
                        {
                            iBuffLen = BytesLeft;
                        }
                        else
                        {
                            iBuffLen = wsBuffer.len;
                        }

                        BytesLeft -= iBuffLen;

                        byte[] bBuff = new byte[iBuffLen];
                        Marshal.Copy(wsBuffer.buf, bBuff, 0, iBuffLen);

                        bReturn = bReturn.Concat(bBuff).ToArray();                        
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion        

        #region//获取指定步长的Byte

        public static byte GetStepByte(byte bStepByte, int iStepLen)
        {
            byte bReturn = byte.MinValue;

            try
            {
                int iStepValue = Convert.ToInt32(bStepByte);
                iStepValue += iStepLen;

                bReturn = Convert.ToByte(iStepValue);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//获取递进后的Byte[]

        public static byte[] GetStepBytes(byte[] bStepBuffer, int iStepPosition, int iStepLen)
        {
            byte[] bReturn = null;

            try
            {
                byte bStepByte = bStepBuffer[iStepPosition];

                bStepBuffer[iStepPosition] = GetStepByte(bStepByte, iStepLen);

                bReturn = bStepBuffer;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//获取封包类型的图标

        public static Image GetImg_BySocketType(Socket_Cache.SocketPacket.PacketType socketType)
        {
            Image imgReturn = null;

            try
            {
                switch (socketType)
                {
                    case Socket_Cache.SocketPacket.PacketType.Send:
                        imgReturn = Properties.Resources.sent;
                        break;
                    case Socket_Cache.SocketPacket.PacketType.Recv:
                        imgReturn = Properties.Resources.received;
                        break;
                    case Socket_Cache.SocketPacket.PacketType.SendTo:
                        imgReturn = Properties.Resources.sent;
                        break;
                    case Socket_Cache.SocketPacket.PacketType.RecvFrom:
                        imgReturn = Properties.Resources.received;
                        break;
                    case Socket_Cache.SocketPacket.PacketType.WSASend:
                        imgReturn = Properties.Resources.sent;
                        break;
                    case Socket_Cache.SocketPacket.PacketType.WSARecv:
                        imgReturn = Properties.Resources.received;
                        break;
                    case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                        imgReturn = Properties.Resources.sent;
                        break;
                    case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                        imgReturn = Properties.Resources.received;
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return imgReturn;
        }

        #endregion

        #region//是否显示封包（过滤条件）        

        public static bool ISShowSocketPacket_ByFilter(Socket_PacketInfo spi)
        {
            try
            {
                //套接字
                if (Socket_Cache.Check_Socket)
                {
                    if (ISFilter_BySocket(spi.PacketSocket))
                    {                        
                        return false;
                    }
                }

                //IP地址
                if (Socket_Cache.Check_IP)
                {                    
                    if (ISFilter_ByIP(spi.PacketFrom) || ISFilter_ByIP(spi.PacketTo))
                    {                        
                        return false;
                    }
                }

                //封包内容
                if (Socket_Cache.Check_Packet)
                {  
                    if (ISFilter_ByPacket(spi.PacketBuffer))
                    {                        
                        return false;
                    }
                }

                //封包大小
                if (Socket_Cache.Check_Size)
                {
                    if (ISFilter_BySize(spi.PacketLen))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return true;
        }

        #region//检测套接字

        private static bool ISFilter_BySocket(int iSocket)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.txtCheck_Socket))
                {
                    string[] sSocketArr = Socket_Cache.txtCheck_Socket.Split(';');

                    foreach (string sSocket in sSocketArr)
                    {
                        int iSocketCheck = int.Parse(sSocket);

                        if (iSocket == iSocketCheck)
                        {
                            return true;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//检测IP地址

        private static bool ISFilter_ByIP(string sCheckIP)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.txtCheck_IP))
                {
                    string[] sIPArr = Socket_Cache.txtCheck_IP.Split(';');

                    foreach (string sIP in sIPArr)
                    {
                        if (sCheckIP.IndexOf(sIP) >= 0)
                        {
                            return true;
                        }
                    }
                }                    
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//检测封包内容

        private static bool ISFilter_ByPacket(byte[] bBuffer)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.txtCheck_Packet))
                {
                    string sPacket = BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);

                    string[] sPacketArr = Socket_Cache.txtCheck_Packet.Split(';');

                    foreach (string sPacketCheck in sPacketArr)
                    {
                        if (sPacket.IndexOf(sPacketCheck) >= 0)
                        {
                            return true;
                        }
                    }
                }                    
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//检测封包大小
        private static bool ISFilter_BySize(int iLength)
        {
            bool bReturn = false;

            try
            {
                if (iLength >= Socket_Cache.txtCheck_Size_From && iLength <= Socket_Cache.txtCheck_Size_To)
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }
        #endregion

        #endregion

        #region//搜索封包数据        

        public static int FindSocketList(Socket_Cache.SocketPacket.EncodingFormat efFormat, int FromIndex, string SearchData, bool MatchCase)
        {
            int iResult = -1;

            try
            {
                if (!string.IsNullOrEmpty(SearchData))
                {
                    int iListCNT = Socket_Cache.SocketList.lstRecPacket.Count;

                    if (iListCNT > 0 && FromIndex < iListCNT)
                    {
                        string sSearch = "";

                        for (int i = FromIndex; i < iListCNT; i++)
                        {
                            byte[] bSearch = Socket_Cache.SocketList.lstRecPacket[i].PacketBuffer;
                            sSearch = BytesToString(efFormat, bSearch);

                            if (!MatchCase)
                            {
                                sSearch = sSearch.ToLower();
                                SearchData = SearchData.ToLower();
                            }

                            if (sSearch.IndexOf(SearchData) >= 0)
                            {
                                iResult = i;
                                break;
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return iResult;
        }

        #endregion      

        #region//检查是否合法的十六进制
        public static bool CheckHEX(object oHex)
        {
            try
            {
                if (oHex == null)
                { 
                    return false;
                }
                
                if (string.IsNullOrEmpty(oHex.ToString()))
                {
                    return false;
                }

                string sHex = oHex.ToString();

                if (sHex.Length != 2)
                {
                    return false;
                }

                string pattern = "^[A-Fa-f0-9]+$";
                return Regex.IsMatch(sHex, pattern);
            }
            catch (Exception ex) 
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);

                return false;
            }
        }
        #endregion

        #region//显示发送窗体

        public static void ShowSendForm(int iSLIndex)
        {
            try
            {
                if (iSLIndex > -1)
                {
                    Socket_SendForm ssForm = new Socket_SendForm(iSLIndex);
                    ssForm.Show();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示发送列表窗体

        public static void ShowSendListForm()
        {
            try
            {
                if (Socket_Cache.SendList.bShow_SendListForm)
                {
                    Socket_SendListForm sslForm = new Socket_SendListForm();
                    sslForm.Show();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存发送列表数据

        public static void SaveSendListToFile()
        {
            int iSuccess = 0;

            try
            {
                SaveFileDialog sfdSocketInfo = new SaveFileDialog();
                                
                sfdSocketInfo.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_70) + "（*.sp）|*.sp";
                sfdSocketInfo.RestoreDirectory = true;

                if (sfdSocketInfo.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(sfdSocketInfo.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    if (Socket_Cache.SendList.dtSocketSendList.Rows.Count > 0)
                    {
                        for (int i = 0; i < Socket_Cache.SendList.dtSocketSendList.Rows.Count; i++)
                        {
                            try
                            {
                                string sIndex = (i + 1).ToString();
                                string sNote = Socket_Cache.SendList.dtSocketSendList.Rows[i]["Remark"].ToString().Trim();
                                string sSocket = Socket_Cache.SendList.dtSocketSendList.Rows[i]["Socket"].ToString().Trim();
                                string sIPTo = Socket_Cache.SendList.dtSocketSendList.Rows[i]["ToAddress"].ToString().Trim();
                                string sLen = Socket_Cache.SendList.dtSocketSendList.Rows[i]["Len"].ToString().Trim();
                                byte[] bBuffer = (byte[])Socket_Cache.SendList.dtSocketSendList.Rows[i]["Bytes"];
                                string sData = BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);

                                string sSave = sIndex + "|" + sNote + "|" + sSocket + "|" + sIPTo + "|" + sLen + "|" + sData;

                                sw.WriteLine(sSave);

                                iSuccess++;
                            }
                            catch(Exception ex)
                            {                                
                                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                            }
                        }
                    }

                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71), iSuccess));                 
                }
            }
            catch (Exception ex)
            {                
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72) + ex.Message);

                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }                            
        }

        #endregion

        #region//加载发送列表数据

        public static void LoadFileToSendList()
        {
            int iSuccess = 0;

            try
            {
                OpenFileDialog ofdLoadSocket = new OpenFileDialog();

                ofdLoadSocket.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_70) + "（*.sp）|*.sp";
                ofdLoadSocket.RestoreDirectory = true;

                ofdLoadSocket.ShowDialog();
                string filePath = ofdLoadSocket.FileName;

                if (!string.IsNullOrEmpty(filePath))
                {
                    string[] slSocket = File.ReadAllLines(filePath, Encoding.UTF8);

                    Socket_Cache.SendList.dtSocketSendList.Rows.Clear();

                    foreach (string sSocketTemp in slSocket)
                    {
                        try
                        {
                            string[] ss = sSocketTemp.Split('|');

                            int iIndex = int.Parse(ss[0]);
                            string sNote = ss[1];
                            int iSocket = int.Parse(ss[2]);
                            string sIPTo = ss[3];
                            int iResLen = int.Parse(ss[4]);
                            string sData = ss[5];

                            byte[] bBuffer = Hex_To_Byte(sData);

                            Socket_Cache.SendList.AddToSendList_New(iIndex, sNote, iSocket, sIPTo, sData, bBuffer);

                            iSuccess++;
                        }
                        catch (Exception ex)
                        {
                            DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }                       
                    }

                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_73), iSuccess));
                }            
            }
            catch (Exception ex)
            {
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_74) + ex.Message);

                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存滤镜列表数据

        public static void SaveFilterList_Dialog()
        {
            int iSuccess = 0;

            try
            {
                if (Socket_Cache.FilterList.lstFilter.Count > 0)
                {
                    SaveFileDialog sfdSocketInfo = new SaveFileDialog();

                    sfdSocketInfo.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_75) + "（*.fp）|*.fp";
                    sfdSocketInfo.RestoreDirectory = true;

                    if (sfdSocketInfo.ShowDialog() == DialogResult.OK)
                    {
                        iSuccess = SaveFilterList(sfdSocketInfo.FileName);
                        ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71), iSuccess));
                    }
                }  
            }
            catch (Exception ex)
            {
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72) + ex.Message);
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static int SaveFilterList(string FilePath)
        {
            int iReturn = 0;
            string sSave = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\FilterList.fp";
                }

                FileStream fs = new FileStream(FilePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                if (Socket_Cache.FilterList.lstFilter.Count > 0)
                {
                    for (int i = 0; i < Socket_Cache.FilterList.lstFilter.Count; i++)
                    {
                        try
                        {
                            string sFNum = Socket_Cache.FilterList.lstFilter[i].FNum.ToString();
                            string sFName = Socket_Cache.FilterList.lstFilter[i].FName.ToString();
                            string sFMode = ((int)Socket_Cache.FilterList.lstFilter[i].FMode).ToString();
                            string sFAction = ((int)Socket_Cache.FilterList.lstFilter[i].FAction).ToString();
                            string sFFunction = GetFilterFunctionString(Socket_Cache.FilterList.lstFilter[i].FFunction);
                            string sFStartFrom = ((int)Socket_Cache.FilterList.lstFilter[i].FStartFrom).ToString();
                            string sFModifyCNT = Socket_Cache.FilterList.lstFilter[i].FModifyCNT.ToString();
                            string sFSearch = Socket_Cache.FilterList.lstFilter[i].FSearch.ToString();
                            string sModify = Socket_Cache.FilterList.lstFilter[i].FModify.ToString();

                            sSave = sFNum + "|" + sFName + "|" + sFMode + "|" + sFAction + "|" + sFFunction + "|" + sFStartFrom + "|" + sFModifyCNT + "|" + sFSearch + "|" + sModify;

                            sw.WriteLine(sSave);

                            iReturn++;
                        }
                        catch (Exception ex)
                        {
                            DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }
                    }
                }                

                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return iReturn;
        }

        #endregion

        #region//获取滤镜作用类别字符串

        public static string GetFilterFunctionString(Socket_Cache.Filter.FilterFunction FilterFunction)
        { 
            string sReturn = string.Empty;

            try
            {
                sReturn += Convert.ToInt32(FilterFunction.Send) + ":";
                sReturn += Convert.ToInt32(FilterFunction.SendTo) + ":";
                sReturn += Convert.ToInt32(FilterFunction.Recv) + ":";
                sReturn += Convert.ToInt32(FilterFunction.RecvFrom) + ":";
                sReturn += Convert.ToInt32(FilterFunction.WSASend) + ":";
                sReturn += Convert.ToInt32(FilterFunction.WSASendTo) + ":";
                sReturn += Convert.ToInt32(FilterFunction.WSARecv) + ":";
                sReturn += Convert.ToInt32(FilterFunction.WSARecvFrom);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//加载滤镜列表数据

        public static void LoadFilterList_Dialog()
        {
            int iSuccess = 0;

            try
            {
                OpenFileDialog ofdLoadSocket = new OpenFileDialog();

                ofdLoadSocket.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_75) + "（*.fp）|*.fp";
                ofdLoadSocket.RestoreDirectory = true;

                ofdLoadSocket.ShowDialog();
                string filePath = ofdLoadSocket.FileName;

                if (!string.IsNullOrEmpty(filePath))
                {
                    iSuccess = LoadFilterList(filePath);
                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_73), iSuccess));
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_74) + ex.Message);
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static int LoadFilterList(string FilePath)
        {
            int iReturn = 0;            

            try
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\FilterList.fp";
                }

                if (File.Exists(FilePath))
                {
                    string[] slFilter = File.ReadAllLines(FilePath, Encoding.UTF8);
                    
                    if (slFilter.Length > 0)
                    {
                        Socket_Cache.FilterList.FilterListClear();

                        foreach (string sFilterTemp in slFilter)
                        {
                            try
                            {
                                string[] ss = sFilterTemp.Split('|');

                                int iFNum = int.Parse(ss[0]);
                                string sFName = ss[1];
                                Socket_Cache.Filter.FilterMode FilterMode = GetFilterMode_ByString(ss[2]);
                                Socket_Cache.Filter.FilterAction FilterAction = GetFilterAction_ByString(ss[3]);
                                Socket_Cache.Filter.FilterFunction FilterFunction = GetFilterFunction_ByString(ss[4]);
                                Socket_Cache.Filter.FilterStartFrom FilterStartFrom = GetFilterStartFrom_ByString(ss[5]);
                                int iFModifyCNT = int.Parse(ss[6]);
                                string sFSearch = ss[7];
                                string sFModify = ss[8];

                                Socket_Cache.FilterList.AddFilter_New(sFName, FilterMode, FilterAction, FilterFunction, FilterStartFrom, iFModifyCNT, sFSearch, sFModify, false);

                                iReturn++;
                            }
                            catch (Exception ex)
                            {
                                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                            }
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return iReturn;
        }

        #endregion

        #region//获取滤镜字符串

        public static string GetFilterString_ByBytes(byte[] bBuffer)
        {
            string sReturn = "";

            try
            {
                for (int i = 0; i < bBuffer.Length; i++)
                {
                    string sHex = bBuffer[i].ToString("X2");
                    sReturn += i.ToString() + "-" + sHex + ",";
                }

                sReturn = sReturn.Trim(',');
            }
            catch (Exception ex)
            {
                sReturn = "";
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }    

        #endregion

        #region//获取滤镜选项

        public static Socket_Cache.Filter.FilterMode GetFilterMode_ByString(string FilterMode)
        {
            Socket_Cache.Filter.FilterMode FMode;

            try
            {
                FMode = (Socket_Cache.Filter.FilterMode)Enum.Parse(typeof(Socket_Cache.Filter.FilterMode), FilterMode);
            }
            catch (Exception ex)
            {
                FMode = Socket_Cache.Filter.FilterMode.Normal;
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }                       

            return FMode;
        }

        public static Socket_Cache.Filter.FilterAction GetFilterAction_ByString(string FilterAction)
        {
            Socket_Cache.Filter.FilterAction FAction;

            try
            {
                FAction = (Socket_Cache.Filter.FilterAction)Enum.Parse(typeof(Socket_Cache.Filter.FilterAction), FilterAction);
            }
            catch (Exception ex)
            {
                FAction = Socket_Cache.Filter.FilterAction.Replace;
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return FAction;
        }

        public static Socket_Cache.Filter.FilterFunction GetFilterFunction_ByString(string FilterFunction)
        {
            Socket_Cache.Filter.FilterFunction FFunction = new Socket_Cache.Filter.FilterFunction();

            try
            {
                string[] slFilterFunction = FilterFunction.Split(':');                

                FFunction.Send = Convert.ToBoolean(int.Parse(slFilterFunction[0]));
                FFunction.SendTo = Convert.ToBoolean(int.Parse(slFilterFunction[1]));
                FFunction.Recv = Convert.ToBoolean(int.Parse(slFilterFunction[2]));
                FFunction.RecvFrom = Convert.ToBoolean(int.Parse(slFilterFunction[3]));
                FFunction.WSASend = Convert.ToBoolean(int.Parse(slFilterFunction[4]));
                FFunction.WSASendTo = Convert.ToBoolean(int.Parse(slFilterFunction[5]));
                FFunction.WSARecv = Convert.ToBoolean(int.Parse(slFilterFunction[6]));
                FFunction.WSARecvFrom = Convert.ToBoolean(int.Parse(slFilterFunction[7]));
            }
            catch (Exception ex)
            {  
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return FFunction;
        }

        public static Socket_Cache.Filter.FilterStartFrom GetFilterStartFrom_ByString(string sFStartFrom)
        {
            Socket_Cache.Filter.FilterStartFrom FStartFrom;

            try
            {
                FStartFrom = (Socket_Cache.Filter.FilterStartFrom)Enum.Parse(typeof(Socket_Cache.Filter.FilterStartFrom), sFStartFrom);
            }
            catch (Exception ex)
            {
                FStartFrom = Socket_Cache.Filter.FilterStartFrom.Head;
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return FStartFrom;
        }

        #endregion

        #region//显示滤镜窗体（对话框）

        public static void ShowFilterForm_Dialog(int iFNum)
        {
            try
            {
                if (Socket_Cache.FilterList.lstFilter.Count > 0)
                {
                    if (iFNum > 0)
                    {
                        Socket_FilterForm fFilterForm = new Socket_FilterForm(iFNum);
                        fFilterForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//删除滤镜（对话框）

        public static void CleanUpFilterList_Dialog()
        {
            try
            {
                DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_38));

                if (dr.Equals(DialogResult.OK))
                {
                    Socket_Cache.FilterList.FilterListClear();
                    Socket_Operation.SaveFilterList("");
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static void DeleteFilterByFilterNum_Dialog(int iFNum)
        {
            try
            {
                if (iFNum > 0)
                {
                    DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                    if (dr.Equals(DialogResult.OK))
                    {
                        Socket_Cache.FilterList.DeleteFilter_ByFilterNum(iFNum);
                        Socket_Operation.SaveFilterList("");
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//检查滤镜是否生效

        public static bool CheckFilter_IsEffective(Socket_Cache.SocketPacket.PacketType ptType, Socket_FilterInfo sfi)
        {
            bool bResult = false;

            try
            {
                if (sfi.IsEnable)
                {
                    if (CheckFilterFunction_ByPacketType(ptType, sfi.FFunction))
                    {
                        if (!string.IsNullOrEmpty(sfi.FSearch))
                        {
                            bResult = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bResult;
        }

        #endregion

        #region//获取封包类别对应的滤镜作用类别

        public static Socket_Cache.Filter.FilterFunction GetFilterFunction_ByPacketType(Socket_Cache.SocketPacket.PacketType ptType)
        {
            Socket_Cache.Filter.FilterFunction ffReturn = new Socket_Cache.Filter.FilterFunction();

            try
            {
                switch (ptType)
                {
                    case Socket_Cache.SocketPacket.PacketType.Send:
                        ffReturn.Send = true;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.SendTo:
                        ffReturn.SendTo = true;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.Recv:
                        ffReturn.Recv = true;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.RecvFrom:
                        ffReturn.RecvFrom = true;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSASend:
                        ffReturn.WSASend = true;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                        ffReturn.WSASendTo = true;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecv:
                        ffReturn.WSARecv = true;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                        ffReturn.WSARecvFrom = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return ffReturn;
        }

        #endregion

        #region//检查滤镜作用类别

        public static bool CheckFilterFunction_ByPacketType(Socket_Cache.SocketPacket.PacketType ptType, Socket_Cache.Filter.FilterFunction ffFunction)
        { 
            bool bReturn = false;

            try
            {
                switch (ptType)
                {
                    case Socket_Cache.SocketPacket.PacketType.Send:
                        bReturn = ffFunction.Send;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.SendTo:
                        bReturn = ffFunction.SendTo;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.Recv:
                        bReturn = ffFunction.Recv;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.RecvFrom:
                        bReturn = ffFunction.RecvFrom;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSASend:
                        bReturn = ffFunction.WSASend;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                        bReturn = ffFunction.WSASendTo;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecv:
                        bReturn = ffFunction.WSARecv;
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                        bReturn = ffFunction.WSARecvFrom;
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//检查滤镜是否匹配成功（普通滤镜）

        public static bool CheckFilter_IsMatch_Normal(Socket_FilterInfo sfi, IntPtr ipBuff, int iLen)
        {
            bool bResult = true;

            try
            {
                string[] slSearch = sfi.FSearch.Split(',');

                foreach (string s in slSearch)
                {
                    int iIndex = int.Parse(s.Split('-')[0]);
                    string sValue = s.Split('-')[1];

                    if (iIndex >= 0 && iIndex < iLen)
                    {
                        string sBuffValue = Marshal.ReadByte(ipBuff, iIndex).ToString("X2");

                        if (!sValue.Equals(sBuffValue))
                        {
                            bResult = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
                bResult = false;
            }

            return bResult;
        }

        #endregion

        #region//检查滤镜是否匹配成功（高级滤镜）

        public static List<int> CheckFilter_IsMatch_Adcanced(Socket_FilterInfo sfi, IntPtr ipBuff, int iLen)
        {
            List<int> lReturn = new List<int>();

            try
            {
                int iFModifyCNT = sfi.FModifyCNT;

                string[] slSearch = sfi.FSearch.Split(',');

                int iMatchIndex = -1;
                int iBuffIndex = -1;

                for (int i = 0; i < iLen; i++)
                {
                    for (int j = 0; j < slSearch.Length; j++)
                    {
                        int iIndex = int.Parse(slSearch[j].Split('-')[0]);
                        string sValue = slSearch[j].Split('-')[1];

                        iBuffIndex = i + iIndex;

                        if (iBuffIndex >= 0 && iBuffIndex < iLen)
                        {
                            string sBuff_Hex = Marshal.ReadByte(ipBuff, iBuffIndex).ToString("X2");

                            if (sValue.Equals(sBuff_Hex))
                            {
                                iMatchIndex = i;
                            }
                            else
                            {
                                iMatchIndex = -1;
                                break;
                            }
                        }
                    }

                    if (iMatchIndex > -1)
                    {
                        if (iFModifyCNT > 0)
                        {
                            lReturn.Add(iMatchIndex);
                            iFModifyCNT--;

                            i = iBuffIndex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
            }

            return lReturn;
        }

        #endregion

        #region//执行滤镜（普通滤镜）

        public static bool DoFilter_Normal(Socket_FilterInfo sfi, IntPtr ipBuff, int iLen)
        {
            bool bReturn = true;

            try
            {
                if (string.IsNullOrEmpty(sfi.FModify))
                {
                    bReturn = false;
                }
                else
                {
                    string[] slModify = sfi.FModify.Split(',');

                    foreach (string s in slModify)
                    {
                        int iIndex = int.Parse(s.Split('-')[0]);
                        string sValue = s.Split('-')[1];

                        if (iIndex >= 0 && iIndex < iLen)
                        {
                            byte bValue = Convert.ToByte(sValue, 16);
                            Marshal.WriteByte(ipBuff, iIndex, bValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                bReturn = false;
            }

            return bReturn;
        }

        #endregion

        #region//执行滤镜（高级滤镜）

        public static bool DoFilter_Advanced(Socket_FilterInfo sfi, int iMatch, IntPtr ipBuff, int iLen)
        {
            bool bReturn = true;

            try
            {
                if (string.IsNullOrEmpty(sfi.FModify))
                {
                    bReturn = false;
                }
                else
                {
                    string[] slModify = sfi.FModify.Split(',');

                    Socket_Cache.Filter.FilterStartFrom FStartFrom = sfi.FStartFrom;

                    int iBufferIndex = -1;

                    foreach (string s in slModify)
                    {
                        int iIndex = int.Parse(s.Split('-')[0]);
                        string sValue = s.Split('-')[1];

                        switch (FStartFrom)
                        {
                            case Socket_Cache.Filter.FilterStartFrom.Head:
                                iBufferIndex = iIndex;
                                break;

                            case Socket_Cache.Filter.FilterStartFrom.Position:
                                iBufferIndex = iMatch + (iIndex - (Socket_Cache.Filter.FilterSize_MaxLen / 2));
                                break;
                        }

                        if (iBufferIndex >= 0 && iBufferIndex < iLen)
                        {
                            byte bValue = Convert.ToByte(sValue, 16);
                            Marshal.WriteByte(ipBuff, iBufferIndex, bValue);
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                bReturn = false;
            }

            return bReturn;
        }

        #endregion

        #region//保存封包列表为Excel

        public static void SaveSocketListToExcel()
        {
            int iSuccess = 0;

            try
            {
                if (Socket_Cache.SocketList.lstRecPacket.Count > 0)
                {
                    SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                    sfdSaveToExcel.Filter = "Execl files (*.xls)|*.xls";
                    sfdSaveToExcel.FilterIndex = 0;
                    sfdSaveToExcel.RestoreDirectory = true;
                    sfdSaveToExcel.CreatePrompt = true;

                    sfdSaveToExcel.Title = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_76);

                    if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                    {
                        Stream myStream = sfdSaveToExcel.OpenFile();
                        StreamWriter sw = new StreamWriter(myStream, Encoding.GetEncoding(-0));

                        string sColTitle = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_77);
                        sw.WriteLine(sColTitle);

                        foreach (Socket_PacketInfo spi in Socket_Cache.SocketList.lstRecPacket)
                        {
                            try
                            {
                                string sColValue = "";

                                string sTime = spi.PacketTime.ToString("yyyy-MM-dd HH:mm:ss:fff");
                                string sIndex = spi.PacketIndex.ToString();
                                string sType = spi.PacketType.ToString();
                                string sSocket = spi.PacketSocket.ToString();
                                string sFrom = spi.PacketFrom;
                                string sTo = spi.PacketTo;
                                string sLen = spi.PacketLen.ToString();
                                byte[] bBuff = spi.PacketBuffer;
                                string sData = BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuff);

                                sColValue += sTime + "\t" + sIndex + "\t" + sType + "\t" + sSocket + "\t" + sFrom + "\t" + sTo + "\t" + sLen + "\t" + sData + "\t";
                                sw.WriteLine(sColValue);

                                iSuccess++;
                            }
                            catch (Exception ex)
                            {
                                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                            }
                        }

                        sw.Close();
                        myStream.Close();

                        ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71), iSuccess));
                    }
                }
            }
            catch(Exception ex)
            {                
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72) + ex.Message);

                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//保存日志列表为Excel        
        public static void SaveLogListToExcel()
        {
            int iSuccess = 0;

            try
            {
                SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                sfdSaveToExcel.Filter = "Execl files (*.xls)|*.xls";
                sfdSaveToExcel.FilterIndex = 0;
                sfdSaveToExcel.RestoreDirectory = true;
                sfdSaveToExcel.CreatePrompt = true;
                                
                sfdSaveToExcel.Title = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_76);

                if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream = sfdSaveToExcel.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, Encoding.GetEncoding(-0));
                                        
                    string sColTitle = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_78);
                    sw.WriteLine(sColTitle);

                    foreach (Socket_LogInfo sl in Socket_Cache.LogList.lstRecLog)
                    {
                        try
                        {
                            string sColValue = "";

                            string sTime = sl.Time;
                            string sFuncName = sl.FuncName;
                            string sContent = sl.Content;

                            sColValue += sTime + "\t" + sFuncName + "\t" + sContent + "\t";
                            sw.WriteLine(sColValue);

                            iSuccess++;
                        }
                        catch (Exception ex)
                        {
                            DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }
                    }

                    sw.Close();
                    myStream.Close();
                                        
                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71), iSuccess));
                }
            }
            catch (Exception ex)
            {                
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72) + ex.Message);
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion        

        #region//弹出对话框
        public static void ShowMessageBox(string sMessage)
        {
            try
            {                
                MessageBox.Show(sMessage, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_79), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        public static DialogResult ShowSelectMessageBox(string sMessage)
        {
            DialogResult dr = new DialogResult();

            try
            {                
                dr = MessageBox.Show(sMessage, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_79), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return dr;
        }
        #endregion

        #region//记录日志        

        public static void DoLog(string sFuncName, string sLogContent)
        {
            Task.Run(() =>
            {
                if (bDoLog)
                {
                    Socket_Cache.LogQueue.LogToQueue(sFuncName, sLogContent);
                }
            });           
        }
        #endregion

        #region//发送封包

        public static bool SendPacket(int iSocket, Socket_Cache.SocketPacket.PacketType stType, string sIPString, byte[] bSendBuffer)
        {
            bool bReturn = false;

            try
            {
                if (iSocket > 0 && bSendBuffer.Length > 0)
                {
                    IntPtr ipSend = Marshal.AllocHGlobal(bSendBuffer.Length);
                    Marshal.Copy(bSendBuffer, 0, ipSend, bSendBuffer.Length);

                    int res = -1;                                

                    if (stType == Socket_Cache.SocketPacket.PacketType.Send || stType == Socket_Cache.SocketPacket.PacketType.Recv || stType == Socket_Cache.SocketPacket.PacketType.WSASend || stType == Socket_Cache.SocketPacket.PacketType.WSARecv)
                    {
                        res = WS2_32.send(iSocket, ipSend, bSendBuffer.Length, SocketFlags.None);
                    }
                    else if (stType == Socket_Cache.SocketPacket.PacketType.SendTo || stType == Socket_Cache.SocketPacket.PacketType.RecvFrom || stType == Socket_Cache.SocketPacket.PacketType.WSASendTo || stType == Socket_Cache.SocketPacket.PacketType.WSARecvFrom)
                    {
                        Socket_Cache.SocketPacket.sockaddr saAddr = Socket_Operation.GetSocketAddr_ByIPString(sIPString);

                        int iSizeAddr = Marshal.SizeOf(saAddr);
                        IntPtr ipAddr = Marshal.AllocHGlobal(iSizeAddr);

                        Marshal.StructureToPtr<Socket_Cache.SocketPacket.sockaddr>(saAddr, ipAddr, true);

                        res = WS2_32.sendto(iSocket, ipSend, bSendBuffer.Length, SocketFlags.None, ipAddr, iSizeAddr);
                    }

                    if (res > 0)
                    {
                        bReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion
    }
}
