# MCP tool surface

All names use the `wpe_` namespace in MCP. Underscores are deliberately used because they are valid in every MCP client's standard tool-name validator. Internal operation names omit that prefix.

| MCP tool | Internal operation | Permission | Sensitive data rule |
|---|---|---|---|
| `wpe_status_get` | `runtime.status` | `read.runtime` | No secrets |
| `wpe_capture_search` | `capture.search` | `read.capture` | Metadata and short preview only |
| `wpe_packet_get` | `capture.get` | `read.capture` | Payload opt-in, bounded and audited |
| `wpe_logs_list` | `logs.list` | `read.runtime` | Bounded tail and pagination |
| `wpe_filters_list` | `filters.list` | `read.runtime` | No edit capability |
| `wpe_executors_list` | `executors.list` | `read.runtime` | No start/stop capability |
| `wpe_connections_list` | `connections.list` | `read.capture` | No credentials/tokens |
| `wpe_accounts_list` | `accounts.list` | `read.capture` | Never expose passwords |
| `wpe_firewall_get` | `firewall.get` | `read.runtime` | Rules only |
| `wpe_proxy_settings_get` | `proxy.settings.get` | `read.runtime` | Non-sensitive settings and limits only; no credentials |
| `wpe_proxy_runtime_get` | `proxy.runtime.get` | `read.runtime` | Live non-sensitive listener state and bounded counters; no payloads or credentials |
| `wpe_connections_summary_get` | `connections.summary.get` | `read.capture` | Protocol and WPC/ordinary counts only; no addresses, device identifiers, credentials or payloads |
| `wpe_proxy_failures_list` | `proxy.failures.list` | `read.runtime` | Bounded failure summaries only; account names, IPs, paths, credentials, tokens and full stacks are omitted |
| `wpe_proxy_health_get` | `proxy.health.get` | `read.runtime` | Read-only consistency check; never starts, stops, or repairs proxy |
| `wpe_executors_detail_get` | `executors.detail.get` | `read.runtime` | Executor counts only; never controls executor tasks |
| `wpe_storage_health_get` | `storage.health.get` | `read.runtime` | Database availability metadata with path hidden and contents excluded |
| `wpe_proxy_bind_ip_set` | `proxy.bindIp.set` | `write.proxy` | Auto or validated IPv4/IPv6 listening address, persisted after WPE-local confirmation |
| `wpe_external_proxy_set_enabled` | `proxy.external.setEnabled` | `write.proxy` | Toggles the existing external proxy endpoint only; credentials are never returned or modified |
| `wpe_proxy_start` | `proxy.start` | `write.proxy.lifecycle` | Starts configured listeners after confirmation |
| `wpe_proxy_stop` | `proxy.stop` | `write.proxy.lifecycle` | Stops listeners after confirmation; existing connections may disconnect |
| `wpe_firewall_rules_list` | `firewall.rules.list` | `read.runtime` | White/black list rules only |
| `wpe_bytes_transcode` | `bytes.transcode` | `read.runtime` | Caller-supplied data only |
| `wpe_bytes_compare` | `bytes.compare` | `read.runtime` | Caller-supplied data only |
| `wpe_bytes_extract` | `bytes.extract` | `read.runtime` | Caller-supplied data only |

## Pagination and limits

- Every list/search requires `limit` in the inclusive range 1..200; omitted means 50.
- Results are sorted deterministically and return an opaque `nextCursor` when more data exists.
- Capture previews are capped at 64 bytes.
- `wpe_packet_get` returns no bytes unless `includePayload=true`.
- A packet payload has a 4 MiB encoded-data budget. A larger packet returns metadata, SHA-256 and `truncated=true`.
- No tool uses `0` to mean unlimited.

## Phase 2 write tools

| MCP tool | Internal operation | Permission | Safety contract |
|---|---|---|---|
| `wpe_filter_set_enabled` | `filters.setEnabled` | `write.filter` | UUID idempotency key, WPE-local confirmation, 60-second expiry, redacted audit event |
| `wpe_account_set_enabled` | `accounts.setEnabled` | `write.account` | Existing account only; enable/disable metadata, no password access, UUID idempotency key, WPE-local confirmation |
| `wpe_proxy_auth_set_enabled` | `proxy.auth.setEnabled` | `write.proxy` | Boolean-only setting, rejects incompatible Only-WPC state, persists after WPE-local confirmation; no credentials |
| `wpe_proxy_http_set_enabled` | `proxy.http.setEnabled` | `write.proxy` | Boolean-only setting, validates SOCKS5/HTTP port compatibility, persists after WPE-local confirmation |
| `wpe_proxy_max_connections_set` | `proxy.maxConnections.set` | `write.proxy` | Integer setting validated against the live machine cap; persists after WPE-local confirmation |
| `wpe_proxy_socks5_port_set` | `proxy.socks5Port.set` | `write.proxy` | Integer port setting, validates range and HTTP conflict, persists after WPE-local confirmation |
| `wpe_proxy_http_port_set` | `proxy.httpPort.set` | `write.proxy` | Integer port setting, requires HTTP enabled and validates SOCKS5 conflict, persists after WPE-local confirmation |
| `wpe_firewall_set_enabled` | `firewall.setEnabled` | `write.firewall` | Boolean-only firewall switch, persists after WPE-local confirmation |
| `wpe_proxy_only_wpc_set_enabled` | `proxy.onlyWpc.setEnabled` | `write.proxy` | Boolean-only setting, requires authentication when enabled, persists after WPE-local confirmation |
| `wpe_external_proxy_set_enabled` | `proxy.external.setEnabled` | `write.proxy` | Boolean-only external proxy switch; validates configured host/port when enabling, never handles credentials |
| `wpe_proxy_start` | `proxy.start` | `write.proxy.lifecycle` | Starts listeners after local confirmation; idempotent when already running |
| `wpe_proxy_stop` | `proxy.stop` | `write.proxy.lifecycle` | Stops listeners after local confirmation; may disconnect existing sessions |
| `wpe_firewall_rule_add` | `firewall.rule.add` | `write.firewall` | Validated IPv4/range, optional ISO-8601 expiry, UUID idempotency key, WPE-local confirmation, persisted before success |
| `wpe_firewall_rule_remove` | `firewall.rule.remove` | `write.firewall` | Exact existing IP/range, UUID idempotency key, WPE-local confirmation, redacted audit event |

`wpe_filter_set_enabled` calls WPE's normal `SetFilterEnable_ById` path, so configuration persistence and UI refresh remain identical to a local change. Firewall add/remove tools call the normal `Operate.ProxyConfig.Proxy` business paths; add waits for IP-location lookup, list insertion and database persistence before returning success, while remove only accepts an exact existing address. Rejected or expired confirmation requests make no change.
