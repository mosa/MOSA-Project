using System.IO;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class ECDsaCng : ECDsa
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

	public CngKey Key
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

	[SupportedOSPlatform("windows")]
	public ECDsaCng()
	{
	}

	[SupportedOSPlatform("windows")]
	public ECDsaCng(int keySize)
	{
	}

	[SupportedOSPlatform("windows")]
	public ECDsaCng(CngKey key)
	{
	}

	[SupportedOSPlatform("windows")]
	public ECDsaCng(ECCurve curve)
	{
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

	public byte[] SignData(byte[] data)
	{
		throw null;
	}

	public byte[] SignData(byte[] data, int offset, int count)
	{
		throw null;
	}

	public byte[] SignData(Stream data)
	{
		throw null;
	}

	public override byte[] SignHash(byte[] hash)
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

	public override bool TrySignHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	protected override bool TrySignHashCore(ReadOnlySpan<byte> hash, Span<byte> destination, DSASignatureFormat signatureFormat, out int bytesWritten)
	{
		throw null;
	}

	public bool VerifyData(byte[] data, byte[] signature)
	{
		throw null;
	}

	public bool VerifyData(byte[] data, int offset, int count, byte[] signature)
	{
		throw null;
	}

	public bool VerifyData(Stream data, byte[] signature)
	{
		throw null;
	}

	public override bool VerifyHash(byte[] hash, byte[] signature)
	{
		throw null;
	}

	public override bool VerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature)
	{
		throw null;
	}

	protected override bool VerifyHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, DSASignatureFormat signatureFormat)
	{
		throw null;
	}
}
