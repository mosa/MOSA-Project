namespace System.Security.Cryptography.Pkcs;

public sealed class ContentInfo
{
	public byte[] Content
	{
		get
		{
			throw null;
		}
	}

	public Oid ContentType
	{
		get
		{
			throw null;
		}
	}

	public ContentInfo(byte[] content)
	{
	}

	public ContentInfo(Oid contentType, byte[] content)
	{
	}

	public static Oid GetContentType(byte[] encodedMessage)
	{
		throw null;
	}

	public static Oid GetContentType(ReadOnlySpan<byte> encodedMessage)
	{
		throw null;
	}
}
