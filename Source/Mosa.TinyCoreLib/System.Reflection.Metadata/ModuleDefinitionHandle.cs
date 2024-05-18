namespace System.Reflection.Metadata;

public readonly struct ModuleDefinitionHandle : IEquatable<ModuleDefinitionHandle>
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

	public bool Equals(ModuleDefinitionHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ModuleDefinitionHandle left, ModuleDefinitionHandle right)
	{
		throw null;
	}

	public static explicit operator ModuleDefinitionHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator ModuleDefinitionHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(ModuleDefinitionHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(ModuleDefinitionHandle handle)
	{
		throw null;
	}

	public static bool operator !=(ModuleDefinitionHandle left, ModuleDefinitionHandle right)
	{
		throw null;
	}
}
