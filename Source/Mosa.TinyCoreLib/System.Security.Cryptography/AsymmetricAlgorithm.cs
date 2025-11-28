using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public abstract class AsymmetricAlgorithm : IDisposable
{
	protected int KeySizeValue;

	[MaybeNull]
	protected KeySizes[] LegalKeySizesValue;

	public virtual string? KeyExchangeAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public virtual int KeySize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual KeySizes[] LegalKeySizes
	{
		get
		{
			throw null;
		}
	}

	public virtual string? SignatureAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public void Clear()
	{
	}

	[Obsolete("The default implementation of this cryptography algorithm is not supported.", DiagnosticId = "SYSLIB0007", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static AsymmetricAlgorithm Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static AsymmetricAlgorithm? Create(string algName)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
	{
		throw null;
	}

	public virtual byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters)
	{
		throw null;
	}

	public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
	{
		throw null;
	}

	public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<char> password, PbeParameters pbeParameters)
	{
		throw null;
	}

	public virtual byte[] ExportPkcs8PrivateKey()
	{
		throw null;
	}

	public string ExportPkcs8PrivateKeyPem()
	{
		throw null;
	}

	public virtual byte[] ExportSubjectPublicKeyInfo()
	{
		throw null;
	}

	public string ExportSubjectPublicKeyInfoPem()
	{
		throw null;
	}

	public virtual void FromXmlString(string xmlString)
	{
	}

	public virtual void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public virtual void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public virtual void ImportFromEncryptedPem(ReadOnlySpan<char> input, ReadOnlySpan<byte> passwordBytes)
	{
	}

	public virtual void ImportFromEncryptedPem(ReadOnlySpan<char> input, ReadOnlySpan<char> password)
	{
	}

	public virtual void ImportFromPem(ReadOnlySpan<char> input)
	{
	}

	public virtual void ImportPkcs8PrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public virtual void ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public virtual string ToXmlString(bool includePrivateParameters)
	{
		throw null;
	}

	public virtual bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public virtual bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public bool TryExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public virtual bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryExportPkcs8PrivateKeyPem(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public virtual bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryExportSubjectPublicKeyInfoPem(Span<char> destination, out int charsWritten)
	{
		throw null;
	}
}
