using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using WinsockPacketEditor;

//代理配置的路径太长，这一段里出现十几次。静态类赋不进变量，只能用 using 别名
using ProxyCfg = WinsockPacketEditor.Operate.ProxyConfig.Proxy;

namespace WPEHybrid
{
    /// <summary>
    /// WebView2 外壳（B10a）。
    ///
    /// 窗口里只有一个铺满的 WebView2，界面全部由 Vue 绘制，
    /// 业务逻辑仍在主工程的 Operate 里，两者将来通过 WebBridge（JSON-RPC over postMessage）通信。
    ///
    /// 【前端来源】只加载内置 wwwroot，<b>不开远程加载</b>。
    /// WPEProxyCap.Hybrid 支持「远程优先 + 本地兜底」，但那是订阅制客户端；
    /// WPE x64 是本地抓包工具，依赖远程服务器会引入离线不可用与中间人风险。
    /// Debug 下例外：连 Vite dev server 以获得热更新。
    /// </summary>
    public sealed class ShellForm : Form
    {
        #region//常量

        /// <summary>本地 wwwroot 映射成的虚拟主机名。仅本 WebView2 实例内可见，外部浏览器访问不到。</summary>
        private const string VirtualHost = "app.wpe64.local";

        private const string LocalIndexUrl = "https://" + VirtualHost + "/index.html";

#if DEBUG
        /// <summary>Debug 下连 Vite dev server，前端可热更新。</summary>
        private const string DevServerUrl = "http://localhost:5173/";
#endif

        /// <summary>
        /// 允许加载的主机白名单（编译期确定）。
        /// 页面若被引导到白名单之外的主机，NavigationStarting 会拦掉
        /// —— 加载进来的页面能调用本机的强力桥方法，这道校验不能省。
        /// </summary>
        private static readonly HashSet<string> AllowedHosts = BuildAllowedHosts();

        #endregion

        #region//字段

        private readonly WebView2 web = new WebView2();

        private WebBridge bridge;

        /// <summary>WebView2 是否接管了拖动区。false 时前端回落到 startDragWindow。</summary>
        private bool nonClientOk;

        /// <summary>
        /// 队列 → 列表的搬运定时器。
        ///
        /// WinForms 那边这活儿是 ProxyModeForm.timerProxyList_Tick 干的；外壳没有那个窗体，
        /// 所以自己起一个。刻意用同样的 System.Windows.Forms.Timer 与同样的 10ms 间隔
        /// （真实精度约 15.6ms），这样两侧的性能对比才公平。
        /// 单拍搬运量由 Operate.SystemConfig.FeedBatchMax 限制，不会因为间隔小就失控。
        /// </summary>
        private readonly System.Windows.Forms.Timer timerFlush =
            new System.Windows.Forms.Timer { Interval = 10 };

        /// <summary>
        /// 每秒一次的统计刷新 + UDP 端口回收。
        ///
        /// WinForms 那边这活儿是 ProxyList.timerProxyListInfo 干的（同样 1000ms），
        /// 外壳没有那个控件，所以自己起一个。<b>不能并进 10ms 的搬运定时器</b>：
        /// 速率是「累加值 ÷ 间隔」，跟着 10ms 跑会把它算成百分之一。
        /// </summary>
        private readonly System.Windows.Forms.Timer timerStat =
            new System.Windows.Forms.Timer { Interval = 1000 };

        /// <summary>
        /// 定期把配置与各份列表写回数据库。
        ///
        /// WinForms 那边是 ProxyModeForm.timerAutoSave + bgwAutoSave（间隔取
        /// <c>Operate.SystemConfig.AutoSaveINT</c>，默认 10 分钟），外壳一直没有对应物 ——
        /// 于是白/黑名单这类「只在关窗时统一保存」的列表改完退出就没了。
        ///
        /// 间隔在 OnLoad 里按 AutoSaveINT 设，<b>不能在字段初始化时读</b>：
        /// 那会儿配置还没从库里加载出来。
        /// </summary>
        private readonly System.Windows.Forms.Timer timerAutoSave =
            new System.Windows.Forms.Timer { Interval = 600000 };

        /// <summary>自动保存正在跑（对应 WinForms 的 bgwAutoSave.IsBusy）。</summary>
        private int savingFlag;

        #endregion

        #region//环境预热

        /// <summary>Program.Main 提前发起的环境创建。null 表示没预热过。</summary>
        private static Task<CoreWebView2Environment> envTask;

        /// <summary>用户数据目录。放在程序目录下，保持「解压即用、卸载即净」。</summary>
        private static string UserDataFolder
        {
            get
            {
                return Path.Combine(
                    Path.GetDirectoryName(Application.ExecutablePath) ?? ".", "WebView2");
            }
        }

        /// <summary>
        /// 发起 WebView2 环境的创建，<b>不等它完成</b>。由 Program.Main 在建库之前调用。
        ///
        /// 创建环境要拉起一个独立的浏览器进程，是启动路径上最慢的一步；
        /// 而建库 / 读配置与它互不依赖。提前发起等于把两件慢活并行掉。
        /// </summary>
        public static void BeginCreateEnvironment()
        {
            try
            {
                if (envTask == null)
                {
                    envTask = CoreWebView2Environment.CreateAsync(null, UserDataFolder);

                    //别让它变成未观察的异常 —— 真失败了由 GetEnvironmentAsync 那边重试并报错
                    envTask.ContinueWith(
                        t => Operate.DoLog(nameof(BeginCreateEnvironment), t.Exception),
                        TaskContinuationOptions.OnlyOnFaulted);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(BeginCreateEnvironment), ex);
                envTask = null;
            }
        }

        /// <summary>
        /// 取环境。优先用预热的那个；没预热或预热失败就现建 ——
        /// 预热只是优化，不能成为启动的必要条件。
        /// </summary>
        private static async Task<CoreWebView2Environment> GetEnvironmentAsync()
        {
            if (envTask != null)
            {
                try
                {
                    return await envTask;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(GetEnvironmentAsync), ex);
                    envTask = null;
                }
            }

            return await CoreWebView2Environment.CreateAsync(null, UserDataFolder);
        }

        #endregion

        #region//运行时检测

        /// <summary>
        /// 微软官方的 WebView2 Evergreen 引导安装程序（MicrosoftEdgeWebview2Setup.exe，约 2MB，
        /// 运行后自动联网下载并装好对应架构的运行时）。这是 WebView2 文档里给应用分发用的固定链接。
        /// </summary>
        private const string WebView2SetupUrl = "https://go.microsoft.com/fwlink/p/?LinkId=2124703";

        /// <summary>WebView2 的下载页，引导安装程序打不开时的退路（离线安装包也在这一页）。</summary>
        private const string WebView2PageUrl = "https://developer.microsoft.com/microsoft-edge/webview2/";

        /// <summary>
        /// 检查 WebView2 运行时是否可用。
        ///
        /// ⚠️ <b>发布包不带运行时</b>（2.1.9 定的：用 Evergreen，不用 Fixed Version）——
        /// Win11 自带、Win10 通常随 Edge 装好；真没有时由 <see cref="PromptInstallWebView2"/>
        /// 引导用户去装，而不是把约 180MB 的运行时打进自解压包。
        /// </summary>
        public static bool HasWebView2Runtime()
        {
            try
            {
                return !string.IsNullOrEmpty(CoreWebView2Environment.GetAvailableBrowserVersionString());
            }
            catch (Exception ex)
            {
                //没装时 GetAvailableBrowserVersionString 抛 WebView2RuntimeNotFoundException，属于正常分支
                Operate.DoLog(nameof(HasWebView2Runtime), ex);
                return false;
            }
        }

        /// <summary>
        /// 缺运行时的时候告诉用户怎么装，并替他打开官方安装程序的下载地址。
        ///
        /// 这一刻库还没建、界面语言还没读出来，所以按<b>系统界面语言</b>给中文或英文。
        /// 装完要重开本程序 —— 运行时是在启动时才检测的。
        /// </summary>
        public static void PromptInstallWebView2()
        {
            bool zh = System.Globalization.CultureInfo.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase);

            string text = zh
                ? "未检测到 Microsoft Edge WebView2 运行时，WPE x64 的界面需要它才能显示。\r\n\r\n"
                  + "安装步骤：\r\n"
                  + "  1. 点「是」，下载微软官方安装程序 MicrosoftEdgeWebview2Setup.exe（约 2MB）\r\n"
                  + "  2. 运行它，按提示装完（需要联网）\r\n"
                  + "  3. 重新打开 WPE x64\r\n\r\n"
                  + "Windows 11 一般已自带；Windows 10 通常随 Microsoft Edge 一起安装。\r\n"
                  + "也可以手动到下面这个页面下载：\r\n" + WebView2PageUrl + "\r\n\r\n"
                  + "现在下载安装程序吗？"
                : "Microsoft Edge WebView2 Runtime was not found. WPE x64 needs it to show its interface.\r\n\r\n"
                  + "To install:\r\n"
                  + "  1. Click Yes to download Microsoft's installer, MicrosoftEdgeWebview2Setup.exe (about 2 MB)\r\n"
                  + "  2. Run it and follow the prompts (an internet connection is required)\r\n"
                  + "  3. Start WPE x64 again\r\n\r\n"
                  + "Windows 11 usually ships with it; on Windows 10 it normally comes with Microsoft Edge.\r\n"
                  + "You can also download it manually from:\r\n" + WebView2PageUrl + "\r\n\r\n"
                  + "Download the installer now?";

            DialogResult r = MessageBox.Show(text, "WPE x64", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (r != DialogResult.Yes)
            {
                return;
            }

            foreach (string url in new[] { WebView2SetupUrl, WebView2PageUrl })
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
                    return;
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(PromptInstallWebView2), ex);
                }
            }

            MessageBox.Show(
                (zh ? "打不开浏览器，请手动访问：\r\n" : "Could not open a browser. Please visit:\r\n") + WebView2PageUrl,
                "WPE x64", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region//窗体

        public ShellForm()
        {
            this.Text = "WPE x64";
            this.StartPosition = FormStartPosition.CenterScreen;

            /*
                默认大小与最小尺寸按「逻辑像素」给（＝100% 缩放下的像素＝页面里的 CSS 像素），
                再乘窗口将要出现的那块显示器的缩放 —— 见 FitToDpi。

                ⚠️ 2026-09-11 之前这里写死的是 1280×800 <b>设备像素</b>：这个窗体没有 AutoScaleMode，
                而清单是 PerMonitorV2，于是 125% 缩放下页面只拿到 1024×640 CSS，
                主页内容区（扣掉标题栏、状态栏、侧栏）只剩 828×564 —— 工具条折行、
                矮窗口挤压那一串问题的根源就在这儿。原 WinForms 的两个模式窗体是
                AutoScaleMode.Font 的 1450×800，会跟着缩放放大，两者在 125% 下差出一大截。

                CenterScreen 对没有 owner 的窗体取的是<b>鼠标所在</b>那块屏，这里取同一块。
            */
            System.Drawing.Point at = Cursor.Position;
            this.FitToDpi(DpiAt(at), Screen.FromPoint(at).WorkingArea, true);

            /*
                无边框：标题栏由前端自绘（官网那套四角标记 + 霓虹标题栏），
                留着系统标题栏会在它上面多压一条灰边，两套边框叠在一起很难看。

                代价是拖动与缩放要自己接：
                  拖动  → 前端在标题栏空白处按下时调 startDragWindow（见下）
                  缩放  → WndProc 里处理 WM_NCHITTEST（见 ResizeBorder）
                不接缩放的话窗口就变成固定尺寸了，而封包列表是宽表，很需要拉宽。
            */
            this.FormBorderStyle = FormBorderStyle.None;
            //与 CSS 里 body 的 --black 同值（跟着主题走，见 ShellBack）；下面 WebView2 的两处背景也用它
            this.BackColor = ShellBack();

            /*
                先隐形，等前端把首屏画完（uiReady）再露脸。

                实测前端从 HTML 到达到 Vue 挂载完成只要约 100ms，慢的是它<b>之前</b>那段：
                WebView2 要拉起浏览器进程。那段时间窗口已经显示但还是空的，
                看着就是「先弹一个空框、过一会儿才有内容」。

                隐形而不是 Visible=false：任务栏按钮照常出现，用户知道程序在启动中，
                不会以为没点着而再点一次。
                露脸的时机见 RevealWindow —— 它有超时兜底，WebView2 万一起不来也不会没窗口。
            */
            this.Opacity = 0;

            /*
                留一圈内边距给缩放。

                WebView2 是 Dock=Fill，会盖满整个客户区 —— 鼠标永远落在它的子窗口上，
                窗体的 WndProc 收不到 WM_NCHITTEST。留出这一圈，边缘才归窗体自己管。

                曾经改成「不留边距、让前端铺探测条发起缩放」，为的是让页面的四角标记贴边。
                后来换回来了：那样渲染进程一卡就既拖不动也拉不动，而且系统抢走鼠标捕获后
                Chromium 收不到 mouseup，会补派发一次 click —— 正是把程序点退出的那类问题。
                原生这条路上按下发生在窗体的 HWND，Chromium 从没接过这次按下，不存在这个状态。

                颜色与页面底色相同（ShellBack，跟着主题走），视觉上看不出这圈边；最大化时收掉。
                右 / 下两边可能多出几个像素的零头，那是把 WebView2 对齐到整数 CSS 像素用的（见 UpdateShellPadding）。
            */
            this.UpdateShellPadding();

            try
            {
                this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ShellForm), ex);
            }

            /*
                启动时那一下白闪，是 WebView2 自己的默认背景色（白）在页面画出来之前刷屏。

                启动分三个阶段，各由一处背景色负责，缺一处就会在那个阶段露白：
                  ① 窗体建好、控件还没初始化  → 控件的 WinForms BackColor
                  ② WebView2 已初始化、页面还没渲染 → DefaultBackgroundColor（默认是白色，就是它）
                  ③ 页面渲染出来之后          → CSS 里 body 的 background

                三处都刷成页面底色（深色 #0A0A0F / 浅色 #EEF1F6），整个启动过程就没有跳变了。
                DefaultBackgroundColor 必须在 EnsureCoreWebView2Async 之前设，
                控件会在创建 controller 时把它带过去。
            */
            System.Drawing.Color shellBack = ShellBack();
            this.web.BackColor = shellBack;
            this.web.DefaultBackgroundColor = shellBack;

            this.web.Dock = DockStyle.Fill;
            this.Controls.Add(this.web);

            this.Load += this.OnLoad;
        }

        private async void OnLoad(object sender, EventArgs e)
        {
            //在任何 await 之前起兜底计时器：下面这条链上任何一步卡住，窗口都还能露脸
            this.StartRevealTimeout();

            //构造时窗口还没摆到最终那块屏上，按那块屏的缩放再对齐一次 WebView2 的尺寸
            this.UpdateShellPadding();

            try
            {
                //Program.Main 早就把创建发起了，这里通常已经完成或接近完成
                CoreWebView2Environment env = await GetEnvironmentAsync();
                await this.web.EnsureCoreWebView2Async(env);

                CoreWebView2 core = this.web.CoreWebView2;

                //把本地 wwwroot 映射成虚拟主机，前端就能用普通的绝对路径引用资源
                string wwwroot = Path.Combine(
                    Path.GetDirectoryName(Application.ExecutablePath) ?? ".", "wwwroot");

                if (Directory.Exists(wwwroot))
                {
                    core.SetVirtualHostNameToFolderMapping(
                        VirtualHost, wwwroot, CoreWebView2HostResourceAccessKind.Allow);
                }

                /*
                    让 WebView2 自己处理无边框窗口的拖动区（CSS 的 app-region: drag）。

                    开这个之前，拖动是前端在 mousedown 时调 startDragWindow、C# 侧
                    SendMessage(WM_NCLBUTTONDOWN, HTCAPTION) 实现的。那个做法有两处硬伤：
                      · SendMessage 是阻塞的，整个拖动期间卡在 WebMessageReceived 处理器里，
                        WebView2 的 IPC 通道也跟着停摆；
                      · 系统把鼠标捕获抢走后，Chromium 收不到这次按下对应的 mouseup，
                        输入状态停在「左键仍按着」；拖动结束后它会在光标当前位置补派发一次
                        click —— 落在「退出」按钮上就直接把程序关了。
                    开启原生支持后由浏览器进程发起拖动，它自己知道这件事，上述两点都不存在，
                    而且双击拖动区最大化这类原生行为也一并有了。

                    需要 WebView2 Runtime 121+。更老的运行时这个 setter 会抛，
                    此时回落到旧的 startDragWindow 路径（见 nonClientOk）。
                */
                try
                {
                    core.Settings.IsNonClientRegionSupportEnabled = true;
                    this.nonClientOk = true;
                }
                catch (Exception ex)
                {
                    this.nonClientOk = false;
                    Operate.DoLog("IsNonClientRegionSupportEnabled", ex);
                }

                /*
                    关掉浏览器自带的右键菜单。

                    这一屏是在模拟 WinForms 界面，弹出「后退 / 重新加载 / 另存为图片 / 检查」
                    立刻就露馅了，而且那些命令在外壳里全都没有意义（页面是内置 wwwroot，
                    没有导航，也不该让用户另存或打印）。

                    <b>DOM 的 contextmenu 事件照常触发</b>，所以将来各列表要做自定义右键菜单
                    （WinForms 侧那 60+ 个 UserControl 里很多都有）不受影响 —— 前端监听
                    contextmenu 自己画一个即可，不必再 preventDefault。

                    代价：Debug 下右键「检查」没了。DevTools 仍可用 F12 打开
                    （AreDevToolsEnabled 默认为 true，这里没关）。
                */
                core.Settings.AreDefaultContextMenusEnabled = false;

                /*
                    顺带关掉状态栏浮层：鼠标悬到 <a> 上时 WebView2 会在左下角弹出一条
                    显示目标 URL 的浮条，正好压在我们自绘的状态栏上。
                    页脚那四个链接走的是 openExternal，不是真跳转，这条浮条纯属穿帮。
                */
                core.Settings.IsStatusBarEnabled = false;

                //拦截页面向白名单外主机的顶层跳转
                core.NavigationStarting += this.OnNavigationStarting;

                /*
                    渲染进程 / GPU 进程崩溃。

                    不订阅的话这类崩溃<b>一行日志都不会留</b> —— 浏览器进程是独立进程，
                    它死了 C# 这边的 AppDomain.UnhandledException 不会响，WER 记的也是
                    msedgewebview2.exe 而不是本程序，用户看到的只是「界面白了」或「窗口没了」。
                    2026-09-03 那次查崩溃就是卡在这儿：日志、事件查看器、WER 三处皆无。

                    Kind 为 BrowserProcessExited 时整个 WebView2 已经不可用，页面回不来了，
                    此时只记日志不重载 —— 重载会再抛一次，把真正的原因盖掉。
                */
                core.ProcessFailed += this.OnWebProcessFailed;

                //B10b：架起桥，UI 出口接到桥上；文案直接查 ClassObject/L10n 的表
                this.bridge = new WebBridge(core, AllowedHosts);
                this.RegisterMethods();

                UI.Attach(new BridgeUiHost(this.bridge, this), new CoreL10n());

                //B10c：数据出口换成桥版本。NeedsRows 一返回 true，
                //B9c 埋在 Operate.FlushToFeed 里的 DTO 构造与推送立刻生效
                UI.AttachFeed(new BridgeUiFeed(this.bridge));

                //B9d：14 份中低频列表靠订阅 ListChanged 推送。
                //必须在 AttachFeed 之后 —— FeedPump.Attach 会先看 UI.Feed.NeedsRows
                FeedPump.Attach();

                //Operate 里的跨线程回 UI 用它；WinForms 两个模式窗体也是这么设的
                Operate.SystemConfig.InvokeAction = action =>
                {
                    if (this.InvokeRequired) { this.Invoke(action); }
                    else { action(); }
                };

                this.timerFlush.Tick += this.OnFlushTick;
                this.timerFlush.Start();

                this.timerStat.Tick += this.OnStatTick;
                this.timerStat.Start();

                //与 WinForms 同一个配置项，同一个默认值（10 分钟）
                if (Operate.SystemConfig.AutoSaveINT > 0)
                {
                    this.timerAutoSave.Interval = Operate.SystemConfig.AutoSaveINT;
                }

                this.timerAutoSave.Tick += this.OnAutoSaveTick;
                this.timerAutoSave.Start();

#if DEBUG
                //Vite 没起的时候回落到内置 wwwroot，否则 F5 直接白屏。
                //探测放在这里而不是让用户记着先开 dev server —— 忘一次就得排查一次"页面空白"。
                core.Navigate(IsDevServerUp() ? DevServerUrl : LocalIndexUrl + BuildStamp());
#else
                core.Navigate(LocalIndexUrl + BuildStamp());
#endif
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnLoad), ex);

                //初始化挂了就别等超时了，立刻露脸 —— 否则错误框会弹在一个隐形窗口前面
                this.RevealWindow();

                MessageBox.Show("WebView2 初始化失败：\r\n\r\n" + ex.Message,
                    "WPE x64", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

#if DEBUG
        /// <summary>
        /// Vite dev server 在不在。只连一下 TCP，不发 HTTP —— 目的只是判断端口有没有人听。
        /// 超时给 300ms：本机连接要么立刻成功要么立刻拒绝，拖长了只是白等。
        /// </summary>
        private static bool IsDevServerUp()
        {
            try
            {
                var u = new Uri(DevServerUrl);

                /*
                    <b>不要写成 using + Wait(300)</b>：超时后 using 会立刻释放 TcpClient，
                    而连接仍在进行，回调随后对已释放的 socket 调 EndConnect 抛 NRE，
                    落到一个没人观察的 Task 上 —— 终结器线程把它报成 UnobservedTaskException。
                    进程死不了，但每次启动都会在日志里留一条「!! CRASH !!」，
                    真崩溃的那条就淹在里面了（查过一次，62 条全是它）。
                    所以这里显式观察掉这个 Task，并把释放推迟到它真正结束之后。
                */
                var c = new System.Net.Sockets.TcpClient();
                var t = c.ConnectAsync(u.Host, u.Port);

                bool done;
                try { done = t.Wait(300); }
                catch { done = true; }   //连接被拒：任务已结束，Wait 会把异常抛出来

                if (done)
                {
                    //已尘埃落定，此时释放是安全的
                    GC.KeepAlive(t.Exception);   //读一下就算「已观察」，GC 便不会再报
                    bool up = c.Connected;
                    try { c.Close(); } catch { }
                    return up;
                }

                //超时：连接还在飞，挂个后继等它自己结束再释放，顺手把异常读掉
                t.ContinueWith(x => { GC.KeepAlive(x.Exception); try { c.Close(); } catch { } });
                return false;
            }
            catch
            {
                //拒绝连接 / 解析不了，都算没起
                return false;
            }
        }

#endif
        /// <summary>最大化时收掉缩放边距，并通知前端换图标。</summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.UpdateShellPadding();

            //标题栏上的最大化/还原图标要跟着变
            if (this.bridge != null)
            {
                this.bridge.PushEvent("window:state", new
                {
                    maximized = this.WindowState == FormWindowState.Maximized,
                });
            }
        }

        /// <summary>
        /// 窗体底色，与 CSS 的 --black 同值：深色 #0A0A0F / 浅色 #EEF1F6（见 tokens.css）。
        ///
        /// 窗体四周那圈缩放内边距（ResizeBorder）露出来的就是它 —— 写死成深色的话，
        /// 浅色主题下窗口四周就挂着一道黑框。
        /// </summary>
        private static System.Drawing.Color ShellBack()
        {
            return UI.Prefs.IsDark
                ? System.Drawing.Color.FromArgb(0x0A, 0x0A, 0x0F)
                : System.Drawing.Color.FromArgb(0xEE, 0xF1, 0xF6);
        }

        /// <summary>主题变了（或切库 / 导入备份把 IsDark 换了）之后，把窗体与 WebView2 的底色跟上。</summary>
        private void ApplyShellBack()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(this.ApplyShellBack));
                return;
            }

            try
            {
                System.Drawing.Color back = ShellBack();
                this.BackColor = back;
                this.web.BackColor = back;
                this.web.DefaultBackgroundColor = back;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ApplyShellBack), ex);
            }
        }

        /// <summary>
        /// 定窗体内边距：普通状态留 ResizeBorder 那一圈给缩放，最大化时收掉；
        /// 另外把 WebView2 的宽高<b>对齐到整数个 CSS 像素</b>。
        ///
        /// ⚠️ 为什么要对齐：WebView2 的 CSS 视口 ＝ 控件的设备像素 ÷ 缩放比，而 Chromium 把它<b>向上</b>取整。
        /// 125% 下默认窗口 1600×1000 扣掉两边各 3px 是 1594×994，÷1.25 ＝ 1275.2×795.2，
        /// 于是页面按 1276×796 排版、却只显示得下 1594×994 —— <b>右边与下边各被裁掉 1 个设备像素</b>。
        /// 表现是四角标记里右上 / 左下 / 右下那三个的右边或下边贴着窗口边，只有左上那个两边都留着缝。
        ///
        /// 所以让 WebView2 的宽高是「一个 CSS 像素对应整数个设备像素」的倍数：
        /// 缩放比 dpi/96 化成最简分数后，分母就是这个步长（120→5、144→3、168→7、192→2、96→1）。
        /// 除不尽的那几个像素拆到两侧的内边距里（左 / 上拿一半，右 / 下拿另一半），
        /// 两侧最多差 1 个设备像素。
        ///
        /// 最大化时不对齐：那时四周本来就不留边，拆出来的零头会在屏幕四周挂一道细边，
        /// 比右 / 下被裁掉 1 个像素更显眼。
        /// </summary>
        private void UpdateShellPadding()
        {
            Padding want;

            if (this.WindowState == FormWindowState.Maximized)
            {
                want = new Padding(0);
            }
            else
            {
                int dpi = 96;
                try
                {
                    System.Drawing.Rectangle b = this.Bounds;
                    dpi = DpiAt(new System.Drawing.Point(b.Left + b.Width / 2, b.Top + b.Height / 2));
                }
                catch { }

                int step = dpi / Gcd(dpi, 96);
                int rw = step > 1 ? Math.Max(0, this.ClientSize.Width - 2 * ResizeBorder) % step : 0;
                int rh = step > 1 ? Math.Max(0, this.ClientSize.Height - 2 * ResizeBorder) % step : 0;

                want = new Padding(
                    ResizeBorder + rw / 2,
                    ResizeBorder + rh / 2,
                    ResizeBorder + rw - rw / 2,
                    ResizeBorder + rh - rh / 2);
            }

            if (this.Padding != want)
            {
                this.Padding = want;
            }
        }

        private static int Gcd(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
            {
                int t = a % b;
                a = b;
                b = t;
            }

            return a == 0 ? 1 : a;
        }

        /// <summary>
        /// 给首页地址加一个随构建变化的查询串，避免 WebView2 拿缓存里的旧 index.html。
        ///
        /// wwwroot 是通过 SetVirtualHostNameToFolderMapping 当普通 https 资源发的，
        /// WebView2 会按 HTTP 缓存启发式缓存它。index.html 里引用的 js/css 带内容哈希，
        /// 本身是安全的；<b>但 index.html 自己没有哈希</b> —— 它一旦被缓存住，
        /// 页面就会一直指向上一版的资源名，表现成「改了前端没反应」，极难查。
        ///
        /// 取 exe 的最后写入时间：同一个构建反复启动值不变（缓存照常生效），
        /// 一重新构建就变（强制取新的）。
        /// </summary>
        private static string BuildStamp()
        {
            try
            {
                DateTime t = File.GetLastWriteTimeUtc(Application.ExecutablePath);
                return "?b=" + t.ToString("yyyyMMddHHmmss");
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(BuildStamp), ex);
                return string.Empty;
            }
        }

        #region//无边框窗口的拖动与缩放

        private const int WM_GETMINMAXINFO = 0x0024;
        private const int WM_NCHITTEST = 0x0084;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCLIENT = 1;
        private const int HTCAPTION = 2;
        private const int HTLEFT = 10, HTRIGHT = 11;
        private const int HTTOP = 12, HTTOPLEFT = 13, HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15, HTBOTTOMLEFT = 16, HTBOTTOMRIGHT = 17;

        /*
            WM_GETMINMAXINFO 的两个结构体。字段顺序不能动 —— 它们是按内存布局
            直接映射 Win32 的 POINT / MINMAXINFO 的。
        */
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public POINT Reserved;
            public POINT MaxSize;
            public POINT MaxPosition;
            public POINT MinTrackSize;
            public POINT MaxTrackSize;
        }

        /// <summary>
        /// 缩放条的厚度，同时也是窗体内边距 —— 两者必须一致：
        /// 窗体只在这一圈里收得到 WM_NCHITTEST，判定写得比它宽也没用。
        /// </summary>
        private const int ResizeBorder = 3;

        /// <summary>
        /// 沿边多长算「角」。
        ///
        /// 只按 ResizeBorder 判角的话，斜向缩放的靶子只有 3×3 像素，基本对不准。
        /// 沿着边再放宽一段，四角就成了 L 形的区域，好点多了 ——
        /// 系统自带边框也是这么做的（角比边宽）。
        /// </summary>
        private const int CornerZone = 16;

        #region//跟着显示器缩放定窗口大小

        /*
            窗口的默认大小与最小尺寸，单位是<b>逻辑像素</b>（＝100% 缩放下的设备像素＝页面里的 CSS 像素）。
            实际的设备像素是它乘所在显示器的缩放，见 FitToDpi。

            1280×800 是这套界面照着排版的那一档（启动页、数据页、选注入方式屏的
            「设计值」都是按 1280×800 CSS 量的）；1120×700 是原来的最小尺寸。
        */
        private const int LogicalWidth = 1280;
        private const int LogicalHeight = 800;
        private const int LogicalMinWidth = 1120;
        private const int LogicalMinHeight = 700;

        private const int WM_DPICHANGED = 0x02E0;
        private const uint MONITOR_DEFAULTTONEAREST = 2;
        private const int MDT_EFFECTIVE_DPI = 0;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

        //shcore 是 Windows 8.1 才有的；Win7 上这句会抛 DllNotFoundException，DpiAt 里退回系统 DPI
        [DllImport("shcore.dll")]
        private static extern int GetDpiForMonitor(IntPtr hmonitor, int dpiType, out uint dpiX, out uint dpiY);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        /// <summary>
        /// 屏幕上某一点所在显示器的 DPI（96 ＝ 100%，120 ＝ 125%，144 ＝ 150%）。
        ///
        /// ⚠️ 不能用 Graphics.FromHwnd(IntPtr.Zero).DpiX 当主路 —— 清单是 PerMonitorV2，
        /// 那个取到的是<b>系统 DPI</b>（登录时主屏的那一档），窗口开在另一块缩放不同的屏上就错了。
        /// 它只在 Win7（没有 GetDpiForMonitor）上当退路。
        ///
        /// 进程若因故没拿到 DPI 感知，GetDpiForMonitor 返回 96 —— 那时系统会整窗位图拉伸，
        /// 按 1 倍算正是对的。
        /// </summary>
        private static int DpiAt(System.Drawing.Point p)
        {
            try
            {
                IntPtr mon = MonitorFromPoint(new POINT { X = p.X, Y = p.Y }, MONITOR_DEFAULTTONEAREST);
                uint dx, dy;

                if (mon != IntPtr.Zero && GetDpiForMonitor(mon, MDT_EFFECTIVE_DPI, out dx, out dy) == 0 && dx > 0)
                {
                    return (int)dx;
                }
            }
            catch (DllNotFoundException) { }        //Win7：走下面的系统 DPI
            catch (EntryPointNotFoundException) { }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(DpiAt), ex);
            }

            try
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
                {
                    return (int)Math.Round(g.DpiX);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(DpiAt), ex);
                return 96;
            }
        }

        private static System.Drawing.Size ScaleByDpi(int width, int height, int dpi)
        {
            return new System.Drawing.Size(
                (int)Math.Round(width * dpi / 96.0),
                (int)Math.Round(height * dpi / 96.0));
        }

        /// <summary>
        /// 最小尺寸：逻辑 1120×700 乘缩放，但<b>不超过这块屏工作区的 80%</b>。
        ///
        /// 不封顶的话，1920×1080 的屏开 150% 缩放时最小尺寸是 1680×1050，比工作区（1920×1040）还高 ——
        /// 窗口塞不进屏幕，而且一点都缩不了。80% 让小屏上始终留得出往下拖的余地；
        /// 那时页面会落到 ≤690 / ≤590 那两档矮窗口排版，它们本来就是为这种情形写的。
        /// </summary>
        private static System.Drawing.Size MinSizeFor(int dpi, System.Drawing.Rectangle work)
        {
            System.Drawing.Size min = ScaleByDpi(LogicalMinWidth, LogicalMinHeight, dpi);

            return new System.Drawing.Size(
                Math.Min(min.Width, work.Width * 4 / 5),
                Math.Min(min.Height, work.Height * 4 / 5));
        }

        /// <summary>
        /// 按 DPI 定最小尺寸；setClientSize 为 true 时连默认大小一起定（只在构造时）。
        ///
        /// 默认大小是逻辑 1280×800 乘缩放，<b>超出工作区就收到工作区那么大</b> ——
        /// 1080p 屏开 150% 时逻辑上只有 1280×693 可用，放不下 1280×800 的界面，铺满工作区是唯一合理的样子。
        /// 无边框窗口的 Size 就是 ClientSize（缩放用的那圈 Padding 在客户区里面），所以两个值可以直接比。
        /// </summary>
        private void FitToDpi(int dpi, System.Drawing.Rectangle work, bool setClientSize)
        {
            if (setClientSize)
            {
                System.Drawing.Size want = ScaleByDpi(LogicalWidth, LogicalHeight, dpi);
                this.ClientSize = new System.Drawing.Size(
                    Math.Min(want.Width, work.Width),
                    Math.Min(want.Height, work.Height));
            }

            this.MinimumSize = MinSizeFor(dpi, work);
        }

        /// <summary>
        /// 窗口被拖到缩放不同的另一块屏上（或那块屏的缩放被改了）。
        ///
        /// 清单是 PerMonitorV2，系统只<b>通知</b>、不替我们改大小：WinForms 在 .NET Framework 上
        /// 只有 app.config 里显式开了 DpiAwareness 才会处理这条消息，而本工程没有那份配置。
        /// 不接的话窗口保持原来的设备像素，页面的 CSS 尺寸就跟着变 —— 从 100% 的屏拖到 150% 的屏，
        /// 界面当场缩成三分之二。
        ///
        /// lParam 是系统建议的新窗口矩形（已按新旧缩放之比换算好），照它摆即可。
        /// ⚠️ <b>最小尺寸与新矩形的先后顺序有讲究</b>：MinimumSize 会经 WM_GETMINMAXINFO
        /// 夹住 SetWindowPos —— 先设新的（往低缩放去时）会被旧的最小尺寸顶回去；
        /// 先抬高（往高缩放去时）又会让窗口在原地先胀一下。所以先放成「新旧两者逐维取小」，
        /// 摆好矩形，再设成最终值：取小那一步永远不会让窗口变大，也不会挡住新矩形。
        /// </summary>
        private void OnDpiChanged(ref Message m)
        {
            try
            {
                int dpi = (int)((long)m.WParam & 0xFFFF);
                RECT r = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
                System.Drawing.Rectangle to = System.Drawing.Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom);
                System.Drawing.Size min = MinSizeFor(dpi, Screen.FromRectangle(to).WorkingArea);

                this.MinimumSize = new System.Drawing.Size(
                    Math.Min(min.Width, this.MinimumSize.Width),
                    Math.Min(min.Height, this.MinimumSize.Height));

                //最大化时由系统管位置与大小，只更新最小尺寸（还原回来时用得上）
                if (this.WindowState == FormWindowState.Normal)
                {
                    SetWindowPos(this.Handle, IntPtr.Zero, to.Left, to.Top, to.Width, to.Height, SWP_NOZORDER | SWP_NOACTIVATE);
                }

                this.MinimumSize = min;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnDpiChanged), ex);
            }
        }

        #endregion

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 无边框窗口自己回答「鼠标在窗口的哪个部位」。
        /// 不接这个消息，窗口边缘就没有缩放光标，也拉不动 —— 变成固定尺寸窗口。
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            /*
                全局快捷键（快捷键设置里注册的那 12 个）。RegisterHotKey 挂在这个窗口的句柄上（InitHotKeys），
                按下时系统发 WM_HOTKEY 过来；派发写法照 ProxyModeForm.WndProc —— BeginInvoke 出去，
                别在消息循环里 await。
            */
            if (m.Msg == User32.WM_HOTKEY)
            {
                int hotKeyId = m.WParam.ToInt32();

                BeginInvoke(new Action(() =>
                {
                    try
                    {
                        /*
                            注入模式下这些快捷键要在<b>目标进程里</b>执行 ——
                            两个执行器都在那边（SendPacket 用的是目标的套接字句柄）。
                            在外壳里跑 DoHotKey 只会拿外壳自己的套接字去发，什么也发不出去。
                        */
                        if (this.DispatchHotKeyToTarget(hotKeyId)) { return; }

                        Operate.SystemConfig.DoHotKey(hotKeyId);
                    }
                    catch (Exception ex) { Operate.DoLog("WndProc.HotKey", ex); }
                }));
            }

            base.WndProc(ref m);

            if (m.Msg == WM_DPICHANGED && m.LParam != IntPtr.Zero)
            {
                this.OnDpiChanged(ref m);
                return;
            }

            /*
                ⚠️ <b>无边框窗口最大化会连任务栏一起盖住</b>，除非自己回答这条消息。

                FormBorderStyle.None 的窗口，Windows 默认按<b>显示器的整个 Bounds</b> 最大化 ——
                有边框的窗口不会这样，是窗口管理器替它算好了工作区。我们把标题栏画进了页面，
                也就一并接过了这份责任。

                所以这里把最大化的尺寸与位置改成当前显示器的 WorkingArea（已经扣掉任务栏）。

                ⚠️ <b>ptMaxPosition 是相对于所在显示器的，不是桌面坐标。</b>
                副屏在主屏左边时桌面坐标是负数，直接填 work.Left 会把窗口甩到另一块屏上 ——
                所以要减去 Bounds 的原点。

                ⚠️ <b>用 Screen.FromHandle 取「当前」显示器，不是 PrimaryScreen。</b>
                窗口拖到副屏再最大化，两块屏的分辨率与任务栏位置都可能不一样。
                窗口跨屏移动时系统会重新发这条消息，所以不必自己盯着。

                ⚠️ <b>改在 base.WndProc 之后</b>：让系统先把 MinTrackSize 那几项按
                WinForms 的 MinimumSize 填好，我们只覆盖最大化相关的两项。
                MaxTrackSize 刻意不动 —— 那管的是「用手拖能拖多大」，与最大化是两件事。

                （已知未覆盖：任务栏设成<b>自动隐藏</b>时，铺满工作区的窗口会让它弹不出来。
                 那要另外查 ABM_GETSTATE 并留出 1px，等真有人用自动隐藏再说。）
            */
            if (m.Msg == WM_GETMINMAXINFO && m.LParam != IntPtr.Zero)
            {
                try
                {
                    Screen screen = Screen.FromHandle(this.Handle);
                    System.Drawing.Rectangle work = screen.WorkingArea;
                    System.Drawing.Rectangle all = screen.Bounds;

                    MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));

                    mmi.MaxSize.X = work.Width;
                    mmi.MaxSize.Y = work.Height;
                    mmi.MaxPosition.X = work.Left - all.Left;
                    mmi.MaxPosition.Y = work.Top - all.Top;

                    Marshal.StructureToPtr(mmi, m.LParam, false);
                }
                catch (Exception ex)
                {
                    //填不上就退回系统的默认值：最大化会盖住任务栏，但窗口还能用
                    Operate.DoLog("WndProc.MinMaxInfo", ex);
                }

                return;
            }

            if (m.Msg != WM_NCHITTEST || (int)m.Result != HTCLIENT)
            {
                return;
            }

            if (this.WindowState != FormWindowState.Normal)
            {
                //最大化时没有可拉的边
                return;
            }

            //lParam 是屏幕坐标，转成客户区坐标再判断落在哪条边上
            int x = unchecked((short)(long)m.LParam);
            int y = unchecked((short)((long)m.LParam >> 16));
            System.Drawing.Point p = this.PointToClient(new System.Drawing.Point(x, y));

            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

            //按实际内边距判：右 / 下两边可能比 ResizeBorder 多出几个像素的对齐零头（见 UpdateShellPadding），
            //那几个像素上面没有 WebView2，不算作缩放条的话就成了点了没反应的一道缝
            Padding pad = this.Padding;
            bool left = p.X <= Math.Max(ResizeBorder, pad.Left);
            bool right = p.X >= w - Math.Max(ResizeBorder, pad.Right);
            bool top = p.Y <= Math.Max(ResizeBorder, pad.Top);
            bool bottom = p.Y >= h - Math.Max(ResizeBorder, pad.Bottom);

            //沿边放宽一段算「角」，斜向缩放才有得点（见 CornerZone）
            bool nearLeft = p.X <= CornerZone;
            bool nearRight = p.X >= w - CornerZone;
            bool nearTop = p.Y <= CornerZone;
            bool nearBottom = p.Y >= h - CornerZone;

            if ((top && nearLeft) || (left && nearTop)) { m.Result = (IntPtr)HTTOPLEFT; }
            else if ((top && nearRight) || (right && nearTop)) { m.Result = (IntPtr)HTTOPRIGHT; }
            else if ((bottom && nearLeft) || (left && nearBottom)) { m.Result = (IntPtr)HTBOTTOMLEFT; }
            else if ((bottom && nearRight) || (right && nearBottom)) { m.Result = (IntPtr)HTBOTTOMRIGHT; }
            else if (left) { m.Result = (IntPtr)HTLEFT; }
            else if (right) { m.Result = (IntPtr)HTRIGHT; }
            else if (top) { m.Result = (IntPtr)HTTOP; }
            else if (bottom) { m.Result = (IntPtr)HTBOTTOM; }
        }

        /// <summary>
        /// 让系统接管一次窗口拖动。
        ///
        /// 前端在标题栏空白处按下左键时调这个，而不是自己算鼠标位移去改 Location ——
        /// 后者在多显示器、不同 DPI、贴边吸附时行为都不对，而 HTCAPTION 是系统原生逻辑。
        /// </summary>
        private void StartDrag()
        {
            try
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HTCAPTION, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StartDrag), ex);
            }
        }

        #endregion

        /// <summary>
        /// 关窗时收尾。灌包线程是后台线程，进程退出时本来也会被干掉，
        /// 但它还在往队列里塞而搬运定时器已经停了 —— 先停掉更干净，
        /// 也避免关窗过程中 Operate 的静态状态被继续写。
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                this.timerFlush.Stop();
                this.timerStat.Stop();
                this.timerAutoSave.Stop();
                this.StopLoad();

                /*
                    ⚠️ 系统代理必须关掉再走，这是关窗收尾里唯一「不做会伤到用户」的一条。

                    它改的是注册表里的 Internet 设置，进程退出不会自动还原；留着的话整机流量
                    仍然指向已经停掉的 SOCKS5 端口，表现就是<b>关掉 WPE 之后上不了网</b>。
                    照 ProxyModeForm_FormClosing 的头三行。
                */
                if (Operate.ProxyConfig.Proxy.Enable_SystemProxy)
                {
                    Operate.ProxyConfig.Proxy.Enable_SystemProxy = false;
                    Operate.ProxyConfig.Proxy.DisableSystemProxy();
                }

                /*
                    ⚠️ 注入的目标要断开，理由与系统代理那条同级：
                    外壳没了而钩子还留在目标里，目标就一直带着 13 个钩子跑。
                    心跳超时（3 秒）也会让目标自行卸钩，但那是<b>兜底</b>；
                    正常退出就该干净地收尾，不要让用户的游戏白白多跑三秒钩子。
                */
                this.DetachInjectOnExit();

                /*
                    ⚠️ 被驱动拦截的进程要从驱动上摘掉，与系统代理那条同级：
                    外壳没了而驱动还把它们的连接转到一个已经不存在的端口上，表现是「关掉 WPE 之后目标进程断网」。
                    驱动本身不卸（卸载会重启电脑）。UDP 那头的 SOCKS5 关联一并收掉。
                */
                Operate.ProxyConfig.Proxy.ReleaseDriverProcesses();
                Operate.ProxyConfig.Proxy.CloseAllUDPProxy();

                //远程管理与启动时的 StartRemoteMGT 成对（在 EnsureProxyConfigLoaded 里）
                Operate.SystemConfig.StopRemoteMGT();

                //关窗前存一次，对应 ProxyModeForm 关闭时的那一串 Save*_ToDB
                this.SaveProxyState();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnFormClosing), ex);
            }

            base.OnFormClosing(e);
        }

        /// <summary>自动保存的节拍。跑在后台线程，对应 WinForms 的 bgwAutoSave。</summary>
        private void OnAutoSaveTick(object sender, EventArgs e)
        {
            //上一次还没跑完就跳过这一拍（等价于 bgwAutoSave.IsBusy 判断）
            if (System.Threading.Interlocked.CompareExchange(ref this.savingFlag, 1, 0) != 0)
            {
                return;
            }

            System.Threading.Tasks.Task.Run(() =>
            {
                try { this.SaveProxyState(); }
                finally { System.Threading.Interlocked.Exchange(ref this.savingFlag, 0); }
            });
        }

        /// <summary>
        /// 把配置与各份列表写回数据库 —— 逐条对应 ProxyModeForm 关闭时做的那一串。
        ///
        /// ⚠️ <b>必须先判断 proxyLoaded</b>。这些 Save*_ToDB 一律是
        /// 「DeleteTable 整表清空 + 按内存里的列表逐条 Insert」，而那 14 份列表是进代理模式时
        /// 才从库里加载的。没进过代理模式就调，等于拿一批空列表把库里的数据全洗掉。
        /// </summary>
        private void SaveProxyState()
        {
            if (!this.proxyLoaded)
            {
                return;
            }

            try
            {
                Operate.SystemConfig.SaveSystemConfig_ToDB();

                /*
                    注入模式配置：EnsureProxyConfigLoaded 加载了它，这里就该对称地写回。
                    （早先没加载，所以这一句是刻意省掉的 —— 那个理由已经不成立了。）
                    这一屏唯一会改到那张表的是列表设置的自动清理，它自己也存了一次，
                    留在这里是为了让「加载 / 保存」逐项配平，别再出现单向的项。
                */
                Operate.SystemConfig.SaveInjectMode_ToDB();

                Operate.SystemConfig.SaveProxyMode_ToDB();
                Operate.SystemConfig.SaveSystemList_ToDB();
                Operate.WareHouseConfig.List.SaveAutoStores_ToDB();
                Operate.ProxyConfig.Mapping.SaveMapLocal_ToDB();
                Operate.ProxyConfig.Mapping.SaveMapRemote_ToDB();
                Operate.ProxyConfig.Proxy.SaveWhiteList_ToDB();
                Operate.ProxyConfig.Proxy.SaveBlackList_ToDB();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(SaveProxyState), ex);
            }
        }

        /// <summary>
        /// 渲染进程崩溃的记录点。见构造 CoreWebView2 那处的注释。
        /// </summary>
        private void OnWebProcessFailed(object sender, CoreWebView2ProcessFailedEventArgs e)
        {
            try
            {
                Operate.DoLog(
                    nameof(OnWebProcessFailed),
                    string.Format(
                        "WebView2 进程失败: Kind={0} Reason={1} ExitCode={2} 描述={3} 模块={4}",
                        e.ProcessFailedKind,
                        e.Reason,
                        e.ExitCode,
                        string.IsNullOrEmpty(e.ProcessDescription) ? "(无)" : e.ProcessDescription,
                        string.IsNullOrEmpty(e.FailureSourceModulePath) ? "(无)" : e.FailureSourceModulePath));
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnWebProcessFailed), ex);
            }
        }

        /// <summary>
        /// 与 WinForms 的 timerProxyList_Tick 做同一件事：
        /// 把队列里的数据批量搬进列表，顺带触发自动清理。
        /// FlushToFeed 内部队列空时立刻返回，不必在外面判空。
        /// </summary>
        /// <summary>
        /// 每秒一拍：刷新三个统计字符串，并回收超时的 UDP 中继端口。
        ///
        /// <b>CloseUDPTimeOut 必须有人调。</b>它原先唯一的调用点就在 WinForms 的那个定时器里，
        /// 外壳缺了它，UDP 监听端口只增不减 —— 这不是显示问题，是资源泄漏。
        ///
        /// <b>RefreshAuthList 同理。</b>认证列表与每个账号的 IsOnLine 原先全靠
        /// WinForms 的客户端列表控件那个定时器维护，外壳没有那个控件 ——
        /// 不调的话客户端列表永远是空的、账号永远显示离线。
        /// </summary>
        /// <summary>上一拍发送列表在不在跑。用来补推「刚停下」那一次。</summary>
        private bool sendWasRunning;
        private bool robotWasRunning;

        private async void OnStatTick(object sender, EventArgs e)
        {
            try
            {
                Operate.ProxyConfig.Proxy.RefreshStatInfo();
                Operate.ProxyConfig.Proxy.CloseUDPTimeOut();

                /*
                    发送列表在跑时，三个计数是在后台线程上就地累加的 ——
                    就地改属性不触发 ListChanged，没人推的话界面上的数字一直不动。
                    只在跑的时候标脏，闲着的时候一分钱不花。

                    还要多推一拍「刚停下」那次：worker 是自己跑完的（发送执行完就结束），
                    停下那一刻的最终计数得让界面收到。
                */
                /*
                    ⚠️ 注入模式下执行器<b>在目标进程里</b>，外壳这边的列表 Task 永远是闲的
                    —— 直接读 IsSendListRunning 会让界面上一直显示「未在执行」，
                    连带三个计数也不会标脏。附加着的时候要问链路，那个值随目标 1 Hz 的
                    Stats 事件报上来（与执行器启停在 AttachedLink() 那里分流是同一件事）。
                */
                var statLink = this.AttachedLink();

                bool running = statLink != null
                    ? statLink.SendListRunning
                    : Operate.SendConfig.List.IsSendListRunning;

                if (running || this.sendWasRunning)
                {
                    FeedPump.MarkDirty(FeedList.Send);
                    this.bridge.PushEvent("send:running", new { running = running });
                }

                this.sendWasRunning = running;

                //机器人列表同一套：执行次数是 RobotExecute 在后台线程上就地累加的
                bool robotRunning = statLink != null
                    ? statLink.RobotListRunning
                    : Operate.RobotConfig.List.IsRobotListRunning;

                if (robotRunning || this.robotWasRunning)
                {
                    FeedPump.MarkDirty(FeedList.Robot);
                    this.bridge.PushEvent("robot:running", new { running = robotRunning });
                }

                this.robotWasRunning = robotRunning;

                //里面要查 IP 归属地，是异步的；这一拍慢一点不影响上面两个
                await Operate.ProxyConfig.Account.RefreshAuthList();
            }
            catch (Exception ex)
            {
                //async void：await 之后抛的异常不会被 WinForms 兜住，必须自己捕获
                Operate.DoLog(nameof(OnStatTick), ex);
            }
        }


        private void OnFlushTick(object sender, EventArgs e)
        {
            try
            {
                this.timerFlush.Stop();

                Operate.PacketConfig.List.FlushToFeed();
                Operate.ProxyConfig.List.FlushToFeed();
                Operate.LogConfig.List.FlushToFeed();

                //B9d：中低频列表这一拍攒下的变更。绝大多数拍是空转，只比一次 Count 判断贵
                FeedPump.FlushDirty();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnFlushTick), ex);
            }
            finally
            {
                this.timerFlush.Start();
            }
        }

        #endregion

        #region//JS 可调用的方法

        /// <summary>
        /// 登记前端能调的 C# 方法。B10b 阶段只放通性验证用的几个，
        /// 真正的业务方法（取封包字节、启停代理…）在后续批次逐个加。
        /// </summary>
        private void RegisterMethods()
        {
            //宿主信息：验证 JS → C# 这一方向通了
            this.bridge.Register("getAppInfo", args => new
            {
                app = "WPE x64",
                mode = "Proxy",
                is64Bit = Environment.Is64BitProcess,
                clr = Environment.Version.ToString(),
                os = Environment.OSVersion.VersionString,
                batchMax = Operate.SystemConfig.FeedBatchMax,
            });

            //按 Id 取封包完整字节，滤镜改写前后一并给。
            //一次往返拿两份：十六进制面板本来就要并排显示，分两次调只是多一次延迟。
            //字节流刻意不进推送流（100 条/秒 x 4KB 走 base64 是每秒 1MB 的纯浪费），
            //只在用户点行时按 Id 回来取，见 CLAUDE.md「字节流按需拉取」。
            this.bridge.Register("getPacketDetail", args =>
            {
                long id = args["id"] == null ? 0L : (long)args["id"];

                /*
                    代理模式的主列表是 ProxyInfo，不是 PacketInfo。
                    这两份列表各有一套独立的 Id 序列（各自 Interlocked 自增），
                    所以取字节时必须说清楚是哪一份 —— 不指定就按代理模式的默认走。

                    B10d 曾把前端主列表接成 FeedList.Packet（注入模式那份），
                    压测时自洽所以没暴露；真接代理客户端上来列表会是空的。
                */
                bool isProxy = args["list"] == null
                    || (int)args["list"] == (int)FeedList.Proxy;

                byte[] buf = isProxy
                    ? Operate.ProxyConfig.List.GetPacketBufferById(id)
                    : Operate.PacketConfig.List.GetPacketBufferById(id);

                byte[] raw = isProxy
                    ? Operate.ProxyConfig.List.GetRawBufferById(id)
                    : Operate.PacketConfig.List.GetRawBufferById(id);

                if (buf == null && raw == null)
                {
                    //已被自动清理掉了
                    return null;
                }

                return new
                {
                    id = id,
                    packet = buf == null ? null : Convert.ToBase64String(buf),
                    raw = raw == null ? null : Convert.ToBase64String(raw),
                    modified = !SameBytes(raw, buf),
                };
            });

            //界面偏好。颜色是用户可配的，UiPrefs 是唯一真源，前端不许写死。
            this.bridge.Register("getPrefs", args =>
            {
                UiPrefs p = UI.Prefs;

                return new
                {
                    isDark = p.IsDark,
                    themeMode = ThemeMode(),
                    scanLine = p.ScanLine,
                    language = p.Language,
                    systemColor = p.SystemColor.Hex,
                    //滤镜标记色：封包列表按 FilterAction 给行上色，与 WinForms 一致
                    filter = new
                    {
                        replace = new { fore = p.FilterReplace_ForeColor.Hex, back = p.FilterReplace_BackColor.Hex },
                        intercept = new { fore = p.FilterIntercept_ForeColor.Hex, back = p.FilterIntercept_BackColor.Hex },
                        change = new { fore = p.FilterChange_ForeColor.Hex, back = p.FilterChange_BackColor.Hex },
                        display = new { fore = p.FilterDisplay_ForeColor.Hex, back = p.FilterDisplay_BackColor.Hex },
                    },
                };
            });

            /*
                主题：深色 / 浅色 / 跟随系统。

                【用的是已有的 UI.Prefs.IsDark】SystemConfig 表的 IsDark 列，备份 XML 里也带着。
                （它是当年 WinForms 顶栏那个暗色开关留下的字段，沿用它就不必改表结构与备份格式。）

                【三态怎么存】IsDark 是 bool，装不下三态，所以拆成两个字段：
                  · IsDark            —— <b>解析后的实际主题</b>。跟随系统时存的是那一刻系统给出的值。
                  · FollowSystemTheme —— 「这个值是不是跟着系统走出来的」。
                前端每次系统主题变了都会再调一次这里，把新解析出来的 isDark 送过来。

                界面本身是 CSS 令牌在管（style.css 的 :root[data-theme="light"]），
                这里只负责存。
            */
            this.bridge.Register("setAppearance", args =>
            {
                try
                {
                    /*
                        「字段出现才改」—— 与 saveLeachSetting / saveHookSetting 同一条协议。
                        跟随系统时系统主题一变，前端只送新的 isDark，不重复送 mode。
                    */
                    if (args["mode"] != null)
                    {
                        UI.Prefs.FollowSystemTheme =
                            string.Equals((string)args["mode"], "system", StringComparison.OrdinalIgnoreCase);
                    }

                    if (args["isDark"] != null)
                    {
                        UI.Prefs.IsDark = (bool)args["isDark"];

                        //窗体四周那圈缩放内边距露的是窗体自己的底色 —— 不跟着换，浅色下就是一道黑框
                        this.ApplyShellBack();
                    }

                    //氛围层那条游走亮带
                    if (args["scan"] != null)
                    {
                        UI.Prefs.ScanLine = (bool)args["scan"];
                    }

                    Operate.SystemConfig.SaveSystemConfig_ToDB();

                    return new { ok = true, isDark = UI.Prefs.IsDark, mode = ThemeMode(), scan = UI.Prefs.ScanLine };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("setAppearance", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            //运行状态：前端用来显示队列积压、列表长度
            this.bridge.Register("getStats", args => new
            {
                //代理模式看的是 ProxyInfo 那份；封包那份留着给注入模式将来用
                queue = Operate.ProxyConfig.Queue.qProxyInfo.Count,
                list = Operate.ProxyConfig.List.lstProxyInfo.Count,
                total = Operate.PacketConfig.Packet.TotalPackets,
                //启停逻辑已搬进 Operate，这里直接问它要状态（原先是反射读 SuperSocket 的 ServerState）
                proxyRunning = Operate.ProxyConfig.Proxy.IsRunning,

                /*
                    统计条那 13 项，取值与 WinForms 的「计时器 - 更新代理统计信息」逐条对齐。

                    代理总数是六个计数<b>相加</b>算出来的，不是单独一个字段
                    （ProxyTotal_CNT 那个字段另有用途，别直接读它）。
                */
                tcpReq = Operate.ProxyConfig.Proxy.TCP_Req_CNT,
                tcpResp = Operate.ProxyConfig.Proxy.TCP_Resp_CNT,
                udpReq = Operate.ProxyConfig.Proxy.UDP_Req_CNT,
                udpResp = Operate.ProxyConfig.Proxy.UDP_Resp_CNT,
                httpReq = Operate.ProxyConfig.Proxy.HTTP_Req_CNT,
                httpResp = Operate.ProxyConfig.Proxy.HTTP_Resp_CNT,
                filterExecute = Operate.FilterConfig.Filter.FilterExecute_CNT,
                filterProxy = Operate.ProxyConfig.Proxy.FilterProxy_CNT,
                //走 Operate 暴露的 int，不直接读 ProxyServer.SessionCount —— 那会 CS0012
                tcpConn = Operate.ProxyConfig.Proxy.SessionCount,
                udpConn = Operate.ProxyConfig.List.cdProxyUDP.Count,
                /*
                    在线账号是「3/8」这种短串，直接用 C# 拼好的那份。

                    流量与速率<b>出裸数字</b>：ProxyBytesInfo / ProxySpeedInfo 是给
                    WinForms 的宽标签拼的（形如「请求 : 62.4 KB (63,915 Bytes) 响应 : …」），
                    Vue 的统计格子只有七分之一屏宽，塞进去必然溢出。
                    格式化交给前端按自己的空间做。
                */
                onlineInfo = Operate.ProxyConfig.Proxy.ProxyOnLineInfo ?? string.Empty,
                totalRequest = Operate.ProxyConfig.Proxy.Total_Request,
                totalResponse = Operate.ProxyConfig.Proxy.Total_Response,
                speedUp = Operate.ProxyConfig.Proxy.ProxySpeed_UpKBps,
                speedDown = Operate.ProxyConfig.Proxy.ProxySpeed_DownKBps,
                /*
                    ⚠️ <b>这里刻意不出 socks5Addr。</b>它要走 GetLocalIPAddress()，
                    而那是 NetworkInterface.GetAllNetworkInterfaces() —— 本机实测 <b>70ms</b>（6 张网卡）。
                    getStats 是代理数据页每 500ms 轮询一次的，等于每秒替 UI 线程背 140ms：
                    拖窗口时就是<b>每半秒卡一下</b>（2026-09-07 查出来的那个）。

                    而且这个字段<b>压根没人用</b>：前端的 socks5Addr 是 stores/runtime 里的共享 ref，
                    由 getSystemCheck（启动）/ saveProxySetting / startProxy / saveInstance 各自更新，
                    ProxyData 那个轮询从来没读过它，bridge/types.ts 的 Stats 里也没有这一项。

                    <b>要在高频路径上取本机 IP，先给 GetLocalIPAddress 加缓存再说。</b>
                */
            });

            /*
                清空封包列表。

                <c>list</c> 指定清哪一份（<c>FeedList.Proxy</c> / <c>FeedList.Packet</c>）；
                <b>不传就两份都清</b> —— 代理数据页那个「清空」一直是这么调的，
                而压测生成器往封包那份里写，留着会让「列表行数」对不上。

                注入模式必须能只清自己那份：两种模式的列表各有一套 Id 序列，
                在注入模式点「清空」把代理那份也洗掉，切回去就少了一批数据而且毫无提示。
            */
            this.bridge.Register("clearPackets", args =>
            {
                int? which = args["list"] == null ? (int?)null : (int)args["list"];

                if (which == null || which.Value == (int)FeedList.Proxy)
                {
                    Operate.ProxyConfig.Queue.ClearProxyInfoQueue();
                    Operate.ProxyConfig.List.ClearProxyInfo();
                    UI.Feed.Clear(FeedList.Proxy);
                    Operate.SystemConfig.ResetPacketCounters(false);
                }

                if (which == null || which.Value == (int)FeedList.Packet)
                {
                    Operate.PacketConfig.Queue.ClearPacketQueue();
                    Operate.PacketConfig.List.ClearPacketList();
                    UI.Feed.Clear(FeedList.Packet);

                    /*
                        ⚠️ 封包计数<b>也要发到目标</b>：注入模式下它们是在目标的钩子线程上数的
                        （见 WpeCore.OnPacket），只清外壳这份，下一拍 Stats 就会把清空前的数盖回来。
                    */
                    ResetCountsEverywhere(WinsockPacketEditor.Ipc.ResetWhat.PacketCounters);
                }

                /*
                    ⚠️ <b>清空要连计数一起复位</b>，与 WinForms 的 CleanUp_ProxyListInfo /
                    CleanUp_PacketListInfo 逐条对应。外壳原来只清列表不清计数 ——
                    表现是「列表空了，统计格里的代理总数 / 流量 / 滤镜执行还举着清空前的数」，
                    而且「统计数据」页那六条进度条<b>再也没有办法归零</b>（外壳没有别的入口）。
                */
                ResetCountsEverywhere(WinsockPacketEditor.Ipc.ResetWhat.FilterStats);

                return new { ok = true };
            });

            /*
                用系统默认浏览器打开外部链接。

                只放行 http/https，且不让页面把任意字符串塞进 Process.Start ——
                那等于把「启动任意程序」的能力开给了前端。
                页面本身是内置 wwwroot，但这道校验属于纵深防御，不能省。
            */
            this.bridge.Register("openExternal", args =>
            {
                string url = args["url"] == null ? null : (string)args["url"];
                Uri u;

                if (!Uri.TryCreate(url, UriKind.Absolute, out u)
                    || (u.Scheme != Uri.UriSchemeHttp && u.Scheme != Uri.UriSchemeHttps))
                {
                    Operate.DoLog("openExternal", "已拒绝非 http(s) 链接: " + url);
                    return new { ok = false };
                }

                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = u.AbsoluteUri,
                        UseShellExecute = true,
                    });

                    return new { ok = true };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("openExternal", ex);
                    return new { ok = false };
                }
            });

            /*
                切换界面语言。

                前端有自己的一份字典（web/src/i18n.ts），页面上的字它自己就能换。
                但<b>弹窗与通知的文案来自 Operate 的 UI.T</b>（C# 侧），
                所以必须把选择推回来 —— 否则会出现「界面英文、弹窗中文」的分裂。

                UI.Prefs.Language 是唯一真源：UI.T 走的 CoreL10n 每次都现读它，
                改完这个字段文案就跟着换了，不需要再有一步「应用」。
                剩下的只是落库，下次启动跟着走。
            */
            this.bridge.Register("setLanguage", args =>
            {
                string lang = args["language"] == null ? null : (string)args["language"];

                /*
                    只认这六种，别的一律当中文 —— 配置列里不该出现脏值。

                    清单与前端的 i18n/langs.ts 是<b>同一份</b>（那边的 culture 字段），
                    与 ClassObject/L10n 的五张表也对得上。三处要一起改，
                    漏一处的表现是「界面切过去了、弹窗还是中文」。
                */
                lang = Normalize(lang);

                UI.Prefs.Language = lang;
                Operate.SystemConfig.SaveSystemConfig_ToDB();

                return new { language = lang };
            });

            /*
                进入代理模式。前端 ProxyView 挂载时调一次。

                【为什么放在这里，而不是第一次启动代理时】
                WinForms 是进 ProxyModeForm 就一次性加载全部列表（那个 Spin 遮罩里那一串），
                所以一进去账号数、滤镜、名单就都是齐的。外壳原先把加载塞在 startProxy 里，
                于是「进代理模式看到 0/0，点了开始代理才变成 0/1」—— 数据没错，时机不对。

                【为什么每次进来都要 MarkAllDirty】
                加载本身只做一次（那些 Load*_FromDB 不清表，不能重复调）。
                但前端从代理模式退回启动页再进来时会<b>重新挂载</b>，副本随组件没了，
                而这边的 BindingList 一动不动、不会触发 ListChanged，也就没人再推 ——
                界面上就是一片空。所以每次进来都强制整体推一次。
            */
            this.bridge.Register("enterProxyMode", args =>
            {
                /*
                    运行模式。WinForms 是在 ProxyModeForm 的构造函数里设的；
                    外壳没有那个窗体，之前一直是 None ——
                    远程管理 Web 服务的 /SystemInfo/GetSelectMode 就会返回空字符串。
                */
                Operate.SystemConfig.SelectMode = Operate.SystemConfig.SystemMode.Proxy;

                EnsureProxyConfigLoaded();
                FeedPump.MarkAllDirty();

                return new { ok = true };
            });

            #region//注入模式（B-IPC 阶段 1）

            /*
                注入模式在外壳里长成什么样：
                  选目标（进程列表 / 启动并注入）→ 注入 → 开始拦截 → 封包列表。

                与代理模式最大的不同是<b>数据来自另一个进程</b>：钩子与滤镜引擎留在目标里，
                封包经命名管道过来，由 ShellLink.Ingest 还原成 PacketInfo 进 cqPacketInfo。
                进队之后的下游（FlushToFeed / DTO / PacketRow / 前端 stores）与代理模式共用。
            */

            this.bridge.Register("enterInjectMode", args =>
            {
                Operate.SystemConfig.SelectMode = Operate.SystemConfig.SystemMode.Inject;

                /*
                    ⚠️ <b>这里刻意不加载配置、也不起远程管理。</b>

                    这一屏是「选注入目标」，还没进注入模式 —— 而 EnsureProxyConfigLoaded 会
                    顺手 StartRemoteMGT()，那会在用户还在挑进程时就弹一句「远程管理已启用」。
                    真正的对应关系是：

                        代理模式  ProxyView 挂载        → enterProxyMode → 加载 + 起远程管理
                        注入模式  <b>injectAttach 成功</b> → 主界面出来   → 同上

                    注入模式多了一屏「选目标」，所以这件事的入口比代理模式晚一步。
                    加载本来就是 injectAttach 的第一句（推快照给目标要用滤镜 / 发送 / 机器人
                    那三份表），这里去掉之后仍然一次都不会漏。

                    ⚠️ 选目标屏上<b>没有侧栏、也没有「设置 ▾」</b>（模板里那两样都在 v-else
                    那一支），所以推迟加载不会让谁读到一份没加载的配置 ——
                    「列表设置一保存就把 12 个拦截开关洗成全开」那个坑够不着这一屏。
                */

                /*
                    「上次注入」在 WinForms 的 ProcessList 上只是一行提示（一个进程名），
                    这里出的是<b>整条记录</b>（时间 / 方式 / 目标 / 路径 / 参数）——
                    选目标屏的注入检测块要显示，「快捷注入」要拿它重放。
                */
                return new { ok = true, lastInject = this.LastInjectInfo() };
            });

            /*
                快捷注入：照上次那条记录重放一次。

                ⚠️ 目标解析与兜底在 ResolveQuickTarget 里（进程还在不在 / 文件还在不在），
                <b>先查再注</b> —— 直接把上次那个 pid 存下来重用是错的：pid 会被系统回收，
                下次开机同一个号多半是别的进程，那就成了「注进一个毫不相干的程序」。
            */
            this.bridge.Register("injectQuick", async args =>
            {
                int pid;
                string path;
                string error;

                if (!this.ResolveQuickTarget(out pid, out path, out error))
                {
                    return new { ok = false, error = error };
                }

                return await this.AttachTarget(
                    pid,
                    path,
                    Operate.SystemConfig.LastInjectArgs,
                    Operate.SystemConfig.LastInjectMethod);
            });

            /// 进程列表。枚举几百个进程 + 读 MainModule 是几十毫秒的活，丢后台去。
            this.bridge.Register("getInjectProcessList", async args =>
            {
                return await System.Threading.Tasks.Task.Run(() =>
                    Operate.ProcessConfig.GetProcessRows());
            });

            //「启动并注入」要选一个可执行文件。浏览器给不出完整路径，同文件框一个理由。
            this.bridge.Register("pickTargetExe", async args =>
            {
                var pick = new FilePick();
                pick.Filter = UI.T("ExecutableFile", "可执行文件") + " (*.exe)|*.exe";
                string path = await UI.PickOpen(pick);
                return new { path = path };
            });

            /*
                注入。pid > -1 = 附加到已运行的进程；否则按 path 挂起启动。

                ⚠️ 挂起启动的命令行要<b>以 exe 路径本身开头</b> —— CreateProcess 会把
                lpCommandLine 的第一个 token 当 argv[0] 丢掉，只写参数的话第一个参数会凭空消失。
            */
            /*
                两条路都走 AttachTarget：
                ① injectAttach —— 用户在选目标屏挑了一个（三种方式之一）；
                ② injectQuick  —— 照上次那条记录重放。
                抽出来是因为「附加」这件事有二十多行（加载配置 / 防自注 / 挂遮罩 /
                接执行器与发包路由 / 记录这一次），复制一份必然会漂。
            */
            this.bridge.Register("injectAttach", async args =>
            {
                int pid0 = args["pid"] == null ? -1 : (int)args["pid"];
                string path0 = args["path"] == null ? null : (string)args["path"];
                string args0 = args["args"] == null ? null : (string)args["args"];

                /*
                    这次用的是哪种方式（0 选择进程 · 1 选择窗体 · 2 可执行文件）——
                    ⚠️ <b>必须由前端说</b>：方式 01 与 02 最后都是「附加到一个 pid」，
                    从参数上分辨不出来，而「快捷注入」要照原样重放，得知道当初点的是哪张卡。
                    没给就按 pid 猜一个（挂起启动只能是方式 03）。
                */
                int method0 = args["method"] == null ? (pid0 < 0 ? 2 : 0) : (int)args["method"];

                return await this.AttachTarget(pid0, path0, args0, method0);
            });

            this.bridge.Register("injectStartHook", async args =>
            {
                var link = this.injectLink;
                if (link == null) { return new { ok = false, error = UI.T("Inject.NotAttached", "还没有附加到目标") }; }

                try
                {
                    //先把 12 个拦截开关推下去，再装钩 —— 顺序反了的话装的是上一份配置
                    await System.Threading.Tasks.Task.Run(() =>
                    {
                        link.PushHookFlags();
                        link.PushFilters();
                        link.PushRuntime();
                        link.StartHook();
                    });
                }
                catch (Exception ex)
                {
                    Operate.DoLog("injectStartHook", ex);
                    return new { ok = false, error = ex.Message };
                }

                return this.InjectStatus();
            });

            this.bridge.Register("injectStopHook", async args =>
            {
                var link = this.injectLink;
                if (link == null) { return new { ok = false, error = UI.T("Inject.NotAttached", "还没有附加到目标") }; }

                try { await System.Threading.Tasks.Task.Run(() => link.StopHook()); }
                catch (Exception ex) { Operate.DoLog("injectStopHook", ex); return new { ok = false, error = ex.Message }; }

                return this.InjectStatus();
            });

            /*
                「选择窗体」：装两个低级钩子，用户在屏幕上点哪个窗口就选中哪个进程，Esc 取消。
                对应 WinForms 的 ProcessList.bSelectForm_Click。

                低级钩子要一个消息循环 —— 外壳本身就是个 WinForms 窗体，有。
                悬停信息经 inject:hover 事件推给前端（换了窗口才推一次，不是每次鼠标移动）。
            */
            this.bridge.Register("pickWindow", async args => await this.PickWindowAsync());

            this.bridge.Register("cancelPickWindow", args =>
            {
                this.FinishPickWindow(new { ok = false, cancelled = true });
                return new { ok = true };
            });

            this.bridge.Register("getInjectStatus", args => this.InjectStatus());

            /*
                注入模式的统计条 —— 对应 WinForms 的 PacketList 那条信息栏
                （10 个 WinSock 计数 + Total + Queue + 滤镜执行 / 已过滤 + 收发字节）。

                <b>不复用 getStats</b>：那一份全是代理口径（TCP_Req_CNT / SessionCount /
                proxyRunning…），两种模式的计数器根本不是同一批字段，硬塞进一个方法
                会变成一堆用不上的 0，而「0」在统计条上是会骗人的。

                这些计数由外壳侧的 ShellLink.Ingest 调 CountPacketInfo 维护
                —— 按<b>收到的</b>算，与目标侧计数的差就是环丢掉的那些，
                而那些另有「丢弃 N」在显示，不会无声消失。
            */
            this.bridge.Register("getInjectStats", args => new
            {
                queue = Operate.PacketConfig.Queue.cqPacketInfo.Count,
                list = Operate.PacketConfig.List.lstPacketInfo.Count,
                total = Operate.PacketConfig.Packet.TotalPackets,

                send = Operate.PacketConfig.Packet.Send_CNT,
                sendTo = Operate.PacketConfig.Packet.SendTo_CNT,
                recv = Operate.PacketConfig.Packet.Recv_CNT,
                recvFrom = Operate.PacketConfig.Packet.RecvFrom_CNT,
                wsaSend = Operate.PacketConfig.Packet.WSASend_CNT,
                wsaSendTo = Operate.PacketConfig.Packet.WSASendTo_CNT,
                wsaRecv = Operate.PacketConfig.Packet.WSARecv_CNT,
                wsaRecvFrom = Operate.PacketConfig.Packet.WSARecvFrom_CNT,

                //滤镜执行次数是目标报上来的（引擎在那边跑）；已过滤是外壳这边 FlushToFeed 数的
                filterExecute = Operate.FilterConfig.Filter.FilterExecute_CNT,
                filterPacket = Operate.PacketConfig.Packet.FilterPacket_CNT,

                /*
                    收发字节。WinForms 的 lSpeedInfo 显示的就是这两个累计值
                    （方法名叫 GetPacketSpeedInfo，其实与速率无关），出裸数字由前端格式化 ——
                    与 getStats 里流量那两项同一个理由：C# 拼好的宽标签塞不进统计格子。
                */
                totalSend = Operate.PacketConfig.Packet.Total_SendBytes,
                totalRecv = Operate.PacketConfig.Packet.Total_RecvBytes,
            });

            #endregion

            #region//代理服务的启停

            /*
                启停代理服务。逻辑在 Operate.ProxyConfig.Proxy，与 WinForms 侧同一份
                （搬迁之前那 240 行长在 Controls/ProxyList.cs 里，外壳复用不了，
                 所以外壳一直起不了代理）。

                配置的加载在 enterProxyMode 里已经做过了（进代理模式时）。
                这里再调一次纯粹是兜底 —— 它是幂等的，万一有人绕过界面直接调 startProxy
                也不会拿着一堆默认值去监听。
            */
            this.bridge.Register("startProxy", async args =>
            {
                EnsureProxyConfigLoaded();

                //StartProxy 是同步阻塞的（SuperSocket 的 Setup/Start 就是同步的），
                //丢到后台去，别让它卡住 UI 线程 —— UI.Busy 会顺带盖上遮罩
                bool ok = await UI.Busy(
                    UI.T("Loading", "正在加载..."),
                    () => Operate.ProxyConfig.Proxy.StartProxy());

                string socks5Addr, httpAddr;
                ProxyAddresses(out socks5Addr, out httpAddr);

                return new
                {
                    ok = ok,
                    running = Operate.ProxyConfig.Proxy.IsRunning,
                    socks5Addr = socks5Addr,
                    httpAddr = httpAddr,
                };
            });

            this.bridge.Register("stopProxy", args =>
            {
                Operate.ProxyConfig.Proxy.StopProxy();
                return new { ok = true, running = Operate.ProxyConfig.Proxy.IsRunning };
            });

            /*
                快捷面板的启停开关：按 Id 改某条规则的 IsEnable。

                改的是<b>对象的属性</b>，不动列表结构，所以 BindingList.ListChanged 不会触发
                （这些模型都没实现 INotifyPropertyChanged）—— 必须手动 PushNow，
                否则前端副本不会更新，勾选框点了没反应。这与编辑弹窗那 15 处是同一个道理。
            */
            this.bridge.Register("setListEnable", args =>
            {
                if (args["list"] == null || args["id"] == null)
                {
                    return new { ok = false };
                }

                FeedList which = (FeedList)(int)args["list"];
                string id = (string)args["id"];
                bool enable = args["enable"] != null && (bool)args["enable"];

                bool ok = false;

                switch (which)
                {
                    case FeedList.Filter:
                        ok = SetEnableById(Operate.FilterConfig.List.lstFilterInfo,
                            id, x => x.FID, (x, v) => x.IsEnable = v, enable);

                        if (ok) { Operate.FilterConfig.List.SaveFilterList_ToDB(); }

                        break;

                    case FeedList.Send:
                        ok = SetEnableById(Operate.SendConfig.List.lstSendInfo,
                            id, x => x.SID, (x, v) => x.IsEnable = v, enable);

                        if (ok) { Operate.SendConfig.List.SaveSendList_ToDB(); }

                        break;

                    case FeedList.Robot:
                        ok = SetEnableById(Operate.RobotConfig.List.lstRobotInfo,
                            id, x => x.RID, (x, v) => x.IsEnable = v, enable);

                        if (ok) { Operate.RobotConfig.List.SaveRobotList_ToDB(); }

                        break;
                }

                /*
                    <b>改完必须落库。</b>原先这里只改内存 + 推给前端，重启就回到旧值。

                    外壳没有 WinForms 那个「关窗时统一 SaveSystemList_ToDB」的时机
                    （理由见滤镜列表那一段的注释：停在启动页就退出会把列表删光），
                    所以约定是<b>每个改动动作各自存一次</b> —— 快捷面板这个开关
                    是唯一漏掉的一处。滤镜列表页那边的 SetFilterEnable_ById 一直是存的，
                    于是同一件事在两个入口下行为不一致，更难发现。
                */
                if (ok)
                {
                    FeedPump.PushNow(which);
                }

                return new { ok = ok };
            });

            #endregion

            /*
                中文国名 → 国家代码的对照表，一次性交给前端。

                国旗在 Vue 侧是 <img src="./flags/xx.png">，图片走静态资源、浏览器缓存，
                所以桥这边只需要给出「归属地字符串怎么变成国家代码」这份表。
                207 条，序列化后约 4KB，整个会话只传一次。

                <b>刻意不做成「传一个归属地、返回一个代码」的逐行接口</b> ——
                匹配是 207 条的前缀线性扫描，每秒几千行地调就是每秒上百万次字符串比较。
                前端只在渲染可见行（约 40 行）时才查，且按归属地做了记忆化。
            */
            this.bridge.Register("getCountryTable", args => CountryCodes.Table);

            #region//代理设置（对应 WinForms 的 Controls/ProxySetting）

            this.bridge.Register("getProxySetting", args =>
            {
                EnsureProxyConfigLoaded();


                //可选的监听地址：本机枚举出来的 IPv4/IPv6，与 WinForms 的下拉框同一份
                var ips = new List<string>();
                if (ProxyCfg.ProxyServerIP != null)
                {
                    foreach (IPAddress ip in ProxyCfg.ProxyServerIP)
                    {
                        ips.Add(ip.ToString());
                    }
                }

                return new
                {
                    proxyIpAuto = ProxyCfg.ProxyIP_Auto,
                    proxyIp = ProxyCfg.ProxyIP ?? string.Empty,
                    localIps = ips,
                    enableSocks5 = ProxyCfg.Enable_SOCKS5,
                    socks5Port = (int)ProxyCfg.SOCKS5_Port,
                    enableAuth = ProxyCfg.Enable_Auth,
                    maxConnection = ProxyCfg.MaxConnectionNumber,
                    enableHttp = ProxyCfg.Enable_HTTP,
                    httpPort = (int)ProxyCfg.HTTP_Port,
                    enableSystemProxy = ProxyCfg.Enable_SystemProxy,
                    //服务在跑时改端口没有意义，界面据此禁用相关输入
                    running = ProxyCfg.IsRunning,
                };
            });

            /*
                保存代理设置。

                校验放在 C# 而不是前端：这两条规则（必须启用 SOCKS5、两个端口不能相同）
                是服务能不能起来的前提，属于业务约束。前端另做一份就会有两套真相。

                <b>比 WinForms 多一步落库</b>：那边靠关窗时统一 SaveProxyMode_ToDB，
                外壳没有那个时机，不落库的话改完端口重启就白改了。
            */
            this.bridge.Register("saveProxySetting", args =>
            {
                bool enableSocks5 = args["enableSocks5"] != null && (bool)args["enableSocks5"];
                bool enableHttp = args["enableHttp"] != null && (bool)args["enableHttp"];
                int socks5Port = args["socks5Port"] == null ? 1080 : (int)args["socks5Port"];
                int httpPort = args["httpPort"] == null ? 1081 : (int)args["httpPort"];

                if (!enableSocks5)
                {
                    return new { ok = false, error = UI.T("ProxySettingsForm.ProxyType.Error", "代理类型未设置") };
                }

                if (enableHttp && socks5Port == httpPort)
                {
                    return new { ok = false, error = UI.T("ProxySettingsForm.ProxyType.Error", "SOCKS 和 HTTP 端口不能相同") };
                }

                if (socks5Port < 1 || socks5Port > 65535 || httpPort < 1 || httpPort > 65535)
                {
                    return new { ok = false, error = UI.T("ProxySettingsForm.Port.Error", "端口必须在 1 ~ 65535 之间") };
                }

                /*
                    不勾「自动检测」就必须真给出一个监听地址。

                    留空（或值坏了）时 InitProxyServer 走的是 IPAddress.TryParse 失败那一支，
                    <b>静默退回自动</b> —— TCP 听 0.0.0.0、UDP 绑 ProxyServerIP[0]，
                    与勾上「自动检测」的结果逐字相同。而界面上「自动检测」没勾、地址框空着，
                    看起来像「我指定了监听地址」，两者对不上且没有任何提示。

                    WinForms 侧撞不到：Controls/ProxySetting.InitProxyIP 里有一句
                    「SelectedValue == null 就 SelectedIndex = 0」，下拉永远有值。
                    外壳的 CyberSelect 没有那个默认，所以在这儿拦。
                */
                bool proxyIpAuto = args["proxyIpAuto"] != null && (bool)args["proxyIpAuto"];
                string proxyIp = args["proxyIp"] == null ? string.Empty : ((string)args["proxyIp"]).Trim();

                if (!proxyIpAuto && !IPAddress.TryParse(proxyIp, out IPAddress _))
                {
                    return new
                    {
                        ok = false,
                        error = UI.T("ProxySettingsForm.ProxyIP.Empty", "请选择监听地址，或勾上「自动检测」"),
                    };
                }

                try
                {

                    ProxyCfg.ProxyIP_Auto = proxyIpAuto;
                    ProxyCfg.ProxyIP = proxyIp;
                    ProxyCfg.Enable_SOCKS5 = enableSocks5;
                    ProxyCfg.SOCKS5_Port = (ushort)socks5Port;
                    ProxyCfg.Enable_Auth = args["enableAuth"] != null && (bool)args["enableAuth"];
                    ProxyCfg.MaxConnectionNumber = args["maxConnection"] == null ? 20000 : (int)args["maxConnection"];
                    ProxyCfg.Enable_HTTP = enableHttp;
                    ProxyCfg.HTTP_Port = (ushort)httpPort;

                    Operate.SystemConfig.SaveProxyMode_ToDB();

                    UI.Toast(UiIcon.Success, UI.T("ProxySettingsForm.Success", "代理设置保存成功"));

                    string socks5Addr, httpAddr;
                    ProxyAddresses(out socks5Addr, out httpAddr);

                    return new { ok = true, socks5Addr = socks5Addr, httpAddr = httpAddr };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveProxySetting", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            /*
                系统代理开关<b>立即生效</b>，不等「保存」——与 WinForms 一致。
                它改的是注册表里的 Internet 设置，不是本程序的配置，
                混在保存流程里反而容易让用户以为「没点保存就没生效」。
            */
            this.bridge.Register("setSystemProxy", args =>
            {
                bool on = args["enable"] != null && (bool)args["enable"];

                try
                {
                    Operate.ProxyConfig.Proxy.Enable_SystemProxy = on;

                    bool ok = on
                        ? Operate.ProxyConfig.Proxy.EnableSystemProxy()
                        : Operate.ProxyConfig.Proxy.DisableSystemProxy();

                    return new { ok = ok, enabled = Operate.ProxyConfig.Proxy.Enable_SystemProxy };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("setSystemProxy", ex);
                    return new { ok = false, enabled = false };
                }
            });

            //导出 HTTPS 中间人证书。文件框由 Operate 内部走 UI.PickSave，与 WinForms 同一条路径
            this.bridge.Register("exportCert", async args =>
            {
                int type = args["type"] == null ? 0 : (int)args["type"];

                //安卓那份的文件名是固定的哈希，装进系统证书目录时必须叫这个
                string name = type == 5 ? "9a7ae4b0" : "WPE64";

                await Operate.ProxyConfig.Proxy.SaveCertToFile_Dialog(type, name);
                return new { ok = true };
            });

            #endregion


            #region//列表设置（对应 WinForms 的 Controls/ListSetting）

            /*
                代理数据列表的列显隐 + 自动清理。

                【与 WinForms 的差别】那边的「列表设置」<b>只管列显隐</b>，
                自动清理是 ProxyList 工具条上的两个控件（勾选框 + 数字框）。
                外壳把这两样并进同一个弹窗 —— 它们讲的是同一件事：这张表怎么显示、留多少。
                工具条上仍然显示当前值，只是改要到这里来。

                <b>ID / 时间 / 数据三列不给关。</b>Id 是取字节的钥匙（getPacketDetail 靠它），
                时间和数据是这张表的意义所在，关掉等于把列表变成一堆地址。
                WinForms 允许关，那是历史遗留，不照搬。

                ⚠️ <b>列显隐是两套字段，按模式分流。</b>WinForms 侧
                ProxyConfig.List.IsShow_*（代理表，落 ProxyMode）与
                PacketConfig.List.IsShow_*（注入表，落 InjectMode）是各自独立的十个字段，
                两种模式各设各的。早先外壳把两边合成一套（都读写代理那份），后果有两条：
                在注入模式关掉一列会把代理那张表的同名列也关掉；而 WinForms 注入模式里
                设过的列显隐，外壳既看不到也改不了。
                这与「拦截设置 / 过滤设置没有模式分支」是同一个病根，一并按模式分流。

                自动清理相反 —— 它<b>本来就只有一套</b>（PacketConfig.List.AutoClear，
                Operate.cs 的 ProxyConfig.List.FlushToFeed 里注明「代理列表沿用封包列表的
                AutoClear 配置，这也是迁移前的写法」），所以不分模式。
            */

            /// 前端传来的 mode → 该读写哪一套列显隐。认不出来一律当代理，别写脏值。
            bool IsInjectList(Newtonsoft.Json.Linq.JObject a)
            {
                string m = a["mode"] == null ? null : (string)a["mode"];
                return string.Equals(m, "packet", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(m, "inject", StringComparison.OrdinalIgnoreCase);
            }

            this.bridge.Register("getListSetting", args => IsInjectList(args) ? (object)new
            {
                showSocket = Operate.PacketConfig.List.IsShow_PacketSocket,
                showType = Operate.PacketConfig.List.IsShow_PacketType,
                showClientAddr = Operate.PacketConfig.List.IsShow_ClientAddr,
                showClientLoc = Operate.PacketConfig.List.IsShow_ClientLocation,
                showServerAddr = Operate.PacketConfig.List.IsShow_ServerAddr,
                showServerLoc = Operate.PacketConfig.List.IsShow_ServerLocation,
                showLen = Operate.PacketConfig.List.IsShow_PacketLen,
                autoClear = Operate.PacketConfig.List.AutoClear,
                autoClearValue = (int)Operate.PacketConfig.List.AutoClear_Value,
            } : new
            {
                showSocket = Operate.ProxyConfig.List.IsShow_PacketSocket,
                showType = Operate.ProxyConfig.List.IsShow_PacketType,
                showClientAddr = Operate.ProxyConfig.List.IsShow_ClientAddr,
                showClientLoc = Operate.ProxyConfig.List.IsShow_ClientLocation,
                showServerAddr = Operate.ProxyConfig.List.IsShow_ServerAddr,
                showServerLoc = Operate.ProxyConfig.List.IsShow_ServerLocation,
                showLen = Operate.ProxyConfig.List.IsShow_PacketLen,
                autoClear = Operate.PacketConfig.List.AutoClear,
                autoClearValue = (int)Operate.PacketConfig.List.AutoClear_Value,
            });

            /*
                封包 / 代理列表的自动清理。

                ⚠️ <b>2026-09-07 从「列表设置」弹窗搬到了数据页的工具条上</b>，理由是
                「设置摆在哪儿，就代表它管哪张表」—— 日志那份一直在日志页的工具条上，
                而这一份藏在弹窗里，两个长得一样的「自动清理」谁都会以为是同一个。
                搬完之后「列表设置」回到只管列显隐，与 WinForms 那边一致。

                <b>它本来就只有一套</b>（PacketConfig.List.AutoClear，Operate.cs 的
                ProxyConfig.List.FlushToFeed 里注明「代理列表沿用封包列表的 AutoClear 配置」），
                所以<b>不按模式分流</b> —— 与列显隐那一对相反，别顺手给它加 mode。

                稀疏报文（字段出现才改），与 saveLogSetting / setAppearance 同一条协议：
                勾选框点一下就存、条数框失焦或回车才存。一律发全量的话，
                点开关会把用户正在编辑的半截数字也写进去。

                初值不用单独取：两个数据页挂载时本来就调了 getListSetting，那里带着这两项。
            */
            this.bridge.Register("saveListAutoClear", args =>
            {
                try
                {
                    if (args["autoClear"] != null)
                    {
                        Operate.PacketConfig.List.AutoClear = (bool)args["autoClear"];
                    }

                    if (args["autoClearValue"] != null)
                    {
                        int keep = (int)args["autoClearValue"];

                        if (keep < 100 || keep > 500000)
                        {
                            return new { ok = false, error = UI.T("ListSettingsForm.Range", "保留条数需在 100 ~ 500000 之间") };
                        }

                        Operate.PacketConfig.List.AutoClear_Value = keep;
                    }

                    //这两项在 InjectMode 表里（代理列表沿用同一份，所以不写 ProxyMode 表）
                    Operate.SystemConfig.SaveInjectMode_ToDB();

                    return new { ok = true };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveListAutoClear", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            this.bridge.Register("saveListSetting", args =>
            {
                bool Flag(string name)
                {
                    return args[name] != null && (bool)args[name];
                }

                try
                {
                    //按模式写对应的那一套，别串到另一种模式的表上
                    if (IsInjectList(args))
                    {
                        Operate.PacketConfig.List.IsShow_PacketSocket = Flag("showSocket");
                        Operate.PacketConfig.List.IsShow_PacketType = Flag("showType");
                        Operate.PacketConfig.List.IsShow_ClientAddr = Flag("showClientAddr");
                        Operate.PacketConfig.List.IsShow_ClientLocation = Flag("showClientLoc");
                        Operate.PacketConfig.List.IsShow_ServerAddr = Flag("showServerAddr");
                        Operate.PacketConfig.List.IsShow_ServerLocation = Flag("showServerLoc");
                        Operate.PacketConfig.List.IsShow_PacketLen = Flag("showLen");
                    }
                    else
                    {
                        Operate.ProxyConfig.List.IsShow_PacketSocket = Flag("showSocket");
                        Operate.ProxyConfig.List.IsShow_PacketType = Flag("showType");
                        Operate.ProxyConfig.List.IsShow_ClientAddr = Flag("showClientAddr");
                        Operate.ProxyConfig.List.IsShow_ClientLocation = Flag("showClientLoc");
                        Operate.ProxyConfig.List.IsShow_ServerAddr = Flag("showServerAddr");
                        Operate.ProxyConfig.List.IsShow_ServerLocation = Flag("showServerLoc");
                        Operate.ProxyConfig.List.IsShow_PacketLen = Flag("showLen");
                    }

                    /*
                        两张表都要落：代理的列显隐在 ProxyMode 表、注入的在 InjectMode 表。
                        不分模式一律两个都存 —— 它们各自是整表重写，
                        只存一个反而要判断另一个有没有被别处改过，得不偿失。
                    */
                    Operate.SystemConfig.SaveProxyMode_ToDB();
                    Operate.SystemConfig.SaveInjectMode_ToDB();
                    UI.Toast(UiIcon.Success, UI.T("ListSettingsForm.Success", "列表设置保存成功"));

                    return new { ok = true };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveListSetting", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            #endregion

            #region//客户端列表（对应 WinForms 的 Controls/ClientList）

            /*
                认证记录的右键菜单：把这个 IP 加进白名单或黑名单。

                【参数只收「关多久」，不收到期时间】前端算出一个时间戳发过来，
                就得两边约定时区与格式，而这里真正要表达的只是「1 小时 / 1 天 /
                30 天 / 永久」四选一 —— 让 C# 按自己的 DateTime.Now 去加，
                两边都少一层可能出错的转换。

                hours <= 0 表示永久：对应 WinForms 的 (IsExpiry: false, MaxDateTime)。

                ⚠️ AddToWhiteList / AddToBlackList 都是 <b>async void</b>，
                调用即返回、里面自己落库。所以这里返回的 ok 只表示"已经派出去了"，
                不代表已经写完 —— 与 WinForms 那边点完菜单立刻弹提示是同一个语义。
            */
            //选中某个客户端后，取它当前开着的连接（那棵树的叶子）
            this.bridge.Register("getClientConnections", args => new
            {
                items = Operate.ProxyConfig.Account.GetClientConnections(
                    args["ip"] == null ? null : (string)args["ip"]),
            });

            this.bridge.Register("addIpRule", args =>
            {
                try
                {
                    string ip = (args["ip"] == null ? string.Empty : (string)args["ip"]).Trim();

                    if (ip.Length == 0)
                    {
                        return new { ok = false, error = "empty ip" };
                    }

                    bool black = args["black"] != null && (bool)args["black"];
                    int hours = args["hours"] == null ? 0 : (int)args["hours"];

                    bool expiry = hours > 0;
                    DateTime until = expiry ? DateTime.Now.AddHours(hours) : Operate.SystemConfig.MaxDateTime;

                    if (black)
                    {
                        ProxyCfg.AddToBlackList(ip, expiry, until, DateTime.Now);
                    }
                    else
                    {
                        //白名单在 WinForms 里恒为永久（AddToWhiteList_ByDateTime 把入参丢了），照搬
                        ProxyCfg.AddToWhiteList(ip, false, Operate.SystemConfig.MaxDateTime, DateTime.Now);
                    }

                    return new { ok = true, error = string.Empty };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("addIpRule", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            #endregion

            #region//防火墙设置（对应 WinForms 的 Controls/FireWallSetting + FireWallRules）

            /*
                两张名单（白 / 黑）+ 总开关 + 模式 + 五条自动规则。

                【名单本身不走这几个方法】lstWhiteList / lstBlackList 已经在 FeedPump 的
                推送流里（FeedList.WhiteList / BlackList），界面直接读前端副本。
                这里只提供"改"的入口。

                【规则那五项原本是嵌套弹窗】WinForms 的「防火墙规则」是在设置窗上再开一个
                Modal。外壳里并进同一个弹窗的一段 —— 弹窗套弹窗在这套皮肤下要处理两层
                inert 与焦点，而这五项本来就属于防火墙，没必要分开。
            */
            this.bridge.Register("getFireWall", args => new
            {
                enable = ProxyCfg.EnableFireWall,
                whiteMode = ProxyCfg.WhiteListMode,
                autoWhiteAuthOk = ProxyCfg.FireWall_AutoWhiteList_AuthSuccess,
                autoBlackUnsupport = ProxyCfg.FireWall_AutoBlackList_UnSupport,
                autoBlackAuthFail = ProxyCfg.FireWall_AutoBlackList_AuthFail,
                autoBlackMinutes = ProxyCfg.FireWall_AutoBlackList_Minutes,
                autoClearExpiry = ProxyCfg.FireWall_AutoClear_Expiry,
            });

            this.bridge.Register("saveFireWall", args =>
            {
                bool Flag(string name)
                {
                    return args[name] != null && (bool)args[name];
                }

                try
                {
                    int minutes = args["autoBlackMinutes"] == null ? 30 : (int)args["autoBlackMinutes"];

                    if (minutes < 1 || minutes > 525600)
                    {
                        return new { ok = false, error = UI.T("FireWallSetting.Minutes.Range", "屏蔽时长需在 1 ~ 525600 分钟之间") };
                    }

                    ProxyCfg.EnableFireWall = Flag("enable");
                    ProxyCfg.WhiteListMode = Flag("whiteMode");
                    ProxyCfg.FireWall_AutoWhiteList_AuthSuccess = Flag("autoWhiteAuthOk");
                    ProxyCfg.FireWall_AutoBlackList_UnSupport = Flag("autoBlackUnsupport");
                    ProxyCfg.FireWall_AutoBlackList_AuthFail = Flag("autoBlackAuthFail");
                    ProxyCfg.FireWall_AutoBlackList_Minutes = minutes;
                    ProxyCfg.FireWall_AutoClear_Expiry = Flag("autoClearExpiry");

                    //这七项都在 ProxyMode 表
                    Operate.SystemConfig.SaveProxyMode_ToDB();
                    UI.Toast(UiIcon.Success, UI.T("FireWallSetting.Success", "防火墙设置保存成功"));

                    return new { ok = true, error = string.Empty };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveFireWall", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            //新增 / 改一条。oldIp 为空 = 新增
            this.bridge.Register("saveIPRule", args =>
            {
                string err = ProxyCfg.SaveIPRule(
                    args["black"] != null && (bool)args["black"],
                    args["oldIp"] == null ? null : (string)args["oldIp"],
                    args["ip"] == null ? null : (string)args["ip"],
                    args["isExpiry"] != null && (bool)args["isExpiry"],
                    args["expiry"] == null ? null : (string)args["expiry"]);

                return new { ok = string.IsNullOrEmpty(err), error = err };
            });

            //删一条。确认框由界面做，这里只管删
            /*
                删一条。确认框在 C# 侧弹（DeleteIPRule_Dialog 里 await UI.Confirm）——
                与账号删除、各列表的删除同一条路数，走的是前端自绘的 ConfirmDialog。

                前端原来用的是浏览器原生 window.confirm：能用，但那是系统画的灰色方框，
                在这套深色皮肤里像从别的程序飞过来的一块，而且它会同步阻塞整个渲染进程。
            */
            this.bridge.Register("deleteIPRule", async args => new
            {
                ok = await ProxyCfg.DeleteIPRule_Dialog(
                    args["black"] != null && (bool)args["black"],
                    args["ip"] == null ? null : (string)args["ip"]),
            });

            //导入 / 导出 / 清空（action 取 SystemConfig.ListAction）
            this.bridge.Register("ipRuleAction", async args =>
            {
                await ProxyCfg.IPRuleAction(
                    args["black"] != null && (bool)args["black"],
                    args["action"] == null ? 0 : (int)args["action"]);

                return new { ok = true };
            });

            #endregion

            #region//拦截设置（对应 WinForms 的 Controls/HookSetting）

            /*
                这一屏决定<b>哪些方向的封包会被抓</b>，是全项目最容易被误解成"程序坏了"的开关：

                    if (ProxyCfg.HookTCP_Req) { DoFilter_SOCKS_TCP(...); }   // 抓包 + 跑滤镜 + 进列表
                    else                      { psSession.TargetSocket.Send(bData); }  // 直接转发

                关掉哪个方向，那个方向就<b>既不进列表也不过滤镜</b>，而界面上没有任何痕迹。

                ⚠️ 这四个标志<b>不落库</b>：全项目只有 Operate.cs:4407 那一句
                `public static bool HookTCP_Req = true, ...` 的初始化，没有任何
                XElement / DB 列 / 读取路径。也就是说它们<b>只在本次运行内有效</b>，
                重启回到全开。拆包那三个则是落库的（ProxyMode 表）——
                同一个弹窗里两种存续方式，界面上要说清楚，否则关掉 UDP、重启、
                发现又开了，只会以为是 bug。

                【两种模式各看一页】WinForms 是 tabHookSettings 按宿主窗体选页
                （HookSetting 26–33）：注入模式那一页是 12 个 WinSock 钩子
                （WS1.1 四个、WS2.0 四个、WSA 四个），代理模式那一页是这四个方向 + 拆包。
                这里一次把 16 个都给出去，前端按 mode 挑一页显示。

                ⚠️ 保存时<b>只改前端真的送上来的那几个</b>（按「字段存不存在」判断）——
                另一组的控件在那一页上根本不存在，送 false 就是明确要求关掉，
                于是在代理模式点一次保存会把注入的 12 个钩子全关掉。
                与过滤设置的类别、以及 saveListSetting 当年那个坑是同一件事。

                【拆包】TCP 是流，一次 recv 可能收到好几个应用层包。
                UnPack_Head 是包头特征字节，UnPack_Length 是长度字段在包里的位置。
            */
            this.bridge.Register("getHookSetting", args => new
            {
                //注入模式那 12 个（落库，在 InjectMode 表）
                ws1Send = Operate.PacketConfig.Packet.HookWS1_Send,
                ws1SendTo = Operate.PacketConfig.Packet.HookWS1_SendTo,
                ws1Recv = Operate.PacketConfig.Packet.HookWS1_Recv,
                ws1RecvFrom = Operate.PacketConfig.Packet.HookWS1_RecvFrom,
                ws2Send = Operate.PacketConfig.Packet.HookWS2_Send,
                ws2SendTo = Operate.PacketConfig.Packet.HookWS2_SendTo,
                ws2Recv = Operate.PacketConfig.Packet.HookWS2_Recv,
                ws2RecvFrom = Operate.PacketConfig.Packet.HookWS2_RecvFrom,
                wsaSend = Operate.PacketConfig.Packet.HookWSA_Send,
                wsaSendTo = Operate.PacketConfig.Packet.HookWSA_SendTo,
                wsaRecv = Operate.PacketConfig.Packet.HookWSA_Recv,
                wsaRecvFrom = Operate.PacketConfig.Packet.HookWSA_RecvFrom,

                //代理模式那四个（<b>不落库</b>，只在本次运行内有效）+ 拆包（落库，ProxyMode 表）
                tcpReq = ProxyCfg.HookTCP_Req,
                tcpResp = ProxyCfg.HookTCP_Resp,
                udpReq = ProxyCfg.HookUDP_Req,
                udpResp = ProxyCfg.HookUDP_Resp,
                unpack = ProxyCfg.Enable_UnPack,
                unpackHead = ProxyCfg.UnPack_Head,
                unpackLength = ProxyCfg.UnPack_Length,
            });

            this.bridge.Register("saveHookSetting", args =>
            {
                bool Flag(string name)
                {
                    return args[name] != null && (bool)args[name];
                }

                void Take(string name, ref bool field)
                {
                    if (args[name] != null) { field = (bool)args[name]; }
                }

                try
                {
                    /*
                        注入模式那一页：12 个 WinSock 钩子。
                        没送上来的一个都不动（代理那一页根本没有这些控件）。
                    */
                    bool inject = args["ws1Send"] != null;

                    if (inject)
                    {
                        Take("ws1Send", ref Operate.PacketConfig.Packet.HookWS1_Send);
                        Take("ws1SendTo", ref Operate.PacketConfig.Packet.HookWS1_SendTo);
                        Take("ws1Recv", ref Operate.PacketConfig.Packet.HookWS1_Recv);
                        Take("ws1RecvFrom", ref Operate.PacketConfig.Packet.HookWS1_RecvFrom);
                        Take("ws2Send", ref Operate.PacketConfig.Packet.HookWS2_Send);
                        Take("ws2SendTo", ref Operate.PacketConfig.Packet.HookWS2_SendTo);
                        Take("ws2Recv", ref Operate.PacketConfig.Packet.HookWS2_Recv);
                        Take("ws2RecvFrom", ref Operate.PacketConfig.Packet.HookWS2_RecvFrom);
                        Take("wsaSend", ref Operate.PacketConfig.Packet.HookWSA_Send);
                        Take("wsaSendTo", ref Operate.PacketConfig.Packet.HookWSA_SendTo);
                        Take("wsaRecv", ref Operate.PacketConfig.Packet.HookWSA_Recv);
                        Take("wsaRecvFrom", ref Operate.PacketConfig.Packet.HookWSA_RecvFrom);

                        //这 12 个在 InjectMode 表。WinForms 靠关窗统一保存，外壳没有那个时机
                        Operate.SystemConfig.SaveInjectMode_ToDB();

                        /*
                            ⚠️ <b>还要推给目标</b> —— 钩子体在目标进程里，读的是它自己那份标志。
                            不推的话「保存成功」的提示照弹，而被关掉的方向照抓不误，
                            要重新装一遍钩子（停止 → 开始）才生效。
                        */
                        var link = this.AttachedLink();
                        if (link != null) { link.TryPush(link.PushHookFlags); }
                    }

                    //代理模式那一页：四个方向 + 拆包。同样没送就不动
                    if (args["tcpReq"] == null && !inject)
                    {
                        //两组都没送 —— 空报文，什么都不做
                        return new { ok = true, error = string.Empty };
                    }

                    if (args["tcpReq"] == null)
                    {
                        UI.Toast(UiIcon.Success, UI.T("HookSettingsForm.Success", "拦截设置保存成功"));
                        return new { ok = true, error = string.Empty };
                    }

                    bool unpack = Flag("unpack");
                    string head = (args["unpackHead"] == null ? string.Empty : (string)args["unpackHead"]).Trim();
                    string len = (args["unpackLength"] == null ? string.Empty : (string)args["unpackLength"]).Trim();

                    /*
                        勾了拆包就得两个都填对。

                        WinForms 的 CheckSetting 只判非空 —— 格式错的串照样存得进去，
                        然后 ParseHeaderBytes / ParseLengthPositions 返回 null / (-1,-1)，
                        SplitPackets 直接 return false，<b>拆包静默失效</b>。
                        与「指定包头」那几个框同一类问题，这里一并按格式卡住。
                    */
                    if (unpack)
                    {
                        //包头：空格 / 逗号 / 分号分隔，每段恰好两位十六进制（照 ParseHeaderBytes）
                        string[] parts = head.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        bool headOk = parts.Length > 0;

                        foreach (string p in parts)
                        {
                            byte b;

                            if (p.Length != 2 || !byte.TryParse(p, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b))
                            {
                                headOk = false;
                                break;
                            }
                        }

                        if (!headOk)
                        {
                            return new { ok = false, error = UI.T("HookSettingsForm.UnPack.Error", "拆包设置不正确") };
                        }

                        //长度：start-end，两个整数，start >= 0 且 end >= start（照 ParseLengthPositions）
                        string[] lp = len.Split('-');
                        int s, e2;

                        if (lp.Length != 2
                            || !int.TryParse(lp[0], out s)
                            || !int.TryParse(lp[1], out e2)
                            || s < 0 || e2 < s)
                        {
                            return new { ok = false, error = UI.T("HookSettingsForm.UnPack.Error", "拆包设置不正确") };
                        }
                    }

                    ProxyCfg.HookTCP_Req = Flag("tcpReq");
                    ProxyCfg.HookTCP_Resp = Flag("tcpResp");
                    ProxyCfg.HookUDP_Req = Flag("udpReq");
                    ProxyCfg.HookUDP_Resp = Flag("udpResp");

                    ProxyCfg.Enable_UnPack = unpack;
                    ProxyCfg.UnPack_Head = head;
                    ProxyCfg.UnPack_Length = len;

                    //这七个字段都在 ProxyMode 表
                    Operate.SystemConfig.SaveProxyMode_ToDB();
                    UI.Toast(UiIcon.Success, UI.T("HookSettingsForm.Success", "拦截设置保存成功"));

                    return new { ok = true, error = string.Empty };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveHookSetting", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            #endregion

            #region//系统设置（对应 WinForms 的 Controls/SystemSetting）

            /*
                极速模式 + 两个执行方式。

                【没有「悬浮按钮」那一项】WinForms 那边是 AntdUI 的 FormFloatButton，
                挂在窗体上给 GitHub / 官网两个快捷入口。外壳里没有这个东西
                （页脚已经有这两个链接），放一个开关在这儿会是个拨了不动的假开关。
                IsShow_FloatButton 这个配置本身照旧保留 —— 注入模式那套 UI 还在用。

                【四组配色不在这里】改到代理数据页那条图例上，点色块就地改，
                见下面的 saveActionColor。
            */
            this.bridge.Register("getSystemSetting", args => new
            {
                speedMode = Operate.SystemConfig.SpeedMode,
                listExecute = (int)Operate.SystemConfig.ListExecute,
                filterExecute = (int)Operate.FilterConfig.Filter.FilterExecute,
            });

            this.bridge.Register("saveSystemSetting", args =>
            {
                try
                {
                    Operate.SystemConfig.SpeedMode = args["speedMode"] != null && (bool)args["speedMode"];

                    Operate.SystemConfig.ListExecute = args["listExecute"] != null && (int)args["listExecute"] == 1
                        ? Operate.SystemConfig.Execute.Sequence
                        : Operate.SystemConfig.Execute.Together;

                    Operate.FilterConfig.Filter.FilterExecute = args["filterExecute"] != null && (int)args["filterExecute"] == 1
                        ? Operate.FilterConfig.Filter.Execute.Sequence
                        : Operate.FilterConfig.Filter.Execute.Priority;

                    //WinForms 靠退出时统一保存，外壳没有那个时机 —— 直接落库
                    Operate.SystemConfig.SaveSystemConfig_ToDB();

                    /*
                        这三项<b>全都在 Runtime 快照里</b>：极速模式、列表执行方式（发送 / 机器人
                        依次还是同时）、滤镜执行方式（DoFilterList 读它，而那个方法跑在目标里）。
                        不推的话在注入模式下它们是三个拨了不动的开关。
                    */
                    this.PushRuntimeToTarget();

                    UI.Toast(UiIcon.Success, UI.T("SystemSettingsForm.Success", "系统设置保存成功"));

                    return new { ok = true };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveSystemSetting", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            /*
                滤镜动作配色：<b>一次只改一组</b>。

                入口在代理数据页的图例上，点哪个色块改哪个 —— 所以这里按动作收，
                不像 saveSystemSetting 那样整表回写。整表回写的写法在这儿是有害的：
                图例弹窗手里只有这一组的值，把另外三组也发回来就意味着它得先取一遍、
                再原样送回，中间任何一次遗漏都会把用户在别处改的颜色悄悄覆盖掉。

                颜色走 "#RRGGBB" 串：RgbColor 本来就有 .Hex 与 FromRgb，
                两边不必各写一套 ARGB 位运算。解析不出来就保留原值，不要写进一个黑色。
            */
            this.bridge.Register("saveActionColor", args =>
            {
                RgbColor Parse(string s, RgbColor now)
                {
                    s = (s ?? string.Empty).Trim();

                    if (s.Length != 7 || s[0] != '#') { return now; }

                    int v;

                    if (!int.TryParse(s.Substring(1), NumberStyles.HexNumber,
                            CultureInfo.InvariantCulture, out v))
                    {
                        return now;
                    }

                    return RgbColor.FromRgb((v >> 16) & 0xFF, (v >> 8) & 0xFF, v & 0xFF);
                }

                try
                {
                    string act = args["action"] == null ? string.Empty : (string)args["action"];
                    string fore = args["fore"] == null ? null : (string)args["fore"];
                    string back = args["back"] == null ? null : (string)args["back"];

                    switch (act)
                    {
                        case "replace":
                            UI.Prefs.FilterReplace_ForeColor = Parse(fore, UI.Prefs.FilterReplace_ForeColor);
                            UI.Prefs.FilterReplace_BackColor = Parse(back, UI.Prefs.FilterReplace_BackColor);
                            break;

                        case "intercept":
                            UI.Prefs.FilterIntercept_ForeColor = Parse(fore, UI.Prefs.FilterIntercept_ForeColor);
                            UI.Prefs.FilterIntercept_BackColor = Parse(back, UI.Prefs.FilterIntercept_BackColor);
                            break;

                        case "change":
                            UI.Prefs.FilterChange_ForeColor = Parse(fore, UI.Prefs.FilterChange_ForeColor);
                            UI.Prefs.FilterChange_BackColor = Parse(back, UI.Prefs.FilterChange_BackColor);
                            break;

                        case "display":
                            UI.Prefs.FilterDisplay_ForeColor = Parse(fore, UI.Prefs.FilterDisplay_ForeColor);
                            UI.Prefs.FilterDisplay_BackColor = Parse(back, UI.Prefs.FilterDisplay_BackColor);
                            break;

                        default:
                            return new { ok = false, error = "unknown action: " + act };
                    }

                    Operate.SystemConfig.SaveSystemConfig_ToDB();

                    return new { ok = true, error = string.Empty };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveActionColor", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            #endregion

            #region//过滤设置（对应 WinForms 的 Controls/LeachSetting）

            /*
                抓包前的过滤 —— 决定哪些封包进列表。

                与「滤镜」不是一回事：滤镜是命中之后改写内容，这里只决定收不收。
                六个条件（套接字 / IP / 端口 / 包头 / 内容 / 长度）各自可开关、各带一个值，
                外加一个「类别」勾选表（FilterFunction 的 12 个 bool）。

                CheckNotShow 是「过滤方式」的两个单选：true = 命中的不显示，false = 只显示命中的。
            */
            this.bridge.Register("getLeachSetting", args =>
            {
                var f = Operate.SystemConfig.CheckType_Value;

                return new
                {
                    notShow = Operate.SystemConfig.CheckNotShow,

                    checkSocket = Operate.SystemConfig.CheckSocket,
                    socketValue = Operate.SystemConfig.CheckSocket_Value ?? string.Empty,
                    checkIP = Operate.SystemConfig.CheckIP,
                    ipValue = Operate.SystemConfig.CheckIP_Value ?? string.Empty,
                    checkPort = Operate.SystemConfig.CheckPort,
                    portValue = Operate.SystemConfig.CheckPort_Value ?? string.Empty,
                    checkHead = Operate.SystemConfig.CheckHead,
                    headValue = Operate.SystemConfig.CheckHead_Value ?? string.Empty,
                    checkData = Operate.SystemConfig.CheckData,
                    dataValue = Operate.SystemConfig.CheckData_Value ?? string.Empty,
                    checkLen = Operate.SystemConfig.CheckLen,
                    lenValue = Operate.SystemConfig.CheckLength_Value ?? string.Empty,

                    checkType = Operate.SystemConfig.CheckType,

                    /*
                        12 个类别一次全给，前端按模式决定显示哪一组
                        （WinForms 那边是 Inject / Proxy 两个页签，同一份 FilterFunction）。

                        注入那八个覆盖 WinSock 1.1 与 2.0 <b>两套</b>入口 ——
                        CheckFilterFunction_ByPacketType 的映射表把 WS1_Send / WS2_Send
                        都指向同一个 Send 标志，所以这里不需要（也不该）分成十六个。
                    */
                    send = f.Send,
                    sendTo = f.SendTo,
                    recv = f.Recv,
                    recvFrom = f.RecvFrom,
                    wsaSend = f.WSASend,
                    wsaSendTo = f.WSASendTo,
                    wsaRecv = f.WSARecv,
                    wsaRecvFrom = f.WSARecvFrom,

                    tcpReq = f.TCP_Req,
                    tcpResp = f.TCP_Resp,
                    udpReq = f.UDP_Req,
                    udpResp = f.UDP_Resp,
                };
            });

            this.bridge.Register("saveLeachSetting", args =>
            {
                bool Flag(string name)
                {
                    return args[name] != null && (bool)args[name];
                }

                string Text(string name)
                {
                    return args[name] == null ? string.Empty : ((string)args[name] ?? string.Empty).Trim();
                }

                try
                {
                    /*
                        勾了条件却没填值 = 这条规则不成立。WinForms 侧是弹一个
                        LeachSetting.Empty 的错误框；这里同样拦下，理由一样：
                        放过去的话过滤会静默地把所有包都滤掉，比报错难查得多。
                    */
                    var pairs = new[]
                    {
                        new { on = Flag("checkSocket"), val = Text("socketValue") },
                        new { on = Flag("checkIP"), val = Text("ipValue") },
                        new { on = Flag("checkPort"), val = Text("portValue") },
                        new { on = Flag("checkHead"), val = Text("headValue") },
                        new { on = Flag("checkData"), val = Text("dataValue") },
                        new { on = Flag("checkLen"), val = Text("lenValue") },
                    };

                    foreach (var p in pairs)
                    {
                        if (p.on && p.val.Length == 0)
                        {
                            return new { ok = false, error = UI.T("LeachSetting.Empty", "勾选的条件不能留空") };
                        }
                    }

                    Operate.SystemConfig.CheckNotShow = Flag("notShow");

                    Operate.SystemConfig.CheckSocket = Flag("checkSocket");
                    Operate.SystemConfig.CheckSocket_Value = Text("socketValue");
                    Operate.SystemConfig.CheckIP = Flag("checkIP");
                    Operate.SystemConfig.CheckIP_Value = Text("ipValue");
                    Operate.SystemConfig.CheckPort = Flag("checkPort");
                    Operate.SystemConfig.CheckPort_Value = Text("portValue");
                    Operate.SystemConfig.CheckHead = Flag("checkHead");
                    Operate.SystemConfig.CheckHead_Value = Text("headValue");
                    Operate.SystemConfig.CheckData = Flag("checkData");
                    Operate.SystemConfig.CheckData_Value = Text("dataValue");
                    Operate.SystemConfig.CheckLen = Flag("checkLen");
                    Operate.SystemConfig.CheckLength_Value = Text("lenValue");

                    Operate.SystemConfig.CheckType = Flag("checkType");

                    /*
                        FilterFunction 是<b>结构体</b>，改字段必须整个取出来改完再写回去 ——
                        直接 CheckType_Value.TCP_Req = x 在这里编译不过（静态字段是值类型副本）。

                        <b>只改前端真的送上来的那几个</b>：两种模式各显示一半类别
                        （WinForms 是 Inject / Proxy 两个页签），代理模式的弹窗里根本没有
                        注入那八个的控件，收不到就照原值保留 —— 否则在代理模式里点一次保存，
                        注入模式的八个类别会被一次清空（那正是 saveListSetting 当年犯过的错）。
                    */
                    var fn = Operate.SystemConfig.CheckType_Value;

                    void Take(string name, ref bool field)
                    {
                        if (args[name] != null) { field = (bool)args[name]; }
                    }

                    Take("send", ref fn.Send);
                    Take("sendTo", ref fn.SendTo);
                    Take("recv", ref fn.Recv);
                    Take("recvFrom", ref fn.RecvFrom);
                    Take("wsaSend", ref fn.WSASend);
                    Take("wsaSendTo", ref fn.WSASendTo);
                    Take("wsaRecv", ref fn.WSARecv);
                    Take("wsaRecvFrom", ref fn.WSARecvFrom);

                    Take("tcpReq", ref fn.TCP_Req);
                    Take("tcpResp", ref fn.TCP_Resp);
                    Take("udpReq", ref fn.UDP_Req);
                    Take("udpResp", ref fn.UDP_Resp);

                    Operate.SystemConfig.CheckType_Value = fn;

                    Operate.SystemConfig.SaveSystemConfig_ToDB();
                    UI.Toast(UiIcon.Success, UI.T("LeachSetting.Success", "过滤设置保存成功"));

                    return new { ok = true };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveLeachSetting", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            #endregion

            #region//账号列表（对应 WinForms 的 Controls/AccountList + AccountEdit）

            /*
                代理账号的增删改。列表本身走 B9d 的推送通道（FeedList.Account），
                这里只提供列表之外的那几个入口。

                【全部按 Id 字符串收发】
                AccountInfo 继承 AntdUI 的 NotifyProperty，外壳没有那个引用 ——
                它一出现在这边的签名上就是 CS0012。所以 Operate 侧另开了一组
                只出基础类型的入口（见 Operate.cs「账号列表 - 只出基础类型的入口」），
                这里调的是那一组。**不要**给外壳加 AntdUI 引用，也不要用反射。

                【密码不进推送流】
                AccountRow 里没有密码字段，是有意的 —— 那是一份加密串，
                每秒随整表推一遍毫无必要，也不该常驻在渲染进程的内存里。
                编辑时按 Id 单独取一次，保存时前端传明文、C# 侧加密。

                【查询与分页留在前端】
                WinForms 侧的「用户名搜索 / 过期时间范围 / 分页」都是对 lstAccountInfo
                做 LINQ，而前端本来就持有整表副本（几十到几百行）。
                再走一次桥只是把同样的过滤搬到另一侧做。
            */

            //编辑时才取这一条的密码明文。WinForms 侧 AccountEdit 也是这么做的
            this.bridge.Register("getAccountPassword", args => new
            {
                password = Operate.ProxyConfig.Account.GetAccountPassword_ById(
                    args["id"] == null ? null : (string)args["id"]),
            });

            //某个账号的登录记录 —— 对应 WinForms 的 AccountLocation 弹窗
            this.bridge.Register("getAccountLogins", args => new
            {
                rows = Operate.ProxyConfig.Account.GetAccountLogins_ById(
                    args["id"] == null ? null : (string)args["id"]),
            });

            this.bridge.Register("saveAccount", args =>
            {
                string id = args["id"] == null ? null : (string)args["id"];
                string user = args["userName"] == null ? string.Empty : ((string)args["userName"] ?? string.Empty).Trim();
                string pass = args["password"] == null ? string.Empty : ((string)args["password"] ?? string.Empty).Trim();

                bool isEnable = args["isEnable"] != null && (bool)args["isEnable"];
                bool limitLinks = args["isLimitLinks"] != null && (bool)args["isLimitLinks"];
                int links = args["limitLinks"] == null ? 0 : (int)args["limitLinks"];
                bool limitDevices = args["isLimitDevices"] != null && (bool)args["isLimitDevices"];
                int devices = args["limitDevices"] == null ? 0 : (int)args["limitDevices"];
                bool isExpiry = args["isExpiry"] != null && (bool)args["isExpiry"];
                string expiry = args["expiryTime"] == null ? string.Empty : (string)args["expiryTime"];

                try
                {
                    if (user.Length == 0)
                    {
                        return new { ok = false, error = UI.T("AccountEditForm.UserName.Empty", "请输入用户名") };
                    }

                    /*
                        新增必须给密码；改则可以留空表示「不动密码」——
                        UpdateProxyAccount_ByAccountID 对空串就是这个语义（那边有一处判空）。
                        所以编辑时能只改链接数，不必重打一遍密码。
                    */
                    if (pass.Length == 0 && string.IsNullOrEmpty(id))
                    {
                        return new { ok = false, error = UI.T("AccountEditForm.PassWord.Empty", "请输入密码") };
                    }

                    /*
                        不限制时存一个远期日期。判定只看 IsExpiry，
                        但存一个合法值能让排序和显示都不用特判 —— WinForms 那边取的是
                        dtpExpiryTime.MaxDate，同一个用意。
                    */
                    DateTime expiryTime = DateTime.Now.AddYears(100);

                    if (isExpiry && !DateTime.TryParse(expiry, out expiryTime))
                    {
                        return new { ok = false, error = UI.T("AccountEditForm.ExpiryTime", "过期时间格式不正确") };
                    }

                    string encrypted = pass.Length == 0 ? string.Empty : Operate.SystemConfig.PassWord_Encrypt(pass);

                    if (string.IsNullOrEmpty(id))
                    {
                        //用户名唯一。AddProxyAccount 内部也查一次，但它查不过只返回 false，说不出原因
                        if (Operate.ProxyConfig.Account.CheckProxyAccount_Exist(user))
                        {
                            return new { ok = false, error = UI.T("AccountEditForm.UserName.Error", "用户名已存在") };
                        }

                        Operate.ProxyConfig.Account.AddProxyAccount(
                            isEnable, user, encrypted,
                            limitLinks, links,
                            limitDevices, devices,
                            isExpiry, expiryTime);

                        //推送在 Operate 侧：新增只推这一行（Append），不是整表
                    }
                    else
                    {
                        //用户名不可改：它是登录凭据，WinForms 侧那个框在编辑时也是只读的
                        Operate.ProxyConfig.Account.UpdateProxyAccount_ByAccountID(
                            id, isEnable, encrypted,
                            limitLinks, links,
                            limitDevices, devices,
                            isExpiry, expiryTime);

                        //推送在 Operate 侧：它只推改动的那一行，不是整表
                    }

                    UI.Toast(UiIcon.Success, UI.T("AccountEditForm.Success", "账号保存成功"));
                    return new { ok = true };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveAccount", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            /*
                ⚠️ 删一条与清空全部是<b>两个入口</b>，不要再合并回一个。

                原先是一个：id 留空即清空全部。那个契约意味着「少传一个参数」
                的后果是把整张表删光 —— 前端拿到空串、序列化漏字段、将来某次重构手滑，
                都会让「删这一条」变成「删全部」。现在少了 id 就是什么都不做。
            */
            this.bridge.Register("deleteAccount", async args =>
            {
                string id = args["id"] == null ? null : (string)args["id"];

                if (string.IsNullOrEmpty(id))
                {
                    return new { ok = false };
                }

                await Operate.ProxyConfig.Account.DeleteAccount_Dialog_ById(id);
                return new { ok = true };
            });

            this.bridge.Register("clearAllAccounts", async args =>
            {
                await Operate.ProxyConfig.Account.ClearAllAccounts_Dialog();
                return new { ok = true };
            });

            //列表里的启用开关。落库与单行推送都在 Operate 侧那个方法里
            this.bridge.Register("setAccountEnable", args =>
            {
                bool ok = Operate.ProxyConfig.Account.SetAccountEnable_ById(
                    args["id"] == null ? null : (string)args["id"],
                    args["enable"] != null && (bool)args["enable"]);

                return new { ok = ok };
            });

            //导入 / 导出。两个都是 Operate 侧现成的 _Dialog，文件框走 UI.PickOpen / UI.PickSave
            this.bridge.Register("importAccounts", async args =>
            {
                await Operate.ProxyConfig.Account.LoadAccountList_Dialog();
                return new { ok = true };
            });

            this.bridge.Register("exportAccounts", async args =>
            {
                await Operate.ProxyConfig.Account.SaveAccount_Dialog(string.Empty);
                return new { ok = true };
            });

            /*
                批量创建账号 —— 对应 WinForms 的 Controls/BatchAccounts。

                三步：生成草稿（previewBatchAccounts）→ 用户在预览表里删掉不要的 →
                落库（saveBatchAccounts）。导出（exportBatchAccounts）在任一步都能用。

                【草稿存在前端，不在 C# 侧留一份】
                WinForms 那边预览表是控件自己的 BindingList，控件一关就没了。
                这里前端持有草稿数组，删行只是删数组元素；C# 侧不留状态，
                两侧就不会出现「你删了但我这边还在」的错位。
            */
            this.bridge.Register("previewBatchAccounts", args =>
            {
                int count = args["count"] == null ? 10 : (int)args["count"];
                int rule = args["rule"] == null ? 0 : (int)args["rule"];
                string prefix = args["prefix"] == null ? string.Empty : (string)args["prefix"];
                int passLen = args["passwordLength"] == null ? 6 : (int)args["passwordLength"];

                //自定义前缀这条规则下前缀不能空，否则生成出来的就是一串纯数字用户名
                if (rule == 1 && string.IsNullOrEmpty((prefix ?? string.Empty).Trim()))
                {
                    return new
                    {
                        ok = false,
                        error = UI.T("BatchAccounts.Prefix.Empty", "请输入用户名前缀"),
                        rows = new List<BatchAccountRow>(),
                    };
                }

                var rows = Operate.ProxyConfig.Account.BuildBatchAccounts(count, rule, prefix, passLen);

                /*
                    与已有账号重名的先标出来 —— 落库时 AddProxyAccount 会静默挡下，
                    等到那时才说「跳过了 3 条」，用户已经没法回头改前缀了。
                */
                var dup = new List<string>();

                foreach (BatchAccountRow r in rows)
                {
                    if (Operate.ProxyConfig.Account.CheckProxyAccount_Exist(r.UserName))
                    {
                        dup.Add(r.UserName);
                    }
                }

                return new { ok = true, error = string.Empty, rows = rows, duplicates = dup };
            });

            this.bridge.Register("saveBatchAccounts", args =>
            {
                try
                {
                    var rows = ReadBatchRows(args);

                    if (rows.Count == 0)
                    {
                        return new { ok = false, added = 0, skipped = 0, error = UI.T("BatchAccounts.Empty", "没有可保存的账号") };
                    }

                    int added = Operate.ProxyConfig.Account.AddBatchAccounts(
                        rows,
                        args["isLimitLinks"] != null && (bool)args["isLimitLinks"],
                        args["limitLinks"] == null ? 0 : (int)args["limitLinks"],
                        args["isLimitDevices"] != null && (bool)args["isLimitDevices"],
                        args["limitDevices"] == null ? 0 : (int)args["limitDevices"],
                        args["isExpiry"] != null && (bool)args["isExpiry"],
                        ReadExpiry(args));

                    //推送在 Operate 侧：整批只推一条 Append，不是整表
                    return new { ok = true, added = added, skipped = rows.Count - added, error = string.Empty };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveBatchAccounts", ex);
                    return new { ok = false, added = 0, skipped = 0, error = ex.Message };
                }
            });

            this.bridge.Register("exportBatchAccounts", async args =>
            {
                await Operate.ProxyConfig.Account.SaveBatchAccounts_Dialog(
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    ReadBatchRows(args),
                    args["isExpiry"] != null && (bool)args["isExpiry"],
                    ReadExpiry(args));

                return new { ok = true };
            });

            /*
                选中若干账号后的批量操作 —— 对应 WinForms 账号列表的右键菜单
                （批量调整 ▸ 过期时间 / 链接数 / 设备数，批量导出，批量删除）。

                选中态留在前端，这边只收 Id 数组。WinForms 那边是存在模型上的
                （AccountInfo.IsCheck），桥这边不跟 —— 选中是纯界面状态。
            */
            this.bridge.Register("adjustAccountExpiry", args =>
            {
                //小时是内部单位；「天」在前端换算好再传，C# 侧只认小时
                int hours = args["hours"] == null ? 0 : (int)args["hours"];
                int addType = args["addType"] == null ? 0 : (int)args["addType"];

                if (hours == 0)
                {
                    return new { ok = false, count = 0, error = UI.T("ExpiryTimeForm.Zero", "请输入要增加的时长") };
                }

                int n = Operate.ProxyConfig.Account.AdjustExpiryTime_ByIds(ReadIds(args), addType, hours);
                return BatchResult(n);
            });

            this.bridge.Register("adjustAccountLimit", args =>
            {
                //一个入口管链接数与设备数：两边的表单结构一模一样，分成两个只会多一份要同步的代码
                bool devices = args["devices"] != null && (bool)args["devices"];
                bool on = args["on"] != null && (bool)args["on"];
                int value = args["value"] == null ? 1 : (int)args["value"];

                if (on && value < 1)
                {
                    return new { ok = false, count = 0, error = UI.T("LimitForm.Range", "限制值至少为 1") };
                }

                int n = devices
                    ? Operate.ProxyConfig.Account.AdjustLimitDevices_ByIds(ReadIds(args), on, value)
                    : Operate.ProxyConfig.Account.AdjustLimitLinks_ByIds(ReadIds(args), on, value);

                return BatchResult(n);
            });

            this.bridge.Register("exportSelectedAccounts", async args =>
            {
                await Operate.ProxyConfig.Account.SaveAccount_Dialog_ByIds(string.Empty, ReadIds(args));
                return new { ok = true };
            });

            this.bridge.Register("deleteSelectedAccounts", async args =>
            {
                await Operate.ProxyConfig.Account.DeleteAccount_Dialog_ByIds(ReadIds(args));
                return new { ok = true };
            });

            #endregion

            #region//滤镜列表（对应 WinForms 的 Controls/FilterList）

            /*
                滤镜是「配置类列表 + 编辑弹窗」这个模式的第一份，
                发送 / 机器人 / 仓库三屏结构几乎相同 —— 那三屏接进来时，
                下面这几个方法照抄改个列表名即可，右键菜单那套更是共用的
                （Operate.SystemConfig.GetCMS_List() + ListAction 枚举）。

                【落库不能等关窗】WinForms 靠 ProxyModeForm 关窗时统一
                SaveSystemList_ToDB()，外壳没有那个时机 —— 而且不能在关窗时补一句：
                用户停在启动页就退出的话这些列表还没加载，那一句会把库里的滤镜全删光。
                所以落库都放在 Operate 侧那一组入口里，每个改动动作各自存一次。
            */

            /*
                滤镜列表的执行模式。设置项在<b>系统设置</b>（Controls/SystemSetting 的
                rbFilterSet_*），不是列表设置 —— 两屏都已完成，这里只读来显示。

                语义以 DoFilterList 为准，不是照着标签猜的：
                  Priority（优先原则）  从上到下，第一个命中的执行完就 return，后面不再跑
                  Sequence（按顺序执行）从上到下逐个匹配，命中的都跑，改写逐个叠加

                【两种模式共有的一条】命中的那条动作是 拦截 / 换包 / 只显示 / 不显示 时
                <b>立刻返回</b>，无论哪种模式 —— 也就是说 Sequence 下只有「替换」会继续往下走。
                这四个动作本质上是终结性的，再往下改没有意义。

                注意别和 SystemConfig.ListExecute 搞混：那是<b>另一个</b>同名枚举
                （Sequence / Together），管的是发送列表与机器人列表，与滤镜无关。
            */
            /*
                验一次导入密码，供密码框在<b>关窗之前</b>自检。

                没有它的话，密码打错只能等弹窗关掉、整条导入流程走完才报
                「导入失败: 密码错误」，用户得从「重新点导入 → 重新选文件」来一遍。
                有了它，前端可以留在框里让人重输 —— 这是密码框最基本的行为。

                解密一次就是验一次：DecryptXMLFile 密码不对返回 null（见它内层那个 catch）。
                配置文件都很小，重复解一次的代价可以忽略。
            */
            this.bridge.Register("verifyEncryptPassword", args =>
            {
                string path = args["path"] == null ? null : (string)args["path"];
                string pw = args["password"] == null ? null : (string)args["password"];

                if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(pw))
                {
                    return new { ok = false };
                }

                try
                {
                    return new { ok = Operate.SystemConfig.DecryptXMLFile(path, pw) != null };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("verifyEncryptPassword", ex);
                    return new { ok = false };
                }
            });

            this.bridge.Register("getFilterExecute", args => new
            {
                mode = (int)Operate.FilterConfig.Filter.FilterExecute,
            });

            /*
                滤镜编辑弹窗：取一条 / 存一条。

                【格子的存储格式不过桥】源模型把它存成 "索引|值," 这样的串（外加三串位置标记），
                解析与拼装都在 Operate 侧（GetFilterEdit_ById / SaveFilterEdit），
                前端拿到 / 交回的都是解析好的 FilterCellRow 数组。
                两边各写一份解析必然在分隔符、尾逗号、越界索引上走岔。

                【校验也在 C# 侧】与 WinForms 的 CheckFilterIsValid 同一份规则 ——
                「勾了指定却没填值」是能不能匹配上的业务约束，不是界面细节，
                前端再写一遍就有两套真相。
            */
            this.bridge.Register("getFilterEdit", args => new
            {
                row = Operate.FilterConfig.List.GetFilterEdit_ById(
                    args["id"] == null ? null : (string)args["id"]),
            });

            this.bridge.Register("saveFilterEdit", args =>
            {
                FilterEditRow row = null;

                try
                {
                    if (args["row"] != null)
                    {
                        row = args["row"].ToObject<FilterEditRow>();
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveFilterEdit", ex);
                }

                string err = Operate.FilterConfig.List.SaveFilterEdit(row);

                return new { ok = string.IsNullOrEmpty(err), error = err };
            });

            /*
                剪贴板走桥，不用前端的 navigator.clipboard。

                写入本来两边都行，但<b>读取</b>在 WebView2 里要过 ClipboardRead 权限：
                外壳没有挂 PermissionRequested，那就落到默认处理上 ——
                在一个无边框窗口里弹一个浏览器权限条，既难看又可能被拒。
                而 Clipboard 本身就是 WinForms 侧现成的东西（FilterEdit 用的就是它），
                走桥还顺带保证两套 UI 的复制粘贴是同一份行为。

                Program.Main 上有 [STAThread]，OLE 剪贴板要求的就是这个。
            */
            this.bridge.Register("clipboardRead", args =>
            {
                try
                {
                    return new { text = Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty };
                }
                catch (Exception ex)
                {
                    //剪贴板可能被别的进程占着，这时候读不到不是错误，只是这一次没读着
                    Operate.DoLog("clipboardRead", ex);
                    return new { text = string.Empty };
                }
            });

            this.bridge.Register("clipboardWrite", args =>
            {
                string s = args["text"] == null ? string.Empty : (string)args["text"];

                try
                {
                    //SetText 空串会抛，改用 Clear —— 语义上也正是「清空剪贴板」
                    if (string.IsNullOrEmpty(s)) { Clipboard.Clear(); }
                    else { Clipboard.SetText(s); }

                    return new { ok = true };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("clipboardWrite", ex);
                    return new { ok = false };
                }
            });

            //「执行」下拉的候选项。type 是 FilterExecuteType 的枚举值，不是下拉下标
            this.bridge.Register("getExecuteTargets", args => new
            {
                items = Operate.FilterConfig.List.GetExecuteTargets(
                    args["type"] == null ? 0 : (int)args["type"],
                    args["excludeId"] == null ? null : (string)args["excludeId"]),
            });

            this.bridge.Register("addFilter", args => new
            {
                id = Operate.FilterConfig.List.AddFilter_New_ById(),
            });

            this.bridge.Register("setFilterEnable", args => new
            {
                ok = Operate.FilterConfig.List.SetFilterEnable_ById(
                    args["id"] == null ? null : (string)args["id"],
                    args["enable"] != null && (bool)args["enable"]),
            });

            //工具条的「全部启用 / 全部禁用」
            this.bridge.Register("setAllFilterEnable", args =>
            {
                int n = Operate.FilterConfig.List.SetAllFilterEnable(
                    args["enable"] != null && (bool)args["enable"]);

                return new { ok = true, count = n };
            });

            //工具条的「重置计数」。运行期计数，不落库
            this.bridge.Register("resetFilterCount", args =>
            {
                Operate.FilterConfig.List.ResetFilterCount();
                return new { ok = true };
            });

            /*
                右键菜单的七个动作。action 是 Operate.SystemConfig.ListAction 的序号：
                0 置顶 · 1 上移 · 2 下移 · 3 置底 · 4 复制 · 5 导出 · 6 删除。

                前端不自己算顺序 —— 移动的语义（多选时逐个移、边界怎么办）在
                UpdateFilterList_ByListAction 里，两套 UI 得是同一份。
            */
            this.bridge.Register("filterListAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var ids = ReadIds(args);

                if (action < 0 || ids.Count == 0)
                {
                    return new { ok = false, delta = 0 };
                }

                int delta = await Operate.FilterConfig.List.FilterListAction_ByIds(action, ids);
                return new { ok = true, delta = delta };
            });

            this.bridge.Register("importFilters", async args =>
            {
                await Operate.FilterConfig.List.LoadFilterList_Dialog_Shell();
                return new { ok = true };
            });

            this.bridge.Register("exportFilters", async args =>
            {
                await Operate.FilterConfig.List.SaveAllFilters_Dialog();
                return new { ok = true };
            });

            this.bridge.Register("clearFilters", async args =>
            {
                await Operate.FilterConfig.List.CleanUpFilterList_Dialog_Shell();
                return new { ok = true };
            });

            #endregion

            #region//发送列表（对应 WinForms 的 Controls/SendList）

            /*
                「套接字」那一列在 UseSystemSocket 为真时显示的是<b>全局系统套接字号</b>
                （WinForms 侧就是 Operate.SystemConfig.SystemSocket），为假时显示「自定义」。
                它是个 public static int，外壳直接读得到，不必绕 DTO。
            */
            this.bridge.Register("getSendMeta", args => new
            {
                systemSocket = Operate.SystemConfig.SystemSocket,
                running = Operate.SendConfig.List.IsSendListRunning,

                //列表页顶上那条「执行顺序」说明条要照实说是哪种模式。
                //0 = Together（同时）/ 1 = Sequence（按顺序）—— 与滤镜那个同名枚举<b>反着</b>，别对调。
                //发送与机器人共用这一个开关，所以 getRobotMeta 里也有一份。
                listExecute = (int)Operate.SystemConfig.ListExecute,
            });

            this.bridge.Register("addSend", args => new
            {
                id = Operate.SendConfig.List.AddSend_New_ById(),
            });

            this.bridge.Register("setSendEnable", args => new
            {
                ok = Operate.SendConfig.List.SetSendEnable_ById(
                    args["id"] == null ? null : (string)args["id"],
                    args["enable"] != null && (bool)args["enable"]),
            });

            this.bridge.Register("setAllSendEnable", args =>
            {
                int n = Operate.SendConfig.List.SetAllSendEnable(
                    args["enable"] != null && (bool)args["enable"]);

                return new { ok = true, count = n };
            });

            //运行期计数，不落库
            this.bridge.Register("resetSendCount", args =>
            {
                //⚠️ 注入模式下执行次数在目标里累加，只清外壳那份会被下一拍 Stats 盖回来
                ResetCountsEverywhere(WinsockPacketEditor.Ipc.ResetWhat.SendCounts);
                return new { ok = true };
            });

            //右键菜单的七个动作，编号同滤镜：0 置顶 · 1 上移 · 2 下移 · 3 置底 · 4 复制 · 5 导出 · 6 删除
            this.bridge.Register("sendListAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var ids = ReadIds(args);

                if (action < 0 || ids.Count == 0)
                {
                    return new { ok = false, delta = 0 };
                }

                int delta = await Operate.SendConfig.List.SendListAction_ByIds(action, ids);
                return new { ok = true, delta = delta };
            });

            this.bridge.Register("importSends", async args =>
            {
                await Operate.SendConfig.List.LoadSendList_Dialog_Shell();
                return new { ok = true };
            });

            this.bridge.Register("exportSends", async args =>
            {
                await Operate.SendConfig.List.SaveAllSends_Dialog();
                return new { ok = true };
            });

            this.bridge.Register("clearSends", async args =>
            {
                await Operate.SendConfig.List.CleanUpSendList_Dialog_Shell();
                return new { ok = true };
            });

            /*
                发送列表的启停。

                「运行中」由 C# 说了算（IsSendListRunning），前端不自己记 ——
                它会自己跑完（所有发送执行完 worker 就结束），前端记的话会一直显示在跑。
                界面靠 1 秒的统计拍轮询这个值。
            */
            this.bridge.Register("startSendList", async args =>
            {
                /*
                    ⚠️ 注入模式下执行器<b>在目标进程里</b>。
                    在外壳里跑等于拿外壳自己的套接字去发，一个包也发不出去（还静默计成失败）。
                    「在跑没在跑」也由目标说了算 —— 它随 1 Hz 的 Stats 事件报上来。
                */
                /*
                    ⚠️ 预检要在<b>外壳这边</b>做：注入模式下 StartSendList 跑在目标进程里，
                    那里没有 UI，「系统套接字没设置」只剩日志、弹不出提示。
                    SystemSocket 本来就是外壳这份为准（再随 Runtime 快照推给目标）。
                */
                if (Operate.SendConfig.List.AllBlockedBySystemSocket())
                {
                    return new { running = false };
                }

                var link = this.AttachedLink();

                if (link != null)
                {
                    /*
                        ⚠️ 丢后台：这是一次<b>同步的管道往返</b>，而桥的处理器跑在 UI 线程上。
                        目标那头处理这条命令要真干活（StartSendList 起 worker、
                        SendPacket 可能撞上一个满的发送缓冲），占住 UI 线程就是整个界面卡住。
                        injectStartHook / OnListPushed 早就是这么写的，这四个漏了。

                        「在跑没在跑」不看这次调用的返回 —— 它由目标随 1 Hz 的 Stats 报上来，
                        前端靠 send:running 事件收，所以这里先回上一次的值完全够用。
                    */
                    await System.Threading.Tasks.Task.Run(() => link.StartSendList());
                    return new { running = link.SendListRunning };
                }

                Operate.SendConfig.List.StartSendList();
                return new { running = Operate.SendConfig.List.IsSendListRunning };
            });

            this.bridge.Register("stopSendList", async args =>
            {
                var link = this.AttachedLink();

                if (link != null)
                {
                    //同 startSendList：同步管道往返不占 UI 线程
                    await System.Threading.Tasks.Task.Run(() => link.StopSendList());
                    return new { running = link.SendListRunning };
                }

                Operate.SendConfig.List.StopSendList();
                return new { running = Operate.SendConfig.List.IsSendListRunning };
            });

            #endregion

            #region//机器人列表（对应 WinForms 的 Controls/RobotList）

            //与发送列表同一套接口，只是没有套接字那一列的元信息
            this.bridge.Register("getRobotMeta", args => new
            {
                running = Operate.RobotConfig.List.IsRobotListRunning,
                listExecute = (int)Operate.SystemConfig.ListExecute,   //见 getSendMeta 那条注释
            });

            this.bridge.Register("addRobot", args => new
            {
                id = Operate.RobotConfig.List.AddRobot_New_ById(),
            });

            this.bridge.Register("setRobotEnable", args => new
            {
                ok = Operate.RobotConfig.List.SetRobotEnable_ById(
                    args["id"] == null ? null : (string)args["id"],
                    args["enable"] != null && (bool)args["enable"]),
            });

            this.bridge.Register("setAllRobotEnable", args =>
            {
                int n = Operate.RobotConfig.List.SetAllRobotEnable(args["enable"] != null && (bool)args["enable"]);
                return new { ok = true, count = n };
            });

            //运行期计数，不落库
            this.bridge.Register("resetRobotCount", args =>
            {
                //同发送列表：真源在目标里
                ResetCountsEverywhere(WinsockPacketEditor.Ipc.ResetWhat.RobotCounts);
                return new { ok = true };
            });

            //右键菜单的七个动作，编号同滤镜 / 发送：0 置顶 · 1 上移 · 2 下移 · 3 置底 · 4 复制 · 5 导出 · 6 删除
            this.bridge.Register("robotListAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var ids = ReadIds(args);

                if (action < 0 || ids.Count == 0)
                {
                    return new { ok = false, delta = 0 };
                }

                int delta = await Operate.RobotConfig.List.RobotListAction_ByIds(action, ids);
                return new { ok = true, delta = delta };
            });

            this.bridge.Register("importRobots", async args =>
            {
                await Operate.RobotConfig.List.LoadRobotList_Dialog_Shell();
                return new { ok = true };
            });

            this.bridge.Register("exportRobots", async args =>
            {
                await Operate.RobotConfig.List.SaveAllRobots_Dialog();
                return new { ok = true };
            });

            this.bridge.Register("clearRobots", async args =>
            {
                await Operate.RobotConfig.List.CleanUpRobotList_Dialog_Shell();
                return new { ok = true };
            });

            /*
                机器人列表的启停。「运行中」由 C# 说了算（IsRobotListRunning），
                worker 会自己跑完，前端靠 1 秒的统计拍收 robot:running 事件。
            */
            this.bridge.Register("startRobotList", async args =>
            {
                //同发送列表：注入模式下执行器在目标里
                var link = this.AttachedLink();

                if (link != null)
                {
                    await System.Threading.Tasks.Task.Run(() => link.StartRobotList());
                    return new { running = link.RobotListRunning };
                }

                Operate.RobotConfig.List.StartRobotList();
                return new { running = Operate.RobotConfig.List.IsRobotListRunning };
            });

            this.bridge.Register("stopRobotList", async args =>
            {
                var link = this.AttachedLink();

                if (link != null)
                {
                    await System.Threading.Tasks.Task.Run(() => link.StopRobotList());
                    return new { running = link.RobotListRunning };
                }

                Operate.RobotConfig.List.StopRobotList();
                return new { running = Operate.RobotConfig.List.IsRobotListRunning };
            });

            #endregion

            #region//机器人编辑（对应 WinForms 的 Controls/RobotEdit）

            /*
                编辑的是一份工作副本：打开时拷贝、按保存才写回。InstructionInfo 没有主键，按<b>下标</b>收发。
                关弹窗（保存或取消）都要调 closeRobotEdit，否则执行器还挂在上一条上。
            */

            this.bridge.Register("openRobotEdit", args =>
                Operate.RobotConfig.Robot.OpenRobotEdit_ById(args["id"] == null ? null : (string)args["id"]));

            this.bridge.Register("closeRobotEdit", args =>
            {
                Operate.RobotConfig.Robot.CloseRobotEdit();
                return new { ok = true };
            });

            this.bridge.Register("getRobotInstructions", args => new
            {
                rows = Operate.RobotConfig.Robot.GetRobotInstructionRows(),
            });

            //插入一条：type 是 InstructionType 序号，content 是「类型|参数」串（格式在 C# 侧校验），insertAt = -1 追加
            this.bridge.Register("addRobotInstruction", args => new
            {
                error = Operate.RobotConfig.Robot.AddRobotInstruction_Edit(
                    args["type"] == null ? -1 : (int)args["type"],
                    args["content"] == null ? null : (string)args["content"],
                    args["insertAt"] == null ? -1 : (int)args["insertAt"]),
            });

            //右键菜单按下标收：0 置顶 · 1 上移 · 2 下移 · 3 置底 · 6 删除 · 7 清空
            this.bridge.Register("robotInstructionAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var idx = new List<int>();
                var arr = args["indexes"] as Newtonsoft.Json.Linq.JArray;

                if (arr != null)
                {
                    foreach (var x in arr) { idx.Add((int)x); }
                }

                if (action < 0)
                {
                    return new { ok = false, delta = 0 };
                }

                int delta = await Operate.RobotConfig.Robot.RobotInstructionAction_ByIndexes(action, idx);
                return new { ok = true, delta = delta };
            });

            //返回空串表示成功；badIndex 是校验没过的那条指令的下标（-1 = 没有），前端拿它高亮
            this.bridge.Register("saveRobotEdit", args => new
            {
                error = Operate.RobotConfig.Robot.SaveRobotEdit(args["name"] == null ? null : (string)args["name"]),
                badIndex = Operate.RobotConfig.Robot.LastBadIndex,
            });

            this.bridge.Register("startRobotEdit", args => new
            {
                error = Operate.RobotConfig.Robot.StartRobotEdit(args["name"] == null ? null : (string)args["name"]),
                badIndex = Operate.RobotConfig.Robot.LastBadIndex,
            });

            this.bridge.Register("stopRobotEdit", args =>
            {
                Operate.RobotConfig.Robot.StopRobotEdit();
                return new { ok = true };
            });

            this.bridge.Register("getRobotEditProgress", args => Operate.RobotConfig.Robot.GetRobotEditProgress());

            #endregion

            #region//工具页（文本对比 / 编码转换 / 数据提取；异或计算是纯前端的）

            /*
                文本比较（逐字符）在前端做；文本查重（找两段十六进制里共同的字节序列）是 O(n²) 的，
                留在 C#（SystemConfig.ComparePackets），而且丢进 Task.Run —— WinForms 那边也是放在 Spin 的后台线程里跑的。
            */
            this.bridge.Register("textDuplicates", async args =>
            {
                string a = args["a"] == null ? string.Empty : (string)args["a"];
                string b = args["b"] == null ? string.Empty : (string)args["b"];
                int min = args["min"] == null ? 2 : (int)args["min"];

                /*
                    ⚠️ 用 FindDuplicates 而不是 ComparePackets：后者还要再跑一趟 FindCommonSequences
                    把没命中的字节涂成下划线 —— 那是 WinForms 那两个只读框的显示方式，
                    外壳自己在十六进制视图上按位置高亮，<b>从来没读过</b>那两个串。白算了一趟。
                */
                var rows = await Task.Run(() => Operate.SystemConfig.FindDuplicates(a, b, Math.Max(1, min)));

                return new { rows };
            });

            //GBK 浏览器里编不了，14 行结果全在 C# 算
            this.bridge.Register("transcode", args => new
            {
                rows = Operate.SystemConfig.Transcode(
                    args["text"] == null ? string.Empty : (string)args["text"],
                    args["decode"] != null && (bool)args["decode"]),
            });

            //点「选择文件」：C# 弹原生文件框（浏览器拿不到完整路径）
            this.bridge.Register("extractPick", async args =>
                await Operate.SystemConfig.ExtractData_Pick_Dialog(args["kind"] == null ? 0 : (int)args["kind"]));

            //拖进浏览器的文件：只有内容没有路径，前端读成 base64 送过来
            this.bridge.Register("extractBytes", args =>
            {
                byte[] content = args["content"] == null ? new byte[0] : Convert.FromBase64String((string)args["content"]);
                var r = Operate.SystemConfig.ExtractData(args["kind"] == null ? 0 : (int)args["kind"], content);
                r.Path = args["name"] == null ? string.Empty : (string)args["name"];
                return r;
            });

            this.bridge.Register("saveExtraction", async args => new
            {
                path = await Operate.SystemConfig.SaveExtraction_Dialog(
                    args["kind"] == null ? 0 : (int)args["kind"],
                    args["text"] == null ? string.Empty : (string)args["text"]),
            });

            #endregion

            #region//WPC 配置（对应 WinForms 的 Controls/WPCConfig + ServerEdit / NoticeEdit / RuleList / RuleEdit）

            /*
                服务器与公告两份列表都在 FeedPump 的推送流里（FeedList.Server / Notice），前端只读副本，这里只出动作；
                规则是服务器下面的嵌套列表，按服务器 Id 单独取。
            */

            this.bridge.Register("saveServer", args => new
            {
                error = Operate.WPCConfig.ServerList.SaveServer_Shell(
                    args["id"] == null ? null : (string)args["id"],
                    args["enable"] != null && (bool)args["enable"],
                    args["name"] == null ? null : (string)args["name"],
                    args["ip"] == null ? null : (string)args["ip"],
                    args["port"] == null ? 0 : (int)args["port"],
                    args["forgotUrl"] == null ? null : (string)args["forgotUrl"],
                    args["registerUrl"] == null ? null : (string)args["registerUrl"],
                    args["verifyUrl"] == null ? null : (string)args["verifyUrl"]),
            });

            this.bridge.Register("setServerEnable", args => new
            {
                ok = Operate.WPCConfig.ServerList.SetServerEnable_ById(
                    args["id"] == null ? null : (string)args["id"], args["enable"] != null && (bool)args["enable"]),
            });

            this.bridge.Register("serverListAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var ids = ReadIds(args);
                if (action < 0 || ids.Count == 0) { return new { ok = false, delta = 0 }; }
                return new { ok = true, delta = await Operate.WPCConfig.ServerList.ServerListAction_ByIds(action, ids) };
            });

            this.bridge.Register("clearServers", async args =>
            {
                await Operate.WPCConfig.ServerList.CleanUpServerList_Dialog_Shell();
                return new { ok = true };
            });

            this.bridge.Register("getRuleTypes", args => new { rows = Operate.WPCConfig.ServerList.GetRuleTypes() });

            this.bridge.Register("getServerRules", args => new
            {
                rows = Operate.WPCConfig.ServerList.GetRuleRows_ById(args["sid"] == null ? null : (string)args["sid"]),
            });

            this.bridge.Register("saveServerRule", args => new
            {
                error = Operate.WPCConfig.ServerList.SaveRule_Shell(
                    args["sid"] == null ? null : (string)args["sid"],
                    args["id"] == null ? null : (string)args["id"],
                    args["enable"] != null && (bool)args["enable"],
                    args["type"] == null ? 0 : (int)args["type"],
                    args["argument"] == null ? null : (string)args["argument"],
                    args["ruleAction"] == null ? 0 : (int)args["ruleAction"]),
            });

            this.bridge.Register("setServerRuleEnable", args => new
            {
                ok = Operate.WPCConfig.ServerList.SetRuleEnable_ById(
                    args["sid"] == null ? null : (string)args["sid"],
                    args["id"] == null ? null : (string)args["id"],
                    args["enable"] != null && (bool)args["enable"]),
            });

            this.bridge.Register("serverRuleAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var ids = ReadIds(args);
                if (action < 0 || ids.Count == 0) { return new { ok = false, delta = 0 }; }
                return new { ok = true, delta = await Operate.WPCConfig.ServerList.RuleListAction_ByIds(args["sid"] == null ? null : (string)args["sid"], action, ids) };
            });

            this.bridge.Register("clearServerRules", async args =>
            {
                await Operate.WPCConfig.ServerList.CleanUpRuleList_Dialog_Shell(args["sid"] == null ? null : (string)args["sid"]);
                return new { ok = true };
            });

            this.bridge.Register("saveNotice", args => new
            {
                error = Operate.WPCConfig.NoticeList.SaveNotice_Shell(
                    args["id"] == null ? null : (string)args["id"],
                    args["type"] == null ? 1 : (int)args["type"],
                    args["title"] == null ? null : (string)args["title"],
                    args["content"] == null ? null : (string)args["content"],
                    args["more"] == null ? null : (string)args["more"]),
            });

            this.bridge.Register("noticeListAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var ids = ReadIds(args);
                if (action < 0 || ids.Count == 0) { return new { ok = false, delta = 0 }; }
                return new { ok = true, delta = await Operate.WPCConfig.NoticeList.NoticeListAction_ByIds(action, ids) };
            });

            this.bridge.Register("clearNotices", async args =>
            {
                await Operate.WPCConfig.NoticeList.CleanUpNoticeList_Dialog_Shell();
                return new { ok = true };
            });

            #endregion

            #region//统计数据

            this.bridge.Register("getFilterStats", args => Operate.SystemConfig.GetFilterStats());

            /*
                统计数据页的「归零」。只清<b>滤镜</b>那一组（六个全局计数 + 每条滤镜的执行次数），
                封包总数 / 流量不动 —— 那是代理数据页那一屏的仪表，清它要走「清空」。
            */
            this.bridge.Register("resetFilterStats", args =>
            {
                ResetCountsEverywhere(WinsockPacketEditor.Ipc.ResetWhat.FilterStats);
                return new { ok = true };
            });

            #endregion

            #region//系统日志（对应 WinForms 的 Controls/LogList 那个右键菜单）

            /*
                kind：0 = 系统日志、1 = 滤镜日志、2 = 代理日志，与前端三个页签同序。

                【前端的「清空」原先只清了自己那 2000 条环形缓冲】
                C# 侧的队列与 BindingList 纹丝不动 —— 表现是清完之后下一拍搬运又把积压的搬回来，
                而且真正占内存的那一份根本没释放。现在走 clearLogs 到 C# 去清，三样一起清。

                「复制」没做：SystemLog 那一页刻意不做虚拟滚动，就是为了能原生选中一段 Ctrl+C 拿走
                （见组件里的说明），再加一个「复制选中行」得先做出行选中来，不划算。
            */

            /*
                日志自己的自动清理（上限 + 开关）。

                <b>它与封包列表那一套是两份配置</b>：封包的在 InjectMode 表
                （PacketList_AutoClear，改在「列表设置」弹窗里），日志的在 SystemConfig 表
                （LogList_AutoClear / LogList_AutoClear_Value），消费点也不同 ——
                LogConfig.List.FlushToFeed 里三路日志各自按 AutoClear_Value 裁到最近 N 条。

                WinForms 侧它是 Controls/LogList 工具条上的控件、不在「列表设置」里，
                所以这里也放在日志页的工具条上，不并进那个弹窗。

                ⚠️ <b>只有自动清理，没有自动滚动。</b>LogList_AutoRoll 那个开关 2026-09-07
                去掉之后<b>没有加回来</b>：跟不跟随底部由「你现在在不在底部」决定
                （onScroll 每次重算），那是 tail -f 的行为，再摆一个落库的开关就是两条真源打架。
                这个字段仍在库里、WinForms 那边还在用，外壳不碰它。
            */
            this.bridge.Register("getLogSetting", args => new
            {
                autoClear = Operate.LogConfig.List.AutoClear,
                autoClearValue = (int)Operate.LogConfig.List.AutoClear_Value,
            });

            this.bridge.Register("saveLogSetting", args =>
            {
                try
                {
                    /*
                        「字段出现才改，没出现就不动」—— 与 saveLeachSetting / setAppearance 同一条协议。
                        勾选框点一下就存、条数框失焦或回车才存，两条路各自只发自己那一半；
                        一律补齐发全量的话，点开关会把用户正在编辑的半截数字也写进去。
                    */
                    if (args["autoClear"] != null)
                    {
                        Operate.LogConfig.List.AutoClear = (bool)args["autoClear"];
                    }

                    if (args["autoClearValue"] != null)
                    {
                        int keep = (int)args["autoClearValue"];

                        //与列表设置同一条范围，两处的语义是一样的：一张表最多留多少行
                        if (keep < 100 || keep > 500000)
                        {
                            return new { ok = false, error = UI.T("ListSettingsForm.Range", "保留条数需在 100 ~ 500000 之间") };
                        }

                        Operate.LogConfig.List.AutoClear_Value = keep;
                    }

                    //这两样都在 SystemConfig 表里，与主题 / 语言 / 快捷键同一张
                    Operate.SystemConfig.SaveSystemConfig_ToDB();

                    return new { ok = true };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveLogSetting", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            this.bridge.Register("clearLogs", async args => new
            {
                ok = await Operate.LogConfig.List.ClearLog_Dialog(args["kind"] == null ? 0 : (int)args["kind"]),
            });

            this.bridge.Register("exportLogs", async args =>
            {
                await Operate.LogConfig.List.ExportLog_Dialog(args["kind"] == null ? 0 : (int)args["kind"]);
                return new { ok = true };
            });

            #endregion

            #region//设置页 II：进程 / 映射 / 外部代理 / 快捷键 / 备份 / 远程管理

            //── 进程设置 ──

            this.bridge.Register("getProcessSetting", args => Operate.ProxyConfig.Proxy.GetProcessSetting());

            //进程枚举要几百毫秒，别卡 UI 线程
            this.bridge.Register("getProcessRows", async args => new { rows = await Task.Run(() => Operate.ProxyConfig.Proxy.GetProcessRows()) });

            /*
                进程图标按路径单独取（ProcessRow 里没有图标 —— B9 的规则把 Image 排除在 DTO 之外）。
                前端按路径记忆化，一次会话里同一个 exe 只取一次。
            */
            /*
                批量取：进程表回来之后前端按去重后的路径一次要一批。
                原先是逐个取（getProcessIcon）—— 二百多个进程就是二百多次桥往返、二百多次 ExtractAssociatedIcon 在 UI 线程上，
                前端那边每回来一个还整表重渲染一次。现在 Task.Run 在后台一次抽完，前端只写一次。
            */
            this.bridge.Register("getProcessIcons", async args =>
            {
                var arr = args["paths"] as Newtonsoft.Json.Linq.JArray;
                var paths = new List<string>();
                if (arr != null) { foreach (var x in arr) { string p = (string)x; if (!string.IsNullOrEmpty(p)) { paths.Add(p); } } }

                var icons = await Task.Run(() =>
                {
                    var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    foreach (string path in paths)
                    {
                        if (map.ContainsKey(path)) { continue; }

                        try
                        {
                            if (!System.IO.File.Exists(path)) { map[path] = string.Empty; continue; }

                            using (var ico = System.Drawing.Icon.ExtractAssociatedIcon(path))
                            using (var bmp = ico.ToBitmap())
                            using (var ms = new System.IO.MemoryStream())
                            {
                                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                map[path] = Convert.ToBase64String(ms.ToArray());
                            }
                        }
                        catch
                        {
                            map[path] = string.Empty;
                        }
                    }

                    return map;
                });

                return new { icons = icons };
            });

            this.bridge.Register("addSelectProcessName", args => new
            {
                ok = Operate.ProxyConfig.Proxy.AddSelectProcessName_ByPid(args["pid"] == null ? 0 : (int)args["pid"]),
            });

            this.bridge.Register("removeSelectProcessName", args => new
            {
                ok = Operate.ProxyConfig.Proxy.RemoveSelectProcessName(args["name"] == null ? string.Empty : (string)args["name"]),
            });

            this.bridge.Register("uninstallDriver", async args => new { ok = await Operate.ProxyConfig.Proxy.UninstallDriver_Dialog() });

            //保存是异步的：先真连一次转代理服务器，装驱动那一段走 UI.Busy 在后台跑（首次装驱动要几秒）
            this.bridge.Register("saveProcessSetting", async args =>
            {
                var pids = new List<int>();
                var arr = args["pids"] as Newtonsoft.Json.Linq.JArray;
                if (arr != null) { foreach (var x in arr) { pids.Add((int)x); } }

                return new
                {
                    error = await Operate.ProxyConfig.Proxy.SaveProcessSetting(
                        args["driverType"] == null ? 1 : (int)args["driverType"],
                        args["mustTcp"] != null && (bool)args["mustTcp"],
                        args["ip"] == null ? null : (string)args["ip"],
                        args["port"] == null ? 1080 : (int)args["port"],
                        args["appointPort"] != null && (bool)args["appointPort"],
                        args["appointPortContent"] == null ? null : (string)args["appointPortContent"],
                        args["auth"] != null && (bool)args["auth"],
                        args["userName"] == null ? null : (string)args["userName"],
                        args["passWord"] == null ? null : (string)args["passWord"],
                        pids),
                };
            });

            //进程设置与外部代理的「检测代理」共用
            this.bridge.Register("testSocksProxy", async args => new
            {
                error = await Operate.ProxyConfig.Proxy.TestSocksProxy(
                    args["auth"] != null && (bool)args["auth"],
                    args["ip"] == null ? null : (string)args["ip"],
                    args["port"] == null ? 0 : (int)args["port"],
                    args["userName"] == null ? null : (string)args["userName"],
                    args["passWord"] == null ? null : (string)args["passWord"]),
            });

            //── 映射设置 ──

            this.bridge.Register("getMapSetting", args => new
            {
                enableLocal = Operate.ProxyConfig.Mapping.Enable_MapLocal,
                enableRemote = Operate.ProxyConfig.Mapping.Enable_MapRemote,
            });

            this.bridge.Register("saveMapSetting", args =>
            {
                Operate.ProxyConfig.Mapping.SaveMapSetting_Shell(
                    args["enableLocal"] != null && (bool)args["enableLocal"],
                    args["enableRemote"] != null && (bool)args["enableRemote"]);
                return new { ok = true };
            });

            this.bridge.Register("saveMapLocal", args => new
            {
                error = Operate.ProxyConfig.Mapping.SaveMapLocal_Shell(
                    args["id"] == null ? null : (string)args["id"],
                    args["protocol"] == null ? 0 : (int)args["protocol"],
                    args["host"] == null ? null : (string)args["host"],
                    args["port"] == null ? 80 : (int)args["port"],
                    args["remotePath"] == null ? null : (string)args["remotePath"],
                    args["localPath"] == null ? null : (string)args["localPath"]),
            });

            this.bridge.Register("saveMapRemote", args => new
            {
                error = Operate.ProxyConfig.Mapping.SaveMapRemote_Shell(
                    args["id"] == null ? null : (string)args["id"],
                    args["protocolFrom"] == null ? 0 : (int)args["protocolFrom"],
                    args["hostFrom"] == null ? null : (string)args["hostFrom"],
                    args["portFrom"] == null ? 80 : (int)args["portFrom"],
                    args["pathFrom"] == null ? null : (string)args["pathFrom"],
                    args["protocolTo"] == null ? 0 : (int)args["protocolTo"],
                    args["hostTo"] == null ? null : (string)args["hostTo"],
                    args["portTo"] == null ? 80 : (int)args["portTo"],
                    args["pathTo"] == null ? null : (string)args["pathTo"]),
            });

            this.bridge.Register("setMapEnable", args =>
            {
                bool remote = args["remote"] != null && (bool)args["remote"];
                string id = args["id"] == null ? null : (string)args["id"];
                bool enable = args["enable"] != null && (bool)args["enable"];
                return new { ok = remote ? Operate.ProxyConfig.Mapping.SetMapRemoteEnable_ById(id, enable) : Operate.ProxyConfig.Mapping.SetMapLocalEnable_ById(id, enable) };
            });

            //单条：0 置顶 · 1 上移 · 2 下移 · 3 置底 · 6 删除
            this.bridge.Register("mapAction", async args =>
            {
                bool remote = args["remote"] != null && (bool)args["remote"];
                int action = args["action"] == null ? -1 : (int)args["action"];
                string id = args["id"] == null ? null : (string)args["id"];
                if (action < 0 || string.IsNullOrEmpty(id)) { return new { ok = false }; }
                bool ok = remote ? await Operate.ProxyConfig.Mapping.MapRemoteAction_ById(action, id) : await Operate.ProxyConfig.Mapping.MapLocalAction_ById(action, id);
                return new { ok = ok };
            });

            //整表：5 导出 · 7 清空 · 8 导入
            this.bridge.Register("mapCommand", async args =>
            {
                bool remote = args["remote"] != null && (bool)args["remote"];
                int action = args["action"] == null ? -1 : (int)args["action"];
                if (action < 0) { return new { ok = false }; }
                if (remote) { await Operate.ProxyConfig.Mapping.MapRemoteCommand_Shell(action); }
                else { await Operate.ProxyConfig.Mapping.MapLocalCommand_Shell(action); }
                return new { ok = true };
            });

            //本地映射的「本地文件」：C# 弹原生文件框（浏览器拿不到完整路径）
            this.bridge.Register("pickLocalFile", async args => new
            {
                path = await UI.PickOpen(new FilePick { Filter = "All Files (*.*)|*.*" }) ?? string.Empty,
            });

            //── 外部代理 ──

            this.bridge.Register("getExtProxySetting", args => Operate.ProxyConfig.Proxy.GetExtProxySetting());

            this.bridge.Register("saveExtProxySetting", args => new
            {
                error = Operate.ProxyConfig.Proxy.SaveExtProxySetting(
                    args["enable"] != null && (bool)args["enable"],
                    args["ip"] == null ? null : (string)args["ip"],
                    args["port"] == null ? 8889 : (int)args["port"],
                    args["appointPort"] != null && (bool)args["appointPort"],
                    args["appointPortContent"] == null ? null : (string)args["appointPortContent"],
                    args["auth"] != null && (bool)args["auth"],
                    args["userName"] == null ? null : (string)args["userName"],
                    args["passWord"] == null ? null : (string)args["passWord"]),
            });

            //── 快捷键 ──

            this.bridge.Register("getHotkeySetting", args => Operate.SystemConfig.GetHotkeySetting());

            this.bridge.Register("registerHotkey", args => new
            {
                ok = Operate.SystemConfig.RegisterHotkey_Shell(
                    args["index"] == null ? 0 : (int)args["index"],
                    args["text"] == null ? null : (string)args["text"]),
            });

            this.bridge.Register("saveHotkeyType", args =>
            {
                Operate.SystemConfig.SaveHotkeyType_Shell(args["type"] == null ? 0 : (int)args["type"]);
                return new { ok = true };
            });

            //── 备份 ──

            /*
                ⚠️ <b>走具名的 BackupParts，别用那个按位置收 11 个 bool 的重载</b>
                —— 那个只为 WinForms 那条线留着。加一项时这里多一行赋值即可，
                位置错了也不会静默出错（原来那种写法会）。
            */
            this.bridge.Register("exportBackup", async args =>
            {
                Func<string, bool> f = k => args[k] != null && (bool)args[k];

                await Operate.SystemConfig.ExportSystemBackUp_Dialog(
                    Operate.SystemConfig.AssemblyVersion,
                    new Operate.SystemConfig.BackupParts
                    {
                        SystemConfig = f("systemConfig"),
                        ProxySet = f("proxySet"),
                        ProxyAccount = f("proxyAccount"),
                        WhiteList = f("whiteList"),
                        BlackList = f("blackList"),
                        ProxyMapping = f("proxyMapping"),
                        InjectSet = f("injectSet"),
                        FilterList = f("filterList"),
                        SendList = f("sendList"),
                        RobotList = f("robotList"),
                        WareHouse = f("wareHouse"),
                        AutoStores = f("autoStores"),
                        WpcServer = f("wpcServer"),
                        WpcNotice = f("wpcNotice"),
                    });

                return new { ok = true };
            });

            /*
                导入备份：配置与各份列表被整份换掉。
                ① 全部列表标脏整表重推 ② 立刻落库。
                语言回给前端，页面字典要跟着切（否则弹窗英文、页面中文）。
                （C# 侧的文案不用做什么：UI.T 每次都现读 UI.Prefs.Language。）
            */
            this.bridge.Register("importBackup", async args =>
            {
                await Operate.SystemConfig.ImportSystemBackUp_Dialog();
                FeedPump.MarkAllDirty();
                this.SaveProxyState();

                /*
                    备份里什么都有 —— 拦截开关、滤镜 / 发送 / 机器人、极速模式、执行方式。
                    MarkAllDirty 只会经 ListPushed 带出那三份列表的快照，
                    <b>拦截开关与 Runtime 得自己补一次</b>，否则导入之后目标还在按旧配置跑。
                */
                var link = this.AttachedLink();
                if (link != null) { link.TryPush(link.PushAll); }

                //备份里的 IsDark 可能与当前相反，窗体四周那圈底色跟着换
                this.ApplyShellBack();

                /*
                    语言与主题都要带回去：备份里存着 DefaultLanguage 与 IsDark，
                    <b>界面是前端在管的</b> —— 不回传的话会变成
                    「弹窗切过去了、页面还是旧语言旧配色」。
                */
                return new
                {
                    language = UI.Prefs.Language ?? string.Empty,
                    isDark = UI.Prefs.IsDark,
                    themeMode = ThemeMode(),
                    scanLine = UI.Prefs.ScanLine,
                };
            });

            //── 远程管理 ──

            this.bridge.Register("getRemoteSetting", args => Operate.SystemConfig.GetRemoteSetting());

            this.bridge.Register("saveRemoteSetting", args => new
            {
                error = Operate.SystemConfig.SaveRemoteSetting(
                    args["isRemote"] != null && (bool)args["isRemote"],
                    args["ip"] == null ? null : (string)args["ip"],
                    args["port"] == null ? 88 : (int)args["port"],
                    args["userName"] == null ? null : (string)args["userName"],
                    args["passWord"] == null ? null : (string)args["passWord"]),
                running = Operate.SystemConfig.GetRemoteSetting().Running,
            });

            #endregion

            #region//封包列表右键菜单（对应 WinForms 的 ProxyList 那个 ContextMenuStrip）

            /*
                菜单结构由前端自己拼（与其余各屏一致）——「添加到发送 / 添加到仓库」
                两个子菜单的内容前端本来就有（FeedList.Send / FeedList.WareHouse 一直在推）。
                这里只出动作。行的键是 ProxyInfo.Id（运行期自增的 long）。
            */

            this.bridge.Register("copyProxyHex", args => new
            {
                text = Operate.ProxyConfig.List.GetProxyHex_ByIds(ReadLongIds(args)),
            });

            this.bridge.Register("addProxyToSend", args => new
            {
                count = Operate.ProxyConfig.List.AddToSend_ByProxyIds(
                    args["sid"] == null ? null : (string)args["sid"], ReadLongIds(args)),
            });

            this.bridge.Register("addProxyToWareHouse", args => new
            {
                count = Operate.ProxyConfig.List.AddToWareHouse_ByProxyIds(
                    args["wid"] == null ? null : (string)args["wid"], ReadLongIds(args)),
            });

            this.bridge.Register("addProxyToFilter", args => new
            {
                ok = Operate.ProxyConfig.List.AddToFilter_ByProxyId(
                    args["id"] == null ? 0L : (long)args["id"]),
            });

            this.bridge.Register("setSystemSocketByProxy", args => new
            {
                socket = Operate.ProxyConfig.List.SetSystemSocket_ByProxyId(
                    args["id"] == null ? 0L : (long)args["id"]),
            });

            //ids 为空就是导整张表 —— SaveProxyList_Dialog 本来就这么写的
            this.bridge.Register("exportProxyExcel", async args =>
            {
                await Operate.ProxyConfig.List.ExportProxyExcel_ByIds(ReadLongIds(args));
                return new { ok = true };
            });

            /*
                注入模式那一份（PacketInfo）。与上面六个<b>逐个对应</b>，
                区别只在取哪张表 —— 两份列表的 Id 各自独立自增，
                同一个数字在两份表里是两条不同的包，所以入口必须分开，不能共用。
            */

            this.bridge.Register("copyPacketHex", args => new
            {
                text = Operate.PacketConfig.List.GetPacketHex_ByIds(ReadLongIds(args)),
            });

            this.bridge.Register("addPacketToSend", args => new
            {
                count = Operate.PacketConfig.List.AddToSend_ByPacketIds(
                    args["sid"] == null ? null : (string)args["sid"], ReadLongIds(args)),
            });

            this.bridge.Register("addPacketToWareHouse", args => new
            {
                count = Operate.PacketConfig.List.AddToWareHouse_ByPacketIds(
                    args["wid"] == null ? null : (string)args["wid"], ReadLongIds(args)),
            });

            this.bridge.Register("addPacketToFilter", args => new
            {
                ok = Operate.PacketConfig.List.AddToFilter_ByPacketId(
                    args["id"] == null ? 0L : (long)args["id"]),
            });

            this.bridge.Register("setSystemSocketByPacket", args =>
            {
                int socket = Operate.PacketConfig.List.SetSystemSocket_ByPacketId(
                    args["id"] == null ? 0L : (long)args["id"]);

                //系统套接字在 Runtime 快照里，改完要推下去 —— 用它的执行器跑在目标进程
                this.PushRuntimeToTarget();
                return new { socket = socket };
            });

            /*
                记下「当前选中的那一条封包」。

                WinForms 侧是表格的 SelectedIndexChanged 顺手做的（PacketList.cs:969），
                外壳没有那个控件 —— 不设的话两条机器人指令会静默失效：
                「发送 → 封包列表」与「设置系统套接字 → 封包列表」。
            */
            this.bridge.Register("setSelectedPacket", args =>
            {
                Operate.PacketConfig.List.SetSelectedPacket_ById(
                    args["id"] == null ? 0L : (long)args["id"]);

                this.PushRuntimeToTarget();
                return new { ok = true };
            });

            //ids 为空就是导整张表 —— SavePacketListToExcel 本来就这么写的
            this.bridge.Register("exportPacketExcel", async args =>
            {
                await Operate.PacketConfig.List.ExportPacketExcel_ByIds(ReadLongIds(args));
                return new { ok = true };
            });

            #endregion

            #region//查找封包（对应 WinForms 的 Controls/SearchPacket）

            /*
                正则扫代理列表，返回第一条命中的行 + 命中字节在包里的位置。

                <b>跑在 Task.Run 上</b>：列表可以有几万行、每行都要解码成字符串再跑一次正则，
                在 UI 线程上做会把界面卡住（WinForms 那边用的是 BackgroundWorker，同一个理由）。

                「查找下一个」由前端把上一次的 Index + 1 传回来 —— 游标留在前端，
                C# 侧不存 Search_Index，省掉一份要同步的状态（WinForms 那边存在 Operate 里，
                因为控件与 Operate 之间没有别的通道）。
            */
            this.bridge.Register("searchProxyList", async args =>
            {
                string pattern = (string)args["pattern"] ?? string.Empty;
                bool isHex = args["isHex"] != null && (bool)args["isHex"];
                int from = args["from"] == null ? 0 : (int)args["from"];

                //在这一行之内从哪儿接着找。前端把上一次的 NextPos 原样传回来，它不需要知道这是什么单位
                int fromPos = args["fromPos"] == null ? 0 : (int)args["fromPos"];

                return await System.Threading.Tasks.Task.Run(
                    () => Operate.ProxyConfig.List.SearchProxy_Shell(pattern, isHex, from, fromPos));
            });

            //注入模式那一份。同样跑在 Task.Run 上，同样把游标留给前端
            this.bridge.Register("searchPacketList", async args =>
            {
                string pattern = (string)args["pattern"] ?? string.Empty;
                bool isHex = args["isHex"] != null && (bool)args["isHex"];
                int from = args["from"] == null ? 0 : (int)args["from"];
                int fromPos = args["fromPos"] == null ? 0 : (int)args["fromPos"];

                return await System.Threading.Tasks.Task.Run(
                    () => Operate.PacketConfig.List.SearchPacket_Shell(pattern, isHex, from, fromPos));
            });

            #endregion

            #region//发送编辑（对应 WinForms 的 Controls/SendEdit）

            /*
                编辑的是一份<b>工作副本</b>：打开时拷贝、按保存才写回。
                所以这一组几乎都不带发送 Id —— C# 侧记着当前编的是哪条。
                关弹窗（保存或取消）都要调 closeSendEdit，否则执行器还挂在上一条上。
            */

            this.bridge.Register("openSendEdit", args =>
                Operate.SendConfig.Send.OpenSendEdit_ById(
                    args["id"] == null ? null : (string)args["id"]));

            this.bridge.Register("closeSendEdit", args =>
            {
                Operate.SendConfig.Send.CloseSendEdit();
                return new { ok = true };
            });

            this.bridge.Register("getSendCollection", args => new
            {
                rows = Operate.SendConfig.Send.GetSendCollectionRows(),
            });

            //发送集的右键菜单，只有六个动作（没有导出，导出在工具条上、整表导）
            this.bridge.Register("sendCollectionAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var ids = ReadIds(args);

                if (action < 0 || ids.Count == 0)
                {
                    return new { ok = false, delta = 0 };
                }

                int delta = await Operate.SendConfig.Send.SendCollectionAction_ByIds(action, ids);
                return new { ok = true, delta = delta };
            });

            this.bridge.Register("importSendCollection", async args =>
            {
                await Operate.SendConfig.Send.ImportSendCollection_Dialog_Shell();
                return new { ok = true };
            });

            this.bridge.Register("exportSendCollection", async args =>
            {
                await Operate.SendConfig.Send.ExportSendCollection_Dialog_Shell();
                return new { ok = true };
            });

            this.bridge.Register("clearSendCollection", async args =>
            {
                await Operate.SendConfig.Send.ClearSendCollection_Dialog_Shell();
                return new { ok = true };
            });

            //返回空串表示成功，否则是要显示给用户的错误文案（校验在 C# 侧，别在前端再写一份）
            this.bridge.Register("saveSendEdit", args => new
            {
                error = Operate.SendConfig.Send.SaveSendEdit(
                    args["name"] == null ? null : (string)args["name"],
                    args["useSystemSocket"] != null && (bool)args["useSystemSocket"],
                    args["loopCount"] == null ? 1 : (int)args["loopCount"],
                    args["loopInterval"] == null ? 0 : (int)args["loopInterval"],
                    args["notes"] == null ? null : (string)args["notes"]),
            });

            this.bridge.Register("startSendEdit", args => new
            {
                error = Operate.SendConfig.Send.StartSendEdit(
                    args["name"] == null ? null : (string)args["name"],
                    args["useSystemSocket"] != null && (bool)args["useSystemSocket"],
                    args["loopCount"] == null ? 1 : (int)args["loopCount"],
                    args["loopInterval"] == null ? 0 : (int)args["loopInterval"],
                    args["notes"] == null ? null : (string)args["notes"]),
            });

            this.bridge.Register("stopSendEdit", args =>
            {
                Operate.SendConfig.Send.StopSendEdit();
                return new { ok = true };
            });

            //在跑的时候前端按 200ms 轮询。不做成推事件的理由见 GetSendEditProgress 的注释
            this.bridge.Register("getSendEditProgress", args =>
                Operate.SendConfig.Send.GetSendEditProgress());

            #endregion

            #region//仓库列表（对应 WinForms 的 Controls/WareHouseList + AutoStoresList + AutoStoresEdit）

            /*
                仓库本身与自动入库规则都在 FeedPump 的推送流里（FeedList.WareHouse / AutoStores），
                前端只读副本；这里只出<b>动作</b>。两份都按 Id 字符串收发：
                仓库是 WID，自动入库是运行期分配的 AID（那个模型没有自然主键，见 AutoStoresInfo.AID）。
            */

            this.bridge.Register("addWareHouse", args => new
            {
                id = Operate.WareHouseConfig.List.AddWareHouse_New_ById(),
            });

            //右键菜单的七个动作，编号同滤镜 / 发送：0 置顶 · 1 上移 · 2 下移 · 3 置底 · 4 复制 · 5 导出 · 6 删除
            this.bridge.Register("wareHouseListAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var ids = ReadIds(args);

                if (action < 0 || ids.Count == 0)
                {
                    return new { ok = false, delta = 0 };
                }

                int delta = await Operate.WareHouseConfig.List.WareHouseListAction_ByIds(action, ids);
                return new { ok = true, delta = delta };
            });

            this.bridge.Register("importWareHouses", async args =>
            {
                await Operate.WareHouseConfig.List.LoadWareHouseList_Dialog_Shell();
                return new { ok = true };
            });

            this.bridge.Register("exportWareHouses", async args =>
            {
                await Operate.WareHouseConfig.List.SaveAllWareHouses_Dialog();
                return new { ok = true };
            });

            this.bridge.Register("clearWareHouses", async args =>
            {
                await Operate.WareHouseConfig.List.CleanUpWareHouseList_Dialog_Shell();
                return new { ok = true };
            });

            /*
                自动入库的总开关。<b>不落库</b>，与 WinForms 一致（AutoStoresList 上那句
                「每次重启软件后需手动开启」就是在说这件事）—— 它决定的是每个经过的封包都要
                多跑一遍规则比对，默认关着是刻意的。规则本身（增删改 / 启用）则每一步都落库。
            */
            this.bridge.Register("getAutoStoresMeta", args => new
            {
                enable = Operate.WareHouseConfig.WareHouse.Enable_AutoStores,
                limit = Operate.WareHouseConfig.WareHouse.StoresLimit,
                limitValue = Operate.WareHouseConfig.WareHouse.StoresLimit_Value,
            });

            /*
                总开关是<b>纯运行期</b>的（重启回到关），上限则要落库 ——
                前者决定「每个经过的封包多跑一遍规则比对」，默认关着是刻意的；
                后者是一条配置，用户改完当然要记住。所以这里存了一次库。
            */
            this.bridge.Register("setAutoStoresSwitch", args =>
            {
                Operate.WareHouseConfig.WareHouse.Enable_AutoStores = args["enable"] != null && (bool)args["enable"];

                if (args["limit"] != null)
                {
                    Operate.WareHouseConfig.WareHouse.StoresLimit = (bool)args["limit"];
                }

                if (args["limitValue"] != null)
                {
                    //1 条以下没有意义；上界照封包列表自动清理那个数量级来
                    int v = (int)args["limitValue"];
                    Operate.WareHouseConfig.WareHouse.StoresLimit_Value = Math.Max(1, Math.Min(1000000, v));
                }

                Operate.SystemConfig.SaveSystemConfig_ToDB();

                return new
                {
                    ok = true,
                    enable = Operate.WareHouseConfig.WareHouse.Enable_AutoStores,
                    limit = Operate.WareHouseConfig.WareHouse.StoresLimit,
                    limitValue = Operate.WareHouseConfig.WareHouse.StoresLimit_Value,
                };
            });

            //新增 / 改一条。id 为空 = 新增。校验（非空 / 十六进制 / 仓库存在 / 包头不重复）在 C# 侧
            this.bridge.Register("saveAutoStores", args =>
            {
                string err = Operate.WareHouseConfig.List.SaveAutoStores_Shell(
                    args["id"] == null ? null : (string)args["id"],
                    args["head"] == null ? null : (string)args["head"],
                    args["wid"] == null ? null : (string)args["wid"]);

                return new { ok = string.IsNullOrEmpty(err), error = err };
            });

            this.bridge.Register("setAutoStoresEnable", args => new
            {
                ok = Operate.WareHouseConfig.List.SetAutoStoresEnable_ById(
                    args["id"] == null ? null : (string)args["id"],
                    args["enable"] != null && (bool)args["enable"]),
            });

            //删一条，确认框在 C# 侧（DeleteAutoStores_Dialog），与 WinForms 同一条链路
            this.bridge.Register("deleteAutoStores", async args => new
            {
                ok = await Operate.WareHouseConfig.List.DeleteAutoStores_Dialog_ById(
                    args["id"] == null ? null : (string)args["id"]),
            });

            //置顶 / 上移 / 下移 / 置底作用于 id 那一条；导入(8) / 导出(5) / 清空(7) 不看 id
            this.bridge.Register("autoStoresAction", async args =>
            {
                await Operate.WareHouseConfig.List.AutoStoresAction_ById(
                    args["action"] == null ? -1 : (int)args["action"],
                    args["id"] == null ? null : (string)args["id"]);

                return new { ok = true };
            });

            #endregion

            #region//仓库编辑（对应 WinForms 的 Controls/WareHouseEdit）

            /*
                与发送编辑不同，这里<b>没有编辑会话</b>：WinForms 那边改的就是仓库本身
                （WareHouseEdit_Load 里 this.Stores = whiSelect.Stores），只有名字按保存才写回。
                所以每个入口都带 wid，前端关弹窗也不用收尾。仓储数据的改动不即时落库，
                理由见 Operate 里「仓库编辑」那段说明。
            */

            //打开：只要名字与条数；仓储行另取（getStoreRows）。行不带字节，要字节走 copyStoresHex。
            //⚠️ 别改成拿 FindWareHouse_ById —— WareHouseInfo 出现在这里就是 CS0012，出 DTO 的那个才能用
            this.bridge.Register("openWareHouseEdit", args =>
            {
                var row = Operate.WareHouseConfig.List.OpenWareHouseEdit_ById(
                    args["wid"] == null ? null : (string)args["wid"]);

                return new { id = row.Id ?? string.Empty, name = row.Name ?? string.Empty };
            });

            this.bridge.Register("getStoreRows", args => new
            {
                rows = Operate.WareHouseConfig.List.GetStoreRows_ById(
                    args["wid"] == null ? null : (string)args["wid"]),
            });

            /*
                预览按可见窗口取。getStoreRows 出的行不带预览（见 StoreRow.From_ 的说明）——
                仓库大起来时那一列就是整条报文的大头。
            */
            this.bridge.Register("getStorePreviews", args => new
            {
                items = Operate.WareHouseConfig.List.GetStorePreviews_ById(
                    args["wid"] == null ? null : (string)args["wid"],
                    args["from"] == null ? 0 : (int)args["from"],
                    args["count"] == null ? 0 : (int)args["count"]),
            });

            //复制的十六进制由 C# 拼好整段再交给前端写剪贴板，格式与封包列表那份一致
            this.bridge.Register("copyStoresHex", args => new
            {
                text = Operate.WareHouseConfig.List.GetStoresHex_ByIds(
                    args["wid"] == null ? null : (string)args["wid"], ReadIds(args)),
            });

            //右键菜单的七个动作：0 置顶 · 1 上移 · 2 下移 · 3 置底 · 4 复制 · 5 导出选中 · 6 删除
            this.bridge.Register("storesAction", async args =>
            {
                int action = args["action"] == null ? -1 : (int)args["action"];
                var ids = ReadIds(args);

                if (action < 0 || ids.Count == 0)
                {
                    return new { ok = false, delta = 0 };
                }

                int delta = await Operate.WareHouseConfig.List.StoresAction_ByIds(
                    args["wid"] == null ? null : (string)args["wid"], action, ids);

                return new { ok = true, delta = delta };
            });

            //工具条：导入(8) / 导出全部(5) / 清空(7)
            this.bridge.Register("storesCommand", async args =>
            {
                await Operate.WareHouseConfig.List.StoresCommand_Shell(
                    args["wid"] == null ? null : (string)args["wid"],
                    args["action"] == null ? -1 : (int)args["action"]);

                return new { ok = true };
            });

            this.bridge.Register("saveWareHouseName", args => new
            {
                error = Operate.WareHouseConfig.List.SaveWareHouseName_ById(
                    args["wid"] == null ? null : (string)args["wid"],
                    args["name"] == null ? null : (string)args["name"]),
            });

            #endregion

            #region//封包编辑（对应 WinForms 的 Controls/PacketEdit）

            /*
                编辑在前端做（十六进制编辑器是纯前端组件），C# 只管：打开时给整段字节、
                保存时收整段字节、发送会话、右键菜单里要碰 Operate 的两个动作。
                字节走 base64：byte[] 由 JSON.NET 自动编成 base64 串，收回来用 FromBase64String。
                list = "proxy"（代理数据列表）/ "packet"（注入模式的封包列表）/ "send"（发送编辑的工作副本），
                id 是运行期自增的 long —— proxy 与 packet 各自独立自增，所以 list 必须一路带下去。
            */

            Func<Newtonsoft.Json.Linq.JObject, byte[]> readBytes = a =>
            {
                string b64 = a["buffer"] == null ? null : (string)a["buffer"];
                if (string.IsNullOrEmpty(b64)) { return new byte[0]; }
                try { return Convert.FromBase64String(b64); }
                catch (Exception ex) { Operate.DoLog("packetEdit.readBytes", ex); return new byte[0]; }
            };

            Func<Newtonsoft.Json.Linq.JObject, string> readList = a =>
                a["list"] == null ? Operate.PacketEditConfig.ListProxy : (string)a["list"];

            Func<Newtonsoft.Json.Linq.JObject, long> readId = a =>
                a["id"] == null ? 0L : (long)a["id"];

            this.bridge.Register("openPacketEdit", args =>
                Operate.PacketEditConfig.Open(readList(args), readId(args)));

            this.bridge.Register("savePacketEdit", args => new
            {
                error = Operate.PacketEditConfig.Save(
                    readList(args), readId(args),
                    args["socket"] == null ? 0 : (int)args["socket"],
                    readBytes(args)),
            });

            //bytes 是编辑器里选中的那一段（没选就是整包）
            this.bridge.Register("packetEditToFilter", args => new
            {
                ok = Operate.PacketEditConfig.AddToFilter(readList(args), readId(args), readBytes(args)),
            });

            this.bridge.Register("packetEditToSend", args => new
            {
                ok = Operate.PacketEditConfig.AddToSend(
                    args["sid"] == null ? null : (string)args["sid"],
                    readList(args), readId(args), readBytes(args)),
            });

            //发送会话：开始 / 停止 / 进度（前端在跑的时候 200ms 轮询，与发送编辑同一套）
            this.bridge.Register("startPacketSend", args => new
            {
                error = Operate.PacketEditConfig.StartSend(
                    readList(args), readId(args),
                    args["socket"] == null ? 0 : (int)args["socket"],
                    readBytes(args),
                    args["continuous"] != null && (bool)args["continuous"],
                    args["times"] == null ? 1 : (int)args["times"],
                    args["interval"] == null ? 0 : (int)args["interval"],
                    args["progression"] != null && (bool)args["progression"],
                    args["position"] == null ? 0 : (int)args["position"],
                    args["step"] == null ? 1 : (int)args["step"],
                    args["carry"] != null && (bool)args["carry"],
                    args["carryCount"] == null ? 1 : (int)args["carryCount"]),
            });

            this.bridge.Register("stopPacketSend", args =>
            {
                Operate.PacketEditConfig.StopSend();
                return new { ok = true };
            });

            this.bridge.Register("getPacketSendProgress", args =>
                Operate.PacketEditConfig.GetSendProgress());

            #endregion

            #region//多开设置（对应 WinForms 的 Controls/DataBaseSetting）

            /*
                选数据库目录。

                <b>必须由 C# 弹原生对话框</b> —— 浏览器拿不到完整路径
                （<input type=file webkitdirectory> 只给相对名与内容），而 Operate 要的正是路径。
                与 BridgeUiHost 的 PickOpen / PickSave 同一个理由，见 CLAUDE.md「文件框不走桥」。

                用 System.Windows.Forms 自带的 FolderBrowserDialog，不用 AntdUI 那个：
                外壳只 ProjectReference 了主工程，AntdUI 是主工程 packages.config 里的包，
                不会传递过来；为一个文件夹对话框再加一个引用不划算。
            */
            this.bridge.Register("pickFolder", args =>
            {
                string start = args["path"] == null ? null : (string)args["path"];

                Func<string> pick = () =>
                {
                    using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                    {
                        fbd.Description = UI.T("MultipleOpenSetting.DBPath", "数据库路径");
                        fbd.ShowNewFolderButton = true;

                        try
                        {
                            //目录不存在时留空，否则对话框会退回「桌面」，还不如从上级开始
                            if (!string.IsNullOrEmpty(start) && Directory.Exists(start))
                            {
                                fbd.SelectedPath = start;
                            }
                        }
                        catch (Exception ex)
                        {
                            Operate.DoLog("pickFolder.SelectedPath", ex);
                        }

                        return fbd.ShowDialog(this) == DialogResult.OK ? fbd.SelectedPath : null;
                    }
                };

                string picked = this.InvokeRequired ? (string)this.Invoke(pick) : pick();
                return new { path = picked };
            });

            /*
                探一个候选路径会发生什么，供界面在保存<b>之前</b>就说清楚。

                WinForms 那版按下保存之前什么都看不出来 —— 目录会不会被创建、
                库是新建还是沿用已有的，全靠猜。
            */
            this.bridge.Register("probeDbPath", args =>
            {
                string dir = args["path"] == null ? string.Empty : ((string)args["path"] ?? string.Empty).Trim();
                string name = Operate.DataBase.dbName ?? string.Empty;

                bool valid = false;
                bool dirExists = false;
                bool fileExists = false;
                long size = 0;
                string modified = string.Empty;
                string full = string.Empty;

                try
                {
                    if (dir.Length > 0)
                    {
                        //非法字符 / 相对路径都在这一步暴露，不要让它烂到 InitDB 里去
                        full = Path.GetFullPath(Path.Combine(dir, name));
                        valid = Path.IsPathRooted(dir);
                        dirExists = Directory.Exists(dir);

                        if (dirExists && File.Exists(full))
                        {
                            FileInfo fi = new FileInfo(full);
                            fileExists = true;
                            size = fi.Length;
                            modified = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //路径非法（含 * ? " 等）在这里被吃掉，valid 保持 false，界面据此禁用保存
                    Operate.DoLog("probeDbPath", ex);
                    valid = false;
                }

                return new
                {
                    valid = valid,
                    dirExists = dirExists,
                    fileExists = fileExists,
                    size = size,
                    modified = modified,
                    full = full,
                    //当前正生效的那个，界面并排显示，便于对比
                    current = CurrentDbFull(),
                    currentSize = CurrentDbSize(),
                };
            });

            /*
                切换实例：改路径 → 建库 → <b>重载配置</b>。

                【为什么要重载】
                WinForms 那版只做前两步，内存里的配置仍是从<b>旧库</b>读进来的，
                退出时又会被写进<b>新库</b> —— 两个实例的配置会互相污染，
                这多少违背了多开的初衷。这里补上第三步。

                【为什么只重载 LoadSystemConfig_FromDB，不连 14 份列表一起重载】
                两个理由，缺一个都不成立：
                  ① 外壳启动时本来也只调这一个（Program.Main），重载它就回到了
                     「以新库重新启动」的等价状态；列表是进模式时才加载的。
                  ② <b>那些 Load*_FromDB 加载前不清表</b>，是照「一辈子只调一次」写的
                     （见 LoadFilterList_FromDB：直接 foreach + AddFilter）。
                     再调一次就是旧库的行 + 新库的行叠在一起。
                     等哪天要在这里连列表一起重载，得先给每份列表补一个 Clear。
            */
            this.bridge.Register("saveInstance", args =>
            {
                string dir = args["path"] == null ? string.Empty : ((string)args["path"] ?? string.Empty).Trim();

                if (dir.Length == 0)
                {
                    return new { ok = false, error = UI.T("MultipleOpenSetting.DBPath", "数据库路径") };
                }

                try
                {
                    Operate.DataBase.dbPath = Path.GetFullPath(dir).TrimEnd(Path.DirectorySeparatorChar);
                    Operate.DataBase.InitDB();

                    //纯字段赋值，可重复调用；不碰任何 BindingList（已核过 2582–2665 行）
                    Operate.SystemConfig.LoadSystemConfig_FromDB();
                    this.ApplyShellBack();

                    Operate.DoLog("saveInstance", "已切换数据库 : " + CurrentDbFull());

                    string socks5Addr, httpAddr;
                    ProxyAddresses(out socks5Addr, out httpAddr);

                    return new
                    {
                        ok = true,
                        dbDir = Operate.DataBase.dbPath,
                        dbFile = Operate.DataBase.dbName,
                        dbFull = CurrentDbFull(),
                        dbInstance = InstanceName(),
                        //配置换了一份，这三样界面都得跟着刷新
                        language = UI.Prefs.Language ?? "zh-CN",
                        socks5Port = Operate.ProxyConfig.Proxy.SOCKS5_Port,
                        socks5Addr = socks5Addr,
                        httpAddr = httpAddr,
                    };
                }
                catch (Exception ex)
                {
                    Operate.DoLog("saveInstance", ex);
                    return new { ok = false, error = ex.Message };
                }
            });

            #endregion

            #region//窗口控制（无边框，标题栏由前端自绘）

            this.bridge.Register("minimizeWindow", args =>
            {
                this.WindowState = FormWindowState.Minimized;
                return new { ok = true };
            });

            this.bridge.Register("toggleMaximize", args =>
            {
                this.WindowState = this.WindowState == FormWindowState.Maximized
                    ? FormWindowState.Normal
                    : FormWindowState.Maximized;

                return new { maximized = this.WindowState == FormWindowState.Maximized };
            });

            this.bridge.Register("closeWindow", args =>
            {
                this.Close();
                return new { ok = true };
            });

            /*
                窗口保持最前。对应 WinForms 里 ProxyList 工具条上那个「窗口保持最前」勾选框
                （`Controls/ProxyList.cs` 的「窗口保持最前」region，那边也是直接写 form.TopMost）。

                <b>刻意不落库</b>：WinForms 侧同样只是一个运行期的窗体属性，没有对应的配置字段，
                重启就回到「不置顶」。抓包时把它压在游戏窗口上面是临时行为，
                下次开机默认还压着反而碍事。

                回的是 this.TopMost 而不是入参 —— 万一 Windows 拒绝了这次置顶
                （极少见，但全屏独占的程序会），前端显示的就该是实际状态。
            */
            this.bridge.Register("setTopMost", args =>
            {
                this.TopMost = args["on"] != null && (bool)args["on"];
                return new { topMost = this.TopMost };
            });

            /*
                回落用的拖动（仅当 WebView2 原生非客户区支持不可用时，前端才会调）。

                用 BeginInvoke 把实际拖动排到消息处理器<b>返回之后</b>再跑 ——
                SendMessage(WM_NCLBUTTONDOWN) 会阻塞到松开鼠标为止，直接在这里调
                等于把整个拖动过程卡在 WebView2 的 WebMessageReceived 处理器里。
            */
            this.bridge.Register("startDragWindow", args =>
            {
                this.BeginInvoke((Action)this.StartDrag);
                return new { ok = true };
            });

            #endregion

            //启动自检：启动页那块终端要显示的内容
            this.bridge.Register("getSystemCheck", args =>
            {
                string dbDir = Operate.DataBase.dbPath ?? string.Empty;
                string dbFile = Operate.DataBase.dbName ?? string.Empty;

                string socks5Addr, httpAddr;
                ProxyAddresses(out socks5Addr, out httpAddr);

                return new
                {
                    isAdmin = Operate.SystemConfig.IsAdministrator(),
                    /*
                        这里曾返回 WebView2 版本给自检显示，已去掉：
                        程序能跑起来就说明运行时在（Program.Main 建窗前调 HasWebView2Runtime，
                        缺失时直接弹框并退出），所以那一项永远显示「有」，没有信息量。
                        真正会变、会过期的是归属地库，那个才值得占位置。
                    */
                    /*
                        数据库的位置由两部分组成，别混：
                          DataBase.dbPath 是<b>目录</b>（默认 C:\WPE64DB），多开设置改的就是它；
                          DataBase.dbName 是<b>文件名</b>（AssemblyVersion + ".db"，如 2.1.9.db）。
                        真正的库是这两者拼起来。此处曾错用 Path.GetFileName(dbPath)，
                        取到的其实是目录名，界面上把文件夹当成了数据库名。

                        多开时区分实例靠的是<b>目录</b>（dbName 由版本号推导，各实例相同），
                        所以状态栏显示目录名、终端里显示完整路径。
                    */
                    dbDir = dbDir,
                    dbFile = dbFile,
                    dbFull = dbDir.Length > 0 && dbFile.Length > 0
                        ? Path.Combine(dbDir, dbFile)
                        : string.Empty,
                    dbInstance = dbDir.Length > 0
                        ? Path.GetFileName(dbDir.TrimEnd(Path.DirectorySeparatorChar))
                        : string.Empty,
                    /*
                        取<b>主工程</b>的版本号，不是外壳自己的。

                        GetExecutingAssembly() 拿到的是 WPEHybrid（现在还是 1.0.0），
                        而界面上要显示的是产品版本 —— 那个写在 WinsockPacketEditor 的
                        AssemblyInfo 里（当前 2.1.9）。
                        用 typeof(Operate).Assembly 定位，比写死程序集名更抗重构。
                    */
                    version = typeof(Operate).Assembly.GetName().Version.ToString(3),
                    isBeta = Operate.SystemConfig.IsBeta,
                    //界面语言的初值。前端拿它决定首屏用哪份字典，切换后走 setLanguage 写回
                    language = UI.Prefs.Language ?? "zh-CN",
                    /*
                        主题的初值，与语言同一个理由搭这一趟车：
                        它要在<b>任何像素画出来之前</b>定好，否则浅色用户会先看见
                        一帧深色再跳成浅色。不为它单开一次 getPrefs。

                        两个值一起给：themeMode 是用户选的那一档（三态），
                        isDark 是上次解析出来的实际值 —— 跟随系统时前端会用
                        matchMedia 自己重新解析，这个值只在解析不出来时兜底。
                    */
                    themeMode = ThemeMode(),
                    isDark = UI.Prefs.IsDark,
                    scanLine = UI.Prefs.ScanLine,
                    lastInjection = Operate.SystemConfig.LastInjection ?? string.Empty,
                    lastInject = this.LastInjectInfo(),
                    socks5Port = Operate.ProxyConfig.Proxy.SOCKS5_Port,
                    socks5Addr = socks5Addr,
                    httpAddr = httpAddr,
                    /*
                        IP 归属地库的版本与条目数，由 Operate 暴露成 string / int ——
                        外壳因此不用引用 QQWry 程序集（直接读 ipSearch.Version 会 CS0012）。
                        库是懒加载的，自检可能比第一次查 IP 还早，所以空值要容忍。
                    */
                    geoVersion = GeoVersionText(),
                    geoCount = Operate.ProxyConfig.Proxy.GeoDbCount,
                    //true = 拖动由 WebView2 原生处理（CSS app-region），前端不必自己发起
                    nativeDrag = this.nonClientOk,
                };
            });

            /*
                前端挂载完成的信号。

                启动期的弹窗必须等这一刻 —— NavigationCompleted 只说明页面文档到了，
                Vue 还没 mount，此时发 ask 前端没有处理器，只能干等到 5 分钟超时。
                有了这个信号，「启动后自动弹 beta 提示」才有确定的时机。
            */
            this.bridge.Register("uiReady", async args =>
            {
                await this.OnUiReady();
                return new { ok = true };
            });

            this.RegisterLoadGenerator();
        }

        #region//窗口露脸

        private bool revealed;

        /// <summary>
        /// 把窗口显示出来。首屏画完时调，超时也调。
        ///
        /// <b>必须幂等</b>：正常路径与超时路径都会走到这里，谁先到算谁。
        /// </summary>
        private void RevealWindow()
        {
            if (this.revealed || this.IsDisposed)
            {
                return;
            }

            this.revealed = true;

            try
            {
                this.Opacity = 1;
                this.Activate();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(RevealWindow), ex);
            }
        }

        /// <summary>
        /// 兜底：不管前端起没起来，到点就把窗口显示出来。
        ///
        /// 没有这个的话，WebView2 初始化失败或页面加载不出来时窗口会一直隐形 ——
        /// 用户看到的是「双击了没反应」，比看到一个空窗口糟得多。
        /// </summary>
        private void StartRevealTimeout()
        {
            var t = new System.Windows.Forms.Timer { Interval = 4000 };

            t.Tick += (s, e) =>
            {
                t.Stop();
                t.Dispose();

                if (!this.revealed)
                {
                    Operate.DoLog(nameof(StartRevealTimeout), "前端未在 4 秒内就绪，先把窗口显示出来");
                    this.RevealWindow();
                }
            };

            t.Start();
        }

        #endregion

        /// <summary>前端就绪后要做的事：把窗口显示出来，再弹测试版提示。</summary>
        private async Task OnUiReady()
        {
            //先露脸再弹提示 —— 否则弹窗会画在一个还隐形的窗口上
            this.RevealWindow();

            try
            {
                if (!Operate.SystemConfig.IsBeta)
                {
                    return;
                }

                //走 UI.Prompt 而不是 event：它是模态的、要等用户点「知道了」，
                //（当年 WinForms 的测试版提示也是模态的）
                await UI.Prompt<object>("beta-notice", new
                {
                    title = UI.T("BetaVersion", "这是一个测试版程序"),
                    content = UI.T("BetaVersionContent",
                        "测试版程序可能存在未知的 Bug，请谨慎使用！\r\n如需使用正式版，请至官网下载最新发布的程序。"),
                    ok = UI.T("GotIt", "知道了"),
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnUiReady), ex);
            }
        }

        /// <summary>
        /// 归属地库的版本日期，规范成 2025-08-13。
        ///
        /// QQWry 给的原串是「2025年08月13日IP数据」这种中文形式（有些版本前面还带
        /// 「纯真网络 」前缀），界面上只需要那个日期。
        ///
        /// 规范化放在这里而不是 Operate.GeoDbVersion 里：那个属性应当返回<b>原始值</b>，
        /// 它是数据本身；这里是给界面看的适配层，格式化归它管。
        ///
        /// 抓不到日期就<b>原样返回</b> —— 宁可显示一串看不懂的原文，
        /// 也不要显示空白让人误以为库没加载。
        /// </summary>
        private static string GeoVersionText()
        {
            string raw = Operate.ProxyConfig.Proxy.GeoDbVersion;

            if (string.IsNullOrEmpty(raw))
            {
                return "未加载";
            }

            //分隔符不写死成「年月日」，别的版本可能是别的写法（连字符、空格等）
            var m = System.Text.RegularExpressions.Regex.Match(
                raw, @"(\d{4})\D{1,3}(\d{1,2})\D{1,3}(\d{1,2})");

            if (!m.Success)
            {
                return raw;
            }

            return m.Groups[1].Value
                 + "-" + m.Groups[2].Value.PadLeft(2, '0')
                 + "-" + m.Groups[3].Value.PadLeft(2, '0');
        }

        /// <summary>
        /// SOCKS5 的监听地址，形如 192.168.1.10:1080。
        ///
        /// IP 取本机网卡里优先级最高的那个（GetLocalIPAddress 已按接口类型排过序）。
        /// 不用 ProxyConfig.Proxy.ProxyTCP_IP —— 那个要等界面调 InitProxyServerIP
        /// 才有值，外壳这条路上一直是 null。
        /// </summary>
        /// <summary>
        /// 当前生效的数据库全路径。
        ///
        /// <b>dbPath 是目录、dbName 是文件名</b>，两者拼起来才是库。
        /// 曾错用 Path.GetFileName(dbPath) 当库名，取到的其实是文件夹名。
        /// </summary>
        private static string CurrentDbFull()
        {
            try
            {
                string dir = Operate.DataBase.dbPath ?? string.Empty;
                string name = Operate.DataBase.dbName ?? string.Empty;

                return dir.Length > 0 && name.Length > 0 ? Path.Combine(dir, name) : string.Empty;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CurrentDbFull), ex);
                return string.Empty;
            }
        }

        /// <summary>当前库的字节数；文件还没建出来时为 0。</summary>
        private static long CurrentDbSize()
        {
            try
            {
                string full = CurrentDbFull();
                return full.Length > 0 && File.Exists(full) ? new FileInfo(full).Length : 0L;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CurrentDbSize), ex);
                return 0L;
            }
        }

        /// <summary>
        /// 实例名 = 数据库<b>目录</b>的最后一段。
        ///
        /// 多开时区分实例靠的是目录（dbName 由版本号推导，各实例都一样），
        /// 所以状态栏与启动页卡片显示的是目录名而不是文件名。
        /// </summary>
        private static string InstanceName()
        {
            try
            {
                string dir = Operate.DataBase.dbPath ?? string.Empty;
                return dir.Length > 0 ? Path.GetFileName(dir.TrimEnd(Path.DirectorySeparatorChar)) : string.Empty;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InstanceName), ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// SOCKS5 与 HTTP 两个监听地址，<b>一次算出来</b>。
        ///
        /// ⚠️ 两者共用同一个 <c>GetLocalIPAddress()</c>，而它实测 <b>70ms</b>（枚举网卡）——
        /// 各写一个方法各算一遍，等于在启动自检那条路上白付两遍。
        /// 这也是它只能待在一次性路径上的原因，见 CLAUDE.md「拖窗口时每半秒卡一下」。
        ///
        /// HTTP 那个在<b>没启用时返回空串</b>，界面据此显示「未启用」而不是一个连不上的地址。
        /// </summary>
        private static void ProxyAddresses(out string socks5, out string http)
        {
            socks5 = string.Empty;
            http = string.Empty;

            try
            {
                var ips = Operate.SystemConfig.GetLocalIPAddress();
                string ip = ips != null && ips.Length > 0 ? ips[0].ToString() : "0.0.0.0";

                socks5 = ip + ":" + Operate.ProxyConfig.Proxy.SOCKS5_Port;

                //与 InitHttpProxy 那条日志同一个口径（那边打的是 ProxyUDP_IP，起来之后就等于这个 ip）
                if (Operate.ProxyConfig.Proxy.Enable_HTTP)
                {
                    http = ip + ":" + Operate.ProxyConfig.Proxy.HTTP_Port;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ProxyAddresses), ex);
            }
        }


        /// <summary>
        /// 在一份列表里按 Id 找到那一条，改它的启用状态。
        ///
        /// 三份表的主键属性名各不相同（FID / SID / RID），所以取键的方式由调用方传进来。
        /// Id 在 DTO 里是<b>大写无括号</b>的 Guid 字符串（见 FeedRows 的 From_），
        /// 比较时统一按不区分大小写来，免得前端回传时大小写不一致就匹配不上。
        /// </summary>
        /// <summary>取前端回传的选中 Id 数组。</summary>
        /// <summary>
        /// ids 数组读成 long。封包 / 代理两份列表的行键是运行期自增的 long，
        /// 不是 Guid 字符串，所以不能复用上面那个 ReadIds。
        /// </summary>
        private static List<long> ReadLongIds(Newtonsoft.Json.Linq.JObject Args)
        {
            var ids = new List<long>();

            try
            {
                var arr = Args["ids"] as Newtonsoft.Json.Linq.JArray;

                if (arr == null)
                {
                    return ids;
                }

                foreach (var x in arr)
                {
                    ids.Add((long)x);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ReadLongIds), ex);
            }

            return ids;
        }

        private static List<string> ReadIds(Newtonsoft.Json.Linq.JObject Args)
        {
            var ids = new List<string>();

            try
            {
                var arr = Args["ids"] as Newtonsoft.Json.Linq.JArray;

                if (arr == null)
                {
                    return ids;
                }

                foreach (var x in arr)
                {
                    string id = (string)x;
                    if (!string.IsNullOrEmpty(id)) { ids.Add(id); }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ReadIds), ex);
            }

            return ids;
        }

        /// <summary>批量操作的统一回执：改了几条，一条都没改就说清楚。</summary>
        private static object BatchResult(int Count)
        {
            if (Count <= 0)
            {
                return new { ok = false, count = 0, error = UI.T("AccountList.Empty", "请选择账号") };
            }

            UI.Toast(UiIcon.Success, string.Format(UI.T("BatchSuccess", "批量调整完成（{0} 个）"), Count));
            return new { ok = true, count = Count, error = string.Empty };
        }

        /// <summary>取前端回传的批量账号草稿。少一行少一列都当作没有，交给调用方判空。</summary>
        private static List<BatchAccountRow> ReadBatchRows(Newtonsoft.Json.Linq.JObject Args)
        {
            var rows = new List<BatchAccountRow>();

            try
            {
                var arr = Args["rows"] as Newtonsoft.Json.Linq.JArray;

                if (arr == null)
                {
                    return rows;
                }

                foreach (var x in arr)
                {
                    string user = x["UserName"] == null ? null : (string)x["UserName"];
                    string pass = x["Password"] == null ? null : (string)x["Password"];

                    if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
                    {
                        rows.Add(new BatchAccountRow { UserName = user.Trim(), Password = pass.Trim() });
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ReadBatchRows), ex);
            }

            return rows;
        }

        /// <summary>
        /// 取过期时间。不勾「设置过期时间」或解析不出来时给一个远期值 ——
        /// 判定只看 IsExpiry，但存一个合法日期能让排序与显示都不用特判。
        /// </summary>
        private static DateTime ReadExpiry(Newtonsoft.Json.Linq.JObject Args)
        {
            DateTime dt;
            string s = Args["expiryTime"] == null ? null : (string)Args["expiryTime"];

            if (!string.IsNullOrEmpty(s) && DateTime.TryParse(s, out dt))
            {
                return dt;
            }

            return DateTime.Now.AddYears(100);
        }

        private static bool SetEnableById<T>(
            System.ComponentModel.BindingList<T> List,
            string Id,
            Func<T, Guid> GetKey,
            Action<T, bool> SetEnable,
            bool Enable)
        {
            try
            {
                if (List == null || string.IsNullOrEmpty(Id))
                {
                    return false;
                }

                foreach (T item in List)
                {
                    if (string.Equals(GetKey(item).ToString(), Id, StringComparison.OrdinalIgnoreCase))
                    {
                        SetEnable(item, Enable);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(SetEnableById), ex);
            }

            return false;
        }

        /// <summary>代理模式的配置是否已从数据库加载过。</summary>
        private bool proxyLoaded;

        /// <summary>
        /// 注入模式的链路（外壳 ↔ 目标进程里的无头核心）。
        /// 没进过注入模式时是 null。
        /// </summary>
        private WinsockPacketEditor.Ipc.ShellLink injectLink;

        /// <summary>
        /// 按需加载代理模式要用的配置与列表。
        ///
        /// 外壳启动时只调了 <c>LoadSystemConfig_FromDB</c>；端口 / 认证 / 映射 / 名单
        /// 这些在别的表里，WinForms 侧是进 ProxyModeForm 时才加载的。
        ///
        /// <b>只做一次</b>：那几个 <c>Load*_FromDB</c> 加载前不清表
        /// （照「一辈子只调一次」写的），重复调用会把旧行与新行叠在一起。
        /// 多开设置切换实例后也不重来 —— 同一个理由，见 saveInstance 那段说明。
        /// </summary>
        #region//注入模式的链路

        /// <summary>
        /// 注入链路的当前状态，喂给界面顶上那条状态条。
        /// 没附加过就是 idle —— 前端据此显示「选目标」那一屏。
        /// </summary>
        private object InjectStatus()
        {
            var link = this.injectLink;

            if (link == null)
            {
                return new { ok = true, state = "idle", pid = 0, name = "", is64 = false, hooked = false, dropped = 0L, ws1 = false, ws2 = false, msws = false };
            }

            return new
            {
                ok = true,
                state = link.State.ToString().ToLowerInvariant(),
                pid = link.TargetPid,
                name = this.SafeProcessName(link.TargetPid),
                /*
                    ⚠️ <b>这里原来还有一个 module（目标主窗口标题）</b>，2026-09-10 删掉了：
                    它是<b>死字段</b> —— 状态条上那块「窗口」读数窗早就撤了（日志里记了完整的
                    注入记录），前端从此没有任何地方读它，只剩 InjectStatus 接口里一个声明。

                    而它不是白占一个字段那么便宜：这个方法是前端 <b>1 秒一次</b>轮询的，
                    每次都要 Process.GetProcessById + MainWindowTitle ——
                    后者会 EnumWindows 把<b>整个桌面的顶层窗口</b>扫一遍去找那个 pid 的主窗口，
                    而且这一趟跑在<b>UI 线程</b>上（桥的处理器就在 UI 线程）。
                    正是「往被轮询的桥方法里加字段之前先问它要不要碰系统 API」那条。

                    要再显示窗口标题的话：在<b>附加成功那一次</b>取一回存下来即可，
                    它本来也不怎么变（SafeWindowTitle 留着没删）。
                */
                is64 = link.TargetIs64,
                hooked = link.HookInstalled,
                //环满丢掉的条数。界面上要显示「丢弃 N」—— 无声丢包比阻塞更糟
                dropped = link.Dropped,
                ws1 = link.SupportWS1,
                ws2 = link.SupportWS2,
                msws = link.SupportMsWS,
            };
        }

        /// <summary>
        /// 两个字段 → 前端认的那三档。
        ///
        /// 「跟随系统」优先：那一档下 IsDark 只是上次解析出来的快照，
        /// 前端会用 matchMedia 自己重新解析，不该被这个快照反过来定住。
        /// </summary>
        private static string ThemeMode()
        {
            if (UI.Prefs.FollowSystemTheme) { return "system"; }

            return UI.Prefs.IsDark ? "dark" : "light";
        }

        /// <summary>
        /// 前端传来的语言码 → 配置列里存的文化名。
        /// 认不出来的一律回中文，别把脏值写进库。清单见 web/src/i18n/langs.ts。
        /// </summary>
        private static string Normalize(string Lang)
        {
            string s = (Lang ?? string.Empty).Trim().ToLowerInvariant();

            /*
                ⚠️ 繁体要在 zh 之前判，而且不能只看前两位：
                zh-TW / zh-HK / zh-Hant 是繁体，zh / zh-CN / zh-Hans 是简体。
                前端传的是 langs.ts 里的 code（"tw" / "zh"），这里也认完整文化名。
            */
            if (s == "tw" || (s.StartsWith("zh") && (s.Contains("tw") || s.Contains("hk") || s.Contains("mo") || s.Contains("hant"))))
            {
                return "zh-TW";
            }

            if (s.StartsWith("en")) { return "en-US"; }
            if (s.StartsWith("ja")) { return "ja-JP"; }
            if (s.StartsWith("ko")) { return "ko-KR"; }
            if (s.StartsWith("vi")) { return "vi-VN"; }
            if (s.StartsWith("ru")) { return "ru-RU"; }

            return "zh-CN";
        }

        /// <summary>
        /// 取进程名。目标可能刚刚没了，这里绝不能抛 ——
        /// 状态条恰恰是「目标没了」的时候最需要显示的东西。
        /// </summary>

        /*
            附加到目标。pid > -1 = 附加到已运行的进程；否则按 path 挂起启动。

            method 只用来<b>记录</b>（0 进程 · 1 窗体 · 2 文件），不影响怎么附加 ——
            方式 01 与 02 到这一步是同一件事，区别只在目标是怎么挑出来的。
        */
        private async System.Threading.Tasks.Task<object> AttachTarget(int pid, string path, string extraArgs, int method)
        {
            /*
                加载必须在注入<b>之前</b>：link.Attach 之后要把滤镜 / 发送 / 机器人
                三份表的快照推给目标，表没加载推过去就是空的。

                ⚠️ 与 enterProxyMode 一样，加载完要 MarkAllDirty 把 14 份列表推给前端 ——
                这两句原来在 enterInjectMode 里（选目标屏），2026-09-10 挪到这儿，
                理由见那边那段注释。前端的 attachListFeed() 在 InjectView 挂载时就订阅了，
                早于这一刻，所以不会出现「推的时候还没人接」。
            */
            EnsureProxyConfigLoaded();
            FeedPump.MarkAllDirty();


            /*
                这次用的是哪种方式（0 选择进程 · 1 选择窗体 · 2 可执行文件）——
                ⚠️ <b>必须由前端说</b>：方式 01 与 02 最后都是「附加到一个 pid」，
                从参数上分辨不出来，而「快捷注入」要照原样重放，得知道当初点的是哪张卡。
                没给就按 pid 猜一个（挂起启动只能是方式 03）。
            */

            /*
                ⚠️ 业务层兜一次「不能注入自己」。

                界面那半边已经不出自己那一行了（ProcessConfig.GetProcessRows），
                但那是<b>控件属性不是约束</b>那条老规矩管的情形 —— 选窗体那条路
                （用户点到本程序自己的窗口）绕开了进程表，快捷键与将来别的调用方同理。
            */
            if (pid >= 0 && Operate.ProxyConfig.Proxy.IsSelfProcess(pid, null))
            {
                return this.InjectFail(UI.T("Inject.Self", "不能注入到 WPE 自己"));
            }

            string cmdLine = null;

            if (pid < 0)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return this.InjectFail(UI.T("Inject.NoTarget", "没有选择目标"));
                }

                cmdLine = "\"" + path + "\"";
                if (!string.IsNullOrEmpty(extraArgs)) { cmdLine += " " + extraArgs; }
            }

            this.DisposeInjectLink();

            var link = new WinsockPacketEditor.Ipc.ShellLink();
            link.StateChanged += this.OnInjectStateChanged;

            try
            {
                //注入是同步阻塞的（EasyHook 的 Inject/CreateAndInject 就是），丢后台并盖遮罩
                //UI.Busy 只有 Func<T> 的重载（要一个返回值），所以补一个 true
                await UI.Busy(
                    UI.T("Loading", "正在加载..."),
                    () => { link.Attach(pid, path, 15000, cmdLine); return true; });
            }
            catch (Exception ex)
            {
                link.Dispose();

                /*
                    ⚠️ 完整异常进系统日志、界面上只给<b>第一行</b>。

                    EasyHook 抛的那条是「一句英文 + 换行 + 文件名: 长路径」，整段塞进
                    右下角那个提示框既读不完也读不懂（2026-09-10 用户真撞上一次：
                    WPEHook.dll 没进外壳的输出目录）。而排查要的堆栈本来就该去日志里看。
                */
                Operate.DoLog("injectAttach", ex);

                string[] lines = (ex.Message ?? string.Empty).Split(
                    new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string first = lines.Length > 0 ? lines[0] : string.Empty;

                return this.InjectFail(string.Format(
                    UI.T("Inject.AttachFail", "注入失败：{0}"), first), false);
            }

            this.injectLink = link;

            //三份共用列表一变就把快照推给目标（滤镜引擎与两个执行器在那边）
            FeedPump.ListPushed -= this.OnListPushed;
            FeedPump.ListPushed += this.OnListPushed;

            /*
                所有「发包」都要交给目标 —— 套接字句柄是进程私有的，
                在外壳里调 send() 命中的是外壳自己句柄表里碰巧同号的那个东西。

                挂在 PacketConfig.Packet.SendPacket 这一层，一次覆盖四个入口：
                封包编辑、发送编辑 / 发送列表、机器人指令、快捷键。
                与执行器启停在 AttachedLink() 那里分流是同一件事，
                只是那几个的分流点在桥方法上，而这些跑在 Operate 的后台线程里，桥拦不住。
            */
            Operate.PacketConfig.Packet.SendRouter = link.SendPacket;

            this.RememberLastInject(method, path, extraArgs, link.TargetPid);
            this.LogInjectAttached(method, path, extraArgs, link);

            return this.InjectStatus();
        }

        /*
            注入失败的统一出口：**记一条系统日志 + 出 { ok = false }**。

            ⚠️ 以前只有「抛异常」那一支进日志，`不能注入自己` / `没有选择目标`
            这类业务拒绝只回给前端弹一下就没了 —— 而右下角那个提示是会自己消失的，
            用户回头想查「刚才为什么没进去」时什么都找不到。

            alreadyLogged：异常那一支上面已经用 DoLog(…, ex) 记过完整堆栈，别再记第二遍。
        */
        private object InjectFail(string reason, bool logIt = true)
        {
            if (logIt)
            {
                Operate.DoLog("InjectAttach", string.Format(
                    UI.T("Inject.Log.Fail", "注入失败 —— {0}"), reason));
            }

            return new { ok = false, error = reason };
        }

        /*
            注入成功往系统日志记一条 —— 与代理模式启动时那条（「SOCKS5 代理地址 : …」）同一条口径：
            **动作成功了就在日志里留一行能对得上的事实**，而不是只弹一个会消失的提示。

            记的是「目标 + 位数 + 方式 + 路径」四样：出问题时要回答的正是
            「当时注的到底是哪一个进程、怎么进去的」，光一个进程名不够（同名进程能开好几个）。
        */
        private void LogInjectAttached(int method, string path, string extraArgs,
            WinsockPacketEditor.Ipc.ShellLink link)
        {
            try
            {
                string name = !string.IsNullOrEmpty(path)
                    ? System.IO.Path.GetFileName(path)
                    : this.SafeProcessName(link.TargetPid);

                string full = !string.IsNullOrEmpty(path) ? path : this.SafeProcessPath(link.TargetPid);

                string sLog = string.Format(
                    UI.T("Inject.Log.Attached", "已注入目标 [ {0} #{1} · {2} ] 方式 [ {3} ]"),
                    name, link.TargetPid, link.TargetIs64 ? "x64" : "x86", this.MethodName(method));

                if (!string.IsNullOrEmpty(full)) { sLog += " " + full; }
                if (!string.IsNullOrEmpty(extraArgs)) { sLog += " " + extraArgs; }

                Operate.DoLog("InjectAttach", sLog);
            }
            catch (Exception ex) { Operate.DoLog("LogInjectAttached", ex); }
        }

        /*
            方式的名字。⚠️ 三条文案与前端选方式屏那三张卡<b>用同一套说法</b>
            （inject.pick.mProc / inject.pick.window / inject.pick.mFile），
            日志里写「可执行文件」而界面上那张卡叫别的，对不上就没法互相印证。
        */
        private string MethodName(int method)
        {
            if (method == 1) { return UI.T("Inject.Method.Window", "选择窗体"); }
            if (method == 2) { return UI.T("Inject.Method.File", "选择文件"); }
            return UI.T("Inject.Method.Process", "选择进程");
        }

        /*
            记下这一次注入 —— 选目标屏的「上次注入」那一块要显示，「快捷注入」要拿它重放。

            ⚠️ 名字与路径分开存：方式 01 / 02 手上只有一个 pid，路径靠 MainModule 取（取不到就空）；
            方式 03 本来就有完整路径。重放时方式 03 按路径重新挂起启动，01 / 02 按名字找同名进程。
        */
        private void RememberLastInject(int method, string path, string extraArgs, int pid)
        {
            try
            {
                bool byFile = !string.IsNullOrEmpty(path);

                Operate.SystemConfig.LastInjection = byFile
                    ? System.IO.Path.GetFileName(path)
                    : this.SafeProcessName(pid);

                Operate.SystemConfig.LastInjectMethod = method;
                Operate.SystemConfig.LastInjectPath = byFile ? path : this.SafeProcessPath(pid);
                Operate.SystemConfig.LastInjectArgs = extraArgs ?? string.Empty;
                Operate.SystemConfig.LastInjectTime = DateTime.Now.ToString("o");

                Operate.SystemConfig.SaveSystemConfig_LastInjection_ToDB();
            }
            catch (Exception ex) { Operate.DoLog("RememberLastInject", ex); }
        }

        /// <summary>目标的完整路径；取不到（权限 / 已退出 / 位数不同）就返回空串，一条都不许抛。</summary>
        private string SafeProcessPath(int Pid)
        {
            try
            {
                using (System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(Pid))
                {
                    return p.MainModule == null ? string.Empty : (p.MainModule.FileName ?? string.Empty);
                }
            }
            catch { return string.Empty; }
        }

        /*
            「上次注入」那条记录，出给前端。

            ⚠️ 时间在<b>这一侧</b>格式化好（与 19 份列表的 DTO 同一条口径：C# 出串、前端不解析）。
            库里存的是 "o"，万一解析不回来就原样给出去 —— 那也比在界面上印一串 ISO 时间戳强。
        */
        private object LastInjectInfo()
        {
            string raw = Operate.SystemConfig.LastInjectTime ?? string.Empty;
            string shown = string.Empty;

            if (raw.Length > 0)
            {
                DateTime t;
                shown = DateTime.TryParse(raw, null, System.Globalization.DateTimeStyles.RoundtripKind, out t)
                    ? t.ToString("yyyy-MM-dd HH:mm:ss")
                    : raw;
            }

            return new
            {
                target = Operate.SystemConfig.LastInjection ?? string.Empty,
                method = Operate.SystemConfig.LastInjectMethod,
                path = Operate.SystemConfig.LastInjectPath ?? string.Empty,
                args = Operate.SystemConfig.LastInjectArgs ?? string.Empty,
                time = shown,
            };
        }

        /*
            「快捷注入」的目标解析 —— 把上次那条记录还原成一次 AttachTarget 的入参。

            ⚠️ <b>兜底全在这儿</b>，而且要说清是哪一种不成立：
              · 压根没注入过        → 让人先挑一个
              · 方式 03 的文件没了  → 说出路径（可能是盘符变了 / 游戏卸了）
              · 方式 01 / 02 的进程不在了 → 说出名字（多半是关掉了）
            一句笼统的「注入失败」在这儿最没用：这三种的下一步动作完全不同。
        */
        private bool ResolveQuickTarget(out int Pid, out string Path, out string Error)
        {
            Pid = -1;
            Path = null;
            Error = null;

            string name = Operate.SystemConfig.LastInjection ?? string.Empty;
            int method = Operate.SystemConfig.LastInjectMethod;
            string last = Operate.SystemConfig.LastInjectPath ?? string.Empty;

            if (name.Length == 0 && last.Length == 0)
            {
                Error = UI.T("Inject.QuickNone", "还没有注入过，先挑一个目标");
                return false;
            }

            //方式 03：照原样再挂起启动一次，所以文件必须还在
            if (method == 2)
            {
                if (last.Length == 0 || !System.IO.File.Exists(last))
                {
                    Error = string.Format(UI.T("Inject.QuickNoFile", "找不到上次那个可执行文件：{0}"), last);
                    return false;
                }

                Path = last;
                return true;
            }

            /*
                方式 01 / 02：按<b>进程名</b>找一个还活着的同名进程。

                ⚠️ 名字要去掉 .exe —— GetProcessesByName 收的是不带扩展名的那种，
                而方式 03 存进去的名字是带的（同一个字段两种来源，这里统一一次）。
                ⚠️ 跳过自己：与进程表那两道兜底同一个判据，别让「快捷注入」成为第三条漏网的路。
            */
            string bare = name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
                ? name.Substring(0, name.Length - 4)
                : name;

            try
            {
                System.Diagnostics.Process[] found = System.Diagnostics.Process.GetProcessesByName(bare);

                for (int i = 0; i < found.Length; i++)
                {
                    using (System.Diagnostics.Process p = found[i])
                    {
                        if (Pid >= 0) { continue; }
                        if (Operate.ProxyConfig.Proxy.IsSelfProcess(p.Id, null)) { continue; }
                        Pid = p.Id;
                    }
                }
            }
            catch (Exception ex) { Operate.DoLog("ResolveQuickTarget", ex); }

            if (Pid < 0)
            {
                Error = string.Format(UI.T("Inject.QuickNoProc", "「{0}」没有在运行，先把它启动起来"), name);
                return false;
            }

            return true;
        }
        private string SafeProcessName(int pid)
        {
            //⚠️ Process 是 IDisposable —— 不 Dispose 就是每秒攒一个进程句柄等终结器
            //（这个方法在 1 秒一次的 InjectStatus() 里）。隔壁 SafeWindowTitle 一直是对的。
            try { using (var proc = System.Diagnostics.Process.GetProcessById(pid)) { return proc.ProcessName; } }
            catch { return "(" + pid + ")"; }
        }

        /// <summary>
        /// 目标的主窗口标题，没有标题就退回主模块名（照 WinForms 的 GetInjectModuleName）。
        /// 同样一条都不能抛。
        /// </summary>
        private string SafeWindowTitle(int pid)
        {
            try
            {
                using (var proc = System.Diagnostics.Process.GetProcessById(pid))
                {
                    if (!string.IsNullOrEmpty(proc.MainWindowTitle)) { return proc.MainWindowTitle; }

                    try { return proc.MainModule.ModuleName; } catch { return string.Empty; }
                }
            }
            catch { return string.Empty; }
        }

        /// <summary>链路状态变了就推给前端（状态条要即时反映「目标没了」）。</summary>
        private void OnInjectStateChanged(WinsockPacketEditor.Ipc.ShellLink.LinkState state)
        {
            try { this.bridge.PushEvent("inject:state", this.InjectStatus()); }
            catch { /* 页面可能正在导航 */ }
        }

        /// <summary>
        /// 把 Runtime 快照推给目标（附加着才推，没附加是空操作）。
        ///
        /// Runtime 里装的是「改了之后目标那边要立刻跟上」的几样：极速模式、系统套接字、
        /// 列表执行方式、滤镜执行方式、当前选中的封包。它们分散在好几个设置页与右键动作里，
        /// 所以收成这一个方法，改动点只要记得调它就行。
        /// </summary>
        private void PushRuntimeToTarget()
        {
            var link = this.AttachedLink();
            if (link != null) { link.TryPush(link.PushRuntime); }
        }

        /// <summary>
        /// 已经附加好的注入链路；没进注入模式、或链路断了就返回 null。
        ///
        /// 「执行器该在哪儿跑」全项目只看这一个判断：<b>不为 null 就走目标</b>。
        /// </summary>
        private WinsockPacketEditor.Ipc.ShellLink AttachedLink()
        {
            var link = this.injectLink;
            return link != null && link.State == WinsockPacketEditor.Ipc.ShellLink.LinkState.Attached ? link : null;
        }

        /// <summary>
        /// 把快捷键转成命令发给目标。已附加就返回 true（表示这一下已经处理掉了）。
        ///
        /// 映射<b>逐条照 Operate.SystemConfig.DoHotKey</b>：
        /// HotKeyType 0 是发送、1 是机器人；9001..9010 按<b>列表下标</b>取第 1..10 条，
        /// 9011 / 9012 是整表的启动 / 停止。
        /// 这里换成按 GUID 发过去 —— 两侧的表不保证同序，按下标发会执行错东西。
        /// </summary>
        private bool DispatchHotKeyToTarget(int hotKeyId)
        {
            var link = this.injectLink;

            if (link == null || link.State != WinsockPacketEditor.Ipc.ShellLink.LinkState.Attached)
            {
                return false;
            }

            bool robot = Operate.SystemConfig.HotKeyType == 1;

            if (hotKeyId == 9011)
            {
                if (robot) { link.StartRobotList(); } else { link.StartSendList(); }
                return true;
            }

            if (hotKeyId == 9012)
            {
                if (robot) { link.StopRobotList(); } else { link.StopSendList(); }
                return true;
            }

            int index = hotKeyId - 9001;
            if (index < 0 || index > 9) { return true; }

            if (robot)
            {
                var robots = Operate.RobotConfig.List.lstRobotInfo;
                if (index >= robots.Count) { return true; }

                //FilterSocket 传 -1：这一路不是滤镜触发的，与 DoRobot_ByIndex 一致
                link.StartRobot(robots[index].RID, -1);
                return true;
            }

            var sends = Operate.SendConfig.List.lstSendInfo;
            if (index >= sends.Count) { return true; }

            link.StartSend(sends[index].SID);
            return true;
        }

        /// <summary>
        /// 滤镜 / 发送 / 机器人这三份列表一变，目标那边的快照也得跟着换。
        ///
        /// 挂在 FeedPump.ListPushed 上而不是在每个改动点插桩 —— 那三份列表的改动点
        /// 有一百多处、而且并不都在 Operate 里，逐点插桩必然会漏，
        /// 漏了的表现是「外壳上改了、目标里没生效」，最难查的那一类。
        /// </summary>
        private void OnListPushed(FeedList which)
        {
            var link = this.injectLink;

            if (link == null || link.State != WinsockPacketEditor.Ipc.ShellLink.LinkState.Attached)
            {
                return;
            }

            //推快照是同步的管道往返，别占住搬运定时器那一拍
            System.Threading.Tasks.Task.Run(() =>
            {
                switch (which)
                {
                    case FeedList.Filter: link.TryPush(link.PushFilters); break;
                    case FeedList.Send: link.TryPush(link.PushSends); break;
                    case FeedList.Robot: link.TryPush(link.PushRobots); break;
                }
            });
        }

        /// <summary>退出时干净地卸钩断开。异常一律吞掉 —— 关窗路径上没有能补救的东西。</summary>
        private void DetachInjectOnExit()
        {
            //「选择窗体」还开着的话先收掉：全局低级钩子留在一个已经没了的进程上是系统级的麻烦
            this.FinishPickWindow(new { ok = false, cancelled = true });

            var link = this.injectLink;
            if (link == null) { return; }

            try { link.Detach(); }
            catch (Exception ex) { Operate.DoLog(nameof(DetachInjectOnExit), ex); }

            this.DisposeInjectLink();
        }

        #region//「选择窗体」取 PID（对应 WinForms 的 ProcessList.bSelectForm_Click）

        /*
            装 WH_MOUSE_LL + WH_KEYBOARD_LL 两个低级钩子：
            鼠标移动时把光标下那个窗口的进程报给前端，左键按下时定下目标，Esc 取消。

            【为什么委托要用字段存着】SetWindowsHookEx 只记住函数指针，
            托管委托没人引用就会被 GC 掉，然后回调时是个野指针 —— 表现为随机崩溃，
            而且大概率不在这一行崩。WinForms 那边也是用字段存的（mProc / kProc）。

            【钩子回调跑在 UI 线程上】低级钩子是靠安装线程的消息循环派发的，
            所以这里可以直接推事件、直接完成那个 TaskCompletionSource，不用切线程。
            代价是回调必须快 —— 系统有超时（LowLevelHooksTimeout），超了会把钩子踢掉。
        */

        private User32.HookProc pickMouseProc;
        private User32.HookProc pickKeyProc;
        private IntPtr pickMouseHook;
        private IntPtr pickKeyHook;
        private IntPtr pickLastHover;

        /// <summary>「选窗体」时把自己最小化了没有，以及最小化之前是哪一档（可能是最大化）。</summary>
        private bool pickMinimized;
        private FormWindowState pickPrevState = FormWindowState.Normal;
        private System.Threading.Tasks.TaskCompletionSource<object> pickTcs;

        private System.Threading.Tasks.Task<object> PickWindowAsync()
        {
            //已经在选了就把上一次取消掉，免得两套钩子叠着
            this.FinishPickWindow(new { ok = false, cancelled = true });

            var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();
            this.pickTcs = tcs;
            this.pickLastHover = IntPtr.Zero;

            try
            {
                this.pickMouseProc = this.PickMouseHook;
                this.pickKeyProc = this.PickKeyHook;

                using (var proc = System.Diagnostics.Process.GetCurrentProcess())
                using (var mod = proc.MainModule)
                {
                    IntPtr h = Kernel32.GetModuleHandle(mod.ModuleName);
                    this.pickMouseHook = User32.SetWindowsHookEx(User32.WH_MOUSE_LL, this.pickMouseProc, h, 0);
                    this.pickKeyHook = User32.SetWindowsHookEx(User32.WH_KEYBOARD_LL, this.pickKeyProc, h, 0);
                }

                if (this.pickMouseHook == IntPtr.Zero)
                {
                    this.FinishPickWindow(new { ok = false, error = UI.T("Inject.PickFailed", "无法监听鼠标，请以管理员身份运行") });
                }
                else
                {
                    /*
                        ⚠️ 装上钩子<b>之后</b>再把自己最小化。

                        「选窗体」要用户去点<b>别的程序</b>的窗口，而本窗口十有八九正压在它上面 ——
                        不让开就得先手动挪走，那正是这个功能想省掉的一步。

                        ⚠️ 顺序不能反：先最小化再装钩，中间那一小段里用户已经能点别的窗口了，
                        点中的那一下没人接。

                        ⚠️ 还原写在 FinishPickWindow 里，不写在这儿 —— 出口有四条
                        （选中 / Esc 取消 / 前端调 cancelPickWindow / 装钩失败），
                        它们全都汇到那一个方法，只有写在那里才不会漏掉其中一条。
                    */
                    this.pickPrevState = this.WindowState;
                    this.pickMinimized = true;
                    this.WindowState = FormWindowState.Minimized;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(PickWindowAsync), ex);
                this.FinishPickWindow(new { ok = false, error = ex.Message });
            }

            return tcs.Task;
        }

        /// <summary>卸钩 + 把结果交给还在等的那个调用。重复调用无害（第二次没有 pending 就直接返回）。</summary>
        private void FinishPickWindow(object result)
        {
            if (this.pickMouseHook != IntPtr.Zero)
            {
                try { User32.UnhookWindowsHookEx(this.pickMouseHook); } catch { }
                this.pickMouseHook = IntPtr.Zero;
            }

            if (this.pickKeyHook != IntPtr.Zero)
            {
                try { User32.UnhookWindowsHookEx(this.pickKeyHook); } catch { }
                this.pickKeyHook = IntPtr.Zero;
            }

            this.pickMouseProc = null;
            this.pickKeyProc = null;

            /*
                还原窗口。四条出口都会走到这儿，所以只在这一处写。

                ⚠️ 要记住<b>原来是哪一档</b>（可能是最大化），照 Normal 还原会把用户的最大化吃掉。
                ⚠️ Activate() 不能省：用户刚点过别的窗口，那个窗口现在是前台 ——
                只还原不激活的话，界面回来了却压在人家后面，看着像没反应。
            */
            if (this.pickMinimized)
            {
                this.pickMinimized = false;
                try
                {
                    this.WindowState = this.pickPrevState;
                    this.Activate();
                }
                catch (Exception ex) { Operate.DoLog("FinishPickWindow.Restore", ex); }
            }

            var tcs = this.pickTcs;
            this.pickTcs = null;
            if (tcs != null) { tcs.TrySetResult(result); }
        }

        private IntPtr PickMouseHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && this.pickTcs != null)
                {
                    var st = (User32.MSLLHOOKSTRUCT)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(User32.MSLLHOOKSTRUCT));

                    if (wParam == (IntPtr)User32.WM_MOUSEMOVE)
                    {
                        IntPtr hWnd = User32.WindowFromPoint(st.pt);

                        //换了窗口才推一次 —— 鼠标移动每秒几百个事件，逐个推会把桥冲垮
                        if (hWnd != IntPtr.Zero && hWnd != this.pickLastHover)
                        {
                            this.pickLastHover = hWnd;

                            int pid;
                            User32.GetWindowThreadProcessId(hWnd, out pid);

                            try { this.bridge.PushEvent("inject:hover", this.DescribeWindow(pid, hWnd)); }
                            catch { /* 页面可能正在导航 */ }
                        }
                    }
                    else if (wParam == (IntPtr)User32.WM_LBUTTONDOWN)
                    {
                        IntPtr hWnd = User32.WindowFromPoint(st.pt);

                        if (hWnd != IntPtr.Zero)
                        {
                            int pid;
                            User32.GetWindowThreadProcessId(hWnd, out pid);

                            /*
                                点到外壳自己身上：当成「取消」。
                                注入自己毫无意义，而用户点回本窗口本来就是想收手 ——
                                什么都不做的话会像卡住了（WinForms 那边没这一条，是这里补的）。
                            */
                            if (pid == System.Diagnostics.Process.GetCurrentProcess().Id)
                            {
                                this.BeginInvoke((MethodInvoker)delegate { this.FinishPickWindow(new { ok = false, cancelled = true }); });
                            }
                            else
                            {
                                object info = this.DescribeWindow(pid, hWnd);

                                /*
                                    ⚠️ 卸钩不能在回调里就地做 —— 正走在这个钩子链上。
                                    丢回 UI 线程的下一拍，与 WinForms 的 BeginInvoke 同一个理由。
                                */
                                this.BeginInvoke((MethodInvoker)delegate { this.FinishPickWindow(info); });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(PickMouseHook), ex);
            }

            //不吞事件：只吞下按不吞抬起会让被点的程序留在「按住」状态。与 WinForms 一致
            return User32.CallNextHookEx(this.pickMouseHook, nCode, wParam, lParam);
        }

        private IntPtr PickKeyHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && this.pickTcs != null)
                {
                    var kb = (User32.KBDLLHOOKSTRUCT)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(User32.KBDLLHOOKSTRUCT));

                    if (kb.vkCode == (uint)System.Windows.Forms.Keys.Escape)
                    {
                        this.BeginInvoke((MethodInvoker)delegate { this.FinishPickWindow(new { ok = false, cancelled = true }); });
                        return (IntPtr)1;   //Esc 吞掉，别让它落进正被悬停的那个程序
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(PickKeyHook), ex);
            }

            return User32.CallNextHookEx(this.pickKeyHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 光标下那个窗口属于谁。<b>只出基础类型</b>，而且一条都不能抛 ——
        /// 这是在钩子回调里跑的，抛出去系统会把钩子踢掉。
        /// </summary>
        private object DescribeWindow(int pid, IntPtr hWnd)
        {
            string name = string.Empty, path = string.Empty, title = string.Empty;

            try
            {
                var sb = new System.Text.StringBuilder(256);
                if (User32.GetWindowText(hWnd, sb, sb.Capacity) > 0) { title = sb.ToString(); }
            }
            catch { }

            try
            {
                using (var proc = System.Diagnostics.Process.GetProcessById(pid))
                {
                    name = proc.ProcessName;

                    //MainModule 对另一位数 / 更高完整性级别的进程会抛，取不到就空着
                    try { path = proc.MainModule.FileName; } catch { }
                }
            }
            catch { }

            return new { ok = true, pid = pid, name = name, path = path, title = title };
        }

        #endregion

        private void DisposeInjectLink()
        {
            FeedPump.ListPushed -= this.OnListPushed;

            //摘掉发送路由，否则断开之后还会往一条已经关掉的管道上发
            Operate.PacketConfig.Packet.SendRouter = null;

            var link = this.injectLink;
            this.injectLink = null;

            if (link == null) { return; }

            link.StateChanged -= this.OnInjectStateChanged;
            try { link.Dispose(); } catch { }
        }

        #endregion

        private void EnsureProxyConfigLoaded()
        {
            if (this.proxyLoaded)
            {
                return;
            }

            this.proxyLoaded = true;

            try
            {
                /*
                    ⚠️ 注入模式配置<b>必须加载</b>，尽管这一屏用不到它。

                    理由是 saveListSetting 要调 SaveInjectMode_ToDB —— 封包列表的自动清理
                    （PacketList_AutoClear / _Value）存在 InjectMode 那张表里，而
                    SaveInjectMode_ToDB 是「整表删 + 整表插」：不先加载的话，内存里 12 个
                    HookWS* 还是静态初值（全 true），一次保存就把用户在注入模式里关掉的钩子
                    全部重新打开。

                    可以放心重复调用 —— 它是纯字段赋值（核过 3602–3630 行，无 .Add(、无 lst*），
                    与 LoadSystemConfig_FromDB 同性质；那 8 个会往列表里追加的 Load* 才不能重来。
                */
                Operate.SystemConfig.LoadInjectMode_FromDB();

                //顺序与 ProxyModeForm 一致
                Operate.SystemConfig.LoadProxyMode_FromDB();
                Operate.SystemConfig.LoadSystemList_FromDB();
                Operate.WareHouseConfig.List.LoadAutoStores_FromDB();
                Operate.ProxyConfig.Account.LoadProxyAccountList_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapLocal_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapRemote_FromDB();
                Operate.ProxyConfig.Proxy.LoadWhiteList_FromDB();
                Operate.ProxyConfig.Proxy.LoadBlackList_FromDB();

                /*
                    监听地址要有可选的本机 IP，InitProxyServer 会取 ProxyServerIP[0]。

                    ⚠️ <b>丢后台，不占进模式这一下。</b>GetLocalIPAddress() 里是
                    NetworkInterface.GetAllNetworkInterfaces()，本机实测 <b>70~80ms</b>。
                    赋的是一个数组引用，写入是原子的；而 InitProxyServer 本来就有
                    「表为空先自己补一次」的兜底，所以哪怕用户在这 80ms 内就点了「开始代理」也不会出事。
                */
                System.Threading.Tasks.Task.Run(() =>
                {
                    try { Operate.ProxyConfig.Proxy.ProxyServerIP = Operate.SystemConfig.GetLocalIPAddress(); }
                    catch (Exception ex) { Operate.DoLog("EnsureProxyConfigLoaded.IP", ex); }
                });

                /*
                    2026-09-11：发送 / 机器人列表从 BackgroundWorker 换成 Task 之后，
                    不再需要在这里挂 DoWork（Task 自带 body）—— 原来那句 InitListExecute() 删了。
                */

                //全局快捷键要挂在一个窗口句柄上；派发在 WndProc 的 WM_HOTKEY 分支
                Operate.SystemConfig.InitHotKeys(this.Handle);

                /*
                    远程管理要跟着进代理模式一起起来。

                    WinForms 是 ProxyModeForm_Load 里无条件调一次，方法内部再按 IsRemote 决定
                    起不起（见 Operate.cs:1008）。外壳原先只在「远程管理设置 → 保存」时起停，
                    于是勾过「启用」的用户重启外壳后管理台并不在线，而设置页上写着「运行中」。
                */
                Operate.SystemConfig.StartRemoteMGT();

                /*
                    远程管理台首页的 CPU / 内存来自这个性能计数器（SystemInfo_Controller
                    .GetCPUAndMemory 读 SystemConfig.cpuCounter）。不 new 出来那一栏永远是空的。
                    同样是 ProxyModeForm_Load 里顺手做的一件事。

                    它自己就是 async void + Task.Run（Operate.cs:367），所以这里是立刻返回的 ——
                    PerformanceCounter 的构造（几百毫秒起）本来就不在 UI 线程上。
                */
                Operate.SystemConfig.InitCPUAndMemoryCounter();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(EnsureProxyConfigLoaded), ex);
            }
        }

        /// <summary>
        /// 两段字节是否相同。判断封包有没有被滤镜改写过。
        /// PacketInfo 未被改写时 RawBuffer 与 PacketBuffer 常常是<b>同一个数组引用</b>，
        /// 先比引用能省掉绝大多数逐字节比较。
        /// </summary>
        private static bool SameBytes(byte[] A, byte[] B)
        {
            if (ReferenceEquals(A, B)) { return true; }
            if (A == null || B == null) { return false; }
            if (A.Length != B.Length) { return false; }

            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] != B[i]) { return false; }
            }

            return true;
        }

        /// <summary>
        /// 把某几组计数归零 —— 外壳这份清掉，<b>附着着的话还要发到目标</b>。
        ///
        /// ⚠️ <b>注入模式下这些计数的真源全在目标进程里</b>：滤镜六个与每条滤镜的执行次数
        /// 在 DoFilterList 里（目标的钩子线程）、封包那 11 个在 OnPacket 里、
        /// 发送 / 机器人的执行次数在目标的 BackgroundWorker 里。外壳那份只是随 1 Hz 的
        /// Stats 事件更新的<b>镜像</b> —— 只清外壳的，下一拍就被目标盖回去，
        /// 用户看到的是「数字闪一下又回来了」。
        ///
        /// 【为什么收成一个方法】四处入口（统计页归零 / 数据页清空 / 发送与机器人各自的
        /// 重置计数）都要走这一步，散着写必然漏 —— 前两处原来就只清了外壳那半边。
        /// </summary>
        private void ResetCountsEverywhere(WinsockPacketEditor.Ipc.ResetWhat what)
        {
            if ((what & WinsockPacketEditor.Ipc.ResetWhat.FilterStats) != 0)
            {
                Operate.SystemConfig.ResetFilterStats();
            }

            if ((what & WinsockPacketEditor.Ipc.ResetWhat.PacketCounters) != 0)
            {
                //封包计数只有「注入」那一组会在目标里递增，所以这一支固定传 true；
                //代理那一组由 clearPackets 自己 ResetPacketCounters(false)，与目标无关
                Operate.SystemConfig.ResetPacketCounters(true);
            }

            if ((what & WinsockPacketEditor.Ipc.ResetWhat.SendCounts) != 0)
            {
                Operate.SendConfig.List.ResetSendCount();
            }

            if ((what & WinsockPacketEditor.Ipc.ResetWhat.RobotCounts) != 0)
            {
                Operate.RobotConfig.List.ResetRobotCount();
            }

            WinsockPacketEditor.Ipc.ShellLink link = this.AttachedLink();
            if (link == null) { return; }

            try { link.ResetStats(what); }
            catch (Exception ex) { Operate.DoLog(nameof(ResetCountsEverywhere), ex); }
        }

        #region//验收跑测用的工具方法（B10e）

        /*
            以下全是<b>开发/验收工具，不是产品功能</b>，B10 验收结束后整个 region 删掉即可。

            为什么要这些：
            「≥3000 条/秒不掉帧」必须用<b>持续速率</b>去压。一次性把 5 万条倒进队列，
            测到的是「队列排空得多快」（搬运定时器每拍 200 条，约 13000 条/秒），
            那是突发排空速度，不是稳态吞吐 —— 两者的帧时间分布完全不同。

            生成的封包走的是与 Hook 完全相同的下游路径（过滤 → 批量搬运 → DTO → 推送），
            只绕开 PacketInfo_ToQueue：那里面的 IP 归属地异步查询是另一个瓶颈，
            混进来会污染对管线本身的测量。
        */

        /// <summary>灌包线程的停止信号。null 表示没在灌。</summary>
        private volatile bool loadRunning;

        private Thread loadThread;

        private void RegisterLoadGenerator()
        {
            //一次性灌指定条数。用于快速目测，不用于测吞吐。
            this.bridge.Register("devGeneratePackets", args =>
            {
                int count = Clamp(args["count"] == null ? 1000 : (int)args["count"], 1, 200000);
                int size = Clamp(args["size"] == null ? 512 : (int)args["size"], 1, 65536);

                var rnd = new Random(count ^ size);
                for (int i = 0; i < count; i++)
                {
                    Operate.ProxyConfig.Queue.qProxyInfo.Enqueue(MakePacket(i, size, rnd));
                }

                return new { queued = count, size = size };
            });

            //按<b>稳态速率</b>持续灌，直到 devStopLoad。
            this.bridge.Register("devStartLoad", args =>
            {
                int rate = Clamp(args["rate"] == null ? 3000 : (int)args["rate"], 1, 100000);
                int size = Clamp(args["size"] == null ? 512 : (int)args["size"], 1, 65536);

                this.StopLoad();
                this.loadRunning = true;

                this.loadThread = new Thread(() => this.LoadLoop(rate, size))
                {
                    //刻意用后台线程：真实封包也来自后台的 Hook 线程，
                    //放到 UI 线程上灌会和搬运定时器抢同一个线程，测出来的数不作数。
                    IsBackground = true,
                    Name = "WPE-LoadGen",
                };
                this.loadThread.Start();

                return new { rate = rate, size = size };
            });

            this.bridge.Register("devStopLoad", args =>
            {
                this.StopLoad();
                return new { stopped = true };
            });

            //验收时要让列表稳定停在 5000 行做滚动测试，得能临时关掉自动清理。
            this.bridge.Register("devSetAutoClear", args =>
            {
                if (args["on"] != null)
                {
                    Operate.PacketConfig.List.AutoClear = (bool)args["on"];
                }

                if (args["value"] != null)
                {
                    Operate.PacketConfig.List.AutoClear_Value = (int)args["value"];
                }

                return new
                {
                    on = Operate.PacketConfig.List.AutoClear,
                    value = (int)Operate.PacketConfig.List.AutoClear_Value,
                };
            });

            //「内存稳定」要看的是整个进程，不只是 JS 堆。
            this.bridge.Register("devMemory", args =>
            {
                using (var p = System.Diagnostics.Process.GetCurrentProcess())
                {
                    return new
                    {
                        //托管堆。false = 不强制 GC，避免测量动作本身改变被测对象
                        managed = GC.GetTotalMemory(false),
                        workingSet = p.WorkingSet64,
                        privateBytes = p.PrivateMemorySize64,
                        gc0 = GC.CollectionCount(0),
                        gc1 = GC.CollectionCount(1),
                        gc2 = GC.CollectionCount(2),
                    };
                }
            });

            //验收报告落盘，方便留档与比对历次结果。
            this.bridge.Register("devWriteReport", args =>
            {
                string name = args["name"] == null ? "acceptance" : (string)args["name"];
                string text = args["text"] == null ? string.Empty : (string)args["text"];

                //只取文件名部分，防止前端传进来的字符串跑出目录
                name = Path.GetFileNameWithoutExtension(name);
                if (string.IsNullOrEmpty(name)) { name = "acceptance"; }

                string dir = Path.GetDirectoryName(Application.ExecutablePath) ?? ".";
                string path = Path.Combine(dir, name + ".txt");

                File.WriteAllText(path, text, System.Text.Encoding.UTF8);
                return new { path = path };
            });
        }

        private static int Clamp(int V, int Lo, int Hi)
        {
            return V < Lo ? Lo : (V > Hi ? Hi : V);
        }

        private void StopLoad()
        {
            this.loadRunning = false;

            Thread t = this.loadThread;
            this.loadThread = null;

            if (t != null && t.IsAlive)
            {
                t.Join(1000);
            }
        }

        /// <summary>
        /// 稳态灌包。
        ///
        /// 按「到此刻为止<b>累计</b>应该产出多少条」来补差，而不是「每拍固定产出 N 条」：
        /// Thread.Sleep(1) 的实际睡眠受系统计时器精度影响（通常 1–15ms），
        /// 按拍数乘法算会让实际速率随机偏低；按累计时间补差则无论怎么抖，
        /// 平均速率都收敛到目标值。
        /// </summary>
        private void LoadLoop(int Rate, int Size)
        {
            try
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var rnd = new Random(Rate ^ Size);
                long produced = 0;
                int i = 0;

                while (this.loadRunning)
                {
                    long due = (long)(sw.Elapsed.TotalSeconds * Rate);
                    int n = (int)(due - produced);

                    //单次补差设个上限：万一线程被长时间挂起，别一口气灌出几十万条
                    if (n > Rate) { n = Rate; }

                    for (int k = 0; k < n; k++)
                    {
                        Operate.ProxyConfig.Queue.qProxyInfo.Enqueue(MakePacket(i++, Size, rnd));
                    }

                    produced += n;
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(LoadLoop), ex);
            }
        }

        /// <summary>
        /// 造一条代理数据。
        ///
        /// 造的是 <b>ProxyInfo 而不是 PacketInfo</b>：外壳服务的是代理模式，
        /// 它的主列表是 lstProxyInfo（对应 WinForms 的 Controls/ProxyList.cs）；
        /// PacketInfo 那份是注入模式的，而注入模式不用 Vue 界面。
        /// 灌错列表的话压测是自洽的（自己灌自己收），但测的是一条产品里用不到的路径。
        /// </summary>
        private static ProxyInfo MakePacket(int I, int Size, Random Rnd)
        {
            byte[] buf = new byte[Size];
            Rnd.NextBytes(buf);

            //掺进几种 FilterAction，让行着色这条路径也参与测量
            Operate.FilterConfig.Filter.FilterAction action;
            switch (I % 16)
            {
                case 3: action = Operate.FilterConfig.Filter.FilterAction.Replace; break;
                case 7: action = Operate.FilterConfig.Filter.FilterAction.Intercept; break;
                case 11: action = Operate.FilterConfig.Filter.FilterAction.Change; break;
                default: action = Operate.FilterConfig.Filter.FilterAction.NoModify_Display; break;
            }

            return new ProxyInfo(
                DateTime.Now,
                1000 + (I % 16),
                20000 + (I % 64),
                (I % 2) == 0
                    ? Operate.PacketConfig.Packet.PacketType.TCP_Req
                    : Operate.PacketConfig.Packet.PacketType.TCP_Resp,
                0,
                "192.168.1.100:" + (10000 + (I % 100)),
                "局域网",
                "203.0.113." + (I % 255) + ":443",
                "美国",
                "cdn" + (I % 32) + ".example.com",
                Operate.ProxyConfig.Proxy.DomainType.Socket,
                buf,
                buf,
                Operate.PacketConfig.Packet.GetPacketData_Hex(
                    buf, Operate.PacketConfig.Packet.PacketData_MaxLen),
                Size,
                action);
        }

        #endregion

        #endregion

        #region//安全

        private static HashSet<string> BuildAllowedHosts()
        {
            var hosts = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { VirtualHost };

#if DEBUG
            hosts.Add("localhost");
            hosts.Add("127.0.0.1");
#endif

            return hosts;
        }

        private void OnNavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            Uri u;

            if (Uri.TryCreate(e.Uri, UriKind.Absolute, out u) && !AllowedHosts.Contains(u.Host))
            {
                e.Cancel = true;
                Operate.DoLog(nameof(OnNavigationStarting), "已拦截跳转: " + e.Uri);
            }
        }

        #endregion
    }
}
