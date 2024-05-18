using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LookupBindingPropertiesAttribute : Attribute
{
	public static readonly LookupBindingPropertiesAttribute Default;

	public string? DataSource
	{
		get
		{
			throw null;
		}
	}

	public string? DisplayMember
	{
		get
		{
			throw null;
		}
	}

	public string? LookupMember
	{
		get
		{
			throw null;
		}
	}

	public string? ValueMember
	{
		get
		{
			throw null;
		}
	}

	public LookupBindingPropertiesAttribute()
	{
	}

	public LookupBindingPropertiesAttribute(string dataSource, string displayMember, string valueMember, string lookupMember)
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
}
