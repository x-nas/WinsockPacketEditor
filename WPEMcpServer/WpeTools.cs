using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

[McpServerToolType]
internal static class WpeTools
{
    [McpServerTool(Name = "wpe_status_get"), Description("Return the current WPE runtime status.")]
    public static Task<string> StatusGet(WpeGatewayClient gateway, CancellationToken cancellationToken) =>
        gateway.InvokeAsync("runtime.status", null, cancellationToken);

    [McpServerTool(Name = "wpe_capture_search"), Description("Search a bounded page of WPE packet metadata. Use wpe_packet_get for complete packet bytes.")]
    public static Task<string> CaptureSearch(WpeGatewayClient gateway, int? limit = null, string? cursor = null, string? mode = null, string? pattern = null, bool hex = false, string direction = "any", CancellationToken cancellationToken = default) =>
        gateway.InvokeAsync("capture.search", new PacketSearchInput(limit, cursor, mode, pattern, hex, direction), cancellationToken);

    [McpServerTool(Name = "wpe_packet_get"), Description("Get one captured packet with its complete current and original payload bytes as Base64.")]
    public static Task<string> PacketGet(WpeGatewayClient gateway, long id, string? mode = null, bool includePayload = true, CancellationToken cancellationToken = default) =>
        gateway.InvokeAsync("capture.get", new PacketGetInput(id, mode, includePayload), cancellationToken);
    [McpServerTool(Name = "wpe_capture_find_next"), Description("Use WPE's native regex packet search to find the next match in the proxy or inject capture list, including byte offset metadata for highlighting.")]
    public static Task<string> CaptureFindNext(WpeGatewayClient gateway, string pattern, bool hex = false, string mode = "proxy", int fromIndex = 0, int fromPosition = 0, CancellationToken cancellationToken = default) =>
        gateway.InvokeAsync("capture.findNext", new CaptureFindNextInput(pattern, hex, mode, fromIndex, fromPosition), cancellationToken);

    [McpServerTool(Name = "wpe_logs_list"), Description("List a bounded page of WPE system, filter, proxy, or MCP operational logs.")]
    public static Task<string> LogsList(WpeGatewayClient gateway, string kind, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) =>
        gateway.InvokeAsync("logs.list", new LogListInput(kind, limit, cursor), cancellationToken);

    [McpServerTool(Name = "wpe_filters_list"), Description("List WPE filters without modifying them.")]
    public static Task<string> FiltersList(WpeGatewayClient gateway, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.list", new PageInput(limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_filter_get"), Description("Get one existing WPE filter's editable configuration, including its current enabled state. Use this to verify mode and enablement after changes.")]
    public static Task<string> FilterGet(WpeGatewayClient gateway, string id, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.get", new FilterGetInput(id), cancellationToken);
    [McpServerTool(Name = "wpe_filter_stats_get"), Description("Get read-only runtime statistics for one existing WPE filter.")]
    public static Task<string> FilterStatsGet(WpeGatewayClient gateway, string id, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.stats.get", new FilterGetInput(id), cancellationToken);
    [McpServerTool(Name = "wpe_filter_set_enabled"), Description("Explicitly enable or disable one filter after local confirmation. This is the only MCP operation that changes a filter's enabled state; call it after wpe_filter_update when the user asks to enable a filter.")]
    public static Task<string> FilterSetEnabled(WpeGatewayClient gateway, string id, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.setEnabled", new FilterSetEnabledInput(id, enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_filters_set_all_enabled"), Description("Enable or disable all existing WPE filters after local confirmation.")]
    public static Task<string> FiltersSetAllEnabled(WpeGatewayClient gateway, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.setAllEnabled", new FiltersSetAllEnabledInput(enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_filter_counts_reset"), Description("Reset existing WPE filters' runtime execution counts after local confirmation. This does not alter filter rules.")]
    public static Task<string> FilterCountsReset(WpeGatewayClient gateway, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.counts.reset", new FilterCountsResetInput(idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_filters_move"), Description("Move selected existing filters to top, up, down, or bottom using WPE's native list-order behavior. Filter order affects execution order.")]
    public static Task<string> FiltersMove(WpeGatewayClient gateway, string[] ids, string direction, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.move", new FiltersMoveInput(ids, direction, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_filters_copy"), Description("Copy selected existing WPE filters using the native list action after local confirmation.")]
    public static Task<string> FiltersCopy(WpeGatewayClient gateway, string[] ids, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.copy", new FiltersCopyInput(ids, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_filters_clear_all"), Description("Clear all existing WPE filters after local confirmation; WPE also shows its native destructive confirmation.")]
    public static Task<string> FiltersClearAll(WpeGatewayClient gateway, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.clearAll", new FilterCountsResetInput(idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_filter_create_from_capture"), Description("Create a filter from one existing captured WPE packet after local confirmation, matching WPE's native packet-to-filter action.")]
    public static Task<string> FilterCreateFromCapture(WpeGatewayClient gateway, long packetId, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.createFromCapture", new FilterCreateFromCaptureInput(packetId, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_filter_create"), Description("Create a new empty WPE filter after local confirmation.")]
    public static Task<string> FilterCreate(WpeGatewayClient gateway, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.create", new FilterCreateInput(idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_filter_update"), Description("Replace an existing filter's rule configuration, such as normal/advanced mode and match/modify rules, after local confirmation. It never changes enabled state; call wpe_filter_set_enabled separately when needed.")]
    public static Task<string> FilterUpdate(WpeGatewayClient gateway, JsonElement filter, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.update", new FilterUpdateInput(filter, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_filter_delete"), Description("Delete an existing WPE filter after local confirmation.")]
    public static Task<string> FilterDelete(WpeGatewayClient gateway, string id, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("filters.delete", new FilterDeleteInput(id, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_account_set_enabled"), Description("Request a reversible proxy-account enable/disable change.")]
    public static Task<string> AccountSetEnabled(WpeGatewayClient gateway, string id, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("accounts.setEnabled", new AccountSetEnabledInput(id, enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_account_create"), Description("Create one WPE proxy account after local confirmation.")]
    public static Task<string> AccountCreate(WpeGatewayClient gateway, string userName, string password, bool enabled, bool limitLinksEnabled, int limitLinks, bool limitDevicesEnabled, int limitDevices, bool expiryEnabled, string? expiryTime, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("accounts.create", new AccountCreateInput(userName, password, enabled, limitLinksEnabled, limitLinks, limitDevicesEnabled, limitDevices, expiryEnabled, expiryTime, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_account_update"), Description("Update one existing WPE proxy account after local confirmation. userName cannot be changed because WPE's account editor does not support it; omit password to keep it unchanged.")]
    public static Task<string> AccountUpdate(WpeGatewayClient gateway, string id, bool enabled, bool limitLinksEnabled, int limitLinks, bool limitDevicesEnabled, int limitDevices, bool expiryEnabled, string? expiryTime, string? password, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("accounts.update", new AccountUpdateInput(id, enabled, limitLinksEnabled, limitLinks, limitDevicesEnabled, limitDevices, expiryEnabled, expiryTime, password, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_account_delete"), Description("Delete one existing WPE proxy account after local confirmation.")]
    public static Task<string> AccountDelete(WpeGatewayClient gateway, string id, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("accounts.delete", new AccountDeleteInput(id, idempotencyKey), cancellationToken);
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
    [McpServerTool(Name = "wpe_connections_list"), Description("List a bounded page of WPE connections.")]
    public static Task<string> ConnectionsList(WpeGatewayClient gateway, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("connections.list", new PageInput(limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_accounts_list"), Description("List proxy accounts including their decrypted passwords.")]
    public static Task<string> AccountsList(WpeGatewayClient gateway, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("accounts.list", new PageInput(limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_account_get"), Description("Get one proxy account's complete configuration, decrypted password, and login records.")]
    public static Task<string> AccountGet(WpeGatewayClient gateway, string id, CancellationToken cancellationToken = default) => gateway.InvokeAsync("accounts.get", new AccountGetInput(id), cancellationToken);
    [McpServerTool(Name = "wpe_account_logins_list"), Description("List the selected WPE proxy account's existing native login-location records. This is not a device inventory.")]
    public static Task<string> AccountLoginsList(WpeGatewayClient gateway, string id, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("accounts.logins.list", new AccountLoginsListInput(id, limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_firewall_get"), Description("Return WPE firewall configuration without modifying it.")]
    public static Task<string> FirewallGet(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("firewall.get", null, cancellationToken);
    [McpServerTool(Name = "wpe_proxy_settings_get"), Description("Return proxy settings, limits, and configured external-proxy credentials without modifying WPE.")]
    public static Task<string> ProxySettingsGet(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("proxy.settings.get", null, cancellationToken);
    [McpServerTool(Name = "wpe_proxy_config_get"), Description("Return the complete proxy configuration snapshot, including configured external-proxy credentials.")]
    public static Task<string> ProxyConfigGet(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("proxy.config.get", null, cancellationToken);
    [McpServerTool(Name = "wpe_proxy_runtime_get"), Description("Return live proxy runtime diagnostics: listener configuration, running state, and connection counts. Nothing is modified.")]
    public static Task<string> ProxyRuntimeGet(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("proxy.runtime.get", null, cancellationToken);
    [McpServerTool(Name = "wpe_connections_summary_get"), Description("Return complete connection counts grouped by protocol and WPC-control versus ordinary sessions.")]
    public static Task<string> ConnectionsSummaryGet(WpeGatewayClient gateway, CancellationToken cancellationToken) => gateway.InvokeAsync("connections.summary.get", null, cancellationToken);
    [McpServerTool(Name = "wpe_proxy_bind_ip_set"), Description("Request a reversible proxy listening-address change. WPE validates an explicit IPv4/IPv6 address or automatic detection, then persists after local confirmation.")]
    public static Task<string> ProxyBindIpSet(WpeGatewayClient gateway, bool auto, string ip, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.bindIp.set", new ProxyBindIpSetInput(auto, ip, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_external_proxy_set_enabled"), Description("Request a reversible external-proxy enable/disable change. WPE validates the configured endpoint and persists after local confirmation.")]
    public static Task<string> ExternalProxySetEnabled(WpeGatewayClient gateway, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.external.setEnabled", new ExternalProxySetEnabledInput(enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_start"), Description("Request starting WPE proxy listeners. This may bind configured ports and requires local confirmation.")]
    public static Task<string> ProxyStart(WpeGatewayClient gateway, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.start", new ProxyLifecycleInput(idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_stop"), Description("Request stopping WPE proxy listeners. Existing proxy connections may be disconnected; local confirmation is required.")]
    public static Task<string> ProxyStop(WpeGatewayClient gateway, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxy.stop", new ProxyLifecycleInput(idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_executors_stop_all"), Description("Request stopping all active WPE sender and robot executors. This never starts tasks or sends packets; it follows the global MCP confirmation setting.")]
    public static Task<string> ExecutorsStopAll(WpeGatewayClient gateway, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("executors.stopAll", new ExecutorStopAllInput(idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_start_mode_select"), Description("Select WPE proxy or inject mode only while WPE is on its start page; success opens that mode's page. This never starts proxy listeners or injects. If the requested mode is already selected, it returns an unchanged successful state; a different selected mode returns a structured unavailable state.")]
    public static Task<string> StartModeSelect(WpeGatewayClient gateway, string mode, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("start.mode.select", new StartModeSelectInput(mode, idempotencyKey), cancellationToken);
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
    [McpServerTool(Name = "wpe_sends_list"), Description("List a bounded page of send-task metadata.")]
    public static Task<string> SendsList(WpeGatewayClient gateway, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("sends.list", new PageInput(limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_send_get"), Description("Get one send task's editable configuration.")]
    public static Task<string> SendGet(WpeGatewayClient gateway, string id, CancellationToken cancellationToken = default) => gateway.InvokeAsync("sends.get", new EntityGetInput(id), cancellationToken);
    [McpServerTool(Name = "wpe_send_collection_list"), Description("List a bounded page of one send task's packet collection with its native preview fields.")]
    public static Task<string> SendCollectionList(WpeGatewayClient gateway, string id, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("sends.collection.list", new EntityPageInput(id, limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_robots_list"), Description("List a bounded page of robot-task metadata.")]
    public static Task<string> RobotsList(WpeGatewayClient gateway, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("robots.list", new PageInput(limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_robot_get"), Description("Get one robot task and its native instruction configuration.")]
    public static Task<string> RobotGet(WpeGatewayClient gateway, string id, CancellationToken cancellationToken = default) => gateway.InvokeAsync("robots.get", new EntityGetInput(id), cancellationToken);
    [McpServerTool(Name = "wpe_warehouse_list"), Description("List a bounded page of warehouse metadata.")]
    public static Task<string> WarehousesList(WpeGatewayClient gateway, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("warehouses.list", new PageInput(limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_warehouse_get"), Description("Get one warehouse's name and a bounded page of its stored-packet records.")]
    public static Task<string> WarehouseGet(WpeGatewayClient gateway, string id, int? limit = null, string? cursor = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("warehouses.get", new EntityPageInput(id, limit, cursor), cancellationToken);
    [McpServerTool(Name = "wpe_send_collection_action"), Description("Move, copy, or delete selected packets in one send collection after local confirmation. This edits and persists the send task but never starts it.")]
    public static Task<string> SendCollectionAction(WpeGatewayClient gateway, string sendId, string[] packetIds, string action, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("sends.collection.action", new SendCollectionActionInput(sendId, packetIds, action, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_send_collection_clear"), Description("Clear one send task's packet collection after local confirmation. This never starts the sender.")]
    public static Task<string> SendCollectionClear(WpeGatewayClient gateway, string sendId, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("sends.collection.clear", new EntityWriteInput(sendId, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_robot_instruction_add"), Description("Add one validated native robot instruction after local confirmation. type is WPE's InstructionType numeric value and content uses its native type|parameter format.")]
    public static Task<string> RobotInstructionAdd(WpeGatewayClient gateway, string robotId, int type, string content, int insertAt, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("robots.instructions.add", new RobotInstructionAddInput(robotId, type, content, insertAt, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_robot_instruction_action"), Description("Move, delete, or clear native robot instructions after local confirmation. Indexes refer to the current robot_get instruction array.")]
    public static Task<string> RobotInstructionAction(WpeGatewayClient gateway, string robotId, int[] indexes, string action, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("robots.instructions.action", new RobotInstructionActionInput(robotId, indexes, action, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_auto_stores_list"), Description("List automatic warehouse rules without changing them.")]
    public static Task<string> AutoStoresList(WpeGatewayClient gateway, CancellationToken cancellationToken = default) => gateway.InvokeAsync("autoStores.list", null, cancellationToken);
    [McpServerTool(Name = "wpe_auto_stores_save"), Description("Create or update one automatic warehouse rule after local confirmation. An empty id creates a disabled rule.")]
    public static Task<string> AutoStoresSave(WpeGatewayClient gateway, string? id, string packetHead, string warehouseId, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("autoStores.save", new AutoStoresSaveInput(id, packetHead, warehouseId, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_auto_stores_set_enabled"), Description("Enable or disable one automatic warehouse rule after local confirmation.")]
    public static Task<string> AutoStoresSetEnabled(WpeGatewayClient gateway, string id, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("autoStores.setEnabled", new EntityEnabledInput(id, enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_auto_stores_delete"), Description("Delete one automatic warehouse rule after local confirmation.")]
    public static Task<string> AutoStoresDelete(WpeGatewayClient gateway, string id, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("autoStores.delete", new EntityWriteInput(id, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_warehouse_stores_action"), Description("Move, copy, export, or delete selected warehouse packets after local confirmation. Export opens WPE's local save dialog.")]
    public static Task<string> WarehouseStoresAction(WpeGatewayClient gateway, string warehouseId, string[] storeIds, string action, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("warehouses.stores.action", new WarehouseStoresActionInput(warehouseId, storeIds, action, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_warehouse_stores_command"), Description("Run the native warehouse import, export-all, or clear command after local confirmation. Import/export open WPE's local file dialogs.")]
    public static Task<string> WarehouseStoresCommand(WpeGatewayClient gateway, string warehouseId, string action, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("warehouses.stores.command", new WarehouseCommandInput(warehouseId, action, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_packet_edit_get"), Description("Get one proxy or inject captured packet for editing. Payload is returned as Base64 only for this explicit request.")]
    public static Task<string> PacketEditGet(WpeGatewayClient gateway, string list, long id, CancellationToken cancellationToken = default) => gateway.InvokeAsync("packet.edit.get", new PacketEditGetInput(list, id), cancellationToken);
    [McpServerTool(Name = "wpe_packet_edit_save"), Description("Replace one proxy or inject captured packet's socket and payload after local confirmation. The complete request is retained in MCP audit data.")]
    public static Task<string> PacketEditSave(WpeGatewayClient gateway, string list, long id, int socket, string payloadBase64, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("packet.edit.save", new PacketEditSaveInput(list, id, socket, payloadBase64, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_packet_edit_add_to_send"), Description("Copy one proxy or inject captured packet, using supplied Base64 bytes, into a send task after local confirmation. It never starts the sender.")]
    public static Task<string> PacketEditAddToSend(WpeGatewayClient gateway, string sendId, string list, long id, string payloadBase64, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("packet.edit.addToSend", new PacketEditAddToSendInput(sendId, list, id, payloadBase64, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_send_set_enabled"), Description("Enable or disable one existing WPE send task after local confirmation. This never starts the sender.")]
    public static Task<string> SendSetEnabled(WpeGatewayClient gateway, string id, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("sends.setEnabled", new EntityEnabledInput(id, enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_robot_set_enabled"), Description("Enable or disable one existing WPE robot task after local confirmation. This never starts the robot.")]
    public static Task<string> RobotSetEnabled(WpeGatewayClient gateway, string id, bool enabled, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("robots.setEnabled", new EntityEnabledInput(id, enabled, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_task_create"), Description("Create an empty send task, robot task, or warehouse after local confirmation.")]
    public static Task<string> TaskCreate(WpeGatewayClient gateway, string kind, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("tasks.create", new TaskCreateInput(kind, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_task_update"), Description("Update a task name and, for sends, editable scheduling settings after local confirmation. It never starts an executor.")]
    public static Task<string> TaskUpdate(WpeGatewayClient gateway, string kind, string id, string name, string idempotencyKey, bool? useSystemSocket = null, int? loopCount = null, int? loopInterval = null, string? notes = null, CancellationToken cancellationToken = default) => gateway.InvokeAsync("tasks.update", new TaskUpdateInput(kind, id, name, idempotencyKey, useSystemSocket, loopCount, loopInterval, notes), cancellationToken);
    [McpServerTool(Name = "wpe_tasks_move"), Description("Move selected send tasks, robot tasks, or warehouses using WPE's native list action after local confirmation.")]
    public static Task<string> TasksMove(WpeGatewayClient gateway, string kind, string[] ids, string direction, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("tasks.move", new TaskActionInput(kind, ids, direction, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_tasks_copy"), Description("Copy selected existing WPE send tasks, robot tasks, or warehouses after local confirmation. This never starts execution.")]
    public static Task<string> TasksCopy(WpeGatewayClient gateway, string kind, string[] ids, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("tasks.copy", new TaskActionInput(kind, ids, "copy", idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_tasks_delete"), Description("Delete selected existing WPE send tasks, robot tasks, or warehouses after local confirmation. This never starts execution.")]
    public static Task<string> TasksDelete(WpeGatewayClient gateway, string kind, string[] ids, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("tasks.delete", new TaskActionInput(kind, ids, "delete", idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_tasks_clear"), Description("Clear all WPE send tasks, robot tasks, or warehouses of one kind after local confirmation. This never starts execution.")]
    public static Task<string> TasksClear(WpeGatewayClient gateway, string kind, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("tasks.clear", new TaskCreateInput(kind, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_capture_add_to_send"), Description("Copy selected inject-capture packets into an existing send task after local confirmation. This never starts the sender.")]
    public static Task<string> CaptureAddToSend(WpeGatewayClient gateway, string sendId, long[] packetIds, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("capture.addToSend", new CaptureAddInput(sendId, packetIds, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_capture_add_to_warehouse"), Description("Copy selected inject-capture packets into an existing warehouse after local confirmation.")]
    public static Task<string> CaptureAddToWarehouse(WpeGatewayClient gateway, string warehouseId, long[] packetIds, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("capture.addToWarehouse", new CaptureAddInput(warehouseId, packetIds, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_capture_add_to_send"), Description("Copy selected proxy-capture packets into an existing send task after local confirmation. This never starts the sender.")]
    public static Task<string> ProxyCaptureAddToSend(WpeGatewayClient gateway, string sendId, long[] packetIds, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxyCapture.addToSend", new CaptureAddInput(sendId, packetIds, idempotencyKey), cancellationToken);
    [McpServerTool(Name = "wpe_proxy_capture_add_to_warehouse"), Description("Copy selected proxy-capture packets into an existing warehouse after local confirmation.")]
    public static Task<string> ProxyCaptureAddToWarehouse(WpeGatewayClient gateway, string warehouseId, long[] packetIds, string idempotencyKey, CancellationToken cancellationToken = default) => gateway.InvokeAsync("proxyCapture.addToWarehouse", new CaptureAddInput(warehouseId, packetIds, idempotencyKey), cancellationToken);
}

public sealed record PageInput(int? Limit = null, string? Cursor = null);
public sealed record AccountGetInput(string Id);
public sealed record FilterSetEnabledInput(string Id, bool Enabled, string IdempotencyKey);
public sealed record FiltersSetAllEnabledInput(bool Enabled, string IdempotencyKey);
public sealed record FilterCountsResetInput(string IdempotencyKey);
public sealed record FiltersMoveInput(string[] Ids, string Direction, string IdempotencyKey);
public sealed record FiltersCopyInput(string[] Ids, string IdempotencyKey);
public sealed record FilterCreateFromCaptureInput(long PacketId, string IdempotencyKey);
public sealed record FilterCreateInput(string IdempotencyKey);
public sealed record FilterUpdateInput(JsonElement Filter, string IdempotencyKey);
public sealed record FilterDeleteInput(string Id, string IdempotencyKey);
public sealed record FilterGetInput(string Id);
public sealed record AccountSetEnabledInput(string Id, bool Enabled, string IdempotencyKey);
public sealed record AccountCreateInput(string UserName, string Password, bool Enabled, bool LimitLinksEnabled, int LimitLinks, bool LimitDevicesEnabled, int LimitDevices, bool ExpiryEnabled, string? ExpiryTime, string IdempotencyKey);
public sealed record AccountUpdateInput(string Id, bool Enabled, bool LimitLinksEnabled, int LimitLinks, bool LimitDevicesEnabled, int LimitDevices, bool ExpiryEnabled, string? ExpiryTime, string? Password, string IdempotencyKey);
public sealed record AccountDeleteInput(string Id, string IdempotencyKey);
public sealed record AccountLoginsListInput(string Id, int? Limit = null, string? Cursor = null);
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
public sealed record ExecutorStopAllInput(string IdempotencyKey);
public sealed record StartModeSelectInput(string Mode, string IdempotencyKey);
public sealed record ProxyFailuresListInput(int? Limit = null);
public sealed record FirewallRulesListInput(string List, int? Limit = null, string? Cursor = null);
public sealed record FirewallRuleAddInput(string List, string Address, string IdempotencyKey, bool ExpiryEnabled = false, string? ExpiryTime = null);
public sealed record FirewallRuleRemoveInput(string List, string Address, string IdempotencyKey);
public sealed record PacketSearchInput(int? Limit = null, string? Cursor = null, string? Mode = null, string? Pattern = null, bool Hex = false, string Direction = "any");
public sealed record PacketGetInput(long Id, string? Mode = null, bool IncludePayload = false);
public sealed record CaptureFindNextInput(string Pattern, bool Hex = false, string Mode = "proxy", int FromIndex = 0, int FromPosition = 0);
public sealed record LogListInput(string Kind, int? Limit = null, string? Cursor = null);
public sealed record BytesTranscodeInput(string Text, bool Decode);
public sealed record BytesCompareInput(string Left, string Right, int MinimumRun = 2);
public sealed record BytesExtractInput(int Kind, string ContentBase64);
public sealed record EntityGetInput(string Id);
public sealed record EntityPageInput(string Id, int? Limit = null, string? Cursor = null);
public sealed record EntityEnabledInput(string Id, bool Enabled, string IdempotencyKey);
public sealed record TaskCreateInput(string Kind, string IdempotencyKey);
public sealed record TaskUpdateInput(string Kind, string Id, string Name, string IdempotencyKey, bool? UseSystemSocket = null, int? LoopCount = null, int? LoopInterval = null, string? Notes = null);
public sealed record TaskActionInput(string Kind, string[] Ids, string Direction, string IdempotencyKey);
public sealed record CaptureAddInput(string TargetId, long[] PacketIds, string IdempotencyKey);
public sealed record EntityWriteInput(string Id, string IdempotencyKey);
public sealed record SendCollectionActionInput(string SendId, string[] PacketIds, string Action, string IdempotencyKey);
public sealed record WarehouseStoresActionInput(string WarehouseId, string[] StoreIds, string Action, string IdempotencyKey);
public sealed record RobotInstructionAddInput(string RobotId, int Type, string Content, int InsertAt, string IdempotencyKey);
public sealed record RobotInstructionActionInput(string RobotId, int[] Indexes, string Action, string IdempotencyKey);
public sealed record AutoStoresSaveInput(string? Id, string PacketHead, string WarehouseId, string IdempotencyKey);
public sealed record WarehouseCommandInput(string WarehouseId, string Action, string IdempotencyKey);
public sealed record PacketEditGetInput(string List, long Id);
public sealed record PacketEditSaveInput(string List, long Id, int Socket, string PayloadBase64, string IdempotencyKey);
public sealed record PacketEditAddToSendInput(string SendId, string List, long Id, string PayloadBase64, string IdempotencyKey);
