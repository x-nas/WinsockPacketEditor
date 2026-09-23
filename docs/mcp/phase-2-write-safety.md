# Phase 2: MCP write safety contract

Phase 2 does not make the MCP client a privileged WPE controller. Every mutation remains owned by WPE and is applied only after an in-app, user-visible approval.

The currently exposed tool's input/output contract is in [`schemas/write-tools.schema.json`](schemas/write-tools.schema.json).

## Execution sequence

```text
MCP tool call (operation + idempotencyKey)
  -> AgentGateway validates schema, capability and idempotency cache
  -> WPE UI displays an approval card with a human-readable diff
  -> user approves or rejects in WPE
  -> WPE applies one transaction on the UI thread
  -> audit event records outcome, affected ids and the complete request/result data
  -> MCP receives approved / rejected / expired result
```

## Rules

- Every mutating request requires a caller-provided UUID `idempotencyKey`; a repeated key returns the original completed result and never applies a second mutation.
- Approval expires after 60 seconds and is bound to the precise canonical request hash. Any changed field requires a new approval.
- The MCP client cannot programmatically answer WPE's approval dialog.
- Audit records contain operation, timestamp, request hash, outcome, complete arguments and complete result values. The local MCP caller is the current Windows user's WPE operator, so passwords, tokens and packet payloads are retained when supplied by a tool call.
- Mutations execute through existing `Operate` business methods on the UI thread. MCP never writes the SQLite database directly.
- A failed validation, rejection, timeout or cancellation causes no mutation.

## Rollout order

1. Reversible list and account configuration edits: enable/disable an existing filter or account, and firewall list add/remove.
2. Account and proxy configuration edits, after complete-data and field validation tests.
3. Operational actions (proxy start/stop, executor start/stop) with a high-risk approval class.
4. Packet transmission and injection controls last; they require a separate per-session safety switch and are out of scope for the first Phase 2 release.

## Initial tool set

| Tool | Approval class | Notes |
|---|---|---|
| `wpe_filter_set_enabled` | reversible-config | Existing filter only; no filter body edits. |
| `wpe_account_set_enabled` | reversible-config | Existing account only; changes its enabled state. |
| `wpe_proxy_auth_set_enabled` | reversible-config | Boolean-only authentication setting; rejects `Only_WPC_Client=true` with auth disabled. |
| `wpe_proxy_max_connections_set` | reversible-config | Integer-only limit; validates against the live memory-based cap before applying. |
| `wpe_proxy_socks5_port_set` | reversible-config | Integer-only port; validates range and conflict with the enabled HTTP port before applying. |
| `wpe_firewall_set_enabled` | reversible-config | Boolean-only firewall switch; reversible and persisted after local confirmation. |
| `wpe_proxy_only_wpc_set_enabled` | reversible-config | Boolean-only Only-WPC setting; enabling requires authentication to remain enabled. |
| `wpe_proxy_bind_ip_set` | reversible-config | Auto or explicit IPv4/IPv6 listening address; invalid explicit addresses are rejected before confirmation. |
| `wpe_external_proxy_set_enabled` | reversible-config | Boolean-only switch for the existing external endpoint; host/port are validated when enabling. |
| `wpe_proxy_start` | high-risk-operational | Starts configured proxy listeners after confirmation; idempotent when already running. |
| `wpe_proxy_stop` | high-risk-operational | Stops listeners after confirmation and may disconnect existing sessions. |
| `wpe_firewall_rule_add` | network-access | Adds one validated IPv4 address/range and persists it after approval. |
| `wpe_firewall_rule_remove` | network-access | Removes one exact existing IP/range from a named list. |

Packet/injection operations remain planned only; they are not exposed by this build. Proxy lifecycle tools are now exposed with the high-risk approval class.
