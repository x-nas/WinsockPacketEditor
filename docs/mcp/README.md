# WPE x64 MCP 改造

本目录记录 MCP 的正式契约、架构和发布验证入口。对外的工具名、权限和数据边界不应仅存在于代码注释中。

## 已确认的产品边界

- MCP 是本地 `stdio` 服务；WPE 主程序必须已运行。连接器绝不启动 WPE、代理、远程管理或注入。
- MCP 连接器是普通权限的 .NET 10 控制台程序 `WPEMcpServer.exe`；它通过当前用户专属 Named Pipe 请求正在运行的 WPE。
- WPE 是唯一的业务与运行状态真源。连接器不得直接读取 SQLite、不得引用 WPEHook、不得操作 WPE 的内部列表。
- MCP 调用者就是当前 Windows 用户下的 WPE 操作者。工具在各自的数据范围内返回完整数据：封包读取包含完整当前/原始 payload，账号读取包含解密后的密码，代理配置读取包含外部代理凭据，写入审计保留原始参数和结果。分页、工具职责以及 Named Pipe 的协议分配上限仍然适用。
- WPE 设置中的 MCP 总开关默认开启；关闭后删除实例发现文件，也不接受 MCP Pipe 请求。全局“需要人工确认”开关默认开启；关闭后，所有经过 MCP 风险守门的操作都会按自动执行处理。
- 已开放的写操作包括筛选器、代理账号、代理设置、发送任务、机器人、仓库、自动入库、单包编辑、已有注入流程和已有驱动管理流程；默认由 WPE 本机确认，也可由全局确认开关统一关闭确认。任意路径文件/剪贴板访问与静默自动注入仍不提供。

## 文档

- [架构](architecture.md)：组件边界、线程模型、生命周期和安全边界。
- [内部管道协议](internal-protocol.md)：WPE 与连接器之间的版本化报文。
- [工具表面](tools.md)：工具、资源、权限及敏感数据约束。
- [只读工具 Schema](schemas/readonly-tools.schema.json)：只读工具输入/输出 JSON Schema。
- [阶段 2 写入安全](phase-2-write-safety.md)：确认、幂等、审计和开放顺序。
- [写工具 Schema](schemas/write-tools.schema.json)：已开放写工具的输入/输出 JSON Schema。
- 稳定性、安全边界和自动化契约检查以仓库脚本 `tools/tests/McpContract.ps1` 为入口；真实 stdio 工具发现由 `tools/tests/McpToolsList.ps1` 验证。
- 发布前总检查入口：`tools/tests/McpReleaseCheck.ps1`；加 `-RequireWpe` 时要求当前 WPE 正在运行并完成 Pipe 生命周期检查。

## 当前发布状态

- WPE 本地 Named Pipe 网关与 stdio MCP Sidecar 已完成；当前注册 **123** 个工具。
- 打包会把自包含单文件 `WPEMcpServer.exe` 作为 `McpServer/` 载荷发布；启动器每次启动同步它到固定路径 `C:\WPE64DB\McpServer\WPEMcpServer.exe`。状态栏使用灰灯/黄灯/绿灯分别表示关闭、需要确认和可自动执行。
- Phase 6 已接入 `wpe_executors_stop_all` 和 `wpe_start_mode_select`：前者只停止现有发送器/机器人执行器，后者只在启动页选择代理或注入页面；两者均不主动发包、不自动注入。真实发布包已验证停止工具在无任务、原生运行态机器人和仅本机回环发送器的停止、UUID 校验和幂等；启动模式已验证首次选择、幂等重放及离开启动页后的新请求拒绝。
- MCP 设置已移至启动页，关闭总开关后仍可从启动页重新开启；多开设置也统一为启动页弹窗，并采用与其他设置页一致的分区布局。
- 代理、账号、滤镜、抓包/分析、发送、机器人、仓库、自动入库、单包编辑、进程代理和注入均只经现有 WPE 原生入口实现；实际 `pack` 产物覆盖 Sidecar 工具发现、生命周期、状态和分析回归。
- Named Pipe 的客户端提前断开属于正常会话结束，不写入 WPE 系统错误日志。后续实际验证必须运行 `pack` 生成的 `dist/WPE64 2.3.exe`。
- `wpe_start_mode_select` 只在启动页有效：成功会切入代理/注入界面，不会启动代理监听或执行注入；重复选择当前模式返回 `alreadySelected`，另一模式返回 `unavailable`。
- MCP 设置页按代理、注入、封包、滤镜、发送、机器人、仓库、设置、其它分组展示当前工具；日志页的 `MCP 日志` 仅记录工具调用结果、写入审计结论和工具错误，模块列使用 `wpe_*` 工具名。`wpe_logs_list` 支持 `kind: "mcp"`；`wpe_logs_all_list` 则按时间合并返回四类日志。
- 滤镜规则保存与启用是刻意分离的：`wpe_filter_rule_save` 不改变启用状态；需要启用/停用时必须调用 `wpe_filter_set_enabled`，`wpe_filter_get` 返回实际 `enabled` 状态供验证。
- 机器人循环必须使用 `wpe_robot_instructions_save` 原子提交完整、顺序化指令列表；逐条 `wpe_robot_instruction_add` 会在每次保存时触发原生循环配对校验。数字行 `1` 的键盘指令内容为 `Press|D1`，不是 `1` 或虚拟键码数值。

外部连接器以官方 MCP C# SDK 的稳定 API 实现，目标为 MCP 2026-07-28，同时保留 SDK 提供的旧协议协商能力。WPE 内部 Named Pipe 协议独立从 v1 开始，绝不复用注入 IPC v4。
