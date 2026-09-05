using EasyHook;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace WinsockPacketEditor.Ipc
{
    #region//外壳侧：注入 + 三条管道 + 把帧还原成 PacketInfo

    /// <summary>
    /// 外壳（WPEHybrid）这一半：建管道 → 注入 → 握手 → 推快照 →
    /// 收封包流与事件流 → 发命令。
    ///
    /// 【故障隔离，方案 3.5】
    ///   · 目标崩 / 退出 → 管道 EOF → 标「已断开」，<b>已抓的数据、配置、界面全部保留</b>（解决 R1）
    ///   · 外壳崩       → 目标心跳超时自行卸钩休眠，目标恢复原生行为
    ///   · 外壳重启     → 对同一 PID 再注一次即可（解决 R6）
    /// </summary>
    public sealed class ShellLink : IDisposable
    {
        #region//状态

        public enum LinkState
        {
            /// <summary>还没注入。</summary>
            Idle,

            /// <summary>注入了，还没等到 Hello。</summary>
            Attaching,

            /// <summary>握上手了，正常工作。</summary>
            Attached,

            /// <summary>目标没了 / 管道断了。数据全部保留。</summary>
            Disconnected,
        }

        private volatile LinkState _state = LinkState.Idle;
        public LinkState State { get { return _state; } }

        /// <summary>目标进程 PID（Hello 带回来的，与注入时那个应当一致）。</summary>
        public int TargetPid { get; private set; }

        /// <summary>目标是不是 64 位。32 位目标走 EasyHook 的 Svc 辅助进程，与老路径一样。</summary>
        public bool TargetIs64 { get; private set; }

        /// <summary>目标侧报上来的累计丢包数（环满丢旧包）。界面上要显示「丢弃 N」。</summary>
        public long Dropped { get; private set; }

        /// <summary>目标侧的钩子装没装上。</summary>
        public bool HookInstalled { get; private set; }

        /// <summary>状态变化时通知外壳（换线程后再动界面）。</summary>
        public event Action<LinkState> StateChanged;

        #endregion

        #region//字段

        private readonly string _sessionId = Guid.NewGuid().ToString("N");

        private NamedPipeServerStream _ctl, _pkt, _evt;
        private Thread _pktThread, _evtThread, _heartbeatThread;
        private volatile bool _running;

        /// <summary>控制通道是「一问一答」，同一时刻只能有一个在途请求。</summary>
        private readonly object _ctlGate = new object();

        /// <summary>CreateAndInject 是挂起启动的，要等「开始拦截」才唤醒（保住老路径的语义）。</summary>
        private bool _needWakeUp;

        #endregion

        #region//注入

        /// <summary>
        /// 建三条管道 → 注入 → 等目标连上来 → 握手。
        /// </summary>
        /// <param name="pid">已运行的进程 PID；小于 0 表示走 <paramref name="path"/> 挂起启动。</param>
        /// <param name="path">挂起启动用的可执行文件路径。</param>
        /// <param name="timeoutMs">等目标连上来的超时。</param>
        public void Attach(int pid, string path, int timeoutMs)
        {
            Dispose();

            _ctl = IpcProtocol.CreateServer(IpcProtocol.ControlPipe(_sessionId), PipeDirection.InOut, 64 * 1024);
            _pkt = IpcProtocol.CreateServer(IpcProtocol.PacketPipe(_sessionId), PipeDirection.In, 1024 * 1024);
            _evt = IpcProtocol.CreateServer(IpcProtocol.EventPipe(_sessionId), PipeDirection.In, 64 * 1024);

            //三条 WaitForConnection 都要先挂起来 —— 目标那头是<b>依次</b> Connect 的，
            //等第一条连上再去建第二条，中间那一小段目标会连不上直接失败。
            IAsyncResult aCtl = _ctl.BeginWaitForConnection(null, null);
            IAsyncResult aPkt = _pkt.BeginWaitForConnection(null, null);
            IAsyncResult aEvt = _evt.BeginWaitForConnection(null, null);

            SetState(LinkState.Attaching);

            string dll = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(typeof(Operate).Assembly.Location),
                Operate.SystemConfig.WPE64_DLL);

            var ip = new Operate.SystemConfig.InjectionParameters
            {
                Mode = Operate.SystemConfig.InjectMode.Headless,
                SessionId = _sessionId,
                DataBasePath = null,   //无头路径不开库
            };

            _needWakeUp = false;

            if (pid > -1)
            {
                RemoteHooking.Inject(pid, dll, dll, "WPE64", ip);
                TargetPid = pid;
            }
            else
            {
                int newPid;
                RemoteHooking.CreateAndInject(path, string.Empty, 0, dll, dll, out newPid, "WPE64", ip);
                TargetPid = newPid;

                //⚠️ 目标是<b>挂起</b>创建的。唤醒推迟到「开始拦截」，
                //这样第一个包也抓得到 —— 老路径里这一句在 PacketList.Start_Hook()。
                _needWakeUp = true;
            }

            if (!WaitAll(timeoutMs, aCtl, aPkt, aEvt))
            {
                Dispose();
                throw new TimeoutException(
                    "注入之后 " + timeoutMs + "ms 内目标没有连上管道。" +
                    "（老路径里这种情况是完全静默的，现在会明确报出来）");
            }

            _ctl.EndWaitForConnection(aCtl);
            _pkt.EndWaitForConnection(aPkt);
            _evt.EndWaitForConnection(aEvt);

            Handshake();

            _running = true;
            _pktThread = StartThread(PacketLoop, "WPE-Shell-Packet");
            _evtThread = StartThread(EventLoop, "WPE-Shell-Event");
            _heartbeatThread = StartThread(HeartbeatLoop, "WPE-Shell-Heartbeat");

            SetState(LinkState.Attached);

            //连上就把全量快照推下去（方案 3.5：显示已附加，推全量快照）
            PushHookFlags();
            PushFilters();
            PushRuntime();
        }

        private static bool WaitAll(int timeoutMs, params IAsyncResult[] rs)
        {
            int deadline = unchecked(Environment.TickCount + timeoutMs);

            foreach (IAsyncResult r in rs)
            {
                int left = unchecked(deadline - Environment.TickCount);
                if (left <= 0) { return false; }
                if (!r.AsyncWaitHandle.WaitOne(left)) { return false; }
            }

            return true;
        }

        private void Handshake()
        {
            var w = new IpcWriter();
            w.U8((byte)IpcCommand.Hello);
            w.I32(IpcProtocol.Version);

            IpcReader r = Call(w);
            var status = (IpcStatus)r.U8();

            if (status == IpcStatus.VersionMismatch)
            {
                int theirs = r.I32();
                throw new InvalidOperationException(
                    "IPC 协议版本对不上：外壳 " + IpcProtocol.Version + "，目标 " + theirs +
                    "。目标里加载的是旧的 WPEHook.dll —— 换句话说 Release 目录里那份没跟着更新。");
            }

            if (status != IpcStatus.Ok) { throw new InvalidOperationException("握手失败"); }

            r.I32();                     //目标的协议版本，已确认一致
            TargetPid = r.I32();
            TargetIs64 = r.Bool();
        }

        private static Thread StartThread(ThreadStart body, string name)
        {
            var t = new Thread(body);
            t.IsBackground = true;
            t.Name = name;
            t.Start();
            return t;
        }

        #endregion

        #region//命令（控制通道，一问一答）

        private IpcReader Call(IpcWriter w)
        {
            lock (_ctlGate)
            {
                Stream s = _ctl;
                if (s == null) { throw new InvalidOperationException("控制管道没建立"); }

                IpcFrame.Write(s, w.ToArray());
                byte[] reply = IpcFrame.Read(s, IpcProtocol.MaxControlFrame);

                if (reply == null) { throw new IOException("目标已断开"); }
                return new IpcReader(reply);
            }
        }

        private void CallVoid(IpcWriter w)
        {
            IpcReader r = Call(w);
            var status = (IpcStatus)r.U8();

            if (status != IpcStatus.Ok)
            {
                throw new InvalidOperationException("目标侧报错: " + r.Str());
            }
        }

        /// <summary>开始拦截。挂起启动的目标在应答之后才唤醒。</summary>
        public void StartHook()
        {
            var w = new IpcWriter();
            w.U8((byte)IpcCommand.StartHook);
            CallVoid(w);

            if (_needWakeUp)
            {
                //顺序要紧：钩子装好了才唤醒，第一个包才抓得到（老路径的语义）
                RemoteHooking.WakeUpProcess();
                _needWakeUp = false;
            }
        }

        public void StopHook()
        {
            var w = new IpcWriter();
            w.U8((byte)IpcCommand.StopHook);
            CallVoid(w);
        }

        /// <summary>发一个封包（封包编辑器 / 快捷键 / 递进发送）。</summary>
        public bool SendPacket(int socket, Operate.PacketConfig.Packet.PacketType type, string from, string to, byte[] bytes)
        {
            var w = new IpcWriter();
            w.U8((byte)IpcCommand.SendPacket);
            w.I32(socket);
            w.I32((int)type);
            w.Str(from);
            w.Str(to);
            w.Bytes(bytes);

            IpcReader r = Call(w);
            if ((IpcStatus)r.U8() != IpcStatus.Ok) { return false; }
            return r.Bool();
        }

        /// <summary>取某个套接字的本机 / 远端地址（"1.2.3.4:5678"）。</summary>
        public void GetSocketInfo(int socket, out string from, out string to)
        {
            var w = new IpcWriter();
            w.U8((byte)IpcCommand.GetSocketInfo);
            w.I32(socket);

            IpcReader r = Call(w);
            if ((IpcStatus)r.U8() != IpcStatus.Ok) { from = null; to = null; return; }

            from = r.Str();
            to = r.Str();
        }

        public void Detach()
        {
            try
            {
                var w = new IpcWriter();
                w.U8((byte)IpcCommand.Detach);
                CallVoid(w);
            }
            catch { /* 目标已经没了也算 Detach 成功 */ }

            Dispose();
        }

        #endregion

        #region//配置快照下推

        private void PushConfig(ConfigKind kind, byte[] payload)
        {
            var w = new IpcWriter();
            w.U8((byte)IpcCommand.SetConfig);
            w.U8((byte)kind);
            w.Bytes(payload);
            CallVoid(w);
        }

        public void PushHookFlags() { PushConfig(ConfigKind.HookFlags, ConfigSnapshot.EncodeHookFlags()); }
        public void PushFilters() { PushConfig(ConfigKind.Filters, ConfigSnapshot.EncodeFilters()); }
        public void PushRuntime() { PushConfig(ConfigKind.Runtime, ConfigSnapshot.EncodeRuntime()); }

        /// <summary>
        /// 推快照时把异常吞掉的版本 —— 给「配置一改就顺手推一次」那些调用点用。
        /// 目标已经断开时不该让一次界面操作抛出来。
        /// </summary>
        public void TryPush(Action push)
        {
            if (_state != LinkState.Attached) { return; }
            try { push(); } catch { /* 断开了，下次连上会推全量 */ }
        }

        #endregion

        #region//封包流 → cqPacketInfo

        private void PacketLoop()
        {
            while (_running)
            {
                byte[] frame;

                try { frame = IpcFrame.Read(_pkt, IpcProtocol.MaxPacketFrame); }
                catch { frame = null; }

                if (frame == null) { OnPipeDown(); return; }

                try { Ingest(PacketFrame.Decode(frame)); }
                catch (Exception ex) { Operate.DoLog("ShellLink.Ingest", ex); }
            }
        }

        /// <summary>
        /// 把一帧还原成 <c>PacketInfo</c> 塞进 <c>cqPacketInfo</c>。
        ///
        /// 【切口就在这一个方法】进了队列之后，<c>FlushToFeed</c> / DTO / <c>PacketRow</c> /
        /// 前端 <c>stores/packets.ts</c> <b>一行都不用改</b> —— 与代理数据页走的是同一条下游。
        ///
        /// 归属地在这里查（方案：QQWry 留在外壳）。<c>GetIPLocation</c> 是异步且带记忆化的，
        /// 26 MB 的库与每包一次查询从此不再占目标进程的线程池（解决 R8）。
        /// </summary>
        private async void Ingest(PacketFrame.Decoded d)
        {
            try
            {
                var type = (Operate.PacketConfig.Packet.PacketType)d.PacketType;
                var action = (Operate.FilterConfig.Filter.FilterAction)d.FilterAction;

                //统计：外壳按<b>收到的</b>计。与目标侧计数的差别就是被环丢掉的那些，
                //而那些界面上另有「丢弃 N」在显示，不会无声消失。
                Operate.PacketConfig.Packet.CountPacketInfo(type, d.Modified.Length);

                string fromLoc = await Operate.SystemConfig.GetIPLocation(d.From.Split(':')[0]);
                string toLoc = await Operate.SystemConfig.GetIPLocation(d.To.Split(':')[0]);

                var pi = new PacketInfo(
                    new DateTime(d.TimeTicks),
                    (int)d.Socket,
                    type,
                    d.From,
                    fromLoc,
                    d.To,
                    toLoc,
                    d.Raw,
                    d.Modified,
                    Operate.PacketConfig.Packet.GetPacketData_Hex(d.Modified, Operate.PacketConfig.Packet.PacketData_MaxLen),
                    d.Modified.Length,
                    action);

                Operate.PacketConfig.Queue.cqPacketInfo.Enqueue(pi);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Ingest), ex);
            }
        }

        #endregion

        #region//事件流

        private void EventLoop()
        {
            while (_running)
            {
                byte[] frame;

                try { frame = IpcFrame.Read(_evt, IpcProtocol.MaxControlFrame); }
                catch { frame = null; }

                if (frame == null) { OnPipeDown(); return; }

                try { HandleEvent(frame); }
                catch (Exception ex) { Operate.DoLog("ShellLink.HandleEvent", ex); }
            }
        }

        private void HandleEvent(byte[] frame)
        {
            var r = new IpcReader(frame);
            var kind = (IpcEvent)r.U8();

            switch (kind)
            {
                case IpcEvent.Log:
                    //目标侧的日志进外壳的系统日志，与本地日志混在一起 ——
                    //加个前缀好认出来是从目标里来的
                    Operate.DoLog("[目标] " + r.Str(), r.Str());
                    break;

                case IpcEvent.FilterLog:
                    Operate.DoFilterLog(
                        r.Str(),
                        (Operate.FilterConfig.Filter.FilterAction)r.I32(),
                        r.I32(),
                        (Operate.PacketConfig.Packet.PacketType)r.I32(),
                        r.I32());
                    break;

                case IpcEvent.StoreAdded:
                    {
                        //目标不再持有仓库，落库在这一侧
                        Guid wid = r.Guid_();
                        byte[] bytes = r.Bytes();
                        WareHouseInfo whi = Operate.WareHouseConfig.WareHouse.GetWareHouse_ByGuid(wid);
                        if (whi != null) { Operate.WareHouseConfig.WareHouse.AddStores(whi.Stores, bytes); }
                        break;
                    }

                case IpcEvent.Dropped:
                    Dropped = r.I64();
                    break;

                case IpcEvent.HookState:
                    HookInstalled = r.Bool();
                    break;

                case IpcEvent.Fatal:
                    Operate.DoLog("[目标] Fatal", r.Str());
                    break;

                default:
                    //阶段 2 才有的事件；忽略而不是报错，这样新目标配旧外壳也不会炸
                    break;
            }
        }

        #endregion

        #region//心跳

        private void HeartbeatLoop()
        {
            while (_running)
            {
                Thread.Sleep(1000);
                if (!_running) { return; }

                try
                {
                    var w = new IpcWriter();
                    w.U8((byte)IpcCommand.Ping);
                    CallVoid(w);
                }
                catch
                {
                    OnPipeDown();
                    return;
                }
            }
        }

        #endregion

        #region//断开与释放

        /// <summary>
        /// 管道断了。<b>只改状态，不清任何数据</b> ——
        /// 已抓的封包、配置、界面全部保留，用户还能继续看、继续导出（解决 R1）。
        /// </summary>
        private void OnPipeDown()
        {
            if (!_running) { return; }
            _running = false;
            HookInstalled = false;
            SetState(LinkState.Disconnected);
        }

        private void SetState(LinkState s)
        {
            _state = s;
            Action<LinkState> h = StateChanged;
            if (h != null) { try { h(s); } catch { } }
        }

        public void Dispose()
        {
            _running = false;

            SafeDispose(ref _ctl);
            SafeDispose(ref _pkt);
            SafeDispose(ref _evt);

            _pktThread = null;
            _evtThread = null;
            _heartbeatThread = null;
        }

        private static void SafeDispose<T>(ref T s) where T : class, IDisposable
        {
            T local = s;
            s = null;
            if (local == null) { return; }
            try { local.Dispose(); } catch { }
        }

        #endregion
    }

    #endregion
}
