using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Concurrent;

public interface IProducerConsumerCollection<T> : IEnumerable<T>, IEnumerable, ICollection
{
	void CopyTo(T[] array, int index);

	T[] ToArray();

	bool TryAdd(T item);

	bool TryTake([MaybeNullWhen(false)] out T item);
}
