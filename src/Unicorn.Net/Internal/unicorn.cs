using System;
using System.Runtime.InteropServices;

namespace Unicorn.Internal
{
    /// <summary>
    /// Provides DLL imports of the unicorn library.
    /// </summary>
    internal static class unicorn
    {
        public const string UNICORN_LIB = "unicorn";

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern int uc_version(ref int major, ref int minor);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr uc_strerror(uc_err err);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_free(IntPtr mem);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_query(IntPtr uc, uc_query_type query, ref int result);

#if !RELEASE
        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool uc_arch_supported(int arch); // Not used.
#endif

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_open(uc_arch arch, uc_mode mode, ref IntPtr uc);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_close(IntPtr uc);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_reg_read(IntPtr uc, int regid, ref long value);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_reg_write(IntPtr uc, int regid, ref long value);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_emu_start(IntPtr uc, ulong begin, ulong end, ulong timeout, int count);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_emu_stop(IntPtr uc);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_map(IntPtr uc, ulong address, int size, int perms);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_unmap(IntPtr uc, ulong address, int size);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_write(IntPtr uc, ulong address, byte[] bytes, int size);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_read(IntPtr uc, ulong address, byte[] bytes, int size);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_protect(IntPtr uc, ulong address, int size, int perms);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_mem_regions(IntPtr uc, ref IntPtr regions, ref int count);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_context_alloc(IntPtr uc, ref IntPtr context);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_context_save(IntPtr uc, IntPtr context);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_context_restore(IntPtr uc, IntPtr context);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_hook_add(IntPtr uc, ref IntPtr hh, uc_hook_type type, IntPtr callback, IntPtr user_data, ulong address, ulong end);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_hook_add(IntPtr uc, ref IntPtr hh, uc_hook_type type, IntPtr callback, IntPtr user_data, ulong address, ulong end, int instruction);

        [DllImport(UNICORN_LIB, CallingConvention = CallingConvention.Cdecl)]
        public static extern uc_err uc_hook_del(IntPtr uc, IntPtr hh);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void uc_cb_hookcode(IntPtr uc, ulong address, int size, IntPtr user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void uc_cb_hookintr(IntPtr uc, int into, IntPtr user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int uc_cb_insn_in(IntPtr uc, int port, int size, IntPtr user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void uc_cb_insn_out(IntPtr uc, int port, int size, int value, IntPtr user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void uc_cb_hookmem(IntPtr uc, uc_mem_type type, ulong address, int size, ulong value, IntPtr user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool uc_cb_eventmem(IntPtr uc, uc_mem_type type, ulong address, int size, ulong value, IntPtr user_data);
}
