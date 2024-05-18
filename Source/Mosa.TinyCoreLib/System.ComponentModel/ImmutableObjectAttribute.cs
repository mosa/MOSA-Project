using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class ImmutableObjectAttribute : Attribute
{
	public static readonly ImmutableObjectAttribute Default;

	public static readonly ImmutableObjectAttribute No;

	public static readonly ImmutableObjectAttribute Yes;

	public bool Immutable
	{
		get
		{
			throw null;
		}
	}

	public ImmutableObjectAttribute(bool immutable)
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
