<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { call } from '../../bridge'
import { ListAction } from '../../bridge/types'
import { t } from '../../i18n'
import { ensurePacketExtractors, extractorRows, type Extractor } from '../../stores/extractor'
import { pushToast } from '../../stores/toast'
import { useRowPick } from '../../usePick'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import PacketExtractorEdit from './PacketExtractorEdit.vue'

const rows = extractorRows, editing = ref<Extractor | null>(null)
const ROW_H = 34, OVERSCAN = 8, scroller = ref<HTMLElement | null>(null), scrollTop = ref(0), viewH = ref(600)
const total = computed(() => rows.value.length)
const start = computed(() => Math.max(0, Math.floor(scrollTop.value / ROW_H) - OVERSCAN))
const end = computed(() => Math.min(total.value, Math.ceil((scrollTop.value + viewH.value) / ROW_H) + OVERSCAN))
const windowRows = computed(() => rows.value.slice(start.value, end.value))
const { picked, pickedIds, onRowClick, selectAll, clear } = useRowPick(rows, (r) => r.Id)
const scopeText = (n: number): string => n === 0 ? t('pex.scopeGlobal') : n === 1 ? t('pex.scopeSocket') : t('pex.scopeSession')
async function refresh(): Promise<void> { await ensurePacketExtractors() }
function onScroll(): void { if (scroller.value) scrollTop.value = scroller.value.scrollTop }
let ro: ResizeObserver | null = null
onMounted(async () => { const el = scroller.value; if (el) { viewH.value = el.clientHeight; ro = new ResizeObserver(() => { if (el) viewH.value = el.clientHeight }); ro.observe(el) }; await refresh() })
onBeforeUnmount(() => { ro?.disconnect() })
async function add(): Promise<void> { const result = await call<{ id: string }>('addPacketExtractor'); await refresh(); editing.value = rows.value.find(x => x.Id === result.id) || null }
async function simple(method: string, arg?: Record<string, unknown>): Promise<void> { await call(method, arg); await refresh() }
async function toggle(row: Extractor): Promise<void> { await call('savePacketExtractor', { extractor: { ...row, IsEnable: !row.IsEnable } }); await refresh() }
function onRowDblClick(e: MouseEvent, row: Extractor): void { if (!(e.target as HTMLElement | null)?.closest('button')) editing.value = row }
const menuAt = ref<{ x: number; y: number } | null>(null)
const menuItems = computed<MenuItem[]>(() => { const n = picked.value.size, tag = n ? ' (' + n + ')' : ''; return [{ id: 'top', label: t('lst.top') + tag, icon: ICON.top }, { divider: true }, { id: 'up', label: t('lst.up') + tag, icon: ICON.up }, { id: 'down', label: t('lst.down') + tag, icon: ICON.down }, { divider: true }, { id: 'bottom', label: t('lst.bottom') + tag, icon: ICON.bottom }, { divider: true }, { id: 'export', label: t('lst.export') + tag, icon: ICON.save }, { id: 'copy', label: t('lst.copy') + tag, icon: ICON.copy }, { divider: true }, { id: 'delete', label: t('lst.delete') + tag, icon: ICON.del, danger: true }, { divider: true }, { id: 'selectAll', label: t('pm.selectAll'), icon: ICON.list }, { id: 'deselect', label: t('pm.deselect'), icon: ICON.del, disabled: !n }] })
const ACTION_OF: Record<string, ListAction> = { top: ListAction.Top, up: ListAction.Up, down: ListAction.Down, bottom: ListAction.Bottom, copy: ListAction.Copy, export: ListAction.Export, delete: ListAction.Delete }
async function onMenuPick(id: string): Promise<void> { if (id === 'selectAll') { selectAll(); return }; if (id === 'deselect') { clear(); return }; if (!picked.value.size) { pushToast('warning', t('lst.needPick')); return }; const action = ACTION_OF[id]; if (action === undefined) return; await call('packetExtractorListAction', { action, ids: pickedIds.value }); await refresh() }
</script>
<template>
  <div class="page list-page">
    <div class="bar"><button class="btn primary" @click="add">{{ t('pex.add') }}</button><button class="btn" :disabled="!rows.length" @click="simple('setAllPacketExtractorEnable', { enable: true })">{{ t('pex.enableAll') }}</button><button class="btn" :disabled="!rows.length" @click="simple('setAllPacketExtractorEnable', { enable: false })">{{ t('pex.disableAll') }}</button><span class="grow" /><button class="btn" @click="simple('importPacketExtractors')">{{ t('pex.import') }}</button><button class="btn" :disabled="!rows.length" @click="simple('exportPacketExtractors')">{{ t('pex.export') }}</button><button class="btn danger" :disabled="!rows.length" @click="simple('clearPacketExtractors')">{{ t('pex.clearAll') }}</button></div>
    <div ref="scroller" class="body" @scroll.passive="onScroll">
      <div class="head"><span class="no">{{ t('col.id') }}</span><span class="ck">{{ t('col.enable') }}</span><span class="name">{{ t('col.extractorName') }}</span><span class="scope">{{ t('col.extractorScope') }}</span><span class="count">{{ t('col.variableCount') }}</span><span class="desc">{{ t('col.notes') }}</span><span class="ops">{{ t('col.ops') }}</span></div>
      <div v-if="!rows.length" class="empty">{{ t('pex.empty') }}</div>
      <div v-else class="spacer" :style="{ height: total * ROW_H + 'px' }"><div class="win" :style="{ transform: `translateY(${start * ROW_H}px)` }"><div v-for="(row, i) in windowRows" :key="row.Id" class="row" :class="{ off: !row.IsEnable, sel: picked.has(row.Id) }" @click="onRowClick(row, $event, start + i)" @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }" @dblclick="onRowDblClick($event, row)"><span class="no">{{ start + i + 1 }}</span><span class="ck"><button class="chk" :class="{ on: row.IsEnable }" :title="t('dec.enable')" @click.stop="toggle(row)"><i /></button></span><span class="name" :title="row.Name">{{ row.Name }}</span><span class="scope">{{ scopeText(row.Scope) }}</span><span class="count">{{ row.Variables.length }}</span><span class="desc" :title="row.Description">{{ row.Description }}</span><span class="ops"><button class="op" :title="t('acct.op.edit')" @click.stop="editing = row"><svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg></button><button class="op del" :title="t('acct.op.del')" @click.stop="picked = new Set([row.Id]); onMenuPick('delete')"><svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg></button></span></div></div></div>
    </div>
    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />
    <PacketExtractorEdit :target="editing" @close="editing = null" @saved="refresh" />
  </div>
</template>
<style scoped>
.page{flex:1;min-width:0;min-height:0;display:flex;flex-direction:column;gap:8px;padding:10px 12px 12px}.head,.row{grid-template-columns:46px 46px minmax(160px,1.2fr) 180px 80px minmax(120px,1fr) 68px;gap:10px;min-width:720px}.head>span,.row>span{text-align:center}.head>.name,.row>.name,.head>.desc,.row>.desc{text-align:left}.row>span{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.no{color:var(--dim);font-variant-numeric:tabular-nums}.name{color:var(--gray)}.scope{color:var(--muted);font-family:var(--mono)}.count{color:var(--acc-violet)}.desc{color:var(--dim2)}
</style>
