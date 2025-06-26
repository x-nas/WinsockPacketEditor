using System;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using WPE.Lib.NativeMethods;

namespace WPE.Lib
{
    public class WinSockHook
    {
        #region//Send_Hook

        public static unsafe Int32 Send_Hook(
            [In] Operate.PacketConfig.Packet.PacketType ptType,
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

                Operate.FilterConfig.Filter.FilterAction FilterAction = Operate.FilterConfig.List.DoFilterList(Socket, bBufferSpan, out bNewBuffer, ptType, new Operate.PacketConfig.Packet.SockAddr());

                if (FilterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                {
                    res = Length;
                }
                else
                {
                    fixed (byte* pBuffer = bNewBuffer)
                    {
                        switch (ptType)
                        {
                            case Operate.PacketConfig.Packet.PacketType.WS1_Send:
                                res = WSock32.send(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags);
                                break;

                            case Operate.PacketConfig.Packet.PacketType.WS2_Send:
                                res = WS2_32.send(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags);
                                break;
                        }
                    }
                }

                _ = Socket_Operation.ProcessingHookResultAsync(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, new Operate.PacketConfig.Packet.SockAddr(), PacketTime);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            Int32 res = 0;

            try
            {
                switch (ptType)
                {
                    case Operate.PacketConfig.Packet.PacketType.WS1_Recv:
                        res = WSock32.recv(Socket, lpBuffer, Length, Flags);
                        break;

                    case Operate.PacketConfig.Packet.PacketType.WS2_Recv:
                        res = WS2_32.recv(Socket, lpBuffer, Length, Flags);
                        break;

                    case Operate.PacketConfig.Packet.PacketType.WSARecvEx:
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

                    Operate.FilterConfig.Filter.FilterAction FilterAction = Operate.FilterConfig.List.DoFilterList(Socket, bBufferSpan, out bNewBuffer, ptType, new Operate.PacketConfig.Packet.SockAddr());

                    if (FilterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                    {
                        res = 0;
                    }
                    else
                    {
                        res = Math.Min(bNewBuffer.Length, res);
                        new Span<byte>(bNewBuffer).CopyTo(new Span<byte>((byte*)lpBuffer, res));
                    }

                    _ = Socket_Operation.ProcessingHookResultAsync(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, new Operate.PacketConfig.Packet.SockAddr(), PacketTime);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            [In] ref Operate.PacketConfig.Packet.SockAddr To,
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

                Operate.FilterConfig.Filter.FilterAction FilterAction = Operate.FilterConfig.List.DoFilterList(Socket, bBufferSpan, out bNewBuffer, ptType, To);

                if (FilterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
                {
                    res = Length;
                }
                else
                {
                    fixed (byte* pBuffer = bNewBuffer)
                    {
                        switch (ptType)
                        {
                            case Operate.PacketConfig.Packet.PacketType.WS1_SendTo:
                                res = WSock32.sendto(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags, ref To, ToLen);
                                break;

                            case Operate.PacketConfig.Packet.PacketType.WS2_SendTo:
                                res = WS2_32.sendto(Socket, (IntPtr)pBuffer, bNewBuffer.Length, Flags, ref To, ToLen);
                                break;
                        }
                    }
                }

                _ = Socket_Operation.ProcessingHookResultAsync(Socket, bRawBuffer, bNewBuffer, res, ptType, FilterAction, To, PacketTime);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            [In, Out] ref Operate.PacketConfig.Packet.SockAddr From,
            [In, Out, Optional] IntPtr FromLen)
        {
            Int32 res = 0;

            try
            {
                switch (ptType)
                {
                    case Operate.PacketConfig.Packet.PacketType.WS1_RecvFrom:
                        res = WSock32.recvfrom(Socket, lpBuffer, Length, Flags, ref From, FromLen);
                        break;

                    case Operate.PacketConfig.Packet.PacketType.WS2_RecvFrom:
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

                    Operate.FilterConfig.Filter.FilterAction FilterAction = Operate.FilterConfig.List.DoFilterList(Socket, bBufferSpan, out bNewBuffer, ptType, From);

                    if (FilterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            Operate.PacketConfig.Packet.PacketType packetType = Operate.PacketConfig.Packet.PacketType.WSASend;

            try
            {
                DateTime packetTime = DateTime.Now;
                Operate.PacketConfig.Packet.WSABUF* pWSABuffers = (Operate.PacketConfig.Packet.WSABUF*)lpWSABuffer;

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

                        Operate.FilterConfig.Filter.FilterAction filterAction =
                        Operate.FilterConfig.List.DoFilterList(
                            socket,
                            bBufferSpan,
                            out bNewBuffer,
                            packetType,
                            new Operate.PacketConfig.Packet.SockAddr());

                        if (filterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
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
                       new Operate.PacketConfig.Packet.SockAddr(),
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

                        Operate.FilterConfig.Filter.FilterAction filterAction =
                            Operate.FilterConfig.List.DoFilterList(
                                socket,
                                bRawBuffer.AsSpan(),
                                out bNewBuffer,
                                packetType,
                                new Operate.PacketConfig.Packet.SockAddr());

                        if (filterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
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
                            new Operate.PacketConfig.Packet.SockAddr(),
                            packetTime);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            Operate.PacketConfig.Packet.PacketType packetType = Operate.PacketConfig.Packet.PacketType.WSARecv;
            SocketError res = WS2_32.WSARecv(socket, lpWSABuffer, bufferCount, lpNumberOfBytesRecvd, ref flags, lpOverlapped, lpCompletionRoutine);

            try
            {
                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    if (BytesRecvd > 0)
                    {
                        DateTime packetTime = DateTime.Now;
                        Operate.PacketConfig.Packet.WSABUF* pWSABuffers = (Operate.PacketConfig.Packet.WSABUF*)lpWSABuffer;

                        if (bufferCount == 1)
                        {
                            #region//单缓存区

                            byte[] bRawBuffer = null;
                            byte[] bNewBuffer = null;

                            Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[0].buf, BytesRecvd);
                            bRawBuffer = bufferSpan.ToArray();

                            Operate.FilterConfig.Filter.FilterAction filterAction =
                                Operate.FilterConfig.List.DoFilterList(
                                    socket,
                                    bufferSpan,
                                    out bNewBuffer,
                                    packetType,
                                    new Operate.PacketConfig.Packet.SockAddr());

                            int bytesToWrite = 0;
                            if (filterAction != Operate.FilterConfig.Filter.FilterAction.Intercept)
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
                                Operate.FilterConfig.List.DoFilterList(
                                    socket,
                                    bRawBuffer.AsSpan(),
                                    out bNewBuffer,
                                    packetType,
                                    new Operate.PacketConfig.Packet.SockAddr());

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
                                new Operate.PacketConfig.Packet.SockAddr(),
                                packetTime);

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            [In] ref Operate.PacketConfig.Packet.SockAddr To,
            [In] IntPtr lpToLen,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            SocketError res = SocketError.SocketError;
            Operate.PacketConfig.Packet.PacketType packetType = Operate.PacketConfig.Packet.PacketType.WSASendTo;

            try
            {
                DateTime packetTime = DateTime.Now;
                Operate.PacketConfig.Packet.WSABUF* pWSABuffers = (Operate.PacketConfig.Packet.WSABUF*)lpWSABuffer;

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

                        Operate.FilterConfig.Filter.FilterAction filterAction =
                        Operate.FilterConfig.List.DoFilterList(
                            socket,
                            bBufferSpan,
                            out bNewBuffer,
                            packetType,
                            To);

                        if (filterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
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

                        Operate.FilterConfig.Filter.FilterAction filterAction =
                            Operate.FilterConfig.List.DoFilterList(
                                socket,
                                bRawBuffer.AsSpan(),
                                out bNewBuffer,
                                packetType,
                                To);

                        if (filterAction == Operate.FilterConfig.Filter.FilterAction.Intercept)
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            [In, Out] ref Operate.PacketConfig.Packet.SockAddr from,
            [In, Out] IntPtr lpFromlen,
            [In] IntPtr lpOverlapped,
            [In] IntPtr lpCompletionRoutine)
        {
            Operate.PacketConfig.Packet.PacketType packetType = Operate.PacketConfig.Packet.PacketType.WSARecvFrom;
            SocketError res = WS2_32.WSARecvFrom(socket, lpWSABuffer, bufferCount, lpNumberOfBytesRecvd, ref flags, ref from, lpFromlen, lpOverlapped, lpCompletionRoutine);

            try
            {
                if (res == SocketError.Success)
                {
                    int BytesRecvd = Marshal.ReadInt32(lpNumberOfBytesRecvd);
                    if (BytesRecvd > 0)
                    {
                        DateTime packetTime = DateTime.Now;
                        Operate.PacketConfig.Packet.WSABUF* pWSABuffers = (Operate.PacketConfig.Packet.WSABUF*)lpWSABuffer;

                        if (bufferCount == 1)
                        {
                            #region//单缓存区

                            byte[] bRawBuffer = null;
                            byte[] bNewBuffer = null;

                            Span<byte> bufferSpan = new Span<byte>((byte*)pWSABuffers[0].buf, BytesRecvd);
                            bRawBuffer = bufferSpan.ToArray();

                            Operate.FilterConfig.Filter.FilterAction filterAction =
                                Operate.FilterConfig.List.DoFilterList(
                                    socket,
                                    bufferSpan,
                                    out bNewBuffer,
                                    packetType,
                                    from);

                            int bytesToWrite = 0;
                            if (filterAction != Operate.FilterConfig.Filter.FilterAction.Intercept)
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
                            Operate.FilterConfig.Filter.FilterAction filterAction =
                                Operate.FilterConfig.List.DoFilterList(
                                    socket,
                                    bRawBuffer.AsSpan(),
                                    out bNewBuffer,
                                    packetType,
                                    from);

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return res;
        }

        #endregion
    }
}
