using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using EasyHook;

namespace WPELibrary.Lib
{
    public class WinSockHook : IEntryPoint
    {
        private static string sLanguage_UI = "";
        private LocalHook lhSend, lhSendTo, lhRecv, lhRecvFrom, lhWSASend, lhWSARecv;

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
                Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

                if (Socket_Cache.Interecept_Send)
                {
                    byte[] bBuff_NULL = new byte[length];
                    Socket_Operation.SetByteToIntPtr(bBuff_NULL, buffer, length);

                    Socket_Cache.SocketQueue.Interecept_CNT++;
                    stSocketType = Socket_Packet.SocketType.Send_Interecept;                    
                }
                else
                {
                    stSocketType = Socket_Packet.SocketType.Send;
                    Socket_Cache.SocketFilterList.DoFilter(buffer, length);                    
                }

                res = send(socket, buffer, length, flags);

                if (res > 0 && length > 0)
                {
                    if (Socket_Cache.Display_Send && !Socket_Cache.Interecept_Send)
                    {
                        Socket_Cache.SocketQueue.Send_CNT++;
                        Socket_Cache.SocketQueue.SocketToQueue(socket, buffer, length, stSocketType, new Socket_Packet.sockaddr(), res);
                    }
                }

                Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

                Socket_Packet.WSABUF wsBuffer;
                wsBuffer = (Socket_Packet.WSABUF)Marshal.PtrToStructure(lpBuffers, typeof(Socket_Packet.WSABUF));

                if (Socket_Cache.Interecept_Send)
                {
                    byte[] bBuff_NULL = new byte[wsBuffer.len];
                    Socket_Operation.SetByteToIntPtr(bBuff_NULL, wsBuffer.buf, wsBuffer.len);

                    Socket_Cache.SocketQueue.Interecept_CNT++;
                    stSocketType = Socket_Packet.SocketType.WSASend_Interecept;                    
                }
                else
                {
                    stSocketType = Socket_Packet.SocketType.WSASend;

                    Socket_Cache.SocketFilterList.DoFilter(wsBuffer.buf, (int)wsBuffer.len);
                }

                res = WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, dwFlags, lpOverlapped, lpCompletionRoutine);
                BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

                if (res == 0 && BytesSent > 0)
                {
                    if (Socket_Cache.Display_Send && !Socket_Cache.Interecept_Send)
                    {
                        Socket_Cache.SocketQueue.Send_CNT++;                        
                        Socket_Cache.SocketQueue.SocketToQueue(Socket, wsBuffer.buf, wsBuffer.len, stSocketType, new Socket_Packet.sockaddr(), BytesSent);                        
                    }
                }

                Socket_Operation.DoLog_HookInfo(stSocketType, Socket, wsBuffer.len, BytesSent);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

                if (Socket_Cache.Interecept_SendTo)
                {
                    byte[] bBuff_NULL = new byte[length];
                    Socket_Operation.SetByteToIntPtr(bBuff_NULL, buffer, length);

                    Socket_Cache.SocketQueue.Interecept_CNT++;
                    stSocketType = Socket_Packet.SocketType.SendTo_Interecept;                    
                }
                else
                {
                    stSocketType = Socket_Packet.SocketType.SendTo;
                    Socket_Cache.SocketFilterList.DoFilter(buffer, length);
                }

                res = sendto(socket, buffer, length, flags, ref To, ref toLenth);

                if (res > 0 && length > 0)
                {
                    if (Socket_Cache.Display_SendTo)
                    {
                        Socket_Cache.SocketQueue.Send_CNT++;                        
                        Socket_Cache.SocketQueue.SocketToQueue(socket, buffer, length, stSocketType, To, res);
                    }
                }

                Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                res = recv(socket, buffer, length, flags);

                Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

                if (res > 0)
                {
                    if (Socket_Cache.Interecept_Recv)
                    {
                        byte[] bBuff_NULL = new byte[res];
                        Socket_Operation.SetByteToIntPtr(bBuff_NULL, buffer, res);

                        Socket_Cache.SocketQueue.Interecept_CNT++;
                        stSocketType = Socket_Packet.SocketType.Recv_Interecept;
                    }
                    else
                    {
                        Socket_Cache.SocketFilterList.DoFilter(buffer, length);

                        if (Socket_Cache.Display_Recv)
                        {
                            Socket_Cache.SocketQueue.Recv_CNT++;
                            stSocketType = Socket_Packet.SocketType.Recv;
                            Socket_Cache.SocketQueue.SocketToQueue(socket, buffer, length, stSocketType, new Socket_Packet.sockaddr(), res);
                        }
                    }

                    Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);                
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
                Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

                Socket_Packet.WSABUF wsBuffer;
                wsBuffer = (Socket_Packet.WSABUF)Marshal.PtrToStructure(lpBuffers, typeof(Socket_Packet.WSABUF));

                res = WSARecv(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, flags, overlapped, completionRoutine);
                BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

                if (res == 0 && BytesRecvd > 0)
                {
                    if (Socket_Cache.Interecept_Recv)
                    {
                        byte[] bBuff_NULL = new byte[wsBuffer.len];
                        Socket_Operation.SetByteToIntPtr(bBuff_NULL, wsBuffer.buf, wsBuffer.len);

                        Socket_Cache.SocketQueue.Interecept_CNT++;
                        stSocketType = Socket_Packet.SocketType.WSARecv_Interecept;                        
                    }
                    else
                    {
                        Socket_Cache.SocketFilterList.DoFilter(wsBuffer.buf, wsBuffer.len);

                        if (Socket_Cache.Display_Recv)
                        {
                            Socket_Cache.SocketQueue.Recv_CNT++;
                            stSocketType = Socket_Packet.SocketType.WSARecv;
                            Socket_Cache.SocketQueue.SocketToQueue(Socket, wsBuffer.buf, wsBuffer.len, stSocketType, new Socket_Packet.sockaddr(), BytesRecvd);
                        }
                    }

                    Socket_Operation.DoLog_HookInfo(stSocketType, Socket, wsBuffer.len, BytesRecvd);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                res = recvfrom(socket, buffer, length, flags, ref from, ref fromLen);

                Socket_Packet.SocketType stSocketType = new Socket_Packet.SocketType();

                if (res > 0)
                {
                    if (Socket_Cache.Interecept_RecvFrom)
                    {
                        byte[] bBuff_NULL = new byte[res];
                        Socket_Operation.SetByteToIntPtr(bBuff_NULL, buffer, res);

                        Socket_Cache.SocketQueue.Interecept_CNT++;
                        stSocketType = Socket_Packet.SocketType.RecvFrom_Interecept;
                    }
                    else
                    {
                        Socket_Cache.SocketFilterList.DoFilter(buffer, length);

                        if (Socket_Cache.Display_RecvFrom)
                        {
                            Socket_Cache.SocketQueue.Recv_CNT++;
                            stSocketType = Socket_Packet.SocketType.RecvFrom;
                            Socket_Cache.SocketQueue.SocketToQueue(socket, buffer, length, stSocketType, from, res);
                        }
                    }

                    Socket_Operation.DoLog_HookInfo(stSocketType, socket, length, res);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    SetProcessDPIAware();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Socket_Form(InArg));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }          
        }

        #endregion

        #region//开始拦截
        public void StartHook()
        {
            try
            {
                lhRecv = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "recv"), new Drecv(Recv_Hook), this);
                lhRecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });                

                lhRecvFrom = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "recvfrom"), new DRecvFrom(RecvFrom_Hook), this);
                lhRecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                lhSend = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "send"), new DSend(Send_Hook), this);
                lhSend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                lhSendTo = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "sendto"), new DSendTo(SendTo_Hook), this);
                lhSendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                lhWSASend = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "WSASend"), new DWSASend(WSASend_Hook), this);
                lhWSASend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                lhWSARecv = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "WSARecv"), new DWSARecv(WSARecv_Hook), this);
                lhWSARecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            
        }
        #endregion

        #region//停止拦截
        public void StopHook()
        {
            try
            {
                lhRecv.Dispose();
                lhSend.Dispose();
                lhRecvFrom.Dispose();
                lhSendTo.Dispose();
                lhWSASend.Dispose();
                lhWSARecv.Dispose();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("发送封包成功！", "Successfully sent packet!");
                    Socket_Operation.DoLog(sLanguage_UI);
                }
                else
                {
                    bReturn = false;
                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("发送封包失败！", "Sending packet failed!");
                    Socket_Operation.DoLog(sLanguage_UI);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }

            return bReturn;            
        }
        #endregion
    }
}
