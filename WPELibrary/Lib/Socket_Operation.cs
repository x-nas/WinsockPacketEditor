﻿using System;
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
using static WPELibrary.Lib.Socket_Cache.SocketPacket;

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

        #region//Byte数组转字符串

        public static string BytesToString(EncodingFormat efFormat, byte[] buffer)
        {
            string sReturn = string.Empty;

            try
            {                
                switch (efFormat)
                {
                    case EncodingFormat.Char:
                        char c = Convert.ToChar(buffer[0]);
                        if ((int)c > 31)
                        {
                            sReturn = c.ToString();
                        }
                        break;

                    case EncodingFormat.Byte:
                        sReturn = buffer[0].ToString();
                        break;

                    case EncodingFormat.Short:
                        if (buffer.Length >= 2)
                        {
                            sReturn = BitConverter.ToInt16(buffer, 0).ToString();
                        }
                        break;

                    case EncodingFormat.UShort:
                        if (buffer.Length >= 2)
                        {
                            sReturn = BitConverter.ToUInt16(buffer, 0).ToString();
                        }
                        break;

                    case EncodingFormat.Int32:
                        if (buffer.Length >= 4)
                        {
                            sReturn = BitConverter.ToInt32(buffer, 0).ToString();
                        }
                        break;

                    case EncodingFormat.UInt32:
                        if (buffer.Length >= 4)
                        {
                            sReturn = BitConverter.ToUInt32(buffer, 0).ToString();
                        }
                        break;

                    case EncodingFormat.Int64:
                        if (buffer.Length >= 8)
                        {
                            sReturn = BitConverter.ToInt64(buffer, 0).ToString();
                        }
                        break;

                    case EncodingFormat.UInt64:
                        if (buffer.Length >= 8)
                        {
                            sReturn = BitConverter.ToUInt64(buffer, 0).ToString();
                        }
                        break;

                    case EncodingFormat.Float:
                        if (buffer.Length >= 4)
                        {
                            sReturn = BitConverter.ToSingle(buffer, 0).ToString();
                        }
                        break;

                    case EncodingFormat.Double:
                        if (buffer.Length >= 8)
                        {
                            sReturn = BitConverter.ToDouble(buffer, 0).ToString();
                        }
                        break;

                    case EncodingFormat.Bin:
                        foreach (byte b in buffer)
                        {
                            string strTemp = Convert.ToString(b, 2);
                            strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));
                            sReturn += strTemp + " ";
                        }
                        sReturn = sReturn.Trim();
                        break;

                    case EncodingFormat.Hex:
                        foreach (byte b in buffer)
                        {
                            sReturn += b.ToString("X2") + " ";
                        }
                        sReturn = sReturn.Trim();
                        break;

                    case EncodingFormat.UTF7:
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

        #region//十六进制字符串转字节
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

        #region//获取指针地址指定长度的字节数据
        public static byte[] GetByte_FromIntPtr(IntPtr ipBuff, int iLen)
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

        #region//获取封包的IP地址和端口

        public static string GetSocketPacketIP(int pSocket, Socket_Cache.SocketPacket.sockaddr pAddr, Socket_Cache.SocketPacket.SocketType pType)
        {
            string sReturn = "";

            try
            {
                string sIP_From = "", sIP_To = "";

                switch (pType)
                {
                    case Socket_Cache.SocketPacket.SocketType.Send:                        
                        sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);
                        sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.To);
                        break;
                    case Socket_Cache.SocketPacket.SocketType.SendTo:                        
                        sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);
                        sIP_To = Socket_Operation.GetIP_ByAddr(pAddr);
                        break;
                    case Socket_Cache.SocketPacket.SocketType.Recv:                        
                        sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.To);
                        sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);
                        break;
                    case Socket_Cache.SocketPacket.SocketType.RecvFrom:                        
                        sIP_From = Socket_Operation.GetIP_ByAddr(pAddr);
                        sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);
                        break;                    
                    case Socket_Cache.SocketPacket.SocketType.WSASend:                        
                        sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);
                        sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.To);
                        break;
                    case Socket_Cache.SocketPacket.SocketType.WSASendTo:                        
                        sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);
                        sIP_To = Socket_Operation.GetIP_ByAddr(pAddr);
                        break;
                    case Socket_Cache.SocketPacket.SocketType.WSARecv:                        
                        sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.To);
                        sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);
                        break;
                    case Socket_Cache.SocketPacket.SocketType.WSARecvFrom:                        
                        sIP_From = Socket_Operation.GetIP_ByAddr(pAddr);
                        sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);
                        break;
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
      
        #endregion

        #region//获取指定位置的16进制数据值
        public static string GetValueByIndex_HEX(string sHex, int iIndex)
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
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }
        #endregion

        #region//获取指定步长的16进制数据值
        public static string GetValueByLen_HEX(string sHex, int iLen)
        {
            string sReturn = "";

            try
            {
                int iValue_Dec = Convert.ToInt32(sHex, 16);
                iValue_Dec += iLen;

                sReturn = iValue_Dec.ToString("X2");
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
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

                    sReturn = Socket_Operation.BytesToString(EncodingFormat.Hex, bTemp) + " ...";
                }
                else
                {
                    sReturn = Socket_Operation.BytesToString(EncodingFormat.Hex, bBuff);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取WSABUF数组的字节        

        public static unsafe byte[] GetByteFromWSABUF(IntPtr lpBuffers, Int32 dwBufferCount, int BytesCNT)
        {
            byte[] bByteBuff = new byte[0];

            int BytesLeft = BytesCNT;

            for (int i = 0; i < dwBufferCount; i++)
            {
                Socket_Cache.SocketPacket.WSABUF wsBuffer = (Socket_Cache.SocketPacket.WSABUF)Marshal.PtrToStructure(IntPtr.Add(lpBuffers, sizeof(Socket_Cache.SocketPacket.WSABUF) * i), typeof(Socket_Cache.SocketPacket.WSABUF));
                Socket_Cache.FilterList.DoFilter(wsBuffer.buf, wsBuffer.len);

                if (wsBuffer.len >= BytesLeft)
                {
                    byte[] bBuffer = new byte[BytesLeft];
                    Marshal.Copy(wsBuffer.buf, bBuffer, 0, bBuffer.Length);

                    bByteBuff = bByteBuff.Concat(bBuffer).ToArray();

                    break;
                }
                else
                {
                    byte[] bBuffer = new byte[wsBuffer.len];
                    Marshal.Copy(wsBuffer.buf, bBuffer, 0, bBuffer.Length);

                    bByteBuff = bByteBuff.Concat(bBuffer).ToArray();

                    BytesLeft -= wsBuffer.len;
                }
            }

            return bByteBuff;
        }

        #endregion

        #region//获取封包类型对应的标识图片

        public static Image GetPacketTypeImg(Socket_Cache.SocketPacket.SocketType stType)
        {
            Image imgReturn = null;

            try
            {
                switch (stType)
                {
                    case Socket_Cache.SocketPacket.SocketType.Send:
                        imgReturn = Properties.Resources.sent;
                        break;
                    case Socket_Cache.SocketPacket.SocketType.Recv:
                        imgReturn = Properties.Resources.received;
                        break;
                    case Socket_Cache.SocketPacket.SocketType.SendTo:
                        imgReturn = Properties.Resources.sent;
                        break;
                    case Socket_Cache.SocketPacket.SocketType.RecvFrom:
                        imgReturn = Properties.Resources.received;
                        break;
                    case Socket_Cache.SocketPacket.SocketType.WSASend:
                        imgReturn = Properties.Resources.sent;
                        break;
                    case Socket_Cache.SocketPacket.SocketType.WSARecv:
                        imgReturn = Properties.Resources.received;
                        break;
                    case Socket_Cache.SocketPacket.SocketType.WSASendTo:
                        imgReturn = Properties.Resources.sent;
                        break;
                    case Socket_Cache.SocketPacket.SocketType.WSARecvFrom:
                        imgReturn = Properties.Resources.received;
                        break;
                    default:
                        imgReturn = Properties.Resources.Info16;
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

        #region//替换封包指定位置的16进制数据值
        public static string ReplaceValueByIndexAndLen_HEX(string sHex, int iIndex, int iLen)
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
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
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
                    string sPacket = BytesToString(EncodingFormat.Hex, bBuffer);

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

        public static int FindSocketList(EncodingFormat efFormat, int FromIndex, string SearchData, bool MatchCase)
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

        #region//检查套接字是否正确
        public static int CheckSocket(string sSocket)
        {
            int iResult = 0;

            try
            {
                if (!string.IsNullOrEmpty(sSocket))
                {
                    iResult = int.Parse(sSocket);

                    if (iResult < 0)
                    {
                        iResult = 0;
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
                                string sData = BytesToString(EncodingFormat.Hex, bBuffer);

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

                            Socket_Cache.SendList.AddSendList_New(iIndex, sNote, iSocket, sIPTo, iResLen, sData, bBuffer);

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

        public static void SaveDialog_FilterList()
        {
            int iSuccess = 0;

            try
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
            catch (Exception ex)
            {
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72) + ex.Message);
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static int SaveFilterList(string FilePath)
        {
            int iReturn = 0;

            try
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\FilterList.fp";
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
                            string sFStartFrom = ((int)Socket_Cache.FilterList.lstFilter[i].FStartFrom).ToString();
                            string sFModifyCNT = Socket_Cache.FilterList.lstFilter[i].FModifyCNT.ToString();
                            string sFSearch = Socket_Cache.FilterList.lstFilter[i].FSearch.ToString();
                            string sFSearchLen = Socket_Cache.FilterList.lstFilter[i].FSearchLen.ToString();
                            string sModify = Socket_Cache.FilterList.lstFilter[i].FModify.ToString();
                            string sModifyLen = Socket_Cache.FilterList.lstFilter[i].FModifyLen.ToString();

                            string sSave = sFNum + "|" + sFName + "|" + sFMode + "|" + sFStartFrom + "|" + sFModifyCNT + "|" + sFSearch + "|" + sFSearchLen + "|" + sModify + "|" + sModifyLen;

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

        #region//加载滤镜列表数据

        public static void LoadDialog_FilterList()
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
                    FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\FilterList.fp";
                }

                bool bFilterList = System.IO.File.Exists(FilePath);

                if (bFilterList)
                {
                    string[] slFilter = File.ReadAllLines(FilePath, Encoding.UTF8);

                    Socket_Cache.FilterList.FilterListClear();

                    foreach (string sFilterTemp in slFilter)
                    {
                        try
                        {
                            string[] ss = sFilterTemp.Split('|');

                            int iFNum = int.Parse(ss[0]);
                            string sFName = ss[1];
                            Socket_FilterInfo.FilterMode FMode = GetFilterMode_ByString(ss[2]);
                            Socket_FilterInfo.StartFrom FStartFrom = GetFilterStartFrom_ByString(ss[3]);
                            int iFModifyCNT = int.Parse(ss[4]);
                            string sFSearch = ss[5];
                            int iFSearchLen = int.Parse(ss[6]);
                            string sFModify = ss[7];
                            int iFModifyLen = int.Parse(ss[8]);

                            Socket_Cache.FilterList.AddFilter_New(sFName, FMode, FStartFrom, iFModifyCNT, sFSearch, iFSearchLen, sFModify, iFModifyLen, false);

                            iReturn++;
                        }
                        catch (Exception ex)
                        {
                            DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

        public static string GetFilterString_ByHEX(string sHEX)
        {
            string sReturn = "";

            try
            {
                string[] slHex = sHEX.Split(' ');

                for (int i = 0; i < slHex.Length; i++)
                {
                    string sCell = slHex[i];
                    sReturn += i.ToString() + "-" + sCell + ",";
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

        public static Socket_FilterInfo.FilterMode GetFilterMode_ByString(string sFMode)
        {
            Socket_FilterInfo.FilterMode FMode;

            try
            {
                FMode = (Socket_FilterInfo.FilterMode)Enum.Parse(typeof(Socket_FilterInfo.FilterMode), sFMode);
            }
            catch (Exception ex)
            {
                FMode = Socket_FilterInfo.FilterMode.Normal;
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }                       

            return FMode;
        }        

        public static Socket_FilterInfo.StartFrom GetFilterStartFrom_ByString(string sFStartFrom)
        {
            Socket_FilterInfo.StartFrom FStartFrom;

            try
            {
                FStartFrom = (Socket_FilterInfo.StartFrom)Enum.Parse(typeof(Socket_FilterInfo.StartFrom), sFStartFrom);
            }
            catch (Exception ex)
            {
                FStartFrom = Socket_FilterInfo.StartFrom.Head;
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return FStartFrom;
        }

        #endregion

        #region//保存封包列表为Excel

        public static void SaveSocketListToExcel()
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
                            string sData = BytesToString(EncodingFormat.Hex, bBuff);

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
            try
            {
                if (bDoLog)
                {  
                    Socket_Cache.LogQueue.LogToQueue(sFuncName, sLogContent);
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }           
        }
        #endregion
    }
}
