using System;
using System.Diagnostics;

namespace Unicorn
{
    /// <summary>
    /// Represents the registers of an <see cref="Emulator"/>.
    /// </summary>
    public class Registers
    {
        // Emulator object instance which owns this Registers object instance.
        private readonly Emulator _emulator;

        internal Registers(Emulator emulator)
        {
            Debug.Assert(emulator != null);
            _emulator = emulator;
        }

        /// <summary>
        /// Reads the value of the register with specified register ID.
        /// </summary>
        /// <param name="registerId">Register ID.</param>
        /// <returns>Value of register read.</returns>
        /// <exception cref="UnicornException">Unicorn did not return <see cref="Binds.UnicornError.Ok"/>.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public long Read(int registerId)
        {
            long value = 0;

            _emulator.ThrowIfDisposed();
            _emulator.RegRead(registerId, ref value);
            return value;
        }

        /// <summary>
        /// Writes the specified value to the register with the specified register ID.
        /// </summary>
        /// <param name="registerId">Register ID.</param>
        /// <param name="value">Value to write to register.</param>
        /// <exception cref="UnicornException">Unicorn did not return <see cref="Binds.UnicornError.Ok"/>.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public void Write(int registerId, long value)
        {
            _emulator.ThrowIfDisposed();
            _emulator.RegWrite(registerId, ref value);
        }
    }
}
