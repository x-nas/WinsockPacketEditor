# MCP tool surface

All names use the `wpe_` namespace in MCP. Underscores are deliberately used because they are valid in every MCP client's standard tool-name validator. Internal operation names omit that prefix.

| MCP tool | Internal operation | Permission | Sensitive data rule |
|---|---|---|---|
| `wpe_status_get` | `runtime.status` | `read.runtime` | No secrets |
| `wpe_capture_search` | `capture.search` | `read.capture` | Metadata and short preview only |
| `wpe_packet_get` | `capture.get` | `read.capture` | Payload opt-in, bounded and audited |
| `wpe_capture_find_next` | `capture.findNext` | `read.capture` | Native proxy/inject regex search with match byte offset and continuation cursor |
| `wpe_logs_list` | `logs.list` | `read.runtime` | Bounded tail and pagination |
| `wpe_filters_list` | `filters.list` | `read.runtime` | No edit capability |
| `wpe_filter_get` | `filters.get` | `read.runtime` | Existing editable configuration plus actual `enabled` state; no mutation |
| `wpe_filter_stats_get` | `filters.stats.get` | `read.runtime` | Runtime execution count and enabled state |
| `wpe_executors_list` | `executors.list` | `read.runtime` | No start/stop capability |
| `wpe_connections_list` | `connections.list` | `read.capture` | No credentials/tokens |
| `wpe_accounts_list` | `accounts.list` | `read.capture` | Never expose passwords |
| `wpe_account_get` | `accounts.get` | `read.capture` | Existing list metadata only; never exposes passwords or tokens |
| `wpe_account_logins_list` | `accounts.logins.list` | `read.capture` | Existing native login-location records; not a device inventory; no passwords or tokens |
| `wpe_firewall_get` | `firewall.get` | `read.runtime` | Rules only |
| `wpe_proxy_settings_get` | `proxy.settings.get` | `read.runtime` | Non-sensitive settings and limits only; no credentials |
| `wpe_proxy_config_get` | `proxy.config.get` | `read.runtime` | Complete non-sensitive proxy configuration snapshot |
| `wpe_proxy_runtime_get` | `proxy.runtime.get` | `read.runtime` | Live non-sensitive listener state and bounded counters; no payloads or credentials |
| `wpe_connections_summary_get` | `connections.summary.get` | `read.capture` | Protocol and WPC/ordinary counts only; no addresses, device identifiers, credentials or payloads |
| `wpe_firewall_rules_list` | `firewall.rules.list` | `read.runtime` | White/black list rules only |
| `wpe_sends_list` | `sends.list` | `read.runtime` | Bounded task metadata; no packet payloads |
| `wpe_send_get` | `sends.get` | `read.runtime` | Existing editable task configuration only |
| `wpe_send_collection_list` | `sends.collection.list` | `read.runtime` | Bounded collection metadata and short preview; no payload |
| `wpe_robots_list` | `robots.list` | `read.runtime` | Bounded robot metadata; no payloads |
| `wpe_robot_get` | `robots.get` | `read.runtime` | Existing instruction configuration only |
| `wpe_warehouse_list` | `warehouses.list` | `read.runtime` | Bounded warehouse metadata; no packet payloads |
| `wpe_warehouse_get` | `warehouses.get` | `read.runtime` | Bounded stored-packet metadata; no payload |
| `wpe_auto_stores_list` | `autoStores.list` | `read.runtime` | Automatic-storage rules only; no mutation |
| `wpe_packet_edit_get` | `packet.edit.get` | `read.capture` | Explicit editing snapshot; payload returned as Base64 |
| `wpe_proxy_bind_ip_set` | `proxy.bindIp.set` | `write.proxy` | Auto or validated IPv4/IPv6 listening address, persisted after WPE-local confirmation |
| `wpe_external_proxy_set_enabled` | `proxy.external.setEnabled` | `write.proxy` | Toggles the existing external proxy endpoint only; credentials are never returned or modified |
| `wpe_proxy_start` | `proxy.start` | `write.proxy.lifecycle` | Starts configured listeners after confirmation |
| `wpe_proxy_stop` | `proxy.stop` | `write.proxy.lifecycle` | Stops listeners after confirmation; existing connections may disconnect |
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
| `wpe_filter_set_enabled` | `filters.setEnabled` | `write.filter` | The only filter enable/disable operation; call after rule update when enablement is requested |
| `wpe_filters_set_all_enabled` | `filters.setAllEnabled` | `write.filter` | Existing toolbar bulk enable/disable action |
| `wpe_filter_counts_reset` | `filters.counts.reset` | `write.filter` | Existing toolbar runtime-count reset; does not alter rules |
| `wpe_filters_move` | `filters.move` | `write.filter` | Existing top/up/down/bottom list action; ordering affects execution |
| `wpe_filters_copy` | `filters.copy` | `write.filter` | Existing list copy action |
| `wpe_filters_clear_all` | `filters.clearAll` | `write.filter` | Existing clear action; WPE-native destructive confirmation remains |
| `wpe_filter_create_from_capture` | `filters.createFromCapture` | `write.filter` | Existing captured-packet-to-filter action |
| `wpe_filter_create` | `filters.create` | `write.filter` | Creates an empty filter through the existing WPE business path after confirmation |
| `wpe_filter_update` | `filters.update` | `write.filter` | Replaces rules and normal/advanced mode through `SaveFilterEdit`; deliberately does not change enabled state |
| `wpe_filter_delete` | `filters.delete` | `write.filter` | Deletes an existing filter through the existing WPE list action after confirmation |
| `wpe_account_set_enabled` | `accounts.setEnabled` | `write.account` | Existing account only; enable/disable metadata, no password access, UUID idempotency key, WPE-local confirmation |
| `wpe_account_create` | `accounts.create` | `write.account` | Existing account-editor fields, UUID idempotency key, WPE-local confirmation; password is redacted from audit data |
| `wpe_account_update` | `accounts.update` | `write.account` | Existing account-editor fields except immutable user name; omitting password preserves it |
| `wpe_account_delete` | `accounts.delete` | `write.account` | Delete one existing account after WPE-local confirmation |
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
| `wpe_executors_stop_all` | `executors.stopAll` | `write.executors.emergency` | Stops active sender and robot executors only; never starts tasks or sends packets; follows the global MCP confirmation setting |
| `wpe_start_mode_select` | `start.mode.select` | `write.startup.mode` | Selects proxy or inject mode only while WPE is on the start page; repeated current mode returns `alreadySelected`, other pages return `unavailable`; it never starts proxy or injects |
| `wpe_firewall_rule_add` | `firewall.rule.add` | `write.firewall` | Validated IPv4/range, optional ISO-8601 expiry, UUID idempotency key, WPE-local confirmation, persisted before success |
| `wpe_firewall_rule_remove` | `firewall.rule.remove` | `write.firewall` | Exact existing IP/range, UUID idempotency key, WPE-local confirmation, redacted audit event |
| `wpe_send_set_enabled` | `sends.setEnabled` | `write.task` | Existing send task only; UUID idempotency key and WPE-local confirmation |
| `wpe_robot_set_enabled` | `robots.setEnabled` | `write.task` | Existing robot task only; UUID idempotency key and WPE-local confirmation |
| `wpe_task_create` | `tasks.create` | `write.task` | Creates one empty send, robot, or warehouse through its native path; never starts execution |
| `wpe_task_update` | `tasks.update` | `write.task` | Updates native editable configuration; never starts execution or sends a packet |
| `wpe_tasks_move` | `tasks.move` | `write.task` | Existing top/up/down/bottom list action after local confirmation |
| `wpe_tasks_copy` | `tasks.copy` | `write.task` | Existing native list copy action after local confirmation |
| `wpe_tasks_delete` | `tasks.delete` | `write.task` | Existing native list delete action after local confirmation |
| `wpe_tasks_clear` | `tasks.clear` | `write.task` | Existing native clear action after local confirmation; native destructive confirmation remains |
| `wpe_capture_add_to_send` | `capture.addToSend` | `write.task` | Adds selected inject-capture packets to an existing send task; does not execute it |
| `wpe_capture_add_to_warehouse` | `capture.addToWarehouse` | `write.task` | Adds selected inject-capture packets to an existing warehouse |
| `wpe_proxy_capture_add_to_send` | `proxyCapture.addToSend` | `write.task` | Adds selected proxy-capture packets to an existing send task; does not execute it |
| `wpe_proxy_capture_add_to_warehouse` | `proxyCapture.addToWarehouse` | `write.task` | Adds selected proxy-capture packets to an existing warehouse |
| `wpe_send_collection_action` | `sends.collection.action` | `write.task` | Native move/copy/delete action for a send collection; never starts it |
| `wpe_send_collection_clear` | `sends.collection.clear` | `write.task` | Native destructive clear for one send collection |
| `wpe_robot_instruction_add` | `robots.instructions.add` | `write.task` | Validated native robot instruction insertion; never starts it |
| `wpe_robot_instruction_action` | `robots.instructions.action` | `write.task` | Native move/delete/clear by current instruction indexes |
| `wpe_auto_stores_save` | `autoStores.save` | `write.task` | Creates or updates an automatic-storage rule through the existing validation path |
| `wpe_auto_stores_set_enabled` | `autoStores.setEnabled` | `write.task` | Toggles an existing automatic-storage rule |
| `wpe_auto_stores_delete` | `autoStores.delete` | `write.task` | Deletes an automatic-storage rule; native confirmation remains |
| `wpe_warehouse_stores_action` | `warehouses.stores.action` | `write.task` | Native selected warehouse-packet action; export opens WPE's save dialog |
| `wpe_warehouse_stores_command` | `warehouses.stores.command` | `write.task` | Native import/export-all/clear; file commands open WPE's local dialogs |
| `wpe_packet_edit_save` | `packet.edit.save` | `write.capture` | Replaces an explicit proxy/inject capture snapshot; payload is Base64 and audit-redacted |
| `wpe_packet_edit_add_to_send` | `packet.edit.addToSend` | `write.task` | Copies an explicit proxy/inject capture snapshot into a send task; never starts it |

`wpe_filter_set_enabled` calls WPE's normal `SetFilterEnable_ById` path, so configuration persistence and UI refresh remain identical to a local change. Firewall add/remove tools call the normal `Operate.ProxyConfig.Proxy` business paths; add waits for IP-location lookup, list insertion and database persistence before returning success, while remove only accepts an exact existing address. Rejected or expired confirmation requests make no change.
