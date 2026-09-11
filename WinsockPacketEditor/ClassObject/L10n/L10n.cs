using System;
using System.Collections.Generic;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 界面语言的对照表（简体中文除外）。
    ///
    /// 【为什么简体不在这里】简体是<b>兜底</b>：全项目每一处 <c>UI.T(key, "中文")</c>
    /// 都自带中文原文，AntdUI 在 <c>Localization.Provider</c> 为 null 时用的就是那一份。
    /// 再抄一份简体表进来，只会多出一处要同步的地方。
    /// 所以简体的做法是「不装 Provider」，其余六种各装一份表。
    ///
    /// 【回退链分两种】
    ///   · 一般语言：当前 → 英文 → null（AntdUI 再回退到调用点的简体兜底）。
    ///     中间这一档是给「新加了键、译文还没跟上」那半天用的，那时露出英文比露出键名强。
    ///   · <b>繁体中文：当前 → null，跳过英文。</b>缺键时落到调用点的简体兜底 ——
    ///     对繁体读者来说简体比英文近得多，字形差异远小于换一门语言。
    ///
    /// 【为什么表是懒加载的】六份表合计 6300 多条，全建出来是几百 KB 的字符串引用。
    /// 用户一次只用一种语言，切换也只在设置里点一下，按需建一份就够；
    /// 建好之后缓存起来，切回来不用重建。
    /// </summary>
    public static class L10n
    {
        private static readonly object gate = new object();
        private static readonly Dictionary<string, Dictionary<string, string>> cache =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>这门语言有没有对照表。没有的（简体，以及认不出来的值）走简体兜底。</summary>
        public static bool Has(string Culture)
        {
            return Prefix(Culture) != null;
        }

        /// <summary>
        /// 取一条译文。取不到返回 null —— 调用方（Localizer）据此回退。
        /// </summary>
        public static string Get(string Culture, string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return null;
            }

            string v;
            string k = Prefix(Culture);

            Dictionary<string, string> table = TableOf(Culture);
            if (table != null && table.TryGetValue(Key, out v))
            {
                return v;
            }

            /*
                繁体<b>不</b>回退英文。缺一条键时让它落到调用点的简体兜底：
                繁体读者读简体只是字形不同，读英文却是换了一门语言。
                （其余语言仍走英文这一档，理由见类注释。）
            */
            if (k == "tw")
            {
                return null;
            }

            //回退英文：新加的键还没翻到这门语言时，英文比键名有用
            Dictionary<string, string> fallback = TableOf("en-US");
            if (fallback != null && fallback.TryGetValue(Key, out v))
            {
                return v;
            }

            return null;
        }

        /// <summary>"ja-JP" / "ja" / "JA" → "ja"。认不出来（含简体中文）返回 null。</summary>
        private static string Prefix(string Culture)
        {
            string s = (Culture ?? string.Empty).Trim().ToLowerInvariant();

            /*
                ⚠️ 繁体要在「zh 开头」之前判，而且不能只看前两位 ——
                zh-TW / zh-HK / zh-MO / zh-Hant 都是繁体，zh-CN / zh-Hans / zh 是简体（走兜底，返回 null）。
                写成 StartsWith("zh") 一刀切的话，繁体会被当成简体，整张表白建。
            */
            if (s.StartsWith("zh"))
            {
                if (s.Contains("tw") || s.Contains("hk") || s.Contains("mo") || s.Contains("hant"))
                {
                    return "tw";
                }

                return null;
            }

            if (s.StartsWith("en")) { return "en"; }
            if (s.StartsWith("ja")) { return "ja"; }
            if (s.StartsWith("ko")) { return "ko"; }
            if (s.StartsWith("vi")) { return "vi"; }
            if (s.StartsWith("ru")) { return "ru"; }

            return null;
        }

        private static Dictionary<string, string> TableOf(string Culture)
        {
            string k = Prefix(Culture);
            if (k == null) { return null; }

            lock (gate)
            {
                Dictionary<string, string> t;
                if (cache.TryGetValue(k, out t)) { return t; }

                switch (k)
                {
                    case "en": t = L10nEn.Build(); break;
                    case "ja": t = L10nJa.Build(); break;
                    case "ko": t = L10nKo.Build(); break;
                    case "vi": t = L10nVi.Build(); break;
                    case "ru": t = L10nRu.Build(); break;
                    case "tw": t = L10nTw.Build(); break;
                    default: return null;
                }

                cache[k] = t;
                return t;
            }
        }
    }

    /// <summary>
    /// <see cref="IL10n"/> 的实现：按 <c>UI.Prefs.Language</c> 直接查上面的对照表。
    ///
    /// 【它取代了什么】原先外壳注入的是 <c>AntdL10n</c>，那是转发给
    /// <c>AntdUI.Localization</c> 的一层，还要靠 <c>ApplyLanguage()</c> 先把 Provider 装上。
    /// WinForms 界面删掉之后，AntdUI 在这个进程里已经没有任何界面可管，
    /// 为了查一张表把它整个带着走不划算 —— 表本来就在这里。
    ///
    /// 【每次都现读语言】不在构造时记下语言：切语言只是改 <c>UI.Prefs.Language</c>，
    /// 不必再有一步「应用」，也就不会出现「改了设置、文案还是旧的」。
    /// 读一个字段 + 一次字典查找，比 AntdUI 那条路还短。
    ///
    /// 【回退与原来逐条相同】简体与认不出来的语言 → 返回调用点的中文兜底；
    /// 其余语言缺键时由 <see cref="L10n.Get"/> 先回退英文（繁体除外），再回退中文兜底。
    /// </summary>
    public sealed class CoreL10n : IL10n
    {
        public string Get(string Key, string Fallback)
        {
            string lang = UI.Prefs == null ? null : UI.Prefs.Language;

            if (!L10n.Has(lang))
            {
                return Fallback;
            }

            string s = L10n.Get(lang, Key);
            return string.IsNullOrEmpty(s) ? Fallback : s;
        }
    }
}
