# MCP 代理模块

本文档是 WPE 代理模块 MCP 工具的实施清单。现有工具保持兼容，后续新增工具按本模块命名和权限约定接入。

## 已接入工具

### 只读

- `wpe_proxy_settings_get`：代理配置、限制和外部代理凭据。
- `wpe_proxy_config_get`：完整代理配置快照（含外部代理凭据）。
- `wpe_proxy_runtime_get`：监听器、运行状态和连接计数。
- `wpe_connections_list`：分页连接元数据。
- `wpe_connections_summary_get`：按协议及 WPC/普通连接汇总。
- `wpe_accounts_list`：账号元数据（含解密后的密码）。

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

所有写入均使用 WPE 本地确认、UUID 幂等键、业务校验和完整审计。

### 生命周期

- `wpe_proxy_start`
- `wpe_proxy_stop`

启动和停止可能绑定端口或断开现有连接，属于高风险操作，必须经过确认。

## 后续候选工具

`wpe_proxy_config_get` 与 `wpe_proxy_settings_get` 均已完成。`wpe_proxy_capabilities_get`、`wpe_proxy_health_get`、`wpe_proxy_failures_list` 和 `wpe_storage_health_get` 是未发布候选，不得写入 Schema 或 `tools/list`；重新纳入前需先补齐 WPE 原生能力，见 [tool-audit.md](tool-audit.md)。连接工具继续只读，不新增 WPE 当前没有的按连接断开等控制能力。

### 账号与设备

- `wpe_account_get`
- `wpe_account_create`
- `wpe_account_update`
- `wpe_account_delete`
- `wpe_account_devices_list`

`wpe_account_get` 与 `wpe_accounts_list` 返回账号的解密后密码；写入审计保留原始参数和结果，供当前 Windows 用户下的 WPE 操作者复核。

## 实施顺序

1. 完成账号单项详情和设备列表。
2. 实现账号 CRUD，逐项验证持久化、完整审计和回滚。
3. 盘点代理页面剩余已有功能，逐项接口化；不创建 WPE 当前没有的连接/设备强制控制。

## 范围约束

本模块只把 WPE 已有的设置、列表、编辑器、启动/停止和执行器入口接口化。MCP 不新增按连接断开、自动修复、远程强制控制等能力；这类能力若未来需要，必须先作为 WPE 原生功能设计和测试，再接入 MCP。

## 已解除的打包版本偏差

筛选器 CRUD 曾因旧启动器 payload 与新 Sidecar 版本不一致而无法由网关分派。现在已通过完整 `pack` 包实际验证 `create -> get -> update -> get -> delete`，测试临时数据会清理。后续 MCP 回归一律以 `pack` 生成的 WPE 进程为目标。

## 统一返回约定

- 配置读取返回 `schemaVersion`、`effective` 和 `requiresRestart`。
- 写入返回 `changed`、修改后的字段和 `outcome`；审计保留完整请求与结果。
- 健康检查返回 `healthy`、`checks[]` 和 `observedAt`，检查失败不自动修复。
- 所有列表使用 1..200 的 `limit` 和 opaque `nextCursor`。
