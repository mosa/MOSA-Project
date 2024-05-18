namespace System.Reflection.Metadata;

public readonly struct LocalScopeHandle : IEquatable<LocalScopeHandle>
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

	public bool Equals(LocalScopeHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(LocalScopeHandle left, LocalScopeHandle right)
	{
		throw null;
	}

	public static explicit operator LocalScopeHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator LocalScopeHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(LocalScopeHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(LocalScopeHandle handle)
	{
		throw null;
	}

	public static bool operator !=(LocalScopeHandle left, LocalScopeHandle right)
	{
		throw null;
	}
}
