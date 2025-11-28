namespace System.Reflection.Metadata;

public readonly struct MethodImplementationHandle : IEquatable<MethodImplementationHandle>
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

	public bool Equals(MethodImplementationHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(MethodImplementationHandle left, MethodImplementationHandle right)
	{
		throw null;
	}

	public static explicit operator MethodImplementationHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator MethodImplementationHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(MethodImplementationHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(MethodImplementationHandle handle)
	{
		throw null;
	}

	public static bool operator !=(MethodImplementationHandle left, MethodImplementationHandle right)
	{
		throw null;
	}
}
