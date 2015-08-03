// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			return (start == End || end == Start);
		}

		public bool IsAdjacent(Interval other)
		{
			return IsAdjacent(other.Start, other.End);
		}

		public bool Contains(SlotIndex slotIndex)
		{
			return (slotIndex >= Start && slotIndex < End);
		}

		public override string ToString()
		{
			return "[" + Start.ToString() + ", " + End.ToString() + "]";
		}
	}
}