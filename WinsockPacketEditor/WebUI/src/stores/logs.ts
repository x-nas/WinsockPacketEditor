// 三路运行日志的接收端。
//
// 通道：Operate.DoLog / DoFilterLog / DoProxyLog → LogConfig 的三个队列 →
// 外壳搬运定时器每拍 FlushToFeed → UI.Feed.Append(FeedList.SystemLog / FilterLog / ProxyLog)。
//
// 【三份必须分开存】
// 它们的 DTO 形状完全不同（见 ClassObject/Ui/FeedRows.cs）：
//   LogRow        Time · FuncName · Content
//   FilterLogRow  Time · FilterName · Action · MatchNum · Type · Len
//   ProxyLogRow   Time · UserName · LoginIP · Content
// 早先图省事合成一个数组，结果代理日志被当系统日志渲染（FuncName 是 undefined、
// 内容却照常显示），滤镜日志更是整行空白 —— 它连 Content 字段都没有。
// WinForms 侧也是三个表格三份 BindingList（Controls/LogList 的三个标签页）。
//
// 【为什么不塞进 stores/packets.ts】
// 那个文件是「唯一的性能热点」，四条约束都是为每秒几千行写的。
// 日志是每秒几条到几十条，混进去只会让那份代码更难读。

import { shallowRef, triggerRef } from 'vue'
import { FeedList, type FilterLogRow, type LogRow, type ProxyLogRow } from '../bridge/types'
import { on } from '../bridge'

/**
 * 每一路在<b>前端这一侧</b>的上限。
 *
 * C# 那边另有一套（LogConfig.List.AutoClear / _Value，默认 5000，可在日志页工具条上改），
 * 会推 feed:trim 过来。这里的 2000 是<b>更严的那一道</b>，理由与那边不同：
 * 这一页刻意不做虚拟滚动（为了能原生选中一段 Ctrl+C 拿走），
 * 所以行数直接等于 DOM 节点数 —— 两千个 div 是 Chromium 还很轻松的量级。
 *
 * ⚠️ 两侧上限不一致<b>不要紧</b>：日志行是自包含的，不像封包那样要按 Id 回 C# 取字节，
 * 所以前端少留几条不会导致「点了取不到」。封包列表那条「两侧必须一致」的约束在这儿不成立。
 */
const MAX = 2000

const sysAll: LogRow[] = []
const filterAll: FilterLogRow[] = []
const proxyAll: ProxyLogRow[] = []

/** 与 packets store 同一个理由用 shallowRef：不给每一行套 Proxy。 */
export const sysLogs = shallowRef<LogRow[]>(sysAll)
export const filterLogs = shallowRef<FilterLogRow[]>(filterAll)
export const proxyLogs = shallowRef<ProxyLogRow[]>(proxyAll)

let rafId = 0
const dirty = new Set<'sys' | 'filter' | 'proxy'>()

/** 与封包列表同一套做法：一帧内多批只触发一次重渲染。 */
function scheduleFlush(which: 'sys' | 'filter' | 'proxy'): void {
  dirty.add(which)
  if (rafId) return

  rafId = requestAnimationFrame(() => {
    rafId = 0
    if (dirty.has('sys')) triggerRef(sysLogs)
    if (dirty.has('filter')) triggerRef(filterLogs)
    if (dirty.has('proxy')) triggerRef(proxyLogs)
    dirty.clear()
  })
}

function append<T>(all: T[], rows: T[], which: 'sys' | 'filter' | 'proxy'): void {
  if (!rows?.length) return

  for (let i = 0; i < rows.length; i++) {
    all.push(Object.freeze(rows[i]) as T)
  }

  //超了从头砍。splice 一次批量删，不要在 push 里逐条 shift
  if (all.length > MAX) {
    all.splice(0, all.length - MAX)
  }

  scheduleFlush(which)
}

function trim<T>(all: T[], keep: number, which: 'sys' | 'filter' | 'proxy'): void {
  const drop = all.length - keep
  if (drop <= 0) return

  all.splice(0, drop)
  scheduleFlush(which)
}

/** 订阅三路日志。返回取消订阅的函数。 */
export function attachLogFeed(): () => void {
  const offs: Array<() => void> = []

  offs.push(on('feed:append', (d: { list: number; rows: any[] }) => {
    if (d.list === FeedList.SystemLog) append(sysAll, d.rows, 'sys')
    else if (d.list === FeedList.FilterLog) append(filterAll, d.rows, 'filter')
    else if (d.list === FeedList.ProxyLog) append(proxyAll, d.rows, 'proxy')
  }))

  offs.push(on('feed:clear', (d: { list: number }) => {
    if (d.list === FeedList.SystemLog) { sysAll.length = 0; scheduleFlush('sys') }
    else if (d.list === FeedList.FilterLog) { filterAll.length = 0; scheduleFlush('filter') }
    else if (d.list === FeedList.ProxyLog) { proxyAll.length = 0; scheduleFlush('proxy') }
  }))

  /*
    自动清理改成环形之后（2026-09-07），C# 侧裁到最近 N 条会推这条过来。

    这里给的是「保留多少条」而不是「删掉多少条」，与封包列表同一条约定：
    中间丢一次事件也只是某一拍多留几行，下一拍就对齐了，不会永久错位。

    实际上多半是空跑 —— 上面那个 MAX 更严（2000 < 默认 5000），
    真正裁到的是 append 里那一句。但把上限调到 1000 以下时这条就生效了，
    而且契约摆在这儿比「反正轮不到它」可靠。
  */
  offs.push(on('feed:trim', (d: { list: number; keep: number }) => {
    const keep = Math.max(0, Number(d.keep) || 0)

    if (d.list === FeedList.SystemLog) trim(sysAll, keep, 'sys')
    else if (d.list === FeedList.FilterLog) trim(filterAll, keep, 'filter')
    else if (d.list === FeedList.ProxyLog) trim(proxyAll, keep, 'proxy')
  }))

  return () => offs.forEach((f) => f())
}
