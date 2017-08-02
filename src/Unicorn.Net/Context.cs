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
        internal Context(Emulator emulator)
        {
            Debug.Assert(emulator != null);

            var err = UnicornLib.uc_context_alloc(emulator._uc, ref _context);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);

            _arch = emulator._arch;
            _mode = emulator._mode;
        }

        internal bool _disposed;
        internal readonly UnicornArch _arch;
        internal readonly UnicornMode _mode;
        internal readonly UIntPtr _context;

        internal void Capture(Emulator emulator)
        {
            Debug.Assert(emulator != null);
            Debug.Assert(emulator._arch == _arch);
            Debug.Assert(emulator._mode == _mode);

            var err = UnicornLib.uc_context_save(emulator._uc, _context);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);
        }

        internal void Restore(Emulator emulator)
        {
            Debug.Assert(emulator != null);
            Debug.Assert(emulator._arch == _arch);
            Debug.Assert(emulator._mode == _mode);

            var err = UnicornLib.uc_context_restore(emulator._uc, _context);
            if (err != UnicornError.UC_ERR_OK)
                throw new UnicornException(err);
        }

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
