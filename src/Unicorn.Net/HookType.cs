using System;

namespace Unicorn
{
    [Flags, Obsolete]
    public enum HookType
    {
        Interrupts = 1,
        Instructions = 2,
        Code = 4,
        Block = 8,

        MemoryUnmappedRead = 16,
        MemoryUnmappedWrite = 32,
        MemoryUnmappedFetch = 64,

        MemoryProtectedRead = 128,
        MemoryProtectedWrite = 256,
        MemoryProtectedFetch = 512,

        MemoryRead = 1024,
        MemoryWrite = 2048,
        MemoryFetch = 4096,

        MemoryReadAfter = 8192
    }
}
