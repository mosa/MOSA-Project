namespace System.Reflection.Metadata;

public readonly struct TypeReferenceHandle : IEquatable<TypeReferenceHandle>
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

	public bool Equals(TypeReferenceHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(TypeReferenceHandle left, TypeReferenceHandle right)
	{
		throw null;
	}

	public static explicit operator TypeReferenceHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator TypeReferenceHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(TypeReferenceHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(TypeReferenceHandle handle)
	{
		throw null;
	}

	public static bool operator !=(TypeReferenceHandle left, TypeReferenceHandle right)
	{
		throw null;
	}
}
