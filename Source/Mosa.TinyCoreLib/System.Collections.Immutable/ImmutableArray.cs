using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Collections.Immutable;

[CollectionBuilder(typeof(ImmutableArray), "Create")]
public readonly struct ImmutableArray<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList, IImmutableList<T>, IStructuralComparable, IStructuralEquatable, IEquatable<ImmutableArray<T>>
{
	public sealed class Builder : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>
	{
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
			set
			{
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

		internal Builder()
		{
		}

		public void Add(T item)
		{
		}

		public void AddRange(IEnumerable<T> items)
		{
		}

		public void AddRange(ImmutableArray<T> items)
		{
		}

		public void AddRange(ImmutableArray<T> items, int length)
		{
		}

		public void AddRange(Builder items)
		{
		}

		public void AddRange(params T[] items)
		{
		}

		public void AddRange(T[] items, int length)
		{
		}

		public void AddRange<TDerived>(ImmutableArray<TDerived> items) where TDerived : T
		{
		}

		public void AddRange<TDerived>(ImmutableArray<TDerived>.Builder items) where TDerived : T
		{
		}

		public void AddRange<TDerived>(TDerived[] items) where TDerived : T
		{
		}

		public void AddRange(ReadOnlySpan<T> items)
		{
		}

		public void AddRange<TDerived>(ReadOnlySpan<TDerived> items) where TDerived : T
		{
		}

		public void Clear()
		{
		}

		public bool Contains(T item)
		{
			throw null;
		}

		public void CopyTo(T[] array, int index)
		{
		}

		public void CopyTo(T[] destination)
		{
			throw null;
		}

		public void CopyTo(int sourceIndex, T[] destination, int destinationIndex, int length)
		{
			throw null;
		}

		public void CopyTo(Span<T> destination)
		{
		}

		public ImmutableArray<T> DrainToImmutable()
		{
			throw null;
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw null;
		}

		public int IndexOf(T item)
		{
			throw null;
		}

		public int IndexOf(T item, int startIndex)
		{
			throw null;
		}

		public int IndexOf(T item, int startIndex, int count)
		{
			throw null;
		}

		public int IndexOf(T item, int startIndex, IEqualityComparer<T>? equalityComparer)
		{
			throw null;
		}

		public int IndexOf(T item, int startIndex, int count, IEqualityComparer<T>? equalityComparer)
		{
			throw null;
		}

		public void Insert(int index, T item)
		{
		}

		public void InsertRange(int index, IEnumerable<T> items)
		{
			throw null;
		}

		public void InsertRange(int index, ImmutableArray<T> items)
		{
			throw null;
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

		public ImmutableArray<T> MoveToImmutable()
		{
			throw null;
		}

		public bool Remove(T element)
		{
			throw null;
		}

		public bool Remove(T element, IEqualityComparer<T>? equalityComparer)
		{
			throw null;
		}

		public void RemoveAll(Predicate<T> match)
		{
			throw null;
		}

		public void RemoveAt(int index)
		{
		}

		public void RemoveRange(int index, int length)
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}

		public T[] ToArray()
		{
			throw null;
		}

		public ImmutableArray<T> ToImmutable()
		{
			throw null;
		}
	}

	public struct Enumerator
	{
		private readonly T[] _array;

		private object _dummy;

		private int _dummyPrimitive;

		public T Current
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
	}

	private readonly T[] array;

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static readonly ImmutableArray<T> Empty;

	public bool IsDefault
	{
		get
		{
			throw null;
		}
	}

	public bool IsDefaultOrEmpty
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

	public int Length
	{
		get
		{
			throw null;
		}
	}

	int ICollection<T>.Count
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

	int IReadOnlyCollection<T>.Count
	{
		get
		{
			throw null;
		}
	}

	T IReadOnlyList<T>.this[int index]
	{
		get
		{
			throw null;
		}
	}

	int ICollection.Count
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

	public ReadOnlySpan<T> AsSpan(Range range)
	{
		throw null;
	}

	public ImmutableArray<T> Add(T item)
	{
		throw null;
	}

	public ImmutableArray<T> AddRange(IEnumerable<T> items)
	{
		throw null;
	}

	public ImmutableArray<T> AddRange(ImmutableArray<T> items)
	{
		throw null;
	}

	public ImmutableArray<T> AddRange(T[] items, int length)
	{
		throw null;
	}

	public ImmutableArray<T> AddRange<TDerived>(TDerived[] items) where TDerived : T
	{
		throw null;
	}

	public ImmutableArray<T> AddRange(ImmutableArray<T> items, int length)
	{
		throw null;
	}

	public ImmutableArray<T> AddRange<TDerived>(ImmutableArray<TDerived> items) where TDerived : T
	{
		throw null;
	}

	public ImmutableArray<T> AddRange(ReadOnlySpan<T> items)
	{
		throw null;
	}

	public ImmutableArray<T> AddRange(params T[] items)
	{
		throw null;
	}

	public ReadOnlyMemory<T> AsMemory()
	{
		throw null;
	}

	public ReadOnlySpan<T> AsSpan()
	{
		throw null;
	}

	public ReadOnlySpan<T> AsSpan(int start, int length)
	{
		throw null;
	}

	public ImmutableArray<TOther> As<TOther>() where TOther : class?
	{
		throw null;
	}

	public ImmutableArray<TOther> CastArray<TOther>() where TOther : class?
	{
		throw null;
	}

	public static ImmutableArray<T> CastUp<TDerived>(ImmutableArray<TDerived> items) where TDerived : class?, T
	{
		throw null;
	}

	public ImmutableArray<T> Clear()
	{
		throw null;
	}

	public bool Contains(T item)
	{
		throw null;
	}

	public bool Contains(T item, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public void CopyTo(int sourceIndex, T[] destination, int destinationIndex, int length)
	{
	}

	public void CopyTo(T[] destination)
	{
	}

	public void CopyTo(T[] destination, int destinationIndex)
	{
	}

	public void CopyTo(Span<T> destination)
	{
	}

	public bool Equals(ImmutableArray<T> other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public int IndexOf(T item)
	{
		throw null;
	}

	public int IndexOf(T item, int startIndex)
	{
		throw null;
	}

	public int IndexOf(T item, int startIndex, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public int IndexOf(T item, int startIndex, int count)
	{
		throw null;
	}

	public int IndexOf(T item, int startIndex, int count, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableArray<T> Insert(int index, T item)
	{
		throw null;
	}

	public ImmutableArray<T> InsertRange(int index, IEnumerable<T> items)
	{
		throw null;
	}

	public ImmutableArray<T> InsertRange(int index, ImmutableArray<T> items)
	{
		throw null;
	}

	public ImmutableArray<T> InsertRange(int index, T[] items)
	{
		throw null;
	}

	public ImmutableArray<T> InsertRange(int index, ReadOnlySpan<T> items)
	{
		throw null;
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

	public IEnumerable<TResult> OfType<TResult>()
	{
		throw null;
	}

	public static bool operator ==(ImmutableArray<T> left, ImmutableArray<T> right)
	{
		throw null;
	}

	public static bool operator ==(ImmutableArray<T>? left, ImmutableArray<T>? right)
	{
		throw null;
	}

	public static bool operator !=(ImmutableArray<T> left, ImmutableArray<T> right)
	{
		throw null;
	}

	public static bool operator !=(ImmutableArray<T>? left, ImmutableArray<T>? right)
	{
		throw null;
	}

	public ImmutableArray<T> Remove(T item)
	{
		throw null;
	}

	public ImmutableArray<T> Remove(T item, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableArray<T> RemoveAll(Predicate<T> match)
	{
		throw null;
	}

	public ImmutableArray<T> RemoveAt(int index)
	{
		throw null;
	}

	public ImmutableArray<T> RemoveRange(IEnumerable<T> items)
	{
		throw null;
	}

	public ImmutableArray<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableArray<T> RemoveRange(ImmutableArray<T> items)
	{
		throw null;
	}

	public ImmutableArray<T> RemoveRange(ImmutableArray<T> items, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableArray<T> RemoveRange(int index, int length)
	{
		throw null;
	}

	public ImmutableArray<T> RemoveRange(ReadOnlySpan<T> items, IEqualityComparer<T>? equalityComparer = null)
	{
		throw null;
	}

	public ImmutableArray<T> RemoveRange(T[] items, IEqualityComparer<T>? equalityComparer = null)
	{
		throw null;
	}

	public ImmutableArray<T> Replace(T oldValue, T newValue)
	{
		throw null;
	}

	public ImmutableArray<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
	{
		throw null;
	}

	public ImmutableArray<T> SetItem(int index, T item)
	{
		throw null;
	}

	public ImmutableArray<T> Slice(int start, int length)
	{
		throw null;
	}

	public ImmutableArray<T> Sort()
	{
		throw null;
	}

	public ImmutableArray<T> Sort(IComparer<T>? comparer)
	{
		throw null;
	}

	public ImmutableArray<T> Sort(Comparison<T> comparison)
	{
		throw null;
	}

	public ImmutableArray<T> Sort(int index, int count, IComparer<T>? comparer)
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

	IImmutableList<T> IImmutableList<T>.Insert(int index, T element)
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

	int IStructuralComparable.CompareTo(object? other, IComparer comparer)
	{
		throw null;
	}

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	public Builder ToBuilder()
	{
		throw null;
	}
}
public static class ImmutableArray
{
	public static int BinarySearch<T>(this ImmutableArray<T> array, int index, int length, T value)
	{
		throw null;
	}

	public static int BinarySearch<T>(this ImmutableArray<T> array, int index, int length, T value, IComparer<T>? comparer)
	{
		throw null;
	}

	public static int BinarySearch<T>(this ImmutableArray<T> array, T value)
	{
		throw null;
	}

	public static int BinarySearch<T>(this ImmutableArray<T> array, T value, IComparer<T>? comparer)
	{
		throw null;
	}

	public static ImmutableArray<T>.Builder CreateBuilder<T>()
	{
		throw null;
	}

	public static ImmutableArray<T>.Builder CreateBuilder<T>(int initialCapacity)
	{
		throw null;
	}

	public static ImmutableArray<T> CreateRange<T>(IEnumerable<T> items)
	{
		throw null;
	}

	public static ImmutableArray<TResult> CreateRange<TSource, TResult>(ImmutableArray<TSource> items, Func<TSource, TResult> selector)
	{
		throw null;
	}

	public static ImmutableArray<TResult> CreateRange<TSource, TResult>(ImmutableArray<TSource> items, int start, int length, Func<TSource, TResult> selector)
	{
		throw null;
	}

	public static ImmutableArray<TResult> CreateRange<TSource, TArg, TResult>(ImmutableArray<TSource> items, Func<TSource, TArg, TResult> selector, TArg arg)
	{
		throw null;
	}

	public static ImmutableArray<TResult> CreateRange<TSource, TArg, TResult>(ImmutableArray<TSource> items, int start, int length, Func<TSource, TArg, TResult> selector, TArg arg)
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>()
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>(ImmutableArray<T> items, int start, int length)
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>(T item)
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>(T item1, T item2)
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>(T item1, T item2, T item3)
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>(T item1, T item2, T item3, T item4)
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>(params T[]? items)
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>(T[] items, int start, int length)
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>(ReadOnlySpan<T> items)
	{
		throw null;
	}

	public static ImmutableArray<T> Create<T>(Span<T> items)
	{
		throw null;
	}

	public static ImmutableArray<TSource> ToImmutableArray<TSource>(this IEnumerable<TSource> items)
	{
		throw null;
	}

	public static ImmutableArray<TSource> ToImmutableArray<TSource>(this ImmutableArray<TSource>.Builder builder)
	{
		throw null;
	}

	public static ImmutableArray<T> ToImmutableArray<T>(this ReadOnlySpan<T> items)
	{
		throw null;
	}

	public static ImmutableArray<T> ToImmutableArray<T>(this Span<T> items)
	{
		throw null;
	}
}
