using System;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace WinsockPacketEditor.Ipc
{
    #region//协议常量与管道命名

    /// <summary>
    /// 外壳（WPEHybrid）与目标进程内无头核心之间的协议。
    ///
    /// 【为什么是命名管道】走 NPFS，不经过 ws2_32 ——
    /// <b>天然不会被自己的 13 个钩子钩到</b>。TCP 回环会撞上自己的钩子，
    /// 除非给 IPC 线程单独加 ACL；EasyHook 自带的 IpcCreateServer 是 .NET Remoting，
    /// 序列化重、容易在 UI 线程死锁。两条都否掉了，理由见 INJECT-IPC-PLAN.md 第三节。
    ///
    /// 【为什么不用 JSON】方案里写的是「沿用桥的 RpcMessage 形状」，
    /// 但阶段 3 的目标是让目标进程里只剩 WPEHook + WPECore + EasyHook + System.Memory，
    /// 引一个 Newtonsoft 与那个目标相反。所以控制通道也用手写的二进制 TLV，
    /// 字段逐个写、逐个读，位数无关、无外部依赖。
    /// </summary>
    public static class IpcProtocol
    {
        /// <summary>
        /// 协议版本。两端 Hello 时对不上就直接拒绝，<b>不猜、不兼容</b>。
        /// 改了任何一个帧的字段就要 +1。
        ///
        /// 4（2026-09-10）：Stats 事件末尾加了<b>封包计数那 11 个 long</b>（计数搬回目标，
        ///                  见 <c>WpeCore.OnPacket</c> 那段说明）；ResetStats 命令加了一个
        ///                  <c>u8 掩码</c>（要清哪几组计数）。
        /// </summary>
        public const int Version = 4;

        /// <summary>控制通道单帧上限（1 MB）。快照最大的是滤镜表，几十条 × 几百字节，余量足够。</summary>
        public const int MaxControlFrame = 1024 * 1024;

        /// <summary>封包流单帧上限（16 MB）。单个封包再大也到不了这个量级，纯属防御。</summary>
        public const int MaxPacketFrame = 16 * 1024 * 1024;

        #region//管道名

        /*
            用一个会话 GUID 而不是「外壳 PID + 目标 PID」：
            CreateAndInject 那条路上目标 PID 是<b>调用之后</b>才知道的，
            而管道必须在注入之前就建好并把名字塞进 InjectionParameters。
        */

        public static string ControlPipe(string sessionId) { return "WPE64-" + sessionId + "-ctl"; }
        public static string PacketPipe(string sessionId) { return "WPE64-" + sessionId + "-pkt"; }
        public static string EventPipe(string sessionId) { return "WPE64-" + sessionId + "-evt"; }

        /// <summary>
        /// 建服务端管道（外壳侧）。ACL 只给当前用户 —— 管道那头能调本机的强力方法，
        /// 不能让同机的其它账户连上来。
        /// </summary>
        public static NamedPipeServerStream CreateServer(string name, PipeDirection direction, int bufferSize)
        {
            var security = new PipeSecurity();
            security.AddAccessRule(new PipeAccessRule(
                WindowsIdentity.GetCurrent().User,
                PipeAccessRights.FullControl,
                AccessControlType.Allow));

            return new NamedPipeServerStream(
                name,
                direction,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous,
                bufferSize,
                bufferSize,
                security);
        }

        #endregion
    }

    #endregion

    #region//控制通道的消息类型

    /// <summary>外壳 → 目标。</summary>
    public enum IpcCommand : byte
    {
        /// <summary>握手：协议版本对不上就拒绝。</summary>
        Hello = 1,

        /// <summary>心跳。双向 1 秒；目标连续 3 次收不到就自行卸钩休眠。</summary>
        Ping = 2,

        /// <summary>装 13 个钩子。CreateAndInject 的目标由外壳在收到应答后 WakeUpProcess。</summary>
        StartHook = 3,

        /// <summary>卸钩。</summary>
        StopHook = 4,

        /// <summary>下推一类配置快照（整表，幂等）。</summary>
        SetConfig = 5,

        /// <summary>发一个封包（封包编辑器 / 快捷键 / 递进发送）。</summary>
        SendPacket = 6,

        /// <summary>取某个套接字的本机 / 远端地址。</summary>
        GetSocketInfo = 7,

        /// <summary>
        /// 诊断：把目标进程<b>当前加载了哪些托管程序集与原生模块</b>报回来。
        ///
        /// 这是「阶段 3 到底值不值得做」的量尺 —— 抽 WPECore 的唯一收益就是
        /// 缩小目标里的程序集足迹（风险清单 R2）。先量一次，别凭猜去动一个三万行的类。
        /// </summary>
        GetFootprint = 9,

        /// <summary>
        /// 把统计计数归零。载荷是一个 <c>u8 掩码</c>，见 <see cref="ResetWhat"/>。
        ///
        /// ⚠️ 注入模式下这些计数<b>全都在目标里递增</b>（滤镜六个与每条滤镜的执行次数在
        /// DoFilterList 里、封包那 11 个在 OnPacket 里、发送 / 机器人的执行次数在目标的
        /// BackgroundWorker 里），外壳那份只是随 Stats 事件更新的<b>镜像</b> ——
        /// 只清外壳的，下一拍就被目标盖回去（用户看到的是「数字闪一下又回来了」）。
        /// 所以复位必须发到目标这边来做。
        /// </summary>
        ResetStats = 10,

        /// <summary>卸钩 + 停执行器 + 断管道 + 核心休眠。不卸载 CLR（做不到，也不必要）。</summary>
        Detach = 8,

        //—— 阶段 2 的执行器命令，先占位，实现在阶段 2 ——
        StartSend = 20,
        StartSendList = 22,
        StopSendList = 23,
        StartRobot = 24,
        StartRobotList = 26,
        StopRobotList = 27,
    }

    /// <summary>目标 → 外壳（事件流；控制通道上只走应答）。</summary>
    public enum IpcEvent : byte
    {
        /// <summary>目标侧的 DoLog。目标不再打开任何文件，日志一律发出来。</summary>
        Log = 1,

        /// <summary>滤镜日志。</summary>
        FilterLog = 2,

        /// <summary>计数器包，1 Hz。</summary>
        Stats = 3,

        /// <summary>丢包计数（环满丢旧包）。</summary>
        Dropped = 4,

        /// <summary>钩子装没装上。</summary>
        HookState = 5,

        /// <summary>
        /// 滤镜的「入库」动作。目标不再持有仓库，只把「哪个仓库 + 哪段字节」送出来，
        /// 由外壳 AddStores 落库。
        /// </summary>
        StoreAdded = 6,

        /// <summary>目标侧出了不可恢复的错。</summary>
        Fatal = 7,
    }

    /// <summary>
    /// <see cref="IpcCommand.ResetStats"/> 要清哪几组计数（可以按位或）。
    ///
    /// 分组而不是「一个命令清所有」：界面上这几处入口的语义各不相同 ——
    /// 统计数据页的「归零」只清滤镜那一组（封包总数与流量不动，那是数据页的仪表），
    /// 数据页的「清空」清封包计数，发送 / 机器人列表各自的「重置计数」只清自己那一列。
    /// </summary>
    [Flags]
    public enum ResetWhat : byte
    {
        /// <summary>滤镜那六个全局计数 + 每条滤镜的 ExecutionCount。</summary>
        FilterStats = 1,

        /// <summary>封包计数（TotalPackets / 八个方向 / 收发字节）。</summary>
        PacketCounters = 2,

        /// <summary>每条发送的执行次数 / 成功 / 失败。</summary>
        SendCounts = 4,

        /// <summary>每个机器人的执行次数。</summary>
        RobotCounts = 8,
    }

    /// <summary>配置快照的类别（见 INJECT-IPC-PLAN.md 第 3.3 节）。</summary>
    public enum ConfigKind : byte
    {
        /// <summary>12 个 HookWS* 开关 + 三个 Support_*。</summary>
        HookFlags = 1,

        /// <summary>滤镜列表整表。</summary>
        Filters = 2,

        /// <summary>极速模式、系统套接字、执行方式、当前选中封包的字节。</summary>
        Runtime = 3,

        //—— 阶段 2 ——
        Sends = 4,
        Robots = 5,
    }

    /// <summary>应答的结果码。</summary>
    public enum IpcStatus : byte
    {
        Ok = 0,
        Error = 1,

        /// <summary>协议版本对不上。</summary>
        VersionMismatch = 2,
    }

    #endregion

    #region//帧的读写（长度前缀）

    /// <summary>
    /// 控制 / 事件通道的分帧：<c>u32 长度</c> + 载荷。载荷第一个字节是消息类型。
    ///
    /// ⚠️ <b>必须自己分帧</b>。管道是<c>字节流</c>模式（PipeTransmissionMode.Byte），
    /// 一次 Read 拿到的既可能是半条消息、也可能是两条半 —— 按「读回来多少就是一条」处理
    /// 在本机小消息上能蒙对九成，然后在某次 GC 停顿后随机崩掉。
    /// </summary>
    public static class IpcFrame
    {
        /// <summary>把一段载荷按「长度前缀 + 内容」写出去。一次 Write，不拆成两次。</summary>
        public static void Write(Stream s, byte[] payload)
        {
            byte[] buf = new byte[4 + payload.Length];
            buf[0] = (byte)(payload.Length & 0xFF);
            buf[1] = (byte)((payload.Length >> 8) & 0xFF);
            buf[2] = (byte)((payload.Length >> 16) & 0xFF);
            buf[3] = (byte)((payload.Length >> 24) & 0xFF);
            Buffer.BlockCopy(payload, 0, buf, 4, payload.Length);

            //一次写完：拆成「先写长度再写内容」的话，两个线程同时写就会交错，
            //而交错的字节流没有任何办法在对端还原。
            s.Write(buf, 0, buf.Length);
            s.Flush();
        }

        /// <summary>读一帧。管道关掉时返回 null。</summary>
        public static byte[] Read(Stream s, int maxLen)
        {
            byte[] head = ReadExactly(s, 4);
            if (head == null) { return null; }

            int len = head[0] | (head[1] << 8) | (head[2] << 16) | (head[3] << 24);

            if (len < 0 || len > maxLen)
            {
                throw new IOException("帧长度不合法: " + len + "（上限 " + maxLen + "）");
            }

            if (len == 0) { return new byte[0]; }

            byte[] body = ReadExactly(s, len);
            if (body == null) { throw new IOException("帧读到一半管道就断了"); }
            return body;
        }

        /// <summary>读满 n 个字节，读不满就一直读；对端关掉返回 null。</summary>
        private static byte[] ReadExactly(Stream s, int n)
        {
            byte[] buf = new byte[n];
            int got = 0;

            while (got < n)
            {
                int r = s.Read(buf, got, n - got);

                //一个字节都没读到 = 对端正常关闭；读了一半才断 = 真出事了，要区分开。
                if (r <= 0)
                {
                    if (got == 0) { return null; }
                    throw new IOException("读到一半就 EOF 了");
                }

                got += r;
            }

            return buf;
        }
    }

    #endregion

    #region//基础类型的读写

    /// <summary>
    /// 极简的二进制写入器。刻意不用 BinaryWriter：
    /// 它的字符串是「7 位变长长度 + UTF8」，读那头必须也用 BinaryReader 才对得上，
    /// 而这个协议将来要能被别的东西（比如一个诊断工具）解析，字段形状写死更好。
    /// 一律小端。
    /// </summary>
    public sealed class IpcWriter
    {
        private byte[] _buf = new byte[256];
        private int _len;

        public int Length { get { return _len; } }

        private void Need(int n)
        {
            if (_len + n <= _buf.Length) { return; }
            int cap = _buf.Length;
            while (cap < _len + n) { cap *= 2; }
            Array.Resize(ref _buf, cap);
        }

        public void U8(byte v) { Need(1); _buf[_len++] = v; }
        public void Bool(bool v) { U8(v ? (byte)1 : (byte)0); }

        public void I32(int v)
        {
            Need(4);
            _buf[_len++] = (byte)v;
            _buf[_len++] = (byte)(v >> 8);
            _buf[_len++] = (byte)(v >> 16);
            _buf[_len++] = (byte)(v >> 24);
        }

        public void I64(long v)
        {
            Need(8);
            for (int i = 0; i < 8; i++) { _buf[_len++] = (byte)(v >> (i * 8)); }
        }

        /// <summary>字符串：<c>i32 字节数</c> + UTF8；null 写 -1，与空串区分得开。</summary>
        public void Str(string v)
        {
            if (v == null) { I32(-1); return; }
            byte[] b = Encoding.UTF8.GetBytes(v);
            I32(b.Length);
            Need(b.Length);
            Buffer.BlockCopy(b, 0, _buf, _len, b.Length);
            _len += b.Length;
        }

        /// <summary>字节数组：<c>i32 长度</c> + 内容；null 写 -1。</summary>
        public void Bytes(byte[] v)
        {
            if (v == null) { I32(-1); return; }
            I32(v.Length);
            Need(v.Length);
            Buffer.BlockCopy(v, 0, _buf, _len, v.Length);
            _len += v.Length;
        }

        public void Guid_(Guid v) { Bytes(v.ToByteArray()); }

        public byte[] ToArray()
        {
            byte[] r = new byte[_len];
            Buffer.BlockCopy(_buf, 0, r, 0, _len);
            return r;
        }
    }

    /// <summary>与 <see cref="IpcWriter"/> 成对的读取器。字段顺序必须逐一对应。</summary>
    public sealed class IpcReader
    {
        private readonly byte[] _buf;
        private int _pos;

        public IpcReader(byte[] buf) { _buf = buf; _pos = 0; }
        public IpcReader(byte[] buf, int start) { _buf = buf; _pos = start; }

        private void Need(int n)
        {
            if (_pos + n > _buf.Length) { throw new IOException("帧内容读越界"); }
        }

        public byte U8() { Need(1); return _buf[_pos++]; }
        public bool Bool() { return U8() != 0; }

        public int I32()
        {
            Need(4);
            int v = _buf[_pos] | (_buf[_pos + 1] << 8) | (_buf[_pos + 2] << 16) | (_buf[_pos + 3] << 24);
            _pos += 4;
            return v;
        }

        public long I64()
        {
            Need(8);
            long v = 0;
            for (int i = 0; i < 8; i++) { v |= (long)_buf[_pos + i] << (i * 8); }
            _pos += 8;
            return v;
        }

        public string Str()
        {
            int n = I32();
            if (n < 0) { return null; }
            Need(n);
            string s = Encoding.UTF8.GetString(_buf, _pos, n);
            _pos += n;
            return s;
        }

        public byte[] Bytes()
        {
            int n = I32();
            if (n < 0) { return null; }
            Need(n);
            byte[] b = new byte[n];
            Buffer.BlockCopy(_buf, _pos, b, 0, n);
            _pos += n;
            return b;
        }

        public Guid Guid_() { return new Guid(Bytes()); }
    }

    #endregion
}
