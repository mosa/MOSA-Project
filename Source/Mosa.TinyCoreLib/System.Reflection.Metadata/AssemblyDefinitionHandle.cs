namespace System.Reflection.Metadata;

public readonly struct AssemblyDefinitionHandle : IEquatable<AssemblyDefinitionHandle>
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

	public bool Equals(AssemblyDefinitionHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(AssemblyDefinitionHandle left, AssemblyDefinitionHandle right)
	{
		throw null;
	}

	public static explicit operator AssemblyDefinitionHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator AssemblyDefinitionHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(AssemblyDefinitionHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(AssemblyDefinitionHandle handle)
	{
		throw null;
	}

	public static bool operator !=(AssemblyDefinitionHandle left, AssemblyDefinitionHandle right)
	{
		throw null;
	}
}
