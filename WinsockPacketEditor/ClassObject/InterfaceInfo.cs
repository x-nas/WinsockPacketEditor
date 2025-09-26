
namespace WinsockPacketEditor
{
    public static class InterfaceInfo
    {
        #region//代理模式接口

        public interface IProxyMode
        {
            void SetColumnVisible_ProxyList();

            void InitFloatButton();

            void SearchProxyList(bool FromHead);

            void RefreshProxyData();

            void RefreshAccountList();

            void RefreshFilterList();

            void RefreshSendList();

            void RefreshRobotList();

            void CleanUp_LogList();

            void SetTextA(string TextA);

            void SetTextB(string TextB);
        }

        #endregion

        #region//注入模式接口

        public interface IInjectMode
        {
            void SetColumnVisible_PacketList();

            void InitFloatButton();

            void SearchPacketList(bool FromHead);

            void RefreshPacketData();

            void RefreshFilterList();

            void RefreshSendList();

            void RefreshRobotList();

            void CleanUp_LogList();

            void SetTextA(string TextA);

            void SetTextB(string TextB);
        }

        #endregion                
    }
}
