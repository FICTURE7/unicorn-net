using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Represents a memory region for emulation.
    /// </summary>
    public struct MemoryRegion
    {
        // Wrap the native uc_mem_region structure.
        internal MemoryRegion(UnicornLib.uc_mem_region region)
        {
            _begin = region.begin;
            _end = region.end;
            _perms = (MemoryPermissions)region.perms;
        }

        private readonly ulong _begin;
        private readonly ulong _end;
        private readonly MemoryPermissions _perms;
        
        /// <summary>
        /// Gets the address at which the <see cref="MemoryRegion"/> starts.
        /// </summary>
        public ulong Begin => _begin;

        /// <summary>
        /// Gets the address at which the <see cref="MemoryRegion"/> ends.
        /// </summary>
        public ulong End => _end;

        /// <summary>
        /// Gets the permissions of the <see cref="MemoryRegion"/>.
        /// </summary>
        public MemoryPermissions Permissions => _perms;
    }
}
