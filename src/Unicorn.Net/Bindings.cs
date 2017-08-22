using System;
using System.Runtime.InteropServices;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// A thin layer bind to the Unicorn engine.
    /// </summary>
    public class Bindings
    {
        //TODO: Implement the other stuff as well. E.g: uc_reg_read_batch.

        /// <summary>
        /// Arches defined in unicorn.h.
        /// </summary>
        public enum Arch
        {
            /// <summary>
            /// ARM.
            /// </summary>
            ARM = uc_arch.UC_ARCH_ARM,

            /// <summary>
            /// ARM64.
            /// </summary>
            ARM64 = uc_arch.UC_ARCH_ARM64,

            /// <summary>
            /// MIPS.
            /// </summary>
            MIPS = uc_arch.UC_ARCH_MIPS,

            /// <summary>
            /// x86.
            /// </summary>
            x86 = uc_arch.UC_ARCH_X86,

            /// <summary>
            /// PPC.
            /// </summary>
            PPC = uc_arch.UC_ARCH_PPC,

            /// <summary>
            /// SPARC.
            /// </summary>
            SPARC = uc_arch.UC_ARCH_SPARC,

            /// <summary>
            /// M68k.
            /// </summary>
            M68k = uc_arch.UC_ARCH_M68K
        }

        /// <summary>
        /// Modes defined in unicorn.h
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Little-endian mode default mode.
            /// </summary>
            LittleEndian = uc_mode.UC_MODE_LITTLE_ENDIAN,
            /// <summary>
            /// Big-endian mode.
            /// </summary>
            BigEndian = uc_mode.UC_MODE_BIG_ENDIAN,

            /// <summary>
            /// ARM mode.
            /// </summary>
            ARM = uc_mode.UC_MODE_ARM,
            /// <summary>
            /// Thumb mode.
            /// </summary>
            ARMThumb = uc_mode.UC_MODE_THUMB,
            /// <summary>
            /// Cortex-M series.
            /// </summary>
            ARMMClass = uc_mode.UC_MODE_MCLASS,
            /// <summary>
            /// ARMv8 A32 encodings.
            /// </summary>
            ARMv8 = uc_mode.UC_MODE_V8,

            /// <summary>
            /// MicroMips mode.
            /// </summary>
            MIPSMicro = uc_mode.UC_MODE_MICRO,
            /// <summary>
            /// MIPS III ISA.
            /// </summary>
            MIPS3 = uc_mode.UC_MODE_MIPS3,
            /// <summary>
            /// MIPS2R6 ISA.
            /// </summary>
            MIPS32R6 = uc_mode.UC_MODE_MIPS32R6,
            /// <summary>
            /// MIPS32 ISA.
            /// </summary>
            MIPS32 = uc_mode.UC_MODE_MIPS32,
            /// <summary>
            /// MIPS64 ISA.
            /// </summary>
            MIPS64 = uc_mode.UC_MODE_MIPS64,

            /// <summary>
            /// 16-bit mode.
            /// </summary>
            x86b16 = uc_mode.UC_MODE_16,
            /// <summary>
            /// 32-bit mode.
            /// </summary>
            x86b32 = uc_mode.UC_MODE_32,
            /// <summary>
            /// 64-bit mode.
            /// </summary>
            x86b64 = uc_mode.UC_MODE_64,

            /// <summary>
            /// 32-bit mode.
            /// </summary>
            PPC32 = uc_mode.UC_MODE_PPC32,
            /// <summary>
            /// 64-bit mode.
            /// </summary>
            PPC64 = uc_mode.UC_MODE_PPC64,
            /// <summary>
            /// Quad processing eXtensions mode.
            /// </summary>
            PPCQPX = uc_mode.UC_MODE_QPX,

            /// <summary>
            /// 32-bit mode.
            /// </summary>
            SPARC32 = uc_mode.UC_MODE_SPARC32,
            /// <summary>
            /// 64-bit mode.
            /// </summary>
            SPARC64 = uc_mode.UC_MODE_SPARC64,
            /// <summary>
            /// SPARCV9 mode.
            /// </summary>
            SPARCV9 = uc_mode.UC_MODE_V9,
        }

        /// <summary>
        /// Query types defined in unicorn.h.
        /// </summary>
        public enum QueryType
        {
            /// <summary>
            /// Mode.
            /// </summary>
            Mode = uc_query_type.UC_QUERY_MODE,

            /// <summary>
            /// Page size.
            /// </summary>
            PageSize = uc_query_type.UC_QUERY_PAGE_SIZE
        }

        /// <summary>
        /// Hook types defined in unicorn.h.
        /// </summary>
        public enum HookType
        {
            /// <summary>
            /// Interrupts/Syscalls.
            /// </summary>
            Interrupts = uc_hook_type.UC_HOOK_INTR,
            /// <summary>
            /// Particular instruction.
            /// </summary>
            Instructions = uc_hook_type.UC_HOOK_INSN,

            /// <summary>
            /// Range of code.
            /// </summary>
            Code = uc_hook_type.UC_HOOK_CODE,
            /// <summary>
            /// Basic block.
            /// </summary>
            Block = uc_hook_type.UC_HOOK_BLOCK,

            /// <summary>
            /// Memory read on unmapped memory.
            /// </summary>
            MemReadUnmapped = uc_hook_type.UC_HOOK_MEM_READ_UNMAPPED,
            /// <summary>
            /// Memory write on unmapped memory.
            /// </summary>
            MemWriteUnmapped = uc_hook_type.UC_HOOK_MEM_WRITE_UNMAPPED,
            /// <summary>
            /// Memory fetch on unmapped memory.
            /// </summary>
            MemFetchUnmapped = uc_hook_type.UC_HOOK_MEM_FETCH_UNMAPPED,

            /// <summary>
            /// Memory read on read-protected memory.
            /// </summary>
            MemReadProtected = uc_hook_type.UC_HOOK_MEM_READ_PROT,
            /// <summary>
            /// Memory write on write-protected memory.
            /// </summary>
            MemWriteProtected = uc_hook_type.UC_HOOK_MEM_WRITE_PROT,
            /// <summary>
            /// Memory fetch on non-executable memory.
            /// </summary>
            MemFetchProtected = uc_hook_type.UC_HOOK_MEM_FETCH_PROT,

            /// <summary>
            /// Memory reads.
            /// </summary>
            MemRead = uc_hook_type.UC_HOOK_MEM_READ,
            /// <summary>
            /// Memory writes.
            /// </summary>
            MemWrite = uc_hook_type.UC_HOOK_MEM_WRITE,
            /// <summary>
            /// Memory fetches.
            /// </summary>
            MemFetch = uc_hook_type.UC_HOOK_MEM_FETCH,

            /// <summary>
            /// Successful memory reads.
            /// </summary>
            MemReadAfter = uc_hook_type.UC_HOOK_MEM_READ_AFTER,
        }

        /// <summary>
        /// Errors defined in unicorn.h
        /// </summary>
        public enum Error
        {
            /// <summary>
            /// No error.
            /// </summary>
            Ok = uc_err.UC_ERR_OK,
            /// <summary>
            /// Out of memory.
            /// </summary>
            NoMem = uc_err.UC_ERR_NOMEM,
            /// <summary>
            /// Unsupported architecture.
            /// </summary>
            Arch = uc_err.UC_ERR_ARCH,
            /// <summary>
            /// Invalid handle.
            /// </summary>
            Handle = uc_err.UC_ERR_HANDLE,
            /// <summary>
            /// Invalid or unsupported mode.
            /// </summary>
            Mode = uc_err.UC_ERR_MODE,
            /// <summary>
            /// Unsupported version.
            /// </summary>
            Version = uc_err.UC_ERR_VERSION,
            /// <summary>
            /// Quit emulation due to read on unmapped memory.
            /// </summary>
            ReadUnmapped = uc_err.UC_ERR_READ_UNMAPPED,
            /// <summary>
            /// Quit emulation due to write on unmapped memory.
            /// </summary>
            WriteUnmapped = uc_err.UC_ERR_WRITE_UNMAPPED,
            /// <summary>
            /// Quit emulation due to fetch on unmapped memory.
            /// </summary>
            FetchUnmapped = uc_err.UC_ERR_FETCH_UNMAPPED,
            /// <summary>
            /// Invalid hook type.
            /// </summary>
            Hook = uc_err.UC_ERR_HOOK,
            /// <summary>
            /// Quit emulation due to invalid instruction.
            /// </summary>
            InstructionInvalid = uc_err.UC_ERR_INSN_INVALID,
            /// <summary>
            /// Invalid memory mapping.
            /// </summary>
            Map = uc_err.UC_ERR_MAP,
            /// <summary>
            /// Quit emulation due to write on write protected memory.
            /// </summary>
            WriteProtected = uc_err.UC_ERR_WRITE_PROT,
            /// <summary>
            /// Quit emulation due to write on read protected memory.
            /// </summary>
            ReadProtected = uc_err.UC_ERR_READ_PROT,
            /// <summary>
            /// Quit emulation due to write on non-executable memory.
            /// </summary>
            FetchProtected = uc_err.UC_ERR_FETCH_PROT,
            /// <summary>
            /// Invalid argument provided.
            /// </summary>
            Argument = uc_err.UC_ERR_ARG,
            /// <summary>
            /// Unaligned read.
            /// </summary>
            ReadUnaligned = uc_err.UC_ERR_READ_UNALIGNED,
            /// <summary>
            /// Unaligned write.
            /// </summary>
            WriteUnaligned = uc_err.UC_ERR_WRITE_UNALIGNED,
            /// <summary>
            /// Unaligned fetch.
            /// </summary>
            FetchUnaligned = uc_err.UC_ERR_FETCH_UNALIGNED,
            /// <summary>
            /// Hook for this event already existed.
            /// </summary>
            HookExist = uc_err.UC_ERR_HOOK_EXIST,
            /// <summary>
            /// Insufficient resource.
            /// </summary>
            Resource = uc_err.UC_ERR_RESOURCE,
            /// <summary>
            /// Unhandled CPU exception.
            /// </summary>
            Exception = uc_err.UC_ERR_EXCEPTION,
        }

        /// <summary>
        /// Binds to uc_version.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <returns></returns>
        public static int Version(ref int major, ref int minor)
        {
            return unicorn.uc_version(ref major, ref minor);
        }

        /// <summary>
        /// Binds to uc_strerror.
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        public static string ErrorToString(Error err)
        {
            var ptr = unicorn.uc_strerror((uc_err)err);
            var errString = Marshal.PtrToStringAnsi(ptr);
            return errString;
        }

        /// <summary>
        /// Binds to uc_free.
        /// </summary>
        /// <param name="addr"></param>
        public static void Free(IntPtr addr)
        {
            var err = unicorn.uc_free(addr);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bindings"/> class.
        /// </summary>
        public Bindings()
        {
            // Space
        }

        private IntPtr _uc;

        /// <summary>
        /// Gets the handle of the <see cref="Bindings"/> returned by <see cref="Open(Arch, Mode)"/>.
        /// </summary>
        public IntPtr UCHandle => _uc;

        /// <summary>
        /// Binds to uc_open.
        /// </summary>
        /// <param name="arch"></param>
        /// <param name="mode"></param>
        public void Open(Arch arch, Mode mode)
        {
            var err = unicorn.uc_open((uc_arch)arch, (uc_mode)mode, ref _uc);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_close.
        /// </summary>
        public void Close()
        {
            var err = unicorn.uc_close(_uc);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_emu_start.
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="timeout"></param>
        /// <param name="count"></param>
        public void EmuStart(ulong begin, ulong end, ulong timeout, int count)
        {
            var err = unicorn.uc_emu_start(_uc, begin, end, timeout, count);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_emu_stop.
        /// </summary>
        public void EmuStop()
        {
            var err = unicorn.uc_emu_stop(_uc);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_reg_read.
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="value"></param>
        public void RegRead(int regId, ref long value)
        {
            var err = unicorn.uc_reg_read(_uc, regId, ref value);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_reg_write.
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="value"></param>
        public void RegWrite(int regId, ref long value)
        {
            var err = unicorn.uc_reg_write(_uc, regId, ref value);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_mem_regions.
        /// </summary>
        /// <param name="regions"></param>
        /// <param name="count"></param>
        public void MemRegions(ref IntPtr regions, ref int count)
        {
            var err = unicorn.uc_mem_regions(_uc, ref regions, ref count);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_mem_regions but manages the reading of uc_mem_region structures.
        /// </summary>
        /// <param name="regions"></param>
        public void MemRegions(ref MemoryRegion[] regions)
        {
            var count = 0;
            var ptr = IntPtr.Zero;

            MemRegions(ref ptr, ref count);

            regions = new MemoryRegion[count];
            var size = Marshal.SizeOf(typeof(uc_mem_region));
            for (int i = 0; i < count; i++)
            {
                var nativeStruct = (uc_mem_region)Marshal.PtrToStructure(ptr, typeof(uc_mem_region));
                var region = new MemoryRegion(nativeStruct.begin, nativeStruct.end, (MemoryPermissions)nativeStruct.perms);

                regions[i] = region;
                ptr += size;
            }

            Free(ptr);
        }

        /// <summary>
        /// Binds to uc_mem_map.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="size"></param>
        /// <param name="permissions"></param>
        public void MemMap(ulong address, int size, MemoryPermissions permissions)
        {
            var err = unicorn.uc_mem_map(_uc, address, size, (int)permissions);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_mem_unmap.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="size"></param>
        public void MemUnmap(ulong address, int size)
        {
            var err = unicorn.uc_mem_unmap(_uc, address, size);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_mem_protect
        /// </summary>
        /// <param name="address"></param>
        /// <param name="size"></param>
        /// <param name="permissions"></param>
        public void MemProtect(ulong address, int size, MemoryPermissions permissions)
        {
            var err = unicorn.uc_mem_protect(_uc, address, size, (int)permissions);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_mem_write.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        public void MemWrite(ulong address, byte[] buffer, int count)
        {
            var err = unicorn.uc_mem_write(_uc, address, buffer, count);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_mem_read.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        public void MemRead(ulong address, byte[] buffer, int count)
        {
            var err = unicorn.uc_mem_read(_uc, address, buffer, count);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_context_alloc.
        /// </summary>
        /// <param name="ctx"></param>
        public void ContextAlloc(ref IntPtr ctx)
        {
            var err = unicorn.uc_context_alloc(_uc, ref ctx);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_context_save.
        /// </summary>
        /// <param name="ctx"></param>
        public void ContextSave(IntPtr ctx)
        {
            var err = unicorn.uc_context_save(_uc, ctx);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_context_restore.
        /// </summary>
        /// <param name="ctx"></param>
        public void ContextRestore(IntPtr ctx)
        {
            var err = unicorn.uc_context_restore(_uc, ctx);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_hook_add.
        /// </summary>
        /// <param name="hh"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <param name="userData"></param>
        /// <param name="address"></param>
        /// <param name="end"></param>
        public void HookAdd(ref IntPtr hh, HookType type, IntPtr callback, IntPtr userData, ulong address, ulong end)
        {
            var err = unicorn.uc_hook_add(_uc, ref hh, (uc_hook_type)type, callback, userData, address, end);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_hook_add.
        /// </summary>
        /// <param name="hh"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <param name="userData"></param>
        /// <param name="address"></param>
        /// <param name="end"></param>
        /// <param name="instruction"></param>
        public void HookAdd(ref IntPtr hh, HookType type, IntPtr callback, IntPtr userData, ulong address, ulong end, int instruction)
        {
            var err = unicorn.uc_hook_add(_uc, ref hh, (uc_hook_type)type, callback, userData, address, end, instruction);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_hook_del.
        /// </summary>
        /// <param name="hh"></param>
        public void HookRemove(IntPtr hh)
        {
            var err = unicorn.uc_hook_del(_uc, hh);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Binds to uc_query.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void Query(QueryType type, ref int value)
        {
            var err = unicorn.uc_query(_uc, (uc_query_type)type, ref value);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }
    }
}
