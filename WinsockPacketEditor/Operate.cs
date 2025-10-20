using AntdUI;
using Be.Windows.Forms;
using DiffPlex.DiffBuilder.Model;
using Microsoft.Owin.Hosting;
using Microsoft.Win32;
using QQWry;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WinsockPacketEditor
{
    public static class Operate
    {
        #region//系统配置

        public static class SystemConfig
        {
            public static int PID = -1;
            public static int AutoSaveINT = 600000;
            public static string PNAME = string.Empty;
            public static string PATH = string.Empty;
            public static string WebSite_Tutorials_CN = "https://www.wpe64.com/tutorials.html";
            public static string WebSite_Tutorials_EN = "https://www.wpe64.com/tutorials_enUS.html";            
            public static string LastInjection = string.Empty;
            public static string WPE64_URL = "https://www.wpe64.com";
            public static string WPE64_IP = "http://101.132.222.195";
            public static string WPE64_Issuse = "https://github.com/x-nas/WinsockPacketEditor/issues";
            public static string WPE64_DLL = "WPEHook.dll";
            public static int HotKeyType = 0;
            public static string HotKey1 = "Ctrl + Alt + F1";
            public static string HotKey2 = "Ctrl + Alt + F2";
            public static string HotKey3 = "Ctrl + Alt + F3";
            public static string HotKey4 = "Ctrl + Alt + F4";
            public static string HotKey5 = "Ctrl + Alt + F5";
            public static string HotKey6 = "Ctrl + Alt + F6";
            public static string HotKey7 = "Ctrl + Alt + F7";
            public static string HotKey8 = "Ctrl + Alt + F8";
            public static string HotKey9 = "Ctrl + Alt + F9";
            public static string HotKey10 = "Ctrl + Alt + F10";
            public static string HotKey11 = "Ctrl + Alt + F11";
            public static string HotKey12 = "Ctrl + Alt + F12";
            public static SystemMode SelectMode = SystemMode.None;
            public static bool SpeedMode = false;
            public static DateTime StartTime = DateTime.Now;
            public static IntPtr MainHandle = IntPtr.Zero;
            public static int SystemSocket = 0;
            public static bool IsRemote = false;
            public static bool IsRemoteRunning = false;
            public static string Remote_IP, Remote_UserName, Remote_PassWord;
            public static ushort Remote_Port = 88;
            public static IDisposable WebServer;
            public static PerformanceCounter cpuCounter;
            public static bool IsShow_FloatButton = true;
            public static Execute ListExecute = Execute.Sequence;
            public static bool CheckNotShow = true, CheckLen, CheckSocket, CheckIP, CheckPort, CheckHead, CheckData, CheckType;
            public static string CheckSocket_Value, CheckLength_Value, CheckIP_Value, CheckPort_Value, CheckHead_Value, CheckData_Value;
            public static FilterConfig.Filter.FilterFunction CheckType_Value;
            public static Color SystemColor = Color.FromArgb(22, 119, 255);
            public static Color Color_30 = Color.FromArgb(30, 30, 30);
            public static Color Color_35 = Color.FromArgb(35, 35, 35);
            public static Color Color_40 = Color.FromArgb(40, 40, 40);
            public static Color Color_50 = Color.FromArgb(50, 50, 50);
            public static Color Color_57 = Color.FromArgb(57, 57, 57);
            public static Color Color_250 = Color.FromArgb(250, 250, 250);
            public static AntdUI.FormFloatButton FloatButton = null;
            public static DateTime MaxDateTime = DateTime.Parse("8888/12/31");

            public static Action<Action> InvokeAction { get; set; }

            #region//结构定义           

            public enum SystemMode
            {
                None = 0,
                Inject = 1,
                Proxy = 2,
            }

            public enum PWType
            {
                Import = 0,
                Export = 1,
            }

            public enum ListAction
            {
                Top = 0,
                Up = 1,
                Down = 2,
                Bottom = 3,
                Copy = 4,
                Export = 5,
                Delete = 6,
                CleanUp = 7,
                Import = 8,
            }

            public enum LogType
            {
                Socket,
                Proxy,
            }

            public enum Execute
            {
                Together,
                Sequence,
            }

            #endregion

            #region//注入参数

            [Serializable]

            public class InjectionParameters
            {
                public string DataBasePath { get; set; }

                public InjectionParameters()
                {
                    //
                }

                public InjectionParameters(string DBPath)
                {
                    DataBasePath = DBPath;
                }
            }

            #endregion

            #region//程序集特性访问器

            public static string AssemblyTitle
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false);
                    if (attributes.Length > 0)
                    {
                        System.Reflection.AssemblyTitleAttribute titleAttribute = (System.Reflection.AssemblyTitleAttribute)attributes[0];
                        if (titleAttribute.Title != "")
                        {
                            return titleAttribute.Title;
                        }
                    }
                    return System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                }
            }

            public static string AssemblyVersion
            {
                get
                {
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    return $"{version.Major}.{version.Minor}.{version.Build}";
                }
            }

            public static string AssemblyDescription
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyDescriptionAttribute), false);
                    if (attributes.Length == 0)
                    {
                        return "";
                    }
                    return ((System.Reflection.AssemblyDescriptionAttribute)attributes[0]).Description;
                }
            }

            public static string AssemblyProduct
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyProductAttribute), false);
                    if (attributes.Length == 0)
                    {
                        return "";
                    }
                    return ((System.Reflection.AssemblyProductAttribute)attributes[0]).Product;
                }
            }

            public static string AssemblyCopyright
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false);
                    if (attributes.Length == 0)
                    {
                        return "";
                    }
                    return ((System.Reflection.AssemblyCopyrightAttribute)attributes[0]).Copyright;
                }
            }

            public static string AssemblyCompany
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyCompanyAttribute), false);
                    if (attributes.Length == 0)
                    {
                        return "";
                    }
                    return ((System.Reflection.AssemblyCompanyAttribute)attributes[0]).Company;
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
                    Operate.DoLog(nameof(GetCPUAndMemory), ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//获取列表执行模式

            public static Execute GetListExecute_ByString(string sListExecute)
            {
                Execute leReturn = Execute.Sequence;

                try
                {
                    return (Execute)Enum.Parse(typeof(Execute), sListExecute);
                }
                catch (Exception ex)
                {
                    DoLog(nameof(GetListExecute_ByString), ex.Message);
                }

                return leReturn;
            }

            #endregion                      

            #region//获取本机的本地IP地址

            public static IPAddress[] GetLocalIPAddress()
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                    .Select(nic => new
                    {
                        Interface = nic,
                        IPAddresses = nic.GetIPProperties().UnicastAddresses
                            .Where(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)
                            .Select(addr => addr.Address)
                            .ToArray(),
                        Priority = GetInterfacePriority(nic)
                    })
                    .Where(item => item.IPAddresses.Length > 0)
                    .OrderBy(item => item.Priority) // 优先级数字越小，排序越靠前
                    .ThenBy(item => item.Interface.Name) // 次要排序：按接口名称
                    .SelectMany(item => item.IPAddresses)
                    .ToArray();
            }

            private static int GetInterfacePriority(NetworkInterface nic)
            {
                // 先检查是否为虚拟网卡
                if (IsVirtualNetworkAdapter(nic))
                {
                    return 90; // 虚拟网卡优先级较低
                }

                switch (nic.NetworkInterfaceType)
                {
                    case NetworkInterfaceType.Ethernet:
                    case NetworkInterfaceType.GigabitEthernet:
                    case NetworkInterfaceType.FastEthernetFx:
                    case NetworkInterfaceType.FastEthernetT:
                        return 0; // 物理有线网卡最高优先级

                    case NetworkInterfaceType.Wireless80211:
                        return 1; // 物理无线网卡

                    case NetworkInterfaceType.Wman:
                    case NetworkInterfaceType.Wwanpp:
                    case NetworkInterfaceType.Wwanpp2:
                        return 2; // 移动网络

                    case NetworkInterfaceType.Tunnel:
                        return 95;

                    case NetworkInterfaceType.Loopback:
                        return 100;

                    case NetworkInterfaceType.Ppp:
                    case NetworkInterfaceType.Slip:
                        return 97;

                    case NetworkInterfaceType.Unknown:
                        return 80;

                    default:
                        return 50;
                }
            }

            private static bool IsVirtualNetworkAdapter(NetworkInterface nic)
            {
                string description = nic.Description.ToLower();

                string[] virtualKeywords = new[]
                {
                    "vmware",
                    "virtual",
                    "hyper-v",
                    "virtualbox",
                    "vbox",
                    "v Ethernet",
                    "vEthernet",
                    "tap-",
                    "tun-",
                    "wireguard",
                    "zerotier",
                    "tailscale",
                    "docker",
                    "wintun"
                };

                return virtualKeywords.Any(keyword => description.Contains(keyword));
            }

            #endregion

            #region//获取列表的右键菜单

            public static AntdUI.IContextMenuStripItem[] GetCMS_List()
            {                
                List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                menuItems.Add(new AntdUI.ContextMenuStripItem("置顶", "Ctrl+⬆")
                {
                    ID = "Top",
                    IconSvg = "VerticalAlignTopOutlined",
                    LocalizationText = "Top",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                menuItems.Add(new AntdUI.ContextMenuStripItem("向上移动", "Alt+⬆")
                {
                    ID = "Up",
                    IconSvg = "ArrowUpOutlined",
                    LocalizationText = "Up",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItem("向下移动", "Alt+⬇")
                {
                    ID = "Down",
                    IconSvg = "ArrowDownOutlined",
                    LocalizationText = "Down",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                menuItems.Add(new AntdUI.ContextMenuStripItem("置底", "Ctrl+⬇")
                {
                    ID = "Bottom",
                    IconSvg = "VerticalAlignBottomOutlined",
                    LocalizationText = "Bottom",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                menuItems.Add(new AntdUI.ContextMenuStripItem("复制")
                {
                    ID = "Copy",
                    IconSvg = "CopyOutlined",
                    LocalizationText = "Copy",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItem("导出到文件")
                {
                    ID = "Export",
                    IconSvg = "DeliveredProcedureOutlined",
                    LocalizationText = "Export",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItem("删除")
                {
                    ID = "Delete",
                    IconSvg = "DeleteOutlined",
                    LocalizationText = "Delete",
                });                

                return menuItems.ToArray();
            }

            #endregion

            #region//获取异或计算的右键菜单

            public static AntdUI.IContextMenuStripItem[] GetCMS_XOR(HexBox hbPacketData)
            {
                List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                menuItems.Add(new AntdUI.ContextMenuStripItem("剪切")
                {
                    Enabled = hbPacketData.CanCut(),
                    ID = "Cut",
                    IconSvg = "ScissorOutlined",
                    LocalizationText = "Cut",
                });

                menuItems.Add(new AntdUI.ContextMenuStripItem("复制")
                {
                    Enabled = hbPacketData.CanCopy(),
                    ID = "Copy",
                    IconSvg = "CopyOutlined",
                    LocalizationText = "Copy",
                });

                menuItems.Add(new AntdUI.ContextMenuStripItem("粘贴")
                {
                    Enabled = hbPacketData.CanPaste(),
                    ID = "Paste",
                    IconSvg = "SnippetsOutlined",
                    LocalizationText = "Paste",
                });

                menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                menuItems.Add(new AntdUI.ContextMenuStripItem("全选")
                {
                    ID = "SelectAll",
                    IconSvg = "ProfileOutlined",
                    LocalizationText = "SelectAll",
                });

                return menuItems.ToArray();
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
                    Operate.DoLog(nameof(GetBoolFromChineseString), ex.Message);
                }

                return bReturn;
            }

            #endregion

            #region//获取系统运行模式名称

            public static string GetSystemModeName()
            {
                string sReturn = string.Empty;

                try
                {
                    switch (Operate.SystemConfig.SelectMode)
                    {
                        case Operate.SystemConfig.SystemMode.Proxy:
                            sReturn = AntdUI.Localization.Get("Proxy Mode", "代理模式");
                            break;

                        case Operate.SystemConfig.SystemMode.Inject:
                            sReturn = AntdUI.Localization.Get("Inject Mode", "注入模式");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(GetSystemModeName), ex.Message);
                }
                return sReturn;
            }

            #endregion

            #region//获取工作模式名称

            public static string GetWorkModeName()
            {
                string sReturn = string.Empty;

                try
                {
                    if (Operate.SystemConfig.SpeedMode)
                    {
                        sReturn = AntdUI.Localization.Get("Speed Mode", "极速模式");
                    }
                    else
                    {
                        sReturn = AntdUI.Localization.Get("Normal Mode", "普通模式");
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(GetWorkModeName), ex.Message);
                }
                return sReturn;
            }

            #endregion

            #region//获取IP的所属地

            public static async Task<string> GetIPLocation(string IPString)
            {
                try
                {
                    if (string.IsNullOrEmpty(IPString))
                    {
                        return string.Empty;
                    }

                    var IPSearch = await ProxyConfig.Proxy.ipSearch.GetIpLocationAsync(IPString);
                    if (IPSearch == null)
                    {
                        return string.Empty;
                    }

                    if (IPSearch.Country.Equals("IANA"))
                    {
                        return IPSearch.Area ?? string.Empty;
                    }
                    else
                    {
                        return (IPSearch.Country ?? string.Empty) + (IPSearch.Area ?? string.Empty);
                    }
                }
                catch
                {
                    //
                }

                return string.Empty;
            }

            #endregion

            #region//获取IP所属地图标

            private static readonly ConcurrentDictionary<string, byte[]> PngCache = new ConcurrentDictionary<string, byte[]>();

            private static readonly Dictionary<string, string> CountryNameToCode = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                #region//国家简称代码

                // A
                { "阿富汗", "af" },
                { "阿尔巴尼亚", "al" },
                { "阿尔及利亚", "dz" },
                { "安道尔", "ad" },
                { "安哥拉", "ao" },
                { "安提瓜和巴布达", "ag" },
                { "阿根廷", "ar" },
                { "亚美尼亚", "am" },
                { "澳大利亚", "au" },
                { "奥地利", "at" },
                { "阿塞拜疆", "az" },
    
                // B
                { "巴哈马", "bs" },
                { "巴林", "bh" },
                { "孟加拉国", "bd" },
                { "巴巴多斯", "bb" },
                { "白俄罗斯", "by" },
                { "比利时", "be" },
                { "伯利兹", "bz" },
                { "贝宁", "bj" },
                { "不丹", "bt" },
                { "玻利维亚", "bo" },
                { "波黑", "ba" },
                { "博茨瓦纳", "bw" },
                { "巴西", "br" },
                { "文莱", "bn" },
                { "保加利亚", "bg" },
                { "布基纳法索", "bf" },
                { "布隆迪", "bi" },
    
                // C
                { "佛得角", "cv" },
                { "柬埔寨", "kh" },
                { "喀麦隆", "cm" },
                { "加拿大", "ca" },
                { "中非共和国", "cf" },
                { "乍得", "td" },
                { "智利", "cl" },
                { "中国", "cn" },
                { "哥伦比亚", "co" },
                { "科摩罗", "km" },
                { "刚果（布）", "cg" },
                { "刚果（金）", "cd" },
                { "哥斯达黎加", "cr" },
                { "克罗地亚", "hr" },
                { "古巴", "cu" },
                { "塞浦路斯", "cy" },
                { "捷克", "cz" },
    
                // D
                { "丹麦", "dk" },
                { "吉布提", "dj" },
                { "多米尼克", "dm" },
                { "多米尼加", "do" },
    
                // E
                { "厄瓜多尔", "ec" },
                { "埃及", "eg" },
                { "萨尔瓦多", "sv" },
                { "赤道几内亚", "gq" },
                { "厄立特里亚", "er" },
                { "爱沙尼亚", "ee" },
                { "斯威士兰", "sz" },
                { "埃塞俄比亚", "et" },
    
                // F
                { "斐济", "fj" },
                { "芬兰", "fi" },
                { "法国", "fr" },
    
                // G
                { "加蓬", "ga" },
                { "冈比亚", "gm" },
                { "格鲁吉亚", "ge" },
                { "德国", "de" },
                { "加纳", "gh" },
                { "希腊", "gr" },
                { "格林纳达", "gd" },
                { "危地马拉", "gt" },
                { "几内亚", "gn" },
                { "几内亚比绍", "gw" },
                { "圭亚那", "gy" },
    
                // H
                { "海地", "ht" },
                { "洪都拉斯", "hn" },
                { "匈牙利", "hu" },
    
                // I
                { "冰岛", "is" },
                { "印度", "in" },
                { "印度尼西亚", "id" },
                { "伊朗", "ir" },
                { "伊拉克", "iq" },
                { "爱尔兰", "ie" },
                { "以色列", "il" },
                { "意大利", "it" },
                { "科特迪瓦", "ci" },
    
                // J
                { "牙买加", "jm" },
                { "日本", "jp" },
                { "约旦", "jo" },
    
                // K
                { "哈萨克斯坦", "kz" },
                { "肯尼亚", "ke" },
                { "基里巴斯", "ki" },
                { "朝鲜", "kp" },
                { "韩国", "kr" },
                { "科威特", "kw" },
                { "吉尔吉斯斯坦", "kg" },
    
                // L
                { "老挝", "la" },
                { "拉脱维亚", "lv" },
                { "黎巴嫩", "lb" },
                { "莱索托", "ls" },
                { "利比里亚", "lr" },
                { "利比亚", "ly" },
                { "列支敦士登", "li" },
                { "立陶宛", "lt" },
                { "卢森堡", "lu" },
    
                // M
                { "马达加斯加", "mg" },
                { "马拉维", "mw" },
                { "马来西亚", "my" },
                { "马尔代夫", "mv" },
                { "马里", "ml" },
                { "马耳他", "mt" },
                { "马绍尔群岛", "mh" },
                { "毛里塔尼亚", "mr" },
                { "毛里求斯", "mu" },
                { "墨西哥", "mx" },
                { "密克罗尼西亚", "fm" },
                { "摩尔多瓦", "md" },
                { "摩纳哥", "mc" },
                { "蒙古", "mn" },
                { "黑山", "me" },
                { "摩洛哥", "ma" },
                { "莫桑比克", "mz" },
                { "缅甸", "mm" },
    
                // N
                { "纳米比亚", "na" },
                { "瑙鲁", "nr" },
                { "尼泊尔", "np" },
                { "荷兰", "nl" },
                { "新西兰", "nz" },
                { "尼加拉瓜", "ni" },
                { "尼日尔", "ne" },
                { "尼日利亚", "ng" },
                { "北马其顿", "mk" },
                { "挪威", "no" },
    
                // O
                { "阿曼", "om" },
    
                // P
                { "巴基斯坦", "pk" },
                { "帕劳", "pw" },
                { "巴勒斯坦", "ps" },
                { "巴拿马", "pa" },
                { "巴布亚新几内亚", "pg" },
                { "巴拉圭", "py" },
                { "秘鲁", "pe" },
                { "菲律宾", "ph" },
                { "波兰", "pl" },
                { "葡萄牙", "pt" },
    
                // Q
                { "卡塔尔", "qa" },
    
                // R
                { "罗马尼亚", "ro" },
                { "俄罗斯", "ru" },
                { "卢旺达", "rw" },
    
                // S
                { "圣基茨和尼维斯", "kn" },
                { "圣卢西亚", "lc" },
                { "圣文森特和格林纳丁斯", "vc" },
                { "萨摩亚", "ws" },
                { "圣马力诺", "sm" },
                { "圣多美和普林西比", "st" },
                { "沙特阿拉伯", "sa" },
                { "塞内加尔", "sn" },
                { "塞尔维亚", "rs" },
                { "塞舌尔", "sc" },
                { "塞拉利昂", "sl" },
                { "新加坡", "sg" },
                { "斯洛伐克", "sk" },
                { "斯洛文尼亚", "si" },
                { "所罗门群岛", "sb" },
                { "索马里", "so" },
                { "南非", "za" },
                { "南苏丹", "ss" },
                { "西班牙", "es" },
                { "斯里兰卡", "lk" },
                { "苏丹", "sd" },
                { "苏里南", "sr" },
                { "瑞典", "se" },
                { "瑞士", "ch" },
                { "叙利亚", "sy" },
    
                // T
                { "塔吉克斯坦", "tj" },
                { "坦桑尼亚", "tz" },
                { "泰国", "th" },
                { "东帝汶", "tl" },
                { "多哥", "tg" },
                { "汤加", "to" },
                { "特立尼达和多巴哥", "tt" },
                { "突尼斯", "tn" },
                { "土耳其", "tr" },
                { "土库曼斯坦", "tm" },
                { "图瓦卢", "tv" },
    
                // U
                { "乌干达", "ug" },
                { "乌克兰", "ua" },
                { "阿联酋", "ae" },
                { "英国", "gb" },  // ISO 代码是 gb，非 uk
                { "美国", "us" },
                { "乌拉圭", "uy" },
                { "乌兹别克斯坦", "uz" },
    
                // V
                { "瓦努阿图", "vu" },
                { "梵蒂冈", "va" },
                { "委内瑞拉", "ve" },
                { "越南", "vn" },
    
                // Y
                { "也门", "ye" },
    
                // Z
                { "赞比亚", "zm" },
                { "津巴布韦", "zw" },
    
                // 特别行政区/地区（非主权国家）
                { "台湾地区", "tw" },  // 中国的省份
                { "香港地区", "hk" },  // 中国的特别行政区
                { "澳门地区", "mo" },  // 中国的特别行政区
                { "格陵兰", "gl" },   // 丹麦自治领地
                { "波多黎各", "pr" }, // 美国自治邦
                { "关岛", "gu" },     // 美国海外领地
                { "新喀里多尼亚", "nc" }, // 法国海外领地
                { "法属波利尼西亚", "pf" }, 
    
                // 特殊国际组织
                { "欧盟", "eu" },
                { "联合国", "un" },
                { "非洲联盟", "au" },  // 与澳大利亚代码冲突，需特殊处理
                { "阿拉伯国家联盟", "arab" } // 非标准代码

                #endregion
            };

            public static Image GetFlagByLocation(string IPLocation)
            {
                try
                {
                    if (string.IsNullOrEmpty(IPLocation))
                        return GetDefaultPng();

                    foreach (var pair in CountryNameToCode)
                    {
                        if (IPLocation.StartsWith(pair.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            var imageBytes = PngCache.GetOrAdd(pair.Value, code =>
                                GetFlagBytesByCountryCode(code));

                            using (var ms = new MemoryStream(imageBytes))
                            {
                                return Image.FromStream(ms);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DoLog(nameof(GetFlagByLocation), ex.Message);
                }

                return GetDefaultPng();
            }

            private static byte[] GetFlagBytesByCountryCode(string countryCode)
            {
                try
                {
                    var bitmap = Properties.Resources.ResourceManager.GetObject(countryCode.ToLower()) as Bitmap;
                    using (var ms = new MemoryStream())
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
                catch
                {
                    return GetDefaultPngBytes();
                }
            }

            private static byte[] GetDefaultPngBytes()
            {
                using (var ms = new MemoryStream())
                {
                    Properties.Resources.Flag_Local.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }

            private static Image GetDefaultPng()
            {
                return Properties.Resources.Flag_Local;
            }

            #endregion

            #region//获取列表的文字和背景颜色

            public static (Color ForeColor, Color BackColor)? GetFilterColors(Operate.FilterConfig.Filter.FilterAction filterAction)
            {
                switch (filterAction)
                {
                    case Operate.FilterConfig.Filter.FilterAction.Replace:
                        return (Operate.FilterConfig.Filter.FilterReplace_ForeColor,
                                Operate.FilterConfig.Filter.FilterReplace_BackColor);
                    case Operate.FilterConfig.Filter.FilterAction.Intercept:
                        return (Operate.FilterConfig.Filter.FilterIntercept_ForeColor,
                                Operate.FilterConfig.Filter.FilterIntercept_BackColor);
                    case Operate.FilterConfig.Filter.FilterAction.Change:
                        return (Operate.FilterConfig.Filter.FilterChange_ForeColor,
                                Operate.FilterConfig.Filter.FilterChange_BackColor);
                    default:
                        return null;
                }
            }

            #endregion

            #region//获取导入和导出的密码

            public static (bool DoEncrypt, string Password) GetEncryptExport(Form form, string Title)
            {
                bool DoEncrypt = false;
                string Password = string.Empty;

                try
                {
                    EncryptionPassword epControl = new EncryptionPassword(SystemConfig.PWType.Export);
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, epControl, TType.Info)
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        OnOk = config =>
                        {
                            Password = epControl.GetPassword();
                            if (string.IsNullOrEmpty(Password))
                            {
                                epControl.EncryptionText_Changed();

                                AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                {
                                    LocalizationText = "ExportList.Error"
                                });

                                return false;
                            }
                            else
                            {
                                DoEncrypt = true;
                                return true;
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(GetEncryptExport), ex.Message);
                }                

                return (DoEncrypt, Password);
            }

            public static XDocument GetEncryptImport(Form form, string Title, string FilePath)
            {
                XDocument xdReturn = null;

                try
                {
                    EncryptionPassword epControl = new EncryptionPassword(SystemConfig.PWType.Import);
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, epControl, TType.Info)
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        OnOk = config =>
                        {
                            string sPW = epControl.GetPassword();
                            if (string.IsNullOrEmpty(sPW))
                            {
                                epControl.EncryptionText_Changed();

                                AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                {
                                    LocalizationText = "ImportList.Error"
                                });

                                return false;
                            }
                            else
                            {
                                xdReturn = SystemConfig.DecryptXMLFile(FilePath, sPW);
                                return true;
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(GetEncryptImport), ex.Message);
                }                

                return xdReturn;
            }

            #endregion

            #region//初始化快捷键

            public static void InitHotKeys(IntPtr MainHandle)
            {
                Operate.SystemConfig.MainHandle = MainHandle;

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9001, Operate.SystemConfig.HotKey1))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9002, Operate.SystemConfig.HotKey2))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9003, Operate.SystemConfig.HotKey3))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9004, Operate.SystemConfig.HotKey4))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9005, Operate.SystemConfig.HotKey5))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9006, Operate.SystemConfig.HotKey6))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9007, Operate.SystemConfig.HotKey7))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9008, Operate.SystemConfig.HotKey8))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9009, Operate.SystemConfig.HotKey9))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9010, Operate.SystemConfig.HotKey10))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9011, Operate.SystemConfig.HotKey11))
                {
                    //
                }

                if (!Operate.SystemConfig.RegisterHotkey_FromText(9012, Operate.SystemConfig.HotKey12))
                {
                    //
                }
            }

            #endregion

            #region//初始化悬浮按钮

            public static void InitFloatButton(Form form)
            {
                if (SystemConfig.IsShow_FloatButton)
                {
                    if (Operate.SystemConfig.FloatButton == null)
                    {
                        Operate.SystemConfig.FloatButton = AntdUI.FloatButton.open(
                            new AntdUI.FloatButton.Config(form,
                            new AntdUI.FloatButton.ConfigBtn[]
                            {
                                new AntdUI.FloatButton.ConfigBtn("GitHub", "QuestionOutlined", true)
                                {
                                    Tooltip = "问题反馈",
                                    LocalizationTooltip = "Feedback",
                                    Type= AntdUI.TTypeMini.Success
                                },
                                new AntdUI.FloatButton.ConfigBtn("WebSite", "HomeOutlined", true)
                                {
                                    Tooltip = "访问官网",
                                    LocalizationTooltip = "OfficialWebsite",
                                    Type= AntdUI.TTypeMini.Default
                                }
                            }, btn =>
                            {
                                btn.Loading = true;

                                AntdUI.ITask.Run(() =>
                                {
                                    switch (btn.Name)
                                    {
                                        case "GitHub":
                                            Process.Start(Operate.SystemConfig.WPE64_Issuse);
                                            break;

                                        case "WebSite":
                                            Process.Start(Operate.SystemConfig.WPE64_URL);
                                            break;
                                    }

                                    btn.Loading = false;
                                });
                            }));
                    }
                    else
                    {
                        Operate.SystemConfig.FloatButton.Show();
                    }
                }
                else
                {
                    if (Operate.SystemConfig.FloatButton != null)
                    {
                        Operate.SystemConfig.FloatButton.Close();
                        Operate.SystemConfig.FloatButton = null;
                    }
                }
            }

            #endregion

            #region//初始化列表执行

            public static void InitListExecute()
            {
                Operate.SendConfig.List.bgwSendList.WorkerSupportsCancellation = true;
                Operate.SendConfig.List.bgwSendList.WorkerReportsProgress = false;
                Operate.SendConfig.List.bgwSendList.DoWork -= Operate.SendConfig.List.SendList_DoWork;
                Operate.SendConfig.List.bgwSendList.DoWork += Operate.SendConfig.List.SendList_DoWork;

                Operate.RobotConfig.List.bgwRobotList.WorkerSupportsCancellation = true;
                Operate.RobotConfig.List.bgwRobotList.WorkerReportsProgress = false;
                Operate.RobotConfig.List.bgwRobotList.DoWork -= Operate.RobotConfig.List.RobotList_DoWork;
                Operate.RobotConfig.List.bgwRobotList.DoWork += Operate.RobotConfig.List.RobotList_DoWork;
            }

            #endregion

            #region//初始化列表数据

            public static void InitSendInfo(AntdUI.Select sSendInfo, Guid SelectSID)
            {
                try
                {
                    if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                    {
                        var selectItems = Operate.SendConfig.List.lstSendInfo.Select(info => new SelectItem(info.SName, info)).ToArray();

                        sSendInfo.Items.Clear();
                        sSendInfo.Items.AddRange(selectItems);
                        sSendInfo.SelectedValue = Operate.SendConfig.Send.GetSend_ByGuid(SelectSID);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InitSendInfo), ex.Message);
                }
            }

            public static void InitRobotInfo(AntdUI.Select sRobotInfo, Guid SelectRID)
            {
                try
                {
                    if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
                    {
                        var selectItems = Operate.RobotConfig.List.lstRobotInfo.Select(info => new SelectItem(info.RName, info)).ToArray();

                        sRobotInfo.Items.Clear();
                        sRobotInfo.Items.AddRange(selectItems);
                        sRobotInfo.SelectedValue = Operate.RobotConfig.Robot.GetRobot_ByGuid(SelectRID);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InitRobotInfo), ex.Message);
                }
            }

            public static void InitFilterInfo(AntdUI.Select sFilterInfo, Guid SelectFID, Guid ExcludeFID)
            {
                try
                {
                    if (Operate.FilterConfig.List.lstFilterInfo.Count > 0)
                    {
                        var query = Operate.FilterConfig.List.lstFilterInfo.AsEnumerable();
                        query = query.Where(info => info.FID != ExcludeFID);

                        var selectItems = query
                            .Select(info => new SelectItem(info.FName, info))
                            .ToArray();

                        sFilterInfo.Items.Clear();
                        sFilterInfo.Items.AddRange(selectItems);
                        sFilterInfo.SelectedValue = Operate.FilterConfig.Filter.GetFilter_ByGuid(SelectFID);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InitFilterInfo), ex.Message);
                }
            }

            #endregion

            #region//查找树节点

            public static TreeItem FindNodeByName(AntdUI.Tree tree, string NodeName, string SubTitle)
            {
                try
                {
                    return FindNodeByName(tree.Items, NodeName, SubTitle);
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(FindNodeByName), ex.Message);
                }
                
                return null;
            }

            public static TreeItem FindNodeByName(TreeItemCollection items, string NodeName, string SubTitle)
            {
                try
                {
                    if (items == null || items.Count == 0)
                    {
                        return null;
                    } 

                    foreach (var item in items)
                    {
                        if (item.Name == NodeName || item.Text == NodeName)
                        {
                            if (string.IsNullOrEmpty(SubTitle))
                            {
                                return item;
                            }
                            else
                            {
                                if (item.SubTitle.Equals(SubTitle))
                                {
                                    return item;
                                }
                            }                                                        
                        }

                        var found = FindNodeByName(item.Sub, NodeName, SubTitle);
                        if (found != null)
                        {
                            return found;
                        } 
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(FindNodeByName), ex.Message);
                }                

                return null;
            }

            #endregion

            #region//启动远程管理

            public static void StartRemoteMGT(Form form)
            {
                try
                {
                    if (Operate.SystemConfig.IsRemote && !Operate.SystemConfig.IsRemoteRunning)
                    {
                        if (!string.IsNullOrEmpty(Operate.SystemConfig.Remote_IP) &&
                            !string.IsNullOrEmpty(Operate.SystemConfig.Remote_UserName) &&
                            !string.IsNullOrEmpty(Operate.SystemConfig.Remote_PassWord))
                        {
                            string sLog = string.Empty;
                            string Remote_URL = SystemConfig.GetRemoteMGT_URL(Operate.SystemConfig.Remote_IP, Operate.SystemConfig.Remote_Port.ToString());

                            try
                            {
                                Operate.SystemConfig.WebServer = WebApp.Start<Socket_Web>(Remote_URL);
                                ProxyConfig.Proxy.InitCCProxy_HTML();

                                sLog = string.Format(AntdUI.Localization.Get("MGT.Enabled", "远程管理已启用：{0}"), Remote_URL);
                                AntdUI.Message.open(new AntdUI.Message.Config(form, sLog, TType.Success));

                                Operate.SystemConfig.IsRemoteRunning = true;
                            }
                            catch
                            {
                                sLog = string.Format(AntdUI.Localization.Get("MGT.Error", "远程管理启动失败: 请尝试使用管理员权限启动 {0}"), Process.GetCurrentProcess().ProcessName);
                                AntdUI.Message.open(new AntdUI.Message.Config(form, sLog, TType.Error));
                            }

                            Operate.DoLog(nameof(StartRemoteMGT), sLog);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(StartRemoteMGT), ex.Message);
                }
            }

            public static void StopRemoteMGT(Form form)
            {
                try
                {
                    if (Operate.SystemConfig.WebServer != null && Operate.SystemConfig.IsRemoteRunning)
                    {
                        Operate.SystemConfig.WebServer.Dispose();
                        Operate.SystemConfig.IsRemoteRunning = false;

                        AntdUI.Message.open(new AntdUI.Message.Config(form, "远程管理已关闭", TType.Error)
                        {
                            LocalizationText = "RemoteMGTSetting.RemoteDisable"
                        });
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(StopRemoteMGT), ex.Message);
                }
            }

            #endregion

            #region//获取远程管理地址

            public static string GetRemoteMGT_URL(string RemoteIP, string RemotePort)
            {
                try
                {
                    if (!string.IsNullOrEmpty(RemoteIP) && !string.IsNullOrEmpty(RemotePort))
                    {
                        return string.Format("http://{0}:{1}", RemoteIP, RemotePort);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(GetRemoteMGT_URL), ex.Message);
                }

                return string.Empty;
            }

            #endregion

            #region//格式化速率字符串

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
                    Operate.DoLog(nameof(GetDisplayBytes), ex.Message);
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
                    Operate.DoLog(nameof(ConvertBytesDisplay), ex.Message);
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
                    Operate.DoLog(nameof(ConvertToOneDigit), ex.Message);
                }

                return sReturn;
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
                    Operate.DoLog(nameof(GetAESKeyFromString), ex.Message);
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
                    Operate.DoLog(nameof(EncryptXMLFile), ex.Message);
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
                    Operate.DoLog(nameof(DecryptXMLFile), ex.Message);
                }

                return xdReturn;
            }

            #endregion

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
                        if (SystemConfig.encryptionMap.TryGetValue(c, out string code))
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
                    DoLog(nameof(PassWord_Encrypt), ex.Message);
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
                            if (SystemConfig.decryptionMap.TryGetValue(code, out char c))
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
                    DoLog(nameof(PassWord_Decrypt), ex.Message);
                }

                return string.Empty;
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
                    Operate.DoLog(nameof(SortDictionaryByKey), ex.Message);
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
                    Operate.DoLog(nameof(SortDictionaryByValue), ex.Message);
                }

                return dReturn;
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
                            bReturn = SystemConfig.Hex_To_Bytes(sString);
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
                    Operate.DoLog(nameof(StringToBytes), ex.Message);
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
                    Operate.DoLog(nameof(BytesToString), ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//十六进制字符串转byte[]

            private static byte[] Hex_To_Bytes(string hexString)
            {
                try
                {
                    hexString = hexString.Replace(" ", "").Replace("-", "").Replace(":", "");

                    if (string.IsNullOrEmpty(hexString) || hexString.Length % 2 != 0)
                    {
                        return Array.Empty<byte>();
                    }

                    byte[] returnBytes = new byte[hexString.Length / 2];
                    ReadOnlySpan<char> hexSpan = hexString.AsSpan();

                    for (int i = 0; i < returnBytes.Length; i++)
                    {
                        int index = i * 2;
                        string byteStr = hexSpan.Slice(index, 2).ToString();
                        returnBytes[i] = Convert.ToByte(byteStr, 16);
                    }

                    return returnBytes;
                }
                catch
                {
                    return Array.Empty<byte>();
                }
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
                    byte[] bBuffer = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sString);
                    sReturn = Convert.ToBase64String(bBuffer);
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(Base64_Encoding), ex.Message);
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
                    Operate.DoLog(nameof(ByteArrayToInt16BigEndian), ex.Message);
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
                    Operate.DoLog(nameof(StringToBool), ex.Message);
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
                    Operate.DoLog(nameof(StringToDateTime), ex.Message);
                }

                return dtReturn;
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
                    Operate.DoLog(nameof(ConvertFILTString), ex.Message);
                }

                return Return;
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
                    Operate.DoLog(nameof(IsHexString), ex.Message);
                }

                return bReturn;
            }

            public static void VerifyHexCharWithWildcard(InputVerifyCharEventArgs verifyArgs, bool allowWildcard)
            {
                try
                {
                    char c = verifyArgs.Char;
                    if (c == '\b') // 退格键
                    {
                        verifyArgs.Result = true;
                        return;
                    }

                    // 根据参数决定是否允许通配符 *
                    if (allowWildcard && c == '*')
                    {
                        verifyArgs.Result = true;
                        return;
                    }

                    if (char.IsDigit(c))
                    {
                        verifyArgs.Result = true;
                    }
                    else if (c >= 'A' && c <= 'F')
                    {
                        verifyArgs.Result = true;
                    }
                    else if (c >= 'a' && c <= 'f')
                    {
                        verifyArgs.ReplaceText = c.ToString().ToUpper();
                        verifyArgs.Result = true;
                    }
                    else
                    {
                        verifyArgs.Result = false;
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(VerifyHexCharWithWildcard), ex.Message);
                }
            }

            public static bool ValidateHexValueWithWildcardAndShowMessage(Form form, string ValidateHex)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(ValidateHex, "^([0-9A-F*]{2})$"))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(form, "请输入有效的十六进制数值或通配符 (*)", TType.Error)
                    {
                        LocalizationText = "InvalidHex"
                    });

                    return false;
                }

                if (ValidateHex == "**")
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(form, "请使用留空替代 (**)", TType.Warn)
                    {
                        LocalizationText = "InvalidWildcard"
                    });

                    return false;
                }

                return true;
            }

            #endregion

            #region//判断是否是有效的IPV4字符串

            public static bool IsValidIPv4(string ipString)
            {
                if (string.IsNullOrWhiteSpace(ipString))
                    return false;

                string pattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
                return Regex.IsMatch(ipString, pattern);
            }

            #endregion

            #region//注册快捷键

            public static bool RegisterHotkey_FromText(int KeyID, string hkString)
            {
                try
                {
                    if (string.IsNullOrEmpty(hkString))
                    {
                        return false;
                    }                        

                    Keys parsedKey = SystemConfig.ParseHotkeyString(hkString);
                    if (parsedKey != Keys.None)
                    {
                        return SystemConfig.RegisterRecordedHotkey(KeyID, parsedKey);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(RegisterHotkey_FromText), ex.Message);
                }

                return false;
            }

            private static bool RegisterRecordedHotkey(int KeyID, Keys _currentKey)
            {
                try
                {
                    if (_currentKey != Keys.None && Operate.SystemConfig.MainHandle != IntPtr.Zero)
                    {
                        if (KeyID != 0)
                        {
                            User32.UnregisterHotKey(Operate.SystemConfig.MainHandle, KeyID);
                        }

                        uint modifiers = 0;
                        if ((_currentKey & Keys.Control) == Keys.Control)
                        {
                            modifiers |= 0x0002; // MOD_CONTROL
                        }

                        if ((_currentKey & Keys.Alt) == Keys.Alt)
                        {
                            modifiers |= 0x0001; // MOD_ALT
                        }

                        if ((_currentKey & Keys.Shift) == Keys.Shift)
                        {
                            modifiers |= 0x0004; // MOD_SHIFT
                        }

                        uint vk = 0;

                        if (_currentKey >= Keys.NumPad0 && _currentKey <= Keys.NumPad9)
                        {
                            vk = (uint)(_currentKey - Keys.NumPad0 + 0x60);
                        }
                        else
                        {
                            vk = (uint)(_currentKey & Keys.KeyCode);
                        }

                        return User32.RegisterHotKey(Operate.SystemConfig.MainHandle, KeyID, modifiers, vk);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(RegisterRecordedHotkey), ex.Message);
                }

                return false;
            }

            private static Keys ParseHotkeyString(string hotkeyString)
            {
                Keys result = Keys.None;

                try
                {
                    string[] parts = hotkeyString.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string part in parts)
                    {
                        string key = part.Trim();

                        if (key.Equals("Ctrl", StringComparison.OrdinalIgnoreCase))
                        {
                            result |= Keys.Control;
                        }
                        else if (key.Equals("Alt", StringComparison.OrdinalIgnoreCase))
                        {
                            result |= Keys.Alt;
                        }
                        else if (key.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                        {
                            result |= Keys.Shift;
                        }
                        else
                        {
                            if (key.Length == 1 && char.IsDigit(key[0]))
                            {
                                result |= (Keys)((int)Keys.D0 + (key[0] - '0'));
                            }
                            else if (key.StartsWith("NumPad", StringComparison.OrdinalIgnoreCase) && int.TryParse(key.Substring(6), out int numpadNum) && numpadNum >= 0 && numpadNum <= 9)
                            {
                                result |= (Keys)((int)Keys.NumPad0 + numpadNum);
                            }
                            else if (key.StartsWith("F", StringComparison.OrdinalIgnoreCase) && int.TryParse(key.Substring(1), out int fNum) && fNum >= 1 && fNum <= 24)
                            {
                                result |= (Keys)((int)Keys.F1 + fNum - 1);
                            }
                            else if (Enum.TryParse<Keys>(key, true, out Keys keyValue))
                            {
                                result |= keyValue;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(ParseHotkeyString), ex.Message);
                }

                return result;
            }

            public static string ConvertHotkeyToString(Keys key)
            {
                string result = "";

                try
                {
                    if ((key & Keys.Control) == Keys.Control)
                    {
                        result += "Ctrl + ";
                    }

                    if ((key & Keys.Alt) == Keys.Alt)
                    {
                        result += "Alt + ";
                    }

                    if ((key & Keys.Shift) == Keys.Shift)
                    {
                        result += "Shift + ";
                    }

                    Keys mainKey = key & Keys.KeyCode;

                    if (mainKey >= Keys.D0 && mainKey <= Keys.D9)
                    {
                        result += ((char)('0' + (mainKey - Keys.D0))).ToString();
                    }
                    else if (mainKey >= Keys.NumPad0 && mainKey <= Keys.NumPad9)
                    {
                        result += "NumPad" + (mainKey - Keys.NumPad0);
                    }
                    else
                    {
                        result += mainKey.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(ConvertHotkeyToString), ex.Message);
                }

                return result;
            }

            public static bool IsModifierKey(Keys key)
            {
                return key == Keys.ControlKey ||
                       key == Keys.LControlKey ||
                       key == Keys.RControlKey ||
                       key == Keys.Menu ||
                       key == Keys.LMenu ||
                       key == Keys.RMenu ||
                       key == Keys.ShiftKey ||
                       key == Keys.LShiftKey ||
                       key == Keys.RShiftKey;
            }

            #endregion

            #region//执行快捷键

            public static async Task DoHotKey(int HotKeyID)
            {
                try
                {
                    if (SystemConfig.HotKeyType == 0)
                    {
                        switch (HotKeyID)
                        {
                            case 9001:
                                await SendConfig.Send.DoSend_ByIndex(0);
                                break;

                            case 9002:
                                await SendConfig.Send.DoSend_ByIndex(1);
                                break;

                            case 9003:
                                await SendConfig.Send.DoSend_ByIndex(2);
                                break;

                            case 9004:
                                await SendConfig.Send.DoSend_ByIndex(3);
                                break;

                            case 9005:
                                await SendConfig.Send.DoSend_ByIndex(4);
                                break;

                            case 9006:
                                await SendConfig.Send.DoSend_ByIndex(5);
                                break;

                            case 9007:
                                await SendConfig.Send.DoSend_ByIndex(6);
                                break;

                            case 9008:
                                await SendConfig.Send.DoSend_ByIndex(7);
                                break;

                            case 9009:
                                await SendConfig.Send.DoSend_ByIndex(8);
                                break;

                            case 9010:
                                await SendConfig.Send.DoSend_ByIndex(9);
                                break;

                            case 9011:
                                SendConfig.List.StartSendList();
                                break;

                            case 9012:
                                SendConfig.List.StopSendList();
                                break;
                        }
                    }
                    else if (SystemConfig.HotKeyType == 1)
                    {
                        switch (HotKeyID)
                        {
                            case 9001:
                                await RobotConfig.Robot.DoRobot_ByIndex(0);
                                break;

                            case 9002:
                                await RobotConfig.Robot.DoRobot_ByIndex(1);
                                break;

                            case 9003:
                                await RobotConfig.Robot.DoRobot_ByIndex(2);
                                break;

                            case 9004:
                                await RobotConfig.Robot.DoRobot_ByIndex(3);
                                break;

                            case 9005:
                                await RobotConfig.Robot.DoRobot_ByIndex(4);
                                break;

                            case 9006:
                                await RobotConfig.Robot.DoRobot_ByIndex(5);
                                break;

                            case 9007:
                                await RobotConfig.Robot.DoRobot_ByIndex(6);
                                break;

                            case 9008:
                                await RobotConfig.Robot.DoRobot_ByIndex(7);
                                break;

                            case 9009:
                                await RobotConfig.Robot.DoRobot_ByIndex(8);
                                break;

                            case 9010:
                                await RobotConfig.Robot.DoRobot_ByIndex(9);
                                break;

                            case 9011:
                                RobotConfig.List.StartRobotList();
                                break;

                            case 9012:
                                RobotConfig.List.StopRobotList();
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DoHotKey), ex.Message);
                }
            }

            #endregion

            #region//文本对比

            public class DifferenceItem
            {
                public int Position { get; set; }
                public string ValueA { get; set; }
                public string ValueB { get; set; }
                public ChangeType ChangeType { get; set; }
            }

            public static List<DifferenceItem> CompareText(AntdUI.Input box1, AntdUI.Input box2)
            {
                var differences = new List<DifferenceItem>();

                try
                {
                    string text1 = box1.Text;
                    string text2 = box2.Text;
                    int maxLength = Math.Max(text1.Length, text2.Length);

                    for (int i = 0; i < maxLength; i++)
                    {
                        ChangeType changeType = GetCharDiffType(text1, text2, i);

                        // 记录差异项
                        if (changeType != ChangeType.Unchanged)
                        {
                            differences.Add(new DifferenceItem
                            {
                                Position = i + 1,
                                ValueA = i < text1.Length ? text1[i].ToString() : "N/A",
                                ValueB = i < text2.Length ? text2[i].ToString() : "N/A",
                                ChangeType = changeType
                            });
                        }

                        // 处理第一个文本框(input2) - 原始文本
                        if (i < text1.Length)
                        {
                            if (changeType == ChangeType.Deleted || changeType == ChangeType.Modified)
                            {
                                box1.SetStyle(i, 1,
                                            font: null,
                                            fore: Color.White,
                                            back: Color.FromArgb(220, 80, 80)); // 红色背景表示删除/修改
                            }
                        }

                        // 处理第二个文本框(input3) - 新文本
                        if (i < text2.Length)
                        {
                            if (changeType == ChangeType.Inserted || changeType == ChangeType.Modified)
                            {
                                box2.SetStyle(i, 1,
                                            font: null,
                                            fore: Color.White,
                                            back: Color.FromArgb(80, 180, 80)); // 绿色背景表示新增/修改
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CompareText), ex.Message);
                }

                return differences;
            }

            private static ChangeType GetCharDiffType(string str1, string str2, int position)
            {
                if (position >= str1.Length) return ChangeType.Inserted;
                if (position >= str2.Length) return ChangeType.Deleted;
                return str1[position] == str2[position] ? ChangeType.Unchanged : ChangeType.Modified;
            }

            #endregion            

            #region //文本查重

            public class DuplicateInfo
            {
                public string Sequence { get; set; }       // 重复的字节序列
                public int Length { get; set; }           // 序列长度(字节数)
                public int CountInA { get; set; }         // 在A中出现的次数
                public int CountInB { get; set; }         // 在B中出现的次数
                public List<int> PositionsInA { get; set; } // 在A中的位置列表(字节偏移)
                public List<int> PositionsInB { get; set; } // 在B中的位置列表(字节偏移)
            }

            public static (string TextA, string TextB, List<DuplicateInfo> Duplicates) ComparePackets(string stringA, string stringB, int minBytes)
            {
                try
                {
                    stringA = CleanAndNormalizeHex(stringA);
                    stringB = CleanAndNormalizeHex(stringB);

                    List<string> bytes1 = SplitIntoBytes(stringA);
                    List<string> bytes2 = SplitIntoBytes(stringB);

                    var commonSequences = FindCommonSequences(bytes1, bytes2, minBytes);
                    var duplicates = AnalyzeContinuousDuplicates(bytes1, bytes2, minBytes);

                    if (duplicates != null)
                    {
                        char[] result1 = new char[stringA.Length];
                        char[] result2 = new char[stringB.Length];

                        for (int i = 0; i < result1.Length; i++) result1[i] = '_';
                        for (int i = 0; i < result2.Length; i++) result2[i] = '_';

                        foreach (var seq in commonSequences)
                        {
                            for (int i = 0; i < seq.Length; i++)
                            {
                                int pos = seq.Pos1 * 2 + i * 2;
                                if (pos + 1 < result1.Length)
                                {
                                    result1[pos] = stringA[pos];
                                    result1[pos + 1] = stringA[pos + 1];
                                }
                            }

                            for (int i = 0; i < seq.Length; i++)
                            {
                                int pos = seq.Pos2 * 2 + i * 2;
                                if (pos + 1 < result2.Length)
                                {
                                    result2[pos] = stringB[pos];
                                    result2[pos + 1] = stringB[pos + 1];
                                }
                            }
                        }

                        return (new string(result1), new string(result2), duplicates);
                    }                    
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(ComparePackets), ex.Message);
                }

                return (string.Empty, string.Empty, new List<DuplicateInfo>());
            }

            public static List<DuplicateInfo> AnalyzeContinuousDuplicates(List<string> bytes1, List<string> bytes2, int minLength)
            {
                try
                {
                    var duplicates = new List<DuplicateInfo>();
                    var processedPositionsA = new HashSet<int>();
                    var processedPositionsB = new HashSet<int>();

                    // 查找所有连续的重复序列
                    for (int i = 0; i <= bytes1.Count - minLength; i++)
                    {
                        if (processedPositionsA.Contains(i)) continue;

                        for (int j = 0; j <= bytes2.Count - minLength; j++)
                        {
                            if (processedPositionsB.Contains(j)) continue;

                            // 查找从当前位置开始的最长连续匹配
                            int matchLen = 0;
                            while (i + matchLen < bytes1.Count &&
                                   j + matchLen < bytes2.Count &&
                                   bytes1[i + matchLen] == bytes2[j + matchLen])
                            {
                                matchLen++;
                            }

                            // 如果匹配长度满足要求
                            if (matchLen >= minLength)
                            {
                                var sequence = GetSequenceString(bytes1, i, matchLen);

                                // 记录在A中的所有连续出现位置
                                var positionsInA = new List<int>();
                                for (int k = i; k <= bytes1.Count - matchLen; k++)
                                {
                                    if (CompareSequences(bytes1, k, bytes2, j, matchLen))
                                    {
                                        positionsInA.Add(k);
                                        processedPositionsA.Add(k); // 标记已处理
                                    }
                                }

                                // 记录在B中的所有连续出现位置
                                var positionsInB = new List<int>();
                                for (int k = j; k <= bytes2.Count - matchLen; k++)
                                {
                                    if (CompareSequences(bytes2, k, bytes1, i, matchLen))
                                    {
                                        positionsInB.Add(k);
                                        processedPositionsB.Add(k); // 标记已处理
                                    }
                                }

                                if (positionsInA.Count > 0 && positionsInB.Count > 0)
                                {
                                    duplicates.Add(new DuplicateInfo
                                    {
                                        Sequence = sequence,
                                        Length = matchLen,
                                        CountInA = positionsInA.Count,
                                        CountInB = positionsInB.Count,
                                        PositionsInA = positionsInA,
                                        PositionsInB = positionsInB
                                    });
                                }

                                // 跳过已匹配的部分
                                i += matchLen - 1;
                                j += matchLen - 1;
                                break;
                            }
                        }
                    }

                    return duplicates.OrderByDescending(d => d.Length).ToList();
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(AnalyzeContinuousDuplicates), ex.Message);
                }

                return null;
            }

            private static string GetSequenceString(List<string> bytes, int start, int length)
            {
                return string.Join(" ", bytes.Skip(start).Take(length));
            }

            private static bool CompareSequences(List<string> source, int sourceStart, List<string> target, int targetStart, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    if (sourceStart + i >= source.Count || targetStart + i >= target.Count)
                        return false;

                    if (source[sourceStart + i] != target[targetStart + i])
                        return false;
                }
                return true;
            }

            public static string FormatHex(string hex)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hex.Length; i++)
                {
                    sb.Append(hex[i]);

                    if (i % 2 == 1 && i != hex.Length - 1)
                    {
                        sb.Append(" ");
                    }
                }

                return sb.ToString();
            }

            private static List<string> SplitIntoBytes(string hex)
            {
                List<string> bytes = new List<string>();

                for (int i = 0; i < hex.Length; i += 2)
                {
                    if (i + 1 < hex.Length)
                    {
                        bytes.Add(hex.Substring(i, 2));
                    }
                    else
                    {
                        bytes.Add(hex[i] + "0");
                    }
                }

                return bytes;
            }

            private static List<(int Pos1, int Pos2, int Length)> FindCommonSequences(List<string> bytes1, List<string> bytes2, int minLength)
            {
                var result = new List<(int, int, int)>();

                for (int i = 0; i < bytes1.Count; i++)
                {
                    for (int j = 0; j < bytes2.Count; j++)
                    {
                        if (bytes1[i] == bytes2[j])
                        {
                            int matchLen = 1;
                            while (i + matchLen < bytes1.Count && j + matchLen < bytes2.Count && bytes1[i + matchLen] == bytes2[j + matchLen])
                            {
                                matchLen++;
                            }

                            if (matchLen >= minLength)
                            {
                                result.Add((i, j, matchLen));

                                i += matchLen - 1;
                                j += matchLen - 1;

                                break;
                            }
                        }
                    }
                }

                return result;
            }

            private static string CleanAndNormalizeHex(string input)
            {
                StringBuilder sb = new StringBuilder();

                foreach (char c in input)
                {
                    if (char.IsDigit(c))
                    {
                        sb.Append(c);
                    }
                    else if (char.ToUpper(c) >= 'A' && char.ToUpper(c) <= 'F')
                    {
                        sb.Append(char.ToUpper(c));
                    }
                }

                return sb.ToString();
            }

            #endregion

            #region//文本过滤（正则表达式）

            public static void FindRegexMatches(string pattern, AntdUI.Input textBoxA, AntdUI.Input textBoxB)
            {
                try
                {
                    if (string.IsNullOrEmpty(pattern))
                    {
                        return;
                    }

                    textBoxA.ClearStyle();
                    textBoxB.ClearStyle();

                    foreach (Match match in Regex.Matches(textBoxA.Text, pattern))
                    {
                        textBoxA.SetStyle(match.Index, match.Length, font: null, fore: Color.White, back: Color.DarkSeaGreen);
                    }

                    foreach (Match match in Regex.Matches(textBoxB.Text, pattern))
                    {
                        textBoxB.SetStyle(match.Index, match.Length, font: null, fore: Color.White, back: Color.DarkSeaGreen);
                    }
                }
                catch
                {
                    //
                }
            }

            public static void LeachRegexMatches(string pattern, AntdUI.Input textBoxA, AntdUI.Input textBoxB)
            {
                try
                {
                    if (string.IsNullOrEmpty(pattern))
                    {
                        return;
                    }

                    textBoxA.ClearStyle();
                    textBoxB.ClearStyle();

                    StringBuilder sbA = new StringBuilder();
                    StringBuilder sbB = new StringBuilder();

                    foreach (Match match in Regex.Matches(textBoxA.Text, pattern))
                    {
                        sbA.Append(match.Value);
                    }
                    textBoxA.Text = sbA.ToString();

                    foreach (Match match in Regex.Matches(textBoxB.Text, pattern))
                    {
                        sbB.Append(match.Value);
                    }
                    textBoxB.Text = sbB.ToString();
                }
                catch
                {
                    //
                }
            }

            #endregion

            #region//支持取消的等待

            public static void DoSleep(int MilliSecond, BackgroundWorker Worker)
            {
                int elapsed = 0;
                int interval = 10;

                while (elapsed < MilliSecond)
                {
                    if (Worker.CancellationPending)
                    {
                        break;
                    }

                    Thread.Sleep(Math.Min(interval, MilliSecond - elapsed));
                    elapsed += interval;
                }
            }

            #endregion

            #region//保存注入进程名称到数据库

            public static void SaveSystemConfig_LastInjection_ToDB()
            {
                try
                {
                    DataBase.UpdateTable_SystemConfig_LastInjection();
                }
                catch (Exception ex)
                {
                    DoLog(nameof(SaveSystemConfig_LastInjection_ToDB), ex.Message);
                }
            }

            #endregion            

            #region//保存系统配置到数据库

            public static void SaveSystemConfig_ToDB()
            {
                try
                {
                    DataBase.DeleteTable_SystemConfig();
                    DataBase.InsertTable_SystemConfig();
                }
                catch (Exception ex)
                {
                    DoLog(nameof(SaveSystemConfig_ToDB), ex.Message);
                }
            }

            public static XElement GetSystemConfig_XML()
            {
                try
                {
                    XElement xeSystemConfig =
                        new XElement("SystemConfig",
                        new XElement("IsAnimation", AntdUI.Config.Animation),
                        new XElement("IsShadowEnabled", AntdUI.Config.ShadowEnabled),
                        new XElement("IsShowInWindow", AntdUI.Config.ShowInWindow),
                        new XElement("IsScrollBarHide", AntdUI.Config.ScrollBarHide),
                        new XElement("IsTextRenderingHighQuality", AntdUI.Config.TextRenderingHighQuality),
                        new XElement("IsDark", AntdUI.Config.IsDark),
                        new XElement("DefaultLanguage", AntdUI.Localization.CurrentLanguage),
                        new XElement("LastInjection", SystemConfig.LastInjection),
                        new XElement("Remote_IsEnable", SystemConfig.IsRemote),
                        new XElement("Remote_UserName", SystemConfig.Remote_UserName),
                        new XElement("Remote_PassWord", SystemConfig.Remote_PassWord),
                        new XElement("Remote_Port", SystemConfig.Remote_Port),
                        new XElement("Remote_IP", SystemConfig.Remote_IP),
                        new XElement("IsShow_FloatButton", SystemConfig.IsShow_FloatButton),
                        new XElement("ListExecute", SystemConfig.ListExecute),
                        new XElement("FilterExecute", FilterConfig.Filter.FilterExecute),
                        new XElement("LogList_AutoRoll", LogConfig.List.AutoRoll),
                        new XElement("LogList_AutoClear", LogConfig.List.AutoClear),
                        new XElement("LogList_AutoClear_Value", LogConfig.List.AutoClear_Value),
                        new XElement("CheckNotShow", SystemConfig.CheckNotShow),
                        new XElement("CheckSocket", SystemConfig.CheckSocket),
                        new XElement("CheckSocket_Value", SystemConfig.CheckSocket_Value),
                        new XElement("CheckIP", SystemConfig.CheckIP),
                        new XElement("CheckIP_Value", SystemConfig.CheckIP_Value),
                        new XElement("CheckPort", SystemConfig.CheckPort),
                        new XElement("CheckPort_Value", SystemConfig.CheckPort_Value),
                        new XElement("CheckHead", SystemConfig.CheckHead),
                        new XElement("CheckHead_Value", SystemConfig.CheckHead_Value),
                        new XElement("CheckData", SystemConfig.CheckData),
                        new XElement("CheckData_Value", SystemConfig.CheckData_Value),
                        new XElement("CheckSize", SystemConfig.CheckLen),
                        new XElement("CheckLength_Value", SystemConfig.CheckLength_Value),
                        new XElement("CheckType", SystemConfig.CheckType),
                        new XElement("CheckType_Value", FilterConfig.Filter.GetFilterFunctionString(SystemConfig.CheckType_Value)),
                        new XElement("HotKeyType", SystemConfig.HotKeyType),
                        new XElement("HotKey1", SystemConfig.HotKey1),
                        new XElement("HotKey2", SystemConfig.HotKey2),
                        new XElement("HotKey3", SystemConfig.HotKey3),
                        new XElement("HotKey4", SystemConfig.HotKey4),
                        new XElement("HotKey5", SystemConfig.HotKey5),
                        new XElement("HotKey6", SystemConfig.HotKey6),
                        new XElement("HotKey7", SystemConfig.HotKey7),
                        new XElement("HotKey8", SystemConfig.HotKey8),
                        new XElement("HotKey9", SystemConfig.HotKey9),
                        new XElement("HotKey10", SystemConfig.HotKey10),
                        new XElement("HotKey11", SystemConfig.HotKey11),
                        new XElement("HotKey12", SystemConfig.HotKey12),
                        new XElement("SystemColor", SystemConfig.SystemColor.ToArgb()),
                        new XElement("SpeedMode", SystemConfig.SpeedMode),
                        new XElement("FilterReplace_BackColor", FilterConfig.Filter.FilterReplace_BackColor.ToArgb()),
                        new XElement("FilterReplace_ForeColor", FilterConfig.Filter.FilterReplace_ForeColor.ToArgb()),
                        new XElement("FilterIntercept_BackColor", FilterConfig.Filter.FilterIntercept_BackColor.ToArgb()),
                        new XElement("FilterIntercept_ForeColor", FilterConfig.Filter.FilterIntercept_ForeColor.ToArgb()),
                        new XElement("FilterChange_BackColor", FilterConfig.Filter.FilterChange_BackColor.ToArgb()),
                        new XElement("FilterChange_ForeColor", FilterConfig.Filter.FilterChange_ForeColor.ToArgb())
                        );

                    return xeSystemConfig;
                }
                catch (Exception ex)
                {
                    DoLog(nameof(GetSystemConfig_XML), ex.Message);
                }

                return null;
            }

            #endregion

            #region//从数据库加载系统配置

            public static void LoadSystemConfig_FromDB()
            {
                try
                {
                    Operate.DataBase.InitConStr();

                    string Lang = "zh-CN";
                    AntdUI.Localization.DefaultLanguage = Lang;
                    AntdUI.Config.SetEmptyImageSvg(Properties.Resources.icon_empty, Properties.Resources.icon_empty_dark);

                    DataTable dtSystemConfig = DataBase.SelectTable_SystemConfig();
                    if (dtSystemConfig.Rows.Count > 0)
                    {
                        AntdUI.Config.Animation = Convert.ToBoolean(dtSystemConfig.Rows[0]["IsAnimation"]);
                        AntdUI.Config.ShadowEnabled = Convert.ToBoolean(dtSystemConfig.Rows[0]["IsShadowEnabled"]);
                        AntdUI.Config.ShowInWindow = Convert.ToBoolean(dtSystemConfig.Rows[0]["IsShowInWindow"]);
                        AntdUI.Config.ScrollBarHide = Convert.ToBoolean(dtSystemConfig.Rows[0]["IsScrollBarHide"]);
                        AntdUI.Config.TextRenderingHighQuality = Convert.ToBoolean(dtSystemConfig.Rows[0]["IsTextRenderingHighQuality"]);
                        AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                        AntdUI.Config.IsDark = Convert.ToBoolean(dtSystemConfig.Rows[0]["IsDark"]);                        
                        Lang = dtSystemConfig.Rows[0]["DefaultLanguage"].ToString();
                        SystemConfig.LastInjection = dtSystemConfig.Rows[0]["LastInjection"].ToString();
                        SystemConfig.IsRemote = Convert.ToBoolean(dtSystemConfig.Rows[0]["Remote_IsEnable"]);
                        SystemConfig.Remote_UserName = dtSystemConfig.Rows[0]["Remote_UserName"].ToString();
                        SystemConfig.Remote_PassWord = dtSystemConfig.Rows[0]["Remote_PassWord"].ToString();
                        SystemConfig.Remote_Port = ushort.Parse(dtSystemConfig.Rows[0]["Remote_Port"].ToString());
                        SystemConfig.Remote_IP = dtSystemConfig.Rows[0]["Remote_IP"].ToString();
                        SystemConfig.IsShow_FloatButton = Convert.ToBoolean(dtSystemConfig.Rows[0]["IsShow_FloatButton"]);
                        SystemConfig.ListExecute = GetListExecute_ByString(dtSystemConfig.Rows[0]["ListExecute"].ToString());
                        FilterConfig.Filter.FilterExecute = FilterConfig.List.GetFilterListExecute_ByString(dtSystemConfig.Rows[0]["FilterExecute"].ToString());
                        LogConfig.List.AutoRoll = Convert.ToBoolean(dtSystemConfig.Rows[0]["LogList_AutoRoll"]);
                        LogConfig.List.AutoClear = Convert.ToBoolean(dtSystemConfig.Rows[0]["LogList_AutoClear"]);
                        LogConfig.List.AutoClear_Value = Convert.ToInt32(dtSystemConfig.Rows[0]["LogList_AutoClear_Value"]);
                        SystemConfig.CheckNotShow = Convert.ToBoolean(dtSystemConfig.Rows[0]["CheckNotShow"]);
                        SystemConfig.CheckSocket = Convert.ToBoolean(dtSystemConfig.Rows[0]["CheckSocket"]);
                        SystemConfig.CheckSocket_Value = dtSystemConfig.Rows[0]["CheckSocket_Value"].ToString();
                        SystemConfig.CheckIP = Convert.ToBoolean(dtSystemConfig.Rows[0]["CheckIP"]);
                        SystemConfig.CheckIP_Value = dtSystemConfig.Rows[0]["CheckIP_Value"].ToString();
                        SystemConfig.CheckPort = Convert.ToBoolean(dtSystemConfig.Rows[0]["CheckPort"]);
                        SystemConfig.CheckPort_Value = dtSystemConfig.Rows[0]["CheckPort_Value"].ToString();
                        SystemConfig.CheckHead = Convert.ToBoolean(dtSystemConfig.Rows[0]["CheckHead"]);
                        SystemConfig.CheckHead_Value = dtSystemConfig.Rows[0]["CheckHead_Value"].ToString();
                        SystemConfig.CheckData = Convert.ToBoolean(dtSystemConfig.Rows[0]["CheckData"]);
                        SystemConfig.CheckData_Value = dtSystemConfig.Rows[0]["CheckData_Value"].ToString();
                        SystemConfig.CheckLen = Convert.ToBoolean(dtSystemConfig.Rows[0]["CheckSize"]);
                        SystemConfig.CheckLength_Value = dtSystemConfig.Rows[0]["CheckLength_Value"].ToString();
                        SystemConfig.CheckType = Convert.ToBoolean(dtSystemConfig.Rows[0]["CheckType"]);
                        SystemConfig.CheckType_Value = FilterConfig.Filter.GetFilterFunction_ByString(dtSystemConfig.Rows[0]["CheckType_Value"].ToString());
                        SystemConfig.HotKeyType = Convert.ToInt32(dtSystemConfig.Rows[0]["HotKeyType"]);
                        SystemConfig.HotKey1 = dtSystemConfig.Rows[0]["HotKey1"].ToString();
                        SystemConfig.HotKey2 = dtSystemConfig.Rows[0]["HotKey2"].ToString();
                        SystemConfig.HotKey3 = dtSystemConfig.Rows[0]["HotKey3"].ToString();
                        SystemConfig.HotKey4 = dtSystemConfig.Rows[0]["HotKey4"].ToString();
                        SystemConfig.HotKey5 = dtSystemConfig.Rows[0]["HotKey5"].ToString();
                        SystemConfig.HotKey6 = dtSystemConfig.Rows[0]["HotKey6"].ToString();
                        SystemConfig.HotKey7 = dtSystemConfig.Rows[0]["HotKey7"].ToString();
                        SystemConfig.HotKey8 = dtSystemConfig.Rows[0]["HotKey8"].ToString();
                        SystemConfig.HotKey9 = dtSystemConfig.Rows[0]["HotKey9"].ToString();
                        SystemConfig.HotKey10 = dtSystemConfig.Rows[0]["HotKey10"].ToString();
                        SystemConfig.HotKey11 = dtSystemConfig.Rows[0]["HotKey11"].ToString();
                        SystemConfig.HotKey12 = dtSystemConfig.Rows[0]["HotKey12"].ToString();
                        SystemConfig.SystemColor = Color.FromArgb(Convert.ToInt32(dtSystemConfig.Rows[0]["SystemColor"]));
                        SystemConfig.SpeedMode = Convert.ToBoolean(dtSystemConfig.Rows[0]["SpeedMode"]);
                        FilterConfig.Filter.FilterReplace_ForeColor = Color.FromArgb(Convert.ToInt32(dtSystemConfig.Rows[0]["FilterReplace_ForeColor"]));
                        FilterConfig.Filter.FilterReplace_BackColor = Color.FromArgb(Convert.ToInt32(dtSystemConfig.Rows[0]["FilterReplace_BackColor"]));
                        FilterConfig.Filter.FilterIntercept_ForeColor = Color.FromArgb(Convert.ToInt32(dtSystemConfig.Rows[0]["FilterIntercept_ForeColor"]));
                        FilterConfig.Filter.FilterIntercept_BackColor = Color.FromArgb(Convert.ToInt32(dtSystemConfig.Rows[0]["FilterIntercept_BackColor"]));
                        FilterConfig.Filter.FilterChange_ForeColor = Color.FromArgb(Convert.ToInt32(dtSystemConfig.Rows[0]["FilterChange_ForeColor"]));
                        FilterConfig.Filter.FilterChange_BackColor = Color.FromArgb(Convert.ToInt32(dtSystemConfig.Rows[0]["FilterChange_BackColor"]));
                    }
                    else
                    {
                        AntdUI.Config.ShowInWindow = true;
                        AntdUI.Config.TextRenderingHighQuality = true;
                        AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    }

                    if (Lang.StartsWith("en"))
                    {
                        AntdUI.Localization.Provider = new Localizer();
                    }
                    else
                    {
                        AntdUI.Localization.Provider = null;
                    }

                    AntdUI.Localization.SetLanguage(Lang);                    
                }
                catch (Exception ex)
                {
                    DoLog(nameof(LoadSystemConfig_FromDB), ex.Message);
                }
            }

            public static void SetSystemConfig_FromXML(XElement xeSystemConfig)
            {
                try
                {
                    XElement xeIsAnimation = xeSystemConfig.Element("IsAnimation");
                    if (xeIsAnimation != null)
                    {
                        AntdUI.Config.Animation = Convert.ToBoolean(xeIsAnimation.Value);
                    }

                    XElement xeIsShadowEnabled = xeSystemConfig.Element("IsShadowEnabled");
                    if (xeIsShadowEnabled != null)
                    {
                        AntdUI.Config.ShadowEnabled = Convert.ToBoolean(xeIsShadowEnabled.Value);
                    }

                    XElement xeIsShowInWindow = xeSystemConfig.Element("IsShowInWindow");
                    if (xeIsShowInWindow != null)
                    {
                        AntdUI.Config.ShowInWindow = Convert.ToBoolean(xeIsShowInWindow.Value);
                    }

                    XElement xeIsScrollBarHide = xeSystemConfig.Element("IsScrollBarHide");
                    if (xeIsScrollBarHide != null)
                    {
                        AntdUI.Config.ScrollBarHide = Convert.ToBoolean(xeIsScrollBarHide.Value);
                    }

                    XElement xeIsTextRenderingHighQuality = xeSystemConfig.Element("IsTextRenderingHighQuality");
                    if (xeIsTextRenderingHighQuality != null)
                    {
                        AntdUI.Config.TextRenderingHighQuality = Convert.ToBoolean(xeIsTextRenderingHighQuality.Value);
                    }

                    XElement xeIsDark = xeSystemConfig.Element("IsDark");
                    if (xeIsDark != null)
                    {
                        AntdUI.Config.IsDark = Convert.ToBoolean(xeIsDark.Value);
                    }

                    XElement xeDefaultLanguage = xeSystemConfig.Element("DefaultLanguage");
                    if (xeDefaultLanguage != null)
                    {
                        string Lang = xeDefaultLanguage.Value;
                        if (Lang.StartsWith("en"))
                        {
                            AntdUI.Localization.Provider = new Localizer();
                        }
                        else
                        {
                            AntdUI.Localization.Provider = null;
                        }

                        AntdUI.Localization.DefaultLanguage = "zh-CN";
                        AntdUI.Localization.SetLanguage(Lang);
                    }

                    XElement xeLastInjection = xeSystemConfig.Element("LastInjection");
                    if (xeLastInjection != null)
                    {
                        SystemConfig.LastInjection = xeLastInjection.Value;
                    }

                    XElement xeIsRemote = xeSystemConfig.Element("Remote_IsEnable");
                    if (xeIsRemote != null)
                    {
                        SystemConfig.IsRemote = Convert.ToBoolean(xeIsRemote.Value);
                    }

                    XElement xeRemote_UserName = xeSystemConfig.Element("Remote_UserName");
                    if (xeRemote_UserName != null)
                    {
                        SystemConfig.Remote_UserName = xeRemote_UserName.Value;
                    }

                    XElement xeRemote_PassWord = xeSystemConfig.Element("Remote_PassWord");
                    if (xeRemote_PassWord != null)
                    {
                        SystemConfig.Remote_PassWord = xeRemote_PassWord.Value;
                    }

                    XElement xeRemote_Port = xeSystemConfig.Element("Remote_Port");
                    if (xeRemote_Port != null)
                    {
                        SystemConfig.Remote_Port = ushort.Parse(xeRemote_Port.Value);
                    }

                    XElement xeRemote_IP = xeSystemConfig.Element("Remote_IP");
                    if (xeRemote_IP != null)
                    {
                        SystemConfig.Remote_IP = xeRemote_IP.Value;
                    }

                    XElement IsShow_FloatButton = xeSystemConfig.Element("IsShow_FloatButton");
                    if (IsShow_FloatButton != null)
                    {
                        SystemConfig.IsShow_FloatButton = Convert.ToBoolean(IsShow_FloatButton.Value);
                    }

                    XElement xeListExecute = xeSystemConfig.Element("ListExecute");
                    if (xeListExecute != null)
                    {
                        SystemConfig.ListExecute = GetListExecute_ByString(xeListExecute.Value);
                    }

                    XElement FilterExecute = xeSystemConfig.Element("FilterExecute");
                    if (FilterExecute != null)
                    {
                        FilterConfig.Filter.FilterExecute = FilterConfig.List.GetFilterListExecute_ByString(FilterExecute.Value);
                    }

                    XElement LogList_AutoRoll = xeSystemConfig.Element("LogList_AutoRoll");
                    if (LogList_AutoRoll != null)
                    {
                        LogConfig.List.AutoRoll = Convert.ToBoolean(LogList_AutoRoll.Value);
                    }

                    XElement LogList_AutoClear = xeSystemConfig.Element("LogList_AutoClear");
                    if (LogList_AutoClear != null)
                    {
                        LogConfig.List.AutoClear = Convert.ToBoolean(LogList_AutoClear.Value);
                    }

                    XElement LogList_AutoClear_Value = xeSystemConfig.Element("LogList_AutoClear_Value");
                    if (LogList_AutoClear_Value != null)
                    {
                        LogConfig.List.AutoClear_Value = int.Parse(LogList_AutoClear_Value.Value);
                    }

                    XElement CheckNotShow = xeSystemConfig.Element("CheckNotShow");
                    if (CheckNotShow != null)
                    {
                        SystemConfig.CheckNotShow = Convert.ToBoolean(CheckNotShow.Value);
                    }

                    XElement CheckSocket = xeSystemConfig.Element("CheckSocket");
                    if (CheckSocket != null)
                    {
                        SystemConfig.CheckSocket = Convert.ToBoolean(CheckSocket.Value);
                    }

                    XElement CheckSocket_Value = xeSystemConfig.Element("CheckSocket_Value");
                    if (CheckSocket_Value != null)
                    {
                        SystemConfig.CheckSocket_Value = CheckSocket_Value.Value;
                    }

                    XElement CheckIP = xeSystemConfig.Element("CheckIP");
                    if (CheckIP != null)
                    {
                        SystemConfig.CheckIP = Convert.ToBoolean(CheckIP.Value);
                    }

                    XElement CheckIP_Value = xeSystemConfig.Element("CheckIP_Value");
                    if (CheckIP_Value != null)
                    {
                        SystemConfig.CheckIP_Value = CheckIP_Value.Value;
                    }

                    XElement CheckPort = xeSystemConfig.Element("CheckPort");
                    if (CheckPort != null)
                    {
                        SystemConfig.CheckPort = Convert.ToBoolean(CheckPort.Value);
                    }

                    XElement CheckPort_Value = xeSystemConfig.Element("CheckPort_Value");
                    if (CheckPort_Value != null)
                    {
                        SystemConfig.CheckPort_Value = CheckPort_Value.Value;
                    }

                    XElement CheckHead = xeSystemConfig.Element("CheckHead");
                    if (CheckHead != null)
                    {
                        SystemConfig.CheckHead = Convert.ToBoolean(CheckHead.Value);
                    }

                    XElement CheckHead_Value = xeSystemConfig.Element("CheckHead_Value");
                    if (CheckHead_Value != null)
                    {
                        SystemConfig.CheckHead_Value = CheckHead_Value.Value;
                    }

                    XElement CheckData = xeSystemConfig.Element("CheckData");
                    if (CheckData != null)
                    {
                        SystemConfig.CheckData = Convert.ToBoolean(CheckData.Value);
                    }

                    XElement CheckData_Value = xeSystemConfig.Element("CheckData_Value");
                    if (CheckData_Value != null)
                    {
                        SystemConfig.CheckData_Value = CheckData_Value.Value;
                    }

                    XElement CheckSize = xeSystemConfig.Element("CheckSize");
                    if (CheckSize != null)
                    {
                        SystemConfig.CheckLen = Convert.ToBoolean(CheckSize.Value);
                    }

                    XElement CheckLength_Value = xeSystemConfig.Element("CheckLength_Value");
                    if (CheckLength_Value != null)
                    {
                        SystemConfig.CheckLength_Value = CheckLength_Value.Value;
                    }

                    XElement CheckType = xeSystemConfig.Element("CheckType");
                    if (CheckType != null)
                    {
                        SystemConfig.CheckType = Convert.ToBoolean(CheckType.Value);
                    }

                    XElement CheckType_Value = xeSystemConfig.Element("CheckType_Value");
                    if (CheckType_Value != null)
                    {
                        SystemConfig.CheckType_Value = FilterConfig.Filter.GetFilterFunction_ByString(CheckType_Value.Value);
                    }

                    XElement HotKeyType = xeSystemConfig.Element("HotKeyType");
                    if (HotKeyType != null)
                    {
                        SystemConfig.HotKeyType = int.Parse(HotKeyType.Value);
                    }

                    XElement HotKey1 = xeSystemConfig.Element("HotKey1");
                    if (HotKey1 != null)
                    {
                        SystemConfig.HotKey1 = HotKey1.Value;
                    }

                    XElement HotKey2 = xeSystemConfig.Element("HotKey2");
                    if (HotKey2 != null)
                    {
                        SystemConfig.HotKey2 = HotKey2.Value;
                    }

                    XElement HotKey3 = xeSystemConfig.Element("HotKey3");
                    if (HotKey3 != null)
                    {
                        SystemConfig.HotKey3 = HotKey3.Value;
                    }

                    XElement HotKey4 = xeSystemConfig.Element("HotKey4");
                    if (HotKey4 != null)
                    {
                        SystemConfig.HotKey4 = HotKey4.Value;
                    }

                    XElement HotKey5 = xeSystemConfig.Element("HotKey5");
                    if (HotKey5 != null)
                    {
                        SystemConfig.HotKey5 = HotKey5.Value;
                    }

                    XElement HotKey6 = xeSystemConfig.Element("HotKey6");
                    if (HotKey6 != null)
                    {
                        SystemConfig.HotKey6 = HotKey6.Value;
                    }

                    XElement HotKey7 = xeSystemConfig.Element("HotKey7");
                    if (HotKey7 != null)
                    {
                        SystemConfig.HotKey7 = HotKey7.Value;
                    }

                    XElement HotKey8 = xeSystemConfig.Element("HotKey8");
                    if (HotKey8 != null)
                    {
                        SystemConfig.HotKey8 = HotKey8.Value;
                    }

                    XElement HotKey9 = xeSystemConfig.Element("HotKey9");
                    if (HotKey9 != null)
                    {
                        SystemConfig.HotKey9 = HotKey9.Value;
                    }

                    XElement HotKey10 = xeSystemConfig.Element("HotKey10");
                    if (HotKey10 != null)
                    {
                        SystemConfig.HotKey10 = HotKey10.Value;
                    }

                    XElement HotKey11 = xeSystemConfig.Element("HotKey11");
                    if (HotKey11 != null)
                    {
                        SystemConfig.HotKey11 = HotKey11.Value;
                    }

                    XElement HotKey12 = xeSystemConfig.Element("HotKey12");
                    if (HotKey12 != null)
                    {
                        SystemConfig.HotKey12 = HotKey12.Value;
                    }

                    XElement SystemColor = xeSystemConfig.Element("SystemColor");
                    if (SystemColor != null)
                    {
                        SystemConfig.SystemColor = Color.FromArgb(Convert.ToInt32(SystemColor.Value));
                    }

                    XElement xeSpeedMode = xeSystemConfig.Element("SpeedMode");
                    if (xeSpeedMode != null)
                    {
                        SystemConfig.SpeedMode = Convert.ToBoolean(xeSpeedMode.Value);
                    }

                    XElement FilterReplace_BackColor = xeSystemConfig.Element("FilterReplace_BackColor");
                    if (FilterReplace_BackColor != null)
                    {
                        FilterConfig.Filter.FilterReplace_BackColor = Color.FromArgb(Convert.ToInt32(FilterReplace_BackColor.Value));
                    }

                    XElement FilterReplace_ForeColor = xeSystemConfig.Element("FilterReplace_ForeColor");
                    if (FilterReplace_ForeColor != null)
                    {
                        FilterConfig.Filter.FilterReplace_ForeColor = Color.FromArgb(Convert.ToInt32(FilterReplace_ForeColor.Value));
                    }

                    XElement FilterIntercept_BackColor = xeSystemConfig.Element("FilterIntercept_BackColor");
                    if (FilterIntercept_BackColor != null)
                    {
                        FilterConfig.Filter.FilterIntercept_BackColor = Color.FromArgb(Convert.ToInt32(FilterIntercept_BackColor.Value));
                    }

                    XElement FilterIntercept_ForeColor = xeSystemConfig.Element("FilterIntercept_ForeColor");
                    if (FilterIntercept_ForeColor != null)
                    {
                        FilterConfig.Filter.FilterIntercept_ForeColor = Color.FromArgb(Convert.ToInt32(FilterIntercept_ForeColor.Value));
                    }

                    XElement FilterChange_BackColor = xeSystemConfig.Element("FilterChange_BackColor");
                    if (FilterChange_BackColor != null)
                    {
                        FilterConfig.Filter.FilterChange_BackColor = Color.FromArgb(Convert.ToInt32(FilterChange_BackColor.Value));
                    }

                    XElement FilterChange_ForeColor = xeSystemConfig.Element("FilterChange_ForeColor");
                    if (FilterChange_ForeColor != null)
                    {
                        FilterConfig.Filter.FilterChange_ForeColor = Color.FromArgb(Convert.ToInt32(FilterChange_ForeColor.Value));
                    }
                }
                catch (Exception ex)
                {
                    DoLog(nameof(SetSystemConfig_FromXML), ex.Message);
                }
            }

            #endregion

            #region//保存注入模式配置到数据库

            public static void SaveInjectMode_ToDB()
            {
                DataBase.DeleteTable_InjectMode();
                DataBase.InsertTable_InjectMode();
            }            

            public static XElement GetInjectMode_XML()
            {
                try
                {
                    XElement xeInjectMode =
                        new XElement("InjectMode",                        
                        new XElement("HookWS1_Send", PacketConfig.Packet.HookWS1_Send),
                        new XElement("HookWS1_SendTo", PacketConfig.Packet.HookWS1_SendTo),
                        new XElement("HookWS1_Recv", PacketConfig.Packet.HookWS1_Recv),
                        new XElement("HookWS1_RecvFrom", PacketConfig.Packet.HookWS1_RecvFrom),
                        new XElement("HookWS2_Send", PacketConfig.Packet.HookWS2_Send),
                        new XElement("HookWS2_SendTo", PacketConfig.Packet.HookWS2_SendTo),
                        new XElement("HookWS2_Recv", PacketConfig.Packet.HookWS2_Recv),
                        new XElement("HookWS2_RecvFrom", PacketConfig.Packet.HookWS2_RecvFrom),
                        new XElement("HookWSA_Send", PacketConfig.Packet.HookWSA_Send),
                        new XElement("HookWSA_SendTo", PacketConfig.Packet.HookWSA_SendTo),
                        new XElement("HookWSA_Recv", PacketConfig.Packet.HookWSA_Recv),
                        new XElement("HookWSA_RecvFrom", PacketConfig.Packet.HookWSA_RecvFrom),                        
                        new XElement("PacketList_AutoRoll", PacketConfig.List.AutoRoll),
                        new XElement("PacketList_AutoClear", PacketConfig.List.AutoClear),
                        new XElement("PacketList_AutoClear_Value", PacketConfig.List.AutoClear_Value)
                        );

                    return xeInjectMode;
                }
                catch (Exception ex)
                {
                    DoLog(nameof(GetInjectMode_XML), ex.Message);
                }

                return null;
            }

            #endregion

            #region//从数据库加载注入模式配置

            public static void LoadInjectMode_FromDB()
            {
                try
                {
                    DataTable InjectMode = DataBase.SelectTable_InjectMode();

                    if (InjectMode.Rows.Count > 0)
                    {
                        PacketConfig.Packet.HookWS1_Send = Convert.ToBoolean(InjectMode.Rows[0]["HookWS1_Send"]);
                        PacketConfig.Packet.HookWS1_SendTo = Convert.ToBoolean(InjectMode.Rows[0]["HookWS1_SendTo"]);
                        PacketConfig.Packet.HookWS1_Recv = Convert.ToBoolean(InjectMode.Rows[0]["HookWS1_Recv"]);
                        PacketConfig.Packet.HookWS1_RecvFrom = Convert.ToBoolean(InjectMode.Rows[0]["HookWS1_RecvFrom"]);
                        PacketConfig.Packet.HookWS2_Send = Convert.ToBoolean(InjectMode.Rows[0]["HookWS2_Send"]);
                        PacketConfig.Packet.HookWS2_SendTo = Convert.ToBoolean(InjectMode.Rows[0]["HookWS2_SendTo"]);
                        PacketConfig.Packet.HookWS2_Recv = Convert.ToBoolean(InjectMode.Rows[0]["HookWS2_Recv"]);
                        PacketConfig.Packet.HookWS2_RecvFrom = Convert.ToBoolean(InjectMode.Rows[0]["HookWS2_RecvFrom"]);
                        PacketConfig.Packet.HookWSA_Send = Convert.ToBoolean(InjectMode.Rows[0]["HookWSA_Send"]);
                        PacketConfig.Packet.HookWSA_SendTo = Convert.ToBoolean(InjectMode.Rows[0]["HookWSA_SendTo"]);
                        PacketConfig.Packet.HookWSA_Recv = Convert.ToBoolean(InjectMode.Rows[0]["HookWSA_Recv"]);
                        PacketConfig.Packet.HookWSA_RecvFrom = Convert.ToBoolean(InjectMode.Rows[0]["HookWSA_RecvFrom"]);
                        
                        PacketConfig.List.AutoRoll = Convert.ToBoolean(InjectMode.Rows[0]["PacketList_AutoRoll"]);
                        PacketConfig.List.AutoClear = Convert.ToBoolean(InjectMode.Rows[0]["PacketList_AutoClear"]);
                        PacketConfig.List.AutoClear_Value = Convert.ToInt32(InjectMode.Rows[0]["PacketList_AutoClear_Value"]);                  
                    }
                }
                catch (Exception ex)
                {
                    DoLog(nameof(LoadInjectMode_FromDB), ex.Message);
                }
            }            

            public static void SetInjectMode_FromXML(XElement xeInjectMode)
            {
                try
                {
                    XElement HookWS1_Send = xeInjectMode.Element("HookWS1_Send");
                    if (HookWS1_Send != null)
                    {
                        PacketConfig.Packet.HookWS1_Send = Convert.ToBoolean(HookWS1_Send.Value);
                    }

                    XElement HookWS1_SendTo = xeInjectMode.Element("HookWS1_SendTo");
                    if (HookWS1_SendTo != null)
                    {
                        PacketConfig.Packet.HookWS1_SendTo = Convert.ToBoolean(HookWS1_SendTo.Value);
                    }

                    XElement HookWS1_Recv = xeInjectMode.Element("HookWS1_Recv");
                    if (HookWS1_Recv != null)
                    {
                        PacketConfig.Packet.HookWS1_Recv = Convert.ToBoolean(HookWS1_Recv.Value);
                    }

                    XElement HookWS1_RecvFrom = xeInjectMode.Element("HookWS1_RecvFrom");
                    if (HookWS1_RecvFrom != null)
                    {
                        PacketConfig.Packet.HookWS1_RecvFrom = Convert.ToBoolean(HookWS1_RecvFrom.Value);
                    }

                    XElement HookWS2_Send = xeInjectMode.Element("HookWS2_Send");
                    if (HookWS2_Send != null)
                    {
                        PacketConfig.Packet.HookWS2_Send = Convert.ToBoolean(HookWS2_Send.Value);
                    }

                    XElement HookWS2_SendTo = xeInjectMode.Element("HookWS2_SendTo");
                    if (HookWS2_SendTo != null)
                    {
                        PacketConfig.Packet.HookWS2_SendTo = Convert.ToBoolean(HookWS2_SendTo.Value);
                    }

                    XElement HookWS2_Recv = xeInjectMode.Element("HookWS2_Recv");
                    if (HookWS2_Recv != null)
                    {
                        PacketConfig.Packet.HookWS2_Recv = Convert.ToBoolean(HookWS2_Recv.Value);
                    }

                    XElement HookWS2_RecvFrom = xeInjectMode.Element("HookWS2_RecvFrom");
                    if (HookWS2_RecvFrom != null)
                    {
                        PacketConfig.Packet.HookWS2_RecvFrom = Convert.ToBoolean(HookWS2_RecvFrom.Value);
                    }

                    XElement HookWSA_Send = xeInjectMode.Element("HookWSA_Send");
                    if (HookWSA_Send != null)
                    {
                        PacketConfig.Packet.HookWSA_Send = Convert.ToBoolean(HookWSA_Send.Value);
                    }

                    XElement HookWSA_SendTo = xeInjectMode.Element("HookWSA_SendTo");
                    if (HookWSA_SendTo != null)
                    {
                        PacketConfig.Packet.HookWSA_SendTo = Convert.ToBoolean(HookWSA_SendTo.Value);
                    }

                    XElement HookWSA_Recv = xeInjectMode.Element("HookWSA_Recv");
                    if (HookWSA_Recv != null)
                    {
                        PacketConfig.Packet.HookWSA_Recv = Convert.ToBoolean(HookWSA_Recv.Value);
                    }

                    XElement HookWSA_RecvFrom = xeInjectMode.Element("HookWSA_RecvFrom");
                    if (HookWSA_RecvFrom != null)
                    {
                        PacketConfig.Packet.HookWSA_RecvFrom = Convert.ToBoolean(HookWSA_RecvFrom.Value);
                    }                    

                    XElement xePacketList_AutoRoll = xeInjectMode.Element("PacketList_AutoRoll");
                    if (xePacketList_AutoRoll != null)
                    {
                        PacketConfig.List.AutoRoll = Convert.ToBoolean(xePacketList_AutoRoll.Value);
                    }

                    XElement xePacketList_AutoClear = xeInjectMode.Element("PacketList_AutoClear");
                    if (xePacketList_AutoClear != null)
                    {
                        PacketConfig.List.AutoClear = Convert.ToBoolean(xePacketList_AutoClear.Value);
                    }

                    XElement xePacketList_AutoClear_Value = xeInjectMode.Element("PacketList_AutoClear_Value");
                    if (xePacketList_AutoClear_Value != null)
                    {
                        PacketConfig.List.AutoClear_Value = int.Parse(xePacketList_AutoClear_Value.Value);
                    }              
                }
                catch (Exception ex)
                {
                    DoLog(nameof(SetInjectMode_FromXML), ex.Message);
                }
            }

            #endregion

            #region//保存代理模式配置到数据库

            public static void SaveProxyMode_ToDB()
            {
                DataBase.DeleteTable_ProxyMode();
                DataBase.InsertTable_ProxyMode();
            }

            public static XElement GetProxyMode_XML()
            {
                try
                {
                    XElement xeProxyMode =
                        new XElement("ProxyMode",
                        new XElement("ProxyIP_Auto", ProxyConfig.Proxy.ProxyIP_Auto),
                        new XElement("Enable_SOCKS5", ProxyConfig.Proxy.Enable_SOCKS5),
                        new XElement("ProxyIP", ProxyConfig.Proxy.ProxyIP),
                        new XElement("ProxyPort", ProxyConfig.Proxy.ProxyPort),                        
                        new XElement("Enable_Auth", ProxyConfig.Proxy.Enable_Auth),
                        new XElement("MaxConnectionNumber", ProxyConfig.Proxy.MaxConnectionNumber),
                        new XElement("Enable_MapLocal", ProxyConfig.Mapping.Enable_MapLocal),
                        new XElement("Enable_MapRemote", ProxyConfig.Mapping.Enable_MapRemote),
                        new XElement("Enable_ExternalProxy", ProxyConfig.Proxy.Enable_ExternalProxy),
                        new XElement("ExternalProxy_IP", ProxyConfig.Proxy.ExternalProxy_IP),
                        new XElement("ExternalProxy_Port", ProxyConfig.Proxy.ExternalProxy_Port),
                        new XElement("Enable_ExternalProxy_AppointPort", ProxyConfig.Proxy.Enable_ExternalProxy_AppointPort),
                        new XElement("ExternalProxy_AppointPort", ProxyConfig.Proxy.ExternalProxy_AppointPort),
                        new XElement("Enable_ExternalProxy_Auth", ProxyConfig.Proxy.Enable_ExternalProxy_Auth),
                        new XElement("ExternalProxy_UserName", ProxyConfig.Proxy.ExternalProxy_UserName),
                        new XElement("ExternalProxy_PassWord", ProxyConfig.Proxy.ExternalProxy_PassWord),
                        new XElement("EnableFireWall", ProxyConfig.Proxy.EnableFireWall),
                        new XElement("WhiteListMode", ProxyConfig.Proxy.WhiteListMode),
                        new XElement("FireWall_AutoBlock_UnSupport", ProxyConfig.Proxy.FireWall_AutoBlock_UnSupport),
                        new XElement("FireWall_AutoBlock_Minutes", ProxyConfig.Proxy.FireWall_AutoBlock_Minutes),
                        new XElement("FireWall_AutoClear_Expiry", ProxyConfig.Proxy.FireWall_AutoClear_Expiry)
                        );

                    return xeProxyMode;
                }
                catch (Exception ex)
                {
                    DoLog(nameof(GetProxyMode_XML), ex.Message);
                }

                return null;
            }

            #endregion

            #region//从数据库加载代理模式配置

            public static void LoadProxyMode_FromDB()
            {
                try
                {
                    DataTable ProxyMode = DataBase.SelectTable_ProxyMode();

                    if (ProxyMode.Rows.Count > 0)
                    {
                        ProxyConfig.Proxy.ProxyIP_Auto = Convert.ToBoolean(ProxyMode.Rows[0]["ProxyIP_Auto"]);
                        ProxyConfig.Proxy.Enable_SOCKS5 = Convert.ToBoolean(ProxyMode.Rows[0]["EnableSOCKS5"]);
                        ProxyConfig.Proxy.ProxyIP = ProxyMode.Rows[0]["ProxyIP"].ToString();
                        ProxyConfig.Proxy.ProxyPort = ushort.Parse(ProxyMode.Rows[0]["ProxyPort"].ToString());                        
                        ProxyConfig.Proxy.Enable_Auth = Convert.ToBoolean(ProxyMode.Rows[0]["EnableAuth"]);
                        ProxyConfig.Proxy.MaxConnectionNumber = Convert.ToInt32(ProxyMode.Rows[0]["MaxConnectionNumber"].ToString());
                        ProxyConfig.Mapping.Enable_MapLocal = Convert.ToBoolean(ProxyMode.Rows[0]["Enable_MapLocal"]);
                        ProxyConfig.Mapping.Enable_MapRemote = Convert.ToBoolean(ProxyMode.Rows[0]["Enable_MapRemote"]);
                        ProxyConfig.Proxy.Enable_ExternalProxy = Convert.ToBoolean(ProxyMode.Rows[0]["Enable_ExternalProxy"]);
                        ProxyConfig.Proxy.ExternalProxy_IP = ProxyMode.Rows[0]["ExternalProxy_IP"].ToString();
                        ProxyConfig.Proxy.ExternalProxy_Port = ushort.Parse(ProxyMode.Rows[0]["ExternalProxy_Port"].ToString());
                        ProxyConfig.Proxy.Enable_ExternalProxy_AppointPort = Convert.ToBoolean(ProxyMode.Rows[0]["Enable_ExternalProxy_AppointPort"]);
                        ProxyConfig.Proxy.ExternalProxy_AppointPort = ProxyMode.Rows[0]["ExternalProxy_AppointPort"].ToString();
                        ProxyConfig.Proxy.Enable_ExternalProxy_Auth = Convert.ToBoolean(ProxyMode.Rows[0]["Enable_ExternalProxy_Auth"]);
                        ProxyConfig.Proxy.ExternalProxy_UserName = ProxyMode.Rows[0]["ExternalProxy_UserName"].ToString();
                        ProxyConfig.Proxy.ExternalProxy_PassWord = ProxyMode.Rows[0]["ExternalProxy_PassWord"].ToString();
                        ProxyConfig.Proxy.EnableFireWall = Convert.ToBoolean(ProxyMode.Rows[0]["EnableFireWall"]);
                        ProxyConfig.Proxy.WhiteListMode = Convert.ToBoolean(ProxyMode.Rows[0]["WhiteListMode"]);
                        ProxyConfig.Proxy.FireWall_AutoBlock_UnSupport = Convert.ToBoolean(ProxyMode.Rows[0]["FireWall_AutoBlock_UnSupport"]);
                        ProxyConfig.Proxy.FireWall_AutoBlock_Minutes = Convert.ToInt32(ProxyMode.Rows[0]["FireWall_AutoBlock_Minutes"].ToString());
                        ProxyConfig.Proxy.FireWall_AutoClear_Expiry = Convert.ToBoolean(ProxyMode.Rows[0]["FireWall_AutoClear_Expiry"]);
                    }
                }
                catch (Exception ex)
                {
                    DoLog(nameof(LoadProxyMode_FromDB), ex.Message);
                }
            }

            public static void SetProxyMode_FromXML(XElement xeProxyMode)
            {
                try
                {
                    XElement ProxyIP_Auto = xeProxyMode.Element("ProxyIP_Auto");
                    if (ProxyIP_Auto != null)
                    {
                        ProxyConfig.Proxy.ProxyIP_Auto = Convert.ToBoolean(ProxyIP_Auto.Value);
                    }

                    XElement Enable_SOCKS5 = xeProxyMode.Element("Enable_SOCKS5");
                    if (Enable_SOCKS5 != null)
                    {
                        ProxyConfig.Proxy.Enable_SOCKS5 = Convert.ToBoolean(Enable_SOCKS5.Value);
                    }

                    XElement ProxyIP = xeProxyMode.Element("ProxyIP");
                    if (ProxyIP != null)
                    {
                        ProxyConfig.Proxy.ProxyIP = ProxyIP.Value;
                    }

                    XElement ProxyPort = xeProxyMode.Element("ProxyPort");
                    if (ProxyPort != null)
                    {
                        ProxyConfig.Proxy.ProxyPort = ushort.Parse(ProxyPort.Value);
                    }

                    XElement Enable_Auth = xeProxyMode.Element("Enable_Auth");
                    if (Enable_Auth != null)
                    {
                        ProxyConfig.Proxy.Enable_Auth = Convert.ToBoolean(Enable_Auth.Value);
                    }

                    XElement Enable_MapLocal = xeProxyMode.Element("Enable_MapLocal");
                    if (Enable_MapLocal != null)
                    {
                        ProxyConfig.Mapping.Enable_MapLocal = Convert.ToBoolean(Enable_MapLocal.Value);
                    }

                    XElement Enable_MapRemote = xeProxyMode.Element("Enable_MapRemote");
                    if (Enable_MapRemote != null)
                    {
                        ProxyConfig.Mapping.Enable_MapRemote = Convert.ToBoolean(Enable_MapRemote.Value);
                    }

                    XElement Enable_ExternalProxy = xeProxyMode.Element("Enable_ExternalProxy");
                    if (Enable_ExternalProxy != null)
                    {
                        ProxyConfig.Proxy.Enable_ExternalProxy = Convert.ToBoolean(Enable_ExternalProxy.Value);
                    }

                    XElement ExternalProxy_IP = xeProxyMode.Element("ExternalProxy_IP");
                    if (ExternalProxy_IP != null)
                    {
                        ProxyConfig.Proxy.ExternalProxy_IP = ExternalProxy_IP.Value;
                    }

                    XElement ExternalProxy_Port = xeProxyMode.Element("ExternalProxy_Port");
                    if (ExternalProxy_Port != null)
                    {
                        ProxyConfig.Proxy.ExternalProxy_Port = ushort.Parse(ExternalProxy_Port.Value);
                    }

                    XElement Enable_ExternalProxy_AppointPort = xeProxyMode.Element("Enable_ExternalProxy_AppointPort");
                    if (Enable_ExternalProxy_AppointPort != null)
                    {
                        ProxyConfig.Proxy.Enable_ExternalProxy_AppointPort = Convert.ToBoolean(Enable_ExternalProxy_AppointPort.Value);
                    }

                    XElement ExternalProxy_AppointPort = xeProxyMode.Element("ExternalProxy_AppointPort");
                    if (ExternalProxy_AppointPort != null)
                    {
                        ProxyConfig.Proxy.ExternalProxy_AppointPort = ExternalProxy_AppointPort.Value;
                    }

                    XElement Enable_ExternalProxy_Auth = xeProxyMode.Element("Enable_ExternalProxy_Auth");
                    if (Enable_ExternalProxy_Auth != null)
                    {
                        ProxyConfig.Proxy.Enable_ExternalProxy_Auth = Convert.ToBoolean(Enable_ExternalProxy_Auth.Value);
                    }

                    XElement ExternalProxy_UserName = xeProxyMode.Element("ExternalProxy_UserName");
                    if (ExternalProxy_UserName != null)
                    {
                        ProxyConfig.Proxy.ExternalProxy_UserName = ExternalProxy_UserName.Value;
                    }

                    XElement ExternalProxy_PassWord = xeProxyMode.Element("ExternalProxy_PassWord");
                    if (ExternalProxy_PassWord != null)
                    {
                        ProxyConfig.Proxy.ExternalProxy_PassWord = ExternalProxy_PassWord.Value;
                    }

                    XElement EnableFireWall = xeProxyMode.Element("EnableFireWall");
                    if (EnableFireWall != null)
                    {
                        ProxyConfig.Proxy.EnableFireWall = Convert.ToBoolean(EnableFireWall.Value);
                    }

                    XElement WhiteListMode = xeProxyMode.Element("WhiteListMode");
                    if (WhiteListMode != null)
                    {
                        ProxyConfig.Proxy.WhiteListMode = Convert.ToBoolean(WhiteListMode.Value);
                    }

                    XElement FireWall_AutoBlock_UnSupport = xeProxyMode.Element("FireWall_AutoBlock_UnSupport");
                    if (FireWall_AutoBlock_UnSupport != null)
                    {
                        ProxyConfig.Proxy.FireWall_AutoBlock_UnSupport = Convert.ToBoolean(FireWall_AutoBlock_UnSupport.Value);
                    }

                    XElement FireWall_AutoBlock_Minutes = xeProxyMode.Element("FireWall_AutoBlock_Minutes");
                    if (FireWall_AutoBlock_Minutes != null)
                    {
                        ProxyConfig.Proxy.FireWall_AutoBlock_Minutes = int.Parse(FireWall_AutoBlock_Minutes.Value);
                    }

                    XElement FireWall_AutoClear_Expiry = xeProxyMode.Element("FireWall_AutoClear_Expiry");
                    if (FireWall_AutoClear_Expiry != null)
                    {
                        ProxyConfig.Proxy.FireWall_AutoClear_Expiry = Convert.ToBoolean(FireWall_AutoClear_Expiry.Value);
                    }
                }
                catch (Exception ex)
                {
                    DoLog(nameof(SetProxyMode_FromXML), ex.Message);
                }
            }

            #endregion            

            #region//保存系统列表到数据库

            public static void SaveSystemList_ToDB()
            {
                try
                {
                    FilterConfig.List.SaveFilterList_ToDB();
                    SendConfig.List.SaveSendList_ToDB();
                    RobotConfig.List.SaveRobotList_ToDB();
                }
                catch (Exception ex)
                {
                    DoLog(nameof(LoadSystemList_FromDB), ex.Message);
                }                
            }

            #endregion

            #region//从数据库加载系统列表

            public static void LoadSystemList_FromDB()
            {
                try
                {
                    FilterConfig.List.LoadFilterList_FromDB();
                    SendConfig.List.LoadSendList_FromDB();
                    RobotConfig.List.LoadRobotList_FromDB();

                    string DBFilePath = string.Format("{0}\\{1}", DataBase.dbPath, DataBase.dbName);
                    Operate.DoLog(nameof(LoadSystemList_FromDB), AntdUI.Localization.Get("StartForm.Database.Loaded", "已加载数据库 : ") + DBFilePath);
                }
                catch (Exception ex)
                {
                    DoLog(nameof(LoadSystemList_FromDB), ex.Message);
                }
            }

            #endregion

            #region//导出系统备份到文件（对话框）

            public static void ExportSystemBackUp_Dialog(
                Form form,
                string FileName,
                bool bSystemConfig,
                bool bProxySet,
                bool bProxyAccount,
                bool bWhiteList,
                bool bBlackList,
                bool bProxyMapping,
                bool bInjectionSet,
                bool bFilterList,
                bool bSendList,
                bool bRobotList)
            {
                try
                {
                    SaveFileDialog sfdSaveFile = new SaveFileDialog();
                    sfdSaveFile.Filter = "WPE x64（*.sb）|*.sb";

                    if (!string.IsNullOrEmpty(FileName))
                    {
                        sfdSaveFile.FileName = FileName;
                    }
                    sfdSaveFile.RestoreDirectory = true;

                    if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = sfdSaveFile.FileName;
                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("BackUpSettingsForm.Export", "导出系统备份"));

                            bool bOK = SystemConfig.ExportSystemBackUp(
                                FilePath,
                                bSystemConfig,
                                bProxySet,
                                bProxyAccount,
                                bWhiteList,
                                bBlackList,
                                bProxyMapping,
                                bInjectionSet,
                                bFilterList,
                                bSendList,
                                bRobotList,
                                EncryptPassword.DoEncrypt,
                                EncryptPassword.Password);

                            if (bOK)
                            {
                                string Title = AntdUI.Localization.Get("BackUpSettingsForm.Export.Success", "导出系统备份成功");
                                AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                Operate.DoLog(nameof(ExportSystemBackUp_Dialog), Title + ": " + FilePath);
                            }
                            else
                            {
                                string Title = AntdUI.Localization.Get("BackUpSettingsForm.Export.Fail", "导出系统备份失败");
                                string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(ExportSystemBackUp_Dialog), ex.Message);
                }
            }

            private static bool ExportSystemBackUp(
                string FilePath,
                bool bSystemConfig,
                bool bProxySet,
                bool bProxyAccount,
                bool bWhiteList,
                bool bBlackList,
                bool bProxyMapping,
                bool bInjectionSet,
                bool bFilterList,
                bool bSendList,
                bool bRobotList,
                bool DoEncrypt,
                string Password)
            {
                try
                {
                    XDocument xdoc = new XDocument
                    {
                        Declaration = new XDeclaration("1.0", "utf-8", "yes")
                    };

                    XElement xeBackUp = new XElement("WPE64_BackUp");

                    //系统设置
                    if (bSystemConfig)
                    {
                        XElement xeSystemConfig = SystemConfig.GetSystemConfig_XML();
                        if (xeSystemConfig != null)
                        {
                            xeBackUp.Add(xeSystemConfig);
                        }
                    }

                    //代理设置
                    if (bProxySet)
                    {
                        XElement xeProxyConfig = SystemConfig.GetProxyMode_XML();
                        if (xeProxyConfig != null)
                        {
                            xeBackUp.Add(xeProxyConfig);
                        }
                    }

                    //代理账号
                    if (bProxyAccount)
                    {
                        if (ProxyConfig.Account.lstAccountInfo.Count > 0)
                        {
                            XElement xeProxyAccount = ProxyConfig.Account.GetAccountList_XML(ProxyConfig.Account.lstAccountInfo.ToList());
                            if (xeProxyAccount != null)
                            {
                                xeBackUp.Add(xeProxyAccount);
                            }
                        }
                    }

                    //白名单
                    if (bWhiteList)
                    {
                        if (ProxyConfig.Proxy.lstWhiteList.Count > 0)
                        {
                            XElement xeWhiteList = ProxyConfig.Proxy.GetWhiteList_XML(ProxyConfig.Proxy.lstWhiteList);
                            if (xeWhiteList != null)
                            {
                                xeBackUp.Add(xeWhiteList);
                            }
                        }
                    }

                    //黑名单
                    if (bBlackList)
                    {
                        if (ProxyConfig.Proxy.lstBlackList.Count > 0)
                        {
                            XElement xeBlackList = ProxyConfig.Proxy.GetBlackList_XML(ProxyConfig.Proxy.lstBlackList);
                            if (xeBlackList != null)
                            {
                                xeBackUp.Add(xeBlackList);
                            }
                        }
                    }

                    //代理映射
                    if (bProxyMapping)
                    {
                        //本地映射
                        if (ProxyConfig.Mapping.lstMapLocal.Count > 0)
                        {
                            XElement xeMapLocal = ProxyConfig.Mapping.GetMapLocal_XML(ProxyConfig.Mapping.lstMapLocal);
                            if (xeMapLocal != null)
                            {
                                xeBackUp.Add(xeMapLocal);
                            }
                        }
                    }

                    //注入设置
                    if (bInjectionSet)
                    {
                        XElement xeInjectionConfig = SystemConfig.GetInjectMode_XML();
                        if (xeInjectionConfig != null)
                        {
                            xeBackUp.Add(xeInjectionConfig);
                        }
                    }

                    //滤镜列表
                    if (bFilterList)
                    {
                        if (FilterConfig.List.lstFilterInfo.Count > 0)
                        {
                            XElement xeFilterList = FilterConfig.List.GetFilterList_XML(FilterConfig.List.lstFilterInfo.ToList());
                            if (xeFilterList != null)
                            {
                                xeBackUp.Add(xeFilterList);
                            }
                        }
                    }

                    //发送列表
                    if (bSendList)
                    {
                        if (SendConfig.List.lstSendInfo.Count > 0)
                        {
                            XElement xeSendList = SendConfig.List.GetSendList_XML(SendConfig.List.lstSendInfo.ToList());
                            if (xeSendList != null)
                            {
                                xeBackUp.Add(xeSendList);
                            }
                        }
                    }

                    //机器人列表
                    if (bRobotList)
                    {
                        if (RobotConfig.List.lstRobotInfo.Count > 0)
                        {
                            XElement xeRobotList = RobotConfig.List.GetRobotList_XML(RobotConfig.List.lstRobotInfo.ToList());
                            if (xeRobotList != null)
                            {
                                xeBackUp.Add(xeRobotList);
                            }
                        }
                    }

                    xdoc.Add(xeBackUp);
                    xdoc.Save(FilePath);

                    if (DoEncrypt)
                    {
                        if (!string.IsNullOrEmpty(Password))
                        {
                            SystemConfig.EncryptXMLFile(FilePath, Password);
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(ExportSystemBackUp), ex.Message);
                }

                return false;
            }

            #endregion

            #region//从文件导入系统备份（对话框）

            public static void ImportSystemBackUp_Dialog(Form form)
            {
                try
                {
                    OpenFileDialog ofdLoadFile = new OpenFileDialog();
                    ofdLoadFile.Filter = "WPE x64（*.sb）|*.sb";
                    ofdLoadFile.RestoreDirectory = true;

                    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = ofdLoadFile.FileName;
                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            if (ImportSystemBackUp(form, FilePath, true))
                            {
                                string Title = AntdUI.Localization.Get("BackUpSettingsForm.Import.Success", "导入系统备份成功");
                                AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                Operate.DoLog(nameof(ImportSystemBackUp_Dialog), Title + ": " + FilePath);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(ImportSystemBackUp_Dialog), ex.Message);
                }
            }

            private static bool ImportSystemBackUp(Form form, string FilePath, bool LoadFromUser)
            {
                try
                {
                    if (File.Exists(FilePath))
                    {
                        XDocument xdoc = null;

                        bool bEncrypt = IsEncryptXMLFile(FilePath);
                        if (bEncrypt)
                        {
                            if (LoadFromUser)
                            {
                                xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("BackUpSettingsForm.Import", "导入系统备份"), FilePath);
                            }
                        }
                        else
                        {
                            xdoc = XDocument.Load(FilePath);
                        }

                        if (xdoc == null)
                        {
                            string sError = AntdUI.Localization.Get("Password.Incorrect", "密码错误");
                            if (LoadFromUser)
                            {
                                AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                            }
                            else
                            {
                                Operate.DoLog(nameof(ImportSystemBackUp), sError);
                            }

                            return false;
                        }

                        ImportSystemBackUp_FromXDocument(form, xdoc);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(ImportSystemBackUp), ex.Message);
                }

                return false;
            }

            private static void ImportSystemBackUp_FromXDocument(Form form, XDocument xdoc)
            {
                #region//有效性检测

                string RootName = xdoc.Root.Name.LocalName;
                if (!RootName.Equals("WPE64_BackUp"))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(form, "备份文件错误", TType.Error)
                    {
                        LocalizationText = "SystemBackUp.Error"
                    });

                    return;
                }

                #endregion

                #region//系统设置

                try
                {
                    XElement xeSystemConfig = xdoc.Root.Element("SystemConfig");
                    if (xeSystemConfig != null)
                    {
                        SetSystemConfig_FromXML(xeSystemConfig);                        
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import SystemConfig", ex.Message);
                }

                #endregion

                #region//代理模式配置

                try
                {
                    XElement xeProxyMode = xdoc.Root.Element("ProxyMode");
                    if (xeProxyMode != null)
                    {
                        SetProxyMode_FromXML(xeProxyMode);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import ProxyMode", ex.Message);
                }

                #endregion

                #region//注入模式配置

                try
                {
                    XElement xeInjectMode = xdoc.Root.Element("InjectMode");
                    if (xeInjectMode != null)
                    {
                        SetInjectMode_FromXML(xeInjectMode);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import InjectMode", ex.Message);
                }

                #endregion

                #region//代理账号

                try
                {
                    XElement xeProxyAccountList = xdoc.Root.Element("ProxyAccountList");
                    if (xeProxyAccountList != null)
                    {
                        XDocument ProxyAccountList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        ProxyAccountList.Add(xeProxyAccountList);

                        ProxyConfig.Account.AccountListClear();
                        ProxyConfig.Account.LoadAccountList_FromXDocument(ProxyAccountList);

                        if (form is InterfaceInfo.IProxyMode pmForm)
                        {
                            pmForm.RefreshAccountList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import ProxyAccountList", ex.Message);
                }

                #endregion

                #region//白名单

                try
                {
                    XElement xeWhiteList = xdoc.Root.Element("WhiteList");
                    if (xeWhiteList != null)
                    {
                        XDocument WhiteList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        WhiteList.Add(xeWhiteList);

                        ProxyConfig.Proxy.CleanUpWhiteList();
                        ProxyConfig.Proxy.LoadWhiteList_FromXDocument(WhiteList);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import WhiteList", ex.Message);
                }

                #endregion

                #region//黑名单

                try
                {
                    XElement xeBlackList = xdoc.Root.Element("BlackList");
                    if (xeBlackList != null)
                    {
                        XDocument BlackList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        BlackList.Add(xeBlackList);

                        ProxyConfig.Proxy.CleanUpBlackList();
                        ProxyConfig.Proxy.LoadBlackList_FromXDocument(BlackList);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import BlackList", ex.Message);
                }

                #endregion

                #region//代理映射

                try
                {
                    //本地代理映射
                    XElement xeMapLocal = xdoc.Root.Element("MapLocal");
                    if (xeMapLocal != null)
                    {
                        XDocument MapLocal = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        MapLocal.Add(xeMapLocal);

                        ProxyConfig.Mapping.LoadMapLocal_FromXDocument(MapLocal);
                    }

                    //远程代理映射
                    XElement xeMapRemote = xdoc.Root.Element("MapRemote");
                    if (xeMapRemote != null)
                    {
                        XDocument MapRemote = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        MapRemote.Add(xeMapRemote);

                        ProxyConfig.Mapping.LoadMapRemote_FromXDocument(MapRemote);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import ProxyAccountList", ex.Message);
                }

                #endregion

                #region//滤镜列表

                try
                {
                    XElement xeFilterList = xdoc.Root.Element("FilterList");
                    if (xeFilterList != null)
                    {
                        XDocument xdFilterList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        xdFilterList.Add(xeFilterList);

                        FilterConfig.List.FilterListClear();
                        FilterConfig.List.LoadFilterList_FromXDocument(xdFilterList);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import FilterList", ex.Message);
                }

                #endregion

                #region//发送列表

                try
                {
                    XElement xeSendList = xdoc.Root.Element("SendList");
                    if (xeSendList != null)
                    {
                        XDocument xdSendList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        xdSendList.Add(xeSendList);

                        SendConfig.List.SendListClear();
                        SendConfig.List.LoadSendList_FromXDocument(xdSendList);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import SendList", ex.Message);
                }

                #endregion

                #region//机器人列表

                try
                {
                    XElement xeRobotList = xdoc.Root.Element("RobotList");
                    if (xeRobotList != null)
                    {
                        XDocument xdRobotList = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };
                        xdRobotList.Add(xeRobotList);

                        RobotConfig.List.RobotListClear();
                        RobotConfig.List.LoadRobotList_FromXDocument(xdRobotList);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import RobotList", ex.Message);
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region//进程配置

        public static class ProcessConfig
        {
            #region//获取进程列表

            public static List<ProcessInfo> GetProcessList()
            {
                List<ProcessInfo> piReturn = new List<ProcessInfo>();

                try
                {
                    Process[] procesArr = Process.GetProcesses();
                    int pCNT = procesArr.Length;

                    foreach (Process p in procesArr)
                    {
                        Image ICO = IconFromFile(p);
                        string ProcessPath = GetProcessPath(p);

                        ProcessInfo processInfo = new ProcessInfo(ICO, p.ProcessName, p.Id, ProcessPath);
                        piReturn.Add(processInfo);
                    }

                    piReturn.Sort((x, y) => x.ProcessName.CompareTo(y.ProcessName));
                }
                catch (Exception ex)
                {
                    DoLog(nameof(GetProcessList), ex.Message);
                }

                return piReturn;
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
                    var extractor = new IconExtractor(filePath);
                    var icon = extractor.GetIcon(0);
                    if (icon != null)
                    {
                        var splitIcons = IconUtil.Split(icon);
                        return GetBestIcon(splitIcons);
                    }
                }
                catch
                {
                    //
                }

                try
                {
                    return Icon.ExtractAssociatedIcon(filePath)?.ToBitmap();
                }
                catch (Exception ex)
                {
                    DoLog(nameof(IconFromFile), ex.Message);
                }

                return new Icon(SystemIcons.Application, 256, 256).ToBitmap();
            }

            private static string GetFilePath(Process process)
            {
                try
                {
                    return process.MainModule.FileName.Replace(".ni.dll", ".dll");
                }
                catch
                {
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
                    if (IconUtil.GetBitCount(icon) > IconUtil.GetBitCount(bestIcon))
                    {
                        bestIcon = icon;
                    }
                    else if (IconUtil.GetBitCount(icon) == IconUtil.GetBitCount(bestIcon) && icon.Width > bestIcon.Width)
                    {
                        bestIcon = icon;
                    }
                }

                return bestIcon.ToBitmap();
            }

            #endregion

            #region//获取进程的路径

            public static string GetProcessPath(Process process)
            {
                try
                {
                    return process.MainModule.FileName;
                }
                catch
                {
                    return string.Empty;
                }
            }

            #endregion

            #region//获取注入的进程的名称

            public static string GetInjectProcessName()
            {
                string sReturn = string.Empty;

                try
                {
                    Process pProcess = Process.GetCurrentProcess();
                    PacketConfig.Packet.InjectProcess = string.Format("{0} [{1}]", pProcess.ProcessName, pProcess.Id);
                    sReturn = PacketConfig.Packet.InjectProcess;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(GetInjectProcessName), ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//获取注入的模块名称        

            public static string GetInjectModuleName()
            {
                string sReturn = string.Empty;

                try
                {
                    Process pProcess = Process.GetCurrentProcess();

                    if (pProcess.MainWindowHandle != IntPtr.Zero)
                    {
                        if (string.IsNullOrEmpty(pProcess.MainWindowTitle))
                        {
                            sReturn = string.Format(AntdUI.Localization.Get("ProcessInfo", "{0} 句柄: {1}"), pProcess.MainModule.ModuleName, pProcess.MainWindowHandle.ToString());
                        }
                        else
                        {
                            sReturn = string.Format(AntdUI.Localization.Get("ProcessInfo", "{0} 句柄: {1}"), pProcess.MainWindowTitle, pProcess.MainWindowHandle.ToString());
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
                    Operate.DoLog(nameof(GetInjectModuleName), ex.Message);
                }

                return sReturn;
            }

            #endregion

            #region//获取注入的进程的 Winsock 版本信息

            public static string GetInjectWinsockInfo()
            {
                string sReturn = "WinSock";

                try
                {
                    Operate.PacketConfig.Packet.Support_WS1 = false;
                    Operate.PacketConfig.Packet.Support_WS2 = false;

                    foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                    {
                        string sModuleName = module.ModuleName;

                        if (sModuleName.Equals(WSock32.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Operate.PacketConfig.Packet.Support_WS1 = true;
                        }

                        if (sModuleName.Equals(WS2_32.ModuleName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Operate.PacketConfig.Packet.Support_WS2 = true;
                        }

                        if (sModuleName.Equals(Mswsock.ModuleName, StringComparison.CurrentCultureIgnoreCase))
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
                    Operate.DoLog(nameof(GetInjectWinsockInfo), ex.Message);
                }

                return sReturn;
            }

            #endregion
        }

        #endregion

        #region//代理配置

        public static class ProxyConfig
        {
            #region//代理

            public static class Proxy
            {
                public static ProxyAppServer ProxyServer;
                public static IPConnectionFilter ipFilter = new IPConnectionFilter();                
                public static long ProxyTotal_CNT, TCP_Req_CNT, UDP_Req_CNT, TCP_Resp_CNT, UDP_Resp_CNT;
                public static int ProxySpeed_Uplink, ProxySpeed_Downlink;
                public static int FilterProxy_CNT = 0;
                public static IPAddress[] ProxyServerIP = null;
                public static IPAddress ProxyTCP_IP = null, ProxyUDP_IP = null;
                public static bool ProxyIP_Auto = true;
                public static bool Enable_SystemProxy = false;
                public static bool Enable_SOCKS5 = true, Enable_Auth = true;
                public static bool Enable_ExternalProxy = false, Enable_ExternalProxy_AppointPort = false, Enable_ExternalProxy_Auth = false;
                public static string ExternalProxy_IP = "127.0.0.1";
                public static ushort ExternalProxy_Port = 8889;
                public static string ExternalProxy_AppointPort = "80,8080,443,8443", ExternalProxy_UserName, ExternalProxy_PassWord;
                public static int SocketBufferSize = 8192;
                public static string ProxyIP = string.Empty;
                public static ushort ProxyPort = 1080;
                public static int MaxConnectionNumber = 20000;
                public static long Total_Request = 0;
                public static long Total_Response = 0;
                public static string ProxyOnLineInfo = string.Empty;
                public static string ProxyBytesInfo = string.Empty;
                public static string ProxySpeedInfo = string.Empty;
                public static bool HookTCP_Req = true, HookTCP_Resp = true, HookUDP_Req = true, HookUDP_Resp = true;
                private static QQWryOptions IPLib = new QQWryOptions()
                {
                    DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IPLocation", "qqwry.dat")
                };
                public static QQWryIpSearch ipSearch = new QQWryIpSearch(IPLib);

                public static readonly ConcurrentDictionary<string, IPAddress> DnsCache = new ConcurrentDictionary<string, IPAddress>(StringComparer.OrdinalIgnoreCase);
                public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

                public static bool EnableFireWall = false;
                public static bool WhiteListMode = false;
                public static bool FireWall_AutoBlock_UnSupport = false;
                public static int FireWall_AutoBlock_Minutes = 30;
                public static bool FireWall_AutoClear_Expiry = false;
                public static BindingList<BlackListInfo> lstBlackList = new BindingList<BlackListInfo>();
                public static BindingList<WhiteListInfo> lstWhiteList = new BindingList<WhiteListInfo>();

                #region//定义结构                

                public enum ProxyType
                {
                    None = 0,
                    Http = 1,
                    Socket5 = 5,
                    Socket6 = 6,
                }

                public enum ProxyStep : byte
                {
                    Handshake = 0,
                    AuthUserName = 1,
                    Command = 2,
                    ForwardData = 3,
                }

                public enum AuthType : byte
                {
                    None = 0,
                    GSSAPI = 1,
                    UserName = 2,
                }

                public enum AddressType : byte
                {
                    Invalid = 0,
                    IPv4 = 1,
                    Domain = 3,
                    IPv6 = 4,
                }

                public enum DomainType : byte
                {
                    Socket = 0,
                    Http = 1,
                    Https = 2,
                    External = 3,
                }

                public enum MapProtocol : byte
                {
                    Http = 0,
                    Https = 1,
                }

                public enum CommandType : byte
                {
                    Connect = 1,
                    Bind = 2,
                    UDP = 3,
                }

                public enum CommandResponse : byte
                {
                    Success = 0,
                    Fault = 1,
                    Unreachable = 4,
                    Unsupport = 7,
                }

                #endregion

                #region//SocketAsyncEventArgsPool

                public class SocketAsyncEventArgsPool
                {
                    private readonly ConcurrentBag<SocketAsyncEventArgs> _pool;
                    private readonly int _maxSize;

                    public SocketAsyncEventArgsPool(int maxSize = 100)
                    {
                        _maxSize = maxSize;
                        _pool = new ConcurrentBag<SocketAsyncEventArgs>();
                    }

                    public SocketAsyncEventArgs Get()
                    {
                        if (_pool.TryTake(out SocketAsyncEventArgs item))
                        {
                            return item;
                        }
                        return new SocketAsyncEventArgs();
                    }

                    public void Return(SocketAsyncEventArgs item)
                    {
                        if (item == null) return;

                        if (_pool.Count < _maxSize)
                        {
                            _pool.Add(item);
                        }
                        else
                        {
                            item.Dispose();
                        }
                    }

                    public int Count => _pool.Count;
                }

                public static class SocketAsyncEventArgsPoolManager
                {
                    private const int DefaultPoolSize = 5000;

                    private static readonly ConcurrentDictionary<string, SocketAsyncEventArgsPool> _pools = new ConcurrentDictionary<string, SocketAsyncEventArgsPool>();

                    public static SocketAsyncEventArgsPool GetOrCreatePool(string poolName, int maxSize = DefaultPoolSize)
                    {
                        return _pools.GetOrAdd(poolName, name => new SocketAsyncEventArgsPool(maxSize));
                    }

                    public static SocketAsyncEventArgs Get(string poolName = "default")
                    {
                        var pool = GetOrCreatePool(poolName);
                        return pool.Get();
                    }

                    public static void Return(SocketAsyncEventArgs e, string poolName = "default")
                    {
                        if (e == null) return;

                        var pool = GetOrCreatePool(poolName);

                        try
                        {
                            // 清理通用状态
                            if (e.Buffer != null)
                            {
                                ArrayPool<byte>.Shared.Return(e.Buffer);
                                e.SetBuffer(null, 0, 0);
                            }

                            e.UserToken = null;
                            e.RemoteEndPoint = null;
                            e.SocketError = SocketError.Success;

                            pool.Return(e);
                        }
                        catch
                        {
                            e.Dispose();
                        }
                    }                    
                }

                #endregion

                #region//握手过程（异步）                

                public static async Task Handshake(ProxySession psSession, byte[] bData)
                {
                    try
                    {
                        Operate.ProxyConfig.Proxy.ProxyType ptType = (Operate.ProxyConfig.Proxy.ProxyType)bData[0];

                        if (ptType == Operate.ProxyConfig.Proxy.ProxyType.Socket5)
                        {
                            bool bSupportAuthType = false;

                            Operate.ProxyConfig.Proxy.AuthType atServer = Operate.ProxyConfig.Proxy.Enable_Auth
                                ? Operate.ProxyConfig.Proxy.AuthType.UserName
                                : Operate.ProxyConfig.Proxy.AuthType.None;

                            int iMETHODS_COUNT = bData[1];
                            byte[] bMETHODS = new byte[iMETHODS_COUNT];
                            Array.Copy(bData, 2, bMETHODS, 0, iMETHODS_COUNT);

                            foreach (byte method in bMETHODS)
                            {
                                Operate.ProxyConfig.Proxy.AuthType atClient = (Operate.ProxyConfig.Proxy.AuthType)method;

                                if (atServer == atClient)
                                {
                                    bSupportAuthType = true;
                                    break;
                                }
                            }

                            if (bSupportAuthType)
                            {
                                byte[] bAuth = new byte[2];
                                bAuth[0] = (byte)Operate.ProxyConfig.Proxy.ProxyType.Socket5;
                                bAuth[1] = (byte)atServer;
                                psSession.TrySend(bAuth, 0, bAuth.Length);

                                if (atServer == Operate.ProxyConfig.Proxy.AuthType.UserName)
                                {
                                    psSession.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName;

                                    if (bData.Length > iMETHODS_COUNT + 2)
                                    {
                                        byte[] bAuthDate = new byte[bData.Length - (iMETHODS_COUNT + 2)];
                                        Array.Copy(bData, iMETHODS_COUNT + 2, bAuthDate, 0, bAuthDate.Length);

                                        bool bIsMatch = Operate.ProxyConfig.Proxy.CheckDataIsMatchProxyStep(bAuthDate, Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName);
                                        if (bIsMatch)
                                        {
                                            await Operate.ProxyConfig.Proxy.AuthUserName(psSession, bAuthDate);
                                        }
                                    }
                                }
                                else
                                {
                                    psSession.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.Command;
                                }
                            }
                        }
                        else
                        {
                            string sLog = string.Format(AntdUI.Localization.Get("SOCKS.Unsupported", "不支持的 SOCKS 协议版本: {0} [ {1} ]"), ptType, psSession.ClientIP);
                            Operate.DoLog(nameof(Handshake), sLog);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(Handshake), ex.Message);
                    }
                }

                #endregion

                #region//验证账号密码（异步）

                public static async Task AuthUserName(ProxySession psSession, byte[] bData)
                {
                    try
                    {
                        byte VERSION = bData[0];

                        if (VERSION == 0x01)
                        {
                            int USERNAME_LENGTH = bData[1];

                            byte[] USERNAME_BYTES = new byte[USERNAME_LENGTH];
                            Array.Copy(bData, 2, USERNAME_BYTES, 0, USERNAME_LENGTH);

                            int PASSWORD_LENGTH = bData[2 + USERNAME_LENGTH];

                            byte[] PASSWORD_BYTES = new byte[PASSWORD_LENGTH];
                            Array.Copy(bData, 3 + USERNAME_LENGTH, PASSWORD_BYTES, 0, PASSWORD_LENGTH);

                            string sUserName = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, USERNAME_BYTES);
                            string sPassWord = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, PASSWORD_BYTES);

                            byte[] bAuth = new byte[2];
                            bAuth[0] = 0x01;

                            // 第一步：先验证账号密码（异步）
                            var (bAuthOK, AccountID) = Operate.ProxyConfig.Account.CheckUserNameAndPassWord(sUserName, sPassWord);
                            if (!bAuthOK)
                            {
                                bAuth[1] = (byte)0x01;
                                psSession.TrySend(bAuth, 0, bAuth.Length);
                                return;
                            }

                            // 第二步：验证通过后检查连接数限制（异步）
                            bool isOverLinks = Operate.ProxyConfig.Account.CheckLimitLinks(AccountID, psSession.ClientIP);
                            if (isOverLinks)
                            {
                                bAuth[1] = (byte)0x01;
                                psSession.TrySend(bAuth, 0, bAuth.Length);
                                return;
                            }

                            // 第三步：检查设备数限制（异步）
                            bool isOverDevices = Operate.ProxyConfig.Account.CheckLimitDevices(AccountID, psSession.ClientIP);
                            if (isOverDevices)
                            {
                                bAuth[1] = (byte)0x01;
                                psSession.TrySend(bAuth, 0, bAuth.Length);
                                return;
                            }

                            // 最终判断是否允许登录
                            bool isAllowed = bAuthOK && !isOverLinks && !isOverDevices;
                            bAuth[1] = isAllowed ? (byte)0x00 : (byte)0x01;

                            if (isAllowed)
                            {
                                Operate.ProxyConfig.Account.SetOnline_ByAccountID(AccountID, true);
                                await Operate.ProxyConfig.Account.IPInfo_ToAccount(AccountID, psSession.ClientIP);
                                await Operate.ProxyConfig.Account.AuthInfo_ToList(AccountID, psSession.ClientIP, true);

                                psSession.AID = AccountID;
                                psSession.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.Command;
                            }

                            psSession.TrySend(bAuth, 0, bAuth.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AuthUserName), ex.Message);
                    }
                }

                #endregion

                #region//执行命令（异步）

                public static async Task Command(ProxySession psSession, byte[] bData)
                {
                    try
                    {
                        Operate.ProxyConfig.Proxy.ProxyType ptType = (Operate.ProxyConfig.Proxy.ProxyType)bData[0];
                        if (ptType != Operate.ProxyConfig.Proxy.ProxyType.Socket5)
                        {
                            return;
                        }

                        psSession.CommandType = (Operate.ProxyConfig.Proxy.CommandType)bData[1];
                        psSession.AddressType = (Operate.ProxyConfig.Proxy.AddressType)bData[3];

                        byte[] bADDRESS = new byte[bData.Length - 4];
                        Array.Copy(bData, 4, bADDRESS, 0, bADDRESS.Length);

                        var (epServer, TargetAddress) = await Operate.ProxyConfig.Proxy.GetIPEndPoint_ByAddressType(psSession.AddressType, bADDRESS);
                        if (epServer == null)
                        {
                            Operate.ProxyConfig.Proxy.SendCommandResponse(psSession, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Fault);
                            return;
                        }

                        string TargetIP = epServer.Address.ToString();
                        int TargetPort = epServer.Port;

                        psSession.DomainType = Operate.ProxyConfig.Proxy.GetDomainType_ByPort(TargetPort);
                        psSession.ClientAddress = Operate.ProxyConfig.Proxy.GetClientAddress(TargetAddress, TargetPort);

                        switch (psSession.CommandType)
                        {
                            case Operate.ProxyConfig.Proxy.CommandType.Connect:
                                await HandleConnectCommandAsync(psSession, bData, TargetIP, TargetPort, TargetAddress);
                                break;

                            case Operate.ProxyConfig.Proxy.CommandType.UDP:
                                Operate.ProxyConfig.Proxy.UDPRelay(psSession);
                                break;

                            default:
                                HandleUnsupportedCommand(psSession);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(Command), ex.Message);
                    }
                }

                public static async Task HandleConnectCommandAsync(ProxySession psSession, byte[] bData, string targetIP, int targetPort, string targetAddress)
                {
                    switch (psSession.DomainType)
                    {
                        case Operate.ProxyConfig.Proxy.DomainType.External:
                            psSession.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(Operate.ProxyConfig.Proxy.ExternalProxy_IP, Operate.ProxyConfig.Proxy.ExternalProxy_Port);
                            psSession.ConnectToEXTProxyServer(Operate.ProxyConfig.Proxy.ExternalProxy_IP, Operate.ProxyConfig.Proxy.ExternalProxy_Port, bData);
                            break;

                        case Operate.ProxyConfig.Proxy.DomainType.Http:
                            await HandleHttpConnect(psSession, targetIP, targetPort, targetAddress);
                            break;

                        case Operate.ProxyConfig.Proxy.DomainType.Https:
                        case Operate.ProxyConfig.Proxy.DomainType.Socket:
                            psSession.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(targetAddress, targetPort);
                            psSession.ConnectToTarget(targetIP, targetPort);
                            break;
                    }

                    if (!Operate.SystemConfig.SpeedMode)
                    {
                        string ProxyIP = (psSession.SocketSession.Client.LocalEndPoint as IPEndPoint).Address.ToString();
                        Operate.DoProxyLog(psSession.AID, psSession.ClientIP, psSession.ServerAddress, ProxyIP);
                    }
                }

                public static async Task HandleHttpConnect(ProxySession psSession, string targetIP, int targetPort, string targetAddress)
                {
                    psSession.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(targetAddress, targetPort);

                    if (Operate.ProxyConfig.Mapping.Enable_MapLocal || Operate.ProxyConfig.Mapping.Enable_MapRemote)
                    {
                        // 本地代理映射
                        if (Operate.ProxyConfig.Mapping.Enable_MapLocal)
                        {
                            var localRule = Operate.ProxyConfig.Mapping.GetMapLocal(
                                Operate.ProxyConfig.Proxy.MapProtocol.Http,
                                targetAddress,
                                targetPort,
                                string.Empty);

                            if (localRule != null)
                            {
                                psSession.ServerIP = targetAddress;
                                psSession.ServerPort = targetPort;

                                bool fileExists = await Task.Run(() => File.Exists(localRule.LocalPath));
                                if (fileExists)
                                {
                                    Operate.ProxyConfig.Proxy.SendCommandResponse(psSession, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Success);
                                    psSession.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
                                    return;
                                }
                                else
                                {
                                    Operate.ProxyConfig.Proxy.SendCommandResponse(psSession, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unreachable);
                                    return;
                                }
                            }
                        }

                        // 远程代理映射
                        if (Operate.ProxyConfig.Mapping.Enable_MapRemote)
                        {
                            var remoteRule = Operate.ProxyConfig.Mapping.GetMapRemote(
                                Operate.ProxyConfig.Proxy.MapProtocol.Http,
                                targetAddress,
                                targetPort,
                                string.Empty);

                            if (remoteRule != null)
                            {
                                psSession.ConnectToTarget(remoteRule.HostTo, remoteRule.PortTo);
                                return;
                            }
                        }
                    }

                    psSession.ConnectToTarget(targetIP, targetPort);
                }

                public static void HandleUnsupportedCommand(ProxySession psSession)
                {
                    Operate.ProxyConfig.Proxy.SendCommandResponse(psSession, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Unsupport);

                    string sLog = string.Format(AntdUI.Localization.Get("Command.Unsupported", "{0} - 不支持的命令: {1}"), psSession.ClientAddress, psSession.CommandType);
                    Operate.DoLog(nameof(HandleUnsupportedCommand), sLog);
                }

                #endregion

                #region//发送 Command 响应数据

                public static void SendCommandResponse(ProxySession psSession, ProtocolType ProtocolType, Operate.ProxyConfig.Proxy.CommandResponse CommandResponse, int UDPPort = 0)
                {
                    try
                    {
                        ReadOnlySpan<byte> bServerIP = null;
                        ReadOnlySpan<byte> bServerPort = null;

                        switch (ProtocolType)
                        {
                            case ProtocolType.Tcp:

                                bServerIP = Operate.ProxyConfig.Proxy.ProxyTCP_IP.GetAddressBytes();
                                bServerPort = BitConverter.GetBytes(Operate.ProxyConfig.Proxy.ProxyPort);

                                break;

                            case ProtocolType.Udp:

                                bServerIP = Operate.ProxyConfig.Proxy.ProxyUDP_IP.GetAddressBytes();
                                bServerPort = BitConverter.GetBytes(UDPPort);

                                break;
                        }

                        Span<byte> response = stackalloc byte[10];
                        response[0] = (byte)Operate.ProxyConfig.Proxy.ProxyType.Socket5;
                        response[1] = (byte)CommandResponse;
                        response[2] = 0x00;
                        response[3] = (byte)Operate.ProxyConfig.Proxy.AddressType.IPv4;
                        bServerIP.CopyTo(response.Slice(4, 4));
                        response[8] = bServerPort[1];
                        response[9] = bServerPort[0];

                        psSession.TrySend(response.ToArray(), 0, response.Length);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SendCommandResponse), ex.Message);
                    }
                }

                #endregion

                #region//创建新 UDP 监听端口

                public static ProxyUDP CreateNewUDP(string SessionID)
                {
                    try
                    {
                        if (Guid.TryParse(SessionID, out Guid gUDP))
                        {
                            var pu = new ProxyUDP(new IPEndPoint(Operate.ProxyConfig.Proxy.ProxyTCP_IP, 0));
                            ProxyConfig.List.cdProxyUDP.TryAdd(gUDP, pu);

                            pu.UpdateActivity();

                            return pu;
                        }                        
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(CreateNewUDP), ex.Message);
                    }

                    return null;
                }

                public static async Task CheckUDPTimeOutAsync()
                {
                    try
                    {
                        var now = DateTime.Now;
                        var UDPToRemove = new List<Guid>();

                        foreach (var pair in ProxyConfig.List.cdProxyUDP)
                        {
                            if (now - pair.Value.LastActivityTime > ProxyConfig.List.UDPTimeout)
                            {
                                UDPToRemove.Add(pair.Key);
                            }
                        }

                        var closeTasks = UDPToRemove.Select(async UDP =>
                        {
                            if (ProxyConfig.List.cdProxyUDP.TryRemove(UDP, out var udpInstance))
                            {
                                await Task.Run(() => udpInstance.Close());
                            }
                        });

                        await Task.WhenAll(closeTasks).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(CheckUDPTimeOutAsync), ex.Message);
                    }
                }

                #endregion

                #region//处理 TCP 请求数据

                public static void ForwardData(ProxySession psSession, byte[] bData)
                {
                    try
                    {
                        if (psSession.CommandType == Operate.ProxyConfig.Proxy.CommandType.Connect)
                        {
                            switch (psSession.DomainType)
                            {
                                case Operate.ProxyConfig.Proxy.DomainType.Http:

                                    if (Operate.ProxyConfig.Mapping.Enable_MapLocal || Operate.ProxyConfig.Mapping.Enable_MapRemote)
                                    {
                                        string request = Encoding.ASCII.GetString(bData);

                                        if (request.StartsWith("GET") || request.StartsWith("POST") || request.StartsWith("HEAD") || request.StartsWith("PUT"))
                                        {
                                            var headers = Operate.ProxyConfig.Proxy.ParseHttpHeaders(request);
                                            if (headers.TryGetValue("Host", out string hostHeader))
                                            {
                                                string requestPath = request.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                                                string cleanPath = requestPath.Split('?')[0];

                                                #region//本地代理映射

                                                if (Operate.ProxyConfig.Mapping.Enable_MapLocal)
                                                {
                                                    var localRule = Operate.ProxyConfig.Mapping.GetMapLocal(
                                                        Operate.ProxyConfig.Proxy.MapProtocol.Http,
                                                        hostHeader.Split(':')[0],
                                                        80,
                                                        cleanPath);

                                                    if (localRule != null)
                                                    {
                                                        Operate.ProxyConfig.Mapping.MappingData_ToQueue(psSession, Operate.PacketConfig.Packet.PacketType.TCP_Req, bData, false);

                                                        if (File.Exists(localRule.LocalPath))
                                                        {
                                                            byte[] fileBytes = File.ReadAllBytes(localRule.LocalPath);
                                                            string contentType = Operate.ProxyConfig.Proxy.GetContentType(Path.GetExtension(localRule.LocalPath));

                                                            string response =
                                                                $"HTTP/1.1 200 OK\r\n" +
                                                                $"Content-Type: {contentType}\r\n" +
                                                                $"Content-Length: {fileBytes.Length}\r\n" +
                                                                "Connection: close\r\n\r\n";

                                                            byte[] headerBytes = Encoding.UTF8.GetBytes(response);

                                                            psSession.TrySend(headerBytes, 0, headerBytes.Length);
                                                            Operate.ProxyConfig.Mapping.MappingData_ToQueue(psSession, Operate.PacketConfig.Packet.PacketType.TCP_Resp, headerBytes, false);

                                                            psSession.TrySend(fileBytes, 0, fileBytes.Length);
                                                            Operate.ProxyConfig.Mapping.MappingData_ToQueue(psSession, Operate.PacketConfig.Packet.PacketType.TCP_Resp, fileBytes, false);

                                                            return;
                                                        }
                                                        else
                                                        {
                                                            byte[] b404 = Operate.ProxyConfig.Proxy.Get404Response();
                                                            psSession.TrySend(b404, 0, b404.Length);
                                                            Operate.ProxyConfig.Mapping.MappingData_ToQueue(psSession, Operate.PacketConfig.Packet.PacketType.TCP_Resp, b404, false);

                                                            return;
                                                        }
                                                    }
                                                }

                                                #endregion

                                                #region//远程代理映射

                                                if (Operate.ProxyConfig.Mapping.Enable_MapRemote)
                                                {
                                                    string TargetIP = hostHeader.Split(':')[0];
                                                    int TargetPort = 80;

                                                    var remoteRule = Operate.ProxyConfig.Mapping.GetMapRemote(
                                                        Operate.ProxyConfig.Proxy.MapProtocol.Http,
                                                        TargetIP,
                                                        TargetPort,
                                                        cleanPath);

                                                    if (remoteRule != null)
                                                    {
                                                        psSession.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(TargetIP, TargetPort);
                                                        Operate.ProxyConfig.Mapping.MappingData_ToQueue(psSession, Operate.PacketConfig.Packet.PacketType.TCP_Req, bData, true);

                                                        byte[] modifiedRequestBytes = Operate.ProxyConfig.Mapping.ModifyRequestHostAndPath(
                                                            request,
                                                            headers,
                                                            remoteRule.HostTo,
                                                            remoteRule.PortTo,
                                                            remoteRule.PathTo);

                                                        if (modifiedRequestBytes != null)
                                                        {
                                                            psSession.ServerAddress = Operate.ProxyConfig.Proxy.GetServerAddress(remoteRule.HostTo, remoteRule.PortTo);
                                                            psSession.TargetSocket.Send(modifiedRequestBytes);
                                                            Operate.ProxyConfig.Mapping.MappingData_ToQueue(psSession, Operate.PacketConfig.Packet.PacketType.TCP_Req, modifiedRequestBytes, true);
                                                        }

                                                        return;
                                                    }
                                                }

                                                #endregion
                                            }
                                        }
                                    }

                                    break;

                                case Operate.ProxyConfig.Proxy.DomainType.Https:
                                case Operate.ProxyConfig.Proxy.DomainType.Socket:
                                case Operate.ProxyConfig.Proxy.DomainType.External:

                                    break;
                            }

                            if (Operate.ProxyConfig.Proxy.HookTCP_Req)
                            {
                                Operate.FilterConfig.Filter.DoFilter_TCP(psSession, bData, Operate.PacketConfig.Packet.PacketType.TCP_Req);
                            }
                            else
                            {
                                psSession.TargetSocket.Send(bData);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ForwardData), ex.Message);
                        psSession.Close(SuperSocket.SocketBase.CloseReason.SocketError);
                    }
                }

                #endregion

                #region//执行 UDP 中继

                public static void UDPRelay(ProxySession psSession)
                {
                    try
                    {
                        ProxyUDP pu = Operate.ProxyConfig.Proxy.CreateNewUDP(psSession.SessionID);

                        if (pu == null)
                        {
                            return;
                        }

                        int localPort = ((IPEndPoint)pu.ClientSocket.LocalEndPoint).Port;
                        Operate.ProxyConfig.Proxy.SendCommandResponse(psSession, ProtocolType.Udp, Operate.ProxyConfig.Proxy.CommandResponse.Success, localPort);

                        psSession.StartUdpReceive(pu);
                    }
                    catch (SocketException)
                    {
                        Operate.ProxyConfig.Proxy.SendCommandResponse(psSession, ProtocolType.Tcp, Operate.ProxyConfig.Proxy.CommandResponse.Fault);
                    }
                }

                #endregion

                #region//处理 UDP 请求数据

                public static void ProcessUdpRequest(ProxySession psSession, ProxyUDP pu, IPEndPoint epRemote, Span<byte> bData)
                {
                    try
                    {
                        Operate.ProxyConfig.Proxy.AddressType addressType = (Operate.ProxyConfig.Proxy.AddressType)bData[3];

                        if (addressType == Operate.ProxyConfig.Proxy.AddressType.IPv4 ||
                            addressType == Operate.ProxyConfig.Proxy.AddressType.IPv6 ||
                            addressType == Operate.ProxyConfig.Proxy.AddressType.Domain)
                        {
                            pu.ClientEndPoint = epRemote;
                            byte[] bADDRESS = bData.Slice(4, bData.Length - 4).ToArray();

                            var (targetEndPoint, AddressString) = Operate.ProxyConfig.Proxy.GetIPEndPoint_ByAddressType(addressType, bADDRESS).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                            if (targetEndPoint != null)
                            {
                                Span<byte> bRequestData = Operate.ProxyConfig.Proxy.GetUDPData_ByAddressType(addressType, bData);
                                if (!bRequestData.IsEmpty)
                                {
                                    Operate.ProxyConfig.Proxy.UDP_Req_CNT++;
                                    Interlocked.Add(ref Operate.ProxyConfig.Proxy.Total_Request, bRequestData.Length);
                                    Interlocked.Add(ref Operate.ProxyConfig.Proxy.ProxySpeed_Uplink, bRequestData.Length);

                                    if (Operate.ProxyConfig.Proxy.HookUDP_Req)
                                    {
                                        Operate.FilterConfig.Filter.DoFilter_UDP(psSession, pu, targetEndPoint, bRequestData, Operate.PacketConfig.Packet.PacketType.UDP_Req);
                                    }
                                    else
                                    {
                                        psSession.SendUdpData(pu.ClientSocket, bRequestData, targetEndPoint);
                                    }

                                    pu.UpdateActivity();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ProcessUdpRequest), ex.Message);
                    }
                }

                #endregion

                #region//处理 UDP 响应数据

                public static void ProcessUdpResponse(ProxySession psSession, ProxyUDP pu, IPEndPoint epRemote, Span<byte> bData)
                {
                    try
                    {
                        if (pu.ClientEndPoint == null)
                        {
                            return;
                        }

                        ReadOnlySpan<byte> bIP = pu.ClientEndPoint.Address.GetAddressBytes();
                        ushort port = ((ushort)pu.ClientEndPoint.Port);
                        ReadOnlySpan<byte> bPort = stackalloc byte[2] { (byte)(port >> 8), (byte)port };

                        byte[] responseBuffer = ArrayPool<byte>.Shared.Rent(4 + bIP.Length + bPort.Length + bData.Length);
                        Span<byte> bResponseData = responseBuffer.AsSpan(0, 4 + bIP.Length + bPort.Length + bData.Length);

                        bResponseData[0] = 0x00;
                        bResponseData[1] = 0x00;
                        bResponseData[2] = 0x00;
                        bResponseData[3] = (byte)Operate.ProxyConfig.Proxy.AddressType.IPv4;
                        bIP.CopyTo(bResponseData.Slice(4, bIP.Length));
                        bPort.CopyTo(bResponseData.Slice(4 + bIP.Length, bPort.Length));
                        bData.CopyTo(bResponseData.Slice(4 + bIP.Length + bPort.Length, bData.Length));

                        if (!bResponseData.IsEmpty)
                        {
                            Operate.ProxyConfig.Proxy.UDP_Resp_CNT++;
                            Interlocked.Add(ref Operate.ProxyConfig.Proxy.Total_Response, bResponseData.Length);
                            Interlocked.Add(ref Operate.ProxyConfig.Proxy.ProxySpeed_Downlink, bResponseData.Length);

                            if (Operate.ProxyConfig.Proxy.HookUDP_Resp)
                            {
                                Operate.FilterConfig.Filter.DoFilter_UDP(psSession, pu, epRemote, bResponseData, Operate.PacketConfig.Packet.PacketType.UDP_Resp);
                            }
                            else
                            {
                                psSession.SendUdpData(pu.ClientSocket, bResponseData, pu.ClientEndPoint);
                            }

                            pu.UpdateActivity();
                        }

                        ArrayPool<byte>.Shared.Return(responseBuffer);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ProcessUdpResponse), ex.Message);
                    }
                }

                #endregion

                #region//处理断包

                public static byte[] CombineData(byte[] m_Buffer, byte[] newData, int offset, int length)
                {
                    if (m_Buffer.Length == 0)
                    {
                        byte[] result = new byte[length];
                        Buffer.BlockCopy(newData, offset, result, 0, length);
                        return result;
                    }
                    else
                    {
                        byte[] result = new byte[m_Buffer.Length + length];
                        Buffer.BlockCopy(m_Buffer, 0, result, 0, m_Buffer.Length);
                        Buffer.BlockCopy(newData, offset, result, m_Buffer.Length, length);
                        return result;
                    }
                }

                #endregion                

                #region//设置系统代理

                public static bool EnableSystemProxy(Form form)
                {
                    try
                    {
                        string proxyServer = string.Format("socks5://127.0.0.1:{0}", ProxyConfig.Proxy.ProxyPort);

                        using (RegistryKey registry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
                        {
                            if (registry == null)
                                return false;

                            // 设置代理服务器
                            registry.SetValue("ProxyServer", proxyServer);

                            // 启用代理
                            registry.SetValue("ProxyEnable", 1);                            

                            NotifySystemProxyChanged();

                            AntdUI.Message.open(new AntdUI.Message.Config(form, "系统代理已启用", TType.Success)
                            {
                                LocalizationText = "ProxySettingsForm.SystemProxy.Start"
                            });

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(EnableSystemProxy), ex.Message);
                        return false;
                    }
                }

                public static bool DisableSystemProxy(Form form)
                {
                    try
                    {
                        using (RegistryKey registry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
                        {
                            if (registry == null)
                                return false;

                            // 禁用代理
                            registry.SetValue("ProxyEnable", 0);
                            NotifySystemProxyChanged();

                            AntdUI.Message.open(new AntdUI.Message.Config(form, "系统代理已关闭", TType.Error)
                            {
                                LocalizationText = "ProxySettingsForm.SystemProxy.Stop"
                            });

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(DisableSystemProxy), ex.Message);
                        return false;
                    }
                }

                private static void NotifySystemProxyChanged()
                {
                    Wininet.InternetSetOption(IntPtr.Zero, Wininet.INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
                    Wininet.InternetSetOption(IntPtr.Zero, Wininet.INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
                }

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

                public static Operate.ProxyConfig.Proxy.AddressType GetAddressType_ByString(string IPString)
                {
                    if (IsValidIPv4(IPString))
                        return Operate.ProxyConfig.Proxy.AddressType.IPv4;

                    if (IsValidIPv6(IPString))
                        return Operate.ProxyConfig.Proxy.AddressType.IPv6;

                    if (IsValidDomain(IPString))
                        return Operate.ProxyConfig.Proxy.AddressType.Domain;

                    return Operate.ProxyConfig.Proxy.AddressType.Invalid;
                }

                #endregion

                #region//判断接收的数据是否匹配代理步骤

                public static bool CheckDataIsMatchProxyStep(ReadOnlySpan<byte> bData, Operate.ProxyConfig.Proxy.ProxyStep proxyStep)
                {
                    bool bReturn = false;

                    try
                    {
                        byte VERSION = bData[0];

                        switch (proxyStep)
                        {
                            case Operate.ProxyConfig.Proxy.ProxyStep.Handshake:

                                if (VERSION == ((byte)Operate.ProxyConfig.Proxy.ProxyType.Socket5))
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

                            case Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName:

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

                            case Operate.ProxyConfig.Proxy.ProxyStep.Command:

                                if (VERSION == ((byte)Operate.ProxyConfig.Proxy.ProxyType.Socket5))
                                {
                                    if (bData.Length > 4)
                                    {
                                        byte ADDRESS_TYPE = bData[3];
                                        Operate.ProxyConfig.Proxy.AddressType AddressType = (Operate.ProxyConfig.Proxy.AddressType)ADDRESS_TYPE;

                                        int DST_ADDR = 0;
                                        switch (AddressType)
                                        {
                                            case Operate.ProxyConfig.Proxy.AddressType.IPv4:
                                                DST_ADDR = 4;
                                                break;

                                            case Operate.ProxyConfig.Proxy.AddressType.IPv6:
                                                DST_ADDR = 16;
                                                break;

                                            case Operate.ProxyConfig.Proxy.AddressType.Domain:
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

                            case Operate.ProxyConfig.Proxy.ProxyStep.ForwardData:

                                bReturn = true;

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(CheckDataIsMatchProxyStep), ex.Message);
                    }

                    return bReturn;
                }

                #endregion

                #region//检测外部代理服务器

                public static async Task<bool> DetectionExternalProxy(Form form, string EXTIP, ushort EXTPort, bool EXTAuth, string EXTUsername, string EXTPassword)
                {
                    try
                    {
                        IPEndPoint ExternalProxyEP = await ProxyConfig.Proxy.GetIPEndPoint_ByAddressString(EXTIP, EXTPort);
                        if (ExternalProxyEP == null)
                        {
                            AntdUI.Message.open(new AntdUI.Message.Config(form, "外部代理设置错误", TType.Error)
                            {
                                LocalizationText = "SystemSettingsForm.Success"
                            });

                            return false;
                        }

                        using (Socket proxySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                        {
                            // 设置连接超时
                            var connectTask = proxySocket.ConnectAsync(ExternalProxyEP);
                            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));

                            if (await Task.WhenAny(connectTask, timeoutTask) == timeoutTask)
                            {
                                AntdUI.Message.open(new AntdUI.Message.Config(form, "连接超时", TType.Error)
                                {
                                    LocalizationText = "SystemSettingsForm.Success"
                                });

                                return false;
                            }

                            //SOCKS5 握手
                            byte[] handshakeRequest = null;
                            if (EXTAuth)
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
                                AntdUI.Message.open(new AntdUI.Message.Config(form, "外部代理不支持SOCKS", TType.Error)
                                {
                                    LocalizationText = "SystemSettingsForm.Success"
                                });

                                return false;
                            }

                            switch (handshakeResponse[1])
                            {
                                case 0x00:
                                    // 无需认证
                                    break;

                                case 0x02:
                                    // 需要用户名/密码认证
                                    if (!EXTAuth)
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(form, "外部代理要求认证", TType.Warn)
                                        {
                                            LocalizationText = "SystemSettingsForm.Success"
                                        });

                                        return false;
                                    }

                                    byte[] AuthRequest = ProxyConfig.Proxy.CreateSOCKS5AuthPacket(EXTUsername, EXTPassword);
                                    if (AuthRequest == null)
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(form, "外部代理认证失败", TType.Error)
                                        {
                                            LocalizationText = "SystemSettingsForm.Success"
                                        });

                                        return false;
                                    }
                                    await proxySocket.SendAsync(new ArraySegment<byte>(AuthRequest), SocketFlags.None);

                                    byte[] AuthResponse = new byte[2];
                                    await proxySocket.ReceiveAsync(new ArraySegment<byte>(AuthResponse), SocketFlags.None);
                                    if (AuthResponse[1] != 0x00)
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(form, "外部代理认证失败", TType.Error)
                                        {
                                            LocalizationText = "SystemSettingsForm.Success"
                                        });

                                        return false;
                                    }
                                    break;

                                default:
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, "不支持的认证方式", TType.Warn)
                                    {
                                        LocalizationText = "SystemSettingsForm.Success"
                                    });

                                    return false;
                            }

                            return true;
                        }
                    }
                    catch
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(form, "外部代理拒绝连接", TType.Error)
                        {
                            LocalizationText = "SystemSettingsForm.Success"
                        });

                        return false;
                    }
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

                #region//初始化 CCProxy 模板

                public static void InitCCProxy_HTML()
                {
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web", "CCProxy", "cn_acclistadmin.htm");

                    if (File.Exists(filePath))
                    {
                        Operate.ProxyConfig.Account.CCProxy_HTML = File.ReadAllText(filePath, Encoding.UTF8);
                    }
                }

                #endregion

                #region//解析 Http 头数据

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
                        DoLog(nameof(ParseHttpHeaders), ex.Message);
                    }

                    return headers;
                }

                #endregion                

                #region//发送404响应

                public static byte[] Get404Response()
                {
                    string response =
                        "HTTP/1.1 404 Not Found\r\n" +
                        "Content-Type: text/html\r\n" +
                        "Content-Length: 0\r\n" +
                        "Connection: close\r\n\r\n";

                    return Encoding.UTF8.GetBytes(response);
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
                        DoLog(nameof(GetContentType), ex.Message);
                    }

                    return "application/octet-stream";
                }

                #endregion                

                #region//获取IP地址信息                

                public static async Task<IPEndPoint> GetIPEndPoint_ByAddressString(string AddressString, ushort Port)
                {
                    try
                    {
                        IPAddress ipAddress = await ProxyConfig.Proxy.ResolveAddress(AddressString);
                        return new IPEndPoint(ipAddress, Port);
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(GetIPEndPoint_ByAddressString), ex.Message);
                    }

                    return null;
                }

                public static async Task<(IPEndPoint EndPoint, string AddressString)> GetIPEndPoint_ByAddressType(Operate.ProxyConfig.Proxy.AddressType addressType, byte[] bData)
                {
                    string addressString = string.Empty;
                    IPEndPoint endPoint = null;

                    try
                    {
                        IPAddress ip = IPAddress.Any;
                        ushort port = 0;
                        int portPosition = 0;

                        switch (addressType)
                        {
                            case Operate.ProxyConfig.Proxy.AddressType.IPv4:
                                ip = new IPAddress(bData.AsSpan(0, 4).ToArray());
                                portPosition = 4;
                                addressString = ip.ToString();
                                break;

                            case Operate.ProxyConfig.Proxy.AddressType.IPv6:
                                ip = new IPAddress(bData.AsSpan(0, 16).ToArray());
                                portPosition = 16;
                                addressString = ip.ToString();
                                break;

                            case Operate.ProxyConfig.Proxy.AddressType.Domain:
                                byte length = bData[0];
                                var domainBytes = bData.AsSpan(1, length).ToArray();
                                addressString = Operate.SystemConfig.BytesToString(
                                    Operate.PacketConfig.Packet.EncodingFormat.UTF8,
                                    domainBytes);
                                ip = await ProxyConfig.Proxy.ResolveAddress(addressString);
                                portPosition = 1 + length;
                                break;
                        }

                        if (ip != null)
                        {
                            port = Operate.SystemConfig.ByteArrayToInt16BigEndian(bData.AsSpan(portPosition, 2).ToArray());
                            endPoint = new IPEndPoint(ip, port);
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(GetIPEndPoint_ByAddressType), ex.Message);
                    }

                    return (endPoint, addressString);
                }

                private static async Task<IPAddress> ResolveAddress(string addressString)
                {
                    try
                    {
                        var addressType = Operate.ProxyConfig.Proxy.GetAddressType_ByString(addressString);

                        switch (addressType)
                        {
                            case Operate.ProxyConfig.Proxy.AddressType.IPv4:
                            case Operate.ProxyConfig.Proxy.AddressType.IPv6:
                                return IPAddress.Parse(addressString);

                            case Operate.ProxyConfig.Proxy.AddressType.Domain:

                                if (Operate.ProxyConfig.Proxy.DnsCache.TryGetValue(addressString, out var cachedIp))
                                {
                                    return cachedIp;
                                }

                                try
                                {
                                    var entry = await Dns.GetHostEntryAsync(addressString).ConfigureAwait(false);
                                    var ipv4 = entry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                                    var result = ipv4 ?? entry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6)
                                                 ?? entry.AddressList.First();

                                    Operate.ProxyConfig.Proxy.DnsCache.AddOrUpdate(
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
                        DoLog(nameof(ResolveAddress), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//获取 UDP 数据包

                public static Span<byte> GetUDPData_ByAddressType(Operate.ProxyConfig.Proxy.AddressType addressType, Span<byte> bData)
                {
                    try
                    {
                        switch (addressType)
                        {
                            case Operate.ProxyConfig.Proxy.AddressType.IPv4:
                                return bData.Length >= 10 ? bData.Slice(10) : Span<byte>.Empty;

                            case Operate.ProxyConfig.Proxy.AddressType.Domain:

                                if (bData.Length < 5)
                                {
                                    return Span<byte>.Empty;
                                }

                                byte domainLength = bData[4];
                                int domainStart = 5 + domainLength + 2;
                                return bData.Length >= domainStart ? bData.Slice(domainStart) : Span<byte>.Empty;

                            case Operate.ProxyConfig.Proxy.AddressType.IPv6:
                                return bData.Length >= 22 ? bData.Slice(22) : Span<byte>.Empty;

                            default:
                                return Span<byte>.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(GetUDPData_ByAddressType), ex.Message);
                        return Span<byte>.Empty;
                    }
                }

                #endregion                

                #region//获取端口对应的域名类型

                public static Operate.ProxyConfig.Proxy.DomainType GetDomainType_ByPort(int Port)
                {
                    try
                    {
                        if (Operate.ProxyConfig.Proxy.Enable_ExternalProxy)
                        {
                            if (Operate.ProxyConfig.Proxy.Enable_ExternalProxy_AppointPort && !string.IsNullOrEmpty(Operate.ProxyConfig.Proxy.ExternalProxy_AppointPort))
                            {
                                HashSet<string> ExternalProxyPorts = new HashSet<string>(Operate.ProxyConfig.Proxy.ExternalProxy_AppointPort.Split(','));

                                if (ExternalProxyPorts.Contains(Port.ToString()))
                                {
                                    return Operate.ProxyConfig.Proxy.DomainType.External;
                                }
                            }
                            else
                            {
                                return Operate.ProxyConfig.Proxy.DomainType.External;
                            }
                        }

                        if (Port == 80 || Port == 8080)
                        {
                            return Operate.ProxyConfig.Proxy.DomainType.Http;
                        }
                        else if (Port == 443 || Port == 8443)
                        {
                            return Operate.ProxyConfig.Proxy.DomainType.Https;
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(GetDomainType_ByPort), ex.Message);
                    }

                    return Operate.ProxyConfig.Proxy.DomainType.Socket;
                }

                #endregion

                #region//获取服务端地址

                public static string GetServerAddress(string TargetAddress, int TargetPort)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(TargetAddress))
                        {
                            return string.Empty;
                        }

                        return string.Format("{0}:{1}", TargetAddress, TargetPort);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetServerAddress), ex.Message);                        
                    }

                    return string.Empty;
                }

                #endregion

                #region//获取客户端地址

                public static string GetClientAddress(string TargetAddress, int TargetPort)
                {
                    if (string.IsNullOrEmpty(TargetAddress))
                    {
                        return string.Empty;
                    }

                    return $"{TargetAddress}:{TargetPort}";
                }

                #endregion                

                #region//新增白名单

                private static readonly object _whiteListLock = new object();

                public static async void AddToWhiteList(string ipOrRange, bool IsExpiry, DateTime ExpiryTime, DateTime CreateTime)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(ipOrRange))
                        {
                            return;
                        }

                        lock (_whiteListLock)
                        {
                            if (ProxyConfig.Proxy.IsExistsInWhiteList(ipOrRange))
                            {
                                return;
                            }
                        }

                        string IPToCheck = ipOrRange;
                        if (ipOrRange.Contains("-"))
                        {
                            IPToCheck = ipOrRange.Split('-')[0].Trim();
                        }

                        string IPLocation = await SystemConfig.GetIPLocation(IPToCheck);
                        WhiteListInfo wli = new WhiteListInfo(ipOrRange, IPLocation, IsExpiry, ExpiryTime, CreateTime);

                        lock (_whiteListLock)
                        {
                            if (!ProxyConfig.Proxy.IsExistsInWhiteList(ipOrRange))
                            {
                                Operate.ProxyConfig.Proxy.lstWhiteList.Add(wli);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddToWhiteList), ex.Message);
                    }
                }

                #endregion

                #region//更新白名单

                public static async void UpdateWhiteList(WhiteListInfo wli, string ipOrRange, bool IsExpiry, DateTime ExpiryTime)
                {
                    try
                    {
                        if (wli == null || string.IsNullOrEmpty(ipOrRange))
                        {
                            return;
                        }

                        if (wli.IPAddress.Equals(ipOrRange) && wli.IsExpiry == IsExpiry && wli.ExpiryTime == ExpiryTime)
                        {
                            return;
                        }

                        if (!wli.IPAddress.Equals(ipOrRange) && ProxyConfig.Proxy.IsExistsInWhiteList(ipOrRange))
                        {
                            return;
                        }

                        wli.IPAddress = ipOrRange;
                        wli.IsExpiry = IsExpiry;
                        wli.ExpiryTime = ExpiryTime;

                        string IPToCheck = ipOrRange;
                        if (ipOrRange.Contains("-"))
                        {
                            IPToCheck = ipOrRange.Split('-')[0].Trim();
                        }

                        string IPLocation = await SystemConfig.GetIPLocation(IPToCheck);
                        wli.IPLocation = IPLocation;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateWhiteList), ex.Message);
                    }
                }

                #endregion

                #region//编辑白名单

                public static void OpenWhiteListEdit(Form form, FireWallSetting fwForm, WhiteListInfo wli)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("FireWallSetting.WhiteListEdit", "白名单编辑"), new WhiteListEdit(form, fwForm, wli))
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//删除白名单（对话框）

                public static void DeleteWhiteList_Dialog(Form form, WhiteListInfo wli)
                {
                    try
                    {
                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("FireWallSetting.WhiteList", "白名单"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                        {
                            Icon = TType.Warn,
                            Keyboard = false,
                            MaskClosable = false,
                            OnOk = config =>
                            {
                                if (wli != null)
                                {
                                    ProxyConfig.Proxy.lstWhiteList.Remove(wli);
                                }

                                return true;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteWhiteList_Dialog), ex.Message);
                    }
                }

                #endregion

                #region//清空白名单（对话框）

                public static void CleanUpWhiteList_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("FireWallSetting.WhiteList", "白名单"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                    {
                        Icon = TType.Warn,
                        Keyboard = false,
                        MaskClosable = false,
                        OnOk = config =>
                        {
                            ProxyConfig.Proxy.CleanUpWhiteList();
                            return true;
                        }
                    });
                }

                public static void CleanUpWhiteList()
                {
                    try
                    {
                        ProxyConfig.Proxy.lstWhiteList.Clear();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CleanUpWhiteList), ex.Message);
                    }
                }

                #endregion

                #region//检测是否已存在此IP（白名单）

                public static bool IsExistsInWhiteList(string ipOrRange)
                {
                    return Operate.ProxyConfig.Proxy.lstWhiteList
                        .Any(wli => wli.IPAddress.Equals(ipOrRange, StringComparison.OrdinalIgnoreCase));
                }

                #endregion

                #region//白名单的列表操作

                public static void UpdateWhiteList_ByListAction(Form form, SystemConfig.ListAction listAction, WhiteListInfo wli)
                {
                    try
                    {
                        int iIndex = 0;

                        switch (listAction)
                        {
                            case SystemConfig.ListAction.Top:

                                ProxyConfig.Proxy.lstWhiteList.Remove(wli);
                                ProxyConfig.Proxy.lstWhiteList.Insert(0, wli);

                                break;

                            case SystemConfig.ListAction.Up:

                                iIndex = ProxyConfig.Proxy.lstWhiteList.IndexOf(wli);
                                if (iIndex > 0)
                                {
                                    ProxyConfig.Proxy.lstWhiteList.Remove(wli);
                                    ProxyConfig.Proxy.lstWhiteList.Insert(iIndex - 1, wli);
                                }

                                break;

                            case SystemConfig.ListAction.Down:

                                iIndex = ProxyConfig.Proxy.lstWhiteList.IndexOf(wli);
                                if (iIndex > -1 && iIndex < ProxyConfig.Proxy.lstWhiteList.Count - 1)
                                {
                                    ProxyConfig.Proxy.lstWhiteList.Remove(wli);
                                    ProxyConfig.Proxy.lstWhiteList.Insert(iIndex + 1, wli);
                                }

                                break;

                            case SystemConfig.ListAction.Bottom:

                                ProxyConfig.Proxy.lstWhiteList.Remove(wli);
                                ProxyConfig.Proxy.lstWhiteList.Add(wli);

                                break;

                            case SystemConfig.ListAction.Import:

                                ProxyConfig.Proxy.LoadWhiteList_Dialog(form);

                                break;

                            case SystemConfig.ListAction.Export:

                                ProxyConfig.Proxy.SaveWhiteList_Dialog(form, string.Empty, ProxyConfig.Proxy.lstWhiteList);

                                break;

                            case SystemConfig.ListAction.CleanUp:

                                ProxyConfig.Proxy.CleanUpWhiteList_Dialog(form);

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateWhiteList_ByListAction), ex.Message);
                    }
                }

                #endregion                

                #region//新增黑名单

                private static readonly object _blackListLock = new object();

                public static async void AddToBlackList(string ipOrRange, bool IsExpiry, DateTime ExpiryTime, DateTime CreateTime)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(ipOrRange))
                        {
                            return;
                        }

                        lock (_blackListLock)
                        {
                            if (ProxyConfig.Proxy.IsExistsInBlackList(ipOrRange))
                            {
                                return;
                            }
                        }

                        string IPToCheck = ipOrRange;
                        if (ipOrRange.Contains("-"))
                        {
                            IPToCheck = ipOrRange.Split('-')[0].Trim();
                        }

                        string IPLocation = await SystemConfig.GetIPLocation(IPToCheck);
                        BlackListInfo bli = new BlackListInfo(ipOrRange, IPLocation, IsExpiry, ExpiryTime, CreateTime);

                        lock (_blackListLock)
                        {
                            if (!ProxyConfig.Proxy.IsExistsInBlackList(ipOrRange))
                            {
                                Operate.ProxyConfig.Proxy.lstBlackList.Add(bli);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddToBlackList), ex.Message);
                    }
                }

                #endregion

                #region//更新黑名单

                public static async void UpdateBlackList(BlackListInfo bli, string ipOrRange, bool IsExpiry, DateTime ExpiryTime)
                {
                    try
                    {
                        if (bli == null || string.IsNullOrEmpty(ipOrRange) || ExpiryTime == null)
                        {
                            return;
                        }

                        if (bli.IPAddress.Equals(ipOrRange) && bli.IsExpiry == IsExpiry && bli.ExpiryTime == ExpiryTime)
                        {
                            return;
                        }

                        if (!bli.IPAddress.Equals(ipOrRange) && ProxyConfig.Proxy.IsExistsInBlackList(ipOrRange))
                        {
                            return;
                        }

                        bli.IPAddress = ipOrRange;
                        bli.IsExpiry = IsExpiry;
                        bli.ExpiryTime = ExpiryTime;

                        string IPToCheck = ipOrRange;
                        if (ipOrRange.Contains("-"))
                        {
                            IPToCheck = ipOrRange.Split('-')[0].Trim();
                        }

                        string IPLocation = await SystemConfig.GetIPLocation(IPToCheck);
                        bli.IPLocation = IPLocation;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateBlackList), ex.Message);
                    }
                }

                #endregion

                #region//编辑黑名单

                public static void OpenBlackListEdit(Form form, FireWallSetting fwForm, BlackListInfo bli)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("FireWallSetting.BlackListEdit", "黑名单编辑"), new BlackListEdit(form, fwForm, bli))
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//删除黑名单（对话框）

                public static void DeleteBlackList_Dialog(Form form, BlackListInfo bli)
                {
                    try
                    {
                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("FireWallSetting.BlackList", "黑名单"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                        {
                            Icon = TType.Warn,
                            Keyboard = false,
                            MaskClosable = false,
                            OnOk = config =>
                            {
                                if (bli != null)
                                {
                                    ProxyConfig.Proxy.lstBlackList.Remove(bli);
                                }

                                return true;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteBlackList_Dialog), ex.Message);
                    }
                }

                #endregion

                #region//清空黑名单（对话框）

                public static void CleanUpBlackList_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("FireWallSetting.BlackList", "黑名单"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                    {
                        Icon = TType.Warn,
                        Keyboard = false,
                        MaskClosable = false,
                        OnOk = config =>
                        {
                            ProxyConfig.Proxy.CleanUpBlackList();
                            return true;
                        }
                    });
                }

                public static void CleanUpBlackList()
                {
                    try
                    {
                        ProxyConfig.Proxy.lstBlackList.Clear();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CleanUpBlackList), ex.Message);
                    }
                }

                #endregion

                #region//检测是否已存在此IP（黑名单）

                public static bool IsExistsInBlackList(string ipOrRange)
                {
                    return Operate.ProxyConfig.Proxy.lstBlackList
                        .Any(bli => bli.IPAddress.Equals(ipOrRange, StringComparison.OrdinalIgnoreCase));
                }

                #endregion

                #region//黑名单的列表操作

                public static void UpdateBlackList_ByListAction(Form form, SystemConfig.ListAction listAction, BlackListInfo bli)
                {
                    try
                    {
                        int iIndex = 0;

                        switch (listAction)
                        {
                            case SystemConfig.ListAction.Top:

                                ProxyConfig.Proxy.lstBlackList.Remove(bli);
                                ProxyConfig.Proxy.lstBlackList.Insert(0, bli);

                                break;

                            case SystemConfig.ListAction.Up:

                                iIndex = ProxyConfig.Proxy.lstBlackList.IndexOf(bli);
                                if (iIndex > 0)
                                {
                                    ProxyConfig.Proxy.lstBlackList.Remove(bli);
                                    ProxyConfig.Proxy.lstBlackList.Insert(iIndex - 1, bli);
                                }

                                break;

                            case SystemConfig.ListAction.Down:

                                iIndex = ProxyConfig.Proxy.lstBlackList.IndexOf(bli);
                                if (iIndex > -1 && iIndex < ProxyConfig.Proxy.lstBlackList.Count - 1)
                                {
                                    ProxyConfig.Proxy.lstBlackList.Remove(bli);
                                    ProxyConfig.Proxy.lstBlackList.Insert(iIndex + 1, bli);
                                }

                                break;

                            case SystemConfig.ListAction.Bottom:

                                ProxyConfig.Proxy.lstBlackList.Remove(bli);
                                ProxyConfig.Proxy.lstBlackList.Add(bli);

                                break;

                            case SystemConfig.ListAction.Import:

                                ProxyConfig.Proxy.LoadBlackList_Dialog(form);

                                break;

                            case SystemConfig.ListAction.Export:

                                ProxyConfig.Proxy.SaveBlackList_Dialog(form, string.Empty, ProxyConfig.Proxy.lstBlackList);

                                break;

                            case SystemConfig.ListAction.CleanUp:

                                ProxyConfig.Proxy.CleanUpBlackList_Dialog(form);

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateBlackList_ByListAction), ex.Message);
                    }
                }

                #endregion                                

                #region//解析防火墙的IP范围

                public static (long StartIP, long EndIP) ParseIpRange(string ipAddress)
                {
                    try
                    {
                        long startIP = -1;
                        long endIP = -1;

                        // 支持单个IP和IP范围（如：192.168.1.1 或 192.168.1.1-192.168.1.100）
                        if (ipAddress.Contains("-"))
                        {
                            var parts = ipAddress.Split('-');
                            if (parts.Length == 2)
                            {
                                startIP = Operate.ProxyConfig.Proxy.ConvertIpToLong(parts[0].Trim());
                                endIP = Operate.ProxyConfig.Proxy.ConvertIpToLong(parts[1].Trim());
                            }
                        }
                        else if (ipAddress.Contains("/"))
                        {
                            // 支持CIDR格式（如：192.168.1.0/24）
                            var cidrResult = Operate.ProxyConfig.Proxy.ParseCidr(ipAddress);
                            if (cidrResult != null)
                            {
                                startIP = cidrResult.Value.Start;
                                endIP = cidrResult.Value.End;
                            }
                        }
                        else
                        {
                            // 单个IP
                            long ipLong = Operate.ProxyConfig.Proxy.ConvertIpToLong(ipAddress.Trim());
                            startIP = ipLong;
                            endIP = ipLong;
                        }

                        return (startIP, endIP);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ParseIpRange), ex.Message);
                        return (-1, -1);
                    }
                }

                public static long ConvertIpToLong(string ip)
                {
                    if (string.IsNullOrWhiteSpace(ip))
                        return -1;

                    try
                    {
                        if (IPAddress.TryParse(ip, out IPAddress address) && address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            byte[] bytes = address.GetAddressBytes();
                            return ((long)bytes[0] << 24) | ((long)bytes[1] << 16) | ((long)bytes[2] << 8) | bytes[3];
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ConvertIpToLong), ex.Message);
                    }                    

                    return -1;
                }

                public static (long Start, long End)? ParseCidr(string cidr)
                {
                    try
                    {
                        var parts = cidr.Split('/');
                        if (parts.Length != 2) return null;

                        long baseIp = ConvertIpToLong(parts[0]);
                        int prefixLength = int.Parse(parts[1]);

                        if (prefixLength < 0 || prefixLength > 32) return null;

                        long mask = (0xFFFFFFFFL << (32 - prefixLength)) & 0xFFFFFFFFL;
                        long start = baseIp & mask;
                        long end = start + (1L << (32 - prefixLength)) - 1;

                        return (start, end);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ParseCidr), ex.Message);                        
                    }

                    return null;
                }

                public static bool IsIpInRanges(long ipValue, BindingList<WhiteListInfo> WhiteList)
                {
                    if (WhiteList == null || WhiteList.Count == 0)
                        return false;

                    try
                    {
                        var matchedItem = WhiteList.FirstOrDefault(wli => wli?.ContainsIp(ipValue) == true);

                        if (matchedItem != null)
                        {
                            bool isNotExpired = !matchedItem.IsExpiry || matchedItem.ExpiryTime > DateTime.Now;

                            if (isNotExpired)
                            {
                                matchedItem.EffectCount += 1;
                                return true;
                            }
                            else
                            {
                                if (Operate.ProxyConfig.Proxy.FireWall_AutoClear_Expiry)
                                {
                                    WhiteList.Remove(matchedItem);
                                }
                                
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(IsIpInRanges), ex.Message);
                    }

                    return false;
                }

                public static bool IsIpInRanges(long ipValue, BindingList<BlackListInfo> BlackList)
                {
                    if (BlackList == null || BlackList.Count == 0)
                        return false;

                    try
                    {
                        var matchedItem = BlackList.FirstOrDefault(bli => bli?.ContainsIp(ipValue) == true);

                        if (matchedItem != null)
                        {
                            bool isNotExpired = !matchedItem.IsExpiry || matchedItem.ExpiryTime > DateTime.Now;

                            if (isNotExpired)
                            {
                                matchedItem.EffectCount += 1;
                                return true;
                            }
                            else
                            {
                                if (Operate.ProxyConfig.Proxy.FireWall_AutoClear_Expiry)
                                {
                                    BlackList.Remove(matchedItem);
                                }
                                
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(IsIpInRanges), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//保存白名单到数据库

                public static void SaveWhiteList_ToDB()
                {
                    try
                    {
                        DataBase.DeleteTable_WhiteList();
                        DataBase.InsertTable_WhiteList();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveWhiteList_ToDB), ex.Message);
                    }
                }

                #endregion

                #region //保存黑名单到数据库

                public static void SaveBlackList_ToDB()
                {
                    try
                    {
                        DataBase.DeleteTable_BlackList();
                        DataBase.InsertTable_BlackList();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveBlackList_ToDB), ex.Message);
                    }
                }

                #endregion

                #region//从数据库加载白名单（异步）

                public static async void LoadWhiteList_FromDB()
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            DataTable dtWhiteList = DataBase.SelectTable_WhiteList();

                            foreach (DataRow dataRow in dtWhiteList.Rows)
                            {
                                string IPAddress = dataRow["IPAddress"].ToString();
                                long StartIP = long.Parse(dataRow["StartIP"].ToString());
                                long EndIP = long.Parse(dataRow["EndIP"].ToString());
                                bool IsExpiry = bool.Parse(dataRow["IsExpiry"].ToString());
                                DateTime ExpiryTime = DateTime.Parse(dataRow["ExpiryTime"].ToString());
                                DateTime CreateTime = DateTime.Parse(dataRow["CreateTime"].ToString());

                                ProxyConfig.Proxy.AddToWhiteList(IPAddress, IsExpiry, ExpiryTime, CreateTime);
                            }
                        }
                        catch (Exception ex)
                        {
                            Operate.DoLog(nameof(LoadWhiteList_FromDB), ex.Message);
                        }
                    });
                }

                #endregion                

                #region //从数据库加载黑名单（异步）

                public static async void LoadBlackList_FromDB()
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            DataTable dtBlackList = DataBase.SelectTable_BlackList();

                            foreach (DataRow dataRow in dtBlackList.Rows)
                            {
                                string IPAddress = dataRow["IPAddress"].ToString();
                                long StartIP = long.Parse(dataRow["StartIP"].ToString());
                                long EndIP = long.Parse(dataRow["EndIP"].ToString());
                                bool IsExpiry = bool.Parse(dataRow["IsExpiry"].ToString());
                                DateTime ExpiryTime = DateTime.Parse(dataRow["ExpiryTime"].ToString());
                                DateTime CreateTime = DateTime.Parse(dataRow["CreateTime"].ToString());

                                ProxyConfig.Proxy.AddToBlackList(IPAddress, IsExpiry, ExpiryTime, CreateTime);
                            }
                        }
                        catch (Exception ex)
                        {
                            Operate.DoLog(nameof(LoadBlackList_FromDB), ex.Message);
                        }
                    });
                }

                #endregion

                #region//保存白名单到文件（对话框）

                public static void SaveWhiteList_Dialog(Form form, string FileName, BindingList<WhiteListInfo> wliList)
                {
                    try
                    {
                        if (ProxyConfig.Proxy.lstWhiteList.Count > 0)
                        {
                            SaveFileDialog sfdSaveFile = new SaveFileDialog();
                            sfdSaveFile.Filter = AntdUI.Localization.Get("FireWallSetting.WhiteListFile", "白名单文件") + "（*.wl）|*.wl";

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveFile.FileName = FileName;
                            }

                            sfdSaveFile.RestoreDirectory = true;
                            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveFile.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("FireWallSetting.WhiteListFile.Export", "导出白名单"));

                                    if (SaveWhiteList(FilePath, wliList, EncryptPassword.DoEncrypt, EncryptPassword.Password))
                                    {
                                        string Title = AntdUI.Localization.Get("FireWallSetting.WhiteListFile.Export.Success", "导出白名单成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SaveWhiteList_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("FireWallSetting.WhiteListFile.Export.Fail", "导出白名单失败");
                                        string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveWhiteList_Dialog), ex.Message);
                    }
                }

                private static bool SaveWhiteList(string FilePath, BindingList<WhiteListInfo> wliList, bool DoEncrypt, string Password)
                {
                    try
                    {
                        XDocument xdoc = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };

                        XElement xeWhiteList = ProxyConfig.Proxy.GetWhiteList_XML(wliList);
                        if (xeWhiteList == null)
                        {
                            return false;
                        }

                        xdoc.Add(xeWhiteList);
                        xdoc.Save(FilePath);

                        if (DoEncrypt)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                SystemConfig.EncryptXMLFile(FilePath, Password);
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveWhiteList), ex.Message);
                    }

                    return false;
                }

                public static XElement GetWhiteList_XML(BindingList<WhiteListInfo> wliList)
                {
                    try
                    {
                        XElement xeWhiteList = new XElement("WhiteList");

                        foreach (WhiteListInfo wli in wliList)
                        {
                            XElement xeWhite =
                                new XElement("White",
                                new XElement("IPAddress", wli.IPAddress),
                                new XElement("IsExpiry", wli.IsExpiry),
                                new XElement("ExpiryTime", wli.ExpiryTime.ToString("yyyy/MM/dd HH:mm:ss")),
                                new XElement("CreateTime", wli.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                );

                            xeWhiteList.Add(xeWhite);
                        }

                        return xeWhiteList;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetWhiteList_XML), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//保存黑名单到文件（对话框）

                public static void SaveBlackList_Dialog(Form form, string FileName, BindingList<BlackListInfo> bliList)
                {
                    try
                    {
                        if (ProxyConfig.Proxy.lstBlackList.Count > 0)
                        {
                            SaveFileDialog sfdSaveFile = new SaveFileDialog();
                            sfdSaveFile.Filter = AntdUI.Localization.Get("FireWallSetting.BlackListFile", "黑名单文件") + "（*.bl）|*.bl";

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveFile.FileName = FileName;
                            }

                            sfdSaveFile.RestoreDirectory = true;
                            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveFile.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("FireWallSetting.BlackListFile.Export", "导出黑名单"));

                                    if (SaveBlackList(FilePath, bliList, EncryptPassword.DoEncrypt, EncryptPassword.Password))
                                    {
                                        string Title = AntdUI.Localization.Get("FireWallSetting.BlackListFile.Export.Success", "导出黑名单成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SaveBlackList_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("FireWallSetting.BlackListFile.Export.Fail", "导出黑名单失败");
                                        string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveBlackList_Dialog), ex.Message);
                    }
                }

                private static bool SaveBlackList(string FilePath, BindingList<BlackListInfo> bliList, bool DoEncrypt, string Password)
                {
                    try
                    {
                        XDocument xdoc = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };

                        XElement xeBlackList = ProxyConfig.Proxy.GetBlackList_XML(bliList);
                        if (xeBlackList == null)
                        {
                            return false;
                        }

                        xdoc.Add(xeBlackList);
                        xdoc.Save(FilePath);

                        if (DoEncrypt)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                SystemConfig.EncryptXMLFile(FilePath, Password);
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveBlackList), ex.Message);
                    }

                    return false;
                }

                public static XElement GetBlackList_XML(BindingList<BlackListInfo> bliList)
                {
                    try
                    {
                        XElement xeBlackList = new XElement("BlackList");

                        foreach (BlackListInfo bli in bliList)
                        {
                            XElement xeBlack =
                                new XElement("Black",
                                new XElement("IPAddress", bli.IPAddress),
                                new XElement("IsExpiry", bli.IsExpiry),
                                new XElement("ExpiryTime", bli.ExpiryTime.ToString("yyyy/MM/dd HH:mm:ss")),
                                new XElement("CreateTime", bli.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                );

                            xeBlackList.Add(xeBlack);
                        }

                        return xeBlackList;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetBlackList_XML), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//从文件加载白名单（对话框）

                public static void LoadWhiteList_Dialog(Form form)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();
                        ofdLoadFile.Filter = AntdUI.Localization.Get("FireWallSetting.WhiteListFile", "白名单文件") + "（*.wl）|*.wl";
                        ofdLoadFile.RestoreDirectory = true;

                        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = ofdLoadFile.FileName;
                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                if (LoadWhiteList(form, FilePath, true))
                                {
                                    string Title = AntdUI.Localization.Get("FireWallSetting.WhiteListFile.Import.Success", "导入白名单成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(nameof(LoadWhiteList_Dialog), Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadWhiteList_Dialog), ex.Message);
                    }
                }

                private static bool LoadWhiteList(Form form, string FilePath, bool LoadFromUser)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            XDocument xdoc = null;

                            bool bEncrypt = SystemConfig.IsEncryptXMLFile(FilePath);
                            if (bEncrypt)
                            {
                                if (LoadFromUser)
                                {
                                    xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("FireWallSetting.WhiteListFile.Import", "导入白名单"), FilePath);
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("Password.Incorrect", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(nameof(LoadWhiteList), sError);
                                }

                                return false;
                            }

                            LoadWhiteList_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadWhiteList), ex.Message);
                    }

                    return false;
                }

                public static void LoadWhiteList_FromXDocument(XDocument xdoc)
                {
                    try
                    {
                        foreach (XElement xeWhiteList in xdoc.Root.Elements())
                        {
                            string IPAddress = string.Empty;
                            if (xeWhiteList.Element("IPAddress") != null)
                            {
                                IPAddress = xeWhiteList.Element("IPAddress").Value;
                            }

                            bool IsExpiry = false;
                            if (xeWhiteList.Element("IsExpiry") != null)
                            {
                                IsExpiry = bool.Parse(xeWhiteList.Element("IsExpiry").Value);
                            }

                            DateTime ExpiryTime = DateTime.Now;
                            if (xeWhiteList.Element("ExpiryTime") != null)
                            {
                                ExpiryTime = DateTime.Parse(xeWhiteList.Element("ExpiryTime").Value);
                            }

                            DateTime CreateTime = DateTime.Now;
                            if (xeWhiteList.Element("CreateTime") != null)
                            {
                                CreateTime = DateTime.Parse(xeWhiteList.Element("CreateTime").Value);
                            }

                            ProxyConfig.Proxy.AddToWhiteList(IPAddress, IsExpiry, ExpiryTime, CreateTime);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadWhiteList_FromXDocument), ex.Message);
                    }
                }

                #endregion

                #region//从文件加载黑名单（对话框）

                public static void LoadBlackList_Dialog(Form form)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();
                        ofdLoadFile.Filter = AntdUI.Localization.Get("FireWallSetting.BlackListFile", "黑名单文件") + "（*.bl）|*.bl";
                        ofdLoadFile.RestoreDirectory = true;

                        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = ofdLoadFile.FileName;
                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                if (LoadBlackList(form, FilePath, true))
                                {
                                    string Title = AntdUI.Localization.Get("FireWallSetting.BlackListFile.Import.Success", "导入黑名单成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(nameof(LoadBlackList_Dialog), Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadBlackList_Dialog), ex.Message);
                    }
                }

                private static bool LoadBlackList(Form form, string FilePath, bool LoadFromUser)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            XDocument xdoc = null;

                            bool bEncrypt = SystemConfig.IsEncryptXMLFile(FilePath);
                            if (bEncrypt)
                            {
                                if (LoadFromUser)
                                {
                                    xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("FireWallSetting.BlackListFile.Import", "导入黑名单"), FilePath);
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("Password.Incorrect", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(nameof(LoadBlackList), sError);
                                }

                                return false;
                            }

                            LoadBlackList_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadBlackList), ex.Message);
                    }

                    return false;
                }

                public static void LoadBlackList_FromXDocument(XDocument xdoc)
                {
                    try
                    {
                        foreach (XElement xeBlackList in xdoc.Root.Elements())
                        {
                            string IPAddress = string.Empty;
                            if (xeBlackList.Element("IPAddress") != null)
                            {
                                IPAddress = xeBlackList.Element("IPAddress").Value;
                            }

                            bool IsExpiry = false;
                            if (xeBlackList.Element("IsExpiry") != null)
                            {
                                IsExpiry = bool.Parse(xeBlackList.Element("IsExpiry").Value);
                            }

                            DateTime ExpiryTime = DateTime.Now;
                            if (xeBlackList.Element("ExpiryTime") != null)
                            {
                                ExpiryTime = DateTime.Parse(xeBlackList.Element("ExpiryTime").Value);
                            }

                            DateTime CreateTime = DateTime.Now;
                            if (xeBlackList.Element("CreateTime") != null)
                            {
                                CreateTime = DateTime.Parse(xeBlackList.Element("CreateTime").Value);
                            }

                            ProxyConfig.Proxy.AddToBlackList(IPAddress, IsExpiry, ExpiryTime, CreateTime);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadBlackList_FromXDocument), ex.Message);
                    }
                }

                #endregion                
            }

            #endregion

            #region//代理队列

            public static class Queue
            {
                public static ConcurrentQueue<ProxyInfo> qProxyInfo = new ConcurrentQueue<ProxyInfo>();                

                #region//代理数据入队列

                public static Task ProxyInfo_ToQueue(
                    DateTime dtNow,
                    Operate.FilterConfig.Filter.FilterAction filterAction,
                    int res,
                    int PacketSocket,
                    PacketConfig.Packet.PacketType PacketType,
                    string ClientAddr,
                    string ServerAddr,
                    string ServerDomain,
                    ProxyConfig.Proxy.DomainType DomainType,
                    byte[] bRawBuffer,
                    byte[] bBuffer)
                {
                    if (filterAction == Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay)
                        return Task.CompletedTask;

                    if (filterAction != Operate.FilterConfig.Filter.FilterAction.Intercept && res <= 0)
                        return Task.CompletedTask;

                    return Task.Run(async () =>
                    {
                        try
                        {
                            switch (PacketType)
                            {
                                case PacketConfig.Packet.PacketType.TCP_Req:
                                    ProxyConfig.Proxy.TCP_Req_CNT++;
                                    Interlocked.Add(ref ProxyConfig.Proxy.Total_Request, bBuffer.Length);
                                    Interlocked.Add(ref Operate.ProxyConfig.Proxy.ProxySpeed_Uplink, bBuffer.Length);
                                    break;

                                case PacketConfig.Packet.PacketType.TCP_Resp:
                                    ProxyConfig.Proxy.TCP_Resp_CNT++;
                                    Interlocked.Add(ref ProxyConfig.Proxy.Total_Response, bBuffer.Length);
                                    Interlocked.Add(ref Operate.ProxyConfig.Proxy.ProxySpeed_Downlink, bBuffer.Length);
                                    break;
                            }                            

                            if (!SystemConfig.SpeedMode)
                            {
                                string ClientLocation = await SystemConfig.GetIPLocation(ClientAddr.Split(':')[0]);
                                string ServerLocation = await SystemConfig.GetIPLocation(ServerAddr.Split(':')[0]);

                                ProxyInfo pi = new ProxyInfo(
                                    dtNow,
                                    PacketSocket,
                                    PacketType,
                                    ClientAddr,
                                    ClientLocation,
                                    ServerAddr,
                                    ServerLocation,
                                    ServerDomain,
                                    DomainType,
                                    bRawBuffer,
                                    bBuffer,
                                    bBuffer.Length,
                                    filterAction);

                                qProxyInfo.Enqueue(pi);
                            }
                        }
                        catch (Exception ex)
                        {
                            Operate.DoLog(nameof(ProxyInfo_ToQueue), ex.Message);
                        }
                    });                    
                }

                #endregion

                #region//清除队列数据                  

                public static void ClearProxyInfoQueue()
                {
                    try
                    {
                        while (!qProxyInfo.IsEmpty)
                        {
                            qProxyInfo.TryDequeue(out ProxyInfo spd);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ClearProxyInfoQueue), ex.Message);
                    }
                }

                #endregion
            }

            #endregion

            #region//代理列表

            public static class List
            {
                public static bool IsShow_ID = true;
                public static bool IsShow_ProxyTime = true;
                public static bool IsShow_PacketType = true;
                public static bool IsShow_PacketSocket = true;
                public static bool IsShow_ClientAddr = true;
                public static bool IsShow_ClientLocation = true;
                public static bool IsShow_ServerAddr = true;
                public static bool IsShow_ServerLocation = true;
                public static bool IsShow_PacketLen = true;
                public static bool IsShow_PacketData = true;
                public static int Search_Index = -1;                
                public static ProxyInfo piSelect = null;
                public static int ClientNumber = 0;                

                public static readonly ConcurrentDictionary<Guid, ProxyUDP> cdProxyUDP = new ConcurrentDictionary<Guid, ProxyUDP>();
                public static readonly TimeSpan UDPTimeout = TimeSpan.FromMinutes(5);

                public static BindingList<ProxyInfo> lstProxyInfo = new BindingList<ProxyInfo>();

                #region//代理数据入列表

                public static void ProxyInfo_ToList()
                {
                    try
                    {
                        if (ProxyConfig.Queue.qProxyInfo.TryDequeue(out ProxyInfo pi))
                        {
                            bool bIsShow = PacketConfig.Packet.IsShowProxy_ByFilter(pi);
                            if (bIsShow)
                            {
                                Span<byte> bufferSpan = pi.PacketBuffer.AsSpan();
                                pi.PacketData = PacketConfig.Packet.GetPacketData_Hex(pi.PacketBuffer.AsSpan(), PacketConfig.Packet.PacketData_MaxLen);

                                if (Operate.SystemConfig.InvokeAction != null)
                                {
                                    Operate.SystemConfig.InvokeAction(() =>
                                    {
                                        Operate.ProxyConfig.List.lstProxyInfo.Add(pi);
                                    });
                                }
                                else
                                {
                                    Operate.ProxyConfig.List.lstProxyInfo.Add(pi);
                                }
                            }
                            else
                            {
                                ProxyConfig.Proxy.FilterProxy_CNT++;
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ProxyInfo_ToList), ex.Message);
                    }
                }

                #endregion

                #region//清除代理数据列表

                public static void ClearProxyInfo()
                {
                    try
                    {
                        if (Operate.SystemConfig.InvokeAction != null)
                        {
                            Operate.SystemConfig.InvokeAction(() =>
                            {
                                ProxyConfig.List.lstProxyInfo.Clear();
                            });
                        }
                        else
                        {
                            ProxyConfig.List.lstProxyInfo.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ClearProxyInfo), ex.Message);
                    }
                }

                #endregion                

                #region//保存代理列表为Excel（对话框）

                public static void SaveProxyList_Dialog(Form form, string FileName, List<ProxyInfo> piList)
                {
                    try
                    {
                        if (ProxyConfig.List.lstProxyInfo.Count > 0)
                        {
                            int SaveCount = ProxyConfig.List.lstProxyInfo.Count;

                            SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                            sfdSaveToExcel.Filter = AntdUI.Localization.Get("ExcelFile", "Excel 文件") + "Excel (*.xls)|*.xls";
                            sfdSaveToExcel.RestoreDirectory = true;

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveToExcel.FileName = FileName;
                            }

                            if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveToExcel.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    bool bOK = ProxyConfig.List.SaveProxyListToExcel(FilePath, piList);
                                    if (bOK)
                                    {
                                        string Title = AntdUI.Localization.Get("ExportToExcel.Success", "导出到Excel成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SaveProxyList_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("ExportToExcel.Error", "导出到Excel失败");
                                        string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveProxyList_Dialog), ex.Message);
                    }
                }

                private static bool SaveProxyListToExcel(string filePath, List<ProxyInfo> piList)
                {
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        using (var writer = new StreamWriter(stream, Encoding.Default))
                        {
                            writer.WriteLine(AntdUI.Localization.Get("ExcelColumn.Proxy", "时间戳\t类别\t套接字\t客户端地址\t服务端地址\t长度\t数据\t"));

                            var dataSource = piList.Count > 0 ? piList : ProxyConfig.List.lstProxyInfo.ToList();
                            foreach (var proxy in dataSource)
                            {
                                try
                                {
                                    var lineBuilder = new StringBuilder();

                                    lineBuilder.Append(proxy.ProxyTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff")).Append('\t');
                                    lineBuilder.Append(proxy.PacketType).Append('\t');
                                    lineBuilder.Append(proxy.PacketSocket).Append('\t');
                                    lineBuilder.Append(proxy.ClientAddr).Append('\t');
                                    lineBuilder.Append(proxy.ServerAddr).Append('\t');
                                    lineBuilder.Append(proxy.PacketLen).Append('\t');
                                    lineBuilder.Append(SystemConfig.BytesToString(PacketConfig.Packet.EncodingFormat.Hex, proxy.PacketBuffer)).Append('\t');

                                    writer.WriteLine(lineBuilder.ToString());
                                }
                                catch (Exception ex)
                                {
                                    Operate.DoLog(nameof(SaveProxyListToExcel), ex.Message);
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveProxyListToExcel), ex.Message);
                        return false;
                    }
                }

                #endregion
            }

            #endregion

            #region//代理账号

            public static class Account
            {    
                public static string CCProxy_HTML = string.Empty;

                public static BindingList<AccountInfo> lstAccountInfo = new BindingList<AccountInfo>();
                public static ConcurrentDictionary<(Guid AID, string AuthIP), AuthInfo> cdAuthInfo = new ConcurrentDictionary<(Guid, string), AuthInfo>();

                #region//代理认证入列表（异步）

                public static async Task AuthInfo_ToList(Guid AID, string AuthIP, bool AuthResult)
                {
                    try
                    {
                        if (AID == Guid.Empty) return;
                        if (string.IsNullOrEmpty(AuthIP)) return;

                        var key = (AID, AuthIP);
                        string IPLocation = await SystemConfig.GetIPLocation(AuthIP).ConfigureAwait(false);

                        cdAuthInfo.AddOrUpdate(
                            key,
                            _ => new AuthInfo(AID, AuthIP, IPLocation, AuthResult, DateTime.Now),
                            (_, existing) =>
                            {
                                return existing;
                            });
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AuthInfo_ToList), ex.Message);
                    }
                }

                #endregion

                #region//查找代理认证

                public static AuthInfo GetProxyAuthInfo(Guid AID, string IPAddress)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(IPAddress) || AID == Guid.Empty)
                        {
                            return null;
                        }

                        if (ProxyConfig.Account.cdAuthInfo.TryGetValue((AID, IPAddress), out var authInfo))
                        {
                            return authInfo;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetProxyAuthInfo), ex.Message);                        
                    }

                    return null;
                }

                #endregion

                #region//删除代理认证

                public static void DeleteProxyAuthInfo_ByAIDAndIP(Guid AID, string IPAddress)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(IPAddress) || AID == Guid.Empty)
                        {
                            return;
                        }

                        var key = (AID, IPAddress);
                        ProxyConfig.Account.cdAuthInfo.TryRemove(key, out _);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteProxyAuthInfo_ByAIDAndIP), ex.Message);
                    }
                }

                #endregion

                #region//编辑账号

                public static void OpenAccountEdit(Form form, AccountInfo ai)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("AccountEditForm", "账号编辑"), new AccountEdit(form, ai))
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//批量创建账号

                public static void BatchAddAccounts(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("AccountList.BatchAdd", "批量创建账号"), new BatchAccounts(form))
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//生成随机密码

                private static readonly Random rPW = new Random();

                public static string RandomPassword(int length)
                {
                    const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                    return new string(Enumerable.Repeat(validChars, length)
                        .Select(s => s[rPW.Next(s.Length)])
                        .ToArray());
                }

                #endregion

                #region//获取代理认证列表的信息

                public static int GetLinksCount_FromAuthList()
                {
                    try
                    {
                        return ProxyConfig.Account.cdAuthInfo.Values.Sum(proxy => proxy.LinksNumber);
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(GetLinksCount_FromAuthList), ex.Message);
                        return 0;
                    }
                }

                public static int GetDevicesCount_FromAuthList()
                {
                    try
                    {
                        return ProxyConfig.Account.cdAuthInfo.Values
                            .GroupBy(proxy => proxy.AID)
                            .Sum(group => group.First().DevicesNumber);
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(GetDevicesCount_FromAuthList), ex.Message);
                        return 0;
                    }
                }

                #endregion                

                #region//验证远程管理的账号密码

                public static bool IsValidAdmin(string username, string password)
                {
                    bool bReturn = false;

                    try
                    {
                        if (SystemConfig.Remote_UserName.Equals(username) && SystemConfig.Remote_PassWord.Equals(password))
                        {
                            bReturn = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(IsValidAdmin), ex.Message);
                    }

                    return bReturn;
                }

                #endregion

                #region//检测代理账号是否已存在

                public static bool CheckProxyAccount_Exist(string UserName)
                {
                    try
                    {
                        foreach (AccountInfo ai in ProxyConfig.Account.lstAccountInfo)
                        {
                            if (ai.UserName.Equals(UserName))
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckProxyAccount_Exist), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//检测用户名和密码是否正确（区分大小写）

                public static (bool ok, Guid accountId) CheckUserNameAndPassWord(string userName, string passWord)
                {
                    try
                    {
                        string pwEncrypt = SystemConfig.PassWord_Encrypt(passWord);

                        foreach (var acc in ProxyConfig.Account.lstAccountInfo)
                        {
                            if (!acc.IsEnable) continue;
                            if (!acc.UserName.Equals(userName)) continue;
                            if (!acc.Password.Equals(pwEncrypt)) continue;

                            if (acc.IsExpiry && acc.ExpiryTime <= DateTime.Now)
                                return (false, Guid.Empty);

                            return (true, acc.AID);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckUserNameAndPassWord), ex.Message);
                    }

                    return (false, Guid.Empty);
                }

                #endregion

                #region//检测是否已超过限制链接数

                public static bool CheckLimitLinks(Guid AID, string IPAddress)
                {
                    try
                    {
                        if (AID != null && AID != Guid.Empty)
                        {
                            AccountInfo paiAccount = ProxyConfig.Account.GetProxyAccount_ByAccountID(AID);
                            AuthInfo paiAuth = ProxyConfig.Account.GetProxyAuthInfo(AID, IPAddress);

                            if (paiAccount != null && paiAuth != null)
                            {
                                if (paiAccount.IsLimitLinks)
                                {
                                    int LimitLinks = paiAccount.LimitLinks;
                                    int LinksNumber = paiAuth.LinksNumber;

                                    if (LinksNumber >= LimitLinks)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckLimitLinks), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//检测是否已超过限制设备数

                public static bool CheckLimitDevices(Guid AID, string ClientIP)
                {
                    try
                    {
                        if (AID != null && AID != Guid.Empty)
                        {
                            AccountInfo paiAccount = ProxyConfig.Account.GetProxyAccount_ByAccountID(AID);
                            if (paiAccount != null)
                            {
                                if (paiAccount.IsLimitDevices)
                                {
                                    int DevicesNumber = ProxyConfig.Account.GetDevicesNumber_ByAccountID(AID);

                                    if (DevicesNumber < paiAccount.LimitDevices)
                                    {
                                        return false;
                                    }
                                    else if (DevicesNumber == paiAccount.LimitDevices)
                                    {
                                        AuthInfo pai = ProxyConfig.Account.GetProxyAuthInfo(AID, ClientIP);

                                        if (pai != null)
                                        {
                                            return false;
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckLimitDevices), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//设置代理账号的在线情况

                public static void SetOnline_ByAccountID(Guid AID, bool IsOnline)
                {
                    try
                    {
                        AccountInfo pai = ProxyConfig.Account.GetProxyAccount_ByAccountID(AID);
                        if (pai != null)
                        {
                            if (IsOnline)
                            {
                                pai.IsOnLine = true;
                            }
                            else
                            {
                                int DevicesNumber = GetDevicesNumber_ByAccountID(AID);
                                if (DevicesNumber == 0)
                                {
                                    pai.IsOnLine = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SetOnline_ByAccountID), ex.Message);
                    }
                }

                #endregion

                #region//获取在线的代理账号数

                public static int GetOnLineProxyAccountCount(BindingList<AccountInfo> allData)
                {
                    int iReturn = 0;

                    try
                    {
                        foreach (AccountInfo pai in allData)
                        {
                            if (pai.IsOnLine)
                            {
                                iReturn++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(GetOnLineProxyAccountCount), ex.Message);
                    }

                    return iReturn;
                }

                #endregion

                #region//获取代理账号的链接数

                public static int GetLinksNumber_ByAccountID(Guid AID, string ClientIP, AntdUI.Tree tree)
                {
                    try
                    {
                        var sessions = Operate.ProxyConfig.Proxy.ProxyServer.GetAllSessions();
                        var SessionList = sessions?.ToList() ?? new List<ProxySession>();

                        int LinksNumber = 0;
                        foreach (ProxySession Session in SessionList)
                        {
                            if (Session.CommandType != Operate.ProxyConfig.Proxy.CommandType.Bind)
                            {
                                if (Session.AID == AID && Session.ClientIP.Equals(ClientIP))
                                {
                                    LinksNumber++;
                                }
                            }
                        }

                        return LinksNumber;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetLinksNumber_ByAccountID), ex.Message);
                    }

                    return 0;
                }

                #endregion

                #region//获取代理账号登录的设备数

                public static int GetDevicesNumber_ByAccountID(Guid AID)
                {
                    try
                    {
                        if (AID == Guid.Empty)
                            return 0;

                        var sessions = Operate.ProxyConfig.Proxy.ProxyServer.GetAllSessions();
                        var SessionList = sessions?.ToList() ?? new List<ProxySession>();

                        List<string> lstIPAddress = new List<string>();
                        foreach (ProxySession Session in SessionList)
                        {
                            if (Session.CommandType != Operate.ProxyConfig.Proxy.CommandType.Bind)
                            {
                                if (Session.AID == AID)
                                {
                                    lstIPAddress.Add(Session.ClientIP);
                                }
                            }
                        }

                        return lstIPAddress.Distinct().Count();
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(GetDevicesNumber_ByAccountID), ex.Message);                        
                    }

                    return 0;
                }

                #endregion

                #region//获取账号列表的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_AccountList()
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();
                                        
                    menuItems.Add(new AntdUI.ContextMenuStripItem("批量调整")
                    {
                        ID = "Adjust",                        
                        IconSvg = "UnorderedListOutlined",
                        LocalizationText = "AccountList.BatchAdjustment",
                        Sub = new AntdUI.IContextMenuStripItem[]
                        {
                            new AntdUI.ContextMenuStripItem("过期时间")
                            {
                                ID = "ExpiryTime",
                                IconSvg = "FieldTimeOutlined",
                                LocalizationText = "AccountList.ExpiryTime",
                            },
                            new AntdUI.ContextMenuStripItem("链接数")
                            {
                                ID = "LimitLinks",
                                IconSvg = "ForkOutlined",
                                LocalizationText = "AccountList.LimitLinks",
                            },
                            new AntdUI.ContextMenuStripItem("设备数")
                            {
                                ID = "LimitDevices",
                                IconSvg = "TabletOutlined",
                                LocalizationText = "AccountList.LimitDevices",
                            },
                        },
                    });                    
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());                    
                    menuItems.Add(new AntdUI.ContextMenuStripItem("批量导出")
                    {
                        ID = "Export",
                        IconSvg = "DeliveredProcedureOutlined",
                        LocalizationText = "AccountList.BatchExport",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("批量删除")
                    {
                        ID = "Delete",
                        IconSvg = "DeleteOutlined",
                        LocalizationText = "AccountList.Delete",
                    });

                    return menuItems.ToArray();
                }

                #endregion                

                #region//获取认证列表的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_AuthList()
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                    menuItems.Add(new AntdUI.ContextMenuStripItem("加入白名单")
                    {
                        ID = "WhiteList_Permanent",
                        IconSvg = "EyeOutlined",
                        LocalizationText = "FireWallSetting.WhiteList.Add",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("加入黑名单")
                    {
                        ID = "BlackList",
                        IconSvg = "EyeInvisibleOutlined",
                        LocalizationText = "FireWallSetting.BlackList.Add",
                        Sub = new AntdUI.IContextMenuStripItem[]
                        {
                            new AntdUI.ContextMenuStripItem("屏蔽 1 小时")
                            {
                                ID = "BlackList_1Hour",
                                LocalizationText = "FireWallSetting.BlackList.1Hour",
                            },
                            new AntdUI.ContextMenuStripItemDivider(),
                            new AntdUI.ContextMenuStripItem("屏蔽 1 天")
                            {
                                ID = "BlackList_1Day",
                                LocalizationText = "FireWallSetting.BlackList.1Day",
                            },
                            new AntdUI.ContextMenuStripItemDivider(),
                            new AntdUI.ContextMenuStripItem("屏蔽 30 天")
                            {
                                ID = "BlackList_30Day",
                                LocalizationText = "FireWallSetting.BlackList.30Day",
                            },
                            new AntdUI.ContextMenuStripItemDivider(),
                            new AntdUI.ContextMenuStripItem("永久屏蔽")
                            {
                                ID = "BlackList_Permanent",                                
                                LocalizationText = "FireWallSetting.BlackList.Permanent",
                            },
                        },
                    });

                    return menuItems.ToArray();
                }

                #endregion

                #region//记录代理账号的IP地址（异步）

                public static async Task IPInfo_ToAccount(Guid AccountID, string IPAddress)
                {
                    try
                    {
                        if (AccountID != Guid.Empty && !string.IsNullOrEmpty(IPAddress))
                        {
                            AccountInfo ai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(item => item.AID == AccountID);
                            if (ai != null)
                            {
                                AccountIPInfo aii = ai.AIPInfo.FirstOrDefault(item => item.LoginIP == IPAddress);
                                if (aii == null)
                                {
                                    string IPLocation = await SystemConfig.GetIPLocation(IPAddress);
                                    aii = new AccountIPInfo(DateTime.Now, IPAddress, IPLocation);
                                    ai.AIPInfo.Add(aii);
                                }
                                else
                                {
                                    aii.LoginTime = DateTime.Now;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(IPInfo_ToAccount), ex.Message);
                    }
                }

                #endregion

                #region//新增代理账号

                public static void AddProxyAccount(bool SaveToDB, AccountInfo ai)
                {
                    try
                    {
                        if (!ProxyConfig.Account.CheckProxyAccount_Exist(ai.UserName))
                        {
                            ProxyConfig.Account.ProxyAccountToList(ai);

                            if (SaveToDB)
                            {
                                DataBase.InsertTable_ProxyAccount(ai);
                            }
                        }                            
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddProxyAccount), ex.Message);
                    }                    
                }

                public static bool AddProxyAccount(
                    bool SaveToDB,
                    Guid AID,
                    bool IsEnable,
                    string UserName,
                    string PassWord,
                    BindingList<AccountIPInfo> AIPInfo,
                    bool IsLimitLinks,
                    int LimitLinks,
                    bool IsLimitDevices,
                    int LimitDevices,
                    bool IsExpiry,
                    DateTime ExpiryTime,
                    DateTime CreateTime)
                {
                    try
                    {
                        if (AID != Guid.Empty && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PassWord))
                        {
                            if (!ProxyConfig.Account.CheckProxyAccount_Exist(UserName))
                            {
                                AccountInfo ai = new AccountInfo(
                                    AID,
                                    IsEnable,
                                    UserName,
                                    PassWord,
                                    AIPInfo,
                                    IsLimitLinks,
                                    LimitLinks,
                                    IsLimitDevices,
                                    LimitDevices,
                                    IsExpiry,
                                    ExpiryTime,
                                    CreateTime);

                                ProxyConfig.Account.ProxyAccountToList(ai);

                                if (SaveToDB)
                                {
                                    DataBase.InsertTable_ProxyAccount(ai);
                                }                                

                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddProxyAccount), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//新增代理账号IP信息

                public static async void AddAccountIPInfo(BindingList<AccountIPInfo> AIPInfo, DateTime LoginTime, string LoginIP)
                {
                    try
                    {
                        string IPLocation = await SystemConfig.GetIPLocation(LoginIP);

                        AccountIPInfo aii = new AccountIPInfo(LoginTime, LoginIP, IPLocation);
                        AIPInfo.Add(aii);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddAccountIPInfo), ex.Message);
                    }
                }

                #endregion

                #region//更新代理账号            

                public static bool UpdateProxyAccount_ByAccountID(
                    Guid AID,
                    bool IsEnable,
                    string PassWord,
                    bool IsLimitLinks,
                    int LimitLinks,
                    bool IsLimitDevices,
                    int LimitDevices,
                    bool IsExpiry,
                    DateTime ExpiryTime)
                {
                    try
                    {
                        if (AID != null)
                        {
                            AccountInfo ai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.AID == AID);

                            if (ai != null)
                            {
                                ai.IsEnable = IsEnable;

                                if (!string.IsNullOrEmpty(PassWord))
                                {
                                    ai.Password = PassWord;
                                }

                                ai.IsLimitLinks = IsLimitLinks;
                                ai.LimitLinks = LimitLinks;
                                ai.IsExpiry = IsExpiry;
                                ai.IsLimitDevices = IsLimitDevices;
                                ai.LimitDevices = LimitDevices;
                                ai.ExpiryTime = ExpiryTime;

                                DataBase.UpdateTable_ProxyAccount(ai);

                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateProxyAccount_ByAccountID), ex.Message);
                    }

                    return false;
                }

                public static bool UpdateProxyAccount_ByCCProxy(
                    string UserName,
                    bool IsEnable,
                    string PassWord,
                    bool IsLimitLinks,
                    int LimitLinks,
                    bool IsExpiry,
                    DateTime ExpiryTime)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(UserName))
                        {
                            AccountInfo ai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.UserName == UserName);

                            if (ai != null)
                            {
                                ai.IsEnable = IsEnable;

                                if (!string.IsNullOrEmpty(PassWord))
                                {
                                    ai.Password = PassWord;
                                }

                                ai.IsLimitLinks = IsLimitLinks;
                                ai.LimitLinks = LimitLinks;
                                ai.IsExpiry = IsExpiry;
                                ai.ExpiryTime = ExpiryTime;

                                DataBase.UpdateTable_ProxyAccount(ai);

                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateProxyAccount_ByCCProxy), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//删除代理账号（对话框）                

                public static void DeleteAccount_Dialog(Form form, List<AccountInfo> aiList)
                {
                    try
                    {
                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("AccountList", "账号列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                        {
                            Icon = TType.Warn,
                            Keyboard = false,
                            MaskClosable = false,
                            OnOk = config =>
                            {
                                if (aiList == null)
                                {
                                    ProxyConfig.Account.AccountListClear();                                    
                                }
                                else
                                {
                                    foreach (AccountInfo ai in aiList)
                                    {
                                        ProxyConfig.Account.lstAccountInfo.Remove(ai);
                                        DataBase.DeleteTable_ProxyAccount(ai.AID);
                                    }
                                }

                                if (form is InterfaceInfo.IProxyMode pmForm)
                                {
                                    pmForm.RefreshAccountList();
                                }

                                return true;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteAccount_Dialog), ex.Message);
                    }
                }

                public static bool DeleteProxyAccount_ByAccountID(Guid AID)
                {
                    try
                    {
                        if (AID != null)
                        {
                            AccountInfo ai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.AID == AID);

                            if (ai != null)
                            {
                                ProxyConfig.Account.lstAccountInfo.Remove(ai);
                                DataBase.DeleteTable_ProxyAccount(ai.AID);
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteProxyAccount_ByAccountID), ex.Message);
                    }

                    return false;
                }

                public static bool DeleteProxyAccount_ByUserName(string UserName)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(UserName))
                        {
                            AccountInfo ai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.UserName == UserName);

                            if (ai != null)
                            {
                                ProxyConfig.Account.lstAccountInfo.Remove(ai);
                                DataBase.DeleteTable_ProxyAccount(ai.AID);
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteProxyAccount_ByUserName), ex.Message);
                    }

                    return false;
                }

                public static void AccountListClear()
                {
                    try
                    {
                        ProxyConfig.Account.lstAccountInfo.Clear();
                        DataBase.DeleteTable_ProxyAccount();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AccountListClear), ex.Message);
                    }
                }

                #endregion

                #region//查找代理账号

                public static string GetUserName_ByAccountID(Guid AID)
                {
                    try
                    {
                        if (AID != null)
                        {
                            AccountInfo pai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.AID == AID);

                            if (pai != null)
                            {
                                return pai.UserName;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetUserName_ByAccountID), ex.Message);
                    }

                    return string.Empty;
                }

                public static AccountInfo GetProxyAccount_ByAccountID(Guid AID)
                {
                    try
                    {
                        if (AID != null)
                        {
                            AccountInfo pai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.AID == AID);

                            if (pai != null)
                            {
                                return pai;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetProxyAccount_ByAccountID), ex.Message);
                    }

                    return null;
                }

                public static BindingList<AccountInfo> GetAccount_ByUserName(string UserName)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(UserName))
                        {
                            BindingList<AccountInfo> pai = new BindingList<AccountInfo>
                                (ProxyConfig.Account.lstAccountInfo.Where(account => account.UserName.Contains(UserName)).ToList());

                            return pai;
                        }
                        else
                        {
                            return ProxyConfig.Account.lstAccountInfo;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetAccount_ByUserName), ex.Message);
                    }

                    return null;
                }

                public static BindingList<AccountInfo> GetProxyAccount_ByIsEnable(bool IsEnable)
                {
                    try
                    {
                        BindingList<AccountInfo> pai = new BindingList<AccountInfo>
                            (ProxyConfig.Account.lstAccountInfo.Where(account => account.IsEnable == IsEnable).ToList());

                        return pai;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetProxyAccount_ByIsEnable), ex.Message);
                    }

                    return null;
                }

                public static BindingList<AccountInfo> GetProxyAccount_ByIsOnLine(bool IsOnLine)
                {
                    try
                    {
                        BindingList<AccountInfo> pai = new BindingList<AccountInfo>
                            (ProxyConfig.Account.lstAccountInfo.Where(account => account.IsOnLine == IsOnLine).ToList());

                        return pai;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetProxyAccount_ByIsOnLine), ex.Message);
                    }

                    return null;
                }

                public static BindingList<AccountInfo> GetProxyAccount_ByIsExpiry(bool IsExpiry)
                {
                    try
                    {
                        BindingList<AccountInfo> pai = new BindingList<AccountInfo>
                            (ProxyConfig.Account.lstAccountInfo.Where(account => account.IsExpiry == IsExpiry).ToList());

                        return pai;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetProxyAccount_ByIsExpiry), ex.Message);
                    }

                    return null;
                }

                public static BindingList<AccountInfo> GetProxyAccount_ByIsLimitLinks(bool IsLimitLinks)
                {
                    try
                    {
                        BindingList<AccountInfo> pai = new BindingList<AccountInfo>
                            (ProxyConfig.Account.lstAccountInfo.Where(account => account.IsLimitLinks == IsLimitLinks).ToList());

                        return pai;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetProxyAccount_ByIsLimitLinks), ex.Message);
                    }

                    return null;
                }

                public static BindingList<AccountInfo> GetProxyAccount_ByIsLimitDevices(bool IsLimitDevices)
                {
                    try
                    {
                        BindingList<AccountInfo> pai = new BindingList<AccountInfo>
                            (ProxyConfig.Account.lstAccountInfo.Where(account => account.IsLimitDevices == IsLimitDevices).ToList());

                        return pai;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetProxyAccount_ByIsLimitDevices), ex.Message);
                    }

                    return null;
                }

                public static BindingList<AccountInfo> GetProxyAccount_ByExpireTime(DateTime dtFrom, DateTime dtTo)
                {
                    try
                    {
                        BindingList<AccountInfo> pai = new BindingList<AccountInfo>
                            (ProxyConfig.Account.lstAccountInfo.Where(account => account.ExpiryTime >= dtFrom && account.ExpiryTime <= dtTo).ToList());

                        return pai;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetProxyAccount_ByExpireTime), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//代理账号入列表

                public static void ProxyAccountToList(AccountInfo ai)
                {
                    try
                    {
                        ProxyConfig.Account.lstAccountInfo.Add(ai);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ProxyAccountToList), ex.Message);
                    }
                }

                #endregion                

                #region//调整过期时间

                public static void AdjustExpiryTime(List<AccountInfo> aiList, int AddType, int AddHours)
                {
                    try
                    {
                        if (aiList.Count > 0)
                        {
                            DateTime dtExpiryTime = DateTime.Now;

                            foreach (AccountInfo ai in aiList)
                            {
                                switch (AddType)
                                {
                                    case 0:
                                        dtExpiryTime = ai.ExpiryTime.AddHours(AddHours);
                                        break;

                                    case 1:
                                        if (ai.ExpiryTime >= DateTime.Now)
                                        {
                                            dtExpiryTime = ai.ExpiryTime.AddHours(AddHours);
                                        }
                                        else
                                        {
                                            dtExpiryTime = DateTime.Now.AddHours(AddHours);
                                        }
                                        break;
                                }

                                ProxyConfig.Account.UpdateProxyAccount_ByAccountID(
                                    ai.AID, 
                                    ai.IsEnable,
                                    ai.Password,
                                    ai.IsLimitLinks,
                                    ai.LimitLinks,
                                    ai.IsLimitDevices,
                                    ai.LimitDevices,
                                    ai.IsExpiry,
                                    dtExpiryTime);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AdjustExpiryTime), ex.Message);
                    }
                }

                #endregion

                #region//调整链接数

                public static void AdjustLimitLinks(List<AccountInfo> aiList, bool IsLimitLinks, int LimitLinks)
                {
                    try
                    {
                        if (aiList.Count > 0)
                        {
                            foreach (AccountInfo ai in aiList)
                            {
                                ProxyConfig.Account.UpdateProxyAccount_ByAccountID(
                                    ai.AID,
                                    ai.IsEnable,
                                    ai.Password,
                                    IsLimitLinks,
                                    LimitLinks,
                                    ai.IsLimitDevices,
                                    ai.LimitDevices,
                                    ai.IsExpiry,
                                    ai.ExpiryTime);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AdjustLimitLinks), ex.Message);
                    }
                }

                #endregion

                #region//调整设备数

                public static void AdjustLimitDevices(List<AccountInfo> aiList, bool IsLimitDevices, int LimitDevices)
                {
                    try
                    {
                        if (aiList.Count > 0)
                        {
                            foreach (AccountInfo ai in aiList)
                            {
                                ProxyConfig.Account.UpdateProxyAccount_ByAccountID(
                                    ai.AID,
                                    ai.IsEnable,
                                    ai.Password,
                                    ai.IsLimitLinks,
                                    ai.LimitLinks,
                                    IsLimitDevices,
                                    LimitDevices,
                                    ai.IsExpiry,
                                    ai.ExpiryTime);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AdjustLimitDevices), ex.Message);
                    }
                }

                #endregion                

                #region//从数据库加载账号IP信息

                public static DataTable LoadAccountIPInfo_FromDB(Guid AID)
                {
                    DataTable dtReturn = null;

                    try
                    {
                        if (AID != Guid.Empty)
                        {
                            dtReturn = DataBase.SelectTable_ProxyAccountIPInfo(AID);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadAccountIPInfo_FromDB), ex.Message);
                    }

                    return dtReturn;
                }

                #endregion                

                #region//从数据库加载代理账号列表（异步）

                public static void LoadProxyAccountList_FromDB()
                {
                    try
                    {
                        DataTable dtProxyAccount = DataBase.SelectTable_ProxyAccount();
                        foreach (DataRow drProxyAccount in dtProxyAccount.Rows)
                        {
                            Guid AID = Guid.Parse(drProxyAccount["GUID"].ToString());
                            bool IsEnable = Convert.ToBoolean(drProxyAccount["IsEnable"]);
                            string UserName = drProxyAccount["UserName"].ToString();
                            string PassWord = drProxyAccount["PassWord"].ToString();                            
                            bool IsLimitLinks = Convert.ToBoolean(drProxyAccount["IsLimitLinks"]);
                            int LimitLinks = int.Parse(drProxyAccount["LimitLinks"].ToString());
                            bool IsLimitDevices = Convert.ToBoolean(drProxyAccount["IsLimitDevices"]);
                            int LimitDevices = int.Parse(drProxyAccount["LimitDevices"].ToString());
                            bool IsExpiry = Convert.ToBoolean(drProxyAccount["IsExpiry"]);
                            DateTime ExpiryTime = Convert.ToDateTime(drProxyAccount["ExpiryTime"]);
                            DateTime CreateTime = Convert.ToDateTime(drProxyAccount["CreateTime"]);

                            BindingList<AccountIPInfo> AIPInfo = new BindingList<AccountIPInfo>();
                            DataTable dtAIPInfo = DataBase.SelectTable_ProxyAccountIPInfo(AID);
                            foreach (DataRow drIPInfo in dtAIPInfo.Rows)
                            {
                                DateTime LoginTime = Convert.ToDateTime(drIPInfo["LoginTime"]);
                                string LoginIP = drIPInfo["LoginIP"].ToString();

                                ProxyConfig.Account.AddAccountIPInfo(AIPInfo, LoginTime, LoginIP);
                            }

                            ProxyConfig.Account.AddProxyAccount(
                                false,
                                AID,
                                IsEnable,
                                UserName,
                                PassWord,
                                AIPInfo,
                                IsLimitLinks,
                                LimitLinks,
                                IsLimitDevices,
                                LimitDevices,
                                IsExpiry,
                                ExpiryTime,
                                CreateTime);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadProxyAccountList_FromDB), ex.Message);
                    }
                }

                #endregion

                #region//保存代理账号列表到文件（对话框）                

                public static void SaveAccount_Dialog(Form form, string FileName, List<AccountInfo> aiList)
                {
                    try
                    {
                        SaveFileDialog sfdSaveFile = new SaveFileDialog();
                        sfdSaveFile.Filter = AntdUI.Localization.Get("ProxyAccountListFile", "代理账号列表文件") + "（*.pa）|*.pa";

                        if (!string.IsNullOrEmpty(FileName))
                        {
                            sfdSaveFile.FileName = FileName;
                        }

                        sfdSaveFile.RestoreDirectory = true;
                        if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = sfdSaveFile.FileName;
                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("ExportProxyAccountList", "导出代理账号列表"));
                           
                                if (SaveAccountList(FilePath, aiList, EncryptPassword.DoEncrypt, EncryptPassword.Password))
                                {
                                    string Title = AntdUI.Localization.Get("InjectModeForm.ExportProxyAccountList.Success", "导出代理账号列表成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(nameof(SaveAccount_Dialog), Title + ": " + FilePath);
                                }
                                else
                                {
                                    string Title = AntdUI.Localization.Get("InjectModeForm.ExportProxyAccountList.Error", "导出代理账号列表失败");
                                    string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                    AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveAccount_Dialog), ex.Message);
                    }
                }

                private static bool SaveAccountList(string FilePath, List<AccountInfo> aiList, bool DoEncrypt, string Password)
                {
                    try
                    {
                        XDocument xdoc = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };

                        XElement xeProxyAccountList = ProxyConfig.Account.GetAccountList_XML(aiList);
                        if (xeProxyAccountList == null)
                        {
                            return false;
                        }

                        xdoc.Add(xeProxyAccountList);
                        xdoc.Save(FilePath);

                        if (DoEncrypt)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                SystemConfig.EncryptXMLFile(FilePath, Password);
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveAccountList), ex.Message);
                    }

                    return false;
                }

                public static XElement GetAccountList_XML(List<AccountInfo> aiList)
                {
                    try
                    {
                        XElement xeProxyAccountList = new XElement("ProxyAccountList");

                        if (aiList == null)
                        {
                            if (ProxyConfig.Account.lstAccountInfo != null)
                            {
                                aiList = ProxyConfig.Account.lstAccountInfo.ToList();
                            }
                            else
                            {
                                aiList = new List<AccountInfo>();
                            }
                        }

                        foreach (AccountInfo ai in aiList)
                        {
                            if (ai == null)
                            {
                                continue;
                            }

                            XElement xeProxyAccount =
                                    new XElement("ProxyAccount",
                                    new XElement("IsEnable", ai.IsEnable.ToString()),
                                    new XElement("ID", ai.AID.ToString().ToUpper()),
                                    new XElement("UserName", ai.UserName),
                                    new XElement("PassWord", ai.Password),
                                    new XElement("IsLimitLinks", ai.IsLimitLinks),
                                    new XElement("LimitLinks", ai.LimitLinks),
                                    new XElement("IsLimitDevices", ai.IsLimitDevices),
                                    new XElement("LimitDevices", ai.LimitDevices),
                                    new XElement("IsExpiry", ai.IsExpiry),
                                    new XElement("ExpiryTime", ai.ExpiryTime.ToString("yyyy/MM/dd HH:mm:ss")),
                                    new XElement("CreateTime", ai.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                    );

                            if (ai.AIPInfo != null && ai.AIPInfo.Count > 0)
                            {
                                XElement xeAccountIPInfo = new XElement("AccountIPInfo");

                                foreach (AccountIPInfo aii in ai.AIPInfo)
                                {
                                    if (aii == null)
                                    {
                                        continue;
                                    }

                                    XElement xeIPInfo =
                                        new XElement("IPInfo",
                                        new XElement("LoginTime", aii.LoginTime),
                                        new XElement("LoginIP", aii.LoginIP)
                                        );

                                    xeAccountIPInfo.Add(xeIPInfo);
                                }

                                if (xeAccountIPInfo.HasElements)
                                {
                                    xeProxyAccount.Add(xeAccountIPInfo);
                                }
                            }

                            xeProxyAccountList.Add(xeProxyAccount);
                        }

                        return xeProxyAccountList;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetAccountList_XML), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//从文件加载代理账号列表（对话框）

                public static void LoadAccountList_Dialog(Form form)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();
                        ofdLoadFile.Filter = AntdUI.Localization.Get("ProxyAccountListFile", "代理账号列表文件") + " (*.pa)|*.pa|INI Files (*.ini)|*.ini";
                        ofdLoadFile.RestoreDirectory = true;

                        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = ofdLoadFile.FileName;
                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                AntdUI.Spin.open(form, AntdUI.Localization.Get("Loading", "正在加载..."), config =>
                                {
                                    if (LoadAccountList(form, FilePath, true))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ImportProxyAccountList.Success", "导入代理账号列表成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(LoadAccountList_Dialog), Title + ": " + FilePath);                                        
                                    }
                                }, () =>
                                {
                                    if (form is InterfaceInfo.IProxyMode pmForm)
                                    {
                                        pmForm.RefreshAccountList();
                                    }
                                });                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadAccountList_Dialog), ex.Message);
                    }
                }

                private static bool LoadAccountList(Form form, string FilePath, bool LoadFromUser)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            string fileExtension = Path.GetExtension(FilePath);
                            if (!string.IsNullOrEmpty(fileExtension))
                            {
                                if (fileExtension.Equals(".ini"))
                                {
                                    LoadAccountList_FromInIFile(FilePath);
                                    return true;
                                }
                                else
                                {
                                    #region//LoadProxyAccountList_FromXDocument

                                    XDocument xdoc = null;

                                    bool bEncrypt = SystemConfig.IsEncryptXMLFile(FilePath);
                                    if (bEncrypt)
                                    {
                                        if (LoadFromUser)
                                        {
                                            xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("ImportProxyAccountList", "导入代理账号列表"), FilePath);
                                        }
                                    }
                                    else
                                    {
                                        xdoc = XDocument.Load(FilePath);
                                    }

                                    if (xdoc == null)
                                    {
                                        string sError = AntdUI.Localization.Get("Password.Incorrect", "导入失败: 密码错误");
                                        if (LoadFromUser)
                                        {
                                            AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                        }
                                        else
                                        {
                                            Operate.DoLog(nameof(LoadAccountList), sError);
                                        }

                                        return false;
                                    }

                                    LoadAccountList_FromXDocument(xdoc);

                                    #endregion

                                    return true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadAccountList), ex.Message);
                    }

                    return false;
                }

                public static void LoadAccountList_FromXDocument(XDocument xdoc)
                {
                    try
                    {
                        foreach (XElement xeProxyAccount in xdoc.Root.Elements())
                        {
                            bool IsEnable = false;
                            if (xeProxyAccount.Element("IsEnable") != null)
                            {
                                IsEnable = bool.Parse(xeProxyAccount.Element("IsEnable").Value);
                            }

                            Guid AID = Guid.Empty;
                            if (xeProxyAccount.Element("ID") == null || !Guid.TryParse(xeProxyAccount.Element("ID").Value, out AID) || ProxyConfig.Account.GetProxyAccount_ByAccountID(AID) != null)
                            {
                                AID = Guid.NewGuid();
                            }

                            string UserName = string.Empty;
                            if (xeProxyAccount.Element("UserName") != null)
                            {
                                UserName = xeProxyAccount.Element("UserName").Value;
                            }

                            string PassWord = string.Empty;
                            if (xeProxyAccount.Element("PassWord") != null)
                            {
                                PassWord = xeProxyAccount.Element("PassWord").Value;
                            }

                            bool IsLimitLinks = false;
                            if (xeProxyAccount.Element("IsLimitLinks") != null)
                            {
                                IsLimitLinks = bool.Parse(xeProxyAccount.Element("IsLimitLinks").Value);
                            }

                            int LimitLinks = 1;
                            if (xeProxyAccount.Element("LimitLinks") != null)
                            {
                                LimitLinks = int.Parse(xeProxyAccount.Element("LimitLinks").Value);
                            }

                            bool IsLimitDevices = true;
                            if (xeProxyAccount.Element("IsLimitDevices") != null)
                            {
                                IsLimitDevices = bool.Parse(xeProxyAccount.Element("IsLimitDevices").Value);
                            }

                            int LimitDevices = 1;
                            if (xeProxyAccount.Element("LimitDevices") != null)
                            {
                                LimitDevices = int.Parse(xeProxyAccount.Element("LimitDevices").Value);
                            }

                            bool IsExpiry = false;
                            if (xeProxyAccount.Element("IsExpiry") != null)
                            {
                                IsExpiry = bool.Parse(xeProxyAccount.Element("IsExpiry").Value);
                            }

                            DateTime ExpiryTime = DateTime.Now;
                            if (xeProxyAccount.Element("ExpiryTime") != null)
                            {
                                ExpiryTime = DateTime.Parse(xeProxyAccount.Element("ExpiryTime").Value);
                            }

                            DateTime CreateTime = DateTime.Now;
                            if (xeProxyAccount.Element("CreateTime") != null)
                            {
                                CreateTime = DateTime.Parse(xeProxyAccount.Element("CreateTime").Value);
                            }

                            BindingList<AccountIPInfo> AIPInfo = new BindingList<AccountIPInfo>();

                            if (xeProxyAccount.Element("AccountIPInfo") != null)
                            {
                                foreach (XElement xeIPInfo in xeProxyAccount.Element("AccountIPInfo").Elements())
                                {
                                    DateTime LoginTime = DateTime.MinValue;
                                    if (xeIPInfo.Element("LoginTime") != null)
                                    {
                                        LoginTime = DateTime.Parse(xeIPInfo.Element("LoginTime").Value);
                                    }

                                    string LoginIP = string.Empty;
                                    if (xeIPInfo.Element("LoginIP") != null)
                                    {
                                        LoginIP = xeIPInfo.Element("LoginIP").Value;
                                    }

                                    ProxyConfig.Account.AddAccountIPInfo(AIPInfo, LoginTime, LoginIP);
                                }
                            }

                            bool bOK = ProxyConfig.Account.AddProxyAccount(
                                true,
                                AID,
                                IsEnable,
                                UserName,
                                PassWord,
                                AIPInfo,
                                IsLimitLinks,
                                LimitLinks,
                                IsLimitDevices,
                                LimitDevices,
                                IsExpiry,
                                ExpiryTime,
                                CreateTime);

                            if (!bOK)
                            {
                                string FailLog = string.Format(AntdUI.Localization.Get("ImportAccount.Error", "导入账号失败！用户名：{0}"), UserName);
                                Operate.DoLog(nameof(LoadAccountList_FromXDocument), FailLog);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadAccountList_FromXDocument), ex.Message);
                    }
                }

                private static void LoadAccountList_FromInIFile(string filePath)
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(filePath);

                        AccountInfo pai = null;
                        foreach (string line in lines)
                        {
                            string trimmedLine = line.Trim();
                            if (trimmedLine.StartsWith("[User"))
                            {
                                if (pai != null)
                                {
                                    ProxyConfig.Account.AddAccount_FromIniFile(pai);
                                }

                                pai = new AccountInfo();
                            }
                            else if (trimmedLine.Contains("="))
                            {
                                string[] parts = trimmedLine.Split(new char[] { '=' }, 2);
                                string key = parts[0].Trim();
                                string value = parts[1].Trim();

                                switch (key)
                                {
                                    case "Enable":
                                        pai.IsEnable = Convert.ToBoolean(int.Parse(value));
                                        break;

                                    case "UserName":
                                        pai.UserName = value;
                                        break;

                                    case "Password":
                                        pai.Password = value;
                                        break;

                                    case "MaxConn":
                                        if (value.Equals("-1"))
                                        {
                                            pai.IsLimitLinks = false;
                                            pai.LimitLinks = 1;
                                        }
                                        else
                                        {
                                            pai.IsLimitLinks = true;
                                            pai.LimitLinks = int.Parse(value);
                                        }
                                        break;

                                    case "AutoDisable":
                                        pai.IsExpiry = Convert.ToBoolean(int.Parse(value));
                                        break;

                                    case "DisableDateTime":
                                        pai.ExpiryTime = DateTime.Parse(value);
                                        break;
                                }
                            }
                        }

                        if (pai != null)
                        {
                            ProxyConfig.Account.AddAccount_FromIniFile(pai);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadAccountList_FromInIFile), ex.Message);
                    }
                }

                private static void AddAccount_FromIniFile(AccountInfo ai)
                {
                    try
                    {
                        if (ai != null)
                        {
                            if (ai.AID == null || ai.AID == Guid.Empty)
                            {
                                ai.AID = Guid.NewGuid();
                            }

                            if (ai.ExpiryTime == DateTime.MinValue)
                            {
                                ai.ExpiryTime = DateTime.Now;
                            }

                            if (ai.CreateTime == DateTime.MinValue)
                            {
                                ai.CreateTime = DateTime.Now;
                            }
                            
                            ai.IsLimitDevices = true;
                            ai.LimitDevices = 1;

                            bool bOK = ProxyConfig.Account.AddProxyAccount(
                                true,
                                ai.AID,
                                ai.IsEnable,
                                ai.UserName,
                                ai.Password,
                                new BindingList<AccountIPInfo>(),
                                ai.IsLimitLinks,
                                ai.LimitLinks,
                                ai.IsLimitDevices,
                                ai.LimitDevices,
                                ai.IsExpiry,
                                ai.ExpiryTime,
                                ai.CreateTime);

                            if (!bOK)
                            {
                                string FailLog = string.Format(AntdUI.Localization.Get("ImportAccount.Error", "导入账号失败！用户名：{0}"), ai.UserName);
                                Operate.DoLog(nameof(AddAccount_FromIniFile), FailLog);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddAccount_FromIniFile), ex.Message);
                    }
                }

                #endregion
            }

            #endregion

            #region//代理映射

            public static class Mapping
            {
                public static bool IsShow_MapLocal = false, IsShow_MapRemote = false;
                public static bool Enable_MapLocal = false, Enable_MapRemote = false;
                public static BindingList<MapLocal> lstMapLocal = new BindingList<MapLocal>();
                public static BindingList<MapRemote> lstMapRemote = new BindingList<MapRemote>();

                #region//获取 MapProtocol 类型

                public static ProxyConfig.Proxy.MapProtocol GetMapProtocol_ByString(string MapProtocol)
                {
                    ProxyConfig.Proxy.MapProtocol MProtocol = ProxyConfig.Proxy.MapProtocol.Http;

                    try
                    {
                        MProtocol = (ProxyConfig.Proxy.MapProtocol)Enum.Parse(typeof(ProxyConfig.Proxy.MapProtocol), MapProtocol);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetMapProtocol_ByString), ex.Message);
                    }

                    return MProtocol;
                }

                #endregion

                #region//获取远程代理映射的请求数据

                public static byte[] ModifyRequestHostAndPath(
                    string originalRequest, 
                    Dictionary<string, string> headers,
                    string newHost, 
                    int newPort, 
                    string newPath)
                {
                    try
                    {
                        string[] lines = originalRequest.Split(new[] { "\r\n" }, StringSplitOptions.None);
                        StringBuilder sb = new StringBuilder();

                        bool firstLine = true;
                        bool hostHeaderFound = false;

                        foreach (string line in lines)
                        {
                            if (string.IsNullOrEmpty(line))
                                break;

                            if (firstLine)
                            {
                                string[] parts = line.Split(' ');
                                if (parts.Length >= 3)
                                {
                                    string originalRequestPath = parts[1];

                                    string pathOnly = originalRequestPath;
                                    string queryString = "";

                                    int queryIndex = originalRequestPath.IndexOf('?');
                                    if (queryIndex >= 0)
                                    {
                                        pathOnly = originalRequestPath.Substring(0, queryIndex);
                                        queryString = originalRequestPath.Substring(queryIndex);
                                    }

                                    string finalPath;
                                    if (string.IsNullOrEmpty(newPath))
                                    {
                                        finalPath = pathOnly;
                                    }
                                    else
                                    {
                                        finalPath = newPath;
                                    }

                                    if (string.IsNullOrEmpty(finalPath) || !finalPath.StartsWith("/"))
                                    {
                                        finalPath = "/" + finalPath;
                                    }

                                    parts[1] = finalPath + queryString;
                                    sb.AppendLine(string.Join(" ", parts));
                                }
                                else
                                {
                                    sb.AppendLine(line);
                                }
                                firstLine = false;
                            }
                            else if (line.StartsWith("Host:", StringComparison.OrdinalIgnoreCase))
                            {
                                string portPart = newPort == 80 ? "" : $":{newPort}";
                                sb.AppendLine($"Host: {newHost}{portPart}");
                                hostHeaderFound = true;
                            }
                            else
                            {
                                sb.AppendLine(line);
                            }
                        }

                        if (!hostHeaderFound)
                        {
                            string portPart = newPort == 80 ? "" : $":{newPort}";
                            string hostLine = $"Host: {newHost}{portPart}";

                            string requestStr = sb.ToString();
                            int insertIndex = requestStr.IndexOf("\r\n", StringComparison.Ordinal);
                            if (insertIndex > 0)
                            {
                                sb.Insert(insertIndex + 2, hostLine + "\r\n");
                            }
                            else
                            {
                                sb.AppendLine(hostLine);
                            }
                        }

                        sb.AppendLine();
                        return Encoding.UTF8.GetBytes(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(ModifyRequestHostAndPath), ex.Message);
                        return Encoding.UTF8.GetBytes(originalRequest);
                    }
                }

                #endregion

                #region//缓存映射数据

                public static void MappingData_ToQueue(ProxySession psSession, Operate.PacketConfig.Packet.PacketType ptType, byte[] bData, bool MapRemote)
                {
                    try
                    {
                        string ClientAddr = $"{psSession.ClientIP}:{psSession.ClientPort}";
                        string ServerAddr = string.Empty;

                        if (MapRemote)
                        {
                            ServerAddr = $"{psSession.ServerIP}:{psSession.ServerPort}";
                        }
                        else
                        {
                            ServerAddr = $"{psSession.ClientIP}:{psSession.ClientPort}";
                        }

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            Operate.FilterConfig.Filter.FilterAction.None,
                            bData.Length,
                            psSession.SocketSession.Client.Handle.ToInt32(),
                            ptType,
                            ClientAddr,
                            ServerAddr,
                            psSession.ServerAddress,
                            psSession.DomainType,
                            bData,
                            bData);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(MappingData_ToQueue), ex.Message);
                    }
                }

                #endregion

                #region//新增本地代理映射    

                public static void AddMapLocal(bool IsEnable, ProxyConfig.Proxy.MapProtocol ProtocolType, string Host, int Port, string RemotePath, string LocalPath)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Host) && Port > 0)
                        {
                            MapLocal pml = new MapLocal(IsEnable, ProtocolType, Host, Port, RemotePath, LocalPath);
                            ProxyConfig.Mapping.lstMapLocal.Add(pml);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddMapLocal), ex.Message);
                    }
                }

                #endregion

                #region//新增远程代理映射    

                public static void AddMapRemote(
                    bool IsEnable,
                    ProxyConfig.Proxy.MapProtocol ProtocolType_From,
                    string Host_From,
                    int Port_From,
                    string Path_From,
                    ProxyConfig.Proxy.MapProtocol ProtocolType_To,
                    string Host_To,
                    int Port_To,
                    string Path_To)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Host_From) && Port_From > 0 && !string.IsNullOrEmpty(Host_To) && Port_To > 0)
                        {
                            MapRemote pmr = new MapRemote(
                                IsEnable,
                                ProtocolType_From,
                                Host_From,
                                Port_From,
                                Path_From,
                                ProtocolType_To,
                                Host_To,
                                Port_To,
                                Path_To);

                            ProxyConfig.Mapping.lstMapRemote.Add(pmr);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddMapRemote), ex.Message);
                    }
                }

                #endregion

                #region//编辑本地映射

                public static void OpenMapLocalEdit(Form form, MapSetting msForm, MapLocal ml)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("MapLocalForm", "本地映射编辑"), new MapLocalEdit(form, msForm, ml))
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//编辑远程映射

                public static void OpenMapRemoteEdit(Form form, MapSetting msForm, MapRemote mr)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("MapRemoteForm", "远程映射编辑"), new MapRemoteEdit(form, msForm, mr))
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//删除本地代理映射（对话框）

                public static void DeleteMapLocal_Dialog(Form form, MapLocal ml)
                {
                    try
                    {
                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("MapSettingsForm.MapLocal", "本地映射"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                        {
                            Icon = TType.Warn,
                            Keyboard = false,
                            MaskClosable = false,
                            OnOk = config =>
                            {
                                if (ml != null)
                                {
                                    ProxyConfig.Mapping.lstMapLocal.Remove(ml);
                                }

                                return true;
                            }
                        });                        
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteMapLocal_Dialog), ex.Message);
                    }
                }

                #endregion

                #region//删除远程代理映射（对话框）

                public static void DeleteMapRemote_Dialog(Form form, MapRemote mr)
                {
                    try
                    {
                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("MapSettingsForm.MapRemote", "远程映射"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                        {
                            Icon = TType.Warn,
                            Keyboard = false,
                            MaskClosable = false,
                            OnOk = config =>
                            {
                                if (mr != null)
                                {
                                    ProxyConfig.Mapping.lstMapRemote.Remove(mr);
                                }

                                return true;
                            }
                        });                        
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteMapRemote_Dialog), ex.Message);
                    }
                }

                #endregion

                #region//清空本地代理映射（对话框）

                public static void CleanUpMapLocal_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("MapSettingsForm.MapLocal", "本地映射"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                    {
                        Icon = TType.Warn,
                        Keyboard = false,
                        MaskClosable = false,
                        OnOk = config =>
                        {
                            ProxyConfig.Mapping.MapLocalClear();
                            return true;
                        }
                    });
                }

                public static void MapLocalClear()
                {
                    try
                    {
                        lstMapLocal.Clear();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(MapLocalClear), ex.Message);
                    }
                }

                #endregion

                #region//清空远程代理映射（对话框）

                public static void CleanUpMapRemote_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("MapSettingsForm.MapRemote", "远程映射"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                    {
                        Icon = TType.Warn,
                        Keyboard = false,
                        MaskClosable = false,
                        OnOk = config =>
                        {
                            ProxyConfig.Mapping.MapRemoteClear();
                            return true;
                        }
                    });
                }

                public static void MapRemoteClear()
                {
                    try
                    {
                        lstMapRemote.Clear();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(MapRemoteClear), ex.Message);
                    }
                }

                #endregion

                #region//更新本地代理映射

                public static void UpdateMapLocal(MapLocal pml, ProxyConfig.Proxy.MapProtocol ProtocolType, string Host, int Port, string RemotePath, string LocalPath)
                {
                    try
                    {
                        if (pml != null && !string.IsNullOrEmpty(Host) && Port > 0)
                        {
                            pml.ProtocolType = ProtocolType;
                            pml.Host = Host;
                            pml.Port = Port;
                            pml.RemotePath = RemotePath;
                            pml.LocalPath = LocalPath;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateMapLocal), ex.Message);
                    }
                }

                #endregion

                #region//更新远程代理映射

                public static void UpdateMapRemote(
                    MapRemote pmr,
                    ProxyConfig.Proxy.MapProtocol ProtocolType_From,
                    string Host_From,
                    int Port_From,
                    string Path_From,
                    ProxyConfig.Proxy.MapProtocol ProtocolType_To,
                    string Host_To,
                    int Port_To,
                    string Path_To)
                {
                    try
                    {
                        if (pmr != null && !string.IsNullOrEmpty(Host_From) && Port_From > 0 && !string.IsNullOrEmpty(Host_To) && Port_To > 0)
                        {
                            pmr.ProtocolTypeFrom = ProtocolType_From;
                            pmr.HostFrom = Host_From;
                            pmr.PortFrom = Port_From;
                            pmr.PathFrom = Path_From;
                            pmr.ProtocolTypeTo = ProtocolType_To;
                            pmr.HostTo = Host_To;
                            pmr.PortTo = Port_To;
                            pmr.PathTo = Path_To;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateMapRemote), ex.Message);
                    }
                }

                #endregion

                #region//查找本地代理映射

                public static MapLocal GetMapLocal(ProxyConfig.Proxy.MapProtocol ProtocolType, string host, int port, string path)
                {
                    try
                    {
                        return ProxyConfig.Mapping.lstMapLocal.FirstOrDefault(rule =>
                            rule.IsEnable == true &&
                            rule.ProtocolType == ProtocolType &&
                            rule.Host.Equals(host, StringComparison.OrdinalIgnoreCase) &&
                            rule.Port == port &&
                            path.StartsWith(rule.RemotePath, StringComparison.OrdinalIgnoreCase));
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetMapLocal), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//查找远程代理映射

                public static MapRemote GetMapRemote(ProxyConfig.Proxy.MapProtocol ProtocolType_From, string Host_From, int Port_From, string Path_From)
                {
                    if (string.IsNullOrEmpty(Path_From))
                    {
                        return ProxyConfig.Mapping.lstMapRemote.FirstOrDefault(rule =>
                        rule.IsEnable == true &&
                        rule.ProtocolTypeFrom == ProtocolType_From &&
                        rule.HostFrom.Equals(Host_From, StringComparison.OrdinalIgnoreCase) &&
                        rule.PortFrom == Port_From);
                    }
                    else
                    {
                        return ProxyConfig.Mapping.lstMapRemote.FirstOrDefault(rule =>
                        rule.IsEnable == true &&
                        rule.ProtocolTypeFrom == ProtocolType_From &&
                        rule.HostFrom.Equals(Host_From, StringComparison.OrdinalIgnoreCase) &&
                        rule.PortFrom == Port_From &&
                        Path_From.StartsWith(rule.PathFrom, StringComparison.OrdinalIgnoreCase));
                    }
                }

                #endregion

                #region//获取代理映射的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_Mapping()
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                    menuItems.Add(new AntdUI.ContextMenuStripItem("置顶", "Ctrl+⬆")
                    {
                        ID = "Top",
                        IconSvg = "VerticalAlignTopOutlined",
                        LocalizationText = "Top",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("向上移动", "Alt+⬆")
                    {
                        ID = "Up",
                        IconSvg = "ArrowUpOutlined",
                        LocalizationText = "Up",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItem("向下移动", "Alt+⬇")
                    {
                        ID = "Down",
                        IconSvg = "ArrowDownOutlined",
                        LocalizationText = "Down",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("置底", "Ctrl+⬇")
                    {
                        ID = "Bottom",
                        IconSvg = "VerticalAlignBottomOutlined",
                        LocalizationText = "Bottom",
                    });                    

                    return menuItems.ToArray();
                }

                #endregion

                #region//本地映射的列表操作

                public static void UpdateMapLocal_ByListAction(Form form, SystemConfig.ListAction listAction, MapLocal pml)
                {
                    try
                    {
                        int iIndex = 0;

                        switch (listAction)
                        {
                            case SystemConfig.ListAction.Top:

                                ProxyConfig.Mapping.lstMapLocal.Remove(pml);
                                ProxyConfig.Mapping.lstMapLocal.Insert(0, pml);

                                break;

                            case SystemConfig.ListAction.Up:

                                iIndex = ProxyConfig.Mapping.lstMapLocal.IndexOf(pml);
                                if (iIndex > 0)
                                {
                                    ProxyConfig.Mapping.lstMapLocal.Remove(pml);
                                    ProxyConfig.Mapping.lstMapLocal.Insert(iIndex - 1, pml);
                                }

                                break;

                            case SystemConfig.ListAction.Down:

                                iIndex = ProxyConfig.Mapping.lstMapLocal.IndexOf(pml);
                                if (iIndex > -1 && iIndex < ProxyConfig.Mapping.lstMapLocal.Count - 1)
                                {
                                    ProxyConfig.Mapping.lstMapLocal.Remove(pml);
                                    ProxyConfig.Mapping.lstMapLocal.Insert(iIndex + 1, pml);
                                }

                                break;

                            case SystemConfig.ListAction.Bottom:

                                ProxyConfig.Mapping.lstMapLocal.Remove(pml);
                                ProxyConfig.Mapping.lstMapLocal.Add(pml);

                                break;

                            case SystemConfig.ListAction.Import:

                                ProxyConfig.Mapping.LoadMapLocal_Dialog(form);

                                break;

                            case SystemConfig.ListAction.Export:

                                ProxyConfig.Mapping.SaveMapLocal_Dialog(form, string.Empty, ProxyConfig.Mapping.lstMapLocal);

                                break;

                            case SystemConfig.ListAction.CleanUp:

                                ProxyConfig.Mapping.CleanUpMapLocal_Dialog(form);

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateMapLocal_ByListAction), ex.Message);
                    }
                }

                #endregion

                #region//远程映射的列表操作

                public static void UpdateMapRemote_ByListAction(Form form, SystemConfig.ListAction listAction, MapRemote pmr)
                {
                    try
                    {
                        int iIndex = 0;

                        switch (listAction)
                        {
                            case SystemConfig.ListAction.Top:

                                ProxyConfig.Mapping.lstMapRemote.Remove(pmr);
                                ProxyConfig.Mapping.lstMapRemote.Insert(0, pmr);

                                break;

                            case SystemConfig.ListAction.Up:

                                iIndex = ProxyConfig.Mapping.lstMapRemote.IndexOf(pmr);
                                if (iIndex > 0)
                                {
                                    ProxyConfig.Mapping.lstMapRemote.Remove(pmr);
                                    ProxyConfig.Mapping.lstMapRemote.Insert(iIndex - 1, pmr);
                                }

                                break;

                            case SystemConfig.ListAction.Down:

                                iIndex = ProxyConfig.Mapping.lstMapRemote.IndexOf(pmr);
                                if (iIndex > -1 && iIndex < ProxyConfig.Mapping.lstMapRemote.Count - 1)
                                {
                                    ProxyConfig.Mapping.lstMapRemote.Remove(pmr);
                                    ProxyConfig.Mapping.lstMapRemote.Insert(iIndex + 1, pmr);
                                }

                                break;

                            case SystemConfig.ListAction.Bottom:

                                ProxyConfig.Mapping.lstMapRemote.Remove(pmr);
                                ProxyConfig.Mapping.lstMapRemote.Add(pmr);

                                break;

                            case SystemConfig.ListAction.Import:

                                ProxyConfig.Mapping.LoadMapRemote_Dialog(form);

                                break;

                            case SystemConfig.ListAction.Export:

                                ProxyConfig.Mapping.SaveMapRemote_Dialog(form, string.Empty, ProxyConfig.Mapping.lstMapRemote);

                                break;

                            case SystemConfig.ListAction.CleanUp:

                                ProxyConfig.Mapping.CleanUpMapRemote_Dialog(form);

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateMapRemote_ByListAction), ex.Message);
                    }
                }

                #endregion

                #region//保存本地代理映射到数据库

                public static void SaveMapLocal_ToDB()
                {
                    try
                    {
                        DataBase.DeleteTable_ProxyMapLocal();
                        DataBase.InsertTable_ProxyMapLocal();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveMapLocal_ToDB), ex.Message);
                    }
                }

                #endregion

                #region//保存远程代理映射到数据库

                public static void SaveMapRemote_ToDB()
                {
                    try
                    {
                        DataBase.DeleteTable_ProxyMapRemote();
                        DataBase.InsertTable_ProxyMapRemote();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveMapRemote_ToDB), ex.Message);
                    }
                }

                #endregion

                #region//从数据库加载本地代理映射（异步）

                public static async void LoadProxyMapLocal_FromDB()
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            DataTable dtProxyMapLocal = DataBase.SelectTable_ProxyMapLocal();

                            foreach (DataRow dataRow in dtProxyMapLocal.Rows)
                            {
                                bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);
                                ProxyConfig.Proxy.MapProtocol ProtocolType = ProxyConfig.Mapping.GetMapProtocol_ByString(dataRow["ProtocolType"].ToString());
                                string Host = dataRow["Host"].ToString();
                                int Port = int.Parse(dataRow["Port"].ToString());
                                string RemotePath = dataRow["RemotePath"].ToString();
                                string LocalPath = dataRow["LocalPath"].ToString();

                                ProxyConfig.Mapping.AddMapLocal(IsEnable, ProtocolType, Host, Port, RemotePath, LocalPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            Operate.DoLog(nameof(LoadProxyMapLocal_FromDB), ex.Message);
                        }
                    });
                }

                #endregion

                #region//从数据库加载远程代理映射（异步）

                public static async void LoadProxyMapRemote_FromDB()
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            DataTable dtProxyMapRemote = DataBase.SelectTable_ProxyMapRemote();

                            foreach (DataRow dataRow in dtProxyMapRemote.Rows)
                            {
                                bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);

                                ProxyConfig.Proxy.MapProtocol ProtocolType_From = ProxyConfig.Mapping.GetMapProtocol_ByString(dataRow["ProtocolType_From"].ToString());
                                string Host_From = dataRow["Host_From"].ToString();
                                int Port_From = int.Parse(dataRow["Port_From"].ToString());
                                string Path_From = dataRow["Path_From"].ToString();

                                ProxyConfig.Proxy.MapProtocol ProtocolType_To = ProxyConfig.Mapping.GetMapProtocol_ByString(dataRow["ProtocolType_To"].ToString());
                                string Host_To = dataRow["Host_To"].ToString();
                                int Port_To = int.Parse(dataRow["Port_To"].ToString());
                                string Path_To = dataRow["Path_To"].ToString();

                                ProxyConfig.Mapping.AddMapRemote(
                                    IsEnable,
                                    ProtocolType_From,
                                    Host_From,
                                    Port_From,
                                    Path_From,
                                    ProtocolType_To,
                                    Host_To,
                                    Port_To,
                                    Path_To);
                            }
                        }
                        catch (Exception ex)
                        {
                            Operate.DoLog(nameof(LoadProxyMapRemote_FromDB), ex.Message);
                        }
                    });
                }

                #endregion

                #region//保存本地映射到文件（对话框）

                public static void SaveMapLocal_Dialog(Form form, string FileName, BindingList<MapLocal> pmlList)
                {
                    try
                    {
                        if (ProxyConfig.Mapping.lstMapLocal.Count > 0)
                        {
                            SaveFileDialog sfdSaveFile = new SaveFileDialog();
                            sfdSaveFile.Filter = AntdUI.Localization.Get("MapLocalFile", "本地映射文件") + "（*.pml）|*.pml";

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveFile.FileName = FileName;
                            }

                            sfdSaveFile.RestoreDirectory = true;
                            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveFile.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("ExportMapLocal", "导出本地映射"));

                                    if (SaveMapLocal(FilePath, pmlList, EncryptPassword.DoEncrypt, EncryptPassword.Password))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportMapLocal.Success", "导出本地映射成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SaveMapLocal_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportMapLocal.Error", "导出本地映射失败");
                                        string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveMapLocal_Dialog), ex.Message);
                    }
                }

                private static bool SaveMapLocal(string FilePath, BindingList<MapLocal> pmlList, bool DoEncrypt, string Password)
                {
                    try
                    {
                        XDocument xdoc = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };

                        XElement xeMapLocal = ProxyConfig.Mapping.GetMapLocal_XML(pmlList);
                        if (xeMapLocal == null)
                        {
                            return false;
                        }

                        xdoc.Add(xeMapLocal);
                        xdoc.Save(FilePath);

                        if (DoEncrypt)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                SystemConfig.EncryptXMLFile(FilePath, Password);
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveMapLocal), ex.Message);
                    }

                    return false;
                }

                public static XElement GetMapLocal_XML(BindingList<MapLocal> pmlList)
                {
                    try
                    {
                        XElement xeMapLocal = new XElement("MapLocal");

                        foreach (MapLocal pml in pmlList)
                        {
                            XElement xeLocal =
                                new XElement("Local",
                                new XElement("IsEnable", pml.IsEnable.ToString()),
                                new XElement("ProtocolType", pml.ProtocolType.ToString()),
                                new XElement("Host", pml.Host),
                                new XElement("Port", pml.Port.ToString()),
                                new XElement("RemotePath", pml.RemotePath),
                                new XElement("LocalPath", pml.LocalPath)
                                );

                            xeMapLocal.Add(xeLocal);
                        }

                        return xeMapLocal;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetMapLocal_XML), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//保存远程映射到文件（对话框）

                public static void SaveMapRemote_Dialog(Form form, string FileName, BindingList<MapRemote> pmrList)
                {
                    try
                    {
                        if (ProxyConfig.Mapping.lstMapRemote.Count > 0)
                        {
                            SaveFileDialog sfdSaveFile = new SaveFileDialog();
                            sfdSaveFile.Filter = AntdUI.Localization.Get("MapRemoteFile", "远程映射文件") + "（*.pmr）|*.pmr";

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveFile.FileName = FileName;
                            }

                            sfdSaveFile.RestoreDirectory = true;
                            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveFile.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("ExportMapRemote", "导出远程映射"));

                                    if (SaveMapRemote(FilePath, pmrList, EncryptPassword.DoEncrypt, EncryptPassword.Password))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportMapRemote.Success", "导出远程映射成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SaveMapRemote_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportMapRemote.Error", "导出远程映射失败");
                                        string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveMapRemote_Dialog), ex.Message);
                    }
                }

                private static bool SaveMapRemote(string FilePath, BindingList<MapRemote> pmrList, bool DoEncrypt, string Password)
                {
                    try
                    {
                        XDocument xdoc = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };

                        XElement xeMapRemote = ProxyConfig.Mapping.GetMapRemote_XML(pmrList);
                        if (xeMapRemote == null)
                        {
                            return false;
                        }

                        xdoc.Add(xeMapRemote);
                        xdoc.Save(FilePath);

                        if (DoEncrypt)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                SystemConfig.EncryptXMLFile(FilePath, Password);
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveMapRemote), ex.Message);
                    }

                    return false;
                }

                public static XElement GetMapRemote_XML(BindingList<MapRemote> pmrList)
                {
                    try
                    {
                        XElement xeMapRemote = new XElement("MapRemote");

                        foreach (MapRemote pmr in pmrList)
                        {
                            XElement xeLocal =
                                new XElement("Remote",
                                new XElement("IsEnable", pmr.IsEnable.ToString()),
                                new XElement("ProtocolType_From", pmr.ProtocolTypeFrom.ToString()),
                                new XElement("Host_From", pmr.HostFrom),
                                new XElement("Port_From", pmr.PortFrom.ToString()),
                                new XElement("Path_From", pmr.PathFrom),
                                new XElement("ProtocolType_To", pmr.ProtocolTypeTo.ToString()),
                                new XElement("Host_To", pmr.HostTo),
                                new XElement("Port_To", pmr.PortTo.ToString()),
                                new XElement("Path_To", pmr.PathTo)
                                );

                            xeMapRemote.Add(xeLocal);
                        }

                        return xeMapRemote;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetMapRemote_XML), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//从文件加载本地映射（对话框）

                public static void LoadMapLocal_Dialog(Form form)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();
                        ofdLoadFile.Filter = AntdUI.Localization.Get("MapLocalFile", "本地映射文件") + "（*.pml）|*.pml";
                        ofdLoadFile.RestoreDirectory = true;

                        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = ofdLoadFile.FileName;
                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                if (LoadMapLocal(form, FilePath, true))
                                {
                                    string Title = AntdUI.Localization.Get("InjectModeForm.ImportMapLocal.Success", "导入本地映射成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(nameof(LoadMapLocal_Dialog), Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadMapLocal_Dialog), ex.Message);
                    }
                }

                private static bool LoadMapLocal(Form form, string FilePath, bool LoadFromUser)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            XDocument xdoc = null;

                            bool bEncrypt = SystemConfig.IsEncryptXMLFile(FilePath);
                            if (bEncrypt)
                            {
                                if (LoadFromUser)
                                {
                                    xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("ImportMapLocal", "导入本地映射"), FilePath);
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("Password.Incorrect", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(nameof(LoadMapLocal), sError);
                                }

                                return false;
                            }

                            LoadMapLocal_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadMapLocal), ex.Message);
                    }

                    return false;
                }

                public static void LoadMapLocal_FromXDocument(XDocument xdoc)
                {
                    try
                    {
                        foreach (XElement xeMapLocal in xdoc.Root.Elements())
                        {
                            bool IsEnable = false;
                            if (xeMapLocal.Element("IsEnable") != null)
                            {
                                IsEnable = bool.Parse(xeMapLocal.Element("IsEnable").Value);
                            }

                            ProxyConfig.Proxy.MapProtocol ProtocolType = ProxyConfig.Proxy.MapProtocol.Http;
                            if (xeMapLocal.Element("ProtocolType") != null)
                            {
                                ProtocolType = ProxyConfig.Mapping.GetMapProtocol_ByString(xeMapLocal.Element("ProtocolType").Value);
                            }

                            string Host = string.Empty;
                            if (xeMapLocal.Element("Host") != null)
                            {
                                Host = xeMapLocal.Element("Host").Value;
                            }

                            int Port = 80;
                            if (xeMapLocal.Element("Port") != null)
                            {
                                Port = int.Parse(xeMapLocal.Element("Port").Value);
                            }

                            string RemotePath = string.Empty;
                            if (xeMapLocal.Element("RemotePath") != null)
                            {
                                RemotePath = xeMapLocal.Element("RemotePath").Value;
                            }

                            string LocalPath = string.Empty;
                            if (xeMapLocal.Element("LocalPath") != null)
                            {
                                LocalPath = xeMapLocal.Element("LocalPath").Value;
                            }

                            ProxyConfig.Mapping.AddMapLocal(IsEnable, ProtocolType, Host, Port, RemotePath, LocalPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadMapLocal_FromXDocument), ex.Message);
                    }
                }

                #endregion

                #region//从文件加载远程映射（对话框）

                public static void LoadMapRemote_Dialog(Form form)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();
                        ofdLoadFile.Filter = AntdUI.Localization.Get("MapRemoteFile", "远程映射文件") + "（*.pmr）|*.pmr";
                        ofdLoadFile.RestoreDirectory = true;

                        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = ofdLoadFile.FileName;
                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                if (LoadMapRemote(form, FilePath, true))
                                {
                                    string Title = AntdUI.Localization.Get("InjectModeForm.ImportMapRemote.Success", "导入远程映射成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(nameof(LoadMapRemote_Dialog), Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadMapRemote_Dialog), ex.Message);
                    }
                }

                private static bool LoadMapRemote(Form form, string FilePath, bool LoadFromUser)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            XDocument xdoc = null;

                            bool bEncrypt = SystemConfig.IsEncryptXMLFile(FilePath);
                            if (bEncrypt)
                            {
                                if (LoadFromUser)
                                {
                                    xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("ImportMapRemote", "导入远程映射"), FilePath);
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("Password.Incorrect", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(nameof(LoadMapRemote), sError);
                                }

                                return false;
                            }

                            LoadMapRemote_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadMapRemote), ex.Message);
                    }

                    return false;
                }

                public static void LoadMapRemote_FromXDocument(XDocument xdoc)
                {
                    try
                    {
                        foreach (XElement xeMapRemote in xdoc.Root.Elements())
                        {
                            bool IsEnable = false;
                            if (xeMapRemote.Element("IsEnable") != null)
                            {
                                IsEnable = bool.Parse(xeMapRemote.Element("IsEnable").Value);
                            }

                            ProxyConfig.Proxy.MapProtocol ProtocolType_From = ProxyConfig.Proxy.MapProtocol.Http;
                            if (xeMapRemote.Element("ProtocolType_From") != null)
                            {
                                ProtocolType_From = ProxyConfig.Mapping.GetMapProtocol_ByString(xeMapRemote.Element("ProtocolType_From").Value);
                            }

                            string Host_From = string.Empty;
                            if (xeMapRemote.Element("Host_From") != null)
                            {
                                Host_From = xeMapRemote.Element("Host_From").Value;
                            }

                            int Port_From = 80;
                            if (xeMapRemote.Element("Port_From") != null)
                            {
                                Port_From = int.Parse(xeMapRemote.Element("Port_From").Value);
                            }

                            string Path_From = string.Empty;
                            if (xeMapRemote.Element("Path_From") != null)
                            {
                                Path_From = xeMapRemote.Element("Path_From").Value;
                            }

                            ProxyConfig.Proxy.MapProtocol ProtocolType_To = ProxyConfig.Proxy.MapProtocol.Http;
                            if (xeMapRemote.Element("ProtocolType_To") != null)
                            {
                                ProtocolType_To = ProxyConfig.Mapping.GetMapProtocol_ByString(xeMapRemote.Element("ProtocolType_To").Value);
                            }

                            string Host_To = string.Empty;
                            if (xeMapRemote.Element("Host_To") != null)
                            {
                                Host_To = xeMapRemote.Element("Host_To").Value;
                            }

                            int Port_To = 80;
                            if (xeMapRemote.Element("Port_To") != null)
                            {
                                Port_To = int.Parse(xeMapRemote.Element("Port_To").Value);
                            }

                            string Path_To = string.Empty;
                            if (xeMapRemote.Element("Path_To") != null)
                            {
                                Path_To = xeMapRemote.Element("Path_To").Value;
                            }

                            ProxyConfig.Mapping.AddMapRemote(
                                IsEnable,
                                ProtocolType_From,
                                Host_From,
                                Port_From,
                                Path_From,
                                ProtocolType_To,
                                Host_To,
                                Port_To,
                                Path_To);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadMapRemote_FromXDocument), ex.Message);
                    }
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region//封包配置

        public static class PacketConfig
        {
            #region//封包

            public static class Packet
            {
                public static int Send_CNT = 0;
                public static int SendTo_CNT = 0;
                public static int Recv_CNT = 0;
                public static int RecvFrom_CNT = 0;
                public static int WSASend_CNT = 0;
                public static int WSASendTo_CNT = 0;
                public static int WSARecv_CNT = 0;
                public static int WSARecvFrom_CNT = 0;
                public static int FilterPacket_CNT = 0;
                public static int PacketData_MaxLen = 60;
                public static long TotalPackets = 0;
                public static long Total_SendBytes = 0;
                public static long Total_RecvBytes = 0;
                public static byte[] bByteBuff = new byte[0];
                public static string InjectProcess = string.Empty;
                public static string SpeedInfo = string.Empty;
                public static bool Support_WS1, Support_WS2, Support_MsWS;
                public static bool HookWS1_Send = true, HookWS1_SendTo = true, HookWS1_Recv = true, HookWS1_RecvFrom = true;
                public static bool HookWS2_Send = true, HookWS2_SendTo = true, HookWS2_Recv = true, HookWS2_RecvFrom = true;
                public static bool HookWSA_Send = true, HookWSA_SendTo = true, HookWSA_Recv = true, HookWSA_RecvFrom = true;

                #region//结构定义

                [StructLayout(LayoutKind.Sequential)]

                public struct SockAddr
                {
                    public short sin_family;
                    public ushort sin_port;
                    public uint sin_addr;
                    private Int64 Zero;

                    public void MarshalFromNative(IntPtr native)
                    {
                        Marshal.PtrToStructure(native, this);

                        sin_port = (ushort)(((sin_port & 0xFF) << 8) | ((sin_port >> 8) & 0xFF));
                    }

                    public void MarshalToNative(IntPtr native)
                    {
                        sin_port = (ushort)(((sin_port & 0xFF) << 8) | ((sin_port >> 8) & 0xFF));

                        Marshal.StructureToPtr(this, native, true);
                    }
                }

                [StructLayout(LayoutKind.Sequential)]

                public struct WSABUF
                {
                    public int len;
                    public IntPtr buf;
                }

                [StructLayout(LayoutKind.Sequential)]

                public struct OVERLAPPED
                {
                    public UIntPtr InternalLow;
                    public UIntPtr InternalHigh;
                    public int OffsetLow;
                    public int OffsetHigh;
                    public IntPtr EventHandle;
                }

                public enum PacketType
                {
                    WS1_Send = 0,
                    WS2_Send = 1,
                    WS1_SendTo = 2,
                    WS2_SendTo = 3,
                    WS1_Recv = 4,
                    WS2_Recv = 5,
                    WS1_RecvFrom = 6,
                    WS2_RecvFrom = 7,
                    WSASend = 8,
                    WSASendTo = 9,
                    WSARecv = 10,
                    WSARecvEx = 11,
                    WSARecvFrom = 12,
                    TCP_Req = 13,
                    UDP_Req = 14,
                    TCP_Resp = 15,
                    UDP_Resp = 16,
                }

                public enum IPType
                {
                    From = 0,
                    To = 1,
                }

                public enum EncodingFormat
                {
                    Default = 0,
                    Char = 1,
                    Byte = 2,
                    Bytes = 3,
                    Short = 4,
                    UShort = 5,
                    Int32 = 6,
                    UInt32 = 7,
                    Int64 = 8,
                    UInt64 = 9,
                    Float = 10,
                    Double = 11,
                    Bin = 12,
                    GBK = 13,
                    Unicode = 14,
                    ASCII = 15,
                    Hex = 16,
                    UTF7 = 17,
                    UTF8 = 18,
                    UTF16 = 19,
                    UTF32 = 20,
                    Base64 = 21,
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
                                case Operate.PacketConfig.Packet.PacketType.TCP_Req:
                                case Operate.PacketConfig.Packet.PacketType.UDP_Req:
                                    sIPString = sIPTo;
                                    break;
                                case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                                case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                                case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                                case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                                case Operate.PacketConfig.Packet.PacketType.WSARecv:
                                case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                                case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:
                                case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                                case Operate.PacketConfig.Packet.PacketType.UDP_Resp:
                                    sIPString = sIPFrom;
                                    break;
                            }

                            int res = -1;
                            switch (packetType)
                            {
                                case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                                case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                                    res = WSock32.send(Socket, ipSend, bSendBuffer.Length, SocketFlags.None);
                                    break;
                                case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                                case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                                case Operate.PacketConfig.Packet.PacketType.WSASend:
                                case Operate.PacketConfig.Packet.PacketType.WSARecv:
                                case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                                case Operate.PacketConfig.Packet.PacketType.TCP_Req:
                                case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                                    res = WS2_32.send(Socket, ipSend, bSendBuffer.Length, SocketFlags.None);
                                    break;
                                case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                                case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                                    if (!string.IsNullOrEmpty(sIPString))
                                    {
                                        Operate.PacketConfig.Packet.SockAddr saAddr = PacketConfig.Packet.GetSocketAddr_ByIPString(sIPString);
                                        res = WSock32.sendto(Socket, ipSend, bSendBuffer.Length, SocketFlags.None, ref saAddr, Marshal.SizeOf(saAddr));
                                    }
                                    break;
                                case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                                case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                                case Operate.PacketConfig.Packet.PacketType.WSASendTo:
                                case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:
                                case Operate.PacketConfig.Packet.PacketType.UDP_Req:
                                case Operate.PacketConfig.Packet.PacketType.UDP_Resp:
                                    if (!string.IsNullOrEmpty(sIPString))
                                    {
                                        Operate.PacketConfig.Packet.SockAddr saAddr = PacketConfig.Packet.GetSocketAddr_ByIPString(sIPString);
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
                        Operate.DoLog(nameof(SendPacket), ex.Message);
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

                #region//编辑发送

                public static void OpenPacketEdit(Form form, PacketInfo pi)
                {
                    var PacketEdit = new PacketEdit(form, pi);
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("PacketEditForm", "封包编辑"), PacketEdit)
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                public static void OpenPacketEdit(Form form, ProxyInfo pi)
                {
                    var PacketEdit = new PacketEdit(form, pi);
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("PacketEditForm", "封包编辑"), PacketEdit)
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//获取封包收发速率

                public static string GetPacketSpeedInfo()
                {
                    string sReturn = string.Empty;

                    try
                    {
                        string sTotal_SendBytes = Operate.SystemConfig.GetDisplayBytes(Operate.PacketConfig.Packet.Total_SendBytes);
                        string sTotal_RecvBytes = Operate.SystemConfig.GetDisplayBytes(Operate.PacketConfig.Packet.Total_RecvBytes);
                        string sSpeedInfo = AntdUI.Localization.Get("InjectModeForm.SpeedInfo", "发送 : {0}  接收 : {1}");
                        sReturn = string.Format(sSpeedInfo, sTotal_SendBytes, sTotal_RecvBytes);
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(GetPacketSpeedInfo), ex.Message);
                    }

                    return sReturn;
                }

                #endregion

                #region//获取封包类型

                public static PacketType GetPacketType_ByString(string PacketType)
                {
                    PacketType ptReturn = new PacketType();

                    try
                    {
                        ptReturn = (PacketType)Enum.Parse(typeof(PacketType), PacketType);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetPacketType_ByString), ex.Message);
                    }

                    return ptReturn;
                }

                #endregion

                #region//获取封包类型对应的名称

                private static class PacketTypeNames
                {
                    public static string WS1_Send => AntdUI.Localization.Get("HookSettingsForm.Send1", "发送 1.1");
                    public static string WS2_Send => AntdUI.Localization.Get("HookSettingsForm.Send", "发送");
                    public static string WS1_Recv => AntdUI.Localization.Get("HookSettingsForm.Recv1", "接收 1.1");
                    public static string WS2_Recv => AntdUI.Localization.Get("HookSettingsForm.Recv", "接收");
                    public static string WS1_SendTo => AntdUI.Localization.Get("HookSettingsForm.SendTo1", "发送到 1.1");
                    public static string WS2_SendTo => AntdUI.Localization.Get("HookSettingsForm.SendTo", "发送到");
                    public static string WS1_RecvFrom => AntdUI.Localization.Get("HookSettingsForm.RecvFrom1", "接收自 1.1");
                    public static string WS2_RecvFrom => AntdUI.Localization.Get("HookSettingsForm.RecvFrom", "接收自");
                    public static string WSASend => AntdUI.Localization.Get("HookSettingsForm.WSASend", "WSA发送");
                    public static string WSARecv => AntdUI.Localization.Get("HookSettingsForm.WSARecv", "WSA接收");
                    public static string WSARecvEx => AntdUI.Localization.Get("HookSettingsForm.WSARecv", "WSA接收");
                    public static string WSASendTo => AntdUI.Localization.Get("HookSettingsForm.WSASendTo", "WSA发送到");
                    public static string WSARecvFrom => AntdUI.Localization.Get("HookSettingsForm.WSARecvFrom", "WSA接收自");
                    public static string TCP_Req => AntdUI.Localization.Get("HookSettingsForm.TCP_Req", "TCP 请求");
                    public static string UDP_Req => AntdUI.Localization.Get("HookSettingsForm.UDP_Req", "UDP 请求");
                    public static string TCP_Resp => AntdUI.Localization.Get("HookSettingsForm.TCP_Resp", "TCP 响应");
                    public static string UDP_Resp => AntdUI.Localization.Get("HookSettingsForm.UDP_Resp", "UDP 响应");
                }

                public static string GetName_ByPacketType(PacketType socketType)
                {
                    try
                    {
                        switch (socketType)
                        {
                            case PacketType.WS1_Send:
                                return PacketTypeNames.WS1_Send;

                            case PacketType.WS2_Send:
                                return PacketTypeNames.WS2_Send;

                            case PacketType.WS1_Recv:
                                return PacketTypeNames.WS1_Recv;

                            case PacketType.WS2_Recv:
                                return PacketTypeNames.WS2_Recv;

                            case PacketType.WS1_SendTo:
                                return PacketTypeNames.WS1_SendTo;

                            case PacketType.WS2_SendTo:
                                return PacketTypeNames.WS2_SendTo;

                            case PacketType.WS1_RecvFrom:
                                return PacketTypeNames.WS1_RecvFrom;

                            case PacketType.WS2_RecvFrom:
                                return PacketTypeNames.WS2_RecvFrom;

                            case PacketType.WSASend:
                                return PacketTypeNames.WSASend;

                            case PacketType.WSARecv:
                                return PacketTypeNames.WSARecv;

                            case PacketType.WSARecvEx:
                                return PacketTypeNames.WSARecvEx;

                            case PacketType.WSASendTo:
                                return PacketTypeNames.WSASendTo;

                            case PacketType.WSARecvFrom:
                                return PacketTypeNames.WSARecvFrom;

                            case PacketType.TCP_Req:
                                return PacketTypeNames.TCP_Req;

                            case PacketType.UDP_Req:
                                return PacketTypeNames.UDP_Req;

                            case PacketType.TCP_Resp:
                                return PacketTypeNames.TCP_Resp;

                            case PacketType.UDP_Resp:
                                return PacketTypeNames.UDP_Resp;

                            default:
                                return string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetName_ByPacketType), ex.Message);
                        return string.Empty;
                    }
                }

                #endregion

                #region//获取封包类型对应的图标                

                public static Bitmap GetImg_ByPacketType(PacketType ptType)
                {
                    try
                    {                        
                        switch (ptType)
                        {
                            case PacketConfig.Packet.PacketType.WS1_Send:
                            case PacketConfig.Packet.PacketType.WS2_Send:
                            case PacketConfig.Packet.PacketType.WS1_SendTo:
                            case PacketConfig.Packet.PacketType.WS2_SendTo:
                            case PacketConfig.Packet.PacketType.WSASend:
                            case PacketConfig.Packet.PacketType.WSASendTo:
                            case PacketConfig.Packet.PacketType.TCP_Req:
                            case PacketConfig.Packet.PacketType.UDP_Req:
                                return Properties.Resources.Send;

                            case PacketConfig.Packet.PacketType.WS1_Recv:
                            case PacketConfig.Packet.PacketType.WS2_Recv:
                            case PacketConfig.Packet.PacketType.WS1_RecvFrom:
                            case PacketConfig.Packet.PacketType.WS2_RecvFrom:
                            case PacketConfig.Packet.PacketType.WSARecv:
                            case PacketConfig.Packet.PacketType.WSARecvEx:
                            case PacketConfig.Packet.PacketType.WSARecvFrom:
                            case PacketConfig.Packet.PacketType.TCP_Resp:
                            case PacketConfig.Packet.PacketType.UDP_Resp:
                                return Properties.Resources.Recv;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetImg_ByPacketType), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//是否显示封包（过滤条件）

                public static bool IsShowPacket_ByFilter(PacketInfo pi)
                {
                    return IsShowByFilter(
                        pi.PacketSocket,
                        pi.PacketFrom,
                        pi.PacketTo,
                        pi.PacketBuffer,
                        pi.PacketLen,
                        pi.PacketType
                    );
                }

                public static bool IsShowProxy_ByFilter(ProxyInfo pi)
                {
                    return IsShowByFilter(
                        pi.PacketSocket,
                        pi.ClientAddr,
                        pi.ServerAddr,
                        pi.PacketBuffer,
                        pi.PacketLen,
                        pi.PacketType
                    );
                }

                private static bool IsShowByFilter(
                    int packetSocket, 
                    string fromAddr, 
                    string toAddr,
                    byte[] packetBuffer, 
                    int packetLen, 
                    PacketConfig.Packet.PacketType packetType)
                {
                    try
                    {
                        //套接字
                        if (SystemConfig.CheckSocket)
                        {
                            bool bIsFilter = IsFilter_BySocket(packetSocket);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //IP地址
                        if (SystemConfig.CheckIP)
                        {
                            bool bIsFilter_From = IsFilter_ByIP(fromAddr);
                            bool bIsFilter_To = IsFilter_ByIP(toAddr);
                            if (SystemConfig.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                            {
                                return false;
                            }
                        }

                        //端口号
                        if (SystemConfig.CheckPort)
                        {
                            bool bIsFilter_From = IsFilter_ByPort(fromAddr);
                            bool bIsFilter_To = IsFilter_ByPort(toAddr);
                            if (SystemConfig.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                            {
                                return false;
                            }
                        }

                        //指定包头
                        if (SystemConfig.CheckHead)
                        {
                            bool bIsFilter = IsFilter_ByHead(packetBuffer);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //封包内容
                        if (SystemConfig.CheckData)
                        {
                            bool bIsFilter = IsFilter_ByPacket(packetBuffer);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //封包大小
                        if (SystemConfig.CheckLen)
                        {
                            bool bIsFilter = IsFilter_BySize(packetLen);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //封包类别
                        if (SystemConfig.CheckType)
                        {
                            bool bIsFilter = FilterConfig.Filter.CheckFilterFunction_ByPacketType(packetType, Operate.SystemConfig.CheckType_Value);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(IsShowByFilter), ex.Message);
                    }

                    return true;
                }

                #region//检测套接字

                public static bool IsFilter_BySocket(int iPacketSocket)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(SystemConfig.CheckSocket_Value))
                        {
                            string[] sSocketArr = SystemConfig.CheckSocket_Value.Split(';');
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
                        Operate.DoLog(nameof(IsFilter_BySocket), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//检测IP地址

                public static bool IsFilter_ByIP(string sPacketIP)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(sPacketIP) || string.IsNullOrEmpty(SystemConfig.CheckIP_Value))
                        {
                            return false;
                        }

                        string sIP = sPacketIP.Split(':')[0];
                        HashSet<string> ipSet = new HashSet<string>(SystemConfig.CheckIP_Value.Split(';'));

                        return ipSet.Contains(sIP);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(IsFilter_ByIP), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//检测端口号

                public static bool IsFilter_ByPort(string sPacketPort)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(sPacketPort) || string.IsNullOrEmpty(SystemConfig.CheckPort_Value))
                        {
                            return false;
                        }

                        string sPort = sPacketPort.Split(':')[1];
                        HashSet<string> portSet = new HashSet<string>(SystemConfig.CheckPort_Value.Split(';'));

                        return portSet.Contains(sPort);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(IsFilter_ByPort), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//检测包头

                public static bool IsFilter_ByHead(byte[] bBuffer)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(SystemConfig.CheckHead_Value))
                        {
                            return false;
                        }

                        string checkHeadValue = SystemConfig.CheckHead_Value.Replace(" ", "");
                        string[] headValues = checkHeadValue.Split(';');

                        foreach (string headValue in headValues)
                        {
                            if (!string.IsNullOrEmpty(headValue))
                            {
                                byte[] headBytes = SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, headValue);

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
                        Operate.DoLog(nameof(IsFilter_ByHead), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//检测封包内容

                public static bool IsFilter_ByPacket(byte[] bBuffer)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(SystemConfig.CheckData_Value))
                        {
                            return false;
                        }

                        string checkDataValue = SystemConfig.CheckData_Value.Replace(" ", "");
                        string[] checkDataArray = checkDataValue.Split(';');

                        string packetString = SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, bBuffer).Replace(" ", "");

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
                        Operate.DoLog(nameof(IsFilter_ByPacket), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//检测封包大小

                public static bool IsFilter_BySize(int PacketLength)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(SystemConfig.CheckLength_Value))
                        {
                            return false;
                        }

                        string[] lengthArray = SystemConfig.CheckLength_Value.Split(';');

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
                        Operate.DoLog(nameof(IsFilter_BySize), ex.Message);
                    }

                    return false;
                }

                #endregion                

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
                            sReturn = SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, bBuffSlice) + " ...";
                        }
                        else
                        {
                            sReturn = SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, bBuff);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetPacketData_Hex), ex.Message);
                    }

                    return sReturn;
                }

                #endregion

                #region//获取封包数据的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_PacketData(HexBox hbPacketData)
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                    menuItems.Add(new AntdUI.ContextMenuStripItem("编辑")
                    {
                        ID = "Edit",
                        IconSvg = "EditOutlined",
                        LocalizationText = "Edit",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    if (SendConfig.List.lstSendInfo.Count > 0)
                    {
                        menuItems.Add(new AntdUI.ContextMenuStripItem("添加到发送列表")
                        {
                            ID = "ToSendList",
                            IconSvg = "PlaySquareOutlined",
                            LocalizationText = "ToSendList",
                            Sub = Operate.SendConfig.List.GetCMS_ToSendList(),
                        });
                    }
                    else
                    {
                        menuItems.Add(new AntdUI.ContextMenuStripItem("添加到发送列表")
                        {
                            Enabled = false,
                            ID = "ToSendList",
                            IconSvg = "PlaySquareOutlined",
                            LocalizationText = "ToSendList",
                        });
                    }

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到滤镜列表")
                    {
                        ID = "ToFilterList",
                        IconSvg = "FunnelPlotOutlined",
                        LocalizationText = "ToFilterList",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("复制")
                    {
                        Enabled = hbPacketData.CanCopy(),
                        ID = "Copy",
                        IconSvg = "CopyOutlined",
                        LocalizationText = "Copy",
                        Sub = new AntdUI.IContextMenuStripItem[]
                        {
                            new AntdUI.ContextMenuStripItem("复制文本")
                            {
                                Enabled = hbPacketData.CanCopy(),
                                ID = "Copy_Text",
                                IconSvg = "CopyOutlined",
                                LocalizationText = "CopyText",
                            },
                            new AntdUI.ContextMenuStripItem("复制十六进制")
                            {
                                Enabled = hbPacketData.CanCopy(),
                                ID = "Copy_Hex",
                                IconSvg = "CopyOutlined",
                                LocalizationText = "CopyHex",
                            },
                        },
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到文本A")
                    {
                        ID = "ToTextA",
                        IconSvg = "FontColorsOutlined",
                        LocalizationText = "ToTextA",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到文本B")
                    {
                        ID = "ToTextB",
                        IconSvg = "BoldOutlined",
                        LocalizationText = "ToTextB",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("全选")
                    {
                        ID = "SelectAll",
                        IconSvg = "ProfileOutlined",
                        LocalizationText = "SelectAll",
                    });

                    return menuItems.ToArray();
                }

                #endregion

                #region//获取封包编辑的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_PacketEdit(HexBox hbPacketData)
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                    if (SendConfig.List.lstSendInfo.Count > 0)
                    {
                        menuItems.Add(new AntdUI.ContextMenuStripItem("添加到发送列表")
                        {
                            ID = "ToSendList",
                            IconSvg = "PlaySquareOutlined",
                            LocalizationText = "ToSendList",
                            Sub = Operate.SendConfig.List.GetCMS_ToSendList(),
                        });
                    }
                    else
                    {
                        menuItems.Add(new AntdUI.ContextMenuStripItem("添加到发送列表")
                        {
                            Enabled = false,
                            ID = "ToSendList",
                            IconSvg = "PlaySquareOutlined",
                            LocalizationText = "ToSendList",
                        });
                    }

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到滤镜列表")
                    {
                        ID = "ToFilterList",
                        IconSvg = "FunnelPlotOutlined",
                        LocalizationText = "ToFilterList",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("剪切")
                    {
                        Enabled = hbPacketData.CanCut(),
                        ID = "Cut",
                        IconSvg = "ScissorOutlined",
                        LocalizationText = "Cut",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("复制")
                    {
                        Enabled = hbPacketData.CanCopy(),
                        ID = "Copy",
                        IconSvg = "CopyOutlined",
                        LocalizationText = "Copy",
                        Sub = new AntdUI.IContextMenuStripItem[]
                        {
                            new AntdUI.ContextMenuStripItem("复制文本")
                            {
                                Enabled = hbPacketData.CanCopy(),
                                ID = "Copy_Text",
                                IconSvg = "CopyOutlined",
                                LocalizationText = "CopyText",
                            },
                            new AntdUI.ContextMenuStripItem("复制十六进制")
                            {
                                Enabled = hbPacketData.CanCopy(),
                                ID = "Copy_Hex",
                                IconSvg = "CopyOutlined",
                                LocalizationText = "CopyHex",
                            },
                        },
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("粘贴")
                    {
                        Enabled = hbPacketData.CanPaste(),
                        ID = "Paste",
                        IconSvg = "SnippetsOutlined",
                        LocalizationText = "Paste",
                        Sub = new AntdUI.IContextMenuStripItem[]
                        {
                            new AntdUI.ContextMenuStripItem("粘贴文本")
                            {
                                Enabled = hbPacketData.CanPaste(),
                                ID = "Paste_Text",
                                IconSvg = "SnippetsOutlined",
                                LocalizationText = "PasteText",
                            },
                            new AntdUI.ContextMenuStripItem("粘贴十六进制")
                            {
                                Enabled = hbPacketData.CanPasteHex(),
                                ID = "Paste_Hex",
                                IconSvg = "SnippetsOutlined",
                                LocalizationText = "PasteHex",
                            },
                        },
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("全选")
                    {
                        ID = "SelectAll",
                        IconSvg = "ProfileOutlined",
                        LocalizationText = "SelectAll",
                    });

                    return menuItems.ToArray();
                }

                #endregion

                #region//获取 SockAddr 对应的 IP 地址和端口

                public static string GetIPString_BySocketAddr(int pSocket, Operate.PacketConfig.Packet.SockAddr pAddr, Operate.PacketConfig.Packet.PacketType pType)
                {
                    string sIP_From = string.Empty;
                    string sIP_To = string.Empty;

                    try
                    {
                        sIP_From = PacketConfig.Packet.GetIP_BySocket(pSocket, Operate.PacketConfig.Packet.IPType.From);

                        switch (pType)
                        {
                            case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                            case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                            case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                            case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                            case Operate.PacketConfig.Packet.PacketType.WSASend:
                            case Operate.PacketConfig.Packet.PacketType.WSARecv:
                            case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                            case Operate.PacketConfig.Packet.PacketType.TCP_Req:
                            case Operate.PacketConfig.Packet.PacketType.UDP_Req:
                            case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                            case Operate.PacketConfig.Packet.PacketType.UDP_Resp:

                                sIP_To = PacketConfig.Packet.GetIP_BySocket(pSocket, Operate.PacketConfig.Packet.IPType.To);

                                break;

                            case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                            case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                            case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                            case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                            case Operate.PacketConfig.Packet.PacketType.WSASendTo:
                            case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:

                                sIP_To = PacketConfig.Packet.GetIP_BySockAddr(pAddr);

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
                        Operate.DoLog(nameof(GetIPString_BySocketAddr), ex.Message);
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
                            string sIP = Marshal.PtrToStringAnsi(WS2_32.inet_ntoa(saAddr.sin_addr));
                            string sPort = WS2_32.ntohs(saAddr.sin_port).ToString();
                            sReturn = $"{sIP}:{sPort}";
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetIP_BySockAddr), ex.Message);
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
                                WS2_32.getsockname(Socket, ref saAddr, ref iAddrLen);
                                break;

                            case Operate.PacketConfig.Packet.IPType.To:
                                WS2_32.getpeername(Socket, ref saAddr, ref iAddrLen);
                                break;
                        }

                        sReturn = GetIP_BySockAddr(saAddr);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetIP_BySocket), ex.Message);
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
                        Operate.DoLog(nameof(GetSocketAddr_ByIPString), ex.Message);
                    }

                    return saReturn;
                }

                #endregion

                #region//统计封包数量

                public static void CountPacketInfo(Operate.PacketConfig.Packet.PacketType ptPacketType, int packetLength)
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
                                    Interlocked.Increment(ref Operate.PacketConfig.Packet.Send_CNT);
                                    Interlocked.Add(ref Operate.PacketConfig.Packet.Total_SendBytes, packetLength);
                                    break;

                                case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                                case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                                    Interlocked.Increment(ref Operate.PacketConfig.Packet.SendTo_CNT);
                                    Interlocked.Add(ref Operate.PacketConfig.Packet.Total_SendBytes, packetLength);
                                    break;

                                case Operate.PacketConfig.Packet.PacketType.WSASend:
                                    Interlocked.Increment(ref Operate.PacketConfig.Packet.WSASend_CNT);
                                    Interlocked.Add(ref Operate.PacketConfig.Packet.Total_SendBytes, packetLength);
                                    break;

                                case Operate.PacketConfig.Packet.PacketType.WSASendTo:
                                    Interlocked.Increment(ref Operate.PacketConfig.Packet.WSASendTo_CNT);
                                    Interlocked.Add(ref Operate.PacketConfig.Packet.Total_SendBytes, packetLength);
                                    break;

                                case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                                case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                                    Interlocked.Increment(ref Operate.PacketConfig.Packet.Recv_CNT);
                                    Interlocked.Add(ref Operate.PacketConfig.Packet.Total_RecvBytes, packetLength);
                                    break;

                                case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                                case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                                    Interlocked.Increment(ref Operate.PacketConfig.Packet.RecvFrom_CNT);
                                    Interlocked.Add(ref Operate.PacketConfig.Packet.Total_RecvBytes, packetLength);
                                    break;

                                case Operate.PacketConfig.Packet.PacketType.WSARecv:
                                case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                                    Interlocked.Increment(ref Operate.PacketConfig.Packet.WSARecv_CNT);
                                    Interlocked.Add(ref Operate.PacketConfig.Packet.Total_RecvBytes, packetLength);
                                    break;

                                case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:
                                    Interlocked.Increment(ref Operate.PacketConfig.Packet.WSARecvFrom_CNT);
                                    Interlocked.Add(ref Operate.PacketConfig.Packet.Total_RecvBytes, packetLength);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CountPacketInfo), ex.Message);
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
            }

            #endregion

            #region//封包队列

            public static class Queue
            {
                public static ConcurrentQueue<PacketInfo> cqPacketInfo = new ConcurrentQueue<PacketInfo>();

                #region//封包入队列            

                public static async void PacketToQueue(
                    int iSocket,
                    byte[] bRawBuff,
                    byte[] bBuffByte,
                    PacketConfig.Packet.PacketType ptPacketType,
                    PacketConfig.Packet.SockAddr sAddr,
                    FilterConfig.Filter.FilterAction pAction,
                    DateTime PacketTime)
                {
                    try
                    {
                        PacketConfig.Packet.CountPacketInfo(ptPacketType, bBuffByte.Length);

                        if (!SystemConfig.SpeedMode)
                        {
                            string sPacketIP = PacketConfig.Packet.GetIPString_BySocketAddr(iSocket, sAddr, ptPacketType);

                            if (!string.IsNullOrEmpty(sPacketIP) && sPacketIP.Contains("|"))
                            {
                                string[] ipParts = sPacketIP.Split('|');
                                string sIPFrom = ipParts[0];
                                string sIPTo = ipParts[1];
                                string sFromLocation = await SystemConfig.GetIPLocation(sIPFrom.Split(':')[0]);
                                string sToLocation = await SystemConfig.GetIPLocation(sIPTo.Split(':')[0]);

                                PacketInfo pi = new PacketInfo(
                                    PacketTime, 
                                    iSocket, 
                                    ptPacketType, 
                                    sIPFrom, 
                                    sFromLocation,
                                    sIPTo, 
                                    sToLocation,
                                    bRawBuff, 
                                    bBuffByte, 
                                    bBuffByte.Length, 
                                    pAction);

                                cqPacketInfo.Enqueue(pi);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(PacketToQueue), ex.Message);
                    }
                }

                #endregion

                #region//清除封包队列

                public static void ClearPacketQueue()
                {
                    try
                    {
                        while (!cqPacketInfo.IsEmpty)
                        {
                            cqPacketInfo.TryDequeue(out PacketInfo spc);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ClearPacketQueue), ex.Message);
                    }
                }

                #endregion
            }

            #endregion

            #region//封包列表

            public static class List
            {
                public static bool IsShow_ID = true;
                public static bool IsShow_ProxyTime = true;
                public static bool IsShow_PacketType = true;
                public static bool IsShow_PacketSocket = true;
                public static bool IsShow_ClientAddr = true;
                public static bool IsShow_ClientLocation = true;
                public static bool IsShow_ServerAddr = true;
                public static bool IsShow_ServerLocation = true;
                public static bool IsShow_PacketLen = true;
                public static bool IsShow_PacketData = true;
                public static bool AutoRoll = false;
                public static bool AutoClear = true;
                public static decimal AutoClear_Value = 5000;
                public static int Search_Index = -1;
                public static FindOptions FindOptions = new FindOptions();
                public static string FindRegex = string.Empty;
                public static PacketInfo piSelect;
                public static BindingList<PacketInfo> lstPacketInfo = new BindingList<PacketInfo>();

                #region//封包入列表

                public static void PacketToList()
                {
                    try
                    {
                        if (PacketConfig.Queue.cqPacketInfo.TryDequeue(out PacketInfo pi))
                        {
                            bool bIsShow = PacketConfig.Packet.IsShowPacket_ByFilter(pi);
                            if (bIsShow)
                            {
                                Span<byte> bufferSpan = pi.PacketBuffer.AsSpan();
                                pi.PacketData = PacketConfig.Packet.GetPacketData_Hex(bufferSpan, PacketConfig.Packet.PacketData_MaxLen);

                                if (Operate.SystemConfig.InvokeAction != null)
                                {
                                    Operate.SystemConfig.InvokeAction(() =>
                                    {
                                        Operate.PacketConfig.List.lstPacketInfo.Add(pi);
                                    });
                                }
                                else
                                {
                                    Operate.PacketConfig.List.lstPacketInfo.Add(pi);
                                }                                
                            }
                            else
                            {
                                PacketConfig.Packet.FilterPacket_CNT++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(PacketToList), ex.Message);
                    }
                }

                #endregion

                #region//清除封包列表

                public static void ClearPacketList()
                {
                    try
                    {
                        if (Operate.SystemConfig.InvokeAction != null)
                        {
                            Operate.SystemConfig.InvokeAction(() =>
                            {
                                Operate.PacketConfig.List.lstPacketInfo.Clear();
                            });
                        }
                        else
                        {
                            Operate.PacketConfig.List.lstPacketInfo.Clear();
                        }                        
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ClearPacketList), ex.Message);
                    }
                }

                #endregion

                #region //搜索封包列表

                public static int SearchForList<T>(int fromIndex, bool isPacketList = true) where T : class
                {
                    int iResult = -1;

                    try
                    {
                        if (!Operate.PacketConfig.List.FindOptions.IsValid)
                        {
                            return -1;
                        }

                        if (fromIndex < 0)
                        {
                            return -1;
                        }

                        int listCount;
                        IList<T> listItems;

                        if (isPacketList)
                        {
                            listCount = PacketConfig.List.lstPacketInfo.Count;
                            listItems = PacketConfig.List.lstPacketInfo as IList<T>;
                        }
                        else
                        {
                            listCount = ProxyConfig.List.lstProxyInfo.Count;
                            listItems = ProxyConfig.List.lstProxyInfo as IList<T>;
                        }

                        if (listItems == null || listCount == 0 || fromIndex >= listCount)
                        {
                            return -1;
                        }

                        switch (PacketConfig.List.FindOptions.Type)
                        {
                            case FindType.Text:

                                for (int i = fromIndex; i < listCount; i++)
                                {
                                    ReadOnlySpan<byte> packetBuffer = GetPacketBuffer(listItems[i], isPacketList);
                                    string packetData = SystemConfig.BytesToString(PacketConfig.Packet.EncodingFormat.UTF8, packetBuffer);

                                    try
                                    {
                                        Match mFind = Regex.Match(packetData, PacketConfig.List.FindRegex);
                                        if (mFind.Success)
                                        {
                                            PacketConfig.List.FindOptions.Text = mFind.Value;
                                            return i;
                                        }
                                    }
                                    catch
                                    {
                                        // 正则表达式错误
                                        return -1;
                                    }
                                }

                                break;

                            case FindType.Hex:

                                if (!string.IsNullOrEmpty(PacketConfig.List.FindRegex))
                                {
                                    for (int i = fromIndex; i < listCount; i++)
                                    {
                                        ReadOnlySpan<byte> packetBuffer = GetPacketBuffer(listItems[i], isPacketList);
                                        string packetData = SystemConfig.BytesToString(PacketConfig.Packet.EncodingFormat.Hex, packetBuffer);

                                        try
                                        {
                                            Match mFind = Regex.Match(packetData, PacketConfig.List.FindRegex);
                                            if (mFind.Success)
                                            {
                                                byte[] bHex = SystemConfig.StringToBytes(PacketConfig.Packet.EncodingFormat.Hex, mFind.Value);
                                                if (bHex.Length == 0)
                                                {
                                                    return -1;
                                                }

                                                Operate.PacketConfig.List.FindOptions.Hex = bHex;
                                                return i;
                                            }
                                        }
                                        catch
                                        {
                                            // 正则表达式错误
                                            return -1;
                                        }
                                    }
                                }

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SearchForList), ex.Message);
                    }

                    return iResult;
                }

                private static ReadOnlySpan<byte> GetPacketBuffer<T>(T item, bool isPacketList)
                {
                    if (isPacketList && item is PacketInfo packetInfo)
                    {
                        return packetInfo.PacketBuffer.AsSpan();
                    }
                    else if (!isPacketList && item is ProxyInfo proxyInfo)
                    {
                        return proxyInfo.PacketBuffer.AsSpan();
                    }

                    return ReadOnlySpan<byte>.Empty;
                }

                #endregion

                #region//封包列表统计

                public static DataTable StatisticalSocketList_ByPacketLen()
                {
                    DataTable dtReturn = new DataTable();
                    dtReturn.Columns.Add("PacketLength", typeof(int));
                    dtReturn.Columns.Add("Number", typeof(int));

                    try
                    {
                        Dictionary<int, int> packetLenCount = new Dictionary<int, int>();

                        foreach (PacketInfo packetInfo in lstPacketInfo)
                        {
                            int packetLen = packetInfo.PacketLen;

                            if (packetLenCount.ContainsKey(packetLen))
                            {
                                packetLenCount[packetLen]++;
                            }
                            else
                            {
                                packetLenCount.Add(packetLen, 1);
                            }
                        }

                        Dictionary<int, int> sortedByKeyAsc = SystemConfig.SortDictionaryByKey(packetLenCount, ascending: true);

                        foreach (KeyValuePair<int, int> kvp in sortedByKeyAsc)
                        {
                            DataRow row = dtReturn.NewRow();
                            row[0] = kvp.Key;
                            row[1] = kvp.Value;
                            dtReturn.Rows.Add(row);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(StatisticalSocketList_ByPacketLen), ex.Message);
                    }

                    return dtReturn;
                }

                public static DataTable StatisticalSocketList_ByPacketSocket()
                {
                    DataTable dtReturn = new DataTable();
                    dtReturn.Columns.Add("PacketSocket", typeof(int));
                    dtReturn.Columns.Add("Number", typeof(int));

                    try
                    {
                        Dictionary<int, int> packetLenCount = new Dictionary<int, int>();

                        foreach (PacketInfo packetInfo in lstPacketInfo)
                        {
                            int packetLen = packetInfo.PacketSocket;

                            if (packetLenCount.ContainsKey(packetLen))
                            {
                                packetLenCount[packetLen]++;
                            }
                            else
                            {
                                packetLenCount.Add(packetLen, 1);
                            }
                        }

                        Dictionary<int, int> sortedByKeyAsc = SystemConfig.SortDictionaryByKey(packetLenCount, ascending: true);

                        foreach (KeyValuePair<int, int> kvp in sortedByKeyAsc)
                        {
                            DataRow row = dtReturn.NewRow();
                            row[0] = kvp.Key;
                            row[1] = kvp.Value;
                            dtReturn.Rows.Add(row);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(StatisticalSocketList_ByPacketSocket), ex.Message);
                    }

                    return dtReturn;
                }                

                #endregion

                #region//获取封包列表的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_PacketList()
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                    menuItems.Add(new AntdUI.ContextMenuStripItem("编辑")
                    {
                        ID = "Edit",
                        IconSvg = "EditOutlined",
                        LocalizationText = "Edit",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("复制", "Ctrl+C")
                    {
                        ID = "Copy",
                        IconSvg = "CopyOutlined",
                        LocalizationText = "Copy",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    if (SendConfig.List.lstSendInfo.Count > 0)
                    {
                        menuItems.Add(new AntdUI.ContextMenuStripItem("添加到发送列表")
                        {
                            ID = "ToSendList",
                            IconSvg = "PlaySquareOutlined",
                            LocalizationText = "ToSendList",
                            Sub = Operate.SendConfig.List.GetCMS_ToSendList(),
                        });
                    }
                    else
                    {
                        menuItems.Add(new AntdUI.ContextMenuStripItem("添加到发送列表")
                        {
                            Enabled = false,
                            ID = "ToSendList",
                            IconSvg = "PlaySquareOutlined",
                            LocalizationText = "ToSendList",                            
                        });
                    }

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到滤镜列表")
                    {
                        ID = "ToFilterList",
                        IconSvg = "FunnelPlotOutlined",
                        LocalizationText = "ToFilterList",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("设置系统套接字")
                    {
                        ID = "SYSSocket",
                        IconSvg = "CheckSquareOutlined",
                        LocalizationText = "SetSSocket",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("查看数据修改")
                    {
                        ID = "PacketModification",
                        IconSvg = "FormOutlined",
                        LocalizationText = "PacketModification",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("导出到Excel")
                    {
                        ID = "ToExcel",
                        IconSvg = "FileExcelOutlined",
                        LocalizationText = "SaveToExcel",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到文本A")
                    {
                        ID = "ToTextA",
                        IconSvg = "FontColorsOutlined",
                        LocalizationText = "ToTextA",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到文本B")
                    {
                        ID = "ToTextB",
                        IconSvg = "BoldOutlined",
                        LocalizationText = "ToTextB",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("全选", "Ctrl+A")
                    {
                        ID = "SelectAll",
                        IconSvg = "UnorderedListOutlined",
                        LocalizationText = "SelectAll",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("取消选择")
                    {
                        ID = "DeSelect",
                        IconSvg = "DeleteRowOutlined",
                        LocalizationText = "DeSelect",
                    });

                    return menuItems.ToArray();
                }

                #endregion

                #region//发送封包列表中当前选中的封包

                public static void SendSocketList_BySelect()
                {
                    try
                    {
                        if (PacketConfig.List.piSelect != null)
                        {
                            int Socket = PacketConfig.List.piSelect.PacketSocket;
                            PacketConfig.Packet.PacketType ptType = PacketConfig.List.piSelect.PacketType;
                            string From = PacketConfig.List.piSelect.PacketFrom;
                            string To = PacketConfig.List.piSelect.PacketTo;
                            byte[] bBuffer = PacketConfig.List.piSelect.PacketBuffer;

                            Operate.PacketConfig.Packet.SendPacket(Socket, ptType, From, To, bBuffer);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SendSocketList_BySelect), ex.Message);
                    }
                }

                #endregion

                #region//保存封包列表为Excel（对话框）

                public static void SavePacketList_Dialog(Form form, string FileName, List<PacketInfo> piList)
                {
                    try
                    {
                        if (PacketConfig.List.lstPacketInfo.Count > 0)
                        {
                            int SaveCount = PacketConfig.List.lstPacketInfo.Count;

                            SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                            sfdSaveToExcel.Filter = AntdUI.Localization.Get("ExcelFile", "Excel 文件") + " (*.xls)|*.xls";                            
                            sfdSaveToExcel.RestoreDirectory = true;

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveToExcel.FileName = FileName;
                            }

                            if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveToExcel.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    bool bOK = SavePacketListToExcel(FilePath, piList);
                                    if (bOK)
                                    {
                                        string Title = AntdUI.Localization.Get("ExportToExcel.Success", "导出到Excel成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SavePacketList_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("ExportToExcel.Error", "导出到Excel失败");
                                        string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SavePacketList_Dialog), ex.Message);
                    }
                }

                private static bool SavePacketListToExcel(string filePath, List<PacketInfo> piList)
                {
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        using (var writer = new StreamWriter(stream, Encoding.Default))
                        {
                            writer.WriteLine(AntdUI.Localization.Get("ExcelColumn.Packet", "时间戳\t类别\t套接字\t源地址\t目的地址\t长度\t数据\t"));

                            var dataSource = piList.Count > 0 ? piList : PacketConfig.List.lstPacketInfo.ToList();
                            foreach (var packet in dataSource)
                            {
                                try
                                {
                                    var lineBuilder = new StringBuilder();

                                    lineBuilder.Append(packet.PacketTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff")).Append('\t');
                                    lineBuilder.Append(packet.PacketType).Append('\t');
                                    lineBuilder.Append(packet.PacketSocket).Append('\t');
                                    lineBuilder.Append(packet.PacketFrom).Append('\t');
                                    lineBuilder.Append(packet.PacketTo).Append('\t');
                                    lineBuilder.Append(packet.PacketLen).Append('\t');
                                    lineBuilder.Append(SystemConfig.BytesToString(PacketConfig.Packet.EncodingFormat.Hex, packet.PacketBuffer)).Append('\t');

                                    writer.WriteLine(lineBuilder.ToString());
                                }
                                catch (Exception ex)
                                {
                                    Operate.DoLog(nameof(SavePacketListToExcel), ex.Message);
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SavePacketListToExcel), ex.Message);
                        return false;
                    }
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region//滤镜配置

        public static class FilterConfig
        {
            #region//滤镜

            public static class Filter
            {
                public static long FilterExecute_CNT = 0;
                public static long FilterReplace_CNT = 0;
                public static long FilterChange_CNT = 0;
                public static long FilterIntercept_CNT = 0;
                public static long FilterDisplay_CNT = 0;
                public static long FilterNoDisplay_CNT = 0;
                public static int FilterSize_MaxLen = 1000;
                public static FilterConfig.Filter.Execute FilterExecute = FilterConfig.Filter.Execute.Sequence;
                public static Color FilterReplace_ForeColor = Color.Black;
                public static Color FilterReplace_BackColor = Color.Goldenrod;
                public static Color FilterIntercept_ForeColor = Color.White;
                public static Color FilterIntercept_BackColor = Color.DarkRed;
                public static Color FilterChange_ForeColor = Color.Black;
                public static Color FilterChange_BackColor = Color.DodgerBlue;

                #region//定义结构

                public enum Execute
                {
                    Priority,
                    Sequence,
                }

                public enum FilterMode
                {
                    Normal,
                    Advanced,
                }

                public enum FilterAction
                {
                    Replace,
                    Intercept,
                    NoModify_Display,
                    NoModify_NoDisplay,
                    None,
                    Change,
                }

                public enum FilterExecuteType
                {                    
                    Send,
                    Robot,
                    None,
                    Filter,
                }

                public enum FilterStartFrom
                {
                    Head,
                    Position,
                }

                public struct FilterFunction
                {
                    public bool Send;// 0
                    public bool SendTo;// 1
                    public bool Recv;// 2
                    public bool RecvFrom;// 3
                    public bool WSASend;// 4
                    public bool WSASendTo;// 5
                    public bool WSARecv;// 6
                    public bool WSARecvFrom;// 7
                    public bool TCP_Req;// 8
                    public bool UDP_Req;// 9
                    public bool TCP_Resp;// 10
                    public bool UDP_Resp;// 11

                    public FilterFunction(
                        bool bSend, 
                        bool bSendTo, 
                        bool bRecv, 
                        bool bRecvFrom, 
                        bool bWSASend, 
                        bool bWSASendTo, 
                        bool bWSARecv, 
                        bool bWSARecvFrom,
                        bool bTCP_Req,
                        bool bUDP_Req,
                        bool bTCP_Resp,
                        bool bUDP_Resp)
                    {
                        Send = bSend;
                        SendTo = bSendTo;
                        Recv = bRecv;
                        RecvFrom = bRecvFrom;
                        WSASend = bWSASend;
                        WSASendTo = bWSASendTo;
                        WSARecv = bWSARecv;
                        WSARecvFrom = bWSARecvFrom;
                        TCP_Req = bTCP_Req;
                        UDP_Req = bUDP_Req;
                        TCP_Resp = bTCP_Resp;
                        UDP_Resp = bUDP_Resp;
                    }
                }

                private struct SearchCondition
                {
                    public int RelativePosition { get; set; }
                    public byte Value { get; set; }
                    public byte Mask { get; set; }
                    public bool IsPartialWildcard { get; set; } // 部分通配符 F*, *A
                }

                private struct Modification
                {
                    public int Index { get; set; }
                    public byte Value { get; set; }
                }

                #endregion

                #region//新增滤镜            

                public static void AddFilter_New()
                {
                    try
                    {
                        Guid FID = Guid.NewGuid();
                        int FNum = FilterConfig.List.lstFilterInfo.Count + 1;
                        string FName = string.Format(AntdUI.Localization.Get("FilterList.NewFilter", "滤镜 {0}"), FNum.ToString());

                        FilterConfig.Filter.FilterMode FilterMode = FilterConfig.Filter.FilterMode.Normal;
                        FilterConfig.Filter.FilterAction FilterAction = FilterConfig.Filter.FilterAction.Replace;  
                        FilterConfig.Filter.FilterFunction FilterFunction = new FilterConfig.Filter.FilterFunction(true, true, true, true, true, true, true, true, true, true, true, true);
                        FilterConfig.Filter.FilterStartFrom FilterStartFrom = FilterConfig.Filter.FilterStartFrom.Head;

                        FilterConfig.Filter.AddFilter(
                            false, 
                            FID, 
                            FName, 
                            false, 
                            string.Empty, 
                            false, 
                            string.Empty, 
                            false, 
                            string.Empty, 
                            false, 
                            string.Empty, 
                            FilterMode, 
                            FilterAction, 
                            false,
                            FilterExecuteType.None,
                            Guid.Empty,
                            FilterFunction, 
                            FilterStartFrom, 
                            false, 
                            false, 
                            1, 
                            false, 
                            1, 
                            string.Empty, 
                            0, 
                            string.Empty,
                            string.Empty, 
                            string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddFilter_New), ex.Message);
                    }
                }

                public static bool AddFilter_ByPacketInfo(PacketInfo pi, byte[] bBuffer)
                {
                    try
                    {
                        if (pi != null)
                        {
                            if (bBuffer == null || bBuffer.Length == 0)
                            {
                                bBuffer = pi.PacketBuffer;
                            }

                            Guid FID = Guid.NewGuid();
                            string sFName = Process.GetCurrentProcess().ProcessName.Trim() + " [" + bBuffer.Length + "]";
                            PacketConfig.Packet.PacketType ptType = pi.PacketType;
                            FilterConfig.Filter.FilterMode FilterMode = FilterConfig.Filter.FilterMode.Normal;
                            FilterConfig.Filter.FilterAction FilterAction = FilterConfig.Filter.FilterAction.Replace;
                            FilterConfig.Filter.FilterFunction FilterFunction = FilterConfig.Filter.GetFilterFunction_ByPacketType(ptType);
                            FilterConfig.Filter.FilterStartFrom FilterStartFrom = FilterConfig.Filter.FilterStartFrom.Head;
                            string sFSearch = FilterConfig.Filter.GetFilterString_ByBytes(bBuffer);

                            FilterConfig.Filter.AddFilter(
                                false, 
                                FID, 
                                sFName, 
                                false, 
                                string.Empty, 
                                false, 
                                string.Empty, 
                                false, 
                                string.Empty, 
                                false, 
                                string.Empty, 
                                FilterMode, 
                                FilterAction, 
                                false,
                                FilterExecuteType.None,
                                Guid.Empty,
                                FilterFunction, 
                                FilterStartFrom, 
                                false, 
                                false, 
                                1, 
                                false, 
                                1, 
                                string.Empty, 
                                0, 
                                string.Empty,
                                sFSearch, 
                                string.Empty);

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddFilter_ByPacketInfo), ex.Message);
                    }

                    return false;
                }

                public static bool AddFilter_ByProxyInfo(ProxyInfo pi, byte[] bBuffer)
                {
                    try
                    {
                        if (pi != null)
                        {
                            if (bBuffer == null || bBuffer.Length == 0)
                            {
                                bBuffer = pi.PacketBuffer;
                            }

                            Guid FID = Guid.NewGuid();
                            string sFName = Process.GetCurrentProcess().ProcessName.Trim() + " [" + bBuffer.Length + "]";
                            PacketConfig.Packet.PacketType ptType = pi.PacketType;
                            FilterConfig.Filter.FilterMode FilterMode = FilterConfig.Filter.FilterMode.Normal;
                            FilterConfig.Filter.FilterAction FilterAction = FilterConfig.Filter.FilterAction.Replace;
                            FilterConfig.Filter.FilterFunction FilterFunction = FilterConfig.Filter.GetFilterFunction_ByPacketType(ptType);
                            FilterConfig.Filter.FilterStartFrom FilterStartFrom = FilterConfig.Filter.FilterStartFrom.Head;
                            string sFSearch = FilterConfig.Filter.GetFilterString_ByBytes(bBuffer);

                            FilterConfig.Filter.AddFilter(
                                false,
                                FID,
                                sFName,
                                false,
                                string.Empty,
                                false,
                                string.Empty,
                                false,
                                string.Empty,
                                false,
                                string.Empty,
                                FilterMode,
                                FilterAction,
                                false,
                                FilterExecuteType.None,
                                Guid.Empty,
                                FilterFunction,
                                FilterStartFrom,
                                false,
                                false,
                                1,
                                false,
                                1,
                                string.Empty,
                                0,
                                string.Empty,
                                sFSearch,
                                string.Empty);

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddFilter_ByProxyInfo), ex.Message);
                    }

                    return false;
                }

                public static void AddFilter(
                    bool IsEnable,
                    Guid FID,
                    string FName,
                    bool bAppointHeader,
                    string HeaderContent,
                    bool bAppointSocket,
                    string SocketContent,
                    bool bAppointLength,
                    string LengthContent,
                    bool bAppointPort,
                    string PortContent,
                    FilterConfig.Filter.FilterMode FilterMode,
                    FilterConfig.Filter.FilterAction FilterAction,
                    bool IsExecute,
                    Operate.FilterConfig.Filter.FilterExecuteType FEType,
                    Guid Execute_GUID,
                    FilterConfig.Filter.FilterFunction FilterFunction,
                    FilterConfig.Filter.FilterStartFrom FilterStartFrom,
                    bool IsProgressionDone,
                    bool IsProgressionContinuous,
                    int ProgressionStep,
                    bool IsProgressionCarry,
                    int ProgressionCarryNumber,
                    string ProgressionPosition,
                    int ProgressionCount,
                    string ExcludePosition,
                    string FSearch,
                    string FModify)
                {
                    try
                    {
                        if (FID != null && !string.IsNullOrEmpty(FName))
                        {
                            FilterInfo fi = new FilterInfo(
                            IsEnable,
                            FID,
                            FName,
                            bAppointHeader,
                            HeaderContent,
                            bAppointSocket,
                            SocketContent,
                            bAppointLength,
                            LengthContent,
                            bAppointPort,
                            PortContent,
                            FilterMode,
                            FilterAction,
                            IsExecute,
                            FEType,
                            Execute_GUID,
                            FilterFunction,
                            FilterStartFrom,
                            IsProgressionDone,
                            IsProgressionContinuous,
                            ProgressionStep,
                            IsProgressionCarry,
                            ProgressionCarryNumber,
                            ProgressionPosition,
                            ProgressionCount,
                            ExcludePosition,
                            FSearch,
                            FModify);

                            FilterConfig.List.FilterToList(fi);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddFilter), ex.Message);
                    }
                }

                #endregion

                #region//更新滤镜

                public static void UpdateFilter(
                    FilterInfo fi,
                    string FName,
                    bool AppointHeader,
                    string HeaderContent,
                    bool AppointSocket,
                    string SocketContent,
                    bool AppointLength,
                    string LengthContent,
                    bool AppointPort,
                    string PortContent,
                    FilterConfig.Filter.FilterMode FilterMode,
                    FilterConfig.Filter.FilterAction FilterAction,
                    bool IsExecute,
                    Operate.FilterConfig.Filter.FilterExecuteType FEType,
                    Guid Execute_GUID,
                    FilterConfig.Filter.FilterFunction FilterFunction,
                    FilterConfig.Filter.FilterStartFrom FilterStartFrom,
                    bool IsProgressionContinuous,
                    int ProgressionStep,
                    bool IsProgressionCarry,
                    int ProgressionCarryNumber,
                    string ProgressionPosition,
                    int ProgressionCount,
                    string ExcludePosition,
                    string FSearch,
                    string FModify)
                {
                    try
                    {
                        if (fi != null)
                        {
                            fi.FName = FName;
                            fi.AppointHeader = AppointHeader;
                            fi.HeaderContent = HeaderContent;
                            fi.AppointSocket = AppointSocket;
                            fi.SocketContent = SocketContent;
                            fi.AppointLength = AppointLength;
                            fi.LengthContent = LengthContent;
                            fi.AppointPort = AppointPort;
                            fi.PortContent = PortContent;
                            fi.FMode = FilterMode;
                            fi.FAction = FilterAction;
                            fi.IsExecute = IsExecute;
                            fi.FEType = FEType;
                            fi.Execute_GUID = Execute_GUID;
                            fi.FFunction = FilterFunction;
                            fi.FStartFrom = FilterStartFrom;
                            fi.IsProgressionContinuous = IsProgressionContinuous;
                            fi.ProgressionStep = ProgressionStep;
                            fi.IsProgressionCarry = IsProgressionCarry;
                            fi.ProgressionCarryNumber = ProgressionCarryNumber;
                            fi.ProgressionPosition = ProgressionPosition;
                            fi.ProgressionCount = ProgressionCount;
                            fi.ExcludePosition = ExcludePosition;
                            fi.FSearch = FSearch;
                            fi.FModify = FModify;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateFilter), ex.Message);
                    }
                }

                #endregion

                #region//删除滤镜（对话框）

                public static void DeleteFilter_Dialog(Form form, List<FilterInfo> fiList)
                {
                    try
                    {
                        if (fiList.Count > 0)
                        {
                            AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miFilterList", "滤镜列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                            {
                                Icon = TType.Warn,
                                Keyboard = false,
                                MaskClosable = false,
                                OnOk = config =>
                                {
                                    foreach (FilterInfo fi in fiList)
                                    {
                                        FilterConfig.List.lstFilterInfo.Remove(fi);
                                    }

                                    return true;
                                }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteFilter_Dialog), ex.Message);
                    }
                }

                #endregion

                #region//复制滤镜

                public static void CopyFilter(FilterInfo fi)
                {
                    try
                    {
                        bool IsEnable = false;
                        Guid FID = Guid.NewGuid();
                        string FName = string.Format(AntdUI.Localization.Get("CopyName", "{0} - 副本"), fi.FName);
                        bool bAppointHeader = fi.AppointHeader;
                        string HeaderContent = fi.HeaderContent;
                        bool bAppointSocket = fi.AppointSocket;
                        string SocketContent = fi.SocketContent;
                        bool bAppointLength = fi.AppointLength;
                        string LengthContent = fi.LengthContent;
                        bool bAppointPort = fi.AppointPort;
                        string PortContent = fi.PortContent;
                        FilterConfig.Filter.FilterMode FMode = fi.FMode;
                        FilterConfig.Filter.FilterAction FAction = fi.FAction;
                        bool IsExecute = fi.IsExecute;
                        FilterConfig.Filter.FilterExecuteType FEType = fi.FEType;
                        Guid Execute_GUID = fi.Execute_GUID;
                        FilterConfig.Filter.FilterFunction FFunction = fi.FFunction;
                        FilterConfig.Filter.FilterStartFrom FStartFrom = fi.FStartFrom;
                        bool IsProgressionDone = false;
                        bool IsProgressionContinuous = fi.IsProgressionContinuous;
                        int ProgressionStep = fi.ProgressionStep;
                        bool IsProgressionCarry = fi.IsProgressionCarry;
                        int ProgressionCarryNumber = fi.ProgressionCarryNumber;
                        string ProgressionPosition = fi.ProgressionPosition;
                        int ProgressionCount = 0;
                        string ExcludePosition = fi.ExcludePosition;
                        string FSearch = fi.FSearch;
                        string FModify = fi.FModify;

                        FilterConfig.Filter.AddFilter(
                            IsEnable,
                            FID,
                            FName,
                            bAppointHeader,
                            HeaderContent,
                            bAppointSocket,
                            SocketContent,
                            bAppointLength,
                            LengthContent,
                            bAppointPort,
                            PortContent,
                            FMode,
                            FAction,
                            IsExecute,
                            FEType,
                            Execute_GUID,
                            FFunction,
                            FStartFrom,
                            IsProgressionDone,
                            IsProgressionContinuous,
                            ProgressionStep,
                            IsProgressionCarry,
                            ProgressionCarryNumber,
                            ProgressionPosition,
                            ProgressionCount,
                            ExcludePosition,
                            FSearch,
                            FModify);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CopyFilter), ex.Message);
                    }
                }

                #endregion

                #region//编辑滤镜

                public static void OpenFilterEdit(Form form, FilterInfo fi)
                {
                    var FilterEdit = new FilterEdit(form, fi);
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("FilterEditForm", "滤镜编辑"), FilterEdit)
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//获取滤镜

                public static FilterInfo GetFilter_ByGuid(Guid FID)
                {
                    try
                    {
                        if (FID != null && FID != Guid.Empty)
                        {
                            foreach (FilterInfo fi in FilterConfig.List.lstFilterInfo)
                            {
                                if (fi.FID == FID)
                                {
                                    return fi;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetFilter_ByGuid), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//设置滤镜是否启用

                public static void SetIsEnable_ByGUID(Guid FID, bool IsEnable)
                {
                    try
                    {
                        FilterInfo fi = Operate.FilterConfig.Filter.GetFilter_ByGuid(FID);
                        if (fi != null)
                        {
                            fi.IsEnable = IsEnable;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SetIsEnable_ByGUID), ex.Message);
                    }
                }

                #endregion

                #region//获取滤镜选项

                public static FilterConfig.Filter.FilterMode GetFilterMode_ByString(string FilterMode)
                {
                    FilterConfig.Filter.FilterMode FMode = new FilterConfig.Filter.FilterMode();

                    try
                    {
                        FMode = (FilterConfig.Filter.FilterMode)Enum.Parse(typeof(FilterConfig.Filter.FilterMode), FilterMode);
                    }
                    catch (Exception ex)
                    {
                        FMode = FilterConfig.Filter.FilterMode.Normal;
                        Operate.DoLog(nameof(GetFilterMode_ByString), ex.Message);
                    }

                    return FMode;
                }

                public static FilterConfig.Filter.FilterAction GetFilterAction_ByString(string FilterAction)
                {
                    FilterConfig.Filter.FilterAction FAction = new FilterConfig.Filter.FilterAction();

                    try
                    {
                        FAction = (FilterConfig.Filter.FilterAction)Enum.Parse(typeof(FilterConfig.Filter.FilterAction), FilterAction);
                    }
                    catch (Exception ex)
                    {
                        FAction = FilterConfig.Filter.FilterAction.Replace;
                        Operate.DoLog(nameof(GetFilterAction_ByString), ex.Message);
                    }

                    return FAction;
                }

                public static FilterConfig.Filter.FilterExecuteType GetFilterExecuteType_ByString(string FilterExecuteType)
                {
                    FilterConfig.Filter.FilterExecuteType FEType = new FilterConfig.Filter.FilterExecuteType();

                    try
                    {
                        FEType = (FilterConfig.Filter.FilterExecuteType)Enum.Parse(typeof(FilterConfig.Filter.FilterExecuteType), FilterExecuteType);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetFilterExecuteType_ByString), ex.Message);
                    }

                    return FEType;
                }

                public static FilterConfig.Filter.FilterFunction GetFilterFunction_ByString(string FilterFunction)
                {
                    FilterConfig.Filter.FilterFunction FFunction = new FilterConfig.Filter.FilterFunction();

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

                        if (slFilterFunction.Length > 8)
                        {
                            FFunction.TCP_Req = Convert.ToBoolean(int.Parse(slFilterFunction[8]));
                            FFunction.UDP_Req = Convert.ToBoolean(int.Parse(slFilterFunction[9]));
                            FFunction.TCP_Resp = Convert.ToBoolean(int.Parse(slFilterFunction[10]));
                            FFunction.UDP_Resp = Convert.ToBoolean(int.Parse(slFilterFunction[11]));
                        }
                        else
                        {
                            FFunction.TCP_Req = true;
                            FFunction.UDP_Req = true;
                            FFunction.TCP_Resp = true;
                            FFunction.UDP_Resp = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetFilterFunction_ByString), ex.Message);
                    }

                    return FFunction;
                }

                public static FilterConfig.Filter.FilterStartFrom GetFilterStartFrom_ByString(string sFStartFrom)
                {
                    FilterConfig.Filter.FilterStartFrom FStartFrom = new FilterConfig.Filter.FilterStartFrom();

                    try
                    {
                        FStartFrom = (FilterConfig.Filter.FilterStartFrom)Enum.Parse(typeof(FilterConfig.Filter.FilterStartFrom), sFStartFrom);
                    }
                    catch (Exception ex)
                    {
                        FStartFrom = FilterConfig.Filter.FilterStartFrom.Head;
                        Operate.DoLog(nameof(GetFilterStartFrom_ByString), ex.Message);
                    }

                    return FStartFrom;
                }

                #endregion

                #region//获取滤镜字符串

                public static string GetFilterString_ByBytes(byte[] bBuffer)
                {
                    string sReturn = string.Empty;

                    try
                    {
                        for (int i = 0; i < bBuffer.Length; i++)
                        {
                            string sHex = bBuffer[i].ToString("X2");
                            sReturn += i.ToString() + "|" + sHex + ",";
                        }

                        sReturn = sReturn.Trim(',');
                    }
                    catch (Exception ex)
                    {
                        sReturn = "";
                        Operate.DoLog(nameof(GetFilterString_ByBytes), ex.Message);
                    }

                    return sReturn;
                }

                #endregion

                #region//获取滤镜动作对应的名称

                public static string GetName_ByFilterAction(FilterConfig.Filter.FilterAction filterAction)
                {
                    try
                    {
                        switch (filterAction)
                        {
                            case FilterConfig.Filter.FilterAction.Replace:
                                return AntdUI.Localization.Get("Replace", "替换");

                            case FilterConfig.Filter.FilterAction.Intercept:
                                return AntdUI.Localization.Get("Intercept", "拦截");

                            case FilterConfig.Filter.FilterAction.Change:
                                return AntdUI.Localization.Get("Change", "换包");

                            case FilterConfig.Filter.FilterAction.NoModify_Display:
                                return AntdUI.Localization.Get("NoModifyDisplay", "不修改-只显示");

                            case FilterConfig.Filter.FilterAction.NoModify_NoDisplay:
                                return AntdUI.Localization.Get("NoModifyNoDisplay", "不修改-不显示");

                            default:
                                return string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetName_ByFilterAction), ex.Message);
                        return string.Empty;
                    }
                }

                #endregion

                #region//获取滤镜作用类别字符串

                public static string GetFilterFunctionString(FilterConfig.Filter.FilterFunction FilterFunction)
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
                        sReturn += Convert.ToInt32(FilterFunction.WSARecvFrom) + ":";
                        sReturn += Convert.ToInt32(FilterFunction.TCP_Req) + ":";
                        sReturn += Convert.ToInt32(FilterFunction.UDP_Req) + ":";
                        sReturn += Convert.ToInt32(FilterFunction.TCP_Resp) + ":";
                        sReturn += Convert.ToInt32(FilterFunction.UDP_Resp);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetFilterFunctionString), ex.Message);
                    }

                    return sReturn;
                }

                #endregion

                #region//获取滤镜执行类型

                public static (Operate.FilterConfig.Filter.FilterExecuteType feType, Guid gGuid) GetFilterExecuteType(AntdUI.Checkbox cbFilterExecute, AntdUI.Select sFilterExecuteType, AntdUI.Select sFilterExecuteInfo)
                {
                    Operate.FilterConfig.Filter.FilterExecuteType feType = Operate.FilterConfig.Filter.FilterExecuteType.None;
                    Guid gGuid = Guid.Empty;

                    if (cbFilterExecute.Checked)
                    {
                        if (sFilterExecuteType.SelectedIndex == 0)
                        {
                            feType = Operate.FilterConfig.Filter.FilterExecuteType.Send;

                            if (sFilterExecuteInfo.SelectedValue != null)
                            {
                                gGuid = ((SendInfo)sFilterExecuteInfo.SelectedValue).SID;
                            }
                        }
                        else if (sFilterExecuteType.SelectedIndex == 1)
                        {
                            feType = Operate.FilterConfig.Filter.FilterExecuteType.Robot;

                            if (sFilterExecuteInfo.SelectedValue != null)
                            {
                                gGuid = ((RobotInfo)sFilterExecuteInfo.SelectedValue).RID;
                            }
                        }
                        else if (sFilterExecuteType.SelectedIndex == 2)
                        {
                            feType = Operate.FilterConfig.Filter.FilterExecuteType.Filter;

                            if (sFilterExecuteInfo.SelectedValue != null)
                            {
                                gGuid = ((FilterInfo)sFilterExecuteInfo.SelectedValue).FID;
                            }
                        }
                    }

                    return (feType, gGuid);
                }

                #endregion

                #region//获取封包类别对应的滤镜作用类别

                public static FilterConfig.Filter.FilterFunction GetFilterFunction_ByPacketType(PacketConfig.Packet.PacketType ptType)
                {
                    FilterConfig.Filter.FilterFunction ffReturn = new FilterConfig.Filter.FilterFunction();

                    try
                    {
                        switch (ptType)
                        {
                            case PacketConfig.Packet.PacketType.WS1_Send:
                                ffReturn.Send = true;
                                break;

                            case PacketConfig.Packet.PacketType.WS2_Send:
                                ffReturn.Send = true;
                                break;

                            case PacketConfig.Packet.PacketType.WS1_SendTo:
                                ffReturn.SendTo = true;
                                break;

                            case PacketConfig.Packet.PacketType.WS2_SendTo:
                                ffReturn.SendTo = true;
                                break;

                            case PacketConfig.Packet.PacketType.WS1_Recv:
                                ffReturn.Recv = true;
                                break;

                            case PacketConfig.Packet.PacketType.WS2_Recv:
                                ffReturn.Recv = true;
                                break;

                            case PacketConfig.Packet.PacketType.WS1_RecvFrom:
                                ffReturn.RecvFrom = true;
                                break;

                            case PacketConfig.Packet.PacketType.WS2_RecvFrom:
                                ffReturn.RecvFrom = true;
                                break;

                            case PacketConfig.Packet.PacketType.WSASend:
                                ffReturn.WSASend = true;
                                break;

                            case PacketConfig.Packet.PacketType.WSASendTo:
                                ffReturn.WSASendTo = true;
                                break;

                            case PacketConfig.Packet.PacketType.WSARecv:
                                ffReturn.WSARecv = true;
                                break;

                            case PacketConfig.Packet.PacketType.WSARecvEx:
                                ffReturn.WSARecv = true;
                                break;

                            case PacketConfig.Packet.PacketType.WSARecvFrom:
                                ffReturn.WSARecvFrom = true;
                                break;

                            case PacketConfig.Packet.PacketType.TCP_Req:
                                ffReturn.TCP_Req = true;
                                break;

                            case PacketConfig.Packet.PacketType.UDP_Req:
                                ffReturn.UDP_Req = true;
                                break;

                            case PacketConfig.Packet.PacketType.TCP_Resp:
                                ffReturn.TCP_Resp = true;
                                break;

                            case PacketConfig.Packet.PacketType.UDP_Resp:
                                ffReturn.UDP_Resp = true;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetFilterFunction_ByPacketType), ex.Message);
                    }

                    return ffReturn;
                }

                #endregion

                #region//检查滤镜是否生效

                public static bool CheckFilter_IsEffective(
                    Int32 iSocket,
                    Span<byte> bufferSpan,
                    PacketConfig.Packet.PacketType ptType,
                    PacketConfig.Packet.SockAddr sAddr,
                    FilterInfo sfi)
                {
                    if (!sfi.IsEnable)
                        return false;

                    if (!FilterConfig.Filter.CheckFilterFunction_ByPacketType(ptType, sfi.FFunction))
                        return false;

                    if (sfi.AppointSocket && !FilterConfig.Filter.CheckPacket_IsMatch_AppointSocket(iSocket, sfi.SocketContent))
                        return false;

                    if (sfi.AppointPort && !FilterConfig.Filter.CheckPacket_IsMatch_AppointPort(iSocket, ptType, sAddr, sfi.PortContent))
                        return false;

                    if (sfi.AppointLength && !FilterConfig.Filter.CheckPacket_IsMatch_AppointLength(bufferSpan.Length, sfi.LengthContent))
                        return false;

                    if (sfi.AppointHeader && !FilterConfig.Filter.CheckPacket_IsMatch_AppointHeader(bufferSpan, sfi.HeaderContent))
                        return false;

                    return true;
                }

                #endregion

                #region//检查封包类别

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static unsafe bool CheckFilterFunction_ByPacketType(PacketConfig.Packet.PacketType ptType, in FilterFunction ffFunction)
                {
                    fixed (bool* pFlags = &ffFunction.Send)
                    {
                        byte* indexMap = stackalloc byte[17]
                        {
                            0,   // WS1_Send -> Send (offset 0)
                            0,   // WS2_Send -> Send
                            1,   // WS1_SendTo -> SendTo
                            1,   // WS2_SendTo -> SendTo
                            2,   // WS1_Recv -> Recv
                            2,   // WS2_Recv -> Recv
                            3,   // WS1_RecvFrom -> RecvFrom
                            3,   // WS2_RecvFrom -> RecvFrom
                            4,   // WSASend -> WSASend
                            5,   // WSASendTo -> WSASendTo
                            6,   // WSARecv -> WSARecv
                            6,   // WSARecvEx -> WSARecv
                            7,   // WSARecvFrom -> WSARecvFrom
                            8,   // TCP_Req -> TCP_Req
                            9,   // UDP_Req -> UDP_Req
                            10,  // TCP_Resp -> TCP_Resp
                            11   // UDP_Resp -> UDP_Resp
                        };

                        int index = (int)ptType;
                        if (index >= 0 && index < 17)
                        {
                            return pFlags[indexMap[index]];
                        }
                    }
                    return false;
                }

                #endregion

                #region//检查是否匹配指定套接字

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool CheckPacket_IsMatch_AppointSocket(Int32 iSocket, string socketContent)
                {
                    if (string.IsNullOrEmpty(socketContent))
                        return false;

                    try
                    {
                        string[] parts = socketContent.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string part in parts)
                        {
                            if (int.TryParse(part.Trim(), out int currentValue))
                            {
                                if (currentValue == iSocket)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckPacket_IsMatch_AppointSocket), ex.Message);                        
                    }

                    return false;
                }

                #endregion

                #region//检查是否匹配指定长度

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool CheckPacket_IsMatch_AppointLength(int len, string lengthContent)
                {
                    if (string.IsNullOrEmpty(lengthContent))
                        return false;

                    try
                    {
                        string[] parts = lengthContent.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string part in parts)
                        {
                            string trimmedPart = part.Trim();
                            int dashIndex = trimmedPart.IndexOf('-');

                            if (dashIndex >= 0)
                            {
                                string fromStr = trimmedPart.Substring(0, dashIndex).Trim();
                                string toStr = trimmedPart.Substring(dashIndex + 1).Trim();

                                if (int.TryParse(fromStr, out int lenFrom) &&
                                    int.TryParse(toStr, out int lenTo) &&
                                    len >= lenFrom &&
                                    len <= lenTo)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (int.TryParse(trimmedPart, out int exactLen) && len == exactLen)
                                {
                                    return true;
                                }
                            }
                        }

                        return false;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckPacket_IsMatch_AppointLength), ex.Message);
                        return false;
                    }
                }

                #endregion

                #region//检查是否匹配指定端口

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool CheckPacket_IsMatch_AppointPort(
                    int iSocket,
                    PacketConfig.Packet.PacketType ptType,
                    PacketConfig.Packet.SockAddr sAddr,
                    string portContent)
                {
                    if (string.IsNullOrEmpty(portContent))
                        return false;

                    try
                    {
                        string packetIP = PacketConfig.Packet.GetIPString_BySocketAddr(iSocket, sAddr, ptType);
                        if (string.IsNullOrEmpty(packetIP))
                            return false;

                        // 获取实际端口号
                        int actualPort = GetPortFromIPString(packetIP);
                        if (actualPort == -1)
                            return false;

                        // 检查端口是否匹配
                        return CheckPortMatch(portContent, actualPort);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckPacket_IsMatch_AppointPort), ex.Message);
                        return false;
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static int GetPortFromIPString(string ipString)
                {
                    int pipeIndex = ipString.IndexOf('|');

                    if (pipeIndex > 0)
                    {
                        // 尝试第一部分
                        if (TryParsePort(ipString.Substring(0, pipeIndex), out int port))
                            return port;

                        // 尝试第二部分
                        if (TryParsePort(ipString.Substring(pipeIndex + 1), out port))
                            return port;
                    }
                    else
                    {
                        // 没有管道符，尝试整个字符串
                        if (TryParsePort(ipString, out int port))
                            return port;
                    }

                    return -1;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static bool TryParsePort(string ipPortPart, out int port)
                {
                    int colonIndex = ipPortPart.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        string portStr = ipPortPart.Substring(colonIndex + 1);
                        return int.TryParse(portStr, out port);
                    }

                    port = -1;
                    return false;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static bool CheckPortMatch(string portContent, int actualPort)
                {
                    string[] parts = portContent.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string part in parts)
                    {
                        string trimmedPart = part.Trim();

                        // 检查范围格式 (如 "1000-2000")
                        int dashIndex = trimmedPart.IndexOf('-');
                        if (dashIndex > 0)
                        {
                            string fromStr = trimmedPart.Substring(0, dashIndex).Trim();
                            string toStr = trimmedPart.Substring(dashIndex + 1).Trim();

                            if (int.TryParse(fromStr, out int minPort) &&
                                int.TryParse(toStr, out int maxPort) &&
                                actualPort >= minPort && actualPort <= maxPort)
                            {
                                return true;
                            }
                            continue;
                        }

                        // 检查精确匹配
                        if (int.TryParse(trimmedPart, out int port) && actualPort == port)
                        {
                            return true;
                        }
                    }

                    return false;
                }

                #endregion

                #region//检查是否匹配指定包头

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool CheckPacket_IsMatch_AppointHeader(ReadOnlySpan<byte> bufferSpan, string headerContent)
                {
                    if (string.IsNullOrEmpty(headerContent))
                        return false;

                    try
                    {
                        byte[] headerBytes = SystemConfig.StringToBytes(
                            PacketConfig.Packet.EncodingFormat.Hex,
                            headerContent);

                        if (headerBytes.Length > 0 && headerBytes.Length <= bufferSpan.Length)
                        {
                            return bufferSpan.Slice(0, headerBytes.Length).SequenceEqual(headerBytes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckPacket_IsMatch_AppointHeader), ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//检查滤镜辅助方法

                [MethodImpl(MethodImplOptions.AggressiveInlining)]

                private static HashSet<int> ParseExcludePositions(string excludeString)
                {
                    var positions = new HashSet<int>();

                    if (string.IsNullOrEmpty(excludeString))
                        return positions;

                    try
                    {
                        var excludeParts = excludeString.AsSpan();

                        while (!excludeParts.IsEmpty)
                        {
                            int commaIndex = excludeParts.IndexOf(',');
                            ReadOnlySpan<char> partSpan = commaIndex >= 0
                                ? excludeParts.Slice(0, commaIndex)
                                : excludeParts;

                            excludeParts = commaIndex >= 0
                                ? excludeParts.Slice(commaIndex + 1)
                                : ReadOnlySpan<char>.Empty;

                            if (partSpan.IsEmpty || partSpan.IsWhiteSpace())
                                continue;

                            var positionSpan = partSpan.Trim();
                            if (TryParseNonNegativeInt(positionSpan, out int position))
                            {
                                positions.Add(position);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ParseExcludePositions), ex.Message);
                    }

                    return positions;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]

                private static bool HexCharsWithWildcardToByte(ReadOnlySpan<char> s, out byte result, out byte mask)
                {
                    result = 0;
                    mask = 0;

                    if (s.Length != 2)
                        return false;

                    if (s[0] == '*')
                    {
                        mask &= 0x0F;
                    }
                    else
                    {
                        int high = CharToNibble(s[0]);
                        if (high == -1) return false;
                        result |= (byte)(high << 4);
                        mask |= 0xF0;
                    }

                    if (s[1] == '*')
                    {
                        mask &= 0xF0;
                    }
                    else
                    {
                        int low = CharToNibble(s[1]);
                        if (low == -1) return false;
                        result |= (byte)low;
                        mask |= 0x0F;
                    }

                    return true;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]

                private static bool TryParseNonNegativeInt(ReadOnlySpan<char> s, out int result)
                {
                    result = 0;
                    if (s.IsEmpty) return false;

                    foreach (char c in s)
                    {
                        if (c < '0' || c > '9')
                            return false;
                        result = result * 10 + (c - '0');
                    }
                    return true;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]

                private static int CharToNibble(char c)
                {
                    uint digit = (uint)(c - '0');
                    if (digit < 10) return (int)digit;

                    uint lower = (uint)(c - 'a');
                    if (lower < 6) return (int)(10 + lower);

                    uint upper = (uint)(c - 'A');
                    if (upper < 6) return (int)(10 + upper);

                    return -1;
                }

                #endregion

                #region //检查滤镜是否匹配成功（普通滤镜）

                [MethodImpl(MethodImplOptions.AggressiveInlining)]

                public static bool CheckFilter_IsMatch_Normal(FilterInfo sfi, ReadOnlySpan<byte> bufferSpan)
                {
                    if (string.IsNullOrEmpty(sfi.FSearch) || bufferSpan.IsEmpty)
                        return false;

                    try
                    {
                        HashSet<int> excludePositions = null;
                        if (!string.IsNullOrEmpty(sfi.ExcludePosition))
                        {
                            excludePositions = ParseExcludePositions(sfi.ExcludePosition);
                        }

                        var searchParts = sfi.FSearch.AsSpan();

                        while (!searchParts.IsEmpty)
                        {
                            int commaIndex = searchParts.IndexOf(',');
                            ReadOnlySpan<char> partSpan = commaIndex >= 0
                                ? searchParts.Slice(0, commaIndex)
                                : searchParts;

                            searchParts = commaIndex >= 0
                                ? searchParts.Slice(commaIndex + 1)
                                : ReadOnlySpan<char>.Empty;

                            if (partSpan.IsEmpty || partSpan.IsWhiteSpace())
                                continue;

                            int pipeIndex = partSpan.IndexOf('|');
                            if (pipeIndex <= 0 || pipeIndex >= partSpan.Length - 1)
                                return false;

                            var indexSpan = partSpan.Slice(0, pipeIndex).Trim();
                            if (!TryParseNonNegativeInt(indexSpan, out int index) ||
                                index >= bufferSpan.Length)
                            {
                                return false;
                            }

                            var hexSpan = partSpan.Slice(pipeIndex + 1).Trim();
                            if (!HexCharsWithWildcardToByte(hexSpan, out byte expected, out byte mask))
                                return false;

                            bool isExcludePosition = excludePositions != null && excludePositions.Contains(index);

                            if (isExcludePosition)
                            {
                                byte actualValue = bufferSpan[index];
                                if ((actualValue & mask) == (expected & mask))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if ((bufferSpan[index] & mask) != (expected & mask))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckFilter_IsMatch_Normal), ex.Message);
                        return false;
                    }

                    return true;
                }

                #endregion

                #region//检查滤镜是否匹配成功（高级滤镜）

                [MethodImpl(MethodImplOptions.AggressiveInlining)]

                public static List<int> CheckFilter_IsMatch_Advanced(FilterInfo sfi, ReadOnlySpan<byte> bufferSpan)
                {
                    var result = new List<int>();
                    if (string.IsNullOrEmpty(sfi.FSearch))
                        return result;

                    try
                    {
                        HashSet<int> excludePositions = null;
                        if (!string.IsNullOrEmpty(sfi.ExcludePosition))
                        {
                            excludePositions = ParseExcludePositions(sfi.ExcludePosition);
                        }

                        var searchConditions = ParseSearchConditions(sfi.FSearch);
                        if (searchConditions.Count == 0)
                            return result;

                        var firstCondition = searchConditions[0];
                        byte firstValue = firstCondition.Value;
                        int relativePosition = firstCondition.RelativePosition;

                        for (int i = 0; i < bufferSpan.Length; i++)
                        {
                            if (bufferSpan[i] == firstValue)
                            {
                                bool isMatch = true;
                                int lastCheckedIndex = i;

                                for (int j = 1; j < searchConditions.Count; j++)
                                {
                                    var condition = searchConditions[j];
                                    int checkIndex = i + condition.RelativePosition - relativePosition;

                                    if (checkIndex < 0 || checkIndex >= bufferSpan.Length)
                                    {
                                        isMatch = false;
                                        break;
                                    }

                                    bool isExcludePosition = excludePositions != null && excludePositions.Contains(condition.RelativePosition);

                                    if (isExcludePosition)
                                    {
                                        byte actualByte = bufferSpan[checkIndex];
                                        if ((actualByte & condition.Mask) == (condition.Value & condition.Mask))
                                        {
                                            isMatch = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (condition.IsPartialWildcard)
                                        {
                                            byte actualByte = bufferSpan[checkIndex];
                                            if ((actualByte & condition.Mask) != (condition.Value & condition.Mask))
                                            {
                                                isMatch = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (bufferSpan[checkIndex] != condition.Value)
                                            {
                                                isMatch = false;
                                                break;
                                            }
                                        }
                                    }

                                    lastCheckedIndex = Math.Max(lastCheckedIndex, checkIndex);
                                }

                                if (isMatch)
                                {
                                    result.Add(i);

                                    if (sfi.FStartFrom == FilterConfig.Filter.FilterStartFrom.Head)
                                    {
                                        break;
                                    }

                                    i = lastCheckedIndex;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckFilter_IsMatch_Advanced), ex.Message);
                    }

                    return result;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]

                private static List<SearchCondition> ParseSearchConditions(string searchPattern)
                {
                    var conditions = new List<SearchCondition>();
                    string[] parts = searchPattern.Split(',');

                    foreach (string part in parts)
                    {
                        if (string.IsNullOrEmpty(part))
                            continue;

                        string[] pair = part.Split('|');
                        if (pair.Length != 2)
                            continue;

                        if (int.TryParse(pair[0], out int position))
                        {
                            string hexValue = pair[1].Trim();

                            if (hexValue.Contains('*'))
                            {
                                if (hexValue.Length == 2 && (hexValue[0] == '*' || hexValue[1] == '*'))
                                {
                                    if (HexCharsWithWildcardToByte(hexValue.AsSpan(), out byte value, out byte mask))
                                    {
                                        conditions.Add(new SearchCondition
                                        {
                                            RelativePosition = position,
                                            Value = value,
                                            Mask = mask,
                                            IsPartialWildcard = true
                                        });
                                    }
                                }
                            }
                            else
                            {
                                if (byte.TryParse(hexValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                                {
                                    conditions.Add(new SearchCondition
                                    {
                                        RelativePosition = position,
                                        Value = value,
                                        Mask = 0xFF,
                                        IsPartialWildcard = false
                                    });
                                }
                            }
                        }
                    }

                    conditions.Sort((a, b) => a.RelativePosition.CompareTo(b.RelativePosition));

                    return conditions;
                }

                #endregion

                #region//执行滤镜

                public static FilterConfig.Filter.FilterAction DoFilter(
                    FilterInfo fi,
                    Int32 iSocket,
                    Span<byte> bufferSpan,
                    out byte[] bNewBuffer,
                    PacketConfig.Packet.PacketType ptType,
                    PacketConfig.Packet.SockAddr sAddr)
                {
                    FilterConfig.Filter.FilterAction faReturn = FilterConfig.Filter.FilterAction.None;
                    bNewBuffer = null;

                    try
                    {
                        if (!FilterConfig.Filter.CheckFilter_IsEffective(iSocket, bufferSpan, ptType, sAddr, fi))
                        {
                            return faReturn;
                        }

                        bool bDoFilter = false;
                        bool isMatch = false;
                        List<int> MatchIndex = null;

                        if (fi.FMode == FilterConfig.Filter.FilterMode.Normal)
                        {
                            isMatch = FilterConfig.Filter.CheckFilter_IsMatch_Normal(fi, bufferSpan);
                        }
                        else if (fi.FMode == FilterConfig.Filter.FilterMode.Advanced)
                        {
                            MatchIndex = FilterConfig.Filter.CheckFilter_IsMatch_Advanced(fi, bufferSpan);
                            isMatch = MatchIndex != null && MatchIndex.Count > 0;
                        }

                        if (!isMatch)
                        {
                            return faReturn;
                        }

                        byte[] tempBuffer = null;

                        switch (fi.FAction)
                        {
                            case FilterConfig.Filter.FilterAction.Replace:

                                fi.IsProgressionDone = false;

                                if (fi.FMode == FilterConfig.Filter.FilterMode.Normal)
                                {
                                    bDoFilter = FilterConfig.Filter.Replace_Normal(fi, bufferSpan);
                                    if (bDoFilter)
                                    {
                                        tempBuffer = bufferSpan.ToArray();
                                    }
                                }
                                else if (fi.FMode == FilterConfig.Filter.FilterMode.Advanced && MatchIndex != null)
                                {
                                    foreach (int iIndex in MatchIndex)
                                    {
                                        bDoFilter = FilterConfig.Filter.Replace_Advanced(fi, iIndex, bufferSpan);
                                    }

                                    tempBuffer = bufferSpan.ToArray();
                                }

                                if (fi.IsProgressionDone && fi.IsProgressionContinuous)
                                {
                                    fi.ProgressionCount++;
                                }

                                break;

                            case FilterConfig.Filter.FilterAction.Change:

                                fi.IsProgressionDone = false;

                                tempBuffer = FilterConfig.Filter.ChangePacket_Filter(fi);
                                bDoFilter = tempBuffer != null && tempBuffer.Length > 0;

                                if (fi.IsProgressionDone && fi.IsProgressionContinuous)
                                {
                                    fi.ProgressionCount++;
                                }

                                break;

                            case FilterConfig.Filter.FilterAction.Intercept:
                            case FilterConfig.Filter.FilterAction.NoModify_Display:
                            case FilterConfig.Filter.FilterAction.NoModify_NoDisplay:
                                bDoFilter = true;
                                break;
                        }

                        if (fi.IsExecute)
                        {
                            switch (fi.FEType)
                            {
                                case FilterConfig.Filter.FilterExecuteType.Send:

                                    SendConfig.Send.DoSend(fi.Execute_GUID);

                                    break;
                                case FilterConfig.Filter.FilterExecuteType.Robot:

                                    Dictionary<string, object> parameters = new Dictionary<string, object>
                                        {
                                            { "FilterSocket", iSocket }
                                        };

                                    RobotConfig.Robot.DoRobot(fi.Execute_GUID, parameters);
                                    break;

                                case Filter.FilterExecuteType.Filter:

                                    FilterInfo fiExecute = FilterConfig.Filter.GetFilter_ByGuid(fi.Execute_GUID);
                                    FilterConfig.Filter.DoFilter(fiExecute, iSocket, bufferSpan, out bNewBuffer, ptType, sAddr);

                                    break;
                            }
                        }

                        if (bDoFilter)
                        {
                            faReturn = fi.FAction;
                            fi.ExecutionCount++;

                            switch (fi.FAction)
                            {
                                case Filter.FilterAction.Replace:
                                    Interlocked.Increment(ref FilterConfig.Filter.FilterReplace_CNT);
                                    break;

                                case Filter.FilterAction.Change:
                                    Interlocked.Increment(ref FilterConfig.Filter.FilterChange_CNT);
                                    break;

                                case Filter.FilterAction.Intercept:
                                    Interlocked.Increment(ref FilterConfig.Filter.FilterIntercept_CNT);
                                    break;

                                case Filter.FilterAction.NoModify_Display:
                                    Interlocked.Increment(ref FilterConfig.Filter.FilterDisplay_CNT);
                                    break;

                                case Filter.FilterAction.NoModify_NoDisplay:
                                    Interlocked.Increment(ref FilterConfig.Filter.FilterNoDisplay_CNT);
                                    break;
                            }
                            Interlocked.Increment(ref FilterConfig.Filter.FilterExecute_CNT);

                            if (tempBuffer != null)
                            {
                                bNewBuffer = tempBuffer;
                            }

                            if (!SystemConfig.SpeedMode)
                            {
                                if (MatchIndex != null && MatchIndex.Count > 0)
                                {
                                    DoFilterLog(fi.FName, fi.FAction, MatchIndex.Count, ptType, bufferSpan.Length);
                                }
                                else
                                {
                                    DoFilterLog(fi.FName, fi.FAction, 1, ptType, bufferSpan.Length);
                                }
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DoFilter), ex.Message);
                    }

                    return faReturn;
                }

                #endregion

                #region//执行滤镜 - 代理模式

                public static void DoFilter_TCP(ProxySession psSession, Span<byte> bData, Operate.PacketConfig.Packet.PacketType ptType)
                {
                    try
                    {
                        Socket TargetSocket = null;

                        switch (ptType)
                        {
                            case Operate.PacketConfig.Packet.PacketType.TCP_Req:
                                TargetSocket = psSession.TargetSocket;
                                break;

                            case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                                TargetSocket = psSession.SocketSession.Client;
                                break;
                        }

                        if (TargetSocket == null || !TargetSocket.Connected)
                        {
                            return;
                        }

                        IPEndPoint epRemote = TargetSocket.RemoteEndPoint as IPEndPoint;
                        int SocketID = TargetSocket.Handle.ToInt32();

                        byte[] bRawBuffer = bData.ToArray();
                        byte[] bNewBuffer = null;

                        Operate.FilterConfig.Filter.FilterAction FilterAction =
                            Operate.FilterConfig.List.DoFilterList(
                                SocketID,
                                bData,
                                out bNewBuffer,
                                ptType,
                                new Operate.PacketConfig.Packet.SockAddr());

                        if (FilterAction != Operate.FilterConfig.Filter.FilterAction.Intercept)
                        {
                            switch (ptType)
                            {
                                case Operate.PacketConfig.Packet.PacketType.TCP_Req:
                                    psSession.TargetSocket.Send(bNewBuffer);
                                    break;

                                case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                                    psSession.TrySend(bNewBuffer, 0, bNewBuffer.Length);
                                    break;
                            }
                        }

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            FilterAction,
                            bNewBuffer.Length,
                            SocketID,
                            ptType,
                            $"{psSession.ClientIP}:{psSession.ClientPort}",
                            $"{psSession.ServerIP}:{psSession.ServerPort}",
                            psSession.ServerAddress,
                            psSession.DomainType,
                            bRawBuffer,
                            bNewBuffer);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DoFilter_TCP), ex.Message);
                    }
                }

                public static void DoFilter_UDP(ProxySession psSession, ProxyUDP pu, IPEndPoint epRemote, Span<byte> bData, Operate.PacketConfig.Packet.PacketType ptType)
                {
                    try
                    {
                        IPEndPoint epSend = null;
                        switch (ptType)
                        {
                            case Operate.PacketConfig.Packet.PacketType.UDP_Req:
                                epSend = epRemote;
                                break;

                            case Operate.PacketConfig.Packet.PacketType.UDP_Resp:
                                epSend = pu.ClientEndPoint;
                                break;
                        }

                        if (epSend == null || pu?.ClientSocket == null)
                        {
                            return;
                        }

                        int iSocket = pu.ClientSocket.Handle.ToInt32();

                        Int32 res = 0;
                        byte[] bRawBuffer = bData.ToArray();
                        byte[] bNewBuffer = null;

                        Operate.FilterConfig.Filter.FilterAction FilterAction =
                            Operate.FilterConfig.List.DoFilterList(
                                iSocket,
                                bData,
                                out bNewBuffer,
                                ptType,
                                new Operate.PacketConfig.Packet.SockAddr());

                        if (FilterAction != Operate.FilterConfig.Filter.FilterAction.Intercept)
                        {
                            res = psSession.SendUdpData(pu.ClientSocket, bNewBuffer, epSend);
                        }

                        string ClientAddr = $"{pu.ClientEndPoint.Address.ToString()}:{pu.ClientEndPoint.Port.ToString()}";
                        string ServerAddr = $"{epRemote.Address.ToString()}:{epRemote.Port.ToString()}";

                        _ = Operate.ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            FilterAction,
                            res,
                            iSocket,
                            ptType,
                            ClientAddr,
                            ServerAddr,
                            ServerAddr,
                            Operate.ProxyConfig.Proxy.DomainType.External,
                            bRawBuffer,
                            bNewBuffer);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DoFilter_UDP), ex.Message);
                    }
                }

                #endregion

                #region//执行替换（普通滤镜）

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool Replace_Normal(FilterInfo sfi, Span<byte> bufferSpan)
                {
                    if (string.IsNullOrEmpty(sfi.FSearch))
                        return false;

                    bool hasModifications = !string.IsNullOrEmpty(sfi.FModify);
                    bool hasProgressions = !string.IsNullOrEmpty(sfi.ProgressionPosition);

                    if (!hasModifications && !hasProgressions)
                        return false;

                    try
                    {
                        bool result = false;

                        if (hasModifications)
                        {
                            result |= FilterConfig.Filter.ProcessModifications(sfi, bufferSpan);
                        }

                        if (hasProgressions)
                        {
                            result |= FilterConfig.Filter.ProcessProgressions(sfi, bufferSpan);
                        }

                        return result;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(Replace_Normal), ex.Message);
                        return false;
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static bool ProcessModifications(FilterInfo sfi, Span<byte> bufferSpan)
                {
                    bool modified = false;
                    string[] modifications = sfi.FModify.Split(',');

                    foreach (string modification in modifications)
                    {
                        if (string.IsNullOrEmpty(modification))
                            continue;

                        string[] parts = modification.Split('|');
                        if (parts.Length != 2)
                            continue;

                        if (int.TryParse(parts[0], out int index) &&
                            index >= 0 &&
                            index < bufferSpan.Length &&
                            byte.TryParse(parts[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                        {
                            bufferSpan[index] = value;
                            modified = true;
                        }
                    }

                    return modified;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static bool ProcessProgressions(FilterInfo sfi, Span<byte> bufferSpan)
                {
                    bool modified = false;
                    int carryCount = 0;
                    int step = (int)sfi.ProgressionStep;
                    string[] positions = sfi.ProgressionPosition.Split(',');

                    foreach (string position in positions)
                    {
                        if (string.IsNullOrEmpty(position) ||
                            !int.TryParse(position, out int index) ||
                            index < 0 ||
                            index >= bufferSpan.Length)
                        {
                            continue;
                        }

                        byte currentValue = bufferSpan[index];
                        byte newValue = SystemConfig.GetStepByte(currentValue, step * (sfi.ProgressionCount + 1), out carryCount);
                        bufferSpan[index] = newValue;
                        modified = true;
                        sfi.IsProgressionDone = true;

                        if (sfi.IsProgressionCarry && carryCount > 0)
                        {
                            for (int i = 0; i < sfi.ProgressionCarryNumber; i++)
                            {
                                int prevIndex = index - (i + 1);
                                if (prevIndex < 0)
                                    break;

                                byte prevValue = bufferSpan[prevIndex];
                                prevValue = SystemConfig.GetStepByte(prevValue, carryCount, out carryCount);
                                bufferSpan[prevIndex] = prevValue;
                                modified = true;

                                if (carryCount == 0)
                                    break;
                            }
                        }
                    }

                    return modified;
                }

                #endregion

                #region//执行替换（高级滤镜）

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool Replace_Advanced(FilterInfo sfi, int matchIndex, Span<byte> bufferSpan)
                {
                    if (string.IsNullOrEmpty(sfi.FSearch))
                        return false;

                    bool hasModifications = !string.IsNullOrEmpty(sfi.FModify);
                    bool hasProgressions = !string.IsNullOrEmpty(sfi.ProgressionPosition);

                    if (!hasModifications && !hasProgressions)
                        return false;

                    try
                    {
                        bool result = false;
                        var startFrom = sfi.FStartFrom;

                        if (hasModifications)
                        {
                            result |= ProcessAdvancedModifications(sfi, matchIndex, bufferSpan, startFrom);
                        }

                        if (hasProgressions)
                        {
                            result |= ProcessAdvancedProgressions(sfi, matchIndex, bufferSpan, startFrom);
                        }

                        return result;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(Replace_Advanced), ex.Message);
                        return false;
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static bool ProcessAdvancedModifications(
                    FilterInfo sfi,
                    int matchIndex,
                    Span<byte> bufferSpan,
                    FilterConfig.Filter.FilterStartFrom startFrom)
                {
                    bool modified = false;
                    string[] modifications = sfi.FModify.Split(',');

                    foreach (string modification in modifications)
                    {
                        if (string.IsNullOrEmpty(modification))
                            continue;

                        string[] parts = modification.Split('|');
                        if (parts.Length != 2)
                            continue;

                        if (!int.TryParse(parts[0], out int index))
                            continue;

                        if (startFrom == FilterConfig.Filter.FilterStartFrom.Position)
                        {
                            index += matchIndex;
                        }

                        if (index < 0 || index >= bufferSpan.Length)
                            continue;

                        if (byte.TryParse(parts[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                        {
                            if (bufferSpan[index] != value)
                            {
                                bufferSpan[index] = value;
                                modified = true;
                            }
                        }
                    }

                    return modified;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static bool ProcessAdvancedProgressions(
                    FilterInfo sfi,
                    int matchIndex,
                    Span<byte> bufferSpan,
                    FilterConfig.Filter.FilterStartFrom startFrom)
                {
                    bool modified = false;
                    int carryCount = 0;
                    int step = (int)sfi.ProgressionStep;
                    string[] positions = sfi.ProgressionPosition.Split(',');

                    foreach (string position in positions)
                    {
                        if (string.IsNullOrEmpty(position) || !int.TryParse(position, out int index))
                            continue;

                        if (startFrom == FilterConfig.Filter.FilterStartFrom.Position)
                        {
                            index += matchIndex;
                        }

                        if (index < 0 || index >= bufferSpan.Length)
                            continue;

                        byte currentValue = bufferSpan[index];
                        byte newValue = SystemConfig.GetStepByte(currentValue, step * (sfi.ProgressionCount + 1), out carryCount);
                        bufferSpan[index] = newValue;
                        modified = true;
                        sfi.IsProgressionDone = true;

                        if (sfi.IsProgressionCarry && carryCount > 0)
                        {
                            HandleCarryOver(sfi, bufferSpan, index, ref carryCount);
                        }
                    }

                    return modified;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static void HandleCarryOver(FilterInfo sfi, Span<byte> bufferSpan, int index, ref int carryCount)
                {
                    for (int i = 0; i < sfi.ProgressionCarryNumber && carryCount > 0; i++)
                    {
                        int prevIndex = index - (i + 1);
                        if (prevIndex < 0)
                            break;

                        byte prevValue = bufferSpan[prevIndex];
                        prevValue = SystemConfig.GetStepByte(prevValue, carryCount, out carryCount);
                        bufferSpan[prevIndex] = prevValue;
                    }
                }

                #endregion

                #region//执行换包

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static byte[] ChangePacket_Filter(FilterInfo sfi)
                {
                    if (string.IsNullOrEmpty(sfi.FModify))
                    {
                        return Array.Empty<byte>();
                    }

                    try
                    {
                        var modifications = FilterConfig.Filter.ParseModifications(sfi.FModify);
                        if (modifications.Count == 0)
                            return Array.Empty<byte>();

                        byte[] newBuffer = new byte[modifications.Max(m => m.Index) + 1];
                        FilterConfig.Filter.ApplyModifications(newBuffer, modifications);

                        if (!string.IsNullOrEmpty(sfi.ProgressionPosition))
                        {
                            ApplyProgressions(sfi, newBuffer);
                        }

                        return newBuffer;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(ChangePacket_Filter), ex.Message);
                        return Array.Empty<byte>();
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static List<FilterConfig.Filter.Modification> ParseModifications(string modifyString)
                {
                    var modifications = new List<FilterConfig.Filter.Modification>();
                    string[] parts = modifyString.Split(',');

                    foreach (string part in parts)
                    {
                        if (string.IsNullOrEmpty(part))
                            continue;

                        string[] pair = part.Split('|');
                        if (pair.Length != 2)
                            continue;

                        if (int.TryParse(pair[0], out int index) &&
                            byte.TryParse(pair[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                        {
                            modifications.Add(new FilterConfig.Filter.Modification { Index = index, Value = value });
                        }
                    }

                    return modifications;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static void ApplyModifications(byte[] buffer, List<FilterConfig.Filter.Modification> modifications)
                {
                    foreach (var mod in modifications)
                    {
                        if (mod.Index >= 0 && mod.Index < buffer.Length)
                        {
                            buffer[mod.Index] = mod.Value;
                        }
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static void ApplyProgressions(FilterInfo sfi, byte[] buffer)
                {
                    int carryCount = 0;
                    int step = (int)sfi.ProgressionStep;
                    string[] positions = sfi.ProgressionPosition.Split(',');

                    foreach (string position in positions)
                    {
                        if (string.IsNullOrEmpty(position) ||
                            !int.TryParse(position, out int index) ||
                            index < 0 ||
                            index >= buffer.Length)
                        {
                            continue;
                        }

                        byte currentValue = buffer[index];
                        byte newValue = SystemConfig.GetStepByte(currentValue, step * (sfi.ProgressionCount + 1), out carryCount);
                        buffer[index] = newValue;
                        sfi.IsProgressionDone = true;

                        if (sfi.IsProgressionCarry && carryCount > 0)
                        {
                            for (int i = 0; i < sfi.ProgressionCarryNumber; i++)
                            {
                                int prevIndex = index - (i + 1);
                                if (prevIndex < 0)
                                    break;

                                byte prevValue = buffer[prevIndex];
                                prevValue = SystemConfig.GetStepByte(prevValue, carryCount, out carryCount);
                                buffer[prevIndex] = prevValue;

                                if (carryCount == 0)
                                    break;
                            }
                        }
                    }
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
            }

            #endregion

            #region//滤镜列表

            public static class List
            {
                public static bool IsFilterListFormShow = false;
                public static BindingList<FilterInfo> lstFilterInfo = new BindingList<FilterInfo>();

                #region//滤镜入列表

                public static void FilterToList(FilterInfo fi)
                {
                    try
                    {
                        FilterConfig.List.lstFilterInfo.Add(fi);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(FilterToList), ex.Message);
                    }
                }

                #endregion

                #region//初始化滤镜列表的计数

                public static void InitFilterList_Count()
                {
                    try
                    {
                        foreach (FilterInfo sfi in lstFilterInfo)
                        {
                            sfi.ExecutionCount = 0;
                            sfi.ProgressionCount = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(InitFilterList_Count), ex.Message);
                    }
                }

                #endregion

                #region//清空滤镜列表（对话框）

                public static void CleanUpFilterList_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miFilterList", "滤镜列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                    {
                        Icon = TType.Warn,                   
                        Keyboard = false,
                        MaskClosable = false,
                        OnOk = config =>
                        {
                            FilterConfig.List.FilterListClear();
                            return true;
                        }
                    });
                }

                public static void FilterListClear()
                {
                    try
                    {
                        lstFilterInfo.Clear();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(FilterListClear), ex.Message);
                    }
                }

                #endregion

                #region//获取滤镜列表执行模式

                public static FilterConfig.Filter.Execute GetFilterListExecute_ByString(string sFLExecute)
                {
                    FilterConfig.Filter.Execute FLExecute = new FilterConfig.Filter.Execute();

                    try
                    {
                        FLExecute = (FilterConfig.Filter.Execute)Enum.Parse(typeof(FilterConfig.Filter.Execute), sFLExecute);
                    }
                    catch (Exception ex)
                    {
                        FLExecute = FilterConfig.Filter.Execute.Priority;
                        Operate.DoLog(nameof(GetFilterListExecute_ByString), ex.Message);
                    }

                    return FLExecute;
                }

                #endregion

                #region//滤镜列表的列表操作

                public static void UpdateFilterList_ByListAction(Form form, SystemConfig.ListAction listAction, List<FilterInfo> fiList)
                {
                    try
                    {
                        switch (listAction)
                        {
                            case SystemConfig.ListAction.Top:

                                foreach (FilterInfo fi in fiList)
                                {
                                    FilterConfig.List.lstFilterInfo.Remove(fi);
                                    FilterConfig.List.lstFilterInfo.Insert(0, fi);
                                }

                                break;

                            case SystemConfig.ListAction.Up:

                                foreach (FilterInfo fi in fiList)
                                {
                                    int iIndex = FilterConfig.List.lstFilterInfo.IndexOf(fi);

                                    if (iIndex > 0)
                                    {
                                        FilterConfig.List.lstFilterInfo.Remove(fi);
                                        FilterConfig.List.lstFilterInfo.Insert(iIndex - 1, fi);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Down:

                                foreach (FilterInfo fi in fiList)
                                {
                                    int iIndex = FilterConfig.List.lstFilterInfo.IndexOf(fi);

                                    if (iIndex > -1 && iIndex < FilterConfig.List.lstFilterInfo.Count - 1)
                                    {
                                        FilterConfig.List.lstFilterInfo.Remove(fi);
                                        FilterConfig.List.lstFilterInfo.Insert(iIndex + 1, fi);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Bottom:

                                foreach (FilterInfo fi in fiList)
                                {
                                    FilterConfig.List.lstFilterInfo.Remove(fi);
                                    FilterConfig.List.lstFilterInfo.Add(fi);
                                }

                                break;

                            case SystemConfig.ListAction.Copy:

                                foreach (FilterInfo fi in fiList)
                                {
                                    FilterConfig.Filter.CopyFilter(fi);
                                }

                                break;

                            case SystemConfig.ListAction.Export:

                                string sFName = fiList[0].FName;
                                FilterConfig.List.SaveFilterList_Dialog(form, sFName, fiList);

                                break;

                            case SystemConfig.ListAction.Delete:

                                FilterConfig.Filter.DeleteFilter_Dialog(form, fiList);

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateFilterList_ByListAction), ex.Message);
                    }
                }

                #endregion

                #region//执行滤镜列表

                public static FilterConfig.Filter.FilterAction DoFilterList(
                    Int32 iSocket, 
                    Span<byte> bufferSpan, 
                    out byte[] bNewBuffer, 
                    PacketConfig.Packet.PacketType ptType, 
                    PacketConfig.Packet.SockAddr sAddr)
                {
                    FilterConfig.Filter.FilterAction faReturn = FilterConfig.Filter.FilterAction.None;
                    bNewBuffer = null;

                    try
                    {
                        var filters = FilterConfig.List.lstFilterInfo;
                        for (int i = 0; i < filters.Count; i++)
                        {
                            FilterConfig.Filter.FilterAction faDoFilter = FilterConfig.Filter.DoFilter(filters[i], iSocket, bufferSpan, out bNewBuffer, ptType, sAddr);

                            if (faDoFilter != Filter.FilterAction.None)
                            {
                                faReturn = faDoFilter;

                                if (faReturn == Filter.FilterAction.Intercept ||
                                    faReturn == Filter.FilterAction.Change ||
                                    faReturn == Filter.FilterAction.NoModify_Display ||
                                    faReturn == Filter.FilterAction.NoModify_NoDisplay ||                                    
                                    FilterConfig.Filter.FilterExecute == FilterConfig.Filter.Execute.Priority)
                                {
                                    if (bNewBuffer == null)
                                    {
                                        bNewBuffer = bufferSpan.ToArray();
                                    }

                                    return faReturn;
                                }
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DoFilterList), ex.Message);
                    }

                    if (bNewBuffer == null)
                    {
                        bNewBuffer = bufferSpan.ToArray();
                    }                    

                    return faReturn;
                }

                #endregion

                #region//保存滤镜列表到数据库

                public static void SaveFilterList_ToDB()
                {
                    try
                    {
                        DataBase.DeleteTable_Filter();

                        foreach (FilterInfo sfi in FilterConfig.List.lstFilterInfo)
                        {
                            DataBase.InsertTable_Filter(sfi);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveFilterList_ToDB), ex.Message);
                    }
                }

                #endregion

                #region//从数据库加载滤镜列表

                public static void LoadFilterList_FromDB()
                {
                    try
                    {
                        DataTable dtFilter = DataBase.SelectTable_Filter();

                        foreach (DataRow dataRow in dtFilter.Rows)
                        {
                            bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);
                            Guid FID = Guid.Parse(dataRow["GUID"].ToString());
                            string FName = dataRow["Name"].ToString();
                            bool AppointHeader = Convert.ToBoolean(dataRow["AppointHeader"]);
                            string FHeaderContent = dataRow["HeaderContent"].ToString();
                            bool AppointSocket = Convert.ToBoolean(dataRow["AppointSocket"]);
                            string FSocketContent = dataRow["SocketContent"].ToString();
                            bool AppointLength = Convert.ToBoolean(dataRow["AppointLength"]);
                            string FLengthContent = dataRow["LengthContent"].ToString();
                            bool AppointPort = Convert.ToBoolean(dataRow["AppointPort"]);
                            string FPortContent = dataRow["PortContent"].ToString();
                            FilterConfig.Filter.FilterMode FilterMode = FilterConfig.Filter.GetFilterMode_ByString(dataRow["Mode"].ToString());
                            FilterConfig.Filter.FilterAction FilterAction = FilterConfig.Filter.GetFilterAction_ByString(dataRow["Action"].ToString());
                            bool IsExecute = Convert.ToBoolean(dataRow["IsExecute"]);
                            FilterConfig.Filter.FilterExecuteType FilterExecuteType = FilterConfig.Filter.GetFilterExecuteType_ByString(dataRow["ExecuteType"].ToString());
                            Guid Execute_GUID = Guid.Parse(dataRow["ExecuteGUID"].ToString());
                            FilterConfig.Filter.FilterFunction FilterFunction = FilterConfig.Filter.GetFilterFunction_ByString(dataRow["Function"].ToString());
                            FilterConfig.Filter.FilterStartFrom FilterStartFrom = FilterConfig.Filter.GetFilterStartFrom_ByString(dataRow["StartFrom"].ToString());
                            bool IsProgressionDone = false;
                            bool IsProgressionContinuous = Convert.ToBoolean(dataRow["IsProgressionContinuous"]);
                            int FProgressionStep = Convert.ToInt32(dataRow["ProgressionStep"]);
                            bool IsProgressionCarry = Convert.ToBoolean(dataRow["IsProgressionCarry"]);
                            int ProgressionCarryNumber = Convert.ToInt32(dataRow["ProgressionCarryNumber"]);
                            string FProgressionPosition = dataRow["ProgressionPosition"].ToString();
                            int ProgressionCount = 0;
                            string FExcludePosition = dataRow["ExcludePosition"].ToString();
                            string FSearch = dataRow["Search"].ToString();
                            string FModify = dataRow["Modify"].ToString();

                            FilterConfig.Filter.AddFilter(
                                IsEnable,
                                FID,
                                FName,
                                AppointHeader,
                                FHeaderContent,
                                AppointSocket,
                                FSocketContent,
                                AppointLength,
                                FLengthContent,
                                AppointPort,
                                FPortContent,
                                FilterMode,
                                FilterAction,
                                IsExecute,
                                FilterExecuteType,
                                Execute_GUID,
                                FilterFunction,
                                FilterStartFrom,
                                IsProgressionDone,
                                IsProgressionContinuous,
                                FProgressionStep,
                                IsProgressionCarry,
                                ProgressionCarryNumber,
                                FProgressionPosition,
                                ProgressionCount,
                                FExcludePosition,
                                FSearch,
                                FModify);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadFilterList_FromDB), ex.Message);
                    }
                }

                #endregion

                #region//保存滤镜列表到文件（对话框）

                public static void SaveFilterList_Dialog(Form form, string FileName, List<FilterInfo> fiList)
                {
                    try
                    {
                        if (FilterConfig.List.lstFilterInfo.Count > 0)
                        {
                            SaveFileDialog sfdSaveFile = new SaveFileDialog();
                            sfdSaveFile.Filter = AntdUI.Localization.Get("FilterListFile", "滤镜列表文件") + "（*.fp）|*.fp";

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveFile.FileName = FileName;
                            }

                            sfdSaveFile.RestoreDirectory = true;
                            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveFile.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("ExportFilterList", "导出滤镜列表"));

                                    if (SaveFilterList(FilePath, fiList, EncryptPassword.DoEncrypt, EncryptPassword.Password))
                                    {
                                        string Title = AntdUI.Localization.Get("ExportFilterList.Success", "导出滤镜列表成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SaveFilterList_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("ExportFilterList.Error", "导出滤镜列表失败");
                                        string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveFilterList_Dialog), ex.Message);
                    }
                }

                private static bool SaveFilterList(string FilePath, List<FilterInfo> fiList, bool DoEncrypt, string Password)
                {
                    try
                    {
                        XDocument xdoc = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };

                        XElement xeRoot = FilterConfig.List.GetFilterList_XML(fiList);
                        if (xeRoot == null)
                        {
                            return false;
                        }

                        xdoc.Add(xeRoot);
                        xdoc.Save(FilePath);

                        if (DoEncrypt)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                SystemConfig.EncryptXMLFile(FilePath, Password);
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveFilterList), ex.Message);
                    }

                    return false;
                }

                public static XElement GetFilterList_XML(List<FilterInfo> fiList)
                {
                    try
                    {
                        XElement xeRoot = new XElement("FilterList");

                        if (fiList == null)
                        {
                            fiList = Operate.FilterConfig.List.lstFilterInfo.ToList();
                        }

                        foreach (FilterInfo fi in fiList)
                        {
                            XElement xeFilter =
                                new XElement("Filter",
                                new XElement("IsEnable", fi.IsEnable.ToString()),
                                new XElement("ID", fi.FID.ToString().ToUpper()),
                                new XElement("Name", fi.FName),
                                new XElement("AppointHeader", fi.AppointHeader.ToString()),
                                new XElement("HeaderContent", fi.HeaderContent),
                                new XElement("AppointSocket", fi.AppointSocket.ToString()),
                                new XElement("SocketContent", fi.SocketContent),
                                new XElement("AppointLength", fi.AppointLength.ToString()),
                                new XElement("LengthContent", fi.LengthContent),
                                new XElement("AppointPort", fi.AppointPort.ToString()),
                                new XElement("PortContent", fi.PortContent),
                                new XElement("Mode", fi.FMode),
                                new XElement("Action", fi.FAction),
                                new XElement("IsExecute", fi.IsExecute.ToString()),
                                new XElement("ExecuteType", fi.FEType),
                                new XElement("ExecuteGUID", fi.Execute_GUID.ToString().ToUpper()),
                                new XElement("Function", FilterConfig.Filter.GetFilterFunctionString(fi.FFunction)),
                                new XElement("StartFrom", fi.FStartFrom),
                                new XElement("IsProgressionContinuous", fi.IsProgressionContinuous.ToString()),
                                new XElement("ProgressionStep", fi.ProgressionStep),
                                new XElement("IsProgressionCarry", fi.IsProgressionCarry.ToString()),
                                new XElement("ProgressionCarryNumber", fi.ProgressionCarryNumber),
                                new XElement("ProgressionPosition", fi.ProgressionPosition),
                                new XElement("ExcludePosition", fi.ExcludePosition),
                                new XElement("Search", fi.FSearch),
                                new XElement("Modify", fi.FModify)
                                );

                            xeRoot.Add(xeFilter);
                        }

                        return xeRoot;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetFilterList_XML), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//从文件加载滤镜列表（对话框）

                public static void LoadFilterList_Dialog(Form form)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();
                        ofdLoadFile.Filter = AntdUI.Localization.Get("FilterListFile", "滤镜列表文件") + "（*.fp）|*.fp";
                        ofdLoadFile.RestoreDirectory = true;

                        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = ofdLoadFile.FileName;
                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                if (LoadFilterList(form, FilePath, true))
                                {
                                    string Title = AntdUI.Localization.Get("ImportFilterList.Success", "导入滤镜列表成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(nameof(LoadFilterList_Dialog), Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadFilterList_Dialog), ex.Message);
                    }
                }

                private static bool LoadFilterList(Form form, string FilePath, bool LoadFromUser)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            XDocument xdoc = null;

                            bool bEncrypt = SystemConfig.IsEncryptXMLFile(FilePath);
                            if (bEncrypt)
                            {
                                if (LoadFromUser)
                                {
                                    xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("ImportFilterList", "导入滤镜列表"), FilePath);
                                }                                
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("Password.Incorrect", "导入失败: 密码错误");

                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(nameof(LoadFilterList), sError);
                                }

                                return false;
                            }

                            LoadFilterList_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadFilterList), ex.Message);
                    }

                    return false;
                }

                public static void LoadFilterList_FromXDocument(XDocument xdoc)
                {
                    try
                    {
                        foreach (XElement xeFilter in xdoc.Root.Elements())
                        {
                            bool bIsEnable = false;
                            if (xeFilter.Element("IsEnable") != null)
                            {
                                bIsEnable = bool.Parse(xeFilter.Element("IsEnable").Value);
                            }

                            Guid FID = Guid.Empty;
                            if (xeFilter.Element("ID") == null || !Guid.TryParse(xeFilter.Element("ID").Value, out FID) || FilterConfig.Filter.GetFilter_ByGuid(FID) != null)
                            {
                                FID = Guid.NewGuid();
                            }

                            string sFName = string.Empty;
                            if (xeFilter.Element("Name") != null)
                            {
                                sFName = xeFilter.Element("Name").Value;
                            }

                            bool bAppointHeader = false;
                            if (xeFilter.Element("AppointHeader") != null)
                            {
                                bAppointHeader = bool.Parse(xeFilter.Element("AppointHeader").Value);
                            }

                            string sFHeaderContent = string.Empty;
                            if (xeFilter.Element("HeaderContent") != null)
                            {
                                sFHeaderContent = xeFilter.Element("HeaderContent").Value;
                            }

                            bool bAppointSocket = false;
                            if (xeFilter.Element("AppointSocket") != null)
                            {
                                bAppointSocket = bool.Parse(xeFilter.Element("AppointSocket").Value);
                            }

                            string sFSocketContent = string.Empty;
                            if (xeFilter.Element("SocketContent") != null)
                            {
                                sFSocketContent = xeFilter.Element("SocketContent").Value;
                            }

                            bool bAppointLength = false;
                            if (xeFilter.Element("AppointLength") != null)
                            {
                                bAppointLength = bool.Parse(xeFilter.Element("AppointLength").Value);
                            }

                            string sFLengthContent = string.Empty;
                            if (xeFilter.Element("LengthContent") != null)
                            {
                                sFLengthContent = xeFilter.Element("LengthContent").Value;
                            }

                            bool bAppointPort = false;
                            if (xeFilter.Element("AppointPort") != null)
                            {
                                bAppointPort = bool.Parse(xeFilter.Element("AppointPort").Value);
                            }

                            string sFPortContent = string.Empty;
                            if (xeFilter.Element("PortContent") != null)
                            {
                                sFPortContent = xeFilter.Element("PortContent").Value;
                            }

                            FilterConfig.Filter.FilterMode FilterMode = FilterConfig.Filter.FilterMode.Normal;
                            if (xeFilter.Element("Mode") != null)
                            {
                                FilterMode = FilterConfig.Filter.GetFilterMode_ByString(xeFilter.Element("Mode").Value);
                            }

                            FilterConfig.Filter.FilterAction FilterAction = FilterConfig.Filter.FilterAction.NoModify_Display;
                            if (xeFilter.Element("Action") != null)
                            {
                                FilterAction = FilterConfig.Filter.GetFilterAction_ByString(xeFilter.Element("Action").Value);
                            }

                            bool bIsExecute = false;
                            if (xeFilter.Element("IsExecute") != null)
                            {
                                bIsExecute = bool.Parse(xeFilter.Element("IsExecute").Value);
                            }

                            FilterConfig.Filter.FilterExecuteType FilterExecuteType = new FilterConfig.Filter.FilterExecuteType();
                            if (xeFilter.Element("ExecuteType") != null)
                            {
                                FilterExecuteType = FilterConfig.Filter.GetFilterExecuteType_ByString(xeFilter.Element("ExecuteType").Value);
                            }

                            Guid Execute_GUID = Guid.Empty;
                            if (xeFilter.Element("ExecuteGUID") != null)
                            {
                                Guid.TryParse(xeFilter.Element("ExecuteGUID").Value, out Execute_GUID);
                            }

                            FilterConfig.Filter.FilterFunction FilterFunction = new FilterConfig.Filter.FilterFunction();
                            if (xeFilter.Element("Function") != null)
                            {
                                FilterFunction = FilterConfig.Filter.GetFilterFunction_ByString(xeFilter.Element("Function").Value);
                            }

                            FilterConfig.Filter.FilterStartFrom FilterStartFrom = FilterConfig.Filter.FilterStartFrom.Head;
                            if (xeFilter.Element("StartFrom") != null)
                            {
                                FilterStartFrom = FilterConfig.Filter.GetFilterStartFrom_ByString(xeFilter.Element("StartFrom").Value);
                            }

                            bool IsProgressionDone = false;

                            bool bIsProgressionContinuous = false;
                            if (xeFilter.Element("IsProgressionContinuous") != null)
                            {
                                bIsProgressionContinuous = bool.Parse(xeFilter.Element("IsProgressionContinuous").Value);
                            }

                            int iFProgressionStep = 1;
                            if (xeFilter.Element("ProgressionStep") != null)
                            {
                                iFProgressionStep = int.Parse(xeFilter.Element("ProgressionStep").Value);
                            }

                            bool bIsProgressionCarry = false;
                            if (xeFilter.Element("IsProgressionCarry") != null)
                            {
                                bIsProgressionCarry = bool.Parse(xeFilter.Element("IsProgressionCarry").Value);
                            }

                            int iFProgressionCarryNumber = 1;
                            if (xeFilter.Element("ProgressionCarryNumber") != null)
                            {
                                iFProgressionCarryNumber = int.Parse(xeFilter.Element("ProgressionCarryNumber").Value);
                            }

                            string sFProgressionPosition = string.Empty;
                            if (xeFilter.Element("ProgressionPosition") != null)
                            {
                                sFProgressionPosition = xeFilter.Element("ProgressionPosition").Value;
                            }

                            int iProgressionCount = 0;

                            string sFExcludePosition = string.Empty;
                            if (xeFilter.Element("ExcludePosition") != null)
                            {
                                sFExcludePosition = xeFilter.Element("ExcludePosition").Value;
                            }

                            string sFSearch = string.Empty;
                            if (xeFilter.Element("Search") != null)
                            {
                                sFSearch = xeFilter.Element("Search").Value;
                            }

                            string sFModify = string.Empty;
                            if (xeFilter.Element("Modify") != null)
                            {
                                sFModify = xeFilter.Element("Modify").Value;
                            }

                            FilterConfig.Filter.AddFilter(
                                bIsEnable,
                                FID,
                                sFName,
                                bAppointHeader,
                                sFHeaderContent,
                                bAppointSocket,
                                sFSocketContent,
                                bAppointLength,
                                sFLengthContent,
                                bAppointPort,
                                sFPortContent,
                                FilterMode,
                                FilterAction,
                                bIsExecute,
                                FilterExecuteType,
                                Execute_GUID,
                                FilterFunction,
                                FilterStartFrom,
                                IsProgressionDone,
                                bIsProgressionContinuous,
                                iFProgressionStep,
                                bIsProgressionCarry,
                                iFProgressionCarryNumber,
                                sFProgressionPosition,
                                iProgressionCount,
                                sFExcludePosition,
                                sFSearch,
                                sFModify);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadFilterList_FromXDocument), ex.Message);
                    }
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region//发送配置

        public static class SendConfig
        {
            #region//发送

            public static class Send
            {
                #region//新增发送

                public static void AddSend_New()
                {
                    try
                    {
                        bool IsEnable = false;
                        Guid SID = Guid.NewGuid();
                        int SNum = SendConfig.List.lstSendInfo.Count + 1;
                        string SName = string.Format(AntdUI.Localization.Get("SendList.NewSend", "发送 {0}"), SNum.ToString());
                        bool SSystemSocket = false;
                        int SLoopCNT = 1;
                        int SLoopINT = 1000;
                        string SNotes = string.Empty;
                        BindingList<PacketInfo> SCollection = new BindingList<PacketInfo>();

                        Send.AddSend(IsEnable, SID, SName, SSystemSocket, SLoopCNT, SLoopINT, SCollection, SNotes);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddSend_New), ex.Message);
                    }
                }

                public static void AddSend(bool IsEnable, Guid SID, string SName, bool SSystemSocket, int SLoopCNT, int SLoopINT, BindingList<PacketInfo> SCollection, string SNotes)
                {
                    try
                    {
                        if (SID != Guid.Empty && !string.IsNullOrEmpty(SName))
                        {
                            SendInfo si = new SendInfo(IsEnable, SID, SName, SSystemSocket, SLoopCNT, SLoopINT, SCollection, SNotes);
                            SendConfig.List.SendToList(si);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddSend), ex.Message);
                    }
                }

                #endregion

                #region//新增发送集

                public static bool AddSendCollection_ByPacketInfo(Guid SID, List<PacketInfo> piList)
                {
                    try
                    {
                        if (SID != null && SID != Guid.Empty && piList.Count > 0)
                        {
                            foreach (SendInfo si in SendConfig.List.lstSendInfo)
                            {
                                if (si.SID == SID)
                                {
                                    foreach (PacketInfo pi in piList)
                                    {
                                        SendConfig.Send.AddSendCollection(si.SCollection, pi.PacketSocket, pi.PacketType, pi.PacketFrom, pi.PacketTo, pi.PacketBuffer);
                                    }
                                }
                            }

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddSendCollection_ByPacketInfo), ex.Message);
                    }

                    return false;
                }

                public static bool AddSendCollection_ByProxyInfo(Guid SID, List<ProxyInfo> piList)
                {
                    try
                    {
                        if (SID != null && SID != Guid.Empty && piList.Count > 0)
                        {
                            foreach (SendInfo si in SendConfig.List.lstSendInfo)
                            {
                                if (si.SID == SID)
                                {
                                    foreach (ProxyInfo pi in piList)
                                    {
                                        SendConfig.Send.AddSendCollection(si.SCollection, pi.PacketSocket, pi.PacketType, pi.ClientAddr, pi.ServerAddr, pi.PacketBuffer);
                                    }
                                }
                            }

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddSendCollection_ByProxyInfo), ex.Message);
                    }

                    return false;
                }

                public static void AddSendCollection(BindingList<PacketInfo> SCollection, int Socket, PacketConfig.Packet.PacketType ptType, string PacketFrom, string PacketTo, byte[] PacketBuffer)
                {
                    try
                    {
                        PacketInfo pi = new PacketInfo();
                        pi.PacketSocket = Socket;
                        pi.PacketType = ptType;
                        pi.PacketFrom = PacketFrom;
                        pi.PacketTo = PacketTo;
                        pi.PacketBuffer = PacketBuffer;
                        pi.PacketLen = PacketBuffer.Length;
                        pi.PacketData = PacketConfig.Packet.GetPacketData_Hex(PacketBuffer, PacketConfig.Packet.PacketData_MaxLen);
                        SCollection.Add(pi);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddSendCollection), ex.Message);
                    }
                }

                #endregion                

                #region//更新发送

                public static void UpdateSend(SendInfo ssi, string SName, bool SSystemSocket, int SLoopCNT, int SLoopINT, BindingList<PacketInfo> SCollection, string SNotes)
                {
                    try
                    {
                        if (ssi != null)
                        {
                            ssi.SName = SName;
                            ssi.SSystemSocket = SSystemSocket;
                            ssi.SLoopCNT = SLoopCNT;
                            ssi.SLoopINT = SLoopINT;
                            ssi.SCollection = new BindingList<PacketInfo>(SCollection.ToList());
                            ssi.SNotes = SNotes;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateSend), ex.Message);
                    }
                }

                #endregion

                #region//复制发送

                public static void CopySend(SendInfo ssi)
                {
                    try
                    {
                        bool IsEnable_Copy = false;
                        Guid SID_New = Guid.NewGuid();
                        string SName_Copy = string.Format(AntdUI.Localization.Get("CopyName", "{0} - 副本"), ssi.SName);
                        bool SSystemSocket_Copy = ssi.SSystemSocket;
                        int SLoopCNT_Copy = ssi.SLoopCNT;
                        int SLoopINT_Copy = ssi.SLoopINT;
                        BindingList<PacketInfo> SCollection_Copy = new BindingList<PacketInfo>(ssi.SCollection.ToList());
                        string SNotes_Copy = ssi.SNotes;

                        Send.AddSend(IsEnable_Copy, SID_New, SName_Copy, SSystemSocket_Copy, SLoopCNT_Copy, SLoopINT_Copy, SCollection_Copy, SNotes_Copy);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CopySend), ex.Message);
                    }
                }

                #endregion

                #region//删除发送（对话框）

                public static void DeleteSend_Dialog(Form form, List<SendInfo> siList)
                {
                    try
                    {
                        if (siList.Count > 0)
                        {
                            AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miSendList", "发送列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                            {
                                Icon = TType.Warn,
                                Keyboard = false,
                                MaskClosable = false,
                                OnOk = config =>
                                {
                                    foreach (SendInfo si in siList)
                                    {
                                        SendConfig.List.lstSendInfo.Remove(si);
                                    }

                                    return true;
                                }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteSend_Dialog), ex.Message);
                    }
                }

                #endregion

                #region//编辑发送

                public static void OpenSendEdit(Form form, SendInfo si)
                {
                    var SendEdit = new SendEdit(form, si);
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("SendEditForm", "发送编辑"), SendEdit)
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//获取发送

                public static SendInfo GetSend_ByGuid(Guid SID)
                {
                    try
                    {
                        if (SID != null && SID != Guid.Empty)
                        {
                            foreach (SendInfo si in SendConfig.List.lstSendInfo)
                            {
                                if (si.SID == SID)
                                {
                                    return si;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetSend_ByGuid), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//获取发送名称

                public static string GetSendName_ByGuid(Guid SID)
                {
                    string sReturn = string.Empty;

                    try
                    {
                        if (SID != null && SID != Guid.Empty)
                        {
                            foreach (SendInfo ssi in SendConfig.List.lstSendInfo)
                            {
                                if (ssi.SID == SID)
                                {
                                    return ssi.SName;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetSendName_ByGuid), ex.Message);
                    }

                    return sReturn;
                }

                #endregion

                #region//获取发送集

                public static BindingList<PacketInfo> GetSendCollection_ByGuid(Guid SID)
                {
                    BindingList<PacketInfo> sscReturn = null;

                    try
                    {
                        if (SID != null && SID != Guid.Empty)
                        {
                            foreach (SendInfo ssi in SendConfig.List.lstSendInfo)
                            {
                                if (ssi.SID == SID)
                                {
                                    return ssi.SCollection;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetSendCollection_ByGuid), ex.Message);
                    }

                    return sscReturn;
                }

                #endregion

                #region//执行发送

                public static SendExecute DoSend(Guid SID)
                {
                    return Task.Run(() => DoSendAsync(SID))
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();
                }

                public static async Task DoSend_ByIndex(int SendListIndex)
                {
                    try
                    {
                        if (SendListIndex > -1 && SendListIndex < SendConfig.List.lstSendInfo.Count)
                        {
                            if (SendConfig.List.lstSendInfo[SendListIndex].IsEnable)
                            {
                                Guid SID = SendConfig.List.lstSendInfo[SendListIndex].SID;
                                Operate.SendConfig.List.lstSendExecute.Add(await DoSendAsync(SID));
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DoSend_ByIndex), ex.Message);
                    }
                }

                public static async Task<SendExecute> DoSendAsync(Guid SID)
                {
                    SendExecute seReturn = null;

                    try
                    {
                        if (SID != null && SID != Guid.Empty)
                        {
                            SendInfo si = SendConfig.List.lstSendInfo.Where(item => item.SID == SID).FirstOrDefault();

                            if (si != null)
                            {
                                if (si.IsEnable)
                                {
                                    if (si.SCollection.Count > 0)
                                    {
                                        seReturn = new SendExecute();
                                        await Task.Run(() => seReturn.StartSend(si));
                                    }
                                }                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DoSendAsync), ex.Message);
                    }

                    return seReturn;
                }

                #endregion

                #region//设置发送是否启用

                public static void SetIsEnable_ByGUID(Guid SID, bool IsEnable)
                {
                    try
                    {
                        SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                        if (si != null)
                        {
                            si.IsEnable = IsEnable;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SetIsEnable_ByGUID), ex.Message);
                    }
                }

                #endregion

                #region//发送集的列表操作

                public static void UpdateSendCollection_ByListAction(Form form, BindingList<PacketInfo> SendCollection, SystemConfig.ListAction listAction, List<PacketInfo> piList)
                {
                    try
                    {
                        switch (listAction)
                        {
                            case SystemConfig.ListAction.Top:

                                foreach (PacketInfo pi in piList)
                                {
                                    SendCollection.Remove(pi);
                                    SendCollection.Insert(0, pi);
                                }

                                break;

                            case SystemConfig.ListAction.Up:

                                foreach (PacketInfo pi in piList)
                                {
                                    int iIndex = SendCollection.IndexOf(pi);
                                    if (iIndex > 0)
                                    {
                                        SendCollection.Remove(pi);
                                        SendCollection.Insert(iIndex - 1, pi);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Down:

                                foreach (PacketInfo pi in piList)
                                {
                                    int iIndex = SendCollection.IndexOf(pi);
                                    if (iIndex > -1 && iIndex < SendCollection.Count - 1)
                                    {
                                        SendCollection.Remove(pi);
                                        SendCollection.Insert(iIndex + 1, pi);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Bottom:

                                foreach (PacketInfo pi in piList)
                                {
                                    SendCollection.Remove(pi);
                                    SendCollection.Add(pi);
                                }

                                break;

                            case SystemConfig.ListAction.Copy:

                                foreach (PacketInfo pi in piList)
                                {
                                    SendConfig.Send.AddSendCollection(
                                        SendCollection, 
                                        pi.PacketSocket, 
                                        pi.PacketType, 
                                        pi.PacketFrom, 
                                        pi.PacketTo, 
                                        pi.PacketBuffer);
                                }

                                break;

                            case SystemConfig.ListAction.Delete:

                                foreach (PacketInfo pi in piList)
                                {
                                    SendCollection.Remove(pi);
                                }

                                break;

                            case SystemConfig.ListAction.Export:

                                if (piList.Count > 0)
                                {
                                    Send.SaveSendCollection_Dialog(form, string.Empty, piList);
                                }
                                else
                                {
                                    Send.SaveSendCollection_Dialog(form, string.Empty, SendCollection.ToList());
                                }                                

                                break;

                            case SystemConfig.ListAction.Import:

                                Send.LoadSendCollection_Dialog(form, SendCollection);

                                break;

                            case SystemConfig.ListAction.CleanUp:

                                if (SendCollection.Count > 0)
                                {
                                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("SendCollection", "发送集列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                                    {
                                        Icon = TType.Warn,
                                        Keyboard = false,
                                        MaskClosable = false,
                                        OnOk = config =>
                                        {
                                            SendCollection.Clear();
                                            return true;
                                        }
                                    });
                                }

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateSendCollection_ByListAction), ex.Message);
                    }
                }

                #endregion

                #region//保存发送集（对话框）

                public static void SaveSendCollection_Dialog(Form form, string FileName, List<PacketInfo> SendCollection)
                {
                    try
                    {
                        if (SendCollection.Count > 0)
                        {
                            SaveFileDialog sfdSaveFile = new SaveFileDialog();
                            sfdSaveFile.Filter = AntdUI.Localization.Get("SendList.SendCollectionFile", "发送集文件") + "（*.sc）|*.sc";

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveFile.FileName = FileName;
                            }

                            sfdSaveFile.RestoreDirectory = true;
                            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveFile.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("ExportSendCollection", "导出发送集"));

                                    if (SaveSendCollection(FilePath, SendCollection, EncryptPassword.DoEncrypt, EncryptPassword.Password))
                                    {
                                        string Title = AntdUI.Localization.Get("ExportSendCollection.Success", "导出发送集成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SaveSendCollection_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("ExportSendCollection.Error", "导出发送集失败");
                                        string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveSendCollection_Dialog), ex.Message);
                    }
                }

                public static bool SaveSendCollection(string FilePath, List<PacketInfo> SendCollection, bool DoEncrypt, string Password)
                {
                    try
                    {
                        SaveSendCollection_ToXDocument(FilePath, SendCollection);

                        if (DoEncrypt)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                SystemConfig.EncryptXMLFile(FilePath, Password);
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveSendCollection), ex.Message);
                    }

                    return false;
                }

                private static void SaveSendCollection_ToXDocument(string FilePath, List<PacketInfo> SendCollection)
                {
                    try
                    {
                        XDocument xdoc = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };

                        XElement xeRoot = new XElement("SendCollection");
                        xdoc.Add(xeRoot);

                        foreach (PacketInfo spi in SendCollection)
                        {
                            string sBuffer = SystemConfig.BytesToString(PacketConfig.Packet.EncodingFormat.Hex, spi.PacketBuffer);

                            XElement xeColl =
                                new XElement("Collection",
                                new XElement("Socket", spi.PacketSocket),
                                new XElement("Type", spi.PacketType),
                                new XElement("IPFrom", spi.PacketFrom),
                                new XElement("IPTo", spi.PacketTo),
                                new XElement("Buffer", sBuffer)
                                );

                            xeRoot.Add(xeColl);
                        }

                        xdoc.Save(FilePath);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveSendCollection_ToXDocument), ex.Message);
                    }
                }

                #endregion

                #region//加载发送集（对话框）

                public static void LoadSendCollection_Dialog(Form form, BindingList<PacketInfo> SendCollection)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();
                        ofdLoadFile.Filter = AntdUI.Localization.Get("SendList.SendCollectionFile", "发送集文件") + "（*.sc）|*.sc";
                        ofdLoadFile.RestoreDirectory = true;

                        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = ofdLoadFile.FileName;
                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                if (LoadSendCollection(form, FilePath, SendCollection, true))
                                {
                                    string Title = AntdUI.Localization.Get("InjectModeForm.ImportSendCollection.Success", "导入发送集成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(nameof(LoadSendCollection_Dialog), Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadSendCollection_Dialog), ex.Message);
                    }
                }

                public static bool LoadSendCollection(Form form, string FilePath, BindingList<PacketInfo> SendCollection, bool LoadFromUser)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            XDocument xdoc = null;

                            bool bEncrypt = SystemConfig.IsEncryptXMLFile(FilePath);
                            if (bEncrypt)
                            {
                                if (LoadFromUser)
                                {
                                    xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("ImportSendCollection", "导入发送集"), FilePath);
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("Password.Incorrect", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(nameof(LoadSendCollection), sError);
                                }

                                return false;
                            }

                            LoadSendCollection_FromXDocument(xdoc, SendCollection);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadSendCollection), ex.Message);
                    }
                    return false;
                }

                private static void LoadSendCollection_FromXDocument(XDocument xdoc, BindingList<PacketInfo> SendCollection)
                {
                    try
                    {
                        XElement xeRoot = xdoc.Root;

                        switch (xeRoot.Name.LocalName)
                        {
                            case "SendList":

                                #region//SendList File

                                foreach (XElement xeSend in xeRoot.Elements())
                                {
                                    int iSocket = 0;
                                    if (xeSend.Element("Socket") != null)
                                    {
                                        iSocket = int.Parse(xeSend.Element("Socket").Value);
                                    }

                                    PacketConfig.Packet.PacketType ptType = new PacketConfig.Packet.PacketType();
                                    if (xeSend.Element("Type") != null)
                                    {
                                        ptType = PacketConfig.Packet.GetPacketType_ByString(xeSend.Element("Type").Value);
                                    }

                                    string sIPFrom = string.Empty;
                                    if (xeSend.Element("IPFrom") != null)
                                    {
                                        sIPFrom = xeSend.Element("IPFrom").Value;
                                    }

                                    string sIPTo = string.Empty;
                                    if (xeSend.Element("ToAddress") != null)
                                    {
                                        sIPTo = xeSend.Element("ToAddress").Value;
                                    }

                                    byte[] bBuffer = null;
                                    if (xeSend.Element("Data") != null)
                                    {
                                        bBuffer = SystemConfig.StringToBytes(PacketConfig.Packet.EncodingFormat.Hex, xeSend.Element("Data").Value);
                                    }

                                    Send.AddSendCollection(SendCollection, iSocket, ptType, sIPFrom, sIPTo, bBuffer);
                                }

                                #endregion

                                break;

                            case "SendCollection":

                                #region//SendCollection File

                                foreach (XElement xeCollection in xeRoot.Elements())
                                {
                                    int iSocket = 0;
                                    if (xeCollection.Element("Socket") != null)
                                    {
                                        iSocket = int.Parse(xeCollection.Element("Socket").Value);
                                    }

                                    PacketConfig.Packet.PacketType ptType = new PacketConfig.Packet.PacketType();
                                    if (xeCollection.Element("Type") != null)
                                    {
                                        ptType = PacketConfig.Packet.GetPacketType_ByString(xeCollection.Element("Type").Value);
                                    }

                                    string sIPFrom = string.Empty;
                                    if (xeCollection.Element("IPFrom") != null)
                                    {
                                        sIPFrom = xeCollection.Element("IPFrom").Value;
                                    }

                                    string sIPTo = string.Empty;
                                    if (xeCollection.Element("IPTo") != null)
                                    {
                                        sIPTo = xeCollection.Element("IPTo").Value;
                                    }

                                    byte[] bBuffer = null;
                                    if (xeCollection.Element("Buffer") != null)
                                    {
                                        bBuffer = SystemConfig.StringToBytes(PacketConfig.Packet.EncodingFormat.Hex, xeCollection.Element("Buffer").Value);
                                    }

                                    Send.AddSendCollection(SendCollection, iSocket, ptType, sIPFrom, sIPTo, bBuffer);
                                }

                                #endregion

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadSendCollection_FromXDocument), ex.Message);
                    }
                }

                #endregion
            }

            #endregion

            #region//发送列表

            public static class List
            {
                public static List<SendExecute> lstSendExecute = new List<SendExecute>();
                public static BindingList<SendInfo> lstSendInfo = new BindingList<SendInfo>();
                public static BackgroundWorker bgwSendList = new BackgroundWorker();

                #region//发送列表索引项

                public class SendListItem
                {
                    public string SName { get; set; }

                    public Guid SID { get; set; }

                    public override string ToString()
                    {
                        return SName;
                    }
                }

                #endregion

                #region//发送入列表

                public static void SendToList(SendInfo si)
                {
                    try
                    {
                        SendConfig.List.lstSendInfo.Add(si);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SendToList), ex.Message);
                    }
                }

                #endregion                

                #region//执行发送列表

                public static void StartSendList()
                {
                    try
                    {
                        if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                        {
                            if (!Operate.SendConfig.List.bgwSendList.IsBusy)
                            {
                                Operate.SendConfig.List.lstSendExecute.Clear();
                                Operate.SendConfig.List.bgwSendList.RunWorkerAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(StartSendList), ex.Message);
                    }
                }

                public static void StopSendList()
                {
                    try
                    {
                        if (Operate.SendConfig.List.bgwSendList.IsBusy)
                        {
                            Operate.SendConfig.List.bgwSendList.CancelAsync();                            
                        }

                        foreach (SendExecute se in Operate.SendConfig.List.lstSendExecute.ToList())
                        {
                            if (se.Worker.IsBusy)
                            {
                                se.StopSend();                                
                            }

                            Operate.SendConfig.List.lstSendExecute.Remove(se);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(StopSendList), ex.Message);
                    }
                }

                public static void SendList_DoWork(object sender, DoWorkEventArgs e)
                {
                    try
                    {
                        for (int index = 0; index < Operate.SendConfig.List.lstSendInfo.Count; index++)
                        {
                            SendInfo si = Operate.SendConfig.List.lstSendInfo[index];
                            if (si.IsEnable)
                            {
                                SendExecute se = Operate.SendConfig.Send.DoSend(si.SID);
                                if (se != null)
                                {
                                    if (Operate.SystemConfig.ListExecute == Operate.SystemConfig.Execute.Together)
                                    {
                                        Operate.SendConfig.List.lstSendExecute.Add(se);
                                    }
                                    else
                                    {
                                        while (se.Worker.IsBusy)
                                        {
                                            if (bgwSendList.CancellationPending)
                                            {
                                                se.StopSend();

                                                e.Cancel = true;
                                                return;
                                            }

                                            Thread.Sleep(10);
                                        }
                                    }
                                }
                            }
                        }

                        while (Operate.SendConfig.List.lstSendExecute.Count > 0)
                        {
                            foreach (SendExecute se in Operate.SendConfig.List.lstSendExecute.ToList())
                            {
                                if (bgwSendList.CancellationPending)
                                {
                                    se.StopSend();
                                }

                                if (!se.Worker.IsBusy)
                                {
                                    Operate.SendConfig.List.lstSendExecute.Remove(se);
                                }
                            }

                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SendList_DoWork), ex.Message);
                    }
                }

                #endregion

                #region//初始化发送列表的计数

                public static void InitSendList_Count()
                {
                    try
                    {
                        foreach (SendInfo si in lstSendInfo)
                        {
                            si.ExecutionCount = 0;
                            si.ExecutionSuccess = 0;
                            si.ExecutionFail = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(InitSendList_Count), ex.Message);
                    }
                }

                #endregion

                #region//发送列表的列表操作

                public static void UpdateSendList_ByListAction(Form form, SystemConfig.ListAction listAction, List<SendInfo> siList)
                {
                    try
                    {
                        switch (listAction)
                        {
                            case SystemConfig.ListAction.Top:

                                foreach (SendInfo si in siList)
                                {
                                    SendConfig.List.lstSendInfo.Remove(si);
                                    SendConfig.List.lstSendInfo.Insert(0, si);
                                }

                                break;

                            case SystemConfig.ListAction.Up:

                                foreach (SendInfo si in siList)
                                {
                                    int iIndex = SendConfig.List.lstSendInfo.IndexOf(si);
                                    if (iIndex > 0)
                                    {
                                        SendConfig.List.lstSendInfo.Remove(si);
                                        SendConfig.List.lstSendInfo.Insert(iIndex - 1, si);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Down:

                                foreach (SendInfo si in siList)
                                {
                                    int iIndex = SendConfig.List.lstSendInfo.IndexOf(si);
                                    if (iIndex > -1 && iIndex < SendConfig.List.lstSendInfo.Count - 1)
                                    {
                                        SendConfig.List.lstSendInfo.Remove(si);
                                        SendConfig.List.lstSendInfo.Insert(iIndex + 1, si);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Bottom:

                                foreach (SendInfo si in siList)
                                {
                                    SendConfig.List.lstSendInfo.Remove(si);
                                    SendConfig.List.lstSendInfo.Add(si);
                                }

                                break;

                            case SystemConfig.ListAction.Copy:

                                foreach (SendInfo si in siList)
                                {
                                    SendConfig.Send.CopySend(si);
                                }

                                break;

                            case SystemConfig.ListAction.Export:

                                string SName = siList[0].SName;
                                SendConfig.List.SaveSendList_Dialog(form, SName, siList);

                                break;

                            case SystemConfig.ListAction.Delete:

                                SendConfig.Send.DeleteSend_Dialog(form, siList);

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateSendList_ByListAction), ex.Message);
                    }
                }

                #endregion

                #region//获取添加到发送列表的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_ToSendList()
                {
                    AntdUI.IContextMenuStripItem[] imsReturn = new AntdUI.IContextMenuStripItem[Operate.SendConfig.List.lstSendInfo.Count];
                    if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                    {
                        for (int i = 0; i < imsReturn.Length; i++)
                        {
                            imsReturn[i] = new AntdUI.ContextMenuStripItem(Operate.SendConfig.List.lstSendInfo[i].SName)
                            {
                                ID = Operate.SendConfig.List.lstSendInfo[i].SID.ToString().ToUpper(),
                            };
                        }
                    }

                    return imsReturn;
                }

                #endregion

                #region//清空发送列表（对话框）

                public static void CleanUpSendList_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miSendList", "发送列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                    {
                        Icon = TType.Warn,
                        Keyboard = false,
                        MaskClosable = false,
                        OnOk = config =>
                        {
                            SendConfig.List.SendListClear();
                            return true;
                        }
                    });
                }

                public static void SendListClear()
                {
                    lstSendInfo.Clear();
                }

                #endregion

                #region//保存发送列表到数据库

                public static void SaveSendList_ToDB()
                {
                    try
                    {
                        DataBase.DeleteTable_Send();

                        foreach (SendInfo ssi in SendConfig.List.lstSendInfo)
                        {
                            DataBase.InsertTable_Send(ssi);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveSendList_ToDB), ex.Message);
                    }
                }

                #endregion

                #region//从数据库加载发送列表

                public static void LoadSendList_FromDB()
                {
                    try
                    {
                        DataTable dtSend = DataBase.SelectTable_Send();
                        foreach (DataRow dataRow in dtSend.Rows)
                        {
                            Guid SID = Guid.Parse(dataRow["GUID"].ToString());
                            bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);
                            string SName = dataRow["Name"].ToString();
                            bool SSystemSocket = Convert.ToBoolean(dataRow["SystemSocket"]);
                            int SLoopCNT = Convert.ToInt32(dataRow["LoopCNT"]);
                            int SLoopINT = Convert.ToInt32(dataRow["LoopINT"]);
                            string SNotes = dataRow["Notes"].ToString();
                            BindingList<PacketInfo> SCollection = new BindingList<PacketInfo>();

                            DataTable dtSCollection = DataBase.SelectTable_SendCollection(SID);
                            foreach (DataRow row in dtSCollection.Rows)
                            {
                                int Socket = Convert.ToInt32(row["Socket"]);
                                PacketConfig.Packet.PacketType ptType = PacketConfig.Packet.GetPacketType_ByString(row["Type"].ToString());
                                string IPFrom = row["IPFrom"].ToString();
                                string IPTo = row["IPTo"].ToString();
                                byte[] Buffer = (byte[])row["Buffer"];

                                Send.AddSendCollection(SCollection, Socket, ptType, IPFrom, IPTo, Buffer);
                            }

                            Send.AddSend(IsEnable, SID, SName, SSystemSocket, SLoopCNT, SLoopINT, SCollection, SNotes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadSendList_FromDB), ex.Message);
                    }
                }

                #endregion

                #region//保存发送列表到文件（对话框）

                public static void SaveSendList_Dialog(Form form, string FileName, List<SendInfo> siList)
                {
                    try
                    {
                        if (SendConfig.List.lstSendInfo.Count > 0)
                        {
                            SaveFileDialog sfdSaveFile = new SaveFileDialog();
                            sfdSaveFile.Filter = AntdUI.Localization.Get("SendListFile", "发送列表文件") + "（*.sp）|*.sp";
                            sfdSaveFile.RestoreDirectory = true;

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveFile.FileName = FileName;
                            }
                            
                            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveFile.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("ExportSendList", "导出发送列表"));

                                    if (SaveSendList(FilePath, siList, EncryptPassword.DoEncrypt, EncryptPassword.Password))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportSendList.Success", "导出发送列表成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SaveSendList_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportSendList.Error", "导出发送列表失败");
                                        string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveSendList_Dialog), ex.Message);
                    }
                }

                private static bool SaveSendList(string FilePath, List<SendInfo> siList, bool DoEncrypt, string Password)
                {
                    try
                    {
                        XDocument xdoc = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };

                        XElement xeRoot = SendConfig.List.GetSendList_XML(siList);
                        if (xeRoot == null)
                        {
                            return false;
                        }

                        xdoc.Add(xeRoot);
                        xdoc.Save(FilePath);

                        if (DoEncrypt)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                SystemConfig.EncryptXMLFile(FilePath, Password);
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveSendList), ex.Message);
                    }

                    return false;
                }

                public static XElement GetSendList_XML(List<SendInfo> siList)
                {
                    try
                    {
                        XElement xeRoot = new XElement("SendList");

                        if (siList == null)
                        {
                            siList = Operate.SendConfig.List.lstSendInfo.ToList();
                        }

                        foreach (SendInfo si in siList)
                        {
                            XElement xeSend =
                                new XElement("Send",
                                new XElement("IsEnable", si.IsEnable.ToString()),
                                new XElement("ID", si.SID.ToString().ToUpper()),
                                new XElement("Name", si.SName),
                                new XElement("SystemSocket", si.SSystemSocket.ToString()),
                                new XElement("LoopCNT", si.SLoopCNT),
                                new XElement("LoopINT", si.SLoopINT),
                                new XElement("Notes", si.SNotes)
                                );

                            if (si.SCollection.Count > 0)
                            {
                                XElement xeCollection = new XElement("SendCollection");

                                foreach (PacketInfo pi in si.SCollection)
                                {
                                    string sBuffer = SystemConfig.BytesToString(PacketConfig.Packet.EncodingFormat.Hex, pi.PacketBuffer);

                                    XElement xeColl =
                                        new XElement("Collection",
                                        new XElement("Socket", pi.PacketSocket),
                                        new XElement("Type", pi.PacketType),
                                        new XElement("IPTo", pi.PacketTo),
                                        new XElement("Buffer", sBuffer)
                                        );

                                    xeCollection.Add(xeColl);
                                }

                                xeSend.Add(xeCollection);
                            }

                            xeRoot.Add(xeSend);
                        }

                        return xeRoot;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetSendList_XML), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//从文件加载发送列表（对话框）

                public static void LoadSendList_Dialog(Form form)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();
                        ofdLoadFile.Filter = AntdUI.Localization.Get("SendListFile", "发送列表文件") + "（*.sp）|*.sp";
                        ofdLoadFile.RestoreDirectory = true;

                        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = ofdLoadFile.FileName;
                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                if (LoadSendList(form, FilePath, true))
                                {
                                    string Title = AntdUI.Localization.Get("InjectModeForm.ImportSendList.Success", "导入发送列表成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(nameof(LoadSendList_Dialog), Title + ": " + FilePath);
                                }                    
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadSendList_Dialog), ex.Message);
                    }
                }

                private static bool LoadSendList(Form form, string FilePath, bool LoadFromUser)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            XDocument xdoc = null;

                            bool bEncrypt = SystemConfig.IsEncryptXMLFile(FilePath);
                            if (bEncrypt)
                            {
                                if (LoadFromUser)
                                {
                                    xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("ImportSendList", "导入发送列表"), FilePath);
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("Password.Incorrect", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(nameof(LoadSendList), sError);
                                }

                                return false;
                            }

                            LoadSendList_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadSendList), ex.Message);
                    }

                    return false;
                }

                public static void LoadSendList_FromXDocument(XDocument xdoc)
                {
                    try
                    {
                        foreach (XElement xeSend in xdoc.Root.Elements())
                        {
                            bool IsEnable = false;
                            if (xeSend.Element("IsEnable") != null)
                            {
                                IsEnable = bool.Parse(xeSend.Element("IsEnable").Value);
                            }

                            Guid SID = Guid.Empty;
                            if (xeSend.Element("ID") == null || !Guid.TryParse(xeSend.Element("ID").Value, out SID) || SendConfig.Send.GetSend_ByGuid(SID) != null)
                            {
                                SID = Guid.NewGuid();
                            }

                            string SName = string.Empty;
                            if (xeSend.Element("Name") != null)
                            {
                                SName = xeSend.Element("Name").Value;
                            }

                            bool SSystemSocket = false;
                            if (xeSend.Element("SystemSocket") != null)
                            {
                                SSystemSocket = bool.Parse(xeSend.Element("SystemSocket").Value);
                            }

                            int SLoopCNT = 1;
                            if (xeSend.Element("LoopCNT") != null)
                            {
                                SLoopCNT = int.Parse(xeSend.Element("LoopCNT").Value);
                            }

                            int SLoopINT = 1000;
                            if (xeSend.Element("LoopINT") != null)
                            {
                                SLoopINT = int.Parse(xeSend.Element("LoopINT").Value);
                            }

                            string SNotes = string.Empty;
                            if (xeSend.Element("Notes") != null)
                            {
                                SNotes = xeSend.Element("Notes").Value;
                            }

                            BindingList<PacketInfo> SCollection = new BindingList<PacketInfo>();

                            if (xeSend.Element("SendCollection") != null)
                            {
                                foreach (XElement xeCollection in xeSend.Element("SendCollection").Elements())
                                {
                                    int iSocket = 0;
                                    if (xeCollection.Element("Socket") != null)
                                    {
                                        iSocket = int.Parse(xeCollection.Element("Socket").Value);
                                    }

                                    PacketConfig.Packet.PacketType ptType = new PacketConfig.Packet.PacketType();
                                    if (xeCollection.Element("Type") != null)
                                    {
                                        ptType = PacketConfig.Packet.GetPacketType_ByString(xeCollection.Element("Type").Value);
                                    }

                                    string sIPFrom = string.Empty;
                                    if (xeCollection.Element("IPFrom") != null)
                                    {
                                        sIPFrom = xeCollection.Element("IPFrom").Value;
                                    }

                                    string sIPTo = string.Empty;
                                    if (xeCollection.Element("IPTo") != null)
                                    {
                                        sIPTo = xeCollection.Element("IPTo").Value;
                                    }

                                    byte[] bBuffer = null;
                                    if (xeCollection.Element("Buffer") != null)
                                    {
                                        bBuffer = SystemConfig.StringToBytes(PacketConfig.Packet.EncodingFormat.Hex, xeCollection.Element("Buffer").Value);
                                    }

                                    Send.AddSendCollection(SCollection, iSocket, ptType, sIPFrom, sIPTo, bBuffer);
                                }
                            }

                            Send.AddSend(IsEnable, SID, SName, SSystemSocket, SLoopCNT, SLoopINT, SCollection, SNotes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadSendList_FromXDocument), ex.Message);
                    }
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region//机器人配置

        public static class RobotConfig
        {
            #region//机器人

            public static class Robot
            {
                #region//结构定义

                public enum KeyBoardType
                {
                    Press = 0,
                    Down = 1,
                    Up = 2,
                    Combine = 3,
                    Text = 4,
                }

                public enum MouseType
                {
                    LeftClick = 0,
                    RightClick = 1,
                    LeftDBClick = 2,
                    RightDBClick = 3,
                    LeftDown = 4,
                    LeftUp = 5,
                    RightDown = 6,
                    RightUp = 7,
                    WheelUp = 8,
                    WheelDown = 9,
                    MoveTo = 10,
                    MoveBy = 11,
                }

                public enum InstructionType
                {
                    SendSendList = 0,
                    Delay = 1,
                    LoopStart = 2,
                    LoopEnd = 3,
                    KeyBoard = 4,
                    Mouse = 5,
                    SendPacketList = 6,
                    SetSystemSocket = 7,
                    Switch = 8,
                }

                #endregion

                #region//新增机器人

                public static void AddRobot_New()
                {
                    try
                    {
                        bool IsEnable = false;
                        Guid RID = Guid.NewGuid();
                        int RNum = RobotConfig.List.lstRobotInfo.Count + 1;
                        string RName = string.Format(AntdUI.Localization.Get("RobotList.NewRobot", "机器人 {0}"), RNum.ToString());
                        BindingList<InstructionInfo> RInstruction = new BindingList<InstructionInfo>();

                        AddRobot(IsEnable, RID, RName, RInstruction);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddRobot_New), ex.Message);
                    }
                }

                public static void AddRobot(bool IsEnable, Guid RID, string RName, BindingList<InstructionInfo> RInstructions)
                {
                    try
                    {
                        if (RID != Guid.Empty && !string.IsNullOrEmpty(RName))
                        {
                            RobotInfo ri = new RobotInfo(IsEnable, RID, RName, RInstructions);
                            RobotConfig.List.RobotToList(ri);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddRobot), ex.Message);
                    }
                }

                #endregion

                #region//更新机器人

                public static void UpdateRobot(RobotInfo sri, string RName, BindingList<InstructionInfo> RInstruction)
                {
                    try
                    {
                        if (sri != null)
                        {
                            sri.RName = RName;
                            sri.RInstruction = new BindingList<InstructionInfo>(RInstruction.ToList());
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateRobot), ex.Message);
                    }
                }

                #endregion

                #region//复制机器人

                public static void CopyRobot(RobotInfo ri)
                {
                    try
                    {
                        bool IsEnable = false;
                        Guid RID_New = Guid.NewGuid();
                        string RName_Copy = string.Format(AntdUI.Localization.Get("CopyName", "{0} - 副本"), ri.RName);                        
                        BindingList<InstructionInfo> RInstruction_Copy = new BindingList<InstructionInfo>(ri.RInstruction.ToList());

                        Robot.AddRobot(IsEnable, RID_New, RName_Copy, RInstruction_Copy);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CopyRobot), ex.Message);
                    }
                }

                #endregion

                #region//删除机器人（对话框）

                public static void DeleteRobot_Dialog(Form form, List<RobotInfo> riList)
                {
                    try
                    {
                        if (riList.Count > 0)
                        {
                            AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miRobotList", "机器人列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                            {
                                Icon = TType.Warn,
                                Keyboard = false,
                                MaskClosable = false,
                                OnOk = config =>
                                {
                                    foreach (RobotInfo ri in riList)
                                    {
                                        RobotConfig.List.lstRobotInfo.Remove(ri);
                                    }

                                    return true;
                                }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DeleteRobot_Dialog), ex.Message);
                    }
                }

                #endregion

                #region//编辑机器人

                public static void OpenRobotEdit(Form form, RobotInfo ri)
                {
                    var RobotEdit = new RobotEdit(form, ri);
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("RobotEditForm", "机器人编辑"), RobotEdit)
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });
                }

                #endregion

                #region//获取机器人

                public static RobotInfo GetRobot_ByGuid(Guid RID)
                {
                    try
                    {
                        if (RID != null && RID != Guid.Empty)
                        {
                            foreach (RobotInfo ri in RobotConfig.List.lstRobotInfo)
                            {
                                if (ri.RID == RID)
                                {
                                    return ri;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetRobot_ByGuid), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//设置机器人是否启用

                public static void SetIsEnable_ByGUID(Guid RID, bool IsEnable)
                {
                    try
                    {
                        RobotInfo ri = Operate.RobotConfig.Robot.GetRobot_ByGuid(RID);
                        if (ri != null)
                        {
                            ri.IsEnable = IsEnable;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SetIsEnable_ByGUID), ex.Message);
                    }
                }

                #endregion

                #region//获取指令类型的名称

                public static string GetName_ByInstructionType(Robot.InstructionType instructionType)
                {
                    string sReturn = string.Empty;

                    try
                    {
                        switch (instructionType)
                        {
                            case Robot.InstructionType.SendSendList:
                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.Send", "发送");
                                break;

                            case Robot.InstructionType.SendPacketList:
                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.Send", "发送");
                                break;

                            case Robot.InstructionType.SetSystemSocket:
                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.Set", "设置");
                                break;

                            case Robot.InstructionType.Delay:
                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.Delay", "延迟");
                                break;

                            case Robot.InstructionType.LoopStart:
                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.LoopBegin", "循环开始");
                                break;

                            case Robot.InstructionType.LoopEnd:
                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.LoopEnd", "循环结束");
                                break;

                            case Robot.InstructionType.Switch:
                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.Switch", "开关");
                                break;

                            case Robot.InstructionType.KeyBoard:
                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.KeyBoard", "键盘");
                                break;

                            case Robot.InstructionType.Mouse:
                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.Mouse", "鼠标");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetName_ByInstructionType), ex.Message);
                    }

                    return sReturn;
                }

                #endregion

                #region//获取指令类型的颜色

                public static Color GetColor_ByInstructionType(Robot.InstructionType instructionType)
                {
                    Color cReturn = Color.White;

                    try
                    {
                        switch (instructionType)
                        {
                            case Robot.InstructionType.SendSendList:
                                cReturn = Color.YellowGreen;
                                break;

                            case Robot.InstructionType.SendPacketList:
                                cReturn = Color.YellowGreen;
                                break;

                            case Robot.InstructionType.SetSystemSocket:
                                cReturn = Color.Violet;
                                break;

                            case Robot.InstructionType.Delay:
                                cReturn = Color.Khaki;
                                break;

                            case Robot.InstructionType.LoopStart:
                                cReturn = Color.Orchid;
                                break;

                            case Robot.InstructionType.LoopEnd:
                                cReturn = Color.Orchid;
                                break;

                            case Robot.InstructionType.Switch:
                                cReturn = Color.DarkOrange;
                                break;

                            case Robot.InstructionType.KeyBoard:
                                cReturn = Color.LightSeaGreen;
                                break;

                            case Robot.InstructionType.Mouse:
                                cReturn = Color.LightSkyBlue;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetColor_ByInstructionType), ex.Message);
                    }

                    return cReturn;
                }

                #endregion

                #region//获取指令内容的字符串

                public static string GetContentString_ByInstructionType(Robot.InstructionType instructionType, string sContent)
                {
                    string sReturn = string.Empty;

                    try
                    {
                        switch (instructionType)
                        {
                            case Robot.InstructionType.SendSendList:

                                if (!string.IsNullOrEmpty(sContent))
                                {
                                    Guid SID = Guid.Parse(sContent);
                                    string SName = SendConfig.Send.GetSendName_ByGuid(SID);

                                    sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.Send.SendList", "发送列表 - [{0}]"), SName);
                                }

                                break;

                            case Robot.InstructionType.SendPacketList:

                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.PacketList.Select", "[封包列表] 选中的封包");

                                break;

                            case Robot.InstructionType.SetSystemSocket:

                                if (sContent.Equals("PacketConfig.List"))
                                {
                                    sReturn = AntdUI.Localization.Get("RobotEditForm.INST.Socket.SelectPacket", "系统套接字 = 选中封包的套接字");
                                }
                                else if (sContent.Equals("FilterSocket"))
                                {
                                    sReturn = AntdUI.Localization.Get("RobotEditForm.INST.Socket.CallFilter", "系统套接字 = 调用滤镜的套接字");
                                }
                                else if (sContent.Contains("Customize") && sContent.Contains("|"))
                                {
                                    string sSocket = sContent.Split('|')[1];
                                    sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.Socket.Customize", "系统套接字 = {0}"), sSocket);
                                }

                                break;

                            case Robot.InstructionType.Delay:

                                if (!string.IsNullOrEmpty(sContent))
                                {
                                    sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.Socket.Millisecond", "{0} 毫秒"), sContent);
                                }

                                break;

                            case Robot.InstructionType.LoopStart:

                                if (!string.IsNullOrEmpty(sContent))
                                {
                                    sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.Loop.Begin", "循环 {0} 次"), sContent);
                                }

                                break;

                            case Robot.InstructionType.LoopEnd:

                                sReturn = AntdUI.Localization.Get("RobotEditForm.INST.Loop.End", "循环结束");

                                break;

                            case Robot.InstructionType.Switch:

                                if (!string.IsNullOrEmpty(sContent))
                                {
                                    if (sContent.Contains("|"))
                                    {
                                        string[] slSwitch = sContent.Split('|');
                                        if (slSwitch.Length == 3)
                                        {
                                            if (Guid.TryParse(slSwitch[2], out Guid GID))
                                            {
                                                string Switch = string.Empty;
                                                switch (slSwitch[0])
                                                {
                                                    case "Enable":
                                                        Switch = AntdUI.Localization.Get("Enable", "启用");
                                                        break;

                                                    case "Disable":
                                                        Switch = AntdUI.Localization.Get("Disable", "禁用");
                                                        break;
                                                }

                                                string SwitchType = string.Empty;
                                                string SwitchInfo = string.Empty;
                                                switch (slSwitch[1])
                                                {
                                                    case "SendList":
                                                        SwitchType = AntdUI.Localization.Get("SendList", "发送列表");
                                                        SwitchInfo = SendConfig.Send.GetSend_ByGuid(GID).SName;                                                        
                                                        break;

                                                    case "RobotList":
                                                        SwitchType = AntdUI.Localization.Get("RobotList", "机器人列表");
                                                        SwitchInfo = RobotConfig.Robot.GetRobot_ByGuid(GID).RName;
                                                        break;

                                                    case "FilterList":
                                                        SwitchType = AntdUI.Localization.Get("FilterList", "滤镜列表");
                                                        SwitchInfo = FilterConfig.Filter.GetFilter_ByGuid(GID).FName;
                                                        break;
                                                }

                                                sReturn = string.Format("{0} - {1} [ {2} ]", Switch, SwitchType, SwitchInfo);
                                            }
                                        }
                                    }                                    
                                }

                                break;

                            case Robot.InstructionType.KeyBoard:

                                if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                                {
                                    Robot.KeyBoardType kbType = Robot.GetKeyBoardType_ByString(sContent.Split('|')[0].ToString());
                                    string KeyCode = sContent.Split('|')[1];

                                    switch (kbType)
                                    {
                                        case Robot.KeyBoardType.Press:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.KeyPress", "按键 {0}"), KeyCode);
                                            break;

                                        case Robot.KeyBoardType.Down:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.KeyDown", "按下 {0}"), KeyCode);
                                            break;

                                        case Robot.KeyBoardType.Up:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.KeyUp", "弹起 {0}"), KeyCode);
                                            break;

                                        case Robot.KeyBoardType.Combine:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.KeyCombine", "组合按键 {0}"), KeyCode);
                                            break;

                                        case Robot.KeyBoardType.Text:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.KeyText", "输入文本 {0}"), KeyCode);
                                            break;
                                    }
                                }

                                break;

                            case Robot.InstructionType.Mouse:

                                if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                                {
                                    Robot.MouseType mType = Robot.GetMouseType_ByString(sContent.Split('|')[0].ToString());
                                    string MouseCode = sContent.Split('|')[1];

                                    switch (mType)
                                    {
                                        case Robot.MouseType.LeftClick:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.INST.LeftClick", "左键单击");
                                            break;

                                        case Robot.MouseType.RightClick:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.INST.RightClick", "右键单击");
                                            break;

                                        case Robot.MouseType.LeftDBClick:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.INST.LeftDBClick", "左键双击");
                                            break;

                                        case Robot.MouseType.RightDBClick:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.INST.RightDBClick", "右键双击");
                                            break;

                                        case Robot.MouseType.LeftDown:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.INST.LeftDown", "左键按下");
                                            break;

                                        case Robot.MouseType.LeftUp:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.INST.LeftUp", "左键弹起");
                                            break;

                                        case Robot.MouseType.RightDown:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.INST.RightDown", "右键按下");
                                            break;

                                        case Robot.MouseType.RightUp:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.INST.RightUp", "右键弹起");
                                            break;

                                        case Robot.MouseType.WheelUp:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.WheelUp", "向上滚动 {0}"), MouseCode);
                                            break;

                                        case Robot.MouseType.WheelDown:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.WheelDown", "向下滚动 {0}"), MouseCode);
                                            break;

                                        case Robot.MouseType.MoveTo:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.MoveTo", "移动到 ( {0} )"), MouseCode);
                                            break;

                                        case Robot.MouseType.MoveBy:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.INST.MoveBy", "相对移动 ( {0} )"), MouseCode);
                                            break;
                                    }
                                }

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetContentString_ByInstructionType), ex.Message);
                    }

                    return sReturn;
                }

                #endregion

                #region//获取指令类型

                public static Robot.InstructionType GetInstructionType_ByString(string InstructionType)
                {
                    Robot.InstructionType instructionType = new InstructionType();

                    try
                    {
                        instructionType = (Robot.InstructionType)Enum.Parse(typeof(Robot.InstructionType), InstructionType);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetInstructionType_ByString), ex.Message);
                    }

                    return instructionType;
                }

                #endregion

                #region//获取键盘按键类型

                public static Robot.KeyBoardType GetKeyBoardType_ByString(string KeyBoardType)
                {
                    Robot.KeyBoardType kbType = new Robot.KeyBoardType();

                    try
                    {
                        kbType = (Robot.KeyBoardType)Enum.Parse(typeof(Robot.KeyBoardType), KeyBoardType);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetKeyBoardType_ByString), ex.Message);
                    }

                    return kbType;
                }

                #endregion

                #region//获取鼠标按键类型

                public static Robot.MouseType GetMouseType_ByString(string MouseType)
                {
                    Robot.MouseType mType = new Robot.MouseType();

                    try
                    {
                        mType = (Robot.MouseType)Enum.Parse(typeof(Robot.MouseType), MouseType);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetMouseType_ByString), ex.Message);
                    }

                    return mType;
                }

                #endregion

                #region//新增指令集

                public static void AddRobotInstruction(
                    BindingList<InstructionInfo> RInstruction, 
                    RobotConfig.Robot.InstructionType instructionType, 
                    string InstContent)
                {
                    try
                    {
                        InstructionInfo ii = new InstructionInfo(instructionType, InstContent);
                        RInstruction.Add(ii);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(AddRobotInstruction), ex.Message);
                    }
                }

                #endregion

                #region//获取指令集的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_RobotInstruction()
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                    menuItems.Add(new AntdUI.ContextMenuStripItem("置顶", "Ctrl+⬆")
                    {
                        ID = "Top",
                        IconSvg = "VerticalAlignTopOutlined",
                        LocalizationText = "Top",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("向上移动", "Alt+⬆")
                    {
                        ID = "Up",
                        IconSvg = "ArrowUpOutlined",
                        LocalizationText = "Up",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItem("向下移动", "Alt+⬇")
                    {
                        ID = "Down",
                        IconSvg = "ArrowDownOutlined",
                        LocalizationText = "Down",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("置底", "Ctrl+⬇")
                    {
                        ID = "Bottom",
                        IconSvg = "VerticalAlignBottomOutlined",
                        LocalizationText = "Bottom",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("删除")
                    {
                        ID = "Delete",
                        IconSvg = "CloseOutlined",
                        LocalizationText = "Delete",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("清空所有指令")
                    {
                        ID = "ClearUp",
                        IconSvg = "DeleteOutlined",
                        LocalizationText = "Clear",
                    });

                    return menuItems.ToArray();
                }

                #endregion

                #region//指令集的列表操作

                public static void UpdateInstruction_ByListAction(
                    Form form, 
                    Operate.SystemConfig.ListAction listAction, 
                    BindingList<InstructionInfo> RInstruction,
                    List<InstructionInfo> iiList)
                {
                    try
                    {
                        switch (listAction)
                        {
                            case SystemConfig.ListAction.Top:

                                foreach (InstructionInfo ii in iiList)
                                {
                                    RInstruction.Remove(ii);
                                    RInstruction.Insert(0, ii);
                                }

                                break;

                            case SystemConfig.ListAction.Up:

                                foreach (InstructionInfo ii in iiList)
                                {
                                    int iIndex = RInstruction.IndexOf(ii);
                                    if (iIndex > 0)
                                    {
                                        RInstruction.Remove(ii);
                                        RInstruction.Insert(iIndex - 1, ii);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Down:

                                foreach (InstructionInfo ii in iiList)
                                {
                                    int iIndex = RInstruction.IndexOf(ii);
                                    if (iIndex > -1 && iIndex < RInstruction.Count - 1)
                                    {
                                        RInstruction.Remove(ii);
                                        RInstruction.Insert(iIndex + 1, ii);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Bottom:

                                foreach (InstructionInfo ii in iiList)
                                {
                                    RInstruction.Remove(ii);
                                    RInstruction.Add(ii);
                                }

                                break;                            

                            case SystemConfig.ListAction.Delete:

                                foreach (InstructionInfo ii in iiList)
                                {
                                    RInstruction.Remove(ii);
                                }

                                break;

                            case SystemConfig.ListAction.CleanUp:

                                if (RInstruction.Count > 0)
                                {
                                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miRobotInstruction", "指令集列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                                    {
                                        Icon = TType.Warn,
                                        Keyboard = false,
                                        MaskClosable = false,
                                        OnOk = config =>
                                        {
                                            RInstruction.Clear();
                                            return true;
                                        }
                                    });
                                }

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateInstruction_ByListAction), ex.Message);
                    }
                }

                #endregion

                #region//检查指令集

                public static int CheckRobotInstruction(Form form, BindingList<InstructionInfo> RInstruction)
                {
                    int iReturn = -1;

                    try
                    {
                        if (RInstruction != null && RInstruction.Count > 0)
                        {
                            List<int> listSendSendList = new List<int>();
                            List<int> listLoopStart = new List<int>();
                            List<int> listLoopEnd = new List<int>();

                            for (int i = 0; i < RInstruction.Count; i++)
                            {
                                switch (RInstruction[i].InstType)
                                {
                                    case Robot.InstructionType.SendSendList:
                                        listSendSendList.Add(i);
                                        break;

                                    case Robot.InstructionType.LoopStart:
                                        listLoopStart.Add(i);
                                        break;

                                    case Robot.InstructionType.LoopEnd:
                                        listLoopEnd.Add(i);
                                        break;
                                }
                            }

                            #region//检测发送指令

                            foreach (int iSendIndex in listSendSendList)
                            {
                                string sSendContent = RInstruction[iSendIndex].InstContent;
                                if (!string.IsNullOrEmpty(sSendContent))
                                {
                                    Guid SID = Guid.Parse(sSendContent);
                                    string SName = SendConfig.Send.GetSendName_ByGuid(SID);

                                    if (string.IsNullOrEmpty(SName))
                                    {
                                        if (form != null)
                                        {
                                            AntdUI.Message.open(new AntdUI.Message.Config(form, "发送列表不正确", TType.Error)
                                            {
                                                LocalizationText = "RobotEditForm.SendList.Error"
                                            });
                                        }

                                        return iSendIndex;
                                    }
                                }
                            }

                            #endregion

                            #region//检测循环指令

                            if (listLoopStart.Count != listLoopEnd.Count)
                            {
                                int iErrorIndex = 0;
                                if (listLoopStart.Count > 0)
                                {
                                    iErrorIndex = listLoopStart[0];
                                }
                                else if (listLoopEnd.Count > 0)
                                {
                                    iErrorIndex = listLoopEnd[0];
                                }

                                if (form != null)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, "循环指令不正确", TType.Error)
                                    {
                                        LocalizationText = "RobotEditForm.LoopINST.Error"
                                    });
                                }

                                return iErrorIndex;
                            }

                            for (int i = 0; i < listLoopStart.Count; i++)
                            {
                                int iLoopStartIndex = listLoopStart[i];
                                int iLoopEndIndex = listLoopEnd[i];

                                if (iLoopStartIndex >= iLoopEndIndex)
                                {
                                    if (form != null)
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(form, "循环指令不正确", TType.Error)
                                        {
                                            LocalizationText = "RobotEditForm.LoopINST.Error"
                                        });
                                    }

                                    return iLoopEndIndex;
                                }
                            }

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(CheckRobotInstruction), ex.Message);
                    }

                    return iReturn;
                }

                #endregion

                #region//执行机器人            

                public static RobotExecute DoRobot(Guid RID, Dictionary<string, object> parameters)
                {
                    return Task.Run(() => DoRobotAsync(RID, parameters))
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();
                }

                public static async Task DoRobot_ByIndex(int RobotListIndex)
                {
                    try
                    {
                        if (RobotListIndex > -1 && RobotListIndex < RobotConfig.List.lstRobotInfo.Count)
                        {
                            if (RobotConfig.List.lstRobotInfo[RobotListIndex].IsEnable)
                            {
                                Guid RID = RobotConfig.List.lstRobotInfo[RobotListIndex].RID;
                                Operate.RobotConfig.List.lstRobotExecute.Add(await DoRobotAsync(RID, null));                                
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DoRobot_ByIndex), ex.Message);
                    }
                }

                private static async Task<RobotExecute> DoRobotAsync(Guid RID, Dictionary<string, object> parameters)
                {
                    RobotExecute reReturn = null;

                    try
                    {
                        if (RID != Guid.Empty)
                        {
                            RobotInfo ri = RobotConfig.List.lstRobotInfo.Where(item => item.RID == RID).FirstOrDefault();

                            if (ri != null)
                            {
                                if (ri.IsEnable)
                                {
                                    if (ri.RInstruction.Count > 0)
                                    {
                                        reReturn = new RobotExecute();
                                        await Task.Run(() => reReturn.StartRobot(ri, parameters));
                                    }
                                }                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(DoRobotAsync), ex.Message);
                    }

                    return reReturn;
                }

                #endregion
            }

            #endregion

            #region//机器人列表

            public static class List
            {
                public static List<RobotExecute> lstRobotExecute = new List<RobotExecute>();
                public static BindingList<RobotInfo> lstRobotInfo = new BindingList<RobotInfo>();
                public static BackgroundWorker bgwRobotList = new BackgroundWorker();

                #region//机器人入列表

                public static void RobotToList(RobotInfo ri)
                {
                    try
                    {
                        RobotConfig.List.lstRobotInfo.Add(ri);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(RobotToList), ex.Message);
                    }
                }

                #endregion

                #region//执行机器人列表

                public static void StartRobotList()
                {
                    try
                    {
                        if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
                        {
                            if (!Operate.RobotConfig.List.bgwRobotList.IsBusy)
                            {
                                Operate.RobotConfig.List.lstRobotExecute.Clear();
                                Operate.RobotConfig.List.bgwRobotList.RunWorkerAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(StartRobotList), ex.Message);
                    }
                }

                public static void StopRobotList()
                {
                    try
                    {
                        if (Operate.RobotConfig.List.bgwRobotList.IsBusy)
                        {
                            Operate.RobotConfig.List.bgwRobotList.CancelAsync();
                        }

                        foreach (RobotExecute re in Operate.RobotConfig.List.lstRobotExecute.ToList())
                        {
                            if (re.Worker.IsBusy)
                            {
                                re.StopRobot();
                            }

                            Operate.RobotConfig.List.lstRobotExecute.Remove(re);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(StopRobotList), ex.Message);
                    }
                }

                public static void RobotList_DoWork(object sender, DoWorkEventArgs e)
                {
                    try
                    {
                        foreach (RobotInfo ri in Operate.RobotConfig.List.lstRobotInfo)
                        {
                            if (ri.IsEnable)
                            {
                                RobotExecute re = Operate.RobotConfig.Robot.DoRobot(ri.RID, null);
                                if (re != null)
                                {
                                    if (Operate.SystemConfig.ListExecute == Operate.SystemConfig.Execute.Together)
                                    {
                                        Operate.RobotConfig.List.lstRobotExecute.Add(re);
                                    }
                                    else
                                    {
                                        while (re.Worker.IsBusy)
                                        {
                                            if (bgwRobotList.CancellationPending)
                                            {
                                                re.StopRobot();

                                                e.Cancel = true;
                                                return;
                                            }

                                            Thread.Sleep(100);
                                        }
                                    }
                                }
                            }
                        }

                        while (Operate.RobotConfig.List.lstRobotExecute.Count > 0)
                        {
                            foreach (RobotExecute re in Operate.RobotConfig.List.lstRobotExecute.ToList())
                            {
                                if (bgwRobotList.CancellationPending)
                                {
                                    re.StopRobot();
                                }

                                if (!re.Worker.IsBusy)
                                {
                                    Operate.RobotConfig.List.lstRobotExecute.Remove(re);
                                }
                            }

                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(RobotList_DoWork), ex.Message);
                    }
                }

                #endregion

                #region//初始化机器人列表的计数

                public static void InitRobotList_Count()
                {
                    try
                    {
                        foreach (RobotInfo ri in lstRobotInfo)
                        {
                            ri.ExecutionCount = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(InitRobotList_Count), ex.Message);
                    }
                }

                #endregion

                #region//清空机器人列表（对话框）

                public static void CleanUpRobotList_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miRobotList", "机器人列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                    {
                        Icon = TType.Warn,
                        Keyboard = false,
                        MaskClosable = false,
                        OnOk = config =>
                        {
                            RobotConfig.List.RobotListClear();
                            return true;
                        }
                    });
                }

                public static void RobotListClear()
                {
                    RobotConfig.List.lstRobotInfo.Clear();
                }

                #endregion

                #region//机器人列表的列表操作

                public static void UpdateRobotList_ByListAction(Form form, SystemConfig.ListAction listAction, List<RobotInfo> riList)
                {
                    try
                    {
                        switch (listAction)
                        {
                            case SystemConfig.ListAction.Top:

                                foreach (RobotInfo ri in riList)
                                {
                                    RobotConfig.List.lstRobotInfo.Remove(ri);
                                    RobotConfig.List.lstRobotInfo.Insert(0, ri);
                                }

                                break;

                            case SystemConfig.ListAction.Up:

                                foreach (RobotInfo ri in riList)
                                {
                                    int iIndex = RobotConfig.List.lstRobotInfo.IndexOf(ri);
                                    if (iIndex > 0)
                                    {
                                        RobotConfig.List.lstRobotInfo.Remove(ri);
                                        RobotConfig.List.lstRobotInfo.Insert(iIndex - 1, ri);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Down:

                                foreach (RobotInfo ri in riList)
                                {
                                    int iIndex = RobotConfig.List.lstRobotInfo.IndexOf(ri);
                                    if (iIndex > -1 && iIndex < RobotConfig.List.lstRobotInfo.Count - 1)
                                    {
                                        RobotConfig.List.lstRobotInfo.Remove(ri);
                                        RobotConfig.List.lstRobotInfo.Insert(iIndex + 1, ri);
                                    }
                                }

                                break;

                            case SystemConfig.ListAction.Bottom:

                                foreach (RobotInfo ri in riList)
                                {
                                    RobotConfig.List.lstRobotInfo.Remove(ri);
                                    RobotConfig.List.lstRobotInfo.Add(ri);
                                }

                                break;

                            case SystemConfig.ListAction.Copy:

                                foreach (RobotInfo ri in riList)
                                {
                                    Robot.CopyRobot(ri);
                                }

                                break;

                            case SystemConfig.ListAction.Export:

                                string sRName = riList[0].RName;
                                RobotConfig.List.SaveRobotList_Dialog(form, sRName, riList);

                                break;

                            case SystemConfig.ListAction.Delete:

                                Robot.DeleteRobot_Dialog(form, riList);

                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(UpdateRobotList_ByListAction), ex.Message);
                    }
                }

                #endregion

                #region//保存机器人列表到数据库

                public static void SaveRobotList_ToDB()
                {
                    try
                    {
                        DataBase.DeleteTable_Robot();

                        foreach (RobotInfo sri in RobotConfig.List.lstRobotInfo)
                        {
                            DataBase.InsertTable_Robot(sri);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveRobotList_ToDB), ex.Message);
                    }
                }

                #endregion

                #region//从数据库加载机器人列表

                public static void LoadRobotList_FromDB()
                {
                    try
                    {
                        DataTable dtRobot = DataBase.SelectTable_Robot();

                        foreach (DataRow dataRow in dtRobot.Rows)
                        {
                            Guid RID = Guid.Parse(dataRow["GUID"].ToString());
                            bool IsEnable = Convert.ToBoolean(dataRow["IsEnable"]);
                            string RName = dataRow["Name"].ToString();
                            BindingList<InstructionInfo> RInstruction = new BindingList<InstructionInfo>();

                            DataTable dtRInstruction = DataBase.SelectTable_RobotInstruction(RID);
                            foreach (DataRow row in dtRInstruction.Rows)
                            {
                                RobotConfig.Robot.InstructionType instructionType = RobotConfig.Robot.GetInstructionType_ByString(row["Type"].ToString());
                                string instructionContent = row["Content"].ToString();

                                RobotConfig.Robot.AddRobotInstruction(RInstruction, instructionType, instructionContent);
                            }

                            RobotConfig.Robot.AddRobot(IsEnable, RID, RName, RInstruction);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadRobotList_FromDB), ex.Message);
                    }
                }

                #endregion

                #region//保存机器人列表到文件（对话框）

                public static void SaveRobotList_Dialog(Form form, string FileName, List<RobotInfo> riList)
                {
                    try
                    {
                        if (RobotConfig.List.lstRobotInfo.Count > 0)
                        {
                            SaveFileDialog sfdSaveFile = new SaveFileDialog();
                            sfdSaveFile.Filter = AntdUI.Localization.Get("RobotListFile", "机器人列表文件") + "（*.rp）|*.rp";

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveFile.FileName = FileName;
                            }

                            sfdSaveFile.RestoreDirectory = true;

                            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveFile.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    var EncryptPassword = SystemConfig.GetEncryptExport(form, AntdUI.Localization.Get("ExportRobotList", "导出机器人列表"));

                                    if (SaveRobotList(FilePath, riList, EncryptPassword.DoEncrypt, EncryptPassword.Password))
                                    {
                                        string Title = AntdUI.Localization.Get("ExportRobotList.Success", "导出机器人列表成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(nameof(SaveRobotList_Dialog), Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("ExportRobotList.Error", "导出机器人列表失败");
                                        string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveRobotList_Dialog), ex.Message);
                    }
                }

                public static bool SaveRobotList(string FilePath, List<RobotInfo> riList, bool DoEncrypt, string Password)
                {
                    try
                    {
                        XDocument xdoc = new XDocument
                        {
                            Declaration = new XDeclaration("1.0", "utf-8", "yes")
                        };

                        XElement xeRoot = RobotConfig.List.GetRobotList_XML(riList);
                        if (xeRoot == null)
                        {
                            return false;
                        }

                        xdoc.Add(xeRoot);
                        xdoc.Save(FilePath);

                        if (DoEncrypt)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                SystemConfig.EncryptXMLFile(FilePath, Password);
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveRobotList), ex.Message);
                    }

                    return false;
                }

                public static XElement GetRobotList_XML(List<RobotInfo> riList)
                {
                    try
                    {
                        XElement xeRoot = new XElement("RobotList");

                        if (riList == null)
                        {
                            riList = Operate.RobotConfig.List.lstRobotInfo.ToList();
                        }

                        foreach (RobotInfo ri in riList)
                        {
                            XElement xeRobot =
                                new XElement("Robot",
                                new XElement("IsEnable", ri.IsEnable.ToString()),
                                new XElement("ID", ri.RID.ToString().ToUpper()),
                                new XElement("Name", ri.RName)
                                );

                            if (ri.RInstruction.Count > 0)
                            {
                                XElement xeInstruction = new XElement("Instructions");

                                foreach (InstructionInfo ii in ri.RInstruction)
                                {
                                    XElement xeInst = 
                                        new XElement("Inst", 
                                        new XAttribute("Type", ii.InstType), ii.InstContent);

                                    xeInstruction.Add(xeInst);
                                }

                                xeRobot.Add(xeInstruction);
                            }

                            xeRoot.Add(xeRobot);
                        }

                        return xeRoot;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(GetRobotList_XML), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//从文件加载机器人列表（对话框）

                public static void LoadRobotList_Dialog(Form form)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();

                        ofdLoadFile.Filter = AntdUI.Localization.Get("RobotListFile", "机器人列表文件") + "（*.rp）|*.rp";
                        ofdLoadFile.RestoreDirectory = true;

                        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                        {
                            string FilePath = ofdLoadFile.FileName;

                            if (!string.IsNullOrEmpty(FilePath))
                            {
                                if (LoadRobotList(form, FilePath, true))
                                {
                                    string Title = AntdUI.Localization.Get("InjectModeForm.ImportRobotList.Success", "导入机器人列表成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(nameof(LoadRobotList_Dialog), Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadRobotList_Dialog), ex.Message);
                    }
                }

                private static bool LoadRobotList(Form form, string FilePath, bool LoadFromUser)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            XDocument xdoc = null;

                            bool bEncrypt = SystemConfig.IsEncryptXMLFile(FilePath);
                            if (bEncrypt)
                            {
                                if (LoadFromUser)
                                {
                                    xdoc = SystemConfig.GetEncryptImport(form, AntdUI.Localization.Get("ImportRobotList", "导入机器人列表"), FilePath);
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("Password.Incorrect", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(nameof(LoadRobotList), sError);
                                }

                                return false;
                            }

                            LoadRobotList_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadRobotList), ex.Message);
                    }

                    return false;
                }

                public static void LoadRobotList_FromXDocument(XDocument xdoc)
                {
                    try
                    {
                        foreach (XElement xeRobot in xdoc.Root.Elements())
                        {
                            bool IsEnable = false;
                            if (xeRobot.Element("IsEnable") != null)
                            {
                                IsEnable = bool.Parse(xeRobot.Element("IsEnable").Value);
                            }

                            Guid RID = Guid.Empty;
                            if (xeRobot.Element("ID") == null || !Guid.TryParse(xeRobot.Element("ID").Value, out RID) || RobotConfig.Robot.GetRobot_ByGuid(RID) != null)
                            {
                                RID = Guid.NewGuid();
                            }

                            string RName = string.Empty;
                            if (xeRobot.Element("Name") != null)
                            {
                                RName = xeRobot.Element("Name").Value;
                            }
                            
                            BindingList<InstructionInfo> RInstruction = new BindingList<InstructionInfo>();

                            if (xeRobot.Element("Instructions") != null)
                            {
                                foreach (XElement xeInstruction in xeRobot.Element("Instructions").Elements())
                                {
                                    RobotConfig.Robot.InstructionType instructionType = RobotConfig.Robot.GetInstructionType_ByString(xeInstruction.Attribute("Type").Value);                                    
                                    RobotConfig.Robot.AddRobotInstruction(RInstruction, instructionType, xeInstruction.Value);                                    
                                }
                            }

                            Robot.AddRobot(IsEnable, RID, RName, RInstruction);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(LoadRobotList_FromXDocument), ex.Message);
                    }
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region//日志配置

        public static class LogConfig
        {
            public const string ProxyLogString = "CONNECT {0} outgoing via {1} SOCKS5";

            #region//日志队列

            public static class Queue
            {
                public static ConcurrentQueue<LogInfo> cqLogInfo = new ConcurrentQueue<LogInfo>();
                public static ConcurrentQueue<FilterLogInfo> cqFilterLogInfo = new ConcurrentQueue<FilterLogInfo>();
                public static ConcurrentQueue<ProxyLogInfo> cqProxyLogInfo = new ConcurrentQueue<ProxyLogInfo>();

                #region//日志入队列

                public static async ValueTask LogToQueueAsync(string FuncName, string LogContent)
                {
                    LogInfo li = new LogInfo(FuncName, LogContent);
                    await Task.Run(() => cqLogInfo.Enqueue(li));
                }

                public static async ValueTask FilterLogToQueueAsync(
                    string FName, 
                    Operate.FilterConfig.Filter.FilterAction FAction,
                    int MatchNum,
                    Operate.PacketConfig.Packet.PacketType pType, 
                    int PacketLen)
                {
                    FilterLogInfo fli = new FilterLogInfo(FName, FAction, MatchNum, pType, PacketLen);
                    await Task.Run(() => cqFilterLogInfo.Enqueue(fli));
                }

                public static async ValueTask ProxyLogToQueueAsync(string UserName, string LoginIP, string LogContent)
                {
                    ProxyLogInfo pli = new ProxyLogInfo(UserName, LoginIP, LogContent);
                    await Task.Run(() => cqProxyLogInfo.Enqueue(pli));
                }

                #endregion

                #region//清除日志队列

                public static void ClearLogQueue()
                {
                    while (!cqLogInfo.IsEmpty)
                    {
                        cqLogInfo.TryDequeue(out LogInfo li);
                    }
                }

                public static void ClearFilterLogQueue()
                {
                    while (!cqFilterLogInfo.IsEmpty)
                    {
                        cqFilterLogInfo.TryDequeue(out FilterLogInfo fli);
                    }
                }

                public static void ClearProxyLogQueue()
                {
                    while (!cqProxyLogInfo.IsEmpty)
                    {
                        cqProxyLogInfo.TryDequeue(out ProxyLogInfo pli);
                    }
                }

                #endregion                
            }

            #endregion

            #region//日志列表

            public static class List
            {
                public static bool AutoRoll = false, AutoClear = true;
                public static decimal AutoClear_Value = 5000;
                public static BindingList<LogInfo> lstLogInfo = new BindingList<LogInfo>();
                public static BindingList<FilterLogInfo> lstFilterLogInfo = new BindingList<FilterLogInfo>();
                public static BindingList<ProxyLogInfo> lstProxyLogInfo = new BindingList<ProxyLogInfo>();

                #region//日志入列表

                public static void LogToList()
                {
                    if (Queue.cqLogInfo.TryDequeue(out LogInfo li))
                    {
                        LogConfig.List.lstLogInfo.Add(li);
                    }
                }

                public static void FilterLogToList()
                {
                    if (Queue.cqFilterLogInfo.TryDequeue(out FilterLogInfo fli))
                    {
                        LogConfig.List.lstFilterLogInfo.Add(fli);
                    }
                }

                public static void ProxyLogToList()
                {
                    if (Queue.cqProxyLogInfo.TryDequeue(out ProxyLogInfo pli))
                    {
                        LogConfig.List.lstProxyLogInfo.Add(pli);
                    }
                }

                #endregion

                #region//清除日志列表

                public static void ClearLogList()
                {
                    lstLogInfo.Clear();
                }

                public static void ClearFilterLogList()
                {
                    lstFilterLogInfo.Clear();
                }

                public static void ClearProxyLogList()
                {
                    lstProxyLogInfo.Clear();
                }

                #endregion

                #region//获取日志列表的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_LogList()
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                    menuItems.Add(new AntdUI.ContextMenuStripItem("复制", "Ctrl+C")
                    {
                        ID = "Copy",
                        IconSvg = "CopyOutlined",
                        LocalizationText = "Copy",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("导出到Excel")
                    {
                        ID = "ToExcel",
                        IconSvg = "FileExcelOutlined",
                        LocalizationText = "SaveToExcel",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("清空日志列表")
                    {
                        ID = "ClearUp",
                        IconSvg = "DeleteOutlined",
                        LocalizationText = "Clear",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("全选", "Ctrl+A")
                    {
                        ID = "SelectAll",
                        IconSvg = "UnorderedListOutlined",
                        LocalizationText = "SelectAll",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("取消选择")
                    {
                        ID = "DeSelect",
                        IconSvg = "DeleteRowOutlined",
                        LocalizationText = "DeSelect",
                    });

                    return menuItems.ToArray();
                }

                #endregion

                #region//保存系统日志列表为Excel（对话框）

                public static void SaveLogList_Dialog(Form form, AntdUI.Table tTable, string FileName, List<LogInfo> liList)
                {
                    try
                    {
                        if (LogConfig.List.lstLogInfo.Count > 0)
                        {
                            int SaveCount = LogConfig.List.lstLogInfo.Count;

                            SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                            sfdSaveToExcel.Filter = AntdUI.Localization.Get("ExcelFile", "Excel 文件") + " (*.xls)|*.xls";
                            sfdSaveToExcel.RestoreDirectory = true;

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveToExcel.FileName = FileName;
                            }

                            if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveToExcel.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    bool bOK = false;
                                    tTable.Spin(AntdUI.Localization.Get("Exporting", "正在导出..."), config =>
                                    {
                                        bOK = SaveLogListToExcel(FilePath, liList);
                                    }, () =>
                                    {
                                        if (bOK)
                                        {
                                            string Title = AntdUI.Localization.Get("ExportToExcel.Success", "导出到 Excel 成功");
                                            AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                            Operate.DoLog(nameof(SaveLogList_Dialog), Title + ": " + FilePath);
                                        }
                                        else
                                        {
                                            string Title = AntdUI.Localization.Get("ExportToExcel.Error", "导出到 Excel 失败");
                                            string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                            AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                        }
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveLogList_Dialog), ex.Message);
                    }
                }

                private static bool SaveLogListToExcel(string filePath, List<LogInfo> liList)
                {
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        using (var writer = new StreamWriter(stream, Encoding.Default))
                        {
                            writer.WriteLine(AntdUI.Localization.Get("ExcelColumn.Log", "记录时间\t模块\t日志内容\t"));

                            var dataSource = liList.Count > 0 ? liList : LogConfig.List.lstLogInfo.ToList();
                            foreach (var log in dataSource)
                            {
                                try
                                {
                                    var lineBuilder = new StringBuilder();

                                    lineBuilder.Append(log.LogTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff")).Append('\t');
                                    lineBuilder.Append(log.FuncName).Append('\t');
                                    lineBuilder.Append(log.LogContent).Append('\t');

                                    writer.WriteLine(lineBuilder.ToString());
                                }
                                catch (Exception ex)
                                {
                                    Operate.DoLog(nameof(SaveLogListToExcel), ex.Message);
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(SaveLogListToExcel), ex.Message);
                        return false;
                    }
                }

                #endregion                

                #region//保存滤镜日志列表为Excel（对话框）

                public static void SaveFilterLogList_Dialog(Form form, AntdUI.Table tTable, string FileName, List<FilterLogInfo> liList)
                {
                    try
                    {
                        if (LogConfig.List.lstFilterLogInfo.Count > 0)
                        {
                            int SaveCount = LogConfig.List.lstFilterLogInfo.Count;

                            SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                            sfdSaveToExcel.Filter = AntdUI.Localization.Get("ExcelFile", "Excel 文件") + " (*.xls)|*.xls";
                            sfdSaveToExcel.RestoreDirectory = true;

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveToExcel.FileName = FileName;
                            }

                            if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveToExcel.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    bool bOK = false;
                                    tTable.Spin(AntdUI.Localization.Get("Exporting", "正在导出..."), config =>
                                    {
                                        bOK = SaveFilterLogListToExcel(FilePath, liList);
                                    }, () =>
                                    {
                                        if (bOK)
                                        {
                                            string Title = AntdUI.Localization.Get("ExportToExcel.Success", "导出到 Excel 成功");
                                            AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                            Operate.DoLog(nameof(SaveFilterLogList_Dialog), Title + ": " + FilePath);
                                        }
                                        else
                                        {
                                            string Title = AntdUI.Localization.Get("ExportToExcel.Error", "导出到 Excel 失败");
                                            string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                            AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                        }
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveFilterLogList_Dialog), ex.Message);
                    }
                }

                private static bool SaveFilterLogListToExcel(string filePath, List<FilterLogInfo> liList)
                {
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        using (var writer = new StreamWriter(stream, Encoding.Default))
                        {
                            writer.WriteLine(AntdUI.Localization.Get("ExcelColumn.FilterLog", "记录时间\t滤镜名称\t动作\t匹配数\t类别\t长度\t"));

                            var dataSource = liList.Count > 0 ? liList : LogConfig.List.lstFilterLogInfo.ToList();
                            foreach (var log in dataSource)
                            {
                                try
                                {
                                    var lineBuilder = new StringBuilder();

                                    lineBuilder.Append(log.LogTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff")).Append('\t');
                                    lineBuilder.Append(log.FName).Append('\t');
                                    lineBuilder.Append(log.FAction).Append('\t');
                                    lineBuilder.Append(log.MatchNum).Append('\t');
                                    lineBuilder.Append(log.PacketType).Append('\t');
                                    lineBuilder.Append(log.PacketLen).Append('\t');

                                    writer.WriteLine(lineBuilder.ToString());
                                }
                                catch (Exception ex)
                                {
                                    Operate.DoLog(nameof(SaveFilterLogListToExcel), ex.Message);
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(SaveFilterLogListToExcel), ex.Message);
                        return false;
                    }
                }

                #endregion                

                #region//保存滤镜日志列表为Excel（对话框）

                public static void SaveProxyLogList_Dialog(Form form, AntdUI.Table tTable, string FileName, List<ProxyLogInfo> liList)
                {
                    try
                    {
                        if (LogConfig.List.lstProxyLogInfo.Count > 0)
                        {
                            int SaveCount = LogConfig.List.lstProxyLogInfo.Count;

                            SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                            sfdSaveToExcel.Filter = AntdUI.Localization.Get("ExcelFile", "Excel 文件") + " (*.xls)|*.xls";
                            sfdSaveToExcel.RestoreDirectory = true;

                            if (!string.IsNullOrEmpty(FileName))
                            {
                                sfdSaveToExcel.FileName = FileName;
                            }

                            if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                            {
                                string FilePath = sfdSaveToExcel.FileName;
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    bool bOK = false;
                                    tTable.Spin(AntdUI.Localization.Get("Exporting", "正在导出..."), config =>
                                    {
                                        bOK = SaveProxyLogListToExcel(FilePath, liList);
                                    }, () =>
                                    {
                                        if (bOK)
                                        {
                                            string Title = AntdUI.Localization.Get("ExportToExcel.Success", "导出到 Excel 成功");
                                            AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                            Operate.DoLog(nameof(SaveProxyLogList_Dialog), Title + ": " + FilePath);
                                        }
                                        else
                                        {
                                            string Title = AntdUI.Localization.Get("ExportToExcel.Error", "导出到 Excel 失败");
                                            string Content = AntdUI.Localization.Get("CheckSystemLog", "请检查系统日志");
                                            AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                        }
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(SaveProxyLogList_Dialog), ex.Message);
                    }
                }

                private static bool SaveProxyLogListToExcel(string filePath, List<ProxyLogInfo> liList)
                {
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        using (var writer = new StreamWriter(stream, Encoding.Default))
                        {
                            writer.WriteLine(AntdUI.Localization.Get("ExcelColumn.ProxyLog", "记录时间\t账号\tIP地址\t日志内容\t"));

                            var dataSource = liList.Count > 0 ? liList : LogConfig.List.lstProxyLogInfo.ToList();
                            foreach (var log in dataSource)
                            {
                                try
                                {
                                    var lineBuilder = new StringBuilder();

                                    lineBuilder.Append(log.LogTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff")).Append('\t');
                                    lineBuilder.Append(log.UserName).Append('\t');
                                    lineBuilder.Append(log.LoginIP).Append('\t');
                                    lineBuilder.Append(log.LogContent).Append('\t');

                                    writer.WriteLine(lineBuilder.ToString());
                                }
                                catch (Exception ex)
                                {
                                    Operate.DoLog(nameof(SaveProxyLogListToExcel), ex.Message);
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        DoLog(nameof(SaveProxyLogListToExcel), ex.Message);
                        return false;
                    }
                }

                #endregion                
            }

            #endregion
        }

        #endregion

        #region//记录日志        

        public static async void DoLog(string sFuncName, string sLogContent)
        {
            await LogConfig.Queue.LogToQueueAsync(sFuncName, sLogContent);
        }

        public static async void DoFilterLog(
            string FName,
            Operate.FilterConfig.Filter.FilterAction FAction,
            int MatchNum,
            Operate.PacketConfig.Packet.PacketType pType,
            int PacketLen)
        {
            await LogConfig.Queue.FilterLogToQueueAsync(FName, FAction, MatchNum, pType, PacketLen);
        }

        public static async void DoProxyLog(Guid AID, string ClientIP, string ServerAddress, string ViaIP)
        {
            try
            {
                string UserName = ProxyConfig.Account.GetUserName_ByAccountID(AID);

                if (string.IsNullOrEmpty(ViaIP))
                {
                    ViaIP = ProxyConfig.Proxy.ProxyTCP_IP.ToString();
                }

                string LogContent = string.Format(LogConfig.ProxyLogString, ServerAddress, ViaIP);

                await LogConfig.Queue.ProxyLogToQueueAsync(UserName, ClientIP, LogContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(DoProxyLog), ex.Message);
            }            
        }

        #endregion

        #region//数据库配置

        public static class DataBase
        {
            public static string dbPath = @"C:\WPE64DB";
            public static string dbName = SystemConfig.AssemblyVersion + ".db";
            public static string conStr = string.Empty;

            #region//初始化

            public static void InitConStr()
            {
                DataBase.conStr = string.Format("Data Source={0}\\{1};Version=3;", DataBase.dbPath, DataBase.dbName);
            }

            public static void InitDB()
            {
                try
                {
                    if (!Directory.Exists(DataBase.dbPath))
                    {
                        Directory.CreateDirectory(DataBase.dbPath);
                    }

                    DataBase.InitConStr();
                    DataBase.CreateTable_SystemConfig();
                    DataBase.CreateTable_InjectMode();
                    DataBase.CreateTable_ProxyMode();
                    DataBase.CreateTable_Filter();
                    DataBase.CreateTable_Send();
                    DataBase.CreateTable_Robot();
                    DataBase.CreateTable_ProxyAccount();
                    DataBase.CreateTable_ProxyMapLocal();
                    DataBase.CreateTable_ProxyMapRemote();
                    DataBase.CreateTable_WhiteList();
                    DataBase.CreateTable_BlackList();
                }
                catch (Exception ex)
                {
                    DoLog(nameof(InitDB), ex.Message);
                }                
            }

            #endregion

            #region//系统配置表

            private static bool CreateTable_SystemConfig()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS SystemConfig (";
                        sql += "IsAnimation BOOLEAN DEFAULT 0,";//系统设置 - 启用动画效果
                        sql += "IsShadowEnabled BOOLEAN DEFAULT 0,";//系统设置 - 启用阴影效果
                        sql += "IsShowInWindow BOOLEAN DEFAULT 0,";//系统设置 - 启用窗口显示
                        sql += "IsScrollBarHide BOOLEAN DEFAULT 0,";//系统设置 - 启用滚动条隐藏
                        sql += "IsTextRenderingHighQuality BOOLEAN DEFAULT 0,";//系统设置 - 启用文本渲染高质量
                        sql += "IsDark BOOLEAN DEFAULT 0,";//系统设置 - 启用深色主题
                        sql += "DefaultLanguage TEXT,";//系统设置 - 默认语言
                        sql += "LastInjection TEXT,";//系统设置 - 上次注入进程名称
                        sql += "Remote_IsEnable BOOLEAN DEFAULT 0,";//系统设置 - 启用远程管理
                        sql += "Remote_UserName TEXT,";//系统设置 - 远程管理账号
                        sql += "Remote_PassWord TEXT,";//系统设置 - 远程管理密码
                        sql += "Remote_Port INTEGER,";//系统设置 - 远程管理端口                    
                        sql += "Remote_IP TEXT,";//系统设置 - 远程管理IP
                        sql += "IsShow_FloatButton BOOLEAN DEFAULT 1,";//是否显示悬浮按钮
                        sql += "ListExecute INTEGER DEFAULT 1,";//列表执行模式
                        sql += "FilterExecute INTEGER DEFAULT 1,";//滤镜执行模式
                        sql += "LogList_AutoRoll BOOLEAN DEFAULT 0,";//日志列表自动滚动
                        sql += "LogList_AutoClear BOOLEAN DEFAULT 1,";//日志列表自动清理
                        sql += "LogList_AutoClear_Value INTEGER DEFAULT 5000,";//日志列表自动清理数值
                        sql += "CheckNotShow BOOLEAN DEFAULT 1,";//过滤设置不显示
                        sql += "CheckSocket BOOLEAN DEFAULT 0,";//过滤套接字
                        sql += "CheckSocket_Value TEXT,";//过滤套接字内容
                        sql += "CheckIP BOOLEAN DEFAULT 0,";//过滤IP
                        sql += "CheckIP_Value TEXT,";//过滤IP内容
                        sql += "CheckPort BOOLEAN DEFAULT 0,";//过滤端口
                        sql += "CheckPort_Value TEXT,";//过滤端口内容
                        sql += "CheckHead BOOLEAN DEFAULT 0,";//过滤包头
                        sql += "CheckHead_Value TEXT,";//过滤包头内容
                        sql += "CheckData BOOLEAN DEFAULT 0,";//过滤数据
                        sql += "CheckData_Value TEXT,";//过滤数据内容
                        sql += "CheckSize BOOLEAN DEFAULT 0,";//过滤长度
                        sql += "CheckLength_Value TEXT,";//过滤长度内容
                        sql += "CheckType BOOLEAN DEFAULT 0,";//过滤封包类别
                        sql += "CheckType_Value TEXT,";//过滤封包类别内容
                        sql += "HotKeyType INTEGER DEFAULT 0,";//快捷键类型
                        sql += "HotKey1 TEXT,";//快捷键1
                        sql += "HotKey2 TEXT,";//快捷键2
                        sql += "HotKey3 TEXT,";//快捷键3
                        sql += "HotKey4 TEXT,";//快捷键4
                        sql += "HotKey5 TEXT,";//快捷键5
                        sql += "HotKey6 TEXT,";//快捷键6
                        sql += "HotKey7 TEXT,";//快捷键7
                        sql += "HotKey8 TEXT,";//快捷键8
                        sql += "HotKey9 TEXT,";//快捷键9
                        sql += "HotKey10 TEXT,";//快捷键10
                        sql += "HotKey11 TEXT,";//快捷键11
                        sql += "HotKey12 TEXT,";//快捷键12
                        sql += "SystemColor INTEGER,";//系统主题颜色
                        sql += "SpeedMode BOOLEAN DEFAULT 0,";//极速模式
                        sql += "FilterReplace_BackColor INTEGER,";//替换背景颜色
                        sql += "FilterReplace_ForeColor INTEGER,";//替换字体颜色
                        sql += "FilterIntercept_BackColor INTEGER,";//拦截背景颜色
                        sql += "FilterIntercept_ForeColor INTEGER,";//拦截字体颜色
                        sql += "FilterChange_BackColor INTEGER,";//换包背景颜色
                        sql += "FilterChange_ForeColor INTEGER";//换包字体颜色                        
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_SystemConfig), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_SystemConfig()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM SystemConfig;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_SystemConfig), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_SystemConfig()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM SystemConfig;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_SystemConfig), ex.Message);
                }
            }

            public static void InsertTable_SystemConfig()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "INSERT INTO SystemConfig (";
                        sql += "IsAnimation,";
                        sql += "IsShadowEnabled,";
                        sql += "IsShowInWindow,";
                        sql += "IsScrollBarHide,";
                        sql += "IsTextRenderingHighQuality,";
                        sql += "IsDark,";
                        sql += "DefaultLanguage,";
                        sql += "LastInjection,";
                        sql += "Remote_IsEnable,";
                        sql += "Remote_UserName,";
                        sql += "Remote_PassWord,";
                        sql += "Remote_Port,";
                        sql += "Remote_IP,";
                        sql += "IsShow_FloatButton,";
                        sql += "ListExecute,";
                        sql += "FilterExecute,";
                        sql += "LogList_AutoRoll,";
                        sql += "LogList_AutoClear,";
                        sql += "LogList_AutoClear_Value,";
                        sql += "CheckNotShow,";
                        sql += "CheckSocket,";
                        sql += "CheckSocket_Value,";
                        sql += "CheckIP,";
                        sql += "CheckIP_Value,";
                        sql += "CheckPort,";
                        sql += "CheckPort_Value,";
                        sql += "CheckHead,";
                        sql += "CheckHead_Value,";
                        sql += "CheckData,";
                        sql += "CheckData_Value,";
                        sql += "CheckSize,";
                        sql += "CheckLength_Value,";
                        sql += "CheckType,";
                        sql += "CheckType_Value,";
                        sql += "HotKeyType,";
                        sql += "HotKey1,";
                        sql += "HotKey2,";
                        sql += "HotKey3,";
                        sql += "HotKey4,";
                        sql += "HotKey5,";
                        sql += "HotKey6,";
                        sql += "HotKey7,";
                        sql += "HotKey8,";
                        sql += "HotKey9,";
                        sql += "HotKey10,";
                        sql += "HotKey11,";
                        sql += "HotKey12,";
                        sql += "SystemColor,";
                        sql += "SpeedMode,";
                        sql += "FilterReplace_BackColor,";
                        sql += "FilterReplace_ForeColor,";
                        sql += "FilterIntercept_BackColor,";
                        sql += "FilterIntercept_ForeColor,";
                        sql += "FilterChange_BackColor,";
                        sql += "FilterChange_ForeColor";                        
                        sql += ") VALUES (";
                        sql += "@IsAnimation,";
                        sql += "@IsShadowEnabled,";
                        sql += "@IsShowInWindow,";
                        sql += "@IsScrollBarHide,";
                        sql += "@IsTextRenderingHighQuality,";
                        sql += "@IsDark,";
                        sql += "@DefaultLanguage,";
                        sql += "@LastInjection,";
                        sql += "@Remote_IsEnable,";
                        sql += "@Remote_UserName,";
                        sql += "@Remote_PassWord,";
                        sql += "@Remote_Port,";
                        sql += "@Remote_IP,";
                        sql += "@IsShow_FloatButton,";
                        sql += "@ListExecute,";
                        sql += "@FilterExecute,";
                        sql += "@LogList_AutoRoll,";
                        sql += "@LogList_AutoClear,";
                        sql += "@LogList_AutoClear_Value,";
                        sql += "@CheckNotShow,";
                        sql += "@CheckSocket,";
                        sql += "@CheckSocket_Value,";
                        sql += "@CheckIP,";
                        sql += "@CheckIP_Value,";
                        sql += "@CheckPort,";
                        sql += "@CheckPort_Value,";
                        sql += "@CheckHead,";
                        sql += "@CheckHead_Value,";
                        sql += "@CheckData,";
                        sql += "@CheckData_Value,";
                        sql += "@CheckSize,";
                        sql += "@CheckLength_Value,";
                        sql += "@CheckType,";
                        sql += "@CheckType_Value,";
                        sql += "@HotKeyType,";
                        sql += "@HotKey1,";
                        sql += "@HotKey2,";
                        sql += "@HotKey3,";
                        sql += "@HotKey4,";
                        sql += "@HotKey5,";
                        sql += "@HotKey6,";
                        sql += "@HotKey7,";
                        sql += "@HotKey8,";
                        sql += "@HotKey9,";
                        sql += "@HotKey10,";
                        sql += "@HotKey11,";
                        sql += "@HotKey12,";
                        sql += "@SystemColor,";
                        sql += "@SpeedMode,";
                        sql += "@FilterReplace_BackColor,";
                        sql += "@FilterReplace_ForeColor,";
                        sql += "@FilterIntercept_BackColor,";
                        sql += "@FilterIntercept_ForeColor,";
                        sql += "@FilterChange_BackColor,";
                        sql += "@FilterChange_ForeColor";                        
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@IsAnimation", AntdUI.Config.Animation);
                            cmd.Parameters.AddWithValue("@IsShadowEnabled", AntdUI.Config.ShadowEnabled);
                            cmd.Parameters.AddWithValue("@IsShowInWindow", AntdUI.Config.ShowInWindow);
                            cmd.Parameters.AddWithValue("@IsScrollBarHide", AntdUI.Config.ScrollBarHide);
                            cmd.Parameters.AddWithValue("@IsTextRenderingHighQuality", AntdUI.Config.TextRenderingHighQuality);
                            cmd.Parameters.AddWithValue("@IsDark", AntdUI.Config.IsDark);
                            cmd.Parameters.AddWithValue("@DefaultLanguage", AntdUI.Localization.CurrentLanguage);
                            cmd.Parameters.AddWithValue("@LastInjection", SystemConfig.LastInjection);
                            cmd.Parameters.AddWithValue("@Remote_IsEnable", SystemConfig.IsRemote);
                            cmd.Parameters.AddWithValue("@Remote_UserName", SystemConfig.Remote_UserName);
                            cmd.Parameters.AddWithValue("@Remote_PassWord", SystemConfig.Remote_PassWord);
                            cmd.Parameters.AddWithValue("@Remote_Port", SystemConfig.Remote_Port);
                            cmd.Parameters.AddWithValue("@Remote_IP", SystemConfig.Remote_IP);
                            cmd.Parameters.AddWithValue("@IsShow_FloatButton", SystemConfig.IsShow_FloatButton);
                            cmd.Parameters.AddWithValue("@ListExecute", SystemConfig.ListExecute);
                            cmd.Parameters.AddWithValue("@FilterExecute", FilterConfig.Filter.FilterExecute);
                            cmd.Parameters.AddWithValue("@LogList_AutoRoll", LogConfig.List.AutoRoll);
                            cmd.Parameters.AddWithValue("@LogList_AutoClear", LogConfig.List.AutoClear);
                            cmd.Parameters.AddWithValue("@LogList_AutoClear_Value", LogConfig.List.AutoClear_Value);
                            cmd.Parameters.AddWithValue("@CheckNotShow", SystemConfig.CheckNotShow);
                            cmd.Parameters.AddWithValue("@CheckSocket", SystemConfig.CheckSocket);
                            cmd.Parameters.AddWithValue("@CheckSocket_Value", SystemConfig.CheckSocket_Value);
                            cmd.Parameters.AddWithValue("@CheckIP", SystemConfig.CheckIP);
                            cmd.Parameters.AddWithValue("@CheckIP_Value", SystemConfig.CheckIP_Value);
                            cmd.Parameters.AddWithValue("@CheckPort", SystemConfig.CheckPort);
                            cmd.Parameters.AddWithValue("@CheckPort_Value", SystemConfig.CheckPort_Value);
                            cmd.Parameters.AddWithValue("@CheckHead", SystemConfig.CheckHead);
                            cmd.Parameters.AddWithValue("@CheckHead_Value", SystemConfig.CheckHead_Value);
                            cmd.Parameters.AddWithValue("@CheckData", SystemConfig.CheckData);
                            cmd.Parameters.AddWithValue("@CheckData_Value", SystemConfig.CheckData_Value);
                            cmd.Parameters.AddWithValue("@CheckSize", SystemConfig.CheckLen);
                            cmd.Parameters.AddWithValue("@CheckLength_Value", SystemConfig.CheckLength_Value);
                            cmd.Parameters.AddWithValue("@CheckType", SystemConfig.CheckType);
                            cmd.Parameters.AddWithValue("@CheckType_Value", FilterConfig.Filter.GetFilterFunctionString(SystemConfig.CheckType_Value));
                            cmd.Parameters.AddWithValue("@HotKeyType", SystemConfig.HotKeyType);
                            cmd.Parameters.AddWithValue("@HotKey1", SystemConfig.HotKey1);
                            cmd.Parameters.AddWithValue("@HotKey2", SystemConfig.HotKey2);
                            cmd.Parameters.AddWithValue("@HotKey3", SystemConfig.HotKey3);
                            cmd.Parameters.AddWithValue("@HotKey4", SystemConfig.HotKey4);
                            cmd.Parameters.AddWithValue("@HotKey5", SystemConfig.HotKey5);
                            cmd.Parameters.AddWithValue("@HotKey6", SystemConfig.HotKey6);
                            cmd.Parameters.AddWithValue("@HotKey7", SystemConfig.HotKey7);
                            cmd.Parameters.AddWithValue("@HotKey8", SystemConfig.HotKey8);
                            cmd.Parameters.AddWithValue("@HotKey9", SystemConfig.HotKey9);
                            cmd.Parameters.AddWithValue("@HotKey10", SystemConfig.HotKey10);
                            cmd.Parameters.AddWithValue("@HotKey11", SystemConfig.HotKey11);
                            cmd.Parameters.AddWithValue("@HotKey12", SystemConfig.HotKey12);
                            cmd.Parameters.AddWithValue("@SystemColor", SystemConfig.SystemColor.ToArgb());
                            cmd.Parameters.AddWithValue("@SpeedMode", SystemConfig.SpeedMode);
                            cmd.Parameters.AddWithValue("@FilterReplace_BackColor", FilterConfig.Filter.FilterReplace_BackColor.ToArgb());
                            cmd.Parameters.AddWithValue("@FilterReplace_ForeColor", FilterConfig.Filter.FilterReplace_ForeColor.ToArgb());
                            cmd.Parameters.AddWithValue("@FilterIntercept_BackColor", FilterConfig.Filter.FilterIntercept_BackColor.ToArgb());
                            cmd.Parameters.AddWithValue("@FilterIntercept_ForeColor", FilterConfig.Filter.FilterIntercept_ForeColor.ToArgb());
                            cmd.Parameters.AddWithValue("@FilterChange_BackColor", FilterConfig.Filter.FilterChange_BackColor.ToArgb());
                            cmd.Parameters.AddWithValue("@FilterChange_ForeColor", FilterConfig.Filter.FilterChange_ForeColor.ToArgb());

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_SystemConfig), ex.Message);
                }
            }

            public static void UpdateTable_SystemConfig_LastInjection()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "UPDATE SystemConfig SET SystemConfig_LastInjection = @LastInjection;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@LastInjection", SystemConfig.LastInjection);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(UpdateTable_SystemConfig_LastInjection), ex.Message);
                }
            }

            #endregion

            #region//注入模式配置表

            private static bool CreateTable_InjectMode()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS InjectMode (";                        
                        sql += "HookWS1_Send BOOLEAN DEFAULT 1,";//发送1.1
                        sql += "HookWS1_SendTo BOOLEAN DEFAULT 1,";//发送到1.1
                        sql += "HookWS1_Recv BOOLEAN DEFAULT 1,";//接收1.1
                        sql += "HookWS1_RecvFrom BOOLEAN DEFAULT 1,";//接收自1.1
                        sql += "HookWS2_Send BOOLEAN DEFAULT 1,";//发送
                        sql += "HookWS2_SendTo BOOLEAN DEFAULT 1,";//发送到
                        sql += "HookWS2_Recv BOOLEAN DEFAULT 1,";//接收
                        sql += "HookWS2_RecvFrom BOOLEAN DEFAULT 1,";//接收自
                        sql += "HookWSA_Send BOOLEAN DEFAULT 1,";//WSA 发送
                        sql += "HookWSA_SendTo BOOLEAN DEFAULT 1,";//WSA 发送到
                        sql += "HookWSA_Recv BOOLEAN DEFAULT 1,";//WSA 接收
                        sql += "HookWSA_RecvFrom BOOLEAN DEFAULT 1,";//WSA 接收自                        
                        sql += "PacketList_AutoRoll BOOLEAN DEFAULT 0,";//封包列表自动滚动
                        sql += "PacketList_AutoClear BOOLEAN DEFAULT 1,";//封包列表自动清理
                        sql += "PacketList_AutoClear_Value INTEGER DEFAULT 5000";//封包列表自动清理数值
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_InjectMode), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_InjectMode()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM InjectMode;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_InjectMode), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_InjectMode()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM InjectMode;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_InjectMode), ex.Message);
                }
            }

            public static void InsertTable_InjectMode()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "INSERT INTO InjectMode (";
                        sql += "HookWS1_Send,";
                        sql += "HookWS1_SendTo,";
                        sql += "HookWS1_Recv,";
                        sql += "HookWS1_RecvFrom,";
                        sql += "HookWS2_Send,";
                        sql += "HookWS2_SendTo,";
                        sql += "HookWS2_Recv,";
                        sql += "HookWS2_RecvFrom,";
                        sql += "HookWSA_Send,";
                        sql += "HookWSA_SendTo,";
                        sql += "HookWSA_Recv,";
                        sql += "HookWSA_RecvFrom,";                        
                        sql += "PacketList_AutoRoll,";
                        sql += "PacketList_AutoClear,";
                        sql += "PacketList_AutoClear_Value";
                        sql += ") VALUES (";
                        sql += "@HookWS1_Send,";
                        sql += "@HookWS1_SendTo,";
                        sql += "@HookWS1_Recv,";
                        sql += "@HookWS1_RecvFrom,";
                        sql += "@HookWS2_Send,";
                        sql += "@HookWS2_SendTo,";
                        sql += "@HookWS2_Recv,";
                        sql += "@HookWS2_RecvFrom,";
                        sql += "@HookWSA_Send,";
                        sql += "@HookWSA_SendTo,";
                        sql += "@HookWSA_Recv,";
                        sql += "@HookWSA_RecvFrom,";                        
                        sql += "@PacketList_AutoRoll,";
                        sql += "@PacketList_AutoClear,";
                        sql += "@PacketList_AutoClear_Value";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {  
                            cmd.Parameters.AddWithValue("@HookWS1_Send", PacketConfig.Packet.HookWS1_Send);
                            cmd.Parameters.AddWithValue("@HookWS1_SendTo", PacketConfig.Packet.HookWS1_SendTo);
                            cmd.Parameters.AddWithValue("@HookWS1_Recv", PacketConfig.Packet.HookWS1_Recv);
                            cmd.Parameters.AddWithValue("@HookWS1_RecvFrom", PacketConfig.Packet.HookWS1_RecvFrom);
                            cmd.Parameters.AddWithValue("@HookWS2_Send", PacketConfig.Packet.HookWS2_Send);
                            cmd.Parameters.AddWithValue("@HookWS2_SendTo", PacketConfig.Packet.HookWS2_SendTo);
                            cmd.Parameters.AddWithValue("@HookWS2_Recv", PacketConfig.Packet.HookWS2_Recv);
                            cmd.Parameters.AddWithValue("@HookWS2_RecvFrom", PacketConfig.Packet.HookWS2_RecvFrom);
                            cmd.Parameters.AddWithValue("@HookWSA_Send", PacketConfig.Packet.HookWSA_Send);
                            cmd.Parameters.AddWithValue("@HookWSA_SendTo", PacketConfig.Packet.HookWSA_SendTo);
                            cmd.Parameters.AddWithValue("@HookWSA_Recv", PacketConfig.Packet.HookWSA_Recv);
                            cmd.Parameters.AddWithValue("@HookWSA_RecvFrom", PacketConfig.Packet.HookWSA_RecvFrom);                            
                            cmd.Parameters.AddWithValue("@PacketList_AutoRoll", PacketConfig.List.AutoRoll);
                            cmd.Parameters.AddWithValue("@PacketList_AutoClear", PacketConfig.List.AutoClear);
                            cmd.Parameters.AddWithValue("@PacketList_AutoClear_Value", PacketConfig.List.AutoClear_Value);  

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_InjectMode), ex.Message);
                }
            }

            #endregion

            #region//代理模式配置表

            private static bool CreateTable_ProxyMode()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS ProxyMode (";
                        sql += "ProxyIP_Auto BOOLEAN DEFAULT 1,";//代理模式 - 自动检测IP                        
                        sql += "EnableSOCKS5 BOOLEAN DEFAULT 1,";//代理模式 - 启用SOCKS5代理
                        sql += "ProxyIP TEXT,";//代理模式 - 代理IP
                        sql += "ProxyPort INTEGER DEFAULT 1080,";//代理模式 - 代理端口                        
                        sql += "EnableAuth BOOLEAN DEFAULT 1,";//代理模式 - 启用代理认证
                        sql += "MaxConnectionNumber INTEGER DEFAULT 5000,";//代理模式 - 最大连接数
                        sql += "Enable_MapLocal BOOLEAN DEFAULT 0,";//代理模式 - 启用本地代理映射
                        sql += "Enable_MapRemote BOOLEAN DEFAULT 0,";//代理模式 - 启用远程代理映射
                        sql += "Enable_ExternalProxy BOOLEAN DEFAULT 0,";//代理模式 - 启用外部代理
                        sql += "ExternalProxy_IP TEXT,";//代理模式 - 外部代理IP
                        sql += "ExternalProxy_Port INTEGER DEFAULT 8889,";//代理模式 - 外部代理端口               
                        sql += "Enable_ExternalProxy_AppointPort BOOLEAN DEFAULT 0,";//代理模式 - 启用指定代理端口
                        sql += "ExternalProxy_AppointPort TEXT,";//代理模式 - 指定代理端口
                        sql += "Enable_ExternalProxy_Auth BOOLEAN DEFAULT 0,";//代理模式 - 启用外部代理认证
                        sql += "ExternalProxy_UserName TEXT,";//代理模式 - 外部代理用户名
                        sql += "ExternalProxy_PassWord TEXT,";//代理模式 - 外部代理密码
                        sql += "EnableFireWall BOOLEAN DEFAULT 0,";//代理模式 - 启用防火墙
                        sql += "WhiteListMode BOOLEAN DEFAULT 0,";//代理模式 - 是否白名单模式
                        sql += "FireWall_AutoBlock_UnSupport BOOLEAN DEFAULT 0,";//代理模式 - 自动屏蔽不支持的协议
                        sql += "FireWall_AutoBlock_Minutes INTEGER DEFAULT 30,";//代理模式 - 自动屏蔽时间
                        sql += "FireWall_AutoClear_Expiry BOOLEAN DEFAULT 0";//代理模式 - 自动清理过期的规则
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_ProxyMode), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_ProxyMode()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM ProxyMode;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_ProxyMode), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_ProxyMode()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM ProxyMode;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_ProxyMode), ex.Message);
                }
            }

            public static void InsertTable_ProxyMode()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "INSERT INTO ProxyMode (";
                        sql += "ProxyIP_Auto,";
                        sql += "EnableSOCKS5,";
                        sql += "ProxyIP,";
                        sql += "ProxyPort,";                        
                        sql += "EnableAuth,";
                        sql += "MaxConnectionNumber,";
                        sql += "Enable_MapLocal,";
                        sql += "Enable_MapRemote,";
                        sql += "Enable_ExternalProxy,";
                        sql += "ExternalProxy_IP,";
                        sql += "ExternalProxy_Port,";
                        sql += "Enable_ExternalProxy_AppointPort,";
                        sql += "ExternalProxy_AppointPort,";
                        sql += "Enable_ExternalProxy_Auth,";
                        sql += "ExternalProxy_UserName,";
                        sql += "ExternalProxy_PassWord,";
                        sql += "EnableFireWall,";
                        sql += "WhiteListMode,";
                        sql += "FireWall_AutoBlock_UnSupport,";
                        sql += "FireWall_AutoBlock_Minutes,";
                        sql += "FireWall_AutoClear_Expiry";
                        sql += ") VALUES (";
                        sql += "@ProxyIP_Auto,";
                        sql += "@EnableSOCKS5,";
                        sql += "@ProxyIP,";
                        sql += "@ProxyPort,";                        
                        sql += "@EnableAuth,";
                        sql += "@MaxConnectionNumber,";
                        sql += "@Enable_MapLocal,";
                        sql += "@Enable_MapRemote,";
                        sql += "@Enable_ExternalProxy,";
                        sql += "@ExternalProxy_IP,";
                        sql += "@ExternalProxy_Port,";
                        sql += "@Enable_ExternalProxy_AppointPort,";
                        sql += "@ExternalProxy_AppointPort,";
                        sql += "@Enable_ExternalProxy_Auth,";
                        sql += "@ExternalProxy_UserName,";
                        sql += "@ExternalProxy_PassWord,";
                        sql += "@EnableFireWall,";
                        sql += "@WhiteListMode,";
                        sql += "@FireWall_AutoBlock_UnSupport,";
                        sql += "@FireWall_AutoBlock_Minutes,";
                        sql += "@FireWall_AutoClear_Expiry";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ProxyIP_Auto", ProxyConfig.Proxy.ProxyIP_Auto);
                            cmd.Parameters.AddWithValue("@EnableSOCKS5", ProxyConfig.Proxy.Enable_SOCKS5);
                            cmd.Parameters.AddWithValue("@ProxyIP", ProxyConfig.Proxy.ProxyIP);
                            cmd.Parameters.AddWithValue("@ProxyPort", ProxyConfig.Proxy.ProxyPort);                            
                            cmd.Parameters.AddWithValue("@EnableAuth", ProxyConfig.Proxy.Enable_Auth);
                            cmd.Parameters.AddWithValue("@MaxConnectionNumber", ProxyConfig.Proxy.MaxConnectionNumber);
                            cmd.Parameters.AddWithValue("@Enable_MapLocal", ProxyConfig.Mapping.Enable_MapLocal);
                            cmd.Parameters.AddWithValue("@Enable_MapRemote", ProxyConfig.Mapping.Enable_MapRemote);
                            cmd.Parameters.AddWithValue("@Enable_ExternalProxy", ProxyConfig.Proxy.Enable_ExternalProxy);
                            cmd.Parameters.AddWithValue("@ExternalProxy_IP", ProxyConfig.Proxy.ExternalProxy_IP);
                            cmd.Parameters.AddWithValue("@ExternalProxy_Port", ProxyConfig.Proxy.ExternalProxy_Port);
                            cmd.Parameters.AddWithValue("@Enable_ExternalProxy_AppointPort", ProxyConfig.Proxy.Enable_ExternalProxy_AppointPort);
                            cmd.Parameters.AddWithValue("@ExternalProxy_AppointPort", ProxyConfig.Proxy.ExternalProxy_AppointPort);
                            cmd.Parameters.AddWithValue("@Enable_ExternalProxy_Auth", ProxyConfig.Proxy.Enable_ExternalProxy_Auth);
                            cmd.Parameters.AddWithValue("@ExternalProxy_UserName", ProxyConfig.Proxy.ExternalProxy_UserName);
                            cmd.Parameters.AddWithValue("@ExternalProxy_PassWord", ProxyConfig.Proxy.ExternalProxy_PassWord);
                            cmd.Parameters.AddWithValue("@EnableFireWall", ProxyConfig.Proxy.EnableFireWall);
                            cmd.Parameters.AddWithValue("@WhiteListMode", ProxyConfig.Proxy.WhiteListMode);
                            cmd.Parameters.AddWithValue("@FireWall_AutoBlock_UnSupport", ProxyConfig.Proxy.FireWall_AutoBlock_UnSupport);
                            cmd.Parameters.AddWithValue("@FireWall_AutoBlock_Minutes", ProxyConfig.Proxy.FireWall_AutoBlock_Minutes);
                            cmd.Parameters.AddWithValue("@FireWall_AutoClear_Expiry", ProxyConfig.Proxy.FireWall_AutoClear_Expiry);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_ProxyMode), ex.Message);
                }
            }

            #endregion            

            #region//滤镜列表

            private static bool CreateTable_Filter()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS Filter (";
                        sql += "GUID TEXT NOT NULL PRIMARY KEY,";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";
                        sql += "Name TEXT NOT NULL,";
                        sql += "AppointHeader BOOLEAN DEFAULT 0,";
                        sql += "HeaderContent TEXT,";
                        sql += "AppointSocket BOOLEAN DEFAULT 0,";
                        sql += "SocketContent INTEGER DEFAULT 0,";
                        sql += "AppointLength BOOLEAN DEFAULT 0,";
                        sql += "LengthContent TEXT,";
                        sql += "AppointPort BOOLEAN DEFAULT 0,";
                        sql += "PortContent INTEGER DEFAULT 0,";
                        sql += "Mode INTEGER NOT NULL DEFAULT 0,";
                        sql += "Action INTEGER NOT NULL DEFAULT 0,";
                        sql += "IsExecute BOOLEAN DEFAULT 0,";
                        sql += "ExecuteType INTEGER DEFAULT 0,";
                        sql += "ExecuteGUID TEXT NOT NULL,";
                        sql += "Function TEXT NOT NULL,";
                        sql += "StartFrom INTEGER DEFAULT 0,";
                        sql += "IsProgressionContinuous BOOLEAN DEFAULT 0,";
                        sql += "ProgressionStep INTEGER DEFAULT 1,";
                        sql += "IsProgressionCarry BOOLEAN DEFAULT 0,";
                        sql += "ProgressionCarryNumber INTEGER DEFAULT 1,";
                        sql += "ProgressionPosition TEXT,";
                        sql += "ExcludePosition TEXT,";
                        sql += "Search TEXT,";
                        sql += "Modify TEXT";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_Filter), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_Filter()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM Filter;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_Filter), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_Filter()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM Filter;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_Filter), ex.Message);
                }
            }

            public static void InsertTable_Filter(FilterInfo sfi)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "INSERT INTO Filter (";
                        sql += "GUID,";
                        sql += "IsEnable,";
                        sql += "Name,";
                        sql += "AppointHeader,";
                        sql += "HeaderContent,";
                        sql += "AppointSocket,";
                        sql += "SocketContent,";
                        sql += "AppointLength,";
                        sql += "LengthContent,";
                        sql += "AppointPort,";
                        sql += "PortContent,";
                        sql += "Mode,";
                        sql += "Action,";
                        sql += "IsExecute,";
                        sql += "ExecuteType,";
                        sql += "ExecuteGUID,";
                        sql += "Function,";
                        sql += "StartFrom,";
                        sql += "IsProgressionContinuous,";
                        sql += "ProgressionStep,";
                        sql += "IsProgressionCarry,";
                        sql += "ProgressionCarryNumber,";
                        sql += "ProgressionPosition,";
                        sql += "ExcludePosition,";
                        sql += "Search,";
                        sql += "Modify";
                        sql += ") VALUES (";
                        sql += "@GUID,";
                        sql += "@IsEnable,";
                        sql += "@Name,";
                        sql += "@AppointHeader,";
                        sql += "@HeaderContent,";
                        sql += "@AppointSocket,";
                        sql += "@SocketContent,";
                        sql += "@AppointLength,";
                        sql += "@LengthContent,";
                        sql += "@AppointPort,";
                        sql += "@PortContent,";
                        sql += "@Mode,";
                        sql += "@Action,";
                        sql += "@IsExecute,";
                        sql += "@ExecuteType,";
                        sql += "@ExecuteGUID,";
                        sql += "@Function,";
                        sql += "@StartFrom,";
                        sql += "@IsProgressionContinuous,";
                        sql += "@ProgressionStep,";
                        sql += "@IsProgressionCarry,";
                        sql += "@ProgressionCarryNumber,";
                        sql += "@ProgressionPosition,";
                        sql += "@ExcludePosition,";
                        sql += "@Search,";
                        sql += "@Modify";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", sfi.FID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@IsEnable", sfi.IsEnable);
                            cmd.Parameters.AddWithValue("@Name", sfi.FName);
                            cmd.Parameters.AddWithValue("@AppointHeader", sfi.AppointHeader);
                            cmd.Parameters.AddWithValue("@HeaderContent", sfi.HeaderContent);
                            cmd.Parameters.AddWithValue("@AppointSocket", sfi.AppointSocket);
                            cmd.Parameters.AddWithValue("@SocketContent", sfi.SocketContent);
                            cmd.Parameters.AddWithValue("@AppointLength", sfi.AppointLength);
                            cmd.Parameters.AddWithValue("@LengthContent", sfi.LengthContent);
                            cmd.Parameters.AddWithValue("@AppointPort", sfi.AppointPort);
                            cmd.Parameters.AddWithValue("@PortContent", sfi.PortContent);
                            cmd.Parameters.AddWithValue("@Mode", sfi.FMode);
                            cmd.Parameters.AddWithValue("@Action", sfi.FAction);
                            cmd.Parameters.AddWithValue("@IsExecute", sfi.IsExecute);
                            cmd.Parameters.AddWithValue("@ExecuteType", sfi.FEType);
                            cmd.Parameters.AddWithValue("@ExecuteGUID", sfi.Execute_GUID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@Function", FilterConfig.Filter.GetFilterFunctionString(sfi.FFunction));
                            cmd.Parameters.AddWithValue("@StartFrom", sfi.FStartFrom);
                            cmd.Parameters.AddWithValue("@IsProgressionContinuous", sfi.IsProgressionContinuous);
                            cmd.Parameters.AddWithValue("@ProgressionStep", sfi.ProgressionStep);
                            cmd.Parameters.AddWithValue("@IsProgressionCarry", sfi.IsProgressionCarry);
                            cmd.Parameters.AddWithValue("@ProgressionCarryNumber", sfi.ProgressionCarryNumber);
                            cmd.Parameters.AddWithValue("@ProgressionPosition", sfi.ProgressionPosition);
                            cmd.Parameters.AddWithValue("@ExcludePosition", sfi.ExcludePosition);
                            cmd.Parameters.AddWithValue("@Search", sfi.FSearch);
                            cmd.Parameters.AddWithValue("@Modify", sfi.FModify);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_Filter), ex.Message);
                }
            }

            #endregion

            #region//发送列表

            private static bool CreateTable_Send()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS Send (";
                        sql += "GUID TEXT NOT NULL PRIMARY KEY,";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";
                        sql += "Name TEXT NOT NULL,";
                        sql += "SystemSocket BOOLEAN DEFAULT 0,";
                        sql += "LoopCNT INTEGER NOT NULL DEFAULT 1,";
                        sql += "LoopINT INTEGER NOT NULL DEFAULT 1000,";
                        sql += "Notes TEXT";
                        sql += ");";

                        sql += "CREATE TABLE IF NOT EXISTS SendCollection (";
                        sql += "GUID TEXT NOT NULL,";
                        sql += "Socket INTEGER NOT NULL,";
                        sql += "Type INTEGER NOT NULL,";
                        sql += "IPFrom TEXT NOT NULL,";
                        sql += "IPTo TEXT NOT NULL,";
                        sql += "Buffer BLOB,";
                        sql += "FOREIGN KEY (GUID) REFERENCES Send(GUID)";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_Send), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_Send()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM Send;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_Send), ex.Message);
                }

                return dtReturn;
            }

            public static DataTable SelectTable_SendCollection(Guid guid)
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM SendCollection WHERE GUID = @GUID;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", guid.ToString().ToUpper());

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_SendCollection), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_Send()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM SendCollection;";
                        sql += "DELETE FROM Send;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_Send), ex.Message);
                }
            }

            public static void InsertTable_Send(SendInfo si)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        conn.Open();

                        string sql = "INSERT INTO Send (";
                        sql += "GUID,";
                        sql += "IsEnable,";
                        sql += "Name,";
                        sql += "SystemSocket,";
                        sql += "LoopCNT,";
                        sql += "LoopINT,";
                        sql += "Notes";
                        sql += ") VALUES (";
                        sql += "@GUID,";
                        sql += "@IsEnable,";
                        sql += "@Name,";
                        sql += "@SystemSocket,";
                        sql += "@LoopCNT,";
                        sql += "@LoopINT,";
                        sql += "@Notes";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", si.SID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@IsEnable", si.IsEnable);
                            cmd.Parameters.AddWithValue("@Name", si.SName);
                            cmd.Parameters.AddWithValue("@SystemSocket", si.SSystemSocket);
                            cmd.Parameters.AddWithValue("@LoopCNT", si.SLoopCNT);
                            cmd.Parameters.AddWithValue("@LoopINT", si.SLoopINT);
                            cmd.Parameters.AddWithValue("@Notes", si.SNotes);
                            cmd.ExecuteNonQuery();
                        }

                        foreach (PacketInfo pi in si.SCollection)
                        {
                            sql = "INSERT INTO SendCollection (";
                            sql += "GUID,";
                            sql += "Socket,";
                            sql += "Type,";
                            sql += "IPFrom,";
                            sql += "IPTo,";
                            sql += "Buffer";
                            sql += ") VALUES (";
                            sql += "@GUID,";
                            sql += "@Socket,";
                            sql += "@Type,";
                            sql += "@IPFrom,";
                            sql += "@IPTo,";
                            sql += "@Buffer";
                            sql += ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@GUID", si.SID.ToString().ToUpper());
                                cmd.Parameters.AddWithValue("@Socket", pi.PacketSocket);
                                cmd.Parameters.AddWithValue("@Type", pi.PacketType);
                                cmd.Parameters.AddWithValue("@IPFrom", pi.PacketFrom);
                                cmd.Parameters.AddWithValue("@IPTo", pi.PacketTo);
                                cmd.Parameters.AddWithValue("@Buffer", pi.PacketBuffer);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_Send), ex.Message);
                }
            }

            #endregion

            #region//机器人列表

            private static bool CreateTable_Robot()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS Robot (";
                        sql += "GUID TEXT NOT NULL PRIMARY KEY,";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";
                        sql += "Name TEXT NOT NULL";
                        sql += ");";

                        sql += "CREATE TABLE IF NOT EXISTS RobotInstruction (";
                        sql += "GUID TEXT NOT NULL,";
                        sql += "Type INTEGER NOT NULL,";
                        sql += "Content TEXT,";
                        sql += "FOREIGN KEY (GUID) REFERENCES Robot(GUID)";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_Robot), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_Robot()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM Robot;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_Robot), ex.Message);
                }

                return dtReturn;
            }

            public static DataTable SelectTable_RobotInstruction(Guid guid)
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT Type, Content FROM RobotInstruction WHERE GUID = @GUID;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", guid.ToString().ToUpper());

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_RobotInstruction), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_Robot()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM RobotInstruction;";
                        sql += "DELETE FROM Robot;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_Robot), ex.Message);
                }
            }

            public static void InsertTable_Robot(RobotInfo ri)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        conn.Open();

                        string sql = "INSERT INTO Robot (";
                        sql += "GUID,";
                        sql += "IsEnable,";
                        sql += "Name";
                        sql += ") VALUES (";
                        sql += "@GUID,";
                        sql += "@IsEnable,";
                        sql += "@Name";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", ri.RID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@IsEnable", ri.IsEnable);
                            cmd.Parameters.AddWithValue("@Name", ri.RName);
                            cmd.ExecuteNonQuery();
                        }

                        foreach (InstructionInfo ii in ri.RInstruction)
                        {
                            sql = "INSERT INTO RobotInstruction (";
                            sql += "GUID,";
                            sql += "Type,";
                            sql += "Content";
                            sql += ") VALUES (";
                            sql += "@GUID,";
                            sql += "@Type,";
                            sql += "@Content";
                            sql += ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@GUID", ri.RID.ToString().ToUpper());
                                cmd.Parameters.AddWithValue("@Type", ii.InstType);
                                cmd.Parameters.AddWithValue("@Content", ii.InstContent);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_Robot), ex.Message);
                }
            }

            #endregion

            #region//代理账号

            private static bool CreateTable_ProxyAccount()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS ProxyAccount (";
                        sql += "GUID TEXT NOT NULL PRIMARY KEY,";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";
                        sql += "UserName TEXT NOT NULL UNIQUE,";
                        sql += "PassWord TEXT NOT NULL,";                        
                        sql += "IsLimitLinks BOOLEAN DEFAULT 0,";
                        sql += "LimitLinks INTEGER DEFAULT 1,";
                        sql += "IsLimitDevices BOOLEAN DEFAULT 0,";
                        sql += "LimitDevices INTEGER DEFAULT 1,";
                        sql += "IsExpiry BOOLEAN DEFAULT 0,";
                        sql += "ExpiryTime TIMESTAMP,";
                        sql += "CreateTime TIMESTAMP";
                        sql += ");";

                        sql += "CREATE TABLE IF NOT EXISTS ProxyAccountIPInfo (";
                        sql += "GUID TEXT NOT NULL,";
                        sql += "LoginTime TIMESTAMP,";
                        sql += "LoginIP TEXT,";
                        sql += "FOREIGN KEY (GUID) REFERENCES ProxyAccount(GUID),";
                        sql += "UNIQUE (GUID, LoginIP)";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_ProxyAccount), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_ProxyAccount()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM ProxyAccount;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_ProxyAccount), ex.Message);
                }

                return dtReturn;
            }

            public static DataTable SelectTable_ProxyAccountIPInfo(Guid guid)
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM ProxyAccountIPInfo WHERE GUID = @GUID;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GUID", guid.ToString().ToUpper());

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_ProxyAccountIPInfo), ex.Message);
                }

                return dtReturn;
            }

            public static bool DeleteTable_ProxyAccount(Guid guid)
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DataBase.conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sqlDeleteIPInfo = @"
                                DELETE FROM ProxyAccountIPInfo 
                                WHERE GUID = @GUID;";

                            string sqlDeleteAccount = @"
                                DELETE FROM ProxyAccount 
                                WHERE GUID = @GUID;";

                            using (SQLiteCommand cmdDeleteIPInfo = new SQLiteCommand(sqlDeleteIPInfo, conn, transaction))
                            using (SQLiteCommand cmdDeleteAccount = new SQLiteCommand(sqlDeleteAccount, conn, transaction))
                            {
                                cmdDeleteIPInfo.Parameters.Add(new SQLiteParameter("@GUID", DbType.String));
                                cmdDeleteAccount.Parameters.Add(new SQLiteParameter("@GUID", DbType.String));

                                string formattedGuid = guid.ToString().ToUpper();
                                cmdDeleteIPInfo.Parameters["@GUID"].Value = formattedGuid;
                                cmdDeleteAccount.Parameters["@GUID"].Value = formattedGuid;

                                cmdDeleteIPInfo.ExecuteNonQuery();

                                int rowsAffected = cmdDeleteAccount.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    transaction.Commit();
                                    bReturn = true;
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_ProxyAccount), ex.Message);
                }

                return bReturn;
            }

            public static bool DeleteTable_ProxyAccount()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DataBase.conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sqlDeleteIPInfo = @"
                                DELETE FROM ProxyAccountIPInfo;";

                            string sqlDeleteAccount = @"
                                DELETE FROM ProxyAccount;";

                            using (SQLiteCommand cmdDeleteIPInfo = new SQLiteCommand(sqlDeleteIPInfo, conn, transaction))
                            using (SQLiteCommand cmdDeleteAccount = new SQLiteCommand(sqlDeleteAccount, conn, transaction))
                            {
                                cmdDeleteIPInfo.ExecuteNonQuery();

                                int rowsAffected = cmdDeleteAccount.ExecuteNonQuery();

                                transaction.Commit();
                                bReturn = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_ProxyAccount), ex.Message);
                }

                return bReturn;
            }

            public static bool InsertTable_ProxyAccount(AccountInfo ai)
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DataBase.conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sqlCheck = @"
                                SELECT COUNT(1) FROM ProxyAccount 
                                WHERE GUID = @GUID OR UserName = @UserName;";

                            string sqlAccount = @"
                                INSERT INTO ProxyAccount (
                                    GUID, IsEnable, UserName, PassWord, 
                                    IsLimitLinks, LimitLinks, IsLimitDevices, LimitDevices, 
                                    IsExpiry, ExpiryTime, CreateTime
                                ) VALUES (
                                    @GUID, @IsEnable, @UserName, @PassWord, 
                                    @IsLimitLinks, @LimitLinks, @IsLimitDevices, @LimitDevices, 
                                    @IsExpiry, @ExpiryTime, @CreateTime
                                );";

                            string sqlIPInfo = @"
                                INSERT INTO ProxyAccountIPInfo (
                                    GUID, LoginTime, LoginIP
                                ) VALUES (
                                    @GUID, @LoginTime, @LoginIP
                                );";

                            using (SQLiteCommand cmdCheck = new SQLiteCommand(sqlCheck, conn, transaction))
                            using (SQLiteCommand cmdAccount = new SQLiteCommand(sqlAccount, conn, transaction))
                            using (SQLiteCommand cmdIPInfo = new SQLiteCommand(sqlIPInfo, conn, transaction))
                            {
                                cmdCheck.Parameters.Add(new SQLiteParameter("@GUID", DbType.String));
                                cmdCheck.Parameters.Add(new SQLiteParameter("@UserName", DbType.String));

                                string guid = ai.AID.ToString().ToUpper();
                                cmdCheck.Parameters["@GUID"].Value = guid;
                                cmdCheck.Parameters["@UserName"].Value = ai.UserName;

                                long existingCount = (long)cmdCheck.ExecuteScalar();
                                if (existingCount > 0)
                                {
                                    transaction.Rollback();
                                    return false;
                                }

                                cmdAccount.Parameters.Add(new SQLiteParameter("@GUID", DbType.String));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@IsEnable", DbType.Boolean));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@UserName", DbType.String));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@PassWord", DbType.String));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@IsLimitLinks", DbType.Boolean));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@LimitLinks", DbType.Int32));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@IsLimitDevices", DbType.Boolean));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@LimitDevices", DbType.Int32));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@IsExpiry", DbType.Boolean));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@ExpiryTime", DbType.DateTime));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@CreateTime", DbType.DateTime));

                                cmdIPInfo.Parameters.Add(new SQLiteParameter("@GUID", DbType.String));
                                cmdIPInfo.Parameters.Add(new SQLiteParameter("@LoginTime", DbType.DateTime));
                                cmdIPInfo.Parameters.Add(new SQLiteParameter("@LoginIP", DbType.String));

                                cmdAccount.Parameters["@GUID"].Value = guid;
                                cmdAccount.Parameters["@IsEnable"].Value = ai.IsEnable;
                                cmdAccount.Parameters["@UserName"].Value = ai.UserName;
                                cmdAccount.Parameters["@PassWord"].Value = ai.Password;
                                cmdAccount.Parameters["@IsLimitLinks"].Value = ai.IsLimitLinks;
                                cmdAccount.Parameters["@LimitLinks"].Value = ai.LimitLinks;
                                cmdAccount.Parameters["@IsLimitDevices"].Value = ai.IsLimitDevices;
                                cmdAccount.Parameters["@LimitDevices"].Value = ai.LimitDevices;
                                cmdAccount.Parameters["@IsExpiry"].Value = ai.IsExpiry;
                                cmdAccount.Parameters["@ExpiryTime"].Value = ai.ExpiryTime;
                                cmdAccount.Parameters["@CreateTime"].Value = ai.CreateTime;

                                int rowsAffected = cmdAccount.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    if (ai.AIPInfo != null && ai.AIPInfo.Count > 0)
                                    {
                                        foreach (AccountIPInfo ipInfo in ai.AIPInfo)
                                        {
                                            cmdIPInfo.Parameters["@GUID"].Value = guid;
                                            cmdIPInfo.Parameters["@LoginTime"].Value = ipInfo.LoginTime;
                                            cmdIPInfo.Parameters["@LoginIP"].Value = ipInfo.LoginIP;

                                            cmdIPInfo.ExecuteNonQuery();
                                        }
                                    }

                                    transaction.Commit();
                                    bReturn = true;
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_ProxyAccount), ex.Message);
                }

                return bReturn;
            }

            public static bool UpdateTable_ProxyAccount(AccountInfo ai)
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DataBase.conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sqlAccount = @"
                                UPDATE ProxyAccount 
                                SET 
                                    IsEnable = @IsEnable,
                                    PassWord = @PassWord,
                                    IsLimitLinks = @IsLimitLinks,
                                    LimitLinks = @LimitLinks,
                                    IsLimitDevices = @IsLimitDevices,
                                    LimitDevices = @LimitDevices,
                                    IsExpiry = @IsExpiry,
                                    ExpiryTime = @ExpiryTime
                                WHERE GUID = @GUID;";

                            using (SQLiteCommand cmdAccount = new SQLiteCommand(sqlAccount, conn, transaction))
                            {
                                cmdAccount.Parameters.Add(new SQLiteParameter("@GUID", DbType.String));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@IsEnable", DbType.Boolean));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@PassWord", DbType.String));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@IsLimitLinks", DbType.Boolean));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@LimitLinks", DbType.Int32));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@IsLimitDevices", DbType.Boolean));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@LimitDevices", DbType.Int32));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@IsExpiry", DbType.Boolean));
                                cmdAccount.Parameters.Add(new SQLiteParameter("@ExpiryTime", DbType.DateTime));

                                cmdAccount.Parameters["@GUID"].Value = ai.AID.ToString().ToUpper();
                                cmdAccount.Parameters["@IsEnable"].Value = ai.IsEnable;
                                cmdAccount.Parameters["@PassWord"].Value = ai.Password;
                                cmdAccount.Parameters["@IsLimitLinks"].Value = ai.IsLimitLinks;
                                cmdAccount.Parameters["@LimitLinks"].Value = ai.LimitLinks;
                                cmdAccount.Parameters["@IsLimitDevices"].Value = ai.IsLimitDevices;
                                cmdAccount.Parameters["@LimitDevices"].Value = ai.LimitDevices;
                                cmdAccount.Parameters["@IsExpiry"].Value = ai.IsExpiry;
                                cmdAccount.Parameters["@ExpiryTime"].Value = ai.ExpiryTime;

                                int rowsAffected = cmdAccount.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    transaction.Commit();
                                    bReturn = true;
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(UpdateTable_ProxyAccount), ex.Message);
                }

                return bReturn;
            }

            #endregion

            #region//本地代理映射

            private static bool CreateTable_ProxyMapLocal()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS ProxyMapLocal (";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";
                        sql += "ProtocolType TEXT NOT NULL,";
                        sql += "Host TEXT NOT NULL,";
                        sql += "Port INTEGER DEFAULT 80,";
                        sql += "RemotePath TEXT,";
                        sql += "LocalPath TEXT NOT NULL";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_ProxyMapLocal), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_ProxyMapLocal()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM ProxyMapLocal;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_ProxyMapLocal), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_ProxyMapLocal()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM ProxyMapLocal;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_ProxyMapLocal), ex.Message);
                }
            }

            public static void InsertTable_ProxyMapLocal()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DataBase.conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sql = "INSERT INTO ProxyMapLocal (" +
                                        "IsEnable, ProtocolType, Host, Port, RemotePath, LocalPath" +
                                        ") VALUES (" +
                                        "@IsEnable, @ProtocolType, @Host, @Port, @RemotePath, @LocalPath" +
                                        ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.Add(new SQLiteParameter("@IsEnable", DbType.Boolean));
                                cmd.Parameters.Add(new SQLiteParameter("@ProtocolType", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@Host", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@Port", DbType.Int32));
                                cmd.Parameters.Add(new SQLiteParameter("@RemotePath", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@LocalPath", DbType.String));

                                foreach (MapLocal pml in ProxyConfig.Mapping.lstMapLocal)
                                {
                                    cmd.Parameters["@IsEnable"].Value = pml.IsEnable;
                                    cmd.Parameters["@ProtocolType"].Value = pml.ProtocolType;
                                    cmd.Parameters["@Host"].Value = pml.Host;
                                    cmd.Parameters["@Port"].Value = pml.Port;
                                    cmd.Parameters["@RemotePath"].Value = pml.RemotePath ?? (object)DBNull.Value;
                                    cmd.Parameters["@LocalPath"].Value = pml.LocalPath;

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_ProxyMapLocal), ex.Message);
                }
            }

            #endregion

            #region//远程代理映射

            private static bool CreateTable_ProxyMapRemote()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS ProxyMapRemote (";
                        sql += "IsEnable BOOLEAN DEFAULT 0,";
                        sql += "ProtocolType_From TEXT NOT NULL,";
                        sql += "Host_From TEXT NOT NULL,";
                        sql += "Port_From INTEGER DEFAULT 80,";
                        sql += "Path_From TEXT,";
                        sql += "ProtocolType_To TEXT NOT NULL,";
                        sql += "Host_To TEXT NOT NULL,";
                        sql += "Port_To INTEGER DEFAULT 80,";
                        sql += "Path_To TEXT";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_ProxyMapRemote), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_ProxyMapRemote()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM ProxyMapRemote;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_ProxyMapRemote), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_ProxyMapRemote()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM ProxyMapRemote;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_ProxyMapRemote), ex.Message);
                }
            }

            public static void InsertTable_ProxyMapRemote()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DataBase.conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sql = "INSERT INTO ProxyMapRemote (" +
                                        "IsEnable, ProtocolType_From, Host_From, Port_From, Path_From, " +
                                        "ProtocolType_To, Host_To, Port_To, Path_To" +
                                        ") VALUES (" +
                                        "@IsEnable, @ProtocolType_From, @Host_From, @Port_From, @Path_From, " +
                                        "@ProtocolType_To, @Host_To, @Port_To, @Path_To" +
                                        ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.Add(new SQLiteParameter("@IsEnable", DbType.Boolean));
                                cmd.Parameters.Add(new SQLiteParameter("@ProtocolType_From", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@Host_From", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@Port_From", DbType.Int32));
                                cmd.Parameters.Add(new SQLiteParameter("@Path_From", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@ProtocolType_To", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@Host_To", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@Port_To", DbType.Int32));
                                cmd.Parameters.Add(new SQLiteParameter("@Path_To", DbType.String));

                                foreach (MapRemote pmr in ProxyConfig.Mapping.lstMapRemote)
                                {
                                    cmd.Parameters["@IsEnable"].Value = pmr.IsEnable;
                                    cmd.Parameters["@ProtocolType_From"].Value = pmr.ProtocolTypeFrom.ToString();
                                    cmd.Parameters["@Host_From"].Value = pmr.HostFrom;
                                    cmd.Parameters["@Port_From"].Value = pmr.PortFrom;
                                    cmd.Parameters["@Path_From"].Value = pmr.PathFrom ?? (object)DBNull.Value;
                                    cmd.Parameters["@ProtocolType_To"].Value = pmr.ProtocolTypeTo.ToString();
                                    cmd.Parameters["@Host_To"].Value = pmr.HostTo;
                                    cmd.Parameters["@Port_To"].Value = pmr.PortTo;
                                    cmd.Parameters["@Path_To"].Value = pmr.PathTo ?? (object)DBNull.Value;

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_ProxyMapRemote), ex.Message);
                }
            }

            #endregion

            #region//白名单

            private static bool CreateTable_WhiteList()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS WhiteList (";
                        sql += "IPAddress TEXT NOT NULL UNIQUE,";
                        sql += "StartIP INTEGER DEFAULT 0,";
                        sql += "EndIP INTEGER DEFAULT 0,";
                        sql += "IsExpiry BOOLEAN DEFAULT 0,";
                        sql += "ExpiryTime TIMESTAMP,";
                        sql += "CreateTime TIMESTAMP";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_WhiteList), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_WhiteList()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM WhiteList;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_WhiteList), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_WhiteList()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM WhiteList;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_WhiteList), ex.Message);
                }
            }

            public static void InsertTable_WhiteList()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sql = "INSERT INTO WhiteList (" +
                                        "IPAddress, StartIP, EndIP, IsExpiry, ExpiryTime, CreateTime" +
                                        ") VALUES (" +
                                        "@IPAddress, @StartIP, @EndIP, @IsExpiry, @ExpiryTime, @CreateTime" +
                                        ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.Add(new SQLiteParameter("@IPAddress", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@StartIP", DbType.Int64));
                                cmd.Parameters.Add(new SQLiteParameter("@EndIP", DbType.Int64));
                                cmd.Parameters.Add(new SQLiteParameter("@IsExpiry", DbType.Boolean));
                                cmd.Parameters.Add(new SQLiteParameter("@ExpiryTime", DbType.DateTime));
                                cmd.Parameters.Add(new SQLiteParameter("@CreateTime", DbType.DateTime));

                                foreach (WhiteListInfo wli in Operate.ProxyConfig.Proxy.lstWhiteList)
                                {
                                    cmd.Parameters["@IPAddress"].Value = wli.IPAddress;
                                    cmd.Parameters["@StartIP"].Value = wli.StartIP;
                                    cmd.Parameters["@EndIP"].Value = wli.EndIP;
                                    cmd.Parameters["@IsExpiry"].Value = wli.IsExpiry;
                                    cmd.Parameters["@ExpiryTime"].Value = wli.ExpiryTime;
                                    cmd.Parameters["@CreateTime"].Value = wli.CreateTime;

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_WhiteList), ex.Message);
                }
            }

            #endregion

            #region //黑名单

            private static bool CreateTable_BlackList()
            {
                bool bReturn = false;

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "CREATE TABLE IF NOT EXISTS BlackList (";
                        sql += "IPAddress TEXT NOT NULL UNIQUE,";
                        sql += "StartIP INTEGER DEFAULT 0,";
                        sql += "EndIP INTEGER DEFAULT 0,";
                        sql += "IsExpiry BOOLEAN DEFAULT 0,";
                        sql += "ExpiryTime TIMESTAMP,";
                        sql += "CreateTime TIMESTAMP";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    bReturn = true;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(CreateTable_BlackList), ex.Message);
                }

                return bReturn;
            }

            public static DataTable SelectTable_BlackList()
            {
                DataTable dtReturn = new DataTable();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "SELECT * FROM BlackList;";

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                        {
                            adapter.Fill(dtReturn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(SelectTable_BlackList), ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_BlackList()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM BlackList;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(DeleteTable_BlackList), ex.Message);
                }
            }

            public static void InsertTable_BlackList()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            string sql = "INSERT INTO BlackList (" +
                                        "IPAddress, StartIP, EndIP, IsExpiry, ExpiryTime, CreateTime" +
                                        ") VALUES (" +
                                        "@IPAddress, @StartIP, @EndIP, @IsExpiry, @ExpiryTime, @CreateTime" +
                                        ");";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.Add(new SQLiteParameter("@IPAddress", DbType.String));
                                cmd.Parameters.Add(new SQLiteParameter("@StartIP", DbType.Int64));
                                cmd.Parameters.Add(new SQLiteParameter("@EndIP", DbType.Int64));
                                cmd.Parameters.Add(new SQLiteParameter("@IsExpiry", DbType.Boolean));
                                cmd.Parameters.Add(new SQLiteParameter("@ExpiryTime", DbType.DateTime));
                                cmd.Parameters.Add(new SQLiteParameter("@CreateTime", DbType.DateTime));

                                foreach (BlackListInfo bli in Operate.ProxyConfig.Proxy.lstBlackList)
                                {
                                    cmd.Parameters["@IPAddress"].Value = bli.IPAddress;
                                    cmd.Parameters["@StartIP"].Value = bli.StartIP;
                                    cmd.Parameters["@EndIP"].Value = bli.EndIP;
                                    cmd.Parameters["@IsExpiry"].Value = bli.IsExpiry;
                                    cmd.Parameters["@ExpiryTime"].Value = bli.ExpiryTime;
                                    cmd.Parameters["@CreateTime"].Value = bli.CreateTime;

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(InsertTable_BlackList), ex.Message);
                }
            }

            #endregion
        }

        #endregion
    }
}
