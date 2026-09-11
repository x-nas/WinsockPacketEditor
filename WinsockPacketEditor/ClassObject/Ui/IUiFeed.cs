namespace WinsockPacketEditor
{
    #region//数据通道的列表标识

    /// <summary>
    /// 界面列表的标识（B9b 引入）。
    ///
    /// 只列「界面直接绑到 Operate 全局列表」的那 19 份数据；
    /// 各编辑器内部的临时表格（滤镜格子、发送集合、指令表、仓储明细、文本对比结果等）
    /// 是组件本地状态，不走数据通道。
    /// </summary>
    public enum FeedList
    {
        #region//高频：由队列驱动，10ms 定时器批量搬运

        /// <summary>封包列表（注入模式）。</summary>
        Packet,

        /// <summary>代理数据列表（代理模式）。</summary>
        Proxy,

        /// <summary>系统日志。</summary>
        SystemLog,

        /// <summary>滤镜日志。</summary>
        FilterLog,

        /// <summary>代理日志。</summary>
        ProxyLog,

        #endregion

        #region//中频：随代理连接与账号状态变化

        /// <summary>代理账号（界面上分页显示）。</summary>
        Account,

        /// <summary>客户端认证记录。</summary>
        Auth,

        /// <summary>已选中的目标进程。</summary>
        SelectProcess,

        #endregion

        #region//低频：用户增删改时整表刷新

        Filter,
        Send,
        Robot,
        WareHouse,
        AutoStores,
        MapLocal,
        MapRemote,
        WhiteList,
        BlackList,
        Server,
        Notice,

        #endregion
    }

    #endregion

    #region//数据出口

    /// <summary>
    /// <b>Operate 向界面输出列表数据的唯一出口</b>（B9b 引入）。
    ///
    /// 【与 IUiHost 的分工】
    ///   IUiHost —— 命令：弹窗 / 通知 / 文件框 / 遮罩 / 表单（B0–B8 已完成）
    ///   IUiFeed —— 数据：列表的追加 / 替换 / 单行更新 / 清空（本接口）
    /// 两者合起来，Operate 就完全不需要认识界面了。
    ///
    /// 【为什么需要】
    /// 界面原本直接绑 <c>BindingList&lt;T&gt;</c> 静态字段，靠 WinForms 的 ListChanged 自动刷新。
    /// Vue 前端没有这个机制，必须改成「C# 推 JSON、前端维护副本」。本接口就是那条推送契约。
    ///
    /// 【实现】
    ///   BridgeUiFeed（WPEHybrid/Bridge）—— 把每次调用变成一条 JSON-RPC 事件推给前端。
    ///   未注入时是 UI.NullFeed（NeedsRows 为 false）—— 注入模式的目标进程里就是它。
    ///
    /// 【线程】可能被非 UI 线程调用，线程切换由实现方负责。
    /// </summary>
    public interface IUiFeed
    {
        /// <summary>
        /// 消费者是否真的需要 DTO 行。
        ///
        /// WinForms 外壳是进程内消费者，直接绑 <c>BindingList&lt;模型&gt;</c> 就够了，返回 false；
        /// 桥接入后返回 true。<b>Operate 在构造 DTO 之前先查这个标志</b>，
        /// 免得纯 WinForms 运行时白付每秒几千次的转换开销。
        /// </summary>
        bool NeedsRows { get; }

        /// <summary>追加一批行（高频列表走这条；Rows 是对应的 *Row DTO 数组）。</summary>
        void Append(FeedList List, object[] Rows);

        /// <summary>整表替换（低频列表增删改后重新下发全量）。</summary>
        void Replace(FeedList List, object[] Rows);

        /// <summary>单行更新（账号上下线、启用状态变化等）。</summary>
        void Update(FeedList List, object Row);

        /// <summary>
        /// 按 Id 删掉一行。
        ///
        /// 【为什么按 Id 而不是下标】增量同步真正危险的是<b>索引错位</b>：
        /// 漏掉一条，两侧就永久对不上、而且自己好不了。按 Id 删的最坏情况只是
        /// 「该走的那行没走」，下一次 Replace 就治好了，不会把它后面的所有行都错开。
        ///
        /// 【什么时候值得用】只在这份列表可能长到几万行时才值得
        /// （目前只有代理账号）。几十行的表整表 Replace 更省心，见 FeedPump 的说明。
        /// </summary>
        void Remove(FeedList List, string Id);

        /// <summary>
        /// 清空。用户点「清空」按钮、切库、以及各处 Clear* 走这条。
        ///
        /// ⚠️ <b>封包 / 代理列表的自动清理不再走这里</b>（2026-09-07 改的）——
        /// 那边改成了「只保留最近 N 条」，走 <see cref="Trim"/>。
        /// </summary>
        void Clear(FeedList List);

        /// <summary>
        /// 只留最近 <paramref name="Keep"/> 条，多出来的从<b>最旧的那头</b>丢掉。
        ///
        /// 封包 / 代理列表的自动清理走这条。原来是整表清空，2026-09-07 改成环形语义：
        /// 「自动清理 5000 条」读起来就该是「留最近 5000 条」，而不是「攒到 5000 就全没」——
        /// 后者会让列表每隔一两秒闪一次，正在看的那一屏也跟着消失。
        ///
        /// ⚠️ <b>两侧必须裁掉同样的行。</b>C# 删了前 M 条，前端也要删前 M 条 ——
        /// 内容一旦不一致，点行取字节就会拿到 null（这是 Clear 当初就有的约束，Trim 一样）。
        /// 所以这里传的是「保留多少条」而不是「删掉多少条」：调用方与接收方各自算差值，
        /// 中间丢一次事件也只会让某一拍多留几行，不会永久错位。
        /// </summary>
        void Trim(FeedList List, int Keep);
    }

    #endregion
}
