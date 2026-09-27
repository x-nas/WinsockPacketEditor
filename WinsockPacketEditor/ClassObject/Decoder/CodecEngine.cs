using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 解码 / 编码结果。字段与原 ProtocolDecoder.Result 一致，前端按同一套字段渲染。
    /// </summary>
    public sealed class CodecResult
    {
        public bool Ok;
        public string Error;
        public string Text;
        public string OutputBase64;
        public string Format;
    }

    /// <summary>
    /// 解码器的算法调度：XOR / AES / DES / Protobuf / MessagePack / 文本编码。
    ///
    /// 与 ProtocolDecoder 的关系：后者服务「快速编解码」的临时算法选择（编码转换页原有那套），
    /// 本类服务「解码器」体系。两条路都只在外壳按需调用，绝不进钩子 / 滤镜 / 代理热路径。
    ///
    /// 帧解析（固定头 / 包长 / 偏移）在 FrameExtractor 里，本类只处理纯 payload。
    /// </summary>
    public static class CodecEngine
    {
        private const int MaxInputBytes = 4 * 1024 * 1024;

        private const int MaxProtoDepth = 16;
        private const int MaxProtoFields = 4096;

        /// <summary>按解码器解码一条数据（含帧解析）。</summary>
        public static CodecResult Run(byte[] input, DecoderInfo cfg)
        {
            return Process(input, cfg, false, true);
        }

        /// <summary>
        /// 按一条实际抓到的封包解码。范围（协议 / HTTP 请求响应）属于配置的一部分，
        /// 不能只存进数据库而在右键、批量、智能解码时忽略。
        /// </summary>
        public static CodecResult Run(byte[] input, DecoderInfo cfg, Operate.PacketConfig.Packet.PacketType packetType)
        {
            return Run(input, cfg, packetType, true);
        }

        /// <summary>带真实封包上下文的解码；即使调用方跳过帧解析，也不得绕过范围校验。</summary>
        public static CodecResult Run(byte[] input, DecoderInfo cfg, Operate.PacketConfig.Packet.PacketType packetType, bool applyFrame)
        {
            if (!AppliesTo(cfg, packetType)) { return ScopeMismatch(cfg, packetType); }
            return Process(input, cfg, false, applyFrame);
        }

        /// <summary>按解码器编码（含帧重建），用于测试台往返与明文重发。</summary>
        public static CodecResult RunEncode(byte[] input, DecoderInfo cfg)
        {
            return Process(input, cfg, true, true);
        }

        /// <summary>
        /// 统一入口。applyFrame 为 false 时跳过帧解析 / 重建（测试台「只测算法」用）。
        /// </summary>
        public static CodecResult Process(byte[] input, DecoderInfo cfg, bool encode, bool applyFrame)
        {
            try
            {
                if (input == null) { return Fail(UI.T("Proto.NoPacket", "封包不存在或已被清理")); }
                if (input.Length > MaxInputBytes) { return Fail(UI.T("Proto.TooLarge", "封包超过 4 MB，未执行解码")); }
                if (cfg == null) { return Fail(UI.T("Dec.NoDecoder", "没有指定解码器")); }

                byte[] data = input;

                if (applyFrame)
                {
                    if (encode)
                    {
                        // 先算算法，再装回帧
                        byte[] core = Core(data, cfg, true);
                        string frameError;
                        byte[] framed = FrameExtractor.Build(core, cfg, out frameError);
                        if (frameError != null) { return Fail(frameError); }
                        return Success(framed, SafeText(framed), "bytes");
                    }

                    string error;
                    data = FrameExtractor.Extract(data, cfg, out error);
                    if (error != null) { return Fail(error); }
                }

                return CoreResult(data, cfg, encode);
            }
            catch (FormatException ex) { return Fail(ex.Message); }
            catch (CryptographicException ex) { return Fail(string.Format(UI.T("Proto.CipherFail", "加解密失败：{0}"), ex.Message)); }
            catch (Exception ex) { return Fail(string.Format(UI.T("Proto.DecodeFail", "解码失败：{0}"), ex.Message)); }
        }

        /// <summary>智能解码命中一条。</summary>
        public sealed class CodecHit
        {
            public string Id;
            public string Name;
            public string Text;
            public string OutputBase64;
            public string Error;
            public bool Ok;
            /// <summary>命中的偏移；配置里已写死偏移时等于它。</summary>
            public int Offset;
        }

        /// <summary>
        /// 智能解码：依次试每个启用的解码器；偏移未写死的，再试 0~4 的偏移，
        /// 取第一个能解出「像明文」的结果。解不出可读明文的不计入。
        /// </summary>
        public static List<CodecHit> SmartDecode(byte[] data, IEnumerable<DecoderInfo> decoders)
        {
            return SmartDecode(data, decoders, null);
        }

        /// <summary>智能解码的带上下文版本：只尝试适用于当前封包的启用解码器。</summary>
        public static List<CodecHit> SmartDecode(byte[] data, IEnumerable<DecoderInfo> decoders,
            Operate.PacketConfig.Packet.PacketType? packetType)
        {
            List<CodecHit> hits = new List<CodecHit>();
            if (data == null || decoders == null) { return hits; }

            foreach (DecoderInfo d in decoders)
            {
                if (d == null || !d.IsEnable || (packetType.HasValue && !AppliesTo(d, packetType.Value))) { continue; }

                int[] offsets = d.DataOffset > 0 ? new int[] { d.DataOffset } : new int[] { 0, 1, 2, 3, 4 };
                CodecResult best = null;
                int bestOffset = 0;

                foreach (int off in offsets)
                {
                    DecoderInfo cfg = d.Clone();
                    cfg.DataOffset = off;

                    CodecResult r;
                    try { r = Process(data, cfg, false, true); }
                    catch { continue; }

                    if (r.Ok && IsSmartCandidate(cfg, r)) { best = r; bestOffset = off; break; }
                }

                if (best != null)
                {
                    hits.Add(new CodecHit
                    {
                        Id = d.GUID.ToString().ToUpper(),
                        Name = d.Name,
                        Text = best.Text,
                        OutputBase64 = best.OutputBase64,
                        Ok = true,
                        Offset = bestOffset,
                    });
                }
            }

            return hits;
        }

        /// <summary>范围判断集中在此处，批量、单包和智能解码共用同一口径。</summary>
        public static bool AppliesTo(DecoderInfo cfg, Operate.PacketConfig.Packet.PacketType packetType)
        {
            if (cfg == null) { return false; }

            DecoderProtocol actualProtocol;
            DecoderDirection actualDirection;
            switch (packetType)
            {
                case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                case Operate.PacketConfig.Packet.PacketType.WSASend:
                case Operate.PacketConfig.Packet.PacketType.TCP_Req:
                    actualProtocol = DecoderProtocol.Tcp; actualDirection = DecoderDirection.Request; break;
                case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                case Operate.PacketConfig.Packet.PacketType.WSARecv:
                case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                case Operate.PacketConfig.Packet.PacketType.TCP_Resp:
                    actualProtocol = DecoderProtocol.Tcp; actualDirection = DecoderDirection.Response; break;
                case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                case Operate.PacketConfig.Packet.PacketType.WSASendTo:
                case Operate.PacketConfig.Packet.PacketType.UDP_Req:
                    actualProtocol = DecoderProtocol.Udp; actualDirection = DecoderDirection.Request; break;
                case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                case Operate.PacketConfig.Packet.PacketType.WSARecvFrom:
                case Operate.PacketConfig.Packet.PacketType.UDP_Resp:
                    actualProtocol = DecoderProtocol.Udp; actualDirection = DecoderDirection.Response; break;
                case Operate.PacketConfig.Packet.PacketType.HTTP_Req:
                case Operate.PacketConfig.Packet.PacketType.HTTPS_Req:
                    actualProtocol = DecoderProtocol.Http; actualDirection = DecoderDirection.Request; break;
                case Operate.PacketConfig.Packet.PacketType.HTTP_Resp:
                case Operate.PacketConfig.Packet.PacketType.HTTPS_Resp:
                    actualProtocol = DecoderProtocol.Http; actualDirection = DecoderDirection.Response; break;
                case Operate.PacketConfig.Packet.PacketType.WebSocket_Req:
                    actualProtocol = DecoderProtocol.WebSocket; actualDirection = DecoderDirection.Request; break;
                case Operate.PacketConfig.Packet.PacketType.WebSocket_Resp:
                    actualProtocol = DecoderProtocol.WebSocket; actualDirection = DecoderDirection.Response; break;
                default:
                    return cfg.ProtocolType == DecoderProtocol.Any && cfg.Direction == DecoderDirection.Any;
            }

            return (cfg.ProtocolType == DecoderProtocol.Any || cfg.ProtocolType == actualProtocol)
                && (cfg.Direction == DecoderDirection.Any || cfg.Direction == actualDirection);
        }

        private static CodecResult ScopeMismatch(DecoderInfo cfg, Operate.PacketConfig.Packet.PacketType packetType)
        {
            return Fail(UI.T("Dec.ScopeMismatch", "当前封包不在此解码器的协议或方向适用范围内"));
        }

        /// <summary>
        /// 结构化格式的解析成功本身就是候选依据；文本则做 Unicode 质量检查。
        /// 不能再要求英文空格和英文单词，否则中文、JSON 数值以及 Protobuf/MessagePack 都会被错过。
        /// </summary>
        private static bool IsSmartCandidate(DecoderInfo cfg, CodecResult result)
        {
            if (cfg.Kind == DecoderKind.Protobuf || cfg.Kind == DecoderKind.MessagePack || cfg.Kind == DecoderKind.Bson || cfg.Kind == DecoderKind.Amf || cfg.Kind == DecoderKind.FlatBuffers) { return true; }
            return LooksLikeText(result.Text);
        }

        /// <summary>解出来的字节像不像可读文本，同时避免纯标点的错误密钥假命中。</summary>
        private static bool LooksLikeText(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < 2) { return false; }

            int total = 0;
            int good = 0;
            int meaningful = 0;

            foreach (char c in text)
            {
                total++;
                // ⚠️ SafeText 把 \0 换成了中点（·，U+00B7）。它是「不可打印字节」的记号，
                // 不能当普通字符放过 —— 否则偏移多跳一格、开头多出个 \0，也照样判成可读明文。
                if (c == '\u00B7' || c == '\uFFFD') { continue; }
                if (char.IsControl(c) && c != '\r' && c != '\n' && c != '\t') { continue; }
                good++;
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)) { meaningful++; }
            }

            if (total == 0 || (good * 100 / total) < 100) { return false; }
            return meaningful * 100 / total >= 50;
        }

        private static CodecResult CoreResult(byte[] data, DecoderInfo cfg, bool encode)
        {
            switch (cfg.Kind)
            {
                case DecoderKind.Xor:
                case DecoderKind.Aes:
                case DecoderKind.Des:
                    return Success(Core(data, cfg, encode), null, "bytes");

                case DecoderKind.TextCharset:
                    return TextCharset(data, cfg, encode);

                case DecoderKind.Protobuf:
                {
                    // 只解码，不做编码；编码方向回送原样字节以便往返验证不出错
                    if (encode) { return Success(data, null, "bytes"); }
                    return Success(data, ProtocolDecoder.ProtobufGuess(data), "protobuf");
                }

                case DecoderKind.MessagePack:
                {
                    if (encode) { return Success(data, null, "bytes"); }
                    return Success(data, MsgpackGuess(data), "msgpack");
                }

                case DecoderKind.Rc4:
                    return Success(AdditionalDecoders.Rc4(data, ParseKey(cfg.Key, cfg.KeyFormat, true)), null, "bytes");

                case DecoderKind.Xxtea:
                    return Success(AdditionalDecoders.Xxtea(data, ParseKey(cfg.Key, cfg.KeyFormat, true), encode), null, "bytes");

                case DecoderKind.Bson:
                    if (encode) { return Success(data, null, "bytes"); }
                    return Success(data, AdditionalDecoders.Bson(data), "bson");

                case DecoderKind.Amf:
                    if (encode) { return Success(data, null, "bytes"); }
                    return Success(data, AdditionalDecoders.Amf(data), "amf");

                case DecoderKind.FlatBuffers:
                    if (encode) { return Success(data, null, "bytes"); }
                    return Success(data, AdditionalDecoders.FlatBuffers(data), "flatbuffers");

                default:
                    return Fail(UI.T("Dec.Unsupported", "不支持的解码器类型"));
            }
        }

        /// <summary>纯算法层：返回算法处理后的字节。</summary>
        private static byte[] Core(byte[] data, DecoderInfo cfg, bool encode)
        {
            switch (cfg.Kind)
            {
                case DecoderKind.Xor:
                    return Xor(data, ParseKey(cfg.Key, cfg.KeyFormat, true));

                case DecoderKind.Rc4:
                    return AdditionalDecoders.Rc4(data, ParseKey(cfg.Key, cfg.KeyFormat, true));

                case DecoderKind.Xxtea:
                    return AdditionalDecoders.Xxtea(data, ParseKey(cfg.Key, cfg.KeyFormat, true), encode);

                case DecoderKind.Aes:
                    return Symmetric(data, cfg, encode, true);

                case DecoderKind.Des:
                    return Symmetric(data, cfg, encode, false);

                default:
                    throw new FormatException(UI.T("Dec.Unsupported", "不支持的解码器类型"));
            }
        }

        // ────────────────────────────── XOR ──────────────────────────────

        private static byte[] Xor(byte[] input, byte[] key)
        {
            if (key.Length == 0) { throw new FormatException(UI.T("Proto.XorEmpty", "XOR 密钥不能为空")); }
            byte[] output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++) { output[i] = (byte)(input[i] ^ key[i % key.Length]); }
            return output;
        }

        // ────────────────────────── AES / DES ────────────────────────────

        private static byte[] Symmetric(byte[] data, DecoderInfo cfg, bool encode, bool aes)
        {
            byte[] key = ParseKey(cfg.Key, cfg.KeyFormat, true);
            byte[] iv = ParseKey(cfg.Iv, cfg.IvFormat, false);

            int blockSize = aes ? 16 : 8;

            bool keyOk = aes ? (key.Length == 16 || key.Length == 24 || key.Length == 32) : (key.Length == 8);
            if (!keyOk)
            {
                throw new FormatException(string.Format(
                    UI.T("Dec.KeySize", "{0} 密钥长度不是合法值"), aes ? "AES（16 / 24 / 32 字节）" : "DES（8 字节）"));
            }

            bool needIv = cfg.CipherMode != DecoderCipherMode.ECB;
            if (needIv && iv.Length != blockSize)
            {
                throw new FormatException(string.Format(UI.T("Dec.IvSize", "IV 必须是 {0} 字节"), blockSize));
            }

            // OFB：本机 .NET Framework 的对称 provider（AesCryptoServiceProvider / AesManaged）
            // 都拒绝这个模式，所以自己实现（OFB 是流模式，几十行就够，且结果可验）。
            if (cfg.CipherMode == DecoderCipherMode.OFB)
            {
                return Ofb(data, key, iv, blockSize, aes);
            }

            // CTS：本机 provider 一律报「指定的模式对此算法无效」，且 CTS 有多个变体、
            // 与 Fatbeans 未约定用哪一种。这里明确不支持，返回可读的错误，不做半对的结果。
            if (cfg.CipherMode == DecoderCipherMode.CTS)
            {
                throw new FormatException(UI.T("Dec.ModeUnsupported", "当前运行环境不支持 CTS 模式"));
            }

            /*
                Zeros 填充自己处理：实测 .NET 的 PaddingMode.Zeros 解密时<b>不会</b>剥掉尾部 0x00
                （加密 24 字节 → 32 字节，解密仍返回 32 字节带 8 个 0x00），与 Fatbeans 的预期不符。
                自己 pad / strip 才与「明文往返一致」对得上。
                ⚠️ 明文本身以 0x00 结尾时，strip 会把它们一并去掉 —— 这是所有 Zeros 实现的固有歧义。
            */
            bool zeros = cfg.Padding == DecoderPadding.Zeros;

            using (SymmetricAlgorithm alg = aes ? (SymmetricAlgorithm)Aes.Create() : DES.Create())
            {
                alg.Mode = ToCipherMode(cfg.CipherMode);
                alg.Padding = zeros ? PaddingMode.None : ToPadding(cfg.Padding);
                alg.Key = key;
                if (needIv) { alg.IV = iv; }

                if (encode)
                {
                    byte[] input = zeros ? ZeroPad(data, blockSize) : data;
                    using (ICryptoTransform enc = alg.CreateEncryptor())
                    {
                        return enc.TransformFinalBlock(input, 0, input.Length);
                    }
                }

                using (ICryptoTransform dec = alg.CreateDecryptor())
                {
                    byte[] output = dec.TransformFinalBlock(data, 0, data.Length);
                    return zeros ? StripZeros(output) : output;
                }
            }
        }

        private static byte[] ZeroPad(byte[] data, int blockSize)
        {
            int rem = data.Length % blockSize;
            if (rem == 0) { return data; }

            byte[] output = new byte[data.Length + (blockSize - rem)];
            Array.Copy(data, output, data.Length);
            return output;
        }

        private static byte[] StripZeros(byte[] data)
        {
            int end = data.Length;
            while (end > 0 && data[end - 1] == 0) { end--; }
            if (end == data.Length) { return data; }

            byte[] output = new byte[end];
            Array.Copy(data, output, end);
            return output;
        }

        /// <summary>
        /// OFB：流模式，keystream = E(feedback)，下一次的 feedback 就是这一次的 keystream。
        /// 与 CFB 一样忽略填充、加解密同一段代码（异或自反）。
        /// </summary>
        private static byte[] Ofb(byte[] data, byte[] key, byte[] iv, int blockSize, bool aes)
        {
            using (SymmetricAlgorithm alg = aes ? (SymmetricAlgorithm)Aes.Create() : DES.Create())
            {
                alg.Mode = CipherMode.ECB;
                alg.Padding = PaddingMode.None;
                alg.Key = key;

                using (ICryptoTransform cipher = alg.CreateEncryptor())
                {
                    byte[] output = new byte[data.Length];
                    byte[] feedback = new byte[blockSize];
                    byte[] keystream = new byte[blockSize];
                    Array.Copy(iv, feedback, blockSize);

                    int pos = 0;
                    while (pos < data.Length)
                    {
                        cipher.TransformBlock(feedback, 0, blockSize, keystream, 0);
                        Array.Copy(keystream, feedback, blockSize);

                        int n = Math.Min(blockSize, data.Length - pos);
                        for (int i = 0; i < n; i++) { output[pos + i] = (byte)(data[pos + i] ^ keystream[i]); }
                        pos += n;
                    }

                    return output;
                }
            }
        }

        private static CipherMode ToCipherMode(DecoderCipherMode mode)
        {
            switch (mode)
            {
                case DecoderCipherMode.ECB: return CipherMode.ECB;
                case DecoderCipherMode.OFB: return CipherMode.OFB;
                case DecoderCipherMode.CFB: return CipherMode.CFB;
                case DecoderCipherMode.CTS: return CipherMode.CTS;
                default: return CipherMode.CBC;
            }
        }

        private static PaddingMode ToPadding(DecoderPadding padding)
        {
            switch (padding)
            {
                case DecoderPadding.None: return PaddingMode.None;
                case DecoderPadding.Zeros: return PaddingMode.Zeros;
                case DecoderPadding.ANSIX923: return PaddingMode.ANSIX923;
                case DecoderPadding.ISO10126: return PaddingMode.ISO10126;
                default: return PaddingMode.PKCS7;
            }
        }

        // ───────────────────────── 文本编码 ───────────────────────────────

        private static CodecResult TextCharset(byte[] data, DecoderInfo cfg, bool encode)
        {
            string format = CharsetToFormat(cfg.Charset);

            if (format == "base64")
            {
                if (encode)
                {
                    string b64 = Operate.SystemConfig.TranscodeOne(string.Empty, false, "base64");
                    // 文本编码的 base64 编码方向：把输入当作原文文本（UTF-8）→ base64 串
                    b64 = Operate.SystemConfig.TranscodeOne(Encoding.UTF8.GetString(data), false, "base64");
                    return Success(Encoding.ASCII.GetBytes(b64), null, "base64");
                }

                string ascii = Encoding.ASCII.GetString(data);
                string decoded = Operate.SystemConfig.TranscodeOne(ascii, true, "base64");
                return Success(data, decoded, "text");
            }

            if (encode)
            {
                string hex = Operate.SystemConfig.TranscodeOne(Encoding.UTF8.GetString(data), false, format);
                byte[] bytes = HexToBytes(hex);
                return Success(bytes, null, "bytes");
            }

            string text = Operate.SystemConfig.TranscodeOne(BytesToHex(data), true, format);
            return Success(data, text, "text");
        }

        private static string CharsetToFormat(DecoderCharset charset)
        {
            switch (charset)
            {
                case DecoderCharset.GBK: return "gbk";
                case DecoderCharset.UTF7: return "utf7";
                case DecoderCharset.UTF16BE: return "utf16be";
                case DecoderCharset.UTF32: return "utf32";
                case DecoderCharset.UTF16LE: return "utf16le";
                case DecoderCharset.Base64: return "base64";
                case DecoderCharset.UTF8: return "utf8";
                default: return "default";
            }
        }

        // ───────────────────────── MessagePack 推测 ──────────────────────

        private static string MsgpackGuess(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            int pos = 0;
            int nodes = 0;
            try
            {
                ParseMsgpack(data, ref pos, 0, sb, ref nodes);
            }
            catch (FormatException) { throw; }
            catch (Exception ex)
            {
                throw new FormatException(string.Format(UI.T("Dec.MsgpackBad", "不是有效的 MessagePack：{0}"), ex.Message));
            }
            return sb.ToString();
        }

        private static void ParseMsgpack(byte[] b, ref int pos, int depth, StringBuilder sb, ref int nodes)
        {
            if (depth >= MaxProtoDepth) { sb.Append("…"); return; }
            if (nodes++ >= MaxProtoFields) { sb.Append("…"); return; }
            if (pos >= b.Length) { throw new FormatException(UI.T("Dec.MsgpackTrunc", "MessagePack 数据被截断")); }

            byte c = b[pos++];
            string pad = new string(' ', depth * 2);

            if (c <= 0x7f) { sb.Append(pad).Append((int)c).AppendLine(); return; }
            if (c >= 0xe0) { sb.Append(pad).Append((int)(sbyte)c).AppendLine(); return; }

            if (c >= 0x80 && c <= 0x8f) { AppendMap(b, ref pos, depth, sb, ref nodes, c & 0x0f); return; }
            if (c >= 0x90 && c <= 0x9f) { AppendArray(b, ref pos, depth, sb, ref nodes, c & 0x0f); return; }
            if (c >= 0xa0 && c <= 0xbf) { AppendStr(b, ref pos, sb, depth, c & 0x1f); return; }

            switch (c)
            {
                case 0xc0: sb.Append(pad).AppendLine("nil"); return;
                case 0xc2: sb.Append(pad).AppendLine("false"); return;
                case 0xc3: sb.Append(pad).AppendLine("true"); return;

                case 0xc4: AppendBin(b, ref pos, sb, depth, ReadU8(b, ref pos)); return;
                case 0xc5: AppendBin(b, ref pos, sb, depth, (int)ReadU16(b, ref pos)); return;
                case 0xc6: AppendBin(b, ref pos, sb, depth, checked((int)ReadU32(b, ref pos))); return;

                case 0xca: sb.Append(pad).Append("float32 ").AppendLine(ReadFloat(b, ref pos).ToString()); return;
                case 0xcb: sb.Append(pad).Append("float64 ").AppendLine(ReadDouble(b, ref pos).ToString()); return;

                case 0xcc: sb.Append(pad).Append((int)ReadU8(b, ref pos)).AppendLine(); return;
                case 0xcd: sb.Append(pad).Append((int)ReadU16(b, ref pos)).AppendLine(); return;
                case 0xce: sb.Append(pad).Append((long)ReadU32(b, ref pos)).AppendLine(); return;
                case 0xcf: sb.Append(pad).Append(ReadU64(b, ref pos)).AppendLine(); return;

                case 0xd0: sb.Append(pad).Append((int)(sbyte)ReadU8(b, ref pos)).AppendLine(); return;
                case 0xd1: sb.Append(pad).Append((int)(short)ReadU16(b, ref pos)).AppendLine(); return;
                case 0xd2: sb.Append(pad).Append((int)ReadU32(b, ref pos)).AppendLine(); return;
                case 0xd3: sb.Append(pad).Append((long)ReadU64(b, ref pos)).AppendLine(); return;

                case 0xd9: AppendStr(b, ref pos, sb, depth, ReadU8(b, ref pos)); return;
                case 0xda: AppendStr(b, ref pos, sb, depth, (int)ReadU16(b, ref pos)); return;
                case 0xdb: AppendStr(b, ref pos, sb, depth, checked((int)ReadU32(b, ref pos))); return;

                case 0xdc: AppendArray(b, ref pos, depth, sb, ref nodes, (int)ReadU16(b, ref pos)); return;
                case 0xdd: AppendArray(b, ref pos, depth, sb, ref nodes, checked((int)ReadU32(b, ref pos))); return;

                case 0xde: AppendMap(b, ref pos, depth, sb, ref nodes, (int)ReadU16(b, ref pos)); return;
                case 0xdf: AppendMap(b, ref pos, depth, sb, ref nodes, checked((int)ReadU32(b, ref pos))); return;

                case 0xc7: AppendExt(b, ref pos, sb, depth, ReadU8(b, ref pos)); return;
                case 0xc8: AppendExt(b, ref pos, sb, depth, (int)ReadU16(b, ref pos)); return;
                case 0xc9: AppendExt(b, ref pos, sb, depth, checked((int)ReadU32(b, ref pos))); return;

                case 0xd4: AppendExt(b, ref pos, sb, depth, 1); return;
                case 0xd5: AppendExt(b, ref pos, sb, depth, 2); return;
                case 0xd6: AppendExt(b, ref pos, sb, depth, 4); return;
                case 0xd7: AppendExt(b, ref pos, sb, depth, 8); return;
                case 0xd8: AppendExt(b, ref pos, sb, depth, 16); return;
            }

            throw new FormatException(string.Format(UI.T("Dec.MsgpackByte", "未知的 MessagePack 类型字节 0x{0:X2}"), c));
        }

        private static void AppendArray(byte[] b, ref int pos, int depth, StringBuilder sb, ref int nodes, int count)
        {
            sb.Append(new string(' ', depth * 2)).Append("array(").Append(count).Append(')').AppendLine();
            for (int i = 0; i < count; i++) { ParseMsgpack(b, ref pos, depth + 1, sb, ref nodes); }
        }

        private static void AppendMap(byte[] b, ref int pos, int depth, StringBuilder sb, ref int nodes, int count)
        {
            sb.Append(new string(' ', depth * 2)).Append("map(").Append(count).Append(')').AppendLine();
            for (int i = 0; i < count; i++)
            {
                ParseMsgpack(b, ref pos, depth + 1, sb, ref nodes); // key
                ParseMsgpack(b, ref pos, depth + 1, sb, ref nodes); // value
            }
        }

        private static void AppendStr(byte[] b, ref int pos, StringBuilder sb, int depth, int len)
        {
            string pad = new string(' ', depth * 2);
            Require(b, pos, len);
            string s;
            bool printable;
            try
            {
                s = new UTF8Encoding(false, true).GetString(b, pos, len);
                printable = LooksPrintable(s);
            }
            catch { s = string.Empty; printable = false; }
            pos += len;
            if (!printable) { sb.Append(pad).Append("str(").Append(len).AppendLine(") [binary]"); return; }
            sb.Append(pad).Append('"').Append(s.Replace("\"", "\\\"")).AppendLine("\"");
        }

        private static void AppendBin(byte[] b, ref int pos, StringBuilder sb, int depth, int len)
        {
            Require(b, pos, len);
            pos += len;
            sb.Append(new string(' ', depth * 2)).Append("bin(").Append(len).AppendLine(")");
        }

        private static void AppendExt(byte[] b, ref int pos, StringBuilder sb, int depth, int len)
        {
            Require(b, pos, 1 + len);
            byte type = b[pos];
            pos += 1 + len;
            sb.Append(new string(' ', depth * 2)).Append("ext(").Append(len).Append(", type ").Append((int)(sbyte)type).AppendLine(")");
        }

        private static void Require(byte[] b, int pos, int len)
        {
            if (len < 0 || pos > b.Length - len) { throw new FormatException(UI.T("Dec.MsgpackTrunc", "MessagePack 数据被截断")); }
        }

        private static byte ReadU8(byte[] b, ref int pos) { Require(b, pos, 1); return b[pos++]; }
        private static ushort ReadU16(byte[] b, ref int pos) { Require(b, pos, 2); ushort v = (ushort)((b[pos] << 8) | b[pos + 1]); pos += 2; return v; }
        private static uint ReadU32(byte[] b, ref int pos) { Require(b, pos, 4); uint v = (uint)((b[pos] << 24) | (b[pos + 1] << 16) | (b[pos + 2] << 8) | b[pos + 3]); pos += 4; return v; }
        private static ulong ReadU64(byte[] b, ref int pos) { Require(b, pos, 8); ulong v = 0; for (int i = 0; i < 8; i++) { v = (v << 8) | b[pos + i]; } pos += 8; return v; }
        private static float ReadFloat(byte[] b, ref int pos) { Require(b, pos, 4); float v = BitConverter.ToSingle(b, pos); pos += 4; return v; }
        private static double ReadDouble(byte[] b, ref int pos) { Require(b, pos, 8); double v = BitConverter.ToDouble(b, pos); pos += 8; return v; }

        // ────────────────────────── 公共小件 ─────────────────────────────

        /// <summary>解析密钥 / IV。required 为真时不允许为空。</summary>
        internal static byte[] ParseKey(string value, DecoderKeyFormat format, bool required)
        {
            string s = value ?? string.Empty;

            if (s.Trim().Length == 0)
            {
                if (required) { throw new FormatException(UI.T("Dec.KeyEmpty", "密钥不能为空")); }
                return new byte[0];
            }

            switch (format)
            {
                case DecoderKeyFormat.Base64:
                    try { return Convert.FromBase64String(s.Trim()); }
                    catch (FormatException) { throw new FormatException(UI.T("Dec.KeyBase64", "密钥不是有效的 Base64")); }

                case DecoderKeyFormat.Text:
                    return Encoding.UTF8.GetBytes(s);

                default:
                {
                    byte[] bytes = FrameExtractor.ParseHex(s);
                    if (bytes.Length == 0) { throw new FormatException(UI.T("Dec.KeyHex", "密钥不是有效的十六进制")); }
                    return bytes;
                }
            }
        }

        private static CodecResult Success(byte[] output, string text, string format)
        {
            if (output == null) { output = new byte[0]; }
            return new CodecResult
            {
                Ok = true,
                Text = text != null ? text : SafeText(output),
                OutputBase64 = Convert.ToBase64String(output),
                Format = format,
            };
        }

        private static CodecResult Fail(string error) { return new CodecResult { Ok = false, Error = error }; }

        private static string SafeText(byte[] bytes)
        {
            string text = new UTF8Encoding(false, false).GetString(bytes);
            return text.Replace("\0", "·");
        }

        private static bool LooksPrintable(string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            foreach (char c in value) { if (char.IsControl(c) && c != '\r' && c != '\n' && c != '\t') { return false; } }
            return true;
        }

        internal static string BytesToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++) { sb.Append(bytes[i].ToString("X2")); }
            return sb.ToString();
        }

        internal static byte[] HexToBytes(string hex)
        {
            string s = (hex ?? string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty).Trim();
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
