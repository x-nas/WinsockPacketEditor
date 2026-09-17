using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WPELauncher
{
    /// <summary>启动器错误提示，按 WebUI 测试版提示的尺寸、层级和色板绘制。</summary>
    internal sealed class LauncherDialog : Form
    {
        private static readonly Color Card = Color.FromArgb(0x12, 0x12, 0x1A);
        private static readonly Color Border = Color.FromArgb(0x2A, 0x2A, 0x3A);
        private static readonly Color Amber = Color.FromArgb(0xEA, 0xB3, 0x08);
        private static readonly Color TextColor = Color.FromArgb(0xE0, 0xE0, 0xE0);
        private static readonly Color Muted = Color.FromArgb(0x6B, 0x72, 0x80);
        private readonly string heading;
        private readonly string content;
        private Rectangle buttonRect;
        private int drawingDpi = 96;
        private readonly Timer beatTimer;
        private readonly Timer cursorTimer;
        private bool beatOn = true;
        private bool cursorOn = true;

        private const int WmNcLButtonDown = 0xA1;
        private const int HtCaption = 0x2;
        private const uint MonitorDefaultToNearest = 2;
        private const int MdtEffectiveDpi = 0;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(Point point, uint flags);

        [DllImport("shcore.dll")]
        private static extern int GetDpiForMonitor(IntPtr monitor, int dpiType, out uint dpiX, out uint dpiY);

        private LauncherDialog(string title, string heading, string content)
        {
            this.heading = heading;
            this.content = content;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = true;
            Text = title;
            BackColor = Card;
            DoubleBuffered = true;
            // 内容只有一行说明，压缩 BetaNotice 的底部留白；仍按当前显示器 DPI 布局。
            ClientSize = new Size(560, 320);
            KeyPreview = true;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter) Close(); };
            beatTimer = new Timer { Interval = 550, Enabled = true };
            beatTimer.Tick += (s, e) => { beatOn = !beatOn; Invalidate(); };
            cursorTimer = new Timer { Interval = 500, Enabled = true };
            cursorTimer.Tick += (s, e) => { cursorOn = !cursorOn; Invalidate(); };
            FormClosed += (s, e) => { beatTimer.Dispose(); cursorTimer.Dispose(); };
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }
        }

        public static void ShowMcpServerInUse(string title)
        {
            using (LauncherDialog dialog = new LauncherDialog(title, Strings.McpServerInUseTitle, Strings.McpServerInUseContent))
            {
                dialog.ShowDialog();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyDpiLayout(DpiAt(Cursor.Position));
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            ApplyDpiLayout(e.DeviceDpiNew);
        }

        private void ApplyDpiLayout(int dpi)
        {
            drawingDpi = dpi > 0 ? dpi : 96;
            float scale = dpi / 96f;
            ClientSize = new Size((int)Math.Round(560 * scale), (int)Math.Round(320 * scale));
        }

        // 与 WPE 主窗体使用同一条每显示器 DPI 检测路径；DeviceDpi 在这个无边框启动器上可能仍是系统 DPI。
        private static int DpiAt(Point point)
        {
            try
            {
                IntPtr monitor = MonitorFromPoint(point, MonitorDefaultToNearest);
                uint dpiX, dpiY;
                if (monitor != IntPtr.Zero && GetDpiForMonitor(monitor, MdtEffectiveDpi, out dpiX, out dpiY) == 0 && dpiX > 0)
                    return (int)dpiX;
            }
            catch (DllNotFoundException) { }
            catch (EntryPointNotFoundException) { }

            try
            {
                using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
                    return (int)Math.Round(graphics.DpiX);
            }
            catch { return 96; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            float scale = drawingDpi / 120f;
            g.SmoothingMode = SmoothingMode.None;
            g.FillRectangle(new SolidBrush(Card), ClientRectangle);
            using (LinearGradientBrush glow = new LinearGradientBrush(new Rectangle(0, 0, Width, P(247, scale)), Color.FromArgb(18, Amber), Color.FromArgb(0, Amber), LinearGradientMode.Vertical))
            using (Pen outerBorder = new Pen(Color.FromArgb(82, Amber)))
            using (Font tag = new Font("Consolas", 7.9f))
            using (Font title = new Font("Bahnschrift SemiBold", 16.5f))
            using (Font body = new Font("Microsoft YaHei UI", 10.1f))
            // 10.15pt 在 96 DPI 下比原字号恰好约大 1px。
            using (Font warning = new Font("Consolas", 10.15f))
            using (Font buttonChinese = new Font("Microsoft YaHei UI", 9.05f))
            using (Font buttonEnglish = new Font("Microsoft YaHei UI", 8.3f))
            using (SolidBrush text = new SolidBrush(TextColor))
            using (SolidBrush muted = new SolidBrush(Muted))
            using (SolidBrush amber = new SolidBrush(Amber))
            using (Pen amberPen = new Pen(Amber, P(2, scale)))
            using (Pen marker = new Pen(Color.FromArgb(140, Amber)))
            {
                g.FillRectangle(glow, new Rectangle(0, 0, Width, P(247, scale)));
                g.DrawRectangle(outerBorder, 0, 0, Width - 1, Height - 1);
                DrawStripes(g, 0, scale);
                DrawStripes(g, Height - P(9, scale), scale);
                DrawCorner(g, marker, P(13, scale), P(27, scale), 1, 1, scale);
                DrawCorner(g, marker, Width - P(13, scale), P(27, scale), -1, 1, scale);
                DrawCorner(g, marker, P(13, scale), Height - P(27, scale), 1, -1, scale);
                DrawCorner(g, marker, Width - P(13, scale), Height - P(27, scale), -1, -1, scale);

                Rectangle tagRect = R(38, 57, 145, 34, scale);
                using (Pen tagBorder = new Pen(Color.FromArgb(115, Amber)))
                {
                    g.DrawRectangle(tagBorder, tagRect);
                }
                string tagText = "MCP SERVER";
                Size tagTextSize = TextRenderer.MeasureText(tagText, tag, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                int tagMarkSize = P(9, scale);
                int tagGap = P(12, scale);
                int tagLeft = tagRect.Left + (tagRect.Width - tagMarkSize - tagGap - tagTextSize.Width) / 2;
                // 对应 BetaNotice 的 beat：亮 550ms，暗 550ms。
                using (SolidBrush tagMark = new SolidBrush(Color.FromArgb(beatOn ? 255 : 64, Amber)))
                {
                    g.FillRectangle(tagMark, new Rectangle(tagLeft, tagRect.Top + (tagRect.Height - tagMarkSize) / 2, tagMarkSize, tagMarkSize));
                }
                TextRenderer.DrawText(g, tagText, tag, new Rectangle(tagLeft + tagMarkSize + tagGap, tagRect.Top, tagTextSize.Width, tagRect.Height), Amber,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                g.DrawString(heading, title, text, P(38, scale), P(118, scale));
                g.DrawString(content, body, muted, new RectangleF(P(38, scale), P(176, scale), Width - P(76, scale), P(48, scale)));

                Rectangle logRect = new Rectangle(P(38, scale), P(239, scale), Width - P(76, scale), P(60, scale));
                using (SolidBrush logBack = new SolidBrush(Color.FromArgb(87, 0, 0, 0)))
                using (Pen logBorder = new Pen(Border))
                {
                    g.FillRectangle(logBack, logRect);
                    g.DrawRectangle(logBorder, logRect);
                }
                TextRenderer.DrawText(g, "warning", warning, new Rectangle(logRect.Left + P(25, scale), logRect.Top, P(80, scale), logRect.Height), Amber,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                int logTextLeft = logRect.Left + P(120, scale);
                string logText = Strings.McpServerInUseLog;
                TextRenderer.DrawText(g, logText, body, new Rectangle(logTextLeft, logRect.Top, logRect.Right - logTextLeft, logRect.Height), Muted,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                if (cursorOn)
                {
                    Size logTextSize = TextRenderer.MeasureText(logText, body, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                    TextRenderer.DrawText(g, "_", body, new Rectangle(logTextLeft + logTextSize.Width + P(2, scale), logRect.Top, P(12, scale), logRect.Height), Amber,
                        TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                }

                // 底部以角标的水平臂为基准留白，和顶部标签相对于顶部角标的间距对称。
                buttonRect = new Rectangle(P(38, scale), Height - P(57, scale) - P(57, scale), Width - P(76, scale), P(57, scale));
                g.DrawRectangle(amberPen, buttonRect);
                string chinese = Strings.Ok;
                const string english = "// ACKNOWLEDGE";
                Size chineseSize = TextRenderer.MeasureText(chinese, buttonChinese, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                Size englishSize = TextRenderer.MeasureText(english, buttonEnglish, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                // TextRenderer 对中文末字仍保留较宽的右侧字形留白；回收 8px 后视觉间距才与英文单词间距一致。
                int buttonGap = -P(8, scale);
                int labelLeft = buttonRect.Left + (buttonRect.Width - chineseSize.Width - buttonGap - englishSize.Width) / 2;
                TextRenderer.DrawText(g, chinese, buttonChinese, new Rectangle(labelLeft, buttonRect.Top, chineseSize.Width, buttonRect.Height), Amber,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                TextRenderer.DrawText(g, english, buttonEnglish, new Rectangle(labelLeft + chineseSize.Width + buttonGap, buttonRect.Top, englishSize.Width, buttonRect.Height), Amber,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }
        }

        private static int P(int value, float scale) { return (int)Math.Round(value * scale); }

        private static Rectangle R(int x, int y, int width, int height, float scale)
        {
            return new Rectangle(P(x, scale), P(y, scale), P(width, scale), P(height, scale));
        }

        private static void DrawStripes(Graphics g, int y, float scale)
        {
            using (SolidBrush baseColor = new SolidBrush(Color.FromArgb(0x0A, 0x0A, 0x0F)))
            using (SolidBrush amber = new SolidBrush(Amber))
            {
                g.FillRectangle(baseColor, 0, y, P(700, scale), P(9, scale));
                for (int x = -10; x < 710; x += 23)
                    g.FillPolygon(amber, new[] { new Point(P(x, scale), y + P(9, scale)), new Point(P(x + 11, scale), y + P(9, scale)), new Point(P(x + 21, scale), y), new Point(P(x + 10, scale), y) });
            }
        }

        private static void DrawCorner(Graphics g, Pen pen, int x, int y, int dx, int dy, float scale)
        {
            g.DrawLine(pen, x, y, x + dx * P(16, scale), y);
            g.DrawLine(pen, x, y, x, y + dy * P(16, scale));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            if (buttonRect.Contains(e.Location)) { Close(); return; }
            ReleaseCapture();
            SendMessage(Handle, WmNcLButtonDown, (IntPtr)HtCaption, IntPtr.Zero);
        }
    }
}
