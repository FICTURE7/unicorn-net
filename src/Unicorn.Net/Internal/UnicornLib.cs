using System;
using System.Runtime.InteropServices;

namespace Unicorn.Internal
{
    /// <summary>
    /// Provides DLL imports of the unicorn library.
    /// </summary>
    internal static class UnicornLib
    {
        // Misc
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uc_version(UIntPtr major, UIntPtr minor);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr uc_strerror(UnicornError err);

#if !RELEASE
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool uc_arch_supported(int arch); // Not used.
#endif

        // Open/Close
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UnicornError uc_open(UnicornArch arch, UnicornMode mode, UIntPtr* uc);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern UnicornError uc_close(UIntPtr uc);

        // Registers Read/Write
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UnicornError uc_reg_read(UIntPtr uc, int regid, long* value);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UnicornError uc_reg_write(UIntPtr uc, int regid, long* value);

        // Emulator Start/Stop
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern UnicornError uc_emu_start(UIntPtr uc, ulong begin, ulong end, ulong timeout, int count);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern UnicornError uc_emu_stop(UIntPtr uc);

        // Memory Read/Write/Map/Unmap/Protect
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern UnicornError uc_mem_map(UIntPtr uc, ulong address, int size, int perms);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern UnicornError uc_mem_unmap(UIntPtr uc, ulong address, int size);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern UnicornError uc_mem_write(UIntPtr uc, ulong address, byte[] bytes, int size);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern UnicornError uc_mem_read(UIntPtr uc, ulong address, byte[] bytes, int size);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern UnicornError uc_mem_protect(UIntPtr uc, ulong address, int size, int perms);
    }
}
