namespace Unicorn.x86
{
    /// <summary>
    /// Defines the modes of an <see cref="x86Emulator"/>.
    /// </summary>
    public enum x86Mode
    {
        // Prefixed the identifiers with a 'b' because identifiers can start with digits.

        /// <summary>
        /// 16-bit mode.
        /// </summary>
        b16 = 2,

        /// <summary>
        /// 32-bit mode.
        /// </summary>
        b32 = 4,

        /// <summary>
        /// 64-bit mode.
        /// </summary>
        b64 = 8
    }
}
