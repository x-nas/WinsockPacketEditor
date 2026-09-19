using EasyHook;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace WinsockPacketEditor
{
    public class WinSockHook
    {
        private LocalHook lhWS1_Send, lhWS1_SendTo, lhWS1_Recv, lhWS1_RecvFrom;
        private LocalHook lhWS2_Send, lhWS2_SendTo, lhWS2_Recv, lhWS2_RecvFrom;
        private LocalHook lhWSA_Send, lhWSA_SendTo, lhWSA_Recv, lhWSA_RecvFrom;
        private LocalHook lhWSA_RecvEx;

        [DllImport("ws2_32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void WSASetLastError(int error);

        [ThreadStatic] private static int nativeError;
        private static int Capture(int result)
        {
            nativeError = Marshal.GetLastWin32Error();
            return result;
        }
        private static SocketError Capture(SocketError result)
        {
            nativeError = Marshal.GetLastWin32Error();
            return result;
        }

        [DllImport("wsock32.dll", EntryPoint = "sendto", SetLastError = true)]
        private static extern int SendTo1(int socket, IntPtr buffer, int length, SocketFlags flags, IntPtr address, int addressLength);
        [DllImport("ws2_32.dll", EntryPoint = "sendto", SetLastError = true)]
        private static extern int SendTo2(int socket, IntPtr buffer, int length, SocketFlags flags, IntPtr address, int addressLength);
        [DllImport("wsock32.dll", EntryPoint = "recvfrom", SetLastError = true)]
        private static extern int RecvFrom1(int socket, IntPtr buffer, int length, SocketFlags flags, IntPtr address, IntPtr addressLength);
        [DllImport("ws2_32.dll", EntryPoint = "recvfrom", SetLastError = true)]
        private static extern int RecvFrom2(int socket, IntPtr buffer, int length, SocketFlags flags, IntPtr address, IntPtr addressLength);
        [DllImport("ws2_32.dll", EntryPoint = "WSASendTo", SetLastError = true)]
        private static extern SocketError SendToWsa(int socket, IntPtr buffers, int count, IntPtr sent, SocketFlags flags,
            IntPtr address, int addressLength, IntPtr overlapped, IntPtr completion);
        [DllImport("ws2_32.dll", EntryPoint = "WSARecvFrom", SetLastError = true)]
        private static extern SocketError RecvFromWsa(int socket, IntPtr buffers, int count, IntPtr received, ref SocketFlags flags,
            IntPtr address, IntPtr addressLength, IntPtr overlapped, IntPtr completion);

        private static Operate.PacketConfig.Packet.SockAddr ReadAddress(IntPtr pointer, int length)
        {
            if (pointer == IntPtr.Zero || length < 16 || Marshal.ReadInt16(pointer) != 2)
                return new Operate.PacketConfig.Packet.SockAddr();
            return Marshal.PtrToStructure<Operate.PacketConfig.Packet.SockAddr>(pointer);
        }

        [DllImport("ws2_32.dll", SetLastError = true)]
        private static extern int getsockopt(int socket, int level, int option, out int value, ref int length);

        private static void ValidateSendReplacement(int socket, byte[] raw, ref byte[] modified,
            ref Operate.FilterConfig.Filter.FilterAction action)
        {
            if (action == Operate.FilterConfig.Filter.FilterAction.Intercept || raw.Length == modified.Length) { return; }
            int size = sizeof(int), type;
            // A stream can accept only a prefix. There is no atomic 'send all' operation
            // and no way to query an already attached socket's nonblocking mode.
            if (getsockopt(socket, 0xffff, 0x1008, out type, ref size) == 0 && type == (int)SocketType.Dgram) { return; }
            modified = raw;
            action = Operate.FilterConfig.Filter.FilterAction.NoModify_Display;
            try { Operate.SystemConfig.LogThrottled("Hook.StreamLengthChange",
                "未执行变长改包：流式套接字无法保证原子发送，已按原包发送以保持非阻塞和部分发送语义。"); }
            catch { }
        }

        private static void LogHookError(string operation, object error)
        {
            // Diagnostics must never escape a native callback, even if its host failed.
            try { Operate.DoLog(operation, error == null ? string.Empty : error.ToString()); }
            catch { }
        }

        private static void ReportPacket(int socket, byte[] raw, byte[] modified, int result,
            Operate.PacketConfig.Packet.PacketType type, Operate.FilterConfig.Filter.FilterAction action,
            Operate.PacketConfig.Packet.SockAddr address, DateTime time)
        {
            try { HookHost.Current.OnPacket(socket, raw, modified, result, type, action, address, time); }
            catch (Exception ex) { LogHookError(nameof(ReportPacket), ex); }
        }

        public int InstalledCount { get; private set; }

        private LocalHook Install(string module, string function, Delegate callback)
        {
            LocalHook hook = null;
            try
            {
                hook = LocalHook.Create(LocalHook.GetProcAddress(module, function), callback, this);
                hook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                InstalledCount++;
                return hook;
            }
            catch (Exception ex)
            {
                if (hook != null) { hook.Dispose(); }
                LogHookError(nameof(StartHook), module + "!" + function + ": " + ex.Message);
                return null;
            }
        }

        #region//开始拦截

        public void StartHook()
        {
            if (InstalledCount > 0) { return; }
            try
            {
                if (Operate.PacketConfig.Packet.Support_WS1)
                {
                    #region//Winsock 1.1 Start Hook

                    if (Operate.PacketConfig.Packet.HookWS1_Send)
                    {
                        lhWS1_Send = Install(WSock32.ModuleName, "send", new WSock32.DSend(WSock32.SendHook));
                    }

                    if (Operate.PacketConfig.Packet.HookWS1_SendTo)
                    {
                        lhWS1_SendTo = Install(WSock32.ModuleName, "sendto", new WSock32.DSendTo(WSock32.SendToHook));
                    }

                    if (Operate.PacketConfig.Packet.HookWS1_Recv)
                    {
                        lhWS1_Recv = Install(WSock32.ModuleName, "recv", new WSock32.Drecv(WSock32.RecvHook));
                    }

                    if (Operate.PacketConfig.Packet.HookWS1_RecvFrom)
                    {
                        lhWS1_RecvFrom = Install(WSock32.ModuleName, "recvfrom", new WSock32.DRecvFrom(WSock32.RecvFromHook));
                    }

                    #endregion
                }

                if (Operate.PacketConfig.Packet.Support_WS2)
                {
                    #region//Winsock 2.0 Start Hook

                    if (Operate.PacketConfig.Packet.HookWS2_Send)
                    {
                        lhWS2_Send = Install(WS2_32.ModuleName, "send", new WS2_32.DSend(WS2_32.SendHook));
                    }

                    if (Operate.PacketConfig.Packet.HookWS2_SendTo)
                    {
                        lhWS2_SendTo = Install(WS2_32.ModuleName, "sendto", new WS2_32.DSendTo(WS2_32.SendToHook));
                    }

                    if (Operate.PacketConfig.Packet.HookWS2_Recv)
                    {
                        lhWS2_Recv = Install(WS2_32.ModuleName, "recv", new WS2_32.Drecv(WS2_32.RecvHook));
                    }

                    if (Operate.PacketConfig.Packet.HookWS2_RecvFrom)
                    {
                        lhWS2_RecvFrom = Install(WS2_32.ModuleName, "recvfrom", new WS2_32.DRecvFrom(WS2_32.RecvFromHook));
                    }

                    if (Operate.PacketConfig.Packet.HookWSA_Send)
                    {
                        lhWSA_Send = Install(WS2_32.ModuleName, "WSASend", new WS2_32.DWSASend(WinSockHook.WSASend_Hook));
                    }

                    if (Operate.PacketConfig.Packet.HookWSA_SendTo)
                    {
                        lhWSA_SendTo = Install(WS2_32.ModuleName, "WSASendTo", new WS2_32.DWSASendTo(WinSockHook.WSASendTo_Hook));
                    }

                    if (Operate.PacketConfig.Packet.HookWSA_Recv)
                    {
                        lhWSA_Recv = Install(WS2_32.ModuleName, "WSARecv", new WS2_32.DWSARecv(WinSockHook.WSARecv_Hook));
                    }

                    if (Operate.PacketConfig.Packet.HookWSA_RecvFrom)
                    {
                        lhWSA_RecvFrom = Install(WS2_32.ModuleName, "WSARecvFrom", new WS2_32.DWSARecvFrom(WinSockHook.WSARecvFrom_Hook));
                    }

                    #endregion
                }

                if (Operate.PacketConfig.Packet.Support_MsWS)
                {
                    #region//Winsock Microsoft Start Hook

                    /*
                        ⚠️ 64 位目标上<b>不装</b> WSARecvEx 的钩子（2.1.9 起）。

                        它的真实签名是 int WSARecvEx(SOCKET, char*, int, int *flags)，第 4 个参数是指针，
                        而 NativeMethods/Mswsock.cs 把它导成了按值传的 SocketFlags（4 字节）：
                        x86 下指针本来就 4 字节、原样传回去正好是对的；x64 下指针被截成低 32 位，
                        钩子再把它传给原函数就是野指针 —— 目标进程当场 AV（实测 0xC0000005）。

                        要在 x64 上也抓它，得给 WSARecvEx 单开一个钩子体（flags 按 IntPtr 收），
                        不能改 Recv_Hook —— 那个 Flags 是 WS1/WS2 recv 共用的、按值传才对。
                        在那之前宁可少抓这一个入口，也不能把目标弄崩。
                    */
                    if (Operate.PacketConfig.Packet.HookWSA_Recv && Environment.Is64BitProcess)
                    {
                        LogHookError(nameof(StartHook), UI.T("Hook.SkipWSARecvEx64", "64 位目标不拦截 WSARecvEx（其余 WinSock 入口照常拦截）"));
                    }
                    else if (Operate.PacketConfig.Packet.HookWSA_Recv)
                    {
                        lhWSA_RecvEx = Install(Mswsock.ModuleName, "WSARecvEx", new Mswsock.DWSARecvEx(Mswsock.WSARecvExHook));
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHookError(nameof(StartHook), ex);
            }
        }

        #endregion

        #region//停止拦截

        public void StopHook()
        {
            var hooks = new[] { lhWS1_Send, lhWS1_SendTo, lhWS1_Recv, lhWS1_RecvFrom,
                lhWS2_Send, lhWS2_SendTo, lhWS2_Recv, lhWS2_RecvFrom,
                lhWSA_Send, lhWSA_SendTo, lhWSA_Recv, lhWSA_RecvFrom, lhWSA_RecvEx };
            foreach (var hook in hooks)
            {
                if (hook == null) { continue; }
                try { hook.Dispose(); }
                catch (Exception ex) { LogHookError(nameof(StopHook), ex); }
            }
            lhWS1_Send = lhWS1_SendTo = lhWS1_Recv = lhWS1_RecvFrom = null;
            lhWS2_Send = lhWS2_SendTo = lhWS2_Recv = lhWS2_RecvFrom = null;
            lhWSA_Send = lhWSA_SendTo = lhWSA_Recv = lhWSA_RecvFrom = lhWSA_RecvEx = null;
            InstalledCount = 0;
        }

        #endregion
        #region//退出

        #endregion                

        #region//Send_Hook

        public static unsafe Int32 Send_Hook(
            [In] Operate.PacketConfig.Packet.PacketType ptType,
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            int previousError = nativeError;
            nativeError = (int)WS2_32.WSAGetLastError();
            try { return Send_HookCore(ptType, Socket, lpBuffer, Length, Flags); }
            finally
            {
                int error = nativeError;
                nativeError = previousError;
                WSASetLastError(error);
            }
        }

        private static unsafe Int32 Send_HookCore(
            [In] Operate.PacketConfig.Packet.PacketType ptType,
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            Int32 res = 0;
            bool invoked = false;
            byte[] bRawBuffer = null;
            byte[] bNewBuffer = null;
            DateTime PacketTime = DateTime.UtcNow;

            try
            {
                Span<byte> bBufferSpan = new Span<byte>((byte*)lpBuffer, Length);
                bRawBuffer = bBufferSpan.ToArray();

                Operate.FilterConfig.Filter.FilterAction FilterAction = Operate.FilterConfig.List.FilterHookPacket(Socket, bRawBuffer, out bNewBuffer, ptType, new Operate.PacketConfig.Packet.SockAddr());

                ValidateSendReplacement(Socket, bRawBuffer, ref bNewBuffer, ref FilterAction);

                if (FilterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                {
                    invoked = true;
                    res = Length;
                }
                else
                {
                    invoked = true;
                    fixed (byte* pBuffer = bNewBuffer)
                    {
                        switch (ptType)
                        {
                            case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                                res = Capture(WSock32.send(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags));
                                break;

                            case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                                res = Capture(WS2_32.send(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags));
                                break;
                        }
                    }
                }

                if (invoked && res == bNewBuffer.Length) { res = Length; }
                ReportPacket(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, new Operate.PacketConfig.Packet.SockAddr(), PacketTime);
            }
            catch (Exception ex)
            {
                LogHookError(nameof(Send_Hook), ex);
                if (!invoked) { return ptType == Operate.PacketConfig.Packet.PacketType.WS1_Send ? Capture(WSock32.send(Socket, lpBuffer, Length, Flags)) : Capture(WS2_32.send(Socket, lpBuffer, Length, Flags)); }
            }

            return res;
        }

        #endregion

        #region//Recv_Hook

        public static unsafe Int32 Recv_Hook(
            [In] Operate.PacketConfig.Packet.PacketType ptType,
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            int previousError = nativeError;
            nativeError = (int)WS2_32.WSAGetLastError();
            try { return Recv_HookCore(ptType, Socket, lpBuffer, Length, Flags); }
            finally
            {
                int error = nativeError;
                nativeError = previousError;
                WSASetLastError(error);
            }
        }

        private static unsafe Int32 Recv_HookCore(
            [In] Operate.PacketConfig.Packet.PacketType ptType,
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            Int32 res = 0;

            try
            {
            Retry:
                switch (ptType)
                {
                    case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                        res = Capture(WSock32.recv(Socket, lpBuffer, Length, Flags));
                        break;

                    case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                        res = Capture(WS2_32.recv(Socket, lpBuffer, Length, Flags));
                        break;

                    case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
                        res = Capture(Mswsock.WSARecvEx(Socket, lpBuffer, Length, Flags));
                        break;
                }

                if (res > 0)
                {
                    byte[] bRawBuffer = null;
                    byte[] bNewBuffer = null;
                    DateTime PacketTime = DateTime.UtcNow;

                    Span<byte> bBufferSpan = new Span<byte>((byte*)lpBuffer, res);
                    bRawBuffer = bBufferSpan.ToArray();

                    Operate.FilterConfig.Filter.FilterAction FilterAction = Operate.FilterConfig.List.FilterHookPacket(Socket, bRawBuffer, out bNewBuffer, ptType, new Operate.PacketConfig.Packet.SockAddr());

                    if (FilterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                    {
                        ReportPacket(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, new Operate.PacketConfig.Packet.SockAddr(), PacketTime);
                        if ((Flags & SocketFlags.Peek) != 0) { nativeError = (int)SocketError.WouldBlock; return -1; }
                        goto Retry;
                    }
                    else
                    {
                        res = Math.Min(bNewBuffer.Length, Length);
                        if (!ReferenceEquals(bRawBuffer, bNewBuffer)) bNewBuffer.AsSpan(0, res).CopyTo(new Span<byte>((byte*)lpBuffer, res));
                    }

                    ReportPacket(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, new Operate.PacketConfig.Packet.SockAddr(), PacketTime);
                }
            }
            catch (Exception ex)
            {
                LogHookError(nameof(Recv_Hook), ex);
            }

            return res;
        }

        #endregion

        #region//SendTo_Hook

        public static unsafe Int32 SendTo_Hook(
            [In] Operate.PacketConfig.Packet.PacketType ptType,
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] IntPtr To,
            [In] Int32 ToLen)
        {
            int previousError = nativeError;
            nativeError = (int)WS2_32.WSAGetLastError();
            try { return SendTo_HookCore(ptType, Socket, lpBuffer, Length, Flags, To, ToLen); }
            finally
            {
                int error = nativeError;
                nativeError = previousError;
                WSASetLastError(error);
            }
        }

        private static unsafe Int32 SendTo_HookCore(
            [In] Operate.PacketConfig.Packet.PacketType ptType,
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] IntPtr To,
            [In] Int32 ToLen)
        {
            Int32 res = 0;
            bool invoked = false;
            byte[] bRawBuffer = null;
            byte[] bNewBuffer = null;
            DateTime PacketTime = DateTime.UtcNow;

            try
            {
                Span<byte> bBufferSpan = new Span<byte>((byte*)lpBuffer, Length);
                bRawBuffer = bBufferSpan.ToArray();

                Operate.FilterConfig.Filter.FilterAction FilterAction = Operate.FilterConfig.List.FilterHookPacket(Socket, bRawBuffer, out bNewBuffer, ptType, ReadAddress(To, ToLen));

                ValidateSendReplacement(Socket, bRawBuffer, ref bNewBuffer, ref FilterAction);

                if (FilterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                {
                    invoked = true;
                    res = Length;
                }
                else
                {
                    invoked = true;
                    fixed (byte* pBuffer = bNewBuffer)
                    {
                        switch (ptType)
                        {
                            case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                                res = Capture(SendTo1(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags, To, ToLen));
                                break;

                            case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                                res = Capture(SendTo2(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags, To, ToLen));
                                break;
                        }
                    }
                }

                if (invoked && res == bNewBuffer.Length) { res = Length; }
                ReportPacket(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, ReadAddress(To, ToLen), PacketTime);
            }
            catch (Exception ex)
            {
                LogHookError(nameof(SendTo_Hook), ex);
                if (!invoked) { return ptType == Operate.PacketConfig.Packet.PacketType.WS1_SendTo ? Capture(SendTo1(Socket, lpBuffer, Length, Flags, To, ToLen)) : Capture(SendTo2(Socket, lpBuffer, Length, Flags, To, ToLen)); }
            }

            return res;
        }

        #endregion

        #region//RecvFrom_Hook

        public static unsafe Int32 RecvFrom_Hook(
            [In] Operate.PacketConfig.Packet.PacketType ptType,
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] IntPtr From,
            [In, Out, Optional] IntPtr FromLen)
        {
            int previousError = nativeError;
            nativeError = (int)WS2_32.WSAGetLastError();
            try { return RecvFrom_HookCore(ptType, Socket, lpBuffer, Length, Flags, From, FromLen); }
            finally
            {
                int error = nativeError;
                nativeError = previousError;
                WSASetLastError(error);
            }
        }

        private static unsafe Int32 RecvFrom_HookCore(
            [In] Operate.PacketConfig.Packet.PacketType ptType,
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] IntPtr From,
            [In, Out, Optional] IntPtr FromLen)
        {
            Int32 res = 0;

            try
            {
            Retry:
                switch (ptType)
                {
                    case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                        res = Capture(RecvFrom1(Socket, lpBuffer, Length, Flags, From, FromLen));
                        break;

                    case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
                        res = Capture(RecvFrom2(Socket, lpBuffer, Length, Flags, From, FromLen));
                        break;
                }

                if (res > 0)
                {
                    var address = ReadAddress(From, FromLen == IntPtr.Zero ? 0 : Marshal.ReadInt32(FromLen));
                    byte[] bNewBuffer = null;
                    byte[] bRawBuffer = null;
                    DateTime PacketTime = DateTime.UtcNow;

                    Span<byte> bBufferSpan = new Span<byte>((byte*)lpBuffer, res);
                    bRawBuffer = bBufferSpan.ToArray();

                    Operate.FilterConfig.Filter.FilterAction FilterAction = Operate.FilterConfig.List.FilterHookPacket(Socket, bRawBuffer, out bNewBuffer, ptType, address);

                    if (FilterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                    {
                        ReportPacket(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, address, PacketTime);
                        if ((Flags & SocketFlags.Peek) != 0) { nativeError = (int)SocketError.WouldBlock; return -1; }
                        goto Retry;
                    }
                    else
                    {
                        res = Math.Min(bNewBuffer.Length, Length);
                        if (!ReferenceEquals(bRawBuffer, bNewBuffer)) bNewBuffer.AsSpan(0, res).CopyTo(new Span<byte>((byte*)lpBuffer, res));
                    }

                    ReportPacket(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, address, PacketTime);
                }
            }
            catch (Exception ex)
            {
                LogHookError(nameof(RecvFrom_Hook), ex);
            }

            return res;
        }

        #endregion

        #region//WSASend_Hook

        private static unsafe SocketError SendSynchronousBuffers(
            int socket, IntPtr buffers, int count, IntPtr sent,
            Operate.PacketConfig.Packet.PacketType type,
            Operate.PacketConfig.Packet.SockAddr address,
            Func<IntPtr, int, IntPtr, SocketError> send)
        {
            if (buffers == IntPtr.Zero || count <= 0 || sent == IntPtr.Zero)
                return send(buffers, count, sent);

            byte[] raw;
            byte[] modified;
            Operate.FilterConfig.Filter.FilterAction action;
            try
            {
                var list = (Operate.PacketConfig.Packet.WSABUF*)buffers;
                int length = 0;
                for (int i = 0; i < count; i++) { length = checked(length + list[i].len); }
                if (length == 0) { return send(buffers, count, sent); }
                raw = new byte[length];
                int offset = 0;
                for (int i = 0; i < count; i++)
                {
                    new ReadOnlySpan<byte>((void*)list[i].buf, list[i].len).CopyTo(raw.AsSpan(offset));
                    offset += list[i].len;
                }
                action = Operate.FilterConfig.List.FilterHookPacket(socket,
                    raw, out modified, type, address);
            }
            catch (Exception ex)
            {
                LogHookError(nameof(SendSynchronousBuffers), ex);
                return send(buffers, count, sent);
            }

            ValidateSendReplacement(socket, raw, ref modified, ref action);
            SocketError result;
            if (action == Operate.FilterConfig.Filter.FilterAction.Intercept)
            {
                Marshal.WriteInt32(sent, raw.Length);
                result = SocketError.Success;
            }
            else
            {
                fixed (byte* data = modified)
                {
                    var buffer = new Operate.PacketConfig.Packet.WSABUF { buf = (IntPtr)data, len = modified.Length };
                    result = send((IntPtr)(&buffer), 1, sent);
                }
                if (result == SocketError.Success && Marshal.ReadInt32(sent) == modified.Length)
                    Marshal.WriteInt32(sent, raw.Length);
            }
            if (result == SocketError.Success)
            {
                try { ReportPacket(socket, raw, modified, Marshal.ReadInt32(sent), type, action, address, DateTime.UtcNow); }
                catch (Exception ex) { LogHookError(nameof(SendSynchronousBuffers), ex); }
            }
            return result;
        }

        public static unsafe SocketError WSASend_Hook(
            [In] Int32 socket,
            [In] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesSent,
            [In] SocketFlags flags,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            int previousError = nativeError;
            nativeError = (int)WS2_32.WSAGetLastError();
            try { return WSASend_HookCore(socket, lpWSABuffer, bufferCount, lpNumberOfBytesSent, flags, lpOverlapped, lpCompletionRoutine); }
            finally
            {
                int error = nativeError;
                nativeError = previousError;
                WSASetLastError(error);
            }
        }

        private static unsafe SocketError WSASend_HookCore(
            [In] Int32 socket,
            [In] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesSent,
            [In] SocketFlags flags,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            // Completion tracking is not implemented: never pin a temporary buffer,
            // modify caller storage, or fake a completion for overlapped sends.
            if (lpOverlapped != IntPtr.Zero || lpCompletionRoutine != IntPtr.Zero)
                return Capture(WS2_32.WSASend(socket, lpWSABuffer, bufferCount, lpNumberOfBytesSent, flags, lpOverlapped, lpCompletionRoutine));
            return SendSynchronousBuffers(socket, lpWSABuffer, bufferCount, lpNumberOfBytesSent,
                Operate.PacketConfig.Packet.PacketType.WSASend, new Operate.PacketConfig.Packet.SockAddr(),
                (b, n, sent) => Capture(WS2_32.WSASend(socket, b, n, sent, flags, IntPtr.Zero, IntPtr.Zero)));
        }

        #endregion
        #region//WSARecv_Hook

        public static unsafe SocketError WSARecv_Hook(
            [In] Int32 socket,
            [In, Out] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags flags,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            int previousError = nativeError;
            nativeError = (int)WS2_32.WSAGetLastError();
            try { return WSARecv_HookCore(socket, lpWSABuffer, bufferCount, lpNumberOfBytesRecvd, ref flags, lpOverlapped, lpCompletionRoutine); }
            finally
            {
                int error = nativeError;
                nativeError = previousError;
                WSASetLastError(error);
            }
        }

        private static unsafe SocketError WSARecv_HookCore(
            [In] Int32 socket,
            [In, Out] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags flags,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Operate.PacketConfig.Packet.PacketType packetType = Operate.PacketConfig.Packet.PacketType.WSARecv;
            SocketFlags inputFlags = flags;
        Retry:
            flags = inputFlags;
            SocketError res = Capture(WS2_32.WSARecv(socket, lpWSABuffer, bufferCount, lpNumberOfBytesRecvd, ref flags, lpOverlapped, lpCompletionRoutine));

            try
            {
                if (res == SocketError.Success && lpOverlapped == IntPtr.Zero && lpCompletionRoutine == IntPtr.Zero && lpNumberOfBytesRecvd != IntPtr.Zero)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    if (BytesRecvd > 0)
                    {
                        DateTime packetTime = DateTime.UtcNow;
                        Operate.PacketConfig.Packet.WSABUF* pWSABuffers = (Operate.PacketConfig.Packet.WSABUF*)lpWSABuffer;

                        if (bufferCount == 1)
                        {
                            #region//单缓存区

                            byte[] bRawBuffer = null;
                            byte[] bNewBuffer = null;

                            Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[0].buf, BytesRecvd);
                            bRawBuffer = bufferSpan.ToArray();

                            Operate.FilterConfig.Filter.FilterAction filterAction =
                                Operate.FilterConfig.List.FilterHookPacket(
                                    socket,
                                    bRawBuffer,
                                    out bNewBuffer,
                                    packetType,
                                    new Operate.PacketConfig.Packet.SockAddr());

                            if (filterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                            {
                                if ((inputFlags & SocketFlags.Peek) != 0) { nativeError = (int)SocketError.WouldBlock; return SocketError.SocketError; }
                                goto Retry;
                            }
                            int bytesToWrite = 0;
                            if (filterAction != Operate.FilterConfig.Filter.FilterAction.Intercept)
                            {
                                bytesToWrite = Math.Min(bNewBuffer.Length, BytesRecvd);
                                if (!ReferenceEquals(bRawBuffer, bNewBuffer)) bNewBuffer.AsSpan(0, bytesToWrite).CopyTo(bufferSpan);
                            }

                            Marshal.WriteInt32(lpNumberOfBytesRecvd, bytesToWrite);

                            ReportPacket(
                                socket,
                                bRawBuffer,
                                bNewBuffer,
                                bytesToWrite,
                                packetType,
                                filterAction,
                                new Operate.PacketConfig.Packet.SockAddr(),
                                packetTime);

                            #endregion
                        }
                        else
                        {
                            #region//多缓存区

                            int remainingBytes = BytesRecvd;
                            int[] bufferBytes = new int[bufferCount];

                            for (int i = 0; i < bufferCount && remainingBytes > 0; i++)
                            {
                                int bufferSize = pWSABuffers[i].len;
                                bufferBytes[i] = Math.Min(bufferSize, remainingBytes);
                                remainingBytes -= bufferBytes[i];
                            }

                            byte[] bRawBuffer = new byte[BytesRecvd];
                            int offset = 0;
                            for (int i = 0; i < bufferCount; i++)
                            {
                                if (bufferBytes[i] > 0)
                                {
                                    Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[i].buf, bufferBytes[i]);
                                    bufferSpan.CopyTo(new Span<byte>(bRawBuffer, offset, bufferBytes[i]));
                                    offset += bufferBytes[i];
                                }
                            }

                            byte[] bNewBuffer = null;
                            Operate.FilterConfig.Filter.FilterAction filterAction =
                                Operate.FilterConfig.List.FilterHookPacket(
                                    socket,
                                    bRawBuffer,
                                    out bNewBuffer,
                                    packetType,
                                    new Operate.PacketConfig.Packet.SockAddr());

                            if (filterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                            {
                                if ((inputFlags & SocketFlags.Peek) != 0) { nativeError = (int)SocketError.WouldBlock; return SocketError.SocketError; }
                                goto Retry;
                            }
                            int bytesToWrite = 0;
                            if (filterAction != Operate.FilterConfig.Filter.FilterAction.Intercept && bNewBuffer != null)
                            {
                                remainingBytes = Math.Min(bNewBuffer.Length, BytesRecvd);
                                bytesToWrite = remainingBytes;

                                for (int i = 0; i < bufferCount && remainingBytes > 0; i++)
                                {
                                    int copyLength = Math.Min(pWSABuffers[i].len, remainingBytes);
                                    if (copyLength > 0)
                                    {
                                        Span<byte> destSpan = new Span<byte>((byte*)pWSABuffers[i].buf, copyLength);
                                        if (!ReferenceEquals(bRawBuffer, bNewBuffer)) bNewBuffer.AsSpan(bytesToWrite - remainingBytes, copyLength).CopyTo(destSpan);
                                        remainingBytes -= copyLength;
                                    }
                                }
                            }

                            Marshal.WriteInt32(lpNumberOfBytesRecvd, bytesToWrite);

                            ReportPacket(
                                socket,
                                bRawBuffer,
                                bNewBuffer,
                                bytesToWrite,
                                packetType,
                                filterAction,
                                new Operate.PacketConfig.Packet.SockAddr(),
                                packetTime);

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHookError(nameof(WSARecv_Hook), ex);
            }

            return res;
        }

        #endregion

        #region//WSASendTo_Hook  

        public static unsafe SocketError WSASendTo_Hook(
            [In] Int32 socket,
            [In] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesSent,
            [In] SocketFlags flags,
            [In] IntPtr To,
            [In] Int32 lpToLen,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            int previousError = nativeError;
            nativeError = (int)WS2_32.WSAGetLastError();
            try { return WSASendTo_HookCore(socket, lpWSABuffer, bufferCount, lpNumberOfBytesSent, flags, To, lpToLen, lpOverlapped, lpCompletionRoutine); }
            finally
            {
                int error = nativeError;
                nativeError = previousError;
                WSASetLastError(error);
            }
        }

        private static unsafe SocketError WSASendTo_HookCore(
            [In] Int32 socket,
            [In] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesSent,
            [In] SocketFlags flags,
            [In] IntPtr To,
            [In] Int32 lpToLen,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            if (lpOverlapped != IntPtr.Zero || lpCompletionRoutine != IntPtr.Zero)
                return Capture(SendToWsa(socket, lpWSABuffer, bufferCount, lpNumberOfBytesSent, flags, To, lpToLen, lpOverlapped, lpCompletionRoutine));
            var destination = ReadAddress(To, lpToLen);
            return SendSynchronousBuffers(socket, lpWSABuffer, bufferCount, lpNumberOfBytesSent,
                Operate.PacketConfig.Packet.PacketType.WSASendTo, destination,
                (b, n, sent) => Capture(SendToWsa(socket, b, n, sent, flags, To, lpToLen, IntPtr.Zero, IntPtr.Zero)));
        }

        #endregion
        #region//WSARecvFrom_Hook

        public static unsafe SocketError WSARecvFrom_Hook(
            [In] Int32 socket,
            [In, Out] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags flags,
            [In, Out] IntPtr from,
            [In, Out] IntPtr lpFromlen,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            int previousError = nativeError;
            nativeError = (int)WS2_32.WSAGetLastError();
            try { return WSARecvFrom_HookCore(socket, lpWSABuffer, bufferCount, lpNumberOfBytesRecvd, ref flags, from, lpFromlen, lpOverlapped, lpCompletionRoutine); }
            finally
            {
                int error = nativeError;
                nativeError = previousError;
                WSASetLastError(error);
            }
        }

        private static unsafe SocketError WSARecvFrom_HookCore(
            [In] Int32 socket,
            [In, Out] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags flags,
            [In, Out] IntPtr from,
            [In, Out] IntPtr lpFromlen,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Operate.PacketConfig.Packet.PacketType packetType = Operate.PacketConfig.Packet.PacketType.WSARecvFrom;
            SocketFlags inputFlags = flags;
        Retry:
            flags = inputFlags;
            SocketError res = Capture(RecvFromWsa(socket, lpWSABuffer, bufferCount, lpNumberOfBytesRecvd, ref flags, from, lpFromlen, lpOverlapped, lpCompletionRoutine));

            try
            {
                if (res == SocketError.Success && lpOverlapped == IntPtr.Zero && lpCompletionRoutine == IntPtr.Zero && lpNumberOfBytesRecvd != IntPtr.Zero)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    if (BytesRecvd > 0)
                    {
                        var address = ReadAddress(from, lpFromlen == IntPtr.Zero ? 0 : Marshal.ReadInt32(lpFromlen));
                        DateTime packetTime = DateTime.UtcNow;
                        Operate.PacketConfig.Packet.WSABUF* pWSABuffers = (Operate.PacketConfig.Packet.WSABUF*)lpWSABuffer;

                        if (bufferCount == 1)
                        {
                            #region//单缓存区

                            byte[] bRawBuffer = null;
                            byte[] bNewBuffer = null;

                            Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[0].buf, BytesRecvd);
                            bRawBuffer = bufferSpan.ToArray();

                            Operate.FilterConfig.Filter.FilterAction filterAction =
                                Operate.FilterConfig.List.FilterHookPacket(
                                    socket,
                                    bRawBuffer,
                                    out bNewBuffer,
                                    packetType,
                                    address);

                            if (filterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                            {
                                if ((inputFlags & SocketFlags.Peek) != 0) { nativeError = (int)SocketError.WouldBlock; return SocketError.SocketError; }
                                goto Retry;
                            }
                            int bytesToWrite = 0;
                            if (filterAction != Operate.FilterConfig.Filter.FilterAction.Intercept)
                            {
                                bytesToWrite = Math.Min(bNewBuffer.Length, BytesRecvd);
                                if (!ReferenceEquals(bRawBuffer, bNewBuffer)) bNewBuffer.AsSpan(0, bytesToWrite).CopyTo(bufferSpan);
                            }

                            Marshal.WriteInt32(lpNumberOfBytesRecvd, bytesToWrite);

                            ReportPacket(
                                socket,
                                bRawBuffer,
                                bNewBuffer,
                                bytesToWrite,
                                packetType,
                                filterAction,
                                address,
                                packetTime);

                            #endregion
                        }
                        else
                        {
                            #region//多缓存区

                            int remainingBytes = BytesRecvd;
                            int[] bufferBytes = new int[bufferCount];

                            for (int i = 0; i < bufferCount && remainingBytes > 0; i++)
                            {
                                int bufferSize = pWSABuffers[i].len;
                                bufferBytes[i] = Math.Min(bufferSize, remainingBytes);
                                remainingBytes -= bufferBytes[i];
                            }

                            byte[] bRawBuffer = new byte[BytesRecvd];
                            int offset = 0;
                            for (int i = 0; i < bufferCount; i++)
                            {
                                if (bufferBytes[i] > 0)
                                {
                                    Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[i].buf, bufferBytes[i]);
                                    bufferSpan.CopyTo(new Span<byte>(bRawBuffer, offset, bufferBytes[i]));
                                    offset += bufferBytes[i];
                                }
                            }

                            byte[] bNewBuffer = null;
                            Operate.FilterConfig.Filter.FilterAction filterAction =
                                Operate.FilterConfig.List.FilterHookPacket(
                                    socket,
                                    bRawBuffer,
                                    out bNewBuffer,
                                    packetType,
                                    address);

                            if (filterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                            {
                                if ((inputFlags & SocketFlags.Peek) != 0) { nativeError = (int)SocketError.WouldBlock; return SocketError.SocketError; }
                                goto Retry;
                            }
                            int bytesToWrite = 0;
                            if (filterAction != Operate.FilterConfig.Filter.FilterAction.Intercept && bNewBuffer != null)
                            {
                                remainingBytes = Math.Min(bNewBuffer.Length, BytesRecvd);
                                bytesToWrite = remainingBytes;

                                for (int i = 0; i < bufferCount && remainingBytes > 0; i++)
                                {
                                    int copyLength = Math.Min(pWSABuffers[i].len, remainingBytes);
                                    if (copyLength > 0)
                                    {
                                        Span<byte> destSpan = new Span<byte>((byte*)pWSABuffers[i].buf, copyLength);
                                        if (!ReferenceEquals(bRawBuffer, bNewBuffer)) bNewBuffer.AsSpan(bytesToWrite - remainingBytes, copyLength).CopyTo(destSpan);
                                        remainingBytes -= copyLength;
                                    }
                                }
                            }

                            Marshal.WriteInt32(lpNumberOfBytesRecvd, bytesToWrite);

                            ReportPacket(
                                socket,
                                bRawBuffer,
                                bNewBuffer,
                                bytesToWrite,
                                packetType,
                                filterAction,
                                address,
                                packetTime);

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHookError(nameof(WSARecvFrom_Hook), ex);
            }

            return res;
        }

        #endregion
    }
}
