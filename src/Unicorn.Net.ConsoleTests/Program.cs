using System;
using Unicorn.x86;

namespace Unicorn.ConsoleTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Unicorn version - " + Version.Current);

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

                var handle = emulator.Hooks.Code.Add(CodeHook, null);

                // Capture context.
                Console.WriteLine("-> Capturing context...");
                using (var context = emulator.Context)
                {
                    emulator.Registers.ECX = ecx;
                    emulator.Registers.EDX = edx;

                    emulator.Memory.Write(addr, x86code, x86code.Length);

                    emulator.Start(addr, addr + (ulong)x86code.Length);

                    Console.WriteLine($"ECX = {emulator.Registers.ECX}");
                    Console.WriteLine($"EDX = {emulator.Registers.EDX}");


                    Console.WriteLine("-> Restoring context...");

                    // Restore captured context.
                    emulator.Context = context;
                }

                Console.WriteLine($"ECX = {emulator.Registers.ECX}");
                Console.WriteLine($"EDX = {emulator.Registers.EDX}");
            }

            Console.ReadLine();
        }

        private static void CodeHook(Emulator emulator, ulong address, int size, object userData)
        {
            var eflags = ((x86Emulator)emulator).Registers.EFLAGS;

            Console.WriteLine($"-> Tracing instruction at 0x{address.ToString("x2")} of size 0x{size.ToString("x2")}.");
            Console.WriteLine($"-> EFLAGS = {eflags.ToString("x2")}");
        }
    }
}
