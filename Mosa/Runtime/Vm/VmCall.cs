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

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// An enumeration used to identify icalls provided by the MOSA VM.
    /// </summary>
    public enum VmCall
    {
        /// <summary>
        /// Allocates memory for a new array or object instance.
        /// </summary>
        Allocate,

        /// <summary>
        /// Boxes a value type.
        /// </summary>
        Box,

        /// <summary>
        /// Casts an object references to a specific type.
        /// </summary>
        Castclass,

        /// <summary>
        /// Loads the address of a function.
        /// </summary>
        GetFunctionPtr,

        /// <summary>
        /// Retrieves a handle for the specified token.
        /// </summary>
        GetHandleForToken,

        /// <summary>
        /// Loads the address of a virtual function.
        /// </summary>
        GetVirtualFunctionPtr,

        /// <summary>
        /// Determines the instance of the type.
        /// </summary>
        IsInstanceOfType,

        /// <summary>
        /// The method call represents a runtime defined memory copy method.
        /// </summary>
        /// <remarks>
        /// The memcpy method is similar to the memcpy function in C runtime libraries. It copies the
        /// specified number of bytes From a source to a destination block.
        /// </remarks>
        Memcpy,

        /// <summary>
        /// The method call represents a runtime defined memory set method.
        /// </summary>
        /// <remarks>
        /// The memset method is similar to the memset function in C runtime libraries. It fills a block 
        /// of memory with a specific value.
        /// </remarks>
        Memset,

        /// <summary>
        /// Rethrows the given exception.
        /// </summary>
        Rethrow,

        /// <summary>
        /// Throws the given exception.
        /// </summary>
        Throw,

        /// <summary>
        /// Unboxes a value type.
        /// </summary>
        Unbox,
    }
}
