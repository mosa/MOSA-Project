namespace System.Reflection.Metadata;

public readonly struct FieldDefinitionHandle : IEquatable<FieldDefinitionHandle>
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

	public bool Equals(FieldDefinitionHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(FieldDefinitionHandle left, FieldDefinitionHandle right)
	{
		throw null;
	}

	public static explicit operator FieldDefinitionHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator FieldDefinitionHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(FieldDefinitionHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(FieldDefinitionHandle handle)
	{
		throw null;
	}

	public static bool operator !=(FieldDefinitionHandle left, FieldDefinitionHandle right)
	{
		throw null;
	}
}
