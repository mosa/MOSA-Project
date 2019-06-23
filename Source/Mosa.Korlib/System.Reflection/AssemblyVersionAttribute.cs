// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyVersionAttribute : Attribute
	{
		public AssemblyVersionAttribute(string version)
		{
			Version = version;
		}

		public string Version { get; }
	}
}
