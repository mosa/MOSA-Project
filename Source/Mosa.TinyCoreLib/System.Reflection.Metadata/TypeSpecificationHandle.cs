namespace System.Reflection.Metadata;

public readonly struct TypeSpecificationHandle : IEquatable<TypeSpecificationHandle>
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

	public bool Equals(TypeSpecificationHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(TypeSpecificationHandle left, TypeSpecificationHandle right)
	{
		throw null;
	}

	public static explicit operator TypeSpecificationHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator TypeSpecificationHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(TypeSpecificationHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(TypeSpecificationHandle handle)
	{
		throw null;
	}

	public static bool operator !=(TypeSpecificationHandle left, TypeSpecificationHandle right)
	{
		throw null;
	}
}
