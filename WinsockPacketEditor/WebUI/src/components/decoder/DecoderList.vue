<script setup lang="ts">
/*
  解码器列表 —— 对应 WinForms 的 DecoderList 那一屏（与滤镜 / 发送 / 机器人 / 仓库并列）。

  一条「解码器」= 一套保存下来的算法 + 帧 + 适用范围配置，供列表 / 详情按需解码，
  也是「智能解码」自动尝试的候选集。列表顺序有意义（智能解码与右键子菜单都按它走），
  所以右键菜单与其它四份列表一样带置顶 / 上移 / 下移 / 置底。

  解码器不在 FeedList 推送流里（走 getDecoders 拉取），所以这屏不订阅 FeedPump，
  每个改动动作之后自己重新拉一次即可。列表规模小，重复拉不贵。

  编辑走 DecoderEdit 弹窗（与「保存为解码器」同一个）。
*/
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { call } from '../../bridge'
import { ListAction, type DecoderRow } from '../../bridge/types'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { decRows } from '../../stores/decoder'
import { useRowPick } from '../../usePick'
import ContextMenu from '../ContextMenu.vue'
import DecoderEdit from './DecoderEdit.vue'
import { kindLabel } from './enums'
import { ensureDecoders } from './actions'
import { ICON, type MenuItem } from '../menu'

const rows = decRows

/* ── 虚拟滚动（与各列表同一套；ROW_H 必须与 .row 的 height 一致）── */
const ROW_H = 34
const OVERSCAN = 8

const scroller = ref<HTMLElement | null>(null)
const scrollTop = ref(0)
const viewH = ref(600)

const total = computed(() => rows.value.length)
const start = computed(() => Math.max(0, Math.floor(scrollTop.value / ROW_H) - OVERSCAN))
const end = computed(() => Math.min(total.value, Math.ceil((scrollTop.value + viewH.value) / ROW_H) + OVERSCAN))
const windowRows = computed(() => rows.value.slice(start.value, end.value))

function onScroll(): void {
  const el = scroller.value
  if (el) scrollTop.value = el.scrollTop
}

let ro: ResizeObserver | null = null

onMounted(async () => {
  const el = scroller.value
  if (el) {
    viewH.value = el.clientHeight
    ro = new ResizeObserver(() => { viewH.value = el.clientHeight })
    ro.observe(el)
  }
  await refresh()
})

onBeforeUnmount(() => {
  ro?.disconnect()
  ro = null
})

/** 重新拉一次解码器列表。挂载与每个改动动作之后都调。 */
async function refresh(): Promise<void> {
  await ensureDecoders(true)
}

/* ── 显示用的换算 ───────────────────────────────────────────── */

const PROTO_LABELS = ['', 'TCP', 'UDP', 'HTTP', 'WebSocket']

/** 适用范围：协议 + 方向。0 都是「全部」。 */
function scopeText(r: DecoderRow): string {
  const p = r.ProtocolType > 0 ? (PROTO_LABELS[r.ProtocolType] ?? String(r.ProtocolType)) : t('dec.any')
  const d = r.Direction === 0 ? t('dec.any') : r.Direction === 1 ? t('dec.dirReq') : t('dec.dirResp')
  return p + ' · ' + d
}

/* ── 多选 ───────────────────────────────────────────────────── */

const { picked, pickedIds, onRowClick, selectAll, clear } = useRowPick(rows, (r) => r.Id)

/* ── 行为 ───────────────────────────────────────────────────── */

const editing = ref<DecoderRow | null>(null)

async function toggle(r: DecoderRow): Promise<void> {
  try {
    await call('setDecoderEnable', { id: r.Id, enable: !r.IsEnable })
    await refresh()
  } catch (e) {
    console.error('[dec] 切换启用失败', e)
  }
}

async function add(): Promise<void> {
  try {
    await call('addDecoder')
    await refresh()
    //新解码器追加在表尾，滚过去让人看见它
    requestAnimationFrame(() => {
      const el = scroller.value
      if (el) el.scrollTop = el.scrollHeight
    })
  } catch (e) {
    console.error('[dec] 新增失败', e)
  }
}

/** 工具条那些「不弹编辑器、只调一个桥方法」的动作。改完列表要重新拉。 */
async function simple(method: string, arg?: Record<string, unknown>): Promise<void> {
  try {
    await call(method, arg)
    await refresh()
  } catch (e) {
    console.error('[dec] ' + method + ' 失败', e)
  }
}

function openEdit(r: DecoderRow): void {
  editing.value = r
}

function onRowDblClick(e: MouseEvent, r: DecoderRow): void {
  if ((e.target as HTMLElement | null)?.closest('button')) return
  openEdit(r)
}

/* ── 右键菜单 ───────────────────────────────────────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  const n = picked.value.size
  const tag = n ? ' (' + n + ')' : ''

  /*
    顺序动作是有的：智能解码按列表顺序逐条尝试，右键「解码 ▸」的子菜单也按这个顺序列。
    七个动作的编号与滤镜 / 发送 / 机器人 / 仓库完全一致（Operate.SystemConfig.ListAction）。
  */
  return [
    { id: 'top', label: t('lst.top') + tag, icon: ICON.top },
    { divider: true },
    { id: 'up', label: t('lst.up') + tag, icon: ICON.up },
    { id: 'down', label: t('lst.down') + tag, icon: ICON.down },
    { divider: true },
    { id: 'bottom', label: t('lst.bottom') + tag, icon: ICON.bottom },
    { divider: true },
    { id: 'export', label: t('lst.export') + tag, icon: ICON.save },
    { id: 'copy', label: t('lst.copy') + tag, icon: ICON.copy },
    { divider: true },
    { id: 'delete', label: t('lst.delete') + tag, icon: ICON.del, danger: true },
    { divider: true },
    { id: 'selectAll', label: t('pm.selectAll'), icon: ICON.list },
    { id: 'deselect', label: t('pm.deselect'), icon: ICON.del, disabled: !n },
  ]
})

const ACTION_OF: Record<string, ListAction> = {
  top: ListAction.Top,
  up: ListAction.Up,
  down: ListAction.Down,
  bottom: ListAction.Bottom,
  copy: ListAction.Copy,
  export: ListAction.Export,
  delete: ListAction.Delete,
}

async function onMenuPick(id: string): Promise<void> {
  if (id === 'selectAll') { selectAll(); return }
  if (id === 'deselect') { clear(); return }

  if (picked.value.size === 0) {
    pushToast('warning', t('lst.needPick'))
    return
  }

  const action = ACTION_OF[id]
  if (action === undefined) return

  try {
    await call('decoderListAction', { action, ids: pickedIds.value })
    await refresh()
  } catch (e) {
    console.error('[dec] 列表操作失败', e)
  }
}
</script>

<template>
  <div class="page list-page">
    <div class="bar">
      <button class="btn primary" @click="add">{{ t('dec.add') }}</button>
      <button class="btn" :disabled="!rows.length" @click="simple('setAllDecoderEnable', { enable: true })">
        {{ t('dec.enableAll') }}
      </button>
      <button class="btn" :disabled="!rows.length" @click="simple('setAllDecoderEnable', { enable: false })">
        {{ t('dec.disableAll') }}
      </button>

      <span class="grow" />

      <button class="btn" @click="simple('importDecoders')">{{ t('dec.import') }}</button>
      <button class="btn" :disabled="!rows.length" @click="simple('exportDecoders')">{{ t('dec.export') }}</button>
      <button class="btn danger" :disabled="!rows.length" @click="simple('clearDecoders')">{{ t('dec.clearAll') }}</button>
    </div>

    <div ref="scroller" class="body" @scroll.passive="onScroll">
      <div class="head">
        <span class="no">{{ t('col.id') }}</span>
        <span class="ck">{{ t('col.enable') }}</span>
        <span class="name">{{ t('col.decoderName') }}</span>
        <span class="kind">{{ t('col.decoderKind') }}</span>
        <span class="scope">{{ t('col.decoderScope') }}</span>
        <span class="desc">{{ t('col.decoderDesc') }}</span>
        <span class="ops">{{ t('col.ops') }}</span>
      </div>

      <div v-if="!rows.length" class="empty">{{ t('dec.empty') }}</div>

      <div v-else class="spacer" :style="{ height: total * ROW_H + 'px' }">
        <div class="win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
          <div
            v-for="(r, i) in windowRows"
            :key="i"
            class="row"
            :class="{ off: !r.IsEnable, sel: picked.has(r.Id) }"
            @click="onRowClick(r, $event, start + i)"
            @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }"
            @dblclick="onRowDblClick($event, r)"
          >
            <span class="no">{{ start + i + 1 }}</span>

            <span class="ck">
              <button class="chk" :class="{ on: r.IsEnable }" :title="t('dec.enable')" @click.stop="toggle(r)"><i /></button>
            </span>

            <span class="name" :title="r.Name">{{ r.Name }}</span>

            <span class="kind">{{ kindLabel(r.Kind) }}</span>

            <span class="scope">{{ scopeText(r) }}</span>

            <span class="desc" :title="r.Description">{{ r.Description }}</span>

            <span class="ops">
              <button class="op" :title="t('acct.op.edit')" @click.stop="openEdit(r)">
                <svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg>
              </button>
              <button class="op del" :title="t('acct.op.del')"
                      @click.stop="picked = new Set([r.Id]); onMenuPick('delete')">
                <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
              </button>
            </span>
          </div>
        </div>
      </div>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

    <DecoderEdit :target="editing" @close="editing = null" @saved="refresh" />
  </div>
</template>

<style scoped>
.page {
  flex: 1;
  min-width: 0;
  min-height: 0;
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding: 10px 12px 12px;
}

/* 七列。名称与描述是两段长度不可预知的文本，各给一份弹性 */
/* display / align-items / padding / 高度 / 配色都在 style.css 的 .list-page 里 */
.head,
.row {
  grid-template-columns: 46px 46px minmax(120px, 1.1fr) 96px 150px minmax(110px, 1fr) 68px;
  gap: 10px;
  min-width: 640px;
}

.head > span,
.row > span { text-align: center; }

.head > span.name,
.row > span.name,
.head > span.desc,
.row > span.desc { text-align: left; }

.row > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }
.kind { color: var(--acc-violet); }
.scope { color: var(--muted); font-family: var(--mono); }
.desc { color: var(--dim2); }
</style>
