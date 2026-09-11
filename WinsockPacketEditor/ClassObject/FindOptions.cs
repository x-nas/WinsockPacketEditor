namespace WinsockPacketEditor
{
    /// <summary>查找封包时按什么匹配。</summary>
    public enum FindType
    {
        /// <summary>把封包按 UTF-8 解成文本，再按正则匹配。</summary>
        Text = 0,

        /// <summary>把封包转成不带分隔符的十六进制串，再按正则匹配（只认落在字节边界上的命中）。</summary>
        Hex = 1,
    }

    /// <summary>
    /// 查找封包的条件与最近一次命中的内容（<c>PacketConfig.List.SearchForList</c> 填、调用方读）。
    ///
    /// 【从哪儿来】原先直接用 Be.Windows.Forms.HexBox 的同名类型 —— WinForms 那边要把它
    /// 原样喂给十六进制控件的 <c>Find()</c>。那个控件随 WinForms 界面删掉之后，
    /// 这里只剩「记下查什么、命中了什么」这件事，为四个字段带一个 WinForms 控件库不划算。
    /// 成员名与原来逐字相同，Operate 里的调用点一行不用改。
    /// </summary>
    public sealed class FindOptions
    {
        /// <summary>按文本还是按十六进制匹配。</summary>
        public FindType Type { get; set; }

        /// <summary>十六进制模式下命中的那一段字节（没命中是 null）。</summary>
        public byte[] Hex { get; set; }

        /// <summary>文本模式下命中的那一段文本。</summary>
        public string Text { get; set; }

        /// <summary>条件是否已经设好。</summary>
        public bool IsValid { get; set; }
    }
}
