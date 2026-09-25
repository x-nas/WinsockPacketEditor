using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 封包详情页的按需解码器。它只在外壳收到用户请求后运行，绝不进入钩子、滤镜或代理转发热路径。
    /// </summary>
    internal static class ProtocolDecoder
    {
        private const int MaxInputBytes = 4 * 1024 * 1024;
        private const int MaxProtoDepth = 16;
        private const int MaxProtoFields = 4096;

        internal sealed class Result
        {
            public bool Ok;
            public string Error;
            public string Text;
            public string OutputBase64;
            public string Format;
        }

        internal static Result Decode(byte[] input, string kind, string keyHex, string ivHex)
        {
            try
            {
                if (input == null) { return Fail("封包不存在或已被清理"); }
                if (input.Length > MaxInputBytes) { return Fail("封包超过 4 MB，未执行解码"); }

                byte[] output;
                switch ((kind ?? string.Empty).ToLowerInvariant())
                {
                    case "utf8":
                        return Success(input, SafeText(input), "text");

                    case "base64":
                        output = Convert.FromBase64String(Encoding.ASCII.GetString(input).Trim());
                        return Success(output, SafeText(output), "bytes");

                    case "xor":
                        output = Xor(input, ParseHex(keyHex, "XOR 密钥"));
                        return Success(output, SafeText(output), "bytes");

                    case "aes-cbc":
                        output = AesCbcDecrypt(input, ParseHex(keyHex, "AES 密钥"), ParseHex(ivHex, "AES IV"));
                        return Success(output, SafeText(output), "bytes");

                    case "protobuf":
                        return Success(input, ProtobufGuess(input), "protobuf");

                    default:
                        return Fail("不支持的解码器");
                }
            }
            catch (FormatException ex) { return Fail(ex.Message); }
            catch (CryptographicException ex) { return Fail("AES 解密失败：" + ex.Message); }
            catch (Exception ex) { return Fail("解码失败：" + ex.Message); }
        }

        private static Result Success(byte[] output, string text, string format)
        {
            return new Result { Ok = true, Text = text, OutputBase64 = Convert.ToBase64String(output), Format = format };
        }

        private static Result Fail(string error) { return new Result { Ok = false, Error = error }; }

        private static byte[] Xor(byte[] input, byte[] key)
        {
            if (key.Length == 0) { throw new FormatException("XOR 密钥不能为空"); }
            byte[] output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++) { output[i] = (byte)(input[i] ^ key[i % key.Length]); }
            return output;
        }

        private static byte[] AesCbcDecrypt(byte[] input, byte[] key, byte[] iv)
        {
            if (key.Length != 16 && key.Length != 24 && key.Length != 32) { throw new FormatException("AES 密钥必须是 16、24 或 32 字节"); }
            if (iv.Length != 16) { throw new FormatException("AES IV 必须是 16 字节"); }
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;
                using (ICryptoTransform decryptor = aes.CreateDecryptor()) { return decryptor.TransformFinalBlock(input, 0, input.Length); }
            }
        }

        private static byte[] ParseHex(string value, string name)
        {
            string s = (value ?? string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);
            if (s.Length == 0 || (s.Length & 1) != 0) { throw new FormatException(name + "必须是偶数位十六进制"); }
            byte[] result = new byte[s.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                byte b;
                if (!byte.TryParse(s.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out b))
                {
                    throw new FormatException(name + "不是有效十六进制");
                }
                result[i] = b;
            }
            return result;
        }

        private static string SafeText(byte[] bytes)
        {
            string text = new UTF8Encoding(false, false).GetString(bytes);
            return text.Replace("\0", "·");
        }

        private static string ProtobufGuess(byte[] input)
        {
            var sb = new StringBuilder();
            int fields = 0;
            ParseMessage(input, 0, input.Length, 0, sb, ref fields);
            return sb.ToString();
        }

        private static void ParseMessage(byte[] bytes, int start, int end, int depth, StringBuilder sb, ref int fields)
        {
            if (depth >= MaxProtoDepth) { sb.AppendLine("… 最大嵌套深度"); return; }
            int pos = start;
            while (pos < end && fields++ < MaxProtoFields)
            {
                ulong tag = ReadVarint(bytes, ref pos, end);
                int field = (int)(tag >> 3);
                int wire = (int)(tag & 7);
                if (field <= 0 || wire == 3 || wire == 4 || wire > 5) { throw new FormatException("不是有效的 Protobuf wire format"); }
                sb.Append(' ', depth * 2).Append(field).Append(" [").Append(wire).Append("]: ");
                switch (wire)
                {
                    case 0: sb.AppendLine(ReadVarint(bytes, ref pos, end).ToString(CultureInfo.InvariantCulture)); break;
                    case 1: Require(pos, 8, end); sb.AppendLine("fixed64 0x" + BitConverter.ToString(bytes, pos, 8).Replace("-", "")); pos += 8; break;
                    case 2:
                        int len = checked((int)ReadVarint(bytes, ref pos, end));
                        Require(pos, len, end);
                        AppendLengthDelimited(bytes, pos, len, depth, sb, ref fields);
                        pos += len;
                        break;
                    case 5: Require(pos, 4, end); sb.AppendLine("fixed32 0x" + BitConverter.ToString(bytes, pos, 4).Replace("-", "")); pos += 4; break;
                }
            }
            if (fields >= MaxProtoFields) { sb.AppendLine("… 字段数达到上限"); }
        }

        private static void AppendLengthDelimited(byte[] bytes, int pos, int len, int depth, StringBuilder sb, ref int fields)
        {
            if (len == 0) { sb.AppendLine("\"\""); return; }
            string text = new UTF8Encoding(false, true).GetString(bytes, pos, len);
            if (LooksPrintable(text)) { sb.AppendLine("\"" + text.Replace("\"", "\\\"") + "\""); return; }
            sb.AppendLine("bytes(" + len + ")");
            try { ParseMessage(bytes, pos, pos + len, depth + 1, sb, ref fields); }
            catch { /* 长度字段也可能是普通二进制，保留 bytes(N) 即可。 */ }
        }

        private static bool LooksPrintable(string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            foreach (char c in value) { if (char.IsControl(c) && c != '\r' && c != '\n' && c != '\t') { return false; } }
            return true;
        }

        private static ulong ReadVarint(byte[] bytes, ref int pos, int end)
        {
            ulong value = 0;
            for (int shift = 0; shift < 64; shift += 7)
            {
                if (pos >= end) { throw new FormatException("Protobuf varint 被截断"); }
                byte b = bytes[pos++];
                value |= (ulong)(b & 0x7F) << shift;
                if ((b & 0x80) == 0) { return value; }
            }
            throw new FormatException("Protobuf varint 过长");
        }

        private static void Require(int pos, int length, int end)
        {
            if (length < 0 || pos > end - length) { throw new FormatException("Protobuf 字段被截断"); }
        }
    }
}
