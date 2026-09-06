using System;
using System.Collections.Generic;

namespace WinsockPacketEditor
{
    /// <summary>
    /// 六种界面语言的对照表（中文除外）。
    ///
    /// 【为什么中文不在这里】中文是<b>兜底</b>：全项目每一处 <c>UI.T(key, "中文")</c>
    /// 都自带中文原文，AntdUI 在 <c>Localization.Provider</c> 为 null 时用的就是那一份。
    /// 再抄一份中文表进来，只会多出一处要同步的地方。
    /// 所以中文的做法是「不装 Provider」，其余五种各装一份表。
    ///
    /// 【回退链】当前语言 → 英文 → null（AntdUI 再回退到调用点的中文兜底）。
    /// 中间这一档是给「新加了键、译文还没跟上」那半天用的 —— 那时露出英文比露出键名强。
    ///
    /// 【为什么表是懒加载的】五份表合计 5300 多条，全建出来是几百 KB 的字符串引用。
    /// 用户一次只用一种语言，切换也只在设置里点一下，按需建一份就够；
    /// 建好之后缓存起来，切回来不用重建。
    /// </summary>
    public static class L10n
    {
        private static readonly object gate = new object();
        private static readonly Dictionary<string, Dictionary<string, string>> cache =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>这门语言有没有对照表。没有的（中文，以及认不出来的值）走中文兜底。</summary>
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

            Dictionary<string, string> table = TableOf(Culture);
            if (table != null && table.TryGetValue(Key, out v))
            {
                return v;
            }

            //回退英文：新加的键还没翻到这门语言时，英文比键名有用
            Dictionary<string, string> fallback = TableOf("en-US");
            if (fallback != null && fallback.TryGetValue(Key, out v))
            {
                return v;
            }

            return null;
        }

        /// <summary>"ja-JP" / "ja" / "JA" → "ja"。认不出来（含中文）返回 null。</summary>
        private static string Prefix(string Culture)
        {
            string s = (Culture ?? string.Empty).Trim().ToLowerInvariant();

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
                    default: return null;
                }

                cache[k] = t;
                return t;
            }
        }
    }
}
