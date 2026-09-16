using System.ComponentModel;
using ModelContextProtocol.Server;

[McpServerToolType]
internal static class WpeTools
{
    [McpServerTool(Name = "wpe_status_get"), Description("Return the current WPE runtime status. This tool never returns credentials or tokens.")]
    public static Task<string> StatusGet(WpeGatewayClient gateway, CancellationToken cancellationToken) =>
        gateway.InvokeAsync("runtime.status", null, cancellationToken);

    [McpServerTool(Name = "wpe_capture_search"), Description("Search bounded WPE packet metadata. Payload bytes are never returned by this tool.")]
    public static Task<string> CaptureSearch(WpeGatewayClient gateway, int? limit = null, string? cursor = null, string? mode = null, string? pattern = null, bool hex = false, string direction = "any", CancellationToken cancellationToken = default) =>
        gateway.InvokeAsync("capture.search", new PacketSearchInput(limit, cursor, mode, pattern, hex, direction), cancellationToken);

    [McpServerTool(Name = "wpe_packet_get"), Description("Get one captured packet. Payload delivery is opt-in and bounded by WPE.")]
    public static Task<string> PacketGet(WpeGatewayClient gateway, long id, string? mode = null, bool includePayload = false, CancellationToken cancellationToken = default) =>
        gateway.InvokeAsync("capture.get", new PacketGetInput(id, mode, includePayload), cancellationToken);

    [McpServerTool(Name = "wpe_logs_list"), Description("List a bounded page of WPE system, filter, or proxy logs.")]
    public static Task<string> LogsList(WpeGatewayClient gateway, string kind, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) =>
        gateway.InvokeAsync("logs.list", new LogListInput(kind, limit, cursor), cancellationToken);

    [McpServerTool(Name = "wpe_filters_list"), Description("List WPE filters without modifying them.")]
    public static Task<string> FiltersList(WpeGatewayClient gateway, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.list", new PageInput(limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_filter_set_enabled"), Description("Request a reversible filter enable/disable change. WPE must show and receive a local confirmation before it applies.")]
    public static Task<string> FilterSetEnabled(WpeGatewayClient gateway, string id, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.setEnabled", new FilterSetEnabledInput(id, enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_account_set_enabled"), Description("Request a reversible proxy-account enable/disable change. Passwords are never returned or entered.")]
    public static Task<string> AccountSetEnabled(WpeGatewayClient gateway, string id, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("accounts.setEnabled", new AccountSetEnabledInput(id, enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_auth_set_enabled"), Description("Request a reversible proxy authentication setting change. WPE validates Only-WPC compatibility and persists the setting after local confirmation.")]
    public static Task<string> ProxyAuthSetEnabled(WpeGatewayClient gateway, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.auth.setEnabled", new ProxyAuthSetEnabledInput(enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_http_set_enabled"), Description("Request a reversible HTTP proxy enable/disable change. WPE validates port compatibility and persists the setting after local confirmation.")]
    public static Task<string> ProxyHttpSetEnabled(WpeGatewayClient gateway, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.http.setEnabled", new ProxyHttpSetEnabledInput(enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_max_connections_set"), Description("Request a reversible proxy maximum-connection limit change. WPE validates the current machine cap and persists the setting after local confirmation.")]
    public static Task<string> ProxyMaxConnectionsSet(WpeGatewayClient gateway, int maxConnection, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.maxConnections.set", new ProxyMaxConnectionsSetInput(maxConnection, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_socks5_port_set"), Description("Request a reversible SOCKS5 listening-port change. WPE validates range and HTTP-port conflicts, then persists after local confirmation.")]
    public static Task<string> ProxySocks5PortSet(WpeGatewayClient gateway, int port, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.socks5Port.set", new ProxySocks5PortSetInput(port, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_http_port_set"), Description("Request a reversible HTTP listening-port change. WPE validates that HTTP is enabled, checks range and SOCKS5 conflicts, then persists after local confirmation.")]
    public static Task<string> ProxyHttpPortSet(WpeGatewayClient gateway, int port, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.httpPort.set", new ProxyHttpPortSetInput(port, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_firewall_set_enabled"), Description("Request a reversible firewall enable/disable change. WPE applies and persists it only after local confirmation.")]
    public static Task<string> FirewallSetEnabled(WpeGatewayClient gateway, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("firewall.setEnabled", new FirewallSetEnabledInput(enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_only_wpc_set_enabled"), Description("Request a reversible Only-WPC proxy setting change. WPE requires authentication when enabling and persists after local confirmation.")]
    public static Task<string> ProxyOnlyWpcSetEnabled(WpeGatewayClient gateway, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.onlyWpc.setEnabled", new ProxyOnlyWpcSetEnabledInput(enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_executors_list"), Description("List WPE send and robot executor status without changing it.")]
    public static Task<string> ExecutorsList(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("executors.list", null, cancellationToken);
    [McpServerTool(Name = "wpe_connections_list"), Description("List a bounded page of WPE connections without credentials.")]
    public static Task<string> ConnectionsList(WpeGatewayClient gateway, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("connections.list", new PageInput(limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_accounts_list"), Description("List proxy account metadata only; passwords and tokens are excluded.")]
    public static Task<string> AccountsList(WpeGatewayClient gateway, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("accounts.list", new PageInput(limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_firewall_get"), Description("Return WPE firewall configuration without modifying it.")]
    public static Task<string> FirewallGet(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("firewall.get", null, cancellationToken);
    [McpServerTool(Name = "wpe_proxy_settings_get"), Description("Return non-sensitive proxy settings and limits. No credentials are returned and nothing is modified.")]
    public static Task<string> ProxySettingsGet(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("proxy.settings.get", null, cancellationToken);
    [McpServerTool(Name = "wpe_proxy_runtime_get"), Description("Return non-sensitive live proxy runtime diagnostics: listener configuration, running state, and connection counts. Nothing is modified.")]
    public static Task<string> ProxyRuntimeGet(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("proxy.runtime.get", null, cancellationToken);
    [McpServerTool(Name = "wpe_connections_summary_get"), Description("Return bounded connection counts grouped by protocol and WPC-control versus ordinary sessions. Addresses, device identifiers, credentials, and payloads are never returned.")]
    public static Task<string> ConnectionsSummaryGet(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("connections.summary.get", null, cancellationToken);
    [McpServerTool(Name = "wpe_proxy_bind_ip_set"), Description("Request a reversible proxy listening-address change. WPE validates an explicit IPv4/IPv6 address or automatic detection, then persists after local confirmation.")]
    public static Task<string> ProxyBindIpSet(WpeGatewayClient gateway, bool auto, string ip, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.bindIp.set", new ProxyBindIpSetInput(auto, ip, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_external_proxy_set_enabled"), Description("Request a reversible external-proxy enable/disable change. Existing credentials are never returned or modified; WPE validates the configured endpoint and persists after local confirmation.")]
    public static Task<string> ExternalProxySetEnabled(WpeGatewayClient gateway, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.external.setEnabled", new ExternalProxySetEnabledInput(enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_start"), Description("Request starting WPE proxy listeners. This may bind configured ports and requires local confirmation.")]
    public static Task<string> ProxyStart(WpeGatewayClient gateway, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.start", new ProxyLifecycleInput(idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_stop"), Description("Request stopping WPE proxy listeners. Existing proxy connections may be disconnected; local confirmation is required.")]
    public static Task<string> ProxyStop(WpeGatewayClient gateway, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.stop", new ProxyLifecycleInput(idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_firewall_rules_list"), Description("List a bounded page of WPE firewall white-list or black-list rules.")]
    public static Task<string> FirewallRulesList(WpeGatewayClient gateway, string list, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("firewall.rules.list", new FirewallRulesListInput(list, limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_firewall_rule_add"), Description("Request addition of one WPE firewall rule. WPE validates it and must receive a local confirmation before it applies and persists the rule.")]
    public static Task<string> FirewallRuleAdd(WpeGatewayClient gateway, string list, string address, string idempotencyKey, bool expiryEnabled = false, string? expiryTime = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("firewall.rule.add", new FirewallRuleAddInput(list, address, idempotencyKey, expiryEnabled, expiryTime), cancellationToken);
    [McpServerTool(Name = "wpe_firewall_rule_remove"), Description("Request removal of one exact WPE firewall rule. WPE must show and receive a local confirmation before it applies.")]
    public static Task<string> FirewallRuleRemove(WpeGatewayClient gateway, string list, string address, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("firewall.rule.remove", new FirewallRuleRemoveInput(list, address, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_bytes_transcode"), Description("Convert caller-supplied bytes using WPE's encoding rules.")]
    public static Task<string> BytesTranscode(WpeGatewayClient gateway, string text, bool decode, CancellationToken cancellationToken = default) => gateway.InvokeAsync("bytes.transcode", new BytesTranscodeInput(text, decode), cancellationToken);
    [McpServerTool(Name = "wpe_bytes_compare"), Description("Compare two caller-supplied byte strings using WPE logic.")]
    public static Task<string> BytesCompare(WpeGatewayClient gateway, string left, string right, int minimumRun = 2, CancellationToken cancellationToken = default) => gateway.InvokeAsync("bytes.compare", new BytesCompareInput(left, right, minimumRun), cancellationToken);
    [McpServerTool(Name = "wpe_bytes_extract"), Description("Extract structured values from caller-supplied Base64 bytes using WPE logic.")]
    public static Task<string> BytesExtract(WpeGatewayClient gateway, int kind, string contentBase64, CancellationToken cancellationToken = default) => gateway.InvokeAsync("bytes.extract", new BytesExtractInput(kind, contentBase64), cancellationToken);
}

public sealed record PageInput(int? Limit = null, string? Cursor = null);
public sealed record FilterSetEnabledInput(string Id, bool Enabled, string IdempotencyKey);
public sealed record AccountSetEnabledInput(string Id, bool Enabled, string IdempotencyKey);
public sealed record ProxyAuthSetEnabledInput(bool Enabled, string IdempotencyKey);
public sealed record ProxyHttpSetEnabledInput(bool Enabled, string IdempotencyKey);
public sealed record ProxyMaxConnectionsSetInput(int MaxConnection, string IdempotencyKey);
public sealed record ProxySocks5PortSetInput(int Port, string IdempotencyKey);
public sealed record ProxyHttpPortSetInput(int Port, string IdempotencyKey);
public sealed record FirewallSetEnabledInput(bool Enabled, string IdempotencyKey);
public sealed record ProxyOnlyWpcSetEnabledInput(bool Enabled, string IdempotencyKey);
public sealed record ProxyBindIpSetInput(bool Auto, string Ip, string IdempotencyKey);
public sealed record ExternalProxySetEnabledInput(bool Enabled, string IdempotencyKey);
public sealed record ProxyLifecycleInput(string IdempotencyKey);
public sealed record FirewallRulesListInput(string List, int? Limit = null, string? Cursor = null);
public sealed record FirewallRuleAddInput(string List, string Address, string IdempotencyKey, bool ExpiryEnabled = false, string? ExpiryTime = null);
public sealed record FirewallRuleRemoveInput(string List, string Address, string IdempotencyKey);
public sealed record PacketSearchInput(int? Limit = null, string? Cursor = null, string? Mode = null, string? Pattern = null, bool Hex = false, string Direction = "any");
public sealed record PacketGetInput(long Id, string? Mode = null, bool IncludePayload = false);
public sealed record LogListInput(string Kind, int? Limit = null, string? Cursor = null);
public sealed record BytesTranscodeInput(string Text, bool Decode);
public sealed record BytesCompareInput(string Left, string Right, int MinimumRun = 2);
public sealed record BytesExtractInput(int Kind, string ContentBase64);
