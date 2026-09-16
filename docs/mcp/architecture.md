# MCP 架构

## 目标结构

```text
MCP client (Codex / Claude / compatible client)
        | stdio, JSON-RPC
        v
WPE.McpServer.exe (.NET 10, asInvoker)
        | authenticated current-user Named Pipe
        v
WPE AgentGateway (.NET Framework 4.8, in WPE main process)
        | UI-thread dispatch / approval / audit
        v
IWpeReadService -> Operate + ShellForm coordinators
        |
        +-- existing WPEHook IPC (unchanged)
```

## Component responsibilities

### WPE.McpServer

- Implements only MCP transport, discovery, schemas, pagination and MCP-shaped errors.
- Writes MCP messages only to stdout; diagnostics go only to stderr.
- Opens no database and does not access packet capture structures directly.
- Connects to a selected running WPE instance and exits cleanly when stdin closes.

### AgentGateway

- Runs while WPE is running, but remains inert until a local MCP client connects.
- Accepts one current-user client connection at a time in v1.
- Validates internal protocol version, frame sizes, deadlines and allowed operation names. The Windows named-pipe ACL is the phase-1 authentication boundary; an application-level handshake is deferred until a remote transport is introduced.
- Marshals list snapshots and mutations through WPE's existing UI dispatcher.
- Emits only detached DTO snapshots. No BindingList, WinForms object, byte array or mutable model escapes the UI thread.
- Owns audit records and the later approval gate.

### IWpeReadService

- Is the application boundary for phase 1 tools.
- Returns bounded immutable DTOs and opaque cursors.
- Does not activate proxy services merely to answer a query.
- Does not call UI dialogs, clipboard, native file pickers or WebView2.

## Threading rules

1. Copy mutable WPE lists on the UI thread via `Operate.SystemConfig.InvokeAction`.
2. Perform serialization, cursor construction and base64 encoding after the copy on a worker thread.
3. Never block the UI thread on pipe I/O or an MCP client response.
4. Never reuse the hook IPC queues for MCP traffic.
5. A cancelled MCP query stops only that query; it never stops user-owned proxy, send or robot tasks.

## Lifecycle

1. WPE starts normally and creates one per-process local pipe.
2. WPE publishes a current-user discovery record with an unguessable, per-run pipe name.
3. The connector requires exactly one running WPE instance and opens its pipe under the same Windows user.
5. WPE validates every operation and returns a detached result.
6. On WPE shutdown the gateway closes the pipe and removes its discovery record. The connector reports WPE offline and exits.

## Deliberate non-goals for phase 1

- Streamable HTTP and OAuth.
- Direct database integration.
- Subscriptions to high-rate packet streams.
- Remote access.
- Any mutating or privileged operation.

## Phase 2

The write-operation safety contract is in [phase-2-write-safety.md](phase-2-write-safety.md). It is intentionally separate from the phase-1 protocol: no write tool is registered until WPE-side approval, idempotency and audit handling are implemented.
