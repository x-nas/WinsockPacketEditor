// 代理数据列表的状态（代理模式的主列表）。这是整个前端唯一的性能热点，四条约束写在下面。
//
// 数据来源：C# 每 10ms 一拍，每拍最多 FeedBatchMax(200) 条，推 feed:append 事件。
// 上限约 6000 条/秒（B9c 实测），所以这里的每一步都按「每秒几千次」来设计。

import { shallowRef, triggerRef, ref } from 'vue'
import { on } from '../bridge'
import { FeedList, type ProxyRow } from '../bridge/types'

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

/** 全表。始终是<b>同一个数组引用</b>，靠 triggerRef 通知，不做整表复制。 */
const all: ProxyRow[] = []

export const rows = shallowRef<ProxyRow[]>(all)

/** 推送计数，用于 B10e 的验收实测。 */
export const stat = ref({
  received: 0, // 累计收到的行数（不受清空影响）
  batches: 0,
  maxBatch: 0,
  rate: 0, // 行/秒，每 500ms 结算
  dropped: 0, // C# 侧自动清理掉的行数
})

let rafId = 0
let lastRateAt = 0
let lastRateRows = 0

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
export function attachPacketFeed(): () => void {
  lastRateAt = performance.now()

  const offAppend = on('feed:append', (d: { list: number; rows: ProxyRow[] }) => {
    if (d.list !== FeedList.Proxy) return

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

  const offClear = on('feed:clear', (d: { list: number }) => {
    if (d.list !== FeedList.Proxy) return

    // C# 的自动清理是<b>整表清空</b>（不是保留最近 N 条），前端必须原样跟随：
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
    window.clearInterval(timer)
    if (rafId) cancelAnimationFrame(rafId)
    rafId = 0
  }
}

/** 前端侧清空（C# 的 clearPackets 会推 feed:clear，通常不必手动调这个）。 */
export function clearLocal(): void {
  all.length = 0
  triggerRef(rows)
}

/** 重置计数器。 */
export function resetStat(): void {
  stat.value = { received: 0, batches: 0, maxBatch: 0, rate: 0, dropped: 0 }
  lastRateRows = 0
  lastRateAt = performance.now()
}
