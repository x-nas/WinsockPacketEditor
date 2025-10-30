using SunnyNetlibray;
using SunnyNetlibray.Internal;

namespace SunnyNetlibray
{
    /// <summary>
    /// 表示一个基本的 HTTP 客户端，支持发送 HTTP 请求和处理响应。
    /// </summary>
    public class HTTPClient
    {
        private long _context = 0;

        public HTTPClient()
        {
            _context = Bridge.CreateHTTPClient();
            if (_context > 0)
            {
                SetRedirect(true);
            }
        }

        ~HTTPClient()
        {
            // 自动销毁
            Bridge.RemoveHTTPClient(_context);
            _context = 0;
        }

        /// <summary>
        /// 重新创建 HTTP 客户端。
        /// </summary>
        public void Reset()
        {
            Bridge.RemoveHTTPClient(_context);
            _context = Bridge.CreateHTTPClient();
            if (_context > 0)
            {
                SetRedirect(true);
            }
        }

        /// <summary>
        /// 打开 HTTP 请求。
        /// </summary>
        /// <param name="method">HTTP 方法，例如 POST、GET 等。</param>
        /// <param name="url">请求的 URL。</param>
        public void Open(string method, string url)
        {
            Bridge.HTTPOpen(_context, method, url);
        }

        /// <summary>
        /// 设置重定向选项。
        /// </summary>
        /// <param name="redirect">如果为 true，则禁止重定向。</param>
        /// <returns>设置是否成功。</returns>
        public bool SetRedirect(bool redirect)
        {
            return Bridge.HTTPSetRedirect(_context, redirect);
        }

        /// <summary>
        /// 设置请求头。
        /// </summary>
        /// <param name="headerName">请求头名称。</param>
        /// <param name="headerValue">请求头值。</param>
        public void SetHeader(string headerName, string headerValue)
        {
            Bridge.HTTPSetHeader(_context, headerName, headerValue);
        }
        /// <summary>
        /// 设置请求实际连接地址
        /// 设置后将不再使用URL或协议头中的HOST地址
        /// 某些时候,协议头中的HOST以及URL中的地址不能修改,修改后请求无法发送，这种情况下有用
        /// </summary>
        /// <param name="ip">例如:8.8.8.8:443,只能IP+端口，如果格式错误，不会使用</param>
        public void SetServerIP(string ip)
        {
            Bridge.HTTPSetServerIP(_context, ip);
        }
        /// <summary>
        /// <para>设置出口IP</para>
        /// </summary>
        /// <param name="ip">请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择</param>
        public void SetOutRouterIP(string ip)
        {
            Bridge.HTTPSetOutRouterIP(_context, ip);
        }
        /// <summary>
        /// 设置证书管理器。
        /// </summary>
        /// <param name="certManager">证书管理器实例。</param>
        /// <returns>设置是否成功。</returns>
        public bool SetCertificateManager(CertManager certManager)
        {
            return Bridge.HTTPSetCertManager(_context, certManager);
        }

        /// <summary>
        /// 设置代理。
        /// </summary>
        /// <param name="ProxyURL">代理 URL，指定要使用的代理地址。
        /// 例如，以下示例格式：
        ///  <list type="bullet">
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </param>
        public void SetProxy(string proxyUrl)
        {
            Bridge.HTTPSetProxyIP(_context, proxyUrl);
        }

        /// <summary>
        /// 设置超时设置。
        /// </summary>
        /// <param name="timeout">超时时间，单位毫秒，默认 30000（30 秒）。</param>
        public void SetTimeout(int timeout = 30000)
        {
            Bridge.HTTPSetTimeouts(_context, timeout);
        }

        /// <summary>
        /// 发送字节数组。
        /// </summary>
        /// <param name="data">要发送的数据。</param>
        public void Send(byte[] data)
        {
            Bridge.HTTPSendBin(_context, data);
        }

        /// <summary>
        /// 发送文本数据。
        /// </summary>
        /// <param name="data">要发送的文本数据。</param>
        public void Send(string data, string Encoding = "UTF-8")
        {
            Send(Tool.StrToBytes(data, Encoding));
        }
        public void Send()
        {
            Send(new byte[0]);
        }
        /// <summary>
        /// 获取响应内容长度。发送数据之后有效。
        /// </summary>
        /// <returns>响应内容长度。</returns>
        public int GetResponseLength()
        {
            return Bridge.HTTPGetBodyLen(_context);
        }

        /// <summary>
        /// 获取响应状态码。发送数据之后有效。
        /// </summary>
        /// <returns>响应状态码。</returns>
        public int GetResponseCode()
        {
            return Bridge.HTTPGetCode(_context);
        }

        /// <summary>
        /// 获取全部响应头。发送数据之后有效。
        /// </summary>
        /// <returns>响应头字符串。</returns>
        public string GetResponseHeaders()
        {
            return Bridge.HTTPGetHeads(_context);
        }

        /// <summary>
        /// 获取响应内容。发送数据之后有效。
        /// </summary>
        /// <returns>响应内容的字节数组。</returns>
        public byte[] GetResponseBody()
        {
            byte[] bs = Bridge.HTTPGetBody(_context);
            string[] ss = GetResponseHeaders().ToLower().Split('\n');
            string Encoding = "";
            for (int i = 0; i < ss.Length; i++)
            {
                if (ss[i].StartsWith("content-encoding"))
                {
                    if (ss[i].Contains("gzip"))
                    {
                        Encoding = "gzip"; break;
                    }
                    if (ss[i].Contains("br"))
                    {
                        Encoding = "br"; break;
                    }
                    if (ss[i].Contains("zlib"))
                    {
                        Encoding = "zlib"; break;
                    }
                    if (ss[i].Contains("zstd"))
                    {
                        Encoding = "zstd"; break;
                    }
                    if (ss[i].Contains("deflate"))
                    {
                        Encoding = "deflate"; break;
                    }
                }

            }
            switch (Encoding)
            {
                case "gzip":
                    {
                        byte[] raw2 = Compress.GzipUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            return raw2;
                        }
                        break;
                    }
                case "br":
                    {
                        byte[] raw2 = Compress.BrUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            return raw2;
                        }
                        break;
                    }
                case "deflate":
                    {
                        byte[] raw2 = Compress.DeflateUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            return raw2;
                        }
                        break;
                    }
                case "zstd":
                    {
                        byte[] raw2 = Compress.ZSTDUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            return raw2;
                        }
                        break;
                    }
                case "zlib":
                    {
                        byte[] raw2 = Compress.ZlibUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            return raw2;
                        }
                        break;
                    }
            }
            return bs;
        }
        /// <summary>
        /// 获取响应数据字节数组，自动解压缩。
        /// </summary>
        /// <returns>解压缩后的响应数据字节数组。</returns>


        /// <summary>
        /// 设置是否随机使用 TLS 指纹。
        /// </summary>
        /// <param name="random">如果为 true，则随机使用 TLS 指纹；否则不随机。</param>
        /// <returns>设置是否成功。</returns>
        public bool RandomJa3(bool random)
        {
            return Bridge.HTTPSetRandomTLS(_context, random);
        }

        /// <summary>
        /// 获取所有请求头。
        /// </summary>
        /// <returns>请求头字符串。</returns>
        public string GetRequestHeaders()
        {
            return Bridge.HTTPGetRequestHeader(_context);
        }

        /// <summary>
        /// 设置 HTTP/2 的指纹配置。
        /// <para>请使用以下常量模板之一：</para>
        /// <list type="bullet">
        ///   <item><see cref="Const.HTTP2_Fingerprint_Config_Firefox"/></item>
        ///   <item><see cref="Const.HTTP2_Fingerprint_Config_Opera"/></item>
        ///   <item><see cref="Const.HTTP2_Fingerprint_Config_Chrome_103_105"/></item>
        ///   <item><see cref="Const.HTTP2_Fingerprint_Config_Chrome_106_116"/></item>
        ///   <item><see cref="Const.HTTP2_Fingerprint_Config_Chrome_117_120_124"/></item>
        ///   <item><see cref="Const.HTTP2_Fingerprint_Config_Safari_IOS_17_0"/></item>
        ///   <item><see cref="Const.HTTP2_Fingerprint_Config_Safari_IOS_16_0"/></item>
        ///   <item><see cref="Const.HTTP2_Fingerprint_Config_Safari"/></item>
        /// </list>
        /// <para>你可以将模板中的数值随机，以达到随机指纹的效果。</para>
        /// 如果强制请求发送时使用HTTP/1.1 请填入参数 http/1.1
        /// </summary>
        /// <param name="config">指纹配置字符串。如果强制请求发送时使用HTTP/1.1 请填入参数 http/1.1</param>
        /// <returns>如果设置成功返回 true，否则返回 false。</returns>
        public bool SetH2Config(string config)
        {
            return Bridge.HTTPSetH2Config(_context, config);
        }
    }
}