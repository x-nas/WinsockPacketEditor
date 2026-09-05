namespace WinsockPacketEditor
{
    #region//文案查询

    /// <summary>
    /// 文案查询。用来收口 Operate 里 332 处 AntdUI.Localization.Get。
    ///
    /// Fallback 是中文默认值。未注入实现时（例如注入模式在建窗之前就会读配置），
    /// UI.T 会直接返回 Fallback，不会抛异常、也不会拿到空串。
    /// </summary>
    public interface IL10n
    {
        string Get(string Key, string Fallback);
    }

    #endregion
}
