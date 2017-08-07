using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
        /// Gets all the <see cref="MemoryRegion"/> mapped for emulation.
        /// </summary>
        public MemoryRegion[] Regions
        {
            get
            {
                _emulator.CheckDisposed();

                var count = 0;
                var regionsPtr = UIntPtr.Zero;
                var regions = (MemoryRegion[])null;

                var err = unicorn.uc_mem_regions(_emulator._uc, ref regionsPtr, ref count);
                if (err != uc_err.UC_ERR_OK)
                    throw new UnicornException(err);

                regions = new MemoryRegion[count];
                var signedPtr = new IntPtr(regionsPtr.ToUInt32());
                for (int i = 0; i < count; i++)
                {
                    var nativeStruct = (unicorn.uc_mem_region)Marshal.PtrToStructure(signedPtr, typeof(unicorn.uc_mem_region));
                    var region = new MemoryRegion(nativeStruct);

                    regions[i] = region;

                    signedPtr += Marshal.SizeOf(nativeStruct);
                }

                // Clean up the memory allocated by uc_mem_regions because we
                // don't like memory leaks.
                err = unicorn.uc_free(regionsPtr);
                if (err != uc_err.UC_ERR_OK)
                    throw new UnicornException(err);
                return regions;
            }
        }

        /// <summary>
        /// Gets the page size of <see cref="Memory"/> which is 4KB.
        /// </summary>
        public int PageSize
        {
            get
            {
                _emulator.CheckDisposed();

                var size = 0;
                var err = unicorn.uc_query(_emulator._uc, uc_query_type.UC_QUERY_PAGE_SIZE, ref size);
                if (err != uc_err.UC_ERR_OK)
                    throw new UnicornException(err);

                return size;
            }
        }

        /// <summary>
        /// Maps a memory region for emulation with the specified starting address, size and <see cref="MemoryPermissions"/>.
        /// </summary>
        /// <param name="address">Starting address of memory region.</param>
        /// <param name="size">Size of memory region.</param>
        /// <param name="permissions">Permissions of memory region.</param>
        public void Map(ulong address, int size, MemoryPermissions permissions)
        {
            _emulator.CheckDisposed();

            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be non-negative.");
            if (permissions > MemoryPermissions.All)
                throw new ArgumentException("Permissions is invalid.", nameof(permissions));

            var err = unicorn.uc_mem_map(_emulator._uc, address, size, (int)permissions);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Unmaps a region of memory used for emulation with the specified starting address and size.
        /// </summary>
        /// <param name="address">Starting address of memory region.</param>
        /// <param name="size">Size of memory region.</param>
        public void Unmap(ulong address, int size)
        {
            _emulator.CheckDisposed();

            if ((address & (ulong)PageSize) != (ulong)PageSize)
                throw new ArgumentException("Address must be aligned with page size.", nameof(address));
            if ((size & PageSize) != PageSize)
                throw new ArgumentException("Size must be a multiple of page size.", nameof(size));

            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be non-negative.");

            var err = unicorn.uc_mem_unmap(_emulator._uc, address, size);
            if (err != uc_err.UC_ERR_OK)
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
            _emulator.CheckDisposed();

            if ((address & (ulong)PageSize) != (ulong)PageSize)
                throw new ArgumentException("Address must be aligned with page size.", nameof(address));
            if ((size & PageSize) != PageSize)
                throw new ArgumentException("Size must be a multiple of page size.", nameof(size));

            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be non-negative.");
            if (permissions > MemoryPermissions.All)
                throw new ArgumentException("Permissions is invalid.", nameof(permissions));

            var err = unicorn.uc_mem_protect(_emulator._uc, address, size, (int)permissions);
            if (err != uc_err.UC_ERR_OK)
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
            _emulator.CheckDisposed();

            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (count < 0 || count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative and less or equal to the length of data.");

            var err = unicorn.uc_mem_write(_emulator._uc, address, buffer, count);
            if (err != uc_err.UC_ERR_OK)
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
            _emulator.CheckDisposed();

            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (count < 0 || count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative and less or equal to the length of data.");

            var err = unicorn.uc_mem_read(_emulator._uc, address, buffer, count);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }
    }
}
