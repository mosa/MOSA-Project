using System.Buffers;

namespace System.IO.Compression;

public struct BrotliDecoder : IDisposable
{
	private object _dummy;

	private int _dummyPrimitive;

	public OperationStatus Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public static bool TryDecompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
