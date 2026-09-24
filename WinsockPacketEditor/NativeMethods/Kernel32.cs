using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

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

        /* ── 取进程映像路径（替代 Process.MainModule）────────────────────────
           Process.MainModule 要读目标进程的 PEB / 模块链表，在受保护、被挂起或页面被换出的
           进程上会无限阻塞，而且没有超时。QueryFullProcessImageName 由内核直接返回路径、
           不读目标内存，因此不会卡；权限只要 PROCESS_QUERY_LIMITED_INFORMATION。 */

        /// <summary>PROCESS_QUERY_LIMITED_INFORMATION —— 取映像路径所需的最小权限。</summary>
        public const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;

        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// 取进程映像的完整 Win32 路径（dwFlags=0）。lpdwSize 传缓冲区字符容量，
        /// 成功时写回实际字符数（不含结尾 \0）。
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QueryFullProcessImageName(IntPtr hProcess, uint dwFlags, [Out] StringBuilder lpExeName, ref int lpdwSize);
    }
}
