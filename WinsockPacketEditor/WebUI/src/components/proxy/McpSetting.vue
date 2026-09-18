<script setup lang="ts">
import { ref, watch } from 'vue'
import { call } from '../../bridge'
import { lang } from '../../i18n'
import SettingsModal from './SettingsModal.vue'
import toolSource from '../../../../../WPEMcpServer/WpeTools.cs?raw'

interface McpTool { name: string; description: string }

// 直接从 MCP Server 的注册特性生成，避免设置页和实际 tools/list 出现两份清单。
const tools: McpTool[] = Array.from(toolSource.matchAll(/\[McpServerTool\(Name = "([^"]+)"\), Description\("([^"]*)"\)\]/g) as Iterable<RegExpMatchArray>, match => ({
  name: match[1],
  description: match[2],
}))

function isWriteTool(name: string): boolean {
  return /(?:_set(?:_|$)|_create$|_update$|_delete$|_clear(?:_|$)|_move$|_copy$|_add(?:_|$)|_save$|_start$|_stop$|_action$|_command$|_reset$|_select$)/.test(name)
}

function zhDescription(name: string): string {
  const area: Array<[RegExp, string]> = [
    [/^wpe_status/, ' WPE 当前运行状态'], [/^wpe_capture/, '已捕获封包'], [/^wpe_packet_edit/, '已捕获封包的编辑内容'],
    [/^wpe_proxy_capture/, '代理模式已捕获封包'], [/^wpe_logs/, ' WPE 系统日志'], [/^wpe_filter/, '筛选器'],
    [/^wpe_account/, '代理账号'], [/^wpe_proxy_(auth|http|bind|external|only|max|socks5|runtime|settings|config|start|stop)/, '代理服务设置'],
    [/^wpe_firewall/, '防火墙规则与设置'], [/^wpe_connection/, '当前代理连接'], [/^wpe_executor/, '发送器与机器人执行器'],
    [/^wpe_bytes/, '调用方提供的字节数据'], [/^wpe_send_collection/, '发送任务中的封包集合'], [/^wpe_send/, '发送任务'],
    [/^wpe_robot_instruction/, '机器人指令'], [/^wpe_robot/, '机器人任务'], [/^wpe_auto_stores/, '自动入库规则'],
    [/^wpe_warehouse_stores/, '仓库中的封包'], [/^wpe_warehouse/, '封包仓库'], [/^wpe_tasks?/, '发送、机器人或仓库任务'],
    [/^wpe_start_mode/, ' WPE 启动模式'],
  ]
  const target = area.find(([pattern]) => pattern.test(name))?.[1] || ' WPE 数据'
  if (/_search$|_find_next$/.test(name)) return `搜索${target}，返回匹配结果和必要的定位信息。`
  if (/_list$/.test(name)) return `查看${target}列表，不修改现有配置。`
  if (/_get$/.test(name)) return `查看指定${target}的详细信息。`
  if (/_set_enabled$/.test(name)) return `启用或停用${target}；是否需要本地确认取决于当前设置。`
  if (/_set$/.test(name)) return `修改${target}的设置；是否需要本地确认取决于当前设置。`
  if (/_create$/.test(name)) return `新建${target}；是否需要本地确认取决于当前设置。`
  if (/_update$|_save$/.test(name)) return `更新${target}；是否需要本地确认取决于当前设置。`
  if (/_delete$|_remove$/.test(name)) return `删除${target}；是否需要本地确认取决于当前设置。`
  if (/_clear/.test(name)) return `清空${target}；是否需要本地确认取决于当前设置。`
  if (/_move$/.test(name)) return `调整${target}的顺序；是否需要本地确认取决于当前设置。`
  if (/_copy$/.test(name)) return `复制${target}；是否需要本地确认取决于当前设置。`
  if (/_add_to_send$/.test(name)) return `将${target}复制到指定发送任务，不会自动开始发送。`
  if (/_add_to_warehouse$/.test(name)) return `将${target}复制到指定封包仓库。`
  if (/_add$/.test(name)) return `向${target}添加一项内容；是否需要本地确认取决于当前设置。`
  if (/_action$|_command$/.test(name)) return `执行${target}的原生操作；是否需要本地确认取决于当前设置。`
  if (/_start$/.test(name)) return `启动${target}；是否需要本地确认取决于当前设置。`
  if (/_stop$/.test(name)) return `停止${target}；是否需要本地确认取决于当前设置。`
  if (/_reset$/.test(name)) return `重置${target}的运行期数据；是否需要本地确认取决于当前设置。`
  return `处理${target}相关功能。`
}

/**
 * MCP 的 Description 是给跨客户端 AI 使用的稳定英文接口；此处是给 WPE 人类界面的本地化摘要。
 * 工具名仍原样显示，便于与 VS Code / Claude Code 等客户端中的实际工具一一对应。
 */
function localizedDescription(tool: McpTool): string {
  if (lang.value === 'en') return tool.description
  const write = isWriteTool(tool.name)
  const action = write
  switch (lang.value) {
    case 'zh': return zhDescription(tool.name)
    case 'tw': return write ? `執行 WPE 的「${tool.name}」寫入操作；是否顯示本機確認取決於目前設定。` : `讀取 WPE 中与 「${tool.name}」 相關的資訊，不會修改現有設定。`
    case 'ja': return write ? `WPE の「${tool.name}」書き込み操作を実行します。ローカル確認の有無は現在の設定に従います。` : `WPE の「${tool.name}」に関する情報を読み取ります。既存の設定は変更しません。`
    case 'ko': return write ? `WPE의 “${tool.name}” 쓰기 작업을 실행합니다. 로컬 확인 여부는 현재 설정을 따릅니다.` : `WPE의 “${tool.name}” 관련 정보를 읽습니다. 기존 설정은 변경하지 않습니다.`
    case 'vi': return write ? `Thực hiện thao tác ghi “${tool.name}” của WPE; việc xác nhận cục bộ tùy theo cài đặt hiện tại.` : `Đọc thông tin liên quan đến “${tool.name}” trong WPE, không thay đổi cấu hình hiện có.`
    case 'ru': return write ? `Выполняет операцию записи WPE «${tool.name}»; локальное подтверждение зависит от текущей настройки.` : `Читает сведения WPE, связанные с «${tool.name}», не изменяя существующую конфигурацию.`
    default: return action ? tool.description : tool.description
  }
}

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', value: boolean): void }>()
const enabled = ref(true)
const requiresConfirmation = ref(false)
const busy = ref(false)
const error = ref('')

watch(() => props.open, async (open) => {
  if (!open) return
  error.value = ''
  try {
    const value = await call<{ enabled: boolean; requiresConfirmation: boolean }>('getMcpSettings')
    enabled.value = value.enabled
    requiresConfirmation.value = value.requiresConfirmation
  } catch (e) { error.value = String(e) }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    await call('saveMcpSettings', { enabled: enabled.value, requiresConfirmation: requiresConfirmation.value })
    emit('update:open', false)
  } catch (e) { error.value = String(e) } finally { busy.value = false }
}
</script>

<template>
  <SettingsModal :open="props.open" title="MCP 设置" subtitle="本机 AI 自动化权限" :busy="busy" :error="error" @update:open="emit('update:open', $event)" @save="save">
    <div class="setf mcp-set">
    <div class="swb">
      <div class="row">
        <label class="chk" :class="{ on: enabled }">
          <i />
          <input v-model="enabled" type="checkbox" hidden />
          <span>启用 MCP 服务</span>
        </label>
      </div>
      <p class="hint">关闭后，本机 AI 无法发现或调用 WPE 的 MCP 服务。</p>
    </div>
    <div class="swb">
      <div class="row">
        <label class="chk" :class="{ on: requiresConfirmation }">
          <i />
          <input v-model="requiresConfirmation" type="checkbox" hidden />
          <span>MCP 操作需要人工确认</span>
        </label>
      </div>
      <p class="hint">{{ requiresConfirmation ? '所有风险等级的 MCP 操作都需要 WPE 本地确认。' : '关闭确认后，MCP 操作将直接执行，请确保 AI 客户端和本机环境可信。' }}</p>
    </div>
    <section class="sec tools-sec">
      <div class="grp"><span>MCP 工具列表</span><b>{{ tools.length }}</b></div>
      <p class="hint">以下为当前 MCP Server 实际注册的全部工具。{{ requiresConfirmation ? '写入工具会请求 WPE 本地确认。' : '已关闭人工确认，写入工具将自动确认并直接执行。' }}</p>
      <div class="mcp-tools">
        <article v-for="tool in tools" :key="tool.name" class="mcp-tool">
          <code>{{ tool.name }}</code>
          <p>{{ localizedDescription(tool) }}</p>
        </article>
      </div>
    </section>
    </div>
  </SettingsModal>
</template>

<style scoped>
.mode-row { display:flex; justify-content:space-between; align-items:center; padding:8px 0; color:var(--tx-2); }
.ok { color:var(--green, #65d59b); }
.hint { margin:10px 0 0; color:var(--tx-3); font-size:var(--fs-small); line-height:1.5; }
.muted { opacity:.9; }
.mcp-set { display: block; width: 100%; padding: 8px 0 14px; color: var(--soft); }
.mcp-set .swb { display: block; width: 100%; }
.mcp-set .swb .row { display: flex; align-items: center; min-height: 34px; padding: 6px 20px; }
.mcp-set .swb .hint { padding: 0 20px; }
.mcp-set .sec { display: block; margin: 10px 20px 12px; padding-bottom: 8px; border: 1px solid rgb(var(--border-rgb) / 80%); background: rgb(var(--inset-rgb) / 16%); }
.mcp-set .sec .grp { display: flex; align-items: center; padding: 9px 14px 8px; border-bottom: 1px solid rgb(var(--border-rgb) / 60%); background: var(--panel); color: var(--soft); font-size: var(--fs-body); }
.mcp-set .sec .row { display: flex; align-items: center; justify-content: space-between; min-height: 32px; padding: 5px 14px; }
.mcp-set .sec .hint { padding: 0 14px; }
.mcp-set .tools-sec { padding-bottom: 0; }
.mcp-set .tools-sec .grp { justify-content: space-between; }
.mcp-set .tools-sec .grp b { padding: 2px 6px; border: 1px solid rgb(var(--cyan-rgb) / 45%); color: var(--cyan); font-family: var(--share); font-size: var(--fs-caption); line-height: 1; }
.mcp-tools { border-top: 1px solid rgb(var(--border-rgb) / 60%); background: rgb(var(--inset-rgb) / 16%); }
.mcp-tool { padding: 9px 14px 8px; border-bottom: 1px solid rgb(var(--border-rgb) / 45%); }
.mcp-tool:last-child { border-bottom: 0; }
.mcp-tool code { color: var(--cyan); font-family: var(--mono); font-size: var(--fs-small); }
.mcp-tool p { margin: 4px 0 0; color: var(--muted); font-size: var(--fs-small); line-height: 1.5; }
</style>
