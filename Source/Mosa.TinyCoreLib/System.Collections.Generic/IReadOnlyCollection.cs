namespace System.Collections.Generic;

public interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable
{
	int Count { get; }
}
