<script setup lang="ts">
/*
  统计数据 —— 对应 WinForms 的 Controls/StatisticalData（六条进度条 + 一张滤镜表 + 「刷新数据」按钮）。

  ══ 2026-09-08 整屏重做，先说清为什么 ══

  老版本是六个并排的仪表，问题在于<b>它们不是并列关系</b>：
  第一个的分母是「封包总数」，另外五个的分母是「滤镜执行次数」——
  摆成一个 3×2 的等价网格，看不出这其实是<b>两级漏斗</b>（总数 → 命中 → 五种动作）。

  现在分两块：
    ① <b>命中</b>：总数 → 命中封包 → 执行次数，一条进度条 + 三个数。
       新增「命中封包」（<b>被滤镜改过的封包条数</b>）—— 它与执行次数不是一回事：
       一条封包可能被好几条滤镜依次处理，两个一起看才知道「命中面有多宽、每条被处理几次」。
    ② <b>动作构成</b>：五种动作合成<b>一条堆叠条</b>。
       每执行一次恰好命中一种动作，所以五项之和必须等于执行次数 ——
       堆叠条把这层关系画出来了，五个独立进度条画不出来。
       ⚠️ 并且<b>真去核对这个恒等式</b>：对不上就在界面上说出来（差多少）。
       这与「未计入统计的封包类型要记一条日志」是同一条规矩：默认不出声的东西要出声。

  ══ 顺带修掉的两个 bug ══

    ① <b>注入模式下六个数全是 0。</b> 滤镜那六个全局计数在 DoFilterList 里递增，
       而 DoFilterList 跑在<b>目标进程</b>的钩子线程上 —— 外壳自己那份从头到尾是 0，
       分母（TotalPackets 由 ShellLink.Ingest 维护）却是有的，于是百分比恒为 0。
       现在 Stats 事件把这六个也报上来（IpcProtocol.Version 2 → 3）。
    ② <b>计数没有任何办法归零。</b> WinForms 的「清空」顺手复位这一组计数器，
       外壳的 clearPackets 只清列表 —— 于是这一页的数字只增不减。
       现在清空会复位（与 WinForms 逐条对应），这一页另有一个「归零」只清滤镜那一组。
*/
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, FilterAction, type FilterRow } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { useList } from '../../stores/lists'
import { useSort } from '../../useSort'
import { pushToast } from '../../stores/toast'

interface Stats {
  ProxyTotal: number; Hit: number; Execute: number
  Replace: number; Change: number; Intercept: number; Display: number; NoDisplay: number
}

/*
  分母的口径按模式换。C# 侧 GetFilterStats 已经按 SelectMode 取了
  （注入模式用 PacketConfig.Packet.TotalPackets），这里只是把标签说对 ——
  在注入模式下写着「代理总数」而数字来自封包计数，比不显示更误导。
*/
const props = withDefaults(defineProps<{ mode?: 'proxy' | 'inject' }>(), { mode: 'proxy' })

const ZERO: Stats = { ProxyTotal: 0, Hit: 0, Execute: 0, Replace: 0, Change: 0, Intercept: 0, Display: 0, NoDisplay: 0 }

const s = ref<Stats>({ ...ZERO })
const filters = useList<FilterRow>(FeedList.Filter)
const busy = ref(false)
const live = ref(true)
let timer = 0

async function refresh(): Promise<void> {
  busy.value = true
  try {
    const r = await call<Stats>('getFilterStats')
    if (r) s.value = r
  } catch (e) {
    console.error('[st] 取统计失败', e)
  } finally {
    busy.value = false
  }
}

/*
  ⚠️ 「实时」是个开关，所以定时器要跟着它起停 —— 别只在 onMounted 里起一次。
  暂停的用途很实在：数字每秒跳一次的时候，想抄下某个值都抄不准。
*/
watch(live, (on) => {
  window.clearInterval(timer)
  timer = 0
  if (on) timer = window.setInterval(refresh, 1000)
}, { immediate: true })

onMounted(() => { void refresh() })
onBeforeUnmount(() => window.clearInterval(timer))

async function reset(): Promise<void> {
  try {
    await call('resetFilterStats')
    await refresh()
    pushToast('success', t('st.reseted'))
  } catch (e) {
    console.error('[st] 归零失败', e)
  }
}

/* ── 数 ────────────────────────────────────────────────────── */

function pct(a: number, b: number): number {
  return b > 0 ? Math.min(100, Math.round((a / b) * 10000) / 100) : 0
}

/** 百分比的写法：整数就不写小数，很小但非零时保留两位（别显示成 0%） */
function pctText(a: number, b: number): string {
  const v = pct(a, b)
  if (!v) return '0'
  if (v >= 10) return v.toFixed(0)
  if (v >= 1) return v.toFixed(1)
  return v.toFixed(2)
}

const ACTIONS = computed(() => [
  { key: 'proxy.act.replace' as Key, n: s.value.Replace, cls: 'rep' },
  { key: 'proxy.act.change' as Key, n: s.value.Change, cls: 'chg' },
  { key: 'proxy.act.intercept' as Key, n: s.value.Intercept, cls: 'itc' },
  { key: 'proxy.act.display' as Key, n: s.value.Display, cls: 'dsp' },
  { key: 'proxy.act.hide' as Key, n: s.value.NoDisplay, cls: 'hid' },
])

const actionSum = computed(() => ACTIONS.value.reduce((n, a) => n + a.n, 0))

/*
  ⚠️ 自检：每执行一次恰好命中一种动作，所以五项之和必须等于执行次数。
  对不上说明有一支动作没被计数（与「switch 没有 default」是同一类缺口）——
  与其让用户看着一堆自洽的百分比，不如把差值摆出来。
*/
const sumGap = computed(() => s.value.Execute - actionSum.value)

/** 平均每条命中的封包被处理几次。1.0 = 每条只被一条滤镜碰过 */
const perHit = computed(() => (s.value.Hit > 0 ? s.value.Execute / s.value.Hit : 0))

const noData = computed(() => s.value.ProxyTotal === 0 && s.value.Execute === 0)

/* ── 滤镜表 ────────────────────────────────────────────────── */

const ACTION_LABEL: Record<number, Key> = {
  [FilterAction.Replace]: 'proxy.act.replace',
  [FilterAction.Change]: 'proxy.act.change',
  [FilterAction.Intercept]: 'proxy.act.intercept',
  [FilterAction.NoModify_Display]: 'proxy.act.display',
  [FilterAction.NoModify_NoDisplay]: 'proxy.act.hide',
  [FilterAction.None]: 'proxy.act.none',
}

const ACTION_CLS: Record<number, string> = {
  [FilterAction.Replace]: 'rep',
  [FilterAction.Change]: 'chg',
  [FilterAction.Intercept]: 'itc',
  [FilterAction.NoModify_Display]: 'dsp',
  [FilterAction.NoModify_NoDisplay]: 'hid',
}

/*
  ⚠️ 这里<b>可以</b>排序，而滤镜列表页不行 —— 两者不是一回事。

  那条「滤镜 / 发送 / 机器人 / 仓库四张表刻意不排」的规矩，理由是那几份列表的
  <b>顺序就是数据</b>（DoWork 按下标循环），排了会让界面显示的顺序与实际执行的顺序对不上。
  这一页是<b>只读的统计视图</b>，排序不会影响任何执行顺序；而「哪条滤镜最忙」
  正是这一页要回答的问题。

  序号列显示的始终是它在滤镜列表里的<b>原始位置</b>，所以排过之后也知道它排第几条。
*/
const ranked = computed(() => filters.value.map((f, i) => ({ f, no: i + 1 })))

const sort = useSort(ranked, {
  exec: (r) => r.f.ExecutionCount,
})

const totalExec = computed(() => filters.value.reduce((n, f) => n + f.ExecutionCount, 0))
</script>

<template>
  <div class="page list-page st">
    <div class="bar">
      <button class="btn" :disabled="busy" @click="refresh">{{ t('st.refresh') }}</button>

      <div class="hx-seg" :title="t('st.liveHint')">
        <button class="hx-segb after" :class="{ on: live }" @click="live = true">{{ t('st.live') }}</button>
        <button class="hx-segb before" :class="{ on: !live }" @click="live = false">{{ t('st.paused') }}</button>
      </div>

      <span class="grow" />

      <button class="btn warn" :title="t('st.resetHint')" @click="reset">{{ t('st.reset') }}</button>
    </div>

    <!-- ① 命中：总数 → 命中封包 → 执行次数 -->
    <div class="card">
      <div class="ch">
        <span class="nm">{{ t('st.funnel') }}</span>
        <span v-if="noData" class="note">{{ t('st.noData') }}</span>
      </div>

      <div class="nums">
        <div class="n">
          <span class="k">{{ t(props.mode === 'inject' ? 'st.packetTotal' : 'st.proxyTotal') }}</span>
          <b class="v">{{ s.ProxyTotal.toLocaleString() }}</b>
        </div>
        <div class="sep" aria-hidden="true">›</div>
        <div class="n" :title="t('st.hitHint')">
          <span class="k">{{ t('st.hit') }}</span>
          <b class="v hit">{{ s.Hit.toLocaleString() }}</b>
          <span class="p">{{ pctText(s.Hit, s.ProxyTotal) }}%</span>
        </div>
        <div class="sep" aria-hidden="true">›</div>
        <div class="n">
          <span class="k">{{ t('st.filterExec') }}</span>
          <b class="v exe">{{ s.Execute.toLocaleString() }}</b>
          <span v-if="s.Hit" class="p">{{ perHit.toFixed(perHit % 1 ? 2 : 0) }} × {{ t('st.perHit') }}</span>
        </div>
      </div>

      <div class="track"><div class="fill hit" :style="{ width: pct(s.Hit, s.ProxyTotal) + '%' }" /></div>
    </div>

    <!-- ② 动作构成：一条堆叠条 + 图例 -->
    <div class="card">
      <div class="ch">
        <span class="nm">{{ t('st.actions') }}</span>
        <span class="note">{{ t('st.actionsHint') }}</span>
        <span v-if="sumGap" class="bad">{{ t('st.sumBad') }} {{ sumGap }}</span>
      </div>

      <div class="stack">
        <div
          v-for="a in ACTIONS"
          :key="a.key"
          class="seg"
          :class="a.cls"
          :style="{ width: pct(a.n, s.Execute) + '%' }"
          :title="t(a.key) + ' · ' + a.n"
        />
        <div v-if="!s.Execute" class="seg none" style="width: 100%" />
      </div>

      <div class="legend">
        <div v-for="a in ACTIONS" :key="a.key" class="lg" :class="a.cls">
          <i class="dot" />
          <span class="k">{{ t(a.key) }}</span>
          <b class="v">{{ a.n.toLocaleString() }}</b>
          <span class="p">{{ pctText(a.n, s.Execute) }}%</span>
        </div>
      </div>
    </div>

    <!-- ③ 滤镜表 -->
    <div class="tbl">
      <div class="tbody">
        <div class="head">
          <span class="no">{{ t('col.id') }}</span>
          <span class="name">{{ t('col.filterName') }}</span>
          <span class="md">{{ t('st.mode') }}</span>
          <span class="stt">{{ t('st.status') }}</span>
          <span class="act">{{ t('col.action') }}</span>
          <span class="cnt so" :class="{ on: sort.active('exec') }" :title="t('st.byExec')" @click="sort.toggle('exec')">
            {{ t('col.execCount') }}<i class="ar">{{ sort.mark('exec') }}</i>
          </span>
          <span class="share">{{ t('st.share') }}</span>
        </div>

        <div v-if="!filters.length" class="empty">{{ t('st.emptyFilters') }}</div>

        <div v-for="r in sort.sorted.value" v-else :key="r.f.Id" class="row2" :class="{ off: !r.f.IsEnable }">
          <span class="no">{{ r.no }}</span>
          <span class="name" :title="r.f.Name">{{ r.f.Name }}</span>
          <span class="md">{{ t(r.f.Mode === 1 ? 'flt.e.advanced' : 'flt.e.normal') }}</span>
          <span class="stt">
            <i class="tg" :class="!r.f.IsEnable ? 'off' : (r.f.ExecutionCount > 0 ? 'run' : 'on')">
              {{ !r.f.IsEnable ? t('st.stopped') : (r.f.ExecutionCount > 0 ? t('st.working') : t('st.idle')) }}
            </i>
          </span>
          <span class="act" :class="ACTION_CLS[r.f.Action]">{{ t(ACTION_LABEL[r.f.Action] ?? 'proxy.act.none') }}</span>
          <span class="cnt">{{ r.f.ExecutionCount.toLocaleString() }}</span>
          <span class="share">
            <span class="sbar"><span class="mf" :style="{ width: pct(r.f.ExecutionCount, totalExec) + '%' }" /></span>
            <span class="mp">{{ pctText(r.f.ExecutionCount, totalExec) }}%</span>
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.page { flex: 1; min-width: 0; min-height: 0; display: flex; flex-direction: column; gap: 8px; padding: 10px 12px 12px; }

/* ── 卡片 ── */
.card { flex: none; padding: 10px 14px 12px; border: 1px solid var(--border); background: var(--card); }

.ch { display: flex; align-items: baseline; gap: 12px; }
.nm { flex: none; font-family: var(--share); font-size: var(--th-size); letter-spacing: .14em; text-transform: uppercase; color: var(--th-fg); }
.note { font-size: var(--fs-small); color: var(--dim2); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; min-width: 0; }
.bad { flex: none; font-size: var(--fs-small); color: var(--danger); }

/* ── 漏斗那三个数 ── */
.nums { display: flex; align-items: baseline; gap: 14px; margin: 10px 0 9px; flex-wrap: wrap; }
.n { display: flex; align-items: baseline; gap: 8px; min-width: 0; }
.n .k { font-family: var(--share); font-size: var(--fs-label); letter-spacing: .1em; text-transform: uppercase; color: var(--muted); white-space: nowrap; }
.n .v { font-family: var(--orbit); font-size: var(--fs-num-lg); font-weight: 700; color: var(--gray); letter-spacing: .02em; }
.n .v.hit { color: var(--amber); }
.n .v.exe { color: var(--cyan); }
.n .p { font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); white-space: nowrap; }
.sep { flex: none; font-size: 16px; color: var(--dim); }

.track { height: 6px; background: rgb(var(--tint-rgb) / 5%); border: 1px solid var(--border); overflow: hidden; }
.fill { height: 100%; transition: width .4s ease; }
.fill.hit { background: linear-gradient(90deg, var(--amber), rgb(var(--amber-rgb) / 45%)); box-shadow: 0 0 8px rgb(var(--amber-rgb) / 60%); }

/* ── 堆叠条 ──
   五段拼成一条，段与段之间用 1px 的底色缝隔开（不用 gap：那会让宽度百分比对不上）。 */
.stack { display: flex; height: 14px; margin: 10px 0 10px; border: 1px solid var(--border); background: rgb(var(--tint-rgb) / 5%); overflow: hidden; }
.seg { height: 100%; transition: width .4s ease; }
.seg + .seg { box-shadow: inset 1px 0 0 var(--card); }

.rep, .lg.rep .dot { --ac: var(--acc-orange); }
.chg, .lg.chg .dot { --ac: var(--acc-violet); }
.itc, .lg.itc .dot { --ac: var(--danger); }
.dsp, .lg.dsp .dot { --ac: var(--green); }
.hid, .lg.hid .dot { --ac: var(--muted); }

.seg.rep, .seg.chg, .seg.itc, .seg.dsp, .seg.hid { background: var(--ac); }
.seg.none { background: repeating-linear-gradient(45deg, transparent 0 5px, rgb(var(--tint-rgb) / 6%) 5px 10px); }

.legend { display: flex; flex-wrap: wrap; gap: 6px 18px; }
.lg { display: flex; align-items: baseline; gap: 6px; }
.lg .dot { width: 8px; height: 8px; background: var(--ac); flex: none; align-self: center; }
.lg .k { font-size: var(--fs-small); color: var(--soft); }
.lg .v { font-family: var(--mono); font-size: var(--fs-body); font-weight: 400; color: var(--gray); }
.lg .p { font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); }

/* ── 表 ── */
.tbl { flex: 1; min-height: 0; display: flex; flex-direction: column; border: 1px solid var(--border); background: var(--sink); }
.tbody { flex: 1; min-height: 0; overflow-y: auto; }

.head, .row2 { display: grid; grid-template-columns: 56px minmax(150px, 1fr) 74px 90px 90px 100px 170px; align-items: center; gap: 8px; padding: 0 14px; font-size: var(--fs-body); }
.row2 { height: 30px; border-bottom: 1px solid rgb(var(--border-rgb) / 45%); color: var(--soft); }
.row2:hover { background: rgb(var(--tint-rgb) / 3%); }
.row2.off > span:not(.stt) { opacity: .5; }
.row2 > span, .head > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.head > span, .row2 > span { text-align: center; }
.head > span.name, .row2 > span.name { text-align: left; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }
.md { color: var(--muted); font-size: var(--fs-small); }
.cnt { font-family: var(--mono); color: var(--cyan); font-variant-numeric: tabular-nums; }

/* 动作那一列跟着堆叠条的配色 —— 表与图对得上，扫一眼就知道这条属于哪一段 */
.act.rep { color: var(--acc-orange); }
.act.chg { color: var(--acc-violet); }
.act.itc { color: var(--danger); }
.act.dsp { color: var(--green); }
.act.hid { color: var(--muted); }

.tg { display: inline-block; padding: 3px 7px 3px; border: 1px solid; font-family: var(--share); font-size: var(--fs-label); line-height: 1; letter-spacing: .08em; text-transform: uppercase; font-style: normal; }
.tg.on { color: var(--green); border-color: rgb(var(--green-rgb) / 35%); }
.tg.run { color: var(--cyan); border-color: rgb(var(--cyan-rgb) / 35%); }
.tg.off { color: var(--danger); border-color: rgb(var(--danger-rgb) / 30%); }

.share { display: flex; align-items: center; gap: 8px; }
.sbar { flex: 1; height: 4px; background: rgb(var(--tint-rgb) / 5%); border: 1px solid var(--border); overflow: hidden; }
.sbar > .mf { display: block; height: 100%; background: var(--cyan); transition: width .3s; }
.mp { flex: none; width: 42px; text-align: right; font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); }

.empty { padding: 40px 20px; text-align: center; color: var(--muted); font-size: var(--fs-body); line-height: 1.8; }
</style>
