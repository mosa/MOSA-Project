using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ComplexBindingPropertiesAttribute : Attribute
{
	public static readonly ComplexBindingPropertiesAttribute Default;

	public string? DataMember
	{
		get
		{
			throw null;
		}
	}

	public string? DataSource
	{
		get
		{
			throw null;
		}
	}

	public ComplexBindingPropertiesAttribute()
	{
	}

	public ComplexBindingPropertiesAttribute(string? dataSource)
	{
	}

	public ComplexBindingPropertiesAttribute(string? dataSource, string? dataMember)
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
