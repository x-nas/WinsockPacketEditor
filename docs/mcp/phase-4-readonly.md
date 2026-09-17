# Phase 4：只读诊断增强

阶段 4 继续保持只读，目标是让 MCP 能判断 WPE 当前是否适合继续工作，而不是代替用户执行高风险操作。

## 已实现工具

| 工具 | 用途 | 安全边界 |
|---|---|---|
| `wpe_proxy_health_get` | 汇总代理是否运行、监听配置是否自洽、连接统计是否可读 | 不启动/停止代理，不修复配置 |
| `wpe_executors_detail_get` | 返回发送器和机器人执行器的数量、运行状态摘要 | 不启动/停止执行器，不返回任务内容 |
| `wpe_storage_health_get` | 检查配置库版本、当前模式和数据库路径是否可用 | 不修改数据库 |

## 实施记录

1. `wpe_proxy_health_get`：复用现有运行态和设置读取逻辑，发现端口冲突、启用项与运行态不一致。
2. `wpe_executors_detail_get`：只返回数量、运行/停止计数和采样时间。
3. `wpe_storage_health_get`：返回数据库可用性摘要。

## 暂不开放

- 任何自动修复；
- 执行器启停；
- 主动发包、注入和驱动操作；
- 任意文件、剪贴板、驱动操作或自动注入。

## 实现状态

三个只读工具已完成代码和 Schema：

- `wpe_proxy_health_get`
- `wpe_executors_detail_get`
- `wpe_storage_health_get`

后续只需按发布前清单做回归烟测，不开放任何新的写操作。

## 集成烟测结果

- `wpe_proxy_health_get`：`healthy=true`，代理未运行时仍能正确报告端口无冲突、会话数为 0；
- `wpe_executors_detail_get`：发送器和机器人运行数均为 0；
- `wpe_storage_health_get`：数据目录和 `2.3 Beta.db` 均存在，版本返回 `2.3 Beta`；
- 三个工具均未返回密码、令牌、完整路径、任务内容或封包正文。
