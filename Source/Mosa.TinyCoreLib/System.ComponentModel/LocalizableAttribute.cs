using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class LocalizableAttribute : Attribute
{
	public static readonly LocalizableAttribute Default;

	public static readonly LocalizableAttribute No;

	public static readonly LocalizableAttribute Yes;

	public bool IsLocalizable
	{
		get
		{
			throw null;
		}
	}

	public LocalizableAttribute(bool isLocalizable)
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
