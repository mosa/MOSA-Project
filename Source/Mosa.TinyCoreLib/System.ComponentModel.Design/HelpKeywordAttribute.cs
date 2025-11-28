using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.Design;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
public sealed class HelpKeywordAttribute : Attribute
{
	public static readonly HelpKeywordAttribute Default;

	public string? HelpKeyword
	{
		get
		{
			throw null;
		}
	}

	public HelpKeywordAttribute()
	{
	}

	public HelpKeywordAttribute(string keyword)
	{
	}

	public HelpKeywordAttribute(Type t)
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
