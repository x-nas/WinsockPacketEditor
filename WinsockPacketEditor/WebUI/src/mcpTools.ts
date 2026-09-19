/*
  MCP 工具清单的唯一来源：从 Sidecar 的注册特性里抽出来，给启动页的「N 个工具可用」
  与 MCP 设置页的工具列表共用 —— 两处各写一份正则会各自漏工具（2026-09-19 修过一次：
  旧的 Description\("([^"]*)"\) 停在转义引号上，带 \" 描述的那条工具两个界面都漏掉了）。
  这里的字符类放行转义，再把 \" 还原成显示用的引号。
*/
import toolSource from '../../../WPEMcpServer/WpeTools.cs?raw'

export interface McpTool {
  name: string
  description: string
}

export const mcpTools: McpTool[] = Array.from(
  toolSource.matchAll(/\[McpServerTool\(Name = "([^"]+)"\), Description\("((?:[^"\\]|\\.)*)"\)\]/g),
  (match) => ({ name: match[1], description: match[2].replace(/\\(["\\])/g, '$1') }),
)

export const mcpToolCount = mcpTools.length
