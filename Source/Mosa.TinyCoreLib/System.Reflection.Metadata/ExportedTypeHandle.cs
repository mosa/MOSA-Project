namespace System.Reflection.Metadata;

public readonly struct ExportedTypeHandle : IEquatable<ExportedTypeHandle>
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

	public bool Equals(ExportedTypeHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ExportedTypeHandle left, ExportedTypeHandle right)
	{
		throw null;
	}

	public static explicit operator ExportedTypeHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator ExportedTypeHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(ExportedTypeHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(ExportedTypeHandle handle)
	{
		throw null;
	}

	public static bool operator !=(ExportedTypeHandle left, ExportedTypeHandle right)
	{
		throw null;
	}
}
