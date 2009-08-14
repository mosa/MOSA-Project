/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.CompilerFramework {

	/// <summary>
	/// Marks a method as a jit trampoline.
	/// </summary>
	/// <remarks>
	/// The marked method is called in place of actual methods, which were not yet
	/// jitted to the native platform. Usually this attribute is applied to a method
	/// in the used Runtime along with a naked attribute, which prevents the method From
	/// having its own stack frame.
	/// <para/>
	/// The method, which was marked with the attribute must be static.
	/// </remarks>
	[ AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public sealed class JitTrampolineAttribute : Attribute {
	}
}
