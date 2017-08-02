using System;
using System.Diagnostics;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Represents an <see cref="Emulator"/> context.
    /// </summary>
    public class Context : IDisposable
    {
        internal Context(Emulator emulator, UIntPtr context)
        {
            Debug.Assert(emulator != null);
            Debug.Assert(context != UIntPtr.Zero);

            _arch = emulator._arch;
            _mode = emulator._mode;
            _context = context;
        }

        internal bool _disposed;
        internal readonly UnicornArch _arch;
        internal readonly UnicornMode _mode;
        internal readonly UIntPtr _context;

        // No real need for the dispose pattern here, but it
        // does not hurt.

        /// <summary>
        /// Finalizes the <see cref="Context"/> instance.
        /// </summary>
        ~Context()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Context"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all unmanaged and optionally managed resources used by the current instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="disposing"><c>true</c> to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            var err = UnicornLib.uc_free(_context);
            Debug.Assert(err == UnicornError.UC_ERR_OK);

            _disposed = true;
        }
    }
}
