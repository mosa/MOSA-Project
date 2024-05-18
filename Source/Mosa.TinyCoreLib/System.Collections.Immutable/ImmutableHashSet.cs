using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Collections.Immutable;

public static class ImmutableHashSet
{
	public static ImmutableHashSet<T>.Builder CreateBuilder<T>()
	{
		throw null;
	}

	public static ImmutableHashSet<T>.Builder CreateBuilder<T>(IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public static ImmutableHashSet<T> CreateRange<T>(IEnumerable<T> items)
	{
		throw null;
	}

	public static ImmutableHashSet<T> CreateRange<T>(IEqualityComparer<T>? equalityComparer, IEnumerable<T> items)
	{
		throw null;
	}

	public static ImmutableHashSet<T> Create<T>()
	{
		throw null;
	}

	public static ImmutableHashSet<T> Create<T>(IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public static ImmutableHashSet<T> Create<T>(IEqualityComparer<T>? equalityComparer, T item)
	{
		throw null;
	}

	public static ImmutableHashSet<T> Create<T>(IEqualityComparer<T>? equalityComparer, params T[] items)
	{
		throw null;
	}

	public static ImmutableHashSet<T> Create<T>(IEqualityComparer<T>? equalityComparer, ReadOnlySpan<T> items)
	{
		throw null;
	}

	public static ImmutableHashSet<T> Create<T>(T item)
	{
		throw null;
	}

	public static ImmutableHashSet<T> Create<T>(params T[] items)
	{
		throw null;
	}

	public static ImmutableHashSet<T> Create<T>(ReadOnlySpan<T> items)
	{
		throw null;
	}

	public static ImmutableHashSet<TSource> ToImmutableHashSet<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static ImmutableHashSet<TSource> ToImmutableHashSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? equalityComparer)
	{
		throw null;
	}

	public static ImmutableHashSet<TSource> ToImmutableHashSet<TSource>(this ImmutableHashSet<TSource>.Builder builder)
	{
		throw null;
	}
}
[CollectionBuilder(typeof(ImmutableHashSet), "Create")]
public sealed class ImmutableHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, IReadOnlySet<T>, ICollection, IImmutableSet<T>
{
	public sealed class Builder : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>
	{
		public int Count
		{
			get
			{
				throw null;
			}
		}

		public IEqualityComparer<T> KeyComparer
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

		public bool Overlaps(IEnumerable<T> other)
		{
			throw null;
		}

		public bool Remove(T item)
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}

		public ImmutableHashSet<T> ToImmutable()
		{
			throw null;
		}

		public void UnionWith(IEnumerable<T> other)
		{
		}
	}

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

	public static readonly ImmutableHashSet<T> Empty;

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

	public IEqualityComparer<T> KeyComparer
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

	internal ImmutableHashSet()
	{
	}

	public ImmutableHashSet<T> Add(T item)
	{
		throw null;
	}

	public ImmutableHashSet<T> Clear()
	{
		throw null;
	}

	public bool Contains(T item)
	{
		throw null;
	}

	public ImmutableHashSet<T> Except(IEnumerable<T> other)
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	public ImmutableHashSet<T> Intersect(IEnumerable<T> other)
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

	public bool Overlaps(IEnumerable<T> other)
	{
		throw null;
	}

	public ImmutableHashSet<T> Remove(T item)
	{
		throw null;
	}

	public bool SetEquals(IEnumerable<T> other)
	{
		throw null;
	}

	public ImmutableHashSet<T> SymmetricExcept(IEnumerable<T> other)
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

	void ICollection.CopyTo(Array array, int arrayIndex)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	IImmutableSet<T> IImmutableSet<T>.Add(T item)
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

	IImmutableSet<T> IImmutableSet<T>.Remove(T item)
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

	public ImmutableHashSet<T> Union(IEnumerable<T> other)
	{
		throw null;
	}

	public ImmutableHashSet<T> WithComparer(IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}
}
