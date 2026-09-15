using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace WinsockPacketEditor.Ipc
{
    #region//目标进程里的无头核心

    /// <summary>
    /// 注入到目标进程里的那一半：13 个钩子 + 滤镜引擎 + 执行器 + 三条管道，
    /// <b>没有窗口、没有数据库、没有 OWIN、不改目标的 DPI</b>。
    ///
    /// 它实现 <see cref="IHookHost"/>：封包进环、仓库动作与日志走事件流、
    /// 选中封包读快照。装配之后 <c>HookHost.Current</c> 就是它，
    /// 于是 <c>Operate</c> 里那套逻辑一行不改就换了归宿。
    ///
    /// 【线程】
    ///   · 钩子线程（目标自己的收发线程）—— 只计数 + 入环 / 入事件队列，见 <see cref="OnPacket"/>
    ///   · 写线程    —— 专用 Thread，解析地址 + 编码 + 写封包流
    ///   · 事件线程  —— 专用 Thread，唯一写事件流的地方（日志 / 仓库 / 统计 / 丢包）
    ///   · 控制线程  —— 专用 Thread，读命令、执行、应答
    ///   · 心跳线程  —— 专用 Thread，1 秒一次；连续 3 次收不到外壳的 Ping 就自行卸钩休眠
    ///
    /// 一律用专用 <c>Thread</c> 而不是 <c>Task.Run</c>：线程池是<b>目标进程自己的</b>，
    /// 今天那些 Task.Run 已经在跟目标抢线程了（风险清单 R8）。
    /// </summary>
    public sealed class WpeCore : IHookHost
    {
        #region//单例

        private static WpeCore _instance;
        private static readonly object _instanceGate = new object();

        /// <summary>
        /// 已经装配好的核心。外壳重启后对同一 PID 再注一次时，
        /// <c>Hook.Run</c> 会再被调用一遍 —— 那时复用这个实例、只换管道，
        /// 这就是「可重新附加」（解决风险清单 R6）。
        /// </summary>
        public static WpeCore Instance { get { return _instance; } }

        #endregion

        #region//字段

        private readonly WinSockHook _hook = new WinSockHook();

        /*
            环的上限：方案第八节第 2 条说这两个数是估的，要按验证矩阵第 4 项实测定。
            65536 条 / 64 MB —— 3000 包/秒 × 4 KB 的话，64 MB 够缓冲 5 秒多，
            而写线程在本机管道上的吞吐是 GB/s 级，正常情况下环里根本不会积压。
        */
        private readonly PacketRing _ring = new PacketRing(65536, 64L * 1024 * 1024);

        /*
            事件流也要一格缓冲，理由与封包环一模一样 —— 见 SendEvent 那段说明。
            8192 条 / 16 MB：事件本来是低频的（日志、仓库、统计），
            这个量只为「外壳卡住的那几秒」留余地，不是为了长期堆积。

            ⚠️ 刻意<b>不复用 PacketRing</b>：那个环装的是 PendingPacket（要在写线程上解析地址、
            按封包帧编码），而事件是已经编码好的整帧、类型也不同。硬套一个泛型进去
            只会让两条路的「满了怎么办」纠缠在一起。这里就是一个队列 + 两个上限。
        */
        private readonly Queue<byte[]> _evtQueue = new Queue<byte[]>();
        private readonly object _evtQueueGate = new object();
        private readonly ManualResetEventSlim _evtSignal = new ManualResetEventSlim(false);
        private long _evtQueueBytes;
        private long _evtDroppedCount;

        private const int EvtQueueMaxCount = 8192;
        private const long EvtQueueMaxBytes = 16L * 1024 * 1024;

        private NamedPipeClientStream _ctl, _pkt, _evt;
        private Thread _writerThread, _controlThread, _heartbeatThread, _eventThread;

        private volatile bool _running;
        private volatile bool _hookInstalled;

        /// <summary>
        /// 控制线程正在处理一条命令。
        ///
        /// 【为什么心跳要看它】外壳的 Ping 与请求<b>共用同一条控制通道、同一把 _ctlGate</b>：
        /// 一条命令处理得久（<c>SendPacket</c> 撞上一个满的发送缓冲、<c>StartHook</c> 装 13 个钩子），
        /// 外壳那边的 Ping 就排在它后面发不出来 —— 而目标这头只看「多久没收到 Ping」，
        /// 于是<b>把正在替外壳干活当成了外壳已经没了</b>，3 秒一到自行卸钩。
        /// 表现是「点一下发送，抓包就断了」，而且日志里只有一句心跳超时。
        ///
        /// 所以：收到任何一条命令都算活着（见 <see cref="ControlLoop"/> 里刷 _lastPingTick 那句），
        /// 而处理过程中一律不判超时。
        /// </summary>
        private volatile bool _dispatching;

        /// <summary>目标是挂起启动创建的 —— 装钩之前要先把 winsock 拉进来，见 <see cref="DetectWinsock"/>。</summary>
        private bool _suspendedLaunch;

        private long _packetSeq;

        /// <summary>外壳最后一次心跳到达的时刻（Environment.TickCount）。</summary>
        private int _lastPingTick;

        /// <summary>事件流的写锁 —— 多条线程都会往它上面发东西（日志、仓库、丢包）。</summary>
        private readonly object _evtGate = new object();

        /// <summary>
        /// 核心收摊时置位。<c>Hook.Run</c> 的无头分支挂在它上面 ——
        /// Run 一返回 EasyHook 就认为注入的代码跑完了，而三条工作线程都是
        /// IsBackground，撑不住这个进程该有的生命周期。
        /// </summary>
        private static readonly ManualResetEventSlim _shutdown = new ManualResetEventSlim(false);

        /// <summary>
        /// 第一次收到 StartHook 时置位。
        ///
        /// 【它是干什么的】挂起启动的目标必须由 <c>RemoteHooking.WakeUpProcess()</c> 唤醒，
        /// 而那一句<b>只在 EasyHook 的 Run 那条线程上有效</b>（实测：在外壳里调作用于外壳自己；
        /// 在控制线程里调静默无效；用 NtResumeProcess 直接恢复则目标当场死掉 ——
        /// 原生的 curl.exe 与托管靶子都一样，说明 EasyHook 的挂起不只是 OS 的挂起计数）。
        ///
        /// 所以 Run 那条线程装配完核心之后就挂在这个事件上等，
        /// 控制线程处理完 StartHook 把它置位，Run 线程再去唤醒。
        /// 顺序仍与老路径一致：<b>钩子装好之后才唤醒</b>，第一个包也抓得到。
        /// </summary>
        private static readonly ManualResetEventSlim _firstStartHook = new ManualResetEventSlim(false);

        /// <summary>Runtime 快照里带下来的「封包列表当前选中的那一条」。</summary>
        public static SelectedPacket SelectedPacketSnapshot;

        #endregion

        #region//装配 / 拆卸

        /// <summary>
        /// 连上外壳的三条管道并开始工作。已经装配过就只换管道（重新附加）。
        /// </summary>
        public static void Attach(string sessionId, int connectTimeoutMs, bool suspendedLaunch)
        {
            lock (_instanceGate)
            {
                if (_instance == null)
                {
                    _instance = new WpeCore();
                }

                _instance._suspendedLaunch = suspendedLaunch;
                _instance.Connect(sessionId, connectTimeoutMs);
            }
        }

        private void Connect(string sessionId, int connectTimeoutMs)
        {
            //重新附加：先把上一轮收拾干净，但<b>不卸钩</b> —— 钩子还在抓，
            //外壳一连上就能接着看，中间那段的包进了环（满了丢旧的）。
            StopThreads();
            ClosePipes();

            _ctl = new NamedPipeClientStream(".", IpcProtocol.ControlPipe(sessionId), PipeDirection.InOut, PipeOptions.Asynchronous);
            _pkt = new NamedPipeClientStream(".", IpcProtocol.PacketPipe(sessionId), PipeDirection.Out, PipeOptions.Asynchronous);
            _evt = new NamedPipeClientStream(".", IpcProtocol.EventPipe(sessionId), PipeDirection.Out, PipeOptions.Asynchronous);

            try
            {
                _ctl.Connect(connectTimeoutMs);
                _pkt.Connect(connectTimeoutMs);
                _evt.Connect(connectTimeoutMs);
            }
            catch
            {
                /*
                    ⚠️ <b>连不上就必须把钩子收掉</b>，不能只是把异常抛出去。

                    这一条只在「重新附加」那条路上要紧，但后果很重：上面几行已经
                    StopThreads + ClosePipes 把<b>上一轮</b>拆了，而钩子是<b>没有</b>卸的
                    （那是有意的 —— 外壳一连上就能接着看）。所以连接失败之后目标会停在：
                    13 个钩子还在、<c>HookHost.Current</c> 还是这个核心、封包一路进环
                    （满 64 MB 就丢最旧的），而<b>三条线程与心跳全没了，再也没有人来收拾</b>。
                    表现是目标白白背着一套钩子和 64 MB 内存跑到进程退出为止。

                    「目标永远不能因为外壳没了而留着钩子在里面」（方案第四节第 9 条）
                    在这条路上同样成立。
                */
                SelfShutdown("连不上外壳的管道");
                throw;
            }

            //装配成 IHookHost：从这一句起，Operate 里所有的封包 / 日志 / 仓库出口都改道到管道。
            HookHost.Attach(this);

            _shutdown.Reset();

            _running = true;
            _lastPingTick = Environment.TickCount;

            _writerThread = StartThread(WriterLoop, "WPE-IPC-Writer");
            _controlThread = StartThread(ControlLoop, "WPE-IPC-Control");
            _heartbeatThread = StartThread(HeartbeatLoop, "WPE-IPC-Heartbeat");
            _eventThread = StartThread(EventLoop, "WPE-IPC-Event");
        }

        /// <summary>
        /// 阻塞到核心收摊（Detach 或心跳超时）。
        ///
        /// 【重新附加时它必须能复位】外壳重启后对同一 PID 再注一次，
        /// <c>Hook.Run</c> 会在<b>一条新线程</b>上再被调用一遍，那时事件已经是置位的 ——
        /// 不复位的话新那条 Run 会立刻返回，核心就没人挂着了。
        /// 复位放在 <c>Connect</c> 里（连上之后、开工之前）。
        /// </summary>
        public static void WaitForShutdown()
        {
            _shutdown.Wait();
        }

        /// <summary>
        /// 挂在「第一次 StartHook」上等。返回 true 表示该唤醒目标了，
        /// 返回 false 表示核心已经收摊（用户压根没开始拦截就退出了）。
        ///
        /// 由 <c>Hook.RunHeadless</c> 在 EasyHook 的 Run 线程上调 —— 见 <see cref="_firstStartHook"/>。
        /// </summary>
        public static bool WaitForWakeSignal()
        {
            int i = WaitHandle.WaitAny(new[] { _firstStartHook.WaitHandle, _shutdown.WaitHandle });
            return i == 0;
        }

        private static Thread StartThread(ThreadStart body, string name)
        {
            var t = new Thread(body);
            t.IsBackground = true;
            t.Name = name;
            t.Start();
            return t;
        }

        /// <summary>
        /// 卸钩 + 停线程 + 断管道，核心进入休眠。<b>不卸载 CLR</b>（做不到，也不必要）。
        /// </summary>
        public void Detach()
        {
            StopHookInternal();

            //⚠️ 事件是异步发的，卸钩那条 HookState(false) 还在队列里 ——
            //不冲一次就会被下面的 ClosePipes 一起带走，界面停在「拦截中」
            FlushEvents(200);

            StopThreads();
            ClosePipes();
            _ring.Clear();
            HookHost.Detach();
            _shutdown.Set();
        }

        private void StopThreads()
        {
            _running = false;
            _ring.Wake();
            _evtSignal.Set();

            //不 Join：控制线程正阻塞在管道 Read 上，要等到管道关掉才回得来。
            //四条线程都是 IsBackground，进程退出时不会被它们拖住。
            _writerThread = null;
            _controlThread = null;
            _heartbeatThread = null;
            _eventThread = null;
        }

        private void ClosePipes()
        {
            SafeDispose(ref _ctl);
            SafeDispose(ref _pkt);
            SafeDispose(ref _evt);
        }

        private static void SafeDispose<T>(ref T s) where T : class, IDisposable
        {
            T local = s;
            s = null;
            if (local == null) { return; }
            try { local.Dispose(); } catch { /* 断管道时的异常没有意义 */ }
        }

        #endregion

        #region//IHookHost —— 封包出口（钩子线程！）

        /// <summary>
        /// ⚠️ <b>这是目标进程的收发线程</b>。这个方法里只许有：两次判断 + 一次入环。
        /// 不写管道、不等锁（环里那把锁只护几十纳秒）、不 Task.Run、不查归属地、不格式化字符串。
        /// </summary>
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
            //这两道早退与 ProcessingHookResultAsync 开头那两句逐字对应，
            //必须一模一样 —— 差一条就是两条路径抓到的封包对不上。
            if (filterAction == Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay) { return; }
            if (filterAction != Operate.FilterConfig.Filter.FilterAction.Intercept && res <= 0) { return; }

            /*
                ⚠️⚠️ <b>计数在这儿数，不在外壳。</b>

                这一句的位置是照着进程内那条路摆的：<c>PacketInfo_ToQueue</c> 里
                <c>CountPacketInfo</c> 就在<b>极速模式判断之前</b>、两道早退之后 ——
                也就是说「极速模式」只关掉列表，<b>计数照数</b>。那正是这个开关的意义：
                不建列表、只看吞吐。

                原来这一句在外壳的 <c>ShellLink.Ingest</c> 里，于是注入模式下：
                  · 打开极速模式 → 这里直接 return → 一帧都不发 → <b>统计格 13 个数字全是 0</b>，
                    而用户开极速模式恰恰就是为了看那几个数；
                  · 地址解析不出来的包（套接字已经关了）在写线程上被丢掉 → 也没计上，
                    而进程内那条路是<b>数了</b>的。

                改成目标自己数、随 1 Hz 的 Stats 报上去（与滤镜那六个全局计数同一条路数：
                「执行的副产品，只有执行者知道真值」）。代价是外壳那份变成 1 秒一跳的镜像，
                而那几个格子本来就是给人看的，不参与任何判断。

                ⚠️ <b>FilterPacket_CNT 不在这里</b> —— 「已过滤」是外壳的 FlushToFeed 数的
                （目标压根不碰它），报上去只会把外壳的真值冲成 0。
            */
            Operate.PacketConfig.Packet.CountPacketInfo(packetType, newBuffer == null ? 0 : newBuffer.Length);

            //极速模式：只数不建列表（与 PacketInfo_ToQueue 里那个 if 同位置、同语义）
            if (Operate.SystemConfig.SpeedMode) { return; }

            _ring.Enqueue(new PendingPacket
            {
                Id = Interlocked.Increment(ref _packetSeq),
                TimeTicks = packetTime.Ticks,
                TimeKind = packetTime.Kind,
                Socket = socket,
                PacketType = packetType,
                FilterAction = filterAction,
                SockAddr = sockaddr,
                Raw = rawBuffer,
                Modified = newBuffer,
            });
        }

        #endregion

        #region//IHookHost —— 仓库 / 日志 / 选中封包

        /// <summary>目标不再持有仓库，只把「哪个仓库 + 哪段字节」送出去，外壳落库。</summary>
        public void OnStore(Guid warehouseGuid, byte[] packetBuffer)
        {
            var w = new IpcWriter();
            w.U8((byte)IpcEvent.StoreAdded);
            w.Guid_(warehouseGuid);
            w.Bytes(packetBuffer);
            SendEvent(w);
        }

        /// <summary>
        /// 目标侧的日志。<b>不写磁盘</b> —— 方案第四节第 8 条：
        /// 目标里不打开任何文件，唯一的句柄是三条管道。
        /// </summary>
        public void OnLog(string funcName, string content)
        {
            var w = new IpcWriter();
            w.U8((byte)IpcEvent.Log);
            w.Str(funcName);
            w.Str(content);
            SendEvent(w);
        }

        public void OnFilterLog(
            string filterName,
            Operate.FilterConfig.Filter.FilterAction filterAction,
            int matchNum,
            Operate.PacketConfig.Packet.PacketType packetType,
            int packetLen)
        {
            var w = new IpcWriter();
            w.U8((byte)IpcEvent.FilterLog);
            w.Str(filterName);
            w.I32((int)filterAction);
            w.I32(matchNum);
            w.I32((int)packetType);
            w.I32(packetLen);
            SendEvent(w);
        }

        public SelectedPacket GetSelectedPacket()
        {
            return SelectedPacketSnapshot;
        }

        #endregion

        #region//事件流

        /*
            ⚠️⚠️ <b>这个方法会在钩子线程上被调到</b>，所以它只许「入队 + 置位」。

            【原来是同步写管道，那是个真问题】OnLog / OnFilterLog / OnStore 三条出口都汇到这儿，
            而它们的调用点全在 DoFilter 里 ——<b>也就是目标进程的收发线程上</b>：

              · 滤镜命中就 DoFilterLog 一次 → 命中率高的滤镜等于<b>每个包一次同步管道写</b>；
              · 滤镜动作是「入库」就 OnStore 一次，载荷是<b>整包字节</b>；
              · 钩子体里任何一次 catch 都会 DoLog。

            管道的写缓冲是有限的（外壳侧 evt 管道 64 KB）。外壳的事件线程一旦慢下来
            （UI 线程忙、归属地查询卡住、进程被挂起），缓冲一满 <c>Write</c> 就<b>阻塞</b> ——
            阻塞的是目标的 send() / recv()，也就是<b>把目标游戏卡住</b>。
            这正是方案第四节第 2 条与第 4 条明令禁止的那件事，而封包那条路一直守着，
            事件这条路却漏了。

            （所以「外壳整个挂起 5 秒、目标吞吐 100%」那次实测并不覆盖这里 ——
             那一轮没有配任何会命中的滤镜，事件流是空的。）

            现在与封包同构：有界队列 + 专用线程，满了<b>丢最旧的并计数</b>，永不阻塞。
        */
        private void SendEvent(IpcWriter w)
        {
            if (!_running) { return; }

            byte[] frame = w.ToArray();

            lock (_evtQueueGate)
            {
                _evtQueue.Enqueue(frame);
                _evtQueueBytes += frame.Length;

                //先入再挤，与 PacketRing 同一条口径：单条超大的也进得来
                while ((_evtQueue.Count > EvtQueueMaxCount || _evtQueueBytes > EvtQueueMaxBytes)
                       && _evtQueue.Count > 1)
                {
                    byte[] old = _evtQueue.Dequeue();
                    _evtQueueBytes -= old.Length;
                    _evtDroppedCount++;
                }
            }

            _evtSignal.Set();
        }

        /// <summary>
        /// 事件线程：队列 → 事件流。<b>唯一一个写 evt 管道的地方</b>。
        /// </summary>
        private void EventLoop()
        {
            while (_running)
            {
                List<byte[]> batch = TakeEvents(true);

                if (batch.Count == 0) { continue; }

                if (!WriteEvents(batch))
                {
                    //管道断了：停下来等心跳去收拾，不要在这里空转刷日志
                    if (!_running) { return; }
                    Thread.Sleep(200);
                }
            }
        }

        private bool EvtQueueEmpty()
        {
            lock (_evtQueueGate) { return _evtQueue.Count == 0; }
        }

        private List<byte[]> TakeEvents(bool wait)
        {
            if (wait && EvtQueueEmpty())
            {
                _evtSignal.Reset();

                //Reset 与 Wait 之间可能刚好来了一条，所以 Reset 之后再看一眼 ——
                //漏掉这一眼的后果是「明明有事件，事件线程却睡满 50ms」（同 PacketRing）
                if (EvtQueueEmpty()) { _evtSignal.Wait(50); }
            }

            var batch = new List<byte[]>();

            lock (_evtQueueGate)
            {
                while (_evtQueue.Count > 0)
                {
                    byte[] f = _evtQueue.Dequeue();
                    _evtQueueBytes -= f.Length;
                    batch.Add(f);
                }
            }

            return batch;
        }

        /// <summary>把一批事件写出去。写不出去返回 false（管道断了）。</summary>
        private bool WriteEvents(List<byte[]> batch)
        {
            Stream s = _evt;
            if (s == null) { return false; }

            try
            {
                lock (_evtGate)
                {
                    foreach (byte[] f in batch) { IpcFrame.Write(s, f); }

                    /*
                        ⚠️ 丢掉的事件要出声。无声丢日志 / 丢入库比阻塞更糟 ——
                        用户会以为「这条滤镜没命中」而不是「那条记录被挤掉了」。
                        与封包那边的「丢弃 N」是同一条规矩，只是这里没有专门的界面位置，
                        所以补一条日志（它本身也走这条队列，写在这里不会递归：
                        入队 → 下一批发出去）。
                    */
                    long dropped;
                    lock (_evtQueueGate) { dropped = _evtDroppedCount; _evtDroppedCount = 0; }

                    if (dropped > 0)
                    {
                        var w = new IpcWriter();
                        w.U8((byte)IpcEvent.Log);
                        w.Str("WpeCore.SendEvent");
                        w.Str("事件流积压，丢弃了 " + dropped + " 条（日志 / 入库 / 统计）");
                        IpcFrame.Write(s, w.ToArray());
                    }
                }

                return true;
            }
            catch
            {
                /*
                    ⚠️ 这里绝对不能再调 Operate.DoLog —— 那会转回 OnLog、再入队、
                    下一轮再抛一次。管道断了本来就有心跳去处理。
                */
                return false;
            }
        }

        /// <summary>
        /// 把队列里的事件尽量写完（收摊前用）。
        ///
        /// 【为什么需要它】<c>StopHookInternal</c> 发的那条 <c>HookState(false)</c> 与
        /// <c>Detach</c> 关管道之间只隔几行 —— 事件改成异步之后，不冲一次的话
        /// 那条状态永远到不了外壳，界面上会停在「拦截中」直到管道断开才更正。
        /// </summary>
        private void FlushEvents(int timeoutMs)
        {
            int deadline = unchecked(Environment.TickCount + timeoutMs);

            while (unchecked(deadline - Environment.TickCount) > 0)
            {
                List<byte[]> batch = TakeEvents(false);
                if (batch.Count == 0) { return; }
                if (!WriteEvents(batch)) { return; }
            }
        }

        #endregion

        #region//写线程：环 → 封包流

        private void WriterLoop()
        {
            while (_running)
            {
                try
                {
                    //一批最多 256 条 / 1 MB：够摊薄系统调用，又不至于让单次写占住太久。
                    List<PendingPacket> batch = _ring.DequeueBatch(256, 1024 * 1024, 5);

                    if (batch.Count == 0) { continue; }

                    Stream s = _pkt;
                    if (s == null) { continue; }

                    foreach (PendingPacket p in batch)
                    {
                        /*
                            地址在这里解析 —— 必须在目标进程里调（套接字句柄是目标的），
                            但不必在钩子线程上调。今天它也不在钩子线程上，
                            是在 PacketInfo_ToQueue 的 Task.Run 里。

                            与今天一致：取不到地址（套接字已经关了）就<b>丢掉这一条</b>，
                            因为 PacketInfo_ToQueue 里那个 `sPacketIP.Contains("|")` 就是这么做的。
                        */
                        string ipPair = Operate.PacketConfig.Packet.GetIPString_BySocketAddr(
                            p.Socket, p.SockAddr, p.PacketType);

                        if (string.IsNullOrEmpty(ipPair) || !ipPair.Contains("|")) { continue; }

                        string[] parts = ipPair.Split('|');

                        byte[] frame = PacketFrame.Encode(
                            p.Id, p.TimeKind == DateTimeKind.Utc
                                ? new DateTime(p.TimeTicks, DateTimeKind.Utc).ToLocalTime().Ticks : p.TimeTicks, p.Socket,
                            (byte)p.PacketType, (byte)p.FilterAction,
                            parts[0], parts[1],
                            p.Raw, p.Modified);

                        IpcFrame.Write(s, frame);
                    }

                    ReportDroppedIfChanged();
                }
                catch (Exception ex)
                {
                    //管道断了：停下来等心跳去收拾，不要在这里空转刷日志。
                    if (!_running) { break; }
                    SendFatal("WriterLoop: " + ex.Message);
                    Thread.Sleep(200);
                }
            }
        }

        private long _reportedDropped;

        #region//1 Hz 的统计上报

        /// <summary>
        /// 计数器与执行器状态，1 秒一包。
        ///
        /// 【为什么这些数归目标】它们是<b>执行的副产品</b>，只有执行者知道真值 ——
        /// 滤镜命中次数在目标的收发线程上累加，发送 / 机器人的执行次数在目标的
        /// BackgroundWorker 里就地 ++。外壳那份是显示用的副本。
        ///
        /// 由心跳线程顺带发（它本来就是 1 秒一拍），不另起一条线程。
        /// </summary>
        private void SendStats()
        {
            var w = new IpcWriter();
            w.U8((byte)IpcEvent.Stats);

            w.Bool(Operate.SendConfig.List.IsSendListRunning);
            w.Bool(Operate.RobotConfig.List.IsRobotListRunning);

            //滤镜的执行次数：按 GUID 报，外壳按 GUID 更新自己那份副本
            var filters = FilterEngine.Filters;
            w.I32(filters.Count);
            for (int i = 0; i < filters.Count; i++)
            {
                w.Guid_(filters[i].FID);
                w.I64(filters[i].ExecutionCount);
            }

            var sends = Snapshot(Operate.SendConfig.List.lstSendInfo);
            w.I32(sends.Count);
            foreach (SendInfo si in sends)
            {
                w.Guid_(si.SID);
                w.I64(si.ExecutionCount);
                w.I64(si.ExecutionSuccess);
                w.I64(si.ExecutionFail);
            }

            /*
                ⚠️ <b>滤镜那六个全局计数也要报上来。</b>

                它们在 DoFilterList 里递增，而 DoFilterList 跑在<b>目标</b>的钩子线程上 ——
                外壳自己那份从头到尾是 0。少了这一段，「统计数据」页在注入模式下
                分母有（TotalPackets 由外壳的 ShellLink.Ingest 维护）、<b>分子恒为 0</b>，
                六条进度条全是 0%，而且看不出是坏了还是真没命中。
            */
            w.I64(Operate.FilterConfig.Filter.FilterExecute_CNT);
            w.I64(Operate.FilterConfig.Filter.FilterReplace_CNT);
            w.I64(Operate.FilterConfig.Filter.FilterChange_CNT);
            w.I64(Operate.FilterConfig.Filter.FilterIntercept_CNT);
            w.I64(Operate.FilterConfig.Filter.FilterDisplay_CNT);
            w.I64(Operate.FilterConfig.Filter.FilterNoDisplay_CNT);

            var robots = Snapshot(Operate.RobotConfig.List.lstRobotInfo);
            w.I32(robots.Count);
            foreach (RobotInfo ri in robots)
            {
                w.Guid_(ri.RID);
                w.I64(ri.ExecutionCount);
            }

            /*
                封包计数那 11 个 —— 注入模式下它们在<b>目标</b>的钩子线程上递增
                （见 OnPacket 里 CountPacketInfo 那段），外壳那份是镜像。
                ⚠️ 顺序与 ShellLink.ApplyStats 里读的那一串必须逐个对上。
                ⚠️ FilterPacket_CNT 不在其内（那是外壳 FlushToFeed 数的）。
            */
            w.I64(Operate.PacketConfig.Packet.TotalPackets);
            w.I64(Operate.PacketConfig.Packet.Send_CNT);
            w.I64(Operate.PacketConfig.Packet.SendTo_CNT);
            w.I64(Operate.PacketConfig.Packet.Recv_CNT);
            w.I64(Operate.PacketConfig.Packet.RecvFrom_CNT);
            w.I64(Operate.PacketConfig.Packet.WSASend_CNT);
            w.I64(Operate.PacketConfig.Packet.WSASendTo_CNT);
            w.I64(Operate.PacketConfig.Packet.WSARecv_CNT);
            w.I64(Operate.PacketConfig.Packet.WSARecvFrom_CNT);
            w.I64(Operate.PacketConfig.Packet.Total_SendBytes);
            w.I64(Operate.PacketConfig.Packet.Total_RecvBytes);

            SendEvent(w);
        }

        /// <summary>
        /// 取一份定格的列表再遍历。
        ///
        /// ⚠️ <b>不能直接 foreach 那两个 BindingList</b>：<c>SetConfig</c> 会在<b>控制线程</b>上
        /// 把它们整表 Clear + Add（见 ConfigSnapshot.ApplySends / ApplyRobots），
        /// 而这一拍跑在心跳线程上 —— 撞上就抛「集合已修改」，整包统计静默丢掉。
        ///
        /// 拷贝本身也可能撞上（CopyTo 会读 Count 再拷），所以外面还有一层 try ——
        /// 但把窗口从「整个遍历 + 序列化」缩到「一次 CopyTo」已经差了几个数量级。
        /// </summary>
        private static List<T> Snapshot<T>(System.ComponentModel.BindingList<T> list)
        {
            try { return new List<T>(list); }
            catch { return new List<T>(); }
        }

        #endregion

        private void ReportDroppedIfChanged()
        {
            long now = _ring.Dropped;
            if (now == _reportedDropped) { return; }
            _reportedDropped = now;

            var w = new IpcWriter();
            w.U8((byte)IpcEvent.Dropped);
            w.I64(now);
            SendEvent(w);
        }

        #endregion

        #region//控制线程：命令 → 应答

        private void ControlLoop()
        {
            while (_running)
            {
                byte[] req;

                try
                {
                    req = IpcFrame.Read(_ctl, IpcProtocol.MaxControlFrame);
                }
                catch
                {
                    req = null;
                }

                //管道关了 = 外壳没了。绝不能留着钩子在目标里（方案第四节第 9 条）。
                if (req == null || req.Length == 0)
                {
                    if (_running) { SelfShutdown("控制管道已断开"); }
                    return;
                }

                /*
                    ⚠️ <b>收到任何一条命令都算「外壳还活着」</b>，不只是 Ping。

                    外壳那边 Ping 与请求<b>共用一条控制通道、同一把锁</b>（ShellLink._ctlGate）：
                    一条命令处理得久一点，Ping 就排在它后面发不出来 —— 而这头原来只看
                    「多久没收到 Ping」，于是把「正在替外壳干活」当成「外壳没了」，
                    3 秒一到自行卸钩。表现是<b>点一下发送，抓包就断了</b>。

                    所以刷一次时间戳，并在处理期间挂起超时判断（_dispatching）。
                */
                _lastPingTick = Environment.TickCount;

                byte[] reply;

                _dispatching = true;
                try { reply = Dispatch(req); }
                catch (Exception ex) { reply = Fail(ex.Message); }
                finally { _dispatching = false; _lastPingTick = Environment.TickCount; }

                try { IpcFrame.Write(_ctl, reply); }
                catch { if (_running) { SelfShutdown("应答写不出去"); } return; }
            }
        }

        private byte[] Dispatch(byte[] req)
        {
            var r = new IpcReader(req);
            var cmd = (IpcCommand)r.U8();

            switch (cmd)
            {
                case IpcCommand.Hello:
                    {
                        int version = r.I32();

                        if (version != IpcProtocol.Version)
                        {
                            //协议对不上就拒绝，不猜（方案第四节第 10 条）
                            var bad = new IpcWriter();
                            bad.U8((byte)IpcStatus.VersionMismatch);
                            bad.I32(IpcProtocol.Version);
                            return bad.ToArray();
                        }

                        _lastPingTick = Environment.TickCount;

                        var w = new IpcWriter();
                        w.U8((byte)IpcStatus.Ok);
                        w.I32(IpcProtocol.Version);
                        //⚠️ 目标进程里的每一个句柄都要还 —— Process 是 IDisposable，
                        //不 Dispose 就是留一个进程句柄等终结器（这一条在<b>别人的进程</b>里更该守）
                        using (var self = System.Diagnostics.Process.GetCurrentProcess()) { w.I32(self.Id); }
                        w.Bool(IntPtr.Size == 8);

                        /*
                            ⚠️ 附加成功就把「这个目标用的是哪套 WinSock」报上去，
                            <b>不等用户点「开始拦截」</b>。

                            那三个标志只随 HookState 事件走，而这个事件原来只在 StartHook /
                            StopHook 时发 —— 于是刚附加上的那段时间界面上那块读数窗是空的，
                            看着像「没探到」。它其实是目标进程的一个<b>静态事实</b>，
                            与钩子装没装无关。

                            ⚠️ `mayLoad: false` —— 握手这一次只看模块表，不拉模块进来。
                            挂起启动的目标这时还没跑加载器，三个都会是 false；
                            等 StartHook 那次会重新探一遍并再报一次，界面自己就更正了。
                        */
                        DetectWinsock(false);
                        SendHookState(_hookInstalled);

                        return w.ToArray();
                    }

                case IpcCommand.Ping:
                    _lastPingTick = Environment.TickCount;
                    return Ok();

                case IpcCommand.SetConfig:
                    {
                        var kind = (ConfigKind)r.U8();
                        byte[] payload = r.Bytes();

                        switch (kind)
                        {
                            case ConfigKind.HookFlags: ConfigSnapshot.ApplyHookFlags(payload); break;
                            case ConfigKind.Filters: ConfigSnapshot.ApplyFilters(payload); break;
                            case ConfigKind.Runtime: ConfigSnapshot.ApplyRuntime(payload); break;
                            case ConfigKind.Sends: ConfigSnapshot.ApplySends(payload); break;
                            case ConfigKind.Robots: ConfigSnapshot.ApplyRobots(payload); break;
                            default: return Fail("不认识的快照类别: " + kind);
                        }

                        return Ok();
                    }

                case IpcCommand.StartHook:
                    if (!_hookInstalled)
                    {
                        DetectWinsock();
                        _hook.StartHook();
                        _hookInstalled = _hook.InstalledCount > 0;
                        if (!_hookInstalled) { return Fail("没有成功安装任何 WinSock 钩子"); }

                        /*
                            ⚠️ <b>唤醒挂起的目标不在这里做，在外壳侧</b>（ShellLink.StartHook）。

                            方案第 3.4 节写的是「由外壳在 StartHook 应答后 WakeUpProcess()」。
                            实测下来这一句放哪儿都不对：
                              · 在<b>外壳</b>里调 —— RemoteHooking.WakeUpProcess() 作用于调用方自己，
                                目标永远醒不过来；
                              · 在目标的<b>控制线程</b>里调 —— 静默返回、不抛异常、也什么都没发生
                                （靶子的留痕文件是空的，说明 Main 根本没进过）。
                            只有在 EasyHook 的 Run 那条线程上调才起作用，而 StartHook 是在
                            控制线程上处理的，够不着那条线程。

                            所以这里只<b>置位</b>，真正那一句由 Run 线程去调（它一直挂在这个事件上等）。
                            顺序仍与老路径一致：<b>钩子装好之后才唤醒</b>，所以第一个包也抓得到。
                        */
                        _firstStartHook.Set();

                        SendHookState(true);
                    }
                    return Ok();

                case IpcCommand.StopHook:
                    StopHookInternal();
                    return Ok();

                case IpcCommand.SendPacket:
                    {
                        int socket = r.I32();
                        var type = (Operate.PacketConfig.Packet.PacketType)r.I32();
                        string from = r.Str();
                        string to = r.Str();
                        byte[] bytes = r.Bytes();

                        bool sent = Operate.PacketConfig.Packet.SendPacket(socket, type, from, to, bytes);

                        var w = new IpcWriter();
                        w.U8((byte)IpcStatus.Ok);
                        w.Bool(sent);
                        return w.ToArray();
                    }

                case IpcCommand.GetSocketInfo:
                    {
                        int socket = r.I32();
                        var w = new IpcWriter();
                        w.U8((byte)IpcStatus.Ok);
                        w.Str(Operate.PacketConfig.Packet.GetIP_BySocket(socket, Operate.PacketConfig.Packet.IPType.From));
                        w.Str(Operate.PacketConfig.Packet.GetIP_BySocket(socket, Operate.PacketConfig.Packet.IPType.To));
                        return w.ToArray();
                    }

                case IpcCommand.ResetStats:
                    {
                        /*
                            掩码而不是「一个命令清所有」：界面上四个入口的语义各不相同，
                            见 IpcProtocol.ResetWhat 上面那段。
                        */
                        var what = (ResetWhat)r.U8();

                        if ((what & ResetWhat.FilterStats) != 0) { Operate.SystemConfig.ResetFilterStats(); }
                        if ((what & ResetWhat.PacketCounters) != 0) { Operate.SystemConfig.ResetPacketCounters(true); }
                        if ((what & ResetWhat.SendCounts) != 0) { Operate.SendConfig.List.InitSendList_Count(); }
                        if ((what & ResetWhat.RobotCounts) != 0) { Operate.RobotConfig.List.InitRobotList_Count(); }

                        var w = new IpcWriter();
                        w.U8((byte)IpcStatus.Ok);
                        return w.ToArray();
                    }

                case IpcCommand.GetFootprint:
                    {
                        var w = new IpcWriter();
                        w.U8((byte)IpcStatus.Ok);

                        var asms = AppDomain.CurrentDomain.GetAssemblies();
                        w.I32(asms.Length);

                        foreach (var a in asms)
                        {
                            string name;
                            try { name = a.GetName().Name; } catch { name = "(?)"; }

                            string loc;
                            try { loc = a.IsDynamic ? "(dynamic)" : a.Location; } catch { loc = "(?)"; }

                            w.Str(name);
                            w.Str(loc);
                        }

                        using (var self = System.Diagnostics.Process.GetCurrentProcess())
                        {
                            var mods = self.Modules;
                            w.I32(mods.Count);
                            foreach (System.Diagnostics.ProcessModule m in mods)
                            {
                                try { w.Str(m.ModuleName); } catch { w.Str("(?)"); }
                            }
                        }

                        return w.ToArray();
                    }

                case IpcCommand.Detach:
                    //先把应答发出去，再由控制线程收尾（不然外壳等不到回复）
                    ThreadPool.QueueUserWorkItem(_ => { Thread.Sleep(50); Detach(); });
                    return Ok();

                #region//执行器（阶段 2）

                /*
                    发送与机器人两个执行器<b>留在目标里</b>：
                    它们最终都是在调 SendPacket，而套接字句柄属于目标进程；
                    而且滤镜可以触发它们（FilterExecuteType.Send / Robot），
                    那条路本来就在目标的收发线程上。
                    外壳只负责启停与显示，不持有执行器 —— 启停的前置条件（列表非空、没在跑）
                    才不会散成两份。
                */

                case IpcCommand.StartSend:
                    //⚠️ 必须经 _Add 进表，否则 StopSendList 收不掉它（与代理模式的 DoSend_ByIndex 同形）
                    Operate.SendConfig.List.SendExecute_Add(Operate.SendConfig.Send.DoSend(r.Guid_()));
                    return Ok();

                case IpcCommand.StartSendList:
                    Operate.SendConfig.List.StartSendList();
                    return Ok();

                case IpcCommand.StopSendList:
                    Operate.SendConfig.List.StopSendList();
                    return Ok();

                case IpcCommand.StartRobot:
                    {
                        Guid rid = r.Guid_();
                        int filterSocket = r.I32();

                        var parameters = new Dictionary<string, object> { { "FilterSocket", filterSocket } };
                        //⚠️ 同上：不进表的话 StopRobotList 收不掉它
                        Operate.RobotConfig.List.RobotExecute_Add(Operate.RobotConfig.Robot.DoRobot(rid, parameters));
                        return Ok();
                    }

                case IpcCommand.StartRobotList:
                    Operate.RobotConfig.List.StartRobotList();
                    return Ok();

                case IpcCommand.StopRobotList:
                    Operate.RobotConfig.List.StopRobotList();
                    return Ok();

                #endregion

                default:
                    return Fail("不认识的命令: " + cmd);
            }
        }

        private static byte[] Ok()
        {
            var w = new IpcWriter();
            w.U8((byte)IpcStatus.Ok);
            return w.ToArray();
        }

        private static byte[] Fail(string msg)
        {
            var w = new IpcWriter();
            w.U8((byte)IpcStatus.Error);
            w.Str(msg);
            return w.ToArray();
        }

        #region//装钩之前先把三个 winsock DLL 拉进来

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string name);

        /// <summary>
        /// 定下这个目标支持哪几套 WinSock（Support_WS1 / WS2 / MsWS），必要时先把模块拉进来。
        ///
        /// 【为什么探测必须在目标里做】它看的是<b>本进程</b>的模块表 ——
        /// 老路径的 <c>GetInjectWinsockInfo</c> 也是这么写的，位置一直是对的。
        /// 外壳看不到目标加载了什么，所以这三个标志<b>不走配置快照</b>，由目标自己定，
        /// 再随 HookState 事件报上去给界面显示。
        ///
        /// 【为什么挂起启动要额外 LoadLibrary】
        /// <c>LocalHook.GetProcAddress</c> 要求模块<b>已经加载进本进程</b>，否则抛
        /// DllNotFoundException —— 而 <c>StartHook</c> 把异常吞进日志了，
        /// 表现是「钩子装上了却一条都抓不到」。挂起创建的进程连加载器都没跑过，
        /// 那一刻连 ws2_32 都还没有，13 个钩子一个都装不上，唤醒之后目标一路裸奔
        /// （实测就是 0 条包）。这三个都是系统 DLL，目标反正马上就要用，
        /// 提前拉进来正是「钩子先就位、第一个包也抓得到」的前提。
        ///
        /// 【为什么已在跑的目标不硬塞】那会凭空给它多出一个本来没有的模块，
        /// 检测面比老路径还大。已在跑的目标只探测，与老路径逐字一致。
        /// </summary>
        private void DetectWinsock(bool mayLoad = true)
        {
            try
            {
                /*
                    ⚠️ `mayLoad: false` 是<b>握手那一次</b>用的：那时只是想让界面上的
                    「WinSock」有个值，不该顺手改变目标进程的模块表。
                    真要装钩之前（StartHook）仍然按老样子拉一次，理由见上面那段。
                */
                if (mayLoad && _suspendedLaunch)
                {
                    LoadLibrary("ws2_32.dll");
                    LoadLibrary("wsock32.dll");
                    LoadLibrary("mswsock.dll");
                }

                bool ws1 = false, ws2 = false, msws = false;

                using (var self = System.Diagnostics.Process.GetCurrentProcess())
                {
                    foreach (System.Diagnostics.ProcessModule m in self.Modules)
                    {
                        string name = m.ModuleName;
                        if (string.Equals(name, WSock32.ModuleName, StringComparison.OrdinalIgnoreCase)) { ws1 = true; }
                        if (string.Equals(name, WS2_32.ModuleName, StringComparison.OrdinalIgnoreCase)) { ws2 = true; }
                        if (string.Equals(name, Mswsock.ModuleName, StringComparison.OrdinalIgnoreCase)) { msws = true; }
                    }
                }

                Operate.PacketConfig.Packet.Support_WS1 = ws1;
                Operate.PacketConfig.Packet.Support_WS2 = ws2;
                Operate.PacketConfig.Packet.Support_MsWS = msws;
            }
            catch (Exception ex)
            {
                SendFatal("探测 winsock 失败: " + ex.Message);
            }
        }

        #endregion

        private void StopHookInternal()
        {
            if (!_hookInstalled) { return; }

            try { _hook.StopHook(); }
            catch { /* 卸钩失败也要把状态改掉，否则再也停不下来 */ }

            _hookInstalled = false;
            _ring.Clear();
            SendHookState(false);
        }

        private void SendHookState(bool on)
        {
            var w = new IpcWriter();
            w.U8((byte)IpcEvent.HookState);
            w.Bool(on);

            //三个 Support_* 是目标自己探出来的，界面上要显示「这个目标用的是哪套 WinSock」
            w.Bool(Operate.PacketConfig.Packet.Support_WS1);
            w.Bool(Operate.PacketConfig.Packet.Support_WS2);
            w.Bool(Operate.PacketConfig.Packet.Support_MsWS);

            SendEvent(w);
        }

        private void SendFatal(string text)
        {
            var w = new IpcWriter();
            w.U8((byte)IpcEvent.Fatal);
            w.Str(text);
            SendEvent(w);
        }

        #endregion

        #region//心跳：外壳没了就自行卸钩

        /// <summary>外壳失联多久算没了。1 秒一跳，连续 3 次收不到就动手。</summary>
        private const int HeartbeatTimeoutMs = 3000;

        private void HeartbeatLoop()
        {
            while (_running)
            {
                Thread.Sleep(1000);
                if (!_running) { return; }

                //统计顺带在这一拍发（本来就是 1 秒一次），不另起线程
                try { SendStats(); } catch { /* 管道断了，下面的超时判断会收拾 */ }

                //⚠️ 正在处理命令时不判超时 —— 那是「在替外壳干活」，不是「外壳没了」
                if (_dispatching) { continue; }

                int idle = unchecked(Environment.TickCount - _lastPingTick);

                if (idle > HeartbeatTimeoutMs)
                {
                    /*
                        目标进程<b>永远不能因为外壳没了而留着钩子在里面</b>（方案第四节第 9 条）。
                        卸钩之后目标就恢复原生行为，用户重开外壳对同一 PID 再注一次即可。
                    */
                    SelfShutdown("外壳心跳超时 " + idle + "ms");
                    return;
                }
            }
        }

        private void SelfShutdown(string why)
        {
            try
            {
                _running = false;
                StopHookInternal();
                _ring.Clear();
                ClosePipes();
                HookHost.Detach();
            }
            catch { /* 这条路上没有能补救的东西了 */ }
            finally { _shutdown.Set(); }
        }

        #endregion
    }

    #endregion
}
