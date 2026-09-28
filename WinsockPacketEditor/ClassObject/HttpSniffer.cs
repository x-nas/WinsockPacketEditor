using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WinsockPacketEditor
{
    internal sealed class HttpSniffEntry
    {
        public byte[] Raw;
        public byte[] Effective;
        public Operate.FilterConfig.Filter.FilterAction Action;
    }

    internal sealed class HttpSniffResult
    {
        public readonly List<HttpSniffEntry> HttpEntries = new List<HttpSniffEntry>();
        public readonly List<HttpSniffEntry> TcpEntries = new List<HttpSniffEntry>();
    }

    /// <summary>
    /// HTTP 只用于封包列表展示。端口仅是启用嗅探的提示；确认协议前先缓存展示数据，
    /// 以免同一批字节先显示为 TCP、之后又显示成 HTTP。缓存最大 4 MB，超限退回 TCP。
    /// </summary>
    internal sealed class HttpSniffer
    {
        private const int MaxBuffer = 4 * 1024 * 1024;
        private static readonly string[] Methods = { "GET ", "POST ", "HEAD ", "PUT ", "DELETE ", "OPTIONS ", "PATCH ", "TRACE ", "CONNECT " };
        private static readonly string[] HttpPrefix = { "HTTP/" };

        private sealed class Segment
        {
            public byte[] Raw;
            public byte[] Effective;
            public Operate.FilterConfig.Filter.FilterAction Action;
        }

        private readonly bool isRequest;
        private readonly MemoryStream buffer = new MemoryStream();
        private readonly Queue<Segment> segments = new Queue<Segment>();
        private bool passthrough;

        public bool IsHttp { get; private set; }
        public bool IsNotHttp { get; private set; }

        public HttpSniffer(bool isRequest) { this.isRequest = isRequest; }

        public HttpSniffResult Feed(byte[] raw, byte[] effective, Operate.FilterConfig.Filter.FilterAction action)
        {
            HttpSniffResult result = new HttpSniffResult();
            if (effective == null || effective.Length == 0) { return result; }

            AddSegment(raw, effective, action);
            if (IsNotHttp) { FlushAsTcp(result); return result; }

            // 在写入前检查，Content-Length 等异常输入也不能越过展示缓存上限。
            if (buffer.Length + effective.Length > MaxBuffer) { RejectAsTcp(result); return result; }

            if (passthrough)
            {
                if (LooksLikeHead(effective, effective.Length)) { passthrough = false; }
                else
                {
                    HttpSniffEntry pass = TakeEntry(effective.Length);
                    if (pass != null) { result.HttpEntries.Add(pass); }
                    return result;
                }
            }

            buffer.Write(effective, 0, effective.Length);
            if (!IsHttp)
            {
                byte[] head = buffer.GetBuffer();
                int length = (int)buffer.Length;
                if (LooksLikeHead(head, length)) { IsHttp = true; }
                else if (length >= 8 || !IsPossiblyHead(head, length)) { RejectAsTcp(result); return result; }
                else { return result; }
            }

            while (true)
            {
                byte[] all = buffer.GetBuffer();
                int length = (int)buffer.Length;
                int headEnd = IndexOfCrlfCrlf(all, length);
                if (headEnd < 0) { break; }

                int bodyStart = headEnd + 4;
                string headers = Encoding.ASCII.GetString(all, 0, headEnd);
                int total;
                if (IsChunked(headers))
                {
                    total = ChunkedTotal(all, length, bodyStart);
                    if (total < 0) { break; }
                }
                else
                {
                    int contentLength = ContentLength(headers);
                    if (contentLength >= 0)
                    {
                        if (contentLength > MaxBuffer - bodyStart) { RejectAsTcp(result); return result; }
                        total = bodyStart + contentLength;
                        if (length < total) { break; }
                    }
                    else { total = length; passthrough = true; }
                }

                HttpSniffEntry entry = TakeEntry(total);
                if (entry != null) { result.HttpEntries.Add(entry); }
                Consume(total);
                if (passthrough || buffer.Length == 0) { break; }
            }
            return result;
        }

        /// <summary>会话关闭时刷出尚未组成完整消息的数据，避免末包从列表消失。</summary>
        public HttpSniffResult Flush()
        {
            HttpSniffResult result = new HttpSniffResult();
            if (segments.Count == 0) { return result; }
            if (IsHttp)
            {
                HttpSniffEntry entry = TakeEntry((int)buffer.Length);
                if (entry != null) { result.HttpEntries.Add(entry); }
                buffer.SetLength(0);
            }
            else { FlushAsTcp(result); }
            return result;
        }

        private void AddSegment(byte[] raw, byte[] effective, Operate.FilterConfig.Filter.FilterAction action)
        {
            byte[] effectiveCopy = Copy(effective);
            byte[] rawCopy = raw != null && raw.Length == effective.Length ? Copy(raw) : Copy(effective);
            segments.Enqueue(new Segment { Raw = rawCopy, Effective = effectiveCopy, Action = action });
        }

        private void RejectAsTcp(HttpSniffResult result)
        {
            IsNotHttp = true;
            IsHttp = false;
            passthrough = false;
            buffer.SetLength(0);
            FlushAsTcp(result);
        }

        private void FlushAsTcp(HttpSniffResult result)
        {
            while (segments.Count > 0)
            {
                Segment segment = segments.Dequeue();
                result.TcpEntries.Add(new HttpSniffEntry { Raw = segment.Raw, Effective = segment.Effective, Action = segment.Action });
            }
        }

        private HttpSniffEntry TakeEntry(int length)
        {
            if (length <= 0 || segments.Count == 0) { return null; }
            byte[] raw = new byte[length];
            byte[] effective = new byte[length];
            int offset = 0;
            Operate.FilterConfig.Filter.FilterAction action = Operate.FilterConfig.Filter.FilterAction.None;
            while (offset < length && segments.Count > 0)
            {
                Segment segment = segments.Peek();
                int take = Math.Min(segment.Effective.Length, length - offset);
                Buffer.BlockCopy(segment.Raw, 0, raw, offset, take);
                Buffer.BlockCopy(segment.Effective, 0, effective, offset, take);
                if (action == Operate.FilterConfig.Filter.FilterAction.None && segment.Action != action) { action = segment.Action; }
                offset += take;
                if (take == segment.Effective.Length) { segments.Dequeue(); }
                else
                {
                    segment.Raw = Slice(segment.Raw, take);
                    segment.Effective = Slice(segment.Effective, take);
                }
            }
            return offset == length ? new HttpSniffEntry { Raw = raw, Effective = effective, Action = action } : null;
        }

        private void Consume(int length)
        {
            int remaining = (int)buffer.Length - length;
            if (remaining > 0) { Buffer.BlockCopy(buffer.GetBuffer(), length, buffer.GetBuffer(), 0, remaining); }
            buffer.SetLength(remaining);
        }

        private bool LooksLikeHead(byte[] data, int length)
        {
            if (data == null || length < 4) { return false; }
            if (!isRequest) { return LooksLikePrefix(data, length, HttpPrefix); }
            return LooksLikePrefix(data, length, Methods) && (Encoding.ASCII.GetString(data, 0, Math.Min(length, 64)).IndexOf("HTTP/", StringComparison.Ordinal) > 0 || length < 16);
        }

        private static bool LooksLikePrefix(byte[] data, int length, string[] candidates)
        {
            foreach (string candidate in candidates)
            {
                int count = Math.Min(length, candidate.Length);
                bool match = true;
                for (int i = 0; i < count; i++) { if (data[i] != (byte)candidate[i]) { match = false; break; } }
                if (match) { return true; }
            }
            return false;
        }

        private bool IsPossiblyHead(byte[] data, int length)
        {
            return length == 0 || (isRequest ? LooksLikePrefix(data, length, Methods) : LooksLikePrefix(data, length, HttpPrefix));
        }

        private static int IndexOfCrlfCrlf(byte[] data, int length) { return IndexOfCrlfCrlf(data, length, 0); }
        private static int IndexOfCrlfCrlf(byte[] data, int length, int from)
        {
            for (int i = from; i + 3 < length; i++) if (data[i] == 13 && data[i + 1] == 10 && data[i + 2] == 13 && data[i + 3] == 10) { return i; }
            return -1;
        }
        private static int IndexOfCrlf(byte[] data, int length, int from)
        {
            for (int i = from; i + 1 < length; i++) if (data[i] == 13 && data[i + 1] == 10) { return i; }
            return -1;
        }
        private static bool IsChunked(string headers)
        {
            return headers.IndexOf("transfer-encoding", StringComparison.OrdinalIgnoreCase) >= 0 && headers.IndexOf("chunked", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        private static int ContentLength(string headers)
        {
            foreach (string raw in headers.Split('\n'))
            {
                string line = raw.Trim(); int colon = line.IndexOf(':');
                if (colon <= 0 || !line.Substring(0, colon).Trim().Equals("Content-Length", StringComparison.OrdinalIgnoreCase)) { continue; }
                int value; if (int.TryParse(line.Substring(colon + 1).Trim(), out value) && value >= 0) { return value; }
            }
            return -1;
        }
        private static int ChunkedTotal(byte[] data, int length, int start)
        {
            int position = start;
            while (true)
            {
                int lineEnd = IndexOfCrlf(data, length, position); if (lineEnd < 0) { return -1; }
                string sizeLine = Encoding.ASCII.GetString(data, position, lineEnd - position).Trim(); int semicolon = sizeLine.IndexOf(';');
                if (semicolon >= 0) { sizeLine = sizeLine.Substring(0, semicolon); }
                int size; try { size = Convert.ToInt32(sizeLine.Trim(), 16); } catch { return -1; }
                position = lineEnd + 2;
                if (size == 0)
                {
                    int trailerEnd = IndexOfCrlfCrlf(data, length, position);
                    if (trailerEnd >= 0) { return trailerEnd + 4; }
                    return position + 1 < length && data[position] == 13 && data[position + 1] == 10 ? position + 2 : -1;
                }
                if (size < 0 || size > MaxBuffer - position || length < position + size + 2) { return -1; }
                position += size + 2;
            }
        }
        private static byte[] Copy(byte[] source) { byte[] result = new byte[source.Length]; Buffer.BlockCopy(source, 0, result, 0, result.Length); return result; }
        private static byte[] Slice(byte[] source, int offset) { byte[] result = new byte[source.Length - offset]; Buffer.BlockCopy(source, offset, result, 0, result.Length); return result; }
    }
}
