// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCopyrightAttribute : Attribute
	{
		public AssemblyCopyrightAttribute(string copyright)
		{
			Copyright = copyright;
		}

		public string Copyright { get; }
	}
}
