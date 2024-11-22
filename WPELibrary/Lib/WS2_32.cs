using System;
using System.Net.Sockets;
using System.Reflection;
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
            Int32 res = 0;

            try
            {
                Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.Send;
                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.GetFilterAction_ByDoFilter(ptType, socket, buffer, length);

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = 0;
                }                
                else
                {
                    res = send(socket, buffer, length, flags);

                    if (res > 0)
                    {
                        Task.Run(() =>
                        {
                            Socket_Operation.CountSocketInfo(ptType, res);
                        });

                        if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                        {
                            if (!Socket_Cache.SpeedMode)
                            {
                                byte[] bBuffer = Socket_Operation.GetBytes_FromIntPtr(buffer, res);

                                Task.Run(() =>
                                {
                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuffer, ptType, new Socket_Cache.SocketPacket.sockaddr(), res);
                                });
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
                res = recv(socket, buffer, length, flags);

                if (res > 0)
                {
                    Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.Recv;
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.GetFilterAction_ByDoFilter(ptType, socket, buffer, res);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            Socket_Operation.CountSocketInfo(ptType, res);
                        });

                        if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                        {
                            if (!Socket_Cache.SpeedMode)
                            {
                                byte[] bBuffer = Socket_Operation.GetBytes_FromIntPtr(buffer, res);

                                Task.Run(() =>
                                {
                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuffer, ptType, new Socket_Cache.SocketPacket.sockaddr(), res);
                                });
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

        #region//SendTo

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern Int32 sendto(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr To, Int32 toLenth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 DSendTo(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr To, Int32 toLenth);

        public static unsafe Int32 SendTo_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr To, Int32 toLenth)
        {
            Int32 res = 0;

            try
            {
                Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.SendTo;
                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.GetFilterAction_ByDoFilter(ptType, socket, buffer, length);

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = 0;
                }
                else
                {
                    res = sendto(socket, buffer, length, flags, To, toLenth);

                    if (res > 0)
                    {
                        Task.Run(() =>
                        {
                            Socket_Operation.CountSocketInfo(ptType, res);
                        });

                        if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                        {
                            if (!Socket_Cache.SpeedMode)
                            {
                                byte[] bBuffer = Socket_Operation.GetBytes_FromIntPtr(buffer, res);

                                Task.Run(() =>
                                {
                                    Socket_Cache.SocketPacket.sockaddr saTo = Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(To);
                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuffer, ptType, saTo, res);
                                });
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

        #region//RecvFrom

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern Int32 recvfrom(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr from, Int32 fromLen);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate Int32 DRecvFrom(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr from, Int32 fromLen);

        public static unsafe Int32 RecvFrom_Hook(Int32 socket, IntPtr buffer, Int32 length, SocketFlags flags, IntPtr from, Int32 fromLen)
        {
            Int32 res = 0;

            try
            {
                res = recvfrom(socket, buffer, length, flags, from, fromLen);

                if (res > 0)
                {                   
                    Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.RecvFrom;
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.GetFilterAction_ByDoFilter(ptType, socket, buffer, res);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            Socket_Operation.CountSocketInfo(ptType, res);
                        });

                        if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                        {
                            if (!Socket_Cache.SpeedMode)
                            {
                                byte[] bBuffer = Socket_Operation.GetBytes_FromIntPtr(buffer, res);

                                Task.Run(() =>
                                {
                                    Socket_Cache.SocketPacket.sockaddr saFrom = Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(from);
                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(socket, bBuffer, ptType, saFrom, res);
                                });
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

        #region//WSASend

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSASend(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSASend(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        public static unsafe SocketError WSASend_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            SocketError res = SocketError.SocketError;

            try
            {
                Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSASend;

                int BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);
                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.GetFilterAction_ByDoWSAFilter(ptType, Socket, lpBuffers, dwBufferCount, BytesSent);

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = SocketError.Success;
                }
                else
                {
                    res = WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, ref dwFlags, lpOverlapped, lpCompletionRoutine);

                    if (res == SocketError.Success)
                    {
                        BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

                        Task.Run(() =>
                        {
                            Socket_Operation.CountSocketInfo(ptType, BytesSent);
                        });

                        if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                        {
                            if (!Socket_Cache.SpeedMode)
                            {
                                byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);

                                Task.Run(() =>
                                {
                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, ptType, new Socket_Cache.SocketPacket.sockaddr(), bBuff.Length);
                                });
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

        #region//WSARecv

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSARecv(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr overlapped, IntPtr completionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSARecv(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr overlapped, IntPtr completionRoutine);

        public static unsafe SocketError WSARecv_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr overlapped, IntPtr completionRoutine)
        {
            SocketError res = SocketError.SocketError;

            try
            {
                res = WSARecv(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref flags, overlapped, completionRoutine);

                if (res == SocketError.Success)
                {
                    Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSARecv;

                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.GetFilterAction_ByDoWSAFilter(ptType, Socket, lpBuffers, dwBufferCount, BytesRecvd);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        Marshal.WriteInt32(lpNumberOfBytesRecvd, 0);
                        res = SocketError.Success;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            Socket_Operation.CountSocketInfo(ptType, BytesRecvd);
                        });

                        if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                        {
                            if (!Socket_Cache.SpeedMode)
                            {
                                byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);

                                Task.Run(() =>
                                {
                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, ptType, new Socket_Cache.SocketPacket.sockaddr(), bBuff.Length);
                                });
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

        #region//WSASendTo

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSASendTo(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr To, Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSASendTo(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr To, Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        public static unsafe SocketError WSASendTo_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesSent, ref SocketFlags dwFlags, IntPtr To, Int32 toLenth, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            SocketError res = SocketError.Success;

            try
            {
                Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSASendTo;

                int BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);
                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.GetFilterAction_ByDoWSAFilter(ptType, Socket, lpBuffers, dwBufferCount, BytesSent);

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = SocketError.Success;
                }
                else
                {
                    res = WSASendTo(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, ref dwFlags, To, toLenth, lpOverlapped, lpCompletionRoutine);

                    if (res == SocketError.Success)
                    {
                        BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

                        Task.Run(() =>
                        {
                            Socket_Operation.CountSocketInfo(ptType, BytesSent);
                        });

                        if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                        {
                            if (!Socket_Cache.SpeedMode)
                            {
                                byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesSent);

                                Task.Run(() =>
                                {
                                    Socket_Cache.SocketPacket.sockaddr saTo = Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(To);
                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, ptType, saTo, bBuff.Length);
                                });
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

        #region//WSARecvFrom

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]

        public unsafe static extern SocketError WSARecvFrom(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr from, Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public unsafe delegate SocketError DWSARecvFrom(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr from, Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine);

        public static unsafe SocketError WSARecvFrom_Hook(Int32 Socket, IntPtr lpBuffers, Int32 dwBufferCount, IntPtr lpNumberOfBytesRecvd, ref SocketFlags flags, IntPtr from, Int32 fromLen, IntPtr overlapped, IntPtr completionRoutine)
        {
            SocketError res = SocketError.SocketError;

            try
            {
                res = WSARecvFrom(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, ref flags, from, fromLen, overlapped, completionRoutine);

                if (res == SocketError.Success)
                {
                    Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketPacket.PacketType.WSARecvFrom;

                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.GetFilterAction_ByDoWSAFilter(ptType, Socket, lpBuffers, dwBufferCount, BytesRecvd);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        Marshal.WriteInt32(lpNumberOfBytesRecvd, 0);
                        res = SocketError.Success;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            Socket_Operation.CountSocketInfo(ptType, BytesRecvd);
                        });

                        if (FilterAction != Socket_Cache.Filter.FilterAction.NoModify_NoDisplay)
                        {
                            if (!Socket_Cache.SpeedMode)
                            {
                                byte[] bBuff = Socket_Operation.GetByteFromWSABUF(lpBuffers, dwBufferCount, BytesRecvd);

                                Task.Run(() =>
                                {
                                    Socket_Cache.SocketPacket.sockaddr saFrom = Marshal.PtrToStructure<Socket_Cache.SocketPacket.sockaddr>(from);
                                    Socket_Cache.SocketQueue.SocketPacket_ToQueue(Socket, bBuff, ptType, saFrom, bBuff.Length);
                                });
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
