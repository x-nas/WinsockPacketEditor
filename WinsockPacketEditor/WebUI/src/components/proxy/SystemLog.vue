<script setup lang="ts">
/*
  运行日志 —— 对应 WinForms 的 Controls/LogList（三个标签页 + 三个表格）。

  三路日志的字段完全不同，所以是三份数据、三套列，不是一个表加个筛选：
    系统  时间 · 模块 · 日志内容
    滤镜  时间 · 滤镜名称 · 动作 · 匹配数 · 类别 · 长度
    代理  时间 · 账号 · IP地址 · 日志内容
  列名逐条照抄 LogList.cs 里那三组 AntdUI.Column。

  排在「代理数据」之后先做这一页，是因为**代理起不来时它是唯一的线索** ——
  启停失败的原因全在 Operate.DoLog 里。
*/
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { FilterAction, PACKET_TYPE } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { call } from '../../bridge'
import { pushToast } from '../../stores/toast'
import {
  attachLogFeed, filterLogs, proxyLogs, sysLogs,
} from '../../stores/logs'

type TabKey = 'sys' | 'filter' | 'proxy'

/*
  ⚠️ 注入模式不出「代理日志」那一页。

  代理日志是 `lstProxyLogInfo`，只由 SOCKS5 服务器那条路产出（握手 / 认证 / 转发）——
  而注入模式压根不起代理服务，那一页<b>恒为空</b>。
  摆一个永远是空的页签，读出来是「日志丢了」而不是「本来就没有」。
*/
const props = withDefaults(defineProps<{ mode?: 'proxy' | 'inject' }>(), { mode: 'proxy' })

const tab = ref<TabKey>('sys')
/*
  跟不跟随底部。<b>没有开关，由「你现在在不在底部」自己决定</b>
  —— 下面 onScroll 每次滚动都会重算它。

  于是行为就是 tail -f 那一套：停在底部时新日志推着往下走，
  往上翻一下就立刻停住（onScroll 把它置 false），翻回底部又接着走。
  「日志是拿来回溯的、别把你翻到的位置冲走」这条顾虑因此仍然成立，
  而且不用记住一个开关的状态。

  初值 true：刚打开时列表是空的、天然就在底部，
  这时候跟着最新的走才是日志页该有的样子。
*/
const follow = ref(true)
const box = ref<HTMLElement | null>(null)

/*
  自动清理 —— 对应 WinForms 的 LogList 工具条上那两个控件
  （cbLogList_AutoClear + txtLogList_AutoClear）。

  语义是<b>环形</b>：超过上限就丢掉最旧的、保留最近 N 条，不是整表清空
  （2026-09-07 与封包 / 代理列表一起改的）。日志尤其需要这样 ——
  它是拿来往回翻的，整表清空会在你正查问题时把整屏一次抹掉。

  ⚠️ <b>它与封包列表的自动清理是两份配置</b>：封包那份在 InjectMode 表
  （PacketList_AutoClear，改在「列表设置」弹窗里），日志这份在 SystemConfig 表
  （LogList_AutoClear / LogList_AutoClear_Value），消费点是 LogConfig.List.FlushToFeed。
  所以放在这一页的工具条上，与 WinForms 一致，不并进那个弹窗。

  开关与条数<b>各自单发</b>（桥那边是「字段出现才改」）：勾选框点一下就存，
  条数框失焦或按 Enter 才存 —— 一起发的话，点开关会把正在编辑的半截数字也写进去。

  ⚠️ 这里<b>没有</b>自动滚动的开关，那个 2026-09-07 去掉之后没有加回来：
  跟不跟随由「你现在在不在底部」决定（上面那段），再摆一个落库的开关就是两条真源打架。
*/
const autoClear = ref(true)
const autoClearValue = ref(5000)
const keepInput = ref('5000')
const keepBad = ref(false)

let detach: (() => void) | null = null

onMounted(async () => {
  detach = attachLogFeed()

  try {
    const s = await call<{ autoClear: boolean; autoClearValue: number }>('getLogSetting')
    autoClear.value = !!s?.autoClear
    autoClearValue.value = Number(s?.autoClearValue) || 5000
    keepInput.value = String(autoClearValue.value)
  } catch {
    /* 桥没接上（探针页）时用默认值，不影响这一页的其余部分 */
  }
})
onBeforeUnmount(() => detach?.())

async function toggleAutoClear(): Promise<void> {
  autoClear.value = !autoClear.value
  try { await call('saveLogSetting', { autoClear: autoClear.value }) } catch { /* 存不上不影响本次会话 */ }
}

/** 条数：范围校验在 C# 侧（与列表设置同一条 100~500000），这里只负责别把空串发过去。 */
async function commitKeep(): Promise<void> {
  const n = Number(keepInput.value)

  if (!Number.isFinite(n) || n < 100 || n > 500000) {
    keepBad.value = true
    return
  }

  keepBad.value = false

  if (n === autoClearValue.value) return

  try {
    const r = await call<{ ok: boolean }>('saveLogSetting', { autoClearValue: n })
    if (r?.ok) autoClearValue.value = n
    else keepBad.value = true
  } catch {
    keepBad.value = true
  }
}

const ALL_TABS: Array<{ key: TabKey; label: Key }> = [
  { key: 'sys', label: 'log.sys' },
  { key: 'filter', label: 'log.filter' },
  { key: 'proxy', label: 'log.proxy' },
]

const TABS = computed(() => (props.mode === 'inject'
  ? ALL_TABS.filter((x) => x.key !== 'proxy')
  : ALL_TABS))

/** 当前这一路的总条数，标签上显示。 */
const counts = computed(() => ({
  sys: sysLogs.value.length,
  filter: filterLogs.value.length,
  proxy: proxyLogs.value.length,
}))

/*
  滤镜动作与封包类别都是枚举，按语言预先展平 —— 与封包列表同一个道理，
  逐格调 t() 会把 lang 挂进每一格的依赖表。
*/
const ACTION_LABEL: Record<number, Key> = {
  [FilterAction.Replace]: 'proxy.act.replace',
  [FilterAction.Intercept]: 'proxy.act.intercept',
  [FilterAction.NoModify_Display]: 'proxy.act.display',
  [FilterAction.NoModify_NoDisplay]: 'proxy.act.hide',
  [FilterAction.None]: 'proxy.act.none',
  [FilterAction.Change]: 'proxy.act.change',
}

const actionText = computed<Record<number, string>>(() => {
  const m: Record<number, string> = {}
  for (const k of Object.keys(ACTION_LABEL)) m[+k] = t(ACTION_LABEL[+k])
  return m
})

const typeText = computed<Record<number, string>>(() => {
  const m: Record<number, string> = {}
  for (const k of Object.keys(PACKET_TYPE)) m[+k] = t(PACKET_TYPE[+k] as Key)
  return m
})

/** 出错的那几条要一眼能看见。 */
function isError(text: string): boolean {
  return /exception|error|fail|失败|异常/i.test(text || '')
}

/*
  跟随底部。

  日志量不大（每秒几条），不做虚拟滚动 —— 2000 个 div 对 Chromium 不算什么，
  而虚拟滚动会让「选中一段文字复制」变得别扭，日志恰恰是最需要复制的东西。
*/
function scrollToBottom(): void {
  const el = box.value
  if (el) el.scrollTop = el.scrollHeight
}

watch([sysLogs, filterLogs, proxyLogs, tab], () => {
  if (follow.value) requestAnimationFrame(scrollToBottom)
}, { flush: 'post' })

function onScroll(): void {
  const el = box.value
  if (!el) return
  follow.value = el.scrollTop + el.clientHeight >= el.scrollHeight - 24
}

/* ── 导出 / 清空（对应 WinForms LogList 右键菜单的那两项）────── */

/*
  kind 与 C# 的 LogConfig.List.LogKind 同序：0 系统 / 1 滤镜 / 2 代理。

  【清空要走 C#】原来这里调的是 stores/logs.ts 里一个只清前端环形缓冲的函数，
  C# 那边的队列与 BindingList 纹丝不动 —— 表现是清完之后下一拍搬运又把积压的搬回来，
  而且真正占内存的那一份根本没释放。现在 C# 清完会推 feed:clear，前端副本跟着空，
  本地那个函数因此没人用了，一并删掉（去掉调用方就顺手清被调方）。

  确认框也在 C# 侧（ClearLog_Dialog 里 await UI.Confirm），与防火墙删除同一条路数。
*/
const KIND: Record<TabKey, number> = { sys: 0, filter: 1, proxy: 2 }

const busy = ref(false)

/** 当前这一路有没有东西 —— 空的时候导出与清空都没有意义，压暗。 */
const hasRows = computed(() => counts.value[tab.value] > 0)

async function doClear(): Promise<void> {
  busy.value = true
  try { await call('clearLogs', { kind: KIND[tab.value] }) }
  catch (e) { console.error('[log] 清空失败', e); pushToast('error', String(e)) }
  finally { busy.value = false }
}

async function doExport(): Promise<void> {
  busy.value = true
  try { await call('exportLogs', { kind: KIND[tab.value] }) }
  catch (e) { console.error('[log] 导出失败', e); pushToast('error', String(e)) }
  finally { busy.value = false }
}
</script>

<template>
  <div class="page">
    <div class="bar">
      <!-- 三路日志各是一套字段，切换等于换一张表 -->
      <div class="tabs">
        <button
          v-for="x in TABS"
          :key="x.key"
          class="tb"
          :class="{ on: tab === x.key }"
          @click="tab = x.key"
        >
          {{ t(x.label) }}
          <span class="n">{{ counts[x.key] || '' }}</span>
        </button>
      </div>

      <span class="grow" />

      <!--
        自动清理 + 条数。与封包列表那一份是两套配置（见 script 里的说明），
        所以按 WinForms 的原样放在这一页的工具条上，不进「列表设置」弹窗。

        自动滚动没有开关 —— 在底部就跟随，往上翻就停住（tail -f 那一套）。
      -->
      <span class="pair">
        <button class="chk" :class="{ on: autoClear }" @click="toggleAutoClear"><i />{{ t('log.autoClear') }}</button>
        <input
          v-model="keepInput"
          class="keep"
          :class="{ bad: keepBad }"
          type="number"
          min="100"
          max="500000"
          :disabled="!autoClear"
          :title="t('log.keepHint')"
          @blur="commitKeep"
          @keydown.enter="commitKeep"
        >
      </span>
      <!-- 导出的是<b>整张表</b>，不分选中 —— WinForms 那三个 Save*LogList_Dialog 收的也是整个列表 -->
      <button class="btn" :disabled="!hasRows || busy" @click="doExport">{{ t('pm.toExcel') }}</button>
      <button class="btn" :disabled="!hasRows || busy" @click="doClear">{{ t('proxy.clear') }}</button>
    </div>

    <div ref="box" class="body" @scroll.passive="onScroll">
      <!-- 系统日志 -->
      <template v-if="tab === 'sys'">
        <div class="head sys">
          <span>{{ t('col.time') }}</span><span>{{ t('log.module') }}</span><span>{{ t('log.content') }}</span>
        </div>
        <div v-if="!sysLogs.length" class="empty">{{ t('log.empty') }}</div>
        <div v-for="(r, i) in sysLogs" :key="i" class="row sys" :class="{ bad: isError(r.Content) }">
          <span class="tm">{{ r.Time }}</span>
          <span class="fn">{{ r.FuncName }}</span>
          <span class="ct">{{ r.Content }}</span>
        </div>
      </template>

      <!-- 滤镜日志 -->
      <template v-else-if="tab === 'filter'">
        <div class="head flt">
          <span>{{ t('col.time') }}</span><span>{{ t('log.filterName') }}</span><span>{{ t('log.action') }}</span>
          <span>{{ t('log.matchNum') }}</span><span>{{ t('col.type') }}</span><span>{{ t('col.len') }}</span>
        </div>
        <div v-if="!filterLogs.length" class="empty">{{ t('log.empty') }}</div>
        <div v-for="(r, i) in filterLogs" :key="i" class="row flt">
          <span class="tm">{{ r.Time }}</span>
          <span class="fn">{{ r.FilterName }}</span>
          <span class="ct">{{ actionText[r.Action] ?? r.Action }}</span>
          <span class="num">{{ r.MatchNum }}</span>
          <span class="ct">{{ typeText[r.Type] ?? r.Type }}</span>
          <span class="num">{{ r.Len }}</span>
        </div>
      </template>

      <!-- 代理日志 -->
      <template v-else>
        <div class="head pxy">
          <span>{{ t('col.time') }}</span><span>{{ t('log.account') }}</span>
          <span>{{ t('log.ip') }}</span><span>{{ t('log.content') }}</span>
        </div>
        <div v-if="!proxyLogs.length" class="empty">{{ t('log.empty') }}</div>
        <div v-for="(r, i) in proxyLogs" :key="i" class="row pxy" :class="{ bad: isError(r.Content) }">
          <span class="tm">{{ r.Time }}</span>
          <span class="fn">{{ r.UserName }}</span>
          <span class="ip">{{ r.LoginIP }}</span>
          <span class="ct">{{ r.Content }}</span>
        </div>
      </template>
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

.bar {
  flex: none;
  display: flex;
  align-items: center;
  gap: 12px;
  /*
    换行 —— 与 .list-page .bar / .gtool 同一条理由：六种语言之后
    「Автоматическая прокрутка / Автоматическая очистка」两个开关横过来
    比中文长一倍多，1280 宽的窗口里「清空」会被切掉半个字。
    折成两行比切掉一颗按钮强，下面那块日志本来就是弹性高度。
  */
  flex-wrap: wrap;
  row-gap: 6px;
  padding: 7px 12px;
  border: 1px solid var(--border);
  background: var(--card);
}

/* 换行之后 grow 要有最小宽度，否则窄屏时它先被压成 0、三个页签和右边那排就贴在一起 */
.bar .grow { flex: 1 1 12px; }

.tabs { display: flex; gap: 2px; }

.tb {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 4px 12px;
  background: transparent;
  border: 1px solid transparent;
  color: var(--muted);
  font-size: var(--fs-body);
  cursor: pointer;
}

.tb:hover { color: var(--gray); }
.tb.on { color: var(--cyan); border-color: rgb(var(--cyan-rgb) / 40%); background: rgb(var(--cyan-rgb) / 8%); }
.tb .n { font-family: var(--share); font-size: var(--fs-caption); color: var(--dim); }
/* 没有计数时整个去掉：空 span 仍占着 6px 的 gap，页签文字会被挤得偏左 3px（100% 缩放实测） */
.tb .n:empty { display: none; }
.tb.on .n { color: var(--cyan); }
.tb:focus-visible { outline-offset: -2px; }

/* 基样式在 style.css 的「勾选框 / 单选框」，这里只补工具条的字体与框线色 */
.chk {
  --chk-ring: var(--border);
  gap: 6px;
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
}

/*
  自动清理的条数框。宽度按「6 位数字 + 箭头」定死 —— 跟着内容伸缩的话，
  从 5000 改成 20000 时整条工具条会往右挪一下。

  ⚠️ <b>上下箭头保留</b>，理由见 style.css 里 .gtool .num 那一段：
  设了 color-scheme，原生箭头跟着主题走；全项目别处的数字框也都有。
*/
.keep {
  width: 86px;
  padding: 5px 2px 5px 8px;
  background: var(--panel);
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--mono);
  font-size: var(--btn-size);
  line-height: 1;
  text-align: center;
}

/* 与数据页同一条：勾选框和它的条数框要一起换行，别被拆到两行去 */
.pair { display: inline-flex; align-items: center; gap: 8px; }

.keep:focus { outline: none; border-color: var(--cyan); }
.keep:disabled { opacity: .45; cursor: not-allowed; }
.keep.bad { border-color: var(--danger); color: var(--danger); }

.btn {
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
}

.btn:hover { border-color: var(--cyan); color: var(--cyan); }

.body {
  flex: 1;
  min-height: 0;
  overflow: auto;
  border: 1px solid var(--border);
  background: var(--sink);
  /* 日志是拿来读和复制的，这里要放开全局的 user-select: none */
  user-select: text;
}

.empty { padding: 30px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

/*
  三套列宽。时间列 96px 装得下 HH:mm:ss.fff（12 字符），
  <b>gap 16px</b> 是这次专门加宽的 —— 原先 12px 时时间和模块名几乎粘在一起。
*/
.head,
.row {
  display: grid;
  gap: 16px;
  padding: 1px 14px;
  font-size: var(--fs-body);
  line-height: 1.6;
}

.head {
  position: sticky;
  top: 0;
  z-index: 1;
  height: var(--th-h);
  align-items: center;
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);   /* 与全项目其它表头同一份，--muted 在 --panel 上只有 3.85:1 */
}

.sys { grid-template-columns: 96px 200px 1fr; }
.flt { grid-template-columns: 96px 200px 90px 70px 110px 70px; }
.pxy { grid-template-columns: 96px 140px 150px 1fr; }

.row { color: var(--soft); }
.row:hover { background: rgb(var(--tint-rgb) / 4%); }

.tm { color: var(--muted); font-variant-numeric: tabular-nums; }
.fn { color: var(--cyan); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.ip { color: var(--dim3); }
.num { color: var(--dim3); text-align: right; font-variant-numeric: tabular-nums; }

/* 内容可能很长（异常堆栈），换行而不是撑出横向滚动条 */
.ct { white-space: pre-wrap; word-break: break-word; }

/*
  ── 滤镜日志的对齐 ────────────────────────────────────────

  <b>只作用于 .flt</b>：.tm / .fn / .ct / .num 这几个类三张日志表共用，
  不圈起来的话代理日志那列很长的「内容」也会被居中，堆栈就没法读了。

  逐列的对齐：

    1 时间 · 2 滤镜名称   左   —— 一个是定宽的时间戳、一个是长短不一的名字，
                              两列都是"从左边起读"的东西，居中反而让起点上下乱跳
    3 动作 · 4 匹配数 · 5 类型  居中 —— 都是短标签
    6 长度               表头与内容都靠右 —— 数字右对齐便于比大小；
                              表头若居中，会和右对齐的数字错开半列

  三条规则各自命中不同的列，不靠源码顺序赢平局：
  居中那条 (0,2,1)，左对齐与右对齐两条都带一个 nth/last-child、是 (0,3,1)。
*/
.head.flt > span,
.row.flt > span { text-align: center; }

/* 前两列（时间 / 滤镜名称）表头与内容都靠左 */
.head.flt > span:nth-child(-n+2),
.row.flt > span:nth-child(-n+2) { text-align: left; }

/* 最后一列（长度）表头与内容都靠右 */
.head.flt > span:last-child,
.row.flt > span:last-child { text-align: right; }

.row.bad .ct { color: var(--danger); }
.row.bad .fn { color: var(--danger); }

/*
  三路日志共用。
  表头每格与数据格同名（对齐规则靠这个），于是数据列的字体 / 字号 / 颜色
  （.notes 12px、.ad / .dt 等宽字、.cnt 青色……）会一并漏进表头，看着就是「备注」「数据」比别的表头大。
  这里按格子把它们收回来：表头只认表头自己那一份。(0,2,1) 压得过任何单类名的列规则。
*/
.head > span {
  padding-top: 2px;   /* 这张表头自带 padding 1px + line-height 1.6，实测只偏上 1px，补 2px 即到中线 */
  font-family: inherit;
  font-size: inherit;
  font-weight: inherit;
  letter-spacing: inherit;
  text-transform: inherit;
  color: inherit;
}
</style>
