using System.Buffers;

namespace System.Text;

public static class Ascii
{
	public static bool Equals(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
	{
		throw null;
	}

	public static bool Equals(ReadOnlySpan<byte> left, ReadOnlySpan<char> right)
	{
		throw null;
	}

	public static bool Equals(ReadOnlySpan<char> left, ReadOnlySpan<byte> right)
	{
		throw null;
	}

	public static bool Equals(ReadOnlySpan<char> left, ReadOnlySpan<char> right)
	{
		throw null;
	}

	public static bool EqualsIgnoreCase(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
	{
		throw null;
	}

	public static bool EqualsIgnoreCase(ReadOnlySpan<byte> left, ReadOnlySpan<char> right)
	{
		throw null;
	}

	public static bool EqualsIgnoreCase(ReadOnlySpan<char> left, ReadOnlySpan<byte> right)
	{
		throw null;
	}

	public static bool EqualsIgnoreCase(ReadOnlySpan<char> left, ReadOnlySpan<char> right)
	{
		throw null;
	}

	public static bool IsValid(ReadOnlySpan<byte> value)
	{
		throw null;
	}

	public static bool IsValid(ReadOnlySpan<char> value)
	{
		throw null;
	}

	public static bool IsValid(byte value)
	{
		throw null;
	}

	public static bool IsValid(char value)
	{
		throw null;
	}

	public static OperationStatus ToLower(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static OperationStatus ToLower(ReadOnlySpan<char> source, Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public static OperationStatus ToLower(ReadOnlySpan<byte> source, Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public static OperationStatus ToLower(ReadOnlySpan<char> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static OperationStatus ToUpper(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static OperationStatus ToUpper(ReadOnlySpan<char> source, Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public static OperationStatus ToUpper(ReadOnlySpan<byte> source, Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public static OperationStatus ToUpper(ReadOnlySpan<char> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static OperationStatus ToLowerInPlace(Span<byte> value, out int bytesWritten)
	{
		throw null;
	}

	public static OperationStatus ToLowerInPlace(Span<char> value, out int charsWritten)
	{
		throw null;
	}

	public static OperationStatus ToUpperInPlace(Span<byte> value, out int bytesWritten)
	{
		throw null;
	}

	public static OperationStatus ToUpperInPlace(Span<char> value, out int charsWritten)
	{
		throw null;
	}

	public static OperationStatus FromUtf16(ReadOnlySpan<char> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static OperationStatus ToUtf16(ReadOnlySpan<byte> source, Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public static Range Trim(ReadOnlySpan<byte> value)
	{
		throw null;
	}

	public static Range Trim(ReadOnlySpan<char> value)
	{
		throw null;
	}

	public static Range TrimEnd(ReadOnlySpan<byte> value)
	{
		throw null;
	}

	public static Range TrimEnd(ReadOnlySpan<char> value)
	{
		throw null;
	}

	public static Range TrimStart(ReadOnlySpan<byte> value)
	{
		throw null;
	}

	public static Range TrimStart(ReadOnlySpan<char> value)
	{
		throw null;
	}
}
