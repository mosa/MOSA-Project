using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct BlobContentId : IEquatable<BlobContentId>
{
	private readonly int _dummyPrimitive;

	public Guid Guid
	{
		get
		{
			throw null;
		}
	}

	public bool IsDefault
	{
		get
		{
			throw null;
		}
	}

	public uint Stamp
	{
		get
		{
			throw null;
		}
	}

	public BlobContentId(byte[] id)
	{
		throw null;
	}

	public BlobContentId(ImmutableArray<byte> id)
	{
		throw null;
	}

	public BlobContentId(Guid guid, uint stamp)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(BlobContentId other)
	{
		throw null;
	}

	public static BlobContentId FromHash(byte[] hashCode)
	{
		throw null;
	}

	public static BlobContentId FromHash(ImmutableArray<byte> hashCode)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static Func<IEnumerable<Blob>, BlobContentId> GetTimeBasedProvider()
	{
		throw null;
	}

	public static bool operator ==(BlobContentId left, BlobContentId right)
	{
		throw null;
	}

	public static bool operator !=(BlobContentId left, BlobContentId right)
	{
		throw null;
	}
}
