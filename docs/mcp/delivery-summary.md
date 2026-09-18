# MCP 阶段 2–4 交付摘要

## 阶段 2：受确认保护的写操作

- 筛选器、账号、代理认证、HTTP/SOCKS5 端口、连接上限、监听地址；
- 防火墙开关与规则增删；
- Only-WPC、外部代理开关；
- 代理启动/停止；
- 所有写操作均使用幂等键、WPE 本地确认、过期保护和完整审计。

## 阶段 3：只读运行诊断

- 代理运行态；
- TCP/UDP 与 WPC/普通连接汇总；
- 已完成普通 TCP、UDP、WPC 控制连接和断开清理测试。

## 阶段 4：只读健康诊断（未发布）

- `wpe_proxy_health_get`、`wpe_executors_detail_get` 与 `wpe_storage_health_get` 曾有 Schema 和文档草案，但从未注册到 Sidecar；现已移除。
- 同类的 `wpe_proxy_capabilities_get` 与 `wpe_proxy_failures_list` 也未发布。
- 五项均需先有 WPE 原生功能，才可重新评估，详见 [tool-audit.md](tool-audit.md)。

## 当前发布形态与可观测性

- 当前 MCP 工具数：**113**。
- `WPEMcpServer.exe` 是自包含单文件；WPE 启动器同步到 `C:\WPE64DB\McpServer\WPEMcpServer.exe`，使 VS Code 等客户端可以使用固定配置路径。
- 日志页新增独立 `MCP 日志`：只记录工具调用结果、写入审计结论和工具错误；调用 `wpe_logs_list` 时使用 `kind: "mcp"` 可读取同一数据。
- 启动页模式选择与代理监听分离：`wpe_start_mode_select` 只进入页面，`wpe_proxy_start` 才绑定代理监听端口。
- 滤镜规则更新不包含启停；启停一律通过 `wpe_filter_set_enabled`，并由 `wpe_filter_get.enabled` 验证。
- 机器人完整指令保存使用 `wpe_robot_instructions_save`：它先在编辑快照中替换全部指令、再统一调用原生校验和保存，因而能安全写入循环开始/结束配对；单条插入工具保留给不会临时破坏该配对的编辑。

## 明确未开放

任意路径文件或剪贴板访问，以及绕过 WPE 原生交互的静默自动注入。
