using SunnyNetlibray.Event;
using SunnyNetlibray.Internal;
using System.Runtime.InteropServices;
using SunnyNetInternal = SunnyNetlibray.Internal.SunnyNet;
using System;

namespace SunnyNetlibray
{

    public class SunnyNet
    {
        public static string Version()
        {
            return Bridge.GetSunnyVersion();
        }
        private SunnyNetInternal _callback = null;
        private NetHttpCallback _httpCallback = null;
        private NetTcpCallback _tcpCallback = null;
        private NetUDPCallback _udpCallback = null;
        private NetWebsockCallback _websocketCallback = null;
        private NetScriptSaveCallback _scriptSaveCallback = null;
        private NetScriptLogCallback _scriptLogCallback = null;

        private delegate void NetHttpCallback(IntPtr sunnyContext, IntPtr uniqueId, IntPtr messageId, IntPtr eventType, IntPtr method, IntPtr url, IntPtr error, IntPtr pid);
        private delegate void NetTcpCallback(IntPtr sunnyContext, IntPtr localAddr, IntPtr remoteAddr, IntPtr eventType, IntPtr messageId, IntPtr data, IntPtr dataLen, IntPtr theologyId, IntPtr pid);
        private delegate void NetUDPCallback(IntPtr sunnyContext, IntPtr localAddr, IntPtr remoteAddr, IntPtr eventType, IntPtr messageId, IntPtr theologyId, IntPtr pid);
        private delegate void NetWebsockCallback(IntPtr sunnyContext, IntPtr uniqueId, IntPtr messageId, IntPtr eventType, IntPtr method, IntPtr url, IntPtr pid, IntPtr wsMessageType);
        private delegate void NetScriptSaveCallback(IntPtr sunnyContext, IntPtr ScriptCode, IntPtr ScriptLen);
        private delegate void NetScriptLogCallback(IntPtr sunnyContext, IntPtr Log);

        private void DefaultNetTcpCallback(IntPtr sunnyContext, IntPtr localAddr, IntPtr remoteAddr, IntPtr eventType, IntPtr messageId, IntPtr data, IntPtr dataLen, IntPtr theologyId, IntPtr pid)
        {
            if (_callback != null)
            {
                TCPEvent obj = new TCPEvent(sunnyContext, Tool.PtrToString(localAddr), Tool.PtrToString(remoteAddr), eventType, messageId, Tool.PtrToBytes(data, (int)dataLen.ToInt64()), theologyId, pid);
                _callback.OnTcpCallback(obj);
            }
        }

        private void DefaultNetUDPCallback(IntPtr sunnyContext, IntPtr localAddr, IntPtr remoteAddr, IntPtr eventType, IntPtr messageId, IntPtr theologyId, IntPtr pid)
        {
            if (_callback != null)
            {
                UDPEvent obj = new UDPEvent(sunnyContext, Tool.PtrToString(localAddr), Tool.PtrToString(remoteAddr), eventType, messageId, theologyId, pid);
                _callback.OnUdpCallback(obj);
            }
        }

        private void DefaultNetWebsocketCallback(IntPtr sunnyContext, IntPtr theologyId, IntPtr messageId, IntPtr eventType, IntPtr method, IntPtr url, IntPtr pid, IntPtr wsMessageType)
        {
            if (_callback != null)
            {
                WebSocketEvent obj = new WebSocketEvent(sunnyContext, theologyId, messageId, eventType, Tool.PtrToString(method), Tool.PtrToString(url), pid, wsMessageType);
                _callback.OnWebSocketCallback(obj);
            }
        }

        private void DefaultNetHttpCallback(IntPtr sunnyContext, IntPtr theologyId, IntPtr messageId, IntPtr eventType, IntPtr method, IntPtr url, IntPtr error, IntPtr pid)
        {
            if (_callback != null)
            {
                HTTPEvent obj = new HTTPEvent(sunnyContext, theologyId, messageId, eventType, Tool.PtrToString(method), Tool.PtrToString(url), Tool.PtrToString(error), pid);
                _callback.OnHttpCallback(obj);
            }
        }
        private void DefaultNetScriptLogCallback(IntPtr sunnyContext, IntPtr Log)
        {
            if (_callback != null)
            {
                EventValue obj = new EventValue(Tool.PtrToBytes(Log));
                _callback.OnScriptLogCallback(sunnyContext.ToInt64(), obj);
            }
        }
        private void DefaultNetScriptSaveCallback(IntPtr sunnyContext, IntPtr ScriptCode, IntPtr ScriptLen)
        {
            if (_callback != null)
            {
                EventValue obj = new EventValue(Tool.PtrToBytes(ScriptCode, ScriptLen.ToInt64()));
                _callback.OnScriptCodeSaveCallback(sunnyContext.ToInt64(), obj);
            }
        }

        private long _sunnyNetContext = 0;

        public SunnyNet()
        {
            _sunnyNetContext = Bridge.CreateSunnyNet();
        }

        ~SunnyNet()
        {
            // 自动销毁
            Bridge.ReleaseSunnyNet(_sunnyNetContext);
        }

        /// <summary>
        /// 导出已经设置的证书。
        /// </summary>
        public string ExportCertificate()
        {
            return Bridge.ExportCert(_sunnyNetContext);
        }

        /// <summary>
        /// 强制客户端使用 TCP，HTTPS 数据将无法解码。
        /// </summary>
        public void MustTcp(bool open)
        {
            Bridge.SunnyNetMustTcp(_sunnyNetContext, open);
        }
        /// <summary>
        /// <para>中间件设置出口IP函数-全局。</para>
        /// <para>你也可以在HTTP/TCP回调中对某个请求单独设置出口IP</para>
        /// </summary>
        /// <param name="ip">请输入网卡对应的内网IP地址,输入空文本,则让系统自动选择</param>
        public void SetOutRouterIP(String ip)
        {
            Bridge.SetOutRouterIP(_sunnyNetContext, ip);
        }

        /// <summary>
        /// 开启身份验证模式，客户端只能使用 S5 代理，并输入设置的账号密码。
        /// </summary>
        public void EnableAuthenticationMode(bool open)
        {
            Bridge.SunnyNetVerifyUser(_sunnyNetContext, open);
        }

        /// <summary>
        /// 在身份验证模式下添加用户名。
        /// </summary>
        public bool AddUserToAuthenticationMode(string user, string pass)
        {
            return Bridge.SunnyNetSocket5AddUser(_sunnyNetContext, user, pass);
        }

        /// <summary>
        /// 在身份验证模式下删除用户名。
        /// </summary>
        public bool RemoveUserFromAuthenticationMode(string user)
        {
            return Bridge.SunnyNetSocket5DelUser(_sunnyNetContext, user);
        }

        /// <summary>
        /// 绑定回调地址，在启动之前调用。
        /// </summary>
        public bool BindCallback(SunnyNetInternal callback = null)
        {
            _httpCallback = new NetHttpCallback(DefaultNetHttpCallback);
            _tcpCallback = new NetTcpCallback(DefaultNetTcpCallback);
            _websocketCallback = new NetWebsockCallback(DefaultNetWebsocketCallback);
            _udpCallback = new NetUDPCallback(DefaultNetUDPCallback);
            _scriptLogCallback = new NetScriptLogCallback(DefaultNetScriptLogCallback);
            _scriptSaveCallback = new NetScriptSaveCallback(DefaultNetScriptSaveCallback);

            IntPtr tcpPtr = Marshal.GetFunctionPointerForDelegate(_tcpCallback);
            IntPtr httpPtr = Marshal.GetFunctionPointerForDelegate(_httpCallback);
            IntPtr websocketPtr = Marshal.GetFunctionPointerForDelegate(_websocketCallback);
            IntPtr udpPtr = Marshal.GetFunctionPointerForDelegate(_udpCallback);
            IntPtr LogPtr = Marshal.GetFunctionPointerForDelegate(_scriptLogCallback);
            IntPtr SavePtr = Marshal.GetFunctionPointerForDelegate(_scriptSaveCallback);

            _callback = callback;
            return Bridge.SunnyNetSetCallback(_sunnyNetContext, httpPtr, tcpPtr, websocketPtr, udpPtr, LogPtr, SavePtr);
        }

        /// <summary>
        /// 在启动之前调用以绑定端口。
        /// </summary>
        public bool BindPort(int port)
        {
            return Bridge.SunnyNetSetPort(_sunnyNetContext, port);
        }

        /// <summary>
        /// 禁用非 HTTP/S 的 TCP 连接。
        /// </summary>
        public bool DisableTcpRequests(bool disable)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 开启后，请求将使用随机的 TLS 指纹，关闭后固定指纹也会被取消。
        /// </summary>
        public bool EnableRandomTLSFingerprinting(bool open)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取 SunnyNet 上下文。
        /// </summary>
        public long GetSunnyNetContext()
        {
            return _sunnyNetContext;
        }

        /// <summary>
        /// 启动 SunnyNet，需先绑定端口。
        /// </summary>
        public bool Start()
        {
            return Bridge.SunnyNetStart(_sunnyNetContext);
        }

        /// <summary>
        /// 取消已设置的 IE 代理。
        /// </summary>
        public bool CancelIEProxy()
        {
            return Bridge.CancelIEProxy(_sunnyNetContext);
        }

        /// <summary>
        /// 设置当前绑定的端口号为当前 IE 代理，设置前请先绑定端口。
        /// </summary>
        public bool SetIEProxy()
        {
            return Bridge.SetIeProxy(_sunnyNetContext);
        }

        /// <summary>
        /// 停止代理并自动关闭 IE 代理。
        /// </summary>
        public bool Stop()
        {
            CancelIEProxy();
            return Bridge.SunnyNetClose(_sunnyNetContext);
        }

        /// <summary>
        /// 设置自定义 CA 证书。
        /// </summary>
        public bool SetCustomCACertificate(CertManager cert)
        {
            return Bridge.SunnyNetSetCert(_sunnyNetContext, cert);
        }

        /// <summary>
        /// 安装证书到系统。
        /// </summary>
        public bool InstallCertificate()
        {
            string err = Bridge.SunnyNetInstallCert(_sunnyNetContext);
            return err.Contains("添加到存储") || err.Contains("已经在存储中");
        }

        /// <summary>
        /// 获取中间件启动时的错误信息。
        /// </summary>
        public string GetError()
        {
            return Bridge.SunnyNetError(_sunnyNetContext);
        }
        /// <summary>
        /// 设置全局上游代理。
        /// <para>例如，以下示例格式：</para>
        ///  <list type="bullet">
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list>
        /// </summary>
        /// <param name="proxyAddress">代理 URL，指定要使用的代理地址。
        /// <para>例如，以下示例格式：</para>
        ///  <list type="bullet">
        ///     <item>HTTP代理, 有账号密码: <c>http://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>S5代理, 有账号密码: <c>socket5://admin:123456@127.0.0.1:8888</c></item>
        ///     <item>HTTP代理, 无账号密码: <c>http://127.0.0.1:8888</c></item>
        ///     <item>S5代理, 无账号密码: <c>socket5://127.0.0.1:8888</c></item>
        /// </list></param>
        /// <param name="outTime">代理超时(毫秒)，默认30秒</param>
        public bool SetGlobalProxy(string proxyAddress, int outTime = 30000)
        {
            return Bridge.SetGlobalProxy(_sunnyNetContext, proxyAddress, outTime);
        }
        /// <summary>
        /// 取消全局上游代理。
        /// </summary>
        public void CancelGlobalProxy()
        {
            SetGlobalProxy("");
        }

        /// <summary>
        /// 设置上游代理使用规则。
        /// 输入 Host 不带端口号；在此规则内的不使用上游代理，多用分号或换行分割，例如 "127.0.0.1;192.168.*.*"。
        /// </summary>
        public bool SetGlobalProxyUsageRule(string regexp)
        {
            return Bridge.CompileProxyRegexp(_sunnyNetContext, regexp);
        }

        /// <summary>
        /// 设置 DNS 服务器地址，仅支持 TLS 的 DNS 服务器，即 853 端口的，例如: 223.5.5.5:853
        /// </summary>
        /// <param name="regexp">DNS 服务器地址。</param>
        public void SetDnsServer(string regexp)
        {
            Bridge.SetDnsServer(regexp);
        }

        /// <summary>
        /// 设置是否随机使用 TLS 指纹。
        /// </summary>
        /// <param name="random">如果为 true，则随机使用 TLS 指纹；否则不随机。</param>
        /// <returns>返回设置是否成功。</returns>
        public bool RandomJa3(bool random)
        {
            return Bridge.SetRandomTLS(_sunnyNetContext, random);
        }

        /// <summary>
        /// 禁用或启用 UDP 功能.
        /// </summary>
        /// <param name="disable">是否禁用 UDP。</param>
        /// <returns>成功返回 true，失败返回 false。</returns>
        public bool DisableUDP(bool disable)
        {
            return Bridge.DisableUDP(_sunnyNetContext, disable);
        }

        /// <summary>
        /// 禁用或启用 TCP 功能.
        /// </summary>
        /// <param name="disable">是否禁用 TCP。</param>
        /// <returns>成功返回 true，失败返回 false。</returns>
        public bool DisableTCP(bool disable)
        {
            return Bridge.DisableTCP(_sunnyNetContext, disable);
        }

        /// <summary>
        /// 当前SDK是否支持脚本代码。
        /// 如果当前SDK是Mini 版本则不支持脚本代码。
        /// </summary>
        /// <returns>支持返回 true ,不支持返回 false</returns>
        public bool IsScriptCodeSupported()
        {
            return !SetScriptPage("").Equals("no", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 设置脚本编辑器页面路径，需不少于 8 个字符.
        /// </summary>
        /// <param name="pagePath">要设置的脚本编辑器页面路径。</param>
        /// <returns>返回设置后的管理页面路径</returns>
        public string SetScriptPage(string pagePath)
        {
            return Bridge.SetScriptPage(_sunnyNetContext, pagePath);
        }

        /// <summary>
        /// 设置脚本编辑器代码.
        /// </summary>
        /// <param name="code">脚本代码。</param>
        /// <param name="encoding">编码格式，默认为 "UTF-8"。</param>
        /// <returns>返回设置结果。</returns>
        public string SetScriptCode(string code, string encoding = "UTF-8")
        {
            return Bridge.SetScriptCode(_sunnyNetContext, code, encoding);
        }

        /// <summary>
        /// <list type="bullet">
        ///   <item>设置 HTTP 请求的最大更新长度.</item>
        ///   <item>超过此长度将转为元素请求,无法修改数据，只能将提交内容储存到文件.</item>
        ///   <item>在回调函数中使用:<see cref="Conn.Request().IsRequestRawBody()"/>检查是否为原始请求</item>
        ///   <item>在回调函数中使用:<see cref="Conn.Request().RawRequestDataToFile(filePath)"/>将POST提交的原始内容储存到文件</item>
        /// </list> 
        /// </summary>
        /// <param name="sunnyContext">SunnyNet 上下文。</param>
        /// <param name="maxLength">最大长度。</param>
        /// <returns>返回设置结果。</returns>
        public static bool SetHTTPRequestMaxUpdateLength(long sunnyContext, long maxLength)
        {
            return Bridge.SetHTTPRequestMaxUpdateLength(sunnyContext, maxLength);
        }

        /// <summary>
        /// <list type="bullet">
        ///   <item>创建 HTTP 证书管理器对象，实现指定 Host 使用指定证书.</item>
        ///   <item>创建后,可以使用 <see cref="DelHttpCertificate"/>删除此规则</item>
        /// </list> 
        /// </summary>
        /// <param name="host">指定的 Host。</param>
        /// <param name="cert">证书管理器对象。</param>
        /// <param name="rules">规则。</param>
        /// <returns>返回添加结果。</returns>
        public static bool AddHttpCertificate(string host, CertManager cert, int rules)
        {
            return Bridge.AddHttpCertificate(host, cert, rules);
        }

        /// <summary>
        /// <list type="bullet">
        ///   <item>删除 HTTP 证书管理器对象.</item>
        ///   <item>删除由 <see cref="AddHttpCertificate"/>添加的证书使用规则</item>
        /// </list>  
        /// </summary>
        /// <param name="host">指定的 Host。</param>
        public static void DelHttpCertificate(string host)
        {
            Bridge.DelHttpCertificate(host);
        }

        /// <summary>
        /// 设置强制走 TCP 规则。
        /// </summary>
        /// <param name="rule">要设置的规则字符串。</param>
        /// <param name="rulesAllow">是否允许走 TCP 的规则。
        /// <para>请使用以下常量模板之一：</para>
        /// <list type="bullet">
        ///   <item><see cref="Const.MustTcpRule.Outside"/>：规则之外走 TCP</item>
        ///   <item><see cref="Const.MustTcpRule.Within"/>：规则之内走 TCP</item>
        /// </list></param>
        /// <returns>返回设置是否成功。</returns>
        public bool SetMustTcpRule(string rule, bool rulesAllow = Const.MustTcpRule.Within)
        {
            return Bridge.SetMustTcpRegexp(_sunnyNetContext, rule, rulesAllow);
        }

        /// <summary>
        /// 加载进程代理驱动。
        /// </summary>
        /// 只允许一个 SunnyNet 使用，自动安装所需驱动文件。
        /// <param name="devMode">驱动模式  0=Proxifier,1=NFAPI,2=Tun
        ///
        ///【NFAPI驱动】win7请安装 KB3033929 补丁
        ///
        ///【Proxifier驱动】不支持UDP,不支持32位系统
        ///
        /// 【Tun驱动】不支持捕获本地127.0.0.1数据</param>
        public bool LoadDriver(long devMode)
        {
            return Bridge.OpenDrive(_sunnyNetContext, devMode);
        }
        /// <summary>
        /// 卸载驱动。
        /// <para>调用 <see cref="UnDrive"/> 卸载驱动，仅在 Windows 上有效【需要管理权限】。</para>
        /// <para>执行成功后会立即重启系统，若函数执行后没有重启系统，表示没有管理员权限。</para>
        /// </summary> 
        public void UnDriver()
        {
            Bridge.UnDrive(_sunnyNetContext);
        }

        /// <summary>
        /// 添加指定的进程名进行捕获。
        /// 需调用<see cref="LoadDriver"/> 后生效，会强制断开此进程已经连接的 TCP 连接。
        /// </summary>
        public void AddProcessName(string processName)
        {
            Bridge.ProcessAddName(_sunnyNetContext, processName);
        }

        /// <summary>
        /// 添加指定的进程 PID 进行捕获。
        /// 需调用<see cref="LoadDriver"/> 后生效，会强制断开此进程已经连接的 TCP 连接。
        /// </summary>
        public void AddProcessPid(int processPid)
        {
            Bridge.ProcessAddPid(_sunnyNetContext, processPid);
        }

        /// <summary>
        /// 删除指定的进程 PID 停止捕获。
        /// 需调用<see cref="LoadDriver"/> 后生效，会强制断开此进程已经连接的 TCP 连接。
        /// </summary>
        public void RemoveProcessPid(int processPid)
        {
            Bridge.ProcessDelPid(_sunnyNetContext, processPid);
        }

        /// <summary>
        /// 开启后，所有进程将会被捕获。
        /// 需调用<see cref="LoadDriver"/> 后生效，会强制断开所有进程已经连接的 TCP 连接。 
        /// </summary>
        /// <param name="enable">开启或关闭</param>
        /// <param name="stopNet">是否需要对所有进程都断网一次</param>
        public void SetCaptureAnyProcess(bool enable, bool stopNet)
        {
            Bridge.ProcessALLName(_sunnyNetContext, enable, stopNet);
        }

        /// <summary>
        /// 删除指定的进程名停止捕获。
        /// 需调用<see cref="LoadDriver"/> 后生效，会强制断开此进程已经连接的 TCP 连接。
        /// </summary>
        public void RemoveProcessName(string processName)
        {
            Bridge.ProcessDelName(_sunnyNetContext, processName);
        }

        /// <summary>
        /// 删除已设置的所有 PID 和进程名。
        /// 需调用<see cref="LoadDriver"/> 后生效，会强制断开所有进程已经连接的 TCP 连接。
        /// </summary>
        public void RemoveAllProcesses()
        {
            Bridge.ProcessCancelAll(_sunnyNetContext);
        }
    }
}