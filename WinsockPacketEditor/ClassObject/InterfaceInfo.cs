
namespace WinsockPacketEditor
{
    public static class InterfaceInfo
    {
        #region//代理模式接口

        public interface IProxyMode
        {
            void InitFloatButton();

            void SearchProxyList(bool FromHead);

            void RefreshProxyData();

            void RefreshFilterList();

            void RefreshAccountList();

            void RefreshSendList();

            void RefreshRobotList();
        }

        #endregion

        #region//注入模式接口

        public interface IInjectMode
        {
            void InitFloatButton();

            void SearchPacketList(bool FromHead);

            void RefreshPacketData();

            void RefreshFilterList();

            void RefreshSendList();

            void RefreshRobotList();
        }

        #endregion
    }
}
