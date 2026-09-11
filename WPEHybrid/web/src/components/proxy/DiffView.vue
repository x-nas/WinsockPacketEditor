<script setup lang="ts">
/*
  并排对齐视图 —— 文本对比页的主视图。

  ⚠️ <b>两栏装在同一个滚动容器里，不是两个各滚各的框。</b>
  这是这一版最要紧的一个决定：A / B 共用一套行，插入 / 删除的那一侧用占位格顶住，
  于是<b>横着扫永远对得齐</b>，也不需要「同步滚动」这种开关（两个独立滚动条同步永远差半行）。
  中缝那道色带也因此是免费的 —— 它就是同一行里的一个格子。

  三种排法：
    · hex   按<b>字节</b>对齐（十六进制模式）
    · text  按<b>行</b>对齐（文本模式）
    · plain 不对齐，两侧各按自己的偏移铺开（查重模式用 —— 那不是差异，是「共同片段」）

  虚拟滚动：行高写死，DOM 里永远只有一屏多一点。照 PacketList 那套。
*/
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { asciiOf, perLineFor, DEFAULT_PER_LINE } from '../../hex'
import type { DiffBlock, DiffOp } from '../../diff'

const props = withDefaults(defineProps<{
  mode: 'hex' | 'text' | 'plain'
  aBytes?: Uint8Array
  bBytes?: Uint8Array
  aLines?: string[]
  bLines?: string[]
  blocks?: DiffBlock[]
  /** 额外高亮（查重命中的片段），[起点, 长度] 的字节区间 */
  hlA?: Array<[number, number]>
  hlB?: Array<[number, number]>
  /*
    plain 模式下两侧头部各垫几个空格。

    查重的两侧<b>没有位置对应关系</b>（同一段可能在 A 的 2042、在 B 的 8147），
    垫一下就能把当前关心的那一处对齐到同一行，横着比。
    两个都是 0 时就是「各按自己的偏移铺开」。
  */
  padA?: number
  padB?: number
}>(), {
  padA: 0,
  padB: 0,
  aBytes: () => new Uint8Array(0),
  bBytes: () => new Uint8Array(0),
  aLines: () => [],
  bLines: () => [],
  blocks: () => [],
  hlA: () => [],
  hlB: () => [],
})

const emit = defineEmits<{ (e: 'view', start: number, end: number): void }>()

const ROW_H = 18
const HEX = '0123456789ABCDEF'
const hex2 = (v: number): string => HEX[v >> 4] + HEX[v & 15]
const off8 = (v: number): string => v.toString(16).toUpperCase().padStart(8, '0')

/* ── 每行放几个字节：按半边宽度实测字符宽算，与十六进制面板同一套 ─── */

const box = ref<HTMLElement | null>(null)
const ruler = ref<HTMLElement | null>(null)
const per = ref(DEFAULT_PER_LINE)

function measure(): void {
  const b = box.value, r = ruler.value
  if (!b || !r) return

  //⚠️ 高度也要跟着刷：不滚动地变高时 onScroll 不会来，vh 停在旧值就会少渲染几行
  const sc = scroller.value
  if (sc) { vh.value = sc.clientHeight; report() }

  const cw = r.getBoundingClientRect().width / 100
  if (cw <= 0) return

  //一行两半 + 中缝（26 宽 + 左右各 6 外边距）+ 行内边距 12 + 滚动条余量
  const half = (b.clientWidth - 38 - 12 - 14) / 2

  /*
    ⚠️ 固定开销传 <b>10</b> 不是默认的 11：这个组件的偏移栏后面只跟一个空格宽的外边距
    （8ch + 1ch + 字符栏前的 1ch），比十六进制面板少一格。用默认值等于每侧白扔一个字符宽。
    下限给 4 也不是 8 —— 一行要装两份，窄窗口下 8 排不开。
  */
  per.value = perLineFor(Math.floor(half / cw), 10, 4)
}

let ro: ResizeObserver | null = null

onMounted(() => {
  measure()
  ro = new ResizeObserver(() => measure())
  if (box.value) ro.observe(box.value)
})

onBeforeUnmount(() => { ro?.disconnect(); ro = null })

/* ── 单元流：把块摊成一格一格的「A 有什么 / B 有什么」 ──────────── */

interface Cell { ai: number; bi: number; op: DiffOp; ci: number }

/** 每一处差异（非 same 的块）在 changes 里的下标，按块序 */
const changeOf = computed<number[]>(() => {
  const out: number[] = []
  let ci = -1
  for (const b of props.blocks) { out.push(b.op === 'same' ? -1 : ++ci) }
  return out
})

const cells = computed<Cell[]>(() => {
  if (props.mode === 'plain') {
    const pa = Math.max(0, props.padA)
    const pb = Math.max(0, props.padB)
    const n = Math.max(pa + props.aBytes.length, pb + props.bBytes.length)
    const out: Cell[] = new Array(n)

    for (let i = 0; i < n; i++) {
      const ai = i - pa
      const bi = i - pb
      out[i] = {
        ai: ai >= 0 && ai < props.aBytes.length ? ai : -1,
        bi: bi >= 0 && bi < props.bBytes.length ? bi : -1,
        op: 'same',
        ci: -1,
      }
    }

    return out
  }

  if (props.mode !== 'hex') return []

  const out: Cell[] = []
  const cs = changeOf.value

  props.blocks.forEach((b, bi2) => {
    const ci = cs[bi2]
    const n = Math.max(b.aLen, b.bLen)

    for (let i = 0; i < n; i++) {
      out.push({
        ai: i < b.aLen ? b.aStart + i : -1,
        bi: i < b.bLen ? b.bStart + i : -1,
        op: b.op,
        ci,
      })
    }
  })

  return out
})

/* ── 行 ────────────────────────────────────────────────────────── */

interface Row {
  /** 对齐单位起点 —— 差异条那把尺子用的就是它 */
  unit: number
  op: DiffOp
  ci: number
  /** hex：这一行的单元区间；text：行下标 */
  from: number
  n: number
  aOff: number
  bOff: number
  aText: string
  bText: string
}

const rows = computed<Row[]>(() => {
  if (props.mode === 'text') {
    const out: Row[] = []
    const cs = changeOf.value
    let unit = 0

    props.blocks.forEach((b, bi2) => {
      const ci = cs[bi2]
      const n = Math.max(b.aLen, b.bLen)

      for (let i = 0; i < n; i++) {
        const ai = i < b.aLen ? b.aStart + i : -1
        const bi = i < b.bLen ? b.bStart + i : -1
        out.push({
          unit: unit + i,
          op: b.op,
          ci,
          from: i,
          n: 1,
          aOff: ai,
          bOff: bi,
          aText: ai >= 0 ? (props.aLines[ai] ?? '') : '',
          bText: bi >= 0 ? (props.bLines[bi] ?? '') : '',
        })
      }

      unit += n
    })

    return out
  }

  const list = cells.value
  const step = per.value
  const out: Row[] = []

  for (let i = 0; i < list.length; i += step) {
    const n = Math.min(step, list.length - i)

    let op: DiffOp = 'same'
    let ci = -1
    let aOff = -1
    let bOff = -1

    for (let j = 0; j < n; j++) {
      const c = list[i + j]
      if (op === 'same' && c.op !== 'same') { op = c.op; ci = c.ci }
      if (aOff < 0 && c.ai >= 0) aOff = c.ai
      if (bOff < 0 && c.bi >= 0) bOff = c.bi
    }

    out.push({ unit: i, op, ci, from: i, n, aOff, bOff, aText: '', bText: '' })
  }

  return out
})

/* ── 虚拟滚动 ──────────────────────────────────────────────────── */

const scroller = ref<HTMLElement | null>(null)
const top = ref(0)
const vh = ref(400)

function onScroll(): void {
  const el = scroller.value
  if (!el) return
  top.value = el.scrollTop
  vh.value = el.clientHeight
  report()
}

const start = computed(() => Math.max(0, Math.floor(top.value / ROW_H) - 4))
const count = computed(() => Math.ceil(vh.value / ROW_H) + 8)
const win = computed(() => rows.value.slice(start.value, start.value + count.value))

/*
  ⚠️ 可见行的格子<b>先算好</b>，模板里只 v-for 一个现成数组。

  写成模板里调 side(r, true) 的话，光十六进制那一半就要调两次（字节栏一次、字符栏一次），
  一屏 40 行就是 160 次、每次现造 per 个对象 —— 这正是 CLAUDE.md 里
  「模板里不要逐格调函数」那一条。滚动时每帧都要重来。
*/
const winDraw = computed(() => win.value.map((r) => ({
  r,
  a: props.mode === 'text' ? [] : side(r, true),
  b: props.mode === 'text' ? [] : side(r, false),
})))

function report(): void {
  const rs = rows.value
  if (!rs.length) { emit('view', 0, 0); return }

  const i0 = Math.max(0, Math.min(Math.floor(top.value / ROW_H), rs.length - 1))
  const i1 = Math.max(i0, Math.min(Math.ceil((top.value + vh.value) / ROW_H), rs.length) - 1)
  const last = rs[i1]

  emit('view', rs[i0].unit, last.unit + (props.mode === 'text' ? 1 : last.n))
}

watch(rows, () => {
  const el = scroller.value
  if (el) { el.scrollTop = 0; top.value = 0; vh.value = el.clientHeight }
  report()
})

onMounted(() => { vh.value = scroller.value?.clientHeight ?? 400; report() })

/* ── 跳到第 i 处差异 ───────────────────────────────────────────── */

function scrollToChange(i: number): void {
  const at = rows.value.findIndex((r) => r.ci === i)
  if (at < 0) return

  const el = scroller.value
  if (!el) return

  //让它落在视窗上方三分之一处，前后文都看得到
  el.scrollTop = Math.max(0, at * ROW_H - el.clientHeight / 3)
}

/**
 * 跳到第 unit 个对齐单位。查重模式（plain）没有差异块可跳，跳的是<b>字节位置</b>。
 */
function scrollToUnit(unit: number): void {
  const at = rows.value.findIndex((r) => unit >= r.unit && unit < r.unit + r.n)
  if (at < 0) return

  const el = scroller.value
  if (!el) return

  el.scrollTop = Math.max(0, at * ROW_H - el.clientHeight / 3)
}

defineExpose({ scrollToChange, scrollToUnit })

/* ── 一行里那些格子 ────────────────────────────────────────────── */

const hlSet = (list: Array<[number, number]>): Set<number> => {
  const s = new Set<number>()
  for (const [st, len] of list) for (let i = 0; i < len; i++) s.add(st + i)
  return s
}

const hlAset = computed(() => hlSet(props.hlA))
const hlBset = computed(() => hlSet(props.hlB))

interface Draw { hx: string; ch: string; cls: string }

function side(r: Row, isA: boolean): Draw[] {
  const list = cells.value
  const bytes = isA ? props.aBytes : props.bBytes
  const hl = isA ? hlAset.value : hlBset.value
  const out: Draw[] = []

  for (let j = 0; j < per.value; j++) {
    if (j >= r.n) { out.push({ hx: '', ch: '', cls: 'pad' }); continue }

    const c = list[r.from + j]
    const idx = isA ? c.ai : c.bi

    if (idx < 0) { out.push({ hx: '··', ch: '·', cls: 'gap ' + c.op }); continue }

    const v = bytes[idx]
    const cls = (c.op === 'same' ? '' : c.op) + (hl.has(idx) ? ' dup' : '')
    out.push({ hx: hex2(v), ch: asciiOf(v), cls })
  }

  return out
}
</script>

<template>
  <div ref="box" class="dv">
    <!-- 字符宽度的量尺：字体是回退链（Consolas / Cascadia Mono），每字宽差 9%，只能实测 -->
    <span ref="ruler" class="dv-ruler">MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM</span>

    <div ref="scroller" class="dv-scroll" @scroll.passive="onScroll">
      <div class="dv-spacer" :style="{ height: rows.length * ROW_H + 'px' }">
        <div class="dv-win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
          <div v-for="d in winDraw" :key="d.r.unit" class="dv-row" :class="d.r.op">
            <!-- A 侧。⚠️ 包一层 .dv-half：不包的话所有列都是 flex:none 左排，
                 取整剩下的那点宽度会<b>整块堆在 B 的右边</b>，看着像「B 没填满」（被问过一次）。
                 包了之后两半各占一半，余量在两侧平分，是对称的。 -->
            <div class="dv-half">
            <span class="dv-off">{{ d.r.aOff >= 0 ? off8(d.r.aOff) : '' }}</span>

            <template v-if="mode === 'text'">
              <span class="dv-line" :class="{ none: d.r.aOff < 0 }">{{ d.r.aOff >= 0 ? d.r.aText : '' }}</span>
            </template>
            <template v-else>
              <span class="dv-hex">
                <i v-for="(c, j) in d.a" :key="j" class="dv-b" :class="c.cls">{{ c.hx }}</i>
              </span>
              <span class="dv-ch">
                <i v-for="(c, j) in d.a" :key="j" class="dv-c" :class="c.cls">{{ c.ch }}</i>
              </span>
            </template>

            </div>

            <!-- 中缝：同一行里的一个格子，连着几行就自然成了一道贯通的带 -->
            <span class="dv-gut" :class="d.r.op" />

            <!-- B 侧 -->
            <div class="dv-half">
            <span class="dv-off">{{ d.r.bOff >= 0 ? off8(d.r.bOff) : '' }}</span>

            <template v-if="mode === 'text'">
              <span class="dv-line" :class="{ none: d.r.bOff < 0 }">{{ d.r.bOff >= 0 ? d.r.bText : '' }}</span>
            </template>
            <template v-else>
              <span class="dv-hex">
                <i v-for="(c, j) in d.b" :key="j" class="dv-b" :class="c.cls">{{ c.hx }}</i>
              </span>
              <span class="dv-ch">
                <i v-for="(c, j) in d.b" :key="j" class="dv-c" :class="c.cls">{{ c.ch }}</i>
              </span>
            </template>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/*
  ⚠️ flex 是 <b>0 1 auto</b> 不是 1：内容只有两行时就只占两行高，
  下面那条差异缩略图跟着贴上来 —— 不然一屏黑底里飘着两行字，
  而缩略图被顶到十万八千里外。内容长了照样能长满（被 flex 容器夹住，
  多出来的部分自己滚）。
*/
.dv {
  flex: 0 1 auto;
  min-height: 0;
  position: relative;
  overflow: hidden;
  border: 1px solid var(--border);
  background: var(--sink);
}

.dv-ruler {
  position: absolute;
  visibility: hidden;
  white-space: pre;
  font-family: Consolas, 'Cascadia Mono', monospace;
  font-size: var(--fs-dense);
}

.dv-scroll { max-height: 100%; overflow: auto; }
.dv-spacer { position: relative; }
.dv-win { position: absolute; top: 0; left: 0; right: 0; will-change: transform; }

.dv-row {
  display: flex;
  align-items: center;
  height: 18px;
  padding: 0 6px;
  font-family: Consolas, 'Cascadia Mono', monospace;
  font-size: var(--fs-dense);
  line-height: 18px;
  white-space: pre;
}

/* 整行底色：很淡的一层，负责「这一行动过」这件事；具体哪个字节动了由格子自己说 */
.dv-row.mod { background: rgb(var(--amber-rgb) / 7%); }
.dv-row.ins { background: rgb(var(--green-rgb) / 7%); }
.dv-row.del { background: rgb(var(--danger-rgb) / 7%); }

/* 两半各占一半 —— 与上面那条结果区表头（.rh 的两个 .rhh）是同一套分法 */
.dv-half { flex: 1 1 0; min-width: 0; display: flex; align-items: center; }

.dv-off {
  flex: none;
  width: 8ch;
  margin-right: 1ch;
  color: var(--dim);
}

.dv-hex { flex: none; }
.dv-ch { flex: none; margin-left: 1ch; }

.dv-b { display: inline-block; width: 3ch; color: var(--gray); font-style: normal; }
.dv-c { display: inline-block; width: 1ch; color: var(--soft); font-style: normal; }

/* 动过的字节：整格上色，比只改字色显眼得多 —— 一屏里就那么几格 */
.dv-b.mod, .dv-c.mod { color: var(--amber); background: rgb(var(--amber-rgb) / 16%); }
.dv-b.ins, .dv-c.ins { color: var(--green); background: rgb(var(--green-rgb) / 16%); }
.dv-b.del, .dv-c.del { color: var(--danger); background: rgb(var(--danger-rgb) / 16%); }

/* 占位格：这一侧没有这个字节。刻意画得很淡 —— 它是「空」，不是「有个值叫 ··」 */
.dv-b.gap, .dv-c.gap { color: var(--dim4); background: rgb(var(--inset-rgb) / 40%); }

/* 查重命中：青色，与差异那三色分得开 */
.dv-b.dup, .dv-c.dup { color: var(--cyan); background: rgb(var(--cyan-rgb) / 16%); }

.dv-gut {
  flex: none;
  width: 26px;
  height: 18px;
  margin: 0 6px;
  border-left: 1px solid var(--border);
  border-right: 1px solid var(--border);
}

.dv-gut.mod { background: rgb(var(--amber-rgb) / 30%); }
.dv-gut.ins { background: rgb(var(--green-rgb) / 30%); }
.dv-gut.del { background: rgb(var(--danger-rgb) / 30%); }

/* 文本模式：一行一句，超宽横向滚（整个容器滚，两侧一起） */
.dv-line {
  flex: 1;
  min-width: 0;
  color: var(--gray);
  overflow: hidden;
  text-overflow: ellipsis;
}

.dv-line.none { background: rgb(var(--inset-rgb) / 40%); }

.dv-row.mod .dv-line { color: var(--amber); }
.dv-row.ins .dv-line { color: var(--green); }
.dv-row.del .dv-line { color: var(--danger); }
</style>
