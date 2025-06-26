using EasyHook;
using System;
using System.Reflection;
using WPE.Lib;
using WPE.Lib.NativeMethods;
using System.Windows.Forms;

namespace WPELibrary
{
    public class Hook : IEntryPoint
    {
        private LocalHook lhWS1_Send, lhWS1_SendTo, lhWS1_Recv, lhWS1_RecvFrom;
        private LocalHook lhWS2_Send, lhWS2_SendTo, lhWS2_Recv, lhWS2_RecvFrom;
        private LocalHook lhWSA_Send, lhWSA_SendTo, lhWSA_Recv, lhWSA_RecvFrom;
        private LocalHook lhWSA_RecvEx;

        #region//EasyHook        

        public Hook()
        {
            //
        }

        public Hook(RemoteHooking.IContext InContext, string ChannelName, Operate.SystemConfig.InjectionParameters parameters)
        {
            //
        }

        public unsafe void Run(RemoteHooking.IContext InContext, string ChannelName, Operate.SystemConfig.InjectionParameters parameters)
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                AntdUI.Config.Animation = parameters.Animation;
                AntdUI.Config.ShadowEnabled = parameters.ShadowEnabled;
                AntdUI.Config.ShowInWindow = parameters.ShowInWindow;
                AntdUI.Config.ScrollBarHide = parameters.ScrollBarHide;
                AntdUI.Config.TextRenderingHighQuality = parameters.TextRenderingHighQuality;
                AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                AntdUI.Config.IsDark = parameters.Dark;
                AntdUI.Localization.DefaultLanguage = "zh-CN";
                if (parameters.Lang.StartsWith("en"))
                {
                    AntdUI.Localization.Provider = new Localizer();
                    AntdUI.Localization.SetLanguage(parameters.Lang);
                }
                
                Application.Run(new Socket_Form());
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//开始拦截

        public void StartHook()
        {
            try
            {
                if (Operate.PacketConfig.Packet.Support_WS1)
                {
                    #region//Winsock 1.1 Start Hook

                    if (Operate.PacketConfig.Packet.HookWS1_Send)
                    {
                        lhWS1_Send = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "send"), new WSock32.DSend(WSock32.SendHook), this);
                        lhWS1_Send.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWS1_SendTo)
                    {
                        lhWS1_SendTo = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "sendto"), new WSock32.DSendTo(WSock32.SendToHook), this);
                        lhWS1_SendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWS1_Recv)
                    {
                        lhWS1_Recv = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "recv"), new WSock32.Drecv(WSock32.RecvHook), this);
                        lhWS1_Recv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWS1_RecvFrom)
                    {
                        lhWS1_RecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "recvfrom"), new WSock32.DRecvFrom(WSock32.RecvFromHook), this);
                        lhWS1_RecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    #endregion
                }

                if (Operate.PacketConfig.Packet.Support_WS2)
                {
                    #region//Winsock 2.0 Start Hook

                    if (Operate.PacketConfig.Packet.HookWS2_Send)
                    {
                        lhWS2_Send = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "send"), new WS2_32.DSend(WS2_32.SendHook), this);
                        lhWS2_Send.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWS2_SendTo)
                    {
                        lhWS2_SendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "sendto"), new WS2_32.DSendTo(WS2_32.SendToHook), this);
                        lhWS2_SendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWS2_Recv)
                    {
                        lhWS2_Recv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recv"), new WS2_32.Drecv(WS2_32.RecvHook), this);
                        lhWS2_Recv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWS2_RecvFrom)
                    {
                        lhWS2_RecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recvfrom"), new WS2_32.DRecvFrom(WS2_32.RecvFromHook), this);
                        lhWS2_RecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWSA_Send)
                    {
                        lhWSA_Send = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASend"), new WS2_32.DWSASend(WinSockHook.WSASend_Hook), this);
                        lhWSA_Send.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWSA_SendTo)
                    {
                        lhWSA_SendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASendTo"), new WS2_32.DWSASendTo(WinSockHook.WSASendTo_Hook), this);
                        lhWSA_SendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWSA_Recv)
                    {
                        lhWSA_Recv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecv"), new WS2_32.DWSARecv(WinSockHook.WSARecv_Hook), this);
                        lhWSA_Recv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Operate.PacketConfig.Packet.HookWSA_RecvFrom)
                    {
                        lhWSA_RecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecvFrom"), new WS2_32.DWSARecvFrom(WinSockHook.WSARecvFrom_Hook), this);
                        lhWSA_RecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    #endregion
                }

                if (Operate.PacketConfig.Packet.Support_MsWS)
                {
                    #region//Winsock Microsoft Start Hook

                    if (Operate.PacketConfig.Packet.HookWSA_Recv)
                    {
                        lhWSA_RecvEx = LocalHook.Create(LocalHook.GetProcAddress(Mswsock.ModuleName, "WSARecvEx"), new Mswsock.DWSARecvEx(Mswsock.WSARecvExHook), this);
                        lhWSA_RecvEx.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//停止拦截

        public void StopHook()
        {
            try
            {
                if (Operate.PacketConfig.Packet.Support_WS1)
                {
                    #region//Winsock 1.1 Stop Hook

                    if (lhWS1_Send != null)
                    {
                        lhWS1_Send.Dispose();
                    }

                    if (lhWS1_SendTo != null)
                    {
                        lhWS1_SendTo.Dispose();
                    }

                    if (lhWS1_Recv != null)
                    {
                        lhWS1_Recv.Dispose();
                    }

                    if (lhWS1_RecvFrom != null)
                    {
                        lhWS1_RecvFrom.Dispose();
                    }

                    #endregion
                }

                if (Operate.PacketConfig.Packet.Support_WS2)
                {
                    #region//Winsock 2.0 Stop Hook

                    if (lhWS2_Send != null)
                    {
                        lhWS2_Send.Dispose();
                    }

                    if (lhWS2_SendTo != null)
                    {
                        lhWS2_SendTo.Dispose();
                    }

                    if (lhWS2_Recv != null)
                    {
                        lhWS2_Recv.Dispose();
                    }

                    if (lhWS2_RecvFrom != null)
                    {
                        lhWS2_RecvFrom.Dispose();
                    }

                    if (lhWSA_Send != null)
                    {
                        lhWSA_Send.Dispose();
                    }

                    if (lhWSA_SendTo != null)
                    {
                        lhWSA_SendTo.Dispose();
                    }

                    if (lhWSA_Recv != null)
                    {
                        lhWSA_Recv.Dispose();
                    }

                    if (lhWSA_RecvFrom != null)
                    {
                        lhWSA_RecvFrom.Dispose();
                    }

                    #endregion
                }

                if (Operate.PacketConfig.Packet.Support_MsWS)
                {
                    #region//Winsock Microsoft Stop Hook

                    if (lhWSA_RecvEx != null)
                    {
                        lhWSA_RecvEx.Dispose();
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//退出

        public void ExitHook()
        {
            try
            {
                this.StopHook();
                LocalHook.Release();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion                
    }
}
