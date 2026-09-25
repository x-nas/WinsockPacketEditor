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
                    var rule = Operate.ProxyConfig.Mapping.GetMapLocal(Operate.ProxyConfig.Proxy.MapProtocol.Https, targetHost, targetPort, path);
                    if (rule != null)
                    {
                        await SendLocal(tls, rule.LocalPath);
                        return;
                    }

                    using (var upstream = new TcpClient())
                    {
                        await upstream.ConnectAsync(targetIp, targetPort);
                        using (var remote = new SslStream(upstream.GetStream(), false))
                        {
                            await remote.AuthenticateAsClientAsync(targetHost, null, SslProtocols.Tls12, true);
                            await remote.WriteAsync(request, 0, request.Length); await remote.FlushAsync();
                            // 请求头之后的正文（以及 keep-alive 的后续请求）不应遗失；
                            // SslStream 支持一个读方向与一个写方向并发运行。
                            Task requestPump = Copy(tls, remote);
                            Task responsePump = Copy(remote, tls);
                            await Task.WhenAny(requestPump, responsePump);
                        }
                    }
                }
            }
            catch (AuthenticationException ex) { LogAuthenticationFailure(ex); }
            catch (Exception ex) { Operate.DoLog(nameof(HttpsMitmSession), ex); }
            finally { try { session.Close(SuperSocket.SocketBase.CloseReason.ServerClosing); } catch { } }
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

        private static async Task Copy(Stream source, Stream destination)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await destination.WriteAsync(buffer, 0, read);
                await destination.FlushAsync();
            }
        }
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
