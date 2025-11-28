using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public readonly struct HashAlgorithmName : IEquatable<HashAlgorithmName>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static HashAlgorithmName MD5
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public static HashAlgorithmName SHA1
	{
		get
		{
			throw null;
		}
	}

	public static HashAlgorithmName SHA256
	{
		get
		{
			throw null;
		}
	}

	public static HashAlgorithmName SHA384
	{
		get
		{
			throw null;
		}
	}

	public static HashAlgorithmName SHA3_256
	{
		get
		{
			throw null;
		}
	}

	public static HashAlgorithmName SHA3_384
	{
		get
		{
			throw null;
		}
	}

	public static HashAlgorithmName SHA3_512
	{
		get
		{
			throw null;
		}
	}

	public static HashAlgorithmName SHA512
	{
		get
		{
			throw null;
		}
	}

	public HashAlgorithmName(string? name)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(HashAlgorithmName other)
	{
		throw null;
	}

	public static HashAlgorithmName FromOid(string oidValue)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(HashAlgorithmName left, HashAlgorithmName right)
	{
		throw null;
	}

	public static bool operator !=(HashAlgorithmName left, HashAlgorithmName right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static bool TryFromOid(string oidValue, out HashAlgorithmName value)
	{
		throw null;
	}
}
