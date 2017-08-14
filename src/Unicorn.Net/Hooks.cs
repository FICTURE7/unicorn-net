using System.Diagnostics;

namespace Unicorn
{
    /// <summary>
    /// Represents the hooks of an <see cref="Emulator"/>.
    /// </summary>
    public class Hooks
    {
        internal Hooks(Emulator emulator)
        {
            Debug.Assert(emulator != null);

            _emulator = emulator;
            _memoryHooks = new MemoryHooksContainer(emulator);
            _codeHooks = new CodeHooksContainer(emulator);
            _blockHooks = new BlockHooksContainer(emulator);
        }

        // Emulator instance which owns this Hooks object instance.
        private readonly Emulator _emulator;
        private readonly MemoryHooksContainer _memoryHooks;
        private readonly CodeHooksContainer _codeHooks;
        private readonly BlockHooksContainer _blockHooks;

        /// <summary>
        /// Gets the <see cref="MemoryHooksContainer"/> of the <see cref="Emulator"/>.
        /// </summary>
        public MemoryHooksContainer Memory
        {
            get
            {
                _emulator.CheckDisposed();

                return _memoryHooks;
            }
        }

        /// <summary>
        /// Gets the <see cref="CodeHooksContainer"/> of the <see cref="Emulator"/>.
        /// </summary>
        public CodeHooksContainer Code
        {
            get
            {
                _emulator.CheckDisposed();

                return _codeHooks;
            }
        }

        /// <summary>
        /// Gets the <see cref="BlockHooksContainer"/> of the <see cref="Emulator"/>.
        /// </summary>
        public BlockHooksContainer Block
        {
            get
            {
                _emulator.CheckDisposed();

                return _blockHooks;
            }
        }
    } 
}
