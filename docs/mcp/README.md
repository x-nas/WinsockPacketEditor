# WPE x64 MCP 改造

本目录记录 `feature-mcp-server` 分支上的 MCP 契约、实施状态和交接信息。它们是实现和测试的依据；对外的工具名、权限和数据边界不应仅存在于代码注释中。

## 已确认的产品边界

- MCP 是本地 `stdio` 服务；WPE 主程序必须已运行。连接器绝不启动 WPE、代理、远程管理或注入。
- MCP 连接器是普通权限的 .NET 10 控制台程序；它通过当前用户专属 Named Pipe 请求正在运行的 WPE。
- WPE 是唯一的业务与运行状态真源。连接器不得直接读取 SQLite、不得引用 WPEHook、不得操作 WPE 的内部列表。
- 默认不返回封包完整内容。调用方必须显式选择 `includePayload`，且受权限与大小限制。
- WPE 设置中的 MCP 总开关默认开启；关闭后不发布实例发现记录，也不接受 MCP Pipe 请求。自动写入开关默认关闭。
- 已开放的写操作仅限于可逆的筛选器/代理账号启停和精确防火墙规则增删；每次都由 WPE 本机确认。密码读取、任意文件/剪贴板访问、驱动卸载、注入和主动发包仍不提供。

## 文档

- [架构](architecture.md)：组件边界、线程模型、生命周期和安全边界。
- [内部管道协议](internal-protocol.md)：WPE 与连接器之间的版本化报文。
- [工具表面](tools.md)：工具、资源、权限及敏感数据约束。
- [只读工具 Schema](schemas/readonly-tools.schema.json)：只读工具输入/输出 JSON Schema。
- [阶段 2 写入安全](phase-2-write-safety.md)：确认、幂等、审计和开放顺序。
- [写工具 Schema](schemas/write-tools.schema.json)：已开放写工具的输入/输出 JSON Schema。
- [交接记忆](handoff.md)：当前完成项、验证证据和下一步工作。
- [阶段 3 只读诊断](phase-3-readonly.md)：下一阶段的工具边界和实施顺序。
- [阶段 4 只读增强](phase-4-readonly.md)：代理健康、执行器状态和存储健康的设计边界。
- [阶段 2–4 交付摘要](delivery-summary.md)：已完成能力、测试覆盖和明确未开放范围。
- 阶段 5：稳定性、安全边界和自动化契约检查以仓库脚本 `tools/tests/McpContract.ps1` 为入口，逐步补充集成回归。

## 当前实施状态（2026-09-16）

- Phase 0 已完成：架构、内部协议、工具表面和 Schema 已落地。
- Phase 1 已完成：18 个只读工具、WPE 本地 Named Pipe 网关和 stdio MCP Sidecar 已完成实际端到端验证。
- Phase 2 已完成：代理配置、外部代理开关和代理启停均已实现并完成验证；停止代理会断开现有连接，符合预期。数据包发送、注入和执行器启停仍未开放。
- Phase 3 第一批只读诊断已完成：运行态、连接汇总和安全失败摘要均已实时烟测通过。
- 当前发布包位于仓库 `dist/WPE64 2.3.exe`，打包已确认包含 `McpServer/` Sidecar。状态栏使用灰灯/黄灯/绿灯分别表示关闭、需要确认和可自动执行。

## 兼容性

外部连接器以官方 MCP C# SDK 的稳定 API 实现，目标为 MCP 2026-07-28，同时保留 SDK 提供的旧协议协商能力。WPE 内部 Named Pipe 协议独立从 v1 开始，绝不复用注入 IPC v4。
