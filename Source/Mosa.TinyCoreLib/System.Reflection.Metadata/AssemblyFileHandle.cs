namespace System.Reflection.Metadata;

public readonly struct AssemblyFileHandle : IEquatable<AssemblyFileHandle>
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

	public bool Equals(AssemblyFileHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(AssemblyFileHandle left, AssemblyFileHandle right)
	{
		throw null;
	}

	public static explicit operator AssemblyFileHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator AssemblyFileHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(AssemblyFileHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(AssemblyFileHandle handle)
	{
		throw null;
	}

	public static bool operator !=(AssemblyFileHandle left, AssemblyFileHandle right)
	{
		throw null;
	}
}
