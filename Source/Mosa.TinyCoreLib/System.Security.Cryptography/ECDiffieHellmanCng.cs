using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography;

public sealed class ECDiffieHellmanCng : ECDiffieHellman
{
	public CngAlgorithm HashAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[]? HmacKey
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CngKey Key
	{
		get
		{
			throw null;
		}
	}

	public ECDiffieHellmanKeyDerivationFunction KeyDerivationFunction
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override int KeySize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[]? Label
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override KeySizes[] LegalKeySizes
	{
		get
		{
			throw null;
		}
	}

	public override ECDiffieHellmanPublicKey PublicKey
	{
		get
		{
			throw null;
		}
	}

	public byte[]? SecretAppend
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[]? SecretPrepend
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[]? Seed
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool UseSecretAgreementAsHmacKey
	{
		get
		{
			throw null;
		}
	}

	[SupportedOSPlatform("windows")]
	public ECDiffieHellmanCng()
	{
	}

	[SupportedOSPlatform("windows")]
	public ECDiffieHellmanCng(int keySize)
	{
	}

	[SupportedOSPlatform("windows")]
	public ECDiffieHellmanCng(CngKey key)
	{
	}

	[SupportedOSPlatform("windows")]
	public ECDiffieHellmanCng(ECCurve curve)
	{
	}

	public override byte[] DeriveKeyFromHash(ECDiffieHellmanPublicKey otherPartyPublicKey, HashAlgorithmName hashAlgorithm, byte[]? secretPrepend, byte[]? secretAppend)
	{
		throw null;
	}

	public override byte[] DeriveKeyFromHmac(ECDiffieHellmanPublicKey otherPartyPublicKey, HashAlgorithmName hashAlgorithm, byte[]? hmacKey, byte[]? secretPrepend, byte[]? secretAppend)
	{
		throw null;
	}

	public byte[] DeriveKeyMaterial(CngKey otherPartyPublicKey)
	{
		throw null;
	}

	public override byte[] DeriveKeyMaterial(ECDiffieHellmanPublicKey otherPartyPublicKey)
	{
		throw null;
	}

	public override byte[] DeriveKeyTls(ECDiffieHellmanPublicKey otherPartyPublicKey, byte[] prfLabel, byte[] prfSeed)
	{
		throw null;
	}

	public SafeNCryptSecretHandle DeriveSecretAgreementHandle(CngKey otherPartyPublicKey)
	{
		throw null;
	}

	public SafeNCryptSecretHandle DeriveSecretAgreementHandle(ECDiffieHellmanPublicKey otherPartyPublicKey)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
	{
		throw null;
	}

	public override byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters)
	{
		throw null;
	}

	public override ECParameters ExportExplicitParameters(bool includePrivateParameters)
	{
		throw null;
	}

	public override ECParameters ExportParameters(bool includePrivateParameters)
	{
		throw null;
	}

	[Obsolete("ToXmlString and FromXmlString have no implementation for ECC types, and are obsolete. Use a standard import and export format such as ExportSubjectPublicKeyInfo or ImportSubjectPublicKeyInfo for public keys and ExportPkcs8PrivateKey or ImportPkcs8PrivateKey for private keys.", DiagnosticId = "SYSLIB0042", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public void FromXmlString(string xml, ECKeyXmlFormat format)
	{
	}

	public override void GenerateKey(ECCurve curve)
	{
	}

	public override void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportParameters(ECParameters parameters)
	{
	}

	public override void ImportPkcs8PrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	[Obsolete("ToXmlString and FromXmlString have no implementation for ECC types, and are obsolete. Use a standard import and export format such as ExportSubjectPublicKeyInfo or ImportSubjectPublicKeyInfo for public keys and ExportPkcs8PrivateKey or ImportPkcs8PrivateKey for private keys.", DiagnosticId = "SYSLIB0042", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public string ToXmlString(ECKeyXmlFormat format)
	{
		throw null;
	}

	public override bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
