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
	public class LiveInterval : Interval
	{
		public enum AllocationStage
		{
			Initial = 0,
			SplitLevel1 = 1,
			SplitLevel2 = 2,
			Spillable = 3,
			Max = 4,
		}

		private SortedList<SlotIndex, SlotIndex> usePositions = new SortedList<SlotIndex, SlotIndex>();

		private SortedList<SlotIndex, SlotIndex> defPositions = new SortedList<SlotIndex, SlotIndex>();

		public VirtualRegister VirtualRegister { get; private set; }

		public int SpillValue { get; set; }

		public int SpillCost { get { return SpillValue / (Length + 1); } }

		public LiveIntervalUnion LiveIntervalUnion { get; set; }

		public AllocationStage Stage { get; set; }

		public IList<SlotIndex> UsePositions { get { return usePositions.Keys; } }

		public IList<SlotIndex> DefPositions { get { return defPositions.Keys; } }

		public bool IsEmpty { get { return usePositions.Count == 0 && defPositions.Count == 0; } }

		public bool IsPhysicalRegister { get { return VirtualRegister.IsPhysicalRegister; } }

		//public bool IsAssignedSpillSlot { get { return AssignedPhysicalRegister == null; } }

		public Register AssignedPhysicalRegister { get { return LiveIntervalUnion == null ? null : LiveIntervalUnion.Register; } }

		public bool ForceSpilled { get; set; }

		public SlotIndex Minimum { get; private set; }

		public SlotIndex Maximum { get; private set; }

		private LiveInterval(VirtualRegister virtualRegister, SlotIndex start, SlotIndex end, IList<SlotIndex> uses, IList<SlotIndex> defs)
			: base(start, end)
		{
			this.VirtualRegister = virtualRegister;
			this.SpillValue = 0;
			this.Stage = AllocationStage.Initial;
			this.ForceSpilled = false;

			SlotIndex max = null;
			SlotIndex min = null;

			foreach (var use in uses)
			{
				if (Contains(use))
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

		public LiveInterval(VirtualRegister virtualRegister, SlotIndex start, SlotIndex end)
			: this(virtualRegister, start, end, virtualRegister.UsePositions, virtualRegister.DefPositions)
		{
		}

		public LiveInterval CreateExpandedLiveInterval(LiveInterval interval)
		{
			Debug.Assert(VirtualRegister == interval.VirtualRegister);

			var start = Start < interval.Start ? Start : interval.Start;
			var end = End > interval.End ? End : interval.End;

			return new LiveInterval(VirtualRegister, start, end);
		}

		public LiveInterval CreateExpandedLiveRange(SlotIndex start, SlotIndex end)
		{
			var mergedStart = Start < start ? Start : start;
			var mergedEnd = End > end ? End : end;

			return new LiveInterval(this.VirtualRegister, mergedStart, mergedEnd);
		}

		public override string ToString()
		{
			return VirtualRegister.ToString() + " between " + base.ToString();
		}

		public void Evict()
		{
			this.LiveIntervalUnion.Evict(this);
		}

		public LiveInterval Split(SlotIndex start, SlotIndex end)
		{
			return new LiveInterval(VirtualRegister, start, end, usePositions.Keys, defPositions.Keys);
		}

		private SlotIndex GetNext(IList<SlotIndex> slots, SlotIndex start)
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

		private SlotIndex GetLowestNotNull(SlotIndex a, SlotIndex b)
		{
			if (a == null)
				return b;
			if (b == null)
				return a;
			return (a < b) ? a : b;
		}

		private SlotIndex GetHighestNotNull(SlotIndex a, SlotIndex b)
		{
			if (a == null)
				return b;
			if (b == null)
				return a;
			return (a > b) ? a : b;
		}

		public SlotIndex GetNextDefOrUsePosition(SlotIndex at)
		{
			return GetLowestNotNull(GetNext(UsePositions, at), GetNext(DefPositions, at));
		}

		public SlotIndex GetPreviousDefOrUsePosition(SlotIndex at)
		{
			return GetHighestNotNull(GetPrevious(UsePositions, at), GetPrevious(DefPositions, at));
		}
	}
}