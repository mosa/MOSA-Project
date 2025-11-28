using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public sealed class RSAEncryptionPadding : IEquatable<RSAEncryptionPadding>
{
	public RSAEncryptionPaddingMode Mode
	{
		get
		{
			throw null;
		}
	}

	public HashAlgorithmName OaepHashAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public static RSAEncryptionPadding OaepSHA1
	{
		get
		{
			throw null;
		}
	}

	public static RSAEncryptionPadding OaepSHA256
	{
		get
		{
			throw null;
		}
	}

	public static RSAEncryptionPadding OaepSHA384
	{
		get
		{
			throw null;
		}
	}

	public static RSAEncryptionPadding OaepSHA3_256
	{
		get
		{
			throw null;
		}
	}

	public static RSAEncryptionPadding OaepSHA3_384
	{
		get
		{
			throw null;
		}
	}

	public static RSAEncryptionPadding OaepSHA3_512
	{
		get
		{
			throw null;
		}
	}

	public static RSAEncryptionPadding OaepSHA512
	{
		get
		{
			throw null;
		}
	}

	public static RSAEncryptionPadding Pkcs1
	{
		get
		{
			throw null;
		}
	}

	internal RSAEncryptionPadding()
	{
	}

	public static RSAEncryptionPadding CreateOaep(HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] RSAEncryptionPadding? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(RSAEncryptionPadding? left, RSAEncryptionPadding? right)
	{
		throw null;
	}

	public static bool operator !=(RSAEncryptionPadding? left, RSAEncryptionPadding? right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
