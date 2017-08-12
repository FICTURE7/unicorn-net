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
        /// <param name="address"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public void Add(CodeHookCallback callback, ulong address, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            var wrapper = new uc_cb_hookcode((uc, addr, size, user_data) =>
            {
                Debug.Assert(uc == Emulator.Bindings.UCHandle);
                callback(Emulator, addr, size, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            var hh = IntPtr.Zero;

            Emulator.Bindings.HookAdd(ref hh, Bindings.HookType.Code, ptr, IntPtr.Zero, address, end);
        }
    }
}
