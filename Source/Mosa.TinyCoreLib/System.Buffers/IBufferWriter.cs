namespace System.Buffers;

public interface IBufferWriter<T>
{
	void Advance(int count);

	Memory<T> GetMemory(int sizeHint = 0);

	Span<T> GetSpan(int sizeHint = 0);
}
