using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DefaultEventAttribute : Attribute
{
	public static readonly DefaultEventAttribute Default;

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public DefaultEventAttribute(string? name)
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
