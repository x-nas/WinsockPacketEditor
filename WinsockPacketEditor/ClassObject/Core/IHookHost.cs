using System;

namespace WinsockPacketEditor
{
    #region//钩子核心的出口抽象

    /// <summary>
    /// 目标进程侧（钩子 + 滤镜引擎 + 执行器）与「数据的归宿」之间的唯一接缝。
    ///
    /// 今天注入模式整个活在目标进程里，钩子处理完一个包就直接往 <c>cqPacketInfo</c> 里塞、
    /// 滤镜的仓库动作直接改 <c>WareHouseInfo.Stores</c>、日志直接写文件与内存队列 ——
    /// 这些「归宿」在 IPC 改造后全部搬到外壳进程，目标侧只剩「把事实送出去」。
    ///
    /// 这个接口就是那条分界线。默认实现 <see cref="InProcHookHost"/> 保持今天的行为
    /// （进程内直调），IPC 改造的阶段 1 再并排长出一个走命名管道的实现。
    ///
    /// 【零 UI 依赖】不 using AntdUI，也不 using System.Windows.Forms ——
    /// 与 <c>ClassObject/Ui/</c> 那一层同一条规矩，理由也一样：
    /// 将来它要被一个没有界面、也没有数据库的无头核心消费。
    /// </summary>
    public interface IHookHost
    {
        #region//封包出口

        /// <summary>
        /// 钩子处理完一个封包之后的唯一出口，取代原先对
        /// <c>FilterConfig.Filter.ProcessingHookResultAsync</c> 的直接调用。
        ///
        /// ⚠️ <b>这是钩子线程</b>（目标进程的收发线程）。实现里只允许做「拷贝 + 一次入队」，
        /// 不得写 I/O、不得等锁、不得查 IP 归属地、不得格式化字符串。
        /// </summary>
        /// <param name="socket">目标进程的 SOCKET 句柄。</param>
        /// <param name="rawBuffer">滤镜改写<b>前</b>的字节。</param>
        /// <param name="newBuffer">滤镜改写<b>后</b>的字节。</param>
        /// <param name="res">API 的实际返回值（发出 / 收到的字节数）。</param>
        /// <param name="packetType">封包类型（13 个 API 各自的枚举值）。</param>
        /// <param name="filterAction">滤镜给出的动作。</param>
        /// <param name="sockaddr">sendto / recvfrom 带的地址；其余类型传空结构。</param>
        /// <param name="packetTime">目标侧时间，保证与钩子顺序一致。</param>
        void OnPacket(
            int socket,
            byte[] rawBuffer,
            byte[] newBuffer,
            int res,
            Operate.PacketConfig.Packet.PacketType packetType,
            Operate.FilterConfig.Filter.FilterAction filterAction,
            Operate.PacketConfig.Packet.SockAddr sockaddr,
            DateTime packetTime);

        #endregion

        #region//滤镜的仓库动作

        /// <summary>
        /// 滤镜命中后执行「入库」动作，取代 <c>DoFilter</c> 里对 <c>AddStores</c> 的直接调用。
        ///
        /// 改造后目标进程不再持有仓库（仓库连同 SQLite 一起在外壳），
        /// 所以这里只把「哪个仓库 + 哪段字节」送出去，落库由外壳做。
        /// </summary>
        void OnStore(Guid warehouseGuid, byte[] packetBuffer);

        #endregion

        #region//日志

        /// <summary>运行日志。默认实现 = 写磁盘 + 进内存队列；无头实现 = 发事件。</summary>
        void OnLog(string funcName, string content);

        /// <summary>滤镜日志。极速模式下调用方本来就不会走到这里。</summary>
        void OnFilterLog(
            string filterName,
            Operate.FilterConfig.Filter.FilterAction filterAction,
            int matchNum,
            Operate.PacketConfig.Packet.PacketType packetType,
            int packetLen);

        #endregion

        #region//封包列表当前选中的那一条

        /// <summary>
        /// 取「封包列表里当前选中的那一条」，取代对 <c>PacketConfig.List.piSelect</c> 的直接读。
        ///
        /// 两个消费方：机器人指令「设置系统套接字 → 封包列表」与「发送封包列表选中的封包」。
        /// 改造后封包列表在外壳，目标侧读的是随 Runtime 快照下推的那一份副本。
        /// 没有选中时返回 null。
        /// </summary>
        SelectedPacket GetSelectedPacket();

        #endregion
    }

    #endregion

    #region//选中封包的快照

    /// <summary>
    /// <see cref="IHookHost.GetSelectedPacket"/> 的返回值。
    ///
    /// 刻意不直接出 <c>PacketInfo</c>：那个模型继承 AntdUI 的 <c>NotifyProperty</c>，
    /// 一出现在无头核心的签名上就是 CS0012（这个坑已经撞过 15 次）。
    /// 这里只出基础类型。
    /// </summary>
    public sealed class SelectedPacket
    {
        /// <summary>目标进程的 SOCKET 句柄。</summary>
        public int Socket { get; set; }

        /// <summary>封包类型，决定 <c>SendPacket</c> 走 send 还是 sendto、取 From 还是 To。</summary>
        public Operate.PacketConfig.Packet.PacketType PacketType { get; set; }

        /// <summary>本机地址（"1.2.3.4:5678"）。UDP 响应类要用它，见 SendExecute 的那个老 bug。</summary>
        public string PacketFrom { get; set; }

        /// <summary>远端地址（"1.2.3.4:5678"）。</summary>
        public string PacketTo { get; set; }

        /// <summary>封包字节（滤镜改写后的那一份）。</summary>
        public byte[] PacketBuffer { get; set; }
    }

    #endregion
}
