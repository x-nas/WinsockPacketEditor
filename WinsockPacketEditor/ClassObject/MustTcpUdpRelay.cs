using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 「强制转代理」里 UDP 那一路的客户端：把被驱动截下来的 UDP 数据报经 SOCKS5 UDP 中继送出去，
    /// 再把中继回来的应答交还给目标进程。
    ///
    /// 【一个 UDP 会话一条关联】
    /// 键是 SunnyNet 的 TheologyID（目标进程的那一个 UDP 套接字）。同一个套接字发往任何远端的数据报
    /// 都走同一条 SOCKS5 UDP 关联 —— 关联本身不绑远端，SOCKS5 的 UDP 头里带着目标地址，
    /// 应答的 UDP 头里带着来源地址；应答该交给谁由关联自己知道，不需要再维护一张「远端 → 会话」的表。
    ///
    /// 【为什么要重写】
    /// 旧写法是每个数据报新开一条 TCP 握手 + UDP ASSOCIATE，发完 <c>using</c> 就把 TCP 与 UdpClient 一起关掉：
    /// 接收循环刚起来就撞上 ObjectDisposedException（被静默吞掉），应答永远回不到目标进程；
    /// 而 TCP 一断，按 RFC 1928 服务端也该收回那条关联。UDP 强制转代理因此从来没有真正通过。
    ///
    /// 【生命周期】
    /// 控制用的 TCP 连接<b>一直开着</b>（RFC 1928：TCP 断开即关联终止），另挂一个 1 字节的读等着它 ——
    /// 服务端主动断开（超时 / 停止服务）时这边跟着收掉。
    /// 目标进程关闭套接字（UDP_Closed 事件）→ <see cref="Close(long)"/>；
    /// 静置超过 <see cref="IdleTimeout"/> → <see cref="SweepIdle"/>（挂在 1 秒统计拍上，与服务端 CloseUDPTimeOut 同一处）；
    /// 停止代理 / 换配置 → <see cref="CloseAll"/>。
    ///
    /// 【线程】
    /// 入口都是无锁的 ConcurrentDictionary；同一个会话的第一批数据报并发到达时，
    /// 靠 <c>GetOrAdd</c> 的 Task 保证只建一条关联，其余的等它建好再发。建立失败的 Task 会被移除，
    /// 下一个数据报重试。发送与接收各走各的 socket 调用，不需要额外的锁。
    /// </summary>
    public sealed class MustTcpUdpRelay
    {
        /// <summary>SOCKS5 服务器与凭据 —— 与 Operate.ProxyConfig.Proxy.MustTCP_* 那组字段一一对应。</summary>
        public sealed class Target
        {
            public bool Auth;
            public string IP = "127.0.0.1";
            public ushort Port = 1080;
            public string UserName = string.Empty;
            public string PassWord = string.Empty;
        }

        private sealed class Association : IDisposable
        {
            public long Theology;
            public Socket Control;              //UDP ASSOCIATE 用的 TCP 连接，活着关联才在
            public UdpClient Udp;               //本地这一头的 UDP 套接字
            public IPEndPoint Relay;            //服务端给的中继地址
            public long LastActiveTicks;
            public int Closed;                  //0 = 活着，1 = 已关（Interlocked）
            public long Sent, Received;         //统计用，跑测也看它

            public void Touch() { Volatile.Write(ref LastActiveTicks, DateTime.UtcNow.Ticks); }

            public void Dispose()
            {
                if (Interlocked.Exchange(ref Closed, 1) != 0) { return; }

                try { Udp?.Close(); } catch { }
                try { Control?.Shutdown(SocketShutdown.Both); } catch { }
                try { Control?.Close(); } catch { }
            }
        }

        private readonly ConcurrentDictionary<long, Task<Association>> map = new ConcurrentDictionary<long, Task<Association>>();
        private readonly Func<Target> target;
        private readonly Action<long, byte[]> deliver;
        private readonly Action<string, string> log;

        /// <summary>静置多久没有收发就回收这条关联。默认与服务端的 UDPTimeout（3 分钟）一致。</summary>
        public TimeSpan IdleTimeout = TimeSpan.FromMinutes(3);

        /// <summary>当前活着的关联数（跑测与统计用）。</summary>
        public int Count
        {
            get
            {
                int n = 0;
                foreach (var kv in map) { if (kv.Value.Status == TaskStatus.RanToCompletion && Volatile.Read(ref kv.Value.Result.Closed) == 0) { n++; } }
                return n;
            }
        }

        /// <param name="Target">每次建关联时取一次代理设置 —— 用户改了设置不用重建整个中继器（已有的关联仍按旧设置跑到自己结束）。</param>
        /// <param name="Deliver">把应答交还给目标进程：(TheologyID, 数据)。生产上是 SunnyNet 的 UDPTools.SendMessage，跑测里是个收集器。</param>
        /// <param name="Log">日志出口，可为 null。</param>
        public MustTcpUdpRelay(Func<Target> Target, Action<long, byte[]> Deliver, Action<string, string> Log)
        {
            this.target = Target ?? throw new ArgumentNullException(nameof(Target));
            this.deliver = Deliver ?? throw new ArgumentNullException(nameof(Deliver));
            this.log = Log ?? ((w, t) => { });
        }

        #region//发送

        /// <summary>
        /// 把一个数据报经中继送到 TargetEndPoint。没有关联就先建一条；建立失败返回 false（这个数据报丢掉，下一个会重试）。
        /// </summary>
        public async Task<bool> SendAsync(long Theology, IPEndPoint TargetEndPoint, byte[] Data)
        {
            if (TargetEndPoint == null || Data == null) { return false; }

            Association a;

            try
            {
                a = await GetOrCreateAsync(Theology).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                map.TryRemove(Theology, out _);
                log(nameof(MustTcpUdpRelay) + ".Establish", ex.Message);
                return false;
            }

            if (a == null || Volatile.Read(ref a.Closed) != 0)
            {
                map.TryRemove(Theology, out _);
                return false;
            }

            byte[] frame = Operate.ProxyConfig.Proxy.CreatSocks5UdpRequest(Data, TargetEndPoint);
            if (frame == null) { return false; }

            try
            {
                int n = await a.Udp.SendAsync(frame, frame.Length, a.Relay).ConfigureAwait(false);
                a.Touch();
                Interlocked.Increment(ref a.Sent);
                return n == frame.Length;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (Exception ex)
            {
                log(nameof(MustTcpUdpRelay) + ".Send", ex.Message);
                Close(Theology);
                return false;
            }
        }

        #endregion

        #region//建立关联

        private Task<Association> GetOrCreateAsync(long Theology)
        {
            //GetOrAdd 的 valueFactory 可能被并发调用两次，但只有一个 Task 进表；
            //另一个 Task 也会跑（多建一条关联）—— 用 Lazy 把「建」这件事也收成一次。
            var lazy = new Lazy<Task<Association>>(() => EstablishAsync(Theology), LazyThreadSafetyMode.ExecutionAndPublication);
            Task<Association> task = map.GetOrAdd(Theology, _ => lazy.Value);

            //表里那条已经死了（服务端断开 / 超时收掉）就换一条新的
            if (task.Status == TaskStatus.RanToCompletion && Volatile.Read(ref task.Result.Closed) != 0)
            {
                map.TryRemove(Theology, out _);
                return GetOrCreateAsync(Theology);
            }

            return task;
        }

        private async Task<Association> EstablishAsync(long Theology)
        {
            Target t = target();
            var control = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                byte[] bCommand = new byte[]
                {
                    0x05, // SOCKS version 5
                    0x03, // UDP ASSOCIATE
                    0x00, // Reserved
                    0x01, // IPv4
                    0x00, 0x00, 0x00, 0x00, // 0.0.0.0：这边还不知道会从哪个端口发
                    0x00, 0x00
                };

                var r = await Operate.ProxyConfig.Proxy.EstablishSocksProxyServer(
                    control, t.Auth, t.IP, t.Port, t.UserName, t.PassWord, bCommand).ConfigureAwait(false);

                if (!r.Success)
                {
                    throw new InvalidOperationException(string.IsNullOrEmpty(r.Error) ? "UDP ASSOCIATE failed" : r.Error);
                }

                IPEndPoint relay = Operate.ProxyConfig.Proxy.ParseEstablishResponse(r.Response);
                if (relay == null)
                {
                    throw new InvalidOperationException("UDP ASSOCIATE: bad bind address");
                }

                //服务端回 0.0.0.0 是常见写法（「用你连我的那个地址」），换成控制连接对端的地址
                if (relay.Address.Equals(IPAddress.Any) || relay.Address.Equals(IPAddress.IPv6Any))
                {
                    relay = new IPEndPoint(((IPEndPoint)control.RemoteEndPoint).Address, relay.Port);
                }

                var a = new Association
                {
                    Theology = Theology,
                    Control = control,
                    Udp = new UdpClient(new IPEndPoint(IPAddress.Any, 0)),
                    Relay = relay,
                };
                a.Touch();

                //两条循环各自持有关联：一条收应答，一条盯着控制连接有没有被服务端断开
                _ = Task.Run(() => ReceiveLoopAsync(a));
                _ = Task.Run(() => WatchControlAsync(a));

                return a;
            }
            catch
            {
                try { control.Close(); } catch { }
                throw;
            }
        }

        #endregion

        #region//接收

        private async Task ReceiveLoopAsync(Association a)
        {
            try
            {
                while (Volatile.Read(ref a.Closed) == 0)
                {
                    UdpReceiveResult r = await a.Udp.ReceiveAsync().ConfigureAwait(false);

                    //只认中继地址发来的（同一台机器上别的东西不该往这个随机端口发）
                    if (!r.RemoteEndPoint.Address.Equals(a.Relay.Address) || r.RemoteEndPoint.Port != a.Relay.Port)
                    {
                        continue;
                    }

                    var (data, _) = Operate.ProxyConfig.Proxy.ParseSocks5UdpResponse(r.Buffer, r.Buffer.Length);
                    if (data == null || data.Length == 0) { continue; }

                    a.Touch();
                    Interlocked.Increment(ref a.Received);

                    try { deliver(a.Theology, data); }
                    catch (Exception ex) { log(nameof(MustTcpUdpRelay) + ".Deliver", ex.Message); }
                }
            }
            catch (ObjectDisposedException) { }
            catch (SocketException) { }          //关掉时 ReceiveAsync 抛的是这个（WSACancelBlockingCall / 10004）
            catch (Exception ex)
            {
                log(nameof(MustTcpUdpRelay) + ".Receive", ex.Message);
            }
            finally
            {
                Drop(a);
            }
        }

        private async Task WatchControlAsync(Association a)
        {
            byte[] one = new byte[1];

            try
            {
                while (Volatile.Read(ref a.Closed) == 0)
                {
                    int n = await a.Control.ReceiveAsync(new ArraySegment<byte>(one), SocketFlags.None).ConfigureAwait(false);
                    if (n <= 0) { break; }   //对端关闭 → 关联结束
                }
            }
            catch { }
            finally
            {
                Drop(a);
            }
        }

        #endregion

        #region//回收

        /// <summary>目标进程关了这个 UDP 套接字（SunnyNet 的 UDP_Closed 事件）。</summary>
        public void Close(long Theology)
        {
            if (map.TryRemove(Theology, out Task<Association> t) && t.Status == TaskStatus.RanToCompletion)
            {
                t.Result.Dispose();
            }
        }

        /// <summary>静置超时的关联收掉。挂在 1 秒统计拍上；每次最多扫一遍表，几十条关联是微秒级。</summary>
        public int SweepIdle()
        {
            long deadline = DateTime.UtcNow.Ticks - IdleTimeout.Ticks;
            int n = 0;

            foreach (var kv in map)
            {
                if (kv.Value.Status != TaskStatus.RanToCompletion) { continue; }
                Association a = kv.Value.Result;

                if (Volatile.Read(ref a.LastActiveTicks) < deadline)
                {
                    Drop(a);
                    n++;
                }
            }

            return n;
        }

        /// <summary>停止代理 / 换配置时全部收掉。</summary>
        public void CloseAll()
        {
            foreach (var kv in map)
            {
                if (kv.Value.Status == TaskStatus.RanToCompletion) { kv.Value.Result.Dispose(); }
            }
            map.Clear();
        }

        private void Drop(Association a)
        {
            a.Dispose();

            //只删「还是它」的那一条 —— 同一个 Theology 可能已经换上新关联
            if (map.TryGetValue(a.Theology, out Task<Association> t) && t.Status == TaskStatus.RanToCompletion && ReferenceEquals(t.Result, a))
            {
                ((ICollection<KeyValuePair<long, Task<Association>>>)map).Remove(new KeyValuePair<long, Task<Association>>(a.Theology, t));
            }
        }

        #endregion

        #region//统计（跑测用）

        /// <summary>某条关联的收发计数；不存在返回 (-1, -1)。</summary>
        public (long Sent, long Received) StatsOf(long Theology)
        {
            if (map.TryGetValue(Theology, out Task<Association> t) && t.Status == TaskStatus.RanToCompletion)
            {
                return (Volatile.Read(ref t.Result.Sent), Volatile.Read(ref t.Result.Received));
            }
            return (-1, -1);
        }

        #endregion
    }
}
