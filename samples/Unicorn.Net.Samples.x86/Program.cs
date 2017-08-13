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

        public static void Main(string[] args)
        {
            TestInvalidMemoryRead();
            TestLoop();

            Console.ReadLine();
        }
    }
}
