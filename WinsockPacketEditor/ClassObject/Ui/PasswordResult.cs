namespace WinsockPacketEditor
{
    #region//密码框结果

    /// <summary>
    /// 加密密码输入框的结果（B6 批次，硬骨头 2）。
    ///
    /// 用户取消时 IUiHost.PromptAsync 返回 null；返回非 null 就一定带着非空密码
    /// ——「密码留空则不关闭弹窗、提示后继续输入」这个循环由宿主实现负责，
    /// 业务侧只看到「拿到密码」或「用户放弃」两种结果。
    ///
    /// 对应的 FormId：
    ///   "encrypt-export" —— 导出时设置密码
    ///   "encrypt-import" —— 导入时输入密码
    /// </summary>
    /// <summary>
    /// 密码框的入参。
    ///
    /// 原先只传一个标题字符串。加上 <see cref="FilePath"/> 是为了让<b>导入</b>那个框
    /// 能在关窗之前自己验一次密码：密码不对就留在框里重试，而不是把整条导入流程
    /// 打回到「重新点导入 → 重新选文件」。宿主实现拿它去调解密试一次即可
    /// （见 Operate.SystemConfig.DecryptXMLFile，密码不对返回 null）。
    ///
    /// 导出时没有文件可验，FilePath 为空。
    /// </summary>
    public sealed class PasswordAsk
    {
        public string Title;
        public string FilePath;

        public PasswordAsk(string Title, string FilePath)
        {
            this.Title = Title;
            this.FilePath = FilePath;
        }
    }

    public sealed class PasswordResult
    {
        public string Password;

        public PasswordResult(string Password)
        {
            this.Password = Password;
        }
    }

    #endregion
}
