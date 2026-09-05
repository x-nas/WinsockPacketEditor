using System;
using System.Threading.Tasks;

namespace WinsockPacketEditor
{
    #region//UI 门面

    /// <summary>
    /// UI 门面。Operate 通过它访问界面，从而不再直接依赖 AntdUI / System.Windows.Forms。
    ///
    /// 【注入时机】与 Operate.SystemConfig.InvokeAction 同级，在三个窗体的构造函数里各一行：
    ///     UI.Attach(new WinFormsUiHost(this), new AntdL10n());
    ///
    /// 【未注入时的行为】（注入模式下 Hook.Run 会在建窗之前就 LoadSystemConfig_FromDB）：
    ///     T()        → 直接返回中文兜底文案
    ///     Prefs      → 永远可用，纯 POCO
    ///     Confirm    → false（即「什么都不做」，是安全的默认：不会误删数据）
    ///     Notify/Toast → 静默丢弃
    ///     PickOpen/PickSave/Prompt → null
    ///     Busy       → 仍然执行 Work，只是没有遮罩
    ///   一律不抛异常。
    ///
    /// 【为什么不挂在 Operate.SystemConfig 上】
    ///     Operate.cs 已有 27000 行，不再往里加东西；
    ///     独立门面也让 Operate 将来能整体搬进独立程序集。
    /// </summary>
    public static class UI
    {
        #region//注入

        public static IUiHost Host { get; private set; }

        public static IL10n L10n { get; private set; }

        /// <summary>
        /// 列表数据出口（B9b 引入）。
        ///
        /// <b>永不为 null</b>：未注入时是下面的 NullFeed，Operate 不必判空。
        /// 构造 DTO 之前先查 <c>UI.Feed.NeedsRows</c> —— 纯 WinForms 运行时它是 false，
        /// 可以整段跳过转换。
        /// </summary>
        public static IUiFeed Feed { get; private set; }

        /// <summary>界面偏好。纯 POCO，与是否 Attach 无关，任何时候都可读写。</summary>
        public static UiPrefs Prefs { get; private set; }

        static UI()
        {
            Prefs = new UiPrefs();
            Feed = new NullFeed();
        }

        /// <summary>
        /// 未注入任何数据出口时的占位实现：全部空转，NeedsRows 为 false。
        /// 有它 UI.Feed 才能保证永不为 null。
        /// </summary>
        private sealed class NullFeed : IUiFeed
        {
            public bool NeedsRows { get { return false; } }
            public void Append(FeedList List, object[] Rows) { }
            public void Replace(FeedList List, object[] Rows) { }
            public void Update(FeedList List, object Row) { }
            public void Remove(FeedList List, string Id) { }
            public void Clear(FeedList List) { }
        }

        public static bool Attached
        {
            get { return Host != null; }
        }

        public static void Attach(IUiHost Host, IL10n L10n)
        {
            UI.Host = Host;
            UI.L10n = L10n;
        }

        /// <summary>注入列表数据出口。传 null 则回到空转的 NullFeed。</summary>
        public static void AttachFeed(IUiFeed Feed)
        {
            UI.Feed = Feed ?? new NullFeed();
        }

        /// <summary>
        /// 摘除注入。
        ///
        /// <b>刻意不在窗体的 FormClosing 里调用</b>：FormClosing 可以被 e.Cancel 取消，
        /// 一旦在那里 Detach 而关闭又被取消，后续所有 UI 调用会静默失效，属于很隐蔽的坑。
        /// 窗体切换时（StartForm → ProxyModeForm）下一次 Attach 会自然覆盖，进程退出时自然回收。
        /// 本方法保留给测试与将来的桥宿主使用。
        /// </summary>
        public static void Detach()
        {
            UI.Host = null;
            UI.L10n = null;
        }

        #endregion

        #region//文案

        /// <summary>取本地化文案。未注入或查询失败时返回 Fallback。</summary>
        public static string T(string Key, string Fallback)
        {
            IL10n l = L10n;
            if (l == null)
            {
                return Fallback;
            }

            try
            {
                string s = l.Get(Key, Fallback);
                return string.IsNullOrEmpty(s) ? Fallback : s;
            }
            catch
            {
                return Fallback;
            }
        }

        #endregion

        #region//转发

        public static Task<bool> Confirm(string Title, string Content, UiIcon Icon = UiIcon.Warn)
        {
            IUiHost h = Host;
            if (h == null)
            {
                Missing("Confirm");
                return Task.FromResult(false);
            }

            return h.ConfirmAsync(Title, Content, Icon);
        }

        public static void Notify(UiIcon Level, string Title, string Content = null)
        {
            IUiHost h = Host;
            if (h == null)
            {
                Missing("Notify");
                return;
            }

            h.Notify(Level, Title, Content);
        }

        public static void Toast(UiIcon Level, string Text)
        {
            IUiHost h = Host;
            if (h == null)
            {
                Missing("Toast");
                return;
            }

            h.Toast(Level, Text);
        }

        public static Task<string> PickOpen(FilePick Pick)
        {
            IUiHost h = Host;
            if (h == null)
            {
                Missing("PickOpen");
                return Task.FromResult<string>(null);
            }

            return h.PickOpenAsync(Pick);
        }

        public static Task<string> PickSave(FilePick Pick)
        {
            IUiHost h = Host;
            if (h == null)
            {
                Missing("PickSave");
                return Task.FromResult<string>(null);
            }

            return h.PickSaveAsync(Pick);
        }

        /// <summary>遮罩 + 后台执行。未注入时仍然执行 Work，只是没有遮罩。</summary>
        public static Task<T> Busy<T>(string Text, Func<T> Work)
        {
            IUiHost h = Host;
            if (h == null)
            {
                Missing("Busy");

                try
                {
                    return Task.FromResult(Work());
                }
                catch (Exception ex)
                {
                    var tcs = new TaskCompletionSource<T>();
                    tcs.SetException(ex);
                    return tcs.Task;
                }
            }

            return h.BusyAsync(Text, Work);
        }

        public static Task<TResult> Prompt<TResult>(string FormId, object Arg) where TResult : class
        {
            IUiHost h = Host;
            if (h == null)
            {
                Missing("Prompt:" + FormId);
                return Task.FromResult<TResult>(null);
            }

            return h.PromptAsync<TResult>(FormId, Arg);
        }

        #endregion

        #region//未注入告警

        /// <summary>
        /// 只在 Debug 下输出，Release 编译期即被移除。
        /// 刻意不走 Operate.DoLog：门面不应反向依赖 Operate。
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private static void Missing(string Method)
        {
            System.Diagnostics.Debug.WriteLine("[UI] 尚未 Attach，忽略调用：" + Method);
        }

        #endregion
    }

    #endregion
}
