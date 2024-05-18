using System.Collections.ObjectModel;

namespace System.Collections.Generic;

public class List<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private T _current;

		private object _dummy;

		private int _dummyPrimitive;

		public T Current
		{
			get
			{
				throw null;
			}
		}

		object? IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			throw null;
		}

		void IEnumerator.Reset()
		{
		}
	}

	public int Capacity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public T this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	bool ICollection<T>.IsReadOnly
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

	bool IList.IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	bool IList.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IList.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public List()
	{
	}

	public List(IEnumerable<T> collection)
	{
	}

	public List(int capacity)
	{
	}

	public void Add(T item)
	{
	}

	public void AddRange(IEnumerable<T> collection)
	{
	}

	public ReadOnlyCollection<T> AsReadOnly()
	{
		throw null;
	}

	public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
	{
		throw null;
	}

	public int BinarySearch(T item)
	{
		throw null;
	}

	public int BinarySearch(T item, IComparer<T>? comparer)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(T item)
	{
		throw null;
	}

	public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
	{
		throw null;
	}

	public void CopyTo(int index, T[] array, int arrayIndex, int count)
	{
	}

	public void CopyTo(T[] array)
	{
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
	}

	public int EnsureCapacity(int capacity)
	{
		throw null;
	}

	public bool Exists(Predicate<T> match)
	{
		throw null;
	}

	public T? Find(Predicate<T> match)
	{
		throw null;
	}

	public List<T> FindAll(Predicate<T> match)
	{
		throw null;
	}

	public int FindIndex(int startIndex, int count, Predicate<T> match)
	{
		throw null;
	}

	public int FindIndex(int startIndex, Predicate<T> match)
	{
		throw null;
	}

	public int FindIndex(Predicate<T> match)
	{
		throw null;
	}

	public T? FindLast(Predicate<T> match)
	{
		throw null;
	}

	public int FindLastIndex(int startIndex, int count, Predicate<T> match)
	{
		throw null;
	}

	public int FindLastIndex(int startIndex, Predicate<T> match)
	{
		throw null;
	}

	public int FindLastIndex(Predicate<T> match)
	{
		throw null;
	}

	public void ForEach(Action<T> action)
	{
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	public List<T> GetRange(int index, int count)
	{
		throw null;
	}

	public int IndexOf(T item)
	{
		throw null;
	}

	public int IndexOf(T item, int index)
	{
		throw null;
	}

	public int IndexOf(T item, int index, int count)
	{
		throw null;
	}

	public void Insert(int index, T item)
	{
	}

	public void InsertRange(int index, IEnumerable<T> collection)
	{
	}

	public int LastIndexOf(T item)
	{
		throw null;
	}

	public int LastIndexOf(T item, int index)
	{
		throw null;
	}

	public int LastIndexOf(T item, int index, int count)
	{
		throw null;
	}

	public bool Remove(T item)
	{
		throw null;
	}

	public int RemoveAll(Predicate<T> match)
	{
		throw null;
	}

	public void RemoveAt(int index)
	{
	}

	public void RemoveRange(int index, int count)
	{
	}

	public void Reverse()
	{
	}

	public void Reverse(int index, int count)
	{
	}

	public List<T> Slice(int start, int length)
	{
		throw null;
	}

	public void Sort()
	{
	}

	public void Sort(IComparer<T>? comparer)
	{
	}

	public void Sort(Comparison<T> comparison)
	{
	}

	public void Sort(int index, int count, IComparer<T>? comparer)
	{
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int arrayIndex)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	int IList.Add(object? item)
	{
		throw null;
	}

	bool IList.Contains(object? item)
	{
		throw null;
	}

	int IList.IndexOf(object? item)
	{
		throw null;
	}

	void IList.Insert(int index, object? item)
	{
	}

	void IList.Remove(object? item)
	{
	}

	public T[] ToArray()
	{
		throw null;
	}

	public void TrimExcess()
	{
	}

	public bool TrueForAll(Predicate<T> match)
	{
		throw null;
	}
}
