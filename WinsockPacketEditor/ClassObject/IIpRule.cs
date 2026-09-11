using System;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 白名单与黑名单在<b>防火墙判定</b>时的共同面。
    ///
    /// <para>
    /// ⚠️ <b>这个接口只为判定而存在，不是为了统一那两个模型。</b>
    /// <c>WhiteListInfo</c> 与 <c>BlackListInfo</c> 至今逐字相同（除类名），但它们各自落一张表、
    /// 各自有一套 XML 与 DTO —— 合并要动数据库、备份格式与桥契约，风险远大于收益。
    /// 这里只把判定用得着的那几个成员抽出来，好让 <c>IsIpInRanges</c> 只写一份。
    /// </para>
    /// </summary>
    public interface IIpRule
    {
        /// <summary>这条规则覆盖这个 IP 吗（IPv4 转成的 long）。</summary>
        bool ContainsIp(long ipValue);

        /// <summary>这条规则设了过期时间吗。</summary>
        bool IsExpiry { get; }

        /// <summary>过期时刻（<c>IsExpiry</c> 为 false 时无意义）。</summary>
        DateTime ExpiryTime { get; }

        /// <summary>
        /// 命中一次。
        ///
        /// <para>
        /// ⚠️ <b>由 SuperSocket 的连接线程调用</b>，所以内部必须是 <c>Interlocked</c>，
        /// 而且<b>不能触发 OnPropertyChanged</b> —— 那会经 <c>BindingList.ItemChanged</c>
        /// 冒到 WinForms 的表格上，<b>跨线程改绑定列表是崩溃路径</b>。
        /// 界面靠 1 秒统计拍统一标脏（见 <c>Operate.ProxyConfig.Proxy.RefreshStatInfo</c>）。
        /// </para>
        /// </summary>
        void HitOnce();
    }
}
