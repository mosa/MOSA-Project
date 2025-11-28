using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Frozen;

public abstract class FrozenSet<T> : IReadOnlySet<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection<T>, ISet<T>, ICollection
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private readonly T[] _entries;

		private object _dummy;

		private int _dummyPrimitive;

		public readonly T Current
		{
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		public bool MoveNext()
		{
			throw null;
		}

		void IEnumerator.Reset()
		{
		}

		void IDisposable.Dispose()
		{
		}
	}

	public IEqualityComparer<T> Comparer
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

	public static FrozenSet<T> Empty
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<T> Items
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

	internal FrozenSet()
	{
	}

	public bool Contains(T item)
	{
		throw null;
	}

	public void CopyTo(Span<T> destination)
	{
	}

	public void CopyTo(T[] destination, int destinationIndex)
	{
	}

	public Enumerator GetEnumerator()
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

	public bool SetEquals(IEnumerable<T> other)
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

	public bool TryGetValue(T equalValue, [MaybeNullWhen(false)] out T actualValue)
	{
		throw null;
	}
}
public static class FrozenSet
{
	public static FrozenSet<T> ToFrozenSet<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer = null)
	{
		throw null;
	}
}
