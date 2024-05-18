namespace System.Collections.Generic;

public interface IReadOnlySet<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
{
	bool Contains(T item);

	bool IsProperSubsetOf(IEnumerable<T> other);

	bool IsProperSupersetOf(IEnumerable<T> other);

	bool IsSubsetOf(IEnumerable<T> other);

	bool IsSupersetOf(IEnumerable<T> other);

	bool Overlaps(IEnumerable<T> other);

	bool SetEquals(IEnumerable<T> other);
}
