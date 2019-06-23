// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyFileVersionAttribute : Attribute
	{
		public AssemblyFileVersionAttribute(string version)
		{
			Version = version ?? throw new ArgumentNullException(nameof(version));
		}

		public string Version { get; }
	}
}
