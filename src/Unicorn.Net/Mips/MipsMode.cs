namespace Unicorn.Mips
{
    /// <summary>
    /// Defines the modes of an <see cref="MipsEmulator"/>.
    /// </summary>
    public enum MipsMode
    {
        /// <summary>
        /// Big endian mode.
        /// </summary>
        BigEndian = UnicornMode.BigEndian,

        /// <summary>
        /// Little endian mode.
        /// </summary>
        LittleEndian = UnicornMode.LittleEndian,

        /// <summary>
        /// MicroMips mode.
        /// </summary>
        Micro = UnicornMode.MIPSMicro,

        /// <summary>
        /// MIPS III ISA mode.
        /// </summary>
        III = UnicornMode.MIPS3,

        /// <summary>
        /// MIPS32R6 ISA mode.
        /// </summary>
        b32R6 = UnicornMode.MIPS32R6,

        /// <summary>
        /// MIPS32 ISA mode.
        /// </summary>
        b32 = UnicornMode.MIPS32,
        
        /// <summary>
        /// MIPS64 ISA mode.
        /// </summary>
        b64 = UnicornMode.MIPS64,
    }
}
