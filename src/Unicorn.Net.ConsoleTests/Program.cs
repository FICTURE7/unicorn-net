using System;
using Unicorn.x86;

namespace Unicorn.ConsoleTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
                emulator.Memory.Unmap(addr + (1024 * 4), 4 * 1024 - 1);

                emulator.Registers.ECX = ecx;
                emulator.Registers.EDX = edx;

                emulator.Memory.Write(addr, x86code, x86code.Length);

                emulator.Start(addr, addr + (ulong)x86code.Length);

                Console.WriteLine(emulator.Registers.ECX);
                Console.WriteLine(emulator.Registers.EDX);
            }

            Console.ReadLine();
        }
    }
}
