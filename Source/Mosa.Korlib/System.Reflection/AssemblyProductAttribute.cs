// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyProductAttribute : Attribute
{
	public AssemblyProductAttribute(string product)
	{
		Product = product;
	}

	public string Product { get; }
}
