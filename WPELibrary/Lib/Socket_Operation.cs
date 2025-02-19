using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Drawing;
using WPELibrary.Lib.NativeMethods;
using EasyHook;
using Microsoft.Win32;

namespace WPELibrary.Lib
{   
    public static class Socket_Operation
    {
        public static Color col_Del = Color.Red;
        public static Color col_Add = Color.Green;
        public static bool bDoLog = true;
        public static bool bDoLog_HookTime = true;
        public static DataTable ProcessTable;
        public static DataTable dtSearchFrom = new DataTable();
        public static DataTable dtPacketFormat = new DataTable();

        #region//判断是否为64位的进程

        public static bool IsWin64Process(int ProcessID)
        {
            bool bReturn = false;

            try
            {
                Process pProcess = Process.GetProcessById(ProcessID);

                if (pProcess != null)
                {
                    if ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
                    {
                        bool retVal;
                        Kernel32.IsWow64Process(pProcess.Handle, out retVal);
                        bReturn = !retVal;
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

        #region//程序集特性访问器

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion

        #region//检测网站可访问性

        public static async Task<bool> CheckWebSite(string sURL)
        {
            bool bReturn = false;

            try
            {
                await Task.Run(() =>
                {
                    HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(sURL);

                    using (HttpWebResponse resp = (HttpWebResponse)hwr.GetResponse())
                    {
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            bReturn = true;
                        }
                    }
                });                
            }
            catch
            {
                bReturn = false;
            }

            return bReturn;
        }

        #endregion

        #region//获取所有进程

        public static async Task<DataTable> GetProcess()
        {
            DataTable dtProcessList = new DataTable();
            dtProcessList.Columns.Add("ICO", typeof(Image));
            dtProcessList.Columns.Add("PName", typeof(string));
            dtProcessList.Columns.Add("PID", typeof(int));
            dtProcessList.Columns.Add("PPath", typeof(string));

            try
            {
                await Task.Run(() =>
                {
                    Process[] procesArr = Process.GetProcesses();
                    int pCNT = procesArr.Length;

                    foreach (Process p in procesArr)
                    {
                        string sPName = p.ProcessName;
                        string sPPath = Socket_Operation.GetProcessPath(p);                        
                        int iPID = p.Id;
                        Image iICO = IconFromFile(p);

                        DataRow dr = dtProcessList.NewRow();
                        dr["ICO"] = iICO;
                        dr["PName"] = sPName;
                        dr["PID"] = iPID;
                        dr["PPath"] = sPPath;
                        dtProcessList.Rows.Add(dr);
                    }

                    DataView dv = dtProcessList.DefaultView;
                    dv.Sort = "PName";
                    dtProcessList = dv.ToTable();

                    ProcessTable = dv.ToTable();
                });                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return dtProcessList;
        }

        #endregion

        #region//获取进程的图标

        private static Image IconFromFile(Process p)
        {
            string filePath = "";
            Image image = null;

            try
            {
                filePath = p.MainModule.FileName.Replace(".ni.dll", ".dll");
            }
            catch
            {
                filePath = "";
            }

            try
            {
                var extractor = new IconExtractor.IconExtractor(filePath);
                var icon = extractor.GetIcon(0);

                Icon[] splitIcons = IconExtractor.IconUtil.Split(icon);

                Icon selectedIcon = null;

                foreach (var item in splitIcons)
                {
                    if (selectedIcon == null)
                    {
                        selectedIcon = item;
                    }
                    else
                    {
                        if (IconExtractor.IconUtil.GetBitCount(item) > IconExtractor.IconUtil.GetBitCount(selectedIcon))
                        {
                            selectedIcon = item;
                        }
                        else if (IconExtractor.IconUtil.GetBitCount(item) == IconExtractor.IconUtil.GetBitCount(selectedIcon) && item.Width > selectedIcon.Width)
                        {
                            selectedIcon = item;
                        }
                    }
                }

                return selectedIcon.ToBitmap();
            }
            catch
            {
                //
            }

            try
            {
                image = Icon.ExtractAssociatedIcon(filePath)?.ToBitmap();
            }
            catch
            {
                image = new Icon(SystemIcons.Application, 256, 256).ToBitmap();
            }

            return image;
        }

        #endregion

        #region//获取内存的数据

        public static byte[] GetBytesFromIntPtr(IntPtr ipBuffer, int Length)
        {
            byte[] bReturn = null;

            try
            {
                bReturn = new byte[Length];
                Marshal.Copy(ipBuffer, bReturn, 0, Length);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//设置系统代理

        public static bool StartSystemProxy()
        {
            bool bReturn = false;

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

                if (key != null)
                {
                    string sProxyServer = string.Format("socks5://127.0.0.1:{0}", Socket_Cache.SocketProxy.ProxyPort);

                    key.SetValue("ProxyEnable", 1, RegistryValueKind.DWord);
                    key.SetValue("ProxyServer", sProxyServer, RegistryValueKind.String);
                    key.SetValue("ProxyOverride", string.Empty, RegistryValueKind.String);
                    key.Close();

                    bReturn = true;

                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_148));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        public static bool StopSystemProxy()
        {
            bool bReturn = false;

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

                if (key != null)
                {
                    key.SetValue("ProxyEnable", 0, RegistryValueKind.DWord);
                    key.Close();

                    bReturn = true;

                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_149));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//数据格式转换

        #region//base64 编码，解码

        public static string Base64_Encoding(string sString)
        {
            string sReturn = string.Empty;

            try
            {
                byte[] bBuffer = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sString);
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
                        bReturn = Socket_Operation.Hex_To_Bytes(sString);
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

        private static byte[] Hex_To_Bytes(string hexString)
        {
            byte[] bReturn = null;

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

                bReturn = returnBytes;
            }
            catch
            {
                bReturn = new byte[0];
            }

            return bReturn;
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

        public static bool IsValidFilterString(string value)
        {
            bool bReturn = true;

            try
            {
                if (!String.IsNullOrEmpty(value))
                {
                    bReturn = Socket_Operation.IsHexString(value);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//获取中文字符串对应的bool类型

        public static bool GetBoolFromChineseString(string ChineseString)
        {
            bool bReturn = false;

            try
            {
                switch (ChineseString)
                {
                    case "真":
                        bReturn = true;
                        break;

                    case "假":
                        bReturn = false;
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

        #region//转换FILT过滤器的字符串

        public static string ConvertFILTString(string FiltString, bool bPosition)
        {
            string Return = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(FiltString) && FiltString.IndexOf("$") > 0)
                {
                    string[] slFiltString = FiltString.Split('$');

                    for (int i = 0; i < slFiltString.Length - 1; i += 3)
                    {
                        int iIndex = int.Parse(slFiltString[i]) - 1;
                        string sHex = slFiltString[i + 1];
                        int iHexCount = int.Parse(slFiltString[i + 2]);

                        for (int j = 0; j < iHexCount; j++)
                        {
                            int iFIndex = iIndex + j;

                            if (bPosition)
                            {
                                iFIndex += 250;
                            }

                            Return += iFIndex.ToString() + "|" + sHex.Substring(j * 2, 2) + ",";
                        }
                    }

                    Return = Return.TrimEnd(',');
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return Return;
        }

        #endregion

        #region//byte[]转Int16大端

        public static ushort ByteArrayToInt16BigEndian(byte[] bytes)
        {
            ushort uReturn = 0;

            try
            {
                if (bytes != null && bytes.Length == 2)
                {
                    uReturn = (ushort)((bytes[0] << 8) | bytes[1]);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return uReturn;
        }

        #endregion

        #endregion

        #region//判断地址的类型

        public static bool IsIPv4(string IPString)
        {
            return IPAddress.TryParse(IPString, out IPAddress ip);
        }

        public static bool IsIPv6(string IPString)
        {
            return IPAddress.TryParse(IPString, out IPAddress ip) && ip.AddressFamily == AddressFamily.InterNetworkV6;
        }

        public static bool IsDomain(string IPString)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(IPString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Socket_Cache.SocketProxy.AddressType GetAddressType_ByString(string IPString)
        {
            Socket_Cache.SocketProxy.AddressType atType = new Socket_Cache.SocketProxy.AddressType();

            try
            {
                if (IsIPv4(IPString))
                {
                    atType = Socket_Cache.SocketProxy.AddressType.IPV4;
                }
                else if (IsIPv6(IPString))
                {
                    atType = Socket_Cache.SocketProxy.AddressType.IPV6;
                }
                else if (IsDomain(IPString))
                {
                    atType = Socket_Cache.SocketProxy.AddressType.Domain;
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return atType;
        }

        #endregion

        #region//统计封包数量

        public static void CountSocketInfo(Socket_Cache.SocketPacket.PacketType ptPacketType, int iPacketLen)
        {
            try
            {
                if (iPacketLen > 0)
                {
                    Interlocked.Increment(ref Socket_Cache.SocketPacket.TotalPackets);

                    switch (ptPacketType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, iPacketLen);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, iPacketLen);
                            break;
                    }
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
                if (char.IsControl(keyChar) || char.IsDigit(keyChar) || keyChar.Equals(';') || keyChar.Equals('.') || keyChar.Equals('-'))
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

        #region//判断接收的数据是否匹配代理步骤

        public static bool CheckDataIsMatchProxyStep(byte[] bData, Socket_Cache.SocketProxy.ProxyStep proxyStep)
        {
            bool bReturn = false;

            try
            {
                byte VERSION = bData[0];

                switch (proxyStep)
                {
                    case Socket_Cache.SocketProxy.ProxyStep.Handshake:

                        if (VERSION == ((byte)Socket_Cache.SocketProxy.ProxyType.Socket5))
                        {
                            if (bData.Length > 2)
                            {
                                byte METHODS_COUNT = bData[1];

                                if (bData.Length >= METHODS_COUNT + 2)
                                {
                                    bReturn = true;
                                }
                            }
                        }

                        break;

                    case Socket_Cache.SocketProxy.ProxyStep.AuthUserName:

                        if (VERSION == 0x01)
                        {
                            if (bData.Length > 2)
                            {
                                byte USERNAME_LENGTH = bData[1];

                                if (bData.Length > USERNAME_LENGTH + 2)
                                {
                                    byte PASSWORD_LENGTH = bData[USERNAME_LENGTH + 2];

                                    if (bData.Length == USERNAME_LENGTH + PASSWORD_LENGTH + 3)
                                    {
                                        bReturn = true;
                                    }
                                }
                            }
                        }

                        break;

                    case Socket_Cache.SocketProxy.ProxyStep.Command:

                        if (VERSION == ((byte)Socket_Cache.SocketProxy.ProxyType.Socket5))
                        {
                            if (bData.Length > 4)
                            {
                                byte ADDRESS_TYPE = bData[3];
                                Socket_Cache.SocketProxy.AddressType AddressType = (Socket_Cache.SocketProxy.AddressType)ADDRESS_TYPE;

                                int DST_ADDR = 0;
                                switch (AddressType)
                                {
                                    case Socket_Cache.SocketProxy.AddressType.IPV4:
                                        DST_ADDR = 4;
                                        break;

                                    case Socket_Cache.SocketProxy.AddressType.IPV6:
                                        DST_ADDR = 16;
                                        break;

                                    case Socket_Cache.SocketProxy.AddressType.Domain:
                                        byte DST_LENGTH = bData[4];
                                        DST_ADDR = DST_LENGTH + 1;
                                        break;
                                }

                                if (bData.Length == DST_ADDR + 6)
                                {
                                    bReturn = true;
                                }
                            }
                        }

                        break;

                    case Socket_Cache.SocketProxy.ProxyStep.ForwardData:

                        bReturn = true;

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//判断 Http 外部代理连接状态

        public static bool CheckHttpProxyState(Socket_ProxyInfo spi, string targetAddress, ushort targetPort)
        {
            bool bReturn = false;

            try
            {
                byte[] bRequest = Socket_Operation.BuildHttpProxyRequest(targetAddress, targetPort);

                if (bRequest != null)
                {
                    Socket_Operation.SendTCPData(spi.TargetSocket, bRequest);

                    byte[] buffer = new byte[4096];
                    int bytesRead = spi.TargetSocket.Receive(buffer);
                    string response = Encoding.Default.GetString(buffer, 0, bytesRead);

                    if (response.StartsWith("HTTP/1.1 200"))
                    {
                        bReturn = true;
                    }
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        private static byte[] BuildHttpProxyRequest(string targetAddress, ushort targetPort)
        {
            byte[] bReturn = null;

            try
            {
                string sRequest = $"CONNECT {targetAddress}:{targetPort} HTTP/1.1\r\n";
                sRequest += $"Host: {targetAddress}:{targetPort}\r\n";
                sRequest += "Proxy-Connection: Keep-Alive\r\n\r\n";
                bReturn = Encoding.UTF8.GetBytes(sRequest);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//初始化滤镜的显示列

        public static DataGridViewTextBoxColumn InitDGVColumn(int ColIndex, Color cFore, Color cBack)
        {
            DataGridViewTextBoxColumn dtcReturn = new DataGridViewTextBoxColumn();

            try
            {
                dtcReturn.Name = "col" + ColIndex.ToString("000");
                dtcReturn.HeaderText = ColIndex.ToString("000");
                dtcReturn.MaxInputLength = 2;
                dtcReturn.DefaultCellStyle.ForeColor = cFore;
                dtcReturn.DefaultCellStyle.BackColor = cBack;
                dtcReturn.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return dtcReturn;
        }

        #endregion

        #region//初始化发送列表的选择框

        public static void InitSendListComboBox(ComboBox cbb)
        {
            try
            {
                cbb.Items.Clear();

                foreach (Socket_SendInfo ssi in Socket_Cache.SendList.lstSend)
                {
                    cbb.Items.Add(new Socket_Cache.SendList.SendListItem { SName = ssi.SName, SID = ssi.SID });
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static void InitSendListComboBox(ToolStripComboBox tscb)
        {
            try
            {
                tscb.Items.Clear();

                foreach (Socket_SendInfo ssi in Socket_Cache.SendList.lstSend)
                {
                    tscb.Items.Add(new Socket_Cache.SendList.SendListItem { SName = ssi.SName, SID = ssi.SID });
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//获取当前进程的格式化名称

        public static string GetProcessName()
        {
            string sReturn = string.Empty;

            try
            {
                Process pProcess = Process.GetCurrentProcess();
                sReturn = string.Format("{0}{1} [{2}]", MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_20), pProcess.ProcessName, RemoteHooking.GetCurrentProcessId());
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取进程的信息

        public static string GetProcessPath(Process process)
        {
            string sReturn = string.Empty;

            try
            {
                sReturn = process.MainModule.FileName;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        public static string GetProcessInfo()
        {
            string sReturn = string.Empty;

            try
            {
                Process pProcess = Process.GetCurrentProcess();

                if (pProcess.MainWindowHandle != IntPtr.Zero)
                {
                    if (string.IsNullOrEmpty(pProcess.MainWindowTitle))
                    {
                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_144), pProcess.MainModule.ModuleName, pProcess.MainWindowHandle.ToString());
                    }
                    else
                    {
                        sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_144), pProcess.MainWindowTitle, pProcess.MainWindowHandle.ToString());
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(pProcess.MainWindowTitle))
                    {
                        sReturn = pProcess.MainModule.ModuleName;
                    }
                    else
                    {
                        sReturn = pProcess.MainWindowTitle;
                    }                    
                }               
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取当前进程支持的Winsock版本信息

        public static string GetWinSockSupportInfo()
        {
            string sReturn = "WinSock";

            try
            {
                Socket_Cache.SocketPacket.Support_WS1 = false;
                Socket_Cache.SocketPacket.Support_WS2 = false;

                foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                {
                    string sModuleName = module.ModuleName;

                    if (sModuleName.Equals(WSock32.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Socket_Cache.SocketPacket.Support_WS1 = true;
                    }

                    if (sModuleName.Equals(WS2_32.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Socket_Cache.SocketPacket.Support_WS2 = true;
                    }

                    if (sModuleName.Equals(Mswsock.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Socket_Cache.SocketPacket.Support_MsWS = true;
                    }
                }

                if (Socket_Cache.SocketPacket.Support_WS1)
                {
                    sReturn += " 1.1";
                }

                if (Socket_Cache.SocketPacket.Support_WS2)
                {
                    sReturn += " 2.0";
                }

                if (Socket_Cache.SocketPacket.Support_MsWS)
                {
                    sReturn += " Microsoft";
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion                

        #region//处理 Hook 结果（异步）

        public static void ProcessingHookResult(
            Int32 socket,
            byte[] bRawBuffer,
            byte[] bBuffer, 
            Int32 res, 
            Socket_Cache.SocketPacket.PacketType ptType, 
            Socket_Cache.Filter.FilterAction FilterAction, 
            Socket_Cache.SocketPacket.SockAddr sockaddr)
        {
            try
            {
                Socket_Operation.CountSocketInfo(ptType, res);

                if (!Socket_Cache.SocketPacket.SpeedMode)
                {
                    if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                    {
                        if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                        {
                            Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bRawBuffer, bBuffer, ptType, sockaddr, FilterAction);
                        }
                        else
                        {
                            if (res > 0)
                            {
                                Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bRawBuffer, bBuffer, ptType, sockaddr, FilterAction);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//获取 SockAddr 对应的 IP 地址和端口

        public static string GetIPString_BySocketAddr(int pSocket, Socket_Cache.SocketPacket.SockAddr pAddr, Socket_Cache.SocketPacket.PacketType pType)
        {
            string sReturn = string.Empty;

            try
            {  
                string sIP_From = string.Empty;
                string sIP_To = string.Empty;

                sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);                

                if (pType == Socket_Cache.SocketPacket.PacketType.WS1_Send ||
                    pType == Socket_Cache.SocketPacket.PacketType.WS2_Send ||
                    pType == Socket_Cache.SocketPacket.PacketType.WS1_Recv ||
                    pType == Socket_Cache.SocketPacket.PacketType.WS2_Recv ||
                    pType == Socket_Cache.SocketPacket.PacketType.WSASend || 
                    pType == Socket_Cache.SocketPacket.PacketType.WSARecv ||
                    pType == Socket_Cache.SocketPacket.PacketType.WSARecvEx)
                {
                    sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.To);
                }
                else if (pType == Socket_Cache.SocketPacket.PacketType.WS1_SendTo ||
                    pType == Socket_Cache.SocketPacket.PacketType.WS2_SendTo ||
                    pType == Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom ||
                    pType == Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom ||
                    pType == Socket_Cache.SocketPacket.PacketType.WSASendTo || 
                    pType == Socket_Cache.SocketPacket.PacketType.WSARecvFrom)
                {
                    sIP_To = Socket_Operation.GetIP_BySockAddr(pAddr);
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

        public static string GetIP_BySockAddr(Socket_Cache.SocketPacket.SockAddr saAddr)
        {
            string sReturn = string.Empty;

            try
            {
                if (saAddr.sin_family == (short)AddressFamily.InterNetwork)
                {
                    string sIP = Marshal.PtrToStringAnsi(WS2_32.inet_ntoa(saAddr.sin_addr));
                    string sPort = WS2_32.ntohs(saAddr.sin_port).ToString();

                    sReturn = sIP + ":" + sPort;
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        public static string GetIP_BySocket(int Socket, Socket_Cache.SocketPacket.IPType IPType)
        {
            string sReturn = "";

            try
            {
                Socket_Cache.SocketPacket.SockAddr saAddr = new Socket_Cache.SocketPacket.SockAddr();
                saAddr.sin_family = (short)AddressFamily.InterNetwork;
                int iAddrLen = Marshal.SizeOf(saAddr);

                switch (IPType)
                {
                    case Socket_Cache.SocketPacket.IPType.From:

                        WS2_32.getsockname(Socket, ref saAddr, ref iAddrLen);

                        break;

                    case Socket_Cache.SocketPacket.IPType.To:

                        WS2_32.getpeername(Socket, ref saAddr, ref iAddrLen);

                        break;                    
                }

                sReturn = GetIP_BySockAddr(saAddr);                
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取 IP 地址和端口对应的 SockAddr

        public static Socket_Cache.SocketPacket.SockAddr GetSocketAddr_ByIPString(string IPString)
        {
            Socket_Cache.SocketPacket.SockAddr saReturn = new Socket_Cache.SocketPacket.SockAddr();

            try
            {
                if (!string.IsNullOrEmpty(IPString) && IPString.IndexOf(":") > 0)
                {
                    string sIP = IPString.Split(':')[0];
                    int iPort = int.Parse(IPString.Split(':')[1]);

                    IPAddress ipAddress = IPAddress.Parse(sIP);

                    saReturn.sin_family = ((short)AddressFamily.InterNetwork);
                    saReturn.sin_port = (ushort)IPAddress.HostToNetworkOrder((short)iPort);
                    saReturn.sin_addr = (uint)ipAddress.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return saReturn;
        }

        #endregion        

        #region//获取IP地址信息

        public static async Task<IPAddress[]> GetLocalIPAddress()
        {
            IPAddress[] ipAddres = null;

            try
            {
                await Task.Run(() =>
                {
                    ipAddres = Dns.GetHostAddresses(Dns.GetHostName()).Where(address => address.AddressFamily == AddressFamily.InterNetwork).ToArray();
                });                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            

            return ipAddres;
        }

        public static string GetIP_ByAddressType(Socket_Cache.SocketProxy.AddressType addressType, byte[] bData)
        { 
            string sReturn = string.Empty;

            try
            {
                byte[] bIP = null;
                IPAddress IP = IPAddress.Any;

                switch (addressType)
                {
                    case Socket_Cache.SocketProxy.AddressType.IPV4:

                        bIP = new byte[4];
                        Buffer.BlockCopy(bData, 0, bIP, 0, bIP.Length);
                        IP = new IPAddress(bIP);
                        sReturn = IP.ToString();

                        break;

                    case Socket_Cache.SocketProxy.AddressType.Domain:

                        byte Length = bData[0];
                        bIP = new byte[Length];
                        Buffer.BlockCopy(bData, 1, bIP, 0, bIP.Length);
                        sReturn = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, bIP);

                        break;

                    case Socket_Cache.SocketProxy.AddressType.IPV6:

                        bIP = new byte[16];
                        Buffer.BlockCopy(bData, 0, bIP, 0, bIP.Length);
                        IP = new IPAddress(bIP);
                        sReturn = IP.ToString();

                        break;
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        public static IPEndPoint GetIPEndPoint_ByAddressType(Socket_Cache.SocketProxy.AddressType addressType, byte[] bData)
        {
            IPEndPoint epReturn = null;

            try
            {
                byte[] bIP = null;
                byte[] bPort = null;
                ushort port = 0;
                string sIPString = string.Empty;
                IPAddress ip = IPAddress.Any;

                switch (addressType)
                {
                    case Socket_Cache.SocketProxy.AddressType.IPV4:

                        bIP = new byte[4];
                        Buffer.BlockCopy(bData, 0, bIP, 0, bIP.Length);
                        ip = new IPAddress(bIP);

                        bPort = new byte[2];
                        Buffer.BlockCopy(bData, 4, bPort, 0, bPort.Length);
                        port = Socket_Operation.ByteArrayToInt16BigEndian(bPort);

                        sIPString = ip.ToString();

                        break;

                    case Socket_Cache.SocketProxy.AddressType.Domain:

                        byte Length = bData[0];
                        bIP = new byte[Length];
                        Buffer.BlockCopy(bData, 1, bIP, 0, bIP.Length);
                        sIPString = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, bIP);

                        Socket_Cache.SocketProxy.AddressType atType = Socket_Operation.GetAddressType_ByString(sIPString);

                        switch (atType)
                        {
                            case Socket_Cache.SocketProxy.AddressType.IPV4:
                                ip = IPAddress.Parse(sIPString);
                                break;

                            case Socket_Cache.SocketProxy.AddressType.IPV6:
                                ip = IPAddress.Parse(sIPString);
                                break;

                            case Socket_Cache.SocketProxy.AddressType.Domain:
                                ip = Dns.GetHostEntry(sIPString).AddressList[0];
                                break;
                        }

                        bPort = new byte[2];
                        Buffer.BlockCopy(bData, 1 + Length, bPort, 0, bPort.Length);
                        port = Socket_Operation.ByteArrayToInt16BigEndian(bPort);

                        break;

                    case Socket_Cache.SocketProxy.AddressType.IPV6:

                        bIP = new byte[16];
                        Buffer.BlockCopy(bData, 0, bIP, 0, bIP.Length);
                        ip = new IPAddress(bIP);

                        bPort = new byte[2];
                        Buffer.BlockCopy(bData, 16, bPort, 0, bPort.Length);
                        port = Socket_Operation.ByteArrayToInt16BigEndian(bPort);

                        sIPString = ip.ToString();

                        break;
                }

                epReturn = new IPEndPoint(ip, port);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return epReturn;
        }

        #endregion

        #region//获取UDP数据包

        public static byte[] GetUDPData_ByAddressType(Socket_Cache.SocketProxy.AddressType addressType, byte[] bData)
        {
            byte[] bReturn = null;

            try
            {
                switch (addressType)
                {
                    case Socket_Cache.SocketProxy.AddressType.IPV4:

                        bReturn = new byte[bData.Length - 10];
                        Buffer.BlockCopy(bData, 10, bReturn, 0, bReturn.Length);

                        break;

                    case Socket_Cache.SocketProxy.AddressType.Domain:

                        byte LENGTH = bData[4];
                        bReturn = new byte[bData.Length - (LENGTH + 7)];
                        Buffer.BlockCopy(bData, LENGTH + 7, bReturn, 0, bReturn.Length);

                        break;

                    case Socket_Cache.SocketProxy.AddressType.IPV6:

                        bReturn = new byte[bData.Length - 22];
                        Buffer.BlockCopy(bData, 22, bReturn, 0, bReturn.Length);

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//获取返回给客户端的数据（SOCKS5，IPV4）

        public static byte[] GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse CommandResponse, byte[] bServerIP, byte[] bServerPort)
        {
            byte[] bReturn = null;

            try
            {
                bReturn = new byte[] { (byte)Socket_Cache.SocketProxy.ProxyType.Socket5, (byte)CommandResponse, 0x00, (byte)Socket_Cache.SocketProxy.AddressType.IPV4, bServerIP[0], bServerIP[1], bServerIP[2], bServerIP[3], bServerPort[1], bServerPort[0] };
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
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

        public static byte[] GetByteFromWSABUF(Socket_Cache.SocketPacket.WSABUF lpBuffers, Int32 dwBufferCount, int BytesCNT)
        {
            byte[] bReturn = new byte[0];

            try
            {
                int BytesLeft = BytesCNT;

                for (int i = 0; i < dwBufferCount; i++)
                {
                    if (BytesLeft > 0)
                    {
                        int iBuffLen = 0;

                        if (lpBuffers.len >= BytesLeft)
                        {
                            iBuffLen = BytesLeft;
                        }
                        else
                        {
                            iBuffLen = lpBuffers.len;
                        }

                        BytesLeft -= iBuffLen;

                        byte[] bBuff = new byte[iBuffLen];
                        Marshal.Copy(lpBuffers.buf, bBuff, 0, iBuffLen);

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

                while (iStepValue > 255)
                {
                    iStepValue -= 256;
                }

                while (iStepValue < 0)
                {
                    iStepValue += 256;
                }

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

                bStepBuffer[iStepPosition] = Socket_Operation.GetStepByte(bStepByte, iStepLen);

                bReturn = bStepBuffer;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
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

        #region//获取域名类型

        public static Socket_Cache.SocketProxy.DomainType GetDomainType_ByPort(ushort Port)
        {
            Socket_Cache.SocketProxy.DomainType dtReturn = new Socket_Cache.SocketProxy.DomainType();            

            try
            {
                if (Socket_Cache.SocketProxy.Enable_EXTHttp)
                {
                    if (!string.IsNullOrEmpty(Socket_Cache.SocketProxy.AppointHttpPort))
                    {
                        string[] slHttpPort = Socket_Cache.SocketProxy.AppointHttpPort.Split(',');

                        foreach (string s in slHttpPort)
                        {
                            if (s.Equals(Port.ToString()))
                            {
                                return Socket_Cache.SocketProxy.DomainType.Http;
                            }
                        }
                    }                    
                }                

                if (Socket_Cache.SocketProxy.Enable_EXTHttps)
                {
                    if (!string.IsNullOrEmpty(Socket_Cache.SocketProxy.AppointHttpsPort))
                    {
                        string[] slHttpsPort = Socket_Cache.SocketProxy.AppointHttpsPort.Split(',');

                        foreach (string s in slHttpsPort)
                        {
                            if (s.Equals(Port.ToString()))
                            {
                                return Socket_Cache.SocketProxy.DomainType.Https;
                            }
                        }
                    }
                }

                if (Port == 80 || Port == 8080)
                {
                    return Socket_Cache.SocketProxy.DomainType.Http;
                }
                else if (Port == 443 || Port == 8443)
                {
                    return Socket_Cache.SocketProxy.DomainType.Https;
                }
                else
                {
                    return Socket_Cache.SocketProxy.DomainType.Socket;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return dtReturn;
        }

        #endregion

        #region//获取对应名称的树节点

        public static async Task<TreeNode> FindNodeAsync(TreeView treeView, string nodeName)
        {
            return await Socket_Operation.FindNode(treeView.Nodes, nodeName);
        }

        private static async Task<TreeNode> FindNode(TreeNodeCollection nodes, string nodeName)
        {
            TreeNode tnReturn = null;

            try
            {
                foreach (TreeNode node in nodes)
                {
                    if (node.Text == nodeName)
                    {
                        tnReturn = node;
                        break;
                    }

                    TreeNode foundNode = await FindNode(node.Nodes, nodeName);

                    if (foundNode != null)
                    {
                        tnReturn = foundNode;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            
            return tnReturn;
        }

        #endregion

        #region//添加树节点

        public static async Task<TreeNode> AddTreeNode(TreeView treeView, TreeNodeCollection Nodes, string NodeName, int ImgIndex, byte[] bData)
        {
            TreeNode tnReturn = null;

            try
            {
                await Task.Run(() =>
                {
                    if (!treeView.IsDisposed)
                    {
                        treeView.Invoke(new MethodInvoker(delegate
                        {
                            tnReturn = Nodes.Add(NodeName);

                            if (ImgIndex > -1)
                            {
                                tnReturn.ImageIndex = ImgIndex;
                                tnReturn.SelectedImageIndex = ImgIndex;
                            }

                            if (bData != null && bData.Length > 0)
                            {
                                tnReturn.Tag = bData;
                            }
                        }));
                    }
                });                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return tnReturn;
        }

        #endregion

        #region//获取远端地址

        public static string GetTargetAddress(string IP, ushort Port, Socket_ProxyInfo spi)
        {
            string sReturn = string.Empty;

            try
            {
                switch (spi.DomainType)
                {
                    case Socket_Cache.SocketProxy.DomainType.Socket:
                        sReturn = "socket://" + IP + ": " + Port;
                        break;

                    case Socket_Cache.SocketProxy.DomainType.Http:
                        sReturn = "http://" + IP;
                        break;

                    case Socket_Cache.SocketProxy.DomainType.Https:
                        sReturn = "https://" + IP;
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

        #region//获取客户端地址

        public static string GetClientAddress(string IP, ushort Port, Socket_ProxyInfo spi)
        {
            string sReturn = string.Empty;

            try
            {
                int ClientPort = ((IPEndPoint)spi.ClientSocket.RemoteEndPoint).Port;
                sReturn = IP + ": " + Port + " [" + ClientPort + "]";
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion        

        #region//数据对比

        public static async Task<string> CompareData(Font font, string sText_A, string sText_B)
        {
            string sReturn = string.Empty;

            try
            {
                await Task.Run(() =>
                {
                    RichTextBox rtbCompare = new RichTextBox();
                    rtbCompare.Font = font;

                    if (sText_A == sText_B)
                    {
                        Socket_Operation.AppendColoredText(rtbCompare, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_29), Color.Blue);
                    }
                    else
                    {
                        string[] linesA = sText_A.Split('\n').Select(s => s.Trim()).ToArray();
                        string[] linesB = sText_B.Split('\n').Select(s => s.Trim()).ToArray();

                        int la = 0;
                        int lb = 0;

                        while (la < linesA.Length)
                        {
                            if (lb >= linesB.Length)
                            {
                                Socket_Operation.AppendColoredText(rtbCompare, linesA[la], Socket_Operation.col_Del);
                            }
                            else if (linesA[la] == linesB[lb])
                            {
                                Socket_Operation.AppendColoredText(rtbCompare, linesA[la], rtbCompare.ForeColor);
                            }
                            else
                            {
                                if ((lb + 1 < linesB.Length) && (linesA[la] == linesB[lb + 1]))
                                {
                                    Socket_Operation.AppendColoredText(rtbCompare, linesB[lb], Socket_Operation.col_Add);
                                    Socket_Operation.AppendColoredText(rtbCompare, "\n" + linesA[la], rtbCompare.ForeColor);

                                    lb++;
                                }
                                else if ((la + 1 < linesA.Length) && (linesA[la + 1] == linesB[lb]))
                                {
                                    Socket_Operation.AppendColoredText(rtbCompare, linesA[la], Socket_Operation.col_Del);
                                    Socket_Operation.AppendColoredText(rtbCompare, "\n" + linesB[lb], rtbCompare.ForeColor);

                                    la++;
                                }
                                else
                                {
                                    string[] wordsA = linesA[la].Split(' ').Select(s => s.Trim()).ToArray();
                                    string[] wordsB = linesB[lb].Split(' ').Select(s => s.Trim()).ToArray();

                                    int wa = 0;
                                    int wb = 0;
                                    while (wa < wordsA.Length)
                                    {
                                        if (wb >= wordsB.Length)
                                        {
                                            Socket_Operation.AppendColoredText(rtbCompare, wordsA[wa], Socket_Operation.col_Del);
                                        }
                                        else if (wordsA[wa] == wordsB[wb])
                                        {
                                            Socket_Operation.AppendColoredText(rtbCompare, wordsA[wa], rtbCompare.ForeColor);
                                        }
                                        else
                                        {
                                            if ((wb + 1 < wordsB.Length) && (wordsA[wa] == wordsB[wb + 1]))
                                            {
                                                Socket_Operation.AppendColoredText(rtbCompare, wordsB[wb], Socket_Operation.col_Add);
                                                Socket_Operation.AppendColoredText(rtbCompare, " " + wordsA[wa], rtbCompare.ForeColor);

                                                wb++;
                                            }
                                            else if ((wa + 1 < wordsA.Length) && (wordsA[wa + 1] == wordsB[wb]))
                                            {
                                                Socket_Operation.AppendColoredText(rtbCompare, wordsA[wa], Socket_Operation.col_Del);
                                                Socket_Operation.AppendColoredText(rtbCompare, " " + wordsB[wb], rtbCompare.ForeColor);

                                                wa++;
                                            }
                                            else
                                            {
                                                Socket_Operation.AppendColoredText(rtbCompare, wordsA[wa], Socket_Operation.col_Del);
                                                Socket_Operation.AppendColoredText(rtbCompare, wordsB[wb], Socket_Operation.col_Add);
                                            }
                                        }
                                        if (wa + 1 < wordsA.Length) Socket_Operation.AppendColoredText(rtbCompare, " ", rtbCompare.ForeColor);

                                        if ((wordsB.Length >= wordsA.Length) && (wa + 1 == wordsA.Length))
                                        {
                                            while (wb + 1 < wordsB.Length)
                                            {
                                                wb++;

                                                Socket_Operation.AppendColoredText(rtbCompare, " ", rtbCompare.ForeColor);
                                                Socket_Operation.AppendColoredText(rtbCompare, wordsB[wb], Socket_Operation.col_Add);
                                            }
                                        }

                                        wa++;
                                        wb++;
                                    }
                                }
                            }

                            if (la + 1 < linesA.Length)
                            {
                                Socket_Operation.AppendColoredText(rtbCompare, "\n", rtbCompare.ForeColor);
                            }

                            if ((linesB.Length >= linesA.Length) && (la + 1 == linesA.Length))
                            {
                                while (lb + 1 < linesB.Length)
                                {
                                    lb++;

                                    Socket_Operation.AppendColoredText(rtbCompare, "\n", rtbCompare.ForeColor);
                                    Socket_Operation.AppendColoredText(rtbCompare, linesB[lb], Socket_Operation.col_Add);
                                }
                            }

                            la++;
                            lb++;
                        }
                    }

                    sReturn = rtbCompare.Rtf;
                });                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        private static void AppendColoredText(RichTextBox box, string text, Color color)
        {
            try
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = text.Length;

                if (color == col_Add)
                {
                    box.SelectionFont = new Font(box.SelectionFont, FontStyle.Underline);
                }

                if (color == col_Del)
                {
                    box.SelectionFont = new Font(box.SelectionFont, FontStyle.Strikeout);
                }

                box.SelectionColor = color;
                box.AppendText(text);

                box.SelectionFont = box.Font;
                box.SelectionColor = box.ForeColor;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//支持取消的等待（异步）

        public static async Task DoSleepAsync(int MilliSecond, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(MilliSecond, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                //
            }
        }        

        #endregion

        #region//发送 TCP 代理数据

        public static int SendTCPData(Socket socket, byte[] bData)
        {
            int iReturn = 0;

            try
            {
                if (socket != null)
                {
                    iReturn = socket.Send(bData, SocketFlags.None);
                }
            }
            catch
            {
                //
            }

            return iReturn;
        }

        #endregion

        #region//接收 TCP 代理数据

        public static int ReceiveTCPData(Socket socket, IAsyncResult ar)
        {
            int iReturn = 0;

            try
            {
                if (socket != null)
                {
                    iReturn = socket.EndReceive(ar);
                }
            }
            catch
            {
                //
            }

            return iReturn;
        }

        #endregion

        #region//发送 UDP 中继数据

        public static int SendUDPData(UdpClient ClientUDP, byte[] bData, IPEndPoint ep)
        {
            int iReturn = 0;

            try
            {
                if (ClientUDP != null)
                {
                    iReturn = ClientUDP.Send(bData, bData.Length, ep);
                }
            }
            catch
            {
                //
            }

            return iReturn;
        }

        #endregion

        #region//接收 UDP 中继数据

        public static byte[] ReceiveUDPData(UdpClient ClientUDP, IAsyncResult ar, ref IPEndPoint ep)
        {
            byte[] bReturn = null;

            try
            {
                if (ClientUDP != null)
                {
                    bReturn = ClientUDP.EndReceive(ar, ref ep);                    
                }
            }
            catch
            {
                //
            }

            return bReturn;
        }

        #endregion

        #region//是否显示封包（过滤条件）        

        public static bool IsShowSocketPacket_ByFilter(Socket_PacketInfo spi)
        {  
            try
            {
                //套接字
                if (Socket_Cache.SocketPacket.CheckSocket)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_BySocket(spi.PacketSocket);

                    if (Socket_Cache.SocketPacket.CheckNotShow)
                    {
                        if (bIsFilter)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!bIsFilter)
                        {
                            return false;
                        }
                    }
                }

                //IP地址
                if (Socket_Cache.SocketPacket.CheckIP)
                {
                    bool bIsFilter_From = Socket_Operation.IsFilter_ByIP(spi.PacketFrom);
                    bool bIsFilter_To = Socket_Operation.IsFilter_ByIP(spi.PacketTo);

                    if (Socket_Cache.SocketPacket.CheckNotShow)
                    {
                        if (bIsFilter_From || bIsFilter_To)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!(bIsFilter_From || bIsFilter_To))
                        {
                            return false;
                        }
                    }               
                }

                //端口号
                if (Socket_Cache.SocketPacket.CheckPort)
                {
                    bool bIsFilter_From = Socket_Operation.IsFilter_ByPort(spi.PacketFrom);
                    bool bIsFilter_To = Socket_Operation.IsFilter_ByPort(spi.PacketTo);

                    if (Socket_Cache.SocketPacket.CheckNotShow)
                    {
                        if (bIsFilter_From || bIsFilter_To)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!(bIsFilter_From || bIsFilter_To))
                        {
                            return false;
                        }
                    }
                }

                //指定包头
                if (Socket_Cache.SocketPacket.CheckHead)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_ByHead(spi.PacketBuffer);

                    if (Socket_Cache.SocketPacket.CheckNotShow)
                    {
                        if (bIsFilter)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!bIsFilter)
                        {
                            return false;
                        }
                    }
                }

                //封包内容
                if (Socket_Cache.SocketPacket.CheckData)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_ByPacket(spi.PacketBuffer);

                    if (Socket_Cache.SocketPacket.CheckNotShow)
                    {
                        if (bIsFilter)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!bIsFilter)
                        {
                            return false;
                        }
                    }
                }

                //封包大小
                if (Socket_Cache.SocketPacket.CheckSize)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_BySize(spi.PacketLen);

                    if (Socket_Cache.SocketPacket.CheckNotShow)
                    {
                        if (bIsFilter)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!bIsFilter)
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

        private static bool IsFilter_BySocket(int iPacketSocket)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckSocket_Value))
                {
                    string[] sSocketArr = Socket_Cache.SocketPacket.CheckSocket_Value.Split(';');

                    foreach (string sSocket in sSocketArr)
                    {
                        if (!string.IsNullOrEmpty(sSocket))
                        {
                            if (int.TryParse(sSocket, out int iCheckSocket))
                            {
                                if (iPacketSocket == iCheckSocket)
                                {
                                    bReturn = true;
                                    break;
                                }
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

        #region//检测IP地址

        private static bool IsFilter_ByIP(string sPacketIP)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(sPacketIP))
                {
                    string sIP = sPacketIP.Split(':')[0];
                    string sPort = sPacketIP.Split(':')[1];

                    if (!string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckIP_Value))
                    {
                        string[] sIPArr = Socket_Cache.SocketPacket.CheckIP_Value.Split(';');

                        foreach (string sCheckIP in sIPArr)
                        {
                            if (!string.IsNullOrEmpty(sCheckIP))
                            {
                                if (sIP.Equals(sCheckIP))
                                {
                                    bReturn = true;
                                    break;
                                }
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

        private static bool IsFilter_ByPort(string sPacketPort)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(sPacketPort))
                {
                    string sIP = sPacketPort.Split(':')[0];
                    string sPort = sPacketPort.Split(':')[1];

                    if (!string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckPort_Value))
                    {
                        string[] sPortArr = Socket_Cache.SocketPacket.CheckPort_Value.Split(';');

                        foreach (string sCheckPort in sPortArr)
                        {
                            if (!string.IsNullOrEmpty(sCheckPort))
                            {
                                if (sPort.Equals(sCheckPort))
                                {
                                    bReturn = true;
                                    break;
                                }
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

        private static bool IsFilter_ByHead(byte[] bBuffer)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckHead_Value))
                {
                    string sPacket = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);
                    sPacket = sPacket.Replace(" ", "");

                    string[] sHeadArr = Socket_Cache.SocketPacket.CheckHead_Value.Replace(" ", "").Split(';');

                    foreach (string sCheckHead in sHeadArr)
                    {
                        if (!string.IsNullOrEmpty(sCheckHead))
                        {
                            if (sPacket.IndexOf(sCheckHead) == 0)
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

        #region//检测封包内容

        private static bool IsFilter_ByPacket(byte[] bBuffer)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckData_Value))
                {
                    string sPacket = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);
                    sPacket = sPacket.Replace(" ", "");

                    string[] sPacketArr = Socket_Cache.SocketPacket.CheckData_Value.Replace(" ", "").Split(';');

                    foreach (string sCheckPacket in sPacketArr)
                    {
                        if (!string.IsNullOrEmpty(sCheckPacket))
                        {
                            if (sPacket.IndexOf(sCheckPacket) >= 0)
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

        #region//检测封包大小

        private static bool IsFilter_BySize(int PacketLength)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckLength_Value))
                {
                    string[] sLengthArr = Socket_Cache.SocketPacket.CheckLength_Value.Split(';');

                    foreach (string sLength in sLengthArr)
                    {
                        if (!string.IsNullOrEmpty(sLength))
                        {
                            if (sLength.IndexOf("-") > 0)
                            {
                                if (int.TryParse(sLength.Split('-')[0], out int iFrom) && int.TryParse(sLength.Split('-')[1], out int iTo))
                                {
                                    if (PacketLength >= iFrom && PacketLength <= iTo)
                                    {
                                        bReturn = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (int.TryParse(sLength, out int iLength))
                                {
                                    if (PacketLength == iLength)
                                    {
                                        bReturn = true;
                                        break;
                                    }
                                }
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

        #region//显示数据对比窗体

        public static void ShowSocketCompareForm(int SelectIndex)
        {
            try
            {
                if (SelectIndex > -1)
                {
                    Socket_CompareForm compareForm = new Socket_CompareForm(SelectIndex);
                    compareForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//显示查找窗体

        public static void ShowFindForm()
        {
            try
            {
                Socket_FindForm sffFindForm = new Socket_FindForm();
                sffFindForm.ShowDialog();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示滤镜窗体（对话框）

        public static void ShowFilterForm_Dialog(int FIndex)
        {
            try
            {
                if (Socket_Cache.FilterList.lstFilter.Count > 0)
                {
                    if (FIndex > -1)
                    {  
                        Socket_FilterForm fFilterForm = new Socket_FilterForm(FIndex);
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

        #region//显示发送列表窗体（对话框）

        public static void ShowSendListForm_Dialog(int SIndex)
        {
            try
            {
                if (Socket_Cache.SendList.lstSend.Count > 0)
                {
                    Socket_SendListForm fSendListForm = new Socket_SendListForm(SIndex);
                    fSendListForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示机器人窗体（对话框）

        public static void ShowRobotForm_Dialog(int RIndex)
        {
            try
            {
                if (Socket_Cache.RobotList.lstRobot.Count > 0)
                {
                    if (RIndex > -1)
                    {
                        Socket_RobotForm fRobotForm = new Socket_RobotForm(RIndex);
                        fRobotForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//注册操作系统快捷键       

        public static void RegisterHotKey()
        {
            try
            {
                if (Socket_Cache.MainHandle != IntPtr.Zero)
                {
                    int KeyControl = (int)User32.KeyModifiers.MOD_CONTROL;
                    int KeyAlt = (int)User32.KeyModifiers.MOD_ALT;
                    int iKeyCode = (int)Keys.F1;

                    for (int i = 9001; i < 9013; i++)
                    {
                        bool bOK = User32.RegisterHotKey(Socket_Cache.MainHandle, i, KeyControl | KeyAlt, iKeyCode);

                        if (!bOK)
                        {
                            Keys HotKey = (Keys)iKeyCode;
                            string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), HotKey.ToString());
                            Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                        }

                        iKeyCode++;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static void UnregisterHotKey()
        {
            try
            {
                if (Socket_Cache.MainHandle != IntPtr.Zero)
                {
                    for (int i = 9001; i < 9013; i++)
                    {
                        User32.UnregisterHotKey(Socket_Cache.MainHandle, i);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存系统列表数据

        public static void SaveSystemList()
        {
            try
            {
                Socket_Cache.FilterList.SaveFilterList(Socket_Cache.FilterList.FilePath, -1, false);
                Socket_Cache.RobotList.SaveRobotList(Socket_Cache.RobotList.FilePath, -1, false);
                Socket_Cache.SendList.SaveSendList(Socket_Cache.SendList.FilePath, -1, false);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//加载系统列表数据

        public static void LoadSystemList()
        {
            Task.Run(() =>
            {
                try
                {
                    Socket_Cache.SendList.LoadSendList(Socket_Cache.SendList.FilePath, false);
                    Socket_Cache.FilterList.LoadFilterList(Socket_Cache.FilterList.FilePath, false);
                    Socket_Cache.RobotList.LoadRobotList(Socket_Cache.RobotList.FilePath, false);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            });            
        }

        #endregion

        #region//保存系统设置

        public static void SaveConfigs_SocketProxy()
        {
            try
            {
                Properties.Settings.Default.ProxyConfig_ProxyIP_Auto = Socket_Cache.SocketProxy.ProxyIP_Auto;                
                Properties.Settings.Default.ProxyConfig_EnableSOCKS5 = Socket_Cache.SocketProxy.Enable_SOCKS5;
                Properties.Settings.Default.ProxyConfig_ProxyPort = Socket_Cache.SocketProxy.ProxyPort;
                Properties.Settings.Default.ProxyConfig_EnableAuth = Socket_Cache.SocketProxy.Enable_Auth;
                Properties.Settings.Default.ProxyConfig_Auth_UserName = Socket_Cache.SocketProxy.Auth_UserName;
                Properties.Settings.Default.ProxyConfig_Auth_PassWord = Socket_Cache.SocketProxy.Auth_PassWord;

                Properties.Settings.Default.ProxyConfig_ProxyList_NoRecord = Socket_Cache.SocketProxyList.NoRecord;
                Properties.Settings.Default.ProxyConfig_ClientList_DelClosed = Socket_Cache.SocketProxyList.DelClosed;

                Properties.Settings.Default.ProxyConfig_LogList_AutoRoll = Socket_Cache.LogList.Proxy_AutoRoll;
                Properties.Settings.Default.ProxyConfig_LogList_AutoClear = Socket_Cache.LogList.Proxy_AutoClear;
                Properties.Settings.Default.ProxyConfig_LogList_AutoClear_Value = Socket_Cache.LogList.Proxy_AutoClear_Value;

                Properties.Settings.Default.ProxyConfig_EXTProxy_EnableHttp =Socket_Cache.SocketProxy.Enable_EXTHttp;
                Properties.Settings.Default.ProxyConfig_EXTProxy_HttpIP = Socket_Cache.SocketProxy.EXTHttpIP;
                Properties.Settings.Default.ProxyConfig_EXTProxy_HttpPort = Socket_Cache.SocketProxy.EXTHttpPort;
                Properties.Settings.Default.ProxyConfig_EXTProxy_EnableHttps = Socket_Cache.SocketProxy.Enable_EXTHttps;
                Properties.Settings.Default.ProxyConfig_EXTProxy_HttpsIP = Socket_Cache.SocketProxy.EXTHttpsIP;
                Properties.Settings.Default.ProxyConfig_EXTProxy_HttpsPort = Socket_Cache.SocketProxy.EXTHttpsPort;
                Properties.Settings.Default.ProxyConfig_EXTProxy_AppointHttpPort = Socket_Cache.SocketProxy.AppointHttpPort;
                Properties.Settings.Default.ProxyConfig_EXTProxy_AppointHttpsPort = Socket_Cache.SocketProxy.AppointHttpsPort;

                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static bool SaveConfigs_SocketPacket()
        {
            bool bReturn = true;

            try
            {
                Properties.Settings.Default.FilterConfig_CheckNotShow = Socket_Cache.SocketPacket.CheckNotShow;
                Properties.Settings.Default.FilterConfig_CheckSocket = Socket_Cache.SocketPacket.CheckSocket;
                Properties.Settings.Default.FilterConfig_CheckIP = Socket_Cache.SocketPacket.CheckIP;
                Properties.Settings.Default.FilterConfig_CheckPort = Socket_Cache.SocketPacket.CheckPort;
                Properties.Settings.Default.FilterConfig_CheckHead = Socket_Cache.SocketPacket.CheckHead;
                Properties.Settings.Default.FilterConfig_CheckData = Socket_Cache.SocketPacket.CheckData;
                Properties.Settings.Default.FilterConfig_CheckSize = Socket_Cache.SocketPacket.CheckSize;

                Properties.Settings.Default.FilterConfig_CheckSocket_Value = Socket_Cache.SocketPacket.CheckSocket_Value;
                Properties.Settings.Default.FilterConfig_CheckLength_Value = Socket_Cache.SocketPacket.CheckLength_Value;
                Properties.Settings.Default.FilterConfig_CheckIP_Value = Socket_Cache.SocketPacket.CheckIP_Value;
                Properties.Settings.Default.FilterConfig_CheckPort_Value = Socket_Cache.SocketPacket.CheckPort_Value;
                Properties.Settings.Default.FilterConfig_CheckHead_Value = Socket_Cache.SocketPacket.CheckHead_Value;
                Properties.Settings.Default.FilterConfig_CheckData_Value = Socket_Cache.SocketPacket.CheckData_Value;

                Properties.Settings.Default.HookConfig_HookWS1_Send = Socket_Cache.SocketPacket.HookWS1_Send;
                Properties.Settings.Default.HookConfig_HookWS1_SendTo = Socket_Cache.SocketPacket.HookWS1_SendTo;
                Properties.Settings.Default.HookConfig_HookWS1_Recv = Socket_Cache.SocketPacket.HookWS1_Recv;
                Properties.Settings.Default.HookConfig_HookWS1_RecvFrom = Socket_Cache.SocketPacket.HookWS1_RecvFrom;
                Properties.Settings.Default.HookConfig_HookWS2_Send = Socket_Cache.SocketPacket.HookWS2_Send;
                Properties.Settings.Default.HookConfig_HookWS2_SendTo = Socket_Cache.SocketPacket.HookWS2_SendTo;
                Properties.Settings.Default.HookConfig_HookWS2_Recv = Socket_Cache.SocketPacket.HookWS2_Recv;
                Properties.Settings.Default.HookConfig_HookWS2_RecvFrom = Socket_Cache.SocketPacket.HookWS2_RecvFrom;
                Properties.Settings.Default.HookConfig_HookWSA_Send = Socket_Cache.SocketPacket.HookWSA_Send;
                Properties.Settings.Default.HookConfig_HookWSA_SendTo = Socket_Cache.SocketPacket.HookWSA_SendTo;
                Properties.Settings.Default.HookConfig_HookWSA_Recv = Socket_Cache.SocketPacket.HookWSA_Recv;
                Properties.Settings.Default.HookConfig_HookWSA_RecvFrom = Socket_Cache.SocketPacket.HookWSA_RecvFrom;

                Properties.Settings.Default.ListConfig_SocketList_AutoRoll = Socket_Cache.SocketList.AutoRoll;
                Properties.Settings.Default.ListConfig_SocketList_AutoClear = Socket_Cache.SocketList.AutoClear;
                Properties.Settings.Default.ListConfig_SocketList_AutoClear_Value = Socket_Cache.SocketList.AutoClear_Value;
                Properties.Settings.Default.ListConfig_LogList_AutoRoll = Socket_Cache.LogList.AutoRoll;
                Properties.Settings.Default.ListConfig_LogList_AutoClear = Socket_Cache.LogList.AutoClear;
                Properties.Settings.Default.ListConfig_LogList_AutoClear_Value = Socket_Cache.LogList.AutoClear_Value;

                Properties.Settings.Default.SystemConfig_SpeedMode = Socket_Cache.SocketPacket.SpeedMode;
                Properties.Settings.Default.SystemConfig_FilterList_Execute = Socket_Cache.FilterList.FilterList_Execute.ToString();

                Properties.Settings.Default.Save();
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

        public static void LoadConfigs_SocketProxy()
        {
            try
            {
                Socket_Cache.SocketProxy.ProxyIP_Auto = Properties.Settings.Default.ProxyConfig_ProxyIP_Auto;                
                Socket_Cache.SocketProxy.Enable_SOCKS5 = Properties.Settings.Default.ProxyConfig_EnableSOCKS5;
                Socket_Cache.SocketProxy.ProxyPort = Properties.Settings.Default.ProxyConfig_ProxyPort;
                Socket_Cache.SocketProxy.Enable_Auth = Properties.Settings.Default.ProxyConfig_EnableAuth;
                Socket_Cache.SocketProxy.Auth_UserName = Properties.Settings.Default.ProxyConfig_Auth_UserName;
                Socket_Cache.SocketProxy.Auth_PassWord = Properties.Settings.Default.ProxyConfig_Auth_PassWord;

                Socket_Cache.SocketProxyList.NoRecord = Properties.Settings.Default.ProxyConfig_ProxyList_NoRecord;
                Socket_Cache.SocketProxyList.DelClosed = Properties.Settings.Default.ProxyConfig_ClientList_DelClosed;

                Socket_Cache.LogList.Proxy_AutoRoll = Properties.Settings.Default.ProxyConfig_LogList_AutoRoll;
                Socket_Cache.LogList.Proxy_AutoClear = Properties.Settings.Default.ProxyConfig_LogList_AutoClear;
                Socket_Cache.LogList.Proxy_AutoClear_Value = Properties.Settings.Default.ProxyConfig_LogList_AutoClear_Value;

                Socket_Cache.SocketProxy.Enable_EXTHttp = Properties.Settings.Default.ProxyConfig_EXTProxy_EnableHttp;
                Socket_Cache.SocketProxy.EXTHttpIP = Properties.Settings.Default.ProxyConfig_EXTProxy_HttpIP;
                Socket_Cache.SocketProxy.EXTHttpPort = Properties.Settings.Default.ProxyConfig_EXTProxy_HttpPort;
                Socket_Cache.SocketProxy.Enable_EXTHttps = Properties.Settings.Default.ProxyConfig_EXTProxy_EnableHttps;
                Socket_Cache.SocketProxy.EXTHttpsIP = Properties.Settings.Default.ProxyConfig_EXTProxy_HttpsIP;
                Socket_Cache.SocketProxy.EXTHttpsPort = Properties.Settings.Default.ProxyConfig_EXTProxy_HttpsPort;
                Socket_Cache.SocketProxy.AppointHttpPort = Properties.Settings.Default.ProxyConfig_EXTProxy_AppointHttpPort;
                Socket_Cache.SocketProxy.AppointHttpsPort = Properties.Settings.Default.ProxyConfig_EXTProxy_AppointHttpsPort;

                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_35));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static void LoadConfigs_SocketPacket()
        {
            try
            {
                Socket_Cache.SocketPacket.CheckNotShow = Properties.Settings.Default.FilterConfig_CheckNotShow;
                Socket_Cache.SocketPacket.CheckSocket = Properties.Settings.Default.FilterConfig_CheckSocket;
                Socket_Cache.SocketPacket.CheckIP = Properties.Settings.Default.FilterConfig_CheckIP;
                Socket_Cache.SocketPacket.CheckPort = Properties.Settings.Default.FilterConfig_CheckPort;
                Socket_Cache.SocketPacket.CheckHead = Properties.Settings.Default.FilterConfig_CheckHead;
                Socket_Cache.SocketPacket.CheckData = Properties.Settings.Default.FilterConfig_CheckData;
                Socket_Cache.SocketPacket.CheckSize = Properties.Settings.Default.FilterConfig_CheckSize;

                Socket_Cache.SocketPacket.CheckSocket_Value = Properties.Settings.Default.FilterConfig_CheckSocket_Value;
                Socket_Cache.SocketPacket.CheckLength_Value = Properties.Settings.Default.FilterConfig_CheckLength_Value;
                Socket_Cache.SocketPacket.CheckIP_Value = Properties.Settings.Default.FilterConfig_CheckIP_Value;
                Socket_Cache.SocketPacket.CheckPort_Value = Properties.Settings.Default.FilterConfig_CheckPort_Value;
                Socket_Cache.SocketPacket.CheckHead_Value = Properties.Settings.Default.FilterConfig_CheckHead_Value;
                Socket_Cache.SocketPacket.CheckData_Value = Properties.Settings.Default.FilterConfig_CheckData_Value;             

                Socket_Cache.SocketPacket.HookWS1_Send = Properties.Settings.Default.HookConfig_HookWS1_Send;
                Socket_Cache.SocketPacket.HookWS1_SendTo = Properties.Settings.Default.HookConfig_HookWS1_SendTo;
                Socket_Cache.SocketPacket.HookWS1_Recv = Properties.Settings.Default.HookConfig_HookWS1_Recv;
                Socket_Cache.SocketPacket.HookWS1_RecvFrom = Properties.Settings.Default.HookConfig_HookWS1_RecvFrom;
                Socket_Cache.SocketPacket.HookWS2_Send = Properties.Settings.Default.HookConfig_HookWS2_Send;
                Socket_Cache.SocketPacket.HookWS2_SendTo = Properties.Settings.Default.HookConfig_HookWS2_SendTo;
                Socket_Cache.SocketPacket.HookWS2_Recv = Properties.Settings.Default.HookConfig_HookWS2_Recv;
                Socket_Cache.SocketPacket.HookWS2_RecvFrom = Properties.Settings.Default.HookConfig_HookWS2_RecvFrom;
                Socket_Cache.SocketPacket.HookWSA_Send = Properties.Settings.Default.HookConfig_HookWSA_Send;
                Socket_Cache.SocketPacket.HookWSA_SendTo = Properties.Settings.Default.HookConfig_HookWSA_SendTo;
                Socket_Cache.SocketPacket.HookWSA_Recv = Properties.Settings.Default.HookConfig_HookWSA_Recv;
                Socket_Cache.SocketPacket.HookWSA_RecvFrom = Properties.Settings.Default.HookConfig_HookWSA_RecvFrom;            

                Socket_Cache.SocketList.AutoRoll = Properties.Settings.Default.ListConfig_SocketList_AutoRoll;
                Socket_Cache.SocketList.AutoClear = Properties.Settings.Default.ListConfig_SocketList_AutoClear;
                Socket_Cache.SocketList.AutoClear_Value = Properties.Settings.Default.ListConfig_SocketList_AutoClear_Value;
                Socket_Cache.LogList.AutoRoll = Properties.Settings.Default.ListConfig_LogList_AutoRoll;
                Socket_Cache.LogList.AutoClear = Properties.Settings.Default.ListConfig_LogList_AutoClear;
                Socket_Cache.LogList.AutoClear_Value = Properties.Settings.Default.ListConfig_LogList_AutoClear_Value;                

                Socket_Cache.SocketPacket.SpeedMode = Properties.Settings.Default.SystemConfig_SpeedMode;
                Socket_Cache.FilterList.FilterList_Execute = Socket_Cache.FilterList.GetFilterListExecute_ByString(Properties.Settings.Default.SystemConfig_FilterList_Execute);

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_35));
            }
            catch (Exception ex)
            {                
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//加解密滤XML文件

        public static bool IsEncryptXMLFile(string FilePath)
        {
            bool bReturn = false;

            try
            {
                XDocument xdoc = XDocument.Load(FilePath);
                XElement xeRoot = xdoc.Root;
            }
            catch
            {
                bReturn = true;
            }

            return bReturn;
        }

        private static byte[] GetAESKeyFromString(string Password)
        {
            byte[] bReturn = null;

            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] bPW = Encoding.Default.GetBytes(Password);

                    byte[] bPW_MD5 = md5.ComputeHash(bPW);
                    string sPW_MD5 = BitConverter.ToString(bPW_MD5, 4, 8).Replace("-", "");

                    bReturn = Encoding.UTF8.GetBytes(sPW_MD5);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        public static void EncryptXMLFile(string FilePath, string Password)
        {
            try
            {
                byte[] bAES = Socket_Operation.GetAESKeyFromString(Password);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = bAES;
                    aesAlg.IV = bAES;

                    XDocument xmlDoc = XDocument.Load(FilePath);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            xmlDoc.Save(cs);
                        }

                        File.WriteAllBytes(FilePath, ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static XDocument DecryptXMLFile(string FilterList_Path, string Password)
        {
            XDocument xdReturn = new XDocument();

            try
            {
                byte[] bAES = Socket_Operation.GetAESKeyFromString(Password);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = bAES;
                    aesAlg.IV = bAES;

                    byte[] xmlBytes = File.ReadAllBytes(FilterList_Path);

                    using (MemoryStream ms = new MemoryStream(xmlBytes))
                    {
                        try
                        {
                            using (CryptoStream cs = new CryptoStream(ms, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                            {
                                xdReturn = XDocument.Load(cs);
                            }
                        }
                        catch
                        {
                            xdReturn = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return xdReturn;
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
                    Socket_Cache.LogQueue.LogToQueue(Socket_Cache.LogType.Socket, sFuncName, sLogContent);
                }
            });                                
        }

        public static void DoLog_Proxy(string sFuncName, string sLogContent)
        {
            Task.Run(() =>
            {
                if (bDoLog)
                {
                    Socket_Cache.LogQueue.LogToQueue(Socket_Cache.LogType.Proxy, sFuncName, sLogContent);
                }
            });            
        }

        #endregion

        #region//发送封包

        public static bool SendPacket(int Socket, Socket_Cache.SocketPacket.PacketType stType, string sIPFrom, string sIPTo, byte[] bSendBuffer)
        {
            bool bReturn = false;
            IntPtr ipSend = IntPtr.Zero;

            try
            {
                if (Socket > 0 && bSendBuffer.Length > 0)
                {
                    ipSend = Marshal.AllocHGlobal(bSendBuffer.Length);
                    Marshal.Copy(bSendBuffer, 0, ipSend, bSendBuffer.Length);

                    int res = -1;
                    string sIPString = string.Empty;

                    switch (stType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                            sIPString = sIPTo;
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            sIPString = sIPFrom;
                            break;
                    }

                    switch (stType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                            res = WSock32.send(Socket, ipSend, bSendBuffer.Length, SocketFlags.None);
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                            res = WS2_32.send(Socket, ipSend, bSendBuffer.Length, SocketFlags.None);
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                            if (!string.IsNullOrEmpty(sIPString))
                            {
                                Socket_Cache.SocketPacket.SockAddr saAddr = Socket_Operation.GetSocketAddr_ByIPString(sIPString);
                                res = WSock32.sendto(Socket, ipSend, bSendBuffer.Length, SocketFlags.None, ref saAddr, Marshal.SizeOf(saAddr));
                            }
                            break;
                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            if (!string.IsNullOrEmpty(sIPString))
                            {
                                Socket_Cache.SocketPacket.SockAddr saAddr = Socket_Operation.GetSocketAddr_ByIPString(sIPString);
                                res = WS2_32.sendto(Socket, ipSend, bSendBuffer.Length, SocketFlags.None, ref saAddr, Marshal.SizeOf(saAddr));
                            }
                            break;
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
            finally
            {
                if (ipSend != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ipSend);
                }
            }

            return bReturn;
        }

        #endregion        
    }
}
