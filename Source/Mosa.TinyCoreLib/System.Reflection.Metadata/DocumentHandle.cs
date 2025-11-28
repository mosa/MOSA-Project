namespace System.Reflection.Metadata;

public readonly struct DocumentHandle : IEquatable<DocumentHandle>
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

	public bool Equals(DocumentHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(DocumentHandle left, DocumentHandle right)
	{
		throw null;
	}

	public static explicit operator DocumentHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator DocumentHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(DocumentHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(DocumentHandle handle)
	{
		throw null;
	}

	public static bool operator !=(DocumentHandle left, DocumentHandle right)
	{
		throw null;
	}
}
