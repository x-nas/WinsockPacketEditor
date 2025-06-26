using System.Net.Sockets;
using System.Runtime.InteropServices;
using System;

namespace WPE.Lib.NativeMethods
{
    public static class WSock32
    {
        public static string ModuleName = "WSOCK32.dll";

        #region//Send

        [DllImport("WSOCK32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
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
            Operate.PacketConfig.Packet.PacketType ptType = Operate.PacketConfig.Packet.PacketType.WS1_Send;
            return WinSockHook.Send_Hook(ptType, Socket, lpBuffer, Length, Flags);
        }

        #endregion

        #region//Recv

        [DllImport("WSOCK32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
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
            Operate.PacketConfig.Packet.PacketType ptType = Operate.PacketConfig.Packet.PacketType.WS1_Recv;
            return WinSockHook.Recv_Hook(ptType, Socket, lpBuffer, Length, Flags);
        }

        #endregion

        #region//SendTo

        [DllImport("WSOCK32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern Int32 sendto(
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] ref Operate.PacketConfig.Packet.SockAddr To,
            [In] Int32 ToLen
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]

        public unsafe delegate Int32 DSendTo(
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] ref Operate.PacketConfig.Packet.SockAddr To,
            [In] Int32 ToLen
            );

        public static unsafe Int32 SendToHook(
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] ref Operate.PacketConfig.Packet.SockAddr To,
            [In] Int32 ToLen)
        {
            Operate.PacketConfig.Packet.PacketType ptType = Operate.PacketConfig.Packet.PacketType.WS1_SendTo;
            return WinSockHook.SendTo_Hook(ptType, Socket, lpBuffer, Length, Flags, ref To, ToLen);
        }

        #endregion

        #region//RecvFrom

        [DllImport("WSOCK32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern Int32 recvfrom(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] ref Operate.PacketConfig.Packet.SockAddr From,
            [In, Out, Optional] IntPtr FromLen
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]

        public unsafe delegate Int32 DRecvFrom(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] ref Operate.PacketConfig.Packet.SockAddr From,
            [In, Out, Optional] IntPtr FromLen
            );

        public static unsafe Int32 RecvFromHook(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] ref Operate.PacketConfig.Packet.SockAddr From,
            [In, Out, Optional] IntPtr FromLen)
        {
            Operate.PacketConfig.Packet.PacketType ptType = Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom;
            return WinSockHook.RecvFrom_Hook(ptType, Socket, lpBuffer, Length, Flags, ref From, FromLen);
        }

        #endregion
    }
}
