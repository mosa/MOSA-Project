using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NotifyParentPropertyAttribute : Attribute
{
	public static readonly NotifyParentPropertyAttribute Default;

	public static readonly NotifyParentPropertyAttribute No;

	public static readonly NotifyParentPropertyAttribute Yes;

	public bool NotifyParent
	{
		get
		{
			throw null;
		}
	}

	public NotifyParentPropertyAttribute(bool notifyParent)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
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
}
