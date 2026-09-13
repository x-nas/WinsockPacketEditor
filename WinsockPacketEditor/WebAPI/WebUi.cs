using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 远程管理的 HTTP 请求跑在 HttpListener 的线程池线程上，而它要读写的那几份列表都是<b>界面线程的</b>：
    ///   · 三份日志 —— 界面线程上的搬运拍（FlushToFeed，10ms 一拍）在往里加、环形裁剪；
    ///   · 认证列表 —— RefreshAuthList 每秒在界面线程上<b>整表清空再重填</b>；
    ///   · 账号列表 —— 程序界面在界面线程上增删改，FeedPump 在界面线程上遍历它整表推给前端，
    ///     SOCKS5 认证线程也在遍历它。
    ///
    /// 2026-09-11 之前 HTTP 线程直接碰这些列表：读的时候撞上「集合已修改」返回 500
    /// （实测一边加认证记录一边请求，6375 次失败 6335 次），写的时候反过来把界面线程上的遍历撞坏。
    ///
    /// 所以一律<b>切到界面线程上取一份拷贝 / 做那一次改动</b>，序列化在 HTTP 线程上对拷贝做。
    /// 走的是程序已有的 <see cref="Operate.SystemConfig.InvokeAction"/>（同步 Invoke）；
    /// 没有界面的时候（跑测、注入模式的目标进程）它是 null，直接在当前线程做。
    /// </summary>
    internal static class WebUi
    {
        public static T OnUi<T>(Func<T> Work)
        {
            Action<Action> invoke = Operate.SystemConfig.InvokeAction;
            if (invoke == null) { return Work(); }

            T result = default(T);
            Exception error = null;

            invoke(() =>
            {
                try { result = Work(); }
                catch (Exception ex) { error = ex; }
            });

            //在界面线程上抛的异常带回 HTTP 线程再抛，Web API 按 500 处理，堆栈原样保留
            if (error != null) { ExceptionDispatchInfo.Capture(error).Throw(); }

            return result;
        }

        /// <summary>
        /// 取列表<b>末尾</b>的 take 条（也就是最近的那些）；take &lt;= 0 取整份。<b>一律返回拷贝</b>。
        ///
        /// ⚠️ 必须在界面线程上调（包在 <see cref="OnUi{T}"/> 里）。
        /// 原来的写法在「take &lt;= 0」和「take &gt;= Count」两种情况下原样返回那份活的列表 ——
        /// 而管理台每次请求 take=300、列表不满 300 条时走的正是这一支，序列化时撞上别人在改就是 500。
        /// </summary>
        public static List<T> Tail<T>(IList<T> List, int Take)
        {
            if (List == null) { return new List<T>(); }

            int count = List.Count;
            int from = Take <= 0 || Take >= count ? 0 : count - Take;

            List<T> copy = new List<T>(count - from);
            for (int i = from; i < count; i++) { copy.Add(List[i]); }
            return copy;
        }
    }
}
