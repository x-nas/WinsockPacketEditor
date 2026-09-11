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

        //表已搬到 ClassObject/CountryCodes.cs（外壳也要它，而它是纯数据）
        private static IReadOnlyDictionary<string, string> CountryNameToCode => CountryCodes.Table;

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
