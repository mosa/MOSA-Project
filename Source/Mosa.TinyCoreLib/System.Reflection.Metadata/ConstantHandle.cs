namespace System.Reflection.Metadata;

public readonly struct ConstantHandle : IEquatable<ConstantHandle>
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

	public bool Equals(ConstantHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ConstantHandle left, ConstantHandle right)
	{
		throw null;
	}

	public static explicit operator ConstantHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator ConstantHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(ConstantHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(ConstantHandle handle)
	{
		throw null;
	}

	public static bool operator !=(ConstantHandle left, ConstantHandle right)
	{
		throw null;
	}
}
