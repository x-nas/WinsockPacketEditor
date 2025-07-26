
namespace WinsockPacketEditor
{
    public static class InterfaceInfo
    {
        #region//代理模式接口

        public interface IProxyMode
        {
            void InitFloatButton();

            void SearchProxyList(bool FromHead);

            void RefreshAccountList();
        }

        #endregion

        #region//注入模式接口

        public interface IInjectMode
        {
            void InitFloatButton();

            void SearchPacketList(bool FromHead);
        }

        #endregion
    }
}
