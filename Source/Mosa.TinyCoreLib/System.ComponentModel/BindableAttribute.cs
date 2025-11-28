using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class BindableAttribute : Attribute
{
	public static readonly BindableAttribute Default;

	public static readonly BindableAttribute No;

	public static readonly BindableAttribute Yes;

	public bool Bindable
	{
		get
		{
			throw null;
		}
	}

	public BindingDirection Direction
	{
		get
		{
			throw null;
		}
	}

	public BindableAttribute(bool bindable)
	{
	}

	public BindableAttribute(bool bindable, BindingDirection direction)
	{
	}

	public BindableAttribute(BindableSupport flags)
	{
	}

	public BindableAttribute(BindableSupport flags, BindingDirection direction)
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
