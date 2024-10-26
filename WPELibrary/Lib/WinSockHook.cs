using System;
using System.Windows.Forms;
using EasyHook;
using System.Reflection;
using System.Net.Sockets;

namespace WPELibrary.Lib
{
    public class WinSockHook : IEntryPoint
    {        
        private LocalHook lhSend, lhSendTo, lhRecv, lhRecvFrom, lhWSASend, lhWSASendTo, lhWSARecv, lhWSARecvFrom;

        #region//WinSockHook

        public WinSockHook()
        { 
            //
        }

        public WinSockHook(RemoteHooking.IContext InContext, String InChannelName)
        {
            //
        }

        public unsafe void Run(RemoteHooking.IContext InContext, String InArg)
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }

                Application.EnableVisualStyles();                
                Application.SetCompatibleTextRenderingDefault(false);                
                Application.Run(new Socket_Form(InArg));                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }          
        }

        #endregion

        #region//开始拦截

        public void StartHook()
        {
            try
            {
                if (Socket_Cache.Support_WS2)
                {
                    #region//Winsock 2.0 Hook

                    if (Socket_Cache.Hook_Send)
                    {
                        lhSend = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "send"), new WS2_32.DSend(WS2_32.Send_Hook), this);
                        lhSend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.Hook_SendTo)
                    {
                        lhSendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "sendto"), new WS2_32.DSendTo(WS2_32.SendTo_Hook), this);
                        lhSendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.Hook_Recv)
                    {
                        lhRecv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recv"), new WS2_32.Drecv(WS2_32.Recv_Hook), this);
                        lhRecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.Hook_RecvFrom)
                    {
                        lhRecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recvfrom"), new WS2_32.DRecvFrom(WS2_32.RecvFrom_Hook), this);
                        lhRecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.Hook_WSASend)
                    {
                        lhWSASend = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASend"), new WS2_32.DWSASend(WS2_32.WSASend_Hook), this);
                        lhWSASend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.Hook_WSASendTo)
                    {
                        lhWSASendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASendTo"), new WS2_32.DWSASendTo(WS2_32.WSASendTo_Hook), this);
                        lhWSASendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.Hook_WSARecv)
                    {
                        lhWSARecv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecv"), new WS2_32.DWSARecv(WS2_32.WSARecv_Hook), this);
                        lhWSARecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.Hook_WSARecvFrom)
                    {
                        lhWSARecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecvFrom"), new WS2_32.DWSARecvFrom(WS2_32.WSARecvFrom_Hook), this);
                        lhWSARecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//停止拦截

        public void StopHook()
        {
            try
            {
                #region//Winsock 2.0 Stop Hook

                if (lhSend != null)
                {
                    lhSend.Dispose();
                }

                if (lhSendTo != null)
                {
                    lhSendTo.Dispose();
                }

                if (lhRecv != null)
                {
                    lhRecv.Dispose();
                }

                if (lhRecvFrom != null)
                {
                    lhRecvFrom.Dispose();
                }          

                if (lhWSASend != null)
                {
                    lhWSASend.Dispose();
                }

                if (lhWSASendTo != null)
                {
                    lhWSASendTo.Dispose();
                }

                if (lhWSARecv != null)
                {
                    lhWSARecv.Dispose();
                }

                if (lhWSARecvFrom != null)
                {
                    lhWSARecvFrom.Dispose();
                }

                #endregion
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//发送封包

        public static bool SendPacket(int socket, IntPtr buffer, int length)
        {  
            bool bReturn = false;

            try
            {
                int res = WS2_32.send(socket, buffer, length, SocketFlags.None);

                if (res > 0)
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;            
        }

        #endregion
    }
}
