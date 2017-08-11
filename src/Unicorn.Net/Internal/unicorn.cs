using System;
using System.Runtime.InteropServices;

namespace Unicorn.Internal
{
    /// <summary>
    /// Provides DLL imports of the unicorn library.
    /// </summary>
    internal static class unicorn
    {
        // Misc
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uc_version(ref int major, ref int minor);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr uc_strerror(uc_err err);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_free(UIntPtr mem);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_query(UIntPtr uc, uc_query_type query, ref int result);

#if !RELEASE
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool uc_arch_supported(int arch); // Not used.
#endif

        // Open/Close
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_open(uc_arch arch, uc_mode mode, ref UIntPtr uc);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_close(UIntPtr uc);

        // Registers Read/Write
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_reg_read(UIntPtr uc, int regid, ref long value);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_reg_write(UIntPtr uc, int regid, ref long value);

        // Emulator Start/Stop
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_emu_start(UIntPtr uc, ulong begin, ulong end, ulong timeout, int count);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_emu_stop(UIntPtr uc);

        // Memory Read/Write/Map/Unmap/Protect/Regions
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_map(UIntPtr uc, ulong address, int size, int perms);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_unmap(UIntPtr uc, ulong address, int size);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_write(UIntPtr uc, ulong address, byte[] bytes, int size);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_read(UIntPtr uc, ulong address, byte[] bytes, int size);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_protect(UIntPtr uc, ulong address, int size, int perms);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_regions(UIntPtr uc, ref UIntPtr regions, ref int count);

        // Context Alloc/Save/Restore.
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_context_alloc(UIntPtr uc, ref UIntPtr context);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_context_save(UIntPtr uc, UIntPtr context);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_context_restore(UIntPtr uc, UIntPtr context);

        // Hook Add/Del.
        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uc_err uc_hook_add(UIntPtr uc, ref UIntPtr hh, uc_hook_type type, IntPtr callback, UIntPtr user_data, ulong address, ulong end);

        [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_hook_del(UIntPtr uc, UIntPtr hh);
    }

    // Callbacks
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void uc_cb_hookcode(UIntPtr uc, ulong address, int size, UIntPtr user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void uc_cb_hookintr(UIntPtr uc, ulong into, UIntPtr user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void uc_cb_hookmem(UIntPtr uc, uc_mem_type type, ulong address, int size, ulong value, UIntPtr user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool uc_cb_eventmem(UIntPtr uc, uc_mem_type type, ulong address, int size, ulong value, UIntPtr user_data);
}
