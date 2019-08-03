namespace Unicorn
{
    /// <summary>
    /// Represents a unicorn-engine version number.
    /// </summary>
    public struct UnicornVersion
    {
        static UnicornVersion()
        {
            var mmajor = 0;
            var mminor = 0;

            var nativeVersion = Binds.Version(ref mmajor, ref mminor);
            var major = nativeVersion >> 0x8;
            var minor = nativeVersion & 0xF;

            Current = new UnicornVersion(major, minor);
        }

        /// <summary>
        /// Gets the current <see cref="UnicornVersion"/> of the wrapped unicorn-engine library.
        /// </summary>
        public static UnicornVersion Current { get; private set; }

        private readonly string _toString;

        /// <summary>
        /// Gets the major version number.
        /// </summary>
        public int Major { get; }

        /// <summary>
        /// Gets the minor version number.
        /// </summary>
        public int Minor { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnicornVersion"/> structure with the specified
        /// major and minor version number.
        /// </summary>
        /// <param name="major">Major version number.</param>
        /// <param name="minor">Minor version number.</param>
        public UnicornVersion(int major, int minor)
        {
            Major = major;
            Minor = minor;

            _toString = Major + "." + Minor;
        }

        /// <summary>
        /// Returns a string representation of the object.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString() => _toString;
    }
}
