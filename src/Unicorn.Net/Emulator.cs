using System;
using System.Diagnostics;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Represents a unicorn-engine emulator.
    /// </summary>
    public abstract class Emulator : IDisposable
    {
        internal Emulator(UnicornArch arch, UnicornMode mode)
        {
            var uc = UIntPtr.Zero;
            unsafe
            {
                var err = UnicornLib.uc_open(arch, mode, &uc);
                if (err != UnicornError.UC_ERR_OK)
                    throw new UnicornException(err);
            }

            _uc = uc;
            _memory = new Memory(this);
        }

        // To determine if we've been disposed or not.
        internal bool _disposed;
        // Memory object instance which represents the memory of the emulator.
        private readonly Memory _memory;

        // Pointer to the native unicorn engine handle.
        internal readonly UIntPtr _uc;

        /// <summary>
        /// Gets the <see cref="Unicorn.Memory"/> of the <see cref="Emulator"/>.
        /// </summary>
        public Memory Memory
        {
            get
            {
                CheckDisposed();

                return _memory;
            }
        }

        /// <summary>
        /// Starts emulation at the specified begin address and end address.
        /// </summary>
        /// <param name="beginAddr">Address at which to begin emulation.</param>
        /// <param name="endAddr">Address at which to end emulation.</param>
        public void Start(ulong beginAddr, ulong endAddr)
        {
            CheckDisposed();

            InternalStart(beginAddr, endAddr, 0, 0);
        }

        /// <summary>
        /// Starts emulation at the specified begin address, end address, timeout and number of instructions
        /// to execute.
        /// </summary>
        /// <param name="begin">Address at which to begin emulation.</param>
        /// <param name="end">Address at which to end emulation.</param>
        /// <param name="timeout">Duration to run emulation.</param>
        /// <param name="count">Number of instructions to execute.</param>
        public void Start(ulong begin, ulong end, TimeSpan timeout, int count)
        {
            CheckDisposed();

            // Convert TimeSpan value into micro seconds.
            var microSeconds = (ulong)(Math.Round(timeout.TotalMilliseconds * 1000));
            InternalStart(begin, end, microSeconds, count);
        }

        /// <summary>
        /// Stops the emulation.
        /// </summary>
        public void Stop()
        {
            CheckDisposed();

            var err = UnicornLib.uc_emu_stop(_uc);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);
        }

        /// <summary>
        /// Finalizes the <see cref="Emulator"/> instance.
        /// </summary>
        ~Emulator()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Emulator"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all unmanaged and optionally managed resources used by the current instance of the <see cref="Emulator"/> class.
        /// </summary>
        /// <param name="disposing"><c>true</c> to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            //NOTE: Might consider throwing an exception here?
            var err = UnicornLib.uc_close(_uc);
            Debug.Assert(err == UnicornError.UC_ERR_OK, $"Disposal uc_close of Emulator instance did not return UC_ERR_OK, but {err}.");
            if (disposing)
            {
                // Dispose managed resources.
            }

            _disposed = true;
        }

        /// <summary>
        /// Determines if the <see cref="Emulator"/> instance object has been disposed;
        /// if it is, throws an <see cref="ObjectDisposedException"/>.
        /// </summary>
        protected internal void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Can not access disposed Emulator object.");
        }

        private void InternalStart(ulong begin, ulong end, ulong timeout, int count)
        {
            var err = UnicornLib.uc_emu_start(_uc, begin, end, timeout, count);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);
        }
    }
}
