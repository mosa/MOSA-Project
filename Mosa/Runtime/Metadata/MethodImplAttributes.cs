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

namespace Mosa.Runtime.Metadata {
    /// <summary>
    /// Specifies flags for the attributes of a method implementation.
    /// </summary>
	public enum MethodImplAttributes {
        /// <summary>
        /// Specifies that the method implementation is managed, otherwise unmanaged.
        /// </summary>
		Managed = 0,
        /// <summary>
        /// Specifies that the method implementation is in Microsoft intermediate language
        ///     (MSIL).
        /// </summary>
		IL = 0,
        /// <summary>
        /// Specifies that the method implementation is native.
        /// </summary>
		Native = 1,
        /// <summary>
        /// Specifies that the method implementation is in Optimized Intermediate Language (OPTIL).
        /// </summary>
		OPTIL = 2,
        /// <summary>
        /// Specifies that the method implementation is provided by the runtime.
        /// </summary>
		Runtime = 3,
        /// <summary>
        /// Specifies flags about code type.
        /// </summary>
		CodeTypeMask = 3,
        /// <summary>
        /// Specifies whether the code is managed or unmanaged.
        /// </summary>
		ManagedMask = 4,
        /// <summary>
        /// Specifies that the method implementation is unmanaged, otherwise managed.
        /// </summary>
		Unmanaged = 4,
        /// <summary>
        /// Specifies that the method cannot be inlined.
        /// </summary>
		NoInlining = 8,
        /// <summary>
        /// Specifies that the method is not defined.
        /// </summary>
		ForwardRef = 16,    
        /// <summary>
        /// Specifies that the method is single-threaded through the body. Static methods
        ///     (Shared in Visual Basic) lock on the type, while instance methods lock on
        ///     the instance. You can also use the C# lock statement or the Visual Basic
        ///     Lock function for this purpose.
        /// </summary>
		Synchronized = 32,
        /// <summary>
        /// Specifies that the method signature is exported exactly as declared.
        /// </summary>
		PreserveSig = 128,
        /// <summary>
        /// Specifies an internal call.
        /// </summary>
		InternalCall = 4096,
        /// <summary>
        /// Specifies a range check value.
        /// </summary>
		MaxMethodImplVal = 65535,
	}
}
