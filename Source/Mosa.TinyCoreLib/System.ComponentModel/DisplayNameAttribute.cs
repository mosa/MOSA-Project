using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
public class DisplayNameAttribute : Attribute
{
	public static readonly DisplayNameAttribute Default;

	public virtual string DisplayName
	{
		get
		{
			throw null;
		}
	}

	protected string DisplayNameValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DisplayNameAttribute()
	{
	}

	public DisplayNameAttribute(string displayName)
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
