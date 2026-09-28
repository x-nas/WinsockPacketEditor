using System;

namespace WinsockPacketEditor
{
    /*
        解码器（跨模式共用，注入 / 代理两模式同一份）。

        命名刻意不带 Proxy 前缀：它与滤镜 / 发送 / 机器人 / 仓库同属「两模式共用」的子系统，
        表叫 Decoder、子系统叫 Operate.DecoderConfig，与 Filter / Send / Robot 一个层级。

        字段分四组：算法（Kind + 密钥 + 模式）、帧（LengthBytes 一族）、范围（ProtocolType /
        Direction）、扩展（ParamsJson）。加新算法时公共字段进列、专属小参数进 ParamsJson，
        这样不必每次都改表结构。
    */

    /// <summary>解码器的算法类型。存 int，新增往后加、不要改已有值。</summary>
    public enum DecoderKind
    {
        Xor = 1,
        Aes = 2,
        Des = 3,
        Protobuf = 4,
        MessagePack = 5,
        Rc4 = 6,
        Xxtea = 7,
        Amf = 8,
        TextCharset = 9,
        Bson = 10,
        FlatBuffers = 11,
    }

    /// <summary>解码器适用的封包协议范围（对应 Fatbeans 的「协议配置」）。</summary>
    public enum DecoderProtocol
    {
        Any = 0,
        Tcp = 1,
        Udp = 2,
        Http = 3,
        WebSocket = 4,
    }

    /// <summary>适用范围里的方向，只对 HTTP 有意义。</summary>
    public enum DecoderDirection
    {
        Any = 0,
        Request = 1,
        Response = 2,
    }

    /// <summary>密钥 / IV 的书写格式。</summary>
    public enum DecoderKeyFormat
    {
        Hex = 0,
        Base64 = 1,
        Text = 2,
    }

    /// <summary>对称加密的分组模式。CTS 为旧配置保留的值，界面不再提供且保存时会拒绝。</summary>
    public enum DecoderCipherMode
    {
        CBC = 0,
        ECB = 1,
        OFB = 2,
        CFB = 3,
        CTS = 4,
    }

    /// <summary>对称加密的填充（与 System.Security.Cryptography.PaddingMode 一一对应）。</summary>
    public enum DecoderPadding
    {
        None = 0,
        PKCS7 = 1,
        Zeros = 2,
        ANSIX923 = 3,
        ISO10126 = 4,
    }

    /// <summary>文本编码解码器用的字符集，与 Operate.SystemConfig.TranscodeOne 的 format 串对应。</summary>
    public enum DecoderCharset
    {
        Default = 0,
        GBK = 1,
        UTF7 = 2,
        UTF8 = 3,
        UTF16BE = 4,
        UTF32 = 5,
        UTF16LE = 6,
        Base64 = 7,
    }

    /// <summary>
    /// 一个解码器的完整配置。纯数据类，不碰界面、不进热路径。
    ///
    /// <b>字段名与 Decoder 表的列名一一对应</b>（GUID / IsEnable / Name / …），
    /// 桥返回给前端时另拼 DecoderRow（见 ShellForm.getDecoders），不要直接把它序列化出去。
    /// </summary>
    public class DecoderInfo
    {
        public Guid GUID { get; set; }

        public bool IsEnable { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        // ── 算法 ──
        public DecoderKind Kind { get; set; }

        public DecoderCharset Charset { get; set; }

        public DecoderKeyFormat KeyFormat { get; set; }

        public string Key { get; set; }

        public DecoderKeyFormat IvFormat { get; set; }

        public string Iv { get; set; }

        public DecoderCipherMode CipherMode { get; set; }

        public DecoderPadding Padding { get; set; }

        public int BlockSize { get; set; }

        // ── 帧 ──
        public int LengthBytes { get; set; }

        public bool BigEndian { get; set; }

        public bool LengthIncludesSelf { get; set; }

        public bool HasFixedHeader { get; set; }

        public string FixedHeader { get; set; }

        public bool LengthIncludesFixedHeader { get; set; }

        public int DataOffset { get; set; }

        // ── 适用范围 ──
        public DecoderProtocol ProtocolType { get; set; }

        public DecoderDirection Direction { get; set; }

        // ── 扩展位 ──
        public string ParamsJson { get; set; }

        public DecoderInfo()
        {
            Name = string.Empty;
            Description = string.Empty;
            Kind = DecoderKind.Xor;
            Charset = DecoderCharset.UTF8;
            KeyFormat = DecoderKeyFormat.Hex;
            Key = string.Empty;
            IvFormat = DecoderKeyFormat.Hex;
            Iv = string.Empty;
            CipherMode = DecoderCipherMode.CBC;
            Padding = DecoderPadding.PKCS7;
            FixedHeader = string.Empty;
            ParamsJson = string.Empty;
            IsEnable = true;
            ProtocolType = DecoderProtocol.Any;
            Direction = DecoderDirection.Any;
        }

        /// <summary>深拷贝。测试台与编辑弹窗改的是副本，保存成功后才回写列表。</summary>
        public DecoderInfo Clone()
        {
            return (DecoderInfo)MemberwiseClone();
        }

        /// <summary>新建时给的默认名。</summary>
        public static string DefaultName(string kindLabel, int sequence)
        {
            return kindLabel + " " + sequence.ToString();
        }
    }
}
