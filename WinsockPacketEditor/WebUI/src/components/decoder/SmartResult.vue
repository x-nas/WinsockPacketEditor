<script setup lang="ts">
/*
  智能解码结果。列出每个命中可读明文的解码器及其结果。
*/
import { computed } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import SettingsModal from '../proxy/SettingsModal.vue'
import { b64ToBytes, bytesToHex, type SmartPayload } from './actions'

const props = defineProps<{ payload: SmartPayload | null }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const hits = computed(() => props.payload?.hits ?? [])

function hexOf(h: { OutputBase64: string }): string {
  if (!h.OutputBase64) return ''
  return bytesToHex(b64ToBytes(h.OutputBase64))
}
async function copy(s: string): Promise<void> {
  if (!s) return
  try { await call('clipboardWrite', { text: s }); pushToast('success', t('pm.copied')) }
  catch (e) { console.error('[dec] copy failed', e) }
}
</script>

<template>
  <SettingsModal :open="props.payload !== null" :title="t('dec.smartTitle')" subtitle="Smart decode" readonly :width="780"
                 @update:open="emit('close')">
    <div class="setf sr" v-if="props.payload">
      <p v-if="!hits.length" class="none">{{ t('dec.smartNone') }}</p>
      <div v-for="h in hits" :key="h.Id" class="hit">
        <div class="hd">
          <span class="nm">{{ h.Name }}</span>
          <span class="off" v-if="h.Offset">+{{ h.Offset }}</span>
          <span class="fill" />
          <button class="op" @click="copy(h.Text || hexOf(h))">{{ t('tr.copyOut') }}</button>
        </div>
        <pre class="box">{{ h.Text }}</pre>
        <pre class="box hx" v-if="hexOf(h)">{{ hexOf(h) }}</pre>
      </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.sr { padding: 4px 0 8px; }
.none { padding: 20px; color: var(--muted); font-size: var(--fs-small); }
.hit { padding: 10px 20px; border-bottom: 1px solid var(--border); }
.hd { display: flex; align-items: center; gap: 8px; }
.nm { font-family: var(--mono); font-size: var(--fs-small); color: var(--cyan); }
.off { font-family: var(--mono); font-size: var(--fs-small); color: var(--amber); }
.fill { flex: 1; }
.op { padding: 4px 10px; border: 1px solid var(--border); background: transparent; color: var(--gray); cursor: pointer; font-size: var(--fs-small); }
.op:hover { color: var(--cyan); border-color: var(--cyan); }
.box { margin: 6px 0 0; padding: 6px 8px; max-height: 130px; overflow: auto; white-space: pre-wrap; overflow-wrap: anywhere; background: rgb(var(--inset-rgb) / 30%); border: 1px solid var(--border); font: var(--fs-small)/1.55 var(--mono); color: var(--acc-green2); }
.box.hx { color: var(--gray); max-height: 80px; }
</style>
