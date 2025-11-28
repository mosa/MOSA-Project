using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public class Queue<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		public T Current => current;

		object? IEnumerator.Current => current;

		private T current;
		private int index;
		private Queue<T> queue;

		internal Enumerator(Queue<T> instance) => queue = instance;

		public void Dispose()
		{
			queue.enumerating = false;
			queue.modified = false;
		}

		public bool MoveNext()
		{
			if (!queue.enumerating || queue.modified)
				Internal.Exceptions.Queue.Enumerator.ThrowQueueModifiedException();

			if (index < queue.count)
			{
				current = queue.backingArray[index++];
				return true;
			}

			return false;
		}

		void IEnumerator.Reset()
		{
			if (!queue.enumerating || queue.modified)
				Internal.Exceptions.Queue.Enumerator.ThrowQueueModifiedException();

			index = 0;
		}
	}

	public int Count => count;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot => this;

	private int count, head;
	private bool enumerating, modified;
	private T[] backingArray;

	public Queue() => backingArray = new T[Internal.Impl.Queue.InitialArraySize];

	public Queue(IEnumerable<T> collection)
	{
		ArgumentNullException.ThrowIfNull(collection, nameof(collection));

		backingArray = new T[Internal.Impl.Queue.InitialArraySize];

		foreach (var item in collection)
			Enqueue(item);
	}

	public Queue(int capacity)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(capacity);

		backingArray = new T[capacity];
	}

	public void Clear()
	{
		for (var i = 0; i < count; i++)
			backingArray[i] = default;

		count = 0;

		if (enumerating)
			modified = true;
	}

	public bool Contains(T item)
	{
		for (var i = 0; i < count; i++)
			if (EqualityComparer<T>.Default.Equals(backingArray[i], item))
				return true;

		return false;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		ArgumentNullException.ThrowIfNull(array, nameof(array));
		ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex, nameof(arrayIndex));

		if (count > array.Length - arrayIndex)
			Internal.Exceptions.Queue.ThrowQueueTooBigException(nameof(Count));

		Array.Copy(backingArray, 0, array, arrayIndex, count);
	}

	public T Dequeue()
	{
		ArgumentOutOfRangeException.ThrowIfZero(count--, nameof(Count));

		var item = backingArray[head];
		head = (head + 1) % backingArray.Length;

		if (enumerating)
			modified = true;

		return item;
	}

	public void Enqueue(T item)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(count, backingArray.Length, nameof(Count));

		var nextCount = count + 1;
		EnsureCapacity(nextCount);

		backingArray[(head + count) % backingArray.Length] = item;
		count = nextCount;

		if (enumerating)
			modified = true;
	}

	public Enumerator GetEnumerator()
	{
		enumerating = true;
		return new(this);
	}

	public T Peek()
	{
		ArgumentOutOfRangeException.ThrowIfZero(count, nameof(Count));
		return backingArray[head];
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

	void ICollection.CopyTo(Array array, int index)
	{
		ArgumentNullException.ThrowIfNull(array, nameof(array));
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));

		if (array.Rank != 1)
			Internal.Exceptions.Generic.ThrowMultiDimensionalArrayException(nameof(array));

		if (array.GetLowerBound(0) != 0)
			Internal.Exceptions.Generic.ThrowArrayNotZeroBasedIndexingException(nameof(array));

		if (array.GetType().GetElementType() is not T)
			Internal.Exceptions.Generic.ThrowArrayTypeMismatchException(nameof(array));

		if (count > array.Length - index)
			Internal.Exceptions.Queue.ThrowQueueTooBigException(nameof(Count));

		Array.Copy(backingArray, 0, array, index, count);
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public T[] ToArray()
	{
		var newArray = new T[count];

		CopyTo(newArray, 0);
		return newArray;
	}

	public void TrimExcess()
	{
		if (count == 0)
		{
			backingArray = new T[Internal.Impl.Queue.InitialArraySize];
			return;
		}

		var capacity = (int)(backingArray.Length * 0.9);
		if (count >= capacity)
			return;

		var newArray = new T[count];

		Array.Copy(backingArray, newArray, count);
		backingArray = newArray;
	}

	public int EnsureCapacity(int capacity)
	{
		if (capacity <= backingArray.Length)
			return capacity;

		var nextCapacity = (int)(capacity * Internal.Impl.Queue.NextCapacityMultiplySize);
		var newArray = new T[nextCapacity];

		Array.Copy(backingArray, newArray, count);
		backingArray = newArray;

		return nextCapacity;
	}

	public bool TryDequeue([MaybeNullWhen(false)] out T result)
	{
		if (count == 0)
		{
			result = default;
			return false;
		}

		result = Dequeue();
		return true;
	}

	public bool TryPeek([MaybeNullWhen(false)] out T result)
	{
		if (count == 0)
		{
			result = default;
			return false;
		}

		result = Peek();
		return true;
	}
}
