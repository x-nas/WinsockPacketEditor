// 支持的界面语言 —— <b>全项目唯一的一份清单</b>。
//
// 前端的字典、设置里的下拉、C# 侧的 Localizer 与 WinForms 启动页的下拉
// 讲的必须是同一批语言；分散成几处的下场是「界面切到日文、弹窗还是英文」。
// 所以语言代码 / 文化名 / 显示名都从这里出，C# 那边照抄同一张表
// （ShellForm.Normalize 与 Forms/StartForm.cs 的下拉，键就是这里的 culture）。
//
// 【为什么是这七种】中（简 / 繁）英是原有的加台港澳；
// 日 / 韩 / 越 / 俄是按 WPE 的实际使用地补的 —— 网游封包工具的用户集中在
// 东亚与东南亚，俄语区是第二大来源。

export type Lang = 'zh' | 'tw' | 'en' | 'ja' | 'ko' | 'vi' | 'ru'

export interface LangDef {
  /** 字典里的键，也是前端内部一律用的短码 */
  code: Lang
  /** C# 侧 UI.Prefs.Language 存的值（SQLite 的 DefaultLanguage 列），也是 AntdUI 的语言名 */
  culture: string
  /** 下拉里显示的名字。<b>一律用该语言自己的写法</b> —— 切到看不懂的语言时，
   *  「English」「日本語」这样的自称是唯一还认得出来的东西 */
  label: string
  /**
   * 两个字母的身份标记，显示在下拉每一项的名字前面。
   *
   * 原先它在标题栏的语言 chip 上（那颗按钮已经换成设置齿轮、标签也去掉了），
   * 现在只剩下拉在用，作用有两个：给一列对得齐的视觉锚点，
   * 以及让 CyberSelect 的「敲首字母跳到下一个匹配项」在 CJK 名字上也能用
   * —— 七个首字母 e/j/k/r/v/c/t 各不相同。
   *
   * ⚠️ <b>它不是排序键</b>：CN / TW 是地区码而不是语言码，
   * 按它排会把两种中文拆到列表两头。排序看 culture，见 LANGS 上面那段。
   */
  short: string
  /**
   * 这门语言的字比汉字宽多少。
   *
   * 侧栏那 14 个标签是按中文的字数排的（196px 正好）；
   * 换成俄语「Извлечение данных」「Системный журнал」就要靠省略号截断，
   * 一屏看下来有四五处 …，很难扫。所以侧栏宽度跟着语言走。
   *
   * 日 / 韩 / 繁体与简体同为方块字，宽度相当；拉丁与西里尔字母的词长得多。
   */
  wide?: boolean
}

/*
  ⚠️ <b>按 BCP-47 语言标记（culture）的字母序排</b>：
  en-US · ja-JP · ko-KR · ru-RU · vi-VN · zh-CN · zh-TW。

  这是语言选择器的通行做法，选它有三条实在的理由：

  · <b>加语言时没有争议</b>。按什么排一旦要靠"觉得"，每加一种都要重新讨论一次；
    按标记排则位置是算出来的 —— 插进它该在的地方就行。
  · <b>不以某一种语言为中心</b>。原先是简体在最前（产品的默认语言），
    那是"我的语言优先"的排法，不是国际惯例。
  · <b>同一门语言的变体天然相邻</b>。zh-CN 与 zh-TW 排在一起，
    不会被别的语言隔开 —— 按显示名或按 short 排都会把它俩拆散。

  <b>顺序不是随手排的，别按"看着顺眼"重排。</b>
  short 那一列（EN JA KO RU VI CN TW）因此不是字母序 —— 它是身份标记不是排序键，
  CN / TW 是地区码、不是语言码，拿它排会把两种中文拆开。
*/
export const LANGS: LangDef[] = [
  { code: 'en', culture: 'en-US', label: 'English', short: 'EN', wide: true },
  { code: 'ja', culture: 'ja-JP', label: '日本語', short: 'JA' },
  { code: 'ko', culture: 'ko-KR', label: '한국어', short: 'KO' },
  { code: 'ru', culture: 'ru-RU', label: 'Русский', short: 'RU', wide: true },
  { code: 'vi', culture: 'vi-VN', label: 'Tiếng Việt', short: 'VI', wide: true },
  { code: 'zh', culture: 'zh-CN', label: '简体中文', short: 'CN' },
  { code: 'tw', culture: 'zh-TW', label: '繁體中文', short: 'TW' },
]

/**
 * 认不出来时回落到哪一种。
 *
 * <b>不能写成 LANGS[0]</b> —— 那是"列表第一项"，而列表现在按语言标记排序，
 * 第一项是 English。回落必须钉在<b>简体</b>上：它是产品的默认语言，
 * 也是 base.ts 里 zh 字段的那一份（其余语言缺键时最终也落回它）。
 */
const FALLBACK: LangDef = LANGS.find((x) => x.code === 'zh')!

/**
 * C# 给的 "ja-JP" / "en-US" / "zh-TW" → 这里的短码。认不出来的一律回简体。
 *
 * ⚠️ <b>简繁必须先分开判</b>：zh-CN 与 zh-TW 前两位相同，
 * 按前缀一刀切的话繁体会被当成简体，整份译文白做。
 * zh-TW / zh-HK / zh-MO / zh-Hant 都是繁体，其余 zh 开头的是简体。
 */
export function normalize(code: string | undefined | null): Lang {
  const s = (code || '').toLowerCase()

  if (s === 'tw') return 'tw'

  if (s.startsWith('zh')) {
    return /tw|hk|mo|hant/.test(s) ? 'tw' : 'zh'
  }

  for (const l of LANGS) {
    if (l.code === 'zh' || l.code === 'tw') continue
    //只比前两位：库里可能存着 "en-GB" 这类值，没必要为此加一张别名表
    if (s.startsWith(l.code)) return l.code
  }

  return 'zh'
}

/** 短码 → C# 要的文化名。 */
export function cultureOf(code: Lang): string {
  const l = LANGS.find((x) => x.code === code)
  return l ? l.culture : 'zh-CN'
}

export function defOf(code: Lang): LangDef {
  return LANGS.find((x) => x.code === code) || FALLBACK
}
