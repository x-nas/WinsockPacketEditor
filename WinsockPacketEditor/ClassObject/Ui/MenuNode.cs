namespace WinsockPacketEditor
{
    #region//右键菜单节点

    /// <summary>
    /// 右键菜单节点。替代 AntdUI 的
    /// IContextMenuStripItem / ContextMenuStripItem / ContextMenuStripItemDivider。
    ///
    /// 字段与 AntdUI 一一对应（Operate 里的菜单本来就已经是「Id + 本地化 key + 图标名」的数据形态），
    /// 转换见 Forms/UiHost/UiAdapters.cs 的 MenuAdapter。
    /// <b>回调分发仍然靠 Id 字符串 switch，与现在完全一致，不需要改动任何菜单事件处理器。</b>
    ///
    /// B0 阶段仅定义，B5 批次把 12 个 GetCMS_* 方法的返回类型换成 MenuNode[]。
    /// </summary>
    public sealed class MenuNode
    {
        /// <summary>分隔符的保留 Id。</summary>
        public const string DividerId = "-";

        /// <summary>原 AntdUI 的 ID，菜单点击回调靠它分发。</summary>
        public string Id;

        /// <summary>原 AntdUI 的 LocalizationText（本地化 key）。</summary>
        public string TextKey;

        /// <summary>原构造函数里的中文默认文案。</summary>
        public string TextFallback;

        /// <summary>原 AntdUI 的 IconSvg（图标名，如 "CopyOutlined"）。</summary>
        public string IconSvg;

        /// <summary>
        /// 原 AntdUI 双参构造的第二个参数（SubText），右侧灰字。
        /// 项目里只用来显示快捷键提示（Ctrl+C / Alt+⬆ 之类），不需要本地化。
        /// </summary>
        public string SubText;

        public bool Enabled = true;

        public bool Checked;

        /// <summary>原 AntdUI 的 Tag，用于携带业务对象（现有代码有 2 处在用）。</summary>
        public object Tag;

        public MenuNode[] Sub;

        public bool IsDivider
        {
            get { return this.Id == DividerId; }
        }

        public static MenuNode Divider()
        {
            return new MenuNode { Id = DividerId };
        }

        public static MenuNode Item(
            string Id,
            string TextFallback,
            string TextKey = null,
            string IconSvg = null,
            bool Enabled = true,
            object Tag = null,
            string SubText = null,
            MenuNode[] Sub = null)
        {
            return new MenuNode
            {
                Id = Id,
                TextFallback = TextFallback,
                TextKey = TextKey,
                IconSvg = IconSvg,
                Enabled = Enabled,
                Tag = Tag,
                SubText = SubText,
                Sub = Sub,
            };
        }
    }

    #endregion
}
