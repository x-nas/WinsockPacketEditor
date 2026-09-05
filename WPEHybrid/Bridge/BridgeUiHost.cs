using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinsockPacketEditor;

namespace WPEHybrid
{
    #region//桥侧的 UI 出口实现

    /// <summary>
    /// <see cref="IUiHost"/> 的桥实现（B10b）。
    ///
    /// 每个方法变成一次 <see cref="WebBridge.AskAsync{T}"/>，由 Vue 前端渲染对应的界面并回答。
    /// 与 <c>WinFormsUiHost</c> 是平级的两个实现，Operate 感知不到差别。
    ///
    /// 【约定】所有方法都不抛异常；前端没答、答错、超时，一律回落到安全默认值
    ///        （Confirm → false 即「什么都不做」，其余 → null）。这条约定由 AskAsync 保证。
    ///
    /// 【前端要实现的接口】
    ///   ask   confirm  { title, content, icon }   -> bool
    ///   ask   prompt   { formId, arg }            -> object | null
    ///   event notify   { level, title, content }  单向，无需回答
    ///   event toast    { level, text }            单向，无需回答
    ///   event busy     { on, text }               单向，只是显示/收起遮罩
    ///
    /// 文件框<b>不走桥</b>：直接在 C# 侧弹原生对话框（浏览器拿不到完整路径）。
    /// </summary>
    public sealed class BridgeUiHost : IUiHost
    {
        private readonly WebBridge bridge;

        /// <summary>外壳窗体，只用来当原生文件对话框的宿主。</summary>
        private readonly Form owner;

        public BridgeUiHost(WebBridge bridge, Form owner)
        {
            this.bridge = bridge;
            this.owner = owner;
        }

        #region//确认框

        public Task<bool> ConfirmAsync(string Title, string Content, UiIcon Icon = UiIcon.Warn)
        {
            return this.bridge.AskAsync<bool>("confirm", new
            {
                title = Title,
                content = Content,
                icon = (int)Icon,
            });
        }

        #endregion

        #region//通知与轻提示（单向，不等回答）

        public void Notify(UiIcon Level, string Title, string Content = null)
        {
            this.bridge.PushEvent("notify", new
            {
                level = (int)Level,
                title = Title,
                content = Content ?? string.Empty,
            });
        }

        public void Toast(UiIcon Level, string Text)
        {
            this.bridge.PushEvent("toast", new
            {
                level = (int)Level,
                text = Text,
            });
        }

        #endregion

        #region//文件选择

        /*
            文件框<b>不走桥</b>，直接在 C# 侧弹原生对话框。

            理由：浏览器里拿不到完整文件路径（<input type=file> 只给文件名与内容），
            而 Operate 要的就是路径。绕到前端再绕回来毫无收益，只多一次超时风险。
            用户看到的也是熟悉的 Windows 文件对话框，比自绘的更好用。

            与 WinFormsUiHost.PickOpenAsync / PickSaveAsync 是同一套实现，只是宿主窗体不同。
        */

        public Task<string> PickOpenAsync(FilePick Pick)
        {
            return Task.FromResult(this.OnUI(() =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ApplyPick(ofd, Pick);
                    return ofd.ShowDialog(this.owner) == DialogResult.OK ? ofd.FileName : null;
                }
            }));
        }

        public Task<string> PickSaveAsync(FilePick Pick)
        {
            return Task.FromResult(this.OnUI(() =>
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    ApplyPick(sfd, Pick);
                    return sfd.ShowDialog(this.owner) == DialogResult.OK ? sfd.FileName : null;
                }
            }));
        }

        private static void ApplyPick(FileDialog Dialog, FilePick Pick)
        {
            Dialog.RestoreDirectory = true;

            if (Pick == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(Pick.Title)) { Dialog.Title = Pick.Title; }
            if (!string.IsNullOrEmpty(Pick.Filter)) { Dialog.Filter = Pick.Filter; }
            if (!string.IsNullOrEmpty(Pick.FileName)) { Dialog.FileName = Pick.FileName; }
            if (!string.IsNullOrEmpty(Pick.InitialDir)) { Dialog.InitialDirectory = Pick.InitialDir; }
        }

        /// <summary>切回外壳的 UI 线程执行；出错返回 null。</summary>
        private string OnUI(Func<string> Work)
        {
            try
            {
                if (this.owner == null || this.owner.IsDisposed)
                {
                    return null;
                }

                if (this.owner.InvokeRequired)
                {
                    return (string)this.owner.Invoke(Work);
                }

                return Work();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnUI), ex);
                return null;
            }
        }

        #endregion

        #region//遮罩 + 后台执行

        /// <summary>
        /// 工作体在 C# 侧的后台线程跑，前端只负责显示/收起遮罩。
        /// 与 WinFormsUiHost 一样，Work 不得访问 UI、不得等 UI 线程。
        /// </summary>
        public async Task<T> BusyAsync<T>(string Text, Func<T> Work)
        {
            if (Work == null)
            {
                return default(T);
            }

            this.bridge.PushEvent("busy", new { on = true, text = Text });

            try
            {
                return await Task.Run(Work);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(BusyAsync), ex);
                return default(T);
            }
            finally
            {
                this.bridge.PushEvent("busy", new { on = false, text = string.Empty });
            }
        }

        #endregion

        #region//表单弹窗

        public Task<TResult> PromptAsync<TResult>(string FormId, object Arg) where TResult : class
        {
            return this.bridge.AskAsync<TResult>("prompt", new
            {
                formId = FormId,
                arg = Arg,
            });
        }

        #endregion
    }

    #endregion
}
