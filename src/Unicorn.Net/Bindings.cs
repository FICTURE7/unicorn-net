using System;
using System.Runtime.InteropServices;
using Unicorn.Internal;

using static Unicorn.Internal.unicorn;

namespace Unicorn
{
    public class Bindings : IBindings
    {
        public static readonly Bindings Instance = new Bindings();

        public string StrError(UnicornError err)
            => Marshal.PtrToStringAnsi(uc_strerror((uc_err)err));

        public int Version(ref int major, ref int minor)
            => uc_version(ref major, ref minor);

        public void Free(IntPtr ptr)
            => ThrowIfError(uc_free(ptr));

        public void Open(UnicornArch arch, UnicornMode mode, ref IntPtr uc)
            => ThrowIfError(uc_open((uc_arch)arch, (uc_mode)mode, ref uc));

        public void Close(IntPtr uc)
            => ThrowIfError(uc_close(uc));

        public void ContextAlloc(IntPtr uc, ref IntPtr ctx)
            => ThrowIfError(uc_context_alloc(uc, ref ctx));

        public void ContextRestore(IntPtr uc, IntPtr ctx)
            => ThrowIfError(uc_context_restore(uc, ctx));

        public void ContextSave(IntPtr uc, IntPtr ctx)
            => ThrowIfError(uc_context_save(uc, ctx));

        public void EmuStart(IntPtr uc, ulong begin, ulong end, ulong timeout, int count)
            => ThrowIfError(uc_emu_start(uc, begin, end, timeout, count));

        public void EmuStop(IntPtr uc)
            => ThrowIfError(uc_emu_stop(uc));

        public void HookAdd(IntPtr uc, ref IntPtr hh, UnicornHookType type, IntPtr callback, IntPtr userData, ulong address, ulong end)
            => ThrowIfError(uc_hook_add(uc, ref hh, (uc_hook_type)type, callback, userData, address, end));

        public void HookAdd(IntPtr uc, ref IntPtr hh, UnicornHookType type, IntPtr callback, IntPtr userData, ulong address, ulong end, int instruction)
            => ThrowIfError(uc_hook_add(uc, ref hh, (uc_hook_type)type, callback, userData, address, end, instruction));

        public void HookDel(IntPtr uc, IntPtr hh)
            => ThrowIfError(uc_hook_del(uc, hh));

        public void MemMap(IntPtr uc, ulong address, int size, MemoryPermissions perms)
            => ThrowIfError(uc_mem_map(uc, address, size, (int)perms));

        public void MemProtect(IntPtr uc, ulong address, int size, MemoryPermissions perms)
            => ThrowIfError(uc_mem_protect(uc, address, size, (int)perms));

        public void MemRead(IntPtr uc, ulong address, byte[] buffer, int count)
            => ThrowIfError(uc_mem_read(uc, address, buffer, count));

        public void MemRegions(IntPtr uc, ref MemoryRegion[] regions)
        {
            var count = 0;
            var ptr = IntPtr.Zero;

            ThrowIfError(uc_mem_regions(uc, ref ptr, ref count));

            regions = new MemoryRegion[count];
            var size = Marshal.SizeOf(typeof(uc_mem_region));
            for (int i = 0; i < count; i++)
            {
                var nativeRegion = (uc_mem_region)Marshal.PtrToStructure(ptr, typeof(uc_mem_region));
                regions[i] = new MemoryRegion(nativeRegion.begin, nativeRegion.end, (MemoryPermissions)nativeRegion.perms);
                ptr += size;
            }

            Free(ptr);
        }

        public void MemUnmap(IntPtr uc, ulong address, int size)
            => ThrowIfError(uc_mem_unmap(uc, address, size));

        public void MemWrite(IntPtr uc, ulong address, byte[] bytes, int size)
            => ThrowIfError(uc_mem_write(uc, address, bytes, size));

        public void Query(IntPtr uc, UnicornQueryType type, ref int value)
            => ThrowIfError(uc_query(uc, (uc_query_type)type, ref value));

        public void RegRead(IntPtr uc, int regId, ref long value)
            => ThrowIfError(uc_reg_read(uc, regId, ref value));

        public void RegWrite(IntPtr uc, int regId, ref long value)
            => ThrowIfError(uc_reg_write(uc, regId, ref value));

        private static void ThrowIfError(uc_err err)
        {
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }
    }
}
