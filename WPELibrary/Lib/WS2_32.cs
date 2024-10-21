using System;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;

namespace WPELibrary.Lib
{
    internal class WS2_32
    {
        public static string ModuleName = "WS2_32.dll";

        #region//Send

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern Int32 send(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 DSend(Int32 s, IntPtr buf, Int32 len, SocketFlags flags);

        public static unsafe Int32 Send_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags)
        {
            Int32 res = 0;

            try
            {
                Socket_Cache.SocketPacket.SocketType stSocketType = Socket_Cache.SocketPacket.SocketType.Send;
                Socket_Cache.FilterList.DoFilter(buffer, length);
                res = send(socket, buffer, length, flags);

                if (res > 0 && length > 0)
                {
                    byte[] bBuff = Socket_Operation.GetByte_FromIntPtr(buffer, res);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuff, stSocketType, new Socket_Cache.SocketPacket.sockaddr(), res);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//SendTo

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern Int32 sendto(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, ref Socket_Cache.SocketPacket.sockaddr To, ref Int32 toLenth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 DSendTo(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, ref Socket_Cache.SocketPacket.sockaddr To, ref Int32 toLenth);
        public static unsafe Int32 SendTo_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, ref Socket_Cache.SocketPacket.sockaddr To, ref Int32 toLenth)
        {
            Int32 res = 0;

            try
            {
                Socket_Cache.SocketPacket.SocketType stSocketType = Socket_Cache.SocketPacket.SocketType.SendTo;
                Socket_Cache.FilterList.DoFilter(buffer, length);
                res = sendto(socket, buffer, length, flags, ref To, ref toLenth);

                if (res > 0 && length > 0)
                {
                    byte[] bBuff = Socket_Operation.GetByte_FromIntPtr(buffer, res);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuff, stSocketType, To, res);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//Recv

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern Int32 recv(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 Drecv(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags);

        public static unsafe Int32 Recv_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags)
        {
            Int32 res = 0;

            try
            {
                Socket_Cache.SocketPacket.SocketType stSocketType = Socket_Cache.SocketPacket.SocketType.Recv;
                res = recv(socket, buffer, length, flags);

                if (res > 0)
                {
                    Socket_Cache.FilterList.DoFilter(buffer, res);

                    byte[] bBuff = Socket_Operation.GetByte_FromIntPtr(buffer, res);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuff, stSocketType, new Socket_Cache.SocketPacket.sockaddr(), res);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion        

        #region//RecvFrom

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern Int32 recvfrom(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, ref Socket_Cache.SocketPacket.sockaddr from, ref Int32 fromLen);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 DRecvFrom(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, ref Socket_Cache.SocketPacket.sockaddr from, ref Int32 fromLen);

        public static unsafe Int32 RecvFrom_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, ref Socket_Cache.SocketPacket.sockaddr from, ref Int32 fromLen)
        {
            Int32 res = 0;

            try
            {
                Socket_Cache.SocketPacket.SocketType stSocketType = Socket_Cache.SocketPacket.SocketType.RecvFrom;
                res = recvfrom(socket, buffer, length, flags, ref from, ref fromLen);

                if (res > 0)
                {
                    Socket_Cache.FilterList.DoFilter(buffer, res);

                    byte[] bBuff = Socket_Operation.GetByte_FromIntPtr(buffer, res);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuff, stSocketType, from, res);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSASend

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSASend(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSASend(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        public static unsafe SocketError WSASend_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            SocketError res = 0;
            int BytesSent = 0;

            try
            {
                Socket_Cache.SocketPacket.SocketType stSocketType = Socket_Cache.SocketPacket.SocketType.WSASend;

                for (int i = 0; i < dwBufferCount; i++)
                {
                    Socket_Cache.SocketPacket.WSABUF wsBuffer = (Socket_Cache.SocketPacket.WSABUF)Marshal.PtrToStructure(IntPtr.Add(lpBuffers, sizeof(Socket_Cache.SocketPacket.WSABUF) * i), typeof(Socket_Cache.SocketPacket.WSABUF));
                    Socket_Cache.FilterList.DoFilter(wsBuffer.buf, wsBuffer.len);
                }

                res = WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, ref dwFlags, lpOverlapped, lpCompletionRoutine);
                BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

                if (res == SocketError.Success && BytesSent > 0)
                {
                    byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, stSocketType, new Socket_Cache.SocketPacket.sockaddr(), bBuff.Length);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSASendTo

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSASendTo(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, ref Socket_Cache.SocketPacket.sockaddr To, ref Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSASendTo(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, ref Socket_Cache.SocketPacket.sockaddr To, ref Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        public static unsafe SocketError WSASendTo_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, ref Socket_Cache.SocketPacket.sockaddr To, ref Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            SocketError res = 0;
            int BytesSent = 0;

            try
            {
                Socket_Cache.SocketPacket.SocketType stSocketType = Socket_Cache.SocketPacket.SocketType.WSASendTo;

                for (int i = 0; i < dwBufferCount; i++)
                {
                    Socket_Cache.SocketPacket.WSABUF wsBuffer = (Socket_Cache.SocketPacket.WSABUF)Marshal.PtrToStructure(IntPtr.Add(lpBuffers, sizeof(Socket_Cache.SocketPacket.WSABUF) * i), typeof(Socket_Cache.SocketPacket.WSABUF));
                    Socket_Cache.FilterList.DoFilter(wsBuffer.buf, wsBuffer.len);
                }

                res = WSASendTo(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, ref dwFlags, ref To, ref toLenth, lpOverlapped, lpCompletionRoutine);
                BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

                if (res == SocketError.Success && BytesSent > 0)
                {
                    byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, stSocketType, To, bBuff.Length);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSARecv

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSARecv(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr overlapped, IntPtr completionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSARecv(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr overlapped, IntPtr completionRoutine);

        public static unsafe SocketError WSARecv_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr overlapped, IntPtr completionRoutine)
        {
            SocketError res = 0;
            int BytesRecvd = 0;

            try
            {
                Socket_Cache.SocketPacket.SocketType stSocketType = Socket_Cache.SocketPacket.SocketType.WSARecv;

                res = WSARecv(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref flags, overlapped, completionRoutine);
                BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

                if (res == SocketError.Success && BytesRecvd > 0)
                {
                    byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, stSocketType, new Socket_Cache.SocketPacket.sockaddr(), bBuff.Length);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSARecvFrom

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSARecvFrom(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, ref Socket_Cache.SocketPacket.sockaddr from, ref Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSARecvFrom(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, ref Socket_Cache.SocketPacket.sockaddr from, ref Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine);

        public static unsafe SocketError WSARecvFrom_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, ref Socket_Cache.SocketPacket.sockaddr from, ref Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine)
        {
            SocketError res = 0;
            int BytesRecvd = 0;

            try
            {
                Socket_Cache.SocketPacket.SocketType stSocketType = Socket_Cache.SocketPacket.SocketType.WSARecvFrom;

                res = WSARecvFrom(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref flags, ref from, ref fromLen, overlapped, completionRoutine);
                BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

                if (res == SocketError.Success && BytesRecvd > 0)
                {
                    byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, stSocketType, from, bBuff.Length);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSAGetLastError

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern int WSAGetLastError();

        #endregion

        #region//getsockname

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern int getsockname(int s, ref Socket_Cache.SocketPacket.sockaddr Address, ref int namelen);

        #endregion

        #region//getpeername

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern int getpeername(int s, ref Socket_Cache.SocketPacket.sockaddr Address, ref int namelen);

        #endregion

        #region//inet_ntoa

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern IntPtr inet_ntoa(Socket_Cache.SocketPacket.in_addr a);

        #endregion

        #region//ntohs

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern ushort ntohs(ushort netshort);

        #endregion
    }
}
