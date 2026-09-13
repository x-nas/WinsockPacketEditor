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
  （那张表本来就是几行，且真的在回答问题），视图切成不对齐的两栏。2026-09-08 又改了三处：

    ① <b>一次只高亮选中的那一条</b>，不再把所有命中一起点亮。
       全点亮的结果是一片青，看不出哪段是哪段 —— 而「共同的分布在哪儿」这个问题
       交给下面两条<b>覆盖率条</b>回答（A / B 各一条，青色是被共同片段盖住的部分）。
    ② 表里多一列<b>占比</b>（长度 × 次数 ÷ 总字节）：哪一条真正解释了这段数据的大头，
       按长度排是看不出来的 —— 一条 2 字节出现 40 次，比一条 8 字节出现 1 次占得多。
    ③ 与「比较」共用同一个 ‹ n / N › 导航（F3 / Shift+F3），走的是<b>片段</b>不是出现位置。

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

/* ── 查重：覆盖率 ──────────────────────────────────────────
   把所有共同片段在某一侧的出现位置并成互不重叠的区间，再摊成 DiffMap 认的块
   （盖住的记 mod、没盖住的记 same）。⚠️ 同时记下每一段属于哪一条片段 ——
   点覆盖率条要能跳到对应的那一行。 */
function coverage(rows: DupRow[], side: 'a' | 'b', total: number) {
  const marks: Array<{ s: number; e: number; i: number }> = []

  rows.forEach((d, i) => {
    for (const p of (side === 'a' ? d.PositionsInA : d.PositionsInB)) {
      marks.push({ s: p, e: p + d.Length, i })
    }
  })

  marks.sort((x, y) => x.s - y.s)

  const merged: Array<{ s: number; e: number; i: number }> = []
  for (const m of marks) {
    const last = merged[merged.length - 1]
    if (last && m.s <= last.e) { last.e = Math.max(last.e, m.e); continue }
    merged.push({ ...m })
  }

  const blocks: DiffBlock[] = []
  const seq: number[] = []
  let at = 0

  for (const m of merged) {
    const s0 = Math.min(m.s, total)
    const e0 = Math.min(m.e, total)
    if (e0 <= s0) continue
    if (s0 > at) blocks.push({ op: 'same', aStart: at, aLen: s0 - at, bStart: at, bLen: s0 - at })
    blocks.push({ op: 'mod', aStart: s0, aLen: e0 - s0, bStart: s0, bLen: e0 - s0 })
    seq.push(m.i)
    at = e0
  }

  if (at < total) blocks.push({ op: 'same', aStart: at, aLen: total - at, bStart: at, bLen: total - at })

  const covered = merged.reduce((n, m) => n + Math.min(m.e, total) - Math.min(m.s, total), 0)
  return { blocks, seq, pct: total ? Math.round((covered / total) * 100) : 0 }
}

const covA = computed(() => coverage(dupRows.value, 'a', aBytes.value.length))
const covB = computed(() => coverage(dupRows.value, 'b', bBytes.value.length))

/*
  占比：这一条片段一共占了多少字节 ÷ 两边总字节。按长度排看不出「哪条解释了大头」——
  一条 5 字节出现两次，比一条 9 字节出现一次占得多。

  ⚠️ <b>不能 Math.round 成整数百分比。</b> 用户在真实数据上问过「占比全是 0 正常吗」：
  两段合计 11327 字节、片段只有 4 字节时，每条都是 0.1~0.3%，四舍五入之后<b>整列全是 0</b> ——
  算得没错，但这一列就什么都没说。所以按大小给不同精度，小到看不见时也照实写出来。
*/
function share(d: DupRow): string {
  const total = aBytes.value.length + bBytes.value.length
  if (!total) return '0%'

  const p = ((d.Length * (d.CountInA + d.CountInB)) / total) * 100
  if (p >= 10) return p.toFixed(0) + '%'
  if (p >= 1) return p.toFixed(1) + '%'
  if (p > 0) return p.toFixed(2) + '%'
  return '0%'
}

async function runDup(): Promise<void> {
  const a = normalize(textA.value), b = normalize(textB.value)
  if (!a || !b) { pushToast('warning', t('tc.needBoth')); return }

  busy.value = true
  try {
    const r = await call<{ rows: DupRow[] }>('textDuplicates', { a, b, min: Math.max(1, Math.trunc(tcMinBytes.value || 1)) })
    const list = r?.rows ?? []

    /*
        ⚠️ <b>这里以前会把 A / B 改写成规范形态（"AA BB CC"），那是个 bug 的来源，已删。</b>

        表现是「点一次查重没反应，再点一次才出来」：改写 textA / textB 会触发
        下面那个 watch([textA, textB]) -> wipe()，而 watch 是<b>下一拍</b>才跑的，
        那时 ran 已经置 true 了 —— 于是刚算出来的结果被当场清空。
        第二次点的时候文本已经是规范形态、改写成了空操作，watch 不触发，结果才留得住。

        ⚠️ 触发条件是「文本不是规范形态」，而真实入口正好都是：
        「添加到文本 A/B」灌进来的是 copyProxyHex 的结果，<b>一条封包一行、带换行</b>。
        所以这个 bug 在真实用法上是必现的，只有手工贴规范形态时才碰不到。

        而改写本来就没有必要了：位置是<b>字节下标</b>，aBytes / bBytes 由 toBytes(textA) 算出来，
        它自己就会把空白与换行剔掉 —— 排不排版都对得上。
        （老版本需要改写，是因为那时高亮是 textarea 里的<b>字符</b>区间。）
    */
    dupRows.value = list
    blocks.value = []
    truncated.value = false
    ran.value = true
    editing.value = false

    //选中最长的那一条（C# 已按长度倒序），只高亮它 —— 全点亮是一片青，看不出哪段是哪段
    cur.value = -1
    if (list.length) nextTick(() => go(0))
    else { hlA.value = []; hlB.value = []; pushToast('info', t('tc.noDup')) }
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

/** 这一页一共有几处可以跳：比较是差异块，查重是共同片段 */
const hits = computed(() => (tcMode.value === 'diff' ? changes.value : dupRows.value.length))

function go(i: number): void {
  const n = hits.value
  if (!n) return

  cur.value = ((i % n) + n) % n

  if (tcMode.value === 'diff') { dv.value?.scrollToChange(cur.value); return }

  //查重：只高亮这一条
  const d = dupRows.value[cur.value]
  if (!d) return

  hlA.value = d.PositionsInA.map((p) => [p, d.Length] as [number, number])
  hlB.value = d.PositionsInB.map((p) => [p, d.Length] as [number, number])

  /*
      ⚠️ 查重的两侧<b>没有位置对应关系</b>：同一段可能在 A 的 2042、在 B 的 8147。
      早先只按 A 的位置滚，B 那边看到的是它自己的 2042 —— 用户报的「B 好像不会定位」就是这个。

      修法不是「各滚各的」（那就回到两个独立滚动条了），而是<b>把选中的这一处对齐</b>：
      短的那一侧头上垫 |posA - posB| 个空格，于是两处并排落在<b>同一行</b>，
      能直接横着比。padA / padB 由下面那两个 computed 给 DiffView。
  */
  const pa = d.PositionsInA.length ? d.PositionsInA[0] : -1
  const pb = d.PositionsInB.length ? d.PositionsInB[0] : -1
  const at = Math.max(pa, pb)

  //⚠️ 垫格会重建行，DiffView 的 watch(rows) 会把 scrollTop 归零 —— 必须等它跑完再滚
  if (at >= 0) nextTick(() => dv.value?.scrollToUnit(at))
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

/*
  查重模式下两侧的头部各垫多少格 —— 让选中那一处在 A 与 B 里落到同一行。
  没选中、或某一侧没有这一段时都是 0（那就退回「各按自己的偏移铺开」）。
*/
const dupPad = computed(() => {
  const d = tcMode.value === 'dup' ? dupRows.value[cur.value] : undefined
  const pa = d?.PositionsInA[0]
  const pb = d?.PositionsInB[0]
  if (pa === undefined || pb === undefined) return { a: 0, b: 0 }
  return { a: Math.max(0, pb - pa), b: Math.max(0, pa - pb) }
})

/** 选中的那一条在覆盖率条上是第几段（DiffMap 的 current 要的是段下标，不是片段下标） */
function covCursor(cov: { blocks: DiffBlock[]; seq: number[] }, side: 'a' | 'b'): number {
  const at = cov.seq.indexOf(cur.value)
  if (at >= 0) return at

  const d = dupRows.value[cur.value]
  const pos = d ? (side === 'a' ? d.PositionsInA : d.PositionsInB)[0] : undefined
  if (pos === undefined) return -1

  //按位置找它落在第几段（只数被盖住的那些段，与 DiffMap 的 changeIndex 同一把尺）
  let i = -1
  for (const b of cov.blocks) {
    if (b.op === 'same') continue
    i++
    if (pos >= b.aStart && pos < b.aStart + b.aLen) return i
  }

  return -1
}

/** 位置太多时只列前几个，完整的挂在 title 上 */
function brief(list: number[]): string {
  if (list.length <= 6) return list.join(', ')
  return list.slice(0, 6).join(', ') + ' … +' + (list.length - 6)
}

/** 点覆盖率条：DiffMap 给的是「第几段」，换算成「第几条片段」 */
function pickCov(side: 'a' | 'b', segIndex: number): void {
  const map = (side === 'a' ? covA.value : covB.value).seq
  const i = map[segIndex]
  if (i !== undefined) go(i)
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

      <!-- 导航：比较走差异块、查重走共同片段，同一个计数器 -->
      <template v-if="ran">
        <span class="nav" :class="{ zero: !hits }">
          <button class="nb" :disabled="!hits" :title="t('tc.prevHit')" @click="prev">‹</button>
          <b>{{ hits ? cur + 1 : 0 }}</b><span class="sl">/</span><b>{{ hits }}</b>
          <button class="nb" :disabled="!hits" :title="t('tc.nextHit')" @click="next">›</button>
        </span>
        <span v-if="truncated" class="warn">{{ t('tc.tooMany') }}</span>
      </template>

      <span v-if="tcView === 'hex' && !hexish" class="warn">{{ t('tc.notHex') }}</span>

      <span class="sep" />

      <!-- 提示文字只留两三个字（框在 125% 缩放下只剩 70 多像素），整句挂在悬停提示上 -->
      <input v-model="tcRegex" class="inp rx" :class="{ bad: regexBad }" spellcheck="false" :placeholder="t('tc.regexPh')" :title="t('tc.regexTip')">
      <button class="btn" :disabled="!tcRegex" @click="leach">{{ t('tc.leach') }}</button>

      <template v-if="tcMode === 'dup'">
        <span class="lb" :title="t('tc.minHint')">{{ t('tc.minBytes') }}</span>
        <input v-model.number="tcMinBytes" class="inp num" type="number" min="1" max="4096" :title="t('tc.minHint')">
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
      <!--
        查重的覆盖率条：A / B 各一条，青色是被共同片段盖住的部分。
        它回答的是「共同的集中在头部（协议头）还是散落各处」—— 而这正是
        「把所有命中一起点亮」答不上来的（那样是一片青）。点一下跳到那一条片段。
      -->
      <div v-if="tcMode === 'dup' && dupRows.length" class="cov">
        <div class="cov-row">
          <span class="cl">{{ t('tc.textA') }}</span>
          <DiffMap tone="dup" slim :blocks="covA.blocks" :current="covCursor(covA, 'a')" @pick="(i) => pickCov('a', i)" />
          <span class="cp">{{ covA.pct }}%</span>
        </div>
        <div class="cov-row">
          <span class="cl">{{ t('tc.textB') }}</span>
          <DiffMap tone="dup" slim :blocks="covB.blocks" :current="covCursor(covB, 'b')" @pick="(i) => pickCov('b', i)" />
          <span class="cp">{{ covB.pct }}%</span>
        </div>
        <span class="ch">{{ t('tc.covHint') }}</span>
      </div>

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
        :pad-a="dupPad.a"
        :pad-b="dupPad.b"
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
            <span class="cnt">{{ t('tc.share') }}</span>
            <span class="pos2">{{ t('tc.posA') }}</span>
            <span class="pos2">{{ t('tc.posB') }}</span>
          </div>

          <div v-if="!dupRows.length" class="empty">{{ t('tc.emptyDup') }}</div>

          <div
            v-for="(d, i) in dupRows"
            :key="i"
            class="row2 hd-dup"
            :class="{ sel: cur === i }"
            @click="go(i)"
          >
            <span class="no">{{ i + 1 }}</span>
            <span class="seq" :title="d.Sequence">{{ d.Sequence }}</span>
            <span class="len">{{ d.Length }}</span>
            <span class="cnt">{{ d.CountInA }}</span>
            <span class="cnt">{{ d.CountInB }}</span>
            <span class="cnt sh">{{ share(d) }}</span>
            <!-- ⚠️ 位置可能有几十个，全铺出来是一坨读不了的数字。只列前 6 个，完整的挂 title -->
            <span class="pos2" :title="d.PositionsInA.join(', ')">{{ brief(d.PositionsInA) }}</span>
            <span class="pos2" :title="d.PositionsInB.join(', ')">{{ brief(d.PositionsInB) }}</span>
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

/* 基样式在 style.css 的 .inp，这一屏没有需要覆盖的 */

/*
  工具条上的输入框跟着同一行的按钮走（`--btn-size`），与数据页工具条的搜索框同一条口径 ——
  12.5px 夹在一排 10.5px 的按钮与分段按钮中间，提示文字是全场最大的那个（2026-09-11 按要求改）。
  框高仍是 .inp 那 28px，与按钮齐平。
*/
/*
  ⚠️ 下限 160 → 96（2026-09-11）：flex-wrap 按项的「假想宽度」断行，而 flex: 1 的假想宽度就是这个下限 ——
  125% 缩放的默认窗口（1024 CSS 宽）里 160 正好让「还原 / 清空」被甩到第二行。
  96 还放得下「正则…」几个字的提示；宽度够时它照样把剩余空间全吃掉。
*/
.inp.rx { flex: 1; min-width: 96px; font-size: var(--btn-size); }
.inp.rx.bad { border-color: var(--danger); color: var(--danger); }

/* 「几位数字 + 上下箭头」的宽度；箭头是刻意留着的，见上面那段 */
.inp.num { flex: none; width: 86px; padding: 0 0 0 8px; text-align: center; font-size: var(--btn-size); }

/* 差异导航 —— 那张几千行的结果表换成的就是它 */
.nav {
  flex: none;
  display: inline-flex;
  align-items: center;
  gap: 2px;
  padding: 0 4px;
  border: 1px solid var(--border);
  font-family: var(--mono);
  font-size: var(--fs-small);
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
  font-size: var(--fs-lead);
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
/* 里面的 HiliteArea 没有自己的边框，焦点由这层容器表示 —— 与输入框同一种语言 */
.iop:focus-within { border-color: var(--cyan); }

.ph {
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  height: var(--th-h);
  padding: 2px 10px 0;   /* 原 4px 在字体度量覆写之后偏低 1~1.5px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  background: var(--panel);
  border-bottom: 1px solid var(--border);
}

.ph .tt { font-family: var(--share); font-size: var(--th-size); letter-spacing: .14em; text-transform: uppercase; color: var(--th-fg); }
.ph .meta { font-family: var(--mono); font-size: var(--fs-small); color: var(--dim); }
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

.rh .rm { margin-left: 8px; font-family: var(--mono); font-size: var(--fs-small); color: var(--dim); }
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
  font-size: var(--fs-small);
  text-align: center;
}

.hint .pr { color: var(--green); }

/* 查重结果表 */
.res { flex: none; max-height: 220px; display: flex; border: 1px solid var(--border); background: var(--sink); }
.tbody { flex: 1; min-width: 0; overflow: auto; }

.head.hd-dup, .row2.hd-dup {
  display: grid;
  grid-template-columns: 52px minmax(150px, 2fr) 62px 70px 70px 62px minmax(110px, 1fr) minmax(110px, 1fr);
  gap: 8px;
  padding: 0 14px;
  align-items: center;
}

/* 覆盖率：两条 12px 的窄条摞起来，右边一句说明 */
.cov {
  flex: none;
  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;
  gap: 4px 8px;
  padding: 7px 12px;
  border: 1px solid var(--border);
  background: var(--card);
}

.cov-row { display: contents; }

.cov .cl {
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.cov .cp { font-family: var(--mono); font-size: var(--fs-small); color: var(--cyan); text-align: right; min-width: 4ch; }

/* 说明横跨三列、单独一行 */
.cov .ch { grid-column: 1 / -1; margin-top: 2px; font-size: var(--fs-small); color: var(--dim); }


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

.row2 { height: 26px; border-bottom: 1px solid var(--wpe-rowline); cursor: pointer; font-family: var(--mono); font-size: var(--fs-small); color: var(--gray); }
.row2:hover { background: rgb(var(--chrome-rgb) / 40%); }
.row2.sel { background: rgb(var(--cyan-rgb) / 12%); }
.row2 > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.row2 .no { color: var(--dim); }
.row2 .seq { color: var(--cyan); }
/* ⚠️ 数字列<b>表头与内容一起</b>居中 —— 只居中内容的话表头还挂在左边，一眼就看得出没对上 */
.hd-dup > .no, .hd-dup > .len, .hd-dup > .cnt { text-align: center; }
.row2 .len, .row2 .cnt { color: var(--soft); }
/* ⚠️ 必须写在 .cnt 之后：这一格的 class 是「cnt sh」，两条特异度相同，平局时后面的赢 */
.row2 .sh { color: var(--cyan); }
.row2 .pos2 { color: var(--dim); }

.empty { padding: 16px 14px; color: var(--muted); font-size: var(--fs-body); }
</style>
