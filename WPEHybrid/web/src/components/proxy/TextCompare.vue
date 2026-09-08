<script setup lang="ts">
/*
  文本对比 —— 对应 WinForms 的 Controls/ComparisonText（两个 Tab：文本比较 / 文本查重）。

  ══ 2026-09-08 重做过一轮，先说清为什么 ══

  老版本是<b>按位置逐字符</b>比的（位置 i 上 A/B 谁缺就是增/删，都有但不同就是改）。
  实测把 A = "AA BB CC DD EE FF"、B = "AA 11 BB CC DD EE FF"（只在中间插了一个字节）
  丢进去，给出 <b>13 条「修改」</b>，而且条条是错的（「位置 7：C vs B 修改」）。
  正确答案只有一句：B 在偏移 1 多了一个字节。而「插了一个字节」正是抓包对比里最常见的事。

  现在三处不同：
    ① 用 Myers 求最短编辑脚本（src/diff.ts），出的是<b>块</b>不是位置 —— 上面那例只剩 1 处；
    ② 主视图是<b>并排对齐</b>的（DiffView）：两栏共用一套行、插删侧用占位格顶住，
       横着扫永远对得齐。默认按<b>字节</b>（主力输入就是「添加到文本 A/B」灌进来的十六进制），
       文本模式按<b>行</b>；
    ③ 那张「一处一行」的结果表撤了 —— 差异多时几千行、答不上「差异集中在哪儿」。
       换成工具条上的 ‹ n / N › 前后导航 ＋ 底部一条差异缩略图（DiffMap）。

  【查重】不是差异，是「两段里共同出现的字节序列」，所以它<b>保留结果表</b>
  （那张表本来就是几行，且真的在回答问题），视图切成不对齐的两栏并把命中处标青。

  【正则】高亮只在<b>编辑区</b>生效（HiliteArea 的老本行）；「过滤」照旧改写 A/B 两边。
*/
import { computed, nextTick, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { textA, textB, tcMode, tcView, tcRegex, tcMinBytes, type DupRow, type Mark } from '../../stores/tools'
import { diffBytes, diffLines, type DiffBlock } from '../../diff'
import HiliteArea from '../HiliteArea.vue'
import DiffView from './DiffView.vue'
import DiffMap from './DiffMap.vue'

const haA = ref<InstanceType<typeof HiliteArea> | null>(null)
const haB = ref<InstanceType<typeof HiliteArea> | null>(null)
const dv = ref<InstanceType<typeof DiffView> | null>(null)

const marksA = ref<Mark[]>([])
const marksB = ref<Mark[]>([])
const dupRows = ref<DupRow[]>([])
const busy = ref(false)
const regexBad = ref(false)

const blocks = ref<DiffBlock[]>([])
const truncated = ref(false)
const cur = ref(-1)
const ran = ref(false)

/** 编辑区展开着没有。两边都空时当然要展开；比较之后自动收起，把地方让给结果 */
const editing = ref(true)

const viewStart = ref(0)
const viewEnd = ref(0)

//暂存 / 还原：WinForms 的 bStore / bReset
const stash = ref<{ a: string; b: string } | null>(null)

/* ── 输入的两种读法 ────────────────────────────────────────── */

/** 与 C# 的 CleanAndNormalizeHex 一致：只留十六进制字符、大写 */
function normalize(s: string): string {
  return s.replace(/[^0-9a-fA-F]/g, '').toUpperCase()
}

function toBytes(s: string): Uint8Array {
  const h = normalize(s)
  const n = h.length >> 1
  const out = new Uint8Array(n)
  for (let i = 0; i < n; i++) out[i] = parseInt(h.substr(i * 2, 2), 16)
  return out
}

const splitLines = (s: string): string[] => (s.length ? s.split(/\r\n|\r|\n/) : [])

const aBytes = computed(() => toBytes(textA.value))
const bBytes = computed(() => toBytes(textB.value))
const aLines = computed(() => splitLines(textA.value))
const bLines = computed(() => splitLines(textB.value))

/*
  内容像不像十六进制。默认视图是十六进制（主力输入就是它），但用户也可能贴一段 HTTP 头进来 ——
  那时按字节比毫无意义。⚠️ <b>不自动切视图</b>（切了用户不知道发生了什么），
  只在工具条上挂一句提示，旁边就是「文本」那一格。
*/
const hexish = computed(() => {
  const s = (textA.value + textB.value).replace(/\s/g, '')
  if (!s) return true
  let ok = 0
  for (let i = 0; i < s.length; i++) if (/[0-9a-fA-F]/.test(s[i])) ok++
  return ok / s.length >= 0.9
})

const changes = computed(() => blocks.value.filter((b) => b.op !== 'same').length)

/* ── 比较 / 查重 ───────────────────────────────────────────── */

function runDiff(): void {
  const r = tcView.value === 'hex'
    ? diffBytes(aBytes.value, bBytes.value)
    : diffLines(aLines.value, bLines.value)

  blocks.value = r.blocks
  truncated.value = r.truncated
  cur.value = -1
  ran.value = true
  dupRows.value = []

  if (r.truncated) pushToast('warning', t('tc.tooMany'))
  else if (!r.changes) pushToast('success', t('tc.same'))

  editing.value = false
  if (r.changes) nextTick(() => go(0))
}

const hlA = ref<Array<[number, number]>>([])
const hlB = ref<Array<[number, number]>>([])

async function runDup(): Promise<void> {
  const a = normalize(textA.value), b = normalize(textB.value)
  if (!a || !b) { pushToast('warning', t('tc.needBoth')); return }

  busy.value = true
  try {
    const r = await call<{ rows: DupRow[] }>('textDuplicates', { a, b, min: Math.max(1, Math.trunc(tcMinBytes.value || 1)) })
    const list = r?.rows ?? []

    //两边换成规范形态（WinForms 也这么做），位置才对得上
    textA.value = a.replace(/(..)(?=.)/g, '$1 ')
    textB.value = b.replace(/(..)(?=.)/g, '$1 ')

    dupRows.value = list
    blocks.value = []
    truncated.value = false
    cur.value = -1
    ran.value = true
    hlA.value = list.flatMap((d) => d.PositionsInA.map((p) => [p, d.Length] as [number, number]))
    hlB.value = list.flatMap((d) => d.PositionsInB.map((p) => [p, d.Length] as [number, number]))

    editing.value = false
    if (!list.length) pushToast('info', t('tc.noDup'))
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

/* ── 差异之间跳 ────────────────────────────────────────────── */

function go(i: number): void {
  if (!changes.value) return
  cur.value = ((i % changes.value) + changes.value) % changes.value
  dv.value?.scrollToChange(cur.value)
}

const next = (): void => go(cur.value + 1)
const prev = (): void => go(cur.value - 1)

function onKey(e: KeyboardEvent): void {
  if (e.key !== 'F3') return
  e.preventDefault()
  if (e.shiftKey) prev()
  else next()
}

onMounted(() => window.addEventListener('keydown', onKey))
onBeforeUnmount(() => window.removeEventListener('keydown', onKey))

/* ── 正则（只在编辑区）────────────────────────────────────── */

let rxTimer = 0
watch(tcRegex, () => { clearTimeout(rxTimer); rxTimer = window.setTimeout(highlightRegex, 200) })

function compile(): RegExp | null {
  const p = tcRegex.value
  if (!p) { regexBad.value = false; marksA.value = []; marksB.value = []; return null }
  try { const rx = new RegExp(p, 'g'); regexBad.value = false; return rx } catch { regexBad.value = true; return null }
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

function highlightRegex(): void {
  const rx = compile()
  if (!rx) return
  editing.value = true
  marksA.value = matchesOf(rx, textA.value)
  marksB.value = matchesOf(rx, textB.value)
}

/** 「过滤」：两边都换成命中片段拼起来（LeachRegexMatches） */
function leach(): void {
  const rx = compile()
  if (!rx) { if (!tcRegex.value) pushToast('warning', t('tc.needRegex')); return }
  textA.value = matchesOf(rx, textA.value).map((m) => textA.value.slice(m.start, m.end)).join('')
  textB.value = matchesOf(rx, textB.value).map((m) => textB.value.slice(m.start, m.end)).join('')
  marksA.value = []
  marksB.value = []
}

/* ── 暂存 / 还原 / 清空 ────────────────────────────────────── */

function store(): void {
  stash.value = { a: textA.value, b: textB.value }
  pushToast('success', t('tc.stored'))
}

function reset(): void {
  if (!stash.value) { pushToast('warning', t('tc.nothingStored')); return }
  textA.value = stash.value.a
  textB.value = stash.value.b
  wipe()
}

function clearAll(): void {
  textA.value = ''
  textB.value = ''
  tcRegex.value = ''
  wipe()
  editing.value = true
}

/** 结果作废：内容变了、或换了看法，上一次算出来的块就不再对应了 */
function wipe(): void {
  blocks.value = []
  dupRows.value = []
  hlA.value = []
  hlB.value = []
  marksA.value = []
  marksB.value = []
  truncated.value = false
  cur.value = -1
  ran.value = false
}

watch([textA, textB], () => { if (ran.value) wipe() })
watch([tcMode, tcView], wipe)

/** 双击结果里的一处 → 回编辑区（那是唯一能改内容的地方） */
function edit(): void {
  editing.value = !editing.value
}

//查重那张表点一行，跳到 A 侧第一处
function pickDup(i: number): void {
  const d = dupRows.value[i]
  if (!d || !d.PositionsInA.length) return
  cur.value = i
  hlA.value = [[d.PositionsInA[0], d.Length]]
  hlB.value = d.PositionsInB.length ? [[d.PositionsInB[0], d.Length]] : []
}

function allDup(): void {
  cur.value = -1
  hlA.value = dupRows.value.flatMap((d) => d.PositionsInA.map((p) => [p, d.Length] as [number, number]))
  hlB.value = dupRows.value.flatMap((d) => d.PositionsInB.map((p) => [p, d.Length] as [number, number]))
}
</script>

<template>
  <div class="page list-page tc">
    <div class="bar">
      <!-- 看法：按字节还是按行。与十六进制面板同一套分段按钮 -->
      <div class="hx-seg" :title="t('tc.viewHint')">
        <button class="hx-segb after" :class="{ on: tcView === 'hex' }" @click="tcView = 'hex'">{{ t('sp.hex') }}</button>
        <button class="hx-segb before" :class="{ on: tcView === 'text' }" @click="tcView = 'text'">{{ t('sp.text') }}</button>
      </div>

      <div class="hx-seg">
        <button class="hx-segb after" :class="{ on: tcMode === 'diff' }" @click="tcMode = 'diff'">{{ t('tc.modeDiff') }}</button>
        <button class="hx-segb before" :class="{ on: tcMode === 'dup' }" @click="tcMode = 'dup'">{{ t('tc.modeDup') }}</button>
      </div>

      <button class="btn primary" :disabled="busy || (!textA && !textB)" @click="run">
        {{ busy ? t('proxy.working') : (tcMode === 'diff' ? t('tc.runDiff') : t('tc.runDup')) }}
      </button>

      <!-- 差异导航：那张几千行的表换成了这一个计数器 -->
      <template v-if="tcMode === 'diff' && ran">
        <span class="nav" :class="{ zero: !changes }">
          <button class="nb" :disabled="!changes" :title="t('tc.prevHit')" @click="prev">‹</button>
          <b>{{ changes ? cur + 1 : 0 }}</b><span class="sl">/</span><b>{{ changes }}</b>
          <button class="nb" :disabled="!changes" :title="t('tc.nextHit')" @click="next">›</button>
        </span>
        <span v-if="truncated" class="warn">{{ t('tc.tooMany') }}</span>
      </template>

      <span v-if="tcView === 'hex' && !hexish" class="warn">{{ t('tc.notHex') }}</span>

      <span class="sep" />

      <input v-model="tcRegex" class="inp rx" :class="{ bad: regexBad }" spellcheck="false" :placeholder="t('tc.regexPh')">
      <button class="btn" :disabled="!tcRegex" @click="leach">{{ t('tc.leach') }}</button>

      <template v-if="tcMode === 'dup'">
        <span class="lb">{{ t('tc.minBytes') }}</span>
        <input v-model.number="tcMinBytes" class="inp num" type="number" min="1" max="4096">
      </template>

      <span class="grow" />

      <button class="btn" :class="{ on: editing }" @click="edit">{{ t('tc.edit') }}</button>
      <button class="btn" :disabled="!textA && !textB" @click="store">{{ t('tc.store') }}</button>
      <button class="btn" :disabled="!stash" @click="reset">{{ t('tc.reset') }}</button>
      <button class="btn danger" :disabled="!textA && !textB && !tcRegex" @click="clearAll">{{ t('tc.clear') }}</button>
    </div>

    <!-- 编辑区：粘贴 / 改内容 / 看正则命中。比较之后自动收起，把地方让给结果 -->
    <div v-if="editing" class="io">
      <div class="iop">
        <div class="ph"><span class="tt">{{ t('tc.textA') }}</span><span class="meta">{{ t('tc.length') }} <b>{{ textA.length }}</b></span></div>
        <HiliteArea ref="haA" v-model="textA" :marks="marksA" :placeholder="t('tc.phA')" />
      </div>
      <div class="iop">
        <div class="ph"><span class="tt">{{ t('tc.textB') }}</span><span class="meta">{{ t('tc.length') }} <b>{{ textB.length }}</b></span></div>
        <HiliteArea ref="haB" v-model="textB" :marks="marksB" :placeholder="t('tc.phB')" />
      </div>
    </div>

    <!-- 结果 -->
    <template v-if="ran">
      <!-- ⚠️ 这一条要与下面 DiffView 的两半<b>逐像素对齐</b>：两个等宽半边 + 中缝 38px
           （= .dv-gut 的 26px 宽 + 左右各 6px 外边距）。写死 38 的地方只有这一处与那一处，改一个就要改另一个。 -->
      <div class="rh">
        <div class="rhh">
          <span class="rt">{{ t('tc.textA') }}</span>
          <span class="rm">{{ tcView === 'hex' ? aBytes.length : aLines.length }} {{ tcView === 'hex' ? t('tc.bytes') : t('tc.lines') }}</span>
        </div>
        <span class="rhg" />
        <div class="rhh">
          <span class="rt">{{ t('tc.textB') }}</span>
          <span class="rm">{{ tcView === 'hex' ? bBytes.length : bLines.length }} {{ tcView === 'hex' ? t('tc.bytes') : t('tc.lines') }}</span>
        </div>
      </div>

      <DiffView
        ref="dv"
        :mode="tcMode === 'dup' ? 'plain' : tcView"
        :a-bytes="aBytes"
        :b-bytes="bBytes"
        :a-lines="aLines"
        :b-lines="bLines"
        :blocks="blocks"
        :hl-a="hlA"
        :hl-b="hlB"
        @view="(s, e) => { viewStart = s; viewEnd = e }"
      />

      <DiffMap
        v-if="tcMode === 'diff' && changes"
        :blocks="blocks"
        :current="cur"
        :view-start="viewStart"
        :view-end="viewEnd"
        @pick="go"
      />

      <!-- 查重：这张表留着 —— 它是几行、而且真的在回答「哪些片段两边都有」 -->
      <div v-if="tcMode === 'dup'" class="res">
        <div class="tbody">
          <div class="head hd-dup">
            <span class="no">{{ t('col.id') }}</span>
            <span class="seq">{{ t('tc.sequence') }}</span>
            <span class="len">{{ t('col.len') }}</span>
            <span class="cnt">{{ t('tc.countA') }}</span>
            <span class="cnt">{{ t('tc.countB') }}</span>
            <span class="pos2">{{ t('tc.posA') }}</span>
            <span class="pos2">{{ t('tc.posB') }}</span>
          </div>

          <div v-if="!dupRows.length" class="empty">{{ t('tc.emptyDup') }}</div>

          <div
            v-for="(d, i) in dupRows"
            :key="i"
            class="row2 hd-dup"
            :class="{ sel: cur === i }"
            @click="pickDup(i)"
            @dblclick="allDup"
          >
            <span class="no">{{ i + 1 }}</span>
            <span class="seq" :title="d.Sequence">{{ d.Sequence }}</span>
            <span class="len">{{ d.Length }}</span>
            <span class="cnt">{{ d.CountInA }}</span>
            <span class="cnt">{{ d.CountInB }}</span>
            <span class="pos2" :title="d.PositionsInA.join(', ')">{{ d.PositionsInA.join(', ') }}</span>
            <span class="pos2" :title="d.PositionsInB.join(', ')">{{ d.PositionsInB.join(', ') }}</span>
          </div>
        </div>
      </div>
    </template>

    <!-- 还没比过：一句话说清这一页是干什么的，照终端提示符那套 -->
    <div v-else-if="!editing" class="hint">
      <span class="pr">&gt;</span> {{ tcMode === 'diff' ? t('tc.emptyDiff') : t('tc.emptyDup') }}
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

/* 工具条：.list-page 那套，只补这一页独有的几个 */
.sep { flex: none; width: 1px; height: 16px; background: var(--border); }
.grow { flex: 1; }
.lb { flex: none; color: var(--muted); font-size: var(--btn-size); font-family: var(--share); letter-spacing: .1em; }

.inp.rx { flex: 1; min-width: 160px; }
.inp.rx.bad { border-color: var(--danger); color: var(--danger); }
.inp.num { flex: none; width: 86px; text-align: center; }

/* 差异导航 —— 那张几千行的结果表换成的就是它 */
.nav {
  flex: none;
  display: inline-flex;
  align-items: center;
  gap: 2px;
  padding: 0 4px;
  border: 1px solid var(--border);
  font-family: var(--mono);
  font-size: 11px;
  color: var(--amber);
}

.nav.zero { color: var(--muted); }
.nav b { font-weight: 400; min-width: 1.5em; text-align: center; }
.nav .sl { color: var(--dim); }

.nb {
  padding: 3px 6px;
  background: transparent;
  border: 0;
  color: var(--muted);
  font-size: 13px;
  line-height: 1;
  cursor: pointer;
}

.nb:hover:not(:disabled) { color: var(--cyan); }
.nb:disabled { opacity: .4; cursor: default; }

/* 「编辑」是开关不是动作按钮：开着时要看得出来（青 = 本项目里「当前选中」的语言） */
.btn.on { border-color: var(--cyan); color: var(--cyan); background: rgb(var(--cyan-rgb) / 10%); }

.warn {
  flex: none;
  padding: 4px 8px 3px;
  border: 1px solid rgb(var(--amber-rgb) / 45%);
  color: var(--amber);
  font-family: var(--share);
  font-size: var(--btn-size);
  letter-spacing: .06em;
}

/* 编辑区 */
.io { flex: none; display: flex; gap: 10px; height: 150px; }
.iop { flex: 1; min-width: 0; display: flex; flex-direction: column; border: 1px solid var(--border); }

.ph {
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  height: var(--th-h);
  padding: 4px 10px 0;
  background: var(--panel);
  border-bottom: 1px solid var(--border);
}

.ph .tt { font-family: var(--share); font-size: var(--th-size); letter-spacing: .14em; text-transform: uppercase; color: var(--th-fg); }
.ph .meta { font-family: var(--mono); font-size: 11px; color: var(--dim); }
.ph .meta b { color: var(--soft); font-weight: 400; }

/* 结果区的两栏表头 —— 与下面 DiffView 的两半对齐（各占一半 + 中缝 38px） */
.rh {
  flex: none;
  display: flex;
  align-items: center;
  height: var(--th-h);
  padding: 4px 6px 0;
  background: var(--panel);
  border: 1px solid var(--border);
  border-bottom: 0;
}

.rh .rt {
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.rh .rm { margin-left: 8px; font-family: var(--mono); font-size: 11px; color: var(--dim); }
.rhh { flex: 1; min-width: 0; display: flex; align-items: center; overflow: hidden; }
.rhg { flex: none; width: 38px; }

.hint {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 0 40px;
  border: 1px solid var(--border);
  background: var(--sink);
  color: var(--muted);
  font-family: var(--mono);
  font-size: 12px;
  text-align: center;
}

.hint .pr { color: var(--green); }

/* 查重结果表 */
.res { flex: none; max-height: 220px; display: flex; border: 1px solid var(--border); background: var(--sink); }
.tbody { flex: 1; min-width: 0; overflow: auto; }

.head.hd-dup, .row2.hd-dup {
  display: grid;
  grid-template-columns: 60px minmax(160px, 2fr) 70px 80px 80px minmax(120px, 1fr) minmax(120px, 1fr);
  gap: 8px;
  padding: 0 14px;
  align-items: center;
}

.head { position: sticky; top: 0; z-index: 1; height: var(--th-h); background: var(--panel); border-bottom: 1px solid var(--border); }

.head > span {
  padding-top: 4px;
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.row2 { height: 26px; border-bottom: 1px solid var(--wpe-rowline); cursor: pointer; font-family: var(--mono); font-size: 11.5px; color: var(--gray); }
.row2:hover { background: rgb(var(--chrome-rgb) / 40%); }
.row2.sel { background: rgb(var(--cyan-rgb) / 12%); }
.row2 > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.row2 .no { color: var(--dim); }
.row2 .seq { color: var(--cyan); }
.row2 .len, .row2 .cnt { text-align: center; color: var(--soft); }
.row2 .pos2 { color: var(--dim); }

.empty { padding: 16px 14px; color: var(--muted); font-size: 12px; }
</style>
