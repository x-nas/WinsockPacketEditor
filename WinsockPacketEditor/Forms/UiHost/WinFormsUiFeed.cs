using System;

namespace WinsockPacketEditor
{
    #region//WinForms 侧的数据出口实现

    /// <summary>
    /// IUiFeed 的 WinForms 实现（B9b 引入）。
    ///
    /// 【为什么它几乎什么都不做】
    /// WinForms 外壳是<b>进程内</b>消费者：各个表格直接绑 Operate 里的
    /// <c>BindingList&lt;模型&gt;</c>，靠 ListChanged 自动刷新，不需要 DTO 中转。
    /// 所以 <see cref="NeedsRows"/> 返回 false，Operate 会跳过 DTO 构造
    /// —— 纯 WinForms 运行时不会为「将来要接桥」白付每秒几千次的转换开销。
    ///
    /// DTO 通道（FeedRows.cs）是给<b>桥</b>准备的：将来的 BridgeUiFeed 返回 true，
    /// 把每次 Append/Replace/Update/Clear 变成一条 JSON-RPC 事件推给 Vue 前端。
    ///
    /// 【那它存在的意义是什么】
    ///   1. 让 UI.Feed 永不为 null，Operate 不必到处判空
    ///   2. <see cref="Cleared"/> 事件：列表被自动清理时通知界面收拾关联面板
    ///      （封包列表清空时右侧十六进制面板也要清）。迁移前这件事写在
    ///      PacketList 的定时器里，属于「UI 自己看着 Operate 的列表长度做决定」；
    ///      现在改由 Operate 发信号，两个消费者都能收到。
    /// </summary>
    public sealed class WinFormsUiFeed : IUiFeed
    {
        /// <summary>全局唯一实例。控件用它订阅 <see cref="Cleared"/>。</summary>
        public static readonly WinFormsUiFeed Instance = new WinFormsUiFeed();

        /// <summary>WinForms 直接绑模型列表，不需要 DTO。</summary>
        public bool NeedsRows
        {
            get { return false; }
        }

        /// <summary>
        /// 某个列表被清空时触发。
        ///
        /// 界面订阅它来清理<b>关联面板</b>——列表本身是 BindingList，清空会自动反映到表格，
        /// 但右侧的十六进制/文本面板显示的是「当前选中行」的内容，没人通知就会留着废数据。
        /// 迁移前这件事写在 PacketList / ProxyList 的定时器里（UI 自己盯着列表长度判断），
        /// 现在改由 Operate 在裁剪时发信号，将来的桥消费者也能收到同一个事件。
        ///
        /// 订阅方记得在控件释放时退订（用 Disposed 事件即可，不必改 Designer）。
        /// </summary>
        public static event Action<FeedList> Cleared;

        public void Append(FeedList List, object[] Rows)
        {
            //空实现：WinForms 侧的追加已经由 Operate 直接写进 BindingList 完成
        }

        public void Replace(FeedList List, object[] Rows)
        {
            //空实现：同上
        }

        public void Update(FeedList List, object Row)
        {
            //空实现：模型实现了 INotifyPropertyChanged，改字段界面自己会刷新
        }

        public void Remove(FeedList List, string Id)
        {
            //空实现：行已经从 BindingList 里摘掉了，ListChanged 会让表格自己刷新
        }

        public void Clear(FeedList List)
        {
            try
            {
                Action<FeedList> h = Cleared;

                if (h != null)
                {
                    h(List);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Clear), ex);
            }
        }
    }

    #endregion
}
