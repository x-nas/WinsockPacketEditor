using System;
using WinsockPacketEditor;

namespace WPEHybrid
{
    #region//桥侧的数据出口实现

    /// <summary>
    /// <see cref="IUiFeed"/> 的桥实现（B10c）。
    ///
    /// 与 <c>WinFormsUiFeed</c> 的根本差别是 <see cref="NeedsRows"/> 返回 <b>true</b>：
    /// WinForms 是进程内消费者，直接绑 BindingList 就行；前端在另一侧，必须拿到 DTO。
    /// 这个标志一翻，B9c 埋在 Operate.FlushToFeed 里的
    /// <c>if (UI.Feed.NeedsRows) rows[i] = PacketRow.From_(...)</c> 立刻生效。
    ///
    /// 【推送的事件名】前端 on(name) 即可
    ///   feed:append   { list, rows }   追加一批（高频：封包 / 代理 / 三种日志）
    ///   feed:replace  { list, rows }   整表替换（低频列表增删改）
    ///   feed:update   { list, row }    单行更新
    ///   feed:clear    { list }         清空（自动清理时也走这条，见 B9c）
    ///
    /// list 是 <see cref="FeedList"/> 的 int 值，前端按 int 分支，不依赖 C# 枚举名。
    ///
    /// 【线程】Operate 的定时器在 UI 线程调这些方法，但 WebBridge.PushEvent 内部
    /// 已经做了到 UI 线程的切换，后台线程调用同样安全。
    /// </summary>
    public sealed class BridgeUiFeed : IUiFeed
    {
        private readonly WebBridge bridge;

        public BridgeUiFeed(WebBridge bridge)
        {
            this.bridge = bridge;
        }

        /// <summary>前端在另一侧，必须拿 DTO。</summary>
        public bool NeedsRows
        {
            get { return true; }
        }

        public void Append(FeedList List, object[] Rows)
        {
            if (Rows == null || Rows.Length == 0)
            {
                return;
            }

            this.Push("feed:append", new { list = (int)List, rows = Rows });
        }

        public void Replace(FeedList List, object[] Rows)
        {
            this.Push("feed:replace", new { list = (int)List, rows = Rows ?? new object[0] });
        }

        public void Update(FeedList List, object Row)
        {
            if (Row == null)
            {
                return;
            }

            this.Push("feed:update", new { list = (int)List, row = Row });
        }

        public void Remove(FeedList List, string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return;
            }

            this.Push("feed:remove", new { list = (int)List, id = Id });
        }

        public void Clear(FeedList List)
        {
            this.Push("feed:clear", new { list = (int)List });
        }

        public void Trim(FeedList List, int Keep)
        {
            this.Push("feed:trim", new { list = (int)List, keep = Keep });
        }

        private void Push(string Name, object Data)
        {
            try
            {
                this.bridge.PushEvent(Name, Data);
            }
            catch (Exception ex)
            {
                //推送失败不能影响抓包主流程
                Operate.DoLog(nameof(Push), ex);
            }
        }
    }

    #endregion
}
