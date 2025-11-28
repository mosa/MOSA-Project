namespace System.Reflection.Metadata;

public readonly struct MemberReferenceHandle : IEquatable<MemberReferenceHandle>
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

	public bool Equals(MemberReferenceHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(MemberReferenceHandle left, MemberReferenceHandle right)
	{
		throw null;
	}

	public static explicit operator MemberReferenceHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator MemberReferenceHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(MemberReferenceHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(MemberReferenceHandle handle)
	{
		throw null;
	}

	public static bool operator !=(MemberReferenceHandle left, MemberReferenceHandle right)
	{
		throw null;
	}
}
