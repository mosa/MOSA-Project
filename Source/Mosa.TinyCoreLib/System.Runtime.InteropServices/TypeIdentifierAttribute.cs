namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
public sealed class TypeIdentifierAttribute : Attribute
{
	public string? Identifier
	{
		get
		{
			throw null;
		}
	}

	public string? Scope
	{
		get
		{
			throw null;
		}
	}

	public TypeIdentifierAttribute()
	{
	}

	public TypeIdentifierAttribute(string? scope, string? identifier)
	{
	}
}
