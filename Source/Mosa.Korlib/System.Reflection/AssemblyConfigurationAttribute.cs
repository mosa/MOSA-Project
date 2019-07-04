// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyConfigurationAttribute : Attribute
	{
		public AssemblyConfigurationAttribute(string configuration)
		{
			Configuration = configuration;
		}

		public string Configuration { get; }
	}
}
