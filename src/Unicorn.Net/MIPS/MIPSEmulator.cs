using System;

namespace Unicorn.MIPS
{
    /// <summary>
    /// Represents a MIPS architecture <see cref="Emulator"/>.
    /// </summary>
    public class MIPSEmulator : Emulator
    {
        // Registsers for the MIPS emulator.
        private readonly MIPSRegisters _registers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MIPSEmulator"/> class with the specified
        /// <see cref="MIPSMode"/> to use.
        /// </summary>
        /// <param name="mode">Mode to use.</param>
        public MIPSEmulator(MIPSMode mode) : base(Bindings.Arch.MIPS, (Bindings.Mode)mode)
        {
            _registers = new MIPSRegisters(this);
        }

        /// <summary>
        /// Gets the <see cref="MIPSRegisters"/> of the <see cref="MIPSRegisters"/> instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public MIPSRegisters Registers
        {
            get
            {
                CheckDisposed();

                return _registers;
            }
        }

        /// <summary>
        /// Gets the <see cref="MIPSMode"/> of the <see cref="MIPSEmulator"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public MIPSMode Mode
        {
            get
            {
                CheckDisposed();

                return (MIPSMode)_mode;
            }
        }
    }
}
