<script setup lang="ts">
/*
  十六进制视图 / 编辑器 —— 全项目唯一的一份。

  【谁在用】代理数据页的十六进制面板（只读，带「改写前 / 改写后」的差异高亮）、
  封包编辑（可编辑）。将来「查看数据修改」（原始 vs 改写后）、仓库 / 发送集看字节也都是它。
  外观以代理数据页那块为模板：同一份列号表头（hex.ts 的 headerLine）、每行字节数按宽度自适应、
  偏移灰 / 正文亮 / 差异琥珀。

  【职责边界】
  · 字节由父组件持有，这里不留副本：可编辑模式下每次改动都 emit('update:bytes', 新数组)，
    父组件 v-model:bytes 接住。64KB 的包一次键入复制一份是微秒级，换来的是「谁改的、改成了什么」
    只有一个出口，父组件保存 / 发送时拿到的永远是最新的。
  · 光标 / 选区 / 栏（十六进制或文本）/ 插入模式是这里的状态，通过 defineExpose 给父组件读
    （标题栏要显示它们），cursor 变化另外 emit 一次（封包编辑的递进位置跟着光标走）。
  · 右键菜单内建：剪切 / 复制文本 / 复制十六进制 / 粘贴文本 / 粘贴十六进制 / 全选（只读时只剩复制与全选）；
    父组件要加的项（添加到滤镜 / 添加到发送）从 extraItems 传进来、排在最前，点了 emit('pick', id)。
  · 剪贴板走桥（clipboardRead / clipboardWrite），理由见 ShellForm 里那段说明。

  【渲染】每个字节一个格子、行定高 18px、虚拟滚动 —— 64KB 是 4096 行，全铺开是 13 万个格子。
  早先只读那块是整包排成一个 <pre>（靠原生文本选择复制），合并后统一成格子：
  原生选择会把偏移列和字符栏一起复制出去，几乎从来不是用户想要的；格子模型下复制的只有字节。

  【光标模型】字节下标 + 高低半字节，另有一个落在末尾（= 长度）的追加位。
  覆盖模式默认（同 HexBox），Insert 切换；末尾键入永远是追加。点十六进制区按十六进制键入、
  点文本区按字符键入，Tab 切栏。选区是半开区间 [lo, hi)，Shift + 方向键 / 拖动都能选；
  复制 / 剪切要求先选中（HexBox 的 CanCopy）；粘贴永远是插入。
*/
import { computed, onBeforeUnmount, ref, shallowRef, watch } from 'vue'
import { call } from '../bridge'
import { headerLine, perLineFor, DEFAULT_PER_LINE, asciiOf } from '../hex'
import { t } from '../i18n'
import { pushToast } from '../stores/toast'
import ContextMenu from './ContextMenu.vue'
import { ICON, type MenuItem } from './menu'

const props = withDefaults(defineProps<{
  bytes: Uint8Array
  /** 对比用的另一份；给了就把不同的字节标成琥珀（长度不等的那一截也算不同） */
  compare?: Uint8Array | null
  readonly?: boolean
  /** 右键菜单最前面的额外项（父组件自己的动作），点了 emit('pick', id) */
  extraItems?: MenuItem[]
}>(), { compare: null, readonly: false, extraItems: () => [] })

const emit = defineEmits<{
  (e: 'update:bytes', v: Uint8Array): void
  (e: 'cursor', i: number): void
  (e: 'pick', id: string): void
}>()

/*
  本地工作副本。emit('update:bytes') 之后父组件要到下一次渲染才把新数组传回 props，
  连着两次键入（键盘连发、脚本派发）第二次就会读到旧数组 —— 探针页抓到的是「AB」打成「2B」。
  所以编辑一律在 local 上做、改完立刻替换 local 再 emit；props 变了再同步回来。
*/
const local = shallowRef<Uint8Array>(props.bytes)
watch(() => props.bytes, (v) => { local.value = v })

const len = computed(() => local.value.length)

const HEX = '0123456789ABCDEF'
function hex2(v: number): string { return HEX[v >> 4] + HEX[v & 15] }
//字符栏的规则在 hex.ts 的 asciiOf 里（照抄 WinForms 那个控件的 converter），别在这儿另写一份
function asc(v: number): string { return asciiOf(v) }
function offset(i: number): string { return i.toString(16).toUpperCase().padStart(8, '0') }

/** 十六进制串 → 字节。空白 / 0x / 逗号都容忍；非法或奇数长度返回 null。 */
function parseHex(s: string): Uint8Array | null {
  const clean = (s || '').replace(/0x/gi, '').replace(/[\s,;:-]+/g, '')
  if (!clean.length || clean.length % 2 || /[^0-9a-f]/i.test(clean)) return null
  const out = new Uint8Array(clean.length / 2)
  for (let i = 0; i < out.length; i++) out[i] = parseInt(clean.substr(i * 2, 2), 16)
  return out
}

/** 文本 → 字节：每个字符取低 8 位，与 HexBox 的 DefaultByteCharConverter 同一种映射。 */
function textToBytes(s: string): Uint8Array {
  const out = new Uint8Array(s.length)
  for (let i = 0; i < s.length; i++) out[i] = s.charCodeAt(i) & 0xff
  return out
}

/* ── 每行字节数：按宽度自适应，字符宽度实测 ─────────────────── */

const scroller = ref<HTMLElement | null>(null)
const per = ref(DEFAULT_PER_LINE)
let charW = 0
let ro: ResizeObserver | null = null

function measureCharWidth(el: HTMLElement): number {
  const probe = document.createElement('span')
  const cs = getComputedStyle(el)
  probe.style.cssText = 'position:absolute;visibility:hidden;white-space:pre;'
  probe.style.font = cs.font
  probe.style.fontFamily = cs.fontFamily
  probe.style.fontSize = cs.fontSize
  probe.textContent = '0'.repeat(100)
  el.appendChild(probe)
  const w = probe.getBoundingClientRect().width / 100
  el.removeChild(probe)
  return w
}

function recalc(): void {
  const el = scroller.value
  if (!el) return
  viewH.value = el.clientHeight
  if (!charW) charW = measureCharWidth(el)
  if (!charW) return
  const cs = getComputedStyle(el)
  const pad = parseFloat(cs.paddingLeft || '0') + parseFloat(cs.paddingRight || '0')
  per.value = perLineFor((el.clientWidth - pad) / charW)
}

watch(scroller, (el) => {
  ro?.disconnect()
  ro = null
  charW = 0
  if (!el) return
  recalc()
  ro = new ResizeObserver(recalc)
  ro.observe(el)
})

onBeforeUnmount(() => ro?.disconnect())

/* ── 光标 / 选区 ────────────────────────────────────────────── */

const cur = ref(0)            //字节下标，可以等于 len（追加位）
const nib = ref<0 | 1>(0)     //高 / 低半字节
const col = ref<'hex' | 'asc'>('hex')
const insertMode = ref(false)
/** 选区半开区间 */
const sel = ref<{ lo: number; hi: number } | null>(null)
let anchor = 0

const hasSel = computed(() => !!sel.value && sel.value.hi > sel.value.lo)
const selCount = computed(() => (sel.value ? sel.value.hi - sel.value.lo : 0))

function clampCur(i: number): number { return Math.max(0, Math.min(len.value, i)) }

function place(i: number, extend: boolean): void {
  i = clampCur(i)
  cur.value = i
  nib.value = 0
  if (extend) {
    sel.value = anchor === i ? null : { lo: Math.min(anchor, i), hi: Math.max(anchor, i) }
  } else {
    anchor = i
    sel.value = null
  }
  if (i < len.value) emit('cursor', i)
  scrollIntoView(i)
}

//字节换了一份（父组件重新打开、或整包被替换）：光标别悬在包外
watch(local, () => {
  if (cur.value > len.value) { cur.value = len.value; nib.value = 0 }
  if (sel.value && sel.value.hi > len.value) sel.value = null
})

/* ── 虚拟滚动 ───────────────────────────────────────────────── */

const ROW_H = 18          //12px × 1.5 行距
const HEAD_H = 27         //列号表头 8 + 18 + 1
const OVERSCAN = 6

const scrollTop = ref(0)
const viewH = ref(300)

//多一行给末尾的追加位（长度正好是整行时它落在新一行）；只读时不需要
const rows = computed(() => Math.floor(len.value / per.value) + (props.readonly && len.value % per.value === 0 && len.value > 0 ? 0 : 1))
const start = computed(() => Math.max(0, Math.floor(scrollTop.value / ROW_H) - OVERSCAN))
const end = computed(() => Math.min(rows.value, Math.ceil((scrollTop.value + viewH.value) / ROW_H) + OVERSCAN))

const windowRows = computed(() => {
  const out: number[] = []
  for (let r = start.value; r < end.value; r++) out.push(r)
  return out
})

function onScroll(): void {
  const el = scroller.value
  if (el) scrollTop.value = el.scrollTop
}

function scrollIntoView(i: number): void {
  const el = scroller.value
  if (!el) return
  const top = HEAD_H + Math.floor(i / per.value) * ROW_H
  if (top - HEAD_H < el.scrollTop) el.scrollTop = top - HEAD_H
  else if (top + ROW_H > el.scrollTop + el.clientHeight) el.scrollTop = top + ROW_H - el.clientHeight
}

function rowIdx(r: number): number[] {
  const out: number[] = []
  const from = r * per.value, to = Math.min(len.value, from + per.value)
  for (let i = from; i < to; i++) out.push(i)
  return out
}

function inSel(i: number): boolean {
  const s = sel.value
  return !!s && i >= s.lo && i < s.hi
}

/** 与对比份不同的字节（长度不等的那一截也算） */
function differs(i: number): boolean {
  const c = props.compare
  if (!c) return false
  return i >= c.length || c[i] !== local.value[i]
}

/** 最后一行末尾补的空格数，字符栏才对得齐 */
function tailPad(): string {
  const rest = len.value % per.value
  const used = rest + (props.readonly ? 0 : 1)   //可编辑时多一格追加位
  return ' '.repeat(Math.max(0, per.value - used) * 3)
}

/* ── 鼠标 ───────────────────────────────────────────────────── */

let dragging = false
const root = ref<HTMLElement | null>(null)

function idxOf(e: Event): number | null {
  const el = (e.target as HTMLElement | null)?.closest<HTMLElement>('[data-i]')
  if (!el) return null
  const i = Number(el.dataset.i)
  return Number.isFinite(i) ? i : null
}

function onMouseDown(e: MouseEvent): void {
  if (e.button !== 0) return
  const i = idxOf(e)
  if (i === null) return
  const c = (e.target as HTMLElement).closest<HTMLElement>('[data-col]')
  col.value = c?.dataset.col === 'asc' ? 'asc' : 'hex'
  place(i, e.shiftKey)
  dragging = true
  root.value?.focus()
  e.preventDefault()
}

function onMouseMove(e: MouseEvent): void {
  if (!dragging) return
  const i = idxOf(e)
  if (i === null) return
  //拖到某个字节上：选到它（含），所以锚点在前时 hi 要 +1
  const a = anchor
  const lo = Math.min(a, i), hi = Math.max(a, i)
  sel.value = { lo, hi: a <= i ? Math.min(len.value, hi + 1) : hi }
  cur.value = i
  nib.value = 0
}

function onMouseUp(): void { dragging = false }

/* ── 编辑（全部走 emit，本组件不持有字节）───────────────────── */

function replaceRange(lo: number, hi: number, ins: Uint8Array): void {
  const old = local.value
  const out = new Uint8Array(old.length - (hi - lo) + ins.length)
  out.set(old.subarray(0, lo), 0)
  out.set(ins, lo)
  out.set(old.subarray(hi), lo + ins.length)
  local.value = out
  emit('update:bytes', out)
}

function setByte(i: number, v: number): void {
  const out = local.value.slice()
  out[i] = v
  local.value = out
  emit('update:bytes', out)
}

function deleteSel(): boolean {
  const s = sel.value
  if (!s || s.hi <= s.lo) return false
  replaceRange(s.lo, s.hi, new Uint8Array(0))
  sel.value = null
  anchor = s.lo
  cur.value = clampCur(s.lo)
  nib.value = 0
  return true
}

function typeHex(v: number): void {
  if (hasSel.value) deleteSel()
  const i = cur.value
  if (i >= len.value || (insertMode.value && nib.value === 0)) {
    replaceRange(i, i, new Uint8Array([v << 4]))
    cur.value = i
    nib.value = 1
    return
  }
  const b = local.value[i]
  if (nib.value === 0) {
    setByte(i, (v << 4) | (b & 0x0f))
    nib.value = 1
  } else {
    setByte(i, (b & 0xf0) | v)
    nib.value = 0
    cur.value = i + 1
  }
  scrollIntoView(cur.value)
}

function typeChar(v: number): void {
  if (hasSel.value) deleteSel()
  const i = cur.value
  if (i >= len.value || insertMode.value) replaceRange(i, i, new Uint8Array([v]))
  else setByte(i, v)
  cur.value = i + 1
  nib.value = 0
  scrollIntoView(cur.value)
}

function selectAll(): void {
  if (!len.value) return
  anchor = 0
  sel.value = { lo: 0, hi: len.value }
  cur.value = len.value
  nib.value = 0
}

/** 选中 [lo, hi) 并滚过去 —— 「查看数据修改」的差异表点一行跳到那一段。 */
function selectRange(lo: number, hi: number): void {
  lo = Math.max(0, Math.min(len.value, lo))
  hi = Math.max(lo, Math.min(len.value, hi))
  anchor = lo
  sel.value = hi > lo ? { lo, hi } : null
  cur.value = lo
  nib.value = 0
  scrollIntoView(lo)
}

/** 选中那段（没选就是整包）。父组件「添加到滤镜 / 发送」用。 */
function selectedBytes(): Uint8Array {
  const s = sel.value
  return s && s.hi > s.lo ? local.value.slice(s.lo, s.hi) : local.value.slice()
}

function onKey(e: KeyboardEvent): void {
  const ctrl = e.ctrlKey || e.metaKey
  const ro = props.readonly

  if (ctrl) {
    const k = e.key.toLowerCase()
    if (k === 'a') { e.preventDefault(); selectAll(); return }
    if (k === 'c') { e.preventDefault(); void copyHex(); return }
    if (!ro && k === 'x') { e.preventDefault(); void cut(); return }
    if (!ro && k === 'v') { e.preventDefault(); void pasteHex(); return }
    if (k === 'home') { e.preventDefault(); place(0, e.shiftKey); return }
    if (k === 'end') { e.preventDefault(); place(len.value, e.shiftKey); return }
    return
  }

  const page = per.value * Math.max(1, Math.floor(viewH.value / ROW_H) - 1)

  switch (e.key) {
    case 'ArrowLeft': e.preventDefault(); place(cur.value - 1, e.shiftKey); return
    case 'ArrowRight': e.preventDefault(); place(cur.value + 1, e.shiftKey); return
    case 'ArrowUp': e.preventDefault(); place(cur.value - per.value, e.shiftKey); return
    case 'ArrowDown': e.preventDefault(); place(cur.value + per.value, e.shiftKey); return
    case 'Home': e.preventDefault(); place(cur.value - (cur.value % per.value), e.shiftKey); return
    case 'End': e.preventDefault(); place(Math.min(len.value, cur.value - (cur.value % per.value) + per.value - 1), e.shiftKey); return
    case 'PageUp': e.preventDefault(); place(cur.value - page, e.shiftKey); return
    case 'PageDown': e.preventDefault(); place(cur.value + page, e.shiftKey); return
    case 'Tab': e.preventDefault(); col.value = col.value === 'hex' ? 'asc' : 'hex'; return
    case 'Escape': return   //交给弹窗
  }

  if (ro) return

  switch (e.key) {
    case 'Insert': e.preventDefault(); insertMode.value = !insertMode.value; return
    case 'Backspace':
      e.preventDefault()
      if (deleteSel()) return
      if (nib.value === 1) { nib.value = 0; return }   //刚写了高位，退回来不删字节
      if (cur.value > 0) { replaceRange(cur.value - 1, cur.value, new Uint8Array(0)); cur.value--; anchor = cur.value }
      return
    case 'Delete':
      e.preventDefault()
      if (deleteSel()) return
      if (cur.value < len.value) { replaceRange(cur.value, cur.value + 1, new Uint8Array(0)); nib.value = 0 }
      return
  }

  if (e.key.length !== 1) return

  if (col.value === 'hex') {
    const v = parseInt(e.key, 16)
    if (Number.isNaN(v)) return
    e.preventDefault()
    typeHex(v)
  } else {
    const code = e.key.charCodeAt(0)
    if (code > 0xff) return
    e.preventDefault()
    typeChar(code)
  }
}

/* ── 剪贴板 ─────────────────────────────────────────────────── */

async function writeClip(text: string): Promise<void> {
  await call('clipboardWrite', { text })
  pushToast('success', t('pm.copied'))
}

async function copyHex(): Promise<void> {
  if (!hasSel.value) return
  const b = selectedBytes()
  let s = ''
  for (let i = 0; i < b.length; i++) s += (i ? ' ' : '') + hex2(b[i])
  await writeClip(s)
}

async function copyText(): Promise<void> {
  if (!hasSel.value) return
  const b = selectedBytes()
  let s = ''
  for (let i = 0; i < b.length; i++) s += String.fromCharCode(b[i])
  await writeClip(s)
}

async function cut(): Promise<void> {
  if (props.readonly || !hasSel.value) return
  await copyHex()
  deleteSel()
}

async function pasteBytes(data: Uint8Array): Promise<void> {
  if (hasSel.value) deleteSel()
  const i = cur.value
  replaceRange(i, i, data)
  cur.value = i + data.length
  anchor = cur.value
  nib.value = 0
  scrollIntoView(cur.value)
}

async function readClip(): Promise<string> {
  const r = await call<{ text: string }>('clipboardRead')
  return r?.text ?? ''
}

async function pasteHex(): Promise<void> {
  if (props.readonly) return
  const s = await readClip()
  if (!s.trim()) { pushToast('warning', t('pe.pasteEmpty')); return }
  const b = parseHex(s)
  if (!b) { pushToast('warning', t('pe.pasteBadHex')); return }
  await pasteBytes(b)
}

async function pasteText(): Promise<void> {
  if (props.readonly) return
  const s = await readClip()
  if (!s) { pushToast('warning', t('pe.pasteEmpty')); return }
  await pasteBytes(textToBytes(s))
}

/* ── 右键菜单 ───────────────────────────────────────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  const n = selCount.value
  const tag = n ? ' (' + n + ')' : ''
  const extra = props.extraItems.length ? [...props.extraItems, { divider: true } as MenuItem] : []
  const edit: MenuItem[] = props.readonly ? [] : [
    { id: 'cut', label: t('pe.m.cut') + tag, icon: ICON.cut, disabled: !n },
  ]
  const paste: MenuItem[] = props.readonly ? [] : [
    { id: 'pasteText', label: t('pe.m.pasteText'), icon: ICON.pasteText },
    { id: 'pasteHex', label: t('pe.m.pasteHex'), icon: ICON.pasteHex },
  ]
  return [
    ...extra,
    ...edit,
    { id: 'copyText', label: t('pe.m.copyText') + tag, icon: ICON.text, disabled: !n },
    { id: 'copyHex', label: t('pe.m.copyHex') + tag, icon: ICON.hex, disabled: !n },
    ...paste,
    { divider: true },
    { id: 'selectAll', label: t('pe.m.selectAll'), icon: ICON.list, disabled: !len.value },
  ]
})

async function onMenuPick(id: string): Promise<void> {
  try {
    switch (id) {
      case 'cut': await cut(); return
      case 'copyText': await copyText(); return
      case 'copyHex': await copyHex(); return
      case 'pasteText': await pasteText(); return
      case 'pasteHex': await pasteHex(); return
      case 'selectAll': selectAll(); return
      default: emit('pick', id)
    }
  } catch (e) {
    console.error('[hex] ' + id + ' 失败', e)
    pushToast('error', String(e))
  }
}

function openMenu(e: MouseEvent): void {
  if (!len.value && props.readonly) return
  menuAt.value = { x: e.clientX, y: e.clientY }
}

defineExpose({ cur, nib, col, insertMode, hasSel, selCount, per, selectAll, selectedBytes, selectRange })
</script>

<template>
  <div
    ref="root"
    class="hexview"
    :class="{ ro: readonly }"
    tabindex="0"
    @keydown="onKey"
    @mousedown="onMouseDown"
    @mousemove="onMouseMove"
    @mouseup="onMouseUp"
    @mouseleave="onMouseUp"
    @contextmenu.prevent="openMenu"
  >
    <!-- 列号表头与正文同在一个滚动容器里：sticky 只锁纵向，横向跟着一起滚 -->
    <div ref="scroller" class="hx-scroll" @scroll.passive="onScroll">
      <div class="hx-head">{{ headerLine(per, t('hex.charCol')) }}</div>

      <div class="spacer" :style="{ height: rows * ROW_H + 'px' }">
        <div class="win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
          <div v-for="r in windowRows" :key="r" class="hrow">
            <!-- 偏移栏定宽 10ch（8 位 + 两个空格），与 headerLine 的前 10 个字符对齐 -->
            <span class="off">{{ offset(r * per) }}</span>
            <span class="hexs" data-col="hex">
              <span v-for="i in rowIdx(r)" :key="i" class="b" :data-i="i"
                    :class="{ d: differs(i), sel: inSel(i), cur: !readonly && i === cur && col === 'hex', hi: !readonly && i === cur && nib === 1 }">{{ hex2(local[i]) }} </span>
              <span v-if="!readonly && r === rows - 1" class="b end" :data-i="len" :class="{ cur: cur === len && col === 'hex' }">   </span>
              <span v-if="r === rows - 1" class="pad">{{ tailPad() }}</span>
            </span>
            <span class="ascs" data-col="asc">
              <span v-for="i in rowIdx(r)" :key="i" class="c" :data-i="i"
                    :class="{ d: differs(i), sel: inSel(i), cur: !readonly && i === cur && col === 'asc' }">{{ asc(local[i]) }}</span>
              <span v-if="!readonly && r === rows - 1" class="c end" :data-i="len" :class="{ cur: cur === len && col === 'asc' }"> </span>
            </span>
          </div>
        </div>
      </div>

      <div v-if="!len" class="empty"><slot name="empty">{{ readonly ? t('hex.pick') : t('pe.empty') }}</slot></div>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />
  </div>
</template>

<style scoped>
/* 外框（边、底色）归父组件；这里只管排版 */
.hexview {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  outline: none;
  /* 只读时整块都是 default；可编辑时只有下面 .spacer 那片格子给 I 形 */
  cursor: default;
}

/*
  ⚠️ <b>cursor 不能写在这一层</b>（它会一路继承到 .hx-scroll，而<b>滚动条吃的正是
  滚动容器自己的 cursor</b>）—— 写在这儿的话，鼠标移到右边那条滚动条上仍是
  「工」字形的文本光标。只读时看不出来（那时本来就是 default），
  可编辑的封包编辑器里一眼就看得见。

  所以 I 形只给<b>真的能落光标的那片格子</b>（.spacer 覆盖全部行）；
  滚动条、内边距、列号表头一律 default。HexPanel 的文本视图是同一个病根、同一种分法。
*/
.hexview:not(.ro) .spacer { cursor: text; }
/*
  不画焦点环：光标块本身已经说明「键入会落到这里」，再套一圈框只是多一道线（用户嫌丑，去掉了）。
  全局的绿色焦点环也要关掉，它画在盒子外沿、被外框裁得只剩上面一条。
*/
.hexview:focus-visible { outline: none; }

.hx-scroll {
  flex: 1;
  min-height: 0;
  overflow: auto;
  overflow-anchor: none;
  position: relative;
  padding: 0 12px 10px;
  font-family: Consolas, 'Cascadia Mono', monospace;
  font-size: var(--fs-dense);
  line-height: 1.5;
}

/*
  列号表头。sticky 让它在纵向滚动时钉住，横向仍跟着正文一起滚。
  width: max-content 是必须的：sticky 元素默认只有容器那么宽，横向滚出去之后右半截会露出空白背景。
  下内边距留 1px 而不是 0：与正文严丝合缝时某些缩放比下会因亚像素取整露出一条正文的顶边。
*/
.hx-head {
  position: sticky;
  top: 0;
  z-index: 1;
  width: max-content;
  min-width: 100%;
  padding: 8px 0 1px;
  /* 滚动容器是透明的，inherit 会让表头透出下面的正文；父组件的外框色通过 --hexview-bg 传进来 */
  background: var(--hexview-bg, var(--card));
  white-space: pre;
  color: var(--muted);
}

.spacer { position: relative; }
.win { position: absolute; top: 0; left: 0; right: 0; will-change: transform; }

.hrow {
  display: flex;
  height: 18px;   /* 必须与 ROW_H 一致（12px × 1.5 行距）*/
  white-space: pre;
  color: var(--gray);
}

.off { flex: none; width: 10ch; color: var(--muted); }
.hexs, .ascs { flex: none; display: flex; }
.ascs { margin-left: 1ch; }
.pad { flex: none; }

/* 每个字节 "XX " 三格；字符栏一格 */
.b { display: inline-block; width: 3ch; }
.c { display: inline-block; width: 1ch; }

/*
  与对比份不同的字节：琥珀而不是红 —— 红在这套皮肤里是「拦截 / 出错」，这里只是"这一位被动过"。
  底色只有 20%：密排的等宽字上实心底会连成色块，看不出边界。
*/
.b.d, .c.d { color: #f9d86f; background: rgb(var(--amber-rgb) / 20%); }

.b.sel, .c.sel { background: rgb(var(--cyan-rgb) / 18%); color: var(--gray); }

/* 光标：当前字节反白；写了高位还没写低位时只亮低位那一格。只读模式不画光标 —— 看的人没有「正在改哪一位」这回事，只留选区 */
.b.cur, .c.cur { background: var(--cyan); color: var(--on-accent); }
.b.cur.hi { background: linear-gradient(90deg, transparent 0 1ch, var(--cyan) 1ch 2ch, transparent 2ch); color: var(--cyan); }
.b.end.cur, .c.end.cur { background: transparent; box-shadow: inset 2px 0 0 var(--cyan); }

.empty {
  position: absolute;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--muted);
  font-family: var(--mono);
  font-size: var(--fs-body);
  pointer-events: none;
}
</style>
