using AntdUI;
using Be.Windows.Forms;
using Microsoft.Owin.Hosting;
using Microsoft.Win32;
using QQWry;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Reflection;
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
            public static Color col_Del = Color.Red;
            public static Color col_Add = Color.Green;
            public static string PNAME = string.Empty;
            public static string PATH = string.Empty;
            public static string WebSite_Tutorials_CN = "https://www.wpe64.com/tutorials.html";
            public static string WebSite_Tutorials_EN = "https://www.wpe64.com/tutorials_enUS.html";            
            public static string LastInjection = string.Empty;
            public static string WPE64_URL = "https://www.wpe64.com";
            public static string WPE64_IP = "http://101.132.222.195";
            public static string WPE64_Issuse = "https://github.com/x-nas/WinsockPacketEditor/issues";
            public static string WPE64_DLL = "WPEHook.dll";
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
            public static SystemMode StartMode = SystemMode.None;
            public static DateTime StartTime = DateTime.Now;
            public static IntPtr MainHandle = IntPtr.Zero;
            public static int SystemSocket = 0;
            public static bool ShowDebug = false;
            public static bool IsRemote = false;
            public static string Remote_URL, Remote_UserName, Remote_PassWord;
            public static ushort Remote_Port = 88;
            public static IDisposable WebServer;
            public static PerformanceCounter cpuCounter;
            public static bool IsShow_FloatButton = true;
            public static bool IsShow_TextCompare = false, IsShow_TextDuplicate = false;
            public static Execute ListExecute = Execute.Sequence;
            public static bool CheckNotShow = true, CheckLen, CheckSocket, CheckIP, CheckPort, CheckHead, CheckData;
            public static string CheckSocket_Value, CheckLength_Value, CheckIP_Value, CheckPort_Value, CheckHead_Value, CheckData_Value;         
            public static readonly Font FontUnderline = new Font(RichTextBox.DefaultFont, FontStyle.Underline);
            public static readonly Font FontStrikeout = new Font(RichTextBox.DefaultFont, FontStyle.Strikeout);

            #region//结构定义           

            public enum SystemMode
            {
                None = 0,
                Process = 1,
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

            #region//国家对应SVG字典

            private static readonly ConcurrentDictionary<string, string> SvgCache = new ConcurrentDictionary<string, string>();

            private static readonly Dictionary<string, string> CountryNameToCode = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
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
            };

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
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return leReturn;
            }

            #endregion          

            #region//获取启动模式

            public static SystemMode GetSystemMode_ByString(string smMode)
            {
                SystemMode systemMode = SystemMode.None;

                try
                {
                    systemMode = (SystemMode)Enum.Parse(typeof(SystemMode), smMode);
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return systemMode;
            }

            #endregion

            #region//获取本机的本地IP地址

            public static IPAddress[] GetLocalIPAddress()
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                    .SelectMany(nic => nic.GetIPProperties().UnicastAddresses)
                    .Where(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(addr => addr.Address)
                    .ToArray();
            }

            #endregion

            #region//获取列表的右键菜单

            public static AntdUI.IContextMenuStripItem[] GetCMS_List()
            {                
                List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                menuItems.Add(new AntdUI.ContextMenuStripItem("置顶", "Ctrl+向上键")
                {
                    ID = "Top",
                    IconSvg = "VerticalAlignTopOutlined",
                    LocalizationText = "InjectModeForm.cmsFilterList.Top",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                menuItems.Add(new AntdUI.ContextMenuStripItem("向上移动", "Alt+向上键")
                {
                    ID = "Up",
                    IconSvg = "ArrowUpOutlined",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItem("向下移动", "Alt+向下键")
                {
                    ID = "Down",
                    IconSvg = "ArrowDownOutlined",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                menuItems.Add(new AntdUI.ContextMenuStripItem("置底", "Ctrl+向下键")
                {
                    ID = "Bottom",
                    IconSvg = "VerticalAlignBottomOutlined",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                menuItems.Add(new AntdUI.ContextMenuStripItem("复制")
                {
                    ID = "Copy",
                    IconSvg = "CopyOutlined",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItem("导出到文件")
                {
                    ID = "Export",
                    IconSvg = "DeliveredProcedureOutlined",
                });
                menuItems.Add(new AntdUI.ContextMenuStripItem("删除")
                {
                    ID = "Delete",
                    IconSvg = "DeleteOutlined",
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
                });

                menuItems.Add(new AntdUI.ContextMenuStripItem("复制")
                {
                    Enabled = hbPacketData.CanCopy(),
                    ID = "Copy",
                    IconSvg = "CopyOutlined",                    
                });

                menuItems.Add(new AntdUI.ContextMenuStripItem("粘贴")
                {
                    Enabled = hbPacketData.CanPaste(),
                    ID = "Paste",
                    IconSvg = "SnippetsOutlined",                    
                });

                menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                menuItems.Add(new AntdUI.ContextMenuStripItem("全选")
                {
                    ID = "SelectAll",
                    IconSvg = "ProfileOutlined",
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    switch (Operate.SystemConfig.StartMode)
                    {
                        case Operate.SystemConfig.SystemMode.Proxy:
                            sReturn = AntdUI.Localization.Get("Proxy Mode", "代理模式");
                            break;

                        case Operate.SystemConfig.SystemMode.Process:
                            sReturn = AntdUI.Localization.Get("Inject Mode", "注入模式");
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
                        sReturn = AntdUI.Localization.Get("Speed Mode", "极速模式");
                    }
                    else
                    {
                        sReturn = AntdUI.Localization.Get("Normal Mode", "普通模式");
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
                return sReturn;
            }

            #endregion

            #region//获取IP的所属地

            public static async Task<string> GetIPLocation(string IPString)
            {
                try
                {
                    var IPSearch = await ProxyConfig.Proxy.ipSearch.GetIpLocationAsync(IPString);

                    if (IPSearch.Country.Equals("IANA"))
                    {
                        return IPSearch.Area;
                    }
                    else
                    {
                        return IPSearch.Country + IPSearch.Area;
                    }
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return string.Empty;
            }

            #endregion

            #region//获取IP所属地图标

            public static string GetSvgByLocation(string IPLocation)
            {
                try
                {
                    if (string.IsNullOrEmpty(IPLocation))
                        return GetDefaultSvg();

                    foreach (var pair in CountryNameToCode)
                    {
                        if (IPLocation.StartsWith(pair.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            return SvgCache.GetOrAdd(pair.Value, code => GetSvgByCountryCode(code));
                        }
                    }
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }                

                return GetDefaultSvg();
            }

            private static string GetSvgByCountryCode(string countryCode)
            {
                try
                {
                    var resource = Properties.Resources.ResourceManager.GetObject(countryCode.ToLower()) as byte[];
                    return resource != null
                        ? Encoding.UTF8.GetString(resource)
                        : GetDefaultSvg();
                }
                catch
                {
                    return GetDefaultSvg();
                }
            }

            private static string GetDefaultSvg()
            {
                return Properties.Resources.Flag_Local;
            }

            #endregion

            #region//初始化悬浮按钮

            public static void InitFloatButton(Form form, AntdUI.FormFloatButton FloatButton)
            {
                if (SystemConfig.IsShow_FloatButton)
                {
                    if (FloatButton == null)
                    {
                        FloatButton = AntdUI.FloatButton.open(new AntdUI.FloatButton.Config(form,
                            new AntdUI.FloatButton.ConfigBtn[]
                            {
                            new AntdUI.FloatButton.ConfigBtn("GitHub", "QuestionOutlined", true)
                    {
                        Tooltip = "问题反馈",
                        Type= AntdUI.TTypeMini.Success
                    },
                            new AntdUI.FloatButton.ConfigBtn("WebSite", "HomeOutlined", true)
                    {
                        Tooltip = "访问官网",
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
                        FloatButton.Show();
                    }
                }
                else
                {
                    if (FloatButton != null)
                    {
                        FloatButton.Close();
                        FloatButton = null;
                    }
                }
            }

            #endregion

            #region//查找树节点

            public static TreeItem FindNodeByName(AntdUI.Tree tree, string name)
            {
                try
                {
                    return FindNodeByName(tree.Items, name);
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
                
                return null;
            }

            public static TreeItem FindNodeByName(TreeItemCollection items, string name)
            {
                try
                {
                    if (items == null || items.Count == 0)
                    {
                        return null;
                    } 

                    foreach (var item in items)
                    {
                        if (item.Name == name || item.Text == name)
                        {
                            return item;
                        }

                        var found = FindNodeByName(item.Sub, name);
                        if (found != null)
                        {
                            return found;
                        } 
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }                

                return null;
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
                                Operate.SystemConfig.WebServer = WebApp.Start<Socket_Web>(Operate.SystemConfig.Remote_URL);
                                ProxyConfig.Proxy.InitCCProxy_HTML();

                                sLog = string.Format(AntdUI.Localization.Get("MGT.Enabled", "远程管理已启用：{0}"), Operate.SystemConfig.Remote_URL);
                            }
                            catch
                            {
                                sLog = string.Format(AntdUI.Localization.Get("MGT.Error", "远程管理启动失败: 请使用管理员权限启动 {0}"), Process.GetCurrentProcess().ProcessName);
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

            public static void StopRemoteMGT()
            {
                try
                {
                    if (Operate.SystemConfig.WebServer != null)
                    {
                        Operate.SystemConfig.WebServer.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    return IsHexString(value);
                }
                else
                {
                    return false;
                }
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

            #region//文本对比

            public static string CompareData(Font font, string sText_A, string sText_B)
            {
                string sReturn = string.Empty;

                try
                {
                    using (RichTextBox rtbCompare = new RichTextBox())
                    {
                        rtbCompare.Font = font;

                        if (sText_A == sText_B)
                        {
                            SystemConfig.AppendColoredText(rtbCompare, AntdUI.Localization.Get("System.Compare.Same", "两个数据相同"), Color.RoyalBlue);
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
                                    SystemConfig.AppendColoredText(rtbCompare, linesA[la], SystemConfig.col_Del);
                                }
                                else if (linesA[la] == linesB[lb])
                                {
                                    SystemConfig.AppendColoredText(rtbCompare, linesA[la], rtbCompare.ForeColor);
                                }
                                else
                                {
                                    if ((lb + 1 < linesB.Length) && (linesA[la] == linesB[lb + 1]))
                                    {
                                        SystemConfig.AppendColoredText(rtbCompare, linesB[lb], SystemConfig.col_Add);
                                        SystemConfig.AppendColoredText(rtbCompare, "\n" + linesA[la], rtbCompare.ForeColor);

                                        lb++;
                                    }
                                    else if ((la + 1 < linesA.Length) && (linesA[la + 1] == linesB[lb]))
                                    {
                                        SystemConfig.AppendColoredText(rtbCompare, linesA[la], SystemConfig.col_Del);
                                        SystemConfig.AppendColoredText(rtbCompare, "\n" + linesB[lb], rtbCompare.ForeColor);

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
                                                SystemConfig.AppendColoredText(rtbCompare, wordsA[wa], SystemConfig.col_Del);
                                            }
                                            else if (wordsA[wa] == wordsB[wb])
                                            {
                                                SystemConfig.AppendColoredText(rtbCompare, wordsA[wa], rtbCompare.ForeColor);
                                            }
                                            else
                                            {
                                                if ((wb + 1 < wordsB.Length) && (wordsA[wa] == wordsB[wb + 1]))
                                                {
                                                    SystemConfig.AppendColoredText(rtbCompare, wordsB[wb], SystemConfig.col_Add);
                                                    SystemConfig.AppendColoredText(rtbCompare, " " + wordsA[wa], rtbCompare.ForeColor);

                                                    wb++;
                                                }
                                                else if ((wa + 1 < wordsA.Length) && (wordsA[wa + 1] == wordsB[wb]))
                                                {
                                                    SystemConfig.AppendColoredText(rtbCompare, wordsA[wa], SystemConfig.col_Del);
                                                    SystemConfig.AppendColoredText(rtbCompare, " " + wordsB[wb], rtbCompare.ForeColor);

                                                    wa++;
                                                }
                                                else
                                                {
                                                    SystemConfig.AppendColoredText(rtbCompare, wordsA[wa], SystemConfig.col_Del);
                                                    SystemConfig.AppendColoredText(rtbCompare, wordsB[wb], SystemConfig.col_Add);
                                                }
                                            }
                                            if (wa + 1 < wordsA.Length) SystemConfig.AppendColoredText(rtbCompare, " ", rtbCompare.ForeColor);

                                            if ((wordsB.Length >= wordsA.Length) && (wa + 1 == wordsA.Length))
                                            {
                                                while (wb + 1 < wordsB.Length)
                                                {
                                                    wb++;

                                                    SystemConfig.AppendColoredText(rtbCompare, " ", rtbCompare.ForeColor);
                                                    SystemConfig.AppendColoredText(rtbCompare, wordsB[wb], SystemConfig.col_Add);
                                                }
                                            }

                                            wa++;
                                            wb++;
                                        }
                                    }
                                }

                                if (la + 1 < linesA.Length)
                                {
                                    SystemConfig.AppendColoredText(rtbCompare, "\n", rtbCompare.ForeColor);
                                }

                                if ((linesB.Length >= linesA.Length) && (la + 1 == linesA.Length))
                                {
                                    while (lb + 1 < linesB.Length)
                                    {
                                        lb++;

                                        SystemConfig.AppendColoredText(rtbCompare, "\n", rtbCompare.ForeColor);
                                        SystemConfig.AppendColoredText(rtbCompare, linesB[lb], SystemConfig.col_Add);
                                    }
                                }

                                la++;
                                lb++;
                            }
                        }

                        sReturn = rtbCompare.Rtf;                        
                    }
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

                    if (color == SystemConfig.col_Add)
                    {
                        box.SelectionFont = SystemConfig.FontUnderline;
                    }

                    if (color == SystemConfig.col_Del)
                    {
                        box.SelectionFont = SystemConfig.FontStrikeout;
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

            public static List<AntdUI.Input.TextStyle> ConvertRtfToTextStyles(string rtfString)
            {
                var styles = new List<AntdUI.Input.TextStyle>();

                using (var rtb = new RichTextBox())
                {
                    rtb.Rtf = rtfString;
                    string plainText = rtb.Text;

                    for (int i = 0; i < plainText.Length; i++)
                    {
                        rtb.Select(i, 1);
                        Font currentFont = rtb.SelectionFont;
                        Color currentColor = rtb.SelectionColor;
                        Color currentBackColor = rtb.SelectionBackColor;

                        int start = i;
                        int length = 1;

                        while (i + length < plainText.Length)
                        {
                            rtb.Select(i + length, 1);
                            if (rtb.SelectionFont.Equals(currentFont) &&
                               rtb.SelectionColor.Equals(currentColor) &&
                               rtb.SelectionBackColor.Equals(currentBackColor))
                            {
                                length++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        var backColor = currentBackColor != Color.White ? currentBackColor : (Color?)null;
                        styles.Add(new AntdUI.Input.TextStyle(start, length, currentFont, currentColor, backColor));

                        i += length - 1;
                    }
                }

                return styles;
            }

            #endregion

            #region//文本查重

            public static (string TextA, string TextB) ComparePackets(string stringA, string stringB, int minBytes)
            {
                stringA = CleanAndNormalizeHex(stringA);
                stringB = CleanAndNormalizeHex(stringB);

                List<string> bytes1 = SplitIntoBytes(stringA);
                List<string> bytes2 = SplitIntoBytes(stringB);

                var commonSequences = FindCommonSequences(bytes1, bytes2, minBytes);

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

                return (new string(result1), new string(result2));
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

            #region//支持取消的等待（异步）

            public static async Task DoSleepAsync(int MilliSecond, CancellationToken cancellationToken)
            {
                await Task.Delay(MilliSecond, cancellationToken);
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
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        new XElement("StartMode", SystemConfig.StartMode),
                        new XElement("Remote_IsEnable", SystemConfig.IsRemote),
                        new XElement("Remote_UserName", SystemConfig.Remote_UserName),
                        new XElement("Remote_PassWord", SystemConfig.Remote_PassWord),
                        new XElement("Remote_Port", SystemConfig.Remote_Port),
                        new XElement("Remote_URL", SystemConfig.Remote_URL),
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
                        new XElement("HotKey12", SystemConfig.HotKey12)
                        );

                    return xeSystemConfig;
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }

            #endregion

            #region//从数据库加载系统配置

            public static void LoadSystemConfig_FromDB()
            {
                try
                {
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
                        SystemConfig.StartMode = Operate.SystemConfig.GetSystemMode_ByString(dtSystemConfig.Rows[0]["StartMode"].ToString());
                        SystemConfig.IsRemote = Convert.ToBoolean(dtSystemConfig.Rows[0]["Remote_IsEnable"]);
                        SystemConfig.Remote_UserName = dtSystemConfig.Rows[0]["Remote_UserName"].ToString();
                        SystemConfig.Remote_PassWord = dtSystemConfig.Rows[0]["Remote_PassWord"].ToString();
                        SystemConfig.Remote_Port = ushort.Parse(dtSystemConfig.Rows[0]["Remote_Port"].ToString());
                        SystemConfig.Remote_URL = dtSystemConfig.Rows[0]["Remote_URL"].ToString();
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
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                    XElement xeStartMode = xeSystemConfig.Element("StartMode");
                    if (xeStartMode != null)
                    {
                        SystemConfig.StartMode = GetSystemMode_ByString(xeStartMode.Value);
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

                    XElement xeRemote_URL = xeSystemConfig.Element("Remote_URL");
                    if (xeRemote_URL != null)
                    {
                        SystemConfig.Remote_URL = xeRemote_URL.Value;
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
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        new XElement("PacketList_AutoClear_Value", PacketConfig.List.AutoClear_Value),                        
                        new XElement("SpeedMode", PacketConfig.Packet.SpeedMode)                        
                        );

                    return xeInjectMode;
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        
                        PacketConfig.Packet.SpeedMode = Convert.ToBoolean(InjectMode.Rows[0]["SpeedMode"]);                        
                    }
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                    XElement SpeedMode = xeInjectMode.Element("SpeedMode");
                    if (SpeedMode != null)
                    {
                        PacketConfig.Packet.SpeedMode = Convert.ToBoolean(SpeedMode.Value);
                    }                    
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        new XElement("ProxyPort", ProxyConfig.Proxy.ProxyPort),
                        new XElement("Enable_Auth", ProxyConfig.Proxy.Enable_Auth),                   
                        new XElement("ProxyList_AutoRoll", ProxyConfig.List.AutoRoll),
                        new XElement("ProxyList_AutoClear", ProxyConfig.List.AutoClear),
                        new XElement("ProxyList_AutoClear_Value", ProxyConfig.List.AutoClear_Value),                        
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
                        new XElement("SpeedMode", ProxyConfig.Proxy.SpeedMode)
                        );

                    return xeProxyMode;
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        ProxyConfig.Proxy.ProxyPort = ushort.Parse(ProxyMode.Rows[0]["ProxyPort"].ToString());
                        ProxyConfig.Proxy.Enable_Auth = Convert.ToBoolean(ProxyMode.Rows[0]["EnableAuth"]);                    
                        ProxyConfig.List.AutoRoll = Convert.ToBoolean(ProxyMode.Rows[0]["ProxyList_AutoRoll"]);
                        ProxyConfig.List.AutoClear = Convert.ToBoolean(ProxyMode.Rows[0]["ProxyList_AutoClear"]);
                        ProxyConfig.List.AutoClear_Value = Convert.ToInt32(ProxyMode.Rows[0]["ProxyList_AutoClear_Value"]);                        
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
                        ProxyConfig.Proxy.SpeedMode = Convert.ToBoolean(ProxyMode.Rows[0]["SpeedMode"]);
                    }
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                    XElement ProxyList_AutoRoll = xeProxyMode.Element("ProxyList_AutoRoll");
                    if (ProxyList_AutoRoll != null)
                    {
                        ProxyConfig.List.AutoRoll = Convert.ToBoolean(ProxyList_AutoRoll.Value);
                    }

                    XElement ProxyList_AutoClear = xeProxyMode.Element("ProxyList_AutoClear");
                    if (ProxyList_AutoClear != null)
                    {
                        ProxyConfig.List.AutoClear = Convert.ToBoolean(ProxyList_AutoClear.Value);
                    }

                    XElement ProxyList_AutoClear_Value = xeProxyMode.Element("ProxyList_AutoClear_Value");
                    if (ProxyList_AutoClear_Value != null)
                    {
                        ProxyConfig.List.AutoClear_Value = int.Parse(ProxyList_AutoClear_Value.Value);
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

                    XElement SpeedMode = xeProxyMode.Element("SpeedMode");
                    if (SpeedMode != null)
                    {
                        ProxyConfig.Proxy.SpeedMode = Convert.ToBoolean(SpeedMode.Value);
                    }
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                bool bProxyMapping,
                bool bInjectionSet,
                bool bFilterList,
                bool bSendList,
                bool bRobotList)
            {
                try
                {
                    SaveFileDialog sfdSaveFile = new SaveFileDialog();
                    sfdSaveFile.Filter = AntdUI.Localization.Get("SystemBackupFile", "系统备份文件") + "（*.sb）|*.sb";

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
                            bool DoEncrypt = false;
                            string Password = string.Empty;

                            using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Export))
                            {
                                string Title = AntdUI.Localization.Get("ExportSystemBackUp", "导出系统备份");
                                AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                {
                                    Keyboard = false,
                                    MaskClosable = false,
                                    OnOk = config =>
                                    {
                                        Password = eForm.GetPassword();
                                        if (string.IsNullOrEmpty(Password))
                                        {
                                            eForm.EncryptionText_Changed();

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

                            bool bOK = SystemConfig.ExportSystemBackUp(
                                FilePath,
                                bSystemConfig,
                                bProxySet,
                                bProxyAccount,
                                bProxyMapping,
                                bInjectionSet,
                                bFilterList,
                                bSendList,
                                bRobotList,
                                DoEncrypt,
                                Password);

                            if (bOK)
                            {
                                string Title = AntdUI.Localization.Get("InjectModeForm.ExportSystemBackUp.Success", "导出系统备份成功");
                                AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                            }
                            else
                            {
                                string Title = AntdUI.Localization.Get("InjectModeForm.ExportSystemBackUp.Error", "导出系统备份失败");
                                string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            private static bool ExportSystemBackUp(
                string FilePath,
                bool bSystemConfig,
                bool bProxySet,
                bool bProxyAccount,
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    ofdLoadFile.Filter = AntdUI.Localization.Get("WPEBackUpFile", "WPE x64 备份文件") + "（*.sb）|*.sb";
                    ofdLoadFile.RestoreDirectory = true;

                    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
                    {
                        string FilePath = ofdLoadFile.FileName;
                        if (!string.IsNullOrEmpty(FilePath))
                        {
                            if (ImportSystemBackUp(form, FilePath, true))
                            {
                                string Title = AntdUI.Localization.Get("InjectModeForm.ImportSystemBackUp.Success", "导入系统备份成功");
                                AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Import))
                                {
                                    string Title = AntdUI.Localization.Get("ImportSystemBackUp", "导入系统备份");
                                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                    {
                                        Keyboard = false,
                                        MaskClosable = false,
                                        OnOk = config =>
                                        {
                                            string sPW = eForm.GetPassword();
                                            if (string.IsNullOrEmpty(sPW))
                                            {
                                                eForm.EncryptionText_Changed();

                                                AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                                {
                                                    LocalizationText = "ImportList.Error"
                                                });

                                                return false;
                                            }
                                            else
                                            {
                                                xdoc = SystemConfig.DecryptXMLFile(FilePath, sPW);
                                                return true;
                                            }
                                        }
                                    });
                                }
                            }
                        }
                        else
                        {
                            xdoc = XDocument.Load(FilePath);
                        }

                        if (xdoc == null)
                        {
                            string sError = AntdUI.Localization.Get("System.Import.Error", "导入失败: 密码错误");
                            if (LoadFromUser)
                            {
                                AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                            }
                            else
                            {
                                Operate.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                            }

                            return false;
                        }

                        ImportSystemBackUp_FromXDocument(form, xdoc);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("Import ProxyAccountList", ex.Message);
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
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                try
                {
                    return Icon.ExtractAssociatedIcon(filePath)?.ToBitmap();
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                string sReturn = string.Empty;

                try
                {
                    sReturn = process.MainModule.FileName;
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return sReturn;
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
                    sReturn = string.Format(AntdUI.Localization.Get("System.InjectProcess", "目标进程: {0}"), Operate.PacketConfig.Packet.InjectProcess);
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                public static long ProxyTotal_CNT, TCP_Req_CNT, UDP_Req_CNT, TCP_Resp_CNT, UDP_Resp_CNT;
                public static int ProxySpeed_Uplink, ProxySpeed_Downlink;
                public static int FilterProxy_CNT = 0;
                public static IPAddress[] ProxyServerIP = null;
                public static IPAddress ProxyTCP_IP = null, ProxyUDP_IP = null;                
                public static bool SpeedMode = false;
                public static bool IsListening = false;
                public static bool ProxyIP_Auto = true;
                public static bool Enable_SystemProxy = false;
                public static bool Enable_SOCKS5 = true, Enable_Auth = true;
                public static bool Enable_ExternalProxy = false, Enable_ExternalProxy_AppointPort = false, Enable_ExternalProxy_Auth = false;
                public static string ExternalProxy_IP = "127.0.0.1";
                public static ushort ExternalProxy_Port = 8889;
                public static string ExternalProxy_AppointPort = "80,8080,443,8443", ExternalProxy_UserName, ExternalProxy_PassWord;
                public static int SocketBufferSize = 8192;
                public static ushort ProxyPort = 1080;
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

                private static readonly ConcurrentStack<SocketAsyncEventArgs> ClientArgsPool = new ConcurrentStack<SocketAsyncEventArgs>();
                private static readonly object ClientArgsLock = new object();

                private static readonly ConcurrentStack<SocketAsyncEventArgs> ServerArgsPool = new ConcurrentStack<SocketAsyncEventArgs>();
                private static readonly object ServerArgsLock = new object();

                public static readonly ConcurrentDictionary<string, IPAddress> DnsCache = new ConcurrentDictionary<string, IPAddress>(StringComparer.OrdinalIgnoreCase);
                public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

                private static SemaphoreSlim _connectionLimiter = new SemaphoreSlim(100);
                private static TimeSpan _connectionTimeout = TimeSpan.FromSeconds(30);                

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
                    Unsupport = 7,
                }

                #endregion

                #region//Args 对象池

                public static SocketAsyncEventArgs RentClientArgs(ProxyTCP pt)
                {
                    if (!ClientArgsPool.TryPop(out var args))
                    {
                        lock (ClientArgsLock)
                        {
                            args = new SocketAsyncEventArgs();
                            args.Completed += (s, e) =>
                            {
                                if (e.LastOperation == SocketAsyncOperation.Receive)
                                    ClientReceiveCompleted(s, e);
                            };
                        }
                    }

                    ResetClientArgs(args, pt);
                    return args;
                }

                private static void ResetClientArgs(SocketAsyncEventArgs args, ProxyTCP pt)
                {
                    args.UserToken = pt;
                    args.SetBuffer(pt.TCP_Client.Buffer, 0, pt.TCP_Client.Buffer.Length);
                    args.SocketError = SocketError.Success;
                    args.AcceptSocket = null;
                }

                public static void ReturnClientArgs(SocketAsyncEventArgs args)
                {
                    if (args == null) return;

                    args.UserToken = null;
                    args.SetBuffer(null, 0, 0);
                    args.Completed -= ClientReceiveCompleted;

                    ClientArgsPool.Push(args);
                }

                public static SocketAsyncEventArgs RentServerArgs(ProxyTCP pt)
                {
                    if (!ServerArgsPool.TryPop(out var args))
                    {
                        lock (ServerArgsLock)
                        {
                            args = new SocketAsyncEventArgs();
                            args.Completed += (s, e) =>
                            {
                                if (e.LastOperation == SocketAsyncOperation.Receive)
                                    ServerReceiveCompleted(s, e);
                            };
                        }
                    }

                    ResetServerArgs(args, pt);
                    return args;
                }

                private static void ResetServerArgs(SocketAsyncEventArgs args, ProxyTCP pt)
                {
                    args.UserToken = pt;
                    args.SetBuffer(pt.TCP_Server.Buffer, 0, pt.TCP_Server.Buffer.Length);
                    args.SocketError = SocketError.Success;
                    args.AcceptSocket = null;
                }

                public static void ReturnServerArgs(SocketAsyncEventArgs args)
                {
                    if (args == null) return;

                    args.UserToken = null;
                    args.SetBuffer(null, 0, 0);
                    args.Completed -= ServerReceiveCompleted;

                    ServerArgsPool.Push(args);
                }

                #endregion

                #region//接收客户端请求

                public static async Task HandleClient(Socket clientSocket)
                {
                    bool acquired = false;
                    ProxyTCP pe = null;

                    try
                    {
                        acquired = await _connectionLimiter.WaitAsync(_connectionTimeout);

                        if (acquired)
                        {
                            clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                            clientSocket.NoDelay = true;

                            pe = new ProxyTCP(clientSocket, clientSocket.ReceiveBufferSize);
                            ProxyConfig.Proxy.StartClientReceive(pe);
                        }
                        else
                        {
                            Operate.DoLog(nameof(HandleClient), "连接等待超时");
                            clientSocket?.Close();
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Operate.DoLog(nameof(HandleClient), "连接操作被取消");
                        clientSocket?.Close();
                    }
                    catch (Exception ex)
                    {
                        pe?.Dispose();
                        Operate.DoLog(nameof(HandleClient), ex.Message);
                    }
                    finally
                    {
                        if (acquired)
                        {
                            _connectionLimiter.Release();
                        }
                    }
                }

                private static void StartClientReceive(ProxyTCP pt)
                {
                    if (pt?.TCP_Client?.Socket == null)
                    {
                        return;
                    }

                    try
                    {
                        var receiveArgs = ProxyConfig.Proxy.RentClientArgs(pt);
                        if (!pt.TCP_Client.Socket.ReceiveAsync(receiveArgs))
                        {
                            ClientReceiveCompleted(pt.TCP_Client.Socket, receiveArgs);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(StartClientReceive), ex.Message);
                        pt?.Dispose();
                    }
                }

                public static void ClientReceiveCompleted(object sender, SocketAsyncEventArgs args)
                {
                    ProxyTCP pt = args.UserToken as ProxyTCP;

                    try
                    {
                        if (pt == null || pt._isDisposed || args.SocketError != SocketError.Success || args.BytesTransferred <= 0)
                        {
                            pt?.Dispose();
                            return;
                        }

                        // 检查 Buffer 和 Data 是否初始化
                        if (pt.TCP_Client.Buffer == null || pt.TCP_Client.Data == null)
                        {
                            Operate.DoLog(MethodBase.GetCurrentMethod().Name, "pt.TCP_Client.Buffer or Data is NULL");
                            pt.Dispose();
                            return;
                        }

                        // 数据处理
                        int bytesRead = Math.Min(args.BytesTransferred, pt.TCP_Client.Buffer.Length);
                        var proxyBufferSpan = pt.TCP_Client.Buffer.AsSpan(0, bytesRead);

                        // 合并数据
                        Span<byte> combinedData = new byte[pt.TCP_Client.Data.Length + bytesRead];
                        pt.TCP_Client.Data.AsSpan().CopyTo(combinedData);
                        proxyBufferSpan.CopyTo(combinedData.Slice(pt.TCP_Client.Data.Length));

                        if (ProxyConfig.Proxy.CheckDataIsMatchProxyStep(combinedData, pt.ProxyStep))
                        {
                            switch (pt.ProxyStep)
                            {
                                case ProxyConfig.Proxy.ProxyStep.Handshake:
                                    ProxyConfig.Proxy.Handshake(pt, combinedData);
                                    break;
                                case ProxyConfig.Proxy.ProxyStep.AuthUserName:
                                    ProxyConfig.Proxy.AuthUserName(pt, combinedData);
                                    break;
                                case ProxyConfig.Proxy.ProxyStep.Command:
                                    ProxyConfig.Proxy.Command(pt, combinedData);
                                    break;
                                case ProxyConfig.Proxy.ProxyStep.ForwardData:
                                    ProxyConfig.Proxy.ForwardData(pt, combinedData);
                                    break;
                            }

                            pt.TCP_Client.Data = Array.Empty<byte>();
                        }
                        else
                        {
                            pt.TCP_Client.Data = combinedData.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        pt?.Dispose();
                    }
                    finally
                    {
                        ProxyConfig.Proxy.ReturnClientArgs(args);

                        if (pt != null && !pt._isDisposed && pt.TCP_Client?.Socket != null)
                        {
                            StartClientReceive(pt);
                        }
                    }
                }

                #endregion

                #region//握手过程                

                private static void Handshake(ProxyTCP pe, ReadOnlySpan<byte> bData)
                {
                    try
                    {
                        pe.ProxyType = (ProxyConfig.Proxy.ProxyType)bData[0];

                        if (pe.ProxyType == ProxyConfig.Proxy.ProxyType.Socket5)
                        {
                            bool bSupportAuthType = false;

                            ProxyConfig.Proxy.AuthType atServer = new ProxyConfig.Proxy.AuthType();
                            if (ProxyConfig.Proxy.Enable_Auth)
                            {
                                atServer = ProxyConfig.Proxy.AuthType.UserName;
                            }
                            else
                            {
                                atServer = ProxyConfig.Proxy.AuthType.None;
                            }

                            int iMETHODS_COUNT = bData[1];
                            ReadOnlySpan<byte> bMETHODS = bData.Slice(2, iMETHODS_COUNT);
                            foreach (byte method in bMETHODS)
                            {
                                ProxyConfig.Proxy.AuthType atClient = (ProxyConfig.Proxy.AuthType)method;

                                if (atServer == atClient)
                                {
                                    bSupportAuthType = true;
                                    break;
                                }
                            }

                            if (bSupportAuthType)
                            {
                                Span<byte> bAuth = stackalloc byte[2];
                                bAuth[0] = (byte)ProxyConfig.Proxy.ProxyType.Socket5;
                                bAuth[1] = (byte)atServer;
                                ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, bAuth);

                                if (atServer == ProxyConfig.Proxy.AuthType.UserName)
                                {
                                    pe.ProxyStep = ProxyConfig.Proxy.ProxyStep.AuthUserName;

                                    if (bData.Length > iMETHODS_COUNT + 2)
                                    {
                                        ReadOnlySpan<byte> bAuthDate = bData.Slice(iMETHODS_COUNT + 2);

                                        bool bIsMatch = ProxyConfig.Proxy.CheckDataIsMatchProxyStep(bAuthDate, ProxyConfig.Proxy.ProxyStep.AuthUserName);
                                        if (bIsMatch)
                                        {
                                            ProxyConfig.Proxy.AuthUserName(pe, bAuthDate);
                                        }
                                    }
                                }
                                else
                                {
                                    pe.ProxyStep = ProxyConfig.Proxy.ProxyStep.Command;
                                }
                            }
                        }
                        else
                        {
                            string sLog = string.Format(AntdUI.Localization.Get("SOCKS.Unsupported", "不支持的 SOCKS 协议版本: {0}"), pe.ProxyType);
                            Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//验证账号密码

                private static void AuthUserName(ProxyTCP pe, ReadOnlySpan<byte> bData)
                {
                    try
                    {
                        byte VERSION = bData[0];

                        if (VERSION == 0x01)
                        {
                            int USERNAME_LENGTH = bData[1];
                            ReadOnlySpan<byte> USERNAME = bData.Slice(2, USERNAME_LENGTH);

                            int PASSWORD_LENGTH = bData[2 + USERNAME_LENGTH];
                            ReadOnlySpan<byte> PASSWORD = bData.Slice(3 + USERNAME_LENGTH, PASSWORD_LENGTH);

                            string sUserName = SystemConfig.BytesToString(PacketConfig.Packet.EncodingFormat.UTF8, USERNAME);
                            string sPassWord = SystemConfig.BytesToString(PacketConfig.Packet.EncodingFormat.UTF8, PASSWORD);
                            string ClientIP = pe.TCP_Client.EndPoint.Address.ToString();

                            Span<byte> bAuth = stackalloc byte[2];
                            bAuth[0] = 0x01;

                            // 第一步：先验证账号密码
                            bool bAuthOK = ProxyConfig.Account.CheckUserNameAndPassWord(sUserName, sPassWord, out Guid AccountID);

                            if (!bAuthOK)
                            {
                                // 账号密码验证失败直接返回
                                bAuth[1] = (byte)0x01;
                                ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, bAuth);
                                return;
                            }

                            // 第二步：验证通过后检查连接数限制
                            bool isOverLinks = ProxyConfig.Account.CheckLimitLinks(AccountID, ClientIP);
                            if (isOverLinks)
                            {
                                bAuth[1] = (byte)0x01;
                                ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, bAuth);
                                return;
                            }

                            // 第三步：检查设备数限制
                            bool isOverDevices = ProxyConfig.Account.CheckLimitDevices(AccountID, ClientIP);
                            if (isOverDevices)
                            {
                                bAuth[1] = (byte)0x01;
                                ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, bAuth);
                                return;
                            }

                            // 最终判断是否允许登录
                            bool isAllowed = bAuthOK && !isOverLinks && !isOverDevices;
                            bAuth[1] = isAllowed ? (byte)0x00 : (byte)0x01;

                            if (isAllowed)
                            {
                                ProxyConfig.Account.SetOnline_ByAccountID(AccountID, true);
                                ProxyConfig.Account.IPInfo_ToAccount(AccountID, ClientIP);
                                ProxyConfig.Account.AuthInfo_ToList(AccountID, ClientIP, true);

                                pe.AID = AccountID;
                                pe.ProxyStep = ProxyConfig.Proxy.ProxyStep.Command;
                            }

                            ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, bAuth);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//执行命令

                private static void Command(ProxyTCP pe, ReadOnlySpan<byte> bData)
                {
                    try
                    {
                        if (pe?.TCP_Client?.Socket == null)
                        {
                            return;
                        }

                        pe.ProxyType = (ProxyConfig.Proxy.ProxyType)bData[0];
                        pe.CommandType = (ProxyConfig.Proxy.CommandType)bData[1];
                        pe.AddressType = (ProxyConfig.Proxy.AddressType)bData[3];

                        if (pe.ProxyType == ProxyConfig.Proxy.ProxyType.Socket5)
                        {
                            try
                            {
                                ReadOnlySpan<byte> bADDRESS = bData.Slice(4, bData.Length - 4);
                                ReadOnlySpan<byte> bServerTCP_IP = ProxyConfig.Proxy.ProxyTCP_IP.GetAddressBytes();
                                ReadOnlySpan<byte> bServerTCP_Port = BitConverter.GetBytes(ProxyConfig.Proxy.ProxyPort);

                                IPEndPoint epServer = ProxyConfig.Proxy.GetIPEndPoint_ByAddressType(pe.AddressType, bADDRESS, out string AddressString);
                                if (epServer == null)
                                {
                                    ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, ProxyConfig.Proxy.GetProxyReturnData(ProxyConfig.Proxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                    return;
                                }

                                pe.TCP_Server.Socket = new Socket(epServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                                pe.TCP_Server.EndPoint = epServer;
                                ushort uPort = ((ushort)epServer.Port);

                                pe.DomainType = ProxyConfig.Proxy.GetDomainType_ByPort(uPort);
                                pe.TCP_Server.Address = ProxyConfig.Proxy.GetServerAddress(pe.DomainType, AddressString, uPort);
                                pe.TCP_Client.Address = ProxyConfig.Proxy.GetClientAddress(pe.TCP_Client.Socket, AddressString, uPort);

                                switch (pe.CommandType)
                                {
                                    case ProxyConfig.Proxy.CommandType.Connect:

                                        #region//代理 TCP

                                        switch (pe.DomainType)
                                        {
                                            case ProxyConfig.Proxy.DomainType.External:

                                                try
                                                {
                                                    IPEndPoint ExternalProxyEP = ProxyConfig.Proxy.GetIPEndPoint_ByAddressString(ProxyConfig.Proxy.ExternalProxy_IP, ProxyConfig.Proxy.ExternalProxy_Port);
                                                    if (ExternalProxyEP == null)
                                                    {
                                                        pe.TCP_Server.Close();
                                                        pe.TCP_Client.Close();
                                                        return;
                                                    }

                                                    var connectResult = pe.TCP_Server.Socket.BeginConnect(ExternalProxyEP, null, null);
                                                    if (!connectResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5)))
                                                    {
                                                        pe.TCP_Server.Close();
                                                        pe.TCP_Client.Close();
                                                        return;
                                                    }
                                                    pe.TCP_Server.Socket.EndConnect(connectResult);

                                                    byte[] handshakeRequest = null;
                                                    if (ProxyConfig.Proxy.Enable_ExternalProxy_Auth)
                                                    {
                                                        handshakeRequest = new byte[] { 0x05, 0x02, 0x00, 0x02 };
                                                    }
                                                    else
                                                    {
                                                        handshakeRequest = new byte[] { 0x05, 0x01, 0x00 };
                                                    }
                                                    pe.TCP_Server.Socket.Send(handshakeRequest);

                                                    byte[] handshakeResponse = new byte[2];
                                                    pe.TCP_Server.Socket.Receive(handshakeResponse);

                                                    if (handshakeResponse[0] != 0x05)
                                                    {
                                                        return;
                                                    }

                                                    switch (handshakeResponse[1])
                                                    {
                                                        case 0x00:
                                                            break;

                                                        case 0x02:

                                                            if (!ProxyConfig.Proxy.Enable_ExternalProxy_Auth)
                                                            {
                                                                return;
                                                            }

                                                            byte[] AuthRequest = ProxyConfig.Proxy.CreateSOCKS5AuthPacket(ProxyConfig.Proxy.ExternalProxy_UserName, ProxyConfig.Proxy.ExternalProxy_PassWord);
                                                            if (AuthRequest == null)
                                                            {
                                                                return;
                                                            }
                                                            pe.TCP_Server.Socket.Send(AuthRequest);

                                                            byte[] AuthResponse = new byte[2];
                                                            pe.TCP_Server.Socket.Receive(AuthResponse);

                                                            if (AuthResponse[1] != 0x00)
                                                            {
                                                                return;
                                                            }

                                                            break;

                                                        default:
                                                            return;
                                                    }

                                                    pe.TCP_Server.Socket.Send(bData.ToArray());

                                                    byte[] connectResponse = new byte[10];
                                                    pe.TCP_Server.Socket.Receive(connectResponse);

                                                    if (connectResponse[1] != 0x00)
                                                    {
                                                        ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, ProxyConfig.Proxy.GetProxyReturnData(ProxyConfig.Proxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                                        return;
                                                    }

                                                    ProxyConfig.Proxy.StartServerReceive(pe);
                                                    pe.ProxyStep = ProxyConfig.Proxy.ProxyStep.ForwardData;
                                                    ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, ProxyConfig.Proxy.GetProxyReturnData(ProxyConfig.Proxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));

                                                    ProxyConfig.Queue.ProxyTCP_ToQueue(pe);
                                                }
                                                catch (SocketException)
                                                {
                                                    pe.TCP_Server.Close();
                                                    pe.TCP_Client.Close();
                                                    ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, ProxyConfig.Proxy.GetProxyReturnData(ProxyConfig.Proxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                                }

                                                break;

                                            case ProxyConfig.Proxy.DomainType.Http:
                                            case ProxyConfig.Proxy.DomainType.Https:
                                            case ProxyConfig.Proxy.DomainType.Socket:

                                                try
                                                {
                                                    pe.TCP_Server.Socket.Connect(pe.TCP_Server.EndPoint);
                                                    ProxyConfig.Proxy.StartServerReceive(pe);
                                                    pe.ProxyStep = ProxyConfig.Proxy.ProxyStep.ForwardData;
                                                    ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, ProxyConfig.Proxy.GetProxyReturnData(ProxyConfig.Proxy.CommandResponse.Success, bServerTCP_IP, bServerTCP_Port));

                                                    ProxyConfig.Queue.ProxyTCP_ToQueue(pe);
                                                }
                                                catch (SocketException)
                                                {
                                                    ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, ProxyConfig.Proxy.GetProxyReturnData(ProxyConfig.Proxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                                }

                                                break;
                                        }

                                        #endregion

                                        break;

                                    case ProxyConfig.Proxy.CommandType.UDP:

                                        #region//UDP 中继                                    

                                        try
                                        {
                                            ProxyUDP pu = ProxyConfig.Proxy.CreateNewUDP();
                                            if (pu == null)
                                            {
                                                return;
                                            }                                            

                                            ReadOnlySpan<byte> bServerUDP_IP = ProxyConfig.Proxy.ProxyUDP_IP.GetAddressBytes();
                                            ReadOnlySpan<byte> bServerUDP_Port = BitConverter.GetBytes(((IPEndPoint)pu.ClientUDP.Client.LocalEndPoint).Port);

                                            ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, ProxyConfig.Proxy.GetProxyReturnData(ProxyConfig.Proxy.CommandResponse.Success, bServerUDP_IP, bServerUDP_Port));
                                            ProxyConfig.Proxy.StartUdpReceive(pu);
                                        }
                                        catch (SocketException)
                                        {
                                            ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, ProxyConfig.Proxy.GetProxyReturnData(ProxyConfig.Proxy.CommandResponse.Fault, bServerTCP_IP, bServerTCP_Port));
                                        }

                                        #endregion

                                        break;

                                    default:

                                        #region//不支持的命令

                                        ProxyConfig.Proxy.SendTCPData(pe.TCP_Client.Socket, ProxyConfig.Proxy.GetProxyReturnData(ProxyConfig.Proxy.CommandResponse.Unsupport, bServerTCP_IP, bServerTCP_Port));

                                        string sLog = string.Format(AntdUI.Localization.Get("Command.Unsupported", "{0} - 不支持的命令: {1}"), pe.TCP_Client.Socket.RemoteEndPoint, pe.CommandType);
                                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);

                                        #endregion

                                        break;
                                }
                            }
                            catch (SocketException ex)
                            {
                                Operate.DoLog(MethodBase.GetCurrentMethod().Name, pe.TCP_Server.Address + " - " + ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//处理 TCP 请求数据

                private static void ForwardData(ProxyTCP pt, Span<byte> bData)
                {
                    try
                    {
                        if (pt.CommandType == ProxyConfig.Proxy.CommandType.Connect)
                        {                            
                            bool requestHandled = false;

                            switch (pt.DomainType)
                            {
                                case ProxyConfig.Proxy.DomainType.Http:
                                    
                                    string request = Encoding.ASCII.GetString(bData.ToArray());

                                    if (request.StartsWith("GET") || request.StartsWith("POST") || request.StartsWith("HEAD") || request.StartsWith("PUT"))
                                    {
                                        var headers = ProxyConfig.Proxy.ParseHttpHeaders(request);
                                        if (headers.TryGetValue("Host", out string hostHeader))
                                        {
                                            string requestPath = request.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                                            string cleanPath = requestPath.Split('?')[0];

                                            #region//本地代理映射

                                            if (ProxyConfig.Mapping.Enable_MapLocal)
                                            {
                                                var localRule = ProxyConfig.Mapping.GetMapLocal(
                                                    ProxyConfig.Proxy.MapProtocol.Http,
                                                    hostHeader.Split(':')[0],
                                                    pt.TCP_Server.EndPoint.Port,
                                                    cleanPath);

                                                if (localRule != null)
                                                {
                                                    if (File.Exists(localRule.LocalPath))
                                                    {
                                                        byte[] fileBytes = File.ReadAllBytes(localRule.LocalPath);
                                                        string contentType = ProxyConfig.Proxy.GetContentType(Path.GetExtension(localRule.LocalPath));

                                                        string response =
                                                            $"HTTP/1.1 200 OK\r\n" +
                                                            $"Content-Type: {contentType}\r\n" +
                                                            $"Content-Length: {fileBytes.Length}\r\n" +
                                                            "Connection: close\r\n\r\n";

                                                        byte[] headerBytes = Encoding.UTF8.GetBytes(response);
                                                        ProxyConfig.Proxy.SendTCPData(pt.TCP_Client.Socket, headerBytes);
                                                        ProxyConfig.Proxy.SendTCPData(pt.TCP_Client.Socket, fileBytes);
                                                        requestHandled = true;
                                                    }
                                                    else
                                                    {
                                                        ProxyConfig.Proxy.Send404Response(pt.TCP_Client.Socket);
                                                        requestHandled = true;
                                                    }
                                                }
                                            }

                                            #endregion

                                            #region//远程代理映射

                                            if (!requestHandled && ProxyConfig.Mapping.Enable_MapRemote)
                                            {
                                                var remoteRule = ProxyConfig.Mapping.GetMapRemote(
                                                    ProxyConfig.Proxy.MapProtocol.Http,
                                                    hostHeader.Split(':')[0],
                                                    pt.TCP_Server.EndPoint.Port,
                                                    cleanPath);

                                                if (remoteRule != null)
                                                {
                                                    string RemoteURL = remoteRule.ProtocolTypeTo.ToString() + "://" + remoteRule.HostTo + ":" + remoteRule.PortTo + remoteRule.PathTo;
                                                    byte[] remoteResponse = ProxyConfig.Mapping.GetRemoteMappedData(RemoteURL, request, headers);
                                                    if (remoteResponse != null)
                                                    {
                                                        ProxyConfig.Proxy.SendTCPData(pt.TCP_Client.Socket, remoteResponse);
                                                        requestHandled = true;
                                                    }
                                                }
                                            }

                                            #endregion
                                        }
                                    }
                                    
                                    break;

                                case ProxyConfig.Proxy.DomainType.Https:
                                case ProxyConfig.Proxy.DomainType.Socket:
                                case ProxyConfig.Proxy.DomainType.External:

                                    requestHandled = false;

                                    break;
                            }

                            if (!requestHandled)
                            {
                                if (ProxyConfig.Proxy.HookTCP_Req)
                                {
                                    ProxyConfig.Proxy.DoFilter_TCP(pt, bData, PacketConfig.Packet.PacketType.TCP_Req);
                                }
                                else
                                {
                                    ProxyConfig.Proxy.SendTCPData(pt.TCP_Server.Socket, bData);
                                }                                    
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        pt.Dispose();
                    }
                }

                #endregion                

                #region//处理 TCP 响应数据

                private static void StartServerReceive(ProxyTCP pt)
                {
                    if (pt?.TCP_Server?.Socket == null)
                    {
                        return;
                    }

                    try
                    {
                        var receiveArgs = ProxyConfig.Proxy.RentServerArgs(pt);

                        if (!pt.TCP_Server.Socket.ReceiveAsync(receiveArgs))
                        {
                            ServerReceiveCompleted(pt.TCP_Server.Socket, receiveArgs);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        pt.Dispose();
                    }                    
                }

                public static void ServerReceiveCompleted(object sender, SocketAsyncEventArgs args)
                {
                    ProxyTCP pt = args.UserToken as ProxyTCP;

                    try
                    {
                        if (pt == null || pt._isDisposed || args.SocketError != SocketError.Success || args.BytesTransferred <= 0)
                        {
                            pt?.Dispose();
                            return;
                        }

                        // 检查 Buffer 是否初始化
                        if (pt.TCP_Server.Buffer == null)
                        {
                            Operate.DoLog(MethodBase.GetCurrentMethod().Name, "pt.TCP_Server.Buffer is NULL");
                            pt?.Dispose();
                            return;
                        }

                        int bytesRead = Math.Min(args.BytesTransferred, pt.TCP_Server.Buffer.Length);
                        var dataSpan = pt.TCP_Server.Buffer.AsSpan(0, bytesRead);

                        if (pt.CommandType == ProxyConfig.Proxy.CommandType.Connect)
                        {
                            if (ProxyConfig.Proxy.HookTCP_Resp)
                            {
                                ProxyConfig.Proxy.DoFilter_TCP(pt, dataSpan, PacketConfig.Packet.PacketType.TCP_Resp);
                            }
                            else
                            {
                                ProxyConfig.Proxy.SendTCPData(pt.TCP_Client.Socket, dataSpan);
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        pt.Dispose();
                    }
                    finally
                    {
                        ProxyConfig.Proxy.ReturnServerArgs(args);

                        if (pt != null && !pt._isDisposed && pt.TCP_Server?.Socket != null)
                        {
                            StartServerReceive(pt);
                        }
                    }
                }

                #endregion

                #region//处理 UDP 中继数据

                public static void StartUdpReceive(ProxyUDP pu)
                {
                    try
                    {
                        if (pu.ClientUDP != null)
                        {
                            pu.ClientUDP.BeginReceive(new AsyncCallback(UdpReceiveCallback), pu);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                private static void UdpReceiveCallback(IAsyncResult ar)
                {
                    if (ar == null || !(ar.AsyncState is ProxyUDP pu))
                    {
                        return;
                    }

                    if (pu.ClientUDP == null)
                    {
                        return;
                    }

                    try
                    {
                        IPEndPoint epRemote = new IPEndPoint(IPAddress.Any, 0);

                        byte[] bReceivedData = ProxyConfig.Proxy.ReceiveUDPData(pu.ClientUDP, ar, ref epRemote);
                        if (bReceivedData.Length == 0 || epRemote.Address.Equals(IPAddress.Any) || epRemote.Port == 0)
                        {
                            return;
                        }

                        Span<byte> bData = bReceivedData.AsSpan();
                        if (bData[0] == 0 && bData[1] == 0 && bData[2] == 0)
                        {
                            #region//处理 UDP 请求数据

                            ProxyConfig.Proxy.AddressType addressType = (ProxyConfig.Proxy.AddressType)bData[3];

                            if (addressType == ProxyConfig.Proxy.AddressType.IPv4 ||
                                addressType == ProxyConfig.Proxy.AddressType.IPv6 ||
                                addressType == ProxyConfig.Proxy.AddressType.Domain)
                            {
                                pu.ClientEndPoint = epRemote;

                                ReadOnlySpan<byte> bADDRESS = bData.Slice(4, bData.Length - 4);
                                IPEndPoint targetEndPoint = ProxyConfig.Proxy.GetIPEndPoint_ByAddressType(addressType, bADDRESS, out string AddressString);
                                if (targetEndPoint != null)
                                {
                                    Span<byte> bRequestData = ProxyConfig.Proxy.GetUDPData_ByAddressType(addressType, bData);
                                    if (!bRequestData.IsEmpty)
                                    {
                                        ProxyConfig.Proxy.UDP_Req_CNT++;
                                        Interlocked.Add(ref ProxyConfig.Proxy.Total_Request, bRequestData.Length);
                                        Interlocked.Add(ref Operate.ProxyConfig.Proxy.ProxySpeed_Uplink, bRequestData.Length);

                                        if (ProxyConfig.Proxy.HookUDP_Req)
                                        {
                                            ProxyConfig.Proxy.DoFilter_UDP(pu, targetEndPoint, bRequestData, PacketConfig.Packet.PacketType.UDP_Req);
                                        }
                                        else
                                        {
                                            ProxyConfig.Proxy.SendUDPData(pu.ClientUDP, bRequestData, targetEndPoint);
                                        }
                                        
                                        pu.UpdateActivity();
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region//处理 UDP 响应数据

                            if (pu.ClientEndPoint == null)
                            {
                                return;
                            }

                            ReadOnlySpan<byte> bIP = pu.ClientEndPoint.Address.GetAddressBytes();
                            ushort port = ((ushort)pu.ClientEndPoint.Port);
                            ReadOnlySpan<byte> bPort = new byte[2] { (byte)(port >> 8), (byte)port };

                            Span<byte> bResponseData = stackalloc byte[4 + bIP.Length + bPort.Length + bData.Length];
                            bResponseData[0] = 0x00;
                            bResponseData[1] = 0x00;
                            bResponseData[2] = 0x00;
                            bResponseData[3] = (byte)ProxyConfig.Proxy.AddressType.IPv4;
                            bIP.CopyTo(bResponseData.Slice(4, bIP.Length));
                            bPort.CopyTo(bResponseData.Slice(8, bPort.Length));
                            bData.CopyTo(bResponseData.Slice(10, bData.Length));

                            if (!bResponseData.IsEmpty)
                            {
                                ProxyConfig.Proxy.UDP_Resp_CNT++;
                                Interlocked.Add(ref ProxyConfig.Proxy.Total_Response, bResponseData.Length);
                                Interlocked.Add(ref Operate.ProxyConfig.Proxy.ProxySpeed_Downlink, bResponseData.Length);

                                if (ProxyConfig.Proxy.HookUDP_Resp)
                                {
                                    ProxyConfig.Proxy.DoFilter_UDP(pu, epRemote, bResponseData, PacketConfig.Packet.PacketType.UDP_Resp);
                                }
                                else
                                {
                                    ProxyConfig.Proxy.SendUDPData(pu.ClientUDP, bResponseData, pu.ClientEndPoint);
                                }
                                
                                pu.UpdateActivity();
                            }

                            #endregion
                        }
                        
                        ProxyConfig.Proxy.StartUdpReceive(pu);                        
                    }
                    catch (SocketException ex) when (Operate.PacketConfig.Packet.IsExpectedSocketError(ex.ErrorCode))
                    {
                        //
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        ProxyConfig.Proxy.StartUdpReceive(pu);
                    }
                }

                #endregion

                #region//创建新UDP端口

                public static ProxyUDP CreateNewUDP()
                {
                    try
                    {
                        var pu = new ProxyUDP(new IPEndPoint(ProxyConfig.Proxy.ProxyUDP_IP, 0));
                        ProxyConfig.List.cdProxyUDP.TryAdd(Guid.NewGuid(), pu);
                        pu.UpdateActivity();
                        return pu;
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return null;
                }

                public static void CheckUDPTimeOut()
                {
                    try
                    {
                        var now = DateTime.Now;
                        foreach (var pair in ProxyConfig.List.cdProxyUDP)
                        {
                            if (now - pair.Value.LastActivityTime > ProxyConfig.List.UDPTimeout)
                            {
                                ProxyConfig.List.cdProxyUDP.TryRemove(pair.Key, out _);
                                pair.Value.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }                    
                }

                #endregion

                #region//发送和接收代理数据

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

                #region//执行滤镜 - 代理模式

                public static void DoFilter_TCP(ProxyTCP pt, Span<byte> bData, PacketConfig.Packet.PacketType ptType)
                {
                    try
                    {                        
                        Socket SendSocket = null;
                        switch (ptType)
                        {
                            case PacketConfig.Packet.PacketType.TCP_Req:
                                SendSocket = pt.TCP_Server.Socket;
                                break;

                            case PacketConfig.Packet.PacketType.TCP_Resp:
                                SendSocket = pt.TCP_Client.Socket;
                                break;
                        }

                        if (SendSocket == null)
                        {
                            return;
                        }

                        int iSocket = SendSocket.Handle.ToInt32();

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
                            res = ProxyConfig.Proxy.SendTCPData(SendSocket, bNewBuffer);
                        }

                        string ClientAddr = $"{pt.TCP_Client.EndPoint.Address.ToString()}:{pt.TCP_Client.EndPoint.Port.ToString()}";
                        string ServerAddr = $"{pt.TCP_Server.EndPoint.Address.ToString()}:{pt.TCP_Server.EndPoint.Port.ToString()}";
                        string ServerDomain = pt.TCP_Server.Address.Trim();

                        _ = ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            FilterAction,
                            res,
                            iSocket,
                            ptType,
                            ClientAddr,
                            ServerAddr,
                            ServerDomain,
                            pt.DomainType,
                            bRawBuffer,
                            bNewBuffer);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                public static void DoFilter_UDP(ProxyUDP pu, IPEndPoint epRemote, Span<byte> bData, PacketConfig.Packet.PacketType ptType)
                {
                    try
                    {
                        IPEndPoint epSend = null;
                        switch (ptType)
                        {
                            case PacketConfig.Packet.PacketType.UDP_Req:
                                epSend = epRemote;
                                break;

                            case PacketConfig.Packet.PacketType.UDP_Resp:
                                epSend = pu.ClientEndPoint;
                                break;
                        }

                        if (epSend == null || pu?.ClientUDP?.Client == null)
                        {
                            return;
                        }

                        int iSocket = pu.ClientUDP.Client.Handle.ToInt32();

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
                            res = ProxyConfig.Proxy.SendUDPData(pu.ClientUDP, bNewBuffer, epSend);
                        }

                        string ClientAddr = $"{pu.ClientEndPoint.Address.ToString()}:{pu.ClientEndPoint.Port.ToString()}";
                        string ServerAddr = $"{epRemote.Address.ToString()}:{epRemote.Port.ToString()}";

                        _ = ProxyConfig.Queue.ProxyInfo_ToQueue(
                            DateTime.Now,
                            FilterAction,
                            res,
                            iSocket,
                            ptType,
                            ClientAddr,
                            ServerAddr,
                            string.Empty,
                            DomainType.External,
                            bRawBuffer,
                            bNewBuffer);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//获取客户端的IP地址

                public static string GetClientIPAddress(ProxyTCP pe)
                {
                    try
                    {
                        if (pe != null && pe.TCP_Client.EndPoint != null)
                        {
                            return pe.TCP_Client.EndPoint.Address.ToString();
                        }
                        else
                        {
                            return string.Empty;
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
                    try
                    {
                        return string.Format("{0} [{1}]", ClientIP, ClientUserName);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return string.Empty;
                }

                #endregion                

                #region//设置系统代理

                public static bool StartSystemProxy(Form form)
                {
                    bool bReturn = false;

                    try
                    {
                        RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

                        if (key != null)
                        {
                            string sProxyServer = string.Format("socks5://127.0.0.1:{0}", Operate.ProxyConfig.Proxy.ProxyPort);

                            key.SetValue("ProxyEnable", 1, RegistryValueKind.DWord);
                            key.SetValue("ProxyServer", sProxyServer, RegistryValueKind.String);
                            key.SetValue("ProxyOverride", string.Empty, RegistryValueKind.String);
                            key.Close();

                            bReturn = true;

                            AntdUI.Message.open(new AntdUI.Message.Config(form, "已启用系统代理", TType.Success)
                            {
                                LocalizationText = "SystemProxy.Start"
                            });                            
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return bReturn;
                }

                public static bool StopSystemProxy(Form form)
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

                            AntdUI.Message.open(new AntdUI.Message.Config(form, "已关闭系统代理", TType.Success)
                            {
                                LocalizationText = "SystemProxy.Stop"
                            });                            
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return bReturn;
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
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return bReturn;
                }

                #endregion

                #region//检测外部代理服务器

                public static async Task<bool> DetectionExternalProxy(Form form)
                {
                    try
                    {
                        IPEndPoint ExternalProxyEP = ProxyConfig.Proxy.GetIPEndPoint_ByAddressString(Operate.ProxyConfig.Proxy.ExternalProxy_IP, Operate.ProxyConfig.Proxy.ExternalProxy_Port);
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
                            if (Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth)
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
                                    if (!Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth)
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(form, "外部代理要求认证", TType.Error)
                                        {
                                            LocalizationText = "SystemSettingsForm.Success"
                                        });

                                        return false;
                                    }

                                    byte[] AuthRequest = ProxyConfig.Proxy.CreateSOCKS5AuthPacket(Operate.ProxyConfig.Proxy.ExternalProxy_UserName, Operate.ProxyConfig.Proxy.ExternalProxy_PassWord);
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
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, "不支持的认证方式", TType.Error)
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

                #region//初始化CCProxy模板

                public static void InitCCProxy_HTML()
                {
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web", "CCProxy", "cn_acclistadmin.htm");

                    if (File.Exists(filePath))
                    {
                        Operate.ProxyConfig.Account.CCProxy_HTML = File.ReadAllText(filePath, Encoding.UTF8);
                    }
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
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        ProxyConfig.Proxy.SendTCPData(clientSocket, responseBytes);
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//是否显示代理数据（过滤条件）

                public static bool IsShowProxy_ByFilter(ProxyInfo pi)
                {
                    try
                    {
                        //套接字
                        if (SystemConfig.CheckSocket)
                        {
                            bool bIsFilter = PacketConfig.Packet.IsFilter_BySocket(pi.PacketSocket);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //IP地址
                        if (SystemConfig.CheckIP)
                        {
                            bool bIsFilter_From = PacketConfig.Packet.IsFilter_ByIP(pi.ClientAddr);
                            bool bIsFilter_To = PacketConfig.Packet.IsFilter_ByIP(pi.ServerAddr);
                            if (SystemConfig.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                            {
                                return false;
                            }
                        }

                        //端口号
                        if (SystemConfig.CheckPort)
                        {
                            bool bIsFilter_From = PacketConfig.Packet.IsFilter_ByPort(pi.ClientAddr);
                            bool bIsFilter_To = PacketConfig.Packet.IsFilter_ByPort(pi.ServerAddr);
                            if (SystemConfig.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                            {
                                return false;
                            }
                        }

                        //指定包头
                        if (SystemConfig.CheckHead)
                        {
                            bool bIsFilter = PacketConfig.Packet.IsFilter_ByHead(pi.PacketBuffer);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //封包内容
                        if (SystemConfig.CheckData)
                        {
                            bool bIsFilter = PacketConfig.Packet.IsFilter_ByPacket(pi.PacketBuffer);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //封包大小
                        if (SystemConfig.CheckLen)
                        {
                            bool bIsFilter = PacketConfig.Packet.IsFilter_BySize(pi.PacketLen);
                            if (SystemConfig.CheckNotShow == bIsFilter)
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
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return "application/octet-stream";
                }

                #endregion                

                #region//获取IP地址信息                

                public static IPEndPoint GetIPEndPoint_ByAddressString(string AddressString, ushort Port)
                {
                    try
                    {
                        IPAddress ipAddress = ProxyConfig.Proxy.ResolveAddress(AddressString);
                        return new IPEndPoint(ipAddress, Port);
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return null;
                }

                public static IPEndPoint GetIPEndPoint_ByAddressType(Operate.ProxyConfig.Proxy.AddressType addressType, ReadOnlySpan<byte> bData, out string AddressString)
                {
                    AddressString = string.Empty;

                    try
                    {
                        IPAddress ip = IPAddress.Any;
                        ushort port = 0;
                        int portPosition = 0;

                        switch (addressType)
                        {
                            case Operate.ProxyConfig.Proxy.AddressType.IPv4:
                                ip = new IPAddress(bData.Slice(0, 4).ToArray());
                                portPosition = 4;
                                AddressString = ip.ToString();
                                break;

                            case Operate.ProxyConfig.Proxy.AddressType.IPv6:
                                ip = new IPAddress(bData.Slice(0, 16).ToArray());
                                portPosition = 16;
                                AddressString = ip.ToString();
                                break;

                            case Operate.ProxyConfig.Proxy.AddressType.Domain:
                                byte length = bData[0];
                                var domainBytes = bData.Slice(1, length);
                                AddressString = Operate.SystemConfig.BytesToString(
                                    Operate.PacketConfig.Packet.EncodingFormat.UTF8,
                                    domainBytes.ToArray());
                                ip = ProxyConfig.Proxy.ResolveAddress(AddressString);
                                portPosition = 1 + length;
                                break;
                        }

                        if (ip != null)
                        {
                            port = Operate.SystemConfig.ByteArrayToInt16BigEndian(bData.Slice(portPosition, 2).ToArray());
                            return new IPEndPoint(ip, port);
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        DoLog(nameof(ResolveAddressAsync), ex.Message);
                    }

                    return null;
                }

                #endregion

                #region//获取UDP数据包

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
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        return Span<byte>.Empty;
                    }
                }

                #endregion

                #region//获取返回给客户端的数据（SOCKS5，IPV4）

                public static byte[] GetProxyReturnData(Operate.ProxyConfig.Proxy.CommandResponse CommandResponse, ReadOnlySpan<byte> bServerIP, ReadOnlySpan<byte> bServerPort)
                {
                    try
                    {
                        Span<byte> response = stackalloc byte[10];
                        response[0] = (byte)Operate.ProxyConfig.Proxy.ProxyType.Socket5;
                        response[1] = (byte)CommandResponse;
                        response[2] = 0x00;
                        response[3] = (byte)Operate.ProxyConfig.Proxy.AddressType.IPv4;
                        bServerIP.CopyTo(response.Slice(4, 4));
                        response[8] = bServerPort[1];
                        response[9] = bServerPort[0];

                        return response.ToArray();
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        return Array.Empty<byte>();
                    }
                }

                #endregion

                #region//获取端口对应的域名类型

                public static Operate.ProxyConfig.Proxy.DomainType GetDomainType_ByPort(ushort Port)
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
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return Operate.ProxyConfig.Proxy.DomainType.Socket;
                }

                #endregion

                #region//获取服务端地址

                public static string GetServerAddress(Operate.ProxyConfig.Proxy.DomainType dtType, string AddressString, ushort port)
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
                            case Operate.ProxyConfig.Proxy.DomainType.Socket:
                                protocol = "socket://";
                                break;
                            case Operate.ProxyConfig.Proxy.DomainType.Http:
                                protocol = "http://";
                                break;
                            case Operate.ProxyConfig.Proxy.DomainType.Https:
                                protocol = "https://";
                                break;
                            case Operate.ProxyConfig.Proxy.DomainType.External:
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
            }

            #endregion

            #region//代理队列

            public static class Queue
            {                
                public static ConcurrentQueue<ProxyTCP> qProxyTCP = new ConcurrentQueue<ProxyTCP>();
                public static ConcurrentQueue<ProxyInfo> qProxyInfo = new ConcurrentQueue<ProxyInfo>();

                #region//TCP代理入队列

                public static void ProxyTCP_ToQueue(ProxyTCP pt)
                {
                    qProxyTCP.Enqueue(pt);
                }

                #endregion                

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

                            if (!ProxyConfig.Proxy.SpeedMode)
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
                            Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }
                    });                    
                }

                #endregion

                #region//清除队列数据

                public static void ResetProxyTCPQueue()
                {
                    try
                    {
                        while (!qProxyTCP.IsEmpty)
                        {
                            qProxyTCP.TryDequeue(out ProxyTCP pt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }                

                public static void ResetProxyInfoQueue()
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion
            }

            #endregion

            #region//代理列表

            public static class List
            {                
                public static int Search_Index = -1;
                public static bool AutoRoll = false;
                public static bool AutoClear = true;
                public static decimal AutoClear_Value = 5000;
                public static ProxyInfo piSelect = null;                

                public static BindingList<ProxyTCP> lstProxyTCP = new BindingList<ProxyTCP>();

                public static readonly ConcurrentDictionary<Guid, ProxyUDP> cdProxyUDP = new ConcurrentDictionary<Guid, ProxyUDP>();
                public static readonly TimeSpan UDPTimeout = TimeSpan.FromMinutes(5);

                public static BindingList<ProxyInfo> lstProxyInfo = new BindingList<ProxyInfo>();                

                #region//TCP代理入列表

                public static void ProxyTCP_ToList()
                {
                    try
                    {
                        if (ProxyConfig.Queue.qProxyTCP.TryDequeue(out ProxyTCP pt))
                        {
                            ProxyConfig.List.lstProxyTCP.Add(pt);
                        }
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }                    
                }

                #endregion                                

                #region//代理数据入列表

                public static void ProxyInfo_ToList()
                {
                    try
                    {
                        if (ProxyConfig.Queue.qProxyInfo.TryDequeue(out ProxyInfo pi))
                        {
                            bool bIsShow = ProxyConfig.Proxy.IsShowProxy_ByFilter(pi);
                            if (bIsShow)
                            {
                                Span<byte> bufferSpan = pi.PacketBuffer.AsSpan();
                                pi.PacketData = PacketConfig.Packet.GetPacketData_Hex(pi.PacketBuffer.AsSpan(), PacketConfig.Packet.PacketData_MaxLen);
                                ProxyConfig.List.lstProxyInfo.Add(pi);
                            }
                            else
                            {
                                ProxyConfig.Proxy.FilterProxy_CNT++;
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//查找代理列表

                public static List<ProxyTCP> GetProxyExecute_ByAccountID(Guid AID)
                {
                    try
                    {
                        if (AID != null)
                        {
                            return new List<ProxyTCP>(ProxyConfig.List.lstProxyTCP.Where(x => x.AID == AID));
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return null;
                }

                public static List<ProxyTCP> GetProxyTCP_ByAIDandIP(Guid AID, string ClientIP)
                {
                    try
                    {
                        if (AID == Guid.Empty || string.IsNullOrWhiteSpace(ClientIP))
                        {
                            return new List<ProxyTCP>();
                        }

                        var proxyList = ProxyConfig.List.lstProxyTCP;

                        return proxyList
                            .Where(x => x != null &&
                                       x.AID == AID &&
                                       x.TCP_Client?.EndPoint?.Address != null &&
                                       x.TCP_Client.EndPoint.Address.ToString().Equals(ClientIP.Trim(), StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        return new List<ProxyTCP>();
                    }
                }

                #endregion

                #region//搜索代理列表

                public static int SearchForProxyList(int fromIndex, ReadOnlySpan<byte> searchData)
                {
                    int iResult = -1;

                    try
                    {
                        if (searchData.Length == 0 || fromIndex < 0)
                        {
                            return -1;
                        }

                        int listCount = ProxyConfig.List.lstProxyInfo.Count;
                        if (listCount == 0 || fromIndex >= listCount)
                        {
                            return -1;
                        }

                        if (fromIndex == -1)
                        {
                            fromIndex = 0;
                        }

                        for (int i = fromIndex; i < listCount; i++)
                        {
                            byte[] packetBuffer = ProxyConfig.List.lstProxyInfo[i].PacketBuffer;
                            if (packetBuffer != null && packetBuffer.Length >= searchData.Length)
                            {
                                ReadOnlySpan<byte> packetSpan = packetBuffer.AsSpan();
                                if (packetSpan.IndexOf(searchData) != -1)
                                {
                                    return i;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return iResult;
                }

                #endregion

                #region//关闭代理列表中的指定账号的链接

                public static void CloseProxyTCP_ByAID(Guid AID)
                {
                    try
                    {
                        List<ProxyTCP> peList = GetProxyExecute_ByAccountID(AID);

                        foreach (ProxyTCP pe in peList)
                        {
                            pe.TCP_Client.Close();
                            pe.TCP_Server.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                public static void CloseProxyTCP_ByAIDAndIP(Guid AID, string ClientIP)
                {
                    try
                    {
                        List<ProxyTCP> peList = GetProxyTCP_ByAIDandIP(AID, ClientIP);

                        foreach (ProxyTCP pe in peList)
                        {
                            pe.TCP_Client.Close();
                            pe.TCP_Server.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//清除代理列表中的指定数据

                public static void ClearProxyTCP(ProxyTCP pt)
                {
                    try
                    {
                        var list = ProxyConfig.List.lstProxyTCP;
                        if (list.Contains(pt))
                        {
                            list.Remove(pt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion                

                #region//清空整个列表

                public static void ResetProxyTCPList()
                {
                    ProxyConfig.List.lstProxyTCP.Clear();
                }

                public static void ResetProxyInfoList()
                {
                    ProxyConfig.List.lstProxyInfo.Clear();
                }

                #endregion                

                #region//保存代理列表为Excel（对话框）

                public static void SaveProxyList_Dialog(Form form, AntdUI.Table tTable, string FileName, List<ProxyInfo> piList)
                {
                    try
                    {
                        if (ProxyConfig.List.lstProxyInfo.Count > 0)
                        {
                            int SaveCount = ProxyConfig.List.lstProxyInfo.Count;

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
                                        bOK = ProxyConfig.List.SaveProxyListToExcel(FilePath, piList);
                                    }, () =>
                                    {
                                        if (bOK)
                                        {
                                            string Title = AntdUI.Localization.Get("InjectModeForm.ExportToExcel.Success", "导出到Excel成功");
                                            AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                            Operate.DoLog(nameof(SaveProxyList_Dialog), Title + ": " + FilePath);
                                        }
                                        else
                                        {
                                            string Title = AntdUI.Localization.Get("InjectModeForm.ExportToExcel.Error", "导出到Excel失败");
                                            string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                            AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                        }
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                private static bool SaveProxyListToExcel(string filePath, List<ProxyInfo> piList)
                {
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        using (var writer = new StreamWriter(stream, Encoding.Default))
                        {
                            writer.WriteLine(AntdUI.Localization.Get("ToExcelTitle", "时间戳\t类别\t套接字\t客户端地址\t服务端地址\t长度\t数据\t"));

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
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        return false;
                    }
                }

                #endregion
            }

            #endregion

            #region//代理账号

            public static class Account
            {
                public static bool NeedSave = false;
                public static bool IsShow_ProxyAccount = false, IsShow_ProxyAuth = false;                
                public static string CCProxy_HTML = string.Empty;

                public static BindingList<AccountInfo> lstAccountInfo = new BindingList<AccountInfo>();
                public static ConcurrentDictionary<(Guid AID, string AuthIP), AuthInfo> cdAuthInfo = new ConcurrentDictionary<(Guid, string), AuthInfo>();

                #region//代理认证入列表            

                public static async void AuthInfo_ToList(Guid AID, string AuthIP, bool AuthResult)
                {
                    try
                    {
                        if (AID == null || AID == Guid.Empty) return;

                        var key = (AID, AuthIP);
                        string IPLocation = await SystemConfig.GetIPLocation(AuthIP);

                        cdAuthInfo.AddOrUpdate(
                            key,
                            _ => new AuthInfo(AID, AuthIP, IPLocation, AuthResult, DateTime.Now),
                            (_, existingItem) =>
                            {
                                existingItem.AuthResult = AuthResult;
                                existingItem.AuthTime = DateTime.Now;
                                return existingItem;
                            });
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                        
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
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
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return bReturn;
                }

                #endregion

                #region//检测代理账号是否已存在

                public static bool CheckProxyAccount_Exist(string UserName)
                {
                    try
                    {
                        foreach (AccountInfo pai in ProxyConfig.Account.lstAccountInfo)
                        {
                            if (pai.UserName.Equals(UserName))
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

                #region//检测用户名和密码是否正确（区分大小写）

                public static bool CheckUserNameAndPassWord(string UserName, string PassWord, out Guid AccountID)
                {
                    AccountID = Guid.Empty;

                    try
                    {
                        string pwEncrypt = SystemConfig.PassWord_Encrypt(PassWord);

                        foreach (AccountInfo pai in ProxyConfig.Account.lstAccountInfo)
                        {
                            if (pai.IsEnable && pai.UserName.Equals(UserName) && pai.Password.Equals(pwEncrypt))
                            {
                                if (pai.IsExpiry)
                                {
                                    if (pai.ExpiryTime > DateTime.Now)
                                    {
                                        AccountID = pai.AID;
                                        return true;
                                    }
                                }
                                else
                                {
                                    AccountID = pai.AID;
                                    return true;
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return iReturn;
                }

                #endregion

                #region//获取代理账号的链接数

                public static int GetLinksNumber_ByAccountID(Guid AID, string ClientIP, AntdUI.Tree tree)
                {
                    try
                    {
                        string ClientUserName = ProxyConfig.Account.GetUserName_ByAccountID(AID);
                        string RootName = ProxyConfig.Proxy.GetClientListName(ClientIP, ClientUserName);

                        TreeItem tiRoot = SystemConfig.FindNodeByName(tree, RootName);
                        if (tiRoot != null)
                        {
                            return tiRoot.Sub.Count;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                        return ProxyConfig.Account.cdAuthInfo.Count(kvp => kvp.Key.AID == AID);
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        return 0;
                    }
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
                        Sub = new AntdUI.IContextMenuStripItem[]
                        {
                            new AntdUI.ContextMenuStripItem("过期时间")
                            {
                                ID = "ExpiryTime",
                                IconSvg = "FieldTimeOutlined",
                            },
                            new AntdUI.ContextMenuStripItem("链接数")
                            {
                                ID = "LimitLinks",
                                IconSvg = "ForkOutlined",
                            },
                            new AntdUI.ContextMenuStripItem("设备数")
                            {
                                ID = "LimitDevices",
                                IconSvg = "TabletOutlined",
                            },
                        },
                    });                    
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());                    
                    menuItems.Add(new AntdUI.ContextMenuStripItem("批量导出")
                    {
                        ID = "Export",
                        IconSvg = "DeliveredProcedureOutlined",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("批量删除")
                    {
                        ID = "Delete",
                        IconSvg = "DeleteOutlined",
                    });

                    return menuItems.ToArray();
                }

                #endregion                

                #region//记录代理账号的IP地址（异步）

                public static async void IPInfo_ToAccount(Guid AccountID, string IPAddress)
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
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//新增代理账号

                public static bool AddProxyAccount(
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            var pai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.AID == AID);

                            if (pai != null)
                            {
                                pai.IsEnable = IsEnable;

                                if (!string.IsNullOrEmpty(PassWord))
                                {
                                    pai.Password = PassWord;
                                }

                                pai.IsLimitLinks = IsLimitLinks;
                                pai.LimitLinks = LimitLinks;
                                pai.IsExpiry = IsExpiry;
                                pai.IsLimitDevices = IsLimitDevices;
                                pai.LimitDevices = LimitDevices;
                                pai.ExpiryTime = ExpiryTime;

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
                            var pai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.UserName == UserName);

                            if (pai != null)
                            {
                                pai.IsEnable = IsEnable;

                                if (!string.IsNullOrEmpty(PassWord))
                                {
                                    pai.Password = PassWord;
                                }

                                pai.IsLimitLinks = IsLimitLinks;
                                pai.LimitLinks = LimitLinks;
                                pai.IsExpiry = IsExpiry;
                                pai.ExpiryTime = ExpiryTime;

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

                #region//删除代理账号（对话框）                

                public static void DeleteAccount_Dialog(Form form, List<AccountInfo> aiList)
                {
                    try
                    {
                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miAccountList", "账号列表"), "\r\n确定删除数据吗\r\n\r\n")
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
                                    }
                                }

                                switch (Operate.SystemConfig.StartMode)
                                {
                                    case Operate.SystemConfig.SystemMode.Process:

                                        //

                                        break;

                                    case Operate.SystemConfig.SystemMode.Proxy:

                                        ((InterfaceInfo.IProxyMode)form).RefreshAccountList();

                                        break;
                                }

                                return true;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                public static bool DeleteProxyAccount_ByAccountID(Guid AID)
                {
                    try
                    {
                        if (AID != null)
                        {
                            var pai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.AID == AID);

                            if (pai != null)
                            {
                                ProxyConfig.Account.lstAccountInfo.Remove(pai);
                                ProxyConfig.List.CloseProxyTCP_ByAID(AID);

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

                public static bool DeleteProxyAccount_ByUserName(string UserName)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(UserName))
                        {
                            var pai = ProxyConfig.Account.lstAccountInfo.FirstOrDefault(account => account.UserName == UserName);

                            if (pai != null)
                            {
                                ProxyConfig.Account.lstAccountInfo.Remove(pai);

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

                public static void AccountListClear()
                {
                    try
                    {
                        ProxyConfig.Account.lstAccountInfo.Clear();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            foreach (AccountInfo ai in aiList)
                            {
                                switch (AddType)
                                {
                                    case 0:

                                        ai.ExpiryTime = ai.ExpiryTime.AddHours(AddHours);

                                        break;

                                    case 1:

                                        if (ai.ExpiryTime >= DateTime.Now)
                                        {
                                            ai.ExpiryTime = ai.ExpiryTime.AddHours(AddHours);
                                        }
                                        else
                                        {
                                            ai.ExpiryTime = DateTime.Now.AddHours(AddHours);
                                        }

                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                ai.IsLimitLinks = IsLimitLinks;
                                ai.LimitLinks = LimitLinks;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                ai.IsLimitDevices = IsLimitDevices;
                                ai.LimitDevices = LimitDevices;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return dtReturn;
                }

                #endregion

                #region//保存代理账号列表到数据库

                public static void SaveAccountList_ToDB()
                {
                    try
                    {
                        DataBase.DeleteTable_ProxyAccount();
                        DataBase.InsertTable_ProxyAccount();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
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
                                bool DoEncrypt = false;
                                string Password = string.Empty;

                                using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Export))
                                {
                                    string Title = AntdUI.Localization.Get("ExportProxyAccountList", "导出代理账号列表");
                                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                    {
                                        Keyboard = false,
                                        MaskClosable = false,
                                        OnOk = config =>
                                        {
                                            Password = eForm.GetPassword();
                                            if (string.IsNullOrEmpty(Password))
                                            {
                                                eForm.EncryptionText_Changed();

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

                                if (SaveAccountList(FilePath, aiList, DoEncrypt, Password))
                                {
                                    string Title = AntdUI.Localization.Get("InjectModeForm.ExportProxyAccountList.Success", "导出代理账号列表成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            aiList = ProxyConfig.Account.lstAccountInfo.ToList();
                        }

                        foreach (AccountInfo ai in aiList)
                        {
                            XElement xeProxyAccount =
                                    new XElement("ProxyAccount",
                                    new XElement("IsEnable", ai.IsEnable.ToString()),
                                    new XElement("AID", ai.AID.ToString().ToUpper()),
                                    new XElement("UserName", ai.UserName),
                                    new XElement("PassWord", ai.Password),
                                    new XElement("IsOnLine", ai.IsOnLine.ToString()),
                                    new XElement("IsLimitLinks", ai.IsLimitLinks),
                                    new XElement("LimitLinks", ai.LimitLinks),
                                    new XElement("IsLimitDevices", ai.IsLimitDevices),
                                    new XElement("LimitDevices", ai.LimitDevices),
                                    new XElement("IsExpiry", ai.IsExpiry),
                                    new XElement("ExpiryTime", ai.ExpiryTime.ToString("yyyy/MM/dd HH:mm:ss")),
                                    new XElement("CreateTime", ai.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                    );

                            if (ai.AIPInfo.Count > 0)
                            {
                                XElement xeAccountIPInfo = new XElement("AccountIPInfo");

                                foreach (AccountIPInfo aii in ai.AIPInfo)
                                {
                                    XElement xeIPInfo =
                                        new XElement("IPInfo",
                                        new XElement("LoginTime", aii.LoginTime),
                                        new XElement("LoginIP", aii.LoginIP)
                                        );

                                    xeAccountIPInfo.Add(xeIPInfo);
                                }

                                xeProxyAccount.Add(xeAccountIPInfo);
                            }

                            xeProxyAccountList.Add(xeProxyAccount);
                        }

                        return xeProxyAccountList;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    if (form is InterfaceInfo.IProxyMode proxyMode)
                                    {
                                        proxyMode.RefreshAccountList();
                                    }
                                });                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                            using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Import))
                                            {
                                                string Title = AntdUI.Localization.Get("ImportProxyAccountList", "导入代理账号列表");
                                                AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                                {
                                                    Keyboard = false,
                                                    MaskClosable = false,
                                                    OnOk = config =>
                                                    {
                                                        string sPW = eForm.GetPassword();
                                                        if (string.IsNullOrEmpty(sPW))
                                                        {
                                                            eForm.EncryptionText_Changed();

                                                            AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                                            {
                                                                LocalizationText = "ImportList.Error"
                                                            });

                                                            return false;
                                                        }
                                                        else
                                                        {
                                                            xdoc = SystemConfig.DecryptXMLFile(FilePath, sPW);
                                                            return true;
                                                        }
                                                    }
                                                });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        xdoc = XDocument.Load(FilePath);
                                    }

                                    if (xdoc == null)
                                    {
                                        string sError = AntdUI.Localization.Get("System.Import.Error", "导入失败: 密码错误");
                                        if (LoadFromUser)
                                        {
                                            AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                        }
                                        else
                                        {
                                            Operate.DoLog(MethodBase.GetCurrentMethod().Name, sError);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                            Guid AID = Guid.NewGuid();

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

                            bool IsOnLine = false;
                            if (xeProxyAccount.Element("IsOnLine") != null)
                            {
                                IsOnLine = bool.Parse(xeProxyAccount.Element("IsOnLine").Value);
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
                                Operate.DoLog(MethodBase.GetCurrentMethod().Name, FailLog);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                private static void AddAccount_FromIniFile(AccountInfo pai)
                {
                    try
                    {
                        if (pai != null)
                        {
                            if (pai.AID == null || pai.AID == Guid.Empty)
                            {
                                pai.AID = Guid.NewGuid();
                            }

                            if (pai.ExpiryTime == DateTime.MinValue)
                            {
                                pai.ExpiryTime = DateTime.Now;
                            }

                            if (pai.CreateTime == DateTime.MinValue)
                            {
                                pai.CreateTime = DateTime.Now;
                            }
                            
                            pai.IsLimitDevices = true;
                            pai.LimitDevices = 1;

                            bool bOK = ProxyConfig.Account.AddProxyAccount(
                                pai.AID,
                                pai.IsEnable,
                                pai.UserName,
                                pai.Password,
                                new BindingList<AccountIPInfo>(),
                                pai.IsLimitLinks,
                                pai.LimitLinks,
                                pai.IsLimitDevices,
                                pai.LimitDevices,
                                pai.IsExpiry,
                                pai.ExpiryTime,
                                pai.CreateTime);

                            if (!bOK)
                            {
                                string FailLog = string.Format(AntdUI.Localization.Get("ImportAccount.Error", "导入账号失败！用户名：{0}"), pai.UserName);
                                Operate.DoLog(MethodBase.GetCurrentMethod().Name, FailLog);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return MProtocol;
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
                        DoLog(MethodBase.GetCurrentMethod().Name, $"远程映射失败: {ex.Message}");

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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//删除本地代理映射（对话框）

                public static void DeleteMapLocal_Dialog(Form form, MapLocal ml)
                {
                    try
                    {
                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miAccountList", "本地映射"), "\r\n确定删除数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//删除远程代理映射

                public static void DeleteMapRemote_Dialog(Form form, MapRemote mr)
                {
                    try
                    {
                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miAccountList", "远程映射"), "\r\n确定删除数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//清空本地代理映射（对话框）

                public static void CleanUpMapLocal_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miSendList", "本地映射"), "\r\n确定删除所有数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//清空远程代理映射（对话框）

                public static void CleanUpMapRemote_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miSendList", "远程映射"), "\r\n确定删除所有数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//查找本地代理映射

                public static MapLocal GetMapLocal(ProxyConfig.Proxy.MapProtocol ProtocolType, string host, int port, string path)
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        return ProxyConfig.Mapping.lstMapLocal.FirstOrDefault(rule =>
                        rule.IsEnable == true &&
                        rule.ProtocolType == ProtocolType &&
                        rule.Host.Equals(host, StringComparison.OrdinalIgnoreCase) &&
                        rule.Port == port);
                    }
                    else
                    {
                        return ProxyConfig.Mapping.lstMapLocal.FirstOrDefault(rule =>
                        rule.IsEnable == true &&
                        rule.ProtocolType == ProtocolType &&
                        rule.Host.Equals(host, StringComparison.OrdinalIgnoreCase) &&
                        rule.Port == port &&
                        path.StartsWith(rule.RemotePath, StringComparison.OrdinalIgnoreCase));
                    }

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

                    menuItems.Add(new AntdUI.ContextMenuStripItem("置顶", "Ctrl+向上键")
                    {
                        ID = "Top",
                        IconSvg = "VerticalAlignTopOutlined",
                        LocalizationText = "InjectModeForm.cmsFilterList.Top",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("向上移动", "Alt+向上键")
                    {
                        ID = "Up",
                        IconSvg = "ArrowUpOutlined",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItem("向下移动", "Alt+向下键")
                    {
                        ID = "Down",
                        IconSvg = "ArrowDownOutlined",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("置底", "Ctrl+向下键")
                    {
                        ID = "Bottom",
                        IconSvg = "VerticalAlignBottomOutlined",
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    bool DoEncrypt = false;
                                    string Password = string.Empty;

                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Export))
                                    {
                                        string Title = AntdUI.Localization.Get("ExportMapLocal", "导出本地映射");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                Password = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(Password))
                                                {
                                                    eForm.EncryptionText_Changed();

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

                                    if (SaveMapLocal(FilePath, pmlList, DoEncrypt, Password))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportMapLocal.Success", "导出本地映射成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    bool DoEncrypt = false;
                                    string Password = string.Empty;

                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Export))
                                    {
                                        string Title = AntdUI.Localization.Get("ExportMapRemote", "导出远程映射");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                Password = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(Password))
                                                {
                                                    eForm.EncryptionText_Changed();

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

                                    if (SaveMapRemote(FilePath, pmrList, DoEncrypt, Password))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportMapRemote.Success", "导出远程映射成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Import))
                                    {
                                        string Title = AntdUI.Localization.Get("ImportMapLocal", "导入本地映射");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                string sPW = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(sPW))
                                                {
                                                    eForm.EncryptionText_Changed();

                                                    AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                                    {
                                                        LocalizationText = "ImportList.Error"
                                                    });

                                                    return false;
                                                }
                                                else
                                                {
                                                    xdoc = SystemConfig.DecryptXMLFile(FilePath, sPW);
                                                    return true;
                                                }
                                            }
                                        });
                                    }
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("System.Import.Error", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                                }

                                return false;
                            }

                            LoadMapLocal_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Import))
                                    {
                                        string Title = AntdUI.Localization.Get("ImportMapRemote", "导入远程映射");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                string sPW = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(sPW))
                                                {
                                                    eForm.EncryptionText_Changed();

                                                    AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                                    {
                                                        LocalizationText = "ImportList.Error"
                                                    });

                                                    return false;
                                                }
                                                else
                                                {
                                                    xdoc = SystemConfig.DecryptXMLFile(FilePath, sPW);
                                                    return true;
                                                }
                                            }
                                        });
                                    }
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("System.Import.Error", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                                }

                                return false;
                            }

                            LoadMapRemote_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                public static bool SpeedMode;
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
                                    res = WSock32.send(Socket, ipSend, bSendBuffer.Length, SocketFlags.None);
                                    break;
                                case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                                case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                                case Operate.PacketConfig.Packet.PacketType.WSASend:
                                case Operate.PacketConfig.Packet.PacketType.WSARecv:
                                case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
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

                #region//获取封包收发速率

                public static string GetPacketSpeedInfo()
                {
                    string sReturn = string.Empty;

                    try
                    {
                        string sTotal_SendBytes = Operate.SystemConfig.GetDisplayBytes(Operate.PacketConfig.Packet.Total_SendBytes);
                        string sTotal_RecvBytes = Operate.SystemConfig.GetDisplayBytes(Operate.PacketConfig.Packet.Total_RecvBytes);
                        string sSpeedInfo = AntdUI.Localization.Get("InjectModeForm.SpeedInfo", "发送: {0}  接收: {1}");
                        sReturn = string.Format(sSpeedInfo, sTotal_SendBytes, sTotal_RecvBytes);
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return ptReturn;
                }

                #endregion

                #region//获取封包类型对应的名称

                private static class PacketTypeNames
                {
                    public static readonly string WS1_Send = AntdUI.Localization.Get("WS1_Send", "发送 1.1");
                    public static readonly string WS2_Send = AntdUI.Localization.Get("WS2_Send", "发送");
                    public static readonly string WS1_Recv = AntdUI.Localization.Get("WS1_Recv", "接收 1.1");
                    public static readonly string WS2_Recv = AntdUI.Localization.Get("WS2_Recv", "接收");
                    public static readonly string WS1_SendTo = AntdUI.Localization.Get("WS1_SendTo", "发送到 1.1");
                    public static readonly string WS2_SendTo = AntdUI.Localization.Get("WS2_SendTo", "发送到");
                    public static readonly string WS1_RecvFrom = AntdUI.Localization.Get("WS1_RecvFrom", "接收自 1.1");
                    public static readonly string WS2_RecvFrom = AntdUI.Localization.Get("WS2_RecvFrom", "接收自");
                    public static readonly string WSASend = AntdUI.Localization.Get("WSASend", "WSA发送");
                    public static readonly string WSARecv = AntdUI.Localization.Get("WSARecv", "WSA接收");
                    public static readonly string WSARecvEx = AntdUI.Localization.Get("WSARecvEx", "WSA接收");
                    public static readonly string WSASendTo = AntdUI.Localization.Get("WSASendTo", "WSA发送到");
                    public static readonly string WSARecvFrom = AntdUI.Localization.Get("WSARecvFrom", "WSA接收自");
                    public static readonly string TCP_Req = AntdUI.Localization.Get("TCP_Req", "TCP");
                    public static readonly string UDP_Req = AntdUI.Localization.Get("UDP_Req", "UDP");
                    public static readonly string TCP_Resp = AntdUI.Localization.Get("TCP_Resp", "TCP");
                    public static readonly string UDP_Resp = AntdUI.Localization.Get("UDP_Resp", "UDP");
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        return string.Empty;
                    }
                }

                #endregion

                #region//获取封包类型对应的图标

                private static readonly ConcurrentDictionary<PacketType, string> _packetImageCache = new ConcurrentDictionary<PacketType, string>();

                public static string GetImg_ByPacketType(PacketType ptType)
                {
                    string sReturn = string.Empty;

                    try
                    {
                        if (_packetImageCache.TryGetValue(ptType, out var cachedImage))
                        {
                            return cachedImage;
                        }
                        
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
                                sReturn = Properties.Resources.Send;
                                break;

                            case PacketConfig.Packet.PacketType.WS1_Recv:
                            case PacketConfig.Packet.PacketType.WS2_Recv:
                            case PacketConfig.Packet.PacketType.WS1_RecvFrom:
                            case PacketConfig.Packet.PacketType.WS2_RecvFrom:
                            case PacketConfig.Packet.PacketType.WSARecv:
                            case PacketConfig.Packet.PacketType.WSARecvEx:
                            case PacketConfig.Packet.PacketType.WSARecvFrom:
                            case PacketConfig.Packet.PacketType.TCP_Resp:
                            case PacketConfig.Packet.PacketType.UDP_Resp:
                                sReturn = Properties.Resources.Recv;
                                break;

                            default:
                                break;
                        }

                        if (string.IsNullOrEmpty(sReturn))
                        {
                            _packetImageCache[ptType] = sReturn;
                        }                        
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return sReturn;
                }

                #endregion

                #region//是否显示封包（过滤条件）

                public static bool IsShowPacket_ByFilter(PacketInfo pi)
                {
                    try
                    {
                        //套接字
                        if (SystemConfig.CheckSocket)
                        {
                            bool bIsFilter = IsFilter_BySocket(pi.PacketSocket);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //IP地址
                        if (SystemConfig.CheckIP)
                        {
                            bool bIsFilter_From = IsFilter_ByIP(pi.PacketFrom);
                            bool bIsFilter_To = IsFilter_ByIP(pi.PacketTo);
                            if (SystemConfig.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                            {
                                return false;
                            }
                        }

                        //端口号
                        if (SystemConfig.CheckPort)
                        {
                            bool bIsFilter_From = IsFilter_ByPort(pi.PacketFrom);
                            bool bIsFilter_To = IsFilter_ByPort(pi.PacketTo);
                            if (SystemConfig.CheckNotShow == (bIsFilter_From || bIsFilter_To))
                            {
                                return false;
                            }
                        }

                        //指定包头
                        if (SystemConfig.CheckHead)
                        {
                            bool bIsFilter = IsFilter_ByHead(pi.PacketBuffer);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //封包内容
                        if (SystemConfig.CheckData)
                        {
                            bool bIsFilter = IsFilter_ByPacket(pi.PacketBuffer);
                            if (SystemConfig.CheckNotShow == bIsFilter)
                            {
                                return false;
                            }
                        }

                        //封包大小
                        if (SystemConfig.CheckLen)
                        {
                            bool bIsFilter = IsFilter_BySize(pi.PacketLen);
                            if (SystemConfig.CheckNotShow == bIsFilter)
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        LocalizationText = "InjectModeForm.Edit",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    if (SendConfig.List.lstSendInfo.Count > 0)
                    {
                        menuItems.Add(new AntdUI.ContextMenuStripItem("添加到发送列表")
                        {
                            ID = "ToSendList",
                            IconSvg = "PlaySquareOutlined",
                            LocalizationText = "InjectModeForm.ToSendList",
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
                            LocalizationText = "InjectModeForm.ToSendList",
                        });
                    }

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到滤镜列表")
                    {
                        ID = "ToFilterList",
                        IconSvg = "FunnelPlotOutlined",
                        LocalizationText = "InjectModeForm.ToFilterList",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("复制")
                    {
                        Enabled = hbPacketData.CanCopy(),
                        ID = "Copy",
                        IconSvg = "CopyOutlined",
                        Sub = new AntdUI.IContextMenuStripItem[]
                        {
                            new AntdUI.ContextMenuStripItem("复制文本")
                            {
                                Enabled = hbPacketData.CanCopy(),
                                ID = "Copy_Text",
                                IconSvg = "CopyOutlined",
                            },
                            new AntdUI.ContextMenuStripItem("复制十六进制")
                            {
                                Enabled = hbPacketData.CanCopy(),
                                ID = "Copy_Hex",
                                IconSvg = "CopyOutlined",
                            },
                        },
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到文本A")
                    {
                        ID = "ToTextA",
                        IconSvg = "FontColorsOutlined",
                        LocalizationText = "InjectModeForm.ToTextA",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到文本B")
                    {
                        ID = "ToTextB",
                        IconSvg = "BoldOutlined",
                        LocalizationText = "InjectModeForm.ToTextB",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("全选")
                    {
                        ID = "SelectAll",
                        IconSvg = "ProfileOutlined",
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
                            LocalizationText = "InjectModeForm.cmsToSendList",
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
                            LocalizationText = "InjectModeForm.cmsToSendList",
                        });
                    }

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到滤镜列表")
                    {
                        ID = "ToFilterList",
                        IconSvg = "FunnelPlotOutlined",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("剪切")
                    {
                        Enabled = hbPacketData.CanCut(),
                        ID = "Cut",
                        IconSvg = "ScissorOutlined",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("复制")
                    {
                        Enabled = hbPacketData.CanCopy(),
                        ID = "Copy",
                        IconSvg = "CopyOutlined",
                        Sub = new AntdUI.IContextMenuStripItem[]
                        {
                            new AntdUI.ContextMenuStripItem("复制文本")
                            {
                                Enabled = hbPacketData.CanCopy(),
                                ID = "Copy_Text",
                                IconSvg = "CopyOutlined",
                            },
                            new AntdUI.ContextMenuStripItem("复制十六进制")
                            {
                                Enabled = hbPacketData.CanCopy(),
                                ID = "Copy_Hex",
                                IconSvg = "CopyOutlined",
                            },
                        },
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("粘贴")
                    {
                        Enabled = hbPacketData.CanPaste(),
                        ID = "Paste",
                        IconSvg = "SnippetsOutlined",
                        Sub = new AntdUI.IContextMenuStripItem[]
                        {
                            new AntdUI.ContextMenuStripItem("粘贴文本")
                            {
                                Enabled = hbPacketData.CanPaste(),
                                ID = "Paste_Text",
                                IconSvg = "SnippetsOutlined",
                            },
                            new AntdUI.ContextMenuStripItem("粘贴十六进制")
                            {
                                Enabled = hbPacketData.CanPasteHex(),
                                ID = "Paste_Hex",
                                IconSvg = "SnippetsOutlined",
                            },
                        },
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("全选")
                    {
                        ID = "SelectAll",
                        IconSvg = "ProfileOutlined",
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
                            string sIP = Marshal.PtrToStringAnsi(WS2_32.inet_ntoa(saAddr.sin_addr));
                            string sPort = WS2_32.ntohs(saAddr.sin_port).ToString();
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                        if (!PacketConfig.Packet.SpeedMode)
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion
            }

            #endregion

            #region//封包列表

            public static class List
            {
                public static bool AutoRoll = false;
                public static bool AutoClear = true;
                public static decimal AutoClear_Value = 5000;
                public static int Search_Index = -1;
                public static FindOptions FindOptions = new FindOptions();
                public static PacketInfo piSelect;
                public static List<PacketInfo> lstPacketInfo = new List<PacketInfo>();

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

                                PacketConfig.List.lstPacketInfo.Add(pi);
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

                #region//搜索封包列表

                public static int SearchForPacketList(int fromIndex, ReadOnlySpan<byte> searchData)
                {
                    int iResult = -1;

                    try
                    {
                        if (searchData.Length == 0 || fromIndex < 0)
                        {
                            return -1;
                        }

                        int listCount = PacketConfig.List.lstPacketInfo.Count;
                        if (listCount == 0 || fromIndex >= listCount)
                        {
                            return -1;
                        }

                        if (fromIndex == -1)
                        {
                            fromIndex = 0;
                        }

                        for (int i = fromIndex; i < listCount; i++)
                        {
                            byte[] packetBuffer = PacketConfig.List.lstPacketInfo[i].PacketBuffer;
                            if (packetBuffer != null && packetBuffer.Length >= searchData.Length)
                            {
                                ReadOnlySpan<byte> packetSpan = packetBuffer.AsSpan();
                                if (packetSpan.IndexOf(searchData) != -1)
                                {
                                    return i;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return iResult;
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return dtReturn;
                }

                public static DataTable StatisticalFilterList_ByExecutionCount()
                {
                    DataTable dtReturn = new DataTable();
                    dtReturn.Columns.Add("FilterExecution", typeof(string));
                    dtReturn.Columns.Add("Number", typeof(int));

                    try
                    {
                        foreach (FilterInfo sfi in FilterConfig.List.lstFilterInfo)
                        {
                            if (sfi.ExecutionCount > 0)
                            {
                                DataRow row = dtReturn.NewRow();
                                row[0] = sfi.FName;
                                row[1] = sfi.ExecutionCount;
                                dtReturn.Rows.Add(row);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        LocalizationText = "InjectModeForm.Edit",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    if (SendConfig.List.lstSendInfo.Count > 0)
                    {
                        menuItems.Add(new AntdUI.ContextMenuStripItem("添加到发送列表")
                        {
                            ID = "ToSendList",
                            IconSvg = "PlaySquareOutlined",
                            LocalizationText = "InjectModeForm.ToSendList",
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
                            LocalizationText = "InjectModeForm.ToSendList",                            
                        });
                    }

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到滤镜列表")
                    {
                        ID = "ToFilterList",
                        IconSvg = "FunnelPlotOutlined",
                        LocalizationText = "InjectModeForm.ToFilterList",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("设置系统套接字")
                    {
                        ID = "SYSSocket",
                        IconSvg = "CheckSquareOutlined",
                        LocalizationText = "InjectModeForm.SYSSocket",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("查看数据修改")
                    {
                        ID = "PacketModification",
                        IconSvg = "FormOutlined",
                        LocalizationText = "InjectModeForm.PacketModification",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("导出到Excel")
                    {
                        ID = "ToExcel",
                        IconSvg = "FileExcelOutlined",
                        LocalizationText = "InjectModeForm.ToExcel",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到文本A")
                    {
                        ID = "ToTextA",
                        IconSvg = "FontColorsOutlined",
                        LocalizationText = "InjectModeForm.ToTextA",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("添加到文本B")
                    {
                        ID = "ToTextB",
                        IconSvg = "BoldOutlined",
                        LocalizationText = "InjectModeForm.ToTextB",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("取消选择")
                    {
                        ID = "DeSelect",
                        IconSvg = "DeleteRowOutlined",
                        LocalizationText = "InjectModeForm.ToTextA",
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//保存封包列表为Excel（对话框）

                public static void SavePacketList_Dialog(Form form, AntdUI.Table tTable, string FileName, List<PacketInfo> piList)
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
                                    bool bOK = false;
                                    tTable.Spin(AntdUI.Localization.Get("Exporting", "正在导出..."), config =>
                                    {
                                        bOK = SavePacketListToExcel(FilePath, piList);
                                    }, () =>
                                    {
                                        if (bOK)
                                        {
                                            string Title = AntdUI.Localization.Get("InjectModeForm.ExportToExcel.Success", "导出到Excel成功");
                                            AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                            Operate.DoLog(nameof(SavePacketList_Dialog), Title + ": " + FilePath);
                                        }
                                        else
                                        {
                                            string Title = AntdUI.Localization.Get("InjectModeForm.ExportToExcel.Error", "导出到Excel失败");
                                            string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                            AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                        }
                                    });                                    
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                private static bool SavePacketListToExcel(string filePath, List<PacketInfo> piList)
                {
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        using (var writer = new StreamWriter(stream, Encoding.Default))
                        {
                            writer.WriteLine(AntdUI.Localization.Get("ToExcelTitle", "时间戳\t类别\t套接字\t源地址\t目的地址\t长度\t数据\t"));

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
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                public static int FilterSize_MaxLen = 500;
                public static FilterConfig.Filter.Execute FilterExecute = FilterConfig.Filter.Execute.Sequence;
                public static readonly Color FilterActionForeColor_Replace = Color.Black;
                public static readonly Color FilterActionBackColor_Replace = Color.Goldenrod;
                public static readonly Color FilterActionForeColor_Intercept = Color.White;
                public static readonly Color FilterActionBackColor_Intercept = Color.DarkRed;
                public static readonly Color FilterActionForeColor_Change = Color.Black;
                public static readonly Color FilterActionBackColor_Change = Color.DodgerBlue;
                public static readonly Color FilterActionForeColor_Other = Color.LimeGreen;
                public static readonly Color FilterActionBackColor_Other = Color.FromArgb(30, 30, 30);

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
                        string FName = string.Format(AntdUI.Localization.Get("NewFilterName", "滤镜 {0}"), FNum.ToString());

                        FilterConfig.Filter.FilterMode FilterMode = FilterConfig.Filter.FilterMode.Normal;
                        FilterConfig.Filter.FilterAction FilterAction = FilterConfig.Filter.FilterAction.Replace;
                        FilterConfig.Filter.FilterExecuteType FilterExecuteType = FilterExecuteType.None;
                        Guid SID = Guid.Empty;
                        Guid RID = Guid.Empty;
                        FilterConfig.Filter.FilterFunction FilterFunction = new FilterConfig.Filter.FilterFunction(true, true, true, true, true, true, true, true, true, true, true, true);
                        FilterConfig.Filter.FilterStartFrom FilterStartFrom = FilterConfig.Filter.FilterStartFrom.Head;

                        FilterConfig.Filter.AddFilter(false, FID, FName, false, string.Empty, false, string.Empty, false, string.Empty, false, string.Empty, FilterMode, FilterAction, false, FilterExecuteType, SID, RID, FilterFunction, FilterStartFrom, false, false, 1, false, 1, string.Empty, 0, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            FilterConfig.Filter.FilterExecuteType FilterExecuteType = FilterExecuteType.None;
                            Guid SID = Guid.Empty;
                            Guid RID = Guid.Empty;
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
                                FilterExecuteType, 
                                SID, 
                                RID, 
                                FilterFunction, 
                                FilterStartFrom, 
                                false, 
                                false, 
                                1, 
                                false, 
                                1, 
                                string.Empty, 
                                0, 
                                sFSearch, 
                                string.Empty);

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            FilterConfig.Filter.FilterExecuteType FilterExecuteType = FilterExecuteType.None;
                            Guid SID = Guid.Empty;
                            Guid RID = Guid.Empty;
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
                                FilterExecuteType,
                                SID,
                                RID,
                                FilterFunction,
                                FilterStartFrom,
                                false,
                                false,
                                1,
                                false,
                                1,
                                string.Empty,
                                0,
                                sFSearch,
                                string.Empty);

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    FilterConfig.Filter.FilterExecuteType FEType,
                    Guid SID,
                    Guid RID,
                    FilterConfig.Filter.FilterFunction FilterFunction,
                    FilterConfig.Filter.FilterStartFrom FilterStartFrom,
                    bool IsProgressionDone,
                    bool IsProgressionContinuous,
                    int ProgressionStep,
                    bool IsProgressionCarry,
                    int ProgressionCarryNumber,
                    string ProgressionPosition,
                    int ProgressionCount,
                    string FSearch,
                    string FModify)
                {
                    try
                    {
                        if (FID != null && !string.IsNullOrEmpty(FName))
                        {
                            FilterInfo sfi = new FilterInfo(
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
                            SID,
                            RID,
                            FilterFunction,
                            FilterStartFrom,
                            IsProgressionDone,
                            IsProgressionContinuous,
                            ProgressionStep,
                            IsProgressionCarry,
                            ProgressionCarryNumber,
                            ProgressionPosition,
                            ProgressionCount,
                            FSearch,
                            FModify);

                            FilterConfig.List.FilterToList(sfi);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//更新滤镜

                public static void UpdateFilter(
                    FilterInfo sfi,
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
                    FilterConfig.Filter.FilterExecuteType FEType,
                    Guid SID,
                    Guid RID,
                    FilterConfig.Filter.FilterFunction FilterFunction,
                    FilterConfig.Filter.FilterStartFrom FilterStartFrom,
                    bool IsProgressionContinuous,
                    int ProgressionStep,
                    bool IsProgressionCarry,
                    int ProgressionCarryNumber,
                    string ProgressionPosition,
                    int ProgressionCount,
                    string FSearch,
                    string FModify)
                {
                    try
                    {
                        if (sfi != null)
                        {
                            sfi.FName = FName;
                            sfi.AppointHeader = AppointHeader;
                            sfi.HeaderContent = HeaderContent;
                            sfi.AppointSocket = AppointSocket;
                            sfi.SocketContent = SocketContent;
                            sfi.AppointLength = AppointLength;
                            sfi.LengthContent = LengthContent;
                            sfi.AppointPort = AppointPort;
                            sfi.PortContent = PortContent;
                            sfi.FMode = FilterMode;
                            sfi.FAction = FilterAction;
                            sfi.IsExecute = IsExecute;
                            sfi.FEType = FEType;
                            sfi.SID = SID;
                            sfi.RID = RID;
                            sfi.FFunction = FilterFunction;
                            sfi.FStartFrom = FilterStartFrom;
                            sfi.IsProgressionContinuous = IsProgressionContinuous;
                            sfi.ProgressionStep = ProgressionStep;
                            sfi.IsProgressionCarry = IsProgressionCarry;
                            sfi.ProgressionCarryNumber = ProgressionCarryNumber;
                            sfi.ProgressionPosition = ProgressionPosition;
                            sfi.ProgressionCount = ProgressionCount;
                            sfi.FSearch = FSearch;
                            sfi.FModify = FModify;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miFilterList", "滤镜列表"), "\r\n确定删除选中的数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//复制滤镜

                public static void CopyFilter(FilterInfo sfi)
                {
                    try
                    {
                        bool IsEnable = false;
                        Guid FID = Guid.NewGuid();
                        string FName = string.Format(AntdUI.Localization.Get("CopyName", "{0} - 副本"), sfi.FName);
                        bool bAppointHeader = sfi.AppointHeader;
                        string HeaderContent = sfi.HeaderContent;
                        bool bAppointSocket = sfi.AppointSocket;
                        string SocketContent = sfi.SocketContent;
                        bool bAppointLength = sfi.AppointLength;
                        string LengthContent = sfi.LengthContent;
                        bool bAppointPort = sfi.AppointPort;
                        string PortContent = sfi.PortContent;
                        FilterConfig.Filter.FilterMode FMode = sfi.FMode;
                        FilterConfig.Filter.FilterAction FAction = sfi.FAction;
                        bool IsExecute = sfi.IsExecute;
                        FilterConfig.Filter.FilterExecuteType FEType = sfi.FEType;
                        Guid SID = sfi.SID;
                        Guid RID = sfi.RID;
                        FilterConfig.Filter.FilterFunction FFunction = sfi.FFunction;
                        FilterConfig.Filter.FilterStartFrom FStartFrom = sfi.FStartFrom;
                        bool IsProgressionDone = false;
                        bool IsProgressionContinuous = sfi.IsProgressionContinuous;
                        int ProgressionStep = sfi.ProgressionStep;
                        bool IsProgressionCarry = sfi.IsProgressionCarry;
                        int ProgressionCarryNumber = sfi.ProgressionCarryNumber;
                        string ProgressionPosition = sfi.ProgressionPosition;
                        int ProgressionCount = 0;
                        string FSearch = sfi.FSearch;
                        string FModify = sfi.FModify;

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
                            SID,
                            RID,
                            FFunction,
                            FStartFrom,
                            IsProgressionDone,
                            IsProgressionContinuous,
                            ProgressionStep,
                            IsProgressionCarry,
                            ProgressionCarryNumber,
                            ProgressionPosition,
                            ProgressionCount,
                            FSearch,
                            FModify);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                return AntdUI.Localization.Get("NoModify_Display", "不修改-只显示");

                            case FilterConfig.Filter.FilterAction.NoModify_NoDisplay:
                                return AntdUI.Localization.Get("NoModify_NoDisplay", "不修改-不显示");

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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return sReturn;
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                #region//检查滤镜作用类别

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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                        
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return false;
                }

                #endregion

                #region//检查滤镜是否匹配成功（普通滤镜）

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool CheckFilter_IsMatch_Normal(FilterInfo sfi, ReadOnlySpan<byte> bufferSpan)
                {
                    if (string.IsNullOrEmpty(sfi.FSearch))
                        return false;

                    try
                    {
                        string[] searchParts = sfi.FSearch.Split(',');
                        foreach (string part in searchParts)
                        {
                            if (!string.IsNullOrEmpty(part) && part.IndexOf('|') > 0)
                            {
                                string[] pair = part.Split('|');
                                if (pair.Length != 2)
                                    return false;

                                if (!TryParseNonNegativeInt(pair[0], out int index) ||
                                    index >= bufferSpan.Length)
                                {
                                    return false;
                                }

                                if (pair[1].Length != 2 ||
                                    !HexCharsToByte(pair[1], out byte expected) ||
                                    bufferSpan[index] != expected)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        return false;
                    }

                    return true;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static bool TryParseNonNegativeInt(string s, out int result)
                {
                    return int.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out result) &&
                           result >= 0;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static bool HexCharsToByte(string s, out byte result)
                {
                    result = 0;
                    if (s.Length != 2) return false;

                    int high = CharToNibble(s[0]);
                    int low = CharToNibble(s[1]);
                    if (high == -1 || low == -1)
                        return false;

                    result = (byte)((high << 4) | low);
                    return true;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static int CharToNibble(char c)
                {
                    if (c >= '0' && c <= '9') return c - '0';
                    if (c >= 'A' && c <= 'F') return 10 + (c - 'A');
                    if (c >= 'a' && c <= 'f') return 10 + (c - 'a');
                    return -1;
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
                        var searchConditions = FilterConfig.Filter.ParseSearchConditions(sfi.FSearch);
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

                                    if (checkIndex < 0 || checkIndex >= bufferSpan.Length ||
                                        bufferSpan[checkIndex] != condition.Value)
                                    {
                                        isMatch = false;
                                        break;
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                        if (int.TryParse(pair[0], out int position) &&
                            byte.TryParse(pair[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                        {
                            conditions.Add(new FilterConfig.Filter.SearchCondition
                            {
                                RelativePosition = position,
                                Value = value
                            });
                        }
                    }

                    return conditions;
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//清空滤镜列表（对话框）

                public static void CleanUpFilterList_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miFilterList", "滤镜列表"), "\r\n确定删除所有数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    bool bBreak = false;
                    bNewBuffer = null;

                    try
                    {
                        var filters = FilterConfig.List.lstFilterInfo;
                        for (int i = 0; i < filters.Count; i++)
                        {
                            var sfi = filters[i];
                            if (!FilterConfig.Filter.CheckFilter_IsEffective(iSocket, bufferSpan, ptType, sAddr, sfi))
                            {
                                continue;
                            }

                            bool bDoFilter = false;
                            bool isMatch = false;
                            List<int> MatchIndex = null;

                            if (sfi.FMode == FilterConfig.Filter.FilterMode.Normal)
                            {
                                isMatch = FilterConfig.Filter.CheckFilter_IsMatch_Normal(sfi, bufferSpan);
                            }
                            else if (sfi.FMode == FilterConfig.Filter.FilterMode.Advanced)
                            {
                                MatchIndex = FilterConfig.Filter.CheckFilter_IsMatch_Advanced(sfi, bufferSpan);
                                isMatch = MatchIndex != null && MatchIndex.Count > 0;
                            }
                            
                            if (!isMatch)
                            {
                                continue;
                            }

                            byte[] tempBuffer = null;

                            switch (sfi.FAction)
                            {
                                case FilterConfig.Filter.FilterAction.Replace:

                                    sfi.IsProgressionDone = false;

                                    if (sfi.FMode == FilterConfig.Filter.FilterMode.Normal)
                                    {
                                        bDoFilter = FilterConfig.Filter.Replace_Normal(sfi, bufferSpan);
                                        if (bDoFilter)
                                        {
                                            tempBuffer = bufferSpan.ToArray();
                                        }
                                    }
                                    else if (sfi.FMode == FilterConfig.Filter.FilterMode.Advanced && MatchIndex != null)
                                    {
                                        foreach (int iIndex in MatchIndex)
                                        {
                                            bDoFilter = FilterConfig.Filter.Replace_Advanced(sfi, iIndex, bufferSpan);
                                        }

                                        tempBuffer = bufferSpan.ToArray();
                                    }

                                    if (sfi.IsProgressionDone && sfi.IsProgressionContinuous)
                                    {
                                        sfi.ProgressionCount++;
                                    }

                                    break;

                                case FilterConfig.Filter.FilterAction.Change:

                                    sfi.IsProgressionDone = false;

                                    tempBuffer = FilterConfig.Filter.ChangePacket_Filter(sfi);
                                    bDoFilter = tempBuffer != null && tempBuffer.Length > 0;

                                    if (sfi.IsProgressionDone && sfi.IsProgressionContinuous)
                                    {
                                        sfi.ProgressionCount++;
                                    }

                                    break;

                                case FilterConfig.Filter.FilterAction.Intercept:
                                case FilterConfig.Filter.FilterAction.NoModify_Display:
                                case FilterConfig.Filter.FilterAction.NoModify_NoDisplay:

                                    bDoFilter = true;
                                    bBreak = true;

                                    break;
                            }

                            if (bDoFilter)
                            {
                                faReturn = sfi.FAction;
                                sfi.ExecutionCount++;

                                switch (sfi.FAction)
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

                                if (sfi.IsExecute)
                                {
                                    switch (sfi.FEType)
                                    {
                                        case FilterConfig.Filter.FilterExecuteType.Send:

                                            SendConfig.Send.DoSend(sfi.SID);

                                            break;
                                        case FilterConfig.Filter.FilterExecuteType.Robot:

                                            var parameters = new Dictionary<string, object>
                                        {
                                            { "FilterSocket", iSocket }
                                        };

                                            RobotConfig.Robot.DoRobot(sfi.RID, parameters);
                                            break;
                                    }
                                }

                                bool bSpeedMode = false;

                                switch (Operate.SystemConfig.StartMode)
                                {
                                    case Operate.SystemConfig.SystemMode.Process:

                                        bSpeedMode = PacketConfig.Packet.SpeedMode;

                                        break;

                                    case Operate.SystemConfig.SystemMode.Proxy:

                                        bSpeedMode = ProxyConfig.Proxy.SpeedMode;

                                        break;
                                }

                                if (!bSpeedMode)
                                {
                                    string sFilterLog = MatchIndex != null && MatchIndex.Count > 0
                                        ? string.Format(AntdUI.Localization.Get("DoFilterMatch", "[{0}] {1} | [{2}] 封包长度: {3} | 匹配数: {4}"),
                                            FilterConfig.Filter.GetName_ByFilterAction(sfi.FAction),
                                            sfi.FName,
                                            PacketConfig.Packet.GetName_ByPacketType(ptType),
                                            bufferSpan.Length,
                                            MatchIndex.Count)
                                        : string.Format(AntdUI.Localization.Get("DoFilter", "[{0}] {1} | [{2}] 封包长度: {3}"),
                                            FilterConfig.Filter.GetName_ByFilterAction(sfi.FAction),
                                            sfi.FName,
                                            PacketConfig.Packet.GetName_ByPacketType(ptType),
                                            bufferSpan.Length);

                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sFilterLog);
                                }

                                if (FilterConfig.Filter.FilterExecute == FilterConfig.Filter.Execute.Priority)
                                {
                                    bBreak = true;
                                }
                            }

                            if (bBreak)
                            {
                                if (bNewBuffer == null)
                                {
                                    bNewBuffer = bufferSpan.ToArray();
                                }

                                return faReturn;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            Guid SID = Guid.Parse(dataRow["Send_GUID"].ToString());
                            Guid RID = Guid.Parse(dataRow["Robot_GUID"].ToString());
                            FilterConfig.Filter.FilterFunction FilterFunction = FilterConfig.Filter.GetFilterFunction_ByString(dataRow["Function"].ToString());
                            FilterConfig.Filter.FilterStartFrom FilterStartFrom = FilterConfig.Filter.GetFilterStartFrom_ByString(dataRow["StartFrom"].ToString());
                            bool IsProgressionDone = false;
                            bool IsProgressionContinuous = Convert.ToBoolean(dataRow["IsProgressionContinuous"]);
                            int FProgressionStep = Convert.ToInt32(dataRow["ProgressionStep"]);
                            bool IsProgressionCarry = Convert.ToBoolean(dataRow["IsProgressionCarry"]);
                            int ProgressionCarryNumber = Convert.ToInt32(dataRow["ProgressionCarryNumber"]);
                            string FProgressionPosition = dataRow["ProgressionPosition"].ToString();
                            int ProgressionCount = 0;
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
                                SID,
                                RID,
                                FilterFunction,
                                FilterStartFrom,
                                IsProgressionDone,
                                IsProgressionContinuous,
                                FProgressionStep,
                                IsProgressionCarry,
                                ProgressionCarryNumber,
                                FProgressionPosition,
                                ProgressionCount,
                                FSearch,
                                FModify);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    bool DoEncrypt = false;
                                    string Password = string.Empty;

                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Export))
                                    {
                                        string Title = AntdUI.Localization.Get("ExportFilterList", "导出滤镜列表");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                Password = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(Password))
                                                {
                                                    eForm.EncryptionText_Changed();

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

                                    if (SaveFilterList(FilePath, fiList, DoEncrypt, Password))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportFilterList.Success", "导出滤镜列表成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportFilterList.Error", "导出滤镜列表失败");
                                        string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                new XElement("SendID", fi.SID.ToString().ToUpper()),
                                new XElement("RobotID", fi.RID.ToString().ToUpper()),
                                new XElement("Function", FilterConfig.Filter.GetFilterFunctionString(fi.FFunction)),
                                new XElement("StartFrom", fi.FStartFrom),
                                new XElement("IsProgressionContinuous", fi.IsProgressionContinuous.ToString()),
                                new XElement("ProgressionStep", fi.ProgressionStep),
                                new XElement("IsProgressionCarry", fi.IsProgressionCarry.ToString()),
                                new XElement("ProgressionCarryNumber", fi.ProgressionCarryNumber),
                                new XElement("ProgressionPosition", fi.ProgressionPosition),
                                new XElement("Search", fi.FSearch),
                                new XElement("Modify", fi.FModify)
                                );

                            xeRoot.Add(xeFilter);
                        }

                        return xeRoot;
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    string Title = AntdUI.Localization.Get("InjectModeForm.ImportFilterList.Success", "导入滤镜列表成功");
                                    AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Import))
                                    {
                                        string Title = AntdUI.Localization.Get("ImportFilterList", "导入滤镜列表");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,                                            
                                            OnOk = config =>
                                            {
                                                string sPW = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(sPW))
                                                {
                                                    eForm.EncryptionText_Changed();

                                                    AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                                    {
                                                        LocalizationText = "ImportList.Error"
                                                    });

                                                    return false;
                                                }
                                                else
                                                {
                                                    xdoc = SystemConfig.DecryptXMLFile(FilePath, sPW);                                                    
                                                    return true;
                                                }
                                            }
                                        });
                                    }
                                }                                
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("System.Import.Error", "导入失败: 密码错误");

                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                                }

                                return false;
                            }

                            LoadFilterList_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                            Guid gFID = Guid.NewGuid();

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

                            Guid gSID = Guid.Empty;
                            if (xeFilter.Element("SendID") != null)
                            {
                                gSID = Guid.Parse(xeFilter.Element("SendID").Value);
                            }
                            else
                            {
                                gSID = Guid.Empty;
                            }

                            Guid gRID = Guid.Empty;
                            if (xeFilter.Element("RobotID") != null)
                            {
                                gRID = Guid.Parse(xeFilter.Element("RobotID").Value);
                            }
                            else
                            {
                                gRID = Guid.Empty;
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
                                gFID,
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
                                gSID,
                                gRID,
                                FilterFunction,
                                FilterStartFrom,
                                IsProgressionDone,
                                bIsProgressionContinuous,
                                iFProgressionStep,
                                bIsProgressionCarry,
                                iFProgressionCarryNumber,
                                sFProgressionPosition,
                                iProgressionCount,
                                sFSearch,
                                sFModify);
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        string SName = string.Format(AntdUI.Localization.Get("NewSendName", "发送 {0}"), SNum.ToString());
                        bool SSystemSocket = false;
                        int SLoopCNT = 1;
                        int SLoopINT = 1000;
                        string SNotes = string.Empty;
                        BindingList<PacketInfo> SCollection = new BindingList<PacketInfo>();

                        Send.AddSend(IsEnable, SID, SName, SSystemSocket, SLoopCNT, SLoopINT, SCollection, SNotes);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miSendList", "发送列表"), "\r\n确定删除选中的数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                public static void DoSend_ByIndex(int SendListIndex)
                {
                    try
                    {
                        if (SendListIndex > -1 && SendListIndex < SendConfig.List.lstSendInfo.Count)
                        {
                            Guid SID = SendConfig.List.lstSendInfo[SendListIndex].SID;

                            Task.Run(() => DoSendAsync(SID))
                              .ConfigureAwait(false)
                              .GetAwaiter()
                              .GetResult();
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                if (si.SCollection.Count > 0)
                                {
                                    seReturn = new SendExecute();
                                    await Task.Run(() => seReturn.StartSend(si));
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

                public static void DoSend_ByHotKey(int HOTKEY_ID)
                {
                    switch (HOTKEY_ID)
                    {
                        case 9001:
                            Send.DoSend_ByIndex(0);
                            break;

                        case 9002:
                            Send.DoSend_ByIndex(1);
                            break;

                        case 9003:
                            Send.DoSend_ByIndex(2);
                            break;

                        case 9004:
                            Send.DoSend_ByIndex(3);
                            break;

                        case 9005:
                            Send.DoSend_ByIndex(4);
                            break;

                        case 9006:
                            Send.DoSend_ByIndex(5);
                            break;

                        case 9007:
                            Send.DoSend_ByIndex(6);
                            break;

                        case 9008:
                            Send.DoSend_ByIndex(7);
                            break;

                        case 9009:
                            Send.DoSend_ByIndex(8);
                            break;

                        case 9010:
                            Send.DoSend_ByIndex(9);
                            break;

                        case 9011:
                            Send.DoSend_ByIndex(10);
                            break;

                        case 9012:
                            Send.DoSend_ByIndex(11);
                            break;
                    }
                }

                #endregion

                #region//设置发送是否启用

                public static void SetIsCheck_BySendIndex(int SIndex, bool bCheck)
                {
                    try
                    {
                        if (SIndex > -1)
                        {
                            SendConfig.List.lstSendInfo[SIndex].IsEnable = bCheck;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miSendCollection", "发送集列表"), "\r\n确定删除所有数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            sfdSaveFile.Filter = AntdUI.Localization.Get("SendCollectionFile", "发送集文件") + "（*.sc）|*.sc";

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
                                    bool DoEncrypt = false;
                                    string Password = string.Empty;

                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Export))
                                    {
                                        string Title = AntdUI.Localization.Get("ExportSendCollection", "导出发送集");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                Password = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(Password))
                                                {
                                                    eForm.EncryptionText_Changed();

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

                                    if (SaveSendCollection(FilePath, SendCollection, DoEncrypt, Password))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportSendCollection.Success", "导出发送集成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportSendCollection.Error", "导出发送集失败");
                                        string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//加载发送集（对话框）

                public static void LoadSendCollection_Dialog(Form form, BindingList<PacketInfo> SendCollection)
                {
                    try
                    {
                        OpenFileDialog ofdLoadFile = new OpenFileDialog();
                        ofdLoadFile.Filter = AntdUI.Localization.Get("SendCollectionFile", "发送集文件") + "（*.sc）|*.sc";
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
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Import))
                                    {
                                        string Title = AntdUI.Localization.Get("ImportSendCollection", "导入发送集");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                string sPW = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(sPW))
                                                {
                                                    eForm.EncryptionText_Changed();

                                                    AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                                    {
                                                        LocalizationText = "ImportList.Error"
                                                    });

                                                    return false;
                                                }
                                                else
                                                {
                                                    xdoc = SystemConfig.DecryptXMLFile(FilePath, sPW);
                                                    return true;
                                                }
                                            }
                                        });
                                    }
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("System.Import.Error", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                                }

                                return false;
                            }

                            LoadSendCollection_FromXDocument(xdoc, SendCollection);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miSendList", "发送列表"), "\r\n确定删除所有数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    bool DoEncrypt = false;
                                    string Password = string.Empty;

                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Export))
                                    {
                                        string Title = AntdUI.Localization.Get("ExportSendList", "导出发送列表");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                Password = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(Password))
                                                {
                                                    eForm.EncryptionText_Changed();

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

                                    if (SaveSendList(FilePath, siList, DoEncrypt, Password))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportSendList.Success", "导出发送列表成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                }                    
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Import))
                                    {
                                        string Title = AntdUI.Localization.Get("ImportSendList", "导入发送列表");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                string sPW = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(sPW))
                                                {
                                                    eForm.EncryptionText_Changed();

                                                    AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                                    {
                                                        LocalizationText = "ImportList.Error"
                                                    });

                                                    return false;
                                                }
                                                else
                                                {
                                                    xdoc = SystemConfig.DecryptXMLFile(FilePath, sPW);
                                                    return true;
                                                }
                                            }
                                        });
                                    }
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("System.Import.Error", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                                }

                                return false;
                            }

                            LoadSendList_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                            Guid SID = Guid.NewGuid();

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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        string RName = string.Format(AntdUI.Localization.Get("System.RobotName", "机器人 {0}"), RNum.ToString());
                        BindingList<InstructionInfo> RInstruction = new BindingList<InstructionInfo>();

                        AddRobot(IsEnable, RID, RName, RInstruction);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//删除机器人

                public static void DeleteRobot_Dialog(Form form, List<RobotInfo> riList)
                {
                    try
                    {
                        if (riList.Count > 0)
                        {
                            AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miRobotList", "机器人列表"), "\r\n确定删除选中的数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//获取机器人

                public static RobotInfo GeRobot_ByGuid(Guid RID)
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return null;
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
                                sReturn = AntdUI.Localization.Get("System.Send", "发送");
                                break;

                            case Robot.InstructionType.SendPacketList:
                                sReturn = AntdUI.Localization.Get("System.Send", "发送");
                                break;

                            case Robot.InstructionType.SetSystemSocket:
                                sReturn = AntdUI.Localization.Get("System.Set", "设置");
                                break;

                            case Robot.InstructionType.Delay:
                                sReturn = AntdUI.Localization.Get("System.Delay", "延迟");
                                break;

                            case Robot.InstructionType.LoopStart:
                                sReturn = AntdUI.Localization.Get("System.LoopStart", "循环开始");
                                break;

                            case Robot.InstructionType.LoopEnd:
                                sReturn = AntdUI.Localization.Get("System.LoopEnd", "循环结束");
                                break;

                            case Robot.InstructionType.KeyBoard:
                                sReturn = AntdUI.Localization.Get("System.KeyBoard", "键盘");
                                break;

                            case Robot.InstructionType.Mouse:
                                sReturn = AntdUI.Localization.Get("System.Mouse", "鼠标");
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                                    sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.SendList", "发送列表 - [{0}]"), SName);
                                }

                                break;

                            case Robot.InstructionType.SendPacketList:

                                sReturn = AntdUI.Localization.Get("RobotEditForm.PacketList", "[封包列表] 选中的封包");

                                break;

                            case Robot.InstructionType.SetSystemSocket:

                                if (sContent.Equals("PacketConfig.List"))
                                {
                                    sReturn = AntdUI.Localization.Get("RobotEditForm.SelectPacket", "系统套接字 = 选中封包的套接字");
                                }
                                else if (sContent.Equals("FilterSocket"))
                                {
                                    sReturn = AntdUI.Localization.Get("RobotEditForm.SelectFilter", "系统套接字 = 调用滤镜的套接字");
                                }
                                else if (sContent.Contains("Customize") && sContent.Contains("|"))
                                {
                                    string sSocket = sContent.Split('|')[1];
                                    sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.SelectSocket", "系统套接字 = {0}"), sSocket);
                                }

                                break;

                            case Robot.InstructionType.Delay:

                                if (!string.IsNullOrEmpty(sContent))
                                {
                                    sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.Millisecond", "{0} 毫秒"), sContent);
                                }

                                break;

                            case Robot.InstructionType.LoopStart:

                                if (!string.IsNullOrEmpty(sContent))
                                {
                                    sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.LoopStart", "循环 {0} 次"), sContent);
                                }

                                break;

                            case Robot.InstructionType.LoopEnd:

                                sReturn = AntdUI.Localization.Get("RobotEditForm.LoopEnd", "循环结束");

                                break;

                            case Robot.InstructionType.KeyBoard:

                                if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                                {
                                    Robot.KeyBoardType kbType = Robot.GetKeyBoardType_ByString(sContent.Split('|')[0].ToString());
                                    string KeyCode = sContent.Split('|')[1];

                                    switch (kbType)
                                    {
                                        case Robot.KeyBoardType.Press:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.KeyPress", "按键 {0}"), KeyCode);
                                            break;

                                        case Robot.KeyBoardType.Down:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.KeyDown", "按下 {0}"), KeyCode);
                                            break;

                                        case Robot.KeyBoardType.Up:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.KeyUp", "弹起 {0}"), KeyCode);
                                            break;

                                        case Robot.KeyBoardType.Combine:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.KeyCombine", "组合按键 {0}"), KeyCode);
                                            break;

                                        case Robot.KeyBoardType.Text:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.KeyText", "文本 {0}"), KeyCode);
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
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.LeftClick", "左键单击");
                                            break;

                                        case Robot.MouseType.RightClick:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.RightClick", "右键单击");
                                            break;

                                        case Robot.MouseType.LeftDBClick:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.LeftDBClick", "左键双击");
                                            break;

                                        case Robot.MouseType.RightDBClick:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.RightDBClick", "右键双击");
                                            break;

                                        case Robot.MouseType.LeftDown:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.LeftDown", "左键按下");
                                            break;

                                        case Robot.MouseType.LeftUp:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.LeftUp", "左键弹起");
                                            break;

                                        case Robot.MouseType.RightDown:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.RightDown", "右键按下");
                                            break;

                                        case Robot.MouseType.RightUp:
                                            sReturn = AntdUI.Localization.Get("RobotEditForm.RightUp", "右键弹起");
                                            break;

                                        case Robot.MouseType.WheelUp:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.WheelUp", "向上滚动 {0}"), MouseCode);
                                            break;

                                        case Robot.MouseType.WheelDown:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.WheelDown", "向下滚动 {0}"), MouseCode);
                                            break;

                                        case Robot.MouseType.MoveTo:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.MoveTo", "移动到 ( {0} )"), MouseCode);
                                            break;

                                        case Robot.MouseType.MoveBy:
                                            sReturn = string.Format(AntdUI.Localization.Get("RobotEditForm.MoveBy", "相对移动 ( {0} )"), MouseCode);
                                            break;
                                    }
                                }

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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//获取指令集的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_RobotInstruction()
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                    menuItems.Add(new AntdUI.ContextMenuStripItem("置顶", "Ctrl+向上键")
                    {
                        ID = "Top",
                        IconSvg = "VerticalAlignTopOutlined",
                        LocalizationText = "InjectModeForm.cmsFilterList.Top",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("向上移动", "Alt+向上键")
                    {
                        ID = "Up",
                        IconSvg = "ArrowUpOutlined",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItem("向下移动", "Alt+向下键")
                    {
                        ID = "Down",
                        IconSvg = "ArrowDownOutlined",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("置底", "Ctrl+向下键")
                    {
                        ID = "Bottom",
                        IconSvg = "VerticalAlignBottomOutlined",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("删除")
                    {
                        ID = "Delete",
                        IconSvg = "CloseOutlined",
                    });
                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());
                    menuItems.Add(new AntdUI.ContextMenuStripItem("清空所有指令")
                    {
                        ID = "ClearUp",
                        IconSvg = "DeleteOutlined",
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
                                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miRobotInstruction", "指令集列表"), "\r\n确定删除所有数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }

                    return iReturn;
                }

                #endregion

                #region//执行机器人            

                public static RobotExecute DoRobot(Guid RID, Dictionary<string, object> parameters)
                {
                    return Task.Run(() => DoRobotAsync(RID, parameters)).GetAwaiter().GetResult();
                }

                private static void DoRobot_ByIndex(int RobotListIndex)
                {
                    try
                    {
                        if (RobotListIndex > -1 && RobotListIndex < RobotConfig.List.lstRobotInfo.Count)
                        {
                            Guid RID = RobotConfig.List.lstRobotInfo[RobotListIndex].RID;
                            Task.Run(() => DoRobotAsync(RID, null)).GetAwaiter().GetResult();
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                if (ri.RInstruction.Count > 0)
                                {
                                    reReturn = new RobotExecute();
                                    await Task.Run(() => reReturn.StartRobot(ri, parameters));
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

                public static void DoRobot_ByHotKey(int HOTKEY_ID)
                {
                    switch (HOTKEY_ID)
                    {
                        case 9001:
                            Robot.DoRobot_ByIndex(0);
                            break;

                        case 9002:
                            Robot.DoRobot_ByIndex(1);
                            break;

                        case 9003:
                            Robot.DoRobot_ByIndex(2);
                            break;

                        case 9004:
                            Robot.DoRobot_ByIndex(3);
                            break;

                        case 9005:
                            Robot.DoRobot_ByIndex(4);
                            break;

                        case 9006:
                            Robot.DoRobot_ByIndex(5);
                            break;

                        case 9007:
                            Robot.DoRobot_ByIndex(6);
                            break;

                        case 9008:
                            Robot.DoRobot_ByIndex(7);
                            break;

                        case 9009:
                            Robot.DoRobot_ByIndex(8);
                            break;

                        case 9010:
                            Robot.DoRobot_ByIndex(9);
                            break;

                        case 9011:
                            Robot.DoRobot_ByIndex(10);
                            break;

                        case 9012:
                            Robot.DoRobot_ByIndex(11);
                            break;
                    }
                }

                #endregion
            }

            #endregion

            #region//机器人列表

            public static class List
            {
                public static List<RobotExecute> lstRobotExecute = new List<RobotExecute>();
                public static BindingList<RobotInfo> lstRobotInfo = new BindingList<RobotInfo>();

                #region//机器人入列表

                public static void RobotToList(RobotInfo ri)
                {
                    try
                    {
                        RobotConfig.List.lstRobotInfo.Add(ri);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                #endregion

                #region//清空机器人列表（对话框）

                public static void CleanUpRobotList_Dialog(Form form)
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("InjectModeForm.miRobotList", "机器人列表"), "\r\n确定删除所有数据吗\r\n\r\n")
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    bool DoEncrypt = false;
                                    string Password = string.Empty;

                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Export))
                                    {
                                        string Title = AntdUI.Localization.Get("ExportRobotList", "导出机器人列表");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                Password = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(Password))
                                                {
                                                    eForm.EncryptionText_Changed();

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

                                    if (SaveRobotList(FilePath, riList, DoEncrypt, Password))
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportRobotList.Success", "导出机器人列表成功");
                                        AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                    }
                                    else
                                    {
                                        string Title = AntdUI.Localization.Get("InjectModeForm.ExportRobotList.Error", "导出机器人列表失败");
                                        string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                        AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                    using (EncryptionPassword eForm = new EncryptionPassword(SystemConfig.PWType.Import))
                                    {
                                        string Title = AntdUI.Localization.Get("ImportRobotList", "导入机器人列表");
                                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, eForm, TType.Info)
                                        {
                                            Keyboard = false,
                                            MaskClosable = false,
                                            OnOk = config =>
                                            {
                                                string sPW = eForm.GetPassword();
                                                if (string.IsNullOrEmpty(sPW))
                                                {
                                                    eForm.EncryptionText_Changed();

                                                    AntdUI.Message.open(new AntdUI.Message.Config(form, "密码不能为空", TType.Error)
                                                    {
                                                        LocalizationText = "ImportList.Error"
                                                    });

                                                    return false;
                                                }
                                                else
                                                {
                                                    xdoc = SystemConfig.DecryptXMLFile(FilePath, sPW);
                                                    return true;
                                                }
                                            }
                                        });
                                    }
                                }
                            }
                            else
                            {
                                xdoc = XDocument.Load(FilePath);
                            }

                            if (xdoc == null)
                            {
                                string sError = AntdUI.Localization.Get("System.Import.Error", "导入失败: 密码错误");
                                if (LoadFromUser)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(form, sError, TType.Error));
                                }
                                else
                                {
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sError);
                                }

                                return false;
                            }

                            LoadRobotList_FromXDocument(xdoc);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                            Guid RID = Guid.NewGuid();

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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            #region//日志队列

            public static class Queue
            {
                public static ConcurrentQueue<LogInfo> cqLogInfo = new ConcurrentQueue<LogInfo>();

                #region//日志入队列

                public static void LogToQueue(string sFuncName, string sLogContent)
                {
                    LogInfo li = new LogInfo(sFuncName, sLogContent);
                    cqLogInfo.Enqueue(li);
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

                #endregion                
            }

            #endregion

            #region//日志列表

            public static class List
            {
                public static bool AutoRoll = false, AutoClear = true;
                public static decimal AutoClear_Value = 5000;
                public static BindingList<LogInfo> lstLogInfo = new BindingList<LogInfo>();

                #region//日志入列表

                public static void LogToList()
                {
                    if (Queue.cqLogInfo.TryDequeue(out LogInfo li))
                    {
                        LogConfig.List.lstLogInfo.Add(li);
                    }
                }

                #endregion

                #region//清除日志列表

                public static void ClearLogList()
                {
                    lstLogInfo.Clear();
                }

                #endregion

                #region//获取日志列表的右键菜单

                public static AntdUI.IContextMenuStripItem[] GetCMS_LogList()
                {
                    List<AntdUI.IContextMenuStripItem> menuItems = new List<AntdUI.IContextMenuStripItem>();

                    menuItems.Add(new AntdUI.ContextMenuStripItem("复制日志信息")
                    {
                        ID = "Copy",
                        IconSvg = "CopyOutlined",
                        LocalizationText = "InjectModeForm.CopyLog",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("导出到Excel")
                    {
                        ID = "ToExcel",
                        IconSvg = "FileExcelOutlined",
                        LocalizationText = "InjectModeForm.ToExcel",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItemDivider());

                    menuItems.Add(new AntdUI.ContextMenuStripItem("清空日志列表")
                    {
                        ID = "ClearUp",
                        IconSvg = "DeleteOutlined",
                        LocalizationText = "InjectModeForm.ClearUp",
                    });

                    menuItems.Add(new AntdUI.ContextMenuStripItem("取消选择")
                    {
                        ID = "DeSelect",
                        IconSvg = "DeleteRowOutlined",
                        LocalizationText = "InjectModeForm.DeSelect",
                    });

                    return menuItems.ToArray();
                }

                #endregion

                #region//保存日志列表为Excel（对话框）

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
                                            string Title = AntdUI.Localization.Get("InjectModeForm.ExportToExcel.Success", "导出到Excel成功");
                                            AntdUI.Notification.success(form, Title, FilePath, AntdUI.TAlignFrom.TR);
                                            Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                                        }
                                        else
                                        {
                                            string Title = AntdUI.Localization.Get("InjectModeForm.ExportToExcel.Error", "导出到Excel失败");
                                            string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                                            AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                                        }
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }

                private static bool SaveLogListToExcel(string filePath, List<LogInfo> liList)
                {
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        using (var writer = new StreamWriter(stream, Encoding.Default))
                        {
                            writer.WriteLine(AntdUI.Localization.Get("ToExcelTitle", "记录时间\t模块\t日志内容\t"));

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
                                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        return false;
                    }
                }

                #endregion                
            }

            #endregion
        }

        #endregion

        #region//记录日志        

        public static void DoLog(string sFuncName, string sLogContent)
        {
            Task.Run(() =>
            {
                LogConfig.Queue.LogToQueue(sFuncName, sLogContent);
            });
        }

        #endregion

        #region//数据库配置

        public static class DataBase
        {
            private static string dbPath = @"C:\WPE64Cache";
            private static string dbName = SystemConfig.AssemblyVersion + ".db";
            private static string conStr = string.Format("Data Source={0}\\{1};Version=3;", dbPath, dbName);

            #region//初始化

            public static void InitDB()
            {
                DataBase.InitdbPath();

                DataBase.CreateTable_SystemConfig();
                DataBase.CreateTable_InjectMode();
                DataBase.CreateTable_ProxyMode();
                DataBase.CreateTable_Filter();
                DataBase.CreateTable_Send();
                DataBase.CreateTable_Robot();
                DataBase.CreateTable_ProxyAccount();
                DataBase.CreateTable_ProxyMapLocal();
                DataBase.CreateTable_ProxyMapRemote();
            }

            private static void InitdbPath()
            {
                try
                {
                    if (!Directory.Exists(dbPath))
                    {
                        Directory.CreateDirectory(dbPath);
                    }
                }
                catch (Exception ex)
                {
                    DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        sql += "StartMode INTEGER DEFAULT 0,";//系统设置 - 启动模式
                        sql += "Remote_IsEnable BOOLEAN DEFAULT 0,";//系统设置 - 启用远程管理
                        sql += "Remote_UserName TEXT,";//系统设置 - 远程管理账号
                        sql += "Remote_PassWord TEXT,";//系统设置 - 远程管理密码
                        sql += "Remote_Port INTEGER,";//系统设置 - 远程管理端口                    
                        sql += "Remote_URL TEXT,";//系统设置 - 远程管理网址
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
                        sql += "HotKey12 TEXT";//快捷键12
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        sql += "StartMode,";
                        sql += "Remote_IsEnable,";
                        sql += "Remote_UserName,";
                        sql += "Remote_PassWord,";
                        sql += "Remote_Port,";
                        sql += "Remote_URL,";
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
                        sql += "HotKey12";
                        sql += ") VALUES (";
                        sql += "@IsAnimation,";
                        sql += "@IsShadowEnabled,";
                        sql += "@IsShowInWindow,";
                        sql += "@IsScrollBarHide,";
                        sql += "@IsTextRenderingHighQuality,";
                        sql += "@IsDark,";
                        sql += "@DefaultLanguage,";
                        sql += "@LastInjection,";
                        sql += "@StartMode,";
                        sql += "@Remote_IsEnable,";
                        sql += "@Remote_UserName,";
                        sql += "@Remote_PassWord,";
                        sql += "@Remote_Port,";
                        sql += "@Remote_URL,";
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
                        sql += "@HotKey12";
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
                            cmd.Parameters.AddWithValue("@StartMode", SystemConfig.StartMode);
                            cmd.Parameters.AddWithValue("@Remote_IsEnable", SystemConfig.IsRemote);
                            cmd.Parameters.AddWithValue("@Remote_UserName", SystemConfig.Remote_UserName);
                            cmd.Parameters.AddWithValue("@Remote_PassWord", SystemConfig.Remote_PassWord);
                            cmd.Parameters.AddWithValue("@Remote_Port", SystemConfig.Remote_Port);
                            cmd.Parameters.AddWithValue("@Remote_URL", SystemConfig.Remote_URL);
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

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        sql += "PacketList_AutoClear_Value INTEGER DEFAULT 5000,";//封包列表自动清理数值                        
                        sql += "SpeedMode BOOLEAN DEFAULT 0";//极速模式                        
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        sql += "PacketList_AutoClear_Value,";                        
                        sql += "SpeedMode";                        
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
                        sql += "@PacketList_AutoClear_Value,";                        
                        sql += "@SpeedMode";                        
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
                            cmd.Parameters.AddWithValue("@SpeedMode", PacketConfig.Packet.SpeedMode);                            

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        sql += "ProxyPort INTEGER DEFAULT 1080,";//代理模式 - 代理端口
                        sql += "EnableAuth BOOLEAN DEFAULT 1,";//代理模式 - 启用代理认证                    
                        sql += "ProxyList_AutoRoll BOOLEAN DEFAULT 0,";//代理模式 - 代理列表自动滚动
                        sql += "ProxyList_AutoClear BOOLEAN DEFAULT 1,";//代理模式 - 代理列表自动清理
                        sql += "ProxyList_AutoClear_Value INTEGER DEFAULT 5000,";//代理模式 - 代理列表自动清理数值                        
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
                        sql += "SpeedMode BOOLEAN DEFAULT 0";//代理模式 - 极速模式
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        sql += "ProxyPort,";
                        sql += "EnableAuth,";                  
                        sql += "ProxyList_AutoRoll,";
                        sql += "ProxyList_AutoClear,";
                        sql += "ProxyList_AutoClear_Value,";
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
                        sql += "SpeedMode";
                        sql += ") VALUES (";
                        sql += "@ProxyIP_Auto,";
                        sql += "@EnableSOCKS5,";
                        sql += "@ProxyPort,";
                        sql += "@EnableAuth,";                 
                        sql += "@ProxyList_AutoRoll,";
                        sql += "@ProxyList_AutoClear,";
                        sql += "@ProxyList_AutoClear_Value,";
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
                        sql += "@SpeedMode";
                        sql += ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ProxyIP_Auto", ProxyConfig.Proxy.ProxyIP_Auto);
                            cmd.Parameters.AddWithValue("@EnableSOCKS5", ProxyConfig.Proxy.Enable_SOCKS5);
                            cmd.Parameters.AddWithValue("@ProxyPort", ProxyConfig.Proxy.ProxyPort);
                            cmd.Parameters.AddWithValue("@EnableAuth", ProxyConfig.Proxy.Enable_Auth);                        
                            cmd.Parameters.AddWithValue("@ProxyList_AutoRoll", ProxyConfig.List.AutoRoll);
                            cmd.Parameters.AddWithValue("@ProxyList_AutoClear", ProxyConfig.List.AutoClear);
                            cmd.Parameters.AddWithValue("@ProxyList_AutoClear_Value", ProxyConfig.List.AutoClear_Value);                            
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
                            cmd.Parameters.AddWithValue("@SpeedMode", ProxyConfig.Proxy.SpeedMode);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        sql += "Send_GUID TEXT NOT NULL,";
                        sql += "Robot_GUID TEXT NOT NULL,";
                        sql += "Function TEXT NOT NULL,";
                        sql += "StartFrom INTEGER DEFAULT 0,";
                        sql += "IsProgressionContinuous BOOLEAN DEFAULT 0,";
                        sql += "ProgressionStep INTEGER DEFAULT 1,";
                        sql += "IsProgressionCarry BOOLEAN DEFAULT 0,";
                        sql += "ProgressionCarryNumber INTEGER DEFAULT 1,";
                        sql += "ProgressionPosition TEXT,";
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        sql += "Send_GUID,";
                        sql += "Robot_GUID,";
                        sql += "Function,";
                        sql += "StartFrom,";
                        sql += "IsProgressionContinuous,";
                        sql += "ProgressionStep,";
                        sql += "IsProgressionCarry,";
                        sql += "ProgressionCarryNumber,";
                        sql += "ProgressionPosition,";
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
                        sql += "@Send_GUID,";
                        sql += "@Robot_GUID,";
                        sql += "@Function,";
                        sql += "@StartFrom,";
                        sql += "@IsProgressionContinuous,";
                        sql += "@ProgressionStep,";
                        sql += "@IsProgressionCarry,";
                        sql += "@ProgressionCarryNumber,";
                        sql += "@ProgressionPosition,";
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
                            cmd.Parameters.AddWithValue("@Send_GUID", sfi.SID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@Robot_GUID", sfi.RID.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@Function", FilterConfig.Filter.GetFilterFunctionString(sfi.FFunction));
                            cmd.Parameters.AddWithValue("@StartFrom", sfi.FStartFrom);
                            cmd.Parameters.AddWithValue("@IsProgressionContinuous", sfi.IsProgressionContinuous);
                            cmd.Parameters.AddWithValue("@ProgressionStep", sfi.ProgressionStep);
                            cmd.Parameters.AddWithValue("@IsProgressionCarry", sfi.IsProgressionCarry);
                            cmd.Parameters.AddWithValue("@ProgressionCarryNumber", sfi.ProgressionCarryNumber);
                            cmd.Parameters.AddWithValue("@ProgressionPosition", sfi.ProgressionPosition);
                            cmd.Parameters.AddWithValue("@Search", sfi.FSearch);
                            cmd.Parameters.AddWithValue("@Modify", sfi.FModify);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return dtReturn;
            }

            public static void DeleteTable_ProxyAccount()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(conStr))
                    {
                        string sql = "DELETE FROM ProxyAccount;";
                        sql += "DELETE FROM ProxyAccountIPInfo;";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            public static void InsertTable_ProxyAccount()
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DataBase.conStr))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            // 1. 插入主表 ProxyAccount
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

                            // 2. 插入子表 ProxyAccountIPInfo
                            string sqlIPInfo = @"
                                INSERT INTO ProxyAccountIPInfo (
                                    GUID, LoginTime, LoginIP
                                ) VALUES (
                                    @GUID, @LoginTime, @LoginIP
                                );";

                            using (SQLiteCommand cmdAccount = new SQLiteCommand(sqlAccount, conn, transaction))
                            using (SQLiteCommand cmdIPInfo = new SQLiteCommand(sqlIPInfo, conn, transaction))
                            {
                                // 设置主表参数
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

                                // 设置子表参数
                                cmdIPInfo.Parameters.Add(new SQLiteParameter("@GUID", DbType.String));
                                cmdIPInfo.Parameters.Add(new SQLiteParameter("@LoginTime", DbType.DateTime));
                                cmdIPInfo.Parameters.Add(new SQLiteParameter("@LoginIP", DbType.String));

                                foreach (AccountInfo pai in ProxyConfig.Account.lstAccountInfo)
                                {
                                    // 插入主表数据
                                    cmdAccount.Parameters["@GUID"].Value = pai.AID.ToString().ToUpper();
                                    cmdAccount.Parameters["@IsEnable"].Value = pai.IsEnable;
                                    cmdAccount.Parameters["@UserName"].Value = pai.UserName;
                                    cmdAccount.Parameters["@PassWord"].Value = pai.Password;
                                    cmdAccount.Parameters["@IsLimitLinks"].Value = pai.IsLimitLinks;
                                    cmdAccount.Parameters["@LimitLinks"].Value = pai.LimitLinks;
                                    cmdAccount.Parameters["@IsLimitDevices"].Value = pai.IsLimitDevices;
                                    cmdAccount.Parameters["@LimitDevices"].Value = pai.LimitDevices;
                                    cmdAccount.Parameters["@IsExpiry"].Value = pai.IsExpiry;
                                    cmdAccount.Parameters["@ExpiryTime"].Value = pai.ExpiryTime;
                                    cmdAccount.Parameters["@CreateTime"].Value = pai.CreateTime;

                                    cmdAccount.ExecuteNonQuery();

                                    // 插入子表数据（AIPInfo）
                                    if (pai.AIPInfo != null)
                                    {
                                        foreach (AccountIPInfo ipInfo in pai.AIPInfo)
                                        {
                                            cmdIPInfo.Parameters["@GUID"].Value = pai.AID.ToString().ToUpper();
                                            cmdIPInfo.Parameters["@LoginTime"].Value = ipInfo.LoginTime;
                                            cmdIPInfo.Parameters["@LoginIP"].Value = ipInfo.LoginIP;

                                            cmdIPInfo.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

            #endregion
        }

        #endregion
    }
}
