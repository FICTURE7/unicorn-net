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
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public HookHandle Add(MemoryHookType type, MemoryHookCallback callback, ulong begin, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var wrapper = new uc_cb_hookmem((uc, _type, addr, size, value, user_data) =>
            {
                Debug.Assert(uc == Emulator.Bindings.UCHandle);
                callback(Emulator, (MemoryType)_type, addr, size, value, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            var hh = IntPtr.Zero;

            return Add((Bindings.HookType)type, ptr, begin, end);
        }

        /// <summary>
        /// Adds a <see cref="MemoryEventHookCallback"/> of the specified <see cref="MemoryHookType"/> to the <see cref="Emulator"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public HookHandle Add(MemoryEventHookType type, MemoryEventHookCallback callback, ulong begin, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var wrapper = new uc_cb_eventmem((uc, _type, addr, size, value, user_data) =>
            {
                Debug.Assert(uc == Emulator.Bindings.UCHandle);
                return callback(Emulator, (MemoryType)_type, addr, size, value, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            var hh = IntPtr.Zero;

            return Add((Bindings.HookType)type, ptr, begin, end);
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
        Read = Bindings.HookType.MemRead,

        /// <summary>
        /// Write memory.
        /// </summary>
        Write = Bindings.HookType.MemWrite,

        /// <summary>
        /// Fetch memory.
        /// </summary>
        Fetch = Bindings.HookType.MemFetch,

        /// <summary>
        /// Read memory successful access.
        /// </summary>
        ReadAfter = Bindings.HookType.MemReadAfter
    }

    /// <summary>
    /// Types of invalid memory accesses for <see cref="MemoryEventHookCallback"/>.
    /// </summary>
    public enum MemoryEventHookType
    {
        /// <summary>
        /// Unmapped memory read.
        /// </summary>
        UnmappedRead = Bindings.HookType.MemReadUnmapped,

        /// <summary>
        /// Unmapped memory write.
        /// </summary>
        UnmappedWrite = Bindings.HookType.MemWriteUnmapped,

        /// <summary>
        /// Unmapped memory fetch.
        /// </summary>
        UnmappedFetch = Bindings.HookType.MemFetchUnmapped,

        /// <summary>
        /// Protected memory read.
        /// </summary>
        ProtectedRead = Bindings.HookType.MemReadProtected,

        /// <summary>
        /// Protected memory write.
        /// </summary>
        ProtectedWrite = Bindings.HookType.MemWriteProtected,

        /// <summary>
        /// Protected memory fetch.
        /// </summary>
        ProtectedFetch = Bindings.HookType.MemFetchProtected,
    }
}
