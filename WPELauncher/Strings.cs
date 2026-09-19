using System.Globalization;

namespace WPELauncher
{
    /// <summary>
    /// 启动器自己的几句文案。那一刻程序的数据库与界面语言都还没读出来，
    /// 所以按<b>系统界面语言</b>出中文或英文。
    ///
    /// ⚠️ 文案里不写产品名：同一个启动器给 WPE x64 与 WPE Proxy Cap 两个产品打包，
    /// 产品名只出现在窗口标题上（payload.txt 的 Title）。
    /// </summary>
    internal static class Strings
    {
        private static readonly bool Zh = CultureInfo.CurrentUICulture.Name.StartsWith("zh");

        private static string T(string zh, string en)
        {
            return Zh ? zh : en;
        }

        /// <summary>还没读到 payload.txt 时的窗口标题。</summary>
        public static string FallbackTitle { get { return "Launcher"; } }

        public static string Preparing { get { return T("首次运行，正在准备程序文件…", "Preparing program files for the first run…"); } }

        public static string Repairing { get { return T("正在修复程序文件…", "Repairing program files…"); } }

        public static string NoPayload
        {
            get { return T("这个 exe 里没有打包程序文件，请重新下载完整的版本。", "This exe does not contain the program files. Please download the full build again."); }
        }

        public static string Busy
        {
            get { return T("另一个启动器正在准备文件，等待超时。请稍后再试。", "Another launcher is still preparing files. Please try again later."); }
        }

        public static string Failed { get { return T("准备程序文件失败：", "Failed to prepare program files: "); } }

        public static string AvHint
        {
            get
            {
                return T("如果是被杀毒软件拦截，请把下面的目录加入信任后重试：",
                         "If an antivirus blocked it, add this folder to its exclusions and try again:");
            }
        }

        public static string LaunchFailed { get { return T("启动程序失败：", "Failed to start the program: "); } }

        public static string RepairLocked
        {
            get { return T("有文件被占用，请先关闭正在运行的程序再试。", "Some files are in use. Close the running program and try again."); }
        }

        public static string McpServerInUseTitle
        {
            get { return T("MCP Server 正在使用", "MCP Server is in use"); }
        }

        public static string McpServerInUseContent
        {
            get { return T("请先停止正在运行的 MCP Server，然后重新启动 WPE。", "Stop the running MCP Server, then start WPE again."); }
        }

        public static string McpServerInUseLog { get { return T("需要先停止 MCP Server", "stop the MCP Server first"); } }

        public static string Ok { get { return T("知道了", "OK"); } }

        public static string NoSpace
        {
            get { return T("磁盘 {0} 空间不足：需要约 {1} MB，剩余 {2} MB。", "Not enough space on {0}: about {1} MB needed, {2} MB free."); }
        }
    }
}
