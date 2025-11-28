namespace System.Reflection.Metadata;

public readonly struct StandaloneSignatureHandle : IEquatable<StandaloneSignatureHandle>
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

	public bool Equals(StandaloneSignatureHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(StandaloneSignatureHandle left, StandaloneSignatureHandle right)
	{
		throw null;
	}

	public static explicit operator StandaloneSignatureHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator StandaloneSignatureHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(StandaloneSignatureHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(StandaloneSignatureHandle handle)
	{
		throw null;
	}

	public static bool operator !=(StandaloneSignatureHandle left, StandaloneSignatureHandle right)
	{
		throw null;
	}
}
