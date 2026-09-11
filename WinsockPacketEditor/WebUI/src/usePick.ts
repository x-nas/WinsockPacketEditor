import { computed, shallowRef, watch, type Ref } from 'vue'

/*
  列表的多选 —— 全项目<b>唯一</b>的一份实现。

  【为什么不是勾选框列】滤镜 / 发送 / 账号那几屏，第一列的勾选框一律是「启用」开关；
  再摆一个长得一模一样的「选中」勾选框，用户分不出哪个是哪个，而点错的代价还不对称
  （一个只是选中，另一个会立刻让某条规则生效或失效）。所以选中一律走按键：

    单击            只选这一行
    Ctrl / Cmd 单击  加选 / 去选
    Shift 单击       从锚点连选

  与资源管理器、以及本程序的封包列表是同一套手势。

  【index 必须是全表下标】虚拟滚动的列表里，v-for 的 i 是<b>窗口内</b>下标，
  调用时要传 `start + i`。传窗口内下标的话，Shift 连选在滚动之后会连错一片。
*/

export interface RowPickOptions {
  /**
   * 整表换掉后自动把已经不存在的 Id 从选中集里剔掉。默认开。
   *
   * <b>封包列表要传 false</b>：它的 rows 每帧都 triggerRef，
   * 挂一个 watch 上去等于每秒跑 60 次，那一屏的帧预算不该花在这儿。
   */
  autoPrune?: boolean
}

/*
  行的键泛型化（K）：配置类列表用 Guid 字符串，封包 / 代理列表用运行期自增的 long（number）。
  写死成 string 的话，封包列表那边每渲染一行都要 String(r.Id) 转一次 —— 没必要。

  用 shallowRef 而不是 ref：选中集<b>永远是整个替换</b>、从不就地改，
  深层响应式在这里只会让 Set<K> 的类型被 Vue 的解包搞成一团。
*/
export function useRowPick<T, K>(
  rows: Ref<readonly T[]>,
  idOf: (row: T) => K,
  options: RowPickOptions = {},
) {
  const picked = shallowRef<Set<K>>(new Set())
  const pickedIds = computed(() => [...picked.value])

  //Shift 连选的锚点，存的是全表下标
  let anchor = -1

  function onRowClick(row: T, ev: MouseEvent, index: number): void {
    const id = idOf(row)

    if (ev.shiftKey && anchor >= 0) {
      const lo = Math.min(anchor, index)
      const hi = Math.max(anchor, index)
      const next = new Set<K>()

      for (let i = lo; i <= hi; i++) {
        const r = rows.value[i]
        if (r) next.add(idOf(r))
      }

      //连选替换掉原来的选中集，锚点不动 —— 可以反复 Shift 调整这一段的范围
      picked.value = next
      return
    }

    if (ev.ctrlKey || ev.metaKey) {
      const next = new Set(picked.value)
      if (next.has(id)) next.delete(id)
      else next.add(id)
      picked.value = next
      anchor = index
      return
    }

    picked.value = new Set([id])
    anchor = index
  }

  function selectAll(): void {
    picked.value = new Set(rows.value.map(idOf))
  }

  function clear(): void {
    picked.value = new Set()
    anchor = -1
  }

  /**
   * 表头被裁掉了一段（封包列表的环形自动清理：只留最近 N 条）。
   *
   * 两件事都要做，缺一件都会静默出错：
   *   ① 选中集里剔掉那些 Id —— 不剔的话右键「编辑」拿去的是一条 C# 那边已经没有的封包；
   *   ② 锚点跟着前移 —— 它存的是<b>全表下标</b>，表头少了 N 行，Shift 连选就会连错 N 行。
   *
   * 返回选中集里被剔掉了几条（调用方据此判断「选中的封包是被自动清理掉的」）。
   * autoPrune 关着的列表（封包列表）靠这个收拾；开着的那些整表 Replace 自己会对。
   */
  function trimHead(removed: readonly T[]): number {
    const n = removed.length
    if (!n) return 0

    if (anchor >= 0) anchor = anchor - n >= 0 ? anchor - n : -1

    if (!picked.value.size) return 0

    let lost = 0
    let next: Set<K> | null = null

    for (const r of removed) {
      const id = idOf(r)
      if (!picked.value.has(id)) continue
      if (!next) next = new Set(picked.value)
      next.delete(id)
      lost++
    }

    if (next) picked.value = next
    return lost
  }

  /*
    整表换掉后对一次选中集：别处删掉的 Id 留着不会出错（C# 侧一律按 Id 取交集），
    但计数会虚高 —— 看着像选了 8 条却只动得了 3 条，比空着更费解。
  */
  if (options.autoPrune !== false) {
    watch(rows, () => {
      const alive = new Set(rows.value.map(idOf))
      const next = new Set<K>()

      for (const id of picked.value) {
        if (alive.has(id)) next.add(id)
      }

      if (next.size !== picked.value.size) picked.value = next
      if (next.size === 0) anchor = -1
    })
  }

  return { picked, pickedIds, onRowClick, selectAll, clear, trimHead }
}
