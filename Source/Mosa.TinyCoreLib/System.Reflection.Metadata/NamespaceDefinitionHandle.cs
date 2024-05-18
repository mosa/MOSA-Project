using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct NamespaceDefinitionHandle : IEquatable<NamespaceDefinitionHandle>
{
	private readonly int _dummyPrimitive;

	public bool IsNil
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(NamespaceDefinitionHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(NamespaceDefinitionHandle left, NamespaceDefinitionHandle right)
	{
		throw null;
	}

	public static explicit operator NamespaceDefinitionHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator Handle(NamespaceDefinitionHandle handle)
	{
		throw null;
	}

	public static bool operator !=(NamespaceDefinitionHandle left, NamespaceDefinitionHandle right)
	{
		throw null;
	}
}
