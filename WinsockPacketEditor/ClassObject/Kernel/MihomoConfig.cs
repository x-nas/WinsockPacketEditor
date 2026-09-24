using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 生成 mihomo 的 config.yaml。
    ///
    /// 【断环是硬要求】规则块最前面固定把 WPE 自身 / 启动器 / 内核进程判为 DIRECT ——
    /// WPE 的 SOCKS5 服务器出站若被 TUN 再抓回内核、又被送回 WPE，会成死环（表现是直接断网）。
    /// 这一段不可由界面关闭，见 AGENTS.md 与 POC 记录。
    /// </summary>
    internal static class MihomoConfig
    {
        /// <summary>内嵌模板的逻辑名，见 csproj 的 EmbeddedResource LogicalName。</summary>
        public const string TemplateName = "base-mihomo.yaml";

        /// <summary>必须直连的自身进程（断环）。WPE64.exe 是启动器，wpe-mihomo.exe 是内核自身。</summary>
        private static readonly string[] SelfDirectNames =
        {
            "WinsockPacketEditor.exe",
            "WPE64.exe",
            "wpe-mihomo.exe",
        };

        /// <summary>读内嵌模板。失败返回 null 并给出 error。</summary>
        public static string ReadTemplate(out string error)
        {
            error = string.Empty;
            try
            {
                using (Stream s = typeof(MihomoConfig).Assembly.GetManifestResourceStream(TemplateName))
                {
                    if (s == null)
                    {
                        error = "找不到内嵌配置模板 " + TemplateName;
                        return null;
                    }
                    using (StreamReader r = new StreamReader(s, Encoding.UTF8))
                    {
                        return r.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 按当前设置生成整份 config.yaml 文本。
        /// processNames 为空 = 不拦截任何进程，只留断环规则 + MATCH,DIRECT。
        /// 返回 null 表示失败，error 里是要显示的文案。
        /// </summary>
        public static string Build(
            string template,
            string server,
            int socks5Port,
            string userName,
            string passWord,
            string tunStack,
            string dnsMode,
            int ctlPort,
            string secret,
            IEnumerable<string> processNames,
            out string error)
        {
            error = string.Empty;
            if (string.IsNullOrEmpty(template)) { error = "配置模板为空"; return null; }
            if (string.IsNullOrEmpty(server)) { error = "SOCKS5 服务器地址为空"; return null; }
            if (socks5Port < 1 || socks5Port > 65535) { error = "SOCKS5 端口不合法"; return null; }

            // ---- rules ----
            StringBuilder rules = new StringBuilder();

            foreach (string n in SelfDirectNames)
            {
                rules.Append("  - PROCESS-NAME,").Append(n).Append(",DIRECT\n");
            }

            if (processNames != null)
            {
                HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (string raw in processNames)
                {
                    if (string.IsNullOrWhiteSpace(raw)) { continue; }
                    string name = raw.Trim();
                    if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) == false) { name += ".exe"; }

                    // mihomo 一条规则写错整份配置加载失败：名字里带逗号 / 空格的一律跳过
                    if (name.IndexOf(',') >= 0 || name.IndexOf(' ') >= 0 || name.IndexOf('\t') >= 0)
                    {
                        Operate.DoLog(nameof(MihomoConfig), "跳过非法进程名规则：" + name);
                        continue;
                    }
                    if (!seen.Add(name)) { continue; }

                    rules.Append("  - PROCESS-NAME,").Append(name).Append(",WPE\n");
                }
            }

            rules.Append("  - MATCH,DIRECT\n");

            // ---- socks5 认证行（没开认证就不写，免得 mihomo 走带认证的握手）----
            string authLines = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                authLines = "    username: \"" + YamlQuote(userName) + "\"\n"
                          + "    password: \"" + YamlQuote(passWord) + "\"\n";
            }

            return template
                .Replace("{server}", server)
                .Replace("{socks5Port}", socks5Port.ToString(CultureInfo.InvariantCulture))
                .Replace("{ctlPort}", ctlPort.ToString(CultureInfo.InvariantCulture))
                .Replace("{tunStack}", tunStack)
                .Replace("{dnsMode}", dnsMode)
                .Replace("{secret}", secret)
                .Replace("{authLines}", authLines)
                .Replace("{rules}", rules.ToString());
        }

        /// <summary>YAML 双引号串里的转义：反斜杠与双引号。</summary>
        private static string YamlQuote(string s)
        {
            if (string.IsNullOrEmpty(s)) { return string.Empty; }
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
