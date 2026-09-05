using System.Collections.Generic;
using System.Drawing;

namespace WinsockPacketEditor
{
    #region//菜单适配器

    /// <summary>
    /// MenuNode 与 AntdUI 菜单项的相互转换。
    ///
    /// B5 批次把 12 个 GetCMS_* 方法的返回类型从 AntdUI.IContextMenuStripItem[]
    /// 换成 MenuNode[] 之后，29 个调用点只需在末尾补一个 .ToAntd()：
    ///
    ///     ContextMenuStrip.open(this, it => { ... }, Operate.PacketConfig.List.GetCMS_PacketList().ToAntd());
    ///
    /// 菜单点击回调仍然按 ID 字符串分发，不需要改动任何事件处理器。
    ///
    /// B0 阶段本类未被引用，属于提前就位的适配层。
    /// </summary>
    public static class MenuAdapter
    {
        public static AntdUI.IContextMenuStripItem[] ToAntd(this MenuNode[] Nodes)
        {
            if (Nodes == null || Nodes.Length == 0)
            {
                return new AntdUI.IContextMenuStripItem[0];
            }

            List<AntdUI.IContextMenuStripItem> lstItem = new List<AntdUI.IContextMenuStripItem>(Nodes.Length);

            foreach (MenuNode Node in Nodes)
            {
                if (Node == null)
                {
                    continue;
                }

                if (Node.IsDivider)
                {
                    lstItem.Add(new AntdUI.ContextMenuStripItemDivider());
                    continue;
                }

                //有副文本的走双参构造（右侧灰字的快捷键提示），与搬迁前一致
                AntdUI.ContextMenuStripItem Item = string.IsNullOrEmpty(Node.SubText)
                    ? new AntdUI.ContextMenuStripItem(Node.TextFallback ?? string.Empty)
                    : new AntdUI.ContextMenuStripItem(Node.TextFallback ?? string.Empty, Node.SubText);

                Item.ID = Node.Id;
                Item.Enabled = Node.Enabled;
                Item.Checked = Node.Checked;

                if (!string.IsNullOrEmpty(Node.IconSvg))
                {
                    Item.IconSvg = Node.IconSvg;
                }

                if (!string.IsNullOrEmpty(Node.TextKey))
                {
                    Item.LocalizationText = Node.TextKey;
                }

                if (Node.Tag != null)
                {
                    Item.Tag = Node.Tag;
                }

                if (Node.Sub != null && Node.Sub.Length > 0)
                {
                    Item.Sub = Node.Sub.ToAntd();
                }

                lstItem.Add(Item);
            }

            return lstItem.ToArray();
        }
    }

    #endregion

    #region//十六进制编辑器适配器

    /// <summary>
    /// HexBox 到 HexState 的转换。
    ///
    /// 四个菜单工厂（GetCMS_XOR / GetCMS_PacketData / GetCMS_PacketEdit / GetCMS_StoresData）
    /// 原先直接收 Be.Windows.Forms.HexBox，但只读它的四个可用性判断；
    /// 改收 HexState 之后 Operate 就不再认识 HexBox 这个控件。
    /// </summary>
    public static class HexAdapter
    {
        public static HexState ToState(this Be.Windows.Forms.HexBox Box)
        {
            if (Box == null)
            {
                return new HexState();
            }

            return new HexState
            {
                CanCopy = Box.CanCopy(),
                CanCut = Box.CanCut(),
                CanPaste = Box.CanPaste(),
                CanPasteHex = Box.CanPasteHex(),
            };
        }
    }

    #endregion

    #region//颜色适配器

    /// <summary>
    /// RgbColor 与 System.Drawing.Color 的互转。
    ///
    /// 刻意放在 UI 层：RgbColor 本身不引用 System.Drawing，
    /// 这样 B3 批次把 Operate 里的配置色改成 RgbColor 之后，
    /// Operate 就不再需要 using System.Drawing。
    /// </summary>
    public static class ColorAdapter
    {
        public static Color ToColor(this RgbColor Value)
        {
            return Color.FromArgb(Value.Argb);
        }

        public static RgbColor ToRgb(this Color Value)
        {
            return new RgbColor(Value.ToArgb());
        }
    }

    #endregion
}
