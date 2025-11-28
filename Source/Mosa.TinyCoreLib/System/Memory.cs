using System.Buffers;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct Memory<T> : IEquatable<Memory<T>>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static Memory<T> Empty
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

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public Span<T> Span
	{
		get
		{
			throw null;
		}
	}

	public Memory(T[]? array)
	{
		throw null;
	}

	public Memory(T[]? array, int start, int length)
	{
		throw null;
	}

	public void CopyTo(Memory<T> destination)
	{
	}

	public bool Equals(Memory<T> other)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		throw null;
	}

	public static implicit operator Memory<T>(ArraySegment<T> segment)
	{
		throw null;
	}

	public static implicit operator ReadOnlyMemory<T>(Memory<T> memory)
	{
		throw null;
	}

	public static implicit operator Memory<T>(T[]? array)
	{
		throw null;
	}

	public MemoryHandle Pin()
	{
		throw null;
	}

	public Memory<T> Slice(int start)
	{
		throw null;
	}

	public Memory<T> Slice(int start, int length)
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

	public bool TryCopyTo(Memory<T> destination)
	{
		throw null;
	}
}
