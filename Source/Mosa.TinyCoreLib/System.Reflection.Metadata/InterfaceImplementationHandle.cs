namespace System.Reflection.Metadata;

public readonly struct InterfaceImplementationHandle : IEquatable<InterfaceImplementationHandle>
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

	public bool Equals(InterfaceImplementationHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(InterfaceImplementationHandle left, InterfaceImplementationHandle right)
	{
		throw null;
	}

	public static explicit operator InterfaceImplementationHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator InterfaceImplementationHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(InterfaceImplementationHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(InterfaceImplementationHandle handle)
	{
		throw null;
	}

	public static bool operator !=(InterfaceImplementationHandle left, InterfaceImplementationHandle right)
	{
		throw null;
	}
}
