using System;
using System.Collections.Generic;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 帧解析：在算法之前，把一条封包按「固定头部 + 包长字段 + 起始偏移」剥成 payload。
    ///
    /// 语义（三处都由解码器配置决定，与 Fatbeans 的「帧配置 / 固定头部」对齐）：
    ///   ① 有固定头部时先校验并剥掉；
    ///   ② LengthBytes &gt; 0 时按字节序读包长值 L，再按两个开关换算成 payload 长度：
    ///        · LengthIncludesSelf        → L 含包长字段自身，减掉 LengthBytes
    ///        · LengthIncludesFixedHeader → L 含固定头，减掉固定头长度
    ///      两者都不勾时，L 就是 payload 长度（最常见）。
    ///   ③ 再跳过 DataOffset 字节（对应 Fatbeans 的「解码起始偏移」）。
    ///
    /// 全部是纯计算，不碰界面、不进热路径。
    /// </summary>
    internal static class FrameExtractor
    {
        /// <summary>按帧配置取出 payload。出错时 error 非空、返回 null。</summary>
        internal static byte[] Extract(byte[] input, DecoderInfo cfg, out string error)
        {
            error = null;

            try
            {
                if (input == null)
                {
                    error = UI.T("Dec.FrameNoData", "没有可解码的数据");
                    return null;
                }

                byte[] header = null;
                if (cfg.HasFixedHeader)
                {
                    header = ParseHex(cfg.FixedHeader);
                    if (header.Length == 0)
                    {
                        error = UI.T("Dec.FrameHeaderEmpty", "启用了固定头部但没有填写头部字节");
                        return null;
                    }
                    if (input.Length < header.Length || !StartsWith(input, header))
                    {
                        error = UI.T("Dec.FrameHeaderMismatch", "封包与解码器的固定头部不匹配");
                        return null;
                    }
                }

                int headerLen = header == null ? 0 : header.Length;
                int pos = headerLen;

                int payloadLen;

                if (cfg.LengthBytes > 0)
                {
                    if (cfg.LengthBytes != 1 && cfg.LengthBytes != 2 && cfg.LengthBytes != 4)
                    {
                        error = UI.T("Dec.FrameLenBytes", "包长字段只能占 1 / 2 / 4 字节");
                        return null;
                    }

                    if (input.Length < pos + cfg.LengthBytes)
                    {
                        error = UI.T("Dec.FrameTruncated", "封包长度不足，读不出包长字段");
                        return null;
                    }

                    long l = ReadLength(input, pos, cfg.LengthBytes, cfg.BigEndian);
                    pos += cfg.LengthBytes;

                    if (cfg.LengthIncludesSelf) { l -= cfg.LengthBytes; }
                    if (cfg.LengthIncludesFixedHeader) { l -= headerLen; }

                    if (l < 0 || l > int.MaxValue)
                    {
                        error = UI.T("Dec.FrameBadLength", "帧长度值非法");
                        return null;
                    }

                    payloadLen = (int)l;

                    if (input.Length - pos < payloadLen)
                    {
                        error = UI.T("Dec.FrameOverrun", "封包实际长度小于帧声明的长度");
                        return null;
                    }
                }
                else
                {
                    payloadLen = input.Length - pos;
                }

                int offset = cfg.DataOffset;
                if (offset < 0)
                {
                    error = UI.T("Dec.FrameOffset", "解码起始偏移不能为负");
                    return null;
                }
                if (offset > payloadLen)
                {
                    error = UI.T("Dec.FrameOffsetOver", "解码起始偏移超过了 payload 长度");
                    return null;
                }

                byte[] result = new byte[payloadLen - offset];
                Array.Copy(input, pos + offset, result, 0, result.Length);
                return result;
            }
            catch (Exception ex)
            {
                error = string.Format(UI.T("Dec.FrameFail", "帧解析失败：{0}"), ex.Message);
                return null;
            }
        }

        /// <summary>Extract 的逆运算：把 payload 装回「固定头 + 包长字段 + payload」。用于往返测试与编码。</summary>
        internal static byte[] Build(byte[] payload, DecoderInfo cfg, out string error)
        {
            error = null;

            try
            {
                if (payload == null) { payload = new byte[0]; }

                byte[] header = cfg.HasFixedHeader ? ParseHex(cfg.FixedHeader) : new byte[0];
                int headerLen = header.Length;

                List<byte> outBytes = new List<byte>(payload.Length + headerLen + 4);

                if (headerLen > 0) { outBytes.AddRange(header); }

                if (cfg.LengthBytes > 0)
                {
                    if (cfg.LengthBytes != 1 && cfg.LengthBytes != 2 && cfg.LengthBytes != 4)
                    {
                        error = UI.T("Dec.FrameLenBytes", "包长字段只能占 1 / 2 / 4 字节");
                        return null;
                    }

                    long l = payload.Length;
                    if (cfg.LengthIncludesSelf) { l += cfg.LengthBytes; }
                    if (cfg.LengthIncludesFixedHeader) { l += headerLen; }

                    if (l > uint.MaxValue)
                    {
                        error = UI.T("Dec.FrameBadLength", "帧长度值非法");
                        return null;
                    }

                    WriteLength(outBytes, l, cfg.LengthBytes, cfg.BigEndian);
                }

                outBytes.AddRange(payload);
                return outBytes.ToArray();
            }
            catch (Exception ex)
            {
                error = string.Format(UI.T("Dec.FrameFail", "帧解析失败：{0}"), ex.Message);
                return null;
            }
        }

        private static bool StartsWith(byte[] data, byte[] prefix)
        {
            for (int i = 0; i < prefix.Length; i++)
            {
                if (data[i] != prefix[i]) { return false; }
            }
            return true;
        }

        private static long ReadLength(byte[] data, int pos, int bytes, bool bigEndian)
        {
            long value = 0;

            if (bigEndian)
            {
                for (int i = 0; i < bytes; i++) { value = (value << 8) | data[pos + i]; }
            }
            else
            {
                for (int i = bytes - 1; i >= 0; i--) { value = (value << 8) | data[pos + i]; }
            }

            return value;
        }

        private static void WriteLength(List<byte> outBytes, long value, int bytes, bool bigEndian)
        {
            byte[] tmp = new byte[bytes];

            if (bigEndian)
            {
                for (int i = 0; i < bytes; i++) { tmp[bytes - 1 - i] = (byte)((value >> (8 * i)) & 0xFF); }
            }
            else
            {
                for (int i = 0; i < bytes; i++) { tmp[i] = (byte)((value >> (8 * i)) & 0xFF); }
            }

            outBytes.AddRange(tmp);
        }

        /// <summary>解析十六进制串（允许空格 / 连字符），与 ProtocolDecoder.ParseHex 同一口径。</summary>
        internal static byte[] ParseHex(string value)
        {
            string s = (value ?? string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);
            if (s.Length == 0 || (s.Length & 1) != 0) { return new byte[0]; }

            byte[] result = new byte[s.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                byte b;
                if (!byte.TryParse(s.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier,
                        System.Globalization.CultureInfo.InvariantCulture, out b))
                {
                    return new byte[0];
                }
                result[i] = b;
            }
            return result;
        }
    }
}
