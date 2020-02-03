// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
	public sealed partial class IntervalTree<T>
	{
		/// <summary>
		/// Representation of bounded interval
		/// </summary>
		private struct Interval
		{
			public int Start;

			public int End;

			public Interval(int start, int end)
			{
				Start = start;
				End = end;

				//Debug.Assert(End >= Start);
				//Debug.Assert(Start.CompareTo(End) <= 0);
			}

			public int Length { get { return End - Start; } }

			public bool IsSame(Interval interval)
			{
				return Start == interval.Start && End == interval.End;
			}

			public bool Overlaps(Interval interval)
			{
				return (Start <= interval.Start && End > interval.Start) || (interval.Start <= Start && interval.End > Start);
			}

			public bool Overlaps(int at)
			{
				return Contains(at);
			}

			public bool Contains(Interval interval)
			{
				return Start >= interval.Start && interval.End < End;
			}

			public bool Contains(int at)
			{
				return at >= Start && at < End;
			}

			public int CompareTo(Interval interval)
			{
				if (Overlaps(interval))
					return 0;

				return Start.CompareTo(interval.Start);
			}

			public int CompareTo(int at)
			{
				if (Contains(at))
					return 0;

				return Start.CompareTo(at);
			}

			public bool IsAdjacent(int start, int end)
			{
				return start == End || end == Start;
			}

			public bool IsAdjacent(Interval interval)
			{
				return IsAdjacent(interval.Start, interval.End);
			}

			public override string ToString()
			{
				return $"{Start}-{End}";
			}
		}
	}
}
