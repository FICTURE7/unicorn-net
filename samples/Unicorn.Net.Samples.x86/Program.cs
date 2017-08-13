using System;
using System.Diagnostics;
using Unicorn.x86;

namespace Unicorn.Net.Samples.x86
{
    // Similar to
    // samples/sample_x86.c

    public class Program
    {
        // test_i386_invalid_mem_read
        public static void TestInvalidMemoryRead()
        {
            var addr = 0x1000000UL;
            var code = new byte[]
            {
                0x8B, 0x0D, 0xAA, 0xAA, 0xAA, 0xAA, // MOV ecx [0xaaaaaaaa]
                0x41, // INC ecx
                0x4A // DEC edx
            };

            Console.WriteLine("===================================");
            Console.WriteLine("Emulate i386 code that read from invalid memory.");

            using (var emulator = new x86Emulator(x86Mode.b32))
            {
                // Map 2MB of memory.
                emulator.Memory.Map(addr, 2 * 1024 * 1024, MemoryPermissions.All);
                // Write machine code to be emulated.
                emulator.Memory.Write(addr, code, code.Length);

                // Initialize registers.
                emulator.Registers.ECX = 0x1234;
                emulator.Registers.EDX = 0x7890;

                // Trace all instructions.
                emulator.Hooks.Code.Add(CodeHook, null);

                try
                {
                    // Start emulating the machine written machine code.
                    emulator.Start(addr, addr + (ulong)code.Length);
                }
                catch (UnicornException ex)
                {
                    Debug.Assert(ex.ErrorCode == Bindings.Error.ReadUnmapped, "Unexpected error code in caught UnicornException.");
                    Console.WriteLine($"Failed to start emulator instance. -> {ex.Message}.");
                }

                Console.WriteLine(">>> Emulation done. Below is the CPU context");
                Console.WriteLine($">>> ECX = 0x{emulator.Registers.ECX.ToString("x2")}");
                Console.WriteLine($">>> EDX = 0x{emulator.Registers.EDX.ToString("x2")}");
            }
        }

        public static void TestInvalidMemoryWrite()
        {
            var addr = 0x1000000UL;
            var code = new byte[]
            {
                0x89, 0x0D, 0xAA, 0xAA, 0xAA, 0xAA, // MOV [0xaaaaaaaa] ecx
                0x41, // INC ecx
                0x4A // DEC edx
            };

            Console.WriteLine("===================================");
            Console.WriteLine("Emulate i386 code that write to invalid memory.");

            using (var emulator = new x86Emulator(x86Mode.b32))
            {
                // Map 2MB of memory.
                emulator.Memory.Map(addr, 2 * 1024 * 1024, MemoryPermissions.All);
                // Write machine code to be emulated.
                emulator.Memory.Write(addr, code, code.Length);

                // Initialize registers.
                emulator.Registers.ECX = 0x1234;
                emulator.Registers.EDX = 0x7890;

                emulator.Hooks.Code.Add(CodeHook, null);
                emulator.Hooks.Memory.Add(MemoryEventHookType.UnmappedRead | MemoryEventHookType.UnmappedWrite, InvalidMemoryHook, 1, 0, null);

                // Start emulating the machine written machine code.
                emulator.Start(addr, addr + (ulong)code.Length);

                Console.WriteLine(">>> Emulation done. Below is the CPU context");
                Console.WriteLine($">>> ECX = 0x{emulator.Registers.ECX.ToString("x2")}");
                Console.WriteLine($">>> EDX = 0x{emulator.Registers.EDX.ToString("x2")}");

                // Read from memory.
                var buffer = new byte[4];
                var tmp = 0;

                emulator.Memory.Read(0xAAAAAAAA, buffer, buffer.Length);
                tmp = BitConverter.ToInt32(buffer, 0);

                Console.WriteLine($">>> Read 4 bytes from [0x{0xAAAAAAAA.ToString("x2")}] = 0x{tmp.ToString("x2")}");

                try
                {
                    emulator.Memory.Read(0xFFFFFFAA, buffer, buffer.Length);
                    tmp = BitConverter.ToInt32(buffer, 0);
                    Console.WriteLine($">>> Read 4 bytes from [0x{0xFFFFFFAA.ToString("x2")}] = 0x{tmp.ToString("x2")}");
                }
                catch
                {
                    Console.WriteLine($">>> Failed to read 4 bytes from [0x{0xFFFFFFAA.ToString("x2")}]");
                }
            }
        }

        // test_i386_loop
        public static void TestLoop()
        {
            var addr = 0x1000000UL;
            var code = new byte[]
            {
                0x41, // INC ecx
                0x4A, // DEC edx
                0xEB, // JMP self-loop
                0xFE
            };

            Console.WriteLine("===================================");
            Console.WriteLine("Emulate i386 code that emulates forever.");

            using (var emulator = new x86Emulator(x86Mode.b32))
            {
                // Map 2MB of memory.
                emulator.Memory.Map(addr, 2 * 1024 * 1024, MemoryPermissions.All);
                // Write machine code to be emulated.
                emulator.Memory.Write(addr, code, code.Length);

                // Initialize registers.
                emulator.Registers.ECX = 0x1234;
                emulator.Registers.EDX = 0x7890;

                // Emulate code for 2 seconds, so we can exit the code since it loops forever.
                emulator.Start(addr, addr + (ulong)code.Length, TimeSpan.FromSeconds(2), 0);

                Console.WriteLine(">>> Emulation done. Below is the CPU context");
                Console.WriteLine($">>> ECX = 0x{emulator.Registers.ECX.ToString("x2")}");
                Console.WriteLine($">>> EDX = 0x{emulator.Registers.EDX.ToString("x2")}");
            }
        }

        public static void CodeHook(Emulator emulator, ulong address, int size, object userData)
        {
            var eflags = ((x86Emulator)emulator).Registers.EFLAGS;

            Console.WriteLine($">>> Tracing instruction at 0x{address.ToString("x2")}, instruction size = 0x{size.ToString("x2")}.");
            Console.WriteLine($">>> --- EFLAGS is {eflags.ToString("x2")}");
        }

        private static bool InvalidMemoryHook(Emulator emulator, MemoryType type, ulong address, int size, ulong value, object userData)
        {
            switch (type)
            {
                case MemoryType.WriteUnmapped:
                    Console.WriteLine($">>> Missing memory is being WRITE at 0x{address.ToString("x2")}, data size = {size}, data value = 0x{value.ToString("x2")}");

                    // Map missing memory & return true to tell unicorn we want to continue execution.
                    emulator.Memory.Map(0xAAAA0000, 2 * 1024 * 1024, MemoryPermissions.All);
                    return true;

                default:
                    return false;
            }
        }

        public static void Main(string[] args)
        {
            TestInvalidMemoryRead();
            TestInvalidMemoryWrite();
            TestLoop();

            Console.ReadLine();
        }
    }
}
