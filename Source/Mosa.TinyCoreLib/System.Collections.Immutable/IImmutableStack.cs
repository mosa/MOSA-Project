using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Collections.Immutable;

[CollectionBuilder(typeof(ImmutableStack), "Create")]
public interface IImmutableStack<T> : IEnumerable<T>, IEnumerable
{
	bool IsEmpty { get; }

	IImmutableStack<T> Clear();

	T Peek();

	IImmutableStack<T> Pop();

	IImmutableStack<T> Push(T value);
}
