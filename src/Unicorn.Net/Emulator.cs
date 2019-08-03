using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unicorn
{
    /// <summary>
    /// Represents a unicorn-engine emulator.
    /// </summary>
    public abstract class Emulator : IDisposable
    {
        private bool _disposed;
        private readonly Memory _memory;
        private readonly Hooks _hooks;
        private readonly IntPtr _handle;

        internal readonly UnicornArch _arch;
        internal readonly UnicornMode _mode;

        /// <summary>
        /// Gets the handle of the <see cref="Emulator"/>.
        /// </summary>
        internal IntPtr Handle => _handle;

        /// <summary>
        /// Gets the <see cref="IBindings"/> of the <see cref="Emulator"/>.
        /// </summary>
        internal IBindings Bindings { get; }

        /// <summary>
        /// Gets the <see cref="Unicorn.Memory"/> of the <see cref="Emulator"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public Memory Memory
        {
            get
            {
                ThrowIfDisposed();
                return _memory;
            }
        }

        /// <summary>
        /// Gets the <see cref="Unicorn.Hooks"/> of the <see cref="Emulator"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public Hooks Hooks
        {
            get
            {
                ThrowIfDisposed();
                return _hooks;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Unicorn.Context"/> of the <see cref="Emulator"/> instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> has a differnt mode or architecture than the <see cref="Emulator"/>.</exception>
        /// <exception cref="UnicornException">Unicorn did not return <see cref="UnicornError.Ok"/>.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public Context Context
        {
            get
            {
                ThrowIfDisposed();

                var context = new Context(this);
                context.Capture(this);
                return context;
            }
            set
            {
                ThrowIfDisposed();

                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                if (value._disposed)
                    throw new ObjectDisposedException(null, "Can not access disposed Context object.");
                if (value._arch != _arch || value._mode != _mode)
                    throw new ArgumentException("value must have same arch and mode as the Emulator instance.", nameof(value));

                value.Restore(this);
            }
        }

        internal Emulator(UnicornArch arch, UnicornMode mode) : this(arch, mode, Unicorn.Bindings.Instance)
        {
            /* Space. */
        }

        internal Emulator(UnicornArch arch, UnicornMode mode, IBindings bindings)
        {
            _arch = arch;
            _mode = mode;
            _memory = new Memory(this);
            _hooks = new Hooks(this);

            Bindings = bindings;
            Bindings.Open(arch, mode, ref _handle);
        }
        
        /// <summary>
        /// Starts emulation at the specified begin address and end address.
        /// </summary>
        /// <param name="begin">Address at which to begin emulation.</param>
        /// <param name="end">Address at which to end emulation.</param>
        /// <exception cref="UnicornException">Unicorn did not return <see cref="UnicornError.Ok"/>.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public void Start(ulong begin, ulong end)
        {
            ThrowIfDisposed();
            Bindings.EmuStart(Handle, begin, end, 0, 0);
        }

        /// <summary>
        /// Starts emulation at the specified begin address, end address, timeout and number of instructions to execute.
        /// </summary>
        /// <param name="begin">Address at which to begin emulation.</param>
        /// <param name="end">Address at which to end emulation.</param>
        /// <param name="timeout">Duration to run emulation.</param>
        /// <param name="count">Number of instructions to execute.</param>
        /// <exception cref="UnicornException">Unicorn did not return <see cref="UnicornError.Ok"/>.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public void Start(ulong begin, ulong end, TimeSpan timeout, int count)
        {
            ThrowIfDisposed();
            var microSeconds = (ulong)Math.Round(timeout.TotalMilliseconds * 1000);
            Bindings.EmuStart(Handle, begin, end, microSeconds, count);
        }

        /// <summary>
        /// Stops the emulation.
        /// </summary>
        /// <exception cref="UnicornException">Unicorn did not return <see cref="UnicornError.Ok"/>.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Emulator"/> instance is disposed.</exception>
        public void Stop()
        {
            ThrowIfDisposed();
            Bindings.EmuStop(Handle);
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

            try { Bindings.Close(Handle); }
            catch
            {
                Debug.WriteLine("Bindings.Close() threw an exception.");
            }

            _disposed = true;
        }

        internal void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Can not access disposed Emulator object.");
        }

        internal void EmuStart()
        {

        }

        internal void EmuStop()
        {

        }
    }
}
