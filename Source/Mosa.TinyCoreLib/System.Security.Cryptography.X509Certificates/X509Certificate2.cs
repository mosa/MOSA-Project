using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

namespace System.Security.Cryptography.X509Certificates;

public class X509Certificate2 : X509Certificate
{
	public bool Archived
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public X509ExtensionCollection Extensions
	{
		get
		{
			throw null;
		}
	}

	public string FriendlyName
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public bool HasPrivateKey
	{
		get
		{
			throw null;
		}
	}

	public X500DistinguishedName IssuerName
	{
		get
		{
			throw null;
		}
	}

	public DateTime NotAfter
	{
		get
		{
			throw null;
		}
	}

	public DateTime NotBefore
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("X509Certificate2.PrivateKey is obsolete. Use the appropriate method to get the private key, such as GetRSAPrivateKey, or use the CopyWithPrivateKey method to create a new instance with a private key.", DiagnosticId = "SYSLIB0028", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public AsymmetricAlgorithm? PrivateKey
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PublicKey PublicKey
	{
		get
		{
			throw null;
		}
	}

	public byte[] RawData
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte> RawDataMemory
	{
		get
		{
			throw null;
		}
	}

	public string SerialNumber
	{
		get
		{
			throw null;
		}
	}

	public Oid SignatureAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public X500DistinguishedName SubjectName
	{
		get
		{
			throw null;
		}
	}

	public string Thumbprint
	{
		get
		{
			throw null;
		}
	}

	public int Version
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate2()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(byte[] rawData)
	{
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(byte[] rawData, SecureString? password)
	{
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(byte[] rawData, SecureString? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(byte[] rawData, string? password)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(byte[] rawData, string? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(IntPtr handle)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(ReadOnlySpan<byte> rawData)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(ReadOnlySpan<byte> rawData, ReadOnlySpan<char> password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected X509Certificate2(SerializationInfo info, StreamingContext context)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(X509Certificate certificate)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(string fileName)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(string fileName, ReadOnlySpan<char> password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet)
	{
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(string fileName, SecureString? password)
	{
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(string fileName, SecureString? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(string fileName, string? password)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public X509Certificate2(string fileName, string? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	public X509Certificate2 CopyWithPrivateKey(ECDiffieHellman privateKey)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static X509Certificate2 CreateFromEncryptedPem(ReadOnlySpan<char> certPem, ReadOnlySpan<char> keyPem, ReadOnlySpan<char> password)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static X509Certificate2 CreateFromEncryptedPemFile(string certPemFilePath, ReadOnlySpan<char> password, string? keyPemFilePath = null)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static X509Certificate2 CreateFromPem(ReadOnlySpan<char> certPem)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static X509Certificate2 CreateFromPem(ReadOnlySpan<char> certPem, ReadOnlySpan<char> keyPem)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static X509Certificate2 CreateFromPemFile(string certPemFilePath, string? keyPemFilePath = null)
	{
		throw null;
	}

	public string ExportCertificatePem()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static X509ContentType GetCertContentType(byte[] rawData)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static X509ContentType GetCertContentType(ReadOnlySpan<byte> rawData)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static X509ContentType GetCertContentType(string fileName)
	{
		throw null;
	}

	public ECDiffieHellman? GetECDiffieHellmanPrivateKey()
	{
		throw null;
	}

	public ECDiffieHellman? GetECDiffieHellmanPublicKey()
	{
		throw null;
	}

	public string GetNameInfo(X509NameType nameType, bool forIssuer)
	{
		throw null;
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override void Import(byte[] rawData)
	{
	}

	[CLSCompliant(false)]
	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override void Import(byte[] rawData, SecureString? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override void Import(byte[] rawData, string? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override void Import(string fileName)
	{
	}

	[CLSCompliant(false)]
	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override void Import(string fileName, SecureString? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	[Obsolete("X509Certificate and X509Certificate2 are immutable. Use the appropriate constructor to create a new certificate.", DiagnosticId = "SYSLIB0026", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public override void Import(string fileName, string? password, X509KeyStorageFlags keyStorageFlags)
	{
	}

	public bool MatchesHostname(string hostname, bool allowWildcards = true, bool allowCommonName = true)
	{
		throw null;
	}

	public override void Reset()
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public override string ToString(bool verbose)
	{
		throw null;
	}

	public bool TryExportCertificatePem(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public bool Verify()
	{
		throw null;
	}
}
