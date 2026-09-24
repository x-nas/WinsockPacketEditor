using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 把一个方向的 TCP 字节流按 HTTP 消息切分，**只为列表展示**。
    ///
    /// 背景：SOCKS5 那条路是纯字节转发，HTTP 请求/响应会被记成一条条 TCP 分片。
    /// 这里按端口（80/8080，见 GetDomainType_ByPort）判定为 HTTP 的会话才启用，
    /// 把完整的一条请求/响应拼出来，交给调用方按 HTTP_Req / HTTP_Resp 入列表。
    ///
    /// ⚠️ 只影响展示：真正发出去的字节仍是 DoFilter_SOCKS_TCP 逐段过滤后的分片，
    /// 这里的拼装不会改变线上数据（因此 HTTP 条目带 FilterAction.None）。
    ///
    /// 支持：Content-Length、Transfer-Encoding: chunked、无正文（204/304/1xx）。
    /// 其它情况（无长度、keep-alive 流式）退化成「头部 + 当前已到的字节」一条，
    /// 之后进入直通模式，下一次看到新的头再恢复解析。
    /// </summary>
    internal sealed class HttpSniffer
    {
        private const int MaxBuffer = 4 * 1024 * 1024;   // 单条消息上限，超了放弃解析退回 TCP

        private static readonly string[] Methods =
        {
            "GET ", "POST ", "HEAD ", "PUT ", "DELETE ", "OPTIONS ", "PATCH ", "TRACE ", "CONNECT ",
        };

        private readonly bool isRequest;
        private readonly MemoryStream buf = new MemoryStream();

        /// <summary>已经确认是 HTTP 会话。</summary>
        public bool IsHttp { get; private set; }

        /// <summary>已经确认不是 HTTP（解析失败/超限），调用方按 TCP 处理。</summary>
        public bool IsNotHttp { get; private set; }

        /// <summary>无长度正文的直通模式：后续 Feed 原样成条。</summary>
        private bool passthrough;

        public HttpSniffer(bool isRequest)
        {
            this.isRequest = isRequest;
        }

        /// <summary>还有没拼完的字节。</summary>
        public bool HasPending { get { return buf.Length > 0; } }

        /// <summary>把残留的一条吐出来（会话关闭时用）。</summary>
        public byte[] TakePending()
        {
            if (buf.Length == 0) { return null; }
            byte[] all = buf.ToArray();
            buf.SetLength(0);
            return all;
        }

        /// <summary>喂一段字节，返回本次切出的完整 HTTP 消息（可能 0 条）。</summary>
        public List<byte[]> Feed(byte[] data)
        {
            List<byte[]> result = new List<byte[]>();
            if (data == null || data.Length == 0 || IsNotHttp) { return result; }

            if (passthrough)
            {
                if (LooksLikeHead(data)) { passthrough = false; }
                else { result.Add(data); return result; }
            }

            buf.Write(data, 0, data.Length);

            if (!IsHttp)
            {
                byte[] head = buf.ToArray();
                if (LooksLikeHead(head)) { IsHttp = true; }
                else if (head.Length >= 8 || !IsPossiblyHead(head))
                {
                    IsNotHttp = true;
                    buf.SetLength(0);
                    return result;
                }
                else
                {
                    return result;   // 还看不出，等更多字节
                }
            }

            while (true)
            {
                byte[] all = buf.ToArray();
                int headEnd = IndexOfCrlfCrlf(all);
                if (headEnd < 0)
                {
                    if (all.Length > MaxBuffer) { IsNotHttp = true; buf.SetLength(0); }
                    break;
                }

                int bodyStart = headEnd + 4;
                string headers = Encoding.ASCII.GetString(all, 0, headEnd);

                int total = -1;
                if (IsChunked(headers))
                {
                    total = ChunkedTotal(all, bodyStart);
                    if (total < 0)
                    {
                        if (all.Length > MaxBuffer) { IsNotHttp = true; buf.SetLength(0); }
                        break;   // 还没收完
                    }
                }
                else
                {
                    int cl = ContentLength(headers);
                    if (cl >= 0)
                    {
                        if (all.Length < bodyStart + cl) { break; }   // 还没收完
                        total = bodyStart + cl;
                    }
                    else
                    {
                        // 没有长度：头 + 当前已到的字节算一条，之后直通
                        total = all.Length;
                        passthrough = true;
                    }
                }

                byte[] msg = new byte[total];
                Array.Copy(all, 0, msg, 0, total);
                result.Add(msg);

                // 剩下的挪回缓冲
                int rest = all.Length - total;
                byte[] rem = new byte[rest];
                Array.Copy(all, total, rem, 0, rest);
                buf.SetLength(0);
                buf.Write(rem, 0, rest);

                if (passthrough || rest == 0) { break; }
            }

            return result;
        }

        private bool LooksLikeHead(byte[] b)
        {
            if (b == null || b.Length < 4) { return false; }

            if (isRequest)
            {
                if (!LooksLikePrefix(b, Methods)) { return false; }
                // 方法之后要看到 "HTTP/" 才认（避免把以 "GET " 开头的二进制当 HTTP）
                return Encoding.ASCII.GetString(b, 0, Math.Min(b.Length, 64)).IndexOf("HTTP/", StringComparison.Ordinal) > 0
                    || b.Length < 16;
            }

            return LooksLikePrefix(b, new[] { "HTTP/" });
        }

        /// <summary>b 是否是某个候选串的前缀（或已完整匹配）。</summary>
        private static bool LooksLikePrefix(byte[] b, string[] candidates)
        {
            foreach (string c in candidates)
            {
                int n = Math.Min(b.Length, c.Length);
                bool ok = true;
                for (int i = 0; i < n; i++)
                {
                    if (b[i] != (byte)c[i]) { ok = false; break; }
                }
                if (ok) { return true; }
            }
            return false;
        }

        /// <summary>还可能是头部开头（字节太少，且与某个候选串前缀一致）。</summary>
        private bool IsPossiblyHead(byte[] b)
        {
            if (b.Length == 0) { return true; }
            if (isRequest) { return LooksLikePrefix(b, Methods); }
            return LooksLikePrefix(b, new[] { "HTTP/" });
        }

        private static int IndexOfCrlfCrlf(byte[] b)
        {
            for (int i = 0; i + 3 < b.Length; i++)
            {
                if (b[i] == 13 && b[i + 1] == 10 && b[i + 2] == 13 && b[i + 3] == 10) { return i; }
            }
            return -1;
        }

        private static bool IsChunked(string headers)
        {
            return headers.IndexOf("transfer-encoding", StringComparison.OrdinalIgnoreCase) >= 0
                && headers.IndexOf("chunked", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static int ContentLength(string headers)
        {
            foreach (string raw in headers.Split('\n'))
            {
                string line = raw.Trim();
                int colon = line.IndexOf(':');
                if (colon <= 0) { continue; }
                string name = line.Substring(0, colon).Trim();
                if (!name.Equals("Content-Length", StringComparison.OrdinalIgnoreCase)) { continue; }
                int v;
                if (int.TryParse(line.Substring(colon + 1).Trim(), out v) && v >= 0) { return v; }
            }
            return -1;
        }

        /// <summary>分块编码的总长度（含结尾 0 块）；没收完返回 -1。</summary>
        private static int ChunkedTotal(byte[] b, int start)
        {
            int pos = start;
            while (true)
            {
                int lineEnd = IndexOfCrlf(b, pos);
                if (lineEnd < 0) { return -1; }

                string sizeLine = Encoding.ASCII.GetString(b, pos, lineEnd - pos).Trim();
                int semi = sizeLine.IndexOf(';');
                if (semi >= 0) { sizeLine = sizeLine.Substring(0, semi); }

                int size;
                try { size = Convert.ToInt32(sizeLine.Trim(), 16); }
                catch { return -1; }

                pos = lineEnd + 2;

                if (size == 0)
                {
                    // 结尾：0\r\n 之后再一个 CRLF（允许有 trailer，简化处理：找下一个 CRLFCRLF 或空行）
                    int endPos = IndexOfCrlf(b, pos);
                    if (endPos < 0) { return -1; }
                    return endPos + 2;
                }

                if (b.Length < pos + size + 2) { return -1; }
                pos += size + 2;   // 数据 + 结尾 CRLF
            }
        }

        private static int IndexOfCrlf(byte[] b, int from)
        {
            for (int i = from; i + 1 < b.Length; i++)
            {
                if (b[i] == 13 && b[i + 1] == 10) { return i; }
            }
            return -1;
        }
    }
}
