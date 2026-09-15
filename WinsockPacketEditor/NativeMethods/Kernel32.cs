using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WinsockPacketEditor
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern uint GetCurrentThreadId();

        /// <summary>
        /// 物理内存总量（字节），取不到返回 0。
        /// 用 GlobalMemoryStatusEx 而不是 WMI：这个值在保存代理设置与启动代理服务时各查一次，
        /// WMI 一次几十毫秒且依赖服务在跑，P/Invoke 是微秒级、永远有值。
        /// </summary>
        public static ulong TotalPhysicalMemory()
        {
            try
            {
                MEMORYSTATUSEX ms = new MEMORYSTATUSEX();
                ms.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
                return GlobalMemoryStatusEx(ref ms) ? ms.ullTotalPhys : 0;
            }
            catch
            {
                return 0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
    }
}
