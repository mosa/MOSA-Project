using System.Diagnostics.CodeAnalysis;

namespace System.Reflection;

public readonly struct CustomAttributeTypedArgument : IEquatable<CustomAttributeTypedArgument>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public Type ArgumentType
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
	}

	public CustomAttributeTypedArgument(object value)
	{
		throw null;
	}

	public CustomAttributeTypedArgument(Type argumentType, object? value)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(CustomAttributeTypedArgument other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
	{
		throw null;
	}

	public static bool operator !=(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
