// 十六进制渲染的共用件：每行放几个字节、列号表头、base64 → 文本。
//
// HexView 用 perLineFor / headerLine / asciiOf；整块文本版的 dump 只服务于 B10 验收跑测，已随它删除（2.1.9）。

/** 每行默认字节数。宽度算不出来时（首帧、面板被隐藏）退回它。 */
export const DEFAULT_PER_LINE = 16

/**
 * 给定可用宽度（字符数），算每行放几个字节。
 *
 * 一行的字符数是 <b>fixed + 4N</b>：偏移(8) + 分隔 + 十六进制(每字节 "XX " 共 3)
 * + 一空格 + 字符栏(每字节 1)。下面就是反过来解这个 N。
 *
 * ⚠️ <b>fixed 要按调用方的真实版式给</b>，不是常数 11：
 * 十六进制面板的偏移栏后面跟两个空格（8 + 2 + 1 = 11），
 * 而并排对比视图（DiffView）只跟一个（8 + 1 + 1 = 10）。
 * 多算的那 1 个字符是白扔的 —— 两侧各扔一个，一行就少一点可用宽度。
 *
 * <b>向下取到 2 的倍数。</b>这个粒度改过两轮，理由都是同一条线：
 *   8  最早取 8，是为了「能心算第几列是第几字节」；
 *      加了列号表头之后这个需求消失了 —— 表头逐列标了号。
 *   4  再按 dword 分组，但 4 的粒度仍会剩下几十像素。
 *   2  现在按 word 分组，最多只浪费 1 个字节的宽度（约 26px）。
 *
 * <b>不取到 1</b>：保持偶数，十六进制通常按 2 字节一组读（word 边界），
 * 而且奇数长度会让字符栏在行与行之间错半格。
 *
 * 最少 8 —— 再窄就横向滚动，总比把一个字节拆两行强。
 */
export function perLineFor(availChars: number, fixed = 11, min = 8): number {
  const raw = Math.floor((availChars - fixed) / 4)
  return Math.max(min, Math.floor(raw / 2) * 2)
}

/**
 * 列号表头，与 HexView 每一行的格子逐列对齐。
 *
 * 前 10 个空格让出「偏移」那一栏（8 位偏移 + 2 空格），
 * 后面每字节一个两位列号 + 空格，正好压在下面对应的那一列上。
 * 列号超过 0xFF 不可能出现 —— 每行最多 64 字节。
 */
export function headerLine(perLine: number, label: string): string {
  const n = perLine > 0 ? perLine : DEFAULT_PER_LINE
  let hex = ''

  for (let i = 0; i < n; i++) {
    hex += i.toString(16).padStart(2, '0').toUpperCase() + ' '
  }

  /*
    ⚠️ 标签由调用方给（<b>要翻译</b>），hex.ts 本身不引 i18n —— 它是纯格式化，
    别给它挂上界面的依赖。

    这一栏叫什么改过两轮，两次都是被用户问出来的：
      ASCII    → 不对。它按 Latin-1 显示，0xA0..0xFF 是 ¶ ² Ç ã ÿ 这些，
                 而 ASCII 只有 0x00..0x7F，标成 ASCII 是在说一件不成立的事；
      LATIN-1  → 对，但是行话。
      <b>字符</b>  → 现在这个。

    ⚠️ <b>刻意不叫「文本」</b>：这块面板右上角那组分段按钮里已经有一个「文本」，
    切过去看的是<b>按 UTF-8 解码</b>的整段文字 —— 与这一栏（逐字节、Latin-1）
    根本是两样东西。两个一模一样的名字摆在同一块面板上、相隔一行，
    谁都会以为是同一个东西（「自动清理」当年就是这么被问的）。

    宽度放得下：perLineFor 的下限是 8 个字节，而各语言里最长的
    「Символы」也只要 7 格。加语言时回来核一眼这条。
  */
  return ' '.repeat(10) + hex.padEnd(n * 3, ' ') + ' ' + label
}

/*
  字节 → 字符栏显示成什么字符。

  ⚠️ 这一栏<b>不是 ASCII</b>（表头上写的也是 LATIN-1）—— 名字里留着 ascii
  只是沿用行业叫法，规则见下面。

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

export function byteLen(b64: string | null): number {
  return b64 ? atob(b64).length : 0
}
