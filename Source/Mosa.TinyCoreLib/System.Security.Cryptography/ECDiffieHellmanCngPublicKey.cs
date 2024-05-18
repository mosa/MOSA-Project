using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class ECDiffieHellmanCngPublicKey : ECDiffieHellmanPublicKey
{
	public CngKeyBlobFormat BlobFormat
	{
		get
		{
			throw null;
		}
	}

	internal ECDiffieHellmanCngPublicKey()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override ECParameters ExportExplicitParameters()
	{
		throw null;
	}

	public override ECParameters ExportParameters()
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static ECDiffieHellmanPublicKey FromByteArray(byte[] publicKeyBlob, CngKeyBlobFormat format)
	{
		throw null;
	}

	[Obsolete("ToXmlString and FromXmlString have no implementation for ECC types, and are obsolete. Use a standard import and export format such as ExportSubjectPublicKeyInfo or ImportSubjectPublicKeyInfo for public keys and ExportPkcs8PrivateKey or ImportPkcs8PrivateKey for private keys.", DiagnosticId = "SYSLIB0042", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static ECDiffieHellmanCngPublicKey FromXmlString(string xml)
	{
		throw null;
	}

	public CngKey Import()
	{
		throw null;
	}

	[Obsolete("ToXmlString and FromXmlString have no implementation for ECC types, and are obsolete. Use a standard import and export format such as ExportSubjectPublicKeyInfo or ImportSubjectPublicKeyInfo for public keys and ExportPkcs8PrivateKey or ImportPkcs8PrivateKey for private keys.", DiagnosticId = "SYSLIB0042", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override string ToXmlString()
	{
		throw null;
	}
}
