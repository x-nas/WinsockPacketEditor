using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HttpsMitmPoc
{
    internal static class Program
    {
        private const string MitmHost = "mapped.test";
        private const string UpstreamHost = "upstream.test";

        private static int Main()
        {
            try
            {
                using (var root = CreateRootCa())
                using (var mitmLeaf = CreateServerCertificate(MitmHost, root))
                using (var upstreamLeaf = CreateSelfSignedServerCertificate(UpstreamHost))
                {
                    Assert(HasDnsSubjectAlternativeName(mitmLeaf), "MITM leaf has DNS SAN");
                    RunProxyRoundTrip(root, mitmLeaf, upstreamLeaf).GetAwaiter().GetResult();
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

        private static async Task RunProxyRoundTrip(X509Certificate2 root, X509Certificate2 mitmLeaf, X509Certificate2 upstreamLeaf)
        {
            using (var upstream = new TestUpstream(upstreamLeaf))
            using (var mitm = new TestMitm(mitmLeaf, upstream.Port, upstreamLeaf))
            {
                string local = await SendRequest(mitm.Port, root, "/local");
                Assert(local.Contains("200 OK") && local.EndsWith("local-response"), "local HTTPS mapping response");

                string forwarded = await SendRequest(mitm.Port, root, "/upstream");
                Assert(forwarded.Contains("200 OK") && forwarded.EndsWith("upstream-response"), "unmatched HTTPS request forwards upstream");
                await mitm.Completion;
                await upstream.Completion;
                Assert(upstream.LastRequest != null && upstream.LastRequest.StartsWith("GET /upstream HTTP/1.1"), "upstream receives original HTTP/1.1 request");
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

        private sealed class TestMitm : IDisposable
        {
            private readonly TcpListener listener;
            private readonly X509Certificate2 certificate;
            private readonly int upstreamPort;
            private readonly X509Certificate2 upstreamCertificate;
            private readonly Task acceptLoop;
            private bool disposed;

            public int Port { get { return ((IPEndPoint)listener.LocalEndpoint).Port; } }
            public Task Completion { get { return acceptLoop; } }

            public TestMitm(X509Certificate2 certificate, int upstreamPort, X509Certificate2 upstreamCertificate)
            {
                this.certificate = certificate;
                this.upstreamPort = upstreamPort;
                this.upstreamCertificate = upstreamCertificate;
                listener = new TcpListener(IPAddress.Loopback, 0);
                listener.Start();
                acceptLoop = Task.Run(AcceptTwo);
            }

            private async Task AcceptTwo()
            {
                for (int i = 0; i < 2; i++)
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

                        using (var upstream = new TcpClient())
                        {
                            await upstream.ConnectAsync(IPAddress.Loopback, upstreamPort);
                            using (var upstreamTls = new SslStream(upstream.GetStream(), false,
                                (sender, remote, chain, errors) => SameCertificate(remote, upstreamCertificate)))
                            {
                                await upstreamTls.AuthenticateAsClientAsync(UpstreamHost, null, SslProtocols.Tls12, false);
                                byte[] rawRequest = Encoding.ASCII.GetBytes(request);
                                await upstreamTls.WriteAsync(rawRequest, 0, rawRequest.Length);
                                await upstreamTls.FlushAsync();
                                string response = await ReadToEnd(upstreamTls);
                                byte[] rawResponse = Encoding.ASCII.GetBytes(response);
                                await clientTls.WriteAsync(rawResponse, 0, rawResponse.Length);
                                await clientTls.FlushAsync();
                            }
                        }
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

        private sealed class TestUpstream : IDisposable
        {
            private readonly TcpListener listener;
            private readonly X509Certificate2 certificate;
            private readonly Task acceptLoop;
            private bool disposed;

            public int Port { get { return ((IPEndPoint)listener.LocalEndpoint).Port; } }
            public Task Completion { get { return acceptLoop; } }
            public string LastRequest { get; private set; }

            public TestUpstream(X509Certificate2 certificate)
            {
                this.certificate = certificate;
                listener = new TcpListener(IPAddress.Loopback, 0);
                listener.Start();
                acceptLoop = Task.Run(AcceptOne);
            }

            private async Task AcceptOne()
            {
                using (TcpClient client = await listener.AcceptTcpClientAsync())
                using (var tls = new SslStream(client.GetStream(), false))
                {
                    await tls.AuthenticateAsServerAsync(certificate, false, SslProtocols.Tls12, false);
                    LastRequest = await ReadHeaders(tls);
                    await WriteResponse(tls, "upstream-response");
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
