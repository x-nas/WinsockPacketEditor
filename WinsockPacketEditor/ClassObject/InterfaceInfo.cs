
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

            void CleanUp_LogList();

            void SetTextA(string TextA);

            void SetTextB(string TextB);
        }

        #endregion        

        public interface IAccountList
        {
            void RefreshAccountList();
        }

        public interface IFilterList
        {
            void RefreshFilterList();
        }

        public interface ISendList
        {
            void RefreshSendList();
        }

        public interface IRobotList
        {
            void RefreshRobotList();
        }
    }
}
