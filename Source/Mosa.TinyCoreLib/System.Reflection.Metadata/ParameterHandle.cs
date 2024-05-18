namespace System.Reflection.Metadata;

public readonly struct ParameterHandle : IEquatable<ParameterHandle>
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

	public bool Equals(ParameterHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ParameterHandle left, ParameterHandle right)
	{
		throw null;
	}

	public static explicit operator ParameterHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator ParameterHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(ParameterHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(ParameterHandle handle)
	{
		throw null;
	}

	public static bool operator !=(ParameterHandle left, ParameterHandle right)
	{
		throw null;
	}
}
