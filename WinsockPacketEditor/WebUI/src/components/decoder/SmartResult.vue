<script setup lang="ts">
/*
  智能解码结果。列出每个命中可读明文的解码器及其结果。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import SettingsModal from '../proxy/SettingsModal.vue'
import { b64ToBytes, bytesToHex, type SmartBatchItem, type SmartHit, type SmartPayload } from './actions'

const props = defineProps<{ payload: SmartPayload | null }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const hits = computed(() => props.payload?.hits ?? [])

/*
  多选封包时后端不是一次给完，是前端逐条跑 smartDecode 攒出来的 items。
  统一成 groups 渲染：有 items 就按封包分组，没有就把单条的 hits 包成一组（不画封包头）。
*/
const groups = computed<SmartBatchItem[]>(() =>
  props.payload?.items ?? [{ Id: 0, Time: '', Preview: '', hits: hits.value }])
const grouped = computed(() => !!props.payload?.items)
const page = ref(0)
const pageSize = 40
const pageCount = computed(() => Math.max(1, Math.ceil(groups.value.length / pageSize)))
const visibleGroups = computed(() => groups.value.slice(page.value * pageSize, (page.value + 1) * pageSize))
const expanded = ref<Record<number, SmartHit[]>>({})
const loading = ref<number | null>(null)
watch(() => props.payload, () => { page.value = 0; expanded.value = {} })
function groupHits(g: SmartBatchItem): SmartHit[] { return expanded.value[g.Id] ?? g.hits }

function hexOf(h: { OutputBase64: string }): string {
  if (!h.OutputBase64) return ''
  return bytesToHex(b64ToBytes(h.OutputBase64))
}
async function copy(s: string): Promise<void> {
  if (!s) return
  try { await call('clipboardWrite', { text: s }); pushToast('success', t('pm.copied')) }
  catch (e) { console.error('[dec] copy failed', e) }
}
async function loadFull(g: SmartBatchItem): Promise<void> {
  if (!props.payload?.list || loading.value !== null) return
  loading.value = g.Id
  try {
    const r = await call<{ hits: SmartHit[] }>('getSmartDecodeDetail', { list: props.payload.list, packetId: g.Id })
    expanded.value = { ...expanded.value, [g.Id]: r?.hits ?? [] }
  } catch (e) { console.error('[dec] load full smart result failed', e) }
  finally { loading.value = null }
}
</script>

<template>
  <SettingsModal :open="props.payload !== null" :title="t('dec.smartTitle')" subtitle="Smart decode" readonly :width="780"
                 @update:open="emit('close')">
    <div class="setf sr" v-if="props.payload">
      <div v-for="g in visibleGroups" :key="g.Id" class="grp">
        <p v-if="!groupHits(g).length" class="none">{{ t('dec.smartNone') }}</p>
        <div v-for="h in groupHits(g)" :key="h.Id" class="result-row">
          <div class="meta">
            <span v-if="grouped" class="id">ID {{ g.Id }}</span>
            <span class="nm">{{ h.Name }}</span>
            <span class="off" v-if="h.Offset">+{{ h.Offset }}</span>
            <span class="fill" />
            <button v-if="h.Truncated && !expanded[g.Id]" class="op full" :disabled="loading !== null" @click="loadFull(g)">{{ loading === g.Id ? '…' : t('dec.fullResult') }}</button>
            <button class="op copy" :title="t('tr.copyOut')" :aria-label="t('tr.copyOut')" @click="copy(h.Text || hexOf(h))">⧉</button>
          </div>
          <div class="txt">
            <b class="tx">{{ h.Text }}</b>
            <i class="hx" v-if="hexOf(h)">{{ hexOf(h) }}</i>
            <i class="clip" v-if="h.Truncated">{{ t('dec.batchPreview') }}</i>
          </div>
        </div>
      </div>
      <div v-if="pageCount > 1" class="pager">
        <button class="op" :disabled="page === 0" @click="page--">‹</button><span>{{ page + 1 }} / {{ pageCount }}</span><button class="op" :disabled="page + 1 >= pageCount" @click="page++">›</button>
      </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.sr { padding: 4px 0 8px; }
.none { padding: 20px; color: var(--muted); font-size: var(--fs-small); }
.result-row { padding: 10px 20px; border-bottom: 1px solid var(--border); }
.meta { display: flex; align-items: center; gap: 8px; margin-bottom: 5px; }
.id { font-family: var(--mono); font-size: var(--fs-small); color: var(--dim2); }
.nm { font-family: var(--mono); font-size: var(--fs-small); color: var(--cyan); }
.off { font-family: var(--mono); font-size: var(--fs-small); color: var(--amber); }
.fill { flex: 1; }
.op { padding: 4px 10px; border: 1px solid var(--border); background: transparent; color: var(--gray); cursor: pointer; font-size: var(--fs-small); }
.copy { width: 18px; height: 18px; padding: 0; border: 0; font: 14px/1 var(--mono); }
.full { padding: 2px 6px; font-size: var(--fs-small); }
.copy:hover { background: rgb(var(--cyan-rgb) / 12%); }
.op:hover { color: var(--cyan); border-color: var(--cyan); }
.txt { min-width: 0; display: flex; flex-direction: column; gap: 2px; }
.tx { font: var(--fs-small)/1.5 var(--mono); color: var(--acc-green2); white-space: pre-wrap; overflow-wrap: anywhere; }
.hx { box-sizing: border-box; height: 80px; margin-top: 2px; padding: 6px 8px; overflow-y: auto; white-space: pre-wrap; overflow-wrap: anywhere; background: rgb(var(--inset-rgb) / 30%); border: 1px solid var(--border); font: var(--fs-small)/1.5 var(--mono); color: var(--muted); }
.clip { color: var(--amber); font-size: var(--fs-small); }
.pager { display: flex; justify-content: center; align-items: center; gap: 10px; padding: 12px; font: var(--fs-small) var(--mono); color: var(--muted); }
</style>
