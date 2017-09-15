using System.Diagnostics;

namespace Unicorn
{
    /// <summary>
    /// Represents the registers of an <see cref="Emulator"/>.
    /// </summary>
    public abstract class Registers
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
        /// <param name="regId">Register ID.</param>
        /// <returns>Value of register read.</returns>
        protected long Read(int regId)
        {
            _emulator.CheckDisposed();

            var value = 0L;
            _emulator.Bindings.RegRead(regId, ref value);
            return value;
        }

        /// <summary>
        /// Writes the specified value to the register with the specified register ID.
        /// </summary>
        /// <param name="regId">Register ID.</param>
        /// <param name="value">Value to write to register.</param>
        protected void Write(int regId, long value)
        {
            _emulator.CheckDisposed();

            _emulator.Bindings.RegWrite(regId, ref value);
        }
    }
}
