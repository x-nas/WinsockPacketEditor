using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 一个取值器是变量的命名空间与运行期作用域容器；它本身不匹配封包，
    /// 由滤镜的「取值器赋值」动作决定何时从封包写入变量。
    /// </summary>
    public enum PacketExtractorScope
    {
        Global = 0,
        Socket = 1,
        ProxySession = 2,
    }

    public enum PacketVariableKind
    {
        Constant = 0,
        PacketExtract = 1,
        Expression = 2,
    }

    public enum PacketVariableDataType
    {
        Integer = 0,
        Double = 1,
        Bytes = 2,
        String = 3,
    }

    public enum PacketVariableEncoding
    {
        Utf8 = 0,
        Gb18030 = 1,
        Big5 = 2,
    }

    public sealed class PacketExtractionSpec
    {
        public int Offset { get; set; }
        public int Length { get; set; }
        public bool RelativeToMatch { get; set; }
        public bool BigEndian { get; set; }
        public bool Signed { get; set; }
        public PacketVariableEncoding Encoding { get; set; }

        public PacketExtractionSpec()
        {
            Length = 1;
            Encoding = PacketVariableEncoding.Utf8;
        }
    }

    public sealed class PacketVariableInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PacketVariableKind Kind { get; set; }
        public PacketVariableDataType DataType { get; set; }
        public string Value { get; set; }
        /// <summary>仅供界面展示的运行期值；不保存、不下推到钩子进程。</summary>
        [JsonIgnore]
        public string CurrentValue { get; set; }
        public PacketExtractionSpec Extraction { get; set; }
        public int TtlSeconds { get; set; }

        public PacketVariableInfo()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Value = string.Empty;
            Extraction = new PacketExtractionSpec();
            DataType = PacketVariableDataType.String;
            Kind = PacketVariableKind.Constant;
        }
    }

    public sealed class PacketExtractorInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnable { get; set; }
        public PacketExtractorScope Scope { get; set; }
        public string Description { get; set; }
        public List<PacketVariableInfo> Variables { get; set; }

        public PacketExtractorInfo()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            //新建后先由用户在列表中显式启用，避免未配置完变量就参与封包处理。
            IsEnable = false;
            Description = string.Empty;
            Scope = PacketExtractorScope.Socket;
            Variables = new List<PacketVariableInfo>();
        }
    }

    /// <summary>
    /// 取值器列表的界面载荷（桥接 getPacketExtractors 的返回行）。
    ///
    /// PacketVariableInfo.CurrentValue 在模型上是 [JsonIgnore]（它只是显示用的运行期
    /// 快照，不落库、不下推到钩子进程），直接用 Newtonsoft 序列化 PacketExtractorInfo
    /// 时会被一并丢掉，界面的「当前值」列于是永远收不到值。这里专门投影一份带
    /// CurrentValue 的行，既不改动模型上「不持久化」的约束，也让外壳运行值与注入
    /// 目标回传的镜像都能显示出来。保存仍用 PacketExtractorInfo（CurrentValue 会被忽略）。
    /// </summary>
    public sealed class PacketExtractorRow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnable { get; set; }
        public PacketExtractorScope Scope { get; set; }
        public string Description { get; set; }
        public List<PacketVariableRow> Variables { get; set; }
    }

    /// <summary>取值器变量的界面载荷；比 PacketVariableInfo 多一个仅显示的 CurrentValue。</summary>
    public sealed class PacketVariableRow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PacketVariableKind Kind { get; set; }
        public PacketVariableDataType DataType { get; set; }
        public string Value { get; set; }
        public string CurrentValue { get; set; }
        public PacketExtractionSpec Extraction { get; set; }
        public int TtlSeconds { get; set; }
    }
}
