namespace System.Reflection.Metadata;

public readonly struct UserStringHandle : IEquatable<UserStringHandle>
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

	public bool Equals(UserStringHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(UserStringHandle left, UserStringHandle right)
	{
		throw null;
	}

	public static explicit operator UserStringHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator Handle(UserStringHandle handle)
	{
		throw null;
	}

	public static bool operator !=(UserStringHandle left, UserStringHandle right)
	{
		throw null;
	}
}
