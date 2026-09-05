using EasyHook;
using System;
using System.Windows.Forms;
using WinsockPacketEditor;

namespace WPEHook
{
    public class Hook : IEntryPoint
    {
        #region//EasyHook

        public Hook()
        {
            //
        }

        public Hook(RemoteHooking.IContext InContext, string ChannelName, Operate.SystemConfig.InjectionParameters ipParameters)
        {
            //
        }

        /// <summary>
        /// 注入之后在目标进程里跑的入口。
        ///
        /// 【B-IPC 阶段 1】按 <c>ipParameters.Mode</c> 分岔：
        ///   · <b>WinFormsInProc</b> —— 改造前的路径，<b>一行未动</b>。
        ///   · <b>Headless</b>      —— 只装配无头核心 + 连管道，界面与数据都在外壳。
        ///
        /// 两条路径并存到新路径过完验证矩阵为止（方案第六节的总原则）。
        /// </summary>
        public void Run(RemoteHooking.IContext InContext, string ChannelName, Operate.SystemConfig.InjectionParameters ipParameters)
        {
            if (ipParameters != null && ipParameters.Mode == Operate.SystemConfig.InjectMode.Headless)
            {
                RunHeadless(ipParameters);
                return;
            }

            RunWinFormsInProc(ipParameters);
        }

        #endregion

        #region//无头核心（新路径）

        /// <summary>
        /// 目标进程里<b>只做三件事</b>：装配核心、连管道、把线程挂住。
        ///
        /// 与老路径逐条对照，这里<b>刻意不做</b>的事：
        ///   · 不调 <c>SetProcessDPIAware</c> —— 那改的是<b>目标进程</b>的 DPI 感知模式，
        ///     对一个非 DPI 感知的游戏会让它的坐标系整个变掉（风险清单 R4）。
        ///     它本来是为 WinForms 界面服务的，而界面不在目标里了。
        ///   · 不开数据库 —— SQLite + EF6 的文件句柄是检测面，也是崩溃时的损失来源。
        ///   · 不初始化 AntdUI、不建任何窗口、不起 OWIN。
        ///
        /// 【为什么末尾要挂住】<c>Run</c> 一返回，EasyHook 就认为注入的代码跑完了。
        /// 核心的三条工作线程都是 <c>IsBackground</c>，靠它们撑不住。
        /// 所以这里阻塞等着，直到外壳发 Detach 或心跳超时。
        /// </summary>
        private void RunHeadless(Operate.SystemConfig.InjectionParameters ipParameters)
        {
            try
            {
                //连不上就直接放弃：外壳那头的管道是注入前就建好的，
                //5 秒还连不上说明外壳已经不在了，此时目标里什么都不该留下。
                WinsockPacketEditor.Ipc.WpeCore.Attach(ipParameters.SessionId, 5000);
            }
            catch
            {
                //⚠️ 这里不能调 Operate.DoLog：核心还没装配，DoLog 会走默认实现去写文件，
                //而「目标里不打开任何文件」正是这条路径的前提。静默放弃，
                //外壳那边等不到 Hello 自然会报「注入失败」（老路径里这种情况是完全静默的）。
                return;
            }

            //挂住这条线程，等核心被 Detach 或心跳超时收摊
            WinsockPacketEditor.Ipc.WpeCore.WaitForShutdown();
        }

        #endregion

        #region//进程内 WinForms（老路径，改造前的行为，一行未动）

        private void RunWinFormsInProc(Operate.SystemConfig.InjectionParameters ipParameters)
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }

                if (ipParameters != null)
                {
                    string DBPath = ipParameters.DataBasePath;
                    if (!string.IsNullOrEmpty(DBPath))
                    {
                        Operate.DataBase.dbPath = DBPath;
                    }
                }

                Operate.SystemConfig.LoadSystemConfig_FromDB();

                //配置只落到 UI.Prefs，这里把主题与语言真正应用到 AntdUI（必须早于任何窗体创建）
                WinFormsUiHost.ApplyAll();

                //登记密码框等表单弹窗的渲染方式，否则 UI.Prompt 取不到工厂、一律返回 null
                UiDialogs.RegisterPrompts();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new InjectModeForm());
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Run), ex);
            }
        }

        #endregion
    }
}
