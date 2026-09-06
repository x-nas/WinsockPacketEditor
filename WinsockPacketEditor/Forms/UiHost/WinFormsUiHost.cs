using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    #region//WinForms 侧的 UI 出口实现

    /// <summary>
    /// IUiHost 的 WinForms 实现：把接口调用翻译成现有的那套 AntdUI 调用。
    ///
    /// 本类属于「UI 层」，允许自由使用 AntdUI 与 System.Windows.Forms；
    /// Operate 只认识 IUiHost，永远不会看到本类。
    ///
    /// 线程：所有对话框都会自动切回宿主窗体的 UI 线程。
    /// 容错：宿主窗体已释放时一律返回安全默认值，不抛异常。
    /// </summary>
    public sealed class WinFormsUiHost : IUiHost
    {
        private readonly Form form;

        public WinFormsUiHost(Form form)
        {
            this.form = form;
        }

        #region//线程与存活性

        private bool IsAlive
        {
            get { return this.form != null && !this.form.IsDisposed; }
        }

        /// <summary>切回 UI 线程执行；窗体已释放或执行出错时返回 Fallback。</summary>
        private T OnUI<T>(Func<T> Work, T Fallback)
        {
            if (!this.IsAlive)
            {
                return Fallback;
            }

            try
            {
                if (this.form.IsHandleCreated && this.form.InvokeRequired)
                {
                    return (T)this.form.Invoke(Work);
                }

                return Work();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnUI), ex);
                return Fallback;
            }
        }

        #endregion

        #region//级别映射

        private static AntdUI.TType ToTType(UiIcon Icon)
        {
            switch (Icon)
            {
                case UiIcon.Success:
                    return AntdUI.TType.Success;

                case UiIcon.Warn:
                    return AntdUI.TType.Warn;

                case UiIcon.Error:
                    return AntdUI.TType.Error;

                default:
                    return AntdUI.TType.Info;
            }
        }

        #endregion

        #region//确认框

        public Task<bool> ConfirmAsync(string Title, string Content, UiIcon Icon = UiIcon.Warn)
        {
            bool bOK = this.OnUI(() =>
            {
                // 与现有 48 处 Modal 的写法保持一致：内容上下各留空行、禁用 Esc 与点遮罩关闭
                var config = new AntdUI.Modal.Config(this.form, Title, "\r\n" + Content + "\r\n\r\n", ToTType(Icon))
                {
                    Keyboard = false,
                    MaskClosable = false,
                };

                return AntdUI.Modal.open(config) == DialogResult.OK;

            }, false);

            return Task.FromResult(bOK);
        }

        #endregion

        #region//通知与轻提示

        public void Notify(UiIcon Level, string Title, string Content = null)
        {
            this.OnUI(() =>
            {
                string sContent = Content ?? string.Empty;

                switch (Level)
                {
                    case UiIcon.Success:
                        AntdUI.Notification.success(this.form, Title, sContent, AntdUI.TAlignFrom.TR);
                        break;

                    case UiIcon.Warn:
                        AntdUI.Notification.warn(this.form, Title, sContent, AntdUI.TAlignFrom.TR);
                        break;

                    case UiIcon.Error:
                        AntdUI.Notification.error(this.form, Title, sContent, AntdUI.TAlignFrom.TR);
                        break;

                    default:
                        AntdUI.Notification.info(this.form, Title, sContent, AntdUI.TAlignFrom.TR);
                        break;
                }

                return true;

            }, false);
        }

        public void Toast(UiIcon Level, string Text)
        {
            this.OnUI(() =>
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this.form, Text, ToTType(Level)));
                return true;

            }, false);
        }

        #endregion

        #region//文件选择

        public Task<string> PickOpenAsync(FilePick Pick)
        {
            string FilePath = this.OnUI(() =>
            {
                using (OpenFileDialog ofdOpenFile = new OpenFileDialog())
                {
                    ApplyPick(ofdOpenFile, Pick);
                    return ofdOpenFile.ShowDialog(this.form) == DialogResult.OK ? ofdOpenFile.FileName : null;
                }

            }, (string)null);

            return Task.FromResult(FilePath);
        }

        public Task<string> PickSaveAsync(FilePick Pick)
        {
            string FilePath = this.OnUI(() =>
            {
                using (SaveFileDialog sfdSaveFile = new SaveFileDialog())
                {
                    ApplyPick(sfdSaveFile, Pick);
                    return sfdSaveFile.ShowDialog(this.form) == DialogResult.OK ? sfdSaveFile.FileName : null;
                }

            }, (string)null);

            return Task.FromResult(FilePath);
        }

        private static void ApplyPick(FileDialog Dialog, FilePick Pick)
        {
            Dialog.RestoreDirectory = true;

            if (Pick == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(Pick.Title))
            {
                Dialog.Title = Pick.Title;
            }

            if (!string.IsNullOrEmpty(Pick.Filter))
            {
                Dialog.Filter = Pick.Filter;
            }

            if (!string.IsNullOrEmpty(Pick.FileName))
            {
                Dialog.FileName = Pick.FileName;
            }

            if (!string.IsNullOrEmpty(Pick.InitialDir))
            {
                Dialog.InitialDirectory = Pick.InitialDir;
            }
        }

        #endregion

        #region//遮罩 + 后台执行

        public Task<T> BusyAsync<T>(string Text, Func<T> Work)
        {
            if (Work == null)
            {
                return Task.FromResult(default(T));
            }

            if (!this.IsAlive || !this.form.IsHandleCreated)
            {
                // 没有可用宿主时退化为直接执行，保证业务不被界面状态卡住
                return RunInline(Work);
            }

            var tcs = new TaskCompletionSource<T>();

            this.OnUI(() =>
            {
                T Result = default(T);

                AntdUI.Spin.open(this.form, Text,
                    config =>
                    {
                        // 后台线程
                        Result = Work();
                    },
                    () =>
                    {
                        // 回到 UI 线程
                        tcs.TrySetResult(Result);
                    },
                    ex =>
                    {
                        Operate.DoLog(nameof(BusyAsync), ex);
                        tcs.TrySetException(ex);
                    });

                return true;

            }, false);

            return tcs.Task;
        }

        private static Task<T> RunInline<T>(Func<T> Work)
        {
            try
            {
                return Task.FromResult(Work());
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(RunInline), ex);

                var tcs = new TaskCompletionSource<T>();
                tcs.SetException(ex);
                return tcs.Task;
            }
        }

        #endregion

        #region//表单弹窗

        /// <summary>
        /// FormId 到渲染方式的登记表，由 UI 层在启动时填充（B1 / B6 批次会用到）：
        ///
        ///     WinFormsUiHost.RegisterPrompt("encrypt-export", (owner, arg) =>
        ///     {
        ///         ... 打开承载 UserControl 的 Modal ...
        ///         return 结果对象;   // 取消则 return null
        ///     });
        ///
        /// 工厂在 UI 线程上被调用，返回 null 表示用户取消。
        /// </summary>
        private static readonly Dictionary<string, Func<Form, object, object>> dicPrompts =
            new Dictionary<string, Func<Form, object, object>>(StringComparer.OrdinalIgnoreCase);

        public static void RegisterPrompt(string FormId, Func<Form, object, object> Factory)
        {
            if (string.IsNullOrEmpty(FormId) || Factory == null)
            {
                return;
            }

            dicPrompts[FormId] = Factory;
        }

        public static bool IsPromptRegistered(string FormId)
        {
            return !string.IsNullOrEmpty(FormId) && dicPrompts.ContainsKey(FormId);
        }

        public Task<TResult> PromptAsync<TResult>(string FormId, object Arg) where TResult : class
        {
            Func<Form, object, object> Factory;

            if (!dicPrompts.TryGetValue(FormId ?? string.Empty, out Factory))
            {
                Operate.DoLog(nameof(PromptAsync), "未登记的表单弹窗: " + FormId);
                return Task.FromResult<TResult>(null);
            }

            TResult Result = this.OnUI(() => Factory(this.form, Arg) as TResult, (TResult)null);

            return Task.FromResult(Result);
        }

        #endregion

        #region//界面偏好回写

        /// <summary>
        /// 把 UI.Prefs 的主题项同步到 AntdUI.Config。
        ///
        /// UI.Prefs 是这些值的唯一真源，AntdUI.Config 是它的运行时镜像。
        /// 界面上改了显示设置或暗色开关，先写 UI.Prefs 再调本方法，不要直接写 AntdUI.Config。
        /// </summary>
        public static void ApplyPrefs()
        {
            try
            {
                UiPrefs p = UI.Prefs;

                AntdUI.Config.SetEmptyImageSvg(Properties.Resources.icon_empty, Properties.Resources.icon_empty_dark);

                AntdUI.Config.IsDark = p.IsDark;
                AntdUI.Config.Animation = p.IsAnimation;
                AntdUI.Config.ShadowEnabled = p.IsShadowEnabled;
                AntdUI.Config.ShowInWindow = p.IsShowInWindow;
                AntdUI.Config.ScrollBarHide = p.IsScrollBarHide;
                AntdUI.Config.TextRenderingHighQuality = p.IsTextRenderingHighQuality;

                //与迁移前逐字一致：只在开启时设置，关闭时不复位（原代码就是单向的）
                if (p.IsTextRenderingHighQuality)
                {
                    AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ApplyPrefs), ex);
            }
        }

        /// <summary>
        /// 把 UI.Prefs.Language 应用到 AntdUI.Localization。
        ///
        /// 与迁移前同一条口径，只是语言从两种变成六种：有对照表的挂 Localizer，中文置空走兜底；
        /// DefaultLanguage 恒为 "zh-CN"（兜底语言，不随界面语言变）。
        /// 单独成一个方法，是为了让「只改主题」的调用不去触发一次全窗体的重新本地化。
        /// </summary>
        public static void ApplyLanguage()
        {
            try
            {
                string Lang = UI.Prefs.Language;
                if (string.IsNullOrEmpty(Lang))
                {
                    Lang = "zh-CN";
                }

                /*
                    中文<b>不装 Provider</b>：每一处 UI.T(key, "中文") 都自带中文原文，
                    AntdUI 在 Provider 为 null 时用的就是那一份。再抄一份中文表进来
                    只会多出一处要同步的地方。

                    其余五种各有一份对照表（ClassObject/L10n/），认不出来的语言
                    L10n.Has 返回 false，同样走中文兜底。
                */
                AntdUI.Localization.Provider = L10n.Has(Lang) ? new Localizer(Lang) : null;

                AntdUI.Localization.DefaultLanguage = "zh-CN";
                AntdUI.Localization.SetLanguage(Lang);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ApplyLanguage), ex);
            }
        }

        /// <summary>
        /// 主题 + 语言一起应用。用在两处：
        ///   1. 程序启动，紧跟 Operate.SystemConfig.LoadSystemConfig_FromDB() 之后
        ///      （Program.Main 与 WPEHook.Hook.Run，此时还没有任何窗体）
        ///   2. 导入系统备份之后（整份配置被覆盖）
        /// </summary>
        public static void ApplyAll()
        {
            ApplyPrefs();
            ApplyLanguage();
        }

        #endregion
    }

    #endregion
}
