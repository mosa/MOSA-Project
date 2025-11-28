namespace System.Reflection.Metadata;

public readonly struct CustomAttributeHandle : IEquatable<CustomAttributeHandle>
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

	public bool Equals(CustomAttributeHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CustomAttributeHandle left, CustomAttributeHandle right)
	{
		throw null;
	}

	public static explicit operator CustomAttributeHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator CustomAttributeHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(CustomAttributeHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(CustomAttributeHandle handle)
	{
		throw null;
	}

	public static bool operator !=(CustomAttributeHandle left, CustomAttributeHandle right)
	{
		throw null;
	}
}
