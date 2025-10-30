using System.Windows.Input;
using System;
using System.Collections.Generic;

namespace SunnyNetlibray.Internal
{
    internal class Bridge
    {
        ////释放指针
        private static bool Free(IntPtr Ptr)
        {
            return api.Sunny_Free(Ptr);
        }

        ////创建Sunny中间件对象,可创建多个
        public static long CreateSunnyNet()
        {
            return api.Sunny_CreateSunnyNet().ToInt64();
        }

        //// ReleaseSunnyNet 释放SunnyNet
        public static bool ReleaseSunnyNet(long SunnyContext)
        {
            return api.Sunny_ReleaseSunnyNet(new IntPtr(SunnyContext));
        }

        ////启动Sunny中间件 成功返回true
        public static bool SunnyNetStart(long SunnyContext)
        {
            return api.Sunny_SunnyNetStart(new IntPtr(SunnyContext));
        }

        ////设置指定端口 Sunny中间件启动之前调用
        public static bool SunnyNetSetPort(long SunnyContext, int Port)
        {
            return api.Sunny_SunnyNetSetPort(new IntPtr(SunnyContext), new IntPtr(Port));
        }


        ////关闭停止指定Sunny中间件
        public static bool SunnyNetClose(long SunnyContext)
        {
            return api.Sunny_SunnyNetClose(new IntPtr(SunnyContext));
        }

        ////设置请求超时
        public static void SetRequestOutTime(long SunnyContext, long times)
        {
            api.Sunny_SetRequestOutTime(new IntPtr(SunnyContext), new IntPtr(times));
        }
        ////设置自定义证书
        public static bool SunnyNetSetCert(long SunnyContext, CertManager CertificateManagerId)
        {
            IntPtr id = IntPtr.Zero;
            if (CertificateManagerId != null)
            {
                id = new IntPtr(CertificateManagerId.Context());
            }
            return api.Sunny_SunnyNetSetCert(new IntPtr(SunnyContext), id);
        }

        ////安装证书 将证书安装到Windows系统内
        public static string SunnyNetInstallCert(long SunnyContext)
        {
            IntPtr p = api.Sunny_SunnyNetInstallCert(new IntPtr(SunnyContext));
            string err = Tool.PtrToString(p);
            Free(p);
            return err;
        }

        ////设置中间件回调地址 httpCallback
        public static bool SunnyNetSetCallback(long SunnyContext, IntPtr httpCallback, IntPtr tcpCallback, IntPtr wsCallback, IntPtr udpCallback, IntPtr ScriptlogCallback, IntPtr ScriptsaveCallback)
        {
            api.Sunny_SetScriptCall(new IntPtr(SunnyContext), ScriptlogCallback, ScriptsaveCallback);
            return api.Sunny_SunnyNetSetCallback(new IntPtr(SunnyContext), httpCallback, tcpCallback, wsCallback, udpCallback);
        }

        ////添加 S5代理需要验证的用户名
        public static bool SunnyNetSocket5AddUser(long SunnyContext, string User, string Pass)
        {
            IntPtr _user = Tool.StringToIntptr(User);
            IntPtr _pass = Tool.StringToIntptr(Pass);
            bool ret = api.Sunny_SunnyNetSocket5AddUser(new IntPtr(SunnyContext), _user, _pass);
            Tool.PtrFree(_user);
            Tool.PtrFree(_pass);
            return ret;
        }

        ////开启身份验证模式
        public static bool SunnyNetVerifyUser(long SunnyContext, bool open)
        {
            return api.Sunny_SunnyNetVerifyUser(new IntPtr(SunnyContext), open);
        }

        ////删除 S5需要验证的用户名
        public static bool SunnyNetSocket5DelUser(long SunnyContext, string User)
        {
            IntPtr _user = Tool.StringToIntptr(User);
            bool ret = api.Sunny_SunnyNetSocket5DelUser(new IntPtr(SunnyContext), _user);
            Tool.PtrFree(_user);
            return ret;
        }

        ////删除 S5需要验证的用户名
        public static string SunnyNetGetSocket5User(long TheologyID)
        {
            IntPtr ret = api.Sunny_SunnyNetGetSocket5User(new IntPtr(TheologyID));
            string a = Tool.PtrToString(ret);
            Free(ret);
            return a;
        }
        ////设置中间件是否开启强制走TCP
        public static void SunnyNetMustTcp(long SunnyContext, bool open)
        {
            api.Sunny_SunnyNetMustTcp(new IntPtr(SunnyContext), open);
        }
        ////设置中间件是否开启强制走TCP
        public static bool SetRandomTLS(long SunnyContext, bool open)
        {
            return api.Sunny_SetRandomTLS(new IntPtr(SunnyContext), open);
        }
        ////禁用或启用UDP功能
        public static bool DisableUDP(long SunnyContext, bool Disable)
        {
            return api.Sunny_DisableUDP(new IntPtr(SunnyContext), Disable);
        }
        ////禁用或启用TCP功能
        public static bool DisableTCP(long SunnyContext, bool Disable)
        {
            return api.Sunny_DisableTCP(new IntPtr(SunnyContext), Disable);
        }
        ////设置脚本编辑器页面路径 需不少于8个字符
        public static string SetScriptPage(long SunnyContext, string PagePath)
        {
            IntPtr _PagePath = Tool.StringToIntptr(PagePath);
            IntPtr ret = api.Sunny_SetScriptPage(new IntPtr(SunnyContext), _PagePath);
            string a = Tool.PtrToString(ret);
            Free(ret);
            Tool.PtrFree(_PagePath);
            if (a == "no")
            {
                return "当前SDK是Mini版本不支持脚本编辑";
            }
            return a;
        }
        ////设置脚本编辑器代码
        public static string SetScriptCode(long SunnyContext, string Code, string Encoding = "UTF-8")
        {
            IntPtr _Code = Tool.StringToIntptr(Code, Encoding);
            IntPtr ret = api.Sunny_SetScriptCode(new IntPtr(SunnyContext), _Code);
            string a = Tool.PtrToString(ret);
            Free(ret);
            Tool.PtrFree(_Code);
            if (a == "no")
            {
                return "当前SDK是Mini版本不支持脚本编辑";
            }
            return a;
        }
        ////设置HTTP请求,提交数据,最大的长度,超过此长度,将无法修改数据，只能将提交内容储存到文件
        public static bool SetHTTPRequestMaxUpdateLength(long SunnyContext, long MaxLength)
        {
            return api.Sunny_SetHTTPRequestMaxUpdateLength(new IntPtr(SunnyContext), MaxLength);
        }





        ////设置中间件上游代理使用规则
        public static bool CompileProxyRegexp(long SunnyContext, string Regexp)
        {
            IntPtr _Regexp = Tool.StringToIntptr(Regexp);
            bool ret = api.Sunny_CompileProxyRegexp(new IntPtr(SunnyContext), _Regexp);
            Tool.PtrFree(_Regexp);
            return ret;
        }

        ////设置DNS服务器地址 仅支持tls的dns 服务器，也就是853端口的，例如:223.5.5.5:853
        public static void SetDnsServer(string DnsServerName)
        {
            IntPtr _Regexp = Tool.StringToIntptr(DnsServerName);
            api.Sunny_SetDnsServer(_Regexp);
            Tool.PtrFree(_Regexp);
        }

        ////设置强制走TCP规则,如果 打开了全部强制走TCP状态,本功能则无效 RulesAllow=false 规则之外走TCP  RulesAllow=true 规则之内走TCP
        public static bool SetMustTcpRegexp(long SunnyContext, string Regexp, bool RulesAllow)
        {
            IntPtr _Regexp = Tool.StringToIntptr(Regexp);
            bool ret = api.Sunny_SetMustTcpRegexp(new IntPtr(SunnyContext), _Regexp, RulesAllow);
            Tool.PtrFree(_Regexp);
            return ret;
        }

        ////获取中间件启动时的错误信息
        public static string SunnyNetError(long SunnyContext)
        {
            IntPtr p = api.Sunny_SunnyNetError(new IntPtr(SunnyContext));
            string a = Tool.PtrToString(p);
            Free(p);
            return a;

        }
        /// <summary>
        /// 设置全局上游代理。
        /// </summary>
        /// <param name="ProxyAddress">代理 URL，指定要使用的代理地址。
        /// 例如，以下示例格式：
        ///  <list type="bullet">
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </param> 
        /// <param name="outTime">代理超时(毫秒)，默认30秒
        /// <returns>如果成功设置代理，返回 true；否则返回 false。</returns>
        public static bool SetGlobalProxy(long SunnyContext, string ProxyAddress, int outTime = 30000)
        {
            IntPtr _ProxyAddress = Tool.StringToIntptr(ProxyAddress);
            bool ret = api.Sunny_SetGlobalProxy(new IntPtr(SunnyContext), _ProxyAddress, new IntPtr(outTime));
            Tool.PtrFree(_ProxyAddress);
            return ret;
        }
        // 导出已设置的证书
        public static string ExportCert(long SunnyContext)
        {
            IntPtr p = api.Sunny_ExportCert(new IntPtr(SunnyContext));
            string a = Tool.PtrToString(p);
            Free(p);
            return a;
        }

        // 设置IE代理
        public static bool SetIeProxy(long SunnyContext)
        {
            return api.Sunny_SetIeProxy(new IntPtr(SunnyContext));
        }
        // 取消设置的IE代理
        public static bool CancelIEProxy(long SunnyContext)
        {
            return api.Sunny_CancelIEProxy(new IntPtr(SunnyContext));
        }
        //中间件设置出口IP函数-全局
        public static bool SetOutRouterIP(long SunnyContext, String IP)
        {
            IntPtr a = Tool.StringToIntptr(IP);
            bool r = api.Sunny_SetOutRouterIP(new IntPtr(SunnyContext), a);
            Tool.PtrFree(a);
            return r;
        }

        //中间件TCP/HTTP请求设置出口IP函数
        public static bool RequestSetOutRouterIP(long MessageId, String IP)
        {
            IntPtr a = Tool.StringToIntptr(IP);
            bool r = api.Sunny_RequestSetOutRouterIP(new IntPtr(MessageId), a);
            Tool.PtrFree(a);
            return r;
        }

        //HTTP客户端设置出口IP函数	
        public static bool HTTPSetOutRouterIP(long Context, String IP)
        {
            IntPtr a = Tool.StringToIntptr(IP);
            bool r = api.Sunny_HTTPSetOutRouterIP(new IntPtr(Context), a);
            Tool.PtrFree(a);
            return r;
        }
        ////修改、设置 HTTP/S当前请求数据中指定Cookie
        public static void SetRequestCookie(long MessageId, string key, string value)
        {
            IntPtr a = Tool.StringToIntptr(key);
            IntPtr b = Tool.StringToIntptr(value);
            api.Sunny_SetRequestCookie(new IntPtr(MessageId), a, b);
            Tool.PtrFree(a);
            Tool.PtrFree(b);
        }

        ////修改、设置 HTTP/S当前请求数据中的全部Cookie
        public static void SetRequestAllCookie(long MessageId, string cookie)
        {
            IntPtr a = Tool.StringToIntptr(cookie);
            api.Sunny_SetRequestAllCookie(new IntPtr(MessageId), a);
            Tool.PtrFree(a);
        }

        ////获取 HTTP/S当前请求数据中指定的Cookie
        public static string GetRequestCookie(long MessageId, string name)
        {
            IntPtr a = Tool.StringToIntptr(name);
            IntPtr i1 = api.Sunny_GetRequestCookie(new IntPtr(MessageId), a);
            Tool.PtrFree(a);
            string sa = Tool.PtrToString(i1);
            Free(i1);
            return sa;
        }

        ////获取 HTTP/S 当前请求全部Cookie
        public static string GetRequestALLCookie(long MessageId)
        {
            IntPtr i1 = api.Sunny_GetRequestALLCookie(new IntPtr(MessageId));
            string sa = Tool.PtrToString(i1);
            Free(i1);
            return sa;
        }

        ////删除HTTP/S返回数据中指定的协议头
        public static void DelResponseHeader(long MessageId, string name)
        {
            IntPtr a = Tool.StringToIntptr(name);
            api.Sunny_DelResponseHeader(new IntPtr(MessageId), a);
            Tool.PtrFree(a);
        }

        ////删除HTTP/S请求数据中指定的协议头
        public static void DelRequestHeader(long MessageId, string name)
        {
            IntPtr a = Tool.StringToIntptr(name);
            api.Sunny_DelRequestHeader(new IntPtr(MessageId), a);
            Tool.PtrFree(a);
        }
        ////将原始请求数据保存到文件。
        public static void RawRequestDataToFile(long MessageId, string SavePath)
        {
            byte[] a = Tool.StrToBytes(SavePath);
            IntPtr aa = Tool.BytesToIntptr(a);
            api.Sunny_RawRequestDataToFile(new IntPtr(MessageId), aa, new IntPtr(a.Length));
            Tool.PtrFree(aa);

        }
        ////检查当前请求是否为原始 body。
        public static bool IsRequestRawBody(long MessageId)
        {
            return api.Sunny_IsRequestRawBody(new IntPtr(MessageId));
        }

        ////设置HTTP/S请求体中的协议头
        public static void SetRequestHeader(long MessageId, string name, string val)
        {
            IntPtr i1 = Tool.StringToIntptr(name);
            IntPtr i2 = Tool.StringToIntptr(val);
            api.Sunny_SetRequestHeader(new IntPtr(MessageId), i1, i2);
            Tool.PtrFree(i1);
            Tool.PtrFree(i2);
        }

        ////修改、设置 HTTP/S当前返回数据中的指定协议头
        public static void SetResponseHeader(long MessageId, string name, string val)
        {
            IntPtr i1 = Tool.StringToIntptr(name);
            IntPtr i2 = Tool.StringToIntptr(val);
            api.Sunny_SetResponseHeader(new IntPtr(MessageId), i1, i2);
            Tool.PtrFree(i1);
            Tool.PtrFree(i2);
        }

        ////获取 HTTP/S当前请求数据中的指定协议头
        public static string GetRequestHeader(long MessageId, string name)
        {
            IntPtr i1 = Tool.StringToIntptr(name);
            IntPtr aa = api.Sunny_GetRequestHeader(new IntPtr(MessageId), i1);
            Tool.PtrFree(i1);
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////获取 HTTP/S 当前返回数据中指定的协议头
        public static string GetResponseHeader(long MessageId, string name)
        {
            IntPtr i1 = Tool.StringToIntptr(name);
            IntPtr aa = api.Sunny_GetResponseHeader(new IntPtr(MessageId), i1);
            Tool.PtrFree(i1);
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }
        ////获取响应的协议版本，例如 HTTP/1.1。
        public static string GetResponseProto(long MessageId)
        {
            IntPtr aa = api.Sunny_GetResponseProto(new IntPtr(MessageId));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////修改、设置 HTTP/S当前返回数据中的全部协议头，例如设置返回两条Cookie 使用本命令设置 使用设置、修改 单条命令无效
        public static void SetResponseAllHeader(long MessageId, string value)
        {
            IntPtr i1 = Tool.StringToIntptr(value);
            api.Sunny_SetResponseAllHeader(new IntPtr(MessageId), i1);
            Tool.PtrFree(i1);
        }

        ////获取 HTTP/S 当前返回全部协议头
        public static string GetResponseAllHeader(long MessageId)
        {
            IntPtr aa = api.Sunny_GetResponseAllHeader(new IntPtr(MessageId));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////获取 HTTP/S 当前请求数据全部协议头
        public static string GetRequestAllHeader(long MessageId)
        {
            IntPtr aa = api.Sunny_GetRequestAllHeader(new IntPtr(MessageId));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }
        ////获取请求的协议版本，例如 HTTP/1.1。
        public static string GetRequestProto(long MessageId)
        {
            IntPtr aa = api.Sunny_GetRequestProto(new IntPtr(MessageId));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        /// <summary>
        /// 设置代理。
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
        /// <param name="outTime">代理超时，单位毫秒。</param>
        /// <returns>如果成功设置代理，返回 true；否则返回 false。</returns>
        public static bool SetRequestProxy(long MessageId, string ProxyUrl, int outTime)
        {
            IntPtr i1 = Tool.StringToIntptr(ProxyUrl);
            bool res = api.Sunny_SetRequestProxy(new IntPtr(MessageId), i1, new IntPtr(outTime));
            Tool.PtrFree(i1);
            return res;
        }


        ////重置完整的协议头
        public static void SetRequestALLHeader(long MessageId, string Headers)
        {
            IntPtr i1 = Tool.StringToIntptr(Headers);
            api.Sunny_SetRequestALLHeader(new IntPtr(MessageId), i1);
            Tool.PtrFree(i1);
        }

        ////随机化请求的  JA3 指纹。
        public static bool RandomRequestCipherSuites(long MessageId)
        {
            return api.Sunny_RandomRequestCipherSuites(new IntPtr(MessageId));
        }
        ////设置HTTP 2.0 请求指纹配置 (若服务器支持则使用,若服务器不支持,设置了也不会使用)
        public static bool SetRequestHTTP2Config(long MessageId, string config)
        {
            IntPtr i1 = Tool.StringToIntptr(config);
            bool res = api.Sunny_SetRequestHTTP2Config(new IntPtr(MessageId), i1);
            Tool.PtrFree(i1);
            return res;
        }

        ////获取HTTP/S返回的状态码
        public static int GetResponseStatusCode(long MessageId)
        {
            return (int)api.Sunny_GetResponseStatusCode(new IntPtr(MessageId));
        }

        ////获取当前HTTP/S请求由哪个IP发起
        public static string GetRequestClientIp(long MessageId)
        {
            IntPtr aa = api.Sunny_GetRequestClientIp(new IntPtr(MessageId));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////获取HTTP/S返回的状态文本 例如 [200 OK]
        public static string GetResponseStatus(long MessageId)
        {
            IntPtr aa = api.Sunny_GetResponseStatus(new IntPtr(MessageId));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////获取取服务器响应IP地址
        public static string GetResponseServerAddress(long MessageId)
        {
            IntPtr aa = api.Sunny_GetResponseServerAddress(new IntPtr(MessageId));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////修改HTTP/S返回的状态码
        public static void SetResponseStatus(long MessageId, int code)
        {
            api.Sunny_SetResponseStatus(new IntPtr(MessageId), new IntPtr(code));
        }

        ////修改HTTP/S当前请求的URL
        public static bool SetRequestUrl(long MessageId, string URI)
        {
            IntPtr i1 = Tool.StringToIntptr(URI);
            bool res = api.Sunny_SetRequestUrl(new IntPtr(MessageId), i1);
            Tool.PtrFree(i1);
            return res;
        }

        ////获取 HTTP/S 当前请求POST提交数据长度
        public static int GetRequestBodyLen(long MessageId)
        {
            return (int)api.Sunny_GetRequestBodyLen(new IntPtr(MessageId)).ToInt64();
        }

        ////获取 HTTP/S 当前返回  数据长度
        public static int GetResponseBodyLen(long MessageId)
        {
            return (int)api.Sunny_GetResponseBodyLen(new IntPtr(MessageId)).ToInt64();
        }

        ////设置、修改 HTTP/S 当前请求返回数据 如果再发起请求时调用本命令，请求将不会被发送，将会直接返回 data=数据指针  dataLen=数据长度
        public static bool SetResponseData(long MessageId, byte[] data)
        {
            IntPtr i1 = Tool.BytesToIntptr(data);
            bool res = api.Sunny_SetResponseData(new IntPtr(MessageId), i1, new IntPtr(data.Length));
            Tool.PtrFree(i1);
            return res;
        }

        ////设置、修改 HTTP/S 当前请求POST提交数据  data=数据指针  dataLen=数据长度
        public static bool SetRequestData(long MessageId, byte[] data)
        {
            IntPtr i1 = Tool.BytesToIntptr(data);
            bool res = api.Sunny_SetRequestData(new IntPtr(MessageId), i1, new IntPtr(data.Length));
            Tool.PtrFree(i1);
            return res;
        }

        ////获取 HTTP/S 当前POST提交数据 返回 数据指针
        public static byte[] GetRequestBody(long MessageId)
        {
            IntPtr p = api.Sunny_GetRequestBody(new IntPtr(MessageId));
            byte[] aa = Tool.PtrToBytes(p, GetRequestBodyLen(MessageId));
            Free(p);
            return aa;

        }

        ////获取 HTTP/S 当前返回数据  返回 数据指针
        public static byte[] GetResponseBody(long MessageId)
        {
            IntPtr p = api.Sunny_GetResponseBody(new IntPtr(MessageId));
            byte[] aa = Tool.PtrToBytes(p, GetResponseBodyLen(MessageId));
            Free(p);
            return aa;
        }

        ////获取 WebSocket消息长度
        public static int GetWebsocketBodyLen(long MessageId)
        {
            return (int)api.Sunny_GetWebsocketBodyLen(new IntPtr(MessageId)).ToInt64();
        }

        ////获取 WebSocket消息 数据
        public static byte[] GetWebsocketBody(long MessageId)
        {
            IntPtr p = api.Sunny_GetWebsocketBody(new IntPtr(MessageId));
            byte[] bs = Tool.PtrToBytes(p, GetWebsocketBodyLen(MessageId));
            Free(p);
            return bs;
        }

        ////修改 WebSocket消息 data=数据指针  dataLen=数据长度
        public static bool SetWebsocketBody(long MessageId, byte[] data)
        {
            IntPtr i1 = Tool.BytesToIntptr(data);
            bool res = api.Sunny_SetWebsocketBody(new IntPtr(MessageId), i1, new IntPtr(data.Length));
            Tool.PtrFree(i1);
            return res;
        }

        ////主动向Websocket服务器发送消息 MessageType=WS消息类型 data=数据 
        public static bool SendWebsocketBody(long TheologyID, long MessageType, byte[] data)
        {
            IntPtr i1 = Tool.BytesToIntptr(data);
            bool res = api.Sunny_SendWebsocketBody(new IntPtr(TheologyID), new IntPtr(MessageType), i1, new IntPtr(data.Length));
            Tool.PtrFree(i1);
            return res;
        }
        ////主动向Websocket客户端发送消息 TheologyID=TheologyID MessageType=WS消息类型 data=数据
        public static bool SendWebsocketClientBody(long TheologyID, long MessageType, byte[] data)
        {
            IntPtr i1 = Tool.BytesToIntptr(data);
            bool res = api.Sunny_SendWebsocketClientBody(new IntPtr(TheologyID), new IntPtr(MessageType), i1, new IntPtr(data.Length));
            Tool.PtrFree(i1);
            return res;
        }
        ////根据TheologyID关闭指定的Websocket连接  TheologyID在回调参数中
        public static bool CloseWebsocket(long TheologyID)
        {
            return api.Sunny_CloseWebsocket(new IntPtr(TheologyID));
        }
        ////修改 TCP消息数据 MsgType=1 发送的消息 MsgType=2 接收的消息 如果 MsgType和MessageId不匹配，将不会执行操作  data=数据指针  dataLen=数据长度
        public static bool SetTcpBody(long MessageId, int MsgType, byte[] data)
        {
            IntPtr i1 = Tool.BytesToIntptr(data);
            bool res = api.Sunny_SetTcpBody(new IntPtr(MessageId), new IntPtr(MsgType), i1, new IntPtr(data.Length));
            Tool.PtrFree(i1);
            return res;
        }
        /// <summary>
        /// 对TCP设置代理。
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
        /// <param name="outTime">代理超时(毫秒)，默认30秒</param>
        /// <returns>如果成功设置代理，返回 true；否则返回 false。</returns>
        public static bool SetTcpAgent(long MessageId, string ProxyUrl, int outTime = 30000)
        {
            IntPtr i1 = Tool.StringToIntptr(ProxyUrl);
            bool res = api.Sunny_SetTcpAgent(new IntPtr(MessageId), i1, new IntPtr(outTime));
            Tool.PtrFree(i1);
            return res;
        }

        ////根据TheologyID关闭指定的TCP连接  TheologyID在回调参数中
        public static bool TcpCloseClient(long theology)
        {
            return api.Sunny_TcpCloseClient(new IntPtr(theology));
        }

        ////给指定的TCP连接 修改目标连接地址 目标地址必须带端口号 例如 baidu.com:443
        public static bool SetTcpConnectionIP(long MessageId, string address)
        {
            IntPtr i1 = Tool.StringToIntptr(address);
            bool res = api.Sunny_SetTcpConnectionIP(new IntPtr(MessageId), i1);
            Tool.PtrFree(i1);
            return res;
        }

        ////指定的TCP连接 模拟客户端向服务器端主动发送数据
        public static bool TcpSendMsg(long theology, byte[] data)
        {
            IntPtr i1 = Tool.BytesToIntptr(data);
            int res = (int)api.Sunny_TcpSendMsg(new IntPtr(theology), i1, new IntPtr(data.Length));
            Tool.PtrFree(i1);
            return res != 0;
        }

        ////指定的TCP连接 模拟服务器端向客户端主动发送数据
        public static bool TcpSendMsgClient(long theology, byte[] data)
        {
            IntPtr i1 = Tool.BytesToIntptr(data);
            int res = (int)api.Sunny_TcpSendMsgClient(new IntPtr(theology), i1, new IntPtr(data.Length));
            Tool.PtrFree(i1);
            return res != 0;
        }

        ////将Go int的Bytes 转为int
        private static int BytesToInt(IntPtr data, int dataLen)
        {
            return (int)api.Sunny_BytesToInt(data, new IntPtr(dataLen));
        }
        private static byte[] PtrAutoToBytes(IntPtr data)
        {
            byte[] datas = new byte[0];
            if (data.ToInt64() != 0)
            {
                long plen = Bridge.BytesToInt(data, 8);
                return Tool.PtrToBytes(data, (int)plen, 8);
            }
            return datas;
        }
        ////Gzip解压缩
        public static byte[] GzipUnCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_GzipUnCompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }


        ////ZSTD解压缩
        public static byte[] ZSTDUnCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_ZSTDDecompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }

        ////ZSTD压缩
        public static byte[] ZSTDCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_ZSTDCompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }
        ////br解压缩
        public static byte[] BrUnCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_BrUnCompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }

        ////br压缩
        public static byte[] BrCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_BrCompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }

        ////Gzip压缩
        public static byte[] GzipCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_GzipCompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }

        ////Zlib压缩
        public static byte[] ZlibCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_ZlibCompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }

        ////Zlib解压缩
        public static byte[] ZlibUnCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_ZlibUnCompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }

        ////Deflate解压缩 (可能等同于zlib解压缩)
        public static byte[] DeflateUnCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_DeflateUnCompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }

        ////Deflate压缩 (可能等同于zlib压缩)
        public static byte[] DeflateCompress(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_DeflateCompress(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }
        //===================================================================================================================================================

        ////Webp图片转JEG图片字节数组 SaveQuality=质量(默认75)
        public static byte[] WebpToJpegBytes(byte[] data, int SaveQuality)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_WebpToJpegBytes(_bin, new IntPtr(data.Length), new IntPtr(SaveQuality));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }

        ////Webp图片转Png图片字节数组
        public static byte[] WebpToPngBytes(byte[] data)
        {
            IntPtr _bin = Tool.BytesToIntptr(data);
            IntPtr p = api.Sunny_WebpToPngBytes(_bin, new IntPtr(data.Length));
            byte[] ret = PtrAutoToBytes(p);
            Tool.PtrFree(_bin);
            Free(p);
            return ret;
        }

        ////Webp图片转JEG图片 根据文件名 SaveQuality=质量(默认75)
        public static bool WebpToJpeg(string webpPath, string savePath, int SaveQuality)
        {
            IntPtr _webpPath = Tool.StringToIntptr(webpPath);
            IntPtr _savePath = Tool.StringToIntptr(savePath);
            bool res = api.Sunny_WebpToJpeg(_webpPath, _savePath, new IntPtr(SaveQuality));
            Tool.PtrFree(_webpPath);
            Tool.PtrFree(_savePath);
            return res;
        }

        ////Webp图片转Png图片 根据文件名
        public static bool WebpToPng(string webpPath, string savePath)
        {
            IntPtr _webpPath = Tool.StringToIntptr(webpPath);
            IntPtr _savePath = Tool.StringToIntptr(savePath);
            bool res = api.Sunny_WebpToPng(_webpPath, _savePath);
            Tool.PtrFree(_webpPath);
            Tool.PtrFree(_savePath);
            return res;
        }


        // OpenDrive 开始进程代理/打开驱动 只允许一个 SunnyNet 使用 [会自动安装所需驱动文件]
        // IsNfapi 如果为true表示使用NFAPI驱动 如果为false 表示使用Proxifier
        public static bool OpenDrive(long SunnyContext, long devMode)
        {
            return api.Sunny_OpenDrive(new IntPtr(SunnyContext), new IntPtr(devMode));
        }

        // UnDrive 卸载驱动，仅Windows 有效【需要管理权限】执行成功后会立即重启系统,若函数执行后没有重启系统表示没有管理员权限
        public static void UnDrive(long SunnyContext)
        {
            api.Sunny_UnDrive(new IntPtr(SunnyContext));
        }

        ////进程代理 添加进程名
        public static void ProcessAddName(long SunnyContext, string Name)
        {
            IntPtr _Name = Tool.StringToIntptr(Name);
            api.Sunny_ProcessAddName(new IntPtr(SunnyContext), _Name);
            Tool.PtrFree(_Name);
        }

        ////进程代理 删除进程名
        public static void ProcessDelName(long SunnyContext, string Name)
        {
            IntPtr _Name = Tool.StringToIntptr(Name);
            api.Sunny_ProcessDelName(new IntPtr(SunnyContext), _Name);
            Tool.PtrFree(_Name);
        }

        ////进程代理 添加PID
        public static void ProcessAddPid(long SunnyContext, int pid)
        {
            api.Sunny_ProcessAddPid(new IntPtr(SunnyContext), new IntPtr(pid));
        }

        ////进程代理 删除PID
        public static void ProcessDelPid(long SunnyContext, int pid)
        {
            api.Sunny_ProcessDelPid(new IntPtr(SunnyContext), new IntPtr(pid));
        }

        ////进程代理 取消全部已设置的进程名
        public static void ProcessCancelAll(long SunnyContext)
        {
            api.Sunny_ProcessCancelAll(new IntPtr(SunnyContext));
        }

        ////进程代理 设置是否全部进程通过
        public static void ProcessALLName(long SunnyContext, bool open, bool StopNet)
        {
            api.Sunny_ProcessALLName(new IntPtr(SunnyContext), open, StopNet);
        }

        ////证书管理器 获取证书 CommonName 字段
        public static string GetCommonName(long Context)
        {
            IntPtr aa = api.Sunny_GetCommonName(new IntPtr(Context));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////证书管理器 导出为P12
        public static bool ExportP12(long Context, string path, string pass)
        {
            IntPtr _path = Tool.StringToIntptr(path);
            IntPtr _pass = Tool.StringToIntptr(pass);
            bool res = api.Sunny_ExportP12(new IntPtr(Context), _path, _pass);
            Tool.PtrFree(_path);
            Tool.PtrFree(_pass);
            return res;
        }

        ////证书管理器 导出公钥
        public static string ExportPub(long Context)
        {
            IntPtr aa = api.Sunny_ExportPub(new IntPtr(Context));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////证书管理器 导出私钥
        public static string ExportKEY(long Context)
        {
            IntPtr aa = api.Sunny_ExportKEY(new IntPtr(Context));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////证书管理器 导出证书
        public static string ExportCA(long Context)
        {
            IntPtr aa = api.Sunny_ExportCA(new IntPtr(Context));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////证书管理器 创建证书
        public static bool CreateCA(
            long Context, string Country, string Organization,
            string OrganizationalUnit, string Province, string
            CommonName, string Locality, int NotAfter)
        {
            IntPtr _Country = Tool.StringToIntptr(Country);
            IntPtr _Organization = Tool.StringToIntptr(Organization);
            IntPtr _OrganizationalUnit = Tool.StringToIntptr(OrganizationalUnit);
            IntPtr _Province = Tool.StringToIntptr(Province);
            IntPtr _CommonName = Tool.StringToIntptr(CommonName);
            IntPtr _Locality = Tool.StringToIntptr(Locality);
            bool res = api.Sunny_CreateCA(new IntPtr(Context), _Country, _Organization, _OrganizationalUnit, _Province, _CommonName, _Locality, new IntPtr(2048), new IntPtr(NotAfter));
            Tool.PtrFree(_Country);
            Tool.PtrFree(_Organization);
            Tool.PtrFree(_OrganizationalUnit);
            Tool.PtrFree(_Province);
            Tool.PtrFree(_CommonName);
            Tool.PtrFree(_Locality);
            return res;
        }

        ////证书管理器 设置ClientAuth
        public static bool AddClientAuth(long Context, int val)
        {
            return api.Sunny_AddClientAuth(new IntPtr(Context), new IntPtr(val));
        }

        ////证书管理器 设置信任的证书 从 文本
        public static bool AddCertPoolText(long Context, string cer)
        {
            IntPtr cert = Tool.StringToIntptr(cer);
            bool res = api.Sunny_AddCertPoolText(new IntPtr(Context), cert);
            Tool.PtrFree(cert);
            return res;
        }
        ////证书管理器 设置CipherSuites
        public static bool SetCipherSuites(long Context, string cer)
        {
            IntPtr cert = Tool.StringToIntptr(cer);
            bool res = api.Sunny_SetCipherSuites(new IntPtr(Context), cert);
            Tool.PtrFree(cert);
            return res;
        }

        ////证书管理器 设置信任的证书 从 文件
        public static bool AddCertPoolPath(long Context, string cer)
        {
            IntPtr cert = Tool.StringToIntptr(cer);
            bool res = api.Sunny_AddCertPoolPath(new IntPtr(Context), cert);
            Tool.PtrFree(cert);
            return res;
        }

        ////证书管理器 取ServerName
        public static string GetServerName(long Context)
        {
            IntPtr aa = api.Sunny_GetServerName(new IntPtr(Context));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////证书管理器 设置ServerName
        public static bool SetServerName(long Context, string name)
        {
            IntPtr cert = Tool.StringToIntptr(name);
            bool res = api.Sunny_SetServerName(new IntPtr(Context), cert);
            Tool.PtrFree(cert);
            return res;
        }

        ////证书管理器 设置跳过主机验证
        public static bool SetInsecureSkipVerify(long Context, bool b)
        {
            return api.Sunny_SetInsecureSkipVerify(new IntPtr(Context), b);
        }

        ////证书管理器 载入X509证书
        public static bool LoadX509Certificate(long Context, string Host, string CA, string KEY)
        {
            IntPtr _Host = Tool.StringToIntptr(Host);
            IntPtr _CA = Tool.StringToIntptr(CA);
            IntPtr _KEY = Tool.StringToIntptr(KEY);
            bool res = api.Sunny_LoadX509Certificate(new IntPtr(Context), _Host, _CA, _KEY);
            Tool.PtrFree(_Host);
            Tool.PtrFree(_CA);
            Tool.PtrFree(_KEY);
            return res;
        }

        ////证书管理器 载入X509证书2
        public static bool LoadX509KeyPair(long Context, string CaPath, string KeyPath)
        {
            IntPtr _CaPath = Tool.StringToIntptr(CaPath);
            IntPtr _KeyPath = Tool.StringToIntptr(KeyPath);
            bool res = api.Sunny_LoadX509KeyPair(new IntPtr(Context), _CaPath, _KeyPath);
            Tool.PtrFree(_CaPath);
            Tool.PtrFree(_KeyPath);
            return res;
        }

        ////证书管理器 载入p12证书
        public static bool LoadP12Certificate(long Context, string Name, string Password)
        {
            IntPtr _Name = Tool.StringToIntptr(Name);
            IntPtr _Password = Tool.StringToIntptr(Password);
            bool res = api.Sunny_LoadP12Certificate(new IntPtr(Context), _Name, _Password);
            Tool.PtrFree(_Name);
            Tool.PtrFree(_Password);
            return res;
        }

        ////释放 证书管理器 对象
        public static void RemoveCertificate(long Context)
        {
            api.Sunny_RemoveCertificate(new IntPtr(Context));
        }

        ////创建 证书管理器 对象
        public static long CreateCertificate()
        {
            return api.Sunny_CreateCertificate().ToInt64();
        }

        ////GoMap 写字符串
        public static void KeysWriteStr(long KeysHandle, string name, string val)
        {
            byte[] bs = Tool.StrToBytes(val);
            IntPtr _val = Tool.BytesToIntptr(bs);
            IntPtr _name = Tool.StringToIntptr(name);
            api.Sunny_KeysWriteStr(new IntPtr(KeysHandle), _name, _val, new IntPtr(bs.Length));
            Tool.PtrFree(_val);
            Tool.PtrFree(_name);
        }

        ////GoMap 转为JSON字符串
        public static string KeysGetJson(long KeysHandle)
        {
            IntPtr aa = api.Sunny_KeysGetJson(new IntPtr(KeysHandle));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////GoMap 取数量
        public static int KeysGetCount(long KeysHandle)
        {
            return (int)api.Sunny_KeysGetCount(new IntPtr(KeysHandle));
        }

        ////GoMap 清空
        public static void KeysEmpty(long KeysHandle)
        {
            api.Sunny_KeysEmpty(new IntPtr(KeysHandle));
        }

        ////GoMap 读整数
        public static int KeysReadInt(long KeysHandle, string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            int res = (int)api.Sunny_KeysReadInt(new IntPtr(KeysHandle), _name).ToInt64();
            Tool.PtrFree(_name);
            return res;
        }

        ////GoMap 写整数
        public static void KeysWriteInt(long KeysHandle, string name, int val)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            api.Sunny_KeysWriteInt(new IntPtr(KeysHandle), _name, new IntPtr(val));
            Tool.PtrFree(_name);
        }

        ////GoMap 读长整数
        public static long KeysReadLong(long KeysHandle, string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            long res = api.Sunny_KeysReadLong(new IntPtr(KeysHandle), _name);
            Tool.PtrFree(_name);
            return res;
        }

        ////GoMap 写长整数
        public static void KeysWriteLong(long KeysHandle, string name, long val)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            api.Sunny_KeysWriteLong(new IntPtr(KeysHandle), _name, val);
            Tool.PtrFree(_name);
        }

        ////GoMap 读浮点数
        public static double KeysReadFloat(long KeysHandle, string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            double res = api.Sunny_KeysReadFloat(new IntPtr(KeysHandle), _name);
            Tool.PtrFree(_name);
            return res;
        }

        ////GoMap 写浮点数
        public static void KeysWriteFloat(long KeysHandle, string name, double val)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            api.Sunny_KeysWriteFloat(new IntPtr(KeysHandle), _name, val);
            Tool.PtrFree(_name);
        }

        ////GoMap 写字节数组
        public static void KeysWrite(long KeysHandle, string name, byte[] val)
        {
            IntPtr _val = Tool.BytesToIntptr(val);
            IntPtr _name = Tool.StringToIntptr(name);
            api.Sunny_KeysWrite(new IntPtr(KeysHandle), _name, _val, new IntPtr(val.Length));
            Tool.PtrFree(_val);
            Tool.PtrFree(_name);
        }

        ////GoMap 写读字符串/字节数组
        public static byte[] KeysRead(long KeysHandle, string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            IntPtr r = api.Sunny_KeysRead(new IntPtr(KeysHandle), _name);
            Tool.PtrFree(_name);
            byte[] aaa = PtrAutoToBytes(r);
            Free(r);
            return aaa;
        }

        ////GoMap 删除
        public static void KeysDelete(long KeysHandle, string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            api.Sunny_KeysDelete(new IntPtr(KeysHandle), _name);
            Tool.PtrFree(_name);
        }

        ////GoMap 删除GoMap
        public static void RemoveKeys(long KeysHandle)
        {
            api.Sunny_RemoveKeys(new IntPtr(KeysHandle));
        }

        ////GoMap 创建
        public static long CreateKeys()
        {
            return api.Sunny_CreateKeys().ToInt64();
        }

        ////HTTP 客户端 设置重定向
        public static bool HTTPSetRedirect(long Context, bool Redirect)
        {
            return api.Sunny_HTTPSetRedirect(new IntPtr(Context), Redirect);
        }

        ////HTTP 客户端 返回响应状态码
        public static int HTTPGetCode(long Context)
        {
            return (int)api.Sunny_HTTPGetCode(new IntPtr(Context));
        }

        ////HTTP 客户端 返回响应内容
        public static byte[] HTTPGetBody(long Context)
        {

            int l = HTTPGetBodyLen(Context);
            IntPtr P = api.Sunny_HTTPGetBody(new IntPtr(Context));
            byte[] a = Tool.PtrToBytes(P, l);
            Free(P);
            return a;
        }
        ////设置是否随机使用 TLS 指纹。
        public static bool HTTPSetRandomTLS(long Context, bool Random)
        {
            return api.Sunny_HTTPSetRandomTLS(new IntPtr(Context), Random);
        }

        ////设置HTTP2指纹
        public static bool HTTPSetH2Config(long Context, string config)
        {
            IntPtr _config = Tool.StringToIntptr(config);
            bool res = api.Sunny_HTTPSetH2Config(new IntPtr(Context), _config);
            Tool.PtrFree(_config);
            return res;
        }

        ////取添加的全部协议头
        public static string HTTPGetRequestHeader(long Context)
        {
            IntPtr _config = api.Sunny_HTTPGetRequestHeader(new IntPtr(Context));
            string ss = Tool.PtrToString(_config);
            Free(_config);
            return ss;
        }

        ////HTTP 客户端 返回响应全部Heads
        public static string HTTPGetHeads(long Context)
        {
            IntPtr aa = api.Sunny_HTTPGetHeads(new IntPtr(Context));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////HTTP 客户端 返回响应长度
        public static int HTTPGetBodyLen(long Context)
        {
            return (int)api.Sunny_HTTPGetBodyLen(new IntPtr(Context));
        }

        ////HTTP 客户端 发送Body
        public static void HTTPSendBin(long Context, byte[] body)
        {
            IntPtr _body = Tool.BytesToIntptr(body);
            api.Sunny_HTTPSendBin(new IntPtr(Context), _body, new IntPtr(body.Length));
            Tool.PtrFree(_body);
        }

        ////HTTP 客户端 设置超时 毫秒
        public static void HTTPSetTimeouts(long Context, int timeOut)
        {
            api.Sunny_HTTPSetTimeouts(new IntPtr(Context), new IntPtr(timeOut));
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
        public static void HTTPSetProxyIP(long Context, string ProxyURL)
        {
            IntPtr _ProxyURL = Tool.StringToIntptr(ProxyURL);
            api.Sunny_HTTPSetProxyIP(new IntPtr(Context), _ProxyURL);
            Tool.PtrFree(_ProxyURL);
        }

        /// HTTP 客户端 设置协议头
        public static void HTTPSetHeader(long Context, string name, string value)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            IntPtr _value = Tool.StringToIntptr(value);
            api.Sunny_HTTPSetHeader(new IntPtr(Context), _name, _value);
            Tool.PtrFree(_name);
            Tool.PtrFree(_value);
        }

        /// <summary>
        /// 设置请求实际连接地址
        /// 设置后将不再使用URL或协议头中的HOST地址
        /// 某些时候,协议头中的HOST以及URL中的地址不能修改,修改后请求无法发送，这种情况下有用
        /// </summary>
        /// <param name="ip">例如:8.8.8.8:443,只能IP+端口，如果格式错误，不会使用</param>
        public static void HTTPSetServerIP(long Context, string ip)
        {
            IntPtr _ip = Tool.StringToIntptr(ip);
            api.Sunny_HTTPSetServerIP(new IntPtr(Context), _ip);
            Tool.PtrFree(_ip);
        }

        ////HTTP 客户端 设置证书管理器
        public static bool HTTPSetCertManager(long Context, CertManager Cert)
        {
            IntPtr id = IntPtr.Zero;
            if (Cert != null)
            {
                id = new IntPtr(Cert.Context());
            }
            return api.Sunny_HTTPSetCertManager(new IntPtr(Context), id);
        }
        ////HTTP 客户端 Open
        public static void HTTPOpen(long Context, string Method, string URL)
        {
            IntPtr _Method = Tool.StringToIntptr(Method);
            IntPtr _URL = Tool.StringToIntptr(URL);
            api.Sunny_HTTPOpen(new IntPtr(Context), _Method, _URL);
        }

        ////释放 HTTP客户端
        public static void RemoveHTTPClient(long Context)
        {
            api.Sunny_RemoveHTTPClient(new IntPtr(Context));
        }

        ////创建 HTTP 客户端
        public static long CreateHTTPClient()
        {
            return api.Sunny_CreateHTTPClient().ToInt64();
        }

        ////JSON格式的protobuf数据转为protobuf二进制数据
        public static byte[] JsonToPB(string Json)
        {
            byte[] bs = Tool.StrToBytes(Json);
            IntPtr _Json = Tool.BytesToIntptr(bs);
            IntPtr aa = api.Sunny_JsonToPB(_Json, new IntPtr(bs.Length));
            byte[] ss = PtrAutoToBytes(aa);
            Free(aa);
            Tool.PtrFree(_Json);
            return ss;
        }

        ////protobuf数据转为JSON格式
        public static string PbToJson(byte[] bin)
        {
            IntPtr _bin = Tool.BytesToIntptr(bin);
            IntPtr aa = api.Sunny_PbToJson(_bin, new IntPtr(bin.Length));
            string ss = Tool.PtrToString(aa);
            Tool.PtrFree(_bin);
            Free(aa);
            return ss;
        }

        ////队列弹出
        public static byte[] QueuePull(string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            IntPtr aa = api.Sunny_QueuePull(_name);
            byte[] ss = PtrAutoToBytes(aa);
            Tool.PtrFree(_name);
            Free(aa);
            return ss;
        }

        ////加入队列
        public static void QueuePush(string name, byte[] val)
        {
            IntPtr _bin = Tool.BytesToIntptr(val);
            IntPtr _name = Tool.StringToIntptr(name);
            api.Sunny_QueuePush(_name, _bin, new IntPtr(val.Length));
            Tool.PtrFree(_bin);
            Tool.PtrFree(_name);
        }

        ////取队列长度
        public static int QueueLength(string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            int res = (int)api.Sunny_QueueLength(_name).ToInt64();
            Tool.PtrFree(_name);
            return res;
        }

        ////清空销毁队列
        public static void QueueRelease(string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            api.Sunny_QueueRelease(_name);
            Tool.PtrFree(_name);
        }

        ////队列是否为空
        public static bool QueueIsEmpty(string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            bool res = api.Sunny_QueueIsEmpty(_name);
            Tool.PtrFree(_name);
            return res;
        }

        ////创建队列
        public static void CreateQueue(string name)
        {
            IntPtr _name = Tool.StringToIntptr(name);
            api.Sunny_CreateQueue(_name);
            Tool.PtrFree(_name);
        }

        ////TCP客户端 发送数据
        public static int SocketClientWrite(long Context, int OutTimes, byte[] val)
        {
            IntPtr _val = Tool.BytesToIntptr(val);
            int res = (int)api.Sunny_SocketClientWrite(new IntPtr(Context), new IntPtr(OutTimes), _val, new IntPtr(val.Length)).ToInt64();
            Tool.PtrFree(_val);
            return res;
        }

        ////TCP客户端 断开连接
        public static void SocketClientClose(long Context)
        {
            api.Sunny_SocketClientClose(new IntPtr(Context));
        }

        ////TCP客户端 同步模式下 接收数据
        public static byte[] SocketClientReceive(long Context, int OutTimes)
        {
            IntPtr aa = api.Sunny_SocketClientReceive(new IntPtr(Context), new IntPtr(OutTimes));
            byte[] ss = PtrAutoToBytes(aa);
            Free(aa);
            return ss;
        }

        ////TCP客户端 连接
        public static bool SocketClientDial(long Context, string addr, IntPtr call, bool isTls, bool synchronous, string ProxyUrl, CertManager Cert, int timeOut, string RouterIP)
        {
            IntPtr _addr = Tool.StringToIntptr(addr);
            IntPtr _RouterIP = Tool.StringToIntptr(RouterIP);
            IntPtr _ProxyUrl = Tool.StringToIntptr(ProxyUrl);
            IntPtr id = IntPtr.Zero;
            if (Cert != null)
            {
                id = new IntPtr(Cert.Context());
            }
            bool res = api.Sunny_SocketClientDial(new IntPtr(Context), _addr, call, isTls, synchronous, _ProxyUrl, id, new IntPtr(timeOut), _RouterIP);
            Tool.PtrFree(_addr);
            Tool.PtrFree(_ProxyUrl);
            Tool.PtrFree(_RouterIP);
            return res;
        }

        ////TCP客户端 置缓冲区大小
        public static bool SocketClientSetBufferSize(long Context, int BufferSize)
        {
            return api.Sunny_SocketClientSetBufferSize(new IntPtr(Context), new IntPtr(BufferSize));
        }

        ////TCP客户端 取错误
        public static string SocketClientGetErr(long Context)
        {
            IntPtr aa = api.Sunny_SocketClientGetErr(new IntPtr(Context));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////释放 TCP客户端
        public static void RemoveSocketClient(long Context)
        {
            api.Sunny_RemoveSocketClient(new IntPtr(Context));
        }

        ////创建 TCP客户端
        public static long CreateSocketClient()
        {
            return api.Sunny_CreateSocketClient().ToInt64();
        }

        ////Websocket客户端 同步模式下 接收数据 返回数据指针 失败返回0 length=返回数据长度
        public static byte[] WebsocketClientReceive(out int wsType, long Context, int OutTimes)
        {
            wsType = Const.WsMessageType.Invalid;
            IntPtr p = api.Sunny_WebsocketClientReceive(new IntPtr(Context), new IntPtr(OutTimes));
            if (p.ToInt64() < 1)
            {
                return new byte[0];
            }
            long plen = BytesToInt(p, 8);
            byte[] mbin = Tool.PtrToBytes(p, 8, 8);
            IntPtr p1 = Tool.BytesToIntptr(mbin);
            int msgtype = Bridge.BytesToInt(p1, 8);
            Tool.PtrFree(p1);
            wsType = msgtype;
            mbin = Tool.PtrToBytes(p, (int)plen, 16);
            Free(p);
            return mbin;
        }

        ////Websocket客户端  发送数据
        public static bool WebsocketReadWrite(long Context, byte[] data, int messageType)
        {
            IntPtr aa = Tool.BytesToIntptr(data);
            bool res = api.Sunny_WebsocketReadWrite(new IntPtr(Context), aa, new IntPtr(data.Length), new IntPtr(messageType));
            Tool.PtrFree(aa);
            return res;
        }

        ////Websocket客户端 断开
        public static void WebsocketClose(long Context)
        {
            api.Sunny_WebsocketClose(new IntPtr(Context));
        }

        ////Websocket客户端 连接
        public static bool WebsocketDial(long Context, string URL, string Heads, IntPtr call, bool synchronous, string ProxyUrl, CertManager Cert, int outTime, string RouterIP)
        {
            IntPtr _URL = Tool.StringToIntptr(URL);
            IntPtr _Heads = Tool.StringToIntptr(Heads);
            IntPtr _RouterIP = Tool.StringToIntptr(RouterIP);
            IntPtr _ProxyUrl = Tool.StringToIntptr(ProxyUrl);
            IntPtr id = IntPtr.Zero;
            if (Cert != null)
            {
                id = new IntPtr(Cert.Context());
            }
            bool res = api.Sunny_WebsocketDial(new IntPtr(Context), _URL, _Heads, call, synchronous, _ProxyUrl, id, new IntPtr(outTime), _RouterIP);
            Tool.PtrFree(_URL);
            Tool.PtrFree(_Heads);
            Tool.PtrFree(_RouterIP);
            Tool.PtrFree(_ProxyUrl);
            return res;
        }

        ////Websocket客户端 设置心跳函数
        public static void WebsocketHeartbeat(long Context, long HeartbeatTime, IntPtr call)
        {
            api.Sunny_WebsocketHeartbeat(new IntPtr(Context), new IntPtr(HeartbeatTime), call);
        }

        ////Websocket客户端 获取错误
        public static string WebsocketGetErr(long Context)
        {
            IntPtr aa = api.Sunny_WebsocketGetErr(new IntPtr(Context));
            string ss = Tool.PtrToString(aa);
            Free(aa);
            return ss;
        }

        ////释放 Websocket客户端 对象
        public static void RemoveWebsocket(long Context)
        {
            api.Sunny_RemoveWebsocket(new IntPtr(Context));
        }

        ////创建 Websocket客户端 对象
        public static long CreateWebsocket()
        {
            return api.Sunny_CreateWebsocket().ToInt64();
        }

        ////创建 Http证书管理器 对象 实现指定Host使用指定证书
        public static bool AddHttpCertificate(string host, CertManager Cert, int Rules)
        {
            IntPtr id = IntPtr.Zero;
            if (Cert != null)
            {
                id = new IntPtr(Cert.Context());
            }
            IntPtr _host = Tool.StringToIntptr(host);
            bool res = api.Sunny_AddHttpCertificate(_host, id, new IntPtr(Rules));
            Tool.PtrFree(_host);
            return res;
        }

        ////删除 Http证书管理器 对象
        public static void DelHttpCertificate(string host)
        {
            IntPtr _host = Tool.StringToIntptr(host);
            api.Sunny_DelHttpCertificate(_host);
            Tool.PtrFree(_host);
        }

        //// Redis 订阅消息
        public static bool RedisSubscribe(long Context, string scribe, IntPtr call, bool nc)
        {
            IntPtr _scribe = Tool.StringToIntptr(scribe);
            bool rs = api.Sunny_RedisSubscribe(new IntPtr(Context), _scribe, call, nc);
            Tool.PtrFree(_scribe);
            return rs;
        }

        //// Redis 删除
        public static bool RedisDelete(long Context, string key)
        {
            IntPtr _key = Tool.StringToIntptr(key);
            bool res = api.Sunny_RedisDelete(new IntPtr(Context), _key);
            Tool.PtrFree(_key);
            return res;
        }

        //// Redis 清空当前数据库
        public static void RedisFlushDB(long Context)
        {
            api.Sunny_RedisFlushDB(new IntPtr(Context));
        }

        //// Redis 清空redis服务器
        public static void RedisFlushAll(long Context)
        {
            api.Sunny_RedisFlushAll(new IntPtr(Context));
        }

        //// Redis 关闭
        public static void RedisClose(long Context)
        {
            api.Sunny_RedisClose(new IntPtr(Context));
        }

        //// Redis 取整数值
        public static long RedisGetInt(long Context, string key)
        {
            IntPtr _key = Tool.StringToIntptr(key);
            long res = api.Sunny_RedisGetInt(new IntPtr(Context), _key);
            Tool.PtrFree(_key);
            return res;
        }

        //// Redis 取指定条件键名
        public static string[] RedisGetKeys(long Context, string key)
        {
            IntPtr _key = Tool.StringToIntptr(key);
            IntPtr res = api.Sunny_RedisGetKeys(new IntPtr(Context), _key);
            Tool.PtrFree(_key);
            byte[] aa = PtrAutoToBytes(res);
            return SplitByZero(aa);

        }
        private static string[] SplitByZero(byte[] data)
        {
            List<string> result = new List<string>();
            List<byte> current = new List<byte>();

            foreach (var b in data)
            {
                if (b == 0)
                {
                    if (current.Count > 0)
                    {
                        byte[] bs = current.ToArray();
                        result.Add(Tool.BytesToStr(bs));
                        current.Clear();
                    }
                }
                else
                {
                    current.Add(b);
                }
            }

            // 添加最后一组数据（如果存在）
            if (current.Count > 0)
            {
                byte[] bs = current.ToArray();
                result.Add(Tool.BytesToStr(bs));
                current.Clear();
            }

            return result.ToArray();
        }
        //// Redis 自定义 执行和查询命令 返回操作结果可能是值 也可能是JSON文本
        public static string RedisDo(long Context, string args, IntPtr error)
        {
            IntPtr _args = Tool.StringToIntptr(args);
            IntPtr res = api.Sunny_RedisDo(new IntPtr(Context), _args, error);
            Tool.PtrFree(_args);
            string rr = Tool.PtrToString(res);
            Free(res);
            return rr;
        }

        //// Redis 取文本值
        public static string RedisGetStr(long Context, string key)
        {
            IntPtr _key = Tool.StringToIntptr(key);
            IntPtr res = api.Sunny_RedisGetStr(new IntPtr(Context), _key);
            Tool.PtrFree(_key);
            string rr = Tool.PtrToString(res);
            Free(res);
            return rr;
        }

        //// Redis 取Bytes值
        public static byte[] RedisGetBytes(long Context, string key)
        {
            IntPtr _key = Tool.StringToIntptr(key);
            IntPtr res = api.Sunny_RedisGetBytes(new IntPtr(Context), _key);
            Tool.PtrFree(_key);
            byte[] rr = PtrAutoToBytes(res);
            Free(res);
            return rr;
        }

        //// Redis 检查指定 key 是否存在
        public static bool RedisExists(long Context, string key)
        {
            IntPtr _key = Tool.StringToIntptr(key);
            bool res = api.Sunny_RedisExists(new IntPtr(Context), _key);
            Tool.PtrFree(_key);
            return res;
        }

        //// Redis 设置NX 【如果键名存在返回假】
        public static bool RedisSetNx(long Context, string key, string val, int expr)
        {
            IntPtr _key = Tool.StringToIntptr(key);
            IntPtr _val = Tool.StringToIntptr(val);
            bool res = api.Sunny_RedisSetNx(new IntPtr(Context), _key, _val, new IntPtr(expr));
            Tool.PtrFree(_key);
            Tool.PtrFree(_val);
            return res;
        }

        //// Redis 设置值
        public static bool RedisSet(long Context, string key, string val, int expr)
        {
            IntPtr _key = Tool.StringToIntptr(key);
            IntPtr _val = Tool.StringToIntptr(val);
            bool res = api.Sunny_RedisSet(new IntPtr(Context), _key, _val, new IntPtr(expr));
            Tool.PtrFree(_key);
            Tool.PtrFree(_val);
            return res;
        }

        //// Redis 设置Bytes值
        public static bool RedisSetBytes(long Context, string key, byte[] val, int expr)
        {
            IntPtr _key = Tool.StringToIntptr(key);
            IntPtr _val = Tool.BytesToIntptr(val);
            bool res = api.Sunny_RedisSetBytes(new IntPtr(Context), _key, _val, new IntPtr(val.Length), new IntPtr(expr));
            Tool.PtrFree(_key);
            Tool.PtrFree(_val);
            return res;
        }

        //// Redis 连接
        public static bool RedisDial(long Context, string host, string pass, int db, int PoolSize, int MinIdleCons, int DialTimeout, int ReadTimeout, int WriteTimeout, int PoolTimeout, int IdleCheckFrequency, int IdleTimeout, IntPtr error)
        {
            IntPtr _host = Tool.StringToIntptr(host);
            IntPtr _pass = Tool.StringToIntptr(pass);
            bool res = api.Sunny_RedisDial(new IntPtr(Context), _host, _pass, new IntPtr(db), new IntPtr(PoolSize), new IntPtr(MinIdleCons), new IntPtr(DialTimeout), new IntPtr(ReadTimeout), new IntPtr(WriteTimeout), new IntPtr(PoolTimeout), new IntPtr(IdleCheckFrequency), new IntPtr(IdleTimeout), error);
            Tool.PtrFree(_host);
            Tool.PtrFree(_pass);
            return res;
        }

        //// 释放 Redis 对象
        public static void RemoveRedis(long Context)
        {
            api.Sunny_RemoveRedis(new IntPtr(Context));
        }

        //// 创建 Redis 对象
        public static long CreateRedis()
        {
            return api.Sunny_CreateRedis().ToInt64();
        }


        ////获取 UDP消息 返回数据指针
        public static byte[] GetUdpData(long MessageId)
        {
            IntPtr d = api.Sunny_GetUdpData(new IntPtr(MessageId));
            byte[] sss = PtrAutoToBytes(d);
            Free(d);
            return sss;
        }

        ////修改设置 UDP消息 返回数据指针
        public static bool SetUdpData(long MessageId, byte[] val)
        {
            IntPtr _val = Tool.BytesToIntptr(val);
            bool res = api.Sunny_SetUdpData(new IntPtr(MessageId), _val, new IntPtr(val.Length));
            Tool.PtrFree(_val);
            return res;
        }
        ////加指定的UDP连接 模拟客户端向服务器端主动发送数据
        public static bool UdpSendToServer(long MessageId, byte[] val)
        {
            IntPtr _val = Tool.BytesToIntptr(val);
            bool res = api.Sunny_UdpSendToServer(new IntPtr(MessageId), _val, new IntPtr(val.Length));
            Tool.PtrFree(_val);
            return res;
        }
        ////指定的UDP连接 模拟服务器端向客户端主动发送数据
        public static bool UdpSendToClient(long MessageId, byte[] val)
        {
            IntPtr _val = Tool.BytesToIntptr(val);
            bool res = api.Sunny_UdpSendToClient(new IntPtr(MessageId), _val, new IntPtr(val.Length));
            Tool.PtrFree(_val);
            return res;
        }
        /// <summary> 
        /// 获取SunnyNet DLL 版本
        /// </summary>
        /// <returns></returns>
        public static string GetSunnyVersion()
        {
            IntPtr d = api.Sunny_GetSunnyVersion();
            string ss = Tool.PtrToString(d);
            Free(d);
            return ss;
        }
    }
}
