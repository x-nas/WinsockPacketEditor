# MCP 代理模块

本文档是 WPE 代理模块 MCP 工具的实施清单。现有工具保持兼容，后续新增工具按本模块命名和权限约定接入。

## 已接入工具

### 只读

- `wpe_proxy_settings_get`：非敏感代理配置和限制。
- `wpe_proxy_config_get`：完整非敏感代理配置快照。
- `wpe_proxy_capabilities_get`：当前构建支持的代理能力。
- `wpe_proxy_runtime_get`：监听器、运行状态和连接计数。
- `wpe_connections_list`：分页连接元数据，不返回凭据。
- `wpe_connections_summary_get`：按协议及 WPC/普通连接汇总。
- `wpe_proxy_failures_list`：脱敏代理失败摘要。
- `wpe_proxy_health_get`：只读一致性诊断，不自动修复。
- `wpe_storage_health_get`：数据库可用性元数据。
- `wpe_accounts_list`：账号元数据，不返回密码和令牌。

### 配置写入

- `wpe_proxy_auth_set_enabled`
- `wpe_proxy_http_set_enabled`
- `wpe_proxy_max_connections_set`
- `wpe_proxy_socks5_port_set`
- `wpe_proxy_http_port_set`
- `wpe_proxy_bind_ip_set`
- `wpe_proxy_only_wpc_set_enabled`
- `wpe_external_proxy_set_enabled`
- `wpe_account_set_enabled`

所有写入均使用 WPE 本地确认、UUID 幂等键、业务校验和脱敏审计。

### 生命周期

- `wpe_proxy_start`
- `wpe_proxy_stop`

启动和停止可能绑定端口或断开现有连接，属于高风险操作，必须经过确认。

## 后续候选工具

`wpe_proxy_config_get`、`wpe_proxy_capabilities_get` 和结构化的 `wpe_proxy_health_get` 已完成；`wpe_proxy_settings_get` 保持兼容。连接工具继续只读，不新增 WPE 当前没有的按连接断开等控制能力。

### 账号与设备

- `wpe_account_get`
- `wpe_account_create`
- `wpe_account_update`
- `wpe_account_delete`
- `wpe_account_devices_list`

账号密码不提供读取工具。设置密码必须使用不可回显、不可进入审计摘要的专用输入流程。

## 实施顺序

1. 完成账号单项详情和设备列表。
2. 实现账号 CRUD，逐项验证持久化、脱敏和回滚。
3. 盘点代理页面剩余已有功能，逐项接口化；不创建 WPE 当前没有的连接/设备强制控制。

## 范围约束

本模块只把 WPE 已有的设置、列表、编辑器、启动/停止和执行器入口接口化。MCP 不新增按连接断开、自动修复、远程强制控制等能力；这类能力若未来需要，必须先作为 WPE 原生功能设计和测试，再接入 MCP。

## 已解除的打包版本偏差

筛选器 CRUD 曾因旧启动器 payload 与新 Sidecar 版本不一致而无法由网关分派。现在已通过完整 `pack` 包实际验证 `create -> get -> update -> get -> delete`，测试临时数据会清理。后续 MCP 回归一律以 `pack` 生成的 WPE 进程为目标。

## 统一返回约定

- 配置读取返回 `schemaVersion`、`effective` 和 `requiresRestart`。
- 写入返回 `changed`、修改后的安全字段和 `outcome`，不返回密码、令牌或完整连接地址。
- 健康检查返回 `healthy`、`checks[]` 和 `observedAt`，检查失败不自动修复。
- 所有列表使用 1..200 的 `limit` 和 opaque `nextCursor`。
