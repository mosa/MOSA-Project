namespace System.Security.Cryptography;

public abstract class ECDiffieHellmanPublicKey : IDisposable
{
	protected ECDiffieHellmanPublicKey()
	{
	}

	[Obsolete("ECDiffieHellmanPublicKey.ToByteArray() and the associated constructor do not have a consistent and interoperable implementation on all platforms. Use ECDiffieHellmanPublicKey.ExportSubjectPublicKeyInfo() instead.", DiagnosticId = "SYSLIB0043", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected ECDiffieHellmanPublicKey(byte[] keyBlob)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual ECParameters ExportExplicitParameters()
	{
		throw null;
	}

	public virtual ECParameters ExportParameters()
	{
		throw null;
	}

	public virtual byte[] ExportSubjectPublicKeyInfo()
	{
		throw null;
	}

	[Obsolete("ECDiffieHellmanPublicKey.ToByteArray() and the associated constructor do not have a consistent and interoperable implementation on all platforms. Use ECDiffieHellmanPublicKey.ExportSubjectPublicKeyInfo() instead.", DiagnosticId = "SYSLIB0043", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual byte[] ToByteArray()
	{
		throw null;
	}

	[Obsolete("ToXmlString and FromXmlString have no implementation for ECC types, and are obsolete. Use a standard import and export format such as ExportSubjectPublicKeyInfo or ImportSubjectPublicKeyInfo for public keys and ExportPkcs8PrivateKey or ImportPkcs8PrivateKey for private keys.", DiagnosticId = "SYSLIB0042", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual string ToXmlString()
	{
		throw null;
	}

	public virtual bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
