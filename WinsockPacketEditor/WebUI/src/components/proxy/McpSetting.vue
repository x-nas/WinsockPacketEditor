<script setup lang="ts">
import { ref, watch } from 'vue'
import { call } from '../../bridge'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', value: boolean): void }>()
const enabled = ref(true)
const requiresConfirmation = ref(true)
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
    <section class="sec">
      <div class="grp">模式范围</div>
      <div class="row"><span class="k">代理模式</span><span class="v ok">可用</span></div>
      <div class="row"><span class="k">注入模式</span><span class="v ok">可用</span></div>
      <p class="hint">MCP 服务关闭时所有 MCP 对接均不可用。幂等保护、敏感数据脱敏和审计仍然保留。</p>
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
</style>
