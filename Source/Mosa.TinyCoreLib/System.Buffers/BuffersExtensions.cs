namespace System.Buffers;

public static class BuffersExtensions
{
	public static void CopyTo<T>(this in ReadOnlySequence<T> source, Span<T> destination)
	{
	}

	public static SequencePosition? PositionOf<T>(this in ReadOnlySequence<T> source, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static T[] ToArray<T>(this in ReadOnlySequence<T> sequence)
	{
		throw null;
	}

	public static void Write<T>(this IBufferWriter<T> writer, ReadOnlySpan<T> value)
	{
	}
}
