using EasyHook;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WPELibrary.Lib.NativeMethods;

namespace WPELibrary.Lib
{
    public class WinSockHook : IEntryPoint
    {        
        private LocalHook lhWS1_Send, lhWS1_SendTo, lhWS1_Recv, lhWS1_RecvFrom;
        private LocalHook lhWS2_Send, lhWS2_SendTo, lhWS2_Recv, lhWS2_RecvFrom;
        private LocalHook lhWSA_Send, lhWSA_SendTo, lhWSA_Recv, lhWSA_RecvFrom;
        private LocalHook lhWSA_RecvEx;

        #region//EasyHook

        public WinSockHook()
        {
            //
        }

        public WinSockHook(RemoteHooking.IContext InContext, string ChannelName)
        {
            //
        }

        public unsafe void Run(RemoteHooking.IContext InContext, string ChannelName)
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }

                Application.EnableVisualStyles();                
                Application.SetCompatibleTextRenderingDefault(false);                
                Application.Run(new Socket_Form());                
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
                if (Socket_Cache.SocketPacket.Support_WS1)
                {
                    #region//Winsock 1.1 Start Hook

                    if (Socket_Cache.SocketPacket.HookWS1_Send)
                    {
                        lhWS1_Send = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "send"), new WSock32.DSend(WSock32.SendHook), this);
                        lhWS1_Send.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS1_SendTo)
                    {
                        lhWS1_SendTo = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "sendto"), new WSock32.DSendTo(WSock32.SendToHook), this);
                        lhWS1_SendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS1_Recv)
                    {
                        lhWS1_Recv = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "recv"), new WSock32.Drecv(WSock32.RecvHook), this);
                        lhWS1_Recv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS1_RecvFrom)
                    {
                        lhWS1_RecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WSock32.ModuleName, "recvfrom"), new WSock32.DRecvFrom(WSock32.RecvFromHook), this);
                        lhWS1_RecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    #endregion
                }

                if (Socket_Cache.SocketPacket.Support_WS2)
                {
                    #region//Winsock 2.0 Start Hook

                    if (Socket_Cache.SocketPacket.HookWS2_Send)
                    {
                        lhWS2_Send = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "send"), new WS2_32.DSend(WS2_32.SendHook), this);
                        lhWS2_Send.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS2_SendTo)
                    {
                        lhWS2_SendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "sendto"), new WS2_32.DSendTo(WS2_32.SendToHook), this);
                        lhWS2_SendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS2_Recv)
                    {
                        lhWS2_Recv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recv"), new WS2_32.Drecv(WS2_32.RecvHook), this);
                        lhWS2_Recv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWS2_RecvFrom)
                    {
                        lhWS2_RecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "recvfrom"), new WS2_32.DRecvFrom(WS2_32.RecvFromHook), this);
                        lhWS2_RecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSA_Send)
                    {
                        lhWSA_Send = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASend"), new WS2_32.DWSASend(WSASend_Hook), this);
                        lhWSA_Send.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSA_SendTo)
                    {
                        lhWSA_SendTo = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSASendTo"), new WS2_32.DWSASendTo(WSASendTo_Hook), this);
                        lhWSA_SendTo.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSA_Recv)
                    {
                        lhWSA_Recv = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecv"), new WS2_32.DWSARecv(WSARecv_Hook), this);
                        lhWSA_Recv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    if (Socket_Cache.SocketPacket.HookWSA_RecvFrom)
                    {
                        lhWSA_RecvFrom = LocalHook.Create(LocalHook.GetProcAddress(WS2_32.ModuleName, "WSARecvFrom"), new WS2_32.DWSARecvFrom(WSARecvFrom_Hook), this);
                        lhWSA_RecvFrom.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                    }

                    #endregion
                }

                if (Socket_Cache.SocketPacket.Support_MsWS)
                {
                    #region//Winsock Microsoft Start Hook

                    if (Socket_Cache.SocketPacket.HookWSA_Recv)
                    {
                        lhWSA_RecvEx = LocalHook.Create(LocalHook.GetProcAddress(Mswsock.ModuleName, "WSARecvEx"), new Mswsock.DWSARecvEx(Mswsock.WSARecvExHook), this);
                        lhWSA_RecvEx.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
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
                if (Socket_Cache.SocketPacket.Support_WS1)
                {
                    #region//Winsock 1.1 Stop Hook

                    if (lhWS1_Send != null)
                    {
                        lhWS1_Send.Dispose();
                    }

                    if (lhWS1_SendTo != null)
                    {
                        lhWS1_SendTo.Dispose();
                    }

                    if (lhWS1_Recv != null)
                    {
                        lhWS1_Recv.Dispose();
                    }

                    if (lhWS1_RecvFrom != null)
                    {
                        lhWS1_RecvFrom.Dispose();
                    }

                    #endregion
                }

                if (Socket_Cache.SocketPacket.Support_WS2)
                {
                    #region//Winsock 2.0 Stop Hook

                    if (lhWS2_Send != null)
                    {
                        lhWS2_Send.Dispose();
                    }

                    if (lhWS2_SendTo != null)
                    {
                        lhWS2_SendTo.Dispose();
                    }

                    if (lhWS2_Recv != null)
                    {
                        lhWS2_Recv.Dispose();
                    }

                    if (lhWS2_RecvFrom != null)
                    {
                        lhWS2_RecvFrom.Dispose();
                    }

                    if (lhWSA_Send != null)
                    {
                        lhWSA_Send.Dispose();
                    }

                    if (lhWSA_SendTo != null)
                    {
                        lhWSA_SendTo.Dispose();
                    }

                    if (lhWSA_Recv != null)
                    {
                        lhWSA_Recv.Dispose();
                    }

                    if (lhWSA_RecvFrom != null)
                    {
                        lhWSA_RecvFrom.Dispose();
                    }

                    #endregion
                }

                if (Socket_Cache.SocketPacket.Support_MsWS)
                {
                    #region//Winsock Microsoft Stop Hook

                    if (lhWSA_RecvEx != null)
                    {
                        lhWSA_RecvEx.Dispose();
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

        #region//Send_Hook

        public static unsafe Int32 Send_Hook(
            [In] Socket_Cache.SocketPacket.PacketType ptType,
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            Int32 res = 0;
            byte[] bRawBuffer = null;
            byte[] bNewBuffer = null;
            DateTime PacketTime = DateTime.Now;

            try
            {
                Span<byte> bBufferSpan = new Span<byte>((byte*)lpBuffer, Length);
                bRawBuffer = bBufferSpan.ToArray();

                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(Socket, bBufferSpan, out bNewBuffer, ptType, new Socket_Cache.SocketPacket.SockAddr());

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = Length;
                }
                else
                {
                    fixed (byte* pBuffer = bNewBuffer)
                    {
                        switch (ptType)
                        {
                            case Socket_Cache.SocketPacket.PacketType.WS1_Send:
                                res = WSock32.send(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags);
                                break;

                            case Socket_Cache.SocketPacket.PacketType.WS2_Send:
                                res = WS2_32.send(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags);
                                break;
                        }
                    }
                }

                _ = Socket_Operation.ProcessingHookResultAsync(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr(), PacketTime);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//Recv_Hook

        public static unsafe Int32 Recv_Hook(
            [In] Socket_Cache.SocketPacket.PacketType ptType,
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags)
        {
            Int32 res = 0;

            try
            {
                switch (ptType)
                {
                    case Socket_Cache.SocketPacket.PacketType.WS1_Recv:
                        res = WSock32.recv(Socket, lpBuffer, Length, Flags);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WS2_Recv:
                        res = WS2_32.recv(Socket, lpBuffer, Length, Flags);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WSARecvEx:
                        res = Mswsock.WSARecvEx(Socket, lpBuffer, Length, Flags);
                        break;
                }

                if (res > 0)
                {
                    byte[] bRawBuffer = null;
                    byte[] bNewBuffer = null;
                    DateTime PacketTime = DateTime.Now;

                    Span<byte> bBufferSpan = new Span<byte>((byte*)lpBuffer, res);
                    bRawBuffer = bBufferSpan.ToArray();

                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(Socket, bBufferSpan, out bNewBuffer, ptType, new Socket_Cache.SocketPacket.SockAddr());

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        res = Math.Min(bNewBuffer.Length, res);
                        new Span<byte>(bNewBuffer).CopyTo(new Span<byte>((byte*)lpBuffer, res));
                    }

                    _ = Socket_Operation.ProcessingHookResultAsync(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, new Socket_Cache.SocketPacket.SockAddr(), PacketTime);
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }       

        #endregion

        #region//SendTo_Hook

        public static unsafe Int32 SendTo_Hook(
            [In] Socket_Cache.SocketPacket.PacketType ptType,
            [In] Int32 Socket,
            [In] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] Int32 ToLen)
        {
            Int32 res = 0;
            byte[] bRawBuffer = null;
            byte[] bNewBuffer = null;
            DateTime PacketTime = DateTime.Now;

            try
            {
                Span<byte> bBufferSpan = new Span<byte>((byte*)lpBuffer, Length);
                bRawBuffer = bBufferSpan.ToArray();

                Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(Socket, bBufferSpan, out bNewBuffer, ptType, To);

                if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                {
                    res = Length;
                }
                else
                {
                    fixed (byte* pBuffer = bNewBuffer)
                    {
                        switch (ptType)
                        {
                            case Socket_Cache.SocketPacket.PacketType.WS1_SendTo:
                                res = WSock32.sendto(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags, ref To, ToLen);
                                break;

                            case Socket_Cache.SocketPacket.PacketType.WS2_SendTo:
                                res = WS2_32.sendto(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags, ref To, ToLen);
                                break;
                        }
                    }
                }

                _ = Socket_Operation.ProcessingHookResultAsync(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, To, PacketTime);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }        

        #endregion

        #region//RecvFrom_Hook

        public static unsafe Int32 RecvFrom_Hook(
            [In] Socket_Cache.SocketPacket.PacketType ptType,
            [In] Int32 Socket,
            [Out] IntPtr lpBuffer,
            [In] Int32 Length,
            [In] SocketFlags Flags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr From,
            [In, Out, Optional] IntPtr FromLen)
        {
            Int32 res = 0;

            try
            {
                switch (ptType)
                {
                    case Socket_Cache.SocketPacket.PacketType.WS1_RecvFrom:
                        res = WSock32.recvfrom(Socket, lpBuffer, Length, Flags, ref From, FromLen);
                        break;

                    case Socket_Cache.SocketPacket.PacketType.WS2_RecvFrom:
                        res = WS2_32.recvfrom(Socket, lpBuffer, Length, Flags, ref From, FromLen);
                        break;
                }

                if (res > 0)
                {
                    byte[] bNewBuffer = null;
                    byte[] bRawBuffer = null;
                    DateTime PacketTime = DateTime.Now;

                    Span<byte> bBufferSpan = new Span<byte>((byte*)lpBuffer, res);
                    bRawBuffer = bBufferSpan.ToArray();

                    Socket_Cache.Filter.FilterAction FilterAction = Socket_Cache.FilterList.DoFilterList(Socket, bBufferSpan, out bNewBuffer, ptType, From);

                    if (FilterAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        res = Math.Min(bNewBuffer.Length, res);
                        new Span<byte>(bNewBuffer).CopyTo(new Span<byte>((byte*)lpBuffer, res));
                    }

                    _ = Socket_Operation.ProcessingHookResultAsync(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, From, PacketTime);
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSASend_Hook

        public static unsafe SocketError WSASend_Hook(
            [In] Int32 socket,
            [In] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesSent,
            [In] SocketFlags flags,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            SocketError res = SocketError.SocketError;
            Socket_Cache.SocketPacket.PacketType packetType = Socket_Cache.SocketPacket.PacketType.WSASend;            

            try
            {
                DateTime packetTime = DateTime.Now;
                Socket_Cache.SocketPacket.WSABUF* pWSABuffers = (Socket_Cache.SocketPacket.WSABUF*)lpWSABuffer;

                if (bufferCount == 1)
                {
                    #region//单缓存区

                    int BytesSent = pWSABuffers[0].len;
                    if (BytesSent > 0)
                    {
                        byte[] bRawBuffer = null;
                        byte[] bNewBuffer = null;

                        Span<byte> bBufferSpan = new Span<byte>((byte*)pWSABuffers[0].buf, BytesSent);                        
                        bRawBuffer = bBufferSpan.ToArray();

                        Socket_Cache.Filter.FilterAction filterAction =
                        Socket_Cache.FilterList.DoFilterList(
                            socket,
                            bBufferSpan,
                            out bNewBuffer,
                            packetType,
                            new Socket_Cache.SocketPacket.SockAddr());

                        if (filterAction == Socket_Cache.Filter.FilterAction.Intercept)
                        {
                            Marshal.WriteInt32(lpNumberOfBytesSent, BytesSent);
                            res = SocketError.Success;
                        }
                        else
                        {
                            BytesSent = Math.Min(bNewBuffer.Length, BytesSent);
                            bNewBuffer.AsSpan(0, BytesSent).CopyTo(bBufferSpan);

                            int WSABufferLen = pWSABuffers[0].len;
                            pWSABuffers[0].len = BytesSent;

                            res = WS2_32.WSASend(
                                socket,
                                lpWSABuffer,
                                bufferCount,
                                lpNumberOfBytesSent,
                                flags,
                                lpOverlapped,
                                lpCompletionRoutine);

                            pWSABuffers[0].len = WSABufferLen;
                        }

                        BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

                        _ = Socket_Operation.ProcessingHookResultAsync(
                       socket,
                       bRawBuffer,
                       bNewBuffer,
                       BytesSent,
                       packetType,
                       filterAction,
                       new Socket_Cache.SocketPacket.SockAddr(),
                       packetTime);
                    }

                    #endregion
                }
                else
                {
                    #region//多缓存区

                    int totalBytes = 0;
                    for (int i = 0; i < bufferCount; i++)
                    {
                        totalBytes += pWSABuffers[i].len;
                    }

                    if (totalBytes > 0)
                    {
                        byte[] bRawBuffer = new byte[totalBytes];
                        byte[] bNewBuffer = null;

                        int offset = 0;
                        for (int i = 0; i < bufferCount; i++)
                        {
                            if (pWSABuffers[i].len > 0)
                            {
                                Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[i].buf, pWSABuffers[i].len);
                                bufferSpan.CopyTo(new Span<byte>(bRawBuffer, offset, pWSABuffers[i].len));
                                offset += pWSABuffers[i].len;
                            }
                        }

                        Socket_Cache.Filter.FilterAction filterAction =
                            Socket_Cache.FilterList.DoFilterList(
                                socket,
                                bRawBuffer.AsSpan(),
                                out bNewBuffer,
                                packetType,
                                new Socket_Cache.SocketPacket.SockAddr());

                        if (filterAction == Socket_Cache.Filter.FilterAction.Intercept)
                        {
                            Marshal.WriteInt32(lpNumberOfBytesSent, totalBytes);
                            res = SocketError.Success;
                        }
                        else if (bNewBuffer != null && bNewBuffer.Length > 0)
                        {
                            int remainingBytes = Math.Min(bNewBuffer.Length, totalBytes);
                            int bufferIndex = 0;
                            int bytesCopied = 0;

                            int[] originalLengths = new int[bufferCount];
                            for (int i = 0; i < bufferCount; i++)
                            {
                                originalLengths[i] = pWSABuffers[i].len;
                            }

                            while (remainingBytes > 0 && bufferIndex < bufferCount)
                            {
                                int copyLength = Math.Min(pWSABuffers[bufferIndex].len, remainingBytes);
                                if (copyLength > 0)
                                {
                                    Span<byte> destSpan = new Span<byte>((byte*)pWSABuffers[bufferIndex].buf, pWSABuffers[bufferIndex].len);
                                    bNewBuffer.AsSpan(bytesCopied, copyLength).CopyTo(destSpan);
                                    pWSABuffers[bufferIndex].len = copyLength;
                                    bytesCopied += copyLength;
                                    remainingBytes -= copyLength;
                                }

                                bufferIndex++;
                            }

                            res = WS2_32.WSASend(
                                socket,
                                lpWSABuffer,
                                bufferCount,
                                lpNumberOfBytesSent,
                                flags,
                                lpOverlapped,
                                lpCompletionRoutine);

                            for (int i = 0; i < bufferCount; i++)
                            {
                                pWSABuffers[i].len = originalLengths[i];
                            }
                        }
                        else
                        {
                            res = WS2_32.WSASend(
                                socket,
                                lpWSABuffer,
                                bufferCount,
                                lpNumberOfBytesSent,
                                flags,
                                lpOverlapped,
                                lpCompletionRoutine);
                        }

                        int bytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

                        _ = Socket_Operation.ProcessingHookResultAsync(
                            socket,
                            bRawBuffer,
                            bNewBuffer,
                            bytesSent,
                            packetType,
                            filterAction,
                            new Socket_Cache.SocketPacket.SockAddr(),
                            packetTime);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
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
            Socket_Cache.SocketPacket.PacketType packetType = Socket_Cache.SocketPacket.PacketType.WSARecv;
            SocketError res = WS2_32.WSARecv(socket, lpWSABuffer, bufferCount, lpNumberOfBytesRecvd, ref flags, lpOverlapped, lpCompletionRoutine);

            try
            {
                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    if (BytesRecvd > 0)
                    {
                        DateTime packetTime = DateTime.Now;
                        Socket_Cache.SocketPacket.WSABUF* pWSABuffers = (Socket_Cache.SocketPacket.WSABUF*)lpWSABuffer;

                        if (bufferCount == 1)
                        {
                            #region//单缓存区

                            byte[] bRawBuffer = null;
                            byte[] bNewBuffer = null;

                            Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[0].buf, BytesRecvd);
                            bRawBuffer = bufferSpan.ToArray();

                            Socket_Cache.Filter.FilterAction filterAction =
                                Socket_Cache.FilterList.DoFilterList(
                                    socket,
                                    bufferSpan,
                                    out bNewBuffer,
                                    packetType,
                                    new Socket_Cache.SocketPacket.SockAddr());

                            int bytesToWrite = 0;
                            if (filterAction != Socket_Cache.Filter.FilterAction.Intercept)
                            {
                                bytesToWrite = Math.Min(bNewBuffer.Length, BytesRecvd);
                                bNewBuffer.AsSpan(0, bytesToWrite).CopyTo(bufferSpan);
                            }

                            Marshal.WriteInt32(lpNumberOfBytesRecvd, bytesToWrite);

                            _ = Socket_Operation.ProcessingHookResultAsync(
                                socket,
                                bRawBuffer,
                                bNewBuffer,
                                bytesToWrite,
                                packetType,
                                filterAction,
                                new Socket_Cache.SocketPacket.SockAddr(),
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
                            Socket_Cache.Filter.FilterAction filterAction =
                                Socket_Cache.FilterList.DoFilterList(
                                    socket,
                                    bRawBuffer.AsSpan(),
                                    out bNewBuffer,
                                    packetType,
                                    new Socket_Cache.SocketPacket.SockAddr());

                            int bytesToWrite = 0;
                            if (filterAction != Socket_Cache.Filter.FilterAction.Intercept && bNewBuffer != null)
                            {
                                remainingBytes = Math.Min(bNewBuffer.Length, BytesRecvd);
                                bytesToWrite = remainingBytes;

                                for (int i = 0; i < bufferCount && remainingBytes > 0; i++)
                                {
                                    int copyLength = Math.Min(pWSABuffers[i].len, remainingBytes);
                                    if (copyLength > 0)
                                    {
                                        Span<byte> destSpan = new Span<byte>((byte*)pWSABuffers[i].buf, copyLength);
                                        bNewBuffer.AsSpan(BytesRecvd - remainingBytes, copyLength).CopyTo(destSpan);
                                        remainingBytes -= copyLength;
                                    }
                                }
                            }

                            Marshal.WriteInt32(lpNumberOfBytesRecvd, bytesToWrite);

                            _ = Socket_Operation.ProcessingHookResultAsync(
                                socket,
                                bRawBuffer,
                                bNewBuffer,
                                bytesToWrite,
                                packetType,
                                filterAction,
                                new Socket_Cache.SocketPacket.SockAddr(),
                                packetTime);

                            #endregion
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

        #region//WSASendTo_Hook  

        public static unsafe SocketError WSASendTo_Hook(
            [In] Int32 socket,
            [In] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesSent,
            [In] SocketFlags flags,
            [In] ref Socket_Cache.SocketPacket.SockAddr To,
            [In] IntPtr lpToLen,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            SocketError res = SocketError.SocketError;
            Socket_Cache.SocketPacket.PacketType packetType = Socket_Cache.SocketPacket.PacketType.WSASendTo;

            try
            {
                DateTime packetTime = DateTime.Now;
                Socket_Cache.SocketPacket.WSABUF* pWSABuffers = (Socket_Cache.SocketPacket.WSABUF*)lpWSABuffer;

                if (bufferCount == 1)
                {
                    #region//单缓存区

                    int BytesSent = pWSABuffers[0].len;
                    if (BytesSent > 0)
                    {
                        byte[] bRawBuffer = null;
                        byte[] bNewBuffer = null;

                        Span<byte> bBufferSpan = new Span<byte>((byte*)pWSABuffers[0].buf, BytesSent);
                        bRawBuffer = bBufferSpan.ToArray();

                        Socket_Cache.Filter.FilterAction filterAction =
                        Socket_Cache.FilterList.DoFilterList(
                            socket,
                            bBufferSpan,
                            out bNewBuffer,
                            packetType,
                            To);

                        if (filterAction == Socket_Cache.Filter.FilterAction.Intercept)
                        {
                            Marshal.WriteInt32(lpNumberOfBytesSent, BytesSent);
                            res = SocketError.Success;
                        }
                        else
                        {
                            BytesSent = Math.Min(bNewBuffer.Length, BytesSent);
                            bNewBuffer.AsSpan(0, BytesSent).CopyTo(bBufferSpan);

                            int WSABufferLen = pWSABuffers[0].len;
                            pWSABuffers[0].len = BytesSent;

                            res = WS2_32.WSASendTo(
                                socket,
                                lpWSABuffer,
                                bufferCount,
                                lpNumberOfBytesSent,
                                flags,
                                ref To,
                                lpToLen,
                                lpOverlapped,
                                lpCompletionRoutine);

                            pWSABuffers[0].len = WSABufferLen;
                        }

                        BytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);

                        _ = Socket_Operation.ProcessingHookResultAsync(
                       socket,
                       bRawBuffer,
                       bNewBuffer,
                       BytesSent,
                       packetType,
                       filterAction,
                       To,
                       packetTime);
                    }

                    #endregion                    
                }
                else
                {
                    #region//多缓存区

                    int totalBytes = 0;
                    for (int i = 0; i < bufferCount; i++)
                    {
                        totalBytes += pWSABuffers[i].len;
                    }

                    if (totalBytes > 0)
                    {
                        byte[] bRawBuffer = new byte[totalBytes];
                        byte[] bNewBuffer = null;

                        int offset = 0;
                        for (int i = 0; i < bufferCount; i++)
                        {
                            if (pWSABuffers[i].len > 0)
                            {
                                Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[i].buf, pWSABuffers[i].len);
                                bufferSpan.CopyTo(new Span<byte>(bRawBuffer, offset, pWSABuffers[i].len));
                                offset += pWSABuffers[i].len;
                            }
                        }

                        Socket_Cache.Filter.FilterAction filterAction =
                            Socket_Cache.FilterList.DoFilterList(
                                socket,
                                bRawBuffer.AsSpan(),
                                out bNewBuffer,
                                packetType,
                                To);

                        if (filterAction == Socket_Cache.Filter.FilterAction.Intercept)
                        {
                            Marshal.WriteInt32(lpNumberOfBytesSent, totalBytes);
                            res = SocketError.Success;
                        }
                        else if (bNewBuffer != null && bNewBuffer.Length > 0)
                        {
                            int remainingBytes = Math.Min(bNewBuffer.Length, totalBytes);
                            int bufferIndex = 0;
                            int bytesCopied = 0;

                            int[] originalLengths = new int[bufferCount];
                            for (int i = 0; i < bufferCount; i++)
                            {
                                originalLengths[i] = pWSABuffers[i].len;
                            }

                            while (remainingBytes > 0 && bufferIndex < bufferCount)
                            {
                                int copyLength = Math.Min(pWSABuffers[bufferIndex].len, remainingBytes);
                                if (copyLength > 0)
                                {
                                    Span<byte> destSpan = new Span<byte>((byte*)pWSABuffers[bufferIndex].buf, pWSABuffers[bufferIndex].len);
                                    bNewBuffer.AsSpan(bytesCopied, copyLength).CopyTo(destSpan);
                                    pWSABuffers[bufferIndex].len = copyLength;
                                    bytesCopied += copyLength;
                                    remainingBytes -= copyLength;
                                }
                                bufferIndex++;
                            }

                            res = WS2_32.WSASendTo(
                                socket,
                                lpWSABuffer,
                                bufferCount,
                                lpNumberOfBytesSent,
                                flags,
                                ref To,
                                lpToLen,
                                lpOverlapped,
                                lpCompletionRoutine);

                            for (int i = 0; i < bufferCount; i++)
                            {
                                pWSABuffers[i].len = originalLengths[i];
                            }
                        }
                        else
                        {
                            res = WS2_32.WSASendTo(
                                socket,
                                lpWSABuffer,
                                bufferCount,
                                lpNumberOfBytesSent,
                                flags,
                                ref To,
                                lpToLen,
                                lpOverlapped,
                                lpCompletionRoutine);
                        }

                        int bytesSent = Marshal.ReadInt32(lpNumberOfBytesSent);
                        _ = Socket_Operation.ProcessingHookResultAsync(
                            socket,
                            bRawBuffer,
                            bNewBuffer,
                            bytesSent,
                            packetType,
                            filterAction,
                            To,
                            packetTime);
                    }                    

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion

        #region//WSARecvFrom_Hook

        public static unsafe SocketError WSARecvFrom_Hook(
            [In] Int32 socket,
            [In, Out] IntPtr lpWSABuffer,
            [In] Int32 bufferCount,
            [Out] IntPtr lpNumberOfBytesRecvd,
            [In, Out] ref SocketFlags flags,
            [In, Out] ref Socket_Cache.SocketPacket.SockAddr from,
            [In, Out] IntPtr lpFromlen,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Socket_Cache.SocketPacket.PacketType packetType = Socket_Cache.SocketPacket.PacketType.WSARecvFrom;
            SocketError res = WS2_32.WSARecvFrom(socket, lpWSABuffer, bufferCount, lpNumberOfBytesRecvd, ref flags, ref from, lpFromlen, lpOverlapped, lpCompletionRoutine);

            try
            {
                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    if (BytesRecvd > 0)
                    {  
                        DateTime packetTime = DateTime.Now;
                        Socket_Cache.SocketPacket.WSABUF* pWSABuffers = (Socket_Cache.SocketPacket.WSABUF*)lpWSABuffer;

                        if (bufferCount == 1)
                        {
                            #region//单缓存区

                            byte[] bRawBuffer = null;
                            byte[] bNewBuffer = null;

                            Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[0].buf, BytesRecvd);
                            bRawBuffer = bufferSpan.ToArray();

                            Socket_Cache.Filter.FilterAction filterAction =
                                Socket_Cache.FilterList.DoFilterList(
                                    socket,
                                    bufferSpan,
                                    out bNewBuffer,
                                    packetType,
                                    from);

                            int bytesToWrite = 0;
                            if (filterAction != Socket_Cache.Filter.FilterAction.Intercept)
                            {
                                bytesToWrite = Math.Min(bNewBuffer.Length, BytesRecvd);
                                bNewBuffer.AsSpan(0, bytesToWrite).CopyTo(bufferSpan);
                            }

                            Marshal.WriteInt32(lpNumberOfBytesRecvd, bytesToWrite);

                            _ = Socket_Operation.ProcessingHookResultAsync(
                                socket,
                                bRawBuffer,
                                bNewBuffer,
                                bytesToWrite,
                                packetType,
                                filterAction,
                                from,
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
                            Socket_Cache.Filter.FilterAction filterAction =
                                Socket_Cache.FilterList.DoFilterList(
                                    socket,
                                    bRawBuffer.AsSpan(),
                                    out bNewBuffer,
                                    packetType,
                                    from);

                            int bytesToWrite = 0;
                            if (filterAction != Socket_Cache.Filter.FilterAction.Intercept && bNewBuffer != null)
                            {
                                remainingBytes = Math.Min(bNewBuffer.Length, BytesRecvd);
                                bytesToWrite = remainingBytes;

                                for (int i = 0; i < bufferCount && remainingBytes > 0; i++)
                                {
                                    int copyLength = Math.Min(pWSABuffers[i].len, remainingBytes);
                                    if (copyLength > 0)
                                    {
                                        Span<byte> destSpan = new Span<byte>((byte*)pWSABuffers[i].buf, copyLength);
                                        bNewBuffer.AsSpan(BytesRecvd - remainingBytes, copyLength).CopyTo(destSpan);
                                        remainingBytes -= copyLength;
                                    }
                                }
                            }

                            Marshal.WriteInt32(lpNumberOfBytesRecvd, bytesToWrite);

                            _ = Socket_Operation.ProcessingHookResultAsync(
                                socket,
                                bRawBuffer,
                                bNewBuffer,
                                bytesToWrite,
                                packetType,
                                filterAction,
                                from,
                                packetTime);

                            #endregion
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
    }
}
