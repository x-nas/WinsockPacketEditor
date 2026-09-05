/*
  表头排序 —— 全项目唯一的一份（客户端列表 / 账号列表 / 进程设置 / 防火墙名单四张表在用）。

  对应 WinForms 那几张 AntdUI 表格上的 SortMode：点表头排一次、再点反向。
  这里多一档：**再点第三下回到原始顺序**。

  为什么要那第三档 —— 这几张表的原始顺序本身有意义：
  客户端列表是按认证先后排的、防火墙名单是按加入先后排的。
  两档循环的话，排过一次就再也回不去，只能刷新整页。

  ⚠️ <b>不排滤镜 / 发送 / 机器人 / 仓库那四张表。</b>
  那四份列表的<b>顺序就是数据</b>（DoWork 是按列表下标循环的，右键还有置顶 / 上移），
  给它们加排序等于让界面显示的顺序和实际执行的顺序对不上。

  ⚠️ <b>不改源数组。</b>传进来的通常是 store 里的 shallowRef 副本，
  Array.prototype.sort 是就地排序，直接排会把推送来的那份表也搅乱。
*/
import { computed, ref, type ComputedRef, type Ref } from 'vue'

export type SortDir = 'asc' | 'desc'

/** 一列的取值函数。返回数字按数值比，返回字符串按本地化比较（中文按拼音）。 */
export type SortGetter<T> = (row: T) => string | number

/**
 * 点四段 IP 转成可比较的数字。
 *
 * 直接按字符串比 IP 是错的：`10.10.10.9` 会排在 `10.10.10.10` 后面。
 * 认不出来的（IPv6、IP 段写法）回 -1，统一沉到最前，至少同类的挨在一起。
 */
export function ipKey(ip: string | null | undefined): number {
  const m = /^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$/.exec((ip || '').trim())
  if (!m) return -1

  let n = 0
  for (let i = 1; i <= 4; i++) {
    const b = Number(m[i])
    if (b > 255) return -1
    n = n * 256 + b
  }
  return n
}

/** "yyyy-MM-dd HH:mm:ss" / "HH:mm:ss" → 可比较的数字；空值排最后。 */
export function timeKey(s: string | null | undefined): number {
  const v = (s || '').trim()
  if (!v) return Number.MAX_SAFE_INTEGER

  const t = Date.parse(v.replace(' ', 'T'))
  if (!isNaN(t)) return t

  //只有时分秒的（客户端列表的认证时间就是这种）：按当天算，比的是先后
  const m = /^(\d{1,2}):(\d{2}):(\d{2})/.exec(v)
  if (m) return (Number(m[1]) * 60 + Number(m[2])) * 60 + Number(m[3])

  return Number.MAX_SAFE_INTEGER
}

export interface UseSort<T> {
  /** 当前排序列；空串 = 原始顺序 */
  key: Ref<string>
  dir: Ref<SortDir>
  /** 点表头：同一列 升 → 降 → 原始；换一列从升开始 */
  toggle: (k: string) => void
  /** 表头要显示的箭头：'' / '↑' / '↓' */
  mark: (k: string) => string
  /**
   * 这一列是不是当前排序列（表头据此点亮）。
   *
   * 有了它模板里就不用写 `sort.key.value === 'ip'` —— <b>那个 .value 是省不掉的</b>：
   * 模板只会自动解包顶层的 ref，`sort` 是个普通对象，里面的 ref 不解包。
   * 写成方法调用既短又不会哪天漏个 .value 变成恒 false（那种错不报，只是排序高亮永远不亮）。
   */
  active: (k: string) => boolean
  /** 排好的副本。key 为空时原样返回（连拷贝都省了） */
  sorted: ComputedRef<T[]>
}

/**
 * @param source   要排的行（store 副本或筛选结果）
 * @param getters  列名 → 取值函数。只有这里列出的列才可点
 */
export function useSort<T>(source: Ref<T[]> | ComputedRef<T[]>, getters: Record<string, SortGetter<T>>): UseSort<T> {
  const key = ref('')
  const dir = ref<SortDir>('asc')

  function toggle(k: string): void {
    if (!getters[k]) return

    if (key.value !== k) { key.value = k; dir.value = 'asc'; return }
    if (dir.value === 'asc') { dir.value = 'desc'; return }

    //第三下：回到原始顺序
    key.value = ''
    dir.value = 'asc'
  }

  function mark(k: string): string {
    if (key.value !== k) return ''
    return dir.value === 'asc' ? '↑' : '↓'
  }

  function active(k: string): boolean {
    return key.value === k
  }

  const sorted = computed<T[]>(() => {
    const g = getters[key.value]
    if (!g) return source.value

    const sign = dir.value === 'asc' ? 1 : -1

    /*
      带原始下标一起排 —— JS 的 sort 在各引擎上虽然都已是稳定的，
      但这里还要保证「比较相等的两行维持原始先后」在<b>降序</b>时也成立：
      单纯反号会把相等的那批也倒过来，看着像是每点一次都在洗牌。
    */
    return source.value
      .map((row, i) => ({ row, i, v: g(row) }))
      .sort((a, b) => {
        let c: number

        if (typeof a.v === 'number' && typeof b.v === 'number') {
          c = a.v - b.v
        } else {
          //中文按拼音、数字串按数值（'10' > '9'），localeCompare 的 numeric 一起管了
          c = String(a.v).localeCompare(String(b.v), undefined, { numeric: true, sensitivity: 'base' })
        }

        return c !== 0 ? c * sign : a.i - b.i
      })
      .map((x) => x.row)
  })

  return { key, dir, toggle, mark, active, sorted }
}
