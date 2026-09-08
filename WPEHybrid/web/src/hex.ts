// 十六进制渲染的共用件：每行放几个字节、列号表头、base64 → 文本。
//
// HexView 用的是前三个；dump 只剩验收跑测在用（面板早已换成 HexView 的格子模型，
// 不再走整块文本）。当初抽成独立模块，是为了让跑测量到与面板<b>相同</b>的那份工作量
// —— 那个前提现在只对「格式化本身」还成立，见 CLAUDE.md 的提醒。

/** 每行默认字节数。宽度算不出来时（首帧、面板被隐藏）退回它。 */
export const DEFAULT_PER_LINE = 16

/**
 * 给定可用宽度（字符数），算每行放几个字节。
 *
 * 一行的字符数是 <b>11 + 4N</b>：偏移(8) + 两空格 + 十六进制(每字节 "XX " 共 3)
 * + 一空格 + ASCII(每字节 1)。下面就是反过来解这个 N。
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

/*
  字节 → ASCII 栏显示成什么字符。

  ⚠️ 这一份<b>照抄 Be.Windows.Forms 的 DefaultByteCharConverter</b>（WinForms 那半边的
  十六进制控件用的就是它，没有设过自定义 converter）。反射把 256 个字节全问过一遍，
  它的规则精确到位：

      0x00..0x1F 与 0x7F..0x9F  ->  '.'
      其余                      ->  String.fromCharCode(b)   //也就是 Latin-1，字节值即码位

  换句话说<b>高半区（0xA0..0xFF）是照 Latin-1 显示的</b>，不是一律打点 ——
  ¶ ² Ç ã ¡ º ÿ 这些都看得见。两套 UI 摆在一起对同一个包，ASCII 栏必须逐格一样，
  否则「同一条封包在两个界面里长得不一样」，而这一栏正是拿来肉眼找结构的。

  ⚠️ <b>不是</b> UTF-8、也<b>不是</b> GBK：这一栏是<b>逐字节</b>的，一个字节一格、
  与左边的十六进制一一对齐。多字节编码会让「第几格 = 第几字节」当场失效。
  要看解码后的文本请用十六进制面板的「文本」页签（那边才按 UTF-8 解）。

  ⚠️ 0xA0 是不断行空格、0xAD 是软连字符，两者渲染出来都是空白 ——
  WinForms 那边也一样（GDI 画的同样是空白），这是忠实照搬，不是漏了。
*/
export function asciiOf(b: number): string {
  return b > 0x1f && !(b > 0x7e && b < 0xa0) ? String.fromCharCode(b) : '.'
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
      asc += asciiOf(c)
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
