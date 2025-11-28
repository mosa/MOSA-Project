using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Immutable;

public static class ImmutableQueue
{
	public static ImmutableQueue<T> CreateRange<T>(IEnumerable<T> items)
	{
		throw null;
	}

	public static ImmutableQueue<T> Create<T>()
	{
		throw null;
	}

	public static ImmutableQueue<T> Create<T>(T item)
	{
		throw null;
	}

	public static ImmutableQueue<T> Create<T>(params T[] items)
	{
		throw null;
	}

	public static ImmutableQueue<T> Create<T>(ReadOnlySpan<T> items)
	{
		throw null;
	}

	public static IImmutableQueue<T> Dequeue<T>(this IImmutableQueue<T> queue, out T value)
	{
		throw null;
	}
}
[CollectionBuilder(typeof(ImmutableQueue), "Create")]
public sealed class ImmutableQueue<T> : IEnumerable<T>, IEnumerable, IImmutableQueue<T>
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

	public static ImmutableQueue<T> Empty
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

	internal ImmutableQueue()
	{
	}

	public ImmutableQueue<T> Clear()
	{
		throw null;
	}

	public ImmutableQueue<T> Dequeue()
	{
		throw null;
	}

	public ImmutableQueue<T> Dequeue(out T value)
	{
		throw null;
	}

	public ImmutableQueue<T> Enqueue(T value)
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

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	IImmutableQueue<T> IImmutableQueue<T>.Clear()
	{
		throw null;
	}

	IImmutableQueue<T> IImmutableQueue<T>.Dequeue()
	{
		throw null;
	}

	IImmutableQueue<T> IImmutableQueue<T>.Enqueue(T value)
	{
		throw null;
	}
}
