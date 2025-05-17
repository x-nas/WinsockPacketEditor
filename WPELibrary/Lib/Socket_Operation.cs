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
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using WPELibrary.Lib.WebAPI;
using System.Management;

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

        #region//密码字典

        private static readonly Dictionary<char, string> encryptionMap = new Dictionary<char, string>
        {
            {'!', "966"},
            {'"', "965"},
            {'#', "964"},
            {'$', "963"},
            {'%', "962"},
            {'^', "961"},
            {'&', "960"},
            {'*', "959"},
            {'(', "958"},
            {')', "957"},
            {'+', "956"},
            {',', "955"},
            {'-', "954"},
            {'.', "953"},
            {'/', "952"},
            {'0', "951"},
            {'1', "950"},
            {'2', "949"},
            {'3', "948"},
            {'4', "947"},
            {'5', "946"},
            {'6', "945"},
            {'7', "944"},
            {'8', "943"},
            {'9', "942"},
            {':', "941"},
            {';', "940"},
            {'<', "939"},
            {'=', "938"},
            {'>', "937"},
            {'?', "936"},
            {'@', "935"},
            {'A', "934"},
            {'B', "933"},
            {'C', "932"},
            {'D', "931"},
            {'E', "930"},
            {'F', "929"},
            {'G', "928"},
            {'H', "927"},
            {'I', "926"},
            {'J', "925"},
            {'K', "924"},
            {'L', "923"},
            {'M', "922"},
            {'N', "921"},
            {'O', "920"},
            {'P', "919"},
            {'Q', "918"},
            {'R', "917"},
            {'S', "916"},
            {'T', "915"},
            {'U', "914"},
            {'V', "913"},
            {'W', "912"},
            {'X', "911"},
            {'Y', "910"},
            {'Z', "909"},
            {'[', "908"},
            {'\\', "907"},
            {']', "906"},
            {'_', "904"},
            {'`', "903"},
            {'a', "902"},
            {'b', "901"},
            {'c', "900"},
            {'d', "899"},
            {'e', "898"},
            {'f', "897"},
            {'g', "896"},
            {'h', "895"},
            {'i', "894"},
            {'j', "893"},
            {'k', "892"},
            {'l', "891"},
            {'m', "890"},
            {'n', "889"},
            {'o', "888"},
            {'p', "887"},
            {'q', "886"},
            {'r', "885"},
            {'s', "884"},
            {'t', "883"},
            {'u', "882"},
            {'v', "881"},
            {'w', "880"},
            {'x', "879"},
            {'y', "878"},
            {'z', "877"},
            {'{', "876"},
            {'|', "875"},
            {'}', "874"},
            {'~', "873"}
        };

        private static readonly Dictionary<string, char> decryptionMap = encryptionMap.ToDictionary(kv => kv.Value, kv => kv.Key);

        #endregion

        #region//判断是否为64位的进程

        public static bool IsWin64Process(int ProcessID)
        {
            bool bReturn = false;

            try
            {
                using (Process pProcess = Process.GetProcessById(ProcessID))
                {
                    if (pProcess != null)
                    {
                        if ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
                        {
                            bool retVal;
                            if (Kernel32.IsWow64Process(pProcess.Handle, out retVal))
                            {
                                bReturn = !retVal;
                            }
                        }
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
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    HttpResponseMessage response = await client.GetAsync(sURL);

                    if (response.IsSuccessStatusCode)
                    {
                        bReturn = true;
                    }
                }  
            }
            catch
            {
                bReturn = false;
            }

            return bReturn;
        }

        #endregion        

        #region//启动远程管理

        public static void StartRemoteMGT()
        {
            try
            {
                if (Socket_Cache.System.IsRemote)
                {
                    if (!string.IsNullOrEmpty(Socket_Cache.System.Remote_URL) &&
                        !string.IsNullOrEmpty(Socket_Cache.System.Remote_UserName) &&
                        !string.IsNullOrEmpty(Socket_Cache.System.Remote_PassWord))
                    {
                        string sLog = string.Empty;

                        try
                        {
                            Socket_Cache.System.WebServer = WebApp.Start<Socket_Web>(Socket_Cache.System.Remote_URL);

                            sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_178), Socket_Cache.System.Remote_URL);
                        }
                        catch
                        {
                            sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_179), Process.GetCurrentProcess().ProcessName);
                        }

                        Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static void StopRemoteMGT(Socket_Cache.System.SystemMode FromMode)
        {
            try
            {
                if (FromMode == Socket_Cache.System.StartMode)
                {
                    if (Socket_Cache.System.WebServer != null)
                    {
                        Socket_Cache.System.WebServer.Dispose();
                    }
                }                         
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//获取CPU和内存使用率

        public static async void InitCPUAndMemoryCounter()
        {
            await Task.Run(() =>
            {
                try
                {
                    Socket_Cache.System.cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    Socket_Cache.System.cpuCounter.NextValue();
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            });            
        }

        public static string[] GetCPUAndMemory()
        {
            string[] sReturn = new string[2];

            try
            {
                if (Socket_Cache.System.cpuCounter != null)
                {
                    // 获取CPU使用率
                    float cpuUsage = Socket_Cache.System.cpuCounter.NextValue();
                    sReturn[0] = $"{cpuUsage:F2}%";

                    // 获取内存使用率
                    string query = "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem";
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            ulong totalMemory = Convert.ToUInt64(obj["TotalVisibleMemorySize"]) / 1024; // MB
                            ulong freeMemory = Convert.ToUInt64(obj["FreePhysicalMemory"]) / 1024; // MB
                            ulong usedMemory = totalMemory - freeMemory;
                            float memoryUsagePercent = (float)usedMemory / totalMemory * 100;

                            sReturn[1] = $"{memoryUsagePercent:F1}%";
                        }
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

        private static Image IconFromFile(Process process)
        {
            string filePath = GetFilePath(process);
            if (string.IsNullOrEmpty(filePath))
            {
                return new Icon(SystemIcons.Application, 256, 256).ToBitmap();
            }

            try
            {
                var extractor = new IconExtractor.IconExtractor(filePath);
                var icon = extractor.GetIcon(0);
                if (icon != null)
                {
                    var splitIcons = IconExtractor.IconUtil.Split(icon);
                    return GetBestIcon(splitIcons);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            try
            {
                return Icon.ExtractAssociatedIcon(filePath)?.ToBitmap();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return new Icon(SystemIcons.Application, 256, 256).ToBitmap();
        }

        private static string GetFilePath(Process process)
        {
            try
            {
                return process.MainModule.FileName.Replace(".ni.dll", ".dll");
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return null;
            }
        }

        private static Image GetBestIcon(Icon[] icons)
        {
            if (icons == null || icons.Length == 0)
            {
                return null;
            }

            Icon bestIcon = icons[0];

            foreach (var icon in icons)
            {
                if (IconExtractor.IconUtil.GetBitCount(icon) > IconExtractor.IconUtil.GetBitCount(bestIcon))
                {
                    bestIcon = icon;
                }
                else if (IconExtractor.IconUtil.GetBitCount(icon) == IconExtractor.IconUtil.GetBitCount(bestIcon) && icon.Width > bestIcon.Width)
                {
                    bestIcon = icon;
                }
            }

            return bestIcon.ToBitmap();
        }

        #endregion

        #region//密码字典        

        public static string PassWord_Encrypt(string plainText)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText))
                {
                    return string.Empty;
                }                    

                StringBuilder encrypted = new StringBuilder();
                foreach (char c in plainText)
                {
                    if (encryptionMap.TryGetValue(c, out string code))
                    {
                        encrypted.Append(code);
                    }
                    else
                    {
                        encrypted.Append(c);
                    }
                }

                return encrypted.ToString();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return string.Empty;
        }
        
        public static string PassWord_Decrypt(string encryptedText)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText))
                {
                    return string.Empty;
                }                    

                StringBuilder plainText = new StringBuilder();

                int i = 0;
                while (i < encryptedText.Length)
                {
                    if (i + 3 <= encryptedText.Length)
                    {
                        string code = encryptedText.Substring(i, 3);
                        if (decryptionMap.TryGetValue(code, out char c))
                        {
                            plainText.Append(c);
                            i += 3;
                            continue;
                        }
                    }

                    plainText.Append(encryptedText[i]);
                    i++;
                }

                return plainText.ToString();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return string.Empty;
        }

        #endregion

        #region//将数据写回内存

        public static unsafe void CopyBufferToIntPtr(IntPtr lpBuffer, byte[] bBuffer)
        {
            if (bBuffer.Length > 0)
            {
                fixed (byte* bufferPtr = bBuffer)
                {
                    Buffer.MemoryCopy(bufferPtr, (void*)lpBuffer, bBuffer.Length, bBuffer.Length);
                }
            }
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
                byte[] bBuffer = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF8, sString);
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
                sReturn = Encoding.UTF8.GetString(bBuffer);
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

        public static string BytesToString(Socket_Cache.SocketPacket.EncodingFormat efFormat, ReadOnlySpan<byte> buffer)
        {
            string sReturn = string.Empty;

            try
            {
                if (buffer.Length > 0)
                {
                    switch (efFormat)
                    {
                        case Socket_Cache.SocketPacket.EncodingFormat.Default:
                            sReturn = Encoding.Default.GetString(buffer.ToArray());
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Char:
                            char c = (char)buffer[0];
                            sReturn = (char.IsControl(c) ? "." : c.ToString());
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Byte:
                            sReturn = buffer[0].ToString();
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Bytes:
                            StringBuilder sbBytes = new StringBuilder();
                            foreach (byte b in buffer)
                            {
                                sbBytes.Append(b).Append(",");
                            }
                            sReturn = sbBytes.ToString().TrimEnd(',');
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Short:
                            if (buffer.Length >= 2)
                            {
                                sReturn = BitConverter.ToInt16(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UShort:
                            if (buffer.Length >= 2)
                            {
                                sReturn = BitConverter.ToUInt16(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Int32:
                            if (buffer.Length >= 4)
                            {
                                sReturn = BitConverter.ToInt32(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UInt32:
                            if (buffer.Length >= 4)
                            {
                                sReturn = BitConverter.ToUInt32(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Int64:
                            if (buffer.Length >= 8)
                            {
                                sReturn = BitConverter.ToInt64(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UInt64:
                            if (buffer.Length >= 8)
                            {
                                sReturn = BitConverter.ToUInt64(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Float:
                            if (buffer.Length >= 4)
                            {
                                sReturn = BitConverter.ToSingle(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Double:
                            if (buffer.Length >= 8)
                            {
                                sReturn = BitConverter.ToDouble(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Bin:
                            StringBuilder sbBin = new StringBuilder();
                            foreach (byte b in buffer)
                            {
                                sbBin.Append(Convert.ToString(b, 2).PadLeft(8, '0')).Append(" ");
                            }
                            sReturn = sbBin.ToString().Trim();
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Hex:
                            StringBuilder sbHex = new StringBuilder();
                            foreach (byte b in buffer)
                            {
                                sbHex.Append(b.ToString("X2")).Append(" ");
                            }
                            sReturn = sbHex.ToString().Trim();
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.GBK:
                            sReturn = Encoding.GetEncoding("GBK").GetString(buffer.ToArray());
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.Unicode:
                            sReturn = Encoding.Unicode.GetString(buffer.ToArray());
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.ASCII:
                            sReturn = Encoding.ASCII.GetString(buffer.ToArray());
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UTF7:
                            sReturn = Encoding.UTF7.GetString(buffer.ToArray());
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UTF8:
                            sReturn = Encoding.UTF8.GetString(buffer.ToArray());
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UTF16:
                            sReturn = Encoding.BigEndianUnicode.GetString(buffer.ToArray());
                            break;

                        case Socket_Cache.SocketPacket.EncodingFormat.UTF32:
                            sReturn = Encoding.UTF32.GetString(buffer.ToArray());
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
            if (string.IsNullOrEmpty(hexString))
            {
                return Array.Empty<byte>();
            }

            try
            {
                hexString = hexString.Replace(" ", "");

                if ((hexString.Length % 2) != 0)
                {
                    hexString += " ";
                }

                byte[] returnBytes = new byte[hexString.Length / 2];
                Span<byte> span = returnBytes.AsSpan();

                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return returnBytes;
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        #endregion

        #region//判断是否十六进制字符串（带空格）

        public static bool IsHexString(string value)
        {
            bool bReturn = false;

            try
            {
                const string pattern = @"^([A-Fa-f0-9]{2}\s?)+$";
                Regex regex = new Regex(pattern, RegexOptions.Compiled);
                bReturn = regex.IsMatch(value);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;            
        }

        public static bool IsValidFilterString(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return Socket_Operation.IsHexString(value);
            }
            else
            {
                return false;
            }
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

        public static ushort ByteArrayToInt16BigEndian(ReadOnlySpan<byte> bytes)
        {
            ushort uReturn = 0;

            try
            {
                if (bytes.Length == 2)
                {
                    uReturn = (ushort)(bytes[0] << 8 | bytes[1]);
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

        public static void CountSocketInfo(Socket_Cache.SocketPacket.PacketType ptPacketType, int packetLength)
        {
            try
            {
                if (packetLength > 0)
                {
                    Interlocked.Increment(ref Socket_Cache.SocketPacket.TotalPackets);

                    switch (ptPacketType)
                    {
                        case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                        case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                            Interlocked.Increment(ref Socket_Cache.SocketQueue.Send_CNT);
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, packetLength);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                        case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                            Interlocked.Increment(ref Socket_Cache.SocketQueue.SendTo_CNT);
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, packetLength);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASend:
                            Interlocked.Increment(ref Socket_Cache.SocketQueue.WSASend_CNT);
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, packetLength);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                            Interlocked.Increment(ref Socket_Cache.SocketQueue.WSASendTo_CNT);
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_SendBytes, packetLength);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                        case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                            Interlocked.Increment(ref Socket_Cache.SocketQueue.Recv_CNT);
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, packetLength);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                        case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                            Interlocked.Increment(ref Socket_Cache.SocketQueue.RecvFrom_CNT);
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, packetLength);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecv:
                        case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                            Interlocked.Increment(ref Socket_Cache.SocketQueue.WSARecv_CNT);
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, packetLength);
                            break;

                        case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                            Interlocked.Increment(ref Socket_Cache.SocketQueue.WSARecvFrom_CNT);
                            Interlocked.Add(ref Socket_Cache.SocketPacket.Total_RecvBytes, packetLength);
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

        #region//统计代理速率

        public static void CountProxySpeed(Socket_Cache.SocketProxy.ProxySpeedType psType, int ProxySpeed)
        {
            try
            {
                if (ProxySpeed > 0)
                {
                    switch (psType)
                    {
                        case Socket_Cache.SocketProxy.ProxySpeedType.Uplink:
                            Interlocked.Add(ref Socket_Cache.SocketProxy.ProxySpeed_Uplink, ProxySpeed);
                            break;

                        case Socket_Cache.SocketProxy.ProxySpeedType.Downlink:
                            Interlocked.Add(ref Socket_Cache.SocketProxy.ProxySpeed_Downlink, ProxySpeed);
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

        public static bool CheckDataIsMatchProxyStep(ReadOnlySpan<byte> bData, Socket_Cache.SocketProxy.ProxyStep proxyStep)
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

        public static bool CheckHttpProxyState(Socket_ProxyTCP spi, string targetAddress, ushort targetPort)
        {
            bool bReturn = false;

            try
            {
                ReadOnlySpan<byte> bRequest = Socket_Operation.BuildHttpProxyRequest(targetAddress, targetPort);
                if (!bRequest.IsEmpty)
                {
                    Socket_Operation.SendTCPData(spi.ServerSocket, bRequest);                    

                    Span<byte> buffer = stackalloc byte[4096];
                    int bytesRead = spi.ServerSocket.Receive(buffer.ToArray());

                    if (bytesRead > 0)
                    {
                        ReadOnlySpan<byte> responseBytes = buffer.Slice(0, bytesRead);
                        string response = Encoding.UTF8.GetString(responseBytes.ToArray());

                        if (response.StartsWith("HTTP/1.1 200"))
                        {
                            bReturn = true;
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        private static ReadOnlySpan<byte> BuildHttpProxyRequest(string targetAddress, ushort targetPort)
        {
            ReadOnlySpan<byte> bReturn = default;

            try
            {
                StringBuilder requestBuilder = new StringBuilder();
                requestBuilder.Append($"CONNECT {targetAddress}:{targetPort} HTTP/1.1\r\n");
                requestBuilder.Append($"Host: {targetAddress}:{targetPort}\r\n");
                requestBuilder.Append("Proxy-Connection: Keep-Alive\r\n\r\n");

                bReturn = Encoding.UTF8.GetBytes(requestBuilder.ToString());
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
                dtcReturn.FillWeight = 50;
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

        #region//获取系统运行模式名称

        public static string GetSystemModeName()
        {
            string sReturn = string.Empty;
            
            try
            {
                switch (Socket_Cache.System.StartMode)
                {
                    case Socket_Cache.System.SystemMode.Proxy:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_185);
                        break;

                    case Socket_Cache.System.SystemMode.Process:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_186);
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

        #region//获取工作模式名称

        public static string GetWorkModeName(bool IsSpeedMode)
        {
            string sReturn = string.Empty;

            try
            {
                if (IsSpeedMode)
                {
                    sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_187);
                }
                else
                {
                    sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_188);
                }                    
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return sReturn;
        }

        #endregion

        #region//获取当前进程的格式化名称

        public static string GetProcessName()
        {
            string sReturn = string.Empty;

            try
            {
                Process pProcess = Process.GetCurrentProcess();
                Socket_Cache.SocketPacket.InjectProcess = string.Format("{0} [{1}]", pProcess.ProcessName, RemoteHooking.GetCurrentProcessId());

                sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_20) + Socket_Cache.SocketPacket.InjectProcess;
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

        public static async Task ProcessingHookResult(
            Int32 socket,
            byte[] bRawBuffer,
            byte[] bBuffer, 
            Int32 res, 
            Socket_Cache.SocketPacket.PacketType ptType, 
            Socket_Cache.Filter.FilterAction FilterAction, 
            Socket_Cache.SocketPacket.SockAddr sockaddr)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                    {
                        if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept || res > 0)
                        {
                            Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bRawBuffer, bBuffer, ptType, sockaddr, FilterAction);
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            });            
        }

        #endregion

        #region//获取 SockAddr 对应的 IP 地址和端口

        public static string GetIPString_BySocketAddr(int pSocket, Socket_Cache.SocketPacket.SockAddr pAddr, Socket_Cache.SocketPacket.PacketType pType)
        {
            string sIP_From = string.Empty;
            string sIP_To = string.Empty;

            try
            {
                sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.From);

                switch (pType)
                {
                    case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                    case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                    case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                    case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                    case Socket_Cache.SocketPacket.PacketType.WSASend:
                    case Socket_Cache.SocketPacket.PacketType.WSARecv:
                    case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                        sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Socket_Cache.SocketPacket.IPType.To);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                    case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                    case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                    case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                    case Socket_Cache.SocketPacket.PacketType.WSASendTo:
                    case Socket_Cache.SocketPacket.PacketType.WSARecvFrom:
                        sIP_To = Socket_Operation.GetIP_BySockAddr(pAddr);
                        break;
                }

                if (!string.IsNullOrEmpty(sIP_From) && !string.IsNullOrEmpty(sIP_To))
                {
                    var sb = new StringBuilder(sIP_From);
                    sb.Append("|");
                    sb.Append(sIP_To);
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return string.Empty;
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
                    sReturn = $"{sIP}:{sPort}";
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

        public static string GetIP_ByAddressType(Socket_Cache.SocketProxy.AddressType addressType, ReadOnlySpan<byte> bData)
        { 
            string sReturn = string.Empty;

            try
            {
                Socket_Cache.SocketProxy.IPAddressAndPort result = ExtractIPAddressAndPort(addressType, bData);
                sReturn = result.IPAddress.ToString();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        public static IPEndPoint GetIPEndPoint_ByAddressType(Socket_Cache.SocketProxy.AddressType addressType, ReadOnlySpan<byte> bData)
        {
            IPEndPoint epReturn = null;

            try
            {
                Socket_Cache.SocketProxy.IPAddressAndPort result = ExtractIPAddressAndPort(addressType, bData);
                epReturn = new IPEndPoint(result.IPAddress, result.Port);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return epReturn;
        }

        public static Socket_Cache.SocketProxy.IPAddressAndPort ExtractIPAddressAndPort(Socket_Cache.SocketProxy.AddressType addressType, ReadOnlySpan<byte> bData)
        {
            IPAddress ip = IPAddress.Any;
            ushort port = 0;

            try
            {
                switch (addressType)
                {
                    case Socket_Cache.SocketProxy.AddressType.IPV4:
                        ip = new IPAddress(bData.Slice(0, 4).ToArray());
                        port = Socket_Operation.ByteArrayToInt16BigEndian(bData.Slice(4, 2));
                        break;

                    case Socket_Cache.SocketProxy.AddressType.Domain:
                        byte length = bData[0];
                        ReadOnlySpan<byte> domainBytes = bData.Slice(1, length);
                        string sIPString = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, domainBytes);

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

                        port = Socket_Operation.ByteArrayToInt16BigEndian(bData.Slice(1 + length, 2));
                        break;

                    case Socket_Cache.SocketProxy.AddressType.IPV6:
                        ip = new IPAddress(bData.Slice(0, 16).ToArray());
                        port = Socket_Operation.ByteArrayToInt16BigEndian(bData.Slice(16, 2));
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return new Socket_Cache.SocketProxy.IPAddressAndPort { IPAddress = ip, Port = port };
        }

        #endregion

        #region//获取UDP数据包

        public static ReadOnlySpan<byte> GetUDPData_ByAddressType(Socket_Cache.SocketProxy.AddressType addressType, ReadOnlySpan<byte> bData)
        {
            try
            {
                switch (addressType)
                {
                    case Socket_Cache.SocketProxy.AddressType.IPV4: 
                        return bData.Slice(10);

                    case Socket_Cache.SocketProxy.AddressType.Domain:
                        byte LENGTH = bData[4];
                        return bData.Slice(LENGTH + 7);

                    case Socket_Cache.SocketProxy.AddressType.IPV6:
                        return bData.Slice(22);

                    default:
                        return ReadOnlySpan<byte>.Empty;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                return ReadOnlySpan<byte>.Empty;
            }
        }

        #endregion

        #region//获取返回给客户端的数据（SOCKS5，IPV4）

        public static byte[] GetProxyReturnData(Socket_Cache.SocketProxy.CommandResponse CommandResponse, ReadOnlySpan<byte> bServerIP, ReadOnlySpan<byte> bServerPort)
        {
            try
            {
                Span<byte> response = stackalloc byte[10];
                response[0] = (byte)Socket_Cache.SocketProxy.ProxyType.Socket5;
                response[1] = (byte)CommandResponse;
                response[2] = 0x00;
                response[3] = (byte)Socket_Cache.SocketProxy.AddressType.IPV4;
                bServerIP.CopyTo(response.Slice(4, 4));
                response[8] = bServerPort[1];
                response[9] = bServerPort[0];

                return response.ToArray();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                return Array.Empty<byte>();
            }
        }

        #endregion

        #region//获取封包数据字符串（十六进制）

        public static string GetPacketData_Hex(Span<byte> bBuff, int Max_DataLen)
        {
            string sReturn = string.Empty;

            try
            {
                int iPacketLen = bBuff.Length;

                if (iPacketLen > Max_DataLen)
                {
                    Span<byte> bBuffSlice = bBuff.Slice(0, Max_DataLen);                
                    sReturn = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffSlice) + " ...";
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

        #region//获取指定步长的Byte

        public static byte GetStepByte(byte bStepByte, int iStepLen)
        {
            int iStepValue = bStepByte + iStepLen;

            iStepValue = (iStepValue % 256 + 256) % 256;

            return (byte)iStepValue;
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

        #region//获取端口对应的域名类型

        public static Socket_Cache.SocketProxy.DomainType GetDomainType_ByPort(ushort Port)
        {
            try
            {
                if (Port == 80 || Port == 8080)
                {
                    return Socket_Cache.SocketProxy.DomainType.Http;
                }
                else if (Port == 443 || Port == 8443)
                {
                    return Socket_Cache.SocketProxy.DomainType.Https;
                }

                if (Socket_Cache.SocketProxy.Enable_EXTHttp && !string.IsNullOrEmpty(Socket_Cache.SocketProxy.AppointHttpPort))
                {
                    HashSet<string> httpPorts = new HashSet<string>(Socket_Cache.SocketProxy.AppointHttpPort.Split(','));
                    if (httpPorts.Contains(Port.ToString()))
                    {
                        return Socket_Cache.SocketProxy.DomainType.Http;
                    }
                }                

                if (Socket_Cache.SocketProxy.Enable_EXTHttps && !string.IsNullOrEmpty(Socket_Cache.SocketProxy.AppointHttpsPort))
                {
                    HashSet<string> httpsPorts = new HashSet<string>(Socket_Cache.SocketProxy.AppointHttpsPort.Split(','));
                    if (httpsPorts.Contains(Port.ToString()))
                    {
                        return Socket_Cache.SocketProxy.DomainType.Https;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return Socket_Cache.SocketProxy.DomainType.Socket;
        }

        #endregion

        #region//获取启用的代理账号数

        public static int GetEnableProxyAccountCount()
        {
            int iReturn = 0;

            try
            {
                foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                {
                    if (pai.IsEnable)
                    {
                        iReturn++;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return iReturn;
        }

        #endregion

        #region//获取过期的代理账号数

        public static int GetExpiryProxyAccountCount()
        {
            int iReturn = 0;

            try
            {
                foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                {
                    if (pai.IsEnable && pai.IsExpiry)
                    {
                        if (DateTime.Now >= pai.ExpiryTime)
                        {
                            iReturn++;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return iReturn;
        }

        #endregion

        #region//获取在线的代理账号数

        public static int GetOnLineProxyAccountCount()
        {
            int iReturn = 0;

            try
            {
                foreach (Proxy_AccountInfo pai in Socket_Cache.ProxyAccount.lstProxyAccount)
                {
                    if (pai.IsOnLine)
                    {
                        iReturn++;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return iReturn;
        }

        #endregion

        #region//查找树节点        

        public static TreeNode FindNodeSync(TreeNodeCollection nodes, string nodeName)
        {
            try
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    TreeNode node = nodes[i];
                    if (node.Text == nodeName)
                    {
                        return node;
                    }

                    TreeNode foundNode = FindNodeSync(node.Nodes, nodeName);
                    if (foundNode != null)
                    {
                        return foundNode;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return null;
        }

        #endregion

        #region//添加树节点

        public static TreeNode AddTreeNode(TreeView treeView, TreeNodeCollection Nodes, string NodeName, int ImgIndex, byte[] bData)
        {
            TreeNode tnReturn = null;

            try
            {
                if (!treeView.IsDisposed)
                {
                    if (treeView.InvokeRequired)
                    {
                        tnReturn = (TreeNode)treeView.Invoke(new Func<TreeNode>(() => AddNode(Nodes, NodeName, ImgIndex, bData)));
                    }
                    else
                    {
                        tnReturn = AddNode(Nodes, NodeName, ImgIndex, bData);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return tnReturn;
        }

        private static TreeNode AddNode(TreeNodeCollection nodes, string nodeName, int imgIndex, byte[] bData)
        {
            TreeNode tn = nodes.Add(nodeName);

            try
            {
                if (imgIndex > -1)
                {
                    tn.ImageIndex = imgIndex;
                    tn.SelectedImageIndex = imgIndex;
                }

                if (bData != null && bData.Length > 0)
                {
                    tn.Tag = bData;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            

            return tn;
        }

        #endregion

        #region//获取远端地址

        public static string GetServerAddress(string IP, ushort Port, Socket_ProxyTCP spc)
        {
            string sReturn = string.Empty;

            try
            {
                switch (spc.DomainType)
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

        public static string GetClientAddress(string IP, ushort Port, Socket_ProxyTCP spi)
        {
            string sReturn = string.Empty;

            try
            {
                if (spi.ClientSocket != null && spi.ClientSocket.RemoteEndPoint != null)
                {
                    int ClientPort = ((IPEndPoint)spi.ClientSocket.RemoteEndPoint).Port;
                    sReturn = IP + ": " + Port + " [" + ClientPort + "]";
                }                
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
                    using (RichTextBox rtbCompare = new RichTextBox())
                    {
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
                    }
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
                    box.SelectionFont = Socket_Cache.SocketPacket.FontUnderline;
                }

                if (color == col_Del)
                {
                    box.SelectionFont = Socket_Cache.SocketPacket.FontStrikeout;
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

        public static int SendTCPData(Socket socket, ReadOnlySpan<byte> bData)
        {
            int iReturn = 0;

            try
            {
                if (socket != null && !bData.IsEmpty)
                {
                    iReturn = socket.Send(bData.ToArray(), SocketFlags.None);
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

        public static int SendUDPData(UdpClient ClientUDP, ReadOnlySpan<byte> bData, IPEndPoint ep)
        {
            int iReturn = 0;

            try
            {
                if (ClientUDP != null && !bData.IsEmpty)
                {
                    iReturn = ClientUDP.Send(bData.ToArray(), bData.Length, ep);
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
            try
            {
                if (ClientUDP != null)
                {
                    return ClientUDP.EndReceive(ar, ref ep);                    
                }
            }
            catch
            {
                return Array.Empty<byte>();
            }

            return Array.Empty<byte>();
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
                    if (Socket_Cache.SocketPacket.CheckNotShow == bIsFilter)
                    {
                        return false;
                    }
                }

                //IP地址
                if (Socket_Cache.SocketPacket.CheckIP)
                {
                    bool bIsFilter_From = Socket_Operation.IsFilter_ByIP(spi.PacketFrom);
                    bool bIsFilter_To = Socket_Operation.IsFilter_ByIP(spi.PacketTo);
                    if (Socket_Cache.SocketPacket.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                    {
                        return false;
                    }     
                }

                //端口号
                if (Socket_Cache.SocketPacket.CheckPort)
                {
                    bool bIsFilter_From = Socket_Operation.IsFilter_ByPort(spi.PacketFrom);
                    bool bIsFilter_To = Socket_Operation.IsFilter_ByPort(spi.PacketTo);
                    if (Socket_Cache.SocketPacket.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                    {
                        return false;
                    }
                }

                //指定包头
                if (Socket_Cache.SocketPacket.CheckHead)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_ByHead(spi.PacketBuffer);
                    if (Socket_Cache.SocketPacket.CheckNotShow == bIsFilter)
                    {
                        return false;
                    }
                }

                //封包内容
                if (Socket_Cache.SocketPacket.CheckData)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_ByPacket(spi.PacketBuffer);
                    if (Socket_Cache.SocketPacket.CheckNotShow == bIsFilter)
                    {
                        return false;
                    }
                }

                //封包大小
                if (Socket_Cache.SocketPacket.CheckSize)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_BySize(spi.PacketLen);
                    if (Socket_Cache.SocketPacket.CheckNotShow == bIsFilter)
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

        private static bool IsFilter_BySocket(int iPacketSocket)
        {
            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckSocket_Value))
                {
                    string[] sSocketArr = Socket_Cache.SocketPacket.CheckSocket_Value.Split(';');
                    HashSet<int> socketSet = new HashSet<int>();

                    foreach (string sSocket in sSocketArr)
                    {
                        if (!string.IsNullOrEmpty(sSocket) && int.TryParse(sSocket, out int iCheckSocket))
                        {
                            socketSet.Add(iCheckSocket);
                        }
                    }

                    return socketSet.Contains(iPacketSocket);
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//检测IP地址

        private static bool IsFilter_ByIP(string sPacketIP)
        {
            try
            {
                if (string.IsNullOrEmpty(sPacketIP) || string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckIP_Value))
                {
                    return false;
                }

                string sIP = sPacketIP.Split(':')[0];
                HashSet<string> ipSet = new HashSet<string>(Socket_Cache.SocketPacket.CheckIP_Value.Split(';'));

                return ipSet.Contains(sIP);
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//检测端口号

        private static bool IsFilter_ByPort(string sPacketPort)
        {
            try
            {
                if (string.IsNullOrEmpty(sPacketPort) || string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckPort_Value))
                {
                    return false;
                }

                string sPort = sPacketPort.Split(':')[1];
                HashSet<string> portSet = new HashSet<string>(Socket_Cache.SocketPacket.CheckPort_Value.Split(';'));

                return portSet.Contains(sPort);
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//检测包头

        private static bool IsFilter_ByHead(byte[] bBuffer)
        {
            try
            {
                if (string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckHead_Value))
                {
                    return false;
                }

                string checkHeadValue = Socket_Cache.SocketPacket.CheckHead_Value.Replace(" ", "");
                string[] headValues = checkHeadValue.Split(';');

                foreach (string headValue in headValues)
                {
                    if (!string.IsNullOrEmpty(headValue))
                    {
                        byte[] headBytes = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, headValue);

                        if (bBuffer.Length >= headBytes.Length)
                        {
                            bool match = true;
                            for (int i = 0; i < headBytes.Length; i++)
                            {
                                if (bBuffer[i] != headBytes[i])
                                {
                                    match = false;
                                    break;
                                }
                            }

                            if (match)
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

            return false;
        }

        #endregion

        #region//检测封包内容

        private static bool IsFilter_ByPacket(byte[] bBuffer)
        {
            try
            {
                if (string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckData_Value))
                {
                    return false;
                }

                string checkDataValue = Socket_Cache.SocketPacket.CheckData_Value.Replace(" ", "");
                string[] checkDataArray = checkDataValue.Split(';');

                string packetString = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer).Replace(" ", "");

                foreach (string checkData in checkDataArray)
                {
                    if (!string.IsNullOrEmpty(checkData) && packetString.IndexOf(checkData) >= 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//检测封包大小

        private static bool IsFilter_BySize(int PacketLength)
        {
            try
            {
                if (string.IsNullOrEmpty(Socket_Cache.SocketPacket.CheckLength_Value))
                {
                    return false;
                }

                string[] lengthArray = Socket_Cache.SocketPacket.CheckLength_Value.Split(';');

                foreach (string length in lengthArray)
                {
                    if (string.IsNullOrEmpty(length))
                    {
                        continue;
                    }

                    if (length.Contains("-"))
                    {
                        string[] range = length.Split('-');
                        if (range.Length == 2 && int.TryParse(range[0], out int iFrom) && int.TryParse(range[1], out int iTo))
                        {
                            if (PacketLength >= iFrom && PacketLength <= iTo)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (int.TryParse(length, out int iLength))
                        {
                            if (PacketLength == iLength)
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

            return false;
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

        #region//显示账号管理窗体

        public static void ShowProxyAccountListForm()
        {
            try
            {
                if (!Socket_Cache.ProxyAccount.IsShow)
                {
                    Proxy_AccountListForm palForm = new Proxy_AccountListForm();
                    palForm.Show();
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static void ShowProxyAccountForm(Guid AID)
        {
            try
            {
                Proxy_AccountForm paForm = new Proxy_AccountForm(AID);
                paForm.ShowDialog();
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
                if (Socket_Cache.System.MainHandle != IntPtr.Zero)
                {
                    int KeyControl = (int)User32.KeyModifiers.MOD_CONTROL;
                    int KeyAlt = (int)User32.KeyModifiers.MOD_ALT;
                    int iKeyCode = (int)Keys.F1;

                    for (int i = 9001; i < 9013; i++)
                    {
                        bool bOK = User32.RegisterHotKey(Socket_Cache.System.MainHandle, i, KeyControl | KeyAlt, iKeyCode);

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
                if (Socket_Cache.System.MainHandle != IntPtr.Zero)
                {
                    for (int i = 9001; i < 9013; i++)
                    {
                        User32.UnregisterHotKey(Socket_Cache.System.MainHandle, i);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Socket_Cache.LogQueue.LogToQueue(Socket_Cache.System.LogType.Socket, sFuncName, sLogContent);
                }
            });                                
        }

        public static void DoLog_Proxy(string sFuncName, string sLogContent)
        {
            Task.Run(() =>
            {
                if (bDoLog)
                {
                    Socket_Cache.LogQueue.LogToQueue(Socket_Cache.System.LogType.Proxy, sFuncName, sLogContent);
                }
            });            
        }

        #endregion

        #region//发送封包

        public static unsafe bool SendPacket(int Socket, Socket_Cache.SocketPacket.PacketType packetType, string sIPFrom, string sIPTo, byte[] bSendBuffer)
        {
            bool bReturn = false;
            IntPtr ipSend = IntPtr.Zero;

            try
            {
                if (Socket > 0 && bSendBuffer.Length > 0)
                {
                    ipSend = Marshal.AllocHGlobal(bSendBuffer.Length);
                    Marshal.Copy(bSendBuffer, 0, ipSend, bSendBuffer.Length);

                    string sIPString = string.Empty;
                    switch (packetType)
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

                    int res = -1;
                    switch (packetType)
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
