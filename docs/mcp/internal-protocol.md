# WPE MCP Internal Pipe Protocol v1

This is the private transport between the .NET 10 `WPEMcpServer` Sidecar and the running .NET Framework WPE process. It is not the public MCP protocol, and it must never share code or versioning with injected-process IPC (`IpcProtocol`).

## Discovery and transport

- WPE publishes one current-user instance to `%LOCALAPPDATA%\WPE64\mcp\instances.json`.
- The record contains `protocol: 1`, a random per-run pipe name, the WPE PID and its UTC start time. The Sidecar requires exactly one instance.
- The named pipe is byte-mode and asynchronous. It is intentionally separate from all injection IPC pipes.
- A frame is a 4-byte little-endian length followed by a UTF-8 JSON document. Both request and response frames are limited to 1 MiB.
- WPE may run elevated while the Sidecar runs normally; pipe ACLs permit this local cross-integrity connection. Security comes from the unguessable per-run name and the current-user discovery location, not from a fixed pipe name.

## Request and response envelopes

```json
{
  "requestId": "opaque-request-id",
  "operation": "filters.create",
  "arguments": {
    "idempotencyKey": "UUID"
  }
}
```

```json
{
  "requestId": "opaque-request-id",
  "ok": true,
  "result": {}
}
```

Failures never include raw exception data across the pipe:

```json
{
  "requestId": "opaque-request-id",
  "ok": false,
  "error": "WPE gateway request failed."
}
```

`requestId` is an opaque response-correlation value. Mutation idempotency is separate and always uses an `idempotencyKey` UUID in `arguments`.

## Ownership boundaries

- `WPEMcpServer` owns MCP stdio, tool discovery, input schemas and conversion to this envelope. It never accesses WPE's SQLite data or WPEHook.
- `McpAgentGateway` owns pipe framing, operation dispatch and UI-thread handoff.
- `McpWriteGuard` owns mutation idempotency, local approval, expiry and complete audit records for the local WPE operator.
- WPE business methods own validation, in-memory changes, feed refresh and persistence. Gateway code must not write SQLite or mutate lists directly.

## Compatibility

- Version 1 accepts the envelope above. Adding optional response fields is compatible; changing frame encoding, required envelope fields or response semantics requires a protocol increment.
- Adding a public MCP tool also requires its Sidecar method, gateway operation, schema, `docs/mcp/tools.md`, `McpContract.ps1` and an appropriate live regression.
- A Sidecar and WPE must be launched from the same `pack` payload for end-to-end testing. `tools/list` alone only validates the Sidecar and cannot prove the target WPE supports a new operation.
