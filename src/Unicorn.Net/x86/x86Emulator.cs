using System;

namespace Unicorn.x86
{
    /// <summary>
    /// Represents an x86 architecture <see cref="Emulator"/>.
    /// </summary>
    public class x86Emulator : Emulator
    {
        // Registers of the x86 emulator.
        private readonly x86Registers _registers;

        /// <summary>
        /// Initializes a new instance of the <see cref="x86Emulator"/> class with the specified
        /// <see cref="x86Mode"/> to use.
        /// </summary>
        /// <param name="mode">Mode to use.</param>
        public x86Emulator(x86Mode mode) : base(Bindings.Arch.x86, (Bindings.Mode)mode)
        {
            _registers = new x86Registers(this);
        }

        /// <summary>
        /// Gets the <see cref="x86Registers"/> of the <see cref="x86Emulator"/> instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public x86Registers Registers
        {
            get
            {
                CheckDisposed();

                return _registers;
            }
        }

        /// <summary>
        /// Gets the <see cref="x86Mode"/> of the <see cref="x86Emulator"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public x86Mode Mode
        {
            get
            {
                CheckDisposed();

                return (x86Mode)_mode;
            }
        }
    }
}
