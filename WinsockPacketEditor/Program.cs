using System;
using System.Windows.Forms;
using WinsockPacketEditor;

namespace WPEHybrid
{
    internal static class Program
    {
        #region//主函数

        /// <summary>
        /// 程序入口：提权 → DPI → 未处理异常进系统日志 → 运行时检测 → 发起 WebView2 环境预热 → 建库 → 读配置 → 建窗。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                //代理模式要装系统代理、开监听端口，注入模式要往别的进程里装钩子 —— 都要管理员权限。
                //2026-09-13 起 app.manifest 声明 requireAdministrator，正常走不到这里；留作清单没嵌进去时的兜底
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
                    未处理异常进「系统日志」，不弹 WinForms 那个默认的异常框、也不退出。

                    ⚠️ 2.1.9 起<b>不再落盘</b>：调试期那个把每条日志同时写进 exe 旁 Logs\wpe.log 的
                    LogFile 已删。
                    日志只在内存里，界面「系统日志」页可看、可导出。

                    <b>必须早于 Application.Run</b>。
                */

                //UI 线程上未捕获的异常走这条（async void 里 await 之后抛的就落在这）
                Application.ThreadException += (s, e) =>
                    Operate.DoLog("UIThread", e.Exception);

                //谁都没 await 的 Task 异常。GC 回收时才报，时间点会滞后
                System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, e) =>
                    Operate.DoLog("Task", e.Exception);

                //WebView2 运行时缺失时引导用户去装（发布包不带运行时），而不是抛一个看不懂的 COM 异常
                if (!ShellForm.HasWebView2Runtime())
                {
                    ShellForm.PromptInstallWebView2();
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

                //IP 归属地库同样在后台先读进内存，别让第一个封包去等它
                Operate.ProxyConfig.Proxy.WarmUpGeoDb();

                Operate.DataBase.InitDB();
                Operate.SystemConfig.LoadSystemConfig_FromDB();

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
