using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class BrowsableAttribute : Attribute
{
	public static readonly BrowsableAttribute Default;

	public static readonly BrowsableAttribute No;

	public static readonly BrowsableAttribute Yes;

	public bool Browsable
	{
		get
		{
			throw null;
		}
	}

	public BrowsableAttribute(bool browsable)
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
