using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Concurrent;

public class ConcurrentStack<T> : IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
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

	public ConcurrentStack()
	{
	}

	public ConcurrentStack(IEnumerable<T> collection)
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

	public void Push(T item)
	{
	}

	public void PushRange(T[] items)
	{
	}

	public void PushRange(T[] items, int startIndex, int count)
	{
	}

	bool IProducerConsumerCollection<T>.TryAdd(T item)
	{
		throw null;
	}

	bool IProducerConsumerCollection<T>.TryTake([MaybeNullWhen(false)] out T item)
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

	public bool TryPop([MaybeNullWhen(false)] out T result)
	{
		throw null;
	}

	public int TryPopRange(T[] items)
	{
		throw null;
	}

	public int TryPopRange(T[] items, int startIndex, int count)
	{
		throw null;
	}
}
