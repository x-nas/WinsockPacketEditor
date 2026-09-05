using System.Drawing;

namespace WinsockPacketEditor
{
    #region//界面配色

    /// <summary>
    /// 界面配色的统一入口（B3b 批次从 Operate.cs 搬出）。
    ///
    /// 【为什么搬】
    /// Operate 里原本有 18 个 System.Drawing.Color 静态字段，其中：
    ///   · 9 个会持久化（SystemColor + 8 个滤镜前景/背景色）——已移入 UI.Prefs 存成 RgbColor
    ///   · 9 个是写死的常量（6 个主题灰阶 + 3 个滤镜标记色）——从没人写过，只被界面读
    /// 后者对业务逻辑毫无意义，全部搬到本类；前者由本类做「RgbColor → Color」的转换出口。
    ///
    /// 【怎么用】
    ///   界面读色     ：UiTheme.Color_40 / UiTheme.SystemColor / UiTheme.FilterReplace_ForeColor
    ///   界面改可配置色：UI.Prefs.SystemColor = 新色.ToRgb();   然后照旧保存配置即可
    ///
    /// 本类属于 UI 层，允许自由使用 System.Drawing。
    /// </summary>
    public static class UiTheme
    {
        #region//主题灰阶（常量，不持久化）

        public static readonly Color Color_30 = Color.FromArgb(30, 30, 30);
        public static readonly Color Color_35 = Color.FromArgb(35, 35, 35);
        public static readonly Color Color_40 = Color.FromArgb(40, 40, 40);
        public static readonly Color Color_50 = Color.FromArgb(50, 50, 50);
        public static readonly Color Color_57 = Color.FromArgb(57, 57, 57);
        public static readonly Color Color_250 = Color.FromArgb(250, 250, 250);

        #endregion

        #region//滤镜标记色（常量，不持久化）

        /// <summary>递进标记：修改行里逐字节累加的那些格子。</summary>
        public static readonly Color FilterProgression_Color = Color.DarkRed;

        /// <summary>随机标记。</summary>
        public static readonly Color FilterRandom_Color = Color.DodgerBlue;

        /// <summary>排除标记：搜索行里被排除的那些格子。</summary>
        public static readonly Color FilterExclude_Color = Color.Violet;

        #endregion

        #region//可配置色（存 UI.Prefs，随系统配置持久化）

        public static Color SystemColor { get { return UI.Prefs.SystemColor.ToColor(); } }

        public static Color FilterReplace_ForeColor { get { return UI.Prefs.FilterReplace_ForeColor.ToColor(); } }
        public static Color FilterReplace_BackColor { get { return UI.Prefs.FilterReplace_BackColor.ToColor(); } }
        public static Color FilterIntercept_ForeColor { get { return UI.Prefs.FilterIntercept_ForeColor.ToColor(); } }
        public static Color FilterIntercept_BackColor { get { return UI.Prefs.FilterIntercept_BackColor.ToColor(); } }
        public static Color FilterChange_ForeColor { get { return UI.Prefs.FilterChange_ForeColor.ToColor(); } }
        public static Color FilterChange_BackColor { get { return UI.Prefs.FilterChange_BackColor.ToColor(); } }
        public static Color FilterDisplay_ForeColor { get { return UI.Prefs.FilterDisplay_ForeColor.ToColor(); } }
        public static Color FilterDisplay_BackColor { get { return UI.Prefs.FilterDisplay_BackColor.ToColor(); } }

        #endregion

        #region//列表的文字与背景颜色

        /// <summary>
        /// 按过滤动作取列表行的前景/背景色。返回 null 表示这一行不着色。
        /// 供封包列表与代理列表的单元格格式化使用。
        /// </summary>
        public static (Color ForeColor, Color BackColor)? GetFilterColors(Operate.FilterConfig.Filter.FilterAction filterAction)
        {
            switch (filterAction)
            {
                case Operate.FilterConfig.Filter.FilterAction.Replace:
                    return (FilterReplace_ForeColor, FilterReplace_BackColor);

                case Operate.FilterConfig.Filter.FilterAction.Intercept:
                    return (FilterIntercept_ForeColor, FilterIntercept_BackColor);

                case Operate.FilterConfig.Filter.FilterAction.Change:
                    return (FilterChange_ForeColor, FilterChange_BackColor);

                case Operate.FilterConfig.Filter.FilterAction.NoModify_Display:
                    return (FilterDisplay_ForeColor, FilterDisplay_BackColor);

                default:
                    return null;
            }
        }

        #endregion

        #region//机器人指令类型的颜色

        /// <summary>按机器人指令类型取标签底色。与搬迁前逐项一致。</summary>
        public static Color GetColor_ByInstructionType(Operate.RobotConfig.Robot.InstructionType instructionType)
        {
            switch (instructionType)
            {
                case Operate.RobotConfig.Robot.InstructionType.SendSendList:
                case Operate.RobotConfig.Robot.InstructionType.SendPacketList:
                    return Color.YellowGreen;

                case Operate.RobotConfig.Robot.InstructionType.SetSystemSocket:
                    return Color.Violet;

                case Operate.RobotConfig.Robot.InstructionType.Delay:
                    return Color.Khaki;

                case Operate.RobotConfig.Robot.InstructionType.LoopStart:
                case Operate.RobotConfig.Robot.InstructionType.LoopEnd:
                    return Color.Orchid;

                case Operate.RobotConfig.Robot.InstructionType.Switch:
                    return Color.DarkOrange;

                case Operate.RobotConfig.Robot.InstructionType.KeyBoard:
                    return Color.LightSeaGreen;

                case Operate.RobotConfig.Robot.InstructionType.Mouse:
                    return Color.LightSkyBlue;

                default:
                    return Color.White;
            }
        }

        #endregion
    }

    #endregion
}
