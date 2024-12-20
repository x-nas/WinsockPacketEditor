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

        public static unsafe Int32 Send_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.Send;
            Int32 res = 0;

            try
            {
                byte[] bRawBuffer = new byte[length];
                Marshal.Copy(buffer, bRawBuffer, 0, length);
                byte[] bBuffer = new byte[length];
                Buffer.BlockCopy(bRawBuffer, 0, bBuffer, 0, length);

                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, socket, bBuffer);                

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = 0;                    
                }
                else
                {
                    IntPtr ipBuffer = Marshal.AllocHGlobal(length);
                    Marshal.Copy(bBuffer, 0, ipBuffer, length);
                    res = WS2_32.send(socket, ipBuffer, length, flags);
                    Marshal.FreeHGlobal(ipBuffer);
                }

                Socket_Operation.ProcessingHookResult(socket, bRawBuffer, bBuffer, res, ptType, FilterAction, new Socket_Cache.SocketPacket.sockaddr());
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//Recv Hook

        public static unsafe Int32 Recv_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.Recv;
            Int32 res = 0;

            try
            {
                IntPtr ipBuffer = Marshal.AllocHGlobal(length);
                res = WS2_32.recv(socket, ipBuffer, length, flags);

                if (res > 0)
                {
                    byte[] bRawBuffer = new byte[res];
                    Marshal.Copy(buffer, bRawBuffer, 0, res);
                    byte[] bBuffer = new byte[res];
                    Buffer.BlockCopy(bRawBuffer, 0, bBuffer, 0, res);
                    
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, socket, bBuffer);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;                        
                    }
                    else
                    {
                        Marshal.Copy(bBuffer, 0, buffer, res);                        
                    }

                    Socket_Operation.ProcessingHookResult(socket, bRawBuffer, bBuffer, res, ptType, FilterAction, new Socket_Cache.SocketPacket.sockaddr());
                }

                Marshal.FreeHGlobal(ipBuffer);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion        

        #region//SendTo Hook

        public static unsafe Int32 SendTo_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr To, Int32 toLenth)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.SendTo;
            Int32 res = 0;

            try
            {
                byte[] bRawBuffer = new byte[length];
                Marshal.Copy(buffer, bRawBuffer, 0, length);
                byte[] bBuffer = new byte[length];
                Buffer.BlockCopy(bRawBuffer, 0, bBuffer, 0, length);
                
                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, socket, bBuffer);                

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = 0;                    
                }
                else
                {
                    IntPtr ipBuffer = Marshal.AllocHGlobal(length);
                    Marshal.Copy(bBuffer, 0, ipBuffer, length);
                    res = WS2_32.sendto(socket, ipBuffer, length, flags, To, toLenth);
                    Marshal.FreeHGlobal(ipBuffer);
                }

                Socket_Operation.ProcessingHookResult(socket, bRawBuffer, bBuffer, res, ptType, FilterAction, Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(To));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//RecvFrom Hook

        public static unsafe Int32 RecvFrom_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr from, Int32 fromLen)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.RecvFrom;
            Int32 res = 0;

            try
            {
                IntPtr ipBuffer = Marshal.AllocHGlobal(length);
                res = WS2_32.recvfrom(socket, ipBuffer, length, flags, from, fromLen);

                if (res > 0)
                {
                    byte[] bRawBuffer = new byte[res];
                    Marshal.Copy(buffer, bRawBuffer, 0, res);
                    byte[] bBuffer = new byte[res];
                    Buffer.BlockCopy(bRawBuffer, 0, bBuffer, 0, res);
             
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(ptType, socket, bBuffer);                    

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        Marshal.Copy(bBuffer, 0, buffer, res);
                    }

                    Socket_Operation.ProcessingHookResult(socket, bRawBuffer, bBuffer, res, ptType, FilterAction, Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(from));
                }

                Marshal.FreeHGlobal(ipBuffer);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSASend Hook

        public static unsafe SocketError WSASend_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSASend;
            SocketError res = SocketError.SocketError;

            try
            {
                int BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);
                byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);
                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, lpBuffers, dwBufferCount, BytesSent);

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = SocketError.Success;
                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesSent, ptType, FilterAction, new Socket_Cache.SocketPacket.sockaddr());
                }
                else
                {
                    res = WS2_32.WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, ref dwFlags, lpOverlapped, lpCompletionRoutine);

                    if (res == SocketError.Success)
                    {
                        BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);
                        byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesSent, ptType, FilterAction, new Socket_Cache.SocketPacket.sockaddr());
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

        public static unsafe SocketError WSARecv_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr overlapped, IntPtr completionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSARecv;
            SocketError res = SocketError.SocketError;

            try
            {
                res = WS2_32.WSARecv(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref flags, overlapped, completionRoutine);

                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, lpBuffers, dwBufferCount, BytesRecvd);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        Marshal.WriteInt32(lpNumberOfBytesRecvd, 0);
                        res = SocketError.Success;
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesRecvd, ptType, FilterAction, new Socket_Cache.SocketPacket.sockaddr());
                    }
                    else
                    {
                        byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesRecvd, ptType, FilterAction, new Socket_Cache.SocketPacket.sockaddr());
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

        public static unsafe SocketError WSASendTo_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr To, Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSASendTo;
            SocketError res = SocketError.Success;

            try
            {
                int BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);
                byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);
                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, lpBuffers, dwBufferCount, BytesSent);

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = SocketError.Success;
                    Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesSent, ptType, FilterAction, Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(To));
                }
                else
                {
                    res = WS2_32.WSASendTo(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, ref dwFlags, To, toLenth, lpOverlapped, lpCompletionRoutine);

                    if (res == SocketError.Success)
                    {
                        BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);
                        byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesSent, ptType, FilterAction, Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(To));
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

        public static unsafe SocketError WSARecvFrom_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr from, Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSARecvFrom;
            SocketError res = SocketError.SocketError;

            try
            {
                res = WS2_32.WSARecvFrom(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref flags, from, fromLen, overlapped, completionRoutine);

                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    byte[] bRawBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoWSAFilterList(ptType, Socket, lpBuffers, dwBufferCount, BytesRecvd);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        Marshal.WriteInt32(lpNumberOfBytesRecvd, 0);
                        res = SocketError.Success;
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bRawBuffer, BytesRecvd, ptType, FilterAction, Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(from));
                    }
                    else
                    {
                        byte[] bBuffer = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                        Socket_Operation.ProcessingHookResult(Socket, bRawBuffer, bBuffer, BytesRecvd, ptType, FilterAction, Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(from));
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
                if (Socket_Cache.Support_WS2)
                {
                    #region//Winsock 2.0 Hook

                    if (Socket_Cache.HookSend)
                    {
                        lhSend = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "send"), new WS2_32.DSend(Send_Hook), this);
                        lhSend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.HookSendTo)
                    {
                        lhSendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "sendto"), new WS2_32.DSendTo(SendTo_Hook), this);
                        lhSendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.HookRecv)
                    {
                        lhRecv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recv"), new WS2_32.Drecv(Recv_Hook), this);
                        lhRecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.HookRecvFrom)
                    {
                        lhRecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recvfrom"), new WS2_32.DRecvFrom(RecvFrom_Hook), this);
                        lhRecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.HookWSASend)
                    {
                        lhWSASend = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASend"), new WS2_32.DWSASend(WSASend_Hook), this);
                        lhWSASend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.HookWSASendTo)
                    {
                        lhWSASendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASendTo"), new WS2_32.DWSASendTo(WSASendTo_Hook), this);
                        lhWSASendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.HookWSARecv)
                    {
                        lhWSARecv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecv"), new WS2_32.DWSARecv(WSARecv_Hook), this);
                        lhWSARecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.HookWSARecvFrom)
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
