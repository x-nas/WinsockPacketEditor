using System;

namespace WinsockPacketEditor
{
    #region//钩子出口的静态门面

    /// <summary>
    /// <see cref="IHookHost"/> 的静态门面，与 <c>UI</c> 门面同一套路数。
    ///
    /// 【为什么不是可空的】<c>UI</c> 门面在未 Attach 时走安全兜底（返回 false / 静默），
    /// 这里不行 —— 钩子出口一旦静默，抓到的包就凭空消失，而且毫无报错。
    /// 所以默认值就是<b>今天的行为</b>（<see cref="InProcHookHost"/>，进程内直调），
    /// 无头核心在装配时才把它换掉。这样阶段 0 与改造前逐字节等价。
    /// </summary>
    public static class HookHost
    {
        #region//当前实现

        private static IHookHost _current = new InProcHookHost();

        /// <summary>
        /// 当前的出口实现。
        ///
        /// ⚠️ 钩子线程每次都读这个属性，所以它必须是一次字段读、不能有锁、不能有惰性初始化。
        /// </summary>
        public static IHookHost Current
        {
            get { return _current; }
        }

        /// <summary>
        /// 换掉出口实现（无头核心在装配时调一次）。
        ///
        /// 传 null 视为「回到进程内直调」，不会把出口置空。
        /// </summary>
        public static void Attach(IHookHost host)
        {
            System.Threading.Volatile.Write(ref _current, host ?? new InProcHookHost());
        }

        /// <summary>回到默认的进程内直调（外壳侧不必调用，它本来就是默认值）。</summary>
        public static void Detach()
        {
            Attach(null);
        }

        #endregion
    }

    #endregion

    #region//默认实现：进程内直调（＝改造前的行为）

    /// <summary>
    /// 今天的行为，一行逻辑都没有新增：
    /// 封包直接进 <c>ProcessingHookResultAsync</c>、仓库动作直接 <c>AddStores</c>、
    /// 日志直接写磁盘 + 内存队列、选中封包直接读 <c>piSelect</c>。
    ///
    /// WinForms 的注入模式与外壳的代理模式都用它。
    /// </summary>
    public sealed class InProcHookHost : IHookHost
    {
        #region//封包出口

        public void OnPacket(
            int socket,
            byte[] rawBuffer,
            byte[] newBuffer,
            int res,
            Operate.PacketConfig.Packet.PacketType packetType,
            Operate.FilterConfig.Filter.FilterAction filterAction,
            Operate.PacketConfig.Packet.SockAddr sockaddr,
            DateTime packetTime)
        {
            //ProcessingHookResultAsync 自己会做两道早退判断（NoModify_NoDisplay、res <= 0），
            //并且内部就是 Task.Run —— 这里保持「发射后不管」，与改造前逐字一致。
            _ = Operate.FilterConfig.Filter.ProcessingHookResultAsync(
                socket, rawBuffer, newBuffer, res, packetType, filterAction, sockaddr, packetTime);
        }

        #endregion

        #region//滤镜的仓库动作

        public void OnStore(Guid warehouseGuid, byte[] packetBuffer)
        {
            //仓库查找搬进实现里：改造后目标进程不再持有仓库，这一步本来就该在外壳做。
            WareHouseInfo whi = Operate.WareHouseConfig.WareHouse.GetWareHouse_ByGuid(warehouseGuid);

            if (whi != null)
            {
                Operate.WareHouseConfig.WareHouse.AddStores(whi.Stores, packetBuffer);
            }
        }

        #endregion

        #region//日志

        /*
            这两个刻意写成 async void，与改造前的 Operate.DoLog / DoFilterLog 逐字一致 ——
            连异常语义都一样（async void 里抛出会进 UnhandledException，而不是变成
            没人观察的 Task 异常）。阶段 0 的验收标准就是「与现状等价」，
            所以这里不顺手「改好」它。
        */
        public async void OnLog(string funcName, string content)
        {
            //2.1.9 起不再同时写 Logs\wpe.log（调试期的 LogFile 已删），日志只进内存队列
            await Operate.LogConfig.Queue.LogToQueueAsync(funcName, content);
        }

        public async void OnFilterLog(
            string filterName,
            Operate.FilterConfig.Filter.FilterAction filterAction,
            int matchNum,
            Operate.PacketConfig.Packet.PacketType packetType,
            int packetLen)
        {
            await Operate.LogConfig.Queue.FilterLogToQueueAsync(
                filterName, filterAction, matchNum, packetType, packetLen);
        }

        #endregion

        #region//封包列表当前选中的那一条

        public SelectedPacket GetSelectedPacket()
        {
            PacketInfo pi = Operate.PacketConfig.List.piSelect;

            if (pi == null)
            {
                return null;
            }

            return new SelectedPacket
            {
                Socket = pi.PacketSocket,
                PacketType = pi.PacketType,
                PacketFrom = pi.PacketFrom,
                PacketTo = pi.PacketTo,
                PacketBuffer = pi.PacketBuffer,
            };
        }

        #endregion
    }

    #endregion
}
