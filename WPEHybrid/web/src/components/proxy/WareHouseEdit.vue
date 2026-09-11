<script setup lang="ts">
/*
  仓库编辑 —— 对应 WinForms 的 Controls/WareHouseEdit。

  上面一行是仓库名称；下面是<b>仓储数据</b>表（序号 / 长度 / 数据预览）。
  工具条：导入 / 导出全部 / 清空 / 复制十六进制；
  右键七项（置顶 / 上移 / 下移 / 置底 / 复制 / 导出选中 / 删除）+ 全选 / 取消选择。

  【没有十六进制面板】WinForms 那边右侧有一块 HexBox。这里做过一版，<b>已按要求去掉</b>：
  这一屏要看的是「仓库里有哪些封包」，要看字节用右键「复制十六进制」拿出去，
  或等封包编辑（PacketEdit）做出来后双击进去看。表因此占满整个宽度。

  【改的是仓库本身，不是工作副本】与发送编辑<b>相反</b>：WinForms 的 WareHouseEdit_Load 里
  this.Stores = whiSelect.Stores，排序 / 删除 / 导入 / 清空直接动仓库里那份列表，
  只有名字按「保存」才写回。这里照搬，所以关弹窗不用收尾、取消也不会撤销对数据的改动 ——
  界面上把这句写在表下面，免得有人以为取消能反悔。

  【行不带字节】仓储数据是能攒到几万条的（自动入库对着一个热包头收一会儿就是），
  行只带长度与预览 —— 与封包列表同一条规矩。
  同理这张表<b>必须虚拟滚动</b>，账号列表那次实测两万行铺开是三秒。
*/
import { computed, onBeforeUnmount, ref, watch } from 'vue'
import { call } from '../../bridge'
import { ListAction } from '../../bridge/types'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { useRowPick } from '../../usePick'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import { useModal } from '../../useModal'

const props = defineProps<{ id: string | null }>()

const emit = defineEmits<{ (e: 'close'): void }>()

interface StoreRow {
  Id: string
  Len: number
  Preview: string
}

const name = ref('')
const loaded = ref(false)
const rows = ref<StoreRow[]>([])
const busy = ref(false)
const error = ref('')

/* ── 打开 / 关闭 ────────────────────────────────────────────── */

watch(() => props.id, async (id) => {
  if (id === null) return

  loaded.value = false
  name.value = ''
  rows.value = []
  picked.value = new Set()
  error.value = ''

  try {
    const r = await call<{ id: string; name: string }>('openWareHouseEdit', { wid: id })

    if (!r?.id) {
      error.value = t('wh.e.gone')
      return
    }

    name.value = r.name
    loaded.value = true
    await reload()
  } catch (e) {
    console.error('[wh.e] 打开失败', e)
    error.value = String(e)
  }
}, { immediate: true })

function close(): void {
  emit('close')
}

/* ── 仓储数据 ───────────────────────────────────────────────── */

async function reload(): Promise<void> {
  if (props.id === null) return

  try {
    const r = await call<{ rows: StoreRow[] }>('getStoreRows', { wid: props.id })
    rows.value = r?.rows ?? []

    /*
      整表换掉了（重排 / 删除 / 导入之后都会走这里）——
      预览是按下标缓存的，必须一起作废，否则会把上一份的预览贴到新顺序上。
    */
    gotPreview.clear()
    void fillPreviews()

    //整表换掉后对一次选中集，理由与各列表一屏相同
    const alive = new Set(rows.value.map((x) => x.Id))
    const next = new Set<string>()
    for (const id of picked.value) { if (alive.has(id)) next.add(id) }
    picked.value = next
  } catch (e) {
    console.error('[wh.e] 取仓储数据失败', e)
  }
}

/*
  ── 预览按可见窗口取 ─────────────────────────────────────────

  getStoreRows 出的行<b>不带预览</b>：那一列是 60 字节的十六进制、约 180 个字符，
  占整条报文的四分之三（50000 条实测：带预览 12.6 MB、不带 3.4 MB）。
  行本身还是要全给 —— 虚拟滚动、Shift 连选、全选、按 Id 发给 C# 的批量动作都要完整 Id 序列。

  按<b>定长块</b>取而不是按精确窗口：滚动时窗口每帧都在变，按块取才有得缓存，
  也不会一屏发出几十个请求。块比一屏大，正常滚动一次最多取两块。
*/
const BLOCK = 200
const gotPreview = new Set<number>()

async function fillPreviews(): Promise<void> {
  if (props.id === null) return

  const first = Math.floor(start.value / BLOCK)
  const last = Math.floor(Math.max(start.value, end.value - 1) / BLOCK)

  for (let b = first; b <= last; b++) {
    if (gotPreview.has(b)) continue

    //先占位再取：同一块的第二次滚动不要重复发
    gotPreview.add(b)

    try {
      const r = await call<{ items: string[] }>('getStorePreviews',
        { wid: props.id, from: b * BLOCK, count: BLOCK })

      const items = r?.items ?? []
      const base = b * BLOCK

      for (let i = 0; i < items.length; i++) {
        const row = rows.value[base + i]

        //rows 是 ref（深响应），就地改这一格就会触发重渲染
        if (row) row.Preview = items[i]
      }
    } catch (e) {
      //取失败就把占位撤掉，滚回来还有机会重试
      gotPreview.delete(b)
      console.error('[wh.e] 取预览失败', e)
    }
  }
}

//单击 / Ctrl / Shift 多选，全项目一份实现，见 usePick.ts
const { picked, pickedIds, onRowClick, selectAll, clear } = useRowPick(rows, (r) => r.Id)

/*
  ── 虚拟滚动 ───────────────────────────────────────────────────
  ROW_H <b>必须与 .row2 的 height 逐像素一致</b>，对不上会随行数线性漂移且不报错。
*/
const ROW_H = 30
const OVERSCAN = 8

const scroller = ref<HTMLElement | null>(null)
const scrollTop = ref(0)
const viewH = ref(400)

const total = computed(() => rows.value.length)
const start = computed(() => Math.max(0, Math.floor(scrollTop.value / ROW_H) - OVERSCAN))

const end = computed(() =>
  Math.min(total.value, Math.ceil((scrollTop.value + viewH.value) / ROW_H) + OVERSCAN))

const windowRows = computed(() => rows.value.slice(start.value, end.value))

function onScroll(): void {
  const el = scroller.value
  if (el) scrollTop.value = el.scrollTop

  //滚到哪儿补哪儿的预览。已取过的块直接跳过，不产生往返
  void fillPreviews()
}

let ro: ResizeObserver | null = null

//表是 v-if 出来的（弹窗关着时不渲染），元素换了就要重接一次
watch(scroller, (el) => {
  ro?.disconnect()
  ro = null
  if (!el) return

  viewH.value = el.clientHeight
  ro = new ResizeObserver(() => { viewH.value = el.clientHeight })
  ro.observe(el)
})

onBeforeUnmount(() => { ro?.disconnect() })

/* ── 工具条 ─────────────────────────────────────────────────── */

/** 导入(8) / 导出全部(5) / 清空(7)。 */
async function command(action: number): Promise<void> {
  if (props.id === null) return

  try {
    await call('storesCommand', { wid: props.id, action })
    await reload()
  } catch (e) {
    console.error('[wh.e] 工具条操作失败', e)
  }
}

async function copyHex(): Promise<void> {
  if (props.id === null) return

  if (picked.value.size === 0) {
    pushToast('warning', t('lst.needPick'))
    return
  }

  try {
    const r = await call<{ text: string }>('copyStoresHex', { wid: props.id, ids: pickedIds.value })

    if (!r?.text) { pushToast('warning', t('pm.copyFail')); return }

    //写剪贴板走 C#（与封包列表的「复制」同一条路），不用 navigator.clipboard
    await call('clipboardWrite', { text: r.text })
    pushToast('success', t('pm.copied'))
  } catch (e) {
    console.error('[wh.e] 复制失败', e)
    pushToast('error', String(e))
  }
}

/* ── 右键菜单 ───────────────────────────────────────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  const n = picked.value.size
  const tag = n ? ' (' + n + ')' : ''

  return [
    { id: 'top', label: t('lst.top') + tag, icon: ICON.top },
    { divider: true },
    { id: 'up', label: t('lst.up') + tag, icon: ICON.up },
    { id: 'down', label: t('lst.down') + tag, icon: ICON.down },
    { divider: true },
    { id: 'bottom', label: t('lst.bottom') + tag, icon: ICON.bottom },
    { divider: true },
    { id: 'copyHex', label: t('wh.e.copyHex') + tag, icon: ICON.hex },
    { id: 'export', label: t('wh.e.exportPicked') + tag, icon: ICON.save },
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
  if (id === 'copyHex') { await copyHex(); return }

  //一处拦住就够了（加菜单项时最容易漏这句）
  if (picked.value.size === 0) {
    pushToast('warning', t('lst.needPick'))
    return
  }

  const action = ACTION_OF[id]
  if (action === undefined || props.id === null) return

  try {
    await call('storesAction', { wid: props.id, action, ids: pickedIds.value })
    await reload()
  } catch (e) {
    console.error('[wh.e] 列表操作失败', e)
  }
}

/* ── 保存 ───────────────────────────────────────────────────── */

async function save(): Promise<void> {
  if (props.id === null) return

  busy.value = true
  error.value = ''

  try {
    const r = await call<{ error: string }>('saveWareHouseName', { wid: props.id, name: name.value })

    if (r?.error) { error.value = r.error; return }

    close()
  } catch (e) {
    console.error('[wh.e] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

/* 登记进模态栈：父窗体因此变 inert；自己被后开的弹窗盖住时也会 inert。见 useModal.ts */
const { covered } = useModal(() => props.id !== null)
</script>

<template>
  <!--
    ⚠️ <b>Teleport 到 body</b> —— 不是为了好看，是必须的，两个理由都在 useModal.ts 里：
    ① 代理模式的 .proxy 是 z-index: 10 的层叠上下文，弹窗留在里面时遮罩盖不住标题栏；
    ② 出去了才不会被 .shell 的 inert 一起禁掉。

    ⚠️ <b>刻意不换行、不重排缩进</b>：模板里有 white-space: pre 的块，
    整体缩进一动，Vue 模板编译器的 condense 会连带改掉渲染结果。

    ⚠️ <b>点遮罩不再关闭弹窗</b>：编辑器里都是填了一半的东西，点空白处就丢掉太容易误操作。
    出口只留「取消 / 关闭」按钮与 Esc。
  -->
  <Teleport to="body"><div v-if="props.id !== null" class="editor-mask" :inert="covered">
    <div class="dlg" role="dialog" aria-modal="true" @keydown.esc="close">
      <span class="mk tl" /><span class="mk tr" /><span class="mk bl" /><span class="mk br" />

      <header class="hd">
        <div class="tt">
          <span class="zh">{{ t('wh.e.title') }}</span>
          <span class="sub">Controls/WareHouseEdit</span>
        </div>
        <button class="x" :title="t('dlg.cancel')" @click="close">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </header>

      <div v-if="!loaded" class="loading">{{ error || t('proxy.working') }}</div>

      <div v-else class="bd">
        <div class="row">
          <div class="k">{{ t('col.wareHouseName') }}</div>
          <div class="v">
            <input v-model="name" class="inp" spellcheck="false" maxlength="100"
                   :placeholder="t('wh.e.namePh')" @keydown.enter="save">
          </div>
        </div>

        <div class="runbar">
          <span class="cap">{{ t('wh.e.stores') }} <b>{{ rows.length }}</b></span>

          <span class="grow" />

          <button class="btn" :disabled="!picked.size" @click="copyHex">
            {{ t('wh.e.copyHex') }}{{ picked.size ? ' (' + picked.size + ')' : '' }}
          </button>
          <span class="sep" />
          <button class="btn" @click="command(8)">{{ t('wh.e.import') }}</button>
          <button class="btn" :disabled="!rows.length" @click="command(5)">{{ t('wh.e.export') }}</button>
          <button class="btn danger" :disabled="!rows.length" @click="command(7)">{{ t('wh.e.clear') }}</button>
        </div>

        <!-- 仓储数据表 -->
        <div class="tbl list-page">
          <div ref="scroller" class="tbody" @scroll.passive="onScroll">
            <!-- 表头在滚动容器里面（sticky），与行共用同一条滚动条，列才对得齐 -->
            <div class="head">
              <span class="no">{{ t('col.id') }}</span>
              <span class="len">{{ t('col.len') }}</span>
              <span class="dt">{{ t('col.data') }}</span>
            </div>

            <div v-if="!rows.length" class="empty">{{ t('wh.e.empty') }}</div>

            <div v-else class="spacer" :style="{ height: total * ROW_H + 'px' }">
              <div class="win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
                <div
                  v-for="(r, i) in windowRows"
                  :key="i"
                  class="row2"
                  :class="{ sel: picked.has(r.Id) }"
                  @click="onRowClick(r, $event, start + i)"
                  @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }"
                >
                  <span class="no">{{ start + i + 1 }}</span>
                  <span class="len">{{ r.Len }}</span>
                  <span class="dt">{{ r.Preview }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <p class="hint">{{ t('wh.e.liveHint') }}</p>
      </div>

      <footer class="ft">
        <span v-if="loaded && error" class="err">{{ error }}</span>
        <span class="grow" />
        <button class="btn" :disabled="busy" @click="close">{{ t('dlg.cancel') }}</button>
        <button class="btn primary" :disabled="busy || !loaded" @click="save">
          {{ busy ? t('proxy.working') : t('set.save') }}
        </button>
      </footer>

      <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />
    </div>
  </div></Teleport>
</template>

<style scoped>
.hd {
  flex: none;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 14px 18px;
  border-bottom: 1px solid var(--border);
  background: var(--panel);
}

.tt { flex: 1; min-width: 0; display: flex; align-items: baseline; gap: 12px; }
.tt .zh { font-family: var(--orbit); font-weight: 700; font-size: var(--fs-title); color: var(--gray); letter-spacing: .04em; }
.tt .sub { font-family: var(--share); font-size: var(--fs-caption); letter-spacing: .14em; text-transform: uppercase; color: var(--dim); }

.x {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 26px;
  height: 26px;
  background: transparent;
  border: 1px solid transparent;
  color: var(--muted);
  cursor: pointer;
}

.x:hover { border-color: var(--danger); color: var(--danger); }
.x .ico { width: 15px; height: 15px; stroke: currentColor; stroke-width: 2; fill: none; }

.loading { padding: 60px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

/* 名称行与工具条定高，表与十六进制吃掉剩下的全部高度 */
.bd {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  padding: 10px 0 10px;
}

.bd > .row,
.bd > .runbar,
.bd > .hint { flex: none; }

.row {
  display: grid;
  grid-template-columns: 92px 1fr;
  align-items: center;
  gap: 12px;
  padding: 3px 18px;
  min-height: 30px;
}

.row > .k { font-size: var(--fs-body); color: var(--muted); }
.row > .v { display: flex; align-items: center; gap: 12px; min-width: 0; }

/* 基样式在 style.css 的 .inp，这里只补布局 */
.inp { flex: 1; min-width: 0; }

.runbar {
  display: flex;
  align-items: center;
  gap: 10px;
  margin: 8px 18px 6px;
  padding: 7px 12px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 20%);
}

.grow { flex: 1; }
.sep { width: 1px; height: 16px; background: var(--border); }

.runbar .cap { font-size: var(--fs-label); color: var(--muted); font-family: var(--share); letter-spacing: .06em; }
.runbar .cap b { font-family: var(--mono); font-variant-numeric: tabular-nums; color: var(--cyan); }

.btn {
  flex: none;
  padding: 8px 13px 8px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
  white-space: nowrap;
}

.btn:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.btn:disabled { opacity: .35; cursor: default; }
.btn.primary { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.primary:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); }
.btn.danger { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }
.btn.danger:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); }

/* 仓储数据表：吃掉名称行与工具条之外的全部高度 */
.tbl {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  margin: 0 18px;
  border: 1px solid var(--border);
  background: var(--sink);
}

/* 表头在 .tbody 里 sticky（style.css 的 .list-page .head 已经是 sticky + top: 0）*/
.tbody { flex: 1; min-height: 0; overflow: auto; overflow-anchor: none; }

.spacer { position: relative; }
.win { position: absolute; top: 0; left: 0; right: 0; will-change: transform; }

.head,
.row2 {
  display: grid;
  grid-template-columns: 48px 56px minmax(120px, 1fr);
  align-items: center;
  gap: 8px;
  /* ⚠️ 必须与 style.css 里 .list-page .head 的 14px 一致，否则表头比内容错 4px（发送编辑栽过）*/
  padding: 0 14px;
  font-size: var(--fs-body);
}

.row2 {
  height: 30px;   /* 必须与 ROW_H 一致 */
  border-bottom: 1px solid rgb(var(--border-rgb) / 45%);
  color: var(--soft);
  cursor: default;
}

.row2:hover { background: rgb(var(--tint-rgb) / 4%); }
.row2.sel { background: rgb(var(--cyan-rgb) / 6%); box-shadow: inset 2px 0 0 var(--cyan); }

.row2 > span,
.head > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.head > span,
.row2 > span { text-align: center; }

.head > span.dt,
.row2 > span.dt { text-align: left; }

.head > span.len,
.row2 > span.len { text-align: right; }

.head > span.dt,
.row2 > span.dt { padding-left: 10px; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.len { color: var(--cyan); font-variant-numeric: tabular-nums; }
.dt { color: var(--acc-green2); font-family: var(--mono); font-size: var(--fs-body); }

.empty { padding: 40px 20px; text-align: center; color: var(--muted); font-size: var(--fs-body); line-height: 1.8; }

.hint { margin: 8px 18px 0; font-size: var(--fs-small); color: var(--dim2); }

.ft {
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 12px 18px;
  border-top: 1px solid var(--border);
  background: var(--panel);
}

.err { font-size: var(--fs-small); color: var(--danger); }
</style>
