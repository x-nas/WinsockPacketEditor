using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WinsockPacketEditor
{
    #region//中低频列表的数据泵

    /// <summary>
    /// 把 14 份中低频列表推给桥（B9d）。
    ///
    /// 高频那 5 份（封包 / 代理 / 三种日志）走的是队列 + <c>FlushToFeed</c> 的批量 <c>Append</c>，
    /// 不归本类管。本类只管「用户点一下才变一次」的那些列表。
    ///
    /// 【为什么订阅 ListChanged，而不是在每个改动点插一行推送】
    /// 实测这 14 份列表的改动点有 140 多处，而且<b>并不都在 Operate 里</b>：
    /// <c>lstSelectProcessName</c> 在 Operate 里是 0 处、全在界面层改，<c>lstAuthInfo</c> 也有一处在界面。
    /// 逐点插桩必然会漏，漏了的表现是「界面上删掉了但 Vue 那边还在」——很难查。
    /// 订阅 <see cref="BindingList{T}.ListChanged"/> 则是一处覆盖全部：
    /// Add / Remove / Insert / Clear 都会触发，无论谁调的。
    ///
    /// 【为什么还需要 PushNow】
    /// 编辑弹窗是<b>就地改对象的属性</b>，不动列表结构，所以一个事件都不会有
    /// （这些模型都没实现 INotifyPropertyChanged，全项目 ResetBindings 调用为 0 —— WinForms 侧
    /// 靠 AntdUI 表格自己重绘，桥没有这份便利，因为它持有的是另一侧的副本）。
    /// 所以 <c>UiDialogs.OpenEditModal</c> 在弹窗关掉后会调一次 <see cref="PushNow"/>。
    ///
    /// 【为什么要攒一拍再推】
    /// 从数据库载入是 Clear + N 次 Add，会连着来 N+1 个事件；逐个事件整表推就是 O(N²)。
    /// 所以事件只打脏标记，由外壳的搬运定时器每拍调一次 <see cref="FlushDirty"/>。
    ///
    /// 【线程】列表本身只在 UI 线程改（WinForms 的 BindingList 也只能这么用），
    /// 但脏标记仍加锁 —— 代价可忽略，省得将来有人从后台线程改列表时出现难查的丢更新。
    /// </summary>
    public static class FeedPump
    {
        #region//字段

        private static readonly HashSet<FeedList> Dirty = new HashSet<FeedList>();

        /// <summary>被 <see cref="Suppress"/> 静音的列表与嵌套层数。</summary>
        private static readonly Dictionary<FeedList, int> Muted = new Dictionary<FeedList, int>();

        private static readonly object Gate = new object();

        private static bool attached;

        #endregion

        #region//接线

        /// <summary>
        /// 订阅 14 份列表的变更。由外壳在 <c>UI.AttachFeed</c> 之后调一次。
        ///
        /// 纯 WinForms 运行时<b>不该调</b>：那边界面直接绑 BindingList，
        /// 推送是白付代价。真调了也没事，<see cref="IUiFeed.NeedsRows"/> 为 false 时直接返回。
        /// </summary>
        public static void Attach()
        {
            if (attached || !UI.Feed.NeedsRows)
            {
                return;
            }

            attached = true;

            Hook(Operate.ProxyConfig.Proxy.lstSelectProcessName, FeedList.SelectProcess);
            Hook(Operate.ProxyConfig.Proxy.lstWhiteList, FeedList.WhiteList);
            Hook(Operate.ProxyConfig.Proxy.lstBlackList, FeedList.BlackList);
            Hook(Operate.ProxyConfig.Account.lstAccountInfo, FeedList.Account);
            Hook(Operate.ProxyConfig.Account.lstAuthInfo, FeedList.Auth);
            Hook(Operate.ProxyConfig.Mapping.lstMapLocal, FeedList.MapLocal);
            Hook(Operate.ProxyConfig.Mapping.lstMapRemote, FeedList.MapRemote);
            Hook(Operate.FilterConfig.List.lstFilterInfo, FeedList.Filter);
            Hook(Operate.SendConfig.List.lstSendInfo, FeedList.Send);
            Hook(Operate.RobotConfig.List.lstRobotInfo, FeedList.Robot);
            Hook(Operate.WareHouseConfig.List.lstWareHouseInfo, FeedList.WareHouse);
            Hook(Operate.WareHouseConfig.List.lstAutoStoresInfo, FeedList.AutoStores);
            Hook(Operate.WPCConfig.ServerList.lstServerInfo, FeedList.Server);
            Hook(Operate.WPCConfig.NoticeList.lstNoticeInfo, FeedList.Notice);

            //首次全量推一遍：接线时列表里通常已经有从数据库载入的数据了
            lock (Gate)
            {
                foreach (FeedList l in AllLists) { Dirty.Add(l); }
            }
        }

        private static void Hook<T>(BindingList<T> List, FeedList Which)
        {
            if (List == null)
            {
                return;
            }

            List.ListChanged += (s, e) => MarkDirty(Which);
        }

        private static readonly FeedList[] AllLists =
        {
            FeedList.SelectProcess, FeedList.WhiteList, FeedList.BlackList,
            FeedList.Account, FeedList.Auth,
            FeedList.MapLocal, FeedList.MapRemote,
            FeedList.Filter, FeedList.Send, FeedList.Robot,
            FeedList.WareHouse, FeedList.AutoStores,
            FeedList.Server, FeedList.Notice,
        };

        #endregion

        #region//推送

        /// <summary>打个脏标记，等下一拍统一推。</summary>
        public static void MarkDirty(FeedList Which)
        {
            if (!UI.Feed.NeedsRows)
            {
                return;
            }

            lock (Gate)
            {
                //被 Suppress 括起来的那段：调用方自己会推增量，别再整表推一次盖掉
                int n;
                if (Muted.TryGetValue(Which, out n) && n > 0) { return; }

                Dirty.Add(Which);
            }
        }

        /// <summary>
        /// 让这一段里对该列表的 ListChanged <b>不标脏</b>。用 <c>using</c> 括住那次改动，
        /// 异常路径也能还原。
        ///
        /// 【只在一处用，别推广】整表 Replace 是本类刻意选的默认，理由见 <c>BuildRows</c>：
        /// 那 14 份表都是几十到几百行，整表推换来的是「两侧内容不可能对不上」。
        /// 唯一的例外是<b>代理账号</b> —— 它是拿来卖的，几万个是真实规模，
        /// 那时一次整表推是几 MB 的 JSON，而用户改的往往只是一行。
        ///
        /// 【用它就必须自己推】括起来之后前端不会收到任何东西，
        /// 调用方要在同一个方法里补上 <c>UI.Feed.Append / Update / Remove</c>。
        /// 漏了的表现是「C# 侧变了、界面上没变」，而且要等下一次整表推才会好。
        /// 所以配对的那两句务必写在一起，不要跨方法。
        /// </summary>
        public static IDisposable Suppress(FeedList Which)
        {
            lock (Gate)
            {
                int n;
                Muted.TryGetValue(Which, out n);
                Muted[Which] = n + 1;
            }

            return new Mute(Which);
        }

        private sealed class Mute : IDisposable
        {
            private readonly FeedList which;
            private bool done;

            public Mute(FeedList Which)
            {
                this.which = Which;
            }

            public void Dispose()
            {
                if (this.done) { return; }
                this.done = true;

                lock (Gate)
                {
                    int n;
                    if (Muted.TryGetValue(this.which, out n) && n > 0)
                    {
                        Muted[this.which] = n - 1;
                    }
                }
            }
        }

        /// <summary>
        /// 立刻整表推一次。给编辑弹窗用 —— 就地改属性不会有任何列表事件。
        /// </summary>
        public static void PushNow(FeedList Which)
        {
            if (!UI.Feed.NeedsRows)
            {
                return;
            }

            lock (Gate) { Dirty.Remove(Which); }
            Push(Which);
        }

        /// <summary>
        /// 把 14 份列表全部标脏，下一拍整体推一次。
        ///
        /// 【什么时候需要】前端<b>重新挂载</b>的时候 —— 比如从代理模式退回启动页再进来。
        /// 那边的副本随组件一起没了，而这边的 BindingList 没有任何变化，
        /// 不会触发 ListChanged，也就不会有人再推一次，界面上就是一片空。
        /// 从数据库加载完之后同理：加载本身会触发事件，但前端可能还没订阅上。
        /// </summary>
        public static void MarkAllDirty()
        {
            if (!UI.Feed.NeedsRows)
            {
                return;
            }

            lock (Gate)
            {
                foreach (FeedList which in AllLists)
                {
                    Dirty.Add(which);
                }
            }
        }

        /// <summary>
        /// 把这一拍攒下的脏列表推出去。由外壳的搬运定时器调用，
        /// 与 <c>PacketConfig.List.FlushToFeed</c> 那三条并列。
        /// </summary>
        public static void FlushDirty()
        {
            if (!UI.Feed.NeedsRows)
            {
                return;
            }

            FeedList[] todo;

            lock (Gate)
            {
                if (Dirty.Count == 0) { return; }

                todo = new FeedList[Dirty.Count];
                Dirty.CopyTo(todo);
                Dirty.Clear();
            }

            for (int i = 0; i < todo.Length; i++)
            {
                Push(todo[i]);
            }
        }

        private static void Push(FeedList Which)
        {
            try
            {
                UI.Feed.Replace(Which, BuildRows(Which));
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Push), ex);
            }
        }

        #endregion

        #region//行构造

        /// <summary>
        /// 整表转 DTO。
        ///
        /// 这里刻意<b>整表推</b>而不是按行增删：这 14 份列表都是几十到几百行、用户点一下才变一次，
        /// 整表推的代价可忽略，换来的是「两侧内容不可能对不上」——
        /// 按行增删则要前端也维护一份等价的顺序与索引，多一处能错的地方。
        /// 高频那 5 份才值得为增量付这个复杂度，它们走的是 Append。
        /// </summary>
        private static object[] BuildRows(FeedList Which)
        {
            switch (Which)
            {
                case FeedList.SelectProcess:
                    return Map(Operate.ProxyConfig.Proxy.lstSelectProcessName, x => ProcessRow.From_(x));

                case FeedList.WhiteList:
                    return Map(Operate.ProxyConfig.Proxy.lstWhiteList, x => IPRuleRow.From_(x));

                case FeedList.BlackList:
                    return Map(Operate.ProxyConfig.Proxy.lstBlackList, x => IPRuleRow.From_(x));

                case FeedList.Account:
                    return Map(Operate.ProxyConfig.Account.lstAccountInfo, x => AccountRow.From_(x));

                case FeedList.Auth:
                    return Map(Operate.ProxyConfig.Account.lstAuthInfo, x => AuthRow.From_(x));

                case FeedList.MapLocal:
                    return Map(Operate.ProxyConfig.Mapping.lstMapLocal, x => MapLocalRow.From_(x));

                case FeedList.MapRemote:
                    return Map(Operate.ProxyConfig.Mapping.lstMapRemote, x => MapRemoteRow.From_(x));

                case FeedList.Filter:
                    return Map(Operate.FilterConfig.List.lstFilterInfo, x => FilterRow.From_(x));

                case FeedList.Send:
                    return Map(Operate.SendConfig.List.lstSendInfo, x => SendRow.From_(x));

                case FeedList.Robot:
                    return Map(Operate.RobotConfig.List.lstRobotInfo, x => RobotRow.From_(x));

                case FeedList.WareHouse:
                    return Map(Operate.WareHouseConfig.List.lstWareHouseInfo, x => WareHouseRow.From_(x));

                case FeedList.AutoStores:
                    return Map(Operate.WareHouseConfig.List.lstAutoStoresInfo, x => AutoStoresRow.From_(x));

                case FeedList.Server:
                    return Map(Operate.WPCConfig.ServerList.lstServerInfo, x => ServerRow.From_(x));

                case FeedList.Notice:
                    return Map(Operate.WPCConfig.NoticeList.lstNoticeInfo, x => NoticeRow.From_(x));

                default:
                    //高频那 5 份不该走到这里 —— 它们由 FlushToFeed 批量 Append
                    Operate.DoLog(nameof(BuildRows), "不该由 FeedPump 推送的列表: " + Which);
                    return new object[0];
            }
        }

        private static object[] Map<T>(BindingList<T> Src, Func<T, object> Conv)
        {
            if (Src == null)
            {
                return new object[0];
            }

            //取一次快照再转换：转换过程中列表若被改动，直接遍历会抛
            T[] snap = new T[Src.Count];
            Src.CopyTo(snap, 0);

            object[] rows = new object[snap.Length];

            for (int i = 0; i < snap.Length; i++)
            {
                rows[i] = Conv(snap[i]);
            }

            return rows;
        }

        #endregion
    }

    #endregion
}
