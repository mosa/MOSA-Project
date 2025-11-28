namespace System.Buffers;

public ref struct SequenceReader<T> where T : unmanaged, IEquatable<T>
{
	private object _dummy;

	private int _dummyPrimitive;

	public readonly long Consumed
	{
		get
		{
			throw null;
		}
	}

	public readonly ReadOnlySpan<T> CurrentSpan
	{
		get
		{
			throw null;
		}
	}

	public readonly int CurrentSpanIndex
	{
		get
		{
			throw null;
		}
	}

	public readonly bool End
	{
		get
		{
			throw null;
		}
	}

	public readonly long Length
	{
		get
		{
			throw null;
		}
	}

	public readonly SequencePosition Position
	{
		get
		{
			throw null;
		}
	}

	public readonly long Remaining
	{
		get
		{
			throw null;
		}
	}

	public readonly ReadOnlySequence<T> Sequence
	{
		get
		{
			throw null;
		}
	}

	public readonly ReadOnlySequence<T> UnreadSequence
	{
		get
		{
			throw null;
		}
	}

	public readonly ReadOnlySpan<T> UnreadSpan
	{
		get
		{
			throw null;
		}
	}

	public SequenceReader(ReadOnlySequence<T> sequence)
	{
		throw null;
	}

	public void Advance(long count)
	{
	}

	public long AdvancePast(T value)
	{
		throw null;
	}

	public long AdvancePastAny(scoped ReadOnlySpan<T> values)
	{
		throw null;
	}

	public long AdvancePastAny(T value0, T value1)
	{
		throw null;
	}

	public long AdvancePastAny(T value0, T value1, T value2)
	{
		throw null;
	}

	public long AdvancePastAny(T value0, T value1, T value2, T value3)
	{
		throw null;
	}

	public void AdvanceToEnd()
	{
		throw null;
	}

	public bool IsNext(scoped ReadOnlySpan<T> next, bool advancePast = false)
	{
		throw null;
	}

	public bool IsNext(T next, bool advancePast = false)
	{
		throw null;
	}

	public void Rewind(long count)
	{
	}

	public bool TryAdvanceTo(T delimiter, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public bool TryAdvanceToAny(scoped ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public readonly bool TryCopyTo(Span<T> destination)
	{
		throw null;
	}

	public readonly bool TryPeek(out T value)
	{
		throw null;
	}

	public readonly bool TryPeek(long offset, out T value)
	{
		throw null;
	}

	public bool TryRead(out T value)
	{
		throw null;
	}

	public bool TryReadTo(out ReadOnlySequence<T> sequence, scoped ReadOnlySpan<T> delimiter, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public bool TryReadTo(out ReadOnlySequence<T> sequence, T delimiter, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public bool TryReadTo(out ReadOnlySequence<T> sequence, T delimiter, T delimiterEscape, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public bool TryReadTo(out ReadOnlySpan<T> span, scoped ReadOnlySpan<T> delimiter, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public bool TryReadTo(out ReadOnlySpan<T> span, T delimiter, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public bool TryReadTo(out ReadOnlySpan<T> span, T delimiter, T delimiterEscape, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public bool TryReadToAny(out ReadOnlySequence<T> sequence, scoped ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public bool TryReadToAny(out ReadOnlySpan<T> span, scoped ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
	{
		throw null;
	}

	public bool TryReadExact(int count, out ReadOnlySequence<T> sequence)
	{
		throw null;
	}
}
