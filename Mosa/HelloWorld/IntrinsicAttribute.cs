/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 */

using System;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class IntrinsicAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IntrinsicAttribute"/> class.
		/// </summary>
		public IntrinsicAttribute()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IntrinsicAttribute"/> class.
		/// </summary>
		public IntrinsicAttribute(Type architecture, Type instructionType)
		{
		}
	}
}
