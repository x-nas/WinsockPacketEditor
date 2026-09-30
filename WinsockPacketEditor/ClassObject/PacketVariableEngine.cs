using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 封包变量运行时。配置与运行值刻意分离：配置由外壳持久化并下推，值只留在
    /// 实际处理封包的进程，避免钩子线程为每个包走 IPC。
    /// </summary>
    public static class PacketVariableEngine
    {
        private sealed class RuntimeValue
        {
            public object Value;
            public DateTime UpdatedUtc;
        }

        private static readonly ConcurrentDictionary<string, RuntimeValue> values = new ConcurrentDictionary<string, RuntimeValue>();
        // 注入模式下真实运行值在目标进程；外壳仅保留 IPC 回传的显示镜像，绝不参与变量计算。
        private static readonly ConcurrentDictionary<string, string> mirroredDisplays = new ConcurrentDictionary<string, string>();
        private static volatile PacketExtractorInfo[] extractors = new PacketExtractorInfo[0];
        // 表达式变量总数：热路径上 RecalculateExpressions 要拿它当轮数上限，
        // 每次现算（LINQ)会在「只有常量/封包取值变量」的滤镜上白白分配，所以在 Publish 时缓存一份。
        private static volatile int expressionCount = 0;
        private static readonly Regex token = new Regex(@"\$\{(?<extractor>[^.}:]+)\.(?<variable>[^}:]+)(:(?<format>[^}]+))?\}", RegexOptions.Compiled);

        public static void Publish(IEnumerable<PacketExtractorInfo> source)
        {
            extractors = (source ?? Enumerable.Empty<PacketExtractorInfo>()).Where(x => x != null).ToArray();
            expressionCount = extractors.Sum(x => x.Variables == null ? 0 : x.Variables.Count(v => v != null && v.Kind == PacketVariableKind.Expression));
            values.Clear();
            mirroredDisplays.Clear();
        }

        public static void ClearScope(PacketExtractorScope scope, long key)
        {
            string suffix = ":" + ScopeKey(scope, key) + ":";
            foreach (string k in values.Keys) if (k.IndexOf(suffix, StringComparison.Ordinal) >= 0) { RuntimeValue ignored; values.TryRemove(k, out ignored); }
        }

        /// <summary>
        /// 供配置界面显示变量当前值。全局变量取唯一值；Socket / ProxySession 变量取未过期的最近一次值。
        /// 这只是显示快照，不参与热路径，也不改变运行值。
        /// </summary>
        public static string GetCurrentDisplay(PacketExtractorInfo extractor, PacketVariableInfo variable)
        {
            if (extractor == null || variable == null) return "—";
            object constant;
            if (variable.Kind == PacketVariableKind.Constant)
            {
                return TryParseConstant(variable, out constant) ? DisplayValue(constant, variable.DataType) : "—";
            }

            RuntimeValue current = null;
            if (extractor.Scope == PacketExtractorScope.Global)
            {
                values.TryGetValue(Key(extractor.Id, variable.Id, extractor.Scope, 0), out current);
            }
            else
            {
                string prefix = extractor.Id.ToString("N") + ":";
                string suffix = ":" + variable.Id.ToString("N");
                current = values.Where(x => x.Key.StartsWith(prefix, StringComparison.Ordinal) && x.Key.EndsWith(suffix, StringComparison.Ordinal) && !IsExpired(variable, x.Value))
                    .Select(x => x.Value).OrderByDescending(x => x.UpdatedUtc).FirstOrDefault();
            }
            if (current != null && !IsExpired(variable, current)) return DisplayValue(current.Value, variable.DataType);

            string mirrored;
            return mirroredDisplays.TryGetValue(DisplayKey(extractor.Id, variable.Id), out mirrored) ? mirrored : "—";
        }

        /// <summary>外壳接收注入目标回传的显示快照；只给配置界面读，绝不回灌到运行值字典。</summary>
        public static void SetMirroredDisplay(Guid extractorId, Guid variableId, string display)
        {
            mirroredDisplays[DisplayKey(extractorId, variableId)] = display ?? "—";
        }

        /// <summary>注入目标随 Stats 回传的非固定变量快照；值截断，保证事件帧始终受控。</summary>
        public static List<PacketVariableDisplay> GetCurrentDisplays(int maxChars)
        {
            var result = new List<PacketVariableDisplay>();
            foreach (PacketExtractorInfo extractor in extractors)
            {
                if (extractor == null || extractor.Variables == null) continue;
                foreach (PacketVariableInfo variable in extractor.Variables)
                {
                    if (variable == null || variable.Kind == PacketVariableKind.Constant) continue;
                    string text = GetCurrentDisplay(extractor, variable) ?? "—";
                    if (maxChars > 0 && text.Length > maxChars) text = text.Substring(0, maxChars) + "…";
                    result.Add(new PacketVariableDisplay { ExtractorId = extractor.Id, VariableId = variable.Id, Display = text });
                }
            }
            return result;
        }

        private static string DisplayKey(Guid extractorId, Guid variableId) { return extractorId.ToString("N") + ":" + variableId.ToString("N"); }

        public static bool TryCapture(Guid extractorId, Guid variableId, long scopeKey, byte[] buffer, int matchOffset, out string error)
        {
            error = null;
            PacketExtractorInfo extractor = extractors.FirstOrDefault(x => x.Id == extractorId && x.IsEnable);
            if (extractor == null) { error = "取值器不存在或未启用"; return false; }
            PacketVariableInfo variable = extractor.Variables.FirstOrDefault(x => x.Id == variableId);
            if (variable == null || variable.Kind != PacketVariableKind.PacketExtract) { error = "目标不是封包取值变量"; return false; }
            if (buffer == null) { error = "封包为空"; return false; }

            PacketExtractionSpec spec = variable.Extraction ?? new PacketExtractionSpec();
            int start = spec.Offset + (spec.RelativeToMatch ? matchOffset : 0);
            if (start < 0 || spec.Length < 1 || start > buffer.Length || spec.Length > buffer.Length - start) { error = "取值范围超出封包长度"; return false; }
            byte[] raw = new byte[spec.Length];
            Buffer.BlockCopy(buffer, start, raw, 0, raw.Length);
            object parsed;
            try { parsed = Parse(raw, variable.DataType, spec); }
            catch (Exception ex) { error = "取值转换失败：" + ex.Message; return false; }
            values[Key(extractor.Id, variable.Id, extractor.Scope, scopeKey)] = new RuntimeValue { Value = parsed, UpdatedUtc = DateTime.UtcNow };
            RecalculateExpressions(scopeKey);
            return true;
        }

        /// <summary>
        /// 渲染前按作用域把所有表达式变量重算一轮。
        /// 一个滤镜可以有多个替换格，逐格调用 <see cref="TryRenderVariable"/> 会各算一遍，
        /// 所以由调用方先 Prepare 一次，再连着渲染（热路径上省下重复的表达式遍历）。
        /// 没有表达式变量时几乎零成本。
        /// </summary>
        public static void Prepare(long scopeKey) { RecalculateExpressions(scopeKey); }

        /// <summary>
        /// 按 GUID 渲染一个取值器变量为字节。
        /// 旧模板按名称匹配，改个名字就静默失效；替换格用 GUID，改名不再影响。
        /// 调用前应先 <see cref="Prepare"/>；这里不再重算表达式。
        /// </summary>
        public static bool TryRenderVariable(Guid extractorId, Guid variableId, string format, long scopeKey, out byte[] output, out string error)
        {
            error = null; output = null;
            PacketExtractorInfo e = extractors.FirstOrDefault(x => x.Id == extractorId);
            if (e == null || !e.IsEnable) { error = "取值器不存在或未启用"; return false; }
            PacketVariableInfo v = e.Variables == null ? null : e.Variables.FirstOrDefault(x => x.Id == variableId);
            if (v == null) { error = "变量不存在"; return false; }
            object value;
            if (v.Kind == PacketVariableKind.Constant)
            {
                if (!TryParseConstant(v, out value)) { error = "变量值无效：" + v.Name; return false; }
            }
            else
            {
                RuntimeValue runtime;
                if (!values.TryGetValue(Key(e.Id, v.Id, e.Scope, scopeKey), out runtime) || IsExpired(v, runtime) || runtime.Value == null)
                { error = "变量未取到或已过期：" + v.Name; return false; }
                value = runtime.Value;
            }
            //数值变量的写入宽度由格式决定，不能留空（ToBytes 空格式会退化成 8 字节）。
            if ((v.DataType == PacketVariableDataType.Integer || v.DataType == PacketVariableDataType.Double) && string.IsNullOrEmpty(format))
            { error = "数值变量必须指定格式：" + v.Name; return false; }
            return ToBytes(value, v.DataType, format, v.Extraction, out output, out error);
        }

        /// <summary>界面/保存校验用：某个数据类型的变量允许哪些写入格式。</summary>
        public static bool IsFormatValid(PacketVariableDataType type, string format)
        {
            string f = (format ?? string.Empty).ToLowerInvariant();
            switch (type)
            {
                case PacketVariableDataType.Integer:
                    return f == "u8" || f == "u16le" || f == "u16be" || f == "u32le" || f == "u32be" || f == "u64le" || f == "u64be";
                case PacketVariableDataType.Double:
                    return f == "f32le" || f == "f32be" || f == "f64le" || f == "f64be";
                case PacketVariableDataType.String:
                    return f.Length == 0 || f == "utf8" || f == "gb18030" || f == "big5";
                default:
                    return f.Length == 0 || f == "hex";
            }
        }

        private static void RecalculateExpressions(long scopeKey)
        {
            // 按多轮计算解决表达式依赖表达式的常见链路；轮数上限同时阻断循环引用。
            int count = expressionCount;
            for (int round = 0; round < count; round++)
            {
                bool changed = false;
                foreach (PacketExtractorInfo extractor in extractors.Where(x => x.IsEnable))
                foreach (PacketVariableInfo variable in extractor.Variables.Where(x => x.Kind == PacketVariableKind.Expression))
                {
                    object result;
                    if (!TryEvaluateExpression(variable, scopeKey, out result)) continue;
                    string key = Key(extractor.Id, variable.Id, extractor.Scope, scopeKey);
                    RuntimeValue old;
                    if (!values.TryGetValue(key, out old) || !SameValue(old.Value, result)) changed = true;
                    values[key] = new RuntimeValue { Value = result, UpdatedUtc = DateTime.UtcNow };
                }
                if (!changed) break;
            }
        }

        private static bool TryEvaluateExpression(PacketVariableInfo variable, long scopeKey, out object result)
        {
            result = null;
            string expression = variable.Value ?? string.Empty;
            switch (variable.DataType)
            {
                case PacketVariableDataType.Integer:
                    {
                        string expanded;
                        if (!TryExpandNumericTokens(expression, scopeKey, false, out expanded)) return false;
                        long number;
                        if (!IntegerExpression.TryEvaluate(expanded, out number)) return false;
                        result = number; return true;
                    }
                case PacketVariableDataType.Double:
                    {
                        string expanded;
                        if (!TryExpandNumericTokens(expression, scopeKey, true, out expanded)) return false;
                        double number;
                        if (!DoubleExpression.TryEvaluate(expanded, out number) || double.IsInfinity(number) || double.IsNaN(number)) return false;
                        result = number; return true;
                    }
                case PacketVariableDataType.Bytes:
                    {
                        byte[] bytes;
                        if (!TryEvaluateBytes(expression, scopeKey, out bytes)) return false;
                        result = bytes; return true;
                    }
                default:
                    result = EvaluateString(expression, scopeKey); return result != null;
            }
        }

        private static bool TryExpandNumericTokens(string expression, long scopeKey, bool floating, out string expanded)
        {
            bool ok = true;
            expanded = token.Replace(expression ?? string.Empty, m =>
            {
                object value; PacketVariableDataType type;
                if (!TryGetReference(m, scopeKey, out value, out type)) { ok = false; return "0"; }
                if (type == PacketVariableDataType.Integer) return Convert.ToInt64(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                if (floating && type == PacketVariableDataType.Double) return Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString("R", CultureInfo.InvariantCulture);
                ok = false; return "0";
            });
            return ok;
        }

        private static string EvaluateString(string expression, long scopeKey)
        {
            List<string> terms;
            if (!TrySplitTerms(expression, out terms)) return null;
            var text = new StringBuilder(); bool ok = true;
            foreach (string raw in terms)
            {
                string term = raw.Trim();
                Match m = token.Match(term);
                if (m.Success && m.Length == term.Length)
                {
                    object value; PacketVariableDataType type;
                    if (!TryGetReference(m, scopeKey, out value, out type)) return null;
                    text.Append(DisplayValue(value, type));
                }
                else if (IsQuoted(term)) text.Append(Unquote(term));
                else text.Append(token.Replace(raw, x =>
                {
                    object value; PacketVariableDataType type;
                    if (TryGetReference(x, scopeKey, out value, out type)) return DisplayValue(value, type);
                    ok = false; return string.Empty;
                }));
            }
            return ok ? text.ToString() : null;
        }

        private static bool TryEvaluateBytes(string expression, long scopeKey, out byte[] result)
        {
            result = null; List<string> terms;
            if (!TrySplitTerms(expression, out terms)) return false;
            var bytes = new List<byte>();
            foreach (string raw in terms)
            {
                string term = raw.Trim();
                Match m = token.Match(term);
                if (m.Success && m.Length == term.Length)
                {
                    object value; PacketVariableDataType type;
                    if (!TryGetReference(m, scopeKey, out value, out type)) return false;
                    byte[] part; string error;
                    // 字节数组表达式也支持 ${取值器.变量:格式}；数值需明确长度和端序，
                    // 字符串可选 utf8 / gb18030 / big5，字节数组不带格式即原样拼接。
                    if (!ToBytes(value, type, m.Groups["format"].Success ? m.Groups["format"].Value : null, new PacketExtractionSpec(), out part, out error)) return false;
                    bytes.AddRange(part);
                }
                else
                {
                    byte[] ignored; string error;
                    if (!AppendHex(IsQuoted(term) ? Unquote(term) : term, bytes, out error, out ignored)) return false;
                }
            }
            result = bytes.ToArray(); return true;
        }

        private static bool TryGetReference(Match match, long scopeKey, out object value, out PacketVariableDataType type)
        {
            value = null; type = PacketVariableDataType.String;
            PacketExtractorInfo extractor = extractors.FirstOrDefault(x => string.Equals(x.Name, match.Groups["extractor"].Value, StringComparison.Ordinal));
            PacketVariableInfo variable = extractor == null || extractor.Variables == null ? null : extractor.Variables.FirstOrDefault(x => string.Equals(x.Name, match.Groups["variable"].Value, StringComparison.Ordinal));
            if (extractor == null || variable == null || !extractor.IsEnable) return false;
            type = variable.DataType;
            if (variable.Kind == PacketVariableKind.Constant) return TryParseConstant(variable, out value);
            RuntimeValue runtime;
            return values.TryGetValue(Key(extractor.Id, variable.Id, extractor.Scope, scopeKey), out runtime) && !IsExpired(variable, runtime) && (value = runtime.Value) != null;
        }

        private static bool TryParseConstant(PacketVariableInfo variable, out object value)
        {
            value = null; string raw = variable.Value ?? string.Empty;
            long integer; double number; byte[] bytes; string error;
            switch (variable.DataType)
            {
                case PacketVariableDataType.Integer: if (!long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out integer)) return false; value = integer; return true;
                case PacketVariableDataType.Double: if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out number)) return false; value = number; return true;
                case PacketVariableDataType.Bytes: return AppendHex(raw, new List<byte>(), out error, out bytes) && (value = bytes) != null;
                default: value = raw; return true;
            }
        }

        private static bool TrySplitTerms(string expression, out List<string> terms)
        {
            terms = new List<string>(); var current = new StringBuilder(); char quote = '\0'; int braces = 0;
            foreach (char c in expression ?? string.Empty)
            {
                if (quote != '\0') { if (c == quote) quote = '\0'; current.Append(c); continue; }
                if (c == '\'' || c == '"') { quote = c; current.Append(c); continue; }
                if (c == '{') braces++; else if (c == '}' && braces > 0) braces--;
                if (c == '+' && braces == 0) { terms.Add(current.ToString()); current.Clear(); } else current.Append(c);
            }
            if (quote != '\0' || braces != 0) return false;
            terms.Add(current.ToString()); return terms.All(x => x.Trim().Length > 0);
        }

        private static bool IsQuoted(string value) { return value != null && value.Length >= 2 && ((value[0] == '\'' && value[value.Length - 1] == '\'') || (value[0] == '"' && value[value.Length - 1] == '"')); }
        private static string Unquote(string value) { return value.Substring(1, value.Length - 2); }
        private static string DisplayValue(object value, PacketVariableDataType type) { return type == PacketVariableDataType.Bytes ? BitConverter.ToString((byte[])value).Replace("-", " ") : Convert.ToString(value, CultureInfo.InvariantCulture); }

        public sealed class PacketVariableDisplay
        {
            public Guid ExtractorId;
            public Guid VariableId;
            public string Display;
        }
        private static bool SameValue(object left, object right) { if (left is byte[] && right is byte[]) return ((byte[])left).SequenceEqual((byte[])right); return Equals(left, right); }

        /// <summary>受限的数值表达式解析器：只接受数值、+ - * / %、括号与一元正负号，不执行任何脚本。</summary>
        private abstract class NumericExpression<T>
        {
            protected readonly string Source;
            protected int Position;
            protected NumericExpression(string source) { Source = source ?? string.Empty; }
            protected abstract bool TryNumber(out T value);
            protected abstract T Add(T left, T right);
            protected abstract T Subtract(T left, T right);
            protected abstract T Multiply(T left, T right);
            protected abstract bool Divide(T left, T right, out T value);
            protected abstract bool Modulo(T left, T right, out T value);
            protected abstract T Negate(T value);

            protected bool Evaluate(out T value)
            {
                value = default(T); if (!ParseSum(out value)) return false; SkipSpace(); return Position == Source.Length;
            }
            private bool ParseSum(out T value)
            {
                if (!ParseProduct(out value)) return false;
                while (true)
                {
                    SkipSpace(); if (Take('+')) { T right; if (!ParseProduct(out right)) return false; value = Add(value, right); }
                    else if (Take('-')) { T right; if (!ParseProduct(out right)) return false; value = Subtract(value, right); }
                    else return true;
                }
            }
            private bool ParseProduct(out T value)
            {
                if (!ParseUnary(out value)) return false;
                while (true)
                {
                    SkipSpace(); char op = Position < Source.Length ? Source[Position] : '\0';
                    if (op != '*' && op != '/' && op != '%') return true;
                    Position++; T right; if (!ParseUnary(out right)) return false;
                    if (op == '*') value = Multiply(value, right);
                    else if (op == '/' && !Divide(value, right, out value)) return false;
                    else if (op == '%' && !Modulo(value, right, out value)) return false;
                }
            }
            private bool ParseUnary(out T value)
            {
                SkipSpace(); if (Take('+')) return ParseUnary(out value);
                if (Take('-')) { if (!ParseUnary(out value)) return false; value = Negate(value); return true; }
                if (Take('(')) { if (!ParseSum(out value)) return false; SkipSpace(); return Take(')'); }
                return TryNumber(out value);
            }
            protected bool Take(char c) { if (Position < Source.Length && Source[Position] == c) { Position++; return true; } return false; }
            protected void SkipSpace() { while (Position < Source.Length && char.IsWhiteSpace(Source[Position])) Position++; }
        }

        private sealed class IntegerExpression : NumericExpression<long>
        {
            private IntegerExpression(string source) : base(source) { }
            public static bool TryEvaluate(string source, out long value) { try { return new IntegerExpression(source).Evaluate(out value); } catch { value = 0; return false; } }
            protected override bool TryNumber(out long value) { value = 0; SkipSpace(); int start = Position; while (Position < Source.Length && char.IsDigit(Source[Position])) Position++; return start < Position && long.TryParse(Source.Substring(start, Position - start), NumberStyles.None, CultureInfo.InvariantCulture, out value); }
            protected override long Add(long left, long right) { return checked(left + right); }
            protected override long Subtract(long left, long right) { return checked(left - right); }
            protected override long Multiply(long left, long right) { return checked(left * right); }
            protected override bool Divide(long left, long right, out long value) { value = 0; if (right == 0) return false; value = left / right; return true; }
            protected override bool Modulo(long left, long right, out long value) { value = 0; if (right == 0) return false; value = left % right; return true; }
            protected override long Negate(long value) { return checked(-value); }
        }

        private sealed class DoubleExpression : NumericExpression<double>
        {
            private DoubleExpression(string source) : base(source) { }
            public static bool TryEvaluate(string source, out double value) { try { return new DoubleExpression(source).Evaluate(out value); } catch { value = 0; return false; } }
            protected override bool TryNumber(out double value)
            {
                value = 0; SkipSpace(); int start = Position;
                while (Position < Source.Length && (char.IsDigit(Source[Position]) || Source[Position] == '.')) Position++;
                if (Position < Source.Length && (Source[Position] == 'e' || Source[Position] == 'E'))
                {
                    Position++; if (Position < Source.Length && (Source[Position] == '+' || Source[Position] == '-')) Position++;
                    while (Position < Source.Length && char.IsDigit(Source[Position])) Position++;
                }
                return start < Position && double.TryParse(Source.Substring(start, Position - start), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
            }
            protected override double Add(double left, double right) { return left + right; }
            protected override double Subtract(double left, double right) { return left - right; }
            protected override double Multiply(double left, double right) { return left * right; }
            protected override bool Divide(double left, double right, out double value) { value = 0; if (right == 0) return false; value = left / right; return true; }
            protected override bool Modulo(double left, double right, out double value) { value = 0; if (right == 0) return false; value = left % right; return true; }
            protected override double Negate(double value) { return -value; }
        }

        private static object Parse(byte[] raw, PacketVariableDataType type, PacketExtractionSpec spec)
        {
            if (type == PacketVariableDataType.Bytes) return raw;
            if (type == PacketVariableDataType.String) return GetEncoding(spec.Encoding).GetString(raw);
            byte[] number = (byte[])raw.Clone();
            if (spec.BigEndian == BitConverter.IsLittleEndian) Array.Reverse(number);
            if (type == PacketVariableDataType.Double)
            {
                if (number.Length == 4) return (double)BitConverter.ToSingle(number, 0);
                if (number.Length == 8) return BitConverter.ToDouble(number, 0);
                throw new InvalidOperationException("浮点数长度必须为 4 或 8");
            }
            if (number.Length > 8) throw new InvalidOperationException("整数长度不能超过 8");
            ulong u = 0; for (int i = number.Length - 1; i >= 0; i--) u = (u << 8) | number[i];
            if (spec.Signed && number.Length < 8 && (number[number.Length - 1] & 0x80) != 0) u |= ulong.MaxValue << (number.Length * 8);
            return unchecked((long)u);
        }

        private static bool ToBytes(object value, PacketVariableDataType type, string format, PacketExtractionSpec spec, out byte[] result, out string error)
        {
            error = null; result = null;
            if (type == PacketVariableDataType.Bytes && string.IsNullOrEmpty(format)) { result = (byte[])value; return true; }
            string f = (format ?? string.Empty).ToLowerInvariant();
            if (type == PacketVariableDataType.Double)
            {
                double number;
                if (!double.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Float, CultureInfo.InvariantCulture, out number)) { error = "变量不是浮点数"; return false; }
                if (f == "f32le" || f == "f32be") result = BitConverter.GetBytes((float)number);
                else if (f == "f64le" || f == "f64be") result = BitConverter.GetBytes(number);
                else { error = "浮点变量需指定格式：f32le、f32be、f64le 或 f64be"; return false; }
                if (f.EndsWith("be", StringComparison.Ordinal)) Array.Reverse(result);
                return true;
            }
            if (type == PacketVariableDataType.String && (f == "" || f == "utf8")) { result = Encoding.UTF8.GetBytes(Convert.ToString(value)); return true; }
            if (type == PacketVariableDataType.String && f == "gb18030") { result = GetEncoding(PacketVariableEncoding.Gb18030).GetBytes(Convert.ToString(value)); return true; }
            if (type == PacketVariableDataType.String && f == "big5") { result = GetEncoding(PacketVariableEncoding.Big5).GetBytes(Convert.ToString(value)); return true; }
            if (f == "hex") { return AppendHex(Convert.ToString(value), new List<byte>(), out error, out result); }
            long n; if (!long.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out n)) { error = "变量不是整数"; return false; }
            int width = f == "u8" ? 1 : f == "u16le" || f == "u16be" ? 2 : f == "u32le" || f == "u32be" ? 4 : 8;
            result = new byte[width]; for (int i = 0; i < width; i++) result[i] = (byte)((ulong)n >> (8 * i));
            if (f.EndsWith("be", StringComparison.Ordinal)) Array.Reverse(result);
            return true;
        }

        private static bool AppendHex(string text, List<byte> target, out string error)
        {
            byte[] ignored; return AppendHex(text, target, out error, out ignored);
        }
        private static bool AppendHex(string text, List<byte> target, out string error, out byte[] result)
        {
            error = null; result = null;
            string clean = Regex.Replace(text ?? string.Empty, @"\s+", string.Empty);
            if (clean.Length == 0) { result = target.ToArray(); return true; }
            if ((clean.Length & 1) != 0 || !Regex.IsMatch(clean, @"\A[0-9a-fA-F]+\z")) { error = "动态模板中的固定字节必须是 Hex"; return false; }
            for (int i = 0; i < clean.Length; i += 2) target.Add(byte.Parse(clean.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture));
            result = target.ToArray(); return true;
        }
        private static bool IsExpired(PacketVariableInfo v, RuntimeValue value) { return v.TtlSeconds > 0 && DateTime.UtcNow - value.UpdatedUtc > TimeSpan.FromSeconds(v.TtlSeconds); }
        private static Encoding GetEncoding(PacketVariableEncoding e) { return e == PacketVariableEncoding.Gb18030 ? Encoding.GetEncoding(54936) : e == PacketVariableEncoding.Big5 ? Encoding.GetEncoding(950) : Encoding.UTF8; }
        private static string ScopeKey(PacketExtractorScope scope, long key) { return scope == PacketExtractorScope.Global ? "global" : key.ToString(CultureInfo.InvariantCulture); }
        private static string Key(Guid extractor, Guid variable, PacketExtractorScope scope, long key) { return extractor.ToString("N") + ":" + ScopeKey(scope, key) + ":" + variable.ToString("N"); }
    }
}
