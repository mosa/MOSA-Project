using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class ReadOnlyAttribute : Attribute
{
	public static readonly ReadOnlyAttribute Default;

	public static readonly ReadOnlyAttribute No;

	public static readonly ReadOnlyAttribute Yes;

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyAttribute(bool isReadOnly)
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
}
