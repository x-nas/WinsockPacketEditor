using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace WinsockPacketEditor
{
    /// <summary>截图预置所需的补充算法 / 通用结构解析器；均只在用户按需解码时运行。</summary>
    internal static class AdditionalDecoders
    {
        internal static byte[] Rc4(byte[] input, byte[] key)
        {
            if (key == null || key.Length == 0) { throw new FormatException(UI.T("Dec.KeyEmpty", "密钥不能为空")); }
            byte[] s = new byte[256];
            for (int i = 0; i < 256; i++) { s[i] = (byte)i; }
            int j = 0;
            for (int i = 0; i < 256; i++) { j = (j + s[i] + key[i % key.Length]) & 255; byte t = s[i]; s[i] = s[j]; s[j] = t; }
            byte[] output = new byte[input.Length]; int x = 0; j = 0;
            for (int i = 0; i < input.Length; i++) { x = (x + 1) & 255; j = (j + s[x]) & 255; byte t = s[x]; s[x] = s[j]; s[j] = t; output[i] = (byte)(input[i] ^ s[(s[x] + s[j]) & 255]); }
            return output;
        }

        internal static byte[] Xxtea(byte[] input, byte[] key, bool encode)
        {
            if (key == null || key.Length != 16) { throw new FormatException(UI.T("Dec.XxteaKey", "XXTEA 密钥必须是 16 字节")); }
            if (input == null || input.Length == 0 || (input.Length & 3) != 0) { throw new FormatException(UI.T("Dec.XxteaLength", "XXTEA 数据长度必须是非零的 4 字节倍数")); }
            uint[] v = ToUInt32(input); uint[] k = ToUInt32(key); int n = v.Length;
            const uint Delta = 0x9E3779B9;
            if (encode)
            {
                uint sum = 0; int rounds = 6 + 52 / n; uint z = v[n - 1];
                while (rounds-- > 0) { sum += Delta; uint e = (sum >> 2) & 3; for (int p = 0; p < n - 1; p++) { uint y = v[p + 1]; z = v[p] += Mx(sum, y, z, p, e, k); } uint first = v[0]; z = v[n - 1] += Mx(sum, first, z, n - 1, e, k); }
            }
            else
            {
                int rounds = 6 + 52 / n; uint sum = (uint)(rounds * Delta); uint y = v[0];
                while (sum != 0) { uint e = (sum >> 2) & 3; for (int p = n - 1; p > 0; p--) { uint z = v[p - 1]; y = v[p] -= Mx(sum, y, z, p, e, k); } uint last = v[n - 1]; y = v[0] -= Mx(sum, y, last, 0, e, k); sum -= Delta; }
            }
            return FromUInt32(v);
        }

        private static uint Mx(uint sum, uint y, uint z, int p, uint e, uint[] k) { return (((z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)) ^ ((sum ^ y) + (k[(p & 3) ^ e] ^ z))); }
        private static uint[] ToUInt32(byte[] bytes) { uint[] v = new uint[bytes.Length / 4]; for (int i = 0; i < v.Length; i++) { int p = i * 4; v[i] = (uint)(bytes[p] | bytes[p + 1] << 8 | bytes[p + 2] << 16 | bytes[p + 3] << 24); } return v; }
        private static byte[] FromUInt32(uint[] v) { byte[] b = new byte[v.Length * 4]; for (int i = 0; i < v.Length; i++) { int p = i * 4; b[p] = (byte)v[i]; b[p + 1] = (byte)(v[i] >> 8); b[p + 2] = (byte)(v[i] >> 16); b[p + 3] = (byte)(v[i] >> 24); } return b; }

        internal static string Bson(byte[] data)
        {
            using (var ms = new MemoryStream(data, false))
            using (var reader = new BsonDataReader(ms))
            {
                JToken token = JToken.ReadFrom(reader);
                if (ms.Position != ms.Length) { throw new FormatException(UI.T("Dec.BsonTrailing", "BSON 后面还有未解析的数据")); }
                return token.ToString(Formatting.Indented);
            }
        }

        internal static string Amf(byte[] data)
        {
            if (data == null || data.Length == 0) { throw new FormatException(UI.T("Dec.AmfEmpty", "AMF 数据为空")); }
            var reader = new AmfReader(data);
            StringBuilder sb = new StringBuilder();
            reader.ReadValue(sb, 0, data[0] == 0x11);
            if (!reader.End) { sb.AppendLine().Append("… trailing bytes: ").Append(data.Length - reader.Position); }
            return sb.ToString();
        }

        internal static string FlatBuffers(byte[] data)
        {
            if (data == null || data.Length < 8) { throw new FormatException(UI.T("Dec.FlatShort", "FlatBuffers 数据不足")); }
            int root = ReadI32(data, 0); if (root < 4 || root >= data.Length - 4) { throw new FormatException(UI.T("Dec.FlatRoot", "不是有效的 FlatBuffers 根偏移")); }
            int vtable = root - ReadI32(data, root); if (vtable < 0 || vtable + 4 > data.Length) { throw new FormatException(UI.T("Dec.FlatVtable", "FlatBuffers vtable 无效")); }
            ushort vlen = ReadU16(data, vtable), objectLen = ReadU16(data, vtable + 2); if (vlen < 4 || vtable + vlen > data.Length || root + objectLen > data.Length) { throw new FormatException(UI.T("Dec.FlatVtable", "FlatBuffers vtable 无效")); }
            var sb = new StringBuilder(); sb.AppendLine("table (generic; configure a schema for field names)");
            for (int slot = 0; vtable + 4 + slot * 2 + 2 <= vtable + vlen; slot++) { ushort off = ReadU16(data, vtable + 4 + slot * 2); if (off == 0) { continue; } int p = root + off; if (p + 4 > data.Length) { throw new FormatException(UI.T("Dec.FlatField", "FlatBuffers 字段越界")); } sb.Append("  field ").Append(slot).Append(": u32=0x").Append(ReadU32(data, p).ToString("X8", CultureInfo.InvariantCulture)).AppendLine(); }
            return sb.ToString();
        }

        private static ushort ReadU16(byte[] b, int p) { if (p < 0 || p + 2 > b.Length) throw new FormatException("truncated"); return (ushort)(b[p] | b[p + 1] << 8); }
        private static int ReadI32(byte[] b, int p) { return unchecked((int)ReadU32(b, p)); }
        private static uint ReadU32(byte[] b, int p) { if (p < 0 || p + 4 > b.Length) throw new FormatException("truncated"); return (uint)(b[p] | b[p + 1] << 8 | b[p + 2] << 16 | b[p + 3] << 24); }

        private sealed class AmfReader
        {
            private readonly byte[] b; internal int Position; internal bool End { get { return Position >= b.Length; } }
            internal AmfReader(byte[] bytes) { b = bytes; }
            private byte U8() { if (End) throw new FormatException("AMF truncated"); return b[Position++]; }
            private ushort U16() { return (ushort)((U8() << 8) | U8()); }
            private double Double() { ulong v = 0; for (int i = 0; i < 8; i++) { v = (v << 8) | U8(); } return BitConverter.Int64BitsToDouble(unchecked((long)v)); }
            private uint U29() { uint x = 0; for (int i = 0; i < 4; i++) { byte c = U8(); if (i == 3) return (x << 8) | c; x = (x << 7) | (uint)(c & 127); if ((c & 128) == 0) return x; } return x; }
            private string Str(bool amf3) { int n = amf3 ? (int)(U29() >> 1) : U16(); if (n < 0 || Position + n > b.Length) throw new FormatException("AMF string truncated"); string s = Encoding.UTF8.GetString(b, Position, n); Position += n; return s; }
            internal void ReadValue(StringBuilder sb, int depth, bool amf3)
            {
                if (depth > 16) { sb.Append("…"); return; }
                byte type = U8(); if (!amf3 && type == 0x11) { ReadValue(sb, depth, true); return; }
                if (amf3) { ReadAmf3(sb, depth, type); return; }
                switch (type) { case 0: sb.Append(Double().ToString(CultureInfo.InvariantCulture)); break; case 1: sb.Append(U8() == 0 ? "false" : "true"); break; case 2: sb.Append('"').Append(Str(false)).Append('"'); break; case 3: Obj(sb, depth, false); break; case 5: sb.Append("null"); break; case 6: sb.Append("undefined"); break; case 8: U8(); U8(); U8(); U8(); Obj(sb, depth, false); break; case 10: int n=U16(); sb.Append("["); for(int i=0;i<n;i++){if(i>0)sb.Append(", ");ReadValue(sb,depth+1,false);} sb.Append("]"); break; case 12: int l=(int)ReadU32(b,Position);Position+=4; if(Position+l>b.Length)throw new FormatException("AMF string truncated");sb.Append('"').Append(Encoding.UTF8.GetString(b,Position,l)).Append('"');Position+=l;break; default: throw new FormatException("unsupported AMF0 marker 0x"+type.ToString("X2")); }
            }
            private void Obj(StringBuilder sb,int d,bool a3) { sb.Append("{"); bool first=true; while(true){ if(!a3 && Position+3<=b.Length && b[Position]==0&&b[Position+1]==0&&b[Position+2]==9){Position+=3;break;} string k=Str(a3); if(a3 && k.Length==0)break; if(!first)sb.Append(", ");first=false;sb.Append(k).Append(": ");ReadValue(sb,d+1,a3);} sb.Append("}"); }
            private void ReadAmf3(StringBuilder sb,int d,byte t) { switch(t){case 0:sb.Append("undefined");break;case 1:sb.Append("null");break;case 2:sb.Append("false");break;case 3:sb.Append("true");break;case 4:uint u=U29();sb.Append((u&0x10000000)!=0?(int)(u-0x20000000):(int)u);break;case 5:sb.Append("double");Position+=8;break;case 6:sb.Append('"').Append(Str(true)).Append('"');break;case 9:uint h=U29();int n=(int)(h>>1);if((h&1)==0)throw new FormatException("AMF3 references are unsupported");Str(true);sb.Append("[");for(int i=0;i<n;i++){if(i>0)sb.Append(", ");ReadValue(sb,d+1,true);}sb.Append("]");break;default:throw new FormatException("unsupported AMF3 marker 0x"+t.ToString("X2"));} }
        }
    }
}
