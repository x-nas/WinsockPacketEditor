using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WPELauncher
{
    /// <summary>
    /// 解压时的进度窗。只在真的要解压 / 修复时出现，平时启动器一闪而过、什么都不画。
    /// 标题是 payload.txt 的 Title（WPE x64 / WPE Proxy Cap）。
    /// 配色照 WPE 的深色令牌（底 #0A0A0F · 边 #2A2A3A · 绿 #00FF88），自绘不用控件。
    /// </summary>
    internal sealed class SplashForm : Form
    {
        private static readonly Color Back = Color.FromArgb(0x0A, 0x0A, 0x0F);
        private static readonly Color Card = Color.FromArgb(0x12, 0x12, 0x1A);
        private static readonly Color Border = Color.FromArgb(0x2A, 0x2A, 0x3A);
        private static readonly Color Green = Color.FromArgb(0x00, 0xFF, 0x88);
        private static readonly Color Gray = Color.FromArgb(0xE0, 0xE0, 0xE0);
        private static readonly Color Muted = Color.FromArgb(0x9A, 0xA3, 0xB0);

        private readonly string title;
        private readonly string version;
        private readonly string status;
        private readonly float scale;
        private int percent;

        private SplashForm(string title, string version, string status)
        {
            this.title = title;
            this.version = version;
            this.status = status;

            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                this.scale = g.DpiX / 96f;
            }

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = true;
            this.Text = title;
            this.BackColor = Back;
            this.DoubleBuffered = true;
            this.ClientSize = new Size(S(440), S(128));

            try
            {
                this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>显示进度窗并在后台跑 <paramref name="work"/>；返回它抛出的异常（成功为 null）。</summary>
        public static Exception Run(string title, string version, string status, Action<IProgress<int>> work)
        {
            Exception error = null;

            using (SplashForm form = new SplashForm(title, version, status))
            {
                form.Shown += async (s, e) =>
                {
                    //在 UI 线程上建，Report 自动 marshal 回来
                    Progress<int> progress = new Progress<int>(form.SetPercent);

                    try
                    {
                        await Task.Run(() => work(progress));
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                    finally
                    {
                        form.Close();
                    }
                };

                Application.Run(form);
            }

            return error;
        }

        private int S(int v)
        {
            return (int)Math.Round(v * this.scale);
        }

        private void SetPercent(int value)
        {
            if (value != this.percent)
            {
                this.percent = Math.Max(0, Math.Min(100, value));
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

            using (SolidBrush card = new SolidBrush(Card))
            using (Pen border = new Pen(Border))
            using (SolidBrush green = new SolidBrush(Green))
            {
                g.FillRectangle(card, 0, 0, w, h);
                g.DrawRectangle(border, 0, 0, w - 1, h - 1);

                //顶沿色轨：左 34% 实心，与启动页模式卡同一个形
                g.FillRectangle(green, 0, 0, (int)(w * 0.34), S(3));
            }

            int pad = S(22);

            using (Font titleFont = new Font("Segoe UI Semibold", 13f, FontStyle.Regular, GraphicsUnit.Point))
            using (Font small = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point))
            using (SolidBrush gray = new SolidBrush(Gray))
            using (SolidBrush muted = new SolidBrush(Muted))
            using (SolidBrush green = new SolidBrush(Green))
            {
                g.DrawString(this.title, titleFont, gray, pad, S(18));

                SizeF tw = g.MeasureString(this.title, titleFont);
                g.DrawString("v" + this.version, small, green, pad + tw.Width + S(4), S(18) + (tw.Height - g.MeasureString("v", small).Height) - S(3));

                g.DrawString(this.status, small, muted, pad, S(54));

                string pct = this.percent + "%";
                SizeF pw = g.MeasureString(pct, small);
                g.DrawString(pct, small, gray, w - pad - pw.Width, S(54));

                int barY = S(88);
                int barH = S(6);
                int barW = w - pad * 2;

                using (SolidBrush track = new SolidBrush(Border))
                {
                    g.FillRectangle(track, pad, barY, barW, barH);
                }

                g.FillRectangle(green, pad, barY, (int)(barW * (this.percent / 100f)), barH);
            }
        }
    }
}
