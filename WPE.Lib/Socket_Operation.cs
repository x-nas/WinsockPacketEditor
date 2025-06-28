﻿using Microsoft.Owin.Hosting;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WPE.Lib
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
                            if (NativeMethods.Kernel32.IsWow64Process(pProcess.Handle, out retVal))
                            {
                                bReturn = !retVal;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
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
                if (Operate.SystemConfig.IsRemote)
                {
                    if (!string.IsNullOrEmpty(Operate.SystemConfig.Remote_URL) &&
                        !string.IsNullOrEmpty(Operate.SystemConfig.Remote_UserName) &&
                        !string.IsNullOrEmpty(Operate.SystemConfig.Remote_PassWord))
                    {
                        string sLog = string.Empty;

                        try
                        {
                            Operate.SystemConfig.WebServer = WebApp.Start<WebAPI.Socket_Web>(Operate.SystemConfig.Remote_URL);
                            Socket_Operation.InitCCProxy_HTML();

                            sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_178), Operate.SystemConfig.Remote_URL);
                        }
                        catch
                        {
                            sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_179), Process.GetCurrentProcess().ProcessName);
                        }

                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static void StopRemoteMGT(Operate.SystemConfig.SystemMode FromMode)
        {
            try
            {
                if (FromMode == Operate.SystemConfig.StartMode)
                {
                    if (Operate.SystemConfig.WebServer != null)
                    {
                        Operate.SystemConfig.WebServer.Dispose();
                    }
                }                         
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.SystemConfig.cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    Operate.SystemConfig.cpuCounter.NextValue();
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InitCPUAndMemoryCounter), ex.Message);
                }
            });            
        }

        public static string[] GetCPUAndMemory()
        {
            string[] sReturn = new string[2];

            try
            {
                if (Operate.SystemConfig.cpuCounter != null)
                {
                    // 获取CPU使用率
                    float cpuUsage = Operate.SystemConfig.cpuCounter.NextValue();
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
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
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return string.Empty;
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
                    string sProxyServer = string.Format("socks5://127.0.0.1:{0}", Operate.ProxyConfig.SocketProxy.ProxyPort);

                    key.SetValue("ProxyEnable", 1, RegistryValueKind.DWord);
                    key.SetValue("ProxyServer", sProxyServer, RegistryValueKind.String);
                    key.SetValue("ProxyOverride", string.Empty, RegistryValueKind.String);
                    key.Close();

                    bReturn = true;

                    Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_148));
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                    Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_149));
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//数据格式转换

        #region//返还 Byte[] 占用的内存

        public static void ReturnBuffer(byte[] buffer)
        {
            if (buffer != null)
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        #endregion

        #region//base64 编码，解码

        public static string Base64_Encoding(string sString)
        {
            string sReturn = string.Empty;

            try
            {
                byte[] bBuffer = Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sString);
                sReturn = Convert.ToBase64String(bBuffer);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

        public static byte[] StringToBytes(Operate.PacketConfig.Packet.EncodingFormat efFormat, string sString)
        {
            byte[] bReturn = new byte[sString.Length];

            try
            {
                switch (efFormat)
                {
                    case Operate.PacketConfig.Packet.EncodingFormat.Default:
                        bReturn = Encoding.Default.GetBytes(sString);
                        break;

                    case Operate.PacketConfig.Packet.EncodingFormat.Hex:
                        bReturn = Socket_Operation.Hex_To_Bytes(sString);
                        break;

                    case Operate.PacketConfig.Packet.EncodingFormat.GBK:
                        bReturn = Encoding.GetEncoding("GBK").GetBytes(sString);
                        break;

                    case Operate.PacketConfig.Packet.EncodingFormat.Unicode:
                        bReturn = Encoding.Unicode.GetBytes(sString);
                        break;

                    case Operate.PacketConfig.Packet.EncodingFormat.UTF7:
                        bReturn = Encoding.UTF7.GetBytes(sString);
                        break;

                    case Operate.PacketConfig.Packet.EncodingFormat.UTF8:
                        bReturn = Encoding.UTF8.GetBytes(sString);
                        break;

                    case Operate.PacketConfig.Packet.EncodingFormat.UTF16:
                        bReturn = Encoding.BigEndianUnicode.GetBytes(sString);
                        break;

                    case Operate.PacketConfig.Packet.EncodingFormat.UTF32:
                        bReturn = Encoding.UTF32.GetBytes(sString);
                        break;                
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//byte[]转字符串

        public static string BytesToString(Operate.PacketConfig.Packet.EncodingFormat efFormat, ReadOnlySpan<byte> buffer)
        {
            string sReturn = string.Empty;

            try
            {
                if (buffer.Length > 0)
                {
                    switch (efFormat)
                    {
                        case Operate.PacketConfig.Packet.EncodingFormat.Default:
                            sReturn = Encoding.Default.GetString(buffer.ToArray());
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Char:
                            char c = (char)buffer[0];
                            sReturn = (char.IsControl(c) ? "." : c.ToString());
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Byte:
                            sReturn = buffer[0].ToString();
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Bytes:
                            StringBuilder sbBytes = new StringBuilder();
                            foreach (byte b in buffer)
                            {
                                sbBytes.Append(b).Append(",");
                            }
                            sReturn = sbBytes.ToString().TrimEnd(',');
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Short:
                            if (buffer.Length >= 2)
                            {
                                sReturn = BitConverter.ToInt16(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.UShort:
                            if (buffer.Length >= 2)
                            {
                                sReturn = BitConverter.ToUInt16(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Int32:
                            if (buffer.Length >= 4)
                            {
                                sReturn = BitConverter.ToInt32(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.UInt32:
                            if (buffer.Length >= 4)
                            {
                                sReturn = BitConverter.ToUInt32(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Int64:
                            if (buffer.Length >= 8)
                            {
                                sReturn = BitConverter.ToInt64(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.UInt64:
                            if (buffer.Length >= 8)
                            {
                                sReturn = BitConverter.ToUInt64(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Float:
                            if (buffer.Length >= 4)
                            {
                                sReturn = BitConverter.ToSingle(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Double:
                            if (buffer.Length >= 8)
                            {
                                sReturn = BitConverter.ToDouble(buffer.ToArray(), 0).ToString();
                            }
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Bin:
                            StringBuilder sbBin = new StringBuilder();
                            foreach (byte b in buffer)
                            {
                                sbBin.Append(Convert.ToString(b, 2).PadLeft(8, '0')).Append(" ");
                            }
                            sReturn = sbBin.ToString().Trim();
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Hex:
                            StringBuilder sbHex = new StringBuilder();
                            foreach (byte b in buffer)
                            {
                                sbHex.Append(b.ToString("X2")).Append(" ");
                            }
                            sReturn = sbHex.ToString().Trim();
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.GBK:
                            sReturn = Encoding.GetEncoding("GBK").GetString(buffer.ToArray());
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.Unicode:
                            sReturn = Encoding.Unicode.GetString(buffer.ToArray());
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.ASCII:
                            sReturn = Encoding.ASCII.GetString(buffer.ToArray());
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.UTF7:
                            sReturn = Encoding.UTF7.GetString(buffer.ToArray());
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.UTF8:
                            sReturn = Encoding.UTF8.GetString(buffer.ToArray());
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.UTF16:
                            sReturn = Encoding.BigEndianUnicode.GetString(buffer.ToArray());
                            break;

                        case Operate.PacketConfig.Packet.EncodingFormat.UTF32:
                            sReturn = Encoding.UTF32.GetString(buffer.ToArray());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return uReturn;
        }

        #endregion

        #region//字符串 1 转 True

        public static bool StringToBool(string bString)
        {
            bool bReturn = false;

            try
            {
                if (bString.Equals("1"))
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return bReturn;
        }

        #endregion

        #region//字符串转DateTime

        public static DateTime StringToDateTime(string sDate, string sTime)
        {
            DateTime dtReturn = DateTime.MinValue;

            try
            {
                if (!string.IsNullOrEmpty(sDate) && !string.IsNullOrEmpty(sTime))
                {
                    string dateTimeStr = $"{sDate} {sTime}";

                    dtReturn = DateTime.ParseExact(dateTimeStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return dtReturn;
        }

        #endregion

        #endregion        

        #region//判断地址的类型

        private static bool IsValidIPv4(string IPString)
        {
            string pattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            return Regex.IsMatch(IPString, pattern);           
        }

        private static bool IsValidIPv6(string IPString)
        {
            string pattern = @"^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$";
            return Regex.IsMatch(IPString, pattern);
        }

        private static bool IsValidDomain(string IPString)
        {
            string pattern = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)+([A-Za-z]{2,}|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$";
            return Regex.IsMatch(IPString, pattern);
        }

        public static Operate.ProxyConfig.SocketProxy.AddressType GetAddressType_ByString(string IPString)
        {
            if (IsValidIPv4(IPString))
                return Operate.ProxyConfig.SocketProxy.AddressType.IPv4;

            if (IsValidIPv6(IPString))
                return Operate.ProxyConfig.SocketProxy.AddressType.IPv6;

            if (IsValidDomain(IPString))
                return Operate.ProxyConfig.SocketProxy.AddressType.Domain;

            return Operate.ProxyConfig.SocketProxy.AddressType.Invalid;
        }

        #endregion

        #region//解析Http头数据

        public static Dictionary<string, string> ParseHttpHeaders(string request)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                using (StringReader reader = new StringReader(request))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
                    {
                        if (line.Contains(":"))
                        {
                            var parts = line.Split(new[] { ':' }, 2);
                            if (parts.Length == 2)
                            {
                                headers[parts[0].Trim()] = parts[1].Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            

            return headers;
        }

        #endregion

        #region//发送404响应

        public static void Send404Response(Socket clientSocket)
        {
            try
            {
                string response =
                "HTTP/1.1 404 Not Found\r\n" +
                "Content-Type: text/html\r\n" +
                "Content-Length: 0\r\n" +
                "Connection: close\r\n\r\n";

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                Socket_Operation.SendTCPData(clientSocket, responseBytes);
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region// 获取 Content-Type 类型

        public static string GetContentType(string fileExtension)
        {
            try
            {
                switch (fileExtension.ToLower())
                {
                    case ".html":
                    case ".htm":
                        return "text/html";

                    case ".js":
                        return "application/javascript";

                    case ".css":
                        return "text/css";

                    case ".png":
                        return "image/png";

                    case ".jpg":
                    case ".jpeg":
                        return "image/jpeg";

                    case ".gif":
                        return "image/gif";

                    case ".svg":
                        return "image/svg+xml";

                    case ".json":
                        return "application/json";

                    default:
                        return "application/octet-stream";
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return "application/octet-stream";
        }

        #endregion

        #region//获取远程代理映射的数据

        public static byte[] GetRemoteMappedData(string remoteUrl, string originalRequest, Dictionary<string, string> headers)
        {
            try
            {
                // 解析原始请求
                string[] requestParts = originalRequest.Split(new[] { "\r\n" }, StringSplitOptions.None);
                string[] requestLine = requestParts[0].Split(' ');
                string method = requestLine[0];
                string path = requestLine.Length > 1 ? requestLine[1] : "/";

                // 构建新的请求URL
                UriBuilder remoteUri = new UriBuilder(remoteUrl);
                if (!string.IsNullOrEmpty(path) && path != "/")
                {
                    // 保留原始路径参数
                    string queryToAppend = remoteUri.Query;
                    if (!string.IsNullOrEmpty(remoteUri.Query))
                    {
                        queryToAppend = "&" + remoteUri.Query.TrimStart('?');
                    }

                    // 处理路径拼接
                    string originalPath = path.Split('?')[0];
                    string originalQuery = path.Contains('?') ? path.Substring(path.IndexOf('?')) : "";

                    remoteUri.Path = remoteUri.Path.TrimEnd('/') + "/" + originalPath.TrimStart('/');
                    remoteUri.Query = originalQuery.TrimStart('?') + queryToAppend;
                }

                // 创建HTTP请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(remoteUri.Uri);
                request.Method = method;

                // 设置超时时间
                request.Timeout = 10000; // 10秒超时
                request.ReadWriteTimeout = 10000;

                // 复制原始请求头（排除不应转发的头）
                foreach (var header in headers)
                {
                    string headerKey = header.Key.ToLower();

                    // 跳过这些不应该转发的头
                    if (headerKey == "connection" ||
                        headerKey == "keep-alive" ||
                        headerKey == "proxy-connection" ||
                        headerKey == "te" ||
                        headerKey == "trailer" ||
                        headerKey == "transfer-encoding" ||
                        headerKey == "upgrade")
                    {
                        continue;
                    }

                    switch (headerKey)
                    {
                        case "host":
                            request.Host = remoteUri.Host;
                            break;
                        case "accept":
                            request.Accept = header.Value;
                            break;
                        case "user-agent":
                            request.UserAgent = header.Value;
                            break;
                        case "content-type":
                            request.ContentType = header.Value;
                            break;
                        case "content-length":
                            // 将在处理请求体时设置
                            break;
                        case "referer":
                            // 更新Referer为新的远程地址
                            if (Uri.TryCreate(header.Value, UriKind.Absolute, out Uri originalReferer))
                            {
                                string newReferer = remoteUri.Scheme + "://" + remoteUri.Host + originalReferer.PathAndQuery;
                                request.Referer = newReferer;
                            }
                            else
                            {
                                request.Referer = header.Value;
                            }
                            break;
                        default:
                            request.Headers[header.Key] = header.Value;
                            break;
                    }
                }

                // 处理请求体（POST/PUT等）
                if ((method == "POST" || method == "PUT" || method == "PATCH") &&
                    headers.TryGetValue("content-length", out string contentLengthStr) &&
                    int.TryParse(contentLengthStr, out int contentLength) &&
                    contentLength > 0)
                {
                    // 从原始请求中提取请求体
                    int bodyStartIndex = originalRequest.IndexOf("\r\n\r\n") + 4;
                    if (bodyStartIndex >= 4 && bodyStartIndex < originalRequest.Length)
                    {
                        string requestBody = originalRequest.Substring(bodyStartIndex);

                        using (Stream requestStream = request.GetRequestStream())
                        using (StreamWriter writer = new StreamWriter(requestStream))
                        {
                            writer.Write(requestBody);
                        }
                    }
                }

                // 获取响应
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    responseStream.CopyTo(memoryStream);

                    // 构建响应头
                    StringBuilder responseHeaders = new StringBuilder();
                    responseHeaders.Append($"HTTP/1.1 {(int)response.StatusCode} {response.StatusDescription}\r\n");

                    // 复制响应头（排除不应转发的头）
                    foreach (string headerName in response.Headers.AllKeys)
                    {
                        string lowerHeaderName = headerName.ToLower();

                        if (lowerHeaderName == "transfer-encoding" ||
                            lowerHeaderName == "connection" ||
                            lowerHeaderName == "keep-alive")
                        {
                            continue;
                        }

                        responseHeaders.Append($"{headerName}: {response.Headers[headerName]}\r\n");
                    }

                    responseHeaders.Append("\r\n");

                    // 合并响应头和响应体
                    byte[] headerBytes = Encoding.UTF8.GetBytes(responseHeaders.ToString());
                    byte[] responseBytes = memoryStream.ToArray();

                    byte[] fullResponse = new byte[headerBytes.Length + responseBytes.Length];
                    Buffer.BlockCopy(headerBytes, 0, fullResponse, 0, headerBytes.Length);
                    Buffer.BlockCopy(responseBytes, 0, fullResponse, headerBytes.Length, responseBytes.Length);

                    return fullResponse;
                }
            }
            catch (WebException webEx) when (webEx.Response is HttpWebResponse errorResponse)
            {
                // 处理远程服务器返回的错误响应
                using (Stream errorStream = errorResponse.GetResponseStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    errorStream?.CopyTo(memoryStream);

                    StringBuilder responseHeaders = new StringBuilder();
                    responseHeaders.Append($"HTTP/1.1 {(int)errorResponse.StatusCode} {errorResponse.StatusDescription}\r\n");

                    foreach (string headerName in errorResponse.Headers.AllKeys)
                    {
                        responseHeaders.Append($"{headerName}: {errorResponse.Headers[headerName]}\r\n");
                    }

                    responseHeaders.Append("\r\n");

                    byte[] headerBytes = Encoding.UTF8.GetBytes(responseHeaders.ToString());
                    byte[] responseBytes = memoryStream.ToArray();

                    byte[] fullResponse = new byte[headerBytes.Length + responseBytes.Length];
                    Buffer.BlockCopy(headerBytes, 0, fullResponse, 0, headerBytes.Length);
                    Buffer.BlockCopy(responseBytes, 0, fullResponse, headerBytes.Length, responseBytes.Length);

                    return fullResponse;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, $"远程映射失败: {ex.Message}");

                // 返回500错误响应
                string errorResponse = "HTTP/1.1 500 Internal Server Error\r\n" +
                                      "Content-Type: text/plain\r\n" +
                                      "Connection: close\r\n" +
                                      "\r\n" +
                                      "Remote mapping failed: " + ex.Message;

                return Encoding.UTF8.GetBytes(errorResponse);
            }
        }

        #endregion

        #region//统计封包数量

        public static void CountSocketInfo(Operate.PacketConfig.Packet.PacketType ptPacketType, int packetLength)
        {
            try
            {
                if (packetLength > 0)
                {
                    Interlocked.Increment(ref Operate.PacketConfig.Packet.TotalPackets);

                    switch (ptPacketType)
                    {
                        case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                        case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                            Interlocked.Increment(ref Operate.PacketConfig.Queue.Send_CNT);
                            Interlocked.Add(ref Operate.PacketConfig.Packet.Total_SendBytes, packetLength);
                            break;

                        case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                        case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                            Interlocked.Increment(ref Operate.PacketConfig.Queue.SendTo_CNT);
                            Interlocked.Add(ref Operate.PacketConfig.Packet.Total_SendBytes, packetLength);
                            break;

                        case Operate.PacketConfig.Packet.PacketType.WSASend:
                            Interlocked.Increment(ref Operate.PacketConfig.Queue.WSASend_CNT);
                            Interlocked.Add(ref Operate.PacketConfig.Packet.Total_SendBytes, packetLength);
                            break;

                        case Operate.PacketConfig.Packet.PacketType.WSASendTo:
                            Interlocked.Increment(ref Operate.PacketConfig.Queue.WSASendTo_CNT);
                            Interlocked.Add(ref Operate.PacketConfig.Packet.Total_SendBytes, packetLength);
                            break;

                        case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                        case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                            Interlocked.Increment(ref Operate.PacketConfig.Queue.Recv_CNT);
                            Interlocked.Add(ref Operate.PacketConfig.Packet.Total_RecvBytes, packetLength);
                            break;

                        case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                        case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                            Interlocked.Increment(ref Operate.PacketConfig.Queue.RecvFrom_CNT);
                            Interlocked.Add(ref Operate.PacketConfig.Packet.Total_RecvBytes, packetLength);
                            break;

                        case Operate.PacketConfig.Packet.PacketType.WSARecv:
                        case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                            Interlocked.Increment(ref Operate.PacketConfig.Queue.WSARecv_CNT);
                            Interlocked.Add(ref Operate.PacketConfig.Packet.Total_RecvBytes, packetLength);
                            break;

                        case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:
                            Interlocked.Increment(ref Operate.PacketConfig.Queue.WSARecvFrom_CNT);
                            Interlocked.Add(ref Operate.PacketConfig.Packet.Total_RecvBytes, packetLength);
                            break;
                    }
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//统计代理速率

        public static void CountProxySpeed(Operate.ProxyConfig.SocketProxy.ProxySpeedType psType, int ProxySpeed)
        {
            try
            {
                if (ProxySpeed > 0)
                {
                    switch (psType)
                    {
                        case Operate.ProxyConfig.SocketProxy.ProxySpeedType.Uplink:
                            Interlocked.Add(ref Operate.ProxyConfig.SocketProxy.ProxySpeed_Uplink, ProxySpeed);
                            break;

                        case Operate.ProxyConfig.SocketProxy.ProxySpeedType.Downlink:
                            Interlocked.Add(ref Operate.ProxyConfig.SocketProxy.ProxySpeed_Downlink, ProxySpeed);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//判断接收的数据是否匹配代理步骤

        public static bool CheckDataIsMatchProxyStep(ReadOnlySpan<byte> bData, Operate.ProxyConfig.SocketProxy.ProxyStep proxyStep)
        {
            bool bReturn = false;

            try
            {
                byte VERSION = bData[0];

                switch (proxyStep)
                {
                    case Operate.ProxyConfig.SocketProxy.ProxyStep.Handshake:

                        if (VERSION == ((byte)Operate.ProxyConfig.SocketProxy.ProxyType.Socket5))
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

                    case Operate.ProxyConfig.SocketProxy.ProxyStep.AuthUserName:

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

                    case Operate.ProxyConfig.SocketProxy.ProxyStep.Command:

                        if (VERSION == ((byte)Operate.ProxyConfig.SocketProxy.ProxyType.Socket5))
                        {
                            if (bData.Length > 4)
                            {
                                byte ADDRESS_TYPE = bData[3];
                                Operate.ProxyConfig.SocketProxy.AddressType AddressType = (Operate.ProxyConfig.SocketProxy.AddressType)ADDRESS_TYPE;

                                int DST_ADDR = 0;
                                switch (AddressType)
                                {
                                    case Operate.ProxyConfig.SocketProxy.AddressType.IPv4:
                                        DST_ADDR = 4;
                                        break;

                                    case Operate.ProxyConfig.SocketProxy.AddressType.IPv6:
                                        DST_ADDR = 16;
                                        break;

                                    case Operate.ProxyConfig.SocketProxy.AddressType.Domain:
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

                    case Operate.ProxyConfig.SocketProxy.ProxyStep.ForwardData:

                        bReturn = true;

                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                foreach (Socket_SendInfo ssi in Operate.SendConfig.SendList.lstSend)
                {
                    cbb.Items.Add(new Operate.SendConfig.SendList.SendListItem { SName = ssi.SName, SID = ssi.SID });
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public static void InitSendListComboBox(ToolStripComboBox tscb)
        {
            try
            {
                tscb.Items.Clear();

                foreach (Socket_SendInfo ssi in Operate.SendConfig.SendList.lstSend)
                {
                    tscb.Items.Add(new Operate.SendConfig.SendList.SendListItem { SName = ssi.SName, SID = ssi.SID });
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//初始化CCProxy模板

        public static void InitCCProxy_HTML()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web", "CCProxy", "cn_acclistadmin.htm");

            if (File.Exists(filePath))
            {
                Operate.ProxyConfig.ProxyAccount.CCProxy_HTML = File.ReadAllText(filePath, Encoding.UTF8);
            }
        }

        #endregion

        #region//获取系统运行模式名称

        public static string GetSystemModeName()
        {
            string sReturn = string.Empty;
            
            try
            {
                switch (Operate.SystemConfig.StartMode)
                {
                    case Operate.SystemConfig.SystemMode.Proxy:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_185);
                        break;

                    case Operate.SystemConfig.SystemMode.Process:
                        sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_186);
                        break;                    
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.PacketConfig.Packet.InjectProcess = string.Format("{0} [{1}]", pProcess.ProcessName, pProcess.Id);

                sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_20) + Operate.PacketConfig.Packet.InjectProcess;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取进程的信息

        

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.PacketConfig.Packet.Support_WS1 = false;
                Operate.PacketConfig.Packet.Support_WS2 = false;

                foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                {
                    string sModuleName = module.ModuleName;

                    if (sModuleName.Equals(NativeMethods.WSock32.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Operate.PacketConfig.Packet.Support_WS1 = true;
                    }

                    if (sModuleName.Equals(NativeMethods.WS2_32.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Operate.PacketConfig.Packet.Support_WS2 = true;
                    }

                    if (sModuleName.Equals(NativeMethods.Mswsock.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Operate.PacketConfig.Packet.Support_MsWS = true;
                    }
                }

                if (Operate.PacketConfig.Packet.Support_WS1)
                {
                    sReturn += " 1.1";
                }

                if (Operate.PacketConfig.Packet.Support_WS2)
                {
                    sReturn += " 2.0";
                }

                if (Operate.PacketConfig.Packet.Support_MsWS)
                {
                    sReturn += " Microsoft";
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion                

        #region//处理 Hook 结果（异步）

        public static Task ProcessingHookResultAsync(
            int socket,
            byte[] bRawBuffer,
            byte[] bBuffer,
            int res,
            Operate.PacketConfig.Packet.PacketType ptType,
            Operate.FilterConfig.Filter.FilterAction filterAction,
            Operate.PacketConfig.Packet.SockAddr sockaddr,
            DateTime packetTime)
        {
            if (filterAction == Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay)
                return Task.CompletedTask;

            if (filterAction != Operate.FilterConfig.Filter.FilterAction.Intercept && res <= 0)
                return Task.CompletedTask;

            return Task.Run(() =>
            {
                try
                {                   
                    Operate.PacketConfig.Queue.PacketToQueue(socket, bRawBuffer, bBuffer, ptType, sockaddr, filterAction, packetTime);
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(ProcessingHookResultAsync), ex.Message);                    
                }
            });
        }

        #endregion

        #region//获取 SockAddr 对应的 IP 地址和端口

        public static string GetIPString_BySocketAddr(int pSocket, Operate.PacketConfig.Packet.SockAddr pAddr, Operate.PacketConfig.Packet.PacketType pType)
        {
            string sIP_From = string.Empty;
            string sIP_To = string.Empty;

            try
            {
                sIP_From = Socket_Operation.GetIP_BySocket(pSocket, Operate.PacketConfig.Packet.IPType.From);

                switch (pType)
                {
                    case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                    case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                    case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                    case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                    case Operate.PacketConfig.Packet.PacketType.WSASend:
                    case Operate.PacketConfig.Packet.PacketType.WSARecv:
                    case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                        sIP_To = Socket_Operation.GetIP_BySocket(pSocket, Operate.PacketConfig.Packet.IPType.To);
                        break;

                    case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                    case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                    case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                    case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                    case Operate.PacketConfig.Packet.PacketType.WSASendTo:
                    case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return string.Empty;
        }

        public static string GetIP_BySockAddr(Operate.PacketConfig.Packet.SockAddr saAddr)
        {
            string sReturn = string.Empty;

            try
            {
                if (saAddr.sin_family == (short)AddressFamily.InterNetwork)
                {
                    string sIP = Marshal.PtrToStringAnsi(NativeMethods.WS2_32.inet_ntoa(saAddr.sin_addr));
                    string sPort = NativeMethods.WS2_32.ntohs(saAddr.sin_port).ToString();
                    sReturn = $"{sIP}:{sPort}";
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        public static string GetIP_BySocket(int Socket, Operate.PacketConfig.Packet.IPType IPType)
        {
            string sReturn = "";

            try
            {
                Operate.PacketConfig.Packet.SockAddr saAddr = new Operate.PacketConfig.Packet.SockAddr();
                saAddr.sin_family = (short)AddressFamily.InterNetwork;
                int iAddrLen = Marshal.SizeOf(saAddr);

                switch (IPType)
                {
                    case Operate.PacketConfig.Packet.IPType.From:
                        NativeMethods.WS2_32.getsockname(Socket, ref saAddr, ref iAddrLen);
                        break;

                    case Operate.PacketConfig.Packet.IPType.To:
                        NativeMethods.WS2_32.getpeername(Socket, ref saAddr, ref iAddrLen);
                        break;                    
                }

                sReturn = GetIP_BySockAddr(saAddr);                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取 IP 地址和端口对应的 SockAddr

        public static Operate.PacketConfig.Packet.SockAddr GetSocketAddr_ByIPString(string IPString)
        {
            Operate.PacketConfig.Packet.SockAddr saReturn = new Operate.PacketConfig.Packet.SockAddr();

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return saReturn;
        }

        #endregion

        #region//检测外部代理服务器

        public static async Task<bool> DetectionExternalProxy()
        {
            try
            {
                IPEndPoint ExternalProxyEP = Socket_Operation.GetIPEndPoint_ByAddressString(Operate.ProxyConfig.SocketProxy.ExternalProxy_IP, Operate.ProxyConfig.SocketProxy.ExternalProxy_Port);
                if (ExternalProxyEP == null)
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_202));
                    return false;
                }

                using (Socket proxySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    // 设置连接超时
                    var connectTask = proxySocket.ConnectAsync(ExternalProxyEP);
                    var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));

                    if (await Task.WhenAny(connectTask, timeoutTask) == timeoutTask)
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_206));
                        return false;
                    }

                    //SOCKS5 握手
                    byte[] handshakeRequest = null;
                    if (Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy_Auth)
                    {
                        handshakeRequest = new byte[] { 0x05, 0x02, 0x00, 0x02 };
                    }
                    else
                    {
                        handshakeRequest = new byte[] { 0x05, 0x01, 0x00 };
                    }
                    await proxySocket.SendAsync(new ArraySegment<byte>(handshakeRequest), SocketFlags.None);

                    byte[] handshakeResponse = new byte[2];
                    int received = await proxySocket.ReceiveAsync(new ArraySegment<byte>(handshakeResponse), SocketFlags.None);

                    if (handshakeResponse[0] != 0x05)
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_207));
                        return false;
                    }

                    switch (handshakeResponse[1])
                    {
                        case 0x00:
                            // 无需认证
                            break;

                        case 0x02:
                            // 需要用户名/密码认证
                            if (!Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy_Auth)
                            {
                                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_208));
                                return false;
                            }

                            byte[] AuthRequest = Socket_Operation.CreateSOCKS5AuthPacket(Operate.ProxyConfig.SocketProxy.ExternalProxy_UserName, Operate.ProxyConfig.SocketProxy.ExternalProxy_PassWord);
                            if (AuthRequest == null)
                            {
                                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_209));
                                return false;
                            }
                            await proxySocket.SendAsync(new ArraySegment<byte>(AuthRequest), SocketFlags.None);

                            byte[] AuthResponse = new byte[2];
                            await proxySocket.ReceiveAsync(new ArraySegment<byte>(AuthResponse), SocketFlags.None);
                            if (AuthResponse[1] != 0x00)
                            {
                                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_209));
                                return false;
                            }
                            break;

                        default:
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_210));
                            return false;
                    }
                    
                    return true;
                }
            }
            catch
            {
                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_212));
                return false;
            }
        }

        #endregion

        #region//判断 Socket 错误码是否是预期的错误

        public static bool IsExpectedSocketError(int errorCode)
        {
            // 10053: 软件导致连接中止
            // 10054: 远程主机强迫关闭了一个现有的连接
            return errorCode == 10053 || errorCode == 10054;
        }

        #endregion

        #region//获取IP地址信息

        

        public static async Task<string> GetIPLocation(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return string.Empty;

            if (!IPAddress.TryParse(ipAddress, out _))
                return string.Empty;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
                    string url = $"https://ip-api.com/json/{ipAddress}?lang=zh-CN";

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();

                    var data = JObject.Parse(json);
                    if (data["status"]?.ToString() == "success")
                    {
                        return $"{data["country"]} {data["regionName"]} {data["city"]} {data["isp"]}";
                    }
                    else
                    {
                        string message = data["message"]?.ToString();
                        switch (message)
                        {
                            case "invalid query":
                            case "private range":
                            case "reserved range":
                                return string.Empty;
                            default:
                                return string.Empty;
                        }
                    }
                }
            }
            catch (HttpRequestException)
            {
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static IPEndPoint GetIPEndPoint_ByAddressString(string AddressString, ushort Port)
        {
            try
            {
                IPAddress ipAddress = Socket_Operation.ResolveAddress(AddressString);                
                return new IPEndPoint(ipAddress, Port);
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);                
            }

            return null;
        }

        public static IPEndPoint GetIPEndPoint_ByAddressType(Operate.ProxyConfig.SocketProxy.AddressType addressType, ReadOnlySpan<byte> bData, out string AddressString)
        {
            AddressString = string.Empty;

            try
            {
                IPAddress ip = IPAddress.Any;
                ushort port = 0;
                int portPosition = 0;

                switch (addressType)
                {
                    case Operate.ProxyConfig.SocketProxy.AddressType.IPv4:
                        ip = new IPAddress(bData.Slice(0, 4).ToArray());
                        portPosition = 4;
                        AddressString = ip.ToString();
                        break;

                    case Operate.ProxyConfig.SocketProxy.AddressType.IPv6:
                        ip = new IPAddress(bData.Slice(0, 16).ToArray());
                        portPosition = 16;
                        AddressString = ip.ToString();
                        break;

                    case Operate.ProxyConfig.SocketProxy.AddressType.Domain:
                        byte length = bData[0];
                        var domainBytes = bData.Slice(1, length);
                        AddressString = Socket_Operation.BytesToString(
                            Operate.PacketConfig.Packet.EncodingFormat.UTF8,
                            domainBytes.ToArray());
                        ip = Socket_Operation.ResolveAddress(AddressString);
                        portPosition = 1 + length;
                        break;
                }

                if (ip != null)
                {
                    port = Socket_Operation.ByteArrayToInt16BigEndian(bData.Slice(portPosition, 2).ToArray());
                    return new IPEndPoint(ip, port);
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);                
            }

            return null;
        }

        private static IPAddress ResolveAddress(string addressString)
        {
            return ResolveAddressAsync(addressString).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task<IPAddress> ResolveAddressAsync(string addressString)
        {
            try
            {
                var addressType = Socket_Operation.GetAddressType_ByString(addressString);

                switch (addressType)
                {
                    case Operate.ProxyConfig.SocketProxy.AddressType.IPv4:
                    case Operate.ProxyConfig.SocketProxy.AddressType.IPv6:
                        return IPAddress.Parse(addressString);

                    case Operate.ProxyConfig.SocketProxy.AddressType.Domain:

                        if (Operate.ProxyConfig.SocketProxy.DnsCache.TryGetValue(addressString, out var cachedIp))
                        {
                            return cachedIp;
                        }

                        try
                        {
                            var entry = await Dns.GetHostEntryAsync(addressString).ConfigureAwait(false);
                            var ipv4 = entry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                            var result = ipv4 ?? entry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6)
                                         ?? entry.AddressList.First();

                            Operate.ProxyConfig.SocketProxy.DnsCache.AddOrUpdate(
                                key: addressString,
                                addValue: result,
                                updateValueFactory: (key, oldValue) => result);

                            return result;
                        }
                        catch
                        {
                            return null;
                        }                    
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(nameof(ResolveAddressAsync), ex.Message);
            }

            return null;
        }

        #endregion

        #region//获取UDP数据包

        public static ReadOnlySpan<byte> GetUDPData_ByAddressType(Operate.ProxyConfig.SocketProxy.AddressType addressType, ReadOnlySpan<byte> bData)
        {
            try
            {
                switch (addressType)
                {
                    case Operate.ProxyConfig.SocketProxy.AddressType.IPv4:
                        return bData.Length >= 10 ? bData.Slice(10) : ReadOnlySpan<byte>.Empty;

                    case Operate.ProxyConfig.SocketProxy.AddressType.Domain:

                        if (bData.Length < 5)
                        {
                            return ReadOnlySpan<byte>.Empty;
                        }
                        
                        byte domainLength = bData[4];
                        int domainStart = 5 + domainLength + 2;
                        return bData.Length >= domainStart ? bData.Slice(domainStart) : ReadOnlySpan<byte>.Empty;

                    case Operate.ProxyConfig.SocketProxy.AddressType.IPv6:
                        return bData.Length >= 22 ? bData.Slice(22) : ReadOnlySpan<byte>.Empty;

                    default:
                        return ReadOnlySpan<byte>.Empty;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                return ReadOnlySpan<byte>.Empty;
            }
        }

        #endregion

        #region//获取返回给客户端的数据（SOCKS5，IPV4）

        public static byte[] GetProxyReturnData(Operate.ProxyConfig.SocketProxy.CommandResponse CommandResponse, ReadOnlySpan<byte> bServerIP, ReadOnlySpan<byte> bServerPort)
        {
            try
            {
                Span<byte> response = stackalloc byte[10];
                response[0] = (byte)Operate.ProxyConfig.SocketProxy.ProxyType.Socket5;
                response[1] = (byte)CommandResponse;
                response[2] = 0x00;
                response[3] = (byte)Operate.ProxyConfig.SocketProxy.AddressType.IPv4;
                bServerIP.CopyTo(response.Slice(4, 4));
                response[8] = bServerPort[1];
                response[9] = bServerPort[0];

                return response.ToArray();
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    sReturn = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, bBuffSlice) + " ...";
                }
                else
                {
                    sReturn = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, bBuff);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取 SOCKS5 认证格式的封包

        public static byte[] CreateSOCKS5AuthPacket(string username, string password)
        {
            // 验证输入参数
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || username.Length > 255 || password.Length > 255)
            {
                return null;
            }

            // 计算所需缓冲区大小
            // 1 (VER) + 1 (ULEN) + username + 1 (PLEN) + password
            int packetSize = 1 + 1 + username.Length + 1 + password.Length;

            // 创建字节数组
            byte[] packet = new byte[packetSize];
            int offset = 0;

            // 版本号 (0x01)
            packet[offset++] = 0x01;

            // 用户名长度 (1字节)
            packet[offset++] = (byte)username.Length;

            // 用户名 (UTF8编码)
            byte[] usernameBytes = Encoding.UTF8.GetBytes(username);
            Buffer.BlockCopy(usernameBytes, 0, packet, offset, usernameBytes.Length);
            offset += usernameBytes.Length;

            // 密码长度 (1字节)
            packet[offset++] = (byte)password.Length;

            // 密码 (UTF8编码)
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            Buffer.BlockCopy(passwordBytes, 0, packet, offset, passwordBytes.Length);

            return packet;
        }

        #endregion

        #region//获取指定步长的 Byte

        public static byte GetStepByte(byte bStepByte, int iStepLen, out int iCarryCount)
        {
            int iStepValue = bStepByte + iStepLen;
            iCarryCount = iStepValue / 256;          
            iStepValue = (iStepValue % 256 + 256) % 256;

            return (byte)iStepValue;
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion        

        #region//获取端口对应的域名类型

        public static Operate.ProxyConfig.SocketProxy.DomainType GetDomainType_ByPort(ushort Port)
        {
            try
            {
                if (Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy)
                {
                    if (Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy_AppointPort && !string.IsNullOrEmpty(Operate.ProxyConfig.SocketProxy.ExternalProxy_AppointPort))
                    {
                        HashSet<string> ExternalProxyPorts = new HashSet<string>(Operate.ProxyConfig.SocketProxy.ExternalProxy_AppointPort.Split(','));

                        if (ExternalProxyPorts.Contains(Port.ToString()))
                        {
                            return Operate.ProxyConfig.SocketProxy.DomainType.External;
                        }
                    }
                    else
                    {
                        return Operate.ProxyConfig.SocketProxy.DomainType.External;
                    }                    
                }

                if (Port == 80 || Port == 8080)
                {
                    return Operate.ProxyConfig.SocketProxy.DomainType.Http;
                }
                else if (Port == 443 || Port == 8443)
                {
                    return Operate.ProxyConfig.SocketProxy.DomainType.Https;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return Operate.ProxyConfig.SocketProxy.DomainType.Socket;
        }

        #endregion

        #region//获取启用的代理账号数

        public static int GetEnableProxyAccountCount(BindingList<Proxy_AccountInfo> allData)
        {
            int iReturn = 0;

            try
            {
                foreach (Proxy_AccountInfo pai in allData)
                {
                    if (pai.IsEnable)
                    {
                        iReturn++;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return iReturn;
        }

        #endregion

        #region//获取过期的代理账号数

        public static int GetExpiryProxyAccountCount(BindingList<Proxy_AccountInfo> allData)
        {
            int iReturn = 0;

            try
            {
                foreach (Proxy_AccountInfo pai in allData)
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
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return iReturn;
        }

        #endregion

        #region//获取在线的代理账号数

        public static int GetOnLineProxyAccountCount(BindingList<Proxy_AccountInfo> allData)
        {
            int iReturn = 0;

            try
            {
                foreach (Proxy_AccountInfo pai in allData)
                {
                    if (pai.IsOnLine)
                    {
                        iReturn++;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            

            return tn;
        }

        #endregion

        #region//获取服务端地址

        public static string GetServerAddress(Operate.ProxyConfig.SocketProxy.DomainType dtType, string AddressString, ushort port)
        {
            if (string.IsNullOrEmpty(AddressString))
            {
                return string.Empty;
            }  

            try
            {
                string protocol = string.Empty;

                switch (dtType)
                {
                    case Operate.ProxyConfig.SocketProxy.DomainType.Socket:
                        protocol = "socket://";
                        break;
                    case Operate.ProxyConfig.SocketProxy.DomainType.Http:
                        protocol = "http://";
                        break;
                    case Operate.ProxyConfig.SocketProxy.DomainType.Https:
                        protocol = "https://";
                        break;
                    case Operate.ProxyConfig.SocketProxy.DomainType.External:
                        protocol = "SOCKS5://";
                        break;
                }

                return string.Format("{0}{1}: {2}", protocol, AddressString, port);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return string.Empty;
            }
        }

        #endregion

        #region//获取客户端地址

        public static string GetClientAddress(Socket clientSocket, string AddressString, ushort port)
        {
            if (string.IsNullOrEmpty(AddressString))
            {
                return string.Empty;
            }                

            try
            {
                if (clientSocket?.RemoteEndPoint is IPEndPoint remoteEndPoint)
                {
                    return $"{AddressString}: {port} [{remoteEndPoint.Port}]";
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                
            }

            return string.Empty;
        }

        #endregion

        #region//获取客户端列表名称

        public static string GetClientListName(string ClientIP, string ClientUserName)
        {
            return string.Format("{0} [{1}]", ClientIP, ClientUserName);
        }

        #endregion

        #region//获取列表中的选中的项

        //封包列表
        public static List<PacketInfo> GetSelectedSocket(DataGridView dgvSocketList)
        {
            List<PacketInfo> spiList = new List<PacketInfo>();

            try
            {
                for (int i = 0; i < dgvSocketList.Rows.Count; i++)
                {
                    if (dgvSocketList.Rows[i].Selected)
                    {
                        spiList.Add(Operate.PacketConfig.List.lstRecPacket[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return spiList;
        }

        //滤镜列表
        public static List<Socket_FilterInfo> GetSelectedFilter(DataGridView dgvFilterList)
        {
            List<Socket_FilterInfo> sfiList = new List<Socket_FilterInfo>();

            try
            {
                for (int i = 0; i < dgvFilterList.Rows.Count; i++)                
                {
                    if (dgvFilterList.Rows[i].Selected)
                    {
                        sfiList.Add(Operate.FilterConfig.List.lstFilter[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                
            }

            return sfiList;
        }

        //发送列表
        public static List<Socket_SendInfo> GetSelectedSend(DataGridView dgvSendList)
        {
            List<Socket_SendInfo> ssiList = new List<Socket_SendInfo>();

            try
            {
                for (int i = 0; i < dgvSendList.Rows.Count; i++)
                {
                    if (dgvSendList.Rows[i].Selected)
                    {
                        ssiList.Add(Operate.SendConfig.SendList.lstSend[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return ssiList;
        }

        //发送集列表
        public static List<PacketInfo> GetSelectedSendCollection(DataGridView dgvSendCollection, BindingList<PacketInfo> SCollection)
        {
            List<PacketInfo> sscList = new List<PacketInfo>();

            try
            {
                for (int i = 0; i < dgvSendCollection.Rows.Count; i++)
                {
                    if (dgvSendCollection.Rows[i].Selected)
                    {
                        sscList.Add(SCollection[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sscList;
        }

        //机器人列表
        public static List<Socket_RobotInfo> GetSelectedRobot(DataGridView dgvRobotList)
        {
            List<Socket_RobotInfo> sriList = new List<Socket_RobotInfo>();

            try
            {
                for (int i = 0; i < dgvRobotList.Rows.Count; i++)
                {
                    if (dgvRobotList.Rows[i].Selected)
                    {
                        sriList.Add(Operate.RobotConfig.RobotList.lstRobot[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sriList;
        }

        //代理账号列表
        public static List<Guid> GetSelectedProxyAccount(DataGridView dgvProxyAccount)
        {
            List<Guid> gReturnList = new List<Guid>();

            try
            {
                DataGridViewSelectedRowCollection selectedRows = dgvProxyAccount.SelectedRows;

                foreach (DataGridViewRow row in selectedRows)
                {
                    gReturnList.Add(Guid.Parse(row.Cells["cAID"].Value.ToString()));
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return gReturnList;
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
                Operate.DoLog(nameof(CompareData), ex.Message);
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
                    box.SelectionFont = Operate.PacketConfig.Packet.FontUnderline;
                }

                if (color == col_Del)
                {
                    box.SelectionFont = Operate.PacketConfig.Packet.FontStrikeout;
                }

                box.SelectionColor = color;
                box.AppendText(text);

                box.SelectionFont = box.Font;
                box.SelectionColor = box.ForeColor;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//对字典进行排序

        public static Dictionary<int, int> SortDictionaryByKey(Dictionary<int, int> dictionary, bool ascending = true)
        {
            Dictionary<int, int> dReturn = new Dictionary<int, int>();

            try
            {
                dReturn = ascending
                ? dictionary.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value)
                : dictionary.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            

            return dReturn;
        }

        public static Dictionary<int, int> SortDictionaryByValue(Dictionary<int, int> dictionary, bool ascending = true)
        {
            Dictionary<int, int> dReturn = new Dictionary<int, int>();

            try
            {
                dReturn = ascending
                ? dictionary.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value)
                : dictionary.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return dReturn;
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

        #region//发送 UDP 代理数据

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

        #region//接收 UDP 代理数据

        public static byte[] ReceiveUDPData(UdpClient ClientUDP, IAsyncResult ar, ref IPEndPoint ep)
        {
            try
            {
                if (ClientUDP != null && ClientUDP.Client != null)
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

        public static bool IsShowPacketConfig_ByFilter(PacketInfo spi)
        {  
            try
            {
                //套接字
                if (Operate.PacketConfig.Packet.CheckSocket)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_BySocket(spi.PacketSocket);
                    if (Operate.PacketConfig.Packet.CheckNotShow == bIsFilter)
                    {
                        return false;
                    }
                }

                //IP地址
                if (Operate.PacketConfig.Packet.CheckIP)
                {
                    bool bIsFilter_From = Socket_Operation.IsFilter_ByIP(spi.PacketFrom);
                    bool bIsFilter_To = Socket_Operation.IsFilter_ByIP(spi.PacketTo);
                    if (Operate.PacketConfig.Packet.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                    {
                        return false;
                    }     
                }

                //端口号
                if (Operate.PacketConfig.Packet.CheckPort)
                {
                    bool bIsFilter_From = Socket_Operation.IsFilter_ByPort(spi.PacketFrom);
                    bool bIsFilter_To = Socket_Operation.IsFilter_ByPort(spi.PacketTo);
                    if (Operate.PacketConfig.Packet.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                    {
                        return false;
                    }
                }

                //指定包头
                if (Operate.PacketConfig.Packet.CheckHead)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_ByHead(spi.PacketBuffer);
                    if (Operate.PacketConfig.Packet.CheckNotShow == bIsFilter)
                    {
                        return false;
                    }
                }

                //封包内容
                if (Operate.PacketConfig.Packet.CheckData)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_ByPacket(spi.PacketBuffer);
                    if (Operate.PacketConfig.Packet.CheckNotShow == bIsFilter)
                    {
                        return false;
                    }
                }

                //封包大小
                if (Operate.PacketConfig.Packet.CheckLen)
                {
                    bool bIsFilter = Socket_Operation.IsFilter_BySize(spi.PacketLen);
                    if (Operate.PacketConfig.Packet.CheckNotShow == bIsFilter)
                    {
                        return false;
                    }
                }               
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return true;
        }

        #region//检测套接字

        private static bool IsFilter_BySocket(int iPacketSocket)
        {
            try
            {
                if (!string.IsNullOrEmpty(Operate.PacketConfig.Packet.CheckSocket_Value))
                {
                    string[] sSocketArr = Operate.PacketConfig.Packet.CheckSocket_Value.Split(';');
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//检测IP地址

        private static bool IsFilter_ByIP(string sPacketIP)
        {
            try
            {
                if (string.IsNullOrEmpty(sPacketIP) || string.IsNullOrEmpty(Operate.PacketConfig.Packet.CheckIP_Value))
                {
                    return false;
                }

                string sIP = sPacketIP.Split(':')[0];
                HashSet<string> ipSet = new HashSet<string>(Operate.PacketConfig.Packet.CheckIP_Value.Split(';'));

                return ipSet.Contains(sIP);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//检测端口号

        private static bool IsFilter_ByPort(string sPacketPort)
        {
            try
            {
                if (string.IsNullOrEmpty(sPacketPort) || string.IsNullOrEmpty(Operate.PacketConfig.Packet.CheckPort_Value))
                {
                    return false;
                }

                string sPort = sPacketPort.Split(':')[1];
                HashSet<string> portSet = new HashSet<string>(Operate.PacketConfig.Packet.CheckPort_Value.Split(';'));

                return portSet.Contains(sPort);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//检测包头

        private static bool IsFilter_ByHead(byte[] bBuffer)
        {
            try
            {
                if (string.IsNullOrEmpty(Operate.PacketConfig.Packet.CheckHead_Value))
                {
                    return false;
                }

                string checkHeadValue = Operate.PacketConfig.Packet.CheckHead_Value.Replace(" ", "");
                string[] headValues = checkHeadValue.Split(';');

                foreach (string headValue in headValues)
                {
                    if (!string.IsNullOrEmpty(headValue))
                    {
                        byte[] headBytes = Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, headValue);

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//检测封包内容

        private static bool IsFilter_ByPacket(byte[] bBuffer)
        {
            try
            {
                if (string.IsNullOrEmpty(Operate.PacketConfig.Packet.CheckData_Value))
                {
                    return false;
                }

                string checkDataValue = Operate.PacketConfig.Packet.CheckData_Value.Replace(" ", "");
                string[] checkDataArray = checkDataValue.Split(';');

                string packetString = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, bBuffer).Replace(" ", "");

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//检测封包大小

        private static bool IsFilter_BySize(int PacketLength)
        {
            try
            {
                if (string.IsNullOrEmpty(Operate.PacketConfig.Packet.CheckLength_Value))
                {
                    return false;
                }

                string[] lengthArray = Operate.PacketConfig.Packet.CheckLength_Value.Split(';');

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #endregion

        #region//显示发送窗体

        public static void ShowSendForm(PacketInfo spi)
        {
            //try
            //{
            //    if (spi != null)
            //    {
            //        Socket_SendForm ssForm = new Socket_SendForm(spi);
            //        ssForm.Show();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示数据对比窗体

        public static void ShowSocketCompareForm(PacketInfo spi)
        {
            //try
            //{
            //    if (spi != null)
            //    {
            //        Socket_CompareForm compareForm = new Socket_CompareForm(spi);
            //        compareForm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion        

        #region//显示查找窗体

        public static void ShowFindForm()
        {
            //try
            //{
            //    Socket_FindForm sffFindForm = new Socket_FindForm();
            //    sffFindForm.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示账号管理窗体

        public static void ShowProxyAccountListForm()
        {
            //try
            //{
            //    if (!Operate.ProxyConfig.ProxyAccount.IsShow_ProxyAccount)
            //    {
            //        Proxy_AccountListForm palForm = new Proxy_AccountListForm();
            //        palForm.Show();
            //    }                
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        public static void ShowProxyAccountForm(Guid AID)
        {
            //try
            //{
            //    Proxy_AccountForm paForm = new Proxy_AccountForm(AID);
            //    paForm.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示账号认证记录窗体

        public static void ShowProxyAuthForm()
        {
            //try
            //{
            //    if (!Operate.ProxyConfig.ProxyAccount.IsShow_ProxyAuth)
            //    {
            //        Proxy_AccountAuthForm paaForm = new Proxy_AccountAuthForm();
            //        paaForm.Show();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示账号登录信息窗体（对话框）

        public static void ShowAccountLoginForm(Guid AID)
        {
            //try
            //{
            //    if (AID != null && AID != Guid.Empty)
            //    {
            //        Proxy_AccountLoginForm palForm = new Proxy_AccountLoginForm(AID);
            //        palForm.ShowDialog();
            //    }                
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示账号加时窗体（对话框）

        public static void ShowAccountTimeForm(List<Guid> gList)
        {
            //try
            //{
            //    if (gList.Count > 0)
            //    {
            //        Proxy_AccountTimeForm patForm = new Proxy_AccountTimeForm(gList);
            //        patForm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示账号链接数数窗体（对话框）

        public static void ShowAccountLinksForm(List<Guid> gList)
        {
            //try
            //{
            //    if (gList.Count > 0)
            //    {
            //        Proxy_AccountLinksForm palForm = new Proxy_AccountLinksForm(gList);
            //        palForm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示账号设备数窗体（对话框）

        public static void ShowAccountDevicesForm(List<Guid> gList)
        {
            //try
            //{
            //    if (gList.Count > 0)
            //    {
            //        Proxy_AccountDevicesForm padForm = new Proxy_AccountDevicesForm(gList);
            //        padForm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示本地映射窗体

        public static void ShowProxyMapLocalListForm()
        {
            //try
            //{
            //    if (!Operate.ProxyConfig.ProxyMapping.IsShow_MapLocal)
            //    {
            //        Proxy_MapLocalListForm pmllForm = new Proxy_MapLocalListForm();
            //        pmllForm.Show();
            //    }                
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        public static void ShowProxyMapLocalForm(Proxy_MapLocal pml)
        {
            //try
            //{
            //    Proxy_MapLocalForm pmlForm = new Proxy_MapLocalForm(pml);
            //    pmlForm.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示远程映射窗体

        public static void ShowProxyMapRemoteListForm()
        {
            //try
            //{
            //    if (!Operate.ProxyConfig.ProxyMapping.IsShow_MapRemote)
            //    {
            //        Proxy_MapRemoteListForm pmllForm = new Proxy_MapRemoteListForm();
            //        pmllForm.Show();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        public static void ShowProxyMapRemoteForm(Proxy_MapRemote pmr)
        {
            //try
            //{
            //    Proxy_MapRemoteForm pmrForm = new Proxy_MapRemoteForm(pmr);
            //    pmrForm.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示滤镜窗体（对话框）

        public static void ShowFilterForm_Dialog(Socket_FilterInfo sfi)
        {
            //try
            //{
            //    if (sfi != null)
            //    {
            //        Socket_FilterForm fFilterForm = new Socket_FilterForm(sfi);
            //        fFilterForm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion                

        #region//显示发送列表窗体（对话框）

        public static void ShowSendListForm_Dialog(Socket_SendInfo ssi)
        {
            //try
            //{
            //    if (ssi != null)
            //    {
            //        Socket_SendListForm fSendListForm = new Socket_SendListForm(ssi);
            //        fSendListForm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
        }

        #endregion

        #region//显示机器人窗体（对话框）

        public static void ShowRobotForm_Dialog(Socket_RobotInfo sri)
        {
            //try
            //{
            //    if (sri != null)
            //    {
            //        Socket_RobotForm fRobotForm = new Socket_RobotForm(sri);
            //        fRobotForm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            //}
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return dr;
        }

        #endregion        

        #region//发送封包

        public static unsafe bool SendPacket(int Socket, Operate.PacketConfig.Packet.PacketType packetType, string sIPFrom, string sIPTo, byte[] bSendBuffer)
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
                        case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                        case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                        case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                        case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                        case Operate.PacketConfig.Packet.PacketType.WSASend:
                        case Operate.PacketConfig.Packet.PacketType.WSASendTo:
                            sIPString = sIPTo;
                            break;
                        case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                        case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                        case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                        case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                        case Operate.PacketConfig.Packet.PacketType.WSARecv:
                        case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                        case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:
                            sIPString = sIPFrom;
                            break;
                    }

                    int res = -1;
                    switch (packetType)
                    {
                        case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                        case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                            res = NativeMethods.WSock32.send(Socket, ipSend, bSendBuffer.Length, SocketFlags.None);
                            break;
                        case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                        case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                        case Operate.PacketConfig.Packet.PacketType.WSASend:
                        case Operate.PacketConfig.Packet.PacketType.WSARecv:
                        case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                            res = NativeMethods.WS2_32.send(Socket, ipSend, bSendBuffer.Length, SocketFlags.None);
                            break;
                        case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                        case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                            if (!string.IsNullOrEmpty(sIPString))
                            {
                                Operate.PacketConfig.Packet.SockAddr saAddr = Socket_Operation.GetSocketAddr_ByIPString(sIPString);
                                res = NativeMethods.WSock32.sendto(Socket, ipSend, bSendBuffer.Length, SocketFlags.None, ref saAddr, Marshal.SizeOf(saAddr));
                            }
                            break;
                        case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                        case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                        case Operate.PacketConfig.Packet.PacketType.WSASendTo:
                        case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:
                            if (!string.IsNullOrEmpty(sIPString))
                            {
                                Operate.PacketConfig.Packet.SockAddr saAddr = Socket_Operation.GetSocketAddr_ByIPString(sIPString);
                                res = NativeMethods.WS2_32.sendto(Socket, ipSend, bSendBuffer.Length, SocketFlags.None, ref saAddr, Marshal.SizeOf(saAddr));
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
