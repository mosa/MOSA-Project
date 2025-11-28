using System.ComponentModel;

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Module, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class NullablePublicOnlyAttribute : Attribute
{
	public readonly bool IncludesInternals;

	public NullablePublicOnlyAttribute(bool value)
	{
	}
}
