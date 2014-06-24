/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class LiveRange : Interval
	{
		private SortedList<SlotIndex, SlotIndex> usePositions = new SortedList<SlotIndex, SlotIndex>();

		private SortedList<SlotIndex, SlotIndex> defPositions = new SortedList<SlotIndex, SlotIndex>();

		public IList<SlotIndex> UsePositions { get { return usePositions.Keys; } }

		public IList<SlotIndex> DefPositions { get { return defPositions.Keys; } }

		public bool IsEmpty { get { return usePositions.Count == 0 && defPositions.Count == 0; } }

		public SlotIndex Minimum { get; private set; }

		public SlotIndex Maximum { get; private set; }

		public LiveRange(SlotIndex start, SlotIndex end, IList<SlotIndex> uses, IList<SlotIndex> defs)
			: base(start, end)
		{
			// live intervals can not start/end at the same location
			Debug.Assert(start != end);

			SlotIndex max = null;
			SlotIndex min = null;

			foreach (var use in uses)
			{
				if (use != Start && (End == use || Contains(use)))
				{
					usePositions.Add(use, use);

					if (max == null || use > max)
						max = use;
					if (min == null || use < min)
						min = use;
				}
			}

			foreach (var def in defs)
			{
				if (Contains(def))
				{
					defPositions.Add(def, def);

					if (max == null || def > max)
						max = def;
					if (min == null || def < min)
						min = def;
				}
			}

			this.Minimum = min;
			this.Maximum = max;
		}

		private static SlotIndex GetNext(IList<SlotIndex> slots, SlotIndex start)
		{
			int cnt = slots.Count;

			for (int i = 0; i < cnt; i++)
			{
				var slot = slots[i];

				if (slot > start)
				{
					return slot;
				}
			}

			return null;
		}

		private SlotIndex GetPrevious(IList<SlotIndex> slots, SlotIndex start)
		{
			for (int i = slots.Count - 1; i >= 0; i--)
			{
				var slot = slots[i];

				if (slot < start)
				{
					return slot;
				}
			}

			return null;
		}

		public SlotIndex GetNextUsePosition(SlotIndex at)
		{
			return GetNext(UsePositions, at);
		}

		public SlotIndex GetNextDefPosition(SlotIndex at)
		{
			return GetNext(DefPositions, at);
		}

		public SlotIndex GetPreviousUsePosition(SlotIndex at)
		{
			return GetPrevious(UsePositions, at);
		}

		public SlotIndex GetPreviousDefPosition(SlotIndex at)
		{
			return GetPrevious(DefPositions, at);
		}

		public bool CanSplitAt(SlotIndex at)
		{
			if (Start == at)
				return false;

			if (at < Start && at > End)
				return false;

			if (defPositions.ContainsKey(at))
				return false;

			return true;
		}

		public List<LiveRange> SplitAt(SlotIndex at)
		{
			Debug.Assert(CanSplitAt(at));

			var ranges = new List<LiveRange>(2);

			ranges.Add(new LiveRange(Start, at, UsePositions, DefPositions));
			ranges.Add(new LiveRange(at, End, UsePositions, DefPositions));

			return ranges;
		}

		public bool CanSplitAt(SlotIndex low, SlotIndex high)
		{
			if (low == high)
				return false;

			if (Start == low)
				return false;

			if (low < Start && low > End)
				return false;

			if (high < Start && high > End)
				return false;

			if (low >= high)
				return false;

			if (defPositions.ContainsKey(low))
				return false;

			if (defPositions.ContainsKey(high))
				return false;

			return true;
		}

		public List<LiveRange> SplitAt(SlotIndex low, SlotIndex high)
		{
			Debug.Assert(CanSplitAt(low, high));

			var ranges = new List<LiveRange>(3);

			ranges.Add(new LiveRange(Start, low, UsePositions, DefPositions));
			ranges.Add(new LiveRange(low, high, UsePositions, DefPositions));
			ranges.Add(new LiveRange(high, End, UsePositions, DefPositions));

			return ranges;
		}
	}
}