using System;

namespace WinsockPacketEditor.Ipc
{
    #region//封包流的帧

    /// <summary>
    /// 封包流（目标 → 外壳，单向高频）的一帧。
    ///
    /// 【与方案书的一处出入，是有意的】INJECT-IPC-PLAN.md 第 3.1 节把地址写成
    /// 四个定宽数字字段（fromIp/fromPort/toIp/toPort），字符串化交给外壳。
    /// 这里改成<b>直接传两个地址字符串</b>，理由：
    ///
    ///   · 地址不是在钩子线程上取的 —— getsockname/getpeername 必须在目标进程里调，
    ///     但可以在<b>写线程</b>上调（今天也不在钩子线程，是在 PacketInfo_ToQueue 的
    ///     Task.Run 里）。既然已经在目标侧调了，顺手格式化成字符串的代价是几十纳秒。
    ///   · 复用现成的 GetIPString_BySocketAddr，不用为「拆出数字」再写一份地址解析 ——
    ///     那是最容易与主路径长出差异的地方。
    ///   · 代价是每帧多约 40 字节。3000 包/秒 = 120 KB/s，而载荷本身是 12 MB/s。
    ///
    /// <b>归属地（QQWry）仍然留在外壳</b>，与方案一致 —— 那才是 R8 的大头（26 MB 数据 + 每包一次异步查询）。
    ///
    /// 帧结构（小端，位数无关）：
    /// <code>
    ///   i64   id            目标侧自增
    ///   i64   timeTicks     目标侧时间（保证与钩子顺序一致）
    ///   i64   socket        SOCKET 句柄，32 位目标高位为 0
    ///   u8    packetType
    ///   u8    filterAction
    ///   u8    flags         bit0 = 改写后与改写前相同（省掉一份字节）
    ///   str   from          "1.2.3.4:5678"
    ///   str   to
    ///   bytes raw
    ///   bytes new           flags.bit0 置位时省略
    /// </code>
    /// </summary>
    public static class PacketFrame
    {
        /// <summary>bit0：改写后的字节与改写前完全相同，帧里只带一份。</summary>
        public const byte FlagSameBuffer = 1;

        #region//编码（目标侧的写线程）

        public static byte[] Encode(
            long id,
            long timeTicks,
            long socket,
            byte packetType,
            byte filterAction,
            string from,
            string to,
            byte[] raw,
            byte[] modified)
        {
            //绝大多数封包没被滤镜改过，两份字节一模一样 —— 那就只传一份。
            //抓包时这是常态，省下的正好是一半带宽。
            bool same = SameBytes(raw, modified);

            var w = new IpcWriter();
            w.I64(id);
            w.I64(timeTicks);
            w.I64(socket);
            w.U8(packetType);
            w.U8(filterAction);
            w.U8(same ? FlagSameBuffer : (byte)0);
            w.Str(from);
            w.Str(to);
            w.Bytes(raw);
            if (!same) { w.Bytes(modified); }

            return w.ToArray();
        }

        private static bool SameBytes(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b)) { return true; }
            if (a == null || b == null) { return false; }
            if (a.Length != b.Length) { return false; }
            for (int i = 0; i < a.Length; i++) { if (a[i] != b[i]) { return false; } }
            return true;
        }

        #endregion

        #region//解码（外壳侧的读线程）

        public sealed class Decoded
        {
            public long Id;
            public long TimeTicks;
            public long Socket;
            public byte PacketType;
            public byte FilterAction;
            public string From;
            public string To;
            public byte[] Raw;
            public byte[] Modified;
        }

        public static Decoded Decode(byte[] payload)
        {
            var r = new IpcReader(payload);
            var d = new Decoded();

            d.Id = r.I64();
            d.TimeTicks = r.I64();
            d.Socket = r.I64();
            d.PacketType = r.U8();
            d.FilterAction = r.U8();
            byte flags = r.U8();
            d.From = r.Str();
            d.To = r.Str();
            d.Raw = r.Bytes();
            d.Modified = (flags & FlagSameBuffer) != 0 ? d.Raw : r.Bytes();

            return d;
        }

        #endregion
    }

    #endregion
}
