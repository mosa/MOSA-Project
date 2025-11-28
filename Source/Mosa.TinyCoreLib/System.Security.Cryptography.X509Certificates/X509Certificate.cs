using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

namespace System.Security.Cryptography.X509Certificates;

public class X509Certificate : IDisposable, IDeserializationCallback, ISerializable
{
	public IntPtr Handle
	{
		get
		{
			throw null;
		}
	}

	public string Issuer
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte> SerialNumberBytes
	{
		get
		{
			throw null;
		}
	}

	public string Subject
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate(byte[] data)
	{
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate(byte[] rawData, SecureString? password)
	{
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate(byte[] rawData, SecureString? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate(byte[] rawData, string? password)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate(byte[] rawData, string? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate(IntPtr handle)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public X509Certificate(SerializationInfo info, StreamingContext context)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate(X509Certificate cert)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate(string fileName)
	{
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate(string fileName, SecureString? password)
	{
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate(string fileName, SecureString? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate(string fileName, string? password)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate(string fileName, string? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public static X509Certificate CreateFromCertFile(string filename)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static X509Certificate CreateFromSignedFile(string filename)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public virtual bool Equals([NotNullWhen(true)] X509Certificate? other)
	{
		throw null;
	}

	public virtual byte[] Export(X509ContentType contentType)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public virtual byte[] Export(X509ContentType contentType, SecureString? password)
	{
		throw null;
	}

	public virtual byte[] Export(X509ContentType contentType, string? password)
	{
		throw null;
	}

	protected static string FormatDate(DateTime date)
	{
		throw null;
	}

	public virtual byte[] GetCertHash()
	{
		throw null;
	}

	public virtual byte[] GetCertHash(HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public virtual string GetCertHashString()
	{
		throw null;
	}

	public virtual string GetCertHashString(HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public virtual string GetEffectiveDateString()
	{
		throw null;
	}

	public virtual string GetExpirationDateString()
	{
		throw null;
	}

	public virtual string GetFormat()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	[Obsolete("X509Certificate.GetIssuerName has been deprecated. Use the Issuer property instead.")]
	public virtual string GetIssuerName()
	{
		throw null;
	}

	public virtual string GetKeyAlgorithm()
	{
		throw null;
	}

	public virtual byte[] GetKeyAlgorithmParameters()
	{
		throw null;
	}

	public virtual string GetKeyAlgorithmParametersString()
	{
		throw null;
	}

	[Obsolete("X509Certificate.GetName has been deprecated. Use the Subject property instead.")]
	public virtual string GetName()
	{
		throw null;
	}

	public virtual byte[] GetPublicKey()
	{
		throw null;
	}

	public virtual string GetPublicKeyString()
	{
		throw null;
	}

	public virtual byte[] GetRawCertData()
	{
		throw null;
	}

	public virtual string GetRawCertDataString()
	{
		throw null;
	}

	public virtual byte[] GetSerialNumber()
	{
		throw null;
	}

	public virtual string GetSerialNumberString()
	{
		throw null;
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual void Import(byte[] rawData)
	{
	}

	[CLSCompliant(false)]
	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual void Import(byte[] rawData, SecureString? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual void Import(byte[] rawData, string? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual void Import(string fileName)
	{
	}

	[CLSCompliant(false)]
	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual void Import(string fileName, SecureString? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual void Import(string fileName, string? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	public virtual void Reset()
	{
	}

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public virtual string ToString(bool fVerbose)
	{
		throw null;
	}

	public virtual bool TryGetCertHash(HashAlgorithmName hashAlgorithm, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
