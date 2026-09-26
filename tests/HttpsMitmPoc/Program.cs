using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HttpsMitmPoc
{
    internal static class Program
    {
        private const string MitmHost = "mapped.test";
        private const string UpstreamHost = "upstream.test";

        private static int Main(string[] args)
        {
            try
            {
                if (args != null && args.Length == 5 && string.Equals(args[0], "--live", StringComparison.OrdinalIgnoreCase))
                {
                    RunLiveWpeProxy(args[1], int.Parse(args[2]), args[3], args[4]).GetAwaiter().GetResult();
                    Console.WriteLine("PASS: live WPE SOCKS5 HTTPS mapping test completed.");
                    return 0;
                }
                using (var root = CreateRootCa())
                using (var mitmLeaf = CreateServerCertificate(MitmHost, root))
                using (var upstreamLeaf = CreateSelfSignedServerCertificate(UpstreamHost))
                {
                    Assert(HasDnsSubjectAlternativeName(mitmLeaf), "MITM leaf has DNS SAN");
                    RunProxyRoundTrip(root, mitmLeaf, upstreamLeaf).GetAwaiter().GetResult();
                    RunFrameStreamLargeResponse(root, mitmLeaf).GetAwaiter().GetResult();
                }

                Console.WriteLine("PASS: HTTPS MITM POC completed without third-party packages.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("FAIL: " + ex);
                return 1;
            }
        }

        /// <summary>不内置用户名或密码；用于连正在运行的 WPE SOCKS5，验证真实 HTTPS 映射数据面。</summary>
        private static async Task RunLiveWpeProxy(string proxyHost, int proxyPort, string user, string password)
        {
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(proxyHost, proxyPort);
                Stream stream = client.GetStream();
                await WriteBytes(stream, new byte[] { 5, 1, 2 });
                byte[] hello = await ReadExactly(stream, 2);
                Assert(hello[0] == 5 && hello[1] == 2, "SOCKS5 selects username/password authentication");

                byte[] userBytes = Encoding.UTF8.GetBytes(user); byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                if (userBytes.Length > 255 || passwordBytes.Length > 255) { throw new ArgumentException("SOCKS5 credentials are too long"); }
                byte[] auth = new byte[3 + userBytes.Length + passwordBytes.Length]; auth[0] = 1; auth[1] = (byte)userBytes.Length;
                Buffer.BlockCopy(userBytes, 0, auth, 2, userBytes.Length); auth[2 + userBytes.Length] = (byte)passwordBytes.Length;
                Buffer.BlockCopy(passwordBytes, 0, auth, 3 + userBytes.Length, passwordBytes.Length);
                await WriteBytes(stream, auth);
                byte[] authReply = await ReadExactly(stream, 2);
                Assert(authReply[0] == 1 && authReply[1] == 0, "SOCKS5 authentication succeeds");

                byte[] host = Encoding.ASCII.GetBytes("www.baidu.com");
                byte[] connect = new byte[7 + host.Length]; connect[0] = 5; connect[1] = 1; connect[2] = 0; connect[3] = 3; connect[4] = (byte)host.Length;
                Buffer.BlockCopy(host, 0, connect, 5, host.Length); connect[5 + host.Length] = 1; connect[6 + host.Length] = 187;
                await WriteBytes(stream, connect);
                byte[] reply = await ReadExactly(stream, 4);
                Assert(reply[0] == 5 && reply[1] == 0, "SOCKS5 CONNECT succeeds");
                int bindLength = reply[3] == 1 ? 4 : reply[3] == 4 ? 16 : (await ReadExactly(stream, 1))[0];
                await ReadExactly(stream, bindLength + 2);

                using (var tls = new SslStream(stream, false, (sender, certificate, chain, errors) =>
                {
                    if (certificate == null) { return false; }
                    var leaf = new X509Certificate2(certificate);
                    Console.WriteLine("INFO: WPE MITM leaf=" + leaf.Subject + " validation=" + errors);
                    // --live 是针对用户明确指定的本机 WPE 代理的数据面测试；正常客户端仍必须安装并信任 WPE 根证书。
                    return string.Equals(leaf.GetNameInfo(X509NameType.DnsName, false), "www.baidu.com", StringComparison.OrdinalIgnoreCase);
                }))
                {
                    await tls.AuthenticateAsClientAsync("www.baidu.com", null, SslProtocols.Tls12, true);
                    byte[] request = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: www.baidu.com\r\nConnection: close\r\n\r\n");
                    await tls.WriteAsync(request, 0, request.Length); await tls.FlushAsync();
                    string headers = await ReadHeaders(tls);
                    int contentLength = ParseContentLength(headers);
                    Assert(headers.StartsWith("HTTP/1.1 200", StringComparison.Ordinal), "mapped endpoint returns HTTP 200");
                    Assert(contentLength > 0, "mapped endpoint provides Content-Length");
                    byte[] body = await ReadExactly(tls, contentLength);
                    string html = Encoding.UTF8.GetString(body);
                    Assert(html.IndexOf("WINSOCK PACKET EDITOR", StringComparison.OrdinalIgnoreCase) >= 0, "mapped WPE website body arrives completely");
                    Console.WriteLine("INFO: response body bytes=" + body.Length);
                }
            }
        }

        private static int ParseContentLength(string headers)
        {
            foreach (string line in headers.Split(new[] { "\r\n" }, StringSplitOptions.None))
            {
                if (line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase) && int.TryParse(line.Substring(15).Trim(), out int value)) { return value; }
            }
            return -1;
        }

        private static async Task WriteBytes(Stream stream, byte[] bytes) { await stream.WriteAsync(bytes, 0, bytes.Length); await stream.FlushAsync(); }
        private static async Task<byte[]> ReadExactly(Stream stream, int count)
        {
            byte[] output = new byte[count]; int offset = 0;
            while (offset < count) { int read = await stream.ReadAsync(output, offset, count - offset); if (read == 0) { throw new EndOfStreamException(); } offset += read; }
            return output;
        }

        private static async Task RunProxyRoundTrip(X509Certificate2 root, X509Certificate2 mitmLeaf, X509Certificate2 upstreamLeaf)
        {
            using (var httpsUpstream = new TestHttpsUpstream(upstreamLeaf, 2))
            using (var httpUpstream = new TestHttpUpstream())
            using (var mitm = new TestMitm(mitmLeaf, httpsUpstream.Port, upstreamLeaf, httpUpstream.Port))
            {
                string local = await SendRequest(mitm.Port, root, "/local");
                Assert(local.Contains("200 OK") && local.EndsWith("local-response"), "local HTTPS mapping response");

                string forwarded = await SendRequest(mitm.Port, root, "/upstream");
                Assert(forwarded.Contains("200 OK") && forwarded.EndsWith("upstream-response"), "unmatched HTTPS request forwards upstream");
                string remoteHttps = await SendRequest(mitm.Port, root, "/remote-https?keep=1");
                Assert(remoteHttps.Contains("200 OK") && remoteHttps.EndsWith("remote-https-response"), "HTTPS to HTTPS remote mapping response");
                string remoteHttp = await SendRequest(mitm.Port, root, "/remote-http?keep=1");
                Assert(remoteHttp.Contains("200 OK") && remoteHttp.EndsWith("remote-http-response"), "HTTPS to HTTP remote mapping response");
                await mitm.Completion;
                await httpsUpstream.Completion;
                await httpUpstream.Completion;
                Assert(httpsUpstream.Requests[0].StartsWith("GET /upstream HTTP/1.1"), "upstream receives original HTTP/1.1 request");
                Assert(httpsUpstream.Requests[1].StartsWith("GET /target-https?keep=1 HTTP/1.1") && httpsUpstream.Requests[1].Contains("\r\nHost: " + UpstreamHost + "\r\n"), "HTTPS target receives rewritten path and Host");
                Assert(httpUpstream.LastRequest.StartsWith("GET /target-http?keep=1 HTTP/1.1") && httpUpstream.LastRequest.Contains("\r\nHost: plain.test:" + httpUpstream.Port + "\r\n"), "HTTP target receives rewritten path and Host");
            }
        }

        private static async Task<string> SendRequest(int port, X509Certificate2 root, string path)
        {
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(IPAddress.Loopback, port);
                using (var tls = new SslStream(client.GetStream(), false, (sender, certificate, chain, errors) => ValidatesToRoot(certificate, root)))
                {
                    await tls.AuthenticateAsClientAsync(MitmHost, null, SslProtocols.Tls12, false);
                    byte[] request = Encoding.ASCII.GetBytes("GET " + path + " HTTP/1.1\r\nHost: " + MitmHost + "\r\nConnection: close\r\n\r\n");
                    await tls.WriteAsync(request, 0, request.Length);
                    await tls.FlushAsync();
                    return await ReadToEnd(tls);
                }
            }
        }

        /// <summary>复现主工程的帧输入/回调输出：TLS 响应跨越多个 16 KB 记录时不能卡住。</summary>
        private static async Task RunFrameStreamLargeResponse(X509Certificate2 root, X509Certificate2 certificate)
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            try
            {
                listener.Start(); int port = ((IPEndPoint)listener.LocalEndpoint).Port;
                Task server = Task.Run(async () =>
                {
                    using (TcpClient peer = await listener.AcceptTcpClientAsync())
                    using (var bridge = new PocFrameStream(data => peer.GetStream().Write(data, 0, data.Length)))
                    {
                        Task rawPump = PumpRaw(peer.GetStream(), bridge);
                        using (var tls = new SslStream(bridge, false))
                        {
                            await tls.AuthenticateAsServerAsync(certificate, false, SslProtocols.Tls12, false);
                            await ReadHeaders(tls);
                            byte[] body = new byte[48 * 1024]; for (int i = 0; i < body.Length; i++) { body[i] = (byte)('A' + i % 26); }
                            byte[] header = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\nContent-Length: " + body.Length + "\r\nConnection: close\r\n\r\n");
                            await tls.WriteAsync(header, 0, header.Length); await tls.WriteAsync(body, 0, body.Length); await tls.FlushAsync();
                        }
                        bridge.Complete();
                        // TcpClient 释放后 PumpRaw 会自然结束；这里不能等它，否则客户端正等 TLS EOF、
                        // 服务端正等客户端先断开的经典半关闭死锁。
                        _ = rawPump.ContinueWith(t => { var ignored = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
                    }
                });
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(IPAddress.Loopback, port);
                    using (var tls = new SslStream(client.GetStream(), false, (s, c, ch, e) => ValidatesToRoot(c, root)))
                    {
                        await tls.AuthenticateAsClientAsync(MitmHost, null, SslProtocols.Tls12, false);
                        byte[] request = Encoding.ASCII.GetBytes("GET /large HTTP/1.1\r\nHost: " + MitmHost + "\r\nConnection: close\r\n\r\n");
                        await tls.WriteAsync(request, 0, request.Length); await tls.FlushAsync();
                        string response = await ReadToEnd(tls);
                        Assert(response.Contains("Content-Length: 49152") && response.Length >= 48 * 1024, "FrameStream forwards HTTPS response larger than one TLS record");
                    }
                }
                await server;
            }
            finally { listener.Stop(); }
        }

        private static async Task PumpRaw(Stream source, PocFrameStream sink)
        {
            byte[] buffer = new byte[16 * 1024]; int read;
            while ((read = await source.ReadAsync(buffer, 0, buffer.Length)) > 0) { byte[] copy = new byte[read]; Buffer.BlockCopy(buffer, 0, copy, 0, read); sink.Feed(copy); }
            sink.Complete();
        }

        private sealed class PocFrameStream : Stream
        {
            private readonly Queue<byte[]> queue = new Queue<byte[]>(); private readonly SemaphoreSlim signal = new SemaphoreSlim(0); private readonly Action<byte[]> write;
            private byte[] current; private int offset; private bool done;
            internal PocFrameStream(Action<byte[]> write) { this.write = write; }
            internal void Feed(byte[] data) { lock (queue) { if (done) { return; } queue.Enqueue(data); } signal.Release(); }
            internal void Complete() { lock (queue) { done = true; } signal.Release(); }
            public override bool CanRead { get { return true; } } public override bool CanSeek { get { return false; } } public override bool CanWrite { get { return true; } }
            public override long Length { get { throw new NotSupportedException(); } } public override long Position { get { throw new NotSupportedException(); } set { throw new NotSupportedException(); } }
            public override async Task<int> ReadAsync(byte[] buffer, int index, int count, CancellationToken token)
            {
                while (current == null || offset >= current.Length) { lock (queue) { if (queue.Count > 0) { current = queue.Dequeue(); offset = 0; break; } if (done) { return 0; } } await signal.WaitAsync(token); }
                int take = Math.Min(count, current.Length - offset); Buffer.BlockCopy(current, offset, buffer, index, take); offset += take; return take;
            }
            public override int Read(byte[] buffer, int offset, int count) { return ReadAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult(); }
            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token) { byte[] copy = new byte[count]; Buffer.BlockCopy(buffer, offset, copy, 0, count); write(copy); return Task.CompletedTask; }
            public override void Write(byte[] buffer, int offset, int count) { WriteAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult(); }
            public override void Flush() { } public override Task FlushAsync(CancellationToken token) { return Task.CompletedTask; }
            public override long Seek(long offset, SeekOrigin origin) { throw new NotSupportedException(); } public override void SetLength(long value) { throw new NotSupportedException(); }
        }

        private sealed class TestMitm : IDisposable
        {
            private readonly TcpListener listener;
            private readonly X509Certificate2 certificate;
            private readonly int upstreamPort;
            private readonly X509Certificate2 upstreamCertificate;
            private readonly int httpUpstreamPort;
            private readonly Task acceptLoop;
            private bool disposed;

            public int Port { get { return ((IPEndPoint)listener.LocalEndpoint).Port; } }
            public Task Completion { get { return acceptLoop; } }

            public TestMitm(X509Certificate2 certificate, int upstreamPort, X509Certificate2 upstreamCertificate, int httpUpstreamPort)
            {
                this.certificate = certificate;
                this.upstreamPort = upstreamPort;
                this.upstreamCertificate = upstreamCertificate;
                this.httpUpstreamPort = httpUpstreamPort;
                listener = new TcpListener(IPAddress.Loopback, 0);
                listener.Start();
                acceptLoop = Task.Run(AcceptFour);
            }

            private async Task AcceptFour()
            {
                for (int i = 0; i < 4; i++)
                {
                    using (TcpClient client = await listener.AcceptTcpClientAsync())
                    using (var clientTls = new SslStream(client.GetStream(), false))
                    {
                        await clientTls.AuthenticateAsServerAsync(certificate, false, SslProtocols.Tls12, false);
                        string request = await ReadHeaders(clientTls);
                        if (request.StartsWith("GET /local HTTP/1.1", StringComparison.Ordinal))
                        {
                            await WriteResponse(clientTls, "local-response");
                            continue;
                        }

                        bool remoteHttps = request.StartsWith("GET /remote-https?", StringComparison.Ordinal);
                        bool remoteHttp = request.StartsWith("GET /remote-http?", StringComparison.Ordinal);
                        string outgoing = remoteHttps ? Rewrite(request, UpstreamHost, 443, "/target-https") : remoteHttp ? Rewrite(request, "plain.test", httpUpstreamPort, "/target-http") : request;
                        using (var upstream = new TcpClient())
                        {
                            await upstream.ConnectAsync(IPAddress.Loopback, remoteHttp ? httpUpstreamPort : upstreamPort);
                            if (remoteHttp)
                            {
                                await Forward(upstream.GetStream(), clientTls, outgoing);
                            }
                            else using (var upstreamTls = new SslStream(upstream.GetStream(), false, (sender, remote, chain, errors) => SameCertificate(remote, upstreamCertificate)))
                            {
                                await upstreamTls.AuthenticateAsClientAsync(UpstreamHost, null, SslProtocols.Tls12, false);
                                await Forward(upstreamTls, clientTls, outgoing);
                            }
                        }
                    }
                }
            }

            private static async Task Forward(Stream upstream, Stream client, string request)
            {
                byte[] rawRequest = Encoding.ASCII.GetBytes(request);
                await upstream.WriteAsync(rawRequest, 0, rawRequest.Length); await upstream.FlushAsync();
                string response = await ReadToEnd(upstream);
                byte[] rawResponse = Encoding.ASCII.GetBytes(response);
                await client.WriteAsync(rawResponse, 0, rawResponse.Length); await client.FlushAsync();
            }

            private static string Rewrite(string request, string host, int port, string path)
            {
                int query = request.IndexOf('?'); int space = request.IndexOf(' ', query < 0 ? 0 : query);
                string suffix = query >= 0 && space > query ? request.Substring(query, space - query) : string.Empty;
                int firstEnd = request.IndexOf("\r\n", StringComparison.Ordinal);
                string rewritten = "GET " + path + suffix + " HTTP/1.1" + request.Substring(firstEnd);
                int hostStart = rewritten.IndexOf("Host: ", StringComparison.OrdinalIgnoreCase);
                int hostEnd = rewritten.IndexOf("\r\n", hostStart, StringComparison.Ordinal);
                return rewritten.Substring(0, hostStart) + "Host: " + host + (port == 443 ? string.Empty : ":" + port) + rewritten.Substring(hostEnd);
            }

            public void Dispose()
            {
                if (disposed) { return; }
                disposed = true;
                listener.Stop();
                try { acceptLoop.GetAwaiter().GetResult(); }
                catch (Exception) { }
            }
        }

        private sealed class TestHttpsUpstream : IDisposable
        {
            private readonly TcpListener listener;
            private readonly X509Certificate2 certificate;
            private readonly Task acceptLoop;
            private bool disposed;

            public int Port { get { return ((IPEndPoint)listener.LocalEndpoint).Port; } }
            public Task Completion { get { return acceptLoop; } }
            private readonly int expectedConnections;
            public System.Collections.Generic.List<string> Requests { get; } = new System.Collections.Generic.List<string>();

            public TestHttpsUpstream(X509Certificate2 certificate, int expectedConnections)
            {
                this.certificate = certificate;
                this.expectedConnections = expectedConnections;
                listener = new TcpListener(IPAddress.Loopback, 0);
                listener.Start();
                acceptLoop = Task.Run(AcceptAll);
            }

            private async Task AcceptAll()
            {
                for (int i = 0; i < expectedConnections; i++)
                {
                    using (TcpClient client = await listener.AcceptTcpClientAsync())
                    using (var tls = new SslStream(client.GetStream(), false))
                    {
                        await tls.AuthenticateAsServerAsync(certificate, false, SslProtocols.Tls12, false);
                        Requests.Add(await ReadHeaders(tls));
                        await WriteResponse(tls, i == 0 ? "upstream-response" : "remote-https-response");
                    }
                }
            }

            public void Dispose()
            {
                if (disposed) { return; }
                disposed = true;
                listener.Stop();
                try { acceptLoop.GetAwaiter().GetResult(); }
                catch (Exception) { }
            }
        }

        private sealed class TestHttpUpstream : IDisposable
        {
            private readonly TcpListener listener;
            private readonly Task acceptLoop;
            private bool disposed;
            public int Port { get { return ((IPEndPoint)listener.LocalEndpoint).Port; } }
            public Task Completion { get { return acceptLoop; } }
            public string LastRequest { get; private set; }
            public TestHttpUpstream()
            {
                listener = new TcpListener(IPAddress.Loopback, 0); listener.Start(); acceptLoop = Task.Run(AcceptOne);
            }
            private async Task AcceptOne()
            {
                using (TcpClient client = await listener.AcceptTcpClientAsync())
                {
                    LastRequest = await ReadHeaders(client.GetStream());
                    await WriteResponse(client.GetStream(), "remote-http-response");
                }
            }
            public void Dispose() { if (disposed) { return; } disposed = true; listener.Stop(); try { acceptLoop.GetAwaiter().GetResult(); } catch (Exception) { } }
        }

        private static X509Certificate2 CreateRootCa()
        {
            using (RSA key = RSA.Create(2048))
            {
                var request = new CertificateRequest("CN=WPE HTTPS MITM POC Root", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
                request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.CrlSign, true));
                request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));
                return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow.AddYears(2));
            }
        }

        private static X509Certificate2 CreateServerCertificate(string host, X509Certificate2 issuer)
        {
            using (RSA key = RSA.Create(2048))
            {
                var request = new CertificateRequest("CN=" + host, key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, true));
                request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, true));
                request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));
                var san = new SubjectAlternativeNameBuilder();
                san.AddDnsName(host);
                request.CertificateExtensions.Add(san.Build());
                byte[] serial = new byte[16];
                using (var random = RandomNumberGenerator.Create()) { random.GetBytes(serial); }
                using (X509Certificate2 publicLeaf = request.Create(issuer, DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow.AddDays(30), serial))
                using (X509Certificate2 leafWithKey = publicLeaf.CopyWithPrivateKey(key))
                {
                    return ReopenForSchannel(leafWithKey);
                }
            }
        }

        private static X509Certificate2 CreateSelfSignedServerCertificate(string host)
        {
            using (RSA key = RSA.Create(2048))
            {
                var request = new CertificateRequest("CN=" + host, key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, true));
                request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, true));
                request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));
                var san = new SubjectAlternativeNameBuilder();
                san.AddDnsName(host);
                request.CertificateExtensions.Add(san.Build());
                using (X509Certificate2 certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow.AddDays(30)))
                {
                    return ReopenForSchannel(certificate);
                }
            }
        }

        // .NET Framework 的 SslStream/Schannel 需要可供 Schannel 获取的私钥句柄；PFX 字节留在内存，
        // UserKeySet 只提供当前用户的临时密钥句柄，不向证书库安装根证书。
        private static X509Certificate2 ReopenForSchannel(X509Certificate2 certificate)
        {
            byte[] pfx = certificate.Export(X509ContentType.Pfx);
            return new X509Certificate2(pfx, (string)null, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable);
        }

        private static bool ValidatesToRoot(X509Certificate certificate, X509Certificate2 root)
        {
            if (certificate == null) { return false; }
            using (var chain = new X509Chain())
            {
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                chain.ChainPolicy.ExtraStore.Add(root);
                bool built = chain.Build(new X509Certificate2(certificate));
                bool expectedRoot = chain.ChainElements.Count >= 2
                    && string.Equals(chain.ChainElements[chain.ChainElements.Count - 1].Certificate.Thumbprint, root.Thumbprint, StringComparison.OrdinalIgnoreCase);
                if (built && expectedRoot) { return true; }

                // POC 不把根装进 Windows 根库；只允许这一项由该临时根造成，其他链错误仍拒绝。
                bool onlyUntrustedRoot = chain.ChainStatus.Length > 0;
                foreach (X509ChainStatus status in chain.ChainStatus)
                {
                    if (status.Status != X509ChainStatusFlags.UntrustedRoot) { onlyUntrustedRoot = false; break; }
                }
                if (!(expectedRoot && onlyUntrustedRoot))
                {
                    Console.Error.WriteLine("client chain validation failed: built=" + built + ", expectedRoot=" + expectedRoot + ", statuses=" + ChainStatuses(chain));
                }
                return expectedRoot && onlyUntrustedRoot;
            }
        }

        private static string ChainStatuses(X509Chain chain)
        {
            var text = new StringBuilder();
            foreach (X509ChainStatus status in chain.ChainStatus)
            {
                if (text.Length > 0) { text.Append(", "); }
                text.Append(status.Status);
            }
            return text.ToString();
        }

        private static bool SameCertificate(X509Certificate actual, X509Certificate2 expected)
        {
            return actual != null && string.Equals(new X509Certificate2(actual).Thumbprint, expected.Thumbprint, StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasDnsSubjectAlternativeName(X509Certificate2 certificate)
        {
            foreach (X509Extension extension in certificate.Extensions)
            {
                if (extension.Oid != null && extension.Oid.Value == "2.5.29.17") { return extension.RawData != null && extension.RawData.Length > 2; }
            }
            return false;
        }

        private static async Task<string> ReadHeaders(Stream stream)
        {
            var buffer = new MemoryStream();
            var one = new byte[1];
            while (buffer.Length < 64 * 1024)
            {
                int read = await stream.ReadAsync(one, 0, 1);
                if (read == 0) { throw new EndOfStreamException("peer closed before HTTP headers"); }
                buffer.WriteByte(one[0]);
                if (buffer.Length >= 4)
                {
                    byte[] bytes = buffer.GetBuffer();
                    int n = (int)buffer.Length;
                    if (bytes[n - 4] == 13 && bytes[n - 3] == 10 && bytes[n - 2] == 13 && bytes[n - 1] == 10) { return Encoding.ASCII.GetString(bytes, 0, n); }
                }
            }
            throw new InvalidDataException("HTTP headers exceed POC limit");
        }

        private static async Task<string> ReadToEnd(Stream stream)
        {
            using (var result = new MemoryStream())
            {
                var buffer = new byte[4096];
                int read;
                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0) { result.Write(buffer, 0, read); }
                return Encoding.ASCII.GetString(result.ToArray());
            }
        }

        private static async Task WriteResponse(Stream stream, string body)
        {
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            byte[] header = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: " + bodyBytes.Length + "\r\nConnection: close\r\n\r\n");
            await stream.WriteAsync(header, 0, header.Length);
            await stream.WriteAsync(bodyBytes, 0, bodyBytes.Length);
            await stream.FlushAsync();
        }

        private static void Assert(bool condition, string name)
        {
            if (!condition) { throw new InvalidOperationException("Assertion failed: " + name); }
            Console.WriteLine("PASS: " + name);
        }
    }
}
