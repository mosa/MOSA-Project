using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DefaultPropertyAttribute : Attribute
{
	public static readonly DefaultPropertyAttribute Default;

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public DefaultPropertyAttribute(string? name)
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
