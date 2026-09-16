# WPE MCP 改造交接记忆

更新时间：2026-09-16。工作分支：`feature-mcp-server`；基线为 `Develop`。不得切换、合并、重置或修改 `Develop`、`master`。

## 已完成

- Phase 0：`architecture.md`、`internal-protocol.md`、工具表和 JSON Schema。
- Phase 1：WPE 内的 `McpAgentGateway`、.NET 10 `WPEMcpServer` stdio Sidecar、打包集成；18 个只读 MCP 工具已实际端到端验证。
- Phase 2：共享 `McpWriteGuard`（UUID 幂等、同键异参拒绝、60 秒本机确认超时、脱敏内存审计）；已开放 `wpe_filter_set_enabled`、`wpe_firewall_rules_list`、`wpe_firewall_rule_add`、`wpe_firewall_rule_remove`。
- Phase 2 追加开放 `wpe_account_set_enabled`：只修改已有代理账号启用状态，不读取或修改密码；已完成批准停用、批准恢复、幂等、同键异参拒绝和最终状态核对。
- Phase 2 追加开放 `wpe_proxy_auth_set_enabled`：只修改代理身份认证布尔值，关闭时校验 `Only_WPC_Client` 约束；已完成批准停用、批准恢复、幂等、同键异参拒绝，以及重启后返回 `changed=false, enabled=true` 的持久化核对。
- Phase 2 追加开放 `wpe_proxy_http_set_enabled`：只修改 HTTP 代理启用布尔值，开启时校验 SOCKS5 已启用、端口范围和端口冲突；已完成批准停用、批准恢复、幂等、同键异参拒绝，以及重启后返回 `changed=false, enabled=true, port=1081` 的持久化核对。
- Phase 2 追加开放 `wpe_proxy_max_connections_set`：只修改最大连接数，确认前后均按实时 `MaxConnectionCap()` 校验；已用默认值 `5000` 完成重启后 `changed=false` 核对、重复幂等键和同键异参拒绝验证，实时上限为 `98304`。
- Phase 2 追加开放 `wpe_proxy_socks5_port_set`：只修改 SOCKS5 端口，校验范围并避免与已启用的 HTTP 端口冲突；已用默认端口 `1080` 完成重启后 `changed=false` 核对、重复幂等键和同键异参拒绝验证。
- Phase 2 追加开放 `wpe_proxy_http_port_set`：只修改 HTTP 端口，要求 HTTP 已启用并校验范围和 SOCKS5 冲突；已用端口 `1081` 完成重启后 `changed=false` 核对、重复幂等键和同键异参拒绝验证。
- Phase 2 追加开放 `wpe_firewall_set_enabled`：只修改防火墙总开关并持久化；已完成从原始关闭状态开启、关闭恢复和最终只读核对，最终保持 `enabled=false`。
- Phase 2 追加开放 `wpe_proxy_only_wpc_set_enabled`：只修改 Only-WPC 布尔值，启用时要求身份认证已开启；已完成批准启用、批准关闭恢复、重复幂等键和同键异参拒绝验证，最终保持 `enabled=false`。
- Phase 2 追加开放 `wpe_proxy_bind_ip_set`：支持自动检测或明确 IPv4/IPv6，明确地址在确认前后均校验；已完成自动模式 `changed=false` 核对，以及非法地址在确认前拒绝验证，最终保持自动检测。
- 筛选器写入已在真实打包 WPE 上验证：确认后返回 `approved`；请求目标已处于指定状态时准确返回 `changed=false`。确认框展示筛选器名称，不展示 GUID。
- 防火墙规则读取已在真实打包 WPE 上验证；当白/黑名单为空时返回空页。对不存在规则的删除在弹确认框前拒绝，未改变任何配置。
- 防火墙新增已完成代码、Schema、Sidecar 注册和打包验证。`SaveIPRuleAsync` 会等待归属地查询和实际插入，随后保存对应名单表；已在本次新包上完成拒绝无变更、批准新增、重复幂等键不重复新增、同键异参拒绝、重启后持久化，以及批准删除清理的实际 E2E 验证。

## 当前 MCP 设置

- `McpEnabled` 默认开启，保存在 `SystemConfig`；关闭后删除本机发现文件并停止接受 MCP Pipe 请求。
- MCP 全局“需要人工确认”默认开启，数据库字段为 `McpRequiresConfirmation`；关闭后，所有经过 `McpWriteGuard` 的风险操作都会跳过本机确认并直接执行。幂等、校验、脱敏审计和业务层约束始终保留。
- 状态栏：灰灯 = 已关闭，黄灯 = 需要确认，绿灯 = 自动执行；代理地址与状态使用 `//`、`-` 分组。

## 阶段 5 进行中

- 已补齐代码工具、`tools.md` 和只读/写入 Schema 的同步检查入口：`tools/tests/McpContract.ps1`。
- 当前阶段只做稳定性、安全边界和自动化回归，不新增主动发包、注入、驱动或执行器控制。
- 已新增 `tools/tests/McpLifecycle.ps1`：在 WPE 已启动时验证发现文件、进程存活、Named Pipe 可连接和 `runtime.status`；使用 `-ExpectDisabled` 验证关闭总开关后发现文件不存在。
- 已新增 `tools/tests/McpToolsList.ps1`：通过真实 stdio MCP `initialize` / `tools/list` 验证 Sidecar 启动、工具数量、名称格式和重复注册。
- 已完成真实生命周期回归：MCP 开启时 PID `2260` 的 Pipe 调用通过；关闭后发现文件删除检查通过；重新开启并重启 WPE 后 PID 变为 `1344`，发现文件、Pipe 和 `runtime.status` 均通过。
- 已修正并验证 C#/前端 `UiIcon` 映射：自动执行绿色、人工确认黄色、关闭红色；契约检查现在会锁定 `None=0, Info=1, Success=2, Warn=3, Error=4`。
- 阶段 5 发布门禁已通过：`McpReleaseCheck.ps1 -RequireWpe` 完成 35 个工具契约、Sidecar `tools/list` 和真实 WPE PID `4520` 的发现文件、Named Pipe、`runtime.status` 验证。
- 网关启动时会清理发现文件中已经不存在的进程记录，避免 WPE 异常退出后残留 PID 导致 Sidecar 误判多实例。
- 下一步：将生命周期脚本纳入发布前清单，并在测试机上补做一次强制退出后的恢复验证。

## 阶段 6 起点

- 阶段 5 发布门禁已通过后，开始设计高风险操作。
- 首个候选为 `wpe_executors_stop_all`，只做紧急停止，不启动执行器、不主动发包，并遵循 MCP 全局确认开关。
- 设计和测试边界见 `phase-6-high-risk.md`；在实现和真实测试完成前，不注册该工具。
- `wpe_executors_stop_all` 已完成首版接入，待真实 WPE 上执行确认、拒绝、幂等和空任务回归。

## 当前结构与约束

- WPE 启动后创建随机 Named Pipe 名称，并在 `%LOCALAPPDATA%\WPE64\mcp\instances.json` 发现文件发布当前实例。Sidecar 只支持一个运行实例。
- 外部 MCP 仅通过 `WPEMcpServer` 的 stdio；Sidecar 不读 SQLite、不引用 WPEHook。WPE 内部 MCP Pipe 协议 v1 与注入 IPC v4 完全独立。
- 所有业务读写都必须经 `Operate.SystemConfig.InvokeAction` 进入 UI 线程。写操作必须通过 `McpWriteGuard`，并复用已有 `Operate` 业务入口；禁止 MCP 直接写 SQLite 或直接修改列表。
- 当前 Pipe 对 World SID 授权以跨 UAC 完整性级别通信；安全性依赖随机、每次运行变化且只在当前用户 LocalAppData 发现文件中公布的 Pipe 名称。不要改回固定 Pipe 名称。
- 保持 stdio 干净：`Program.cs` 已清空日志提供程序，不能输出日志到 stdout。

## 验证命令

```powershell
dotnet build WPEMcpServer/WPEMcpServer.csproj --no-restore -c Release -v:minimal
dotnet build WinsockPacketEditor/WinsockPacketEditor.csproj --no-restore -c Release -v:minimal
powershell -ExecutionPolicy Bypass -File tools/CheckUiCoupling.ps1
powershell -ExecutionPolicy Bypass -File tools/pack/Pack.ps1 -SkipBuild
```

主工程直接构建适用于 MCP 代码验证；完整发布仍应按项目约定使用解决方案构建以包含 `WPEHook`。打包脚本已发布 Sidecar 到 `WinsockPacketEditor/bin/Release/McpServer`，并在 Required 清单验证其 exe、dll、deps 和 runtimeconfig。

## 阶段 2 当前验证点

最新发布包为 `dist/WPE64 2.3.exe`；每次前端改动必须先构建 WebUI，再用解决方案构建同步 `bin/Release/wwwroot`，最后运行打包脚本。不要使用 `-SkipBuild` 代替前端同步，除非已先完成这两步。

阶段 2 防火墙写入 E2E 已完成：测试地址 `203.0.113.77` 经过拒绝、批准、幂等、同键异参、重启持久化和删除清理全流程，最终黑名单为空。

账号启停 E2E 已完成：唯一测试账号 `111` 经过批准停用、状态核对、批准恢复、最终状态核对、重复幂等键和同键异参流程，最终保持启用；密码始终未返回、未进入请求摘要或审计结果。

下一步仍需谨慎选择代理配置字段并逐项验证；代理启停、执行器控制、主动发包和注入控制仍属于更高风险层，不能提前开放。
