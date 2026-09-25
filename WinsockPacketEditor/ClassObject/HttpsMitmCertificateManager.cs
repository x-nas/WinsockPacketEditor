using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WinsockPacketEditor
{
    /// <summary>
    /// HTTPS 本地映射的每服务器根 CA。私钥以机器 DPAPI 密文保存在 ProgramData，
    /// 从不进入数据库、备份、日志或发布包。此类不安装证书；安装必须由 UI 明确确认后调用 Trust。
    /// </summary>
    internal sealed class HttpsMitmCertificateManager
    {
        internal sealed class Status
        {
            public bool Exists;
            public bool Trusted;
            public string Subject = string.Empty;
            public string Thumbprint = string.Empty;
            public string Error = string.Empty;
        }

        private static readonly object Gate = new object();
        private static readonly byte[] Entropy = { 0x57, 0x50, 0x45, 0x2D, 0x48, 0x54, 0x54, 0x50, 0x53, 0x2D, 0x43, 0x41, 0x2D, 0x31 };

        // WPE 本身以管理员权限运行。根 CA 必须属于这台服务器，而不是某个登录用户：
        // 服务重启、切换管理员帐号都复用同一张根证书，已安装客户端无需反复装证书。
        private static string DirectoryPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "WPE64", "https-mitm");
        private static string PfxPath => Path.Combine(DirectoryPath, "root-ca.pfx.dpapi");
        // 2.4 HTTPS 首版曾放在 LocalAppData + CurrentUser DPAPI；迁移保留原 PFX，绝不换根。
        private static string LegacyPfxPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WPE64", "https-mitm", "root-ca.pfx.dpapi");

        public Status GetStatus()
        {
            lock (Gate)
            {
                var status = new Status();
                try { EnsureServerMaterial(); }
                catch (Exception ex) { status.Exists = File.Exists(PfxPath) || File.Exists(LegacyPfxPath); status.Error = ex.Message; return status; }
                status.Exists = File.Exists(PfxPath);
                if (!status.Exists) { return status; }
                try
                {
                    using (X509Certificate2 root = Load())
                    {
                        status.Subject = root.Subject;
                        status.Thumbprint = root.Thumbprint ?? string.Empty;
                        status.Trusted = IsTrusted(root);
                    }
                }
                catch (Exception ex) { status.Error = ex.Message; }
                return status;
            }
        }

        public Status Create()
        {
            lock (Gate)
            {
                EnsureServerMaterial();
                if (!File.Exists(PfxPath))
                {
                    using (RSA key = RSA.Create(2048))
                    {
                        var request = new CertificateRequest("CN=WPE64", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
                        request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.CrlSign, true));
                        request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));
                        using (X509Certificate2 root = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow.AddYears(5)))
                        {
                            Directory.CreateDirectory(DirectoryPath);
                            byte[] protectedPfx = ProtectedData.Protect(root.Export(X509ContentType.Pfx), Entropy, DataProtectionScope.LocalMachine);
                            File.WriteAllBytes(PfxPath, protectedPfx);
                        }
                    }
                }
                return GetStatus();
            }
        }

        public bool Trust()
        {
            lock (Gate)
            {
                using (X509Certificate2 root = Load())
                using (var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser))
                {
                    store.Open(OpenFlags.ReadWrite);
                    if (!IsTrustedInOpenStore(store, root)) { store.Add(new X509Certificate2(root.Export(X509ContentType.Cert))); }
                    return true;
                }
            }
        }

        public bool Untrust()
        {
            lock (Gate)
            {
                using (X509Certificate2 root = Load())
                using (var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser))
                {
                    store.Open(OpenFlags.ReadWrite);
                    foreach (X509Certificate2 item in store.Certificates.Find(X509FindType.FindByThumbprint, root.Thumbprint, false)) { store.Remove(item); }
                    return true;
                }
            }
        }

        /// <summary>
        /// 导出公开根证书。cer/crt/der 为 DER；pem 为 PEM；android 为 Android 系统 CA 目录所需的
        /// &lt;subject_hash_old&gt;.0（PEM）文件。私钥永不导出。
        /// 返回实际写入路径：.0 的文件名由 Android 规则决定，不能由用户自定义。
        /// </summary>
        public string GetExportFileName(string format)
        {
            lock (Gate)
            using (X509Certificate2 root = Load())
            {
                switch ((format ?? string.Empty).ToLowerInvariant())
                {
                    case "crt": return "WPE64.crt";
                    case "der": return "WPE64.der";
                    case "pem": return "WPE64.pem";
                    case "android": return AndroidSystemFileName(root);
                    default: return "WPE64.cer";
                }
            }
        }

        public string ExportCertificate(string path, string format)
        {
            if (string.IsNullOrWhiteSpace(path)) { return null; }
            lock (Gate)
            using (X509Certificate2 root = Load())
            {
                string kind = (format ?? string.Empty).ToLowerInvariant();
                if (kind == "pem")
                {
                    path = Path.ChangeExtension(path, ".pem");
                    File.WriteAllText(path, ToPem(root), new UTF8Encoding(false));
                    return path;
                }

                if (kind == "android")
                {
                    string directory = Path.GetDirectoryName(path);
                    if (string.IsNullOrEmpty(directory)) { directory = Environment.CurrentDirectory; }
                    string androidPath = Path.Combine(directory, AndroidSystemFileName(root));
                    File.WriteAllText(androidPath, ToPem(root), new UTF8Encoding(false));
                    return androidPath;
                }

                string extension = kind == "crt" ? ".crt" : kind == "der" ? ".der" : ".cer";
                path = Path.ChangeExtension(path, extension);
                File.WriteAllBytes(path, root.Export(X509ContentType.Cert));
                return path;
            }
        }

        public bool DeleteMaterial()
        {
            lock (Gate)
            {
                // 删除前同步撤销信任，避免把无法再由本程序管理的根证书遗留在系统信任库。
                // DPAPI 材料损坏时仍必须允许用户删除残留文件；此时没有可用指纹可撤销。
                if (File.Exists(PfxPath) || File.Exists(LegacyPfxPath)) { try { Untrust(); } catch { } }
                if (File.Exists(PfxPath)) { File.Delete(PfxPath); }
                if (File.Exists(LegacyPfxPath)) { File.Delete(LegacyPfxPath); }
                return true;
            }
        }

        /// <summary>动态站点叶证书只保存在内存；调用方负责 Dispose。</summary>
        public X509Certificate2 CreateLeaf(string host)
        {
            if (string.IsNullOrWhiteSpace(host)) { throw new ArgumentException("HTTPS 映射缺少主机名", nameof(host)); }
            lock (Gate)
            using (X509Certificate2 root = Load())
            using (RSA key = RSA.Create(2048))
            {
                var request = new CertificateRequest("CN=" + host, key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, true));
                request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, true));
                request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));
                var san = new SubjectAlternativeNameBuilder();
                IPAddress ip;
                if (IPAddress.TryParse(host, out ip)) { san.AddIpAddress(ip); }
                else { san.AddDnsName(host); }
                request.CertificateExtensions.Add(san.Build());
                byte[] serial = new byte[16];
                using (var random = RandomNumberGenerator.Create()) { random.GetBytes(serial); }
                using (X509Certificate2 publicLeaf = request.Create(root, DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow.AddDays(30), serial))
                using (X509Certificate2 leaf = publicLeaf.CopyWithPrivateKey(key))
                {
                    return new X509Certificate2(leaf.Export(X509ContentType.Pfx), (string)null, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable);
                }
            }
        }

        private static X509Certificate2 Load()
        {
            EnsureServerMaterial();
            if (!File.Exists(PfxPath)) { throw new FileNotFoundException("HTTPS 映射根证书尚未创建"); }
            byte[] pfx = ProtectedData.Unprotect(File.ReadAllBytes(PfxPath), Entropy, DataProtectionScope.LocalMachine);
            return new X509Certificate2(pfx, (string)null, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable);
        }

        /// <summary>将首版按用户保存的根 CA 原样迁移为服务器材料；成功前不删除旧文件。</summary>
        private static void EnsureServerMaterial()
        {
            if (File.Exists(PfxPath)) { return; }
            if (!File.Exists(LegacyPfxPath)) { return; }

            byte[] legacy = ProtectedData.Unprotect(File.ReadAllBytes(LegacyPfxPath), Entropy, DataProtectionScope.CurrentUser);
            byte[] machine = ProtectedData.Protect(legacy, Entropy, DataProtectionScope.LocalMachine);
            Directory.CreateDirectory(DirectoryPath);
            File.WriteAllBytes(PfxPath, machine);
            File.Delete(LegacyPfxPath);
        }

        private static string ToPem(X509Certificate2 certificate)
        {
            string base64 = Convert.ToBase64String(certificate.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks);
            return "-----BEGIN CERTIFICATE-----\r\n" + base64 + "\r\n-----END CERTIFICATE-----\r\n";
        }

        private static string AndroidSystemFileName(X509Certificate2 certificate)
        {
            // Android 的 cacerts 命名采用 OpenSSL subject_hash_old：X509_NAME 的 DER 做 MD5，
            // 取摘要前四字节的小端整数。X500DistinguishedName.RawData 就是该 X509_NAME DER。
            using (MD5 md5 = MD5.Create())
            {
                byte[] digest = md5.ComputeHash(certificate.SubjectName.RawData);
                return BitConverter.ToUInt32(digest, 0).ToString("x8") + ".0";
            }
        }

        private static bool IsTrusted(X509Certificate2 root)
        {
            using (var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                return IsTrustedInOpenStore(store, root);
            }
        }

        private static bool IsTrustedInOpenStore(X509Store store, X509Certificate2 root)
        {
            return store.Certificates.Find(X509FindType.FindByThumbprint, root.Thumbprint, false).Count > 0;
        }
    }
}
