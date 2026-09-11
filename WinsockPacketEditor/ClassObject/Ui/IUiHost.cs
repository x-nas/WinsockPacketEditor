using System;
using System.Threading.Tasks;

namespace WinsockPacketEditor
{
    #region//提示级别

    /// <summary>
    /// 提示级别。与 AntdUI.TType 一一对应，但不依赖 AntdUI。
    /// </summary>
    public enum UiIcon
    {
        None,
        Info,
        Success,
        Warn,
        Error,
    }

    #endregion

    #region//文件选择参数

    /// <summary>
    /// 文件选择框参数。
    /// Filter 沿用 WinForms 的写法，例如："WPE x64（*.sb）|*.sb"
    /// </summary>
    public sealed class FilePick
    {
        public string Title;
        public string Filter;
        public string FileName;
        public string InitialDir;
    }

    #endregion

    #region//UI 出口

    /// <summary>
    /// Operate 唯一允许依赖的 UI 出口。
    ///
    /// 目的：把「弹窗 / 通知 / 文件框 / 遮罩」从业务逻辑里剥离，
    ///       Operate 只出数据与请求，界面由外壳决定怎么画。
    ///
    /// 实现：
    ///   BridgeUiHost（WPEHybrid/Bridge）—— 把每个调用经 JSON-RPC 推给 Vue 前端并等待回值。
    ///   （2026-09-11 之前还有一个 WinFormsUiHost，随 WinForms 界面删除。）
    ///
    /// 约定：
    ///   1. 所有方法都可能被非 UI 线程调用，线程切换由实现方负责。
    ///   2. 所有方法都不得抛异常；失败一律返回安全默认值（false / null）。
    /// </summary>
    public interface IUiHost
    {
        /// <summary>确认框。用户点确定返回 true，其余一律 false。</summary>
        Task<bool> ConfirmAsync(string Title, string Content, UiIcon Icon = UiIcon.Warn);

        /// <summary>右上角通知（有标题与正文，停留时间较长）。</summary>
        void Notify(UiIcon Level, string Title, string Content = null);

        /// <summary>轻提示（一句话，自动消失）。</summary>
        void Toast(UiIcon Level, string Text);

        /// <summary>打开文件。用户取消返回 null。</summary>
        Task<string> PickOpenAsync(FilePick Pick);

        /// <summary>保存文件。用户取消返回 null。</summary>
        Task<string> PickSaveAsync(FilePick Pick);

        /// <summary>
        /// 遮罩 + 后台执行一段耗时工作，完成后收起遮罩。
        /// Work 在后台线程执行：不得访问任何 UI 对象，也不得等待 UI 线程（会死锁）。
        /// </summary>
        Task<T> BusyAsync<T>(string Text, Func<T> Work);

        /// <summary>
        /// 表单弹窗。FormId 是两端约定的字符串（如 "encrypt-export" / "whitelist-edit"）。
        /// 用户取消返回 null。
        /// 渲染由前端组件实现（桥上的 ask「prompt」）。
        /// </summary>
        Task<TResult> PromptAsync<TResult>(string FormId, object Arg) where TResult : class;
    }

    #endregion
}
