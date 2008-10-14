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
        /// Determines the instance of the type.
        /// </summary>
        IsInstanceOfType,

        /// <summary>
        /// Casts an object references to a specific type.
        /// </summary>
        Castclass,

        /// <summary>
        /// Boxes a value type.
        /// </summary>
        Box,

        /// <summary>
        /// Unboxes a value type.
        /// </summary>
        Unbox,

        /// <summary>
        /// Allocates memory for a new instance of an object.
        /// </summary>
        Newobj,

        /// <summary>
        /// Allocates memory for a new array instance.
        /// </summary>
        Newarr
    }
}
