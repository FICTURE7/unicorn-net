using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Unicorn
{
    /// <summary>
    /// Base class of hook containers.
    /// </summary>
    public abstract class HooksContainer : IEnumerable<HookHandle>
    {
        internal HooksContainer(Emulator emulator)
        {
            Debug.Assert(emulator != null);

            _emulator = emulator;
            _handles = new List<HookHandle>();
        }

        private readonly Emulator _emulator;
        private readonly List<HookHandle> _handles;

        /// <summary>
        /// Gets the number of <see cref="HookHandle"/> in this <see cref="HooksContainer"/>.
        /// </summary>
        public int Count => _handles.Count;

        /// <summary>
        /// Gets the <see cref="Unicorn.Emulator"/> instance which owns this <see cref="HooksContainer"/>.
        /// </summary>
        protected Emulator Emulator => _emulator;

        /// <summary>
        /// Adds a hook to <see cref="Emulator"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected HookHandle Add(Bindings.HookType type, IntPtr callback, ulong begin, ulong end)
        {
            var ptr = IntPtr.Zero;
            Emulator.Bindings.HookAdd(ref ptr, type, callback, IntPtr.Zero, begin, end);

            var handle = new HookHandle(ptr);
            _handles.Add(handle);

            return handle;
        }

        /// <summary>
        /// Removes a hook with the specified <see cref="HookHandle"/> from the <see cref="Emulator"/>.
        /// </summary>
        /// <param name="handle"><see cref="HookHandle"/> to the hook to remove.</param>
        /// <returns><c>true</c> if <paramref name="handle"/> was found and removed; otherwise <c>false</c>.</returns>
        public bool Remove(HookHandle handle)
        {
            Emulator.Bindings.HookRemove(handle._hh);
            return _handles.Remove(handle);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> which iterates through the <see cref="HookHandle"/> of the <see cref="HooksContainer"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> which iterates through the <see cref="HookHandle"/> of the <see cref="HooksContainer"/>.</returns>
        public IEnumerator<HookHandle> GetEnumerator()
        {
            return ((IEnumerable<HookHandle>)_handles).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<HookHandle>)_handles).GetEnumerator();
        }
    }
}
