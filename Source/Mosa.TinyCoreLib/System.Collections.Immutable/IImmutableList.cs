using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Collections.Immutable;

[CollectionBuilder(typeof(ImmutableList), "Create")]
public interface IImmutableList<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, IReadOnlyList<T>
{
	IImmutableList<T> Add(T value);

	IImmutableList<T> AddRange(IEnumerable<T> items);

	IImmutableList<T> Clear();

	int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer);

	IImmutableList<T> Insert(int index, T element);

	IImmutableList<T> InsertRange(int index, IEnumerable<T> items);

	int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer);

	IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer);

	IImmutableList<T> RemoveAll(Predicate<T> match);

	IImmutableList<T> RemoveAt(int index);

	IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer);

	IImmutableList<T> RemoveRange(int index, int count);

	IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer);

	IImmutableList<T> SetItem(int index, T value);
}
