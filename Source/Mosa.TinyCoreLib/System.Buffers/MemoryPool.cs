namespace System.Buffers;

public abstract class MemoryPool<T> : IDisposable
{
	public abstract int MaxBufferSize { get; }

	public static MemoryPool<T> Shared
	{
		get
		{
			throw null;
		}
	}

	public void Dispose()
	{
	}

	protected abstract void Dispose(bool disposing);

	public abstract IMemoryOwner<T> Rent(int minBufferSize = -1);
}
