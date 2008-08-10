/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata {
	// Summary:
	//     Specifies flags for the attributes of a method implementation.
	public enum MethodImplAttributes {
		// Summary:
		//     Specifies that the method implementation is managed, otherwise unmanaged.
		Managed = 0,
		//
		// Summary:
		//     Specifies that the method implementation is in Microsoft intermediate language
		//     (MSIL).
		IL = 0,
		//
		// Summary:
		//     Specifies that the method implementation is native.
		Native = 1,
		//
		// Summary:
		//     Specifies that the method implementation is in Optimized Intermediate Language
		//     (OPTIL).
		OPTIL = 2,
		//
		// Summary:
		//     Specifies that the method implementation is provided by the runtime.
		Runtime = 3,
		//
		// Summary:
		//     Specifies flags about code type.
		CodeTypeMask = 3,
		//
		// Summary:
		//     Specifies whether the code is managed or unmanaged.
		ManagedMask = 4,
		//
		// Summary:
		//     Specifies that the method implementation is unmanaged, otherwise managed.
		Unmanaged = 4,
		//
		// Summary:
		//     Specifies that the method cannot be inlined.
		NoInlining = 8,
		//
		// Summary:
		//     Specifies that the method is not defined.
		ForwardRef = 16,
		//
		// Summary:
		//     Specifies that the method is single-threaded through the body. Static methods
		//     (Shared in Visual Basic) lock on the type, while instance methods lock on
		//     the instance. You can also use the C# lock statement or the Visual Basic
		//     Lock function for this purpose.
		Synchronized = 32,
		//
		// Summary:
		//     Specifies that the method signature is exported exactly as declared.
		PreserveSig = 128,
		//
		// Summary:
		//     Specifies an internal call.
		InternalCall = 4096,
		//
		// Summary:
		//     Specifies a range check value.
		MaxMethodImplVal = 65535,
	}
}
