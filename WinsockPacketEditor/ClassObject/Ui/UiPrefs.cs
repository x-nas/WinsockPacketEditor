namespace WinsockPacketEditor
{
    #region//界面偏好

    /// <summary>
    /// 界面偏好。替代把 AntdUI.Config 当配置存储的做法（Operate 里 31 处）。
    ///
    /// 字段名与数据库列名、XML 备份的元素名一一对应，
    /// B3a 迁移 <b>不改任何列名、不改任何存储格式</b>：
    ///   SQLite  ：IsAnimation / IsShadowEnabled / IsShowInWindow /
    ///             IsScrollBarHide / IsTextRenderingHighQuality / IsDark / DefaultLanguage
    ///   XML 备份：同名元素
    ///
    /// <b>本类是这些值的唯一真源</b>：AntdUI.Config 与 AntdUI.Localization 是它的运行时镜像。
    ///   Prefs → 运行时：WinFormsUiHost.ApplyPrefs() / ApplyLanguage() / ApplyAll()
    ///   界面上改了设置：先写本类，再调上面的 Apply*，不要直接写 AntdUI.Config
    ///
    /// 纯 POCO，任何时候都可用，不依赖是否已 UI.Attach
    /// （注入模式在建窗之前就要读配置，所以这一点是必需的）。
    /// </summary>
    public sealed class UiPrefs
    {
        /// <summary>
        /// 当前<b>实际</b>用的是不是深色。
        ///
        /// 跟随系统时这里存的是「那一刻系统给出来的值」，不是用户选的三态之一 ——
        /// WinForms 那半边（AntdUI 只认深浅两态）读的就是它，得始终是个能直接用的值。
        /// 用户到底选了哪一档由 <see cref="FollowSystemTheme"/> 补充说明。
        ///
        /// ⚠️ <b>默认 true（深色）。</b>整套皮肤是照深色设计的，浅色那份是后加的令牌覆盖。
        /// 这个初值决定<b>第一次启动</b>（库还是空的、没有任何配置行）看到的是哪种 ——
        /// 早先没写初值，bool 默认 false，于是新装的机器第一眼是浅色。
        /// 改这里要连 <c>LoadSystemConfig_FromDB</c> 里「空库」那条分支一起改。
        /// </summary>
        public bool IsDark = true;

        /// <summary>
        /// 主题是不是「跟随系统」。
        ///
        /// 这是 <see cref="IsDark"/> 之外多出来的一个比特：两个字段合起来才是三态
        /// （false + false = 浅色、false + true = 深色、true + 任意 = 跟随系统）。
        /// 单开一个枚举也行，但那样 WinForms 那边每个读 IsDark 的地方都要改。
        ///
        /// <b>只有 WebView2 外壳认这一档</b>：AntdUI 没有「跟随系统」的概念，
        /// 所以 WinForms 侧照旧只看 IsDark。
        /// </summary>
        public bool FollowSystemTheme;
        /// <summary>
        /// 氛围层那条<b>缓慢上下游走的亮带</b>（`.scan`，10 秒一趟）开不开。
        ///
        /// ⚠️ <b>只有 WebView2 外壳用得上</b> —— WinForms 那套界面根本没有这一层。
        /// 放在这里是因为「界面偏好的唯一真源」就是 UiPrefs：
        /// 它要落 SystemConfig 表、要进备份、切库时要跟着换，
        /// 这几件事在别处再造一份只会多出一个要同步的地方。
        ///
        /// 默认<b>开</b> —— 它是这套皮肤的一部分，关掉是用户的选择而不是出厂状态。
        /// </summary>
        public bool ScanLine = true;

        public bool IsAnimation;
        public bool IsShadowEnabled;
        public bool IsShowInWindow = true;
        public bool IsScrollBarHide;
        public bool IsTextRenderingHighQuality;

        /// <summary>界面语言，存 SQLite 的 DefaultLanguage 列。取值形如 "zh-CN" / "en-US"。</summary>
        public string Language = "zh-CN";

        #region//可配置的界面颜色

        // 存储格式与迁移前完全一致：SQLite 与 XML 里存的都是 ARGB int
        //（原来是 Color.ToArgb() / Color.FromArgb(int)，现在是 RgbColor.Argb / new RgbColor(int)）。
        // 默认值取自搬迁前 Operate 里的字段初始值，括号里是原来的 KnownColor 名。
        // 只有这 9 个颜色是用户可改的；主题灰阶与滤镜标记色是常量，放在 UI 层的 UiTheme。

        /// <summary>主题色（22,119,255）。</summary>
        public RgbColor SystemColor = RgbColor.FromRgb(22, 119, 255);

        /*
            ── 滤镜命中行的四组配色 ────────────────────────────────

            老默认是给<b>浅色 WinForms 表格</b>配的：Goldenrod / DodgerBlue /
            LightGreen 这类满饱和的实心底 + 黑字。铺在深色表上就是四条发光的横带，
            亮到把它压着的那一行内容本身都盖过去了（对表格底色的对比度到 8.8）。

            现在改成<b>暗调底 + 同色相亮字</b>：底色只带出色相、不抢亮度，
            字色跟着底色走。四个色相沿用全项目已有的语义：
            替换=洋红、换包=琥珀、拦截=红、只显示=青。

            量过的两组对比度（底对表格底色 #0A0A0F / 字对底）：
                替换   1.53 / 8.30      换包   1.88 / 8.00
                拦截   1.46 / 7.89      只显示 1.83 / 8.18
            底色那一栏数值低是<b>刻意</b>的 —— 命中行靠色相区分而不是靠亮度压过正文，
            字对底则一律 ≥7.8，比老默认更好读。

            ⚠️ 这些值会<b>落库</b>（SystemConfig 表），所以只对新建的数据库生效；
            老库里存的还是老颜色，要在「系统设置」里改或换新库。
        */

        /// <summary>替换 · 前景（亮洋红）。</summary>
        public RgbColor FilterReplace_ForeColor = new RgbColor(unchecked((int)0xFFFFB0F8));

        /// <summary>替换 · 背景（暗洋红）。</summary>
        public RgbColor FilterReplace_BackColor = new RgbColor(unchecked((int)0xFF4E1C56));

        /// <summary>拦截 · 前景（亮红）。</summary>
        public RgbColor FilterIntercept_ForeColor = new RgbColor(unchecked((int)0xFFFFA5B8));

        /// <summary>拦截 · 背景（暗红）。</summary>
        public RgbColor FilterIntercept_BackColor = new RgbColor(unchecked((int)0xFF511C2B));

        /// <summary>修改 · 前景（亮琥珀）。</summary>
        public RgbColor FilterChange_ForeColor = new RgbColor(unchecked((int)0xFFF9D86F));

        /// <summary>修改 · 背景（暗琥珀）。</summary>
        public RgbColor FilterChange_BackColor = new RgbColor(unchecked((int)0xFF4E3D0D));

        /// <summary>不修改仅显示 · 前景（亮青）。</summary>
        public RgbColor FilterDisplay_ForeColor = new RgbColor(unchecked((int)0xFF8AEAFF));

        /// <summary>不修改仅显示 · 背景（暗青）。</summary>
        public RgbColor FilterDisplay_BackColor = new RgbColor(unchecked((int)0xFF0D4353));

        #endregion
    }

    #endregion
}
