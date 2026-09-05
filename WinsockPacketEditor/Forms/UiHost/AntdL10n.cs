namespace WinsockPacketEditor
{
    #region//WinForms 侧的文案实现

    /// <summary>
    /// IL10n 的 WinForms 实现：直接转发给 AntdUI.Localization。
    ///
    /// 文案数据本身仍在 ClassObject/Localizer.cs（1038 个 case），
    /// 通过 AntdUI.Localization.Provider 注册，本类不重复维护任何文案。
    ///
    /// 将来的 WebView2 桥用另一个实现（把同一份 Localizer 导出成 JSON 给前端）。
    /// </summary>
    public sealed class AntdL10n : IL10n
    {
        public string Get(string Key, string Fallback)
        {
            return AntdUI.Localization.Get(Key, Fallback);
        }
    }

    #endregion
}
