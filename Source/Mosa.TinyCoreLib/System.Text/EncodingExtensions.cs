using System.Buffers;

namespace System.Text;

public static class EncodingExtensions
{
	public static void Convert(this Decoder decoder, in ReadOnlySequence<byte> bytes, IBufferWriter<char> writer, bool flush, out long charsUsed, out bool completed)
	{
		throw null;
	}

	public static void Convert(this Decoder decoder, ReadOnlySpan<byte> bytes, IBufferWriter<char> writer, bool flush, out long charsUsed, out bool completed)
	{
		throw null;
	}

	public static void Convert(this Encoder encoder, in ReadOnlySequence<char> chars, IBufferWriter<byte> writer, bool flush, out long bytesUsed, out bool completed)
	{
		throw null;
	}

	public static void Convert(this Encoder encoder, ReadOnlySpan<char> chars, IBufferWriter<byte> writer, bool flush, out long bytesUsed, out bool completed)
	{
		throw null;
	}

	public static byte[] GetBytes(this Encoding encoding, in ReadOnlySequence<char> chars)
	{
		throw null;
	}

	public static long GetBytes(this Encoding encoding, in ReadOnlySequence<char> chars, IBufferWriter<byte> writer)
	{
		throw null;
	}

	public static int GetBytes(this Encoding encoding, in ReadOnlySequence<char> chars, Span<byte> bytes)
	{
		throw null;
	}

	public static long GetBytes(this Encoding encoding, ReadOnlySpan<char> chars, IBufferWriter<byte> writer)
	{
		throw null;
	}

	public static long GetChars(this Encoding encoding, in ReadOnlySequence<byte> bytes, IBufferWriter<char> writer)
	{
		throw null;
	}

	public static int GetChars(this Encoding encoding, in ReadOnlySequence<byte> bytes, Span<char> chars)
	{
		throw null;
	}

	public static long GetChars(this Encoding encoding, ReadOnlySpan<byte> bytes, IBufferWriter<char> writer)
	{
		throw null;
	}

	public static string GetString(this Encoding encoding, in ReadOnlySequence<byte> bytes)
	{
		throw null;
	}
}
