using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using EasyHook;
using System.Reflection;
using System.Threading;

namespace WPELibrary.Lib
{
    public class WinSockHook : IEntryPoint
    {        
        private LocalHook lhSend, lhSendTo, lhRecv, lhRecvFrom, lhWSASend, lhWSASendTo, lhWSARecv, lhWSARecvFrom;        

        #region//user32.dll 

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        #endregion

        #region//ws2_32.dll WSAGetLastError

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int WSAGetLastError();

        #endregion

        #region//ws2_32.dll Send Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 send(Int32 socket, IntPtr buffer, Int32 length, Int32 flags);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 DSend(Int32 s, IntPtr buf, Int32 len, Int32 flags);

        private static unsafe Int32 Send_Hook(Int32 socket, IntPtr buffer, Int32 length, Int32 flags)
        {
            Int32 res = 0;            

            try
            {
                Socket_Packet.SocketType stSocketType = Socket_Packet.SocketType.Send;

                Socket_Cache.SocketFilterList.DoFilter(buffer, length);
                res = send(socket, buffer, length, flags);

                if (res > 0 && length > 0)
                {
                    SocketPacketToQueue(socket, buffer, length, stSocketType, new Socket_Packet.sockaddr(), res);                    
                    Socket_Operation.DoLog_HookInfo(MethodBase.GetCurrentMethod().Name, stSocketType, socket, length, res);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }           

            return res;
        }

        #endregion

        #region//ws2_32.dll SendTo Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 sendto(Int32 socket, IntPtr buffer, Int32 length, Int32 flags, ref Socket_Packet.sockaddr To, ref Int32 toLenth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 DSendTo(Int32 socket, IntPtr buffer, Int32 length, Int32 flags, ref Socket_Packet.sockaddr To, ref Int32 toLenth);
        private static unsafe Int32 SendTo_Hook(Int32 socket, IntPtr buffer, Int32 length, Int32 flags, ref Socket_Packet.sockaddr To, ref Int32 toLenth)
        {
            Int32 res = 0;

            try
            {
                Socket_Packet.SocketType stSocketType = Socket_Packet.SocketType.SendTo;
                Socket_Cache.SocketFilterList.DoFilter(buffer, length);
                res = sendto(socket, buffer, length, flags, ref To, ref toLenth);

                if (res > 0 && length > 0)
                {
                    SocketPacketToQueue(socket, buffer, length, stSocketType, To, res);
                    Socket_Operation.DoLog_HookInfo(MethodBase.GetCurrentMethod().Name, stSocketType, socket, length, res);
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//ws2_32.dll Recv Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 recv(Int32 socket, IntPtr buffer, Int32 length, Int32 flags);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 Drecv(Int32 socket, IntPtr buffer, Int32 length, Int32 flags);

        private static unsafe Int32 Recv_Hook(Int32 socket, IntPtr buffer, Int32 length, Int32 flags)
        {
            Int32 res = 0;

            try
            {
                Socket_Packet.SocketType stSocketType = Socket_Packet.SocketType.Recv;
                Socket_Cache.SocketFilterList.DoFilter(buffer, length);
                res = recv(socket, buffer, length, flags);

                if (res > 0)
                {
                    SocketPacketToQueue(socket, buffer, length, stSocketType, new Socket_Packet.sockaddr(), res);
                    Socket_Operation.DoLog_HookInfo(MethodBase.GetCurrentMethod().Name, stSocketType, socket, length, res);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion        

        #region//ws2_32.dll RecvFrom Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 recvfrom(Int32 socket, IntPtr buffer, Int32 length, Int32 flags, ref Socket_Packet.sockaddr from, ref Int32 fromLen);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 DRecvFrom(Int32 socket, IntPtr buffer, Int32 length, Int32 flags, ref Socket_Packet.sockaddr from, ref Int32 fromLen);

        private static unsafe Int32 RecvFrom_Hook(Int32 socket, IntPtr buffer, Int32 length, Int32 flags, ref Socket_Packet.sockaddr from, ref Int32 fromLen)
        {
            Int32 res = 0;

            try
            {
                Socket_Packet.SocketType stSocketType = Socket_Packet.SocketType.RecvFrom;
                Socket_Cache.SocketFilterList.DoFilter(buffer, length);
                res = recvfrom(socket, buffer, length, flags, ref from, ref fromLen);

                if (res > 0)
                {
                    SocketPacketToQueue(socket, buffer, length, stSocketType, from, res);
                    Socket_Operation.DoLog_HookInfo(MethodBase.GetCurrentMethod().Name, stSocketType, socket, length, res);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//ws2_32.dll WSASend Hook

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 WSASend(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 DWSASend(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        private static unsafe Int32 WSASend_Hook(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            Int32 res = 0;
            int BytesSent = 0;            

            try
            {
                Socket_Packet.SocketType stSocketType = Socket_Packet.SocketType.WSASend;

                res = WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, dwFlags, lpOverlapped, lpCompletionRoutine);
                BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);
                

                if (res == 0 && BytesSent > 0)
                {
                    Socket_Packet.WSABUF wsBuffer = (Socket_Packet.WSABUF)Marshal.PtrToStructure(lpBuffers, typeof(Socket_Packet.WSABUF));
                    Socket_Cache.SocketFilterList.DoFilter(wsBuffer.buf, wsBuffer.len);

                    SocketPacketToQueue(Socket, wsBuffer.buf, wsBuffer.len, stSocketType, new Socket_Packet.sockaddr(), BytesSent);
                    Socket_Operation.DoLog_HookInfo(MethodBase.GetCurrentMethod().Name, stSocketType, Socket, wsBuffer.len, BytesSent);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//ws2_32.dll WSASendTo Hook

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 WSASendTo(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, ref Socket_Packet.sockaddr To, ref Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 DWSASendTo(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, ref Socket_Packet.sockaddr To, ref Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        private static unsafe Int32 WSASendTo_Hook(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, ref Socket_Packet.sockaddr To, ref Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            Int32 res = 0;
            int BytesSent = 0;

            try
            {
                Socket_Packet.SocketType stSocketType = Socket_Packet.SocketType.WSASendTo;

                Socket_Packet.WSABUF wsBuffer;
                wsBuffer = (Socket_Packet.WSABUF)Marshal.PtrToStructure(lpBuffers, typeof(Socket_Packet.WSABUF));

                Socket_Cache.SocketFilterList.DoFilter(wsBuffer.buf, wsBuffer.len);

                res = WSASendTo(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, dwFlags, ref To, ref toLenth, lpOverlapped, lpCompletionRoutine);
                BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

                if (res == 0 && BytesSent > 0)
                {
                    SocketPacketToQueue(Socket, wsBuffer.buf, wsBuffer.len, stSocketType, To, BytesSent);
                    Socket_Operation.DoLog_HookInfo(MethodBase.GetCurrentMethod().Name, stSocketType, Socket, wsBuffer.len, BytesSent);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//ws2_32.dll WSARecv Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 WSARecv(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, Int32 flags, IntPtr overlapped, IntPtr completionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 DWSARecv(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, Int32 flags, IntPtr overlapped, IntPtr completionRoutine);

        private static unsafe Int32 WSARecv_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, Int32 flags, IntPtr overlapped, IntPtr completionRoutine)
        {
            Int32 res = 0;
            int BytesRecvd = 0;

            try
            {
                Socket_Packet.SocketType stSocketType = Socket_Packet.SocketType.WSARecv;

                res = WSARecv(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, flags, overlapped, completionRoutine);
                BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

                if (res == 0 && BytesRecvd > 0)
                {
                    Socket_Packet.WSABUF wsBuffer;
                    wsBuffer = (Socket_Packet.WSABUF)Marshal.PtrToStructure(lpBuffers, typeof(Socket_Packet.WSABUF));
                    Socket_Cache.SocketFilterList.DoFilter(wsBuffer.buf, wsBuffer.len);

                    SocketPacketToQueue(Socket, wsBuffer.buf, wsBuffer.len, stSocketType, new Socket_Packet.sockaddr(), BytesRecvd);
                    Socket_Operation.DoLog_HookInfo(MethodBase.GetCurrentMethod().Name, stSocketType, Socket, wsBuffer.len, BytesRecvd);
                }                    
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//ws2_32.dll WSARecvFrom Hook

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 WSARecvFrom(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, Int32 flags, ref Socket_Packet.sockaddr from, ref Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 DWSARecvFrom(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, Int32 flags, ref Socket_Packet.sockaddr from, ref Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine);

        private static unsafe Int32 WSARecvFrom_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, Int32 flags, ref Socket_Packet.sockaddr from, ref Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine)
        {
            Int32 res = 0;
            int BytesRecvd = 0;

            try
            {
                Socket_Packet.SocketType stSocketType = Socket_Packet.SocketType.WSARecvFrom;

                Socket_Packet.WSABUF wsBuffer;
                wsBuffer = (Socket_Packet.WSABUF)Marshal.PtrToStructure(lpBuffers, typeof(Socket_Packet.WSABUF));

                Socket_Cache.SocketFilterList.DoFilter(wsBuffer.buf, wsBuffer.len);

                res = WSARecvFrom(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, flags, ref from, ref fromLen, overlapped, completionRoutine);
                BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

                if (res == 0 && BytesRecvd > 0)
                {
                    SocketPacketToQueue(Socket, wsBuffer.buf, wsBuffer.len, stSocketType, from, BytesRecvd);
                    Socket_Operation.DoLog_HookInfo(MethodBase.GetCurrentMethod().Name, stSocketType, Socket, wsBuffer.len, BytesRecvd);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WinSockHook Run

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
                if (Socket_Cache.Hook_Send)
                {
                    lhSend = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "send"), new DSend(Send_Hook), this);
                    lhSend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                }

                if (Socket_Cache.Hook_SendTo)
                {
                    lhSendTo = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "sendto"), new DSendTo(SendTo_Hook), this);
                    lhSendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                }

                if (Socket_Cache.Hook_Recv)
                {
                    lhRecv = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "recv"), new Drecv(Recv_Hook), this);
                    lhRecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                }

                if (Socket_Cache.Hook_RecvFrom)
                {
                    lhRecvFrom = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "recvfrom"), new DRecvFrom(RecvFrom_Hook), this);
                    lhRecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                }

                if (Socket_Cache.Hook_WSASend)
                {
                    lhWSASend = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "WSASend"), new DWSASend(WSASend_Hook), this);
                    lhWSASend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                }

                if (Socket_Cache.Hook_WSASendTo)
                {
                    lhWSASendTo = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "WSASendTo"), new DWSASendTo(WSASendTo_Hook), this);
                    lhWSASendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                }

                if (Socket_Cache.Hook_WSARecv)
                {
                    lhWSARecv = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "WSARecv"), new DWSARecv(WSARecv_Hook), this);
                    lhWSARecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                }

                if (Socket_Cache.Hook_WSARecvFrom)
                {
                    lhWSARecvFrom = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "WSARecvFrom"), new DWSARecvFrom(WSARecvFrom_Hook), this);
                    lhWSARecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
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
                int res = send(socket, buffer, length, 0);

                if (res > 0)
                {
                    bReturn = true;
                    
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_80));
                }
                else
                {
                    bReturn = false;
                    
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_81));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;            
        }
        #endregion

        #region//封包入队列（异步）
        private static void SocketPacketToQueue(int iSocket, IntPtr ipBuff, int iLen, Socket_Packet.SocketType sType, Socket_Packet.sockaddr sAddr, int iResLen)
        {
            try
            {
                byte[] bBuffer = new byte[iResLen];
                Marshal.Copy(ipBuff, bBuffer, 0, iResLen);

                Socket_Packet sp = new Socket_Packet(iSocket, ipBuff, iLen, sType, sAddr, bBuffer, iResLen);

                Thread t = new Thread(new ParameterizedThreadStart(Socket_Cache.SocketQueue.SocketToQueue_Thread));
                t.Start(sp);
            }
            catch (Exception ex) 
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion
    }
}
