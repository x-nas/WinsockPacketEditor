using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

namespace WPELibrary.Lib
{   
    public static class Socket_Operation
    {        
        public static bool bDoLog = true;
        public static bool bDoLog_HookTime = true;
        public static DataTable dtSearchFrom = new DataTable();
        public static DataTable dtPacketFormat = new DataTable();

        #region//数据格式转换

        #region//base64 编码，解码

        public static string Base64_Encoding(string sString)
        {
            string sReturn = string.Empty;

            try
            {
                byte[] bBuffer = StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sString);
                sReturn = Convert.ToBase64String(bBuffer);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        public static string Base64_Decoding(string sString)
        {
            string sReturn = string.Empty;

            try
            {
                byte[] bBuffer = Convert.FromBase64String(sString);
                sReturn = Encoding.Default.GetString(bBuffer);
            }
            catch
            {
                //
            }

            return sReturn;
        }

        #endregion

        #region//字符串转byte[]

        public static byte[] StringToBytes(Socket_Cache.SocketPacket.EncodingFormat efFormat, string sString)
        {
            byte[] bReturn = new byte[sString.Length];

            try
            {
                switch (efFormat)
                {
                    case Socket_Cache.SocketPacket.EncodingFormat.Default:
                        bReturn = Encoding.Default.GetBytes(sString);
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Hex:
                        bReturn = Hex_To_Bytes(sString);
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.GBK:
                        bReturn = Encoding.GetEncoding("GBK").GetBytes(sString);
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.Unicode:
                        bReturn = Encoding.Unicode.GetBytes(sString);
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.UTF7:
                        bReturn = Encoding.UTF7.GetBytes(sString);
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.UTF8:
                        bReturn = Encoding.UTF8.GetBytes(sString);
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.UTF16:
                        bReturn = Encoding.BigEndianUnicode.GetBytes(sString);
                        break;

                    case Socket_Cache.SocketPacket.EncodingFormat.UTF32:
                        bReturn = Encoding.UTF32.GetBytes(sString);
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

        #region//byte[]转字符串

        public static string BytesToString(Socket_Cache.SocketPacket.EncodingFormat efFormat, byte[] buffer)
        {
            string sReturn = string.Empty;

            try
            {
                if (buffer.Length > 0)
                {
                    switch (efFormat)
                    {
                        case Socket_Cache.SocketPacket.EncodingFormat.Default:
                            sReturn = Encoding.Default.GetString(buffer);
                            break;

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

                        case Socket_Cache.SocketPacket.EncodingFormat.Bytes:

                            foreach (byte b in buffer)
                            {
                                sReturn += Convert.ToInt32(b) + ",";
                            }

                            sReturn = sReturn.TrimEnd(',');
                            sReturn = string.Format("{{{0}}}", sReturn);

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

                        case Socket_Cache.SocketPacket.EncodingFormat.GBK:
                            sReturn = Encoding.GetEncoding("GBK").GetString(buffer);
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Unicode:
                            sReturn = Encoding.Unicode.GetString(buffer);
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.ASCII:
                            sReturn = Encoding.ASCII.GetString(buffer);
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UTF7:
                            sReturn = Encoding.UTF7.GetString(buffer);
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UTF8:
                            sReturn = Encoding.UTF8.GetString(buffer);
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UTF16:
                            sReturn = Encoding.BigEndianUnicode.GetString(buffer);
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UTF32:
                            sReturn = Encoding.UTF32.GetString(buffer);
                            break;                    
                    }
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

        public static byte[] Hex_To_Bytes(string hexString)
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
            catch
            {  
                return new byte[0];
            }
        }

        #endregion

        #region//判断是否十六进制字符串（带空格）

        public static bool IsHexString(string value)
        {
            bool bReturn = false;

            try
            {  
                string pattern = @"^([A-Fa-f0-9]{2}\s?)+$";

                bReturn = Regex.IsMatch(value, pattern);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;            
        }

        #endregion

        #endregion

        #region//统计封包数量

        public static void CountSocketInfo(Socket_Cache.SocketPacket.PacketType ptPacketType, int iPacketLen)
        {
            try
            {
                Interlocked.Increment(ref Socket_Cache.TotalPackets);

                switch (ptPacketType)
                {
                    case Socket_Cache.SocketPacket.PacketType.Send:
                        Interlocked.Add(ref Socket_Cache.Total_SendBytes, iPacketLen);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.SendTo:
                        Interlocked.Add(ref Socket_Cache.Total_SendBytes, iPacketLen);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.Recv:
                        Interlocked.Add(ref Socket_Cache.Total_RecvBytes, iPacketLen);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.RecvFrom:
                        Interlocked.Add(ref Socket_Cache.Total_RecvBytes, iPacketLen);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSASend:
                        Interlocked.Add(ref Socket_Cache.Total_SendBytes, iPacketLen);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                        Interlocked.Add(ref Socket_Cache.Total_SendBytes, iPacketLen);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecv:
                        Interlocked.Add(ref Socket_Cache.Total_RecvBytes, iPacketLen);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                        Interlocked.Add(ref Socket_Cache.Total_RecvBytes, iPacketLen);
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//判断文本框输入的字符类型

        public static bool CheckTextInput_IsDigit(char keyChar)
        {
            bool bReturn = false;

            try
            {
                if (char.IsControl(keyChar) || char.IsDigit(keyChar) || keyChar.Equals(';') || keyChar.Equals('.'))
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        public static bool CheckTextInput_IsHex(char keyChar)
        {
            bool bReturn = false;

            try
            {
                if (char.IsControl(keyChar) || Socket_Operation.IsHexChar(keyChar) || keyChar.Equals(' ') || keyChar.Equals(';'))
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        private static bool IsHexChar(char c)
        {
            bool bReturn = false;

            try
            {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

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

        #region//获取封包类型对应的名称

        public static string GetName_ByPacketType(Socket_Cache.SocketPacket.PacketType socketType)
        {
            string sReturn = string.Empty;

            try
            {
                switch (socketType)
                {
                    case Socket_Cache.SocketPacket.PacketType.Send:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_54);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.Recv:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_55);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.SendTo:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_56);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.RecvFrom:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_57);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSASend:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_58);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecv:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_59);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_60);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_61);
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取滤镜动作对应的名称

        public static string GetName_ByFilterAction(Socket_Cache.Filter.FilterAction filterAction)
        {
            string sReturn = string.Empty;

            try
            {
                switch (filterAction)
                {
                    case Socket_Cache.Filter.FilterAction.Replace:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_65);
                        break;

                    case Socket_Cache.Filter.FilterAction.Intercept:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_66);
                        break;

                    case Socket_Cache.Filter.FilterAction.NoModify_Display:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_67);
                        break;

                    case Socket_Cache.Filter.FilterAction.NoModify_NoDisplay:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_68);
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取封包类型对应的图标

        public static Image GetImg_ByPacketType(Socket_Cache.SocketPacket.PacketType socketType)
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

        #region//获取字节长度对应的字符串

        public static string GetDisplayBytes(long size)
        {
            string sReturn = string.Empty;

            try
            {
                const long multi = 1024;
                long kb = multi;
                long mb = kb * multi;
                long gb = mb * multi;
                long tb = gb * multi;

                const string BYTES = "Bytes";
                const string KB = "KB";
                const string MB = "MB";
                const string GB = "GB";
                const string TB = "TB";
                
                if (size < kb)
                {
                    sReturn = string.Format("{0} {1}", size, BYTES);
                }
                else if (size < mb)
                {
                    sReturn = string.Format("{0} {1} ({2} Bytes)", ConvertToOneDigit(size, kb), KB, ConvertBytesDisplay(size));
                }
                else if (size < gb)
                {
                    sReturn = string.Format("{0} {1} ({2} Bytes)", ConvertToOneDigit(size, mb), MB, ConvertBytesDisplay(size));
                }
                else if (size < tb)
                {
                    sReturn = string.Format("{0} {1} ({2} Bytes)", ConvertToOneDigit(size, gb), GB, ConvertBytesDisplay(size));
                }
                else
                {
                    sReturn = string.Format("{0} {1} ({2} Bytes)", ConvertToOneDigit(size, tb), TB, ConvertBytesDisplay(size));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }              

            return sReturn;
        }

        private static string ConvertBytesDisplay(long size)
        {
            string sReturn = string.Empty;

            try
            {
                sReturn = size.ToString("###,###,###,###,###", CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        private static string ConvertToOneDigit(long size, long quan)
        {
            string sReturn = string.Empty;

            try
            {
                double quotient = (double)size / (double)quan;
                sReturn = quotient.ToString("0.#", CultureInfo.CurrentCulture);                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                if (Socket_Cache.CheckSocket)
                {
                    if (Socket_Cache.CheckNotShow)
                    {
                        if (ISFilter_BySocket(spi.PacketSocket))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (ISFilter_BySocket(spi.PacketSocket))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                //IP地址
                if (Socket_Cache.CheckIP)
                {
                    if (Socket_Cache.CheckNotShow)
                    {
                        if (ISFilter_ByIP(spi.PacketFrom) || ISFilter_ByIP(spi.PacketTo))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (ISFilter_ByIP(spi.PacketFrom) || ISFilter_ByIP(spi.PacketTo))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }               
                }

                //端口号
                if (Socket_Cache.CheckPort)
                {
                    if (Socket_Cache.CheckNotShow)
                    {
                        if (ISFilter_ByPort(spi.PacketFrom) || ISFilter_ByPort(spi.PacketTo))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (ISFilter_ByPort(spi.PacketFrom) || ISFilter_ByPort(spi.PacketTo))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                //指定包头
                if (Socket_Cache.CheckHead)
                {
                    if (Socket_Cache.CheckNotShow)
                    {
                        if (ISFilter_ByHead(spi.PacketBuffer))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (ISFilter_ByHead(spi.PacketBuffer))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                //封包内容
                if (Socket_Cache.CheckData)
                {
                    if (Socket_Cache.CheckNotShow)
                    {
                        if (ISFilter_ByPacket(spi.PacketBuffer))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (ISFilter_ByPacket(spi.PacketBuffer))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                //封包大小
                if (Socket_Cache.CheckSize)
                {
                    if (Socket_Cache.CheckNotShow)
                    {
                        if (ISFilter_BySize(spi.PacketLen))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (ISFilter_BySize(spi.PacketLen))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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
                if (!string.IsNullOrEmpty(Socket_Cache.CheckSocket_Value))
                {
                    string[] sSocketArr = Socket_Cache.CheckSocket_Value.Split(';');

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
                if (!string.IsNullOrEmpty(sCheckIP))
                {
                    string sIP = sCheckIP.Split(':')[0];
                    string sPort = sCheckIP.Split(':')[1];
                    
                    if (!string.IsNullOrEmpty(Socket_Cache.CheckIP_Value))
                    {
                        string[] sIPArr = Socket_Cache.CheckIP_Value.Split(';');

                        foreach (string s in sIPArr)
                        {
                            if (sIP.Equals(s))
                            {
                                return true;
                            }
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

        #region//检测端口号

        private static bool ISFilter_ByPort(string sCheckPort)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(sCheckPort))
                {
                    string sIP = sCheckPort.Split(':')[0];
                    string sPort = sCheckPort.Split(':')[1];

                    if (!string.IsNullOrEmpty(Socket_Cache.CheckPort_Value))
                    {
                        string[] sPortArr = Socket_Cache.CheckPort_Value.Split(';');

                        foreach (string s in sPortArr)
                        {
                            if (sPort.Equals(s))
                            {
                                return true;
                            }
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

        #region//检测包头

        private static bool ISFilter_ByHead(byte[] bBuffer)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.CheckHead_Value))
                {
                    string sHead = BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);

                    string[] sHeadArr = Socket_Cache.CheckHead_Value.Split(';');

                    foreach (string sHeadCheck in sHeadArr)
                    {
                        if (sHead.IndexOf(sHeadCheck) == 0)
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
                if (!string.IsNullOrEmpty(Socket_Cache.CheckData_Value))
                {
                    string sPacket = BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);

                    string[] sPacketArr = Socket_Cache.CheckData_Value.Split(';');

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
                if (iLength >= Socket_Cache.CheckSizeFrom_Value && iLength <= Socket_Cache.CheckSizeTo_Value)
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

        public static bool SaveSendList()
        {
            bool bReturn = true;

            try
            {
                SaveFileDialog sfdSocketInfo = new SaveFileDialog();
                                
                sfdSocketInfo.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_70) + "（*.sp）|*.sp";
                sfdSocketInfo.RestoreDirectory = true;

                if (sfdSocketInfo.ShowDialog() == DialogResult.OK)
                {
                    XmlDocument doc = new XmlDocument();

                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                    doc.AppendChild(xmlDeclaration);

                    XmlElement xeSendList = doc.CreateElement("SendList");
                    doc.AppendChild(xeSendList);

                    if (Socket_Cache.SendList.dtSocketSendList.Rows.Count > 0)
                    {
                        for (int i = 0; i < Socket_Cache.SendList.dtSocketSendList.Rows.Count; i++)
                        {
                            string sIndex = (i + 1).ToString();
                            string sNote = Socket_Cache.SendList.dtSocketSendList.Rows[i]["Remark"].ToString().Trim();
                            string sSocket = Socket_Cache.SendList.dtSocketSendList.Rows[i]["Socket"].ToString().Trim();
                            string sIPTo = Socket_Cache.SendList.dtSocketSendList.Rows[i]["ToAddress"].ToString().Trim();
                            string sLen = Socket_Cache.SendList.dtSocketSendList.Rows[i]["Len"].ToString().Trim();
                            byte[] bBuffer = (byte[])Socket_Cache.SendList.dtSocketSendList.Rows[i]["Bytes"];
                            string sData = BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);

                            XmlElement xeSend = doc.CreateElement("Send");
                            xeSendList.AppendChild(xeSend);

                            XmlElement xeIndex = doc.CreateElement("Index");
                            xeIndex.InnerText = sIndex;
                            xeSend.AppendChild(xeIndex);

                            XmlElement xeNote = doc.CreateElement("Note");
                            xeNote.InnerText = sNote;
                            xeSend.AppendChild(xeNote);

                            XmlElement xeSocket = doc.CreateElement("Socket");
                            xeSocket.InnerText = sSocket;
                            xeSend.AppendChild(xeSocket);

                            XmlElement xeIPTo = doc.CreateElement("IPTo");
                            xeIPTo.InnerText = sIPTo;
                            xeSend.AppendChild(xeIPTo);

                            XmlElement xeLen = doc.CreateElement("Len");
                            xeLen.InnerText = sLen;
                            xeSend.AppendChild(xeLen);

                            XmlElement xeData = doc.CreateElement("Data");
                            xeData.InnerText = sData;
                            xeSend.AppendChild(xeData);
                        }
                    }

                    doc.Save(sfdSocketInfo.FileName);
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            
            return bReturn;
        }

        #endregion

        #region//加载发送列表数据

        public static bool LoadSendList()
        {
            bool bReturn = true;

            try
            {
                OpenFileDialog ofdLoadSocket = new OpenFileDialog();

                ofdLoadSocket.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_70) + "（*.sp）|*.sp";
                ofdLoadSocket.RestoreDirectory = true;

                ofdLoadSocket.ShowDialog();
                string FilePath = ofdLoadSocket.FileName;

                if (!string.IsNullOrEmpty(FilePath))
                {  
                    Socket_Cache.SendList.SendListClear();

                    XmlDocument doc = new XmlDocument();
                    doc.Load(FilePath);
                    XmlNode xnSendList = doc.DocumentElement;

                    foreach (XmlNode xnSend in xnSendList.ChildNodes)
                    {
                        string sIndex = xnSend.SelectSingleNode("Index").InnerText;
                        string sNote = xnSend.SelectSingleNode("Note").InnerText;
                        string sSocket = xnSend.SelectSingleNode("Socket").InnerText;
                        string sIPTo = xnSend.SelectSingleNode("IPTo").InnerText;
                        string sLen = xnSend.SelectSingleNode("Len").InnerText;
                        string sData = xnSend.SelectSingleNode("Data").InnerText;

                        int iIndex = int.Parse(sIndex);                        
                        int iSocket = int.Parse(sSocket);                        
                        int iLen = int.Parse(sLen);

                        byte[] bBuffer = StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sData);

                        Socket_Cache.SendList.AddToSendList_New(iIndex, sNote, iSocket, sIPTo, sData, bBuffer);
                    }
                }            
            }
            catch (Exception ex)
            {
                bReturn = false;
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//保存滤镜列表数据

        public static void SaveFilterList_Dialog(string FileName, int FilterIndex)
        {
            try
            {
                if (Socket_Cache.FilterList.lstFilter.Count > 0)
                {
                    SaveFileDialog sfdSocketInfo = new SaveFileDialog();

                    sfdSocketInfo.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_75) + "（*.fp）|*.fp";

                    if (!string.IsNullOrEmpty(FileName))
                    {
                        sfdSocketInfo.FileName = FileName;
                    }
                    
                    sfdSocketInfo.RestoreDirectory = true;

                    if (sfdSocketInfo.ShowDialog() == DialogResult.OK)
                    {
                        bool bOK = SaveFilterList(sfdSocketInfo.FileName, FilterIndex);

                        if (bOK)
                        {
                            ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71));
                        }
                        else
                        {
                            ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72));
                        }
                    }
                }  
            }
            catch (Exception ex)
            {                
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static bool SaveFilterList(string FilePath, int FilterIndex)
        {
            bool bReturn = true;

            try
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\FilterList.fp";
                }

                XmlDocument doc = new XmlDocument();

                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(xmlDeclaration);

                XmlElement xeFilterList = doc.CreateElement("FilterList");
                doc.AppendChild(xeFilterList);

                if (Socket_Cache.FilterList.lstFilter.Count > 0)
                {
                    int Start = 0;
                    int End = Socket_Cache.FilterList.lstFilter.Count;

                    if (FilterIndex > -1 && FilterIndex < End)
                    {
                        Start = FilterIndex;
                        End = FilterIndex + 1;
                    }

                    for (int i = Start; i < End; i++)
                    {
                        string sFNum = Socket_Cache.FilterList.lstFilter[i].FNum.ToString();
                        string sFName = Socket_Cache.FilterList.lstFilter[i].FName;
                        string sFAppointHeader = Socket_Cache.FilterList.lstFilter[i].AppointHeader.ToString();
                        string sFHeaderContent = Socket_Cache.FilterList.lstFilter[i].HeaderContent;
                        string sFMode = ((int)Socket_Cache.FilterList.lstFilter[i].FMode).ToString();
                        string sFAction = ((int)Socket_Cache.FilterList.lstFilter[i].FAction).ToString();
                        string sFFunction = GetFilterFunctionString(Socket_Cache.FilterList.lstFilter[i].FFunction);
                        string sFStartFrom = ((int)Socket_Cache.FilterList.lstFilter[i].FStartFrom).ToString();
                        string sFSearch = Socket_Cache.FilterList.lstFilter[i].FSearch;
                        string sFModify = Socket_Cache.FilterList.lstFilter[i].FModify;

                        XmlElement xeFilter = doc.CreateElement("Filter");
                        xeFilterList.AppendChild(xeFilter);

                        XmlElement xeFNum = doc.CreateElement("Num");
                        xeFNum.InnerText = sFNum;
                        xeFilter.AppendChild(xeFNum);

                        XmlElement xeFName = doc.CreateElement("Name");
                        xeFName.InnerText = sFName;
                        xeFilter.AppendChild(xeFName);

                        XmlElement xeAppointHeader = doc.CreateElement("AppointHeader");
                        xeAppointHeader.InnerText = sFAppointHeader;
                        xeFilter.AppendChild(xeAppointHeader);

                        XmlElement xeHeaderContent = doc.CreateElement("HeaderContent");
                        xeHeaderContent.InnerText = sFHeaderContent;
                        xeFilter.AppendChild(xeHeaderContent);

                        XmlElement xeFMode = doc.CreateElement("Mode");
                        xeFMode.InnerText = sFMode;
                        xeFilter.AppendChild(xeFMode);

                        XmlElement xeFAction = doc.CreateElement("Action");
                        xeFAction.InnerText = sFAction;
                        xeFilter.AppendChild(xeFAction);

                        XmlElement xeFFunction = doc.CreateElement("Function");
                        xeFFunction.InnerText = sFFunction;
                        xeFilter.AppendChild(xeFFunction);

                        XmlElement xeFStartFrom = doc.CreateElement("StartFrom");
                        xeFStartFrom.InnerText = sFStartFrom;
                        xeFilter.AppendChild(xeFStartFrom);

                        XmlElement xeSearch = doc.CreateElement("Search");
                        xeSearch.InnerText = sFSearch;
                        xeFilter.AppendChild(xeSearch);

                        XmlElement xeModify = doc.CreateElement("Modify");
                        xeModify.InnerText = sFModify;
                        xeFilter.AppendChild(xeModify);
                    }
                }

                doc.Save(FilePath);
            }
            catch (Exception ex)
            {
                bReturn = false;
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//加载滤镜列表数据

        public static void LoadFilterList_Dialog()
        {
            try
            {
                OpenFileDialog ofdLoadSocket = new OpenFileDialog();

                ofdLoadSocket.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_75) + "（*.fp）|*.fp";
                ofdLoadSocket.RestoreDirectory = true;

                ofdLoadSocket.ShowDialog();
                string FilePath = ofdLoadSocket.FileName;

                if (!string.IsNullOrEmpty(FilePath))
                {
                    bool bOK = LoadFilterList(FilePath);

                    if (bOK)
                    {
                        ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_73));
                    }
                    else
                    {
                        ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_74));
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static bool LoadFilterList(string FilePath)
        {
            bool bReturn = true;

            try
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\FilterList.fp";
                }

                if (File.Exists(FilePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(FilePath);
                    XmlNode xnFilterList = doc.DocumentElement;

                    foreach (XmlNode xnFilter in xnFilterList.ChildNodes)
                    {
                        string sFNum = xnFilter.SelectSingleNode("Num").InnerText;
                        string sFName = xnFilter.SelectSingleNode("Name").InnerText;
                        string sFAppointHeader = xnFilter.SelectSingleNode("AppointHeader").InnerText;
                        string sFHeaderContent = xnFilter.SelectSingleNode("HeaderContent").InnerText;
                        string sFMode = xnFilter.SelectSingleNode("Mode").InnerText;
                        string sFAction = xnFilter.SelectSingleNode("Action").InnerText;
                        string sFFunction = xnFilter.SelectSingleNode("Function").InnerText;
                        string sFStartFrom = xnFilter.SelectSingleNode("StartFrom").InnerText;                        
                        string sFSearch = xnFilter.SelectSingleNode("Search").InnerText;
                        string sFModify = xnFilter.SelectSingleNode("Modify").InnerText;

                        bool bAppointHeader = bool.Parse(sFAppointHeader);

                        Socket_Cache.Filter.FilterMode FilterMode = GetFilterMode_ByString(sFMode);
                        Socket_Cache.Filter.FilterAction FilterAction = GetFilterAction_ByString(sFAction);
                        Socket_Cache.Filter.FilterFunction FilterFunction = GetFilterFunction_ByString(sFFunction);
                        Socket_Cache.Filter.FilterStartFrom FilterStartFrom = GetFilterStartFrom_ByString(sFStartFrom);

                        Socket_Cache.FilterList.AddFilter_New(sFName, bAppointHeader, sFHeaderContent, FilterMode, FilterAction, FilterFunction, FilterStartFrom, sFSearch, sFModify);
                    }
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
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

        public static Socket_Cache.FilterList.Execute GetFilterListExecute_ByString(string sFLExecute)
        {
            Socket_Cache.FilterList.Execute FLExecute;

            try
            {
                FLExecute = (Socket_Cache.FilterList.Execute)Enum.Parse(typeof(Socket_Cache.FilterList.Execute), sFLExecute);
            }
            catch (Exception ex)
            {
                FLExecute = Socket_Cache.FilterList.Execute.Priority;
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return FLExecute;
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
                    Socket_Operation.SaveFilterList(string.Empty, -1);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static void DeleteFilter_ByFilterNum_Dialog(int iFNum)
        {
            try
            {
                if (iFNum > 0)
                {
                    DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                    if (dr.Equals(DialogResult.OK))
                    {
                        Socket_Cache.FilterList.DeleteFilter_ByFilterNum(iFNum);
                        Socket_Operation.SaveFilterList(string.Empty, -1);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//复制滤镜

        public static int CopyFilter_ByFilterIndex(int iFIndex)
        {
            int iReturn = -1;

            try
            {  
                string FName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_62), Socket_Cache.FilterList.lstFilter[iFIndex].FName);
                bool bAppointHeader = Socket_Cache.FilterList.lstFilter[iFIndex].AppointHeader;
                string HeaderContent = Socket_Cache.FilterList.lstFilter[iFIndex].HeaderContent;
                Socket_Cache.Filter.FilterMode FMode = Socket_Cache.FilterList.lstFilter[iFIndex].FMode;
                Socket_Cache.Filter.FilterAction FAction = Socket_Cache.FilterList.lstFilter[iFIndex].FAction;
                Socket_Cache.Filter.FilterFunction FFunction = Socket_Cache.FilterList.lstFilter[iFIndex].FFunction;
                Socket_Cache.Filter.FilterStartFrom FStartFrom = Socket_Cache.FilterList.lstFilter[iFIndex].FStartFrom;                
                string FSearch = Socket_Cache.FilterList.lstFilter[iFIndex].FSearch;
                string FModify = Socket_Cache.FilterList.lstFilter[iFIndex].FModify;

                Socket_Cache.FilterList.AddFilter_New(FName, bAppointHeader, HeaderContent, FMode, FAction, FFunction, FStartFrom, FSearch, FModify);

                iReturn = Socket_Cache.FilterList.lstFilter.Count - 1;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return iReturn;
        }

        #endregion

        #region//移动滤镜在列表中的位置

        public static int MoveFilter_ByFilterIndex(int iFIndex, Socket_Cache.Filter.FilterMove filterMove)
        {
            int iReturn = -1;

            try
            {
                int iFilterListCount = Socket_Cache.FilterList.lstFilter.Count;

                Socket_FilterInfo sfi = Socket_Cache.FilterList.lstFilter[iFIndex];

                switch (filterMove)
                {
                    case Socket_Cache.Filter.FilterMove.Top:

                        if (iFIndex > 0)
                        {
                            Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                            Socket_Cache.FilterList.lstFilter.Insert(0, sfi);

                            iReturn = 0;
                        }

                        break;

                    case Socket_Cache.Filter.FilterMove.Up:

                        if (iFIndex > 0)
                        {
                            Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                            Socket_Cache.FilterList.lstFilter.Insert(iFIndex - 1, sfi);

                            iReturn = iFIndex - 1;
                        }

                        break;

                    case Socket_Cache.Filter.FilterMove.Down:

                        if (iFIndex < iFilterListCount - 1)
                        {
                            Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                            Socket_Cache.FilterList.lstFilter.Insert(iFIndex + 1, sfi);

                            iReturn = iFIndex + 1;
                        }

                        break;

                    case Socket_Cache.Filter.FilterMove.Bottom:

                        if (iFIndex < iFilterListCount - 1)
                        {
                            Socket_Cache.FilterList.lstFilter.RemoveAt(iFIndex);
                            Socket_Cache.FilterList.lstFilter.Add(sfi);

                            iReturn = Socket_Cache.FilterList.lstFilter.Count - 1;
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return iReturn;
        }

        #endregion

        #region//检查滤镜是否生效

        public static bool CheckFilter_IsEffective(IntPtr ipBuff, int iLen, Socket_Cache.SocketPacket.PacketType ptType, Socket_FilterInfo sfi)
        {
            bool bResult = false;

            try
            {
                if (sfi.IsEnable)
                {
                    if (!string.IsNullOrEmpty(sfi.FSearch))
                    {
                        if (CheckFilterFunction_ByPacketType(ptType, sfi.FFunction))
                        {
                            if (sfi.AppointHeader)
                            {
                                if (CheckPacket_IsMatch_AppointHeader(ipBuff, iLen, sfi.HeaderContent))
                                {
                                    bResult = true;
                                }
                            }
                            else
                            {
                                bResult = true;
                            }
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

        #region//检查封包是否匹配滤镜的指定包头

        public static bool CheckPacket_IsMatch_AppointHeader(IntPtr ipBuff, int iLen, string sHeaderContent)
        {
            bool bResult = false;

            try
            {
                if (!string.IsNullOrEmpty(sHeaderContent))
                {
                    byte[] bHeaderContent = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sHeaderContent);
                    int iHeaderContent_Len = bHeaderContent.Length;

                    if (iHeaderContent_Len > 0 && iHeaderContent_Len <= iLen)
                    {
                        byte[] bPacketHeader = Socket_Operation.GetBytes_FromIntPtr(ipBuff, iHeaderContent_Len);
                        string sPacketHeader = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bPacketHeader);

                        if (sHeaderContent.Equals(sPacketHeader))
                        {
                            bResult = true;
                        }
                    }                    
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_53) + ex.Message);
            }

            return bResult;
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
                    else
                    {
                        bResult = false;
                        break;
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
                byte[] bBUffer = Socket_Operation.GetBytes_FromIntPtr(ipBuff, iLen);

                Dictionary<int, int> dSearchIndex = new Dictionary<int, int>();
                Dictionary<int, byte> dSearchValue = new Dictionary<int, byte>();

                string[] slSearch = sfi.FSearch.Split(',');

                for (int i = 0; i < slSearch.Length; i++)
                {
                    int iIndex = int.Parse(slSearch[i].Split('-')[0]);
                    string sValue = slSearch[i].Split('-')[1];
                    byte bValue = Convert.ToByte(sValue, 16);

                    dSearchIndex.Add(i, iIndex);
                    dSearchValue.Add(i, bValue);
                }

                int iMatchIndex = -1;
                int iBuffIndex = -1;

                byte bFirst_SearchValue = dSearchValue[0];

                for (int i = 0; i < iLen; i++)
                {
                    if (bBUffer[i] == bFirst_SearchValue)
                    {
                        iMatchIndex = i;

                        for (int j = 1; j < slSearch.Length; j++)
                        {
                            int iIndex = dSearchIndex[j];
                            byte bValue = dSearchValue[j];

                            iBuffIndex = i + iIndex;

                            if (iBuffIndex >= 0 && iBuffIndex < iLen)
                            {
                                if (bBUffer[iBuffIndex] != bValue)
                                {
                                    iMatchIndex = -1;
                                    break;
                                }                                
                            }
                            else
                            {
                                iMatchIndex = -1;
                                break;
                            }
                        }

                        if (iMatchIndex > -1)
                        {
                            lReturn.Add(iMatchIndex);

                            if (iBuffIndex > i)
                            {
                                i = iBuffIndex;
                            }
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

                                string sTime = spi.PacketTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
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

                            string sTime = sl.LogTime;
                            string sFuncName = sl.FuncName;
                            string sContent = sl.LogContent;

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

        #region//保存系统设置

        public static bool SaveSystemConfig()
        {
            bool bReturn = true;

            try
            {
                string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\system.config";

                XmlDocument doc = new XmlDocument();

                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(xmlDeclaration);

                XmlElement xeConfig = doc.CreateElement("Config");
                doc.AppendChild(xeConfig);

                #region//过滤设置

                XmlElement xeFilterConfig = doc.CreateElement("FilterConfig");
                xeConfig.AppendChild(xeFilterConfig);

                string sCheckNotShow = Socket_Cache.CheckNotShow.ToString();
                string sCheckSocket = Socket_Cache.CheckSocket.ToString();
                string sCheckIP = Socket_Cache.CheckIP.ToString();
                string sCheckPort = Socket_Cache.CheckPort.ToString();
                string sCheckHead = Socket_Cache.CheckHead.ToString();
                string sCheckData = Socket_Cache.CheckData.ToString();
                string sCheckSize = Socket_Cache.CheckSize.ToString();

                string sCheckSocket_Value = Socket_Cache.CheckSocket_Value;
                string sCheckIP_Value = Socket_Cache.CheckIP_Value;
                string sCheckPort_Value = Socket_Cache.CheckPort_Value;
                string sCheckHead_Value = Socket_Cache.CheckHead_Value;
                string sCheckData_Value = Socket_Cache.CheckData_Value;
                string sCheckSizeFrom_Value = Socket_Cache.CheckSizeFrom_Value.ToString();
                string sCheckSizeTo_Value = Socket_Cache.CheckSizeTo_Value.ToString();

                XmlElement xeCheckNotShow = doc.CreateElement("CheckNotShow");
                xeCheckNotShow.InnerText = sCheckNotShow;
                xeFilterConfig.AppendChild(xeCheckNotShow);

                XmlElement xeCheckSocket = doc.CreateElement("CheckSocket");
                xeCheckSocket.InnerText = sCheckSocket;
                xeFilterConfig.AppendChild(xeCheckSocket);

                XmlElement xeCheckIP = doc.CreateElement("CheckIP");
                xeCheckIP.InnerText = sCheckIP;
                xeFilterConfig.AppendChild(xeCheckIP);

                XmlElement xeCheckPort = doc.CreateElement("CheckPort");
                xeCheckPort.InnerText = sCheckPort;
                xeFilterConfig.AppendChild(xeCheckPort);

                XmlElement xeCheckHead = doc.CreateElement("CheckHead");
                xeCheckHead.InnerText = sCheckHead;
                xeFilterConfig.AppendChild(xeCheckHead);

                XmlElement xeCheckData = doc.CreateElement("CheckData");
                xeCheckData.InnerText = sCheckData;
                xeFilterConfig.AppendChild(xeCheckData);

                XmlElement xeCheckSize = doc.CreateElement("CheckSize");
                xeCheckSize.InnerText = sCheckSize;
                xeFilterConfig.AppendChild(xeCheckSize);

                XmlElement xeCheckSocket_Value = doc.CreateElement("CheckSocket_Value");
                xeCheckSocket_Value.InnerText = sCheckSocket_Value;
                xeFilterConfig.AppendChild(xeCheckSocket_Value);

                XmlElement xeCheckIP_Value = doc.CreateElement("CheckIP_Value");
                xeCheckIP_Value.InnerText = sCheckIP_Value;
                xeFilterConfig.AppendChild(xeCheckIP_Value);

                XmlElement xeCheckPort_Value = doc.CreateElement("CheckPort_Value");
                xeCheckPort_Value.InnerText = sCheckPort_Value;
                xeFilterConfig.AppendChild(xeCheckPort_Value);

                XmlElement xeCheckHead_Value = doc.CreateElement("CheckHead_Value");
                xeCheckHead_Value.InnerText = sCheckHead_Value;
                xeFilterConfig.AppendChild(xeCheckHead_Value);

                XmlElement xeCheckData_Value = doc.CreateElement("CheckData_Value");
                xeCheckData_Value.InnerText = sCheckData_Value;
                xeFilterConfig.AppendChild(xeCheckData_Value);

                XmlElement xeCheckSizeFrom_Value = doc.CreateElement("CheckSizeFrom_Value");
                xeCheckSizeFrom_Value.InnerText = sCheckSizeFrom_Value;
                xeFilterConfig.AppendChild(xeCheckSizeFrom_Value);

                XmlElement xeCheckSizeTo_Value = doc.CreateElement("CheckSizeTo_Value");
                xeCheckSizeTo_Value.InnerText = sCheckSizeTo_Value;
                xeFilterConfig.AppendChild(xeCheckSizeTo_Value);

                #endregion

                #region//拦截设置

                XmlElement xeHookConfig = doc.CreateElement("HookConfig");
                xeConfig.AppendChild(xeHookConfig);

                string sHookSend = Socket_Cache.HookSend.ToString();
                string sHookSendTo = Socket_Cache.HookSendTo.ToString();
                string sHookRecv = Socket_Cache.HookRecv.ToString();
                string sHookRecvFrom = Socket_Cache.HookRecvFrom.ToString();
                string sHookWSASend = Socket_Cache.HookWSASend.ToString();
                string sHookWSASendTo = Socket_Cache.HookWSASendTo.ToString();
                string sHookWSARecv = Socket_Cache.HookWSARecv.ToString();
                string sHookWSARecvFrom = Socket_Cache.HookWSARecvFrom.ToString();

                XmlElement xeHookSend = doc.CreateElement("HookSend");
                xeHookSend.InnerText = sHookSend;
                xeHookConfig.AppendChild(xeHookSend);

                XmlElement xeHookSendTo = doc.CreateElement("HookSendTo");
                xeHookSendTo.InnerText = sHookSendTo;
                xeHookConfig.AppendChild(xeHookSendTo);

                XmlElement xeHookRecv = doc.CreateElement("HookRecv");
                xeHookRecv.InnerText = sHookRecv;
                xeHookConfig.AppendChild(xeHookRecv);

                XmlElement xeHookRecvFrom = doc.CreateElement("HookRecvFrom");
                xeHookRecvFrom.InnerText = sHookRecvFrom;
                xeHookConfig.AppendChild(xeHookRecvFrom);

                XmlElement xeHookWSASend = doc.CreateElement("HookWSASend");
                xeHookWSASend.InnerText = sHookWSASend;
                xeHookConfig.AppendChild(xeHookWSASend);

                XmlElement xeHookWSASendTo = doc.CreateElement("HookWSASendTo");
                xeHookWSASendTo.InnerText = sHookWSASendTo;
                xeHookConfig.AppendChild(xeHookWSASendTo);

                XmlElement xeHookWSARecv = doc.CreateElement("HookWSARecv");
                xeHookWSARecv.InnerText = sHookWSARecv;
                xeHookConfig.AppendChild(xeHookWSARecv);

                XmlElement xeHookWSARecvFrom = doc.CreateElement("HookWSARecvFrom");
                xeHookWSARecvFrom.InnerText = sHookWSARecvFrom;
                xeHookConfig.AppendChild(xeHookWSARecvFrom);

                #endregion

                #region//列表设置

                XmlElement xeListConfig = doc.CreateElement("ListConfig");
                xeConfig.AppendChild(xeListConfig);

                string sSocketList_AutoRoll = Socket_Cache.SocketList.AutoRoll.ToString();
                string sSocketList_AutoClear = Socket_Cache.SocketList.AutoClear.ToString();
                string sSocketList_AutoClear_Value = Socket_Cache.SocketList.AutoClear_Value.ToString();                

                XmlElement xeSocketList_AutoRoll = doc.CreateElement("SocketList_AutoRoll");
                xeSocketList_AutoRoll.InnerText = sSocketList_AutoRoll;
                xeListConfig.AppendChild(xeSocketList_AutoRoll);

                XmlElement xeSocketList_AutoClear = doc.CreateElement("SocketList_AutoClear");
                xeSocketList_AutoClear.InnerText = sSocketList_AutoClear;
                xeListConfig.AppendChild(xeSocketList_AutoClear);

                XmlElement xeSocketList_AutoClear_Value = doc.CreateElement("SocketList_AutoClear_Value");
                xeSocketList_AutoClear_Value.InnerText = sSocketList_AutoClear_Value;
                xeListConfig.AppendChild(xeSocketList_AutoClear_Value);

                string sFilterList_AutoRoll = Socket_Cache.FilterList.AutoRoll.ToString();
                string sFilterList_AutoClear = Socket_Cache.FilterList.AutoClear.ToString();
                string sFilterList_AutoClear_Value = Socket_Cache.FilterList.AutoClear_Value.ToString();

                XmlElement xeFilterList_AutoRoll = doc.CreateElement("FilterList_AutoRoll");
                xeFilterList_AutoRoll.InnerText = sFilterList_AutoRoll;
                xeListConfig.AppendChild(xeFilterList_AutoRoll);

                XmlElement xeFilterList_AutoClear = doc.CreateElement("FilterList_AutoClear");
                xeFilterList_AutoClear.InnerText = sFilterList_AutoClear;
                xeListConfig.AppendChild(xeFilterList_AutoClear);

                XmlElement xeFilterList_AutoClear_Value = doc.CreateElement("FilterList_AutoClear_Value");
                xeFilterList_AutoClear_Value.InnerText = sFilterList_AutoClear_Value;
                xeListConfig.AppendChild(xeFilterList_AutoClear_Value);

                #endregion

                #region//系统设置

                XmlElement xeSystemConfig = doc.CreateElement("SystemConfig");
                xeConfig.AppendChild(xeSystemConfig);

                string sSpeedMode = Socket_Cache.SpeedMode.ToString();
                string sFilterList_Execute = Socket_Cache.FilterList.FilterList_Execute.ToString();             

                XmlElement xeSpeedMode = doc.CreateElement("SpeedMode");
                xeSpeedMode.InnerText = sSpeedMode;
                xeSystemConfig.AppendChild(xeSpeedMode);

                XmlElement xeFilterList_Execute = doc.CreateElement("FilterList_Execute");
                xeFilterList_Execute.InnerText = sFilterList_Execute;
                xeSystemConfig.AppendChild(xeFilterList_Execute);           

                #endregion

                doc.Save(FilePath);
            }
            catch (Exception ex)
            {
                bReturn = false;
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//加载系统设置

        public static bool LoadSystemConfig()
        {
            bool bReturn = true;

            try
            {
                string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\system.config";

                if (File.Exists(FilePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(FilePath);
                    XmlNode xnConfig = doc.DocumentElement;

                    #region//过滤设置

                    XmlNode xnFilterConfig = xnConfig.SelectSingleNode("FilterConfig");

                    string sCheckNotShow = xnFilterConfig.SelectSingleNode("CheckNotShow").InnerText;
                    string sCheckSocket = xnFilterConfig.SelectSingleNode("CheckSocket").InnerText;
                    string sCheckIP = xnFilterConfig.SelectSingleNode("CheckIP").InnerText;
                    string sCheckPort = xnFilterConfig.SelectSingleNode("CheckPort").InnerText;
                    string sCheckHead = xnFilterConfig.SelectSingleNode("CheckHead").InnerText;
                    string sCheckData = xnFilterConfig.SelectSingleNode("CheckData").InnerText;
                    string sCheckSize = xnFilterConfig.SelectSingleNode("CheckSize").InnerText;

                    string sCheckSocket_Value = xnFilterConfig.SelectSingleNode("CheckSocket_Value").InnerText;
                    string sCheckIP_Value = xnFilterConfig.SelectSingleNode("CheckIP_Value").InnerText;
                    string sCheckPort_Value = xnFilterConfig.SelectSingleNode("CheckPort_Value").InnerText;
                    string sCheckHead_Value = xnFilterConfig.SelectSingleNode("CheckHead_Value").InnerText;
                    string sCheckData_Value = xnFilterConfig.SelectSingleNode("CheckData_Value").InnerText;
                    string sCheckSizeFrom_Value = xnFilterConfig.SelectSingleNode("CheckSizeFrom_Value").InnerText;
                    string sCheckSizeTo_Value = xnFilterConfig.SelectSingleNode("CheckSizeTo_Value").InnerText;

                    Socket_Cache.CheckNotShow = bool.Parse(sCheckNotShow);
                    Socket_Cache.CheckSocket = bool.Parse(sCheckSocket);
                    Socket_Cache.CheckIP = bool.Parse(sCheckIP);
                    Socket_Cache.CheckPort = bool.Parse(sCheckPort);
                    Socket_Cache.CheckHead = bool.Parse(sCheckHead);
                    Socket_Cache.CheckData = bool.Parse(sCheckData);
                    Socket_Cache.CheckSize = bool.Parse(sCheckSize);

                    Socket_Cache.CheckSocket_Value = sCheckSocket_Value;
                    Socket_Cache.CheckIP_Value = sCheckIP_Value;
                    Socket_Cache.CheckPort_Value = sCheckPort_Value;
                    Socket_Cache.CheckHead_Value = sCheckHead_Value;
                    Socket_Cache.CheckData_Value = sCheckData_Value;
                    Socket_Cache.CheckSizeFrom_Value = decimal.Parse(sCheckSizeFrom_Value);
                    Socket_Cache.CheckSizeTo_Value = decimal.Parse(sCheckSizeTo_Value);

                    #endregion

                    #region//拦截设置

                    XmlNode xnHookConfig = xnConfig.SelectSingleNode("HookConfig");

                    string sHookSend = xnHookConfig.SelectSingleNode("HookSend").InnerText;
                    string sHookSendTo = xnHookConfig.SelectSingleNode("HookSendTo").InnerText;
                    string sHookRecv = xnHookConfig.SelectSingleNode("HookRecv").InnerText;
                    string sHookRecvFrom = xnHookConfig.SelectSingleNode("HookRecvFrom").InnerText;
                    string sHookWSASend = xnHookConfig.SelectSingleNode("HookWSASend").InnerText;
                    string sHookWSASendTo = xnHookConfig.SelectSingleNode("HookWSASendTo").InnerText;
                    string sHookWSARecv = xnHookConfig.SelectSingleNode("HookWSARecv").InnerText;
                    string sHookWSARecvFrom = xnHookConfig.SelectSingleNode("HookWSARecvFrom").InnerText;

                    Socket_Cache.HookSend = bool.Parse(sHookSend);
                    Socket_Cache.HookSendTo = bool.Parse(sHookSendTo);
                    Socket_Cache.HookRecv = bool.Parse(sHookRecv);
                    Socket_Cache.HookRecvFrom = bool.Parse(sHookRecvFrom);
                    Socket_Cache.HookWSASend = bool.Parse(sHookWSASend);
                    Socket_Cache.HookWSASendTo = bool.Parse(sHookWSASendTo);
                    Socket_Cache.HookWSARecv = bool.Parse(sHookWSARecv);
                    Socket_Cache.HookWSARecvFrom = bool.Parse(sHookWSARecvFrom);

                    #endregion

                    #region//列表设置

                    XmlNode xnListConfig = xnConfig.SelectSingleNode("ListConfig");

                    string sSocketList_AutoRoll = xnListConfig.SelectSingleNode("SocketList_AutoRoll").InnerText;
                    string sSocketList_AutoClear = xnListConfig.SelectSingleNode("SocketList_AutoClear").InnerText;
                    string sSocketList_AutoClear_Value = xnListConfig.SelectSingleNode("SocketList_AutoClear_Value").InnerText;

                    Socket_Cache.SocketList.AutoRoll = bool.Parse(sSocketList_AutoRoll);
                    Socket_Cache.SocketList.AutoClear = bool.Parse(sSocketList_AutoClear);
                    Socket_Cache.SocketList.AutoClear_Value = decimal.Parse(sSocketList_AutoClear_Value);

                    string sFilterList_AutoRoll = xnListConfig.SelectSingleNode("FilterList_AutoRoll").InnerText;
                    string sFilterList_AutoClear = xnListConfig.SelectSingleNode("FilterList_AutoClear").InnerText;
                    string sFilterList_AutoClear_Value = xnListConfig.SelectSingleNode("FilterList_AutoClear_Value").InnerText;

                    Socket_Cache.FilterList.AutoRoll = bool.Parse(sFilterList_AutoRoll);
                    Socket_Cache.FilterList.AutoClear = bool.Parse(sFilterList_AutoClear);
                    Socket_Cache.FilterList.AutoClear_Value = decimal.Parse(sFilterList_AutoClear_Value);

                    #endregion

                    #region//系统设置

                    XmlNode xnSystemConfig = xnConfig.SelectSingleNode("SystemConfig");

                    string sSpeedMode = xnSystemConfig.SelectSingleNode("SpeedMode").InnerText;
                    string sFilterList_Execute = xnSystemConfig.SelectSingleNode("FilterList_Execute").InnerText;                  

                    Socket_Cache.SpeedMode = bool.Parse(sSpeedMode);
                    Socket_Cache.FilterList.FilterList_Execute = GetFilterListExecute_ByString(sFilterList_Execute);                 

                    #endregion
                }
                else
                {
                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
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
            if (bDoLog)
            {
                Task.Run(() =>
                {
                    Socket_Cache.LogQueue.LogToQueue(sFuncName, sLogContent);
                });
            }                     
        }

        #endregion

        #region//发送封包

        public static bool SendPacket(int iSocket, Socket_Cache.SocketPacket.PacketType stType, string sIPFrom, string sIPTo, byte[] bSendBuffer)
        {
            bool bReturn = false;

            try
            {
                if (iSocket > 0 && bSendBuffer.Length > 0)
                {
                    IntPtr ipSend = Marshal.AllocHGlobal(bSendBuffer.Length);
                    Marshal.Copy(bSendBuffer, 0, ipSend, bSendBuffer.Length);

                    int res = -1;
                    string sIPString = string.Empty;

                    if (stType == Socket_Cache.SocketPacket.PacketType.Send || stType == Socket_Cache.SocketPacket.PacketType.SendTo || stType == Socket_Cache.SocketPacket.PacketType.WSASend || stType == Socket_Cache.SocketPacket.PacketType.WSASendTo)
                    {
                        sIPString = sIPTo;
                    }
                    else if (stType == Socket_Cache.SocketPacket.PacketType.Recv || stType == Socket_Cache.SocketPacket.PacketType.RecvFrom || stType == Socket_Cache.SocketPacket.PacketType.WSARecv || stType == Socket_Cache.SocketPacket.PacketType.WSARecvFrom)
                    {
                        sIPString = sIPFrom;
                    }

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
