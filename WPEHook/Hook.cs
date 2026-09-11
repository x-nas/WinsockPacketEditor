using EasyHook;
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
        /// 注入之后在目标进程里跑的入口：装配无头核心 + 连管道，界面与数据都在外壳。
        ///
        /// 【只剩这一条路】原先按 <c>InjectionParameters.Mode</c> 分岔，另一支是
        /// 在目标进程里开库、初始化 AntdUI、跑 WinForms 的 InjectModeForm。
        /// WinForms 界面删掉之后那一支随之删除（要找回见 git tag <c>winforms-final</c>）。
        /// </summary>
        public void Run(RemoteHooking.IContext InContext, string ChannelName, Operate.SystemConfig.InjectionParameters ipParameters)
        {
            if (ipParameters == null)
            {
                return;
            }

            RunHeadless(ipParameters);
        }

        #endregion

        #region//无头核心

        /// <summary>
        /// 目标进程里<b>只做三件事</b>：装配核心、连管道、把线程挂住。
        ///
        /// 这里<b>刻意不做</b>的事：
        ///   · 不调 <c>SetProcessDPIAware</c> —— 那改的是<b>目标进程</b>的 DPI 感知模式，
        ///     对一个非 DPI 感知的游戏会让它的坐标系整个变掉（风险清单 R4）。
        ///   · 不开数据库 —— SQLite 的文件句柄是检测面，也是崩溃时的损失来源。
        ///   · 不建任何窗口、不起 OWIN。
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
                WinsockPacketEditor.Ipc.WpeCore.Attach(
                    ipParameters.SessionId, 5000, ipParameters.SuspendedLaunch);
            }
            catch
            {
                //⚠️ 这里不能调 Operate.DoLog：核心还没装配，DoLog 会走默认实现去写文件，
                //而「目标里不打开任何文件」正是这条路径的前提。静默放弃，
                //外壳那边等不到 Hello 自然会报「注入失败」。
                return;
            }

            /*
                ⚠️ <b>唤醒挂起启动的目标，必须在这条线程上做。</b>

                CreateAndInject 把目标挂起创建，要靠 RemoteHooking.WakeUpProcess() 放行。
                实测下来别的做法都不行：
                  · 在<b>外壳</b>里调 —— 作用于外壳自己，目标永远醒不过来；
                  · 在目标的<b>控制线程</b>里调 —— 静默返回、什么都没发生；
                  · 外壳用 NtResumeProcess 直接恢复 —— 目标<b>当场死掉</b>
                    （原生的 curl.exe 与托管靶子都一样，说明 EasyHook 的「挂起」
                     不只是 OS 的挂起计数，还有它自己的一道闸）。
                EasyHook 的文档也是这么写的：「call this method in the library Run() method
                after all hooks have been installed」。

                所以这条线程装配完就挂在「第一次 StartHook」上等，
                控制线程处理完 StartHook 置位，这里再唤醒 —— 钩子装好之后才唤醒，
                第一个包也抓得到。

                对已经在跑的进程（Inject 那条路）这一句是空操作，无条件调即可。
            */
            if (WinsockPacketEditor.Ipc.WpeCore.WaitForWakeSignal())
            {
                try { EasyHook.RemoteHooking.WakeUpProcess(); } catch { }
            }

            //挂住这条线程，等核心被 Detach 或心跳超时收摊
            WinsockPacketEditor.Ipc.WpeCore.WaitForShutdown();
        }

        #endregion
    }
}
