namespace System.Reflection.Metadata;

public readonly struct ModuleReferenceHandle : IEquatable<ModuleReferenceHandle>
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

	public bool Equals(ModuleReferenceHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ModuleReferenceHandle left, ModuleReferenceHandle right)
	{
		throw null;
	}

	public static explicit operator ModuleReferenceHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator ModuleReferenceHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(ModuleReferenceHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(ModuleReferenceHandle handle)
	{
		throw null;
	}

	public static bool operator !=(ModuleReferenceHandle left, ModuleReferenceHandle right)
	{
		throw null;
	}
}
