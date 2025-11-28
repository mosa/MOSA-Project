using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public class DescriptionAttribute : Attribute
{
	public static readonly DescriptionAttribute Default;

	public virtual string Description
	{
		get
		{
			throw null;
		}
	}

	protected string DescriptionValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DescriptionAttribute()
	{
	}

	public DescriptionAttribute(string description)
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
