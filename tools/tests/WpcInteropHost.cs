// WPC 控制通道的联调宿主（2026-09-14）：起一个真的 WPE SOCKS5 服务（带一个测试账号）和一个 TCP 回显靶子，然后等着 ——
// 让另一个进程里的客户端连过来跑真协议。目前的用户是 WPEProxyCap.Android 的 InteropTest（JVM 单元测试），
// 以后改控制通道协议时，Windows 版 WPC 也可以对着它联调。
//
// 编译进产品输出目录运行（依赖按目录解析，与 Socks5Proxy.cs 同一种方式）：
//   set VS=<vswhere -latest -property installationPath>
//   cd WinsockPacketEditor\bin\Release
//   "%VS%\MSBuild\Current\Bin\Roslyn\csc.exe" -nologo -out:WpcInteropHost.exe -r:WinsockPacketEditor.exe ^
//       -r:SuperSocket.SocketBase.dll -r:System.Net.dll ..\..\..\tools\tests\WpcInteropHost.cs
//   copy /y WinsockPacketEditor.exe.config WpcInteropHost.exe.config
//   WpcInteropHost.exe [秒数，默认 300] [--stdin]   （跑完删掉 WpcInteropHost.exe / .exe.config，别让它们进发布目录）
//
// 启动后打印一行：READY socks=<端口> echo=<端口>，之后客户端连 127.0.0.1:<socks>。
// 账号 interop / pw，限 1 台设备。到时间就停服务、删临时库；带 --stdin 时标准输入关闭也会提前停。
// 数据库指向临时目录（不碰真库）；只听 127.0.0.1；不起 HTTP 代理、不碰驱动。
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinsockPacketEditor;

static class WpcInteropHost
{
    static int FreePort()
    {
        TcpListener l = new TcpListener(IPAddress.Loopback, 0);
        l.Start();
        int p = ((IPEndPoint)l.LocalEndpoint).Port;
        l.Stop();
        return p;
    }

    /// <summary>回显靶子：每条连接收到什么就原样发回去，直到对端关闭。</summary>
    static TcpListener StartEcho()
    {
        TcpListener l = new TcpListener(IPAddress.Loopback, 0);
        l.Start();
        Task.Run(async () =>
        {
            while (true)
            {
                Socket s;
                try { s = await l.AcceptSocketAsync(); } catch { return; }
                _ = Task.Run(() =>
                {
                    try
                    {
                        byte[] buf = new byte[8192];
                        int n;
                        while ((n = s.Receive(buf)) > 0) { s.Send(buf, 0, n, SocketFlags.None); }
                    }
                    catch { }
                    finally { try { s.Close(); } catch { } }
                });
            }
        });
        return l;
    }

    [STAThread]
    static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;   //ProxyConfig.Proxy 的静态构造按工作目录找 qqwry.dat

        int seconds = 300;
        if (args.Length > 0) { int.TryParse(args[0], out seconds); }

        string tmp = Path.Combine(Path.GetTempPath(), "wpe-interop-" + Guid.NewGuid().ToString("N").Substring(0, 8));
        Directory.CreateDirectory(tmp);
        Operate.DataBase.dbPath = tmp;
        Operate.DataBase.InitDB();

        TcpListener echo = null;
        try
        {
            int port = FreePort();

            Operate.ProxyConfig.Proxy.Enable_HTTP = false;
            Operate.ProxyConfig.Proxy.Enable_SOCKS5 = true;
            Operate.ProxyConfig.Proxy.Enable_ExternalProxy = false;
            Operate.ProxyConfig.Proxy.ProxyIP_Auto = false;
            Operate.ProxyConfig.Proxy.ProxyIP = "127.0.0.1";
            Operate.ProxyConfig.Proxy.SOCKS5_Port = (ushort)port;
            Operate.ProxyConfig.Proxy.Enable_Auth = true;
            Operate.ProxyConfig.Proxy.MaxConnectionNumber = Operate.ProxyConfig.Proxy.DefaultMaxConnectionNumber;
            Operate.ProxyConfig.Proxy.EnableFireWall = false;

            //账号 interop：不限链接数、限 1 台设备、不过期
            Operate.ProxyConfig.Account.lstAccountInfo.Add(new AccountInfo(
                Guid.NewGuid(), true, "interop", Operate.SystemConfig.PassWord_Encrypt("pw"), new BindingList<AccountIPInfo>(),
                false, 1, true, 1, false, DateTime.MaxValue, DateTime.Now));

            if (!Operate.ProxyConfig.Proxy.StartProxy())
            {
                Console.WriteLine("FAILED StartProxy");
                return 1;
            }

            echo = StartEcho();
            int echoPort = ((IPEndPoint)echo.LocalEndpoint).Port;

            Console.WriteLine("READY socks=" + port + " echo=" + echoPort);
            Console.Out.Flush();

            /*
                等：到时间为止。带 --stdin 时，调用方把标准输入关掉也会提前结束。
                ⚠️ 「标准输入关闭就停」不能默认开：后台跑（没有终端、没有管道）时标准输入一上来就是结束状态，
                   宿主会打完 READY 立刻 STOPPING（2026-09-14 实测撞到）。
            */
            bool watchStdin = Array.IndexOf(args, "--stdin") >= 0;
            var stdinClosed = watchStdin
                ? Task.Run(() => { try { while (Console.In.Read() >= 0) { } } catch { } })
                : new TaskCompletionSource<bool>().Task;
            var deadline = Task.Delay(TimeSpan.FromSeconds(seconds <= 0 ? 300 : seconds));

            //每秒打一次拍：外壳里 OnStatTick 做的两件事（设备表、控制连接判活），宿主里得自己做
            using (var tick = new Timer(_ =>
            {
                try
                {
                    Operate.ProxyConfig.Account.RefreshAuthList().GetAwaiter().GetResult();
                    Operate.WPCConfig.Device.SweepControlSessions(DateTime.Now);
                }
                catch { }
            }, null, 1000, 1000))
            {
                Task.WaitAny(stdinClosed, deadline);
            }

            Console.WriteLine("STOPPING");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("EXCEPTION " + ex);
            return 1;
        }
        finally
        {
            try { echo?.Stop(); } catch { }
            try { Operate.ProxyConfig.Proxy.StopProxy(); } catch { }
            try { Directory.Delete(tmp, true); } catch { }
        }
    }
}
