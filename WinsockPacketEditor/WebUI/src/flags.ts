// 归属地 → 国旗图。
//
// 对应 WinForms 的 UiImages.GetFlagByLocation。那边是 DataGridView 的图片列，
// 这里是 <img src="./flags/xx.png">：图片走静态资源（web/public/flags/，272 个 PNG 共 381KB），
// 浏览器缓存，虚拟滚动下 DOM 里最多约 80 个 img。
//
// 【性能上唯一要守住的一条】
// 名称→代码的匹配是<b>207 条的前缀线性扫描</b>（C# 那边也是）。它只能发生在
// 「渲染可见行」的时候，绝不能进 DTO 转换 —— 那是每秒几千行的热路径，
// 放进去就是每秒上百万次字符串比较。
//
// 再加一层按归属地字符串的记忆化：一次抓包会话里不同的归属地通常只有几十个，
// 第一次之后全是 Map 命中。

import { call } from './bridge'

/** 中文国名 → 国家代码。启动时由 C# 一次性给过来（约 4KB）。 */
let table: Array<[string, string]> = []

/** 归属地字符串 → 国家代码。线性扫描的结果缓存在这儿。 */
const memo = new Map<string, string>()

/** 本机/局域网用的默认图，与 WinForms 的 Flag_Local 是同一个文件。 */
const LOCAL = 'Flag_Local'

/**
 * 取一次对照表。ProxyView 挂载时调用。
 *
 * 失败不抛：国旗是锦上添花，拿不到表就一律显示默认图，列表照常能用。
 */
export async function loadCountryTable(): Promise<void> {
  if (table.length) return

  try {
    const d = await call<Record<string, string>>('getCountryTable')
    table = Object.entries(d || {})
  } catch (e) {
    console.error('[flags] 取国名对照表失败，将一律显示默认图', e)
  }
}

/**
 * 归属地字符串对应的国旗文件名（不含扩展名）。
 *
 * 匹配规则与 C# 的 GetFlagByLocation 一致：<b>前缀</b>匹配，取第一条命中的。
 * 归属地形如「中国-江西-南昌 电信」「新加坡阿里云」，国名都在最前面。
 */
export function flagOf(location: string | null | undefined): string {
  if (!location) return LOCAL

  const hit = memo.get(location)
  if (hit !== undefined) return hit

  let code = LOCAL
  for (let i = 0; i < table.length; i++) {
    if (location.startsWith(table[i][0])) {
      code = table[i][1]
      break
    }
  }

  memo.set(location, code)
  return code
}

/** 图片地址。base 是相对的（见 vite.config.ts），所以这里也要相对。 */
export function flagSrc(location: string | null | undefined): string {
  return './flags/' + flagOf(location) + '.png'
}
