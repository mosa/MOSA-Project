using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct Range(Index start, Index end) : IEquatable<Range>
{
	public static Range All { get; } = new(Index.Start, Index.End);

	public Index End { get; } = end;

	public Index Start { get; } = start;

	public static Range EndAt(Index end) => new(Index.Start, end);

	public override bool Equals([NotNullWhen(true)] object? value) => value is Range range && Equals(range);

	public bool Equals(Range other) => Start.Equals(other.Start) && End.Equals(other.End);

	public override int GetHashCode() => HashCode.Combine(Start, End);

	public (int Offset, int Length) GetOffsetAndLength(int length)
	{
		var startOffset = Start.GetOffset(length);
		var endOffset = End.GetOffset(length);

		if (startOffset >= length || endOffset > length)
			Internal.Exceptions.Generic.ParameterOutOfRange(nameof(length));

		return (startOffset, endOffset - startOffset);
	}

	public static Range StartAt(Index start) => new(start, Index.End);

	public override string ToString() => $"{Start}..{End}";
}
