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

        #region//获取所有进程

        public static DataTable GetProcess()
        {
            DataTable dtProcessList = new DataTable();
            dtProcessList.Columns.Add("ICO", typeof(Image));
            dtProcessList.Columns.Add("PName", typeof(string));
            dtProcessList.Columns.Add("PID", typeof(int));

            try
            {
                Process[] procesArr = Process.GetProcesses();
                int pCNT = procesArr.Length;

                foreach (Process p in procesArr)
                {
                    string sPName = p.ProcessName;
                    int iPID = p.Id;
                    Image iICO = IconFromFile(p);

                    DataRow dr = dtProcessList.NewRow();
                    dr["ICO"] = iICO;
                    dr["PName"] = sPName;
                    dr["PID"] = iPID;
                    dtProcessList.Rows.Add(dr);
                }

                DataView dv = dtProcessList.DefaultView;
                dv.Sort = "PName";
                dtProcessList = dv.ToTable();

                ProcessTable = dv.ToTable();
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

                            Return += iFIndex.ToString() + "-" + sHex.Substring(j * 2, 2) + ",";
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

        #region//统计封包数量

        public static void CountSocketInfo(Socket_Cache.SocketPacket.PacketType ptPacketType, int iPacketLen)
        {
            try
            {
                if (iPacketLen > 0)
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

        #region//获取当前进程的信息

        public static string GetProcessInfo()
        {
            string sReturn = string.Empty;

            try
            {
                Process pProcess = Process.GetCurrentProcess();

                string sMainWindowTitle = pProcess.MainWindowTitle;
                string sMainWindowHandle = pProcess.MainWindowHandle.ToString();

                if (String.IsNullOrEmpty(sMainWindowTitle))
                {
                    sReturn = pProcess.MainModule.ModuleName;
                }
                else
                {  
                    sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_73), pProcess.MainWindowTitle, pProcess.MainWindowHandle.ToString());
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
                Socket_Cache.Support_WS1 = false;
                Socket_Cache.Support_WS2 = false;                

                foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
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

                if (Socket_Cache.Support_WS1)
                {
                    sReturn += " 1.1";
                }

                if (Socket_Cache.Support_WS2)
                {
                    sReturn += " 2.0";
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
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

        #region//处理 Hook 结果（异步）

        public static void ProcessingHookResult(
            Int32 socket,
            byte[] bRawBuffer,
            byte[] bBuffer, 
            Int32 res, 
            Socket_Cache.SocketPacket.PacketType ptType, 
            Socket_Cache.Filter.FilterAction FilterAction, 
            Socket_Cache.SocketPacket.sockaddr sockaddr)
        {
            try
            {
                Task.Run(() =>
                {
                    Socket_Operation.CountSocketInfo(ptType, res);

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
                                if (res != bBuffer.Length)
                                {
                                    byte[] bRes = new byte[res];
                                    Buffer.BlockCopy(bBuffer, 0, bRes, 0, res);

                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bRawBuffer, bRes, ptType, sockaddr, FilterAction);
                                }
                                else
                                {
                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bRawBuffer, bBuffer, ptType, sockaddr, FilterAction);
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
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

        #region//获取对应名称的树节点

        public static TreeNode FindNodeByName(TreeNodeCollection nodes, string Name)
        {
            try
            {
                if (nodes == null)
                {
                    return null;
                }

                foreach (TreeNode node in nodes)
                {
                    if (node.Text == Name)
                    {
                        return node;
                    }

                    TreeNode foundNode = FindNodeByName(node.Nodes, Name);
                    if (foundNode != null)
                    {
                        return foundNode;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return null;
        }

        #endregion

        #region//数据对比

        public static void CompareData(RichTextBox rtbCompare, string sText_A, string sText_B)
        {
            try
            {
                rtbCompare.Clear();

                if (sText_A == sText_B)
                {
                    Socket_Operation.AppendColoredText(rtbCompare, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_29), Color.Blue);
                    return;
                }

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
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
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
                        if (!ISFilter_BySocket(spi.PacketSocket))
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
                        if (!(ISFilter_ByIP(spi.PacketFrom) || ISFilter_ByIP(spi.PacketTo)))
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
                        if (!(ISFilter_ByPort(spi.PacketFrom) || ISFilter_ByPort(spi.PacketTo)))
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
                        if (!ISFilter_ByHead(spi.PacketBuffer))
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
                        if (!ISFilter_ByPacket(spi.PacketBuffer))
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
                        if (!ISFilter_BySize(spi.PacketLen))
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

        private static bool ISFilter_BySocket(int iPacketSocket)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.CheckSocket_Value))
                {
                    string[] sSocketArr = Socket_Cache.CheckSocket_Value.Split(';');

                    foreach (string sSocket in sSocketArr)
                    {
                        if (!string.IsNullOrEmpty(sSocket))
                        {
                            if (int.TryParse(sSocket, out int iCheckSocket))
                            {
                                if (iPacketSocket == iCheckSocket)
                                {
                                    return true;
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

        private static bool ISFilter_ByIP(string sPacketIP)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(sPacketIP))
                {
                    string sIP = sPacketIP.Split(':')[0];
                    string sPort = sPacketIP.Split(':')[1];
                    
                    if (!string.IsNullOrEmpty(Socket_Cache.CheckIP_Value))
                    {
                        string[] sIPArr = Socket_Cache.CheckIP_Value.Split(';');

                        foreach (string sCheckIP in sIPArr)
                        {
                            if (!string.IsNullOrEmpty(sCheckIP))
                            {
                                if (sIP.Equals(sCheckIP))
                                {
                                    return true;
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

        private static bool ISFilter_ByPort(string sPacketPort)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(sPacketPort))
                {
                    string sIP = sPacketPort.Split(':')[0];
                    string sPort = sPacketPort.Split(':')[1];

                    if (!string.IsNullOrEmpty(Socket_Cache.CheckPort_Value))
                    {
                        string[] sPortArr = Socket_Cache.CheckPort_Value.Split(';');

                        foreach (string sCheckPort in sPortArr)
                        {
                            if (!string.IsNullOrEmpty(sCheckPort))
                            {
                                if (sPort.Equals(sCheckPort))
                                {
                                    return true;
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

        private static bool ISFilter_ByHead(byte[] bBuffer)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.CheckHead_Value))
                {
                    string sPacket = BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer).Replace(" ", "");

                    string[] sHeadArr = Socket_Cache.CheckHead_Value.Replace(" ", "").Split(';');

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

        private static bool ISFilter_ByPacket(byte[] bBuffer)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.CheckData_Value))
                {
                    string sPacket = BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer).Replace(" ", "");

                    string[] sPacketArr = Socket_Cache.CheckData_Value.Replace(" ", "").Split(';');

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

        private static bool ISFilter_BySize(int PacketLength)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.CheckLength_Value))
                {
                    string[] sLengthArr = Socket_Cache.CheckLength_Value.Split(';');

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

                    for (int i = 9001;i < 9013;i ++)
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
        }

        #endregion

        #region//保存系统设置

        public static bool SaveConfigs()
        {
            bool bReturn = true;

            try
            {
                Properties.Settings.Default.FilterConfig_CheckNotShow = Socket_Cache.CheckNotShow;
                Properties.Settings.Default.FilterConfig_CheckSocket = Socket_Cache.CheckSocket;
                Properties.Settings.Default.FilterConfig_CheckIP = Socket_Cache.CheckIP;
                Properties.Settings.Default.FilterConfig_CheckPort = Socket_Cache.CheckPort;
                Properties.Settings.Default.FilterConfig_CheckHead = Socket_Cache.CheckHead;
                Properties.Settings.Default.FilterConfig_CheckData = Socket_Cache.CheckData;
                Properties.Settings.Default.FilterConfig_CheckSize = Socket_Cache.CheckSize;

                Properties.Settings.Default.FilterConfig_CheckSocket_Value = Socket_Cache.CheckSocket_Value;
                Properties.Settings.Default.FilterConfig_CheckLength_Value = Socket_Cache.CheckLength_Value;
                Properties.Settings.Default.FilterConfig_CheckIP_Value = Socket_Cache.CheckIP_Value;
                Properties.Settings.Default.FilterConfig_CheckPort_Value = Socket_Cache.CheckPort_Value;
                Properties.Settings.Default.FilterConfig_CheckHead_Value = Socket_Cache.CheckHead_Value;
                Properties.Settings.Default.FilterConfig_CheckData_Value = Socket_Cache.CheckData_Value;                

                Properties.Settings.Default.HookConfig_HookSend = Socket_Cache.HookSend;
                Properties.Settings.Default.HookConfig_HookSendTo = Socket_Cache.HookSendTo;
                Properties.Settings.Default.HookConfig_HookRecv = Socket_Cache.HookRecv;
                Properties.Settings.Default.HookConfig_HookRecvFrom = Socket_Cache.HookRecvFrom;
                Properties.Settings.Default.HookConfig_HookWSASend = Socket_Cache.HookWSASend;
                Properties.Settings.Default.HookConfig_HookWSASendTo = Socket_Cache.HookWSASendTo;
                Properties.Settings.Default.HookConfig_HookWSARecv = Socket_Cache.HookWSARecv;
                Properties.Settings.Default.HookConfig_HookWSARecvFrom = Socket_Cache.HookWSARecvFrom;

                Properties.Settings.Default.ListConfig_SocketList_AutoRoll = Socket_Cache.SocketList.AutoRoll;
                Properties.Settings.Default.ListConfig_SocketList_AutoClear = Socket_Cache.SocketList.AutoClear;
                Properties.Settings.Default.ListConfig_SocketList_AutoClear_Value = Socket_Cache.SocketList.AutoClear_Value;
                Properties.Settings.Default.ListConfig_LogList_AutoRoll = Socket_Cache.LogList.AutoRoll;
                Properties.Settings.Default.ListConfig_LogList_AutoClear = Socket_Cache.LogList.AutoClear;
                Properties.Settings.Default.ListConfig_LogList_AutoClear_Value = Socket_Cache.LogList.AutoClear_Value;                

                Properties.Settings.Default.SystemConfig_SpeedMode = Socket_Cache.SpeedMode;
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

        public static void LoadConfigs()
        {
            try
            {
                Socket_Cache.CheckNotShow = Properties.Settings.Default.FilterConfig_CheckNotShow;
                Socket_Cache.CheckSocket = Properties.Settings.Default.FilterConfig_CheckSocket;
                Socket_Cache.CheckIP = Properties.Settings.Default.FilterConfig_CheckIP;
                Socket_Cache.CheckPort = Properties.Settings.Default.FilterConfig_CheckPort;
                Socket_Cache.CheckHead = Properties.Settings.Default.FilterConfig_CheckHead;
                Socket_Cache.CheckData = Properties.Settings.Default.FilterConfig_CheckData;
                Socket_Cache.CheckSize = Properties.Settings.Default.FilterConfig_CheckSize;

                Socket_Cache.CheckSocket_Value = Properties.Settings.Default.FilterConfig_CheckSocket_Value;
                Socket_Cache.CheckLength_Value = Properties.Settings.Default.FilterConfig_CheckLength_Value;
                Socket_Cache.CheckIP_Value = Properties.Settings.Default.FilterConfig_CheckIP_Value;
                Socket_Cache.CheckPort_Value = Properties.Settings.Default.FilterConfig_CheckPort_Value;
                Socket_Cache.CheckHead_Value = Properties.Settings.Default.FilterConfig_CheckHead_Value;
                Socket_Cache.CheckData_Value = Properties.Settings.Default.FilterConfig_CheckData_Value;             

                Socket_Cache.HookSend = Properties.Settings.Default.HookConfig_HookSend;
                Socket_Cache.HookSendTo = Properties.Settings.Default.HookConfig_HookSendTo;
                Socket_Cache.HookRecv = Properties.Settings.Default.HookConfig_HookRecv;
                Socket_Cache.HookRecvFrom = Properties.Settings.Default.HookConfig_HookRecvFrom;
                Socket_Cache.HookWSASend = Properties.Settings.Default.HookConfig_HookWSASend;
                Socket_Cache.HookWSASendTo = Properties.Settings.Default.HookConfig_HookWSASendTo;
                Socket_Cache.HookWSARecv = Properties.Settings.Default.HookConfig_HookWSARecv;
                Socket_Cache.HookWSARecvFrom = Properties.Settings.Default.HookConfig_HookWSARecvFrom;            

                Socket_Cache.SocketList.AutoRoll = Properties.Settings.Default.ListConfig_SocketList_AutoRoll;
                Socket_Cache.SocketList.AutoClear = Properties.Settings.Default.ListConfig_SocketList_AutoClear;
                Socket_Cache.SocketList.AutoClear_Value = Properties.Settings.Default.ListConfig_SocketList_AutoClear_Value;
                Socket_Cache.LogList.AutoRoll = Properties.Settings.Default.ListConfig_LogList_AutoRoll;
                Socket_Cache.LogList.AutoClear = Properties.Settings.Default.ListConfig_LogList_AutoClear;
                Socket_Cache.LogList.AutoClear_Value = Properties.Settings.Default.ListConfig_LogList_AutoClear_Value;                

                Socket_Cache.SpeedMode = Properties.Settings.Default.SystemConfig_SpeedMode;
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
                byte[] bAES = GetAESKeyFromString(Password);

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
                byte[] bAES = GetAESKeyFromString(Password);

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
            if (bDoLog)
            {
                Task.Run(() =>
                {
                    Socket_Cache.LogQueue.LogToQueue(Socket_Cache.LogType.Socket, sFuncName, sLogContent);
                });
            }                     
        }

        public static void DoLog_Proxy(string sFuncName, string sLogContent)
        {
            if (bDoLog)
            {
                Task.Run(() =>
                {
                    Socket_Cache.LogQueue.LogToQueue(Socket_Cache.LogType.Proxy, sFuncName, sLogContent);
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
