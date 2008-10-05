/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// Marks a method as naked.
	/// </summary>
	/// <remarks>
	/// Naked methods do not get stack frames and register save/restore code inserted 
	/// for them. This is usually applied to special methods, which are invoked by
	/// native code or in special circumstances.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public sealed class NakedAttribute : Attribute {
        /// <summary>
        /// Initializes a new instance of <see cref="NakedAttribute"/>.
        /// </summary>
		public NakedAttribute()
		{
		}
	}
}
