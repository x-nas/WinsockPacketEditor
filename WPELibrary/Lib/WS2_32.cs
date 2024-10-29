using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
            Socket_Cache.FilterList.DoFilter(buffer, length);

            Int32 res = send(socket, buffer, length, flags);

            if (res > 0)
            {
                Task.Run(() =>
                {
                    Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.Send;
                    byte[] bBuffer = Socket_Operation.GetBytes_FromIntPtr(buffer, res);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuffer, ptType, new Socket_Cache.SocketPacket.sockaddr(), res);
                });
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
            Int32 res = recv(socket, buffer, length, flags);

            if (res > 0)
            {
                Socket_Cache.FilterList.DoFilter(buffer, res);

                Task.Run(() =>
                {
                    Socket_Cache.SocketPacket.PacketType stSocketType = Socket_Cache.SocketPacket.PacketType.Recv;
                    byte[] bBuff = Socket_Operation.GetBytes_FromIntPtr(buffer, res);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuff, stSocketType, new Socket_Cache.SocketPacket.sockaddr(), res);
                });
            }

            return res;
        }

        #endregion        

        #region//SendTo

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern Int32 sendto(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr To, Int32 toLenth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 DSendTo(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr To, Int32 toLenth);

        public static unsafe Int32 SendTo_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr To, Int32 toLenth)
        {
            Socket_Cache.FilterList.DoFilter(buffer, length);

            Int32 res = sendto(socket, buffer, length, flags, To, toLenth);

            if (res > 0)
            {
                Task.Run(() =>
                {
                    Socket_Cache.SocketPacket.sockaddr saTo = Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(To);
                    Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.SendTo;
                    byte[] bBuff = Socket_Operation.GetBytes_FromIntPtr(buffer, res);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuff, ptType, saTo, res);
                });
            }          

            return res;
        }

        #endregion

        #region//RecvFrom

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern Int32 recvfrom(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr from, Int32 fromLen);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 DRecvFrom(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr from, Int32 fromLen);

        public static unsafe Int32 RecvFrom_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr from, Int32 fromLen)
        {
            Int32 res = recvfrom(socket, buffer, length, flags, from, fromLen);

            if (res > 0)
            {
                Socket_Cache.FilterList.DoFilter(buffer, res);

                Task.Run(() =>
                {
                    Socket_Cache.SocketPacket.PacketType stSocketType = Socket_Cache.SocketPacket.PacketType.RecvFrom;
                    Socket_Cache.SocketPacket.sockaddr saFrom = Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(from);
                    byte[] bBuff = Socket_Operation.GetBytes_FromIntPtr(buffer, res);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuff, stSocketType, saFrom, res);
                });
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
            for (int i = 0; i < dwBufferCount; i++)
            {
                IntPtr lpNewBuffer = IntPtr.Add(lpBuffers, sizeof(Socket_Cache.SocketPacket.WSABUF) * i);

                Socket_Cache.SocketPacket.WSABUF wsBuffer = Marshal.PtrToStructure<Socket_Cache.SocketPacket.WSABUF>(lpNewBuffer);                
                Socket_Cache.FilterList.DoFilter(wsBuffer.buf, wsBuffer.len);
            }

            SocketError res = WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, ref dwFlags, lpOverlapped, lpCompletionRoutine);
            int BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

            if (res == SocketError.Success && BytesSent > 0)
            {
                Task.Run(() =>
                {
                    Socket_Cache.SocketPacket.PacketType stSocketType = Socket_Cache.SocketPacket.PacketType.WSASend;
                    byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, stSocketType, new Socket_Cache.SocketPacket.sockaddr(), bBuff.Length);
                });
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
            SocketError res = WSARecv(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref flags, overlapped, completionRoutine);
            int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

            if (res == SocketError.Success && BytesRecvd > 0)
            {
                for (int i = 0; i < dwBufferCount; i++)
                {
                    IntPtr lpNewBuffer = IntPtr.Add(lpBuffers, sizeof(Socket_Cache.SocketPacket.WSABUF) * i);

                    Socket_Cache.SocketPacket.WSABUF wsBuffer = Marshal.PtrToStructure<Socket_Cache.SocketPacket.WSABUF>(lpNewBuffer);
                    Socket_Cache.FilterList.DoFilter(wsBuffer.buf, wsBuffer.len);
                }

                Task.Run(() =>
                {
                    Socket_Cache.SocketPacket.PacketType stSocketType = Socket_Cache.SocketPacket.PacketType.WSARecv;
                    byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, stSocketType, new Socket_Cache.SocketPacket.sockaddr(), bBuff.Length);
                });
            }            

            return res;
        }

        #endregion

        #region//WSASendTo

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSASendTo(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr To, Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSASendTo(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr To, Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        public static unsafe SocketError WSASendTo_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr To, Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            for (int i = 0; i < dwBufferCount; i++)
            {
                IntPtr lpNewBuffer = IntPtr.Add(lpBuffers, sizeof(Socket_Cache.SocketPacket.WSABUF) * i);

                Socket_Cache.SocketPacket.WSABUF wsBuffer = Marshal.PtrToStructure<Socket_Cache.SocketPacket.WSABUF>(lpNewBuffer);
                Socket_Cache.FilterList.DoFilter(wsBuffer.buf, wsBuffer.len);
            }

            SocketError res = WSASendTo(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, ref dwFlags, To, toLenth, lpOverlapped, lpCompletionRoutine);
            int BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

            if (res == SocketError.Success && BytesSent > 0)
            {
                Task.Run(() =>
                {
                    Socket_Cache.SocketPacket.PacketType stSocketType = Socket_Cache.SocketPacket.PacketType.WSASendTo;
                    Socket_Cache.SocketPacket.sockaddr saTo = Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(To);
                    byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, stSocketType, saTo, bBuff.Length);
                });
            }

            return res;
        }

        #endregion

        #region//WSARecvFrom

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSARecvFrom(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr from, Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSARecvFrom(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr from, Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine);

        public static unsafe SocketError WSARecvFrom_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr from, Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine)
        {
            SocketError res = WSARecvFrom(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref flags, from, fromLen, overlapped, completionRoutine);
            int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);

            if (res == SocketError.Success && BytesRecvd > 0)
            {
                for (int i = 0; i < dwBufferCount; i++)
                {
                    IntPtr lpNewBuffer = IntPtr.Add(lpBuffers, sizeof(Socket_Cache.SocketPacket.WSABUF) * i);

                    Socket_Cache.SocketPacket.WSABUF wsBuffer = Marshal.PtrToStructure<Socket_Cache.SocketPacket.WSABUF>(lpNewBuffer);
                    Socket_Cache.FilterList.DoFilter(wsBuffer.buf, wsBuffer.len);
                }

                Task.Run(() =>
                {
                    Socket_Cache.SocketPacket.PacketType stSocketType = Socket_Cache.SocketPacket.PacketType.WSARecvFrom;
                    Socket_Cache.SocketPacket.sockaddr saFrom = Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(from);
                    byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);
                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, stSocketType, saFrom, bBuff.Length);
                });
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

        #region//inet_addr

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern uint inet_addr(string cp);

        #endregion

        #region//ntohs

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern ushort ntohs(ushort netshort);

        #endregion

        #region//htons

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern ushort htons(ushort hostshort);

        #endregion       
    }
}
