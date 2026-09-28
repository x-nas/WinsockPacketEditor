using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinsockPacketEditor
{
    /// <summary>HTTPS 本地映射的受限 HTTP/1.1 会话。客户端字节由 SuperSocket 转发帧输入，绝不直接抢读其 socket。</summary>
    internal sealed class HttpsMitmSession
    {
        private readonly ProxySession session;
        private readonly string targetHost, targetIp;
        private readonly int targetPort;
        private readonly FrameStream client;

        internal HttpsMitmSession(ProxySession session, string targetHost, string targetIp, int targetPort)
        {
            this.session = session; this.targetHost = targetHost; this.targetIp = targetIp; this.targetPort = targetPort;
            client = new FrameStream(data => session.SendToClient(data, 0, data.Length));
        }

        internal void Start() { _ = Task.Run(Run); }
        internal void Feed(byte[] data) { client.Feed(data); }
        internal void Complete() { client.Complete(); }

        private async Task Run()
        {
            try
            {
                using (X509Certificate2 leaf = new HttpsMitmCertificateManager().CreateLeaf(targetHost))
                using (var tls = new SslStream(client, false))
                {
                    await tls.AuthenticateAsServerAsync(leaf, false, SslProtocols.Tls12, false);
                    byte[] request = await ReadHeaders(tls);
                    string text = Encoding.ASCII.GetString(request);
                    string[] first = text.Split(new[] { "\r\n" }, StringSplitOptions.None)[0].Split(' ');
                    if (first.Length < 3 || !first[2].StartsWith("HTTP/1.", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidDataException("HTTPS 映射只支持 HTTP/1.x 请求");
                    }
                    string path = first.Length > 1 ? first[1] : "/";
                    var localRule = Operate.ProxyConfig.Mapping.Enable_MapLocal
                        ? Operate.ProxyConfig.Mapping.GetMapLocal(Operate.ProxyConfig.Proxy.MapProtocol.Https, targetHost, targetPort, path)
                        : null;
                    if (localRule != null)
                    {
                        Operate.ProxyConfig.Mapping.LogMapLocalMatch(session, localRule, targetHost, targetPort, path);
                        await SendLocal(tls, localRule.LocalPath);
                        return;
                    }

                    var remoteRule = Operate.ProxyConfig.Mapping.Enable_MapRemote
                        ? Operate.ProxyConfig.Mapping.GetMapRemote(Operate.ProxyConfig.Proxy.MapProtocol.Https, targetHost, targetPort, path)
                        : null;
                    if (remoteRule != null)
                    {
                        Operate.ProxyConfig.Mapping.LogMapRemoteMatch(session, remoteRule, targetHost, targetPort, path);
                        string headers = Encoding.ASCII.GetString(request);
                        byte[] rewritten = Operate.ProxyConfig.Mapping.ModifyRequestHostAndPath(
                            headers, Operate.ProxyConfig.Proxy.ParseHttpHeaders(headers), remoteRule.HostTo,
                            remoteRule.PortTo, remoteRule.PathTo, remoteRule.ProtocolTypeTo);
                        await Relay(tls, rewritten, remoteRule.HostTo, remoteRule.PortTo,
                            remoteRule.ProtocolTypeTo == Operate.ProxyConfig.Proxy.MapProtocol.Https, true);
                        return;
                    }

                    await Relay(tls, request, targetHost, targetPort, true, false, targetIp);
                }
            }
            catch (AuthenticationException ex) { LogAuthenticationFailure(ex); }
            catch (Exception ex) { Operate.DoLog(nameof(HttpsMitmSession), ex); }
            finally { try { session.Close(SuperSocket.SocketBase.CloseReason.ServerClosing); } catch { } }
        }

        /// <summary>首个请求决定上游，并串行完成该请求及其响应，避免同一 SslStream 读写并发。</summary>
        private async Task Relay(Stream client, byte[] firstRequest, string host, int port, bool useTls, bool remoteMapping, string resolvedIp = null)
        {
            using (var upstream = new TcpClient())
            {
                await upstream.ConnectAsync(string.IsNullOrEmpty(resolvedIp) ? host : resolvedIp, port);
                    Stream remote = upstream.GetStream();
                SslStream tls = null;
                try
                {
                    if (useTls)
                    {
                        tls = new SslStream(remote, false);
                        await tls.AuthenticateAsClientAsync(host, null, SslProtocols.Tls12, true);
                        remote = tls;
                    }
                    byte[] request = WithConnectionClose(firstRequest);
                    await remote.WriteAsync(request, 0, request.Length); await remote.FlushAsync();
                    await CopyKnownRequestBody(client, remote, firstRequest);
                    await CopySingleHttpResponse(remote, client);
                }
                finally { if (tls != null) tls.Dispose(); }
            }
        }

        private static async Task CopyKnownRequestBody(Stream client, Stream remote, byte[] headers)
        {
            int contentLength = ContentLength(headers);
            if (contentLength <= 0) { return; }
            await CopyExactly(client, remote, contentLength);
        }

        /// <summary>串行转发 HTTP/1.x 响应；无 Content-Length 时由已请求的 Connection: close 定界。</summary>
        private static async Task CopySingleHttpResponse(Stream remote, Stream client)
        {
            byte[] headers = WithConnectionClose(await ReadHeaders(remote));
            int contentLength = ContentLength(headers);
            await client.WriteAsync(headers, 0, headers.Length); await client.FlushAsync();
            if (contentLength >= 0) { await CopyExactly(remote, client, contentLength); }
            else { await CopyUntilEnd(remote, client); }
        }

        private static async Task CopyExactly(Stream source, Stream destination, int length)
        {
            byte[] buffer = new byte[16 * 1024]; int remaining = length;
            while (remaining > 0)
            {
                int read = await source.ReadAsync(buffer, 0, Math.Min(buffer.Length, remaining));
                if (read == 0) { throw new EndOfStreamException("HTTP 正文未完整到达"); }
                await destination.WriteAsync(buffer, 0, read); await destination.FlushAsync(); remaining -= read;
            }
        }

        private static async Task CopyUntilEnd(Stream source, Stream destination)
        {
            byte[] buffer = new byte[16 * 1024]; int read;
            while ((read = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await destination.WriteAsync(buffer, 0, read); await destination.FlushAsync();
            }
        }

        private static int ContentLength(byte[] headers)
        {
            string text = Encoding.ASCII.GetString(headers);
            foreach (string line in text.Split(new[] { "\r\n" }, StringSplitOptions.None))
            {
                if (line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase) && int.TryParse(line.Substring(15).Trim(), out int value) && value >= 0) { return value; }
            }
            return -1;
        }

        /// <summary>该受限会话在一组请求/响应完成后即关闭，两端必须看到一致的连接语义。</summary>
        private static byte[] WithConnectionClose(byte[] headers)
        {
            string text = Encoding.ASCII.GetString(headers);
            int end = text.IndexOf("\r\n\r\n", StringComparison.Ordinal);
            if (end < 0) { throw new InvalidDataException("HTTP 头部不完整"); }

            string[] lines = text.Substring(0, end).Split(new[] { "\r\n" }, StringSplitOptions.None);
            var result = new StringBuilder(); bool found = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 0) { result.Append("\r\n"); }
                if (i > 0 && lines[i].StartsWith("Connection:", StringComparison.OrdinalIgnoreCase))
                {
                    result.Append("Connection: close"); found = true;
                }
                else { result.Append(lines[i]); }
            }
            if (!found) { result.Append("\r\nConnection: close"); }
            result.Append("\r\n\r\n");
            return Encoding.ASCII.GetBytes(result.ToString());
        }

        /// <summary>客户端拒绝 MITM 证书是预期分支；不把每次浏览器提示都刷进系统日志。</summary>
        private void LogAuthenticationFailure(AuthenticationException ex)
        {
            var win32 = ex.InnerException as Win32Exception;
            uint code = win32 == null ? 0U : unchecked((uint)win32.NativeErrorCode);
            string target = targetHost + ":" + targetPort;
            string clientAddress = string.IsNullOrEmpty(session.ClientAddress) ? session.ClientIP : session.ClientAddress;

            if (code == 0x80090327U)
            {
                return;
            }

            Operate.DoLog(nameof(HttpsMitmSession),
                "HTTPS 映射 TLS 握手失败（" + clientAddress + " → " + target + "）：" + ex.Message
                + (win32 == null ? string.Empty : "（SSPI=0x" + code.ToString("X8") + "）"));
        }

        private static async Task SendLocal(Stream stream, string path)
        {
            if (!File.Exists(path)) { await Write(stream, "HTTP/1.1 404 Not Found\r\nContent-Length: 0\r\nConnection: close\r\n\r\n"); return; }
            var file = new FileInfo(path);
            await Write(stream, "HTTP/1.1 200 OK\r\nContent-Length: " + file.Length + "\r\nConnection: close\r\n\r\n");
            using (var input = File.OpenRead(path)) { await input.CopyToAsync(stream); await stream.FlushAsync(); }
        }
        private static Task Write(Stream stream, string text) { byte[] b = Encoding.ASCII.GetBytes(text); return stream.WriteAsync(b, 0, b.Length); }

        private static async Task<byte[]> ReadHeaders(Stream stream)
        {
            using (var output = new MemoryStream())
            {
                var one = new byte[1];
                while (output.Length < 64 * 1024)
                {
                    if (await stream.ReadAsync(one, 0, 1) == 0) throw new EndOfStreamException();
                    output.WriteByte(one[0]); int n = (int)output.Length; byte[] b = output.GetBuffer();
                    if (n >= 4 && b[n - 4] == 13 && b[n - 3] == 10 && b[n - 2] == 13 && b[n - 1] == 10) return output.ToArray();
                }
                throw new InvalidDataException("HTTPS 映射请求头超过 64 KB");
            }
        }

        private sealed class FrameStream : Stream
        {
            private readonly Queue<byte[]> queue = new Queue<byte[]>(); private readonly SemaphoreSlim signal = new SemaphoreSlim(0); private readonly Action<byte[]> write;
            private byte[] current; private int offset; private bool done;
            internal FrameStream(Action<byte[]> write) { this.write = write; }
            internal void Feed(byte[] data) { if (data == null || data.Length == 0) return; byte[] copy = (byte[])data.Clone(); lock (queue) { if (done) return; queue.Enqueue(copy); } signal.Release(); }
            internal void Complete() { lock (queue) { done = true; } signal.Release(); }
            public override bool CanRead => true; public override bool CanSeek => false; public override bool CanWrite => true; public override long Length => throw new NotSupportedException(); public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
            public override async Task<int> ReadAsync(byte[] buffer, int index, int count, CancellationToken token)
            {
                while (current == null || offset >= current.Length) { lock (queue) { if (queue.Count > 0) { current = queue.Dequeue(); offset = 0; break; } if (done) return 0; } await signal.WaitAsync(token); }
                int take = Math.Min(count, current.Length - offset); Buffer.BlockCopy(current, offset, buffer, index, take); offset += take; return take;
            }
            public override int Read(byte[] buffer, int offset, int count) { return ReadAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult(); }
            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token) { byte[] copy = new byte[count]; Buffer.BlockCopy(buffer, offset, copy, 0, count); write(copy); return Task.CompletedTask; }
            public override void Write(byte[] buffer, int offset, int count) { WriteAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult(); }
            public override void Flush() { } public override Task FlushAsync(CancellationToken token) { return Task.CompletedTask; }
            public override long Seek(long offset, SeekOrigin origin) { throw new NotSupportedException(); } public override void SetLength(long value) { throw new NotSupportedException(); }
        }
    }
}
