// 十六进制渲染。抽成独立模块是为了让验收跑测能量到<b>与面板完全相同</b>的那份工作量
// —— 只测桥的往返而不含格式化，量出来的数会偏乐观。

/** 每行默认字节数。宽度算不出来时（首帧、面板被隐藏）退回它。 */
export const DEFAULT_PER_LINE = 16

/**
 * 一行占多少个字符。
 *
 * 布局：偏移(8) + 两空格 + 十六进制(每字节 "XX " 共 3) + 一空格 + ASCII(每字节 1)
 * 也就是 11 + 4N。反过来解 N 就是 perLineFor 的算法。
 */
export function lineChars(perLine: number): number {
  return 11 + perLine * 4
}

/**
 * 给定可用宽度（字符数），算每行放几个字节。
 *
 * <b>向下取到 2 的倍数。</b>这个粒度改过两轮，理由都是同一条线：
 *   8  最早取 8，是为了「能心算第几列是第几字节」；
 *      加了列号表头之后这个需求消失了 —— 表头逐列标了号。
 *   4  再按 dword 分组，但 4 的粒度仍会剩下几十像素。
 *   2  现在按 word 分组，最多只浪费 1 个字节的宽度（约 26px）。
 *
 * <b>不取到 1</b>：保持偶数，十六进制通常按 2 字节一组读（word 边界），
 * 而且奇数长度会让 ASCII 那栏在行与行之间错半格。
 *
 * 最少 8 —— 再窄就横向滚动，总比把一个字节拆两行强。
 */
export function perLineFor(availChars: number): number {
  const raw = Math.floor((availChars - 11) / 4)
  return Math.max(8, Math.floor(raw / 2) * 2)
}

/**
 * 列号表头，与 dump 的每一行逐字符对齐。
 *
 * 前 10 个空格让出「偏移」那一栏（8 位偏移 + 2 空格），
 * 后面每字节一个两位列号 + 空格，正好压在下面对应的那一列上。
 * 列号超过 0xFF 不可能出现 —— 每行最多 64 字节。
 */
export function headerLine(perLine: number): string {
  const n = perLine > 0 ? perLine : DEFAULT_PER_LINE
  let hex = ''

  for (let i = 0; i < n; i++) {
    hex += i.toString(16).padStart(2, '0').toUpperCase() + ' '
  }

  return ' '.repeat(10) + hex.padEnd(n * 3, ' ') + ' ' + 'ASCII'
}

/** base64 → 十六进制 + ASCII。 */
export function dump(b64: string | null, perLine: number = DEFAULT_PER_LINE): string {
  if (!b64) return ''

  const n = perLine > 0 ? perLine : DEFAULT_PER_LINE
  const bin = atob(b64)
  const len = bin.length
  const out: string[] = []

  //十六进制那段的固定宽度，最后一行不满时靠它把 ASCII 列对齐
  const hexWidth = n * 3

  for (let off = 0; off < len; off += n) {
    const end = Math.min(off + n, len)
    let hex = ''
    let asc = ''

    for (let i = off; i < end; i++) {
      const c = bin.charCodeAt(i)
      hex += c.toString(16).padStart(2, '0').toUpperCase() + ' '
      asc += c >= 0x20 && c < 0x7f ? bin[i] : '.'
    }

    out.push(
      off.toString(16).padStart(8, '0').toUpperCase() + '  ' + hex.padEnd(hexWidth, ' ') + ' ' + asc,
    )
  }

  return out.join('\n')
}

export function byteLen(b64: string | null): number {
  return b64 ? atob(b64).length : 0
}

/** dump 的一段：`d` 为真表示这一段的字节与另一份不同。 */
export interface HexSeg {
  t: string
  d: boolean
  /** 这一段是行首的偏移（行号）—— 面板把它画成与列号同一种灰，不与字节合并 */
  o?: boolean
}

/**
 * 与 dump 排版完全一致，但把结果切成若干段，标出<b>与 other 不同</b>的字节。
 *
 * 【为什么不是逐字节一个 span】改写后的包常常是整段连续不同（换包动作会替换整个包），
 * 逐字节切会给 4KB 的包造出八千个节点。这里把相邻同状态的字符<b>合并成一段</b>，
 * 于是「整包都变了」只出两三段，「改了三个字节」也只出几段 —— DOM 规模跟
 * <b>差异块的个数</b>成正比，而不是跟包长成正比。
 *
 * 【长度不等也算差异】换包会改变包长，长出来的那一截在另一份里根本不存在，
 * 按「不同」标出来才说得通。
 *
 * other 为 null 时整份都标成相同 —— 没有可比的对象，不是「全都变了」。
 */
export function dumpDiff(
  b64: string | null,
  other: string | null,
  perLine: number = DEFAULT_PER_LINE,
): HexSeg[] {
  if (!b64) return []

  const n = perLine > 0 ? perLine : DEFAULT_PER_LINE
  const bin = atob(b64)
  const ref = other ? atob(other) : null
  const len = bin.length
  const hexWidth = n * 3

  const segs: HexSeg[] = []

  //合并相邻同状态的字符，段数才不会随包长膨胀
  function push(t: string, d: boolean, o = false): void {
    if (!t) return

    const last = segs[segs.length - 1]
    //行号自成一段：它要用另一种颜色画，不能并进前后的字节里
    if (last && last.d === d && !o && !last.o) { last.t += t; return }

    segs.push(o ? { t, d, o: true } : { t, d })
  }

  const differs = (i: number): boolean => {
    if (!ref) return false
    return i >= ref.length || ref.charCodeAt(i) !== bin.charCodeAt(i)
  }

  for (let off = 0; off < len; off += n) {
    const end = Math.min(off + n, len)

    push(off.toString(16).padStart(8, '0').toUpperCase() + '  ', false, true)

    let width = 0

    for (let i = off; i < end; i++) {
      push(bin.charCodeAt(i).toString(16).padStart(2, '0').toUpperCase() + ' ', differs(i))
      width += 3
    }

    //最后一行不满时补齐，ASCII 栏才对得上
    push(' '.repeat(hexWidth - width) + ' ', false)

    for (let i = off; i < end; i++) {
      const c = bin.charCodeAt(i)
      push(c >= 0x20 && c < 0x7f ? bin[i] : '.', differs(i))
    }

    if (end < len) push('\n', false)
  }

  return segs
}
