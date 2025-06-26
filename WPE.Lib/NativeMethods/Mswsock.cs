using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace WPE.Lib.NativeMethods
{
    public static class Mswsock
    {
        public static string ModuleName = "Mswsock.dll";

        #region//WSARecvEx

        [DllImport("Mswsock.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public unsafe static extern Int32 WSARecvEx(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In, Out] SocketFlags Flags
            );

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 DWSARecvEx(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In, Out] SocketFlags Flags
            );

        public static unsafe Int32 WSARecvExHook(
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In, Out] SocketFlags Flags)
        {
            Operate.PacketConfig.Packet.PacketType ptType = Operate.PacketConfig.Packet.PacketType.WSARecvEx;
            return WinSockHook.Recv_Hook(ptType, Socket, lpBuffer, Length, Flags);
        }

        #endregion
    }
}
