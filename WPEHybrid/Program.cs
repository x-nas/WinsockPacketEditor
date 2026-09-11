using System;
using System.Windows.Forms;
using WinsockPacketEditor;

namespace WPEHybrid
{
    internal static class Program
    {
        #region//主函数

        /// <summary>
        /// 启动顺序与主程序 WinsockPacketEditor/Program.cs 保持一致，
        /// 只是最后打开的是 WebView2 外壳而不是 StartForm。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                //代理模式要装系统代理、开监听端口，与主程序一样需要管理员权限
                if (!Operate.SystemConfig.IsAdministrator())
                {
                    Operate.SystemConfig.RestartAsAdmin();
                    return;
                }

                /*
                    ⚠️ 这一句在外壳里实际<b>不起作用</b>：app.manifest 已经声明了 PerMonitorV2，
                    清单先生效，之后再调它只会失败返回。真正的 DPI 模式是 PerMonitorV2 ——
                    窗口大小按所在显示器的缩放定，见 ShellForm.FitToDpi / OnDpiChanged。
                    留着它只为清单万一没嵌进去时还有个系统级 DPI 感知兜底。
                */
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                /*
                    崩溃留痕。

                    「程序意外退出」以前不留任何东西 —— 运行日志只在内存里，
                    进程一死就没了，事后完全查不出发生过什么（真栽过一次：
                    账号表被清空，查不到是哪条路径干的）。
                    这两个订阅让最后一口气也能写进 Logs\wpe.log。

                    <b>必须早于 Application.Run</b>，也早于任何可能抛异常的初始化。
                */
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                    LogFile.Crash("AppDomain", e.ExceptionObject);

                //UI 线程上未捕获的异常走这条（async void 里 await 之后抛的就落在这）
                Application.ThreadException += (s, e) =>
                    LogFile.Crash("UIThread", e.Exception);

                //谁都没 await 的 Task 异常。GC 回收时才报，所以时间点会滞后，但总比没有强
                System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, e) =>
                    LogFile.Crash("Task", e.Exception);

                //WebView2 运行时缺失时给出明确指引，而不是抛一个看不懂的 COM 异常
                string sRuntimeError = ShellForm.CheckWebView2Runtime();
                if (sRuntimeError != null)
                {
                    MessageBox.Show(sRuntimeError, "WPE x64", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                /*
                    先把 WebView2 环境的创建<b>发起</b>，不等它。

                    它要拉起一个独立的浏览器进程，是启动路径上最慢的一步（冷启动可达一秒以上），
                    而下面的建库 / 读配置与它完全无关。原来两件事是串行的，
                    这样改成并行，墙钟时间约等于两者中较慢的那个。

                    ShellForm 在 OnLoad 里 await 这个 Task；万一它失败了，那边会回退成自己创建，
                    所以这里不需要处理异常，只是别让未观察的异常冒出来。
                */
                ShellForm.BeginCreateEnvironment();

                Operate.DataBase.InitDB();
                Operate.SystemConfig.LoadSystemConfig_FromDB();

                //一行分隔，事后翻日志时用它切分「这是哪一次运行、用的哪个库」
                LogFile.BeginSession(
                    typeof(Operate).Assembly.GetName().Version.ToString(),
                    Operate.DataBase.dbPath);

                //配置只落到 UI.Prefs，这里才真正应用到 AntdUI（必须早于任何窗体创建）
                //注意：外壳本身没有 AntdUI 界面，但 Operate 内部仍会读 UI.Prefs，
                //而且将来若从这里弹出 WinForms 编辑器也需要它
                WinFormsUiHost.ApplyAll();
                UiDialogs.RegisterPrompts();

                Application.Run(new ShellForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误 Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
