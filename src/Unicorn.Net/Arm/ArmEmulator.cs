using System;

namespace Unicorn.Arm
{
    /// <summary>
    /// Represents an ARM architecture <see cref="Emulator"/>.
    /// </summary>
    public class ArmEmulator : Emulator
    {
        private readonly ArmRegisters _registers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArmEmulator"/> class with the specified
        /// <see cref="ArmMode"/> to use.
        /// </summary>
        /// <param name="mode">Mode to use.</param>
        public ArmEmulator(ArmMode mode) : base(UnicornArch.ARM, (UnicornMode)mode)
        {
            _registers = new ArmRegisters(this);
        }

        /// <summary>
        /// Gets the <see cref="ArmRegisters"/> of the <see cref="ArmEmulator"/> instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public ArmRegisters Registers
        {
            get
            {
                ThrowIfDisposed();
                return _registers;
            }
        }

        /// <summary>
        /// Gets the <see cref="ArmMode"/> of the <see cref="ArmEmulator"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public ArmMode Mode
        {
            get
            {
                ThrowIfDisposed();
                return (ArmMode)_mode;
            }
        }
    }
}
