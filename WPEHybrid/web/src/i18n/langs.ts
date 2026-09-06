// 支持的界面语言 —— <b>全项目唯一的一份清单</b>。
//
// 前端的字典、标题栏的下拉、C# 侧的 Localizer 与 WinForms 启动页的下拉
// 讲的必须是同一批语言；分散成几处的下场是「界面切到日文、弹窗还是英文」。
// 所以语言代码 / 文化名 / 显示名都从这里出，C# 那边照抄同一张表
// （ClassObject/Localizer.cs 顶上的 Tables，键就是这里的 culture）。
//
// 【为什么是这六种】中英是原有的；日 / 韩 / 越 / 俄是按 WPE 的实际使用地补的
// —— 网游封包工具的用户集中在东亚与东南亚，俄语区是第二大来源。
// 繁体中文<b>刻意没做</b>：简繁转换要逐字过，机械替换会留下一堆「里 / 裡」
// 「发 / 髮」这类选错字的地方，比不做更糟。

export type Lang = 'zh' | 'en' | 'ja' | 'ko' | 'vi' | 'ru'

export interface LangDef {
  /** 字典里的键，也是前端内部一律用的短码 */
  code: Lang
  /** C# 侧 UI.Prefs.Language 存的值（SQLite 的 DefaultLanguage 列），也是 AntdUI 的语言名 */
  culture: string
  /** 下拉里显示的名字。<b>一律用该语言自己的写法</b> —— 切到看不懂的语言时，
   *  「English」「日本語」这样的自称是唯一还认得出来的东西 */
  label: string
  /** 标题栏 chip 上那两个字母。定宽，切换时整排窗口按钮才不会平移 */
  short: string
  /**
   * 这门语言的字比汉字宽多少。
   *
   * 侧栏那 14 个标签是按中文的字数排的（196px 正好）；
   * 换成俄语「Извлечение данных」「Системный журнал」就要靠省略号截断，
   * 一屏看下来有四五处 …，很难扫。所以侧栏宽度跟着语言走。
   *
   * 日 / 韩与中文同为方块字，宽度相当；拉丁与西里尔字母的词长得多。
   */
  wide?: boolean
}

export const LANGS: LangDef[] = [
  { code: 'zh', culture: 'zh-CN', label: '简体中文', short: 'CN' },
  { code: 'en', culture: 'en-US', label: 'English', short: 'EN', wide: true },
  { code: 'ja', culture: 'ja-JP', label: '日本語', short: 'JA' },
  { code: 'ko', culture: 'ko-KR', label: '한국어', short: 'KO' },
  { code: 'vi', culture: 'vi-VN', label: 'Tiếng Việt', short: 'VI', wide: true },
  { code: 'ru', culture: 'ru-RU', label: 'Русский', short: 'RU', wide: true },
]

/** C# 给的 "ja-JP" / "en-US" → 这里的短码。认不出来的一律回中文。 */
export function normalize(code: string | undefined | null): Lang {
  const s = (code || '').toLowerCase()

  for (const l of LANGS) {
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
  return LANGS.find((x) => x.code === code) || LANGS[0]
}
