using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public struct SignatureHeader : IEquatable<SignatureHeader>
{
	private int _dummyPrimitive;

	public const byte CallingConventionOrKindMask = 15;

	public SignatureAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public SignatureCallingConvention CallingConvention
	{
		get
		{
			throw null;
		}
	}

	public bool HasExplicitThis
	{
		get
		{
			throw null;
		}
	}

	public bool IsGeneric
	{
		get
		{
			throw null;
		}
	}

	public bool IsInstance
	{
		get
		{
			throw null;
		}
	}

	public SignatureKind Kind
	{
		get
		{
			throw null;
		}
	}

	public byte RawValue
	{
		get
		{
			throw null;
		}
	}

	public SignatureHeader(byte rawValue)
	{
		throw null;
	}

	public SignatureHeader(SignatureKind kind, SignatureCallingConvention convention, SignatureAttributes attributes)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(SignatureHeader other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(SignatureHeader left, SignatureHeader right)
	{
		throw null;
	}

	public static bool operator !=(SignatureHeader left, SignatureHeader right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
