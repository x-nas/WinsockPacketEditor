# Phase 3：只读运行诊断

阶段 3 先扩展只读能力，不增加新的控制权限。所有工具只从 WPE 当前运行状态读取数据，不写 SQLite、不启动或停止服务。

## 第一批工具

| 工具 | 目的 | 数据边界 |
|---|---|---|
| `wpe_proxy_runtime_get` | 查看代理运行态、监听配置、会话数和外部代理状态 | 返回该运行态工具的完整字段 |
| `wpe_connections_summary_get` | 按协议和设备类型汇总连接 | 返回该汇总工具的完整计数字段 |

## 实施顺序

1. 先实现 `wpe_proxy_runtime_get`，复用现有 `wpe_status_get` 与 `wpe_proxy_settings_get` 的数据边界。
2. 实现连接汇总，并用空列表、WPC 控制连接和普通连接分别验证。

## 暂不开放

- 代理启停和配置修改以外的自动化控制；
- 执行器启动/停止；
- 主动发包、注入、驱动操作；
- 任意文件、剪贴板、驱动操作和自动注入。

## 当前状态

第一批已完成并通过实时烟测：

- `wpe_proxy_runtime_get`：返回运行态、端口、连接数和抓包计数；
- `wpe_connections_summary_get`：空连接状态返回各类计数 0；

`wpe_proxy_failures_list` 未进入 Sidecar 注册表。它是 MCP 专用的日志筛选视图；在 WPE 提供同口径的原生页面前，保持为不公开候选，参见 [tool-audit.md](tool-audit.md)。

## 真实连接场景验证

- 普通 TCP 短连接：连接保持期间返回 `total=1, tcp=1, ordinary=1`，关闭后全部恢复为 0；
- WPEProxyCap 控制连接：返回 `wpcControl=1`；
- UDP 关联：返回 `udp=1`；
- 联合场景：返回 `total=2, tcp=1, udp=1, wpcControl=1, ordinary=1`；
- WPEProxyCap 断开后：所有计数恢复为 0。
