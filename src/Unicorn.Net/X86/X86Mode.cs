namespace Unicorn.X86
{
    /// <summary>
    /// Defines the modes of an <see cref="X86Emulator"/>.
    /// </summary>
    public enum X86Mode
    {
        /// <summary>
        /// 16-bit mode.
        /// </summary>
        b16 = UnicornMode.X86b16,

        /// <summary>
        /// 32-bit mode.
        /// </summary>
        b32 = UnicornMode.X86b32,

        /// <summary>
        /// 64-bit mode.
        /// </summary>
        b64 = UnicornMode.X86b64
    }
}
