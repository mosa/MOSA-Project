namespace System.Security.Cryptography.Cose;

public abstract class CoseMessage
{
	public ReadOnlyMemory<byte>? Content
	{
		get
		{
			throw null;
		}
	}

	public CoseHeaderMap ProtectedHeaders
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte> RawProtectedHeaders
	{
		get
		{
			throw null;
		}
	}

	public CoseHeaderMap UnprotectedHeaders
	{
		get
		{
			throw null;
		}
	}

	internal CoseMessage()
	{
	}

	public static CoseMultiSignMessage DecodeMultiSign(byte[] cborPayload)
	{
		throw null;
	}

	public static CoseMultiSignMessage DecodeMultiSign(ReadOnlySpan<byte> cborPayload)
	{
		throw null;
	}

	public static CoseSign1Message DecodeSign1(byte[] cborPayload)
	{
		throw null;
	}

	public static CoseSign1Message DecodeSign1(ReadOnlySpan<byte> cborPayload)
	{
		throw null;
	}

	public byte[] Encode()
	{
		throw null;
	}

	public int Encode(Span<byte> destination)
	{
		throw null;
	}

	public abstract int GetEncodedLength();

	public abstract bool TryEncode(Span<byte> destination, out int bytesWritten);
}
