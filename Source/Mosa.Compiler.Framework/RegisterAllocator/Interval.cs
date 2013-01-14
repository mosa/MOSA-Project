/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{

	public class Interval
	{
		public SlotIndex Start { get; private set; }
		public SlotIndex End { get; private set; }

		public Interval(SlotIndex start, SlotIndex end)
		{
			Debug.Assert(start <= end);

			this.Start = start;
			this.End = end;
		}

		public Interval CreateExpandedInterval(Interval interval)
		{
			var start = Start < interval.Start ? Start : interval.Start;
			var end = End > interval.End ? End : interval.End;

			return new Interval(start, end);
		}

		public int Length { get { return End - Start; } }

		public bool Intersects(SlotIndex start, SlotIndex end)
		{
			return ((Start <= start && End > start) || (start <= Start && end > Start));
		}

		public bool Intersects(Interval other)
		{
			return Intersects(other.Start, other.End);
		}

		public bool IsAdjacent(SlotIndex start, SlotIndex end)
		{
			return (start == End) || (end == Start);
		}

		public bool IsAdjacent(Interval other)
		{
			return IsAdjacent(other.Start, other.End);
		}

		public bool Contains(SlotIndex slot)
		{
			return (Start >= slot && slot < End);
		}

	}
}