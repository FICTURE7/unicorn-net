using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Callback for hooking memory.
    /// </summary>
    /// <param name="emulator"></param>
    /// <param name="type"></param>
    /// <param name="address"></param>
    /// <param name="size"></param>
    /// <param name="value"></param>
    /// <param name="userData"></param>
    public delegate void MemoryHookCallback(Emulator emulator, MemoryType type, ulong address, int size, ulong value, object userData);

    /// <summary>
    /// Callback for handling invalid memory accesses.
    /// </summary>
    /// <param name="emulator"></param>
    /// <param name="type"></param>
    /// <param name="address"></param>
    /// <param name="size"></param>
    /// <param name="value"></param>
    /// <param name="userData"></param>
    /// <returns></returns>
    public delegate bool MemoryEventHookCallback(Emulator emulator, MemoryType type, ulong address, int size, ulong value, object userData);

    /// <summary>
    /// Represents hooks for memory of an <see cref="Emulator"/>.
    /// </summary>
    public class MemoryHooksContainer : HooksContainer
    {
        internal MemoryHooksContainer(Emulator emulator) : base(emulator)
        {
            // Space
        }

        /// <summary>
        /// Adds a <see cref="MemoryHookCallback"/> of the specified <see cref="MemoryHookType"/> to the <see cref="Emulator"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public void Add(MemoryHookType type, MemoryHookCallback callback, ulong start, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            var wrapper = new uc_cb_hookmem((uc, _type, addr, size, value, user_data) =>
            {
                Debug.Assert(uc == Emulator._uc);
                callback(Emulator, (MemoryType)_type, addr, size, value, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            var hh = UIntPtr.Zero;

            var err = unicorn.uc_hook_add(Emulator._uc, ref hh, (uc_hook_type)type, ptr, UIntPtr.Zero, start, end);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Adds a <see cref="MemoryEventHookCallback"/> of the specified <see cref="MemoryHookType"/> to the <see cref="Emulator"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public void Add(MemoryEventHookType type, MemoryEventHookCallback callback, ulong start, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            var wrapper = new uc_cb_eventmem((uc, _type, addr, size, value, user_data) =>
            {
                Debug.Assert(uc == Emulator._uc);
                return callback(Emulator, (MemoryType)_type, addr, size, value, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            var hh = UIntPtr.Zero;

            var err = unicorn.uc_hook_add(Emulator._uc, ref hh, (uc_hook_type)type, ptr, UIntPtr.Zero, start, end);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }
    }

    /// <summary>
    /// Types of memory accesses for <see cref="MemoryHookCallback"/>.
    /// </summary>
    public enum MemoryHookType
    {
        /// <summary>
        /// Read memory.
        /// </summary>
        Read = 1024,

        /// <summary>
        /// Write memory.
        /// </summary>
        Write = 2048,

        /// <summary>
        /// Fetch memory.
        /// </summary>
        Fetch = 4096,

        /// <summary>
        /// Read memory successful access.
        /// </summary>
        ReadAfter = 8192
    }

    /// <summary>
    /// Types of invalid memory accesses for <see cref="MemoryEventHookCallback"/>.
    /// </summary>
    public enum MemoryEventHookType
    {
        /// <summary>
        /// Unmapped memory read.
        /// </summary>
        UnmappedRead = 16,

        /// <summary>
        /// Unmapped memory write.
        /// </summary>
        UnmappedWrite = 32,

        /// <summary>
        /// Unmapped memory fetch.
        /// </summary>
        UnmappedFetch = 64,

        /// <summary>
        /// Protected memory read.
        /// </summary>
        ProtectedRead = 128,

        /// <summary>
        /// Protected memory write.
        /// </summary>
        ProtectedWrite = 256,

        /// <summary>
        /// Protected memory fetch.
        /// </summary>
        ProtectedFetch = 512,
    }
}
