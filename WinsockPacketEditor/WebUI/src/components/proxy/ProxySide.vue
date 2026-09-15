<script setup lang="ts">
/*
  代理模式的左侧导航。

  【为什么是一套而不是两套】
  WinForms 的 ProxyModeForm 里有一个 119px 的 AntdUI.Menu（14 项）**和**一个
  AntdUI.Tabs（14 页），两者同步联动 —— 同一件事画了两遍。这里只保留侧栏，
  与官网 WPEWeb 的 .sidebar 一致。

  【为什么要分组】
  WinForms 是平铺 14 项，扫起来很累。按官网 .sb-cap 的做法分成
  Data / Rules / Tools / System 四组，配合右侧计数，一眼能看出哪份表有东西。
*/
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { FeedList } from '../../bridge/types'
import { useList } from '../../stores/lists'
import { injectFeed, rows } from '../../stores/packets'
import { filterLogs, proxyLogs, sysLogs } from '../../stores/logs'
import { t } from '../../i18n'
import type { PageGroup, PageKey } from './pages'
import { GROUPS } from './pages'

/*
  两种模式共用这一份侧栏。

  注入模式的 11 页里有 10 页与代理模式是同一个组件、同一份数据源，
  只有主屏那一页不同（PacketInfo 与 ProxyInfo 各有一套 Id 序列），
  所以差别只在<b>传进来的 groups</b> 与「主屏计数取哪一路推送」。
  抄一份 InjectSide 出来，下场是两边慢慢走样 —— 见 CLAUDE.md 的 .list-page。
*/
const props = withDefaults(
  defineProps<{ current: PageKey; groups?: PageGroup[]; mode?: 'proxy' | 'inject' }>(),
  { groups: () => GROUPS, mode: 'proxy' },
)
const emit = defineEmits<{ (e: 'go', page: PageKey): void }>()

/*
  代理数据与日志的条数<b>按 2Hz 采样，不跟数据源走</b>。

  rows 是每一帧 triggerRef 一次的高频列表（B10 的四条热路径约束之一）；
  三路日志也走 rAF 合并，代理日志在大流量下同样能到几十条每秒。
  把它们写进 computed，整条侧栏就会跟着每秒重渲染几十遍 —— 而这里只是个计数，
  慢半秒没有任何影响。B10 验收的 ① 项就是被这类「顺手挂上高频依赖」吃掉余量的。

  其余几份是中低频配置表（用户点一下才变），直接读副本没问题。
*/
const dataCount = ref(0)
const logCount = ref(0)
let countTimer = 0

function sample(): void {
  dataCount.value = (props.mode === 'inject' ? injectFeed.rows : rows).value.length
  //侧栏那一项对应的是整个日志页，所以给三路之和
  logCount.value = sysLogs.value.length + filterLogs.value.length + proxyLogs.value.length
}

onMounted(() => {
  sample()
  countTimer = window.setInterval(sample, 500)
})

onBeforeUnmount(() => window.clearInterval(countTimer))

/*
  侧栏右侧的计数。

  这些表都走 B9d 的推送通道（FeedPump 订阅 ListChanged 后整表 Replace），
  所以这里只是读前端副本，不产生任何往返。
*/
const counts = computed<Partial<Record<PageKey, number>>>(() => ({
  data: dataCount.value,
  //注入模式的主屏。与 data 同一个采样值 —— 两者不会同时出现在一份 groups 里
  packet: dataCount.value,
  //在线客户端数。RefreshAuthList 每秒重建这份表，所以它跟着变，不必另外采样
  client: useList(FeedList.Auth).value.length,
  account: useList(FeedList.Account).value.length,
  filter: useList(FeedList.Filter).value.length,
  send: useList(FeedList.Send).value.length,
  robot: useList(FeedList.Robot).value.length,
  warehouse: useList(FeedList.WareHouse).value.length,
  wpc: useList(FeedList.Server).value.length,
  log: logCount.value,
}))

function fmt(n: number | undefined): string {
  return n ? (n > 9999 ? Math.round(n / 1000) + 'k' : String(n)) : ''
}
</script>

<template>
  <nav class="side">
    <template v-for="g in props.groups" :key="g.cap">
      <div class="sb-cap">{{ g.cap }}</div>

      <!--
        role="button" + tabindex 而不是真 <button>：与启动页的模式卡同一个理由，
        这里还额外要显示计数徽标，用 div 布局更自由。
      -->
      <div
        v-for="p in g.items"
        :key="p.key"
        class="sb-item"
        :class="{ on: p.key === props.current }"
        role="button"
        tabindex="0"
        :aria-current="p.key === props.current ? 'page' : undefined"
        @click="emit('go', p.key)"
        @keydown.enter.prevent="emit('go', p.key)"
        @keydown.space.prevent="emit('go', p.key)"
      >
        <svg class="ico" viewBox="0 0 24 24" v-html="p.icon" />
        <!-- 加宽之后仍可能被截（比如俄语的「Извлечение данных」），悬停能看到全名 -->
        <span class="t" :title="t(p.label)">{{ t(p.label) }}</span>
        <span class="n">{{ fmt(counts[p.key]) }}</span>
      </div>
    </template>
  </nav>
</template>

<style scoped>
.side {
  border-right: 1px solid var(--border);
  background: rgb(var(--chrome-rgb) / 55%);
  padding: 14px 0 12px;
  overflow-y: auto;
}

.sb-cap {
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .26em;
  text-transform: uppercase;
  color: var(--dim);
  padding: 0 18px;
  margin: 16px 0 7px;
}

.sb-cap:first-child { margin-top: 0; }

.sb-item {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 7px 18px;
  color: var(--muted);
  font-size: var(--fs-body);
  cursor: pointer;
  /* 选中态用左侧竖条，不用整块反色 —— 侧栏很窄，反色会显得脏 */
  border-left: 2px solid transparent;
  transition: color .12s, background .12s;
}

.sb-item .ico { width: 15px; height: 15px; }
.sb-item .t { flex: 1; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.sb-item .n {
  /* 计数比条目名小一号，行内居中后仍偏高 1px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  position: relative;
  top: 1px;
  font-family: var(--share);
  font-size: var(--fs-caption);
  color: var(--dim);
}

.sb-item:hover { color: var(--gray); background: rgb(var(--tint-rgb) / 3%); }

.sb-item.on {
  color: var(--cyan);
  border-left-color: var(--cyan);
  background: linear-gradient(90deg, rgb(var(--cyan-rgb) / 10%), transparent 70%);
}

.sb-item.on .n { color: var(--cyan); }

/* 焦点环画在内侧：侧栏右边就是内容区的边界，正偏移会压过去 */
.sb-item:focus-visible { outline-offset: -2px; }
</style>
