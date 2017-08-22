using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Callback for tracing code.
    /// </summary>
    /// <param name="emulator"></param>
    /// <param name="address"></param>
    /// <param name="size"></param>
    /// <param name="userData"></param>
    public delegate void CodeHookCallback(Emulator emulator, ulong address, int size, object userData);

    /// <summary>
    /// Represents hooks for code of an <see cref="Emulator"/>.
    /// </summary>
    public class CodeHooksContainer : HooksContainer
    {
        internal CodeHooksContainer(Emulator emulator) : base(emulator)
        {
            // Space
        }

        /// <summary>
        /// Adds a <see cref="CodeHookCallback"/> to the <see cref="Emulator"/>.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public HookHandle Add(CodeHookCallback callback, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            return AddInternal(callback, 1, 0, userData);
        }

        /// <summary>
        /// Adds a <see cref="CodeHookCallback"/> to the <see cref="Emulator"/>.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public HookHandle Add(CodeHookCallback callback, ulong begin, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            return AddInternal(callback, begin, end, userData);
        }

        private HookHandle AddInternal(CodeHookCallback callback, ulong begin, ulong end, object userData)
        {
            var wrapper = new uc_cb_hookcode((uc, addr, size, user_data) =>
            {
                Debug.Assert(uc == Emulator.Bindings.UCHandle);
                callback(Emulator, addr, size, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            return Add(Bindings.HookType.Code, ptr, begin, end);
        }
    }
}
