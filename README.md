# Unicorn.Net
Slightly fancier *WIP* .NET binding/wrapper for the [Unicorn engine](https://github.com/unicorn-engine/unicorn).

### Example
Here is an example of how to use it. This is also the same example as the official documentation available [here](http://www.unicorn-engine.org/docs/tutorial.html) but in C# and using Unicorn.Net.
```csharp
using (var emulator = new x86Emulator(x86Mode.b32))
{
    ulong addr = 0x1000000;
    byte[] x86code =
    {
        0x41, // INC ECX
        0x4a  // DEC EDX
    };

    var ecx = 0x1234;
    var edx = 0x7890;

    // Map 2mb of memory.
    emulator.Memory.Map(addr, 2 * 1024 * 1024, MemoryPermissions.All);

    emulator.Registers.ECX = ecx;
    emulator.Registers.EDX = edx;

    emulator.Memory.Write(addr, x86code, x86code.Length);

    emulator.Start(addr, addr + (ulong)x86code.Length);

    Console.WriteLine(emulator.Registers.ECX);
    Console.WriteLine(emulator.Registers.EDX);
}
```

### TODO
List of stuff thats needs to implemented or that has been implemented.
- [x] Emulator
    - [x] uc_emu_start
    - [x] uc_emu_stop
    - [x] uc_query
- [x] Context
    - [x] uc_context_alloc
    - [x] uc_context_save
    - [x] uc_context_restore
- [x] Registers
    - [x] uc_reg_read
    - [x] uc_reg_write
- [x] Memory
    - [x] uc_mem_write
    - [x] uc_mem_read
    - [x] uc_mem_protect
    - [x] uc_mem_regions
    - [x] uc_mem_map
    - [x] uc_mem_unmap
- [ ] Hooking
    - [ ] uc_hook_add
    - [ ] uc_hook_del
- [ ] Arches
    - [x] x86
    - [ ] arm
    - [ ] arm64
    - [ ] m68k
    - [ ] mips
    - [ ] sparc

### Licensing
Unicorn.Net is licensed under the permissive [MIT License](/LICENSE).
