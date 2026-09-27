<script setup lang="ts">
/*
  批量解码结果。逐条列出选中封包按同一个解码器解出来的结果。
*/
import { computed } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import SettingsModal from '../proxy/SettingsModal.vue'
import type { BatchPayload } from './actions'

const props = defineProps<{ payload: BatchPayload | null }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const rows = computed(() => props.payload?.rows ?? [])
const okCount = computed(() => rows.value.filter((r) => r.Ok).length)

async function copy(s: string): Promise<void> {
  if (!s) return
  try { await call('clipboardWrite', { text: s }); pushToast('success', t('pm.copied')) }
  catch (e) { console.error('[dec] copy failed', e) }
}
</script>

<template>
  <SettingsModal :open="props.payload !== null" :title="t('dec.batchTitle')" :subtitle="props.payload?.decoder || ''" readonly :width="880"
                 @update:open="emit('close')">
    <div class="setf br" v-if="props.payload">
      <p v-if="!rows.length" class="none">{{ t('dec.batchNone') }}</p>
      <template v-else>
        <div class="sum">{{ props.payload.decoder }} · {{ okCount }} / {{ rows.length }}</div>
        <div class="row head"><span class="id">ID</span><span class="txt">{{ t('tr.outPane') }}</span><span class="ops" /></div>
        <div v-for="r in rows" :key="r.Id" class="row" :class="{ bad: !r.Ok }">
          <span class="id">{{ r.Id }}</span>
          <span class="txt">
            <template v-if="r.Ok"><b class="tx">{{ r.Text }}</b><i class="hx">{{ r.Hex }}</i></template>
            <template v-else><i class="err">{{ r.Error }}</i></template>
          </span>
          <span class="ops"><button class="op" :disabled="!r.Ok" @click="copy(r.Text || r.Hex)">{{ t('tr.copyOut') }}</button></span>
        </div>
      </template>
    </div>
  </SettingsModal>
</template>

<style scoped>
.br { padding: 4px 0 8px; }
.none { padding: 20px; color: var(--muted); font-size: var(--fs-small); }
.sum { padding: 8px 20px; font-family: var(--mono); font-size: var(--fs-small); color: var(--cyan); }
.row { display: grid; grid-template-columns: 70px minmax(0, 1fr) 80px; gap: 10px; align-items: start; padding: 6px 20px; border-bottom: 1px solid var(--border); }
.row.head { color: var(--muted); font: var(--fs-small) var(--mono); text-transform: uppercase; letter-spacing: .1em; }
.row.bad { opacity: .7; }
.id { font-family: var(--mono); font-size: var(--fs-small); color: var(--dim2); }
.txt { min-width: 0; display: flex; flex-direction: column; gap: 2px; }
.tx { font: var(--fs-small)/1.5 var(--mono); color: var(--acc-green2); white-space: pre-wrap; overflow-wrap: anywhere; }
.hx { font: var(--fs-small)/1.5 var(--mono); color: var(--muted); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
.err { color: var(--danger); font-size: var(--fs-small); }
.ops { text-align: right; }
.op { padding: 4px 8px; border: 1px solid var(--border); background: transparent; color: var(--gray); cursor: pointer; font-size: var(--fs-small); }
.op:hover:not(:disabled) { color: var(--cyan); border-color: var(--cyan); }
.op:disabled { opacity: .4; cursor: default; }
</style>
