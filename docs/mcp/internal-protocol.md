# WPE MCP Internal Pipe Protocol v1

This protocol is private to WPE and `WPE.McpServer`. It is not MCP, and it must never share names, commands or compatibility rules with `WinsockPacketEditor.Ipc.IpcProtocol` used by injected targets.

## Transport

- Named pipe, byte mode, asynchronous.
- Pipe access is restricted to the interactive Windows user that owns WPE.
- Each frame is `u32 little-endian byteLength` followed by UTF-8 JSON.
- Maximum request and response frame: 1 MiB in phase 1.
- Payload-bearing packet responses have a smaller default data budget declared by the operation.

## Envelope

```json
{
  "version": 1,
  "kind": "request",
  "id": "opaque-request-id",
  "operation": "capture.search",
  "deadlineUtc": "2026-09-15T00:00:00.0000000Z",
  "arguments": {}
}
```

Responses use:

```json
{
  "version": 1,
  "kind": "response",
  "id": "opaque-request-id",
  "ok": true,
  "result": {},
  "error": null
}
```

Errors are application errors, not raw exception text:

```json
{
  "code": "wpe_offline|invalid_argument|forbidden|not_found|expired|busy|timeout|internal",
  "message": "Human-readable recovery guidance",
  "retryable": false
}
```

## Handshake

The first request must be `gateway.hello` and includes the connector version, selected instance id and short-lived proof. WPE rejects every other operation until the handshake succeeds. A connection is closed after three invalid envelopes, any version mismatch, an oversized frame, or a failed proof.

## Operations in phase 1

```text
runtime.status
capture.search
capture.get
logs.list
filters.list
executors.list
connections.list
accounts.list
firewall.get
bytes.transcode
bytes.compare
bytes.extract
```

## Versioning

- Any incompatible envelope or operation-contract change increments `version`.
- New optional fields are backward compatible and do not increment the version.
- Both sides reject unknown required fields and report a version mismatch rather than guessing.
