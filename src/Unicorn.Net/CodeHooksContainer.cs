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
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public void Add(CodeHookCallback callback, ulong start, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            var wrapper = new uc_cb_hookcode((uc, addr, size, user_data) =>
            {
                Debug.Assert(uc == Emulator._uc);
                callback(Emulator, addr, size, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            var hh = UIntPtr.Zero;

            var err = unicorn.uc_hook_add(Emulator._uc, ref hh, uc_hook_type.UC_HOOK_CODE, ptr, UIntPtr.Zero, start, end);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }
    }
}
