// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Security
{
	[AttributeUsage(AttributeTargets.Module, AllowMultiple = true, Inherited = false)]
	public sealed class UnverifiableCodeAttribute : Attribute
	{
	}
}
