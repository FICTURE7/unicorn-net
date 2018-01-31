namespace Unicorn.ARM
{
    /// <summary>
    /// Defines the modes of an <see cref="ARMEmulator"/>.
    /// </summary>
    public enum ARMMode
    {
        /// <summary>
        /// ARM mode.
        /// </summary>
        Arm = Bindings.Mode.ARM,

        /// <summary>
        /// Thumb mode.
        /// </summary>
        Thumb = Bindings.Mode.ARMThumb,

        /// <summary>
        /// Cortext-M series mode.
        /// </summary>
        MClass = Bindings.Mode.ARMMClass,

        /// <summary>
        /// ARMv8 A32 encoding mode.
        /// </summary>
        v8 = Bindings.Mode.ARMv8
    }
}
