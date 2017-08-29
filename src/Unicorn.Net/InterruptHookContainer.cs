using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="emulator"></param>
    /// <param name="into"></param>
    /// <param name="userData"></param>
    public delegate void InterruptHookCallback(Emulator emulator, int into, object userData);

    /// <summary>
    /// 
    /// </summary>
    public class InterruptHookContainer : HookContainer
    {
        internal InterruptHookContainer(Emulator emulator) : base(emulator)
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public HookHandle Add(InterruptHookCallback callback, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            return AddInternal(callback, 1, 0, userData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public HookHandle Add(InterruptHookCallback callback, ulong begin, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            return AddInternal(callback, begin, end, userData);
        }

        private HookHandle AddInternal(InterruptHookCallback callback, ulong begin, ulong end, object userData)
        {
            var wrapper = new uc_cb_hookintr((uc, into, user_data) =>
            {
                Debug.Assert(uc == Emulator.Bindings.UCHandle);
                callback(Emulator, into, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            return Add(Bindings.HookType.Interrupts, ptr, begin, end);
        }
    }
}
