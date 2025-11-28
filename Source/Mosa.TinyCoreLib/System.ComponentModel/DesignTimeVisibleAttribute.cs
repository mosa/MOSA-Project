using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class DesignTimeVisibleAttribute : Attribute
{
	public static readonly DesignTimeVisibleAttribute Default;

	public static readonly DesignTimeVisibleAttribute No;

	public static readonly DesignTimeVisibleAttribute Yes;

	public bool Visible
	{
		get
		{
			throw null;
		}
	}

	public DesignTimeVisibleAttribute()
	{
	}

	public DesignTimeVisibleAttribute(bool visible)
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
