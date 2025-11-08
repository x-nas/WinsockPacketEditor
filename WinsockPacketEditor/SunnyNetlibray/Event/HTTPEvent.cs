using System;
using SunnyNetlibray.Internal;
using System.Text.RegularExpressions;

namespace SunnyNetlibray.Event
{

    public class HTTPEvent
    {
        /// <summary>
        /// HTTP 事件类型常量：发起请求。
        /// </summary>
        public const int EventType_HTTP_Request = 1;

        /// <summary>
        /// HTTP 事件类型常量：请求完成。
        /// </summary>
        public const int EventType_HTTP_Response = 2;

        /// <summary>
        /// HTTP 事件类型常量：请求错误。
        /// </summary>
        public const int EventType_HTTP_Error = 3;

        private long __SunnyNetContext;
        private long __TheologyID;
        private long __MessageId;
        private long __EventType;
        private string __Method;
        private string __Url;
        private string __err;
        private bool ___Debug;
        private long __pid;
        private Request __Request;
        private Response __Response;

        /// <summary>
        /// HTTPEvent 构造函数，用于初始化事件对象。
        /// </summary>
        /// <param name="SunnyContext">Sunny 上下文。</param>
        /// <param name="TheologyID">事件的唯一标识符。</param>
        /// <param name="MessageId">消息 ID。</param>
        /// <param name="EventType">事件类型。</param>
        /// <param name="Method">HTTP 方法（如 GET、POST）。</param>
        /// <param name="Url">请求的 URL。</param>
        /// <param name="err">错误信息（如果有）。</param>
        /// <param name="pid">进程 ID。</param>
        public HTTPEvent(IntPtr SunnyNetContext, IntPtr TheologyID, IntPtr MessageId, IntPtr EventType, string Method, string Url, string err, IntPtr pid)
        {
            __SunnyNetContext = SunnyNetContext.ToInt64();
            __TheologyID = TheologyID.ToInt64();
            __MessageId = MessageId.ToInt64();
            __EventType = EventType.ToInt64();
            __Method = Method;
            __Url = Url;
            __err = err;
            __pid = pid.ToInt64();
            ___Debug = __err == "Debug";
            if (___Debug)
            {
                __err = "";
            }
            __Request = new Request(__MessageId);
            __Response = new Response(__MessageId);
        }
        /// <summary>
        /// 获取请求的 URL。
        /// </summary>
        /// <returns>请求的 URL。</returns>
        public string URL()
        {
            return __Url;
        }

        /// <summary>
        /// 获取请求方法。
        /// </summary>
        /// <returns>HTTP 方法。</returns>
        public string Method()
        {
            return __Method;
        }

        /// <summary>
        /// 返回唯一 ID。
        /// </summary>
        /// <returns>事件的唯一标识符。</returns>
        public long TheologyID()
        {
            return __TheologyID;
        }

        /// <summary>
        /// SunnyNet 上下文
        /// </summary>
        public long Context()
        {
            return __SunnyNetContext;
        }

        /// <summary>
        /// 如果开启了身份验证模式，将返回客户端使用的 S5 账号。
        /// </summary>
        /// <returns>客户端的 S5 账号。</returns>
        public string GetUser()
        {
            return Bridge.SunnyNetGetSocket5User(__TheologyID);
        }

        /// <summary>
        /// 返回事件由哪个进程发起。如果返回 0，表示远程设备通过代理连接。
        /// </summary>
        /// <returns>进程 ID。</returns>
        public int PID()
        {
            return (int)__pid;
        }

        /// <summary>
        /// 请求错误的信息。当 Type() 为 Conn.EventType_HTTP_Error 时有效。
        /// </summary>
        /// <returns>错误信息。</returns>
        public string Error()
        {
            return __err;
        }

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <remarks>
        /// 请使用以下常量来判断事件类型：
        /// <list type="bullet">
        ///   <item><see cref="HTTPEvent.EventType_HTTP_Request"/> 发起请求</item>
        ///   <item><see cref="HTTPEvent.EventType_HTTP_Response"/> 请求完成</item>
        ///   <item><see cref="HTTPEvent.EventType_HTTP_Error"/> 请求错误</item>
        /// </list>
        /// </remarks>
        /// <returns>HTTP 当前 事件类型。</returns>
        public int Type()
        {
            return (int)__EventType;
        }
        public new int GetType()
        {
            return Type();

        }

        /// <summary>
        /// 获取数据来源客户端的 IP 地址。
        /// </summary>
        /// <returns>客户端 IP 地址。</returns>
        public string ClientIP()
        {
            return Bridge.GetRequestClientIp(__MessageId);
        }
        /// <summary>
        /// 获取请求对象。当 Type() 为 Conn.EventType_HTTP_Request 时有效。
        /// </summary>
        /// <returns>请求对象。</returns>
        public Request Request()
        {
            return __Request;
        }

        /// <summary>
        /// 获取响应对象。当 Type() 为以下情况时有效:
        /// <list type="bullet">
        ///     <item>Conn.EventType_HTTP_Request - 发起请求</item>
        ///     <item>或</item>
        ///     <item>Conn.EventType_HTTP_Response - 请求完成</item>
        /// </list>
        /// </summary>
        /// <returns>响应对象。</returns>
        public Response Response()
        {
            return __Response;
        }
    }

    public class Request
    {
        private long MessageId = 0;

        /// <summary>
        /// 初始化 Request 对象。
        /// </summary>
        /// <param name="_MessageId">消息 ID。</param>
        public Request(long _MessageId)
        {
            MessageId = _MessageId;
        }

        /// <summary>
        /// 将原始请求数据保存到文件。
        /// 请使用 "Conn.Request().IsRequestRawBody()" 来检查当前请求是否为原始 body。
        /// 如果是，将无法修改提交的 Body，请使用此命令来储存原始提交数据到文件。
        /// </summary>
        /// <param name="SavePath">保存路径。</param>
        public void RawRequestDataToFile(string SavePath)
        {
            Bridge.RawRequestDataToFile(MessageId, SavePath);
        }

        /// <summary>
        /// 检查当前请求是否为原始 body。
        /// 如果是，将无法修改提交的 Body，请使用 "Conn.Request().RawRequestDataToFile(filePath)" 命令来储存到文件。
        /// 如果当前请求为原始 body，返回 true；否则返回 false。
        /// </summary>
        /// <returns>如果是原始 body 返回 true， 否则返回 false。</returns>
        public bool IsRequestRawBody()
        {
            return Bridge.IsRequestRawBody(MessageId);
        }

        /// <summary>
        /// 请求协议头中去除压缩标记。
        /// 如果不删除压缩标记，返回数据可能是压缩后的。
        /// </summary>
        public void RemoveCompressionMark()
        {
            DelHeader("Accept-Encoding");
        }

        /// <summary>
        /// 获取 POST 提交数据长度。
        /// </summary>
        /// <returns>POST 数据长度。</returns>
        public int BodyLen()
        {
            return Bridge.GetRequestBodyLen(MessageId);
        }
        /// <summary>
        /// <para>设置出口IP</para>
        /// <para>你也可以在中间件中设置全局出口IP</para>
        /// </summary>
        /// <param name="ip">请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择</param>
        public void SetOutRouterIP(String ip)
        {
            Bridge.RequestSetOutRouterIP(MessageId, ip);
        }
        /// <summary>
        /// 获取 POST 提交数据 
        /// </summary>
        /// <returns>POST 提交数据的字节数组。</returns>
        public EventValue Body()
        {
            return new EventValue(Bridge.GetRequestBody(MessageId));
        }

        /// <summary>
        /// 修改 POST 提交数据，传入字节数组。
        /// </summary>
        /// <param name="data">要设置的字节数组。</param>
        /// <returns>如果成功修改，返回 true；否则返回 false。</returns>
        public bool Body(byte[] data)
        {
            return Bridge.SetRequestData(MessageId, data);
        }

        /// <summary>
        /// 修改 POST 提交数据，传入字符串。
        /// </summary>
        /// <param name="data">要设置的字符串。</param>
        /// <param name="Encoding">编码格式，默认为 UTF-8。</param>
        /// <returns>如果成功修改，返回 true；否则返回 false。</returns>
        public bool Body(string data, string Encoding = "UTF-8")
        {
            return Body(Tool.StrToBytes(data, Encoding));
        }

        /// <summary>
        /// 终止请求。在发起请求时使用，使用本命令后，此请求将不会被发送出去。
        /// </summary>
        public void Stop()
        {
            Bridge.SetResponseHeader(MessageId, "Connection", "Close");
        }

        /// <summary>
        /// 设置请求超时，以毫秒为单位。
        /// </summary>
        /// <param name="OutTime">超时时间，单位为毫秒。</param>
        public void SetOutTime(int OutTime)
        {
            Bridge.SetRequestOutTime(MessageId, OutTime);
        }

        /// <summary>
        /// 设置 HTTP/2 指纹。如果服务器支持则使用，否则将不使用。
        /// 请使用以下常量模板之一，模板中的数值可以随机化以达到随机指纹的效果。
        /// </summary>
        /// <param name="h2Config">HTTP/2 配置字符串。</param>
        /// <returns>如果成功设置配置，返回 true；否则返回 false。</returns>
        public bool SetH2Config(string config)
        {
            return Bridge.SetRequestHTTP2Config(MessageId, config);
        }

        /// <summary>
        /// 随机化请求的 JA3 指纹。
        /// 如果全局设置中没有开启 TLS 指纹随机，可以单独使用此命令来随机化 JA3 指纹。
        /// </summary>
        /// <returns>如果成功随机化指纹，返回 true；否则返回 false。</returns>
        public bool RandomJA3()
        {
            return Bridge.RandomRequestCipherSuites(MessageId);
        }
        /// <summary>
        /// 设置代理。
        /// 例如，以下示例格式：
        ///  <list type="bullet"> 
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </summary>
        /// <param name="ProxyUrl">代理 URL，指定要使用的代理地址。
        /// 例如，以下示例格式：
        ///  <list type="bullet"> 
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </param>
        /// <param name="OutTime">代理超时，单位毫秒。</param>
        /// <returns>如果成功设置代理，返回 true；否则返回 false。</returns>
        public bool SetProxy(string ProxyUrl, int outTime)
        {
            return Bridge.SetRequestProxy(MessageId, ProxyUrl, outTime);
        }
        /// <summary>
        /// 重置完整的协议头。
        /// </summary>
        /// <param name="Headers">完整的协议头字符串。</param>
        public void SetALLHeaders(string Headers)
        {
            Bridge.SetRequestALLHeader(MessageId, Headers);
        }

        /// <summary>
        /// 修改全部 Cookie，设置请求的全部 Cookies，例如 a=1;b=2;c=3。
        /// </summary>
        /// <param name="cookie">Cookies 字符串，无需前缀（Cookie:）。</param>
        public void SetAllCookie(string cookie)
        {
            Bridge.SetRequestAllCookie(MessageId, cookie);
        }

        /// <summary>
        /// 设置多个同名的协议头。
        /// 如果本身无指定的协议头，则为新增；如果有，则为修改。
        /// </summary>
        /// <param name="key">协议头名称。</param>
        /// <param name="value">协议头值的数组。</param>
        public void SetHeader(string key, string[] value)
        {
            SetHeader(key, string.Join("\n", value));
        }

        /// <summary>
        /// 设置单个协议头。
        /// 如果本身无指定的协议头，则为新增；如果有，则为修改。
        /// </summary>
        /// <param name="key">协议头名称。</param>
        /// <param name="value">协议头值。</param>
        public void SetHeader(string key, string value)
        {
            Bridge.SetRequestHeader(MessageId, key, value);
        }

        /// <summary>
        /// 修改请求的 URL。
        /// 可以用于转向，例如从网址 A 转向网址 B。
        /// </summary>
        /// <param name="newUrl">新的 URL。</param>
        /// <returns>如果成功修改，返回 true；否则返回 false。</returns>
        public bool SetUrl(string newUrl)
        {
            return Bridge.SetRequestUrl(MessageId, newUrl);
        }

        /// <summary>
        /// 设置 Cookie。
        /// 如果 key 存在则修改，不存在则新增。
        /// </summary>
        /// <param name="key">Cookie 名。</param>
        /// <param name="value">Cookie 值。</param>
        public void SetCookie(string key, string value)
        {
            Bridge.SetRequestCookie(MessageId, key, value);
        }

        /// <summary>
        /// 删除指定的协议头。
        /// </summary>
        /// <param name="key">协议头名称。</param>
        public void DelHeader(string key)
        {
            Bridge.DelRequestHeader(MessageId, key);
        }

        /// <summary>
        /// 获取全部协议头。
        /// </summary>
        /// <returns>返回所有协议头的字符串。</returns>
        public string GetAllHeader()
        {
            return Bridge.GetRequestAllHeader(MessageId);
        }

        /// <summary>
        /// 获取指定协议头的数组。
        /// </summary>
        /// <param name="key">协议头名称。</param>
        /// <returns>返回指定名称的协议头值数组。</returns>
        public string[] GetHeaderArray(string key)
        {
            return Bridge.GetRequestHeader(MessageId, key).Replace("\r", "").Split('\n');
        }

        /// <summary>
        /// 获取指定协议头。
        /// 如果有多个同名协议头，将返回第一个。
        /// </summary>
        /// <param name="key">协议头名称。</param>
        /// <returns>返回指定名称的协议头值。</returns>
        public string GetHeader(string key)
        {
            string[] sa = GetHeaderArray(key);
            if (sa.Length < 1)
            {
                return "";
            }
            return sa[0];
        }

        /// <summary>
        /// 获取请求的协议版本，例如 HTTP/1.1。
        /// </summary>
        /// <returns>请求的协议版本。</returns>
        public string GetProto()
        {
            return Bridge.GetRequestProto(MessageId);
        }

        /// <summary>
        /// 获取全部 Cookies。
        /// </summary>
        /// <returns>返回所有 Cookies 的字符串。</returns>
        public string GetCookies()
        {
            return Bridge.GetRequestALLCookie(MessageId);
        }

        /// <summary>
        /// 获取指定 Cookie。
        /// </summary>
        /// <param name="key">Cookie 名。</param>
        /// <returns>返回指定 Cookie 的字符串。</returns>
        public string GetCookie(string key)
        {
            return Bridge.GetRequestCookie(MessageId, key);
        }

        /// <summary>
        /// 获取指定 Cookie，不包含键名。
        /// </summary>
        /// <param name="key">Cookie 名。</param>
        /// <returns>返回指定 Cookie 的值。</returns>
        public string GetCookie_value(string key)
        {
            string s = GetCookie(key);
            string[] arr = Regex.Split(s, "=", RegexOptions.IgnoreCase);
            if (arr.Length < 2)
            {
                return "";
            }
            string value = arr[1].Replace(arr[0] + "=", "");
            return value;
        }

        /// <summary>
        /// 删除全部协议头。
        /// </summary>
        public void DelAllHeader()
        {
            string[] arr = Regex.Split(GetAllHeader(), "\r\n", RegexOptions.IgnoreCase);
            foreach (string s in arr)
            {
                string[] arr1 = Regex.Split(s, ":", RegexOptions.IgnoreCase);
                if (arr1.Length >= 1)
                {
                    DelHeader(arr1[0]);
                }
            }
        }
    }

    public class Response
    {
        private long MessageId = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="_MessageId">消息 ID。</param>
        public Response(long _MessageId)
        {
            MessageId = _MessageId;
        }

        /// <summary>
        /// 修改响应状态码。
        /// </summary>
        /// <param name="Code">状态码，默认为 200。</param>
        public void StatusCode(int Code = 200)
        {
            Bridge.SetResponseStatus(MessageId, Code);
        }

        /// <summary>
        /// 获取响应状态码。
        /// </summary>
        /// <returns>响应状态码。</returns>
        public int StatusCode()
        {
            return Bridge.GetResponseStatusCode(MessageId);
        }

        /// <summary>
        /// 获取状态码对应的状态文本。
        /// </summary>
        /// <returns>状态文本。</returns>
        public string StatusText()
        {
            return Bridge.GetResponseStatus(MessageId);
        }

        /// <summary>
        /// 获取服务器响应 IP 地址。
        /// </summary>
        /// <returns>服务器响应 IP 地址。</returns>
        public string ServerAddress()
        {
            return Bridge.GetResponseServerAddress(MessageId);
        }

        /// <summary>
        /// 获取响应数据长度。
        /// </summary>
        /// <returns>响应数据的字节长度。</returns>
        public int BodyLen()
        {
            return Bridge.GetResponseBodyLen(MessageId);
        }
        /// <summary>
        /// 获取响应数据字节数组。
        /// </summary>
        /// <returns>响应数据字节数组。</returns>
        public EventValue Body()
        {
            return new EventValue(Bridge.GetResponseBody(MessageId));
        }

        /// <summary>
        /// 修改响应数据字节数组。
        /// </summary>
        /// <param name="data">新的响应数据字节数组。</param>
        /// <returns>是否成功修改。</returns>
        public bool SetBody(byte[] data)
        {
            return Bridge.SetResponseData(MessageId, data);
        }

        /// <summary>
        /// 修改响应数据为字符串。
        /// </summary>
        /// <param name="data">新的响应数据字符串。</param>
        /// <param name="Encoding">编码，默认为 "UTF-8"。</param>
        /// <returns>是否成功修改。</returns>
        public bool SetBody(string data, string Encoding = "UTF-8")
        {
            byte[] A = Tool.StrToBytes(data, Encoding);
            return SetBody(A);
        }

        /// <summary>
        /// 获取响应数据字节数组，自动解压缩。
        /// </summary>
        /// <returns>解压缩后的响应数据字节数组。</returns>
        public EventValue BodyAuto()
        {
            byte[] bs = Bridge.GetResponseBody(MessageId);
            string Encoding = GetHeader("Content-Encoding").ToLower();
            switch (Encoding)
            {
                case "gzip":
                    {
                        byte[] raw2 = Compress.GzipUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            DelHeader("Content-Encoding");
                            SetBody(raw2);
                            return new EventValue(raw2);
                        }
                        break;
                    }
                case "br":
                    {
                        byte[] raw2 = Compress.BrUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            DelHeader("Content-Encoding");
                            SetBody(raw2);
                            return new EventValue(raw2);
                        }
                        break;
                    }
                case "deflate":
                    {
                        byte[] raw2 = Compress.DeflateUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            DelHeader("Content-Encoding");
                            SetBody(raw2);
                            return new EventValue(raw2);
                        }
                        break;
                    }
                case "zstd":
                    {
                        byte[] raw2 = Compress.ZSTDUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            DelHeader("Content-Encoding");
                            SetBody(raw2);
                            return new EventValue(raw2);
                        }
                        break;
                    }
                case "zlib":
                    {
                        byte[] raw2 = Compress.ZlibUnCompress(bs);
                        if (raw2 != null && raw2.Length > 0)
                        {
                            DelHeader("Content-Encoding");
                            SetBody(raw2);
                            return new EventValue(raw2);
                        }
                        break;
                    }
            }
            return new EventValue(bs);
        }

        /// <summary>
        /// 获取响应协议头。如果有多个同名协议头，将返回第一个。
        /// </summary>
        /// <param name="key">协议头的键名。</param>
        /// <returns>对应的协议头值。</returns>
        public string GetHeader(string key)
        {
            string[] arr = GetHeaderArray(key);
            if (arr.Length < 1)
            {
                return "";
            }
            return arr[0];
        }

        /// <summary>
        /// 获取响应协议头。如果有多个同名协议头，将返回数组。
        /// </summary>
        /// <param name="key">协议头的键名。</param>
        /// <returns>对应的协议头值数组。</returns>
        public string[] GetHeaderArray(string key)
        {
            return Bridge.GetResponseHeader(MessageId, key).Replace("\r", "").Split('\n');
        }

        /// <summary>
        /// 删除指定协议头。
        /// </summary>
        /// <param name="key">协议头的键名。</param>
        public void DelHeader(string key)
        {
            Bridge.DelResponseHeader(MessageId, key);
        }

        /// <summary>
        /// 设置指定协议头，支持多个同名协议头。
        /// </summary>
        /// <param name="key">协议头的键名。</param>
        /// <param name="value">协议头的值数组。</param>
        public void SetHeader(string key, string[] value)
        {
            SetHeader(key, string.Join("\n", value));
        }

        /// <summary>
        /// 设置指定协议头，支持多个同名协议头，用换行分割。
        /// </summary>
        /// <param name="key">协议头的键名。</param>
        /// <param name="value">协议头的值。</param>
        public void SetHeader(string key, string value)
        {
            Bridge.SetResponseHeader(MessageId, key, value);
        }

        /// <summary>
        /// 重置完整的协议头。
        /// </summary>
        /// <param name="Heads">协议头字符串。</param>
        public void SetAllHeader(string Heads)
        {
            Bridge.SetResponseAllHeader(MessageId, Heads);
        }

        /// <summary>
        /// 获取全部协议头。
        /// </summary>
        /// <returns>完整的协议头字符串。</returns>
        public string GetAllHeader()
        {
            return Bridge.GetResponseAllHeader(MessageId);
        }

        /// <summary>
        /// 删除全部协议头。
        /// </summary>
        public void DelAllHeader()
        {
            string[] arr = GetAllHeader().Replace("\r", "").Split('\n');
            foreach (string s in arr)
            {
                string[] arr1 = Regex.Split(s, ":", RegexOptions.IgnoreCase);
                if (arr1.Length >= 1)
                {
                    DelHeader(arr1[0]);
                }
            }
        }

        /// <summary>
        /// 获取响应的协议版本，例如 HTTP/1.1。
        /// </summary>
        /// <returns>协议版本字符串。</returns>
        public string GetProto()
        {
            return Bridge.GetResponseProto(MessageId);
        }
    }

}
