// 代理数据列表的状态（代理模式的主列表）。这是整个前端唯一的性能热点，四条约束写在下面。
//
// 数据来源：C# 每 10ms 一拍，每拍最多 FeedBatchMax(200) 条，推 feed:append 事件。
// 上限约 6000 条/秒（B9c 实测），所以这里的每一步都按「每秒几千次」来设计。

import { shallowRef, triggerRef, ref, type ShallowRef, type Ref } from 'vue'
import { on } from '../bridge'
import { FeedList, type PacketRow, type ProxyRow } from '../bridge/types'

/*
  ① 用 shallowRef，绝不用 ref([]) / reactive([])
     后两者会给每一行都套 Proxy 并深度遍历，3000 行/秒下光这一步就吃满一个核。
     shallowRef 只跟踪 .value 这一层引用，行对象原样保留。

  ② 数组就地 push + triggerRef，不 concat
     rows.value = rows.value.concat(batch) 每批都要复制整表；
     列表到 5 万行时就是每秒上百万次元素拷贝。就地 push 是 O(新增条数)。
     shallowRef 认引用相等不会自动触发，所以必须手动 triggerRef —— 这正是它的用途。

  ③ triggerRef 按 rAF 合并
     C# 一秒推 ~30 批，逐批 trigger 就是一秒 30 次重渲染，且和屏幕刷新不同步。
     合并到每帧一次，渲染次数与帧率对齐，多推的批次只是让同一帧的数据更全。

  ④ 冻结行对象
     成本很小（V8 下每行约 0.1µs，3000 行/秒 ≈ 0.3ms/秒），
     换来的是：哪天有人把 shallowRef 手滑改成 ref()，Vue 会跳过已冻结的对象而不是
     去深度代理五万行 —— 这是给约束①上的保险，不是可有可无的装饰。
*/

/** 推送计数，用于 B10e 的验收实测。 */
export interface FeedStat {
  received: number // 累计收到的行数（不受清空影响）
  batches: number
  maxBatch: number
  rate: number // 行/秒，每 500ms 结算
  dropped: number // C# 侧自动清理掉的行数
}

export interface PacketFeed<T> {
  rows: ShallowRef<T[]>
  stat: Ref<FeedStat>
  attach: () => () => void
  clearLocal: () => void
  resetStat: () => void
  /**
   * 自动清理（环形裁剪）裁掉了哪些行。返回取消订阅的函数。
   *
   * 页面的选中集 / 详情面板里挂着的 Id 要靠它收拾 —— 那两份不在 rows 里，
   * 行被裁掉之后它们还指着一条 C# 那边已经没有的封包，右键「编辑」就会撞上「不在列表里」。
   */
  onTrimmed: (fn: (removed: readonly T[]) => void) => () => void
}

/**
 * 造一路封包推送的接收端。
 *
 * 【为什么是工厂】代理模式与注入模式各有一份主列表（FeedList.Proxy / FeedList.Packet），
 * 两份的 Id 序列互相独立，热路径那四条约束却一模一样。
 * 抄第二份的话，哪天优化了一边、另一边就悄悄退化了 —— 这个项目里
 * 「三份手抄抄歪一次」的教训已经有过（见 CLAUDE.md 的 .list-page）。
 */
export function createPacketFeed<T extends { Id: number }>(list: FeedList): PacketFeed<T> {
  /** 全表。始终是<b>同一个数组引用</b>，靠 triggerRef 通知，不做整表复制。 */
  const all: T[] = []
  const rows = shallowRef<T[]>(all)

  const stat = ref<FeedStat>({ received: 0, batches: 0, maxBatch: 0, rate: 0, dropped: 0 })

  let rafId = 0
  let lastRateAt = 0
  let lastRateRows = 0

  /** 裁剪的订阅者（onTrimmed）。通常只有当前挂着的那个数据页一个。 */
  const trimListeners = new Set<(removed: readonly T[]) => void>()

  function scheduleFlush(): void {
    if (rafId) return

    rafId = requestAnimationFrame(() => {
      rafId = 0
      triggerRef(rows)
    })
  }

  /**
   * 接上 C# 的推送。在 App 挂载时调一次。
   * 返回取消订阅的函数（供热更新时清理，正式运行中不会调）。
   */
  function attach(): () => void {
    lastRateAt = performance.now()

    const offAppend = on('feed:append', (d: { list: number; rows: T[] }) => {
      if (d.list !== list) return

      const batch = d.rows
      const n = batch.length
      if (!n) return

      for (let i = 0; i < n; i++) {
        all.push(Object.freeze(batch[i]))
      }

      const s = stat.value
      s.received += n
      s.batches++
      if (n > s.maxBatch) s.maxBatch = n

      scheduleFlush()
    })

    /*
      自动清理不再走 feed:clear，走这条：<b>只留最近 keep 条</b>（2026-09-07 改的）。

      ⚠️ <b>两侧必须裁掉同样的行。</b>C# 那边删了前 M 条，这里也删前 M 条 ——
      内容一旦不一致，点行取字节就会拿到 null。
      报文里给的是「保留多少条」而不是「删掉多少条」：各自算差值，
      中间丢一次事件也只是某一拍多留几行，下一拍就对齐了，不会永久错位。

      splice(0, drop) 是 O(表长) 的一次搬移，与 push 那条约束不冲突 ——
      它一拍最多来一次，不是每条封包一次。
    */
    const offTrim = on('feed:trim', (d: { list: number; keep: number }) => {
      if (d.list !== list) return

      const drop = all.length - d.keep
      if (drop <= 0) return

      //splice 本来就返回被删掉的那一段，交给订阅者收拾它们手里的 Id（没人订阅就不用管它）
      const removed = all.splice(0, drop)
      stat.value.dropped += drop
      scheduleFlush()

      for (const fn of trimListeners) {
        try { fn(removed) } catch (e) { console.error('[feed] onTrimmed 回调出错', e) }
      }
    })
    const offClear = on('feed:clear', (d: { list: number }) => {
      if (d.list !== list) return

      // 用户点「清空」、切库这些走这条。自动清理走 feed:trim，见上面。
      // 两侧的内容一旦不一致，点行取字节就会拿到 null。
      stat.value.dropped += all.length
      all.length = 0
      scheduleFlush()
    })

    const timer = window.setInterval(() => {
      const now = performance.now()
      const dt = now - lastRateAt
      if (dt <= 0) return

      stat.value.rate = Math.round(((stat.value.received - lastRateRows) * 1000) / dt)
      lastRateRows = stat.value.received
      lastRateAt = now
    }, 500)

    return () => {
      offAppend()
      offClear()
      offTrim()
      window.clearInterval(timer)
      if (rafId) cancelAnimationFrame(rafId)
      rafId = 0
    }
  }

  return {
    rows,
    stat,
    attach,
    /** 前端侧清空（C# 的 clearPackets 会推 feed:clear，通常不必手动调这个）。 */
    clearLocal(): void {
      all.length = 0
      triggerRef(rows)
    },
    resetStat(): void {
      stat.value = { received: 0, batches: 0, maxBatch: 0, rate: 0, dropped: 0 }
      lastRateRows = 0
      lastRateAt = performance.now()
    },
    onTrimmed(fn) {
      trimListeners.add(fn)
      return () => { trimListeners.delete(fn) }
    },
  }
}

/*
  ── 两路实例 ──────────────────────────────────────────

  代理模式那一路保持原来的具名导出，调用点一处都不用改。
*/
const proxyFeed = createPacketFeed<ProxyRow>(FeedList.Proxy)

export const rows = proxyFeed.rows
export const attachPacketFeed = proxyFeed.attach
export const resetStat = proxyFeed.resetStat
export const onProxyTrimmed = proxyFeed.onTrimmed

/** 注入模式的主列表（对应 WinForms 的 Controls/PacketList.cs）。 */
export const injectFeed = createPacketFeed<PacketRow>(FeedList.Packet)
