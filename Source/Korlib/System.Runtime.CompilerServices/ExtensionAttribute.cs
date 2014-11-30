/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System.Runtime.CompilerServices
{
	/// <summary>
	/// Indicates that a method is an extension method, or that a class or assembly contains extension methods.
	/// </summary>
	[AttributeUsageAttribute(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class ExtensionAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionAttribute"/> class.
		/// </summary>
		public ExtensionAttribute()
		{
		}
	}
}