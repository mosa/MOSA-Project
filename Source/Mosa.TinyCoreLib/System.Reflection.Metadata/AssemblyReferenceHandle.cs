namespace System.Reflection.Metadata;

public readonly struct AssemblyReferenceHandle : IEquatable<AssemblyReferenceHandle>
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

	public bool Equals(AssemblyReferenceHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(AssemblyReferenceHandle left, AssemblyReferenceHandle right)
	{
		throw null;
	}

	public static explicit operator AssemblyReferenceHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator AssemblyReferenceHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(AssemblyReferenceHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(AssemblyReferenceHandle handle)
	{
		throw null;
	}

	public static bool operator !=(AssemblyReferenceHandle left, AssemblyReferenceHandle right)
	{
		throw null;
	}
}
