namespace System.Reflection.Metadata;

public readonly struct PropertyDefinitionHandle : IEquatable<PropertyDefinitionHandle>
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

	public bool Equals(PropertyDefinitionHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(PropertyDefinitionHandle left, PropertyDefinitionHandle right)
	{
		throw null;
	}

	public static explicit operator PropertyDefinitionHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator PropertyDefinitionHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(PropertyDefinitionHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(PropertyDefinitionHandle handle)
	{
		throw null;
	}

	public static bool operator !=(PropertyDefinitionHandle left, PropertyDefinitionHandle right)
	{
		throw null;
	}
}
