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
    /// <param name="port"></param>
    /// <param name="size"></param>
    /// <param name="userData"></param>
    /// <returns></returns>
    public delegate int InstructionInHookCallback(Emulator emulator, int port, int size, object userData);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="emulator"></param>
    /// <param name="port"></param>
    /// <param name="size"></param>
    /// <param name="value"></param>
    /// <param name="userData"></param>
    public delegate void InstructionOutHookCallback(Emulator emulator, int port, int size, int value, object userData);

    /// <summary>
    /// 
    /// </summary>
    public class InstructionHooksContainer : HooksContainer
    {
        internal InstructionHooksContainer(Emulator emulator) : base(emulator)
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="instruction"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public HookHandle Add(InstructionInHookCallback callback, Instruction instruction, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            return AddInInternal(callback, instruction, 1, 0, userData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="instruction"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public HookHandle Add(InstructionInHookCallback callback, Instruction instruction, ulong begin, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            return AddInInternal(callback, instruction, begin, end, userData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="instruction"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public HookHandle Add(InstructionOutHookCallback callback, Instruction instruction, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            return AddOutInternal(callback, instruction, 1, 0, userData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="instruction"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="userData"></param>
        public HookHandle Add(InstructionOutHookCallback callback, Instruction instruction, ulong begin, ulong end, object userData)
        {
            Emulator.CheckDisposed();

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            return AddOutInternal(callback, instruction, begin, end, userData);
        }

        private HookHandle AddInInternal(InstructionInHookCallback callback, Instruction instruction, ulong begin, ulong end, object userData)
        {
            var wrapper = new uc_cb_insn_in((uc, port, size, user_data) =>
            {
                Debug.Assert(uc == Emulator.Bindings.UCHandle);
                return callback(Emulator, port, size, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            return AddInternal(ptr, begin, end, instruction);
        }

        private HookHandle AddOutInternal(InstructionOutHookCallback callback, Instruction instruction, ulong begin, ulong end, object userData)
        {
            var wrapper = new uc_cb_insn_out((uc, port, size, value, user_data) =>
            {
                Debug.Assert(uc == Emulator.Bindings.UCHandle);
                callback(Emulator, port, size, value, userData);
            });

            var ptr = Marshal.GetFunctionPointerForDelegate(wrapper);
            return AddInternal(ptr, begin, end, instruction);
        }

        private HookHandle AddInternal(IntPtr callback, ulong begin, ulong end, Instruction inst)
        {
            var ptr = IntPtr.Zero;
            Emulator.Bindings.HookAdd(ref ptr, Bindings.HookType.Instructions, callback, IntPtr.Zero, begin, end, inst._id);

            var handle = new HookHandle(ptr);
            Handles.Add(handle);

            return handle;
        }
    }
}
