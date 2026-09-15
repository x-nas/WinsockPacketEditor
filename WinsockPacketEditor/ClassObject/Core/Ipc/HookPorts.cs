using System;

namespace WinsockPacketEditor.Ipc
{
    // Cache only within one filter-list invocation. A cache keyed only by socket
    // would become stale when Windows reuses a closed handle.
    internal static class HookPorts
    {
        internal struct State
        {
            internal bool Active, Cached;
            internal int Socket, Local, Remote;
            internal Operate.PacketConfig.Packet.PacketType Type;
            internal Operate.PacketConfig.Packet.SockAddr Address;
        }

        [ThreadStatic] private static State current;

        internal struct Scope : IDisposable
        {
            private readonly State previous;
            internal Scope(State previous) { this.previous = previous; }
            public void Dispose() { current = previous; }
        }

        internal static Scope Enter()
        {
            var scope = new Scope(current);
            current = new State { Active = true };
            return scope;
        }

        private static int Port(Operate.PacketConfig.Packet.SockAddr address)
        {
            return address.sin_family == 2
                ? ((address.sin_port & 255) << 8) | (address.sin_port >> 8) : -1;
        }

        internal static void Get(int socket, Operate.PacketConfig.Packet.PacketType type,
            Operate.PacketConfig.Packet.SockAddr address, out int local, out int remote)
        {
            if (current.Active && current.Cached && current.Socket == socket && current.Type == type &&
                current.Address.sin_family == address.sin_family && current.Address.sin_port == address.sin_port)
            {
                local = current.Local; remote = current.Remote; return;
            }
            int size = 16;
            var endpoint = new Operate.PacketConfig.Packet.SockAddr();
            local = WS2_32.getsockname(socket, ref endpoint, ref size) == 0 ? Port(endpoint) : -1;
            remote = Port(address);
            if (remote < 0)
            {
                size = 16;
                endpoint = new Operate.PacketConfig.Packet.SockAddr();
                remote = WS2_32.getpeername(socket, ref endpoint, ref size) == 0 ? Port(endpoint) : -1;
            }
            if (current.Active)
            {
                current.Cached = true; current.Socket = socket; current.Type = type; current.Address = address;
                current.Local = local; current.Remote = remote;
            }
        }
    }
}
