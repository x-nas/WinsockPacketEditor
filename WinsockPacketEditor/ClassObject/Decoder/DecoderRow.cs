using System;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 解码器的界面 DTO。字段名与前端 <c>DecoderRow</c> 逐字一致（桥不配驼峰）。
    ///
    /// 枚举一律按 int 过桥（前端不依赖 C# 的枚举名），Id 用字符串形式的 GUID。
    /// </summary>
    public class DecoderRow
    {
        public string Id;
        public bool IsEnable;

        public string Name;
        public string Description;

        public int Kind;
        public int Charset;
        public int KeyFormat;
        public string Key;
        public int IvFormat;
        public string Iv;
        public int CipherMode;
        public int Padding;
        public int BlockSize;

        public int LengthBytes;
        public bool BigEndian;
        public bool LengthIncludesSelf;
        public bool HasFixedHeader;
        public string FixedHeader;
        public bool LengthIncludesFixedHeader;
        public int DataOffset;

        public int ProtocolType;
        public int Direction;

        public string ParamsJson;
    }
}
