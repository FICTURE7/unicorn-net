using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Types of memory accesses for memory hooks.
    /// </summary>
    public enum MemoryType
    {
        /// <summary>
        /// Memory read from.
        /// </summary>
        Read = uc_mem_type.UC_MEM_READ,
        /// <summary>
        /// Memory written to.
        /// </summary>
        Write = uc_mem_type.UC_MEM_WRITE,
        /// <summary>
        /// Memory fetched at.
        /// </summary>
        Fetch = uc_mem_type.UC_MEM_FETCH,

        /// <summary>
        /// Unmapped memory read from.
        /// </summary>
        ReadUnmapped = uc_mem_type.UC_MEM_READ_UNMAPPED,
        /// <summary>
        /// Unmapped memory written to.
        /// </summary>
        WriteUnmapped = uc_mem_type.UC_MEM_WRITE_UNMAPPED, 
        /// <summary>
        /// Unmapped memory fetched at.
        /// </summary>
        FetchUnmapped = uc_mem_type.UC_MEM_FETCH_UNMAPPED,

        /// <summary>
        /// Write to write protected memory.
        /// </summary>
        WriteProtected = uc_mem_type.UC_MEM_WRITE_PROT,
        /// <summary>
        /// Read to read protected memory.
        /// </summary>
        ReadProtected = uc_mem_type.UC_MEM_READ_PROT,
        /// <summary>
        /// Fetch on non-executable memory.
        /// </summary>
        FetchProtected = uc_mem_type.UC_MEM_FETCH_PROT,

        /// <summary>
        /// Successful memory read.
        /// </summary>
        ReadAfter = uc_mem_type.UC_MEM_READ_AFTER
    }
}
