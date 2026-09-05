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
 * 每一路的上限。
 *
 * 日志没有「自动清理」那套配置（那是封包列表的），所以自己设一个环形上限。
 * 2000 条足够回溯一次启动失败，又不会让 DOM 和内存无限涨。
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

  return () => offs.forEach((f) => f())
}
