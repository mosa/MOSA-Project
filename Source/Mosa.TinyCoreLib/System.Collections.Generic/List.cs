using System.Collections.ObjectModel;

namespace System.Collections.Generic;

public class List<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		public T Current => current;

		object? IEnumerator.Current => current;

		private T current;
		private int index;
		private List<T> list;

		internal Enumerator(List<T> instance) => list = instance;

		public void Dispose()
		{
			list.enumerating = false;
			list.modified = false;
		}

		public bool MoveNext()
		{
			if (!list.enumerating || list.modified)
				Internal.Exceptions.List.Enumerator.ThrowListModifiedException();

			if (index < list.backingCount)
			{
				current = list.backingArray[index++];
				return true;
			}

			return false;
		}

		void IEnumerator.Reset()
		{
			if (!list.enumerating || list.modified)
				Internal.Exceptions.List.Enumerator.ThrowListModifiedException();

			index = 0;
		}
	}

	public int Capacity
	{
		get => backingArray.Length;
		set
		{
			ArgumentOutOfRangeException.ThrowIfLessThan(value, backingCount, nameof(value));

			EnsureCapacity(value);
		}
	}

	public int Count => backingCount;

	public T this[int index]
	{
		get
		{
			ArgumentOutOfRangeException.ThrowIfLessThan(index, 0, nameof(index));
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));

			return backingArray[index];
		}
		set
		{
			ArgumentOutOfRangeException.ThrowIfLessThan(index, 0, nameof(index));
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));

			backingArray[index] = value;

			if (enumerating)
				modified = true;
		}
	}

	bool ICollection<T>.IsReadOnly => false;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot => this;

	bool IList.IsFixedSize => false;

	bool IList.IsReadOnly => false;

	object? IList.this[int index]
	{
		get => this[index];
		set
		{
			if (value is not T item)
			{
				Internal.Exceptions.List.ThrowValueIncorrectTypeException(nameof(value));

				// For some reason, Roslyn thinks "item" may be uninitialized, even though ThrowValueIncorrectType()
				// is marked with the "DoesNotReturn" attribute
				return;
			}

			this[index] = item;

			if (enumerating)
				modified = true;
		}
	}

	private int backingCount;
	private bool enumerating, modified;
	private T[] backingArray;

	public List() => backingArray = new T[Internal.Impl.List.InitialArraySize];

	public List(IEnumerable<T> collection)
	{
		ArgumentNullException.ThrowIfNull(collection, nameof(collection));

		backingArray = new T[Internal.Impl.List.InitialArraySize];

		foreach (var item in collection)
			Add(item);
	}

	public List(int capacity)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(capacity);

		backingArray = new T[capacity];
	}

	public void Add(T item)
	{
		var nextCount = backingCount + 1;
		EnsureCapacity(nextCount);

		backingArray[backingCount] = item;
		backingCount = nextCount;

		if (enumerating)
			modified = true;
	}

	public void AddRange(IEnumerable<T> collection)
	{
		ArgumentNullException.ThrowIfNull(collection, nameof(collection));

		foreach (var item in collection)
			Add(item);
	}

	public ReadOnlyCollection<T> AsReadOnly() => new(this);

	public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(count, backingCount, nameof(count));

		var theComparer = comparer ?? Comparer<T>.Default;
		var middle = -1;
		var found = false;
		var imin = index;
		var imax = count;

		while (imin <= imax)
		{
			middle = (imin + imax) / 2;
			var value = theComparer.Compare(backingArray[middle], item);

			if (value == 0)
			{
				found = true;
				break;
			}

			if (value < 0)
				imin = middle + 1;
			else
				imax = middle - 1;
		}

		if (middle == -1)
			return ~backingCount;

		return found ? middle : ~middle;
	}

	public int BinarySearch(T item) => BinarySearch(item, null);

	public int BinarySearch(T item, IComparer<T>? comparer) => BinarySearch(0, backingCount, item, comparer);

	public void Clear()
	{
		for (var i = 0; i < backingCount; i++)
			backingArray[i] = default;

		backingCount = 0;

		if (enumerating)
			modified = true;
	}

	public bool Contains(T item)
	{
		for (var i = 0; i < backingCount; i++)
			if (EqualityComparer<T>.Default.Equals(backingArray[i], item))
				return true;

		return false;
	}

	public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
	{
		ArgumentNullException.ThrowIfNull(converter, nameof(converter));

		var newList = new List<TOutput>();

		for (var i = 0; i < backingCount; i++)
			newList.Add(converter(backingArray[i]));

		return newList;
	}

	public void CopyTo(int index, T[] array, int arrayIndex, int count)
	{
		ArgumentNullException.ThrowIfNull(array, nameof(array));
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex, nameof(arrayIndex));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

		if (index >= backingCount || backingCount - index > array.Length - arrayIndex)
			Internal.Exceptions.List.ThrowIndexOutOfBoundsException(nameof(index));

		Array.Copy(backingArray, index, array, arrayIndex, index + count);
	}

	public void CopyTo(T[] array) => CopyTo(0, array, 0, backingCount);

	public void CopyTo(T[] array, int arrayIndex) => CopyTo(0, array, arrayIndex, backingCount);

	public int EnsureCapacity(int capacity)
	{
		if (capacity <= backingArray.Length)
			return capacity;

		var nextCapacity = (int)(capacity * Internal.Impl.List.NextCapacityMultiplySize);
		var newArray = new T[nextCapacity];

		Array.Copy(backingArray, newArray, backingCount);
		backingArray = newArray;

		return nextCapacity;
	}

	public bool Exists(Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));

		for (var i = 0; i < backingCount; i++)
			if (match(backingArray[i]))
				return true;

		return false;
	}

	public T? Find(Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));

		for (var i = 0; i < backingCount; i++)
		{
			var item = backingArray[i];
			if (match(item))
				return item;
		}

		return default;
	}

	public List<T> FindAll(Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));

		var newList = new List<T>();

		for (var i = 0; i < backingCount; i++)
		{
			var item = backingArray[i];
			if (match(item))
				newList.Add(item);
		}

		return newList;
	}

	public int FindIndex(int startIndex, int count, Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));
		ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, backingCount, nameof(startIndex));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(count, backingCount, nameof(count));

		for (var i = startIndex; i < startIndex + count; i++)
			if (match(backingArray[i]))
				return i;

		return -1;
	}

	public int FindIndex(int startIndex, Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));
		ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, backingCount, nameof(startIndex));

		for (var i = startIndex; i < backingCount; i++)
			if (match(backingArray[i]))
				return i;

		return -1;
	}

	public int FindIndex(Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));

		for (var i = 0; i < backingCount; i++)
			if (match(backingArray[i]))
				return i;

		return -1;
	}

	public T? FindLast(Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));

		for (var i = backingCount - 1; i >= 0; i--)
		{
			var item = backingArray[i];
			if (match(item))
				return item;
		}

		return default;
	}

	public int FindLastIndex(int startIndex, int count, Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));
		ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, backingCount, nameof(startIndex));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(count, backingCount, nameof(count));

		for (var i = startIndex + count - 1; i >= startIndex; i--)
			if (match(backingArray[i]))
				return i;

		return -1;
	}

	public int FindLastIndex(int startIndex, Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));
		ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, backingCount, nameof(startIndex));

		for (var i = backingCount - 1; i >= startIndex; i--)
			if (match(backingArray[i]))
				return i;

		return -1;
	}

	public int FindLastIndex(Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));

		for (var i = backingCount - 1; i >= 0; i--)
			if (match(backingArray[i]))
				return i;

		return -1;
	}

	public void ForEach(Action<T> action)
	{
		ArgumentNullException.ThrowIfNull(action, nameof(action));

		enumerating = true;
		for (var i = 0; i < backingCount; i++)
		{
			// We're not *technically* using the enumerator... but close enough
			if (modified)
				Internal.Exceptions.List.Enumerator.ThrowListModifiedException();

			action(backingArray[i]);
		}
		enumerating = false;
		modified = false;
	}

	public Enumerator GetEnumerator()
	{
		enumerating = true;
		return new(this);
	}

	// TODO: Use Slice()?
	public List<T> GetRange(int index, int count)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(count, backingCount, nameof(count));

		var newList = new List<T>();

		for (var i = index; i < index + count; i++)
			newList.Add(backingArray[i]);

		return newList;
	}

	public int IndexOf(T item)
	{
		for (var i = 0; i < backingCount; i++)
			if (EqualityComparer<T>.Default.Equals(backingArray[i], item))
				return i;

		return -1;
	}

	public int IndexOf(T item, int index)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));

		for (var i = index; i < backingCount; i++)
			if (EqualityComparer<T>.Default.Equals(backingArray[i], item))
				return i;

		return -1;
	}

	public int IndexOf(T item, int index, int count)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(count, backingCount, nameof(count));

		for (var i = index; i < index + count; i++)
			if (EqualityComparer<T>.Default.Equals(backingArray[i], item))
				return i;

		return -1;
	}

	public void Insert(int index, T item)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(index, backingCount, nameof(index));

		var nextCount = backingCount + 1;
		EnsureCapacity(nextCount);

		for (var i = backingCount - 1; i >= index; i--)
			backingArray[i + 1] = backingArray[i];

		backingArray[index] = item;
		backingCount = nextCount;

		if (enumerating)
			modified = true;
	}

	public void InsertRange(int index, IEnumerable<T> collection)
	{
		ArgumentNullException.ThrowIfNull(collection, nameof(collection));
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(index, backingCount, nameof(index));

		var i = 0;
		foreach (var item in collection)
			Insert(index + i++, item);
	}

	public int LastIndexOf(T item)
	{
		for (var i = backingCount - 1; i >= 0; i--)
			if (EqualityComparer<T>.Default.Equals(backingArray[i], item))
				return i;

		return -1;
	}

	public int LastIndexOf(T item, int index)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));

		for (var i = backingCount - 1; i >= index; i--)
			if (EqualityComparer<T>.Default.Equals(backingArray[i], item))
				return i;

		return -1;
	}

	public int LastIndexOf(T item, int index, int count)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(count, backingCount, nameof(count));

		for (var i = index + count - 1; i >= index; i--)
			if (EqualityComparer<T>.Default.Equals(backingArray[i], item))
				return i;

		return -1;
	}

	public bool Remove(T item)
	{
		var index = IndexOf(item);
		if (index == -1)
			return false;

		RemoveAt(index);
		return true;
	}

	public int RemoveAll(Predicate<T> match)
	{
		ArgumentNullException.ThrowIfNull(match, nameof(match));

		var elements = 0;

		for (var i = 0; i < backingCount; i++)
			if (match(backingArray[i]))
			{
				RemoveAt(i);
				elements++;
			}

		return elements;
	}

	public void RemoveAt(int index)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));

		backingArray[index] = default;

		var nextCount = backingCount - 1;

		for (var i = index; i < nextCount; i++)
			backingArray[i] = backingArray[i + 1];

		backingCount = nextCount;

		if (enumerating)
			modified = true;
	}

	public void RemoveRange(int index, int count)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(count, backingCount, nameof(count));

		for (var i = index; i < index + count; i++)
			RemoveAt(index);
	}

	public void Reverse()
	{
		Array.Reverse(backingArray);

		if (enumerating)
			modified = true;
	}

	public void Reverse(int index, int count)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(count, backingCount, nameof(count));

		Array.Reverse(backingArray, index, index + count);

		if (enumerating)
			modified = true;
	}

	public List<T> Slice(int start, int length)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(start, nameof(start));
		ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(start, backingCount, nameof(start));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(length, backingCount, nameof(length));

		var newList = new List<T>();

		for (var i = start; i < length; i++)
			newList.Add(backingArray[i]);

		return newList;
	}

	public void Sort() => Sort(comparer: null);

	public void Sort(IComparer<T>? comparer) => Sort(0, backingCount, comparer);

	public void Sort(Comparison<T> comparison) => Sort(Comparer<T>.Create(comparison));

	public void Sort(int index, int count, IComparer<T>? comparer)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingCount, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(count, backingCount, nameof(count));

		var theComparer = comparer ?? Comparer<T>.Default;

		for (var i = index; i < count; i++)
		{
			var value = backingArray[i];

			// Find the index of the minimum value
			var iMin = i;
			for (var j = i + 1; j < count; j++)
				if (theComparer.Compare(backingArray[j], value) < 0)
					iMin = j;

			// If the minimum value is the one we're searching a minimum for, it means we've
			// finished shorting the list and can safely exit
			if (i == iMin)
				break;

			(backingArray[i], backingArray[iMin]) = (backingArray[iMin], backingArray[i]);
		}
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

	void ICollection.CopyTo(Array array, int arrayIndex)
	{
		ArgumentNullException.ThrowIfNull(array, nameof(array));
		ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex, nameof(arrayIndex));

		if (array.Rank != 1)
			Internal.Exceptions.Generic.ThrowMultiDimensionalArrayException(nameof(array));

		if (array.GetLowerBound(0) != 0)
			Internal.Exceptions.Generic.ThrowArrayNotZeroBasedIndexingException(nameof(array));

		if (array.GetType().GetElementType() is not T)
			Internal.Exceptions.Generic.ThrowArrayTypeMismatchException(nameof(array));

		if (backingCount > array.Length - arrayIndex)
			Internal.Exceptions.List.ThrowListTooBigException(nameof(Count));

		Array.Copy(backingArray, 0, array, arrayIndex, backingCount);
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	int IList.Add(object? item)
	{
		if (item is not T value)
		{
			Internal.Exceptions.List.ThrowValueIncorrectTypeException(nameof(item));
			return -1;
		}

		Add(value);
		return backingCount - 1;
	}

	bool IList.Contains(object? item)
	{
		if (item is not T value)
		{
			Internal.Exceptions.List.ThrowValueIncorrectTypeException(nameof(item));
			return false;
		}

		return Contains(value);
	}

	int IList.IndexOf(object? item)
	{
		if (item is not T value)
		{
			Internal.Exceptions.List.ThrowValueIncorrectTypeException(nameof(item));
			return -1;
		}

		return IndexOf(value);
	}

	void IList.Insert(int index, object? item)
	{
		if (item is not T value)
		{
			Internal.Exceptions.List.ThrowValueIncorrectTypeException(nameof(item));
			return;
		}

		Insert(index, value);
	}

	void IList.Remove(object? item)
	{
		if (item is not T value)
		{
			Internal.Exceptions.List.ThrowValueIncorrectTypeException(nameof(item));
			return;
		}

		Remove(value);
	}

	public T[] ToArray()
	{
		var newArray = new T[backingCount];

		Array.Copy(backingArray, newArray, backingCount);
		return newArray;
	}

	public void TrimExcess()
	{
		if (backingCount == 0)
		{
			backingArray = new T[Internal.Impl.List.InitialArraySize];
			return;
		}

		var capacity = (int)(backingArray.Length * Internal.Impl.List.CapacityTrimThreshold);
		if (backingCount >= capacity)
			return;

		var newArray = new T[backingCount];

		Array.Copy(backingArray, newArray, backingCount);
		backingArray = newArray;
	}

	public bool TrueForAll(Predicate<T> match)
	{
		for (var i = 0; i < backingCount; i++)
			if (!match(backingArray[i]))
				return false;

		return true;
	}
}
