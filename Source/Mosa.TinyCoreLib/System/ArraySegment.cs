using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct ArraySegment<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
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

		object? IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			throw null;
		}

		void IEnumerator.Reset()
		{
		}
	}

	private readonly T[] _array;

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public T[]? Array
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

	public static ArraySegment<T> Empty
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
		set
		{
		}
	}

	public int Offset
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

	T IReadOnlyList<T>.this[int index]
	{
		get
		{
			throw null;
		}
	}

	public ArraySegment(T[] array)
	{
		throw null;
	}

	public ArraySegment(T[] array, int offset, int count)
	{
		throw null;
	}

	public void CopyTo(ArraySegment<T> destination)
	{
	}

	public void CopyTo(T[] destination)
	{
	}

	public void CopyTo(T[] destination, int destinationIndex)
	{
	}

	public bool Equals(ArraySegment<T> obj)
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

	public static bool operator ==(ArraySegment<T> a, ArraySegment<T> b)
	{
		throw null;
	}

	public static implicit operator ArraySegment<T>(T[] array)
	{
		throw null;
	}

	public static bool operator !=(ArraySegment<T> a, ArraySegment<T> b)
	{
		throw null;
	}

	public ArraySegment<T> Slice(int index)
	{
		throw null;
	}

	public ArraySegment<T> Slice(int index, int count)
	{
		throw null;
	}

	void ICollection<T>.Add(T item)
	{
	}

	void ICollection<T>.Clear()
	{
	}

	bool ICollection<T>.Contains(T item)
	{
		throw null;
	}

	bool ICollection<T>.Remove(T item)
	{
		throw null;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		throw null;
	}

	int IList<T>.IndexOf(T item)
	{
		throw null;
	}

	void IList<T>.Insert(int index, T item)
	{
	}

	void IList<T>.RemoveAt(int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public T[] ToArray()
	{
		throw null;
	}
}
