namespace System.Reflection.Metadata;

public readonly struct EventDefinitionHandle : IEquatable<EventDefinitionHandle>
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

	public bool Equals(EventDefinitionHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(EventDefinitionHandle left, EventDefinitionHandle right)
	{
		throw null;
	}

	public static explicit operator EventDefinitionHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator EventDefinitionHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(EventDefinitionHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(EventDefinitionHandle handle)
	{
		throw null;
	}

	public static bool operator !=(EventDefinitionHandle left, EventDefinitionHandle right)
	{
		throw null;
	}
}
