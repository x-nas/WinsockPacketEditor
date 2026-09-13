// WPE x64 数据文件图标（wpe-data.ico）的生成器 —— 以 wpe.ico 为底图，画「一页带折角的纸 + 右下角那顶帽子」
//
// 资源管理器里 .sb / .fp / .sp … 这 13 种 WPE 自己的导出文件用它（注册见 ClassObject/FileAssociation.cs）。
// 2026-09-11 定稿的是「B · 纸内徽章」：纸上几行灰色数据行表明「这是一份数据」，帽子当右下角的徽章。
//
// ⚠️ 这是派生产物：换了 WinsockPacketEditor\wpe.ico 就重跑一次（与管理台那份 Web\wpe.ico 子集同一个道理）。
//
//   cd tools\icons
//   "%VS%\MSBuild\Current\Bin\Roslyn\csc.exe" -nologo -r:System.Drawing.dll DataIcon.cs
//   DataIcon.exe                         → 写 ..\..\WinsockPacketEditor\wpe-data.ico
//   DataIcon.exe --preview <目录>        → 另外把每一层导成 PNG，方便看
//   （跑完删掉 DataIcon.exe，别入库）
//
// 几条定下来的做法：
//   · 8 层：16 / 20 / 24 / 32 / 40 / 48 / 64 / 256 —— Windows 在 100%~250% 缩放下会取的全部尺寸。
//     256 那层存 PNG（否则一层就 256 KB），其余存 32 位 BMP（老的读取方只认它）。
//   · 32px 及以下<b>逐像素</b>画：GDI+ 在这个尺寸会做半像素偏移，纸的上边线会丢、帽子会糊（第一版预览就栽过）。
//   · 帽子优先用 wpe.ico 里<b>同尺寸的原始像素</b>，没有同尺寸的才从大一档缩下来（HighQualityBicubic）。
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

static class DataIcon
{
    static readonly int[] Sizes = { 16, 20, 24, 32, 40, 48, 64, 256 };

    static readonly Color Edge = Color.FromArgb(0x8E, 0x98, 0xA4);        //纸的边线
    static readonly Color FoldFill = Color.FromArgb(0xDC, 0xE2, 0xE9);    //折角
    static readonly Color Row = Color.FromArgb(0xC3, 0xCA, 0xD3);         //数据行
    static readonly Color PaperTop = Color.White, PaperBottom = Color.FromArgb(0xEE, 0xF1, 0xF5);

    //wpe.ico 的各层，按边长；已按墨迹裁掉上下的空白（帽子横向本来就是满的）
    static readonly Dictionary<int, Bitmap> hats = new Dictionary<int, Bitmap>();

    static int Main(string[] args)
    {
        string here = AppDomain.CurrentDomain.BaseDirectory;
        string src = Path.GetFullPath(Path.Combine(here, @"..\..\WinsockPacketEditor\wpe.ico"));
        string dst = Path.GetFullPath(Path.Combine(here, @"..\..\WinsockPacketEditor\wpe-data.ico"));
        string preview = args.Length >= 2 && args[0] == "--preview" ? args[1] : null;

        LoadHats(src);

        List<Bitmap> layers = new List<Bitmap>();
        foreach (int s in Sizes)
        {
            Bitmap b = s <= 32 ? RenderSmall(s) : RenderBig(s);
            layers.Add(b);
            if (preview != null) { Directory.CreateDirectory(preview); b.Save(Path.Combine(preview, "wpe-data-" + s + ".png"), ImageFormat.Png); }
        }

        WriteIco(dst, layers);
        Console.WriteLine("wrote " + dst + "  (" + new FileInfo(dst).Length + " bytes, " + layers.Count + " layers)");
        return 0;
    }

    #region//读 wpe.ico

    static void LoadHats(string path)
    {
        byte[] b = File.ReadAllBytes(path);
        int n = BitConverter.ToUInt16(b, 4);

        for (int i = 0; i < n; i++)
        {
            int o = 6 + i * 16;
            int w = b[o] == 0 ? 256 : b[o];
            int size = BitConverter.ToInt32(b, o + 8), off = BitConverter.ToInt32(b, o + 12);

            Bitmap bmp;
            if (b[off] == 0x89 && b[off + 1] == 0x50)
            {
                //PNG 层（主图标哪天换成 PNG 压缩的也能读）
                using (MemoryStream ms = new MemoryStream(b, off, size)) { bmp = new Bitmap(Image.FromStream(ms)); }
            }
            else
            {
                //32 位 BMP 层：40 字节头 + 自下而上的 BGRA（其后的 AND 掩码用不上）
                int bpp = BitConverter.ToUInt16(b, off + 14);
                if (bpp != 32) { continue; }
                bmp = new Bitmap(w, w, PixelFormat.Format32bppArgb);
                int px = off + BitConverter.ToInt32(b, off);
                for (int y = 0; y < w; y++)
                    for (int x = 0; x < w; x++)
                    {
                        int p = px + ((w - 1 - y) * w + x) * 4;
                        bmp.SetPixel(x, y, Color.FromArgb(b[p + 3], b[p + 2], b[p + 1], b[p]));
                    }
            }

            int top = w, bot = -1;
            for (int y = 0; y < w; y++) for (int x = 0; x < w; x++) if (bmp.GetPixel(x, y).A > 8) { if (y < top) top = y; if (y > bot) bot = y; }
            hats[w] = bmp.Clone(new Rectangle(0, top, w, bot - top + 1), PixelFormat.Format32bppArgb);
        }

        if (!hats.ContainsKey(256)) { throw new InvalidDataException("wpe.ico 里没有 256 那一层（32 位）"); }
    }

    //宽 w 的帽子：有同尺寸的原图就原样用，否则从大一档缩
    static Bitmap Hat(int w)
    {
        int src = 256;
        foreach (int n in new[] { 16, 32, 48, 64, 128, 256 }) { if (n >= w && hats.ContainsKey(n)) { src = n; break; } }

        Bitmap h = hats[src];
        if (h.Width == w) { return h; }

        int hh = (int)Math.Round(h.Height * (double)w / h.Width);
        Bitmap o = new Bitmap(w, hh, PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage(o))
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(h, new Rectangle(0, 0, w, hh));
        }
        return o;
    }

    #endregion

    #region//32px 及以下：逐像素

    static Bitmap RenderSmall(int s)
    {
        Bitmap bmp = new Bitmap(s, s, PixelFormat.Format32bppArgb);

        int L, T, W, H, f;
        if (s == 16) { L = 2; T = 0; W = 12; H = 16; f = 4; }
        else if (s == 20) { L = 3; T = 0; W = 14; H = 20; f = 5; }
        else if (s == 24) { L = 3; T = 0; W = 18; H = 24; f = 6; }
        else { L = 5; T = 1; W = 22; H = 31; f = 7; }
        int R = L + W - 1, B = T + H - 1;

        for (int y = T; y <= B; y++)
            for (int x = L; x <= R; x++)
            {
                int fx = x - (R - f), fy = y - T;
                if (fx > 0 && fy < f && fx > fy) { continue; }                                    //折角外面：透明

                Color c = Lerp(PaperTop, PaperBottom, (y - T) / (double)H);
                if (fx >= 0 && fy <= f && fx <= fy) { c = FoldFill; }                             //折角
                bool border = x == L || y == B || (y == T && x <= R - f) || (x == R && y >= T + f)
                              || (fx > 0 && fx == fy && fy <= f) || (fx == 0 && fy <= f) || (fy == f && fx >= 0);
                if (border) { c = Edge; }
                bmp.SetPixel(x, y, c);
            }

        //32px 才放得下数据行（两像素高）
        if (s == 32)
        {
            for (int x = L + 3; x <= R - f - 2; x++) { bmp.SetPixel(x, T + 7, Row); bmp.SetPixel(x, T + 8, Row); }
            for (int r = 1; r < 3; r++)
                for (int x = L + 3; x <= R - 5 - r * 3; x++) { bmp.SetPixel(x, T + 7 + r * 4, Row); bmp.SetPixel(x, T + 8 + r * 4, Row); }
        }

        int hw = s - (s >= 24 ? 4 : 2);
        Bitmap h = Hat(hw);
        int hx = (s - hw) / 2 + 1, hy = s - h.Height;
        for (int y = 0; y < h.Height; y++) for (int x = 0; x < h.Width; x++) { Over(bmp, hx + x, hy + y, h.GetPixel(x, y)); }

        return bmp;
    }

    static Color Lerp(Color a, Color b, double t)
    {
        return Color.FromArgb(255, (int)Math.Round(a.R + (b.R - a.R) * t), (int)Math.Round(a.G + (b.G - a.G) * t), (int)Math.Round(a.B + (b.B - a.B) * t));
    }

    //source-over（非预乘）
    static void Over(Bitmap d, int x, int y, Color c)
    {
        if (x < 0 || y < 0 || x >= d.Width || y >= d.Height || c.A == 0) { return; }
        Color b = d.GetPixel(x, y);
        double a = c.A / 255.0, ba = b.A / 255.0, oa = a + ba * (1 - a);
        Func<int, int, int> m = (cs, bs) => (int)Math.Round((cs * a + bs * ba * (1 - a)) / oa);
        d.SetPixel(x, y, Color.FromArgb((int)Math.Round(oa * 255), m(c.R, b.R), m(c.G, b.G), m(c.B, b.B)));
    }

    #endregion

    #region//40px 及以上：GDI+ 抗锯齿

    static GraphicsPath PagePath(RectangleF r, float f)
    {
        GraphicsPath p = new GraphicsPath();
        p.AddLine(r.Left, r.Top, r.Right - f, r.Top);
        p.AddLine(r.Right - f, r.Top, r.Right, r.Top + f);
        p.AddLine(r.Right, r.Top + f, r.Right, r.Bottom);
        p.AddLine(r.Right, r.Bottom, r.Left, r.Bottom);
        p.CloseFigure();
        return p;
    }

    static Bitmap RenderBig(int s)
    {
        Bitmap bmp = new Bitmap(s, s, PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            float k = s / 256f;

            RectangleF page = new RectangleF(R(36 * k), R(6 * k), R(184 * k), R(244 * k));
            float fold = R(54 * k), bw = Math.Max(1f, R(2.5f * k));

            //投影（48 起才画，更小的看不出来只会发脏）
            if (s >= 48)
                for (int i = 3; i >= 1; i--)
                    using (GraphicsPath sp = PagePath(new RectangleF(page.X + i * k * 1.5f, page.Y + i * k * 2.5f, page.Width, page.Height), fold))
                    using (SolidBrush sb = new SolidBrush(Color.FromArgb(10, 0, 0, 0))) { g.FillPath(sb, sp); }

            using (GraphicsPath pp = PagePath(page, fold))
            {
                using (LinearGradientBrush lb = new LinearGradientBrush(new PointF(0, page.Top), new PointF(0, page.Bottom + 1), PaperTop, PaperBottom)) { g.FillPath(lb, pp); }

                //数据行：像十六进制视图里一行行的字节
                float[] len = { 0.62f, 0.78f, 0.52f, 0.70f, 0.60f, 0.74f };
                float lx = page.Left + 22 * k, lt = page.Top + 50 * k, lh = Math.Max(1f, 9 * k), gap = 22 * k;
                using (SolidBrush rb = new SolidBrush(Row))
                    for (int i = 0; i < len.Length; i++)
                    {
                        float w = (page.Width - 48 * k) * len[i];
                        if (i == 0) { w = Math.Min(w, page.Width - fold - 34 * k); }
                        g.FillRectangle(rb, R(lx), R(lt + i * gap), R(w), R(lh));
                    }

                using (Pen pen = new Pen(Edge, bw) { Alignment = PenAlignment.Inset }) { g.DrawPath(pen, pp); }
            }

            using (GraphicsPath fp = new GraphicsPath())
            {
                fp.AddLine(page.Right - fold, page.Top, page.Right - fold, page.Top + fold);
                fp.AddLine(page.Right - fold, page.Top + fold, page.Right, page.Top + fold);
                fp.CloseFigure();
                using (SolidBrush fb = new SolidBrush(FoldFill)) { g.FillPath(fb, fp); }
                using (Pen pen = new Pen(Edge, bw) { Alignment = PenAlignment.Inset }) { g.DrawPath(pen, fp); }
            }

            //帽子：右下角的徽章，下沿与画布底平齐、右侧略出纸边
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            int hw = (int)Math.Round(150 * k);
            Bitmap h = Hat(hw);
            float hx = page.Right - hw + 26 * k, hy = page.Bottom + 6 * k - h.Height;
            g.DrawImage(h, (int)Math.Round(hx), (int)Math.Round(hy), h.Width, h.Height);
        }
        return bmp;
    }

    static float R(float v) { return (float)Math.Round(v); }

    #endregion

    #region//写 .ico

    static void WriteIco(string path, List<Bitmap> layers)
    {
        List<byte[]> blobs = new List<byte[]>();
        foreach (Bitmap b in layers) { blobs.Add(b.Width >= 256 ? Png(b) : Dib(b)); }

        using (MemoryStream ms = new MemoryStream())
        using (BinaryWriter w = new BinaryWriter(ms))
        {
            w.Write((ushort)0); w.Write((ushort)1); w.Write((ushort)layers.Count);

            int offset = 6 + 16 * layers.Count;
            for (int i = 0; i < layers.Count; i++)
            {
                int s = layers[i].Width;
                w.Write((byte)(s >= 256 ? 0 : s)); w.Write((byte)(s >= 256 ? 0 : s));
                w.Write((byte)0); w.Write((byte)0);
                w.Write((ushort)1); w.Write((ushort)32);
                w.Write(blobs[i].Length); w.Write(offset);
                offset += blobs[i].Length;
            }
            foreach (byte[] blob in blobs) { w.Write(blob); }

            File.WriteAllBytes(path, ms.ToArray());
        }
    }

    static byte[] Png(Bitmap b)
    {
        using (MemoryStream ms = new MemoryStream()) { b.Save(ms, ImageFormat.Png); return ms.ToArray(); }
    }

    //32 位 BMP：BITMAPINFOHEADER（高度写两倍）+ 自下而上的 BGRA + 1 位 AND 掩码（每行补齐到 4 字节）
    static byte[] Dib(Bitmap b)
    {
        int s = b.Width, maskStride = ((s + 31) / 32) * 4;
        using (MemoryStream ms = new MemoryStream())
        using (BinaryWriter w = new BinaryWriter(ms))
        {
            w.Write(40); w.Write(s); w.Write(s * 2); w.Write((ushort)1); w.Write((ushort)32);
            w.Write(0); w.Write(s * s * 4 + maskStride * s); w.Write(0); w.Write(0); w.Write(0); w.Write(0);

            for (int y = s - 1; y >= 0; y--)
                for (int x = 0; x < s; x++)
                {
                    Color c = b.GetPixel(x, y);
                    w.Write(c.B); w.Write(c.G); w.Write(c.R); w.Write(c.A);
                }

            for (int y = s - 1; y >= 0; y--)
            {
                byte[] row = new byte[maskStride];
                for (int x = 0; x < s; x++) { if (b.GetPixel(x, y).A == 0) { row[x / 8] |= (byte)(0x80 >> (x % 8)); } }
                w.Write(row);
            }
            return ms.ToArray();
        }
    }

    #endregion
}
