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
    ///   · 钩子线程（目标自己的收发线程）—— 只入环，见 <see cref="OnPacket"/>
    ///   · 写线程    —— 专用 Thread，解析地址 + 编码 + 写封包流
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

        private NamedPipeClientStream _ctl, _pkt, _evt;
        private Thread _writerThread, _controlThread, _heartbeatThread;

        private volatile bool _running;
        private volatile bool _hookInstalled;

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
        public static void Attach(string sessionId, int connectTimeoutMs)
        {
            lock (_instanceGate)
            {
                if (_instance == null)
                {
                    _instance = new WpeCore();
                }

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

            _ctl.Connect(connectTimeoutMs);
            _pkt.Connect(connectTimeoutMs);
            _evt.Connect(connectTimeoutMs);

            //装配成 IHookHost：从这一句起，Operate 里所有的封包 / 日志 / 仓库出口都改道到管道。
            HookHost.Attach(this);

            _shutdown.Reset();

            _running = true;
            _lastPingTick = Environment.TickCount;

            _writerThread = StartThread(WriterLoop, "WPE-IPC-Writer");
            _controlThread = StartThread(ControlLoop, "WPE-IPC-Control");
            _heartbeatThread = StartThread(HeartbeatLoop, "WPE-IPC-Heartbeat");
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

            //不 Join：控制线程正阻塞在管道 Read 上，要等到管道关掉才回得来。
            //三条线程都是 IsBackground，进程退出时不会被它们拖住。
            _writerThread = null;
            _controlThread = null;
            _heartbeatThread = null;
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

            //极速模式下 PacketInfo_ToQueue 本来就不入队（核过），这里对齐。
            if (Operate.SystemConfig.SpeedMode) { return; }

            _ring.Enqueue(new PendingPacket
            {
                Id = Interlocked.Increment(ref _packetSeq),
                TimeTicks = packetTime.Ticks,
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

        private void SendEvent(IpcWriter w)
        {
            Stream s = _evt;
            if (s == null || !_running) { return; }

            try
            {
                lock (_evtGate) { IpcFrame.Write(s, w.ToArray()); }
            }
            catch
            {
                /*
                    ⚠️ 这里绝对不能再调 Operate.DoLog —— 那会转回 OnLog、再写一次事件流、
                    再抛一次，直接无限递归把栈打爆。管道断了本来就有心跳去处理。
                */
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
                            p.Id, p.TimeTicks, p.Socket,
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

                byte[] reply;
                try { reply = Dispatch(req); }
                catch (Exception ex) { reply = Fail(ex.Message); }

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
                        w.I32(System.Diagnostics.Process.GetCurrentProcess().Id);
                        w.Bool(IntPtr.Size == 8);
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
                            default: return Fail("阶段 1 还不认这类快照: " + kind);
                        }

                        return Ok();
                    }

                case IpcCommand.StartHook:
                    if (!_hookInstalled)
                    {
                        EnsureWinsockLoaded();
                        _hook.StartHook();
                        _hookInstalled = true;

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

                case IpcCommand.Detach:
                    //先把应答发出去，再由控制线程收尾（不然外壳等不到回复）
                    ThreadPool.QueueUserWorkItem(_ => { Thread.Sleep(50); Detach(); });
                    return Ok();

                default:
                    return Fail("阶段 1 还不认这个命令: " + cmd);
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
        /// <c>LocalHook.GetProcAddress</c> 要求模块<b>已经加载进本进程</b>，否则抛
        /// DllNotFoundException —— 而 <c>StartHook</c> 把异常吞进日志了，
        /// 表现是「钩子装上了却一条都抓不到」。
        ///
        /// 【为什么无头路径特别需要这一句】
        /// 老路径在目标里跑整套 WinForms + SQLite + OWIN，那一堆东西顺手就把
        /// ws2_32 / mswsock 带进来了，所以从来没人撞上。无头核心什么都不加载，
        /// 于是「挂起启动」的目标在装钩那一刻<b>连 ws2_32 都还没有</b>
        /// （进程挂着，加载器根本没跑），13 个钩子一个都装不上，
        /// 唤醒之后目标一路裸奔 —— 实测就是 0 条包。
        ///
        /// 这三个都是系统 DLL，目标反正马上就要用；提前拉进来正是
        /// 「钩子先就位、第一个包也抓得到」的前提。<b>不引入任何 WPE 的依赖。</b>
        /// </summary>
        private void EnsureWinsockLoaded()
        {
            try
            {
                if (Operate.PacketConfig.Packet.Support_WS2) { LoadLibrary("ws2_32.dll"); }
                if (Operate.PacketConfig.Packet.Support_WS1) { LoadLibrary("wsock32.dll"); }
                if (Operate.PacketConfig.Packet.Support_MsWS) { LoadLibrary("mswsock.dll"); }
            }
            catch (Exception ex)
            {
                SendFatal("预加载 winsock 失败: " + ex.Message);
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
