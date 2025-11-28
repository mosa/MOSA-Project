using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class ListBindableAttribute : Attribute
{
	public static readonly ListBindableAttribute Default;

	public static readonly ListBindableAttribute No;

	public static readonly ListBindableAttribute Yes;

	public bool ListBindable
	{
		get
		{
			throw null;
		}
	}

	public ListBindableAttribute(bool listBindable)
	{
	}

	public ListBindableAttribute(BindableSupport flags)
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
