using System.IO;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class DSACryptoServiceProvider : DSA, ICspAsymmetricAlgorithm
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

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public DSACryptoServiceProvider()
	{
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public DSACryptoServiceProvider(int dwKeySize)
	{
	}

	[SupportedOSPlatform("windows")]
	public DSACryptoServiceProvider(int dwKeySize, CspParameters? parameters)
	{
	}

	[SupportedOSPlatform("windows")]
	public DSACryptoServiceProvider(CspParameters? parameters)
	{
	}

	public override byte[] CreateSignature(byte[] rgbHash)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public byte[] ExportCspBlob(bool includePrivateParameters)
	{
		throw null;
	}

	public override DSAParameters ExportParameters(bool includePrivateParameters)
	{
		throw null;
	}

	protected override byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	protected override byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
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

	public override void ImportParameters(DSAParameters parameters)
	{
	}

	public byte[] SignData(byte[] buffer)
	{
		throw null;
	}

	public byte[] SignData(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public byte[] SignData(Stream inputStream)
	{
		throw null;
	}

	public byte[] SignHash(byte[] rgbHash, string? str)
	{
		throw null;
	}

	public bool VerifyData(byte[] rgbData, byte[] rgbSignature)
	{
		throw null;
	}

	public bool VerifyHash(byte[] rgbHash, string? str, byte[] rgbSignature)
	{
		throw null;
	}

	public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
	{
		throw null;
	}
}
