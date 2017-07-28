using System;
using System.Diagnostics;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Represents the memory of an <see cref="Emulator"/>.
    /// </summary>
    public class Memory
    {
        internal Memory(Emulator emulator)
        {
            Debug.Assert(emulator != null);

            _emulator = emulator;
        }

        // Emulator object instance which owns this Memory object instance.
        private readonly Emulator _emulator;

        /// <summary>
        /// Maps a memory region for emulation with the specified starting address, size and <see cref="MemoryPermissions"/>.
        /// </summary>
        /// <param name="address">Starting address of memory region.</param>
        /// <param name="size">Size of memory region.</param>
        /// <param name="permissions">Permissions of memory region.</param>
        public void Map(ulong address, int size, MemoryPermissions permissions)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be non-negative.");
            if (permissions > MemoryPermissions.All)
                throw new ArgumentException("Permissions was invalid.", nameof(permissions));

            var err = UnicornLib.uc_mem_map(_emulator._uc, address, size, (int)permissions);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Unmaps a region of memory used for emulation with the specified starting address and size.
        /// </summary>
        /// <param name="address">Starting address of memory region.</param>
        /// <param name="size">Size of memory region.</param>
        public void Unmap(ulong address, int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be non-negative.");

            var err = UnicornLib.uc_mem_unmap(_emulator._uc, address, size);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Sets permissions for a region of memory with the specified starting address, size and <see cref="MemoryPermissions"/>.
        /// </summary>
        /// <param name="address">Starting address of memory region.</param>
        /// <param name="size">Size of memory region.</param>
        /// <param name="permissions">Permissions of memory region.</param>
        public void Protect(ulong address, int size, MemoryPermissions permissions)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be non-negative.");
            if (permissions > MemoryPermissions.All)
                throw new ArgumentException("Permissions was invalid.", nameof(permissions));
            
            var err = UnicornLib.uc_mem_protect(_emulator._uc, address, size, (int)permissions);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Writes the specified buffer to the specified memory address.
        /// </summary>
        /// <param name="address">Address to write data.</param>
        /// <param name="buffer">Data to write.</param>
        /// <param name="count">Amount of data to write.</param>
        public void Write(ulong address, byte[] buffer, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (count < 0 || count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative and less or equal to the length of data.");

            var err = UnicornLib.uc_mem_write(_emulator._uc, address, buffer, count);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Reads data at the specified address to the specified buffer.
        /// </summary>
        /// <param name="address">Address to read.</param>
        /// <param name="buffer">Buffer thats going to contain the read data.</param>
        /// <param name="count">Amount of data to read.</param>
        public void Read(ulong address, byte[] buffer, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (count < 0 || count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative and less or equal to the length of data.");

            var err = UnicornLib.uc_mem_read(_emulator._uc, address, buffer, count);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);
        }
    }
}
