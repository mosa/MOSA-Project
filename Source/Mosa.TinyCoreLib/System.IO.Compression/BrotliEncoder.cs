using System.Buffers;

namespace System.IO.Compression;

public struct BrotliEncoder : IDisposable
{
	private object _dummy;

	private int _dummyPrimitive;

	public BrotliEncoder(int quality, int window)
	{
		throw null;
	}

	public OperationStatus Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, bool isFinalBlock)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public OperationStatus Flush(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static int GetMaxCompressedLength(int inputSize)
	{
		throw null;
	}

	public static bool TryCompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static bool TryCompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, int quality, int window)
	{
		throw null;
	}
}
