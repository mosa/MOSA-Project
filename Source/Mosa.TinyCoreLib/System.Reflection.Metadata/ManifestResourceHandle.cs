namespace System.Reflection.Metadata;

public readonly struct ManifestResourceHandle : IEquatable<ManifestResourceHandle>
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

	public bool Equals(ManifestResourceHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ManifestResourceHandle left, ManifestResourceHandle right)
	{
		throw null;
	}

	public static explicit operator ManifestResourceHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator ManifestResourceHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(ManifestResourceHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(ManifestResourceHandle handle)
	{
		throw null;
	}

	public static bool operator !=(ManifestResourceHandle left, ManifestResourceHandle right)
	{
		throw null;
	}
}
