// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCompanyAttribute : Attribute
	{
		public AssemblyCompanyAttribute(string company)
		{
			Company = company;
		}

		public string Company { get; }
	}
}
