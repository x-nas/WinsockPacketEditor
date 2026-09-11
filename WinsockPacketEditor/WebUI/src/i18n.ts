// 界面文案。七种语言：简体中文 / 繁體中文 / English / 日本語 / 한국어 / Tiếng Việt / Русский。
//
// 【这一层只是门面】对照表在 i18n/base.ts（简体与英文同键，是基准），
// 其余五种各一份 i18n/<code>.ts，都声明成 Record<Key, string> ——
// 往基准里加一个键，五份译文当场编译不过，不会出现半翻译的界面。
//
// 繁体那份（tw.ts）是<b>生成物</b>：由 base.ts 的 zh 用 opencc 的 twp 转出来，
// 与 C# 的 ClassObject/L10n/L10nTw.cs 同一套用词。改简体之后要重新生成，别手改。
//
// 两边靠 setLanguage 保持同步：用户在这里切换，C# 侧 UI.Prefs.Language 跟着变，
// 于是 UI.T（弹窗与通知的文案，来自 ClassObject/Localizer.cs）也换语言，
// 不会出现「界面日文、弹窗中文」。
//
// ⚠️ 所有调用点都 import 自 './i18n'（或 '../i18n'）—— 这个文件的导出名一个都不能改，
// 全项目 100 多处在用。

import { computed, ref } from 'vue'
import { call } from './bridge'
import { DICT, type Key } from './i18n/base'
import { cultureOf, defOf, LANGS, normalize, type Lang, type LangDef } from './i18n/langs'
import { ja } from './i18n/ja'
import { tw } from './i18n/tw'
import { ko } from './i18n/ko'
import { vi } from './i18n/vi'
import { ru } from './i18n/ru'

export type { Key, Lang, LangDef }
export { LANGS, cultureOf, defOf, normalize }

/** 当前语言。模板里直接用 t()，它依赖这个 ref，切换后整页自动重渲染。 */
export const lang = ref<Lang>('zh')

/**
 * 官网链接要不要加 en/ 前缀（App.vue 的 site()，唯一的用处）。
 *
 * <b>语义是「不是中文」</b>而不是「是英文」：官网只有中英两版，
 * 日 / 韩 / 越 / 俄的用户点进去看英文页，总好过看中文页。
 * <b>繁体走中文站</b> —— 繁体读者看简体页远比看英文页顺。
 */
export const isEn = computed(() => lang.value !== 'zh' && lang.value !== 'tw')

/** 除简体与英文之外的五份译文。那两种直接从 DICT 的 zh / en 取，不进这张表。 */
const EXTRA: Partial<Record<Lang, Record<Key, string>>> = { tw, ja, ko, vi, ru }

/**
 * 取文案。
 *
 * 键写错时返回键名本身而不是空串 —— 界面上会明晃晃地露出 "start.foo"，
 * 比静默显示空白容易发现得多。
 *
 * 【回退链】当前语言 → 英文 → 简体。五份译文都是全量的（类型上强制），
 * 所以这条回退平时用不上；它是给「新加了键、译文还没跟上」那半天兜底的，
 * 那时露出英文比露出键名强。
 *
 * <b>繁体是例外：当前 → 简体，跳过英文。</b>繁体读者读简体只是字形不同，
 * 读英文却是换了一门语言。C# 侧的 L10n.Get 也是同一条口径。
 */
export function t(key: Key): string {
  const e = DICT[key] as { zh: string; en: string } | undefined

  if (!e) {
    console.warn('[i18n] 未知文案键:', key)
    return key
  }

  const l = lang.value
  if (l === 'zh') return e.zh
  if (l === 'en') return e.en

  const table = EXTRA[l]
  if (l === 'tw') return (table && table[key]) || e.zh

  return (table && table[key]) || e.en || e.zh
}

/**
 * 同步 <html lang>。
 *
 * index.html 里写死的是 zh-CN，加了语言切换之后它就不再总是对的。
 * 这个属性影响读屏发音与字体回退的选择，改一下几乎不要钱。
 */
function applyDocumentLang(): void {
  try {
    document.documentElement.lang = cultureOf(lang.value)
  } catch {
    /* 非浏览器环境，忽略 */
  }
}

/** 用 C# 给的初值设定语言。启动时调一次，不回写 —— 值就是从那边来的。 */
export function initLang(code: string | undefined | null): void {
  lang.value = normalize(code)
  applyDocumentLang()
}

/**
 * 切到指定语言。
 *
 * 先改本地再推 C#：本地这一步是同步的，界面立刻就变；
 * 落库那一步万一失败也只是「这次没记住」，不该让界面卡在旧语言上。
 */
export async function setLang(next: Lang): Promise<void> {
  if (next === lang.value) return

  lang.value = next
  applyDocumentLang()

  try {
    await call('setLanguage', { language: cultureOf(next) })
  } catch (e) {
    console.error('[i18n] 语言未能写回 C#，本次切换不会被记住', e)
  }
}

