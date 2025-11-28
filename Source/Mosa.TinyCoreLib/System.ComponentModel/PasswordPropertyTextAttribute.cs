using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class PasswordPropertyTextAttribute : Attribute
{
	public static readonly PasswordPropertyTextAttribute Default;

	public static readonly PasswordPropertyTextAttribute No;

	public static readonly PasswordPropertyTextAttribute Yes;

	public bool Password
	{
		get
		{
			throw null;
		}
	}

	public PasswordPropertyTextAttribute()
	{
	}

	public PasswordPropertyTextAttribute(bool password)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? o)
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
