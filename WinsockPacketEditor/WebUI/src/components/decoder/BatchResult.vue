<script setup lang="ts">
/*
  批量解码结果。逐条列出选中封包按同一个解码器解出来的结果。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import SettingsModal from '../proxy/SettingsModal.vue'
import type { BatchPayload } from './actions'

const props = defineProps<{ payload: BatchPayload | null }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const rows = computed(() => props.payload?.rows ?? [])
const okCount = computed(() => rows.value.filter((r) => r.Ok).length)
const page = ref(0)
const pageSize = 40
const pageCount = computed(() => Math.max(1, Math.ceil(rows.value.length / pageSize)))
const visibleRows = computed(() => rows.value.slice(page.value * pageSize, (page.value + 1) * pageSize))
const expanded = ref<Record<number, { Text: string; Hex: string }>>({})
const loading = ref<number | null>(null)
watch(() => props.payload, () => { page.value = 0; expanded.value = {} })

async function copy(s: string): Promise<void> {
  if (!s) return
  try { await call('clipboardWrite', { text: s }); pushToast('success', t('pm.copied')) }
  catch (e) { console.error('[dec] copy failed', e) }
}
async function loadFull(id: number): Promise<void> {
  if (!props.payload || loading.value !== null) return
  loading.value = id
  try {
    const r = await call<{ Id: number; Text: string; Hex: string }>('getBatchDecodeDetail', {
      list: props.payload.list, decoderId: props.payload.decoderId, packetId: id,
    })
    if (r) expanded.value = { ...expanded.value, [id]: { Text: r.Text ?? '', Hex: r.Hex ?? '' } }
  } catch (e) { console.error('[dec] load full batch result failed', e) }
  finally { loading.value = null }
}
</script>

<template>
  <SettingsModal :open="props.payload !== null" :title="t('dec.batchTitle')" :subtitle="props.payload?.decoder || ''" readonly :width="880"
                 @update:open="emit('close')">
    <div class="setf br" v-if="props.payload">
      <p v-if="!rows.length" class="none">{{ t('dec.batchNone') }}</p>
      <template v-else>
        <div class="sum">{{ props.payload.decoder }} · {{ okCount }} / {{ rows.length }}</div>
        <div v-for="r in visibleRows" :key="r.Id" class="result-row" :class="{ bad: !r.Ok }">
          <div class="meta">
            <span class="id">ID {{ r.Id }}</span>
            <button v-if="r.Truncated && !expanded[r.Id]" class="op full" :disabled="loading !== null" @click="loadFull(r.Id)">{{ loading === r.Id ? '…' : t('dec.fullResult') }}</button>
            <button class="op copy" :disabled="!r.Ok" :title="t('tr.copyOut')" :aria-label="t('tr.copyOut')" @click="copy(expanded[r.Id]?.Text || expanded[r.Id]?.Hex || r.Text || r.Hex)">⧉</button>
          </div>
          <div class="txt">
            <template v-if="r.Ok"><b class="tx">{{ expanded[r.Id]?.Text ?? r.Text }}</b><i class="hx">{{ expanded[r.Id]?.Hex ?? r.Hex }}</i></template>
            <template v-else><i class="err">{{ r.Error }}</i></template>
          </div>
        </div>
      </template>
      <div v-if="pageCount > 1" class="pager">
        <button class="op" :disabled="page === 0" @click="page--">‹</button><span>{{ page + 1 }} / {{ pageCount }}</span><button class="op" :disabled="page + 1 >= pageCount" @click="page++">›</button>
      </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.br { padding: 4px 0 8px; }
.none { padding: 20px; color: var(--muted); font-size: var(--fs-small); }
.sum { padding: 8px 20px; font-family: var(--mono); font-size: var(--fs-small); color: var(--cyan); }
.result-row { padding: 10px 20px; border-bottom: 1px solid var(--border); }
.result-row.bad { opacity: .7; }
.meta { display: flex; align-items: center; justify-content: space-between; gap: 12px; margin-bottom: 5px; }
.id { font-family: var(--mono); font-size: var(--fs-small); color: var(--dim2); }
.txt { min-width: 0; display: flex; flex-direction: column; gap: 2px; }
.tx { font: var(--fs-small)/1.5 var(--mono); color: var(--acc-green2); white-space: pre-wrap; overflow-wrap: anywhere; }
.hx { box-sizing: border-box; height: 80px; margin-top: 2px; padding: 6px 8px; overflow-y: auto; white-space: pre-wrap; overflow-wrap: anywhere; background: rgb(var(--inset-rgb) / 30%); border: 1px solid var(--border); font: var(--fs-small)/1.5 var(--mono); color: var(--muted); }
.err { color: var(--danger); font-size: var(--fs-small); }
.op { padding: 4px 8px; border: 1px solid var(--border); background: transparent; color: var(--gray); cursor: pointer; font-size: var(--fs-small); }
.copy { width: 18px; height: 18px; padding: 0; border: 0; font: 14px/1 var(--mono); }
.full { margin-left: auto; padding: 2px 6px; font-size: var(--fs-small); }
.pager { display: flex; justify-content: center; align-items: center; gap: 10px; padding: 12px; font: var(--fs-small) var(--mono); color: var(--muted); }
.copy:hover:not(:disabled) { background: rgb(var(--cyan-rgb) / 12%); }
.op:hover:not(:disabled) { color: var(--cyan); border-color: var(--cyan); }
.op:disabled { opacity: .4; cursor: default; }
</style>
