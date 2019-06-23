// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCultureAttribute : Attribute
	{
		public AssemblyCultureAttribute(string culture)
		{
			Culture = culture;
		}

		public string Culture { get; }
	}
}
