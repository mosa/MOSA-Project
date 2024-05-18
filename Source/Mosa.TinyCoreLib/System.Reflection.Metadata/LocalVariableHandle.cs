namespace System.Reflection.Metadata;

public readonly struct LocalVariableHandle : IEquatable<LocalVariableHandle>
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

	public bool Equals(LocalVariableHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(LocalVariableHandle left, LocalVariableHandle right)
	{
		throw null;
	}

	public static explicit operator LocalVariableHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator LocalVariableHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(LocalVariableHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(LocalVariableHandle handle)
	{
		throw null;
	}

	public static bool operator !=(LocalVariableHandle left, LocalVariableHandle right)
	{
		throw null;
	}
}
