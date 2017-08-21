using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Callback for tracing basic blocks.
    /// </summary>
    /// <param name="emulator"></param>
    /// <param name="address"></param>
    /// <param name="size"></param>
    /// <param name="userData"></param>
    public delegate void BlockHookCallback(Emulator emulator, ulong address, int size, object userData);

    /// <summary>
    /// Represents hooks for block of an <see cref="Emulator"/>.
    /// </summary>
    public class BlockHooksContainer : HooksContainer
    {
        internal BlockHooksContainer(Emulator emulator) : base(emulator)
        {
            // Space
        }

        /// <summary>
        /// Adds a <see cref="BlockHookCallback"/> to the <see cref="Emulator"/>.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public HookHandle Add(BlockHookCallback callback, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            return AddInternal(callback, 1, 0, userData);
        }

        /// <summary>
        /// Adds a <see cref="BlockHookCallback"/> to the <see cref="Emulator"/>.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public HookHandle Add(BlockHookCallback callback, ulong begin, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            if (begin > end)
                throw new ArgumentException("Begin address cannot be greater than end address. Use Add(BlockHookCallback, object) instead to add a global hook.");

            return AddInternal(callback, begin, end, userData);
        }

        private HookHandle AddInternal(BlockHookCallback callback, ulong begin, ulong end, object userData)
        {
            var wrapper = new uc_cb_hookcode((uc, addr, size, user_data) =>
            {
                Debug.Assert(uc == Emulator.Bindings.UCHandle);
                callback(Emulator, addr, size, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            return Add(Bindings.HookType.Block, ptr, begin, end);
        }
    }
}
