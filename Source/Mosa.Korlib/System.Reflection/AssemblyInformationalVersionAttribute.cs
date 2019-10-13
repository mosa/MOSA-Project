// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyInformationalVersionAttribute : Attribute
	{
		public AssemblyInformationalVersionAttribute(string informationalVersion)
		{
			InformationalVersion = informationalVersion;
		}

		public string InformationalVersion { get; }
	}
}
