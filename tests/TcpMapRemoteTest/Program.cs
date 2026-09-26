using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpMapRemoteTest
{
    /// <summary>
    /// WPE TCP 连接级远程映射的端到端跑测。
    ///
    /// 先在 WPE 中启用规则：tcp://127.0.0.1:39101 → tcp://127.0.0.1:39102，
    /// 然后运行本程序。程序只监听 39102，却经 SOCKS5 请求 39101；目标收到
    /// 带随机标记的负载并回显同一标记，才算真正证明 CONNECT 改道和数据转发均成功。
    /// </summary>
    internal static class Program
    {
        private const int DefaultSourcePort = 39101;
        private const int DefaultTargetPort = 39102;

        private static int Main(string[] args)
        {
            try
            {
                Options options = Options.Parse(args);
                Run(options).GetAwaiter().GetResult();
                Console.WriteLine("PASS: WPE TCP 连接级远程映射已将 127.0.0.1:{0} 改道至 127.0.0.1:{1}，转发数据完整。", options.SourcePort, options.TargetPort);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("FAIL: " + ex.Message);
                return 1;
            }
        }

        private static async Task Run(Options options)
        {
            string marker = "WPE-TCP-MAP-" + Guid.NewGuid().ToString("N");
            byte[] request = Encoding.ASCII.GetBytes(marker);
            TcpListener target = new TcpListener(IPAddress.Loopback, options.TargetPort);
            target.Start();
            try
            {
                Task targetTask = ServeOnce(target, request);
                using (TcpClient client = new TcpClient())
                {
                    client.NoDelay = true;
                    client.ReceiveTimeout = 10000;
                    client.SendTimeout = 10000;
                    await client.ConnectAsync(options.SocksHost, options.SocksPort);
                    NetworkStream stream = client.GetStream();
                    Socks5Authenticate(stream, options);
                    Socks5Connect(stream, IPAddress.Loopback, options.SourcePort);
                    stream.Write(request, 0, request.Length);
                    byte[] response = ReadExactly(stream, request.Length);
                    if (!ByteArrayEquals(request, response)) { throw new InvalidOperationException("目标回显内容不一致，数据转发或滤镜链路被破坏。"); }
                }

                if (await Task.WhenAny(targetTask, Task.Delay(10000)) != targetTask)
                {
                    throw new TimeoutException("映射目标没有收到连接；请确认 WPE 代理已运行且规则已启用。");
                }
                await targetTask;
            }
            finally
            {
                target.Stop();
            }
        }

        private static async Task ServeOnce(TcpListener listener, byte[] expected)
        {
            using (TcpClient target = await listener.AcceptTcpClientAsync())
            using (NetworkStream stream = target.GetStream())
            {
                byte[] received = ReadExactly(stream, expected.Length);
                if (!ByteArrayEquals(expected, received)) { throw new InvalidOperationException("映射目标收到的测试标记不正确。"); }
                stream.Write(received, 0, received.Length);
            }
        }

        private static void Socks5Authenticate(NetworkStream stream, Options options)
        {
            bool authenticated = !string.IsNullOrEmpty(options.User);
            byte[] hello = authenticated ? new byte[] { 0x05, 0x02, 0x00, 0x02 } : new byte[] { 0x05, 0x01, 0x00 };
            stream.Write(hello, 0, hello.Length);
            byte[] response = ReadExactly(stream, 2);
            if (response[0] != 0x05) { throw new InvalidOperationException("对端不是 SOCKS5 服务。"); }
            if (!authenticated && response[1] == 0x00) { return; }
            if (!authenticated) { throw new InvalidOperationException("WPE SOCKS5 要求认证；请使用 --user 与 --password。 "); }
            if (response[1] != 0x02) { throw new InvalidOperationException("WPE SOCKS5 未选择用户名密码认证。"); }

            byte[] user = Encoding.UTF8.GetBytes(options.User);
            byte[] password = Encoding.UTF8.GetBytes(options.Password ?? string.Empty);
            if (user.Length == 0 || user.Length > 255 || password.Length > 255) { throw new InvalidOperationException("SOCKS5 用户名或密码长度无效。"); }
            List<byte> auth = new List<byte> { 0x01, (byte)user.Length };
            auth.AddRange(user); auth.Add((byte)password.Length); auth.AddRange(password);
            byte[] request = auth.ToArray();
            stream.Write(request, 0, request.Length);
            response = ReadExactly(stream, 2);
            if (response[0] != 0x01 || response[1] != 0x00) { throw new InvalidOperationException("WPE SOCKS5 认证失败。"); }
        }

        private static void Socks5Connect(NetworkStream stream, IPAddress target, int port)
        {
            byte[] address = target.GetAddressBytes();
            byte[] request = new byte[10];
            request[0] = 0x05; request[1] = 0x01; request[2] = 0x00; request[3] = 0x01;
            Buffer.BlockCopy(address, 0, request, 4, 4);
            request[8] = (byte)(port >> 8); request[9] = (byte)port;
            stream.Write(request, 0, request.Length);

            byte[] head = ReadExactly(stream, 4);
            if (head[0] != 0x05 || head[1] != 0x00) { throw new InvalidOperationException("WPE SOCKS5 CONNECT 被拒绝，状态=" + head[1] + "。"); }
            int tail = head[3] == 0x01 ? 6 : head[3] == 0x04 ? 18 : head[3] == 0x03 ? ReadExactly(stream, 1)[0] + 2 : throw new InvalidOperationException("SOCKS5 CONNECT 应答地址类型无效。");
            ReadExactly(stream, tail);
        }

        private static byte[] ReadExactly(Stream stream, int count)
        {
            byte[] buffer = new byte[count];
            int offset = 0;
            while (offset < count)
            {
                int read = stream.Read(buffer, offset, count - offset);
                if (read <= 0) { throw new EndOfStreamException("连接在读取完整响应前关闭。"); }
                offset += read;
            }
            return buffer;
        }

        private static bool ByteArrayEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length) { return false; }
            for (int i = 0; i < left.Length; i++) { if (left[i] != right[i]) { return false; } }
            return true;
        }

        private sealed class Options
        {
            public string SocksHost = "127.0.0.1";
            public int SocksPort = 1080;
            public int SourcePort = DefaultSourcePort;
            public int TargetPort = DefaultTargetPort;
            public string User;
            public string Password;

            public static Options Parse(string[] args)
            {
                Options result = new Options();
                for (int i = 0; i < args.Length; i++)
                {
                    string value = i + 1 < args.Length ? args[++i] : throw new ArgumentException("参数缺少值：" + args[i]);
                    if (args[i - 1] == "--socks-host") { result.SocksHost = value; }
                    else if (args[i - 1] == "--socks-port") { result.SocksPort = int.Parse(value); }
                    else if (args[i - 1] == "--source-port") { result.SourcePort = int.Parse(value); }
                    else if (args[i - 1] == "--target-port") { result.TargetPort = int.Parse(value); }
                    else if (args[i - 1] == "--user") { result.User = value; }
                    else if (args[i - 1] == "--password") { result.Password = value; }
                    else { throw new ArgumentException("未知参数：" + args[i - 1]); }
                }
                if (result.SocksPort < 1 || result.SocksPort > 65535 || result.SourcePort < 1 || result.SourcePort > 65535 || result.TargetPort < 1 || result.TargetPort > 65535)
                {
                    throw new ArgumentException("端口必须在 1 到 65535 之间。");
                }
                if (string.IsNullOrEmpty(result.User) != string.IsNullOrEmpty(result.Password)) { throw new ArgumentException("--user 与 --password 必须同时提供。"); }
                return result;
            }
        }
    }
}
