namespace System.Security.Cryptography;

public static class PemEncoding
{
	public static PemFields Find(ReadOnlySpan<char> pemData)
	{
		throw null;
	}

	public static int GetEncodedSize(int labelLength, int dataLength)
	{
		throw null;
	}

	public static bool TryFind(ReadOnlySpan<char> pemData, out PemFields fields)
	{
		throw null;
	}

	public static bool TryWrite(ReadOnlySpan<char> label, ReadOnlySpan<byte> data, Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public static char[] Write(ReadOnlySpan<char> label, ReadOnlySpan<byte> data)
	{
		throw null;
	}

	public static string WriteString(ReadOnlySpan<char> label, ReadOnlySpan<byte> data)
	{
		throw null;
	}
}
