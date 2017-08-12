using System;

namespace Unicorn
{
    /// <summary>
    /// Represents a hook handle.
    /// </summary>
    public struct HookHandle
    {
        internal HookHandle(IntPtr hh)
        {
            _hh = hh;
        }

        internal readonly IntPtr _hh;
    }
}
