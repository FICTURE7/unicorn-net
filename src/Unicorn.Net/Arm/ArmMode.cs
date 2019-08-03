namespace Unicorn.Arm
{
    /// <summary>
    /// Defines the modes of an <see cref="ArmEmulator"/>.
    /// </summary>
    public enum ArmMode
    {
        /// <summary>
        /// ARM mode.
        /// </summary>
        Arm = UnicornMode.ARM,

        /// <summary>
        /// Thumb mode.
        /// </summary>
        Thumb = UnicornMode.ARMThumb,

        /// <summary>
        /// Cortext-M series mode.
        /// </summary>
        MClass = UnicornMode.ARMMClass,

        /// <summary>
        /// ARMv8 A32 encoding mode.
        /// </summary>
        V8 = UnicornMode.ARMv8
    }
}
