// SOCKS5 代理服务（SuperSocket 那一层）的回归跑测 —— 2026-09-14「第一组」改动的自动化验证：
//   ① 启动预分配：默认 5000 × 16 KB，GC 堆增量必须远小于老值（老值 20000 × 64 KB 是 1.27 GB）
//   ② 最大连接数按本机内存封顶（NormalizeMaxConnection / MaxConnectionCap / PreallocBytes）
//   ③ 握手无匹配方法回 05 FF 并关闭（RFC 1928；老代码什么都不回，连接挂着）
//   ④ 密码错回 01 01
//   ⑤ 认证 + CONNECT（IPv4）+ 大流量下行、客户端慢读：一个字节都不能错位（老代码 TrySend 队列满就丢）
//   ⑥ IPv6 目标（老代码目标套接字写死 IPv4）
//   ⑦ 两头套接字都关了 Nagle（NoDelay）
//   ⑧ 关掉会话快照后 GetAllSessions 立刻能看到新会话（老代码要等 5 秒一拍）
//   ⑨ 不钩响应（HookTCP_Resp=false）时目标的后续数据仍然转发（老代码只转第一块）
//   ⑩ 问候与认证粘在一个报文里也能过（既有行为，别改坏）
//
// 编译进产品输出目录运行（依赖按目录解析）：
//   set VS=<vswhere -latest -property installationPath>
//   cd WinsockPacketEditor\bin\Release
//   "%VS%\MSBuild\Current\Bin\Roslyn\csc.exe" -nologo -out:Socks5Proxy.exe -r:WinsockPacketEditor.exe ^
//       -r:SuperSocket.SocketBase.dll -r:System.Net.dll ..\..\..\tools\tests\Socks5Proxy.cs
//   copy /y WinsockPacketEditor.exe.config Socks5Proxy.exe.config
//   Socks5Proxy.exe            （跑完删掉 Socks5Proxy.exe / .exe.config，别让它们进发布目录）
//
// 数据库指向临时目录（不碰真库）；只听 127.0.0.1 / ::1；不起 HTTP 代理、不碰驱动。
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using WinsockPacketEditor;

static class T
{
    static int pass = 0, fail = 0, port;
    static void Check(string name, bool ok, string detail = "")
    {
        if (ok) { pass++; Console.WriteLine("  PASS  " + name + (detail.Length > 0 ? "   [" + detail + "]" : "")); }
        else { fail++; Console.WriteLine("  FAIL  " + name + (detail.Length > 0 ? "   [" + detail + "]" : "")); }
    }

    static int FreePort(IPAddress ip)
    {
        TcpListener l = new TcpListener(ip, 0);
        l.Start();
        int p = ((IPEndPoint)l.LocalEndpoint).Port;
        l.Stop();
        return p;
    }

    // ———— 一个最小的 SOCKS5 客户端：只做本跑测需要的几步 ————

    static TcpClient Dial()
    {
        TcpClient c = new TcpClient(AddressFamily.InterNetwork);
        c.NoDelay = true;
        c.Connect(IPAddress.Loopback, port);
        c.ReceiveTimeout = 10000;
        c.SendTimeout = 10000;
        return c;
    }

    static byte[] ReadN(NetworkStream s, int n)
    {
        byte[] b = new byte[n];
        int got = 0;
        while (got < n)
        {
            int r = s.Read(b, got, n - got);
            if (r <= 0) { throw new IOException("对端关闭，只收到 " + got + "/" + n); }
            got += r;
        }
        return b;
    }

    static void Write(NetworkStream s, params byte[] b) { s.Write(b, 0, b.Length); }

    static byte[] AuthPacket(string u, string p)
    {
        byte[] ub = Encoding.UTF8.GetBytes(u), pb = Encoding.UTF8.GetBytes(p);
        List<byte> l = new List<byte> { 0x01, (byte)ub.Length };
        l.AddRange(ub); l.Add((byte)pb.Length); l.AddRange(pb);
        return l.ToArray();
    }

    /// <summary>问候 + 认证 + CONNECT，返回已经处于转发阶段的流；失败抛异常。</summary>
    static NetworkStream Open(TcpClient c, IPAddress target, int targetPort, string user = "wpe", string pass = "s3cret")
    {
        NetworkStream s = c.GetStream();
        Write(s, 0x05, 0x01, 0x02);
        byte[] r = ReadN(s, 2);
        if (r[0] != 0x05 || r[1] != 0x02) { throw new Exception("问候应答不对: " + BitConverter.ToString(r)); }

        byte[] a = AuthPacket(user, pass);
        s.Write(a, 0, a.Length);
        r = ReadN(s, 2);
        if (r[1] != 0x00) { throw new Exception("认证被拒: " + BitConverter.ToString(r)); }

        List<byte> cmd = new List<byte> { 0x05, 0x01, 0x00 };
        byte[] ab = target.GetAddressBytes();
        cmd.Add(target.AddressFamily == AddressFamily.InterNetworkV6 ? (byte)0x04 : (byte)0x01);
        cmd.AddRange(ab);
        cmd.Add((byte)(targetPort >> 8)); cmd.Add((byte)(targetPort & 0xFF));
        s.Write(cmd.ToArray(), 0, cmd.Count);
        r = ReadN(s, 10);
        if (r[1] != 0x00) { throw new Exception("CONNECT 应答: " + BitConverter.ToString(r)); }
        return s;
    }

    // ———— 靶子：收到任何字节后按脚本回数据 ————

    /// <summary>
    /// 回显靶子。<paramref name="script"/> 决定收到第一个字节后怎么回：
    /// 返回一串「数据块」，靶子按顺序发，块之间可以停一会儿（delayMs）。
    /// </summary>
    static TcpListener Target(IPAddress ip, Func<byte[], List<byte[]>> script, int delayMs = 0)
    {
        TcpListener l = new TcpListener(ip, 0);
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
                        byte[] buf = new byte[4096];
                        int n = s.Receive(buf);
                        if (n <= 0) { s.Close(); return; }
                        foreach (byte[] chunk in script(buf.Take(n).ToArray()))
                        {
                            s.Send(chunk);
                            if (delayMs > 0) { Thread.Sleep(delayMs); }
                        }
                        s.Shutdown(SocketShutdown.Send);
                        Thread.Sleep(500);
                        s.Close();
                    }
                    catch { try { s.Close(); } catch { } }
                });
            }
        });
        return l;
    }

    // ———— WPC 控制帧：'W' 01 type len(u16 BE) payload ————

    static void WriteFrame(NetworkStream s, byte type, string json)
    {
        byte[] p = Encoding.UTF8.GetBytes(json);
        byte[] f = new byte[5 + p.Length];
        f[0] = 0x57; f[1] = 0x01; f[2] = type; f[3] = (byte)(p.Length >> 8); f[4] = (byte)(p.Length & 0xFF);
        Buffer.BlockCopy(p, 0, f, 5, p.Length);
        s.Write(f, 0, f.Length);
    }

    static byte[] ReadFrame(NetworkStream s, out byte type)
    {
        byte[] h = ReadN(s, 5);
        if (h[0] != 0x57 || h[1] != 0x01) { throw new Exception("控制帧头不对: " + BitConverter.ToString(h)); }
        type = h[2];
        int len = (h[3] << 8) | h[4];
        return len == 0 ? new byte[0] : ReadN(s, len);
    }

    static string Register(NetworkStream s, string user, string pass, string device, string version)
    {
        WriteFrame(s, 0x01, "{\"user\":\"" + user + "\",\"pass\":\"" + pass + "\",\"device\":\"" + device + "\",\"version\":\"" + version + "\",\"client\":\"WPC\"}");
        byte[] body = ReadFrame(s, out byte type);
        if (type != 0x81) { throw new Exception("注册应答类型不对: " + type); }
        return Encoding.UTF8.GetString(body);
    }

    /// <summary>从扁平 JSON 里抠一个字段（数字或字符串），够本跑测用。</summary>
    static string Field(string json, string name)
    {
        int i = json.IndexOf("\"" + name + "\":", StringComparison.Ordinal);
        if (i < 0) { return ""; }
        i += name.Length + 3;
        if (json[i] == '"') { int j = json.IndexOf('"', i + 1); return json.Substring(i + 1, j - i - 1); }
        int k = i; while (k < json.Length && (char.IsDigit(json[k]) || json[k] == '-')) { k++; }
        return json.Substring(i, k - i);
    }

    static byte[] Pattern(int len, int seed)
    {
        byte[] b = new byte[len];
        for (int i = 0; i < len; i++) { b[i] = (byte)((i * 31 + seed) & 0xFF); }
        return b;
    }

    static bool IsPattern(byte[] b, int len, int seed)
    {
        if (b.Length != len) { return false; }
        for (int i = 0; i < len; i++) { if (b[i] != (byte)((i * 31 + seed) & 0xFF)) { return false; } }
        return true;
    }

    [STAThread]
    static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;   //ProxyConfig.Proxy 的静态构造按工作目录找 qqwry.dat

        string tmp = Path.Combine(Path.GetTempPath(), "wpe-socks5-" + Guid.NewGuid().ToString("N").Substring(0, 8));
        Directory.CreateDirectory(tmp);
        Operate.DataBase.dbPath = tmp;
        Operate.DataBase.InitDB();

        try
        {
            port = FreePort(IPAddress.Loopback);

            var P = typeof(Operate.ProxyConfig.Proxy);   //只是让下面短一点
            Operate.ProxyConfig.Proxy.Enable_HTTP = false;
            Operate.ProxyConfig.Proxy.Enable_SOCKS5 = true;
            Operate.ProxyConfig.Proxy.Enable_ExternalProxy = false;
            Operate.ProxyConfig.Proxy.ProxyIP_Auto = false;
            Operate.ProxyConfig.Proxy.ProxyIP = "127.0.0.1";
            Operate.ProxyConfig.Proxy.SOCKS5_Port = (ushort)port;
            Operate.ProxyConfig.Proxy.Enable_Auth = true;
            Operate.ProxyConfig.Proxy.MaxConnectionNumber = Operate.ProxyConfig.Proxy.DefaultMaxConnectionNumber;
            Operate.ProxyConfig.Proxy.EnableFireWall = false;

            Operate.ProxyConfig.Account.lstAccountInfo.Add(new AccountInfo(
                Guid.NewGuid(), true, "wpe", Operate.SystemConfig.PassWord_Encrypt("s3cret"), new BindingList<AccountIPInfo>(),
                false, 1, false, 1, false, DateTime.MaxValue, DateTime.Now));

            //② 封顶口径（不依赖服务起没起）
            int cap = Operate.ProxyConfig.Proxy.MaxConnectionCap();
            Check("② MaxConnectionCap ≥ 默认值且有限", cap >= Operate.ProxyConfig.Proxy.DefaultMaxConnectionNumber && cap < int.MaxValue, cap.ToString());
            Check("② NormalizeMaxConnection 收进上限 / 非法回默认",
                Operate.ProxyConfig.Proxy.NormalizeMaxConnection(int.MaxValue) == cap
                && Operate.ProxyConfig.Proxy.NormalizeMaxConnection(0) == Operate.ProxyConfig.Proxy.DefaultMaxConnectionNumber
                && Operate.ProxyConfig.Proxy.NormalizeMaxConnection(1234) == 1234);
            Check("② PreallocBytes(5000) = 5000 × 16 KB", Operate.ProxyConfig.Proxy.PreallocBytes(5000) == 5000L * 16 * 1024);
            Check("② 预留封顶 ≤ 1.5 GB", Operate.ProxyConfig.Proxy.MaxConnectionPreallocCap() <= (long)(1.5 * 1024 * 1024 * 1024));

            //① 启动预分配
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            long gc0 = GC.GetTotalMemory(true);
            bool started = Operate.ProxyConfig.Proxy.StartProxy();
            long gcDelta = GC.GetTotalMemory(false) - gc0;
            Check("① StartProxy 起得来", started && Operate.ProxyConfig.Proxy.IsRunning, "127.0.0.1:" + port);
            Check("① 启动预分配 < 120 MB（5000 × 16 KB ≈ 80 MB）", gcDelta < 120L * 1024 * 1024, (gcDelta / 1048576.0).ToString("F1") + " MB");
            Check("① 生效的接收缓冲是 16 KB", Operate.ProxyConfig.Proxy.ProxyReceiveBufferSize == 16 * 1024, Operate.ProxyConfig.Proxy.ProxyReceiveBufferSize.ToString());

            if (!started) { return Finish(); }

            //③ 无匹配方法 → 05 FF 并关闭
            using (TcpClient c = Dial())
            {
                NetworkStream s = c.GetStream();
                Write(s, 0x05, 0x01, 0x00);          //服务端要求认证，客户端只给 NOAUTH
                byte[] r = ReadN(s, 2);
                bool closed;
                try { closed = s.Read(new byte[1], 0, 1) == 0; } catch { closed = true; }
                Check("③ 无匹配方法回 05 FF 并关闭", r[0] == 0x05 && r[1] == 0xFF && closed, BitConverter.ToString(r));
            }

            //④ 密码错
            using (TcpClient c = Dial())
            {
                NetworkStream s = c.GetStream();
                Write(s, 0x05, 0x01, 0x02);
                ReadN(s, 2);
                byte[] a = AuthPacket("wpe", "wrong");
                s.Write(a, 0, a.Length);
                byte[] r = ReadN(s, 2);
                Check("④ 密码错回 01 01", r[0] == 0x01 && r[1] == 0x01, BitConverter.ToString(r));
            }

            //⑩ 问候与认证粘在一起
            using (TcpClient c = Dial())
            {
                NetworkStream s = c.GetStream();
                List<byte> glued = new List<byte> { 0x05, 0x01, 0x02 };
                glued.AddRange(AuthPacket("wpe", "s3cret"));
                s.Write(glued.ToArray(), 0, glued.Count);
                byte[] r = ReadN(s, 4);
                Check("⑩ 问候 + 认证粘包也能过", r[0] == 0x05 && r[1] == 0x02 && r[2] == 0x01 && r[3] == 0x00, BitConverter.ToString(r));
            }

            //⑤ 大流量下行 + 慢读客户端：8 MB，一个字节都不能错位
            const int BIG = 8 * 1024 * 1024;
            TcpListener big = Target(IPAddress.Loopback, first => new List<byte[]> { Pattern(BIG, 7) });
            using (TcpClient c = Dial())
            {
                c.ReceiveBufferSize = 8 * 1024;      //把客户端接收窗口压小，逼服务端的发送队列堆起来
                NetworkStream s = Open(c, IPAddress.Loopback, ((IPEndPoint)big.LocalEndpoint).Port);

                //⑦ 顺便看两头的 NoDelay，⑧ 看会话表是不是立刻可见
                Thread.Sleep(150);
                var sessions = Operate.ProxyConfig.Proxy.ProxyServer.GetAllSessions().ToList();
                Check("⑧ 关快照后 GetAllSessions 立刻看到会话", sessions.Count >= 1, sessions.Count.ToString());
                ProxySession ps = sessions.OrderByDescending(x => x.StartTime).FirstOrDefault();
                Check("⑦ 客户端侧套接字 NoDelay", ps != null && ps.SocketSession.Client.NoDelay);
                Check("⑦ 目标侧套接字 NoDelay 且地址族 IPv4", ps != null && ps.TargetSocket != null && ps.TargetSocket.NoDelay && ps.TargetSocket.AddressFamily == AddressFamily.InterNetwork);

                Write(s, 0x01);
                MemoryStream got = new MemoryStream();
                byte[] buf = new byte[4096];
                s.ReadTimeout = 20000;
                try
                {
                    while (got.Length < BIG)
                    {
                        int n = s.Read(buf, 0, buf.Length);
                        if (n <= 0) { break; }
                        got.Write(buf, 0, n);
                        if ((got.Length / 4096) % 64 == 0) { Thread.Sleep(1); }   //慢读
                    }
                }
                catch (Exception ex) { Console.WriteLine("        读中断: " + ex.Message); }
                Check("⑤ 8 MB 下行经慢读客户端逐字节一致", IsPattern(got.ToArray(), BIG, 7), got.Length + " / " + BIG);
            }
            big.Stop();

            //⑥ IPv6 目标
            try
            {
                TcpListener v6 = Target(IPAddress.IPv6Loopback, first => new List<byte[]> { Pattern(1000, 3) });
                using (TcpClient c = Dial())
                {
                    NetworkStream s = Open(c, IPAddress.IPv6Loopback, ((IPEndPoint)v6.LocalEndpoint).Port);
                    Write(s, 0x01);
                    byte[] r = ReadN(s, 1000);
                    Check("⑥ IPv6 目标连得上、数据对", IsPattern(r, 1000, 3));
                }
                v6.Stop();
            }
            catch (Exception ex) { Check("⑥ IPv6 目标连得上、数据对", false, ex.Message); }

            //⑨ 不钩响应时后续数据块仍然转发
            Operate.ProxyConfig.Proxy.HookTCP_Resp = false;
            try
            {
                TcpListener multi = Target(IPAddress.Loopback, first => new List<byte[]> { Pattern(100, 1), Pattern(100, 2), Pattern(100, 3) }, 120);
                using (TcpClient c = Dial())
                {
                    NetworkStream s = Open(c, IPAddress.Loopback, ((IPEndPoint)multi.LocalEndpoint).Port);
                    Write(s, 0x01);
                    byte[] r = ReadN(s, 300);
                    Check("⑨ HookTCP_Resp=false 时三块数据都到", IsPattern(r.Take(100).ToArray(), 100, 1) && IsPattern(r.Skip(100).Take(100).ToArray(), 100, 2) && IsPattern(r.Skip(200).ToArray(), 100, 3));
                }
                multi.Stop();
            }
            finally { Operate.ProxyConfig.Proxy.HookTCP_Resp = true; }

            //⑪ CONNECT 与数据粘在一个报文里（客户端不等应答就发）：数据必须原样到目标（老代码在命令步 await 期间把它当命令帧解析后清掉）
            {
                TcpListener echo = Target(IPAddress.Loopback, first => new List<byte[]> { first });
                using (TcpClient c = Dial())
                {
                    NetworkStream s = c.GetStream();
                    Write(s, 0x05, 0x01, 0x02);
                    ReadN(s, 2);
                    byte[] a = AuthPacket("wpe", "s3cret");
                    s.Write(a, 0, a.Length);
                    ReadN(s, 2);

                    int tp = ((IPEndPoint)echo.LocalEndpoint).Port;
                    List<byte> glued = new List<byte> { 0x05, 0x01, 0x00, 0x01, 127, 0, 0, 1, (byte)(tp >> 8), (byte)(tp & 0xFF) };
                    glued.AddRange(Pattern(100, 9));
                    s.Write(glued.ToArray(), 0, glued.Count);

                    byte[] rep = ReadN(s, 10);
                    bool ok = rep[1] == 0x00;
                    byte[] back = ok ? ReadN(s, 100) : new byte[0];
                    Check("⑪ CONNECT + 数据粘包：数据原样到目标并回显", ok && IsPattern(back, 100, 9), BitConverter.ToString(rep, 0, 2));
                }
                echo.Stop();
            }

            //⑫ 认证步塞垃圾：超过上限就该被关掉，而不是一直攒到空闲超时
            using (TcpClient c = Dial())
            {
                NetworkStream s = c.GetStream();
                Write(s, 0x05, 0x01, 0x02);
                ReadN(s, 2);
                byte[] junk = new byte[4096];
                junk[0] = 0x01; junk[1] = 0xFF; junk[257] = 0xFF;   //ulen=255、plen=255 → 一帧 513 字节，之后全是多余的
                //先只发 200 字节凑不成帧，再发剩下的：过滤器合并后应切出 513 字节的帧（密码错 → 01 01），多出的 3583 字节按下一步（命令）切 → 版本号不对 → Error 关闭
                s.Write(junk, 0, 200);
                Thread.Sleep(50);
                s.Write(junk, 200, junk.Length - 200);

                //服务端要么先回 01 01 再关，要么过滤器先判出错直接关 —— 两种都算对；老代码是攒着不关，3 秒内读不到关闭
                var sw = System.Diagnostics.Stopwatch.StartNew();
                s.ReadTimeout = 3000;
                bool closed = false;
                try
                {
                    byte[] b = new byte[16];
                    while (true) { if (s.Read(b, 0, b.Length) <= 0) { closed = true; break; } }
                }
                catch (IOException) { closed = true; }
                catch { }
                Check("⑫ 拆两段发的认证帧能合并；多余的垃圾字节让连接在 3 秒内被关闭", closed && sw.ElapsedMilliseconds < 3000, sw.ElapsedMilliseconds + " ms");
            }

            //⑬ 只收不发也算活跃：LastActiveTime 要随客户端发来的数据更新（老代码只在服务端往外发时更新）
            {
                TcpListener sink = Target(IPAddress.Loopback, first => new List<byte[]>());   //收了不回
                using (TcpClient c = Dial())
                {
                    NetworkStream s = Open(c, IPAddress.Loopback, ((IPEndPoint)sink.LocalEndpoint).Port);
                    Thread.Sleep(100);
                    ProxySession ps = Operate.ProxyConfig.Proxy.ProxyServer.GetAllSessions().OrderByDescending(x => x.StartTime).First();
                    DateTime t0 = ps.LastActiveTime;
                    Thread.Sleep(300);
                    Write(s, Pattern(50, 4));
                    Thread.Sleep(200);
                    Check("⑬ 客户端只发不收，LastActiveTime 也更新", ps.LastActiveTime > t0, (ps.LastActiveTime - t0).TotalMilliseconds.ToString("F0") + " ms");
                }
                sink.Stop();
            }

            //———————————— WPC 控制通道（第三组）————————————
            //账号 "cap"：限 1 台设备
            Operate.ProxyConfig.Account.lstAccountInfo.Add(new AccountInfo(
                Guid.NewGuid(), true, "cap", Operate.SystemConfig.PassWord_Encrypt("pw"), new BindingList<AccountIPInfo>(),
                false, 1, true, 1, false, DateTime.MaxValue, DateTime.Now));

            //⑭ 问候报 [02, 80] → 选 80；注册 → OK + 令牌；Ping → Pong
            string token;
            TcpClient ctl = Dial();
            {
                NetworkStream s = ctl.GetStream();
                Write(s, 0x05, 0x02, 0x02, 0x80);
                byte[] r = ReadN(s, 2);
                Check("⑭ 问候含 0x80 时服务端选 0x80", r[0] == 0x05 && r[1] == 0x80, BitConverter.ToString(r));

                string body = Register(s, "cap", "pw", "device-aaaa-0001", "1.0");
                token = Field(body, "token");
                Check("⑭ 注册成功拿到令牌", Field(body, "code") == "0" && token.StartsWith("wpc1."), body);

                WriteFrame(s, 0x02, "{}");
                byte[] pong = ReadFrame(s, out byte ptype);
                Check("⑭ Ping 有 Pong", ptype == 0x82, "type=" + ptype);
                Check("⑭ 已注册设备数 = 1", Operate.WPCConfig.Device.Count == 1, Operate.WPCConfig.Device.Count.ToString());
            }

            //⑮ 数据连接用令牌认证 → 01 00，会话带设备指纹；两条数据连接 + 控制连接 = 1 台设备 2 条链接
            TcpListener echo2 = Target(IPAddress.Loopback, first => new List<byte[]> { first });
            using (TcpClient d1 = Dial())
            using (TcpClient d2 = Dial())
            {
                NetworkStream s1 = Open(d1, IPAddress.Loopback, ((IPEndPoint)echo2.LocalEndpoint).Port, "cap", token);
                NetworkStream s2 = Open(d2, IPAddress.Loopback, ((IPEndPoint)echo2.LocalEndpoint).Port, "cap", token);

                //先看会话表再发数据：靶子回完就关连接，发了数据那条会话马上就没了
                Thread.Sleep(100);
                var wpcSessions = Operate.ProxyConfig.Proxy.ProxyServer.GetAllSessions().Where(x => x.DeviceId == "device-aaaa-0001").ToList();
                Check("⑮ 三条会话都带设备指纹（1 控制 + 2 数据）", wpcSessions.Count == 3 && wpcSessions.Count(x => x.IsWpcControl) == 1, wpcSessions.Count.ToString());

                Operate.ProxyConfig.Account.RefreshAuthList().GetAwaiter().GetResult();
                AuthInfo row = Operate.ProxyConfig.Account.lstAuthInfo.ToList().FirstOrDefault(a => a.DeviceId == "device-aaaa-0001");
                Check("⑮ 客户端列表：1 台设备、2 条链接、客户端 WPC 1.0", row != null && row.DevicesNumber == 1 && row.LinksNumber == 2 && row.Client == "WPC 1.0",
                    row == null ? "no row" : row.DevicesNumber + "/" + row.LinksNumber + "/" + row.Client);
                AccountInfo capAcc = Operate.ProxyConfig.Account.lstAccountInfo.First(a => a.UserName == "cap");
                Check("⑮ 账号在线", capAcc.IsOnLine);

                Write(s1, Pattern(20, 5));
                Check("⑮ 令牌认证的数据连接能转发", IsPattern(ReadN(s1, 20), 20, 5));

                //⑯ 第二台设备注册 → 设备数超限（账号限 1 台）
                using (TcpClient ctl2 = Dial())
                {
                    NetworkStream s = ctl2.GetStream();
                    Write(s, 0x05, 0x02, 0x02, 0x80);
                    ReadN(s, 2);
                    string body = Register(s, "cap", "pw", "device-bbbb-0002", "1.0");
                    Check("⑯ 第二台设备注册被拒：DeviceLimit(4)", Field(body, "code") == "4", body);
                }

                //⑯ 密码错 / 账号不存在 / 设备 ID 非法 的错误码
                using (TcpClient ctl3 = Dial())
                {
                    NetworkStream s = ctl3.GetStream();
                    Write(s, 0x05, 0x01, 0x80);
                    ReadN(s, 2);
                    Check("⑯ 密码错：BadCredential(1)", Field(Register(s, "cap", "nope", "device-cccc-0003", "1.0"), "code") == "1");
                }
                using (TcpClient ctl4 = Dial())
                {
                    NetworkStream s = ctl4.GetStream();
                    Write(s, 0x05, 0x01, 0x80);
                    ReadN(s, 2);
                    Check("⑯ 设备 ID 非法：BadRequest(5)", Field(Register(s, "cap", "pw", "bad id!", "1.0"), "code") == "5");
                }

                //⑰ 开关打开：普通密码被拒，令牌照常
                Operate.ProxyConfig.Proxy.Only_WPC_Client = true;
                try
                {
                    using (TcpClient p = Dial())
                    {
                        NetworkStream s = p.GetStream();
                        Write(s, 0x05, 0x01, 0x02);
                        ReadN(s, 2);
                        byte[] a = AuthPacket("wpe", "s3cret");
                        s.Write(a, 0, a.Length);
                        byte[] r = ReadN(s, 2);
                        Check("⑰ 开关打开后普通账号密码被拒 01 01", r[1] == 0x01, BitConverter.ToString(r));
                    }
                    using (TcpClient d3 = Dial())
                    {
                        NetworkStream s3 = Open(d3, IPAddress.Loopback, ((IPEndPoint)echo2.LocalEndpoint).Port, "cap", token);
                        Write(s3, Pattern(8, 6));
                        Check("⑰ 开关打开后令牌照常可用", IsPattern(ReadN(s3, 8), 8, 6));
                    }
                }
                finally { Operate.ProxyConfig.Proxy.Only_WPC_Client = false; }

                //⑱ 同一设备重新注册（WPC 重启）：旧控制连接被踢，新令牌可用、旧令牌作废
                string token2;
                using (TcpClient ctl5 = Dial())
                {
                    NetworkStream s = ctl5.GetStream();
                    Write(s, 0x05, 0x01, 0x80);
                    ReadN(s, 2);
                    string body = Register(s, "cap", "pw", "device-aaaa-0001", "1.0");
                    token2 = Field(body, "token");
                    Check("⑱ 同设备重注册成功（不算新设备）", Field(body, "code") == "0" && token2.Length > 10, body);
                    Thread.Sleep(150);
                    bool oldClosed;
                    try { ctl.GetStream().ReadTimeout = 1000; oldClosed = ctl.GetStream().Read(new byte[1], 0, 1) == 0; } catch (IOException) { oldClosed = false; } catch { oldClosed = true; }
                    Check("⑱ 旧控制连接被踢掉", oldClosed);
                    Check("⑱ 已注册设备数仍是 1", Operate.WPCConfig.Device.Count == 1, Operate.WPCConfig.Device.Count.ToString());

                    using (TcpClient old = Dial())
                    {
                        NetworkStream so = old.GetStream();
                        Write(so, 0x05, 0x01, 0x02);
                        ReadN(so, 2);
                        byte[] a = AuthPacket("cap", token);
                        so.Write(a, 0, a.Length);
                        Check("⑱ 旧令牌作废 01 01", ReadN(so, 2)[1] == 0x01);
                    }
                    using (TcpClient nw = Dial())
                    {
                        NetworkStream sn = Open(nw, IPAddress.Loopback, ((IPEndPoint)echo2.LocalEndpoint).Port, "cap", token2);
                        Write(sn, Pattern(8, 8));
                        Check("⑱ 新令牌可用", IsPattern(ReadN(sn, 8), 8, 8));
                    }
                }

                //⑲ 控制连接断开 → 令牌作废、设备槽释放、账号离线
                Thread.Sleep(200);
                Check("⑲ 控制连接断开后令牌表清空", Operate.WPCConfig.Device.Count == 0, Operate.WPCConfig.Device.Count.ToString());
                using (TcpClient gone = Dial())
                {
                    NetworkStream sg = gone.GetStream();
                    Write(sg, 0x05, 0x01, 0x02);
                    ReadN(sg, 2);
                    byte[] a = AuthPacket("cap", token2);
                    sg.Write(a, 0, a.Length);
                    Check("⑲ 令牌随控制连接失效 01 01", ReadN(sg, 2)[1] == 0x01);
                }
            }
            ctl.Close();
            echo2.Stop();
            Thread.Sleep(300);
            Operate.ProxyConfig.Account.RefreshAuthList().GetAwaiter().GetResult();
            Check("⑲ 数据连接也关了：设备槽为 0、账号离线",
                Operate.ProxyConfig.Account.Devices.Count(Operate.ProxyConfig.Account.lstAccountInfo.First(a => a.UserName == "cap").AID) == 0
                && !Operate.ProxyConfig.Account.lstAccountInfo.First(a => a.UserName == "cap").IsOnLine);

            //⑳ 认证关着时不选 0x80（WPC 会退回普通模式）
            Operate.ProxyConfig.Proxy.Enable_Auth = false;
            try
            {
                using (TcpClient c = Dial())
                {
                    NetworkStream s = c.GetStream();
                    Write(s, 0x05, 0x02, 0x02, 0x80);
                    byte[] r = ReadN(s, 2);
                    Check("⑳ 认证关着：报了 0x80 也回 05 FF（没有 0x00 可选）", r[1] == 0xFF, BitConverter.ToString(r));
                }
                using (TcpClient c = Dial())
                {
                    NetworkStream s = c.GetStream();
                    Write(s, 0x05, 0x03, 0x00, 0x02, 0x80);
                    byte[] r = ReadN(s, 2);
                    Check("⑳ 认证关着且报了 0x00：选 0x00", r[1] == 0x00, BitConverter.ToString(r));
                }
            }
            finally { Operate.ProxyConfig.Proxy.Enable_Auth = true; }

            Operate.ProxyConfig.Proxy.StopProxy();
            Check("收尾：StopProxy 后不在运行", !Operate.ProxyConfig.Proxy.IsRunning);
        }
        catch (Exception ex)
        {
            Console.WriteLine("  EXCEPTION  " + ex);
            fail++;
        }
        finally
        {
            try { Operate.ProxyConfig.Proxy.StopProxy(); } catch { }
            try { Directory.Delete(tmp, true); } catch { }
        }

        return Finish();
    }

    static int Finish()
    {
        Console.WriteLine();
        Console.WriteLine(fail == 0 ? "ALL PASS  (" + pass + ")" : "FAILED  pass=" + pass + " fail=" + fail);
        return fail == 0 ? 0 : 1;
    }
}
