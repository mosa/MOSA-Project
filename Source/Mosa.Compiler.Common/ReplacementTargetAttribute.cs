using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Compiler.Common
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
