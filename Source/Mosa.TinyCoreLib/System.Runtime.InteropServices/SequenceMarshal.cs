using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

public static class SequenceMarshal
{
	public static bool TryGetArray<T>(ReadOnlySequence<T> sequence, out ArraySegment<T> segment)
	{
		throw null;
	}

	public static bool TryGetReadOnlyMemory<T>(ReadOnlySequence<T> sequence, out ReadOnlyMemory<T> memory)
	{
		throw null;
	}

	public static bool TryGetReadOnlySequenceSegment<T>(ReadOnlySequence<T> sequence, [NotNullWhen(true)] out ReadOnlySequenceSegment<T>? startSegment, out int startIndex, [NotNullWhen(true)] out ReadOnlySequenceSegment<T>? endSegment, out int endIndex)
	{
		throw null;
	}

	public static bool TryRead<T>(ref SequenceReader<byte> reader, out T value) where T : unmanaged
	{
		throw null;
	}
}
