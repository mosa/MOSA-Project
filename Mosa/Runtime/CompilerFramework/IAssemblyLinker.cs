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

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Collects linker requests for processing in the AssemblyLinkerStage.
    /// </summary>
    /// <remarks>
    /// The assembly linker collector performs runtime specific requests in order to resolve a metadata object
    /// to its physical address in memory. All link requests require the metadata object, the request address
    /// and a relative flag. These are used to either resolve the request immediately or patch the code during
    /// a later linker stage, when all methods and fields have been compiled.
    /// <para/>
    /// The methods return a long instead of IntPtr to allow cross-compilation for 64-bit on a 32-bit system.
    /// </remarks>
    public interface IAssemblyLinker
    {
        /// <summary>
        /// Issues a linker request for the given runtime method.
        /// </summary>
        /// <param name="member">The method or static field to link against.</param>
        /// <param name="address">The address, where the retrieved address is stored in.</param>
        /// <param name="relativeBase">The base address, if a relative link is required.</param>
        /// <returns>
        /// The return value is the preliminary address to place in the generated machine 
        /// code. On 32-bit systems, only the lower 32 bits are valid. The above are not used. An implementation of
        /// IAssemblyLinker may not rely on 64-bits being stored in the memory defined by position.
        /// </returns>
        long Link(RuntimeMember member, long address, long relativeBase);
    }
}
