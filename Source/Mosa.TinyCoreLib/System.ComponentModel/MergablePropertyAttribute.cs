using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class MergablePropertyAttribute : Attribute
{
	public static readonly MergablePropertyAttribute Default;

	public static readonly MergablePropertyAttribute No;

	public static readonly MergablePropertyAttribute Yes;

	public bool AllowMerge
	{
		get
		{
			throw null;
		}
	}

	public MergablePropertyAttribute(bool allowMerge)
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
