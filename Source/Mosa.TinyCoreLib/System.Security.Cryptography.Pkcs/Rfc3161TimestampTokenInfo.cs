using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class Rfc3161TimestampTokenInfo
{
	public long? AccuracyInMicroseconds
	{
		get
		{
			throw null;
		}
	}

	public bool HasExtensions
	{
		get
		{
			throw null;
		}
	}

	public Oid HashAlgorithmId
	{
		get
		{
			throw null;
		}
	}

	public bool IsOrdering
	{
		get
		{
			throw null;
		}
	}

	public Oid PolicyId
	{
		get
		{
			throw null;
		}
	}

	public DateTimeOffset Timestamp
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

	public Rfc3161TimestampTokenInfo(Oid policyId, Oid hashAlgorithmId, ReadOnlyMemory<byte> messageHash, ReadOnlyMemory<byte> serialNumber, DateTimeOffset timestamp, long? accuracyInMicroseconds = null, bool isOrdering = false, ReadOnlyMemory<byte>? nonce = null, ReadOnlyMemory<byte>? timestampAuthorityName = null, X509ExtensionCollection? extensions = null)
	{
	}

	public byte[] Encode()
	{
		throw null;
	}

	public X509ExtensionCollection GetExtensions()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> GetMessageHash()
	{
		throw null;
	}

	public ReadOnlyMemory<byte>? GetNonce()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> GetSerialNumber()
	{
		throw null;
	}

	public ReadOnlyMemory<byte>? GetTimestampAuthorityName()
	{
		throw null;
	}

	public static bool TryDecode(ReadOnlyMemory<byte> encodedBytes, [NotNullWhen(true)] out Rfc3161TimestampTokenInfo? timestampTokenInfo, out int bytesConsumed)
	{
		throw null;
	}

	public bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
