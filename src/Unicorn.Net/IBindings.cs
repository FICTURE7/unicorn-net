using System;

namespace Unicorn
{
    public interface IBindings
    {
        string StrError(UnicornError err);
        int Version(ref int major, ref int minor);
        void Free(IntPtr ptr);

        void Open(UnicornArch arch, UnicornMode mode, ref IntPtr uc);
        void Close(IntPtr uc);

        void ContextAlloc(IntPtr uc, ref IntPtr ctx);
        void ContextRestore(IntPtr uc, IntPtr ctx);
        void ContextSave(IntPtr uc, IntPtr ctx);

        void EmuStart(IntPtr uc, ulong begin, ulong end, ulong timeout, int count);
        void EmuStop(IntPtr uc);

        void HookAdd(IntPtr uc, ref IntPtr hh, UnicornHookType type, IntPtr callback, IntPtr userData, ulong address, ulong end);
        void HookAdd(IntPtr uc, ref IntPtr hh, UnicornHookType type, IntPtr callback, IntPtr userData, ulong address, ulong end, int instruction);
        void HookDel(IntPtr uc, IntPtr hh);

        void MemMap(IntPtr uc, ulong address, int size, MemoryPermissions permissions);
        void MemProtect(IntPtr uc, ulong address, int size, MemoryPermissions permissions);
        void MemRead(IntPtr uc, ulong address, byte[] buffer, int count);
        void MemRegions(IntPtr uc, ref MemoryRegion[] regions);
        void MemUnmap(IntPtr uc, ulong address, int size);
        void MemWrite(IntPtr uc, ulong address, byte[] buffer, int count);

        void Query(IntPtr uc, UnicornQueryType type, ref int value);

        void RegRead(IntPtr uc, int regId, ref long value);
        void RegWrite(IntPtr uc, int regId, ref long value);
    }
}