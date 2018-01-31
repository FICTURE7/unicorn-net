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
        public ArmEmulator(ArmMode mode) : base(Bindings.Arch.ARM, (Bindings.Mode)mode)
        {
            _registers = new ArmRegisters(this);
        }

        /// <summary>
        /// Gets the <see cref="ArmRegisters"/> of the <see cref="ArmEmulator"/> instance.
        /// </summary>
        public ArmRegisters Registers
        {
            get
            {
                CheckDisposed();

                return _registers;
            }
        }

        /// <summary>
        /// Gets the <see cref="ArmMode"/> of the <see cref="ArmEmulator"/>.
        /// </summary>
        public ArmMode Mode
        {
            get
            {
                CheckDisposed();

                return (ArmMode)_mode;
            }
        }
    }
}
