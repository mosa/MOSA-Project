using System.Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices;

[Embedded]
[AttributeUsage(AttributeTargets.Module, AllowMultiple = false, Inherited = false)]
public sealed class RefSafetyRulesAttribute : Attribute
{
	public readonly int Version;

	public RefSafetyRulesAttribute(int version)
	{
		Version = version;
	}
}
