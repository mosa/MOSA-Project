// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree;

public sealed partial class IntervalTree<T>
{
	/// <summary>
	/// Representation of bounded interval
	/// </summary>
	private struct Interval
	{
		public readonly int Start;

		public readonly int End;

		public Interval(int start, int end)
		{
			Start = start;
			End = end;
		}

		public readonly int Length => End - Start;

		public readonly bool IsSame(Interval interval)
		{
			return Start == interval.Start && End == interval.End;
		}

		public readonly bool Overlaps(Interval interval)
		{
			return (Start <= interval.Start && End >= interval.Start) || (interval.Start <= Start && interval.End >= Start);
		}

		public readonly bool Overlaps(int at)
		{
			return Contains(at);
		}

		public readonly bool Contains(Interval interval)
		{
			return Start >= interval.Start && interval.End <= End;
		}

		public readonly bool Contains(int at)
		{
			return at >= Start && at <= End;
		}

		public readonly int CompareTo(Interval interval)
		{
			if (Overlaps(interval))
				return 0;

			return Start.CompareTo(interval.Start);
		}

		public readonly int CompareTo(int at)
		{
			if (Contains(at))
				return 0;

			return Start.CompareTo(at);
		}

		public override readonly string ToString()
		{
			return $"{Start}-{End}";
		}
	}
}
