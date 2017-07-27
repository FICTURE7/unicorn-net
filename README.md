# Unicorn.Net
Slightly fancier *WIP* .NET binding/wrapper for the unicorn-engine.

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
List of stuff thats needs to implemented.
- [ ] Making a proper TODO list.

### Licensing
Unicorn.Net is licensed under the permissive [MIT License](/LICENSE).