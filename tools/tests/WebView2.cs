// WebView2 SDK ↔ 本机运行时的冒烟跑测 —— 升级 Microsoft.Web.WebView2 包之后跑一次
//
// 外壳要提权（UAC），自动化起不来；这个探针用<b>产品输出目录里的那几份</b> WebView2 DLL 与 loader，
// 照 ShellForm.OnLoad 的顺序走一遍：建环境 → EnsureCoreWebView2Async → 虚拟主机映射 wwwroot →
// 那几个 Settings（NonClientRegion 要运行时 121+）→ 导航 index.html → 执行脚本读回页面状态。
//
//   cd WinsockPacketEditor\bin\Release
//   "%VS%\MSBuild\Current\Bin\Roslyn\csc.exe" -nologo -target:winexe -out:WebView2Probe.exe ^
//       -r:Microsoft.Web.WebView2.Core.dll -r:Microsoft.Web.WebView2.WinForms.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll ^
//       ..\..\..\tools\tests\WebView2.cs
//   start /wait WebView2Probe.exe & type WebView2Probe.txt     （winexe 没有控制台，结果写到旁边的 WebView2Probe.txt）
//   跑完删掉 WebView2Probe.exe / .txt 与 WebView2Probe.exe.WebView2\ 目录（浏览器进程退出要一两秒，稍等再删），别让它们进发布目录
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

static class T
{
    const string VirtualHost = "app.wpe64.local";   //与 ShellForm.VirtualHost 一致
    static int pass = 0, fail = 0;
    static StringBuilder log = new StringBuilder();

    static void Check(string name, bool ok, string detail = "")
    {
        if (ok) { pass++; } else { fail++; }
        log.AppendLine("  " + (ok ? "PASS" : "FAIL") + "  " + name + (detail.Length > 0 ? "   [" + detail + "]" : ""));
    }

    [STAThread]
    static int Main()
    {
        string dir = AppDomain.CurrentDomain.BaseDirectory;
        string udf = Path.Combine(dir, "WebView2Probe.exe.WebView2");

        string sdk = typeof(CoreWebView2Environment).Assembly.GetName().Version.ToString();
        string rt = "";
        try { rt = CoreWebView2Environment.GetAvailableBrowserVersionString(); } catch (Exception ex) { rt = "<" + ex.GetType().Name + ">"; }
        Check("① loader 找得到运行时", !string.IsNullOrEmpty(rt) && !rt.StartsWith("<"), "SDK 程序集 " + sdk + " · 运行时 " + rt);

        Form f = new Form { Opacity = 0, ShowInTaskbar = false, Size = new Size(1280, 800), StartPosition = FormStartPosition.Manual, Location = new Point(-2000, -2000) };
        WebView2 web = new WebView2 { Dock = DockStyle.Fill, DefaultBackgroundColor = Color.FromArgb(0x0A, 0x0A, 0x0F) };
        f.Controls.Add(web);

        f.Load += async (s, e) =>
        {
            try
            {
                CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(null, udf);
                await web.EnsureCoreWebView2Async(env);
                CoreWebView2 core = web.CoreWebView2;
                Check("② 建环境 + EnsureCoreWebView2Async", core != null, "浏览器进程 pid " + core.BrowserProcessId);

                core.SetVirtualHostNameToFolderMapping(VirtualHost, Path.Combine(dir, "wwwroot"), CoreWebView2HostResourceAccessKind.Allow);

                bool nc;
                try { core.Settings.IsNonClientRegionSupportEnabled = true; nc = core.Settings.IsNonClientRegionSupportEnabled; } catch { nc = false; }
                Check("③ IsNonClientRegionSupportEnabled（运行时 121+）", nc);

                core.Settings.AreDefaultContextMenusEnabled = false;
                core.Settings.IsStatusBarEnabled = false;
                Check("④ 右键菜单 / 状态栏开关", !core.Settings.AreDefaultContextMenusEnabled && !core.Settings.IsStatusBarEnabled);

                TaskCompletionSource<bool> nav = new TaskCompletionSource<bool>();
                core.NavigationCompleted += (a, b) => nav.TrySetResult(b.IsSuccess);
                core.Navigate("https://" + VirtualHost + "/index.html");
                bool ok = await Task.WhenAny(nav.Task, Task.Delay(15000)) == nav.Task && nav.Task.Result;
                Check("⑤ 虚拟主机导航 index.html", ok);

                await Task.Delay(1500);   //等 Vue 挂载
                string r = await core.ExecuteScriptAsync(
                    "JSON.stringify({ app: !!document.querySelector('#app'), kids: (document.querySelector('#app')||{children:[]}).children.length," +
                    " scripts: document.scripts.length, css: document.styleSheets.length, title: document.title })");
                Check("⑥ 页面脚本与样式都加载了、#app 已挂载", r.Contains("\\\"app\\\":true") && !r.Contains("\\\"kids\\\":0") && !r.Contains("\\\"css\\\":0"), r);
            }
            catch (Exception ex)
            {
                Check("!! 异常", false, ex.GetType().Name + ": " + ex.Message);
            }
            finally
            {
                f.Close();
            }
        };

        Application.Run(f);
        web.Dispose();

        log.AppendLine();
        log.AppendLine("  " + pass + " PASS / " + fail + " FAIL");
        //winexe 没有控制台，设 Console.OutputEncoding 会抛「句柄无效」—— 结果直接写文件
        File.WriteAllText(Path.Combine(dir, "WebView2Probe.txt"), log.ToString(), new UTF8Encoding(false));
        return fail == 0 ? 0 : 1;
    }
}
