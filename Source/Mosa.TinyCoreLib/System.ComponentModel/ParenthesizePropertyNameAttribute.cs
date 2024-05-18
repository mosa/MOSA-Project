using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class ParenthesizePropertyNameAttribute : Attribute
{
	public static readonly ParenthesizePropertyNameAttribute Default;

	public bool NeedParenthesis
	{
		get
		{
			throw null;
		}
	}

	public ParenthesizePropertyNameAttribute()
	{
	}

	public ParenthesizePropertyNameAttribute(bool needParenthesis)
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
