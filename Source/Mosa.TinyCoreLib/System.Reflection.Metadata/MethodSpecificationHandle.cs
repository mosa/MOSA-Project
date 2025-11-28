namespace System.Reflection.Metadata;

public readonly struct MethodSpecificationHandle : IEquatable<MethodSpecificationHandle>
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

	public bool Equals(MethodSpecificationHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(MethodSpecificationHandle left, MethodSpecificationHandle right)
	{
		throw null;
	}

	public static explicit operator MethodSpecificationHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator MethodSpecificationHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(MethodSpecificationHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(MethodSpecificationHandle handle)
	{
		throw null;
	}

	public static bool operator !=(MethodSpecificationHandle left, MethodSpecificationHandle right)
	{
		throw null;
	}
}
