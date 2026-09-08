<script setup lang="ts">
/*
  差异条 —— 把整段内容压成一条 22px 的缩略图，差异在哪儿一眼看得见。

  它回答的是「差异集中在头部还是尾部、密不密」这个问题 —— 那正是原来那张
  几千行的结果表<b>答不上来</b>的（表只能一行行翻）。点一下跳到那一处。

  坐标用<b>对齐后的单位</b>（每块取 max(aLen, bLen)），与右边视图滚动的那把尺子一致，
  所以视窗方框的位置是准的，不是估的。
*/
import { computed } from 'vue'
import type { DiffBlock } from '../../diff'

const props = withDefaults(defineProps<{
  blocks: DiffBlock[]
  /** 当前是第几处差异（changes 里的下标），-1 = 还没选 */
  current?: number
  /** 视窗覆盖的对齐单位区间 [start, end)，用来画那个方框 */
  viewStart?: number
  viewEnd?: number
  /*
    色调。diff = 按 op 分绿/琥珀/红（增/改/删）；
    dup = 一律青 —— 查重那两条覆盖率条上每一段都是「共同片段」，没有增删改之分，
    按 op 上色只会让人以为它们不是一回事。青也正是十六进制视图里查重命中的颜色。
  */
  tone?: 'diff' | 'dup'
  /** 高度。覆盖率条比差异条矮一半，两条摞起来才不占地方 */
  slim?: boolean
}>(), { current: -1, viewStart: 0, viewEnd: 0, tone: 'diff', slim: false })

const emit = defineEmits<{ (e: 'pick', changeIndex: number): void }>()

const span = (b: DiffBlock): number => Math.max(b.aLen, b.bLen)

/** 每一处差异在缩略条上的位置（百分比）+ 它是 changes 里的第几个 */
const segs = computed(() => {
  let at = 0
  let ci = -1
  const out: Array<{ left: number; width: number; op: string; i: number }> = []

  for (const b of props.blocks) {
    const n = span(b)
    if (b.op !== 'same') {
      ci++
      out.push({ left: at, width: n, op: b.op, i: ci })
    }
    at += n
  }

  const total = at || 1

  /*
    ⚠️ 差异很碎时（几千处）逐个画会是几千个 div。相邻的两段离得不到 0.4% 就并成一段 ——
    那个距离在 22px 高、整屏宽的条上本来也分不开，但<b>并的时候要保住 i</b>：
    点它要能跳到那一批里的第一处。
  */
  const merged: Array<{ left: number; width: number; op: string; i: number }> = []

  for (const s of out) {
    const left = (s.left / total) * 100
    const width = Math.max((s.width / total) * 100, 0.35)
    const prev = merged[merged.length - 1]

    if (prev && prev.op === s.op && left - (prev.left + prev.width) < 0.4) {
      prev.width = left + width - prev.left
      continue
    }

    merged.push({ left, width, op: s.op, i: s.i })
  }

  return merged
})

const total = computed(() => props.blocks.reduce((n, b) => n + span(b), 0) || 1)

const box = computed(() => {
  const s = Math.max(0, Math.min(props.viewStart, total.value))
  const e = Math.max(s, Math.min(props.viewEnd, total.value))
  if (e <= s) return null
  return { left: (s / total.value) * 100, width: Math.max((e - s) / total.value * 100, 1) }
})

/** 当前那一处在条上的位置 —— 单独画一根竖线，段太窄时也看得见 */
const cursor = computed(() => {
  if (props.current < 0) return null
  let at = 0
  let ci = -1
  for (const b of props.blocks) {
    const n = span(b)
    if (b.op !== 'same') {
      ci++
      if (ci === props.current) return { left: (at / total.value) * 100 }
    }
    at += n
  }
  return null
})

function onClick(e: MouseEvent): void {
  const el = e.currentTarget as HTMLElement
  const r = el.getBoundingClientRect()
  const pct = ((e.clientX - r.left) / r.width) * 100

  //点空白处也要有反应：跳到离点击点最近的那一处
  let best = -1
  let bestD = Infinity
  for (const s of segs.value) {
    const d = pct < s.left ? s.left - pct : pct > s.left + s.width ? pct - s.left - s.width : 0
    if (d < bestD) { bestD = d; best = s.i }
  }

  if (best >= 0) emit('pick', best)
}
</script>

<template>
  <div class="dmap" :class="[tone, { slim }]" @click="onClick">
    <div v-if="box" class="dm-box" :style="{ left: box.left + '%', width: box.width + '%' }" />
    <div
      v-for="(s, i) in segs"
      :key="i"
      class="dm-seg"
      :class="s.op"
      :style="{ left: s.left + '%', width: s.width + '%' }"
    />
    <div v-if="cursor" class="dm-cur" :style="{ left: cursor.left + '%' }" />
  </div>
</template>

<style scoped>
.dmap {
  flex: none;
  position: relative;
  height: 22px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 30%);
  cursor: pointer;
  overflow: hidden;
}

/* 视窗方框画在最下层，色块压在它上面 —— 反过来会把窄段盖住 */
.dm-box {
  position: absolute;
  top: 0;
  bottom: 0;
  background: rgb(var(--chrome-rgb) / 55%);
  border-left: 1px solid var(--border2);
  border-right: 1px solid var(--border2);
}

.dm-seg {
  position: absolute;
  top: 3px;
  bottom: 3px;
  min-width: 2px;
}

/* 与右边视图同一套语义色：琥珀=改、绿=增、红=删 */
.dm-seg.mod { background: var(--amber); }
.dm-seg.ins { background: var(--green); }
.dm-seg.del { background: var(--danger); }

/* 查重：一律青，与十六进制视图里命中的那个颜色一致 */
.dmap.dup .dm-seg { background: var(--cyan); }

.dmap.slim { height: 12px; }
.dmap.slim .dm-seg { top: 2px; bottom: 2px; }

/* ⚠️ 查重色调下段本身就是青的，光标线再用青就看不见了 —— 换琥珀 */
.dmap.dup .dm-cur { background: var(--amber); box-shadow: 0 0 6px rgb(var(--amber-rgb) / 70%); }

.dm-cur {
  position: absolute;
  top: 0;
  bottom: 0;
  width: 2px;
  margin-left: -1px;
  background: var(--cyan);
  box-shadow: 0 0 6px rgb(var(--cyan-rgb) / 70%);
}
</style>
