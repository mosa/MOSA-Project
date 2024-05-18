using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace System;

[NativeMarshalling(typeof(ReadOnlySpanMarshaller<, >))]
public readonly ref struct ReadOnlySpan<T>
{
	public ref struct Enumerator
	{
		private object _dummy;

		private int _dummyPrimitive;

		public ref readonly T Current
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

	public static ReadOnlySpan<T> Empty
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

	public ref readonly T this[int index]
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
	public unsafe ReadOnlySpan(void* pointer, int length)
	{
		throw null;
	}

	public ReadOnlySpan(T[]? array)
	{
		throw null;
	}

	public ReadOnlySpan(T[]? array, int start, int length)
	{
		throw null;
	}

	public ReadOnlySpan([In] ref T reference)
	{
		throw null;
	}

	public void CopyTo(Span<T> destination)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Equals() on ReadOnlySpan will always throw an exception. Use the equality operator instead.")]
	public override bool Equals(object? obj)
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("GetHashCode() on ReadOnlySpan will always throw an exception.")]
	public override int GetHashCode()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ref readonly T GetPinnableReference()
	{
		throw null;
	}

	public static bool operator ==(ReadOnlySpan<T> left, ReadOnlySpan<T> right)
	{
		throw null;
	}

	public static implicit operator ReadOnlySpan<T>(ArraySegment<T> segment)
	{
		throw null;
	}

	public static implicit operator ReadOnlySpan<T>(T[]? array)
	{
		throw null;
	}

	public static bool operator !=(ReadOnlySpan<T> left, ReadOnlySpan<T> right)
	{
		throw null;
	}

	public ReadOnlySpan<T> Slice(int start)
	{
		throw null;
	}

	public ReadOnlySpan<T> Slice(int start, int length)
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
