namespace System.Reflection.Metadata;

public readonly struct StringHandle : IEquatable<StringHandle>
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

	public bool Equals(StringHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(StringHandle left, StringHandle right)
	{
		throw null;
	}

	public static explicit operator StringHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator Handle(StringHandle handle)
	{
		throw null;
	}

	public static bool operator !=(StringHandle left, StringHandle right)
	{
		throw null;
	}
}
