using System;

namespace Unicorn
{
    /// <summary>
    /// Represents a hook handle.
    /// </summary>
    public struct HookHandle
    {
        internal readonly IntPtr _hh;
        private readonly Delegate _hCallback;

        internal HookHandle(IntPtr hh, Delegate d)
        {
            _hh = hh;
            _hCallback = d;
        }
    }
}
