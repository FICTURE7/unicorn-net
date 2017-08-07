﻿using System;
using System.Diagnostics;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Represents a unicorn-engine emulator.
    /// </summary>
    public abstract class Emulator : IDisposable
    {
        internal Emulator(uc_arch arch, uc_mode mode)
        {
            var uc = UIntPtr.Zero;
            var err = unicorn.uc_open(arch, mode, ref uc);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);

            _uc = uc;
            _arch = arch;
            _mode = mode;
            _memory = new Memory(this);
            _hooks = new Hooks(this);
        }

        // To determine if we've been disposed or not.
        private bool _disposed;
        // Memory object instance which represents the memory of the emulator.
        private readonly Memory _memory;
        // Hooks object instance which represents the hooks of the emulator.
        private readonly Hooks _hooks;

        // Arch with which the Emulator instance was initialized.
        internal readonly uc_arch _arch;
        // Mode with which the Emulator instance was initialized.
        internal readonly uc_mode _mode;
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
        /// Gets the <see cref="Unicorn.Hooks"/> of the <see cref="Emulator"/>.
        /// </summary>
        public Hooks Hooks
        {
            get
            {
                CheckDisposed();

                return _hooks;
            }
        }

        /// <summary>
        /// Gets the mode of the <see cref="Emulator"/>.
        /// </summary>
        public int Mode
        {
            get
            {
                CheckDisposed();

                // -> Unicorn returns UC_ERR_ARGS when the emulator is not in arm?

                /*
                var mode = 0;
                var err = UnicornLib.uc_query(_uc, UnicornQuery.UC_QUERY_MODE, ref mode);
                if (err != UnicornError.UC_ERR_OK)
                    throw new UnicornException(err);
                Debug.Assert(mode == (int)_mode);
                */

                return (int)_mode;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Unicorn.Context"/> of the <see cref="Emulator"/> instance.
        /// </summary>
        public Context Context
        {
            get
            {
                CheckDisposed();

                //TODO: Make contexts reusable so we don't create new instances and do unneeded allocations?

                var context = new Context(this);
                context.Capture(this);
                return context;
            }
            set
            {
                CheckDisposed();

                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                if (value._disposed)
                    throw new ObjectDisposedException(null, "Can not access disposed Context object.");
                if (value._arch != _arch || value._mode != _mode)
                    throw new ArgumentException("value must have same arch and mode as the Emulator instance.", nameof(value));

                value.Restore(this);
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

            var err = unicorn.uc_emu_stop(_uc);
            if (err != uc_err.UC_ERR_OK)
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
            var err = unicorn.uc_close(_uc);
            Debug.Assert(err == uc_err.UC_ERR_OK, $"Disposal uc_close of Emulator instance did not return UC_ERR_OK, but {err}.");

            _disposed = true;
        }

        internal void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Can not access disposed Emulator object.");
        }

        private void InternalStart(ulong begin, ulong end, ulong timeout, int count)
        {
            var err = unicorn.uc_emu_start(_uc, begin, end, timeout, count);
            if (err != uc_err.UC_ERR_OK)
                throw new UnicornException(err);
        }
    }
}
