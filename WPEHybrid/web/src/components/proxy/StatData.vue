<script setup lang="ts">
/*
  统计数据 —— 对应 WinForms 的 Controls/StatisticalData（六条进度条 + 一张滤镜表 + 「刷新数据」按钮）。

  上面六个仪表：滤镜执行占代理总数的比例，以及五种动作各占滤镜执行的比例；
  下面是滤镜表（来自 FeedList.Filter 的副本）：启用 / 动作 / 执行次数。
  WinForms 要手按「刷新」，这里页面开着时每秒自己刷一次，按钮留着给想立刻看的人。
*/
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { call } from '../../bridge'
import { FeedList, FilterAction, type FilterRow } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { useList } from '../../stores/lists'

interface Stats { ProxyTotal: number; Execute: number; Replace: number; Change: number; Intercept: number; Display: number; NoDisplay: number }

const s = ref<Stats>({ ProxyTotal: 0, Execute: 0, Replace: 0, Change: 0, Intercept: 0, Display: 0, NoDisplay: 0 })
const filters = useList<FilterRow>(FeedList.Filter)
const busy = ref(false)
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

onMounted(() => { void refresh(); timer = window.setInterval(refresh, 1000) })
onBeforeUnmount(() => clearInterval(timer))

function pct(a: number, b: number): number {
  return b > 0 ? Math.min(100, Math.round((a / b) * 10000) / 100) : 0
}

const gauges = computed(() => [
  { key: 'st.execute', a: s.value.Execute, b: s.value.ProxyTotal, cls: 'exe', hint: 'st.executeHint' },
  { key: 'proxy.act.replace', a: s.value.Replace, b: s.value.Execute, cls: 'rep' },
  { key: 'proxy.act.change', a: s.value.Change, b: s.value.Execute, cls: 'chg' },
  { key: 'proxy.act.intercept', a: s.value.Intercept, b: s.value.Execute, cls: 'itc' },
  { key: 'proxy.act.display', a: s.value.Display, b: s.value.Execute, cls: 'dsp' },
  { key: 'proxy.act.hide', a: s.value.NoDisplay, b: s.value.Execute, cls: 'hid' },
] as Array<{ key: Key; a: number; b: number; cls: string; hint?: Key }>)

const ACTION_LABEL: Record<number, Key> = {
  [FilterAction.Replace]: 'proxy.act.replace',
  [FilterAction.Change]: 'proxy.act.change',
  [FilterAction.Intercept]: 'proxy.act.intercept',
  [FilterAction.NoModify_Display]: 'proxy.act.display',
  [FilterAction.NoModify_NoDisplay]: 'proxy.act.hide',
  [FilterAction.None]: 'proxy.act.none',
}

const totalExec = computed(() => filters.value.reduce((n, f) => n + f.ExecutionCount, 0))
</script>

<template>
  <div class="page list-page st">
    <div class="bar">
      <button class="btn primary" :disabled="busy" @click="refresh">{{ t('st.refresh') }}</button>
      <span class="lb">{{ t('st.autoHint') }}</span>
      <span class="grow" />
      <span class="kv">{{ t('st.proxyTotal') }} <b>{{ s.ProxyTotal }}</b></span>
      <span class="kv">{{ t('st.filterExec') }} <b class="c">{{ s.Execute }}</b></span>
    </div>

    <!-- 六个仪表：占比 + 分子 / 分母，条子按动作着色（颜色照代理数据页那组动作配色的语义） -->
    <div class="gauges">
      <div v-for="g in gauges" :key="g.key" class="g" :class="g.cls">
        <div class="gh">
          <span class="nm">{{ t(g.key) }}</span>
          <span class="pc">{{ pct(g.a, g.b).toFixed(pct(g.a, g.b) % 1 ? 2 : 0) }}<i>%</i></span>
        </div>
        <div class="track"><div class="fill" :style="{ width: pct(g.a, g.b) + '%' }" /></div>
        <div class="gf">
          <span class="frac"><b>{{ g.a }}</b> / {{ g.b }}</span>
          <span v-if="g.hint" class="hint">{{ t(g.hint) }}</span>
          <span v-else class="hint">{{ t('st.ofExec') }}</span>
        </div>
      </div>
    </div>

    <div class="tbl">
      <div class="tbody">
        <div class="head">
          <span class="no">{{ t('col.id') }}</span>
          <span class="name">{{ t('col.filterName') }}</span>
          <span class="stt">{{ t('st.status') }}</span>
          <span class="act">{{ t('col.action') }}</span>
          <span class="cnt">{{ t('col.execCount') }}</span>
          <span class="share">{{ t('st.share') }}</span>
        </div>

        <div v-if="!filters.length" class="empty">{{ t('st.emptyFilters') }}</div>

        <div v-for="(f, i) in filters" v-else :key="f.Id" class="row2" :class="{ off: !f.IsEnable }">
          <span class="no">{{ i + 1 }}</span>
          <span class="name" :title="f.Name">{{ f.Name }}</span>
          <span class="stt"><i class="tg" :class="f.IsEnable ? (f.ExecutionCount > 0 ? 'run' : 'on') : 'off'">{{ f.IsEnable ? (f.ExecutionCount > 0 ? t('st.working') : t('col.enable')) : t('st.stopped') }}</i></span>
          <span class="act">{{ t(ACTION_LABEL[f.Action] ?? 'proxy.act.none') }}</span>
          <span class="cnt">{{ f.ExecutionCount }}</span>
          <span class="share">
            <span class="mini"><span class="mf" :style="{ width: pct(f.ExecutionCount, totalExec) + '%' }" /></span>
            <span class="mp">{{ pct(f.ExecutionCount, totalExec).toFixed(0) }}%</span>
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.page { flex: 1; min-width: 0; min-height: 0; display: flex; flex-direction: column; gap: 8px; padding: 10px 12px 12px; }
.lb { font-size: 11.5px; color: #8a94a6; }
.kv { font-family: var(--share); font-size: 10.5px; letter-spacing: .1em; text-transform: uppercase; color: var(--muted); }
.kv b { font-family: var(--mono); font-weight: 400; color: var(--gray); margin-left: 4px; }
.kv b.c { color: var(--cyan); }

.gauges { flex: none; display: grid; grid-template-columns: repeat(3, 1fr); gap: 8px; }

.g { padding: 12px 14px 10px; border: 1px solid var(--border); background: var(--card); --gc: var(--cyan); }
.g.exe { --gc: var(--cyan); }
.g.rep { --gc: #ff8c00; }
.g.chg { --gc: #c084fc; }
.g.itc { --gc: var(--danger); }
.g.dsp { --gc: var(--green); }
.g.hid { --gc: #6b7280; }

.gh { display: flex; align-items: baseline; justify-content: space-between; gap: 10px; }
.nm { font-family: var(--share); font-size: 10.5px; letter-spacing: .14em; text-transform: uppercase; color: #a8b2c0; }
.pc { font-family: var(--orbit); font-size: 22px; font-weight: 700; color: var(--gc); letter-spacing: .02em; }
.pc i { font-style: normal; font-size: 12px; margin-left: 2px; opacity: .8; }

.track { height: 6px; margin: 8px 0 8px; background: rgb(255 255 255 / 5%); border: 1px solid var(--border); overflow: hidden; }
.fill { height: 100%; background: linear-gradient(90deg, var(--gc), color-mix(in srgb, var(--gc) 45%, transparent)); box-shadow: 0 0 8px var(--gc); transition: width .4s ease; }

.gf { display: flex; align-items: baseline; justify-content: space-between; gap: 10px; }
.frac { font-family: var(--mono); font-size: 12px; color: var(--muted); }
.frac b { color: var(--gray); font-weight: 400; }
.hint { font-size: 11px; color: #4b5563; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.tbl { flex: 1; min-height: 0; display: flex; flex-direction: column; border: 1px solid var(--border); background: #000; }
.tbody { flex: 1; min-height: 0; overflow-y: auto; }

.head, .row2 { display: grid; grid-template-columns: 56px minmax(160px, 1fr) 90px 90px 90px 180px; align-items: center; gap: 8px; padding: 0 14px; font-size: 12.5px; }
.row2 { height: 30px; border-bottom: 1px solid rgb(42 42 58 / 45%); color: #cbd5cc; }
.row2:hover { background: rgb(255 255 255 / 3%); }
.row2.off > span:not(.stt) { opacity: .5; }
.row2 > span, .head > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.head > span, .row2 > span { text-align: center; }
.head > span.name, .row2 > span.name { text-align: left; }

.no { color: #4b5563; font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }
.act { color: #a78bfa; }
.cnt { font-family: var(--mono); color: var(--cyan); font-variant-numeric: tabular-nums; }

.tg { display: inline-block; padding: 4px 7px 2px; border: 1px solid; font-family: var(--share); font-size: 10.5px; line-height: 1; letter-spacing: .08em; text-transform: uppercase; font-style: normal; }
.tg.on { color: var(--green); border-color: rgb(0 255 136 / 35%); }
.tg.run { color: var(--cyan); border-color: rgb(0 212 255 / 35%); }
.tg.off { color: var(--danger); border-color: rgb(255 51 102 / 30%); }

.share { display: flex; align-items: center; gap: 8px; }
.mini { flex: 1; height: 4px; background: rgb(255 255 255 / 5%); border: 1px solid var(--border); overflow: hidden; }
.mf { display: block; height: 100%; background: var(--cyan); transition: width .3s; }
.mp { flex: none; width: 36px; text-align: right; font-family: var(--mono); font-size: 11px; color: var(--muted); }

.empty { padding: 40px 20px; text-align: center; color: var(--muted); font-size: 12.5px; }
</style>
