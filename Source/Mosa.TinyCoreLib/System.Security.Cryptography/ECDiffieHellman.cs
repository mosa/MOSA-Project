using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public abstract class ECDiffieHellman : ECAlgorithm
{
	public override string KeyExchangeAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public abstract ECDiffieHellmanPublicKey PublicKey { get; }

	public override string? SignatureAlgorithm
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("browser")]
	public new static ECDiffieHellman Create()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static ECDiffieHellman Create(ECCurve curve)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static ECDiffieHellman Create(ECParameters parameters)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static ECDiffieHellman? Create(string algorithm)
	{
		throw null;
	}

	public byte[] DeriveKeyFromHash(ECDiffieHellmanPublicKey otherPartyPublicKey, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public virtual byte[] DeriveKeyFromHash(ECDiffieHellmanPublicKey otherPartyPublicKey, HashAlgorithmName hashAlgorithm, byte[]? secretPrepend, byte[]? secretAppend)
	{
		throw null;
	}

	public byte[] DeriveKeyFromHmac(ECDiffieHellmanPublicKey otherPartyPublicKey, HashAlgorithmName hashAlgorithm, byte[]? hmacKey)
	{
		throw null;
	}

	public virtual byte[] DeriveKeyFromHmac(ECDiffieHellmanPublicKey otherPartyPublicKey, HashAlgorithmName hashAlgorithm, byte[]? hmacKey, byte[]? secretPrepend, byte[]? secretAppend)
	{
		throw null;
	}

	public virtual byte[] DeriveKeyMaterial(ECDiffieHellmanPublicKey otherPartyPublicKey)
	{
		throw null;
	}

	public virtual byte[] DeriveKeyTls(ECDiffieHellmanPublicKey otherPartyPublicKey, byte[] prfLabel, byte[] prfSeed)
	{
		throw null;
	}

	public virtual byte[] DeriveRawSecretAgreement(ECDiffieHellmanPublicKey otherPartyPublicKey)
	{
		throw null;
	}

	public override void FromXmlString(string xmlString)
	{
	}

	public override string ToXmlString(bool includePrivateParameters)
	{
		throw null;
	}
}
