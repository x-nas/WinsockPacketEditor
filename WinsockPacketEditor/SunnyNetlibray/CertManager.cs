using SunnyNetlibray.Internal;
using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace SunnyNetlibray
{
    /// <summary>
    /// 证书管理器，请参考易语言用法。
    /// </summary>
    public class CertManager
    {
        /// <summary>
        /// 不请求客户端证书。
        /// </summary>
        public const int NoClientCert = 0;

        /// <summary>
        /// 请求客户端证书。
        /// </summary>
        public const int RequestClientCert = 1;

        /// <summary>
        /// 至少发送一个证书。
        /// </summary>
        public const int RequireAnyClientCert = 2;

        /// <summary>
        /// 验证客户端证书（如果提供）。
        /// </summary>
        public const int VerifyClientCertIfGiven = 3;

        /// <summary>
        /// 要求并验证客户端证书。
        /// </summary>
        public const int RequireAndVerifyClientCert = 4;

        private long _context = 0;

        /// <summary>
        /// 初始化证书管理器。
        /// </summary>
        public CertManager()
        {
            _context = Bridge.CreateCertificate();
            SetInsecureSkipVerify();
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        ~CertManager()
        {
            Bridge.RemoveCertificate(_context);
        }

        /// <summary>
        /// 载入 P12 证书。
        /// </summary>
        /// <param name="p12FilePath">证书文件的路径。</param>
        /// <param name="password">证书的密码。</param>
        /// <returns>如果载入成功返回 true，否则返回 false。</returns>
        public bool LoadP12Certificate(string p12FilePath, string password)
        {
            return Bridge.LoadP12Certificate(_context, p12FilePath, password);
        }

        /// <summary>
        /// 载入 X509 证书。
        /// </summary>
        /// <param name="host">证书的主机名。</param>
        /// <param name="ca">证书的 CA 文件内容。</param>
        /// <param name="key">证书的密钥文件内容。</param>
        /// <returns>如果载入成功返回 true，否则返回 false。</returns>
        public bool LoadX509Certificate(string host, string ca, string key)
        {
            return Bridge.LoadX509Certificate(_context, host, ca, key);
        }

        /// <summary>
        /// 载入 X509 证书（不指定主机名）。
        /// </summary>
        /// <param name="ca">证书的 CA 文件内容。</param>
        /// <param name="key">证书的密钥文件内容。</param>
        /// <returns>如果载入成功返回 true，否则返回 false。</returns>
        public bool LoadX509Certificate(string ca, string key)
        {
            return LoadX509Certificate("", ca, key);
        }

        /// <summary>
        /// 载入 X509 密钥对证书。
        /// </summary>
        /// <param name="caPath">CA 证书的路径。</param>
        /// <param name="keyPath">密钥文件的路径。</param>
        /// <returns>如果载入成功返回 true，否则返回 false。</returns>
        public bool LoadX509KeyPair(string caPath, string keyPath)
        {
            return Bridge.LoadX509KeyPair(_context, caPath, keyPath);
        }

        /// <summary>
        /// 设置是否跳过主机验证。
        /// </summary>
        /// <param name="skip">如果为 true，将跳过主机验证。</param>
        public void SetInsecureSkipVerify(bool skip = true)
        {
            Bridge.SetInsecureSkipVerify(_context, skip);
        }

        /// <summary>
        /// 设置服务器名称，请先载入证书。
        /// </summary>
        /// <param name="serverName">证书上的主机名。</param>
        /// <returns>如果设置成功返回 true，否则返回 false。</returns>
        public bool SetServerName(string serverName)
        {
            return Bridge.SetServerName(_context, serverName);
        }

        /// <summary>
        /// 获取服务器名称，请先载入证书。
        /// </summary>
        /// <returns>返回证书上的主机名。</returns>
        public string GetServerName()
        {
            return Bridge.GetServerName(_context);
        }

        /// <summary>
        /// 获取证书的“颁发给”信息。
        /// </summary>
        /// <returns>返回证书的“颁发给”。</returns>
        public string GetCommonName()
        {
            return Bridge.GetCommonName(_context);
        }

        /// <summary>
        /// 添加客户端信任证书（文件）。
        /// </summary>
        /// <param name="filePath">证书文件路径。</param>
        /// <returns>如果添加成功返回 true，否则返回 false。</returns>
        public bool AddCertPoolPath(string filePath)
        {
            return Bridge.AddCertPoolPath(_context, filePath); ;
        }

        /// <summary>
        /// 添加客户端信任证书（文本）。
        /// </summary>
        /// <param name="trustedCertContent">信任的证书文件内容。</param>
        /// <returns>如果添加成功返回 true，否则返回 false。</returns>
        public bool AddCertPoolText(string trustedCertContent)
        {
            return Bridge.AddCertPoolText(_context, trustedCertContent);
        }

        /// <summary>
        /// 设置加密套件（Cipher Suites）。
        /// </summary>
        /// <param name="suites">加密套件的字符串。格式例如:1,2,10,65,120,45,6530</param>
        /// <returns>如果设置成功返回 true，否则返回 false。</returns>
        public bool SetCipherSuites(string suites)
        {
            return Bridge.SetCipherSuites(_context, suites);
        }

        /// <summary>
        /// 添加客户端认证。
        /// </summary>
        /// <param name="authType">客户端认证的上下文 ID。请使用以下常量之一：
        /// <ul>
        /// <li><br></br></li>
        /// <li><see cref="NoClientCert"/> 不请求客户端证书</li>
        /// <li><br></br></li>
        /// <li><see cref="RequestClientCert"/> 请求客户端证书</li>
        /// <li><br></br></li>
        /// <li><see cref="RequireAndVerifyClientCert"/> 要求并验证客户端证书</li>
        /// <li><br></br></li>
        /// <li><see cref="RequireAnyClientCert"/> 至少发送一个证书</li>
        /// <li><br></br></li>
        /// <li><see cref="VerifyClientCertIfGiven"/> 验证客户端证书</li>
        /// </ul>
        /// </param>
        /// <returns>如果设置成功返回 true，否则返回 false。</returns>
        public bool AddClientAuth(int authType)
        {
            return Bridge.AddClientAuth(_context, authType);
        }

        /// <summary>
        /// 重置证书管理器。
        /// </summary>
        public void Reset()
        {
            Bridge.RemoveCertificate(_context);
            _context = Bridge.CreateCertificate();
            SetInsecureSkipVerify();
        }

        /// <summary>
        /// 获取当前上下文 ID。
        /// </summary>
        /// <returns>当前上下文 ID。</returns>
        public long Context()
        {
            return _context;
        }

        /// <summary>
        /// 创建新的证书。
        /// </summary>
        /// <param name="domainName">证书域名，例如：www.baidu.com。</param>
        /// <param name="country">证书所属国家，默认 CN。</param>
        /// <param name="organization">证书所属公司名称，默认 Sunny。</param>
        /// <param name="department">证书所属部门名称，默认 Sunny。</param>
        /// <param name="province">证书签发机构所在省，默认 BeiJing。</param>
        /// <param name="city">证书签发机构所在市，默认 BeiJing。</param>
        /// <param name="expirationDays">到期时间（单位：天），默认 3650 天。</param>
        /// <returns>如果创建成功返回 true，否则返回 false。</returns>
        public bool CreateCertificate(string domainName, string country = "CN", string organization = "Sunny",
            string department = "Sunny", string province = "BeiJing", string city = "BeiJing", int expirationDays = 3650)
        {
            return Bridge.CreateCA(_context, country, organization, department, province, domainName, city, expirationDays);
        }

        /// <summary>
        /// 导出公钥。
        /// </summary>
        /// <returns>公钥字符串。</returns>
        public string ExportPublicKey()
        {
            return Bridge.ExportPub(_context);
        }

        /// <summary>
        /// 导出私钥。
        /// </summary>
        /// <returns>私钥字符串。</returns>
        public string ExportPrivateKey()
        {
            return Bridge.ExportKEY(_context);
        }

        /// <summary>
        /// 导出 CA。
        /// </summary>
        /// <returns>CA 字符串。</returns>
        public string ExportCA()
        {
            return Bridge.ExportCA(_context);
        }

        /// <summary>
        /// 导出 P12 文件。
        /// </summary>
        /// <param name="savePath">保存路径。</param>
        /// <param name="p12Password">P12 密码。</param>
        /// <returns>如果导出成功返回 true，否则返回 false。</returns>
        public bool ExportP12(string savePath, string p12Password)
        {
            return Bridge.ExportP12(_context, savePath, p12Password);
        }
    }
}