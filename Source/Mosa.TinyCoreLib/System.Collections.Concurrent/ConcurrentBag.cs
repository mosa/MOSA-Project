using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Concurrent;

public class ConcurrentBag<T> : IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsEmpty
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

	public ConcurrentBag()
	{
	}

	public ConcurrentBag(IEnumerable<T> collection)
	{
	}

	public void Add(T item)
	{
	}

	public void Clear()
	{
	}

	public void CopyTo(T[] array, int index)
	{
	}

	public IEnumerator<T> GetEnumerator()
	{
		throw null;
	}

	bool IProducerConsumerCollection<T>.TryAdd(T item)
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

	public T[] ToArray()
	{
		throw null;
	}

	public bool TryPeek([MaybeNullWhen(false)] out T result)
	{
		throw null;
	}

	public bool TryTake([MaybeNullWhen(false)] out T result)
	{
		throw null;
	}
}
