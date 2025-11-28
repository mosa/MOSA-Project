using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Threading;

namespace System.Collections.Concurrent;

[UnsupportedOSPlatform("browser")]
public class BlockingCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDisposable
{
	public int BoundedCapacity
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsAddingCompleted
	{
		get
		{
			throw null;
		}
	}

	public bool IsCompleted
	{
		get
		{
			throw null;
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public BlockingCollection()
	{
	}

	public BlockingCollection(IProducerConsumerCollection<T> collection)
	{
	}

	public BlockingCollection(IProducerConsumerCollection<T> collection, int boundedCapacity)
	{
	}

	public BlockingCollection(int boundedCapacity)
	{
	}

	public void Add(T item)
	{
	}

	public void Add(T item, CancellationToken cancellationToken)
	{
	}

	public static int AddToAny(BlockingCollection<T>[] collections, T item)
	{
		throw null;
	}

	public static int AddToAny(BlockingCollection<T>[] collections, T item, CancellationToken cancellationToken)
	{
		throw null;
	}

	public void CompleteAdding()
	{
	}

	public void CopyTo(T[] array, int index)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public IEnumerable<T> GetConsumingEnumerable()
	{
		throw null;
	}

	public IEnumerable<T> GetConsumingEnumerable(CancellationToken cancellationToken)
	{
		throw null;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public T Take()
	{
		throw null;
	}

	public T Take(CancellationToken cancellationToken)
	{
		throw null;
	}

	public static int TakeFromAny(BlockingCollection<T>[] collections, out T? item)
	{
		throw null;
	}

	public static int TakeFromAny(BlockingCollection<T>[] collections, out T? item, CancellationToken cancellationToken)
	{
		throw null;
	}

	public T[] ToArray()
	{
		throw null;
	}

	public bool TryAdd(T item)
	{
		throw null;
	}

	public bool TryAdd(T item, int millisecondsTimeout)
	{
		throw null;
	}

	public bool TryAdd(T item, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public bool TryAdd(T item, TimeSpan timeout)
	{
		throw null;
	}

	public static int TryAddToAny(BlockingCollection<T>[] collections, T item)
	{
		throw null;
	}

	public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout)
	{
		throw null;
	}

	public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static int TryAddToAny(BlockingCollection<T>[] collections, T item, TimeSpan timeout)
	{
		throw null;
	}

	public bool TryTake([MaybeNullWhen(false)] out T item)
	{
		throw null;
	}

	public bool TryTake([MaybeNullWhen(false)] out T item, int millisecondsTimeout)
	{
		throw null;
	}

	public bool TryTake([MaybeNullWhen(false)] out T item, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public bool TryTake([MaybeNullWhen(false)] out T item, TimeSpan timeout)
	{
		throw null;
	}

	public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T? item)
	{
		throw null;
	}

	public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T? item, int millisecondsTimeout)
	{
		throw null;
	}

	public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T? item, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T? item, TimeSpan timeout)
	{
		throw null;
	}
}
