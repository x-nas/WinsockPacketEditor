using System;
using System.ComponentModel;
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
            var oldCounts = new Dictionary<Guid, FilterInfo>();
            foreach (FilterInfo old in Operate.FilterConfig.List.lstFilterInfo)
            {
                oldCounts[old.FID] = old;
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

                FilterInfo keep;
                if (oldCounts.TryGetValue(fid, out keep))
                {
                    fi.State = keep.State;
                    if (keep.ProgressionPosition != fi.ProgressionPosition ||
                        keep.ProgressionStep != fi.ProgressionStep ||
                        keep.IsProgressionContinuous != fi.IsProgressionContinuous ||
                        keep.IsProgressionCarry != fi.IsProgressionCarry ||
                        keep.ProgressionCarryNumber != fi.ProgressionCarryNumber)
                    {
                        lock (fi.State)
                        {
                            fi.ProgressionCount = 0;
                            fi.IsProgressionDone = false;
                        }
                    }
                }

                Operate.FilterConfig.Filter.WarmRules(fi);
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

        #region//Sends：发送列表整表（含每条的发送集字节）

        /*
            发送与机器人两个<b>执行器留在目标里</b>（方案第二节）：
            SendExecute 就是循环调 SendPacket，而套接字句柄属于目标进程；
            拆成「外壳循环 + 每包一条命令」会让间隔与顺序失真。

            所以整份发送列表要下推，<b>连发送集的字节一起</b>。
            发送集是人手攒出来的重放序列，几十条顶天（见 CLAUDE.md 的发送编辑），
            整表推的字节完全可以接受。

            SCollection 里的 PacketInfo 只有五个字段会被执行器读到
            （PacketSocket / PacketType / PacketFrom / PacketTo / PacketBuffer，
            核过 SendExecute.Send_DoWork），其余一概不传。
        */

        public static byte[] EncodeSends()
        {
            var list = Operate.SendConfig.List.lstSendInfo;

            var w = new IpcWriter();
            w.I32(list.Count);

            foreach (SendInfo si in list)
            {
                w.Bool(si.IsEnable);
                w.Guid_(si.SID);
                w.Str(si.SName);
                w.Bool(si.SSystemSocket);
                w.I32(si.SLoopCNT);
                w.I32(si.SLoopINT);
                w.Str(si.SNotes);

                var packets = si.SCollection;
                w.I32(packets == null ? 0 : packets.Count);

                if (packets != null)
                {
                    foreach (PacketInfo pi in packets)
                    {
                        w.I32(pi.PacketSocket);
                        w.I32((int)pi.PacketType);
                        w.Str(pi.PacketFrom);
                        w.Str(pi.PacketTo);
                        w.Bytes(pi.PacketBuffer);
                    }
                }
            }

            return w.ToArray();
        }

        public static void ApplySends(byte[] payload)
        {
            var r = new IpcReader(payload);
            int n = r.I32();

            //运行期计数归目标（执行的副产品，只有执行者知道真值），换快照时按 GUID 迁移
            var oldCounts = new Dictionary<Guid, long[]>();
            foreach (SendInfo old in Operate.SendConfig.List.lstSendInfo)
            {
                oldCounts[old.SID] = new[] { old.ExecutionCount, old.ExecutionSuccess, old.ExecutionFail };
            }

            var fresh = new List<SendInfo>(n);

            for (int i = 0; i < n; i++)
            {
                bool isEnable = r.Bool();
                Guid sid = r.Guid_();
                string name = r.Str();
                bool sysSocket = r.Bool();
                int loopCnt = r.I32();
                int loopInt = r.I32();
                string notes = r.Str();

                int pn = r.I32();
                var packets = new BindingList<PacketInfo>();

                for (int k = 0; k < pn; k++)
                {
                    var pi = new PacketInfo();
                    pi.PacketSocket = r.I32();
                    pi.PacketType = (Operate.PacketConfig.Packet.PacketType)r.I32();
                    pi.PacketFrom = r.Str();
                    pi.PacketTo = r.Str();
                    pi.PacketBuffer = r.Bytes();
                    packets.Add(pi);
                }

                /*
                    ⚠️ 这条路<b>绕开 SendConfig.Send.AddSend</b>（那儿才是 NormalizeLoopCount 的咽喉），
                    所以自己夹一次。正常情况下编码那侧读的已经是外壳夹过的值，这里只是防
                    「协议对不上 / 编码侧被谁改坏」—— 目标进程里没有界面，坏值的表现会是
                    「发送列表看着在跑、一个包都不发」，比在外壳里更难查。
                    ⚠️ 这里<b>不记日志</b>：外壳夹的时候已经记过一条，目标里再记一条只是重复。
                */
                if (loopCnt < 1) { loopCnt = 1; }
                if (loopInt < 0) { loopInt = 0; }

                var si = new SendInfo(isEnable, sid, name, sysSocket, loopCnt, loopInt, packets, notes);

                long[] keep;
                if (oldCounts.TryGetValue(sid, out keep))
                {
                    si.ExecutionCount = keep[0];
                    si.ExecutionSuccess = keep[1];
                    si.ExecutionFail = keep[2];
                }

                fresh.Add(si);
            }

            var bl = Operate.SendConfig.List.lstSendInfo;
            bl.Clear();
            foreach (SendInfo si in fresh) { bl.Add(si); }
        }

        #endregion

        #region//Robots：机器人列表整表（含指令集）

        public static byte[] EncodeRobots()
        {
            var list = Operate.RobotConfig.List.lstRobotInfo;

            var w = new IpcWriter();
            w.I32(list.Count);

            foreach (RobotInfo ri in list)
            {
                w.Bool(ri.IsEnable);
                w.Guid_(ri.RID);
                w.Str(ri.RName);

                var inst = ri.RInstruction;
                w.I32(inst == null ? 0 : inst.Count);

                if (inst != null)
                {
                    foreach (InstructionInfo ii in inst)
                    {
                        w.I32((int)ii.InstType);
                        w.Str(ii.InstContent);
                    }
                }
            }

            return w.ToArray();
        }

        public static void ApplyRobots(byte[] payload)
        {
            var r = new IpcReader(payload);
            int n = r.I32();

            var oldCounts = new Dictionary<Guid, long>();
            foreach (RobotInfo old in Operate.RobotConfig.List.lstRobotInfo)
            {
                oldCounts[old.RID] = old.ExecutionCount;
            }

            var fresh = new List<RobotInfo>(n);

            for (int i = 0; i < n; i++)
            {
                bool isEnable = r.Bool();
                Guid rid = r.Guid_();
                string name = r.Str();

                int cn = r.I32();
                var inst = new BindingList<InstructionInfo>();

                for (int k = 0; k < cn; k++)
                {
                    var type = (Operate.RobotConfig.Robot.InstructionType)r.I32();
                    inst.Add(new InstructionInfo(type, r.Str()));
                }

                var ri = new RobotInfo(isEnable, rid, name, inst);

                long keep;
                if (oldCounts.TryGetValue(rid, out keep)) { ri.ExecutionCount = keep; }

                fresh.Add(ri);
            }

            var bl = Operate.RobotConfig.List.lstRobotInfo;
            bl.Clear();
            foreach (RobotInfo ri in fresh) { bl.Add(ri); }
        }

        #endregion

        #region//Runtime：极速模式 / 系统套接字 / 两种执行方式 / 当前选中封包

        public static byte[] EncodeRuntime()
        {
            var w = new IpcWriter();

            w.Bool(Operate.SystemConfig.SpeedMode);
            w.I32(Operate.SystemConfig.SystemSocket);
            w.I32((int)Operate.SystemConfig.ListExecute);

            /*
                滤镜执行方式（优先 / 依次）。<b>读它的是 DoFilterList，而那个方法跑在目标里</b>——
                不推下去的话「系统设置 → 滤镜执行方式」在注入模式下是个拨了不动的开关：
                外壳这边改了、落库了，目标那边仍按自己的初值走。
            */
            w.I32((int)Operate.FilterConfig.Filter.FilterExecute);

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
            Operate.FilterConfig.Filter.FilterExecute = (Operate.FilterConfig.Filter.Execute)r.I32();

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
