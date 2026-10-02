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
            Operate.ProxyConfig.Proxy.UnpackRules = new List<Operate.ProxyConfig.Proxy.UnpackRule>
            {
                new Operate.ProxyConfig.Proxy.UnpackRule { Name = "AA", Header = "AA BB", Length = "3-4", Direction = 0, IsEnable = true }
            };

            Run("首包跨接收边界会缓存", FirstPacketFragment);
            Run("完整包加半包不会丢响应数据", CompletePlusPartial);
            Run("连续粘包逐帧输出", CoalescedPackets);
            Run("包头跨接收边界会缓存", HeaderFragment);
            Run("非帧前缀原样转发并重同步", Resynchronize);
            Run("零长度不会卡死且不丢字节", ZeroLength);
            Run("超大长度不会无限攒缓存", OversizedLength);
            Run("拒绝零基和超过四字节的长度字段", InvalidSettings);
            Run("多规则按方向选择", MultiRulesByDirection);
            Run("同一方向首帧后固定规则", RuleIsPinnedPerStream);
            Run("零规则时原样直通", NoRulesPassthrough);
            Run("多规则配置可序列化并恢复", RuleSerialization);

            Console.WriteLine("PacketUnpackTest: {0} passed, {1} failed", passed, failed);
            Environment.ExitCode = failed == 0 ? 0 : 1;
        }

        private static byte[] Frame(byte a, byte b)
        {
            return new byte[] { 0xAA, 0xBB, 0x00, 0x06, a, b };
        }

        private static void FirstPacketFragment()
        {
            var buffer = new Operate.ProxyConfig.Proxy.UnpackStreamState();
            byte[] frame = Frame(0x10, 0x11);
            byte[][] first = Operate.ProxyConfig.Proxy.ProcessResponseData(frame.Take(3).ToArray(), buffer);
            Assert(first.Length == 0, "首段应完整留在残片");
            byte[][] second = Operate.ProxyConfig.Proxy.ProcessResponseData(frame.Skip(3).ToArray(), buffer);
            Assert(second.Length == 1 && Same(second[0], frame), "第二段应组合成一帧");
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
            Operate.ProxyConfig.Proxy.UnpackRules = new List<Operate.ProxyConfig.Proxy.UnpackRule>
            {
                new Operate.ProxyConfig.Proxy.UnpackRule { Name = "large", Header = "AA BB", Length = "3-6", Direction = 0, IsEnable = true }
            };
            byte[] buffer = Array.Empty<byte>();
            byte[] source = new byte[] { 0xAA, 0xBB, 0xFF, 0xFF, 0xFF, 0xFF };
            byte[][] output = Operate.ProxyConfig.Proxy.ProcessResponseData(source, ref buffer);
            Assert(Same(Join(output, buffer), source) && buffer.Length == 0, "超大长度不能留下无限等待的残片");
            Operate.ProxyConfig.Proxy.UnPack_Length = "3-4";
            Operate.ProxyConfig.Proxy.UnpackRules = new List<Operate.ProxyConfig.Proxy.UnpackRule>
            {
                new Operate.ProxyConfig.Proxy.UnpackRule { Name = "AA", Header = "AA BB", Length = "3-4", Direction = 0, IsEnable = true }
            };
        }

        private static void InvalidSettings()
        {
            string error;
            Assert(!Operate.ProxyConfig.Proxy.ValidateUnpackSettings("AA BB", "0-1", out error), "零基位置必须被拒绝");
            Assert(!Operate.ProxyConfig.Proxy.ValidateUnpackSettings("AA BB", "1-5", out error), "五字节长度字段必须被拒绝");
            Assert(Operate.ProxyConfig.Proxy.ValidateUnpackSettings("AA BB", "3-4", out error), "默认格式必须通过");
        }

        private static void MultiRulesByDirection()
        {
            Operate.ProxyConfig.Proxy.UnpackRules = new List<Operate.ProxyConfig.Proxy.UnpackRule>
            {
                new Operate.ProxyConfig.Proxy.UnpackRule { Name = "request", Header = "AA BB", Length = "3-4", Direction = 1, IsEnable = true },
                new Operate.ProxyConfig.Proxy.UnpackRule { Name = "response", Header = "CC DD", Length = "3-4", Direction = 2, IsEnable = true },
            };
            var state = new Operate.ProxyConfig.Proxy.UnpackStreamState();
            byte[] response = new byte[] { 0xCC, 0xDD, 0x00, 0x06, 0x12, 0x13 };
            byte[][] output = Operate.ProxyConfig.Proxy.ProcessResponseData(response, state);
            Assert(output.Length == 1 && Same(output[0], response), "响应方向必须命中仅响应规则");
        }

        private static void RuleIsPinnedPerStream()
        {
            Operate.ProxyConfig.Proxy.UnpackRules = new List<Operate.ProxyConfig.Proxy.UnpackRule>
            {
                new Operate.ProxyConfig.Proxy.UnpackRule { Name = "first", Header = "AA BB", Length = "3-4", Direction = 0, IsEnable = true }
            };
            var state = new Operate.ProxyConfig.Proxy.UnpackStreamState();
            byte[] first = Frame(0x80, 0x81);
            Assert(Operate.ProxyConfig.Proxy.ProcessResponseData(first, state).Length == 1, "首帧应命中第一条规则");
            Operate.ProxyConfig.Proxy.UnpackRules = new List<Operate.ProxyConfig.Proxy.UnpackRule>
            {
                new Operate.ProxyConfig.Proxy.UnpackRule { Name = "new", Header = "CC DD", Length = "3-4", Direction = 0, IsEnable = true }
            };
            byte[] second = Frame(0x82, 0x83);
            byte[][] output = Operate.ProxyConfig.Proxy.ProcessResponseData(second, state);
            Assert(output.Length == 1 && Same(output[0], second), "已建立 TCP 流不能因设置更新切换规则");
        }

        private static void NoRulesPassthrough()
        {
            Operate.ProxyConfig.Proxy.UnpackRules = new List<Operate.ProxyConfig.Proxy.UnpackRule>();
            byte[] source = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var state = new Operate.ProxyConfig.Proxy.UnpackStreamState();
            byte[][] output = Operate.ProxyConfig.Proxy.ProcessResponseData(source, state);
            Assert(output.Length == 1 && Same(output[0], source), "没有规则时必须原样转发");
        }

        private static void RuleSerialization()
        {
            Operate.ProxyConfig.Proxy.UnpackRules = new List<Operate.ProxyConfig.Proxy.UnpackRule>
            {
                new Operate.ProxyConfig.Proxy.UnpackRule { Id = "rule-a", Name = "A", Header = "AA BB", Length = "3-4", Direction = 1, IsEnable = true },
                new Operate.ProxyConfig.Proxy.UnpackRule { Id = "rule-b", Name = "B", Header = "CC DD", Length = "3-4", Direction = 2, IsEnable = false },
            };
            string json = Operate.ProxyConfig.Proxy.SerializeUnpackRules();
            Operate.ProxyConfig.Proxy.UnpackRules = new List<Operate.ProxyConfig.Proxy.UnpackRule>();
            Operate.ProxyConfig.Proxy.LoadUnpackRules(json);
            var rules = Operate.ProxyConfig.Proxy.UnpackRules;
            Assert(rules.Count == 2 && rules[0].Id == "rule-a" && rules[1].Direction == 2 && !rules[1].IsEnable, "规则存储恢复必须保持顺序和字段");
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
