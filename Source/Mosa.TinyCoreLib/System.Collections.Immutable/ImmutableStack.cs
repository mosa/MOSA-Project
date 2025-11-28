using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Immutable;

public static class ImmutableStack
{
	public static ImmutableStack<T> CreateRange<T>(IEnumerable<T> items)
	{
		throw null;
	}

	public static ImmutableStack<T> Create<T>()
	{
		throw null;
	}

	public static ImmutableStack<T> Create<T>(T item)
	{
		throw null;
	}

	public static ImmutableStack<T> Create<T>(params T[] items)
	{
		throw null;
	}

	public static ImmutableStack<T> Create<T>(ReadOnlySpan<T> items)
	{
		throw null;
	}

	public static IImmutableStack<T> Pop<T>(this IImmutableStack<T> stack, out T value)
	{
		throw null;
	}
}
[CollectionBuilder(typeof(ImmutableStack), "Create")]
public sealed class ImmutableStack<T> : IEnumerable<T>, IEnumerable, IImmutableStack<T>
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public struct Enumerator
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

		public bool MoveNext()
		{
			throw null;
		}
	}

	public static ImmutableStack<T> Empty
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

	internal ImmutableStack()
	{
	}

	public ImmutableStack<T> Clear()
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	public T Peek()
	{
		throw null;
	}

	public ref readonly T PeekRef()
	{
		throw null;
	}

	public ImmutableStack<T> Pop()
	{
		throw null;
	}

	public ImmutableStack<T> Pop(out T value)
	{
		throw null;
	}

	public ImmutableStack<T> Push(T value)
	{
		throw null;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	IImmutableStack<T> IImmutableStack<T>.Clear()
	{
		throw null;
	}

	IImmutableStack<T> IImmutableStack<T>.Pop()
	{
		throw null;
	}

	IImmutableStack<T> IImmutableStack<T>.Push(T value)
	{
		throw null;
	}
}
