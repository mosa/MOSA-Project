namespace System.Buffers;

public abstract class MemoryManager<T> : IMemoryOwner<T>, IDisposable, IPinnable
{
	public virtual Memory<T> Memory
	{
		get
		{
			throw null;
		}
	}

	protected Memory<T> CreateMemory(int length)
	{
		throw null;
	}

	protected Memory<T> CreateMemory(int start, int length)
	{
		throw null;
	}

	protected abstract void Dispose(bool disposing);

	public abstract Span<T> GetSpan();

	public abstract MemoryHandle Pin(int elementIndex = 0);

	void IDisposable.Dispose()
	{
	}

	protected internal virtual bool TryGetArray(out ArraySegment<T> segment)
	{
		throw null;
	}

	public abstract void Unpin();
}
