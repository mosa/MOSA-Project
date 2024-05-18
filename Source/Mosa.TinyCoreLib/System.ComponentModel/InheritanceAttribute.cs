using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
public sealed class InheritanceAttribute : Attribute
{
	public static readonly InheritanceAttribute Default;

	public static readonly InheritanceAttribute Inherited;

	public static readonly InheritanceAttribute InheritedReadOnly;

	public static readonly InheritanceAttribute NotInherited;

	public InheritanceLevel InheritanceLevel
	{
		get
		{
			throw null;
		}
	}

	public InheritanceAttribute()
	{
	}

	public InheritanceAttribute(InheritanceLevel inheritanceLevel)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override bool IsDefaultAttribute()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
