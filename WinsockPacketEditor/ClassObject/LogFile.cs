using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace WinsockPacketEditor
{
    #region//运行日志落盘

    /// <summary>
    /// 把 <see cref="Operate.DoLog"/> 的每一条同时写进磁盘文件。
    ///
    /// 【为什么要有它】运行日志原本<b>只进内存队列</b>（LogConfig.Queue），
    /// 进程一死就全没了 —— 而最需要日志的恰恰是「程序意外退出」那一刻。
    /// 真出过一次：账号表被清空，事后完全查不出是哪条路径干的，因为日志随进程走了。
    ///
    /// 【为什么不用 log4net】主工程一行都没用过它（那个 DLL 是 SuperSocket 在用，
    /// 见 WpeLogFactory）。为一件几十行能做完的事拖一个依赖 + 一条绑定重定向不划算。
    ///
    /// 【三条硬要求】
    ///   ① <b>写完就落到系统</b>：AutoFlush 让每条日志离开进程缓冲区。
    ///      崩溃时数据已经在 OS 文件缓存里，进程死了也还在
    ///      （不做 fsync —— 那是防断电的，代价大得多，这里不需要）。
    ///   ② <b>永不抛异常</b>：日志写不出去是小事，因此把程序搞崩是大事。
    ///      整条路径吞掉所有异常，打不开文件就退回成「只有内存日志」。
    ///   ③ <b>有上限</b>：超过 <see cref="MaxBytes"/> 就轮换，只留一份旧的，
    ///      磁盘占用封顶在两倍。
    /// </summary>
    public static class LogFile
    {
        #region//字段

        /// <summary>单个文件的上限。超过就轮换成 .1 —— 磁盘占用封顶 2 × 这个值。</summary>
        private const long MaxBytes = 4 * 1024 * 1024;

        private static readonly object Gate = new object();

        private static StreamWriter writer;
        private static string filePath;
        private static string rollPath;

        /// <summary>自己记字节数，省得每条日志都去 stat 一次文件。</summary>
        private static long written;

        /// <summary>打不开文件时置位，之后不再反复重试（省得每条日志都去撞一次权限）。</summary>
        private static bool disabled;

        #endregion

        #region//写入

        /// <summary>当前日志文件的完整路径。还没开过或开不了时返回空串。</summary>
        public static string Path
        {
            get { lock (Gate) { return filePath ?? string.Empty; } }
        }

        /// <summary>
        /// 写一条。<b>同步写</b>，不丢进线程池 ——
        /// 排队等着写的那几条正是崩溃时最想看到的，异步就等于白做。
        ///
        /// 代价是一次带缓冲的写 + 一次 WriteFile，微秒级；
        /// 而 DoLog 不在抓包热路径上（那条路只在 catch 里才记日志），
        /// SuperSocket 的 Debug / Info 两级也是关掉的。
        /// </summary>
        public static void Write(string FuncName, string Content)
        {
            try
            {
                lock (Gate)
                {
                    StreamWriter w = Open();

                    if (w == null)
                    {
                        return;
                    }

                    string line = string.Format(
                        "{0:yyyy-MM-dd HH:mm:ss.fff}  [{1}]  {2}  {3}",
                        DateTime.Now,
                        Process.GetCurrentProcess().Id,
                        FuncName ?? string.Empty,
                        Indent(Content));

                    w.WriteLine(line);
                    written += line.Length + 2;

                    if (written >= MaxBytes)
                    {
                        Roll();
                    }
                }
            }
            catch
            {
                //日志写不出去不该影响任何事。这里连 DoLog 都不能调 —— 会转回来递归
            }
        }

        /// <summary>
        /// 每次启动写一条分隔行。
        ///
        /// 多开时两个实例写同一个文件，加上进程号与库路径才分得清哪条是谁的；
        /// 事后翻日志时也是靠它切分「这是哪一次运行」。
        /// </summary>
        public static void BeginSession(string Version, string DbPath)
        {
            Write("Session",
                "========== WPE x64 " + (Version ?? "?") + " 启动"
                + "  进程 " + Process.GetCurrentProcess().Id
                + "  库 " + (DbPath ?? "?")
                + " ==========");
        }

        /// <summary>
        /// 记一条未处理异常并立刻落盘。
        ///
        /// 这是本类真正的用武之地：<b>「程序意外退出」以前不留任何痕迹</b>。
        /// 由入口处订阅 AppDomain.UnhandledException / Application.ThreadException 调过来。
        /// </summary>
        public static void Crash(string Source, object ExceptionObject)
        {
            Write("!! CRASH !! " + Source, ExceptionObject == null ? "(null)" : ExceptionObject.ToString());
        }

        #endregion

        #region//内部

        /// <summary>异常堆栈是多行的，后续行缩进，免得和下一条日志混在一起。</summary>
        private static string Indent(string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return string.Empty;
            }

            return Content.Replace("\r\n", "\n").Replace("\n", "\n        ");
        }

        private static StreamWriter Open()
        {
            if (writer != null)
            {
                return writer;
            }

            if (disabled)
            {
                return null;
            }

            try
            {
                string dir = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(Application_ExecutablePath()), "Logs");

                Directory.CreateDirectory(dir);

                filePath = System.IO.Path.Combine(dir, "wpe.log");
                rollPath = System.IO.Path.Combine(dir, "wpe.1.log");

                var fi = new FileInfo(filePath);
                written = fi.Exists ? fi.Length : 0;

                /*
                    Append + FileShare.ReadWrite：
                      Append —— 多开时两个进程各自追加，写入位置由系统保证不互相覆盖
                      ReadWrite —— 程序跑着的时候也能用记事本打开看
                */
                var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                //UTF-8 不带 BOM：追加时每次都写一个 BOM 会在文件中间留下乱码
                writer = new StreamWriter(fs, new UTF8Encoding(false)) { AutoFlush = true };

                return writer;
            }
            catch
            {
                //目录只读、磁盘满、被占用 …… 一律退回成「只有内存日志」，不再重试
                disabled = true;
                writer = null;
                return null;
            }
        }

        /// <summary>超上限就轮换：旧的那份改名成 .1（覆盖上一份），当前文件从头开始。</summary>
        private static void Roll()
        {
            try
            {
                writer.Dispose();
                writer = null;

                if (File.Exists(rollPath))
                {
                    File.Delete(rollPath);
                }

                File.Move(filePath, rollPath);
                written = 0;
            }
            catch
            {
                //轮换失败就继续用原来那份，顶多文件大一点，不值得为它出错
                written = 0;
            }
        }

        /// <summary>
        /// 取自身路径。
        /// 不直接用 <c>Application.ExecutablePath</c>，是为了让本类不依赖 WinForms ——
        /// 将来 Operate 整体搬进独立程序集时它得能跟着走。
        /// </summary>
        private static string Application_ExecutablePath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        #endregion
    }

    #endregion
}
