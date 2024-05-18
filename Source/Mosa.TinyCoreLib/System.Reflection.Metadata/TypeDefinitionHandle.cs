namespace System.Reflection.Metadata;

public readonly struct TypeDefinitionHandle : IEquatable<TypeDefinitionHandle>
{
	private readonly int _dummyPrimitive;

	public bool IsNil
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public bool Equals(TypeDefinitionHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(TypeDefinitionHandle left, TypeDefinitionHandle right)
	{
		throw null;
	}

	public static explicit operator TypeDefinitionHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator TypeDefinitionHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(TypeDefinitionHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(TypeDefinitionHandle handle)
	{
		throw null;
	}

	public static bool operator !=(TypeDefinitionHandle left, TypeDefinitionHandle right)
	{
		throw null;
	}
}
