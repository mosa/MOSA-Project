namespace System.Reflection.Metadata;

public readonly struct MethodDefinitionHandle : IEquatable<MethodDefinitionHandle>
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

	public bool Equals(MethodDefinitionHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(MethodDefinitionHandle left, MethodDefinitionHandle right)
	{
		throw null;
	}

	public static explicit operator MethodDefinitionHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator MethodDefinitionHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(MethodDefinitionHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(MethodDefinitionHandle handle)
	{
		throw null;
	}

	public static bool operator !=(MethodDefinitionHandle left, MethodDefinitionHandle right)
	{
		throw null;
	}

	public MethodDebugInformationHandle ToDebugInformationHandle()
	{
		throw null;
	}
}
