using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Immutable;

public static class ImmutableList
{
	public static ImmutableList<T>.Builder CreateBuilder<T>()
	{
		throw null;
	}

	public static ImmutableList<T> CreateRange<T>(IEnumerable<T> items)
	{
		throw null;
	}

	public static ImmutableList<T> Create<T>()
	{
		throw null;
	}

	public static ImmutableList<T> Create<T>(T item)
	{
		throw null;
	}

	public static ImmutableList<T> Create<T>(params T[] items)
	{
		throw null;
	}

	public static ImmutableList<T> Create<T>(ReadOnlySpan<T> items)
	{
		throw null;
	}

	public static int IndexOf<T>(this IImmutableList<T> list, T item)
	{
		throw null;
	}

	public static int IndexOf<T>(this IImmutableList<T> list, T item, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public static int IndexOf<T>(this IImmutableList<T> list, T item, int startIndex)
	{
		throw null;
	}

	public static int IndexOf<T>(this IImmutableList<T> list, T item, int startIndex, int count)
	{
		throw null;
	}

	public static int LastIndexOf<T>(this IImmutableList<T> list, T item)
	{
		throw null;
	}

	public static int LastIndexOf<T>(this IImmutableList<T> list, T item, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public static int LastIndexOf<T>(this IImmutableList<T> list, T item, int startIndex)
	{
		throw null;
	}

	public static int LastIndexOf<T>(this IImmutableList<T> list, T item, int startIndex, int count)
	{
		throw null;
	}

	public static IImmutableList<T> RemoveRange<T>(this IImmutableList<T> list, IEnumerable<T> items)
	{
		throw null;
	}

	public static IImmutableList<T> Remove<T>(this IImmutableList<T> list, T value)
	{
		throw null;
	}

	public static IImmutableList<T> Replace<T>(this IImmutableList<T> list, T oldValue, T newValue)
	{
		throw null;
	}

	public static ImmutableList<TSource> ToImmutableList<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static ImmutableList<TSource> ToImmutableList<TSource>(this ImmutableList<TSource>.Builder builder)
	{
		throw null;
	}
}
[CollectionBuilder(typeof(ImmutableList), "Create")]
public sealed class ImmutableList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList, IImmutableList<T>
{
	public sealed class Builder : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
	{
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

		internal Builder()
		{
		}

		public void Add(T item)
		{
		}

		public void AddRange(IEnumerable<T> items)
		{
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

		public ImmutableList<TOutput> ConvertAll<TOutput>(Func<T, TOutput> converter)
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

		public bool Exists(Predicate<T> match)
		{
			throw null;
		}

		public T? Find(Predicate<T> match)
		{
			throw null;
		}

		public ImmutableList<T> FindAll(Predicate<T> match)
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

		public ImmutableList<T> GetRange(int index, int count)
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

		public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
		{
			throw null;
		}

		public void Insert(int index, T item)
		{
		}

		public void InsertRange(int index, IEnumerable<T> items)
		{
		}

		public ref readonly T ItemRef(int index)
		{
			throw null;
		}

		public int LastIndexOf(T item)
		{
			throw null;
		}

		public int LastIndexOf(T item, int startIndex)
		{
			throw null;
		}

		public int LastIndexOf(T item, int startIndex, int count)
		{
			throw null;
		}

		public int LastIndexOf(T item, int startIndex, int count, IEqualityComparer<T>? equalityComparer)
		{
			throw null;
		}

		public bool Remove(T item)
		{
			throw null;
		}

		public bool Remove(T item, IEqualityComparer<T>? equalityComparer)
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
			throw null;
		}

		public void RemoveRange(IEnumerable<T> items)
		{
			throw null;
		}

		public void RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
		{
			throw null;
		}

		public void Replace(T oldValue, T newValue)
		{
			throw null;
		}

		public void Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
		{
			throw null;
		}

		public void Reverse()
		{
		}

		public void Reverse(int index, int count)
		{
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

		int IList.Add(object? value)
		{
			throw null;
		}

		void IList.Clear()
		{
		}

		bool IList.Contains(object? value)
		{
			throw null;
		}

		int IList.IndexOf(object? value)
		{
			throw null;
		}

		void IList.Insert(int index, object? value)
		{
		}

		void IList.Remove(object? value)
		{
		}

		public ImmutableList<T> ToImmutable()
		{
			throw null;
		}

		public bool TrueForAll(Predicate<T> match)
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
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

		public void Reset()
		{
		}
	}

	public static readonly ImmutableList<T> Empty;

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

	public T this[int index]
	{
		get
		{
			throw null;
		}
	}

	bool ICollection<T>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	T IList<T>.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
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

	internal ImmutableList()
	{
	}

	public ImmutableList<T> Add(T value)
	{
		throw null;
	}

	public ImmutableList<T> AddRange(IEnumerable<T> items)
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

	public ImmutableList<T> Clear()
	{
		throw null;
	}

	public bool Contains(T value)
	{
		throw null;
	}

	public ImmutableList<TOutput> ConvertAll<TOutput>(Func<T, TOutput> converter)
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

	public bool Exists(Predicate<T> match)
	{
		throw null;
	}

	public T? Find(Predicate<T> match)
	{
		throw null;
	}

	public ImmutableList<T> FindAll(Predicate<T> match)
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

	public ImmutableList<T> GetRange(int index, int count)
	{
		throw null;
	}

	public int IndexOf(T value)
	{
		throw null;
	}

	public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableList<T> Insert(int index, T item)
	{
		throw null;
	}

	public ImmutableList<T> InsertRange(int index, IEnumerable<T> items)
	{
		throw null;
	}

	public ref readonly T ItemRef(int index)
	{
		throw null;
	}

	public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableList<T> Remove(T value)
	{
		throw null;
	}

	public ImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableList<T> RemoveAll(Predicate<T> match)
	{
		throw null;
	}

	public ImmutableList<T> RemoveAt(int index)
	{
		throw null;
	}

	public ImmutableList<T> RemoveRange(IEnumerable<T> items)
	{
		throw null;
	}

	public ImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableList<T> RemoveRange(int index, int count)
	{
		throw null;
	}

	public ImmutableList<T> Replace(T oldValue, T newValue)
	{
		throw null;
	}

	public ImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableList<T> Reverse()
	{
		throw null;
	}

	public ImmutableList<T> Reverse(int index, int count)
	{
		throw null;
	}

	public ImmutableList<T> SetItem(int index, T value)
	{
		throw null;
	}

	public ImmutableList<T> Sort()
	{
		throw null;
	}

	public ImmutableList<T> Sort(IComparer<T>? comparer)
	{
		throw null;
	}

	public ImmutableList<T> Sort(Comparison<T> comparison)
	{
		throw null;
	}

	public ImmutableList<T> Sort(int index, int count, IComparer<T>? comparer)
	{
		throw null;
	}

	void ICollection<T>.Add(T item)
	{
	}

	void ICollection<T>.Clear()
	{
	}

	bool ICollection<T>.Remove(T item)
	{
		throw null;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		throw null;
	}

	void IList<T>.Insert(int index, T item)
	{
	}

	void IList<T>.RemoveAt(int index)
	{
	}

	void ICollection.CopyTo(Array array, int arrayIndex)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	int IList.Add(object? value)
	{
		throw null;
	}

	void IList.Clear()
	{
	}

	bool IList.Contains(object? value)
	{
		throw null;
	}

	int IList.IndexOf(object? value)
	{
		throw null;
	}

	void IList.Insert(int index, object? value)
	{
	}

	void IList.Remove(object? value)
	{
	}

	void IList.RemoveAt(int index)
	{
	}

	IImmutableList<T> IImmutableList<T>.Add(T value)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.Clear()
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.Insert(int index, T item)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.RemoveAt(int index)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	IImmutableList<T> IImmutableList<T>.SetItem(int index, T value)
	{
		throw null;
	}

	public Builder ToBuilder()
	{
		throw null;
	}

	public bool TrueForAll(Predicate<T> match)
	{
		throw null;
	}
}
