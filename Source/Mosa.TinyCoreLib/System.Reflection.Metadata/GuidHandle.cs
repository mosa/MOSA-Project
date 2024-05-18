using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct GuidHandle : IEquatable<GuidHandle>
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

	public bool Equals(GuidHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(GuidHandle left, GuidHandle right)
	{
		throw null;
	}

	public static explicit operator GuidHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator Handle(GuidHandle handle)
	{
		throw null;
	}

	public static bool operator !=(GuidHandle left, GuidHandle right)
	{
		throw null;
	}
}
