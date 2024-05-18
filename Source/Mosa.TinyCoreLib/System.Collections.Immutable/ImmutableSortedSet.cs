using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Immutable;

public static class ImmutableSortedSet
{
	public static ImmutableSortedSet<T>.Builder CreateBuilder<T>()
	{
		throw null;
	}

	public static ImmutableSortedSet<T>.Builder CreateBuilder<T>(IComparer<T>? comparer)
	{
		throw null;
	}

	public static ImmutableSortedSet<T> CreateRange<T>(IComparer<T>? comparer, IEnumerable<T> items)
	{
		throw null;
	}

	public static ImmutableSortedSet<T> CreateRange<T>(IEnumerable<T> items)
	{
		throw null;
	}

	public static ImmutableSortedSet<T> Create<T>()
	{
		throw null;
	}

	public static ImmutableSortedSet<T> Create<T>(IComparer<T>? comparer)
	{
		throw null;
	}

	public static ImmutableSortedSet<T> Create<T>(IComparer<T>? comparer, T item)
	{
		throw null;
	}

	public static ImmutableSortedSet<T> Create<T>(IComparer<T>? comparer, params T[] items)
	{
		throw null;
	}

	public static ImmutableSortedSet<T> Create<T>(IComparer<T>? comparer, ReadOnlySpan<T> items)
	{
		throw null;
	}

	public static ImmutableSortedSet<T> Create<T>(T item)
	{
		throw null;
	}

	public static ImmutableSortedSet<T> Create<T>(params T[] items)
	{
		throw null;
	}

	public static ImmutableSortedSet<T> Create<T>(ReadOnlySpan<T> items)
	{
		throw null;
	}

	public static ImmutableSortedSet<TSource> ToImmutableSortedSet<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static ImmutableSortedSet<TSource> ToImmutableSortedSet<TSource>(this IEnumerable<TSource> source, IComparer<TSource>? comparer)
	{
		throw null;
	}

	public static ImmutableSortedSet<TSource> ToImmutableSortedSet<TSource>(this ImmutableSortedSet<TSource>.Builder builder)
	{
		throw null;
	}
}
[CollectionBuilder(typeof(ImmutableSortedSet), "Create")]
public sealed class ImmutableSortedSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ISet<T>, IReadOnlySet<T>, ICollection, IList, IImmutableSet<T>
{
	public sealed class Builder : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, ICollection
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
		}

		public IComparer<T> KeyComparer
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public T? Max
		{
			get
			{
				throw null;
			}
		}

		public T? Min
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

		internal Builder()
		{
		}

		public bool Add(T item)
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

		public void ExceptWith(IEnumerable<T> other)
		{
		}

		public Enumerator GetEnumerator()
		{
			throw null;
		}

		public void IntersectWith(IEnumerable<T> other)
		{
		}

		public int IndexOf(T item)
		{
			throw null;
		}

		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			throw null;
		}

		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			throw null;
		}

		public bool IsSubsetOf(IEnumerable<T> other)
		{
			throw null;
		}

		public bool IsSupersetOf(IEnumerable<T> other)
		{
			throw null;
		}

		public ref readonly T ItemRef(int index)
		{
			throw null;
		}

		public bool Overlaps(IEnumerable<T> other)
		{
			throw null;
		}

		public bool Remove(T item)
		{
			throw null;
		}

		public IEnumerable<T> Reverse()
		{
			throw null;
		}

		public bool SetEquals(IEnumerable<T> other)
		{
			throw null;
		}

		public void SymmetricExceptWith(IEnumerable<T> other)
		{
		}

		public bool TryGetValue(T equalValue, out T actualValue)
		{
			throw null;
		}

		void ICollection<T>.Add(T item)
		{
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
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

		public ImmutableSortedSet<T> ToImmutable()
		{
			throw null;
		}

		public void UnionWith(IEnumerable<T> other)
		{
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

	public static readonly ImmutableSortedSet<T> Empty;

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

	public IComparer<T> KeyComparer
	{
		get
		{
			throw null;
		}
	}

	public T? Max
	{
		get
		{
			throw null;
		}
	}

	public T? Min
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

	internal ImmutableSortedSet()
	{
	}

	public ImmutableSortedSet<T> Add(T value)
	{
		throw null;
	}

	public ImmutableSortedSet<T> Clear()
	{
		throw null;
	}

	public bool Contains(T value)
	{
		throw null;
	}

	public ImmutableSortedSet<T> Except(IEnumerable<T> other)
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(T item)
	{
		throw null;
	}

	public ImmutableSortedSet<T> Intersect(IEnumerable<T> other)
	{
		throw null;
	}

	public bool IsProperSubsetOf(IEnumerable<T> other)
	{
		throw null;
	}

	public bool IsProperSupersetOf(IEnumerable<T> other)
	{
		throw null;
	}

	public bool IsSubsetOf(IEnumerable<T> other)
	{
		throw null;
	}

	public bool IsSupersetOf(IEnumerable<T> other)
	{
		throw null;
	}

	public ref readonly T ItemRef(int index)
	{
		throw null;
	}

	public bool Overlaps(IEnumerable<T> other)
	{
		throw null;
	}

	public ImmutableSortedSet<T> Remove(T value)
	{
		throw null;
	}

	public IEnumerable<T> Reverse()
	{
		throw null;
	}

	public bool SetEquals(IEnumerable<T> other)
	{
		throw null;
	}

	public ImmutableSortedSet<T> SymmetricExcept(IEnumerable<T> other)
	{
		throw null;
	}

	void ICollection<T>.Add(T item)
	{
	}

	void ICollection<T>.Clear()
	{
	}

	void ICollection<T>.CopyTo(T[] array, int arrayIndex)
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

	bool ISet<T>.Add(T item)
	{
		throw null;
	}

	void ISet<T>.ExceptWith(IEnumerable<T> other)
	{
	}

	void ISet<T>.IntersectWith(IEnumerable<T> other)
	{
	}

	void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
	{
	}

	void ISet<T>.UnionWith(IEnumerable<T> other)
	{
	}

	void ICollection.CopyTo(Array array, int index)
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

	IImmutableSet<T> IImmutableSet<T>.Add(T value)
	{
		throw null;
	}

	IImmutableSet<T> IImmutableSet<T>.Clear()
	{
		throw null;
	}

	IImmutableSet<T> IImmutableSet<T>.Except(IEnumerable<T> other)
	{
		throw null;
	}

	IImmutableSet<T> IImmutableSet<T>.Intersect(IEnumerable<T> other)
	{
		throw null;
	}

	IImmutableSet<T> IImmutableSet<T>.Remove(T value)
	{
		throw null;
	}

	IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(IEnumerable<T> other)
	{
		throw null;
	}

	IImmutableSet<T> IImmutableSet<T>.Union(IEnumerable<T> other)
	{
		throw null;
	}

	public Builder ToBuilder()
	{
		throw null;
	}

	public bool TryGetValue(T equalValue, out T actualValue)
	{
		throw null;
	}

	public ImmutableSortedSet<T> Union(IEnumerable<T> other)
	{
		throw null;
	}

	public ImmutableSortedSet<T> WithComparer(IComparer<T>? comparer)
	{
		throw null;
	}
}
