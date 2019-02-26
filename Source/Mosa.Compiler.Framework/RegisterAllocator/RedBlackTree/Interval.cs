// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
    /// <summary>
    /// Representation of bounded interval
    /// </summary>
    public class Interval
    {
        public virtual int Start { get; set; }

        public virtual int End { get; set; }

        public Interval()
        {
        }

        public Interval(int start, int end)
        {
            Start = start;
            End = end;

            if (Start.CompareTo(End) > 0)
            {
                throw new ArgumentException("Start cannot be larger than End of interval");
            }
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

        public bool Overlaps(int val)
        {
            return Contains(val);
        }

        public bool Contains(Interval interval)
        {
            return Start >= interval.Start && interval.End < End;
        }

        public bool Contains(int val)
        {
            return val >= Start && val < End;
        }

        public int CompareTo(Interval interval)
        {
            if (Overlaps(interval))
                return 0;

            return Start.CompareTo(interval.Start);
        }

        public int CompareTo(int val)
        {
            if (Contains(val))
                return 0;

            return Start.CompareTo(val);
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
