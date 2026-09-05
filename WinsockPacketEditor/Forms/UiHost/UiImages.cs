using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WinsockPacketEditor
{
    #region//界面图像

    /// <summary>
    /// 界面用到的图像（B3c 批次从 Operate.cs 搬出）。
    ///
    /// 【为什么搬】
    /// 这三块都是「数据 → System.Drawing 图像」的纯展示转换，与业务逻辑无关，
    /// 却让 Operate 依赖 System.Drawing、Properties.Resources 和 IconExtractor：
    ///   · IP 所属地 → 国旗图（含中文国名到国家代码的对照表与 PNG 缓存）
    ///   · 进程路径 → 进程图标
    ///   · 封包类型 → 收发方向图标
    /// 搬出后 Operate 只出数据（所属地字符串 / 进程路径 / 封包类型枚举），由本类负责画。
    ///
    /// 本类属于 UI 层，允许自由使用 System.Drawing 与 Properties.Resources。
    /// </summary>
    public static class UiImages
    {
        #region//IP 所属地的国旗图标

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

        /// <summary>
        /// 中文国名 → 国家代码的对照表（只读视图）。
        ///
        /// 【为什么要暴露】WebView2 外壳里国旗是 &lt;img&gt; 标签，前端要自己按归属地字符串
        /// 查代码。表放两份必然会漂，所以这里出一份只读视图，由桥一次性交给前端。
        ///
        /// <b>不要在这里加「按归属地查代码」的单条方法给桥逐行调用</b> ——
        /// 匹配是 207 条的前缀线性扫描，放进每秒几千行的 DTO 转换里就是每秒上百万次比较。
        /// 前端只在渲染可见行时才查，并且按归属地字符串做了记忆化。
        /// </summary>
        public static IReadOnlyDictionary<string, string> CountryTable
        {
            get { return CountryNameToCode; }
        }

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
                Operate.DoLog(nameof(GetFlagByLocation), ex);
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

        #region//进程图标

        /// <summary>
        /// 按可执行文件路径取进程图标。取不到一律回落系统默认图标，与搬迁前一致。
        /// 搬迁前收的是 Process 对象、内部调 GetFilePath(process)；
        /// 现在收 ProcessInfo.ProcessPath（同样取自 MainModule.FileName），在这里做相同的 .ni.dll 替换。
        /// </summary>
        public static Image GetProcessIcon(string ProcessPath)
        {
            string filePath = string.IsNullOrEmpty(ProcessPath) ? null : ProcessPath.Replace(".ni.dll", ".dll");

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
            catch
            {
                //
            }

            return new Icon(SystemIcons.Application, 256, 256).ToBitmap();
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

        #region//进程列表的图标填充

        /// <summary>
        /// 给 Operate.ProcessConfig.GetProcessList() 返回的列表补上图标。
        /// Operate 只负责列出进程与路径，图标由 UI 层按需生成。
        /// </summary>
        public static List<ProcessInfo> FillIcons(List<ProcessInfo> List)
        {
            if (List == null)
            {
                return null;
            }

            foreach (ProcessInfo pi in List)
            {
                pi.ICO = GetProcessIcon(pi.ProcessPath);
            }

            return List;
        }

        #endregion

        #region//封包类型图标

        public static Bitmap GetImg_ByPacketType(Operate.PacketConfig.Packet.PacketType ptType)
        {
            try
            {                        
                switch (ptType)
                {
                    case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                    case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                    case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                    case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                    case Operate.PacketConfig.Packet.PacketType.WSASend:
                    case Operate.PacketConfig.Packet.PacketType.WSASendTo:
                    case Operate.PacketConfig.Packet.PacketType.TCP_Req:
                    case Operate.PacketConfig.Packet.PacketType.UDP_Req:
                    case Operate.PacketConfig.Packet.PacketType.HTTP_Req:
                    case Operate.PacketConfig.Packet.PacketType.HTTPS_Req:
                    case Operate.PacketConfig.Packet.PacketType.WebSocket_Req:
                        return Properties.Resources.Send;

                    case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                    case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                    case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                    case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                    case Operate.PacketConfig.Packet.PacketType.WSARecv:
                    case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                    case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:
                    case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                    case Operate.PacketConfig.Packet.PacketType.UDP_Resp:
                    case Operate.PacketConfig.Packet.PacketType.HTTP_Resp:
                    case Operate.PacketConfig.Packet.PacketType.HTTPS_Resp:
                    case Operate.PacketConfig.Packet.PacketType.WebSocket_Resp:
                        return Properties.Resources.Recv;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(GetImg_ByPacketType), ex);
            }

            return null;
        }

        #endregion
    }

    #endregion
}
