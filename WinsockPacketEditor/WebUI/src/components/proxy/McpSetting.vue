<script setup lang="ts">
import { ref, watch } from 'vue'
import { call } from '../../bridge'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', value: boolean): void }>()
const autoApproveWrites = ref(false)
const busy = ref(false)
const error = ref('')

watch(() => props.open, async (open) => {
  if (!open) return
  error.value = ''
  try {
    const value = await call<{ autoApproveWrites: boolean }>('getMcpSettings')
    autoApproveWrites.value = value.autoApproveWrites
  } catch (e) { error.value = String(e) }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    await call('saveMcpSettings', { autoApproveWrites: autoApproveWrites.value })
    emit('update:open', false)
  } catch (e) { error.value = String(e) } finally { busy.value = false }
}
</script>

<template>
  <SettingsModal :open="props.open" title="MCP 设置" subtitle="本机 AI 自动化权限" :busy="busy" :error="error" @update:open="emit('update:open', $event)" @save="save">
    <div class="setf mcp-set">
    <section class="sec">
      <div class="sec-h">写操作确认</div>
      <label class="swb">
        <input v-model="autoApproveWrites" type="checkbox" />
        <span class="swb-ui" />
        <span class="swb-text">
          <b>允许 MCP 自动执行写操作</b>
          <small v-if="autoApproveWrites">关闭 WPE 本地确认；MCP 写操作将直接执行。</small>
          <small v-else>每次高风险写操作都需要 WPE 本地确认。</small>
        </span>
      </label>
      <p class="hint">此开关仅影响本机 MCP。幂等保护、敏感数据脱敏和审计仍然保留。</p>
    </section>
    <section class="sec muted">
      <div class="sec-h">模式范围</div>
      <div class="mode-row"><span>代理模式</span><span class="ok">可用</span></div>
      <div class="mode-row"><span>注入模式</span><span class="ok">可用</span></div>
      <p class="hint">后续增加模式专属 MCP 配置时，会在此处按模式显示可用性。</p>
    </section>
    </div>
  </SettingsModal>
</template>

<style scoped>
.mcp-set { padding: 8px 20px 14px; }
.mcp-set .sec { margin: 0; padding: 12px 0; }
.mcp-set .sec + .sec { margin-top: 4px; }
.mode-row { display:flex; justify-content:space-between; align-items:center; padding:8px 0; color:var(--tx-2); }
.ok { color:var(--green, #65d59b); }
.hint { margin:10px 0 0; color:var(--tx-3); font-size:var(--fs-small); line-height:1.5; }
.muted { opacity:.9; }
</style>
