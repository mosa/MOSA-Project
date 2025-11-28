using System.Diagnostics.CodeAnalysis;

namespace System.Reflection;

public readonly struct CustomAttributeNamedArgument : IEquatable<CustomAttributeNamedArgument>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public bool IsField
	{
		get
		{
			throw null;
		}
	}

	public MemberInfo MemberInfo
	{
		get
		{
			throw null;
		}
	}

	public string MemberName
	{
		get
		{
			throw null;
		}
	}

	public CustomAttributeTypedArgument TypedValue
	{
		get
		{
			throw null;
		}
	}

	public CustomAttributeNamedArgument(MemberInfo memberInfo, object? value)
	{
		throw null;
	}

	public CustomAttributeNamedArgument(MemberInfo memberInfo, CustomAttributeTypedArgument typedArgument)
	{
		throw null;
	}

	public bool Equals(CustomAttributeNamedArgument other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
	{
		throw null;
	}

	public static bool operator !=(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
