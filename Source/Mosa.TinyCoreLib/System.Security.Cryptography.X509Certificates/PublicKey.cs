using System.Runtime.Versioning;

namespace System.Security.Cryptography.X509Certificates;

public sealed class PublicKey
{
	public AsnEncodedData EncodedKeyValue
	{
		get
		{
			throw null;
		}
	}

	public AsnEncodedData EncodedParameters
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("PublicKey.Key is obsolete. Use the appropriate method to get the public key, such as GetRSAPublicKey.", DiagnosticId = "SYSLIB0027", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public AsymmetricAlgorithm Key
	{
		get
		{
			throw null;
		}
	}

	public Oid Oid
	{
		get
		{
			throw null;
		}
	}

	public PublicKey(AsymmetricAlgorithm key)
	{
	}

	public PublicKey(Oid oid, AsnEncodedData parameters, AsnEncodedData keyValue)
	{
	}

	public static PublicKey CreateFromSubjectPublicKeyInfo(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public byte[] ExportSubjectPublicKeyInfo()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public DSA? GetDSAPublicKey()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public ECDiffieHellman? GetECDiffieHellmanPublicKey()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public ECDsa? GetECDsaPublicKey()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public RSA? GetRSAPublicKey()
	{
		throw null;
	}

	public bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
