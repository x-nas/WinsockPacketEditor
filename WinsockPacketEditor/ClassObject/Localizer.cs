namespace WinsockPacketEditor
{
    /// <summary>
    /// AntdUI 的文案提供器。<c>WinFormsUiHost.ApplyLanguage</c> 按当前语言装一个。
    ///
    /// 【原先是什么样】这个文件曾是一个 1063 分支的 <c>switch</c>，只出英文 ——
    /// 那时界面只有中英两种，「英文就装它、中文就把 Provider 置 null」两句话说得完。
    /// 加到六种语言之后，一个 switch 变五个 switch 显然不行，改成按语言查表：
    /// 数据在 <c>ClassObject/L10n/L10n{En,Ja,Ko,Vi,Ru}.cs</c>（生成物），
    /// 回退逻辑在 <c>L10n.Get</c>。
    ///
    /// 【中文没有表】每一处 <c>UI.T(key, "中文")</c> 都自带中文原文，
    /// AntdUI 在 Provider 为 null 时用的就是那一份 —— 所以中文的做法是不装 Provider。
    /// </summary>
    public class Localizer : AntdUI.ILocalization
    {
        private readonly string culture;

        /// <param name="Culture">"en-US" / "ja-JP" / … 认不出来的一律走英文表。</param>
        public Localizer(string Culture)
        {
            this.culture = string.IsNullOrEmpty(Culture) ? "en-US" : Culture;
        }

        /// <summary>
        /// 取一条文案。返回 null 时 AntdUI 会用调用点给的兜底（也就是中文原文）。
        /// </summary>
        public string GetLocalizedString(string key)
        {
            return L10n.Get(this.culture, key);
        }
    }
}
