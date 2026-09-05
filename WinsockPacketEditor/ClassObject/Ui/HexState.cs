namespace WinsockPacketEditor
{
    #region//十六进制编辑器状态

    /// <summary>
    /// 十六进制编辑器的可操作状态。
    ///
    /// 用来替换 4 个菜单工厂方法里的 Be.Windows.Forms.HexBox 参数
    /// （GetCMS_XOR / GetCMS_PacketData / GetCMS_PacketEdit / GetCMS_StoresData）。
    /// 这 4 处对 HexBox 的调用只有 CanCopy / CanCut / CanPaste / CanPasteHex 四个布尔，
    /// 改由 UI 层算好后传进来，Operate 就不再认识 HexBox。
    ///
    /// B0 阶段仅定义，B5 批次启用。
    /// </summary>
    public struct HexState
    {
        public bool CanCopy;
        public bool CanCut;
        public bool CanPaste;
        public bool CanPasteHex;
    }

    #endregion
}
