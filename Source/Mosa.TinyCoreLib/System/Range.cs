using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct Range : IEquatable<Range>
{
	private readonly int _dummyPrimitive;

	public static Range All
	{
		get
		{
			throw null;
		}
	}

	public Index End
	{
		get
		{
			throw null;
		}
	}

	public Index Start
	{
		get
		{
			throw null;
		}
	}

	public Range(Index start, Index end)
	{
		throw null;
	}

	public static Range EndAt(Index end)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public bool Equals(Range other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public (int Offset, int Length) GetOffsetAndLength(int length)
	{
		throw null;
	}

	public static Range StartAt(Index start)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
