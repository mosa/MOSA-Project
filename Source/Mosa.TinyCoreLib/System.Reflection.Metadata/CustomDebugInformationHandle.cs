using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct CustomDebugInformationHandle : IEquatable<CustomDebugInformationHandle>
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

	public bool Equals(CustomDebugInformationHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CustomDebugInformationHandle left, CustomDebugInformationHandle right)
	{
		throw null;
	}

	public static explicit operator CustomDebugInformationHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator CustomDebugInformationHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(CustomDebugInformationHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(CustomDebugInformationHandle handle)
	{
		throw null;
	}

	public static bool operator !=(CustomDebugInformationHandle left, CustomDebugInformationHandle right)
	{
		throw null;
	}
}
