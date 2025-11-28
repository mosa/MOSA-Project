using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography.Cose;

public readonly struct CoseHeaderLabel : IEquatable<CoseHeaderLabel>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static CoseHeaderLabel Algorithm
	{
		get
		{
			throw null;
		}
	}

	public static CoseHeaderLabel ContentType
	{
		get
		{
			throw null;
		}
	}

	public static CoseHeaderLabel CriticalHeaders
	{
		get
		{
			throw null;
		}
	}

	public static CoseHeaderLabel KeyIdentifier
	{
		get
		{
			throw null;
		}
	}

	public CoseHeaderLabel(int label)
	{
		throw null;
	}

	public CoseHeaderLabel(string label)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(CoseHeaderLabel other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CoseHeaderLabel left, CoseHeaderLabel right)
	{
		throw null;
	}

	public static bool operator !=(CoseHeaderLabel left, CoseHeaderLabel right)
	{
		throw null;
	}
}
