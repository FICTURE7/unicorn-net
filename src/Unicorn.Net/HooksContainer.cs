using System.Diagnostics;

namespace Unicorn
{
    /// <summary>
    /// Base class of hook containers.
    /// </summary>
    public abstract class HooksContainer
    {
        internal HooksContainer(Emulator emulator)
        {
            Debug.Assert(emulator != null);
            _emulator = emulator;
        }

        private readonly Emulator _emulator;

        /// <summary>
        /// <see cref="Unicorn.Emulator"/> instance which owns this <see cref="HooksContainer"/>.
        /// </summary>
        protected Emulator Emulator => _emulator;
    }
}
