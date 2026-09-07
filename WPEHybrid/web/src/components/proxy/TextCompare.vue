<script setup lang="ts">
/*
  文本对比 —— 对应 WinForms 的 Controls/ComparisonText（两个 Tab：文本比较 / 文本查重）。

  这里不分两个 Tab、不放两套文本框：A / B 只有一份（stores/tools 里的 textA / textB，
  封包列表右键「添加到文本 A / B」写的就是它），工具条上切「比较 / 查重」两种模式，
  结果表按模式换列。WinForms 那边 SetTextA 也是同时写进两个 Tab 的框，本来就是一份数据。

  【比较】逐字符对齐（与 UiControls.CompareText 一样：位置 i 上 A / B 谁缺就是新增 / 删除，
  都有但不同就是修改），A 里删 / 改标红、B 里增 / 改标绿，差异逐条列表，点一行两边一起跳过去。
  【查重】找两段十六进制里共同的字节序列（C# 的 ComparePackets，O(n²)，丢在后台线程跑），
  两边文本会被整理成规范的 "AA BB CC" 形态（WinForms 也这么做，只是它还把不重复的字节抹成下划线，
  这里改成高亮重复的那些 —— 原文留着，看得更清楚）。
  【正则】输入即高亮命中；「过滤」把两边都替换成命中的部分拼起来（LeachRegexMatches）。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { textA, textB, tcMode, tcRegex, tcMinBytes, type DupRow, type Mark } from '../../stores/tools'
import HiliteArea from '../HiliteArea.vue'

interface DiffRow { pos: number; a: string; b: string; type: 'ins' | 'del' | 'mod' }

const haA = ref<InstanceType<typeof HiliteArea> | null>(null)
const haB = ref<InstanceType<typeof HiliteArea> | null>(null)

const marksA = ref<Mark[]>([])
const marksB = ref<Mark[]>([])
const diffRows = ref<DiffRow[]>([])
const dupRows = ref<DupRow[]>([])
const busy = ref(false)
const regexBad = ref(false)
const picked = ref(-1)

//暂存 / 还原：WinForms 的 bStore / bReset
const stash = ref<{ a: string; b: string } | null>(null)

const ROW_CAP = 3000
const rows = computed(() => (tcMode.value === 'diff' ? diffRows.value : dupRows.value))
const shownDiff = computed(() => diffRows.value.slice(0, ROW_CAP))

function clearMarks(): void {
  marksA.value = []
  marksB.value = []
}

/* ── 比较 ──────────────────────────────────────────────────── */

/** 连着的同类字符合成一段，两段完全不同的长文本才不会给出几万个单字符区间 */
function push(list: Mark[], i: number, cls: string): void {
  const last = list[list.length - 1]
  if (last && last.cls === cls && last.end === i) last.end = i + 1
  else list.push({ start: i, end: i + 1, cls })
}

function runDiff(): void {
  const a = textA.value, b = textB.value
  const n = Math.max(a.length, b.length)
  const out: DiffRow[] = []
  const ma: Mark[] = [], mb: Mark[] = []

  for (let i = 0; i < n; i++) {
    const type: DiffRow['type'] | '' = i >= a.length ? 'ins' : i >= b.length ? 'del' : a[i] === b[i] ? '' : 'mod'
    if (!type) continue

    out.push({ pos: i + 1, a: i < a.length ? a[i] : 'N/A', b: i < b.length ? b[i] : 'N/A', type })
    if (type !== 'ins') push(ma, i, 'del')
    if (type !== 'del') push(mb, i, 'ins')
  }

  diffRows.value = out
  marksA.value = ma
  marksB.value = mb
  picked.value = -1
}

/* ── 查重 ──────────────────────────────────────────────────── */

/** 与 C# 的 CleanAndNormalizeHex 完全一致：只留十六进制字符、大写 */
function normalize(s: string): string {
  return s.replace(/[^0-9a-fA-F]/g, '').toUpperCase()
}

function formatHex(s: string): string {
  return s.replace(/(..)(?=.)/g, '$1 ')
}

async function runDup(): Promise<void> {
  const a = normalize(textA.value), b = normalize(textB.value)
  if (!a || !b) { pushToast('warning', t('tc.needBoth')); return }

  busy.value = true
  try {
    const r = await call<{ rows: DupRow[] }>('textDuplicates', { a, b, min: Math.max(1, Math.trunc(tcMinBytes.value || 1)) })
    const list = r?.rows ?? []

    //两边换成规范形态，位置才能按「一个字节三个字符」直接对上
    textA.value = formatHex(a)
    textB.value = formatHex(b)
    dupRows.value = list

    const ma: Mark[] = [], mb: Mark[] = []
    for (const d of list) {
      for (const p of d.PositionsInA) ma.push({ start: p * 3, end: p * 3 + d.Length * 3 - 1, cls: 'dup' })
      for (const p of d.PositionsInB) mb.push({ start: p * 3, end: p * 3 + d.Length * 3 - 1, cls: 'dup' })
    }
    marksA.value = ma
    marksB.value = mb
    picked.value = -1
  } catch (e) {
    console.error('[tc] 查重失败', e)
  } finally {
    busy.value = false
  }
}

function run(): void {
  if (tcMode.value === 'diff') runDiff()
  else void runDup()
}

/* ── 正则 ──────────────────────────────────────────────────── */

let rxTimer = 0

watch(tcRegex, () => {
  clearTimeout(rxTimer)
  rxTimer = window.setTimeout(highlightRegex, 200)
})

function compile(): RegExp | null {
  const p = tcRegex.value
  if (!p) { regexBad.value = false; return null }
  try {
    const rx = new RegExp(p, 'g')
    regexBad.value = false
    return rx
  } catch {
    regexBad.value = true
    return null
  }
}

function matchesOf(rx: RegExp, s: string): Mark[] {
  const out: Mark[] = []
  rx.lastIndex = 0
  let m: RegExpExecArray | null
  while ((m = rx.exec(s)) !== null && out.length < 20000) {
    if (m[0].length === 0) { rx.lastIndex++; continue }
    out.push({ start: m.index, end: m.index + m[0].length, cls: 'rx' })
  }
  return out
}

/** 输入即高亮（FindRegexMatches）。会把上一次的差异高亮清掉，WinForms 也是先 ClearStyle */
function highlightRegex(): void {
  const rx = compile()
  if (!rx) return
  marksA.value = matchesOf(rx, textA.value)
  marksB.value = matchesOf(rx, textB.value)
}

/** 「过滤」：两边都换成命中片段拼起来（LeachRegexMatches） */
function leach(): void {
  const rx = compile()
  if (!rx) { if (!tcRegex.value) pushToast('warning', t('tc.needRegex')); return }
  textA.value = matchesOf(rx, textA.value).map((m) => textA.value.slice(m.start, m.end)).join('')
  textB.value = matchesOf(rx, textB.value).map((m) => textB.value.slice(m.start, m.end)).join('')
  clearMarks()
}

/* ── 定位 / 暂存 / 清空 ────────────────────────────────────── */

function locate(i: number): void {
  picked.value = i
  if (tcMode.value === 'diff') {
    const r = diffRows.value[i]
    if (!r) return
    haA.value?.focusAt(r.pos - 1, 1)
    haB.value?.focusAt(r.pos - 1, 1)
  } else {
    const d = dupRows.value[i]
    if (!d) return
    const len = d.Length * 3 - 1
    if (d.PositionsInA.length) haA.value?.focusAt(d.PositionsInA[0] * 3, len)
    if (d.PositionsInB.length) haB.value?.focusAt(d.PositionsInB[0] * 3, len)
  }
}

function store(): void {
  stash.value = { a: textA.value, b: textB.value }
  pushToast('success', t('tc.stored'))
}

function reset(): void {
  if (!stash.value) { pushToast('warning', t('tc.nothingStored')); return }
  textA.value = stash.value.a
  textB.value = stash.value.b
  clearMarks()
}

function clearAll(): void {
  textA.value = ''
  textB.value = ''
  tcRegex.value = ''
  diffRows.value = []
  dupRows.value = []
  clearMarks()
}

//切模式时结果表跟着换，旧的高亮不再对应
watch(tcMode, () => { clearMarks(); picked.value = -1 })

const TYPE_KEY = { ins: 'tc.inserted', del: 'tc.deleted', mod: 'tc.modified' } as const
</script>

<template>
  <div class="page list-page tc">
    <div class="bar">
      <!-- 模式切换：分段按钮，与快捷面板的页签同一种「选中亮青」语言 -->
      <div class="seg">
        <button class="sg" :class="{ on: tcMode === 'diff' }" @click="tcMode = 'diff'">{{ t('tc.modeDiff') }}</button>
        <button class="sg" :class="{ on: tcMode === 'dup' }" @click="tcMode = 'dup'">{{ t('tc.modeDup') }}</button>
      </div>

      <span class="sep" />

      <input v-model="tcRegex" class="inp rx" :class="{ bad: regexBad }" spellcheck="false" :placeholder="t('tc.regexPh')">
      <button class="btn" :disabled="!tcRegex" @click="leach">{{ t('tc.leach') }}</button>

      <template v-if="tcMode === 'dup'">
        <span class="sep" />
        <span class="lb">{{ t('tc.minBytes') }}</span>
        <input v-model.number="tcMinBytes" class="inp num" type="number" min="1" max="4096">
      </template>

      <span class="sep" />
      <button class="btn primary" :disabled="busy || (!textA && !textB)" @click="run">
        {{ busy ? t('proxy.working') : (tcMode === 'diff' ? t('tc.runDiff') : t('tc.runDup')) }}
      </button>

      <span class="grow" />

      <button class="btn" :disabled="!textA && !textB" @click="store">{{ t('tc.store') }}</button>
      <button class="btn" :disabled="!stash" @click="reset">{{ t('tc.reset') }}</button>
      <button class="btn danger" :disabled="!textA && !textB && !tcRegex" @click="clearAll">{{ t('tc.clear') }}</button>
    </div>

    <div class="panes">
      <div class="pane">
        <div class="ph">
          <span class="tt">{{ t('tc.textA') }}</span>
          <span class="meta">{{ t('tc.length') }} <b>{{ textA.length }}</b></span>
          <span v-if="marksA.length" class="meta hit">{{ t('tc.marks') }} <b>{{ marksA.length }}</b></span>
        </div>
        <HiliteArea ref="haA" v-model="textA" :marks="marksA" :placeholder="t('tc.phA')" />
      </div>

      <div class="pane">
        <div class="ph">
          <span class="tt">{{ t('tc.textB') }}</span>
          <span class="meta">{{ t('tc.length') }} <b>{{ textB.length }}</b></span>
          <span v-if="marksB.length" class="meta hit">{{ t('tc.marks') }} <b>{{ marksB.length }}</b></span>
        </div>
        <HiliteArea ref="haB" v-model="textB" :marks="marksB" :placeholder="t('tc.phB')" />
      </div>
    </div>

    <!-- 结果表：比较 / 查重各一套列，点一行两边一起定位 -->
    <div class="res">
      <div class="tbody">
        <div v-if="tcMode === 'diff'" class="head hd-diff">
          <span class="no">{{ t('col.id') }}</span>
          <span class="pos">{{ t('tc.position') }}</span>
          <span class="va">{{ t('tc.valueA') }}</span>
          <span class="vb">{{ t('tc.valueB') }}</span>
          <span class="ty">{{ t('tc.changeType') }}</span>
        </div>
        <div v-else class="head hd-dup">
          <span class="no">{{ t('col.id') }}</span>
          <span class="seq">{{ t('tc.sequence') }}</span>
          <span class="len">{{ t('col.len') }}</span>
          <span class="cnt">{{ t('tc.countA') }}</span>
          <span class="cnt">{{ t('tc.countB') }}</span>
          <span class="pos2">{{ t('tc.posA') }}</span>
          <span class="pos2">{{ t('tc.posB') }}</span>
        </div>

        <div v-if="!rows.length" class="empty">{{ tcMode === 'diff' ? t('tc.emptyDiff') : t('tc.emptyDup') }}</div>

        <template v-else-if="tcMode === 'diff'">
          <div
            v-for="(r, i) in shownDiff"
            :key="i"
            class="row2 hd-diff"
            :class="[r.type, { sel: picked === i }]"
            @click="locate(i)"
          >
            <span class="no">{{ i + 1 }}</span>
            <span class="pos">{{ r.pos }}</span>
            <span class="va" :title="r.a">{{ r.a }}</span>
            <span class="vb" :title="r.b">{{ r.b }}</span>
            <span class="ty"><i class="tg">{{ t(TYPE_KEY[r.type]) }}</i></span>
          </div>
          <div v-if="diffRows.length > ROW_CAP" class="more">{{ t('tc.more') }} {{ diffRows.length }}</div>
        </template>

        <template v-else>
          <div
            v-for="(d, i) in dupRows"
            :key="i"
            class="row2 hd-dup"
            :class="{ sel: picked === i }"
            @click="locate(i)"
          >
            <span class="no">{{ i + 1 }}</span>
            <span class="seq" :title="d.Sequence">{{ d.Sequence }}</span>
            <span class="len">{{ d.Length }}</span>
            <span class="cnt">{{ d.CountInA }}</span>
            <span class="cnt">{{ d.CountInB }}</span>
            <span class="pos2" :title="d.PositionsInA.join(', ')">{{ d.PositionsInA.join(', ') }}</span>
            <span class="pos2" :title="d.PositionsInB.join(', ')">{{ d.PositionsInB.join(', ') }}</span>
          </div>
        </template>
      </div>
    </div>
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

.sep { width: 1px; height: 16px; background: var(--border); flex: none; }
.lb { flex: none; font-size: 12px; color: var(--muted); white-space: nowrap; }

/* 分段按钮 */
.seg { display: inline-flex; border: 1px solid var(--border); flex: none; }

.sg {
  padding: 9px 13px 7px;
  background: transparent;
  border: 0;
  color: var(--muted);
  font-family: var(--share);
  font-size: var(--btn-size);
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
}

.sg + .sg { border-left: 1px solid var(--border); }
.sg:hover { color: var(--gray); }
.sg.on { background: rgb(var(--cyan-rgb) / 10%); color: var(--cyan); }

.inp {
  height: 28px;
  padding: 0 10px;
  background: rgb(var(--inset-rgb) / 30%);
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--mono);
  font-size: 12.5px;
  outline: none;
}

.inp:focus { border-color: var(--cyan); }
.inp.rx { flex: 0 1 300px; min-width: 140px; }
.inp.rx.bad { border-color: var(--danger); color: var(--danger); }
.inp.num { width: 70px; text-align: center; flex: none; }

/* 上下分：两个文本框吃 60%，结果表吃 40% */
.panes {
  flex: 3;
  min-height: 0;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 8px;
}

.pane {
  min-width: 0;
  min-height: 0;
  display: flex;
  flex-direction: column;
  border: 1px solid var(--border);
  background: var(--sink);
}

/* 面板标题：与各表表头同一份令牌 */
.ph {
  flex: none;
  display: flex;
  align-items: center;
  gap: 14px;
  height: var(--th-h);
  padding: 0 12px;
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.ph > span { padding-top: 4px; }   /* 字形偏上 2px，与各表表头同一份补偿 */
.ph .tt { color: var(--cyan); }
.ph .meta { color: var(--muted); letter-spacing: .06em; text-transform: none; }
.ph .meta b { font-family: var(--mono); color: var(--gray); font-weight: 400; }
.ph .meta.hit b { color: var(--amber); }

/* 结果表 */
.res {
  flex: 2;
  min-height: 120px;
  display: flex;
  flex-direction: column;
  border: 1px solid var(--border);
  background: var(--sink);
}

.tbody { flex: 1; min-height: 0; overflow-y: auto; }

.head,
.row2 {
  display: grid;
  align-items: center;
  gap: 8px;
  padding: 0 14px;   /* 与 style.css 的 .list-page .head 一致 */
  font-size: 12.5px;
}

.hd-diff { grid-template-columns: 56px 90px minmax(80px, 1fr) minmax(80px, 1fr) 110px; }
.hd-dup { grid-template-columns: 56px minmax(220px, 2fr) 60px 70px 70px minmax(120px, 1fr) minmax(120px, 1fr); }

.row2 { height: 30px; border-bottom: 1px solid rgb(var(--border-rgb) / 45%); color: var(--soft); cursor: pointer; }
.row2:hover { background: rgb(var(--tint-rgb) / 4%); }
.row2.sel { background: rgb(var(--cyan-rgb) / 6%); box-shadow: inset 2px 0 0 var(--cyan); }

.row2 > span,
.head > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.head > span,
.row2 > span { text-align: center; }

.head > span.seq, .row2 > span.seq,
.head > span.pos2, .row2 > span.pos2 { text-align: left; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.pos, .len, .cnt { font-family: var(--mono); font-variant-numeric: tabular-nums; color: var(--cyan); }
.va { font-family: var(--mono); color: var(--pink); }
.vb { font-family: var(--mono); color: var(--acc-green2); }
.seq { font-family: var(--mono); color: var(--acc-green2); }
.pos2 { font-family: var(--mono); font-size: 12px; color: var(--dim3); }

/* 变更类型标签：新增绿 / 删除红 / 修改琥珀 */
.tg {
  display: inline-block;
  padding: 4px 7px 2px;
  border: 1px solid;
  font-family: var(--share);
  font-size: 10.5px;
  line-height: 1;
  letter-spacing: .08em;
  text-transform: uppercase;
  font-style: normal;
}

.row2.ins .tg { color: var(--green); border-color: rgb(var(--green-rgb) / 35%); }
.row2.del .tg { color: var(--danger); border-color: rgb(var(--danger-rgb) / 35%); }
.row2.mod .tg { color: var(--amber); border-color: rgb(var(--amber-rgb) / 40%); }

.more { padding: 10px 14px; font-size: 11.5px; color: var(--muted); text-align: center; }
</style>
