namespace System.Buffers.Text;

public static class Base64
{
	public static OperationStatus DecodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> bytes, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true)
	{
		throw null;
	}

	public static OperationStatus DecodeFromUtf8InPlace(Span<byte> buffer, out int bytesWritten)
	{
		throw null;
	}

	public static OperationStatus EncodeToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true)
	{
		throw null;
	}

	public static OperationStatus EncodeToUtf8InPlace(Span<byte> buffer, int dataLength, out int bytesWritten)
	{
		throw null;
	}

	public static int GetMaxDecodedFromUtf8Length(int length)
	{
		throw null;
	}

	public static int GetMaxEncodedToUtf8Length(int length)
	{
		throw null;
	}

	public static bool IsValid(ReadOnlySpan<char> base64Text)
	{
		throw null;
	}

	public static bool IsValid(ReadOnlySpan<char> base64Text, out int decodedLength)
	{
		throw null;
	}

	public static bool IsValid(ReadOnlySpan<byte> base64TextUtf8)
	{
		throw null;
	}

	public static bool IsValid(ReadOnlySpan<byte> base64TextUtf8, out int decodedLength)
	{
		throw null;
	}
}
