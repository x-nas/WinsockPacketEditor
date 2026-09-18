# Phase 4：只读诊断增强（未发布）

本阶段曾规划下列 MCP 专属诊断工具，但它们没有进入 Sidecar 注册表，也不属于当前 103 个已发布工具：

- `wpe_proxy_health_get`
- `wpe_executors_detail_get`
- `wpe_storage_health_get`

三项功能都不是 WPE 当前已有的同口径原生页面或业务入口，因而不符合“只接口化既有 WPE 能力”的工具筛选准则。相应 Schema 已移除；若未来先在 WPE 增加原生诊断页面，可重新设计并实现 MCP 契约。候选及重新纳入条件见 [tool-audit.md](tool-audit.md)。

当前替代能力为 `wpe_proxy_runtime_get`、`wpe_connections_summary_get` 和 `wpe_executors_list`；它们均直接映射既有运行态或列表。
