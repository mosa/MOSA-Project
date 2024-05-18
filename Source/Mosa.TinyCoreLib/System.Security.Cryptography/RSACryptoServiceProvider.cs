using System.ComponentModel;
using System.IO;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class RSACryptoServiceProvider : RSA, ICspAsymmetricAlgorithm
{
	[SupportedOSPlatform("windows")]
	public CspKeyContainerInfo CspKeyContainerInfo
	{
		get
		{
			throw null;
		}
	}

	public override string? KeyExchangeAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public override int KeySize
	{
		get
		{
			throw null;
		}
	}

	public override KeySizes[] LegalKeySizes
	{
		get
		{
			throw null;
		}
	}

	public bool PersistKeyInCsp
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool PublicOnly
	{
		get
		{
			throw null;
		}
	}

	public override string SignatureAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public static bool UseMachineKeyStore
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public RSACryptoServiceProvider()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public RSACryptoServiceProvider(int dwKeySize)
	{
	}

	[SupportedOSPlatform("windows")]
	public RSACryptoServiceProvider(int dwKeySize, CspParameters? parameters)
	{
	}

	[SupportedOSPlatform("windows")]
	public RSACryptoServiceProvider(CspParameters? parameters)
	{
	}

	public byte[] Decrypt(byte[] rgb, bool fOAEP)
	{
		throw null;
	}

	public override byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("RSA.EncryptValue and DecryptValue are not supported and throw NotSupportedException. Use RSA.Encrypt and RSA.Decrypt instead.", DiagnosticId = "SYSLIB0048", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override byte[] DecryptValue(byte[] rgb)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public byte[] Encrypt(byte[] rgb, bool fOAEP)
	{
		throw null;
	}

	public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("RSA.EncryptValue and DecryptValue are not supported and throw NotSupportedException. Use RSA.Encrypt and RSA.Decrypt instead.", DiagnosticId = "SYSLIB0048", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override byte[] EncryptValue(byte[] rgb)
	{
		throw null;
	}

	public byte[] ExportCspBlob(bool includePrivateParameters)
	{
		throw null;
	}

	public override RSAParameters ExportParameters(bool includePrivateParameters)
	{
		throw null;
	}

	public void ImportCspBlob(byte[] keyBlob)
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

	public override void ImportParameters(RSAParameters parameters)
	{
	}

	public byte[] SignData(byte[] buffer, int offset, int count, object halg)
	{
		throw null;
	}

	public byte[] SignData(byte[] buffer, object halg)
	{
		throw null;
	}

	public byte[] SignData(Stream inputStream, object halg)
	{
		throw null;
	}

	public override byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public byte[] SignHash(byte[] rgbHash, string? str)
	{
		throw null;
	}

	public bool VerifyData(byte[] buffer, object halg, byte[] signature)
	{
		throw null;
	}

	public override bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
	{
		throw null;
	}
}
