using System;
using System.Collections.Generic;
using System.Threading;

namespace WinsockPacketEditor.Ipc
{
    #region//滤镜引擎读的那一份表

    /// <summary>
    /// 滤镜引擎（<c>DoFilterList</c> / <c>GetFilter_ByGuid</c>）到底遍历哪一份表。
    ///
    /// 【问题】<c>lstFilterInfo</c> 是 <c>BindingList</c>，不是线程安全的。
    /// 而钩子线程随时可能在 <c>DoFilterList</c> 里遍历它 —— 用户这边一改滤镜就是
    /// Clear + N 次 Add，两者撞上就抛「集合已修改」/「下标越界」。
    /// 异常会被钩子的 try/catch 吞掉，表现成<b>换配置的那一瞬间漏几个包</b>，
    /// 没有任何报错，极难查。这个竞态<b>今天就存在</b>（注入模式的界面与钩子同进程）。
    ///
    /// 【解法】无头核心收到滤镜快照后发布一份<b>不可变数组</b>，用 <c>Volatile.Write</c> 换引用；
    /// 钩子线程每次进 DoFilterList 先 <c>Volatile.Read</c> 拿到那一份用到底，<b>全程不加锁</b>。
    /// 换配置时老数组还被上一拍的钩子线程握着，让它安全用完即可。
    ///
    /// 【为什么不顺手把 WinForms 那条路也改了】没有发布快照时这里返回
    /// <c>lstFilterInfo</c> 本身 —— 也就是<b>今天的行为，逐字未变</b>。
    /// 阶段 0 到阶段 2 的验收标准都是「与现状等价」，把这个老竞态一并修掉是另一件事，
    /// 该单独做、单独验（它会改变 WinForms 注入模式的行为）。
    /// </summary>
    public static class FilterEngine
    {
        private static FilterInfo[] _snapshot;

        /// <summary>
        /// 滤镜引擎该遍历的那一份表。
        ///
        /// ⚠️ <b>调用方必须把返回值存进局部变量再遍历</b>，不要写成
        /// <c>for (i &lt; FilterEngine.Filters.Count)</c> 加 <c>FilterEngine.Filters[i]</c> ——
        /// 那样每次都重新取一次引用，正好把「读一次用到底」这个保证给绕过去了。
        /// </summary>
        public static IList<FilterInfo> Filters
        {
            get
            {
                FilterInfo[] snap = Volatile.Read(ref _snapshot);
                return snap != null ? (IList<FilterInfo>)snap : Operate.FilterConfig.List.lstFilterInfo;
            }
        }

        /// <summary>发布一份新快照（无头核心收到 Filters 快照时调）。</summary>
        public static void PublishSnapshot(IEnumerable<FilterInfo> filters)
        {
            var arr = new List<FilterInfo>(filters).ToArray();
            Volatile.Write(ref _snapshot, arr);
        }
    }

    #endregion
}
