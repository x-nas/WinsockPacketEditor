namespace WinsockPacketEditor
{
    #region//颜色值

    /// <summary>
    /// 颜色值（<b>配置用，不参与绘制</b>）。
    /// 替代 Operate 里被当作配置存储的 System.Drawing.Color（主题色板、8 个滤镜配色等）。
    ///
    /// 数据库与 XML 备份里存的一直是 ARGB int，本类型 <b>不改变任何存储格式</b>。
    /// 与 System.Drawing.Color 的互转见 Forms/UiHost/UiAdapters.cs 的 ColorAdapter。
    /// </summary>
    public struct RgbColor
    {
        public readonly int Argb;

        public RgbColor(int Argb)
        {
            this.Argb = Argb;
        }

        public static RgbColor FromRgb(int R, int G, int B)
        {
            return new RgbColor(unchecked((int)0xFF000000) | ((R & 0xFF) << 16) | ((G & 0xFF) << 8) | (B & 0xFF));
        }

        public int A { get { return (this.Argb >> 24) & 0xFF; } }
        public int R { get { return (this.Argb >> 16) & 0xFF; } }
        public int G { get { return (this.Argb >> 8) & 0xFF; } }
        public int B { get { return this.Argb & 0xFF; } }

        /// <summary>"#RRGGBB"，给前端用。</summary>
        public string Hex { get { return "#" + (this.Argb & 0xFFFFFF).ToString("X6"); } }

        public override string ToString()
        {
            return this.Hex;
        }
    }

    #endregion
}
