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
        private LocalHook lhSend, lhSendTo, lhRecv, lhRecvFrom, lhWSASend, lhWSASendTo, lhWSARecv, lhWSARecvFrom;

        #region//WinSockHook

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

        #region//Send Hook

        public static unsafe Int32 Send_Hook(
            [In] Int32 Socket,
            [In] IntPtr buffer,
            [In] Int32 length,
            [In] ref SocketFlags flags)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.Send;
            Int32 res = 0;

            try
            {
                if (length > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetBytesFromIntPtr(buffer, length);
                    byte[] bBuffer = Socket_Operation.GetBytesFromIntPtr(buffer, length);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, Socket, bBuffer);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        Marshal.Copy(bBuffer, 0, buffer, length);
                        res = WS2_32.send(Socket, buffer, length, ref flags);

                        if (res > 0)
                        {
                            if (res != length)
                            {
                                bBuffer = Socket_Operation.GetBytesFromIntPtr(buffer, res);
                            }
                        }
                    }

                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, res, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//Recv Hook

        public static unsafe Int32 Recv_Hook(
            [In] Int32 Socket,
            [Out] IntPtr buffer,
            [In] Int32 length,
            [In] ref SocketFlags flags)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.Recv;
            int res = WS2_32.recv(Socket, buffer, length, ref flags);

            try
            {
                if (res > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetBytesFromIntPtr(buffer, res);                    
                    byte[] bBuffer = Socket_Operation.GetBytesFromIntPtr(buffer, res);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, Socket, bBuffer);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;                        
                    }
                    else
                    {
                        Marshal.Copy(bBuffer, 0, buffer, res);                        
                    }

                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, res, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion        

        #region//SendTo Hook

        public static unsafe Int32 SendTo_Hook(
            [In] Int32 Socket,
            [In] IntPtr ipBuffer,
            [In] Int32 length,
            [In] ref SocketFlags lpFlags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] Int32 ToLen)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.SendTo;
            Int32 res = 0;            

            try
            {
                if (length > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetBytesFromIntPtr(ipBuffer, length);
                    byte[] bBuffer = Socket_Operation.GetBytesFromIntPtr(ipBuffer, length);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, Socket, bBuffer);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        Marshal.Copy(bBuffer, 0, ipBuffer, length);
                        res = WS2_32.sendto(Socket, ipBuffer, length, ref lpFlags, ref To, ToLen);

                        if (res > 0)
                        {
                            if (res != length)
                            {
                                bBuffer = Socket_Operation.GetBytesFromIntPtr(ipBuffer, res);
                            }
                        }
                    }

                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, res, ptType, FilterAction, To);
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//RecvFrom Hook

        public static unsafe Int32 RecvFrom_Hook(
            [In] Int32 Socket,
            [Out] IntPtr ipBuffer,
            [In] Int32 length,
            [In] ref SocketFlags lpFlags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr From,
            [In, Out, Optional] IntPtr FromLen)
        {            
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.RecvFrom;
            Int32 res = WS2_32.recvfrom(Socket, ipBuffer, length, ref lpFlags, ref From, FromLen);

            try
            {
                if (res > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetBytesFromIntPtr(ipBuffer, res);
                    byte[] bBuffer = Socket_Operation.GetBytesFromIntPtr(ipBuffer, res);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, Socket, bBuffer);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        Marshal.Copy(bBuffer, 0, ipBuffer, res);
                    }

                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, res, ptType, FilterAction, From);
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSASend Hook

        public static unsafe SocketError WSASend_Hook(
            [In] Int32 Socket,
            [In] ref Socket_Cache.SocketPacket.WSABUF lpBuffers,
            [In] Int32 dwBufferCount,
            [Out] IntPtr lpNumberOfBytesSend,
            [In] ref SocketFlags lpFlags,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSASend;
            SocketError res = SocketError.SocketError;

            try
            {
                int BytesSend = lpBuffers.len;

                if (BytesSend > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSend);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, lpBuffers, dwBufferCount, BytesSend);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        Marshal.WriteInt32(lpNumberOfBytesSend, 0);
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesSend, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                    }
                    else
                    {
                        res = WS2_32.WSASend(Socket, ref lpBuffers, dwBufferCount, lpNumberOfBytesSend, ref lpFlags, ref lpOverlapped, lpCompletionRoutine);

                        if (res == SocketError.Success)
                        {
                            BytesSend = Marshal.ReadInt32(lpNumberOfBytesSend);

                            if (BytesSend > 0)
                            {
                                byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSend);
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

        #region//WSARecv Hook

        public static unsafe SocketError WSARecv_Hook(
            [In] Int32 Socket,
            [In, Out] ref Socket_Cache.SocketPacket.WSABUF lpBuffers,
            [In] Int32 dwBufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags lpFlags,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSARecv;
            SocketError res = WS2_32.WSARecv(Socket, ref lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref lpFlags, ref lpOverlapped, lpCompletionRoutine);

            try
            {
                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);                    

                    if (BytesRecvd > 0)
                    {
                        byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                        Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, lpBuffers, dwBufferCount, BytesRecvd);

                        if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                        {
                            Marshal.WriteInt32(lpNumberOfBytesRecvd, 0);
                            Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesRecvd, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr());
                        }
                        else
                        {
                            byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
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

        #region//WSASendTo Hook        

        public static unsafe SocketError WSASendTo_Hook(
            [In] Int32 Socket,
            [In] ref Socket_Cache.SocketPacket.WSABUF lpBuffers,
            [In] Int32 dwBufferCount,
            [Out] IntPtr lpNumberOfBytesSend,
            [In] ref SocketFlags lpFlags,
            [In] ref Socket_Cache.SocketPacket.SockAddr lpTo,
            [In] IntPtr lpToLen,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSASendTo;
            SocketError res = SocketError.Success;            

            try
            {
                int BytesSend = lpBuffers.len;

                if (BytesSend > 0)
                {
                    byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSend);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, lpBuffers, dwBufferCount, BytesSend);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        Marshal.WriteInt32(lpNumberOfBytesSend, 0);
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesSend, ptType, FilterAction, lpTo);
                    }
                    else
                    {
                        res = WS2_32.WSASendTo(Socket, ref lpBuffers, dwBufferCount, lpNumberOfBytesSend, ref lpFlags, ref lpTo, lpToLen, ref lpOverlapped, lpCompletionRoutine);

                        if (res == SocketError.Success)
                        {
                            BytesSend = Marshal.ReadInt32(lpNumberOfBytesSend);

                            if (BytesSend > 0)
                            {
                                byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSend);
                                Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesSend, ptType, FilterAction, lpTo);
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

        #region//WSARecvFrom Hook

        public static unsafe SocketError WSARecvFrom_Hook(
            [In] Int32 Socket,
            [In, Out] ref Socket_Cache.SocketPacket.WSABUF lpBuffers,
            [In] Int32 dwBufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags lpFlags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr lpFrom,
            [In, Out] IntPtr lpFromlen,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSARecvFrom;
            SocketError res = WS2_32.WSARecvFrom(Socket, ref lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref lpFlags, ref lpFrom, lpFromlen, ref lpOverlapped, lpCompletionRoutine);
            
            try
            {
                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

                    if (BytesRecvd > 0)
                    {
                        byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                        Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, lpBuffers, dwBufferCount, BytesRecvd);

                        if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                        {
                            Marshal.WriteInt32(lpNumberOfBytesRecvd, 0);
                            Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesRecvd, ptType, FilterAction, lpFrom);
                        }
                        else
                        {
                            byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                            Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesRecvd, ptType, FilterAction, lpFrom);
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

        #endregion

        #region//开始拦截

        public void StartHook()
        {
            try
            {
                if (Socket_Cache.SocketPacket.Support_WS2)
                {
                    #region//Winsock 2.0 Hook

                    if (Socket_Cache.SocketPacket.HookSend)
                    {
                        lhSend = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "send"), new WS2_32.DSend(Send_Hook), this);
                        lhSend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookSendTo)
                    {
                        lhSendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "sendto"), new WS2_32.DSendTo(SendTo_Hook), this);
                        lhSendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookRecv)
                    {
                        lhRecv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recv"), new WS2_32.Drecv(Recv_Hook), this);
                        lhRecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookRecvFrom)
                    {
                        lhRecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recvfrom"), new WS2_32.DRecvFrom(RecvFrom_Hook), this);
                        lhRecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSASend)
                    {
                        lhWSASend = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASend"), new WS2_32.DWSASend(WSASend_Hook), this);
                        lhWSASend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSASendTo)
                    {
                        lhWSASendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASendTo"), new WS2_32.DWSASendTo(WSASendTo_Hook), this);
                        lhWSASendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSARecv)
                    {
                        lhWSARecv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecv"), new WS2_32.DWSARecv(WSARecv_Hook), this);
                        lhWSARecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSARecvFrom)
                    {
                        lhWSARecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecvFrom"), new WS2_32.DWSARecvFrom(WSARecvFrom_Hook), this);
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
                SocketFlags flags = SocketFlags.None;
                int res = WS2_32.send(socket, buffer, length, ref flags);

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
