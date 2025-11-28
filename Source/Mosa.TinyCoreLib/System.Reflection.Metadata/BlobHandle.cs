using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct BlobHandle : IEquatable<BlobHandle>
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

	public bool Equals(BlobHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(BlobHandle left, BlobHandle right)
	{
		throw null;
	}

	public static explicit operator BlobHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator Handle(BlobHandle handle)
	{
		throw null;
	}

	public static bool operator !=(BlobHandle left, BlobHandle right)
	{
		throw null;
	}
}
