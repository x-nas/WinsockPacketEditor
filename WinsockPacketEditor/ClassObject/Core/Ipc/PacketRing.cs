using System;
using System.Collections.Generic;
using System.Threading;

namespace WinsockPacketEditor.Ipc
{
    #region//排队等着发出去的一个封包

    /// <summary>
    /// 钩子线程放进环里的东西。
    ///
    /// ⚠️ <b>刻意不在钩子线程上编码成帧</b>，也不解析地址：
    /// 那两件事都要格式化字符串、要调 getsockname/getpeername（一次系统调用），
    /// 而钩子线程是目标进程的收发线程 —— 方案第四节第 2 条说得很死：
    /// 那上面只允许「拷贝缓冲 + DoFilterList + 一次无锁入环」。
    /// 地址解析与编码全部推到写线程（它也在目标进程里，句柄一样能用）。
    /// </summary>
    public sealed class PendingPacket
    {
        public long Id;
        public long TimeTicks;
        public int Socket;
        public Operate.PacketConfig.Packet.PacketType PacketType;
        public Operate.FilterConfig.Filter.FilterAction FilterAction;
        public Operate.PacketConfig.Packet.SockAddr SockAddr;
        public byte[] Raw;
        public byte[] Modified;

        /// <summary>估个大小，给环的字节上限用。两份字节 + 一点固定开销。</summary>
        public int Size
        {
            get
            {
                int n = 64;
                if (Raw != null) { n += Raw.Length; }
                if (Modified != null && !ReferenceEquals(Modified, Raw)) { n += Modified.Length; }
                return n;
            }
        }
    }

    #endregion

    #region//有界环形缓冲

    /// <summary>
    /// 钩子线程与写线程之间那一格缓冲。
    ///
    /// 【唯一正确的背压方向】环满时<b>丢最旧的那条并计数</b>，
    /// 绝不让 <see cref="Enqueue"/> 等待 —— 它跑在目标进程的收发线程上，
    /// 在那儿等一下，目标的每一次 send() 就跟着卡一下。
    /// 宁可列表里少几条，也不能让目标卡住（方案第四节第 4 条）。
    ///
    /// 无声丢包比阻塞更糟，所以丢了要计数，报给外壳，界面上显示「丢弃 N」。
    ///
    /// 【为什么是 lock 而不是无锁队列】临界区里只有「入队 / 出队一个引用 + 改两个计数」，
    /// 几十纳秒，而且没有任何阻塞调用。真正要避免的是<b>等 I/O</b> 与<b>等对端</b>，
    /// 不是等一个从不被长时间持有的锁。用 ConcurrentQueue 反而做不了「满了丢最旧的」——
    /// 它没有容量上限，也没法从头上摘。
    /// </summary>
    public sealed class PacketRing
    {
        private readonly Queue<PendingPacket> _q = new Queue<PendingPacket>();
        private readonly object _gate = new object();

        private readonly int _maxCount;
        private readonly long _maxBytes;

        private long _bytes;
        private long _dropped;

        /// <summary>写线程在等东西时挂在这上面，避免空转。</summary>
        private readonly ManualResetEventSlim _signal = new ManualResetEventSlim(false);

        /// <param name="maxCount">条数上限。</param>
        /// <param name="maxBytes">字节上限 —— 两个都要卡：只卡条数的话，几万条 4 KB 的包就是几百 MB。</param>
        public PacketRing(int maxCount, long maxBytes)
        {
            _maxCount = maxCount;
            _maxBytes = maxBytes;
        }

        /// <summary>累计丢弃条数。</summary>
        public long Dropped { get { return Interlocked.Read(ref _dropped); } }

        /// <summary>当前积压条数。</summary>
        public int Count { get { lock (_gate) { return _q.Count; } } }

        #region//入队（钩子线程）

        /// <summary>入队。<b>永不阻塞</b>：满了就从头上丢，丢多少计多少。</summary>
        public void Enqueue(PendingPacket item)
        {
            if (item == null) { return; }

            lock (_gate)
            {
                _q.Enqueue(item);
                _bytes += item.Size;

                //先入再挤：这样单条超大的包也进得来（挤完只剩它自己），
                //而不是被「加进去就超了」拦在门外、永远发不出去。
                while ((_q.Count > _maxCount || _bytes > _maxBytes) && _q.Count > 1)
                {
                    PendingPacket old = _q.Dequeue();
                    _bytes -= old.Size;
                    Interlocked.Increment(ref _dropped);
                }
            }

            _signal.Set();
        }

        #endregion

        #region//出队（写线程）

        /// <summary>
        /// 取一批出来。没有东西时最多等 <paramref name="waitMs"/> 毫秒。
        /// 成批取是为了让写线程按批做一次 Write，而不是每条一次系统调用。
        /// </summary>
        public List<PendingPacket> DequeueBatch(int maxCount, long maxBytes, int waitMs)
        {
            if (Count == 0)
            {
                _signal.Reset();

                //Reset 与 Wait 之间可能刚好来了一条，所以 Reset 之后再看一眼。
                //漏掉这一眼的后果是「明明有数据，写线程却睡满一个 waitMs」。
                if (Count == 0) { _signal.Wait(waitMs); }
            }

            var batch = new List<PendingPacket>();
            long taken = 0;

            lock (_gate)
            {
                while (_q.Count > 0 && batch.Count < maxCount && taken < maxBytes)
                {
                    PendingPacket f = _q.Dequeue();
                    _bytes -= f.Size;
                    taken += f.Size;
                    batch.Add(f);
                }
            }

            return batch;
        }

        #endregion

        #region//清空 / 唤醒

        /// <summary>管道断了 / 卸钩时清空。积压的那些已经没有归宿了。</summary>
        public void Clear()
        {
            lock (_gate)
            {
                _q.Clear();
                _bytes = 0;
            }
        }

        /// <summary>让等在 <see cref="DequeueBatch"/> 里的写线程立刻醒过来（退出时用）。</summary>
        public void Wake()
        {
            _signal.Set();
        }

        #endregion
    }

    #endregion
}
