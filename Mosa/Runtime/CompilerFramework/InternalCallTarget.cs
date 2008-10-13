/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// An enumeration of runtime/architecture provided call targets.
    /// </summary>
    public enum InternalCallTarget
    {
        /// <summary>
        /// The method call represents a runtime defined memory set method.
        /// </summary>
        /// <remarks>
        /// The memset method is similar to the memset function in C runtime libraries. It fills a block 
        /// of memory with a specific value.
        /// </remarks>
        Memset,

        /// <summary>
        /// The method call represents a runtime defined memory copy method.
        /// </summary>
        /// <remarks>
        /// The memcpy method is similar to the memcpy function in C runtime libraries. It copies the
        /// specified number of bytes from a source to a destination block.
        /// </remarks>
        Memcpy
    }
}
