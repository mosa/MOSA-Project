// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			Target = target;
		}
	}
}
