namespace System.Buffers;

public sealed class ArrayBufferWriter<T> : IBufferWriter<T>
{
	public int Capacity
	{
		get
		{
			throw null;
		}
	}

	public int FreeCapacity
	{
		get
		{
			throw null;
		}
	}

	public int WrittenCount
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<T> WrittenMemory
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlySpan<T> WrittenSpan
	{
		get
		{
			throw null;
		}
	}

	public ArrayBufferWriter()
	{
	}

	public ArrayBufferWriter(int initialCapacity)
	{
	}

	public void Advance(int count)
	{
	}

	public void Clear()
	{
	}

	public void ResetWrittenCount()
	{
	}

	public Memory<T> GetMemory(int sizeHint = 0)
	{
		throw null;
	}

	public Span<T> GetSpan(int sizeHint = 0)
	{
		throw null;
	}
}
