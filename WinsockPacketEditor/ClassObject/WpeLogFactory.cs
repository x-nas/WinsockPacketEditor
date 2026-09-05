using System;
using SuperSocket.SocketBase.Logging;

namespace WinsockPacketEditor
{
    #region//SuperSocket 的日志出口

    /// <summary>
    /// 把 SuperSocket 内部的日志接到 <see cref="Operate.DoLog"/> 上。
    ///
    /// 【为什么要有这个类】
    /// SuperSocket 的 <c>AppServerBase.Setup()</c> 在不指定 ILogFactory 时会默认构造
    /// <c>Log4NetLogFactory</c>，于是硬性依赖 log4net.dll —— 而且要的是它编译时
    /// 绑定的 <b>1.2.13.0</b>，与本项目 packages 里的 3.3.1 对不上，得靠 app.config
    /// 的绑定重定向才能跑起来。
    ///
    /// WPE 自己的代码<b>一行都没用过 log4net</b>（日志走 LogConfig 的内存队列），
    /// 这个依赖纯粹是 SuperSocket 的默认值带来的。外壳工程因此一启动代理就抛
    /// FileNotFoundException —— 它没有那份 DLL，也没有那条重定向。
    ///
    /// 【为什么不是换成 ConsoleLogFactory】
    /// SuperSocket 自带的那个把日志写 Console，而 WinForms 程序没有控制台，
    /// 等于把这些信息直接丢掉。SuperSocket 内部的错误（端口占用、会话异常）
    /// 恰恰是排查代理问题最有用的东西，接进 WPE 自己的日志才是对的 ——
    /// 这样两套 UI 的「系统日志」里都能看到，比原先写进没人看的 Logs\*.log 更好。
    ///
    /// 【级别】Debug 与 Info 默认关掉：SuperSocket 在每条连接上都会打 Debug，
    /// 抓包场景下每秒几千条，会把日志列表冲垮。Warn 以上才收。
    /// </summary>
    public sealed class WpeLogFactory : ILogFactory
    {
        public ILog GetLog(string name)
        {
            return new WpeLog(name);
        }
    }

    /// <summary>
    /// <see cref="ILog"/> 的实现，转发给 <see cref="Operate.DoLog"/>。
    ///
    /// 名字取 SuperSocket 传进来的 logger 名（通常是服务名），
    /// 在日志列表的「函数名」那一列显示成 <c>SuperSocket.Socks5ProxyServer</c> 这样。
    /// </summary>
    internal sealed class WpeLog : ILog
    {
        private readonly string source;

        public WpeLog(string Name)
        {
            this.source = "SuperSocket." + (string.IsNullOrEmpty(Name) ? "Server" : Name);
        }

        #region//级别开关

        /*
            Debug / Info 关掉是有意的，见类注释：SuperSocket 会在每条连接、每个包上打点，
            抓包场景下这两级每秒能有几千条，日志列表（前端 2000 条上限）会被瞬间冲空。
            真要排查协议层问题时把这两个改成 true 即可。
        */
        public bool IsDebugEnabled { get { return false; } }

        public bool IsInfoEnabled { get { return false; } }

        public bool IsWarnEnabled { get { return true; } }

        public bool IsErrorEnabled { get { return true; } }

        public bool IsFatalEnabled { get { return true; } }

        #endregion

        #region//写入

        private void Write(string Level, object Message, Exception Ex)
        {
            try
            {
                string text = Message == null ? string.Empty : Message.ToString();

                if (Ex != null)
                {
                    text = text.Length > 0 ? text + Environment.NewLine + Ex : Ex.ToString();
                }

                Operate.DoLog(this.source + " [" + Level + "]", text);
            }
            catch
            {
                //日志出口本身不能再抛：它可能正在处理另一条异常的记录
            }
        }

        private void WriteFormat(string Level, IFormatProvider Provider, string Format, params object[] Args)
        {
            try
            {
                //格式串来自 SuperSocket，参数个数对不上时 string.Format 会抛 —— 兜住，原样记下
                string text = Provider == null
                    ? string.Format(Format, Args)
                    : string.Format(Provider, Format, Args);

                this.Write(Level, text, null);
            }
            catch
            {
                this.Write(Level, Format, null);
            }
        }

        #endregion

        #region//ILog 的 35 个成员

        public void Debug(object message) { }

        public void Debug(object message, Exception exception) { }

        public void DebugFormat(string format, object arg0) { }

        public void DebugFormat(string format, object arg0, object arg1) { }

        public void DebugFormat(string format, object arg0, object arg1, object arg2) { }

        public void DebugFormat(string format, params object[] args) { }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args) { }

        public void Info(object message) { }

        public void Info(object message, Exception exception) { }

        public void InfoFormat(string format, object arg0) { }

        public void InfoFormat(string format, object arg0, object arg1) { }

        public void InfoFormat(string format, object arg0, object arg1, object arg2) { }

        public void InfoFormat(string format, params object[] args) { }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args) { }

        public void Warn(object message) { this.Write("WARN", message, null); }

        public void Warn(object message, Exception exception) { this.Write("WARN", message, exception); }

        public void WarnFormat(string format, object arg0) { this.WriteFormat("WARN", null, format, arg0); }

        public void WarnFormat(string format, object arg0, object arg1) { this.WriteFormat("WARN", null, format, arg0, arg1); }

        public void WarnFormat(string format, object arg0, object arg1, object arg2) { this.WriteFormat("WARN", null, format, arg0, arg1, arg2); }

        public void WarnFormat(string format, params object[] args) { this.WriteFormat("WARN", null, format, args); }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args) { this.WriteFormat("WARN", provider, format, args); }

        public void Error(object message) { this.Write("ERROR", message, null); }

        public void Error(object message, Exception exception) { this.Write("ERROR", message, exception); }

        public void ErrorFormat(string format, object arg0) { this.WriteFormat("ERROR", null, format, arg0); }

        public void ErrorFormat(string format, object arg0, object arg1) { this.WriteFormat("ERROR", null, format, arg0, arg1); }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2) { this.WriteFormat("ERROR", null, format, arg0, arg1, arg2); }

        public void ErrorFormat(string format, params object[] args) { this.WriteFormat("ERROR", null, format, args); }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) { this.WriteFormat("ERROR", provider, format, args); }

        public void Fatal(object message) { this.Write("FATAL", message, null); }

        public void Fatal(object message, Exception exception) { this.Write("FATAL", message, exception); }

        public void FatalFormat(string format, object arg0) { this.WriteFormat("FATAL", null, format, arg0); }

        public void FatalFormat(string format, object arg0, object arg1) { this.WriteFormat("FATAL", null, format, arg0, arg1); }

        public void FatalFormat(string format, object arg0, object arg1, object arg2) { this.WriteFormat("FATAL", null, format, arg0, arg1, arg2); }

        public void FatalFormat(string format, params object[] args) { this.WriteFormat("FATAL", null, format, args); }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args) { this.WriteFormat("FATAL", provider, format, args); }

        #endregion
    }

    #endregion
}
