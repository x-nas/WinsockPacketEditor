using System;
using System.Collections.Generic;

namespace WinsockPacketEditor.Ipc
{
    #region//配置快照的序列化

    /// <summary>
    /// 外壳是配置的<b>唯一真源</b>，目标只持有一份只读快照（方案第 3.3 节）。
    ///
    /// 【为什么是整表而不是增量】与 FeedPump 推那 14 份列表同一个理由：
    /// 滤镜表是几十行量级，整推的字节可以忽略，换来的是「不用维护两侧的索引对齐」——
    /// 增量推一旦漏一条，两边就永久错位且无法自愈，而这一份错位会直接表现成
    /// 「某条滤镜在目标里没生效」，最难查的那一类。
    ///
    /// 快照是<b>幂等</b>的：丢一次下一次自愈。
    /// </summary>
    public static class ConfigSnapshot
    {
        #region//HookFlags：12 个入口开关

        /*
            ⚠️ 三个 Support_*（WS1 / WS2 / MsWS）<b>不在这里</b>。
            它们是「这个目标加载了哪几个 winsock 模块」，只有目标自己看得到 ——
            由 WpeCore.DetectWinsock 探测，随 HookState 事件报上来。
            外壳这边那三个字段是给代理模式用的，与目标无关，推下去只会把目标探出来的覆盖掉。
        */
        public static byte[] EncodeHookFlags()
        {
            var w = new IpcWriter();

            w.Bool(Operate.PacketConfig.Packet.HookWS1_Send);
            w.Bool(Operate.PacketConfig.Packet.HookWS1_SendTo);
            w.Bool(Operate.PacketConfig.Packet.HookWS1_Recv);
            w.Bool(Operate.PacketConfig.Packet.HookWS1_RecvFrom);
            w.Bool(Operate.PacketConfig.Packet.HookWS2_Send);
            w.Bool(Operate.PacketConfig.Packet.HookWS2_SendTo);
            w.Bool(Operate.PacketConfig.Packet.HookWS2_Recv);
            w.Bool(Operate.PacketConfig.Packet.HookWS2_RecvFrom);
            w.Bool(Operate.PacketConfig.Packet.HookWSA_Send);
            w.Bool(Operate.PacketConfig.Packet.HookWSA_SendTo);
            w.Bool(Operate.PacketConfig.Packet.HookWSA_Recv);
            w.Bool(Operate.PacketConfig.Packet.HookWSA_RecvFrom);

            return w.ToArray();
        }

        public static void ApplyHookFlags(byte[] payload)
        {
            var r = new IpcReader(payload);

            Operate.PacketConfig.Packet.HookWS1_Send = r.Bool();
            Operate.PacketConfig.Packet.HookWS1_SendTo = r.Bool();
            Operate.PacketConfig.Packet.HookWS1_Recv = r.Bool();
            Operate.PacketConfig.Packet.HookWS1_RecvFrom = r.Bool();
            Operate.PacketConfig.Packet.HookWS2_Send = r.Bool();
            Operate.PacketConfig.Packet.HookWS2_SendTo = r.Bool();
            Operate.PacketConfig.Packet.HookWS2_Recv = r.Bool();
            Operate.PacketConfig.Packet.HookWS2_RecvFrom = r.Bool();
            Operate.PacketConfig.Packet.HookWSA_Send = r.Bool();
            Operate.PacketConfig.Packet.HookWSA_SendTo = r.Bool();
            Operate.PacketConfig.Packet.HookWSA_Recv = r.Bool();
            Operate.PacketConfig.Packet.HookWSA_RecvFrom = r.Bool();
        }

        #endregion

        #region//Filters：滤镜列表整表

        /*
            FilterInfo 是<b>纯标量</b>的（27 个字段，没有嵌套列表），
            所以逐字段收发就够了，不需要任何序列化框架。

            ⚠️ 加字段要改<b>两处</b>（Encode 与 Apply），删字段只改一处。
            这与 FeedRows 那 18 个 DTO 是同一条契约规矩。
            对不齐的表现是「滤镜在目标里行为不对」，不会报错，所以别嫌啰嗦。

            ExecutionCount 刻意<b>不下推</b>：运行期计数归目标（方案 3.3 末），
            外壳那份是显示用的副本，推下去会把目标正在累加的值冲掉。
        */

        public static byte[] EncodeFilters()
        {
            var list = Operate.FilterConfig.List.lstFilterInfo;

            var w = new IpcWriter();
            w.I32(list.Count);

            foreach (FilterInfo fi in list)
            {
                w.Bool(fi.IsEnable);
                w.Guid_(fi.FID);
                w.Str(fi.FName);

                w.Bool(fi.AppointHeader);
                w.Str(fi.HeaderContent);
                w.Bool(fi.AppointSocket);
                w.Str(fi.SocketContent);
                w.Bool(fi.AppointLength);
                w.Str(fi.LengthContent);
                w.Bool(fi.AppointPort);
                w.Str(fi.PortContent);

                w.I32((int)fi.FMode);
                w.I32((int)fi.FAction);
                w.Bool(fi.IsExecute);
                w.I32((int)fi.FEType);
                w.Guid_(fi.Execute_GUID);

                var ff = fi.FFunction;
                w.Bool(ff.Send); w.Bool(ff.SendTo); w.Bool(ff.Recv); w.Bool(ff.RecvFrom);
                w.Bool(ff.WSASend); w.Bool(ff.WSASendTo); w.Bool(ff.WSARecv); w.Bool(ff.WSARecvFrom);
                w.Bool(ff.TCP_Req); w.Bool(ff.UDP_Req); w.Bool(ff.TCP_Resp); w.Bool(ff.UDP_Resp);

                w.I32((int)fi.FStartFrom);
                w.Bool(fi.IsProgressionDone);
                w.Bool(fi.IsProgressionContinuous);
                w.I32(fi.ProgressionStep);
                w.Bool(fi.IsProgressionCarry);
                w.I32(fi.ProgressionCarryNumber);
                w.Str(fi.ProgressionPosition);
                w.I32(fi.ProgressionCount);
                w.Str(fi.ExcludePosition);
                w.Str(fi.RandomPosition);
                w.Str(fi.FSearch);
                w.Str(fi.FModify);
            }

            return w.ToArray();
        }

        /// <summary>
        /// 目标侧应用滤镜快照。
        ///
        /// 【计数按 GUID 迁移】换快照时把旧表里同 GUID 的 ExecutionCount 带过来，
        /// 否则用户在外壳上编辑一条滤镜，整表计数就被清零了 —— 那是运行的副产品，
        /// 只有目标知道真值。
        /// </summary>
        public static void ApplyFilters(byte[] payload)
        {
            var r = new IpcReader(payload);
            int n = r.I32();

            //先把旧计数按 GUID 存下来
            var oldCounts = new Dictionary<Guid, long>();
            foreach (FilterInfo old in Operate.FilterConfig.List.lstFilterInfo)
            {
                oldCounts[old.FID] = old.ExecutionCount;
            }

            var fresh = new List<FilterInfo>(n);

            for (int i = 0; i < n; i++)
            {
                /*
                    ⚠️ FilterInfo <b>没有无参构造</b>，只能走那个 30 参的完整构造。
                    所以字段得先读进局部变量 —— 而 C# 的实参求值顺序虽然是从左到右、
                    直接把 r.Bool() 写进参数表也能对，但那样一眼看不出顺序对没对，
                    而顺序错了不会报错、只会让滤镜行为莫名其妙。摊开写。
                */
                bool isEnable = r.Bool();
                Guid fid = r.Guid_();
                string fname = r.Str();

                bool appointHeader = r.Bool();
                string headerContent = r.Str();
                bool appointSocket = r.Bool();
                string socketContent = r.Str();
                bool appointLength = r.Bool();
                string lengthContent = r.Str();
                bool appointPort = r.Bool();
                string portContent = r.Str();

                var fmode = (Operate.FilterConfig.Filter.FilterMode)r.I32();
                var faction = (Operate.FilterConfig.Filter.FilterAction)r.I32();
                bool isExecute = r.Bool();
                var fetype = (Operate.FilterConfig.Filter.FilterExecuteType)r.I32();
                Guid executeGuid = r.Guid_();

                var ff = new Operate.FilterConfig.Filter.FilterFunction();
                ff.Send = r.Bool(); ff.SendTo = r.Bool(); ff.Recv = r.Bool(); ff.RecvFrom = r.Bool();
                ff.WSASend = r.Bool(); ff.WSASendTo = r.Bool(); ff.WSARecv = r.Bool(); ff.WSARecvFrom = r.Bool();
                ff.TCP_Req = r.Bool(); ff.UDP_Req = r.Bool(); ff.TCP_Resp = r.Bool(); ff.UDP_Resp = r.Bool();

                var fstartFrom = (Operate.FilterConfig.Filter.FilterStartFrom)r.I32();
                bool progDone = r.Bool();
                bool progContinuous = r.Bool();
                int progStep = r.I32();
                bool progCarry = r.Bool();
                int progCarryNumber = r.I32();
                string progPosition = r.Str();
                int progCount = r.I32();
                string excludePosition = r.Str();
                string randomPosition = r.Str();
                string fsearch = r.Str();
                string fmodify = r.Str();

                var fi = new FilterInfo(
                    isEnable, fid, fname,
                    appointHeader, headerContent,
                    appointSocket, socketContent,
                    appointLength, lengthContent,
                    appointPort, portContent,
                    fmode, faction, isExecute, fetype, executeGuid,
                    ff, fstartFrom,
                    progDone, progContinuous, progStep, progCarry, progCarryNumber,
                    progPosition, progCount,
                    excludePosition, randomPosition,
                    fsearch, fmodify);

                long keep;
                if (oldCounts.TryGetValue(fid, out keep)) { fi.ExecutionCount = keep; }

                fresh.Add(fi);
            }

            /*
                ⚠️ 这里是<b>整表替换</b>，而钩子线程随时可能在 DoFilterList 里遍历它。
                BindingList 不是线程安全的，边遍历边 Clear + Add 会抛
                「集合已修改」——而且是在钩子线程上抛，会被钩子的 try/catch 吞掉，
                表现成「换配置的那一瞬间漏几个包」。

                目标侧的 DoFilterList 走的是 FilterEngine.Snapshot（不可变数组 + Volatile 替换），
                这里只是把 BindingList 也同步过去，供别的地方（统计、日志）读。
            */
            FilterEngine.PublishSnapshot(fresh);

            var bl = Operate.FilterConfig.List.lstFilterInfo;
            bl.Clear();
            foreach (FilterInfo fi in fresh) { bl.Add(fi); }
        }

        #endregion

        #region//Runtime：极速模式 / 系统套接字 / 执行方式 / 当前选中封包

        public static byte[] EncodeRuntime()
        {
            var w = new IpcWriter();

            w.Bool(Operate.SystemConfig.SpeedMode);
            w.I32(Operate.SystemConfig.SystemSocket);
            w.I32((int)Operate.SystemConfig.ListExecute);

            //当前选中的封包：机器人指令「设置系统套接字 → 封包列表」与
            //「发送封包列表选中的封包」要读它。目标不再持有封包列表。
            SelectedPacket sp = HookHost.Current.GetSelectedPacket();

            if (sp == null)
            {
                w.Bool(false);
            }
            else
            {
                w.Bool(true);
                w.I32(sp.Socket);
                w.I32((int)sp.PacketType);
                w.Str(sp.PacketFrom);
                w.Str(sp.PacketTo);
                w.Bytes(sp.PacketBuffer);
            }

            return w.ToArray();
        }

        public static void ApplyRuntime(byte[] payload)
        {
            var r = new IpcReader(payload);

            Operate.SystemConfig.SpeedMode = r.Bool();
            Operate.SystemConfig.SystemSocket = r.I32();
            Operate.SystemConfig.ListExecute = (Operate.SystemConfig.Execute)r.I32();

            if (!r.Bool())
            {
                WpeCore.SelectedPacketSnapshot = null;
                return;
            }

            WpeCore.SelectedPacketSnapshot = new SelectedPacket
            {
                Socket = r.I32(),
                PacketType = (Operate.PacketConfig.Packet.PacketType)r.I32(),
                PacketFrom = r.Str(),
                PacketTo = r.Str(),
                PacketBuffer = r.Bytes(),
            };
        }

        #endregion
    }

    #endregion
}
