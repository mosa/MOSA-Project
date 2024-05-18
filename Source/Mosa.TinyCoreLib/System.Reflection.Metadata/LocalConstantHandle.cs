using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct LocalConstantHandle : IEquatable<LocalConstantHandle>
{
	private readonly int _dummyPrimitive;

	public bool IsNil
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(LocalConstantHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(LocalConstantHandle left, LocalConstantHandle right)
	{
		throw null;
	}

	public static explicit operator LocalConstantHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator LocalConstantHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(LocalConstantHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(LocalConstantHandle handle)
	{
		throw null;
	}

	public static bool operator !=(LocalConstantHandle left, LocalConstantHandle right)
	{
		throw null;
	}
}
