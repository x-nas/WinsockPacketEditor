using System;
using System.Windows.Forms;
using EasyHook;
using System.Reflection;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using WPELibrary.Lib.NativeMethods;

namespace WPELibrary.Lib
{
    public class WinSockHook : IEntryPoint
    {
        private LocalHook lhWS1_Send, lhWS1_SendTo, lhWS1_Recv, lhWS1_RecvFrom;
        private LocalHook lhWS2_Send, lhWS2_SendTo, lhWS2_Recv, lhWS2_RecvFrom;
        private LocalHook lhWSA_Send, lhWSA_SendTo, lhWSA_Recv, lhWSA_RecvFrom;
        private LocalHook lhWSA_RecvEx;

        #region//EasyHook

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
                if (Socket_Cache.SocketPacket.Support_WS1)
                {
                    #region//Winsock 1.1 Start Hook

                    if (Socket_Cache.SocketPacket.HookWS1_Send)
                    {
                        lhWS1_Send = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "send"), new WSock32.DSend(WSock32.SendHook), this);
                        lhWS1_Send.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS1_SendTo)
                    {
                        lhWS1_SendTo = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "sendto"), new WSock32.DSendTo(WSock32.SendToHook), this);
                        lhWS1_SendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS1_Recv)
                    {
                        lhWS1_Recv = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "recv"), new WSock32.Drecv(WSock32.RecvHook), this);
                        lhWS1_Recv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS1_RecvFrom)
                    {
                        lhWS1_RecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "recvfrom"), new WSock32.DRecvFrom(WSock32.RecvFromHook), this);
                        lhWS1_RecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    #endregion
                }

                if (Socket_Cache.SocketPacket.Support_WS2)
                {
                    #region//Winsock 2.0 Start Hook

                    if (Socket_Cache.SocketPacket.HookWS2_Send)
                    {
                        lhWS2_Send = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "send"), new WS2_32.DSend(WS2_32.SendHook), this);
                        lhWS2_Send.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS2_SendTo)
                    {
                        lhWS2_SendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "sendto"), new WS2_32.DSendTo(WS2_32.SendToHook), this);
                        lhWS2_SendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS2_Recv)
                    {
                        lhWS2_Recv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recv"), new WS2_32.Drecv(WS2_32.RecvHook), this);
                        lhWS2_Recv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS2_RecvFrom)
                    {
                        lhWS2_RecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recvfrom"), new WS2_32.DRecvFrom(WS2_32.RecvFromHook), this);
                        lhWS2_RecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSA_Send)
                    {
                        lhWSA_Send = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASend"), new WS2_32.DWSASend(WSASend_Hook), this);
                        lhWSA_Send.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSA_SendTo)
                    {
                        lhWSA_SendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASendTo"), new WS2_32.DWSASendTo(WSASendTo_Hook), this);
                        lhWSA_SendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSA_Recv)
                    {
                        lhWSA_Recv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecv"), new WS2_32.DWSARecv(WSARecv_Hook), this);
                        lhWSA_Recv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSA_RecvFrom)
                    {
                        lhWSA_RecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecvFrom"), new WS2_32.DWSARecvFrom(WSARecvFrom_Hook), this);
                        lhWSA_RecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    #endregion
                }

                if (Socket_Cache.SocketPacket.Support_MsWS)
                {
                    #region//Winsock Microsoft Start Hook

                    if (Socket_Cache.SocketPacket.HookWSA_Recv)
                    {
                        lhWSA_RecvEx = LocalHook.Create(LocalHook.GetProcAddress(Mswsock.ModuleName, "WSARecvEx"), new Mswsock.DWSARecvEx(Mswsock.WSARecvExHook), this);
                        lhWSA_RecvEx.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
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
                if (Socket_Cache.SocketPacket.Support_WS1)
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

                if (Socket_Cache.SocketPacket.Support_WS2)
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

                if (Socket_Cache.SocketPacket.Support_MsWS)
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

        #region//Send_Hook

        public static unsafe Int32 Send_Hook(
            [In] Socket_Cache.SocketPacket.PacketType ptType,
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            IntPtr lpCache = Marshal.AllocHGlobal(Length);
            Int32 res = 0;

            try
            {
                if (Length > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetBytesFromIntPtr(lpBuffer, Length);
                    byte[] bBuffer = Socket_Operation.GetBytesFromIntPtr(lpBuffer, Length);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, Socket, bBuffer);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {                        
                        Marshal.Copy(bBuffer, 0, lpCache, bBuffer.Length);

                        switch (ptType)
                        {
                            case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                                res = WSock32.send(Socket, lpCache, bBuffer.Length, Flags);
                                break;

                            case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                                res = WS2_32.send(Socket, lpCache, bBuffer.Length, Flags);
                                break;
                        }                        

                        if (res > 0 && res < Length)
                        {
                            Buffer.BlockCopy(bBuffer, 0, bBuffer, 0, res);
                        }
                    }

                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, res, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(lpCache);
            }

            return res;
        }

        #endregion

        #region//Recv_Hook

        public static unsafe Int32 Recv_Hook(
            [In] Socket_Cache.SocketPacket.PacketType ptType,
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            IntPtr lpCache = Marshal.AllocHGlobal(Length);
            Int32 res = 0;

            try
            {
                switch (ptType)
                {
                    case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                        res = WSock32.recv(Socket, lpCache, Length, Flags);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                        res = WS2_32.recv(Socket, lpCache, Length, Flags);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                        res = Mswsock.WSARecvEx(Socket, lpCache, Length, Flags);
                        break;
                }

                if (res > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetBytesFromIntPtr(lpCache, res);
                    byte[] bBuffer = Socket_Operation.GetBytesFromIntPtr(lpCache, res);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, Socket, bBuffer);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        Marshal.Copy(bBuffer, 0, lpBuffer, res);
                    }

                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, res, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(lpCache);
            }

            return res;
        }

        #endregion

        #region//SendTo_Hook

        public static unsafe Int32 SendTo_Hook(
            [In] Socket_Cache.SocketPacket.PacketType ptType,
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] Int32 ToLen)
        {
            IntPtr lpCache = Marshal.AllocHGlobal(Length);
            Int32 res = 0;

            try
            {
                if (Length > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetBytesFromIntPtr(lpBuffer, Length);
                    byte[] bBuffer = Socket_Operation.GetBytesFromIntPtr(lpBuffer, Length);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, Socket, bBuffer);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        Marshal.Copy(bBuffer, 0, lpCache, bBuffer.Length);

                        switch (ptType)
                        {
                            case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                                res = WSock32.sendto(Socket, lpCache, bBuffer.Length, Flags, ref To, ToLen);
                                break;

                            case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                                res = WS2_32.sendto(Socket, lpCache, bBuffer.Length, Flags, ref To, ToLen);
                                break;
                        }

                        if (res > 0 && res < Length)
                        {
                            Buffer.BlockCopy(bBuffer, 0, bBuffer, 0, res);
                        }
                    }

                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, res, ptType, FilterAction, To);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(lpCache);
            }

            return res;
        }

        #endregion

        #region//RecvFrom_Hook

        public static unsafe Int32 RecvFrom_Hook(
            [In] Socket_Cache.SocketPacket.PacketType ptType,
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr From,
            [In, Out, Optional] IntPtr FromLen)
        {
            IntPtr lpCache = Marshal.AllocHGlobal(Length);
            Int32 res = 0;

            try
            {
                switch (ptType)
                {
                    case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                        res = WSock32.recvfrom(Socket, lpCache, Length, Flags, ref From, FromLen);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                        res = WS2_32.recvfrom(Socket, lpCache, Length, Flags, ref From, FromLen);
                        break;
                }

                if (res > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetBytesFromIntPtr(lpCache, res);
                    byte[] bBuffer = Socket_Operation.GetBytesFromIntPtr(lpCache, res);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, Socket, bBuffer);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        Marshal.Copy(bBuffer, 0, lpBuffer, res);
                    }

                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, res, ptType, FilterAction, From);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(lpCache);
            }

            return res;
        }

        #endregion

        #region//WSASend_Hook

        public static unsafe SocketError WSASend_Hook(
            [In] Int32 Socket,
            [In] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesSend,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSASend;
            SocketError res = SocketError.SocketError;

            try
            {
                int BytesSend = WSABuffer.len;

                if (BytesSend > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(WSABuffer, BufferCount, BytesSend);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, WSABuffer, BufferCount, BytesSend);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        Marshal.WriteInt32(lpNumberOfBytesSend, 0);
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesSend, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                    }
                    else
                    {
                        res = WS2_32.WSASend(Socket, ref WSABuffer, BufferCount, lpNumberOfBytesSend, Flags, ref Overlapped, lpCompletionRoutine);

                        if (res == SocketError.Success)
                        {
                            BytesSend = Marshal.ReadInt32(lpNumberOfBytesSend);

                            if (BytesSend > 0)
                            {
                                byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(WSABuffer, BufferCount, BytesSend);
                                Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesSend, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSARecv_Hook

        public static unsafe SocketError WSARecv_Hook(
            [In] Int32 Socket,
            [In, Out] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSARecv;
            SocketError res = WS2_32.WSARecv(Socket, ref WSABuffer, BufferCount, lpNumberOfBytesRecvd, ref Flags, ref Overlapped, lpCompletionRoutine);

            try
            {
                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

                    if (BytesRecvd > 0)
                    {
                        byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(WSABuffer, BufferCount, BytesRecvd);
                        Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, WSABuffer, BufferCount, BytesRecvd);

                        if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                        {
                            Marshal.WriteInt32(lpNumberOfBytesRecvd, 0);
                            Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesRecvd, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                        }
                        else
                        {
                            byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(WSABuffer, BufferCount, BytesRecvd);
                            Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesRecvd, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSASendTo_Hook  

        public static unsafe SocketError WSASendTo_Hook(
            [In] Int32 Socket,
            [In] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesSend,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] IntPtr lpToLen,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSASendTo;
            SocketError res = SocketError.Success;

            try
            {
                int BytesSend = WSABuffer.len;

                if (BytesSend > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(WSABuffer, BufferCount, BytesSend);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, WSABuffer, BufferCount, BytesSend);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        Marshal.WriteInt32(lpNumberOfBytesSend, 0);
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesSend, ptType, FilterAction, To);
                    }
                    else
                    {
                        res = WS2_32.WSASendTo(Socket, ref WSABuffer, BufferCount, lpNumberOfBytesSend, Flags, ref To, lpToLen, ref Overlapped, lpCompletionRoutine);

                        if (res == SocketError.Success)
                        {
                            BytesSend = Marshal.ReadInt32(lpNumberOfBytesSend);

                            if (BytesSend > 0)
                            {
                                byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(WSABuffer, BufferCount, BytesSend);
                                Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesSend, ptType, FilterAction, To);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSARecvFrom_Hook

        public static unsafe SocketError WSARecvFrom_Hook(
            [In] Int32 Socket,
            [In, Out] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags Flags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr From,
            [In, Out] IntPtr lpFromlen,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSARecvFrom;
            SocketError res = WS2_32.WSARecvFrom(Socket, ref WSABuffer, BufferCount, lpNumberOfBytesRecvd, ref Flags, ref From, lpFromlen, ref Overlapped, lpCompletionRoutine);

            try
            {
                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

                    if (BytesRecvd > 0)
                    {
                        byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(WSABuffer, BufferCount, BytesRecvd);
                        Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, WSABuffer, BufferCount, BytesRecvd);

                        if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                        {
                            Marshal.WriteInt32(lpNumberOfBytesRecvd, 0);
                            Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesRecvd, ptType, FilterAction, From);
                        }
                        else
                        {
                            byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(WSABuffer, BufferCount, BytesRecvd);
                            Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesRecvd, ptType, FilterAction, From);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion
    }
}
