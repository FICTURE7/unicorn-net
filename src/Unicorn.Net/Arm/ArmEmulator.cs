using System;

namespace Unicorn.ARM
{
    /// <summary>
    /// Represents an ARM architecture <see cref="Emulator"/>.
    /// </summary>
    public class ARMEmulator : Emulator
    {
        // Registers for the ARM emulator.
        private readonly ARMRegisters _registers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ARMEmulator"/> class with the specified
        /// <see cref="ARMMode"/> to use.
        /// </summary>
        /// <param name="mode">Mode to use.</param>
        public ARMEmulator(ARMMode mode) : base(Bindings.Arch.ARM, (Bindings.Mode)mode)
        {
            _registers = new ARMRegisters(this);
        }

        /// <summary>
        /// Gets the <see cref="ARMRegisters"/> of the <see cref="ARMEmulator"/> instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public ARMRegisters Registers
        {
            get
            {
                CheckDisposed();

                return _registers;
            }
        }

        /// <summary>
        /// Gets the <see cref="ARMMode"/> of the <see cref="ARMEmulator"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public ARMMode Mode
        {
            get
            {
                CheckDisposed();

                return (ARMMode)_mode;
            }
        }
    }
}
