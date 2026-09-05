// 14 份中低频列表的接收端（B9d）。
//
// 与 stores/packets.ts 的分工：
//   packets.ts  高频那 5 份，走 feed:append 增量追加 + rAF 合并，是性能热点
//   本文件      中低频那 14 份，走 feed:replace 整表替换，代价可忽略
//
// C# 侧对应 ClassObject/FeedPump.cs。那边订阅 BindingList.ListChanged，
// 攒一拍后整表推 —— 所以这里收到的永远是完整快照，不需要自己维护顺序或去重。

import { shallowRef, triggerRef, type ShallowRef } from 'vue'
import { on } from '../bridge'
import { FeedList } from '../bridge/types'

/*
  为什么整表替换就够，不必按行增删：
  这 14 份都是几十到几百行、用户点一下才变一次。整表替换的代价可忽略，
  换来的是「两侧内容不可能对不上」—— 按行增删要前端也维护一份等价的顺序与索引，
  多一处能错的地方，而且错了以后很难复现。
*/

/** 每份列表一个 shallowRef。整表替换，所以不需要 packets.ts 那套就地 push 的技巧。 */
const stores = new Map<FeedList, ShallowRef<any[]>>()

function slot(list: FeedList): ShallowRef<any[]> {
  let s = stores.get(list)
  if (!s) {
    s = shallowRef<any[]>([])
    stores.set(list, s)
  }
  return s
}

/** 取某份列表的响应式引用。组件里直接 `const filters = useList<FilterRow>(FeedList.Filter)`。 */
export function useList<T = any>(list: FeedList): ShallowRef<T[]> {
  return slot(list) as unknown as ShallowRef<T[]>
}

/** 接上 C# 的推送。在 App 挂载时调一次，返回取消订阅的函数。 */
export function attachListFeed(): () => void {
  const offReplace = on('feed:replace', (d: { list: number; rows: any[] }) => {
    const s = slot(d.list)
    s.value = d.rows || []
  })

  const offUpdate = on('feed:update', (d: { list: number; row: any }) => {
    // 单行更新目前只有封包/代理编辑在用（见 UiDialogs.OpenPacketEdit）。
    // 那两份是高频列表，不在本模块管辖内，交给 packets.ts 处理；
    // 这里只处理中低频那 14 份，按 Id 就地换掉。
    const s = stores.get(d.list)
    if (!s || !d.row) return

    const id = d.row.Id
    const i = s.value.findIndex((x) => x.Id === id)
    if (i < 0) return

    // 换一个新数组，shallowRef 才会认；这些表只有几百行，复制代价可忽略
    const next = s.value.slice()
    next[i] = d.row
    s.value = next
  })

  /*
    增量追加。

    【为什么这 14 份里也需要它】整表 Replace 是本模块的默认，理由见文件开头 ——
    几十行的表整表推最省心。代理账号是唯一的例外：它是拿来卖的，几万个是真实规模，
    那时一次整表推是几 MB 的 JSON，而用户新建的往往只有一行。
    C# 侧对应 Operate 里 AddProxyAccount / AddBatchAccounts 的 UI.Feed.Append。

    【必须先认领再处理】feed:append 也是高频那 5 份（封包 / 三种日志）走的通道，
    packets.ts 与 logs.ts 各自按 d.list 过滤。这里靠 stores.get 认领：
    没人 useList 过、也没收到过 Replace 的列表，一律不接。
  */
  const offAppend = on('feed:append', (d: { list: number; rows: any[] }) => {
    const s = stores.get(d.list)
    if (!s || !d.rows || !d.rows.length) return

    // 换新数组，shallowRef 才会认
    s.value = s.value.concat(d.rows)
  })

  /*
    按 Id 删一行。C# 侧对应 DeleteAccount_Dialog_ById 的 UI.Feed.Remove。

    找不到就当已经删过了 —— 静默返回而不是报错：整表 Replace 与增量删可能
    在同一拍里前后脚到达，那时这一行确实已经不在了，不是错误。
  */
  const offRemove = on('feed:remove', (d: { list: number; id: string }) => {
    const s = stores.get(d.list)
    if (!s || !d.id) return

    const i = s.value.findIndex((x) => x.Id === d.id)
    if (i < 0) return

    const next = s.value.slice()
    next.splice(i, 1)
    s.value = next
  })

  const offClear = on('feed:clear', (d: { list: number }) => {
    const s = stores.get(d.list)
    if (!s) return
    s.value = []
    triggerRef(s)
  })

  return () => {
    offReplace()
    offUpdate()
    offAppend()
    offRemove()
    offClear()
  }
}
