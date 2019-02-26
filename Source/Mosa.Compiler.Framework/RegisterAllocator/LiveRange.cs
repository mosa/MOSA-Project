// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class LiveRange : SlotInterval
	{
		// FUTURE:
		//		Replace use/def individual list used below
		//		with a single list from the VirutalRegistor instance bounded start/end boundaries
		// RATIONAL:
		//		1. No more list creation
		//		2. Smaller memory working set
		//		3. Avoid the use of interfaces

		private readonly SortedList<SlotIndex, SlotIndex> usePositions = new SortedList<SlotIndex, SlotIndex>();

		private readonly SortedList<SlotIndex, SlotIndex> defPositions = new SortedList<SlotIndex, SlotIndex>();

		public IList<SlotIndex> UsePositions { get { return usePositions.Keys; } }

		public IList<SlotIndex> DefPositions { get { return defPositions.Keys; } }

		public bool IsEmpty { get { return usePositions.Count == 0 && defPositions.Count == 0; } }

		public SlotIndex Minimum { get; }

		public SlotIndex Maximum { get; }

		public SlotIndex FirstUse { get { return usePositions.Count == 0 ? SlotIndex.NullSlot : usePositions.Values[0]; } }

		public SlotIndex FirstDef { get { return defPositions.Count == 0 ? SlotIndex.NullSlot : defPositions.Values[0]; } }

		public SlotIndex LastUse { get { return usePositions.Count == 0 ? SlotIndex.NullSlot : usePositions.Values[usePositions.Count - 1]; } }

		public SlotIndex LastDef { get { return defPositions.Count == 0 ? SlotIndex.NullSlot : defPositions.Values[defPositions.Count - 1]; } }

		public int UseCount { get { return usePositions.Count; } }

		public int DefCount { get { return defPositions.Count; } }

		public bool IsDefFirst { get; }

		public LiveRange(SlotIndex start, SlotIndex end, IList<SlotIndex> uses, IList<SlotIndex> defs)
			: base(start, end)
		{
			// live intervals can not start/end at the same location
			Debug.Assert(start != end);

			var max = SlotIndex.NullSlot;
			var min = SlotIndex.NullSlot;

			foreach (var use in uses)
			{
				if (use != Start && (End == use || Contains(use)))
				{
					usePositions.Add(use, use);

					if (max == SlotIndex.NullSlot || use > max)
						max = use;
					if (min == SlotIndex.NullSlot || use < min)
						min = use;
				}
			}

			foreach (var def in defs)
			{
				if (Contains(def))
				{
					defPositions.Add(def, def);

					if (max == SlotIndex.NullSlot || def > max)
						max = def;
					if (min == SlotIndex.NullSlot || def < min)
						min = def;
				}
			}

			Minimum = min;
			Maximum = max;

			if (FirstDef.IsNull)
				IsDefFirst = false;
			else if (FirstUse.IsNull)
				IsDefFirst = true;
			else if (FirstDef < FirstUse)
				IsDefFirst = true;
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

			return SlotIndex.NullSlot;
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

			return SlotIndex.NullSlot;
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
			if (at <= Start || at >= End)
				return false;

			if (usePositions.ContainsKey(at))
				return false;

			return true;
		}

		public List<LiveRange> SplitAt(SlotIndex at)
		{
			Debug.Assert(CanSplitAt(at));

			var ranges = new List<LiveRange>(2)
			{
				new LiveRange(Start, at, UsePositions, DefPositions),
				new LiveRange(at, End, UsePositions, DefPositions)
			};
			return ranges;
		}

		public bool CanSplitAt(SlotIndex low, SlotIndex high)
		{
			if (low <= Start || low >= End)
				return false;

			if (high <= Start || high >= End)
				return false;

			if (low >= high)
				return false;

			if (usePositions.ContainsKey(low))
				return false;

			if (usePositions.ContainsKey(high))
				return false;

			return true;
		}

		public List<LiveRange> SplitAt(SlotIndex low, SlotIndex high)
		{
			if (low == high)
			{
				return SplitAt(low);
			}

			Debug.Assert(CanSplitAt(low, high));

			var ranges = new List<LiveRange>(3)
			{
				new LiveRange(Start, low, UsePositions, DefPositions),
				new LiveRange(low, high, UsePositions, DefPositions),
				new LiveRange(high, End, UsePositions, DefPositions)
			};
			return ranges;
		}
	}
}
