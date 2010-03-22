/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
    // FIXME: Move this to the x86 platform
    // FIXME: Realize these methods (take GCC 4.0 ASM implementations)
    // FIXME: Use simple ptr code for 0.1
    // FIXME: Use the Native class to realize the operations - only when the code will run inside our own runtime, jitted

    /// <summary>
    /// Provides compiler support functions.
    /// </summary>
    [InternalCallType]
    public static class CompilerSupport
    {
        /// <summary>
        /// Fills the destination with <paramref name="value"/>.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="value">The value.</param>
        /// <param name="count">The number of bytes to fill.</param>
        [VmCall(VmCall.Memset)]
        public unsafe static void Memset(byte* destination, byte value, int count)
        {
        }

        /// <summary>
        /// Copies bytes from the source to destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="source">The source.</param>
        /// <param name="count">The number of bytes to copy.</param>
        [VmCall(VmCall.Memset)]
        public unsafe static void Memcpy(byte* destination, byte* source, int count)
        {
            for (int i = 0; i < count; ++i)
                *destination++ = *source++;
        }
    }
}
