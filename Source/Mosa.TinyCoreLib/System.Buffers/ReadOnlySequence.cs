namespace System.Buffers;

public readonly struct ReadOnlySequence<T>
{
	public struct Enumerator
	{
		private object _dummy;

		private int _dummyPrimitive;

		public ReadOnlyMemory<T> Current
		{
			get
			{
				throw null;
			}
		}

		public Enumerator(in ReadOnlySequence<T> sequence)
		{
			throw null;
		}

		public bool MoveNext()
		{
			throw null;
		}
	}

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static readonly ReadOnlySequence<T> Empty;

	public SequencePosition End
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<T> First
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlySpan<T> FirstSpan
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

	public bool IsSingleSegment
	{
		get
		{
			throw null;
		}
	}

	public long Length
	{
		get
		{
			throw null;
		}
	}

	public SequencePosition Start
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlySequence(ReadOnlySequenceSegment<T> startSegment, int startIndex, ReadOnlySequenceSegment<T> endSegment, int endIndex)
	{
		throw null;
	}

	public ReadOnlySequence(ReadOnlyMemory<T> memory)
	{
		throw null;
	}

	public ReadOnlySequence(T[] array)
	{
		throw null;
	}

	public ReadOnlySequence(T[] array, int start, int length)
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	public long GetOffset(SequencePosition position)
	{
		throw null;
	}

	public SequencePosition GetPosition(long offset)
	{
		throw null;
	}

	public SequencePosition GetPosition(long offset, SequencePosition origin)
	{
		throw null;
	}

	public ReadOnlySequence<T> Slice(int start, int length)
	{
		throw null;
	}

	public ReadOnlySequence<T> Slice(int start, SequencePosition end)
	{
		throw null;
	}

	public ReadOnlySequence<T> Slice(long start)
	{
		throw null;
	}

	public ReadOnlySequence<T> Slice(long start, long length)
	{
		throw null;
	}

	public ReadOnlySequence<T> Slice(long start, SequencePosition end)
	{
		throw null;
	}

	public ReadOnlySequence<T> Slice(SequencePosition start)
	{
		throw null;
	}

	public ReadOnlySequence<T> Slice(SequencePosition start, int length)
	{
		throw null;
	}

	public ReadOnlySequence<T> Slice(SequencePosition start, long length)
	{
		throw null;
	}

	public ReadOnlySequence<T> Slice(SequencePosition start, SequencePosition end)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public bool TryGet(ref SequencePosition position, out ReadOnlyMemory<T> memory, bool advance = true)
	{
		throw null;
	}
}
