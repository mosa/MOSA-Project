using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace System.Security.Cryptography.X509Certificates;

public class X509Certificate2Collection : X509CertificateCollection, IEnumerable<X509Certificate2>, IEnumerable
{
	public new X509Certificate2 this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509Certificate2Collection()
	{
	}

	public X509Certificate2Collection(X509Certificate2 certificate)
	{
	}

	public X509Certificate2Collection(X509Certificate2Collection certificates)
	{
	}

	public X509Certificate2Collection(X509Certificate2[] certificates)
	{
	}

	public int Add(X509Certificate2 certificate)
	{
		throw null;
	}

	public void AddRange(X509Certificate2Collection certificates)
	{
	}

	public void AddRange(X509Certificate2[] certificates)
	{
	}

	public bool Contains(X509Certificate2 certificate)
	{
		throw null;
	}

	public byte[]? Export(X509ContentType contentType)
	{
		throw null;
	}

	public byte[]? Export(X509ContentType contentType, string? password)
	{
		throw null;
	}

	public string ExportCertificatePems()
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public string ExportPkcs7Pem()
	{
		throw null;
	}

	public X509Certificate2Collection Find(X509FindType findType, object findValue, bool validOnly)
	{
		throw null;
	}

	public new X509Certificate2Enumerator GetEnumerator()
	{
		throw null;
	}

	public void Import(byte[] rawData)
	{
	}

	public void Import(byte[] rawData, string? password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet)
	{
	}

	public void Import(ReadOnlySpan<byte> rawData)
	{
	}

	public void Import(ReadOnlySpan<byte> rawData, ReadOnlySpan<char> password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet)
	{
	}

	public void Import(ReadOnlySpan<byte> rawData, string? password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet)
	{
	}

	public void Import(string fileName)
	{
	}

	public void Import(string fileName, ReadOnlySpan<char> password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet)
	{
	}

	public void Import(string fileName, string? password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet)
	{
	}

	public void ImportFromPem(ReadOnlySpan<char> certPem)
	{
	}

	public void ImportFromPemFile(string certPemFilePath)
	{
	}

	public void Insert(int index, X509Certificate2 certificate)
	{
	}

	public void Remove(X509Certificate2 certificate)
	{
	}

	public void RemoveRange(X509Certificate2Collection certificates)
	{
	}

	public void RemoveRange(X509Certificate2[] certificates)
	{
	}

	IEnumerator<X509Certificate2> IEnumerable<X509Certificate2>.GetEnumerator()
	{
		throw null;
	}

	public bool TryExportCertificatePems(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public bool TryExportPkcs7Pem(Span<char> destination, out int charsWritten)
	{
		throw null;
	}
}
