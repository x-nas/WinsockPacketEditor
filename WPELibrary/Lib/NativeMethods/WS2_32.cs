using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace WPELibrary.Lib.NativeMethods
{
    internal class WS2_32
    {
        public static string ModuleName = "WS2_32.dll";        

        #region//WSAGetLastError

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SocketError WSAGetLastError();

        #endregion

        #region//WSAGetOverlappedResult

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WSAGetOverlappedResult(
            Int32 Socket,
            ref Socket_Cache.SocketPacket.OVERLAPPED lpOverlapped,
            out int bytesTransferred,
            bool wait,
            out SocketFlags flags
            );

        #endregion

        #region//WSAWaitForMultipleEvents

        [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSAWaitForMultipleEvents(
            int cEvents, 
            IntPtr[] lphEvents, 
            bool fWaitAll, 
            int dwMilliseconds, 
            bool bAlertable
            );

        #endregion

        #region//WSACreateEvent

        [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr WSACreateEvent();

        #endregion

        #region//getsockname

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern int getsockname(
            [In] int Socket, 
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr Address, 
            [In, Out] ref int namelen
            );

        #endregion

        #region//getpeername

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern int getpeername(
            [In] int Socket, 
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr Address, 
            [In, Out] ref int namelen
            );

        #endregion

        #region//inet_ntoa

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public static extern IntPtr inet_ntoa(uint a);

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

        #region//Send

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern Int32 send(
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 DSend(
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags
            );

        public static unsafe Int32 SendHook(
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WS2_Send;
            return WinSockHook.Send_Hook(ptType, Socket, lpBuffer, Length, Flags);
        }

        #endregion

        #region//Recv

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern Int32 recv(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 Drecv(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags
            );

        public static unsafe Int32 RecvHook(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WS2_Recv;
            return WinSockHook.Recv_Hook(ptType, Socket, lpBuffer, Length,Flags);          
        }

        #endregion

        #region//SendTo

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern Int32 sendto(
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] Int32 ToLen
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]

        public unsafe delegate Int32 DSendTo(
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] Int32 ToLen
            );

        public static unsafe Int32 SendToHook(
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] Int32 ToLen)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WS2_SendTo;
            return WinSockHook.SendTo_Hook(ptType, Socket, lpBuffer, Length, Flags, ref To, ToLen);
        }

        #endregion

        #region//RecvFrom

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern Int32 recvfrom(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr From,
            [In, Out, Optional] IntPtr FromLen
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]

        public unsafe delegate Int32 DRecvFrom(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr From,
            [In, Out, Optional] IntPtr FromLen
            );

        public static unsafe Int32 RecvFromHook(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr From,
            [In, Out, Optional] IntPtr FromLen)
        {
            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom;
            return WinSockHook.RecvFrom_Hook(ptType, Socket, lpBuffer, Length, Flags, ref From, FromLen);
        }

        #endregion

        #region//WSASend

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern SocketError WSASend(
            [In] Int32 Socket,
            [In] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesSend,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSASend(
            [In] Int32 Socket,
            [In] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesSend,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine
            );

        #endregion

        #region//WSARecv

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern SocketError WSARecv(
            [In] Int32 Socket,
            [In, Out] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]

        public unsafe delegate SocketError DWSARecv(
            [In] Int32 Socket,
            [In, Out] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine
            );

        #endregion

        #region//WSASendTo

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern SocketError WSASendTo(
            [In] Int32 Socket,
            [In] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesSend,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] IntPtr lpToLen,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]

        public unsafe delegate SocketError DWSASendTo(
            [In] Int32 Socket,
            [In] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesSend,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] IntPtr lpToLen,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine
            );

        #endregion

        #region//WSARecvFrom       

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern SocketError WSARecvFrom(
            [In] Int32 Socket,
            [In, Out] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags Flags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr From,
            [In, Out] IntPtr lpFromlen,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine
            );
       
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]

        public unsafe delegate SocketError DWSARecvFrom(
            [In] Int32 Socket,
            [In, Out] ref Socket_Cache.SocketPacket.WSABUF WSABuffer,
            [In] Int32 BufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags Flags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr From,
            [In, Out] IntPtr lpFromlen,
            [In] ref Socket_Cache.SocketPacket.OVERLAPPED Overlapped,
            [In] IntPtr lpCompletionRoutine
           );

        #endregion
    }
}
