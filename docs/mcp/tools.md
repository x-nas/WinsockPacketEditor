# MCP tool surface

All names use the `wpe_` namespace in MCP. Underscores are deliberately used because they are valid in every MCP client's standard tool-name validator. Internal operation names omit that prefix.

For filters and WPC servers, reference fields accept an exact visible name or a GUID. Proxy accounts, send tasks, robots, and warehouses require the GUID returned by their list/create tools, so automated calls cannot silently select an ambiguous display name. Captured packets, live connections, WPC rule rows, stored packets, and automatic-storage rules remain runtime records and likewise require returned IDs.

| MCP tool | Internal operation | Permission | Sensitive data rule |
|---|---|---|---|
| `wpe_status_get` | `runtime.status` | `read.runtime` | No secrets |
| `wpe_capture_search` | `capture.search` | `read.capture` | Metadata and short preview only |
| `wpe_packet_get` | `capture.get` | `read.capture` | Complete current and original payloads as Base64 |
| `wpe_capture_find_next` | `capture.findNext` | `read.capture` | Native proxy/inject regex search with match byte offset and continuation cursor |
| `wpe_logs_list` | `logs.list` | `read.runtime` | Bounded tail and pagination |
| `wpe_logs_all_list` | `logs.all.list` | `read.runtime` | One time-ordered, paginated stream across system, filter, proxy and MCP logs; every row includes `kind` |
| `wpe_filters_list` | `filters.list` | `read.runtime` | No edit capability |
| `wpe_filter_get` | `filters.get` | `read.runtime` | Semantic rule object plus actual `enabled` state; its `rule` can be used directly as the basis for `wpe_filter_rule_save` |
| `wpe_filter_stats_get` | `filters.stats.get` | `read.runtime` | Runtime execution count and enabled state |
| `wpe_executors_list` | `executors.list` | `read.runtime` | No start/stop capability |
| `wpe_connections_list` | `connections.list` | `read.capture` | Bounded native connection records |
| `wpe_accounts_list` | `accounts.list` | `read.capture` | `userName` is optional case-insensitive login-name text search; use each returned `Id` GUID for later account calls |
| `wpe_account_get` | `accounts.get` | `read.capture` | Complete account configuration, decrypted password and login records; `id` is the GUID returned by `wpe_accounts_list`, never `userName` |
| `wpe_account_logins_list` | `accounts.logins.list` | `read.capture` | Existing native login-location records; `id` is the GUID returned by `wpe_accounts_list`, not `userName` |
| `wpe_firewall_get` | `firewall.get` | `read.runtime` | Rules only |
| `wpe_proxy_settings_get` | `proxy.settings.get` | `read.runtime` | Settings, limits and configured external-proxy credentials |
| `wpe_proxy_config_get` | `proxy.config.get` | `read.runtime` | Complete proxy configuration snapshot |
| `wpe_proxy_runtime_get` | `proxy.runtime.get` | `read.runtime` | Live listener state and bounded counters |
| `wpe_remote_management_get` | `remoteManagement.get` | `read.runtime` | Remote-management address, administrator credentials, available local addresses and current HTTP-server state |
| `wpe_setting_get` | `settings.get` | `read.runtime` | Complete snapshot for a named WPE settings page |
| `wpe_wpc_servers_list` | `wpc.servers.list` | `read.runtime` | WPC server configurations including all three client URLs and nested rule counts |
| `wpe_wpc_server_rules_list` | `wpc.server.rules.list` | `read.runtime` | Rules for one WPC server, including native numeric type/action values |
| `wpe_connections_summary_get` | `connections.summary.get` | `read.capture` | Complete count fields for the summary's protocol and WPC/ordinary groups |
| `wpe_firewall_rules_list` | `firewall.rules.list` | `read.runtime` | White/black list rules only |
| `wpe_sends_list` | `sends.list` | `read.runtime` | Bounded task metadata; use the collection/detail tools for task data |
| `wpe_send_get` | `sends.get` | `read.runtime` | Existing editable task configuration only |
| `wpe_send_collection_list` | `sends.collection.list` | `read.runtime` | Bounded collection records with native preview fields |
| `wpe_robots_list` | `robots.list` | `read.runtime` | Bounded robot metadata |
| `wpe_robot_get` | `robots.get` | `read.runtime` | Existing instruction configuration only |
| `wpe_warehouse_list` | `warehouses.list` | `read.runtime` | Bounded warehouse metadata |
| `wpe_warehouse_get` | `warehouses.get` | `read.runtime` | One warehouse's bounded stored-packet records; `id` is required and must be the `Id` GUID returned by `wpe_warehouse_list`, never its display name |
| `wpe_auto_stores_list` | `autoStores.list` | `read.runtime` | Automatic-storage rules only; no mutation |
| `wpe_packet_edit_get` | `packet.edit.get` | `read.capture` | Explicit editing snapshot; payload returned as Base64 |
| `wpe_proxy_bind_ip_set` | `proxy.bindIp.set` | `write.proxy` | Auto or validated IPv4/IPv6 listening address, persisted after WPE-local confirmation |
| `wpe_external_proxy_set_enabled` | `proxy.external.setEnabled` | `write.proxy` | Toggles the existing external proxy endpoint only; use proxy settings/config tools to inspect credentials |
| `wpe_proxy_start` | `proxy.start` | `write.proxy.lifecycle` | Starts configured listeners after confirmation |
| `wpe_proxy_stop` | `proxy.stop` | `write.proxy.lifecycle` | Stops listeners after confirmation; existing connections may disconnect |
| `wpe_bytes_transcode` | `bytes.transcode` | `read.runtime` | Caller-supplied data only |
| `wpe_bytes_compare` | `bytes.compare` | `read.runtime` | Caller-supplied data only |
| `wpe_bytes_extract` | `bytes.extract` | `read.runtime` | Caller-supplied data only |

## Pagination and limits

- Every list/search requires `limit` in the inclusive range 1..200; omitted means 50.
- Results are sorted deterministically and return an opaque `nextCursor` when more data exists.
- Capture previews are capped at 64 bytes.
- `wpe_packet_get` always returns complete current and original bytes as Base64. The legacy `includePayload` argument is accepted for compatibility but no longer restricts output.
- `truncated` is always `false`; the current-user MCP caller receives the full packet data within the Named Pipe protocol allocation guard.
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
| `wpe_filter_rule_save` | `filters.rule.save` | `write.filter` | Complete semantic rule editor. Use `replace` for selected-byte substitutions; `change` replaces the whole packet and needs contiguous modify cells from offset 0. Advanced rules must state `startFrom`: `position` applies to every match, `head` only the first. `packetTypes` must select one or more types; use `all` for all types in the current mode. It preserves enablement |
| `wpe_filter_delete` | `filters.delete` | `write.filter` | Deletes an existing filter through the existing WPE list action after confirmation |
| `wpe_account_set_enabled` | `accounts.setEnabled` | `write.account` | `id` is the GUID returned by `wpe_accounts_list`; UUID idempotency key and WPE-local confirmation |
| `wpe_account_create` | `accounts.create` | `write.account` | `userName` is new login-name text, not a GUID; complete arguments are retained in audit data |
| `wpe_account_update` | `accounts.update` | `write.account` | `id` is the GUID returned by `wpe_accounts_list`, never `userName`; username is immutable and omitting password preserves it |
| `wpe_account_delete` | `accounts.delete` | `write.account` | `id` is the GUID returned by `wpe_accounts_list`, never `userName` |
| `wpe_proxy_auth_set_enabled` | `proxy.auth.setEnabled` | `write.proxy` | Boolean-only setting, rejects incompatible Only-WPC state and persists after WPE-local confirmation |
| `wpe_proxy_max_connections_set` | `proxy.maxConnections.set` | `write.proxy` | Integer setting validated against the live machine cap; persists after WPE-local confirmation |
| `wpe_proxy_socks5_port_set` | `proxy.socks5Port.set` | `write.proxy` | Integer port setting, validates range and HTTP conflict, persists after WPE-local confirmation |
| `wpe_firewall_set_enabled` | `firewall.setEnabled` | `write.firewall` | Boolean-only firewall switch, persists after WPE-local confirmation |
| `wpe_proxy_only_wpc_set_enabled` | `proxy.onlyWpc.setEnabled` | `write.proxy` | Boolean-only setting, requires authentication when enabled, persists after WPE-local confirmation |
| `wpe_external_proxy_set_enabled` | `proxy.external.setEnabled` | `write.proxy` | Boolean-only external proxy switch; validates configured host/port when enabling |
| `wpe_proxy_start` | `proxy.start` | `write.proxy.lifecycle` | Starts listeners after local confirmation; idempotent when already running |
| `wpe_proxy_stop` | `proxy.stop` | `write.proxy.lifecycle` | Stops listeners after local confirmation; may disconnect existing sessions |
| `wpe_remote_management_save` | `remoteManagement.save` | `write.remote-management` | Saves the existing remote-management enabled state, bind address, port and administrator credentials; starts or stops its HTTP server accordingly |
| `wpe_setting_save` | `settings.save` | `write.settings` | Persists a complete named settings-page configuration through WPE's native business rules |
| `wpe_map_local_save` / `wpe_map_remote_save` | `map.local.save` / `map.remote.save` | `write.mapping` | Creates or updates the two native HTTP mapping record types |
| `wpe_export` / `wpe_import` | `export.run` / `import.run` | `write.export` / `write.import` | The two unified file-workflow tools cover every native WPE export and import. Use `kind: backup` plus `backupParts.all: true` to export every module. |
| `wpe_wpc_server_save` | `wpc.server.save` | `write.wpc` | Creates when `id` is omitted; updates accept the exact visible server name or its GUID |
| `wpe_wpc_server_rule_save` | `wpc.server.rule.save` | `write.wpc` | `serverId` accepts the exact visible server name or GUID; creates rules when `id` is omitted. Global proxy is enabled `type: 15`, empty `argument`, `action: 0` |
| `wpe_executors_stop_all` | `executors.stopAll` | `write.executors.emergency` | Stops active sender and robot executors only; never starts tasks or sends packets; follows the global MCP confirmation setting |
| `wpe_start_mode_select` | `start.mode.select` | `write.startup.mode` | Selects proxy or inject mode only while WPE is on the start page; repeated current mode returns `alreadySelected`, other pages return `unavailable`; it never starts proxy or injects |
| `wpe_firewall_rule_add` | `firewall.rule.add` | `write.firewall` | Validated IPv4/range, optional ISO-8601 expiry, UUID idempotency key, WPE-local confirmation, persisted before success |
| `wpe_firewall_rule_remove` | `firewall.rule.remove` | `write.firewall` | Exact existing IP/range, UUID idempotency key, WPE-local confirmation, complete audit event |
| `wpe_send_set_enabled` | `sends.setEnabled` | `write.task` | Existing send task only; UUID idempotency key and WPE-local confirmation |
| `wpe_robot_set_enabled` | `robots.setEnabled` | `write.task` | Existing robot task only; UUID idempotency key and WPE-local confirmation |
| `wpe_send_create` | `tasks.create` | `write.task` | Creates one empty send task; never sends packets |
| `wpe_robot_create` | `tasks.create` | `write.task` | Creates one empty robot task; never executes it |
| `wpe_warehouse_create` | `tasks.create` | `write.task` | Creates one empty warehouse; never imports or changes packets |
| `wpe_send_update` | `tasks.update` | `write.send` | Updates one send task by returned GUID; omitted scheduling fields retain their current values and it never sends packets |
| `wpe_robot_update` | `tasks.update` | `write.robot` | Renames one robot by returned GUID; it never starts execution |
| `wpe_warehouse_update` | `tasks.update` | `write.warehouse` | Renames one warehouse by returned GUID |
| `wpe_send_move` / `wpe_robot_move` / `wpe_warehouse_move` | `tasks.move` | scope-specific write | Moves only the stated task scope by returned GUIDs; direction is top, up, down, or bottom |
| `wpe_send_copy` / `wpe_robot_copy` / `wpe_warehouse_copy` | `tasks.copy` | scope-specific write | Copies only the stated task scope by returned GUIDs; does not start execution |
| `wpe_send_delete` / `wpe_robot_delete` / `wpe_warehouse_delete` | `tasks.delete` | scope-specific write | Deletes only the stated task scope by returned GUIDs after local confirmation |
| `wpe_sends_clear` / `wpe_robots_clear` / `wpe_warehouses_clear` | `tasks.clear` | scope-specific write | Clears only the stated scope after local confirmation; WPE-native destructive confirmation remains |
| `wpe_capture_add_to_send` | `capture.addToSend` | `write.task` | Adds selected inject-capture packets to an existing send task; does not execute it |
| `wpe_capture_add_to_warehouse` | `capture.addToWarehouse` | `write.task` | Adds selected inject-capture packets to an existing warehouse |
| `wpe_proxy_capture_add_to_send` | `proxyCapture.addToSend` | `write.task` | Adds selected proxy-capture packets to an existing send task; does not execute it |
| `wpe_proxy_capture_add_to_warehouse` | `proxyCapture.addToWarehouse` | `write.task` | Adds selected proxy-capture packets to an existing warehouse |
| `wpe_send_collection_action` | `sends.collection.action` | `write.task` | Native move/copy/delete action for a send collection; never starts it |
| `wpe_send_collection_clear` | `sends.collection.clear` | `write.task` | Native destructive clear for one send collection |
| `wpe_robot_instruction_add` | `robots.instructions.add` | `write.task` | Validated native robot instruction insertion; never starts it. `robot` accepts the exact visible name (for example `机器人 1`) or a GUID; WPE resolves names internally and reports missing/duplicate names. `content` is the parameter only: a fixed delay is `type: 1, content: "1000"`; never send `1|1000` or `延迟|1000`. |
| `wpe_robot_instructions_save` | `robots.instructions.save` | `write.task` | Atomically replaces one robot's complete instruction list, so a loop pair is never rejected in its temporary unmatched state. Use `type: 2, content: "10"` for loop start, `type: 1, content: "1000"` for delay, `type: 4, content: "Press|D1"` for the number-row 1 key, and `type: 3, content: ""` for loop end. |
| `wpe_robot_instruction_action` | `robots.instructions.action` | `write.task` | Native move/delete/clear by current instruction indexes |
| `wpe_auto_stores_save` | `autoStores.save` | `write.task` | Creates or updates an automatic-storage rule through the existing validation path |
| `wpe_auto_stores_set_enabled` | `autoStores.setEnabled` | `write.task` | Toggles an existing automatic-storage rule |
| `wpe_auto_stores_delete` | `autoStores.delete` | `write.task` | Deletes an automatic-storage rule; native confirmation remains |
| `wpe_warehouse_stores_action` | `warehouses.stores.action` | `write.task` | Native selected warehouse-packet action; export opens WPE's save dialog |
| `wpe_warehouse_stores_command` | `warehouses.stores.command` | `write.task` | Native import/export-all/clear; file commands open WPE's local dialogs |
| `wpe_packet_edit_save` | `packet.edit.save` | `write.capture` | Replaces an explicit proxy/inject capture snapshot; payload is Base64 and retained in the complete audit record |
| `wpe_packet_edit_add_to_send` | `packet.edit.addToSend` | `write.task` | Copies an explicit proxy/inject capture snapshot into a send task; never starts it |
| `wpe_capture_clear` | `capture.clear` | `write.capture` | Clears one native proxy or inject capture list after confirmation |
| `wpe_export` (`kind: capture`) | `export.run` | `write.export` | Exports selected or all proxy/inject captured packets through WPE's native save dialog |
| `wpe_import` (`kind: filters`) | `import.run` | `write.import` | Opens WPE's native file and encryption-password dialogs |
| `wpe_export` (`kind: filters`) | `export.run` | `write.export` | Opens WPE's native save and encryption-password dialogs |
| `wpe_export` | `export.run` | `write.export` | The only export tool. It covers backups, captures, filters, certificates, accounts, firewall lists, mappings, sends, robots, send collections, warehouses and their packets, automatic-storage rules, logs, and extraction results. `kind` selects the format; `fileName` pre-fills the native save dialog. |
| `wpe_sends_start` / `wpe_sends_stop` | `sends.start` / `sends.stop` | `write.executors` | Starts or stops WPE's existing send-list worker |
| `wpe_send_start` | `send.start` | `write.executors` | Starts one enabled native send task |
| `wpe_robots_start` / `wpe_robots_stop` | `robots.start` / `robots.stop` | `write.executors` | Starts or stops WPE's existing robot-list worker |
| `wpe_robot_start` | `robot.start` | `write.executors` | Starts one enabled native robot task |
| `wpe_inject_attach` | `inject.attach` | `write.inject` | Uses WPE's existing attach or start-and-inject path |
| `wpe_inject_quick_attach` | `inject.quickAttach` | `write.inject` | Repeats the existing WPE quick-inject action |
| `wpe_inject_detach` | `inject.detach` | `write.inject` | Detaches the current native injection link |
| `wpe_inject_start_hook` / `wpe_inject_stop_hook` | `inject.startHook` / `inject.stopHook` | `write.inject` | Starts or stops the existing hook on the attached target |
| `wpe_driver_uninstall` | `driver.uninstall` | `write.driver` | Opens WPE's existing driver-uninstall confirmation flow |
| `wpe_process_proxy_save` | `processProxy.save` | `write.driver` | Saves existing process-proxy settings and performs its native on-demand driver install/configuration |
| `wpe_packet_edit_send_start` / `wpe_packet_edit_send_stop` | `packetEdit.sendStart` / `packetEdit.sendStop` | `write.capture` | Starts or stops the existing packet-editor send session |

`wpe_filter_set_enabled` calls WPE's normal `SetFilterEnable_ById` path, so configuration persistence and UI refresh remain identical to a local change. Firewall add/remove tools call the normal `Operate.ProxyConfig.Proxy` business paths; add waits for IP-location lookup, list insertion and database persistence before returning success, while remove only accepts an exact existing address. Rejected or expired confirmation requests make no change.
