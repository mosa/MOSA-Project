namespace System.Reflection.Metadata;

public readonly struct GenericParameterHandle : IEquatable<GenericParameterHandle>
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

	public bool Equals(GenericParameterHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(GenericParameterHandle left, GenericParameterHandle right)
	{
		throw null;
	}

	public static explicit operator GenericParameterHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator GenericParameterHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(GenericParameterHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(GenericParameterHandle handle)
	{
		throw null;
	}

	public static bool operator !=(GenericParameterHandle left, GenericParameterHandle right)
	{
		throw null;
	}
}
