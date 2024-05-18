namespace System.Collections.Generic;

public interface IReadOnlyList<out T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
{
	T this[int index] { get; }
}
