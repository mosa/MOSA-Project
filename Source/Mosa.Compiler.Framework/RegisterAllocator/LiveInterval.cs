// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class LiveInterval
	{
		public enum AllocationStage
		{
			Initial = 0,
			SplitLevel1 = 1,
			SplitLevel2 = 2,
			SplitLevel3 = 3,
			SplitFinal = 4,
			Max = 5,
		}

		public int StartValue { get { return LiveRange.Start.Value; } }
		public int EndValue { get { return LiveRange.End.Value; } }

		public int Length { get { return LiveRange.Length; } }

		public LiveRange LiveRange { get; }

		public VirtualRegister VirtualRegister { get; }

		public int SpillValue { get; set; }

		public int SpillCost { get { return NeverSpill || TooSmallToSplit ? int.MaxValue : (SpillValue / (LiveRange.Length + 1)); } }

		public LiveIntervalTrack LiveIntervalTrack { get; set; }

		public AllocationStage Stage { get; set; }

		public bool IsPhysicalRegister { get { return VirtualRegister.IsPhysicalRegister; } }

		public PhysicalRegister AssignedPhysicalRegister { get { return LiveIntervalTrack?.Register; } }

		public Operand AssignedPhysicalOperand { get; set; }

		public Operand AssignedOperand { get { return (AssignedPhysicalRegister != null) ? AssignedPhysicalOperand : VirtualRegister.SpillSlotOperand; } }

		public bool ForceSpilled { get; set; }

		public bool NeverSpill { get; set; }

		public bool TooSmallToSplit { get; }

		#region Short Cuts

		public SlotIndex StartSlot { get { return LiveRange.Start; } }

		public SlotIndex EndSlot { get { return LiveRange.End; } }

		public IList<SlotIndex> UsePositions { get { return LiveRange.UsePositions; } }

		public IList<SlotIndex> DefPositions { get { return LiveRange.DefPositions; } }

		//public int Length { get { return LiveRange.Length; } }

		public bool IsEmpty { get { return LiveRange.IsEmpty; } }

		public SlotIndex Minimum { get { return LiveRange.Minimum; } }

		public SlotIndex Maximum { get { return LiveRange.Maximum; } }

		public bool IsAdjacent(SlotIndex start, SlotIndex end)
		{
			return LiveRange.IsAdjacent(start, end);
		}

		public bool Intersects(SlotIndex start, SlotIndex end)
		{
			return LiveRange.Intersects(start, end);
		}

		public bool Contains(SlotIndex start)
		{
			return LiveRange.Contains(start);
		}

		public bool IsAdjacent(LiveInterval other)
		{
			return LiveRange.IsAdjacent(other.LiveRange.Start, other.LiveRange.End);
		}

		public bool Intersects(LiveInterval other)
		{
			return LiveRange.Intersects(other.LiveRange.Start, other.LiveRange.End);
		}

		#endregion Short Cuts

		private LiveInterval(VirtualRegister virtualRegister, SlotIndex start, SlotIndex end, IList<SlotIndex> uses, IList<SlotIndex> defs)
		{
			LiveRange = new LiveRange(start, end, uses, defs);

			VirtualRegister = virtualRegister;
			SpillValue = 0;
			Stage = AllocationStage.Initial;
			ForceSpilled = false;
			NeverSpill = false;

			TooSmallToSplit = IsTooSmallToSplit();
		}

		public LiveInterval(VirtualRegister virtualRegister, SlotIndex start, SlotIndex end)
			: this(virtualRegister, start, end, virtualRegister.UsePositions, virtualRegister.DefPositions)
		{
		}

		public LiveInterval CreateExpandedLiveInterval(LiveInterval interval)
		{
			Debug.Assert(VirtualRegister == interval.VirtualRegister);

			var start = StartSlot < interval.StartSlot ? StartSlot : interval.StartSlot;
			var end = EndSlot > interval.EndSlot ? EndSlot : interval.EndSlot;

			return new LiveInterval(VirtualRegister, start, end);
		}

		public LiveInterval CreateExpandedLiveRange(SlotIndex start, SlotIndex end)
		{
			var mergedStart = StartSlot < start ? StartSlot : start;
			var mergedEnd = EndSlot > end ? EndSlot : end;

			return new LiveInterval(VirtualRegister, mergedStart, mergedEnd);
		}

		private bool IsTooSmallToSplit()
		{
			if (LiveRange.UseCount == 1)
			{
				var firstUse = LiveRange.FirstUse;

				Debug.Assert(firstUse.IsNotNull);

				if (firstUse.Before == StartSlot && firstUse.After == EndSlot)
					return true;
			}

			return false;
		}

		public override string ToString()
		{
			return $"{VirtualRegister} between {LiveRange}";
		}

		public void Evict()
		{
			LiveIntervalTrack.Evict(this);
		}

		private LiveInterval CreateSplit(LiveRange liveRange)
		{
			return new LiveInterval(VirtualRegister, liveRange.Start, liveRange.End, LiveRange.UsePositions, LiveRange.DefPositions);
		}

		public List<LiveInterval> SplitAt(SlotIndex at)
		{
			var liveRanges = LiveRange.SplitAt(at);

			var intervals = new List<LiveInterval>(liveRanges.Count);

			foreach (var liveRange in liveRanges)
			{
				intervals.Add(CreateSplit(liveRange));
			}

			return intervals;
		}

		public List<LiveInterval> SplitAt(SlotIndex low, SlotIndex high)
		{
			var liveRanges = LiveRange.SplitAt(low, high);

			var intervals = new List<LiveInterval>(liveRanges.Count);

			foreach (var liveRange in liveRanges)
			{
				intervals.Add(CreateSplit(liveRange));
			}

			return intervals;
		}
	}
}
