/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;
using System.Runtime.InteropServices;

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
        [CompilerSupport(CompilerSupportFunction.Memset)]
        public unsafe static void Memset(byte* destination, byte value, int count)
        {
        }

        /// <summary>
        /// Copies bytes from the source to destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="source">The source.</param>
        /// <param name="count">The number of bytes to copy.</param>
        [CompilerSupport(CompilerSupportFunction.Memcpy)]
        public unsafe static void Memcpy(byte* destination, byte* source, int count)
        {
        }
    }
}
