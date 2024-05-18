using System.Buffers;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct ReadOnlyMemory<T> : IEquatable<ReadOnlyMemory<T>>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static ReadOnlyMemory<T> Empty
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

	public ReadOnlySpan<T> Span
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory(T[]? array)
	{
		throw null;
	}

	public ReadOnlyMemory(T[]? array, int start, int length)
	{
		throw null;
	}

	public void CopyTo(Memory<T> destination)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ReadOnlyMemory<T> other)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		throw null;
	}

	public static implicit operator ReadOnlyMemory<T>(ArraySegment<T> segment)
	{
		throw null;
	}

	public static implicit operator ReadOnlyMemory<T>(T[]? array)
	{
		throw null;
	}

	public MemoryHandle Pin()
	{
		throw null;
	}

	public ReadOnlyMemory<T> Slice(int start)
	{
		throw null;
	}

	public ReadOnlyMemory<T> Slice(int start, int length)
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
