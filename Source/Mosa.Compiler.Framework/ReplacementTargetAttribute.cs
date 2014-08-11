/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Used for defining targets when using intrinsic replacements
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ReplacementTargetAttribute : Attribute
	{
		public string Target
		{
			get;
			private set;
		}

		public ReplacementTargetAttribute(string target)
		{
			this.Target = target;
		}
	}
}