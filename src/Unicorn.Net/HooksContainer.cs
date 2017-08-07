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
            Emulator = emulator;
        }

        /// <summary>
        /// <see cref="Unicorn.Emulator"/> instance which owns this <see cref="HooksContainer"/>.
        /// </summary>
        protected readonly Emulator Emulator;
    }
}
