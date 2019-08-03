using System;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Exception thrown when the unicorn-engine does not return UC_ERR_OK.
    /// </summary>
    public class UnicornException : Exception
    {
        /// <summary>
        /// Gets the <see cref="Binds.UnicornError"/> of the <see cref="UnicornException"/>.
        /// </summary>
        public UnicornError ErrorCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnicornException"/> class with the specified <see cref="Binds.UnicornError"/>.
        /// </summary>
        /// <param name="error"><see cref="Binds.UnicornError"/> error code.</param>
        public UnicornException(UnicornError error) : base(Bindings.Instance.StrError(error))
        {
            ErrorCode = error;
        }

        internal UnicornException(uc_err err) : this((UnicornError)err)
        {
            // Space
        }
    }
}
