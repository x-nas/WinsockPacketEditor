using System;
using System.Collections.Generic;
using System.Linq;
using WinsockPacketEditor;

namespace PacketUnpackTest
{
    /// <summary>代理 TCP 拆包回归：分片、粘包、重同步、非法长度和配置边界。</summary>
    internal static class Program
    {
        private static int passed;
        private static int failed;

        private static void Main()
        {
            Operate.ProxyConfig.Proxy.Enable_UnPack = true;
            Operate.ProxyConfig.Proxy.UnPack_Head = "AA BB";
            Operate.ProxyConfig.Proxy.UnPack_Length = "3-4";

            Run("首包跨接收边界会缓存", FirstPacketFragment);
            Run("完整包加半包不会丢响应数据", CompletePlusPartial);
            Run("连续粘包逐帧输出", CoalescedPackets);
            Run("包头跨接收边界会缓存", HeaderFragment);
            Run("非帧前缀原样转发并重同步", Resynchronize);
            Run("零长度不会卡死且不丢字节", ZeroLength);
            Run("超大长度不会无限攒缓存", OversizedLength);
            Run("拒绝零基和超过四字节的长度字段", InvalidSettings);

            Console.WriteLine("PacketUnpackTest: {0} passed, {1} failed", passed, failed);
            Environment.ExitCode = failed == 0 ? 0 : 1;
        }

        private static byte[] Frame(byte a, byte b)
        {
            return new byte[] { 0xAA, 0xBB, 0x00, 0x06, a, b };
        }

        private static void FirstPacketFragment()
        {
            byte[] buffer = Array.Empty<byte>();
            byte[] frame = Frame(0x10, 0x11);
            byte[][] first = Operate.ProxyConfig.Proxy.ProcessResponseData(frame.Take(3).ToArray(), ref buffer);
            Assert(first.Length == 0 && Same(buffer, frame.Take(3).ToArray()), "首段应完整留在残片");
            byte[][] second = Operate.ProxyConfig.Proxy.ProcessResponseData(frame.Skip(3).ToArray(), ref buffer);
            Assert(second.Length == 1 && Same(second[0], frame) && buffer.Length == 0, "第二段应组合成一帧");
        }

        private static void CompletePlusPartial()
        {
            byte[] buffer = Array.Empty<byte>();
            byte[] one = Frame(0x20, 0x21);
            byte[] two = Frame(0x30, 0x31);
            byte[] input = one.Concat(two.Take(4)).ToArray();
            byte[][] first = Operate.ProxyConfig.Proxy.ProcessResponseData(input, ref buffer);
            Assert(first.Length == 1 && Same(first[0], one) && Same(buffer, two.Take(4).ToArray()), "尾部半包必须保留");
            byte[][] second = Operate.ProxyConfig.Proxy.ProcessResponseData(two.Skip(4).ToArray(), ref buffer);
            Assert(second.Length == 1 && Same(second[0], two) && buffer.Length == 0, "保留的半包必须在下一段完成");
        }

        private static void CoalescedPackets()
        {
            byte[] buffer = Array.Empty<byte>();
            byte[] one = Frame(0x40, 0x41);
            byte[] two = Frame(0x50, 0x51);
            byte[][] output = Operate.ProxyConfig.Proxy.ProcessResponseData(one.Concat(two).ToArray(), ref buffer);
            Assert(output.Length == 2 && Same(output[0], one) && Same(output[1], two) && buffer.Length == 0, "粘包必须拆成两帧");
        }

        private static void HeaderFragment()
        {
            byte[] buffer = Array.Empty<byte>();
            byte[] frame = Frame(0x60, 0x61);
            byte[][] first = Operate.ProxyConfig.Proxy.ProcessResponseData(new byte[] { 0xAA }, ref buffer);
            Assert(first.Length == 0 && Same(buffer, new byte[] { 0xAA }), "包头前缀不能提前转发");
            byte[][] second = Operate.ProxyConfig.Proxy.ProcessResponseData(frame.Skip(1).ToArray(), ref buffer);
            Assert(second.Length == 1 && Same(second[0], frame), "跨边界包头应被识别");
        }

        private static void Resynchronize()
        {
            byte[] buffer = Array.Empty<byte>();
            byte[] frame = Frame(0x70, 0x71);
            byte[] source = (new byte[] { 0x99, 0x98 }).Concat(frame).ToArray();
            byte[][] output = Operate.ProxyConfig.Proxy.ProcessResponseData(source, ref buffer);
            Assert(Same(Join(output, buffer), source) && output.Length == 2 && Same(output[1], frame), "重同步不能修改或遗漏原始字节");
        }

        private static void ZeroLength()
        {
            byte[] buffer = Array.Empty<byte>();
            byte[] source = new byte[] { 0xAA, 0xBB, 0x00, 0x00 };
            byte[][] output = Operate.ProxyConfig.Proxy.ProcessResponseData(source, ref buffer);
            Assert(Same(Join(output, buffer), source), "零长度输入必须保持字节完整");
        }

        private static void OversizedLength()
        {
            Operate.ProxyConfig.Proxy.UnPack_Length = "3-6";
            byte[] buffer = Array.Empty<byte>();
            byte[] source = new byte[] { 0xAA, 0xBB, 0xFF, 0xFF, 0xFF, 0xFF };
            byte[][] output = Operate.ProxyConfig.Proxy.ProcessResponseData(source, ref buffer);
            Assert(Same(Join(output, buffer), source) && buffer.Length == 0, "超大长度不能留下无限等待的残片");
            Operate.ProxyConfig.Proxy.UnPack_Length = "3-4";
        }

        private static void InvalidSettings()
        {
            string error;
            Assert(!Operate.ProxyConfig.Proxy.ValidateUnpackSettings("AA BB", "0-1", out error), "零基位置必须被拒绝");
            Assert(!Operate.ProxyConfig.Proxy.ValidateUnpackSettings("AA BB", "1-5", out error), "五字节长度字段必须被拒绝");
            Assert(Operate.ProxyConfig.Proxy.ValidateUnpackSettings("AA BB", "3-4", out error), "默认格式必须通过");
        }

        private static void Run(string name, Action test)
        {
            try { test(); passed++; Console.WriteLine("[PASS] " + name); }
            catch (Exception ex) { failed++; Console.Error.WriteLine("[FAIL] " + name + ": " + ex.Message); }
        }

        private static void Assert(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
        private static bool Same(byte[] left, byte[] right) { return left != null && right != null && left.SequenceEqual(right); }
        private static byte[] Join(IEnumerable<byte[]> packets, byte[] tail) { return packets.SelectMany(x => x).Concat(tail ?? Array.Empty<byte>()).ToArray(); }
    }
}
