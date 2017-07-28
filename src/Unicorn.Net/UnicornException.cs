using System;
using System.Runtime.InteropServices;
using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Exception thrown when the native unicorn library does not return UC_ERR_OK.
    /// </summary>
    public class UnicornException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnicornException"/> class.
        /// </summary>
        public UnicornException() : base()
        {
            // Space
        }
        
        /// <summary>
        /// Initializes a new instance with the specified message describing the cause of the exception.
        /// </summary>
        /// <param name="message">Message describing the cause of the exception.</param>
        public UnicornException(string message) : base(message)
        {
            // Space
        }

        internal UnicornException(UnicornError err) : base(GetUnicornErrorString(err))
        {
            // Space
        }

        private static string GetUnicornErrorString(UnicornError err)
        {
            var ptr = UnicornLib.uc_strerror(err);
            var errString = Marshal.PtrToStringAnsi(ptr);
            return errString;
        }
    }
}
