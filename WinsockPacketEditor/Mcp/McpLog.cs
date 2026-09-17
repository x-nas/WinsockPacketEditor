using System;
using System.Text.RegularExpressions;

namespace WinsockPacketEditor.Mcp
{
    /// <summary>Dedicated, redacted operational log for the local MCP boundary.</summary>
    internal static class McpLog
    {
        internal static string ToolName(string operation)
        {
            if (string.IsNullOrWhiteSpace(operation)) return "wpe_unknown";
            if (string.Equals(operation, "runtime.status", StringComparison.Ordinal)) return "wpe_status_get";
            if (string.Equals(operation, "filters.stats.get", StringComparison.Ordinal)) return "wpe_filter_stats_get";
            return "wpe_" + Regex.Replace(operation, "([a-z])([A-Z])", "$1_$2").Replace('.', '_').ToLowerInvariant();
        }

        internal static void Write(string category, string content)
        {
            try
            {
                Operate.LogConfig.Queue.McpLogToQueue(category ?? "MCP", content ?? string.Empty);
            }
            catch
            {
                // Logging must never make an MCP request fail.
            }
        }

        internal static void Error(string category, Exception exception)
        {
            Write(category, exception == null ? "调用失败：未知错误。" : "调用失败：" + exception.GetType().Name + "，" + exception.Message);
        }

    }
}
