using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DefaultBindingPropertyAttribute : Attribute
{
	public static readonly DefaultBindingPropertyAttribute Default;

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public DefaultBindingPropertyAttribute()
	{
	}

	public DefaultBindingPropertyAttribute(string? name)
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
