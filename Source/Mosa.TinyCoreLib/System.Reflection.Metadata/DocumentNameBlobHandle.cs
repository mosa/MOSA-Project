using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct DocumentNameBlobHandle : IEquatable<DocumentNameBlobHandle>
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

	public bool Equals(DocumentNameBlobHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(DocumentNameBlobHandle left, DocumentNameBlobHandle right)
	{
		throw null;
	}

	public static explicit operator DocumentNameBlobHandle(BlobHandle handle)
	{
		throw null;
	}

	public static implicit operator BlobHandle(DocumentNameBlobHandle handle)
	{
		throw null;
	}

	public static bool operator !=(DocumentNameBlobHandle left, DocumentNameBlobHandle right)
	{
		throw null;
	}
}
