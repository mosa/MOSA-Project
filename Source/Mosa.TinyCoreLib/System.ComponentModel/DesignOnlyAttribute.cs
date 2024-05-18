using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class DesignOnlyAttribute : Attribute
{
	public static readonly DesignOnlyAttribute Default;

	public static readonly DesignOnlyAttribute No;

	public static readonly DesignOnlyAttribute Yes;

	public bool IsDesignOnly
	{
		get
		{
			throw null;
		}
	}

	public DesignOnlyAttribute(bool isDesignOnly)
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
