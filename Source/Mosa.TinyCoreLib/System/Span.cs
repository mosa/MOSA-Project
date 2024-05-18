using System.ComponentModel;
using System.Runtime.InteropServices.Marshalling;

namespace System;

[NativeMarshalling(typeof(SpanMarshaller<, >))]
public readonly ref struct Span<T>
{
	public ref struct Enumerator
	{
		private object _dummy;

		private int _dummyPrimitive;

		public ref T Current
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

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static Span<T> Empty
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

	public ref T this[int index]
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

	[CLSCompliant(false)]
	public unsafe Span(void* pointer, int length)
	{
		throw null;
	}

	public Span(T[]? array)
	{
		throw null;
	}

	public Span(T[]? array, int start, int length)
	{
		throw null;
	}

	public Span(ref T reference)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public void CopyTo(Span<T> destination)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Equals() on Span will always throw an exception. Use the equality operator instead.")]
	public override bool Equals(object? obj)
	{
		throw null;
	}

	public void Fill(T value)
	{
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("GetHashCode() on Span will always throw an exception.")]
	public override int GetHashCode()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ref T GetPinnableReference()
	{
		throw null;
	}

	public static bool operator ==(Span<T> left, Span<T> right)
	{
		throw null;
	}

	public static implicit operator Span<T>(ArraySegment<T> segment)
	{
		throw null;
	}

	public static implicit operator ReadOnlySpan<T>(Span<T> span)
	{
		throw null;
	}

	public static implicit operator Span<T>(T[]? array)
	{
		throw null;
	}

	public static bool operator !=(Span<T> left, Span<T> right)
	{
		throw null;
	}

	public Span<T> Slice(int start)
	{
		throw null;
	}

	public Span<T> Slice(int start, int length)
	{
		throw null;
	}

	public T[] ToArray()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public bool TryCopyTo(Span<T> destination)
	{
		throw null;
	}
}
