namespace System.Reflection.Metadata;

public readonly struct MethodDebugInformationHandle : IEquatable<MethodDebugInformationHandle>
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

	public bool Equals(MethodDebugInformationHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(MethodDebugInformationHandle left, MethodDebugInformationHandle right)
	{
		throw null;
	}

	public static explicit operator MethodDebugInformationHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator MethodDebugInformationHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(MethodDebugInformationHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(MethodDebugInformationHandle handle)
	{
		throw null;
	}

	public static bool operator !=(MethodDebugInformationHandle left, MethodDebugInformationHandle right)
	{
		throw null;
	}

	public MethodDefinitionHandle ToDefinitionHandle()
	{
		throw null;
	}
}
